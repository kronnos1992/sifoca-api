using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Contracts;

namespace server.api.Services.Functions
{
    public class AcessoFunctions : IAcessoContract
    {
        private readonly SignInManager<AppUser>? signInManager;
        private readonly RoleManager<AppRole>? roleManager;
        private readonly IConfiguration configuration;
        //private readonly IMapper mapper;
        private readonly UserManager<AppUser>? userManager;
        public AcessoFunctions(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager)
        {

            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
           // this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<string> CreateRoleAsync(string roleName)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var role = await roleManager.CreateAsync(new AppRole { Name = "MASTER" });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (role.Succeeded)
                {
                    return role.Succeeded.ToString();
                }
                else
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return role.Errors.ToString();
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<string> AssignRoleToUser(string userId, string roleName)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                var role = await roleManager.FindByNameAsync(roleName);
                if (user == null)
                {
                    return "nenhum usuario encontrado";
                }
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    return "perfil não encontrada";
                }
                var result = await userManager.AddToRoleAsync(user, role.Name);
                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro de banco de dados", ex);
            }
        }
        public async Task<string> RegisterAsync(UserDTO __user)
        {
            try
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                var user = new AppUser
                {
                    NomeCompleto = __user.NomeCompleto,
                    UserName = __user.Usuario,
                    //NormalizedUserName = __user.Usuario,
                    Email = __user.Email,
                    PhoneNumber = __user.Telefone,
                    Departamento = __user.Departamento,
                    DataNascimento = __user.DataNascimento,
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    DataAtualizacao = null
                };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var userMaped = await userManager.CreateAsync(user, __user.Senha);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (userMaped.Succeeded)
                {
                    return $"Nome Completo: {user.NomeCompleto} \n Telefone: {user.PhoneNumber} \n Email: {user.Email}";
                }
                throw new Exception($"{userMaped.Errors}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<AppRole>> GetRoles()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var roles = await roleManager.Roles.Include(u => u.Users).ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (roles.Count == 0)
            {
                throw new Exception("nenhum registro encontrado. ");
            }
            return roles;
        }
        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var users = await userManager.Users.ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (users.Count == 0)
                {
                    throw new Exception("nenhum registro encontrado. ");
                };
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro de banco de dados", ex);
            }
        }
        public async Task<AuthResult> LoginAsync(LoginDTO login)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var user = await userManager
                    .FindByNameAsync(login.Username) 
                    ?? throw new Exception("Usuário não encontrado.");
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                var output = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (output.Succeeded)
                {
                    // verificar se o usuario for do tipo master e retonar verdadeiro se for.
                    var isInRole = await userManager.IsInRoleAsync(user, "MASTER");
                    var token = await GenerateToken(user); 
                    if (isInRole)
                    {
                        return new AuthResult
                        {
                            Success = true,
                            SuccessMessage = "Login efeituado com sucesso",
                            Token = token,
                            FullUserName = user.NomeCompleto,
                            UserName = user.UserName,
                            IsMaster = true
                        };
                    }
                    // retornar false se não for                  
                    else{
                        return new AuthResult
                        {
                            Success = true,
                            SuccessMessage = "Login efeituado com sucesso",
                            Token = token,
                            FullUserName = user.NomeCompleto,
                            UserName = user.UserName,
                            IsMaster = false
                        };
                    }                    
                }
                else
                {
                    return new AuthResult
                    {
                        Success = false,
                        ErrorMessage = "Nome de usuário ou senha incorretos."
                    };
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro de servidor, consulte o administrador para melhor esclarecimento", ex);
            }
        }
        public async Task<bool> DeleteUser(string userId)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var user = await userManager.FindByIdAsync(userId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (user == null)
                {
                    return false;
                }
                await userManager.DeleteAsync(user);
                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteRole(string roleId)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var role = await roleManager.FindByIdAsync(roleId)
                    ?? throw new NullReferenceException($"{roleId} does not exist");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                await roleManager.DeleteAsync(role);
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<bool> UpdateteUser(string userId, UserDTO userDTO)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var user = await userManager.FindByIdAsync(userId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (user == null)
                {
                    return false;
                }
                user.Email = userDTO.Email;
                //user.NormalizedUserName = userDTO.Usuario;
                user.NomeCompleto = userDTO.NomeCompleto;
                user.PhoneNumber = userDTO.Telefone;
                user.DataAtualizacao = Generic.GetCurrentAngolaDateTime();
                await userManager.UpdateAsync(user);
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"{ex.Message}\n{ex.StackTrace}");
            }
        }
        public async Task<bool> UpdateteRole(string roleId, RoleDTO roleDTO)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var role = await roleManager.FindByNameAsync(roleId);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (role == null)
            {
                return false;
            }
            role.Name = roleDTO.Perfil;
            await roleManager.UpdateAsync(role);
            return true;
        }
        public async Task<string> GenerateToken(AppUser user)
        {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            var claims = new List<Claim>{
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.GivenName, user.NomeCompleto),
                new(ClaimTypes.Name, user.UserName)
            };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var roles = await userManager.GetRolesAsync(user);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

#pragma warning disable CS8604 // Possible null reference argument.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value));
#pragma warning restore CS8604 // Possible null reference argument.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}