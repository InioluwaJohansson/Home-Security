namespace Home_Security.Models.DTOs;
public class CreateLightDto : CreateBaseDefaultDto
{
    public string LightName { get; set; }
    public int BrightnessLevel { get; set; }
}
public class UpdateLightDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string LightName { get; set; }
    public int BrightnessLevel { get; set; }
}
public class GetLightDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string LightName { get; set; }
    public string LightId { get; set; }
    public int BrightnessLevel { get; set; }
}
public class LightResponseModel : BaseResponse
{
    public GetLightDto Data { get; set; }
}
public class LightsResponseModel : BaseResponse
{
    public ICollection<GetLightDto> Data { get; set; } = new HashSet<GetLightDto>();
}