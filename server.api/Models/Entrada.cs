namespace server.api.Models;

public class Entrada : TabelaBase
{
    public required string Operador { get; set; }
    public required string FormaPagamento { get; set; }
    public required string FonteEntrada { get; set; }
    public string ? DescricaoEntrada { get; set; }
    public decimal ValorEntrada { get; set; }
}