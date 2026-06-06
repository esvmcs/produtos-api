using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

public interface ICriarProdutoService
{
    Task<ProdutoCriadoResponse> CriarProdutoAsync(CriarProdutoRequest request);
}
