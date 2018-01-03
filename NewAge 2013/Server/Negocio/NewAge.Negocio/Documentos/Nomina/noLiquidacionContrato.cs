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
    public class noLiquidacionContrato : noLiquidacionBase
    {
        #region Propiedades 

        public DateTime FechaRetiro { get; set; }
        public int CausaLiquidacion { get; set; }

        #endregion

        public noLiquidacionContrato(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }
        
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
            this.DocumentoID = AppDocuments.LiquidacionContrato;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noLiquidacionPreliminar = (DAL_noLiquidacionPreliminar)this.GetInstance(typeof(DAL_noLiquidacionPreliminar), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);

            if (result.Result == ResultValue.OK)
                result = this.LiquidarNovedadesNomina();

            if (result.Result == ResultValue.OK)
            {
                #region genera la liquidación de Contrato
                //Ejecuta procedimiento almacenado para liquidar la Prenomina
                this._dal_noLiquidacionPreliminar.DAL_noLiquidacionPreliminar_LiquidarContrato(this.Empleado.ID.Value, this.DocCtrl.NumeroDoc.Value.Value, this.FechaRetiro, this.tasaCambio, this.CausaLiquidacion, out errorInd, out errorMsg);

                #region Manejo de Errores

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    result.Result = ResultValue.NOK;                                        
                    switch (errorInd)
                    {
                        case -3: { result.ResultMessage = DictionaryMessages.Err_No_noExistConceptoNom + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                   this.errorGral = true;
                                   break;
                        }
                        case -1: { result.ResultMessage = DictionaryMessages.Err_No_DatosAnuales + "&&" + this.Empresa.Descriptivo.Value; 
                                   this.errorGral = true;
                                   break;
                        }
                        case -0: { result.ResultMessage = DictionaryMessages.Err_ControlNoData + "&&" + errorMsg + "&&" + string.Empty;
                                   this.errorGral = true;
                                   break; 
                        }
                        case 1: { result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                                  this.errorGral = false;
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
