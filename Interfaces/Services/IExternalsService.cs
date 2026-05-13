using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IExternalsService
{
    public Task<BaseResponse> CreateExternals(CreateLightDto createLightDto, int personId);
    public Task<BaseResponse> UpdateExternals(UpdateLightDto updateLightDto, int personId);
    public Task<LightResponseModel> GetById(int lightId);
    public Task<LightsResponseModel> GetAllExternals();
    public Task<LightsResponseModel> GetAllExternalsByRoomId(int roomId);
    public Task<LightsResponseModel> GetAllExternalsBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int lightId, int personId);
}