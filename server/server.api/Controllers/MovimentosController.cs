using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("entrada/")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entradas = await contract.GetEntradas();
                if (entradas.Count() < 1)
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

        [HttpGet("entrada/{op}")]
        public async Task<IActionResult> GetByOperator(string op)
        {
            try
            {
                var entradas = await contract.GetEntradas(op);
                if (entradas.Count() < 1)
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


        [HttpGet("entrada/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entradas = await contract.GetEntradas();
                if (entradas.Count() < 1)
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
        public async Task<IActionResult> Insert(MovimentoDTO movimento)
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
        public async Task<IActionResult> Update(MovimentoDTO movimento, int id)
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
    }
}