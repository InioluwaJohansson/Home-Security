using Home_Security.Entities.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Services.Calls;
public interface ICallService
{
    Task<CallDto> StartCallAsync(StartCallDto dto, int personId);
    Task<BaseResponse> AcceptCallAsync(int callId, int personId);
    Task<BaseResponse> RejectCallAsync(int callId, int personId);
    Task<BaseResponse> EndCallAsync(int callId, int personId);
    Task<CallDto?> GetCallAsync(int callId, int personId);
    Task<bool> IsParticipantAsync(int callId, int personId);
}