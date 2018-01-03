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
    public class DAL_ccRecompraDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccRecompraDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de DTO_ccNominaDeta
        /// </summary>
        /// <returns>retorna una lista de DTO_ccNominaDeta</returns>
        public List<DTO_ccNominaDeta> DAL_ccRecompraDocu_GetByID(int NumeroDoc)
        {
            try
            {
                List<DTO_ccNominaDeta> result = new List<DTO_ccNominaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT * FROM ccRecompraDocu with(nolock)  " +
                                           "WHERE NumeroDoc = @NumeroDoc";
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccNominaDeta r = new DTO_ccNominaDeta(dr);
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDocu_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccRecompraDocu_Add(DTO_ccRecompraDocu recompraDocu)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query Coorporativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccRecompraDocu   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[DocCXP] " +
                                               "    ,[CompradorCarteraID] " +
                                               "    ,[NumeroDocCXP]  " +
                                               "    ,[TipoRecompra] " +
                                               "    ,[DocRecompra] " +
                                               "    ,[Observacion] " +
                                               "    ,[FactorRecompra] " +
                                               "    ,[NoComercialInd] " +
                                               "    ,[eg_ccCompradorCartera]) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@DocCXP " +
                                               "  ,@CompradorCarteraID " +
                                               "  ,@NumeroDocCXP " +
                                               "  ,@TipoRecompra " +
                                               "  ,@DocRecompra " +
                                               "  ,@Observacion " +
                                               "  ,@FactorRecompra " +
                                               "  ,@NoComercialInd " +
                                               "  ,@eg_ccCompradorCartera) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoRecompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocRecompra", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FactorRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NoComercialInd", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@eg_ccCompradorCartera", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = recompraDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocCXP"].Value = recompraDocu.DocCXP.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = recompraDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["NumeroDocCXP"].Value = recompraDocu.NumeroDocCXP.Value;
                mySqlCommandSel.Parameters["@TipoRecompra"].Value = recompraDocu.TipoRecompra.Value;
                mySqlCommandSel.Parameters["@DocRecompra"].Value = recompraDocu.DocRecompra.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = recompraDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FactorRecompra"].Value = recompraDocu.FactorRecompra.Value;
                mySqlCommandSel.Parameters["@NoComercialInd"].Value = recompraDocu.NoComercialInd.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDocu_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza el campo Observacion de la tabla ccSolicitudDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccRecompraDocu_Update(DTO_ccRecompraDocu recompraDocu)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@DocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDocCXP", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoRecompra", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@DocRecompra", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FactorRecompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NoComercialInd", SqlDbType.Int);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = recompraDocu.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@DocCXP"].Value = recompraDocu.DocCXP.Value;
                mySqlCommandSel.Parameters["@CompradorCarteraID"].Value = recompraDocu.CompradorCarteraID.Value;
                mySqlCommandSel.Parameters["@NumeroDocCXP"].Value = recompraDocu.NumeroDocCXP.Value; ;
                mySqlCommandSel.Parameters["@TipoRecompra"].Value = recompraDocu.TipoRecompra.Value;
                mySqlCommandSel.Parameters["@DocRecompra"].Value = recompraDocu.DocRecompra.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = recompraDocu.Observacion.Value;
                mySqlCommandSel.Parameters["@FactorRecompra"].Value = recompraDocu.FactorRecompra.Value;
                mySqlCommandSel.Parameters["@NoComercialInd"].Value = recompraDocu.NoComercialInd.Value;
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccRecompraDocu SET" +
                    "  ,DocCXP = @DocCXP  " +
                    "  ,CompradorCarteraID = @CompradorCarteraID  " +
                    "  ,NumeroDocCXP = @NumeroDocCXP  " +
                    "  ,TipoRecompra = @TipoRecompra  " +
                    "  ,DocRecompra = @DocRecompra  " +
                    "  ,Observacion = @Observacion  " +
                    "  ,FactorRecompra = @FactorRecompra  " +
                    "  ,NoComercialInd = @NoComercialInd  " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecompraDocu_Update");
                throw exception;
            }
        }

        #endregion
    }

}
