using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;

namespace GestorDeEstoque.Repositories
{
    public interface IProdutoRepository
    {
        Task<ProdutoDTO> BuscaPorIdProdutoEhIdEstoqueAsync(int idEstoque, int idProduto);
        Task<bool> InserirProdutoAsync(int idEstoque,Produto novoProduto);
        Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque);
        Task<Produto> AtualizarProdutoAsync(int idEstoque, Produto produtoAtualizado, int idProduto);
        Task<bool> RemoverProdutoAsync(int id);
    }
}
