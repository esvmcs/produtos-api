using Pyferium.Produtos.Aplicacao.Produtos.Excecoes;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos;

public class DeletarProdutoService : IDeletarProdutoService
{
    private readonly IProdutoComandoRepositorio _produtoComandoRepositorio;

    public DeletarProdutoService(IProdutoComandoRepositorio produtoComandoRepositorio)
    {
        _produtoComandoRepositorio = produtoComandoRepositorio;
    }

    public async Task<bool> DeletarProdutoAsync(int codigoProduto)
    {
        ValidarCodigoProduto(codigoProduto);

        var produtoDeletado = await _produtoComandoRepositorio
            .DeletarProdutoAsync(codigoProduto);

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