using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserControler : ControllerBase
    {
        private UserRepository _userRepository;

        public UserControler(UserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.Get();
            return Ok(users);
        }
    }
}