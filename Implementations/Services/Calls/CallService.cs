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
    IConfiguration _callTimeOut;
    public CallService(ICallRepo callRepo, IChatRepo chatRepo, IChatHub realtimeNotificationService, IConfiguration config)
    {
        _callRepo = callRepo;
        _chatRepo = chatRepo;
        _realtimeNotificationService = realtimeNotificationService;
        _callTimeOut = config.GetSection("CallSettings");
    }
    public async Task ExecuteAllAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckTimedOutCallsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            await Task.Delay(TimeSpan.FromSeconds(Double.Parse(_callTimeOut["RingTimeoutSeconds"])),stoppingToken);
        }
    }
    public async Task CheckTimedOutCallsAsync()
    {
        var calls = await _callRepo.GetTimedOutRingingCallsAsync(TimeSpan.FromSeconds(30));
        if(calls != null)
        {

            
            foreach (var call in calls)
            {
                foreach (var participant in call.Participants)
                {
                    if (participant.Status == CallParticipantStatus.Ringing)
                    {
                        participant.Status = CallParticipantStatus.Missed;
                    }
                }
                bool hasConnectedParticipants = call.Participants.Any(p => p.Status == CallParticipantStatus.Connected);
                if (!hasConnectedParticipants)
                {
                    call.Status = CallStatus.Missed;
                    call.EndedAt = DateTime.UtcNow;
                }
                await _callRepo.Update(call);
                var callDto = MapToDto(await _callRepo.GetCallDetails(call.Id));
                await _realtimeNotificationService.UpdateChat("CallTimedOut", call.ChatId, callDto);
            }
        }
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
        var callDto = MapToDto(await _callRepo.GetCallDetails(call.Id));
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
        await _realtimeNotificationService.UpdateChat("CallAccepted", call.ChatId, MapToDto(await _callRepo.GetCallDetails(call.Id)));
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
        if (call.Status != CallStatus.Ringing)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "The call can no longer be rejected."
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
        var callDto = MapToDto(await _callRepo.GetCallDetails(call.Id));
        await _realtimeNotificationService.UpdateChat("CallRejected", call.ChatId, callDto);
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
            var chatName = call.Chat.Name;
            if(!call.Chat.IsGroup){
                var chat = call.Chat.Participants.FirstOrDefault(x => x.PersonId != personId);
                chatName = $"{chat.Person.PersonDetails.FirstName} {chat.Person.PersonDetails.LastName}";
            }
            return new CallLogDto
            {
                CallId = call.Id,
                ChatId = call.ChatId,
                ChatName = chatName,
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
        if (call.Status != CallStatus.Connected)
        {
            return new BaseResponse
            {
                Status = false,
                Message = "The call is not active."
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
        bool callEnded = false;
        if (!call.Chat.IsGroup)
        {
            call.Status = CallStatus.Ended;
            call.EndedAt = DateTime.UtcNow;
            foreach (var p in call.Participants.Where(p => p.Status == CallParticipantStatus.Connected)) p.Status = CallParticipantStatus.Left;
            callEnded = true;

        }
        else
        {
            bool hasConnectedParticipants = call.Participants.Any(p => p.Status == CallParticipantStatus.Connected);
            if (!hasConnectedParticipants)
            {
                foreach (var p in call.Participants.Where(p => p.Status == CallParticipantStatus.Ringing)) p.Status = CallParticipantStatus.Missed;
                call.Status = CallStatus.Ended;
                call.EndedAt = DateTime.UtcNow;
                callEnded = true;
            }
        }
        await _callRepo.Update(call);
        var updatedCall = await _callRepo.GetCallDetails(call.Id);
        var callDto = MapToDto(updatedCall);
        if (callEnded) await _realtimeNotificationService.UpdateChat("CallEnded", call.ChatId, callDto);
        else await _realtimeNotificationService.UpdateChat("ParticipantLeft", call.ChatId, callDto);
        return new BaseResponse
        {
            Status = true,
            Message = callEnded ? "Call ended successfully." : "You left the call successfully."
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