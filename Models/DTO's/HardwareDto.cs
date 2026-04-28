namespace Home_Security.Models.DTOs;
public class CreateHardwareDto
{
    public string HardwareName { get; set; }
}
public class UpdateHardwareDto
{
    public int Id { get; set; }
    public string HardwareName { get; set; }
    public List<int> LightIds { get; set; }
    public List<int> ApplianceIds { get; set; }
    public List<int> CameraIds { get; set; }
    public List<int> WindowIds { get; set; }
    public List<int> DoorIds { get; set; }
}
public class GetHardwareDto
{
    public int Id { get; set; }
    public string HardwareName { get; set; }
    public string HardwareId { get; set; }
    public List<ApplianceIdName> ApplianceIdNames { get; set; }
    public List<CameraIdName> CameraIdNames { get; set; }
    public List<LightIdNames> LightIdNames { get; set; }
    public List<WindowIdName> WindowIdNames { get; set; }
    public List<DoorIdName> DoorIdNames { get; set; }
}
public class GetHardwareIdsDto
{
    public int Id { get; set; }
    public List<int> ApplianceIds { get; set; }
    public List<int> CameraIds { get; set; }
    public List<int> LightIds { get; set; }
    public List<int> WindowIds { get; set; }
    public List<int> DoorIds { get; set; }
}
public class GenericIdNames
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class LightIdName : GenericIdNames{}
public class CameraIdName : GenericIdNames{}
public class ApplianceIdName : GenericIdNames{}
public class WindowIdName : GenericIdNames{}
public class DoorIdName : GenericIdNames{}
public class HardwareResponseModel : BaseResponse
{
    public GetHardwareDto Data { get; set; }
}
public class HardwaresResponseModel : BaseResponse
{
    public ICollection<GetHardwareDto> Data { get; set; } = new HashSet<GetHardwareDto>();
}

public class UpdateHardwareResponseIds
{
    public List<int> cameraIds { get; set; }
    public List<int> doorIds { get; set; }
    public List<int> lightIds { get; set; }
    public List<int> windowIds { get; set; }
}
public class HardwareResponse : BaseResponse
{
    public List<GetCameraDto> getCameraDtos { get; set; }
    public List<GetDoorDto> getDoorDtos { get; set; }
    public List<GetLightDto> getLightDtos { get; set; }
    public List<GetWindowDto> getWindowDtos { get; set; }
}
public class UpdateHardwareDto
{
    public List<UpdateCameraDto> updateCameraDtos { get; set; }
    public List<UpdateDoorDto> updateDoorDtos { get; set; }
    public List<UpdateLightDto> updateLightDtos { get; set; }
    public List<UpdateWindowDto> updateWindowDtos { get; set; }

}