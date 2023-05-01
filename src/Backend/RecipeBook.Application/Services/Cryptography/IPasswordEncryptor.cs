namespace RecipeBook.Application.Services.Cryptography;

public interface IPasswordEncryptor
{
    string Encrypt(string password);
}