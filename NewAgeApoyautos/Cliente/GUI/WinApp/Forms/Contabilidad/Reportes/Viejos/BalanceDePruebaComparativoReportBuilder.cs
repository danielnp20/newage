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
    class BalanceDePruebaComparativoReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Builder for Balance De Prueba Comparativo Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Filtros">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="balanceTipo">tipo de balance</param>
        ///  <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaComparativoReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, string balanceTipo, DateTime minDate, DateTime maxDate)
        {
            #region Report data
            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> data = null;

            data = _bc.AdministrationModel.GetReportData(AppReports.coBalanceDePruebaComparativo, Consulta, consultaFields).ToList();

            List<DTO_ReportBalanceDePruebaComparativo> reportData = new List<DTO_ReportBalanceDePruebaComparativo>();
            foreach (DTO_BasicReport item in data)
            {
                DTO_ReportBalanceDePruebaComparativo itemBP = (DTO_ReportBalanceDePruebaComparativo)item;
                //if (!cuentaFuncInd)
                //    if (itemBP.CuentaID.Trim().Length == cuentaLength)
                //        itemBP.MaxLengthInd = 1;
                //    else
                //        itemBP.MaxLengthInd = 0;
                reportData.Add(itemBP);
            };

            reportData = reportData.OrderBy(x => x.CuentaID).ToList();

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("BalanceTipoID") && !filter.CampoFisico.Contains("@Year") && !filter.CampoFisico.Contains("@Month") && !filter.CampoFisico.Contains("@CuentaLength"))
                {
                    selectedFiltersList.Add(filter.CampoFisico + " " + filter.OperadorFiltro + " " + filter.ValorFiltro);
                }
            };
            #endregion

            #region Report field list
            List<string> fieldList = null;
            switch (reportMM)
            {
                case TipoMoneda.Local:
                    fieldList = new List<string>() 
                    {                
                        "CuentaID",
                        "CuentaDesc", 
                        "MovimientoML_prev",
                        "MovimientoML_curr", 
                        "Dif_MovML",
                        "FinalML_prev",
                        "FinalML_curr",
                        "Dif_FinalML"
                    };
                    break;
                case TipoMoneda.Foreign:
                    fieldList = new List<string>() 
                    {                  
                        "CuentaID",
                        "CuentaDesc", 
                        "MovimientoME_prev",
                        "MovimientoME_curr", 
                        "Dif_MovME",
                        "FinalME_prev",
                        "FinalME_curr",
                        "Dif_FinalME"
                    };
                    break;
                //case TipoMoneda.Both:
                //    fieldList = new List<string>() 
                //    {                  
                //        "CuentaID",
                //        "CuentaDesc", 
                //        "InicialML",
                //        "MovimientoML",                
                //        "FinalML",
                //        "InicialME",
                //        "MovimientoME",
                //        "FinalME"
                //    };
                //    break;

            };
            #endregion

            BalanceDePruebaComparativoReport report = new BalanceDePruebaComparativoReport(reportData, fieldList, reportMM, balanceTipo, minDate, maxDate, selectedFiltersList);
            report.ShowPreview();
        }

        #endregion
    }
}
