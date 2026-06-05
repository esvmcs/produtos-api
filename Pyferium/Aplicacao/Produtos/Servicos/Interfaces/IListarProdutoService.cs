using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

public interface IListarProdutoService
{
    Task<IEnumerable<ProdutoListagemResponse>> ListarProdutosAsync();
    Task<IEnumerable<ProdutoListagemResponse>> ListarPorCodigoAsync(int codigoProduto);
}
