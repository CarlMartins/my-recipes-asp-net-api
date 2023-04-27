namespace RecipeBook.Domain.Entities;

public class EntityBase
{
    public virtual long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}