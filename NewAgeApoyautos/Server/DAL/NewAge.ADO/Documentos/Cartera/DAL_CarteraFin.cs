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
using System.ComponentModel;
using System.Reflection;
using NewAge.DTO.Resultados;

namespace NewAge.ADO
{
    public class DAL_CarteraFin : DAL_Base
    {

        /// <summary>
        /// Constructor para manejo de conexion y parametros generales
        /// </summary>
        /// <param name="c">conexion</param>
        /// <param name="tx">transaccion</param>
        /// <param name="empresa">empresa</param>
        /// <param name="userId">userId</param>
        public DAL_CarteraFin(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }

        #region Funciones Generales

        /// <summary>
        /// Procesa la migracon de nomina
        /// </summary>
        /// <param name="centroPagoID">Identificador del centro de pago</param>
        /// <param name="periodo">Periodo de proceso</param>
        /// <param name="data">Información a migrar</param>
        /// <returns></returns>
        public DTO_TxResult DAL_CarteraFin_RecaudosMasivos_Inconsistencias(string centroPagoID, DateTime periodo, List<DTO_ccIncorporacionDeta> data)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            try
            {
                string egCtrl = StaticMethods.GetGrupoEmpresasControl(base.MySqlConnection, base.MySqlConnectionTx, this.loggerConnectionStr);

                SqlCommand mySqlCommand = base.MySqlConnection.CreateCommand();
                mySqlCommand.Transaction = base.MySqlConnectionTx;

                #region Carga la info de los detalles
                DataTable nomina = new DataTable();
                nomina.Columns.Add("Consecutivo", typeof(int));
                nomina.Columns.Add("Fecha", typeof(DateTime));
                nomina.Columns.Add("Libranza", typeof(int));
                nomina.Columns.Add("ValorCuota", typeof(decimal));

                int i = 0;
                foreach (DTO_ccIncorporacionDeta dto in data)
                {
                    i++;

                    DataRow fila = nomina.NewRow();
                    fila["Consecutivo"] = i;
                    fila["Fecha"] = dto.FechaNomina.Value;
                    fila["Libranza"] = dto.Libranza.Value;
                    fila["ValorCuota"] = dto.ValorCuota.Value;

                    nomina.Rows.Add(fila);
                }
                #endregion
                #region Creación de parametros

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters.Add("@CentroPagoID", SqlDbType.Char, UDT_CentroPagoID.MaxLength);
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.DateTime);
                mySqlCommand.Parameters.Add("@eg_ccCentroPagoPAG", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coPlanCuenta", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_coTercero", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccCarteraComponente", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ccAbogado", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                //Parametro del footer (lista)
                mySqlCommand.Parameters.Add("@Nomina", SqlDbType.Structured);
                mySqlCommand.Parameters["@Nomina"].TypeName = "MigracionNomina";

                //Parametros de salida
                mySqlCommand.Parameters.Add("@ErrorDesc", SqlDbType.VarChar, 1000);
                mySqlCommand.Parameters["@ErrorDesc"].Direction = ParameterDirection.Output;
                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@CentroPagoID"].Value = centroPagoID;
                mySqlCommand.Parameters["@Periodo"].Value = periodo;
                mySqlCommand.Parameters["@eg_ccCentroPagoPAG"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCentroPagoPAG, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coPlanCuenta"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_coTercero"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coTercero, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccCarteraComponente"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccCarteraComponente, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ccAbogado"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ccAbogado, this.Empresa, egCtrl);

                mySqlCommand.Parameters["@Nomina"].Value = nomina;

                #endregion
                #region Ejecuta la consulta y carga los resultados
                mySqlCommand.CommandTimeout = 6000;
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "CarteraFin_RecaudosMasivos_Inconsistencia";

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                if (mySqlCommand.Parameters["@ErrorDesc"].Value != null && !string.IsNullOrWhiteSpace(mySqlCommand.Parameters["@ErrorDesc"].Value.ToString()))
                {
                    var exception = new Exception(mySqlCommand.Parameters["@ErrorDesc"].Value.ToString());
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_Cartera_RecaudosMasivos_Inconsistencias");
                }
                else
                {
                    #region Resultado de cruce
                    int line = 1;

                    while (dr.Read())
                    {
                        DTO_TxResultDetail rd = new DTO_TxResultDetail();
                        rd.line = line;

                        switch (dr["EstadoCruce"].ToString())
                        {
                            #region Tipos de inconsistencias
                            case "5":
                                rd.Message = DictionaryMessages.Err_Cf_EstadoCruce5;
                                break;
                            case "7":
                                rd.Message = DictionaryMessages.Err_Cf_EstadoCruce7 + "&&" + dr["DocumentoTercero"].ToString().Trim();
                                break;
                            case "8":
                                rd.Message = DictionaryMessages.Err_Cf_EstadoCruce8;
                                break;
                            case "9":
                                rd.Message = DictionaryMessages.Err_Cf_EstadoCruce9;
                                break;
                            case "11":
                                rd.Message = DictionaryMessages.Err_Cf_EstadoCruce11;
                                break;
                            #endregion
                        }

                        if (!string.IsNullOrWhiteSpace(rd.Message))
                            result.Details.Add(rd);

                        ++line;
                    }
                    #endregion
                }

                dr.Close();

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_CarteraFin_MigracionNomina_Inconsistencias");
                return result;
            }
        }

        /// <summary>
        /// Trae informacion  de los mvtos de inventarios para contabilidad
        /// </summary>
        /// <returns>retorna una lista de comprob </returns>
        public object DAL_CarteraFin_GetComprobanteAmortizaDerechos(int numeroDoc, DateTime periodo, bool isAmortizaPagos)
        {
            try
            {
                DTO_Comprobante comp = new DTO_Comprobante();
                List<DTO_ComprobanteFooter> comprobantes = new List<DTO_ComprobanteFooter>();
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                SqlCommand mySqlCommand = new SqlCommand(isAmortizaPagos ? "Cartera_ComprobanteAmortizaDerechosPagos" : "Cartera_ComprobanteAmortizaDerechosPrepagos", base.MySqlConnection.CreateCommand().Connection);
                mySqlCommand.Transaction = base.MySqlConnectionTx;
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@EmpresaID", SqlDbType.Char, UDT_EmpresaID.MaxLength);
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
                mySqlCommand.Parameters["@NumeroDoc"].Value = numeroDoc;
                mySqlCommand.Parameters.Add("@Periodo", SqlDbType.SmallDateTime);
                mySqlCommand.Parameters["@Periodo"].Value = periodo;

                SqlDataReader dr = mySqlCommand.ExecuteReader();
                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;
                while (dr.Read())
                {
                    //Valida si hay errores de salida
                    if (dr.GetName(0) == "Linea")
                    {
                        #region Asigna el error
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
                        rd.Message = dr["Valor"].ToString();
                        #endregion
                    }
                    else //Sino crea el comprobante
                    {
                        #region Footer
                        DTO_ComprobanteFooter dto = new DTO_ComprobanteFooter();
                        dto.CuentaID.Value = dr["CuentaID"].ToString();
                        dto.TerceroID.Value = dr["TerceroID"].ToString();
                        dto.ProyectoID.Value = dr["ProyectoID"].ToString();
                        dto.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                        dto.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                        dto.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                        dto.LugarGeograficoID.Value = dr["LugarGeograficoID"].ToString();
                        dto.PrefijoCOM.Value = dr["PrefijoCOM"].ToString();
                        dto.DocumentoCOM.Value = dr["DocumentoCOM"].ToString();
                        dto.ActivoCOM.Value = dr["ActivoCOM"].ToString();
                        dto.ConceptoSaldoID.Value = dr["ConceptoSaldoID"].ToString();
                        dto.IdentificadorTR.Value = Convert.ToInt64(dr["IdentificadorTR"]);
                        dto.Descriptivo.Value = dr["Descriptivo"].ToString();
                        dto.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambioBase"]);
                        dto.vlrBaseML.Value = Convert.ToDecimal(dr["vlrBaseML"]);
                        dto.vlrBaseME.Value = Convert.ToDecimal(dr["vlrBaseME"]);
                        dto.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                        dto.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);
                        dto.CuentaAlternaID.Value = dr["CuentaAlternaID"].ToString();
                        if (!string.IsNullOrWhiteSpace(dr["vlrMdaOtr"].ToString()))
                            dto.vlrMdaOtr.Value = Convert.ToDecimal(dr["vlrMdaOtr"]);
                        dto.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                        dto.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                        dto.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                        dto.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                        dto.DatoAdd5.Value = dr["DatoAdd5"].ToString();
                        dto.DatoAdd6.Value = dr["DatoAdd6"].ToString();
                        dto.DatoAdd7.Value = dr["DatoAdd7"].ToString();
                        dto.DatoAdd8.Value = dr["DatoAdd8"].ToString();
                        dto.DatoAdd9.Value = dr["DatoAdd9"].ToString();
                        dto.DatoAdd10.Value = dr["DatoAdd10"].ToString();
                        #endregion
                        #region Header
                        comp.Header.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                        comp.Header.ComprobanteNro.Value = 0;
                        comp.Header.EmpresaID.Value = dr["EmpresaID"].ToString();
                        comp.Header.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                        comp.Header.NumeroDoc.Value = numeroDoc;
                        comp.Header.MdaOrigen.Value = Convert.ToByte(dr["MdaOrigen"]);
                        comp.Header.MdaTransacc.Value = dr["MdaTransacc"].ToString();
                        comp.Header.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                        comp.Header.TasaCambioBase.Value = dto.TasaCambio.Value;
                        comp.Header.TasaCambioOtr.Value = Convert.ToDecimal(dr["TasaCambioOtr"]);
                        comprobantes.Add(dto);
                        #endregion
                    }
                }
                comp.Footer = comprobantes.FindAll(x => x.vlrMdaLoc.Value != 0).ToList();
                dr.Close();

                if (result.Result == ResultValue.NOK)
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Err_GettingData;
                    return result;
                }

                return comp;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "DAL_CarteraFin_GetComprobanteAmortizaDerechos");
                throw exception;
            }
        }
        
        #endregion
    }
}
