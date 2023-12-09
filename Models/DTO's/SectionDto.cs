using Home_Security.Entities;
namespace Home_Security.Models.DTOs;
public class CreateSectionDto
{   
    public string SectionName { get; set; }
}
public class UpdateSectionDto
{
    public int Id { get; set; }
    public string SectionName { get; set; }
}
public class GetSectionDto
{
    public int Id { get; set; }
    public string SectionName { get; set; }
    public string SectionId { get; set; }
    public List<GetRoomDto> Room { get; set; }
    public List<GetDoorDto> Door { get; set; }
    public List<GetLightDto> Light { get; set; }
    public List<GetWindowDto> Window { get; set; }
    public List<GetApplianceDto> Appliance { get; set; }
}
public class SectionResponseModel : BaseResponse
{
    public GetSectionDto Data { get; set; }
}
public class SectionsResponseModel : BaseResponse
{
    public ICollection<GetSectionDto> Data { get; set; } = new HashSet<GetSectionDto>();
}