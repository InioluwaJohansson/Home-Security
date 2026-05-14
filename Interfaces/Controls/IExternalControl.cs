using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Controls;
public interface IExternalControl
{
    public Task<BaseResponse> CreateExternal(GetAuthControlInfoDto getAuthControlInfoDto, CreateExternalDto createExternalDto);
    public Task<BaseResponse> UpdateExternal(GetAuthControlInfoDto getAuthControlInfoDto, UpdateExternalDto updateExternalDto);
    public Task<ExternalResponseModel> GetExternalById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<ExternalsResponseModel> GetAllExternalsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<ExternalsResponseModel> GetAllExternalsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
    public Task<ExternalsResponseModel> GetAllExternals(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteExternal(GetAuthControlInfoDto getAuthControlInfoDto, int lightId);
}
