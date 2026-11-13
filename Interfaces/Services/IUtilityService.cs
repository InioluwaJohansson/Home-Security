using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Interfaces.Services;
public interface IUtilityService
{
    public Task<BaseResponse> CreateUtility(CreateUtilityDto createUtilityDto, int personId);
    public Task<BaseResponse> UpdateUtility(UpdateUtilityDto updateUtilityDto, int personId);
    public Task<UtilityResponseModel> GetById(int utilityId);
    public Task<UtilitiesResponseModel> GetAllUtilities();
    public Task<UtilitiesResponseModel> GetAllUtilitiesByUtilityType(UtilityType utilityType);
    public Task<BaseResponse> Delete(int utilityId, int personId);
}