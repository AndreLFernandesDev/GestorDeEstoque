using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserRepository : ControllerBase
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }
    }
}