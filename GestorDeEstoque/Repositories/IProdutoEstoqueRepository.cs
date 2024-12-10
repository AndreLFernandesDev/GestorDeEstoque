using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IProdutoEstoqueRepository
    {
        public Task<ProdutoEstoque> BuscarQuantidadePorIdEstoqueEhIdProdutoAsync(int idProduto, int idEstoque);
        public Task AdicionarQuantidadeAsync(ProdutoEstoque produtoEstoque);
        public Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque);
    }
}