using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> BuscarUsuarioPorNomeUsuarioAsync(string nomeUsuario);
        Task<Usuario> CriarUsuarioAsync(string nomeUsuario, string senha);
    }
}
