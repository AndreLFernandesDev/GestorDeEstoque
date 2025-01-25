using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
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

        public async Task<ProdutoEstoque> BuscarProdutoPorIdEstoqueEhIdProdutoAsync(
            int idProduto,
            int idEstoque
        )
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq =>
                pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque
            );
            if (produtoEstoque == null)
            {
                throw new Exception("Produto não existe");
            }

            return produtoEstoque;
        }

        public async Task CriarProdutoOuInserirQuantidadeAsync(ProdutoEstoque produtoEstoque)
        {
            var produtoEstoqueExistente = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe =>
                pe.ProdutoId == produtoEstoque.ProdutoId && pe.EstoqueId == produtoEstoque.EstoqueId
            );
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

        public async Task<ProdutoDTO> AtualizarQuantidadeProdutoAsync(
            int idEstoque,
            int idProduto,
            AtualizarQuantidadeProdutoDTO quantidadeProdutoDTO
        )
        {
            var produtoEstoque = await _context
                .ProdutosEstoques.Include(pe => pe.Produto)
                .FirstOrDefaultAsync(pe => pe.ProdutoId == idProduto && pe.EstoqueId == idEstoque);

            if (produtoEstoque == null)
            {
                throw new Exception("Produto ou estoque não existe");
            }
            produtoEstoque.Quantidade += quantidadeProdutoDTO.Quantidade;
            if (produtoEstoque.Quantidade < 0)
            {
                throw new Exception("Quantidade não pode ser negativa");
            }
            _context.ProdutosEstoques.Update(produtoEstoque);
            await _context.SaveChangesAsync();
            return new ProdutoDTO
            {
                Nome = produtoEstoque.Produto.Nome,
                Descricao = produtoEstoque.Produto.Descricao,
                Preco = produtoEstoque.Produto.Preco,
                Quantidade = produtoEstoque.Quantidade,
            };
        }

        public async Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque)
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(qP =>
                qP.ProdutoId == idProduto && qP.EstoqueId == idEstoque
            );
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
