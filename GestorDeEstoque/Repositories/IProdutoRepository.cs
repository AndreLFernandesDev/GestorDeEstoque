using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface IProdutoRepository
    {
        Produto BuscarProdutoPorId(int id);
        bool InserirProduto(Produto novoProduto);
        Task<IEnumerable<Produto>> ListarProdutosAync();
        Task<Produto> AtualizarProdutoAsync(Produto produto);
    }
}