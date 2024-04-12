using server.api.DTOs;
using server.api.Models;

namespace server.api.Services.Contracts
{
    public interface ISaidasContract
    {
        //saídas
        Task CreateSaida(SaidaDTO saida);
        Task UpdateSaida(SaidaDTO saida, int id);
        Task DeleteSaida(int id);
        Task<IEnumerable<Saida>> GetSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<IEnumerable<object>> GetSumSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<IEnumerable<object>> GetCountSaidasArea(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<Saida> GetSaidas(int id);
    }
}