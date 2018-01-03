using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_prSaldosDocu
    /// </summary>
    public class DAL_prSaldosDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prSaldosDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prSaldosDocu

        #region Funciones publicas
                
        /// <summary>
        /// Adiciona el registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prSaldosDocu_Add(DTO_prSaldosDocu saldo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prSaldosDocu " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,ConsecutivoDetaID " +
                                           "    ,CantidadDocu " +
                                           "    ,CantidadMovi ) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@ConsecutivoDetaID " +
                                           "    ,@CantidadDocu " +
                                           "    ,@CantidadMovi) ";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantidadDocu", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadMovi", SqlDbType.Decimal);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = saldo.NumeroDoc.Value;
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = saldo.ConsecutivoDetaID.Value;
                mySqlCommand.Parameters["@CantidadDocu"].Value = saldo.CantidadDocu.Value;
                mySqlCommand.Parameters["@CantidadMovi"].Value = saldo.CantidadMovi.Value;  
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSaldosDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza un registro en tabla prSaldosDocu
        /// </summary>
        /// <param name="footer">Saldos</param>
        public void DAL_prSaldosDocu_Upd(DTO_prSaldosDocu saldo)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prSaldosDocu " +
                                           "    SET CantidadMovi = @CantidadMovi " +
                                           "    WHERE ConsecutivoDetaID = @ConsecutivoDetaID";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@CantidadDocu", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@CantidadMovi", SqlDbType.Decimal);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = saldo.ConsecutivoDetaID.Value;
                mySqlCommand.Parameters["@CantidadDocu"].Value = saldo.CantidadDocu.Value;
                mySqlCommand.Parameters["@CantidadMovi"].Value = saldo.CantidadMovi.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }
                mySqlCommand.ExecuteNonQuery();
             
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSaldosDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSaldosDocu segun el ConsecutivoDetaID
        /// </summary>
        /// <param name="ConsecutivoDetaID">ID del registro del detalle</param>
        /// <returns>lista de prSaldosDocu</returns>
        public DTO_prSaldosDocu DAL_prSaldosDocu_GetByID(int ConsecutivoDetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prSaldosDocu with(nolock) where prSaldosDocu.ConsecutivoDetaID = @ConsecutivoDetaID ";

                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = ConsecutivoDetaID;

                DTO_prSaldosDocu res = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    res = new DTO_prSaldosDocu(dr);
                }
                dr.Close();
                return res;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_DTO_prSaldosDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prSaldosDocu segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de prSaldosDocu</returns>
        //public List<DTO_prSaldosDocu> DAL_prSaldosDocu_GetByNumeroDoc(int NumeroDoc)
        //{
        //    try
        //    {
        //        SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
        //        mySqlCommand.Transaction = base.MySqlConnectionTx;

        //        mySqlCommand.CommandText = "select * from prSaldosDocu with(nolock) where prSaldosDocu.NumeroDoc = @NumeroDoc ";

        //        mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
        //        mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

        //        List<DTO_prSaldosDocu> footer = new List<DTO_prSaldosDocu>();
        //        SqlDataReader dr = mySqlCommand.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            DTO_prSaldosDocu detail = new DTO_prSaldosDocu(dr);
        //            footer.Add(detail);
        //        }
        //        dr.Close();
        //        return footer;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log error
        //        var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
        //        Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_DTO_prSaldosDocu_GetByNumeroDoc");
        //        throw exception;
        //    }
        //}

        /// <summary>
        /// Elimina registros de la tabla de prSaldosDocu
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        //public void DAL_prSolicitudCargos_Delete(int numeroDoc)
        //{
        //    try
        //    {
        //        SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
        //        mySqlCommandSel.Transaction = base.MySqlConnectionTx;

        //        mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
        //        mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

        //        mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
        //        mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

        //        mySqlCommandSel.CommandText = "DELETE FROM prSaldosDocu where EmpresaID = @EmpresaID " +
        //        " and NumeroDoc = @NumeroDoc";

        //        mySqlCommandSel.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
        //        Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSaldosDocu_Delete");
        //        throw exception;
        //    }
        //}

        #endregion 

        #endregion
    }
}
