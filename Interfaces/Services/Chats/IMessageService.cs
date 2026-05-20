using Home_Security.Models.ChatDTO;
namespace Home_Security.Interfaces.Services.Chats;
public interface IMessageService
{
    Task<MessageDto> SendMessageAsync(SendMessageDto dto, int senderId);
    Task<List<MessageDto>> GetChatMessagesAsync(int chatId, int personId, int page, int pageSize);
    Task<MessageDto> EditMessageAsync(EditMessageDto dto, int personId);
    Task DeleteMessageAsync(int messageId, int personId);
    Task<int> GetUnreadCountAsync(int chatId, int personId);
}