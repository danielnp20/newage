using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Reports;
using System.Windows.Forms;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;
using System.Collections;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Forms;
using NewAge.ReportesComunes;
using NewAge.DTO.Reportes;
using System.Drawing.Design;
using NewAge.DTO.Negocio;
using NewAge.Reports.Dinamicos;

namespace NewAge.Cliente.GUI.WinApp.Clases
{
    internal class CustomizeReport
    {
        BaseController _bc = BaseController.GetInstance();
        private ReportForm ribbonForm;
        private int documentID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="docID">Identificador del documento que contiene el reporte</param>
        public CustomizeReport(int docID)
        {
            try
            {
                this.ribbonForm = new ReportForm();
                this.documentID = docID;
                bool showReport = true;

                #region region Carga la info del reporte

                // Create a design form and get its MDI controller.
                XRDesignForm form = new XRDesignForm();
                ribbonForm.mdiController = form.DesignMdiController;

                // Handle the DesignPanelLoaded event of the MDI controller,
                // to override the SaveCommandHandler for every loaded report.
                ribbonForm.mdiController.DesignPanelLoaded += new DesignerLoadedEventHandler(mdiController_DesignPanelLoaded);

                byte[] arr = _bc.AdministrationModel.aplReporte_GetByID(this.documentID);
                if (arr != null)
                {
                    XtraReport customReport = new ReporteDinamico(_bc);
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                        customReport.LoadLayout(memoryStream);

                    ribbonForm.mdiController.OpenReport(customReport);
                }
                else
                    showReport = this.GetReportData();

                // Show the form.
                if (showReport)
                {
                    form.ShowDialog();
                    if (ribbonForm.mdiController.ActiveDesignPanel != null)
                        ribbonForm.mdiController.ActiveDesignPanel.CloseReport();
                }

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CustomizeReport.cs", "CustomizeReport"));
            }

        }

        /// <summary>
        /// Obtiene la info del reporte
        /// </summary>
        private bool GetReportData()
        {
            ArrayList fieldList = new ArrayList();

            switch (this.documentID)
            {
                case AppDocuments.DocumentoContable:
                    #region Documento Contable
                    fieldList.AddRange(ColumnsInfo.ComprobanteFields);

                    DocumentoContableReport report = new DocumentoContableReport(AppReports.coDocumentoContable, new List<DTO_ReportDocumentoContable>(),
                        _bc.AdministrationModel.MultiMoneda, fieldList, _bc);

                    // Open an empty report in the form.
                    ribbonForm.mdiController.OpenReport(report);
                    #endregion
                    return true;
                case AppReports.coReporteEstadoResultados:
                    #region Reporte Ingresos
                    Report_Co_ReporteLineasV ing = new Report_Co_ReporteLineasV(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(ing);
                    #endregion
                    return true;
                case AppReports.coReporteSituacionFinanciero:
                    #region Reporte Ingresos
                    Report_Co_ReporteLineasV bal = new Report_Co_ReporteLineasV(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(bal);
                    #endregion
                    return true;
                case AppDocuments.FacturaVenta:
                    #region Factura Venta
                    Report_Fa_FacturaVenta facVenta = new Report_Fa_FacturaVenta(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(facVenta);
                    #endregion
                    return true;
                case AppReports.faCuentaCobro:
                    #region Cuenta de Cobro
                    Report_Fa_FacturaVenta ctaCobro = new Report_Fa_FacturaVenta(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(ctaCobro);
                    #endregion
                    return true;
                case AppReports.faFacturasVentaAIU:
                    #region Factura Venta AIU
                    Report_Fa_FacturaVenta facVentaAIU = new Report_Fa_FacturaVenta(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(facVentaAIU);
                    #endregion
                    return true;
                case 32314:
                    #region Incorporaciones

                    Report_Cc_Incorporacion incorp = new Report_Cc_Incorporacion(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(incorp);

                    #endregion
                    return true;
                case (90):
                    #region Balance General
                    fieldList = fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc", 
                        "FinalML_L6", 
                        "FinalML_L5",
                        "FinalML_L4",
                        "FinalML_L3",
                        "FinalML_L2",
                    };
                    //AppReports.BalanceGeneral, new List<DTO_ReportBalanceGeneral>() { new DTO_ReportBalanceGeneral() }, fieldList, _bc
                    List<DTO_ReportBalanceGeneral> data = new List<DTO_ReportBalanceGeneral>();
                    List<string> filtros = new List<string>();
                    BalanceGeneralReport reporteGeneral = new BalanceGeneralReport(data, fieldList, TipoMoneda.Local, "FUNC", DateTime.Now, filtros);
                    
                    // Open an empty report in the form.
                    ribbonForm.mdiController.OpenReport(reporteGeneral);
                    #endregion
                    return true;
                case AppDocuments.OrdenCompra:
                    #region Orden Compra
                    Report_Pr_OrdenCompra ordCompra = new Report_Pr_OrdenCompra(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(ordCompra);
                    #endregion
                    return true;
                case AppReports.prOrdenCompraAnexo:
                    #region Orden Compra Anexo
                    Report_Pr_OrdenCompra ordCompraAnexo = new Report_Pr_OrdenCompra(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(ordCompraAnexo);
                    #endregion
                    return true;
                case AppReports.prOrdenCompraServicio:
                    #region Orden Compra Servicio
                    Report_Pr_OrdenCompra ordCompraServ = new Report_Pr_OrdenCompra(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(ordCompraServ);
                    #endregion
                    return true;
                case AppReports.tsPagoFacturasBanco1:
                    #region Pago Factura
                    Report_Ts_PagoFacturas pagoFact = new Report_Ts_PagoFacturas(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(pagoFact);
                    #endregion
                    return true;
                case AppReports.tsTransferenciaBancos:
                    #region TransferenciaBancos
                    Report_Ts_TransaccionBancaria transferBanco = new Report_Ts_TransaccionBancaria(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(transferBanco);
                    #endregion
                    return true;
                case AppReports.prSolicitudDoc:
                    #region Solicitud
                    Report_Pr_SolicitudRecibido solicitudProv = new Report_Pr_SolicitudRecibido(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(solicitudProv);
                    #endregion
                    return true;
              
                case AppReports.prRecibidoDoc:
                    #region RecibidoProv
                    Report_Pr_SolicitudRecibido recibidoProv = new Report_Pr_SolicitudRecibido(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(recibidoProv);
                    #endregion
                    return true;
                case AppReports.ccLiquidacionCredito:
                    #region Liquidacion Credito Cartera
                    Report_Cc_LiquidacionCredito liqCredito = new Report_Cc_LiquidacionCredito(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(liqCredito);
                    #endregion
                    return true;
                case AppReports.ccInformesSIGCOOP:
                    #region Formato Pagare
                    //Report_Cc_Pagare pagare = new Report_Cc_Pagare(_bc.AdministrationModel.Empresa);
                    //ribbonForm.mdiController.OpenReport(pagare);
                    #endregion
                    return true;
                case AppReports.pySolicitudProyecto:
                    #region Planeacion costos Presup
                    Report_py_PlaneacionCostos pagare = new Report_py_PlaneacionCostos(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(pagare);
                    #endregion
                    return true;
                case AppReports.pySolicitudProyectoCompara:
                    #region Planeacion costos Compar 
                    Report_py_PlaneacionCostosCompar comp = new Report_py_PlaneacionCostosCompar(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(comp);
                    #endregion
                    return true;
                case AppReports.pySolicitudProyectoDetallado:
                    #region Planeacion costos Detallado
                    Report_py_PlaneacionCostosDetallado solDetallado = new Report_py_PlaneacionCostosDetallado(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(solDetallado);
                    #endregion
                    return true;
                case AppReports.ccCertificadoDeuda:
                    #region Ceertificado Deuda
                    Report_Cc_CertificadoDeuda certDeuda = new Report_Cc_CertificadoDeuda(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(certDeuda);
                    #endregion
                    return true;
                case AppReports.ccCertificadoPazYSalvo:
                    #region Certificado Paz y Salvo
                    Report_Cc_PazYSalvo certPaz = new Report_Cc_PazYSalvo(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(certPaz);
                    #endregion
                    return true;
                case AppReports.ccCertificadoPagosAlDia:
                    #region Certificado Pagos al dia
                    Report_Cc_CertificadoPagosAlDia certPagos = new Report_Cc_CertificadoPagosAlDia(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(certPagos);
                    #endregion
                    return true;
                case AppReports.ccCertificadoRelacionPagos:
                    #region Certificado Relacion Pagos 
                    Report_Cc_CertificadoPagosAlDia certRelacionPagos = new Report_Cc_CertificadoPagosAlDia(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(certRelacionPagos);
                    #endregion
                    return true;
                case AppReports.ccContratoMutuo:
                    #region Contrato mutuo
                    Report_Cc_ContratoMutuo mutuo = new Report_Cc_ContratoMutuo(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(mutuo);
                    #endregion
                    return true;
                case AppReports.ccAutDescuento:
                    #region Autorizacion Descuento
                    Report_Cc_AutorizacionDescuento Recaudo = new Report_Cc_AutorizacionDescuento(_bc.AdministrationModel.Empresa);
                    ribbonForm.mdiController.OpenReport(Recaudo);
                    #endregion
                    return true;
                default:
                    MessageBox.Show("Reporte sin implementacion");
                    return false;
            }
        }

        /// <summary>
        /// Carga eventos del diseñaror de reportes al abrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mdiController_DesignPanelLoaded(object sender, DesignerLoadedEventArgs e)
        {
            XRDesignPanel panel = (XRDesignPanel)sender;
            ribbonForm.mdiController.AddCommandHandler(new ReportsCommands(panel, this.documentID));

            //// Get the toolbox service.
            //IToolboxService ts = (IToolboxService)e.DesignerHost.GetService(typeof(IToolboxService));

            //// Get a collection of toolbox items.
            //ToolboxItemCollection coll = ts.GetToolboxItems();

            //// Iterate through toolbox items.
            //foreach (ToolboxItem item in coll)
            //    item.DisplayName = "Cool " + item.DisplayName;
        }

    }
}
