using Home_Security.Entities.Chats;
namespace Home_Security.Interfaces.Repositories.Chats;
public interface IMessageRepo : IRepo<Message>
{
    Task<Message?> GetByIdAsync(int messageId);
    Task<Message> GetMessageByExpression(Expression<Func<Message, bool>> expression);

    Task<List<Message>> GetChatMessagesAsync(int chatId, int page, int pageSize);

    Task<int> GetUnreadCountAsync(int chatId, DateTime? lastReadAt);
}