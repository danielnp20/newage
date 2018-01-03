using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ContabilizarProvisiones : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public delegate void EndProcess();
        public EndProcess endProcess;
        public void EndProcessMethod()
        {          
            this.ControlBox = true;
        }

        #endregion

        public ContabilizarProvisiones() : base() { }


        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.CierreNomina;

            InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);
            this.endProcess = new EndProcess(EndProcessMethod);
            _bc.InitPeriodUC(this.uc_Periodo, 0);
            string periodo = this._bc.GetControlValueByCompany(DTO.Negocio.ModulesPrefix.no, AppControl.no_Periodo);
            this.uc_Periodo.DateTime = DateTime.Parse(periodo);

        }

        #endregion

        #region Eventos

        /// <summary>
        /// Procesa la Nomina del Periodo en Curso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
