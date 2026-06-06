using Pyferium.Produtos.Aplicacao.Produtos.Comandos;
using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;

namespace Pyferium.Produtos.Aplicacao.Produtos.Repositorios;

public interface IProdutoComandoRepositorio
{
    Task<ProdutoCriadoResponse> CriarProdutoAsync(
        string nomeProduto,
        int codigoCategoria,
        decimal valorProduto);

    Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(
        int codigoProduto,
        EditarProdutoComando request);

    Task<bool> DeletarProdutoAsync(int codigoProduto);
}