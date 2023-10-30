using Microsoft.AspNetCore.Identity;

namespace server.api.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public override int UserId { get; set; }
        public override int RoleId { get; set; }

        public DateTime? DataRegistro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public AppRole? Role { get; set; }
        public AppUser? User { get; set; }
    }
}