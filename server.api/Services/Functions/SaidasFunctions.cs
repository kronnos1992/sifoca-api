using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;

namespace server.api.Services.Functions
{
    public class SaidasFunctions : ISaidasContract
    {
        private readonly SifocaContext sifoca;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<AppUser> userManager;
        public SaidasFunctions(SifocaContext sifoca, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.sifoca = sifoca;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        
        #region SAÍDAS
        public async Task CreateSaida(SaidaDTO saida)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            try
            {
                var _saida = new Saida
                {
                    Responsável = user.UserName.ToUpper(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    Beneficiario = saida.Beneficiario.ToUpper(),
                    DataAtualizacao = null,
                    DescricaoSaida = saida.DescricaoSaida.ToUpper(),
                    ValorSaida = saida.ValorSaida,

                };
                await sifoca.AddRangeAsync(_saida);
                await sifoca.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateSaida(SaidaDTO saida, int id)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            var _saida = await sifoca.Tb_Saida.FirstOrDefaultAsync(x => x.Id == id);
            try
            {
                if (_saida != null)
                {
                    _saida.Responsável = user.UserName.ToUpper();
                    _saida.Beneficiario = saida.Beneficiario.ToUpper();
                    _saida.DescricaoSaida = saida.DescricaoSaida.ToUpper();
                    _saida.ValorSaida = saida.ValorSaida;
                    _saida.DataAtualizacao = Generic.GetCurrentAngolaDateTime();

                    sifoca.UpdateRange(_saida);
                    await sifoca.SaveChangesAsync();
                }
                else
                {
                    throw new NullReferenceException("O movimento que deseja alterar não foi encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro de servidor, {ex.Message}");
            }
        }
        public async Task DeleteSaida(int id)
        {
            try
            {
                var saida = await sifoca.Tb_Saida.FirstOrDefaultAsync(x => x.Id == id);
                if (saida != null)
                {
                    sifoca.Tb_Saida.Remove(saida);
                    await sifoca.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Registro nº{id} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<IEnumerable<Saida>?> GetSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            try
            {
                // Verificar se o usuário possui o perfil "master"
                bool isMasterUser = await userManager.IsInRoleAsync(user, "MASTER".ToLower());

                IQueryable<Saida> query = sifoca.Tb_Saida
                    .OrderBy(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

                if (!isMasterUser)
                {
                    // Se o usuário não for "master", filtrar apenas as saídas do usuário logado
                    query = query.Where(p => p.Responsável.ToLower() == user.UserName.ToLower());
                }

                var saidas = await query.ToListAsync();

                return saidas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados: {ex.Message}");
            }
        }


        public async Task<IEnumerable<object>> GetSumSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                IQueryable<Saida> query = sifoca.Tb_Saida
                    .AsSplitQuery()
                    .OrderByDescending(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

                if (!string.IsNullOrEmpty(op))
                {
                    query = query.Where(p => p.Responsável.ToLower().Contains(op.ToLower()));
                }

                var saidasPorDia = await query
                    .GroupBy(p => p.DataRegistro.Date)
                    .Select(g => new
                    {
                        Data = g.Key,
                        Total = g.Sum(s => (double)s.ValorSaida)
                    }).ToListAsync();
                return saidasPorDia;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<IEnumerable<object>> GetCountSaidasArea(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                IQueryable<Saida> query = sifoca.Tb_Saida
                    .AsSplitQuery()
                    .OrderByDescending(x => x.DataRegistro)
                    .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal);

                if (!string.IsNullOrEmpty(op))
                {
                    query = query.Where(p => p.Responsável.ToLower().Contains(op.ToLower()));
                }

                var countSaidasArea = await query
                    .GroupBy(p => p.Responsável)
                    .Select(g => new
                    {
                        Area = g.Key,
                        //Total = g.Sum(s => s.saida.Valor) // Substitua 'Valor' pelo nome do campo que representa o valor da despesa
                        Total = g.Sum(s => (double)s.ValorSaida)
                    })
                .OrderByDescending(resultado => resultado.Total)
                .Take(5)
                .ToListAsync();
                return countSaidasArea;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<Saida?> GetSaidas(int id)
        {
            try
            {
                var saida = await sifoca.Tb_Saida
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.Id == id);
                if (saida == null)
                {
                    return null;
                }
                return saida;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        #endregion
    }
}