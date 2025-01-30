using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/estoques")]
    public class EstoqueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ProdutoRepository _produtoRepository;
        private EstoqueRepository _estoqueRepository;
        private LogRepository _logRepository;

        private ProdutoEstoqueRepository _produtoEstoqueRepository;

        public EstoqueController(
            ApplicationDbContext context,
            ProdutoRepository produtoRepository,
            EstoqueRepository estoqueRepository,
            LogRepository logRepository,
            ProdutoEstoqueRepository produtoEstoqueRepository
        )
        {
            _context = context;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _logRepository = logRepository;
            _produtoEstoqueRepository = produtoEstoqueRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstoqueDTO>>> ListarEstoquesAsync()
        {
            try
            {
                var estoques = await _estoqueRepository.ListarEstoquesAsync();
                if (estoques == null || !estoques.Any())
                {
                    return NotFound("Nenhum estoque encontrado");
                }
                return Ok(estoques);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{idEstoque}")]
        public async Task<ActionResult<Estoque>> BuscarEstoquePorIdAsync(int idEstoque)
        {
            try
            {
                var estoque = await _estoqueRepository.BuscarEstoquePorIdAsync(idEstoque);
                if (estoque == null)
                {
                    return NotFound("Estoque não encontrado");
                }
                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{idEstoque}/logs")]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterLogsAsync(int idEstoque)
        {
            try
            {
                var logs = await _logRepository.ObterLogsAsync(idEstoque);
                if (logs == null || !logs.Any())
                {
                    return NotFound("Nenhum log encontrado");
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AdicionarEstoqueAsync([FromBody] Estoque novoEstoque)
        {
            try
            {
                var resultado = await _estoqueRepository.AdicionarEstoqueAsync(novoEstoque);
                if (resultado == false)
                {
                    return BadRequest("Estoque não adicionado");
                }
                return Ok(novoEstoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{idEstoque}")]
        public async Task<ActionResult<Estoque>> AtualizarEstoqueAsync(
            int idEstoque,
            [FromBody] Estoque estoqueAtualizado
        )
        {
            try
            {
                var resultado = await _estoqueRepository.AtualizarEstoqueAsync(
                    idEstoque,
                    estoqueAtualizado
                );
                if (resultado == null)
                {
                    return NotFound("Estoque não encontrado");
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{idEstoque}")]
        public async Task<ActionResult<bool>> RemoverEstoqueAsync(int idEstoque)
        {
            try
            {
                var resultado = await _estoqueRepository.RemoverEstoqueAsync(idEstoque);
                if (resultado == false)
                {
                    return NotFound("Estoque não encontrado");
                }
                return Ok(new { mensagem = "Estoque deletado" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{idEstoque}/produtos")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ListarProdutos(int idEstoque)
        {
            try
            {
                var produtos = await _produtoRepository.ListarProdutosAsync(idEstoque);
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

        [HttpGet("{idEstoque}/produtos/{idProduto}")]
        public async Task<IActionResult> BuscarProdutoPorId(int idEstoque, int idProduto)
        {
            try
            {
                var produto = await _produtoRepository.BuscaPorIdProdutoEhIdEstoqueAsync(
                    idEstoque,
                    idProduto
                );
                if (produto == null)
                {
                    return NotFound(new { mensagem = "Estoque ou produto não encontrado" });
                }
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("{idEstoque}/produtos")]
        public async Task<ActionResult<ProdutoDTO>> InserirProdutoAsync(
            int idEstoque,
            [FromBody] ProdutoDTO novoProdutoDTO
        )
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var produto = new Produto
                {
                    Nome = novoProdutoDTO.Nome,
                    Descricao = novoProdutoDTO.Descricao,
                    Preco = novoProdutoDTO.Preco,
                };

                var estoqueExiste = await _produtoRepository.InserirProdutoAsync(
                    idEstoque,
                    produto
                );
                if (!estoqueExiste)
                {
                    return NotFound(new { mensagem = "Estoque não encontrado" });
                }
                await _context.SaveChangesAsync();

                var produtoEstoque = new ProdutoEstoque
                {
                    ProdutoId = produto.Id,
                    EstoqueId = idEstoque,
                    Quantidade = novoProdutoDTO.Quantidade,
                };

                await _produtoEstoqueRepository.CriarProdutoOuInserirQuantidadeAsync(
                    produtoEstoque
                );
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var produtoDTO = new ProdutoDTO
                {
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco,
                    Quantidade = produtoEstoque.Quantidade,
                };
                return Ok(produtoDTO);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{idEstoque}/produtos/{idProduto}")]
        public async Task<ActionResult<Produto>> AtualizarProdutoAsync(
            int idEstoque,
            [FromBody] Produto produtoAtualizado,
            int idProduto
        )
        {
            try
            {
                if (produtoAtualizado == null)
                {
                    return BadRequest("Dados inválidos do produto");
                }
                await _produtoRepository.AtualizarProdutoAsync(
                    idEstoque,
                    produtoAtualizado,
                    idProduto
                );
                return Ok(
                    new
                    {
                        produtoAtualizado.Nome,
                        produtoAtualizado.Descricao,
                        produtoAtualizado.Preco,
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPatch("{idEstoque}/produtos/{idProduto}/quantidade")]
        public async Task<ActionResult<ProdutoDTO>> AtualizarQuantidadeProdutoAsync(
            int idEstoque,
            int idProduto,
            [FromBody] AtualizarQuantidadeProdutoDTO produtoQuantidadeDTO
        )
        {
            try
            {
                var produtoDTO = await _produtoEstoqueRepository.AtualizarQuantidadeProdutoAsync(
                    idEstoque,
                    idProduto,
                    produtoQuantidadeDTO
                );
                return Ok(produtoDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{idEstoque}/produtos/{idProduto}")]
        public async Task<IActionResult> DeletarProduto(int idEstoque, int idProduto)
        {
            using var transaction = _context.Database.BeginTransaction();
            {
                try
                {
                    var produtoEstoque =
                        await _produtoEstoqueRepository.BuscarProdutoPorIdProdutoEhIdEstoqueAsync(
                            idProduto,
                            idEstoque
                        );
                    var produtoDeletado = await _produtoRepository.RemoverProdutoAsync(
                        produtoEstoque.ProdutoId
                    );
                    await _produtoEstoqueRepository.RemoverQuantidadeProdutoAsync(
                        produtoEstoque.ProdutoId,
                        produtoEstoque.EstoqueId
                    );
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Ok(new { mensagem = $"Produto deletado: {produtoDeletado.Nome}" });
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
