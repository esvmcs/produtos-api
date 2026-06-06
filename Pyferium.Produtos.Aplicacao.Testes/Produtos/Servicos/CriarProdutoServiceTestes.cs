using Moq;
using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;
using Pyferium.Produtos.Aplicacao.Categorias.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos;

namespace Pyferium.Produtos.Aplicacao.Testes.Produtos.Servicos;

public class CriarProdutoServiceTestes
{
    private readonly Mock<IProdutoComandoRepositorio> _produtoComandoRepositorioMock;
    private readonly Mock<IProdutoConsultaRepositorio> _produtoConsultaRepositorioMock;
    private readonly Mock<ICategoriaRepositorio> _categoriaRepositorioMock;
    private readonly CriarProdutoService _service;

    public CriarProdutoServiceTestes()
    {
        _produtoComandoRepositorioMock = new Mock<IProdutoComandoRepositorio>();
        _produtoConsultaRepositorioMock = new Mock<IProdutoConsultaRepositorio>();
        _categoriaRepositorioMock = new Mock<ICategoriaRepositorio>();

        _service = new CriarProdutoService(
            _produtoComandoRepositorioMock.Object,
            _produtoConsultaRepositorioMock.Object,
            _categoriaRepositorioMock.Object);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoRequestForNulo_DeveLancarArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(null!));

        Assert.Equal("Os dados do produto são obrigatórios.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoCategoriaForInvalida_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 0,
            ValorProduto = 3500
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("O código da categoria deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoValorForNegativo_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 1,
            ValorProduto = -10
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("O valor do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoNomeForVazio_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "",
            CodigoCategoria = 1,
            ValorProduto = 100
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("O nome do produto é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoNomeTiverCaracterInvalido_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook@",
            CodigoCategoria = 1,
            ValorProduto = 100
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("O nome do produto contém caracteres inválidos.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoCategoriaNaoExistir_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 99,
            ValorProduto = 3500
        };

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(request.CodigoCategoria))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("A categoria 99 não existe ou está inativa.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoProdutoJaExistirNaCategoria_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 1,
            ValorProduto = 3500
        };

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(request.CodigoCategoria))
            .ReturnsAsync(true);

        _produtoConsultaRepositorioMock
            .Setup(x => x.ExisteProdutoAtivoComMesmoNomeAsync(
                request.NomeProduto,
                request.CodigoCategoria,
                null))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal(
            "Já existe um produto ativo com o nome 'Notebook' nessa categoria.",
            exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoNomeTiverEspacos_DeveNormalizarNome()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "  Notebook  ",
            CodigoCategoria = 1,
            ValorProduto = 3500
        };

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(request.CodigoCategoria))
            .ReturnsAsync(true);

        _produtoConsultaRepositorioMock
            .Setup(x => x.ExisteProdutoAtivoComMesmoNomeAsync(
                "Notebook",
                request.CodigoCategoria,
                null))
            .ReturnsAsync(false);

        _produtoComandoRepositorioMock
            .Setup(x => x.CriarProdutoAsync(
                "Notebook",
                request.CodigoCategoria,
                request.ValorProduto))
            .ReturnsAsync(new ProdutoCriadoResponse
            {
                CodigoProduto = 1,
                NomeProduto = "Notebook",
                CodigoCategoria = 1,
                ValorProduto = 3500,
                IdtAtivo = "S"
            });

        var produtoCriado = await _service.CriarProdutoAsync(request);

        Assert.Equal("Notebook", produtoCriado.NomeProduto);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoValorForZero_DeveLancarArgumentException()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 1,
            ValorProduto = 0
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CriarProdutoAsync(request));

        Assert.Equal("O valor do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task CriarProdutoAsync_QuandoDadosForemValidos_DeveCriarProduto()
    {
        var request = new CriarProdutoRequest
        {
            NomeProduto = "Notebook",
            CodigoCategoria = 1,
            ValorProduto = 3500
        };

        var responseEsperado = new ProdutoCriadoResponse
        {
            CodigoProduto = 10,
            NomeProduto = "Notebook",
            CodigoCategoria = 1,
            ValorProduto = 3500,
            IdtAtivo = "S"
        };

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(request.CodigoCategoria))
            .ReturnsAsync(true);

        _produtoConsultaRepositorioMock
            .Setup(x => x.ExisteProdutoAtivoComMesmoNomeAsync(
                request.NomeProduto,
                request.CodigoCategoria,
                null))
            .ReturnsAsync(false);

        _produtoComandoRepositorioMock
            .Setup(x => x.CriarProdutoAsync(
                request.NomeProduto,
                request.CodigoCategoria,
                request.ValorProduto))
            .ReturnsAsync(responseEsperado);

        var produtoCriado = await _service.CriarProdutoAsync(request);

        Assert.NotNull(produtoCriado);
        Assert.Equal(10, produtoCriado.CodigoProduto);
        Assert.Equal("Notebook", produtoCriado.NomeProduto);
        Assert.Equal(1, produtoCriado.CodigoCategoria);
        Assert.Equal(3500, produtoCriado.ValorProduto);
        Assert.Equal("S", produtoCriado.IdtAtivo);
    }
}