namespace Home_Security.DTOs;

public class GetRecordingDto
{
    public string FilePath { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}