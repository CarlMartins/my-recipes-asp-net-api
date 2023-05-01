using System.Security.Cryptography;
using System.Text;

namespace RecipeBook.Application.Services.Cryptography;

public class PasswordEncryptor : IPasswordEncryptor
{
    private readonly string _salt;

    public PasswordEncryptor(string salt)
    {
        _salt = salt;
    }

    public string Encrypt(string password)
    {
        var saltedPassword = $"{password}{_salt}";
        
        var bytes = Encoding.UTF8.GetBytes(saltedPassword);
        var sha512 = SHA512.Create();
        var hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] hashBytes)
    {
        var builder = new StringBuilder();
        foreach (var b in hashBytes)
        {
            var hex = b.ToString("x2");
            builder.Append(hex);
        }

        return builder.ToString();
    }
}