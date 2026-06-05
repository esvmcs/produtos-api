using Pyferium.Aplicacao.Produtos.Excecoes;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class ListarProdutoService : IListarProdutoService
{
    private readonly IProdutoRepositorio _produtoRepositorio;

    public ListarProdutoService(IProdutoRepositorio produtoRepositorio)
    {
        _produtoRepositorio = produtoRepositorio;
    }

    public async Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        var produtos = await _produtoRepositorio.ListarProdutosAsync();

        return produtos.ToList();
    }

    public async Task<ProdutoListagemResponse> ListarPorCodigoAsync(int codigoProduto)
    {
        ValidarCodigoProduto(codigoProduto);

        return await ObterProdutoPorCodigoAsync(codigoProduto);
    }

    private async Task<ProdutoListagemResponse> ObterProdutoPorCodigoAsync(int codigoProduto)
    {
        var produto = await _produtoRepositorio.ListarPorCodigoAsync(codigoProduto);

        if (produto is null)
            throw new ProdutoNaoEncontradoException(codigoProduto);

        return produto;
    }

    private static void ValidarCodigoProduto(int codigoProduto)
    {
        if (codigoProduto <= 0)
            throw new ArgumentException("O código do produto deve ser maior que zero.");
    }
}