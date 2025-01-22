using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IProdutoRepository
    {
        Task<ProdutoDTO> BuscaPorIdProdutoEhIdEstoqueAsync(int idEstoque, int idProduto);
        Task<bool> InserirProdutoAsync(Produto novoProduto);
        Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque);
        Task AtualizarProdutoAsync(int idProduto, Produto produtoAtualizado, int idEstoque);
        Task<bool> RemoverProdutoAsync(int id);
    }
}
