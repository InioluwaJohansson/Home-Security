using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;

namespace Home_Security.RealTimeServices
{
    public class RealtimeNotificationService
    {
        private readonly IHubContext<HomeSecurity> _hub;

        public RealtimeNotificationService(IHubContext<HomeSecurity> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAll<T>(string eventName, T payload)
        {
            await _hub.Clients.All.SendAsync(eventName, payload);
        }
    }
}