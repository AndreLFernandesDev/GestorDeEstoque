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
                var produtos = await _produtoRepository.ListarProdutosAync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (produtoDTO == null || id <= 0)
            {
                return BadRequest("Dados inválidos do produto");
            }
            var produtoExistente = _produtoRepository.BuscarProdutoPorId(id);
            if (produtoExistente == null)
            {
                return BadRequest("Produto não existe");
            }
            produtoExistente.Nome = produtoDTO.Nome;
            produtoExistente.Descricao = produtoDTO.Descricao;
            produtoExistente.Preco = produtoDTO.Preco;
            produtoExistente.Quantidade = produtoDTO.Quantidade;
            produtoExistente.EstoqueId = produtoDTO.EstoqueId;

            await _produtoRepository.AtualizarProdutoAsync(produtoExistente);
            return Ok(produtoExistente);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id)
        {
            var produto = _produtoRepository.BuscarProdutoPorId(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado");
            }
            _produtoRepository.RemoverProduto(id);
            return Ok(new { mensagem = "Produto deletado" });
        }
    }
}