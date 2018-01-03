using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Web;

namespace NewAge.Server.AppService
{
    /// <summary>
    /// Extensión de comportamiento para el manejo de errores en el servicio (usado en el archivo de configuración)
    /// </summary>
    public class ErrorHandlerBehavior : BehaviorExtensionElement
    {
        /// <summary>
        /// Crea el comportamiento
        /// </summary>
        /// <returns>Retorna un nuevo comportamiento</returns>
        protected override object CreateBehavior()
        {
            return new ErrorServiceBehavior();
        }

        /// <summary>
        /// Obtiene el tipo de comportamiento
        /// </summary>
        public override Type BehaviorType
        {
            get 
            { 
                return typeof(ErrorServiceBehavior); 
            }
        }
    }
}
