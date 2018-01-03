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
    public class DAL_pyProyectoTareaCliente : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoTareaCliente(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTareaCliente> DAL_pyProyectoTareaCliente_Get(int numeroDoc, string tareaEntregable, string descripcion)
        {
            try
            {
                List<DTO_pyProyectoTareaCliente> results = new List<DTO_pyProyectoTareaCliente>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "";

                #region Creacion de Parametros
                if (!string.IsNullOrEmpty(tareaEntregable))
                {
                    query += " and TareaEntregable = @TareaEntregable ";

                    mySqlCommand.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                    mySqlCommand.Parameters["@TareaEntregable"].Value = tareaEntregable;
                }
                if (!string.IsNullOrEmpty(descripcion))
                {
                    query += " and Descripcion = @Descripcion ";

                    mySqlCommand.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                    mySqlCommand.Parameters["@Descripcion"].Value = descripcion;
                }
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc; 
                #endregion

                mySqlCommand.CommandText = "Select * from  pyProyectoTareaCliente where NumeroDoc = @NumeroDoc  " + query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                while (dr.Read())
                {
                    DTO_pyProyectoTareaCliente tarea = new DTO_pyProyectoTareaCliente(dr);
                    results.Add(tarea);
                }
                dr.Close(); 
               
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyProyectoTareaCliente_Add(DTO_pyProyectoTareaCliente tarea )
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "INSERT INTO pyProyectoTareaCliente " +
                                            "  ([NumeroDoc]  " +
                                            "  ,[TareaEntregable]  " +
                                            "  ,[Descripcion]  " +
                                            "  ,[FechaInicio]  " +
                                            "  ,[FechaFinal]  " +
                                            "  ,[FacturaInd]  " +
                                            "  ,[ServicioID]  " +
                                            "  ,[MonedaFactura]  " +
                                            "  ,[ValorFactura]  " +
                                            "  ,[Cantidad]  " +
                                            "  ,[Observaciones] " +
                                            "  ,[eg_faServicios]) " +
                                      "  VALUES  " +
                                            "  (@NumeroDoc  " +
                                            "  ,@TareaEntregable  " +
                                            "  ,@Descripcion  " +
                                            "  ,@FechaInicio  " +
                                            "  ,@FechaFinal  " +
                                            "  ,@FacturaInd  " +
                                            "  ,@ServicioID  " +
                                            "  ,@MonedaFactura  " +
                                            "  ,@ValorFactura  " +
                                            "  ,@Cantidad  " +
                                            "  ,@Observaciones  " +
                                            "  ,@eg_faServicios ) " +
                                            "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@MonedaFactura", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_faServicios", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaEntregable"].Value = tarea.TareaEntregable.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = tarea.Descripcion.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = tarea.FechaFinal.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ServicioID"].Value = tarea.ServicioID.Value;
                mySqlCommandSel.Parameters["@MonedaFactura"].Value = tarea.MonedaFactura.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@eg_faServicios"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faServicios, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTareaCliente_Upd(DTO_pyProyectoTareaCliente tarea )
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyProyectoTareaCliente Set  " +
                                            "   [NumeroDoc] = @NumeroDoc " +
                                            "  ,[TareaEntregable] = @TareaEntregable " +
                                            "  ,[Descripcion] = @Descripcion " +
                                            "  ,[FechaInicio]  = @FechaInicio" +
                                            "  ,[FechaFinal]  = @FechaFinal" +
                                            "  ,[FacturaInd]  = @FacturaInd" +
                                            "  ,[ServicioID]  = @ServicioID" +
                                            "  ,[MonedaFactura]  = @MonedaFactura" +
                                            "  ,[ValorFactura]  = @ValorFactura" +
                                            "  ,[Cantidad]  = @Cantidad" +
                                            "  ,[Observaciones]  = @Observaciones " +
                                            "  ,[eg_faServicios]  = @eg_faServicios " +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = tarea.Consecutivo.Value;
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descripcion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFinal", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FacturaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@ServicioID", SqlDbType.Char, UDT_ServicioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@MonedaFactura", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ValorFactura", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_faServicios", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaEntregable"].Value = tarea.TareaEntregable.Value;
                mySqlCommandSel.Parameters["@Descripcion"].Value = tarea.Descripcion.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFinal"].Value = tarea.FechaFinal.Value;
                mySqlCommandSel.Parameters["@FacturaInd"].Value = tarea.FacturaInd.Value;
                mySqlCommandSel.Parameters["@ServicioID"].Value = tarea.ServicioID.Value;
                mySqlCommandSel.Parameters["@MonedaFactura"].Value = tarea.MonedaFactura.Value;
                mySqlCommandSel.Parameters["@ValorFactura"].Value = tarea.ValorFactura.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@eg_faServicios"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.faServicios, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTareaCliente_DeleteByNumeroDoc(int numeroDoc, string tareaEntregable)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;


                string byTarea = string.Empty;
                if (!string.IsNullOrEmpty(tareaEntregable))
                {
                    byTarea = " and TareaEntregable = @TareaEntregable";
                    mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                    mySqlCommandSel.Parameters["@TareaEntregable"].Value = tareaEntregable;
                }

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoTareaCliente" +
                                            "  where NumeroDoc = @NumeroDoc " + byTarea;

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_DeleteByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTareaCliente_DeleteByConsecutivo(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoTareaCliente" +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyProyectoTareaCliente_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyProyectoTareaCliente with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaCliente_Exist");
                throw exception;
            }
        }

    }
}
