namespace Pyferium.Produtos.Aplicacao.Produtos.Responses;

public class ProdutoEditadoResponse
{
    public int CodigoProduto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public int CodigoCategoria { get; set; }
    public decimal ValorProduto { get; set; }
    public string IdtAtivo { get; set; } = string.Empty;
}