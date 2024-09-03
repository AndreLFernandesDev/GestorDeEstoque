namespace GestorDeEstoque.Models
{
    public class Produto
    {
        public int Id { get; private set; }
        public required string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public Produto(int id, string nome, string descricao, decimal preco)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
        }

    }
}