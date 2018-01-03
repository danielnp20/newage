using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;

namespace NewAge.ReportesComunes
{
    /// <summary>
    /// Interface que provee de informacion a un reporte comun
    /// </summary>
    public interface CommonReportDataSupplier
    {
        /// <summary>
        /// Trae un recurso
        /// </summary>
        /// <param name="rsxType">tipo</param>
        /// <param name="key">llave</param>
        /// <returns>valor</returns>
        string GetResource(LanguageTypes rsxType, string key);

        /// <summary>
        /// Nombre de la empresa del reporte
        /// </summary>
        /// <returns></returns>
        string GetNombreEmpresa();

        /// <summary>
        /// Logo de la empresa del reporte
        /// </summary>
        /// <returns></returns>
        byte[] GetLogoEmpresa();

        /// <summary>
        /// Nombre de usuario para mostrar
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// Nit de la empresa del reporte
        /// </summary>
        /// <returns></returns>
        string GetNitEmpresa();
    }
}
