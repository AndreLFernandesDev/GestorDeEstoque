using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestorDeEstoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private UsuarioRepository _usuarioRepository;

        public LoginController(IConfiguration configuration, UsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDTO usuario)
        {
            var usuarioEncontrado = await _usuarioRepository.BuscarUsuarioPorEmailAsync(
                usuario.Email
            );

            if (
                usuarioEncontrado == null
                || !BCrypt.Net.BCrypt.Verify(usuario.Senha, usuarioEncontrado.SenhaHash)
            )
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );
                var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(
                        Convert.ToDouble(_configuration["Jwt:ExpireHours"])
                    ),
                    signingCredentials: credenciais
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.WriteToken(token);

                return Ok(new { Token = jwtToken });
            }
        }
    }
}
