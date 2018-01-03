using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    public class DateTimeExtension
    {
        /// <summary>
        /// Devuelve un valor Long que especifica el número de
        /// intervalos de tiempo entre dos valores Date.
        /// </summary>
        /// <param name="interval">Obligatorio. Valor de enumeración
        /// DateInterval o expresión String que representa el intervalo
        /// de tiempo que se desea utilizar como unidad de diferencia
        /// entre Date1 y Date2.</param>
        /// <param name="date1">Obligatorio. Date. Primer valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <param name="date2">Obligatorio. Date. Segundo valor de
        /// fecha u hora que se desea utilizar en el cálculo.</param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            long rs = 0;
            TimeSpan diff = date2.Subtract(date1);
            switch (interval)
            {
                case DateInterval.Day:
                case DateInterval.DayOfYear:
                    rs = (long)diff.TotalDays;
                    break;
                case DateInterval.Hour:
                    rs = (long)diff.TotalHours;
                    break;
                case DateInterval.Minute:
                    rs = (long)diff.TotalMinutes;
                    break;
                case DateInterval.Month:
                    rs = (date2.Month - date1.Month) + (12 * DateTimeExtension.DateDiff(DateInterval.Year, date1, date2));
                    break;
                case DateInterval.Quarter:
                    rs = (long)Math.Ceiling((double)(DateTimeExtension.DateDiff(DateInterval.Month, date1, date2) / 3.0));
                    break;
                case DateInterval.Second:
                    rs = (long)diff.TotalSeconds;
                    break;
                case DateInterval.Weekday:
                case DateInterval.WeekOfYear:
                    rs = (long)(diff.TotalDays / 7);
                    break;
                case DateInterval.Year:
                    rs = date2.Year - date1.Year;
                    break;
            }//switch
            return rs;
        }//Dat

        /// <summary>
        /// Funcion para establecer el numero de dias calendario en el año para periodos de 30 dias
        /// </summary>
        /// <param name="date">Fecha para </param>
        /// <returns></returns>
        public static int DaysInBussinesDays(DateTime date)
        {
            int mes = date.Month;
            int year = date.Year;
            int diasCal = date.Day;

            if (mes == 2 && diasCal >= 28)
                diasCal = 30;
            else if (diasCal == 31)
                diasCal = 30;

            diasCal += ((year * 12) + (mes)) * 30;

            return diasCal;

        }

        /// <summary>
        /// Cambia una fecha en string en cierto formato a otro formato
        /// </summary>
        /// <param name="fecha">Fecha en formato original</param>
        /// <param name="formatoOriginal">Formato original</param>
        /// <param name="formatoDestino">Formato destino</param>
        /// <returns></returns>
        public static string ChangeDateFormat(string fecha, string formatoOriginal, string formatoDestino)
        {
            return (DateTime.ParseExact(fecha, formatoOriginal, null)).ToString(formatoDestino);
        }


    }
}
