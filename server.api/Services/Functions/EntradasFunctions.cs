using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;

namespace server.api.Services.Functions;

public class EntradasFunctions : IEntradasContract
{
    private readonly SifocaContext sifoca;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly UserManager<AppUser> userManager;

    public EntradasFunctions(SifocaContext sifoca, UserManager<AppUser> userManager, IHttpContextAccessor
         httpContextAccessor, ILogger<EntradasFunctions> logger)
    {
        this.sifoca = sifoca;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    #region ENTRADAS
    public async Task<IEnumerable<Entrada>?> GetEntradas(DateTime? dataInicial, DateTime? dataFinal)
    {

        // Obter o usuário logado a partir do contexto HTTP
        var username = httpContextAccessor.HttpContext.User.Identity.Name;
        var user = await userManager.FindByNameAsync(username);

        try
        {
            // Verificar se o usuário possui o perfil "master"
            bool isMasterUser = await userManager.IsInRoleAsync(user, "MASTER".ToLower());

            IQueryable<Entrada> query = sifoca.Tb_Entrada
                .OrderBy(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

            if (!isMasterUser)
                {
                    // Se o usuário não for "master", filtrar apenas as saídas do usuário logado
                    query = query.Where(p => p.Operador.ToLower() == user.NomeCompleto.ToLower());
                }
            var entradas = await query.ToListAsync();
            return entradas;            
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro na busca dos dados {ex.Message}");
        }
    }
    public async Task<IEnumerable<object>> GetSumEntradas(DateTime? dataInicial, DateTime? dataFinal, string? op)
    {
        try
        {
            IQueryable<Entrada> query = sifoca.Tb_Entrada
                .AsSplitQuery()
                .OrderByDescending(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

            if (!string.IsNullOrEmpty(op))
            {
                query = query.Where(p => p.Operador.ToLower().Contains(op.ToLower()));
            }

            var entradasPorDia = await query
                .GroupBy(p => new { p.DataRegistro.Date, p.FormaPagamento })
                .Select(g => new
                {
                    Data = g.Key.Date,
                    Form_de_Pagamento = g.Key.FormaPagamento,
                    Total = g.Sum(s => (double)s.ValorEntrada),
                    Count = g.Count() // Count of sales per area
                })
                .OrderByDescending(resultado => resultado.Total)
                .Take(10)
                .ToListAsync();

            return entradasPorDia;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro na busca dos dados {ex.Message}");
        }
    }
    public async Task<IEnumerable<object>> GetCountEntradas(DateTime? dataInicial, DateTime? dataFinal, string? op)
    {
        try
        {
            IQueryable<Entrada> query = sifoca.Tb_Entrada
                .AsSplitQuery()
                .OrderByDescending(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

            if (!string.IsNullOrEmpty(op))
            {
                query = query.Where(p => p.Operador.ToLower().Contains(op.ToLower()));
            }

            var countEntradasArea = await query
                .GroupBy(p => p.FormaPagamento.ToUpper())
                .Select(g => new
                {
                    Pagamento = g.Key,
                    //Total = g.Sum(s => s._entrada.Valor) // Substitua 'Valor' pelo nome do campo que representa o valor da despesa
                    Total = g.Sum(s => (double)s.ValorEntrada)
                })
                .OrderByDescending(resultado => resultado.Total)
                .Take(5)
                .ToListAsync();
            return countEntradasArea;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro na busca dos dados {ex.Message}");
        }
    }
    public async Task<Entrada?> GetEntrada(int id)
    {
        try
        {
            var entrada = await sifoca.Tb_Entrada
                .FindAsync(id);
            if (entrada.Id <1)
            {
                return null;
            }
            return entrada;
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro na busca dos dados {ex.Message}");
        }
    }
    public async Task CreateEntrada(EntradaDTO _entrada)
    {
        try
        {

            //buscar o usuario logado pelo contexto do http
            string username = httpContextAccessor.HttpContext.User.Identity.Name
                ??throw new Exception("Usuário não encontrado");
        
            var user = await userManager.FindByNameAsync(username)
                ??throw new Exception("Nenhum usuario encontrado");

            var entrada = new Entrada
            {
                Operador = user.NomeCompleto,
                FonteEntrada = _entrada.FonteEntrada.ToUpper(),
                DataRegistro = Generic.GetCurrentAngolaDateTime(),
                FormaPagamento = _entrada.FormaPagamento.ToUpper(),
                DataAtualizacao = null,
                DescricaoEntrada = _entrada.DescricaoEntrada.ToUpper(),
                ValorEntrada = _entrada.ValorEntrada,
                
            };

            //sifoca.UpdateRange(fundo);
            await sifoca.AddRangeAsync(entrada);
            await sifoca.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task UpdateEntrada(EntradaDTO _entrada, int id)
    {
        try
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            var entrada = await sifoca.Tb_Entrada.FirstOrDefaultAsync(x => x.Id == id);

            entrada.Operador = user.UserName.ToUpper();
            entrada.DescricaoEntrada = _entrada.DescricaoEntrada.ToUpper();
            entrada.ValorEntrada = _entrada.ValorEntrada;
            entrada.DataAtualizacao = Generic.GetCurrentAngolaDateTime();
            entrada.FormaPagamento = _entrada.FormaPagamento.ToUpper();
            sifoca.UpdateRange(entrada);
            await sifoca.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro de servidor, {ex.Message}");
        }
    }
    public async Task DeleteEntrada(int id)
    {
        try
        {
            var entrada = await sifoca.Tb_Entrada.FindAsync(id);
            if (entrada != null)
            {
                sifoca.Tb_Entrada.Remove(entrada);
                await sifoca.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro de servidor, {ex.Message}");
        }
    }

    #endregion
}