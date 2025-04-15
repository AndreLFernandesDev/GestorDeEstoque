using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> BuscarUsuarioPorEmailAsync(string nomeUsuario);
        Task<Usuario> CriarUsuarioAsync(string email, string senha);
    }
}
