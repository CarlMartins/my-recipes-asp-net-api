using RecipeBook.Api.Filters;
using RecipeBook.Application;
using RecipeBook.Application.Services.AutoMapper;
using RecipeBook.Domain.Extensions;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.Migrations;
using RecipeBook.Infrastructure.RepositoryAccess;
using RecipeBook.Infrastructure.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.SetupSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMvc(option => { option.Filters.Add(typeof(ExceptionFilter)); });

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguration());
}).CreateMapper());

builder.Services.AddScoped<AuthenticatedUserAttribute>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

UpdateDatabase();

app.Run();

void UpdateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<MyRecipesContext>();
    
    var databaseInMemory = context?.Database.ProviderName?
        .Equals("Microsoft.EntityFrameworkCore.InMemory", StringComparison.InvariantCultureIgnoreCase);
    
    if (databaseInMemory.HasValue && databaseInMemory.Value) 
        return;
    
    var connectionString = builder.Configuration.GetConnetion();
    var databaseName = builder.Configuration.GetDatabaseName();

    Database.CriarDatabase(connectionString, databaseName);

    app.MigrateDatabase();
}

public partial class Program { }