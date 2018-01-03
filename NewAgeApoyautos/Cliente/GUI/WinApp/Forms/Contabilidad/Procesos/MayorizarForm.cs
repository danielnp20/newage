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
    public partial class MayorizarForm : ProcessForm
    {
        BaseController _bc = BaseController.GetInstance();

        #region Delegados

        public delegate void EndProcess();
        public EndProcess endProcessDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public void EndProcessMethod()
        {
            this.btnMayor.Enabled = true;
            this.ControlBox = true;
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public MayorizarForm() : base(){ }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.MayorizarForm;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcessDelegate = new EndProcess(EndProcessMethod);

            #region Asigna la info del periodo
            string periodo14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
            if (periodo14 == "1")
                _bc.InitPeriodUC(this.periodoEdit, 2);
            else
                _bc.InitPeriodUC(this.periodoEdit, 1);

            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
            DateTime dt = DateTime.Today;
            try
            {
                dt = DateTime.Parse(periodo);
            }
            catch (Exception)
            {
                dt = DateTime.Today;
            }
            this.periodoEdit.DateTime = dt;

            #endregion

            //Asigna la info del balance
            _bc.InitMasterUC(this.masterTipoBal, AppMasters.coBalanceTipo, true, true, true, false);
            string balTipo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
            this.masterTipoBal.Value = balTipo;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnMayor_Click(object sender, EventArgs e)
        {
            if (!this.masterTipoBal.ValidID)
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                MessageBox.Show(string.Format(msg, this.masterTipoBal.Value));
            }
            else
            {
                this.ControlBox = false;
                this.btnMayor.Enabled = false;
                new Thread(MayorizarThread).Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de mayorizar
        /// </summary>
        private void MayorizarThread()
        {
            try
            {
                DateTime dt = periodoEdit.DateTime;

                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();
                DTO_TxResult result = this._bc.AdministrationModel.Proceso_Mayorizar(AppProcess.MayorizarForm, dt, this.masterTipoBal.Value);

                this.Invoke(this.endProcessDelegate);
                this.StopProgressBarThread();
                
                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MayorizarForm.cs", "btnMayorizar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
