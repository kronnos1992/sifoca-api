using server.api.Models;

namespace server.api.DTOs;

public class ResumoDTO
{
    public List<Entrada>? Entradas { get; set; }
    public List<Saida>? Saidas { get; set; }
    public decimal Remanescente { get; set; }
}