using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Domain.Entities;
using RecipeBook.Infrastructure.RepositoryAccess;

namespace WebApi.Test;

public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private User _user = null!;
    private string _password = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MyRecipesContext>));
                
                if (descriptor != null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<MyRecipesContext>(opt =>
                {
                    opt.UseInMemoryDatabase("InMemoryDbForTesting");
                    opt.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var database = scope.ServiceProvider.GetRequiredService<MyRecipesContext>();

                database.Database.EnsureDeleted();

                (_user, _password) =  ContextSeedInMemory.Seed(database);
            });
    }
    
    public User GetUser()
    {
        return _user;
    }
    
    public string GetPassword()
    {
        return _password;
    }
}