using Home_Security.Models.ChatDTO;
namespace Home_Security.Interfaces.Services.Chats;
public interface IChatService
{
    Task<ChatDto> CreateDirectChatAsync(int recipientPersonId, int currentPersonId);

    Task<ChatDto> CreateGroupChatAsync(CreateGroupChatDto dto, int currentPersonId);

    Task<List<ChatDto>> GetMyChatsAsync(int personId);

    Task<ChatDto?> GetChatByIdAsync(int chatId, int personId);

    Task AddParticipantsAsync(int chatId, AddParticipantsDto dto, int currentPersonId);

    Task RemoveParticipantAsync(int chatId, int personId, int currentPersonId);

    Task<bool> IsParticipantAsync(int chatId, int personId);
}