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
    public partial class CierreAnual : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndPreliminar();
        public EndPreliminar endPreliminar;
        public void EndPreliminarMethod()
        {
            this.btnCierre.Enabled = true;
            this.ControlBox = true;
            if (this._resPreliminar != null && this._resPreliminar.Result == ResultValue.OK && this._aprob != null)
            {
                this.cmbLibro.Enabled = false;
                this.btnProcesar.Enabled = true;
                this._aprob.DocumentoID.Value = AppDocuments.ComprobanteCierreAnual;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesar;
        public void EndProcesarMethod()
        {
            this.btnCierre.Enabled = true;
            this.ControlBox = true;
            if (this._resAprobar != null && this._resAprobar.Result == ResultValue.OK)
            {
                this.btnProcesar.Enabled = false;
                this.cmbLibro.Enabled = true;
            }
        }

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        //public CierreAnual()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;
        private DTO_TxResult _resAprobar = null;
        private DTO_ComprobanteAprobacion _aprob = null;
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
                this.documentID = AppProcess.CierreAnual;

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                this.endPreliminar = new EndPreliminar(EndPreliminarMethod);
                this.endProcesar = new EndProcesar(EndProcesarMethod);

                //Carga los libros
                this.libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                this.libroIFRS = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);
                string comprobanteIDCierre = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ComprobanteCierreAnual);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(libroFunc, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroFunc"));
                dic.Add(libroIFRS, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroIFRS"));

                #region 3.Asigna el periodo
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                string perido14 = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndicadorMes14);
                bool p14 = false;
                if (perido14.Equals("1") || perido14.Equals(true.ToString()))
                    p14 = true;

                DateTime periodo = new DateTime(dt.Year, 12, p14 ? 3 : 2);
                #endregion

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                ctrl.DocumentoID.Value = documentID;
                ctrl.PeriodoDoc.Value = periodo;
                ctrl.ComprobanteID.Value = comprobanteIDCierre;
                DTO_glDocumentoControl ctrlExist = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(ctrl).FirstOrDefault();
                if (ctrlExist != null && ctrlExist.Estado.Value != (byte)EstadoDocControl.Aprobado)
                {
                    this.btnProcesar.Enabled = true;
                    this._aprob = new DTO_ComprobanteAprobacion();
                    this._aprob.ComprobanteID.Value = comprobanteIDCierre;
                    this._aprob.ComprobanteNro.Value = ctrlExist.ComprobanteIDNro.Value.Value;
                    this._aprob.PeriodoID.Value = periodo;
                    this._aprob.NumeroDoc.Value = ctrlExist.NumeroDoc.Value.Value;
                    this._aprob.Aprobado.Value = true;
                    this._resPreliminar = new DTO_TxResult();
                    this._resPreliminar.Result = ResultValue.OK;
                    this._resPreliminar.ResultMessage = string.Empty;
                }

                //Revisa si tiene info de un libro asignada
                //string libroAct = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LibroOpConjuntas);
                //if (!string.IsNullOrWhiteSpace(libroAct))
                //{
                //    dic.Add(libroAct, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroFunc"));

                //    this.cmbLibro.Enabled = false;
                //    this.cmbLibro.EditValue = libroAct;
                //}

                this.cmbLibro.Properties.DataSource = dic;
                this.cmbLibro.EditValue = this.libroFunc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreAnual.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCierre_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnCierre.Enabled = false;
            this.btnProcesar.Enabled = false;
            new Thread(CierreAnualThread).Start();
        }

        /// <summary>
        /// Procesa el cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            if (this._aprob != null)
            {
                this.ControlBox = false;
                this.btnCierre.Enabled = false;
                this.btnProcesar.Enabled = false;
                new Thread(ProcesarThread).Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de cierre
        /// </summary>
        private void CierreAnualThread()
        {
            try
            {
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));

                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                Tuple<DTO_TxResult, DTO_ComprobanteAprobacion> result = this._bc.AdministrationModel.Proceso_CierreAnual(this.documentID, this._actFlujo.ID.Value, _bc.AdministrationModel.User.AreaFuncionalID.Value, dt.Year, this.cmbLibro.EditValue.ToString());
                this._aprob = result.Item2;
                this._resPreliminar = result.Item1;

                this.Invoke(this.endPreliminar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(result.Item1);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreAnual.cs-", "CierreAnualThread"));
                this.StopProgressBarThread();
            }
        }

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                List<DTO_ComprobanteAprobacion> aprobaciones = new List<DTO_ComprobanteAprobacion>();
                aprobaciones.Add(this._aprob);

                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(aprobaciones[0].NumeroDoc.Value.Value);
                aprobaciones[0].ComprobanteNro.Value = ctrl.ComprobanteIDNro.Value.Value;
                DateTime dt = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo));
                List<DTO_TxResult> result = _bc.AdministrationModel.Proceso_ProcesarBalancePreliminar(this.documentID, this._actFlujo.ID.Value, aprobaciones, dt, this.cmbLibro.EditValue.ToString());
                this._resAprobar = result.First();

                this.Invoke(this.endProcesar);
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(_resAprobar);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreAnual.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
