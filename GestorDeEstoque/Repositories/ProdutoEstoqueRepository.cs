using GestorDeEstoque.Models;
using GestorDeEstoque.Data;
using Microsoft.EntityFrameworkCore;
namespace GestorDeEstoque.Repositories
{
    public class ProdutoEstoqueRepository : IProdutoEstoqueRepository
    {
        private readonly ApplicationDbContext _context;
        public ProdutoEstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProdutoEstoque> BuscarQuantidadePorIdEstoqueEhIdProdutoAsync(int idProduto, int idEstoque)
        {
            var produto = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq => pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque);
            if (produto == null)
            {
                throw new Exception("Produto não existe");
            }

            return produto;
        }

        public async Task AdicionarQuantidadeAsync(ProdutoEstoque produtoEstoque)
        {
            var produto = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe => pe.ProdutoId == produtoEstoque.ProdutoId && pe.EstoqueId == produtoEstoque.EstoqueId);
            if (produto == null)
            {
                await _context.ProdutosEstoques.AddAsync(produtoEstoque);
            }
            else
            {
                produto.Quantidade += produtoEstoque.Quantidade;
                _context.ProdutosEstoques.Update(produto);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque)
        {
            var produto = await _context.ProdutosEstoques.FirstOrDefaultAsync(qP => qP.ProdutoId == idProduto && qP.EstoqueId == idEstoque);
            if (produto == null)
            {
                return false;
            }
            _context.ProdutosEstoques.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

