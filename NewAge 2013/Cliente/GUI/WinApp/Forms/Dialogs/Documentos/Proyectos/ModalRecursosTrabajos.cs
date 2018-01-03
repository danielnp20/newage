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
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalRecursosTrabajo : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_pyPreProyectoDeta> _listRecursosDet = null;
        protected List<DTO_pyPreProyectoDeta> _listRecursosAll = null;
        protected List<DTO_pyPreProyectoDeta> _listRecursoSelected = null;
        protected List<int> _recursosDelete = new List<int>();
        private DTO_pyPreProyectoDeta detCurrent = new DTO_pyPreProyectoDeta();
        private string unboundPrefix = "Unbound_";
        private int _documentID;
        private decimal _tasaCambio = 0;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalRecursosTrabajo()
        {
           // this.InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalRecursosTrabajo(List<DTO_pyPreProyectoDeta> recursos, decimal tasaCambio)
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();
                this.InitControls(recursos);
                this._tasaCambio = tasaCambio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos.cs", "ModalRecursosTrabajos")); ;
            }
        }      

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls(List<DTO_pyPreProyectoDeta> recursos)
        {
            #region Paginador
            //this._bc.Pagging_Init(this.pgGrid, this._pageSize);
            //this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
            //this.pgGrid.UpdatePageNumber(this._listRecursosDet.Count, true, true, false);
            //this.toolTipGrid.SetToolTip(this.gcDocument, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ToolTipGrid)); 
            #endregion

            FormProvider.LoadResources(this, this._documentID);

            this.LoadData();
            this._listRecursoSelected = recursos;

            this._listRecursosDet.ForEach(newRec =>
            {
                this._listRecursoSelected.ForEach(sel =>
                {
                    if (sel.RecursoID.Value == newRec.RecursoID.Value)
                        newRec.SelectInd.Value = true;
                });
            });
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalRecursosTrabajo;
            this._listRecursosDet = new List<DTO_pyPreProyectoDeta>();
            this._listRecursoSelected = new List<DTO_pyPreProyectoDeta>();
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Componentes
                //Aprobar
                GridColumn SelectInd = new GridColumn();
                SelectInd.FieldName = this.unboundPrefix + "SelectInd";
                SelectInd.Caption = "√";
                SelectInd.UnboundType = UnboundColumnType.Boolean;
                SelectInd.VisibleIndex = 0;
                SelectInd.Width = 15;
                SelectInd.Visible = true;
                SelectInd.ToolTip = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SelectInd");
                SelectInd.AppearanceHeader.ForeColor = Color.Lime;
                SelectInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                SelectInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                SelectInd.AppearanceHeader.Options.UseTextOptions = true;
                SelectInd.AppearanceHeader.Options.UseFont = true;
                SelectInd.AppearanceHeader.Options.UseForeColor = true;
                SelectInd.ColumnEdit = this.editChkBox;
                this.gvDocument.Columns.Add(SelectInd);

                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.AppearanceCell.Options.UseFont = true;
                RecursoID.AppearanceCell.Options.UseTextOptions = true;
                RecursoID.VisibleIndex = 1;
                RecursoID.Width = 90;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RecursoID);

                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 2;
                RecursoDesc.Width = 250;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RecursoDesc);

                GridColumn FactorID = new GridColumn();
                FactorID.FieldName = this.unboundPrefix + "FactorID";
                FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorID.UnboundType = UnboundColumnType.Decimal;
                FactorID.VisibleIndex = 3;
                FactorID.Width = 80;
                FactorID.Visible = false;
                FactorID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FactorID);

                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
                UnidadInvID.VisibleIndex = 3;
                UnidadInvID.Width = 40;
                UnidadInvID.Visible = true;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(UnidadInvID);

                GridColumn CostoLocal = new GridColumn();
                CostoLocal.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocal.UnboundType = UnboundColumnType.Integer;
                CostoLocal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CostoLocal.AppearanceCell.Options.UseTextOptions = true;
                CostoLocal.VisibleIndex = 4;
                CostoLocal.Width = 80;
                CostoLocal.Visible = true;
                CostoLocal.ColumnEdit = this.editSpin;
                CostoLocal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CostoLocal);

                if(this._bc.AdministrationModel.MultiMoneda)
                {
                    GridColumn CostoExtra = new GridColumn();
                    CostoExtra.FieldName = this.unboundPrefix + "CostoExtra";
                    CostoExtra.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoExtra");
                    CostoExtra.UnboundType = UnboundColumnType.Integer;
                    CostoExtra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    CostoExtra.AppearanceCell.Options.UseTextOptions = true;
                    CostoExtra.VisibleIndex = 5;
                    CostoExtra.Width = 80;
                    CostoExtra.Visible = true;
                    CostoExtra.ColumnEdit = this.editSpin;
                    CostoExtra.OptionsColumn.AllowEdit = false;
                    this.gvDocument.Columns.Add(CostoExtra);
                }
             
                GridColumn Modelo = new GridColumn();
                Modelo.FieldName = this.unboundPrefix + "Modelo";
                Modelo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Modelo");
                Modelo.UnboundType = UnboundColumnType.String;
                Modelo.VisibleIndex = 6;
                Modelo.Width = 60;
                Modelo.Visible = true;
                Modelo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Modelo);

                GridColumn MarcaDesc = new GridColumn();
                MarcaDesc.FieldName = this.unboundPrefix + "MarcaDesc";
                MarcaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_MarcaDesc");
                MarcaDesc.UnboundType = UnboundColumnType.String;
                MarcaDesc.VisibleIndex = 7;
                MarcaDesc.Width = 40;
                MarcaDesc.Visible = true;
                MarcaDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MarcaDesc);


                this.gvDocument.OptionsBehavior.Editable = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                this._listRecursosDet = this._bc.AdministrationModel.pyPreProyectoDeta_GetByTarea(this._documentID, string.Empty, string.Empty, null, false);

                #region Convierte Dolares en Pesos
                foreach (var det in this._listRecursosDet)
                {
                    if (det.CostoExtra.Value != 0 && det.CostoLocal.Value == 0 && this._tasaCambio != 0)
                        det.CostoLocal.Value = det.CostoExtra.Value * this._tasaCambio;
                } 
                #endregion

                this._listRecursosAll = this._listRecursosDet;
                this.rbtnTipoRecurso.SelectedIndex = 4;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos.cs", "LoadData"));
            }
        }
       
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Carga la Data en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
        /// Se realiza al digitar una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocument_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.gvDocument.FocusedRowHandle >= 0)
                        this.Close();
                }
                if (e.KeyCode == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editChek_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int index = this.gvDocument.FocusedRowHandle;
                if (((CheckEdit)sender).Checked)
                {
                    if (!this._listRecursoSelected.Exists(x => x.RecursoID.Value == this.detCurrent.RecursoID.Value))
                    {
                        if (this.detCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            this.detCurrent.Peso_Cantidad.Value = 1; //Cantidad de personal por def
                        this._listRecursoSelected.Add(this.detCurrent);
                    }
                }
                else
                {
                    var rec = this._listRecursoSelected.Find(x => x.RecursoID.Value == this.detCurrent.RecursoID.Value);
                    if (rec != null && rec.Consecutivo.Value != null && !this._recursosDelete.Exists(x => x == rec.Consecutivo.Value))
                        this._recursosDelete.Add(rec.Consecutivo.Value.Value);
                    this._listRecursoSelected.RemoveAll(x => x.RecursoID.Value == this.detCurrent.RecursoID.Value);                    
                }
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos.cs", "editChek_CheckedChanged"));
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
               this.detCurrent = (DTO_pyPreProyectoDeta)this.gvDocument.GetRow(e.FocusedRowHandle);
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    this.detCurrent = (DTO_pyPreProyectoDeta)this.gvDocument.GetRow(e.RowHandle);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                    this.detCurrent = (DTO_pyPreProyectoDeta)this.gvDocument.GetRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            try
            {
                DTO_pyPreProyectoDeta det = (DTO_pyPreProyectoDeta)this.gvDocument.GetRow(e.RowHandle);
                if (fieldName == "RecursoID" && det != null)
                {
                    if(det.TipoRecurso.Value == (byte)TipoRecurso.Insumo)
                       e.Appearance.ForeColor = Color.Blue;
                    else if (det.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                        e.Appearance.ForeColor = Color.Green;
                    else if (det.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                        e.Appearance.ForeColor = Color.Red;
                    else if (det.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                        e.Appearance.ForeColor = Color.Maroon;
                }
                   
            }
            catch (Exception)
            {                
                throw;
            }
        }


        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.pgGrid.UpdatePageNumber(this._listRecursosDet.Count, false, false, false);
        }

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (((CheckEdit)sender).Checked)
                {
                    this._listRecursoSelected.AddRange(this._listRecursosDet);
                    foreach (var doc in this._listRecursosDet)
                        doc.SelectInd.Value = true;
                }
                else
                {
                    this._listRecursoSelected.Clear();
                    foreach (var doc in this._listRecursosDet)
                        doc.SelectInd.Value = false;
                }
                this.gcDocument.DataSource = this._listRecursosDet;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos.cs", "chkSelectAll_CheckedChanged"));
            }
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Cuando selecciona un item del radioGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnTipoRecurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Insumo
                if (this.rbtnTipoRecurso.SelectedIndex == 0)
                    this._listRecursosDet = this._listRecursosAll.FindAll(x => x.TipoRecurso.Value == 1).ToList();
                //Equipo
                else if (this.rbtnTipoRecurso.SelectedIndex == 1)
                    this._listRecursosDet = this._listRecursosAll.FindAll(x => x.TipoRecurso.Value == 2).ToList();
                //Mano de Obra
                else if (this.rbtnTipoRecurso.SelectedIndex == 2)
                    this._listRecursosDet = this._listRecursosAll.FindAll(x => x.TipoRecurso.Value == 3).ToList();
                //Transporte
                else if (this.rbtnTipoRecurso.SelectedIndex == 3)
                    this._listRecursosDet = this._listRecursosAll.FindAll(x => x.TipoRecurso.Value == 4).ToList();
                //Todos
                else if (this.rbtnTipoRecurso.SelectedIndex == 4)
                    this._listRecursosDet = this._listRecursosAll;
                //Seleccionados
                else if (this.rbtnTipoRecurso.SelectedIndex == 5)
                    this._listRecursosDet = this._listRecursosAll.FindAll(x => x.SelectInd.Value.Value).ToList();

                this.gcDocument.DataSource = this._listRecursosDet;
                this.gcDocument.RefreshDataSource();
                if (this.gvDocument.FocusedRowHandle >= 0)
                    this.detCurrent = (DTO_pyPreProyectoDeta)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalRecursosTrabajos.cs", "rbtnTipoRecurso_SelectedIndexChanged"));
            }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_pyPreProyectoDeta> ListSelected
        {
            get { return this._listRecursoSelected; }
        }

        /// <summary>
        /// Indica si permite selecionar y retornar varios Docs
        /// </summary>
        public Boolean MultipleSelection
        {
            get;
            set;
        }

        /// <summary>
        /// Items a eliminar
        /// </summary>
        public List<int> RecursosDelete
        {
            get { return this._recursosDelete;}
        }

        #endregion
    }
}
