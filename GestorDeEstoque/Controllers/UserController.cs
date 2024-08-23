using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserControlers : ControllerBase
    {
        private UserRepository _db;

        public UserControlers(UserRepository db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _db.Get();
            return Ok(users);
        }
    }
}