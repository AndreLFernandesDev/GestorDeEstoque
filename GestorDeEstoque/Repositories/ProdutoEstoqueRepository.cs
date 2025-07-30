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

        public async Task<List<ProdutosObsoletosDTO>> ProdutosObsoletosAsync(int idEstoque)
        {
            var dataLimite = DateTime.UtcNow.AddMonths(-6);

            var ultimasSaidas = await _context
                .LogsEstoques.Where(log =>
                    log.EstoqueId == idEstoque
                    && log.TipoDeMovimentacao == LogEstoqueTipoDeMovimentacao.Saida
                )
                .GroupBy(log => log.ProdutoId)
                .Select(group => new
                {
                    ProdutoId = group.Key,
                    UltimaSaida = group.Max(log => log.Data),
                })
                .ToListAsync();

            var ultimasSaidasDict = ultimasSaidas.ToDictionary(
                u => u.ProdutoId,
                u => u.UltimaSaida
            );

            var produtosEstoque = await _context
                .ProdutosEstoques.Where(pe => pe.EstoqueId == idEstoque)
                .Select(pe => new
                {
                    ProdutoId = pe.ProdutoId,
                    Nome = pe.Produto.Nome,
                    Quantidade = pe.Quantidade,
                })
                .ToListAsync();

            var produtosObsoletos = produtosEstoque
                .Where(pe =>
                    !ultimasSaidasDict.ContainsKey(pe.ProdutoId)
                    || ultimasSaidasDict[pe.ProdutoId] < dataLimite
                )
                .Select(pe => new ProdutosObsoletosDTO
                {
                    ProdutoId = pe.ProdutoId,
                    Nome = pe.Nome,
                    Quantidade = pe.Quantidade,
                    UltimaSaida = ultimasSaidasDict.ContainsKey(pe.ProdutoId)
                        ? ultimasSaidasDict[pe.ProdutoId]
                        : null,
                })
                .ToList();

            return produtosObsoletos;
        }
    }
}
