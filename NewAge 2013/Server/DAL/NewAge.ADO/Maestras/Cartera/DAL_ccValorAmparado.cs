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
    public class DAL_ccValorAmparado : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccValorAmparado(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Consulta el valor autorizado en un rango de edades
        /// </summary>
        /// <returns>Retorna el valor autorizado</returns>
        public bool DAL_ccValorAmparado_IsValidData(int monto, int edad)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Edad", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@ActivoInd", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@Valor"].Value = monto;
                mySqlCommand.Parameters["@Edad"].Value = edad;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccLineaComponentePlazo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@ActivoInd"].Value = true;

                mySqlCommand.CommandText =
                    "select count(*) from ccValorAmparado with(nolock) " +
                    "where Valor >= @Valor and Edad = @Edad and EmpresaGrupoID = @EmpresaGrupoID and ActivoInd = @ActivoInd ";

                int obj = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return obj == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccValorAmparado_IsValidData");
                throw exception;
            }
        }
    }
}
