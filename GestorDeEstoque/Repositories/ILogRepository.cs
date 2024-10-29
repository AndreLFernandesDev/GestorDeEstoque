using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public interface ILogRepository
    {
        public Task<LogEstoque> RegistrarLogEstoqueAsync(int produtoId, decimal quantidade);
        public Task<IEnumerable<LogEstoque>> ObterLogsAsync();
    }
}