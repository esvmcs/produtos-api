using Pyferium.Produtos.Aplicacao.Produtos.Excecoes;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos;

public class ListarProdutoService : IListarProdutoService
{
    private readonly IProdutoConsultaRepositorio _produtoConsultaRepositorio;

    public ListarProdutoService(IProdutoConsultaRepositorio produtoConsultaRepositorio)
    {
        _produtoConsultaRepositorio = produtoConsultaRepositorio;
    }

    public async Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        return await _produtoConsultaRepositorio.ListarProdutosAsync();
    }

    public async Task<ProdutoListagemResponse> ListarPorCodigoAsync(int codigoProduto)
    {
        ValidarCodigoProduto(codigoProduto);

        var produto = await _produtoConsultaRepositorio.ListarPorCodigoAsync(codigoProduto);

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