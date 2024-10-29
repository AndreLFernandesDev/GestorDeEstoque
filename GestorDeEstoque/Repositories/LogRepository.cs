using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
using Microsoft.EntityFrameworkCore;
namespace GestorDeEstoque.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;
        public LogRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<LogEstoque>> ObterLogsAsync()
        {
            return await _context.LogsEstoques.ToListAsync();
        }

        public async Task<LogEstoque> RegistrarLogEstoqueAsync(int produtoId, decimal quantidadeAtual)
        {
            var log = new LogEstoque(produtoId, quantidadeAtual);
            _context.LogsEstoques.Add(log);
            _context.SaveChanges();
            return log;
        }
    }
}