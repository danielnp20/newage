﻿using System;
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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigracionCartera : ProcessForm
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

            this.btnTemplate.Enabled = true;
            this.btnImport.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this.btnGenerar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = !this.pasteRet.Success ? false : true;
                this.btnGenerar.Enabled = false;
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

            this.btnTemplate.Enabled = true;
            this.btnImport.Enabled = true;

            if (this._isOK)
            {
                MessageForm frm = new MessageForm(this.results);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnInconsistencias.Enabled = false;
            }
            else
                this.btnInconsistencias.Enabled = true;

            this.btnGenerar.Enabled = false;
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
        private string areaFuncionalID;
        private string reportName;
        private string fileURl;

        //Variables para importar
        private string format;
        private string formatSeparator = "\t";

        //Variables de monedas
        private string monedaLocal;

        //Variables del formulario
        private bool _isOK;
        private List<DTO_MigracionCartera> _creditos;
        private Dictionary<string, string> _cachePagadurias = new Dictionary<string, string>();
        private Dictionary<string, List<string>> _cacheComponentes = new Dictionary<string, List<string>>();
        private string codCarteraPropia = string.Empty;
        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        //public MigracionCartera() { }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.MigracionCartera;

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);
                this.endInconsistenciasDelegate = new EndInconsistencias(EndInconsistenciasMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                //Funciones para iniciar el formulario
                _bc.InitPeriodUC(this.dtPeriod, 0);
                _bc.InitMasterUC(this.masterComodin, AppMasters.ccCarteraComponente, false, true, true, false);

                //Inicia las variables
                this.dtPeriod.Enabled = false;
                this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                this.dtPeriod.DateTime = Convert.ToDateTime(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_MesProceso));
                this.format = _bc.GetImportExportFormat(typeof(DTO_MigracionCartera), this.documentID);

                //Carga las variables de glControl
                this.codCarteraPropia = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionCartera.cs", "InitForm"));
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="credito">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgLibranza">Mensaje de error para elvalor de la libranza</param>
        /// <param name="msgTipoEstado">Mensaje de restriccion para el tipo de estado</param>
        /// <param name="msgLineaComponentes">Mensaje para indicar que no hay relacion entre la linea de credito y los componentes de cartera</param>
        /// <param name="msgComodin">Mensaje que indica que la linea de credito no tiene el componente comodin</param>
        private void ValidateDataImport(DTO_MigracionCartera credito, DTO_TxResultDetail rd, string msgComponentes, string msgLibranza, string msgTipoEstado, string msgLineaComponentes,
            string msgComodin, string msgCompradorCartera, string msgCompradorCarteraPropia, string msgCuotasVendidas, string msgTasaVenta, string msgTipoVenta, string msgInvalidValorVenta)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;

            #region Valida las FKs
            #region ClienteID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ClienteID");
            rdF = _bc.ValidGridCell(colRsx, credito.ClienteID.Value, false, true, false, AppMasters.ccCliente);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CompradorID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CompradorID");
            rdF = _bc.ValidGridCell(colRsx, credito.CompradorID.Value, true, true, false, AppMasters.ccCompradorCartera);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region LineaCreditoID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_LineaCreditoID");
            rdF = _bc.ValidGridCell(colRsx, credito.LineaCreditoID.Value, false, true, false, AppMasters.ccLineaCredito);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region AsesorID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_AsesorID");
            rdF = _bc.ValidGridCell(colRsx, credito.AsesorID.Value, false, true, false, AppMasters.ccAsesor);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CentroPagoID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CentroPagoID");
            rdF = _bc.ValidGridCell(colRsx, credito.CentroPagoID.Value, false, true, false, AppMasters.ccCentroPagoPAG);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region ZonaID
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ZonaID");
            rdF = _bc.ValidGridCell(colRsx, credito.ZonaID.Value, false, true, false, AppMasters.glZona);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Ciudad
            colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Ciudad");
            rdF = _bc.ValidGridCell(colRsx, credito.Ciudad.Value, false, true, true, AppMasters.glLugarGeografico);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
            #region Valida el valor de la libranza
            decimal aux = Math.Round(credito.VlrCuota.Value.Value * credito.Plazo.Value.Value, 2);
            if (credito.VlrLibranza.Value.Value != aux)
            {
                createDTO = false;

                rdF = new DTO_TxResultDetailFields();
                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrLibranza");
                rdF.Message = msgLibranza;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Valida el valor de los componentes
            decimal tempValores = credito.VlrComponente1.Value.Value + credito.VlrComponente2.Value.Value + credito.VlrComponente3.Value.Value + credito.VlrComponente4.Value.Value +
                                  credito.VlrComponente5.Value.Value + credito.VlrComponente6.Value.Value + credito.VlrComponente7.Value.Value + credito.VlrComponente8.Value.Value +
                                  credito.VlrComponente9.Value.Value + credito.VlrComponente10.Value.Value;
            tempValores = Math.Round(tempValores, 2);
            if (credito.VlrLibranza.Value.Value != tempValores)
            {
                createDTO = false;

                rdF = new DTO_TxResultDetailFields();
                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrComponentes");
                rdF.Message = msgComponentes;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Valida el tipo de credito
            if (credito.TipoEstado.Value.Value != 1 && credito.TipoEstado.Value.Value != 2)
            {
                createDTO = false;

                rdF = new DTO_TxResultDetailFields();
                rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_TipoEstado");
                rdF.Message = msgTipoEstado;
                rd.DetailsFields.Add(rdF);
            }

            #endregion
            #region Valida la linea de credito y el componente comodin
            if (createDTO)
            {
                if (!this._cacheComponentes.ContainsKey(credito.LineaCreditoID.Value))
                {
                    DTO_glConsulta consulta = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                    filtros.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "LineaCreditoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = credito.LineaCreditoID.Value,
                    });
                    consulta.Filtros = filtros;
                    IEnumerable<DTO_MasterComplex> compsObj = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.ccLineaComponente, 30, 1, consulta, true).ToList();
                    IEnumerable<DTO_ccLineaComponente> comps = compsObj.Cast<DTO_ccLineaComponente>();
                    List<string> ids = (from c in comps select c.ComponenteCarteraID.Value).ToList();

                    this._cacheComponentes.Add(credito.LineaCreditoID.Value, ids);
                }

                if (this._cacheComponentes[credito.LineaCreditoID.Value].Count == 0)
                {
                    createDTO = false;

                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_LineaCreditoID");
                    rdF.Message = msgLineaComponentes;
                    rd.DetailsFields.Add(rdF);
                }
                else if (!this._cacheComponentes[credito.LineaCreditoID.Value].Contains(this.masterComodin.Value))
                {
                    createDTO = false;

                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_LineaCreditoID");
                    rdF.Message = msgComodin;
                    rd.DetailsFields.Add(rdF);
                }
            }
            #endregion
            #region Valida si en cartera propia o cedida
            if (credito.TipoEstado.Value == 1)
            {
                if (credito.CompradorID.Value != this.codCarteraPropia && !String.IsNullOrWhiteSpace(credito.CompradorID.Value))
                {
                    createDTO = false;

                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CompradorID");
                    rdF.Message = string.Format(msgCompradorCarteraPropia, credito.CompradorID.Value);
                    rd.DetailsFields.Add(rdF);
                }
            }
            else
            {
                if (credito.CompradorID.Value == this.codCarteraPropia)
                {
                    createDTO = false;

                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CompradorID");
                    rdF.Message = string.Format(msgCompradorCartera, credito.CompradorID.Value);
                    rd.DetailsFields.Add(rdF);
                }

                if (credito.VlrVenta.Value.Value <= 0)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrVenta");
                    rdF.Message = msgInvalidValorVenta;
                    rd.DetailsFields.Add(rdF);
                }

                if (credito.TasaVenta.Value.Value <= 0)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_TasaVenta");
                    rdF.Message = msgTasaVenta;
                    rd.DetailsFields.Add(rdF);
                }

                if (credito.NumCuotaVendidas.Value.Value <= 0)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_NumCuotaVendidas");
                    rdF.Message = msgCuotasVendidas;
                    rd.DetailsFields.Add(rdF);
                }
            }
            #endregion
            #region Asigna los valores por defecto
            if (createDTO)
            {
                //Credito
                if (!this._cachePagadurias.ContainsKey(credito.CentroPagoID.Value))
                {
                    DTO_ccCentroPagoPAG centroPag = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, credito.CentroPagoID.Value, true);
                    this._cachePagadurias.Add(credito.CentroPagoID.Value, centroPag.PagaduriaID.Value);
                }

                credito.VlrCredito.Value = Convert.ToInt32(credito.VlrCredito.Value);
                credito.PagaduriaID.Value = this._cachePagadurias[credito.CentroPagoID.Value];
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

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionCartera.cs", "btnTemplate_Click"));
            }
        }

        /// <summary>
        /// Procesa el cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!this.masterComodin.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblComodin.Text);
                MessageBox.Show(msg);
            }
            else
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                this.btnImport.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnGenerar.Enabled = false;
                this.results = null;

                this._cacheComponentes = new Dictionary<string, List<string>>();
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
                this.Enabled = false;

                this.btnImport.Enabled = false;
                this.btnTemplate.Enabled = false;
                this.btnGenerar.Enabled = false;
                this.results = null;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionCartera.cs", "btnGenerarDocumentos_Click"));
            }
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
                    string msgInvalidVlrComponentes = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidVlrComponentes);
                    string msgInvalidVlrLibranza = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidVlrLibranza);
                    string msgLineaComponentes = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LineaCreditoSinComp);
                    string msgTipoEstado = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_TipoEstadoConstraint);
                    string msgComodin = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoCompComodin);
                    string msgCompradorCartera = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNotValid);
                    string msgCompradorCarteraPropia = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraPropiaNotValid);
                    string msgInvalidValorVenta = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_VlrVentaMigracion);
                    string msgTasaVenta = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_TasaVenta);
                    string msgTipoVenta = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_TipoVenta);
                    string msgCuotasVendidas = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CuotasVendidas);
                    msgComodin = string.Format(msgComodin, this.masterComodin.Value);
                    //Popiedades de un comprobante
                    DTO_MigracionCartera credito = new DTO_MigracionCartera();
                    bool createDTO = true;
                    bool validList = true;
                    //Inicializacion de variables generales
                    this._creditos = new List<DTO_MigracionCartera>();
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> propInfo = typeof(DTO_MigracionCartera).GetProperties().ToList();

                    //Recorre el objeto suplementario y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in propInfo)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_" + pi.Name);
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
                                credito = new DTO_MigracionCartera();
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
                                            colName != "CompradorID" &&
                                            colName != "TipoVenta" &&
                                            colName != "VlrVenta" &&
                                            colName != "NumCuotaVendidas" &&
                                            colName != "TasaVenta" &&
                                            colName != "NumeroCesion" &&
                                            colName != "FlujosPago" &&
                                            colName != "PorcentajeInteres" &&
                                            colName != "ValorComponente1" &&
                                            colName != "ValorComponente2" &&
                                            colName != "ValorComponente3" &&
                                            colName != "ValorComponente4" &&
                                            colName != "ValorComponente5" &&
                                            colName != "ValorComponente6" &&
                                            colName != "ValorComponente7" &&
                                            colName != "ValorComponente8" &&
                                            colName != "ValorComponente9" &&
                                            colName != "ValorComponente10"
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

                                            PropertyInfo pi = credito.GetType().GetProperty(colName);
                                            UDT udt = (UDT)pi.GetValue(credito, null);
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
                                                        decimal val = Convert.ToDecimal(colValue);
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionCartera.cs - Creacion de DTO y validacion Formatos");
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
                                this._creditos.Add(credito);
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

                        for (int index = 0; index < this._creditos.Count; ++index)
                        {
                            credito = this._creditos[index];

                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this._creditos.Count);
                            #endregion
                            #region Definicion de variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            #endregion
                            #region Valida los DTOs (glDocumentoControl y tabla suplementaria)
                            this.ValidateDataImport(credito, rd, msgInvalidVlrComponentes, msgInvalidVlrLibranza, msgTipoEstado, msgLineaComponentes, msgComodin, msgCompradorCartera,
                                msgCompradorCarteraPropia, msgCuotasVendidas, msgTasaVenta, msgTipoVenta, msgInvalidValorVenta);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionCartera.cs", "ImportThread"));
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

                this.results = this._bc.AdministrationModel.MigracionCartera(this.documentID, dtPeriod.DateTime, this.masterComodin.Value, this._creditos);

                this.result = null; //Lo vuelve null para poder mostrar los mensajes
                this.StopProgressBarThread();

                this._isOK = true;
                bool notOK = this.results.Any(x => x.Result == ResultValue.NOK);
                if (notOK)
                    this._isOK = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionCartera.cs", "btnProcesar_Click"));
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

    }
}
