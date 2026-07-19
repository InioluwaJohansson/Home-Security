using Home_Security.Entities.Chats;

namespace Home_Security.Models.ChatDTO;
public class SendMessageDto
{
    public int ChatId { get; set; }
    public string? Content { get; set; }
    public int? ReplyToMessageId { get; set; }
    public List<MessageAttachmentDto>? Attachments { get; set; } = new();
}
public class MessageAttachmentDto
{
    public string FileName { get; set; }
    public IFormFile FilePath { get; set; }
    public MessageType Type { get; set; }
    public long FileSize { get; set; }
    public string? ThumbnailPath { get; set; }
}
public class GetMessageAttachmentDto
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string ContentType { get; set; }
    public long FileSize { get; set; }
    public string? ThumbnailPath { get; set; }
}
public class EditMessageDto
{
    public int MessageId { get; set; }
    public string Content { get; set; }
}
public class MessageDto
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int SenderPersonId { get; set; }
    public int? ReplyMessageId { get; set; }
    public string SenderName { get; set; }
    public string? SenderProfileImage { get; set; }
    public string? Content { get; set; }
    public MessageType Type { get; set; }
    public bool IsEdited { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime SentAt { get; set; }
    public List<GetMessageAttachmentDto> Attachments { get; set; } = new();
}
public class TypingDto
{
    public int ChatId { get; set; }
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Action { get; set; }
    public bool IsTyping { get; set; }
}