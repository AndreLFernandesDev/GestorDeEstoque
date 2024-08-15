using GestorDeEstoque.Models;

namespace GestorDeEstoque.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Users> Get()
        {
            return _context.Users.ToList();
        }
    }
}
