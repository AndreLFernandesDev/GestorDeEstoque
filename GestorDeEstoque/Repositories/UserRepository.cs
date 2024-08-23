using GestorDeEstoque.Data;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<User> Get()
        {
            return _context.Users.ToList();
        }
    }
}
