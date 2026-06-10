using Home_Security.Models.DTOs;
namespace Home_Security.Models.DTOs;
public class CreateHardwareDto
{
    public string HardwareName { get; set; }
}
public class UpdateHardwareDto
{
    public int Id { get; set; }
    public string HardwareName { get; set; }
    public string AuthKey { get; set; }
    public List<int> LightIds { get; set; }
    public List<int> ApplianceIds { get; set; }
    public List<int> CameraIds { get; set; }
    public List<int> WindowIds { get; set; }
    public List<int> DoorIds { get; set; }
    public List<int> ExternalIds { get; set; }
}
public class GetHardwareDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HardwareId { get; set; }
    public string AuthKey { get; set; }
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public List<GenericIdNames> ApplianceIdNames { get; set; }
    public List<GenericIdNames> CameraIdNames { get; set; }
    public List<GenericIdNames> LightIdNames { get; set; }
    public List<GenericIdNames> WindowIdNames { get; set; }
    public List<GenericIdNames> DoorIdNames { get; set; }
    public List<GenericIdNames> ExternalIdNames { get; set; }
}
public class GetHardwareIdsDto
{
    public int Id { get; set; }
    public List<int> ApplianceIds { get; set; }
    public List<int> CameraIds { get; set; }
    public List<int> LightIds { get; set; }
    public List<int> WindowIds { get; set; }
    public List<int> DoorIds { get; set; }
    public List<int> ExternalIds { get; set; }
}
public class GenericIdNames
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class HardwareHeartBeatDto
{
    public string Token { get; set; }
    public string HardwareId { get; set; }
    public bool PowerActive { get; set; }
}
public class NFIDCodeDto
{
    public string Token { get; set; }
    public string NFIDCode { get; set; }
}
public class HardwareResponseModel : BaseResponse
{
    public GetHardwareDto Data { get; set; }
}
public class HardwaresResponseModel : BaseResponse
{
    public ICollection<GetHardwareDto> Data { get; set; } = new HashSet<GetHardwareDto>();
}
