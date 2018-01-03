using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_Billing : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Billing(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        
        public object DAL_Billing_GetBilling(DateTime periodoID, string monedaID, bool isML)
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
                mySqlCommand.Parameters.Add("@PeriodoID", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@MonedaID", SqlDbType.Char, UDT_MonedaID.MaxLength);
                mySqlCommand.Parameters.Add("@IsML", SqlDbType.Bit);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coProyecto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCentroCosto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_plLineaPresupuesto", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ocCuentaGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coCuentaGrupo", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ocParticionTabla", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ocSocio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                //se elimino esta maestra
                //mySqlCommand.Parameters.Add("@eg_ocCuentaConjunta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                //mySqlCommand.Parameters.Add("@eg_ocContrato", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glLocFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_glAreaFisica", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@MonedaID"].Value = monedaID;
                mySqlCommand.Parameters["@IsML"].Value = isML;
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coProyecto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCentroCosto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_plLineaPresupuesto"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ocCuentaGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocCuentaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coCuentaGrupo"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCuentaGrupo, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ocParticionTabla"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocParticionTabla, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ocSocio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocSocio, this.Empresa, egCtrl);
                //se elimino esta maestra
                //mySqlCommand.Parameters["@eg_ocCuentaConjunta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocCuentaConjunta, this.Empresa, egCtrl);
                //mySqlCommand.Parameters["@eg_ocContrato"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocContrato, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glLocFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glLocFisica, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_glAreaFisica"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.glAreaFisica, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "OC_GenerarBilling";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;

                while (dr.Read())
                {
                    if (dr.FieldCount <= 10)
                    {
                        #region Errores
                        result.Result = ResultValue.NOK;

                        DTO_TxResultDetailFields rdf = new DTO_TxResultDetailFields();
                        line = Convert.ToInt32(dr["Linea"]);

                        if (line != rd.line)
                        {
                            if (addLine)
                                result.Details.Add(rd);

                            addLine = true;
                            rd = new DTO_TxResultDetail();
                            rd.line = line;
                            rd.Message = "NOK";
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                        }

                        switch (dr["CodigoError"].ToString())
                        {
                            case "001": // Falta datos en glControl
                                rd.Message = DictionaryMessages.Err_ControlNoData + "&&" + dr["Valor"].ToString() + "&&" + string.Empty;
                                break;
                            case "002": // No se encontro cuenta de contrapartida
                                rdf.Field = dr["Campo"].ToString();
                                rdf.Message = DictionaryMessages.Err_Oc_NoCtaContra + "&&" + dr["Valor"].ToString();
                                rd.DetailsFields.Add(rdf);
                                break;
                            case "003": // La particion no corresponde al 100%
                                rdf.Field = dr["Campo"].ToString();
                                rdf.Message = DictionaryMessages.Err_Oc_InvalidParticionPorc + "&&" + dr["Valor"].ToString();
                                rd.DetailsFields.Add(rdf);
                                break;
                            case "004": // La partición no tiene socio operador
                                rdf.Field = dr["Campo"].ToString();
                                rdf.Message = DictionaryMessages.Err_Oc_InvalidParticionSocio + "&&" + dr["Valor"].ToString();
                                rd.DetailsFields.Add(rdf);
                                break;
                            case "005": // No existe relación para obtener el contrato
                                rdf.Field = dr["Campo"].ToString();
                                rdf.Message = DictionaryMessages.Err_Oc_NoContrato + "&&" + dr["Valor"].ToString();
                                rd.DetailsFields.Add(rdf);
                                break;
                            default: // (999): Error del SP 
                                rd.Message = dr["Valor"].ToString();
                                break;

                        }
                        #endregion
                    }
                    else
                    {
                        #region Footer
                        DTO_ComprobanteFooter det = new DTO_ComprobanteFooter(dr, true);
                        footer.Add(det);
                        #endregion
                    }
                }
                dr.Close();

                if (result.Result == ResultValue.OK)
                    return footer;
                else
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_Oc_Billing;
                    
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Billing_GetBilling");
                return result;
            }
        }
    }
}
