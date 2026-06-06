using Moq;
using Pyferium.Aplicacao.Produtos.Excecoes;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos;

namespace Pyferium.Aplicacao.Tests.Produtos.Servicos;

public class ListarProdutoServiceTestes
{
    private readonly Mock<IProdutoConsultaRepositorio> _produtoConsultaRepositorioMock;
    private readonly ListarProdutoService _service;

    public ListarProdutoServiceTestes()
    {
        _produtoConsultaRepositorioMock = new Mock<IProdutoConsultaRepositorio>();

        _service = new ListarProdutoService(
            _produtoConsultaRepositorioMock.Object);
    }

    [Fact]
    public async Task ListarPorCodigoAsync_QuandoCodigoForInvalido_DeveLancarArgumentException()
    {
        // Arrange
        var codigoProduto = 0;

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.ListarPorCodigoAsync(codigoProduto));

        // Assert
        Assert.Equal("O código do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task ListarPorCodigoAsync_QuandoProdutoNaoExistir_DeveLancarProdutoNaoEncontradoException()
    {
        // Arrange
        var codigoProduto = 999;

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync((ProdutoListagemResponse?)null);

        // Act
        var exception = await Assert.ThrowsAsync<ProdutoNaoEncontradoException>(() =>
            _service.ListarPorCodigoAsync(codigoProduto));

        // Assert
        Assert.Equal($"Produto com código {codigoProduto} não encontrado.", exception.Message);
    }

    [Fact]
    public async Task ListarPorCodigoAsync_QuandoProdutoExistir_DeveRetornarProduto()
    {
        // Arrange
        var codigoProduto = 1;

        var produtoEsperado = new ProdutoListagemResponse
        {
            CodigoProduto = codigoProduto,
            NomeProduto = "Notebook",
            ValorProduto = 3500,
            CodigoCategoria = 1,
            DescricaoCategoria = "Eletrônicos",
            CodigoNivel = "01",
            IdtAtivo = "S"
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(produtoEsperado);

        // Act
        var produto = await _service.ListarPorCodigoAsync(codigoProduto);

        // Assert
        Assert.NotNull(produto);
        Assert.Equal(codigoProduto, produto.CodigoProduto);
        Assert.Equal("Notebook", produto.NomeProduto);
    }
}