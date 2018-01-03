using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Configuration;
using SentenceTransformer;
using System.Collections;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalTareasFilter : Form
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables basicas
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";
        private int _pageSize = 50;
        private bool _filterActive = false;
        private bool _selectMasivoInd = false;
        //Variables de data
        private DTO_TareasFilter _tareaCurrent = new DTO_TareasFilter();
        private List<DTO_TareasFilter> _listTareas = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public string TareaSelected
        {
            get { return this._tareaCurrent.TareaID.Value; }
        }

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<string> ListTareaSelected
        {
            get
            {
                List<string> res = new List<string>();
                foreach (var item in _listTareas.FindAll(x => x.SelectInd.Value == true))
                    res.Add(item.TareaID.Value);
                return res;
            }
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ModalTareasFilter(bool selectMasivo)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._selectMasivoInd = selectMasivo;
                _bc.Pagging_Init(this.pgGrid, _pageSize);
                _bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
                this.AddGridColsTarea();
                this.AddGridColsRecurso();
                this.LoadGridData();
                FormProvider.LoadResources(this, AppDocuments.PreProyecto);
              
                this.Text = this._bc.GetResource(LanguageTypes.Forms, "110_frmTareas");
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinAppModalTareasFilter.cs", "ModalTareasFilter: " + ex.Message));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.PreProyecto;
            this._frmModule = ModulesPrefix.py;

            #region Inicializa Controles
            this._bc.InitMasterUC(this.masterGrupoInv, AppMasters.inRefGrupo, true, false, false, false);
            this._bc.InitMasterUC(this.masterClaseInv, AppMasters.inRefClase, true, false, false, false);
            this._bc.InitMasterUC(this.masterTipoInv, AppMasters.inRefTipo, true, false, false, false);
            this._bc.InitMasterUC(this.masterSerieInv, AppMasters.inSerie, true, false, false, false);
            this._bc.InitMasterUC(this.masterMaterialInv, AppMasters.inMaterial, true, false, false, false);
            this._bc.InitMasterUC(this.masterMarcaInv, AppMasters.inMarca, true, false, false, false);
            this._bc.InitMasterUC(this.masterUnidadInv, AppMasters.inUnidad, true, false, false, false);
            this._bc.InitMasterUC(this.masterEmpaqueInv, AppMasters.inEmpaque, true, false, false, false);
            #endregion
            this._listTareas = new List<DTO_TareasFilter>();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsTarea()
        {
            //Aprobar
            GridColumn SelectInd = new GridColumn();
            SelectInd.FieldName = this._unboundPrefix + "SelectInd";
            SelectInd.Caption = "√";
            SelectInd.UnboundType = UnboundColumnType.Boolean;
            SelectInd.VisibleIndex = 0;
            SelectInd.Width = 15;
            SelectInd.Visible = this._selectMasivoInd;
            SelectInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SelectInd");
            SelectInd.AppearanceHeader.ForeColor = Color.Lime;
            SelectInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            SelectInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            SelectInd.AppearanceHeader.Options.UseTextOptions = true;
            SelectInd.AppearanceHeader.Options.UseFont = true;
            SelectInd.AppearanceHeader.Options.UseForeColor = true;
            this.gvTarea.Columns.Add(SelectInd);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this._unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 0;
            TareaID.Width = 70;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaID);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this._unboundPrefix + "TareaDesc";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TareaDesc");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 1;
            TareaDesc.Width = 200;
            TareaDesc.Visible = true;
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaDesc);           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridColsRecurso()
        {
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this._unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            RecursoID.AppearanceCell.Options.UseTextOptions = true;
            RecursoID.AppearanceCell.Options.UseFont = true;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 55;
            RecursoID.Visible = true;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this._unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RecursoDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 150;
            RecursoDesc.Visible = true;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn ClaseServicioID = new GridColumn();
            ClaseServicioID.FieldName = this._unboundPrefix + "ClaseServicioID";
            ClaseServicioID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseServicioID");
            ClaseServicioID.UnboundType = UnboundColumnType.String;
            ClaseServicioID.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ClaseServicioID.AppearanceCell.Options.UseTextOptions = true;
            ClaseServicioID.AppearanceCell.Options.UseFont = true;
            ClaseServicioID.VisibleIndex = 2;
            ClaseServicioID.Width = 58;
            ClaseServicioID.Visible = true;
            this.gvRecurso.Columns.Add(ClaseServicioID);

            GridColumn ClaseServicioDesc = new GridColumn();
            ClaseServicioDesc.FieldName = this._unboundPrefix + "ClaseServicioDesc";
            ClaseServicioDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseServicioDesc");
            ClaseServicioDesc.UnboundType = UnboundColumnType.String;
            ClaseServicioDesc.VisibleIndex = 3;
            ClaseServicioDesc.Width = 100;
            ClaseServicioDesc.Visible = true;
            this.gvRecurso.Columns.Add(ClaseServicioDesc);

            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this._unboundPrefix + "RefProveedor";
            RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 4;
            RefProveedor.Width = 70;
            RefProveedor.Visible = true;
            this.gvRecurso.Columns.Add(RefProveedor);

            GridColumn MarcaDesc = new GridColumn();
            MarcaDesc.FieldName = this._unboundPrefix + "MarcaDesc";
            MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MarcaDesc");
            MarcaDesc.UnboundType = UnboundColumnType.String;
            MarcaDesc.VisibleIndex = 5;
            MarcaDesc.Width = 70;
            MarcaDesc.Visible = true;
            this.gvRecurso.Columns.Add(MarcaDesc);

            GridColumn MaterialDesc = new GridColumn();
            MaterialDesc.FieldName = this._unboundPrefix + "MaterialDesc";
            MaterialDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_MaterialDesc");
            MaterialDesc.UnboundType = UnboundColumnType.String;
            MaterialDesc.VisibleIndex = 6;
            MaterialDesc.Width = 70;
            MaterialDesc.Visible = true;
            this.gvRecurso.Columns.Add(MaterialDesc);

            GridColumn SerieDesc = new GridColumn();
            SerieDesc.FieldName = this._unboundPrefix + "SerieDesc";
            SerieDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SerieDesc");
            SerieDesc.UnboundType = UnboundColumnType.String;
            SerieDesc.VisibleIndex = 7;
            SerieDesc.Width = 70;
            SerieDesc.Visible = true;
            this.gvRecurso.Columns.Add(SerieDesc);

            GridColumn UnidadDesc = new GridColumn();
            UnidadDesc.FieldName = this._unboundPrefix + "UnidadDesc";
            UnidadDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_UnidadDesc");
            UnidadDesc.UnboundType = UnboundColumnType.String;
            UnidadDesc.VisibleIndex = 8;
            UnidadDesc.Width = 70;
            UnidadDesc.Visible = true;
            this.gvRecurso.Columns.Add(UnidadDesc);

            GridColumn EmpaqueDesc = new GridColumn();
            EmpaqueDesc.FieldName = this._unboundPrefix + "EmpaqueDesc";
            EmpaqueDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EmpaqueDesc");
            EmpaqueDesc.UnboundType = UnboundColumnType.String;
            EmpaqueDesc.VisibleIndex = 9;
            EmpaqueDesc.Width = 70;
            EmpaqueDesc.Visible = true;
            this.gvRecurso.Columns.Add(EmpaqueDesc);

            GridColumn ClaseDesc = new GridColumn();
            ClaseDesc.FieldName = this._unboundPrefix + "ClaseDesc";
            ClaseDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseDesc");
            ClaseDesc.UnboundType = UnboundColumnType.String;
            ClaseDesc.VisibleIndex = 10;
            ClaseDesc.Width = 70;
            ClaseDesc.Visible = true;
            this.gvRecurso.Columns.Add(ClaseDesc);

            GridColumn GrupoDesc = new GridColumn();
            GrupoDesc.FieldName = this._unboundPrefix + "GrupoDesc";
            GrupoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_GrupoDesc");
            GrupoDesc.UnboundType = UnboundColumnType.String;
            GrupoDesc.VisibleIndex = 11;
            GrupoDesc.Width = 70;
            GrupoDesc.Visible = true;
            this.gvRecurso.Columns.Add(GrupoDesc);

            GridColumn TipoDesc = new GridColumn();
            TipoDesc.FieldName = this._unboundPrefix + "TipoDesc";
            TipoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoDesc");
            TipoDesc.UnboundType = UnboundColumnType.String;
            TipoDesc.VisibleIndex = 12;
            TipoDesc.Width = 70;
            TipoDesc.Visible = true;
            this.gvRecurso.Columns.Add(TipoDesc);

            this.gvRecurso.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
        }

        /// <summary>
        /// Carga los datos de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                DTO_TareasFilter filter = new DTO_TareasFilter();
                filter.TareaID.Value = this.txtCodigo.Text;
                filter.TareaDesc.Value = this.txtDesc.Text;
                filter.RefProveedor.Value = this.txtRefProveed.Text;
                filter.MarcaInvID.Value = this.masterMarcaInv.Value;
                filter.MaterialInvID.Value = this.masterMaterialInv.Value;
                filter.SerieID.Value = this.masterSerieInv.Value;
                filter.UnidadInvID.Value = this.masterUnidadInv.Value;
                filter.EmpaqueInvID.Value = this.masterEmpaqueInv.Value;
                filter.TipoInvID.Value = this.masterTipoInv.Value;
                filter.GrupoInvID.Value = this.masterGrupoInv.Value;
                filter.ClaseInvID.Value = this.masterClaseInv.Value;
                List<string> seleccionados = this._listTareas.Where(x => x.SelectInd.Value == true).Select(x => x.TareaID.Value).ToList();
                this._listTareas = this._bc.AdministrationModel.TareasFilter_Get(filter);

                //Valida los que ya seleccionaron
                foreach (string sel in seleccionados)
                {
                    DTO_TareasFilter tar = this._listTareas.Find(x => x.TareaID.Value == sel);
                    if (tar != null)
                        tar.SelectInd.Value = true;
                }

                this.pgGrid.UpdatePageNumber(this._listTareas.Count, true, true, false);

                var tmp = this._listTareas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_TareasFilter>();
                this.gvTarea.MoveFirst();
                //this.gcTarea.DataSource = null;
                this.gcTarea.DataSource = tmp;
                this.gcTarea.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasFilter.cs", "ModalTareasFilter(" + this._documentID + ")-LoadGridData: " + ex.Message));
            }
        }
    
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            try
            {              
                var tmp = this._filterActive ?  this._listTareas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_TareasFilter>() :
                                                this._listTareas.Skip((this.pgGrid.PageNumber - 1) * this._pageSize).Take(this._pageSize).ToList<DTO_TareasFilter>(); ;
                this.pgGrid.UpdatePageNumber(this._listTareas.Count, false, false, false);
                this.gvTarea.MoveFirst();
                this.gcTarea.DataSource = tmp;

                if (this.gvTarea.DataRowCount > 0)
                {
                    this._tareaCurrent = (DTO_TareasFilter)this.gvTarea.GetRow(this.gvTarea.FocusedRowHandle);
                    this.gcRecurso.DataSource = this._tareaCurrent.Detalle;
                }
                else
                    this._tareaCurrent = new DTO_TareasFilter();

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasFilter.cs", "pagging_Click: " + ex.Message)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.LoadGridData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.btnFilter_Click(this.btnFilter, e);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
        {
            if(this.gvTarea.DataRowCount > 0)
                this._tareaCurrent = (DTO_TareasFilter)this.gvTarea.GetRow(this.gvTarea.FocusedRowHandle);
            this.Close();
        }

        /// <summary>
        /// Al hacer click para cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._tareaCurrent = new DTO_TareasFilter();
            this.Close();
        }

        #endregion        

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
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
            if (e.IsSetData)
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

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._tareaCurrent = (DTO_TareasFilter)this.gvTarea.GetRow(e.FocusedRowHandle);
                    this.gvRecurso.MoveFirst();
                    this.gcRecurso.DataSource = this._tareaCurrent.Detalle;
                }
                else
                    this.gcRecurso.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasFilter.cs", "gvTarea_FocusedRowChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTarea_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    this._tareaCurrent = (DTO_TareasFilter)this.gvTarea.GetRow(e.RowHandle);
                    this.gvRecurso.MoveFirst();
                    this.gcRecurso.DataSource = this._tareaCurrent.Detalle;
                }
                else
                    this.gcRecurso.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasFilter.cs", "gvTarea_RowClick: " + ex.Message));
            }
        }


        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion      

    
    }
}
