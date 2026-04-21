using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services;
public interface IApplianceService
{
    public Task<BaseResponse> CreateAppliance(CreateApplianceDto createApplianceDto, int personId);
    public Task<BaseResponse> UpdateAppliance(UpdateApplianceDto updateApplianceDto, int personId);
    public Task<ApplianceResponseModel> GetById(int applianceId);
    public Task<AppliancesResponseModel> GetAllAppliances();
    public Task<AppliancesResponseModel> GetAllAppliancesByRoomId(int roomId);
    public Task<AppliancesResponseModel> GetAllAppliancesBySectionId(int sectionId);
    public Task<BaseResponse> Delete(int applianceId, int personId);
}