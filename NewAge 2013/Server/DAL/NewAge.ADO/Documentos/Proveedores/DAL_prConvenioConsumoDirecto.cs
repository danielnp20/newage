using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.ADO
{
    /// <summary>
    /// DAL de DAL_prConvenioConsumoDirecto
    /// </summary>
    public class DAL_prConvenioConsumoDirecto : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_prConvenioConsumoDirecto(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region DAL_prConvenioConsumoDirecto

        #region Funciones publicas

        /// <summary>
        /// Consulta un DAL_prConvenioConsumoDirecto segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prConvenioConsumoDirecto> DAL_prConvenioConsumoDirecto_GetByNumeroDoc(int NumeroDoc)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenioConsumoDirecto with(nolock) where prConvenioConsumoDirecto.NumeroDoc = @NumeroDoc ";

                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;

                List<DTO_prConvenioConsumoDirecto> footer = new List<DTO_prConvenioConsumoDirecto>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prConvenioConsumoDirecto detail = new DTO_prConvenioConsumoDirecto(dr);
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioConsumoDirecto_Get");
                throw exception;
            }
        }

        /// <summary>
        /// Consulta un prConvenioConsumoDirecto segun el numero de documento asociado
        /// </summary>
        /// <param name="NumeroDoc">Numero de Documento Control</param>
        /// <returns>lista de Cargos</returns>
        public List<DTO_prConvenioConsumoDirecto> DAL_prConvenioConsumoDirecto_GetByID(int ConsecutivoDetaID)
        {
            try
            {
                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.CommandText = "select * from prConvenioConsumoDirecto with(nolock) where prConvenioConsumoDirecto.ConsecutivoDetaID = @ConsecutivoDetaID ";

                mySqlCommand.Parameters.Add("@ConsecutivoDetaID", SqlDbType.Int);
                mySqlCommand.Parameters["@ConsecutivoDetaID"].Value = ConsecutivoDetaID;

                List<DTO_prConvenioConsumoDirecto> footer = new List<DTO_prConvenioConsumoDirecto>();
                SqlDataReader dr = mySqlCommand.ExecuteReader();

                int index = 0;
                while (dr.Read())
                {
                    DTO_prConvenioConsumoDirecto detail = new DTO_prConvenioConsumoDirecto(dr);
                    footer.Add(detail);
                    index++;
                }
                dr.Close();
                return footer;
            }
            catch (Exception ex)
            {
                //Log error
                var exception = new Exception(DictionaryMessages.Err_GettingDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioConsumoDirecto_GetByID");
                throw exception;
            }
        }

        /// <summary>
        /// Adiciona en tabla prConvenioConsumoDirecto
        /// </summary>
        /// <param name="footer">Cargos</param>
        public void DAL_prConvenioConsumoDirecto_Add(List<DTO_prConvenioConsumoDirecto> footer)
        {
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region CommandText
                mySqlCommand.CommandText = "    INSERT INTO prConvenioConsumoDirecto " +
                                           "    (EmpresaID    " +
                                           "    ,NumeroDoc    " +
                                           "    ,FechaPlanilla " +
                                           "    ,ProyectoID " +
                                           "    ,CentroCostoID " +
                                           "    ,ProveedorID " +
                                           "    ,CodigoBSID    " +
                                           "    ,inReferenciaID    " +
                                           "    ,SerialID    " +
                                           "    ,Cantidad    " +
                                           "    ,NumeroDocOC    " +
                                           "    ,eg_coProyecto " +
                                           "    ,eg_coCentroCosto " +
                                           "    ,eg_prProveedor " +
                                           "    ,eg_prBienServicio " +
                                           "    ,eg_inReferencia )" +
                                           "    VALUES" +
                                           "    (@EmpresaID " +
                                           "    ,@NumeroDoc " +
                                           "    ,@FechaPlanilla " +
                                           "    ,@ProyectoID " +
                                           "    ,@CentroCostoID " +
                                           "    ,@ProveedorID " +
                                           "    ,@CodigoBSID    " +
                                           "    ,@inReferenciaID    " +
                                           "    ,@SerialID    " +
                                           "    ,@Cantidad    " +
                                           "    ,@NumeroDocOC    " +
                                           "    ,@eg_coProyecto " +
                                           "    ,@eg_coCentroCosto " +
                                           "    ,@eg_prProveedor " +
                                           "    ,@eg_prBienServicio " +
                                           "    ,@eg_inReferencia )";
                #endregion
                #region Creacion de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaPlanilla", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@ProyectoID", SqlDbType.Char, UDT_ProyectoID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroCostoID", SqlDbType.Char, UDT_CentroCostoID.MaxLength);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@CodigoBSID", SqlDbType.Char, UDT_CodigoBSID.MaxLength);
                mySqlCommand.Parameters.Add("@inReferenciaID", SqlDbType.Char, UDT_inReferenciaID.MaxLength);             
                mySqlCommand.Parameters.Add("@SerialID", SqlDbType.Char, UDT_SerialID.MaxLength);
                mySqlCommand.Parameters.Add("@Cantidad", SqlDbType.Decimal);
                mySqlCommand.Parameters.Add("@NumeroDocOC", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prProveedor", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_prBienServicio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_inReferencia", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                foreach (DTO_prConvenioConsumoDirecto det in footer)
                {
                    #region Asignacion de valores
                    mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                    mySqlCommand.Parameters["@NumeroDoc"].Value = det.NumeroDoc.Value;
                    mySqlCommand.Parameters["@FechaPlanilla"].Value = det.FechaPlanilla.Value;
                    mySqlCommand.Parameters["@ProyectoID"].Value = det.ProyectoID.Value;
                    mySqlCommand.Parameters["@CentroCostoID"].Value = det.CentroCostoID.Value;
                    mySqlCommand.Parameters["@ProveedorID"].Value = det.ProveedorID.Value;
                    mySqlCommand.Parameters["@CodigoBSID"].Value = det.CodigoBSID.Value;
                    mySqlCommand.Parameters["@inReferenciaID"].Value = det.inReferenciaID.Value;
                    mySqlCommand.Parameters["@SerialID"].Value = det.SerialID.Value;
                    mySqlCommand.Parameters["@Cantidad"].Value = det.Cantidad.Value;
                    mySqlCommand.Parameters["@NumeroDocOC"].Value = det.NumeroDocOC.Value;   
                    mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_prProveedor"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prProveedor, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_prBienServicio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.prBienServicio, this.Empresa, egCtrl);
                    mySqlCommand.Parameters["@eg_inReferencia"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.inReferencia, this.Empresa, egCtrl);

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
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddDocument, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioConsumoDirecto_Add");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina registros de la tabla de prConvenioConsumoDirecto
        /// </summary>
        /// <param name="numeroDoc">NumeroDoc</param>
        public void DAL_prConvenioConsumoDirecto_Delete(int numeroDoc)
        {
            try
            {
                SqlCommand mySqlCommandSel = base.MySqlConnection.CreateCommand();
                mySqlCommandSel.Transaction = base.MySqlConnectionTx;

                mySqlCommandSel.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommandSel.Parameters.Add("@NumeroDoc", SqlDbType.Int);

                mySqlCommandSel.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommandSel.Parameters["@NumeroDoc"].Value = numeroDoc;

                mySqlCommandSel.CommandText = "DELETE FROM prConvenioConsumoDirecto where NumeroDoc = @NumeroDoc";

                mySqlCommandSel.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_DeleteData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConvenioConsumoDirecto_Delete");
                throw exception;
            }
        }

        /// <summary>
        /// Trae un listado de Consumos de Proyecto pendientes para aprobar
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConvenioAprob> DAL_prConsumoProyecto_GetPendientesByModulo(int doc, string actividadFlujoID, DTO_seUsuario usuario)
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
                   "   Select distinct det.FechaPlanilla,det.ProveedorID,det.CodigoBSID,bs.Descriptivo as DescriptivoCodBS, " +
                  "         det.inReferenciaID,refer.Descriptivo as DescriptivoRef,det.SerialID,det.Cantidad, det.NumeroDocOC, Cast(RTrim(ctrlOC.PrefijoID)+'-'+Convert(Varchar, ctrlOC.DocumentoNro) as Varchar(100)) as PrefDocOC, " + 
                  "         ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.MonedaID as Moneda,ctrl.ProyectoID, ctrl.CentroCostoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc Fecha," +
                  "         ctrl.Observacion,prov.Descriptivo as DescriptivoProv " +                 
                  "   from glDocumentoControl ctrl with(nolock) " +
                  "        inner join glActividadEstado act with(nolock) on act.NumeroDoc = ctrl.NumeroDoc  " +
                  "               and act.CerradoInd=0  and act.ActividadFlujoID= @ActividadFlujoID  " +
                  "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID " +
                  "        inner join seUsuario usr with(nolock) on ctrl.seUsuarioID = usr.ReplicaID  " +
                  "        inner join prConvenioConsumoDirecto det with(nolock) on ctrl.NumeroDoc = det.NumeroDoc " +
                  "        inner join prProveedor prov with(nolock) on det.ProveedorID = prov.ProveedorID " +
                  "        inner join prBienServicio bs with(nolock) on bs.CodigoBSID = det.CodigoBSID " +
                  "        inner join inReferencia refer with(nolock) on refer.inReferenciaID = det.inReferenciaID " +
                  "        inner join glDocumentoControl ctrlOC with(nolock) on ctrlOC.NumeroDoc = det.NumeroDocOC " +
                  "        inner join glActividadPermiso perm with(nolock) on perm.EmpresaGrupoID = ctrl.EmpresaID " +
                  "               and perm.UsuarioID = @UsuarioID and Perm.AreaFuncionalID = Ctrl.AreaFuncionalID " +
                  "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID and ctrl.Estado = @Estado " +
                  "    and perm.ActividadFlujoID = @ActividadFlujoID " +
                  "    group by det.FechaPlanilla,det.ProveedorID,det.CodigoBSID,bs.Descriptivo,det.inReferenciaID,refer.Descriptivo,det.SerialID,det.Cantidad, det.NumeroDocOC, " +
                  "         ctrlOC.PrefijoID,ctrlOC.DocumentoNro, ctrl.EmpresaID,ctrl.NumeroDoc,ctrl.MonedaID,ctrl.ProyectoID, ctrl.CentroCostoID,ctrl.PrefijoID,ctrl.DocumentoNro,ctrl.FechaDoc," +
                  "         ctrl.Observacion,prov.Descriptivo  ";

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

                    DTO_prConvenioConsumoDirectoAprobDet dtoDet = new DTO_prConvenioConsumoDirectoAprobDet(dr);
                    dto.listConvenioConsumoDet.Add(dtoDet);

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
        /// Trae un listado de Consumos Directo para Recibido de Consumo
        /// </summary>
        /// <param name="mod">Modulo del que va a traer el listado de pendientes</param>
        /// <returns></returns>
        public List<DTO_ConveniosResumen> DAL_prConsumoDirecto_GetResumen(int documentID, DTO_seUsuario usuario, ModulesPrefix mod, DateTime fechaCorte, string proveedorID)
        {
            try
            {
                List<DTO_ConveniosResumen> result = new List<DTO_ConveniosResumen>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                string filter = string.Empty;

                mySqlCommand.CommandText =

                "select ctrlConsumo.NumeroDoc,ctrlConsumo.DocumentoNro,ctrlConsumo.PrefijoID, CAST(RTRIM(ctrlConsumo.PrefijoID)+'-'+CONVERT(VARCHAR, ctrlConsumo.DocumentoNro) AS VARCHAR(50)) as PrefDoc,ctrlConsumo.MonedaID MonedaIDConvenio,ctrlConsumo.ProyectoID, ctrlConsumo.CentroCostoID," +
                "       detCons.ConsecutivoDetaID,detCons.Documento1ID ConsumoDocuID,detCons.Documento1ID SolicitudDespachoDocuID, detCons.Detalle1ID ConsumoDetaID,detCons.Detalle1ID SolicitudDespachoDetaID, " +
                "       detCons.CodigoBSID,detCons.inReferenciaID,detCons.SerialID,detCons.Descriptivo DescripDetalle, " +
                "       detCons.CantidadDoc1 CantidadConvenio,temp.ValorUni, temp.IVAUni,temp.FechaPlanilla FechaConsumo, temp.ProveedorID, temp.Descriptivo " +
                "from ( " +
                "    select det.Documento1ID,det.Detalle1ID, consumoDet.FechaPlanilla,prov.ProveedorID, prov.Descriptivo,det.ValorUni, det.IVAUni,sum(det.CantidadDoc1) CantidadDoc" +
                "    from prDetalleDocu det with(nolock)  " +
                "        inner join glDocumentoControl ctrl with(nolock) on det.NumeroDoc = ctrl.NumeroDoc  " +
                "        inner join prConvenioConsumoDirecto consumoDet with(nolock) on  det.Documento1ID = consumoDet.NumeroDoc  " +
                "        inner join prProveedor prov with(nolock) on prov.ProveedorID = consumoDet.ProveedorID " +
                "        inner join glDocumento doc with(nolock) on ctrl.DocumentoID = doc.DocumentoID  " +
                "    where ctrl.EmpresaID = @EmpresaID and doc.ModuloID = @ModuloID  and consumoDet.ProveedorID = @ProveedorID  " +
                "        and ( (ctrl.DocumentoID = @DocumentoConsumo or ctrl.DocumentoID =  @DocumentoRecibido) and ctrl.Estado = @Estado )  " +
                "    group by  det.Documento1ID,det.Detalle1ID, consumoDet.FechaPlanilla,prov.ProveedorID,prov.Descriptivo,det.ValorUni,det.IVAUni " +
                "    ) temp  " +
                "    inner join prDetalleDocu detCons with(nolock) on temp.Detalle1ID = detCons.ConsecutivoDetaID " +
                "    inner join glDocumentoControl ctrlConsumo with(nolock) on temp.Documento1ID = ctrlConsumo.NumeroDoc  " +
                "where temp.CantidadDoc != 0 and  temp.FechaPlanilla <= @FechaPlanilla  ";

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@ProveedorID", SqlDbType.Char, UDT_ProveedorID.MaxLength);
                mySqlCommand.Parameters.Add("@Estado", SqlDbType.TinyInt);
                mySqlCommand.Parameters.Add("@DocumentoConsumo", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@DocumentoRecibido", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@FechaPlanilla", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters.Add("@ModuloID", SqlDbType.Char, UDT_ModuloID.MaxLength);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@ProveedorID"].Value = proveedorID;              
                mySqlCommand.Parameters["@Estado"].Value = (int)EstadoDocControl.Aprobado;
                mySqlCommand.Parameters["@DocumentoConsumo"].Value = AppDocuments.ConsumoProyecto;
                mySqlCommand.Parameters["@DocumentoRecibido"].Value = AppDocuments.Recibido;
                mySqlCommand.Parameters["@FechaPlanilla"].Value = DateTime.Today.ToShortDateString();
                mySqlCommand.Parameters["@ModuloID"].Value = mod.ToString();
              

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
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_prConsumoDirecto_GetResumen");
                throw exception;
            }
        }

        #endregion 

        #endregion
    }
}
