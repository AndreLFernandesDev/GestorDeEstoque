namespace GestorDeEstoque.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Email { get; set; } = string.Empty;
        public required string SenhaHash { get; set; } = string.Empty;
    }
}
