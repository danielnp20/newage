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
    public class DAL_seMaquina : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_seMaquina(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// valida que la maquina pueda ingresar al sistema
        /// </summary>
        /// <param name="macs">MACs posibles</param>
        /// <returns>Retorna verdadero si la maquina tiene permiso</returns>
        public bool DAL_seMaquina_ValidatePC(List<string> macs)
        {
            try
            {
                bool result = false;

                SqlCommand mySqlCommand = this.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT Count(*) FROM seMaquina WHERE seMaquinaID = @seMaquinaID and ActivoInd = 1 ";

                mySqlCommand.Parameters.Add("@seMaquinaID", SqlDbType.VarChar, UDT_seMaquinaID.MaxLength);

                foreach (string mac in macs)
                {
                    mySqlCommand.Parameters["@seMaquinaID"].Value = mac;

                    var total = mySqlCommand.ExecuteScalar();
                    int maqs = Convert.ToInt16(total);
                    if (maqs > 0)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_seMaquina_ValidatePC");
                throw exception;
            }
        }

    }
}
