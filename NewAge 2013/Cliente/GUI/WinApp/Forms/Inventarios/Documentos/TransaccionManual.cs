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
    public partial class TransaccionManual : DocumentMvtoForm
    {
        public TransaccionManual()
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
        private bool _terceroInd = false;
        private bool _txtNumberFocus;
        private bool bodTransitoOrZonaFrancaInd = false;
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.TransaccionManual;
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
            this._bc.InitMasterUC(this.masterTerceroHeader, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyectoOrig, AppMasters.coProyecto, false, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCtoOrig, AppMasters.coCentroCosto, false, true, true, false);
            this._bc.InitMasterUC(this.masterProyectoDest, AppMasters.coProyecto, false, true, true, false);
            this._bc.InitMasterUC(this.masterCentroCtoDest, AppMasters.coCentroCosto, false, true, true, false);
            this._bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            this.masterBodegaDestino.EnableControl(false);
            this.masterTerceroHeader.EnableControl(false);
            this.masterProyectoOrig.EnableControl(false);
            this.masterCentroCtoOrig.EnableControl(false);
            this.masterProyectoDest.EnableControl(false);
            this.masterCentroCtoDest.EnableControl(false);
            this.masterMoneda.EnableControl(false);           
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.EnableFooter(this.data.Footer.Count > 0 ? true:false);
            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.masterMoneda.EnableControl(false);
                this.masterMoneda.Value = this.monedaLocal;
                this.lblTasaCambio.Visible = false;
                this.txtTasaCambio.Visible = false;
            }
            else
                this.masterMoneda.Value = this.monedaLocal;
            this.AsignarTasaCambio();

            //Ajuste el tamaño de las secciones
            base.tlSeparatorPanel.RowStyles[0].Height = 162;
            base.tlSeparatorPanel.RowStyles[1].Height = 210;
            base.tlSeparatorPanel.RowStyles[2].Height = 180;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.in_Periodo);
                this.dtPeriod.Text = periodo;

                this.masterMvtoTipoInv.Value = string.Empty;
                this.txtNumber.Text = string.Empty;
            }
            this.masterMoneda.Value = this.monedaLocal;
            this.AsignarTasaCambio();
            this.masterBodegaOrigen.Value = string.Empty;
            this.masterBodegaDestino.Value = string.Empty;
            this.masterCentroCtoOrig.Value = string.Empty;
            this.masterProyectoOrig.Value = string.Empty;
            this.masterTerceroHeader.Value = string.Empty;
            this.masterCentroCtoDest.Value = string.Empty;
            this.masterProyectoDest.Value = string.Empty;
            this.txtDocTercero.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            base.CleanHeader(basic);
            this.masterMvtoTipoInv.Focus();
        }

        /// <summary>
        /// Deshabilita los controles del header
        /// </summary>
        protected override void EnableHeader(short TipoMov, bool basic)
        {
            this.grpboxHeader.Enabled = true;
            this.masterMvtoTipoInv.EnableControl(true);
            this.masterBodegaOrigen.EnableControl(true);
            this.txtNumber.Enabled = true;
            this.btnQueryDoc.Enabled = true;
            if (basic)
            {
                #region Habilita para Entradas
                if (TipoMov == (byte)TipoMovimientoInv.Entradas)
                {
                    if (this.multiMoneda)
                    {
                        this.masterMoneda.EnableControl(true);
                        this.lblTasaCambio.Visible = true;
                        this.txtTasaCambio.Visible = true;
                    }
                    else
                    {
                        this.masterMoneda.EnableControl(false);
                        this.lblTasaCambio.Visible = false;
                        this.txtTasaCambio.Visible = false;
                    }
                    this.masterBodegaDestino.EnableControl(false);
                    this.masterProyectoOrig.EnableControl(false);
                    this.masterCentroCtoOrig.EnableControl(false);
                    this.masterProyectoDest.EnableControl(false);
                    this.masterCentroCtoDest.EnableControl(false);
                    if (mvtoTipo.TerceroInd.Value.Value)
                    {
                        this._terceroInd = true;
                        this.masterTerceroHeader.EnableControl(true);
                        this.txtDocTercero.Enabled = true;
                    }
                    else
                    {
                        this._terceroInd = false;
                        this.masterTerceroHeader.EnableControl(false);
                        this.txtDocTercero.Enabled = false;
                    }
                }
                #endregion               
                #region Habilita para Salidas
                else if (TipoMov == (byte)TipoMovimientoInv.Salidas)
                {
                    this.masterProyectoOrig.EnableControl(false);
                    this.masterCentroCtoOrig.EnableControl(false);
                    this.masterProyectoDest.EnableControl(false);
                    this.masterCentroCtoDest.EnableControl(false);
                    this.masterBodegaDestino.EnableControl(false);
                    this.masterMoneda.EnableControl(false);
                    if (mvtoTipo.TerceroInd.Value.Value)
                    {
                        this._terceroInd = true;
                        this.masterTerceroHeader.EnableControl(true);
                        this.txtDocTercero.Enabled = true;
                    }
                    else
                    {
                        this._terceroInd = false;
                        this.masterTerceroHeader.EnableControl(false);
                        this.txtDocTercero.Enabled = false;
                    }
                }
                #endregion
                #region Habilita para Traslados
                else if (TipoMov == (byte)TipoMovimientoInv.Traslados)
                {
                    this.masterBodegaDestino.EnableControl(true);
                    this.masterProyectoOrig.EnableControl(false);
                    this.masterCentroCtoOrig.EnableControl(false);
                    this.masterProyectoDest.EnableControl(false);
                    this.masterCentroCtoDest.EnableControl(false);
                    this.masterMoneda.EnableControl(false);
                    if (mvtoTipo.TerceroInd.Value.Value)
                    {
                        this._terceroInd = true;
                        this.masterTerceroHeader.EnableControl(true);
                        this.txtDocTercero.Enabled = true;
                    }
                    else
                    {
                        this._terceroInd = false;
                        this.masterTerceroHeader.EnableControl(false);
                        this.txtDocTercero.Enabled = false;
                    }
                }
                #endregion            
            }
            else
            {
                if (TipoMov != 0)
                {
                    this.masterMvtoTipoInv.EnableControl(false);
                    this.masterBodegaOrigen.EnableControl(false);
                    this.txtNumber.Enabled = false;
                    this.btnQueryDoc.Enabled = false;
                }
                this.masterProyectoOrig.EnableControl(false);
                this.masterCentroCtoOrig.EnableControl(false);
                this.masterProyectoDest.EnableControl(false);
                this.masterCentroCtoDest.EnableControl(false);
                this.masterMoneda.EnableControl(false);
                this.masterBodegaDestino.EnableControl(false);
                this.masterTerceroHeader.EnableControl(false);
                this.txtDocTercero.Enabled = false;
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
                header.DatoAdd1.Value = this.txtDocTransporte.Text;
                header.DatoAdd2.Value = this.txtManifiestoCarga.Text;
                header.TipoTraslado.Value = this.data != null? this.data.Header.TipoTraslado.Value : header.TipoTraslado.Value;
                //header.DocumentoREL.Value = 
                this.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaOrigen.Value, true);
                this.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodegaDestino.Value, true);

                this._ctrl.TerceroID.Value = this.masterTerceroHeader.Value;
                this._ctrl.NumeroDoc.Value = this.data.DocCtrl.NumeroDoc.Value != null ? this.data.DocCtrl.NumeroDoc.Value : 0;
                this._ctrl.MonedaID.Value = this.masterMoneda.Value;
                this._ctrl.ProyectoID.Value = this.masterProyectoOrig.Value;
                this._ctrl.CentroCostoID.Value = this.masterCentroCtoOrig.Value;
                this._ctrl.PrefijoID.Value = this.bodegaOrigen.PrefijoID.Value;
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
                this._ctrl.Observacion.Value = this.txtObservacion.Text;
                this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                //this._ctrl.UsuarioRESP.Value = this._bc.AdministrationModel.User.ID.Value;

                DTO_MvtoInventarios mvto = new DTO_MvtoInventarios();
                mvto.Header = header;
                mvto.DocCtrl = this._ctrl;
                if (this.data != null && this.data.Footer.Count > 0)
                    mvto.Footer = this.data.Footer;
                else
                    mvto.Footer = new List<DTO_inMovimientoFooter>();

                return mvto;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "LoadTempHeader: " + ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio()
        {
            if (this.monedaLocal == this.masterMoneda.Value)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            //Si la empresa no permite multimoneda
            if (!this.multiMoneda)
                this.txtTasaCambio.EditValue = 0;
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio((byte)TipoMoneda.Foreign, dtFecha.DateTime);
                base._tasaCambioValue = Convert.ToDecimal(txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
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
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterMvtoTipoInv.CodeRsx);

                MessageBox.Show(msg);
                this.masterMvtoTipoInv.Focus();

                result = false;
            }
            if (!this.masterBodegaOrigen.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblBodegaOrigen.Text);

                MessageBox.Show(msg);
                this.masterBodegaOrigen.Focus();

                result = false;
            }
            if (!this.masterBodegaDestino.ValidID && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblBodegaDestino.Text);

                MessageBox.Show(msg);
                this.masterBodegaDestino.Focus();

                result = false;
            }
            if (!this.masterTerceroHeader.ValidID && _terceroInd)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterTerceroHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterTerceroHeader.Focus();

                result = false;
            }
            if (string.IsNullOrEmpty(this.txtDocTercero.Text) && _terceroInd)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblDocTercero.Text);

                MessageBox.Show(msg);
                this.txtDocTercero.Focus();

                result = false;
            }
            if (!this.masterProyectoOrig.ValidID && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterProyectoOrig.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyectoOrig.Focus();

                result = false;
            }
            if (!this.masterCentroCtoOrig.ValidID && this.mvtoTipo != null && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCentroCtoOrig.CodeRsx);

                MessageBox.Show(msg);
                this.masterCentroCtoOrig.Focus();

                result = false;
            }
            if (!this.masterMoneda.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterMoneda.Text);

                MessageBox.Show(msg);
                this.masterMoneda.Focus();

                result = false;
            }
            if (this.bodTransitoOrZonaFrancaInd && string.IsNullOrEmpty(this.txtDocTransporte.Text) && string.IsNullOrEmpty(this.txtManifiestoCarga.Text))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_TransporteManifiestoEmpty));
                this.txtDocTransporte.Focus();
                result = false;
            }
            else
            {
                if (this.bodTransitoOrZonaFrancaInd)
                {
                    DTO_inMovimientoDocu headerExist = new DTO_inMovimientoDocu();
                    headerExist.DatoAdd1.Value = this.txtDocTransporte.Text;
                    List<DTO_inMovimientoDocu> movExist = this._bc.AdministrationModel.inMovimientoDocu_GetByParameter(this.documentID, headerExist);
                    if (movExist != null &&  movExist.Count > 0)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Err_Pr_DocTransporteExist));
                        this.txtDocTransporte.Focus();
                        result = false;
                    } 
                }
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
                this.txtNumber.Text = ctrl.DocumentoNro.Value != null ? ctrl.DocumentoNro.Value.ToString() : "0";
                this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
                this.dtFecha.DateTime = ctrl.FechaDoc.Value.Value;
                this.masterMvtoTipoInv.Value = header.MvtoTipoInvID.Value;
                this.masterBodegaOrigen.Value = header.BodegaOrigenID.Value;
                this.masterBodegaDestino.Value = header.BodegaDestinoID.Value;
                this.masterTerceroHeader.Value = ctrl.TerceroID.Value;
                this.txtDocTercero.Text = ctrl.DocumentoTercero.Value;
                this.txtObservacion.Text = ctrl.Observacion.Value;
                this.masterMoneda.Value = ctrl.MonedaID.Value;
                this.masterProyectoOrig.Value = ctrl.ProyectoID.Value;
                this.masterCentroCtoOrig.Value = ctrl.CentroCostoID.Value;
                base.proyectoHeader = ctrl.ProyectoID.Value;
                base.centroCostoHeader = ctrl.CentroCostoID.Value;             
                base.bodegaOrigen = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, header.BodegaOrigenID.Value, true);
                base.costeoGrupoOri = !string.IsNullOrEmpty(header.BodegaOrigenID.Value) ? (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, base.bodegaOrigen.CosteoGrupoInvID.Value, true) : null;
                base.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, header.BodegaDestinoID.Value, true);
                base.costeoGrupoDest = !string.IsNullOrEmpty(header.BodegaDestinoID.Value) ? (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, base.bodegaDestino.CosteoGrupoInvID.Value, true) : null;
                base.mvtoTipo = (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, this.masterMvtoTipoInv.Value, true);
                this._terceroInd = this.mvtoTipo.TerceroInd.Value.Value;
                base.terceroHeader = this._terceroInd ? ctrl.TerceroID.Value : string.Empty;
                base._documentoNro = ctrl.DocumentoNro.Value != null ? ctrl.DocumentoNro.Value.Value : 0;
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
                        this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                    this.validHeader = true;
                    this.gcDocument.Focus();
                }
                else
                    this.CleanHeader(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "LoadTempData: " + ex.Message));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Obtiene un movimiento de Inventarios
        /// </summary>
        private void GetMvtoInventario(DTO_glDocumentoControl ctrl)
        {
            if (ctrl != null)
            {
                try
                {
                    if (ctrl.Estado.Value.Value == (byte)EstadoDocControl.Aprobado || ctrl.Estado.Value.Value == (byte)EstadoDocControl.Revertido)
                    {
                        DTO_MvtoInventarios saldoCostos = this._bc.AdministrationModel.Transaccion_Get(this.documentID, ctrl.NumeroDoc.Value.Value);
                        if (!string.IsNullOrEmpty(this.txtNumber.Text) && saldoCostos.Header.BodegaOrigenID.Value != this.masterBodegaOrigen.Value)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberxBodega));
                            this.masterBodegaOrigen.Focus();
                            return;
                        }
                        FormProvider.Master.itemSave.Enabled = true;
                        if (this._copyData)
                        {
                            saldoCostos.DocCtrl.NumeroDoc.Value = 0;
                            saldoCostos.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            saldoCostos.Header.NumeroDoc.Value = 0;
                            this._copyData = false;
                        }
                        else
                        {
                            FormProvider.Master.itemSave.Enabled = false;
                            FormProvider.Master.itemPrint.Enabled = true;
                        }

                        this.masterBodegaOrigen.Value = saldoCostos.Header.BodegaOrigenID.Value;
                        this.masterBodegaDestino.Value = saldoCostos.Header.BodegaDestinoID.Value;
                        this.dtFecha.DateTime = saldoCostos.DocCtrl.FechaDoc.Value.Value;
                        this.masterMoneda.Value = ctrl.MonedaID.Value;
                        this.masterTerceroHeader.Value = ctrl.TerceroID.Value;
                        this.masterProyectoOrig.Value = ctrl.ProyectoID.Value;
                        this.masterCentroCtoOrig.Value = ctrl.CentroCostoID.Value;
                        this.txtDocTercero.Text = ctrl.DocumentoTercero.Value;
                        this.txtObservacion.Text = ctrl.Observacion.Value;
                        if (this.AsignarTasaCambio())
                        {
                            this.data = saldoCostos;
                            this._totalUnidades = 0;
                            if (this.data.Footer.Count > 0)
                                this.EnableFooter(true);
                            foreach (var footer in this.data.Footer)
                                this._totalUnidades += footer.Movimiento.CantidadUNI.Value.Value;
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
                    else
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "GetMvtoInventario: " + ex.Message));
                }
            }
            else
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidNumberTransaccion));
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

                if(this.txtNumber.Text == string.Empty)
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
                            this.mvtoTipo = (DTO_inMovimientoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMovimientoTipo, false, master.Value, true);
                            if (this.mvtoTipo != null)
                            {
                                this.EnableHeader(mvtoTipo.TipoMovimiento.Value.Value,true);
                                this.data.Header.MvtoTipoInvID.Value = master.Value;
                                base.tipoMovActual = (TipoMovimientoInv)Enum.Parse(typeof(TipoMovimientoInv), mvtoTipo.TipoMovimiento.Value.ToString());
                                if (mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Salidas || mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                {
                                    base.pnlSaldos.Visible = true;
                                    base.pnlValor.Enabled = false;
                                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[2].Enabled = true;
                                }
                                else
                                {
                                    base.pnlSaldos.Visible = false;
                                    base.pnlValor.Enabled = true;
                                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[2].Enabled = false;
                                }
                            }
                        }

                        #endregion
                        break;
                    case "BodegaID":
                        #region Bodegas
                        if (master.ValidID)
                        {
                            Dictionary<string, string> keysUserBodega = new Dictionary<string, string>();
                            keysUserBodega.Add("seUsuarioID", this.userID.ToString());
                            keysUserBodega.Add("BodegaID",this.masterBodegaOrigen.Value);
                            userBodega = (DTO_seUsuarioBodega)this._bc.GetMasterComplexDTO(AppMasters.seUsuarioBodega, keysUserBodega, true);
                            if (userBodega != null && this.mvtoTipo != null)
                            {
                                #region Bodega Origen
                                if (master.Name == this.masterBodegaOrigen.Name)
                                {
                                    #region Valida el tipo de mvto
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
                                    DTO_inBodegaTipo tipoBodegaOrig = (DTO_inBodegaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, this.bodegaOrigen.BodegaTipoID.Value, true);
                                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.bodegaOrigen.ProyectoID.Value, true);
                                    DTO_coActividad activ = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proy.ActividadID.Value, true);
                                    this._ctrl.PrefijoID.Value = this.bodegaOrigen.PrefijoID.Value;
                                    base.txtPrefix.Text = this.bodegaOrigen.PrefijoID.Value;
                                    this.masterProyectoOrig.Value = this.bodegaOrigen.ProyectoID.Value;
                                    this._ctrl.ProyectoID.Value = this.masterProyectoOrig.Value;
                                    base.proyectoHeader = this.masterProyectoOrig.Value;
                                    this.masterCentroCtoOrig.Value = this.bodegaOrigen.CentroCostoID.Value;
                                    this._ctrl.CentroCostoID.Value = this.masterCentroCtoOrig.Value;
                                    base.centroCostoHeader = this.masterCentroCtoOrig.Value;                                 
                                    this.txtNumber.Text = "0"; 
                                    #endregion
                                    #region Carga el stock de la bodega
                                    base._moduloProyectosInd = base.moduleProyectosActiveInd && activ != null && activ.ModuloProyectosInd.Value.Value ? true : false;
                                    base.mvtoStockBodOrigen = new List<DTO_inMovimientoFooter>();
                                    if (base.moduleProyectosActiveInd && this.mvtoTipo.TipoMovimiento.Value == (byte)TipoMovimientoInv.Traslados)
                                    {                                       
                                        //Carga la Info del Proyecto de la Bodega Proyecto
                                        if (tipoBodegaOrig.BodegaTipo.Value == (byte)TipoBodega.Proyecto)
                                        {
                                            //Carga las existencias de la Bodega
                                            //base.GetStockByBodega(master.Value);
                                            //base.GetInfoProyecto(this.masterProyecto.Value);
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
                                            if (!userBodega.EntradaInd.Value.Value)
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_UserNotPermittedOUTorTRASLATE));
                                                this.masterBodegaDestino.Value = string.Empty;
                                                return;
                                            }

                                            this.bodegaDestino = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                                            this.costeoGrupoDest = (DTO_inCosteoGrupo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inCosteoGrupo, false, this.bodegaDestino.CosteoGrupoInvID.Value, true);

                                            if ((this.costeoGrupoOri.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto && this.costeoGrupoDest.CosteoTipo.Value.Value != (byte)TipoCosteoInv.SinCosto) ||
                                                (this.costeoGrupoOri.CosteoTipo.Value.Value != (byte)TipoCosteoInv.SinCosto && this.costeoGrupoDest.CosteoTipo.Value.Value == (byte)TipoCosteoInv.SinCosto))
                                            {
                                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_NotTraslateBodegas));
                                                master.Value = string.Empty;
                                                master.Focus();
                                                return;
                                            }
                                            #region Valida el tipo de Bodega
                                            DTO_inBodegaTipo bodTipo = (DTO_inBodegaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, this.bodegaDestino.BodegaTipoID.Value, true);
                                            if (bodTipo.BodegaTipo.Value == (byte)TipoBodega.Transito || bodTipo.BodegaTipo.Value == (byte)TipoBodega.ZonaFranca)
                                            {
                                                this.lblDocTransporte.Visible = true;
                                                this.txtDocTransporte.Visible = true;

                                                this.lblManifiestoCarga.Visible = true;
                                                this.txtManifiestoCarga.Visible = true;
                                                this.bodTransitoOrZonaFrancaInd = true;
                                            }
                                            else
                                            {
                                                this.lblDocTransporte.Visible = false;
                                                this.txtDocTransporte.Visible = false;
                                                this.lblManifiestoCarga.Visible = false;
                                                this.txtManifiestoCarga.Visible = false;
                                                this.bodTransitoOrZonaFrancaInd = false;
                                            } 
                                            #endregion
                                            #region Carga la Info del Proyecto de la Bodega
                                            if (bodTipo.BodegaTipo.Value == (byte)TipoBodega.Proyecto)
                                            {
                                                //base.GetInfoProyecto(this.masterProyecto.Value);
                                            }
                                            if (base.mvtoStockBodOrigen.Count > 0)
                                            {
                                                this.data.Footer = base.mvtoStockBodOrigen;
                                                this.gcDocument.DataSource = this.data.Footer;
                                            }
                                            #endregion
                                        }
                                        this.data.Header.BodegaDestinoID.Value = master.Value;
                                        this.masterProyectoDest.Value = this.bodegaDestino.ProyectoID.Value;
                                        this.masterCentroCtoDest.Value = this.bodegaDestino.CentroCostoID.Value;
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
                            //Verifica luego que el Proyecto de capital de trabajo corresponda si el modulo de Operaciones Conjuntas esta activado
                            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false).ToList();
                            bool OperacionConjuntasActive = false;
                            foreach (DTO_aplModulo item in modules)
                            {
                                if (item.ModuloID.Value == ModulesPrefix.oc.ToString())
                                {
                                    OperacionConjuntasActive = true;
                                    break;
                                }
                            }
                            if (OperacionConjuntasActive)
                            {
                                if (proy.PryCapitalTrabajo.Value != proyBodega.PryCapitalTrabajo.Value)
                                {
                                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.In_InvalidCapTrabProyecto));
                                    master.Value = string.Empty;
                                } 
                            }
                            this._ctrl.ProyectoID.Value = master.Value;
                            base.proyectoHeader = master.Value;
                        }

                        #endregion
                        break;
                    case "TerceroID":
                        #region Tercero
                        if (master.ValidID)
                        {
                            this._ctrl.TerceroID.Value = master.Value;
                            base.terceroHeader = master.Value;
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "masterHeader_Leave: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocTercero_Leave(object sender, EventArgs e)
        {
            this._ctrl.DocumentoTercero.Value = this.txtDocTercero.Text;
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
                docs.Add(AppDocuments.TransaccionManual);
                docs.Add(AppDocuments.TransaccionAutomatica);
                ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.GetMvtoInventario(getDocControl.DocumentoControl);
                    this.txtNumber.Text = this.data.DocCtrl.DocumentoNro.Value.ToString();
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
            try
            {
                if (this.ValidateHeader())
                    this.validHeader = true;
                else
                    this.validHeader = false;
                //Si el diseño esta cargado y el header es valido
                if (this.validHeader)
                {
                    this.ValidHeaderTB();
                    if (this.txtNumber.Text == "0")
                    {
                        FormProvider.Master.itemExport.Enabled = false;
                        FormProvider.Master.itemPrint.Enabled = false;
                    }
                    else
                        FormProvider.Master.itemPrint.Enabled = true;
                    #region Si entra al detalle y no tiene datos
                    this.EnableHeader(-1, false);
                    DTO_MvtoInventarios saldoCosto = new DTO_MvtoInventarios();
                    try
                    {
                        //if (this.data == null || this.data.Footer.Count == 0)
                        //{
                            saldoCosto = (DTO_MvtoInventarios)this.LoadTempHeader();
                            this.data = saldoCosto;

                            this.LoadData(true);
                            this.UpdateTemp(this.data);
                            if (this.gvDocument.DataRowCount == 0) this.isValid = true;
                        //}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "grlDocument_Enter: " + ex.Message));
                    }

                    #endregion
                }
                else
                    this.masterMvtoTipoInv.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "TransaccionManual.cs-gcDocument_Enter: " + ex.Message));
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
                    this.EnableHeader(0,true);
                    this.masterMvtoTipoInv.Focus();
                }
                FormProvider.Master.itemSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "TBNew: " + ex.Message));

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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "TBSave: " + ex.Message));

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
                if (this.data.DocCtrl.NumeroDoc.Value.Value != 0)
                {
                    update = true;
                    numeroDoc = this.data.DocCtrl.NumeroDoc.Value.Value;
                }
                if (this.tipoMovActual == TipoMovimientoInv.Traslados && !this.data.Header.TipoTraslado.Value.HasValue)
                    this.data.Header.TipoTraslado.Value = 0;//Pone traslado stock
                else if (this.tipoMovActual != TipoMovimientoInv.Traslados)
                    this.data.Header.TipoTraslado.Value = null;

                DTO_SerializedObject obj = this._bc.AdministrationModel.Transaccion_Add(this.documentID, this.data, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_MvtoInventarios();
                    base._documentoNro = 0;

                    #region Genera Reporte

                    if (obj.GetType() == typeof(DTO_Alarma))
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TransaccionManual.cs", "SaveThread: " + ex.Message));
            }
            finally
            {               
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion  
    }
}
