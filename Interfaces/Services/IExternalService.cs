using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IExternalService
{
    public Task<BaseResponse> CreateExternal(CreateExternalDto createExternalsDto, int personId);
    public Task<BaseResponse> UpdateExternal(UpdateExternalDto updateExternalsDto, int personId);
    public Task<ExternalResponseModel> GetById(int ExternalsId);
    public Task<ExternalsResponseModel> GetAllExternals();
    public Task<ExternalsResponseModel> GetAllExternalsByRoomId(int roomId);
    public Task<ExternalsResponseModel> GetAllExternalsBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int extensionId, int personId);
}