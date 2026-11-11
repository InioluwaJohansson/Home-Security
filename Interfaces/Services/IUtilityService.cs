using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IUtilityService
{
    public Task<BaseResponse> CreateUtility(CreateUtilityDto createUtilityDto, int personId);
    public Task<BaseResponse> UpdateUtility(UpdateUtilityDto updateUtilityDto, int personId);
    public Task<UtilityResponseModel> GetById(int utilityId);
    public Task<UtilitiesResponseModel> GetAllUtilities();
    public Task<BaseResponse> Delete(int utilityId, int personId);
}