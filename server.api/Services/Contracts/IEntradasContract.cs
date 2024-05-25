using server.api.DTOs;
using server.api.Models;

namespace server.api.Services.Contracts
{
    public interface IEntradasContract
    {

        // Entradas
        Task CreateEntrada(EntradaDTO entrada);
        Task UpdateEntrada(EntradaDTO entrada, int id);
        Task DeleteEntrada(int id);
        Task<IEnumerable<Entrada>> GetEntradas(DateTime? dataInicial, DateTime? dataFinal);
        Task<IEnumerable<object>> GetSumEntradas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<IEnumerable<object>> GetCountEntradas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<Entrada> GetEntrada(int id);
    }
}