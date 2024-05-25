using Microsoft.EntityFrameworkCore;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;

namespace server.api.Services.Functions
{
    public class RelatorioFunctions : IRelatorioContract
    {
        private readonly SifocaContext sifoca;

        public RelatorioFunctions(SifocaContext sifoca)
        {
            this.sifoca = sifoca;
        }
        public async Task<IEnumerable<Saida>> GetSaidasReport(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                if(op == null)
                {
                    var saidas = await sifoca.Tb_Saida
                    .OrderBy(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                    .ToListAsync();
                    if (saidas == null)
                    {   
                        return null;
                    }
                    return saidas;
                }
                else{
                    var saidas = await sifoca.Tb_Saida
                    
                    .OrderBy(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal && p.Responsável.ToLower().Contains(op.ToLower()))
                    .ToListAsync();
                    if (saidas == null)
                    {   
                        return null;
                    }
                    return saidas;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<IEnumerable<Entrada>> GetEntradasReport(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                if(op == null)
                {
                    var entradas = await sifoca.Tb_Entrada
                    .OrderBy(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                    .ToListAsync();
                    if (entradas == null)
                    {   
                        return null;
                    }
                    return entradas;
                }
                else{
                    var entradas = await sifoca.Tb_Entrada
                    
                    .OrderBy(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal && p.Operador.ToLower().Contains(op.ToLower()))
                    .ToListAsync();
                    if (entradas == null)
                    {   
                        return null;
                    }
                    return entradas;
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }

    }
}