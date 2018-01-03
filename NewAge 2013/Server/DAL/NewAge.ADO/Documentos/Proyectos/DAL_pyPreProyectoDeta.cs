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
    public class DAL_pyPreProyectoDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyPreProyectoDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="deta">solicitud</param>
        public void DAL_pyPreProyectoDeta_Add(DTO_pyPreProyectoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyPreProyectoDeta " +
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
                mySqlCommandSel.Parameters.Add("@FechaInicioAUT", SqlDbType.SmallDateTime);
                mySqlCommandSel.Parameters.Add("@FechaFijadaInd", SqlDbType.Bit);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char,UDT_DescripTExt.MaxLength);
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
                deta.Consecutivo.Value = Convert.ToInt32(mySqlCommandSel.Parameters["@Consecutivo"].Value);
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyPreProyectoDeta_Delete(int conseTarea,int? numeroDoc)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyPreProyectoDeta " +
                                            "  where ConsecTarea = @ConsecTarea";

                mySqlCommandSel.Parameters.Add("@ConsecTarea", SqlDbType.Int);
                mySqlCommandSel.Parameters["@ConsecTarea"].Value = conseTarea;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Borra informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyPreProyectoDeta DAL_pyPreProyectoDeta_DeleteByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyPreProyectoDeta result = new DTO_pyPreProyectoDeta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "delete from pyPreProyectoDeta " +
                                           "Where Consecutivo =  @Consecutivo ";

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                if (dr.Read())
                {
                    result = new DTO_pyPreProyectoDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_DeleteByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="deta">documento</param>
        /// <returns></returns>
        public void DAL_pyPreProyectoDeta_Upd(DTO_pyPreProyectoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyPreProyectoDeta SET					" +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_Upd");
                throw exception;
            }
        }        

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyPreProyectoDeta> DAL_pyPreProyectoDeta_GetByTarea(string tareaID, string claseproyectoID,int? numeroDoc, bool detaExist, bool recursoTimeInd)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_pyPreProyectoDeta> result = new List<DTO_pyPreProyectoDeta>();
                string where = string.Empty;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                if(!string.IsNullOrEmpty(tareaID))
                {
                    where = " and tareaRec.TareaID = @TareaID ";
                    mySqlCommand.Parameters.Add("@TareaID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@TareaID"].Value = tareaID;
                }
                if (!string.IsNullOrEmpty(claseproyectoID))
                {
                    where += " and tareaClase.ClaseServicioID = @ClaseServicioID ";
                    mySqlCommand.Parameters.Add("@ClaseServicioID", SqlDbType.Char, UDT_CodigoGrl.MaxLength);
                    mySqlCommand.Parameters["@ClaseServicioID"].Value = claseproyectoID;
                }
                if (numeroDoc!= null)
                {
                    where += " and  det.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                    if (recursoTimeInd)
                    {
                        where += " and  und.TipoMedida = @TipoMedida ";
                        mySqlCommand.Parameters.Add("@TipoMedida", SqlDbType.TinyInt);
                        mySqlCommand.Parameters["@TipoMedida"].Value = TipoMedida.Tiempo;
                    }
                }  

                if (!detaExist)
                {
                    #region Commantex Proyecto Nuevo 
                    if (!string.IsNullOrEmpty(tareaID))
                    {
                        mySqlCommand.CommandText =
                            "Select rec.RecursoID, rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID, ref.RefProveedor as Modelo,marca.Descriptivo as MarcaDesc, " +
                            "      costo.CostoLocalEMP as CostoLocal,costo.CostoExtraEMP as CostoExtra,   " +
                            "      tareaRec.FactorID, tareaClase.TareaID, tarea.CapituloTareaID " +
                            "From pyTareaRecurso tareaRec with(nolock)     " +
                            "   inner join pyTarea tarea with(nolock) On tarea.TareaID = tareaRec.TareaID and tarea.EmpresaGrupoID = tareaRec.eg_pyTarea     " +
                            "   left join pyTareaClase tareaClase with(nolock) On tareaClase.TareaID = tareaRec.TareaID and tareaClase.EmpresaGrupoID = tareaRec.eg_pyTarea     " +
                            "   left join pyRecurso rec with(nolock) On rec.RecursoID = tareaRec.RecursoID and rec.EmpresaGrupoID = tareaRec.eg_pyRecurso       " +
                            "   left join pyRecursoCostoBase costo with(nolock) On rec.RecursoID = costo.RecursoID and rec.EmpresaGrupoID = costo.eg_pyRecurso   " +
                            "   left join inUnidad und with(nolock) On und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                            "   left join inReferencia ref with(nolock) On ref.inReferenciaID = rec.inReferenciaID and ref.EmpresaGrupoID = rec.eg_inReferencia     " +
                            "   left join inMarca marca with(nolock) On marca.MarcaInvID = ref.MarcaInvID and marca.EmpresaGrupoID = ref.eg_inMarca    " +
                            "   where rec.EmpresaGrupoID = @EmpresaID " + where +
                            "Order by rec.RecursoID "; 
                    }
                    else
                    {
                        mySqlCommand.CommandText =
                            "Select rec.RecursoID, rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,  " +
                            "       costo.CostoLocalEMP as CostoLocal,costo.CostoExtraEMP as CostoExtra,ref.RefProveedor as Modelo,marca.Descriptivo as MarcaDesc " +
                            "From pyRecurso rec with(nolock)  " +
                            "   inner join pyRecursoCostoBase costo with(nolock) On rec.RecursoID = costo.RecursoID and rec.EmpresaGrupoID = costo.eg_pyRecurso   " +
                            "   left join inUnidad und with(nolock) On und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                            "   left join inReferencia ref with(nolock) On ref.inReferenciaID = rec.inReferenciaID and ref.EmpresaGrupoID = rec.eg_inReferencia     " +
                            "   left join inMarca marca with(nolock) On marca.MarcaInvID = ref.MarcaInvID and marca.EmpresaGrupoID = ref.eg_inMarca    " +
                            "   where rec.EmpresaGrupoID = @EmpresaID " +
                            "Order by rec.RecursoID ";
                    }
                    #endregion

                    SqlDataReader dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_pyPreProyectoDeta dto = new DTO_pyPreProyectoDeta(dr, true);
                        dto.Modelo.Value = Convert.ToString(dr["Modelo"]);
                        dto.MarcaDesc.Value = Convert.ToString(dr["MarcaDesc"]);
                        if (!string.IsNullOrEmpty(tareaID))
                        {
                            dto.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
                            dto.TareaID.Value = Convert.ToString(dr["TareaID"]);
                            dto.TrabajoID.Value = Convert.ToString(dr["TareaID"]);                          
                        }   
                        else
                            dto.FactorID.Value = 1;

                        result.Add(dto);
                    }
                    dr.Close();                     
                }
                else
                {
                    #region CommandText Proyecto Existente
                    mySqlCommand.CommandText =
                        "Select det.*,rec.Descriptivo as RecursoDesc,rec.TipoRecurso, rec.UnidadInvID ,trab.Descriptivo as TrabajoDesc,refer.RefProveedor as Modelo,marca.Descriptivo as MarcaDesc  " +
                        "From pyPreProyectoDeta det  with(nolock)   " + 
                        "   inner join pyPreProyectoDocu docu with(nolock) on docu.NumeroDoc = det.NumeroDoc  " +
                        "   inner join pyRecurso rec with(nolock) on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso      " +
                        "   inner join inUnidad und with(nolock) on und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                        "   left join inReferencia refer with(nolock) on refer.inReferenciaID = det.RecursoID  and refer.EmpresaGrupoID = det.eg_pyRecurso      " +
                        "   left join inMarca marca with(nolock) On marca.MarcaInvID = refer.MarcaInvID and marca.EmpresaGrupoID = refer.eg_inMarca    " +
                        "   left join pyTrabajo trab with(nolock) on trab.TrabajoID = det.TrabajoID and trab.EmpresaGrupoID = det.eg_pyTrabajo     " +
                        "   where docu.EmpresaID = @EmpresaID " + where +
                        "Order by rec.RecursoID ";
                    #endregion

                    SqlDataReader dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_pyPreProyectoDeta dto = new DTO_pyPreProyectoDeta(dr);
                        dto.Modelo.Value = Convert.ToString(dr["Modelo"]);
                        dto.MarcaDesc.Value = Convert.ToString(dr["MarcaDesc"]);
                        result.Add(dto);
                    }
                    dr.Close(); 
                    
                }

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_RecursosTrabajo_GetByTarea");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public DTO_pyPreProyectoDeta DAL_pyPreProyectoDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                DTO_pyPreProyectoDeta result = new DTO_pyPreProyectoDeta();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandText = "select * from pyPreProyectoDeta  with(nolock) " +
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
                    result = new DTO_pyPreProyectoDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public bool DAL_pyPreProyectoDeta_Exist(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select count(*) from pyPreProyectoDeta with(nolock) where  Consecutivo = @Consecutivo";

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyPreProyectoDeta_Exist");
                throw exception;
            }
        }
    }        
       
}
