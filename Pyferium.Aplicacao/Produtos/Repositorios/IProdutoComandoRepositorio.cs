using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;

namespace Pyferium.Aplicacao.Produtos.Repositorios;

public interface IProdutoComandoRepositorio
{
    Task<ProdutoCriadoResponse> CriarProdutoAsync(
        string nomeProduto,
        int codigoCategoria,
        decimal valorProduto);

    Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(
        int codigoProduto,
        ProdutoRequest request);

    Task<bool> DeletarProdutoAsync(int codigoProduto);
}