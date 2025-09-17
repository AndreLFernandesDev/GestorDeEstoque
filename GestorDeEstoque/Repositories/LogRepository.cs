using GestorDeEstoque.Controllers;
using GestorDeEstoque.Data;
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
            decimal quantidadeOperacao,
            int estoqueId
        )
        {
            var tipoDeMovimentacao =
                quantidadeOperacao > 0
                    ? LogEstoque.LogEstoqueTipoDeMovimentacao.Entrada
                    : LogEstoque.LogEstoqueTipoDeMovimentacao.Saida;

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
