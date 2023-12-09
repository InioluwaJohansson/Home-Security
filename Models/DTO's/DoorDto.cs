using Home_Security.Models.Enums;
namespace Home_Security.Models.DTOs;
public class CreateDoorDto : CreateBaseDefaultDto
{
    public string DoorName { get; set; }
    public DoorType DoorType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class UpdateDoorDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string DoorName { get; set; }
    public DoorType DoorType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class GetDoorDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string DoorName { get; set; }
    public string DoorId { get; set; }
    public DoorType DoorType { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public int OpenedBy { get; set; }
    public int LockedBy { get; set; }
    public int UnlockedBy { get; set; }
}
public class DoorResponseModel : BaseResponse
{
    public GetDoorDto Data { get; set; }
}
public class DoorsResponseModel : BaseResponse
{
    public ICollection<GetDoorDto> Data { get; set; } = new HashSet<GetDoorDto>();
}