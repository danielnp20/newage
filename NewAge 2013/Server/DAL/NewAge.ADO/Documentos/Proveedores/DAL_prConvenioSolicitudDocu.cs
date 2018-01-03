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
    /// DAL_prConvenioSolicitudDocu
    /// </summary>
    public class DAL_prConvenioSolicitudDocu : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prConvenioSolicitudDocu(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones publicas

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prConvenioSolicitudDocu DAL_prConvenioSolicitudDocu_Get(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenioSolicitudDocu with(nolock) where NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                DTO_prConvenioSolicitudDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prConvenioSolicitudDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un Orden de Compra segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns></returns>
        public DTO_prConvenioSolicitudDocu DAL_prConvenioSolicitudDocu_GetByNroContrato(int NumeroDocContrato)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select Top (1) * from prConvenioSolicitudDocu with(nolock) where NumeroDocContrato = @NumeroDocContrato Order By NumeroDoc desc ";

                mySqlCommand.Parameters.Add("@NumeroDocContrato", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDocContrato"].Value = NumeroDocContrato;

                DTO_prConvenioSolicitudDocu result = null;
                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    result = new DTO_prConvenioSolicitudDocu(dr);
                }
                dr.Close();
                return result;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_Get");
                throw exception;
            }
        }

        /// <summary>
        /// adiciona en tabla prOrdenCompraDocu 
        /// </summary>
        /// <param name="sol">Orden de Compra</param>
        /// <returns></returns>
        public void DAL_prConvenioSolicitudDocu_Add(DTO_prConvenioSolicitudDocu convenioSol)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prConvenioSolicitudDocu " +
                                           "    (EmpresaID " +
                                           "    ,NumeroDoc " +
                                           "    ,ProveedorID " +
                                           "    ,NumeroDocContrato " +
                                           "    ,Moneda " +
                                           "    ,Valor " +
                                           "    ,IVA " +
                                           "    ,eg_prProveedor) " +
                                            "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +                                          
                                           "    ,@ProveedorID " +
                                            "   ,@NumeroDocContrato " +
                                           "    ,@Moneda " +
                                           "    ,@Valor " +
                                           "    ,@IVA " +
                                           "    ,@eg_prProveedor)";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocContrato", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Moneda", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = convenioSol.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = convenioSol.ProveedorID.Value;
                mySqlCommand.Parameters["@NumeroDocContrato"].Value = convenioSol.NumeroDocContrato.Value;
                mySqlCommand.Parameters["@Moneda"].Value = convenioSol.Moneda.Value;
                mySqlCommand.Parameters["@Valor"].Value = convenioSol.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = convenioSol.IVA.Value;
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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_Add");
                throw exception;
            }

        }

        /// <summary>
        /// Actualizar el registro en tabla prOrdenCompraDocu y asociar en glDocumentoControl
        /// </summary>
        /// <param name="leg">Orden de Compra</param>
        public void DAL_prConvenioSolicitudDocu_Upd(DTO_prConvenioSolicitudDocu convenioSol)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                //Actualiza Tabla prConvenioSolicitudDocu
                #region CommandText
                mySqlCommand.CommandText = "    UPDATE prConvenioSolicitudDocu " +
                                           "    SET EmpresaID  = @EmpresaID  " +
                                           "    ,ProveedorID = @ProveedorID " +
                                           "    ,NumeroDocContrato = @NumeroDocContrato " +
                                           "    ,Moneda = @Moneda " +
                                           "    ,Valor = @Valor" +
                                           "    ,IVA = @IVA" +
                                           "    ,eg_prProveedor = @eg_prProveedor" +   
                                           "    WHERE NumeroDoc = @NumeroDoc";

                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDocContrato", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@Moneda", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@Valor", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@IVA", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@NumeroDoc"].Value = convenioSol.NumeroDoc.Value;
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = convenioSol.ProveedorID.Value;
                mySqlCommand.Parameters["@NumeroDocContrato"].Value = convenioSol.NumeroDocContrato.Value;
                mySqlCommand.Parameters["@Moneda"].Value = convenioSol.Moneda.Value;
                mySqlCommand.Parameters["@Valor"].Value = convenioSol.Valor.Value;
                mySqlCommand.Parameters["@IVA"].Value = convenioSol.IVA.Value;
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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_Upd");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de Compra pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConvenioAprob> DAL_prConvenioSolicitudDocu_GetPendientesByModulo(int doc, string actividadFlujoID, DTO_seUsuario usuario)
        {
            try
            {
                List<DTO_ConvenioAprob> result = new List<DTO_ConvenioAprob>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Common parameters
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@CerradoInd", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@ActividadFlujoID", SqlDbType.Char, UDT_ActividadFlujoID.MaxLength);
                mySqlCommand.Parameters.Add("@AreaAprobacion", SqlDbType.Char, UDT_AreaFuncionalID.MaxLength);
                mySqlCommand.Parameters.Add("@UsuarioID", SqlDbType.Char, UDT_UsuarioID.MaxLength);
                
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = ModulesPrefix.pr.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.ParaAprobacion;
                mySqlCommand.Parameters["@CerradoInd"].Value = false;
                mySqlCommand.Parameters["@ActividadFlujoID"].Value = actividadFlujoID;
                mySqlCommand.Parameters["@AreaAprobacion"].Value = usuario.AreaFuncionalID.Value;
                mySqlCommand.Parameters["@UsuarioID"].Value = usuario.ID.Value;
                #endregion

                  mySqlCommand.CommandText =
                    "Select distinct det.CodigoBSID,bs.Descriptivo as DescriptivoCodBS,det.inReferenciaID,refer.Descriptivo as DescriptivoRef, " +
                    "       det.CantidadDoc1,det.ValorUni, det.IVAUni,det.ValorTotML, det.ValorTotME, det.IvaTotML, det.IvaTotME, " + 
                    "       ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha, ctrl.Observacion, " +
                    "       pr.ProveedorID,pr.Descriptivo ProveedorNombre,conv.Moneda,conv.Valor " +
                    "    from glDocumentoControl ctrl with(nolock) " + 
                    "        inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc  " +
                    "               and act.CerradoInd=0  and act.ActividadFlujoID= @ActividadFlujoID  " +
	                "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                    "        inner join prConvenioSolicitudDocu conv with(nolock) on ctrl.NumeroDoc = conv.NumeroDoc  " +
	                "        inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                    "        inner join prProveedor pr with(nolock) on conv.ProveedorID = pr.ProveedorID " +
                    "        inner join prBienServicio bs with(nolock) on bs.CodigoBSID = det.CodigoBSID " +
                    "        inner join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID " +
                    "        inner join prDetalleDocu det with(nolock) on conv.NumeroDoc = det.NumeroDoc " +
                    "        inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                    "               and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                    "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " +
                    "    and perm.ActividadFlujoID = @ActividadFlujoID " +
                    "    group by det.CodigoBSID,bs.Descriptivo,det.inReferenciaID,refer.Descriptivo,det.CantidadDoc1,det.ValorUni, det.IVAUni,det.ValorTotML, det.ValorTotME, det.IvaTotML, det.IvaTotME, " + 
                    "         ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc, " +
                    "         ctrl.Observacion, pr.ProveedorID,pr.Descriptivo,conv.Moneda,conv.Valor " ;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;
                    DTO_ConvenioAprob dto = new DTO_ConvenioAprob(dr);
                    List<DTO_ConvenioAprob> list = result.Where(x => ((DTO_ConvenioAprob)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dto = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dto = new DTO_ConvenioAprob(dr);
                        dto.Aprobado.Value = false;
                        dto.Rechazado.Value = false;
                    }

                    DTO_prSolicitudDespachoAprobDet dtoDet = new DTO_prSolicitudDespachoAprobDet(dr);
                    dto.listConvenioSolicitudDet.Add(dtoDet);

                    if (nuevo)
                        result.Add(dto);
                }

                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_GetPendientesByModulo");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Ordenes de COmpra para Recibido
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConveniosResumen> DAL_prConvenioSolicitudDocu_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, string proveedor)
        {
            try
            {
                List<DTO_ConveniosResumen> result = new List<DTO_ConveniosResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;

                mySqlCommand.CommandText =

                "select ctrlConvenio.NumeroDoc,ctrlConvenio.DocumentoNro,ctrlConvenio.PrefijoID, CAST(RTRIM(ctrlConvenio.PrefijoID)+'-'+CONVERT(VARCHAR, ctrlConvenio.DocumentoNro) AS VARCHAR(50)) as PrefDoc,ctrlConvenio.MonedaID MonedaIDConvenio,ctrlConvenio.ProyectoID, ctrlConvenio.CentroCostoID,ctrlConvenio.FechaDoc FechaConsumo," +
                "       detCons.ConsecutivoDetaID,detCons.Documento1ID ConsumoDocuID,detCons.Documento1ID SolicitudDespachoDocuID, detCons.Detalle1ID ConsumoDetaID,detCons.Detalle1ID SolicitudDespachoDetaID, " +
                "       detCons.CodigoBSID,detCons.inReferenciaID,detCons.SerialID,detCons.Descriptivo DescripDetalle, " +
                "       detCons.CantidadDoc1 CantidadConvenio,temp.ValorUni, temp.IVAUni,temp.ProveedorID, temp.Descriptivo " +
                "from ( " +
                "    select det.Documento1ID,det.Detalle1ID, prov.ProveedorID, prov.Descriptivo,det.ValorUni, det.IVAUni,sum(det.CantidadDoc1) CantidadDoc" +
                "    from prDetalleDocu det with(nolock)  " +
                "        inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc  " +
                "        inner join prConvenioSolicitudDocu convSolDocu with(nolock) on  det.Documento1ID = convSolDocu.NumeroDoc  " +
                "        inner join prProveedor prov with(nolock) on prov.ProveedorID = convSolDocu.ProveedorID " +
                "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and convSolDocu.ProveedorID = @ProveedorID " +
                "        and ( (ctrl.DocumentoID = @DocumentoSolDespacho or ctrl.DocumentoID =  @DocumentoRecibido) and ctrl.Estado = @Estado )  " +
                "    group by  det.Documento1ID,det.Detalle1ID, prov.ProveedorID,prov.Descriptivo,det.ValorUni,det.IVAUni " +
                "    ) temp  " +
                "    inner join prDetalleDocu detCons with(nolock) on temp.Detalle1ID = detCons.ConsecutivoDetaID " +
                "    inner join glDocumentoControl ctrlConvenio with(nolock) on temp.Documento1ID = ctrlConvenio.NumeroDoc  " +
                "where temp.CantidadDoc != 0 ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoSolDespacho", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoRecibido", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@DocumentoSolDespacho"].Value = AppDocuments.SolicitudDespachoConvenio;
                mySqlCommand.Parameters["@DocumentoRecibido"].Value = AppDocuments.Recibido;
                mySqlCommand.Parameters["@ProveedorID"].Value = proveedor;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    int numDoc = Convert.ToInt32(dr["NumeroDoc"]);
                    bool nuevo = true;  
                    DTO_ConveniosResumen dtoConsumo = new DTO_ConveniosResumen(dr);
                    List<DTO_ConveniosResumen> list = result.Where(x => ((DTO_ConveniosResumen)x).NumeroDoc.Value.Value.Equals(numDoc)).ToList();
                    if (list.Count > 0)
                    {
                        dtoConsumo = list.First();
                        nuevo = false;
                    }
                    else
                    {
                        dtoConsumo = new DTO_ConveniosResumen(dr);
                    }

                    DTO_ConveniosResumenDet dtoConsumoDet = new DTO_ConveniosResumenDet(dr);
                    dtoConsumoDet.ValorTotal.Value = dtoConsumoDet.ValorUni.Value * dtoConsumoDet.CantidadConvenio.Value;
                    dtoConsumoDet.IVATotal.Value = dtoConsumoDet.IVAUni.Value * dtoConsumoDet.CantidadConvenio.Value;
                    dtoConsumo.Detalle.Add(dtoConsumoDet);

                    if (nuevo)
                        result.Add(dtoConsumo);
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioSolicitudDocu_GetResumen");
                throw exception;
            }
        }
        
        #endregion

    }
}
