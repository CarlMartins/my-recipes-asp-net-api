using System.Reflection;
using FluentMigrator.Runner;
using LivroDeReceitas.Domain.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LivroDeReceitas.Infrastructure;

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
                .ScanIn(Assembly.Load("LivroDeReceitas.Infrastructure"))
                .For.Migrations());
    }
}