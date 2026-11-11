using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IUtilityControl
{
    public Task<BaseResponse> CreateUtility(GetAuthControlInfoDto getAuthControlInfoDto, CreateUtilityDto createUtilityDto);
    public Task<BaseResponse> UpdateUtility(GetAuthControlInfoDto getAuthControlInfoDto, UpdateUtilityDto updateUtilityDto);
    public Task<UtilityResponseModel> GetUtilityById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<UtilitiesResponseModel> GetAllUtilities(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteUtility(GetAuthControlInfoDto getAuthControlInfoDto, int applianceId);
}
