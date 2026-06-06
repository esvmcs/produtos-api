using Microsoft.AspNetCore.Mvc;
using Pyferium.Produtos.Aplicacao.Produtos.Requests;
using Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Produtos.API.Controllers;

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

    /// <summary>
    /// Cria um novo produto com base nos dados fornecidos e retorna o produto criado.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    [HttpPost]
    public async Task<IActionResult> CriarProdutoAsync([FromBody] CriarProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando criação de produto. Nome: {NomeProduto}",
            request.NomeProduto);

        var produtoCriado = await _criarProdutoService.CriarProdutoAsync(request);

        _logger.LogInformation(
            "Produto criado com sucesso. Código: {CodigoProduto}",
            produtoCriado.CodigoProduto);

        return CreatedAtRoute(
            routeName: "ListarProdutoPorCodigo",
            routeValues: new { codigoProduto = produtoCriado.CodigoProduto },
            value: produtoCriado);
    }

    /// <summary>
    /// Atualiza um produto existente com base no código do produto e nos dados fornecidos.
    /// </summary>
    /// <param name="codigoProduto"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    [HttpPatch("{codigoProduto:int}")]
    public async Task<IActionResult> AtualizarProdutoAsync(
    int codigoProduto,
    [FromBody] EditarProdutoRequest request)
    {
        _logger.LogInformation(
            "Iniciando atualização de produto. Código: {CodigoProduto}",
            codigoProduto);

        var produtoAtualizado = await _editarProdutoService
            .AtualizarProdutoAsync(codigoProduto, request);

        _logger.LogInformation(
            "Produto atualizado com sucesso. Código: {CodigoProduto}",
            produtoAtualizado.CodigoProduto);

        return Ok(produtoAtualizado);
    }

    /// <summary>
    /// Retorna uma lista de todos os produtos disponíveis.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Retorna os detalhes de um produto específico com base no código do produto fornecido.
    /// </summary>
    /// <param name="codigoProduto"></param>
    /// <returns></returns>
    [HttpGet("{codigoProduto:int}", Name = "ListarProdutoPorCodigo")]
    public async Task<IActionResult> ListarPorCodigoAsync(int codigoProduto)
    {
        _logger.LogInformation(
            "Iniciando busca de produto. Código: {CodigoProduto}",
            codigoProduto);

        var produto = await _listarProdutoService.ListarPorCodigoAsync(codigoProduto);

        _logger.LogInformation(
            "Produto encontrado. Código: {CodigoProduto}",
            produto.CodigoProduto);

        return Ok(produto);
    }

    /// <summary>
    /// Exclui um produto existente com base no código do produto fornecido.
    /// </summary>
    /// <param name="codigoProduto"></param>
    /// <returns></returns>
    [HttpDelete("{codigoProduto:int}")]
    public async Task<IActionResult> DeletarProdutoAsync(int codigoProduto)
    {
        _logger.LogInformation(
            "Iniciando exclusão de produto. Código: {CodigoProduto}",
            codigoProduto);

        await _deletarProdutoService.DeletarProdutoAsync(codigoProduto);

        _logger.LogInformation(
            "Produto excluído com sucesso. Código: {CodigoProduto}",
            codigoProduto);

        return NoContent();
    }
}