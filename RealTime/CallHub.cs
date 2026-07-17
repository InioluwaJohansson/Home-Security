using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Hubs;
using Home_Security.Interfaces.Controls.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Authentication;
using Home_Security.Models.DTOs;

namespace Home_Security.RealTimeServices;
public class CallHub : ICallHub
{
    private readonly IHubContext<HomeSecurityHub> _hub;
    private readonly ICallControl _callControl;
    private readonly IJWTAuthentication _jwtAuthentication;
    public CallHub(IHubContext<HomeSecurityHub> hub, ICallControl callControl, IJWTAuthentication jwtAuthentication)
    {
        _hub = hub;
        _callControl = callControl;
        _jwtAuthentication = jwtAuthentication;
    }
    public async Task StartCall(StartCallDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
            var call = await _callControl.StartCall(getAuthControlInfoDto, dto);
            await _hub.Groups.AddToGroupAsync(Context.ConnectionId, dto.ChatId.ToString());
            await _hub.Clients.Group(dto.ChatId.ToString()).SendAsync("IncomingCall", call);
        }
    }
    public async Task AcceptCall(int callId,string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
            await _callControl.AcceptCall(getAuthControlInfoDto, callId);
            var call = await _callControl.GetCall(getAuthControlInfoDto, callId);
            await _hub.Clients.Group(call.ChatId.ToString()).SendAsync("CallAccepted", call);
        }
    }
    public async Task Offer(OfferDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
        var call = await _callControl.GetCall(getAuthControlInfoDto, dto.CallId);

        await _hub.Clients.Group(call.ChatId.ToString())
            .SendAsync("Offer", dto);
        }
    }
    public async Task Answer(AnswerDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
        var call = await _callControl.GetCall(getAuthControlInfoDto, dto.CallId);

        await _hub.Clients.Group(call.ChatId.ToString())
            .SendAsync("Answer", dto);
        }
    }
    public async Task IceCandidate(IceCandidateDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
            var call = await _callControl.GetCall(getAuthControlInfoDto, dto.CallId);

            await _hub.Clients.Group(call.ChatId.ToString())
                .SendAsync("IceCandidate", dto);
        }
    }
}