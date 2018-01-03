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
    /// <summary>
    /// DAL de DAL_Moneda
    /// </summary>
    public class DAL_glTasaDeCambio : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_glTasaDeCambio(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene el la tasa de cambio
        /// </summary>
        /// <param name="monedaID">Identificador de la moneda</param>
        /// <param name="fecha">Fecha</param>
        /// <returns>Retorna la tasa de cambio</returns>
        public decimal DAL_TasaDeCambio_Get(string monedaID, DateTime fecha)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "select TasaCambio from glTasaCambio with(nolock) where EmpresaGrupoID = @EmpresaGrupoID and MonedaID = @MonedaID and Fecha = @Fecha";

                mySqlCommandSel.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Fecha", SqlDbType.DateTime);

                mySqlCommandSel.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glTasaCambio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@MonedaID"].Value = monedaID;
                mySqlCommandSel.Parameters["@Fecha"].Value = fecha;
                
                var valor = mySqlCommandSel.ExecuteScalar();
                if (valor != null)
                    return Convert.ToDecimal(valor);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_glTasaDeCambio_Get");
                throw exception;
            }
        }

    }
}
