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
    public partial class DistribucionLegalizacion : ProcessForm
    {
        //public DistribucionLegalizacion()
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
                this.documentID = AppProcess.DistribucionLegalizacion;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                _bc.InitPeriodUC(this.dtPeriod, 0);
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.oc, AppControl.oc_Periodo));
                this.dtPeriod.DateTime = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CashCall.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

       
        /// <summary>
        /// Boton de procesar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            new Thread(ProcesarThread).Start();         
        }

        #endregion

        #region Hilos

  
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
                this._resPreliminar = _bc.AdministrationModel.Proceso_ocDetalleLegalizacion_Distribucion(this.dtPeriod.DateTime);

                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resPreliminar);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DistribucionLegalizacion.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
