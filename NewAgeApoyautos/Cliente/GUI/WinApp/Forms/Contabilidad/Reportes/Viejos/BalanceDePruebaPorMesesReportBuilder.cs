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
    class BalanceDePruebaPorMesesReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Balance De Prueba Por Meses Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Filtros">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaPorMesesReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, string balanceTipo, DateTime Date)
        {
            #region Report data
            //ArrayList filterFieldList = new ArrayList()
            //{
            //    "LineaPresupuestoID",
            //    "ProyectoID",
            //    "CentroCostoID"
            //};
            //Consulta = ConsultaBdPpM(filterFieldList);

            Consulta = new DTO_glConsulta();

            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> data = null;
            int cuentaLength = 0;

            data = _bc.AdministrationModel.GetReportData(AppReports.coBalanceDePruebaPorMeses, Consulta, consultaFields).ToList();
            List<DTO_ReportBalanceDePruebaPorMeses> reportData = new List<DTO_ReportBalanceDePruebaPorMeses>();

            foreach (DTO_BasicReport item in data)
            {
                DTO_ReportBalanceDePruebaPorMeses itemBP = (DTO_ReportBalanceDePruebaPorMeses)item;
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
                        "MovML_01",                         
                        "MovML_02",
                        "MovML_03",
                        "MovML_04",
                        "MovML_05",
                        "MovML_06",
                        "MovML_07",
                        "MovML_08",
                        "MovML_09",
                        "MovML_10",
                        "MovML_11",
                        "MovML_12",
                        "TotalML_Year",
                    };
                    break;

                case TipoMoneda.Foreign:
                    fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc",  
                        "InicialME", 
                        "MovME_01", 
                        "MovME_02", 
                        "MovME_03", 
                        "MovME_04", 
                        "MovME_05", 
                        "MovME_06", 
                        "MovME_07", 
                        "MovME_08", 
                        "MovME_09",
                        "MovME_10",
                        "MovME_11", 
                        "MovME_12",
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
                        "MovML_01",
                        "MovML_02", 
                        "MovML_03",
                        "MovME_01", 
                        "MovME_02", 
                        "MovME_03",
                        "MovML_04", 
                        "MovML_05",
                        "MovML_06", 
                        "MovME_04", 
                        "MovME_05",
                        "MovME_06",
                        "MovML_07",
                        "MovML_08", 
                        "MovML_09",
                        "MovME_07", 
                        "MovME_08", 
                        "MovME_09",
                        "MovML_10",
                        "MovML_11",
                        "MovML_12", 
                        "MovME_10", 
                        "MovME_11", 
                        "MovME_12",                       
                        "TotalML_Year", 
                        "TotalME_Year",
                    };
                    break;

            };
            #endregion

            BalanceDePruebaPorMesesReport report = new BalanceDePruebaPorMesesReport(reportData, fieldList, reportMM, balanceTipo, Date, selectedFiltersList);
            report.ShowPreview();
        }
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Default Fiters of the report
        /// </summary>
        /// <param name="filterFieldList">list of all filters applied to the report</param>
        private DTO_glConsulta ConsultaBdPpM(ArrayList filterFieldList)
        {
            DTO_glConsulta cons = new DTO_glConsulta();
            DTO_glConsultaFiltro FiltroBdPpM;

            foreach (string filterfieldName in filterFieldList)
            {
                FiltroBdPpM = new DTO_glConsultaFiltro();

                FiltroBdPpM.CampoFisico = "LEN([coBalance]." + filterfieldName + ")";
                FiltroBdPpM.OperadorFiltro = "=";
                FiltroBdPpM.OperadorSentencia = "AND";
                switch (filterfieldName)
                {
                    case "LineaPresupuestoID":
                        string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto);
                        Tuple<int, string> tup = new Tuple<int, string>(AppMasters.plLineaPresupuesto, empGrupo);
                        DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpM.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "ProyectoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto);
                        tup = new Tuple<int, string>(AppMasters.coProyecto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpM.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "CentroCostoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto);
                        tup = new Tuple<int, string>(AppMasters.coCentroCosto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdPpM.ValorFiltro = table.CodeLength(1).ToString();
                        break;
                };
                cons.Filtros.Add(FiltroBdPpM);
            };
            return cons;
        }
        #endregion
    }
}
