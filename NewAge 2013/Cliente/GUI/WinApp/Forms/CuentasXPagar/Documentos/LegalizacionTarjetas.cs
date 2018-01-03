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
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class LegalizacionTarjetas : DocumentLegalizaForm
    {
        public LegalizacionTarjetas()
        {
            //this.InitializeComponent();
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
            this.masterTarjetaCredito.Focus();
            FormProvider.Master.itemSendtoAppr.Enabled = false;
            this._canSendApprove = true;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.gcDocument.DataSource = this.data.Footer;
            FormProvider.Master.itemSendtoAppr.Enabled = false;

            this.CleanHeader(true);
            this.EnableHeader(true);
            this.EnableFooter(false);
            this.masterTarjetaCredito.Focus();
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //private DTO_cpCajaMenor _cajaMenor = null;
        private int _cajaNro = 0;

        private DTO_cpLegalizaDocu _cpLegHeader = null;
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_coDocumento _coDocumento = null;
        private DTO_coTercero terceroID = null;
        private bool _txtNumCajaFocus;
        private bool _headerLoaded = false;
        private bool _newNroCaja = true;
        private bool _legalValid = true;

        private List<DTO_AnticiposResumen> _anticipos;
        private List<DTO_ComprobanteFooter> _anticiposComp;
        //Variables por defecto
        private string proyectoxDef = string.Empty;
        private string centroCtoxDef = string.Empty;
        private string cuentaxDef = string.Empty;

        //Variable para reporte
        private string reportName;
        private string fileURl;
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
                        this._legalValid = false;
                    else
                        this._legalValid = true;
                    this.data.Header.Valor.Value = Convert.ToDecimal(sumValorNeto, CultureInfo.InvariantCulture);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionGastos.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.LegalizaTarjeta;
            InitializeComponent();

            this.frmModule = ModulesPrefix.cp;
            base.SetInitParameters();

            this.data = new DTO_Legalizacion();
            this._ctrl = new DTO_glDocumentoControl();
            this._anticipos = new List<DTO_AnticiposResumen>();
            this._anticiposComp = new List<DTO_ComprobanteFooter>();

            //Obtiene  valores por defecto
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this._tipoMonedaOr = TipoMoneda_LocExt.Local;
            this.proyectoxDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.centroCtoxDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.cuentaxDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CtaXDefecto);

            _bc.InitMasterUC(this.masterMonedaHeader, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterTarjetaCredito, AppMasters.cpTarjetaCredito, true, true, true, false);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            //this.btnRelacionAnticipo.Enabled = false;
            this.EnableFooter(false);
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
            this._anticipos = new List<DTO_AnticiposResumen>();
            this._anticiposComp = new List<DTO_ComprobanteFooter>();
            this.masterMonedaHeader.Value = this.monedaLocal;
            this.masterTarjetaCredito.Value = string.Empty;
            this.txtNumber.Text = string.Empty;
            this.dtFechaIni.DateTime = base.dtFecha.DateTime;
            this.dtFechaFin.DateTime = base.dtFecha.DateTime;
            this.txtValorAnticipos.EditValue = 0;
            this.txtValorLegal.EditValue = 0;
            //this.txtValorReintegro.EditValue = 0;
            this.txtValorNeto.EditValue = 0;
            this.txtValorNeto.EditValue = 0;
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
                decimal valorNeto = 0;
                decimal temp = base.CalcularTotal();
                this.txtValorLegal.EditValue = temp;
                valorNeto = temp - Convert.ToDecimal(this.txtValorAnticipos.EditValue, CultureInfo.InvariantCulture);//+ Convert.ToDecimal(this.txtValorReintegro.EditValue);

                if (valorNeto < 0)
                    return -1;
                else
                    this.txtValorNeto.EditValue = valorNeto;

                return valorNeto;
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
            this.dtFechaIni.Enabled = enable;
            this.dtFechaFin.Enabled = enable;
            this.masterTarjetaCredito.EnableControl(enable);
            this.masterMonedaHeader.EnableControl(enable);
            if (this.data.DocCtrl.Estado.Value != null)
                this.btnComprobante.Enabled = this.data.DocCtrl.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion ? true : false;
            //this.btnRelacionAnticipo.Enabled = enable ? false : true;
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_Legalizacion LoadTempHeader()
        {
            this._ctrl.TerceroID.Value = this.terceroID.ID.Value;
            this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            this._ctrl.ComprobanteID.Value = this.comprobanteID;
            this._ctrl.MonedaID.Value = this.masterMonedaHeader.Value;
            this._ctrl.CuentaID.Value = this.cuentaxDef;
            this._ctrl.ProyectoID.Value = this.proyectoxDef;
            this._ctrl.CentroCostoID.Value = this.centroCtoxDef;
            this._ctrl.LugarGeograficoID.Value = this.masterLugarGeo.Value;
            this._ctrl.Fecha.Value = DateTime.Now;
            this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
            this._ctrl.PrefijoID.Value = this.prefijoID;
            this._ctrl.TasaCambioCONT.Value = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.TasaCambioDOCU.Value = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);
            this._ctrl.DocumentoNro.Value = Convert.ToInt32(txtNumber.Text);
            this._ctrl.DocumentoID.Value = this.documentID;
            this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
            this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
            this._ctrl.seUsuarioID.Value = this.userID;
            this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
            this._ctrl.ConsSaldo.Value = 0;
            this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
            this._ctrl.FechaDoc.Value = this.dtFecha.DateTime;
            this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
            this._ctrl.Valor.Value = 0;
            this._ctrl.Iva.Value = 0;

            DTO_cpLegalizaDocu legHeader = new DTO_cpLegalizaDocu();
            legHeader.NumeroDoc.Value = this._ctrl.NumeroDoc.Value.Value;
            legHeader.EmpresaID.Value = this.empresaID;
            legHeader.FechaCont.Value = this.dtFecha.DateTime;
            legHeader.FechaIni.Value = this.dtFechaIni.DateTime;
            legHeader.FechaFin.Value = this.dtFechaFin.DateTime;
            legHeader.TarjetaCreditoID.Value = this.masterTarjetaCredito.Value;
            //legHeader.ValorFondo.Value = Convert.ToDecimal(this.txtValorAnticipos.EditValue, CultureInfo.InvariantCulture);
            legHeader.Valor.Value = Convert.ToDecimal(this.txtValorNeto.EditValue, CultureInfo.InvariantCulture);
            legHeader.IdentificadorAnt1.Value = this.data.Header.IdentificadorAnt1.Value != null ? this.data.Header.IdentificadorAnt1.Value : null;
            legHeader.IdentificadorAnt2.Value = this.data.Header.IdentificadorAnt2.Value != null ? this.data.Header.IdentificadorAnt2.Value : null;
            legHeader.IdentificadorAnt3.Value = this.data.Header.IdentificadorAnt3.Value != null ? this.data.Header.IdentificadorAnt3.Value : null;
            legHeader.IdentificadorAnt4.Value = this.data.Header.IdentificadorAnt4.Value != null ? this.data.Header.IdentificadorAnt4.Value : null;
            legHeader.IdentificadorAnt5.Value = this.data.Header.IdentificadorAnt5.Value != null ? this.data.Header.IdentificadorAnt5.Value : null;
            legHeader.ValorAnticipo1.Value = this.data.Header.ValorAnticipo1.Value != null ? this.data.Header.ValorAnticipo1.Value : 0;
            legHeader.ValorAnticipo2.Value = this.data.Header.ValorAnticipo2.Value != null ? this.data.Header.ValorAnticipo2.Value : 0;
            legHeader.ValorAnticipo3.Value = this.data.Header.ValorAnticipo3.Value != null ? this.data.Header.ValorAnticipo3.Value : 0;
            legHeader.ValorAnticipo4.Value = this.data.Header.ValorAnticipo4.Value != null ? this.data.Header.ValorAnticipo4.Value : 0;
            legHeader.ValorAnticipo5.Value = this.data.Header.ValorAnticipo5.Value != null ? this.data.Header.ValorAnticipo5.Value : 0;

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
            this._tipoMonedaOr = this.masterMonedaHeader.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
            int monOr = (int)this._tipoMonedaOr;
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

            //Valida datos en la maestra tercero
            if (!this.masterTarjetaCredito.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTarjetaCredito.CodeRsx);

                MessageBox.Show(msg);
                this.masterTarjetaCredito.Focus();

                return false;
            }

            //Valida datos en la maestra de moneda
            if (!this.masterMonedaHeader.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterMonedaHeader.CodeRsx);

                MessageBox.Show(msg);
                this.masterMonedaHeader.Focus();

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
            this._anticipos = new List<DTO_AnticiposResumen>();

            if (leg.Footer == null)
                leg.Footer = new List<DTO_cpLegalizaFooter>();

            #region Trae los datos del formulario dado la caja menor
            //Trae la caja menor
            //this._cajaMenor = (DTO_cpCajaMenor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, false, legHeader.CajaMenorID.Value, true);
            //Trae la info de coDocumento
            //this._coDocumento = this._cajaMenor != null ? (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._cajaMenor.coDocumentoID.Value, true) : null;
            //this.prefijoID = this._cajaMenor.PrefijoID.Value;
            //base.comprobanteID = this._coDocumento != null ? this._coDocumento.ComprobanteID.Value : string.Empty;
            #endregion

            this._tipoMonedaOr = ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;

            this.txtPrefix.Text = this.prefijoID;
            this.txtNumber.Text = leg.DocCtrl.DocumentoNro.Value.Value.ToString();
            this.masterMonedaHeader.Value = this.monedaLocal;
            this.masterTarjetaCredito.Value = leg.Header.TarjetaCreditoID.Value;
            this.dtFechaIni.DateTime = legHeader.FechaIni.Value.Value;
            this.dtFechaFin.DateTime = legHeader.FechaFin.Value.Value;
            this.txtTasaCambioActual.EditValue = _ctrl.TasaCambioCONT.Value;
            this.txtTasaCambioCont.EditValue = _ctrl.TasaCambioCONT.Value;
            this.txtValorAnticipos.EditValue = legHeader.ValorAnticipo1.Value.Value + legHeader.ValorAnticipo2.Value.Value + legHeader.ValorAnticipo3.Value.Value + legHeader.ValorAnticipo4.Value.Value + legHeader.ValorAnticipo5.Value.Value;
            this.txtValorNeto.EditValue = legHeader.Valor.Value;

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
                {
                    this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = true;
                }
                else
                {
                    this.CleanHeader(true);
                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = false;
                }
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
                this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth - 1, minDay);
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir del detalle de la caja Menor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterTarjeta_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterTarjetaCredito.ValidID)
                {
                    this.newDoc = true;

                    this.masterMonedaHeader.Value = this.monedaLocal;
                    this.AsignarTasaCambio(true);
                    base.txtPrefix.Text = this.prefijoID;
                    base.txtTasaCambioCont.EditValue = this.txtTasaCambioActual.EditValue;
                    //this._cpLegHeader.TarjetaCreditoID.Value = this.masterTarjetaCredito.Value;
                    // Trae el comprobante asociado y lo asigna
                    string coDocxDefecto = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoDocContableLegalizacionGastos);
                    this._coDocumento = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, coDocxDefecto, true);
                    base.comprobanteID = this._coDocumento.ComprobanteID.Value;
                    ////////////

                    DTO_cpTarjetaCredito tarjeta = (DTO_cpTarjetaCredito)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpTarjetaCredito, false, this.masterTarjetaCredito.Value, true);
                    this.terceroID = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, tarjeta.TerceroID.Value, true);

                    List<DTO_AnticiposResumen> anticipos = _bc.AdministrationModel.cpAnticipos_GetResumen(this.dtPeriod.DateTime, this._tipoMoneda, this.terceroID.ID.Value, true);
                    List<DTO_AnticiposResumen> newData = new List<DTO_AnticiposResumen>();

                    #region Carga la nueva lista de anticipos seleccionados
                    List<DTO_AnticiposResumen> emptyList = new List<DTO_AnticiposResumen>();
                    anticipos.ForEach(retList =>
                    {
                        retList.MaxVal.Value = this._tipoMoneda == TipoMoneda.Local ? retList.ML.Value : retList.ME.Value;
                        bool finded = false;
                        this._anticipos.ForEach(currAnt =>
                        {
                            if
                            (
                                !finded &&
                                retList.CuentaID.Value == currAnt.CuentaID.Value &&
                                retList.TerceroID.Value == currAnt.TerceroID.Value &&
                                retList.ProyectoID.Value == currAnt.ProyectoID.Value &&
                                retList.CentroCostoID.Value == currAnt.CentroCostoID.Value &&
                                retList.LineaPresupuestoID.Value == currAnt.LineaPresupuestoID.Value &&
                                retList.ConceptoSaldoID.Value == currAnt.ConceptoSaldoID.Value &&
                                retList.ConceptoCargoID.Value == currAnt.ConceptoCargoID.Value &&
                                retList.IdentificadorTR.Value == currAnt.IdentificadorTR.Value
                            )
                            {
                                currAnt.MaxVal.Value = retList.MaxVal.Value;
                                emptyList.Add(currAnt);
                                finded = true;
                            }
                        });
                        if (!finded)
                            emptyList.Add(retList);
                    });

                    this._anticipos = emptyList;
                    #endregion
                    #region Elimina los anticipos existentes
                    this._anticiposComp.Clear();
                    #endregion
                    #region Agrega los registros de los anticipos nuevamente
                    int index = 1;
                    this._anticipos.ForEach(newAnt =>
                    {
                        DTO_ComprobanteFooter newDet = new DTO_ComprobanteFooter();
                        newDet.Index = index;
                        newDet.CuentaID.Value = newAnt.CuentaID.Value;
                        newDet.TerceroID.Value = newAnt.TerceroID.Value;
                        newDet.ProyectoID.Value = newAnt.ProyectoID.Value;
                        newDet.CentroCostoID.Value = newAnt.CentroCostoID.Value;
                        newDet.LineaPresupuestoID.Value = newAnt.LineaPresupuestoID.Value;
                        newDet.ConceptoCargoID.Value = newAnt.ConceptoCargoID.Value;
                        newDet.LugarGeograficoID.Value = this.defLugarGeo;
                        newDet.PrefijoCOM.Value = newAnt.PrefijoID.Value;
                        newDet.DocumentoCOM.Value = newAnt.DocumentoTercero.Value;
                        newDet.ActivoCOM.Value = string.Empty;
                        newDet.ConceptoSaldoID.Value = newAnt.ConceptoSaldoID.Value;
                        newDet.IdentificadorTR.Value = newAnt.IdentificadorTR.Value;
                        newDet.Descriptivo.Value = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoAbona);
                        newDet.TasaCambio.Value = Convert.ToDecimal(this.txtTasaCambioActual.EditValue, CultureInfo.InvariantCulture);

                        newDet.vlrBaseML.Value = 0;
                        newDet.vlrBaseME.Value = 0;
                        newDet.vlrMdaLoc.Value = newAnt.ML.Value;
                        newDet.vlrMdaExt.Value = newAnt.ME.Value;
                        newDet.vlrMdaOtr.Value = this.monedaId == this.monedaLocal ? newDet.vlrMdaLoc.Value : newDet.vlrMdaExt.Value;
                        newDet.DatoAdd3.Value = newAnt.MaxVal.Value.Value.ToString();
                        newDet.DatoAdd4.Value = AuxiliarDatoAdd4.Anticipo.ToString();

                        this._anticiposComp.Add(newDet);
                        index++;
                    });
                    if (_anticiposComp.Count() > 5)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticiposCountInvalid));
                        this.txtValorAnticipos.EditValue = 0;
                        return;
                    }

                    #endregion
                    #region Asigna los anticipos al Header
                    this.data.Header.IdentificadorAnt1.Value = null;
                    this.data.Header.IdentificadorAnt2.Value = null;
                    this.data.Header.IdentificadorAnt3.Value = null;
                    this.data.Header.IdentificadorAnt4.Value = null;
                    this.data.Header.IdentificadorAnt5.Value = null;
                    this.data.Header.ValorAnticipo1.Value = null;
                    this.data.Header.ValorAnticipo2.Value = null;
                    this.data.Header.ValorAnticipo3.Value = null;
                    this.data.Header.ValorAnticipo4.Value = null;
                    this.data.Header.ValorAnticipo5.Value = null;
                    if (this._anticipos.Count > 0)
                    {
                        this.data.Header.IdentificadorAnt1.Value = this._anticipos[0].IdentificadorTR.Value;
                        if (this.monedaId == this.monedaLocal)
                            this.data.Header.ValorAnticipo1.Value = this._anticipos[0].ML.Value;
                        else
                            this.data.Header.ValorAnticipo1.Value = this._anticipos[0].ME.Value;

                        if (this._anticipos.Count > 1)
                        {
                            this.data.Header.IdentificadorAnt2.Value = this._anticipos[1].IdentificadorTR.Value;
                            if (this.monedaId == this.monedaLocal)
                                this.data.Header.ValorAnticipo2.Value = this._anticipos[1].ML.Value;
                            else
                                this.data.Header.ValorAnticipo2.Value = this._anticipos[1].ME.Value;

                            if (this._anticipos.Count > 2)
                            {
                                this.data.Header.IdentificadorAnt3.Value = this._anticipos[2].IdentificadorTR.Value;
                                if (this.monedaId == this.monedaLocal)
                                    this.data.Header.ValorAnticipo3.Value = this._anticipos[2].ML.Value;
                                else
                                    this.data.Header.ValorAnticipo3.Value = this._anticipos[2].ME.Value;

                                if (this._anticipos.Count > 3)
                                {
                                    this.data.Header.IdentificadorAnt4.Value = this._anticipos[3].IdentificadorTR.Value;
                                    if (this.monedaId == this.monedaLocal)
                                        this.data.Header.ValorAnticipo4.Value = this._anticipos[3].ML.Value;
                                    else
                                        this.data.Header.ValorAnticipo4.Value = this._anticipos[3].ME.Value;

                                    if (this._anticipos.Count > 4)
                                    {
                                        this.data.Header.IdentificadorAnt5.Value = this._anticipos[4].IdentificadorTR.Value;
                                        if (this.monedaId == this.monedaLocal)
                                            this.data.Header.ValorAnticipo5.Value = this._anticipos[4].ML.Value;
                                        else
                                            this.data.Header.ValorAnticipo5.Value = this._anticipos[4].ME.Value;
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                    #region Realiza la sumatoria de los anticipos
                    decimal sum = 0;
                    foreach (var anticipo in this._anticipos)
                    {
                        if (this.monedaId == this.monedaLocal)
                            sum += Convert.ToDecimal(anticipo.ML.Value, CultureInfo.InvariantCulture);
                        else
                            sum += Convert.ToDecimal(anticipo.ME.Value, CultureInfo.InvariantCulture);
                    }
                    this.txtValorAnticipos.EditValue = sum;
                    if (this.data.Footer.Count > 0)
                        this.ValidateRow(this.NumFila);
                    #endregion



                    ///////////////
                    //Verifica si la tarjeta ya inicio un consecutivo
                    DTO_glDocumentoControl docCtrlExist = null;
                    int documentoNro = _bc.AdministrationModel.DocumentoNro_Get(this.documentID, this.prefijoID);
                    if (documentoNro != 0)
                        docCtrlExist = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this.prefijoID, documentoNro);
                    else if (documentoNro == 0)
                    {
                        documentoNro = 1;
                        docCtrlExist = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(this.documentID, this.prefijoID, documentoNro);
                    }
                    if (docCtrlExist != null)
                    {
                        #region Si existe el documento
                        DTO_Legalizacion Leg = _bc.AdministrationModel.Legalizacion_Get(docCtrlExist.NumeroDoc.Value.Value);
                        //Valida si existe
                        if (Leg != null)
                            if (this.terceroID.ID.Value == Leg.DocCtrl.TerceroID.Value && Leg.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                this.data = new DTO_Legalizacion();

                                //  Valida el estado
                                if (Leg.Header.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion)
                                {
                                    this.btnComprobante.Enabled = true;
                                    Leg.Header.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                                    this.masterTarjetaCredito.Focus();
                                }
                                else if (Leg.Header.Estado.Value.Value == (byte)EstadoDocControl.Aprobado)
                                {
                                    this.txtNumber.Text = (documentoNro + 1).ToString();
                                }

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
                                    footer.ImpRFteDesc.Value = footer.PorRteFuente.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente) : string.Empty;
                                    footer.ImpRIVA1Desc.Value = footer.PorRteIVA1.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA) : string.Empty;
                                    footer.ImpRIVA2Desc.Value = footer.PorRteIVA2.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA) : string.Empty;
                                    footer.ImpRICADesc.Value = footer.PorRteICA.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA) : string.Empty;
                                    footer.ImpConsumoDesc.Value = footer.PorImpConsumo.Value != 0 ? _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo) : string.Empty;
                                }

                                this._ctrl = docCtrlExist;
                                LoadTempData(Leg);
                                this._headerLoaded = true;
                                this.newDoc = false;
                                this._newNroCaja = false;
                                if (this._ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado && this.data.Footer.Count > 0)
                                    base.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Enabled = true;
                            }
                            else
                            {
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
                        this._ctrl.PrefijoID.Value = this.prefijoID;
                        this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                        this._ctrl.seUsuarioID.Value = this.userID;
                        this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                        this._ctrl.ConsSaldo.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionGastos.cs", "CajaMenor.cs-masterCajaMenor_Leave"));
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

                this.dtFechaDetalle.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
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
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
                //FormProvider.Master.itemPrint.Enabled = false;

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
                        this.btnRelacionAnticipo.Enabled = true;
                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                    //else
                    //{
                    //    FormProvider.Master.itemSendtoAppr.Enabled = true;
                    //    FormProvider.Master.itemPrint.Enabled = true;
                    //}

                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionGastos.cs", "grlDocument_Enter" + ex.Message));
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
                this.masterTarjetaCredito.Focus();
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
            try
            {
                base.textEditControl_Leave(sender, e);
                decimal valorLegalizado = 0;
                foreach (var item in this.data.Footer)
                {
                    valorLegalizado += item.ValorNeto.Value.Value;
                }
                //decimal valorAnticipos = Convert.ToDecimal(this.txtValorAnticipos.EditValue);
                //if (valorLegalizado <= valorAnticipos)
                //{
                this.txtValorLegal.EditValue = valorLegalizado;
                this.txtValorNeto.EditValue = valorLegalizado - Convert.ToDecimal(this.txtValorAnticipos.EditValue, CultureInfo.InvariantCulture);// +Convert.ToDecimal(this.txtValorReintegro.EditValue);
                if (this.data.Footer.Count > 0)
                    this.ValidateRow(this.NumFila);
                //}
                //else
                //{
                //    base.txtValor.EditValue = 0;
                //    base.txtValor.Focus();
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_ValueInvalid));
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionTarjetas.cs", "textEditControl_Leave"));
            }
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
                    // this._cajaMenor = null;
                    this._cpLegHeader = new DTO_cpLegalizaDocu();
                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this.EnableHeader(true);
                    this._headerLoaded = false;
                    this._newNroCaja = true;
                    this.masterTarjetaCredito.Focus();
                    this.btnRelacionAnticipo.Enabled = false;
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
                //if (!this._legalValid) // SE QUITO POR LA VALIDACION DE ANTICIPOS
                //{
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Cp_ValueLegalInvalid));
                //    return;
                //}
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

                //if (this.data.Header.Valor.Value == 0)
                //{
                if (this._newNroCaja)
                    result = _bc.AdministrationModel.Legalizacion_Add(this.documentID, this.data, false);
                else
                {
                    this.data.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    result = _bc.AdministrationModel.Legalizacion_Update(this.documentID, this.data.Footer, this.data.Header);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_Legalizacion();
                    this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
                //}
                //else
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "El valor legalizado debe ser el mismo del anticipo generado por la tarjeta" ));

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionGastos.cs", "SaveThread"));
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
                FormProvider.Master.StopProgressBarThread(this.documentID);
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

                #region Devuelve la URL del Reporte

                reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_LegalizaTarjetas(numeroDoc, ExportFormatType.pdf);
                fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                Process.Start(fileURl);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LegalizacionGastos.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
