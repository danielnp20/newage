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
using System.Threading;
using NewAge.DTO.Resultados;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ParametersCertificates : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Hilo, Genera el PDF
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                if(_impuestoInfo != "-")
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_CertificadoReteFuente(this._Periodo, this._impuestoInfo, this._formatType);
                else
                     MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Pr_NoSeGeneroReporte));

                #region Genera el  El reporte

                //Genera el PDF
                if (reportName.Result == ResultValue.OK)
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ExtraField);
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                    MessageBox.Show(msg);
                    return;
                }

                //Verifica q se halla ejecutado con exito la generacion del PDF para mostrarlo al usuario
                if (!string.IsNullOrEmpty(fileURl))
                    Process.Start(fileURl);
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidInputReportData));
                    return;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParametersCertificates.cs", "LoadReportMethod_PDF"));
                throw;
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        private string _impuestoInfo = "-";
        private string _certificado = string.Empty;
        DTO_TxResult reportName = null;
        string fileURl = string.Empty;

        //Variables para Hilo
        DateTime _Periodo;

        #endregion

        #region Funciones Privadad

        /// <summary>
        /// Funcion que se encarga de desahbilitar los controles que no se utilizan
        /// </summary>
        private void EnableControls()
        {
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public ParametersCertificates()
        {
            this.Module = ModulesPrefix.co;
            this.ReportForm = AppReportParametersForm.coCertificates;
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.coCertificates).ToString());

            #region Configurar Opciones

            #region Cargo los combos del reporte

            // Carga El combo de impuesto
            long count = 0;
            List<ReportParameterListItem> impuestos = new List<ReportParameterListItem>() { new ReportParameterListItem() { Key = "-", Desc = "-" } };

            count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coImpuestoDeclaracion, null, null, true);
            IEnumerable<DTO_MasterBasic> DeclaracionesImpuesto = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coImpuestoDeclaracion, count, 1, null, null, true);

            foreach (var declar in DeclaracionesImpuesto)
                impuestos.Add(new ReportParameterListItem() { Key = declar.ID.ToString(), Desc = declar.ID.ToString() + " - " + declar.Descriptivo.ToString() });

            //Carga el combo con el tipo de formato que desea exportar
            List<ReportParameterListItem> certificateReportType = new List<ReportParameterListItem>()
                        {
                            new ReportParameterListItem() { Key = "Certificate", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Certificate) },
                            new ReportParameterListItem() { Key = "Soporte", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Soporte) }, 
                        };


            #endregion

            this.AddList("Impuesto", (AppForms.ReportForm).ToString() + "_Impuesto", impuestos, true, "-", true);
            this.AddList("certificado", (AppForms.ReportForm).ToString() + "_TipoReport", certificateReportType, true, "Declaracion", true);

            #endregion

            #region Configurar Filtros

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();

            this.EnableControls();

            #endregion
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            this.btnExportToPDF.Enabled = false;
            this.EnableControls();

            int año = Convert.ToInt16(this.periodoFilter1.txtYear.Text);
            int mes = DateTime.Now.Month;

            _Periodo = new DateTime(año, mes, DateTime.DaysInMonth(año, mes));
            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();

        }

        #endregion

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            #region Carga el tipo de impuesto

            if (option.Equals("Impuesto"))
            {
                ReportParameterList impuesto = (ReportParameterList)this.RPControls["Impuesto"];
                this._impuestoInfo = impuesto.SelectedListItem.Key;
            }

            #endregion
            #region Carga el tipo de Certificado a Imprimir

            if (option.Equals("certificado"))
            {
                ReportParameterList cetificado = (ReportParameterList)this.RPControls["certificado"];
                this._certificado = cetificado.SelectedListItem.Key;
            }

            #endregion
        }

        #endregion
    }
}
