using Microsoft.AspNetCore.Mvc;

namespace Pyferium.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("teste")]
    public IActionResult Teste()
    {
        _logger.LogInformation("Endpoint de teste acessado.");
        return Ok(new { Mensagem = "Endpoint de teste funcionando!" });
    }
}
