using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace NewAge.Server.AppService
{
    /// <summary>
    /// Modela el comportamiento del servicio
    /// </summary>
    public class ErrorServiceBehavior : IServiceBehavior
    {        
        /// <summary>
        /// Envia a un elemento con binding la información del servicio para que pueda ser soportado
        /// </summary>
        /// <param name="serviceDescription">Descripción del servicio</param>
        /// <param name="serviceHostBase">Base del servicio del host</param>
        /// <param name="endpoints">End points</param>
        /// <param name="bindingParameters">Parametros del Binding</param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Examina la descripción antes de construir el servicio para confirmar si se puede ejecutar correctamente
        /// </summary>
        /// <param name="serviceDescription">Descripción del servicio</param>
        /// <param name="serviceHostBase">Base del servicio del host</param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        /// <summary>
        /// Cambia en tiempo de ejecuación los valores de las propiedades o inserta objetos personalizados como manejo de errores, 
        /// mensajes o interceptadores de parametros, extensiones de seguridad y otros objetos personalizados
        /// </summary>
        /// <param name="serviceDescription">Descripción del servicio</param>
        /// <param name="serviceHostBase">Base del servicio del host</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            ErrorHandler handler = new ErrorHandler();
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(handler);
            }
        }
    }
}
