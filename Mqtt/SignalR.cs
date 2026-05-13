using Microsoft.AspNetCore.SignalR;
using Home_Security.Hubs;

namespace Home_Security.RealTimeServices
{
    public class RealtimeNotificationService
    {
        private readonly IHubContext<SecurityHub> _hub;

        public RealtimeNotificationService(IHubContext<SecurityHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAll<T>(string eventName, T payload)
        {
            await _hub.Clients.All.SendAsync(eventName, payload);
        }
    }
}