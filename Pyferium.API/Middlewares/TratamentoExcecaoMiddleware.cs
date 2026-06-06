using System.Net;
using System.Text.Json;
using Pyferium.Aplicacao.Produtos.Excecoes;

namespace Pyferium.API.Middlewares;

public class TratamentoExcecaoMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TratamentoExcecaoMiddleware> _logger;

    public TratamentoExcecaoMiddleware(
        RequestDelegate next,
        ILogger<TratamentoExcecaoMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception ex)
    {
        var statusCode = ObterStatusCode(ex);

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(ex, "Erro interno não tratado.");
        }
        else
        {
            _logger.LogWarning(ex, "Erro tratado pela aplicação.");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            erro = ex.Message,
            statusCode = context.Response.StatusCode
        };

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private static HttpStatusCode ObterStatusCode(Exception ex)
    {
        return ex switch
        {
            ProdutoNaoEncontradoException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}