using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/login")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private UsuarioRepository _usuarioRepository;

        public LoginController(ApplicationDbContext context, UsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuarioAsync([FromBody] Usuario usuario)
        {
            var usuarioExiste = await _usuarioRepository.BuscarUsuarioPorNomeUsuarioAsync(
                usuario.NomeUsuario
            );
            if (usuarioExiste != null)
            {
                return BadRequest("Nome de usuário já existe");
            }
            try
            {
                var usuarioCriado = await _usuarioRepository.CriarUsuarioAsync(
                    usuario.NomeUsuario,
                    usuario.SenhaHash
                );
                return Ok(usuarioCriado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
