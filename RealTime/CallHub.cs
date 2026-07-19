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
    private readonly IAuthControl _authControl;
    private readonly IJWTAuthentication _jwtAuthentication;
    public CallHub(IHubContext<HomeSecurityHub> hub, IAuthControl authControl, IJWTAuthentication jwtAuthentication)
    {
        _hub = hub;
        _authControl = authControl;
        _jwtAuthentication = jwtAuthentication;
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
    public async Task ToggleCallItems(ToggleCallItemsDto dto, string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Ice Candidate !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await _hub.Clients.Group(dto.ChatId.ToString()).SendAsync("ToggleCallItems", dto);
        }
    }
}