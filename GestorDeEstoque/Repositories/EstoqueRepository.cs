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
                    throw new Exception ("Estoque não adicionado, nenhuma alteração feita no banco de dados");
                }
                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> AtualizarEstoqueAsync(int idEstoque,Estoque estoqueAtualizado)
        {
         try{
            var estoque =await _context.Estoques.FindAsync(idEstoque);
            if(estoque==null)
            {
                return false;
            }
            estoque.Nome=estoqueAtualizado.Nome;
            _context.Estoques.Update(estoque);
            var resultado=await _context.SaveChangesAsync();
            if(resultado==0)
            {
                throw new Exception("Estoque não atualizado, nenhuma alteração feita no banco de dados");
            }
            return true;
         }
         catch
            {
                throw;
            }
        }

        public async Task<bool> RemoverEstoqueAsync(int idEstoque)
        {
            try{
                var estoque = await _context.Estoques.FindAsync(idEstoque);
                if(estoque==null)
                {
                    return false;
                }
                _context.Estoques.Remove(estoque);
                var resultado=await _context.SaveChangesAsync();
                if(resultado==0)
                {
                    throw new Exception("Estoque não deletado, nenhuma alteração feita no banco de dados");
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
