namespace LivroDeReceitas.Domain.Entidades;

public class User : EntityBase
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public string Telefone { get; set; } = null!;
}
