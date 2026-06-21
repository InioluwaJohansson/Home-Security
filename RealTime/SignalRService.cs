using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;
using Microsoft.AspNetCore.Authorization;
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
        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(update));
        await _hub.Clients.Group(role.ToString()).SendAsync(eventName, update);
    }
}
