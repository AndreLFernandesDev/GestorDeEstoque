using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuarioAsync([FromBody] UsuarioDTO usuario)
        {
            var usuarioExistente = await _usuarioRepository.BuscarUsuarioPorEmailAsync(
                usuario.Email
            );
            if (usuarioExistente != null)
            {
                return BadRequest("Este email já foi cadastrado");
            }
            try
            {
                var usuarioCriado = await _usuarioRepository.CriarUsuarioAsync(
                    usuario.Email,
                    usuario.Senha
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
