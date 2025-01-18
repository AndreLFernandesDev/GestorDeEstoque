using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<EstoqueDTO?> BuscarEstoquePorIdAsync(int id)
        {
            var estoque = await _context
                .Estoques.Select(e => new EstoqueDTO { Id = e.Id, Nome = e.Nome })
                .FirstOrDefaultAsync(e => e.Id == id);
            return estoque;
        }
    }
}
