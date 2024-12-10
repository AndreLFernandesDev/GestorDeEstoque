using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GestorDeEstoque.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        public ProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProdutoDTO> BuscaPorIdProdutoEhIdEstoqueAsync(int idProduto, int idEstoque)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);
            var produtoQuantidade = await _context.ProdutosEstoques.FirstOrDefaultAsync(pq => pq.ProdutoId == idProduto && pq.EstoqueId == idEstoque);
            if (produto == null || produtoQuantidade == null)
            {
                throw new InvalidOperationException("Produto ou quantidade não encontrado");
            }
            var produtoDTO = new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Quantidade = produtoQuantidade.Quantidade,
                EstoqueId = idEstoque
            };
            return produtoDTO;
        }
        public async Task<bool> InserirProdutoAsync(Produto novoProduto)
        {
            try
            {
                await _context.AddAsync(novoProduto);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque)
        {
            var produtos = await _context.ProdutosEstoques.Include(pe => pe.Produto)
            .Where(pe => pe.EstoqueId == idEstoque)
            .Select
            (
                  pe => new ProdutoDTO
                  {
                      Nome = pe.Produto.Nome,
                      Descricao = pe.Produto.Descricao,
                      Preco = pe.Produto.Preco,
                      Quantidade = pe.Quantidade,
                      EstoqueId = idEstoque
                  }
            )
            .ToListAsync();
            return produtos;
        }

        public async Task AtualizarProdutoAsync(int idProduto, Produto produtoAtualizado, int estoqueId)
        {
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe => pe.ProdutoId == idProduto && pe.EstoqueId == estoqueId);
            if (produtoEstoque == null)
            {
                throw new InvalidOperationException("Produto não encontrado no estoque especificado");
            }
            var produto = await _context.Produtos.FirstOrDefaultAsync(P => P.Id == idProduto);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }
            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Preco = produtoAtualizado.Preco;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return false;
            }
            _context.Remove(produto);
            _context.SaveChanges();
            return true;
        }
    }
}