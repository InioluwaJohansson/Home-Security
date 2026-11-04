using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Calls;
using Home_Security.Interfaces.Services.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Implementations.Controls.Calls;
public class CallControl : ICallControl
{
    IAuthControl _authControl;
    ICallService _callService;
    public CallControl(IAuthControl authControl, ICallService callService)
    {
        _authControl = authControl;
        _callService = callService;
    }
    public async Task<CallDto> StartCall(GetAuthControlInfoDto getAuthControlInfoDto, StartCallDto dto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.StartCallAsync(dto, auth.Id);
            return call;
        }
        return null;
    }
    public async Task<BaseResponse> AcceptCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.AcceptCallAsync(callId, auth.Id);
            return call;
        }
        return null;
    }
    public async Task<BaseResponse> RejectCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.RejectCallAsync(callId, auth.Id);
            return call;
        }
        return null;
    }
    public async Task<CallDto> GetCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.GetCallAsync(callId, auth.Id);
            return call;
        }
        return null;
    }
    public async Task<BaseResponse> EndCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.EndCallAsync(callId, auth.Id);
            return call;
        }
        return null;
    }
    public async Task<List<CallLogDto>> GetCallLogsAsync(GetAuthControlInfoDto getAuthControlInfoDto, int page, int pageSize)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var call = await _callService.GetCallLogsAsync(auth.Id, page, pageSize);
            return call;
        }
        return null;
    }
}