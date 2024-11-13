using GestorDeEstoque.Models;

namespace GestorDeEstoque.Models
{
    public class ProdutoEstoque
    {
        public int ProdutoId { get; set; }
        public Produto? Produto { get; set; }

        public int EstoqueId { get; set; }
        public Estoque? Estoque { get; set; }

        public int Quantidade { get; set; }
    }
}
