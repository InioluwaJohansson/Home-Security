using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface ILightControl
{
    public Task<BaseResponse> CreateLight(GetAuthControlInfoDto getAuthControlInfoDto, CreateLightDto createLightDto);
    public Task<BaseResponse> UpdateLight(GetAuthControlInfoDto getAuthControlInfoDto, UpdateLightDto updateLightDto);
    public Task<LightResponseModel> GetLightById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<LightsResponseModel> GetAllLightsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<LightsResponseModel> GetAllLightsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
    public Task<LightsResponseModel> GetAllLights(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteLight(GetAuthControlInfoDto getAuthControlInfoDto, int lightId);
}
