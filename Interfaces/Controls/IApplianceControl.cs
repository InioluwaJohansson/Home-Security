using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IApplianceControl
{
    public Task<BaseResponse> CreateAppliance(GetAuthControlInfoDto getAuthControlInfoDto, CreateApplianceDto createApplianceDto);
    public Task<BaseResponse> UpdateAppliance(GetAuthControlInfoDto getAuthControlInfoDto, UpdateApplianceDto updateApplianceDto);
    public Task<ApplianceResponseModel> GetApplianceById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<AppliancesResponseModel> GetAllAppliancesBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
    public Task<AppliancesResponseModel> GetAllAppliancesByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId);
    public Task<AppliancesResponseModel> GetAllAppliances(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteAppliance(GetAuthControlInfoDto getAuthControlInfoDto, int applianceId);
}
