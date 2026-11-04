using System.Diagnostics;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Home_Security.Streaming;
public class CameraStreamingService : ICameraStreamingService
{
    private readonly Dictionary<int, Process> _streamProcesses = new();
    private const int MaxConcurrentStarts = 5;
    private readonly SemaphoreSlim _streamLock = new(MaxConcurrentStarts);
    IConfiguration _FFmpeg;
    ICameraRepo _cameraRepo;
    public CameraStreamingService(ICameraRepo cameraRepo, IConfiguration config)
    {
        _cameraRepo = cameraRepo;
        _FFmpeg = config.GetSection("FFMpeg");
    }
    public async Task StartProcessingCameras(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
            if(cameras != null)
            {
                foreach (var camera in cameras) await ProcessCamera(camera, stoppingToken);
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
    public async Task ProcessCamera(Camera camera, CancellationToken token)
    {
        try
        {
            bool online = await CheckStreamAsync(camera.InputStreamUrl);
            camera = await _cameraRepo.Get(x => x.Id == camera.Id);
            camera.IsOnline = online;
            if (!online)
            {
                await StopStream(camera);
                camera.IsStreaming = false;
                await _cameraRepo.Update(camera);
                return;
            }
            if (!IsProcessAlive(camera.Id))
            {
                await _streamLock.WaitAsync(token);
                try
                {
                    camera = await StartStream(camera);
                    camera.IsStreaming = true;
                    camera.LastStreamStart = DateTime.UtcNow;
                    await _cameraRepo.Update(camera);
                }
                finally
                {
                    _streamLock.Release();
                }
            }
        }
        catch
        {
            camera.IsStreaming = false;
            await _cameraRepo.Update(camera);
        }
    }
    public async Task<Camera> RestartStream(Camera camera)
    {
        if (_streamProcesses.TryGetValue(camera.Id, out var process))
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(true);
                    process.WaitForExit(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop stream for camera {camera.CameraId}: {ex.Message}");
            }
            finally
            {
                process.Dispose();
                _streamProcesses.Remove(camera.Id);
            }
        }
        return await StartStream(camera);
    }
    public async Task<bool> CheckStreamAsync(string streamUrl)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(5);
            var response = await httpClient.GetAsync(streamUrl);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    public async Task<Camera> StartStream(Camera camera)
    {
        if (_streamProcesses.TryGetValue(camera.Id, out var existingProcess))
        {
            if (!existingProcess.HasExited) return camera;
            _streamProcesses.Remove(camera.Id);
        }
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var hlsFolder = Path.Combine(basePath, "storage", "Cameras", "hls", $"CAM_{camera.CameraId}");
        var recordingsFolder = Path.Combine(basePath, "storage", "Cameras", "Recordings", $"CAM_{camera.CameraId}");
        Directory.CreateDirectory(hlsFolder);
        Directory.CreateDirectory(recordingsFolder);
        var hlsOutput = Path.Combine(hlsFolder, "live.m3u8");
        var recordingOutput = Path.Combine(recordingsFolder, "cam_%Y-%m-%d_%H-%M-%S.mp4");
        var ffmpegArgs =
            $"-rtsp_transport tcp " + $"-i \"{camera.InputStreamUrl}\" " +
            $"-map 0:v " +
            $"-c:v libx264 " +
            $"-preset veryfast " +
            $"-tune zerolatency " +
            $"-f hls " +
            $"-hls_time 2 " +
            $"-hls_list_size 30 " +
            $"-hls_flags delete_segments+append_list " +
            $"\"{hlsOutput}\" " +
            $"-map 0:v " + $"-c:v copy " + $"-f segment " +
            $"-segment_time {60 * Double.Parse(_FFmpeg["RecordingTimeMinutes"])} " + $"-reset_timestamps 1 " + $"-strftime 1 " + $"\"{recordingOutput}\"";
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _FFmpeg["Path"],
                Arguments = ffmpegArgs,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = basePath
            },
            EnableRaisingEvents = true
        };
        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine($"[FFmpeg][{camera.CameraId}] {e.Data}");
        };
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data)) Console.WriteLine($"[FFmpeg][{camera.CameraId}] {e.Data}");
        };
        process.Exited += (_, _) =>
        {
            Console.WriteLine($"FFmpeg exited for camera {camera.CameraId}");
            lock (_streamProcesses) _streamProcesses.Remove(camera.Id);
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        lock (_streamProcesses)  _streamProcesses[camera.Id] = process;
        camera.LiveStreamUrl = $"Cameras/hls/CAM_{camera.CameraId}/live.m3u8";
        camera.IsStreaming = true;
        camera.LastStreamStart = DateTime.UtcNow;
        await Task.Delay(1500);
        if (process.HasExited) throw new Exception($"Unable to start stream for camera {camera.CameraId}. Check the FFmpeg logs above.");
        return camera;
    }
    public async Task StopStream(Camera camera)
    {
        if (_streamProcesses.TryGetValue(camera.Id, out var process))
        {
            try
            {
                if (!process.HasExited) process.Kill(true);
                process.Dispose();
            }
            catch {}
            _streamProcesses.Remove(camera.Id);
        }
    }
    public bool IsProcessAlive(int cameraId)
    {
        return _streamProcesses.ContainsKey(cameraId) && !_streamProcesses[cameraId].HasExited;
    }
}