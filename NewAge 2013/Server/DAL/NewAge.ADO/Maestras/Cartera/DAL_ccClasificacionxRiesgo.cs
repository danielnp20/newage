using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Diagnostics;

namespace NewAge.ADO
{
    public class DAL_ccClasificacionxRiesgo : DAL_MasterSimple
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccClasificacionxRiesgo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) :
            base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Trae la lista de tareas asociados a un documento
        /// </summary>
        /// <param name="documentoID">Id de la pagaduria</param>
        /// <returns>Retorna una lista con la maestra de lista de chequo</returns>
        public DTO_ccClasificacionxRiesgo DAL_ccClasificacionxRiesgo_GetByID(string claseCredito, int diasVencidos)
        {
            DTO_ccClasificacionxRiesgo result = null;
            try
            {
                #region Query

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@ClaseCredito", SqlDbType.Char, UDT_CodigoGrl5.MaxLength);
                mySqlCommand.Parameters.Add("@DiasVencidos", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@ClaseCredito"].Value = claseCredito;
                mySqlCommand.Parameters["@DiasVencidos"].Value = diasVencidos;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccClasificacionxRiesgo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                //Query basico
                mySqlCommand.CommandText =
                    "SELECT TOP(1) * " +
                    "FROM ccClasificacionXRiesgo WITH(NOLOCK) " +
                    "WHERE ClaseCredito = @ClaseCredito AND EmpresaGrupoID = @EmpresaGrupoID AND DiasVencidos >= @DiasVencidos AND ActivoInd = @ActivoInd " +
                    "ORDER BY DiasVencidos ";

                #endregion
                
                SqlDataReader dr;
                
                dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                    result = new DTO_ccClasificacionxRiesgo(dr, this.props, true);

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccClasificacionxRiesgo_GetByID");
                throw exception;
            }

        }               

        #endregion

    }
}
