using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.api.Models
{
    public class TabelaBase
    {
        public int Id { get; set; }
        public string DataRegistro { get; set; }
        public string DataAtualizacao { get; set; }
    }
}