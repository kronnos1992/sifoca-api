using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using server.api.Models;

namespace server.api.Services.Functions
{
    public static class Generic
    {
        public static string GetCurrentAngolaDateTime()
        {
            // Defina o fuso horário para Angola (WAT - Hora Padrão da África Ocidental)
            TimeZoneInfo angolaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");

            // Obtenha a data e hora atual em UTC
            DateTime utcNow = DateTime.UtcNow;

            // Converta a data e hora atual de UTC para o fuso horário de Angola
            DateTime angolaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, angolaTimeZone);

            return angolaTime.ToString("dd/MM/yyyy HH:mm");
        }

    }
}