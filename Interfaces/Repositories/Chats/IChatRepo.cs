using Home_Security.Entities.Chats;
namespace Home_Security.Interfaces.Repositories.Chats;
public interface IChatRepo: IRepo<Chat>
{
    Task<Chat?> GetByIdAsync(int chatId);

    Task<Chat?> GetChatWithParticipantsAsync(int chatId);

    Task<List<Chat>> GetPersonChatsAsync(int personId);

    Task<Chat?> GetDirectChatAsync(int firstPersonId, int secondPersonId);

    Task<bool> ChatExistsAsync(int chatId);

    Task<bool> IsParticipantAsync(int chatId, int personId);
}