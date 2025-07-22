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

        public async Task<ProdutoEstoque> BuscarProdutoPorIdProdutoEhIdEstoqueAsync(
            int idProduto,
            int idEstoque
        )
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq =>
                pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque
            );
            if (produtoEstoque == null)
            {
                throw new Exception("Produto ou estoque não existe");
            }

            return produtoEstoque;
        }

        public async Task CriarProdutoAsync(ProdutoEstoque produtoEstoque)
        {
            produtoEstoque.Quantidade += produtoEstoque.Quantidade;
            await _context.ProdutosEstoques.AddAsync(produtoEstoque);
            await _context.SaveChangesAsync();
        }

        public async Task<ProdutoEstoque> AtualizarQuantidadeProdutoAsync(
            int idEstoque,
            int idProduto,
            AtualizarQuantidadeProdutoDTO quantidadeProdutoDTO
        )
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe =>
                pe.ProdutoId == idProduto && pe.EstoqueId == idEstoque
            );

            produtoEstoque.Quantidade += quantidadeProdutoDTO.Quantidade;
            if (produtoEstoque.Quantidade < 0)
            {
                throw new Exception("Quantidade não pode ser negativa");
            }
            _context.ProdutosEstoques.Update(produtoEstoque);
            await _context.SaveChangesAsync();
            return produtoEstoque;
        }

        public async Task<bool> RemoverQuantidadeProdutoAsync(int idProduto, int idEstoque)
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(qP =>
                qP.ProdutoId == idProduto && qP.EstoqueId == idEstoque
            );
            _context.ProdutosEstoques.Remove(produtoEstoque);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProdutoDTOQuantidadeMinima>> ProdutoBaixoEstoqueAsync(
            int idEstoque,
            decimal limite
        )
        {
            var produtos = await _context
                .ProdutosEstoques.Where(pe => pe.EstoqueId == idEstoque && pe.Quantidade < limite)
                .Select(pe => new ProdutoDTOQuantidadeMinima
                {
                    Produto = new ProdutoDTO
                    {
                        Nome = pe.Produto.Nome,
                        Descricao = pe.Produto.Descricao,
                        Preco = pe.Produto.Preco,
                        Quantidade = pe.Quantidade,
                    },
                    Limite = limite,
                })
                .ToListAsync();

            if (produtos == null)
            {
                throw new Exception("Nenhum produto abaixo do estoque mínimo encontrado");
            }

            return produtos;
        }
    }
}
