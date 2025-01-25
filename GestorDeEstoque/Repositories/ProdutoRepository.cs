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

        public async Task<ProdutoDTO> BuscaPorIdProdutoEhIdEstoqueAsync(
            int idEstoque,
            int idProduto
        )
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == idProduto);
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe =>
                pe.ProdutoId == idEstoque && pe.EstoqueId == idEstoque
            );
            if (produto == null || produtoEstoque == null)
            {
                throw new InvalidOperationException("Estoque ou produto não encontrado");
            }
            var produtoDTO = new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Quantidade = produtoEstoque.Quantidade,
            };
            return produtoDTO;
        }

        public async Task<bool> InserirProdutoAsync(int idEstoque, Produto novoProduto)
        {
            try
            {
                var estoque = await _context.Estoques.FindAsync(idEstoque);
                if (estoque == null)
                {
                    return false;
                }
                await _context.AddAsync(novoProduto);
                return _context.SaveChanges() > 0;
            }
            catch
            {
                throw;
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
            var produtoEstoque = await _context.ProdutosEstoques.FirstOrDefaultAsync(pe =>
                pe.EstoqueId == idEstoque && pe.ProdutoId == idProduto
            );
            if (produtoEstoque == null)
            {
                throw new InvalidOperationException(
                    "Produto não encontrado no estoque especificado"
                );
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
            return produto;
        }

        public async Task<ProdutoDTO> RemoverProdutoAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto não encontrado");
            }
            _context.Remove(produto);
            _context.SaveChanges();
            return new ProdutoDTO
            {
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Quantidade = 0,
            };
        }
    }
}
