using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ILightService
{
    public Task<BaseResponse> CreateLight(CreateLightDto createLightDto);
    public Task<BaseResponse> UpdateLight(UpdateLightDto updateLightDto);
    public Task<LightResponseModel> GetById(int lightId);
    public Task<LightsResponseModel> GetAllLights();
    public Task<LightsResponseModel> GetAllLightsByRoomId(int roomId);
    public Task<LightsResponseModel> GetAllLightsBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int lightId, int personId);
}