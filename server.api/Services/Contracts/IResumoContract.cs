using server.api.DTOs;

namespace server.api.Services.Contracts
{
    public interface IResumoContract
    {
        ResumoDTO GetResumo (DateTime data1, DateTime data2, string? op);
    }
}