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
    private readonly IMessageService _messageService;
    private readonly IChatService _chatService;
    private readonly IAuthControl _authControl;
    private static readonly Dictionary<int, List<string>> _connections = new();
    public ChatHub(IMessageService messageService,IChatService chatService, IAuthControl authControl)
    {
        _messageService = messageService;
        _chatService = chatService;
        _authControl = authControl;
    }
    private int GetPersonId()
    {
        return int.Parse(Context.User?.FindFirst("PersonId")?.Value);
    }
    public async Task<BaseResponse> JoinChat(int chatId)
    {
        var personId = GetPersonId();
        var isParticipant = await _chatService.IsParticipantAsync(chatId, personId);
        if (!isParticipant)
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "You are not a participant of this chat!"
            };
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        return new BaseResponse()
        {
            Status = true,
            Message = "Joined chat successfully!"
        };
    }
    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
    public async Task<MessageDto> SendMessage(SendMessageDto dto, int personId)
    {
        var message = await _messageService.SendMessageAsync(dto, personId);
        await Clients.Group(dto.ChatId.ToString()).SendAsync("ReceiveMessage", message);
        return message;
    }
    public async Task Typing(TypingDto dto)
    {
        var personId = GetPersonId();
        await Clients.Group(dto.ChatId.ToString()).SendAsync("UserTyping", new
        {
            dto.ChatId,
            PersonName = _authControl.GetUserPersonName(personId),
            PersonId = personId,
            dto.IsTyping
        });
    }
    public async Task MarkAsRead(int chatId)
    {
        var personId = GetPersonId();
        await _chatService.MarkAsReadAsync(chatId, personId);
        await Clients.Group(chatId.ToString()).SendAsync("MessagesRead", new
        {
            ChatId = chatId,
            PersonId = personId,
            ReadAt = DateTime.UtcNow
        });
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