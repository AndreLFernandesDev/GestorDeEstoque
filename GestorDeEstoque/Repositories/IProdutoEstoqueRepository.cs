using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IProdutoEstoqueRepository
    {
        public Task<ProdutoEstoque> BuscarProdutoPorIdEstoqueEhIdProdutoAsync(
            int idProduto,
            int idEstoque
        );
        public Task CriarProdutoOuInserirQuantidadeAsync(ProdutoEstoque produtoEstoque);
        public Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque);
    }
}
