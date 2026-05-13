using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Microsoft.AspNetCore.Authorization;
namespace Home_Security.RealTimeServices;

[Authorize]
public class RealtimeNotificationService
{
    private readonly IHubContext<HomeSecurityHub> _hub;
    public RealtimeNotificationService(IHubContext<HomeSecurityHub> hub)
    {
        _hub = hub;
    }
    public async Task NotifyAll<T>(string eventName, T payload)
    {
        await _hub.Clients.All.SendAsync(eventName, payload);
    }
}
