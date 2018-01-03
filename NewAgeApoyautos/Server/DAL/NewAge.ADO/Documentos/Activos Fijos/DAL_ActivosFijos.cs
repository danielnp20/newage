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
    /// DAL Activos Fijos
    /// </summary>
    public class DAL_ActivosFijos : DAL_Base
    {
        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_ActivosFijos(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        /// <summary>
        /// Obtiene la lista de activos recibidos por numero de Factura
        /// </summary>
        /// <param name="numeroDoc">Numero de factura</param>
        /// <returns>Lista de activos</returns>
        public List<DTO_acActivoControl> DAL_ActivosFijos_GetActivosByNumDoc(int numeroDoc)
        {
            try
            {
                List<DTO_acActivoControl> result = new List<DTO_acActivoControl>();

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters.Add("@TipoCodigo", SqlDbType.Int);

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters["@TipoCodigo"].Value = (byte)TipoCodigo.Activo;

                mySqlCommand.CommandText =
                " SELECT recib.FacturaDocuID as NumeroDoc, recib.ConsecutivoDetaID as ConsecutivoDetaID, recib.SerialID, recib.inReferenciaID, recib.Descriptivo, clase.ActivoClaseID, clase.VidaUtilLOC, clase.VidaUtilIFRS, " +
                " clase.TipoDepreLOC, clase.TipoDepreIFRS, (recib.ValorTotML + recib.IvaTotML) as CostoLOC, (recib.ValorTotME + recib.IvaTotME) as CostoEXT, recib.CodigoBSID, " +
                " detalle.ProyectoID as ProyectoID, detalle.CentroCostoID  as CentroCostoID" +
                " FROM prDetalleDocu AS recib with(nolock) " +
	            "                   INNER JOIN inReferencia as ref with(nolock) on ref.inReferenciaID = recib.inReferenciaID " +
	            "                   INNER JOIN inRefClase AS refClase with(nolock) on ref.ClaseInvID = refClase.ClaseInvID " +
                "                   INNER JOIN acClase AS clase with(nolock)on refClase.ActivoClaseID = clase.ActivoClaseID and clase.EmpresaGrupoID = @EmpresaID" +
	            "                   INNER JOIN prBienServicio as bs with(nolock) on bs.CodigoBSID = recib.CodigoBSID " +
                "                   INNER JOIN prDetalleCargos as detalle with(nolock) on recib.ConsecutivoDetaID = detalle.ConsecutivoDetaID " +
                "INNER JOIN glBienServicioClase As bsclase with(nolock) on bsclase.ClaseBSID = bs.ClaseBSID and bsclase.TipoCodigo = @TipoCodigo " +
                "WHERE recib.EmpresaID = @EmpresaID and recib.FacturaDocuID = @NumeroDoc and recib.ActivoDocuID is null";

                SqlDataReader dr;
                dr = mySqlCommand.ExecuteReader();

                int index = 0;

                while (dr.Read())
                {
                    DTO_acActivoControl activo = new DTO_acActivoControl(dr, false);
                    result.Add(activo);
                    index++;
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_ActivosFijos_GetActivosByNumDoc");
                throw exception;

            }
        }

        /// <summary>
        /// Genera el comprobante para la depreciacion
        /// </summary>
        /// <param name="periodo">Periodo de cierre</param>
        /// <returns>Retorna una lista del comprobante</returns>
        public object DAL_ActivosFijos_GetComprobanteForDepreciacion(DateTime periodo,string terceroPorDefecto)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();

            try
            {
                List<DTO_ComprobanteFooter> footer = new List<DTO_ComprobanteFooter>();
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Creación de parametros
                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@NitEmpresa", SqlDbType.Char, UDT_TerceroID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coBalanceTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acComponenteActivo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acMovimientoTipo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_acContabiliza", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glBienServicioClase", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coOperacion", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coConceptoCargo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCargoCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                #endregion
                #region Asignacion de valores
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@NitEmpresa"].Value = terceroPorDefecto;
                mySqlCommand.Parameters["@eg_coBalanceTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coBalanceTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acComponenteActivo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acComponenteActivo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acMovimientoTipo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acMovimientoTipo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_acContabiliza"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.acContabiliza, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glBienServicioClase"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glBienServicioClase, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coOperacion"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coOperacion, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coConceptoCargo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coConceptoCargo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCargoCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCargoCosto, this.Empresa, egCtrl);
                #endregion
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Activos_CierreMensual";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();

                while (dr.Read())
                {
                    if(dr.GetName(0) == "Linea")
                    {
                        #region Revisa si hay error
                        result.Result = ResultValue.NOK;

                        rd = new DTO_TxResultDetail();
                        rd.line = Convert.ToInt32(dr["Linea"]);

                        #endregion
                        #region Carga el resultados con los mensajes de error
                        switch (dr["CodigoError"].ToString())
                        {
                            case "001": // Falta datos en glControl
                                rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                                break;
                            case "100": // No existe la operacion en coCargoCosto
                                rd.Message = DictionaryMessages.Err_Co_NoCtaCargoCosto + "&&" + dr["Valor"].ToString();
                                break;
                            case "101": // No se encontro componente de costo
                                rd.Message = DictionaryMessages.Err_Ac_NoCompCosto + "&&" + dr["Valor"].ToString();
                                break;
                            default: // (999): Error del SP 
                                rd.Message = dr["Valor"].ToString();
                                break;
                        }
                        #endregion

                        result.Details.Add(rd);
                    }
                    else
                    {
                        DTO_ComprobanteFooter det = new DTO_ComprobanteFooter(dr, true);
                        footer.Add(det);
                    }
                }
                dr.Close();

                return result.Result == ResultValue.OK ? (object)footer : (object)result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_ActivosFijos_GetComprobanteForDepreciacion");
                return result;
            }
        }

    }
}
