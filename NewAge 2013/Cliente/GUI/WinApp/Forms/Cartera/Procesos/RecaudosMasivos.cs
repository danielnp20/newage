using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using NewAge.DTO.Negocio;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes; 
using NewAge.DTO.UDT;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RecaudosMasivos : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.Enabled = true;            
            this.btnTemplate.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this.btnPreliminar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = !this.pasteRet.Success ? false : true;

                this.masterPagaduria.Enabled = true;
                this.dtFecha.Enabled = true;
                this.dtFechaAplica.Enabled = true;
                this.txtValor.Enabled = true;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de validaciones del servidor
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.Enabled = true;
            
            this.btnTemplate.Enabled = true;
            this.btnImportar.Enabled = true;
            this.btnPreliminar.Enabled = true;

            if (this.result.Result == ResultValue.NOK)
            {
                this.btnInconsistencias.Enabled = true;
                this.validarInconsistencias = true;
                this._isOK = false;
            }
            else if (this.result.Details.Count == 0)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this._isOK = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = true;
                this._isOK = true;
            }

            if (this._isOK || !this.validarInconsistencias)
                this.btnProcesar.Enabled = true;
        }

        /// <summary>
        /// Delegado que finaliza el proceso de pago de creditos
        /// </summary>
        public delegate void EndPagar();
        public EndPagar endPagarDelegate;
        public void EndPagarMethod()
        {
            this.Enabled = true;
            this.btnTemplate.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                
                this.btnPreliminar.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.pagaduria = new DTO_ccPagaduria();
                this.centroPagoID = string.Empty;
                this.masterPagaduria.Value = string.Empty;
                this.dtFecha.DateTime = this.dtPeriod.DateTime;
                this.dtFechaAplica.DateTime = this.dtPeriod.DateTime;
                this.lkp_periodo.EditValue = "1";
                this.txtValor.EditValue = 0;
                
                this.dtFecha.Enabled = true;
                this.dtFechaAplica.Enabled = true;
                this.txtValor.Enabled = true;

                this.results = null;
                this.result = null;

                List<DTO_ExportRecaudoMasivo> listExport = new List<DTO_ExportRecaudoMasivo>();
                foreach (DTO_ccIncorporacionDeta incorp in this.data)
                {
                    DTO_ExportRecaudoMasivo rec = new DTO_ExportRecaudoMasivo();
                    DTO_glDocumentoControl ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(incorp.NumeroDoc.Value.Value);
                    if (ctrl != null)
                    {
                        rec.Libranza.Value = incorp.Libranza.Value;
                        rec.ClienteID.Value = !string.IsNullOrEmpty(incorp.ClienteID.Value) ? incorp.ClienteID.Value : incorp.CodEmpleado.Value;
                        rec.TipoMvto.Value = AppDocuments.RecaudosMasivos;
                        rec.Documento.Value = ctrl.Comprobante.Value + "/" + ctrl.ComprobanteIDNro.Value.ToString();
                        rec.ValorCuota.Value = incorp.ValorCuota.Value;
                        rec.FechaNomina.Value = incorp.FechaNomina.Value;
                        rec.FechaDoc.Value = ctrl.FechaDoc.Value;
                        listExport.Add(rec);
                    }                 
                }
                DataTableOperations tableOp = new DataTableOperations();
                System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportRecaudoMasivo), listExport);
                ReportExcelBase frmExcel = new ReportExcelBase(tableAll);
                frmExcel.ShowDialog();
                this.data = new List<DTO_ccIncorporacionDeta>();
            }
            else
            {
                this.btnProcesar.Enabled = true; 
                this.btnInconsistencias.Enabled = true;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso imprimir las inconsistencias
        /// </summary>
        public delegate void EndInconsistencias();
        public EndInconsistencias endInconsistenciasDelegate;
        public void EndInconsistenciasMethod()
        {
            this.Enabled = true;
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private PasteOpDTO pasteRet;
        //Variables para importar
        private string format;
        private string formatSeparator = "\t";
        //Variables del formulario
        private bool _isOK;
        private List<DTO_ccIncorporacionDeta> data;
        private string centroPagoID = string.Empty;
        private DateTime periodo = DateTime.Now;
        bool isMensual = false;
        private DTO_ccIncorporacionDeta incorporacion = new DTO_ccIncorporacionDeta();
        private DTO_ccPagaduria pagaduria = new DTO_ccPagaduria();
        private bool validarInconsistencias;
        private decimal valorPagaduria;
        //Variables para la importacion
        DTO_TxResult result;
        List<DTO_TxResult> results;
        List<string> codigos = new List<string>();
        Dictionary<string, string> clientes = new Dictionary<string, string>();
        //Variables con los recursos de las Fks
        private string _clienteRsx = string.Empty;
        private string reportName;
        private string fileURl;

        #endregion

        public RecaudosMasivos()
        {
            //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {

            this.documentID = AppDocuments.RecaudosMasivos; //AppProcess.RecaudosMasivos;

            this.InitializeComponent();
            FormProvider.LoadResources(this, AppProcess.RecaudosMasivos);

            //Inicializa los delegados
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
            this.endProcesarDelegate = new EndProcesar(this.EndProcesarMethod);
            this.endPagarDelegate = new EndPagar(this.EndPagarMethod);
            this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

            //Carga la configuracion inicial
            this._isOK = false;

            this.data = new List<DTO_ccIncorporacionDeta>();
            this.btnImportar.Enabled = false;
            this.btnPreliminar.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            //Carga los recursos de las Fks
            this._clienteRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID");

            //Carga la info inicial de los controles (centro de pago y periodo)
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);

            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = this.periodo;

            //Fecha
            int maxDay = DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month);
            if(maxDay == 31)
                maxDay = 30;

            DateTime fechaIni = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            DateTime fechaFin = new DateTime(this.periodo.Year, this.periodo.Month, maxDay);
            this.dtFecha.Properties.MinValue = fechaIni;
            this.dtFecha.Properties.MaxValue = fechaFin;
            this.dtFecha.DateTime = fechaFin;
            this.dtFechaAplica.DateTime = fechaFin;

            //Info del combo de seleccion
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, AppProcess.RecaudosMasivos.ToString() + "_tbl_Mensual"));
            dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, AppProcess.RecaudosMasivos.ToString() + "_tbl_Quincenal"));
            this.lkp_periodo.Properties.DataSource = dic;
            this.lkp_periodo.EditValue = "1";
            this.lkp_periodo.Enabled = false;

            //Variable para valida inconsistencias
            string validar = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AnalisisInconsistenciaNomina);
            if (validar == "0")
                this.validarInconsistencias = false;
            else
                this.validarInconsistencias = true;

            //Asigna el formato
            this.format = _bc.GetImportExportFormat(typeof(DTO_ccIncorporacionDeta), AppProcess.RecaudosMasivos);
            this.dtFechaAplica.Leave += new System.EventHandler(this.dtFechaAplica_Leave);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        private int GetMasterDocumentID(string colName)
        {
            //Comprobante
            if (colName == this._clienteRsx)
                return AppMasters.ccCliente;

            return 0;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_ccIncorporacionDeta dto, DTO_TxResultDetail rd, string msgFecha, string msgNoRel, string msgPositive, string msgEmptyField, 
            string msgClienteRepetido, string msgCodAndCliente)
        {
            try
            {
                #region Valida que la fecha se encuentre en el periodo
                if (dto.FechaNomina.Value.Value != this.dtFechaAplica.DateTime)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_FechaNomina"); ;
                    rdF.Message = msgFecha;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion cliente / codigo / libranza
                if (string.IsNullOrWhiteSpace(dto.ClienteID.Value) && string.IsNullOrWhiteSpace(dto.CodEmpleado.Value) && (dto.Libranza.Value == null))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID") +
                            _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado") +
                            _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_Libranza");

                    rdF.Message = msgNoRel;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion de Valores

                if (dto.ValorCuota.Value.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ValorCuota");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }

                #endregion
                #region Validacion Codigo del empleado

                if (this.pagaduria.CodEmpleadoInd.Value.Value && String.IsNullOrEmpty(dto.CodEmpleado.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado"); ;
                    rdF.Message = msgEmptyField;
                    rd.DetailsFields.Add(rdF);
                }

                #endregion
                #region Validacion de exclusion entre cliente y codigo de empleado
                if (!string.IsNullOrWhiteSpace(dto.CodEmpleado.Value) && !string.IsNullOrWhiteSpace(dto.ClienteID.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_ClienteID") + _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado");
                    rdF.Message = msgCodAndCliente;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion para que codigo de empleado que se repita debe tener la libranza

                if (!string.IsNullOrWhiteSpace(dto.CodEmpleado.Value))
                {
                    if (this.codigos.Contains(dto.CodEmpleado.Value))
                    {
                        if (string.IsNullOrWhiteSpace(clientes[dto.CodEmpleado.Value]) || dto.Libranza.Value == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos + "_CodEmpleado");
                            rdF.Message = msgClienteRepetido;
                            rd.DetailsFields.Add(rdF);
                        }
                    }
                    else
                    {
                        string val = dto.Libranza.Value == null ? string.Empty : dto.Libranza.Value.Value.ToString();
                        this.codigos.Add(dto.CodEmpleado.Value);
                        this.clientes.Add(dto.CodEmpleado.Value, val);
                    }
                }

                #endregion
                #region Validacion para que cliente que se repita debe tener la libranza

                if (!string.IsNullOrWhiteSpace(dto.ClienteID.Value))
                {
                    if (clientes.ContainsKey(dto.ClienteID.Value))
                    {
                        if (string.IsNullOrWhiteSpace(clientes[dto.ClienteID.Value]) || dto.Libranza.Value == null)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                            rdF.Message = msgClienteRepetido;
                            rd.DetailsFields.Add(rdF);
                        }
                    }
                    else
                    {
                        string val = dto.Libranza.Value == null ? string.Empty : dto.Libranza.Value.Value.ToString();
                        clientes.Add(dto.ClienteID.Value, val);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "ValidateDataImport"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al salir del centro de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterPag_Leave(object sender, EventArgs e)
        {
            try
            {
                bool loadCP = true;
                if (this.data.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                        loadCP = false;
                }

                // Revisa si se debe volver a cargar la info del centro de pago
                if (loadCP)
                {
                    this.data = new List<DTO_ccIncorporacionDeta>();
                    this.btnImportar.Enabled = true;
                    this.btnPreliminar.Enabled = false;
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    if (this.masterPagaduria.ValidID)
                    {
                        this.pagaduria = (DTO_ccPagaduria)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, this.masterPagaduria.Value, true);
                        this.masterPagaduria.Value = this.pagaduria.ID.Value;
                            
                        #region Calculo del combo del periodo

                        this.lkp_periodo.EditValue = this.pagaduria.PeriodoPago.Value.Value.ToString();
                        this.isMensual = this.pagaduria.PeriodoPago.Value.Value == 1 ? true : false;

                        #endregion
                    }
                    else
                    {
                        this.btnImportar.Enabled = false;
                            
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPagaduria.LabelRsx);
                        MessageBox.Show(msg);

                        this.masterPagaduria.Value = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "masterCentroPago_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFechaAplica_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFechaAplica.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            }
            else if(this.isMensual || (this.dtFechaAplica.DateTime.Day != 15 && this.dtFechaAplica.DateTime.Day != 30))
            {
                this.dtFechaAplica.DateTime = new DateTime(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month, DateTime.DaysInMonth(this.dtFechaAplica.DateTime.Year, this.dtFechaAplica.DateTime.Month)); 
            }

            if (this.dtFechaAplica.DateTime.Day == 31)
                this.dtFechaAplica.DateTime = this.dtFechaAplica.DateTime.AddDays(-1);
        }

        /// <summary>
        /// Boton para limpiar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            if (this.data.Count > 0)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.data = new List<DTO_ccIncorporacionDeta>();
                    this.btnPreliminar.Enabled = false;
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this._isOK = false;
                    this.pagaduria = new DTO_ccPagaduria();
                    this.centroPagoID = string.Empty;
                    this.masterPagaduria.Value = string.Empty;
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtFechaAplica.DateTime = this.dtPeriod.DateTime;
                    this.lkp_periodo.EditValue = "1";
                    this.txtValor.EditValue = 0;
                    
                    this.dtFecha.Enabled = true;
                    this.dtFechaAplica.Enabled = true;
                    this.txtValor.Enabled = true;

                    this.results = null;
                    this.result = null;
                }
            }
        }

        /// <summary>
        /// Evento para generar laplantilla de importacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "btnTemplate_Click"));
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            if (this.data.Count > 0)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                    loadData = false;
                else
                {
                    decimal val = Convert.ToDecimal(this.txtValor.EditValue);
                    if (val <= 0)
                    {
                        loadData = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoValue));
                    }
                }
            }
            else
            {
                decimal val = Convert.ToDecimal(this.txtValor.EditValue);
                if (val <= 0)
                {
                    loadData = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoValue));
                }
            }


            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                this.btnImportar.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnPreliminar.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.masterPagaduria.Enabled = false;
                this.dtFecha.Enabled = false;
                this.dtFechaAplica.Enabled = false;
                this.txtValor.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ImportThread);
                process.Start();
            }
        }

        /// <summary>
        /// Evento que se encarga de verificar las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreliminar_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            if (this.data.Count > 0 && this.btnProcesar.Enabled)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                    loadData = false;
            }

            if (loadData)
            {
                this.Enabled = false;

                this.btnImportar.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnPreliminar.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.PreliminarThread);
                process.Start();
            }
        }

        /// <summary>
        /// Evento que se encarga de pagar las migraciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            this.btnImportar.Enabled = false;
            this.btnTemplate.Enabled = false;
            this.btnPreliminar.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            this.results = null;
            this.result = null;

            Thread process = new Thread(this.PagarThread);
            process.Start();
        }

        /// <summary>
        /// Evento que muestra las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInconsistencias_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            Thread process = new Thread(this.InconsistenciasThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    this.clientes = new Dictionary<string, string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.data = new List<DTO_ccIncorporacionDeta>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidFechaAplica);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgClinteRepetido = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteSinLibranza);
                    string msgCodAndCliente = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteYCodigo);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    //Popiedades de la incorporacion
                    DTO_ccIncorporacionDeta incorp = new DTO_ccIncorporacionDeta();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_ccIncorporacionDeta).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.RecaudosMasivos.ToString() + "_" + pi.Name);
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
                    fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            this.result.Details = new List<DTO_TxResultDetail>();
                            this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            this.result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            this.result.ResultMessage = msgNoCopyField;
                            this.result.Result = ResultValue.NOK;
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
                                this.result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                this.result.Details.Add(rdL);

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

                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]) && colRsx == this._clienteRsx)
                                    {
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
                                            int docId = this.GetMasterDocumentID(colRsx);
                                            object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, false, line[colIndex], true);

                                            if (dto == null)
                                            {
                                                fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                                fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                        }
                                    }
                                }
                                #endregion
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                incorp = new DTO_ccIncorporacionDeta();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == "FechaNomina" || colName == "ValorCuota"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos

                                        PropertyInfo pi = incorp.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(incorp, null);
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

                                        } //validacion si no es null
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "RecaudosMasivos.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {
                                DTO_ccIncorporacionDeta exist = this.data.Find(x => x.Libranza.Value == incorp.Libranza.Value &&
                                                        x.CodEmpleado.Value == incorp.CodEmpleado.Value &&
                                                        x.ClienteID.Value == incorp.ClienteID.Value &&
                                                        x.FechaNomina.Value == incorp.FechaNomina.Value);

                                if (exist != null)
                                    exist.ValorCuota.Value += incorp.ValorCuota.Value;
                                else
                                    this.data.Add(incorp);
                            }
                              
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    #endregion
                    #region Valida las restricciones particulares de la migracion de nomina
                    if (validList)
                    {
                        #region Variables generales
                        this.result = new DTO_TxResult();
                        this.result.Result = ResultValue.OK;
                        this.result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        #endregion
                        foreach (DTO_ccIncorporacionDeta dto in this.data)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this.data.Count);
                            i++;

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                this.result.Details = new List<DTO_TxResultDetail>();
                                this.result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                this.result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;
                            #endregion
                            #region Validaciones particulares del DTO
                            this.ValidateDataImport(dto, rd, msgFecha, msgNoRel, msgPositive, msgEmptyField, msgClinteRepetido, msgCodAndCliente);
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;

                                validList = false;
                            }
                            #endregion
                        }

                        this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                    }
                    #endregion
                    #region Valida los valores

                    if (validList)
                    {
                        decimal valNomina = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                        decimal valPagos = this.data.Sum(x => x.ValorCuota.Value.Value);
                        this.valorPagaduria = valNomina - valPagos;
                        if (valPagos != valNomina)
                        {
                            string valNominaStr = valNomina.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));
                            string valPagosStr = valPagos.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));
                            string valDifStr = this.valorPagaduria.ToString("c", CultureInfo.CreateSpecificCulture("en-US"));

                            string msgTitleDif = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NominaDifVal);

                            string msgDif = string.Format(msg, valNominaStr, valPagosStr, valDifStr);
                            if (MessageBox.Show(msgDif, msgTitleDif, MessageBoxButtons.YesNo) == DialogResult.No)
                                validList = false;
                        }
                    }

                    #endregion
                    if (validList)
                    {
                        #region Hace validaciones del servidor
                        this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                        this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                        this.ProgressBarThread.Start();

                        this.result = _bc.AdministrationModel.RecaudosMasivos_Validar(this.documentID, this.periodo, this.pagaduria.ID.Value,
                            this.dtFechaAplica.DateTime, ref this.data);
                        this.StopProgressBarThread();

                        if (this.result.Result == ResultValue.OK)
                        {
                            this._isOK = true;
                        }
                        else
                        {
                            this._isOK = false;
                            this.data = new List<DTO_ccIncorporacionDeta>();
                        }
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.data = new List<DTO_ccIncorporacionDeta>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccIncorporacionDeta>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "ImportThread");
            }
            finally
            {
                this.Invoke(this.endImportarDelegate);
                if (!this.pasteRet.Success)
                {
                    this._isOK = false;
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
        }

        /// <summary>
        /// Hilo de Procesar las inconsistencias
        /// </summary>
        private void PreliminarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.result = _bc.AdministrationModel.RecaudosMasivos_Procesar(this.documentID, this.centroPagoID, this.pagaduria.ID.Value, this.dtPeriod.DateTime, 
                    this.dtFechaAplica.DateTime, this.data);
                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccIncorporacionDeta>(); 
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "ProcesarThread");
            }
            finally
            {
                this.Invoke(this.endProcesarDelegate);
            }
        }

        /// <summary>
        /// Hilo de Pago de nomina
        /// </summary>
        private void PagarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                decimal valNomina = Convert.ToDecimal(this.txtValor.EditValue);
                decimal valPagos = this.data.Sum(x => x.ValorCuota.Value.Value);
                bool isMensual = this.lkp_periodo.EditValue == "1" ? false : true;
                List<DTO_ccIncorporacionDeta> temp = ObjectCopier.Clone(this.data);
                this.result = _bc.AdministrationModel.RecaudosMasivos_Pagar(this.documentID, this._actFlujo.ID.Value, this.periodo, 
                    this.dtFecha.DateTime, this.dtFechaAplica.DateTime, this.valorPagaduria, this.centroPagoID, null, this.data, this.data);

                //this.data = temp;
                this.StopProgressBarThread();

                this._isOK = this.result.Result == ResultValue.OK ? true : false;
            }
            catch (Exception ex)
            {
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-RecaudosMasivos.cs", "PagarThread");
            }
            finally
            {
                this.Invoke(this.endPagarDelegate);
            }
        }

        /// <summary>
        /// Carga el reporte con las inconsistencias
        /// </summary>
        private void InconsistenciasThread()
        {
            try
            {
                if (this.result != null)
                {
                    _bc.AssignResultResources(null, this.result);

                    reportName = this._bc.AdministrationModel.Rep_TxResult(this.result);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else if(this.results != null)
                {

                    _bc.AssignResultResources(null, this.results);

                    reportName = this._bc.AdministrationModel.Rep_TxResultDetails(this.results);
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                this.Invoke(this.endInconsistenciasDelegate);
            }
        }

        #endregion

    }
}
