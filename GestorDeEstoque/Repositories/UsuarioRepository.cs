using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;
using Bcrypt = BCrypt.Net.BCrypt;

namespace GestorDeEstoque.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> BuscarUsuarioPorEmailAsync(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return null!;
            }
            return usuario;
        }

        public async Task<Usuario> CriarUsuarioAsync(string email, string senha)
        {
            {
                var senhaHash = Bcrypt.HashPassword(senha);
                var usuario = new Usuario { Email = email, SenhaHash = senhaHash };
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
        }
    }
}
