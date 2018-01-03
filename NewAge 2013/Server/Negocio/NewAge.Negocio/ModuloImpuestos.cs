using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.Librerias.ExceptionHandler;
using SentenceTransformer;

namespace NewAge.Negocio
{
    public class ModuloImpuestos : ModuloBase
    {
        #region Variables

        #region Dals
        
        #endregion

        #region Modulos

        private ModuloAplicacion _moduloAplicacion = null;
        private ModuloGlobal _moduloGlobal = null;

        #endregion

        #endregion    
    
        /// <summary>
        /// Constructor Modulo Activos Fijos
        /// </summary>
        /// <param name="conn"></param>
        public ModuloImpuestos(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

    }
}
