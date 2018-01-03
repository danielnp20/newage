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
    public class DAL_CashCall : DAL_Base
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DAL_CashCall(SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, string loggerConn) : base(c, tx, empresa, userId, loggerConn) { }


        public DTO_TxResult DAL_CashCall_Procesar(DateTime periodoID)
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
                mySqlCommand.Parameters.Add("@eg_ocParticionTabla", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);
                mySqlCommand.Parameters.Add("@eg_ocSocio", SqlDbType.Char, UDT_EmpresaGrupoID.MaxLength);

                #endregion
                #region Asignacion de parametros

                mySqlCommand.Parameters["@EmpresaID"].Value = this.Empresa.ID.Value;
                mySqlCommand.Parameters["@PeriodoID"].Value = periodoID;
                mySqlCommand.Parameters["@eg_ocParticionTabla"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocParticionTabla, this.Empresa, egCtrl);
                mySqlCommand.Parameters["@eg_ocSocio"].Value = this.GetMaestraEmpresaGrupoByDocumentID(AppMasters.ocSocio, this.Empresa, egCtrl);

                #endregion

                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.CommandText = "OC_CashCall";

                SqlDataReader dr = mySqlCommand.ExecuteReader();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.line = 0;
                int line = 0;
                bool addLine = false;

                while (dr.Read())
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
                        case "002": // No existe información de legalización para el periodo {0} 
                            rdf.Field = dr["Campo"].ToString();
                            rdf.Message = DictionaryMessages.Err_Oc_NoLegalizacion + "&&" + dr["Valor"].ToString();
                            rd.DetailsFields.Add(rdf);
                            break;
                        default: // (999): Error del SP 
                            rd.Message = dr["Valor"].ToString();
                            break;

                    }
                    #endregion
                }
                dr.Close();

                return result;
            }
            catch (Exception ex)
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "DAL_CashCall_Procesar");
                return result;
            }
        }
    }
}
