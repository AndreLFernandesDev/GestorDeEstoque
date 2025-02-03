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

        public async Task<Estoque> BuscarEstoquePorIdAsync(int id)
        {
            var estoque = await _context.Estoques.FirstOrDefaultAsync(e => e.Id == id);
            return estoque;
        }

        public async Task<Estoque> AdicionarEstoqueAsync(Estoque novoEstoque)
        {
            _context.Estoques.Add(novoEstoque);
            var resultado = await _context.SaveChangesAsync();
            if (resultado == 0)
            {
                throw new Exception(
                    "Estoque não adicionado, nenhuma alteração feita no banco de dados"
                );
            }
            return novoEstoque;
        }

        public async Task<Estoque> AtualizarEstoqueAsync(int idEstoque, Estoque estoqueAtualizado)
        {
            var estoque = await _context.Estoques.FindAsync(idEstoque);
            estoque.Nome = estoqueAtualizado.Nome;
            _context.Estoques.Update(estoque);
            var resultado = await _context.SaveChangesAsync();
            if (resultado == 0)
            {
                throw new Exception(
                    "Estoque não atualizado, nenhuma alteração feita no banco de dados"
                );
            }
            return estoque;
        }

        public async Task<bool> RemoverEstoqueAsync(int idEstoque)
        {
            var estoque = await _context.Estoques.FindAsync(idEstoque);
            _context.Estoques.Remove(estoque);
            var resultado = await _context.SaveChangesAsync();
            if (resultado == 0)
            {
                throw new Exception(
                    "Estoque não deletado, nenhuma alteração feita no banco de dados"
                );
            }
            return true;
        }
    }
}
