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
    public class DAL_coComprobantePrefijo : DAL_Base
    {     
        
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_coComprobantePrefijo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Trae el identificador de un comprobante segun el documento y el prefijo
        /// </summary>
        /// <param name="documentoIDExt">coDocumentoID del documento de la PK</param>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="compAnulacion">Indica si debe traer el comprobante de anulacion</param>
        /// <returns>Retorna el identificador de un comprobante o null si no existe</returns>
        public string DAL_coComprobantePrefijo_GetComprobanteByDocPref(int coDocumentoID, string prefijoID, bool compAnulacion)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@coDocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PrefijoID", SqlDbType.Char, UDT_PrefijoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_glPrefijo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coComprobante", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                mySqlCommandSel.Parameters["@coDocumentoID"].Value = coDocumentoID;
                mySqlCommandSel.Parameters["@PrefijoID"].Value = prefijoID;
                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobantePrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_glPrefijo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glPrefijo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coComprobante"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coComprobante, this.Empresa, egCtrl);

                string compField = compAnulacion ? "ComprobanteAnulID" : "ComprobanteID";

                mySqlCommandSel.CommandText =
                    "select " + compField + " " +
                    "from coComprobantePrefijo with(nolock) " +
                    "where " +
                    "   coDocumentoID = @coDocumentoID AND PrefijoID = @PrefijoID AND ActivoInd = 1 " +
                    "   and eg_glPrefijo = @eg_glPrefijo " +
                    "   and eg_coComprobante = @eg_coComprobante ";

                object obj = mySqlCommandSel.ExecuteScalar();
                return obj != null ? obj.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_coComprobantePrefijo_GetComprobanteByDocPref");
                throw exception;
            }
        }

        #endregion
     
    }
}
