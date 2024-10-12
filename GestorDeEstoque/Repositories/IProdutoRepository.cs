using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IProdutoRepository
    {
        Task<Produto> BuscarProdutoPorIdAsync(int id);
        Task<bool> InserirProdutoAsync(Produto novoProduto);
        Task<IEnumerable<Produto>> ListarProdutosAsync();
        Task<Produto> AtualizarProdutoAsync(Produto produto);
        Task<bool> RemoverProdutoAsync(int id);
    }
}