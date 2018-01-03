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
    public partial class ParticionBilling : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndPreliminar();
        public EndPreliminar endPreliminar;
        public void EndPreliminarMethod()
        {
            this.btnBilling.Enabled = true;
            this.ControlBox = true;
            if (_resPreliminar != null && _resPreliminar.Result == ResultValue.OK)
                this.btnProcesar.Enabled = true;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesar;
        public void EndProcesarMethod()
        {
            this.btnBilling.Enabled = true;
            this.ControlBox = true;

            if (_resPreliminar != null && _resPreliminar.Result == ResultValue.OK)
                this.btnProcesar.Enabled = false;  
        }

        #endregion

        //public ParticionBilling()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.ParticionBilling;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.endPreliminar = new EndPreliminar(EndPreliminarMethod);
                this.endProcesar = new EndProcesar(EndProcesarMethod);

                _bc.InitPeriodUC(this.dtPeriod, 0);
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo));
                this.dtPeriod.DateTime = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParticionBilling.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnBilling_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnBilling.Enabled = false;
            new Thread(ParticionBillingThread).Start();
        }

        /// <summary>
        /// Boton de procesar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            if (this._resPreliminar.Result == ResultValue.OK)
            {
                this.ControlBox = false;
                this.btnBilling.Enabled = false;
                this.btnProcesar.Enabled = false;
                new Thread(ProcesarThread).Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Ajuste en Cambio
        /// </summary>
        private void ParticionBillingThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo));
                this._resPreliminar = this._bc.AdministrationModel.Billing_Particion(this.documentID, this._actFlujo.ID.Value);

                this.Invoke(this.endPreliminar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resPreliminar);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParticionBilling.cs", "btnCierre_Click"));
                this.StopProgressBarThread();
            }
        }

        /// <summary>
        /// Hilo de procesar cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                string libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                this._resPreliminar = _bc.AdministrationModel.ProcesarBilling(this.documentID, this._actFlujo.ID.Value, this.dtPeriod.DateTime);

                this.Invoke(this.endProcesar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resPreliminar);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ParticionBilling.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
