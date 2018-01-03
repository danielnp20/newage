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
    class BalanceGeneralReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Balance General Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Filtros">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="balanceTipo">tipo de balance</param>
        /// <param name="Date">report period</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceGeneralReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, string balanceTipo, DateTime Date)
        {
            #region Report data
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> data = null;

            data = _bc.AdministrationModel.GetReportData(AppReports.coBalanceGeneral, Consulta, consultaFields).ToList();

            List<DTO_ReportBalanceGeneral> reportData = new List<DTO_ReportBalanceGeneral>();

            foreach (DTO_BasicReport item in data)
                reportData.Add((DTO_ReportBalanceGeneral)item);

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("BalanceTipoID") && !filter.CampoFisico.Contains("@Level"))
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
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
                        "FinalML_L6", 
                        "FinalML_L5",
                        "FinalML_L4",
                        "FinalML_L3",
                        "FinalML_L2",
                    };
                    break;

                case TipoMoneda.Foreign:
                    fieldList = new ArrayList()
                    {                
                        "CuentaID",
                        "CuentaDesc",
                        "FinalME_L6", 
                        "FinalME_L5",
                        "FinalME_L4",
                        "FinalME_L3",
                        "FinalME_L2",
                    };
                    break;
            };
            #endregion

            BalanceGeneralReport report = new BalanceGeneralReport(reportData, fieldList, reportMM, balanceTipo, Date, selectedFiltersList);
            report.ShowPreview();
        }
        #endregion

    }
}
