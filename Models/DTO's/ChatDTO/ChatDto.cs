using Home_Security.Entities.Chats;
namespace Home_Security.Models.ChatDTO;
public class CreateGroupChatDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<int> ParticipantPersonIds { get; set; } = new();
}
public class UpdateGroupChatDto
{
    public int ChatId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
public class AddParticipantsDto
{
    public List<int> PersonIds { get; set; } = new();
}
public class ChatDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsGroup { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ChatParticipantDto> Participants { get; set; } = new();
    public MessageDto? LastMessage { get; set; }
    public int UnreadCount { get; set; }
}
public class ChatParticipantDto
{
    public int PersonId { get; set; }
    public string FullName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? LastReadAt { get; set; }
}