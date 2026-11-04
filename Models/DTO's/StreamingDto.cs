namespace Home_Security.Models.DTOs;
public class GetRecordingDto
{
    public int CameraId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}