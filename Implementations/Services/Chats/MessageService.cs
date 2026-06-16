using Home_Security.Interfaces.Services.Chats;
using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Entities.Chats;
public class MessageService : IMessageService
{
    private readonly IMessageRepo _messageRepo;
    private readonly IChatRepo _chatRepo;

    public MessageService(IMessageRepo messageRepo, IChatRepo chatRepo)
    {
        _messageRepo = messageRepo;
        _chatRepo = chatRepo;
    }
    public async Task<MessageDto> SendMessageAsync(SendMessageDto dto, int senderId)
    {
        var isParticipant = await _chatRepo.IsParticipantAsync(dto.ChatId, senderId);

        if (!isParticipant)
            throw new Exception("Not allowed in this chat");
        var date = DateTime.UtcNow;
        var message = new Message
        {
            ChatId = dto.ChatId,
            SenderPersonId = senderId,
            ReplyToMessageId = dto.ReplyToMessageId,
            Content = dto.Content,
            Type = dto.Type,
            SentAt = date,
            Attachments = (await Task.WhenAll(
        dto.Attachments.Select(async a => new MessageAttachment
        {
            FileName = a.FileName,
            FilePath = await GetAttachmentUrl(a.FilePath, dto.Type),
            ContentType = a.ContentType,
            FileSize = a.FileSize,
            ThumbnailPath = a.ThumbnailPath
        }))).ToList()
        };

        await _messageRepo.Create(message);
        var messageData = await _messageRepo.Get(x => x.SentAt == date);
        return MapToDto(messageData);
    }
    public async Task<List<MessageDto>> GetChatMessagesAsync(int chatId, int personId, int page, int pageSize)
    {
        var isParticipant = await _chatRepo.IsParticipantAsync(chatId, personId);
        if (!isParticipant)
            throw new Exception("Access denied");
        var messages = await _messageRepo.GetChatMessagesAsync(chatId, page, pageSize);
        return messages.Select(MapToDto).ToList();
    }
    public async Task<MessageDto> EditMessageAsync(EditMessageDto dto, int personId)
    {

        var message = await _messageRepo.GetByIdAsync(dto.MessageId);
        if (message == null)
            throw new Exception("Message not found");

        if (message.SenderPersonId != personId)
            throw new Exception("Not allowed");

        message.Content = dto.Content;
        message.IsEdited = true;

        await _messageRepo.Update(message);
        return MapToDto(message);
    }
    public async Task DeleteMessageAsync(int messageId, int personId)
    {

        var message = await _messageRepo.GetByIdAsync(messageId);

        if (message == null)
            throw new Exception("Message not found");

        if (message.SenderPersonId != personId)
            throw new Exception("Not allowed");

        message.IsDeleted = true;

        await _messageRepo.Update(message);
    }
    public async Task<int> GetUnreadCountAsync(int chatId, int personId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        var participant = chat.Participants.FirstOrDefault(x => x.PersonId == personId);

        return await _messageRepo.GetUnreadCountAsync(chatId, participant?.LastReadAt);
    }
    private MessageDto MapToDto(Message message)
    {
        return new MessageDto
        {
            Id = message.Id,
            ChatId = message.ChatId,
            SenderPersonId = message.SenderPersonId,
            ReplyMessageId = message.ReplyToMessageId,
            Content = message.Content,
            Type = message.Type,
            IsEdited = message.IsEdited,
            IsDeleted = message.IsDeleted,
            SentAt = message.SentAt,
            SenderName = $"{message.SenderPerson.PersonDetails.FirstName} {message.SenderPerson.PersonDetails.LastName}",
            SenderProfileImage = ConvertImageToBase64(message.SenderPerson.PersonDetails.ImageUrl) ?? "",
            Attachments = message.Attachments?
                .Select(a => new GetMessageAttachmentDto
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath,
                    ContentType = a.ContentType,
                    FileSize = a.FileSize,
                    ThumbnailPath = a.ThumbnailPath
                })
                .ToList() ?? new List<GetMessageAttachmentDto>()
        };
    }
    public static string? ConvertImageToBase64(string? path)
    {
        if (!File.Exists(path))
            return null;
        var bytes = File.ReadAllBytes(path);
        return Convert.ToBase64String(bytes);
    }
    public async Task<string> GetAttachmentUrl(IFormFile file, MessageType type)
    {
        if (file == null || file.Length == 0)
            return string.Empty;
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var storagePath = Path.Combine(basePath, "storage");
        var folderName = type switch
        {
            MessageType.Image => "Images",
            MessageType.Video => "Videos",
            MessageType.Audio => "Audios",
            MessageType.File => "Files",
            _ => "Others"
        };
        var folderPath = Path.Combine(storagePath, folderName);
        Directory.CreateDirectory(folderPath);
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(folderPath, uniqueFileName);
        await using var stream = new FileStream(absolutePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return Path.Combine(folderName, uniqueFileName);
    }
}