using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NewAge.Librerias.Project
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with an elipses (...)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length)
        {
            return Truncate(input, length, "...");
        }

        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with the given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <param name="suffix">The value to suffix the return value with (if truncation is performed)</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length, string suffix)
        {
            if (input == null) return "";
            if (input.Length <= length) return input;

            if (suffix == null) suffix = "...";

            return input.Substring(0, length - suffix.Length) + suffix;
        }

        /// <summary>
        /// Splits a given string into an array based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String array, each containing one line</returns>
        public static string[] ToLineArray(this string input)
        {
            if (input == null) return new string[] { };
            return System.Text.RegularExpressions.Regex.Split(input, "\r\n");
        }

        /// <summary>
        /// Splits a given string into a strongly-typed list based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Strongly-typed string list, each containing one line</returns>
        public static List<string> ToLineList(this string input)
        {
            List<string> output = new List<string>();
            output.AddRange(input.ToLineArray());
            return output;
        }

        /// <summary>
        /// Replaces line breaks with self-closing HTML 'br' tags
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceBreaksWithBR(this string input)
        {
            return string.Join("<br/>", input.ToLineArray());
        }

        /// <summary>
        /// Replaces any single apostrophes with two of the same
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string DoubleApostrophes(this string input)
        {
            return Regex.Replace(input, "'", "''");
        }

        /// <summary>
        /// Encodes the input string as HTML (converts special characters to entities)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>HTML-encoded string</returns>
        public static string ToHTMLEncoded(this string input)
        {
            return HttpContext.Current.Server.HtmlEncode(input);
        }

        /// <summary>
        /// Encodes the input string as a URL (converts special characters to % codes)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>URL-encoded string</returns>
        public static string ToURLEncoded(this string input)
        {
            return HttpContext.Current.Server.UrlEncode(input);
        }

        /// <summary>
        /// Decodes any HTML entities in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string FromHTMLEncoded(this string input)
        {
            return HttpContext.Current.Server.HtmlDecode(input);
        }

        /// <summary>
        /// Decodes any URL codes (% codes) in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string FromURLEncoded(this string input)
        {
            return HttpContext.Current.Server.UrlDecode(input);
        }

        /// <summary>
        /// Removes any HTML tags from the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, @"<(style|script)[^<>]*>.*?</\1>|</?[a-z][a-z0-9]*[^<>]*>|<!--.*?-->", "");
        }

        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }

        /// <summary>
        /// Personaliza un texto de acuerdo a su tipo y longitud
        /// </summary>
        /// <param name="typeField">Tipo de campo</param>
        /// <param name="value">valor</param>
        /// <param name="lenght">longitud requerida</param>
        /// <returns>string personalizado</returns>
        public static string CustomizeString(Type typeField, object value, int lenght)
        {
            string result = string.Empty;
            string espacioFaltante = string.Empty;
            int longEspacioFaltante = lenght - value.ToString().Length;
          
            //Valida el tipo de dato
            if (typeField == typeof(string))
            {
                //Asigna los espacios que faltan llenar 
                for (int i = 0; i < longEspacioFaltante; i++)
			        espacioFaltante += " ";

                 result = value.ToString() + espacioFaltante;
            }     
            else if(typeField == typeof(int))
            {              
                value = value != null ? Convert.ToInt32(value) : 0;
                longEspacioFaltante = lenght - value.ToString().Length;

                //Asigna los espacios que faltan llenar 
                for (int i = 0; i < longEspacioFaltante; i++)
                    espacioFaltante += "0";
                
                result = espacioFaltante + value.ToString();
            }
            else if (typeField == typeof(decimal))
            {
                //Asigna los espacios que faltan llenar 
                for (int i = 0; i < longEspacioFaltante; i++)
                    espacioFaltante += "0";

                 result = (value.ToString() + espacioFaltante).Replace(',', '.');
            }
            else if (typeField == typeof(bool))
            {
                longEspacioFaltante = 0;
                value = value != null ? value : false;
                bool v = Convert.ToBoolean(value);

                result = v? "X" : " ";
            }
            else if (typeField == typeof(DateTime))
            {

            }

            //Recorta el string si  la longitud del resultado es mayor a la fijada 
            if (longEspacioFaltante < 0)
                result = result.ToString().Substring(0, lenght);

            return result;
        }
    }
}
