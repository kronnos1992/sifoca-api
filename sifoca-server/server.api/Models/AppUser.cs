using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace server.api.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public string? BirthDate { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }

        public IEnumerable<AppUserRole>? Roles { get; set; }
    }
}