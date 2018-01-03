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
    public class noLiquidacionCesantias : noLiquidacionBase
    {
        #region Propiedades

        public DateTime FechaIniLiq { get; set; }
        public DateTime FechaFinLiq { get; set; }
        public DateTime FechaPago { get; set; }
        public string Resolucion { get; set; }
        public TipoLiqCesantias tliqCesantias { get; set; }

        #endregion

        public noLiquidacionCesantias(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction]
        public override DTO_TxResult Liquidar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            string periodo = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
            this.DocumentoID = AppDocuments.Cesantias;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);
            if (result.Result == ResultValue.OK)
            {                
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarCesantias(this.Empleado.ID.Value,
                                                                                                this.DocCtrl.NumeroDoc.Value.Value,
                                                                                                this.FechaIniLiq,
                                                                                                this.FechaFinLiq,
                                                                                                this.FechaPago,
                                                                                                this.Resolucion,
                                                                                                this.tliqCesantias,
                                                                                                this.tasaCambio,
                                                                                                out errorInd,
                                                                                                out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;

                    switch (errorInd)
                    {
                        case -3:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_noExistConceptoNom + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                break;
                            }
                        case -1:
                            {
                                result.ResultMessage = DictionaryMessages.Err_No_DatosAnuales + "&&" + this.Empresa.Descriptivo.Value;
                                break;
                            }                    
                        case 1:
                            {
                                result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + errorMsg + "&&" + string.Empty;
                                break;
                            }
                    }                                  
                }                
                #endregion
            }
            return result;
        }

        #region Public Functions

        /// <summary>
        /// Actualiza los valores de cesantias ó intereses de cesantias
        /// </summary>
        /// <param name="numeroDoc">numero Doc</param>
        /// <param name="valorCesantias">valor cesantias</param>
        /// <param name="valorIntereses">valor intereses</param>
        /// <param name="indCesantias">ind cesantias</param>
        [Transaction]
        public void UpdateCesantias(int numeroDoc, decimal valorCesantias, decimal valorIntereses, bool indCesantias)
        {
            this._dal_noLiquidacionesDocu = (DAL_noLiquidacionesDocu)this.GetInstance(typeof(DAL_noLiquidacionesDocu), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_noLiquidacionesDocu.DAL_noLiquidacionesDocu_UpdateCesantias(numeroDoc, valorCesantias, valorIntereses, indCesantias);
        }

        #endregion

    }
}
