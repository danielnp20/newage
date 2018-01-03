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
    public partial class ProrrateoIVA : ProcessForm
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
                try
                {
                    DateTime dtPeriodo = DateTime.Parse(periodoStr);
                    this.uc_Periodo.DateTime = dtPeriodo;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProrrateoIVA.cs", "Mod"));
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
            this.btnProcesar.Enabled = true;
            this.ControlBox = true;
            this.Mod = this._mod;
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProrrateoIVA() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.ProrrateoIVA;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);

            string periodo14 = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
            if (periodo14 == "1")
                _bc.InitPeriodUC(this.uc_Periodo, 2);
            else
                _bc.InitPeriodUC(this.uc_Periodo, 1);

            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
            DateTime dtPeriodo = DateTime.Parse(periodoStr);
            this.uc_Periodo.DateTime = dtPeriodo;
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de cierre mensual
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnProcesar.Enabled = false;
            new Thread(ProcesarThread).Start();
        }

        #endregion 

        #region Hilos

        /// <summary>
        /// Hilo de Cierre Mensual
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                DateTime dt = uc_Periodo.DateTime;
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = this._bc.AdministrationModel.Proceso_ProrrateoIVA(this.documentID, this._actFlujo.ID.Value);
                this.Invoke(this.endProcess);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProrrateoIVA.cs", "ProcesarThread"));
                this.StopProgressBarThread();
            }

        }

        #endregion

    }
}
