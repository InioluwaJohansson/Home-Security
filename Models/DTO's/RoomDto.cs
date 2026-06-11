using Home_Security.Entities;
namespace Home_Security.Models.DTOs;
public class CreateRoomDto
{
    public string RoomName { get; set; }
    public string Icon { get; set; }
    public int? PersonId { get; set; }
    public int? SectionId { get; set; }
    public bool IsHidden { get; set; }
}
public class UpdateRoomDto
{
    public int Id { get; set; }
    public string RoomName { get; set; }
    public string Icon { get; set; }
    public int? PersonId { get; set; }
    public int? SectionId { get; set; }
    public bool IsHidden { get; set; }
}
public class GetRoomDto : UserBaseDefaultDto
{
    public int Id { get; set; }
    public string RoomName { get; set; }
    public string RoomId { get; set; }
    public string Icon { get; set; }
    public int? SectionId { get; set; }
    public bool IsHidden { get; set; }
    public List<GetDoorDto> Doors { get; set; }
    public List<GetLightDto> Lights { get; set; }
    public List<GetWindowDto> Windows { get; set; }
    public List<GetApplianceDto> Appliances { get; set; }
    public List<GetCameraDto> Cameras { get; set; }
    public List<GetExternalDto> Externals { get; set; }
}
public class RoomResponseModel : BaseResponse
{
    public GetRoomDto Data { get; set; }
}
public class RoomsResponseModel : BaseResponse
{
    public ICollection<GetRoomDto> Data { get; set; } = new HashSet<GetRoomDto>();
}
