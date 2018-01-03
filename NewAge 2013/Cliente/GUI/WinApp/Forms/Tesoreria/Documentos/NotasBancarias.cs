using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using SentenceTransformer;
using System.Threading;
using System.Globalization;
using NewAge.Forms.Dialogs.Documentos;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class NotasBancarias : DocumentAuxiliarForm
    {
        //public NotasBancarias()
        //{
        //    this.InitializeComponent();
        //}

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            if (this.resultOK)
            {
                this.CleanHeader(true);
                this.EnableHeader(true);
                this.gcDocument.DataSource = null;
                this.gvDocument.RefreshData();
            }
            else
            {
                //Remueve el ultimo registro
                this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                        || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());

                this.gcDocument.DataSource = this.Comprobante.Footer;
                this.gvDocument.RefreshData();
            }
        }

        #endregion

        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private int _currency = (int)TipoMoneda_LocExt.Local;

        //Variables para no repetir validaciones sobre un control
        private bool _cuentaBancFocus;
        private bool _notaBancFocus;

        //Variables para el servidor
        private decimal _tc = 0;
        private byte _mdaOrigen;
        private string _compNro = string.Empty;
        private DTO_glDocumentoControl _docCtrl = null;
        private DTO_coDocumentoRevelacion _revelacion;
        private DTO_tsBancosCuenta _bancoCuenta;
        private DTO_coPlanCuenta _cuentaContra;

        //Variables de Reportes
        private string reportName;

        #endregion

        #region Propiedades

        /// <summary>
        /// Cambia la informacion de un objeto con los datos por uno de tipo temporal
        /// </summary>
        protected override DTO_Comprobante Comprobante
        {
            set 
            {
                this.data = value;
                base.Comprobante = value; 
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected decimal TraerValor()
        {
            try
            {
                base.CalcularTotal();

                decimal val = 0;
                if (this._mdaOrigen == (byte)TipoMoneda.Local)
                    val = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                else
                    val = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

                return val * -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "TraerValor"));
                return 0;
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.NotasBancarias;
            InitializeComponent();

            this.frmModule = ModulesPrefix.ts;
            base.SetInitParameters();

            _bc.InitMasterUC(this.masterCuentaBanc, AppMasters.tsBancosCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterNotaBanc, AppMasters.tsNotaBancaria, true, true, true, false);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.EnableHeader(true);
            this.dtFecha.Enabled = true;

            string docIDComprobante = _bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_DocContableNotasBancarias);
            if(!string.IsNullOrWhiteSpace(docIDComprobante))
            {
                DTO_coDocumento doc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, docIDComprobante, true);
                if (doc == null || string.IsNullOrWhiteSpace(doc.ComprobanteID.Value))
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_InvalidCompDoc));
                else
                {
                    this.comprobanteID = doc.ComprobanteID.Value;
                    this.prefijoID = doc.PrefijoID.Value;
                    this.txtPrefix.Text = this.prefijoID;
                }
            }

            base.AfterInitialize();
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            this.masterCuentaBanc.Value = string.Empty;
            this.masterNotaBanc.Value = string.Empty;
            this.txtObservacionHeader.Text = string.Empty;

            this._compNro = "0";
            this.comprobanteID = string.Empty;
            this._docCtrl = null;
            this._bancoCuenta = null;
            this._cuentaContra = null;
            this._revelacion = null;
            this.Comprobante = new DTO_Comprobante();

            this.masterCuenta.Value = string.Empty;
            this.Cuenta = null;
            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterCuentaBanc.EnableControl(enable);
            this.masterNotaBanc.EnableControl(enable);
            this.txtObservacionHeader.Enabled = enable;

            this.dtFecha.Enabled = enable;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override Object LoadTempHeader()
        {
            DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
            header.ComprobanteID.Value = this.comprobanteID;
            header.ComprobanteNro.Value = 0;
            header.EmpresaID.Value = this.empresaID;
            header.Fecha.Value = this.dtFecha.DateTime;
            header.NumeroDoc.Value = 0;
            header.MdaOrigen.Value = this._mdaOrigen;
            header.MdaTransacc.Value = this.txtMonedaOrigen.Text;
            header.PeriodoID.Value = this.dtPeriod.DateTime;
            header.TasaCambioBase.Value = this._tc;
            header.TasaCambioOtr.Value = this._tc;

            return header;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            if (this._mdaOrigen == (byte)TipoMoneda.Foreign)
                this.monedaId = this.monedaExtranjera;
            else
                this.monedaId = this.monedaLocal;

            this.txtMonedaOrigen.Text = this.monedaId;

            //Si la empresa no permite mmultimoneda
            if (!this.multiMoneda)
                this._tc = 0;
            else
            {
                this._tc = this.LoadTasaCambio(Convert.ToInt32(this._mdaOrigen));
                if (this._tc == 0)
                {
                    this.validHeader = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                    return false;
                }
            }

            if (!fromTop)
                this.validHeader = true;
            else
                this.validHeader = false;

            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            //Valida datos en la maestra de documento contable
            if (!this.masterCuentaBanc.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCuentaBanc.CodeRsx);

                MessageBox.Show(msg);
                this.masterCuentaBanc.Focus();

                return false;
            }
            else if (this._cuentaContra == null)
            {
                string msg = _bc.GetResourceError(DictionaryMessages.Err_Co_DocNoCta);
                MessageBox.Show(string.Format(msg, this._bancoCuenta.coDocumentoID.Value));
                this.masterCuentaBanc.Focus(); 
                
                return false;
            }

            //Valida datos en la maestra de cuentas
            if (!this.masterNotaBanc.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterNotaBanc.CodeRsx);

                MessageBox.Show(msg);
                this.masterNotaBanc.Focus();

                return false;
            }

            //Valida que la observacion tenga información
            if (string.IsNullOrWhiteSpace(this.txtObservacionHeader.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescrDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);

                MessageBox.Show(msg);
                this.txtObservacionHeader.Focus();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            base.AddNewRow();
            
            if (this.Comprobante.Footer.Count == 0)
            {
                this.masterCuenta.Value = this._cuentaContra.ID.Value;
                this.masterCuenta.Focus();
                this.masterTercero.Focus();
                //this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this.masterCuenta.Value, true);
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);

                //if (FormProvider.Master.LoadFormTB)
                //{
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemPrint.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar del control de documento contable
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterCuentaBanc_Enter(object sender, EventArgs e)
        {
            this._cuentaBancFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar del control de documento contable
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterNotaBanc_Enter(object sender, EventArgs e)
        {
            this._notaBancFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la cuenta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCuentaBanc_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._cuentaBancFocus)
                {
                    this._cuentaBancFocus = false;
                    if (this.masterCuentaBanc.ValidID)
                    {
                        this._bancoCuenta = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterCuentaBanc.Value, true);
                        DTO_coDocumento coDoc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._bancoCuenta.coDocumentoID.Value, true);

                        if (!string.IsNullOrWhiteSpace(coDoc.CuentaLOC.Value))
                        {
                            this._cuentaContra = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, coDoc.CuentaLOC.Value, true);
                            this.txtMonedaOrigen.Text = this.monedaLocal;
                            this._mdaOrigen = (byte)TipoMoneda.Local;
                            this.monedaId = this.monedaLocal;
                        }
                        else if (!string.IsNullOrWhiteSpace(coDoc.CuentaEXT.Value))
                        {
                            this._cuentaContra = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, coDoc.CuentaEXT.Value, true);
                            this.txtMonedaOrigen.Text = this.monedaExtranjera;
                            this._mdaOrigen = (byte)TipoMoneda.Foreign;
                            this.monedaId = this.monedaExtranjera;
                        }
                        else
                        {
                            this._cuentaContra = null;
                            this.txtMonedaOrigen.Text = string.Empty;
                            string msg = _bc.GetResourceError(DictionaryMessages.Err_Co_DocNoCta);
                            MessageBox.Show(string.Format(msg, this._bancoCuenta.coDocumentoID.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "masterDocumentoCont_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar del control de documento contable
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void masterNotaBanc_Leave(object sender, EventArgs e)
        {
            try
            {
                //No borrar por cambios que vana  venir sobre el doc
                if (this._notaBancFocus)
                {
                    this._notaBancFocus = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "masterNotaBanc_Leave"));
            }
        }

        /// <summary>
        /// Incluye revelación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevelaciones_Click(object sender, EventArgs e)
        {
            string docContable = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocuContableDefecto);
            DTO_coDocumento coDoc = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, docContable, true);
            if (coDoc != null)
            {
                ModalRevelaciones modalRev = new ModalRevelaciones(coDoc.NotaRevelacionID.Value);
                modalRev.ShowDialog();
                this._revelacion = modalRev.DocRevelacion;
            }
        }

        #endregion

        #region Eventos header superior

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_Leave(object sender, EventArgs e)
        {
            base.dtFecha_Leave(sender, e);
            bool tc = this.AsignarTasaCambio(true);
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                DTO_Comprobante comp = new DTO_Comprobante();
                try
                {
                    if (this.Comprobante == null || this.Comprobante.Footer.Count == 0)
                    {
                        comp.Header = (DTO_ComprobanteHeader)this.LoadTempHeader(); 
                        comp.Footer = new List<DTO_ComprobanteFooter>();
                        this.Comprobante = comp;
                        this.Comprobante.TipoDoc = DocumentoTipo.DocInterno;

                        this.LoadData(true);
                        //this.UpdateTemp(this.Comprobante);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "grlDocument_Enter: " + ex.Message));
                }
                #endregion
            }
            else
            {
                this.masterCuentaBanc.Focus();
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
            if (!this.validHeader)
                this.masterCuentaBanc.Focus();
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                base.TBNew();

                if (this.cleanDoc)
                {
                    this.EnableHeader(true);
                    this.CleanHeader(true);

                    this.data = new DTO_Comprobante();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;
                    
                    this.masterCuentaBanc.Focus();
                    this.CalcularTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();
                
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();

                if (this.ValidGrid())
                {
                    //Si es cta debito cambia los valores
                    decimal ML = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture) *-1;
                    decimal ME = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture) *-1;

                    #region Documento ctrl

                    this._docCtrl = new DTO_glDocumentoControl();
                    this._docCtrl.NumeroDoc.Value = 0;
                    this._docCtrl.DocumentoNro.Value = 0;
                    this._docCtrl.DocumentoID.Value = this.documentID;
                    this._docCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this._docCtrl.ComprobanteID.Value = this.comprobanteID;
                    this._docCtrl.Fecha.Value = this.dtFecha.DateTime;
                    this._docCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                    this._docCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                    this._docCtrl.MonedaID.Value = this.txtMonedaOrigen.Text;
                    this._docCtrl.CuentaID.Value = this._cuentaContra.ID.Value;
                    this._docCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._docCtrl.TerceroID.Value = this.defTercero;
                    this._docCtrl.DocumentoTercero.Value = "Nota Bancaria";
                    this._docCtrl.ProyectoID.Value = this.defProyecto;
                    this._docCtrl.CentroCostoID.Value = this.defCentroCosto;
                    this._docCtrl.LugarGeograficoID.Value = this.defLugarGeo;
                    this._docCtrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                    this._docCtrl.PrefijoID.Value = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    this._docCtrl.Descripcion.Value = this.txtDocDesc.Text;
                    this._docCtrl.Observacion.Value = "NOTA BANCARIA";
                    this._docCtrl.TasaCambioDOCU.Value = this._tc;
                    this._docCtrl.TasaCambioCONT.Value = this._tc;
                    this._docCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    this._docCtrl.seUsuarioID.Value = _bc.AdministrationModel.User.ReplicaID.Value;
                    this._docCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                    this._docCtrl.Valor.Value = this.txtMonedaOrigen.Text == this.monedaLocal ? ML * -1 : ME * -1;
                    this._docCtrl.Iva.Value = 0;

                    #endregion
                    #region Crea el ultimo registro

                    DTO_ComprobanteFooter last = new DTO_ComprobanteFooter();
                    last.Index = this.Comprobante.Footer.Count;
                    last.CuentaID.Value = this._cuentaContra.ID.Value;
                    last.TerceroID.Value = this._docCtrl.TerceroID.Value;
                    last.ProyectoID.Value = this._docCtrl.ProyectoID.Value;
                    last.CentroCostoID.Value = this._docCtrl.CentroCostoID.Value;
                    last.LineaPresupuestoID.Value = this._docCtrl.LineaPresupuestoID.Value;
                    last.ConceptoCargoID.Value = this.defConceptoCargo;
                    last.LugarGeograficoID.Value = this.defLugarGeo;
                    last.PrefijoCOM.Value = this._docCtrl.PrefijoID.Value;
                    last.DocumentoCOM.Value = this._docCtrl.DocumentoTercero.Value;
                    last.ActivoCOM.Value = string.Empty;
                    last.ConceptoSaldoID.Value = this._cuentaContra.ConceptoSaldoID.Value;
                    last.IdentificadorTR.Value = 0;
                    last.Descriptivo.Value = this.txtDocDesc.Text.Length <= 50 ? this.txtDocDesc.Text : this.txtDocDesc.Text.Substring(0, 50);
                    last.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;

                    last.vlrBaseML.Value = 0;
                    last.vlrBaseME.Value = 0;
                    last.vlrMdaLoc.Value = ML;
                    last.vlrMdaExt.Value = ME;
                    last.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? last.vlrMdaLoc.Value : last.vlrMdaExt.Value;
                    last.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

                    this.Comprobante.Footer.Add(last);

                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                result = _bc.AdministrationModel.NotasBancarias_Radicar(this.documentID, this._docCtrl, this.Comprobante, this._revelacion);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (result.Result.Equals(ResultValue.OK))
                {
                    string[] vars = Regex.Split(result.ResultMessage, "&&");
                    //this.compNro = vars.ElementAt(2);

                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.resultOK = true;
                }
                else
                {
                    this.resultOK = false;
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NotasBancarias.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
