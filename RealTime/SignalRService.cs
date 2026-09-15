using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Entities.Identity;
namespace Home_Security.RealTimeServices;

[Authorize]
public class RealtimeNotificationService : IRealtimeNotificationService
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
    public async Task UpdateAppList(string eventName, Role role, object update)
    {
        await _hub.Clients.Group(role.ToString()).SendAsync(eventName, update);
    }
    public async Task NotifyAll<T>(string eventName, List<Role> roles, T payload)
    {
        foreach (var role in roles) await _hub.Clients.Group(role.ToString()).SendAsync(eventName, payload);
    }
    public async Task NotifyAll<T>(string eventName, int personId, T payload)
    {
        await _hub.Clients.Group(personId.ToString()).SendAsync(eventName, payload);
    }
}
