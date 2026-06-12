using System.Security.Cryptography;
using System.Text;
namespace Home_Security.Envryption;
public static class EncryptionHelper
{
    public static string Encrypt(string plainText, string key)
    {
        using Aes aes = Aes.Create();

        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();

        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = encryptor.TransformFinalBlock(
            plainBytes,
            0,
            plainBytes.Length);

        byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];

        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(
            encryptedBytes,
            0,
            result,
            aes.IV.Length,
            encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    public static string Decrypt(string cipherText, string key)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();

        aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));

        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(
            fullCipher,
            iv.Length,
            cipher,
            0,
            cipher.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();

        byte[] decryptedBytes = decryptor.TransformFinalBlock(
            cipher,
            0,
            cipher.Length);

        return Encoding.UTF8.GetString(decryptedBytes);
    }
    using System.Text.Json;

public static async Task SaveEncryptedUser(
    CreatePersonDto person,
    string filePath,
    string key)
{
    var json = JsonSerializer.Serialize(person);

    var encrypted = EncryptionHelper.Encrypt(json, key);

    await File.WriteAllTextAsync(filePath, encrypted);
}
  using System.Text.Json;

public static async Task<CreatePersonDto?> ReadEncryptedUser(
    string filePath,
    string key)
{
    var encrypted = await File.ReadAllTextAsync(filePath);

    var json = EncryptionHelper.Decrypt(encrypted, key);

    return JsonSerializer.Deserialize<CreatePersonDto>(json);
}
}