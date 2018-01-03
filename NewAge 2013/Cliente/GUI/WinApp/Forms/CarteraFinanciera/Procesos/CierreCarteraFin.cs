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
    public partial class CierreCarteraFin : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        private delegate void EndProcesar();
        private EndProcesar EndProcesarDelegate;
        private void EndProcesarMethod()
        {
            this.btnCierre.Enabled = true;
            this.ControlBox = true;
            this.Enabled = true;
        }

        #endregion

        //public CierreCarteraFin()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resProcesar = null;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.CierreCartera;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.EndProcesarDelegate = new EndProcesar(EndProcesarMethod);

                _bc.InitPeriodUC(this.dtPeriod, 0);
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo));
                this.dtPeriod.DateTime = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreCartera.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCierre_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnCierre.Enabled = false;
            new Thread(CierreCarteraThread).Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Ajuste en Cambio
        /// </summary>
        private void CierreCarteraThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo));
                this._resProcesar = this._bc.AdministrationModel.Proceso_CierreMesCarteraFin(this.documentID, dt);

                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resProcesar);
                frm.ShowDialog();

                this.Invoke(this.EndProcesarDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreCartera.cs", "btnCierre_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
