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
using System.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class Reports_SobreEjecucion : ReportParametersForm
    {

        #region Variables

        //Variables del Reporte
        private string _proyecto = string.Empty;
        private string _contrato = string.Empty;
        private string _bloque = string.Empty;
        private string _campo = string.Empty;
        private string _pozo = string.Empty;
        private string _actividad = string.Empty;
        private string _linea = string.Empty;
        private string _centroCos = string.Empty;
        private string _recurso = string.Empty;
        private string _usuario = string.Empty;
        private string _prefijo = string.Empty;

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Desahabilida los controles que no se utilicen
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
        /// Form Constructor
        /// </summary>
        public Reports_SobreEjecucion()
        {
            this.Module = ModulesPrefix.pl;
            this.ReportForm = AppReportParametersForm.plSobreEjecicion;
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Evento que inicializa el reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.pl;
            this.btnExportToXLS.Visible = true;
            this.btnExportToPDF.Visible = false;
            this.Text = _bc.GetResource(Librerias.Project.LanguageTypes.Forms, (AppReports.plSobreEjecicion).ToString());

            #region Configurar Opciones

            //Crea y carga los controles respectivamente

            this.AddMaster("contrato", AppMasters.pyContrato, true, null);
            this.AddMaster("bloque", AppMasters.ocBloque, true, null);
            this.AddMaster("campo", AppMasters.glAreaFisica, true, null);
            this.AddMaster("pozo", AppMasters.glLocFisica, true, null);
            this.AddMaster("proyecto", AppMasters.coProyecto, true, null);
            this.AddMaster("actividad", AppMasters.coActividad, true, null);
            this.AddMaster("linea", AppMasters.plLineaPresupuesto, true, null);
            this.AddMaster("centroCos", AppMasters.coCentroCosto, true, null);
            this.AddMaster("recurso", AppMasters.plRecurso, true, null);
            this.AddMaster("usuario", AppMasters.seUsuario, true, null);
            this.AddMaster("prefijo", AppMasters.glPrefijo, true, null);
            this.AddTextBox("numeroDoc", false, (AppForms.ReportForm).ToString() + "_numeroDoc");



            #endregion
            #region Configurar Filtros del periodo

            this.periodoFilter1.FilterOptions = PeriodFilterOptions.Year;
            this.periodoFilter1.txtYear.Text = (DateTime.Now.Year).ToString();

            this.EnableControls();

            #endregion
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                DateTime _Periodo;
                this.EnableControls();
                this.periodoFilter1.txtYear1.Visible = false;
                this.btnExportToXLS.Enabled = false;

                int year = Convert.ToInt16(this.periodoFilter1.Year[0].ToString());
                int Month = DateTime.Now.Month;
                _Periodo = new DateTime(year, Month, DateTime.DaysInMonth(year, Month));

                ReportParameterTextBox _numeroDoc = (ReportParameterTextBox)this.RPControls["numeroDoc"];

                this._Query = this._bc.AdministrationModel.ReportesPlaneacion_SobreEjecucion(_Periodo.Year, _contrato, _bloque, _campo, _pozo, _proyecto, _actividad, _linea, _centroCos, _recurso,
                    _usuario, _prefijo, _numeroDoc.txtFrom.Text);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_ProyectoPresupuesto.cs", "Report_XLS"));
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
            #region carga el contrato que se desea Verificar

            if (option.Equals("contrato"))
            {
                uc_MasterFind masterContrato = (uc_MasterFind)sender;
                this._contrato = masterContrato.ValidID ? masterContrato.Value : string.Empty;
            }

            #endregion
            #region Carga el bloque que se desea verificar

            if (option.Equals("bloque"))
            {
                uc_MasterFind masterbloque = (uc_MasterFind)sender;
                this._bloque = masterbloque.ValidID ? masterbloque.Value : string.Empty;
            }

            #endregion
            #region Carga el campo que se desea verificar

            if (option.Equals("campo"))
            {
                uc_MasterFind mastercampo = (uc_MasterFind)sender;
                this._campo = mastercampo.ValidID ? mastercampo.Value : string.Empty;
            }

            #endregion
            #region Carga el pozo que se desea verificar

            if (option.Equals("pozo"))
            {
                uc_MasterFind masterpozo = (uc_MasterFind)sender;
                this._pozo = masterpozo.ValidID ? masterpozo.Value : string.Empty;
            }

            #endregion
            #region Carga el proyecto que se desea verificar

            if (option.Equals("proyecto"))
            {
                uc_MasterFind masterProyecto = (uc_MasterFind)sender;
                this._proyecto = masterProyecto.ValidID ? masterProyecto.Value : string.Empty;
            }

            #endregion
            #region Carga la Actividad que se desea Verificar

            if (option.Equals("actividad"))
            {
                uc_MasterFind masteractividad = (uc_MasterFind)sender;
                this._actividad = masteractividad.ValidID ? masteractividad.Value : string.Empty;
            }

            #endregion
            #region Carga la linea presupuestal que se desea verificar

            if (option.Equals("linea"))
            {
                uc_MasterFind masterlinea = (uc_MasterFind)sender;
                this._linea = masterlinea.ValidID ? masterlinea.Value : string.Empty;
            }

            #endregion
            #region Carga e centro de costo q se desea verificar

            if (option.Equals("centroCos"))
            {
                uc_MasterFind mastercentroCos = (uc_MasterFind)sender;
                this._centroCos = mastercentroCos.ValidID ? mastercentroCos.Value : string.Empty;
            }

            #endregion
            #region Carga el recurso que se desea verificar

            if (option.Equals("recurso"))
            {
                uc_MasterFind masterrecurso = (uc_MasterFind)sender;
                this._recurso = masterrecurso.ValidID ? masterrecurso.Value : string.Empty;
            }

            #endregion
            #region Carga el usuario que se desea Verificar

            if (option.Equals("usuario"))
            {
                uc_MasterFind masterusuario = (uc_MasterFind)sender;
                this._usuario = masterusuario.ValidID ? masterusuario.Value : string.Empty;
            }

            #endregion
            #region Carga el prefijo que se desea verificar

            if (option.Equals("prefijo"))
            {
                uc_MasterFind masterprefijo = (uc_MasterFind)sender;
                this._prefijo = masterprefijo.ValidID ? masterprefijo.Value : string.Empty;
            }

            #endregion
        }

        #endregion

    }
}
