using System.Diagnostics;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Home_Security.Streaming;
public class CameraStreamingService : BackgroundService, ICameraStreamingService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<int, Process> _streamProcesses = new();
    private const int MaxConcurrentStarts = 5;
    private readonly SemaphoreSlim _streamLock = new(MaxConcurrentStarts);
    ICameraRepo _cameraRepo;
    public CameraStreamingService(IServiceScopeFactory scopeFactory, ICameraRepo cameraRepo)
    {
        _scopeFactory = scopeFactory;
        _cameraRepo = cameraRepo;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var cameras = await _cameraRepo.GetByExpression(x => x.IsDeleted == false);
            var tasks = cameras.Select(camera => ProcessCamera(camera, stoppingToken));
            await Task.WhenAll(tasks);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
    public async Task ProcessCamera(Camera camera, CancellationToken token)
    {
        try
        {
            bool online = await CheckStreamAsync(camera.InputStreamUrl);
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
                    await StartStream(camera);
                    camera.IsStreaming = true;
                    camera.LastStreamStart = DateTime.UtcNow;
                }
                finally
                {
                    _streamLock.Release();
                }
            }
            await _cameraRepo.Update(camera);
        }
        catch
        {
            camera.IsStreaming = false;
            await _cameraRepo.Update(camera);
        }
    }
    public async Task RestartStream(Camera camera)
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
        await StartStream(camera);
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
    public async Task<string> StartStream(Camera camera)
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var cameraStoragePath = Path.Combine("storage", "Cameras");
        var hlsFolderPath = Path.Combine(cameraStoragePath, "hls", $"CAM_{camera.CameraId}");
        var recordingsFolderPath = Path.Combine(cameraStoragePath, "recordings", $"CAM_{camera.CameraId}");
        Directory.CreateDirectory(Path.Combine(basePath, hlsFolderPath));
        Directory.CreateDirectory(Path.Combine(basePath, recordingsFolderPath));
        string hlsOutput = $"{hlsFolderPath}/live.m3u8";
        string recordingOutput = $"{recordingsFolderPath}/cam_%Y-%m-%d_%H-%M-%S.mp4";
        string ffmpegArgs =
            $"-rtsp_transport tcp " +
            $"-i \"{camera.InputStreamUrl}\" " +
            $"-c:v libx264 -preset veryfast -tune zerolatency " +
            $"-f hls -hls_time 2 -hls_list_size 30 -hls_flags delete_segments " +
            $"\"{hlsOutput}\" " +
            $"-c:v copy -f segment -segment_time 300 -reset_timestamps 1 -strftime 1 " +
            $"\"{recordingOutput}\"";
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        _streamProcesses[camera.Id] = process;
        camera.LiveStreamUrl = $"{hlsFolderPath}/live.m3u8";
        return camera.LiveStreamUrl;
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