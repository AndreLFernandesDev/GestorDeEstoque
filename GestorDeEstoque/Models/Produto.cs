namespace GestorDeEstoque.Models
{
    public class Produto
    {
        public int Id { get; private set; }
        public required string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public required decimal Quantidade { get; set; }
        public required int EstoqueId { get; set; }
        public virtual Estoque Estoque { get; set; } = null!;
        public virtual ICollection<LogEstoque> LogsEstoque { get; set; } = [];

        public Produto(int id, string nome, string descricao, decimal preco, decimal quantidade, int estoqueId)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Quantidade = quantidade;
            EstoqueId = estoqueId;
        }

    }
}