using Pyferium.Produtos.Aplicacao.Categorias.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Comandos;
using Pyferium.Produtos.Aplicacao.Produtos.Excecoes;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos;

public class EditarProdutoService : IEditarProdutoService
{
    private const int TamanhoMaximoNomeProduto = 80;
    private const string ProdutoAtivo = "S";
    private const string ProdutoInativo = "N";

    private readonly ICategoriaRepositorio _categoriaRepositorio;
    private readonly IProdutoConsultaRepositorio _produtoConsultaRepositorio;
    private readonly IProdutoComandoRepositorio _produtoComandoRepositorio;

    public EditarProdutoService(
        ICategoriaRepositorio categoriaRepositorio,
        IProdutoConsultaRepositorio produtoConsultaRepositorio,
        IProdutoComandoRepositorio produtoComandoRepositorio)
    {
        _categoriaRepositorio = categoriaRepositorio;
        _produtoConsultaRepositorio = produtoConsultaRepositorio;
        _produtoComandoRepositorio = produtoComandoRepositorio;
    }

    public async Task<ProdutoEditadoResponse> AtualizarProdutoAsync(
        int codigoProduto,
        EditarProdutoRequest request)
    {
        ValidarCodigoProduto(codigoProduto);
        ValidarRequest(request);

        var produtoAtual = await _produtoConsultaRepositorio.ListarPorCodigoAsync(codigoProduto);

        if (produtoAtual is null)
            throw new ProdutoNaoEncontradoException(codigoProduto);

        var requestNormalizado = NormalizarRequest(request);

        ValidarNomeProduto(requestNormalizado.NomeProduto);
        ValidarValorProduto(requestNormalizado.ValorProduto);
        ValidarIdtAtivo(requestNormalizado.IdtAtivo);

        if (requestNormalizado.CodigoCategoria.HasValue)
            await ValidarCategoriaAsync(requestNormalizado.CodigoCategoria.Value);

        await ValidarProdutoDuplicadoAsync(
            codigoProduto,
            produtoAtual.NomeProduto,
            produtoAtual.CodigoCategoria,
            requestNormalizado);

        var comando = new EditarProdutoComando
        {
            NomeProduto = requestNormalizado.NomeProduto,
            CodigoCategoria = requestNormalizado.CodigoCategoria,
            ValorProduto = requestNormalizado.ValorProduto,
            IdtAtivo = requestNormalizado.IdtAtivo
        };

        var produtoEditado = await _produtoComandoRepositorio.AtualizarProdutoAsync(
            codigoProduto,
            comando);

        if (produtoEditado is null)
            throw new ProdutoNaoEncontradoException(codigoProduto);

        return produtoEditado;
    }

    private static void ValidarCodigoProduto(int codigoProduto)
    {
        if (codigoProduto <= 0)
            throw new ArgumentException("O código do produto deve ser maior que zero.");
    }
    private static void ValidarRequest(EditarProdutoRequest? request)
    {
        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

        var nenhumCampoInformado =
            string.IsNullOrWhiteSpace(request.NomeProduto) &&
            request.CodigoCategoria is null &&
            request.ValorProduto is null &&
            string.IsNullOrWhiteSpace(request.IdtAtivo);

        if (nenhumCampoInformado)
            throw new ArgumentException("Informe ao menos um campo para atualização.");
    }

    private static EditarProdutoRequest NormalizarRequest(EditarProdutoRequest request)
    {
        return new EditarProdutoRequest
        {
            NomeProduto = string.IsNullOrWhiteSpace(request.NomeProduto)
                ? null
                : request.NomeProduto.Trim(),

            CodigoCategoria = request.CodigoCategoria,

            ValorProduto = request.ValorProduto,

            IdtAtivo = string.IsNullOrWhiteSpace(request.IdtAtivo)
                ? null
                : request.IdtAtivo.Trim().ToUpper()
        };
    }

    private static void ValidarNomeProduto(string? nomeProduto)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            return;

        if (nomeProduto.Length > TamanhoMaximoNomeProduto)
            throw new ArgumentException("O nome do produto deve conter no máximo 80 caracteres.");

        var contemCaracterInvalido = nomeProduto.Any(c =>
            !char.IsLetterOrDigit(c) &&
            !char.IsWhiteSpace(c) &&
            c != '-' &&
            c != '/' &&
            c != '.');

        if (contemCaracterInvalido)
            throw new ArgumentException("O nome do produto contém caracteres inválidos.");
    }

    private static void ValidarValorProduto(decimal? valorProduto)
    {
        if (valorProduto.HasValue && valorProduto.Value < 0)
            throw new ArgumentException("O valor do produto não pode ser negativo.");
    }

    private static void ValidarIdtAtivo(string? idtAtivo)
    {
        if (string.IsNullOrWhiteSpace(idtAtivo))
            return;

        if (idtAtivo != ProdutoAtivo && idtAtivo != ProdutoInativo)
            throw new ArgumentException("O campo IdtAtivo deve ser 'S' ou 'N'.");
    }

    private async Task ValidarCategoriaAsync(int codigoCategoria)
    {
        if (codigoCategoria <= 0)
            throw new ArgumentException("O código da categoria deve ser maior que zero.");

        var categoriaExiste = await _categoriaRepositorio
            .VerificarExistenciaCategoriaAsync(codigoCategoria);

        if (!categoriaExiste)
            throw new ArgumentException($"A categoria {codigoCategoria} não existe ou está inativa.");
    }

    private async Task ValidarProdutoDuplicadoAsync(
        int codigoProduto,
        string nomeProdutoAtual,
        int codigoCategoriaAtual,
        EditarProdutoRequest request)
    {
        var nomeFinal = request.NomeProduto ?? nomeProdutoAtual;
        var categoriaFinal = request.CodigoCategoria ?? codigoCategoriaAtual;

        var alterouNomeOuCategoria =
            request.NomeProduto is not null ||
            request.CodigoCategoria.HasValue;

        if (!alterouNomeOuCategoria)
            return;

        var produtoJaExiste = await _produtoConsultaRepositorio
            .ExisteProdutoAtivoComMesmoNomeAsync(
                nomeFinal,
                categoriaFinal,
                codigoProdutoIgnorar: codigoProduto);

        if (produtoJaExiste)
        {
            throw new ArgumentException(
                $"Já existe outro produto ativo com o nome '{nomeFinal}' nessa categoria.");
        }
    }
}