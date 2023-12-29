using server.api.DTOs;
using server.api.Models;

namespace server.api.Services.Contracts
{
    public interface IMovimentoContract
    {

        // Entradas
        Task CreateEntrada(EntradaDTO movimento);
        Task UpdateEntrada(EntradaDTO movimento, int id);
        Task DeleteEntrada(int id);
        Task<IEnumerable<Entrada>> GetEntradas(DateTime dataInicial, DateTime dataFinal);
        Task<IEnumerable<Entrada>> GetEntradas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<IEnumerable<Entrada>> GetOpEntradas(DateTime dataInicial, DateTime dataFinal);
        Task<IEnumerable<Entrada>> GetEntradas(DateTime dataInicial, DateTime dataFinal, string area);
        Task<IEnumerable<Entrada>> GetEntradas(DateTime dataInicial, string formaPagamento, DateTime dataFinal);
        Task<Entrada> GetEntradas(int id);

        //saídas
        Task CreateSaida(MovimentoDTO movimento);
        Task UpdateSaida(MovimentoDTO movimento, int id);
        Task DeleteSaida(int id);
        Task<IEnumerable<Saida>> GetSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<Saida> GetSaidas(int id);

        //GERAL
        Task<IEnumerable<Movimento>> GetMovimentos(DateTime? dataInicial, DateTime? dataFinal);
    }
}