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
    public partial class MigracionProvisiones : ProcessForm
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

        //private MigracionNominas()
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
        private string _empleadoRsx = string.Empty;
        private string _conceptoRsx = string.Empty;
        //Variables del formulario
        private bool _isOK;
        private List<DTO_noProvisionDeta> _data;
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.MigracionProvisiones;

                this.InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = false;
                this._data = new List<DTO_noProvisionDeta>();

                //Funciones para iniciar el formulario
                this.InitVars();
                this.AssignFormat();
                this.CleanFormat();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionNominas.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las variables
        /// </summary>
        private void InitVars()
        {
            //Carga las variables globales
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, AppDocuments.Nomina);

            this._empleadoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoID");
            this._conceptoRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
        }


        /// <summary>
        /// Asigna el formato de importacion
        /// </summary>
        private void AssignFormat()
        {
            string templateFormat = _bc.GetImportExportFormat(typeof(DTO_noProvisionDeta), this.documentID);
            this.format = templateFormat;
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
            //Empleado
            if (colName == this._empleadoRsx)
                return AppMasters.noEmpleado;
            //Concepto
            if (colName == this._conceptoRsx)
                return AppMasters.noConceptoNOM;
      

            return 0;
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

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionNominas.cs", "btnTemplate_Click"));
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
            this.btnImport.Enabled = false;
            this.btnTemplate.Enabled = false;
            this.btnGenerar.Enabled = false;

            Thread process = new Thread(this.ProcesarThread);
            process.Start();
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
                    this._data.Clear();
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    List<DTO_noProvisionDeta> list = new List<DTO_noProvisionDeta>();

                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgCtaCargoProy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                    string msgCtaPeriodClosed = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CtaPeriodClosed);
                    //Popiedades de un comprobante
                    DTO_noProvisionDeta det = new DTO_noProvisionDeta();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_noProvisionDeta).GetProperties();

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
                    fks.Add(_empleadoRsx, new List<Tuple<string, bool>>());
                    fks.Add(_conceptoRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        //Recorre todas las columnas y verifica que tengan datos validos
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
                                        if (colRsx == this._empleadoRsx || colRsx == this._conceptoRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupInvalid))
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
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                det = new DTO_noProvisionDeta();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                                (colRsx == this._empleadoRsx ||
                                                colRsx == this._conceptoRsx ||
                                                colName == "Periodo" ||
                                                colName == "Dias Trabajados" ||
                                                colName == "Dias Provisión" ||
                                                colName == "Sueldo" ||
                                                colName == "Aux. Transporte" ||
                                                colName == "Base Variable " ||
                                                colName == "Base Neta" ||
                                                colName == "Vlr Prov. Mes" ||
                                                colName == "Vlr Pagado" 
                                              )
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
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
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
                                            //Si paso las validaciones asigne el valor al DTO
                                            if (createDTO)
                                            {
                                                udt.SetValueFromString(colVal);
                                            }
                                        }

                                        #region Otros Formatos

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

                                        #endregion
                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DocumentNominaForm.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {                               
                                list.Add(det);      
                            }
                            else
                                validList = false;
                        }
                    }
                    if (validList)
                    {
                        this._data = list;
                        this._isOK = true;
                    }
                    #endregion 
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });                    
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionProvisiones.cs", "ImportThread"));
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
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                List<DTO_TxResult> results = this._bc.AdministrationModel.Proceso_MigracionProvisiones(this.documentID, this._data);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionProvisiones.cs", "MigracionProvisiones.cs-btnProcesar_Click"));
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
