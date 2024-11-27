using Microsoft.AspNetCore.Mvc;
using GestorDeEstoque.Repositories;
using GestorDeEstoque.Models;
using GestorDeEstoque.DTOs;
using GestorDeEstoque.Data;
namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/estoque")]
    public class EstoqueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private EstoqueRepository _estoqueRepository;
        private LogRepository _logRepository;
        public EstoqueController(ApplicationDbContext context, EstoqueRepository estoqueRepository, LogRepository logRepository)
        {
            _context = context;
            _estoqueRepository = estoqueRepository;
            _logRepository = logRepository;
        }

        [HttpPut("{idProduto}/idEstoque/{idEstoque}")]
        public async Task<ActionResult<Produto>> AtualizarQuantidadeProdutoAsync(int idProduto, [FromBody] EstoqueDTO estoqueDTO, int idEstoque)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _estoqueRepository.AtualizarQuantidadeProdutoAsync(idProduto, estoqueDTO.Quantidade, idEstoque);
                await _logRepository.RegistrarLogEstoqueAsync(idProduto, estoqueDTO.Quantidade, idEstoque);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new { mensagem = "Estoque atualizado" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { mensagem = ex.Message });
            }
        }
        [HttpGet("{idEstoque}")]
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
    }
}
