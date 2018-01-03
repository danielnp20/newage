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
    public partial class ReprocesoDepreciacion : ProcessForm
    {
        BaseController _bc = BaseController.GetInstance();

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
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ReprocesoDepreciacion() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.ReprocesoDepreciacion;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);

            _bc.InitPeriodUC(this.uc_PeriodoIni, 0);
            _bc.InitPeriodUC(this.uc_PeriodoFin, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.ac, AppControl.co_Periodo);
            DateTime dtPeriodo = DateTime.Parse(periodoStr);
            this.uc_PeriodoIni.DateTime = dtPeriodo;
            this.uc_PeriodoFin.DateTime = dtPeriodo;
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
                DateTime dt = uc_PeriodoIni.DateTime;
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoActivos(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                List<DTO_TxResult> results = this._bc.AdministrationModel.Proceso_ReProcesoDepreciacion(this.documentID, this.uc_PeriodoIni.DateTime, this.uc_PeriodoFin.DateTime);
                this.Invoke(this.endProcess);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(results);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReprocesoDepreciacion.cs", "ProcesarThread"));
                this.StopProgressBarThread();
            }

        }

        #endregion

    }
}
