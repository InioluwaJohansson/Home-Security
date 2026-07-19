using Home_Security.Entities.Calls;

namespace Home_Security.Interfaces.Repositories.Calls;
public interface ICallRepo : IRepo<Call>
{
    Task<Call?> GetByIdAsync(int callId);
    Task<Call?> GetCallDetails(int callId);
    Task<Call?> GetActiveCallByChatIdAsync(int chatId);
    Task<List<Call>> GetCallLogsAsync(int personId, int page, int pageSize);
    Task<bool> CallExistsAsync(int callId);
    Task<bool> IsParticipantAsync(int callId, int personId);
    Task<Call> UpdateCall(Call call);
}