using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Hubs;
namespace Home_Security.RealTimeServices;

public class ChatHub : IChatHub
{
    private readonly IHubContext<HomeSecurityHub> _hub;
    
    public ChatHub(IHubContext<HomeSecurityHub> hub)
    {
        _hub = hub;
    }
    public async Task UpdateChat(string eventName, int chatId, object update)
    {
        await _hub.Clients.Group(chatId.ToString()).SendAsync(eventName, update);
    }
}