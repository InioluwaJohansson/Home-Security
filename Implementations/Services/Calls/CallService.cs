using Home_Security.Entities.Calls;
using Home_Security.Entities.Chats;
using Home_Security.Interfaces.Repositories.Calls;
using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Interfaces.Services.Calls;
using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services.Calls;
public class CallService : ICallService
{
    private readonly ICallRepo _callRepo;
    private readonly IChatRepo _chatRepo;
    private readonly IChatHub _realtimeNotificationService;
    public CallService(ICallRepo callRepo, IChatRepo chatRepo, IChatHub realtimeNotificationService)
    {
        _callRepo = callRepo;
        _chatRepo = chatRepo;
        _realtimeNotificationService = realtimeNotificationService;
    }
    public async Task<CallDto> StartCallAsync(StartCallDto dto, int personId)
    {
        var activeCall = await _callRepo.GetActiveCallByChatIdAsync(dto.ChatId);
        if (activeCall != null)
            throw new Exception("A call is already active for this chat.");
        var chat = await _chatRepo.GetChatWithParticipantsAsync(dto.ChatId);
        if (chat == null)
            throw new Exception("Chat not found.");
        if (!chat.Participants.Any(x => x.PersonId == personId))
            throw new Exception("You are not a participant of this chat.");
        var date = DateTime.UtcNow;
        var call = new Call
        {
            ChatId = dto.ChatId,
            CallerPersonId = personId,
            Type = dto.Type,
            Status = CallStatus.Ringing,
            StartedAt = date
        };
        foreach (var chatParticipant in chat.Participants)
        {
            call.Participants.Add(new CallParticipant
            {
                PersonId = chatParticipant.PersonId,
                Status = chatParticipant.PersonId == personId ? CallParticipantStatus.Connected : CallParticipantStatus.Ringing,
                IsMuted = false,
                IsCameraEnabled = dto.Type == CallType.Video,
                IsScreenSharing = false,
            });
        }
        call = await _callRepo.Create(call);

        call = await _callRepo.Get(x => x.StartedAt == date);
        var callDto = MapToDto(await _callRepo.GetByIdAsync(call.Id));
        await _realtimeNotificationService.UpdateChat("IncomingCall", call.ChatId, callDto);
        return callDto;
    }
    public async Task<BaseResponse> AcceptCallAsync(int callId, int personId)
    {
        var call = await _callRepo.GetByIdAsync(callId);
        if (call == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Call not found."
            };
        }
        var participant = call.Participants.FirstOrDefault(x => x.PersonId == personId);
        if (participant == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "User is not a participant of this call."
            };
        }
        if (participant.Status == CallParticipantStatus.Connected)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "User is already connected."
            };
        }
        participant.Status = CallParticipantStatus.Connected;
        if (call.Status != CallStatus.Connected)
        {
            call.Status = CallStatus.Connected;
            if (!call.AnsweredAt.HasValue) call.AnsweredAt = DateTime.UtcNow;
        }
        await _callRepo.Update(call);
        await _realtimeNotificationService.UpdateChat("CallAccepted", call.ChatId, MapToDto(call));
        return new BaseResponse
        {
            Status = true,
            Message = "Call accepted successfully."
        };
    }
    public async Task<BaseResponse> RejectCallAsync(int callId, int personId)
    {
        var call = await _callRepo.GetByIdAsync(callId);
        if (call == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Call not found."
            };
        }
        var participant = call.Participants.FirstOrDefault(x => x.PersonId == personId);
        if (participant == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "User is not a participant of this call."
            };
        }
        participant.Status = CallParticipantStatus.Declined;
        if (!call.Chat.IsGroup)
        {
            call.Status = CallStatus.Rejected;
            call.EndedAt = DateTime.UtcNow;
        }
        else
        {
            bool hasActiveParticipants = call.Participants.Any(p => p.Status == CallParticipantStatus.Ringing || p.Status == CallParticipantStatus.Connected);
            if (!hasActiveParticipants)
            {
                call.Status = CallStatus.Rejected;
                call.EndedAt = DateTime.UtcNow;
            }
        }
        await _callRepo.Update(call);
        await _realtimeNotificationService.UpdateChat("CallRejected", call.ChatId, new
        {
            CallId = call.Id,
            PersonId = personId,
            Status = participant.Status,
            CallStatus = call.Status
        });
        return new BaseResponse
        {
            Status = true,
            Message = "Call rejected successfully."
        };
    }
    public async Task<bool> IsParticipantAsync(int callId, int personId)
    {
        var call = await _chatRepo.GetChatWithParticipantsAsync(callId);
        return call?.Participants.Any(x => x.PersonId == personId) ?? false;
    }
    public async Task<List<CallLogDto>> GetCallLogsAsync(int personId, int page, int pageSize)
    {
        var calls = await _callRepo.GetCallLogsAsync(personId, page, pageSize);
        
        return calls.Select(call =>
        {
            var duration = call.EndedAt.HasValue? call.EndedAt.Value - call.StartedAt : TimeSpan.Zero;
            var myParticipant = call.Participants.First(p => p.PersonId == personId);
            return new CallLogDto
            {
                CallId = call.Id,
                ChatId = call.ChatId,
                ChatName = call.Chat.Name,
                Type = call.Type,
                Status = call.Status,
                StartedAt = call.StartedAt,
                EndedAt = call.EndedAt,
                Duration = duration,
                CallerPersonId = call.CallerPersonId,
                IsIncoming = call.CallerPersonId != personId,
                IsMissed = myParticipant.Status == CallParticipantStatus.Missed  
            };
        }).ToList();
    }
    public async Task<BaseResponse> EndCallAsync(int callId, int personId)
    {
        var call = await _callRepo.GetByIdAsync(callId);
        if (call == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "Call not found."
            };
        }
        var participant = call.Participants.FirstOrDefault(x => x.PersonId == personId);
        if (participant == null)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "User is not a participant of this call."
            };
        }
        participant.Status = CallParticipantStatus.Left;
        if (!call.Chat.IsGroup)
        {
            call.Status = CallStatus.Ended;
            call.EndedAt = DateTime.UtcNow;
        }
        else
        {
            bool hasConnectedParticipants = call.Participants.Any(p => p.Status == CallParticipantStatus.Connected);
            if (!hasConnectedParticipants)
            {
                call.Status = CallStatus.Ended;
                call.EndedAt = DateTime.UtcNow;
            }
        }
        await _callRepo.Update(call);
        await _realtimeNotificationService.UpdateChat("CallEnded", call.ChatId, new
        {
            CallId = call.Id,
            PersonId = personId,
            Status = participant.Status,
            CallStatus = call.Status
        });
        return new BaseResponse
        {
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
            Participants = call.Participants.Select(p => MapToDto(p)).ToList()
        };
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
            Status = participant.Status
        };
    }
}