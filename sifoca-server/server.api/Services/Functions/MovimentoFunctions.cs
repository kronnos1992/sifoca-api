using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using server.api.DTOs;
using server.api.Models;
using server.api.Services.Context;
using server.api.Services.Contracts;

namespace server.api.Services.Functions
{
    public class MovimentoFunctions : IMovimentoContract
    {
        private readonly SifocaContext sifoca;

        public MovimentoFunctions(SifocaContext sifoca)
        {
            this.sifoca = sifoca;
        }

        #region ENTRADAS
        public async Task<IEnumerable<Entrada>?> GetEntradas()
        {
            try
            {
                var entradas = await sifoca.Tb_Entrada
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
        public async Task<IEnumerable<Entrada>?> GetEntradas(string op)
        {
            try
            {
                var entradas = await sifoca.Tb_Entrada
                    .OrderBy(p => p.DataAtualizacao)
                    .Where(p => p.Operador.ToLower().Contains(op))
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
        public async Task<Entrada?> GetEntradas(int id)
        {
            try
            {
                var entrada = await sifoca.Tb_Entrada
                    .OrderBy(p => p.DataAtualizacao)
                    .FirstOrDefaultAsync(p => p.Id == id);
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
                if (fundo.Id != null)
                    fundo.Total += movimento.Valor;
                var entrada = new Entrada
                {
                    Operador = movimento.Operador.ToUpper(),
                    TipoPagamento = movimento.TipoPagamento.ToUpper(),
                    Assinante = movimento.Assinante.ToUpper(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    FormaPagamento = movimento.FormaPagamento.ToUpper(),
                    DataAtualizacao = "",
                    Movimento = new Movimento
                    {
                        Categoria = "Entrada".ToUpper(),
                        Descricao = movimento.Descricao.ToUpper(),
                        DataRegistro = Generic.GetCurrentAngolaDateTime(),
                        Valor = movimento.Valor,
                        Area = movimento.Area,
                        Caixa = fundo.Total,
                        DataAtualizacao = "",
                    }
                };

                sifoca.UpdateRange(fundo);
                await sifoca.AddRangeAsync(entrada);
                await sifoca.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateEntrada(EntradaDTO movimento, int id)
        {
            try
            {
                var entrada = await sifoca.Tb_Entrada.Include(p => p.Movimento).FirstOrDefaultAsync(x => x.Id == id);

                if (entrada != null)
                {
                    entrada.Operador = movimento.Operador.ToUpper();
                    entrada.Movimento.Descricao = movimento.Descricao.ToUpper();
                    entrada.Movimento.Valor = movimento.Valor;
                    entrada.FormaPagamento = movimento.FormaPagamento.ToUpper();
                    entrada.DataAtualizacao = DateAndTime.Now.ToString("dd/MM/yyyy - HH:mm");

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
        #endregion

        #region SAÍDAS
        public async Task CreateSaida(MovimentoDTO movimento)
        {
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
                    Responsável = movimento.Operador.ToUpper(),
                    DataRegistro = Generic.GetCurrentAngolaDateTime(),
                    DataAtualizacao = "",
                    Movimento = new Movimento
                    {
                        Categoria = "Saida".ToUpper(),
                        Descricao = movimento.Descricao.ToUpper(),
                        Valor = movimento.Valor,
                        DataRegistro = Generic.GetCurrentAngolaDateTime(),
                        Area = movimento.Area.ToUpper(),
                        DataAtualizacao = "",
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
            try
            {
                var saida = await sifoca.Tb_Saida.Include(p => p.Movimento).FirstOrDefaultAsync(x => x.Id == id);

                if (saida != null)
                {
                    saida.Responsável = movimento.Operador.ToUpper();
                    saida.Movimento.Descricao = movimento.Descricao.ToUpper();
                    saida.Movimento.Valor = movimento.Valor;
                    saida.DataAtualizacao = DateAndTime.Now.ToString("dd/MM/yyyy - HH:mm");

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
        public async Task<IEnumerable<Saida>?> GetSaidas()
        {
            try
            {
                var saidas = await sifoca.Tb_Saida
                    .Include(p => p.Movimento)
                    .ToListAsync();

                if (saidas == null)
                {
                    return null;
                }
                return saidas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro na busca dos dados {ex.Message}");
            }
        }
        public async Task<IEnumerable<Saida>> GetSaidas(string op)
        {
            try
            {
                var saidas = await sifoca.Tb_Saida
                .Where(p => p.Responsável.ToLower().Contains(op))
                .Include(p => p.Movimento)
                .ToListAsync();
                if (saidas == null)
                {
                    return null;
                }
                return saidas;
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
        public async Task<IEnumerable<Movimento>> GetMovimentos()
        {
            try
            {
                var movimentos = await sifoca.Tb_Movimento.ToListAsync();
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