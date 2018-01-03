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
    public class noLiquidacionNomina : noLiquidacionBase
    {
        public noLiquidacionNomina(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        /// <summary>
        /// Metodo para Liquidar la Nomina
        /// </summary>
        /// <returns></returns>
        [Transaction]       
        public override DTO_TxResult Liquidar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            string periodo = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
            this.DocumentoID = AppDocuments.Nomina;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);            
            if(result.Result == ResultValue.OK)
                result = this.LiquidarNovedadesNomina();
            
            if (result.Result == ResultValue.OK)
            {
                #region genera la liquidación de Nomina

                //Ejecuta procedimiento almacenado para liquidar la Prenomina
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarNomina(this.Empleado.ID.Value, this.DocCtrl.NumeroDoc.Value.Value, this.tasaCambio, out errorInd, out errorMsg);
                
                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;

                    #region Errores Generales

                    if (errorInd == -4)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_FechaFueraRango + "&&" + this.Empleado.ID.Value;
                        
                    }
                    if (errorInd == -3)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_noExistConceptoNom + "&&" + this.Empleado.ID.Value + "&&" + "";
                    }
                    if (errorInd == -1)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_No_DatosAnuales + "&&" + this.Empresa.Descriptivo.Value;                        
                    }
                    if (errorInd == 0)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + errorMsg.Trim() + "&&" + "";                        
                    }

                    #endregion

                    switch (errorInd)
                    {
                        case 1:
                            {
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
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
