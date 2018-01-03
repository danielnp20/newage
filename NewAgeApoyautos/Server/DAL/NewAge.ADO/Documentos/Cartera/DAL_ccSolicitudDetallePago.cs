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
    public class DAL_ccSolicitudDetallePago : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>DAL_ccSolicitudDetallePago
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ccSolicitudDetallePago(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region CRUD

        /// <summary>
        /// Trae todos los registros de ccSolicitudDetallePago
        /// </summary>
        /// <param name="NumeroDoc">Numero Doc que filtra la busqueda</param>
        /// <returns>retorna una lista de DTO_ccSolicitudDetallePago</returns>
        public List<DTO_ccSolicitudDetallePago> DAL_ccSolicitudDetallePago_GetByID(int NumeroDoc)
        {
            try
            {
                List<DTO_ccSolicitudDetallePago> result = new List<DTO_ccSolicitudDetallePago>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                mySqlCommand.CommandText = "SELECT detaPago.*, tercero.Descriptivo AS Nombre " +
                                           "FROM ccSolicitudDetallePago detaPago with(nolock) " +
                                           "    INNER JOIN coTercero tercero with(nolock) ON tercero.TerceroID = detaPago.TerceroID " +
                                           "        AND tercero.EmpresaGrupoID = detaPago.eg_coTercero " +
                                           "WHERE NumeroDoc = @NumeroDoc";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_ccSolicitudDetallePago r = new DTO_ccSolicitudDetallePago(dr);
                    r.Nombre.Value = dr["Nombre"].ToString();
                    result.Add(r);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDetallePago_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Agrega informacion a la tabla ccCompradorFinalDocu
        /// </summary>
        /// <returns></returns>
        public void DAL_ccSolicitudDetallePago_Add(DTO_ccSolicitudDetallePago SolicitudDetallePago)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                #region Query Cooperativa
                mySqlCommandSel.CommandText = "    INSERT INTO ccSolicitudDetallePago   " +
                                               "    ([NumeroDoc] " +
                                               "    ,[TerceroID] " +
                                               "    ,[Documento] " +
                                               "    ,[Valor]  " +
                                               "    ,[eg_coTercero] ) " +
                                               "  VALUES " +
                                               "  (@NumeroDoc " +
                                               "  ,@TerceroID " +
                                               "  ,@Documento " +
                                               "  ,@Valor " +
                                               "  ,@eg_coTercero) ";

                #endregion
                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = SolicitudDetallePago.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = SolicitudDetallePago.TerceroID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = SolicitudDetallePago.Documento.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = SolicitudDetallePago.Valor.Value;
                mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDetallePago_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza la tabla ccCompradorFinalDocu
        /// </summary>
        /// <param name="docSolicitud"></param>
        /// <returns></returns>
        public void DAL_ccSolicitudDetallePago_Update(DTO_ccSolicitudDetallePago SolicitudDetallePago)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Creacion de comandos
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TerceroID", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Documento", SqlDbType.Char, UDT_DocTerceroID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Valor", SqlDbType.Decimal);
                //mySqlCommandSel.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna los valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = SolicitudDetallePago.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TerceroID"].Value = SolicitudDetallePago.TerceroID.Value;
                mySqlCommandSel.Parameters["@Documento"].Value = SolicitudDetallePago.Documento.Value;
                mySqlCommandSel.Parameters["@Valor"].Value = SolicitudDetallePago.Valor.Value;
                //mySqlCommandSel.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                #endregion
                #region Query Cooperativa
                mySqlCommandSel.CommandText =
                    "UPDATE ccSolicitudDetallePago SET" +
                    "  ,TerceroID = @TerceroID " +
                    "  ,Documento = @Documento " +
                    "  ,Valor = @Valor " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ccSolicitudDetallePago_Update");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina los componentes asociados a un documento
        /// </summary>
        /// <param name="numeroDoc">Identificador del documento</param>
        public void DAL_ccSolicitudDetallePago_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.CommandText = "DELETE FROM ccSolicitudDetallePago WHERE NumeroDoc=@NumeroDoc ";
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_SolicitudDetallePago_Delete");
                throw exception;
            }
        }

        #endregion
    }

}
