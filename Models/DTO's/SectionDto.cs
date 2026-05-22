using Home_Security.Entities;
namespace Home_Security.Models.DTOs;
public class CreateSectionDto
{   
    public string SectionName { get; set; }
    public bool IsHidden { get; set; }
}
public class UpdateSectionDto
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public bool IsHidden {get; set; }
}
public class GetSectionDto : UserBaseDefaultDto
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public string SectionId { get; set; }
    public bool IsHidden {get; set; }
    public List<GetRoomDto> Rooms { get; set; }
    public List<GetDoorDto> Doors { get; set; }
    public List<GetLightDto> Lights { get; set; }
    public List<GetWindowDto> Windows { get; set; }
    public List<GetApplianceDto> Appliances { get; set; }
    public List<GetCameraDto> Cameras { get; set; }
    public List<GetExternalDto> Externals { get; set; }
}
public class SectionResponseModel : BaseResponse
{
    public GetSectionDto Data { get; set; }
}
public class SectionsResponseModel : BaseResponse
{
    public ICollection<GetSectionDto> Data { get; set; } = new HashSet<GetSectionDto>();
}
