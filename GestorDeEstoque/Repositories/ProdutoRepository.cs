using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
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
        public async Task<ProdutoDTO> BuscarProdutoPorIdAsync(int idProduto, int idEstoque)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);
            var produtoQuantidade = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq => pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque);
            if (produto == null || produtoQuantidade == null)
            {
                throw new InvalidOperationException("Produto ou quantidade não encontrado");
            }
            var produtoDTO = new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Quantidade = produtoQuantidade.Quantidade,
                EstoqueId = idEstoque
            };
            return produtoDTO;
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

        public async Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque)
        {
            var produtos = await _context.ProdutosEstoques.Include(pe => pe.Produto)
            .Where(pe => pe.EstoqueId == idEstoque)
            .Select
            (
                  pe => new ProdutoDTO
                  {
                      Nome = pe.Produto.Nome,
                      Descricao = pe.Produto.Descricao,
                      Preco = pe.Produto.Preco,
                      Quantidade = pe.Quantidade,
                      EstoqueId = idEstoque
                  }
            )
            .ToListAsync();
            return produtos;
        }

        public async Task<Produto> AtualizarProdutoAsync(int idProduto, Produto produtoAtualizado, int estoqueId)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);
            var estoque = await _context.Estoques.FirstOrDefaultAsync(e => e.Id == estoqueId);

            if (produto == null || estoque == null)
            {
                throw new InvalidOperationException("Produto ou estoque não encontrados.");
            }
            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Preco = produtoAtualizado.Preco;

            await _context.SaveChangesAsync();
            return produtoAtualizado;
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