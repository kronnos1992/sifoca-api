using Microsoft.EntityFrameworkCore;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;


public class ResumoFunctions : IResumoContract
{

    private readonly SifocaContext _contexto; 
    public ResumoFunctions(SifocaContext context)
    {
        _contexto = context;
    }

    public ResumoDTO GetResumo (DateTime dataInicial, DateTime dataFinal, string? op)
    {
        try
        {
            IQueryable<Entrada> queryEntradas = _contexto.Tb_Entrada
                .AsSplitQuery()
                .OrderByDescending(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal );

            IQueryable<Saida> querySaidas = _contexto.Tb_Saida
                .AsSplitQuery()
                .OrderByDescending(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);  

            if (!string.IsNullOrEmpty(op))
            {
                queryEntradas = queryEntradas.Where(p => p.Operador.ToLower().Contains(op.ToLower()));
                querySaidas = querySaidas.Where(p => p.Responsável.ToLower().Contains(op.ToLower()));
            }
            var todasEntradas = queryEntradas.ToList();
            var todasSaidas = querySaidas.ToList();
            decimal totalEntradas = todasEntradas.Sum(e => e.ValorEntrada);
            decimal totalSaidas = todasSaidas.Sum(s => s.ValorSaida);
            decimal remanescente = totalEntradas - totalSaidas;

            var resumo = new ResumoDTO
            {
                Remanescente = remanescente
            };
            
            return resumo;

        }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
    }
}
