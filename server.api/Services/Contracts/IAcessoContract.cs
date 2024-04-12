using server.api.DTOs;
using server.api.Models;

namespace server.api.Services.Contracts
{
    public interface IAcessoContract
    {
        public Task<string> RegisterAsync(UserDTO user);
        public Task<IEnumerable<AppRole>> GetRoles();
        public Task<IEnumerable<AppUser>> GetUsers();
        public Task<AuthResult> LoginAsync(LoginDTO login);
        Task<string> CreateRoleAsync(string roleName);
        Task AssignRoleToUser(string userId, string roleName);
        Task<bool> DeleteUser(string userId);
        Task<bool> DeleteRole(string roleId);
        Task<bool> UpdateteUser(string userId, UserDTO user);
        Task<bool> UpdateteRole(RoleDTO roleDTO, string roleId);
        Task<string> GenerateToken(AppUser appUser);
    }
}