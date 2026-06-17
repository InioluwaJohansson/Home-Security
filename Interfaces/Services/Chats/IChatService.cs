using Home_Security.Models.ChatDTO;
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services.Chats;
public interface IChatService
{
    Task<ChatDto> CreateDirectChatAsync(int recipientPersonId, int currentPersonId);
    Task<ChatDto> CreateGroupChatAsync(CreateGroupChatDto dto, int currentPersonId);
    Task<BaseResponse> UpdateGroupChatAsync(int chatId, UpdateGroupChatDto dto, int currentPersonId);
    Task<BaseResponse> DeleteChatAsync(int chatId, int currentPersonId);
    Task<List<ChatDto>> GetMyChatsAsync(int personId);
    Task<ChatDto?> GetChatByIdAsync(int chatId, int personId);
    Task<BaseResponse> AddParticipantsAsync(int chatId, AddParticipantsDto dto, int currentPersonId);
    Task<BaseResponse> RemoveParticipantAsync(int chatId, int personId, int currentPersonId);
    public Task<BaseResponse> MarkAsReadAsync(int chatId, int personId);
    Task<bool> IsParticipantAsync(int chatId, int personId);
}