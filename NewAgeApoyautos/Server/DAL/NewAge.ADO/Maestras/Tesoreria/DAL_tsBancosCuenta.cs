using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.ExceptionHandler;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_tsBancosCuenta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_tsBancosCuenta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Publicas

        /// <summary>
        /// Obtiene el consecutivo del siguiente cheque y actualiza la base de datos
        /// </summary>
        /// <param name="bancoCuentaID">ID de la cuenta bancaria</param>
        /// <returns>Resultado de la operación</returns>
        public void DAL_tsBancosCuenta_IncrementarChequeInicial(string bancoCuentaID, int chequeInicial)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@BancoCuentaID", SqlDbType.Char, UDT_BancoCuentaID.MaxLength);
                mySqlCommand.Parameters.Add("@EmpresaGrupoID", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@SiguienteCheque", SqlDbType.Int);

                mySqlCommand.Parameters["@BancoCuentaID"].Value = bancoCuentaID;
                mySqlCommand.Parameters["@EmpresaGrupoID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@SiguienteCheque"].Value = chequeInicial + 1;
              
                
                mySqlCommand.CommandText =
                    "UPDATE tsBancosCuenta " +
                    "SET ChequeInicial = @SiguienteCheque " +
                    "WHERE BancoCuentaID = @BancoCuentaID and EmpresaGrupoID = @EmpresaGrupoID ";

                mySqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_tsBancosCuenta_IncrementarChequeInicial");
                throw exception;
            }
        }

        #endregion
    }
}
