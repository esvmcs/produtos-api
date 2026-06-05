using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Infraestrutura.Repositorios.Interfaces;

public interface IProdutoRepositorio
{
    Task<ProdutoCriadoResponse> CriarProdutoAsync(string nomeProduto, int codigoCategoria, decimal valorProduto);
    Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(int codigoProduto, EditarProdutoRequest request);
    Task<IEnumerable<ProdutoListagemResponse>> ListarProdutosAsync();
    Task<IEnumerable<ProdutoListagemResponse>> ListarPorCodigoAsync(int codigoProduto);
    Task<bool> ExisteProdutoAtivoComMesmoNomeAsync(string nomeProduto, int codigoCategoria, int? codigoProdutoIgnorar = null);
}
