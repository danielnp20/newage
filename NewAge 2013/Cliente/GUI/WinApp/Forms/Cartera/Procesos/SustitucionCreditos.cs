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
    public partial class SustitucionCreditos : ProcessForm
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
                this.btnProcesar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = !this.pasteRet.Success ? false : true;

                this.masterCompradorCartera.Enabled = true;
                this.dtFecha.Enabled = true;
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

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.data = new List<DTO_ccSustitucionCreditos>();
                this.btnImportar.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this._isOK = false;
                this.compradorCarteraID = string.Empty;
                this.masterCompradorCartera.Value = string.Empty;
                this.dtFecha.DateTime = this.dtPeriod.DateTime;

                this.masterCompradorCartera.Enabled = true;
                this.dtFecha.Enabled = true;

                this.results = null;
                this.result = null;
            }
            else
            {
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
        private List<DTO_ccSustitucionCreditos> data;
        private string compradorCarteraID = string.Empty;
        private DateTime periodo = DateTime.Now;
        //Variables para la importacion
        DTO_TxResult result;
        List<DTO_TxResult> results;
        List<int> libranzas = new List<int>();
        List<int> libranzasSustituye = new List<int>();
        //Variables con los recursos de las Fks
        private string _compradorRsx = string.Empty;
        private string reportName;
        private string fileURl;

        #endregion

        //public SustitucionCreditos()
        //{
        //    InitializeComponent();
        //}

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.SustitucionCreditos;

            this.InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);

            //Inicializa los delegados
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
            this.endProcesarDelegate = new EndProcesar(this.EndProcesarMethod);
            this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

            //Carga la configuracion inicial
            this._isOK = false;

            this.data = new List<DTO_ccSustitucionCreditos>();
            this.btnImportar.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            //Carga los recursos de las Fks
            this._compradorRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.SustitucionCreditos + "_CompradorCarteraID");

            //Carga la info inicial de los controles (centro de pago y periodo)
            _bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false);

            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = this.periodo;

            //Fecha
            DateTime fechaIni = new DateTime(this.periodo.Year, this.periodo.Month, 1);
            DateTime fechaFin = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            this.dtFecha.Properties.MinValue = fechaIni;
            this.dtFecha.Properties.MaxValue = fechaFin;
            this.dtFecha.DateTime = fechaFin;

            //Asigna el formato
            this.format = _bc.GetImportExportFormat(typeof(DTO_ccSustitucionCreditos), AppProcess.SustitucionCreditos);
            this.dtFecha.DateTimeChanged += new System.EventHandler(this.dtFecha_DateTimeChanged);
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
            if (colName == this._compradorRsx)
                return AppMasters.ccCompradorCartera;

            return 0;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidComprador">Mensaje que indica que el comprador es incorrecto</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        private void ValidateDataImport(DTO_ccSustitucionCreditos dto, DTO_TxResultDetail rd, string msgInvalidComprador, string msgLibranzaSustRepetida)
        {
            try
            {
                #region Validación comprador
                if (dto.CompradorCarteraID.Value != this.masterCompradorCartera.Value)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = this._compradorRsx;
                    rdF.Message = msgInvalidComprador;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validación para que no se repitan las libranzas

                //Sustituyen
                if (this.libranzasSustituye.Contains(dto.LibranzaSustituye.Value.Value))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LibranzaSustituye");
                    rdF.Message = msgLibranzaSustRepetida + "&&" + dto.LibranzaSustituye.Value.ToString();
                    rd.DetailsFields.Add(rdF);
                }
                else
                {
                    this.libranzasSustituye.Add(dto.LibranzaSustituye.Value.Value);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "ValidateDataImport"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al salir del centro de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCompradorCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compradorCarteraID != this.masterCompradorCartera.Value)
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
                        this.compradorCarteraID = this.masterCompradorCartera.Value;
                        this.data = new List<DTO_ccSustitucionCreditos>();
                        this.btnImportar.Enabled = true;
                        this.btnProcesar.Enabled = false;
                        this.btnInconsistencias.Enabled = false;

                        if (!this.masterCompradorCartera.ValidID)
                        {
                            this.btnImportar.Enabled = false;
                            
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCompradorCartera.LabelRsx);
                            MessageBox.Show(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFecha_DateTimeChanged(object sender, EventArgs e)
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
                    this.compradorCarteraID = this.masterCompradorCartera.Value;
                    this.data = new List<DTO_ccSustitucionCreditos>();
                    this.btnImportar.Enabled = true;
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "dtFecha_DateTimeChanged"));
            }
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
                    this.data = new List<DTO_ccSustitucionCreditos>();
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this._isOK = false;
                    this.compradorCarteraID = string.Empty;
                    this.masterCompradorCartera.Value = string.Empty;
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;

                    this.masterCompradorCartera.Enabled = true;
                    this.dtFecha.Enabled = true;

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "btnTemplate_Click"));
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
            }

            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                this.btnImportar.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.masterCompradorCartera.Enabled = false;
                this.dtFecha.Enabled = false;

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
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            this.btnImportar.Enabled = false;
            this.btnTemplate.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            this.results = null;
            this.result = null;

            Thread process = new Thread(this.ProcesarThread);
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
                    this.libranzas = new List<int>();
                    this.libranzasSustituye = new List<int>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.data = new List<DTO_ccSustitucionCreditos>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgInvalidComprador = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidCompradorCartera);
                    string msgLibranzaSustRepetida = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaSustituyeRepetida);
                    //Popiedades de la incorporacion
                    DTO_ccSustitucionCreditos sustitucion = new DTO_ccSustitucionCreditos();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_ccSustitucionCreditos).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.SustitucionCreditos.ToString() + "_" + pi.Name);
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
                    fks.Add(this._compradorRsx, new List<Tuple<string, bool>>());

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
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]) && colRsx == this._compradorRsx)
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
                                sustitucion = new DTO_ccSustitucionCreditos();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls
                                        if (string.IsNullOrEmpty(colValue))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos

                                        PropertyInfo pi = sustitucion.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(sustitucion, null);
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "SustitucionCreditos.cs - Creacion de DTO y validación Formatos");
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
                                this.data.Add(sustitucion);
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
                        foreach (DTO_ccSustitucionCreditos dto in this.data)
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
                            this.ValidateDataImport(dto, rd, msgInvalidComprador, msgLibranzaSustRepetida);
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

                    if (validList)
                    {
                        #region Hace validaciones del servidor
                        this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                        this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                        this.ProgressBarThread.Start();

                        this.result = _bc.AdministrationModel.SustitucionCreditos_Validar(this.documentID, this.dtFecha.DateTime, ref this.data);
                        this.StopProgressBarThread();

                        if (this.result.Result == ResultValue.OK)
                        {
                            this._isOK = true;
                        }
                        else
                        {
                            this._isOK = false;
                            this.data = new List<DTO_ccSustitucionCreditos>();
                        }
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.data = new List<DTO_ccSustitucionCreditos>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccSustitucionCreditos>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "ImportThread");
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
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.result = _bc.AdministrationModel.SustitucionCreditos_Procesar(this.documentID, this.dtFecha.DateTime, this.data);

                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this.data = new List<DTO_ccSustitucionCreditos>(); 
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-SustitucionCreditos.cs", "ProcesarThread");
            }
            finally
            {
                this.Invoke(this.endProcesarDelegate);
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
