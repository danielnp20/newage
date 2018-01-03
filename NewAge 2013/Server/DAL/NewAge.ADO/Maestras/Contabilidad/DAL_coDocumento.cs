using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    public class DAL_coDocumento : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coDocumento(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        #region Funciones Publicas

        /// <summary>
        /// Dice si un prefijo ya esta asignado en la tabla coDocumento
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <returns>Retorna Verdadero si el prefijo ya existe, de lo contrario retorna falso</returns>
        public bool DAL_coDocumento_PrefijoExists(string prefijoID)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@prefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@PrefijoID"].Value = prefijoID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coDocumento, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);

                mySqlCommandSel.CommandText =
                    "select coDocumentoID" +
                    "from coDocumento with(nolock) " +
                    "where " +
                    "   PrefijoID = @PrefijoID AND ActivoInd = 1 " +
                    "   and eg_glPrefijo = @eg_glPrefijo " +
                    "   and EmpresaGrupoID = @EmpresaGrupoID ";

                object obj = mySqlCommandSel.ExecuteScalar();
                return obj != null ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coDocumento_PrefijoExists");
                throw exception;
            }
        }

        #endregion
    }
}
