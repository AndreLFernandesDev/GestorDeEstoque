using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IEstoqueRepository
    {
        public Task<ProdutoEstoque> AtualizarQuantidadeProdutoAsync(int idProduto, decimal quantidade, int idEstoque);
    }
}