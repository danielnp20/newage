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
    public class DAL_aplIdioma : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplIdioma(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Trae todos los aplIdioma
        /// </summary>
        /// <returns>Lista de aplIdioma</returns>
        public IEnumerable<DTO_aplIdioma> DAL_aplIdioma_GetAll()
        {
            try
            {
                List<DTO_aplIdioma> idiomas = new List<DTO_aplIdioma>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "SELECT * FROM aplIdioma with(nolock)";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    var idioma = new DTO_aplIdioma(dr);
                    
                    idiomas.Add(idioma);
                }
                dr.Close();

                return idiomas;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplIdioma_DAL_aplIdioma_GetAll");
                throw exception;
            }
        }

        /// <summary>
        /// Verifica si un idioma existe
        /// </summary>
        /// <returns>Lista de aplIdioma</returns>
        public bool DAL_aplIdioma_Exists(string langId)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "select * from aplIdioma with(nolock) where IdiomaID = @IdiomaID";

                mySqlCommand.Parameters.Add("@IdiomaID", SqlDbType.Char, 30);
                mySqlCommand.Parameters["@IdiomaID"].Value = langId;

                object first = mySqlCommand.ExecuteScalar();
                return first == null ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplIdioma_DAL_aplIdioma_GetAll");
                throw exception;
            }
        }

    }
}
