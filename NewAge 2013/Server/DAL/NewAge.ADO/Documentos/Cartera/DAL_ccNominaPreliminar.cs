using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.ADO
{
    public class DAL_ccNominaPreliminar : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccNominaPreliminar(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }
        
        /// <summary>
        /// Elimina la info de ccNominaPreliminar 
        /// </summary>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public void DAL_ccNominaPreliminar_Delete()
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "delete from ccNominaPreliminar";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina la info de ccNominaPreliminar para un centro de pago
        /// </summary>
        /// <param name="PagaduriaID">Identificador del centro de pago</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public void DAL_ccNominaPreliminar_DeleteByPagaduria(string PagaduriaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters["@PagaduriaID"].Value = PagaduriaID;

                mySqlCommand.CommandText = "delete from ccNominaPreliminar where PagaduriaID=@PagaduriaID";

                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae la info de una couta
        /// </summary>
        /// <param name="PagaduriaID">Identificador del centro de pago</param>
        /// <param name="numDocCredito">Identificador del crédito</param>
        /// <returns>retorna una cuota de un plan de pagos</returns>
        public bool DAL_ccNominaPreliminar_HasIncorporacionPrevia(string PagaduriaID, int numDocCredito)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@PagaduriaID", SqlDbType.Char, UDT_PagaduriaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@EstadoCruce", SqlDbType.Int);

                mySqlCommand.Parameters["@PagaduriaID"].Value = PagaduriaID;
                mySqlCommand.Parameters["@NumDocCredito"].Value = numDocCredito;
                mySqlCommand.Parameters["@EstadoCruce"].Value = 11;

                mySqlCommand.CommandText = "select COUNT(*) from ccNominaPreliminar with(nolock) " +
                    "where PagaduriaID=@PagaduriaID AND NumDocNomina = @NumDocNomina AND NumDocCredito = @NumDocCredito and EstadoCruce = @EstadoCruce ";

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccNominaPreliminar_HasIncorporacionPrevia");
                throw exception;
            }
        }

    }
}
