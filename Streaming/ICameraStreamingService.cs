using Home_Security.Entities;
namespace Home_Security.Streaming;

public interface ICameraStreamingService
{

    Task<string> StartStream(Camera camera);

    Task StopStream(Camera camera);

    Task RestartStream(Camera camera);

    bool IsStreaming(int cameraId);

    Task<bool> CheckStreamHealth(string streamUrl);
}