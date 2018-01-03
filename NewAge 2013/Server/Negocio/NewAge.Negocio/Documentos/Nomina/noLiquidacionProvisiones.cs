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
    public class noLiquidacionProvisiones : noLiquidacionBase
    {
        public noLiquidacionProvisiones(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn) : base(conn, tx, emp, userID, loggerConn) { }

        [Transaction]
        public override DTO_TxResult Liquidar()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            string periodo = this.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo);
            this.DocumentoID = AppDocuments.Provisiones;
            this.Periodo = Convert.ToDateTime(periodo);
            this._dal_noProvisionDeta = (DAL_noProvisionDeta)this.GetInstance(typeof(DAL_noProvisionDeta), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            result = this.GenerateDocument();
            
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion, string.Empty, true);
            this._moduloGlobal.glDocumentoControl_ChangeDocumentStatus(this.DocumentoID, this.DocCtrl.NumeroDoc.Value.Value, EstadoDocControl.Aprobado, string.Empty, true);

            if (result.Result == ResultValue.OK)
            {
                 this._dal_noProvisionDeta.DAL_noProvisionDeta_LiquidarProvisiones(this.Empleado.ID.Value,
                                                                                    this.DocCtrl.NumeroDoc.Value.Value,
                                                                                    this.tasaCambio,
                                                                                    out errorInd,
                                                                                    out errorMsg);

                #region Manejo de Errores

                switch(errorInd)
                {                    
                    case -1:
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_No_DatosAnuales + "&&" + this.Empresa.Descriptivo.Value;
                            break;
                        }
                    case 1:
                        {
                            result.Result = ResultValue.NOK;
                            result.ResultMessage = DictionaryMessages.Err_No_GenerarDocumento + "&&" + this.Empleado.ID.Value + "&&" + errorMsg;
                            break;
                        }
                }               

                #endregion
            }
            return result;
        }
    }
}
