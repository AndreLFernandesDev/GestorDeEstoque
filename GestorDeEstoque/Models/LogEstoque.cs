namespace GestorDeEstoque.Models
{
    public class LogEstoque(int produtoId, decimal quantidade, int estoqueId)
    {
        public int Id { get; set; }
        public decimal Quantidade { get; set; } = quantidade;
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public int ProdutoId { get; set; } = produtoId;
        public virtual Produto Produto { get; set; } = null!;
        public int EstoqueId { get; set; } = estoqueId;
        public virtual Estoque Estoque { get; set; } = null!;
    }
}