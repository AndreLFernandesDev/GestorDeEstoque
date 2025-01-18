using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        public Task<IEnumerable<EstoqueDTO>> ListarEstoquesAsync();
    }
}
