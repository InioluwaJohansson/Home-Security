using Home_Security.Entities.Chats;
using Home_Security.Contracts;

namespace Home_Security.Entities.Chats;
public class Message : AuditableEntity
{
    public int ChatId { get; set; }
    public Chat Chat { get; set; }

    public int SenderPersonId { get; set; }

    public string? Content { get; set; }

    public MessageType Type { get; set; }

    public bool IsEdited { get; set; }

    public int? ReplyToMessageId { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public ICollection<MessageAttachment> Attachments { get; set; } = new List<MessageAttachment>();
    public Person SenderPerson { get; set; }
}