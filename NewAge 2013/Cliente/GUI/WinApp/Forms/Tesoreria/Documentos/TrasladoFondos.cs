﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class TrasladoFondos : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            try
            {
                this.NewTrasladoFondos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _monedaLocal;
        private string _monedaExtranjera;

        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_tsBancosDocu _tblAux = new DTO_tsBancosDocu();
        private string _actFlujoID = string.Empty;

        #endregion

        #region Propiedades

        /// <summary>
        /// Pago facturas sobre el cual se esta trabajando
        /// </summary>
        private DTO_PagoFacturas _pagoFacturas = null;
        protected virtual DTO_PagoFacturas PagoFacturasAct
        {
            get { return this._pagoFacturas; }
            set { this._pagoFacturas = value; }
        }

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public TrasladoFondos()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppDocuments.TrasladoFondos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.ts;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Carga info de las monedas
                this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                
                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterCuentaOrigen, AppMasters.tsBancosCuenta, false, true, true, true);
                _bc.InitMasterUC(this.masterCuentaDestino, AppMasters.tsBancosCuenta, false, true, true, true);

                //Carga la info del periodo
                _bc.InitPeriodUC(this.dtPeriod, 0);
                this.dtPeriod.DateTime = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo));
                this.dtFecha.DateTime = DateTime.Today;
                this.dtFecha.Properties.MinValue = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, 1);
                this.dtFecha.Properties.MaxValue = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));

                this.saveDelegate = new Save(this.SaveMethod);

                if (!_bc.AdministrationModel.MultiMoneda)
                {
                    this.lblTasaCambio.Visible = false;
                    this.txtTasaCambio.Visible = false;
                    this.txtTasaCambio.EditValue = 0;
                }

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                    this._actFlujoID = actividades[0];
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "TrasladoFondos"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Valida si los datos del formulario están bien asignados y crea las fuentes de datos de los documentos a guardar
        /// </summary>
        /// <returns>True si es válido, false si es inválido</returns>
        private bool ValidData()
        {
            if (this.masterCuentaOrigen.Value == string.Empty ||
                this.masterCuentaDestino.Value == string.Empty ||
                this.masterCuentaOrigen.Value == this.masterCuentaDestino.Value)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_BancoCuentaOrigenDestinoInvalidos));
                return false;
            }
            else
            {
                DTO_tsBancosCuenta bancoCuentaOrigen = (DTO_tsBancosCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuentaOrigen.Value, true);
                DTO_coDocumento documentoOrigen = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, bancoCuentaOrigen.coDocumentoID.Value, true);
                DTO_tsBancosCuenta bancoCuentaDestino = (DTO_tsBancosCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuentaDestino.Value, true);
                DTO_coDocumento documentoDestino = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, bancoCuentaDestino.coDocumentoID.Value, true);
            }

            if (_bc.AdministrationModel.MultiMoneda && Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture) <= 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                return false;
            }

            if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) <= 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_ValorATrasladarInvalido));
                return false;
            }

            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);

            this._tblAux.BancoCuentaID.Value = this.masterCuentaDestino.Value;
            this._tblAux.Dato1.Value = this.masterCuentaOrigen.Value;
            this._tblAux.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
            
            return true;
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void NewTrasladoFondos()
        {
            this.masterCuentaOrigen.Value = string.Empty;
            this.masterCuentaDestino.Value = string.Empty;
            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo);
            this.dtPeriod.DateTime = !string.IsNullOrEmpty(periodo)? Convert.ToDateTime(periodo) : DateTime.Now;
            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.txtValor.EditValue = "0";
            this.chkGenerarOrdenPago.Checked = false;
            this.gctrlBody.Focus();
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);

                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Form

        /// <summary>
        /// Valida que las fechas estén en el período contable actual 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    decimal tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(this._monedaExtranjera, this.dtFecha.DateTime);

                    if (tasaCambio > 0)
                    {
                        this.txtTasaCambio.EditValue = tasaCambio;
                        this.gctrlBody.Enabled = true;
                    }
                    else
                    {
                        this.txtTasaCambio.EditValue = 0;
                        
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoTasaCambioFechaSeleccionada));
                        this.gctrlBody.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoFondos.cs", "dtFecha_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Valida que las cuentas origen y destino sean distintas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCuentaOrigenDestino_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind cuenta = (ControlsUC.uc_MasterFind)sender;
            if (this.masterCuentaOrigen.Value == this.masterCuentaDestino.Value)
            {
                cuenta.Value = string.Empty;
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Ts_NoCuentasDistintas));
                return;
            }

            if (this.masterCuentaOrigen.Value != string.Empty && this.masterCuentaDestino.Value != string.Empty)
            {
                DTO_tsBancosCuenta bancoCuentaOrigen = (DTO_tsBancosCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuentaOrigen.Value, true);
                DTO_coDocumento documentoOrigen = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, bancoCuentaOrigen.coDocumentoID.Value, true);
                DTO_tsBancosCuenta bancoCuentaDestino = (DTO_tsBancosCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuentaDestino.Value, true);
                DTO_coDocumento documentoDestino = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, bancoCuentaDestino.coDocumentoID.Value, true);
                //if (documentoOrigen.MonedaOrigen.Value != documentoDestino.MonedaOrigen.Value)
                //{
                //    cuenta.Value = string.Empty;
                //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_MonedaCuentasDistintas));
                //    return;                    
                //}
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.NewTrasladoFondos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                if (this.ValidData())
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                result = _bc.AdministrationModel.TrasladoFondos_TrasladarFondos(this._documentID, this._actFlujoID, this._ctrl, this._tblAux, this.chkGenerarOrdenPago.Checked);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                if (result.Result == ResultValue.OK)
                {
                    MessageForm frm = new MessageForm(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK)+" "+result.ExtraField,MessageType.Confirmation);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    this.Invoke(this.saveDelegate);
                }
                else
                { 
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        
        }

        #endregion

    }
}