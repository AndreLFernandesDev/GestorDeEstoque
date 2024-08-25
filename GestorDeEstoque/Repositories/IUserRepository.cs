using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IUserRepository
    {
        List<User> Get();
    }
}