namespace server.api.Models
{
    public class Saida : TabelaBase
    {
        public required string Responsável { get; set; }
        public required Movimento Movimento { get; set; }
    }
}