namespace server.api.Models;

public class Saida : TabelaBase
{
    public required string Responsável { get; set; }
    public required string Beneficiario { get; set; }
    public string ? DescricaoSaida { get; set; }
    public decimal ValorSaida { get; set; }
}