using Pyferium.Aplicacao.Produtos.Excecoes;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class DeletarProdutoService : IDeletarProdutoService
{
    private readonly IProdutoRepositorio _produtoRepositorio;

    public DeletarProdutoService(IProdutoRepositorio produtoRepositorio)
    {
        _produtoRepositorio = produtoRepositorio;
    }

    public async Task<bool> DeletarProdutoAsync(int codigoProduto)
    {
        ValidarCodigoProduto(codigoProduto);

        var produtoDeletado = await _produtoRepositorio.DeletarProdutoAsync(codigoProduto);

        if (!produtoDeletado)
            throw new ProdutoNaoEncontradoException(codigoProduto);

        return true;
    }

    private static void ValidarCodigoProduto(int codigoProduto)
    {
        if (codigoProduto <= 0)
            throw new ArgumentException("O código do produto deve ser maior que zero.");
    }
}