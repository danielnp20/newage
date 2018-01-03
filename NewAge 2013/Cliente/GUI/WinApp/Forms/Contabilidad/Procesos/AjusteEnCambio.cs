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
    public partial class AjusteEnCambio : ProcessForm
    {

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndPreliminar();
        public EndPreliminar endPreliminar;
        public void EndPreliminarMethod()
        {
            this.btnAjuste.Enabled = true;
            this.ControlBox = true;
            if (_resPreliminar != null && _resPreliminar.Result == ResultValue.OK && _aprob != null)
            {
                this.btnProcesar.Enabled = true;
                this.cmbLibro.Enabled = false;
                foreach (DTO_ComprobanteAprobacion ap in _aprob)
                    ap.DocumentoID.Value = AppDocuments.ComprobanteAjusteCambio;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesar;
        public void EndProcesarMethod()
        {
            this.btnAjuste.Enabled = true;
            this.ControlBox = true;
            if (_resAprobar != null)
            {
                bool isOK = true;
                foreach(DTO_TxResult result in _resAprobar)
                {
                    if(result.Details != null && result.Details.Count > 0)
                        isOK = false;
                }

                if (isOK)
                {
                    this.btnProcesar.Enabled = false;
                    this.cmbLibro.Enabled = true;
                }
            }
        }

        #endregion

        //public AjusteEnCambio()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;
        private List<DTO_TxResult> _resAprobar = null;
        private List<DTO_ComprobanteAprobacion> _aprob = null;
        //Libros
        private string libroFunc = string.Empty;
        private string libroIFRS = string.Empty;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.AjusteEnCambio;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.endPreliminar = new EndPreliminar(EndPreliminarMethod);
                this.endProcesar = new EndProcesar(EndProcesarMethod);

                //Carga los libros
                this.libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                this.libroIFRS = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(libroFunc, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroFunc"));
                dic.Add(libroIFRS, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroIFRS"));

                this.cmbLibro.Properties.DataSource = dic;
                this.cmbLibro.EditValue = this.libroFunc;

                //Revisa si tiene info de un libro asignada
                //string libroAct = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LibroOpConjuntas);
                //if (!string.IsNullOrWhiteSpace(libroAct))
                //{
                //    this.cmbLibro.Enabled = false;
                //    this.cmbLibro.EditValue = libroAct;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AjusteEnCambio.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de mayorizar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAjuste_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
            if (dt.Day > 1)
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_AjustePeriodoInv));
            else
            {
                this.ControlBox = false;
                this.btnAjuste.Enabled = false;
                new Thread(AjusteEnCambioThread).Start();
            }
        }

        /// <summary>
        /// Boton de procesar
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            if (_aprob != null)
            {
                this.ControlBox = false;
                this.btnAjuste.Enabled = false;
                this.btnProcesar.Enabled = false;
                new Thread(ProcesarThread).Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Ajuste en Cambio
        /// </summary>
        private void AjusteEnCambioThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(AppDocuments.ComprobanteAjusteCambio));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                Tuple<DTO_TxResult, List<DTO_ComprobanteAprobacion>> result = this._bc.AdministrationModel.Proceso_AjusteEnCambio(AppDocuments.ComprobanteAjusteCambio, this._actFlujo.ID.Value,
                    _bc.AdministrationModel.User.AreaFuncionalID.Value, dt, this.cmbLibro.EditValue.ToString());
                this._aprob = result.Item2;
                this._resPreliminar = result.Item1;

                this.Invoke(this.endPreliminar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result.Item1);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AjusteEnCambio.cs", "btnAjuste_Click"));
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
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(AppDocuments.ComprobanteAjusteCambio));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                this._resAprobar = _bc.AdministrationModel.Proceso_ProcesarBalancePreliminar(AppDocuments.ComprobanteAjusteCambio, this._actFlujo.ID.Value, this._aprob, dt, this.cmbLibro.EditValue.ToString());

                List<DTO_TxResult> resNOK = new List<DTO_TxResult>();
                foreach (DTO_TxResult result in _resAprobar)
                {
                    if (result.Result == ResultValue.NOK || (result.Details != null && result.Details.Count > 0))
                        resNOK.Add(result);
                }

                this.Invoke(this.endProcesar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(resNOK);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AjusteEnCambio.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
