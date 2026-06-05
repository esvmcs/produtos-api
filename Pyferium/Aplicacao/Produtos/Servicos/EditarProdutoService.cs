using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;
using Pyferium.Infraestrutura.Repositorios.Interfaces;

namespace Pyferium.Aplicacao.Produtos.Servicos;

public class EditarProdutoService : IEditarProdutoService
{
    private readonly ICategoriaRepositorio _categoriaRepositorio;
    private readonly IProdutoRepositorio _produtoRepositorio;

    public EditarProdutoService(
        ICategoriaRepositorio categoriaRepositorio,
        IProdutoRepositorio produtoRepositorio)
    {
        _categoriaRepositorio = categoriaRepositorio;
        _produtoRepositorio = produtoRepositorio;
    }

    public async Task<ProdutoEditadoResponse> AtualizarProdutoAsync(int codigoProduto, EditarProdutoRequest request)
    {
        if (codigoProduto <= 0)
            throw new ArgumentException("O código do produto deve ser maior que zero.");

        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

        var nenhumCampoInformado =
            string.IsNullOrWhiteSpace(request.NomeProduto) &&
            request.CodigoCategoria is null &&
            request.ValorProduto is null &&
            string.IsNullOrWhiteSpace(request.IdtAtivo);

        if (nenhumCampoInformado)
            throw new ArgumentException("Informe ao menos um campo para atualização.");

        if (!string.IsNullOrWhiteSpace(request.NomeProduto))
        {
            request.NomeProduto = request.NomeProduto.Trim();

            if (request.NomeProduto.Length > 80)
                throw new ArgumentException("O nome do produto deve conter no máximo 80 caracteres.");

            var contemCaracterInvalido = request.NomeProduto.Any(c =>
                !char.IsLetterOrDigit(c) &&
                !char.IsWhiteSpace(c) &&
                c != '-' &&
                c != '/' &&
                c != '.');

            if (contemCaracterInvalido)
                throw new ArgumentException("O nome do produto contém caracteres inválidos.");
        }

        if (request.CodigoCategoria.HasValue)
        {
            if (request.CodigoCategoria.Value <= 0)
                throw new ArgumentException("O código da categoria deve ser maior que zero.");

            var categoriaExiste = await _categoriaRepositorio.VerificarExistenciaCategoriaAsync(request.CodigoCategoria.Value);

            if (!categoriaExiste)
                throw new ArgumentException($"A categoria {request.CodigoCategoria.Value} não existe ou está inativa.");
        }

        if (request.ValorProduto.HasValue && request.ValorProduto.Value < 0)
            throw new ArgumentException("O valor do produto não pode ser negativo.");

        if (!string.IsNullOrWhiteSpace(request.IdtAtivo))
        {
            request.IdtAtivo = request.IdtAtivo.Trim().ToUpper();

            if (request.IdtAtivo != "S" && request.IdtAtivo != "N")
                throw new ArgumentException("O campo IdtAtivo deve ser 'S' ou 'N'.");
        }

        var produtoEditado = await _produtoRepositorio.AtualizarProdutoAsync(codigoProduto, request);

        if (produtoEditado is null)
            throw new ArgumentException($"Produto com código {codigoProduto} não foi encontrado.");

        return produtoEditado;
    }
}
