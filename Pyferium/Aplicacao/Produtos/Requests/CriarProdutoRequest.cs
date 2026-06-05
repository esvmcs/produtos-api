namespace Pyferium.Aplicacao.Produtos.Requests;

public class CriarProdutoRequest
{
    public string NomeProduto { get; set; } = string.Empty;
    public int CodigoCategoria { get; set; }
    public decimal ValorProduto { get; set; }
}