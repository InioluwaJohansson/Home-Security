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
        return await context.Calls.AsNoTracking().Include(x => x.Chat).Include(c => c.Participants).ThenInclude(p => p.Person).FirstOrDefaultAsync(c => c.Id == callId);
    }
    public async Task<Call?> GetCallDetails(int callId)
    {
        return await context.Calls.AsNoTracking().Include(x => x.Chat).ThenInclude(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).Include(c => c.Participants).ThenInclude(p => p.Person).ThenInclude(x => x.PersonDetails).FirstOrDefaultAsync(c => c.Id == callId);
    }
    public async Task<Call?> GetActiveCallByChatIdAsync(int chatId)
    {
        return await context.Calls.Include(x => x.Chat).ThenInclude(x => x.Participants).ThenInclude(x => x.Person).Include(c => c.Participants).FirstOrDefaultAsync(c => c.ChatId == chatId && (c.Status == CallStatus.Ringing || c.Status == CallStatus.Connected));
    }
    public async Task<bool> CallExistsAsync(int callId)
    {
        return await context.Calls.AnyAsync(c => c.Id == callId);
    }
    public async Task<bool> IsParticipantAsync(int callId, int personId)
    {
        return await context.CallParticipants.AnyAsync(p => p.CallId == callId && p.PersonId == personId);
    }
    public async Task<List<Call>> GetPersonCallsAsync(int personId)
    {
        return await context.Calls.Include(x => x.Chat).ThenInclude(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).Include(c => c.Participants).Where(c => c.Participants.Any(p => p.PersonId == personId)).OrderByDescending(c => c.StartedAt).ToListAsync();
    }
    public async Task<List<Call>> GetCallLogsAsync(int personId, int page, int pageSize)
    {
        return await context.Calls.Include(x => x.Chat).ThenInclude(x => x.Participants).ThenInclude(x => x.Person).ThenInclude(x => x.PersonDetails).Include(c => c.Chat).Include(c => c.Participants).ThenInclude(p => p.Person).Where(c => c.Participants.Any(p => p.PersonId == personId)).OrderByDescending(c => c.StartedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<List<Call>> GetTimedOutRingingCallsAsync(TimeSpan timeout)
    {
        var cutoff = DateTime.UtcNow.Subtract(timeout);
        return await context.Calls.Include(c => c.Chat).Include(c => c.Participants).ThenInclude(p => p.Person).Where(c => c.Status == CallStatus.Ringing && c.StartedAt <= cutoff).ToListAsync();
    }
}