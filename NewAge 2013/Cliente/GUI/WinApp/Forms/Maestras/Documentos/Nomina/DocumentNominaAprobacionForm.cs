using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Attributes;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class DocumentNominaAprobacionForm : FormWithToolbar
    {
        #region Delegados

        protected delegate void RefreshGrid();
        protected RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected virtual void RefreshGridMethod() { }

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected virtual void SaveMethod() { }

        
        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;

        //Variables Protegidas
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected bool multiMoneda;
        //Internas del formulario
        protected string areaFuncionalID;
        protected string prefijoID;
        protected string terceroID;
        protected bool newDoc = false;
        protected bool newReg = false;
        protected string lastColName = string.Empty;
        protected PasteOpDTO pasteRet;
        protected bool importando = false;
        //protected bool RowEdited = false;
        protected bool hasChanges = false;
        //Variables para importar
        protected string format = string.Empty;
        protected string formatSeparator = "\t";
        protected string unboundPrefix = "Unbound_";
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;
        protected bool noGeneraActividad = false;
        
        protected Dictionary<string, string> documentos = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }
              

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>)
        public DocumentNominaAprobacionForm()
        {
            try
            {
                //this.InitializeComponent();
                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AfterInitialize();
                this.CleanFormat();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "DocumentNominaAprobacionForm"));
            }
        }

        #region Funciones Privadas
        
        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            this.IsModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        #endregion

        #region Funciones Virtuales (Abstractas)

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected virtual void LoadData() { }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected virtual int GetMasterDocumentID(string colName) { return 0; }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected virtual void RowIndexChanged(int fila, bool oper) { }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);
                    

            this.LoadDocumentInfo(true);
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected virtual void CleanFormat() { }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected virtual void LookUpDocumentosDataSource() 
        {
            this.lookUpDocumentos.Properties.ValueMember = "Key";
            this.lookUpDocumentos.Properties.DisplayMember = "Value";
            this.lookUpDocumentos.Properties.DataSource = this.documentos;
            this.lookUpDocumentos.EditValue = this.documentID;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() { }

        /// <summary>
        /// Habilita o deshabilita la barra de herramientas segun donde el usuario este
        /// </summary>
        protected virtual void ValidHeaderTB() { }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// </summary>
        /// <param name="aux">Informacion del temporal</param>
        protected virtual void LoadTempData(object aux) { }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            //Campo de marca
            GridColumn marca = new GridColumn();
            marca.FieldName = this.unboundPrefix + "Seleccionar";
            marca.UnboundType = UnboundColumnType.Boolean;
            marca.VisibleIndex = 0;
            marca.Width = 40;
            marca.Visible = true;
            marca.Fixed = FixedStyle.Left;
            marca.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            marca.OptionsColumn.ShowCaption = false;
            this.gvDocument.Columns.Add(marca);
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected virtual void LoadEditGridData(bool isNew, int rowIndex) { }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected virtual void AddNewRow() { }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidateRow(int fila)
        {
            return true;
        }

        /// <summary>
        /// Valida una celda que tiene una llave Foranea
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <param name="colName">Nombre de la columna sin el unbound</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="isFK">Indica si la celda corresponde a una llave foranea</param>
        /// <param name="isHierarchy">Indica si es un control de jerarquia</param>
        /// <param name="FKDocID">Documento Id de la FK</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidGridCell(int fila, string colName, bool acceptNull, bool isFK, bool isHierarchy, int? FKDocID)
        {
            string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxFK = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
            string rsxNotLeaf = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotLeaf);

            string msg;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + colName];
            string colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
            {
                msg = string.Format(rsxEmpty, col.Caption);
                this.gvDocument.SetColumnError(col, msg);
                return false;
            }
            else if (!string.IsNullOrEmpty(colVal) && isFK)
            {
                DTO_MasterBasic dto;
                if (isHierarchy)
                    dto = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, FKDocID.Value, false, colVal, true);
                else
                    dto = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, FKDocID.Value, false, colVal, true);

                if (dto == null)
                {
                    msg = string.Format(rsxFK, colVal);
                    this.gvDocument.SetColumnError(col, msg);
                    return false;
                }
                else if (isHierarchy)
                {
                    DTO_MasterHierarchyBasic h = (DTO_MasterHierarchyBasic)dto;
                    if (!h.MovInd.Value.Value)
                    {
                        msg = string.Format(rsxNotLeaf, colVal);
                        this.gvDocument.SetColumnError(col, msg);
                        return false;
                    }
                }
            }

            this.gvDocument.SetColumnError(col, string.Empty);
            return true;
        }

        /// <summary>
        /// Valida una celda de valor es valida
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <param name="colName">Nombre de la columna sin el unbound</param>
        /// <param name="acceptNull">Indica si la celda acepta valores vacios o no</param>
        /// <param name="acceptCero">Indica si la celda acepta ceros como valor</param>
        /// <param name="OnlyPositive">Indica si la celda acepta solo numeros positivos</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidGridCellValue(int fila, string colName, bool acceptNull, bool acceptCero, bool OnlyPositive, bool invalidImp)
        {
            string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            string rsxDouble = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DoubleField);
            string rsxCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
            string rsxPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
            string rsxInvalidImp = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_InvalidImpValue);

            string msg;
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + colName];
            string colVal = this.gvDocument.GetRowCellValue(fila, col).ToString();

            if (string.IsNullOrEmpty(colVal) && !acceptNull)
            {
                msg = string.Format(rsxEmpty, col.Caption);
                this.gvDocument.SetColumnError(col, msg);
                return false;
            }
            else if (!string.IsNullOrEmpty(colVal))
            {
                decimal val = 0;
                try
                {
                    val = Convert.ToDecimal(colVal, CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    msg = string.Format(rsxDouble, colVal);
                    this.gvDocument.SetColumnError(col, msg);
                    return false;
                }

                if (!acceptCero && val == 0)
                {
                    msg = string.Format(rsxCero, colVal);
                    this.gvDocument.SetColumnError(col, msg);
                    return false;
                }

                if (OnlyPositive && val < 0)
                {
                    msg = string.Format(rsxPositive, colVal);
                    this.gvDocument.SetColumnError(col, msg);
                    return false;
                }

                if (invalidImp)
                {
                    this.gvDocument.SetColumnError(col, rsxInvalidImp);
                    return false;
                }
            }

            this.gvDocument.SetColumnError(col, string.Empty);
            return true;
        }

        /// <summary>
        /// Revisa si el documento actual tiene temporales
        /// </summary>
        /// <returns></returns>
        protected virtual bool HasTemporales()
        {
            return _bc.AdministrationModel.aplTemporales_HasTemp(this.documentID.ToString(), _bc.AdministrationModel.User);
        }

        /// <summary>
        /// Actualiza la informacion de los temporales
        /// </summary>
        protected virtual void UpdateTemp(object data)
        {
            try
            {
                _bc.AdministrationModel.aplTemporales_Save(this.documentID.ToString(), _bc.AdministrationModel.User, data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Pregunta si desea reemplazar el documento actual por una nueva fuente de datos
        /// </summary>
        /// <returns></returns>
        protected virtual bool ReplaceDocument()
        {
            string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

            if (!this.hasChanges)
                return true;

            if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;

            return false;
        }

        /// <summary>
        /// Trae los filtros complejos necesarios para la pnatalla de una maestra
        /// </summary>
        /// <param name="docIdFK"></param>
        /// <returns></returns>
        protected virtual List<DTO_glConsultaFiltro> GetFiltroComplejo(int docIdFK)
        {
            return null;
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        protected void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                    {
                        this.txtPrefix.Text = this.prefijoID;
                    }

                    this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.no_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;

                    #region Carga la info de las actividades
                    if (!this.noGeneraActividad)
                    {
                        List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                        if (actividades.Count != 1)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                            MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                        }
                        else
                        {
                            this.actividadFlujoID = actividades[0];
                        }

                        if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                            this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "-LoadDocumentInfo"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemDelete.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                    FormProvider.Master.itemCopy.Enabled = false;
                    FormProvider.Master.itemPaste.Enabled = false;
                    FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemRevert.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.HasTemporales())
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgLostInfo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                    if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                        if (this.importando)
                            e.Cancel = true;
                        else
                            FormProvider.Master.Form_Closing(this, this.documentID);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtPeriod_EditValueChanged()
        {
            try
            {
                EstadoPeriodo validPeriod = _bc.AdministrationModel.CheckPeriod(this.dtPeriod.DateTime, this.frmModule);
                if (this.dtPeriod.Enabled && validPeriod != EstadoPeriodo.Abierto)
                {
                    if (validPeriod == EstadoPeriodo.Cerrado)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoCerrado));
                    if (validPeriod == EstadoPeriodo.EnCierre)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_PeriodoEnCierre));

                    this.dtPeriod.Focus();
                }
                else
                {
                    int currentMonth = this.dtPeriod.DateTime.Month;
                    int currentYear = this.dtPeriod.DateTime.Year;
                    int minDay = 1;
                    int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

                    this.dtFecha.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
                    this.dtFecha.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
                    this.dtFecha.DateTime = new DateTime(currentYear, currentMonth, minDay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentNominaAprobacionForm.cs", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFecha_Enter(object sender, EventArgs e) { }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtFecha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.dtFecha.Text))
            {
                this.dtFecha.DateTime = this.dtFecha.Properties.MinValue;
            }
        }

        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Evento encargado de seleccionar todos los registros de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void chkSeleccionarTodos_CheckedChanged(object sender, System.EventArgs e)
        {
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Cambia el Documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void lookUpDocumentos_EditValueChanged(object sender, EventArgs e)
        {
            this.documentID = Convert.ToInt32(lookUpDocumentos.EditValue);
        }           

        #endregion

        #region Eventos Grilla Encabezado

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gcDocument_Enter(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e) { }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) 
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Seleccionar")
            {
                e.RepositoryItem = this.editChkBox;
            }
            if (fieldName == "Observacion")
            {
                e.RepositoryItem = this.editRichControl;
            }  
        }

        /// <summary>
        /// asigna controles a la grilla cuando entra a una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e) 
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Observacion")
                {
                    e.RepositoryItem = this.editPopupRich;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) { }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al cambiar el foco de una columna a otra
        /// Cambia el estilo de una celda segun las condiciones del formulario
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e) { }

        /// <summary>
        /// Revisa botones al digitar algo sobre la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.gvDocument.IsLastRow && this.gvDocument.FocusedColumn.FieldName == this.lastColName && e.KeyCode == Keys.Tab)
            {
                bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                if (isV)
                {
                    this.newReg = true;
                    this.AddNewRow();
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
                this.RowIndexChanged(e.FocusedRowHandle, false);
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e) {}          

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Cambia valor de los campos chek
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void editChkBox_EditValueChanged(object sender, EventArgs e)  {}

        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvPreliminar_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e) { }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvPreliminar_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e) { }
                      

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void editRich_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocument.GetFocusedRowCellValue(fieldName).ToString();
        }      

         //<summary>
         //Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
         //</summary>
         //<param name="sender">Objeto que envia el evento</param>
         //<param name="e">Evento</param>
        protected virtual void editRich_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas


        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public virtual void SaveThread() { }
        
        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public virtual void ImportThread() { }

        #endregion     
    }
}
