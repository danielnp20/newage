using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.Librerias.ExceptionHandler;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    public class DAL_acSaldos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_acSaldos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los aplIdioma
        /// </summary>
        /// <returns>Lista de aplIdioma</returns>
        public List<DTO_acContabiliza> DAL_acSaldos_GetAll(string componente, string activoClaseID)
        {
            try
            {
                List<DTO_acContabiliza> cuentas = new List<DTO_acContabiliza>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                
                mySqlCommand.Parameters.Add("@activoClaseID", SqlDbType.Char);
                mySqlCommand.Parameters["@activoClaseID"].Value = activoClaseID;

                mySqlCommand.CommandText =
                        "SELECT * from acContabiliza as acCont with(nolock)" +
                        " WHERE acCont.ActivoClaseID = @ActivoClaseID ";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var cuenta = new DTO_acContabiliza(dr);
                    cuentas.Add(cuenta);
                }
                dr.Close();

                return cuentas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_acSaldos_GetAll");
                throw exception;
            }
        }
    }
}
