using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.GlobalConfig;
using NewAge.DTO.Reportes;
using System.Collections;
using SentenceTransformer;
using System.Reflection;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Reports.Documentos
{
    class FacturaEquivalenteBuilder
    {
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;

        /// <summary>
        /// Builder for Factura equivalente (collects and organises data)
        /// </summary>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        /// <param name="Date">period for the reported documents</param>
        public FacturaEquivalenteBuilder(List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime Date)
       {
            #region Report data
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> dataFE = _bc.AdministrationModel.GetReportData(AppReports.FacturaEquivalente, Consulta, consultaFields).ToList();

            DTO_BasicReport.FillRompimiento(dataFE, "FactEquivalente");


            List<DTO_FacturaEquivalente> reportData = new List<DTO_FacturaEquivalente>();
            foreach (DTO_BasicReport item in dataFE)
            {
                DTO_FacturaEquivalente itemEF = (DTO_FacturaEquivalente)item;
                foreach (DTO_FacturaDetail itemDet in itemEF.FacturaDetail)
                {
                    itemDet.IvaTeorico_det = (itemDet.ImpuestoTipoID == _bc.GetControlValueByCompany(ModulesPrefix.cp, GlobalControl.cp_Key_CodigoIVA)) ? itemDet.IvaTeorico_det : 0;
                    itemDet.ValorImp_det = (itemDet.ImpuestoTipoID == _bc.GetControlValueByCompany(ModulesPrefix.cp, GlobalControl.cp_Key_CodigoReteIVA)) ? -(itemDet.ValorImp_det) : 0;
                };
                foreach (DTO_FacturaHeader itemHead in itemEF.FacturaHeader)
                {
                    itemHead.IvaTeorico = itemEF.FacturaDetail.Sum(x => x.IvaTeorico_det);
                    itemHead.ValorImp = itemEF.FacturaDetail.Sum(x => x.ValorImp_det);
                    if (itemHead.TarifaRet == 0) itemHead.TarifaRet = itemHead.IvaTeorico / itemHead.ValorMisma;
                };
                reportData.Add(itemEF);
            };

            _bc.GetControlValueByCompany(ModulesPrefix.cp, GlobalControl.cp_Key_CodigoIVA);

            reportData = reportData.OrderBy(x => x.TerceroNit).ToList();

            #endregion

            #region Report field list
            ArrayList headerFieldList = new ArrayList() 
            { 
                "DocumentoDesc",
                "ValorMisma",
                "IvaTeorico",
                "TarifaRet",
                "ValorImp",
            };            

            ArrayList detailFieldList = new ArrayList()
            {
                "CuentaID",
                "CuentaDesc",
                "DebitoML",
                "CreditoML",
            };
            #endregion

            FacturaEquivalente report = new FacturaEquivalente(reportData, headerFieldList, detailFieldList, Date);
            report.ShowPreview();

       }
    }
}
