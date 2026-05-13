public class Recording : BaseDefault
{
    public int CameraId { get; set; }
    public Camera Camera { get; set; }

    // MP4 file path
    public string FilePath { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    // File size in bytes
    public long FileSize { get; set; }
}