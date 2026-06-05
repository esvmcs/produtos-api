namespace Pyferium.Aplicacao.Produtos.Requests;

public class EditarProdutoRequest
{
    public string? NomeProduto { get; set; }
    public int? CodigoCategoria { get; set; }
    public decimal? ValorProduto { get; set; }
    public string? IdtAtivo { get; set; }
}