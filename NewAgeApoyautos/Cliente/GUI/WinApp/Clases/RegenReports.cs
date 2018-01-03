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
                        break;
                    case AppDocuments.TransferenciasBancarias:
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
                        break;
                    case AppDocuments.TransaccionAutomatica:
                        break;
                    case AppDocuments.NotaEnvio:
                        break;
                }

                if (documentID != AppDocuments.DesembolsoFacturas && documentID != AppDocuments.TransferenciasBancarias)
                    Process.Start(fileURl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RegenReports.cs", "GenerarReporte"));
            }
        }

    }
}
