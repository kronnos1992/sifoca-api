using Microsoft.AspNetCore.Identity;

namespace server.api.Models
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime DataRegistro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public IEnumerable<AppUserRole>? Users { get; set; }
    }
}