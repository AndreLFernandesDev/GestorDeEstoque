using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorDeEstoque.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<LogEstoque>> ObterLogsAsync(int idEstoque)
        {
            return await _context.LogsEstoques.Where(le => le.EstoqueId == idEstoque).ToListAsync();
        }

        public async Task<LogEstoque> RegistrarLogEstoqueAsync(
            int produtoId,
            decimal quantidadeAtual,
            int estoqueId
        )
        {
            var ultimoLog = await _context
                .LogsEstoques.Where(le => le.ProdutoId == produtoId && le.EstoqueId == estoqueId)
                .OrderByDescending(le => le.Data)
                .FirstOrDefaultAsync();

            var tipoDeMovimentacao =
                ultimoLog == null || ultimoLog.Quantidade < quantidadeAtual
                    ? LogEstoqueTipoDeMovimentacao.Entrada
                    : LogEstoqueTipoDeMovimentacao.Saida;

            var novoLog = new LogEstoque(produtoId, quantidadeAtual, estoqueId)
            {
                TipoDeMovimentacao = tipoDeMovimentacao,
            };

            _context.LogsEstoques.Add(novoLog);
            await _context.SaveChangesAsync();

            return novoLog;
        }
    }
}
