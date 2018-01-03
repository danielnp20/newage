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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigracionNewFact : ProcessForm
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
        private PasteOpDTO pasteRet;
        //Variables para importar
        private int colsCount;
        private string format;
        private string formatSeparator = "\t";
        //Variables de monedas
        private bool isMultiMoneda;
        private string monedaExtranjera;
        private decimal tasaCierre;
        private bool hasTasaCierre;
        private decimal valorTotal = 0;
        private bool _isOK;
        private List<DTO_MigrarFacturaVenta> _migracionList;

        #endregion

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public MigracionNewFact() 
        {
           // this.InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.MigracionNewFact;              

                InitializeComponent();
                FormProvider.LoadResources(this, this.documentID);

                //Inicializa los delegados
                this.endImportarDelegate = new EndImportar(EndImportarMethod);
                this.endProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Carga la configuracion inicial
                this._isOK = false;
                this.btnGenerar.Enabled = false;

                //Funciones para iniciar el formulario
                string periodo = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo);
                this.dtFecha.DateTime = !string.IsNullOrEmpty(periodo)? Convert.ToDateTime(periodo): DateTime.Now;
                this.dtFecha.Enabled = false;
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, true);
                this._bc.InitMasterUC(this.masterTipoFactura, AppMasters.faFacturaTipo, true, true, true, true);
                
                //Inicia las variables
                this.documentMigracionID = AppDocuments.FacturaVenta;
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
            this.monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //Carga las variables globales
            this.isMultiMoneda = this._bc.AdministrationModel.MultiMoneda;
            //Variables del formato
            this.colsCount = 0;
            this.format = string.Empty;
            #endregion
            #region Carga la info de la tasa de cierre
            if (isMultiMoneda)
            {
                Dictionary<string, string> pksTasaCierre = new Dictionary<string, string>();
                pksTasaCierre.Add("MonedaID", this.monedaExtranjera);
                pksTasaCierre.Add("PeriodoID", this.dtFecha.DateTime.ToString(FormatString.ControlDate));
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
            string formatDoc = this._bc.GetImportExportFormat(typeof(DTO_MigrarFacturaVenta), this.documentID);
            this.format = formatDoc + this.formatSeparator;
          
            // Limpia el formato
            string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
            this.colsCount = cols.Count();  
            #endregion
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="ctrl">glDocumentoControl a validar</param>
        /// <param name="suppl">DTO Sumplementario a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgInvalidDate">Fecha en periodo invalido</param>
        private void ValidateDataImport(DTO_MigrarFacturaVenta migracion, DTO_TxResultDetail rd, string msgInvalidDate)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;

            #region Valida FKs
            #region PrefijoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_PrefijoID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.PrefijoID.Value, false, true, false, AppMasters.glPrefijo);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region FacturaTipoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_FacturaTipoID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.FacturaTipoID.Value, false, true, false, AppMasters.faFacturaTipo);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region ClienteID -- NO se valida la FK aun
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ClienteID");
            rdF = this._bc.ValidGridCellValue(colRsx, migracion.ClienteID.Value.ToString(), false, false, true, false);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region ProyectoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ProyectoID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.ProyectoID.Value, false, true, false, AppMasters.coProyecto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region CentroCostoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CentroCostoID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region Moneda
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Moneda");
            rdF = this._bc.ValidGridCell(colRsx, migracion.Moneda.Value, false, true, false, AppMasters.glMoneda);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion                        
            #region ServicioID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ServicioID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.ServicioID.Value, false, true, false, AppMasters.faServicios);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region TerceroID -- NO se valida la FK aun
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_TerceroID");
            rdF = this._bc.ValidGridCellValue(colRsx, migracion.TerceroID.Value.ToString(), false, false, true, false);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion  
            #region Ciudad
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Ciudad");
            rdF = this._bc.ValidGridCell(colRsx, migracion.Ciudad.Value, false, true, false, AppMasters.glLugarGeografico);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region Reg Fiscal
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_RegFiscal");
            rdF = this._bc.ValidGridCell(colRsx, migracion.RegFiscal.Value, false, true, false, AppMasters.coRegimenFiscal);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region Act Economica
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ActEconomicaID");
            rdF = this._bc.ValidGridCell(colRsx, migracion.ActEconomicaID.Value, false, true, false, AppMasters.coActEconomica);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion            
            #region Tipo Documento
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_TipoDocumento");
            rdF = this._bc.ValidGridCell(colRsx, migracion.TipoDocumento.Value, false, true, false, AppMasters.coTerceroDocTipo);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion              
            #endregion
            #region Valida Campos adicionales
            #region FacturaItem
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_FacturaItem");
            rdF = this._bc.ValidGridCellValue(colRsx, migracion.FacturaItem.Value.ToString(), false, false, true, false);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Fecha
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Fecha");
            if (migracion.Fecha.Value == null)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Descripcion
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Descripcion");
            if (string.IsNullOrEmpty(migracion.Descripcion.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Cantidad
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Cantidad");
            if (string.IsNullOrEmpty(migracion.Cantidad.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Valor
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Valor");
            if (string.IsNullOrEmpty(migracion.Valor.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region IVA
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Iva");
            if (string.IsNullOrEmpty(migracion.Iva.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Apellido1
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Apellido1");
            if (string.IsNullOrEmpty(migracion.Apellido1.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Nombre1
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Nombre1");
            if (string.IsNullOrEmpty(migracion.Nombre1.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Direccion
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Direccion");
            if (string.IsNullOrEmpty(migracion.Direccion.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Telefono
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_Telefono");
            if (string.IsNullOrEmpty(migracion.Telefono.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region correoElectronico
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_correoElectronico");
            if (string.IsNullOrEmpty(migracion.CorreoElectronico.Value))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region AutoRetenedorInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_AutoRetenedorInd");
            if (string.IsNullOrEmpty(migracion.AutoRetenedorInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region AutoRetenedorIVAInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_AutoRetenedorIVAInd");
            if (string.IsNullOrEmpty(migracion.AutoRetenedorIVAInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region DeclaraIVAInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_DeclaraIVAInd");
            if (string.IsNullOrEmpty(migracion.DeclaraIVAInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region DeclaraRentaInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_DeclaraRentaInd");
            if (string.IsNullOrEmpty(migracion.DeclaraRentaInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region IndependienteEMPInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_IndependienteEMPInd");
            if (string.IsNullOrEmpty(migracion.IndependienteEMPInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region ExcluyeCREEInd
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_ExcluyeCREEInd");
            if (string.IsNullOrEmpty(migracion.ExcluyeCREEInd.Value.ToString()))
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx;
                rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion             
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
            else if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);
                MessageBox.Show(msg);
                validInfo = false;
            }
            else if (!this.masterTipoFactura.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoFactura.CodeRsx);
                MessageBox.Show(msg);
                validInfo = false;
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
                if(this.masterPrefijo.ValidID && this.masterTipoFactura.ValidID)
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
                    DTO_MigrarFacturaVenta mig= null;
                    this._migracionList = new List<DTO_MigrarFacturaVenta>();
                    bool createDTO = true;
                    bool validList = true;               
                    
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_MigrarFacturaVenta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_" + pi.Name);
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
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
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
                            mig = new DTO_MigrarFacturaVenta();
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < colsCount; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = mig.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(mig, null) : null;  
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
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
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
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
                                                #endregion
                                                #region Valores Numericos
                                                else  if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
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
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
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
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
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
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
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
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
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
                                                #endregion                                              
                                            }                                           
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "MigracionNewFact.cs - Creacion de DTO y validacion Formatos");
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
                                this._migracionList.Add(mig);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this._migracionList.Count; ++index)
                        {
                            mig =this._migracionList[index];

                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this._migracionList.Count);
                            i++;
                            #endregion
                            #region Definicion de variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            #endregion
                            #region Valida el DTO de Migracion
                            mig.FacturaTipoID.Value = this.masterTipoFactura.Value;
                            if (this.masterPrefijo.Value != mig.PrefijoID.Value)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = DictionaryMessages.Err_PrefixNotCompatible;
                                result.Details.Add(rdL);
                            }
                            else if (this.dtFecha.DateTime.Date.Year != mig.Fecha.Value.Value.Year ||
                                     this.dtFecha.DateTime.Date.Month != mig.Fecha.Value.Value.Month)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = DictionaryMessages.Err_InvalidDateFact;
                                result.Details.Add(rdL);
                            }
                            else
                            {
                                this.ValidateDataImport(mig, rd, msgInvalidDate);
                                if (rd.DetailsFields.Count > 0)
                                {
                                    result.Details.Add(rd);
                                    rd.Message = "NOK";
                                    result.Result = ResultValue.NOK;
                                }
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
                    this.Invoke(this.UpdateProgressDelegate, new object[] { 100 });
                    this.Invoke(this.endImportarDelegate);
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                    this.Invoke(this.UpdateProgressDelegate, new object[] { 100 });
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

                List<int> numDocs = new List<int>();
                DTO_TxResult result = this._bc.AdministrationModel.FacturaVenta_MigracionGeneral(this.documentMigracionID, this._actFlujo.ID.Value, this._migracionList, ref numDocs);
                this.StopProgressBarThread();

                this._isOK = true;
                if (result.Result == ResultValue.NOK)
                    this._isOK = false;
                else
                {
                    #region Genera los reportes

                    bool deseaImp = false;
                    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    var imprimirRep = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (imprimirRep == DialogResult.Yes)
                        deseaImp = true;

                    #endregion
                    foreach(int numDoc in numDocs)
                    {
                        string reportName;
                        string fileURl;

                        reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(AppDocuments.FacturaVenta, numDoc.ToString(), false, ExportFormatType.pdf,0,0,0);
                        if (deseaImp)
                        {
                            fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numDoc, null, reportName.ToString());
                            Process.Start(fileURl);
                        }
                    }
                }

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
