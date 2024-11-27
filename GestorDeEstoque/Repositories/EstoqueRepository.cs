using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
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
        public async Task<ProdutoEstoque> AtualizarQuantidadeProdutoAsync(int idProduto, decimal quantidade, int idEstoque)
        {
            var quantidadeProduto = await _context.ProdutosEstoques.FirstOrDefaultAsync(qp => qp.ProdutoId == idProduto && qp.EstoqueId == idEstoque);
            if (quantidadeProduto == null)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }
            else
            {
                quantidadeProduto.Quantidade += quantidade;
                _context.SaveChanges();
                return quantidadeProduto;
            }
        }
    }
}