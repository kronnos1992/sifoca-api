using Microsoft.AspNetCore.Identity;

namespace server.api.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string? NomeCompleto { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Departamento { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public IEnumerable<AppUserRole>? Roles { get; set; }
    }
}