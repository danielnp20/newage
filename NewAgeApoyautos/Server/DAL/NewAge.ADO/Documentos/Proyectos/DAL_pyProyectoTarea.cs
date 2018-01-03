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
    public class DAL_pyProyectoTarea : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoTarea(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene detalle Proyectos
        /// </summary>
        /// <param name="periodo">periodo (opcional)</param>
        /// <returns></returns>
        public List<DTO_pyProyectoTarea> DAL_pyProyectoTarea_Get(int? numeroDoc, string tareaID)
        {
            try
            {
                List<DTO_pyProyectoTarea> results = new List<DTO_pyProyectoTarea>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "";

                if (!string.IsNullOrEmpty(numeroDoc.ToString()))
                {
                    #region Carga Tareas Existentes
                    query = "SELECT proy.*,cap.CapituloTareaID,cap.Descriptivo as CapituloDesc,cap.CapituloGrupoID," +
                            " Isnull((select top(1) Cantidad from pyPreProyectoTarea where NumeroDoc = docu.DocSolicitud and TareaID = proy.TareaID and Descriptivo = proy.Descriptivo),0) as CantidadPresup " +
                            " FROM pyProyectoTarea proy with(nolock)  " +
                            " LEFT JOIN pyTarea tarea with(nolock) on tarea.TareaID = proy.TareaID and tarea.EmpresaGrupoID = proy.eg_pyTarea    " +
                            " LEFT JOIN pyTareaCapitulo cap with(nolock) on cap.CapituloTareaID = tarea.CapituloTareaID   " +
                            " LEFT JOIN pyProyectoDocu docu with(nolock) on docu.NumeroDoc = @NumeroDoc  " +
                            "WHERE proy.NumeroDoc = @NumeroDoc " +
                            " Order by  proy.Consecutivo ";

                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;

                    mySqlCommand.CommandText = query;

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        DTO_pyProyectoTarea tarea = new DTO_pyProyectoTarea(dr);
                        tarea.CapituloTareaID.Value = dr["CapituloTareaID"].ToString();
                        tarea.CapituloDesc.Value = dr["CapituloDesc"].ToString();
                        tarea.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
                        tarea.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                        if (!string.IsNullOrEmpty(dr["CantidadPresup"].ToString())) //Cantidad del Preproyecto
                            tarea.CantidadPresup.Value = Convert.ToDecimal(dr["CantidadPresup"]);
                        results.Add(tarea);
                    }
                    dr.Close(); 
                    #endregion     
                }
                else
                {
                    #region Carga Tareas Iniciales parametrizadas(pyTarea)
                    query += "SELECT tar.TareaID,tar.Descriptivo,tar.CapituloTareaID,tar.UnidadInvID,cap.Descriptivo as CapituloDesc, cap.CapituloGrupoID " +
                            "FROM   pyTarea tar with(nolock) " +
                            "LEFT JOIN pyTareaClase tareaClase with(nolock) on  tareaClase.TareaID = tar.TareaID and tareaClase.eg_pyTarea = tar.EmpresaGrupoID  " +
                            "LEFT JOIN pyTareaCapitulo cap with(nolock) on cap.CapituloTareaID = tar.CapituloTareaID  and cap.EmpresaGrupoID = tar.eg_pyTareaCapitulo   "+
                            " Order by cap.CapituloGrupoID ";
                    mySqlCommand.CommandText = query;

                    SqlDataReader dr;
                    dr = mySqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        DTO_pyProyectoTarea tarea = new DTO_pyProyectoTarea();
                        tarea.TareaID.Value = dr["TareaID"].ToString();
                        tarea.TareaCliente.Value = dr["TareaID"].ToString();
                        tarea.Descriptivo.Value = dr["Descriptivo"].ToString();
                        tarea.CapituloTareaID.Value = dr["CapituloTareaID"].ToString();
                        tarea.CapituloDesc.Value = dr["CapituloDesc"].ToString();
                        tarea.CapituloGrupoID.Value = dr["CapituloGrupoID"].ToString();
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Ingresa ingformación en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public int DAL_pyProyectoTarea_Add(DTO_pyProyectoTarea tarea)
        {
            try
            {

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  INSERT INTO pyProyectoTarea  " +
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
                                            "  ,[FechaTermina]  " +
                                            "  ,[Observaciones]  " +
                                            "  ,[Nivel]  " +
                                            "  ,[DetalleInd]  " +
                                            "  ,[TareaPadre]  " +
                                            "  ,[TareaEntregable]  " +
                                            "  ,[ImprimirTareaInd]  " +
                                            "  ,[CostoAdicionalInd]  " +
                                            "  ,[UsuarioID]  " +
                                            "  ,[PorDescuento]  " +
                                            "  ,[ConsEntrega]  " +
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
                                            "  ,@FechaTermina  " +
                                            "  ,@Observaciones  " +
                                            "  ,@Nivel  " +
                                            "  ,@DetalleInd  " +
                                            "  ,@TareaPadre  " +
                                            "  ,@TareaEntregable  " +
                                            "  ,@ImprimirTareaInd  " +
                                            "  ,@CostoAdicionalInd  " +
                                            "  ,@UsuarioID  " +
                                            "  ,@PorDescuento  " +
                                            "  ,@ConsEntrega  " +
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
                mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nivel", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DetalleInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TareaPadre", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@ImprimirTareaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CostoAdicionalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorDescuento", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ConsEntrega", SqlDbType.Int);
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
                mySqlCommandSel.Parameters["@FechaTermina"].Value = tarea.FechaTermina.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@Nivel"].Value = tarea.Nivel.Value;
                mySqlCommandSel.Parameters["@DetalleInd"].Value = tarea.DetalleInd.Value;
                mySqlCommandSel.Parameters["@TareaPadre"].Value = tarea.TareaPadre.Value;
                mySqlCommandSel.Parameters["@TareaEntregable"].Value = tarea.TareaEntregable.Value;
                mySqlCommandSel.Parameters["@ImprimirTareaInd"].Value = tarea.ImprimirTareaInd.Value;
                mySqlCommandSel.Parameters["@CostoAdicionalInd"].Value = tarea.CostoAdicionalInd.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@PorDescuento"].Value = tarea.PorDescuento.Value;
                mySqlCommandSel.Parameters["@ConsEntrega"].Value = tarea.ConsEntrega.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTarea_Upd(DTO_pyProyectoTarea tarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                string updFechas = string.Empty;
                if(tarea.FechaInicio.Value.HasValue)
                {
                    updFechas = "  ,[FechaInicio] = @FechaInicio,[FechaFin] = @FechaFin,[FechaTermina]  = @FechaTermina ";
                    mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                    mySqlCommandSel.Parameters["@FechaInicio"].Value = tarea.FechaInicio.Value;
                    mySqlCommandSel.Parameters["@FechaFin"].Value = tarea.FechaFin.Value;
                    mySqlCommandSel.Parameters["@FechaTermina"].Value = tarea.FechaTermina.Value;
                }

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyProyectoTarea Set  " +
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
                                                updFechas +
                                            "  ,[Observaciones]  = @Observaciones" +
                                            "  ,[Nivel]  = @Nivel" +
                                            "  ,[DetalleInd]  = @DetalleInd" +
                                            "  ,[TareaPadre]  = @TareaPadre" +
                                            "  ,[TareaEntregable]  = @TareaEntregable" +
                                            "  ,[ImprimirTareaInd]  = @ImprimirTareaInd" +
                                            "  ,[CostoAdicionalInd]  = @CostoAdicionalInd" +
                                            "  ,[UsuarioID]  = @UsuarioID" +
                                            "  ,[PorDescuento]  = @PorDescuento" +
                                            "  ,[ConsEntrega]  = @ConsEntrega" +
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
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@Nivel", SqlDbType.SmallInt);
                mySqlCommandSel.Parameters.Add("@DetalleInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@TareaPadre", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@ImprimirTareaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@CostoAdicionalInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommandSel.Parameters.Add("@PorDescuento", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@ConsEntrega", SqlDbType.Int);
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
                mySqlCommandSel.Parameters["@Observaciones"].Value = tarea.Observaciones.Value;
                mySqlCommandSel.Parameters["@Nivel"].Value = tarea.Nivel.Value;
                mySqlCommandSel.Parameters["@DetalleInd"].Value = tarea.DetalleInd.Value;
                mySqlCommandSel.Parameters["@TareaPadre"].Value = tarea.TareaPadre.Value;
                mySqlCommandSel.Parameters["@TareaEntregable"].Value = tarea.TareaEntregable.Value;
                mySqlCommandSel.Parameters["@ImprimirTareaInd"].Value = tarea.ImprimirTareaInd.Value;
                mySqlCommandSel.Parameters["@CostoAdicionalInd"].Value = tarea.CostoAdicionalInd.Value;
                mySqlCommandSel.Parameters["@UsuarioID"].Value = tarea.UsuarioID.Value;
                mySqlCommandSel.Parameters["@PorDescuento"].Value = tarea.PorDescuento.Value;
                mySqlCommandSel.Parameters["@ConsEntrega"].Value = tarea.ConsEntrega.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza información en la programación 
        /// </summary>
        /// <param name="tarea">programación</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTarea_UpdEntregable(DTO_pyProyectoTarea tarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "  UPDATE pyProyectoTarea Set  " +                                         
                                            "   [TareaEntregable] = @TareaEntregable" +
                                            "  ,[ConsEntrega]  = @ConsEntrega" +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = tarea.Consecutivo.Value;
                #endregion

                #region Crea parametros               
                mySqlCommandSel.Parameters.Add("@TareaEntregable", SqlDbType.Char, UDT_CodigoGrl20.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConsEntrega", SqlDbType.Int);
                #endregion

                #region Asigna valores
                mySqlCommandSel.Parameters["@TareaEntregable"].Value = tarea.TareaEntregable.Value;
                mySqlCommandSel.Parameters["@ConsEntrega"].Value = tarea.ConsEntrega.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_UpdEntregable");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTarea_DeleteByNumeroDoc(int numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoTarea " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_DeleteByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoTarea_DeleteByConsecutivo(int consecutivo)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoTarea " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoTarea DAL_pyProyectoTarea_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoTarea result = new DTO_pyProyectoTarea();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select * from pyProyectoTarea  with(nolock) " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyProyectoTarea_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyProyectoTarea with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoTarea_Exist");
                throw exception;
            }
        }
    }
}
