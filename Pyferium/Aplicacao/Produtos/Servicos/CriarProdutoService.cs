using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;
using Pyferium.Dominio.Entidades;
using Pyferium.Infraestrutura.Repositorios.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class CriarProdutoService : ICriarProdutoService
{
    private readonly IProdutoRepositorio _produtoRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public CriarProdutoService(
        IProdutoRepositorio produtoRepositorio,
        ICategoriaRepositorio categoriaRepositorio)
    {
        _produtoRepositorio = produtoRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
    }

    public async Task<ProdutoCriadoResponse> CriarProdutoAsync(CriarProdutoRequest request)
    {
        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

        if (request.CodigoCategoria <= 0)
            throw new ArgumentException("O código da categoria deve ser maior que zero.");

        if (request.ValorProduto < 0)
            throw new ArgumentException("O valor do produto não pode ser negativo.");

        ValidarNomeProduto(request.NomeProduto);

        var categoriaExiste = await _categoriaRepositorio.VerificarExistenciaCategoriaAsync(request.CodigoCategoria);

        var produtoJaExiste = await _produtoRepositorio.ExisteProdutoAtivoComMesmoNomeAsync(request.NomeProduto, request.CodigoCategoria);

        if (produtoJaExiste)
            throw new ArgumentException($"Já existe um produto ativo com o nome '{request.NomeProduto}' nessa categoria.");

        if (!categoriaExiste)
            throw new ArgumentException($"A categoria {request.CodigoCategoria} não existe ou está inativa.");

        return await _produtoRepositorio.CriarProdutoAsync(
            request.NomeProduto,
            request.CodigoCategoria,
            request.ValorProduto);
    }

    private static void ValidarNomeProduto(string nomeProduto)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new ArgumentException("O nome do produto é obrigatório.");

        nomeProduto = nomeProduto.Trim();

        if (nomeProduto.Length > 80)
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
}
