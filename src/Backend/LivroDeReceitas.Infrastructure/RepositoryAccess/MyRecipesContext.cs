using LivroDeReceitas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LivroDeReceitas.Infrastructure.RepositoryAccess;

public class MyRecipesContext : DbContext
{
    public MyRecipesContext(DbContextOptions<MyRecipesContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipesContext).Assembly);
    }
}