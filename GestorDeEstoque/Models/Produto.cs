namespace GestorDeEstoque.Models
{
    public class Produto
    {
        public int Id { get; private set; }
        public required string Nome { get; set; } = null!;
        public required string Descricao { get; set; } = null!;
        public required decimal Preco { get; set; }
        public virtual ICollection<LogEstoque> LogsEstoque { get; set; } = [];
        public virtual ICollection<ProdutoEstoque> ProdutosEstoques { get; set; } = [];
    }
}