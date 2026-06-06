using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

public interface IEditarProdutoService
{
    Task<ProdutoEditadoResponse> AtualizarProdutoAsync(int codigoProduto, EditarProdutoRequest request);
}
