using RecipeBook.Application.Services.Cryptography;

namespace TestHelpers.Encryptor;

public class PasswordEncryptorBuilder
{
    public static PasswordEncryptor Instance()
    {
        return new PasswordEncryptor("ABCD123");
    }
}