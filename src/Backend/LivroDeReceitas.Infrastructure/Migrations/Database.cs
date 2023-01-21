using Dapper;
using MySqlConnector;

namespace LivroDeReceitas.Infrastructure.Migrations;

public static class Database
{
    public static void CriarDatabase(string connectionString, string databaseName)
    {
        using var connection = new MySqlConnection(connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", databaseName);

        var existingDatabase = connection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name",
            parameters);

        if (!existingDatabase.Any())
        {
            connection.Execute($"CREATE DATABASE {databaseName}");
        }
    }
}