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
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class CajaMenor : DocumentLegalizaForm
    {
        public CajaMenor()
        {
           //InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.data.Footer;

            this.CleanHeader(true);
            this.EnableHeader(true);
            this.EnableFooter(false);
            this.masterCajaMenor.Focus();
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
           // this.TBNew();
            this.gcDocument.DataSource = this.data.Footer;
            FormProvider.Master.itemSendtoAppr.Enabled = false;

            this.CleanHeader(true);
            this.EnableHeader(true);
            this.EnableFooter(false);
            this.masterCajaMenor.Focus();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_cpCajaMenor _cajaMenor = null;
        private int _cajaNro = 0;

        private DTO_cpLegalizaDocu _cpLegHeader = null;
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_coDocumento _coDocumento = null;
        private bool _txtNumCajaFocus;
        private bool _headerLoaded = false;
        private bool _newNroCaja = true;

        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_Legalizacion TempData
        {
            get
            {
                return (DTO_Legalizacion)this.data;
            }
            set
            {
                this.data = value;
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;
            try
            {
                validRow = base.ValidateRow(fila);
               
                if (validRow)
                {
                    #region Valida que la suma de los netos no supere al maximo permitido en valorFondo ni inferior a 0
                    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "ValorNeto"];
                    string colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();
                    string valInsufficient = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ValueFondoInsufficient);

                    decimal sumValorNeto = this.CalcularTotal();
                    if (sumValorNeto == -1)
                    {
                         this.gvDocument.SetColumnError(col, valInsufficient);
                         validRow = false;
                    }

                    this.txtValorSoportes.EditValue = sumValorNeto;
                    this.data.Header.Valor.Value = Convert.ToDecimal(sumValorNeto, CultureInfo.InvariantCulture);
                    this.txtValorEfectivo.EditValue = (data.Header.ValorFondo.Value - sumValorNeto);

                    #endregion
                }

                if (validRow)
                {
                    this.isValid = true;
                    this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.CajaMenor;
            InitializeComponent();

            this.frmModule = ModulesPrefix.cp;
            base.SetInitParameters();

            this.data = new DTO_Legalizacion();
            this._ctrl = new DTO_glDocumentoControl();

            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;

            List<DTO_glConsultaFiltro> filtrosCoDocumento = new List<DTO_glConsultaFiltro>();
            filtrosCoDocumento.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "AreaFuncionalID",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = this.areaFuncionalID
            });

            this._bc.InitMasterUC(this.masterCajaMenor, AppMasters.cpCajaMenor, true, true, true, false, filtrosCoDocumento);
            this._bc.InitMasterUC(this.masterMonedaHeader, AppMasters.glMoneda, true, true, true, false);
            this._bc.InitMasterUC(this.masterTerceroHeader, AppMasters.coTercero, true, true, true, false);
            this.masterMonedaHeader.EnableControl(false);
            this.masterTerceroHeader.EnableControl(false);

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize(); 
            this.EnableFooter(false);
            if (!base.multiMoneda)
            {
                this.txtTasaCambioActual.Visible = false;
                this.lblTasaCambioActual.Visible = false;
                base.txtTasaCambioCont.Visible = false;
                base.lblTasaCambioCont.Visible = false;
                base.txtTasaCambioDoc.Visible = false;
                base.lblTasaCambioDoc.Visible = false;
            }
            this.btnComprobante.Visible = false;
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            base.CleanHeader(basic);

            this.txtPrefix.Text = string.Empty;
            this._cajaNro = 0;
            this._coDocumento = new DTO_coDocumento();

            this.masterCajaMenor.Value = string.Empty;
            this.masterMonedaHeader.Value = string.Empty;
            this.masterTerceroHeader.Value = string.Empty;
            this.txtNumber.Text = string.Empty;
            this.dtFechaIni.DateTime = base.dtFecha.DateTime;
            this.dtFechaFin.DateTime = base.dtFecha.DateTime;
            this.txtTasaCambioActual.EditValue = 0;
            this.txtValorFondo.EditValue = 0;
            this.txtValorSoportes.EditValue = 0;
            this.txtValorEfectivo.EditValue = 0;
            this.monedaId = this.monedaLocal;
            this.btnComprobante.Enabled = false;           
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        protected override decimal CalcularTotal()
        {
            try
            {
                decimal temp = base.CalcularTotal();
                if (temp < 0 || temp > (decimal)data.Header.ValorFondo.Value.Value)
                    return -1;

                return temp;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterCajaMenor.EnableControl(enable);
            this.dtFechaIni.Enabled = enable;
            this.dtFechaFin.Enabled = enable;
            if(this.data.DocCtrl.Estado.Value != null)
                this.btnComprobante.Enabled = this.data.DocCtrl.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion ? true : false;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_Legalizacion LoadTempHeader()
        {
            this._ctrl.TerceroID.Value = this.masterTerceroHeader.Value;
            this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._ctrl.ComprobanteID.Value = this.comprobanteID;
            this._ctrl.MonedaID.Value = this.masterMonedaHeader.Value;
            this._ctrl.CuentaID.Value = this._cajaMenor.CuentaID.Value;
            this._ctrl.ProyectoID.Value = this._cajaMenor.ProyectoID.Value;
            this._ctrl.CentroCostoID.Value = this._cajaMenor.CentroCostoID.Value;
            this._ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
            this._ctrl.Fecha.Value = DateTime.Now;
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._ctrl.PrefijoID.Value = this._cajaMenor.PrefijoID.Value;
            this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.DocumentoNro.Value = Convert.ToInt32(txtNumber.Text);
            this._ctrl.DocumentoID.Value = this.documentID;
            this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.seUsuarioID.Value = this.userID;
            this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this._ctrl.ConsSaldo.Value =0;
            this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
            this._ctrl.Valor.Value = 0;
            this._ctrl.Iva.Value = 0;

            DTO_cpLegalizaDocu legHeader = new DTO_cpLegalizaDocu();
            legHeader.CajaMenorID.Value = this.masterCajaMenor.Value;
            legHeader.NumeroDoc.Value = this._ctrl.NumeroDoc.Value.Value;
            legHeader.EmpresaID.Value = this.empresaID;
            legHeader.FechaCont.Value = this.dtFecha.DateTime;
            legHeader.FechaIni.Value = this.dtFechaIni.DateTime;
            legHeader.FechaFin.Value = this.dtFechaFin.DateTime;
            legHeader.ValorFondo.Value = Convert.ToDecimal(this.txtValorFondo.EditValue, CultureInfo.InvariantCulture);
            legHeader.Valor.Value = Convert.ToDecimal(this.txtValorSoportes.EditValue, CultureInfo.InvariantCulture);
            legHeader.ValorAnticipo1.Value = 0;
            legHeader.ValorAnticipo2.Value = 0;
            legHeader.ValorAnticipo3.Value = 0;
            legHeader.ValorAnticipo4.Value = 0;
            legHeader.ValorAnticipo5.Value = 0;

            DTO_Legalizacion leg = new DTO_Legalizacion();
            leg.Header = legHeader;
            //leg.Footer = new List<DTO_cpLegalizaFooter>();
            leg.DocCtrl = this._ctrl;

            return leg;
        }

        /// <summary>
        /// Asigna la tasa de cambio
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            if (this._cajaMenor != null)
            {
                DTO_coPlanCuenta cuentaCaja = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, this._cajaMenor.CuentaID.Value, true);

                int monOr = cuentaCaja.OrigenMonetario.Value.Value;
                this._tipoMoneda = (TipoMoneda)monOr;

                if (monOr == (int)TipoMoneda.Local)
                    this.monedaId = this.monedaLocal;
                else
                    this.monedaId = this.monedaExtranjera;
                //Si la empresa no permite mmultimoneda
                if (!this.multiMoneda)
                    this.txtTasaCambioActual.EditValue = 0;
                else
                {
                    this.txtTasaCambioActual.EditValue = this.LoadTasaCambio(monOr, dtFecha.DateTime);
                    decimal tc = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);
                    if (tc == 0)
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
            }
           
            return true;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            #region Valida los datos obligatorios

            //Valida que ya este asignada una tasa de cambio
            if (!this.AsignarTasaCambio(false))
                return false;

            //Valida datos en la maestra de caja menor
            if (!this.masterCajaMenor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCajaMenor.CodeRsx);

                MessageBox.Show(msg);
                this.masterCajaMenor.Focus();

                return false;
            }
            //Valida datos en la maestra de moneda
            if (!this.masterMonedaHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterCajaMenor.Focus();

                return false;
            }
            //Valida datos en la fecha de Inicio
            if (string.IsNullOrEmpty(this.dtFechaIni.Text))
            {
                this.dtFechaIni.Focus();
                return false;
            }

            //Valida datos en la fecha de Fin
            if (string.IsNullOrEmpty(this.dtFechaFin.Text))
            {
                this.dtFechaFin.Focus();
                return false;
            }

            #endregion
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(DTO_Legalizacion leg)
        {
            DTO_glDocumentoControl ctrl = leg.DocCtrl;
            DTO_cpLegalizaDocu legHeader = leg.Header;

            if (leg.Footer == null)
                leg.Footer = new List<DTO_cpLegalizaFooter>();

            #region Trae los datos del formulario dado la caja menor
            //Trae la caja menor
            this._cajaMenor = (DTO_cpCajaMenor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, false, legHeader.CajaMenorID.Value, true);
            //Trae la info de coDocumento
            this._coDocumento = this._cajaMenor != null ? (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cajaMenor.coDocumentoID.Value, true): null;
            this.prefijoID = this._cajaMenor.PrefijoID.Value;
            base.comprobanteID = this._coDocumento != null ? this._coDocumento.ComprobanteID.Value : string.Empty;
            #endregion

            this.masterCajaMenor.Value = legHeader.CajaMenorID.Value;
            this.txtPrefix.Text = this.prefijoID;
            this.txtNumber.Text = leg.DocCtrl.DocumentoNro.Value.Value.ToString();
            this.masterMonedaHeader.Value = this._cajaMenor.MonedaID.Value;
            this.masterTerceroHeader.Value = this._cajaMenor.Responsable.Value;
            this.dtFechaIni.DateTime = legHeader.FechaIni.Value.Value;
            this.dtFechaFin.DateTime = legHeader.FechaFin.Value.Value;
            this.txtTasaCambioActual.EditValue = _ctrl.TasaCambioCONT.Value;
            this.txtValorFondo.EditValue = legHeader.ValorFondo.Value;
            this.txtValorSoportes.EditValue = legHeader.Valor.Value;
            decimal ValEfectivo = legHeader.ValorFondo.Value.Value - legHeader.Valor.Value.Value;
            this.txtValorEfectivo.EditValue = ValEfectivo;
            this.masterMoneda.Value = this._cajaMenor.MonedaID.Value;

            //Si se presenta un problema asignando la tasa de cambio lo bloquea
            if (this.ValidateHeader())
            {
                this.EnableHeader(false);
                this.data = leg;
                this.LoadData(true);
                this.validHeader = true;
                //this.gcDocument.Focus();
                this._headerLoaded = true;
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                if (this.data.Footer.Count > 0)
                    this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
            }
            else
                this.CleanHeader(true);
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            try
            {
                DTO_cpLegalizaFooter footerDet = new DTO_cpLegalizaFooter();

                #region Asigna datos a la fila
                if (this.data.Footer.Count > 0)
                {
                    footerDet.Index = this.data.Footer.Last().Index+1;
                    footerDet.Item.Value = footerDet.Index+1;
                    footerDet.EmpresaID.Value = this.empresaID;
                    footerDet.Fecha.Value = this.data.Footer.Last().Fecha.Value;
                    footerDet.MonedaID.Value = this.data.Footer.Last().MonedaID.Value;
                    footerDet.CentroCostoID.Value = this.data.Footer.Last().CentroCostoID.Value;
                    footerDet.ProyectoID.Value = this.data.Footer.Last().ProyectoID.Value;
                    footerDet.LugarGeograficoID.Value = this.data.Footer.Last().LugarGeograficoID.Value;
                }
                else
                {
                    footerDet.Index = 0;
                    footerDet.Item.Value = footerDet.Index + 1;
                    footerDet.EmpresaID.Value = this.empresaID;
                    footerDet.Fecha.Value = dtFecha.DateTime;
                    footerDet.MonedaID.Value = this._cajaMenor.MonedaID.Value;
                    footerDet.CentroCostoID.Value = this._cajaMenor.CentroCostoID.Value;
                    footerDet.ProyectoID.Value = this._cajaMenor.ProyectoID.Value;
                    footerDet.LugarGeograficoID.Value = this.defLugarGeo;                
                }
                footerDet.ValorBruto.Value = 0;
                footerDet.ValorNeto.Value = 0;
                footerDet.BaseIVA1.Value = 0;
                footerDet.BaseIVA2.Value = 0;
                footerDet.BaseRteFuente.Value = 0;
                footerDet.BaseRteIVA1.Value = 0;
                footerDet.BaseRteIVA2.Value = 0;
                footerDet.BaseRteICA.Value = 0;
                footerDet.BaseImpConsumo.Value = 0;
                footerDet.ValorIVA1.Value = 0;
                footerDet.ValorIVA2.Value = 0;              
                footerDet.ValorRteFuente.Value = 0;
                footerDet.ValorRteIVA1.Value = 0;
                footerDet.ValorRteIVA2.Value = 0;
                footerDet.ValorRteICA.Value = 0;
                footerDet.ValorImpConsumo.Value = 0;
                footerDet.PorIVA1.Value = 0;
                footerDet.PorIVA2.Value = 0;
                footerDet.PorRteFuente.Value = 0;
                footerDet.PorRteIVA1.Value = 0;
                footerDet.PorRteIVA2.Value = 0;
                footerDet.PorRteICA.Value = 0;
                footerDet.PorImpConsumo.Value = 0;
                footerDet.TasaCambioCONT.Value = Convert.ToDecimal(base.txtTasaCambioCont.EditValue, CultureInfo.InvariantCulture);
                footerDet.TasaCambioDOCU.Value = Convert.ToDecimal(base.txtTasaCambioDoc.EditValue, CultureInfo.InvariantCulture);
                footerDet.RteFteAsumidoInd.Value = false;
                footerDet.RteIVA1AsumidoInd.Value = false;
                footerDet.RteIVA2AsumidoInd.Value = false;
                footerDet.RteICAAsumidoInd.Value = false;
                footerDet.SumIVA.Value = 0;
                footerDet.SumRete.Value = 0;

                #endregion

                this.data.Footer.Add(footerDet);
                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                this.isValid = false;
                this.EnableFooter(true);
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = this.gvDocument.DataRowCount > 1 ? true : false;
                this.masterCargoEspecial.Focus();
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "AddNewRow"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtPeriod_EditValueChanged()
        {
            base.dtPeriod_EditValueChanged();

            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            //this.dtFechaIni.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFechaIni.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaIni.DateTime = new DateTime(currentYear, currentMonth, minDay);

            //this.dtFechaFin.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFechaFin.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaFin.DateTime = new DateTime(currentYear, currentMonth, minDay);

            //this.dtFechaDetalle.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFechaDetalle.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFechaDetalle.DateTime = new DateTime(currentYear, currentMonth, minDay);

            if (currentMonth > 1)
                this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth-1, minDay);
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la caja Menor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCajaMenor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCajaMenor.ValidID)
                {
                    this.newDoc = true;
                    this._cajaMenor = (DTO_cpCajaMenor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, false, this.masterCajaMenor.Value, true);
                    
                    this.masterTerceroHeader.Value = this._cajaMenor.Responsable.Value;
                    this.masterMonedaHeader.Value = this._cajaMenor.MonedaID.Value;
                    this.txtValorFondo.EditValue = this._cajaMenor.ValorFondo.Value.Value;
                    this.AsignarTasaCambio(true);
                    base.masterMoneda.Value = this._cajaMenor.MonedaID.Value;
                    base.txtPrefix.Text = this._cajaMenor.PrefijoID.Value;
                    base.txtTasaCambioCont.EditValue = this.txtTasaCambioActual.EditValue;

                    //Trae el comprobante asociado y lo asigna
                    this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cajaMenor.coDocumentoID.Value, true);
                    base.comprobanteID = this._coDocumento.PrefijoID.Value == this._cajaMenor.PrefijoID.Value ? this._coDocumento.ComprobanteID.Value : string.Empty;

                    //Verifica si la caja menor ya inicio un consecutivo
                    DTO_glDocumentoControl docCtrlExist = null;
                    int documentoNro = _bc.AdministrationModel.DocumentoNro_Get(this.documentID, this._cajaMenor.PrefijoID.Value);
                    if (documentoNro != 0)
                        docCtrlExist = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this._cajaMenor.PrefijoID.Value, documentoNro);
                    else if (documentoNro == 0)
                    {
                        documentoNro = 1;
                        docCtrlExist = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this._cajaMenor.PrefijoID.Value, documentoNro);
                    }
                    if (docCtrlExist != null)
                    {
                        #region Si existe el documento
                        DTO_Legalizacion Leg = _bc.AdministrationModel.Legalizacion_Get(docCtrlExist.NumeroDoc.Value.Value);
                        //Valida si existe
                        if (Leg != null)
                        {
                            if (this.masterCajaMenor.Value == Leg.Header.CajaMenorID.Value && Leg.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                this.data = new DTO_Legalizacion();

                                //  Valida el estado
                                if (Leg.DocCtrl.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion)
                                    this.btnComprobante.Enabled = true;
                                    //Leg.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                                else if (Leg.Header.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                                    this.txtNumber.Text = (documentoNro + 1).ToString();

                                //Carga los datos
                                this.data.DocCtrl = Leg.DocCtrl;
                                this.data.Header = Leg.Header;
                                this.data.Footer = Leg.Footer;

                                foreach (var footer in data.Footer)
                                {
                                    DTO_coTercero nombreTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, footer.TerceroID.Value, true);
                                    if (nombreTercero != null)
                                        footer.Nombre.Value = nombreTercero.Descriptivo.Value;
                                    footer.SumIVA.Value = footer.ValorIVA1.Value + footer.ValorIVA2.Value;
                                    footer.SumRete.Value = footer.ValorRteFuente.Value + footer.ValorRteIVA1.Value + footer.ValorRteICA.Value;
                                    footer.ImpRFteDesc.Value = footer.PorRteFuente.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp,AppControl.cp_CodigoReteFuente) : string.Empty;
                                    footer.ImpRIVA1Desc.Value = footer.PorRteIVA1.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA) : string.Empty;
                                    footer.ImpRIVA2Desc.Value = footer.PorRteIVA2.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA) : string.Empty;
                                    footer.ImpRICADesc.Value = footer.PorRteICA.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA) : string.Empty;
                                    footer.ImpConsumoDesc.Value = footer.PorImpConsumo.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo) : string.Empty;
                                }
                                this._ctrl = docCtrlExist;
                                this.LoadTempData(Leg);
                                this._headerLoaded = true;
                                this.newDoc = false;
                                this._newNroCaja = false;
                                if (this._ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado && this.data.Footer.Count > 0)
                                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = true;
                            }
                            else
                                this.txtNumber.Text = (documentoNro + 1).ToString();
                        }
                        #endregion
                    }
                    else
                        this.txtNumber.Text = "1";

                    if (this._newNroCaja)
                    {
                        this._ctrl.ComprobanteID.Value = this.comprobanteID;
                        this._ctrl.DocumentoID.Value = this.documentID;
                        this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                        this._ctrl.PrefijoID.Value = this._cajaMenor.PrefijoID.Value;
                        this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                        this._ctrl.seUsuarioID.Value = this.userID;
                        this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        this._ctrl.ConsSaldo.Value = 0;
                        this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    }
                   // this.gvDocument.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "masterCajaMenor_Leave"));
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
        /// valida la edición de las fechas de Inicio y Fin 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechaInicioFin_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                DateEdit dt = (DateEdit)sender;
                if (this.dtFechaFin.DateTime < dtFechaIni.DateTime)
                    this.dtFechaFin.DateTime = dtFechaIni.DateTime;
                
                int currentMonth = this.dtPeriod.DateTime.Month;
                int currentYear = this.dtPeriod.DateTime.Year;
                int minDay = dtFechaIni.DateTime.Day;
                int lastDay = dtFechaFin.DateTime.Day;

                //this.dtFechaDetalle.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                this.dtFechaDetalle.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                this.dtFechaDetalle.DateTime = new DateTime(currentYear, currentMonth, minDay);
            }
            catch (Exception)
            { ; }

        }

        /// <summary>
        /// Genera la vista del comprobante de contabilizacion
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                if (this.data != null)
                {
                    if (!this.data.Header.NumeroDocCXP.Value.HasValue)
                    {
                        result = _bc.AdministrationModel.Legalizacion_ComprobantePreAdd(this.documentID, this.data);
                        MessageForm msg = new MessageForm(result);
                        msg.Show();
                    }
                    else
                        MessageBox.Show("El comprobante preliminar de la Cuenta por Pagar ya existe"); 
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
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
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_Legalizacion leg = this.LoadTempHeader();
                        this._cpLegHeader = leg.Header;
                        this.TempData = leg;
                        FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                        if (this.data.Footer.Count == 0)
                        {
                            FormProvider.Master.itemSendtoAppr.Enabled = false;
                            FormProvider.Master.itemPrint.Enabled = false;

                            this.LoadData(true);
                        }

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                    else
                    {
                        if (!this._newNroCaja && !newReg)
                        {
                            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr); 
                            FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                        }                    
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "grlDocument_Enter" + ex.Message));
                }
                #endregion
                #region Si ya tiene datos cargados
                if (!this.dataLoaded)
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                #endregion
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
                this.masterCajaMenor.Focus();
            else if (gvDocument.RowCount > 0)
                this.masterCargoEspecial.Focus();
        }

        /// <summary>
        /// Evento que se ejecuta al salir del campo de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void textEditControl_Leave(object sender, EventArgs e)
        {
            base.textEditControl_Leave(sender, e);
            this.ValidateRow(gvDocument.FocusedRowHandle);
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
                    this.data = new DTO_Legalizacion();
                    this._cajaMenor = null;
                    this._cpLegHeader = new DTO_cpLegalizaDocu();
                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this.EnableHeader(true);
                    this._headerLoaded = false;
                    this._newNroCaja = true;
                    this.masterCajaMenor.Focus();   
                }
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
                base.TBSave();
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
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

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            Thread process = new Thread(this.SendToApproveThread);
            process.Start();
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;
                if (this._newNroCaja)
                    result = _bc.AdministrationModel.Legalizacion_Add(this.documentID, this.data,false);
                else
                {
                    this.data.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    result = _bc.AdministrationModel.Legalizacion_Update(this.documentID, this.data.Footer, this.data.Header);
                }
             
                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    #region Genera Reporte
                    string reportName = this._bc.AdministrationModel.Report_Cp_CajaMenor(this._ctrl.NumeroDoc.Value.Value, string.Empty, null, true);
                    //string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this._ctrl.NumeroDoc.Value.Value, null, reportName.ToString());
                    //Process.Start(fileURl);

                    #endregion

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_Legalizacion();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread() 
        {
            try
            {
                int numeroDoc = this.data.Header.NumeroDoc.Value.Value;
                if (numeroDoc == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                    return;
                }

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject result = _bc.AdministrationModel.Legalizacion_SendToAprob(this.documentID, numeroDoc, true);
              
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_Legalizacion();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CajaMenor.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion
    }
}
