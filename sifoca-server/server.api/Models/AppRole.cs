using Microsoft.AspNetCore.Identity;

namespace server.api.Models
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }

        public IEnumerable<AppUserRole>? Users { get; set; }
    }
}