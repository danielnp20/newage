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
    class Reports_Dinamicos : ReportParametersForm
    {
        #region Variables

        //Variables del hilo
        DateTime _fechaIni;
        private DTO_TxResult reportName;
        private string fileURl;
        private string _modulo = string.Empty;
        private byte _tipoReport = 1;
        private int _documentID = 0;
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
            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            filtros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "DocumentoTipo",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "4"
            });
            this.AddMaster("DocumentoID", AppMasters.glDocumento, false, filtros);

            #endregion
            #region Configurar Filtros del periodo            
            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
            this.periodoFilter1.monthCB.SelectedIndex = 0;
            this.periodoFilter1.Visible = false;
            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.btnFilter.Visible = false;
            this.btnResetFilter.Visible = false;
            this.btnExportToPDF.Text = "Abrir Editor";
            #endregion
        }

        /// <summary>
        /// Form Constructer
        /// </summary>
        public Reports_Dinamicos()
        {
            this.Module = ModulesPrefix.gl;
            this.documentReportID = AppReports.glReportesDinamicos;
        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                if (this._documentID != 0) { CustomizeReport report = new CustomizeReport(this._documentID); };
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Dinamicos.cs", "Report_PDF"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            if (option.Equals("DocumentoID"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._documentID = master.ValidID ? Convert.ToInt32(master.Value) : 0;
            }
        }

        #endregion
    }
}
