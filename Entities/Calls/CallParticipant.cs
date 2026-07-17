using Home_Security.Contracts;

namespace Home_Security.Entities.Calls;
public class CallParticipant : AuditableEntity
{
    public int CallId { get; set; }
    public Call Call { get; set; } = null!;
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    public bool IsMuted { get; set; }
    public bool IsCameraEnabled { get; set; }
    public bool IsScreenSharing { get; set; }
    public CallParticipantStatus Status { get; set; }
}