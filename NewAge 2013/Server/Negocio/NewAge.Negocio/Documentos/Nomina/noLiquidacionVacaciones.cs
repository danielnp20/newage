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
    public class noLiquidacionVacaciones : noLiquidacionBase
    {
        #region Propiedades

        public DateTime FechaIniLiq { get; set; }
        public DateTime FechaFinLiq { get; set; }
        public DateTime FechaIniPagoLiq { get; set; }
        public DateTime FechaFinPagoLiq { get; set; }
        public int DiasVacTiempo { get; set; }
        public int DiasVacDinero { get; set; }
        public bool IndIncNomina { get; set; }
        public bool IndPrima { get; set; }
        public string Resolucion { get; set; }
        public DateTime FechaIniPendVacac { get; set; }
        public DateTime FechaFinPendVacac { get; set; }

        #endregion

        public noLiquidacionVacaciones(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }


        [Transaction]
        public override DTO_TxResult Liquidar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            string periodo = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
            this.DocumentoID = AppDocuments.Vacaciones;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);            

            if (result.Result == ResultValue.OK)
                result = this.LiquidarNovedadesNomina();

            if (result.Result == ResultValue.OK)
            {
                #region genera la liquidación de Vacaciones
                
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarVacaciones(this.Empleado.ID.Value,
                                                                                                this.DocCtrl.NumeroDoc.Value.Value,
                                                                                                this.FechaIniLiq,
                                                                                                this.FechaFinLiq,
                                                                                                this.DiasVacTiempo,
                                                                                                this.DiasVacDinero,
                                                                                                this.IndIncNomina,
                                                                                                this.IndPrima,
                                                                                                this.Resolucion,
                                                                                                this.tasaCambio,
                                                                                                this.FechaIniPagoLiq,
                                                                                                this.FechaFinPagoLiq,
                                                                                                this.FechaIniPendVacac,
                                                                                                this.FechaFinPendVacac,
                                                                                                out errorInd,
                                                                                                out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;
                    switch (errorInd)
                    {
                        case 1:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                break;
                            }
                        case 2:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_VacacionesDiasCausados;
                                break;
                            }
                        case 3:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_liquidarVacaciones + "&&" + this.Empleado.ID.Value;
                                break;
                            }
                    }
                }

                #endregion

                #endregion
            }

            return result;
        }

     
    }
}
