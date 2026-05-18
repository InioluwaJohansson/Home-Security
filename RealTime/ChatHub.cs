using Microsoft.AspNetCore.SignalR;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IChatService _chatService;

    public ChatHub(
        IMessageService messageService,
        IChatService chatService)
    {
        _messageService = messageService;
        _chatService = chatService;
    }
    private int GetPersonId()
    {
        return int.Parse(
            Context.User?.FindFirst("PersonId")?.Value
        );
    }
    public async Task JoinChat(int chatId)
{
    var personId = GetPersonId();

    var isParticipant = await _chatService
        .IsParticipantAsync(chatId, personId);

    if (!isParticipant)
        throw new HubException("Not allowed in this chat");

    await Groups.AddToGroupAsync(
        Context.ConnectionId,
        chatId.ToString()
    );
}
public async Task LeaveChat(int chatId)
{
    await Groups.RemoveFromGroupAsync(
        Context.ConnectionId,
        chatId.ToString()
    );
}
public async Task SendMessage(SendMessageDto dto)
{
    var message = await _messageService.SendMessageAsync(dto, GetPersonId());

    await Clients.Group(dto.ChatId.ToString())
        .SendAsync("ReceiveMessage", message);
}
public async Task Typing(TypingDto dto)
{
    var personId = GetPersonId();

    await Clients.Group(dto.ChatId.ToString())
        .SendAsync("UserTyping", new
        {
            dto.ChatId,
            PersonId = personId,
            dto.IsTyping
        });
}
public async Task MarkAsRead(int chatId)
{
    var personId = GetPersonId();

    await _chatService.MarkAsReadAsync(chatId, personId);

    await Clients.Group(chatId.ToString())
        .SendAsync("MessagesRead", new
        {
            ChatId = chatId,
            PersonId = personId,
            ReadAt = DateTime.UtcNow
        });
}
public override async Task OnConnectedAsync()
{
    var personId = GetPersonId();

    if (!Connection.ContainsKey(personId))
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