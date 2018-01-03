using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Class Utility:
    /// Exposes utility operations
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// Get random string of 11 characters.
        /// </summary>
        /// <returns>Random string.</returns>
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path;
        }

        /// <summary>
        /// Replaces the size for the given match
        /// </summary>
        /// <param name="m">Match</param>
        /// <returns>Returns the replaced match</returns>
        private static string ReplaceSize(Match m)
        {
            string newsize = "";

            if (m.Groups["attrib"].Value.ToLower() == "width")
            {
                newsize = "100%";
            }
            else
            {
                newsize = "100%";
            }

            return m.Groups["front"].Value + newsize + m.Groups["back"].Value;
        }

        /// <summary>
        /// Replaces the size for the given match
        /// </summary>
        /// <param name="m">Match</param>
        /// <returns>Returns the replaced match</returns>
        private static string ReplaceSizeInverted(Match m)
        {
            string newsize = "";

            if (m.Groups["attrib"].Value.ToLower() == "width")
            {
                newsize = "100%";
            }
            else
            {
                newsize = "100%";
            }

            return m.Groups["front"].Value + newsize + m.Groups["middle"].Value + m.Groups["attrib"] + m.Groups["back"].Value;
        }

        /// <summary>
        /// Takes a text an generates a new version to be used in a query string
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>Returns a new text ready to be used in a query string</returns>
        public static string GenerateQueryString(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string newText = text.Trim();
                newText = newText.Replace("-", "");
                newText = newText.Replace(" ", "-");
                newText = newText.Replace("á", "a");
                newText = newText.Replace("é", "e");
                newText = newText.Replace("í", "i");
                newText = newText.Replace("ó", "o");
                newText = newText.Replace("ú", "u");
                newText = newText.Replace("ñ", "n");
                newText = newText.Replace("Á", "A");
                newText = newText.Replace("É", "E");
                newText = newText.Replace("I", "I");
                newText = newText.Replace("Ó", "O");
                newText = newText.Replace("Ó", "U");
                newText = newText.Replace("Ñ", "N");
                newText = newText.Replace("à", "a");
                newText = newText.Replace("è", "e");
                newText = newText.Replace("ì", "i");
                newText = newText.Replace("ò", "o");
                newText = newText.Replace("ù", "u");
                newText = newText.Replace("À", "A");
                newText = newText.Replace("È", "E");
                newText = newText.Replace("Ì", "I");
                newText = newText.Replace("Ò", "O");
                newText = newText.Replace("Ù", "U");
                newText = newText.Replace(",", "");
                newText = newText.Replace(".", "");
                newText = newText.Replace(";", "");
                newText = newText.Replace(":", "");
                newText = newText.Replace("'", "");
                newText = newText.Replace("\"", "");
                newText = newText.Replace("!", "");
                newText = newText.Replace("¡", "");
                newText = newText.Replace("@", "");
                newText = newText.Replace("#", "");
                newText = newText.Replace("$", "");
                newText = newText.Replace("%", "");
                newText = newText.Replace("^", "");
                newText = newText.Replace("&", "");
                newText = newText.Replace("*", "");
                newText = newText.Replace("(", "");
                newText = newText.Replace(")", "");
                newText = newText.Replace("=", "");
                newText = newText.Replace("+", "");
                newText = newText.Replace("/", "");
                newText = newText.Replace("<", "");
                newText = newText.Replace(">", "");
                newText = newText.Replace("?", "");
                newText = newText.Replace("¿", "");
                newText = newText.Replace("~", "");
                newText = newText.Replace("`", "");

                return newText;
            }

            return String.Empty;
        }

        /// <summary>
        /// Converts a string to byte []
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(String cadena)
        {
            System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
            return codificador.GetBytes(cadena);
        }

        /// <summary>
        /// Parte una cadena en los tamaños determinados por los enteros
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lon1"></param>
        /// <param name="lon2"></param>
        /// <param name="lon3"></param>
        /// <param name="lon4"></param>
        /// <param name="lon5"></param>
        /// <returns></returns>
        public static List<string> CustomSplit(string value, int lon1, int lon2, int lon3, int lon4, int lon5)
        {
            List<string> res = new List<string>();
            string str = "" + value;
            if (str.Length >= lon1 && lon1 > 0)
            {
                res.Add(str.Substring(0, lon1));
                str = str.Substring(lon1);
                if (str.Length >= lon2 && lon2 > 0)
                {
                    res.Add(str.Substring(0, lon2));
                    str = str.Substring(lon2);
                    if (str.Length >= lon3 && lon3 > 0)
                    {
                        res.Add(str.Substring(0, lon3));
                        str = str.Substring(lon3);
                        if (str.Length >= lon4 && lon4 > 0)
                        {
                            res.Add(str.Substring(0, lon4));
                            str = str.Substring(lon4);
                            if (str.Length >= lon5 && lon5 > 0)
                                res.Add(str.Substring(0, lon5));
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="lengths"></param>
        /// <returns></returns>
        public static List<string> CustomSplit(string value, int[] lengths)
        {
            List<string> res = new List<string>();
            string str = "" + value;
            foreach (int lon in lengths)
            {
                if (str.Length >= lon && lon > 0)
                {
                    res.Add(str.Substring(0, lon));
                    str = str.Substring(lon);
                }
                else
                    break;
            }
            return res;
        }

        /// <summary>
        /// Retorna el valor de una propiedad en forma de string para la grilla de edicion
        /// </summary>
        /// <param name="dto">dto</param>
        /// <param name="property">nombre del campo</param>
        /// <returns>el valor pasado a string o nullo si la propiedad no existe</returns>
        public static string GetPropertyValueToString(object dto, string property)
        {
            PropertyInfo pi = dto.GetType().GetProperty(property);
            if (pi != null)
            {
                object o = pi.GetValue(dto, null);
                return o.ToString();
            }
            else
            {
                FieldInfo fi = dto.GetType().GetField(property);
                if (fi != null)
                {
                    object o = fi.GetValue(dto);
                    return o.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// Retorna el valor de una propiedad
        /// </summary>
        /// <param name="dto">dto</param>
        /// <param name="property">nombre del campo</param>
        /// <returns>el valor</returns>
        public static object GetPropertyValue(object dto, string property)
        {
            PropertyInfo pi = dto.GetType().GetProperty(property);
            if (pi != null)
            {
                object o = pi.GetValue(dto, null);
                return o.GetType().GetProperty("Value").GetValue(o, null);
            }
            else
            {
                FieldInfo fi = dto.GetType().GetField(property);
                if (fi != null)
                {
                    object o = fi.GetValue(dto);
                    return o.GetType().GetProperty("Value").GetValue(o, null);
                }
                return null;
            }
        }

        /// <summary>
        /// Remove digits from string.
        /// </summary>
        public static string RemoveDigits(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        /// <summary>
        /// Convert an object to byte array
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Returns the byte array</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            if(obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            byte[] arr = ms.ToArray();
            return arr;
        }

        /// <summary>
        /// Convert an a byte array from an object
        /// </summary>
        /// <param name="arrBytes">Array</param>
        /// <returns>Returns the object</returns>
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);
                return obj;
            }
            catch(Exception)
            { 
                return null; 
            }
        }
    }
}
