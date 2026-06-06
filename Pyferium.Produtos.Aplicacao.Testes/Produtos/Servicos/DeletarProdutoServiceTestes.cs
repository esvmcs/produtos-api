using Moq;
using Pyferium.Produtos.Aplicacao.Produtos.Excecoes;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos;

namespace Pyferium.Produtos.Aplicacao.Testes.Produtos.Servicos;

public class DeletarProdutoServiceTestes
{
    private readonly Mock<IProdutoComandoRepositorio> _produtoComandoRepositorioMock;
    private readonly DeletarProdutoService _service;

    public DeletarProdutoServiceTestes()
    {
        _produtoComandoRepositorioMock = new Mock<IProdutoComandoRepositorio>();

        _service = new DeletarProdutoService(
            _produtoComandoRepositorioMock.Object);
    }

    [Fact]
    public async Task DeletarProdutoAsync_QuandoCodigoForInvalido_DeveLancarArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.DeletarProdutoAsync(0));

        Assert.Equal("O código do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task DeletarProdutoAsync_QuandoProdutoNaoExistir_DeveLancarProdutoNaoEncontradoException()
    {
        var codigoProduto = 99;

        _produtoComandoRepositorioMock
            .Setup(x => x.DeletarProdutoAsync(codigoProduto))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<ProdutoNaoEncontradoException>(() =>
            _service.DeletarProdutoAsync(codigoProduto));

        Assert.Equal("Produto com código 99 não encontrado.", exception.Message);
    }

    [Fact]
    public async Task DeletarProdutoAsync_QuandoProdutoExistir_DeveRetornarTrue()
    {
        var codigoProduto = 1;

        _produtoComandoRepositorioMock
            .Setup(x => x.DeletarProdutoAsync(codigoProduto))
            .ReturnsAsync(true);

        var resultado = await _service.DeletarProdutoAsync(codigoProduto);

        Assert.True(resultado);

        _produtoComandoRepositorioMock.Verify(
            x => x.DeletarProdutoAsync(codigoProduto),
            Times.Once);
    }
}