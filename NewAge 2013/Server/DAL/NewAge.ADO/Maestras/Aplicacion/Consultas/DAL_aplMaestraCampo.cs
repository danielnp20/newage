using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;

namespace NewAge.ADO
{
    public class DAL_aplMaestraCampo : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplMaestraCampo(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

       
        /// <summary>
        /// Trae la informacion el registro de maestra campo asociado al documento y columna
        /// </summary>
        /// <param name="documentID">identificador documento</param>
        /// <param name="columnName">nombre de la columna</param>
        /// <returns></returns>
        public DTO_aplMaestraCampo DAL_aplMaestraCampo_GetColumn(int documentID, string columnName)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = " select * from aplMaestraCampo   " +
                                              " where DocumentoID = @DocumentoID and NombreColumna = @NombreColumna";

                mySqlCommandSel.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NombreColumna", SqlDbType.Char);

                mySqlCommandSel.Parameters["@DocumentoID"].Value = documentID;
                mySqlCommandSel.Parameters["@NombreColumna"].Value = columnName;

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                DTO_aplMaestraCampo dto = null;

                if (dr.Read())
                {
                    dto = new DTO_aplMaestraCampo(dr);
                }

                dr.Close();
                return dto;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplMaestraCampo_GetColumn");
                throw exception;
            }
        }

        #endregion
    }
}
