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
using System.Threading;
using DevExpress.XtraGrid.Columns;
using SentenceTransformer;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Transaccion Manual
    /// </summary>
    public partial class NotaEnvio : DocumentMvtoForm
    {
        public NotaEnvio()
        {
           //this.InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.data.Footer;
            this.newDoc = true;
            this.CleanHeader(true);
            this.EnableHeader(0, true);
            this.EnableFooter(false);
            this.CleanFooter(true);
        }

        #endregion

        #region Variables privadas
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private TipoTransporte _tipoTransporte;
        DTO_inBodegaTipo tipoBodegaOrig = null;
        //private List<DTO_inMovimientoFooter> _mvtoFooter = null;
        private bool _clienteInd = false;
        private bool _txtNumberFocus;
        private int numeroDocFact = 0;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.NotaEnvio;
            InitializeComponent();

            this.frmModule = ModulesPrefix.@in;
            base.SetInitParameters();

            this.data = new DTO_MvtoInventarios();
            this._ctrl = new DTO_glDocumentoControl();
            this._ctrl.DocumentoID.Value = this.documentID;

            //Controles del header
            this._bc.InitMasterUC(this.masterMvtoTipoInv, AppMasters.inMovimientoTipo, true, true, true, true);
            this._bc.InitMasterUC(this.masterBodegaOrigen, AppMasters.inBodega, false, true, true, true);
            this._bc.InitMasterUC(this.masterBodegaDestino, AppMasters.inBodega, false, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.coTercero, false, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, false);
            this._bc.InitMasterUC(this.masterTransporProveedor, AppMasters.prProveedor, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
            this.masterBodegaDestino.EnableControl(false);
            this.masterCliente.EnableControl(false);
            this.masterTransporProveedor.EnableControl(false);
            this.masterProyecto.EnableControl(false);
            this.masterCentroCto.EnableControl(false);  
            
            TablesResources.GetTableResources(this.cmbTipoTransporte, typeof(TipoTransporte));
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            //Ajuste el tamaño de las secciones
            base.tlSeparatorPanel.RowStyles[0].Height = 145;
            base.tlSeparatorPanel.RowStyles[1].Height = 225;
            base.tlSeparatorPanel.RowStyles[2].Height = 180;

            this.EnableFooter(false);
            this.cmbTipoTransporte.SelectedItem = this.cmbTipoTransporte.GetItem(((byte)TipoTransporte.Terrestre).ToString());
            this._tipoTransporte = (TipoTransporte)Enum.Parse(typeof(TipoTransporte), (this.cmbTipoTransporte.SelectedItem as ComboBoxItem).Value);
            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
                this.txtTasaCambio.Visible = false;
            this.AsignarTasaCambio();
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = this._bc.GetControlValueByCompany(this.frmModule, AppControl.in_Periodo);
                this.dtPeriod.Text = periodo;
                this.masterMvtoTipoInv.Value = string.Empty;
                this.txtDatoAdd1.Text = string.Empty;
            }
            this.AsignarTasaCambio();
            this.masterBodegaOrigen.Value = string.Empty;
            this.masterBodegaDestino.Value = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.txtNumber.Text = string.Empty;
            this.masterTransporProveedor.Value = string.Empty;
            this.txtDatoAdd6.Text = string.Empty;
            this.cmbTipoTransporte.SelectedItem = this.cmbTipoTransporte.GetItem(((byte)TipoTransporte.Terrestre).ToString());
            this.txtDatoAdd5.Text = string.Empty;
            this.txtDatoAdd2.Text = string.Empty;
            this.txtDatoAdd4.Text = string.Empty;
            this.txtDatoAdd3.Text = string.Empty;
            this.txtDatoAdd7.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.masterCentroCto.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            base.CleanHeader(basic);
            this.masterMvtoTipoInv.Focus();
            this.btnFacturaCompra.Visible = false;
            this.btnFacturaVenta.Visible = false;
            this._bc.InitMasterUC(this.masterTransporProveedor, AppMasters.prProveedor, true, true, true, false);
            this.txtDocExternoProv.BackColor = Color.White;
        }

        /// <summary>
        /// Deshabilita los controles del header segun condicion
        /// </summary>
        protected override void EnableHeader(short TipoMov, bool basic)
        {
            this.grpboxHeader.Enabled = true;
            this.masterMvtoTipoInv.EnableControl(true);
            this.masterBodegaOrigen.EnableControl(true);
            this.txtDatoAdd1.Enabled = true;
            this.txtNumber.Enabled = true;
            this.btnQueryDoc.Enabled = true;
            if (basic)
            {
                #region Habilita para Salidas
                if (TipoMov == (byte)TipoMovimientoInv.Salidas)
                {
                    this.masterProyecto.EnableControl(true);
                    this.masterCentroCto.EnableControl(true);
                    this.masterBodegaDestino.EnableControl(false);
                    if (mvtoTipo.TerceroInd.Value.Value)
                    {
                        this._clienteInd = true;
                        this.masterCliente.EnableControl(true);
                    }
                    else
                    {
                        this._clienteInd = false;
                        this.masterCliente.EnableControl(false);
                    }
                }
                #endregion
                #region Habilita para Traslados
                else if (TipoMov == (byte)TipoMovimientoInv.Traslados)
                {
                    this.masterBodegaDestino.EnableControl(true);
                    this.masterProyecto.EnableControl(false);
                    this.masterCentroCto.EnableControl(false);
                    if (mvtoTipo.TerceroInd.Value.Value)
                    {
                        this._clienteInd = true;
                        this.masterCliente.EnableControl(true);  
                    }
                    else
                    {
                        this._clienteInd = false;
                        this.masterCliente.EnableControl(false);                     
                    }
                    if (this.tipoBodegaOrig != null && this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.PuertoFOB)
                    {
                        this.masterTransporProveedor.EnableControl(true);
                        this.txtDocExternoProv.Enabled = true;
                    }
                    else
                    {
                        this.masterTransporProveedor.EnableControl(false);
                        this.txtDocExternoProv.Enabled = false;
                    }
                }
                this.txtDatoAdd3.Enabled = true;
                this.txtDatoAdd4.Enabled = true;
                this.txtDatoAdd6.Enabled = true;
                this.cmbTipoTransporte.Enabled = true;
                this.txtDatoAdd5.Enabled = true;
                this.txtDatoAdd7.Enabled = true;
                this.txtObservacion.Enabled = true;
                #endregion            
            }
            else
            {
                if (TipoMov != 0)
                {
                    this.masterMvtoTipoInv.EnableControl(false);
                    this.masterBodegaOrigen.EnableControl(false);
                    this.txtDatoAdd1.Enabled = false;
                    this.txtNumber.Enabled = false;
                    this.btnQueryDoc.Enabled = false;
                }
                this.masterProyecto.EnableControl(false);
                this.masterCentroCto.EnableControl(false);
                this.masterBodegaDestino.EnableControl(false);
                this.masterCliente.EnableControl(false);
                if (this.tipoBodegaOrig != null && this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.PuertoFOB)
                {
                    this.masterTransporProveedor.EnableControl(false);
                    this.txtDocExternoProv.Enabled = false;
                }
                this.txtDatoAdd2.Enabled = false;
                this.txtDatoAdd3.Enabled = false;
                this.txtDatoAdd4.Enabled = false;
                this.txtDatoAdd6.Enabled = false;
                this.cmbTipoTransporte.Enabled = false;
                this.txtDatoAdd5.Enabled = false;
                this.txtDatoAdd7.Enabled = false;
                this.txtObservacion.Enabled = false;
            }
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected Object LoadTempHeader()
        {
            try
            {
                DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
                header.MvtoTipoInvID.Value = this.masterMvtoTipoInv.Value;
                header.BodegaOrigenID.Value = this.masterBodegaOrigen.Value;
                header.BodegaDestinoID.Value = this.masterBodegaDestino.Value;
                header.EmpresaID.Value = this.empresaID;
                header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                header.TipoTransporte.Value = Convert.ToByte((this.cmbTipoTransporte.SelectedItem as ComboBoxItem).Value);
                header.ClienteID.Value = this.masterCliente.Value;
                header.DatoAdd1.Value = this.txtDatoAdd2.Text;
                header.DatoAdd2.Value = this.txtDatoAdd3.Text;
                header.DatoAdd3.Value = this.txtDatoAdd4.Text;
                header.DatoAdd4.Value = this.txtDatoAdd5.Text;
                header.DatoAdd5.Value = this.txtDatoAdd6.Text;
                header.DatoAdd6.Value = this.txtDatoAdd7.Text;
                header.Observacion.Value = this.txtObservacion.Text;

                this.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaOrigen.Value, true);
                this.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaDestino.Value, true);

                this._ctrl.NumeroDoc.Value = this.data.DocCtrl.NumeroDoc.Value != null ? this.data.DocCtrl.NumeroDoc.Value : 0;
                this._ctrl.MonedaID.Value = this.monedaId;// this.masterMoneda.Value;
                this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
                this._ctrl.CentroCostoID.Value = this.masterCentroCto.Value;
                this._ctrl.PrefijoID.Value = this.bodegaOrigen != null ? this.bodegaOrigen.PrefijoID.Value : this.txtPrefix.Text;
                this._ctrl.Fecha.Value = DateTime.Now;              
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.seUsuarioID.Value = this.userID;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.ConsSaldo.Value = 0;
                this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this._ctrl.Descripcion.Value = base.txtDocDesc.Text;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                if (this.masterTransporProveedor.ValidID)
                {
                    DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterTransporProveedor.Value, true);
                    this._ctrl.TerceroID.Value = proveedor.TerceroID.Value;
                }
                this._ctrl.DocumentoTercero.Value = this.txtDocExternoProv.Text;

                DTO_MvtoInventarios mvto = new DTO_MvtoInventarios();
                mvto.Header = header;
                mvto.DocCtrl = this._ctrl;
                if(this.data != null && this.data.Footer.Count > 0)
                    mvto.Footer = this.data.Footer;
                else
                    mvto.Footer = new List<DTO_inMovimientoFooter>();

                return mvto;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "LoadTempHeader"));
                return null;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio()
        {
            //if (this.monedaLocal == this.masterMoneda.Value)
            this.monedaId = this.monedaLocal;
            //else
            //    this.monedaId = this.monedaExtranjera;

            //Si la empresa no permite multimoneda
            if (!this.multiMoneda)
                this.txtTasaCambio.EditValue = 0;
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio((byte)TipoMoneda.Foreign, dtFecha.DateTime);
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (tc == 0)
                {
                    this.validHeader = false;
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_NoTasaCambio));
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterMvtoTipoInv.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMvtoTipoInv.CodeRsx);

                MessageBox.Show(msg);
                this.masterMvtoTipoInv.Focus();
                result = false;
            }
            if (!this.masterBodegaOrigen.ValidID && !base.isFacturaVenta)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterBodegaOrigen.CodeRsx);

                MessageBox.Show(msg);
                this.masterBodegaOrigen.Focus();
                result = false;
            }
            if (!this.masterBodegaDestino.ValidID && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterBodegaDestino.CodeRsx);

                MessageBox.Show(msg);
                this.masterBodegaDestino.Focus();
                result = false;
            }
            if (!this.masterCliente.ValidID && _clienteInd)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.CodeRsx);

                MessageBox.Show(msg);
                this.masterCliente.Focus();
                result = false;
            }
            if (string.IsNullOrEmpty(this.txtDatoAdd3.Text))
            {
                //string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDatoAdd3.Text);

                //MessageBox.Show(msg);
                //this.txtDatoAdd3.Focus();
                //result = false;
            }
            if (string.IsNullOrEmpty(this.txtDatoAdd4.Text))
            {
                //string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDatoAdd4.Text);

                //MessageBox.Show(msg);
                //this.txtDatoAdd4.Focus();
                //result = false;
            }
            if (string.IsNullOrEmpty(this.txtDatoAdd5.Text))
            {
                //string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDatoAdd5.Text);

                //MessageBox.Show(msg);
                //this.txtDatoAdd5.Focus();
                //result = false;
            }
            if (string.IsNullOrEmpty(this.txtDatoAdd6.Text))
            {
                //string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDatoAdd6.Text);

                //MessageBox.Show(msg);
                //this.txtDatoAdd6.Focus();
                //result = false;
            }
            if (string.IsNullOrEmpty(this.txtDatoAdd7.Text))
            {
                //string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDatoAdd7.Text);

                //MessageBox.Show(msg);
                //this.txtDatoAdd7.Focus();
                //result = false;
            }
            if (string.IsNullOrEmpty(this.txtObservacion.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblObservacion.Text);

                MessageBox.Show(msg);
                this.txtObservacion.Focus();
                result = false;
            }
            if (!this.masterProyecto.ValidID && !base.isFacturaVenta)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();
                result = false;
            }
            if (!this.masterCentroCto.ValidID && !base.isFacturaVenta)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroCto.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCto.Focus();
                result = false;
            }
            if (!this.masterTransporProveedor.ValidID && this.btnFacturaCompra.Visible)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblprProveedor.Text);

                MessageBox.Show(msg);
                this.masterTransporProveedor.Focus();
                result = false;
            }
            if (string.IsNullOrEmpty(this.txtDocExternoProv.Text) && this.btnFacturaCompra.Visible)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblDocExterno.Text);

                MessageBox.Show(msg);
                this.txtDocExternoProv.Focus();
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(DTO_MvtoInventarios aux)
        {
            try
            {
                DTO_MvtoInventarios saldoCosto = (DTO_MvtoInventarios)aux;
                DTO_inMovimientoDocu header = saldoCosto.Header;
                DTO_glDocumentoControl ctrl = saldoCosto.DocCtrl;
                if (saldoCosto.Footer == null)
                    saldoCosto.Footer = new List<DTO_inMovimientoFooter>();

                this.masterMvtoTipoInv.Value = header.MvtoTipoInvID.Value;
                this.txtDatoAdd1.Text = ctrl.DocumentoNro.Value != null ? ctrl.DocumentoNro.Value.ToString() : "0";
                this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
                this.dtFecha.DateTime = ctrl.FechaDoc.Value.Value;
                this.masterMvtoTipoInv.Value = header.MvtoTipoInvID.Value;
                this.masterBodegaOrigen.Value = header.BodegaOrigenID.Value;
                this.masterBodegaDestino.Value = header.BodegaDestinoID.Value;
                this.masterCliente.Value = header.ClienteID.Value;
                //this.masterMoneda.Value = ctrl.MonedaID.Value;
                this.masterProyecto.Value = ctrl.ProyectoID.Value;
                this.masterCentroCto.Value = ctrl.CentroCostoID.Value;
                this.txtDatoAdd2.Text = header.DatoAdd1.Value;
                this.txtDatoAdd3.Text = header.DatoAdd2.Value;
                this.txtDatoAdd4.Text = header.DatoAdd3.Value;
                this.txtDatoAdd5.Text = header.DatoAdd4.Value;
                this.txtDatoAdd6.Text = header.DatoAdd5.Value;
                this.cmbTipoTransporte.SelectedItem = header.TipoTransporte.Value;
                this.txtDatoAdd7.Text = header.DatoAdd6.Value;
                this.txtObservacion.Text = header.Observacion.Value;
                base.proyectoHeader = ctrl.ProyectoID.Value;
                base.centroCostoHeader = ctrl.CentroCostoID.Value;
                base._documentoNro = ctrl.DocumentoNro.Value != null ? ctrl.DocumentoNro.Value.Value : 0;
                base.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, header.BodegaOrigenID.Value, true);
                base.costeoGrupoOri = !string.IsNullOrEmpty(header.BodegaOrigenID.Value) ? (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, base.bodegaOrigen.CosteoGrupoInvID.Value, true) : null;
                base.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, header.BodegaDestinoID.Value, true);
                base.costeoGrupoDest = !string.IsNullOrEmpty(header.BodegaDestinoID.Value) ? (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, base.bodegaDestino.CosteoGrupoInvID.Value, true) : null;
                base.mvtoTipo = (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, this.masterMvtoTipoInv.Value, true);
                this._clienteInd = this.mvtoTipo.TerceroInd.Value.Value;
                base.tipoMovActual = (TipoMovimientoInv)Enum.Parse(typeof(TipoMovimientoInv), mvtoTipo.TipoMovimiento.Value.ToString());

                if (this.ValidateHeader() && this.AsignarTasaCambio())
                {
                    this.data = saldoCosto;
                    this._ctrl = this.data.DocCtrl;
                    this.EnableHeader(-1, false);
                    foreach (var footer in this.data.Footer)
                    {
                        DTO_inReferencia referencia = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, footer.Movimiento.inReferenciaID.Value, true);
                        footer.UnidadRef.Value = referencia.UnidadInvID.Value;
                        base._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                    }
                    base.txtTotalCantidad.EditValue = base._totalUnidades;
                    this.LoadData(true);
                    if (this.data.Footer.Count > 0)
                    {
                        this.EnableFooter(isFacturaVenta ? false : true);
                        this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                    }
                    this.validHeader = true;
                    this.gcDocument.Focus();
                }
                else
                    this.CleanHeader(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "LoadTempData"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario(DTO_glDocumentoControl ctrl)
        {
            bool tipoMovInd = this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados ? true : false;
            DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, ctrl.NumeroDoc.Value.Value, tipoMovInd);
            if (!string.IsNullOrEmpty(this.txtNumber.Text)  && saldoCostos.Header.BodegaOrigenID.Value != this.masterBodegaOrigen.Value)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberxBodega));
                this.masterBodegaOrigen.Focus();
                return;
            }
            if (this._copyData)
            {
                saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                saldoCostos.Header.NumeroDoc.Value = 0;
                this._copyData = false;  
            }
            this.masterBodegaOrigen.Value = saldoCostos.Header.BodegaOrigenID.Value;
            this.masterBodegaDestino.Value = saldoCostos.Header.BodegaDestinoID.Value;
            base.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaOrigen.Value, true);
            base.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaDestino.Value, true);
            base.costeoGrupoOri = (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, this.bodegaOrigen.CosteoGrupoInvID.Value, true);
            base.costeoGrupoDest = this.bodegaDestino != null? (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, base.bodegaDestino.CosteoGrupoInvID.Value, true) : null;
            
            this.masterCliente.Value = saldoCostos.Header.ClienteID.Value;
            this.txtDatoAdd2.Text = saldoCostos.Header.DatoAdd1.Value;
            this.txtDatoAdd3.Text = saldoCostos.Header.DatoAdd2.Value;
            this.txtDatoAdd4.Text = saldoCostos.Header.DatoAdd3.Value;
            this.txtDatoAdd5.Text = saldoCostos.Header.DatoAdd4.Value;
            this.txtDatoAdd6.Text = saldoCostos.Header.DatoAdd5.Value;
            this.cmbTipoTransporte.SelectedItem = saldoCostos.Header.TipoTransporte.Value;
            this.txtDatoAdd7.Text = saldoCostos.Header.DatoAdd6.Value;
            this.txtObservacion.Text = saldoCostos.Header.Observacion.Value;
            this.dtFecha.DateTime = saldoCostos.DocCtrl.FechaDoc.Value.Value;
            this.masterProyecto.Value = ctrl.ProyectoID.Value;
            this.masterCentroCto.Value = ctrl.CentroCostoID.Value;
            base._documentoNro = ctrl.DocumentoNro.Value.Value;
            if (this.AsignarTasaCambio())
            {
                this.data = saldoCostos;
                this._totalUnidades = 0;
                if (this.data.Footer.Count > 0)
                    this.EnableFooter(isFacturaVenta ? false : true);
                foreach (var footer in this.data.Footer)
                {
                    DTO_inReferencia referencia = (DTO_inReferencia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, false, footer.Movimiento.inReferenciaID.Value, true);
                    footer.UnidadRef.Value = referencia.UnidadInvID.Value;
                    this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
                }
                this.txtTotalCantidad.EditValue = this._totalUnidades;
                this.UpdateTemp(this.data);
                this.LoadData(true);
                this.validHeader = true;
                this.ValidHeaderTB();
                this.newDoc = false;
            }
            else
                this.validHeader = false;                                       
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Leave(object sender, EventArgs e)
        {
            if (this._txtNumberFocus)
            {
                this._txtNumberFocus = false;

                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";

                if (this.txtNumber.Text == "0")
                {
                    #region Nueva transaccion
                    this.gcDocument.DataSource = null;
                    if (!this.multiMoneda)
                    {
                        this.newDoc = true;
                        this.validHeader = true;
                    }
                    #endregion
                }
                else
                {
                    #region Transaccion existente
                    try
                    {
                        //Verifica si la bodega ya inicio un consecutivo
                        DTO_glDocumentoControl docCtrlExist = null;
                        docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this.txtPrefix.Text, Convert.ToInt32(this.txtNumber.Text));
                        this.GetMvtoInventario(docCtrlExist);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "txtNumber_Leave: " + ex.Message));
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Enter(object sender, EventArgs e)
        {
            bool res = this.masterBodegaOrigen.ValidID;
            if (res)
            {
                this._txtNumberFocus = true;

                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de maestra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterHeader_Leave(object sender, EventArgs e)
        {
            try
            {
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
                DTO_seUsuarioBodega userBodega = null;
                int index = this.NumFila;
                switch (master.ColId)
                {
                    case "MvtoTipoInvID":
                        #region Tipo Movimiento
                        if (master.ValidID)
                        {
                            #region Valida si el Mvto es de Facturas de Venta
                            string tipoMovVentasLocales = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovVentasLoc);
                            if (tipoMovVentasLocales.Equals(master.Value))
                            {
                                this.masterBodegaOrigen.EnableControl(false);
                                this.masterBodegaDestino.EnableControl(false);
                                this.masterCliente.EnableControl(true);
                                this.txtDatoAdd1.Enabled = false;
                                this.btnFacturaVenta.Visible = true;
                                this.masterCliente.Focus();
                            }
                            else
                                this.btnFacturaVenta.Visible = false; 
                            #endregion

                            #region Valida controles segun el tipo de Mvto
                            this.mvtoTipo = (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, master.Value, true);
                            if (mvtoTipo != null)
                            {
                                if (!tipoMovVentasLocales.Equals(master.Value))
                                    this.EnableHeader(this.mvtoTipo.TipoMovimiento.Value.Value, true);
                                this.data.Header.MvtoTipoInvID.Value = master.Value;
                                base.tipoMovActual = (TipoMovimientoInv)Enum.Parse(typeof(TipoMovimientoInv), this.mvtoTipo.TipoMovimiento.Value.ToString());
                                if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                                {
                                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidMovimientoIn));
                                    this.masterMvtoTipoInv.Value = string.Empty;
                                    this.masterMvtoTipoInv.Focus();
                                }
                                this.txtDatoAdd1.ReadOnly = true;
                            } 
                            #endregion
                        }
                        #endregion
                        break;
                    case "BodegaID":
                        #region Bodegas
                        if (master.ValidID)
                        {
                            #region Valida permisos del Usuario en Bodegas
                            Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                            keysUserBodega.Add("seUsuarioID", this.userID.ToString());
                            keysUserBodega.Add("BodegaID", this.masterBodegaOrigen.Value);
                            userBodega = (DTO_seUsuarioBodega)this._bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                            if (userBodega != null && this.mvtoTipo != null)
                            {
                                #region Bodega Origen
                                if (master.Name == this.masterBodegaOrigen.Name)
                                {
                                    #region Valida el Tipo de Mvto
                                    if (!userBodega.EntradaInd.Value.Value && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Entradas)
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserNotPermittedIN));
                                        this.masterBodegaOrigen.Value = string.Empty;
                                        return;
                                    }
                                    if (!userBodega.SalidaInd.Value.Value && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas)
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserNotPermittedOUTorTRASLATE));
                                        this.masterBodegaOrigen.Value = string.Empty;
                                        return;
                                    } 
                                    #endregion
                                    #region Carga los datos requeridos
                                    this.data.Header.BodegaOrigenID.Value = master.Value;
                                    this.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                                    this.costeoGrupoOri = (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, this.bodegaOrigen.CosteoGrupoInvID.Value, true);
                                    this.tipoBodegaOrig = (DTO_inBodegaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, this.bodegaOrigen.BodegaTipoID.Value, true);
                                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.bodegaOrigen.ProyectoID.Value, true);
                                    DTO_coActividad activ = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proy.ActividadID.Value, true);
                                    this._ctrl.PrefijoID.Value = this.bodegaOrigen.PrefijoID.Value;
                                    base.txtPrefix.Text = this.bodegaOrigen.PrefijoID.Value;
                                    this.masterProyecto.Value = this.bodegaOrigen.ProyectoID.Value;
                                    this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
                                    base.proyectoHeader = this.masterProyecto.Value;
                                    this.masterCentroCto.Value = this.bodegaOrigen.CentroCostoID.Value;
                                    this._ctrl.CentroCostoID.Value = this.masterCentroCto.Value;
                                    base.centroCostoHeader = this.masterCentroCto.Value;
                                    this.txtDatoAdd1.Text = "0"; 
                                    #endregion                                    
                                    #region Segun TipoBodega muestra Controles Adicionales
                                    this.cmbTipoTransporte.SelectedItem = this.cmbTipoTransporte.GetItem(((byte)TipoTransporte.Terrestre).ToString());
                                    if (this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.PuertoFOB)
                                    {
                                        this.cmbTipoTransporte.SelectedItem = this.cmbTipoTransporte.GetItem(((byte)TipoTransporte.Maritimo).ToString());
                                        this.masterTransporProveedor.EnableControl(true);
                                        this.txtDocExternoProv.Enabled = true;
                                        this._bc.InitMasterUC(this.masterTransporProveedor, AppMasters.prProveedor, true, true, true, true);
                                        this.txtDocExternoProv.BackColor = Color.LightBlue;
                                        this.btnFacturaCompra.Visible = true;
                                    }
                                    else if (this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.ZonaFranca)
                                        this.cmbTipoTransporte.SelectedItem = this.cmbTipoTransporte.GetItem(((byte)TipoTransporte.Aereo).ToString());
                                    else
                                    {
                                        this.masterTransporProveedor.EnableControl(false);
                                        this.txtDocExternoProv.Enabled = false;
                                        this._bc.InitMasterUC(this.masterTransporProveedor, AppMasters.prProveedor, true, true, true, false);
                                        this.txtDocExternoProv.BackColor = Color.White;
                                        this.btnFacturaCompra.Visible = false;
                                    }                                    
                                    #endregion
                                    #region Carga el stock de la bodega
                                    base._moduloProyectosInd = base.moduleProyectosActiveInd && activ != null && activ.ModuloProyectosInd.Value.Value ? true : false;
                                    base.mvtoStockBodOrigen = new List<DTO_inMovimientoFooter>();
                                    if (base.moduleProyectosActiveInd && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                    {
                                        //Carga las existencias de la Bodega Stock
                                        if (false)//this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.Stock)
                                            this.GetStockByBodega(master.Value); 
                                        //Carga la Info del Proyecto de la Bodega Proyecto
                                        else if (this.tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.Proyecto)
                                        {
                                            base.GetInfoProyecto(this.masterProyecto.Value);
                                        }
                                        if(this.masterBodegaDestino.ValidID)
                                        {

                                        }

                                    } 
                                    #endregion
                                }
                                #endregion
                                #region Bodega Destino
                                if (master.Name == this.masterBodegaDestino.Name)
                                {
                                    if (master.Value != this.masterBodegaOrigen.Value)
                                    {
                                        if (this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                        {
                                            #region Valida el Tipo de Mvto
                                            if (!userBodega.EntradaInd.Value.Value)
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserNotPermittedOUTorTRASLATE));
                                                this.masterBodegaDestino.Value = string.Empty;
                                                return;
                                            } 
                                            #endregion
                                            #region Carga datos requeridos
                                            this.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                                            this.costeoGrupoDest = (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, this.bodegaDestino.CosteoGrupoInvID.Value, true);
                                            DTO_inBodegaTipo tipoBodegaDest = (DTO_inBodegaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, this.bodegaDestino.BodegaTipoID.Value, true);

                                            if ((this.costeoGrupoOri.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto && this.costeoGrupoDest.CosteoTipo.Value.Value != (byte)TipoCosteoInv.SinCosto) ||
                                                (this.costeoGrupoOri.CosteoTipo.Value.Value != (byte)TipoCosteoInv.SinCosto && this.costeoGrupoDest.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto))
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotTraslateBodegas));
                                                master.Value = string.Empty;
                                                master.Focus();
                                                return;
                                            } 
                                            #endregion
                                            #region Carga la Info del Proyecto de la Bodega
                                            //if (tipoBodegaDest.BodegaTipo.Value == (byte)TipoBodega.Proyecto)
                                            //{
                                                base.GetInfoProyecto(this.masterProyecto.Value);
                                            //}
                                            if (base.mvtoStockBodOrigen.Count > 0)
                                            {
                                                this.data.Footer = base.mvtoStockBodOrigen;
                                                this.gcDocument.DataSource = this.data.Footer;
                                                this.txtObservacion.Text = this._proyectoInfo != null ? this._proyectoInfo.DocCtrl.Descripcion.Value : string.Empty;
                                            } 
                                            #endregion
                                        }
                                        this.data.Header.BodegaDestinoID.Value = master.Value;
                                    }
                                    else
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotTraslateSameBodega));
                                        master.Value = string.Empty;
                                        master.Focus();
                                        return;
                                    }

                                }
                                #endregion
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserAnyPermission));
                                this.masterBodegaOrigen.Value = string.Empty;
                                return;
                            } 
                            #endregion
                        }
                        #endregion
                        break;
                    case "MonedaID":
                        #region Moneda
                        if (master.ValidID)
                        {
                            this.AsignarTasaCambio();
                            this._ctrl.MonedaID.Value = master.Value;
                        }

                        #endregion
                        break;
                    case "ClienteID":
                        #region Cliente
                        if (master.ValidID)
                            this.data.Header.ClienteID.Value = master.Value;
                        #endregion
                        break;
                    case "ProveedorID":
                        #region Proveedor
                        if (master.ValidID)
                        {
                            DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.prProveedor, false, master.Value, true);
                            this.data.DocCtrl.TerceroID.Value = proveedor.TerceroID.Value;
                        }
                        #endregion
                        break;
                    case "ProyectoID":
                        #region Proyecto
                        if (master.ValidID)
                        {
                            DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, master.Value, true);
                            DTO_coActividad actividad = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proy.ActividadID.Value, true);
                            this._ctrl.ProyectoID.Value = master.Value;

                            DTO_coProyecto proyBodega = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.bodegaOrigen.ProyectoID.Value, true);
                            DTO_coActividad actividadBodega = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proyBodega.ActividadID.Value, true);
                            //Verifica que el tipo de proyecto corresponda
                            if (actividad.ProyectoTipo.Value != actividadBodega.ProyectoTipo.Value)
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidTipoProyecto));
                                master.Value = string.Empty;
                            }

                            #region Valida el proyecto de capital de trabajo en el Modulo Operaciones Conjuntas
                            //Verifica luego que el Proyecto de capital de trabajo corresponda si el modulo de Operaciones Conjuntas esta activado
                            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false).ToList();
                            foreach (DTO_aplModulo item in modules)
                            {
                                if (item.ModuloID.Value == ModulesPrefix.oc.ToString())
                                {
                                    if (proy.PryCapitalTrabajo.Value != proyBodega.PryCapitalTrabajo.Value)
                                    {
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidCapTrabProyecto));
                                        master.Value = string.Empty;
                                    }
                                    break;
                                }
                            }   
                            #endregion                         

                            this._ctrl.ProyectoID.Value = master.Value;
                            base.proyectoHeader = master.Value;
                        }

                        #endregion
                        break;
                    case "CentroCostoID":
                        #region CentroCosto
                        if (master.ValidID)
                        {
                            this._ctrl.CentroCostoID.Value = master.Value;
                            base.centroCostoHeader = master.Value;
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "masterHeader_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtControl_Leave(object sender, EventArgs e)
        {
                TextBox ctrl = (TextBox)sender;
                try
                {
                    int index = this.NumFila;
                    switch (ctrl.Name)
                    {
                        case "txtNumber":
                            #region Nro Nota Envio
                            if (this._txtNumberFocus)
                            {
                                this._txtNumberFocus = false;

                                if (this.txtDatoAdd1.Text == string.Empty)
                                    this.txtDatoAdd1.Text = "0";

                                if (this.txtDatoAdd1.Text == "0")
                                {
                                    #region Nueva transaccion
                                    this.gcDocument.DataSource = null;
                                    if (!this.multiMoneda)
                                    {
                                        this.newDoc = true;
                                        this.validHeader = true;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Transaccion existente
                                    try
                                    {
                                        //Verifica si la bodega ya inicio un consecutivo
                                        DTO_glDocumentoControl docCtrlExist = null;
                                        docCtrlExist = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this.txtPrefix.Text, Convert.ToInt32(this.txtDatoAdd1.Text));

                                        #region Si existe el documento
                                        if (docCtrlExist != null)
                                            this.GetMvtoInventario(docCtrlExist);
                                        else
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
                                            //this.txtNumber.Focus();
                                        }
                                        #endregion
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "txtNumber_Leave"));
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            break;
                        case "txtDocExternoProv":
                            #region Doc Externo
                            this.data.Header.DatoAdd1.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtTipoVeh":
                            #region Tipo Vehiculo
                            this.data.Header.DatoAdd2.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtPlacaVeh":
                            #region Placa Vehiculo
                            this.data.Header.DatoAdd3.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtConductor":
                            #region Conductor
                            this.data.Header.DatoAdd4.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtCedula":
                            #region Cedula
                            this.data.Header.DatoAdd5.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtTelConductor":
                            #region Telefono Conductor
                            this.data.Header.DatoAdd6.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtObservacion":
                            #region Observacion
                            this.data.Header.Observacion.Value = ctrl.Text;
                            #endregion
                            break;
                        case "txtDocTercero":
                            #region Doc Tercero de empresa Transportadora
                            this.data.Header.DatoAdd10.Value = ctrl.Text;
                            #endregion
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "txtControl_Leave"));
                }
        }

        /// <summary>
        /// Evento que abre la grilla de facturas de venta
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnFacturasVenta_Click(object sender, EventArgs e)
        {
           try
            {
                if (this.masterCliente.ValidID)
                {
                    ModalFacturacion fact = new ModalFacturacion(this.masterCliente.Value);
                    fact.ShowDialog();
                    if (fact.ReturnVals)
                    {
                        #region Carga la lista de facturas
                        List<DTO_glMovimientoDeta> movDetaList = new List<DTO_glMovimientoDeta>();
                        DTO_glMovimientoDeta movDeta;
                        int i = 0;
                        fact.ReturnListCliente.ForEach(retList =>
                        {                           
                            movDeta = new DTO_glMovimientoDeta();
                            movDeta.NumeroDoc.Value = retList.NumeroDoc.Value.Value;
                            this.numeroDocFact = retList.NumeroDoc.Value.Value;
                            List<DTO_glMovimientoDeta> movDetaListTemp = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(movDeta, false);
                            foreach (var mov in movDetaListTemp)
                            {
                                if (!string.IsNullOrEmpty(mov.ServicioID.Value))
                                {
                                    DTO_faServicios servicio = (DTO_faServicios)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, mov.ServicioID.Value, true);
                                    DTO_faConceptos concepto = (DTO_faConceptos)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, false, servicio.ConceptoIngresoID.Value, true);
                                    if (concepto.TipoConcepto.Value.Value == (byte)TipoConcepto.VentaInv)
                                        movDetaList.Add(mov);
                                }
                            }                            
                        });
                        if (movDetaList.Count > 0)
                            base.isFacturaVenta = true;
                        #endregion
                        #region Agrega los registros de las facturas
                        this.data.Footer.Clear();
                        foreach (var mov in movDetaList)
                        {
                            DTO_inMovimientoFooter footerDet = new DTO_inMovimientoFooter();                           
                            #region Asigna datos a la fila
                            footerDet.Movimiento.Index = i;
                            footerDet.Movimiento.EmpresaID.Value = this.empresaID;
                            footerDet.Movimiento.TerceroID.Value = mov.TerceroID.Value;
                            footerDet.Movimiento.ProyectoID.Value = mov.ProyectoID.Value;
                            footerDet.Movimiento.CentroCostoID.Value = mov.CentroCostoID.Value;
                            footerDet.Movimiento.BodegaID.Value = mov.BodegaID.Value;
                            footerDet.Movimiento.inReferenciaID.Value = mov.inReferenciaID.Value;
                            footerDet.Movimiento.EstadoInv.Value = mov.EstadoInv.Value.Value;
                            this.cmbEstado.SelectedItem = this.cmbEstado.GetItem(((byte)mov.EstadoInv.Value.Value).ToString());
                            footerDet.Movimiento.Parametro1.Value = mov.Parametro1.Value;
                            footerDet.Movimiento.Parametro2.Value = mov.Parametro2.Value;
                            footerDet.ReferenciaIDP1P2.Value = mov.inReferenciaID.Value + (mov.Parametro1.Value == this.param1xDef.ToUpper() ? string.Empty : "-" + mov.Parametro1.Value)
                                                                                        + (mov.Parametro2.Value == this.param2xDef.ToUpper() ? string.Empty : "-" + mov.Parametro2.Value);
                            footerDet.Movimiento.ActivoID.Value = mov.ActivoID.Value.Value;
                            footerDet.Movimiento.MvtoTipoInvID.Value = mov.MvtoTipoInvID.Value;
                            footerDet.Movimiento.IdentificadorTr.Value = mov.IdentificadorTr.Value.Value;
                            footerDet.Movimiento.SerialID.Value = mov.SerialID.Value;
                            footerDet.Movimiento.EmpaqueInvID.Value = mov.EmpaqueInvID.Value;
                            footerDet.Movimiento.CantidadDoc.Value = mov.CantidadDoc.Value.Value;
                            footerDet.Movimiento.CantidadEMP.Value = mov.CantidadEMP.Value.Value; 
                            footerDet.Movimiento.CantidadUNI.Value = mov.CantidadUNI.Value.Value;
                            footerDet.Movimiento.EntradaSalida.Value = mov.EntradaSalida.Value;
                            footerDet.Movimiento.DescripTExt.Value = mov.DescripTExt.Value;
                            footerDet.Movimiento.ValorUNI.Value = mov.ValorUNI.Value.Value;
                            footerDet.Movimiento.DocSoporte.Value = mov.DocSoporte.Value.Value;
                            footerDet.Movimiento.DocSoporteTER.Value = mov.DocSoporteTER.Value;
                            footerDet.Movimiento.Valor1LOC.Value = mov.Valor1LOC.Value.Value;
                            footerDet.Movimiento.Valor2LOC.Value = mov.Valor2LOC.Value.Value;
                            footerDet.Movimiento.Valor1EXT.Value = mov.Valor1EXT.Value.Value;
                            footerDet.Movimiento.Valor2EXT.Value = mov.Valor2EXT.Value.Value;
                            this.data.Footer.Add(footerDet);
                            i++;  
                            #endregion                              
                        }
                        #endregion
                        #region Actualiza la grilla y temporal
                        this.gvDocument.RefreshData();
                        base.LoadData(true);
                        base.deleteOP = true;
                        base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;
                        this.UpdateTemp(this.data);
                        #endregion
                    } 
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidCliente));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "btnFacturasVenta_Click"));
            }
        }

        /// <summary>
        /// Evento que abre la grilla de facturas de compra
        /// </summary>
        /// <param name="sender">Objeto que ejecuta el evento</param>
        /// <param name="e">Evento que se esta ejecutando</param>
        private void btnFacturasCompra_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.masterBodegaOrigen.ValidID)
                {
                    #region Trae los items del detalle de las bodegas en Puerto de Embarque(FOB)
                    ModalNotasEnvio fact = new ModalNotasEnvio(this.masterBodegaOrigen.Value);
                    fact.ShowDialog();
                    if (fact.ReturnVals)
                    {
                        #region Carga la lista de facturas
                        List<DTO_glMovimientoDeta> movDetaList = new List<DTO_glMovimientoDeta>();
                        DTO_glMovimientoDeta movDeta;
                        int i = 0;
                        fact.ReturnList.ForEach(retList =>
                        {
                            movDeta = new DTO_glMovimientoDeta();
                            movDeta.BodegaID.Value = this.masterBodegaOrigen.Value;
                            movDeta.NumeroDoc.Value = retList.NumeroDoc.Value.Value;
                            this.numeroDocFact = retList.NumeroDoc.Value.Value;
                            List<DTO_glMovimientoDeta> tmp = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(movDeta, false);
                            if (tmp != null && tmp.Count > 0)
                                movDetaList.AddRange(tmp);
                        });
                        #endregion
                        #region Agrega los registros de las facturas
                        this.data.Footer.Clear();
                        this.ValidHeaderTB();
                        this.EnableHeader(-1, false);
                        this.data = (DTO_MvtoInventarios)this.LoadTempHeader();
                        foreach (var mov in movDetaList)
                        {
                            DTO_inMovimientoFooter footerDet = new DTO_inMovimientoFooter();
                            #region Asigna datos a la fila
                            footerDet.Movimiento.Index = i;
                            footerDet.Movimiento.EmpresaID.Value = this.empresaID;
                            footerDet.Movimiento.TerceroID.Value = mov.TerceroID.Value;
                            footerDet.Movimiento.ProyectoID.Value = mov.ProyectoID.Value;
                            footerDet.Movimiento.CentroCostoID.Value = mov.CentroCostoID.Value;
                            footerDet.Movimiento.BodegaID.Value = mov.BodegaID.Value;
                            footerDet.Movimiento.inReferenciaID.Value = mov.inReferenciaID.Value;
                            footerDet.Movimiento.EstadoInv.Value = mov.EstadoInv.Value.Value;
                            this.cmbEstado.SelectedItem = this.cmbEstado.GetItem(((byte)mov.EstadoInv.Value.Value).ToString());
                            footerDet.Movimiento.Parametro1.Value = mov.Parametro1.Value;
                            footerDet.Movimiento.Parametro2.Value = mov.Parametro2.Value;
                            footerDet.ReferenciaIDP1P2.Value = mov.inReferenciaID.Value + (mov.Parametro1.Value == this.param1xDef.ToUpper() ? string.Empty : "-" + mov.Parametro1.Value)
                                                                                        + (mov.Parametro2.Value == this.param2xDef.ToUpper() ? string.Empty : "-" + mov.Parametro2.Value);
                            footerDet.Movimiento.ActivoID.Value = mov.ActivoID.Value.Value;
                            footerDet.Movimiento.MvtoTipoInvID.Value = mov.MvtoTipoInvID.Value;
                            footerDet.Movimiento.IdentificadorTr.Value = mov.IdentificadorTr.Value.Value;
                            footerDet.Movimiento.SerialID.Value = mov.SerialID.Value;
                            footerDet.Movimiento.EmpaqueInvID.Value = mov.EmpaqueInvID.Value;
                            footerDet.Movimiento.CantidadDoc.Value = mov.CantidadDoc.Value.Value;
                            footerDet.Movimiento.CantidadEMP.Value = mov.CantidadEMP.Value.Value;
                            footerDet.Movimiento.CantidadUNI.Value = mov.CantidadUNI.Value.Value;
                            footerDet.Movimiento.EntradaSalida.Value = mov.EntradaSalida.Value;
                            footerDet.Movimiento.DescripTExt.Value = mov.DescripTExt.Value;
                            footerDet.Movimiento.ValorUNI.Value = mov.ValorUNI.Value.Value;
                            footerDet.Movimiento.DocSoporte.Value = mov.DocSoporte.Value.Value;
                            footerDet.Movimiento.DocSoporteTER.Value = mov.DocSoporteTER.Value;
                            footerDet.Movimiento.Valor1LOC.Value = mov.Valor1LOC.Value.Value;
                            footerDet.Movimiento.Valor2LOC.Value = mov.Valor2LOC.Value.Value;
                            footerDet.Movimiento.Valor1EXT.Value = mov.Valor1EXT.Value.Value;
                            footerDet.Movimiento.Valor2EXT.Value = mov.Valor2EXT.Value.Value;
                            this.data.Footer.Add(footerDet);
                            i++;
                            #endregion
                        }
                        #endregion

                        #region Actualiza la grilla y temporal
                        this.gvDocument.RefreshData();
                        base.LoadData(true);
                        base.deleteOP = true;
                        base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;
                        this.UpdateTemp(this.data);
                        #endregion
                    } 
                    #endregion
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidBodegaOrigen));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "btnFacturasCompra_Click"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando deja el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoTransporte_SelectedValueChanged(object sender, EventArgs e)
        {
            this._tipoTransporte = (TipoTransporte)Enum.Parse(typeof(TipoTransporte), (this.cmbTipoTransporte.SelectedItem as ComboBoxItem).Value);
            try
            {
                switch (_tipoTransporte)
                {
                    case TipoTransporte.Terrestre:
                        #region Terrestre
                        this.lblDatoAdd2.Visible = true;
                        this.lblDatoAdd2.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblDocCliente");
                        this.lblDatoAdd3.Visible = true;
                        this.lblDatoAdd3.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblTipoVehiculo");
                        this.lblDatoAdd4.Visible = true;
                        this.lblDatoAdd5.Visible = true;
                        this.lblDatoAdd6.Visible = true;
                        this.lblDatoAdd7.Visible = true;
                        this.txtDatoAdd2.Visible = true;
                        this.txtDatoAdd3.Visible = true;
                        this.txtDatoAdd4.Visible = true;
                        this.txtDatoAdd5.Visible = true;
                        this.txtDatoAdd6.Visible = true;
                        this.txtDatoAdd7.Visible = true;
                        #endregion
                        break;
                    case TipoTransporte.Aereo:
                        #region Aereo
                        this.lblDatoAdd2.Visible = true;
                        this.lblDatoAdd2.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblManifiestoEnvio");
                        this.lblDatoAdd3.Visible = true;
                        this.lblDatoAdd3.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblGuiaAerea");
                        this.lblDatoAdd4.Visible = false;
                        this.lblDatoAdd5.Visible = false;
                        this.lblDatoAdd6.Visible = false;
                        this.lblDatoAdd7.Visible = false;
                        this.txtDatoAdd2.Visible = true;
                        this.txtDatoAdd3.Visible = true;
                        this.txtDatoAdd4.Visible = false;
                        this.txtDatoAdd5.Visible = false;
                        this.txtDatoAdd6.Visible = false;
                        this.txtDatoAdd7.Visible = false;
                        #endregion
                        break;
                    case TipoTransporte.Maritimo:
                        #region Maritimo
                        this.lblDatoAdd2.Visible = true;
                        this.lblDatoAdd2.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblManifiestoCarga");
                        this.lblDatoAdd3.Visible = false;
                        this.lblDatoAdd4.Visible = false;
                        this.lblDatoAdd5.Visible = false;
                        this.lblDatoAdd6.Visible = false;
                        this.lblDatoAdd7.Visible = false;
                        this.txtDatoAdd2.Visible = true;
                        this.txtDatoAdd3.Visible = false;
                        this.txtDatoAdd4.Visible = false;
                        this.txtDatoAdd5.Visible = false;
                        this.txtDatoAdd6.Visible = false;
                        this.txtDatoAdd7.Visible = false;
                        #endregion
                        break;
                    case TipoTransporte.TraficoPostal:
                        #region TraficoPostal
                        this.lblDatoAdd2.Visible = true;
                        this.lblDatoAdd2.Text = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_lblManifiestoTraficoPostal");
                        this.lblDatoAdd3.Visible = false;
                        this.lblDatoAdd4.Visible = false;
                        this.lblDatoAdd5.Visible = false;
                        this.lblDatoAdd6.Visible = false;
                        this.lblDatoAdd7.Visible = false;
                        this.txtDatoAdd2.Visible = true;
                        this.txtDatoAdd3.Visible = false;
                        this.txtDatoAdd4.Visible = false;
                        this.txtDatoAdd5.Visible = false;
                        this.txtDatoAdd6.Visible = false;
                        this.txtDatoAdd7.Visible = false;
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "cmbTipoTransporte_Leave"));
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            if (this.masterMvtoTipoInv.ValidID)
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.NotaEnvio);
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.GetMvtoInventario(getDocControl.DocumentoControl);
                    this.txtNumber.Text = this.data.DocCtrl.DocumentoNro.Value.ToString();
                    if (!getDocControl.CopiadoInd)
                        this.EnableFooter(false);
                    this.EnableHeader(0, false);
                }
            }
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_NoAssignedMove));
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
                this.EnableHeader(-1, false);
                DTO_MvtoInventarios saldoCosto = new DTO_MvtoInventarios();
                try
                {
                    saldoCosto = (DTO_MvtoInventarios)this.LoadTempHeader();
                    this.data = saldoCosto;
                    this.LoadData(true);
                    this.UpdateTemp(this.data);
                    if (this.gvDocument.DataRowCount == 0) this.isValid = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "gcDocument_Enter: " + ex.Message));
                }

                #endregion
            }
            else
                this.masterMvtoTipoInv.Focus();
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
                this.masterMvtoTipoInv.Focus();
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
                    this.data = new DTO_MvtoInventarios();
                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this._documentoNro = 0;
                    this.CleanHeader(true);
                    this.EnableHeader(0,false);
                    this.masterMvtoTipoInv.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "TBNew: " + ex.Message)); 
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
                if (this.ValidGrid() && this.data.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "TBSave: " + ex.Message)); 
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                
                int numeroDoc = 0;
                bool update = false;
                bool isOK = false;
                DTO_SerializedObject obj = null;
                if (base.isFacturaVenta)
                {
                    if(this.numeroDocFact != 0) 
                    {
                        #region Llena un Movimiento para facturas
                        DTO_inMovimientoDocu header = new DTO_inMovimientoDocu();
                        header.MvtoTipoInvID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_TipoMovVentasLoc);
                        header.BodegaOrigenID.Value = string.Empty;
                        header.BodegaDestinoID.Value = string.Empty;
                        header.EmpresaID.Value = this.empresaID;
                        header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                        header.DocumentoREL.Value = numeroDocFact;
                        header.ClienteID.Value = this.masterCliente.Value;
                        header.DatoAdd1.Value = this.txtDatoAdd2.Text;
                        header.DatoAdd2.Value = this.txtDatoAdd3.Text;
                        header.DatoAdd3.Value = this.txtDatoAdd4.Text;
                        header.DatoAdd4.Value = this.txtDatoAdd5.Text;
                        header.DatoAdd5.Value = this.txtDatoAdd6.Text;
                        header.TipoTransporte.Value = (byte)this._tipoTransporte;
                        header.DatoAdd6.Value = this.txtDatoAdd7.Text;
                        header.Observacion.Value = this.txtObservacion.Text;

                        DTO_glDocumentoControl ctrlFactura = new DTO_glDocumentoControl();
                        DTO_faCliente cliente = (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                        ctrlFactura.TerceroID.Value = cliente != null ? cliente.TerceroID.Value : null;
                        ctrlFactura.NumeroDoc.Value = 0;
                        ctrlFactura.MonedaID.Value = this.monedaId;
                        //this._ctrl.ProyectoID.Value = this.masterProyecto.Value;
                        //this._ctrl.CentroCostoID.Value = this.masterCentroCto.Value;
                        ctrlFactura.PrefijoID.Value = this.prefijoID;
                        ctrlFactura.Fecha.Value = DateTime.Now;
                        ctrlFactura.PeriodoDoc.Value = this.dtPeriod.DateTime;
                        ctrlFactura.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                        ctrlFactura.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                        ctrlFactura.DocumentoID.Value = this.documentID;
                        ctrlFactura.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        ctrlFactura.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                        ctrlFactura.seUsuarioID.Value = this.userID;
                        ctrlFactura.AreaFuncionalID.Value = this.areaFuncionalID;
                        ctrlFactura.ConsSaldo.Value = 0;
                        ctrlFactura.Estado.Value = (byte)EstadoDocControl.Aprobado;
                        ctrlFactura.FechaDoc.Value = this.dtFecha.DateTime;
                        ctrlFactura.Descripcion.Value = base.txtDocDesc.Text;
                        ctrlFactura.Observacion.Value = this.txtObservacion.Text;
                        ctrlFactura.Valor.Value = 0;
                        ctrlFactura.Iva.Value = 0;

                        this.data.Header = header;
                        this.data.DocCtrl = ctrlFactura;
                        this.data.Footer = new List<DTO_inMovimientoFooter>(); 
                        #endregion
                        #region Guarda el Mvto
                        obj = this._bc.AdministrationModel.Transaccion_Add(this.documentID, this.data, update, out numeroDoc);
                        FormProvider.Master.StopProgressBarThread(this.documentID);
                        isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true); 
                        #endregion
                    }
                }
                else
                {
                    #region Guarda el Mvto
                    if (this.data.DocCtrl.NumeroDoc.Value != null && this.data.DocCtrl.NumeroDoc.Value != 0)
                    {
                        update = true;
                        numeroDoc = this.data.DocCtrl.NumeroDoc.Value.Value;
                    }
                    if (this.tipoMovActual == TipoMovimientoInv.Traslados && !this.data.Header.TipoTraslado.Value.HasValue)
                        this.data.Header.TipoTraslado.Value = 0;//Pone traslado stock
                    else if (this.tipoMovActual != TipoMovimientoInv.Traslados)
                        this.data.Header.TipoTraslado.Value = null;

                    obj = this._bc.AdministrationModel.Transaccion_Add(this.documentID, this.data, update, out numeroDoc);
                    FormProvider.Master.StopProgressBarThread(this.documentID);
                    isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true); 
                    #endregion
                }                
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_MvtoInventarios();
                    base._documentoNro = 0;

                    #region Genera Reporte

                    if (obj != null && obj.GetType() == typeof(DTO_Alarma))
                    {
                        int numDoc = Convert.ToInt32(((DTO_Alarma)obj).NumeroDoc);
                        string reportName = this._bc.AdministrationModel.Reports_In_TransaccionMvto(null, this.documentID, numDoc, this.mvtoTipo.TipoMovimiento.Value.Value);

                        if (reportName == string.Empty)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                        else
                        {
                            //Muestra el reporte que se genero
                            string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, null);
                            Process.Start(fileURl);
                        }
                    }

                    #endregion

                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-NotaEnvio.cs", "SaveThread: " + ex.Message)); 
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion
    }
}
