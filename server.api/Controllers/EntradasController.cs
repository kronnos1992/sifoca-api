using Microsoft.AspNetCore.Mvc;
using server.api.DTOs;
using server.api.Services.Contracts;

namespace server.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntradasController : ControllerBase
    {
        private readonly ILogger<EntradasController> _logger;
        private readonly IEntradasContract contract;
        private readonly IWebHostEnvironment environment;

        public EntradasController(ILogger<EntradasController> logger, IEntradasContract entradasContract,
             IWebHostEnvironment environment)
        {
            _logger = logger;
            contract = entradasContract;
            this.environment = environment;

        }
        
        #region ENTRADA ENDPOINTS

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime dataInicial, [FromQuery] DateTime dataFinal)
        {
            try
            {
                var entradas = await contract.GetEntradas(dataInicial, dataFinal);
                if (!entradas.Any())
                {
                    return NoContent();
                }
                return Ok(entradas); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route(nameof(GetChart))]
        public async Task<IActionResult> GetChart(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                var entradas = await contract.GetSumEntradas(dataInicial, dataFinal, op);
                if (!entradas.Any())
                {
                    return NoContent();
                }
                return Ok(entradas); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route(nameof(GetCount))]
        public async Task<IActionResult> GetCount(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                var entradas = await contract.GetCountEntradas(dataInicial, dataFinal, op);
                if (!entradas.Any())
                {
                    return NoContent();
                }
                return Ok(entradas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route(nameof(GetSum))]
        public async Task<IActionResult> GetSum(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                var entradas = await contract.GetSumEntradas(dataInicial, dataFinal, op);
                if (!entradas.Any())
                {
                    return NoContent();
                }
                return Ok(entradas); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entrada = await contract.GetEntrada(id);
                return Ok(entrada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await contract.DeleteEntrada(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(EntradaDTO movimento)
        {
            try
            {
                await contract.CreateEntrada(movimento);
                return Ok(movimento);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EntradaDTO movimento, [FromQuery]int id)
        {
            try
            {
                await contract.UpdateEntrada(movimento, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        #endregion

    }
}