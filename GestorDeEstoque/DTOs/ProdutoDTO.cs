namespace GestorDeEstoque.DTOs
{
    public class ProdutoDTO
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required decimal Preco { get; set; }
        public required decimal Quantidade { get; set; }
    }
}
