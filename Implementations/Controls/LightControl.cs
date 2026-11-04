using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class LightControl : ILightControl
{
    IAuthControl _authControl;
    ILightService _lightService;
    ILogService _logService;
    public LightControl(IAuthControl authControl, ILightService lightService, ILogService logService)
    {
        _authControl = authControl;
        _lightService = lightService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateLight(GetAuthControlInfoDto getAuthControlInfoDto, CreateLightDto createLightDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var light = await _lightService.CreateLight(createLightDto, auth.Id);
            if (light.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = $"{createLightDto.LightName} Light was newly added. Awaiting Physical Interfacing!";
                log.FacilityType = FacilityTypeApp.Light;
                await _logService.CreateLog(log);
            }
            return light;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateLight(GetAuthControlInfoDto getAuthControlInfoDto, UpdateLightDto updateLightDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var light = await _lightService.UpdateLight(updateLightDto, auth.Id);
            if (light.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateLightDto.LightName} Light was recently updated!";
                log.FacilityType = FacilityTypeApp.Light;
                log.FacilityId = updateLightDto.Id;
                await _logService.CreateLog(log);
            }
            return light;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<LightResponseModel> GetLightById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var light = await _lightService.GetById(id);
            light.Data = DtoRefactor(light.Data);
            if (light.Data.IsHidden && auth.Role == Role.Owner || auth.Role == Role.Wife)
                return light;
            else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
            {
                return new LightResponseModel()
                {
                    Data = null,
                    Status = _authControl.AuthFaliure().Status,
                    Message = "Unauthorized Action"
                };
            }
        }
        return new LightResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LightsResponseModel> GetAllLightsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var light = await _lightService.GetAllLightsBySectionId(sectionId);
            light.Data = light.Data.Select(x => DtoRefactor(x)).ToList();
            return light;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var light = await _lightService.GetAllLightsBySectionId(sectionId);
            light.Data = light.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return light;
        }
        return new LightsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LightsResponseModel> GetAllLightsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var light = await _lightService.GetAllLightsByRoomId(roomId);
            light.Data = light.Data.Select(x => DtoRefactor(x)).ToList();
            return light;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var light = await _lightService.GetAllLightsByRoomId(roomId);
            light.Data = light.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return light;
        }
        return new LightsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<LightsResponseModel> GetAllLights(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var light = await _lightService.GetAllLights();
            light.Data = light.Data.Select(x => DtoRefactor(x)).ToList();
            return light;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var light = await _lightService.GetAllLights();
            light.Data = light.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return light;
        }
        return new LightsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteLight(GetAuthControlInfoDto getAuthControlInfoDto, int lightId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var light = await _lightService.Delete(lightId, auth.Id);
            if (light.Status == true)
            {
                var getLight = _authControl.GetLightName(lightId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getLight} Light was Deleted!";
                log.FacilityType = FacilityTypeApp.Light;
                log.FacilityId = lightId;
                await _logService.CreateLog(log);
            }
            return light;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetLightDto DtoRefactor(GetLightDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId ?? 0);
        x.SectionName = _authControl.GetSectionName(x.SectionId ?? 0);
        return x;
    }
}