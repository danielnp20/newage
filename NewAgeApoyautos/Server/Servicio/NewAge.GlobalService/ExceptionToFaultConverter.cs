using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace NewAge.Server.GlobalService
{
    /// <summary>
    /// Mapea excepciones a faltas para registro
    /// </summary>
    public static class ExceptionToFaultConverter
    {
        /// <summary>
        /// Colección de mapeos
        /// </summary>
        private static Dictionary<Type, Delegate> _converters = new Dictionary<Type, Delegate>();
        
        /// <summary>
        /// Registra un mapeo de excepción
        /// </summary>
        /// <typeparam name="TException">Tipo de excepción</typeparam>
        /// <typeparam name="TFault">Tipo de falta</typeparam>
        /// <param name="converter">Convertidor (Mapeo)</param>
        public static void RegisterConverter<TException, TFault>(Func<TException, TFault> converter)
        {
            ExceptionToFaultConverter._converters.Add(typeof(TException), converter);
        }

        /// <summary>
        /// Mapea una excepción ana falta
        /// </summary>
        /// <param name="ex">Excepción</param>
        /// <returns>Retorna la excepción como falta</returns>
        public static object ConvertExceptionToFault(Exception ex)
        {
            Delegate converter;

            if (!_converters.TryGetValue(ex.GetType(), out converter))
            {
                return null;
            }

            return converter.DynamicInvoke(ex);
        }
    }
}