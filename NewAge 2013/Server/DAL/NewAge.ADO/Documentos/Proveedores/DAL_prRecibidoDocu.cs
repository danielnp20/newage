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
using System.Data.SqlTypes;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL_prRecibidoDocu
    /// </summary>
    public class DAL_prRecibidoDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prRecibidoDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Recibido segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prRecibidoDocu DAL_prRecibidoDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prRecibidoDocu with(nolock) where prRecibidoDocu.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prRecibidoDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prRecibidoDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prRecibidoDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prRecibidoDocu 
        /// </summary>
        /// <param name="sol">Recibid</param>
        /// <returns></returns>
        public void DAL_prRecibidoDocu_Add(DTO_prRecibidoDocu orden)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prRecibidoDocu " +
                                           "    ( " +
                                           //"     EmpresaID, " +
                                           "    NumeroDoc " +
                                           "    ,ProveedorID " +                                         
                                           "    ,LugarEntrega " +
                                           "    ,BodegaID " +                                         
                                           "    ,eg_glLocFisica " +
                                           "    ,eg_Proveedor) " +
                                           "    VALUES" +
                                           "    ( " +
                                           //"     @EmpresaID, " +
                                           "    @NumeroDoc " +
                                           "    ,@ProveedorID " +                                          
                                           "    ,@LugarEntrega " +
                                           "    ,@BodegaID " +                                         
                                           "    ,@eg_glLocFisica " +
                                           "    ,@eg_Proveedor) ";

                #endregion
                #region Creacion de parametros
                //mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);             
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_Proveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);               
                #endregion
                #region Asignacion de valores
                //mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = orden.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = orden.ProveedorID.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = orden.LugarEntrega.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = orden.BodegaID.Value;
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_Proveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);             
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prRecibidoDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prRecibidoDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prRecibidoDocu_Upd(DTO_prRecibidoDocu orden)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                //Actualiza Tabla prOrdenCompraDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prRecibidoDocu " +
                                           "    SET  " +
                                           //"     EmpresaID  = @EmpresaID,  " +
                                           "     ProveedorID = @ProveedorID " +                                        
                                           "    ,LugarEntrega = @LugarEntrega " +
                                           "    ,BodegaID = @BodegaID " +                                                                    
                                           "    WHERE NumeroDoc = @NumeroDoc";
                #endregion
                #region Creacion de parametros
                //mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@LugarEntrega", SqlDbType.Char, UDT_LocFisicaID.MaxLength);
                mySqlCommand.Parameters.Add("@BodegaID", SqlDbType.Char, UDT_BodegaID.MaxLength);
                #endregion
                #region Asignacion de valores
                //mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = orden.NumeroDoc.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = orden.ProveedorID.Value;
                mySqlCommand.Parameters["@LugarEntrega"].Value = orden.LugarEntrega.Value;
                mySqlCommand.Parameters["@BodegaID"].Value = orden.BodegaID.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prRecibidoDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prRecibidoAprob> DAL_prRecibidoDocu_GetPendientesByModulo(DTO_glDocumento doc, string actFlujoID, string usuarioID)
        {
            try
            {
                List<DTO_prRecibidoAprob> result = new List<DTO_prRecibidoAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                //mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = doc.ModuloID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actFlujoID;//actividadPerm.ActividadFlujoID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuarioID;
                //mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                #endregion

                mySqlCommand.CommandText =
                    "   select temp.*,ctrl.EmpresaID,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha, " +
                    "       ctrl.Observacion ObservacionDesc,pr.ProveedorID,pr.Descriptivo ProveedorNombre,ctrl.MonedaID,ctrl.TasaCambioDOCU, " +
                    "       det.ConsecutivoDetaID,det.RecibidoDocuID,det.RecibidoDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID,det.ValorUni,det.IVAUni, " +
                    "       det.SolicitudDocuID,det.SolicitudDetaID,det.CodigoBSID,det.SerialID,det.inReferenciaID,det.Descriptivo,det.UnidadInvID,det.CantidadRec " +
                    "   from ( select distinct ctrl.NumeroDoc,usr.UsuarioID,SUM(det.ValorTotML) CostoML, SUM(det.ValorTotME) CostoME, SUM(det.IvaTotML) CostoIvaML,SUM(det.IvaTotME) CostoIvaME " +
                    "   from glDocumentoControl ctrl with(nolock)  " +
                    "       inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "	        and act.CerradoInd=@CerradoInd and act.ActividadFlujoID=@ActividadFlujoID " +
	                "       inner join prDetalleDocu det with(nolock) on ctrl.NumeroDoc = det.NumeroDoc " +
                    "   	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +                  
                    "   	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "   	inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID and perm.AreaFuncionalID = ctrl.AreaFuncionalID " +
                    "           and perm.UsuarioID = @UsuarioID " +
                    "   where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and perm.ActividadFlujoID = @ActividadFlujoID  " +
                    "   group by ctrl.NumeroDoc,usr.UsuarioID " +
                    "   ) temp 	inner join glDocumentoControl ctrl with(nolock) on temp.NumeroDoc = ctrl.NumeroDoc " +
                    "   	inner join prRecibidoDocu rec with(nolock) on temp.NumeroDoc = rec.NumeroDoc " +
                    "   	inner join prProveedor pr on rec.ProveedorID = pr.ProveedorID " +
                    "   	inner join prDetalleDocu det with(nolock) on temp.NumeroDoc = det.NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_prRecibidoAprob dto = new DTO_prRecibidoAprob(dr);
                    List<DTO_prRecibidoAprob> list = result.Where(x => ((DTO_prRecibidoAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_prRecibidoAprob(dr);
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                    }

                    DTO_prRecibidoAprobDet dtoDet = new DTO_prRecibidoAprobDet(dr);
                    dto.Detalle.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prRecibidoDocu_GetPendientesByModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_prRecibidoAprob> DAL_prRecibidoDocu_GetRecibidoNoFacturado(DTO_glDocumento doc,string proveedor, int NroDocfacturaExist)
        {
            try
            {
                List<DTO_prRecibidoAprob> result = new List<DTO_prRecibidoAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string whereFactu = string.Empty;
                string whereProveedor = string.Empty;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = doc.ModuloID.Value;
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;

                if (NroDocfacturaExist != 0)
                {
                    mySqlCommand.Parameters.Add("@FacturaDocuID", SqlDbType.Char);
                    mySqlCommand.Parameters["@FacturaDocuID"].Value = NroDocfacturaExist;
                    whereFactu = "det.FacturaDocuID = @FacturaDocuID";
                }
                else
                    whereFactu = "det.FacturaDocuID is null";

                if (!string.IsNullOrEmpty(proveedor))
                {
                    whereProveedor = " and pr.ProveedorID = @ProveedorID";
                    mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                    mySqlCommand.Parameters["@ProveedorID"].Value = proveedor;
                }

                #endregion

                mySqlCommand.CommandText =
                    "   select temp.*,ctrl.EmpresaID,ctrl.DocumentoID,ctrl.PrefijoID,ctrl.DocumentoNro,'' as PrefDoc,ctrl.FechaDoc Fecha,0 as TasaCambioDOCU, " +
                    "       ctrl.Observacion ObservacionDesc,pr.ProveedorID,pr.Descriptivo ProveedorNombre,ctrl.MonedaID, det.OrigenMonetario, " +
                    "       det.ConsecutivoDetaID,det.RecibidoDocuID,det.RecibidoDetaID,det.OrdCompraDocuID,det.OrdCompraDetaID, det.ValorUni,det.IVAUni,IsNull(mat.Margen,0) as Margen," +
                    "       det.SolicitudDocuID,det.SolicitudDetaID,det.CodigoBSID,det.inReferenciaID,det.SerialID,det.Descriptivo,det.UnidadInvID,det.CantidadRec, " +
                    "       det.ValorTotML as ValorTotMLRec,det.ValorTotME as ValorTotMERec, oc.MonedaOrden as MonedaOC,oc.TasaOrden as TasaOC " +
                    "   from ( select distinct ctrl.NumeroDoc,usr.UsuarioID,Isnull(SUM(det.ValorTotML),0) CostoML, IsNull(SUM(det.ValorTotME),0) CostoME, IsNull(SUM(det.IvaTotML),0) CostoIvaML,IsNull(SUM(det.IvaTotME),0) CostoIvaME " +
                    "   from glDocumentoControl ctrl with(nolock)  " +
                    "       inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc " +
                    "       inner join prDetalleDocu det with(nolock) on ctrl.NumeroDoc = det.NumeroDoc " +
                    "   	inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                    "   	inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "   where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado and " + whereFactu +
                    "   group by ctrl.NumeroDoc,usr.UsuarioID " +
                    "   ) temp 	inner join glDocumentoControl ctrl with(nolock) on temp.NumeroDoc = ctrl.NumeroDoc " +
                    "   	inner join prRecibidoDocu rec with(nolock) on temp.NumeroDoc = rec.NumeroDoc " +
                    "   	inner join prProveedor pr on rec.ProveedorID = pr.ProveedorID " + whereProveedor +
                    "   	inner join prDetalleDocu det with(nolock) on temp.NumeroDoc = det.NumeroDoc and det.DatoAdd3 is null " +
                    "       left join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID and refer.EmpresaGrupoID = det.eg_inReferencia " +
                    "       left join inEmpaque empaque with(nolock) on empaque.EmpaqueInvID = refer.EmpaqueInvID and empaque.EmpresaGrupoID = refer.eg_inEmpaque " +
                    "       left join inMaterial mat with(nolock) on mat.MaterialInvID = refer.MaterialInvID and mat.EmpresaGrupoID = refer.eg_inMaterial " +
                    "       left join prOrdenCompraDocu oc with(nolock)on det.OrdCompraDocuID = oc.NumeroDoc ";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_prRecibidoAprob dto = new DTO_prRecibidoAprob(dr);
                    List<DTO_prRecibidoAprob> list = result.Where(x => ((DTO_prRecibidoAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_prRecibidoAprob(dr);
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                        dto.PrefDoc = dto.PrefijoID.Value.Trim() + "-" + dto.DocumentoNro.Value.ToString();
                    }

                    DTO_prRecibidoAprobDet dtoDet = new DTO_prRecibidoAprobDet(dr);
                    if (!string.IsNullOrWhiteSpace(dr["OrigenMonetario"].ToString()))
                        dtoDet.OrigenMonetario.Value = Convert.ToByte(dr["OrigenMonetario"]);
                    if (!string.IsNullOrWhiteSpace(dr["TasaOC"].ToString()))
                        dtoDet.TasaOC.Value = Convert.ToDecimal(dr["TasaOC"]);
                    if (!string.IsNullOrWhiteSpace(dr["ValorTotMLRec"].ToString()))
                        dtoDet.ValorTotMLRecibDet.Value = Convert.ToDecimal(dr["ValorTotMLRec"]);
                    if (!string.IsNullOrWhiteSpace(dr["ValorTotMERec"].ToString()))
                        dtoDet.ValorTotMERecibDet.Value = Convert.ToDecimal(dr["ValorTotMERec"]);
                    dtoDet.MonedaOC.Value = (dr["MonedaOC"]).ToString();

                    dto.Detalle.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prRecibidoDocu_GetRecibidoNoFacturado");
                throw exception;
            }
        }

        #endregion
    }
}
