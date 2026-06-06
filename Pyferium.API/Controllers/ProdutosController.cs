using Microsoft.AspNetCore.Mvc;
using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.API.Controllers;

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
    public async Task<IActionResult> CriarProdutoAsync([FromBody] CriarProdutoRequest? request)
    {
        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

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

    [HttpPut("{codigoProduto:int}")]
    public async Task<IActionResult> AtualizarProdutoAsync(
        int codigoProduto,
        [FromBody] ProdutoRequest? request)
    {
        if (request is null)
            throw new ArgumentException("Os dados do produto são obrigatórios.");

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

        var produto = await _listarProdutoService.ListarPorCodigoAsync(codigoProduto);

        _logger.LogInformation(
            "Produto encontrado. Código: {CodigoProduto}",
            produto.CodigoProduto);

        return Ok(produto);
    }

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

        return Ok(new
        {
            Mensagem = $"Produto com código {codigoProduto} excluído com sucesso."
        });
    }
}