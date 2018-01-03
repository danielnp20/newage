using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors;
using System.Collections;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Reports.Formularios;
using NewAge.DTO.Reportes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersFormularios : ReportParametersForm
    {
        #region Variables
        private DTO_coImpuestoDeclaracion _impuestoInfo;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ParametersFormularios()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coFormularios;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            if (this._impuestoInfo == null)
            {
                MessageBox.Show("Declaracion is not identified");
                return;
            }            
            
            #region Variables
            string _cuentaDesc;
            string _tercDesc;
            Dictionary<string, string> cacheCuenta = new Dictionary<string, string>();
            Dictionary<string, string> cacheTerc = new Dictionary<string, string>();  

            Dictionary<string, string[]> reportParameters = this.GetValues();
            int _year = this.periodoFilter1.Year[0];
            int _period = this.periodoFilter1.Months[0];
            #region Trae las fechas y Verifica si esta definido o preliminar
            PeriodoFiscal peroidoFisc = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), this._impuestoInfo.PeriodoDeclaracion.Value.Value.ToString());
            bool preInd = false;
            int mesIni = 0;
            int mesFin = 0;            
            switch (peroidoFisc)
            {
                case PeriodoFiscal.Anual:
                    preInd = (_year == DateTime.Now.Year) ? true : false;
                    mesIni = 1;
                    mesFin = 12; 
                    break;
                case PeriodoFiscal.Bimestral:
                    preInd = (_year == DateTime.Now.Year && _period * 2 >= DateTime.Now.Month) ? true : false;
                    mesIni = _period * 2 - 1;
                    mesFin = _period * 2; 
                    break;
                case PeriodoFiscal.Mensual:
                    preInd = (_year == DateTime.Now.Year && _period >= DateTime.Now.Month) ? true : false;
                    mesIni = _period;
                    mesFin = _period; 
                    break;
                case PeriodoFiscal.Semestral:
                    preInd = (_year == DateTime.Now.Year && _period * 6 >= DateTime.Now.Month) ? true : false;
                    mesIni = _period * 6 - 5;
                    mesFin = _period * 6; 
                    break;
                case PeriodoFiscal.Trimestral:
                    preInd = (_year == DateTime.Now.Year && _period * 3 >= DateTime.Now.Month) ? true : false;
                    mesIni = _period * 3 - 2;
                    mesFin = _period * 3; 
                    break;
            }
            #endregion
            #endregion

            List<DTO_glConsultaFiltro> userFilterList = new List<DTO_glConsultaFiltro>();
            userFilterList.Add(Filtro("impC.ImpuestoDeclID", "=", "and", "'"  + reportParameters["1"][0] + "'"));
            userFilterList.Add(Filtro("Year(aux.Fecha)", "=", "and", _year.ToString()));
            userFilterList.Add(Filtro("Month(aux.Fecha)", ">=", "and", mesIni.ToString()));
            userFilterList.Add(Filtro("Month(aux.Fecha)", "<=", "and", mesFin.ToString()));                      

            FormulariosReportBuilder fdrp = new FormulariosReportBuilder(userFilterList, _year, _period, preInd, this._impuestoInfo.ImpuestoTipoID.Value, reportParameters["2"][0]);
        } 
        #endregion

        #region Funciones Privadas
        private DTO_glConsultaFiltro Filtro(string campoFisico, string operadorFiltro, string operadorSentencia, string valorFiltro)
        {
            DTO_glConsultaFiltro filter = new DTO_glConsultaFiltro();
            filter.CampoFisico = campoFisico;
            filter.OperadorFiltro = operadorFiltro;
            filter.OperadorSentencia = operadorSentencia;
            filter.ValorFiltro = valorFiltro;

            return filter;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                if (!string.IsNullOrEmpty(reportParameters["1"][0].Trim()) && reportParameters["1"][0] != "*")
                {
                    this._impuestoInfo = (DTO_coImpuestoDeclaracion)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coImpuestoDeclaracion, false, reportParameters["1"][0], true);
                    PeriodoFiscal periodoFiscal = (PeriodoFiscal)Enum.Parse(typeof(PeriodoFiscal), this._impuestoInfo.PeriodoDeclaracion.Value.Value.ToString());
 
                    ReportParameterList listReportType = (ReportParameterList)this.RPControls["2"];
                    listReportType.RefreshList();
                    listReportType.Enabled = true;

                    this.periodoFilter1.monthCB.Name = (periodoFiscal == PeriodoFiscal.Mensual) ? "monthCB" : "monthCB_" + periodoFiscal.ToString();
                    this.periodoFilter1.monthCB.Items.Clear();
                    this.periodoFilter1.monthCB.Items.Add(1);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;
                    this.periodoFilter1.txtYear.Text = this.periodo.Date.Year.ToString();
                    this.periodoFilter1.Enabled = true;

                }
            }
        } 
        #endregion
    }
}
