using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Home_Security.Models.CallDTO;
using Home_Security.Authentication;
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
}