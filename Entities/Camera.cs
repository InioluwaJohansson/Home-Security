namespace Home_Security.Entities;

public class Camera : BaseDefault
{
    public string CameraName { get; set; }
    public string CameraId { get; set; }

    public string IpAddress { get; set; }
    public string InputStreamUrl { get; set; }

    public string LiveStreamUrl { get; set; }
    public string RecordingPath { get; set; }

    public bool IsOnline { get; set; }
    public bool IsStreaming { get; set; }

    public DateTime? LastStreamStart { get; set; }
    public List<Recording> Recordings { get; set; }
}