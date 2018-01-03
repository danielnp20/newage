using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using System.Data;
using NewAge.DTO.UDT;
using System.ComponentModel;
using System.Reflection;

namespace NewAge.ADO
{
    public class DAL_ccCompradorFinalDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>DAL_ccCompradorFinalDocu
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCompradorFinalDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de ccCompradorFinalDocu
        /// </summary>
        /// <returns>retorna una lista de DTO_ccCompradorFinalDocu</returns>
        public List<DTO_ccCompradorFinalDocu> DAL_ccCompradorFinalDocu_GetByID(int NumeroDoc)
        {
            try
            {
                List<DTO_ccCompradorFinalDocu> result = new List<DTO_ccCompradorFinalDocu>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccCompradorFinalDocu with(nolock)  " +
                                                           "WHERE NumeroDoc = NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCompradorFinalDocu r = new DTO_ccCompradorFinalDocu(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CompradorFinalDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCompradorFinalDocu
        /// </summary>
        /// <returns></returns>
        public void DAL_ccCompradorFinalDocu_Add(DTO_ccCompradorFinalDocu compradorFinalDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccCompradorFinalDocu   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[CompradorCarteraID] " +
                                               "    ,[CompradorFinal] " +
                                               "    ,[eg_ccCompradorCartera] ) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@CompradorCarteraID " +
                                               "  ,@CompradorFinal " +
                                               "  ,@eg_ccCompradorCartera) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorFinal", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compradorFinalDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compradorFinalDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@CompradorFinal"].Value = compradorFinalDocu.CompradorFinal.Value;
                mySqlCommandSel.Parameters["@eg_ccCompradorCartera"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCompradorCartera, this.Empresa, egCtrl);
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CompradorFinalDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccCompradorFinalDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccCompradorFinalDocu_Update(DTO_ccCompradorFinalDocu compradorFinalDocu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CompradorFinal", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compradorFinalDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = compradorFinalDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@CompradorFinal"].Value = compradorFinalDocu.CompradorFinal.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccCompradorFinalDocu SET" +
                    "  ,CompradorCarteraID = @CompradorCarteraID " +
                    "  ,CompradorFinal = @CompradorFinal " +
                    " WHERE  NumeroDoc = @NumeroDoc ";
                #endregion
                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_VentaDocu_Update");
                throw exception;
            }
        }

        #endregion
    }

}
