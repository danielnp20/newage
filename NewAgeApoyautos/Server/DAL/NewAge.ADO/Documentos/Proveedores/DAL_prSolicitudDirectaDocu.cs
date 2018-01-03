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
    /// DAL_prSolicitudDirectaDocu
    /// </summary>
    public class DAL_prSolicitudDirectaDocu : DAL_Base
    {
       /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prSolicitudDirectaDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta una Solicitud segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prSolicitudDirectaDocu DAL_prSolicitudDirectaDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prSolicitudDirectaDocu with(nolock) where prSolicitudDirectaDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prSolicitudDirectaDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prSolicitudDirectaDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDirectaDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prSolicitudDirectaDocu 
        /// </summary>
        /// <param name="sol">Solicitud</param>
        /// <returns></returns>
        public void DAL_prSolicitudDirectaDocu_Add(DTO_prSolicitudDirectaDocu sol)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prSolicitudDirectaDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,ProveedorID " +
                                           "    ,eg_prProveedor) " +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                            "   ,@ProveedorID " +
                                           "    ,@eg_prProveedor) ";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = sol.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = sol.ProveedorID.Value;
                mySqlCommand.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDirectaDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar la solicitud en tabla prSolicitudDirectaDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">solicitud</param>
        public void DAL_prSolicitudDirectaDocu_Upd(DTO_prSolicitudDirectaDocu sol)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prSolicitudDirectaDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prSolicitudDirectaDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,ProveedorID  = @ProveedorID " +                                    
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = sol.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = sol.ProveedorID.Value;

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDirectaDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudDirectaAprob> DAL_prSolicitudDirectaDocu_GetPendientesByModulo(DTO_glDocumento doc, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_prSolicitudDirectaAprob> result = new List<DTO_prSolicitudDirectaAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                //mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioAprueba", SqlDbType.Char, UDT_UsuarioID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = doc.ModuloID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                //mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                mySqlCommand.Parameters["@UsuarioAprueba"].Value = usuario.ReplicaID.Value;
                #endregion

                mySqlCommand.CommandText =
                    "   select temp.*, " +
                    "    det.ConsecutivoDetaID,det.CodigoBSID,det.Descriptivo,det.inReferenciaID,det.UnidadInvID,det.CantidadDoc5, " +
                    "    det.Documento5ID,det.Detalle5ID,det.ValorUni,det.ValorTotML,det.IvaTotML,det.ValorTotME,det.IvaTotME  " +
                    "    from ( select distinct ctrl.EmpresaID,ctrl.PeriodoDoc,ctrl.MonedaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha,ctrl.Observacion Justificacion, " +
                    "        pr.ProveedorID,pr.Descriptivo ProveedorNombre,docAprueba.UsuarioAprueba,usr.UsuarioID, " +
                    "        SUM(det.ValorTotML) TotalML,SUM(det.ValorTotME) TotalME,SUM(det.IvaTotML) IvaML,SUM(det.IvaTotME) IvaME  " +
                    "    from glDocumentoControl ctrl with(nolock) " +
                    "        inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc  " +
                    "               and act.CerradoInd=0  and act.ActividadFlujoID= @ActividadFlujoID  " +
                    "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "        inner join prSolicitudDirectaDocu sd with(nolock) on ctrl.NumeroDoc = sd.NumeroDoc  " +
                    "        inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "        inner join prProveedor pr with(nolock) on sd.ProveedorID = pr.ProveedorID " +
                    "        inner join prDetalleDocu det with(nolock) on sd.NumeroDoc = det.NumeroDoc " +
                    "        inner join glDocumentoAprueba docAprueba with(nolock) on docAprueba.NumeroDoc = ctrl.NumeroDoc " +
                    //"        inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    //"               and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " +
                    //"    and perm.ActividadFlujoID = @ActividadFlujoID " +
                    "    and docAprueba.UsuarioAprueba = @UsuarioAprueba" +
                    "    group by ctrl.EmpresaID,ctrl.PeriodoDoc,ctrl.MonedaID,ctrl.NumeroDoc,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc,ctrl.Observacion, " +
                    "        pr.ProveedorID,pr.Descriptivo,docAprueba.UsuarioAprueba,usr.UsuarioID ) temp  " +
                    "        inner join prDetalleDocu det with(nolock) on temp.EmpresaID = det.EmpresaID and temp.NumeroDoc = det.NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_prSolicitudDirectaAprob dto = new DTO_prSolicitudDirectaAprob(dr);
                    List<DTO_prSolicitudDirectaAprob> list = result.Where(x => ((DTO_prSolicitudDirectaAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_prSolicitudDirectaAprob(dr);
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                    }

                    DTO_prSolDirectaAprobDet dtoDet = new DTO_prSolDirectaAprobDet(dr);
                    dto.SolicitudDirectaAprobDet.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDirectaDocu_GetPendientesByModulo");
                throw exception;
            }
        }        
       
        /// <summary>
        /// Trae un listado de Solicitudes para Orden de compra
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prSolicitudResumen> DAL_prSolicitudDirectaDocu_GetResumen(int documentID, bool asignInd, DTO_seUsuario usuario, ModulesPrefix mod,bool DestinoContrato = false)
        {
            try
            {
                List<DTO_prSolicitudResumen> result = new List<DTO_prSolicitudResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Filtros
                string filtroAssign = string.Empty;
                string filtroDestino = string.Empty;
                if (asignInd)
                    filtroAssign = " and det.DatoAdd1 = @UsuarioAsignado ";
                if (DestinoContrato)
                    filtroDestino = " and docu.Destino = 1";
                else
                    filtroDestino = " and docu.Destino = 0"; 
                #endregion

                mySqlCommand.CommandText =
                "select ctrl.NumeroDoc,ctrl.PeriodoDoc as PeriodoID,ctrl.FechaDoc Fecha,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,solCargo.ProyectoID,solCargo.LineaPresupuestoID " +
                    ",det.SolicitudDocuID,det.ConsecutivoDetaID,det.CodigoBSID,det.inReferenciaID,det.LineaPresupuestoID,refer.MarcaInvID,refer.RefProveedor,empaque.UnidadInvID as UnidadEmpaque,empaque.Cantidad as CantidadEmpaque " +
                    ",det.Descriptivo,det.Parametro1,det.Parametro2,det.UnidadInvID,refer.EmpaqueInvID,temp.CantidadSum CantidadSol " +
                    ",det.Documento1ID,det.Documento2ID,det.Documento3ID,det.Documento4ID,det.Documento5ID  " +
                    ",det.Detalle1ID,det.Detalle2ID,det.Detalle3ID,det.Detalle4ID,det.Detalle5ID  " +
                    ",det.CantidadDoc1,det.CantidadDoc2,det.CantidadDoc3,det.CantidadDoc4,det.CantidadDoc5  " +
                    ",det.DatoAdd1,det.DatoAdd2,det.DatoAdd3,det.DatoAdd4,det.DatoAdd5,det.OrigenMonetario  " +
                "from (select det.SolicitudDocuID,det.SolicitudDetaID,SUM(det.CantidadSol) CantidadSum " +
                "from prDetalleDocu det with(nolock) " +
                    "inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc " +
                    "inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                "where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID " +
                    "and ( ctrl.DocumentoID = @DocumentoIDSol and ctrl.Estado = @Estado " + filtroAssign + " or ctrl.DocumentoID = @DocumentoIDOC or ctrl.DocumentoID = @DocumentoIDCierre) " +
                "group by det.SolicitudDocuID,det.SolicitudDetaID ) temp " +
                    "inner join glDocumentoControl ctrl with(nolock) on temp.SolicitudDocuID = ctrl.NumeroDoc " +
                    "inner join prDetalleDocu det with(nolock) on temp.SolicitudDetaID = det.ConsecutivoDetaID " +
                    "inner join prSolicitudCargos solCargo with(nolock) on det.ConsecutivoDetaID = solCargo.ConsecutivoDetaID " +
                    "inner join prSolicitudDirectaDocu docu with(nolock) on temp.SolicitudDocuID = docu.NumeroDoc " +
                    "left join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID and refer.EmpresaGrupoID = det.eg_inReferencia " +
                    "left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = refer.EmpaqueInvID and empaque.EmpresaGrupoID = refer.eg_inEmpaque " +
                "where temp.CantidadSum!=0 " + filtroDestino;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@UsuarioAsignado", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                mySqlCommand.Parameters.Add("@DocumentoIDSol", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoIDOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoIDCierre", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prSolicitudDirectaDocu_GetResumen");
                throw exception;
            }
        }
        
        #endregion
    }
}
