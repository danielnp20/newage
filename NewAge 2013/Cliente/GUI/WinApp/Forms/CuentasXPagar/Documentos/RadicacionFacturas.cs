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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RadicacionFacturas : DocumentForm
    {
        public RadicacionFacturas()
        {
            //InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _dtoCtrl;
        private DTO_cpCuentaXPagar _ctaXPagar;
        private List<DTO_MasterBasic> _causales;
        private string _nDias;
        private short estado;

        private string monedaID;
        private string monedaExtranjera;
        private List<int> select = new List<int>();
      
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }

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
                #region Carga el documento
                var estado = Convert.ToInt16((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);

                string tasaMon = this.monedaID;
                var tipoMoneda = Convert.ToInt32((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);
                if (tipoMoneda == (int)TipoMoneda.Local)
                    tasaMon = this.monedaExtranjera;

                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.Descripcion.Value = "Radicacion Factura"; 
                this._dtoCtrl.Estado.Value = estado;
                this._dtoCtrl.TasaCambioCONT.Value = this.multiMoneda ? _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFecha.DateTime) : 0;
                this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                this._dtoCtrl.Valor.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Iva.Value = Convert.ToDecimal(this.txtValorIVA.EditValue, CultureInfo.InvariantCulture);
                #endregion
                #region Campos variables DTO_cpCuentasXPagar
                this._ctaXPagar.ConceptoCxPID.Value = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPxDefecto); // (this.cbNotaCredito.Checked)? 
                this._ctaXPagar.Valor.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorTercero.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.IVA.Value = Convert.ToDecimal(this.txtValorIVA.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.MonedaPago.Value = tasaMon;
                this._ctaXPagar.FacturaFecha.Value = this.dtFechaFactura.DateTime;
                this._ctaXPagar.VtoFecha.Value = this.dtFechaVencimiento.DateTime;
                this._ctaXPagar.DistribuyeImpLocalInd.Value = false;
                #endregion
                int numDoc = 0;
                if (estado == (byte)EstadoDocControl.Devuelto || estado == (short)EstadoDocControl.Cerrado)
                {
                    #region Devuelve una radicacion
                    if (estado != (byte)EstadoDocControl.Devuelto)
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
                        #endregion
                    }

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);
                    
                    DTO_TxResult result = _bc.AdministrationModel.CuentasXPagar_Devolver(this.documentID, this._dtoCtrl, this._ctaXPagar, true);
                    FormProvider.Master.StopProgressBarThread(this.documentID);

                    if (result.Result == ResultValue.OK)
                    {
                        string msgOk = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NumeroRadicacion);
                        MessageBox.Show(string.Format(msgOk, result.ExtraField));

                        this.RefreshDocument();
                    }

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                    #endregion
                }
                else
                {
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);

                    object obj = _bc.AdministrationModel.CuentasXPagar_Radicar(this.documentID, this._dtoCtrl, this._ctaXPagar, true, true, out numDoc);
                    FormProvider.Master.StopProgressBarThread(this.documentID);

                    bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                    if (isOK)
                    {
                        DTO_Alarma alarma = (DTO_Alarma)obj;
                        string msgOk = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NumeroDevolucion);
                        MessageBox.Show(string.Format(msgOk, alarma.ExtraField));

                        this.RefreshDocument();

                        this._dtoCtrl = null;
                        this._ctaXPagar = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "ActualizarFacturaMethod" + ex.Message));
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
                this._dtoCtrl.DocumentoID.Value = (this.cbNotaCredito.Checked)? AppDocuments.NotaCreditoCxP: AppDocuments.CausarFacturas;
                this._dtoCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._dtoCtrl.PrefijoID.Value = this.txtPrefix.Text;
                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.Descripcion.Value = "Radicacion Factura";
                this._dtoCtrl.Valor.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Iva.Value = Convert.ToDecimal(this.txtValorIVA.EditValue, CultureInfo.InvariantCulture);

                string tasaMon = this.monedaID;
                var tipoMoneda = Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);
                if (tipoMoneda == (int)TipoMoneda.Foreign)
                    tasaMon = this.monedaExtranjera;

                this._dtoCtrl.MonedaID.Value = tasaMon;
                this._dtoCtrl.TasaCambioCONT.Value = this.multiMoneda ? _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFecha.DateTime) : 0;
                this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;

                this._dtoCtrl.TerceroID.Value = ucMasterTercero.Value;
                this._dtoCtrl.DocumentoTercero.Value = txtFactura.Text.Trim();
                this._dtoCtrl.Estado.Value = (byte)EstadoDocControl.Radicado;
                this._dtoCtrl.seUsuarioID.Value = this.userID;
                #endregion
                #region Campos variables DTO_cpCuentasXPagar
                this._ctaXPagar = new DTO_cpCuentaXPagar();
                this._ctaXPagar.ConceptoCxPID.Value = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPxDefecto);
                this._ctaXPagar.Valor.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.ValorTercero.Value = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.IVA.Value = Convert.ToDecimal(this.txtValorIVA.EditValue, CultureInfo.InvariantCulture);
                this._ctaXPagar.MonedaPago.Value = tasaMon;
                this._ctaXPagar.FacturaFecha.Value = this.dtFechaFactura.DateTime;
                this._ctaXPagar.VtoFecha.Value = this.dtFechaVencimiento.DateTime;
                this._ctaXPagar.DistribuyeImpLocalInd.Value = false;
                this._ctaXPagar.RadicaCodigo.Value = txtNroRadicado.Text.Trim();
                #endregion
                int numDoc = 0;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.CuentasXPagar_Radicar(this.documentID, this._dtoCtrl, this._ctaXPagar, true, false, out numDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    DTO_Alarma alarma = (DTO_Alarma)obj;
                    string msgOk = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NumeroRadicacion);
                    MessageBox.Show(string.Format(msgOk, alarma.ExtraField));
                   
                    this.RefreshDocument();
                    this._bc.AdministrationModel.ControlList = this._bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, this._bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();
                }

                this._dtoCtrl = null;
                this._ctaXPagar = null;
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "RadicarFacturaMethod" + ex.Message));
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
            _bc.InitMasterUC(this.ucMasterTercero, AppMasters.coTercero, true, true, true, true);
          
            this.dtFecha.Enabled = false;
            this.btnGuardarCausa.Enabled = false;
            this.txtOtraCausa.Enabled = false;

            this.cmbTipoModena.SelectedIndex = 0;
            this.txtValorFactura.EditValue = "0";
            this.txtValorIVA.EditValue = "0";
        }

        /// <summary>
        /// Devuelve el documento control asociado al tercero 
        /// </summary>
        /// <returns></returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = this.ucMasterTercero.Value;
                string fact = this.txtFactura.Text.Trim();
                int docID = (this.cbNotaCredito.Checked) ? AppDocuments.NotaCreditoCxP : AppDocuments.CausarFacturas;
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(docID, tercero, fact);

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
            this.ucMasterTercero.Value = this._dtoCtrl.TerceroID.Value;
            this.txtFactura.Text = this._dtoCtrl.DocumentoTercero.Value;
            this.cmbTipoMovimiento.SelectedValue = this._dtoCtrl.Estado.Value;
            this.txtValorFactura.EditValue = this._ctaXPagar.Valor.Value;
            this.txtValorIVA.EditValue = this._ctaXPagar.IVA.Value;
            this.txtValorTotal.EditValue = (this._ctaXPagar.Valor.Value + this._ctaXPagar.IVA.Value);
            this.txtDescripcion.Text = this._dtoCtrl.Observacion.Value;
            this.txtNroRadicado.Text = this._ctaXPagar.RadicaCodigo.Value;
            this.cbNotaCredito.Enabled = false;

            this.AsignarFechas();
            DateTime fechaVto = this._ctaXPagar.VtoFecha.Value.Value;
            this.dtFecha.DateTime = this._ctaXPagar.FacturaFecha.Value.Value;
            this.dtFechaFactura.DateTime = this._ctaXPagar.FacturaFecha.Value.Value;
            this.dtFechaVencimiento.DateTime = fechaVto;

            if (this.dtPeriod.DateTime.Month != this.dtFecha.DateTime.Month)
                this.chkProvisiones.Checked = true;
            else
                this.chkProvisiones.Checked = false;
        }

        /// <summary>
        /// Calcula valor factura + iva
        /// </summary>
        private void CalcularTotal()
        {
            decimal valor = 0;
            decimal iva = 0;

            if (!string.IsNullOrEmpty(this.txtValorFactura.Text))
                valor = Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(this.txtValorIVA.Text))
                iva = Convert.ToDecimal(this.txtValorIVA.EditValue, CultureInfo.InvariantCulture);

            this.txtValorTotal.EditValue = (valor + iva);

        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.dtFechaFactura.Enabled = estado;
            this.dtFechaVencimiento.Enabled = estado;
            this.cmbTipoModena.Enabled = this.multiMoneda && estado ? true : false;
            //this.ucMasterTercero.Enabled = estado;
            //this.txtFactura.Enabled = estado;
            this.txtValorFactura.Enabled = estado;
            this.txtValorIVA.Enabled = estado;
            this.txtDescripcion.Enabled = estado;
            this.btnGuardarCausa.Enabled = !estado;
            this.txtOtraCausa.Enabled = !estado;
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
            field = string.IsNullOrEmpty(ucMasterTercero.Value) ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coTercero + "_lblTitle") : field;
            field = string.IsNullOrEmpty(txtFactura.Text) ? field = field + "\n" + this.lblFactura.Text : field;
            field = string.IsNullOrEmpty(txtValorFactura.Text) ? field = field + "\n" + this.lblValorFactura.Text : field;
            field = string.IsNullOrEmpty(txtValorIVA.Text) ? field = field + "\n" + this.lblValorIVA.Text : field;

            try
            {
                Convert.ToInt32((this.cmbTipoModena.SelectedItem as ComboBoxItem).Value);
            }
            catch (Exception ex)
            {
                field += "\n" + this.lblTipoMoneda.Text;
            }

            if (!string.IsNullOrEmpty(field))
            {
                message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaFieldObligated);
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
            if (Convert.ToDecimal(this.txtValorFactura.EditValue, CultureInfo.InvariantCulture) == 0)
            {
                message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValueDocumentInvalid);
                return message;
            }
            return message;
            #endregion

            return message;
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.chkProvisiones.Checked = false;
            this.txtNroRadicado.Text = string.Empty;
            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;

            this.ucMasterTercero.Value = string.Empty;
            this.txtFactura.Text = string.Empty;
            this.txtValorFactura.EditValue = 0;
            this.txtValorIVA.EditValue = 0;
            this.txtValorTotal.EditValue = 0;
            this.txtDescripcion.Text = string.Empty;
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            this.cmbTipoModena.SelectedIndex = 0;
            this.cmbTipoMovimiento.SelectedValue = (byte)TipoMovimientoCXP.Radicado;
            this.cbNotaCredito.Checked = false;
            this.cbNotaCredito.CheckState = CheckState.Unchecked;
            this.cbNotaCredito.Enabled = true; ;


            FormProvider.Master.itemSendtoAppr.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;

            this.txtNroRadicado.Focus();
            this.LoadData(true);
        }

        /// <summary>
        /// Asigna las fechas
        /// </summary>
        private void AsignarFechas()
        {
            try
            {
                this.dtFechaFactura.EditValueChanged -= new System.EventHandler(this.dtFechaFactura_EditValueChanged);
                if (this.chkProvisiones.Checked)
                {
                    DateTime provisionDate = this.dtPeriod.DateTime.AddMonths(-1);
                    int provisionMonth = provisionDate.Month;
                    int provisionYear = provisionDate.Year;
                    int provisionLastDay = DateTime.DaysInMonth(provisionYear, provisionMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(provisionYear, provisionMonth, 1);
                    this.dtFecha.Properties.MaxValue = new DateTime(provisionYear, provisionMonth, provisionLastDay);
                    this.dtFecha.DateTime = new DateTime(provisionYear, provisionMonth, 1);

                    this.dtFechaFactura.Properties.MinValue = new DateTime(provisionYear, provisionMonth, 1);
                    this.dtFechaFactura.Properties.MaxValue = new DateTime(provisionYear, provisionMonth, provisionLastDay);
                    this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
                }
                else
                {
                    int currentMonth = this.dtPeriod.DateTime.Month;
                    int currentYear = this.dtPeriod.DateTime.Year;
                    int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, 1);
                    this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, 1);

                    this.dtFechaFactura.Properties.MinValue = new DateTime(currentYear, currentMonth, 1);
                    this.dtFechaFactura.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFechaFactura.DateTime = this.dtFecha.DateTime;
                }

                this.dtFechaFactura.EditValueChanged += new System.EventHandler(this.dtFechaFactura_EditValueChanged);

                this.dtFechaVencimiento.Properties.MinValue = this.dtFecha.DateTime;
                this.dtFechaVencimiento.DateTime = this.dtFecha.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "AsignarFechas"));
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.cpCausalesDev, null, null, true);
                _causales = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.cpCausalesDev, count, 1, null, null, true).ToList();
            }

        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            return 0;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.RadicacionFactura;

            base.SetInitParameters();
            
            this.actualizarFacturaDelegate = new ActualizarFactura(this.ActualizarFacturaMethod);
            this.radicarFacturaDelegate = new RadicarFactura(this.RadicarFacturaMethod);

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.cp;
            this.LoadData(true);

            this._nDias = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_DiasPlazoPagoFact);
            
            //Carga info de las monedas
            this.monedaID = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            this.InitControls();
            this.AddGridCols();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Causas Devolucion

                //TareaID
                GridColumn tareaId = new GridColumn();
                tareaId.FieldName = this.unboundPrefix + "Descriptivo";
                tareaId.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                tareaId.UnboundType = UnboundColumnType.String;
                tareaId.VisibleIndex = 0;
                tareaId.Width = 600;
                tareaId.Visible = true;
                tareaId.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(tareaId);

                //Tarea
                GridColumn tarea = new GridColumn();
                tarea.FieldName = this.unboundPrefix + "SiNo";
                tarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SiNo");
                tarea.UnboundType = UnboundColumnType.String;
                tarea.VisibleIndex = 1;
                tarea.Width = 100;
                tarea.Visible = true;
                tarea.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(tarea);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.cmbTipoModena.Enabled = this.multiMoneda ? true : false;
            this.cbNotaCredito.Checked = false;
            this.cbNotaCredito.CheckState = CheckState.Unchecked;

            //Asigna las fechas
            this.AsignarFechas();
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
            base.Form_Enter(sender, e);

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

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        #endregion

        #region Eventos Header
        private void ucMasterTercero_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.ucMasterTercero.ValidID)
                {
                    DTO_coTercero ter = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.ucMasterTercero.Value, true);
                    DTO_coRegimenFiscal reg = (DTO_coRegimenFiscal)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coRegimenFiscal, false, ter.ReferenciaID.Value, true);
                    string factEquivalente = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoFacturaEquivalente);
                    if(reg != null && reg.FactEquivalenteInd.Value.Value)
                        this.txtFactura.Text = factEquivalente;
                    else
                        this.txtFactura.Text  = string.Empty;
                }
                else
                    this.txtFactura.Text = string.Empty; 
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "ucMasterTercero_Leave"));
            }
        }

        /// <summary>
        /// Valida si ya fue creado el documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Factura_Leave(object sender, EventArgs e)
        {
            this._dtoCtrl = this.GetDocumentExt();
            if (this._dtoCtrl != null)
            {
                this._ctaXPagar = _bc.AdministrationModel.CuentasXPagar_Get(_dtoCtrl.NumeroDoc.Value.Value);
                if (this._ctaXPagar != null)
                {
                    if (this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.Radicado)
                        && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.SinAprobar)
                        && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.ParaAprobacion)
                        )
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaAlreadyExist));
                    }
                    else
                        this.LoadDocumentExist();
                }
            }
            else
                this.cbNotaCredito.Enabled = true;
        }

        /// <summary>
        /// Habilita o desabilita las causas de devolucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_TipoMovimiento_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var item = Convert.ToInt16((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);
                if (item == (short)EstadoDocControl.Devuelto || item == (short)EstadoDocControl.Cerrado)
                {
                    if (this.cbNotaCredito.Checked)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoDevNotaCredito));
                        this.cbNotaCredito.Checked = false;
                        this.cbNotaCredito.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        this.gcDocument.DataSource = _causales;
                        FieldsEnabled(false);
                    }
                }
                else
                {
                    FieldsEnabled(true);
                    this.gcDocument.DataSource = null;
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "cmb_TipoMovimiento_SelectedValueChanged"));
            }
        }

        /// <summary>
        /// Calcula valor total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_ValorFactura_Leave(object sender, EventArgs e)
        {
            this.CalcularTotal();
        }

        /// <summary>
        /// Calcula valor total
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_ValorIVA_Leave(object sender, EventArgs e)
        {
            this.CalcularTotal();
        }

        /// <summary>
        /// Valida los q los campos sean numericos en el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_ValorFactura_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
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
            this.dtFecha.DateTime = this.dtFechaFactura.DateTime;
            this.dtFechaVencimiento.Properties.MinValue = this.dtFecha.DateTime;
            this.dtFechaVencimiento.DateTime = this.dtFechaFactura.DateTime.AddDays(double.Parse(this._nDias));
        }

        /// <summary>
        /// Tipo del documento (true - NotaCredito; false - Factura)
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void cbNotaCredito_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox item = (CheckBox)sender;
                int estado =  Convert.ToInt16((this.cmbTipoMovimiento.SelectedItem as ComboBoxItem).Value);
                if (item.Checked && estado == (byte)EstadoDocControl.Devuelto || item.Checked && estado == (short)EstadoDocControl.Cerrado)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_NoReturnNC));

                    this.cbNotaCredito.Checked = false;
                    this.cbNotaCredito.CheckState = CheckState.Unchecked;
                }
                else if (this.ucMasterTercero.ValidID && !string.IsNullOrWhiteSpace(this.txtFactura.Text))
                {
                    this._dtoCtrl = this.GetDocumentExt();
                    if (this._dtoCtrl != null)
                    {
                        this._ctaXPagar = _bc.AdministrationModel.CuentasXPagar_Get(_dtoCtrl.NumeroDoc.Value.Value);
                        if (this._ctaXPagar != null)
                        {
                            if (this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.Radicado)
                                && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.SinAprobar)
                                && this._dtoCtrl.Estado.Value.Value != Convert.ToByte(EstadoDocControl.ParaAprobacion)
                                )
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaAlreadyExist));
                            }
                            else
                                this.LoadDocumentExist();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "cbNotaCredito_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la seleccion de provisiones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkProvisiones_CheckedChanged(object sender, EventArgs e)
        {
            this.AsignarFechas();
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "SiNo")
            {
                e.RepositoryItem = this.editChkBox;
            }
        }

        /// <summary>
        /// Adiciona un nuevo registro a la maestra de cpCausaDevolucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarCausa_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOtraCausa.Text))
            {
                DTO_MasterBasic dto = new DTO_MasterBasic();
                dto.Descriptivo.Value = txtOtraCausa.Text;
                _causales.Add(dto);
                this.txtOtraCausa.Text = string.Empty;                
                gcDocument.DataSource = _causales;
                gcDocument.RefreshDataSource();
        
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaMustAddRecord));
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
            this.estado = Convert.ToInt16((((ComboBoxItem)(this.cmbTipoMovimiento.SelectedItem)).Value));
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
        public override void SaveThread()
        {
            try
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
                #region Devolver o Cerrar
                else if (estado == (byte)EstadoDocControl.Devuelto || estado == (short)EstadoDocControl.Cerrado)
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
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RadicacionFacturas.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

       

    }
}
