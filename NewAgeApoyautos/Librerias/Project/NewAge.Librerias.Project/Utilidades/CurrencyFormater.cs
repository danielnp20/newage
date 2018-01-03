using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase que convierte un valor a letras
    /// </summary>
    public static class CurrencyFormater
    {

        /// <summary>
        /// Obtiene el valor en letras de un numero
        /// </summary>
        /// <param name="lang">Idioma</param>
        /// <param name="mda">Moneda de la transaccion</param>
        /// <param name="value">Valor a decifrar</param>
        /// <returns></returns>
        public static string GetCurrencyString(string lang, string mda, decimal value)
        {
            try
            {
                string result = string.Empty;
                string resEntero = string.Empty;
                string resDecimal = string.Empty;
                string shortLang = lang.Substring(0, 2).ToLower();
                string shortMda = mda.Substring(0, 3).ToLower();

                decimal val = (Decimal)(value - (Int64)value);
                decimal pow = (Decimal)(Math.Pow(10, 2));
                Int32 enteroDecimal = (Int32)Math.Round(val * pow, 0);

                switch (shortLang)
                {
                    case "es":
                        resEntero = GetStringVal_ESP(value);
                        result = resEntero + " " + GetCurrencyExt_ESP(shortMda);
                        if (enteroDecimal != 0)
                        {
                            resDecimal = GetStringVal_ESP(enteroDecimal);
                            result += " CON " + resDecimal + " CENTAVOS";
                        }

                        break;
                    default:
                        resEntero = GetStringVal_ESP(value);
                        break;
                }

                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #region Español

        /// <summary>
        /// Obtiene las letras en español
        /// </summary>
        /// <param name="lang">Idioma</param>
        /// <param name="mda">Moneda seleccionada</param>
        /// <param name="value">Valor a calcular</param>
        /// <param name="isDecimal">Indica si esta sacando la parte decimal</param>
        /// <returns>Retorna el valor en letras (español)</returns>
        public static String GetStringVal_ESP(decimal value)
        {
            string result = "";
            value = Math.Truncate(value);

            if (value == 0)
                result = "CERO";
            else if (value == 1)
                result = "UNO";
            else if (value == 2)
                result = "DOS";
            else if (value == 3)
                result = "TRES";
            else if (value == 4)
                result = "CUATRO";
            else if (value == 5)
                result = "CINCO";
            else if (value == 6)
                result = "SEIS";
            else if (value == 7)
                result = "SIETE";
            else if (value == 8)
                result = "OCHO";
            else if (value == 9)
                result = "NUEVE";
            else if (value == 10)
                result = "DIEZ";
            else if (value == 11)
                result = "ONCE";
            else if (value == 12)
                result = "DOCE";
            else if (value == 13)
                result = "TRECE";
            else if (value == 14)
                result = "CATORCE";
            else if (value == 15)
                result = "QUINCE";
            else if (value < 20)
                result = "DIECI" + GetStringVal_ESP(value - 10);
            else if (value == 20)
                result = "VEINTE";
            else if (value < 30)
                result = "VEINTI" + GetStringVal_ESP(value - 20);
            else if (value == 30)
                result = "TREINTA";
            else if (value == 40)
                result = "CUARENTA";
            else if (value == 50)
                result = "CINCUENTA";
            else if (value == 60)
                result = "SESENTA";
            else if (value == 70)
                result = "SETENTA";
            else if (value == 80)
                result = "OCHENTA";
            else if (value == 90)
                result = "NOVENTA";
            else if (value < 100)
                result = GetStringVal_ESP(Math.Truncate(value / 10) * 10) + " Y " + GetStringVal_ESP(value % 10);
            else if (value == 100)
                result = "CIEN";
            else if (value < 200)
                result = "CIENTO " + GetStringVal_ESP(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                result = GetStringVal_ESP(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500)
                result = "QUINIENTOS";
            else if (value == 700)
                result = "SETECIENTOS";
            else if (value == 900)
                result = "NOVECIENTOS";
            else if (value < 1000)
                result = GetStringVal_ESP(Math.Truncate(value / 100) * 100) + " " + GetStringVal_ESP(value % 100);
            else if (value == 1000)
                result = "MIL";
            else if (value < 2000)
                result = "MIL " + GetStringVal_ESP(value % 1000);
            else if (value < 1000000)
            {
                result = GetStringVal_ESP(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) result = result + " " + GetStringVal_ESP(value % 1000);
            }
            else if (value == 1000000)
                result = "UN MILLON";
            else if (value < 2000000)
                result = "UN MILLON " + GetStringVal_ESP(value % 1000000);
            else if (value < 1000000000000)
            {
                result = GetStringVal_ESP(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                    result = result + " " + GetStringVal_ESP(value - Math.Truncate(value / 1000000) * 1000000);
            }
            else if (value == 1000000000000)
                result = "UN BILLON";
            else if (value < 2000000000000)
                result = "UN BILLON " + GetStringVal_ESP(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                result = GetStringVal_ESP(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) result = result + " " + GetStringVal_ESP(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }

            return result;
        }

        /// <summary>
        /// Obtiene la extension para el tipo de mda
        /// </summary>
        /// <param name="mda">Moneda de la operacion</param>
        /// <returns>Retorna la extension segun lamoneda</returns>
        private static string GetCurrencyExt_ESP(string mda)
        {
            switch (mda)
            {
                case "cop":
                    return "PESOS MDA/CTE";
                case "usd":
                    return "DOLARES";
                case "num":
                    return " ";
                default:
                    return "SIN MONEDA";
            }
        }

        #endregion

    }
}
