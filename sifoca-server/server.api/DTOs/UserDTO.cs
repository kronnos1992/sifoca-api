using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.api.DTOs
{
    public class UserDTO
    {
        public string? NomeCompleto { get; set; }
        public string? DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Usuario { get; set; }
        public string? Senha { get; set; }
    }
}