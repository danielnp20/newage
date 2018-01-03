using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_Prestamo : ReportParametersForm
    {
        #region Variables
        private int _documentID = 0;
        private int _numeroDoc = 0;
        private string _orden = null;
        private bool _orderByName = false;
        private String _empleadoid = null;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Prestamo()
        {
            this.Module = ModulesPrefix.no;
            this.ReportForm = AppReportParametersForm.noPrestamo; 
            this.documentReportID = AppReports.noPrestamo;
            _numeroDoc = (int)ReportForm;
            this.btnExportToXLS.Visible = true;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.periodoFilter1.Year[0].ToString();
            string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
            DateTime FechaIni = Convert.ToDateTime(fechaIniString);
            DateTime FechaFin = Convert.ToDateTime(fechaFinString);
            string reportName;
            string fileURl;

            reportName = this._bc.AdministrationModel.Report_No_Prestamo(FechaIni, FechaFin, this._orderByName, this._formatType, this._empleadoid);
            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
            Process.Start(fileURl);
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                this.periodoFilter1.Year[0].ToString();
                string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                string fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                DateTime FechaIni = Convert.ToDateTime(fechaIniString);
                DateTime FechaFin = Convert.ToDateTime(fechaFinString);

                this._Query = this._bc.AdministrationModel.Reportes_No_NominaToExcel(this.documentReportID, null, FechaIni, FechaFin, this._empleadoid, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._orderByName, null, null);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query, this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Prestamo.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
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
            #region Opc 1
            if (option.Equals("1"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                switch (reportParameters["1"][0])
                {
                    case "Nomina":
                        if (listReportType.SelectedListItem.Key == "Nomina")
                        {
                            //listReportType.RefreshList();
                            this._documentID = 81;
                        }
                        //listReportType.RemoveItem("Nomina");
                        break;
                    case "Vacaciones":
                        if (listReportType.SelectedListItem.Key == "Vacaciones")
                        {
                            //listReportType.RefreshList();
                            this._documentID = 82;
                        }
                        //listReportType.RemoveItem("Vacaciones");
                        break;
                    case "Liquidacion":
                        if (listReportType.SelectedListItem.Key == "Liquidacion")
                        {
                            //listReportType.RefreshList();
                            this._documentID = 84;
                        }
                        //listReportType.RemoveItem("Liquidacion");
                        break;
                    case "Prima":
                        if (listReportType.SelectedListItem.Key == "Prima")
                        {
                            //listReportType.RefreshList();
                            this._documentID = 83;
                        }
                        break;
                    case "Cesantias":
                        if (listReportType.SelectedListItem.Key == "Cesantias")
                        {
                            //listReportType.RefreshList();
                            this._documentID = 85;
                        }
                        //listReportType.RemoveItem("Cesantias");
                        break;
                    case "Todos":
                        if (listReportType.SelectedListItem.Key == "Todos")
                            listReportType.RefreshList();
                        //listReportType.RemoveItem("Todos");
                        break;
                    case "Prenomina":
                        if (listReportType.SelectedListItem.Key == "Prenomina")
                            listReportType.RefreshList();
                        //listReportType.RemoveItem("Prenomina");
                        break;
                }
            }
            #endregion

            #region Opc 2
            if (option.Equals("2"))
            {
                Dictionary<string, string[]> reportParameters = this.GetValues();
                    switch (reportParameters["2"][0])
                    {
                        case "True":
                            this._orderByName = true;
                            break;
                        case "False":
                            this._orderByName = false;
                            break;
                    }
            }
            #endregion
            #region Carga el filtro por Cliente
            else if (option.Equals("3"))
            {
                uc_MasterFind masterempleado = (uc_MasterFind)sender;
                this._empleadoid = masterempleado.ValidID ? masterempleado.Value : string.Empty;
            }
            #endregion
        }
        #endregion
    }
}
