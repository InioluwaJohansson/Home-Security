using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Context;
using Home_Security.Entities.Chats;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories.Chats;
public class ChatRepo : BaseRepository<Chat>, IChatRepo
{
    public ChatRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Chat?> GetByIdAsync(int chatId)
    {
        return await context.Chat.FirstOrDefaultAsync(x => x.Id == chatId && !x.IsDeleted);
    }
    public async Task<Chat?> GetChatWithParticipantsAsync(int chatId)
    {
        return await context.Chat.Include(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).FirstOrDefaultAsync(x => x.Id == chatId && !x.IsDeleted);
    }
    public async Task<bool> IsParticipantAsync(int chatId, int personId)
    {
        return await context.ChatParticipant.AnyAsync(x => x.ChatId == chatId && x.PersonId == personId && x.IsDeleted == false);
    }
     public async Task<Chat?> GetDirectChatAsync(int firstPersonId, int secondPersonId)
    {
        return await context.Chat.Include(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).Where(x => !x.IsGroup && !x.IsDeleted).FirstOrDefaultAsync(x => x.Participants.Count == 2 && x.Participants.Any(p => p.PersonId == firstPersonId) && x.Participants.Any(p => p.PersonId == secondPersonId));
    }
    public async Task<List<Chat>> GetPersonChatsAsync(int personId)
    {
        return await context.Chat.Include(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).Include(x => x.Messages.OrderByDescending(m => m.SentAt).Take(1)).Where(x => x.Participants.Any(p => p.PersonId == personId && !p.IsDeleted)).Where(x => !x.IsDeleted).OrderByDescending(x => x.Messages.Max(m =>(DateTime?)m.SentAt)).ToListAsync();
    }
    public async Task<bool> ChatExistsAsync(int chatId)
    {
        return await context.Chat.AnyAsync(x => x.Id == chatId && !x.IsDeleted);
    }
}