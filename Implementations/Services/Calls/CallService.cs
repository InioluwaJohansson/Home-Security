using Home_Security.Entities.Calls;
using Home_Security.Interfaces.Repositories.Calls;
using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Interfaces.Services.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Implementations.Services.Calls;
public class CallService : ICallService
{
    private readonly ICallRepo _callRepo;
    private readonly IChatRepo _chatRepo;
    public CallService(ICallRepo callRepo, IChatRepo chatRepo)
    {
        _callRepo = callRepo;
        _chatRepo = chatRepo;
    }
    public async Task<CallDto> StartCallAsync(StartCallDto dto, int personId)
    {
        var activeCall = await _callRepo.GetActiveCallByChatIdAsync(dto.ChatId);
        if (activeCall != null)
            throw new Exception("A call is already in progress.");
        var chat = await _chatRepo.GetChatWithParticipantsAsync(dto.ChatId);
        if (chat == null)
            throw new Exception("Chat not found.");
        if (!chat.Participants.Any(x => x.PersonId == personId))
            throw new Exception("You are not a participant of this chat.");
        var call = new Call
        {
            ChatId = dto.ChatId,
            CallerPersonId = personId,
            Type = dto.Type,
            Status = CallStatus.Ringing,
            StartedAt = DateTime.UtcNow
        };
        foreach (var participant in chat.Participants)
        {
            call.Participants.Add(new CallParticipant
            {
                PersonId = participant.PersonId,
                IsMuted = false,
                IsCameraEnabled = dto.Type == CallType.Video,
                IsScreenSharing = false,
                IsConnected = participant.PersonId == personId
            });
        }
        await _callRepo.Create(call);
        return MapToDto(call);
    }
    public async Task<BaseResponse> AcceptCallAsync(int callId, int personId)
    {
        var call = await _callRepo.Get(x => x.Id == callId);
        if (call == null)
        {
            return new BaseResponse { 
                Status = true,
                Message = "Unable to find call."
            };
        }
        var participant = call.Participants.FirstOrDefault(x => x.PersonId == personId);
        if (participant == null)
        {
            return new BaseResponse { 
                Status = true,
                Message = "User not found."
            };
        }
        participant.IsConnected = true;
        call.Status = CallStatus.Connected;
        call.AnsweredAt = DateTime.UtcNow;
        await _callRepo.Update(call);
        return new BaseResponse { 
            Status = true,
            Message = "Call accepted successfully."
        };
    }
    public async Task<BaseResponse> RejectCallAsync(int callId, int personId)
    {
        var call = await _callRepo.Get(x => x.Id == callId);
        if (call == null){
            return new BaseResponse { 
                Status = true,
                Message = "Unable to find call."
            };
        }
        if (!call.Participants.Any(x => x.PersonId == personId))
        {
            return new BaseResponse { 
                Status = true,
                Message = "User not found."
            };
        }
        call.Status = CallStatus.Rejected;
        call.EndedAt = DateTime.UtcNow;
        await _callRepo.Update(call);
        return new BaseResponse { 
            Status = true,
            Message = "Call rejected successfully."
        };
    }
    public async Task<bool> IsParticipantAsync(int callId, int personId)
    {
        var call = await _chatRepo.GetChatWithParticipantsAsync(callId);
        return call?.Participants.Any(x => x.PersonId == personId) ?? false;
    }
    private CallDto MapToDto(Call call)
    {
        return new CallDto
        {
            Id = call.Id,
            ChatId = call.ChatId,
            CallerPersonId = call.CallerPersonId,
            Type = call.Type,
            Status = call.Status,
            StartedAt = call.StartedAt,
            AnsweredAt = call.AnsweredAt,
            EndedAt = call.EndedAt,
            Participants = call.Participants.Select(p => new CallParticipantDto
            {
                PersonId = p.PersonId,
                IsMuted = p.IsMuted,
                IsCameraEnabled = p.IsCameraEnabled,
                IsScreenSharing = p.IsScreenSharing,
                IsConnected = p.IsConnected
            }).ToList()
        };
    }
    public async Task<BaseResponse> EndCallAsync(int callId, int personId)
    {
        var call = await _callRepo.GetByIdAsync(callId);
        if (call == null){
            return new BaseResponse { 
                Status = true,
                Message = "Unable to find call."
            };
        }
        if (!call.Participants.Any(x => x.PersonId == personId))
        {
            return new BaseResponse { 
                Status = true,
                Message = "User not found."
            };
        }
        call.Status = CallStatus.Ended;
        call.EndedAt = DateTime.UtcNow;
        foreach (var participant in call.Participants)
        {
            participant.IsConnected = false;
        }
        await _callRepo.Update(call);
        return new BaseResponse { 
            Status = true,
            Message = "Call ended successfully."
        };
    }
    public async Task<CallDto?> GetCallAsync(int callId, int personId)
    {
        var call = await _callRepo.GetByIdAsync(callId);
        if (call == null)
            return null;
        if (!call.Participants.Any(x => x.PersonId == personId))
            return null;
        return MapToDto(call);
    }
    private CallParticipantDto MapToDto(CallParticipant participant)
    {
        return new CallParticipantDto
        {
            PersonId = participant.PersonId,
            FullName = participant.Person.PersonDetails.FirstName + " " + participant.Person.PersonDetails.LastName,
            ProfileImage = participant.Person.PersonDetails.ImageUrl,
            IsMuted = participant.IsMuted,
            IsCameraEnabled = participant.IsCameraEnabled,
            IsScreenSharing = participant.IsScreenSharing,
            IsConnected = participant.IsConnected
        };
    }
}