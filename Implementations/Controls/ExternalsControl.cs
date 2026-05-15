using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class ExternalControl : IExternalControl
{
    IAuthControl _authControl;
    IExternalService _externalService;
    ILogService _logService;
    public ExternalControl(IAuthControl authControl, IExternalService externalService, ILogService logService)
    {
        _authControl = authControl;
        _externalService = externalService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateExternal(GetAuthControlInfoDto getAuthControlInfoDto, CreateExternalDto createExternalDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var external = await _externalService.CreateExternal(createExternalDto, auth.Id);
            if (external.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "A External was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return external;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateExternal(GetAuthControlInfoDto getAuthControlInfoDto, UpdateExternalDto updateExternalDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var external = await _externalService.UpdateExternal(updateExternalDto, auth.Id);
            if (external.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateExternalDto.ExternalName} External was recently updated!";
                await _logService.CreateLog(log);
            }
            return external;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<ExternalResponseModel> GetExternalById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var external = await _externalService.GetById(id);
            external.Data = DtoRefactor(external.Data);
            return external;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ExternalResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ExternalResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ExternalsResponseModel> GetAllExternalsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var external = await _externalService.GetAllExternalsBySectionId(sectionId);
            external.Data = external.Data.Select(x => DtoRefactor(x)).ToList();
            return external;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ExternalsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ExternalsResponseModel> GetAllExternalsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var external = await _externalService.GetAllExternalsByRoomId(roomId);
            external.Data = external.Data.Select(x => DtoRefactor(x)).ToList();
            return external;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ExternalsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ExternalsResponseModel> GetAllExternals(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var external = await _externalService.GetAllExternals();
            external.Data = external.Data.Select(x => DtoRefactor(x)).ToList();
            return external;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ExternalsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ExternalsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteExternal(GetAuthControlInfoDto getAuthControlInfoDto, int externalId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var external = await _externalService.Delete(externalId, auth.Id);
            if (external.Status == true)
            {
                var getExternal = _authControl.GetExternalName(externalId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getExternal} External was Deleted!";
                await _logService.CreateLog(log);
            }
            return external;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetExternalDto DtoRefactor(GetExternalDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId);
        x.SectionName = _authControl.GetSectionName(x.SectionId);
        return x;
    }
}