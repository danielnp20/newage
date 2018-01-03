using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewAge.Cliente.Proxy.Model
{
    public interface IPersistance
    {
        /// <summary>
        /// Obtiene la ubicación donde ocurrio un error
        /// </summary>
        /// <returns>Retorna la ubicación donde se encontro el error</returns>
        string GetErrorLocation();
    }
}
