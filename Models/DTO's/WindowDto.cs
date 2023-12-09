using Home_Security.Entities;
namespace Home_Security.Models.DTOs;
public class CreateWindowDto : CreateBaseDefaultDto
{
    public string WindowName { get; set; }
    public string WindowId { get; set; }
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class UpdateWindowDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string WindowName { get; set; }
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class GetWindowDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string WindowName { get; set; }
    public string WindowId { get; set; }
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class WindowResponseModel : BaseResponse
{
    public GetWindowDto Data { get; set; }
}
public class WindowsResponseModel : BaseResponse
{
    public ICollection<GetWindowDto> Data { get; set; } = new HashSet<GetWindowDto>();
}