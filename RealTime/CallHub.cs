using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Home_Security.Interfaces.Controls.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Authentication;
using Home_Security.Models.DTOs;
using Home_Security.Interfaces.Controls;

namespace Home_Security.RealTimeServices;
public class CallHub : ICallHub
{
    private readonly IHubContext<HomeSecurityHub> _hub;
    private readonly ICallControl _callControl;
    private readonly IAuthControl _authControl;
    private readonly IJWTAuthentication _jwtAuthentication;
    public CallHub(IHubContext<HomeSecurityHub> hub, ICallControl callControl, IAuthControl authControl, IJWTAuthentication jwtAuthentication)
    {
        _hub = hub;
        _callControl = callControl;
        _authControl = authControl;
        _jwtAuthentication = jwtAuthentication;
    }
    public async Task AcceptCall(int callId, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
            Console.WriteLine($"Call Acepted !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {getAuthControlInfoDto.PersonId}");
            await _callControl.AcceptCall(getAuthControlInfoDto, callId);
        }
    }
    public async Task Offer(OfferDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Offer Sent !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await _hub.Clients.Group(dto.ChatId.ToString()).SendAsync("Offer", dto);
        }
    }
    public async Task Answer(AnswerDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Call Answered !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await _hub.Clients.Group(dto.ChatId.ToString()).SendAsync("Answer", dto);
        }
    }
    public async Task RejectCall(int callId, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            var getAuthControlInfoDto = new GetAuthControlInfoDto(){
                PersonId = user.PersonId,
                AuthorizationCode = user.AuthorizationCode,
            };
            Console.WriteLine($"Call Rejected !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {getAuthControlInfoDto.PersonId}");
            await _callControl.RejectCall(getAuthControlInfoDto, callId);
        }
    }
    public async Task IceCandidate(IceCandidateDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Ice Candidate !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await _hub.Clients.Group(dto.ChatId.ToString()).SendAsync("IceCandidate", dto);
        }
    }
}