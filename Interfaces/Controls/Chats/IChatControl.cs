using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls.Chats;
public interface IChatControl
{
    public Task<ChatDto> CreateDirectChat(GetAuthControlInfoDto getAuthControlInfoDto, int recipientPersonId);
    public Task<ChatDto> CreateGroupChat(GetAuthControlInfoDto getAuthControlInfoDto, CreateGroupChatDto createGroupChatDto);
    public Task<BaseResponse> UpdateGroupChat(GetAuthControlInfoDto getAuthControlInfoDto, UpdateGroupChatDto updateGroupChatDto);
    public Task<BaseResponse> DeleteChat(GetAuthControlInfoDto getAuthControlInfoDto, int chatId);
    public Task<BaseResponse> AddParticipants(GetAuthControlInfoDto getAuthControlInfoDto, int chatId, AddParticipantsDto addParticipantsDto);
    public Task<ChatDto> GetChatById(GetAuthControlInfoDto getAuthControlInfoDto, int chatId);
    public Task<List<ChatDto>> GetMyChats(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> RemoveParticipant(GetAuthControlInfoDto getAuthControlInfoDto,int chatId, int recipientId);
}
