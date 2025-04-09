using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDeEstoque.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> BuscarUsuarioPorNomeUsuarioAsync(string nomeUsuario)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.NomeUsuario == nomeUsuario
            );
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado");
            }
            return usuario;
        }

        public async Task<Usuario> CriarUsuarioAsync(string nomeUsuario, string senha)
        {
            {
                var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
                var usuario = new Usuario { NomeUsuario = nomeUsuario, SenhaHash = senhaHash };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
        }
    }
}
