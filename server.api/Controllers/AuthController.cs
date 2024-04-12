
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using server.api.Services.Contracts;
using server.api.DTOs;

namespace siades.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(201)]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public class AuthController : ControllerBase
    {
        private readonly IAcessoContract acessoContract;


        public AuthController(IAcessoContract acessoContract)
        {
            this.acessoContract = acessoContract;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            try
            {
                var login = await acessoContract.LoginAsync(userDTO);
                if (!login.Success)
                {
                    return NotFound($"Usuario {userDTO.Username}, não encontrado. ");
                }
                return Ok(login);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        [HttpPost]
        [Route(nameof(SignUp))]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> SignUp([FromBody] UserDTO userDTO)
        {
            try
            {
                await acessoContract.RegisterAsync(userDTO);
                if (!ModelState.IsValid)
                {
                    return BadRequest("Erro ao cadastrar novo usuario");
                }
                return CreatedAtAction("SignUp", $"Usuário {userDTO.NomeCompleto} registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost("createrole")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            try
            {
                var role = await acessoContract.CreateRoleAsync(roleDTO.Perfil);
                if (!ModelState.IsValid)
                {
                    return BadRequest("Erro ao cadastrar o perfil!");
                }
                return CreatedAtAction("createrole", $"Perfil {roleDTO.Perfil} cadastrado!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            };
        }

        [HttpPost("signroletouser")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> SignRolesToUsers(string userId, string roleName)
        {
            try
            {
                await acessoContract.AssignRoleToUser(userId, roleName);
                if (!ModelState.IsValid)
                {
                    return BadRequest($"Erro ao atribuir roles aos usuários! {ModelState.Values}");
                }
                return Created("", $"permissão {roleName} atribuida com sucesso ao usuário {userId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }
        [HttpGet("getusers")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await acessoContract.GetUsers();
                if (!users.Any())
                {
                    return NotFound("nenhum registro encontrado");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro, por favor tente novamente , {ex.Message}");
            };
        }

        [HttpGet("getroles")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await acessoContract.GetRoles();
                if (!roles.Any())
                {
                    return NotFound($"nenhum registro encontrado");
                }
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ocorreu um erro, por favor tente novamente,  {ex.Message}");
            };
        }
        [HttpDelete("deleteuser")]
        [Authorize(Roles = "MASTER")]
        
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await acessoContract.DeleteUser(userId);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(error: $"Erro ao deletar {ex.Message}");
            }
        }
        [HttpDelete("deleterole")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                var role = await acessoContract.DeleteRole(roleId);
                if (role)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("updateuser")]
        [Authorize(Roles = "MASTER")]
        public async Task<IActionResult> UpdateteUser(string userId, UserDTO userDTO)
        {
            try
            {
                var user = await acessoContract.UpdateteUser(userId, userDTO);
                if (!user)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPut("updaterole")]
        [Authorize(Roles = "MASTER")] 
        public async Task<IActionResult> UpdateteRole([FromBody]RoleDTO _role, [FromQuery] string roleId)
        {
            var role = await acessoContract.UpdateteRole(_role, roleId);
            if (role)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}