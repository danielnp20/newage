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
    public class DAL_pyProyectoPlanEntrega : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoPlanEntrega(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyProyectoPlanEntrega> DAL_pyProyectoPlanEntrega_GetTareaCliente(int consecTarea, DateTime? fechaEntrega)
        {
            try
            {
                List<DTO_pyProyectoPlanEntrega> results = new List<DTO_pyProyectoPlanEntrega>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "";

                #region Creacion de Parametros
                if (fechaEntrega != null)
                {
                    query += " and FechaEntrega = @FechaEntrega ";

                    mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                    mySqlCommand.Parameters["@FechaEntrega"].Value = fechaEntrega.Value.Date;
                }
                
                mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea; 
                #endregion

                mySqlCommand.CommandText = "Select * from  pyProyectoPlanEntrega where ConsecTarea = @ConsecTarea  " + query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_pyProyectoPlanEntrega tarea = new DTO_pyProyectoPlanEntrega(dr);
                    results.Add(tarea);
                }
                dr.Close(); 
               
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoPlanEntrega_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyProyectoPlanEntrega_Add(DTO_pyProyectoPlanEntrega tarea )
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO pyProyectoPlanEntrega " +
                                            "  ([ConsecTarea]  " +
                                            "  ,[FechaEntrega]  " +
                                            "  ,[TipoEntrega]  " +
                                            "  ,[PorEntrega]  " +
                                            "  ,[FacturaInd]  " +
                                            "  ,[ValorFactura]  " +
                                            "  ,[Cantidad]  " +
                                            "  ,[Observaciones]  " +
                                            "  ,[FechaRecaudoFactura]) " +
                                      "  VALUES  " +
                                            "  (@ConsecTarea  " +
                                            "  ,@FechaEntrega  " +
                                            "  ,@TipoEntrega  " +
                                            "  ,@PorEntrega  " +
                                            "  ,@FacturaInd  " +
                                            "  ,@ValorFactura  " +
                                            "  ,@Cantidad  " +
                                            "  ,@Observaciones " +
                                            "  ,@FechaRecaudoFactura ) " +
                                            "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TipoEntrega", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorEntrega", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaRecaudoFactura", SqlDbType.SmallDateTime);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = tarea.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@FechaEntrega"].Value = tarea.FechaEntrega.Value;
                mySqlCommandSel.Parameters["@TipoEntrega"].Value = tarea.TipoEntrega.Value;
                mySqlCommandSel.Parameters["@PorEntrega"].Value = tarea.PorEntrega.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@FechaRecaudoFactura"].Value = tarea.FechaRecaudoFactura.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoPlanEntrega_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoPlanEntrega_Upd(DTO_pyProyectoPlanEntrega tarea )
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyProyectoPlanEntrega Set  " +
                                            "   [ConsecTarea] = @ConsecTarea " +
                                            "  ,[FechaEntrega] = @FechaEntrega " +
                                            "  ,[TipoEntrega] = @TipoEntrega " +
                                            "  ,[PorEntrega]  = @PorEntrega" +
                                            "  ,[FacturaInd]  = @FacturaInd" +
                                            "  ,[ValorFactura]  = @ValorFactura" +
                                            "  ,[Cantidad]  = @Cantidad" +
                                            "  ,[Observaciones]  = @Observaciones " +
                                            "  ,[FechaRecaudoFactura]  = @FechaRecaudoFactura " +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = tarea.Consecutivo.Value;
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FechaEntrega", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@TipoEntrega", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@PorEntrega", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaRecaudoFactura", SqlDbType.SmallDateTime);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = tarea.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@FechaEntrega"].Value = tarea.FechaEntrega.Value;
                mySqlCommandSel.Parameters["@TipoEntrega"].Value = tarea.TipoEntrega.Value;
                mySqlCommandSel.Parameters["@PorEntrega"].Value = tarea.PorEntrega.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@FechaRecaudoFactura"].Value = tarea.FechaRecaudoFactura.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoPlanEntrega_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoPlanEntrega_DeleteByConsecutivo(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoPlanEntrega" +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoPlanEntrega_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyProyectoPlanEntrega_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyProyectoPlanEntrega with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoPlanEntrega_Exist");
                throw exception;
            }
        }

    }
}
