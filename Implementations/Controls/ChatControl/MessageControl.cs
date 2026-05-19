using Home_Security.Entities.Chats;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Controls.Messages;
public class MessageControl : IMessageControl
{
    IAuthControl _authControl;
    IMessageService _messageService;
    public MessageControl(IAuthControl authControl, IMessageService messageService)
    {
        _authControl = authControl;
        _messageService = messageService;
    }
    public async Task<MessageDto> SendMessage(GetAuthControlInfoDto getAuthControlInfoDto, SendMessageDto sendMessageDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var message = await _messageService.SendMessageAsync(sendMessageDto, auth.Id);
            return message;
        }
        return null;
    }
    public async Task<MessageDto> EditMessage(GetAuthControlInfoDto getAuthControlInfoDto, EditMessageDto editMessageDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var message = await _messageService.EditMessageAsync(editMessageDto, auth.Id);
            return message;
        }
        return null;
    }
    public async Task<List<MessageDto>> GetChatMessages(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, int page, int pageSize)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var message = await _messageService.GetChatMessagesAsync(chatId, auth.Id, page, pageSize);
            return message;
        }
        return null;
    }
    public async Task DeleteMessage(GetAuthControlInfoDto getAuthControlInfoDto, int messageId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            await _messageService.DeleteMessageAsync(messageId, auth.Id);
    }
    public async Task<int> GetUnreadCount(GetAuthControlInfoDto getAuthControlInfoDto, int chatId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _messageService.GetUnreadCountAsync(chatId, auth.Id);

        return 0;
    }
}