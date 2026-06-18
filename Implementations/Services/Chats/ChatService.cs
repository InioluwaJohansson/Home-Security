using Home_Security.Interfaces.Repositories.Chats;
using Home_Security.Interfaces.Services.Chats;
using Home_Security.Models.ChatDTO;
using Home_Security.Entities.Chats;
using Home_Security.Models.DTOs;
using Home_Security.RealTimeServices;
public class ChatService : IChatService
{
    private readonly IChatRepo _chatRepo;
    private IChatHub _chatHub;
    public ChatService(IChatRepo chatRepo, IChatHub chatHub)
    {
        _chatRepo = chatRepo;
        _chatHub = chatHub;
    }
    public async Task<ChatDto> CreateDirectChatAsync(int recipientPersonId, int currentPersonId)
    {

        var existingChat = await _chatRepo.GetDirectChatAsync(currentPersonId, recipientPersonId);
        if (existingChat != null)
            return MapToDto(existingChat);
        var chat = new Chat
        {
            IsGroup = false,
            CreatedByPersonId = currentPersonId,
            Participants = new List<ChatParticipant>
            {
                new ChatParticipant
                {
                    PersonId = currentPersonId
                },
                new ChatParticipant
                {
                    PersonId = recipientPersonId
                }
            }
        };
        await _chatRepo.Create(chat);
        return MapToDto(chat);
    }
    public async Task<ChatDto> CreateGroupChatAsync(CreateGroupChatDto dto, int currentPersonId)
    {
        var chat = new Chat
        {
            IsGroup = true,
            Name = dto.Name,
            Description = dto.Description,
            ImageUrl = await GetImageUrl(dto.ImageUrl ?? null),
            CreatedByPersonId = currentPersonId,
            Participants = dto.ParticipantPersonIds.Append(currentPersonId).Distinct()
            .Select(x => new ChatParticipant
            {
                PersonId = x,
                IsAdmin = x == currentPersonId
            }).ToList()
        };
        await _chatRepo.Create(chat);
        return MapToDto(chat);
    }
    public async Task<List<ChatDto>> GetMyChatsAsync(int personId)
    {
        var chats = await _chatRepo.GetPersonChatsAsync(personId);
        return chats.Select(MapToDto).ToList();
    }
    public async Task<ChatDto?> GetChatByIdAsync(int chatId, int personId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        if (chat == null)
            return null;

        var isParticipant = chat.Participants.Any(p => p.PersonId == personId);
        if (!isParticipant)
            throw new Exception("Access denied");
        return MapToDto(chat);
    }
    public async Task<BaseResponse> UpdateGroupChatAsync(int chatId, UpdateGroupChatDto dto, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);
        if (chat == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        if (!chat.IsGroup)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to update chat!"  
            };
        }
        var isAdmin = chat.Participants.Any(p => p.PersonId == currentPersonId && p.IsAdmin);
        if (!isAdmin)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Only admins can modify chat details!"  
            };
        }
        chat.Name = dto.Name ?? chat.Name;
        chat.Description = dto.Description ?? chat.Description;
        if (dto.ImageUrl != null)
            chat.ImageUrl = chat.ImageUrl ?? await GetImageUrl(dto.ImageUrl);
        await _chatRepo.Update(chat);
        {
            return new BaseResponse()
            {
              Status = true,
              Message = "Group Details Updated!"  
            };
        }
    }
    public async Task<BaseResponse> DeleteChatAsync(int chatId, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);
        if (chat == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        if (!chat.IsGroup)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Cannot add participants to direct chat!"  
            };
        }
        var isAdmin = chat.Participants.Any(p => p.PersonId == currentPersonId && p.IsAdmin);
        if (!isAdmin)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Only admins can add participants!"  
            };
        }
        chat.IsDeleted = true;
        await _chatRepo.Update(chat);
        return new BaseResponse()
        {
            Status = true,
            Message = "Chat Deleted!"  
        };
    }
    public async Task<BaseResponse> AddParticipantsAsync(int chatId, AddParticipantsDto dto, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);
        if (chat == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        if (!chat.IsGroup)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Cannot add participants to direct chat!"  
            };
        }
        var isAdmin = chat.Participants.Any(p => p.PersonId == currentPersonId && p.IsAdmin);
        if (!isAdmin)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Only admins can add participants!"  
            };
        }
        var existingIds = chat.Participants.Select(p => p.PersonId).ToHashSet();
        var newParticipants = dto.PersonIds.Where(id => !existingIds.Contains(id))
        .Select(id => new ChatParticipant
        {
            ChatId = chatId,
            PersonId = id,
            JoinedAt = DateTime.UtcNow,
            IsAdmin = false
        });
        foreach (var participant in newParticipants) chat.Participants.Add(participant);
        
        await _chatRepo.Update(chat);    
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable to find chat!"  
        };
    }
    public async Task<BaseResponse> RemoveParticipantAsync(int chatId, int personId, int currentPersonId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);

        if (chat == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        if (!chat.IsGroup)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable remove participants in group!"  
            };
        }
        var isSelfRemove = currentPersonId == personId;
        var isAdmin = chat.Participants.Any(p => p.PersonId == currentPersonId && p.IsAdmin);
        if (!isSelfRemove && !isAdmin)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        var participant = chat.Participants.FirstOrDefault(p => p.PersonId == personId);
        var adminCount = chat.Participants.Count(x => x.IsAdmin);
        if (participant == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find participant!"  
            };
        }
        if (participant.IsAdmin && adminCount == 1)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        chat.Participants.Remove(participant);
        await _chatRepo.Update(chat);
        {
            return new BaseResponse()
            {
              Status = true,
              Message = "Participant removed!"  
            };
        }
    }
    public async Task<BaseResponse> MarkAsReadAsync(int chatId, int personId)
    {
        var chat = await _chatRepo.GetChatWithParticipantsAsync(chatId);
        if (chat == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find chat!"  
            };
        }
        var participant = chat.Participants.FirstOrDefault(x => x.PersonId == personId);
        if (participant == null)
        {
            return new BaseResponse()
            {
              Status = false,
              Message = "Unable to find participant!"  
            };
        }
        participant.LastReadAt = DateTime.UtcNow;
        await _chatRepo.Update(chat);
        await _chatHub.UpdateChat("MessagesRead", chatId, new
        {
            ChatId = chatId,
            PersonId = personId,
            ReadAt = DateTime.UtcNow
        });
        return new BaseResponse()
        {
            Status = true,
            Message = "Marked as read!"  
        };
    }
    public async Task<BaseResponse> JoinChat(int chatId, int personId)
    {
        var isParticipant = await _chatRepo.IsParticipantAsync(chatId, personId);
        if (!isParticipant)
        {
            return new BaseResponse()
            {
                Status = false,
                Message = "You are not a participant of this chat!"
            };
        }
        await _chatHub.JoinChat(chatId);
        return new BaseResponse()
        {
            Status = true,
            Message = "Joined chat successfully!"
        };
    }
    public async Task LeaveChat(int chatId)
    {
        await _chatHub.LeaveChat(chatId);
    }
    public async Task<bool> IsParticipantAsync(int chatId, int personId)
    {
        return await _chatRepo.IsParticipantAsync(chatId, personId);
    }
    private ChatDto MapToDto(Chat chat)
    {
        return new ChatDto
        {
            Id = chat.Id,
            Name = chat.Name,
            Description = chat.Description,
            IsGroup = chat.IsGroup,
            ImageUrl = ConvertImageToBase64(chat.ImageUrl),
            CreatedAt = chat.CreatedAt,
            Participants = chat.Participants?
                .Select(p => new ChatParticipantDto
                {
                    PersonId = p.PersonId,
                    IsAdmin = p.IsAdmin,
                    IsOnline = false,
                    FullName = "",
                    ProfileImageUrl = null
                })
                .ToList() ?? new List<ChatParticipantDto>(),
            LastMessage = chat.Messages?
                .OrderByDescending(x => x.SentAt)
                .Select(x => new MessageDto
                {
                    Id = x.Id,
                    ChatId = x.ChatId,
                    SenderPersonId = x.SenderPersonId,
                    Content = x.Content,
                    Type = x.Type,
                    IsEdited = x.IsEdited,
                    IsDeleted = x.IsDeleted,
                    SentAt = x.SentAt,
                    SenderName = "",
                    SenderProfileImage = null,
                    Attachments = x.Attachments?
                    .Select(a => new GetMessageAttachmentDto
                    {
                        FileName = a.FileName,
                        FilePath = a.FilePath,
                        ContentType = a.ContentType,
                        FileSize = a.FileSize,
                        ThumbnailPath = a.ThumbnailPath
                    })
                    .ToList() ?? new List<GetMessageAttachmentDto>()
                }).FirstOrDefault(),
            UnreadCount = 0
        };
    }
    public static string? ConvertImageToBase64(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;
        if (!File.Exists(path))
            return null;
        var bytes = File.ReadAllBytes(path);
        return Convert.ToBase64String(bytes);
    }
    public async Task<string> GetImageUrl(IFormFile file){
        if (file == null || file.Length == 0)
            return string.Empty;
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var storagePath = Path.Combine(basePath, "storage");
        var folderPath = Path.Combine(storagePath, "Images");
        Directory.CreateDirectory(folderPath);
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid():N}{extension}";
        var absolutePath = Path.Combine(folderPath, uniqueFileName);
        await using var stream = new FileStream(absolutePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return Path.Combine("Images", uniqueFileName);
    }
}