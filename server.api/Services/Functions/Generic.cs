using System.Data;
using FastReport.Web;
using server.api.Models;

namespace server.api.Services.Functions
{
    public static class Generic
    {
        public static DateTime GetCurrentAngolaDateTime()
        {
            // Defina o fuso horário para Angola (WAT - Hora Padrão da África Ocidental)
            TimeZoneInfo angolaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");

            // Obtenha a data e hora atual em UTC
            DateTime utcNow = DateTime.UtcNow;

            // Converta a data e hora atual de UTC para o fuso horário de Angola
            DateTime angolaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, angolaTimeZone);

            return angolaTime;
        }
        public async static void GenerateEntradasDataTableReports(Task<IEnumerable<Entrada>>? entradas, WebReport report)
        {
            var entradasTable = new DataTable();

            entradasTable.Columns.Add("DESCRIÇÃO", typeof(string));
            entradasTable.Columns.Add("MONTANTE", typeof(string));
            entradasTable.Columns.Add("ORIGEM", typeof(string));
            entradasTable.Columns.Add("FORM. PAGTO", typeof(string));
            entradasTable.Columns.Add("OPERADOR", typeof(string));
            entradasTable.Columns.Add("DATA", typeof(string));

            foreach (var entrada in await entradas)
            {
                entradasTable.Rows.Add(
                    entrada.DescricaoEntrada, 
                    entrada.ValorEntrada, 
                    entrada.FonteEntrada, 
                    entrada.FormaPagamento, 
                    entrada.Operador, 
                    entrada.DataRegistro
                );
                report.Report.RegisterData(entradasTable, "Entradas");
            }
        }
    }
}