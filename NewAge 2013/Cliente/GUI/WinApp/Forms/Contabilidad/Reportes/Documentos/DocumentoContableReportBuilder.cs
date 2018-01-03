using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Reportes;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Forms;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.ReportesComunes;
using NewAge.Librerias.Project;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    public class DocumentoContableReportBuilder
    {
        #region Variables
        protected DTO_glConsulta Consulta = null;
        protected bool MultiMonedaInd;        
        BaseController _bc = BaseController.GetInstance();
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Builder for Documento Contable Report (collects and organises report data)
        /// </summary>
        /// <param name="documentId">ID of the current document allowing to get information about it</param>
        /// <param name="multimoneda">MultiMoneda property of the document (true - MultiMoneda; false - not MultiMoneda)</param>
        /// <param name="data">data for the report</param>
        /// <param name="show">Indica si se debe mostrar o solo generar</param>
        /// <param name="allFields">Indica si se debe mostrar todos los campos</param>
        public DocumentoContableReportBuilder(int documentId, bool multimoneda, DTO_Comprobante data, bool show, bool allFields = false)
        {
            this.MultiMonedaInd = multimoneda;

            #region Report Data 
            DTO_ReportDocumentoContable docContableReport = new DTO_ReportDocumentoContable();
            docContableReport.CentroCostoID = data.CentroCostoID;
            docContableReport.coDocumentoID = documentId.ToString();
            docContableReport.CuentaID = data.CuentaID;
            docContableReport.DocumentoTercero = (documentId==AppDocuments.CruceCuentas&& string.IsNullOrEmpty(data.DocumentoTercero.ToString().Trim()))?data.DocumentoNro.ToString():data.DocumentoTercero;
            docContableReport.LineaPresupuestoID = data.LineaPresupuestoID;
            docContableReport.LugarGeograficoID = data.LugarGeograficoID;
            docContableReport.Observacion = data.Observacion;
            docContableReport.ProyectoID = data.ProyectoID;
            docContableReport.TerceroID = data.TerceroID;
            docContableReport.DocumentoNro = data.Header.ComprobanteNro.Value.Value.ToString();
            docContableReport.ValorDoc = string.IsNullOrWhiteSpace(data.ValorDoc) ? 0 : Convert.ToDecimal(data.ValorDoc, CultureInfo.InvariantCulture);

            docContableReport.Header = data.Header;
            docContableReport.DescMonedaOrigen = ((TipoMoneda)Convert.ToInt16(data.Header.MdaOrigen.Value)).ToString();

            DTO_glDocumentoControl rdDocCtrl = (documentId == AppDocuments.DocumentoContable) ? _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(documentId, data.TerceroID, data.DocumentoTercero) : _bc.AdministrationModel.glDocumentoControl_GetByID(data.Header.NumeroDoc.Value.Value);
            if (rdDocCtrl == null)
            {
                docContableReport.DocumentoEstado = DictionaryMessages.Error.ToString();
                docContableReport.UsuarioResp = DictionaryMessages.Error.ToString();
            }
            else
            {
                docContableReport.DocumentoEstado = ((EstadoDocControl)rdDocCtrl.Estado.Value).ToString();
                //docContableReport.UsuarioResp = rdDocCtrl.UsuarioRESP.ToString();
            };
            
            DTO_glDocumento glDocInfo = null;
            DTO_coDocumento coDocInfo = null;
            if (documentId == AppDocuments.DocumentoContable)
            {
                coDocInfo = (DTO_coDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coDocumento, new UDT_BasicID() { Value = docContableReport.coDocumentoID }, true);
                docContableReport.DescDocumento = ((DTO_coDocumento)coDocInfo).Descriptivo.ToString();
            }
            else
            {
                glDocInfo = (DTO_glDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glDocumento, new UDT_BasicID() { Value = docContableReport.coDocumentoID }, true);
                docContableReport.DescDocumento = ((DTO_glDocumento)glDocInfo).Descriptivo.ToString();
                docContableReport.DocAjustadoID = data.coDocumentoID; 
                coDocInfo = (DTO_coDocumento)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coDocumento,new UDT_BasicID() { Value = docContableReport.DocAjustadoID }, true);
                docContableReport.DocAjustadoDesc = ((DTO_coDocumento)coDocInfo).Descriptivo.ToString();
            };    

            DTO_coPlanCuenta cuentaInfo = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = docContableReport.CuentaID }, true);
            docContableReport.DescCuenta = ((DTO_coPlanCuenta)cuentaInfo).Descriptivo.ToString();

            DTO_coCentroCosto centroCostoInfo = (DTO_coCentroCosto)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coCentroCosto, new UDT_BasicID() { Value = docContableReport.CentroCostoID }, true);
            docContableReport.DescCentroCosto = ((DTO_coCentroCosto)centroCostoInfo).Descriptivo.ToString();

            DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = docContableReport.TerceroID }, true);
            docContableReport.DescTercero = ((DTO_coTercero)terceroInfo).Descriptivo.ToString();

            DTO_coProyecto proyectoInfo = (DTO_coProyecto)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coProyecto, new UDT_BasicID() { Value = docContableReport.ProyectoID }, true);
            docContableReport.DescProyecto = ((DTO_coProyecto)proyectoInfo).Descriptivo.ToString();

            DTO_glLugarGeografico lugarGeograficoInfo = (DTO_glLugarGeografico)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glLugarGeografico, new UDT_BasicID() { Value = docContableReport.LugarGeograficoID }, true);
            docContableReport.DescLugarGeografico = ((DTO_glLugarGeografico)lugarGeograficoInfo).Descriptivo.ToString();

            docContableReport.DescMonedaTransac = (_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.glMoneda, new UDT_BasicID() { Value = docContableReport.Header.MdaTransacc.Value }, true)).Descriptivo.ToString();
            
            docContableReport.footerReport = new List<DTO_ReportDocumentoContableFooter>();
            foreach (DTO_ComprobanteFooter footer in data.Footer)
            {
                DTO_ReportDocumentoContableFooter docContableFooter = new DTO_ReportDocumentoContableFooter(footer);
                DTO_coPlanCuenta cuenta = (DTO_coPlanCuenta)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coPlanCuenta, new UDT_BasicID() { Value = footer.CuentaID.Value }, true);
                docContableFooter.Debito = cuenta.Naturaleza.Value.Equals((byte)NaturalezaCuenta.Debito);
                docContableReport.footerReport.Add(docContableFooter);
            };
            
            #region Crea el ultimo registro
            DTO_ReportDocumentoContableFooter last = new DTO_ReportDocumentoContableFooter();
            last.Index = docContableReport.footerReport.Count;
            last.CuentaID.Value = docContableReport.CuentaID;
            last.TerceroID.Value = docContableReport.TerceroID;
            last.ProyectoID.Value = docContableReport.ProyectoID;
            last.CentroCostoID.Value = docContableReport.CentroCostoID;
            last.LineaPresupuestoID.Value = docContableReport.LineaPresupuestoID;
            last.ConceptoCargoID.Value = string.IsNullOrEmpty(cuentaInfo.ConceptoCargoID.Value) ? _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto) : cuentaInfo.ConceptoCargoID.Value;
            last.LugarGeograficoID.Value = docContableReport.LugarGeograficoID;
            last.Descriptivo.Value = docContableReport.Observacion;

            last.vlrBaseML.Value = 0;
            last.vlrMdaLoc.Value = -1 * docContableReport.footerReport.Sum(x=>x.vlrMdaLoc.Value.Value);
            last.vlrMdaExt.Value = -1 * docContableReport.footerReport.Sum(x => x.vlrMdaExt.Value.Value);

            docContableReport.footerReport.Add(last);
            #endregion

            List<DTO_ReportDocumentoContable> docContableList = new List<DTO_ReportDocumentoContable>();
            docContableList.Add(docContableReport);
            #endregion
            #region  Report field list
            ArrayList fieldList = new ArrayList();
            fieldList.AddRange(ColumnsInfo.ComprobanteFields);

            if (MultiMonedaInd)
            {
                fieldList.Add("DebitoME");
                fieldList.Add("CreditoME");
            };
            #endregion

            byte[] arr = _bc.AdministrationModel.aplReporte_GetByID(documentId);
            if (arr != null)
            {
                ReportForm ribbonForm = new ReportForm();
                XtraReport customReport = new NewAge.ReportesComunes.ReporteDinamico(_bc);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                    customReport.LoadLayout(memoryStream);

                customReport.DataSource = docContableList;
                if (show)
                    customReport.ShowPreview();
            }
            else
            {
                DocumentoContableReport report =
                    new DocumentoContableReport((documentId == AppDocuments.DocumentoContable) ? AppReports.coDocumentoContable : AppReports.coAjusteSaldos,
                        docContableList, this.MultiMonedaInd, fieldList, _bc);

                if (show)
                    report.ShowPreview();
            }
        } 

        #endregion
    }
}
