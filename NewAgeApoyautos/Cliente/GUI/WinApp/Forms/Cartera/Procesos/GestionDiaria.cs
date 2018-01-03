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
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Configuration;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class GestionDiaria : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnIncosistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this.btnProcesar.Enabled = true;
            }
            else
            {
                this.btnIncosistencias.Enabled = !this.pasteRet.Success ? false : true;
                this.btnProcesar.Enabled = false;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de aprobar los comprobantes del cierre
        /// </summary>
        public delegate void EndProcesar();
        public EndProcesar endProcesarDelegate;
        public void EndProcesarMethod()
        {
            this.Enabled = true;
            this.btnImportar.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.results);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnIncosistencias.Enabled = false;
            }
            else
                this.btnIncosistencias.Enabled = true;

            this.btnProcesar.Enabled = false;
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

        BaseController _bc = BaseController.GetInstance();
        //Variables para proceso
        private PasteOpDTO pasteRet;
        private DTO_TxResult result;
        private List<DTO_TxResult> results;

        private string reportName;
        private string fileURl;

        //Variables para importar
        private string format;
        private string formatSeparator = "\t";

        //Variables del formulario
        private bool _isOK;
        private List<DTO_ccHistoricoGestionCobranza> _listGestion;

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
       // public GestionDiaria() { this.InitializeComponent(); }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.GestionDiaria;

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                Dictionary<string, string> datosEstado = new Dictionary<string, string>();
                datosEstado.Add("0", "Todos");
                datosEstado.Add("1", "Pendientes");
                datosEstado.Add("2", "Procesados");
                this.cmbEstado.Properties.ValueMember = "Key";
                this.cmbEstado.Properties.DisplayMember = "Value";
                this.cmbEstado.Properties.DataSource = datosEstado;
                this.cmbEstado.EditValue = "0";

                Dictionary<string, string> datosTipoCom = new Dictionary<string, string>();
                datosTipoCom.Add("0", "Todos");
                datosTipoCom.Add("1", "Correo");
                datosTipoCom.Add("2", "Carta");
                datosTipoCom.Add("3", "Mensaje Voz");
                datosTipoCom.Add("4", "Mensaje Texto");
                datosTipoCom.Add("5", "Llamada");
                datosTipoCom.Add("6", "Reporte");
                this.cmbTipoComunic.Properties.ValueMember = "Key";
                this.cmbTipoComunic.Properties.DisplayMember = "Value";
                this.cmbTipoComunic.Properties.DataSource = datosTipoCom;
                this.cmbTipoComunic.EditValue = "0";

                //Funciones para iniciar el formulario
                this._bc.InitMasterUC(this.masterGestionCob, AppMasters.ccCobranzaGestion, true, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);

                //Inicia las variables
                this.dtFechaCorte.DateTime = DateTime.Now;
                this.format = _bc.GetImportExportFormat(typeof(DTO_ccHistoricoGestionCobranza), this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);
                this.endInconsistenciasDelegate = new EndInconsistencias(EndInconsistenciasMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Genera el reporte de cartas a clientes
        /// </summary>
        private void GenerateReport()
        {
            try
            {
                this.btnGenerarCartas.Enabled = false;
                List<DTO_ccHistoricoGestionCobranza> histGestionCob = this._bc.AdministrationModel.HistoricoGestionCobranza_GetGestion(this.documentID, this.dtFechaCorte.DateTime.Date, string.Empty, null, null);
                List<DTO_CorreoCliente> correos = histGestionCob.Count > 0 ? this._bc.AdministrationModel.GetCorreosCliente(string.Empty, true, true, true) : null;
                if (histGestionCob.Any(x => x.CartaInd.Value.Value))
                    this._bc.AdministrationModel.Report_Cc_CartaCierreDiario(this.documentID, string.Empty, histGestionCob.FindAll(x => x.CartaInd.Value.Value));
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));

                this.btnGenerarCartas.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "GenerateReport"));
            }
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="gestion">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgLibranza">Mensaje de error para elvalor de la libranza</param>
        /// <param name="msgTipoEstado">Mensaje de restriccion para el tipo de estado</param>
        /// <param name="msgLineaComponentes">Mensaje para indicar que no hay relacion entre la linea de credito y los componentes de cartera</param>
        /// <param name="msgComodin">Mensaje que indica que la linea de credito no tiene el componente comodin</param>
        private void ValidateDataImport(DTO_ccHistoricoGestionCobranza gestion, DTO_TxResultDetail rd)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;         
            if (!gestion.Consecutivo.Value.HasValue)
            {
                createDTO = false;
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Consecutivo");
                rdF.Message = "Vacío";
                rd.DetailsFields.Add(rdF);
            }           
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarDocumentos_Click(object sender, EventArgs e)
        {
            try
            { 
                DataTable result = this._bc.AdministrationModel.HistoricoGestionCobranza_GetExcel(this.documentID, this.dtFechaCorte.DateTime, this.masterGestionCob.Value,this.masterCliente.Value,this.txtLibranza.Text, Convert.ToByte(this.cmbEstado.EditValue), Convert.ToByte(this.cmbTipoComunic.EditValue));
                ReportExcelBase frm = new ReportExcelBase(result, this.documentID);
                frm.Show();               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "btnGenerarDocumentos_Click"));
            }
        }

        /// <summary>
        /// Boton de Cartas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarCartas_Click(object sender, EventArgs e)
        {
            this.GenerateReport();
        }

        /// <summary>
        /// Boton de mensajes
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGenerarMensajesTxt_Click(object sender, EventArgs e)
        {
            this.btnMensajesTelefono.Enabled = false;
            List<DTO_ccHistoricoGestionCobranza> histGestionCob = this._bc.AdministrationModel.HistoricoGestionCobranza_GetGestion(this.documentID, this.dtFechaCorte.DateTime.Date, string.Empty, null, null);
            if (!histGestionCob.Any(x => x.MensajeTextoInd.Value.Value))
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                return;
            }
            //NewAge.Server.Tareas.MobileHelper.sendMessageTextCliente(histGestionCob);
            HttpClient client = new HttpClient();
            string baseUrl = "http://mensajesdevoz.co/";
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string emailSender = ConfigurationManager.AppSettings["Mail.Sender"];
            string passwordSender = ConfigurationManager.AppSettings["MessageTxt.PasswordGestion"];
            foreach (DTO_ccHistoricoGestionCobranza gest in histGestionCob.FindAll(x => x.MensajeTextoInd.Value.Value &&
                                                                                    !string.IsNullOrEmpty(x.Telefono.Value) && !string.IsNullOrEmpty(x.Mensaje.Value)))
            {
                MensajeTxt msg = new MensajeTxt();
                msg.email = emailSender;
                msg.password = passwordSender;
                msg.uri_app = "sms-kkatoo";
                msg.mensaje = gest.Mensaje.Value;
                msg.SMS = "1";
                msg.timeout = "35";
                msg.id_campaign = "5584";
                msg.fecha = DateTime.Today.ToString(FormatString.DB_Date_YYYY_MM_DD);
                msg.minuto = DateTime.Now.AddMinutes(5).Minute.ToString();
                msg.hora = DateTime.Now.AddMinutes(5).Hour.ToString();
                msg.voice = "IVONA 2 Miguel";
                msg.phone = gest.Telefono.Value;
                msg.pais = "57";
                msg.user_id = "591";
                msg.id_wapp = "327";

                //Envia el mensaje a traves de una WEB API
                var serializer = new JavaScriptSerializer();
                var stringContent = new StringContent(serializer.Serialize(msg), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("api/makeCall/format/", stringContent).Result;
            }
            this.btnMensajesTelefono.Enabled = true;
        }
        
        private void btnCorreos_Click(object sender, EventArgs e)
        {
            //Trae los datos de Historico de Cobranzas del dia actual y obtiene correos
            List<DTO_ccHistoricoGestionCobranza> histGestionCob = this._bc.AdministrationModel.HistoricoGestionCobranza_GetGestion(this.documentID, this.dtFechaCorte.DateTime.Date, string.Empty, null, null);
            List<DTO_CorreoCliente> correos = histGestionCob.Count > 0 ? this._bc.AdministrationModel.GetCorreosCliente(string.Empty, true, true, true) : null;
            foreach (DTO_ccHistoricoGestionCobranza hist in histGestionCob)
            {
                #region Envia correos a los Clientes correspondientes
                DTO_CorreoCliente exist = correos.Find(x => x.ClienteID.Value == hist.ClienteID.Value);
                if (exist != null && hist.CorreoInd.Value.Value)
                {
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{fecha}", DateTime.Now.Day + " de " + DateTime.Now.ToString("MMMM"));
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{nombres}", hist.Nombre.Value);
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{credito}", hist.Libranza.Value.ToString());
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{valorcuota}", hist.VlrCuota.Value.HasValue ? hist.VlrCuota.Value.Value.ToString("n0") : " ");
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{fechavto}", hist.FechaControl.Value.HasValue ? hist.FechaControl.Value.Value.Day + " de " + hist.FechaControl.Value.Value.ToString("MMMM") + " de " + hist.FechaControl.Value.Value.Year : string.Empty);
                    hist.PlantillaEMail.Value = hist.PlantillaEMail.Value.Replace("{dato1}", hist.Dato1.Value);
                    this._bc.SendMail(this.documentID, hist.Referencia.Value, hist.PlantillaEMail.Value, exist.Correo.Value, 3);
                }
                #endregion
            }
        }

        /// <summary>
        /// Evento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterGestionCobranza_Leave(object sender, EventArgs e)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoComunic_EditValueChanged(object sender, EventArgs e)
        {
            switch (this.cmbTipoComunic.EditValue.ToString())
            {
                case "1": //Opcion Correos
                    this.btnCorreos.Visible = true;
                    this.btnMensajesTelefono.Visible = false;
                    this.btnGenerarCartas.Visible = false;
                    break;
                case "2": //Opcion Cartas
                    this.btnGenerarCartas.Visible = true;
                    this.btnMensajesTelefono.Visible = false;
                    this.btnCorreos.Visible = false;
                    break;
                case "4": //Opcion Mensajes Texto
                    this.btnMensajesTelefono.Visible = true;
                    this.btnGenerarCartas.Visible = false;
                    this.btnCorreos.Visible = false;
                    break;
                default:
                    this.btnCorreos.Visible = false;
                    this.btnMensajesTelefono.Visible = false;
                    this.btnGenerarCartas.Visible = false;
                    break;
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            this.btnImportar.Enabled = false;
            this.btnProcesar.Enabled = false;
            this.results = null;

            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;

                this.btnImportar.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.results = null;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "btnGenerarDocumentos_Click"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncosistencias_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            Thread process = new Thread(this.InconsistenciasThread);
            process.Start();
        }

        /// <summary>
        /// Evento que revisa que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
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
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    // msgComodin = string.Format(msgComodin, this.masterComodin.Value);
                    //Popiedades de un comprobante
                    DTO_ccHistoricoGestionCobranza gestion = new DTO_ccHistoricoGestionCobranza();
                    bool createDTO = true;
                    bool validList = true;
                    //Inicializacion de variables generales
                    this._listGestion = new List<DTO_ccHistoricoGestionCobranza>();
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> propInfo = typeof(DTO_ccHistoricoGestionCobranza).GetProperties().ToList();

                    //Recorre el objeto suplementario y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in propInfo)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_" + pi.Name);
                            foreach (var c in cols)
                            {
                                if (c == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                            //for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            //{
                            //    if (cols[colIndex] == colRsx)
                            //    {
                            //        colVals.Add(colRsx, string.Empty);
                            //        colNames.Add(colRsx, pi.Name);
                            //        break;
                            //    }
                            //}
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
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                gestion = new DTO_ccHistoricoGestionCobranza();
                                for (int colIndex = 0; colIndex < colNames.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de nulls y Formatos (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                        (
                                            colName != "CodConfirmacion" && colName != "FechaConfirmacion" &&
                                            colName != "CodConfirmacion" && colName != "FechaConfirmacion" &&
                                            colName != "CorreoInd" && colName != "CartaInd" &&
                                            colName != "MensajeTextoInd" && colName != "MensajeVozInd" &&
                                            colName != "ReporteInd" && colName != "LlamadaInd"  &&
                                            colName != "GestionDesc" && colName != "Destino" &&
                                            colName != "Nombre" && colName != "Libranza"
                                        ))
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

                                            PropertyInfo pi = gestion.GetType().GetProperty(colName);
                                            UDT udt = (UDT)pi.GetValue(gestion, null);
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
                                            if (!string.IsNullOrEmpty(colValue))
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "GestionDiaria.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                this.result.Details.Add(rd);
                                rd.Message = "NOK";
                                this.result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                this._listGestion.Add(gestion);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la migracion cartera
                    if (validList)
                    {
                        this.result = new DTO_TxResult();
                        this.result.Result = ResultValue.OK;
                        this.result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this._listGestion.Count; ++index)
                        {
                            gestion = this._listGestion[index];

                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this._listGestion.Count);
                            #endregion
                            #region Definicion de variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Message = "OK";
                            #endregion
                            #region Valida los DTOs (glDocumentoControl y tabla suplementaria)
                            this.ValidateDataImport(gestion, rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                this.result.Details.Add(rd);
                                rd.Message = "NOK";
                                this.result.Result = ResultValue.NOK;
                            }
                            #endregion

                            i++;
                        }
                    }
                    #endregion
                    #region Carga la informacion en la lista de importacion

                    this.Invoke(this.UpdateProgressDelegate, new object[] { 0 });

                    if (this.result.Result != ResultValue.NOK)
                        this._isOK = true;
                    else
                        this._isOK = false;

                    //this.results.Add(this.result);
                    //this.Invoke(this.endImportarDelegate);
                    #endregion
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "ImportThread"));
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
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this.results = new List<DTO_TxResult>();
                this.results.Add(this._bc.AdministrationModel.HistoricoGestionCobranza_Update(this.documentID, this._listGestion));

                this.result = null; //Lo vuelve null para poder mostrar los mensajes
                this.StopProgressBarThread();

                this._isOK = true;
                bool notOK = this.results.Any(x => x.Result == ResultValue.NOK);
                if (notOK)
                    this._isOK = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GestionDiaria.cs", "btnProcesar_Click"));
                this.StopProgressBarThread();
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
                else if (this.results != null)
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

        #region Clases

        public class MensajeTxt
        {
            public string email { get; set; }
            public string password { get; set; }
            public string uri_app { get; set; }
            public string mensaje { get; set; }
            public string SMS { get; set; }
            public string timeout { get; set; }
            public string id_campaign { get; set; }
            public string fecha { get; set; }
            public string hora { get; set; }
            public string minuto { get; set; }
            public string voice { get; set; }
            public string phone { get; set; }
            public string pais { get; set; }
            public string user_id { get; set; }
            public string id_wapp { get; set; }
        }
        #endregion

    }
}
