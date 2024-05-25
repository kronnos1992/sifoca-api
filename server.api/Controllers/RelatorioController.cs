using Microsoft.AspNetCore.Mvc;
using server.api.Services.Contracts;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using server.api.Services.Functions;
using Microsoft.AspNetCore.Authorization;

namespace server.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly ILogger<RelatorioController> _logger;
        private readonly IRelatorioContract contract;
        private readonly IWebHostEnvironment environment;

        public RelatorioController(ILogger<RelatorioController> logger, IRelatorioContract relatorio, IWebHostEnvironment environment)
        {
            _logger = logger;
            contract = relatorio;
            this.environment = environment;

        }

        [HttpGet(nameof(GetEntradasReport))]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> GetEntradasReport(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
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
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet(nameof(GetSaidasReport))]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> GetSaidasReport(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                var saidas = contract.GetSaidasReport(dataInicial, dataFinal, op);
                if (saidas != null)
                {
                    //configurando o relatorio
                    var webReport = new WebReport();
                    webReport.Report.Load(Path.Combine(environment.ContentRootPath, "wwwroot/reports", "RPT_SAIDAS.frx"));
                    Generic.GenerateSaidasDataTableReports(saidas, webReport);

                    webReport.Report.Prepare();

                    // preparando o memory stream para o sistema fazer a leitura do ficheiro estatico
                    MemoryStream memory  = new();
                    webReport.Report.Export(new PDFSimpleExport(), memory);
                    memory.Flush();

                    byte[] arrayReport = memory.ToArray();
                    return File(arrayReport, "application/zip", "RPT_SAIDAS.frx");
                }
                return BadRequest("Erro ao gerar o relatorio");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}