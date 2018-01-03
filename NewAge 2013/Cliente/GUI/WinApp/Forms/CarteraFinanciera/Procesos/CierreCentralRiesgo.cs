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
using DevExpress.XtraGrid.Columns;
using NewAge.DTO.Negocio;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using SentenceTransformer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CierreCentralRiesgo : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {

           
        }

        /// <summary>
        /// Delegado que finaliza el proceso de validaciones del servidor
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.Enabled = true;

            this.btnProcesar.Enabled = true;

            this.ExportToExcel();
            //if (this.result.Result == ResultValue.NOK)
            //{
            //    this.btnInconsistencias.Enabled = true;
            //    this.validarInconsistencias = true;
            //    this._isOK = false;
            //}
            //else if (this.result.Details.Count == 0)
            //{
            //    MessageForm frm = new MessageForm(this.result);
            //    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            //    this._isOK = true;
            //}
            //else
            //{
            //    this.btnInconsistencias.Enabled = true;
            //    this._isOK = true;
            //}

            //if (this._isOK || !this.validarInconsistencias)
            //{
            //    this.btnRelPagos.Enabled = true; 
            //    this.btnPagar.Enabled = true;
            //}
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

        private BaseController _bc = BaseController.GetInstance();
        //Variables del formulario
        private bool _isOK;
        private List<DTO_CentralRiesgoMes> data;
        private string centroPagoID = string.Empty;
        private DTO_tsBancosCuenta bancosCta;
        private DateTime periodo = DateTime.Now;
        private bool validarInconsistencias;
        //Variables para la importacion
        DTO_TxResult result;
        List<DTO_TxResult> results;
        DataTableOperations tableOp;
        DataTable tableImportacion = new DataTable();
        List<string> codigos = new List<string>();
        private string reportName;
        private string fileURl;

        #endregion

        public CierreCentralRiesgo()
        {
            //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.CierreCentralRiesgo; 
            this.InitializeComponent();

            //Inicializa los delegados
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
            this.endProcesarDelegate = new EndProcesar(this.EndProcesarMethod);           
            this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

            //Carga la configuracion inicial
            this._isOK = false;

            this.data = new List<DTO_CentralRiesgoMes>();
            this.btnProcesar.Enabled = true;
            this.btnInconsistencias.Enabled = false;

            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = this.periodo;

            //Fecha de cierre
            string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
            int diaCierre = string.IsNullOrWhiteSpace(diaCierreStr) ? 1 : Convert.ToInt16(diaCierreStr);
            //Carga los recuros
            FormProvider.LoadResources(this, AppProcess.CierreCentralRiesgo);
        }

        #endregion

        #region Funciones Privadas


        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        private void ExportToExcel()
        {
            try
            {
                DataTableOperations tableOp = new DataTableOperations();
                DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_CentralRiesgoMes), data);
                DataTable tableExport = new DataTable();

                ReportExcelBase frm = new ReportExcelBase(tableAll);
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreCentralRiesgo.cs", "TBExport"));
            }
        }
        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se encarga de verificar las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            if (this.data.Count > 0)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                    loadData = false;
            }

            if (loadData)
            {
                this.Enabled = false;

                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
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
        /// Hilo de Procesar las inconsistencias
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.data = this._bc.AdministrationModel.ccCierreMesCartera_GetCierreCentralRiesgoMes(this.dtPeriod.DateTime.Date);
                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_CentralRiesgoMes>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-CierreCentralRiesgo.cs", "ProcesarThread");
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
            catch (Exception ex)
            {
                _bc.GetResourceForException(ex, "WinApp-CierreCentralRiesgo.cs", "InconsistenciasThread");
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        #endregion

    }
}
