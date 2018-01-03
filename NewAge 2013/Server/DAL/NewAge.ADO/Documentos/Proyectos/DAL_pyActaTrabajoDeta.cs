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
    public class DAL_pyActaTrabajoDeta : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_pyActaTrabajoDeta(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        ///  Adicona Solicitud servicio detallado
        /// </summary>
        /// <param name="deta">solicitud</param>
        public int DAL_pyActaTrabajoDeta_Add(DTO_pyActaTrabajoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query

                mySqlCommandSel.CommandText = "   INSERT INTO pyActaTrabajoDeta " +
                                              "    ([NumeroDoc] " +
                                              "    ,[ConsOrdCompraDeta] " +
                                              "    ,[ConsProyMvto] " +
                                              "    ,[NumDocProyecto] " +
                                              "    ,[NumDocOrdCompra] " +
                                              "    ,[NumDocRecibido] " +
                                              "    ,[CantidadPend] " +
                                              "    ,[CantidadREC] " +
                                              "    ,[Observaciones] " +
                                              "    ,[MonedaOrdCompra] " +
                                              "    ,[ValorUniREC]) " +
                                         "   VALUES " +
                                              "    (@NumeroDoc " +
                                              "    ,@ConsOrdCompraDeta " +
                                              "    ,@ConsProyMvto " +
                                              "    ,@NumDocProyecto " +
                                              "    ,@NumDocOrdCompra " +
                                              "    ,@NumDocRecibido " +
                                              "    ,@CantidadPend " +
                                              "    ,@CantidadREC " +
                                              "    ,@Observaciones " +
                                              "    ,@MonedaOrdCompra" +
                                              "    ,@ValorUniREC) " +
                                              "   SET @Consecutivo = SCOPE_IDENTITY()";

                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters["@Consecutivo"].Direction = ParameterDirection.Output; 
                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsOrdCompraDeta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsProyMvto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOrdCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRecibido", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CantidadPend", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadREC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@MonedaOrdCompra", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ValorUniREC", SqlDbType.Decimal);
                #endregion
                #region Asigna valores 
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = deta.NumeroDoc.Value;
                mySqlCommandSel.Parameters["@ConsOrdCompraDeta"].Value = deta.ConsOrdCompraDeta.Value;
                mySqlCommandSel.Parameters["@ConsProyMvto"].Value = deta.ConsProyMvto.Value;
                mySqlCommandSel.Parameters["@NumDocProyecto"].Value = deta.NumDocProyecto.Value;
                mySqlCommandSel.Parameters["@NumDocOrdCompra"].Value = deta.NumDocOrdCompra.Value;
                mySqlCommandSel.Parameters["@NumDocRecibido"].Value = deta.NumDocRecibido.Value;
                mySqlCommandSel.Parameters["@CantidadPend"].Value = deta.CantidadPend.Value;
                mySqlCommandSel.Parameters["@CantidadREC"].Value = deta.CantidadREC.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = deta.Observaciones.Value;
                mySqlCommandSel.Parameters["@MonedaOrdCompra"].Value = deta.MonedaOrdCompra.Value;
                mySqlCommandSel.Parameters["@ValorUniREC"].Value = deta.ValorUniREC.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTrabajoDeta_Add");
                return 0;
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información de las tareas 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyActaTrabajoDeta_Delete(int conseTarea)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyActaTrabajoDeta " +
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTrabajoDeta_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina información por numeroDoc 
        /// </summary>
        /// <param name="consecutivo">id</param>
        /// <returns>retorna idenfiticador</returns>
        public void DAL_pyActaTrabajoDeta_DeleteByNumeroDoc(int numeroDocActa)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText = "DELETE FROM pyActaTrabajoDeta" +
                                            "  where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDocActa;
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
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTrabajoDeta_DeleteByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <returns>Lista de componentes</returns>
        public List<DTO_pyActaTrabajoDeta> DAL_pyActaTrabajoDeta_GetByNumeroDoc(int? numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                List<DTO_pyActaTrabajoDeta> result = new List<DTO_pyActaTrabajoDeta>();
                string where = string.Empty;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;

                mySqlCommand.CommandText =
                        "Select det.*,rec.Descriptivo as RecursoDesc,rec.TipoRecurso, rec.UnidadInvID ,trab.Descriptivo as TrabajoDesc  " +
                        "From pyActaTrabajoDeta det     " +
                        "   inner join pyProyectoDocu docu on docu.NumeroDoc = det.NumeroDoc  " +
                        "   inner join pyRecurso rec on rec.RecursoID = det.RecursoID  and rec.EmpresaGrupoID = det.eg_pyRecurso      " +
                        "   inner join inUnidad und on und.UnidadInvID = rec.UnidadInvID and und.EmpresaGrupoID = rec.eg_inUnidad     " +
                        "   left join pyTrabajo trab on trab.TrabajoID = det.TrabajoID and trab.EmpresaGrupoID = det.eg_pyTrabajo     " +
                        "   where docu.EmpresaID = @EmpresaID " + where +
                        "Order by rec.RecursoID ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_pyActaTrabajoDeta dto = new DTO_pyActaTrabajoDeta(dr);
                    result.Add(dto);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTrabajoDeta_GetByNumeroDoc");
                throw exception;
            }
        }

        /// <summary>
        /// Trae una lista de servicio Deta
        /// </summary>
        /// <param name="documentID">Identificador del Documento</param>
        /// <param name="TrabajoID">Identificador del prefijo</param>
        /// <returns>Lista de componentes</returns>
        public DTO_pyActaTrabajoDeta DAL_pyActaTrabajoDeta_GetByConsecutivo(int? consec)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                DTO_pyActaTrabajoDeta result = new DTO_pyActaTrabajoDeta();

                mySqlCommand.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommand.Parameters["@Consecutivo"].Value = consec;

                mySqlCommand.CommandText =  "Select acta.*, deta.RecursoID,deta.FactorID,tarea.TareaCliente,tarea.Descriptivo as TareaDesc,tarea.Cantidad as CantidadTarea, " +
                                            "       rec.RecursoID,rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,rec.inReferenciaID,rec.CodigoBSID,ordCompra.ProveedorID  " +
                                            "from pyActaTrabajoDeta acta "+
                                            "inner join pyProyectoMvto mvto with(nolock) on mvto.Consecutivo = acta.ConsProyMvto " +
                                            "inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                                            "inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                                            "inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                                            "inner join prOrdenCompraDocu ordCompra with(nolock) on ordCompra.NumeroDoc = acta.NumDocOrdCompra " +
                                            " where acta.Consecutivo = @Consecutivo ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    result = new DTO_pyActaTrabajoDeta(dr);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingList, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTrabajoDeta_GetByConsecutivo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae informacion de acuerdo al filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Dto de Detalle Docu</returns>
        public List<DTO_pyActaTrabajoDeta> DAL_pyActaTrabajoDeta_GetByParameter(DTO_pyActaTrabajoDeta filter)
        {
            try
            {
                List<DTO_pyActaTrabajoDeta> result = new List<DTO_pyActaTrabajoDeta>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string query;
                bool filterInd = false;

                query = " Select acta.*, deta.RecursoID,deta.FactorID,tarea.TareaCliente,tarea.Consecutivo as ConsecTarea,tarea.Descriptivo as TareaDesc,tarea.Cantidad as CantidadTarea, " +
                        " ctrl.DocumentoNro,ctrl.Fecha,ctrl.Estado,IsNull(ctrl.ConsSaldo,1) as ConsSaldo," +
                        " rec.RecursoID,rec.Descriptivo as RecursoDesc,rec.TipoRecurso,rec.UnidadInvID,rec.inReferenciaID,rec.CodigoBSID, " +
                        " RTrim(ctrlOC.PrefijoID)+'-'+Cast(ctrlOC.DocumentoNro as Varchar(3)) as PrefDocOC, RTrim(prov.ProveedorID) + '-' + prov.Descriptivo as ProveedorID " +
                        " from pyActaTrabajoDeta acta with(nolock)  " +
                        " inner join gldocumentocontrol ctrl with(nolock) on ctrl.NumeroDoc = acta.NumeroDoc " +
                        " inner join pyProyectoMvto mvto with(nolock) on mvto.Consecutivo = acta.ConsProyMvto " +
                        " inner join pyProyectoDeta deta with(nolock) on deta.Consecutivo = mvto.ConsecDeta " +
                        " inner join pyProyectoTarea tarea with(nolock) on tarea.Consecutivo = deta.ConsecTarea " +
                        " inner join pyRecurso rec with(nolock) on rec.RecursoID = deta.RecursoID and  rec.EmpresaGrupoID = deta.eg_pyRecurso " +
                        " inner join prOrdenCompraDocu ordCompra with(nolock) on ordCompra.NumeroDoc = acta.NumDocOrdCompra " +
                        " inner join gldocumentocontrol ctrlOC with(nolock) on ctrlOC.NumeroDoc = acta.NumDocOrdCompra " +
                         " inner join prProveedor prov with(nolock) on prov.ProveedorID = ordCompra.ProveedorID and prov.EmpresaGrupoID = ordCompra.eg_prProveedor " +
                        " where mvto.EmpresaID = @EmpresaID ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                if (!string.IsNullOrEmpty(filter.NumeroDoc.Value.ToString()))
                {
                    query += "and acta.NumeroDoc = @NumeroDoc ";
                    mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumeroDoc"].Value = filter.NumeroDoc.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.ConsProyMvto.Value.ToString()))
                {
                    query += "and acta.ConsProyMvto = @ConsProyMvto ";
                    mySqlCommand.Parameters.Add("@ConsProyMvto", SqlDbType.Int);
                    mySqlCommand.Parameters["@ConsProyMvto"].Value = filter.ConsProyMvto.Value.Value;
                    filterInd = true;
                }
                if (!string.IsNullOrEmpty(filter.NumDocProyecto.Value.ToString()))
                {
                    query += "and acta.NumDocProyecto = @NumDocProyecto ";
                    mySqlCommand.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                    mySqlCommand.Parameters["@NumDocProyecto"].Value = filter.NumDocProyecto.Value;
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
                    DTO_pyActaTrabajoDeta det = new DTO_pyActaTrabajoDeta(dr);
                    det.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                    det.Estado.Value = Convert.ToInt16(dr["Estado"]);
                    det.ConsActaProy.Value = Convert.ToInt32(dr["ConsSaldo"]);
                    det.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                    det.ProveedorID.Value = dr["ProveedorID"].ToString();
                    det.PrefDocOC = dr["PrefDocOC"].ToString();
                    result.Add(det);
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
        /// Actualiza docu Solproyecto
        /// </summary>
        /// <param name="deta">documento</param>
        /// <returns></returns>
        public void DAL_pyActaTabajoDeta_Upd(DTO_pyActaTrabajoDeta deta)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                #region Query
                mySqlCommandSel.CommandText =
                                                "   UPDATE pyActaTrabajoDeta SET					" +
                                                "   ConsOrdCompraDeta	=	@ConsOrdCompraDeta	" +
                                                "  ,ConsProyMvto		=	@ConsProyMvto			" +
                                                "  ,NumDocProyecto		=	@NumDocProyecto			" +
                                                "  ,NumDocOrdCompra		=	@NumDocOrdCompra			" +
                                                "  ,NumDocRecibido		=	@NumDocRecibido			" +
                                                "  ,CantidadPend		=	@CantidadPend			" +
                                                "  ,MonedaOrdCompra		=	@MonedaOrdCompra			" +
                                                "  ,ValorUniREC		    =	@ValorUniREC			" +
                                                "  ,Observaciones		=	@Observaciones			" +
                                                "   WHERE  Consecutivo	=	@Consecutivo				";

                #endregion
                #region Crea parametros
                mySqlCommandSel.Parameters.Add("@Consecutivo", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsOrdCompraDeta", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@ConsProyMvto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocProyecto", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocOrdCompra", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@NumDocRecibido", SqlDbType.Int);
                mySqlCommandSel.Parameters.Add("@CantidadPend", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@CantidadREC", SqlDbType.Decimal);
                mySqlCommandSel.Parameters.Add("@Observaciones", SqlDbType.Char, UDT_DescripTExt.MaxLength);
                mySqlCommandSel.Parameters.Add("@MonedaOrdCompra", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@ValorUniREC", SqlDbType.Decimal);
                #endregion
                #region Asigna valores
                mySqlCommandSel.Parameters["@Consecutivo"].Value = deta.Consecutivo.Value;
                mySqlCommandSel.Parameters["@ConsOrdCompraDeta"].Value = deta.ConsOrdCompraDeta.Value;
                mySqlCommandSel.Parameters["@ConsProyMvto"].Value = deta.ConsProyMvto.Value;
                mySqlCommandSel.Parameters["@NumDocProyecto"].Value = deta.NumDocProyecto.Value;
                mySqlCommandSel.Parameters["@NumDocOrdCompra"].Value = deta.NumDocOrdCompra.Value;
                mySqlCommandSel.Parameters["@NumDocRecibido"].Value = deta.NumDocRecibido.Value;
                mySqlCommandSel.Parameters["@CantidadPend"].Value = deta.CantidadPend.Value;
                mySqlCommandSel.Parameters["@CantidadREC"].Value = deta.CantidadREC.Value;
                mySqlCommandSel.Parameters["@Observaciones"].Value = deta.Observaciones.Value;
                mySqlCommandSel.Parameters["@MonedaOrdCompra"].Value = deta.MonedaOrdCompra.Value;
                mySqlCommandSel.Parameters["@ValorUniREC"].Value = deta.ValorUniREC.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_pyActaTabajoDeta_Upd");
                throw exception;
            }
        }        

    }
}
