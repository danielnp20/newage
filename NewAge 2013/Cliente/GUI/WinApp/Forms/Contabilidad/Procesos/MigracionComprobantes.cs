using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigracionComprobantes : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.btnTemplate.Enabled = true;
            this.btnImport.Enabled = true; 

            if (this._isOK)
                this.btnGenerar.Enabled = true;
            else
                this.btnGenerar.Enabled = false;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.btnTemplate.Enabled = true;
            this.btnImport.Enabled = true;
            if (this._isOK)
                this.btnGenerar.Enabled = false;
            else
                this.btnGenerar.Enabled = true;
        }

        #endregion

        //private MigracionComprobantes()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private bool multiMoneda;
        private string areaFuncionalID;
        private string prefijoID;
        private PasteOpDTO pasteRet;
        //Variables para importar
        private string format;
        private string formatSeparator = "\t";
        //Variables con los recursos de las Fks
        private string _libroRsx = string.Empty;
        private string _comprobanteRsx = string.Empty;
        private string _monedaRsx = string.Empty;
        private string _cuentaRsx = string.Empty;
        private string _terceroRsx = string.Empty;
        private string _prefijoRsx = string.Empty;
        private string _proyectoRsx = string.Empty;
        private string _centroCostoRsx = string.Empty;
        private string _lineaPresupRsx = string.Empty;
        private string _conceptoCargoRsx = string.Empty;
        private string _lugarGeoRsx = string.Empty;
        //Variables de validacion
        private bool _validCtaProyCC;
        //Variables de comprobante
        private string monedaLocal;
        private string monedaExtranjera;
        private string monedaId;
        private bool biMoneda = false;
        private decimal validDif;
        //Variables con valores x defecto (glControl)
        private string defTercero = string.Empty;
        private string defPrefijo = string.Empty;
        private string defProyecto = string.Empty;
        private string defCentroCosto = string.Empty;
        private string defLineaPresupuesto = string.Empty;
        private string defConceptoCargo = string.Empty;
        private string defLugarGeo = string.Empty;
        //Libros
        private string libroFunc = string.Empty;
        private string libroIFRS = string.Empty;
        //Variables del formulario
        private bool _isOK;
        private List<DTO_Comprobante> _data;
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.MigracionComprobantes;

                this.InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = false;
                this._data = new List<DTO_Comprobante>();

                //Funciones para iniciar el formulario
                this.InitVars();
                this.AssignPeriod();
                this.AssignFormat();
                this.CleanFormat();

                //Carga los libros
                this.libroFunc = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                this.libroIFRS = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceIFRS);

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(libroFunc, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroFunc"));
                dic.Add(libroIFRS, this._bc.GetResource(LanguageTypes.Tables, "11009_tbl_LibroIFRS"));
                
                this.cmbLibro.Properties.DataSource = dic;
                this.cmbLibro.EditValue = this.libroFunc;

                //Indica si debe validae la relacion proy-Cta-CC
                string v = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ValidCta_Operacion);
                this._validCtaProyCC = v == "1" ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las variables
        /// </summary>
        private void InitVars()
        {
            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            this.validDif = Convert.ToDecimal(_bc.GetControlValue(AppControl.DiferenciaContablePermitida), CultureInfo.InvariantCulture);
            //Carga los valores por defecto
            this.defTercero = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.defPrefijo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.defConceptoCargo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            this.defLugarGeo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            //Carga los recursos de las Fks
            this._libroRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LibroID");
            this._comprobanteRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComprobanteID");
            this._monedaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MdaTransacc");
            this._cuentaRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            this._terceroRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
            this._prefijoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefijoCOM");
            this._proyectoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            this._centroCostoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            this._lineaPresupRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            this._conceptoCargoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoCargoID");
            this._lugarGeoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LugarGeograficoID");
            //Carga las variables globales
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, AppDocuments.ComprobanteManual);
        }

        /// <summary>
        /// Asigna el periodo abierto al formulario
        /// </summary>
        private void AssignPeriod()
        {
            _bc.InitPeriodUC(this.dtPeriod, 2);
            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
            DateTime p = Convert.ToDateTime(periodo);
            this.dtPeriod.DateTime = p;

            this.dtPeriod.Enabled = false;
        }

        /// <summary>
        /// Asigna el formato de importacion
        /// </summary>
        private void AssignFormat()
        {
            string headerFormat = _bc.GetImportExportFormat(typeof(DTO_ComprobanteHeader), this.documentID);
            string footerFormat = _bc.GetImportExportFormat(typeof(DTO_ComprobanteFooter), this.documentID);

            this.format = headerFormat + this.formatSeparator + footerFormat;
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        private void CleanFormat()
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
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "DatoAdd10") &&
                    col != _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "CuentaAlternaID"))
                {
                    if (col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrBaseME") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "vlrMdaExt") ||
                        col == _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambio"))
                    {
                        if (this.multiMoneda)
                            f += col + this.formatSeparator;
                    }
                    else
                        f += col + this.formatSeparator;
                }
            }

            this.format = f;
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        private int GetMasterDocumentID(string colName)
        {
            //Libro
            if (colName == this._libroRsx)
                return AppMasters.coBalanceTipo;
            //Comprobante
            if (colName == this._comprobanteRsx)
                return AppMasters.coComprobante;
            //Moneda
            if (colName == this._monedaRsx)
                return AppMasters.glMoneda;
            //Cuenta
            if (colName == this._cuentaRsx)
                return AppMasters.coPlanCuenta;
            //Tercero
            if (colName == this._terceroRsx)
                return AppMasters.coTercero;
            //Prefijo
            if (colName == this._prefijoRsx)
                return AppMasters.glPrefijo;
            //Proyecto
            if (colName == this._proyectoRsx)
                return AppMasters.coProyecto;
            //Cwentro Costo
            if (colName == this._centroCostoRsx)
                return AppMasters.coCentroCosto;
            //Linea presupuestal
            if (colName == this._lineaPresupRsx)
                return AppMasters.plLineaPresupuesto;
            //Concepto Cargo
            if (colName == this._conceptoCargoRsx)
                return AppMasters.coConceptoCargo;
            //Lugar Geografico
            if (colName == this._lugarGeoRsx)
                return AppMasters.glLugarGeografico;

            return 0;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="cta">Cuenta del detalle</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgVals">Mensaje paralos valores incorrectos</param>
        private bool ValidateDataImport(DTO_ComprobanteFooter dto, DTO_coPlanCuenta cta, DTO_TxResultDetail rd, string msgVals)
        {
            bool createDTO = true;
            decimal impuesto = 0;
            #region Asignacion de tasa de cambio
            if (!this.multiMoneda)
                dto.TasaCambio.Value = 0;
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
                impuesto = cta.ImpuestoPorc.Value.Value;
            #endregion
            #region Validacion de Valores

            if (cta.ImpuestoTipoID != null && !string.IsNullOrEmpty(cta.ImpuestoTipoID.Value))
            {
                if (!this.multiMoneda)
                {
                    dto.vlrBaseME.Value = 0;
                    dto.vlrMdaExt.Value = 0;
                }

                decimal impRealML = Math.Round(dto.vlrBaseML.Value.Value * impuesto / 100);
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

                if (this.biMoneda)
                {
                    if (Math.Abs(difML) > Math.Abs(difMaxML))
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgVals;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                    if (Math.Abs(difME) > Math.Abs(difMaxME))
                    {
                        string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = rsxField;
                        rdF.Message = msgVals;
                        rd.DetailsFields.Add(rdF);

                        createDTO = false;
                    }
                }
                else if (this.monedaId == this.monedaLocal && Math.Abs(difML) > Math.Abs(difMaxML))
                {
                    string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = rsxField;
                    rdF.Message = msgVals;
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
                }
                else if (this.multiMoneda && this.monedaId == this.monedaExtranjera && Math.Abs(difME) > Math.Abs(difMaxME))
                {
                    string rsxField = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = rsxField;
                    rdF.Message = msgVals;
                    rd.DetailsFields.Add(rdF);

                    createDTO = false;
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
                dto.vlrMdaOtr.Value = 0;

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


            return createDTO;
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto calculando los valores correspondientes
        /// </summary>
        private DTO_TxResult ValidarTotal(DTO_Comprobante comp)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                byte monOr = Convert.ToByte(comp.Header.MdaOrigen.Value.Value);
                decimal sumBaseLoc = 0;
                decimal sumBaseExt = 0;
                decimal sumLocal = 0;
                decimal sumExtran = 0;
                decimal credlocal = 0;
                decimal credextran = 0;

                List<DTO_ComprobanteFooter> footer = comp.Footer;
                for (int i = 0; i < footer.Count; i++)
                {
                    DTO_ComprobanteFooter det = footer[i];
                    decimal vlrBaseLoc = det.vlrBaseML.Value.Value;
                    decimal vlrBaseExt = det.vlrBaseME.Value.Value;
                    decimal vlrLocal = det.vlrMdaLoc.Value.Value;
                    decimal vlrExtran = det.vlrMdaExt.Value.Value;

                    if (vlrLocal < 0)
                        credlocal += vlrLocal;

                    if (vlrExtran < 0)
                        credextran += vlrExtran;

                    sumBaseLoc += vlrBaseLoc;
                    sumBaseExt += vlrBaseExt;
                    sumLocal += vlrLocal;
                    sumExtran += vlrExtran;
                }

                sumBaseLoc = Math.Round(sumBaseLoc, 2);
                sumBaseExt = Math.Round(sumBaseExt, 2);
                sumLocal = Math.Round(sumLocal, 2);
                sumExtran = Math.Round(sumExtran, 2);

                //Si el comprobante permite 2 monedas las 2 deben estar cuadradas en 0
                if (this.biMoneda && (sumLocal != 0 || sumExtran != 0))
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotalBiMoneda);
                    return result;
                }

                if (monOr == (int)TipoMoneda.Local)
                {
                    //Si es moneda local varifica qe la suma local sea 0 y que la ext no sea mayor quela diferencia permitida
                    if (sumLocal != 0 || Math.Abs(sumExtran) > validDif)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal);
                        return result;
                    }
                    else if (sumExtran < 0)
                        comp.Footer.Last().vlrMdaExt.Value = comp.Footer.Last().vlrMdaExt.Value.Value + (sumExtran * -1);
                    else if (sumExtran > 0)
                        comp.Footer.Last().vlrMdaExt.Value = comp.Footer.Last().vlrMdaExt.Value.Value - sumExtran;
                }
                else
                {
                    //Si es moneda extranjera varifica qe la suma ext sea 0 y que la local no sea mayor quela diferencia permitida
                    if (sumExtran != 0 || Math.Abs(sumLocal) > validDif)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_InvalidTotal);
                        return result;
                    }
                    else if (sumLocal < 0)
                        comp.Footer.Last().vlrMdaLoc.Value = comp.Footer.Last().vlrMdaLoc.Value + sumExtran;
                    else if (sumLocal > 0)
                        comp.Footer.Last().vlrMdaLoc.Value = comp.Footer.Last().vlrMdaLoc.Value - sumExtran;
                }

                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp", "ValidarTotal"));
                result.Result = ResultValue.NOK;
                return result;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtPeriod_EditValueChanged()
        {
            try
            {
                EstadoPeriodo validPeriod = _bc.AdministrationModel.CheckPeriod(this.dtPeriod.DateTime, ModulesPrefix.co);
                if (validPeriod != EstadoPeriodo.Abierto)
                {
                    if (validPeriod == EstadoPeriodo.Cerrado)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoCerrado));
                    if (validPeriod == EstadoPeriodo.EnCierre)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoEnCierre));

                    this.dtPeriod.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Procesa el cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "btnTemplate_Click"));
            }
        }

        /// <summary>
        /// Procesa el cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            this.btnImport.Enabled = false;
            this.btnTemplate.Enabled = false;
            this.btnGenerar.Enabled = false;
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarComprobantes_Click(object sender, EventArgs e)
        {
            try
            {
                List<DTO_TxResult> results = new List<DTO_TxResult>();
                List<DTO_TxResultDetail> details;
                bool validTotal = true;

                #region Valida los totales
                int i = 1;
                foreach (DTO_Comprobante comp in this._data)
                {
                    this.monedaId = comp.Header.MdaTransacc.Value;
                    int mdaOrigen = this.monedaId == this.monedaLocal ? (byte)TipoMoneda.Local : (byte)TipoMoneda.Foreign;

                    DTO_TxResult result = this.ValidarTotal(comp);
                    if (result.Result == ResultValue.NOK)
                    {
                        DTO_TxResultDetail det = new DTO_TxResultDetail();
                        det.line = i;
                        det.Message = result.ResultMessage;
                        details = new List<DTO_TxResultDetail>();
                        details.Add(det);

                        result.ResultMessage = _comprobanteRsx + ": (" + comp.Header.ComprobanteID.Value + "-" + comp.Header.ComprobanteNro.Value.Value + ")";
                        result.Details = details;
                        validTotal = false;
                    }

                    i = i + comp.Footer.Count;
                    results.Add(result);
                }

                if (!validTotal)
                {
                    MessageForm frm = new MessageForm(results);
                    frm.ShowDialog();
                    return;
                }
                #endregion

                this.btnImport.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnGenerar.Enabled = false;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "btnGenerarComprobantes_Click"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de cierre
        /// </summary>
        private void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    List<DTO_Comprobante> list = new List<DTO_Comprobante>();
                    List<DTO_Comprobante> listFinal = new List<DTO_Comprobante>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    Dictionary<string, Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>> ctas = new Dictionary<string, Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgCompInvalidNro = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompInvalidCompNro);
                    string msgCompInvalidMda = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompInvalidMda);
                    string msgCompInvalidTC = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompInvalidTC);
                    string msgCompInvalidDate = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompInvalidDate);
                    string msgCompInvalidLibro = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CompInvalidLibro); 
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    string msgVals = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidImpValue);
                    string msgCtaCargoProy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                    //Popiedades de un comprobante
                    DTO_Comprobante comprobante = new DTO_Comprobante();
                    DTO_ComprobanteHeader header = new DTO_ComprobanteHeader();
                    DTO_ComprobanteFooter footerDet = new DTO_ComprobanteFooter();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<PropertyInfo> pisHeader = typeof(DTO_ComprobanteHeader).GetProperties().ToList();
                    List<PropertyInfo> pisFooter = typeof(DTO_ComprobanteFooter).GetProperties().ToList();
                    List<PropertyInfo> pis = pisHeader;
                    pis.AddRange(pisFooter);

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    //Fks
                    fks.Add(this._libroRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._comprobanteRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._monedaRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._cuentaRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._terceroRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._prefijoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._proyectoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._centroCostoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._lineaPresupRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._conceptoCargoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._lugarGeoRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];

                                    //Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        if (colRsx == this._libroRsx ||
                                            colRsx == this._comprobanteRsx ||
                                            colRsx == this._monedaRsx ||
                                            colRsx == this._cuentaRsx ||
                                            colRsx == this._terceroRsx ||
                                            colRsx == this._prefijoRsx ||
                                            colRsx == this._proyectoRsx ||
                                            colRsx == this._centroCostoRsx ||
                                            colRsx == this._lineaPresupRsx ||
                                            colRsx == this._conceptoCargoRsx ||
                                            colRsx == this._lugarGeoRsx)
                                        {
                                            #region Carga la info de la FK
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                                continue;
                                            else if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                #region Revisa si la FK tiene datoas validos
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                    #region Asigna los valores de las cuentas
                                                    if (colRsx == _cuentaRsx)
                                                    {
                                                        DTO_coPlanCuenta cta = (DTO_coPlanCuenta)dto;
                                                        DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, cta.ConceptoSaldoID.Value, true);
                                                        Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo> tup = new Tuple<DTO_coPlanCuenta, DTO_glConceptoSaldo>(cta, cSaldo);
                                                        ctas.Add(line[colIndex].Trim(), tup);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Carga la innfo para ver si el registro es invalido
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                    #endregion
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    //revisa si es header o footer
                                    if (colIndex == 0)
                                        header = new DTO_ComprobanteHeader();
                                    else if (colIndex == 5)
                                        footerDet = new DTO_ComprobanteFooter();

                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                                (colRsx == this._libroRsx ||
                                                colRsx == this._comprobanteRsx ||
                                                colRsx == this._monedaRsx ||
                                                colRsx == this._cuentaRsx ||
                                                colRsx == this._terceroRsx ||
                                                colRsx == this._prefijoRsx ||
                                                colRsx == this._proyectoRsx ||
                                                colRsx == this._centroCostoRsx ||
                                                colRsx == this._lineaPresupRsx ||
                                                colRsx == this._conceptoCargoRsx ||
                                                colRsx == this._lugarGeoRsx ||
                                                colName == "Fecha" ||
                                                colName == "Descriptivo" ||
                                                colName == "TasaCambio" ||
                                                colName == "vlrBaseML" ||
                                                colName == "vlrBaseME" ||
                                                colName == "vlrMdaLoc" ||
                                                colName == "vlrMdaExt")
                                        )
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos

                                        UDT udt;
                                        PropertyInfo pi = header.GetType().GetProperty(colName);
                                        if (pi != null)
                                            udt = (UDT)pi.GetValue(header, null);
                                        else
                                        {
                                            pi = footerDet.GetType().GetProperty(colName);
                                            udt = (UDT)pi.GetValue(footerDet, null);
                                        }
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                        }

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        } //validacion del formato
                                        #endregion

                                        //Si paso las validaciones asigne el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionComprobantes.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {
                                comprobante = new DTO_Comprobante();
                                comprobante.Header = header;
                                comprobante.Footer = new List<DTO_ComprobanteFooter>();
                                comprobante.Footer.Add(footerDet);

                                list.Add(comprobante);
                            }
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares del comprobante
                    if (validList)
                    {
                        #region Variables Generales
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        //Variables para carga de comprobantes completos (compID, CompNro)
                        Dictionary<Tuple<string, int>, int> comps = new Dictionary<Tuple<string, int>, int>();
                        Dictionary<string, int> compsConsecutivo = new Dictionary<string, int>();
                        //Diccionario que tiene la moneda original de un comprobante en una fecha
                        Dictionary<int, DateTime> cacheFechas = new Dictionary<int, DateTime>();
                        Dictionary<int, string> cacheMdas = new Dictionary<int, string>();
                        Dictionary<int, decimal> cacheTC = new Dictionary<int, decimal>();
                        Tuple<string, int> lastPK = new Tuple<string, int>("*", 0);
                        bool newComp = false;
                        comprobante = null;
                        #endregion
                        foreach (DTO_Comprobante comp in list)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (list.Count);
                            i++;
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;

                            //Variables del comprobante
                            this.monedaId = comp.Header.MdaTransacc.Value;
                            DateTime fechaComp = comp.Header.Fecha.Value.Value;
                            string libroID = comp.Header.LibroID.Value;
                            DTO_ComprobanteFooter footerFirst = comp.Footer.First();
                            Tuple<string, int> pk = new Tuple<string, int>(comp.Header.ComprobanteID.Value, comp.Header.ComprobanteNro.Value.Value);
                            #endregion
                            #region Validación del comprobante
                            if (!compsConsecutivo.ContainsKey(pk.Item1))
                            {
                                //Validación de consecutivos
                                newComp = true;
                                if (comprobante != null)
                                    listFinal.Add(comprobante);
                                comps.Add(pk, i);

                                #region Valida que la fecha se encuentre en el periodo
                                if (fechaComp.Year != this.dtPeriod.DateTime.Year || fechaComp.Month != this.dtPeriod.DateTime.Month)
                                {
                                    rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                                    rdF.Message = string.Format(msgCompInvalidDate, pk.Item1 + "-" + pk.Item2.ToString());
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                                #endregion
                                #region Valida el libro
                                if (libroID != this.cmbLibro.EditValue.ToString())
                                {
                                    rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LibroID");
                                    rdF.Message = string.Format(msgCompInvalidLibro, pk.Item1 + "-" + pk.Item2.ToString());//, comps[pk]);
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                                #endregion
                                #region Inicializa los diccionarios del comprobante
                                compsConsecutivo.Add(pk.Item1, pk.Item2);
                                cacheFechas.Add(i, fechaComp);
                                cacheMdas.Add(i, comp.Header.MdaTransacc.Value);
                                if(_bc.AdministrationModel.MultiMoneda)
                                    cacheTC.Add(i, footerFirst.TasaCambio.Value.Value);
                                #endregion
                            }
                            else if (compsConsecutivo[pk.Item1] == 0)
                            {
                                newComp = false;
                                createDTO = false;
                            }
                            else if (!comps.ContainsKey(pk))
                            {
                                comps.Add(pk, i);
                                #region Inicializa los diccionarios del comprobante
                                cacheFechas.Add(i, fechaComp);
                                cacheMdas.Add(i, comp.Header.MdaTransacc.Value);
                                if (_bc.AdministrationModel.MultiMoneda)
                                    cacheTC.Add(i, footerFirst.TasaCambio.Value.Value);
                                #endregion
                                #region Valida que la fecha se encuentre en el periodo
                                if (fechaComp.Year != this.dtPeriod.DateTime.Year || fechaComp.Month != this.dtPeriod.DateTime.Month)
                                {
                                    rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                                    rdF.Message = string.Format(msgCompInvalidDate, pk.Item1 + "-" + pk.Item2.ToString(), comps[pk]);
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                                #endregion
                                #region Valida que el consecutivo sea el correspondiente
                                if (pk.Item2 != compsConsecutivo[pk.Item1] + 1)
                                {
                                    rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComprobanteNro");
                                    rdF.Message = string.Format(msgCompInvalidNro, pk.Item1);
                                    rd.DetailsFields.Add(rdF);

                                    createDTO = false;
                                }
                                else
                                    compsConsecutivo[pk.Item1] += 1;
                                #endregion
                                #region Crea un nuevo comprobante en la lista final
                                newComp = true;
                                if (createDTO && comprobante != null)
                                    listFinal.Add(comprobante);
                                #endregion
                            }
                            else
                            {
                                int lnOrigen = comps[pk];
                                int lastConsecutivo = compsConsecutivo[pk.Item1];
                                #region Revisa informacion del comprobante original
                                if (lastPK.Item1 == pk.Item1 && lastPK.Item2 == pk.Item2)
                                {
                                    newComp = false;
                                    #region Revisa que la fecha sea la misma del comprobante original
                                    DateTime fechaOrigen = cacheFechas[lnOrigen];
                                    if (fechaOrigen != comp.Header.Fecha.Value.Value)
                                    {
                                        rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                                        rdF.Message = string.Format(msgCompInvalidDate, pk.Item1 + "-" + pk.Item2.ToString(), lnOrigen.ToString());
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que la moneda de origen sea la misma del comprobante original
                                    string mdaOrigen = cacheMdas[lnOrigen];
                                    if (mdaOrigen != comp.Header.MdaTransacc.Value)
                                    {
                                        rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _monedaRsx;
                                        rdF.Message = string.Format(msgCompInvalidMda, pk.Item1 + "-" + pk.Item2.ToString(), lnOrigen.ToString());
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                    #region Revisa que la tasa de cambio de origen sea la misma del comprobante original
                                    if (mdaOrigen != comp.Header.MdaTransacc.Value)
                                    {
                                        rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TasaCambio");
                                        rdF.Message = string.Format(msgCompInvalidTC, pk.Item1 + "-" + pk.Item2.ToString(), lnOrigen.ToString());
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion
                            #region Carga la info para un nuevo comprobante
                            if (createDTO && newComp)
                            {
                                this.monedaId = comp.Header.MdaTransacc.Value;
                                comprobante = new DTO_Comprobante();

                                //Completa la info del header
                                comp.Header.PeriodoID.Value = this.dtPeriod.DateTime;
                                comp.Header.MdaOrigen.Value = this.monedaId == this.monedaLocal ? (byte)TipoMoneda.Local : (byte)TipoMoneda.Foreign;
                                comp.Header.TasaCambioBase.Value = this.multiMoneda ? footerDet.TasaCambio.Value.Value : 0;
                                comp.Header.TasaCambioOtr.Value = header.TasaCambioBase.Value;
                                comprobante.Header = comp.Header;
                                //Crea la lista del detalle
                                comprobante.Footer = new List<DTO_ComprobanteFooter>();
                            }
                            else if (!createDTO)
                                comprobante = null;
                            #endregion
                            #region Valida el DTO y el control de saldos
                            if (createDTO)
                            {
                                DTO_coPlanCuenta cta = ctas[footerFirst.CuentaID.Value].Item1;
                                DTO_glConceptoSaldo cSaldo = ctas[footerFirst.CuentaID.Value].Item2;
                                footerFirst.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
                                #region Validaciones particulares del documento al importar del DTO

                                createDTO = this.ValidateDataImport(footerFirst, cta, rd, msgVals);

                                #endregion
                                #region Validacion segun el control de saldos
                                switch (cSaldo.coSaldoControl.Value)
                                {
                                    case (int)SaldoControl.Cuenta:
                                        #region Por Cuenta
                                        #region Tercero
                                        if (!cta.TerceroInd.Value.Value)
                                            footerFirst.TerceroID.Value = this.defTercero;
                                        else if (string.IsNullOrWhiteSpace(footerFirst.TerceroID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _terceroRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Proyecto
                                        if (!cta.ProyectoInd.Value.Value)
                                            footerFirst.ProyectoID.Value = this.defProyecto;
                                        else if (string.IsNullOrWhiteSpace(footerFirst.ProyectoID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _proyectoRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Centro Costo
                                        if (!cta.CentroCostoInd.Value.Value)
                                            footerFirst.CentroCostoID.Value = this.defCentroCosto;
                                        else if (string.IsNullOrWhiteSpace(footerFirst.CentroCostoID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _centroCostoRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Linea presupuesto
                                        if (!cta.LineaPresupuestalInd.Value.Value)
                                            footerFirst.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                        else if (string.IsNullOrWhiteSpace(footerFirst.LineaPresupuestoID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _lineaPresupRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Concepto Cargo
                                        if (!cta.ConceptoCargoInd.Value.Value)
                                            footerFirst.ConceptoCargoID.Value = this.defConceptoCargo;
                                        else if (cta.ConceptoCargoID != null && !string.IsNullOrWhiteSpace(cta.ConceptoCargoID.Value))
                                            footerFirst.ConceptoCargoID.Value = cta.ConceptoCargoID.Value;

                                        //Hace la validacion con la tabla de costos de cargos
                                        if (cta.ConceptoCargoInd.Value.Value)
                                        {
                                            if (cta.ProyectoInd.Value.Value)
                                            {
                                                string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(footerFirst.ConceptoCargoID.Value, footerFirst.ProyectoID.Value, string.Empty, footerFirst.LineaPresupuestoID.Value).Trim();
                                                if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != cta.ID.Value)
                                                {
                                                    rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = _proyectoRsx + " - " + _conceptoCargoRsx;
                                                    rdF.Message = msgCtaCargoProy;
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            else if (cta.CentroCostoInd.Value.Value)
                                            {
                                                string ctaCargoCosto = _bc.AdministrationModel.coCargoCosto_GetCuentaIDByCargoProy(footerFirst.ConceptoCargoID.Value, string.Empty, footerFirst.CentroCostoID.Value, footerFirst.LineaPresupuestoID.Value).Trim();
                                                if (string.IsNullOrEmpty(ctaCargoCosto) || ctaCargoCosto != cta.ID.Value)
                                                {
                                                    rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = _proyectoRsx + " - " + _conceptoCargoRsx;
                                                    rdF.Message = msgCtaCargoProy;
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }

                                        #endregion
                                        #region Lugar Geografico
                                        if (!cta.LugarGeograficoInd.Value.Value)
                                            footerFirst.LugarGeograficoID.Value = this.defLugarGeo;
                                        else if (string.IsNullOrWhiteSpace(footerFirst.LugarGeograficoID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _lugarGeoRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Prefijo
                                        footerFirst.PrefijoCOM.Value = this.defPrefijo;
                                        #endregion
                                        #region Documento
                                        if (string.IsNullOrWhiteSpace(footerFirst.DocumentoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        footerFirst.IdentificadorTR.Value = 0;
                                        #endregion
                                        break;
                                    case (int)SaldoControl.Doc_Interno:
                                        #region Documento Interno
                                        footerFirst.ConceptoCargoID.Value = this.defConceptoCargo;
                                        footerFirst.LugarGeograficoID.Value = this.defLugarGeo;

                                        #region Revisa que haya documento
                                        if (string.IsNullOrWhiteSpace(footerFirst.DocumentoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Revisa que tenga prefijo
                                        if (string.IsNullOrWhiteSpace(footerFirst.PrefijoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _prefijoRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Revisa que el documento sea numerico
                                        if (createDTO)
                                        {
                                            try
                                            {
                                                int dInt = Convert.ToInt32(footerFirst.DocumentoCOM.Value);
                                            }
                                            catch (Exception)
                                            {
                                                rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                                rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NumericDocInt);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                        }
                                        #endregion
                                        #region Trae la info del documento
                                        if (createDTO)
                                        {
                                            int docIntId = Convert.ToInt32(footerFirst.DocumentoCOM.Value);
                                            DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetInternalDocByCta(footerFirst.CuentaID.Value, footerFirst.PrefijoCOM.Value, docIntId);

                                            if (docCtrl == null)
                                            {
                                                rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                                rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                footerFirst.TerceroID.Value = docCtrl.TerceroID.Value;
                                                footerFirst.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                                footerFirst.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                                footerFirst.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                                footerFirst.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                            }

                                        }
                                        #endregion
                                        #endregion
                                        break;
                                    case (int)SaldoControl.Doc_Externo:
                                        #region Documento externo
                                        footerFirst.ConceptoCargoID.Value = this.defConceptoCargo;
                                        footerFirst.LugarGeograficoID.Value = this.defLugarGeo;

                                        #region Revisa que haya documento
                                        if (string.IsNullOrWhiteSpace(footerFirst.DocumentoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Revisa que tenga tercero
                                        if (string.IsNullOrWhiteSpace(footerFirst.TerceroID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _terceroRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Trae la info del documento
                                        if (createDTO)
                                        {
                                            DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(footerFirst.CuentaID.Value, footerFirst.TerceroID.Value, footerFirst.DocumentoCOM.Value);

                                            if (docCtrl == null)
                                            {
                                                rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                                rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                footerFirst.TerceroID.Value = docCtrl.TerceroID.Value;
                                                footerFirst.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                                footerFirst.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                                footerFirst.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                                footerFirst.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                            }

                                        }
                                        #endregion
                                        #endregion
                                        break;
                                    case(int)SaldoControl.Componente_Tercero:
                                        footerFirst.IdentificadorTR.Value = Convert.ToInt32(footerFirst.TerceroID.Value);
                                        DTO_coCuentaSaldo saldo = _bc.AdministrationModel.Saldo_GetByDocumento(cta.ID.Value, cSaldo.ID.Value, Convert.ToInt32(footerFirst.IdentificadorTR.Value.Value), string.Empty);
                                        if (saldo != null)
                                        {
                                            footerFirst.ProyectoID.Value = saldo.ProyectoID.Value;
                                            footerFirst.CentroCostoID.Value = saldo.CentroCostoID.Value;
                                            footerFirst.LineaPresupuestoID.Value = saldo.LineaPresupuestoID.Value;
                                            footerFirst.ConceptoCargoID.Value = saldo.ConceptoCargoID.Value;
                                        }
                                        break;
                                    case (int)SaldoControl.Componente_Documento:
                                        #region Componente Documento
                                        footerFirst.ConceptoCargoID.Value = this.defConceptoCargo;
                                        footerFirst.LugarGeograficoID.Value = this.defLugarGeo;

                                        #region Revisa que haya documento
                                        if (string.IsNullOrWhiteSpace(footerFirst.DocumentoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Revisa que tenga tercero
                                        if (string.IsNullOrWhiteSpace(footerFirst.TerceroID.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _terceroRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Trae la info del documento
                                        if (createDTO)
                                        {
                                            DTO_glDocumentoControl docCtrl = _bc.AdministrationModel.glDocumentoControl_GetExternalDocByCta(footerFirst.CuentaID.Value, footerFirst.TerceroID.Value, footerFirst.DocumentoCOM.Value);

                                            if (docCtrl == null)
                                            {
                                                rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoCOM");
                                                rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                footerFirst.TerceroID.Value = docCtrl.TerceroID.Value;
                                                footerFirst.ProyectoID.Value = docCtrl.ProyectoID.Value;
                                                footerFirst.CentroCostoID.Value = docCtrl.CentroCostoID.Value;
                                                footerFirst.LineaPresupuestoID.Value = docCtrl.LineaPresupuestoID.Value;
                                                footerFirst.IdentificadorTR.Value = docCtrl.NumeroDoc.Value.Value;
                                            }

                                        }
                                        #endregion
                                        #endregion
                                        break;
                                    case (int)SaldoControl.Componente_Activo:
                                        #region Activo
                                        footerFirst.ConceptoCargoID.Value = this.defConceptoCargo;
                                        footerFirst.LugarGeograficoID.Value = this.defLugarGeo;

                                        #region Revisa que tenga activo
                                        if (string.IsNullOrWhiteSpace(footerFirst.ActivoCOM.Value))
                                        {
                                            rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Trae la info del activo
                                        if (createDTO)
                                        {
                                            DTO_acActivoControl acCtrl = _bc.AdministrationModel.acActivoControl_GetByPlaqueta(footerFirst.ActivoCOM.Value);
                                            if (acCtrl == null)
                                            {
                                                rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivoCOM");
                                                rdF.Message = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                footerFirst.TerceroID.Value = acCtrl.TerceroID.Value;
                                                footerFirst.PrefijoCOM.Value = defPrefijo;
                                                footerFirst.DocumentoCOM.Value = footerFirst.ActivoCOM.Value;
                                                footerFirst.ProyectoID.Value = acCtrl.ProyectoID.Value;
                                                footerFirst.CentroCostoID.Value = acCtrl.CentroCostoID.Value;
                                                footerFirst.LineaPresupuestoID.Value = defLineaPresupuesto;
                                                footerFirst.IdentificadorTR.Value = acCtrl.ActivoID.Value.Value;
                                            }
                                        }
                                        #endregion
                                        #endregion
                                        break;
                                }
                                #endregion
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && comprobante != null && validList)
                                comprobante.Footer.Add(footerFirst);
                            else
                                validList = false;
                            #endregion
                            lastPK = pk;
                        }
                    }
                    #endregion
                    #region Carga la informacion en la lista de importacion
                    if (validList)
                    {
                        //Agrega el ultimo comprobante
                        if (comprobante != null)
                            listFinal.Add(comprobante);

                        this._isOK = true;
                        this._data = listFinal;
                    }
                    else
                    {
                        this._isOK = false;
                        this._data = null;
                    }

                    MessageForm frm = new MessageForm(result);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                    #endregion
                }
                else
                {
                    this._isOK = false;

                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "ImportThread"));
            }
            finally
            {
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 }); 
                this.StopProgressBarThread();
                this.Invoke(this.endImportarDelegate);
            }
        }

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                List<DTO_TxResult> results = _bc.AdministrationModel.Comprobante_Migracion(this.documentID, this.dtPeriod.DateTime.Date, this._data, this.areaFuncionalID, 
                    this.prefijoID, false);

                this.StopProgressBarThread();

                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                this._isOK = true;
                foreach (DTO_TxResult result in results)
                {
                    if (result.Result == ResultValue.NOK)
                    {
                        resultsNOK.Add(result);
                        this._isOK = false;
                    }
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionComprobantes.cs", "MigracionComprobante.cs-btnProcesar_Click"));
            }
            finally
            {
                this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });
                this.StopProgressBarThread();
                this.Invoke(this.endProcesarDelegate);
            }
        }

        #endregion

    }
}
