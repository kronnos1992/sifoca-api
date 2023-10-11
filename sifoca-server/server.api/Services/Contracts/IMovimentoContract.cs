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
        Task<IEnumerable<Entrada>> GetEntradas();
        Task<IEnumerable<Entrada>> GetEntradas(string op);
        Task<Entrada> GetEntradas(int id);

        //saídas
        Task CreateSaida(MovimentoDTO movimento);
        Task UpdateSaida(MovimentoDTO movimento, int id);
        Task DeleteSaida(int id);
        Task<IEnumerable<Saida>> GetSaidas();
        Task<IEnumerable<Saida>> GetSaidas(string op);
        Task<Saida> GetSaidas(int id);

        //GERAL
        Task<IEnumerable<Movimento>> GetMovimentos();
    }
}