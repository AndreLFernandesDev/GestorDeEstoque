using GestorDeEstoque.Models;
using GestorDeEstoque.Repositories;
using Microsoft.AspNetCore.Mvc;
using GestorDeEstoque.DTOs;
namespace GestorDeEstoque.Controllers
{
    [ApiController]
    [Route("/produtos")]
    public class ProdutoController : ControllerBase
    {
        private ProdutoRepository _produtoRepository;
        public ProdutoController(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));

        }

        [HttpGet("{id}")]
        public IActionResult BuscarProdutoPorId(int id)
        {
            var produto = _produtoRepository.BuscarProdutoPorId(id);
            if (produto == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(produto);
            }
        }


        [HttpPost]
        public IActionResult InserirProduto([FromBody] ProdutoDTO novoProdutoDTO)
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
                Quantidade = novoProdutoDTO.Quantidade,
                EstoqueId = novoProdutoDTO.EstoqueId
            };
            _produtoRepository.InserirProduto(produto);

            return CreatedAtAction(nameof(BuscarProdutoPorId), new { id = produto.Id }, produto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ListarProdutos()
        {
            try
            {
                var produtos = await _produtoRepository.ListarProdutos();
                if (produtos == null)
                {
                    return NotFound("Nenhum produto encontradi");
                }
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu erro interno ao buscar produtos");
            }

        }

    }
}