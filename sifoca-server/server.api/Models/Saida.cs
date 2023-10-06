using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.api.Models
{
    public class Saida : TabelaBase
    {
        public string Responsável { get; set; }
        public Movimento Movimento { get; set; }
    }
}