using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;
using NewAge.DTO;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CierreLegalizacion : ProcessForm
    {
        BaseController _bc = BaseController.GetInstance();
        string ultimaQuincenaLiquidada = string.Empty;
        string esLiqQuincenal = string.Empty;
        DateTime dtPeriodo;

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
        public CierreLegalizacion() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.CierreLegalizacion;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);

            _bc.InitPeriodUC(this.uc_Periodo, 0);

            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.pl_Periodo);
            dtPeriodo = DateTime.Parse(periodoStr);
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
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = this._bc.AdministrationModel.Proceso_plCierreLegalizacion_Cierre(this.uc_Periodo.DateTime);
                this.Invoke(this.endProcess);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ProcesarThread"));
                this.StopProgressBarThread();
            }

        }

        #endregion

      
       
    }
}
