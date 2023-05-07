using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Extensions;
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.UserRepositories;
using RecipeBook.Infrastructure.RepositoryAccess;
using RecipeBook.Infrastructure.RepositoryAccess.Repositories;

namespace RecipeBook.Infrastructure;

public static class Bootstrapper
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentMigrator(services, configuration);
        AddContext(services, configuration);
        AddRepositories(services);
        AddUnitOfWork(services);
    }

    private static void AddContext(IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration.GetInMemoryDatabaseSetting(), out var inMemoryDatabase);

        if (inMemoryDatabase) return;
        
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 26));
        var connectionString = configuration.GetCompleteConnectionString();
            
        services.AddDbContext<MyRecipesContext>(options =>
        {
            options.UseMySql(connectionString, serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services
            .AddScoped<IUserWriteOnlyRepository, UserRepository>()
            .AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration.GetInMemoryDatabaseSetting(), out var inMemoryDatabase);
        
        if (inMemoryDatabase) return;
        
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(x => x.AddMySql5()
                .WithGlobalConnectionString(configuration.GetCompleteConnectionString())
                .ScanIn(Assembly.Load("RecipeBook.Infrastructure"))
                .For.Migrations());
    }
}