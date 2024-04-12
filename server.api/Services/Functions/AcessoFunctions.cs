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

namespace server.api.Services.Functions;

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
            var role = await roleManager.CreateAsync(new AppRole { Name = roleName });
            if (role.Succeeded)
            {
                return role.Succeeded.ToString();
            }
            else
            {
                return role.Errors.ToString();
            }
        }
        catch (System.Exception ex)
        {
            throw new Exception("", ex);
        }
    }
    public async Task AssignRoleToUser(string userId, string roleName)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = await roleManager.FindByNameAsync(roleName);
            if (user == null)
            {
                throw new NullReferenceException("nenhum usuario encontrado");
            }
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                throw new NullReferenceException("perfil não encontrado");
            }
            var result = await userManager.AddToRoleAsync(user, role.Name);

        }
        catch (Exception ex)
        {
            throw new Exception($"{ex.Message}");
        }
    }
    public async Task<string> RegisterAsync(UserDTO __user)
    {
        try
        {
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
            var userMaped = await userManager.CreateAsync(user, __user.Senha);
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
        var roles = await roleManager.Roles.Include(u => u.Users).ToListAsync();
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
            var users = await userManager.Users.ToListAsync();
            if (users.Count == 0)
            {
                throw new Exception("nenhum registro encontrado. ");
            };
            return users;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro de banco de dados {ex.Message}");
        }
    }
    public async Task<AuthResult> LoginAsync(LoginDTO login)
    {
        try
        {
            var user = await userManager
                .FindByNameAsync(login.Username) 
                ?? throw new Exception("Usuário não encontrado.");
            var output = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
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
            var user = await userManager.FindByIdAsync(userId);
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
            var role = await roleManager.FindByIdAsync(roleId)
                ?? throw new NullReferenceException($"{roleId} does not exist");
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
            var user = await userManager.FindByIdAsync(userId);
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
    public async Task<bool> UpdateteRole(RoleDTO roleDTO, string roleId)
    {
        var role = await roleManager.FindByNameAsync(roleId);
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
        var claims = new List<Claim>{
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.GivenName, user.NomeCompleto),
            new(ClaimTypes.Name, user.UserName)
        };

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value));
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