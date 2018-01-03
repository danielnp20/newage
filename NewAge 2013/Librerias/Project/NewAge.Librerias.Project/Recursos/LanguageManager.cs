using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewAge.Librerias.Project
{
    /// <summary>
    /// Clase que se encarga del manejo de los idiomas del sitio
    /// </summary>
    public static class LanguageManager
    {
        /// <summary>
        /// valor que devuelve una consulta por defecto
        /// </summary>
        private static string _value;

        /// <summary>
        /// Trae una traducción de un diccionario
        /// </summary>
        /// <param name="dictionary">Diccionario</param>
        /// <param name="key">Llave para buscar la traducción</param>
        /// <returns>Retirna la traduccion de una llaves</returns>
        public static string GetResource(Dictionary<string, string> dictionary, string key)
        {
            try
            {
                _value = string.Empty;

                dictionary.TryGetValue(key, out _value);
                return string.IsNullOrEmpty(_value) ? key : _value;
            }
            catch (Exception)
            {
                return key;
            }
        }

        /// <summary>
        /// Trae una traducción de un diccionario segun el modulo
        /// </summary>
        /// <param name="dictionary">Diccionario</param>
        /// <param name="type">Language type</param>
        /// <param name="key">Llave para buscar la traducción</param>
        /// <returns>Retorna la traduccion de una llaves</returns>
        public static string GetResource(Dictionary<string, Dictionary<string, string>> dictionary, LanguageTypes type, string key)
        {
            try
            {
                Dictionary<string, string> tempDictionary = dictionary[type.ToString()];
                return GetResource(tempDictionary, key);
            }
            catch (Exception ex)
            {
                return key;
            }
        }

    }
}
