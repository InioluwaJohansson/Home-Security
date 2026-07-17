using Microsoft.EntityFrameworkCore;
using Home_Security.Context;
using Home_Security.Entities.Calls;
using Home_Security.Interfaces.Repositories.Calls;
namespace Home_Security.Implementations.Repositories.Calls;
public class CallRepo : BaseRepository<Call>, ICallRepo
{
    public CallRepo(HomeSecurityContext _context)
    {
        context = _context;
    }
    public async Task<Call?> GetByIdAsync(int callId)
    {
        return await context.Calls.AsNoTracking().Include(c => c.Participants).ThenInclude(p => p.Person).FirstOrDefaultAsync(c => c.Id == callId);
    }
    public async Task<Call?> GetActiveCallByChatIdAsync(int chatId)
    {
        return await context.Calls.Include(c => c.Participants).FirstOrDefaultAsync(c =>
                c.ChatId == chatId &&
                (c.Status == CallStatus.Ringing ||
                c.Status == CallStatus.Connected));
    }
    public async Task<bool> CallExistsAsync(int callId)
    {
        return await context.Calls.AnyAsync(c => c.Id == callId);
    }
    public async Task<bool> IsParticipantAsync(int callId, int personId)
    {
        return await context.CallParticipants.AnyAsync(p => p.CallId == callId && p.PersonId == personId);
    }
}