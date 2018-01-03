using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Reports;
using SentenceTransformer;
using NewAge.Librerias.Project;
using DevExpress.XtraEditors;
using System.Diagnostics;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_DocuPendientes : ReportParametersForm
    {
        #region Hilos

        /// <summary>
        /// Fucion que genera el reporte
        /// </summary>
        protected override void LoadReportMethod_PDF()
        {
            try
            {
                reportName = this._bc.AdministrationModel.ReportesGlobal_DocumentosPendiente(this._fechaIni,this._tipoReport, this._modulo,this._documentID, this._formatType);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DocuPendientes.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        private DTO_TxResult reportName;
        private string fileURl;
        private string _modulo = string.Empty;
        private byte _tipoReport = 1;
        private string _documentID = string.Empty;
        #endregion

        #region Funciones Publicas

        protected override void InitReport()
        {
            this.Module = ModulesPrefix.gl;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.glDocumentosPendientes).ToString());

            #region Configurar Opciones

            #region Crea la lista para los los diferentes combos
            
            //Se establece la lista del combo con sus respectivos valores
            List<ReportParameterListItem> tipoReport = new List<ReportParameterListItem>()
            {
                new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables,"Documentos Pendientes") }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, "Documentos Procesados") }, 
            };
            
            //Carga el combo con los modulos que esten activos para la empresa
            List<ReportParameterListItem> modulos = new List<ReportParameterListItem>();
            var modulo = this._bc.AdministrationModel.aplModulo_GetByVisible(1, true).ToList();
            modulos.Add(new ReportParameterListItem() { Key = "*", Desc = "*" });

            for (int i = 0; i < modulo.Count; i++)
                modulos.Add(new ReportParameterListItem() { Key = modulo[i].ModuloID.Value, Desc = modulo[i].ModuloID.Value + " - " + modulo[i].Descriptivo.Value });

            #endregion

            //Crea y carga los controles respectivamente
            this.AddList("TipoReporte", (AppForms.ReportForm) + "_Tipo", tipoReport, true, "1");
            this.AddList("Modulo", (AppForms.ReportForm).ToString() + "_modulo", modulos, true, "*");
            this.AddMaster("DocumentoID", AppMasters.glDocumento, false, null);

            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
            #endregion
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public Reports_DocuPendientes()
        {
            this.Module = ModulesPrefix.gl;
            this.ReportForm = AppReportParametersForm.glDocumentosPendientes;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                string fechaIniString;
                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;
                this.periodoFilter1.txtYear1.Visible = false;

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);
                Thread process = new Thread(this.LoadReportMethod_PDF);
                process.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            #region Carga el modulo por el cual se va a filtrar

            if (option.Equals("Modulo"))
            {
                ReportParameterList module = (ReportParameterList)this.RPControls["Modulo"];
                this._modulo = module.SelectedListItem.Key;

                if (this._modulo.Equals("*"))
                    this._modulo = string.Empty;
            }
            else if (option.Equals("TipoReporte"))
            {
                ReportParameterList tipoReport = (ReportParameterList)this.RPControls["TipoReporte"];
                this._tipoReport = Convert.ToByte(tipoReport.SelectedListItem.Key);
            }
            if (option.Equals("DocumentoID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._documentID = master.ValidID ? master.Value : string.Empty;
            }
            #endregion
        }

        #endregion
    }
}
