﻿using System;
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
    public class DAL_pyProyectoDetaCLI : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyProyectoDetaCLI(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="deta">solicitud</param>
        public int DAL_pyProyectoDetaCLI_Add(DTO_pyProyectoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyProyectoDetaCLI " +
                                              "    ([ConsecTarea] " +
                                              "    ,[TrabajoID] " +
                                              "    ,[RecursoID] " +
                                              "    ,[NumeroDoc] " +
                                              "    ,[FactorID] " +
                                              "    ,[Cantidad] " +
                                              "    ,[CantSolicitud] " +
                                              "    ,[Peso_Cantidad] " +
                                              "    ,[Distancia_Turnos] " +
                                              "    ,[DocCompra] " +
                                              "    ,[PorVariacion] " +
                                              "    ,[CostoLocalPRY] " +
                                              "    ,[CostoExtraPRY] " +
                                              "    ,[CostoLocalEMP] " +
                                              "    ,[CostoExtraEMP] " +
                                              "    ,[CostoLocal] " +
                                              "    ,[CostoExtra] " +
                                              "    ,[CantidadTOT] " +
                                              "    ,[CostoLocalTOT] " +
                                              "    ,[CostoExtraTOT] " +
                                              "    ,[TiempoTotal] " +
                                              "    ,[FechaInicio] " +
                                              "    ,[FechaFin] " +
                                              "    ,[FechaTermina] " +
                                              "    ,[FechaInicioAUT] " +
                                              "    ,[FechaFijadaInd] " +
                                              "    ,[Observaciones] " +
                                              "    ,[eg_pyTrabajo] " +
                                              "    ,[eg_pyRecurso]) " +
                                         "   VALUES " +
                                              "    (@ConsecTarea " +
                                              "    ,@TrabajoID " +
                                              "    ,@RecursoID " +
                                              "    ,@NumeroDoc " +
                                              "    ,@FactorID " +
                                              "    ,@Cantidad " +
                                              "    ,@CantSolicitud " +
                                              "    ,@Peso_Cantidad " +
                                              "    ,@Distancia_Turnos " +
                                              "    ,@DocCompra " +
                                              "    ,@PorVariacion " +
                                              "    ,@CostoLocalPRY " +
                                              "    ,@CostoExtraPRY " +
                                              "    ,@CostoLocalEMP " +
                                              "    ,@CostoExtraEMP " +
                                              "    ,@CostoLocal " +
                                              "    ,@CostoExtra " +
                                              "    ,@CantidadTOT " +
                                              "    ,@CostoLocalTOT " +
                                              "    ,@CostoExtraTOT " +
                                              "    ,@TiempoTotal " +
                                              "    ,@FechaInicio " +
                                              "    ,@FechaFin " +
                                              "    ,@FechaTermina " +
                                              "    ,@FechaInicioAUT " +
                                              "    ,@FechaFijadaInd " +
                                              "    ,@Observaciones " +
                                              "    ,@eg_pyTrabajo " +
                                              "    ,@eg_pyRecurso ) "+
                                              "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros

                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TrabajoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactorID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantSolicitud", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Peso_Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Distancia_Turnos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorVariacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalPRY", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraPRY", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalEMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraEMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TiempoTotal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaInicioAUT", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFijadaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyTrabajo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommandSel.Parameters.Add("@eg_pyRecurso", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asigna valores

                mySqlCommandSel.Parameters["@ConsecTarea"].Value = deta.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@TrabajoID"].Value = deta.TrabajoID.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = deta.RecursoID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FactorID"].Value = deta.FactorID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = deta.Cantidad.Value;
                mySqlCommandSel.Parameters["@CantSolicitud"].Value = deta.CantSolicitud.Value;
                mySqlCommandSel.Parameters["@Peso_Cantidad"].Value = deta.Peso_Cantidad.Value;
                mySqlCommandSel.Parameters["@Distancia_Turnos"].Value = deta.Distancia_Turnos.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = deta.DocCompra.Value;
                mySqlCommandSel.Parameters["@PorVariacion"].Value = deta.PorVariacion.Value;
                mySqlCommandSel.Parameters["@CostoLocalPRY"].Value = deta.CostoLocalPRY.Value;
                mySqlCommandSel.Parameters["@CostoExtraPRY"].Value = deta.CostoExtraPRY.Value;
                mySqlCommandSel.Parameters["@CostoLocalEMP"].Value = deta.CostoLocalEMP.Value;
                mySqlCommandSel.Parameters["@CostoExtraEMP"].Value = deta.CostoExtraEMP.Value;
                mySqlCommandSel.Parameters["@CostoLocal"].Value = deta.CostoLocal.Value;
                mySqlCommandSel.Parameters["@CostoExtra"].Value = deta.CostoExtra.Value;
                mySqlCommandSel.Parameters["@CantidadTOT"].Value = deta.CantidadTOT.Value;
                mySqlCommandSel.Parameters["@CostoLocalTOT"].Value = deta.CostoLocalTOT.Value;
                mySqlCommandSel.Parameters["@CostoExtraTOT"].Value = deta.CostoExtraTOT.Value;
                mySqlCommandSel.Parameters["@TiempoTotal"].Value = deta.TiempoTotal.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = deta.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = deta.FechaFin.Value;
                mySqlCommandSel.Parameters["@FechaTermina"].Value = deta.FechaTermina.Value;
                mySqlCommandSel.Parameters["@FechaInicioAUT"].Value = deta.FechaInicioAUT.Value;
                mySqlCommandSel.Parameters["@FechaFijadaInd"].Value = deta.FechaFijadaInd.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = deta.Observaciones.Value;
                mySqlCommandSel.Parameters["@eg_pyTrabajo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyTrabajo, this.Empresa, egCtrl);
                mySqlCommandSel.Parameters["@eg_pyRecurso"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.pyRecurso, this.Empresa, egCtrl);
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
                deta.Consecutivo.Value = consecutivo;
                return consecutivo;
            }
            catch (Exception ex)
            {
                //Log error                
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_Add");
                return 0;
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información del detalle 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoDetaCLI_Delete(int? consec)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoDetaCLI " +
                                            "  where Consecutivo = @Consecutivo";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Value = consec;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información del detalle 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyProyectoDetaCLI_DeleteByConsecTarea(int? consecTarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyProyectoDetaCLI " +
                                            "  where ConsecTarea = @ConsecTarea";

                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = consecTarea;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="deta">documento</param>
        /// <returns></returns>
        public void DAL_pyProyectoDetaCLI_Upd(DTO_pyProyectoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyProyectoDetaCLI SET					" +
                                                "   ConsecTarea			=	@ConsecTarea				" +
                                                "  ,TrabajoID		=	@TrabajoID			" +
                                                "  ,RecursoID		=	@RecursoID			" +
                                                "  ,NumeroDoc		=	@NumeroDoc			" +
                                                "  ,FactorID		=	@FactorID			" +
                                                "  ,Cantidad		=	@Cantidad			" +
                                                "  ,CantSolicitud		=	@CantSolicitud			" +
                                                "  ,Peso_Cantidad		=	@Peso_Cantidad			" +
                                                "  ,Distancia_Turnos		=	@Distancia_Turnos			" +
                                                "  ,DocCompra		=	@DocCompra			" +
                                                "  ,PorVariacion		=	@PorVariacion			" +
                                                "  ,CostoLocalPRY		=	@CostoLocalPRY			" +
                                                "  ,CostoExtraPRY	=	@CostoExtraPRY		" +
                                                "  ,CostoLocalEMP	=	@CostoLocalEMP	" +
                                                "  ,CostoExtraEMP		=	@CostoExtraEMP				" +
                                                "  ,CostoLocal	=	@CostoLocal		" +
                                                "  ,CostoExtra	=	@CostoExtra			" +
                                                "  ,CantidadTOT	    =	@CantidadTOT	" +
                                                "  ,CostoLocalTOT	=	@CostoLocalTOT			" +
                                                "  ,CostoExtraTOT	=	@CostoExtraTOT			" +
                                                "  ,TiempoTotal		=	@TiempoTotal			" +
                                                "  ,FechaInicio		=	@FechaInicio			" +
                                                "  ,FechaFin		=	@FechaFin			" +
                                                "  ,FechaTermina		=	@FechaTermina			" +
                                                "  ,FechaInicioAUT		=	@FechaInicioAUT			" +
                                                "  ,Observaciones		=	@Observaciones			" +
                                                "   WHERE  Consecutivo	=	@Consecutivo				";

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@TrabajoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@RecursoID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@FactorID", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantSolicitud", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Peso_Cantidad", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Distancia_Turnos", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@DocCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@PorVariacion", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalPRY", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraPRY", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalEMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraEMP", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtra", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoLocalTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CostoExtraTOT", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@TiempoTotal", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@FechaInicio", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFin", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaTermina", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaInicioAUT", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFijadaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = deta.Consecutivo.Value;
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = deta.ConsecTarea.Value;
                mySqlCommandSel.Parameters["@TrabajoID"].Value = deta.TrabajoID.Value;
                mySqlCommandSel.Parameters["@RecursoID"].Value = deta.RecursoID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@FactorID"].Value = deta.FactorID.Value;
                mySqlCommandSel.Parameters["@Cantidad"].Value = deta.Cantidad.Value;
                mySqlCommandSel.Parameters["@CantSolicitud"].Value = deta.CantSolicitud.Value;
                mySqlCommandSel.Parameters["@Peso_Cantidad"].Value = deta.Peso_Cantidad.Value;
                mySqlCommandSel.Parameters["@Distancia_Turnos"].Value = deta.Distancia_Turnos.Value;
                mySqlCommandSel.Parameters["@DocCompra"].Value = deta.DocCompra.Value;
                mySqlCommandSel.Parameters["@PorVariacion"].Value = deta.PorVariacion.Value;
                mySqlCommandSel.Parameters["@CostoLocalPRY"].Value = deta.CostoLocalPRY.Value;
                mySqlCommandSel.Parameters["@CostoExtraPRY"].Value = deta.CostoExtraPRY.Value;
                mySqlCommandSel.Parameters["@CostoLocalEMP"].Value = deta.CostoLocalEMP.Value;
                mySqlCommandSel.Parameters["@CostoExtraEMP"].Value = deta.CostoExtraEMP.Value;
                mySqlCommandSel.Parameters["@CostoLocal"].Value = deta.CostoLocal.Value;
                mySqlCommandSel.Parameters["@CostoExtra"].Value = deta.CostoExtra.Value;
                mySqlCommandSel.Parameters["@CantidadTOT"].Value = deta.CantidadTOT.Value;
                mySqlCommandSel.Parameters["@CostoLocalTOT"].Value = deta.CostoLocalTOT.Value;
                mySqlCommandSel.Parameters["@CostoExtraTOT"].Value = deta.CostoExtraTOT.Value;
                mySqlCommandSel.Parameters["@TiempoTotal"].Value = deta.TiempoTotal.Value;
                mySqlCommandSel.Parameters["@FechaInicio"].Value = deta.FechaInicio.Value;
                mySqlCommandSel.Parameters["@FechaFin"].Value = deta.FechaFin.Value;
                mySqlCommandSel.Parameters["@FechaTermina"].Value = deta.FechaTermina.Value;
                mySqlCommandSel.Parameters["@FechaInicioAUT"].Value = deta.FechaInicioAUT.Value;
                mySqlCommandSel.Parameters["@FechaFijadaInd"].Value = deta.FechaFijadaInd.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = deta.Observaciones.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_Upd");
                throw exception;
            }
        }        

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyProyectoDeta> DAL_pyProyectoDeta_GetByNumeroDoc(int? numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_pyProyectoDeta> result = new List<DTO_pyProyectoDeta>();
                string where = string.Empty;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText =
                        "Select det.*,rec.Descriptivo as RecursoDesc,rec.TipoRecurso, rec.UnidadInvID ,trab.Descriptivo as TrabajoDesc  " +
                        "From pyProyectoDetaCLI det     " +
                        "   inner join pyProyectoDocu docu on docu.NumeroDoc = det.NumeroDoc  " +
                        "   inner join pyRecurso rec on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso      " +
                        "   inner join inUnidad und on und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                        "   left join pyTrabajo trab on trab.TrabajoID = det.TrabajoID and trab.EmpresaGrupoID = det.eg_pyTrabajo     " +
                        "   where docu.EmpresaID = @EmpresaID " + where +
                        "Order by rec.RecursoID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pyProyectoDeta dto = new DTO_pyProyectoDeta(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyProyectoDeta> DAL_pyProyectoDeta_GetByConsecTarea(int? consecTarea)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_pyProyectoDeta> result = new List<DTO_pyProyectoDeta>();
                string where = string.Empty;

                mySqlCommand.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecTarea"].Value = consecTarea;

                mySqlCommand.CommandText =
                      "Select det.*,rec.Descriptivo as RecursoDesc,rec.TipoRecurso, rec.UnidadInvID ,trab.Descriptivo as TrabajoDesc  " +
                      "From pyProyectoDetaCLI det     " +
                      "   inner join pyRecurso rec on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso      " +
                      "   left join inUnidad und on und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                      "   left join pyTrabajo trab on trab.TrabajoID = det.TrabajoID and trab.EmpresaGrupoID = det.eg_pyTrabajo     " +
                      "Where ConsecTarea =  @ConsecTarea " +
                      "Order by rec.RecursoID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pyProyectoDeta dto = new DTO_pyProyectoDeta(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDetaCLI_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyProyectoDeta DAL_pyProyectoDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyProyectoDeta result = new DTO_pyProyectoDeta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select * from pyProyectoDetaCLI  with(nolock) " +
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
                    result = new DTO_pyProyectoDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyProyectoDetaCLI_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyProyectoDetaCLI with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyProyectoDetaCLI_Exist");
                throw exception;
            }
        }      
    }
}
