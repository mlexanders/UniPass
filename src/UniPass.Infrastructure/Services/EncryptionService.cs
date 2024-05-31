using System.Security.Cryptography;
using System.Text;
using UniPass.Infrastructure.Models;

namespace UniPass.Infrastructure.Services;

public static class EncryptionService
{
    private static readonly byte[] EncryptionKey = Convert.FromBase64String("RYOOciUTSn48zDqf5wdm/c394rDwpdPVmjvv1cAgbf8=");

    public static void Encrypt(List<Key>? keys)
    {
        if (keys is null)  return;
        
        foreach (var key in keys)
        {
            Encrypt(key);
        }
    }
    
    public static void Decrypt(List<Key>? keys)
    {
        if (keys is null)  return;
        
        foreach (var key in keys)
        {
            Decrypt(key);
        }
    }

    private static void Encrypt(Key? key)
    {
        if (key is null)  return;

        key.Login = Encrypt(key.Login);
        key.Password = Encrypt(key.Password);
    }

    private static void Decrypt(Key? key)
    {
        if (key is null)  return;

        key.Login = Decrypt(key.Login);
        key.Password = Decrypt(key.Password);
    }

    private static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = EncryptionKey;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var result = new byte[iv.Length + encryptedBytes.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

        return Convert.ToBase64String(result);
    }

    private static string Decrypt(string encryptedText)
    {
        var fullCipher = Convert.FromBase64String(encryptedText);

        using var aes = Aes.Create();
        aes.Key = EncryptionKey;

        var iv = new byte[aes.BlockSize / 8];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        using var decryptor = aes.CreateDecryptor(aes.Key, iv);
        var decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}