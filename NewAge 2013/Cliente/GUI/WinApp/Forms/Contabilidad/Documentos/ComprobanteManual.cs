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
using NewAge.Forms.Dialogs.Documentos;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Comprobante Manual
    /// </summary>
    public partial class ComprobanteManual : DocumentAuxiliarForm
    {
        //public ComprobanteManual()
        //{
        //    this.InitializeComponent();
        //}

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
                //this.CleanHeader(true);
                //this.EnableHeader(true);

                this.txtNumber.Text = this.compNro;
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.gcDocument.DataSource = this.Comprobante.Footer; 
            this.CleanHeader(true);
            this.EnableHeader(true);
        }

        #endregion

        #region Variables
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private bool _txtNumberFocus;
        private bool _cmbMonedaOrigenFocus;
        private DTO_coDocumentoRevelacion revelacion;
        private bool _compExist = false;

        private string compNro = string.Empty;

        //Variable para Reporte
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

        //Cuenta
        protected virtual DTO_coPlanCuenta Cuenta
        {
            set
            {
                base.Cuenta = value;
                if (value != null && !this.biMoneda)
                {
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
            this.documentID = AppDocuments.ComprobanteManual;
            InitializeComponent();

            this.frmModule = ModulesPrefix.co;
            base.SetInitParameters();

            //Controles del header
            _bc.InitMasterUC(this.masterComprobante, AppMasters.coComprobante, true, true, true, false);
            _bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, true, false);
            this.masterMoneda.EnableControl(false);

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
            foreach(string col in cols)
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

            //Si la empresa no es multimoneda
            if (!this.multiMoneda)
            {
                this.cmbMonedaOrigen.Enabled = false;
                this.AsignarTasaCambio(false);//this.masterMoneda.Value = this.monedaLocal;
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
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                this.dtPeriod.Text = periodo;

                this.masterComprobante.Value = string.Empty;
                this.txtNumber.Text = string.Empty;
            }

            this.txtTasaCambio.Text = string.Empty;
            this.monedaId = this.monedaLocal;
            this.cmbMonedaOrigen.SelectedIndex = 0;
            this.masterMoneda.Value = this.monedaId;
            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.masterComprobante.EnableControl(enable);
            this.dtPeriod.Enabled = enable;
            this.txtNumber.Enabled = enable;
            this.cmbMonedaOrigen.Enabled = this.multiMoneda && enable ? true : false;
            this.dtFecha.Enabled = enable;

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
            header.TasaCambioBase.Value = this.multiMoneda ? Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture) : 0;
            header.TasaCambioOtr.Value = header.TasaCambioBase.Value;

            return header;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="fromTop">Indica si el evento se esta ejecutando desde un control del header superior</param>
        protected override bool AsignarTasaCambio(bool fromTop)
        {
            int monOr = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
            if (monOr == (int)TipoMoneda.Local)
                this.monedaId = this.monedaLocal;
            else
                this.monedaId = this.monedaExtranjera;

            this.masterMoneda.Value = this.monedaId;
            //Sio la empresa no permite mmultimoneda
            if (!this.multiMoneda)
            {
                this.txtTasaCambio.EditValue = 0;
            }
            else
            {
                this.txtTasaCambio.EditValue = this.LoadTasaCambio(monOr);
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
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
            decimal validDif = Convert.ToDecimal(_bc.GetControlValue(AppControl.DiferenciaContablePermitida));
            decimal difLoc = Convert.ToDecimal(this.txtTotalLocal.EditValue, CultureInfo.InvariantCulture);
            decimal difExt = Convert.ToDecimal(this.txtTotalForeign.EditValue, CultureInfo.InvariantCulture);

            //Si el comprobante permite 2 monedas las 2 deben estar cuadradas en 0
            if (this.biMoneda && (difLoc != 0 || difExt != 0))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotalBiMoneda));
                return false;
            }

            if (monOr == (int)TipoMoneda.Local)
            {
                //Si es moneda local varifica qe la suma local sea 0 y que la ext no sea mayor quela diferencia permitida
                if (difLoc != 0 || Math.Abs(difExt) > validDif)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal));
                    return false;
                }
                else if (difExt < 0)
                {
                    this.Comprobante.Footer.Last().vlrMdaExt.Value = this.Comprobante.Footer.Last().vlrMdaExt.Value.Value + (difExt * -1);
                    this.CalcularTotal();
                }
                else if (difExt > 0)
                {
                    this.Comprobante.Footer.Last().vlrMdaExt.Value = this.Comprobante.Footer.Last().vlrMdaExt.Value.Value - difExt;
                    this.CalcularTotal();
                }
            }
            else
            {
                //Si es moneda extranjera varifica qe la suma ext sea 0 y que la local no sea mayor quela diferencia permitida
                if (difExt != 0 || Math.Abs(difLoc) > validDif)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal));
                    return false;
                }
                else if (difLoc < 0)
                {
                    this.Comprobante.Footer.Last().vlrMdaLoc.Value = this.Comprobante.Footer.Last().vlrMdaLoc.Value + difExt;
                    this.CalcularTotal();
                }
                else if (difLoc > 0)
                {
                    this.Comprobante.Footer.Last().vlrMdaLoc.Value = this.Comprobante.Footer.Last().vlrMdaLoc.Value - difExt;
                    this.CalcularTotal();
                }
            }

            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected override void LoadTempData(object aux)
        {
            DTO_Comprobante comp = (DTO_Comprobante)aux;
            DTO_ComprobanteHeader header = comp.Header;
            if (comp.Footer == null)
                comp.Footer = new List<DTO_ComprobanteFooter>();

            bool usefulTemp = _bc.AdministrationModel.ComprobantePre_Exists(this.documentID, header.PeriodoID.Value.Value, header.ComprobanteID.Value, header.ComprobanteNro.Value.Value);
            if (usefulTemp || header.ComprobanteNro.Value == 0)
            {
                this.masterComprobante.Value = header.ComprobanteID.Value;
                this.txtNumber.Text = header.ComprobanteNro.Value.Value.ToString();
                this.dtPeriod.DateTime = header.PeriodoID.Value.Value;
                this.dtFecha.DateTime = header.Fecha.Value.Value;
                this.masterMoneda.Value = header.MdaTransacc.Value;
                this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(header.MdaOrigen.Value.Value.ToString());

                if (this.ValidateHeader() && this.AsignarTasaCambio(false))
                {
                    this.EnableHeader(false);

                    this.Comprobante = comp;
                    this.LoadData(true);
                    this.validHeader = true;
                    this.gcDocument.Focus();
                }
                else
                {
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

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="cta">Cuenta del detalle</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgCero">Mensaje que no permite ceros en un campo</param>
        /// <param name="msgVals">Mensaje paralos valores incorrectos</param>
        protected override void ValidateValoresImport(DTO_ComprobanteFooter dto, DTO_coPlanCuenta cta, DTO_TxResultDetail rd, string msgCero, string msgVals)
        {
            bool createDTO = true;
            decimal impuesto = 0;
            if (!this.multiMoneda)
                dto.TasaCambio.Value = 0;

            #region Asignacion de tasa de cambio
            //Revisa que si no trae una tasa de cambio asigna la del header
            if (dto.TasaCambio.Value.Value == 0)
                dto.TasaCambio.Value = this.Comprobante.Header.TasaCambioBase.Value.Value;
            else
                dto.TasaCambio.Value = Math.Round(dto.TasaCambio.Value.Value, 4);
            #endregion
            #region Validacion de cuentas con impuestos
            if (cta.ImpuestoTipoID == null || string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
            {
                dto.vlrBaseML.Value = 0;
                dto.vlrBaseME.Value = 0;
            }
            else
            {
                impuesto = cta.ImpuestoPorc.Value.Value;
                if (this.biMoneda)
                {
                    #region Revisa que se hayan ingresado los valores bases de las monedas
                    if (dto.vlrBaseML.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    else if (this.multiMoneda && dto.vlrBaseME.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    #endregion
                }
                else
                {
                    #region Revisa los valores bases de la moneda con la que se esta trabajando
                    if (this.monedaId == this.monedaLocal && dto.vlrBaseML.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseML");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    else if (this.multiMoneda && this.monedaId == this.monedaExtranjera && dto.vlrBaseME.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrBaseME");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    #endregion
                }
            }
            #endregion
            #region Validacion de cuentas que no manejan impuestos
            if (!this.biMoneda)
            {
                #region Revisa que exista el valor para la moneda que se esta ingresando
                if (this.monedaId == this.monedaLocal)
                {
                    if (dto.vlrMdaLoc.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                else if (this.monedaId == this.monedaExtranjera)
                {
                    if (this.multiMoneda && dto.vlrMdaExt.Value == 0)
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = string.Format(msgCero, rsxField);
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                #endregion
            }
            #endregion
            #region Validacion de valores para cuentas con impuestos
            if (cta.ImpuestoTipoID != null && !string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
            {
                if (!this.multiMoneda)
                {
                    dto.vlrBaseME.Value = 0;
                    dto.vlrMdaExt.Value = 0;
                }

                decimal impRealML = dto.vlrBaseML.Value.Value * impuesto / 100;
                decimal impRealME = dto.vlrBaseME.Value.Value * impuesto / 100;

                decimal valML = dto.vlrMdaLoc.Value.Value;
                decimal valME = dto.vlrMdaExt.Value.Value;

                if (cta.Naturaleza.Value.Value == (short)NaturalezaCuenta.Credito)
                {
                    valML *= -1;
                    valME *= -1;
                }

                decimal difML = Math.Abs(impRealML) - Math.Abs(valML);
                decimal difMaxML = Math.Abs(dto.vlrBaseML.Value.Value * (Decimal)0.01 / 100);
                decimal difME = Math.Abs(impRealME) - Math.Abs(valME);
                decimal difMaxME = Math.Abs(dto.vlrBaseME.Value.Value * (Decimal)0.01 / 100);

                if (dto.TerceroID.Value != base.nitDIAN)
                {
                    if (this.biMoneda)
                    {
                        if (difML > difMaxML)
                        {
                            decimal nuevaBase = impuesto != 0? Math.Round((Math.Abs(valML) / impuesto) * 100, 2): 0;
                            string msgBase = string.Format(msgVals, nuevaBase);
                            string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = rsxField;
                            rdF.Message = msgBase;
                            rd.DetailsFields.Add(rdF);

                            createDTO = false;
                        }
                        if (difME > difMaxME)
                        {
                            decimal nuevaBase = impuesto != 0? Math.Round((Math.Abs(valME) / impuesto) * 100, 2): 0;
                            string msgBase = string.Format(msgVals, nuevaBase);
                            string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = rsxField;
                            rdF.Message = msgBase;
                            rd.DetailsFields.Add(rdF);

                            createDTO = false;
                        }
                    }
                    else if (this.monedaId == this.monedaLocal && difML > difMaxML)
                    {
                        decimal nuevaBase = impuesto != 0? Math.Round((Math.Abs(valML) / impuesto) * 100, 2): 0;
                        string msgBase = string.Format(msgVals, nuevaBase);
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgBase;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    else if (this.multiMoneda && this.monedaId == this.monedaExtranjera && difME > difMaxME)
                    {
                        decimal nuevaBase = impuesto != 0? Math.Round((Math.Abs(valME) / impuesto) * 100, 2): 0;
                        string msgBase = string.Format(msgVals, nuevaBase);
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgBase;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    } 
                }
            }

            #endregion
            #region Asigna los valores que deben ser calculados
            if (createDTO)
            {
                if (!this.multiMoneda)
                {
                    dto.vlrBaseME.Value = 0;
                    dto.vlrMdaExt.Value = 0;
                }

                if (cta.ImpuestoTipoID == null || string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
                {
                    dto.vlrBaseML.Value = 0;
                    dto.vlrBaseME.Value = 0;
                }

                dto.TasaCambio.Value = Math.Round(dto.TasaCambio.Value.Value, 2);
                dto.vlrBaseME.Value = Math.Round(dto.vlrBaseME.Value.Value, 2);
                dto.vlrMdaExt.Value = Math.Round(dto.vlrMdaExt.Value.Value, 2);
                //Valor de moneda extranjera
                dto.vlrBaseML.Value = Math.Round(dto.vlrBaseML.Value.Value, 2);
                dto.vlrMdaLoc.Value = Math.Round(dto.vlrMdaLoc.Value.Value, 2);

                if (this.biMoneda)
                {
                    //Valor de moneda local
                    dto.vlrBaseME.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseML.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaExt.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaLoc.Value / dto.TasaCambio.Value.Value;
                    //Valor de moneda extranjera
                    dto.vlrBaseML.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseME.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaLoc.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaExt.Value * dto.TasaCambio.Value.Value;

                    dto.vlrMdaOtr.Value = dto.vlrMdaLoc.Value;                
                }
                else if (this.monedaId == this.monedaLocal)
                {
                    dto.vlrBaseME.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseML.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaExt.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaLoc.Value / dto.TasaCambio.Value.Value;
                    dto.vlrMdaOtr.Value = dto.vlrMdaLoc.Value;
                }
                else if (this.multiMoneda && this.monedaId == this.monedaExtranjera)
                {
                    dto.vlrBaseML.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrBaseME.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaLoc.Value = dto.TasaCambio.Value.Value == 0 ? 0 : dto.vlrMdaExt.Value * dto.TasaCambio.Value.Value;
                    dto.vlrMdaOtr.Value = dto.vlrMdaExt.Value;
                }

                //Valor de moneda local
                dto.vlrBaseME.Value = Math.Round(dto.vlrBaseME.Value.Value, 2);
                dto.vlrMdaExt.Value = Math.Round(dto.vlrMdaExt.Value.Value, 2);
                dto.vlrMdaOtr.Value = Math.Round(dto.vlrMdaOtr.Value.Value, 2);
                //Valor de moneda extranjera
                dto.vlrBaseML.Value = Math.Round(dto.vlrBaseML.Value.Value, 2);
                dto.vlrMdaLoc.Value = Math.Round(dto.vlrMdaLoc.Value.Value, 2);
            }
            #endregion
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
            try
            {
                if (this._txtNumberFocus)
                {
                    this._txtNumberFocus = false;

                    if (this.txtNumber.Text == string.Empty)
                        this.txtNumber.Text = "0";

                    if (this.txtNumber.Text == "0")
                    {
                        #region Nuevo Comprobante
                        this.gcDocument.DataSource = null;
                        this.Comprobante = null;
                        if (!this.multiMoneda)
                        {
                            this.validHeader = true;
                            this.gcDocument.Focus();
                        }
                        #endregion
                    }
                    else
                    {
                        #region Comprobante existente
                        try
                        {
                            if (_bc.AdministrationModel.ComprobantePre_Exists(this.documentID, this.dtPeriod.DateTime, this.masterComprobante.Value, Convert.ToInt32(this.txtNumber.Text)))
                            {
                                DTO_Comprobante comp = _bc.AdministrationModel.Comprobante_Get(false, true, this.dtPeriod.DateTime, this.masterComprobante.Value, Convert.ToInt32(this.txtNumber.Text), null, null);
                                this.dtFecha.DateTime = comp.Header.Fecha.Value.Value;
                                this.cmbMonedaOrigen.SelectedItem = this.cmbMonedaOrigen.GetItem(comp.Header.MdaOrigen.Value.Value.ToString());
                                this.masterMoneda.Value = comp.Header.MdaTransacc.Value;
                                this._compExist = true;

                                if (this.AsignarTasaCambio(false))
                                {
                                    this.EnableHeader(false);

                                    this.Comprobante = comp;
                                    this.UpdateTemp(this.Comprobante);
                                    this.LoadData(true);

                                    this.validHeader = true;
                                    this.ValidHeaderTB();

                                    this.gcDocument.Focus();
                                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                                }
                                else
                                {
                                    this.validHeader = false;
                                }

                                //Asigna la revelacion si existe
                                revelacion = this._bc.AdministrationModel.DocumentoRevelacion_Get(comp.Header.NumeroDoc.Value.Value);

                            }
                            else
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ComprobantesCount));
                                this.txtNumber.Focus();
                                FormProvider.Master.itemSendtoAppr.Enabled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManual.cs", "txtNumber_Leave"));
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManuel.cs", "txtNumber_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la moneda origen
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbMonedaOrigen_Leave(object sender, EventArgs e)
        {
            if (this._cmbMonedaOrigenFocus)
            {
                this._cmbMonedaOrigenFocus = false;
                int cMonedaOrigen = -1;
                try
                {
                    cMonedaOrigen = Convert.ToInt32((this.cmbMonedaOrigen.SelectedItem as ComboBoxItem).Value);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidComboValue));
                    this.cmbMonedaOrigen.SelectedIndex = 0;
                    this.cmbMonedaOrigen.Focus();
                    return;
                }

                this.AsignarTasaCambio(false);
            }
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

                if(this.txtNumber.Text == string.Empty)
                    this.txtNumber.Text = "0";  
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "^\\d+$") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back )
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar del control de moneda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void cmbMonedaOrigen_Enter(object sender, EventArgs e)
        {
            var res = this.ValidateHeader();
            if (res)
                this._cmbMonedaOrigenFocus = true;
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
                if (!this._compExist)
                {
                    ModalRevelaciones modalRev = new ModalRevelaciones(coDoc.NotaRevelacionID.Value);
                    modalRev.ShowDialog();
                    this.revelacion = modalRev.DocRevelacion;
                }
                else
                {
                    if (revelacion != null)
                    {
                        ModalRevelaciones modalRev = new ModalRevelaciones(revelacion);
                        modalRev.ShowDialog();
                    }
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
                if (this.txtNumber.Text == "0")
                {                    
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }
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
                    if(this.Comprobante == null || this.Comprobante.Footer.Count == 0)
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
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManual.cs", "grlDocument_Enter: " + ex.Message));
                }

                #endregion
            }
            else
            {
                this.masterComprobante.Focus();
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
            {
                this.masterComprobante.Focus();
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
                    this.data = new DTO_Comprobante();
                    this.Comprobante = new DTO_Comprobante();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.Comprobante.Footer;
                    this.disableValidate = false;

                    this.CleanHeader(true);
                    this.masterComprobante.Focus();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManuel.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un comprobante
        /// </summary>
        public override void TBDelete()
        {
            base.TBDelete();
            try
            {
                if (this.ValidGrid())
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgDelDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Document);

                    if (MessageBox.Show(msgDelDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.gvDocument.ActiveFilterString = string.Empty;
                        
                        int compNro = Convert.ToInt32(this.txtNumber.Text);
                        if (compNro == 0)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                            return;
                        }
                        _bc.AdministrationModel.ComprobantePre_Delete(this.documentID, this._actFlujo.ID.Value, this.dtPeriod.DateTime, this.masterComprobante.Value, compNro);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CancelledComp));

                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        this.newDoc = true;
                        this.deleteOP = true;
                        this.Comprobante = new DTO_Comprobante();
                        this.gcDocument.DataSource = this.Comprobante.Footer;

                        this.CleanHeader(true);
                        this.EnableHeader(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManuel.cs", "TBDelete"));
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

        /// <summary>
        /// Funcion que se encarga de imprimir el Documento para su respectiva modificacion
        /// </summary>
        public override void TBPrint()
        {
            var numDoc = this.Comprobante.Header.NumeroDoc.Value.Value;
            if(numDoc != null)
                Process.Start(_bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null));


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

                result = _bc.AdministrationModel.ComprobantePre_Add(this.documentID, this.frmModule, this.Comprobante, this.areaFuncionalID, this.prefijoID, null, this.revelacion);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                this.Comprobante.Footer.RemoveAll(x => x.DatoAdd4.Value == AuxiliarDatoAdd4.Contrapartida.ToString()
                    || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambio.ToString()
                    || x.DatoAdd4.Value == AuxiliarDatoAdd4.AjEnCambioContra.ToString());

                if (result.Result.Equals(ResultValue.OK))
                {
                    string[] vars = Regex.Split(result.ResultMessage, "&&");
                    this.compNro = vars.ElementAt(2);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManual.cs", "SaveThread"));
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
                int compNro = Convert.ToInt32(this.txtNumber.Text);
                if (compNro == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NotDeleteComp));
                    return;
                }

                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                
                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = _bc.AdministrationModel.ComprobantePre_SendToAprob(this.documentID, this._actFlujo.ID.Value, this.frmModule, this.dtPeriod.DateTime, this.masterComprobante.Value, compNro, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                #region Genera Reporte

                if (obj.GetType() == typeof(DTO_Alarma))
                {
                    string numDoc = ((DTO_Alarma)obj).NumeroDoc;
                    bool finaliza = ((DTO_Alarma)obj).Finaliza;
                    reportName = this._bc.AdministrationModel.ReportesContabilidad_ComprobanteManual(Convert.ToInt32(numDoc), finaliza, true, ExportFormatType.pdf);

                    if (reportName == string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoSeGeneroReporte));                    
                    else
                    {
                        if(finaliza)
                        { 
                            string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(numDoc), null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                    }
                    
                }

                #endregion

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.Comprobante = new DTO_Comprobante();
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ComprobanteManuel.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
       
    }
}
