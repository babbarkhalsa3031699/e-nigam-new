
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("6543210987654321"); // 16 bytes

    public static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

               
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}


