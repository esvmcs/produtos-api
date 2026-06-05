using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Aplicacao.Produtos.Repositorios;

public interface IProdutoRepositorio
{
    Task<ProdutoCriadoResponse> CriarProdutoAsync(
        string nomeProduto,
        int codigoCategoria,
        decimal valorProduto);

    Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(
        int codigoProduto,
        ProdutoRequest request);

    Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync();

    Task<ProdutoListagemResponse?> ListarPorCodigoAsync(int codigoProduto);

    Task<bool> ExisteProdutoAtivoComMesmoNomeAsync(
        string nomeProduto,
        int codigoCategoria,
        int? codigoProdutoIgnorar = null);

    Task<bool> DeletarProdutoAsync(int codigoProduto);
}