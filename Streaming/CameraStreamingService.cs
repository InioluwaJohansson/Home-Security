using System.Diagnostics;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Home_Security.Streaming;

public class CameraStreamingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly Dictionary<int, Process> _streamProcesses = new();

    private const int MaxConcurrentStarts = 5;
    private readonly SemaphoreSlim _streamLock = new(MaxConcurrentStarts);

    public CameraStreamingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var cameraRepo = scope.ServiceProvider.GetRequiredService<ICameraRepo>();

            var cameras = await cameraRepo.GetByExpression(x => x.IsDeleted == false);

            var tasks = cameras.Select(camera => ProcessCamera(camera, cameraRepo, stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessCamera(Camera camera, ICameraRepo cameraRepo, CancellationToken token)
    {
        try
        {
            bool online = await CheckStreamAsync(camera.InputStreamUrl);

            camera.IsOnline = online;

            if (!online)
            {
                StopStreaming(camera);
                camera.IsStreaming = false;

                await cameraRepo.Update(camera);
                return;
            }

            if (!IsProcessAlive(camera.Id))
            {
                await _streamLock.WaitAsync(token);

                try
                {
                    StartStreaming(camera);

                    camera.IsStreaming = true;
                    camera.LastStreamStart = DateTime.UtcNow;
                }
                finally
                {
                    _streamLock.Release();
                }
            }

            await cameraRepo.Update(camera);
        }
        catch
        {
            camera.IsStreaming = false;
            await cameraRepo.Update(camera);
        }
    }
    private async Task<bool> CheckStreamAsync(string streamUrl)
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

    private void StartStreaming(Camera camera)
    {
        Directory.CreateDirectory($"storage/hls/cam_{camera.CameraId}");
        Directory.CreateDirectory($"storage/recordings/cam_{camera.CameraId}");

        string hlsOutput =
            $"storage/hls/cam_{camera.CameraId}/live.m3u8";
        string recordingOutput =
            $"storage/recordings/cam_{camera.CameraId}/cam_%Y-%m-%d_%H-%M-%S.mp4";

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

        camera.LiveStreamUrl =
            $"http://localhost:5000/storage/hls/cam_{camera.CameraId}/live.m3u8";
    }

    private void StopStreaming(Camera camera)
    {
        if (_streamProcesses.TryGetValue(camera.Id, out var process))
        {
            try
            {
                if (!process.HasExited)
                    process.Kill(true);

                process.Dispose();
            }
            catch { }

            _streamProcesses.Remove(camera.Id);
        }
    }

    private bool IsProcessAlive(int cameraId)
    {
        return _streamProcesses.ContainsKey(cameraId) &&
               !_streamProcesses[cameraId].HasExited;
    }
}