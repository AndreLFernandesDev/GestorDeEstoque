namespace GestorDeEstoque.Models
{
    public class LogEstoque
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public DateTime Data { get; set; }
        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; } = null!;

        public LogEstoque(int id, int quantidade, DateTime data)
        {
            Id = id;
            Quantidade = quantidade;
            Data = data;
        }
    }
}