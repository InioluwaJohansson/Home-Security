using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Home_Security.Models.DTOs;

namespace Home_Security.Encryption;
public class EncryptionService : IEncryptionService
{
    private readonly IConfiguration _configuration;
    IConfiguration _encryptionKey;
    public EncryptionService(IConfiguration configuration)
    {
        _encryptionKey = configuration.GetSection("Encryption");
    }
    public async Task EncryptFileAsync(CreatePersonDto createPersonDto)
    {
        string json = JsonSerializer.Serialize(createPersonDto);
        string encrypted = Encrypt(json);
        var storagePath = Path.Combine(Directory.GetCurrentDirectory(), _encryptionKey["Path"]);
        if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
        string encryptedPath = Path.ChangeExtension(storagePath,".homeSecurity");
        await File.WriteAllTextAsync(encryptedPath, encrypted);
    }
    public async Task<CreatePersonDto?> DecryptObjectAsync()
    {
        if (!File.Exists(_encryptionKey["Path"]))return default;
        string encrypted = await File.ReadAllTextAsync(_encryptionKey["Path"].Replace(".data",".homeSecurity"));
        string json = Decrypt(encrypted);
        return JsonSerializer.Deserialize<CreatePersonDto>(json);
    }
    public void DeleteFile()
    {
        var encryptedPath = Path.Combine(Directory.GetCurrentDirectory(), _encryptionKey["Path"].Replace(".data",".homeSecurity"));
        if (File.Exists(encryptedPath)) File.Delete(encryptedPath);
    }
    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(_encryptionKey["Key"]));
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        var result = new byte[aes.IV.Length + encryptedBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
        return Convert.ToBase64String(result);
    }
    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(_encryptionKey["Key"]));
        var iv = new byte[16];
        var cipher = new byte[fullCipher.Length - iv.Length];
        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor();
        var decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}