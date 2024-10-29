using GestorDeEstoque.Data;
using GestorDeEstoque.Models;
namespace GestorDeEstoque.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly ApplicationDbContext _context;
        public EstoqueRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Produto> AtualizarQuantidadeProdutoAsync(int idProduto, decimal quantidade)
        {
            var produto = await _context.Produtos.FindAsync(idProduto);
            if (produto == null)
            {
                return null;
            }
            else
            {
                produto.Quantidade += quantidade;
                _context.SaveChanges();
                return produto;
            }
        }
    }
}