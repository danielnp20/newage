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
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using SentenceTransformer;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ProvisionRecibidosNoFact : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.results);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this.btnInconsistencias.Enabled = false;
            }
            else
                this.btnInconsistencias.Enabled = true;
            this.btnGenerar.Enabled = false;
        }

        /// <summary>
        /// Delegado que finaliza el proceso imprimir las inconsistencias
        /// </summary>
        public delegate void EndInconsistencias();
        public EndInconsistencias endInconsistenciasDelegate;
        public void EndInconsistenciasMethod()
        {
            this.Enabled = true;
        }

        #endregion

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private DTO_TxResult result;
        private List<DTO_TxResult> results;
        private string reportName;
        private string fileURl;

        //Variables de monedas
        private string monedaLocal;

        //Variables del formulario
        private bool _isOK;
        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public ProvisionRecibidosNoFact() 
        {
           // this.InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppDocuments.ProvisionRecibNoFact;

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);
                this.endInconsistenciasDelegate = new EndInconsistencias(EndInconsistenciasMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = true;
                this.btnInconsistencias.Enabled = false;

                //Funciones para iniciar el formulario
                _bc.InitPeriodUC(this.dtPeriod, 0);

                //Inicia las variables
                this.dtPeriod.Enabled = false;
                this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this.dtPeriod.DateTime = Convert.ToDateTime(_bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_Periodo));            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProvisionRecibidosNoFact.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                this.btnGenerar.Enabled = false;
                this.results = null;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProvisionRecibidosNoFact.cs", "btnGenerarDocumentos_Click"));
            }
        }

        /// <summary>
        /// Evento que muestra las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInconsistencias_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            Thread process = new Thread(this.InconsistenciasThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.results = new List<DTO_TxResult>();
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.result = this._bc.AdministrationModel.Provision_RecibidoNotFacturadoAdd(this.documentID);
                this.results.Add(this.result);
                // this.result = null; //Lo vuelve null para poder mostrar los mensajes
                this.StopProgressBarThread();

                this._isOK = true;
                bool notOK = this.results.Any(x => x.Result == ResultValue.NOK);
                if (notOK)
                    this._isOK = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ProvisionRecibidosNoFact.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
            finally
            {
                this.Invoke(this.endProcesarDelegate);
            }
        }

        /// <summary>
        /// Carga el reporte con las inconsistencias
        /// </summary>
        private void InconsistenciasThread()
        {
            try
            {
                if (this.result != null)
                {
                    _bc.AssignResultResources(null, this.result);
                    reportName = this._bc.AdministrationModel.Rep_TxResult(this.result);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else if (this.results != null)
                {

                    _bc.AssignResultResources(null, this.results);
                    reportName = this._bc.AdministrationModel.Rep_TxResultDetails(this.results);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        #endregion

    }
}
