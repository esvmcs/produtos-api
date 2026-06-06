using Pyferium.Produtos.Aplicacao.Produtos.Responses;

namespace Pyferium.Produtos.Aplicacao.Produtos.Repositorios;

public interface IProdutoConsultaRepositorio
{
    Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync();

    Task<ProdutoListagemResponse?> ListarPorCodigoAsync(int codigoProduto);

    Task<bool> ExisteProdutoAtivoComMesmoNomeAsync(
        string nomeProduto,
        int codigoCategoria,
        int? codigoProdutoIgnorar = null);
}