using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GestorDeEstoque.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;

        public EstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EstoqueDTO>> ListarEstoquesAsync()
        {
            return await _context
                .Estoques.Select(e => new EstoqueDTO { Id = e.Id, Nome = e.Nome })
                .ToListAsync();
        }
    }
}
