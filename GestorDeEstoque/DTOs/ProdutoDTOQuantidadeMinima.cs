namespace GestorDeEstoque.DTOs
{
    public class ProdutoDTOQuantidadeMinima
    {
        public required ProdutoDTO Produto { get; set; }
        public decimal Limite { get; set; }
    }
}
