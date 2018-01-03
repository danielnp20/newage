using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Collections;
using NewAge.DTO.Negocio.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Reportes;
using SentenceTransformer;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class BalanceDePruebaPorQReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Balance De Prueba Por quarters Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Filtros">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaPorQReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, string balanceTipo, DateTime Date)
        {
            #region Report data
            //ArrayList filterFieldList = new ArrayList()
            //{
            //    "LineaPresupuestoID",
            //    "ProyectoID",
            //    "CentroCostoID"
            //};
            //Consulta = ConsultaBdPpQ(filterFieldList);

            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> data = null;
            int cuentaLength = 0;

            data = _bc.AdministrationModel.GetReportData(AppReports.coBalanceDePruebaPorQ, Consulta, consultaFields).ToList();

            List<DTO_ReportBalanceDePruebaPorQ> reportData = new List<DTO_ReportBalanceDePruebaPorQ>();

            foreach (DTO_BasicReport item in data)
            {
                DTO_ReportBalanceDePruebaPorQ itemBP = (DTO_ReportBalanceDePruebaPorQ)item;
                //if (!cuentaFuncInd)
                //    if (itemBP.CuentaID.Trim().Length == cuentaLength)
                //        itemBP.MaxLengthInd = 1;
                //    else
                //        itemBP.MaxLengthInd = 0;
                reportData.Add(itemBP);
            };

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("BalanceTipoID") && !filter.CampoFisico.Contains("@CuentaLength"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report field list
            ArrayList fieldList = null;
            switch (reportMM)
            {
                case TipoMoneda.Local:
                    fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc", 
                        "InicialML",
                        "TotalML_Q1", 
                        "TotalML_Q2", 
                        "TotalML_Q3", 
                        "TotalML_Q4", 
                        "TotalML_Year", 
                    };
                    break;

                case TipoMoneda.Foreign:
                    fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc",  
                        "InicialME", 
                        "TotalME_Q1", 
                        "TotalME_Q2", 
                        "TotalME_Q3", 
                        "TotalME_Q4", 
                        "TotalME_Year",
                    };
                    break;

                case TipoMoneda.Both:
                    fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc", 
                        "InicialML", 
                        "InicialME", 
                        "TotalML_Q1",  
                        "TotalME_Q1", 
                        "TotalML_Q2", 
                        "TotalME_Q2", 
                        "TotalML_Q3",  
                        "TotalME_Q3",
                        "TotalML_Q4", 
                        "TotalME_Q4",                         
                        "TotalML_Year", 
                        "TotalME_Year",
                    };
                    break;

            };
            #endregion

            BalanceDePruebaPorQReport report = new BalanceDePruebaPorQReport(reportData, fieldList, reportMM, balanceTipo, Date, selectedFiltersList);
            report.ShowPreview();
        }
        #endregion

        #region Funciones Privadas
        /// <summary>
        /// Default Fiters of the report
        /// </summary>
        /// <param name="filterFieldList">list of all filters applied to the report</param>
        private DTO_glConsulta ConsultaBdPpQ(ArrayList filterFieldList)
        {
            DTO_glConsulta cons = new DTO_glConsulta();
            DTO_glConsultaFiltro FiltroBdPpQ;

            foreach (string filterfieldName in filterFieldList)
            {
                FiltroBdPpQ = new DTO_glConsultaFiltro();

                FiltroBdPpQ.CampoFisico = "LEN([coBalance]." + filterfieldName + ")";
                FiltroBdPpQ.OperadorFiltro = "=";
                FiltroBdPpQ.OperadorSentencia = "AND";
                switch (filterfieldName)
                {
                    case "LineaPresupuestoID":
                        string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto);
                        Tuple<int, string> tup = new Tuple<int, string>(AppMasters.plLineaPresupuesto, empGrupo);
                        DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpQ.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "ProyectoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto);
                        tup = new Tuple<int, string>(AppMasters.coProyecto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpQ.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "CentroCostoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto);
                        tup = new Tuple<int, string>(AppMasters.coCentroCosto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpQ.ValorFiltro = table.CodeLength(1).ToString();
                        break;
                };
                cons.Filtros.Add(FiltroBdPpQ);
            };
            return cons;
        }
        #endregion
    }
}
