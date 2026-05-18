using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Entities.Chats;
public class ChatService : IChatService
{
    private readonly IChatRepo _chatRepo;
    private readonly IHttpContextAccessor _http;

    public ChatService(IChatRepo chatRepo, IHttpContextAccessor http)
    {
        _chatRepo = chatRepo;
        _http = http;
    }
    public async Task<ChatDto> CreateDirectChatAsync(int recipientPersonId, int currentPersonId)
    {

        var existingChat = await _chatRepo
            .GetDirectChatAsync(currentPersonId, recipientPersonId);

        if (existingChat != null)
            return MapToDto(existingChat);

        var chat = new Chat
        {
            IsGroup = false,
            CreatedByPersonId = currentPersonId,
            Participants = new List<ChatParticipant>
            {
                new ChatParticipant
                {
                    PersonId = currentPersonId
                },
                new ChatParticipant
                {
                    PersonId = recipientPersonId
                }
            }
        };
        await _chatRepo.Create(chat);
        return MapToDto(chat);
    }
    public async Task<ChatDto> CreateGroupChatAsync(CreateGroupChatDto dto, int currentPersonId)
    {
        var chat = new Chat
        {
            IsGroup = true,
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            CreatedByPersonId = currentPersonId,
            Participants = dto.ParticipantPersonIds
                .Append(currentPersonId)
                .Distinct()
                .Select(x => new ChatParticipant
                {
                    PersonId = x,
                    IsAdmin = x == currentPersonId
                }).ToList()
        };

        await _chatRepo.Create(chat);
        return MapToDto(chat);
    }
    public async Task<List<ChatDto>> GetMyChatsAsync(int personId)
    {
        var chats = await _chatRepo.GetPersonChatsAsync(personId);
        return chats.Select(MapToDto).ToList();
    }
    public async Task<ChatDto?> GetChatByIdAsync(int chatId, int personId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        if (chat == null)
            return null;

        var isParticipant = chat.Participants
            .Any(p => p.PersonId == personId);

        if (!isParticipant)
            throw new Exception("Access denied");

        return MapToDto(chat);
    }
    public async Task AddParticipantsAsync(int chatId, AddParticipantsDto dto, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        if (chat == null)
            throw new Exception("Chat not found");

        if (!chat.IsGroup)
            throw new Exception("Cannot add participants to direct chat");

        var isAdmin = chat.Participants
            .Any(p => p.PersonId == currentPersonId && p.IsAdmin);

        if (!isAdmin)
            throw new Exception("Only admins can add participants");

        var existingIds = chat.Participants
            .Select(p => p.PersonId)
            .ToHashSet();

        var newParticipants = dto.PersonIds
            .Where(id => !existingIds.Contains(id))
            .Select(id => new ChatParticipant
            {
                ChatId = chatId,
                PersonId = id,
                JoinedAt = DateTime.UtcNow,
                IsAdmin = false
            });

        foreach (var participant in newParticipants)
        {
            chat.Participants.Add(participant);
        }

        await _chatRepo.Update(chat);
    }
    public async Task RemoveParticipantAsync(int chatId, int personId, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        if (chat == null)
            throw new Exception("Chat not found");

        if (!chat.IsGroup)
            throw new Exception("Cannot remove participants from direct chat");
            var isSelfRemove = currentPersonId == personId;

        var isAdmin = chat.Participants
            .Any(p => p.PersonId == currentPersonId && p.IsAdmin);

        if (!isSelfRemove && !isAdmin)
            throw new Exception("Not allowed");
            var participant = chat.Participants
        .FirstOrDefault(p => p.PersonId == personId);
        var adminCount = chat.Participants.Count(x => x.IsAdmin);

        if (participant == null)
            return;
        if (participant.IsAdmin && adminCount == 1)
            throw new Exception("Cannot remove last admin");

        chat.Participants.Remove(participant);

        await _chatRepo.Update(chat);

    }
    public async Task<bool> IsParticipantAsync(int chatId, int personId)
    {
        return await _chatRepo.IsParticipantAsync(chatId, personId);
    }
    private ChatDto MapToDto(Chat chat)
    {
        return new ChatDto
        {
            
        };
    }
}