using OttApiPlatform.Application.Common.Contracts;

namespace OttApiPlatform.Infrastructure.Services.Security;

public class EncryptionService : ICryptoService
{
    private readonly byte[] _key;

    public EncryptionService(IConfiguration configuration)
    {
        _key = Encoding.UTF8.GetBytes(configuration["EncryptionKey"] ?? string.Empty);
    }

    public string EncryptText(string textToEncrypt)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(textToEncrypt);
        }

        var encryptedContent = ms.ToArray();
        return Convert.ToBase64String(iv.Concat(encryptedContent).ToArray());
    }

    public string DecryptText(string cipheredText)
    {
        var fullCipher = Convert.FromBase64String(cipheredText);
        using var aes = Aes.Create();
        aes.Key = _key;
        var iv = fullCipher.Take(aes.BlockSize / 8).ToArray();
        var cipher = fullCipher.Skip(aes.BlockSize / 8).ToArray();

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        {
            return sr.ReadToEnd();
        }
    }
}

