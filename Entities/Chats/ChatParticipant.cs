using Home_Security.Contracts;

namespace Home_Security.Entities.Chats;
public class ChatParticipant : AuditableEntity
{
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    public int PersonId { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsMuted { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReadAt { get; set; }
}