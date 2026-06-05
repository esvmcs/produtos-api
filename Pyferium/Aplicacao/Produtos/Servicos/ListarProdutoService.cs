using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;
using Pyferium.Infraestrutura.Repositorios.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class ListarProdutoService : IListarProdutoService
{
    private readonly IProdutoRepositorio _produtoRepositorio;

    public ListarProdutoService(IProdutoRepositorio produtoRepositorio)
    {
        _produtoRepositorio = produtoRepositorio;
    }

    public async Task<IEnumerable<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        return await _produtoRepositorio.ListarProdutosAsync();
    }

    public async Task<IEnumerable<ProdutoListagemResponse>> ListarPorCodigoAsync(int codigoProduto)
    {
        if (codigoProduto <= 0)
            throw new ArgumentException("O código do produto deve ser maior que zero.");

        var produtos = await _produtoRepositorio.ListarPorCodigoAsync(codigoProduto);

        var listaProdutos = produtos.ToList();

        if (!listaProdutos.Any())
            throw new ArgumentException("Nenhum produto encontrado com o código fornecido.");

        return listaProdutos;
    }
}