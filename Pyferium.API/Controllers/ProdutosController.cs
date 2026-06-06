using Microsoft.AspNetCore.Mvc;
using Pyferium.Aplicacao.Produtos.Excecoes;
using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly ILogger<ProdutosController> _logger;
    private readonly IListarProdutoService _listarProdutoService;
    private readonly ICriarProdutoService _criarProdutoService;
    private readonly IEditarProdutoService _editarProdutoService;
    private readonly IDeletarProdutoService _deletarProdutoService;

    public ProdutosController(
        ILogger<ProdutosController> logger,
        IListarProdutoService listarProdutoService,
        ICriarProdutoService criarProdutoService,
        IEditarProdutoService editarProdutoService,
        IDeletarProdutoService deletarProdutoService)
    {
        _logger = logger;
        _listarProdutoService = listarProdutoService;
        _criarProdutoService = criarProdutoService;
        _editarProdutoService = editarProdutoService;
        _deletarProdutoService = deletarProdutoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProdutoAsync([FromBody] CriarProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando criação de produto. Nome: {NomeProduto}",
            request?.NomeProduto);

        try
        {
            var produtoCriado = await _criarProdutoService.CriarProdutoAsync(request);

            _logger.LogInformation(
                "Produto criado com sucesso. Código: {CodigoProduto}",
                produtoCriado.CodigoProduto);

            return CreatedAtRoute(
                routeName: "ListarProdutoPorCodigo",
                routeValues: new { codigoProduto = produtoCriado.CodigoProduto },
                value: produtoCriado);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Falha ao criar produto. Nome: {NomeProduto}. Erro: {Erro}",
                request?.NomeProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }

    [HttpPut("{codigoProduto:int}")]
    public async Task<IActionResult> AtualizarProdutoAsync(
        int codigoProduto,
        [FromBody] ProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando atualização de produto. Código: {CodigoProduto}",
            codigoProduto);

        try
        {
            var produtoAtualizado = await _editarProdutoService
                .AtualizarProdutoAsync(codigoProduto, request);

            _logger.LogInformation(
                "Produto atualizado com sucesso. Código: {CodigoProduto}",
                produtoAtualizado.CodigoProduto);

            return Ok(produtoAtualizado);
        }
        catch (ProdutoNaoEncontradoException ex)
        {
            _logger.LogInformation(
                "Produto não encontrado para atualização. Código: {CodigoProduto}",
                codigoProduto);

            return NotFound(new
            {
                Erro = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Falha ao atualizar produto. Código: {CodigoProduto}. Erro: {Erro}",
                codigoProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarProdutosAsync()
    {
        _logger.LogInformation("Iniciando listagem de produtos.");

        var produtos = await _listarProdutoService.ListarProdutosAsync();

        _logger.LogInformation(
            "Listagem de produtos finalizada. Total: {TotalProdutos}",
            produtos.Count);

        return Ok(produtos);
    }

    [HttpGet("{codigoProduto:int}", Name = "ListarProdutoPorCodigo")]
    public async Task<IActionResult> ListarPorCodigoAsync(int codigoProduto)
    {
        _logger.LogInformation(
            "Iniciando busca de produto. Código: {CodigoProduto}",
            codigoProduto);

        try
        {
            var produto = await _listarProdutoService.ListarPorCodigoAsync(codigoProduto);

            _logger.LogInformation(
                "Produto encontrado. Código: {CodigoProduto}",
                produto.CodigoProduto);

            return Ok(produto);
        }
        catch (ProdutoNaoEncontradoException ex)
        {
            _logger.LogInformation(
                "Produto não encontrado. Código: {CodigoProduto}",
                codigoProduto);

            return NotFound(new
            {
                Erro = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Código inválido ao buscar produto. Código: {CodigoProduto}. Erro: {Erro}",
                codigoProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }

    [HttpDelete("{codigoProduto:int}")]
    public async Task<IActionResult> DeletarProdutoAsync(int codigoProduto)
    {
        _logger.LogInformation(
            "Iniciando exclusão de produto. Código: {CodigoProduto}",
            codigoProduto);

        try
        {
            await _deletarProdutoService.DeletarProdutoAsync(codigoProduto);

            _logger.LogInformation(
                "Produto excluído com sucesso. Código: {CodigoProduto}",
                codigoProduto);

            return Ok(new
            {
                Mensagem = $"Produto com código {codigoProduto} excluído com sucesso."
            });
        }
        catch (ProdutoNaoEncontradoException ex)
        {
            _logger.LogInformation(
                "Produto não encontrado para exclusão. Código: {CodigoProduto}",
                codigoProduto);

            return NotFound(new
            {
                Erro = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Falha ao excluir produto. Código: {CodigoProduto}. Erro: {Erro}",
                codigoProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }
}