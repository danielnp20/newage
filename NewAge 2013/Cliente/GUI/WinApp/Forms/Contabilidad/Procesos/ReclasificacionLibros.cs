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
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ReclasificacionLibros : ProcessForm
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
            this.btnReclasificar.Enabled = true;
            this.ControlBox = true;
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ReclasificacionLibros() : base() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.ReclasificacionLibros;

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
            this.periodoEdit.DateTime = Convert.ToDateTime(periodo);

            #endregion

            #region Asigna la info del balance

            string libroIFRS = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            DTO_glConsultaFiltro filtroIFRS = new DTO_glConsultaFiltro() 
            { 
                CampoFisico = "BalanceTipoID", 
                OperadorFiltro = OperadorFiltro.Igual, 
                ValorFiltro = libroIFRS,
                OperadorSentencia = OperadorSentencia.Or
            };

            filtros.Add(filtroIFRS);
            _bc.InitMasterUC(this.masterTipoBal, AppMasters.coBalanceTipo, true, true, true, false, filtros);

            string balTipo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
            

            this.masterTipoBal.Value = balTipo;
            #endregion
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnReclasificar_Click(object sender, EventArgs e)
        {
            if (!this.masterTipoBal.ValidID)
            {
                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                MessageBox.Show(string.Format(msg, this.masterTipoBal.Value));
            }
            else
            {
                this.ControlBox = false;
                this.btnReclasificar.Enabled = false;
                new Thread(ReclasificarThread).Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de mayorizar
        /// </summary>
        private void ReclasificarThread()
        {
            try
            {
                DateTime dt = periodoEdit.DateTime;

                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();
                DTO_TxResult result = this._bc.AdministrationModel.Proceso_ReclasificacionLibros(AppProcess.ReclasificacionLibros, dt, this.masterTipoBal.Value);

                this.Invoke(this.endProcessDelegate);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionLibros.cs", "ReclasificarThread"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
