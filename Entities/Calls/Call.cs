using Home_Security.Contracts;

namespace Home_Security.Entities.Calls;
public class Call : AuditableEntity
{
    public int ChatId { get; set; }
    public int CallerPersonId { get; set; }
    public CallType Type { get; set; }
    public CallStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? AnsweredAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public ICollection<CallParticipant> Participants { get; set; } = new List<CallParticipant>();
}