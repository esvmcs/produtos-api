using Microsoft.AspNetCore.Mvc;
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

    public ProdutosController(
        ILogger<ProdutosController> logger,
        IListarProdutoService listarProdutoService,
        ICriarProdutoService criarProdutoService,
        IEditarProdutoService editarProdutoService)
    {
        _logger = logger;
        _listarProdutoService = listarProdutoService;
        _criarProdutoService = criarProdutoService;
        _editarProdutoService = editarProdutoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProdutoAsync([FromBody] CriarProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando a criação de um novo produto com nome: {NomeProduto}.",
            request?.NomeProduto);

        try
        {
            var produtoCriado = await _criarProdutoService.CriarProdutoAsync(request);

            _logger.LogInformation(
                "Produto criado com sucesso. Código do produto: {CodigoProduto}.",
                produtoCriado.CodigoProduto);

            return CreatedAtRoute(
                routeName: "ListarProdutoPorCodigo",
                routeValues: new { codigoProduto = produtoCriado.CodigoProduto },
                value: produtoCriado);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Erro ao criar produto com nome: {NomeProduto}. Erro: {ErrorMessage}",
                request?.NomeProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }

    [HttpPut]
    [Route("{codigoProduto:int}")]
    public async Task<IActionResult> AtualizarProdutoAsync(int codigoProduto, [FromBody] EditarProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando a atualização do produto com código: {CodigoProduto}.",
            codigoProduto);

        try
        {
            var produtoAtualizado = await _editarProdutoService.AtualizarProdutoAsync(codigoProduto, request);

            if (produtoAtualizado == null)
                return NotFound(new
                {
                    Erro = $"Produto com código {codigoProduto} não encontrado."
                });

            _logger.LogInformation(
                "Produto atualizado com sucesso. Código do produto: {CodigoProduto}.",
                produtoAtualizado.CodigoProduto);

            return Ok(produtoAtualizado);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Erro ao atualizar produto com código: {CodigoProduto}. Erro: {ErrorMessage}",
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
        _logger.LogInformation("Iniciando a listagem de produtos.");

        var produtos = await _listarProdutoService.ListarProdutosAsync();
        var listaProdutos = produtos.ToList();

        _logger.LogInformation(
            "Finalizando a listagem de produtos. Total de produtos encontrados: {TotalProdutos}.",
            listaProdutos.Count);

        return Ok(listaProdutos);
    }

    [HttpGet("{codigoProduto:int}", Name = "ListarProdutoPorCodigo")]
    public async Task<IActionResult> ListarPorCodigoAsync(int codigoProduto)
    {
        _logger.LogInformation(
            "Iniciando a busca por produto com código: {CodigoProduto}.",
            codigoProduto);

        try
        {
            var produto = await _listarProdutoService.ListarPorCodigoAsync(codigoProduto);

            _logger.LogInformation(
                "Produto encontrado com código: {CodigoProduto}.",
                codigoProduto);

            return Ok(produto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                "Erro ao buscar produto com código: {CodigoProduto}. Erro: {ErrorMessage}",
                codigoProduto,
                ex.Message);

            return BadRequest(new
            {
                Erro = ex.Message
            });
        }
    }
}