using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.RealTimeServices;
public interface IChatHub
{
    Task UpdateChat(string eventName, int chatId, object update);
    Task JoinChat(int chatId);
    Task LeaveChat(int chatId);
}