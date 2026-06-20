using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls.Chats;
public interface IMessageControl
{
    public Task<BaseResponse> SendMessage(GetAuthControlInfoDto getAuthControlInfoDto, SendMessageDto sendMessageDto);
    public Task<BaseResponse> EditMessage(GetAuthControlInfoDto getAuthControlInfoDto, EditMessageDto editMessageDto);
    public Task<List<MessageDto>> GetChatMessages(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, int page, int pageSize);
    public Task<BaseResponse> DeleteMessage(GetAuthControlInfoDto getAuthControlInfoDto, int messageId);
    public Task<int> GetUnreadCount(GetAuthControlInfoDto getAuthControlInfoDto, int chatId);
    Task Typing(GetAuthControlInfoDto getAuthControlInfoDto, int chatId);
}