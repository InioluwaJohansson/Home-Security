namespace Home_Security.Entities.Chats;
using Home_Security.Contracts;
public class Chat : AuditableEntity
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool IsGroup { get; set; }

    public string? ImageUrl { get; set; }

    public int? CreatedByPersonId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}