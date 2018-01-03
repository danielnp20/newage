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
    public class DAL_pyProyectoTareaHistoria : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoTareaHistoria(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTarea> DAL_pyProyectoTareaHistoria_Get(int? numDoc, byte version)
        {
            try
            {
                List<DTO_pyProyectoTarea> result = new List<DTO_pyProyectoTarea>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = " Select * from pyProyectoTareaHistoria  with(nolock) " +
                                           " Where NumeroDoc =  @NumeroDoc and Version = @Version";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numDoc;
                mySqlCommand.Parameters["@Version"].Value = version;

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                while(dr.Read())
                {
                    DTO_pyProyectoTarea res = new DTO_pyProyectoTarea(dr);
                    result.Add(res);
                }
                dr.Close();

                #region Consulta Detalle
                if (result.Count > 0)
                {
                    //mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numDoc;
                    mySqlCommand.Parameters["@Version"].Value = version;
                }
                foreach (DTO_pyProyectoTarea item in result)
                {
                    //mySqlCommand.Parameters["@ConsecTarea"].Value = item.Consecutivo.Value;
                    //mySqlCommand.CommandText =  " Select det.*,rec.Descriptivo as RecursoDesc,rec.TipoRecurso, rec.UnidadInvID ,trab.Descriptivo as TrabajoDesc  " +
                    //                            " From pyProyectoDetaHistoria det   with(nolock)    " +
                    //                            "   inner join pyRecurso  rec  with(nolock)  on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso      " +
                    //                            "   inner join inUnidad und  with(nolock)  on und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                    //                            "   left join pyTrabajo trab  with(nolock)  on trab.TrabajoID = det.TrabajoID and trab.EmpresaGrupoID = det.eg_pyTrabajo     " +
                    //                            "   where det.ConsecTarea = @ConsecTarea and det.NumeroDoc =  @NumeroDoc and  det.Version = @Version  " +
                    //                            "Order by rec.RecursoID ";
                    //dr = mySqlCommand.ExecuteReader();
                    //while (dr.Read())
                    //{
                    //    DTO_pyProyectoDeta d = new DTO_pyProyectoDeta(dr);
                    //    item.Detalle.Add(d);
                    //}
                    //dr.Close();
                }
                #endregion  

                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyProyectoTareaHistoria_Add(DTO_pyProyectoTarea tarea)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  INSERT INTO pyProyectoTareaHistoria  " +
                                            "  ([NumeroDoc]  " +
                                            "  ,[TareaID]  " +
                                            "  ,[TareaCliente]  " +
                                            "  ,[Version]  " +
                                            "  ,[Descriptivo]  " +
                                            "  ,[UnidadInvID]  " +
                                            "  ,[Cantidad]  " +
                                            "  ,[Observacion]  " +
                                            "  ,[CentroCostoID]  " +
                                            "  ,[CostoLocalCLI]  " +
                                            "  ,[CostoExtraCLI]  " +
                                            "  ,[CostoTotalUnitML]  " +
                                            "  ,[CostoTotalUnitME]  " +
                                            "  ,[CostoTotalML]  " +
                                            "  ,[CostoTotalME]  " +
                                            "  ,[FechaInicio]  " +
                                            "  ,[FechaFin]  " +
                                            "  ,[FechaTermina]  " +
                                            "  ,[Observaciones]  " +
                                            "  ,[Nivel]  " +
                                            "  ,[DetalleInd]  " +
                                            "  ,[TareaPadre]  " +
                                            "  ,[ImprimirTareaInd]  " +
                                            "  ,[CostoAdicionalInd]  " +
                                            "  ,[UsuarioID]  " +
                                            "  ,[PorDescuento]  " +
                                            "  ,[eg_pyTarea]  " +
                                            "  ,[eg_coCentroCosto]  " +
                                            "  ,[eg_inUnidad] ) " +
                                      "  VALUES  " +
                                            "  (@NumeroDoc  " +
                                            "  ,@TareaID  " +
                                            "  ,@TareaCliente  " +
                                            "  ,@Version  " +
                                            "  ,@Descriptivo  " +
                                            "  ,@UnidadInvID  " +
                                            "  ,@Cantidad  " +
                                            "  ,@Observacion  " +
                                            "  ,@CentroCostoID  " +
                                            "  ,@CostoLocalCLI  " +
                                            "  ,@CostoExtraCLI  " +
                                            "  ,@CostoTotalUnitML  " +
                                            "  ,@CostoTotalUnitME  " +
                                            "  ,@CostoTotalML  " +
                                            "  ,@CostoTotalME  " +
                                            "  ,@FechaInicio  " +
                                            "  ,@FechaFin  " +
                                            "  ,@FechaTermina  " +
                                            "  ,@Observaciones  " +
                                            "  ,@Nivel  " +
                                            "  ,@DetalleInd  " +
                                            "  ,@TareaPadre  " +
                                            "  ,@ImprimirTareaInd  " +
                                            "  ,@CostoAdicionalInd  " +
                                            "  ,@UsuarioID  " +
                                            "  ,@PorDescuento  " +
                                            "  ,@eg_pyTarea  " +
                                            "  ,@eg_coCentroCosto  " +
                                            "  ,@eg_inUnidad ) " +
                                            "   SET @Consecutivo = SCOPE_IDENTITY()";


                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@TareaCliente", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CostoLocalCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nivel", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DetalleInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TareaPadre", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@ImprimirTareaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CostoAdicionalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorDescuento", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_pyTarea", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_inUnidad", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength); 
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaID"].Value = tarea.TareaID.Value;
                mySqlCommandSel.Parameters["@TareaCliente"].Value = tarea.TareaCliente.Value;
                mySqlCommandSel.Parameters["@Version"].Value = tarea.Version.Value;
                mySqlCommandSel.Parameters["@Descriptivo"].Value = tarea.Descriptivo.Value;
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = tarea.UnidadInvID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = tarea.Observacion.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = tarea.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@CostoLocalCLI"].Value = tarea.CostoLocalCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraCLI"].Value = tarea.CostoExtraCLI.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitML"].Value = tarea.CostoTotalUnitML.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitME"].Value = tarea.CostoTotalUnitME.Value;
                mySqlCommandSel.Parameters["@CostoTotalML"].Value = tarea.CostoTotalML.Value;
                mySqlCommandSel.Parameters["@CostoTotalME"].Value = tarea.CostoTotalME.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = tarea.FechaFin.Value;
                mySqlCommandSel.Parameters["@FechaTermina"].Value = tarea.FechaTermina.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@Nivel"].Value = tarea.Nivel.Value;
                mySqlCommandSel.Parameters["@DetalleInd"].Value = tarea.DetalleInd.Value;
                mySqlCommandSel.Parameters["@TareaPadre"].Value = tarea.TareaPadre.Value;
                mySqlCommandSel.Parameters["@ImprimirTareaInd"].Value = tarea.ImprimirTareaInd.Value;
                mySqlCommandSel.Parameters["@CostoAdicionalInd"].Value = tarea.CostoAdicionalInd.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@PorDescuento"].Value = tarea.PorDescuento.Value;
                mySqlCommandSel.Parameters["@eg_pyTarea"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTarea, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_inUnidad"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inUnidad, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTareaHistoria_Upd(DTO_pyProyectoTarea tarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyProyectoTareaHistoria Set  " +
                                            "  [NumeroDoc] = @NumeroDoc " +
                                            "  ,[TareaID] = @TareaID " +
                                            "  ,[TareaCliente] = @TareaCliente " +
                                            "  ,[Version] = @Version " +
                                            "  ,[Descriptivo] = @Descriptivo " +
                                            "  ,[Cantidad] = @Cantidad " +
                                            "  ,[Observacion]  = @Observacion" +
                                            "  ,[CentroCostoID]  = @CentroCostoID" +
                                            "  ,[UnidadInvID]  = @UnidadInvID" +
                                            "  ,[CostoLocalCLI]  = @CostoLocalCLI" +
                                            "  ,[CostoExtraCLI]  = @CostoExtraCLI" +
                                            "  ,[CostoTotalUnitML]  = @CostoTotalUnitML" +
                                            "  ,[CostoTotalUnitME]  = @CostoTotalUnitME" +
                                            "  ,[CostoTotalML]  = @CostoTotalML" +
                                            "  ,[CostoTotalME]  = @CostoTotalME" +
                                            "  ,[FechaInicio]  = @FechaInicio" +
                                            "  ,[FechaFin]  = @FechaFin" +
                                            "  ,[FechaTermina]  = @FechaTermina" +
                                            "  ,[Observaciones]  = @Observaciones" +
                                            "  ,[Nivel]  = @Nivel" +
                                            "  ,[DetalleInd]  = @DetalleInd" +
                                            "  ,[TareaPadre]  = @TareaPadre" +
                                            "  ,[ImprimirTareaInd]  = @ImprimirTareaInd" +
                                            "  ,[CostoAdicionalInd]  = @CostoAdicionalInd" +
                                            "  ,[UsuarioID]  = @UsuarioID" +
                                            "  ,[PorDescuento]  = @PorDescuento" +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = tarea.Consecutivo.Value;
                #endregion

                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TareaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@TareaCliente", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CostoLocalCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nivel", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DetalleInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TareaPadre", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@ImprimirTareaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CostoAdicionalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorDescuento", SqlDbType.TinyInt);
                #endregion

                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = tarea.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@TareaID"].Value = tarea.TareaID.Value;
                mySqlCommandSel.Parameters["@TareaCliente"].Value = tarea.TareaCliente.Value;
                mySqlCommandSel.Parameters["@Version"].Value = tarea.Version.Value;
                mySqlCommandSel.Parameters["@Descriptivo"].Value = tarea.Descriptivo.Value;              
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = tarea.Observacion.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = tarea.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = tarea.UnidadInvID.Value;
                mySqlCommandSel.Parameters["@CostoLocalCLI"].Value = tarea.CostoLocalCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraCLI"].Value = tarea.CostoExtraCLI.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitML"].Value = tarea.CostoTotalUnitML.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitME"].Value = tarea.CostoTotalUnitME.Value;
                mySqlCommandSel.Parameters["@CostoTotalML"].Value = tarea.CostoTotalML.Value;
                mySqlCommandSel.Parameters["@CostoTotalME"].Value = tarea.CostoTotalME.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = tarea.FechaFin.Value;
                mySqlCommandSel.Parameters["@FechaTermina"].Value = tarea.FechaTermina.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@Nivel"].Value = tarea.Nivel.Value;
                mySqlCommandSel.Parameters["@DetalleInd"].Value = tarea.DetalleInd.Value;
                mySqlCommandSel.Parameters["@TareaPadre"].Value = tarea.TareaPadre.Value;
                mySqlCommandSel.Parameters["@ImprimirTareaInd"].Value = tarea.ImprimirTareaInd.Value;
                mySqlCommandSel.Parameters["@CostoAdicionalInd"].Value = tarea.CostoAdicionalInd.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@PorDescuento"].Value = tarea.PorDescuento.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTareaHistoria_DeleteByNumeroDoc(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoTareaHistoria " +
                                            "  where NumeroDoc = @NumeroDoc";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_DeleteByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoTarea DAL_pyProyectoTareaHistoria_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoTarea result = new DTO_pyProyectoTarea();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select * from pyProyectoTareaHistoria  with(nolock) " +
                                           "Where Consecutivo =  @Consecutivo ";

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

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_pyProyectoTarea(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyProyectoTareaHistoria_Exist(int numeroDoc, string tareaID, string tareaCliente, byte version)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                 
                mySqlCommand.CommandText = " select count(*) from pyProyectoTareaHistoria with(nolock)  " +
                                           " where NumeroDoc = @NumeroDoc and TareaID = @TareaID and TareaCliente = @TareaCliente and Version =@Version ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TareaID", SqlDbType.Char,UDT_TareaID.MaxLength);
                mySqlCommand.Parameters.Add("@TareaCliente", SqlDbType.Char,20);
                mySqlCommand.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@TareaID"].Value = tareaID;
                mySqlCommand.Parameters["@TareaCliente"].Value = tareaCliente;
                mySqlCommand.Parameters["@Version"].Value = version;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTareaHistoria_Exist");
                throw exception;
            }
        }
    }
}
