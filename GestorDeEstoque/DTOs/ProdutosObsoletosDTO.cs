namespace GestorDeEstoque.DTOs;

public class ProdutosObsoletosDTO
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public DateTime? UltimaSaida { get; set; }
}
