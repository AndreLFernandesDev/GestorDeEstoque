using GestorDeEstoque.Models;

namespace GestorDeEstoque.Models
{
    public class ProdutoEstoque
    {
        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; } = null!;

        public int EstoqueId { get; set; }
        public virtual Estoque Estoque { get; set; } = null!;

        public decimal Quantidade { get; set; }
    }
}
