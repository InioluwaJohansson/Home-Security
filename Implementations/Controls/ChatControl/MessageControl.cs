using Home_Security.Entities.Chats;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
namespace Home_Security.Implementations.Controls.Messages;
public class MessageControl : IMessageControl
{
    IAuthControl _authControl;
    IMessageService _messageService;
    IChatHub _chatHub;
    public MessageControl(IAuthControl authControl, IMessageService messageService, IChatHub chatHub)
    {
        _authControl = authControl;
        _messageService = messageService;
        _chatHub = chatHub;
    }
    public async Task<BaseResponse> SendMessage(GetAuthControlInfoDto getAuthControlInfoDto, SendMessageDto sendMessageDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _messageService.SendMessageAsync(sendMessageDto, auth.Id);
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> EditMessage(GetAuthControlInfoDto getAuthControlInfoDto, EditMessageDto editMessageDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var message = await _messageService.EditMessageAsync(editMessageDto, auth.Id);
            return message;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<List<MessageDto>> GetChatMessages(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, int page, int pageSize)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _messageService.GetChatMessagesAsync(chatId, auth.Id, page, pageSize);

        return null;
    }
    public async Task<BaseResponse> DeleteMessage(GetAuthControlInfoDto getAuthControlInfoDto, int messageId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _messageService.DeleteMessageAsync(messageId, auth.Id);
        return _authControl.AuthFaliure();
    }
    public async Task<int> GetUnreadCount(GetAuthControlInfoDto getAuthControlInfoDto, int chatId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _messageService.GetUnreadCountAsync(chatId, auth.Id);
        return 0;
    }
    public async Task Typing(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, string action)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
           await _messageService.Typing(new TypingDto
           {
               ChatId = chatId,
               PersonId = auth.Id,
               Name = _authControl.GetUserPersonName(auth.Id),
               Action = action,
               IsTyping = true
           });
    }
}