using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Implementations.Services;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls;
public class ActionControl : IActionControl
{
    IAuthControl _authControl;
    IActionService _actionService;
    ILogService _logService;
    public ActionControl(IAuthControl authControl, IActionService actionService, ILogService logService)
    {
        _authControl = authControl;
        _actionService = actionService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateAction(GetAuthControlInfoDto getAuthControlInfoDto, CreateActionDto createActionDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            
            var action = await _actionService.CreateAction(createActionDto, auth.Id);
            if (action.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = "An Action was newly added. Awaiting Physical Interfacing!";
                await _logService.CreateLog(log);
            }
            return action;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateAction(GetAuthControlInfoDto getAuthControlInfoDto, UpdateActionDto updateActionDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var action = await _actionService.UpdateAction(updateActionDto, auth.Id);
            if (action.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateActionDto.ActionName} Action was recently updated!";
                await _logService.CreateLog(log);
            }
            
            return action;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<ActionResponseModel> GetActionById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var action = await _actionService.GetById(id);
            action.Data = DtoRefactor(action.Data);
            return action;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ActionResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ActionResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ActionsResponseModel> GetAllActionsBySectionId(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var action = await _actionService.GetAllActionsBySectionId(sectionId);
            action.Data = action.Data.Select(x => DtoRefactor(x)).ToList();
            return action;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ActionsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ActionsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ActionsResponseModel> GetAllActionsByRoomId(GetAuthControlInfoDto getAuthControlInfoDto, int roomId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var action = await _actionService.GetAllActionsBySectionId(roomId);
            action.Data = action.Data.Select(x => DtoRefactor(x)).ToList();
            return action;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ActionsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ActionsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<ActionsResponseModel> GetAllActions(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var action = await _actionService.GetAllActions();
            action.Data = action.Data.Select(x => DtoRefactor(x)).ToList();
            return action;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            return new ActionsResponseModel()
            {
                Data = null,
                Status = _authControl.AuthFaliure().Status,
                Message = "Unauthorized Action"
            };
        }
        return new ActionsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteAction(GetAuthControlInfoDto getAuthControlInfoDto, int actionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var action = await _actionService.Delete(actionId, auth.Id);
            if (action.Status == true)
            {
                var getAction = _authControl.GetActionName(actionId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getAction} Action was Deleted!";
                await _logService.CreateLog(log);
            }
            return action;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetActionDto DtoRefactor(GetActionDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        x.RoomName = _authControl.GetRoomName(x.RoomId);
        x.SectionName = _authControl.GetSectionName(x.SectionId);
        return x;
    }
}