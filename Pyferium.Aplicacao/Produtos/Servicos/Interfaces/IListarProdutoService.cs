using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

public interface IListarProdutoService
{
    Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync();
    Task<ProdutoListagemResponse> ListarPorCodigoAsync(int codigoProduto);
}