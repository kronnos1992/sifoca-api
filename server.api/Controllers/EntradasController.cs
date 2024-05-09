
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Mvc;
using server.api.DTOs;
using server.api.Services.Contracts;
using server.api.Services.Functions;

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

        [HttpGet(nameof(GetEntradasReport))]
        public async Task<IActionResult> GetEntradasReport(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            var entradas = contract.GetEntradasReport(dataInicial, dataFinal, op);
            if (entradas != null)
            {
                //configurando o relatorio
                var webReport = new WebReport();
                webReport.Report.Load(Path.Combine(environment.ContentRootPath, "wwwroot/reports", "RPT_ENTRADAS.frx"));
                Generic.GenerateEntradasDataTableReports(entradas, webReport);

                webReport.Report.Prepare();

                // preparando o memory stream para o sistema fazer a leitura do ficheiro estatico
                MemoryStream memory  = new();
                webReport.Report.Export(new PDFSimpleExport(), memory);
                memory.Flush();

                byte[] arrayReport = memory.ToArray();
                return File(arrayReport, "application/zip", "RPT_ENTRADAS.frx");
            }
            return BadRequest("Erro ao gerar o relatorio");
        }
        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await contract.DeleteEntrada(id);
                return Created("", "Registro Eliminado");
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