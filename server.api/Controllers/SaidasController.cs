using Microsoft.AspNetCore.Mvc;
using server.api.DTOs;
using server.api.Services.Contracts;

namespace server.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaidasController : ControllerBase
    {
        private readonly ISaidasContract contract;

        public SaidasController(ISaidasContract contract)
        {
            this.contract = contract;
        }

        #region SAÍDA ENDPOINTS
        
        [HttpGet]
        public async Task<IActionResult> GetAllSaidas(DateTime dataInicial, DateTime dataFinal, string? op)
        {
            try
            {
                var saidas = await contract.GetSaidas(dataInicial, dataFinal, op);
                if (!saidas.Any())
                {
                    return NoContent();
                }
                return Ok(saidas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route(nameof(GetTotalSaidaDia))]
        public async Task<IActionResult> GetTotalSaidaDia(DateTime dataInicial, DateTime dataFinal, string? op)
        {
            try
            {
                var saidas = await contract.GetSumSaidas(dataInicial, dataFinal, op);
                if (!saidas.Any())
                {
                    return NoContent();
                }
                return Ok(saidas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpGet]
        [Route(nameof(GetCountSaida))]
        public async Task<IActionResult> GetCountSaida(DateTime dataInicial, DateTime dataFinal, string? op)
        {
            try
            {
                var saidas = await contract.GetCountSaidasArea(dataInicial, dataFinal, op);
                if (!saidas.Any())
                {
                    return NoContent();
                }
                return Ok(saidas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route(nameof(GetOneSaida))]
        public async Task<IActionResult> GetOneSaida(int id)
        {
            try
            {
                var saida = await contract.GetSaidas(id);
                if (saida != null)
                {
                    return NoContent();
                }
                return Ok(saida);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteSaida(int id)
        {
            try
            {
                await contract.DeleteSaida(id);
                return Ok($"Registro nº{id} eliminado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir registro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertSaida(SaidaDTO movimento)
        {
            try
            {
                await contract.CreateSaida(movimento);
                return Created("", movimento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSaida([FromBody] SaidaDTO movimento, [FromQuery] int id)
        {
            try
            {
                await contract.UpdateSaida(movimento, id);
                return Created("",$"Registro Nº{id} atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        #endregion

    }
}