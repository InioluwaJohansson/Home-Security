using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.RealTimeServices;
public interface IChatHub
{
    Task<BaseResponse> JoinChat(int chatId);
    Task LeaveChat(int chatId);
    Task<MessageDto> SendMessage(SendMessageDto dto, int personId);
    Task Typing(TypingDto dto);
    Task MarkAsRead(int chatId);
}