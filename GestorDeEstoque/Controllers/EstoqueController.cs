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

        public EstoqueController(
            ApplicationDbContext context,
            ProdutoRepository produtoRepository,
            EstoqueRepository estoqueRepository,
            LogRepository logRepository
        )
        {
            _context = context;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _logRepository = logRepository;
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
