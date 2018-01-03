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
    class BalanceDePruebaReportBuilder
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();

        protected DTO_glConsulta Consulta = null;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Builder for Balance De Prueba Report (collects and organises report data)
        /// </summary>
        /// <param name="reportMM">Moneda type of the report - local, foreign, both</param>
        /// <param name="Filtros">list of all filters applied to the report</param>
        /// <param name="consultaFields">list of filters that can be applied by user</param>
        /// <param name="balanceTipo">tipo de balance</param>
        ///  <param name="minDate">initial date of the period</param>
        /// <param name="maxDate">final date of the period</param>
        /// <param name="RompIndList">list of bool parameters to enable rompimientos (true - enabled)</param>
        /// <param name="cuentaFuncInd">indicador del tipo de la cuenta (true - cuenta Funcional;false - cuenta Alterna)</param>
        public BalanceDePruebaReportBuilder(TipoMoneda reportMM, List<DTO_glConsultaFiltro> Filtros, List<ConsultasFields> consultaFields, string balanceTipo, DateTime minDate, DateTime maxDate, List<string> RompIndList)
        {
            #region Report data
            //ArrayList filterFieldList = new ArrayList()
            //{
            //    "LineaPresupuestoID",
            //    "ProyectoID",
            //    "CentroCostoID"
            //};

            //Consulta = ConsultaBdP(filterFieldList);

            //string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coPlanCuenta);
            //Tuple<int, string> tup = new Tuple<int, string>(AppMasters.coPlanCuenta, empGrupo);
            //DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
            //int maxCuentaLength = table.CodeLength(table.LevelsUsed());

            //int cuentaLength = maxCuentaLength;
            //if (!numberOfDigits.Contains("Max"))
            //{
            //    cuentaLength = 0;
            //    int i = 1;
            //    while (table.CodeLength(i) < Convert.ToInt16(numberOfDigits))
            //    {
            //        cuentaLength=table.CodeLength(i); 
            //        i++;
            //    }
            //};                

            Consulta = new DTO_glConsulta();
            Consulta.Filtros.AddRange(Filtros);

            List<DTO_BasicReport> data = null;
            data = _bc.AdministrationModel.GetReportData(AppReports.coBalanceDePrueba, Consulta, consultaFields).ToList();

            DTO_BasicReport.FillRompimiento(data, RompIndList[0], RompIndList[1]);

            List<DTO_ReportBalanceDePrueba> reportData = new List<DTO_ReportBalanceDePrueba>();
            foreach (DTO_BasicReport item in data)
            {
                DTO_ReportBalanceDePrueba itemBP = (DTO_ReportBalanceDePrueba)item;
                //if (!cuentaFuncInd && numberOfDigits.Contains("Max"))
                //    if (itemBP.CuentaID.Trim().Length == cuentaLength)
                //        itemBP.MaxLengthInd = 1;
                //    else
                //        itemBP.MaxLengthInd = 0;
                //if (!numberOfDigits.Contains("Max") && itemBP.CuentaID.Trim().Length == maxCuentaLength)
                //{
                //    itemBP.CuentaID = (itemBP.CuentaID.Trim()).Substring(0, cuentaLength - 1);
                //}                
                reportData.Add(itemBP);
            };

            reportData = reportData.OrderBy(x => x.CuentaID).ToList();

            #region Signos de los totales
            decimal vlrIniCuentaCreditoML = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.InicialML : 0));
            decimal vlrIniCuentaDebitoML = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.InicialML : 0));
            decimal vlrIniCuentaCreditoME = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.InicialME : 0));
            decimal vlrIniCuentaDebitoME = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.InicialME : 0));

            decimal vlrFinCuentaCreditoML = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.FinalML : 0));
            decimal vlrFinCuentaDebitoML = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.FinalML : 0));
            decimal vlrFinCuentaCreditoME = Math.Abs(reportData.Sum(x => (x.Signo < 0) ? x.FinalME : 0));
            decimal vlrFinCuentaDebitoME = Math.Abs(reportData.Sum(x => (x.Signo > 0) ? x.FinalME : 0));

            int signIni = 1;
            int signFin = 1;
            if (vlrIniCuentaCreditoML > vlrIniCuentaDebitoML || vlrIniCuentaCreditoME > vlrIniCuentaDebitoME)
                signIni = -1;
            if (vlrFinCuentaCreditoML > vlrFinCuentaDebitoML || vlrFinCuentaCreditoME > vlrFinCuentaDebitoME)
                signFin = -1;
            #endregion

            List<bool> RompIndList_bool = new List<bool>()
            {
                (string.IsNullOrWhiteSpace(RompIndList[0]))? false :  true,
                (string.IsNullOrWhiteSpace(RompIndList[1]))? false :  true,
            };

            List<string> selectedFiltersList = new List<string>();
            foreach (DTO_glConsultaFiltro filter in Filtros)
            {
                if (!filter.CampoFisico.Contains("PeriodoID") && !filter.CampoFisico.Contains("BalanceTipoID") && !filter.CampoFisico.Contains("@MesIni") && !filter.CampoFisico.Contains("@MesFin")
                    && !filter.CampoFisico.Contains("@CuentaLength") && !filter.CampoFisico.Contains("@SaldoInicial") && !filter.CampoFisico.Contains("@EmpresaID")
                    && !filter.CampoFisico.Contains("@cuentaIni") && !filter.CampoFisico.Contains("@cuentaFin"))
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
                        "InicialML",
                        "DebitoML", 
                        "CreditoML",
                        "FinalML",
                    };
                    break;
                case TipoMoneda.Foreign:
                    fieldList = new List<string>() 
                    {                  
                        "CuentaID",
                        "CuentaDesc",
                        "InicialME",
                        "DebitoME", 
                        "CreditoME",
                        "FinalME"
                    };
                    break;
                case TipoMoneda.Both:
                    fieldList = new List<string>() 
                    {                  
                        "CuentaID",
                        "CuentaDesc", 
                        "InicialML",
                        "MovimientoML",                
                        "FinalML",
                        "InicialME",
                        "MovimientoME",
                        "FinalME"
                    };
                    break;

            };
            #endregion

            BalanceDePruebaReport report = new BalanceDePruebaReport(reportData, fieldList, reportMM, balanceTipo, minDate, maxDate, selectedFiltersList, RompIndList_bool, signIni, signFin);
            report.ShowPreview();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Default Fiters of the report
        /// </summary>
        /// <param name="filterFieldList">list of all filters applied to the report</param>
        private DTO_glConsulta ConsultaBdP(ArrayList filterFieldList)
        {
            DTO_glConsulta cons = new DTO_glConsulta();
            DTO_glConsultaFiltro FiltroBdP;

            foreach (string filterfieldName in filterFieldList)
            {
                FiltroBdP = new DTO_glConsultaFiltro();

                FiltroBdP.CampoFisico = "LEN([coBalance]." + filterfieldName + ")";
                FiltroBdP.OperadorFiltro = "=";
                FiltroBdP.OperadorSentencia = "AND";
                switch (filterfieldName)
                {
                    case "LineaPresupuestoID":
                        string empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.plLineaPresupuesto);
                        Tuple<int, string> tup = new Tuple<int, string>(AppMasters.plLineaPresupuesto, empGrupo);
                        DTO_glTabla table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdP.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "ProyectoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coProyecto);
                        tup = new Tuple<int, string>(AppMasters.coProyecto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdP.ValorFiltro = table.CodeLength(1).ToString();
                        break;

                    case "CentroCostoID":
                        empGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(AppMasters.coCentroCosto);
                        tup = new Tuple<int, string>(AppMasters.coCentroCosto, empGrupo);
                        table = _bc.AdministrationModel.Tables[tup];
                        FiltroBdP.ValorFiltro = table.CodeLength(1).ToString();
                        break;
                };
                cons.Filtros.Add(FiltroBdP);
            };
            return cons;
        }
        #endregion
    }
}
