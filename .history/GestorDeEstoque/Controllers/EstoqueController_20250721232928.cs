using GestorDeEstoque.Data;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Models;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/estoques")]
    [Authorize]
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
                if (resultado == null)
                {
                    return BadRequest("Estoque não adicionado");
                }
                return Ok(resultado);
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
                var estoqueExistente = await _estoqueRepository.BuscarEstoquePorIdAsync(idEstoque);
                if (estoqueExistente == null)
                {
                    return NotFound("Estoque não encontrado");
                }
                var resultado = await _estoqueRepository.AtualizarEstoqueAsync(
                    idEstoque,
                    estoqueAtualizado
                );
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
                var estoqueExistente = await _estoqueRepository.BuscarEstoquePorIdAsync(idEstoque);
                if (estoqueExistente == null)
                {
                    return NotFound("Estoque não encontrado");
                }
                var resultado = await _estoqueRepository.RemoverEstoqueAsync(idEstoque);
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
                var estoqueExistente = await _context.Estoques.FindAsync(idEstoque);
                if (estoqueExistente == null)
                {
                    return NotFound("Estoque não encontrado");
                }
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
        public async Task<ActionResult<Produto>> BuscarProdutoPorId(int idEstoque, int idProduto)
        {
            try
            {
                var produto = await _produtoRepository.BuscaPorIdEstoqueEhIdProdutoAsync(
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
                var estoqueExistente = await _context.Estoques.FindAsync(idEstoque);
                if (estoqueExistente == null)
                {
                    return NotFound(new { mensagem = "Estoque não encontrado" });
                }
                var produto = new Produto
                {
                    Nome = novoProdutoDTO.Nome,
                    Descricao = novoProdutoDTO.Descricao,
                    Preco = novoProdutoDTO.Preco,
                };
                await _produtoRepository.InserirProdutoAsync(idEstoque, produto);
                await _context.SaveChangesAsync();

                var produtoEstoque = new ProdutoEstoque
                {
                    ProdutoId = produto.Id,
                    EstoqueId = idEstoque,
                    Quantidade = novoProdutoDTO.Quantidade,
                };

                await _produtoEstoqueRepository.CriarProdutoAsync(produtoEstoque);
                await _context.SaveChangesAsync();

                await _logRepository.RegistrarLogEstoqueAsync(
                    produto.Id,
                    produtoEstoque.Quantidade,
                    idEstoque
                );

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
                var produtoEstoqueExistente =
                    await _produtoRepository.BuscaPorIdEstoqueEhIdProdutoAsync(
                        idEstoque,
                        idProduto
                    );
                if (produtoEstoqueExistente == null)
                {
                    return NotFound(new { mensagem = "Produto ou estoque não encontrado" });
                }
                await _produtoRepository.AtualizarProdutoAsync(
                    idEstoque,
                    produtoAtualizado,
                    idProduto
                );
                return Ok(
                    new Produto
                    {
                        Nome = produtoAtualizado.Nome,
                        Descricao = produtoAtualizado.Descricao,
                        Preco = produtoAtualizado.Preco,
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPatch("{idEstoque}/produtos/{idProduto}/quantidade")]
        public async Task<ActionResult<ProdutoEstoque>> AtualizarQuantidadeProdutoAsync(
            int idEstoque,
            int idProduto,
            [FromBody] AtualizarQuantidadeProdutoDTO produtoQuantidadeDTO
        )
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var produtoEstoqueExistente =
                    await _produtoEstoqueRepository.BuscarProdutoPorIdProdutoEhIdEstoqueAsync(
                        idProduto,
                        idEstoque
                    );
                if (produtoEstoqueExistente == null)
                {
                    return NotFound(new { mensagem = "Produto ou estoque não encontrado" });
                }
                var produtoEstoque =
                    await _produtoEstoqueRepository.AtualizarQuantidadeProdutoAsync(
                        idEstoque,
                        idProduto,
                        produtoQuantidadeDTO
                    );
                await _context.SaveChangesAsync();

                await _logRepository.RegistrarLogEstoqueAsync(
                    idProduto,
                    produtoEstoque.Quantidade,
                    idEstoque
                );
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(produtoEstoque);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
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
                    var produtoEstoqueExistente =
                        await _produtoRepository.BuscaPorIdEstoqueEhIdProdutoAsync(
                            idEstoque,
                            idProduto
                        );
                    if (produtoEstoqueExistente == null)
                    {
                        return NotFound(new { mensagem = "Produto ou estoque não encontrado" });
                    }
                    var produtoDeletado = await _produtoRepository.RemoverProdutoAsync(idProduto);
                    await _produtoEstoqueRepository.RemoverQuantidadeProdutoAsync(
                        idProduto,
                        idEstoque
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

        [HttpGet("{idEstoque}/relatorios/baixo-estoque")]
        public async Task<ActionResult<ProdutoDTOQuantidadeMinima>> ProdutoBaixoEstoqueAsync(
            int idEstoque,
            [FromQuery] int limite
        )
        {
            try
            {
                var produtosBaixoEstoque = await _produtoEstoqueRepository.ProdutoBaixoEstoqueAsync(
                    idEstoque,
                    limite
                );
                if (produtosBaixoEstoque == null || produtosBaixoEstoque.Count == 0)
                {
                    return NotFound("Nenhum produto abaixo da quantidade mínima encontrada");
                }
                return Ok(produtosBaixoEstoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
