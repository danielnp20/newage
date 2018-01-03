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
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Ajuste de comprobates
    /// </summary>
    public partial class ComprobanteAjuste : DocumentAuxiliarForm
    {
        public ComprobanteAjuste()
        {
            // InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadData(true);
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer;
            this.gvDocument.RefreshData();
            if (this.resultOK)
            {
                this.CleanHeader(true);
                this.EnableHeader(true);
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
            }
        }

        #endregion

        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private bool _txtNumberFocus;

        private bool newRow = false;
        private decimal vlrCuentasCosto = 0;
        private int externalDocID = 0;

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

        //Cuenta
        protected override DTO_coPlanCuenta Cuenta
        {
            set
            {
                if (value != null)
                {
                    base.Cuenta = value;
                    if (this.saldoControl != SaldoControl.Cuenta)
                    {
                        //Deshabilita las columnas
                        this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;

                        this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                        this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;

                        if (!this.masterTercero.ValidID)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_AjustarCompInvCta));
                            this.masterCuenta.Focus();
                        }
                    }
                }
                else
                {
                    base.Cuenta = value;
                    if (!this.biMoneda)
                        this.gvDocument.Columns[this.unboundPrefix + "TasaCambio"].OptionsColumn.AllowEdit = true;
                }
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.ComprobanteAjuste;
            InitializeComponent();

            this.frmModule = ModulesPrefix.co;
            base.SetInitParameters();

            //Controles del header
            _bc.InitMasterUC(this.masterComprobante, AppMasters.coComprobante, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            _bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false);
            this.masterMoneda.EnableControl(false);
            this.masterDocumento.EnableControl(false);

            //Llena los combos
            TablesResources.GetTableResources(this.cmbMonedaOrigen, typeof(TipoMoneda_LocExt));
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected override void CleanFormat()
        {
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            string f = string.Empty;
            foreach (string col in cols)
            {
                if (col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaOtr") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd1") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd2") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd3") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd4") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd5") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd6") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd7") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd8") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd9") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd10"))
                {
                    if (col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrBaseME") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaExt") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambio"))
                    {
                        if (this.multiMoneda)
                            f += col + this.formatSeparator;
                    }
                    else
                    {
                        f += col + this.formatSeparator;
                    }
                }
            }

            this.format = f;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.dtFecha.Enabled = false;
            this.dtPeriod.Enabled = true;
            this.dtPeriod.Focus();
            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.lblTasaCambio.Visible = false;
                this.txtTasaCambio.Visible = false;
            }
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                this.dtFecha.DateTime = this.dtPeriod.DateTime;

                this.masterComprobante.Value = string.Empty;
                this.txtNumber.Text = string.Empty;

                this.txtTasaCambio.Text = string.Empty;
                this.txtValor.Text = string.Empty;
                this.monedaId = this.monedaLocal;
                this.cmbMonedaOrigen.SelectedIndex = 0;
                this.masterMoneda.Value = this.monedaId;
                this.masterDocumento.Value = string.Empty;
            }

            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterComprobante.EnableControl(enable);
            this.txtNumber.Enabled = enable;

            if (enable)
                this.masterComprobante.Focus();
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override Object LoadTempHeader()
        {
            DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
            header.ComprobanteID.Value = this.masterComprobante.Value;
            header.ComprobanteNro.Value = Convert.ToInt16(this.txtNumber.Text);
            header.EmpresaID.Value = this.empresaID;
            header.Fecha.Value = this.dtFecha.DateTime;
            header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
            header.MdaOrigen.Value = Convert.ToByte((((ComboBoxItem)(this.cmbMonedaOrigen.SelectedItem)).Value));
            header.MdaTransacc.Value = this.monedaId;
            header.PeriodoID.Value = this.dtPeriod.DateTime;
            header.TasaCambioBase.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
            header.TasaCambioOtr.Value = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

            return header;
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterComprobante.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterComprobante.CodeRsx);

                MessageBox.Show(msg);
                this.masterComprobante.Focus();

                result = false;
            }
            else
            {
                this.comprobanteID = this.masterComprobante.Value;
                DTO_coComprobante dtoComp = (DTO_coComprobante)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, false, this.masterComprobante.Value, true);
                this.biMoneda = dtoComp.biMonedaInd.Value.Value;
            }

            return result;
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave(int monOr)
        {
            try
            {
                decimal totalLoc = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
                decimal totalExt = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

                #region Valida sumatoria de valores
                decimal validDif = Convert.ToDecimal(_bc.GetControlValue(AppControl.DiferenciaContablePermitida), CultureInfo.InvariantCulture);

                if (monOr == (int)TipoMoneda.Local)
                {
                    //Si es moneda local varifica qe la suma local sea 0 y que la ext no sea mayor quela diferencia permitida
                    if (totalLoc != 0 || Math.Abs(totalExt) > validDif)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal));
                        return false;
                    }
                    else if (totalExt < 0)
                    {
                        totalExt = 0;
                        this.Comprobante.Footer.Last().vlrMdaExt.Value = this.Comprobante.Footer.Last().vlrMdaExt.Value.Value + (totalExt * -1);
                        this.CalcularTotal();
                    }
                    else if (totalExt > 0)
                    {
                        totalExt = 0;
                        this.Comprobante.Footer.Last().vlrMdaExt.Value = this.Comprobante.Footer.Last().vlrMdaExt.Value.Value - totalExt;
                        this.CalcularTotal();
                    }
                }
                else
                {
                    //Si es moneda extranjera varifica que la suma ext sea 0 y que la local no sea mayor quela diferencia permitida
                    if (totalExt != 0 || Math.Abs(totalLoc) > validDif)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal));
                        return false;
                    }
                    else if (totalLoc < 0)
                    {
                        totalLoc = 0;
                        this.Comprobante.Footer.Last().vlrMdaLoc.Value = this.Comprobante.Footer.Last().vlrMdaLoc.Value + totalExt;
                        this.CalcularTotal();
                    }
                    else if (totalLoc > 0)
                    {
                        totalLoc = 0;
                        this.Comprobante.Footer.Last().vlrMdaLoc.Value = this.Comprobante.Footer.Last().vlrMdaLoc.Value - totalExt;
                        this.CalcularTotal();
                    }
                }

                #endregion
                #region Valida el valor original del documento

                decimal valor = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                decimal sumVlr = 0;

                if (this.masterMoneda.Value == this.monedaLocal)
                    sumVlr = (from c in this.Comprobante.Footer where c.IdentificadorTR.Value != this.Comprobante.Header.NumeroDoc.Value.Value select c.vlrMdaLoc.Value.Value).Sum();
                else
                    sumVlr = (from c in this.Comprobante.Footer where c.IdentificadorTR.Value != this.Comprobante.Header.NumeroDoc.Value.Value select c.vlrMdaExt.Value.Value).Sum();

                if (Math.Abs(valor) != Math.Abs(sumVlr))
                {
                    string msg = DictionaryMessages.Co_InvalidTotalAjuste + "&&" + sumVlr.ToString();
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, msg));
                    return false;
                }

                #endregion
                #region Valida el valor de las cuentas de costo (CxP)
                if (this.externalDocID == AppDocuments.CausarFacturas || this.externalDocID == AppDocuments.NotaCreditoCxP)
                {
                    Dictionary<string, DTO_coPlanCuenta> cacheCtas = new Dictionary<string, DTO_coPlanCuenta>();
                    Dictionary<string, DTO_coCuentaGrupo> cacheGrupos = new Dictionary<string, DTO_coCuentaGrupo>();
                    DTO_coPlanCuenta ctaTemp = new DTO_coPlanCuenta();
                    DTO_coCuentaGrupo grupoTemp = new DTO_coCuentaGrupo();
                    decimal valCostoTemp = 0;
                    foreach (DTO_ComprobanteFooter det in this.Comprobante.Footer)
                    {
                        //Carga la cuenta
                        if (cacheCtas.ContainsKey(det.CuentaID.Value))
                            ctaTemp = cacheCtas[det.CuentaID.Value];
                        else
                        {
                            ctaTemp = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, det.CuentaID.Value, true);
                            cacheCtas.Add(det.CuentaID.Value, ctaTemp);
                        }

                        //Carga el grupo de cuentas
                        if (cacheGrupos.ContainsKey(ctaTemp.CuentaGrupoID.Value))
                            grupoTemp = cacheGrupos[ctaTemp.CuentaGrupoID.Value];
                        else
                        {
                            grupoTemp = (DTO_coCuentaGrupo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coCuentaGrupo, false, ctaTemp.CuentaGrupoID.Value, true);
                            cacheGrupos.Add(ctaTemp.CuentaGrupoID.Value, grupoTemp);
                        }

                        if (grupoTemp.TipoCuenta.Value == (int)TipoCuentaGrupo.Costo)
                            valCostoTemp += det.vlrMdaLoc.Value.Value;
                    }

                    if (this.vlrCuentasCosto != valCostoTemp)
                    {
                        string msg = DictionaryMessages.Co_InvalidTotalCuentaCosto + "&&" + this.vlrCuentasCosto.ToString() + "&&" + valCostoTemp.ToString();
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, msg));
                        return false;
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "CanSave"));
                return false;
            }

        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            try
            {
                DTO_Comprobante comp = (DTO_Comprobante)aux;
                DTO_ComprobanteHeader header = comp.Header;
                if (comp.Footer == null)
                    comp.Footer = new List<DTO_ComprobanteFooter>();

                bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, header.PeriodoID.Value.Value, header.ComprobanteID.Value, header.ComprobanteNro.Value.Value);
                if (usefulTemp)
                {
                    this.masterComprobante.Value = header.ComprobanteID.Value;
                    this.txtNumber.Text = header.ComprobanteNro.Value.Value.ToString();
                    this.dtPeriod.DateTime = header.PeriodoID.Value.Value;
                    this.dtFecha.DateTime = header.Fecha.Value.Value;
                    this.txtTasaCambio.Text = header.TasaCambioBase.Value.Value.ToString();
                    this.masterMoneda.Value = header.MdaTransacc.Value;
                    this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(header.MdaOrigen.Value.Value.ToString());

                    if (this.ValidateHeader())
                    {
                        //DTO_Comprobante compOriginal = _bc.AdministrationModel.Comprobante_Get(true, false, this.dtPeriod.DateTime, this.masterComprobante.Value, Convert.ToInt32(this.txtNumber.Text), null, null);
                        DTO_glDocumentoControl dtoCtrol = _bc.AdministrationModel.glDocumentoControl_GetByID(header.NumeroDoc.Value.Value);
                        this.masterDocumento.Value = dtoCtrol.DocumentoID.Value.Value.ToString();

                        decimal valor = 0;
                        if (this.masterMoneda.Value == this.monedaLocal)
                            valor = (from c in comp.Footer where c.IdentificadorTR.Value == header.NumeroDoc.Value.Value select c.vlrMdaLoc.Value.Value).Sum();
                        else
                            valor = (from c in comp.Footer where c.IdentificadorTR.Value == header.NumeroDoc.Value.Value select c.vlrMdaExt.Value.Value).Sum();

                        this.txtValor.EditValue = valor;

                        this.EnableHeader(false);

                        this.Comprobante = comp;
                        this.LoadData(true);
                        this.gcDocument.Focus();
                        this.validHeader = true;
                    }
                    else
                    {
                        this.validHeader = false;
                        this.CleanHeader(true);
                    }
                }
                else
                {
                    this.validHeader = false;
                    string rsx = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCompTemp);
                    string msg = string.Format(rsx, header.ComprobanteID.Value, header.ComprobanteNro.Value.Value, header.PeriodoID.Value);
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "LoadTempData"));
            }
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected override void LoadEditGridData(bool isNew, int rowIndex)
        {
            if (!this.disableValidate)
            {
                try
                {
                    bool allowEdit = true;
                    #region Asigna los valores
                    string val_Cuenta = (isNew || gvDocument.Columns[this.unboundPrefix + "CuentaID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "CuentaID"]).ToString();
                    string val_Tercero = (isNew || gvDocument.Columns[this.unboundPrefix + "TerceroID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "TerceroID"]).ToString();
                    string val_Prefijo = (isNew || gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "PrefijoCOM"]).ToString();
                    string val_Documento = (isNew || gvDocument.Columns[this.unboundPrefix + "DocumentoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "DocumentoCOM"]).ToString();
                    string val_Activo = (isNew || gvDocument.Columns[this.unboundPrefix + "ActivoCOM"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ActivoCOM"]).ToString();
                    string val_Descr = (isNew || gvDocument.Columns[this.unboundPrefix + "Descriptivo"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "Descriptivo"]).ToString();
                    string val_ConceptoCargo = (isNew || gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"]).ToString();
                    string val_Proyecto = (isNew || gvDocument.Columns[this.unboundPrefix + "ProyectoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "ProyectoID"]).ToString();
                    string val_CentroCosto = (isNew || gvDocument.Columns[this.unboundPrefix + "CentroCostoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "CentroCostoID"]).ToString();
                    string val_LineaPres = (isNew || gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"]).ToString();
                    string val_LugarGeo = (isNew || gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"]).ToString();
                    string val_BaseML = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrBaseML"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrBaseML"]).ToString();
                    string val_BaseME = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrBaseME"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrBaseME"]).ToString();
                    string val_ValorML = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"]).ToString();
                    string val_ValorME = (isNew || gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"] == null || gvDocument.RowCount == 0) ? string.Empty : this.gvDocument.GetRowCellValue(rowIndex, gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"]).ToString();


                    this.masterCuenta.Value = val_Cuenta;
                    this.masterTercero.Value = val_Tercero;
                    this.masterPrefijo.Value = val_Prefijo;
                    this.txtDocumento.Text = val_Documento;
                    this.txtActivo.Text = val_Activo;
                    this.txtDescripcion.Text = val_Descr;

                    this.masterConceptoCargo.Value = val_ConceptoCargo;
                    this.masterProyecto.Value = val_Proyecto;
                    this.masterCentroCosto.Value = val_CentroCosto;
                    this.masterLineaPre.Value = val_LineaPres;
                    this.masterLugarGeo.Value = val_LugarGeo;
                    this.txtBaseML.EditValue = val_BaseML;
                    this.txtBaseME.EditValue = val_BaseME;
                    this.txtValorML.EditValue = val_ValorML;
                    this.txtValorME.EditValue = val_ValorME;
                    #endregion

                    if (!this.newRow)
                    {
                        this.Cuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, val_Cuenta, true);
                        if (this.saldoControl != SaldoControl.Cuenta)
                            allowEdit = false;

                        #region Carga la info del footer y deshabilita columnas

                        if (!allowEdit)
                        {
                            this.EnableFooter(false);
                            this.txtDescripcion.Enabled = false;

                            //Deshabilita las columnas
                            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;

                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                        }
                        else
                        {
                            this.EnableFooter(true);
                            this.txtDescripcion.Enabled = true;

                            //Habilita las columnas
                            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = true;

                            this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = true;
                            this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = true;
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "LoadEditGridData"));
                }
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            DTO_ComprobanteFooter footerDet = new DTO_ComprobanteFooter();

            try
            {
                #region Asigna datos a la fila
                footerDet.Index = this.Comprobante.Footer.Last().Index + 1;
                footerDet.CuentaID.Value = string.Empty;
                footerDet.TerceroID.Value = string.Empty;
                footerDet.ProyectoID.Value = string.Empty;
                footerDet.CentroCostoID.Value = string.Empty;
                footerDet.LineaPresupuestoID.Value = string.Empty;
                footerDet.ConceptoCargoID.Value = string.Empty;
                footerDet.PrefijoCOM.Value = string.Empty;
                footerDet.DocumentoCOM.Value = string.Empty;
                footerDet.ActivoCOM.Value = string.Empty;
                footerDet.LugarGeograficoID.Value = string.Empty;
                footerDet.ConceptoSaldoID.Value = string.Empty;
                footerDet.Descriptivo.Value = string.Empty;
                footerDet.IdentificadorTR.Value = 0;
                footerDet.TasaCambio.Value = this.Comprobante.Footer.Last().TasaCambio.Value.Value;
                footerDet.DatoAdd1.Value = string.Empty;
                footerDet.DatoAdd2.Value = string.Empty;
                footerDet.PrefDoc = string.Empty;
                footerDet.vlrBaseML.Value = 0;
                footerDet.vlrBaseME.Value = 0;
                footerDet.vlrMdaLoc.Value = 0;
                footerDet.vlrMdaExt.Value = 0;
                footerDet.vlrMdaOtr.Value = 0;
                footerDet.DatoAdd3.Value = string.Empty;
                footerDet.DatoAdd4.Value = string.Empty;
                #endregion
                #region Asigna la visibilidad de las columnas
                //Fks
                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "ConceptoCargoID"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "LugarGeograficoID"].OptionsColumn.AllowEdit = false;
                //Valores
                this.gvDocument.Columns[this.unboundPrefix + "vlrBaseML"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "vlrBaseME"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "vlrMdaLoc"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "vlrMdaExt"].OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns[this.unboundPrefix + "TasaCambio"].OptionsColumn.AllowEdit = false;
                #endregion

                this.newRow = true;
                this.newReg = true;

                this.Comprobante.Footer.Add(footerDet);
                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;

                this.isValid = false;
                this.EnableFooter(false);

                this.Cuenta = null;
                this.newRow = false;

                this.masterCuenta.EnableControl(true);
                this.masterCuenta.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "AddNewRow"));
            }
        }

        #endregion

        #region Eventos del MDI

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

                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al cambiar de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_Leave(object sender, EventArgs e)
        {
            base.dtFecha_Leave(sender, e);
            //if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            //{
            //    this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            //}
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
        /// Valida que el usuario haya ingresado un comprobante existente
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Enter(object sender, EventArgs e)
        {
            bool res = this.ValidateHeader();
            if (res)
            {
                this._txtNumberFocus = true;
                UDT_BasicID basic = new UDT_BasicID() { Value = this.masterComprobante.Value };
                DTO_coComprobante comp = (DTO_coComprobante)_bc.AdministrationModel.MasterSimple_GetByID(AppMasters.coComprobante, basic, true);

                if (this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";
            }
        }

        /// <summary>
        /// Valida que el numero ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumber_Leave(object sender, EventArgs e)
        {
            if (this._txtNumberFocus && !string.IsNullOrWhiteSpace(this.txtNumber.Text) && this.txtNumber.Text != "0")
            {
                this._txtNumberFocus = false;

                try
                {
                    this.externalDocID = 0;
                    DTO_Comprobante compOriginal = _bc.AdministrationModel.AjusteComprobante_Get(this.dtPeriod.DateTime, this.masterComprobante.Value, Convert.ToInt32(this.txtNumber.Text));

                    #region Validaciones

                    #region Comprobante inexistente
                    if (compOriginal == null)
                    {
                        this.validHeader = false;
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Co_CompNoResults));
                        this.txtNumber.Focus();
                        return;
                    }
                    #endregion
                    #region Estado del documento
                    DTO_glDocumentoControl dtoCtrol = _bc.AdministrationModel.glDocumentoControl_GetByID(compOriginal.Header.NumeroDoc.Value.Value);
                    if (dtoCtrol.Estado.Value.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        this.validHeader = false;
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Co_AjusteInvalidEstado));
                        this.txtNumber.Focus();
                        return;
                    }
                    #endregion
                    #region Indicador de ajuste
                    DTO_glDocumento doc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, dtoCtrol.DocumentoID.Value.ToString(), true);
                    this.externalDocID = Convert.ToInt32(doc.ID.Value);
                    if (!doc.AjustaComprobanteInd.Value.Value)
                    {
                        this.validHeader = false;
                        MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_IndAjusteComp));
                        this.txtNumber.Focus();
                        return;
                    }
                    #endregion
                    #region Validación del periodo

                    ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), doc.ModuloID.Value.ToLower());
                    //Valida si existes cierres con el periodo en Cont
                    bool hasCierre = _bc.AdministrationModel.PeriodoHasCierre(compOriginal.Header.PeriodoID.Value.Value, mod);
                    //Valida si esta abierto o cerrado el period en Cont
                    EstadoPeriodo isCerrado = _bc.AdministrationModel.CheckPeriod(compOriginal.Header.PeriodoID.Value.Value, ModulesPrefix.co);

                    if (hasCierre) //Si existen cierres valida el estado del periodo
                    {
                        if (isCerrado == EstadoPeriodo.Cerrado)
                        {
                            this.validHeader = false;
                            MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_Co_AjusteCompPeriodo)+ " y se encuentra actualmente cerrado");
                            this.txtNumber.Focus();
                            return;
                        }
                    }
                    
                    #endregion

                    #endregion
                    #region Agrega la info al formulario

                    //Agrega el periodo y la fecha al formulario
                    this.dtPeriod.DateTime = compOriginal.Header.PeriodoID.Value.Value;
                    this.dtFecha.DateTime = compOriginal.Header.Fecha.Value.Value;

                    //Trae el valor de las cuentas de costo
                    this.vlrCuentasCosto = 0;
                    if (this.externalDocID == AppDocuments.CausarFacturas || this.externalDocID == AppDocuments.NotaCreditoCxP)
                        this.vlrCuentasCosto = _bc.AdministrationModel.Comprobante_GetValorByCuentaCosto(dtoCtrol.NumeroDoc.Value.Value);

                    this.dtFecha.DateTime = compOriginal.Header.Fecha.Value.Value;
                    this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(compOriginal.Header.MdaOrigen.Value.Value.ToString());
                    this.masterMoneda.Value = compOriginal.Header.MdaTransacc.Value;
                    this.txtTasaCambio.Text = compOriginal.Header.TasaCambioBase.Value.Value.ToString();
                    this.masterDocumento.Value = dtoCtrol.DocumentoID.Value.Value.ToString();

                    this.monedaId = compOriginal.Header.MdaTransacc.Value;

                    this.EnableHeader(false);
                    this.Comprobante = compOriginal;
                    this.UpdateTemp(this.Comprobante);
                    this.LoadData(true);

                    this.validHeader = true;
                    this.ValidHeaderTB();

                    this.gcDocument.Focus();
                    #endregion
                    #region Asigna el el valor del documento

                    decimal valor = 0;
                    if (this.masterMoneda.Value == this.monedaLocal)
                        valor = (from c in compOriginal.Footer where c.IdentificadorTR.Value == compOriginal.Header.NumeroDoc.Value.Value select c.vlrMdaLoc.Value.Value).Sum();
                    else
                        valor = (from c in compOriginal.Footer where c.IdentificadorTR.Value == compOriginal.Header.NumeroDoc.Value.Value select c.vlrMdaExt.Value.Value).Sum();

                    this.txtValor.EditValue = Math.Abs(valor);

                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "txtNumber_Leave"));
                }
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
            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();
                #region Si ya tiene datos cargados
                if (!this.dataLoaded && this.txtNumber.Text != "0")
                {
                    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                    return;
                }
                #endregion
                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                DTO_Comprobante comp = new DTO_Comprobante();
                try
                {
                    if (this.Comprobante == null || this.Comprobante.Footer.Count == 0)
                    {
                        comp.Footer = new List<DTO_ComprobanteFooter>();
                        comp.Header = (DTO_ComprobanteHeader)this.LoadTempHeader();

                        comp.TipoDoc = DocumentoTipo.DocInterno;
                        this.Comprobante = comp;

                        this.LoadData(true);
                        this.UpdateTemp(this.Comprobante);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "grlDocument_Enter: " + ex.Message));
                }

                #endregion
            }
            else
                this.masterComprobante.Focus();
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (this.validHeader)
            {
                if (this.data == null)
                {
                    this.gcDocument.Focus();
                    e.Handled = true;
                }
                else
                    this.gvDocument.PostEditor();

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove &&
                    this.saldoControl != SaldoControl.Cuenta)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_AjustarCompInvCta));
                    e.Handled = true;
                }
                else
                    base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
            }
            else
                this.masterComprobante.Focus();
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
                    this.data = new DTO_Comprobante();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;

                    this.masterComprobante.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "TBNew"));
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

                int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                this.gvDocument.ActiveFilterString = string.Empty;
                this.CalcularTotal();
                if (this.ValidGrid() && this.CanSave(monOr))
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Document);

                if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;

                    result = _bc.AdministrationModel.AjusteComprobante_Eliminar(this.documentID, this._actFlujo.ID.Value, this.Comprobante.Header.NumeroDoc.Value.Value);
                    FormProvider.Master.StopProgressBarThread(this.documentID);

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();

                    base.TBNew();
                    if (this.cleanDoc)
                    {
                        this.data = new DTO_Comprobante();
                        this.Comprobante = new DTO_Comprobante();

                        this.gvDocument.ActiveFilterString = string.Empty;
                        this.disableValidate = true;
                        this.gcDocument.DataSource = this.Comprobante.Footer;
                        this.disableValidate = false;

                        this.masterComprobante.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "TBDelete"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
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

                result = _bc.AdministrationModel.AjusteComprobante_Generar(this.documentID, this._actFlujo.ID.Value, this.Comprobante);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (result.Result.Equals(ResultValue.OK))
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.Comprobante = new DTO_Comprobante();
                    this.resultOK = true;
                }
                else
                {
                    this.resultOK = false;
                    this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                            || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());
                }

                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteAjuste.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
