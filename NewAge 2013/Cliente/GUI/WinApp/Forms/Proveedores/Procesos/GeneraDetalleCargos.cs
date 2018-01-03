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
    public partial class GeneraDetalleCargos : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private ModulesPrefix _mod = ModulesPrefix.co;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public delegate void EndProcess();
        public EndProcess endProcess;
        public void EndProcessMethod()
        {
            this.btnProcesar.Enabled = false;
            this.ControlBox = true;
            this.Close();
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public GeneraDetalleCargos() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.GeneraCargosRecibidos;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);
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
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                List<DTO_SerializedObject> results = this._bc.AdministrationModel.Recibido_GenerarDetalleCargosRecib(this.documentID);
                this.StopProgressBarThread();

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                foreach (DTO_SerializedObject result in results)
                {
                    DTO_TxResult r = (DTO_TxResult)result;
                    if (r.Result == ResultValue.NOK)
                    {
                        resultsNOK.Add(r);
                    }
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.endProcess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GeneraDetalleCargos.cs", "ProcesarThread"));
                this.StopProgressBarThread();
            }

        }

        #endregion

    }
}
