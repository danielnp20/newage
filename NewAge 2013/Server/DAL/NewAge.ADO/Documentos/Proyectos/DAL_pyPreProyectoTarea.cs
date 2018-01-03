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
    public class DAL_pyPreProyectoTarea : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyPreProyectoTarea(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyPreProyectoTarea> DAL_pyPreProyectoTarea_Get(int? numeroDoc, string tareaID, string claseServicioID)
        {
            try
            {
                List<DTO_pyPreProyectoTarea> results = new List<DTO_pyPreProyectoTarea>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "";

                if (!string.IsNullOrEmpty(numeroDoc.ToString()))
                {
                    #region Carga Tareas Existentes
                    query = "SELECT proy.*, cap.CapituloTareaID,cap.Descriptivo as CapituloDesc,cap.CapituloGrupoID    " +
                            " FROM pyPreProyectoTarea proy with(nolock)  " +
                            " LEFT JOIN pyTarea tarea with(nolock) on tarea.TareaID = proy.TareaID and tarea.EmpresaGrupoID = proy.eg_pyTarea    " +
                            " LEFT JOIN pyTareaCapitulo cap with(nolock) on cap.CapituloTareaID = tarea.CapituloTareaID   " +
                            "WHERE NumeroDoc = @NumeroDoc "+
                            " Order by proy.Consecutivo ";

                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                    mySqlCommand.CommandText = query;

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        DTO_pyPreProyectoTarea tarea = new DTO_pyPreProyectoTarea(dr);
                        tarea.CapituloTareaID.Value = dr["CapituloTareaID"].ToString();
                        tarea.CapituloDesc.Value = dr["CapituloDesc"].ToString();
                        tarea.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
                        results.Add(tarea);
                    }
                    dr.Close(); 
                    #endregion     
                }
                else
                {
                    string where = string.Empty;
                    if (!string.IsNullOrEmpty(claseServicioID))
                    {
                        where += " where tareaClase.ClaseServicioID = @ClaseServicioID ";
                        mySqlCommand.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                        mySqlCommand.Parameters["@ClaseServicioID"].Value = claseServicioID;
                    }

                    #region Carga Tareas Iniciales parametrizadas
                    query += "SELECT tar.TareaID,tar.Descriptivo,tar.CapituloTareaID,tar.UnidadInvID,cap.Descriptivo as CapituloDesc,cap.CapituloGrupoID  " +
                            "    FROM   pyTarea tar with(nolock) " +
                            " LEFT JOIN pyTareaClase tareaClase with(nolock) on  tareaClase.TareaID = tar.TareaID and tareaClase.eg_pyTarea = tar.EmpresaGrupoID  " +
                            " LEFT JOIN pyTareaCapitulo cap with(nolock) on cap.CapituloTareaID = tar.CapituloTareaID  and cap.EmpresaGrupoID = tar.eg_pyTareaCapitulo  " + where +
                            " Order by cap.CapituloGrupoID ";
                    mySqlCommand.CommandText = query;

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        DTO_pyPreProyectoTarea tarea = new DTO_pyPreProyectoTarea();
                        tarea.TareaID.Value = dr["TareaID"].ToString();
                        tarea.TareaCliente.Value = dr["TareaID"].ToString();
                        tarea.CapituloTareaID.Value = dr["CapituloTareaID"].ToString();
                        tarea.CapituloDesc.Value = dr["CapituloDesc"].ToString();
                        tarea.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
                        tarea.Descriptivo.Value = dr["Descriptivo"].ToString();
                        tarea.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                        tarea.Cantidad.Value = 1;
                        results.Add(tarea);
                    }
                    dr.Close(); 
                    #endregion     
                }
                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyPreProyectoTarea_Add(DTO_pyPreProyectoTarea tarea)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  INSERT INTO pyPreProyectoTarea  " +
                                            "  ([NumeroDoc]  " +
                                            "  ,[TareaID]  " +
                                            "  ,[TareaCliente]  " +
                                            "  ,[Descriptivo]  " +
                                            "  ,[UnidadInvID]  " +
                                            "  ,[Cantidad]  " +
                                            "  ,[Observacion]  " +
                                            "  ,[CentroCostoID]  " +
                                            "  ,[CostoLocalCLI]  " +
                                            "  ,[CostoExtraCLI]  " +
                                            "  ,[CostoLocalUnitCLI]  " +
                                            "  ,[CostoExtraUnitCLI]  " +
                                            "  ,[CostoTotalUnitML]  " +
                                            "  ,[CostoTotalUnitME]  " +
                                            "  ,[CostoTotalML]  " +
                                            "  ,[CostoTotalME]  " +
                                            "  ,[FechaInicio]  " +
                                            "  ,[FechaFin]  " +
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
                                            "  ,@Descriptivo  " +
                                            "  ,@UnidadInvID  " +
                                            "  ,@Cantidad  " +
                                            "  ,@Observacion  " +
                                            "  ,@CentroCostoID  " +
                                            "  ,@CostoLocalCLI  " +
                                            "  ,@CostoExtraCLI  " +
                                            "  ,@CostoLocalUnitCLI  " +
                                            "  ,@CostoExtraUnitCLI  " +
                                            "  ,@CostoTotalUnitML  " +
                                            "  ,@CostoTotalUnitME  " +
                                            "  ,@CostoTotalML  " +
                                            "  ,@CostoTotalME  " +
                                            "  ,@FechaInicio  " +
                                            "  ,@FechaFin  " +
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
                mySqlCommandSel.Parameters.Add("@Descriptivo", SqlDbType.Char, UDT_Descriptivo.MaxLength);
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CostoLocalCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalUnitCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraUnitCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
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
                mySqlCommandSel.Parameters["@Descriptivo"].Value = tarea.Descriptivo.Value;
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = tarea.UnidadInvID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = tarea.Observacion.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = tarea.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@CostoLocalCLI"].Value = tarea.CostoLocalCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraCLI"].Value = tarea.CostoExtraCLI.Value;
                mySqlCommandSel.Parameters["@CostoLocalUnitCLI"].Value = tarea.CostoLocalUnitCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraUnitCLI"].Value = tarea.CostoExtraUnitCLI.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitML"].Value = tarea.CostoTotalUnitML.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitME"].Value = tarea.CostoTotalUnitME.Value;
                mySqlCommandSel.Parameters["@CostoTotalML"].Value = tarea.CostoTotalML.Value;
                mySqlCommandSel.Parameters["@CostoTotalME"].Value = tarea.CostoTotalME.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = tarea.FechaFin.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyPreProyectoTarea_Upd(DTO_pyPreProyectoTarea tarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyPreProyectoTarea Set  " +
                                            "  [NumeroDoc] = @NumeroDoc " +
                                            "  ,[TareaID] = @TareaID " +
                                            "  ,[TareaCliente] = @TareaCliente " +
                                            "  ,[Descriptivo] = @Descriptivo " +
                                            "  ,[Cantidad] = @Cantidad " +
                                            "  ,[Observacion]  = @Observacion" +
                                            "  ,[CentroCostoID]  = @CentroCostoID" +
                                            "  ,[UnidadInvID]  = @UnidadInvID" +
                                            "  ,[CostoLocalCLI]  = @CostoLocalCLI" +
                                            "  ,[CostoExtraCLI]  = @CostoExtraCLI" +
                                            "  ,[CostoLocalUnitCLI]  = @CostoLocalUnitCLI" +
                                            "  ,[CostoExtraUnitCLI]  = @CostoExtraUnitCLI" +
                                            "  ,[CostoTotalUnitML]  = @CostoTotalUnitML" +
                                            "  ,[CostoTotalUnitME]  = @CostoTotalUnitME" +
                                            "  ,[CostoTotalML]  = @CostoTotalML" +
                                            "  ,[CostoTotalME]  = @CostoTotalME" +
                                            "  ,[FechaInicio]  = @FechaInicio" +
                                            "  ,[FechaFin]  = @FechaFin" +
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
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observacion", SqlDbType.Char, UDT_DescripTBase.MaxLength);
                mySqlCommandSel.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_UnidadInvID.MaxLength);
                mySqlCommandSel.Parameters.Add("@CostoLocalCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalUnitCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraUnitCLI", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalUnitME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoTotalME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
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
                mySqlCommandSel.Parameters["@Descriptivo"].Value = tarea.Descriptivo.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = tarea.Cantidad.Value;
                mySqlCommandSel.Parameters["@Observacion"].Value = tarea.Observacion.Value;
                mySqlCommandSel.Parameters["@CentroCostoID"].Value = tarea.CentroCostoID.Value;
                mySqlCommandSel.Parameters["@UnidadInvID"].Value = tarea.UnidadInvID.Value;
                mySqlCommandSel.Parameters["@CostoLocalCLI"].Value = tarea.CostoLocalCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraCLI"].Value = tarea.CostoExtraCLI.Value;
                mySqlCommandSel.Parameters["@CostoLocalUnitCLI"].Value = tarea.CostoLocalUnitCLI.Value;
                mySqlCommandSel.Parameters["@CostoExtraUnitCLI"].Value = tarea.CostoExtraUnitCLI.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitML"].Value = tarea.CostoTotalUnitML.Value;
                mySqlCommandSel.Parameters["@CostoTotalUnitME"].Value = tarea.CostoTotalUnitME.Value;
                mySqlCommandSel.Parameters["@CostoTotalML"].Value = tarea.CostoTotalML.Value;
                mySqlCommandSel.Parameters["@CostoTotalME"].Value = tarea.CostoTotalME.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = tarea.FechaFin.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyPreProyectoTarea_DeleteByNumeroDoc(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyPreProyectoTarea " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_DeleteByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyPreProyectoTarea_DeleteByConsecutivo(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyPreProyectoTarea " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyPreProyectoTarea DAL_pyPreProyectoTarea_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyPreProyectoTarea result = new DTO_pyPreProyectoTarea();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select * from pyPreProyectoTarea  with(nolock) " +
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
                    result = new DTO_pyPreProyectoTarea(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyPreProyectoTarea_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyPreProyectoTarea with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoTarea_Exist");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_TareasFilter> DAL_TareasFilter_Get(DTO_TareasFilter filter)
        {
            try
            {
               List<DTO_TareasFilter> results = new List<DTO_TareasFilter>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = " Select t.TareaID,t.Descriptivo as TareaDesc, tc.ClaseServicioID,clas.Descriptivo as ClaseServicioDesc, tr.RecursoID, rec.Descriptivo as RecursoDesc, " +
                                " ref.inReferenciaID, ref.RefProveedor, ref.MarcaInvID,marc.Descriptivo as MarcaDesc, " +
                                " ref.MaterialInvID,mat.Descriptivo as MaterialDesc,  ref.SerieID,ser.Descriptivo as SerieDesc,  " +
                                " rec.UnidadInvID,und.Descriptivo as UnidadDesc,  ref.EmpaqueInvID,emp.Descriptivo as EmpaqueDesc,  " +
                                " ref.ClaseInvID,cla.Descriptivo as ClaseDesc,ref.GrupoInvID,grup.Descriptivo as GrupoDesc,ref.TipoInvID,tip.Descriptivo as TipoDesc  " +
                                " From pyTarea t with(nolock) " +
                                "   left join pyTareaClase tc with(nolock) on tc.TareaID = t.TareaID and t.EmpresaGrupoID = tc.eg_pyTarea " +
                                "   left join pyTareaRecurso tr with(nolock) on tr.TareaID = t.TareaID and t.EmpresaGrupoID = tr.eg_pyTarea " +
                                "   left join pyRecurso rec with(nolock) on rec.RecursoID = tr.RecursoID and rec.EmpresaGrupoID = tr.eg_pyRecurso " +
                                "   left join inReferencia ref with(nolock) on ref.inReferenciaID = rec.inReferenciaID and ref.EmpresaGrupoID = rec.eg_inReferencia " +
                                "   left join inMarca marc with(nolock) on marc.MarcaInvID = ref.MarcaInvID and marc.EmpresaGrupoID = ref.eg_inMarca " +
                                "   left join inMaterial mat with(nolock) on mat.MaterialInvID = ref.MaterialInvID and mat.EmpresaGrupoID = ref.eg_inMaterial " +
                                "   left join inSerie ser with(nolock) on ser.SerieID = ref.SerieID and ser.EmpresaGrupoID = ref.eg_inSerie " +
                                "   left join inUnidad und with(nolock) on und.UnidadInvID = ref.UnidadInvID and und.EmpresaGrupoID = ref.eg_inUnidad " +
                                "   left join inEmpaque emp with(nolock) on emp.EmpaqueInvID = ref.EmpaqueInvID and emp.EmpresaGrupoID = ref.eg_inEmpaque " +
                                "   left join inRefClase cla with(nolock) on cla.ClaseInvID = ref.ClaseInvID and cla.EmpresaGrupoID = ref.eg_inRefClase " +
                                "   left join inRefGrupo grup with(nolock) on grup.GrupoInvID = ref.GrupoInvID and grup.EmpresaGrupoID = ref.eg_inRefGrupo " +
                                "   left join inRefTipo tip with(nolock) on tip.TipoInvID = ref.TipoInvID and tip.EmpresaGrupoID = ref.eg_inRefTipo " +
                                "   left join pyClaseProyecto clas with(nolock) on clas.ClaseServicioID = tc.ClaseServicioID and clas.EmpresaGrupoID = tc.eg_pyClaseProyecto " +
                                " Where t.EmpresaGrupoID = @EmpresaID  ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(filter.TareaID.Value.ToString()))
                    query += " and t.TareaID like '%" + filter.TareaID.Value + "%'";
                if (!string.IsNullOrEmpty(filter.TareaDesc.Value.ToString()))
                    query += " and t.Descriptivo like '%" + filter.TareaDesc.Value + "%' ";
                if (!string.IsNullOrEmpty(filter.RefProveedor.Value.ToString()))
                    query += " and ref.RefProveedor like '%" + filter.RefProveedor.Value + "%' ";
                if (!string.IsNullOrEmpty(filter.MarcaInvID.Value))
                {
                    query += " and ref.MarcaInvID = @MarcaInvID ";
                    mySqlCommand.Parameters.Add("@MarcaInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@MarcaInvID"].Value = filter.MarcaInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.MaterialInvID.Value))
                {
                    query += " and ref.MaterialInvID = @MaterialInvID ";
                    mySqlCommand.Parameters.Add("@MaterialInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@MaterialInvID"].Value = filter.MaterialInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.SerieID.Value.ToString()))
                {
                    query += " and ref.SerieID = @SerieID ";
                    mySqlCommand.Parameters.Add("@SerieID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@SerieID"].Value = filter.SerieID.Value;
                }
                if (!string.IsNullOrEmpty(filter.UnidadInvID.Value.ToString()))
                {
                    query += " and rec.UnidadInvID = @UnidadInvID ";
                    mySqlCommand.Parameters.Add("@UnidadInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@UnidadInvID"].Value = filter.UnidadInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.EmpaqueInvID.Value.ToString()))
                {
                    query += " and ref.EmpaqueInvID = @EmpaqueInvID ";
                    mySqlCommand.Parameters.Add("@EmpaqueInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@EmpaqueInvID"].Value = filter.EmpaqueInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.ClaseInvID.Value.ToString()))
                {
                    query += " and ref.ClaseInvID = @ClaseInvID ";
                    mySqlCommand.Parameters.Add("@ClaseInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@ClaseInvID"].Value = filter.ClaseInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.GrupoInvID.Value.ToString()))
                {
                    query += " and ref.GrupoInvID = @GrupoInvID ";
                    mySqlCommand.Parameters.Add("@GrupoInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@GrupoInvID"].Value = filter.GrupoInvID.Value;
                }
                if (!string.IsNullOrEmpty(filter.TipoInvID.Value.ToString()))
                {
                    query += " and ref.TipoInvID = @TipoInvID ";
                    mySqlCommand.Parameters.Add("@TipoInvID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@TipoInvID"].Value = filter.TipoInvID.Value;
                }
                
                mySqlCommand.CommandText = query;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_TareasFilter tar = new DTO_TareasFilter(dr);
                    tar.SelectInd.Value = false;
                    results.Add(tar);
                }
                dr.Close();

                return results;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_TareasFilter_Get");
                throw exception;
            }
        }

    }
}
