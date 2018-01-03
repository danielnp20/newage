using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Globalization;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RadicacionRecibido : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _dtoCtrl;
        private DTO_cpCuentaXPagar _ctaXPagar;
        private List<DTO_MasterBasic> _causales;
        private List<DTO_prRecibidoAprob> _listRecibidosNoFact = null;
        private FormTypes _frmType = FormTypes.Document;
        private string _nDias;
        private byte estado;
        private int _tipoMoneda = 0;

        private string monedaLocal;
        private string monedaExtranjera;
        private DateTime fechaRadicacion;
        private DateTime fechaVencimiento;
        private decimal _tasaCambio = 0;
        private List<int> select = new List<int>();
        private DTO_prProveedor _proveedor = null;
        private decimal _costoML = 0;
        private decimal _costoME = 0;
        private decimal _costoIvaML = 0;
        private decimal _costoIvaME = 0;


        protected string _frmNewName;
        private string _frmName;
        //Variables Protegidas
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected bool multiMoneda;
        protected bool disableValidate = false;
        //Internas del formulario
        protected string areaFuncionalID;
        protected string prefijoID;
        protected string terceroID;
        protected string comprobanteID;
        protected bool dataLoaded = false;
        protected int indexFila = 0;
        protected bool isValid = true;
        protected bool deleteOP = false;
        protected bool newDoc = false;
        protected bool newReg = false;
        protected string lastColName = string.Empty;
        protected PasteOpDTO pasteRet;
        //protected bool RowEdited = false;
        protected bool hasChanges = false;
        protected DTO_glConsulta consulta = null;
        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        //Variables para importar
        protected string format;
        protected string formatSeparator = "\t";
        protected string unboundPrefix = "Unbound_";

        #endregion

        public RadicacionRecibido()
        {
            this.SetInitParameters();
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            if (!string.IsNullOrWhiteSpace(this._frmNewName))
                this._frmName = this._frmNewName;

            this.LoadDocumentInfo(true);
            this.frmModule = ModulesPrefix.cp;

            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            #region Carga la info de las actividades
            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

            if (actividades.Count != 1)
            {
                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                string actividadFlujoID = actividades[0];
                this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);

                this.AfterInitialize();
            }
            #endregion
        }        

        #region Delegados

        protected delegate void RefreshGrid();
        protected RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected virtual void RefreshGridMethod() { }

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected virtual void SaveMethod() { this.RefreshDocument(); }

        private delegate void ActualizarFactura();
        private ActualizarFactura actualizarFacturaDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        /// <summary>
        /// Actualiza la informacion y el estado de la factura
        /// </summary>
        private void ActualizarFacturaMethod()
        {
            try
            {
                #region Asigna valores antes de actualizar factura 
		        var estado = Convert.ToByte((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);
                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.Fecha.Value = DateTime.Now;
                this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.Estado.Value = estado;
                this._dtoCtrl.FechaDoc.Value = this.fechaRadicacion;
                this._dtoCtrl.Descripcion.Value = "Radicacion Factura x Recibidos";
                this._dtoCtrl.Valor.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Iva.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorIVALocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorIVAExtr.EditValue, CultureInfo.InvariantCulture); 

                string mdaFact = this.monedaLocal;
                this._tipoMoneda = Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);
                if (this._tipoMoneda == (int)TipoMoneda.Foreign)
                    mdaFact = this.monedaExtranjera;

                if (this._tasaCambio == 0)
                {
                    this._dtoCtrl.TasaCambioCONT.Value = this.multiMoneda ? this._bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.fechaRadicacion) : 0;
                    this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                }
                else
                {
                    this._dtoCtrl.TasaCambioCONT.Value = this._tasaCambio;
                    this._dtoCtrl.TasaCambioDOCU.Value = this._tasaCambio;
                
                }
	            #endregion
                #region Asigna valores DTO_cpCuentasXPagar
                this._ctaXPagar.ConceptoCxPID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPxDefecto); // (this.cbNotaCredito.Checked)? 

                this._ctaXPagar.Valor.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorLocal.Value = Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorExtra.Value = Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorTercero.Value = this._ctaXPagar.Valor.Value;
                this._ctaXPagar.IVA.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorIVALocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorIVAExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.MonedaPago.Value = mdaFact;
                this._ctaXPagar.FacturaFecha.Value = dtFechaFactura.DateTime;
                this._ctaXPagar.VtoFecha.Value = this.dtFechaVencimiento.DateTime;
                this._ctaXPagar.DistribuyeImpLocalInd.Value = false;
                #endregion
                int numDoc = 0;
                bool devolucionInd = false;
                if (estado == (byte)EstadoDocControl.Devuelto)
                {
                    #region Ingresa las causas para devolucion de documentos
                    Dictionary<string, string> causas = new Dictionary<string, string>();
                    foreach (var index in this.select)
                        causas.Add(index.ToString(), ((DTO_MasterBasic)this._causales.ToList()[index]).Descriptivo.Value);

                    this._ctaXPagar.Dato1.Value = this.select.Any(x => x == 0) ? causas.Where(x => x.Key == "0").First().Value : string.Empty;
                    this._ctaXPagar.Dato2.Value = this.select.Any(x => x == 1) ? causas.Where(x => x.Key == "1").First().Value : string.Empty;
                    this._ctaXPagar.Dato3.Value = this.select.Any(x => x == 2) ? causas.Where(x => x.Key == "2").First().Value : string.Empty;
                    this._ctaXPagar.Dato4.Value = this.select.Any(x => x == 3) ? causas.Where(x => x.Key == "3").First().Value : string.Empty;
                    this._ctaXPagar.Dato5.Value = this.select.Any(x => x == 4) ? causas.Where(x => x.Key == "4").First().Value : string.Empty;
                    this._ctaXPagar.Dato6.Value = this.select.Any(x => x == 5) ? causas.Where(x => x.Key == "5").First().Value : string.Empty;
                    this._ctaXPagar.Dato7.Value = this.select.Any(x => x == 6) ? causas.Where(x => x.Key == "6").First().Value : string.Empty;
                    this._ctaXPagar.Dato8.Value = this.select.Any(x => x == 7) ? causas.Where(x => x.Key == "7").First().Value : string.Empty;
                    this._ctaXPagar.Dato9.Value = this.select.Any(x => x == 8) ? causas.Where(x => x.Key == "8").First().Value : string.Empty;
                    this._ctaXPagar.Dato10.Value = this.select.Any(x => x == 9) ? causas.Where(x => x.Key == "9").First().Value : string.Empty;
                    devolucionInd = true;
                    #endregion
                }
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject obj = this._bc.AdministrationModel.Recibido_RadicarDevolver(this.documentID,this._listRecibidosNoFact, _dtoCtrl, _ctaXPagar, true, out numDoc, devolucionInd);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = estado != (byte)EstadoDocControl.Devuelto ? this._bc.SendDocumentMail(MailType.Reject, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true) :true;              
                if (isOK)
                {
                    this.RefreshDocument();
                    this._dtoCtrl = null;
                    this._ctaXPagar = null;
                }            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "ActualizarFacturaMethod" + ex.Message));
            }
        }

        private delegate void RadicarFactura();
        private RadicarFactura radicarFacturaDelegate;
        /// <summary>
        /// Radica la factura en el sistema
        /// </summary>
        private void RadicarFacturaMethod()
        {
            try
            {
                #region Campos variables DTO_glDocumentoControl
                this._dtoCtrl = new DTO_glDocumentoControl();
                this._dtoCtrl.DocumentoID.Value = AppDocuments.CausarFacturas;
                this._dtoCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._dtoCtrl.PrefijoID.Value = this.txtPrefix.Text;
                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.FechaDoc.Value = this.dtFechaFactura.DateTime;
                this._dtoCtrl.Descripcion.Value = "Radicacion Factura x Recibidos";
                this._dtoCtrl.Valor.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Iva.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorIVALocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorIVAExtr.EditValue, CultureInfo.InvariantCulture); 

                string mdaFact = this.monedaLocal;
                this._tipoMoneda = Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);
                if (this._tipoMoneda == (int)TipoMoneda.Foreign)
                    mdaFact = this.monedaExtranjera;

                this._dtoCtrl.MonedaID.Value = mdaFact;
                if (this._tasaCambio == 0)
                {
                    this._dtoCtrl.TasaCambioCONT.Value = this.multiMoneda ? this._bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.fechaRadicacion) : 0;
                    this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                }
                else
                {
                    this._dtoCtrl.TasaCambioCONT.Value = this._tasaCambio;
                    this._dtoCtrl.TasaCambioDOCU.Value = this._tasaCambio;
                }
                this._dtoCtrl.TerceroID.Value = this._proveedor.TerceroID.Value;
                this._dtoCtrl.DocumentoTercero.Value = txtFactura.Text.Trim();
                this._dtoCtrl.Estado.Value = (byte)EstadoDocControl.Radicado;
                this._dtoCtrl.seUsuarioID.Value = this.userID;
                this._dtoCtrl.Fecha.Value = DateTime.Now;

                #endregion
                #region Campos variables DTO_cpCuentasXPagar
                this._ctaXPagar = new DTO_cpCuentaXPagar();
                this._ctaXPagar.ConceptoCxPID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPxDefecto); //_bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPNotaCredito)
                this._ctaXPagar.Valor.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.IVA.Value = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorIVALocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorIVAExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorLocal.Value = Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorExtra.Value = Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorTercero.Value = this._ctaXPagar.Valor.Value;
                this._ctaXPagar.MonedaPago.Value = mdaFact;
                this._ctaXPagar.FacturaFecha.Value = this.dtFechaFactura.DateTime;
                this._ctaXPagar.VtoFecha.Value = this.dtFechaVencimiento.DateTime;
                this._ctaXPagar.DistribuyeImpLocalInd.Value = false;
                this._ctaXPagar.RadicaCodigo.Value = this.txtNroRadicado.Text.Trim();
                #endregion
                int numDoc = 0;
                   
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject obj = this._bc.AdministrationModel.Recibido_RadicarDevolver(this.documentID, this._listRecibidosNoFact, _dtoCtrl, _ctaXPagar, false, out numDoc, false);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                    this.RefreshDocument();

                this._dtoCtrl = null;
                this._ctaXPagar = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionFacturas.cs-RadicarFacturaMethod" + ex.Message));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            //Llena Combo de Tipo Movimiento
            TablesResources.GetTableResources(this.cmbTipoMovimiento, typeof(TipoMovimientoCXP));
            TablesResources.GetTableResources(this.cmbTipoModena, typeof(TipoMoneda_LocExt));
            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, true);

            this.dtFechaVencimiento.DateTime = this.fechaVencimiento;
            this.dtFecha.Enabled = false;
            this.cmbTipoModena.SelectedIndex = 0;
            this.txtValorLocal.Text = "0";
            this.txtValorExtr.Text = "0";
            this.txtValorIVAExtr.Text = "0";
            this.txtValorIVALocal.Text = "0";
            this.masterProveedor.Focus();

            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;

        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    if (basicDTO == null)
                        MessageBox.Show("El área funcional del usuario NO existe");
                    else
                        this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                        this.txtPrefix.Text = this.prefijoID;

                    this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    if (this.documentID == AppDocuments.ComprobanteManual || this.documentID == AppDocuments.DocumentoContable)
                        this.dtPeriod.Enabled = true;
                    else
                        this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Devuelve el documento control asociado al tercero 
        /// </summary>
        /// <returns></returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                DTO_glDocumentoControl doc = null;
                if (this._proveedor != null)
                {
                    string tercero = this._proveedor.TerceroID.Value;
                    string numDoc = txtFactura.Text.Trim();
                    doc = this._bc.AdministrationModel.glDocumentoControl_GetExternalDoc(AppDocuments.CausarFacturas, tercero, numDoc);
                    if (doc != null)
                        this._listRecibidosNoFact = this._bc.AdministrationModel.Recibido_GetRecibidoNoFacturado(this.documentID, this._actFlujo.ID.Value, this.masterProveedor.Value,true, doc.NumeroDoc.Value.Value);

                    this.gcDocument.RefreshDataSource();
                }

                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Si existe el documento carga la informacion del mismo
        /// </summary>
        private void LoadDocumentExist()
        {
            try
            {
                this.txtFactura.Text = this._dtoCtrl.DocumentoTercero.Value;               
                this.dtFechaFactura.DateTime = this._ctaXPagar.FacturaFecha.Value.Value;
                this.dtFechaVencimiento.DateTime = this._ctaXPagar.VtoFecha.Value.Value;
                this.txtValorLocal.EditValue = !string.IsNullOrEmpty(this._ctaXPagar.ValorLocal.ToString()) ? this._ctaXPagar.ValorLocal.Value : 0;
                this.txtValorExtr.EditValue = !string.IsNullOrEmpty(this._ctaXPagar.ValorExtra.ToString()) ? this._ctaXPagar.ValorExtra.Value : 0;
                this.txtValorIVALocal.EditValue = this._ctaXPagar.IVA.Value;
                this.txtValorIVAExtr.EditValue =this.txtTasaCambio.EditValue != null && Convert.ToDecimal(this.txtTasaCambio.EditValue) != 0 ?
                                                (this._ctaXPagar.IVA.Value.Value /Convert.ToDecimal(this.txtTasaCambio.EditValue)) : 0;
                this.txtTotalLocal.EditValue = (this._ctaXPagar.ValorLocal.Value.Value + _ctaXPagar.IVA.Value.Value);
                this.txtTotalExtr.EditValue = (this._ctaXPagar.ValorExtra.Value.Value + Convert.ToDecimal(this.txtValorIVAExtr.EditValue, CultureInfo.InvariantCulture));
                this.txtDescripcion.Text = this._dtoCtrl.Observacion.Value;
                this.txtNroRadicado.Text = this._ctaXPagar.NumeroRadica.Value.ToString();
                this.txtTasaCambio.EditValue = this._dtoCtrl.TasaCambioDOCU.Value;
                this._tasaCambio = this._dtoCtrl.TasaCambioDOCU.Value.HasValue ? this._dtoCtrl.TasaCambioDOCU.Value.Value : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionRecibido.cs-LoadDocumentExist"));
            }
        }

        /// <summary>
        /// Calcula valor factura + iva
        /// </summary>
        private void CalcularTotal()
        {
            decimal valor = 0;
            decimal iva = 0;

            if (!string.IsNullOrEmpty(this.txtValorLocal.Text))
                valor = Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(this.txtValorIVALocal.Text))
                iva = Convert.ToDecimal(this.txtValorIVALocal.EditValue, CultureInfo.InvariantCulture);

            this.txtTotalLocal.EditValue = (valor + iva);

        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.dtFechaFactura.Enabled = estado;
            this.dtFechaVencimiento.Enabled = estado;
            //this.cmbTipoModena.Enabled = this.multiMoneda && estado ? true : false;
            this.masterProveedor.Enabled = estado;
            this.txtFactura.Enabled = estado;
            this.txtValorLocal.Enabled = estado;
            this.txtValorExtr.Enabled = estado;
            this.txtValorIVALocal.Enabled = estado;
            this.txtValorIVAExtr.Enabled = estado;
            this.txtTotalExtr.Enabled = estado;
            this.txtTotalLocal.Enabled = estado;
            this.txtDescripcion.Enabled = estado;
            this.txtTasaCambio.Enabled = estado;       
        }

        /// <summary>
        /// Valida los campos obligatorios
        /// </summary>
        /// <returns></returns>
        private string FieldsObligated()
        {
            var message = string.Empty;
            var field = string.Empty;
            #region Valida campos obligatorios
            field = string.IsNullOrEmpty(dtFechaFactura.Text) ? field = field + "\n" + this.lblFechaFactura.Text : string.Empty;
            field = string.IsNullOrEmpty(dtFechaVencimiento.Text) ? field = field + "\n" + this.lblFechaVencimiento.Text : field;
            field = string.IsNullOrEmpty(masterProveedor.Value) ? field = field + "\n" + this._bc.GetResource(LanguageTypes.Forms, AppMasters.coTercero + "_lblTitle") : field;
            field = string.IsNullOrEmpty(txtFactura.Text) ? field = field + "\n" + this.lblFactura.Text : field;
            field = string.IsNullOrEmpty(txtValorLocal.Text) ? field = field + "\n" + this.lblValorLocal.Text : field;
            field = string.IsNullOrEmpty(txtValorIVALocal.Text) ? field = field + "\n" + this.lblValorIVALocal.Text : field;

            if (!string.IsNullOrEmpty(field))
            {
                message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaFieldObligated);
                message = string.Format(message, field);
                return message;
            } 
            #endregion

            #region Valida fecha vencimiento
            if (this.dtFechaFactura.DateTime > this.dtFechaVencimiento.DateTime)
            {
                message += "\n" + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvFechaFact);
                return message;
            }
            #endregion

            #region Valida el valor de la factura
            decimal valorFactura = 0;
            this._tipoMoneda = Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);
            valorFactura = this._tipoMoneda == (int)TipoMoneda.Local ? Convert.ToDecimal(this.txtValorLocal.EditValue, CultureInfo.InvariantCulture) : Convert.ToDecimal(this.txtValorExtr.EditValue, CultureInfo.InvariantCulture);
            if (valorFactura == 0)
            {
                message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValueDocumentInvalid);
                return message; 
            }
            #region Valida si debe existir tasa de cambio
            bool existMdaExtr = false;
            foreach (var rec in this._listRecibidosNoFact.FindAll(x=>x.Seleccionar.Value.Value))
            {
                existMdaExtr = rec.Detalle.Any(x => x.OrigenMonetario.Value == (byte)TipoMoneda_LocExt.Foreign);
                if (existMdaExtr) break;
            }
            if (existMdaExtr && this._tasaCambio == 0)
            {
                message = this._bc.GetResource(LanguageTypes.Messages, "Debe digitar la tasa de cambio");
                return message;
            }
            #endregion
            return message; 
            #endregion

            
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.txtNroRadicado.Text = string.Empty;
            this.dtFechaFactura.Text = this.dtFecha.Text;
            this.dtFechaVencimiento.Text = this.dtFecha.Text;
            this._costoML = 0;
            this._costoME = 0;
            this._costoIvaML = 0;
            this._costoIvaME = 0;
            this.masterProveedor.Value = string.Empty;
            this.txtFactura.Text = string.Empty;
            this.txtValorLocal.Text = "0";
            this.txtValorExtr.Text = "0";
            this.txtValorIVALocal.Text = "0";
            this.txtValorIVAExtr.Text = "0";
            this.txtTotalLocal.Text = "0";
            this.txtTotalExtr.Text = "0";
            this.txtTasaCambio.EditValue = "0";
            this.txtDescripcion.Text = string.Empty;
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            this.cmbTipoModena.SelectedIndex = 0;
            this.cmbTipoMovimiento.SelectedIndex = 0;
            this._proveedor = null;           
            this.LoadData(true);
        }

        /// <summary>
        /// Valida controles de fechas
        /// </summary>
        private void ValidarFechas()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFechaFactura.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFechaFactura.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaFactura.DateTime = new DateTime(currentYear, currentMonth, minDay);

            this.dtFechaVencimiento.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            //this.dtFechaVencimiento.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaVencimiento.DateTime = new DateTime(currentYear, currentMonth, minDay).AddDays(double.Parse(this._nDias));
        }

        /// <summary>
        /// Valida la conssitencia de info de los valores de monedas de Recibidos y Facturas
        /// </summary>
        private bool ValidaConsistenciaDatos()
        {
            try
            {
                decimal porcMargen = string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_MargenValorRadicaVSRecibido)) ? 0 : Convert.ToDecimal(this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_MargenValorRadicaVSRecibido));

                foreach (DTO_prRecibidoAprob rec in this._listRecibidosNoFact.FindAll(x => x.Seleccionar.Value.Value))
                {
                    if (rec.CostoML.Value > 0)
                    {
                        decimal vlrDif = Math.Abs(rec.CostoML.Value.Value - rec.CostoMLRecib.Value.Value);
                        decimal vlrMargen = rec.CostoML.Value.Value * (porcMargen / 100);
                        if (vlrDif > vlrMargen)
                        {
                            MessageBox.Show("El margen del costo con el recibido  " + rec.PrefDoc + " es muy alto, verifique");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "ValidaConsistenciaDatos"));
            }
            return true;

        }

        /// <summary>
        /// Calcula y muestra el valor total de las monedas de los recibidos seleccionados
        /// </summary>
        private void ValorTotalSelect(bool selectInd, int rowhandle,bool calculateAll = false)
        {
            try
            {
                if (this._listRecibidosNoFact != null && this._listRecibidosNoFact.Count > 0)
                {
                    if (!calculateAll)
                    {
                        if (selectInd)
                        {
                            this._costoML += this._listRecibidosNoFact[rowhandle].CostoML.Value.Value;
                            this._costoME += this._listRecibidosNoFact[rowhandle].CostoME.Value.Value;
                            this._costoIvaML += this._listRecibidosNoFact[rowhandle].CostoIvaML.Value.Value;
                            this._costoIvaME += this._listRecibidosNoFact[rowhandle].CostoIvaME.Value.Value;
                        }
                        else
                        {
                            this._costoML -= this._listRecibidosNoFact[rowhandle].CostoML.Value.Value;
                            this._costoME -= this._listRecibidosNoFact[rowhandle].CostoME.Value.Value;
                            this._costoIvaML -= this._listRecibidosNoFact[rowhandle].CostoIvaML.Value.Value;
                            this._costoIvaME -= this._listRecibidosNoFact[rowhandle].CostoIvaME.Value.Value;
                        }
                    }
                    else
                    {
                        this._costoML = 0;
                        this._costoME = 0;
                        this._costoIvaML = 0;
                        this._costoIvaME = 0;
                        foreach (DTO_prRecibidoAprob recibido in this._listRecibidosNoFact)
                        {
                            if (recibido.Seleccionar.Value.Value)
                            {
                                this._costoML += recibido.CostoML.Value.Value;
                                this._costoME += recibido.CostoME.Value.Value;
                                this._costoIvaML += recibido.CostoIvaML.Value.Value;
                                this._costoIvaME += recibido.CostoIvaME.Value.Value;
                            }
                        }
                    }
                }
                this.txtValorLocal.EditValue = this._costoML;
                this.txtValorExtr.EditValue = this._costoME;
                this.txtValorIVALocal.EditValue = this._costoIvaML;
                this.txtValorIVAExtr.EditValue = this._costoIvaME;
                this.txtTotalLocal.EditValue = this._costoML + this._costoIvaML;
                this.txtTotalExtr.EditValue = this._costoME + this._costoIvaME;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "ValorTotalSelect"));
            }
        }

        /// <summary>
        /// Convierte el valor de las moneda segun el tipo de moneda selecccionado
        /// </summary>
        private void ConvertMoneda()
        {
            if (this._listRecibidosNoFact != null)
            {
                this._tipoMoneda = Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);

                foreach (DTO_prRecibidoAprob recibido in this._listRecibidosNoFact)
                {
                    foreach (DTO_prRecibidoAprobDet d in recibido.Detalle)
                    {
                        //Calcula la moneda segun el Origen Monetario
                        if (d.OrigenMonetario.Value == (byte)TipoMoneda_LocExt.Local)
                        {
                            d.ValorMLDet.Value = d.ValorUni.Value * d.CantidadRec.Value;
                            d.ValorMEDet.Value = this._tasaCambio != 0? Math.Round(d.ValorMLDet.Value.Value / this._tasaCambio,2) : 0;
                            d.ValorIvaMLDet.Value = d.ValorMLDet.Value * (d.PorcIVA/100);
                            d.ValorIvaMEDet.Value = d.ValorMEDet.Value * (d.PorcIVA / 100);
                        }
                        else
                        {
                            d.ValorMEDet.Value = d.ValorUni.Value * d.CantidadRec.Value;
                            d.ValorMLDet.Value = d.ValorMEDet.Value * this._tasaCambio;
                            d.ValorIvaMEDet.Value = d.ValorMEDet.Value * (d.PorcIVA / 100);
                            d.ValorIvaMLDet.Value = d.ValorMLDet.Value * (d.PorcIVA / 100);
                        }
                    }
                    recibido.CostoML.Value =  recibido.Detalle.Sum(x=>x.ValorMLDet.Value);
                    recibido.CostoME.Value = recibido.Detalle.Sum(x => x.ValorMEDet.Value);
                    recibido.CostoIvaML.Value = recibido.Detalle.Sum(x => x.ValorIvaMLDet.Value);
                    recibido.CostoIvaME.Value = recibido.Detalle.Sum(x => x.ValorIvaMEDet.Value);
                    recibido.CostoMLRecib.Value = recibido.Detalle.Sum(x => x.ValorTotMLRecibDet.Value);
                    recibido.CostoMERecib.Value = recibido.Detalle.Sum(x => x.ValorTotMERecibDet.Value);
                }             
                this.gcDocument.RefreshDataSource();
                this.ValorTotalSelect(false,0,true);
            }

        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.cpCausalesDev, null, null, true);
                this._causales = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.cpCausalesDev, count, 1, null, null, true).ToList();
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected void SetInitParameters()
        {
            this.InitializeComponent();
            this.documentID = AppDocuments.RadicacionRecibidos;

            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);

            this.actualizarFacturaDelegate = new ActualizarFactura(this.ActualizarFacturaMethod);
            this.radicarFacturaDelegate = new RadicarFactura(this.RadicarFacturaMethod);

            //Inicia las variables del formulario
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.cp;
            this.LoadData(true);
            this._nDias = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DiasPlazoPagoFact);
            if (string.IsNullOrEmpty(this._nDias))
                this._nDias = "0";
            //Carga info de las monedas
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga config de controles
            this.gvDocument.OptionsView.ColumnAutoWidth = true;
            this.InitControls();
            this.AddGridCols();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected void AddGridCols()
        {
            try
            {
                this.editValue.Mask.EditMask = "n2";
                #region Grilla Principal
                //Seleccionar
                BandedGridColumn selec = new BandedGridColumn();
                selec.FieldName = this.unboundPrefix + "Seleccionar";
                selec.Caption = "√";
                selec.UnboundType = UnboundColumnType.Boolean;
                selec.VisibleIndex = 0;
                selec.Width = 15;
                selec.Visible = true;
                selec.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                selec.AppearanceHeader.ForeColor = Color.Lime;
                selec.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                selec.AppearanceHeader.Options.UseTextOptions = true;
                selec.AppearanceHeader.Options.UseFont = true;
                selec.AppearanceHeader.Options.UseForeColor = true;
                selec.ColumnEdit = this.editChkBox;
                selec.ToolTip = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Seleccionar");
                this.gvDocument.Columns.Add(selec);
                this.bandGral.Columns.Add(selec);

                //Documento recibido
                BandedGridColumn docRec = new BandedGridColumn();
                docRec.FieldName = this.unboundPrefix + "PrefDoc";
                docRec.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                docRec.UnboundType = UnboundColumnType.String;
                docRec.VisibleIndex = 1;
                docRec.Width = 70;
                docRec.Visible = true;
                docRec.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(docRec);
                this.bandRecibidos.Columns.Add(docRec);

                //MonedaID
                BandedGridColumn moneda = new BandedGridColumn();
                moneda.FieldName = this.unboundPrefix + "MonedaID";
                moneda.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                moneda.UnboundType = UnboundColumnType.String;
                moneda.VisibleIndex = 2;
                moneda.Width = 80;
                moneda.Visible = false;
                moneda.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(moneda);
                this.bandRecibidos.Columns.Add(moneda);

                //CostoMLRecib
                BandedGridColumn CostoMLRecib = new BandedGridColumn();
                CostoMLRecib.FieldName = this.unboundPrefix + "CostoMLRecib";
                CostoMLRecib.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoML");
                CostoMLRecib.UnboundType = UnboundColumnType.Decimal;
                CostoMLRecib.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CostoMLRecib.AppearanceCell.ForeColor = Color.DarkSlateGray;
                CostoMLRecib.AppearanceCell.Options.UseForeColor = true;
                CostoMLRecib.ToolTip = "Valor en Mda Local del recibido e ingreso de inventarios";
                CostoMLRecib.AppearanceCell.Options.UseTextOptions = true;
                CostoMLRecib.VisibleIndex = 3;
                CostoMLRecib.Width = 70;
                CostoMLRecib.Visible = true;
                CostoMLRecib.ColumnEdit = this.editSpin;
                CostoMLRecib.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CostoMLRecib);
                this.bandRecibidos.Columns.Add(CostoMLRecib);

                //CostoMERecib
                BandedGridColumn CostoMERecib = new BandedGridColumn();
                CostoMERecib.FieldName = this.unboundPrefix + "CostoMERecib";
                CostoMERecib.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoME");
                CostoMERecib.UnboundType = UnboundColumnType.Decimal;
                CostoMERecib.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CostoMERecib.AppearanceCell.ForeColor = Color.DarkSlateGray;
                CostoMERecib.AppearanceCell.Options.UseForeColor = true;
                CostoMERecib.ToolTip = "Valor en Mda Extranjera del recibido e ingreso de inventarios";
                CostoMERecib.AppearanceCell.Options.UseTextOptions = true;
                CostoMERecib.VisibleIndex = 4;
                CostoMERecib.Width = 70;
                CostoMERecib.Visible = true;
                CostoMERecib.ColumnEdit = this.editSpin;
                CostoMERecib.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CostoMERecib);
                this.bandRecibidos.Columns.Add(CostoMERecib);

                //CostoML
                BandedGridColumn costoML = new BandedGridColumn();
                costoML.FieldName = this.unboundPrefix + "CostoML";
                costoML.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoML");
                costoML.UnboundType = UnboundColumnType.Decimal;
                costoML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                costoML.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); 
                costoML.AppearanceCell.Options.UseFont = true;
                costoML.AppearanceCell.Options.UseTextOptions = true;
                costoML.VisibleIndex = 5;
                costoML.Width = 120;
                costoML.Visible = true;
                costoML.ColumnEdit = this.editSpin;
                costoML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(costoML);
                this.bandFactura.Columns.Add(costoML);

                //CostoME
                BandedGridColumn costoME = new BandedGridColumn();
                costoME.FieldName = this.unboundPrefix + "CostoME";
                costoME.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoME");
                costoME.UnboundType = UnboundColumnType.Decimal;
                costoME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                costoME.AppearanceCell.Options.UseTextOptions = true;
                costoME.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                costoME.AppearanceCell.Options.UseFont = true;
                costoME.VisibleIndex = 6;
                costoME.Width = 120;
                costoME.Visible = true;
                costoME.ColumnEdit = this.editSpin;
                costoME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(costoME);
                this.bandFactura.Columns.Add(costoME);
                #endregion

                #region Grilla Detalle
                //Documento OrdenCompra
                BandedGridColumn docOC = new BandedGridColumn();
                docOC.FieldName = this.unboundPrefix + "PrefDoc";
                docOC.Caption = this._bc.GetResource(LanguageTypes.Forms, "Nro OC");
                docOC.UnboundType = UnboundColumnType.String;
                docOC.VisibleIndex = 0;
                docOC.Width = 70;
                docOC.Visible = true;
                docOC.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(docOC);

                BandedGridColumn MonedaOC = new BandedGridColumn();
                MonedaOC.FieldName = this.unboundPrefix + "MonedaOC";
                MonedaOC.Caption = this._bc.GetResource(LanguageTypes.Forms, "Moneda OC");
                MonedaOC.UnboundType = UnboundColumnType.String;
                MonedaOC.VisibleIndex = 1;
                MonedaOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                MonedaOC.AppearanceCell.Options.UseTextOptions = true;
                MonedaOC.Width = 72;
                MonedaOC.Visible = true;
                MonedaOC.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(MonedaOC);

                BandedGridColumn TasaOC = new BandedGridColumn();
                TasaOC.FieldName = this.unboundPrefix + "TasaOC";
                TasaOC.Caption = this._bc.GetResource(LanguageTypes.Forms, "Tasa OC");
                TasaOC.UnboundType = UnboundColumnType.Decimal;
                TasaOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TasaOC.AppearanceCell.Options.UseTextOptions = true;
                TasaOC.VisibleIndex = 2;
                TasaOC.Width = 55;
                TasaOC.Visible = true;
                TasaOC.ColumnEdit = this.editSpin;
                TasaOC.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(TasaOC);

                //ValorTotMLRecibDet
                BandedGridColumn ValorTotMLRecibDet = new BandedGridColumn();
                ValorTotMLRecibDet.FieldName = this.unboundPrefix + "ValorTotMLRecibDet";
                ValorTotMLRecibDet.Caption = this._bc.GetResource(LanguageTypes.Forms, "Vlr ML Ord");
                ValorTotMLRecibDet.UnboundType = UnboundColumnType.Decimal;
                ValorTotMLRecibDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorTotMLRecibDet.AppearanceCell.Options.UseTextOptions = true;
                ValorTotMLRecibDet.AppearanceCell.ForeColor = Color.DarkSlateGray;
                ValorTotMLRecibDet.AppearanceCell.Options.UseForeColor = true;
                ValorTotMLRecibDet.VisibleIndex = 3;
                ValorTotMLRecibDet.Width = 90;
                ValorTotMLRecibDet.Visible = true;
                ValorTotMLRecibDet.ColumnEdit = this.editSpin;
                ValorTotMLRecibDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorTotMLRecibDet);

                //ValorTotMLRecibDet
                BandedGridColumn ValorTotMERecibDet = new BandedGridColumn();
                ValorTotMERecibDet.FieldName = this.unboundPrefix + "ValorTotMERecibDet";
                ValorTotMERecibDet.Caption = this._bc.GetResource(LanguageTypes.Forms, "Vlr ME Ord");
                ValorTotMERecibDet.UnboundType = UnboundColumnType.Decimal;
                ValorTotMERecibDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorTotMERecibDet.AppearanceCell.Options.UseTextOptions = true;
                ValorTotMERecibDet.AppearanceCell.ForeColor = Color.DarkSlateGray;
                ValorTotMERecibDet.AppearanceCell.Options.UseForeColor = true;
                ValorTotMERecibDet.VisibleIndex = 4;
                ValorTotMERecibDet.Width = 80;
                ValorTotMERecibDet.Visible = true;
                ValorTotMERecibDet.ColumnEdit = this.editSpin;
                ValorTotMERecibDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorTotMERecibDet);

                //CodigoBSID
                BandedGridColumn CodigoBSID = new BandedGridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 5;
                CodigoBSID.Width = 60;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CodigoBSID);

                //inReferenciaID
                BandedGridColumn inReferenciaID = new BandedGridColumn();
                inReferenciaID.FieldName = this.unboundPrefix + "inReferenciaID";
                inReferenciaID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                inReferenciaID.UnboundType = UnboundColumnType.String;
                inReferenciaID.VisibleIndex = 6;
                inReferenciaID.Width = 60;
                inReferenciaID.Visible = true;
                inReferenciaID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(inReferenciaID);

                //Descriptivo
                BandedGridColumn Descriptivo = new BandedGridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 7;
                Descriptivo.Width = 200;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descriptivo);

                //SerialID
                BandedGridColumn SerialID = new BandedGridColumn();
                SerialID.FieldName = this.unboundPrefix + "SerialID";
                SerialID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                SerialID.UnboundType = UnboundColumnType.String;
                SerialID.VisibleIndex = 8;
                SerialID.Width = 50;
                SerialID.Visible = false;
                SerialID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SerialID);

                //UnidadInvID
                BandedGridColumn UnidadInvID = new BandedGridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
                UnidadInvID.VisibleIndex = 9;
                UnidadInvID.Width = 40;
                UnidadInvID.Visible = true;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(UnidadInvID);

                //CantidadRec
                BandedGridColumn Cantidad = new BandedGridColumn();
                Cantidad.FieldName = this.unboundPrefix + "CantidadRec";
                Cantidad.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
                Cantidad.UnboundType = UnboundColumnType.Decimal;
                Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cantidad.AppearanceCell.Options.UseTextOptions = true;
                Cantidad.VisibleIndex = 10;
                Cantidad.Width = 70;
                Cantidad.ColumnEdit = this.editValue;
                Cantidad.Visible = true;
                Cantidad.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Cantidad);

                //OrigenMonetario
                BandedGridColumn OrigenMonetario = new BandedGridColumn();
                OrigenMonetario.FieldName = this.unboundPrefix + "OrigenMonetario";
                OrigenMonetario.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_OrigenMonetario");
                OrigenMonetario.UnboundType = UnboundColumnType.Decimal;
                OrigenMonetario.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                OrigenMonetario.AppearanceCell.Options.UseTextOptions = true;
                OrigenMonetario.VisibleIndex = 11;
                OrigenMonetario.Width = 45;
                OrigenMonetario.ColumnEdit = this.editValue;
                OrigenMonetario.Visible = true;
                OrigenMonetario.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(OrigenMonetario);

                //ValorMLDet
                BandedGridColumn ValorMLDet = new BandedGridColumn();
                ValorMLDet.FieldName = this.unboundPrefix + "ValorMLDet";
                ValorMLDet.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorMLDet");
                ValorMLDet.UnboundType = UnboundColumnType.Decimal;
                ValorMLDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorMLDet.AppearanceCell.Options.UseTextOptions = true;
                ValorMLDet.VisibleIndex = 12;
                ValorMLDet.Width = 85;
                ValorMLDet.Visible = true;
                ValorMLDet.ColumnEdit = this.editSpin;
                ValorMLDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorMLDet);

                //ValorMEDet
                BandedGridColumn ValorMEDet = new BandedGridColumn();
                ValorMEDet.FieldName = this.unboundPrefix + "ValorMEDet";
                ValorMEDet.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorMEDet");
                ValorMEDet.UnboundType = UnboundColumnType.Decimal;
                ValorMEDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorMEDet.AppearanceCell.Options.UseTextOptions = true;
                ValorMEDet.VisibleIndex = 13;
                ValorMEDet.Width = 75;
                ValorMEDet.Visible = true;
                ValorMEDet.ColumnEdit = this.editSpin;
                ValorMEDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ValorMEDet);
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionRecibido.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected void AfterInitialize()
        {
            //this.dtFecha.DateTime = this.fechaRadicacion;
            this.fechaRadicacion = this.dtFecha.DateTime;
            this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
            //this.cmbTipoModena.Enabled = this.multiMoneda ? true : false;
            this.ValidarFechas();
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
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
            FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemCopy.Enabled = false;
                FormProvider.Master.itemPaste.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;
                FormProvider.Master.itemExport.Enabled = false;
                FormProvider.Master.itemRevert.Enabled = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
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
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
          
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida si ya fue creado el documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Factura_Leave(object sender, EventArgs e)
        {
            try
            {
                this._dtoCtrl = this.GetDocumentExt();
                if (this._dtoCtrl != null && this._listRecibidosNoFact.Count > 0)
                {
                    this._ctaXPagar = this._bc.AdministrationModel.CuentasXPagar_Get(_dtoCtrl.NumeroDoc.Value.Value);
                    if (this._ctaXPagar != null)
                    {
                        if (this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.Radicado)
                            && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.SinAprobar)
                            && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.ParaAprobacion))
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaAlreadyExist));
                        else
                        {
                            this.LoadDocumentExist();
                            this.ConvertMoneda();
                            this.gcDocument.DataSource = this._listRecibidosNoFact;
                        }
                    }
                }
                else if (this._dtoCtrl != null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Esta factura existente no tiene asignado movimientos de recibidos"));
                    this._listRecibidosNoFact = new List<DTO_prRecibidoAprob>();
                    this.gcDocument.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionRecibido.cs-txt_Factura_Leave"));
            }
        }

        /// <summary>
        /// Habilita o desabilita las causas de devolucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_TipoMovimiento_SelectedValueChanged(object sender, EventArgs e)
        {
            var item = Convert.ToInt32((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);
            if (item == (byte)EstadoDocControl.Devuelto)
            {            
                    this.FieldsEnabled(false);
            }
            else
            {
                this.FieldsEnabled(true);
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTasaCambio_Leave(object sender, EventArgs e)
        {
            try
            {
                this._tasaCambio = Convert.ToDecimal(this.txtTasaCambio.EditValue);
                this.ConvertMoneda();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionRecibido.cs-txt_Factura_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de numero de factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroRadicado_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtNroRadicado.Text))
                this.txtNroRadicado.Text = "0";
        }

        /// <summary>
        /// Modifica el valor de la fecha de vencimiento con respecto a la fecha de factura
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void dtFechaFactura_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.fechaVencimiento = this.dtFechaFactura.DateTime.AddDays(double.Parse(_nDias));
                this.dtFechaVencimiento.DateTime = this.fechaVencimiento;
                this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFechaFactura.DateTime);
                this.txtTasaCambio.EditValue = _tasaCambio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "RadicacionRecibido.cs-dtFechaFactura_EditValueChanged"));
            }
        }

        /// <summary>
        ///  Valida los datos ingresados al momento de dejar el control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            if (this.masterProveedor.ValidID)
            {
                this._proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._proveedor.TerceroID.Value, true);
                if (tercero.RadicaDirectoInd.Value.Value)
                {
                    #region Habilita los controles de valores para radicacion manual
                    this._listRecibidosNoFact = new List<DTO_prRecibidoAprob>();
                    if (this._tipoMoneda == (int)TipoMoneda.Foreign)
                        this.txtTotalExtr.Properties.ReadOnly = false;
                    else if (this._tipoMoneda == (int)TipoMoneda.Local)
                        this.txtTotalLocal.Properties.ReadOnly = false;
                    this.txtValorLocal.Enabled = false;
                    this.txtValorExtr.Enabled = false;
                    this.txtValorIVALocal.Enabled = false;
                    this.txtValorIVAExtr.Enabled = false;
                    this.txtTotalLocal.Focus();
                    #endregion
                }
                else
                {
                    this._listRecibidosNoFact = this._bc.AdministrationModel.Recibido_GetRecibidoNoFacturado(this.documentID, this._actFlujo.ID.Value, this.masterProveedor.Value, true);
                    this._listRecibidosNoFact = this._listRecibidosNoFact.FindAll(x => x.ProveedorID.Value == this._proveedor.ID.Value).ToList();
                    this.ConvertMoneda();
                    this.gcDocument.DataSource = this._listRecibidosNoFact;
                }
            }
            else
                this.RefreshDocument();
        }

        /// <summary>
        /// Cuando el valor del combo cambia
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoModena_SelectedValueChanged(object sender, EventArgs e)
        {
            this.ConvertMoneda();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroOC_EditValueChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtNroOC.EditValue.ToString()))
            {
                List<DTO_prRecibidoAprob> _listFilter = new List<DTO_prRecibidoAprob>();
                foreach (DTO_prRecibidoAprob rec in _listRecibidosNoFact)
                {
                      if(rec.Detalle.Any(x=>x.PrefDoc.Contains(this.txtNroOC.EditValue.ToString())))
                        _listFilter.Add(rec);
                }
                this._listRecibidosNoFact = _listFilter;
                this.gcDocument.DataSource = this._listRecibidosNoFact;
            }
            else
            {
                this._listRecibidosNoFact = this._bc.AdministrationModel.Recibido_GetRecibidoNoFacturado(this.documentID, this._actFlujo.ID.Value, this.masterProveedor.Value, true);
                this._listRecibidosNoFact = this._listRecibidosNoFact.FindAll(x => x.ProveedorID.Value == this._proveedor.ID.Value).ToList();
                this.ConvertMoneda();
                this.gcDocument.DataSource = this._listRecibidosNoFact;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroOC_Leave(object sender, EventArgs e)
        {

        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "SiNo" && e.Value == null)
                {
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                        }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            {
                                e.Value = fi.GetValue(dto);
                            }
                            else
                            {
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                            }
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "SiNo")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {
           // this.ValorTotalSelect();
        }

        /// <summary>
        /// Calcula los valores y hace operaciones mientras se modifican los valores del campo en la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == this.unboundPrefix + "Seleccionar")
                this.ValorTotalSelect(Convert.ToBoolean(e.Value), e.RowHandle);

        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDetalle_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "OrigenMonetario")
                {
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "Loc";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "Ext";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "gvDetalle_CustomColumnDisplayText"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBNew()
        {
            this.RefreshDocument();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            this.estado = Convert.ToByte((((ComboBoxItem)(this.cmbTipoMovimiento.SelectedItem)).Value));
            this.gvDocument.PostEditor();

            var message = this.FieldsObligated();
            if (string.IsNullOrEmpty(message))
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            else
                MessageBox.Show(message);
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
                if (this.ValidaConsistenciaDatos())
                {
                    #region Radicar
                    if (estado == (byte)EstadoDocControl.Radicado)
                    {
                        if (this._dtoCtrl == null || this._ctaXPagar == null)
                            this.Invoke(this.radicarFacturaDelegate);
                        else
                            this.Invoke(this.actualizarFacturaDelegate);
                    }
                    #endregion
                    #region Devolver
                    else if (estado == (byte)EstadoDocControl.Devuelto)
                    {
                        if (this._dtoCtrl == null)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_DocNotRadicate));
                        else
                        {
                            if (this._ctaXPagar == null)
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_DocNotRadicate));
                            else
                            {
                                if (this._dtoCtrl.Estado.Value == (byte)EstadoDocControl.Devuelto)
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaAlreadyReturned));
                                else if (this._dtoCtrl.Estado.Value != Convert.ToByte(EstadoDocControl.Devuelto)
                                            || this._dtoCtrl.Estado.Value != Convert.ToByte(EstadoDocControl.Radicado)
                                            || this._dtoCtrl.Estado.Value != Convert.ToByte(EstadoDocControl.SinAprobar)
                                            || this._dtoCtrl.Estado.Value != Convert.ToByte(EstadoDocControl.ParaAprobacion)
                                        )
                                {
                                    this.Invoke(this.actualizarFacturaDelegate);
                                }
                                else
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaAlreadyExist));
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionRecibido.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    
    }
}
