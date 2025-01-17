using GestorDeEstoque.Models;
using GestorDeEstoque.DTOs;
namespace GestorDeEstoque.Repositories
{
    public interface IProdutoRepository
    {
        Task<ProdutoDTO> BuscaPorIdProdutoEhIdEstoqueAsync(int idProduto, int idEstoque);
        Task<bool> InserirProdutoAsync(Produto novoProduto);
        Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque);
        Task AtualizarProdutoAsync(int idProduto, Produto produtoAtualizado, int idEstoque);
        Task<bool> RemoverProdutoAsync(int id);
    }
}