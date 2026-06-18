using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Hubs;
namespace Home_Security.RealTimeServices;

[Authorize]
public class ChatHub : HomeSecurityHub, IChatHub
{
    private readonly IHubContext<HomeSecurityHub> _hub;
    private static readonly Dictionary<int, List<string>> _connections = new();
    public ChatHub(IHubContext<HomeSecurityHub> hub)
    {
        _hub = hub;
    }
    private int GetPersonId()
    {
        int personId = int.Parse(Context.User?.FindFirst("PersonId")?.Value);
        Console.WriteLine($"PersonId from claims: {personId}");
        return personId;
    }
    public async Task JoinChat(int chatId)
    {
        await _hub.Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task LeaveChat(int chatId)
    {
        await _hub.Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task UpdateChat(string eventName, int chatId, object update)
    {
        await _hub.Clients.Group(chatId.ToString()).SendAsync(eventName, update);
    }
    public override async Task OnConnectedAsync()
    {
        var personId = GetPersonId();
        if (!_connections.ContainsKey(personId))
            _connections[personId] = new List<string>();
        _connections[personId].Add(Context.ConnectionId);
        await Clients.All.SendAsync("UserOnline", personId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var personId = GetPersonId();
        if (_connections.ContainsKey(personId))
        {
            _connections[personId].Remove(Context.ConnectionId);
            if (_connections[personId].Count == 0)
            {
                _connections.Remove(personId);
                await Clients.All.SendAsync("UserOffline", personId);
            }
        }
        await base.OnDisconnectedAsync(exception);
    }
}