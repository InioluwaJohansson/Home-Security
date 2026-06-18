using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services.Chats;
public interface IMessageService
{
    Task<BaseResponse> SendMessageAsync(SendMessageDto dto, int senderId);
    Task<List<MessageDto>> GetChatMessagesAsync(int chatId, int personId, int page, int pageSize);
    Task<BaseResponse> EditMessageAsync(EditMessageDto dto, int personId);
    Task<BaseResponse> DeleteMessageAsync(int messageId, int personId);
    Task<int> GetUnreadCountAsync(int chatId, int personId);
    Task Typing(TypingDto dto);
}