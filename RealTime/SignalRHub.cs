using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Home_Security.Authentication;
using Home_Security.Interfaces.Controls;
using Home_Security.Models.CallDTO;
using System.Text.Json;
namespace Home_Security.Hubs;
[Authorize]
public class HomeSecurityHub : Hub
{
    IAuthControl _authControl;
    IJWTAuthentication _jwtAuthentication;
    public HomeSecurityHub(IAuthControl authControl, IJWTAuthentication jwtAuthentication){
        _authControl = authControl;
        _jwtAuthentication = jwtAuthentication;
    }
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
    public async Task AddPersonRole(string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if(user != null){
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status != false){
                await Groups.AddToGroupAsync(Context.ConnectionId, auth.Role.ToString());
                await Groups.AddToGroupAsync(Context.ConnectionId, auth.Id.ToString());
            }
        }
    }
    public async Task Logout(string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if(user != null){
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status != false){
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, auth.Role.ToString());
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, auth.Id.ToString());
            }
        }
    }
    public async Task LogOutAllUsers(string token)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if(user != null){
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status != false){
                await Clients.All.SendAsync("HighLevelLogOut");
            }
        }
    }
    public async Task LogOutPersonId(string token, int personId)
    {
        var user = _jwtAuthentication.GetUserFromToken(token);
        if(user != null){
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status != false){
                await Clients.All.SendAsync("LogOut", personId);
            }
        }
    }
    public void AddPerson(string token)
    {

    }

    //Chats
    public async Task JoinChat(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task UserOnline(int personId)
    {
        await Clients.All.SendAsync("UserOnline", personId);
    }

    //Calls
    public async Task Offer(OfferDto dto, string token)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Offer Sent !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await Clients.Group(dto.ChatId.ToString()).SendAsync("Offer", dto);
        }
    }
    public async Task Answer(AnswerDto dto, string token)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Call Answered !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await Clients.Group(dto.ChatId.ToString()).SendAsync("Answer", dto);
        }
    }
    public async Task IceCandidate(IceCandidateDto dto, string token)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Ice Candidate !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await Clients.Group(dto.ChatId.ToString()).SendAsync("IceCandidate", dto);
        }
    }
    public async Task ToggleCallItems(ToggleCallItemsDto dto, string token)
    {
        Console.WriteLine(JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user != null)
        {
            Console.WriteLine($"Ice Candidate !!!!!!!!!!!!!!!!!!!!!!!!!!    PersonId: {user.PersonId}");
            var auth = await _authControl.GetAuthDetails(user.PersonId, user.AuthorizationCode);
            if(auth.Status) await Clients.Group(dto.ChatId.ToString()).SendAsync("ToggleCallItems", dto);
        }
    }
}