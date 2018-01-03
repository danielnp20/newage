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
    public partial class CierreMensual : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private ModulesPrefix _mod = ModulesPrefix.co;

        #endregion

        #region Propiedades

        public ModulesPrefix Mod
        {
            get { return this._mod; }
            set
            {
                this._mod = value;
                string periodoStr = _bc.GetControlValueByCompany(this._mod, AppControl.co_Periodo);
                string periodo14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
                try
                {
                    if (periodo14 == "1")
                        _bc.InitPeriodUC(this.periodoEdit, 2);
                    else
                        _bc.InitPeriodUC(this.periodoEdit, 1);
                    DateTime dtPeriodo = DateTime.Parse(periodoStr);
                    this.periodoEdit.DateTime = dtPeriodo;       
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreMensual.cs", "Mod"));
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
            this.btnCierrePeriodo.Enabled = true;
            this.ControlBox = true;
            this.Mod = this._mod;
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public CierreMensual() : base(){ }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.CierreMensual;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);

            TablesResources.GetTableResources(this.cmbModulo, typeof(ModulesPrefix));
            this.cmbModulo.SelectedIndex = -1;

            string periodo14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
            if (periodo14 == "1")
                _bc.InitPeriodUC(this.periodoEdit, 2);
            else
                _bc.InitPeriodUC(this.periodoEdit, 1);

            this.cmbModulo.SelectedIndex = 0;
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
            if (this.cmbModulo.SelectedIndex != -1)
            {
                ComboBoxItem item = (ComboBoxItem)((ComboBoxEx)sender).SelectedItem;
                this.Mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), item.Value);
            }
        }

        /// <summary>
        /// Boton de cierre mensual
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCierre_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnCierrePeriodo.Enabled = false;
            new Thread(CierreThread).Start();
        }

        #endregion 

        #region Hilos

        /// <summary>
        /// Hilo de Cierre Mensual
        /// </summary>
        private void CierreThread()
        {
            try
            {
                DateTime dt = periodoEdit.DateTime;
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = this._bc.AdministrationModel.Proceso_CierrePeriodo(this.documentID, this.periodoEdit.DateTime, this.Mod);
                _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, _bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();
                this.Invoke(this.endProcess);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreAnual.cs", "btnCierre_Click"));
                this.StopProgressBarThread();
            }

        }

        #endregion

    }
}
