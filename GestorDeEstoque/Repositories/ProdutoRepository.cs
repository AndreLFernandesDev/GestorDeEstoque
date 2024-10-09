using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;
namespace GestorDeEstoque.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        public ProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Produto BuscarProdutoPorId(int id)
        {
            return _context.Produtos.Find(id);
        }
        public bool InserirProduto(Produto novoProduto)
        {
            try
            {
                _context.Add(novoProduto);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> AtualizarProdutoAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}