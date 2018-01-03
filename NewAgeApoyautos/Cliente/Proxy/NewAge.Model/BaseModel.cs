using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewAge.Cliente.Proxy.Model
{
    /// <summary>
    /// Clase padre de todos los modelos
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Ambiente actual
        /// </summary>
        private EnvironmentType _environmentType = EnvironmentType.Undetermined;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public BaseModel()
        {
        }

        /// <summary>
        /// Obtiene el ambiente actual
        /// </summary>
        public EnvironmentType EnvironmentType
        {
            get
            {
                if (this._environmentType == EnvironmentType.Undetermined)
                {
                    string s = ConfigurationManager.AppSettings["Environment.Type"];
                    this._environmentType = !String.IsNullOrEmpty(s) ? (EnvironmentType)Convert.ToByte(s) : EnvironmentType.Web;
                }
                return this._environmentType;
            }
        }

        /// <summary>
        /// Obtiene la IP del cliente
        /// </summary>
        public string UserIP
        {
            get
            {
                string clientIP = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    clientIP = HttpContext.Current.Request.UserHostAddress;
                }
                return clientIP;
            }
        }
    }
}
