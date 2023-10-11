using System.ComponentModel.DataAnnotations.Schema;

namespace server.api.Models
{
    public class Movimento : TabelaBase
    {
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string Area { get; set; }
        public decimal Valor { get; set; }
        public decimal Caixa { get; set; }
        public List<Entrada> Entradas { get; set; }
        public List<Saida> Saidas { get; set; }

    }
}