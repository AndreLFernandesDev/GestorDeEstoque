using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        public Task<Produto> AtualizarQuantidadeProdutoAsync(int idProduto, decimal quantidade);
    }
}