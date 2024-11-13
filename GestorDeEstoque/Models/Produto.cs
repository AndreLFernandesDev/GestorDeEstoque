namespace GestorDeEstoque.Models
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Preco { get; set; }
        public decimal Quantidade { get; set; }
        public int EstoqueId { get; set; }
        public virtual ICollection<LogEstoque> LogsEstoque { get; set; } = [];
        public virtual ICollection<ProdutoEstoque> ProdutosEstoques { get; set; } = [];
    }
}