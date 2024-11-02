namespace GestorDeEstoque.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; } = [];

        public Estoque(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}