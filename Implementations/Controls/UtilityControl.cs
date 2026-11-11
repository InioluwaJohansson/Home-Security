using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class UtilityControl : IUtilityControl
{
    IAuthControl _authControl;
    IUtilityService _utilityService;
    ILogService _logService;
    public UtilityControl(IAuthControl authControl, IUtilityService utilityService, ILogService logService)
    {
        _authControl = authControl;
        _utilityService = utilityService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateUtility(GetAuthControlInfoDto getAuthControlInfoDto, CreateUtilityDto createUtilityDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var utility = await _utilityService.CreateUtility(createUtilityDto, auth.Id);
            if (utility.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = $"{createUtilityDto.UtilityName} Utility was newly added. Awaiting Physical Interfacing!";
                log.FacilityType = FacilityTypeApp.Utility;
                await _logService.CreateLog(log);
            }
            return utility;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateUtility(GetAuthControlInfoDto getAuthControlInfoDto, UpdateUtilityDto updateUtilityDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var utility = await _utilityService.UpdateUtility(updateUtilityDto, auth.Id);
            if (utility.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateUtilityDto.UtilityName} Utility was recently updated!";
                log.FacilityType = FacilityTypeApp.Utility;
                log.FacilityId = updateUtilityDto.Id;
                await _logService.CreateLog(log);
            }
            
            return utility;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<UtilityResponseModel> GetUtilityById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var utility = await _utilityService.GetById(id);
            utility.Data = DtoRefactor(utility.Data);
            return utility;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new UtilityResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new UtilityResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<UtilitiesResponseModel> GetAllUtilities(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var utility = await _utilityService.GetAllUtilities();
            utility.Data = utility.Data.Select(x => DtoRefactor(x)).ToList();
            return utility;
        }
        return new UtilitiesResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteUtility(GetAuthControlInfoDto getAuthControlInfoDto, int utilityId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var utility = await _utilityService.Delete(utilityId, auth.Id);
            if (utility.Status == true)
            {
                var getUtility = _authControl.GetUtilityName(utilityId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getUtility} Utility was Deleted!";
                log.FacilityType = FacilityTypeApp.Utility;
                log.FacilityId = utilityId;
                await _logService.CreateLog(log);
            }
            return utility;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetUtilityDto DtoRefactor(GetUtilityDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId ?? 0);
        x.SectionName = _authControl.GetSectionName(x.SectionId ?? 0);
        return x;
    }
}