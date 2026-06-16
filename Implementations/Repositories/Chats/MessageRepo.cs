using Home_Security.Entities.Chats;
using Home_Security.Implementations.Repositories;
using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Context;
using Microsoft.EntityFrameworkCore;

public class MessageRepository : BaseRepository<Message>, IMessageRepo
{
    public MessageRepository(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Message?> GetByIdAsync(int messageId)
    {
        return await context.Message.AsNoTracking().Include(x => x.Attachments).Include(x => x.SenderPerson).ThenInclude(x => x.PersonDetails).FirstOrDefaultAsync(x => x.Id == messageId && !x.IsDeleted);
    }
    public async Task<Message> GetMessageByExpression(Expression<Func<Message, bool>> expression)
    {
        return await context.Message.AsNoTracking().Include(x => x.Attachments).Include(x => x.SenderPerson).ThenInclude(x => x.PersonDetails).FirstOrDefaultAsync(expression);
    }
    public async Task<List<Message>> GetChatMessagesAsync(int chatId, int page, int pageSize)
    {
        return await context.Message.Include(x => x.Attachments).Include(x => x.SenderPerson).ThenInclude(p => p.PersonDetails).Where(x => x.ChatId == chatId).OrderByDescending(x => x.SentAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<int> GetUnreadCountAsync(int chatId, DateTime? lastReadAt)
    {
        var query = context.Message.Where(x => x.ChatId == chatId && !x.IsDeleted);
        if (lastReadAt.HasValue)
        {
            query = query.Where(x =>x.SentAt > lastReadAt.Value);
        }
        return await query.CountAsync();
    }
}