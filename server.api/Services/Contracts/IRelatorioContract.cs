using server.api.Models;

namespace server.api.Services.Contracts
{
    public interface IRelatorioContract
    {
        Task<IEnumerable<Entrada>> GetEntradasReport(DateTime? dataInicial, DateTime? dataFinal, string? op);
        Task<IEnumerable<Saida>> GetSaidasReport(DateTime? dataInicial, DateTime? dataFinal, string? op);
        //Task<IEnumerable<Entrada, Saida>> GetResumoReport(DateTime? dataInicial, DateTime? dataFinal, string? op);
    }
}