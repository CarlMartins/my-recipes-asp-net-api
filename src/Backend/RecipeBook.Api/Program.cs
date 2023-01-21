using RecipeBook.Api.Filters;
using RecipeBook.Domain.Extensions;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMvc(option =>
{
    option.Filters.Add(typeof(ExceptionFilter));
});

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
    var connectionString = builder.Configuration.GetConnetion();
    var databaseName = builder.Configuration.GetDatabaseName();
    
    
    Database.CriarDatabase(connectionString, databaseName);
    
    app.MigrateDatabase();
}