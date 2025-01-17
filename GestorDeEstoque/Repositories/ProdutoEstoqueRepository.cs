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

        public async Task<ProdutoEstoque> BuscarProdutoPorIdEstoqueEhIdProdutoAsync(int idProduto, int idEstoque)
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq => pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque);
            if (produtoEstoque == null)
            {
                throw new Exception("Produto não existe");
            }

            return produtoEstoque;
        }

        public async Task CriarProdutoOuInserirQuantidadeAsync(ProdutoEstoque produtoEstoque)
        {
            var produtoEstoqueExistente = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe => pe.ProdutoId == produtoEstoque.ProdutoId && pe.EstoqueId == produtoEstoque.EstoqueId);
            if (produtoEstoqueExistente == null)
            {
                await _context.ProdutosEstoques.AddAsync(produtoEstoque);
            }
            else
            {
                produtoEstoqueExistente.Quantidade += produtoEstoque.Quantidade;
                _context.ProdutosEstoques.Update(produtoEstoqueExistente);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque)
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(qP => qP.ProdutoId == idProduto && qP.EstoqueId == idEstoque);
            if (produtoEstoque == null)
            {
                return false;
            }
            _context.ProdutosEstoques.Remove(produtoEstoque);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

