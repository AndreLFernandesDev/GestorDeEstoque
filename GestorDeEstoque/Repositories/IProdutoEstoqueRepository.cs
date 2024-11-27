using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IProdutoEstoqueRepository
    {
        public Task<ProdutoEstoque> BuscarQuantidadePorIdAsync(int idProduto, int idEstoque);
        public Task<ProdutoEstoque> AdicionarQuantidadeAsync(ProdutoEstoque produtoEstoque);
        public Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque);
    }
}