using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> BuscarUsuarioPorEmailAsync(string email);
        Task<Usuario> CriarUsuarioAsync(string email, string senha);
    }
}
