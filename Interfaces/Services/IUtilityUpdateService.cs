using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Interfaces.Services;
public interface IUtilityUpdateService
{
    public Task<BaseResponse> UpdateUtility(UpdateUtilityDto updateUtilityDto, int personId);
}