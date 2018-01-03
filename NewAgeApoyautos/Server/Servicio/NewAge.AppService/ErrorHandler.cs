using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using NewAge.Librerias.ExceptionHandler;
using System.Configuration;

namespace NewAge.Server.AppService
{
    /// <summary>
    /// Maneja los errores que se presentan sobre el servicio
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Provee una falta como error
        /// </summary>
        /// <param name="error">Error</param>
        /// <param name="version">Version</param>
        /// <param name="fault">Parametro de salida con la falta correspondiente</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var faultDetail = new FaultException(error.ToString());
            string loggerConn = ConfigurationManager.ConnectionStrings["sqlLoggerConnectionString"].ToString(); 
            string msg = Mentor_Exception.LogException_Local(loggerConn, error, "NewAge.Service", error.Source);

            fault = Message.CreateMessage(version, FaultCode.CreateSenderFaultCode(faultDetail.Code.Name, faultDetail.Code.Namespace), msg, faultDetail, faultDetail.Action);
        }

        /// <summary>
        /// Manejo de error
        /// </summary>
        /// <param name="error">Error</param>
        /// <returns>Retorna verdadero para indicar si el error fue controlado, de lo contrario retorna falso</returns>
        public bool HandleError(Exception error)
        {
            return false;
        }
    }
}
