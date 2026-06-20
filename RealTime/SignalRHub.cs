using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
namespace Home_Security.Hubs;
[Authorize]
public class HomeSecurityHub : Hub
{
    private static readonly Dictionary<int, HashSet<string>> _connections = new();
    private int GetPersonId()
    {
        var value = Context.User?.FindFirst("PersonId")?.Value;

        if (string.IsNullOrEmpty(value))
            throw new HubException("PersonId claim is missing.");

        return int.Parse(value);
    }

    public override async Task OnConnectedAsync()
    {
        var personId = GetPersonId();
        var connectionId = Context.ConnectionId;

        lock (_connections)
        {
            if (!_connections.ContainsKey(personId))
                _connections[personId] = new HashSet<string>();

            _connections[personId].Add(connectionId);
        }

        await Clients.All.SendAsync("UserOnline", personId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var personId = GetPersonId();
        var connectionId = Context.ConnectionId;

        bool shouldMarkOffline = false;

        lock (_connections)
        {
            if (_connections.ContainsKey(personId))
            {
                _connections[personId].Remove(connectionId);

                if (_connections[personId].Count == 0)
                {
                    _connections.Remove(personId);
                    shouldMarkOffline = true;
                }
            }
        }

        if (shouldMarkOffline)
        {
            await Clients.All.SendAsync("UserOffline", personId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinChat(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}