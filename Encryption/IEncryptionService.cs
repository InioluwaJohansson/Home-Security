using Home_Security.Models.DTOs;

namespace Home_Security.Encryption;
public interface IEncryptionService
{
    Task EncryptFileAsync(CreatePersonDto createPersonDto);
    CreatePersonDto? DecryptEncryptionKeyAsync(string encryptionKey);
    Task<CreatePersonDto?> DecryptObjectAsync();
    void DeleteFile();
}