namespace Pyferium.Produtos.Aplicacao.Produtos.Excecoes;

public class ProdutoNaoEncontradoException : Exception
{
    public ProdutoNaoEncontradoException(int codigoProduto)
        : base($"Produto com código {codigoProduto} não encontrado.")
    {
    }
}