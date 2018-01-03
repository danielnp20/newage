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
    public class DAL_ccValorAutorizado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccValorAutorizado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Consulta el valor autorizado en un rango de valores
        /// </summary>
        /// <returns>Retorna el valor autorizado</returns>
        public bool DAL_ccValorAutorizado_IsValidData(int monto, int plazo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Plazo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@Valor"].Value = monto;
                mySqlCommand.Parameters["@Plazo"].Value = plazo;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaComponentePlazo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                mySqlCommand.CommandText =
                    "select count(*) from ccValorAutorizado with(nolock) " +
                    "where Valor >= @Valor and Plazo = @Plazo and EmpresaGrupoID = @EmpresaGrupoID and ActivoInd = @ActivoInd ";

                int obj = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return obj == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccLineaComponentePlazo_GetByMonto");
                throw exception;
            }
        }
    }
}
