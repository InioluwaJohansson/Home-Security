using Microsoft.AspNetCore.SignalR;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;

[Authorize]
public class ChatHub : Hub, IChatHub
{
    private static readonly Dictionary<int, List<string>> _connections = new();
    public ChatHub()
    {
    }
    private int GetPersonId()
    {
        int personId = int.Parse(Context.User?.FindFirst("PersonId")?.Value);
        Console.WriteLine($"PersonId from claims: {personId}");
        return personId;
    }
    public async Task JoinChat(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task UpdateChat(string eventName, int chatId, object update)
    {
        await Clients.Group(chatId.ToString()).SendAsync(eventName, update);
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