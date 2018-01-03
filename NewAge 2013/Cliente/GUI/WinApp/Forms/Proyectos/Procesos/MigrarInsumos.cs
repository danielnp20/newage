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
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class MigrarInsumos : ProcessForm
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
            
            this.btnImportarInsumo.Enabled = true;
            this.btnImportProveedor.Enabled = true;
            this.btnImportAnalisis.Enabled = true;
            this.btnImportAPU.Enabled = true;

            if (this._isOK && this.result != null)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });

                this.btnInconsistencias.Enabled = this.result.Details.Count > 0 ? true : false;
                this.btnProcesar.Enabled = true;
            }
            else
            {
                this.btnInconsistencias.Enabled = !this.pasteRet.Success ? false : true;
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
            
            this.btnImportarInsumo.Enabled = true;
            this.btnImportProveedor.Enabled = true;
            this.btnImportAnalisis.Enabled = true;
            this.btnImportAPU.Enabled = true;

            this.btnProcesar.Enabled = true;

            if (this.result != null && this.result.Result == ResultValue.NOK)
            {
                this.btnInconsistencias.Enabled = true;
                this.validarInconsistencias = true;
                this._isOK = false;
            }
            else if (this.result != null && this.result.Details == null || this.result.Details.Count == 0)
            {
                MessageForm frm = new MessageForm(this.result);
                this.Invoke(this.ShowResultDialogDelegate, new Object[] { frm });
                this._isOK = true;
                this.dataInsumos = new List<DTO_MigracionInsumos>();
                this.dataProveedor = new List<DTO_MigracionProveedor>();
                this.dataGrupos = new List<DTO_MigracionGrupos>();
                this.dataAPU = new List<DTO_MigracionAPU>();
            }
            else
            {
                this.btnInconsistencias.Enabled = true;
                this._isOK = true;
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
        private string formatInsumo;
        private string formatProveedor;
        private string formatGruposCap;
        private string formatAPU;
        private string formatSeparator = "\t";
        //Variables del formulario
        private bool _isOK;
        private List<DTO_MigracionInsumos> dataInsumos;
        private List<DTO_MigracionProveedor> dataProveedor;
        private List<DTO_MigracionGrupos> dataGrupos;
        private List<DTO_MigracionAPU> dataAPU;
        private string monedaLoc = string.Empty;
        private string monedaExtr = string.Empty;
        private DateTime periodo = DateTime.Now;
        private DTO_MigracionInsumos insumo = new DTO_MigracionInsumos();
        private bool validarInconsistencias = false;
        //Variables para la importacion
        DTO_TxResult result;
        List<DTO_TxResult> results;
        List<string> codigos = new List<string>();
        private string reportName;
        private string fileURl;
        #endregion

        public MigrarInsumos()
        {
         // InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppProcess.MigrarInsumos;

            this.InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);

            //Inicializa los delegados
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);
            this.endProcesarDelegate = new EndProcesar(this.EndProcesarMethod);
            this.endInconsistenciasDelegate = new EndInconsistencias(this.EndInconsistenciasMethod);

            //Carga la configuracion inicial
            this._isOK = false;

            this.dataInsumos = new List<DTO_MigracionInsumos>();
            this.dataProveedor = new List<DTO_MigracionProveedor>();
            this.dataGrupos = new List<DTO_MigracionGrupos>();
            this.dataAPU = new List<DTO_MigracionAPU>();
            this.btnProcesar.Enabled = false;
            this.btnInconsistencias.Enabled = false;

            //Variables por def
            this.monedaLoc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtr = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_Periodo);

            //Asigna el formato
            this.formatInsumo = _bc.GetImportExportFormat(typeof(DTO_MigracionInsumos), AppProcess.MigrarInsumos);
            this.formatProveedor = _bc.GetImportExportFormat(typeof(DTO_MigracionProveedor), AppProcess.MigrarInsumos);
            this.formatGruposCap = _bc.GetImportExportFormat(typeof(DTO_MigracionGrupos), AppProcess.MigrarInsumos);
            this.formatAPU = _bc.GetImportExportFormat(typeof(DTO_MigracionAPU), AppProcess.MigrarInsumos);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dtoInsumo">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_MigracionInsumos dtoInsumo, DTO_MigracionProveedor dtoProveedor, DTO_MigracionGrupos dtoGrupos,DTO_MigracionAPU dtoAPU, DTO_TxResultDetail rd, string msgFkNotFound, string msgEmptyField)
        {
            try
            {
                if (dtoInsumo != null) 
                {                    
                    #region Validacion Codigo
                    if (string.IsNullOrWhiteSpace(dtoInsumo.Codigo.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Codigo");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                     
                    #region Validacion MonedaID
                    if (string.IsNullOrWhiteSpace(dtoInsumo.MonedaID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Moneda");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    else
                    {
                        if(!dtoInsumo.MonedaID.Value.Equals(this.monedaLoc) && !dtoInsumo.MonedaID.Value.Equals(this.monedaExtr))
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Moneda");
                            rdF.Message = string.Format(msgFkNotFound,dtoInsumo.MonedaID.Value);
                            rd.DetailsFields.Add(rdF);
                        }
                    }
                    #endregion                        
                    #region Validacion Tipo_Grupo
                    if (string.IsNullOrWhiteSpace(dtoInsumo.Tipo_Grupo.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Tipo_Grupo");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                        
                }
                else if (dtoProveedor != null)
                {                    
                    #region Validacion Codigo
                    if (string.IsNullOrWhiteSpace(dtoProveedor.Codigo.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Codigo");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Nombre
                    if (string.IsNullOrWhiteSpace(dtoProveedor.Nombre.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Nombre");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Direccion
                    if (string.IsNullOrWhiteSpace(dtoProveedor.Direccion.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Direccion");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                                            
                    #region Validacion Ciudad
                    if (string.IsNullOrWhiteSpace(dtoProveedor.Ciudad.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Ciudad");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                                            
                    #region Validacion Identificacion
                    if (string.IsNullOrWhiteSpace(dtoProveedor.Identificacion.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Identificacion");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                        
                }
                else if (dtoGrupos != null)
                {
                    #region Validacion CapituloGrupoID
                    if (string.IsNullOrWhiteSpace(dtoGrupos.CapituloGrupoID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_CapituloGrupoID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion CapituloTareaID
                    if (string.IsNullOrWhiteSpace(dtoGrupos.CapituloTareaID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_CapituloTareaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Descriptivo
                    if (string.IsNullOrWhiteSpace(dtoGrupos.Descriptivo.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Descriptivo");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                    
                    
                }
                else if (dtoAPU != null)
                {
                    #region Validacion TareaID
                    if (string.IsNullOrWhiteSpace(dtoAPU.TareaID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_TareaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                    
                    #region Validacion Cantidad
                    if (string.IsNullOrWhiteSpace(dtoAPU.Factor.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_Factor");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Componentes
                    if (string.IsNullOrWhiteSpace(dtoAPU.RecursoID.Value.ToString()))                        
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos + "_EmptyComponente");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        private void GenerateTemplates(byte tipoPlantilla)
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = new string[10];
                switch (tipoPlantilla)
                {
                    case 1 :
                         cols = this.formatProveedor.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                         break;
                    case 2:
                         cols = this.formatInsumo.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case 3:
                        cols = this.formatGruposCap.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case 4:
                        cols = this.formatAPU.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                        break;
                }
                
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
                throw ex;
            }
        }

        #endregion

        #region Eventos Formulario
   
        /// <summary>
        /// Boton para limpiar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, EventArgs e)
        {
            if (this.dataInsumos.Count > 0)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.dataInsumos = new List<DTO_MigracionInsumos>();
                    this.dataProveedor = new List<DTO_MigracionProveedor>();
                    this.dataGrupos = new List<DTO_MigracionGrupos>();
                    this.dataAPU = new List<DTO_MigracionAPU>();
                    this.btnProcesar.Enabled = false;
                    this.btnInconsistencias.Enabled = false;

                    this._isOK = false;                   
                    this.monedaLoc = string.Empty;

                    this.results = null;
                    this.result = null;
                }
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportarInsumo_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            //if (this.dataInsumos.Count > 0)
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;               
            //}
            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatInsumo);
                this.btnImportarInsumo.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ImportThreadInsumos);
                process.Start();
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportarProveedor_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            //if (this.dataProveedor.Count > 0)
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;
            //}
            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatProveedor);
                this.btnImportProveedor.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ImportThreadProveedor);
                process.Start();
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportarGrupos_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            //if (this.dataAnalisis.Count > 0)
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;
            //}
            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatGruposCap);
                this.btnImportAnalisis.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ImportThreadGrupos);
                process.Start();
            }
        }

        /// <summary>
        /// Evento que importa los registros de la plantilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportarAPU_Click(object sender, EventArgs e)
        {
            bool loadData = true;
            //if (this.dataAPU.Count > 0)
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;
            //}
            if (loadData)
            {
                this.Enabled = false;

                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatAPU);
                this.btnImportAPU.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ImportThreadAPU);
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
            bool loadData = true;
            //if (this.dataInsumos.Count > 0 )
            //{
            //    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
            //    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

            //    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
            //        loadData = false;
            //}

            if (loadData)
            {
                this.Enabled = false;

                this.btnImportarInsumo.Enabled = false;
                this.btnImportProveedor.Enabled = false;
                this.btnImportAnalisis.Enabled = false;
                this.btnImportAPU.Enabled = false;
                this.btnProcesar.Enabled = false;
                this.btnInconsistencias.Enabled = false;

                this.results = null;
                this.result = null;

                Thread process = new Thread(this.ProcesarThread);
                process.Start();
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

        /// <summary>
        /// Evento que muestra las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            SimpleButton txt = (SimpleButton)sender;
            switch (txt.Name)
            {
                case "btnTemplate1" :
                    this.GenerateTemplates(1);
                    break;
                case "btnTemplate2":
                    this.GenerateTemplates(2);
                    break;
                case "btnTemplate3":
                    this.GenerateTemplates(3);
                    break;
                case "btnTemplate4":
                    this.GenerateTemplates(4);
                    break;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadInsumos()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.dataInsumos = new List<DTO_MigracionInsumos>();
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
                    //Popiedades
                    DTO_MigracionInsumos insumo = new DTO_MigracionInsumos();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatInsumo.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigracionInsumos).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos.ToString() + "_" + pi.Name);
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
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

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
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                insumo = new DTO_MigracionInsumos();
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

                                        PropertyInfo pi = insumo.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(insumo, null);
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
                                        else if (colValue != string.Empty)
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
                                            else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
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
                                        {
                                            if (udt.ColRsx.Equals("Nombre") && colValue.Length > 100)
                                                colValue = colValue.Substring(0,99);
                                            else if (udt.ColRsx.Equals("Modelo/Ref") && colValue.Length > 20)
                                                colValue = colValue.Substring(0, 19);
                                            udt.SetValueFromString(colValue);
                                        }
                                            
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionInsumos.cs - Creacion de DTO y validacion Formatos");
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
                                this.dataInsumos.Add(insumo);
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
                        foreach (DTO_MigracionInsumos dto in this.dataInsumos)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this.dataInsumos.Count);
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
                            this.ValidateDataImport(dto, null, null, null, rd, msgFkNotFound, msgEmptyField);
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
                        this._isOK = true;
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.dataInsumos = new List<DTO_MigracionInsumos>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.dataInsumos = new List<DTO_MigracionInsumos>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ImportThreadInsumos");
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
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadProveedor()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.dataProveedor = new List<DTO_MigracionProveedor>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    //Popiedades de la incorporacion
                    DTO_MigracionProveedor prov = new DTO_MigracionProveedor();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatProveedor.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigracionProveedor).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos.ToString() + "_" + pi.Name);
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
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

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
                                }                                   
                            }
                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                prov = new DTO_MigracionProveedor();
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

                                        PropertyInfo pi = prov.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(prov, null);
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionInsumos.cs - Creacion de DTO y validacion Formatos");
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
                                this.dataProveedor.Add(prov);
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
                        foreach (DTO_MigracionProveedor dto in this.dataProveedor)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this.dataProveedor.Count);
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
                            this.ValidateDataImport(null, dto, null, null, rd, msgFkNotFound, msgEmptyField);
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
                         this._isOK = true;
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.dataProveedor = new List<DTO_MigracionProveedor>();
                        #endregion
                    }

                    #endregion
                    }
            }
            catch (Exception ex)
            {
                this.dataProveedor = new List<DTO_MigracionProveedor>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ImportThreadProveedor");
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
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadGrupos()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.dataGrupos = new List<DTO_MigracionGrupos>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    //Popiedades
                    DTO_MigracionGrupos analisis = new DTO_MigracionGrupos();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatGruposCap.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigracionGrupos).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos.ToString() + "_" + pi.Name);
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
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

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
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                analisis = new DTO_MigracionGrupos();
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

                                        PropertyInfo pi = analisis.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(analisis, null);
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionInsumos.cs - Creacion de DTO y validacion Formatos");
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
                                this.dataGrupos.Add(analisis);
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
                        foreach (DTO_MigracionGrupos dto in this.dataGrupos)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this.dataGrupos.Count);
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
                            this.ValidateDataImport(null, null, dto, null, rd, msgFkNotFound, msgEmptyField);
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
                        this._isOK = true;
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.dataGrupos = new List<DTO_MigracionGrupos>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.dataGrupos = new List<DTO_MigracionGrupos>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ImportThreadAnalisis");
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
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadAPU()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    this.codigos = new List<string>();
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.dataAPU = new List<DTO_MigracionAPU>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    //Popiedades
                    DTO_MigracionAPU apu = new DTO_MigracionAPU();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatAPU.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigracionAPU).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppProcess.MigrarInsumos.ToString() + "_" + pi.Name);
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
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

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
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                apu = new DTO_MigracionAPU();
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

                                        PropertyInfo pi = apu.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(apu, null);
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
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "MigracionInsumos.cs - Creacion de DTO y validacion Formatos");
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
                                this.dataAPU.Add(apu);
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
                        foreach (DTO_MigracionAPU dto in this.dataAPU)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            percent = ((i + 1) * 100) / (this.dataAPU.Count);
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
                            this.ValidateDataImport(null, null, null, dto, rd, msgFkNotFound, msgEmptyField);
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
                        this._isOK = true;
                        #endregion
                    }
                    else
                    {
                        #region Muestra mensajes de error
                        this._isOK = false;
                        this.dataAPU = new List<DTO_MigracionAPU>();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                this.dataAPU = new List<DTO_MigracionAPU>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ImportThreadAnalisis");
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

                this.result =  _bc.AdministrationModel.MigracionInsumos(this.documentID, this.dataInsumos,this.dataProveedor,this.dataGrupos, this.dataAPU);
                if (this.result == null)
                    this.result = new DTO_TxResult();
                this.StopProgressBarThread();
            }
            catch (Exception ex)
            {
                this.dataInsumos = new List<DTO_MigracionInsumos>();
                this.dataProveedor = new List<DTO_MigracionProveedor>();
                this.dataGrupos = new List<DTO_MigracionGrupos>();
                this.dataAPU = new List<DTO_MigracionAPU>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-MigracionInsumos.cs", "ProcesarThread");
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
