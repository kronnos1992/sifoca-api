using Microsoft.AspNetCore.Mvc;
using server.api.DTOs;
using server.api.Services.Contracts;

namespace server.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentosController : ControllerBase
    {
        private readonly IMovimentoContract contract;

        public MovimentosController(IMovimentoContract contract)
        {
            this.contract = contract;
        }

        #region ENTRADA ENDPOINTS

        [HttpGet("entrada/getall")]
        public async Task<IActionResult> GetAll(DateTime dataInicial, DateTime dataFinal)
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

        [HttpGet("entrada/getbyop")]
        public async Task<IActionResult> GetByOperator(string op, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var entradas = await contract.GetEntradas(op, dataInicial, dataFinal);
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

        [HttpGet("entrada/getbyarea")]
        public async Task<IActionResult> GetByArea(DateTime dataInicial, DateTime dataFinal, string area)
        {
            try
            {
                var entradas = await contract.GetEntradas(dataInicial, dataFinal, area);
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

        [HttpGet("entrada/getbyforma")]
        public async Task<IActionResult> GetByForma(string forma, DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var entradas = await contract.GetEntradas(forma, dataInicial, dataFinal);
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

        [HttpGet("entrada/getbyid")]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entrada = await contract.GetEntradas(id);
                if (entrada == null)
                {
                    return NoContent();
                }
                return Ok(entrada);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("entrada/{id:int}")]
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

        [HttpPost("entrada/")]
        public async Task<IActionResult> Insert(EntradaDTO movimento)
        {
            try
            {
                await contract.CreateEntrada(movimento);
                return Ok(movimento);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro de validação {ex.Message}");
            }
        }

        [HttpPut("entrada/{id}")]
        public async Task<IActionResult> Update(EntradaDTO movimento, int id)
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

        #region SAÍDA ENDPOINTS
        [HttpGet("saida/")]
        public async Task<IActionResult> GetAllSaidas()
        {
            try
            {
                var saidas = await contract.GetSaidas();
                if (saidas == null)
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

        [HttpGet("saida/{resp}")]
        public async Task<IActionResult> GetByResponsavel(string resp)
        {
            try
            {
                var saidas = await contract.GetSaidas(resp);
                if (saidas.Count() < 1)
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


        [HttpGet("saida/{id:int}")]
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


        [HttpDelete("saida/{id:int}")]
        public async Task<IActionResult> DeleteSaida(int id)
        {
            try
            {
                await contract.DeleteSaida(id);
                return Created("", "Registro Eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("saida/")]
        public async Task<IActionResult> InsertSaida(MovimentoDTO movimento)
        {
            try
            {
                await contract.CreateSaida(movimento);
                return Ok(movimento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("saida/{id}")]
        public async Task<IActionResult> UpdateSaida(MovimentoDTO movimento, int id)
        {
            try
            {
                await contract.UpdateSaida(movimento, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region MOVIMENTO ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> GetAllMovimentos(DateTime dataInicial, DateTime dataFinal)
        {
            try
            {
                var movimentos = await contract.GetMovimentos(dataInicial, dataFinal);
                if (movimentos == null)
                {
                    return NotFound();
                }
                return Ok(movimentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}