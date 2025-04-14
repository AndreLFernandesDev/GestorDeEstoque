namespace GestorDeEstoque.DTOs
{
    public class UsuarioDTO
    {
        public required string Email { get; set; } = string.Empty;
        public required string SenhaHash { get; set; } = string.Empty;
    }
}
