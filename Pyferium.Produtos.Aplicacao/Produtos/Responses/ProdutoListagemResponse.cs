namespace Pyferium.Produtos.Aplicacao.Produtos.Responses
{
    public class ProdutoListagemResponse
    {
        public int CodigoProduto { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public decimal ValorProduto { get; set; }
        public int CodigoCategoria { get; set; }
        public string DescricaoCategoria { get; set; } = string.Empty;
        public string CodigoNivel { get; set; } = string.Empty;
        public string IdtAtivo { get; set; } = string.Empty;
    }
}
