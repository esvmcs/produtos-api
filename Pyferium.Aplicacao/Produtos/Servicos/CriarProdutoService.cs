using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Categorias.Repositorios;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class CriarProdutoService : ICriarProdutoService
{
    private readonly IProdutoComandoRepositorio _produtoComandoRepositorio;
    private readonly IProdutoConsultaRepositorio _produtoConsultaRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public CriarProdutoService(
        IProdutoComandoRepositorio produtoComandoRepositorio,
        IProdutoConsultaRepositorio produtoConsultaRepositorio,
        ICategoriaRepositorio categoriaRepositorio)
    {
        _produtoComandoRepositorio = produtoComandoRepositorio;
        _produtoConsultaRepositorio = produtoConsultaRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
    }

    public async Task<ProdutoCriadoResponse> CriarProdutoAsync(CriarProdutoRequest request)
    {
        ValidarRequest(request);

        var nomeProduto = NormalizarNomeProduto(request.NomeProduto);

        await ValidarCategoriaAsync(request.CodigoCategoria);
        await ValidarProdutoDuplicadoAsync(nomeProduto, request.CodigoCategoria);

        return await _produtoComandoRepositorio.CriarProdutoAsync(
            nomeProduto,
            request.CodigoCategoria,
            request.ValorProduto);
    }

    private static void ValidarRequest(CriarProdutoRequest request)
    {
        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

        if (request.CodigoCategoria <= 0)
            throw new ArgumentException("O código da categoria deve ser maior que zero.");

        if (request.ValorProduto < 0)
            throw new ArgumentException("O valor do produto não pode ser negativo.");

        ValidarNomeProduto(request.NomeProduto);
    }

    private async Task ValidarCategoriaAsync(int codigoCategoria)
    {
        var categoriaExiste = await _categoriaRepositorio
            .VerificarExistenciaCategoriaAsync(codigoCategoria);

        if (!categoriaExiste)
            throw new ArgumentException($"A categoria {codigoCategoria} não existe ou está inativa.");
    }

    private async Task ValidarProdutoDuplicadoAsync(string nomeProduto, int codigoCategoria)
    {
        var produtoJaExiste = await _produtoConsultaRepositorio
            .ExisteProdutoAtivoComMesmoNomeAsync(nomeProduto, codigoCategoria);

        if (produtoJaExiste)
            throw new ArgumentException(
                $"Já existe um produto ativo com o nome '{nomeProduto}' nessa categoria.");
    }

    private static void ValidarNomeProduto(string nomeProduto)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new ArgumentException("O nome do produto é obrigatório.");

        var nomeNormalizado = NormalizarNomeProduto(nomeProduto);

        if (nomeNormalizado.Length > 80)
            throw new ArgumentException("O nome do produto deve conter no máximo 80 caracteres.");

        var contemCaracterInvalido = nomeNormalizado.Any(c =>
            !char.IsLetterOrDigit(c) &&
            !char.IsWhiteSpace(c) &&
            c != '-' &&
            c != '/' &&
            c != '.');

        if (contemCaracterInvalido)
            throw new ArgumentException("O nome do produto contém caracteres inválidos.");
    }

    private static string NormalizarNomeProduto(string nomeProduto)
    {
        return nomeProduto.Trim();
    }
}