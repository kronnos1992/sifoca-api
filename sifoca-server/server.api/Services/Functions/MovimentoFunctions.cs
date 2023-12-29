using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;

namespace server.api.Services.Functions
{
    public class MovimentoFunctions : IMovimentoContract
    {
        private readonly SifocaContext sifoca;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<AppUser> userManager;

        public MovimentoFunctions(SifocaContext sifoca, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.sifoca = sifoca;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        #region ENTRADAS
        public async Task<IEnumerable<Entrada>?> GetEntradas(DateTime dataInicial, DateTime dataFinal)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            try
            {
                var entradas = await sifoca.Tb_Entrada
                .Include(p => p.Movimento)
                .OrderByDescending(x => x.DataRegistro)
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                .ToListAsync();

                if (entradas == null)
                {
                    return null;
                }
                return entradas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<IEnumerable<Entrada>?> GetEntradas(DateTime? dataInicial, DateTime? dataFinal, string op)
        {

            try
            {
                if(op == null)
                {
                    var entradas = await sifoca.Tb_Entrada
                    .Include(p => p.Movimento)
                    .OrderByDescending(x => x.DataRegistro)
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
                    .Include(p => p.Movimento)
                    .OrderByDescending(x => x.DataRegistro)
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
        public async Task<IEnumerable<Entrada>?> GetOpEntradas(DateTime dataInicial, DateTime dataFinal)
        {
            var currentUser = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(currentUser);

            if (user != null )
            {
                var entradas = await sifoca.Tb_Entrada
                .OrderBy(p => p.DataRegistro)
                .Where(p => 
                    p.DataRegistro.Date >= dataInicial 
                    && 
                    p.DataRegistro.Date <= dataFinal 
                    && 
                    p.Operador.Contains(user.UserName))
                .Include(p => p.Movimento)
                .ToListAsync();
                // Restante do seu código...
                if (entradas != null)
                {
                    return entradas;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public async Task<Entrada?> GetEntradas(int id)
        {
            try
            {
                var entrada = await sifoca.Tb_Entrada
                    .Include(p => p.Movimento)
                    .FirstOrDefaultAsync( x => x.Id == id);
                if (entrada == null)
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
        public async Task CreateEntrada(EntradaDTO movimento)
        {
            try
            {
                var fundo = await sifoca.Tb_Fundo.FindAsync("caixa");
                if (fundo != null && fundo.Id != null)
                    fundo.Total += movimento.Valor;

                //buscar o usuario logado pelo contexto do http
                string username = httpContextAccessor.HttpContext.User.Identity.Name
                    ??throw new Exception("Usuário não encontrado");
            
                var user = await userManager.FindByNameAsync(username)
                    ??throw new Exception("Nenhum usuario encontrado");

                var entrada = new Entrada
                {
                    Operador = user.UserName,
                    TipoPagamento = movimento.TipoPagamento.ToUpper(),
                    Assinante = movimento.Assinante.ToUpper(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    FormaPagamento = movimento.FormaPagamento.ToUpper(),
                    DataAtualizacao = null,
                    Movimento = new Movimento
                    {
                        Categoria = "Entrada".ToUpper(),
                        Descricao = movimento.Descricao.ToUpper(),
                        DataRegistro = Generic.GetCurrentAngolaDateTime(),
                        Valor = movimento.Valor,
                        Area = user.Departamento,
                        Caixa = fundo.Total,
                        DataAtualizacao = null,
                    }
                };

                sifoca.UpdateRange(fundo);
                await sifoca.AddRangeAsync(entrada);
                await sifoca.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateEntrada(EntradaDTO movimento, int id)
        {
            try
            {
                // Obter o usuário logado a partir do contexto HTTP
                var username = httpContextAccessor.HttpContext.User.Identity.Name;
                var user = await userManager.FindByNameAsync(username);
                var entrada = await sifoca.Tb_Entrada.Include(p => p.Movimento).FirstOrDefaultAsync(x => x.Id == id);

                if (entrada != null)
                {
                    entrada.Operador = user.UserName.ToUpper();
                    entrada.Movimento.Descricao = movimento.Descricao.ToUpper();
                    entrada.Movimento.Valor = movimento.Valor;
                    entrada.FormaPagamento = movimento.FormaPagamento.ToUpper();
                    entrada.DataAtualizacao = Generic.GetCurrentAngolaDateTime();
                    entrada.Movimento.Area = user.Departamento.ToUpper();
                    sifoca.UpdateRange(entrada);
                    await sifoca.SaveChangesAsync();
                }
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
                var entrada = await sifoca.Tb_Entrada.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IEnumerable<Entrada>?> GetEntradas(DateTime dataInicial, DateTime dataFinal, string area)
        {
            try
            {
                var entradas = await sifoca.Tb_Entrada
                    .OrderBy(p => p.DataRegistro)
                    .Include(p => p.Movimento)
                    .Where(p => p.Movimento.Area.Contains(area.ToUpper()) && p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                    .Include(p => p.Movimento)

                    .ToListAsync();

                if (entradas == null)
                {
                    return null;
                }
                return entradas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<IEnumerable<Entrada>?> GetEntradas(DateTime dataInicial, string formaPagamento, DateTime dataFinal)
        {
           try
            {
                var entradas = await sifoca.Tb_Entrada
                    .OrderBy(p => p.DataRegistro)
                    .Include(p => p.FormaPagamento.Contains(formaPagamento.ToUpper()) && p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                    .Include(p => p.Movimento)

                    .ToListAsync();

                if (entradas == null)
                {
                    return null;
                }
                return entradas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            } 
        }
        
        #endregion

        #region SAÍDAS
        public async Task CreateSaida(MovimentoDTO movimento)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);

            var fundo = await sifoca.Tb_Fundo.FindAsync("caixa");
            try
            {
                if (fundo.Id == string.Empty)
                    throw new Exception("Caixa não registrada."); if (fundo.Total >= 10000)

                    if (fundo.Total <= 10000)
                        throw new Exception("Fundo insuficiente");

                if (fundo.Total < movimento.Valor)
                    throw new Exception("Fundo insuficiente");
                fundo.Total -= movimento.Valor;
                var saida = new Saida
                {
                    Responsável = user.UserName.ToUpper(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    Beneficiario = movimento.Beneficiario.ToUpper(),
                    DataAtualizacao = null,
                    Movimento = new Movimento
                    {
                        Categoria = "Saida".ToUpper(),
                        Descricao = movimento.Descricao.ToUpper(),
                        Valor = movimento.Valor,
                        DataRegistro = Generic.GetCurrentAngolaDateTime(),
                        Area =user.Departamento.ToUpper(),
                        DataAtualizacao = null,
                        Caixa = fundo.Total
                    }
                };
                await sifoca.AddRangeAsync(saida);
                await sifoca.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateSaida(MovimentoDTO movimento, int id)
        {
            // Obter o usuário logado a partir do contexto HTTP
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            try
            {
                var saida = await sifoca.Tb_Saida.Include(p => p.Movimento).FirstOrDefaultAsync(x => x.Id == id);

                if (saida != null)
                {
                    saida.Responsável = user.UserName.ToUpper();
                    saida.Beneficiario = movimento.Beneficiario.ToUpper();
                    saida.Movimento.Descricao = movimento.Descricao.ToUpper();
                    saida.Movimento.Valor = movimento.Valor;
                    saida.DataAtualizacao = Generic.GetCurrentAngolaDateTime();

                    sifoca.UpdateRange(saida);
                    await sifoca.SaveChangesAsync();
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
                var saida = await sifoca.Tb_Saida.Include(p => p.Movimento).FirstOrDefaultAsync(x => x.Id == id);
                if (saida != null)
                {
                    sifoca.Tb_Saida.RemoveRange(saida);
                    await sifoca.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro de servidor, {ex.Message}");
            }
        }
        public async Task<IEnumerable<Saida>?> GetSaidas(DateTime? dataInicial, DateTime? dataFinal, string? op)
        {
            try
            {
                if(op == null)
                {
                    var saidas = await sifoca.Tb_Saida
                    .Include(p => p.Movimento)
                    .OrderByDescending(x => x.DataRegistro)
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
                    .Include(p => p.Movimento)
                    .OrderByDescending(x => x.DataRegistro)
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
        public async Task<Saida?> GetSaidas(int id)
        {
            try
            {
                var saida = await sifoca.Tb_Saida
                .Include(p => p.Movimento)
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

        #region GERAL
        public async Task<IEnumerable<Movimento>?> GetMovimentos(DateTime? dataInicial, DateTime? dataFinal)
        {
            try
            {
                var movimentos = await sifoca.Tb_Movimento
                .Where(p => p.DataRegistro.Date >= dataInicial && p.DataRegistro.Date <= dataFinal)
                .OrderByDescending(x => x.DataRegistro)
                .ToListAsync();
                if (movimentos == null)
                {
                    return null;
                }
                return movimentos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}