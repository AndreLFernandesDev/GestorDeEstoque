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

        public async Task<Produto> BuscaPorIdEstoqueEhIdProdutoAsync(int idEstoque, int idProduto)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);
            if (produto == null)
            {
                throw new Exception("Produto não encontrado");
            }
            return produto;
        }

        public async Task<bool> InserirProdutoAsync(int idEstoque, Produto novoProduto)
        {
            try
            {
                await _context.Produtos.AddAsync(novoProduto);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProdutoDTO>> ListarProdutosAsync(int idEstoque)
        {
            var produtos = await _context
                .ProdutosEstoques.Include(pe => pe.Produto)
                .Where(pe => pe.EstoqueId == idEstoque)
                .Select(pe => new ProdutoDTO
                {
                    Nome = pe.Produto.Nome,
                    Descricao = pe.Produto.Descricao,
                    Preco = pe.Produto.Preco,
                    Quantidade = pe.Quantidade,
                })
                .ToListAsync();
            return produtos;
        }

        public async Task<Produto> AtualizarProdutoAsync(
            int idEstoque,
            Produto produtoAtualizado,
            int idProduto
        )
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(P => P.Id == idProduto);
            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Preco = produtoAtualizado.Preco;

            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> RemoverProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }
    }
}
