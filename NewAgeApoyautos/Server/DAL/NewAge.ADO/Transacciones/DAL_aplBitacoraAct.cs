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
    public class DAL_aplBitacoraAct : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_aplBitacoraAct(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Inserta una aplBitacoraAct
        /// </summary>
        /// <param name="bitacoraID">bitacoraID</param>
        /// <param name="documentoID">documentoID</param>
        /// <param name="nombreCampo">nombreCampo</param>
        /// <param name="valor">valor</param>
        /// <returns>True si se ingreso False de lo contrario</returns>
        public bool DAL_aplBitacoraAct_Add(int bitacoraID, int documentoID, string nombreCampo, string valor)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText =
                  "INSERT INTO aplBitacoraAct (" +
                  "  BitacoraID, DocumentoID, NombreCampo, Valor" +
                  ") VALUES (" +
                  "  @BitacoraID, @DocumentoID, @NombreCampo, @Valor" +
                  ")";

                mySqlCommand.Parameters.Add("@BitacoraID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@NombreCampo", SqlDbType.VarChar, 50);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.VarChar, 2024);

                mySqlCommand.Parameters["@BitacoraID"].Value = bitacoraID;
                mySqlCommand.Parameters["@DocumentoID"].Value = documentoID;
                mySqlCommand.Parameters["@NombreCampo"].Value = nombreCampo;
                mySqlCommand.Parameters["@Valor"].Value = valor;

                mySqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddBita, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_aplBitacoraAct_Add");
                throw exception;  
            }
        }
        
        #endregion
    }
}
