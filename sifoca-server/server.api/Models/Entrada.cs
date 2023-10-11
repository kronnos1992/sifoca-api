using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.api.Models
{
    public class Entrada : TabelaBase
    {
        public string Operador { get; set; }
        public string TipoPagamento { get; set; }
        public string Assinante { get; set; }
        public Movimento Movimento { get; set; }
    }
}