using Home_Security.Entities.Calls;
namespace Home_Security.Models.CallDTO;
public class CallDto
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int CallerPersonId { get; set; }
    public CallType Type { get; set; }
    public CallStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? AnsweredAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public List<CallParticipantDto> Participants { get; set; } = new();
}
public class CallParticipantDto
{
    public int PersonId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;
    public bool IsMuted { get; set; }
    public bool IsCameraEnabled { get; set; }
    public bool IsScreenSharing { get; set; }
    public CallParticipantStatus Status { get; set; }
}
public class CallLogDto
{
    public int CallId { get; set; }
    public int ChatId { get; set; }
    public string ChatName { get; set; } = "";
    public CallType Type { get; set; }
    public CallStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsIncoming { get; set; }
    public int CallerPersonId { get; set; }
    public bool IsMissed { get; set; }
}
public class StartCallDto
{
    public int ChatId { get; set; }
    public CallType Type { get; set; }
}
public class OfferDto
{
    public int CallId { get; set; }
    public int ChatId { get; set; }
    public int PersonId { get; set; }
    public string Sdp { get; set; } = string.Empty;
}
public class AnswerDto
{
    public int CallId { get; set; }
    public int ChatId { get; set; }
    public int PersonId { get; set; }
    public string Sdp { get; set; } = string.Empty;
}
public class IceCandidateDto
{
    public int CallId { get; set; }
    public int ChatId { get; set; }
    public int PersonId { get; set; }
    public string Candidate { get; set; } = string.Empty;
    public string? SdpMid { get; set; }
    public int? SdpMLineIndex { get; set; }
}
public class ToggleCallItemsDto
{
    public int CallId { get; set; }
    public int ChatId { get; set; }
    public int PersonId { get; set; }
    public bool IsMuted { get; set; }
    public bool IsCameraEnabled { get; set; }
    public bool IsSharing { get; set; }
}