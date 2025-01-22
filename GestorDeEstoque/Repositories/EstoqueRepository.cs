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

        public async Task<bool> AdicionarEstoqueAsync(Estoque novoEstoque)
        {
            try
            {
                _context.Estoques.Add(novoEstoque);
                var resultado=await _context.SaveChangesAsync();
                if(resultado==0)
                {
<<<<<<< HEAD
                    throw new Exception ("Estoque não adicionado, nenhuma alteração feita no banco de dados");
=======
                    throw new Exception ("Produto não adicionado, nenhuma alteração feita no banco de dados");
>>>>>>> c422c2fa0d8e2c3167cdcfe8cb33eb983101260a
                }
                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
