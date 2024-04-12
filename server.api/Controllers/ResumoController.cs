using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.api.Services.Contracts;

namespace server.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumoController : ControllerBase
{
    private readonly IResumoContract resumoContract;

    public ILogger<ResumoController> logger { get; }

    public ResumoController(ILogger<ResumoController> logger,IResumoContract resumoContract)
    {
        this.resumoContract = resumoContract;
        this.logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "MASTER")]
    public IActionResult Get(DateTime dataInicial, DateTime dataFinal, string? operador)
    {
        try
        {
            var balancoDTO = this.resumoContract.GetResumo(dataInicial, dataFinal, operador);
            return Ok(balancoDTO);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}