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
    public class DAL_ccCompradorFinalDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccCompradorFinalDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Agrega informacion a la tabla ccCompradorFinalDeta
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public void DAL_ccCompradorFinalDeta_Add(DTO_ccCompradorFinalDeta compradorFinDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                    #region Query Cooperativa
                    mySqlCommandSel.CommandText = "INSERT INTO ccCompradorFinalDeta   " +
                                                   "    ([NumeroDoc]   " +
                                                   "    ,[NumDocCredito]  " +
                                                   "    ,[Observacion]  )  " +
                                                   "VALUES    " +
                                                   "    (@NumeroDoc    " +
                                                   "    ,@NumDocCredito  " +
                                                   "    ,@Observacion ) ";
                    #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compradorFinDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = compradorFinDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = compradorFinDeta.Observacion.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_cfCompradorFinalDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza los campos de la tabla ccCompradorFinalDeta
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public bool DAL_ccCompradorFinalDeta_Update(DTO_ccCompradorFinalDeta compradorFinDeta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                    #region Query Cooperativa
                    mySqlCommandSel.CommandText =
                                               "UPDATE ccCompradorFinalDeta SET" +
                                               "    (NumDocCredito = @NumDocCredito  " +
                                               "    ,Observacion = @Observacion " +
                                               " WHERE  NumeroDoc = @NumeroDoc";
                    #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocCredito", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = compradorFinDeta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@NumDocCredito"].Value = compradorFinDeta.NumDocCredito.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = compradorFinDeta.Observacion.Value;
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
                return true;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CompradorFinalDeta_Update");
                throw exception;
            }
        }

        #endregion

        #region Otras

        public List<DTO_ccCompradorFinalDeta> DAL_ccCompradorFinalDeta_Get(string actFlujoID, string compradorCarteraID, string usuarioID)
        {
            try
            {
                List<DTO_ccCompradorFinalDeta> result = new List<DTO_ccCompradorFinalDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                #region Creacion Parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CompradorCarteraID", SqlDbType.Char, UDT_CompradorCarteraID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                #endregion
                #region Asignacion Campos
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;
                mySqlCommand.Parameters["@CerradoInd"].Value = 0;
                mySqlCommand.Parameters["@CompradorCarteraID"].Value = compradorCarteraID;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                #endregion
                #region Query Cooperativa
                    mySqlCommand.CommandText =
                            "SELECT cred.NumeroDoc as NumDocCredito, cred.CompradorFinalID, cred.Libranza, cli.ClienteID, cli.Descriptivo as Nombre, " +
                            "       SUM(planPag.VlrCapitalCesion + planPag.VlrUtilidadCesion) AS VlrLibranza "+
                            "FROM ccCreditoDocu cred WITH(NOLOCK) " +
                            "    INNER JOIN ccCreditoPlanPagos planPag WITH(NOLOCK) ON planPag.NumeroDoc = cred.NumeroDoc " +
                            "        AND planPag.CompradorCarteraID = cred.CompradorCarteraID " +
                            "    INNER JOIN ccCliente cli WITH(NOLOCK) ON cli.ClienteID = cred.ClienteID and cli.EmpresaGrupoID = cred.eg_ccCliente " +
                            "    INNER JOIN glDocumentoControl ctrl WITH(NOLOCK) ON ctrl.NumeroDoc = cred.NumeroDoc " +
                            "    INNER JOIN glActividadEstado act WITH(NOLOCK) on act.NumeroDoc = cred.NumeroDoc " +
                            "        AND act.actividadFlujoID = @ActividadFlujoID AND act.CerradoInd = @CerradoInd AND perm.areaFuncionalID = ctrl.areaFuncionalID  " +
                            "    INNER JOIN glActividadPermiso perm WITH(NOLOCK) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                            "        AND perm.actividadFlujoID = @ActividadFlujoID AND perm.UsuarioID = @UsuarioID " +
                            "WHERE cred.EmpresaID = @EmpresaID AND cred.CompradorCarteraID = @CompradorCarteraID " +
                            "GROUP BY cred.NumeroDoc, cred.CompradorFinalID, cred.Libranza, cli.ClienteID, cli.Descriptivo " +
                            "ORDER BY CAST(cred.Libranza as INT) ";
                    #endregion
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccCompradorFinalDeta dto = new DTO_ccCompradorFinalDeta();
                    dto.Aprobado.Value = false;
                    dto.CompradorFinal.Value = dr["CompradorFinalID"].ToString();
                    dto.LibranzaID.Value = Convert.ToInt32(dr["Libranza"]);
                    dto.ClienteID.Value = dr["ClienteID"].ToString();
                    dto.Nombre.Value = dr["Nombre"].ToString();
                    dto.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                    dto.VlrLibranza.Value = Convert.ToInt32(dr["VlrLibranza"]);
                    dto.Observacion.Value = string.Empty;
                    result.Add(dto);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CompradorFinalDeta_Get");
                throw exception;
            }
        }

        #endregion
    }

}
