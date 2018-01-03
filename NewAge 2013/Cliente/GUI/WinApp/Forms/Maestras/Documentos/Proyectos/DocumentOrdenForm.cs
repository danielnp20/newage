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
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class DocumentOrdenForm : FormWithToolbar, IFiltrable
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

        protected delegate void SendToApprove();
        protected SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected virtual void SendToApproveMethod() { }

        #endregion

        #region Variables
        //Para uso general de los formularios
        public BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;

        //Variables Protegidas
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected List<int> select = new List<int>();
        protected bool multiMoneda;
        protected bool disableValidate = false;
        //Internas del formulario
        protected string areaFuncionalID;
        protected string prefijoID;
        protected string terceroID;
        protected string comprobanteID;
        protected bool dataLoaded = false;
        protected int indexFila = 0;
        protected bool isValid = true;
        protected bool deleteOP = false;
        protected bool newDoc = false;
        protected bool newReg = false;
        protected string lastColName = string.Empty;
        protected PasteOpDTO pasteRet;
        protected bool importando = false;
        //protected bool RowEdited = false;
        protected bool hasChanges = false;
        protected DTO_glConsulta consulta = null;
        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        //Variables para importar
        protected string format;
        protected string formatSeparator = "\t";
        protected string unboundPrefix = "Unbound_";

        public DTO_glDocumentoControl _ctrl = null;
        public DTO_pyProyectoDocu _doc = null;
        public List<DTO_pyServicio> _lista = null;

        public string _lineaFlujoID = string.Empty;
        public string _TareaID = string.Empty;
        public string _EtapaID = string.Empty;
        public string _TrabajoID = string.Empty;

        public List<DTO_pyLineas> _lLineasFlujo = new List<DTO_pyLineas>();
        public List<DTO_pyTareas> _lTareas = new List<DTO_pyTareas>();


        private DateTime fechaInicio;

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
        /// </summary>
        public DocumentOrdenForm()
        {
            try
            {
                //this.InitializeComponent();
                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.LoadDocumentInfo(true);
                this.AfterInitialize();
                this.CleanFormat();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentOrdenForm.cs", "DocumentOrdenForm"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
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
                        this.txtPrefix.Text = this.prefijoID;

                    this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    if (this.documentID == AppDocuments.ComprobanteManual || this.documentID == AppDocuments.DocumentoContable)
                        this.dtPeriod.Enabled = true;
                    else
                        this.dtPeriod.Enabled = false;

                    this._bc.InitMasterUC(this.uc_Prefijo, AppMasters.glPrefijo, true, true, false, false);
                    this._bc.InitMasterUC(this.uc_Proyecto, AppMasters.coProyecto, true, true, false, false);
                    this._bc.InitMasterUC(this.uc_Cliente, AppMasters.faCliente, true, true, false, false);
                    this._bc.InitMasterUC(this.uc_ClaseServicio, AppMasters.pyClaseProyecto, true, true, false, false);
                    this._bc.InitMasterUC(this.uc_AreaFuncional, AppMasters.glAreaFuncional, true, true, false, false);
                    this._bc.InitMasterUC(this.uc_Responsable, AppMasters.seUsuario, false, true, false, false);
                    this._bc.InitMasterUC(this.uc_ResponsableCliente, AppMasters.coTercero, false, true, false, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentOrdenForm", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        protected void ShowFKModal(int row, string col, ButtonEdit be)
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
        protected virtual void LoadData(bool firstTime) 
        {
            try
            {
                if (firstTime)
                {
                    DTO_pyLineas _lineaFlujo = null;

                    var lineas = (from l in this._lista.AsEnumerable()
                                  select new
                                  {
                                      LineaFlujoID = l.LineaFlujoID.Value,
                                      LineaFlujoDesc = l.LineaFlujoIDDesc.Value
                                  }
                                 ).Distinct().ToList();

                    int index = 0;
                    foreach (var item in lineas)
                    {
                        _lineaFlujo = new DTO_pyLineas();
                        _lineaFlujo.LineaFlujoID = item.LineaFlujoID;
                        _lineaFlujo.LineaFlujoIDDesc = item.LineaFlujoDesc;
                        _lineaFlujo.Index = index;

                        var etapas = (from e in this._lista.AsEnumerable()
                                      where e.LineaFlujoID.Value == item.LineaFlujoID
                                      select new
                                      {
                                          ActividadEtapaID = e.ActividadEtapaID.Value,
                                          ActividadEtapaIDDesc = e.ActividadEtapaIDDesc.Value
                                      }
                                       ).Distinct().ToList();

                        _lineaFlujo.LEtapas = (from et in etapas.AsQueryable()
                                               select new DTO_pyEtapas()
                                               {
                                                   ActividadEtapaID = et.ActividadEtapaID,
                                                   ActividadEtapaIDDesc = et.ActividadEtapaIDDesc
                                               }
                                              ).ToList();

                        _lLineasFlujo.Add(_lineaFlujo);
                        index++;
                    }
                    this.gcDocument.DataSource = _lLineasFlujo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
        }

        /// <summary>
        /// Limpia el formato de importacion segun algun documento
        /// </summary>
        protected virtual void CleanFormat() { }

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

            #region Lineas

            GridColumn lineaFlujoID = new GridColumn();
            lineaFlujoID.FieldName = this.unboundPrefix + "LineaFlujoID";
            lineaFlujoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaFlujoID");
            lineaFlujoID.UnboundType = UnboundColumnType.String;
            lineaFlujoID.VisibleIndex = 0;
            lineaFlujoID.Width = 100;
            lineaFlujoID.Visible = true;
            lineaFlujoID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(lineaFlujoID);

            GridColumn lineaFlujoIDDesc = new GridColumn();
            lineaFlujoIDDesc.FieldName = this.unboundPrefix + "LineaFlujoIDDesc";
            lineaFlujoIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaFlujoIDDesc");
            lineaFlujoIDDesc.UnboundType = UnboundColumnType.String;
            lineaFlujoIDDesc.VisibleIndex = 0;
            lineaFlujoIDDesc.Width = 200;
            lineaFlujoIDDesc.Visible = true;
            lineaFlujoIDDesc.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(lineaFlujoIDDesc);

            #endregion

            #region Etapas

            GridColumn actividadEtapaID = new GridColumn();
            actividadEtapaID.FieldName = this.unboundPrefix + "ActividadEtapaID";
            actividadEtapaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActividadEtapaID");
            actividadEtapaID.UnboundType = UnboundColumnType.String;
            actividadEtapaID.VisibleIndex = 0;
            actividadEtapaID.Width = 100;
            actividadEtapaID.Visible = true;
            actividadEtapaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(actividadEtapaID);

            GridColumn actividadEtapaIDDesc = new GridColumn();
            actividadEtapaIDDesc.FieldName = this.unboundPrefix + "ActividadEtapaIDDesc";
            actividadEtapaIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActividadEtapaIDDesc");
            actividadEtapaIDDesc.UnboundType = UnboundColumnType.String;
            actividadEtapaIDDesc.VisibleIndex = 0;
            actividadEtapaIDDesc.Width = 200;
            actividadEtapaIDDesc.Visible = true;
            actividadEtapaIDDesc.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(actividadEtapaIDDesc);

            #endregion

            #region Tareas

            GridColumn tareaID = new GridColumn();
            tareaID.FieldName = this.unboundPrefix + "TareaID";
            tareaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaID");
            tareaID.UnboundType = UnboundColumnType.String;
            tareaID.VisibleIndex = 0;
            tareaID.Width = 100;
            tareaID.Visible = true;
            tareaID.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(tareaID);

            GridColumn tareaIDDesc = new GridColumn();
            tareaIDDesc.FieldName = this.unboundPrefix + "TareaIDDesc";
            tareaIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaIDDesc");
            tareaIDDesc.UnboundType = UnboundColumnType.String;
            tareaIDDesc.VisibleIndex = 0;
            tareaIDDesc.Width = 200;
            tareaIDDesc.Visible = true;
            tareaIDDesc.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(tareaIDDesc);

            #endregion

            #region Etapas Tareas

            GridColumn trabajoID = new GridColumn();
            trabajoID.FieldName = this.unboundPrefix + "TrabajoID";
            trabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoID");
            trabajoID.UnboundType = UnboundColumnType.String;
            trabajoID.VisibleIndex = 0;
            trabajoID.Width = 100;
            trabajoID.Visible = true;
            trabajoID.OptionsColumn.AllowEdit = false;
            this.gvEtapasTareas.Columns.Add(trabajoID);

            GridColumn trabajoIDIDDesc = new GridColumn();
            trabajoIDIDDesc.FieldName = this.unboundPrefix + "TrabajoIDDesc";
            trabajoIDIDDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrabajoIDDesc");
            trabajoIDIDDesc.UnboundType = UnboundColumnType.String;
            trabajoIDIDDesc.VisibleIndex = 0;
            trabajoIDIDDesc.Width = 200;
            trabajoIDIDDesc.Visible = true;
            trabajoIDIDDesc.OptionsColumn.AllowEdit = false;
            this.gvEtapasTareas.Columns.Add(trabajoIDIDDesc);

            #endregion
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

      
        /// <summary>
        /// Evento carga el documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void txtNro_Leave(object sender, System.EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtNro.Text))
                {
                    this._ctrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.uc_Prefijo.Value, Convert.ToInt32(this.txtNro.Text));
                    if (this._ctrl != null)
                    {
                        //this._doc = this._bc.AdministrationModel.pyServicioDocu_Get(this._ctrl.NumeroDoc.Value.Value);
                        //this._lista = this._bc.AdministrationModel.pyServicioPrograma_GetOrden(this._ctrl.NumeroDoc.Value.Value);
                        this.uc_AreaFuncional.Value = this._ctrl.AreaFuncionalID.Value;
                        this.uc_Cliente.Value = this._doc.ClienteID.Value;
                        this.uc_Proyecto.Value = this._ctrl.ProyectoID.Value;
                        this.uc_Responsable.Value = this._doc.ResponsableEMP.Value;
                        this.uc_ResponsableCliente.Value = this._doc.ResponsableCLI.Value;
                        this.dtFechaIni.DateTime = DateTime.Now;
                        this.txtDescripcion.Text = this._ctrl.Descripcion.Value;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "dtPeriod_EditValueChanged"));
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
        /// busca el documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.Proyecto);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtNro.Enabled = true;
                this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.uc_Prefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        #endregion

        #region Eventos Grilla

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
        protected virtual void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) { }

        /// <summary>
        /// asigna controles a la grilla cuando entra a una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e) { }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) { }

        /// <summary>
        /// Calcula los valores y hace operaciones mientras se modifican los valores del campo en la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) { }

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
            if (!string.IsNullOrWhiteSpace(this.lastColName) && this.gvDocument.IsLastRow && this.gvDocument.FocusedColumn.FieldName == this.lastColName && e.KeyCode == Keys.Tab)
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
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                //e.Value = pi.GetValue(dto, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)fi.GetValue(dto);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                    }
                }
            }
        }
               
        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RowIndexChanged(e.FocusedRowHandle, false);
            if (this._lLineasFlujo != null && this._lLineasFlujo.Count > 0)
            {
                this._lineaFlujoID = this._lLineasFlujo[e.FocusedRowHandle].LineaFlujoID;
                this._TareaID = string.Empty;
                this._EtapaID = string.Empty;
                this._TrabajoID = string.Empty;
            }
        }

        /// <summary>
        /// Filtra por Etapa del Flujo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e) 
        {
            if (this._lLineasFlujo != null && this._lLineasFlujo.Count > 0)
            {
                var linea = this._lLineasFlujo.Where(x => x.LineaFlujoID == this._lineaFlujoID).FirstOrDefault();
                if (linea.LEtapas != null && linea.LEtapas.Count > 0)
                {
                    this._EtapaID = this._lLineasFlujo.Where(x => x.LineaFlujoID == this._lineaFlujoID).FirstOrDefault().LEtapas[e.FocusedRowHandle].ActividadEtapaID;
                    this._TareaID = string.Empty;
                    this._TrabajoID = string.Empty;
                }
            }            
        }

        /// <summary>
        /// Filtra las tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvTareas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this._lTareas != null && this._lTareas.Count > 0)
            {
                this._TareaID = this._lTareas[e.FocusedRowHandle].TareaID;
                this._EtapaID = string.Empty;
                this._TrabajoID = string.Empty;
            }
        }


        /// <summary>
        /// Filtra la grilla de etapas del las tareas 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvEtapasTareas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this._lTareas != null && this._lTareas.Count > 0)
            {
                if (this._lTareas.Count == 1)
                {
                    this._TrabajoID = this._lTareas.FirstOrDefault().LTrabajos[e.FocusedRowHandle].TrabajoID;  
                }
                else
                {
                    var tarea = this._lTareas.Where(x => x.TareaID == this._TareaID).FirstOrDefault();
                    if (tarea != null)
                    {
                        if (tarea.LTrabajos != null && tarea.LTrabajos.Count > 0)
                        {
                            this._TrabajoID = this._lTareas.Where(x => x.TareaID == this._TareaID).FirstOrDefault().LTrabajos[e.FocusedRowHandle].TrabajoID;
                        }
                    }
                }
                this._EtapaID = string.Empty;
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBGenerateTemplate()
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
                throw ex;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            //Revisa que cumple las condiciones
            if (!this.ReplaceDocument())
                return;

            this.gvDocument.ActiveFilterString = string.Empty;

            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public virtual void SaveThread() { }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public virtual void SendToApproveThread() { }

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public virtual void ImportThread() { }

        #endregion

        #region Filtrado de la grilla

        /// <summary>
        /// Asigna una consulta desde MasterQuery para hacer el filtrado de datos
        /// </summary>
        /// <param name="consulta"></param>
        /// <param name="fields"></param>
        public virtual void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            try
            {
                string filtros = Transformer.FiltrosGrilla(consulta.Filtros, fields, typeof(DTO_ComprobanteFooter));
                //this.disableValidate = true;
                this.deleteOP = true;
                this.newDoc = true;
                this.gvDocument.ActiveFilterString = filtros;
                if (this.gvDocument.RowCount > 0)
                    this.RowIndexChanged(0, true);
            }
            catch (Exception e)
            {
                ;
            }
            finally
            {
                this.disableValidate = false;
            }
        }

        #endregion

        private void dtFechaIni_EditValueChanged(object sender, EventArgs e)
        {
            this.fechaInicio = this.dtFechaIni.DateTime;
            
        }
              
    }
}
