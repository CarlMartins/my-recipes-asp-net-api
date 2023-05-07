using Microsoft.Extensions.Configuration;

namespace RecipeBook.Domain.Extensions;

public static class RepositoryExtension
{
    public static string GetDatabaseName(this IConfiguration configuration)
    {
        var databaseName = configuration.GetConnectionString("DatabaseName");

        return databaseName ?? string.Empty;
    }

    public static string GetConnetion(this IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Connection");

        return connection ?? string.Empty;
    }

    public static string GetCompleteConnectionString(this IConfiguration configuration)
    {
        var connection = configuration.GetConnetion();
        var databaseName = configuration.GetDatabaseName();

        return $"{connection}Database={databaseName}";
    }
    
    public static string GetInMemoryDatabaseSetting(this IConfiguration configuration)
    {
        var inMemoryDatabase = configuration.GetSection("Settings.InMemoryDatabase").Value;

        return inMemoryDatabase ?? string.Empty;
    }
}