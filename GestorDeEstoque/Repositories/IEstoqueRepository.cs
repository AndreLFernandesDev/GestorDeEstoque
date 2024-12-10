using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        public Task<ProdutoEstoque> AtualizarQuantidadeProdutoAsync(int idEstoque, decimal quantidade, int idProduto);
    }
}