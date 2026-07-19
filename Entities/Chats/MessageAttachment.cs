using Home_Security.Contracts;
using Home_Security.Entities.Chats;

namespace Home_Security.Entities.Chats;
public class MessageAttachment : AuditableEntity
{
    public int MessageId { get; set; }
    public Message Message { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public string ContentType { get; set; }

    public long FileSize { get; set; }

    public string? ThumbnailPath { get; set; }
}