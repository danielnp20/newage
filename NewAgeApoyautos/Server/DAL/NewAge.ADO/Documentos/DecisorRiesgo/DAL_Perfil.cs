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
    public class DAL_Perfil : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_Perfil(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }


        public object DAL_Perfil_GetPerfil(DateTime periodoID, int Numerodoc, bool isML)
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
                mySqlCommand.Parameters.Add("@numeroDoc", SqlDbType.Int);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@Periodo"].Value = periodoID;
                mySqlCommand.Parameters["@numeroDoc"].Value = Numerodoc;
                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "DecisorRiesgo_Perfil";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;

                while (dr.Read())
                {
                    if (dr.FieldCount <= 10 && dr.FieldCount>0)
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
                        result.Result = ResultValue.OK;
                        return result;
                    }
                }

                dr.Close();

                if (result.Result == ResultValue.OK)
                {
                    result.Result = ResultValue.OK;
                    result.ResultMessage = "Se genero el Perfil exitosamente: "; //+ Numerodoc.ToString();
                    return result;
                }
                else
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Error;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Perfil_GetPerfil");
                return result;
            }
        }

        public object DAL_Genera_ObligacionesGarantias( int? NumeroDoc,string ClienteID,bool isML)
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
                mySqlCommand.Parameters.Add("@NumeroDoc", SqlDbType.Int);
  //              mySqlCommand.Parameters.Add("@ClienteID", SqlDbType.Char,UDT_ClienteID.MaxLength);
                mySqlCommand.Parameters.Add("@TipoConsulta",SqlDbType.TinyInt);
                #endregion
                #region Asignacion de parametros
                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@NumeroDoc"].Value = NumeroDoc;
//                mySqlCommand.Parameters["@ClienteID"].Value = ClienteID;
                mySqlCommand.Parameters["@TipoConsulta"].Value = 1;

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "Cartera_ObligacionesGarantias";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;

                while (dr.Read())
                {
                    if (dr.FieldCount <= 10 && dr.FieldCount > 0)
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
                        result.Result = ResultValue.OK;
                        return result;
                    }
                }

                dr.Close();

                if (result.Result == ResultValue.OK)
                {
                    result.Result = ResultValue.OK;
                    result.ResultMessage = "Se genero informacion de Obligaciones y Garantias: "; //+ Numerodoc.ToString();
                    return result;
                }
                else
                {
                    result.Details.Add(rd);
                    result.ResultMessage = DictionaryMessages.Error;

                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_Genera_ObligacionesGarantias");
                return result;
            }
        }
    
    }
}
