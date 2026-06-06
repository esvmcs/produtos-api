using Pyferium.Aplicacao.Produtos.Excecoes;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class DeletarProdutoService : IDeletarProdutoService
{
    private readonly IProdutoComandoRepositorio produtoComandoRepositorio;

    public DeletarProdutoService(IProdutoComandoRepositorio produtoComandoRepositorio)
    {
        this.produtoComandoRepositorio = produtoComandoRepositorio;
    }

    public async Task<bool> DeletarProdutoAsync(int codigoProduto)
    {
        ValidarCodigoProduto(codigoProduto);

        var produtoDeletado = await produtoComandoRepositorio.DeletarProdutoAsync(codigoProduto);

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