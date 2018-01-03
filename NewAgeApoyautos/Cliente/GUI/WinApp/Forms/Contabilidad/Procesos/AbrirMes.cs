using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AbrirMes : ProcessForm
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        private ModulesPrefix _mod = ModulesPrefix.co;
        private DateTime _maxDate = DateTime.Now;
        private DateTime _minDate = DateTime.Now;

        private bool _validatePeriod = true;
        #endregion

        #region Propiedades

        public ModulesPrefix Mod
        {
            get { return this._mod; }
            set
            {
                this._mod = value;
                string periodoStr = _bc.GetControlValueByCompany(this._mod, AppControl.co_Periodo);
                try
                {
                    DateTime dtPeriodo = DateTime.Parse(periodoStr);
                    this._maxDate = dtPeriodo.AddMonths(-1);
                    this._minDate = this._maxDate.AddMonths(-1);

                    this.periodoEdit.DateTime = this._maxDate;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbrirMes.cs", "Mod"));
                }
            }
        }

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public delegate void EndProcess();
        public EndProcess endProcess;
        public void EndProcessMethod()
        {
            this.btnAbrirMes.Enabled = true;
            this.ControlBox = true;

            this._validatePeriod = false;
            this.Mod = this._mod;
            this._validatePeriod = true;
            this.Close();
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public AbrirMes() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.AbrirMes;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);

            this.endProcess = new EndProcess(EndProcessMethod);
            _bc.InitPeriodUC(this.periodoEdit, 1);

            TablesResources.GetTableResources(this.cmbModulo, typeof(ModulesPrefix));
            this.cmbModulo.RemoveItem(ModulesPrefix.apl.ToString());
            this.cmbModulo.RemoveItem(ModulesPrefix.gl.ToString());
            this.cmbModulo.RemoveItem(ModulesPrefix.se.ToString());
            this.cmbModulo.SelectedIndex = -1;

            this.periodoEdit.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.periodoEdit_EditValueChanged);
            this.cmbModulo.Text = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Master_Mod_CO);
            this.cmbModulo.Enabled = false;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al seleccinar un nuevo modulo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbModulo_SelectedValueChanged(object sender, EventArgs e)
        {
            this._validatePeriod = true;
            if (this.cmbModulo.SelectedIndex != -1)
            {
                ComboBoxItem item = (ComboBoxItem)((ComboBoxEx)sender).SelectedItem;
                this.Mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), item.Value);

                if (this.Mod == ModulesPrefix.co)
                    this.periodoEdit.ExtraPeriods = 1;
                else
                    this.periodoEdit.ExtraPeriods = 0;
            }
        }

        /// <summary>
        /// Boton de cierre mensual
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAbrir_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnAbrirMes.Enabled = false;
            new Thread(OpenThread).Start();
        }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void periodoEdit_EditValueChanged()
        {
            if (this.periodoEdit.DateTime > this._maxDate)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidPeriodSelected));
                this.periodoEdit.DateTime = this._maxDate;
                this.periodoEdit.Focus();
            }
            else if (this._mod != ModulesPrefix.co && this._validatePeriod)
            {
                bool res = _bc.AdministrationModel.UltimoMesCerrado(this._mod, this.periodoEdit.DateTime);
                if (!res)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_OpenLastPeriod));
                    this.periodoEdit.DateTime = this._maxDate;
                    this.periodoEdit.Focus();
                }
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Cierre Mensual
        /// </summary>
        private void OpenThread()
        {
            try
            {
                DateTime dt = periodoEdit.DateTime;
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = this._bc.AdministrationModel.Proceso_AbrirMes(this.documentID, this.periodoEdit.DateTime, this.Mod);
                _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, _bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.endProcess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AbrirMes.cs", "OpenThread"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
