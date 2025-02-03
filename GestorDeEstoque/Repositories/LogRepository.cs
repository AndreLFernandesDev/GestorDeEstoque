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
            int estoqueId
        )
        {
            var log = new LogEstoque(produtoId, quantidadeAtual, estoqueId);
            _context.LogsEstoques.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }
    }
}
