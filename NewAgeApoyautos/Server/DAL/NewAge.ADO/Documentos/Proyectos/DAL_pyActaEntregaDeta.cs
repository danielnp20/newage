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

namespace NewAge.ADO
{
    public class DAL_pyActaEntregaDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyActaEntregaDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyActaEntregaDeta_Add(DTO_pyActaEntregaDeta tarea)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO pyActaEntregaDeta " +
                                            "  ([NumeroDoc]  " +
                                            "  ,[ConsTareaCliente]  " +
                                            "  ,[ConsTareaEntrega]  " +
                                            "  ,[NumDocProyecto]  " +
                                            "  ,[FechaEntrega]  " +
                                            "  ,[EntregaFinalInd]  " +
                                            "  ,[PorEntregado]  " +
                                            "  ,[FacturaInd]  " +
                                            "  ,[ValorFactura]  " +
                                            "  ,[Cantidad]  " +
                                            "  ,[RespCliente]  " +
                                            "  ,[UsuarioID]  " +
                                            "  ,[NumDocFactura]  " +
                                            "  ,[Observaciones]) " +
                                      "  VALUES  " +
                                            "  (@NumeroDoc  " +
                                            "  ,@ConsTareaCliente  " +
                                            "  ,@ConsTareaEntrega  " +
                                            "  ,@NumDocProyecto  " +
                                            "  ,@FechaEntrega  " +
                                            "  ,@EntregaFinalInd  " +
                                            "  ,@PorEntregado  " +
                                            "  ,@FacturaInd  " +
                                            "  ,@ValorFactura  " +
                                            "  ,@Cantidad  " +
                                            "  ,@RespCliente  " +
                                            "  ,@UsuarioID  " +
                                            "  ,@NumDocFactura  " +
                                            "  ,@Observaciones) " +
                                            "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsTareaCliente", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsTareaEntrega", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EntregaFinalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorEntregado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RespCliente", SqlDbType.Char,60);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char ,UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsTareaCliente"].Value = tarea.ConsTareaCliente.Value;
                mySqlCommandSel.Parameters["@ConsTareaEntrega"].Value = tarea.ConsTareaEntrega.Value;
                mySqlCommandSel.Parameters["@NumDocProyecto"].Value = tarea.NumDocProyecto.Value;
                mySqlCommandSel.Parameters["@FechaEntrega"].Value = tarea.FechaEntrega.Value;
                mySqlCommandSel.Parameters["@EntregaFinalInd"].Value = tarea.EntregaFinalInd.Value;
                mySqlCommandSel.Parameters["@PorEntregado"].Value = tarea.PorEntregado.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@RespCliente"].Value = tarea.RespCliente.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@NumDocFactura"].Value = tarea.NumDocFactura.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
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
                int consecutivo = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
                tarea.Consecutivo.Value = consecutivo;
                return consecutivo;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyActaEntregaDeta_Upd(DTO_pyActaEntregaDeta tarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyActaEntregaDeta Set  " +
                                            "   [NumeroDoc] = @NumeroDoc " +
                                            "  ,[ConsTareaCliente] = @ConsTareaCliente " +
                                            "  ,[ConsTareaEntrega] = @ConsTareaEntrega " +
                                            "  ,[NumDocProyecto]  = @NumDocProyecto" +
                                            "  ,[FechaEntrega]  = @FechaEntrega" +
                                            "  ,[EntregaFinalInd]  = @EntregaFinalInd" +
                                            "  ,[PorEntregado]  = @PorEntregado" +
                                            "  ,[FacturaInd]  = @FacturaInd" +
                                            "  ,[ValorFactura]  = @ValorFactura" +
                                            "  ,[Cantidad]  = @Cantidad" +
                                            "  ,[RespCliente]  = @RespCliente" +
                                            "  ,[UsuarioID]  = @UsuarioID" +
                                            "  ,[NumDocFactura]  = @NumDocFactura" +
                                            "  ,[Observaciones]  = @Observaciones " +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = tarea.Consecutivo.Value;
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsTareaCliente", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsTareaEntrega", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@EntregaFinalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@PorEntregado", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@RespCliente", SqlDbType.Char, 60);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocFactura", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsTareaCliente"].Value = tarea.ConsTareaCliente.Value;
                mySqlCommandSel.Parameters["@ConsTareaEntrega"].Value = tarea.ConsTareaEntrega.Value;
                mySqlCommandSel.Parameters["@NumDocProyecto"].Value = tarea.NumDocProyecto.Value;
                mySqlCommandSel.Parameters["@FechaEntrega"].Value = tarea.FechaEntrega.Value;
                mySqlCommandSel.Parameters["@EntregaFinalInd"].Value = tarea.EntregaFinalInd.Value;
                mySqlCommandSel.Parameters["@PorEntregado"].Value = tarea.PorEntregado.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@RespCliente"].Value = tarea.RespCliente.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@NumDocFactura"].Value = tarea.NumDocFactura.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyActaEntregaDeta> DAL_pyActaEntregaDeta_Get(int? numeroDoc, int? numeroDocProy, int? consTareaCliente, int? consTareaEntrega)
        {
            try
            {
                List<DTO_pyActaEntregaDeta> results = new List<DTO_pyActaEntregaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "";

                #region Creacion de Parametros
                if (numeroDoc != null)
                {
                    query += " det.NumeroDoc = @NumeroDoc ";

                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                }
                if (numeroDocProy != null)
                {
                    if (!string.IsNullOrEmpty(query)) query += " and ";
                    query += " det.NumDocProyecto = @NumDocProyecto ";

                    mySqlCommand.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumDocProyecto"].Value = numeroDocProy;
                }
                if (consTareaCliente != null)
                {
                    if (!string.IsNullOrEmpty(query)) query += " and ";
                    query += " det.ConsTareaCliente = @ConsTareaCliente ";

                    mySqlCommand.Parameters.Add("@ConsTareaCliente", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsTareaCliente"].Value = consTareaCliente;
                }
                if (consTareaEntrega != null)
                {
                    if (!string.IsNullOrEmpty(query)) query += " and ";
                    query += " det.ConsTareaEntrega = @ConsTareaEntrega ";

                    mySqlCommand.Parameters.Add("@ConsTareaEntrega", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsTareaEntrega"].Value = consTareaEntrega;
                }

                if (!string.IsNullOrEmpty(query)) query = " where " + query;

                #endregion

                mySqlCommand.CommandText = "Select tarea.TareaEntregable, tarea.Descripcion, det.* from  pyActaEntregaDeta det " +
                                           " Inner join pyProyectoTareaCliente tarea with(nolock) on tarea.Consecutivo = det.ConsTareaCliente " + query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_pyActaEntregaDeta tarea = new DTO_pyActaEntregaDeta(dr);
                    results.Add(tarea);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyActaEntregaDeta> DAL_pyActaEntregaDeta_GetByParameter(DTO_pyActaEntregaDeta filter)
        {
            try
            {
                List<DTO_pyActaEntregaDeta> results = new List<DTO_pyActaEntregaDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = string.Empty;

                mySqlCommand.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.SmallInt);
                mySqlCommand.Parameters["@NumDocProyecto"].Value = filter.NumDocProyecto.Value;
                mySqlCommand.Parameters["@Estado"].Value = (byte)EstadoDocControl.Aprobado;
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query = "and acta.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                }
                if (!string.IsNullOrEmpty(filter.ConsTareaCliente.Value.ToString()))
                {
                    query += "and acta.ConsTareaCliente = @ConsTareaCliente ";
                    mySqlCommand.Parameters.Add("@ConsTareaCliente", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsTareaCliente"].Value = filter.ConsTareaCliente.Value;
                }
                if (!string.IsNullOrEmpty(filter.ConsTareaEntrega.Value.ToString()))
                {
                    query += "and acta.ConsTareaEntrega = @ConsTareaEntrega ";
                    mySqlCommand.Parameters.Add("@ConsTareaEntrega", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsTareaEntrega"].Value = filter.ConsTareaEntrega.Value;
                }
                if (!string.IsNullOrEmpty(filter.NumDocFactura.Value.ToString()))
                {
                    query += "and acta.NumDocFactura = @NumDocFactura ";
                    mySqlCommand.Parameters.Add("@NumDocFactura", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumDocFactura"].Value = filter.NumDocFactura.Value;
                }

                query = "Select acta.*, tarea.TareaEntregable,tarea.Descripcion " +
                        " From pyActaEntregaDeta acta with(nolock) " +
                        " inner join gldocumentocontrol ctrl with(nolock) on ctrl.NumeroDoc = acta.NumeroDoc " +
                        " inner join pyProyectoTareaCliente tarea with(nolock) on tarea.Consecutivo = acta.ConsTareaCliente " +
                        "where NumDocProyecto = @NumDocProyecto and ctrl.Estado = @Estado " + query;

                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_pyActaEntregaDeta ctrl = new DTO_pyActaEntregaDeta(dr);
                    results.Add(ctrl);
                    index++;
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyActaEntregaDeta_DeleteByConsecutivo(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyActaEntregaDeta" +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consecutivo;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyActaEntregaDeta_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyActaEntregaDeta with(nolock) where  Consecutivo = @Consecutivo";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                int count = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                return count == 0 ? false : true;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaEntregaDeta_Exist");
                throw exception;
            }
        }
    }
}
