using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_prSolicitudDocu
    /// </summary>
    public class DAL_prSolicitudDocu : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prSolicitudDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta una Solicitud segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prSolicitudDocu DAL_prSolicitudDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prSolicitudDocu with(nolock) where prSolicitudDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prSolicitudDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prSolicitudDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prSolicitudDocu 
        /// </summary>
        /// <param name="sol">Solicitud</param>
        /// <returns></returns>
        public void DAL_prSolicitudDocu_Add(DTO_prSolicitudDocu sol)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prSolicitudDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,Ano " +
                                           "    ,FechaEntrega " +
                                           "    ,LugarEntrega " +
                                           "    ,AreaAprobacion " +
                                           "    ,Prioridad " +
                                           "    ,eg_glLocFisica " +
                                           "    ,eg_glAreaFuncional " +
                                           "    ,UsuarioSolicita " +
                                           "    ,Destino) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@Ano " +
                                           "    ,@FechaEntrega " +
                                           "    ,@LugarEntrega " +
                                           "    ,@AreaAprobacion " +
                                           "    ,@Prioridad " +
                                           "    ,@eg_glLocFisica " +
                                           "    ,@eg_glAreaFuncional " +
                                           "    ,@UsuarioSolicita " +
                                           "    ,@Destino) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@Prioridad", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glAreaFuncional", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@Destino", SqlDbType.TinyInt);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = sol.NumeroDoc.Value;
                mySqlCommand.Parameters["@Ano"].Value = sol.Ano.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = sol.FechaEntrega.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = sol.LugarEntrega.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = sol.AreaAprobacion.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = sol.Prioridad.Value;
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glAreaFuncional"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFuncional, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = sol.UsuarioSolicita.Value;
                mySqlCommand.Parameters["@Destino"].Value = sol.Destino.Value;
                #endregion

                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }

                mySqlCommand.ExecuteNonQuery();
                int numDoc = Convert.ToInt32(mySqlCommand.Parameters["@NumeroDoc"].Value);

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar la solicitud en tabla prSolicitudDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">solicitud</param>
        public void DAL_prSolicitudDocu_Upd(DTO_prSolicitudDocu sol)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prSolicitudDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prSolicitudDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,Ano  = @Ano " +
                                           "    ,FechaEntrega  = @FechaEntrega " +
                                           "    ,LugarEntrega  = @LugarEntrega " +
                                           "    ,AreaAprobacion  = @AreaAprobacion " +
                                           "    ,Prioridad  = @Prioridad " +
                                           "    ,UsuarioSolicita  = @UsuarioSolicita " +                                       
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Ano", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaEntrega", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@Prioridad", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = sol.NumeroDoc.Value;
                mySqlCommand.Parameters["@Ano"].Value = sol.Ano.Value;
                mySqlCommand.Parameters["@FechaEntrega"].Value = sol.FechaEntrega.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = sol.LugarEntrega.Value;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = sol.AreaAprobacion.Value;
                mySqlCommand.Parameters["@Prioridad"].Value = sol.Prioridad.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = sol.UsuarioSolicita.Value;
                #endregion
                foreach (SqlParameter param in mySqlCommand.Parameters)
                {
                    if (param.Direction.Equals(ParameterDirection.Input))
                    {
                        if (param.Value == null || ((param.Value is string) && string.IsNullOrWhiteSpace(param.Value.ToString())))
                            param.Value = DBNull.Value;
                    }
                }                
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_Upd");
                throw exception;
            }
        }
        
        /// <summary>
        /// Trae un listado de Solicitudes pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<Object> DAL_prSolicitudDocu_GetPendientesByModulo(ModulesPrefix mod, int documentID, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<Object> result = new List<Object>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filtro = string.Empty;
                if (documentID != AppDocuments.SolicitudAsign && documentID == AppDocuments.SolicitudPreAprob)
                    filtro = " and sol.UsuarioSolicita = @UsuarioSolicita ";
                else
                    filtro = " and docAprueba.UsuarioAprueba = @UsuarioAprueba ";
                #region Query y comandos

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioSolicita", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (documentID != AppDocuments.SolicitudAsign) ? (int)EstadoDocControl.ParaAprobacion : (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@UsuarioSolicita"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = usuario.ReplicaID.Value;

                if (documentID != AppDocuments.SolicitudAsign)
                    mySqlCommand.CommandText =
                        "   select distinct ctrl.*, ctrl.PeriodoDoc as PeriodoID, ctrl.Observacion as ObservacionDoc, " +
                        "	usr.UsuarioID as UsuarioID, sol.FechaEntrega,sol.LugarEntrega,sol.AreaAprobacion,sol.UsuarioSolicita,sol.Prioridad " +
                        "from glDocumentoControl ctrl with(nolock)  " +
                        "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                        "	    and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
                        "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                        "	inner join prSolicitudDocu sol with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc  " +
                        "   inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc " +
                        "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                        "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " + filtro;
                else
                        mySqlCommand.CommandText =
                        "   select distinct ctrl.*, ctrl.PeriodoDoc as PeriodoID, ctrl.Observacion as ObservacionDoc, " +
                        "	usr.UsuarioID as UsuarioID, sol.FechaEntrega,sol.LugarEntrega,sol.AreaAprobacion,sol.UsuarioSolicita,sol.Prioridad " +
                        "from glDocumentoControl ctrl with(nolock)  " +
                        "   inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                        "	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                        "	inner join prSolicitudDocu sol with(nolock) on ctrl.NumeroDoc = sol.NumeroDoc  " +
                        "   inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc " +
                        "	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                        "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " + filtro;

                #endregion

                SqlDataReader dr;
                if (documentID != AppDocuments.SolicitudAsign)
                {
                    dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        DTO_prSolicitudAprobacion dtoAprob = new DTO_prSolicitudAprobacion(dr);
                        dtoAprob.Aprobado.Value = false;
                        dtoAprob.Rechazado.Value = false;
                        result.Add(dtoAprob);
                    }
                }
                else 
                {
                    mySqlCommand.CommandText = "select det.NumeroDoc, det.ConsecutivoDetaID, det.inReferenciaID, det.CodigoBSID, det.Descriptivo, " +
                        "det.EstadoInv, det.Parametro1, det.Parametro2, det.UnidadInvID, det.CantidadSol, det.DatoAdd1, doc.* from(" + mySqlCommand.CommandText + 
                        ") doc left join prDetalleDocu det with(nolock) on (doc.EmpresaID = det.EmpresaID and doc.NumeroDoc = det.NumeroDoc)";

                    dr = mySqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                        bool nuevo = true;
                        DTO_prSolicitudAsignacion dtoAsign = new DTO_prSolicitudAsignacion(dr);
                        List<Object> list = result.Where(x=>((DTO_prSolicitudAsignacion)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                        if (list.Count>0)
                        {
                            dtoAsign = (DTO_prSolicitudAsignacion)list.First();
                            nuevo = false;
                        }
                        else
                        {
                            dtoAsign = new DTO_prSolicitudAsignacion(dr);
                            dtoAsign.Asignado.Value = false;
                        }

                        DTO_prSolicitudAsignDet solDet = new DTO_prSolicitudAsignDet(dr);
                        if (string.IsNullOrEmpty(solDet.DatoAdd1.Value))
                        {
                            solDet.Asignado.Value = false;
                            dtoAsign.SolicitudAsignDet.Add(solDet);
                        }

                        if (nuevo)
                        {
                            //if (result.Count > 0 && ((DTO_prSolicitudAsignacion)result.Last()).SolicitudAsignDet.Count == 0)
                                //result.Remove(result.Last());
                         if (dtoAsign.SolicitudAsignDet.Count > 0)
                            result.Add(dtoAsign);
                        }
                    }
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_GetPendientesByModulo");
                throw exception;
            }
        }
        
        /// <summary>
        /// Trae un listado de Solicitudes para Orden de compra
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudResumen> DAL_prSolicitudDocu_GetResumen(int documentID, bool asignInd, DTO_seUsuario usuario, ModulesPrefix mod, TipoMoneda origenMonet, bool DestinoContrato = false)
        {
            try
            {
                List<DTO_prSolicitudResumen> result = new List<DTO_prSolicitudResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Filtros
                string filtroAssign = string.Empty;
                string filtroDestino = string.Empty;
                string filtroOrigenMon = string.Empty;
                if (asignInd)
                    filtroAssign = " and det.DatoAdd1 = @UsuarioAsignado ";
                if (DestinoContrato)
                    filtroDestino = " and docu.Destino = 1 ";
                else
                    filtroDestino = " and docu.Destino = 0 ";

                //if (origenMonet == TipoMoneda.Local)
                //    filtroOrigenMon = " and (det.OrigenMonetario = 1 or det.OrigenMonetario is null) ";
                //else if (origenMonet == TipoMoneda.Foreign)
                //    filtroOrigenMon = " and det.OrigenMonetario = 2 ";
                #endregion

                mySqlCommand.CommandText =
                "select ctrl.NumeroDoc,ctrl.PeriodoDoc as PeriodoID,ctrl.FechaDoc Fecha,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,solCargo.ProyectoID,solCargo.LineaPresupuestoID " +
                    ",det.SolicitudDocuID,det.ConsecutivoDetaID,det.CodigoBSID,det.inReferenciaID,det.LineaPresupuestoID,refer.MarcaInvID,refer.RefProveedor,empaque.UnidadInvID as UnidadEmpaque,empaque.Cantidad as CantidadEmpaque " +
                    ",det.Descriptivo,det.Parametro1,det.Parametro2,det.UnidadInvID,refer.EmpaqueInvID,temp.CantidadSum CantidadSol,det.CantidadSol CantidadSolTOT " +
                    ",det.Documento1ID,det.Documento2ID,det.Documento3ID,det.Documento4ID,det.Documento5ID  " +
                    ",det.Detalle1ID,det.Detalle2ID,det.Detalle3ID,det.Detalle4ID,det.Detalle5ID  " +
                    ",det.CantidadDoc1,det.CantidadDoc2,det.CantidadDoc3,det.CantidadDoc4,det.CantidadDoc5  " +
                    ",det.DatoAdd1,det.DatoAdd2,det.DatoAdd3,det.DatoAdd4,det.DatoAdd5,det.OrigenMonetario  " +
                "from (select det.SolicitudDocuID,det.SolicitudDetaID,SUM(det.CantidadSol) CantidadSum " +
                "from prDetalleDocu det with(nolock) " +
                    "inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc " +
                    "inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                "where ctrl.EmpresaID = @EmpresaID and (doc.ModuloID = 'pr' or doc.ModuloID = 'in') " +
                    "and ( ctrl.DocumentoID = @DocumentoIDSol and ctrl.Estado = @Estado " + filtroAssign + " or ctrl.DocumentoID = @DocumentoIDOC  or ctrl.DocumentoID = @DocumentoIDCierre or InventarioDocuID is not null) " +
                "group by det.SolicitudDocuID,det.SolicitudDetaID ) temp " +
                    "inner join glDocumentoControl ctrl with(nolock) on temp.SolicitudDocuID = ctrl.NumeroDoc " +
                    "inner join prDetalleDocu det with(nolock) on temp.SolicitudDetaID = det.ConsecutivoDetaID " +
                    "inner join prSolicitudDocu docu with(nolock) on temp.SolicitudDocuID = docu.NumeroDoc " +
                    "inner join prSolicitudCargos solCargo with(nolock) on det.ConsecutivoDetaID = solCargo.ConsecutivoDetaID " +
                    "left join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID and refer.EmpresaGrupoID = det.eg_inReferencia " +
                    "left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = refer.EmpaqueInvID and empaque.EmpresaGrupoID = refer.eg_inEmpaque " +
                "where temp.CantidadSum!=0 and temp.CantidadSum > 0   " + filtroDestino + filtroOrigenMon;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioAsignado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoIDSol", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoIDOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoIDCierre", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@UsuarioAsignado"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@DocumentoIDSol"].Value = AppDocuments.Solicitud;
                mySqlCommand.Parameters["@DocumentoIDOC"].Value = AppDocuments.OrdenCompra;
                mySqlCommand.Parameters["@DocumentoIDCierre"].Value = AppDocuments.CierreDetalleSolicitud;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    DTO_prSolicitudResumen dtoRes = new DTO_prSolicitudResumen(dr);
                    dtoRes.Selected.Value = false;
                    result.Add(dtoRes);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDocu_GetResumen");
                throw exception;
            }
        }
        
        #endregion
    }
}
