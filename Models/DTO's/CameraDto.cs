namespace Home_Security.Models.DTOs;
public class CreateCameraDto : CreateBaseDefaultDto
{
    public string CameraName { get; set; }
    public string IpAddress { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string StreamPath { get; set; }  
    public int Port { get; set; } 
}
public class UpdateCameraDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string CameraName { get; set; }
    public bool IsActive { get; set; }
}
public class GetCameraDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string CameraName { get; set; }
    public string CameraId { get; set; }
    public string LiveStreamUrl { get; set; }
    public string IpAddress { get; set; }
    public string? Username { get; set; }
    public string StreamPath { get; set; }  
    public int Port { get; set; } 
    public List<GetRecordingDto> Recordings { get; set; }
}
public class CameraResponseModel : BaseResponse
{
    public GetCameraDto Data { get; set; }
}
public class CamerasResponseModel : BaseResponse
{
    public ICollection<GetCameraDto> Data { get; set; } = new HashSet<GetCameraDto>();
}