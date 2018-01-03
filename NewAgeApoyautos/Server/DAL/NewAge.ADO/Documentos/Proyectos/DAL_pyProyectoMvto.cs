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
    public class DAL_pyProyectoMvto : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoMvto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="mvto">solicitud</param>
        public int DAL_pyProyectoMvto_Add(DTO_pyProyectoMvto mvto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyProyectoMvto  " +
                                              "    ([NumeroDoc] " +
                                              "    ,[ConsecDeta] " +
                                              "    ,[EmpresaID] " +
                                              "    ,[TipoMvto] " +
                                              "    ,[NumProyecto] " +
                                              "    ,[CostoLocal] " +
                                              "    ,[CostoExtra] " +
                                              "    ,[FactorID] " +
                                              "    ,[CantidadTOT] " +
                                              "    ,[CantidadSOL] " +
                                              "    ,[CantidadPROV] " +
                                              "    ,[CantidadNOM] " +
                                              "    ,[CantidadACT] " +
                                              "    ,[CantidadINV] " +
                                              "    ,[CantidadBOD] " +
                                              "    ,[CantidadREC] " +                                             
                                              "    ,[CostoLocalTOT] " +
                                              "    ,[CostoExtraTOT] " +
                                              "    ,[AjCambioLocal] " +
                                              "    ,[AjCambioextra] " +
                                              "    ,[VPN_ML] " +
                                              "    ,[VPN_ME] " +
                                              "    ,[FechaInicioTarea] " +
                                              "    ,[FechaOrdCompra] " +
                                              "    ,[FechaRecibido] " +
                                              "    ,[FechaPago] " +
                                              "    ,[CodigoBSID] " +
                                              "    ,[LineaPresupuestoID] " +
                                              "    ,[NumDocPresupuesto] " +
                                              "    ,[Observaciones] " +
                                              "    ,[Version] " +
                                              "    ,[eg_prBienServicio] " +
                                              "    ,[eg_plLineaPresupuesto]) " +
                                         "   VALUES " +
                                              "    (@NumeroDoc " +
                                              "    ,@ConsecDeta " +
                                              "    ,@EmpresaID " +
                                              "    ,@TipoMvto " +
                                              "    ,@NumProyecto " +
                                              "    ,@CostoLocal " +
                                              "    ,@CostoExtra " +
                                              "    ,@FactorID " +
                                              "    ,@CantidadTOT " +
                                              "    ,@CantidadSOL " +
                                              "    ,@CantidadPROV " +
                                              "    ,@CantidadNOM " +
                                              "    ,@CantidadACT " +
                                              "    ,@CantidadINV " +
                                              "    ,@CantidadBOD " +
                                              "    ,@CantidadREC " +                                             
                                              "    ,@CostoLocalTOT " +
                                              "    ,@CostoExtraTOT " +
                                              "    ,@AjCambioLocal " +
                                              "    ,@AjCambioextra " +
                                              "    ,@VPN_ML " +
                                              "    ,@VPN_ME " +
                                              "    ,@FechaInicioTarea " +
                                              "    ,@FechaOrdCompra " +
                                              "    ,@FechaRecibido " +
                                              "    ,@FechaPago " +
                                              "    ,@CodigoBSID " +
                                              "    ,@LineaPresupuestoID " +
                                              "    ,@NumDocPresupuesto " +
                                              "    ,@Observaciones  "+
                                              "    ,@Version  " +
                                              "    ,@eg_prBienServicio " +
                                              "    ,@eg_plLineaPresupuesto ) " +
                                              "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 

                #endregion                                       
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecDeta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CostoLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadNOM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPROV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadACT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadINV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadBOD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadREC", SqlDbType.Decimal);              
                mySqlCommandSel.Parameters.Add("@CostoLocalTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AjCambioLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@AjCambioextra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VPN_ML", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@VPN_ME", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicioTarea", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaOrdCompra", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaRecibido", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaPago", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);             
                mySqlCommandSel.Parameters.Add("@NumDocPresupuesto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = mvto.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsecDeta"].Value = mvto.ConsecDeta.Value;
                mySqlCommandSel.Parameters["@EmpresaID"].Value = mvto.EmpresaID.Value;
                mySqlCommandSel.Parameters["@TipoMvto"].Value = mvto.TipoMvto.Value;
                mySqlCommandSel.Parameters["@NumProyecto"].Value = mvto.NumProyecto.Value;
                mySqlCommandSel.Parameters["@CostoLocal"].Value = mvto.CostoLocal.Value;
                mySqlCommandSel.Parameters["@CostoExtra"].Value = mvto.CostoExtra.Value;
                mySqlCommandSel.Parameters["@FactorID"].Value = mvto.FactorID.Value;
                mySqlCommandSel.Parameters["@CantidadTOT"].Value = mvto.CantidadTOT.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = mvto.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CantidadPROV"].Value = mvto.CantidadPROV.Value;
                mySqlCommandSel.Parameters["@CantidadNOM"].Value = mvto.CantidadNOM.Value;
                mySqlCommandSel.Parameters["@CantidadACT"].Value = mvto.CantidadACT.Value;
                mySqlCommandSel.Parameters["@CantidadINV"].Value = mvto.CantidadINV.Value;
                mySqlCommandSel.Parameters["@CantidadBOD"].Value = mvto.CantidadBOD.Value;
                mySqlCommandSel.Parameters["@CantidadREC"].Value = mvto.CantidadREC.Value;               
                mySqlCommandSel.Parameters["@CostoLocalTOT"].Value = mvto.CostoLocalTOT.Value;
                mySqlCommandSel.Parameters["@CostoExtraTOT"].Value = mvto.CostoExtraTOT.Value;
                mySqlCommandSel.Parameters["@AjCambioLocal"].Value = mvto.AjCambioLocal.Value;
                mySqlCommandSel.Parameters["@AjCambioextra"].Value = mvto.AjCambioExtra.Value;
                mySqlCommandSel.Parameters["@VPN_ML"].Value = mvto.VPN_ML.Value;
                mySqlCommandSel.Parameters["@VPN_ME"].Value = mvto.VPN_ME.Value;
                mySqlCommandSel.Parameters["@FechaInicioTarea"].Value = mvto.FechaInicioTarea.Value;
                mySqlCommandSel.Parameters["@FechaOrdCompra"].Value = mvto.FechaOrdCompra.Value;
                mySqlCommandSel.Parameters["@FechaRecibido"].Value = mvto.FechaRecibido.Value;
                mySqlCommandSel.Parameters["@FechaPago"].Value = mvto.FechaPago.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = mvto.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = mvto.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = mvto.Observaciones.Value;             
                mySqlCommandSel.Parameters["@NumDocPresupuesto"].Value = mvto.NumDocPresupuesto.Value;
                mySqlCommandSel.Parameters["@Version"].Value = mvto.Version.Value;
                mySqlCommandSel.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);

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
                mvto.Consecutivo.Value = consecutivo;
                return consecutivo;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_Add");
                throw exception;
                return 0;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="mvto">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoMvto_Upd(DTO_pyProyectoMvto mvto)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyProyectoMvto SET					" +
                                                "   TipoMvto			=	@TipoMvto				" +
                                                "  ,NumProyecto		=	@NumProyecto			" +
                                                "  ,CostoLocal		=	@CostoLocal			" +
                                                "  ,CostoExtra		=	@CostoExtra			" +
                                                "  ,CantidadTOT		=	@CantidadTOT			" +
                                                "  ,CantidadSOL		=	@CantidadSOL			" +
                                                "  ,CantidadPROV	=	@CantidadPROV			" +
                                                "  ,CantidadNOM		=	@CantidadNOM			" +
                                                "  ,CantidadACT		=	@CantidadACT			" +
                                                "  ,CantidadINV		=	@CantidadINV			" +
                                                "  ,CantidadBOD		=	@CantidadBOD			" +
                                                "  ,CantidadREC		=	@CantidadREC			" +
                                                "  ,FactorID		=	@FactorID			" +
                                                "  ,CostoLocalTOT	=	@CostoLocalTOT		" +
                                                "  ,CostoExtraTOT	=	@CostoExtraTOT	" +
                                                "  ,FechaInicioTarea		=	@FechaInicioTarea				" +
                                                "  ,FechaOrdCompra	=	@FechaOrdCompra		" +
                                                "  ,FechaRecibido	=	@FechaRecibido			" +
                                                "  ,FechaPago	    =	@FechaPago	" +
                                                "  ,Observaciones		=	@Observaciones			" +
                                                "  ,CodigoBSID		=	@CodigoBSID			" +
                                                "  ,LineaPresupuestoID		=	@LineaPresupuestoID			" +
                                                "  ,NumDocPresupuesto		=	@NumDocPresupuesto			" +
                                                "  ,Version		=	@Version			" +
                                                "   WHERE  Consecutivo	=	@Consecutivo				";

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);
                mySqlCommandSel.Parameters.Add("@NumProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CostoLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadSOL", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadPROV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadNOM", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadACT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadINV", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadBOD", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadREC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FactorID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicioTarea", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaOrdCompra", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaRecibido", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@FechaPago", SqlDbType.DateTime);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommandSel.Parameters.Add("@LineaPresupuestoID", SqlDbType.Char, UDT_LineaPresupuestoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumDocPresupuesto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@Version", SqlDbType.TinyInt);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = mvto.Consecutivo.Value;
                mySqlCommandSel.Parameters["@NumProyecto"].Value = mvto.NumProyecto.Value;
                mySqlCommandSel.Parameters["@TipoMvto"].Value = mvto.TipoMvto.Value;
                mySqlCommandSel.Parameters["@CostoLocal"].Value = mvto.CostoLocal.Value;
                mySqlCommandSel.Parameters["@CostoExtra"].Value = mvto.CostoExtra.Value;
                mySqlCommandSel.Parameters["@CantidadTOT"].Value = mvto.CantidadTOT.Value;
                mySqlCommandSel.Parameters["@CantidadSOL"].Value = mvto.CantidadSOL.Value;
                mySqlCommandSel.Parameters["@CantidadPROV"].Value = mvto.CantidadPROV.Value;
                mySqlCommandSel.Parameters["@CantidadNOM"].Value = mvto.CantidadNOM.Value;
                mySqlCommandSel.Parameters["@CantidadACT"].Value = mvto.CantidadACT.Value;
                mySqlCommandSel.Parameters["@CantidadINV"].Value = mvto.CantidadINV.Value;
                mySqlCommandSel.Parameters["@CantidadBOD"].Value = mvto.CantidadBOD.Value;
                mySqlCommandSel.Parameters["@CantidadREC"].Value = mvto.CantidadREC.Value;
                mySqlCommandSel.Parameters["@FactorID"].Value = mvto.FactorID.Value;
                mySqlCommandSel.Parameters["@CostoLocalTOT"].Value = mvto.CostoLocalTOT.Value;
                mySqlCommandSel.Parameters["@CostoExtraTOT"].Value = mvto.CostoExtraTOT.Value;
                mySqlCommandSel.Parameters["@FechaInicioTarea"].Value = mvto.FechaInicioTarea.Value;
                mySqlCommandSel.Parameters["@FechaOrdCompra"].Value = mvto.FechaOrdCompra.Value;
                mySqlCommandSel.Parameters["@FechaRecibido"].Value = mvto.FechaRecibido.Value;
                mySqlCommandSel.Parameters["@FechaPago"].Value = mvto.FechaPago.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = mvto.Observaciones.Value;
                mySqlCommandSel.Parameters["@CodigoBSID"].Value = mvto.CodigoBSID.Value;
                mySqlCommandSel.Parameters["@LineaPresupuestoID"].Value = mvto.LineaPresupuestoID.Value;
                mySqlCommandSel.Parameters["@NumDocPresupuesto"].Value = mvto.NumDocPresupuesto.Value;
                mySqlCommandSel.Parameters["@Version"].Value = mvto.Version.Value;
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
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_Upd");
                throw exception;
            }
        }        

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoMvto_Delete(int consecutivo, int? numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoMvto " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_pyProyectoMvto> DAL_pyProyectoMvto_GetByParameter(DTO_pyProyectoMvto filter)
        {
            try
            {
                List<DTO_pyProyectoMvto> result = new List<DTO_pyProyectoMvto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = "select mvto.*, deta.RecursoID,deta.FactorID,tarea.TareaCliente,tarea.Descriptivo as TareaDesc,tarea.TareaID,tarea.Cantidad as CantidadTarea,tarea.Consecutivo as ConsecTarea, " +
                        "       rec.RecursoID,rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,rec.inReferenciaID,rec.CodigoBSID  " +
                        "from pyProyectoMvto mvto with(nolock)  " +
                        "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        "where mvto.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and mvto.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.Consecutivo.Value.ToString()))
                {
                    query += "and mvto.Consecutivo = @Consecutivo ";
                    mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                    mySqlCommand.Parameters["@Consecutivo"].Value = filter.Consecutivo.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConsecDeta.Value.ToString()))
                {
                    query += "and mvto.ConsecDeta = @ConsecDeta ";
                    mySqlCommand.Parameters.Add("@ConsecDeta", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsecDeta"].Value = filter.ConsecDeta.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.TipoMvto.Value.ToString()))
                {
                    query += "and mvto.TipoMvto = @TipoMvto ";
                    mySqlCommand.Parameters.Add("@TipoMvto", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@TipoMvto"].Value = filter.TipoMvto.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.NumProyecto.Value.ToString()))
                {
                    query += "and mvto.NumProyecto = @NumProyecto ";
                    mySqlCommand.Parameters.Add("@NumProyecto", SqlDbType.TinyInt);
                    mySqlCommand.Parameters["@NumProyecto"].Value = filter.NumProyecto.Value;
                    filterInd = true;
                }         
                mySqlCommand.CommandText = query;

                if (!filterInd)
                    return result;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_pyProyectoMvto ctrl = new DTO_pyProyectoMvto(dr);
                    ctrl.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                    result.Add(ctrl);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_pyProyectoMvto> DAL_pyProyectoMvto_GetPendientesForAprrove(DateTime fechaTope)
        {
            try
            {
                List<DTO_pyProyectoMvto> result = new List<DTO_pyProyectoMvto>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query = "select mvto.*, Cast(RTrim(ctrl.PrefijoID)+'-'+Convert(Varchar, ctrl.DocumentoNro) as Varchar(100)) as PrefDoc,ctrl.ProyectoID,  " +
                        "  rec.RecursoID,rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,rec.inReferenciaID,rec.CodigoBSID,bs.Descriptivo as CodigoBSDesc,deta.FactorID, " +
                        "  tarea.TareaCliente,tarea.Descriptivo as TareaDesc,tarea.TareaID,tarea.Cantidad as CantidadTarea,refer.MarcaInvID, refer.RefProveedor  " +
                        " from pyProyectoMvto mvto with(nolock)  " +
                        "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        "inner join pyProyectoDocu docu with(nolock) on docu.NumeroDoc = tarea.NumeroDoc " +
                        "inner join glDocumentoControl ctrl with(nolock) on ctrl.NumeroDoc = docu.NumeroDoc " +
                        "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        "left join inReferencia refer with(nolock) on refer.inReferenciaID = rec.inReferenciaID and  refer.EmpresaGrupoID = rec.eg_inReferencia " +
                        "left join prBienServicio bs with(nolock) on bs.CodigoBSID = mvto.CodigoBSID and  bs.EmpresaGrupoID = mvto.eg_prBienServicio " +
                        "where mvto.EmpresaID = @EmpresaID and mvto.FechaOrdCompra <= @FechaOrdCompra " +
                        "      and mvto.CantidadSOL > 0 ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@FechaOrdCompra", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@FechaOrdCompra"].Value = fechaTope.Date;
              
                mySqlCommand.CommandText = query;

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto(dr);                              
                    mvto.PrefDoc = dr["PrefDoc"].ToString();
                    mvto.CodigoBSDesc.Value = dr["CodigoBSDesc"].ToString();
                    mvto.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                    mvto.RefProveedor.Value = dr["RefProveedor"].ToString();
                    mvto.ProyectoID.Value = dr["ProyectoID"].ToString();
                    result.Add(mvto);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_GetByParameter");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoMvto DAL_pyProyectoMvto_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoMvto result = new DTO_pyProyectoMvto();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select mvto.*, deta.RecursoID,deta.FactorID,tarea.TareaCliente,tarea.Descriptivo as TareaDesc,tarea.TareaID,tarea.Cantidad as CantidadTarea,tarea.Consecutivo as ConsecTarea, " +
                        "       rec.RecursoID,rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,rec.inReferenciaID,rec.CodigoBSID  " +
                        "from pyProyectoMvto mvto with(nolock)  " +
                        "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        "where mvto.EmpresaID = @EmpresaID and mvto.Consecutivo =  @Consecutivo ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;               
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
                    result = new DTO_pyProyectoMvto(dr);
                    result.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el consolidado de cantidades y valores de Proyectos
        /// </summary>
        /// <returns></returns>
        public List<DTO_QueryTrazabilidad> DAL_QueryTrazabilidadProy(int? numeroDoc)
        {
            try
            {
                List<DTO_QueryTrazabilidad> result = new List<DTO_QueryTrazabilidad>();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();                

                #region CommandText

                mySqlCommandSel = new SqlCommand("ConsultaTrazabilidadProyecto", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                mySqlCommandSel.CommandType = CommandType.StoredProcedure;

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);          
                #endregion
                #region Asignacion Valores Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
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

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    int consecTarea = Convert.ToInt32(dr["ConsecTarea"]);
                    bool nuevo = true;
                    DTO_QueryTrazabilidad sobreEjec = null;
                    List<DTO_QueryTrazabilidad> list = result.Where(x => ((DTO_QueryTrazabilidad)x).ConsecTarea.Value.Value.Equals(consecTarea)).ToList();
                    if (list.Count > 0)
                    {
                        sobreEjec = list.First();
                        nuevo = false;
                    }
                    else
                        sobreEjec = new DTO_QueryTrazabilidad(dr);

                    DTO_QueryTrazabilidad sobreEjecDet = new DTO_QueryTrazabilidad(dr);
                    sobreEjec.Detalle.Add(sobreEjecDet);

                    if (nuevo)
                        result.Add(sobreEjec);
                }
                dr.Close();          

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_QueryTrazabilidadProy");
                throw exception;
            }
        }

        /// <summary>
        /// Obtiene el consolidado de documentos relacionados al proyecto
        /// </summary>
        /// <param name="consProyMvto">identificador del mvto de proyecto</param>
        /// <returns>Documentos</returns>
        public List<DTO_glDocumentoControl> DAL_pyProyectoMvto_GetDocsAnexo(int? consProyMvto)
        {
            try
            {
                List<DTO_glDocumentoControl> result = new List<DTO_glDocumentoControl>();
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;
                #region CommandText

                mySqlCommandSel.CommandText =
                "SELECT ctrlProy.*, Cast(RTrim(ctrlProy.PrefijoID)+'-'+Convert(Varchar, ctrlProy.DocumentoNro)  as Varchar(100)) as PrefDoc, " +
                "       IsNull(DocAnexo.Valor,0) as Valor, IsNull(DocAnexo.Cantidad,0) as Cantidad,DocAnexo.ConsecutivoDetaID " +
                "FROM glDocumentoControl ctrlProy with(nolock)   " +
                "    inner join glDocumento doc with(nolock) on doc.DocumentoID = ctrlProy.DocumentoID  " +
                "    inner join pyProyectoMvto mvto with(nolock) on mvto.Consecutivo = @ConsProyMvto  " +
                "    inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta 		 " +
                "    inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea  " +
                "    inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                "    inner join (   select d.ValorTotML as Valor,d.CantidadSol as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto  and d.ConsecutivoDetaID = d.SolicitudDetaID	/**Solicitado**/			 " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadOC as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                    from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.ConsecutivoDetaID =  d.OrdCompraDetaID  /**Comprado**/ " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadRec as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.ConsecutivoDetaID =  d.RecibidoDetaID /**Recibido**/ " +
                "                union " +
                "                   select m.Valor1LOC as Valor,m.CantidadUNI as Cantidad,m.NumeroDoc,m.Consecutivo " +
                "                   from glMovimientoDeta m where m.DocSoporte = @ConsProyMvto and m.EntradaSalida = 1   /**Inventarios**/ " +
                "                union " +
                "                   select m.Valor1LOC as Valor,m.CantidadUNI as Cantidad,m.NumeroDoc,m.Consecutivo " +
                "                   from glMovimientoDeta m where m.DocSoporte = @ConsProyMvto and m.EntradaSalida = 2   /**Consumido**/ " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadRec as Cantidad,d.FacturaDocuID,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.ConsecutivoDetaID =  d.RecibidoDetaID and d.FacturaDocuID is not null /**Facturado**/ " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadSol as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.DatoAdd3 = 'Cerrado' and d.CantidadOC is null and d.CantidadRec is null /**Cerrados**/ " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadOC as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.DatoAdd3 = 'Cerrado' and d.CantidadSol is null and d.CantidadRec is null /**Cerrados**/ " +
                "                Union " +
                "                   select d.ValorTotML as Valor,d.CantidadRec as Cantidad,d.NumeroDoc,d.ConsecutivoDetaID " +
                "                   from prDetalleDocu d where d.Detalle4ID = @ConsProyMvto and d.DatoAdd3 = 'Cerrado' and d.CantidadSol is null and d.CantidadOC is null) /**Cerrados**/ " +

                "    as DocAnexo ON DocAnexo.NumeroDoc = ctrlProy.NumeroDoc  " +
                "WHERE mvto.EmpresaID = @EmpresaID  " +
                "Order by ctrlProy.NumeroDoc";

                #endregion
                #region Parametros
                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ConsProyMvto", SqlDbType.Int);
                #endregion
                #region Asignacion Valores Parametros
                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@ConsProyMvto"].Value = consProyMvto;
                #endregion

                foreach (SqlParameter param in mySqlCommandSel.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                SqlDataReader dr = mySqlCommandSel.ExecuteReader();
                while (dr.Read())
                {
                    DTO_glDocumentoControl doc = new DTO_glDocumentoControl(dr);
                    doc.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                    result.Add(doc);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoMvto_GetDocsAnexo");
                throw exception;
            }
        }
        
    }
}
