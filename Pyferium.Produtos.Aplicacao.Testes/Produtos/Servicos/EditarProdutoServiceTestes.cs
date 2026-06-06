using Moq;
using Pyferium.Produtos.Aplicacao.Categorias.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Comandos;
using Pyferium.Produtos.Aplicacao.Produtos.Excecoes;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos;

namespace Pyferium.Produtos.Aplicacao.Testes.Produtos.Servicos;

public class EditarProdutoServiceTestes
{
    private readonly Mock<ICategoriaRepositorio> _categoriaRepositorioMock;
    private readonly Mock<IProdutoConsultaRepositorio> _produtoConsultaRepositorioMock;
    private readonly Mock<IProdutoComandoRepositorio> _produtoComandoRepositorioMock;
    private readonly EditarProdutoService _service;

    public EditarProdutoServiceTestes()
    {
        _categoriaRepositorioMock = new Mock<ICategoriaRepositorio>();
        _produtoConsultaRepositorioMock = new Mock<IProdutoConsultaRepositorio>();
        _produtoComandoRepositorioMock = new Mock<IProdutoComandoRepositorio>();

        _service = new EditarProdutoService(
            _categoriaRepositorioMock.Object,
            _produtoConsultaRepositorioMock.Object,
            _produtoComandoRepositorioMock.Object);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoCodigoForInvalido_DeveLancarArgumentException()
    {
        var request = new EditarProdutoRequest
        {
            NomeProduto = "Notebook"
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(0, request));

        Assert.Equal("O código do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoRequestForNulo_DeveLancarArgumentException()
    {
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(1, null!));

        Assert.Equal("Os dados do produto são obrigatórios.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoNenhumCampoForInformado_DeveLancarArgumentException()
    {
        var request = new EditarProdutoRequest();

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(1, request));

        Assert.Equal("Informe ao menos um campo para atualização.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoProdutoNaoExistir_DeveLancarProdutoNaoEncontradoException()
    {
        var codigoProduto = 99;

        var request = new EditarProdutoRequest
        {
            NomeProduto = "Notebook"
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync((ProdutoListagemResponse?)null);

        var exception = await Assert.ThrowsAsync<ProdutoNaoEncontradoException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("Produto com código 99 não encontrado.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoNomeTiverCaracterInvalido_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            NomeProduto = "Notebook@"
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("O nome do produto contém caracteres inválidos.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoCategoriaForInvalida_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            CodigoCategoria = 0
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("O código da categoria deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoCategoriaNaoExistir_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            CodigoCategoria = 99
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(99))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("A categoria 99 não existe ou está inativa.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoValorForNegativo_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            ValorProduto = -1
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("O valor do produto deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoIdtAtivoForInvalido_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            IdtAtivo = "X"
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal("O campo IdtAtivo deve ser 'S' ou 'N'.", exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoJaExistirProdutoComMesmoNomeNaCategoria_DeveLancarArgumentException()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            NomeProduto = "TV",
            CodigoCategoria = 2
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(2))
            .ReturnsAsync(true);

        _produtoConsultaRepositorioMock
            .Setup(x => x.ExisteProdutoAtivoComMesmoNomeAsync(
                "TV",
                2,
                codigoProduto))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.AtualizarProdutoAsync(codigoProduto, request));

        Assert.Equal(
            "Já existe outro produto ativo com o nome 'TV' nessa categoria.",
            exception.Message);
    }

    [Fact]
    public async Task AtualizarProdutoAsync_QuandoDadosForemValidos_DeveAtualizarProduto()
    {
        var codigoProduto = 1;

        var request = new EditarProdutoRequest
        {
            NomeProduto = "Notebook Gamer",
            CodigoCategoria = 2,
            ValorProduto = 4500,
            IdtAtivo = "S"
        };

        var responseEsperado = new ProdutoEditadoResponse
        {
            CodigoProduto = codigoProduto,
            NomeProduto = "Notebook Gamer",
            CodigoCategoria = 2,
            ValorProduto = 4500,
            IdtAtivo = "S"
        };

        _produtoConsultaRepositorioMock
            .Setup(x => x.ListarPorCodigoAsync(codigoProduto))
            .ReturnsAsync(CriarProdutoAtual(codigoProduto));

        _categoriaRepositorioMock
            .Setup(x => x.VerificarExistenciaCategoriaAsync(2))
            .ReturnsAsync(true);

        _produtoConsultaRepositorioMock
            .Setup(x => x.ExisteProdutoAtivoComMesmoNomeAsync(
                "Notebook Gamer",
                2,
                codigoProduto))
            .ReturnsAsync(false);

        _produtoComandoRepositorioMock
    .Setup(x => x.AtualizarProdutoAsync(
        codigoProduto,
        It.Is<EditarProdutoComando>(comando =>
            comando.NomeProduto == "Notebook Gamer" &&
            comando.CodigoCategoria == 2 &&
            comando.ValorProduto == 4500 &&
            comando.IdtAtivo == "S")))
    .ReturnsAsync(responseEsperado);

        var produtoEditado = await _service.AtualizarProdutoAsync(codigoProduto, request);

        Assert.NotNull(produtoEditado);
        Assert.Equal(codigoProduto, produtoEditado.CodigoProduto);
        Assert.Equal("Notebook Gamer", produtoEditado.NomeProduto);
        Assert.Equal(2, produtoEditado.CodigoCategoria);
        Assert.Equal(4500, produtoEditado.ValorProduto);
        Assert.Equal("S", produtoEditado.IdtAtivo);
    }

    private static ProdutoListagemResponse CriarProdutoAtual(int codigoProduto)
    {
        return new ProdutoListagemResponse
        {
            CodigoProduto = codigoProduto,
            NomeProduto = "Notebook",
            ValorProduto = 3500,
            CodigoCategoria = 1,
            DescricaoCategoria = "Eletrônicos",
            CodigoNivel = "01",
            IdtAtivo = "S"
        };
    }
}