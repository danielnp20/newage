using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Windows.Forms;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    public class RegenReports
    {
        private BaseController _bc = BaseController.GetInstance();

        /// <summary>
        /// Regenera un reporte
        /// </summary>
        /// <param name="documentID">Documento generador del reporte</param>
        /// <param name="numeroDoc">Identificador del reporte</param>
        internal void GenerarReporte(int documentID, int numeroDoc, List<string> extraParams, bool isApprove = true)
        {
            try
            {
                string reportName = string.Empty;
                string fileURl = string.Empty;

                switch (documentID)
                {
                    //Cartera
                    case AppDocuments.LiquidacionCredito:
                        int libranza = Convert.ToInt32(extraParams[0]);
                        reportName = this._bc.AdministrationModel.ReportesCartera_Cc_LiquidacionCredito(libranza, ExportFormatType.pdf, numeroDoc);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.RecaudosManuales:
                        this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numeroDoc);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null);
                        break;
                    case AppDocuments.RecaudosMasivos:
                        this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numeroDoc);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null);
                        break;
                    case AppDocuments.PagosTotales:
                        this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numeroDoc);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null);
                        break;
                    //CXP
                    case AppDocuments.CausarFacturas:
                        reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(numeroDoc, isApprove,false, ExportFormatType.pdf);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.NotaCreditoCxP:
                        reportName = this._bc.AdministrationModel.Reportes_Cp_CausacionFacturas(numeroDoc, isApprove, true, ExportFormatType.pdf);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    //Tesoreria
                    case AppDocuments.DesembolsoFacturas:
                        DTO_tsBancosDocu docu = this._bc.AdministrationModel.tsBancosDocu_Get(numeroDoc);
                        DTO_tsBancosCuenta bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, docu.BancoCuentaID.Value, true);
                        int docReporte = bancoCuenta != null && !string.IsNullOrEmpty(bancoCuenta.DocumentoID.Value) ? Convert.ToInt32(bancoCuenta.DocumentoID.Value) : documentID;
                        reportName = this._bc.AdministrationModel.ReportesTesoreria_PagosFactura(docReporte, numeroDoc, ExportFormatType.pdf);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.TransferenciasBancarias:
                        DTO_tsBancosDocu docuTransf = this._bc.AdministrationModel.tsBancosDocu_Get(numeroDoc);
                        DTO_tsBancosCuenta bancoCuentaTrans = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, docuTransf.BancoCuentaID.Value, true);
                        int docReporteTrans = bancoCuentaTrans != null && !string.IsNullOrEmpty(bancoCuentaTrans.DocumentoID.Value) ? Convert.ToInt32(bancoCuentaTrans.DocumentoID.Value) : documentID;
                        reportName = this._bc.AdministrationModel.ReportesTesoreria_PagosFactura(docReporteTrans, numeroDoc, ExportFormatType.pdf);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.ReciboCaja:
                        this._bc.AdministrationModel.Report_Ts_ReciboCajaDoc(documentID, numeroDoc);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null);
                        break;
                    //Facturacion
                    case AppDocuments.FacturaVenta:
                        reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(AppDocuments.FacturaVenta, numeroDoc.ToString(), isApprove, ExportFormatType.pdf, 0, 0, 0);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    //Inventarios
                    case AppDocuments.TransaccionManual:
                        DTO_MvtoInventarios trans = this._bc.AdministrationModel.Transaccion_Get(documentID,numeroDoc);
                        DTO_inMovimientoTipo tipoMvto = trans != null? (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple,AppMasters.inMovimientoTipo,false,trans.Header.MvtoTipoInvID.Value,true) : new DTO_inMovimientoTipo();
                        reportName = this._bc.AdministrationModel.Reports_In_TransaccionMvto(null, documentID, numeroDoc, tipoMvto.TipoMovimiento.Value.Value);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.TransaccionAutomatica:
                        DTO_MvtoInventarios transAutom = this._bc.AdministrationModel.Transaccion_Get(documentID, numeroDoc);
                        DTO_inMovimientoTipo tipoMvtoAutom = transAutom != null ? (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, transAutom.Header.MvtoTipoInvID.Value, true) : new DTO_inMovimientoTipo();
                        reportName = this._bc.AdministrationModel.Reports_In_TransaccionMvto(null, documentID, numeroDoc, tipoMvtoAutom.TipoMovimiento.Value.Value);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    case AppDocuments.NotaEnvio:
                        DTO_MvtoInventarios notaEnv = this._bc.AdministrationModel.Transaccion_Get(documentID, numeroDoc);
                        DTO_inMovimientoTipo tipoMvtoNota = notaEnv != null ? (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, notaEnv.Header.MvtoTipoInvID.Value, true) : new DTO_inMovimientoTipo();
                        reportName = this._bc.AdministrationModel.Reports_In_TransaccionMvto(null, documentID, numeroDoc, tipoMvtoNota.TipoMovimiento.Value.Value);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName.ToString());
                        break;
                    //Proveedores
                    case AppDocuments.Solicitud:
                        reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(documentID, numeroDoc, false, 0);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName);
                        break;
                    case AppDocuments.OrdenCompra:
                        reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompra(numeroDoc,1,true);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName);
                        break;
                    case AppDocuments.Recibido:
                        reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(documentID, numeroDoc, false, 0);
                        fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName);
                        break;
                }

                if (documentID != AppDocuments.DesembolsoFacturas && documentID != AppDocuments.TransferenciasBancarias && documentID != AppDocuments.OrdenCompra)
                    Process.Start(fileURl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RegenReports.cs", "GenerarReporte"));
            }
        }

    }
}
