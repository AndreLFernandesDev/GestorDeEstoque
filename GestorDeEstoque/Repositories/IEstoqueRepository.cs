using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        public Task<IEnumerable<EstoqueDTO>> ListarEstoquesAsync();

        public Task<Estoque> BuscarEstoquePorIdAsync(int id);

        public Task<Estoque> AdicionarEstoqueAsync(Estoque novoEstoque);

        public Task<Estoque> AtualizarEstoqueAsync(int idEstoque, Estoque estoqueAtualizado);

        public Task<bool> RemoverEstoqueAsync(int idEstoque);
    }
}
