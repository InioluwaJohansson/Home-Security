using Home_Security.Entities.Identity;
using Home_Security.Entities.Chats;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Controls.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
namespace Home_Security.Implementations.Controls.Chats;
public class ChatControl : IChatControl
{
    IAuthControl _authControl;
    IChatService _chatService;
    public ChatControl(IAuthControl authControl, IChatService chatService)
    {
        _authControl = authControl;
        _chatService = chatService;
    }
    public async Task<ChatDto> CreateDirectChat(GetAuthControlInfoDto getAuthControlInfoDto, int recipientPersonId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var chat = await _chatService.CreateDirectChatAsync(recipientPersonId, auth.Id);
            return chat;
        }
        return null;
    }
    public async Task<ChatDto> CreateGroupChat(GetAuthControlInfoDto getAuthControlInfoDto, CreateGroupChatDto createGroupChatDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var chat = await _chatService.CreateGroupChatAsync(createGroupChatDto, auth.Id);
            return chat;
        }
        return null;
    }
    public async Task<BaseResponse> UpdateGroupChat(GetAuthControlInfoDto getAuthControlInfoDto, UpdateGroupChatDto updateGroupChatDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _chatService.UpdateGroupChatAsync(updateGroupChatDto.ChatId, updateGroupChatDto, auth.Id);
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> DeleteChat(GetAuthControlInfoDto getAuthControlInfoDto, int chatId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _chatService.DeleteChatAsync(chatId, auth.Id);
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> AddParticipants(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, AddParticipantsDto addParticipantsDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
            return await _chatService.AddParticipantsAsync(chatId, addParticipantsDto, auth.Id);
        return _authControl.AuthFaliure();
    }
    public async Task<ChatDto> GetChatById(GetAuthControlInfoDto getAuthControlInfoDto, int chatId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId, auth.Id);
            return ChatRefactor(chat, auth.Id);
        }
        return null;
    }
    public async Task<List<ChatDto>> GetMyChats(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
        {
            var chat = await _chatService.GetMyChatsAsync(auth.Id);
            var newChats = chat.Select(x => ChatRefactor(x, auth.Id)).ToList();
            return newChats;
        }
        return null;
    }
    public async Task<BaseResponse> RemoveParticipant(GetAuthControlInfoDto getAuthControlInfoDto,int chatId, int recipientId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false)
           return await _chatService.RemoveParticipantAsync(chatId, recipientId, auth.Id);
        return _authControl.AuthFaliure();
    }
    private ChatDto ChatRefactor(ChatDto x, int personId){
        if(!x.IsGroup){
            if(x.Participants.FirstOrDefault().PersonId == personId){
                x.Name = _authControl.GetUserPersonName(x.Participants.LastOrDefault().PersonId);
                x.ImageUrl = _authControl.GetPersonById(x.Participants.LastOrDefault().PersonId)?.PersonDetails.ImageUrl ?? "";
            }
            if(x.Participants.LastOrDefault().PersonId == personId){
                x.Name = _authControl.GetUserPersonName(x.Participants.FirstOrDefault().PersonId);
                x.ImageUrl = _authControl.GetPersonById(x.Participants.FirstOrDefault().PersonId)?.PersonDetails.ImageUrl ?? "";
            }
        }
        return x;
    }
}