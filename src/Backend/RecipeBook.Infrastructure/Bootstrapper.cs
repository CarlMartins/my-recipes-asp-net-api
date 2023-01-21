using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Extensions;

namespace RecipeBook.Infrastructure;

public static class Bootstrapper
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentMigrator(services, configuration);
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(x => x.AddMySql5()
                .WithGlobalConnectionString(configuration.GetCompleteConnectionString())
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.Migrations());
    }
}