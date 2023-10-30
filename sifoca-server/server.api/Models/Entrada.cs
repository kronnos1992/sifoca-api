namespace server.api.Models
{
    public class Entrada : TabelaBase
    {
        public required string Operador { get; set; }
        public required string FormaPagamento { get; set; }
        public required string TipoPagamento { get; set; }
        public required string Assinante { get; set; }
        public required Movimento Movimento { get; set; }
    }
}