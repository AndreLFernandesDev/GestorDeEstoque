using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IProdutoEstoqueRepository
    {
        public Task<ProdutoEstoque> BuscarProdutoPorIdProdutoEhIdEstoqueAsync(
            int idProduto,
            int idEstoque
        );
        public Task CriarProdutoAsync(ProdutoEstoque produtoEstoque);
        public Task<ProdutoEstoque> AtualizarQuantidadeProdutoAsync(
            int idEstoque,
            int idProduto,
            AtualizarQuantidadeProdutoDTO quantidadeProdutoDTO
        );
        public Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque);

        public Task<List<ProdutoDTOQuantidadeMinima>> ProdutoBaixoEstoqueAsync(
            int idEstoque,
            decimal limite
        );
    }
}
