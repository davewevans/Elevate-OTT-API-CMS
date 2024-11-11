namespace OttApiPlatform.Application.Common.Contracts;

public interface ICryptoService
{
    string EncryptText(string textToEncrypt);
    string DecryptText(string cipheredText);
}
