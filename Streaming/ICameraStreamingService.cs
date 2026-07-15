using Home_Security.Entities;
namespace Home_Security.Streaming;

public interface ICameraStreamingService
{
    Task StartProcessingCameras(CancellationToken stoppingToken);
    Task ProcessCamera(Camera camera, CancellationToken token);
    Task<bool> CheckStreamAsync(string streamUrl);
    Task<string> StartStream(Camera camera);
    Task RestartStream(Camera camera);
    Task StopStream(Camera camera);
    bool IsProcessAlive(int cameraId);
}