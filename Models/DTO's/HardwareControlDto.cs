namespace Home_Security.Models.DTOs;
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