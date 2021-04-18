using System;

namespace Framework.Extenders
{
    public static class DateTimeSlaExtender
    {
        public static int? CalcularSla(this DateTime? dataSlaMensagem)
        {
            if (!dataSlaMensagem.HasValue)
                return null;

            var dataSla = dataSlaMensagem.Value;
            var totalHoras = TimeSpan.Zero;
            var dataAtual = DateTime.Now;
            do
            {
                if (dataSla.DayOfWeek != DayOfWeek.Sunday &&
                    dataSla.DayOfWeek != DayOfWeek.Saturday)
                    totalHoras += TimeSpan.FromMinutes(1);
            }
            while ((dataSla = dataSla.AddMinutes(1)) <= dataAtual);

            return (int)totalHoras.TotalHours;
        }
    }
}
