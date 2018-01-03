using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Negocio.Reportes;
using NewAge.DTO.Reportes;
using System.Collections;
using SentenceTransformer;
using System.Reflection;
using NewAge.Librerias.Project;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Reports
{
    class SaldosReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas
        /// <summary>
        /// Builder for Saldos Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="RompIndList">list of bool parameters to enable rompimientos (true - enabled)</param>
        /// <param name="Filtros">list of the filters that are applied by used</param>
        /// <param name="consultaFields">list of filters that user can apply</param>
        ///  <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="tipoBalance">tipo de balance</param>
        public SaldosReportBuilder(TipoMoneda reportMM, List<string> RompIndList, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, DateTime minDate, DateTime maxDate, string tipoBalance)
        {
            #region Report data

            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> dataS = null;

            dataS = _bc.AdministrationModel.GetReportData(AppReports.coSaldos, Consulta, consultaFields).ToList();

            DTO_BasicReport.FillRompimiento(dataS, RompIndList[0], RompIndList[1], RompIndList[2]);

            List<DTO_ReportSaldos1> reportData = new List<DTO_ReportSaldos1>();
            foreach (DTO_BasicReport itemS in dataS)
            {
                reportData.Add((DTO_ReportSaldos1)itemS);
            };

            List<bool> RompIndList_bool = new List<bool>()
            {
                (string.IsNullOrWhiteSpace(RompIndList[1]))? false :  true,
                (string.IsNullOrWhiteSpace(RompIndList[2]))? false :  true,
            };

            foreach (string item in RompIndList)
            {
                switch (item)
                {
                    case "CuentaID":
                        reportData = reportData.OrderBy(x => x.CuentaID).ToList();
                        break;
                    case "CentroCostoID":
                        reportData = reportData.OrderBy(x => x.CentroCostoID).ToList();
                        break;
                    case "DocumentoID":
                        reportData = reportData.OrderBy(x => x.DocumentoID).ToList();
                        break;
                    case "LineaPresupuestoID":
                        reportData = reportData.OrderBy(x => x.LineaPresupuestoID).ToList();
                        break;
                    case "ProyectoID":
                        reportData = reportData.OrderBy(x => x.ProyectoID).ToList();
                        break;
                    case "TerceroID":
                        reportData = reportData.OrderBy(x => x.TerceroID).ToList();
                        break;
                };
            };


            #region Signos de los totales
            int signIni = 1;
            int signFin = 1;

            if (reportData.Count > 0)
            {
                decimal vlrIniCuentaCreditoML = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.InicialML : 0));
                decimal vlrIniCuentaDebitoML = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.InicialML : 0));
                decimal vlrIniCuentaCreditoME = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.InicialME : 0));
                decimal vlrIniCuentaDebitoME = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.InicialME : 0));

                decimal vlrFinCuentaCreditoML = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.FinalML : 0));
                decimal vlrFinCuentaDebitoML = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.FinalML : 0));
                decimal vlrFinCuentaCreditoME = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.FinalME : 0));
                decimal vlrFinCuentaDebitoME = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.FinalME : 0));

                if (vlrIniCuentaCreditoML > vlrIniCuentaDebitoML || vlrIniCuentaCreditoME > vlrIniCuentaDebitoME)
                    signIni = -1;
                if (vlrFinCuentaCreditoML > vlrFinCuentaDebitoML || vlrFinCuentaCreditoME > vlrFinCuentaDebitoME)
                    signFin = -1;
            }
            #endregion

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("BalanceTipoID") && !filter.CampoFisico.Contains("@Month") && !filter.CampoFisico.Contains("@EmpresaID")
                    && !filter.CampoFisico.Contains("@cuentaIni") && !filter.CampoFisico.Contains("@cuentaFin"))
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
                         RompIndList[0],
                         RompIndList[0].Remove(RompIndList[0].Length - 2, 2) + "Desc",
                        "InicialML",
                        "DebitoML", 
                        "CreditoML",
                        "FinalML"
                    };
                    break;
                case TipoMoneda.Foreign:
                    fieldList = new ArrayList() 
                    {                                                
                         RompIndList[0],
                         RompIndList[0].Replace("ID","Desc"),
                        "InicialME",
                        "DebitoME", 
                        "CreditoME",
                        "FinalME"
                    };
                    break;
                case TipoMoneda.Both:
                    fieldList = new ArrayList() 
                    {                                                 
                        RompIndList[0],
                         RompIndList[0].Remove(RompIndList[0].Length - 2, 2) + "Desc",
                        "InicialML",
                        "DebitoML", 
                        "CreditoML",
                        "FinalML",
                        "InicialME",
                        "DebitoME", 
                        "CreditoME",
                        "FinalME"
                    };
                    break;

            };
            #endregion

            SaldosReport report = new SaldosReport(reportData, fieldList, reportMM, minDate, maxDate, RompIndList_bool, tipoBalance, selectedFiltersList, signIni, signFin);
            report.ShowPreview();

        }
        #endregion
    }
}
