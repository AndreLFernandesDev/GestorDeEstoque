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
        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }
        public async Task<bool> InserirProdutoAsync(Produto novoProduto)
        {
            try
            {
                await _context.AddAsync(novoProduto);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> AtualizarProdutoAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return false;
            }
            _context.Remove(produto);
            _context.SaveChanges();
            return true;
        }
    }
}