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

        public async Task<ProdutoEstoque> BuscarQuantidadePorIdAsync(int idProduto, int idEstoque)
        {
            var produtoQuantidade = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq => pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque);
            if (produtoQuantidade == null)
            {
                throw new Exception("Produto não existe");
            }

            return produtoQuantidade;
        }

        public async Task<ProdutoEstoque> AdicionarQuantidadeAsync(ProdutoEstoque produtoEstoque)
        {
            var produtoExistente = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe => pe.ProdutoId == produtoEstoque.ProdutoId && pe.EstoqueId == produtoEstoque.EstoqueId);
            if (produtoExistente == null)
            {
                await _context.ProdutosEstoques.AddAsync(produtoEstoque);
            }
            else
            {
                produtoExistente.Quantidade += produtoEstoque.Quantidade;
                _context.ProdutosEstoques.Update(produtoExistente);
            }
            await _context.SaveChangesAsync();
            return produtoEstoque;
        }

        public async Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque)
        {
            var quantidadeProduto = await _context.ProdutosEstoques.FirstOrDefaultAsync(qP => qP.ProdutoId == idProduto && qP.EstoqueId == idEstoque);
            if (quantidadeProduto == null)
            {
                return false;
            }
            _context.ProdutosEstoques.Remove(quantidadeProduto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

