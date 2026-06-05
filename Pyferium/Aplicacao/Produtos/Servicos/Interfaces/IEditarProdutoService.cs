using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

public interface IEditarProdutoService
{
    Task<ProdutoEditadoResponse> AtualizarProdutoAsync(int codigoProduto, ProdutoRequest request);
}
