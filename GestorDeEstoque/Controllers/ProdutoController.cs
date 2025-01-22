using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ProdutoRepository _produtoRepository;
        private ProdutoEstoqueRepository _produtoEstoqueRepository;

        public ProdutoController(
            ApplicationDbContext context,
            ProdutoRepository produtoRepository,
            ProdutoEstoqueRepository produtoEstoqueRepository
        )
        {
            _context = context;
            _produtoRepository =
                produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
            _produtoEstoqueRepository =
                produtoEstoqueRepository
                ?? throw new ArgumentNullException(nameof(produtoEstoqueRepository));
        }

        [HttpGet("{produtoId}/estoqueId/{estoqueId}")]
        public async Task<IActionResult> BuscarProdutoPorId(int produtoId, int estoqueId)
        {
            try
            {
                var produto = await _produtoRepository.BuscaPorIdProdutoEhIdEstoqueAsync(
                    produtoId,
                    estoqueId
                );
                if (produto == null)
                {
                    return NotFound(new { mensagem = "Produto ou estoque não encontrado" });
                }
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ListarProdutos(
            [FromQuery] int estoqueId
        )
        {
            try
            {
                var produtos = await _produtoRepository.ListarProdutosAsync(estoqueId);
                if (produtos == null)
                {
                    return NotFound("Nenhum produto encontrado");
                }
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu erro interno ao buscar produtos");
            }
        }

        [HttpPut("{idProduto}/estoqueId/{idEstoque}")]
        public async Task<IActionResult> AtualizarProduto(
            int idProduto,
            [FromBody] Produto produtoAtualizado,
            int idEstoque
        )
        {
            try
            {
                if (produtoAtualizado == null)
                {
                    return BadRequest("Dados inválidos do produto");
                }
                await _produtoRepository.AtualizarProdutoAsync(
                    idProduto,
                    produtoAtualizado,
                    idEstoque
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}/estoqueId/{idEstoque}")]
        public async Task<IActionResult> DeletarProduto(int id, int idEstoque)
        {
            using var transaction = _context.Database.BeginTransaction();
            {
                try
                {
                    var produtoEstoque =
                        await _produtoEstoqueRepository.BuscarProdutoPorIdEstoqueEhIdProdutoAsync(
                            id,
                            idEstoque
                        );
                    if (produtoEstoque == null)
                    {
                        return NotFound("Produto não encontrado");
                    }
                    await _produtoRepository.RemoverProdutoAsync(produtoEstoque.ProdutoId);
                    await _produtoEstoqueRepository.RemoverQuantidadeProdutoAsync(
                        produtoEstoque.ProdutoId,
                        produtoEstoque.EstoqueId
                    );
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok(new { mensagem = "Produto deletado" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { mensagem = ex.Message });
                }
            }
        }
    }
}
