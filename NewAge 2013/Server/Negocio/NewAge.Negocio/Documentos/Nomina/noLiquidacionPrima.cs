using NewAge.ADO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.Negocio.PostSharpAspects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NewAge.Negocio
{
    public class noLiquidacionPrima : noLiquidacionBase
    {
        #region Propiedades

        public DateTime FechaIniLiq { get; set; }
        public DateTime FechaFinLiq { get; set; }
        public bool IndNomina { get; set; }

        #endregion

        public noLiquidacionPrima(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction]
        public override DTO_TxResult Liquidar()
        {
             DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            string periodo = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
            this.DocumentoID = AppDocuments.Prima;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

            if (result.Result == ResultValue.OK)
            {
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarPrima(this.Empleado.ID.Value,
                                                                                            this.DocCtrl.NumeroDoc.Value.Value,
                                                                                            this.FechaIniLiq,
                                                                                            this.FechaFinLiq,
                                                                                            this.tasaCambio,
                                                                                            this.IndNomina,
                                                                                            out errorInd,
                                                                                            out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;

                    switch (errorInd)
                    {
                        case -1 :
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_DatosAnuales + "&&" + this.Empresa.Descriptivo.Value;
                                break;
                            }
                        case -3:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_noExistConceptoNom + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                break;
                            }
                        case 0:
                            {
                                result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + errorMsg + "&&" + string.Empty;
                                break;
                            }
                        case 1:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                break;
                            }
                        case 2:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_liquidarPrima + "&&" + this.Empleado.ID.Value;
                                break;
                            }
                        case 3:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_ExistNovedadNominaPrima;
                                break;
                            }
                    }                    
                }
                
                #endregion
            }
            return result;
        }
    }
}
