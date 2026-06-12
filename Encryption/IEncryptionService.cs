using Home_Security.Models.DTOs;

namespace Home_Security.Encryption;
public interface IEncryptionService
{
    Task EncryptFileAsync(CreatePersonDto createPersonDto);
    Task<CreatePersonDto?> DecryptObjectAsync();
    void DeleteFile();
}