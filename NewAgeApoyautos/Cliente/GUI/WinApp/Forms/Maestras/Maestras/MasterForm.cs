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
using NewAge.Cliente.GUI.WinApp.Reports;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using SentenceTransformer;
using System.Threading;
using NewAge.Cliente.GUI.WinApp.Reports.Formularios;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils.Win;
using System.Diagnostics;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de Maestras
    /// </summary>
    public abstract partial class MasterForm : FormWithToolbar, IFiltrable
    {
        #region Delegados

        public delegate void RefreshGrid();
        public RefreshGrid refreshGrid;

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        public void RefreshGridMethod()
        {
            this.pnSearch.Visible = false;
            this.txtCode.Text = string.Empty;
            this.txtDescrip.Text = string.Empty;
            this.filtrosConsulta = null;
            this.consulta = null;

            this.pgGrid.PageNumber = 1;
            this.LoadGridData(false, false, false);
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();

        //Variables Privadas
        private int _levelNew = 1;
        private int _currentRow = -1;
        private int _rowTemp;
        private bool _assignDate = false;
        private bool _isPeriod = false;
        //Para manejo de propiedades
        private bool _insertando;
        private DTO_aplMaestraPropiedades _frmProperties;

        protected string editGridFilterString="[Visible] = true";
        protected DTO_glEmpresa empresa;
        //Para carga de datos
        protected int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["Pagging.Master.PageSize.Main"]);
        protected OrderDirection sortOrder = OrderDirection.ASC;
        protected DTO_glTabla table;
        protected Dictionary<int, FieldConfiguration> fields = new Dictionary<int, FieldConfiguration>();
        protected static int editRowHeight=21;
        protected IEnumerable data;
        protected DTO_glConsulta consulta = null;
        protected List<DTO_glConsultaFiltro> filtrosConsulta = null;
        //Para manejo de propiedades
        protected string empresaGrupoID = string.Empty;
        protected int DocumentID;
        protected string idSelected;
        protected Type frmType;
        protected ModulesPrefix frmModule;
        protected string sortField;
        protected string unboundPrefix="Unbound_";
        //Numero de fila de edicion de los campos basicos
        protected int editrow_code;
        protected int editrow_desc;
        protected int editrow_mov;
        protected int editrow_act;
        protected int editrow_vers;
        protected int editrow_replica;
        protected int editrow_company;
        //Internas del formulario
        protected string colIdName = string.Empty;
        protected UDT_BasicID basicUDT;
        protected string exportableCols;
        protected DTO_MasterBasic selectedDto = null;
        protected FormTypes _frmType = FormTypes.Master;
        protected string _frmName;
        protected static string basicTab = "basic";
        protected bool? hasEditAccess = null;
        ///Consultas
        protected Dictionary<string, Dictionary<string, string>> combosData = new Dictionary<string, Dictionary<string, string>>();

        //Lista con las grillas creadas dinamicamente (No incluye la primera grilla)
        protected Dictionary<string, GridControl> extraGrids = new Dictionary<string, GridControl>();

        //Vista que esta siendo editada. Auxiliar para saber cual es la grilla en la que se esta trabajando
        protected GridView viewBeingEdited = null;

        //Variables de importacion
        protected string format;
        protected PasteOpDTO pasteRet;
        protected bool importando = false;
        protected bool saveNew;


        #endregion

        #region Propiedades

        /// <summary>
        /// Encapsula el estado de insertando en el formulario 
        /// </summary>
        protected virtual bool Insertando
        {
            get
            {
                return this._insertando;
            }
            set
            {
                this._insertando = value;
                FieldConfiguration fc=this.GetFieldConfigByFieldName(this.FrmProperties.ColumnaID);
                if (fc != null && !(bool)table.Jerarquica.Value)
                {
                    fc.Editable = value;
                }
                
                if (value)
                    this.selectedDto = null;


                if (!value && this.hasEditAccess != null)
                {
                    FormProvider.Master.itemSave.Enabled = this.hasEditAccess.Value;
                }
            }
        }

        /// <summary>
        /// Porpiedades generales de una tabla maestra
        /// </summary>
        public DTO_aplMaestraPropiedades FrmProperties
        {
            get { return this._frmProperties; }
            set { this._frmProperties = value; }
        }

        /// <summary>
        /// Cantidad de filas a mostrar en la grilla de edición
        /// </summary>
        protected virtual int EditVisibleRows
        {
            get
            {
                int count = 0;
                foreach (FieldConfiguration fc in this.fields.Values)
                {
                    if (fc.EditVisible)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        /// <summary>
        /// Indica el nivel de la inserción para las jerárquicas.
        /// Se sobrecarga en la maestra jerarquica
        /// </summary>
        protected virtual int LevelNew{
            get{
                return this._levelNew;
            }
            set{
                this._levelNew = value;
            }
        }

        /// <summary>
        /// Sentencia de la consulta
        /// </summary>
        public virtual List<ConsultasFields> ConsultaFiels
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public MasterForm(string mod = null)
        {           
            try
            {
                this.InitForm();
                this.RefreshRowIndexFields();
                //variables
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString());
                
                //Inicializa el formulario
                InitializeComponent();

                if (!string.IsNullOrWhiteSpace(mod))
                    this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this.frmModule, this.DocumentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                Tuple<int, string> tup = new Tuple<int, string>(this._frmProperties.DocumentoID, this.empresaGrupoID);
                this.table = _bc.AdministrationModel.Tables[tup];
                if (table.Jerarquica.Value.Value)
                {
                    //Verifica q estén configurados los niveles
                    if (table.lonNivel1.Value == null || table.lonNivel1.Value.Value == 0)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NotConfiguredMaster));
                        return;
                    }
                }
                this.empresa = _bc.AdministrationModel.Empresa;

                this.AfterInitialize();

                string ps = _bc.GetControlValue(AppControl.PaginadorMaestra);
                if (!string.IsNullOrWhiteSpace(ps))
                    this.pageSize = Convert.ToInt32(ps);
                
                _bc.Pagging_Init(this.pgGrid, this.pageSize);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);

                this.editFecha.EditMask = FormatString.Date;
                this.editFecha.DisplayFormat.FormatString = FormatString.Date;

                this.editPeriodo.EditMask = FormatString.Period;
                this.editPeriodo.DisplayFormat.FormatString = FormatString.Period;

                this.LoadGridData(true, false, false);
                this.refreshGrid = new RefreshGrid(RefreshGridMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "MasterForm"));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Dice el tipo de columna
        /// </summary>
        /// <param name="configType">Tipo de dato</param>
        /// <returnsRetorna el tipo de dato para la columna></returns>
        private UnboundColumnType GetColumnType(Type configType)
        {
            if(configType == typeof(string))
                return UnboundColumnType.String;
            if (configType == typeof(int))
                return UnboundColumnType.Integer;
            if (configType == typeof(bool))
                return UnboundColumnType.Boolean;
            if (configType == typeof(DateTime))
                return UnboundColumnType.DateTime;
                       
            return UnboundColumnType.String;
        }

        /// <summary>
        /// Funcion que se encarga de realizar la busqueda rapida
        /// </summary>
        private void ValidateSearchData()
        {
            if (string.IsNullOrWhiteSpace(this.txtCode.Text) && string.IsNullOrWhiteSpace(this.txtDescrip.Text))
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoSearchCriteria));
                return;
            }

            this.filtrosConsulta = new List<DTO_glConsultaFiltro>();
            //Agrega el codigo al filtro
            if (!string.IsNullOrWhiteSpace(this.txtCode.Text))
            {
                DTO_glConsultaFiltro codFilter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = this.colIdName,
                    ValorFiltro = this.txtCode.Text.Trim(),
                    OperadorFiltro = OperadorFiltro.Contiene,
                    OperadorSentencia = "AND"
                };
                this.filtrosConsulta.Add(codFilter);
            }

            //Agrega la descripcion al filtro
            if (!string.IsNullOrWhiteSpace(this.txtDescrip.Text))
            {
                DTO_glConsultaFiltro descFilter = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "Descriptivo",
                    ValorFiltro = this.txtDescrip.Text.Trim(),
                    OperadorFiltro = OperadorFiltro.Contiene,
                    OperadorSentencia = "AND"
                };
                this.filtrosConsulta.Add(descFilter);
            }

            this.LoadGridData(false, true, false);
        }

        /// <summary>
        /// Funcion que permite validar si se desea sobreescribir la grilla de edicion 
        /// </summary>
        private bool ValidateOverwrite(int row)
        {
            if (!this._insertando || this.saveNew)
            {
                if (row != this._currentRow)
                {
                    this.saveNew = false;
                    this.RowSelected(row);
                }
                
                return true;
            }
            else
            {
                string msgTitleWarnig = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                string msgLostedit = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostEdit));
                if (MessageBox.Show(msgLostedit, msgTitleWarnig, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.RowSelected(row);
                    return true;
                }
                else
                    return false;
            }
        }

        #endregion

        #region Funciones protected

        /// <summary>
        /// Refresca todos los rowindex de los campos de _fields
        /// </summary>
        protected void RefreshRowIndexFields()
        {
            foreach (int key in this.fields.Keys)
            {
                fields[key].RowIndex = key;
                fields[key].ColumnIndex = key;
            }
        }

        /// <summary>
        /// Trae la informacionde configuracion de un campo dado el nombre
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Configuracion del campo en la maestra</returns>
        protected FieldConfiguration GetFieldConfigByFieldName(string fieldName)
        {
            List<FieldConfiguration> list = this.fields.Values.ToList();
            List<FieldConfiguration> list2= list.Where(x => x.FieldName.Equals(fieldName)).ToList();
            if (list2.Count > 0)
                return list2.First();
            else
                return null;
        }

        /// <summary>
        /// Trae la informacion de configuracion de un campo dado el nombre a mostrar(caption)
        /// </summary>
        /// <param name="fieldName">Nombre a mostrar del campo</param>
        /// <returns>Configuracion del campo en la maestra</returns>
        protected FieldConfiguration GetFieldConfigByCaption(string caption)
        {
            List<FieldConfiguration> list = this.fields.Values.ToList();
            return list.Where(x => x.Caption.Equals(caption)).ToList().First();
        }

        /// <summary>
        /// Trae el campo seleccionado
        /// </summary>
        /// <returns>`Retorna el campo requerido</returns>
        protected FieldConfiguration GetFocusedFieldConfig()
        {
            GridView gv = this.GetFocusedGridView();
            GridProperty gp = (GridProperty)gv.GetFocusedRow();
            return this.GetFieldConfigByCaption(gp.Campo);
        }

        /// <summary>
        /// Saca la grilla actual segun la pestaña en la que se encuentra
        /// </summary>
        /// <returns>Retorna la grilla actual</returns>
        protected GridView GetFocusedGridView()
        {
            if (this.viewBeingEdited != null)
                return viewBeingEdited;

            GridView gv = null;
            if (this.gvRecordEdit.IsFocusedView)
                gv = this.gvRecordEdit;
           
            foreach (GridControl gc in this.extraGrids.Values)
            {
                if (gc.MainView.IsFocusedView)
                    gv = (GridView)gc.MainView;
            }
           
            return gv;
        }

        /// <summary>
        /// Trae la informacion de un campo dado el nombre
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Propiedad del campo en la maestra</returns>
        protected DTO_aplMaestraCampo GetFieldByFieldName(string fieldName)
        {
            IEnumerable<DTO_aplMaestraCampo> CamposWhere = this.FrmProperties.Campos.Where(x => x.NombreColumna.Equals(fieldName));
            if (CamposWhere.Count() > 0)
                return this.FrmProperties.Campos.Where(x => x.NombreColumna.Equals(fieldName)).First();
            else
                return null;
        }

        /// <summary>
        /// Crea una nueva tab en para una nueva grilla de edición
        /// </summary>
        /// <param name="tabName">Nombre del tab</param>
        /// <returns>Grid Control generado</returns>
        protected GridControl CreateEditTab(string tabName)
        {
            GridControl gc = new GridControl();
            GridView gv = new GridView(gc);
            DevExpress.XtraTab.XtraTabPage xtp = new DevExpress.XtraTab.XtraTabPage();

            //GridControl
            gc.Location = new System.Drawing.Point(15, 19);
            gc.MainView = gv;
            gc.LookAndFeel.SkinName = "Dark Side";
            gc.LookAndFeel.UseDefaultLookAndFeel = false;
            gc.Name = "grlControlRecordEdit"+tabName;
            gc.Size = new System.Drawing.Size(338, 86);
            gc.TabIndex = 0;
            //this.grlControlRecordEdit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            //this.gvRecordEdit});
            gc.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.grlRecordEdit_ProcessGridKey);

            //Gridview
            gv.Appearance.Empty.BackColor = System.Drawing.Color.White;
            gv.Appearance.Empty.Options.UseBackColor = true;
            gv.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            gv.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            gv.Appearance.FocusedCell.Options.UseBackColor = true;
            gv.Appearance.FocusedCell.Options.UseForeColor = true;
            gv.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            gv.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            gv.Appearance.FocusedRow.Options.UseBackColor = true;
            gv.Appearance.FocusedRow.Options.UseForeColor = true;
            gv.Appearance.FooterPanel.BackColor = System.Drawing.Color.White;
            gv.Appearance.FooterPanel.Options.UseBackColor = true;
            gv.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            gv.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.White;
            gv.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            gv.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gv.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            gv.Appearance.HeaderPanel.Options.UseBackColor = true;
            gv.Appearance.HeaderPanel.Options.UseBorderColor = true;
            gv.Appearance.HeaderPanel.Options.UseFont = true;
            gv.Appearance.HeaderPanel.Options.UseForeColor = true;
            gv.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            gv.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            gv.Appearance.HideSelectionRow.Options.UseBackColor = true;
            gv.Appearance.HideSelectionRow.Options.UseForeColor = true;
            gv.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            gv.Appearance.HorzLine.Options.UseBackColor = true;
            gv.Appearance.Row.BackColor = System.Drawing.Color.White;
            gv.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            gv.Appearance.Row.Options.UseBackColor = true;
            gv.Appearance.Row.Options.UseForeColor = true;
            gv.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            gv.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            gv.Appearance.SelectedRow.Options.UseBackColor = true;
            gv.Appearance.SelectedRow.Options.UseForeColor = true;
            gv.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            gv.Appearance.VertLine.Options.UseBackColor = true;
            gv.HorzScrollStep = 1;
            gv.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            gv.Name = "gvRecordEdit"+ tabName;
            gv.OptionsCustomization.AllowFilter = false;
            gv.OptionsDetail.AllowZoomDetail = false;
            gv.OptionsMenu.EnableColumnMenu = false;
            gv.OptionsMenu.EnableFooterMenu = false;
            gv.OptionsMenu.EnableGroupPanelMenu = false;
            gv.OptionsView.ColumnAutoWidth = false;
            gv.OptionsView.ShowGroupPanel = false;
            gv.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecordEdit_CustomRowCellEdit);
            gv.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecordEdit_CustomRowCellEditForEditing);
            gv.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gvRecordEdit_MouseWheel);
            gv.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gvRecordEdit_ValidatingEditor);            // 
            // Tab Page
            // 
            xtp.AutoScroll = true;
            xtp.Controls.Add(gc);
            xtp.Name = "tpRecordEdit"+tabName;
            xtp.Size = new System.Drawing.Size(378, 126);
            xtp.Text = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString() + "_tab_" + tabName);

            this.tabRecordEdit.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            xtp});

            return gc;
        }

        /// <summary>
        /// trae una fila de la grilla de edición
        /// </summary>
        /// <param name="rowIndex">numero de la fila en la grilla de edicion</param>
        /// <returns>la fila si existe, null si no existe</returns>
        internal GridProperty GetEditRow(int rowIndex)
        {
            try
            {
                List<GridProperty> list = new List<GridProperty>((List<GridProperty>)this.gvRecordEdit.DataSource);
                FieldConfiguration config = this.fields[rowIndex];
                List<GridProperty> gps = list.Where(x => x.Campo.Equals(config.Caption)).ToList();
                if (gps.Count == 1)
                {
                    return gps.First();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "GetEditRow"));
                return null;
            }
        }

         #endregion

        #region Funciones abstractas

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected abstract void InitForm();

        /// <summary>
        /// Cuenta los elementos dado un filtro
        /// </summary>
        /// <returns></returns>
        protected abstract long CountElements(bool useFastFilter=true);

        /// <summary>
        /// Trae los datos
        /// </summary>
        /// <param name="pageSize">Tamaño de la página</param>
        /// <param name="pageNum">Número de página</param>
        /// <returns></returns>
        protected abstract IEnumerable GetPagedData(int pageNum);

        /// <summary>
        /// Actualiza los campos de la grilla de edición
        /// </summary>
        /// <param name="isNew">Indica si se va agregar un nuevo registro</param>
        /// <param name="rowIndex">Indice de la fila</param>
        protected abstract void LoadEditGridData(bool isNew, int rowIndex);

        /// <summary>
        /// Valida los campos de la grilla de edicion y  llena el dto
        /// SI ocurren problemas de validación devuelve el dto en null
        /// </summary>
        /// <returns>El DTO de Grupo lleno si la validación pasó y el DTO nulo si no pasó</returns>
        protected abstract object ValidateEditGrid();

        /// <summary>
        /// Función que trae los registros para el reporte
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable GetReportData();

        #endregion

        #region Funciones virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// <param name="isH">Defini si es jerarquica</param>
        /// </summary>
        protected virtual void SetInitParameters(bool isH)
        {
            this.FrmProperties = _bc.AdministrationModel.MasterProperties[this.DocumentID];

            //Propiedades de la maestra
            this.colIdName = this.FrmProperties.ColumnaID;
            this.frmModule = this.FrmProperties.ModuloID;
            this.frmType = Type.GetType("NewAge.DTO.Negocio." + this.FrmProperties.DTOTipo + ", NewAge.DTO");
            this.sortField = this.FrmProperties.ColumnaID;

            #region Formato de exportar/importar
            List<string> cols = new List<string>();
            cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"));
            cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.FrmProperties.ColumnaID));
            if (this.DocumentID != (int)AppMasters.coTercero)
            {
                cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo"));
            }
            foreach (var ef in this.FrmProperties.Campos)
            {
                if (ef.ImportacionInd)
                {
                    cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + ef.NombreColumna));
                }
            }
            if (isH)
            {
                cols.Add(_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_MovInd"));
            }
            this.format = TableFormat.FillMasterSimple(cols);
            #endregion
            //Inicia las variables del formulario
            this.empresaGrupoID = _bc.GetMaestraEmpresaGrupoByDocumentID(this.DocumentID);
            basicUDT = new UDT_BasicID();
            basicUDT.MaxLength = FrmProperties.IDLongitudMax;

            //this.editrow_act = 0;
            this.editrow_code = 0;
            this.editrow_desc = 1;

            this.CreateFieldsConfig(isH);
        }

        /// <summary>
        /// Crea las configuraciones de los campos
        /// <param name="isH">Defini si es jerarquica</param>
        /// </summary>
        protected virtual void CreateFieldsConfig(bool isH)
        {
            fields = new Dictionary<int, FieldConfiguration>();

            TextFieldConfiguration codeConfig = new TextFieldConfiguration(typeof(string), this.basicUDT.MaxLength, CharacterCasing.Normal, TextFieldType.Letters, true);
            codeConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.FrmProperties.ColumnaID);
            codeConfig.FieldName = this.FrmProperties.ColumnaID;
            codeConfig.GridVisible = true;
            codeConfig.EditVisible = true;
            codeConfig.ColumnWidth = 100;
            codeConfig.ColumnIndex = this.editrow_code;
            if (this.FrmProperties.ColumnaID == "DocumentoID" || this.FrmProperties.ColumnaID == "TerceroID" || this.FrmProperties.ColumnaID == "acComponenteID")
            {
                codeConfig.Regex = UDT.GetRegex(typeof(UDTSQL_int));
            }
            else
            {
                codeConfig.Regex = UDT.GetRegex(typeof(UDT_BasicID));
            }

            codeConfig.Editable = false;
            codeConfig.GridVisible = true;
            codeConfig.Tab = basicTab;
            codeConfig.Fixed = true;
            codeConfig.Casing = CharacterCasing.Upper;
            this.fields.Add(this.editrow_code, codeConfig);

            TextFieldConfiguration descConfig = new TextFieldConfiguration(typeof(string), UDT_Descriptivo.MaxLength, CharacterCasing.Normal, TextFieldType.Everything, true);
            descConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo");
            descConfig.FieldName = "Descriptivo";
            descConfig.GridVisible = true;
            descConfig.EditVisible = true;
            descConfig.ColumnWidth = 275;
            descConfig.ColumnIndex = this.editrow_desc;
            descConfig.Tab = basicTab;
            descConfig.Fixed = true;
            descConfig.Regex = UDT.GetRegex(typeof(UDT_DescripTBase));
            this.fields.Add(this.editrow_desc, descConfig);

            int extraF = 1;
            this.FrmProperties.Campos.ForEach(f =>
            {
                extraF = extraF + 1;
                this.AddFormField(ref extraF, f);
            });

            if (isH)
            {
                this.editrow_mov = extraF + 1;
                extraF++;
            }

            this.editrow_act = extraF + 1;
            this.editrow_vers = extraF + 2;
            this.editrow_replica = extraF + 3;
            this.editrow_company = extraF + 4;

            CheckFieldConfiguration activoConfig = new CheckFieldConfiguration();
            activoConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd");
            activoConfig.FieldName = "ActivoInd";
            activoConfig.GridVisible = true;
            activoConfig.EditVisible = true;
            activoConfig.ColumnWidth = 40;
            activoConfig.ColumnIndex = this.editrow_act;
            activoConfig.Tab = basicTab;
            activoConfig.Fixed = true;
            this.fields.Add(this.editrow_act, activoConfig); 
            
            if (isH)
            {
                CheckFieldConfiguration movConfig = new CheckFieldConfiguration();
                movConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_MovInd");
                movConfig.FieldName = "MovInd";
                movConfig.ColumnWidth = 40;
                movConfig.ColumnIndex = this.editrow_mov;
                movConfig.Editable = false;
                movConfig.Tab = basicTab;
                this.fields.Add(this.editrow_mov, movConfig); 
            }

            TextFieldConfiguration versionConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
            versionConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_CtrlVersion");
            versionConfig.FieldName = "CtrlVersion";
            versionConfig.GridVisible = false;
            versionConfig.EditVisible = false;
            versionConfig.ColumnWidth = 0;
            versionConfig.ColumnIndex = this.editrow_vers;
            versionConfig.Tab = basicTab;
            this.fields.Add(this.editrow_vers, versionConfig);

            TextFieldConfiguration replicaConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
            replicaConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ReplicaID");
            replicaConfig.FieldName = "ReplicaID";
            replicaConfig.GridVisible = false;
            replicaConfig.EditVisible = false;
            replicaConfig.ColumnWidth = 0;
            replicaConfig.ColumnIndex = this.editrow_replica;
            replicaConfig.Tab = basicTab;
            this.fields.Add(this.editrow_replica, replicaConfig);

            //Verifica la columna de la empresa
            if (this.FrmProperties.GrupoEmpresaInd)
            {
                TextFieldConfiguration companyConfig = new TextFieldConfiguration(typeof(int), 10, CharacterCasing.Normal, TextFieldType.Numbers, false);
                companyConfig.Caption = _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_EmpresaID");
                companyConfig.FieldName = "EmpresaGrupoID";
                companyConfig.GridVisible = false;
                companyConfig.EditVisible = false;
                companyConfig.ColumnWidth = 0;
                companyConfig.ColumnIndex = this.editrow_company;
                companyConfig.Tab = basicTab;
                this.fields.Add(this.editrow_company, companyConfig);
            }
            this.RefreshRowIndexFields();
            this.CustomizeFieldsConfig();
        }

        /// <summary>
        /// Agrega un campo a la ista de campos
        /// </summary>
        /// <param name="campo">Campo que se va agregar a la tabla</param>
        protected virtual void AddFormField(ref int index, DTO_aplMaestraCampo campo)
        {
            try
            {
                bool rankRegex = true;
                string[] arraux = new string[0];
                if (!string.IsNullOrWhiteSpace(campo.RegExpression) && campo.RegExpression.Split("|".ToArray()).Count() > 1)
                {
                    arraux = campo.RegExpression.Split("|".ToArray());
                    foreach (string part in arraux)
                    {
                        try
                        {
                            if(this.DocumentID != AppMasters.ccClasificacionxRiesgo)
                                Convert.ToInt32(part);
                        }
                        catch (Exception numberExc)
                        {
                            MessageBox.Show(_bc.GetResourceForException(numberExc, "WinApp-MasterForm.cs", "AddFormField"));
                            rankRegex = false;
                        }
                    }
                }
                else
                {
                    rankRegex = false;
                }
                if (campo.FKInd)
                {
                    //Definición de la FK
                    ForeignKeyFieldConfig fk = new ForeignKeyFieldConfig()
                    {
                        CountMethod = "MasterSimple_Count",
                        DataMethod = "MasterSimple_GetPaged",
                        DataRowMethod = "MasterSimple_GetByID",
                        DescField = campo.FKColumnaDesc,
                        KeyField = campo.FKColumnaID,
                        ModalFormCode = campo.FKDocumentoID.ToString()
                    };

                    fk.TableName = campo.FKNombreTabla;

                    //Asignacion del campo
                    ButtonEditFKConfiguration beFk = new ButtonEditFKConfiguration(typeof(string), _bc.AdministrationModel.MasterProperties[campo.FKDocumentoID].IDLongitudMax, CharacterCasing.Normal, TextFieldType.Letters, campo.EditableInd, fk);
                    beFk.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    beFk.FieldName = campo.NombreColumna;
                    beFk.GridVisible = campo.GrillaInd;
                    beFk.EditVisible = campo.GrillaEdicionInd;
                    beFk.ColumnWidth = campo.ColumnaTamano;
                    beFk.ColumnIndex = campo.ColumnaPosicion;
                    beFk.AllowNull = campo.VacioInd;
                    beFk.Casing = CharacterCasing.Upper;
                    beFk.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    if (campo.NombreColumna == "DocumentoID")
                    {
                        beFk.Regex = UDT.GetRegex(typeof(UDTSQL_int));
                    }
                    else
                    {
                        beFk.Regex = UDT.GetRegex(Type.GetType("NewAge.DTO.UDT." + campo.Tipo + ", NewAge.DTO"));
                    }
                    this.fields.Add(index, beFk);
                }
                else if (campo.Tipo == "UDT_SiNo")
                {
                    CheckFieldConfiguration chkConfig = new CheckFieldConfiguration();
                    chkConfig.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    chkConfig.FieldName = campo.NombreColumna;
                    chkConfig.GridVisible = campo.GrillaInd;
                    chkConfig.EditVisible = campo.GrillaEdicionInd;
                    chkConfig.ColumnWidth = campo.ColumnaTamano;
                    chkConfig.ColumnIndex = campo.ColumnaPosicion;
                    chkConfig.Editable = campo.EditableInd;
                    chkConfig.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    this.fields.Add(index, chkConfig);
                }
                else if (campo.Tipo == "UDT_Imagen")
                {
                    ImageFieldConfiguration imgConfig = new ImageFieldConfiguration();
                    imgConfig.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    imgConfig.FieldName = campo.NombreColumna;
                    imgConfig.GridVisible = campo.GrillaInd;
                    imgConfig.EditVisible = campo.GrillaEdicionInd;
                    imgConfig.ColumnWidth = campo.ColumnaTamano;
                    imgConfig.ColumnIndex = campo.ColumnaPosicion;
                    imgConfig.Editable = campo.EditableInd;
                    imgConfig.AllowNull = campo.VacioInd;
                    imgConfig.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    this.fields.Add(index, imgConfig);
                }
                else if (rankRegex)
                {
                    IntRankConfiguration rnkConfig = new IntRankConfiguration(arraux);
                    rnkConfig.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    rnkConfig.FieldName = campo.NombreColumna;
                    rnkConfig.GridVisible = campo.GrillaInd;
                    rnkConfig.EditVisible = campo.GrillaEdicionInd;
                    rnkConfig.ColumnWidth = campo.ColumnaTamano;
                    rnkConfig.ColumnIndex = campo.ColumnaPosicion;
                    rnkConfig.AllowNull = campo.VacioInd;
                    rnkConfig.Regex = campo.RegExpression;
                    rnkConfig.Editable = campo.EditableInd;
                    rnkConfig.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    this.fields.Add(index, rnkConfig);
                    if (!combosData.ContainsKey(campo.NombreColumna))
                    {
                        Dictionary<string, string> descs = new Dictionary<string, string>();
                        foreach (string op in arraux)
                        {
                            string desc = _bc.GetResource(LanguageTypes.Tables, "tbl_" + campo.TablaDesc + "_v" + op);
                            descs.Add(op, desc);
                        }
                        combosData.Add(campo.NombreColumna, descs);
                    }

                }
                else if (campo.Tipo == "UDTSQL_varcharMAX")
                {
                    RichTextFieldConfiguration rtf = new RichTextFieldConfiguration();
                    rtf.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    rtf.FieldName = campo.NombreColumna;
                    rtf.GridVisible = campo.GrillaInd;
                    rtf.EditVisible = campo.GrillaEdicionInd;
                    rtf.ColumnWidth = campo.ColumnaTamano;
                    rtf.ColumnIndex = campo.ColumnaPosicion;
                    rtf.AllowNull = campo.VacioInd;
                    rtf.Editable = campo.EditableInd;
                    rtf.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    this.fields.Add(index, rtf);
                }
                else
                {
                    TextFieldConfiguration fConfig = new TextFieldConfiguration(typeof(string), campo.LongitudMax, CharacterCasing.Normal, TextFieldType.Everything, campo.EditableInd);
                    switch (campo.Tipo)
                    {
                        case "UDTSQL_datetime":
                            fConfig = new TextFieldConfiguration(typeof(DateTime), 1, CharacterCasing.Normal, TextFieldType.Everything, campo.EditableInd);
                            break;
                        case "UDT_PeriodoID":
                            fConfig = new TextFieldConfiguration(typeof(DateTime), 1, CharacterCasing.Normal, TextFieldType.Everything, campo.EditableInd);
                            break;
                        case "UDTSQL_smalldatetime":
                            fConfig = new TextFieldConfiguration(typeof(DateTime), 1, CharacterCasing.Normal, TextFieldType.Everything, campo.EditableInd);
                            break;
                        case "UDTSQL_int":
                            fConfig = new TextFieldConfiguration(typeof(int), 50, CharacterCasing.Normal, TextFieldType.Numbers, campo.EditableInd);
                            break;
                    }

                    fConfig.Caption = _bc.GetResource(LanguageTypes.Forms, campo.DocumentoID.ToString() + "_" + campo.NombreColumna);
                    fConfig.FieldName = campo.NombreColumna;
                    fConfig.GridVisible = campo.GrillaInd;
                    fConfig.EditVisible = campo.GrillaEdicionInd;
                    fConfig.ColumnWidth = campo.ColumnaTamano;
                    fConfig.ColumnIndex = campo.ColumnaPosicion;
                    fConfig.Tab = (string.IsNullOrWhiteSpace(campo.Tab)) ? basicTab : campo.Tab;
                    if (string.IsNullOrWhiteSpace(campo.RegExpression))
                    {
                        fConfig.Regex = UDT.GetRegex(Type.GetType("NewAge.DTO.UDT." + campo.Tipo + ", NewAge.DTO"));
                    }
                    else
                    {
                        fConfig.Regex = campo.RegExpression;
                    }
                    fConfig.AllowNull = campo.VacioInd;
                    this.fields.Add(index, fConfig);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "AddFormField"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            try
            {
                foreach (FieldConfiguration field in this.fields.Values)
                {
                    GridColumn code = new GridColumn();
                    code.FieldName = this.unboundPrefix + field.FieldName;
                    code.Caption = field.Caption;
                    code.UnboundType = this.GetColumnType(field.ValueType);
                    code.VisibleIndex = field.ColumnIndex;
                    code.Width = field.ColumnWidth;
                    code.Visible = field.GridVisible;
                    if (field.Fixed)
                        code.Fixed = FixedStyle.Left;

                    if (field.FieldName == "Monto" || field.FieldName == "Valor" || field.FieldName == "CostoLocalEMP" || field.FieldName == "CostoLocalEXT")
                    {
                        code.ColumnEdit = this.editSpin;
                    }
                    else if (field.FieldName == "PorcentajeID")
                    {
                        code.ColumnEdit = this.editPorc;
                    }

                    this.gvModule.Columns.Add(code);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Sobrecargar para modificar alguna configuración de campo de la maestra
        /// </summary>
        public virtual void CustomizeFieldsConfig()
        {
            ;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() 
        {
            string idColName = this.FrmProperties.ColumnaID;//_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.FrmProperties.ColumnaID);
            string descColName = "Descriptivo";//_bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo"); ;

            this.gvModule.OptionsFind.FindFilterColumns = "*";// idColName + ";" + descColName;
        }

        /// <summary>
        /// Permite a los hijos procesar la data cuando se está cargando
        /// </summary>
        /// <param name="data">data traida del servidor</param>
        protected virtual void ProcessDataLoading(IEnumerable data){ }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="firstPage">Si debe ir a la primera página</param>
        /// <param name="lastPage">Si debe ir a la ultima página</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected virtual void LoadGridData(bool firstTime, bool firstPage, bool lastPage, bool isUpdate=false)
        {
            try
            {
                //if (!firstTime)
                //    this.gvModule.MoveFirst();

                long count = this.CountElements();
                bool hasItems = count > 0 ? true : false;
               
                this.pgGrid.UpdatePageNumber(count, firstTime || isUpdate, firstPage, lastPage);

                this.data = this.GetPagedData(this.pgGrid.PageNumber);
                IEnumerable gridData = this.data;

                this.gvModule.OptionsBehavior.AutoPopulateColumns = false;
                try
                {
                    List<object> tmp = new List<object>((IEnumerable<object>)gridData);
                    if (tmp.Count > 0)
                        this._currentRow = 0;

                    this.grlcontrolModule.DataSource = tmp;
                    //this.grlcontrolModule.RefreshDataSource();
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex1, "WinApp-MasterForm.cs", "LoadGridData"));
                }

                this.ProcessDataLoading(gridData);

                if (firstTime)
                    this.AddGridCols();

                this.LoadEditGridData(false, 0);

                if (firstTime)
                {
                    if (hasItems)
                        this.gvModule.MoveFirst();
                    else
                        this.TBNew();
                }
                else if (!hasItems)
                {
                    this.TBNew();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "LoadGridData"));
            }
        }

        /// <summary>
        /// Valida el codigo de un formulario jerarquico
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual bool ValidateCode(string code)
        {
            var regexItem = new Regex(_bc.RegExpNoSymbols);
            return regexItem.IsMatch(code);
        }

        /// <summary>
        /// Sobrecargar para procesar el cambio en un campo especifico de la grilla de edición
        /// </summary>
        /// <param name="Field">Nombre del campo en la grilla de edición</param>
        /// <param name="Value">Valor ingrtesado</param>
        /// <param name="RowIndex">Numero de la fila en la grilla de edición</param>
        protected virtual bool FieldValidate(string Field, object Value, int RowIndex, out string msg)
        {
            msg = string.Empty; 
            return true;
        }

        /// <summary>
        /// Valida restricciones MUY particulares 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected virtual DTO_TxResult ValidateDataOnImport(List<DTO_MasterBasic> list)
        {
            return new DTO_TxResult(){ Result = ResultValue.OK};
        }

        /// <summary>
        /// Valida las reglas de =negocio de un DTO
        /// </summary>
        /// <param name="dtoObj">Objeto que se envia</param>
        /// <param name="res">Respuesta si esta bien el DTO</param>
        /// <param name="msg">Mensaje de error (vacio si esta todo ok)</param>
        /// <returns>Retorna un diccionario con la lista de campos y sus errores</returns>
        protected virtual SortedDictionary<string, string> ValidateDTORules(Object dtoObj, out bool res, out string msg)
        {
            msg = string.Empty;
            res = true;
            SortedDictionary<string, string> ret = DTO_Validations.CheckRules(dtoObj, out res, out msg, this.table, this.Insertando);
            return ret;
        }

        /// <summary>
        /// Carga los datos iniciales de una maestra
        /// </summary>
        /// <param name="fieldName">Nombre del campo</param>
        /// <returns>Retorna el valor que debe ir en el campo</returns>
        protected virtual string NewRecordLoadData(string fieldName)
        {
            return  string.Empty;
        }

        /// <summary>
        /// Completa la información de un dto antes de cargar la grilla de edición. P.E. Imágenes o BLOBs
        /// </summary>
        /// <param name="dto">dto sin los datos llenos</param>
        public virtual Object FillEditData(Object dto)
        {
            return dto;
        }

        #endregion

        #region Funciones publicas

        /// <summary>
        /// Organiza las grillas de edición
        /// </summary>
        /// <param name="datos"></param>
        public void ConfigureEditGrids(List<GridProperty> datos)
        {
            //Llena el tab de todos los que los tengan vacio por proteccion
            foreach (GridProperty gp in datos)
            {
                if (string.IsNullOrWhiteSpace(gp.Tab))
                    gp.Tab = basicTab;
            }

            this.tabRecordEdit.Height = 248;

            //Personaliza la primera columna 
            this.grlControlRecordEdit.DataSource = datos;
            //this.grlControlRecordEdit.RefreshDataSource();
            this.grlControlRecordEdit.Height = 180;
            this.gvRecordEdit.Columns["Visible"].Visible = false;
            this.gvRecordEdit.Columns["Tab"].Visible = false;
            this.gvRecordEdit.ActiveFilterString = "[Visible] = true AND [Tab] = '" + basicTab + "'";
            this.gvRecordEdit.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvRecordEdit.OptionsCustomization.AllowSort = false;
            this.gvRecordEdit.OptionsCustomization.AllowRowSizing = false;
            this.gvRecordEdit.Columns["Campo"].Width = 115;
            this.gvRecordEdit.Columns["Campo"].OptionsColumn.AllowEdit = false;
            this.gvRecordEdit.Columns["Campo"].OptionsColumn.AllowFocus = false;
            this.gvRecordEdit.Columns["Campo"].AppearanceCell.BackColor = Color.GhostWhite;
            this.gvRecordEdit.Columns["Valor"].Width = 187;
            this.gvRecordEdit.Columns["Valor"].OptionsColumn.AllowEdit = (SecurityManager.HasAccess(this.DocumentID, FormsActions.Edit) || this.Insertando);

            this.tpRecordEdit.Text = _bc.GetResource(LanguageTypes.Forms, AppForms.MasterForm + "_tpGeneral");

            List<FieldConfiguration> extraf = this.fields.Values.Where(x => !x.Tab.Equals(basicTab)).ToList();
            foreach (FieldConfiguration fc in extraf)
            {
                if (!extraGrids.ContainsKey(fc.Tab))
                    this.extraGrids.Add(fc.Tab, this.CreateEditTab(fc.Tab));
            }

            foreach (string tab in extraGrids.Keys)
            {
                GridControl gc = extraGrids[tab];
                gc.DataSource = datos;
                //gc.RefreshDataSource();
                gc.Height = 180;
                GridView gv = (GridView)gc.FocusedView;

                gv.Columns["Visible"].Visible = false;
                gv.Columns["Tab"].Visible = false;
                //gv.ActiveFilterString = EditGridFilterString;
                gv.ActiveFilterString = "[Visible] = true AND [Tab] = '" + tab + "'";
                gv.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
                gv.OptionsCustomization.AllowSort = false;
                gv.OptionsCustomization.AllowRowSizing = false;
                gv.Columns["Campo"].Width = 115;
                gv.Columns["Campo"].OptionsColumn.AllowEdit = false;
                gv.Columns["Campo"].OptionsColumn.AllowFocus = false;
                gv.Columns["Campo"].AppearanceCell.BackColor = Color.GhostWhite;
                gv.Columns["Valor"].Width = 187;
                gv.Columns["Valor"].OptionsColumn.AllowEdit = (SecurityManager.HasAccess(this.DocumentID, FormsActions.Edit) || this.Insertando);
            }

        }

        /// <summary>
        /// Pone en la grilla correspondiente el valor indicado dado el indice del campo
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="value"></param>
        public void SetEditGridValue(int rowIndex, object value)
        {
            List<GridProperty> list = new List<GridProperty>((List<GridProperty>)this.gvRecordEdit.DataSource);
            FieldConfiguration config = this.fields[rowIndex];
            List<GridProperty> gps = list.Where(x => x.Campo.Equals(config.Caption)).ToList();
            if (gps.Count == 1)
            {
                GridProperty gp = gps.First();
                if (config is TextFieldConfiguration && (config as TextFieldConfiguration).Casing == CharacterCasing.Upper)
                {
                    gp.Valor = value.ToString().ToUpper();
                }
                else
                {
                    gp.Valor = value;
                }
                //Actualizar grillas
                this.grlControlRecordEdit.DataSource = list;
                foreach (GridControl gc in this.extraGrids.Values)
                {
                    gc.DataSource = list;
                    //gc.RefreshDataSource();
                }
            }
        }

        #region Funciones Consultas

        /// <summary>
        /// Asigna la consulta seleccionada con sus valores
        /// </summary>
        /// <param name="dto">dto</param>
        /// <param name="property">nombre del campo</param>
        /// <returns>el valor pasado a string o nullo si la propiedad no existe</returns>
        public void SetConsulta(DTO_glConsulta consulta, List<ConsultasFields> fields)
        {
            this.consulta = consulta;
            this.ConsultaFiels = fields;
            foreach (FieldConfiguration fc in this.fields.Values)
            {
                List<DTO_glConsultaSeleccion> search = consulta.Selecciones.Where(x => x.CampoFisico == fc.FieldName).ToList();
                if (search.Count() > 0)
                {
                    search.First().Idx = fc.ColumnIndex;
                }
            }
            List<DTO_glConsultaSeleccion> temp = consulta.Selecciones.OrderByDescending(x => x.Idx).ToList();
            foreach (GridColumn col in this.gvModule.Columns)
            {
                col.Visible = false;
            }
            foreach (DTO_glConsultaSeleccion sel in temp)
            {
                string campoSel = unboundPrefix + sel.CampoFisico;
                if (campoSel.Equals(unboundPrefix + "ID"))
                {
                    if (this.table.Jerarquica.Value.HasValue && (this.table.Jerarquica.Value.Value != true))
                    {
                        campoSel = unboundPrefix + this.FrmProperties.ColumnaID;
                    }
                    else
                    {
                        continue;
                    }
                }
                GridColumn columna = this.gvModule.Columns.ColumnByFieldName(campoSel);
                if (columna != null)
                {
                    columna.Visible = true;
                    columna.VisibleIndex = 0;
                }
            }

            string campoSelID = unboundPrefix + this.FrmProperties.ColumnaID;

            GridColumn columnaID = this.gvModule.Columns.ColumnByFieldName(campoSelID);
            if (columnaID != null)
            {
                columnaID.Visible = true;
                columnaID.VisibleIndex = 0;
            }

            this._insertando = false;
            this.pnSearch.Visible = false;
            this.txtCode.Text = string.Empty;
            this.txtDescrip.Text = string.Empty;
            this.filtrosConsulta = null;
            this.LoadGridData(false, true, false);
        }

        #endregion

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
                FormProvider.Master.Form_Enter(this, this.DocumentID, this._frmType, this.frmModule);
                if (this.hasEditAccess != null && !Insertando && FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = this.hasEditAccess.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "Form_Leave"));
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
                if (this.importando)
                    e.Cancel = true;
                else
                    FormProvider.Master.Form_Closing(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Paginación

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        protected virtual void pagging_Click(object sender, System.EventArgs e)
        {
            this.LoadGridData(false, false, false);
            this._insertando = false;
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvModule_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RowSelected(e.FocusedRowHandle);
            //bool valid = this.ValidateOverwrite(e.FocusedRowHandle);
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvModule_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            bool valid = this.ValidateOverwrite(e.RowHandle);
            if (!valid)
                e.Allow = false;
        }
        
        /// <summary>
        /// Evento que se ejecuta al hacer click sobre una fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvModule_RowClick(object sender, RowClickEventArgs e)
        {
            bool valid = this.ValidateOverwrite(e.RowHandle);
            if (!valid)
                e.Handled = true;
        }

        /// <summary>
        /// Crea las propiedades de la fila seleccionada
        /// </summary>
        /// <param name="row">numero de fila que esta siendo usada</param>
        protected virtual void RowSelected(int row)
        {
            try
            {
                this._currentRow = row;

                this.Insertando = false;
                this.selectedDto = (DTO_MasterBasic)this.gvModule.GetRow(row);
                this.LoadEditGridData(false, row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "RowSelected"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvModule_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e) { }

        /// <summary>
        /// Busca data en la grilla segun la información filtrada en las cajas de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ValidateSearchData();
        }

        /// <summary>
        /// Busca data en la grilla segun la información filtrada en las cajas de texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtSearch_Click(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
                this.ValidateSearchData();
        }

        #endregion

        #region Eventos grilla edición

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(ButtonEdit be)
        {
            this.IsModalFormOpened = true;
            try
            {
                FieldConfiguration fc = this.GetFocusedFieldConfig();
                if (fc != null)
                {
                    if (fc is ButtonEditFKConfiguration)
                    {
                        ButtonEditFKConfiguration bec = (ButtonEditFKConfiguration)fc;
                        ForeignKeyFieldConfig config = bec.FkConfig;

                        string modFrmCode = config.ModalFormCode;
                        string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                        Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                        DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                        if (fktable.Jerarquica.Value.Value)
                        {
                            if (config.DescField == "CuentaAlternaDesc")
                            {
                                config.CountMethod = "coPlanCuenta_Count";
                                config.DataMethod = "coPlanCuenta_GetPaged";
                            }

                            ModalMaster modal = new ModalMaster(be, modFrmCode, config.CountMethod, config.DataMethod, config.args, config.KeyField, config.DescField, true, bec.Filtros);
                            modal.ShowDialog();
                        }
                        else
                        {
                            ModalMaster modal = new ModalMaster(be, modFrmCode, config.CountMethod, config.DataMethod, config.args, config.KeyField, config.DescField, false, bec.Filtros);
                            modal.ShowDialog();
                        }
                    }
                }
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Coloca en la celda booleana el editor de celda(check) requerido
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvRecordEdit_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                //GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
                FieldConfiguration config = new FieldConfiguration();
                this.fields.TryGetValue(e.RowHandle, out config);

                if (e.Column.FieldName.Equals("Valor"))
                {
                    if ((e.CellValue.Equals(true.ToString()) || e.CellValue.Equals(false.ToString())))
                    {
                        //Asigna una caja de chekeo a las celdas que contengan True o False 
                        chkRecordEdit.ReadOnly = false;
                        e.RepositoryItem = this.chkRecordEdit;
                        chkRecordEdit.ValueChecked = true.ToString();
                        chkRecordEdit.ValueUnchecked = false.ToString();
                    }
                    else if (this._assignDate)
                    {
                        this._assignDate = false;
                        GridProperty gp = this.GetEditRow(this._rowTemp);

                        DateTime d;
                        DateTime.TryParse(gp.Valor.ToString(), out d);
                        string strTemp = string.Empty; ;
                        if (d != null && d != DateTime.MinValue)
                        {
                            if (this._isPeriod)
                            {
                                DateTime tempDate = new DateTime(d.Year, d.Month, 1);
                                strTemp = tempDate.ToString(FormatString.Date);//.Period);
                            }
                            else
                                strTemp = d.ToString(FormatString.Date);
                        }
                        gp.Valor = strTemp;
                    }
                    else if (config.FieldName.StartsWith("Monto") || config.FieldName.StartsWith("Valor") || config.FieldName.StartsWith("Sueldo") ||
                             config.FieldName.StartsWith("CostoLocalEMP") || config.FieldName.StartsWith("CostoLocalEXT"))
                    {
                        e.RepositoryItem = this.editSpin;
                    }
                    else if (config.FieldName.StartsWith("PorcentajeID"))
                    {
                        e.RepositoryItem = this.editPorc;
                    }
                    else if (config is RichTextFieldConfiguration)
                    {
                        this.richTextEdit.ReadOnly = false;
                        e.RepositoryItem = richTextEdit;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "gvRecordEdit_CustomRowCellEdit"));
            }
        }

        /// <summary>
        /// Asigna un editor de celda(button, check, textbox..) a la celda relacionada del index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                this.viewBeingEdited = null;
                this.viewBeingEdited = this.GetFocusedGridView();

                //FieldConfiguration config = new FieldConfiguration();
                //this.fields.TryGetValue(e.RowHandle, out config);

                GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
                FieldConfiguration config = this.GetFieldConfigByCaption(gpHandle.Campo);
               
                if (config != null)
                {
                    #region Asigna el repository item de edicion 
                    bool restrictedByHierarchy = false;
                    if (this.table.Jerarquica.Value.Value)
                    {
                        //Validar de acuerdo al nivel del dto
                        int level = LevelNew;
                        DTO_MasterHierarchyBasic dto = (DTO_MasterHierarchyBasic)this.selectedDto;
                        if (dto != null)
                            level = dto.Jerarquia.NivelInstancia;
                        DTO_aplMaestraCampo campo = this.GetFieldByFieldName(config.FieldName);
                        if (campo != null && campo.NivelJerarquia != 0)
                        {
                            if (level != campo.NivelJerarquia)
                                restrictedByHierarchy = true;
                        }
                    }
                    if (config is TextFieldConfiguration)
                    {
                        TextFieldConfiguration tfc = (TextFieldConfiguration)config;
                        if (!tfc.Editable || restrictedByHierarchy)
                        {
                            this.txtRecordEdit.ReadOnly = true;
                            this.txtRecordEdit.Mask.MaskType = MaskType.RegEx;
                            this.txtRecordEdit.Mask.EditMask = tfc.Regex;
                            this.txtRecordEdit.MaxLength = tfc.MaxLength;
                            e.RepositoryItem = this.txtRecordEdit;
                        }
                        else
                        {
                            if (tfc.ValueType == typeof(DateTime))
                            {
                                this._rowTemp = tfc.RowIndex;//e.RowHandle;
                                if (tfc.FieldName == "PeriodoID" || tfc.FieldName == "Periodo")
                                {
                                    this._isPeriod = true;
                                    e.RepositoryItem = this.editFecha;// this.editPeriodo;
                                }
                                else
                                {
                                    this._isPeriod = false;
                                    e.RepositoryItem = this.editFecha;
                                }
                            }
                            else
                            {
                                this.txtRecorMax.Mask.MaskType = MaskType.RegEx;
                                this.txtRecorMax.Mask.EditMask = tfc.Regex;
                                this.txtRecorMax.MaxLength = tfc.MaxLength;
                                this.txtRecorMax.ReadOnly = false;
                                this.txtRecorMax.CharacterCasing = tfc.Casing;
                                e.RepositoryItem = this.txtRecorMax;
                            }
                        }
                    }
                    if (config is ButtonEditFKConfiguration)
                    {
                        ButtonEditFKConfiguration bec = (ButtonEditFKConfiguration)config;
                        ForeignKeyFieldConfig configFK = bec.FkConfig;
                        btnRecordEdit.Mask.MaskType = MaskType.RegEx;
                        btnRecordEdit.Mask.EditMask = bec.Regex;
                        btnRecordEdit.MaxLength = bec.MaxLength;
                        btnRecordEdit.CharacterCasing = bec.Casing;
                        btnRecordEdit.ReadOnly = (!bec.Editable || restrictedByHierarchy);
                        btnRecordEdit.Buttons[0].Enabled = (bec.Editable && !restrictedByHierarchy);
                        e.RepositoryItem = this.btnRecordEdit;
                    }
                    if (config is CheckFieldConfiguration)
                    {
                        CheckFieldConfiguration cfc = (CheckFieldConfiguration)config;
                        chkRecordEdit.ReadOnly = (!cfc.Editable || restrictedByHierarchy);
                        e.RepositoryItem = this.chkRecordEdit;
                    }
                    if (config is IntRankConfiguration)
                    {
                        IntRankConfiguration cfc = (IntRankConfiguration)config;
                        this.cmbRank.Items.Clear();
                        foreach (string opt in cfc.Options)
                        {
                            this.cmbRank.Items.Add(opt);
                        }
                        this.cmbRank.Mask.MaskType = MaskType.RegEx;
                        this.cmbRank.Mask.EditMask = cfc.Regex;
                        this.cmbRank.ReadOnly = (!cfc.Editable || restrictedByHierarchy);
                        e.RepositoryItem = this.cmbRank;
                    }
                    if (config is RichTextFieldConfiguration)
                    {
                        RichTextFieldConfiguration rtf = config as RichTextFieldConfiguration;
                        this.richEditControl.ReadOnly = (!rtf.Editable || restrictedByHierarchy);
                        e.RepositoryItem = this.riPopup;
                    }
                    if (config is ImageFieldConfiguration)
                    {
                        ImageFieldConfiguration ifc = config as ImageFieldConfiguration;
                        e.RepositoryItem = this.imgEdit;
                    }
                    #endregion
                }
                else
                {
                    txtRecorMax.MaxLength = 50;
                    txtRecorMax.ReadOnly = false;
                    e.RepositoryItem = this.txtRecorMax;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "grlRecordEdit_CustomRowCelEditForEditing"));
            }
        }

        /// <summary>
        /// Inactiva el movimiento de la rueda del mouse
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecordEdit_MouseWheel(object sender, MouseEventArgs e)
        {
            (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
        }

        /// <summary>
        /// Identifica la tecla presionada para cambiar el foco al control de jerarquia
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void grlRecordEdit_ProcessGridKey(object sender, KeyEventArgs e)
        {
            try
            {
                int lastRow = GetFocusedGridView().RowCount - 1;
                
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab || e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp)
                {
                    #region Siguiente fila
                    e.SuppressKeyPress = true;

                    if (this.GetFocusedGridView().FocusedRowHandle == (lastRow))
                    {
                        GetFocusedGridView().FocusedColumn = GetFocusedGridView().VisibleColumns[1];
                        GetFocusedGridView().FocusedRowHandle = 0;
                    }
                    else
                    {
                        try { GetFocusedGridView().FocusedRowHandle++; }  
                        catch (Exception ex) 
                        {
                            MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "grlRecordEdit_ProcessGridKey"));
                        }
                    }

                    return;
                    #endregion
                }
                if (e.KeyCode == Keys.Enter)
                {
                    #region Enter

                    //if (this.gvRecordEdit.ActiveEditor is ButtonEdit)
                    //{
                    //    ButtonEdit button = (ButtonEdit)this.gvRecordEdit.ActiveEditor;
                    //    button.PerformClick(button.Properties.Buttons[0]);
                    //}

                    return;
                    #endregion
                }
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.End)
                {
                    #region Atras
                    e.SuppressKeyPress = true;
                    if (GetFocusedGridView().FocusedRowHandle == lastRow)
                        GetFocusedGridView().FocusedRowHandle = 0;
                    if (GetFocusedGridView().FocusedRowHandle == 0)
                    {
                        GetFocusedGridView().FocusedColumn = GetFocusedGridView().VisibleColumns[1];
                        GetFocusedGridView().FocusedRowHandle = lastRow;
                    }

                    else
                    {
                        GetFocusedGridView().FocusedRowHandle++;
                    }
                    return;
                    #endregion
                }
                if (e.KeyCode == Keys.Up)
                {
                    #region Arriba
                    if (GetFocusedGridView().FocusedRowHandle == 0)
                        GetFocusedGridView().FocusedRowHandle = 0;
                    #endregion
                }
                if (e.KeyCode == Keys.Escape && gvModule.DataRowCount > 0 && Insertando)
                {
                    #region Salir
                    this.gvModule.Focus();
                    this.gvModule.FocusedRowHandle = 0;
                    #endregion
                }                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "grlRecordEdit_ProcessGridKey"));
            }
         }

        /// <summary>
        /// Evento que se genera al hacer click en un grupo 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>        
        protected virtual void btnRecordEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit origin = (ButtonEdit)sender;
            this.ShowFKModal(origin);
        }

        /// <summary>
        /// Valida la tecla presionada al entrar al control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void txtRecordEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetterOrDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Evento al cambiar el campo de pais en la grilla de edicion
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>   
        /// <param name="e">Argumentos del evento</param>
        protected virtual void btnRecordEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            ButtonEdit origin = (ButtonEdit)sender;
            FieldConfiguration fc = this.GetFocusedFieldConfig();
            if (fc!=null)
            {
                int row = this.GetFocusedGridView().FocusedRowHandle;
                if (fc is ButtonEditFKConfiguration)
                {
                    ButtonEditFKConfiguration bec = (ButtonEditFKConfiguration)fc;
                    ForeignKeyFieldConfig config = bec.FkConfig;
                    try
                    {
                        Type t = _bc.AdministrationModel.GetType();

                        //Llama el contador de registros
                        MethodInfo mGetId = t.GetMethod(config.DataRowMethod);
                        UDT_BasicID bid = new UDT_BasicID();
                        if (bec.FkConfig.KeyField == "DocumentoID")
                        {
                            bid = new UDT_BasicID(true);
                        } 

                        bid.Value = e.NewValue.ToString();

                        Object rowData = null;
                        if (config.DescField == "CuentaAlternaDesc")
                            rowData = _bc.AdministrationModel.coPlanCuenta_GetCuentaAlterna(Convert.ToInt32(config.ModalFormCode), bid, true);
                        else
                            rowData = _bc.AdministrationModel.MasterSimple_GetByID(Convert.ToInt32(config.ModalFormCode), bid, true);

                        if (rowData != null && rowData is DTO_MasterBasic)
                        {
                            bool hierarchyFather = false;
                            if (rowData is DTO_MasterHierarchyBasic)
                            {
                                if ((rowData as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                {
                                    hierarchyFather = true;
                                }
                            }

                            DTO_MasterBasic dtoMB = (DTO_MasterBasic)rowData;
                            if (dtoMB.ActivoInd.Value == true && !hierarchyFather)
                            {
                                string desc = string.Empty;
                                desc = ((DTO_MasterBasic)rowData).Descriptivo.Value;
                                this.GetFocusedGridView().SetRowCellValue(row + 1, this.GetFocusedGridView().Columns["Valor"], desc);
                            }
                            else
                            {
                                this.GetFocusedGridView().SetRowCellValue(row + 1, this.GetFocusedGridView().Columns["Valor"], string.Empty);
                            }
                        }
                        else
                        {
                            this.GetFocusedGridView().SetRowCellValue(row + 1, this.GetFocusedGridView().Columns["Valor"], string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        e.NewValue = string.Empty;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Evento para utilizar en el paste sobre un objeto
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>   
        /// <param name="e">Argumentos del evento</param>
        protected virtual void txtRecordEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            var regexItem = new Regex(_bc.RegExpNoSymbols);
            if (!regexItem.IsMatch(e.NewValue.ToString()))
                e.NewValue = e.OldValue;
        }

        /// <summary>
        /// Evento de validación de un editor de la grilla de edición
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>   
        /// <param name="e">Argumentos del evento</param>
        private void gvRecordEdit_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Valid)
            {
                try
                {
                    GridProperty gp = (GridProperty)this.GetFocusedGridView().GetFocusedRow();
                    string err;
                    bool result = this.FieldValidate(gp.Campo, e.Value, GetFocusedGridView().FocusedRowHandle, out err);
                    if (!result)
                    {
                        if (!string.IsNullOrEmpty(err))
                        {
                             err = _bc.GetResource(LanguageTypes.Messages, err);
                             MessageBox.Show(err);
                             e.Valid = false;
                        }
                    }
                    else
                    {
                        //asignar el valor en el datasource de la grilla
                        FieldConfiguration config=this.GetFieldConfigByCaption(gp.Campo);
                        if (config is IntRankConfiguration)
                        {
                            IntRankConfiguration rank = config as IntRankConfiguration;
                        }

                        this.SetEditGridValue(config.RowIndex, e.Value);
                    }
                }
                catch (Exception ex)
                {
                    e.Valid = false;
                    e.ErrorText = "";
                }
                return;
            }
        }

        #region Eventos editor de texto

        /// <summary>
        /// Personaliza comos e muestran los combos de la grilla de edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbRank_DrawItem(object sender, ListBoxDrawItemEventArgs e)
        {

            ComboBoxEdit cbe = (ComboBoxEdit)sender;
            string campoCaption = ((GridProperty)this.GetFocusedGridView().GetFocusedRow()).Campo;
            FieldConfiguration config = this.GetFieldConfigByCaption(campoCaption);
            string field = config.FieldName;
            string desc = string.Empty;
            if (combosData.ContainsKey(field))
            {
                Dictionary<string, string> descs = combosData[field];
                try
                {
                    string num = e.Item.ToString();
                    if (descs.ContainsKey(num))
                    {
                        desc = descs[num];
                    }
                }
                catch (Exception ex)
                {
                    //Combo de datos que no son enteros
                    return;
                }
            }
            string str = e.Item.ToString();
            if ((e.State & DrawItemState.Selected) > 0)
            {
                Font boldFont = new Font(cbe.Font.FontFamily, cbe.Font.Size, FontStyle.Bold);
                e.Graphics.DrawString(e.Item.ToString() + "-" + desc, boldFont, new SolidBrush(cbe.ForeColor),
                  e.Bounds);
                e.Handled = true;
            }
            else
            {
                e.Graphics.DrawString(e.Item.ToString() + "-" + desc, cbe.Font, new SolidBrush(cbe.ForeColor),
                  e.Bounds);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Configura el control de fecha cuando se abre el Popup
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editPeriodo_Popup(object sender, EventArgs e)
        {
            DateEdit edit = sender as DateEdit;
            PopupDateEditForm form = (edit as IPopupControl).PopupWindow as PopupDateEditForm;
            form.Calendar.View = DevExpress.XtraEditors.Controls.DateEditCalendarViewType.YearInfo;
        }

        /// <summary>
        /// Evento que se ejecuta al salir de un campo de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editFecha_Leave(object sender, EventArgs e)
        {
            this._assignDate = true;
        }

        void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            richEditControl.Document.Text = (sender as BaseEdit).EditValue.ToString();
        }

        void riPopup_QueryDisplayText(object sender, QueryDisplayTextEventArgs e)
        {
            e.DisplayText = richEditControl.Document.Text;
        }

        void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = richEditControl.Document.Text;
        }

        #endregion

        #endregion

        #region Eventos barra de herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            if (this.hasEditAccess==null)
                this.hasEditAccess = FormProvider.Master.itemSave.Enabled;

            this.Insertando = true;
            FormProvider.Master.itemSave.Enabled = true;
            
            this.LoadEditGridData(true, 0);
            this.gvRecordEdit.Focus();
            this.gvRecordEdit.FocusedRowHandle = 0;
            this.gvModule.ClearSelection();
        }

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBSave()
        {
            this.gvRecordEdit.PostEditor();
            foreach (GridControl gc in this.extraGrids.Values)
                gc.MainView.PostEditor();
        }

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

                //Codigo
                excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.colIdName));
                col++;
                //Descripción (Para tercero no se pide)
                if (this.DocumentID != (int)AppMasters.coTercero)
                {
                    excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo"));
                    col++;
                }
                //Campos Extras
                this.FrmProperties.Campos.ForEach(f =>
                {
                    if (f.ImportacionInd)
                    {
                        excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + f.NombreColumna));
                        col++;
                    }
                });

                //Activo
                excell_app.AddData(row, col, _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd"));


                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterForm.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para busquedas
        /// </summary>
        public override void TBSearch()
        {
            if (this.pnSearch.Visible)
            {
                #region Oculta el control de busqueda
                this.pnSearch.Visible = false;
                if (this.filtrosConsulta != null)
                {
                    this.filtrosConsulta = null;
                    this.txtCode.Text = string.Empty;
                    this.txtDescrip.Text = string.Empty;
                    this.LoadGridData(false, true, false);
                }
                #endregion
            }
            else
            {
                //Muestra el control de busqueda
                this.pnSearch.Visible = true;
                this.txtCode.Focus();
            }
        }

        /// <summary>
        /// Boton para filtrar la lista de resultados
        /// </summary>
        public override void TBFilter()
        {
            List<DTO_aplMaestraCampo> fields = new List<DTO_aplMaestraCampo>();

            bool complex = false;
            MasterComplexForm mcf = this as MasterComplexForm;
            if (mcf != null)
            {
                complex = true;
            }
            MasterForm ms = this;
            if (ms.FrmProperties.ColumnaID != "" && !complex)
            {
                var col = ms.FrmProperties.ColumnaID;
                DTO_aplMaestraCampo Col_ID = new DTO_aplMaestraCampo("UDT_BasicID");
                //Col_ID.NombreColumna = ms.FrmProperties.ColumnaID;
                Col_ID.NombreColumna = "ID";
                fields.Add(Col_ID);
            }

            //id
            //descripcion
            //activo
            if (!complex)
            {
                DTO_aplMaestraCampo descripcion = new DTO_aplMaestraCampo("UDT_Descriptivo");
                descripcion.NombreColumna = "Descriptivo";
                fields.Add(descripcion);
            }

            fields.AddRange(this.FrmProperties.Campos.Where(x=>x.GrillaInd).ToList());

            DTO_aplMaestraCampo activoInd = new DTO_aplMaestraCampo("UDT_SiNo");
            activoInd.NombreColumna = "ActivoInd";
            fields.Add(activoInd);

            List<ConsultasFields> consultaFields = new List<ConsultasFields>();
            foreach (DTO_aplMaestraCampo f in fields)
            {
                ConsultasFields fl = new ConsultasFields();
                fl.Field = f.NombreColumna;
                fl.FieldShown = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString() + "_" + f.NombreColumna);
                    
                if (fl.Field == "ID")
                    fl.FieldShown = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString() + "_" + this.FrmProperties.ColumnaID);
                    
                Type t = Type.GetType("NewAge.DTO.UDT." + f.Tipo + ", NewAge.DTO");
                PropertyInfo pi = t.GetProperty("Value");
                var type = pi.PropertyType;
                fl.Tipo = type;
                fl.A_Seleccion = false;
                //Agrega
                consultaFields.Add(fl);
            }

            MasterQuery mq = new MasterQuery(ms, this.FrmProperties.DocumentoID, (int)this._bc.AdministrationModel.User.ReplicaID.Value, true, consultaFields);
            foreach (KeyValuePair<string, Dictionary<string, string>> fieldCfg in combosData)
            {
                mq.SetValueDictionary(fieldCfg.Key, fieldCfg.Value);
            }

            List<FieldConfiguration> fks = this.fields.Values.Where(x => (x is ButtonEditFKConfiguration)).ToList();
            foreach (FieldConfiguration fc in fks){
                ButtonEditFKConfiguration fec=fc as ButtonEditFKConfiguration;
                mq.SetFK(fc.FieldName, Convert.ToInt32(fec.FkConfig.ModalFormCode), fec);
            }

            mq.ShowDialog();
        }

        /// <summary>
        /// Boton para asignar un filtro de resultados por defecto
        /// </summary>
        public override void TBFilterDef()
        {
            this.consulta = null;
            this.ConsultaFiels = null;
            List<FieldConfiguration> tem = this.fields.Values.Where(x=>x.GridVisible==true).OrderByDescending(x => x.ColumnIndex).ToList();
            foreach (FieldConfiguration field in tem)
            {
                GridColumn code = this.gvModule.Columns[this.unboundPrefix + field.FieldName];
                code.Width = field.ColumnWidth;
                code.Visible = field.GridVisible;
                code.VisibleIndex = 0;
            }

            this._insertando = false;
            this.LoadGridData(false, true, false);
            this.gvModule.ClearSorting();
        }

        /// <summary>
        /// Boton para generar reportes
        /// </summary>
        public override void TBPrint()
        {
            MasterReport mr = new MasterReport(this.DocumentID, this.consulta);
            mr.DataSource = this.GetReportData();
            mr.ShowPreview();
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            this.importando = true;
            this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
            Thread process = new Thread(this.ImportThread);
            process.Start();
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                #region Carga los titulos de las columnas

                string colsRsx = string.Empty;
                string separator = ",";

                //Codigo y descripcion
                colsRsx += _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + this.colIdName);
                if (this.DocumentID != (int)AppMasters.coTercero)
                    colsRsx += separator + _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_Descriptivo");

                //Campos Extras
                this.FrmProperties.Campos.ForEach(f =>
                {
                    if (f.ImportacionInd)
                        colsRsx += separator + _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_" + f.NombreColumna);
                });

                //Activo
                colsRsx += separator + _bc.GetResource(LanguageTypes.Forms, this.FrmProperties.DocumentoID.ToString() + "_ActivoInd");

                #endregion

                string fileName = _bc.AdministrationModel.MasterSimple_Export(this.DocumentID, colsRsx, this.consulta, this.filtrosConsulta);
                string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, fileName);

                Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-MasterSimpleForm.cs", "TBExport"));
            }
        }

         /// <summary>
        /// Boton para actualizar la lista
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadGridData(false, true, false,true);
        }

        #endregion

        #region Funciones reflection

        /// <summary>
        /// Genera grid properties para los campos extra
        /// </summary>
        /// <returns></returns>
        internal virtual List<GridProperty> GetExtraGridProperties(Type dtoType, object dto)
        
        {
            List<GridProperty> result = new List<GridProperty>();
            foreach (DTO_aplMaestraCampo mf in this.FrmProperties.Campos)
            
            {
                try
                {
                    FieldConfiguration fc = this.GetFieldConfigByFieldName(mf.NombreColumna);
                    if (dto != null)
                    {
                        #region Si el DTO tiene datos
                        Object value = null;
                        if (this.Insertando)
                            value = this.NewRecordLoadData(mf.NombreColumna);
                        else
                        {
                            #region Carga los campos de edicion
                            if (fc is ImageFieldConfiguration)
                            {
                                #region Columna de imagen
                                int? replicaId = null;
                                if (dto is DTO_MasterBasic)
                                    replicaId = (dto as DTO_MasterBasic).ReplicaID.Value;

                                if (dto is DTO_MasterComplex)
                                    replicaId = (dto as DTO_MasterComplex).ReplicaID.Value;

                                if (replicaId != null)
                                {
                                    byte[] arr = _bc.AdministrationModel.Master_GetImage(this.DocumentID, (int)replicaId, mf.NombreColumna);
                                    value = Utility.ByteArrayToObject(arr);
                                    if (value == null)
                                        value = string.Empty;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Otras columnas
                                if (dtoType == typeof(DTO_glEmpresa) && mf.NombreColumna == "EmpresaGrupoID")
                                {
                                    DTO_glEmpresa emp = (DTO_glEmpresa)dto;
                                    value = emp.EmpresaGrupoID_.Value;
                                }
                                else if (dtoType == typeof(DTO_seUsuario) && mf.NombreColumna == "Contrasena")
                                    value = string.Empty;
                                else if (dtoType == typeof(DTO_coTercero) && mf.NombreColumna == "Contrasena")
                                    value = string.Empty;
                                else
                                    value = Utility.GetPropertyValueToString(dto, mf.NombreColumna);
                                #endregion
                            }

                            #endregion
                        }

                        if (value.Equals(string.Empty) && mf.Tipo.Equals("UDT_SiNo"))
                            value = bool.FalseString;

                        if (value != null)
                            result.Add(new GridProperty(fc.Caption, value, fc.EditVisible) { Tab = fc.Tab });
                        #endregion
                    }
                    else
                    {
                        #region Carga el DTO por reflection
                        //Se debe hacer por reflection la lista
                        Type tipopropiedad = null;
                        PropertyInfo pi = dtoType.GetProperty(mf.NombreColumna);
                        if (pi != null)
                            tipopropiedad = pi.PropertyType;
                        else
                        {
                            FieldInfo fi = dtoType.GetField(mf.NombreColumna);
                            if (fi != null)
                                tipopropiedad = fi.FieldType;
                        }
                        if (tipopropiedad != null)
                        {
                            string defValue = this.NewRecordLoadData(mf.NombreColumna);

                            string c = fc.Caption;
                            string val = string.IsNullOrEmpty(defValue) ? UDT.DefaultStringValue(tipopropiedad) : defValue;
                            bool v = fc.EditVisible;
                            GridProperty gp = new GridProperty(c, val, v);
                            gp.Tab=fc.Tab;
                            result.Add(gp);
                        }
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Err_GettingColsInfo));
                }
            }
            return result;
        }

        #endregion    

        #region Hilos

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public virtual void ImportThread()
        {
            this.saveNew = true;
        }

        #endregion

    }//clase
}//namespace
