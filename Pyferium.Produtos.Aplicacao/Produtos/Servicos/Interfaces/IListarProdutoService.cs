using Pyferium.Produtos.Aplicacao.Produtos.Responses;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

public interface IListarProdutoService
{
    Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync();
    Task<ProdutoListagemResponse> ListarPorCodigoAsync(int codigoProduto);
}