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
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigracionCxP : ProcessForm
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

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private int documentMigracionID;
        private string areaFuncionalID;
        private PasteOpDTO pasteRet;
        //Variables para importar
        private int colsCount;
        private string firstColSuppl;
        private string format;
        private string formatSeparator = "\t";
        //Variables de monedas
        private bool isMultiMoneda;
        private string monedaLocal;
        private string monedaExtranjera;
        private decimal tasaCierre;
        private bool hasTasaCierre;
        private decimal valorTotal = 0;
        //Variables del formulario
        private bool _isOK;
        private List<DTO_glDocumentoControl> _ctrlList;
        private List<DTO_cpCuentaXPagar> _cxpList;
        //Variables con valores x defecto (glControl)
        protected string defTercero = string.Empty;
        protected string defPrefijo = string.Empty;
        protected string defProyecto = string.Empty;
        protected string defCentroCosto = string.Empty;
        protected string defLineaPresupuesto = string.Empty;
        protected string defConceptoCargo = string.Empty;
        protected string defLugarGeo = string.Empty;
        protected string defConceptoCxP = string.Empty;
        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public MigracionCxP() 
        {
            //this.InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.MigracionCxP;              

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = false;

                List<DTO_glConsultaFiltro> filtroMaster = new List<DTO_glConsultaFiltro>();
                filtroMaster.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "DocumentoID",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = AppDocuments.CausarFacturas.ToString(),
                    OperadorSentencia = "OR"
                });
                filtroMaster.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "DocumentoID",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = AppDocuments.NotaCreditoCxP.ToString(),
                });

                //Funciones para iniciar el formulario
                this._bc.InitPeriodUC(this.dtPeriod, 0);
                this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
                this._bc.InitMasterUC(this.masterConcSaldo, AppMasters.glConceptoSaldo, true, true, true, false);
                this._bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false, filtroMaster);
                
                //Inicia las variables
                this.dtPeriod.Enabled = false;
                this.documentMigracionID = AppDocuments.CausarFacturas;
                this.masterDocumento.Value = this.documentMigracionID.ToString();
                this.InitVars();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-MigracionCxP.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las variables
        /// </summary>
        private void InitVars()
        {
            #region Variables
            //Carga info de las monedas
            this.monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga las variables globales
            this.isMultiMoneda = this._bc.AdministrationModel.MultiMoneda;
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Carga los valores por defecto
            this.defTercero = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.defPrefijo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
            this.defProyecto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.defLineaPresupuesto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.defConceptoCargo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ConceptoCargoXDefecto);
            this.defLugarGeo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            this.defConceptoCxP = this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_ConceptoCXPxDefecto);
            //Variables del formato
            this.colsCount = 0;
            this.dtPeriod.DateTime = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.cp, AppControl.co_Periodo));
            this.format = string.Empty;
            #endregion
            #region Carga la info de la tasa de cierre
            if (isMultiMoneda)
            {
                Dictionary<string, string> pksTasaCierre = new Dictionary<string, string>();
                pksTasaCierre.Add("MonedaID", this.monedaExtranjera);
                pksTasaCierre.Add("PeriodoID", this.dtPeriod.DateTime.ToString(FormatString.ControlDate));
                DTO_coTasaCierre dto_TC = (DTO_coTasaCierre)this._bc.GetMasterComplexDTO(AppMasters.coTasaCierre, pksTasaCierre, true);
                if (dto_TC == null)
                    this.hasTasaCierre = false;
                else
                {
                    this.hasTasaCierre = true;
                    this.tasaCierre = dto_TC.TasaCambio.Value.Value;
                }
            }
            #endregion
            #region Formato de importar
            string formatDoc = this._bc.GetImportExportFormat(typeof(DTO_glDocumentoControl), this.documentID);
            string formatCxP = this._bc.GetImportExportFormat(typeof(DTO_cpCuentaXPagar), this.documentMigracionID);
            this.format = formatDoc + this.formatSeparator + formatCxP;

            this.firstColSuppl = "Valor";
            this.colsCount = 9;

            // Limpia el formato
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);

            string f = string.Empty;
            foreach (string col in cols)
            {
                #region Columnas Documento Control y Cxp
                if (col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "PrefijoID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "MonedaID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambioCONT") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "LineaPresupuestoID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "LugarGeograficoID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "Observacion") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_" + "RadicaFecha") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_" + "ConceptoCxPID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_" + "MonedaPago") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_" + "CausalDevID") &&
                    col != this._bc.GetResource(LanguageTypes.Forms, "Iva"))
                {
                    if (col == this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + "TasaCambioDOCU"))
                    {
                        if (this.isMultiMoneda)
                            f += col + this.formatSeparator;
                        else
                            this.colsCount--;
                    }
                    else
                        f += col + this.formatSeparator;
                }
                #endregion
            }

            this.format = f;
            #endregion
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="ctrl">glDocumentoControl a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        private void ValidateDataImport(DTO_glDocumentoControl ctrl, DTO_cpCuentaXPagar cxp, DTO_TxResultDetail rd, string msgInvalidDate)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;

            #region Valida las FKs
            #region TerceroID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_TerceroID");
            rdF = this._bc.ValidGridCell(colRsx, ctrl.TerceroID.Value, false, true, false, AppMasters.coTercero);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CuentaID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CuentaID");
            rdF = this._bc.ValidGridCell(colRsx, ctrl.CuentaID.Value, false, true, true, AppMasters.coPlanCuenta);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region ProyectoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ProyectoID");
            rdF = this._bc.ValidGridCell(colRsx, ctrl.ProyectoID.Value, false, true, false, AppMasters.coProyecto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CentroCostoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CentroCostoID");
            rdF = this._bc.ValidGridCell(colRsx, ctrl.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
            #region Validaciones del documento
            #region Validacion de la cuenta
            if (ctrl.CuentaID.Value != this.masterCuenta.Value)
            {
                string rsxField = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CuentaID");
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = rsxField;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField);
                rd.DetailsFields.Add(rdF);

                createDTO = false;
            }
            #endregion
            #region Validacion de fechas
            //Valida que la fecha de vencimiento de la factura sea superior o igual a la fecha del documento
            if (cxp.VtoFecha.Value.Value < ctrl.FechaDoc.Value.Value)
            {
                string rsxField = this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_VtoFecha");
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = rsxField;
                rdF.Message = msgInvalidDate;
                rd.DetailsFields.Add(rdF);

                createDTO = false;
            }
            #endregion
            #endregion
            #region Asigna los valores por defecto
            if (createDTO)
            {
                //Info de la moneda
                ctrl.MonedaID.Value = this.monedaLocal;
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, ctrl.CuentaID.Value, true);
                TipoMoneda tipoMda = (TipoMoneda)Enum.Parse(typeof(TipoMoneda), cta.OrigenMonetario.Value.Value.ToString());
                if (this.isMultiMoneda && tipoMda == TipoMoneda.Foreign)
                    ctrl.MonedaID.Value = this.monedaExtranjera;

                //Documento Control
                ctrl.DocumentoID.Value = this.documentMigracionID;
                ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                ctrl.PrefijoID.Value = this.defPrefijo;
                ctrl.TasaCambioCONT.Value = this.isMultiMoneda ? this.tasaCierre : 0;
                ctrl.TasaCambioDOCU.Value = ctrl.TasaCambioCONT.Value;
                ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                ctrl.LugarGeograficoID.Value = this.defLugarGeo;
                ctrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value;
                ctrl.Descripcion.Value = "CONTABILIZA MIGRACION DE SALDOS CXP";

                //CxP
                cxp.RadicaFecha.Value = ctrl.Fecha.Value;
                cxp.ConceptoCxPID.Value = this.defConceptoCxP;
                cxp.MonedaPago.Value = ctrl.MonedaID.Value;
                cxp.FacturaFecha.Value = ctrl.Fecha.Value;
                cxp.DistribuyeImpLocalInd.Value = false;
                cxp.PagoAprobacionInd.Value = false;
                cxp.PagoInd.Value = false;

                //Info de los valores
                if (this.isMultiMoneda)
                {
                    if (ctrl.MonedaID.Value == this.monedaLocal)
                    {
                        cxp.ValorLocal.Value = cxp.Valor.Value;
                        cxp.ValorExtra.Value = Math.Round((cxp.Valor.Value.Value / ctrl.TasaCambioCONT.Value.Value), 2);
                    }
                    else
                    {
                        cxp.ValorLocal.Value = Math.Round((cxp.Valor.Value.Value * ctrl.TasaCambioCONT.Value.Value), 2);
                        cxp.ValorExtra.Value = cxp.Valor.Value;
                    }
                }
                else
                {
                    cxp.ValorLocal.Value = cxp.Valor.Value;
                    cxp.ValorExtra.Value = 0;
                }

                this.valorTotal += cxp.Valor.Value.Value;
            }
            #endregion
        }

        #endregion

        #region Eventos

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

                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-MigracionCxP.cs", "btnTemplate_Click"));
            }
        }

        /// <summary>
        /// Procesa el cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            bool validInfo = true;
            this.valorTotal = 0;

            if (this.isMultiMoneda && !this.hasTasaCierre)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_TasaCierre));
                validInfo = false;
            }
            else if (!this.masterCuenta.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCuenta.CodeRsx);
                MessageBox.Show(msg);
                validInfo = false;
            }
            else if (!this.masterConcSaldo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterConcSaldo.CodeRsx);
                MessageBox.Show(msg);
                validInfo = false;
            }
            else if (!this.masterDocumento.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterDocumento.CodeRsx);
                MessageBox.Show(msg);
                validInfo = false;
            }
            else
            {
                DTO_glDocumento doc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, false, this.masterDocumento.Value, true);
                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, this.masterCuenta.Value, true);
                DTO_glConceptoSaldo cs = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, this.masterConcSaldo.Value, true);
                SaldoControl saldoCtrl = (SaldoControl)Enum.Parse(typeof(SaldoControl), cs.coSaldoControl.Value.Value.ToString());
                if (saldoCtrl != SaldoControl.Cuenta)
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidConcSaldoCta));
                    validInfo = false;
                }
                else
                {
                    if (cs.ModuloID.Value  != doc.ModuloID.Value)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Gl_ModuleNotCompatible));
                        validInfo = false;
                    }
                    cs = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, this.masterConcSaldo.Value, true);
                    saldoCtrl = (SaldoControl)Enum.Parse(typeof(SaldoControl), cs.coSaldoControl.Value.Value.ToString());
                    if (saldoCtrl == SaldoControl.Cuenta)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidConcSaldo));
                        validInfo = false;
                    }
                }
            }

            if (validInfo)
            {
                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                this.btnImport.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnGenerar.Enabled = false;
                Thread process = new Thread(this.ImportThread);
                process.Start();
            }
        }

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                string msg;
                this.valorTotal = Math.Round(this.valorTotal, 2);

                DTO_glDocumentoControl ctrl = this._ctrlList.First();
                bool isML = ctrl.MonedaID.Value == this.monedaLocal ? true : false;
                string libroFunc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional); 
                decimal saldosVal = this._bc.AdministrationModel.Saldo_GetByPeriodoCuenta(isML, ctrl.PeriodoDoc.Value.Value, ctrl.CuentaID.Value, libroFunc);

                DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, false, ctrl.CuentaID.Value, true);
                NaturalezaCuenta nat = (NaturalezaCuenta)Enum.Parse(typeof(NaturalezaCuenta), cta.Naturaleza.Value.Value.ToString());
                if (nat == NaturalezaCuenta.Debito && saldosVal < 0 || nat == NaturalezaCuenta.Credito && saldosVal > 0)
                {
                    msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidSaldos);
                    MessageBox.Show(string.Format(msg, saldosVal));
                }
                else if (saldosVal != this.valorTotal)
                {
                    msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidSaldosImport);
                    MessageBox.Show(string.Format(msg, saldosVal, this.valorTotal));
                }
                else
                {
                    this.btnImport.Enabled = false;
                    this.btnTemplate.Enabled = false;
                    this.btnGenerar.Enabled = false;

                    Thread process = new Thread(this.ProcesarThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-MigracionCxP.cs", "btnGenerarDocumentos_Click"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            try
            {
                ControlsUC.uc_MasterFind ctrl = (ControlsUC.uc_MasterFind)sender;

                if(ctrl.Name.Equals("masterDocumento"))
                {
                    if (ctrl.ValidID)
                    {
                        this.documentMigracionID = Convert.ToInt32(ctrl.Value);
                        this.InitVars();
                    }
                }
        
            }
            catch (Exception ex)
            {
                throw ex;
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
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgInvalidDate = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cp_InvFechaFact);
                    //Popiedades de un comprobante
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_cpCuentaXPagar cxp = new DTO_cpCuentaXPagar();
                    bool createDTO = true;
                    bool validList = true;
                    //Inicializacion de variables generales
                    this._ctrlList = new List<DTO_glDocumentoControl>();
                    this._cxpList = new List<DTO_cpCuentaXPagar>();
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<PropertyInfo> pisCtrl = typeof(DTO_glDocumentoControl).GetProperties().ToList();
                    List<PropertyInfo> pisSuppl = typeof(DTO_cpCuentaXPagar).GetProperties().ToList();

                    //Recorre el objeto de docCtrl y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisCtrl)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);
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
                    //Recorre el objeto suplementario y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSuppl)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentMigracionID + "_" + pi.Name);
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
                            if (line.Length < colsCount)
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
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < colsCount; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        //revisa si es documento control o la tabla suplementaria
                                        if (colIndex == 0)
                                            ctrl = new DTO_glDocumentoControl();
                                        else if (colName == this.firstColSuppl)
                                            cxp = new DTO_cpCuentaXPagar();

                                        #region Validacion de Nulls y Formatos (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue))
                                        {
                                            #region Valida nulls
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Validacion Formatos

                                            UDT udt;
                                            PropertyInfo pi = ctrl.GetType().GetProperty(colName);
                                            if (pi != null)
                                                udt = (UDT)pi.GetValue(ctrl, null);
                                            else
                                            {
                                                pi = cxp.GetType().GetProperty(colName);
                                                udt = (UDT)pi.GetValue(cxp, null);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
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
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }

                                                //Si paso las validaciones asigne el valor al DTO
                                                udt.ColRsx = colRsx;
                                                if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                                    udt.SetValueFromString(colValue);
                                            } 
                                            #endregion
                                        }
                                        #endregion

                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "MigracionCxP.cs - Creacion de DTO y validacion Formatos");
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
                                createDTO = false;
                            }

                            if (createDTO)
                            {
                                this._ctrlList.Add(ctrl);
                                this._cxpList.Add(cxp);
                            }
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la cxp
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this._ctrlList.Count; ++index)
                        {
                            ctrl = this._ctrlList[index];
                            cxp = this._cxpList[index];

                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this._ctrlList.Count);
                            i++;
                            #endregion
                            #region Definicion de variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            #endregion
                            #region Valida los DTOs (glDocumentoControl y tabla suplementaria)
                            this.ValidateDataImport(ctrl, cxp, rd, msgInvalidDate);
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Carga la informacion en la lista de importacion
                    if (result.Result != ResultValue.NOK)
                        this._isOK = true;
                    else
                        this._isOK = false;

                    MessageForm frm = new MessageForm(result);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                    this.Invoke(this.endImportarDelegate);
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                    this._isOK = false; 
                    this.Invoke(this.endImportarDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-MigracionCxP.cs", "ImportThread"));
                this.StopProgressBarThread();
            }
        }

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                DTO_TxResult result = this._bc.AdministrationModel.CuentasXPagar_Migracion(Convert.ToInt32(this.documentMigracionID), this._actFlujo.ID.Value, this.masterConcSaldo.Value, this._ctrlList, this._cxpList);
                this.StopProgressBarThread();

                this._isOK = true;
                if (result.Result == ResultValue.NOK)
                    this._isOK = false;

                MessageForm frm = new MessageForm(result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.endProcesarDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-MigracionCxP.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
