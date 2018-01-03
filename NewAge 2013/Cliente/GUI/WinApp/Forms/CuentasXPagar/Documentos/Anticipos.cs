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
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList.Data;
using NewAge.Librerias.Project;
using System.Text.RegularExpressions;
using System.Threading;
using NewAge.DTO.Reportes;
using NewAge.ReportesComunes;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors;
using System.Globalization;
using DevExpress.XtraReports.UI;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Anticipos : DocumentForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _dtoCtrl;
        private DTO_cpAnticipo _anticipo;
        private DTO_cpAnticipoTipo _tipoAnticipo;
        private DTO_coPlanCuenta _cta;
        private int _tipoMoneda;
        private DTO_Comprobante _comprobante;
        private DTO_coDocumento _documento;
        private int _compNro;
        private bool _isFistLoad = true;
        private int _numDocumento;
        private string _message;
        private decimal _tc;
        private bool _dataLoadedInd = false;
        private bool _nuevoDoc = true;

        private decimal _gAlojamiento;
        private decimal _gAlimentacion;
        private decimal _gTransporte;
        private decimal _gOtrosGastos;
        private decimal _gTiquetes;

        private string _defProy;
        private string _defCtoCosto;

        private string monedaLoc;
        private string monedaExtranjera;

        private string consecutivoAnticipo;
        //Variables de Reportes
        private string reportName;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.RefreshDocument(true);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        public void InitControls()
        {
            TablesResources.GetTableResources(this.cmbMoneda, typeof(TipoMoneda_LocExt));
            TablesResources.GetTableResources(this.cmbTipoViaje, typeof(TipoViaje));
            _bc.InitMasterUC(this.uc_MasterTercero, AppMasters.coTercero, true, true, true, false);
            _bc.InitMasterUC(this.uc_MasterTipoAnticipo, AppMasters.cpAnticipoTipo, true, true, true, false);
            _bc.InitMasterUC(this.uc_MasterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
            _bc.InitMasterUC(this.uc_MasterProyecto, AppMasters.coProyecto, true, true, true, false);

            this._defProy = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this._defCtoCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

            this._tipoMoneda = -1;
            this._tipoAnticipo = new DTO_cpAnticipoTipo();
        }

        /// <summary>
        /// Carga la informacion del glDocumentoControl y el Anticipo en los controles
        /// </summary>
        private void LoadDocumentInfo()
        {
            try
            {
                this.dtFecha.DateTime = this._anticipo.RadicaFecha.Value.Value;
                this.uc_MasterTipoAnticipo.Value = this._anticipo.AnticipoTipoID.Value;
                this.txtPlazo.Text = this._anticipo.Plazo.Value.ToString();
                this.txtValor.EditValue = this._anticipo.Valor.Value != null ? this._anticipo.Valor.Value : 0;
                this.txtDescripcion.Text = this._dtoCtrl.Observacion.Value;
                this.uc_MasterCentroCosto.Value = this._dtoCtrl.CentroCostoID.Value;
                this.uc_MasterProyecto.Value = this._dtoCtrl.ProyectoID.Value;
                TipoMoneda_LocExt tm = this._dtoCtrl.MonedaID.Value == this.monedaLoc ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                string tmVal = ((int)tm).ToString();
                this.cmbMoneda.SelectedItem = this.cmbMoneda.GetItem(tmVal);

                if (this._dtoCtrl.Estado.Value == (byte)EstadoDocControl.ParaAprobacion)
                {
                    this.dtFecha.Enabled = false;
                    this.uc_MasterTipoAnticipo.EnableControl(false);
                    this.txtPlazo.Enabled = false;
                    this.txtValor.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.uc_MasterCentroCosto.Enabled = false;
                    this.uc_MasterProyecto.Enabled = false;
                    this.txtValor.Enabled = false;
                    this.uc_MasterTercero.EnableControl(false);
                }

                this.ValidateCtaValues();
                if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && this._tipoAnticipo.GastosViajeInd.Value.Value)
                {
                    this.cmbTipoViaje.SelectedValue = this._anticipo.TipoViaje.Value;
                    this.txtDiasGAlimentacion.EditValue = this._anticipo.DiasAlimentacion.Value ?? 0;
                    this.txtDiasGAlojamiento.EditValue = this._anticipo.DiasAlojamiento.Value ?? 0;
                    this.txtDiasGTransporte.EditValue = this._anticipo.DiasTransporte.Value ?? 0;
                    this.txtDiasOtrosGastos.EditValue = this._anticipo.DiasOtrosGastos.Value ?? 0;
                    this.txtVDGAlimentacion.EditValue = this._anticipo.ValorAlimentacion.Value ?? 0;
                    this.txtVDGAlojamiento.EditValue = this._anticipo.ValorAlojamiento.Value ?? 0;
                    this.txtVDGTransporte.EditValue = this._anticipo.ValorTransporte.Value ?? 0;
                    this.txtVDOtrosGastos.EditValue = this._anticipo.ValorOtrosGastos.Value ?? 0;
                    this.txtValorTiquetes.EditValue = this._anticipo.ValorTiquetes.Value ?? 0;

                    this.txtVTGAlimentacion.EditValue = this._anticipo.DiasAlimentacion.Value * this._anticipo.ValorAlimentacion.Value;
                    this.txtVTGAlojamiento.EditValue = this._anticipo.DiasAlojamiento.Value * this._anticipo.ValorAlojamiento.Value;
                    this.txtVTGTransporte.EditValue = this._anticipo.DiasTransporte.Value * this._anticipo.ValorTransporte.Value;
                    this.txtVTOtrosGastos.EditValue = this._anticipo.DiasOtrosGastos.Value * this._anticipo.ValorOtrosGastos.Value;
                    this.gbGridDocument.Enabled = true;
                    this.txtValor.Enabled = false;

                    this.dtViajeFin.DateTime = this._anticipo.FechaRetorno.Value.Value;
                    this.dtViajeInicio.DateTime = this._anticipo.FechaSalida.Value.Value;
                }
                else
                {
                    this.cmbTipoViaje.Text = string.Empty;
                    this.txtDiasGAlimentacion.EditValue = 0;
                    this.txtDiasGAlojamiento.EditValue = 0;
                    this.txtDiasGTransporte.EditValue = 0;
                    this.txtDiasOtrosGastos.EditValue = 0;
                    this.txtVDGAlimentacion.EditValue = 0;
                    this.txtVDGAlojamiento.EditValue = 0;
                    this.txtVDGTransporte.EditValue = 0;
                    this.txtVDOtrosGastos.EditValue = 0;
                    this.txtValorTiquetes.EditValue = 0;

                    this.txtVTGAlimentacion.EditValue = 0;
                    this.txtVTGAlojamiento.EditValue = 0;
                    this.txtVTGTransporte.EditValue = 0;
                    this.txtVTOtrosGastos.EditValue = 0;
                    this.gbGridDocument.Enabled = false;
                    this.txtValor.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "LoadDocumentInfo"));
            }

            this._dataLoadedInd = true;
        }

        /// <summary>
        /// Recarga los campos del formulario
        /// </summary>
        private void RefreshDocument(bool allData)
        {
            if (allData)
            {
                this.uc_MasterTercero.Value = string.Empty;
                this.uc_MasterTipoAnticipo.Value = string.Empty;
                this.txtDocumentoTercero.Text = string.Empty;
                this.cmbTipoViaje.SelectedIndex = 0;
                this.cmbMoneda.SelectedIndex = 0;
            }

            this._nuevoDoc = true;
            this.txtDescripcion.Text = string.Empty;
            this.txtPlazo.Text = string.Empty;
            this.txtValor.EditValue = 0;
            this.uc_MasterCentroCosto.Value = string.Empty;
            this.uc_MasterProyecto.Value = string.Empty;
            this.uc_MasterProyecto.EnableControl(true);
            this.uc_MasterCentroCosto.EnableControl(true);
            this.uc_MasterTercero.EnableControl(true);
            this.txtDescripcion.Enabled =true;
            FormProvider.Master.itemPrint.Enabled = false;

            //Campo adicionales para control de viajes
            this.txtDiasGAlimentacion.EditValue = 0;
            this.txtDiasGAlojamiento.EditValue = 0;
            this.txtDiasGTransporte.EditValue = 0;
            this.txtDiasOtrosGastos.EditValue = 0;
            this.txtVDGAlojamiento.EditValue = 0;
            this.txtVDGAlimentacion.EditValue = 0;
            this.txtVDGTransporte.EditValue = 0;
            this.txtVDOtrosGastos.EditValue = 0;
            this.txtValorTiquetes.EditValue = 0;
            this.txtVTGAlimentacion.EditValue = 0;
            this.txtVTGAlojamiento.EditValue = 0;
            this.txtVTOtrosGastos.EditValue = 0;
            this.txtVTGTransporte.EditValue = 0;
            this.dtViajeFin.DateTime = DateTime.Now;
            this.dtViajeInicio.DateTime = DateTime.Now;
            this.gbGridDocument.Enabled = false;

            this._tipoAnticipo = new DTO_cpAnticipoTipo();
            this._dataLoadedInd = false;
            this.uc_MasterCentroCosto.Focus();

        }

        /// <summary>
        /// Devuelve el documento control asociado al tercero 
        /// </summary>
        /// <returns></returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = this.uc_MasterTercero.Value;
                string numDoc = this.txtDocumentoTercero.Text.Trim();
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(AppDocuments.Anticipos, tercero, numDoc);

                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Asigna el proyecto y el centro de costo segun el tipo de anticipo y la moneda
        /// </summary>
        private void ValidateCtaValues()
        {
            if (this._tipoMoneda == -1)
                this._tipoMoneda = Convert.ToInt32((this.cmbMoneda.SelectedItem as ComboBoxItem).Value);

            if (this.uc_MasterTipoAnticipo.ValidID)
            {
                this._tipoAnticipo = (DTO_cpAnticipoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, this.uc_MasterTipoAnticipo.Value, true);

                DTO_coDocumento doc = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, this._tipoAnticipo.coDocumentoID.Value, true);
                if (this._tipoMoneda == (int)TipoMoneda.Local)
                {
                    //Valida que el documento asociado tenga cuenta local
                    if (string.IsNullOrWhiteSpace(doc.CuentaLOC.Value))
                    {
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Co_DocNoCta + "&&" + doc.ID.Value));
                        return;
                    }
                    this._cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, doc.CuentaLOC.Value, true);
                }
                else
                {
                    //Valida que el documento asociado tenga cuenta local
                    if (string.IsNullOrWhiteSpace(doc.CuentaEXT.Value))
                    {
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Co_DocNoCta + "&&" + doc.ID.Value));
                        return;
                    }
                    this._cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, doc.CuentaEXT.Value, true);
                }

                //Verifica el proyecto
                if (this._cta.ProyectoInd.Value.Value)
                {
                    this.uc_MasterProyecto.EnableControl(true);
                    //this.uc_MasterProyecto.Value = string.Empty;
                }
                else
                {
                    this.uc_MasterProyecto.EnableControl(false);
                    this.uc_MasterProyecto.Value = this._defProy;
                }

                //Verifica el centro de costo
                if (this._cta.CentroCostoInd.Value.Value)
                {
                    this.uc_MasterCentroCosto.EnableControl(true);
                    //this.uc_MasterCentroCosto.Value = string.Empty;
                }
                else
                {
                    this.uc_MasterCentroCosto.EnableControl(false);
                    this.uc_MasterCentroCosto.Value = this._defCtoCosto;
                }
            }
            else
            {
                this.uc_MasterCentroCosto.EnableControl(true);
                this.uc_MasterProyecto.EnableControl(true);
            }
        }

        /// <summary>
        /// Calcula el valor total de los subtotales en los viajes
        /// </summary>
        /// <returns></returns>
        private decimal ValorTotalViajes()
        {
            try
            {
                this._gAlojamiento = Convert.ToDecimal(this.txtVTGAlojamiento.EditValue, CultureInfo.InvariantCulture);
                this._gAlimentacion = Convert.ToDecimal(this.txtVTGAlimentacion.EditValue, CultureInfo.InvariantCulture);
                this._gTransporte = Convert.ToDecimal(this.txtVTGTransporte.EditValue, CultureInfo.InvariantCulture);
                this._gOtrosGastos = Convert.ToDecimal(this.txtVTOtrosGastos.EditValue, CultureInfo.InvariantCulture);
                this._gTiquetes = Convert.ToDecimal(this.txtValorTiquetes.EditValue, CultureInfo.InvariantCulture);
                return _gAlojamiento + _gAlimentacion + _gTransporte + _gOtrosGastos + _gTiquetes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "ValorTotalViajes"));
                return 0;
            }
        }

        /// <summary>
        /// Valida controles de fechas
        /// </summary>
        private void validateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
            this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);
        }

        /// <summary>
        /// Valida los campos obligatorios
        /// </summary>
        /// <returns></returns>
        private string FieldsObligated()
        {
            this._message = string.Empty;

            if (this.multiMoneda)
                this._tc = _bc.AdministrationModel.TasaDeCambio_Get(this.monedaExtranjera, this.dtFecha.DateTime);

            var field = string.Empty;
            if (this.multiMoneda && this._tc == 0)
                return _bc.GetResource(LanguageTypes.Forms, DictionaryMessages.Err_Co_NoTasaCambio);

            field = string.IsNullOrWhiteSpace(this.dtFecha.Text) ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaAnticipo") : string.Empty;
            field = string.IsNullOrWhiteSpace(this.dtFecha.Text) ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaAnticipo") : string.Empty;
            field = !this.uc_MasterTercero.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coTercero + "_lblTitle") : field;
            field = string.IsNullOrWhiteSpace(this.txtDocumentoTercero.Text) ? field = field + "\n" + this.lblDocumentoTercero.Text : field;
            field = string.IsNullOrWhiteSpace(this.cmbMoneda.Text) ? field = field + "\n" + this.lblMoneda.Text : field;
            field = !this.uc_MasterTipoAnticipo.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.cpAnticipoTipo + "_lblTitle") : field;
            field = string.IsNullOrWhiteSpace(this.txtDescripcion.Text) ? field = field + "\n" + this.lblDescripcion.Text : field;
            field = !this.uc_MasterCentroCosto.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coCentroCosto + "_lblTitle") : field;
            field = !this.uc_MasterProyecto.ValidID ? field = field + "\n" + _bc.GetResource(LanguageTypes.Forms, AppMasters.coProyecto + "_lblTitle") : field;
            field = string.IsNullOrWhiteSpace(this.txtPlazo.Text) ? field = field + "\n" + this.lblDiasLegalizacion.Text : field;
            if (!string.IsNullOrWhiteSpace(field))
            {
                this._message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_FacturaFieldObligated);
                this._message = string.Format(this._message, field);
            }

            return _message;
        }

        /// <summary>
        /// Carga la informacion para radicar un anticipo
        /// </summary>
        private bool LoadRadicarData()
        {
            try
            {
                //Campos variables DTO_glDocumentoControl
                this._dtoCtrl = new DTO_glDocumentoControl();
                this._dtoCtrl.DocumentoID.Value = this.documentID;
                this._dtoCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._dtoCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._dtoCtrl.PrefijoID.Value = this.txtPrefix.Text;

                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.Descripcion.Value = "Cont. Anticipos";
                this._dtoCtrl.CuentaID.Value = this._cta.ID.Value;
                this._dtoCtrl.ProyectoID.Value = this.uc_MasterProyecto.Value;
                this._dtoCtrl.CentroCostoID.Value = this.uc_MasterCentroCosto.Value;
                this._dtoCtrl.LineaPresupuestoID.Value = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                this._dtoCtrl.LugarGeograficoID.Value = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                this._dtoCtrl.ConsSaldo.Value = 0;
                this._dtoCtrl.MonedaID.Value = this._tipoMoneda == (int)TipoMoneda.Local ? this.monedaLoc : this.monedaExtranjera;
                this._dtoCtrl.TasaCambioCONT.Value = this._tc;
                this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                this._dtoCtrl.TerceroID.Value = uc_MasterTercero.Value;
                this._dtoCtrl.DocumentoTercero.Value = txtDocumentoTercero.Text;
                this._dtoCtrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                this._dtoCtrl.seUsuarioID.Value = this.userID;

                //Campos variables DTO_cpAnticipos
                this._anticipo = new DTO_cpAnticipo();
                this._anticipo.RadicaFecha.Value = this.dtFecha.DateTime;
                this._anticipo.AnticipoTipoID.Value = uc_MasterTipoAnticipo.Value;
                this._anticipo.Plazo.Value = Convert.ToByte(this.txtPlazo.Text);
                this._anticipo.ConceptoCxPID.Value = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCxPAnticipos);

                /* Espacio para programar tipo anticipo viaje  */
                if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && this._tipoAnticipo.GastosViajeInd.Value.Value)
                {
                    this._anticipo.DiasAlojamiento.Value = Convert.ToByte(this.txtDiasGAlojamiento.EditValue);
                    this._anticipo.DiasAlimentacion.Value = Convert.ToByte(this.txtDiasGAlimentacion.EditValue);
                    this._anticipo.DiasTransporte.Value = Convert.ToByte(this.txtDiasGTransporte.EditValue);
                    this._anticipo.DiasOtrosGastos.Value = Convert.ToByte(this.txtDiasOtrosGastos.EditValue);

                    this._anticipo.ValorAlimentacion.Value = Convert.ToDecimal(this.txtVDGAlimentacion.EditValue);
                    this._anticipo.ValorAlojamiento.Value = Convert.ToDecimal(this.txtVDGAlojamiento.EditValue);
                    this._anticipo.ValorTransporte.Value = Convert.ToDecimal(this.txtVDGTransporte.EditValue);
                    this._anticipo.ValorOtrosGastos.Value = Convert.ToDecimal(this.txtVDOtrosGastos.EditValue);
                    this._anticipo.ValorTiquetes.Value = Convert.ToDecimal(this.txtValorTiquetes.EditValue);

                    this._anticipo.FechaSalida.Value = this.dtViajeInicio.DateTime;
                    this._anticipo.FechaRetorno.Value = this.dtViajeFin.DateTime;

                    this._anticipo.TipoViaje.Value = Convert.ToByte((this.cmbTipoViaje.SelectedItem as ComboBoxItem).Value);
                }

                this._anticipo.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Valor.Value = this._anticipo.Valor.Value;

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_LoadingData));
                return false;
            }
        }

        /// <summary>
        /// Carga la informacion para actualizar un anticipo
        /// </summary>
        private bool LoadUpdateData()
        {
            try
            {
                //Campos variables DTO_glDocumentoControl           
                this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                this._dtoCtrl.Observacion.Value = this.txtDescripcion.Text;
                this._dtoCtrl.MonedaID.Value = this._tipoMoneda == (int)TipoMoneda.Local ? this.monedaLoc : this.monedaExtranjera;
                this._dtoCtrl.TasaCambioCONT.Value = this._tc;
                this._dtoCtrl.TasaCambioDOCU.Value = this._dtoCtrl.TasaCambioCONT.Value;
                this._dtoCtrl.seUsuarioID.Value = this.userID;

                //Campos variables DTO_cpAnticipos
                this._anticipo = new DTO_cpAnticipo();
                this._anticipo.RadicaFecha.Value = this.dtFecha.DateTime;
                this._anticipo.AnticipoTipoID.Value = uc_MasterTipoAnticipo.Value;
                this._anticipo.Plazo.Value = Convert.ToByte(this.txtPlazo.Text);
                this._anticipo.ConceptoCxPID.Value = _bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCxPAnticipos);
                this._anticipo.NumeroDoc.Value = this._dtoCtrl.NumeroDoc.Value;

                /* Espacio para programar tipo anticipo viaje  */
                if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && this._tipoAnticipo.GastosViajeInd.Value.Value)
                {
                    this._anticipo.DiasAlojamiento.Value = Convert.ToByte(this.txtDiasGAlojamiento.EditValue);
                    this._anticipo.DiasAlimentacion.Value = Convert.ToByte(this.txtDiasGAlimentacion.EditValue);
                    this._anticipo.DiasTransporte.Value = Convert.ToByte(this.txtDiasGTransporte.EditValue);
                    this._anticipo.DiasOtrosGastos.Value = Convert.ToByte(this.txtDiasOtrosGastos.EditValue);

                    this._anticipo.ValorAlimentacion.Value = Convert.ToDecimal(this.txtVDGAlimentacion.EditValue);
                    this._anticipo.ValorAlojamiento.Value = Convert.ToDecimal(this.txtVDGAlojamiento.EditValue);
                    this._anticipo.ValorTransporte.Value = Convert.ToDecimal(this.txtVDGTransporte.EditValue);
                    this._anticipo.ValorOtrosGastos.Value = Convert.ToDecimal(this.txtVDOtrosGastos.EditValue);
                    this._anticipo.ValorTiquetes.Value = Convert.ToDecimal(this.txtValorTiquetes.EditValue);

                    this._anticipo.FechaSalida.Value = this.dtViajeInicio.DateTime;
                    this._anticipo.FechaRetorno.Value = this.dtViajeFin.DateTime;

                    this._anticipo.TipoViaje.Value = Convert.ToByte((this.cmbTipoViaje.SelectedItem as ComboBoxItem).Value);
                }

                this._anticipo.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                this._dtoCtrl.Valor.Value = this._anticipo.Valor.Value;

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_LoadingData));
                return false;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            base.SetInitParameters();
            InitializeComponent();
            this.InitControls();
            this.documentID = AppDocuments.Anticipos;

            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.frmModule = ModulesPrefix.cp;
            //Carga info de las monedas
            this.monedaLoc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.consecutivoAnticipo = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoAnticipo);
            this.consecutivoAnticipo = string.IsNullOrEmpty(this.consecutivoAnticipo) ? "1" : this.consecutivoAnticipo;
        }

        /// <summary>
        /// Funcion q se ejecuta despues de inicializar la pantalla
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.validateDates();
            this.gbGridDocument.Enabled = false;
            this.dtViajeInicio.DateTime = DateTime.Now;
            this.dtViajeFin.DateTime = DateTime.Now;
            if (!this.multiMoneda)
                this.cmbMoneda.Enabled = false;
            this.txtDocumentoTercero.Text = this.consecutivoAnticipo;
        }

        #endregion

        #region Eventos Virtuales MDI

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
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSave.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                FormProvider.Master.itemPrint.Enabled = false;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que valida si existe el documento y si este tiene asociado un anticipo a su vez
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtDocumentoTercero_Leave(object sender, EventArgs e)
        {
            try
            {
                this._nuevoDoc = true;
                this._dtoCtrl = this.GetDocumentExt();
                if (this._dtoCtrl != null)
                {
                    this._anticipo = _bc.AdministrationModel.cpAnticipos_GetByEstado(_dtoCtrl.NumeroDoc.Value.Value, EstadoDocControl.ParaAprobacion);
                    if (this._anticipo != null)
                    {
                        this._nuevoDoc = false;
                        this.LoadDocumentInfo();
                        FormProvider.Master.itemPrint.Enabled = true;
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoNoDisponible));
                }
                else
                    this.RefreshDocument(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtDocumentoTercero_Leave"));
            }
        }

        /// <summary>
        /// Carga los campos para tipo de anticipo de viajes
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void uc_MasterTipoAnticipo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.uc_MasterTipoAnticipo.ValidID)
                {
                    if (this._tipoAnticipo.ID.Value != this.uc_MasterTipoAnticipo.Value)
                    {
                        this._tipoAnticipo = (DTO_cpAnticipoTipo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, this.uc_MasterTipoAnticipo.Value, true);
                        if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && this._tipoAnticipo.GastosViajeInd.Value.Value)
                        {
                            this.gbGridDocument.Enabled = true;
                            this.txtValor.Enabled = false;
                        }
                        else
                        {
                            this.gbGridDocument.Enabled = false;
                            this.txtValor.Enabled = true;
                        }

                        this.ValidateCtaValues();
                    }
                }
                else
                {
                    this._tipoAnticipo = new DTO_cpAnticipoTipo();
                    this.txtValor.Enabled = true;
                    this.RefreshDocument(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "uc_MasterTipoAnticipo_Leave"));
            }
        }

        /// <summary>
        /// Carga la info de la cuenta segun la moneda
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void cmbMoneda_Leave(object sender, EventArgs e)
        {
            try
            {
                int tm = 0;
                try
                {
                    tm = Convert.ToInt32((this.cmbMoneda.SelectedItem as ComboBoxItem).Value);
                }
                catch (Exception)
                {
                    this.cmbMoneda.SelectedIndex = 0;
                }

                if (this._tipoMoneda != tm)
                {
                    this._tipoMoneda = tm;
                    this.ValidateCtaValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "cmbMoneda_Leave"));
            }
        }

        /// <summary>
        /// Dias de alojamiento X valor diario de alojamiento
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtDiasGAlojamiento_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtDiasGAlojamiento.Text))
                    this.txtDiasGAlojamiento.EditValue = 0;

                decimal dias = Convert.ToDecimal(this.txtDiasGAlojamiento.EditValue, CultureInfo.InvariantCulture);
                decimal valordiario = Convert.ToDecimal(this.txtVDGAlojamiento.EditValue, CultureInfo.InvariantCulture);

                this.txtVTGAlojamiento.EditValue = (dias * valordiario);
                this.txtValor.EditValue = this.ValorTotalViajes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtDiasGAlojamiento_Leave"));
            }
        }

        /// <summary>
        /// Dias de alimentacion X Gastos diarios de alimentacion
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtDiasGAlimentacion_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtDiasGAlimentacion.Text))
                    this.txtDiasGAlimentacion.EditValue = 0;

                decimal dias = Convert.ToDecimal(this.txtDiasGAlimentacion.EditValue, CultureInfo.InvariantCulture);
                decimal valordiario = Convert.ToDecimal(this.txtVDGAlimentacion.EditValue, CultureInfo.InvariantCulture);

                this.txtVTGAlimentacion.EditValue = (dias * valordiario);
                this.txtValor.EditValue = this.ValorTotalViajes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtDiasGAlimentacion_Leave"));
            }
        }

        /// <summary>
        /// Dias de transporte X Gastos de Transporte
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtDiasGTransporte_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtDiasGTransporte.Text))
                    this.txtDiasGTransporte.EditValue = 0;

                decimal dias = Convert.ToDecimal(this.txtDiasGTransporte.EditValue, CultureInfo.InvariantCulture);
                decimal valordiario = Convert.ToDecimal(this.txtVDGTransporte.EditValue, CultureInfo.InvariantCulture);

                this.txtVTGTransporte.EditValue = (dias * valordiario);
                this.txtValor.EditValue = this.ValorTotalViajes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtDiasGTransporte_Leave"));
            }
        }

        /// <summary>
        /// Dias otrosn gastos X Valor Otros Gastos
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtDiasOtrosGastos_Leave(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.txtDiasOtrosGastos.Text))
                    this.txtDiasOtrosGastos.EditValue = 0;

                decimal dias = Convert.ToDecimal(this.txtDiasOtrosGastos.EditValue, CultureInfo.InvariantCulture);
                decimal valordiario = Convert.ToDecimal(this.txtVDOtrosGastos.EditValue, CultureInfo.InvariantCulture);

                this.txtVTOtrosGastos.EditValue = (dias * valordiario);
                this.txtValor.EditValue = this.ValorTotalViajes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtDiasOtrosGastos_Leave"));
            }
        }

        /// <summary>
        /// Carga el valor de los tiquetes
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void txtValorTiquetes_Leave(object sender, EventArgs e)
        {
            this.txtValor.EditValue = this.ValorTotalViajes();
        }

        /// <summary>
        /// Evento que controlar la digitacion del campo este es solo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPlazo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Regex.IsMatch(e.KeyChar.ToString(
                    ), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                    e.Handled = true;
                else if (e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                {
                    try
                    {
                        TextEdit txt = (TextEdit)sender;
                        string str = txt.Text + e.KeyChar.ToString();
                        Convert.ToByte(str);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "txtPlazo_KeyPress"));
            }
        }

        /// <summary>
        /// Evento que controlar la digitacion del campo este es solo numerico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
        }

        /// <summary>
        /// Eventa valida rango de fechas para los viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtViajeInicio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isFistLoad)
                {
                    if (dtViajeInicio.DateTime.Date > dtViajeFin.DateTime.Date)
                    {
                        this.dtViajeInicio.DateTime = DateTime.Now;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoDateStartNotValid));
                    }
                }
                this._isFistLoad = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "dtViajeInicio_EditValueChanged"));
            }
        }

        /// <summary>
        /// Eventa valida rango de fechas para los viajes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtViajeFin_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_isFistLoad)
                {
                    if (dtViajeInicio.DateTime.Date > dtViajeFin.DateTime.Date)
                    {
                        this.dtViajeFin.DateTime = DateTime.Now;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoDateStartNotValid));
                    }
                }
                this._isFistLoad = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "dtViajeFin_EditValueChanged"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Restablece los valores iniciales en el formulario
        /// </summary>
        public override void TBNew()
        {
            base.TBNew();
            this.RefreshDocument(true);

            this._bc.AdministrationModel.ControlList = this._bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, this._bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();
            this.consecutivoAnticipo = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConsecutivoAnticipo);
            this.consecutivoAnticipo = string.IsNullOrEmpty(this.consecutivoAnticipo) ? "1" : this.consecutivoAnticipo;
            this.txtDocumentoTercero.Text = this.consecutivoAnticipo;
        }

        /// <summary>
        /// Se envia el anticipo para aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            if (this._cta == null || this._tipoAnticipo.ID == null)
                this.ValidateCtaValues();
            this._message = this.FieldsObligated();
            if (string.IsNullOrEmpty(this._message))
            {
                if (this._nuevoDoc)
                {
                    if (this.LoadRadicarData())
                    {
                        Thread process = new Thread(this.RadicarThread);
                        process.Start();
                    }
                }
                else
                {
                    if (this.LoadUpdateData())
                    {
                        Thread process = new Thread(this.ActualizarThread);
                        process.Start();
                    }
                }
            }
            else
                MessageBox.Show(this._message);
        }

        /// <summary>
        /// Se envia el anticipo para aprobación
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                #region Genera el reporte
               
                var a = this._anticipo.AnticipoTipoID.Value;
                DTO_cpAnticipoTipo tipoAnticipo = (DTO_cpAnticipoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, (string)a, true);

                if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && !tipoAnticipo.GastosViajeInd.Value.Value)
                    this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipo(this._dtoCtrl.NumeroDoc.Value.Value, false, ExportFormatType.pdf);
                else
                    this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipoViaje(this._dtoCtrl.NumeroDoc.Value.Value, false, ExportFormatType.pdf);

                if (this.reportName == string.Empty)
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                else
                {
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this._dtoCtrl.NumeroDoc.Value.Value, null, this.reportName);
                    Process.Start(fileURl);
                }

                #endregion

                //if (this._dataLoadedInd)
                //{
                    //EstadoDocControl estado = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), this._dtoCtrl.Estado.Value.Value.ToString());
                    //if (!(bool)_tipoAnticipo.GastosViajeInd.Value)
                    //{
                    //    DTO_ReportAnticipo anticipos = new DTO_ReportAnticipo();

                    //    //if (this.dtFecha.DateTime == _anticipo.RadicaFecha.Value.Value)
                    //    anticipos.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar) ? false : true;
                    //    anticipos.Fecha = Convert.ToDateTime(_dtoCtrl.Fecha.Value);
                    //    anticipos.AnticipoTipoDesc = this._tipoAnticipo.Descriptivo.Value.ToString();
                    //    anticipos.TerceroID = this._dtoCtrl.TerceroID.Value.ToString();
                    //    DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = this._dtoCtrl.TerceroID.Value }, true);
                    //    anticipos.TerceroDesc = ((NewAge.DTO.Negocio.DTO_MasterBasic)(terceroInfo)).Descriptivo.ToString();
                    //    anticipos.Observacion = this._dtoCtrl.Observacion.Value.ToString();
                    //    anticipos.Valor = this._anticipo.Valor.Value.Value;

                    //    List<DTO_ReportAnticipo> anticipoList = new List<DTO_ReportAnticipo>();
                    //    anticipoList.Add(anticipos);

                    //    AnticipoReport anticiposReport = new AnticipoReport(AppReports.cpAnticipo, anticipoList, anticipos.EstadoInd, _bc);
                    //    anticiposReport.ShowPreview();
                    //}
                    //else
                    //{
                        //DTO_ReportAnticipoViaje anticiposViaje = new DTO_ReportAnticipoViaje();

                        //anticiposViaje.No = ""; ////////////
                        //anticiposViaje.Fecha = Convert.ToDateTime(this._dtoCtrl.Fecha.Value);
                        //anticiposViaje.EmpresaID = _anticipo.EmpresaID.Value.ToString();
                        //anticiposViaje.EstadoInd = (estado != EstadoDocControl.Radicado && estado != EstadoDocControl.ParaAprobacion && estado != EstadoDocControl.SinAprobar) ? false : true;
                        //anticiposViaje.Area = ""; /////////////
                        //anticiposViaje.DocumentoIdent = this._dtoCtrl.TerceroID.Value.ToString();
                        //DTO_coTercero terceroInfo = (DTO_coTercero)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coTercero, new UDT_BasicID() { Value = this._dtoCtrl.TerceroID.Value }, true);
                        //anticiposViaje.Nombres = ((NewAge.DTO.Negocio.DTO_MasterBasic)(terceroInfo)).Descriptivo.ToString();
                        //anticiposViaje.MotivoViaje = ""; /////////////
                        //anticiposViaje.Destino = ""; /////////////
                        //anticiposViaje.DiasAlojamiento = (int)_anticipo.DiasAlojamiento.Value;
                        //anticiposViaje.ValorAlojamiento = (decimal)_anticipo.ValorAlojamiento.Value;
                        //anticiposViaje.TotalAlojamiento = anticiposViaje.DiasAlojamiento * anticiposViaje.ValorAlojamiento;
                        //anticiposViaje.DiasAlimentacion = (int)_anticipo.DiasAlimentacion.Value;
                        //anticiposViaje.ValorAlimentacion = (decimal)_anticipo.ValorAlimentacion.Value;
                        //anticiposViaje.TotalAlimentacion = anticiposViaje.DiasAlimentacion * anticiposViaje.ValorAlimentacion;
                        //anticiposViaje.DiasTransporte = (int)_anticipo.DiasTransporte.Value;
                        //anticiposViaje.ValorTransporte = (decimal)_anticipo.ValorTransporte.Value;
                        //anticiposViaje.TotalTransporte = anticiposViaje.DiasTransporte * anticiposViaje.ValorTransporte;
                        //anticiposViaje.DiasOtrosGastos = (int)_anticipo.DiasOtrosGastos.Value;
                        //anticiposViaje.ValorOtrosGastos = (decimal)_anticipo.ValorOtrosGastos.Value;
                        //anticiposViaje.TotalOtrosGastos = anticiposViaje.DiasOtrosGastos * anticiposViaje.ValorOtrosGastos;
                        //anticiposViaje.TotalAnticipo = anticiposViaje.TotalAlojamiento + anticiposViaje.TotalAlimentacion + anticiposViaje.TotalTransporte + anticiposViaje.TotalOtrosGastos;
                        //anticiposViaje.Funcionario = "";
                        //anticiposViaje.Autorizado = "";

                        //List<DTO_ReportAnticipoViaje> anticViajeList = new List<DTO_ReportAnticipoViaje>();
                        //anticViajeList.Add(anticiposViaje);

                        //AnticipoViajeReport anticViajeReport = new AnticipoViajeReport(AppReports.cpAnticipoViaje, anticViajeList, anticiposViaje.EstadoInd, _bc);
                        //anticViajeReport.ShowPreview();
                    //}
                //}
                //else
                //{
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_AnticipoNoExiste));
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo para radicar un anticipo
        /// </summary>
        private void RadicarThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.cpAnticipos_Guardar(this.documentID, _dtoCtrl, _anticipo, false);
                FormProvider.Master.StopProgressBarThread(this.documentID);                

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);

                if (isOK)
                {
                    #region Genera el reporte
                    string numDoc = string.Empty;
                    if (obj.GetType() == typeof(DTO_Alarma))
                    {
                        numDoc = ((DTO_Alarma)obj).NumeroDoc;
                        this._dtoCtrl.NumeroDoc.Value = Convert.ToInt32(numDoc);
                    }

                    var a = this._anticipo.AnticipoTipoID.Value;
                    DTO_cpAnticipoTipo tipoAnticipo = (DTO_cpAnticipoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, (string)a, true);

                    if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && !tipoAnticipo.GastosViajeInd.Value.Value)
                        this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipo(Convert.ToInt32(numDoc), false, ExportFormatType.pdf);
                    else
                        this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipoViaje(Convert.ToInt32(numDoc), false, ExportFormatType.pdf);

                    if (this.reportName == string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                    else
                    {
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(numDoc), null, this.reportName);
                        Process.Start(fileURl);
                    }

                    #endregion
                    this.Invoke(this.sendToApproveDelegate);
                }                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Anticipos.cs", "RadicarThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo para actualizar un anticipo
        /// </summary>
        private void ActualizarThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.cpAnticipos_Guardar(this.documentID, this._dtoCtrl, this._anticipo, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);               

                if (isOK)
                {
                    #region Genera el reporte
                    string numDoc = string.Empty;
                    if (obj.GetType() == typeof(DTO_Alarma))
                    {
                        numDoc = ((DTO_Alarma)obj).NumeroDoc;
                        this._dtoCtrl.NumeroDoc.Value = Convert.ToInt32(numDoc);
                    }

                    var a = this._anticipo.AnticipoTipoID.Value;
                    DTO_cpAnticipoTipo tipoAnticipo = (DTO_cpAnticipoTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpAnticipoTipo, false, (string)a, true);

                    if (this._tipoAnticipo.GastosViajeInd.Value.HasValue && !tipoAnticipo.GastosViajeInd.Value.Value)
                        this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipo(Convert.ToInt32(numDoc), false, ExportFormatType.pdf);
                    else
                        this.reportName = this._bc.AdministrationModel.ReportesCuentasXPagar_DocumentoAnticipoViaje(Convert.ToInt32(numDoc), false, ExportFormatType.pdf);

                    if (this.reportName == string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));
                    else
                    {
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(numDoc), null, this.reportName);
                        Process.Start(fileURl);
                    }

                    #endregion
                    this.Invoke(this.sendToApproveDelegate);
                }                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
