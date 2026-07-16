using Home_Security.Entities;
namespace Home_Security.Streaming;

public interface ICameraStreamingService
{
    Task StartProcessingCameras(CancellationToken stoppingToken);
    Task ProcessCamera(Camera camera, CancellationToken token);
    Task<bool> CheckStreamAsync(string streamUrl);
    Task<Camera> StartStream(Camera camera);
    Task<Camera> RestartStream(Camera camera);
    Task StopStream(Camera camera);
    bool IsProcessAlive(int cameraId);
}