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
    }
}