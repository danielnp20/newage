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
using System.Globalization;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalCostosInsumo : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        private string unboundPrefix = "Unbound_";
        private bool isValid = false;
        private bool isCancel = false;
        private int _documentID;
        private decimal _porcAIU = 0;
        private DTO_pyProyectoMvto _rowCurrent = new DTO_pyProyectoMvto();
        private List<DTO_pyProyectoMvto> _listInsumos = new List<DTO_pyProyectoMvto>();
        private Dictionary<string, decimal> itemsModif = new Dictionary<string, decimal>();


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalCostosInsumo()
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();               
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "ModalCostosInsumo")); 
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalCostosInsumo(List<DTO_pyProyectoMvto> movs, decimal porcAIU)
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();
                this._porcAIU = porcAIU;
                this._listInsumos = movs;     
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "ModalCostosInsumo"));
            }
        }      

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Grilla Recursos
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 0;
                RecursoID.Width = 50;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = true;
                this.gvRecursos.Columns.Add(RecursoID);

                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 1;
                RecursoDesc.Width = 130;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                this.gvRecursos.Columns.Add(RecursoDesc);

                GridColumn UnidadInvIDrec = new GridColumn();
                UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrec.UnboundType = UnboundColumnType.String;
                UnidadInvIDrec.VisibleIndex = 2;
                UnidadInvIDrec.Width = 50;
                UnidadInvIDrec.Visible = true;
                UnidadInvIDrec.OptionsColumn.AllowEdit = false;
                this.gvRecursos.Columns.Add(UnidadInvIDrec);
   
                GridColumn CantidadTOT = new GridColumn();
                CantidadTOT.FieldName = this.unboundPrefix + "CantidadSUM";
                CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.ModalTareasProyecto + "_CantidadSUM");
                CantidadTOT.UnboundType = UnboundColumnType.Decimal;
                CantidadTOT.VisibleIndex = 8;
                CantidadTOT.Width = 80;
                CantidadTOT.Visible = true;
                CantidadTOT.ColumnEdit = this.editCant2;
                CantidadTOT.OptionsColumn.AllowEdit = false;
                this.gvRecursos.Columns.Add(CantidadTOT);

                //Costo Unit
                GridColumn CostoLocal = new GridColumn();
                CostoLocal.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocal.UnboundType = UnboundColumnType.Decimal;
                CostoLocal.VisibleIndex = 9;
                CostoLocal.Width = 80;
                CostoLocal.Visible = true;
                CostoLocal.ColumnEdit = this.editSpin;
                CostoLocal.OptionsColumn.AllowEdit = true;
                this.gvRecursos.Columns.Add(CostoLocal);

                GridColumn CostoTotalML = new GridColumn();
                CostoTotalML.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
                CostoTotalML.UnboundType = UnboundColumnType.Decimal;
                CostoTotalML.VisibleIndex = 10;
                CostoTotalML.Width = 80;
                CostoTotalML.Visible = true;
                CostoTotalML.ColumnEdit = this.editSpin;
                CostoTotalML.OptionsColumn.AllowEdit = false;
                CostoTotalML.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                CostoTotalML.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
                this.gvRecursos.Columns.Add(CostoTotalML);           

                GridColumn TipoRecurso = new GridColumn();
                TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecurso.UnboundType = UnboundColumnType.Integer;
                TipoRecurso.Width = 80;
                TipoRecurso.Visible = false;
                TipoRecurso.Group();
                TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
                this.gvRecursos.Columns.Add(TipoRecurso);

                this.gvRecursos.OptionsBehavior.Editable = true;
                this.gvRecursos.OptionsView.ColumnAutoWidth = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo", "AddGridCols"));
            }
        }

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {  
            try
            {                
                FormProvider.LoadResources(this, this._documentID);
                this.LoadData();
                this.UpdateValues();  
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "InitControls")); 
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalTareasProyecto;
            this.AddGridCols();           
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.gcRecursos.DataSource = this._listInsumos;
                this.gcRecursos.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "LoadData"));
            }            
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                if(col == "TareaID")
                {
                    props.DocumentoID = AppMasters.pyTarea;
                    props.DTOTipo = "DTO_pyTarea";
                    props.ModuloID = ModulesPrefix.py;
                    props.NombreTabla = "pyTarea";
                }
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Actualiza los costos generales
        /// </summary>
        private void UpdateValues()
        {
            try
            {
                decimal _totalPresupuesto = 0;
                decimal _totalPresupuestoAIU = 0;
                foreach (DTO_pyProyectoMvto rec in this._listInsumos)
                    _totalPresupuesto += rec.CostoTotalML.Value.Value;
               
                //Actualiza valores generales
                this.txtTotalPresup.EditValue = _totalPresupuesto;
                this.txtTotalPresupAIU.EditValue = _totalPresupuesto + (_totalPresupuesto * (this._porcAIU / 100));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumos.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Carga la Data en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// se ejecuta al cambiar de foco de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvRecurso_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {              
                this._rowCurrent = (DTO_pyProyectoMvto)this.gvRecursos.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Se realiza al digitar una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcRecurso_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.gvRecursos.FocusedRowHandle >= 0)
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
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gRecurso_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {            
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvRecursos.Columns[this.unboundPrefix + fieldName];                
              
                if (fieldName == "CostoLocal")
                {
                    this._rowCurrent.CostoTotalML.Value = e.Value != null ? Math.Round((Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) * this._rowCurrent.CantidadSUM.Value.Value),2) : 0;
                    this.itemsModif.Remove(this._rowCurrent.RecursoID.Value);
                    this.itemsModif.Add(this._rowCurrent.RecursoID.Value, this._rowCurrent.CostoLocal.Value.Value);
                    this.UpdateValues();
                }
                this.gvRecursos.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
                    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "MATERIALES";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "EQUIPO-HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = "MANO DE OBRA";
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = "TRANSPORTES";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudTrabajo.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvRecursos.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                if (colName != "CapituloTrabajoID")
                    this.ShowFKModal(this.gvRecursos.FocusedRowHandle,colName, origin);
                else
                    this.ShowFKModal(this.gvRecursos.FocusedRowHandle, colName, origin);
                    //this.ShowFKComplex(this.gvDocument.FocusedRowHandle, colName, origin);               

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "editBtnGrid_ButtonClick"));
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
                //int index = this.gvDocument.FocusedRowHandle;
                //if (((CheckEdit)sender).Checked)
                //{
                //    DTO_pyPreProyectoTarea det = this._listTareaDet[index];
                //    det.TrabajoID.Value = string.Empty;
                //    det.Cantidad.Value = 1;
                //    det.CantidadTarea.Value = 1;
                //    this._listTareaSelected.Add(det);
                //}
                //else
                //    this._listTareaSelected.RemoveAll(x => x.TareaID.Value == this._listTareaDet[index].TareaID.Value);
                //this.gvDocument.RefreshData();
                //this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "editChek_CheckedChanged"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
              this.Close();
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.isCancel = true;
        }

        

        /// <summary>
        /// Cuando cambia el valor del texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evente</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            try
            {
                //ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;

                //if (master.ValidID)
                //{
                //    if (master.ColId == "CapituloTrabajID")
                //        this._tareaSelected.CapituloTrabajoID.Value = master.Value;
                //    else if (master.ColId == "UnidadInvID")
                //        this._tareaSelected.UnidadInvID.Value = master.Value;
                //    else if (master.ColId == "CapituloGrupoID")
                //        this._tareaSelected.CapituloGrupoID.Value = master.Value;
                //}
                //else
                //{
                //    if (master.DocId != 0)
                //    {
                //        MessageBox.Show(master.Value + " " + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FieldNotFound));
                //        master.Focus();
                //    }
                //}
                //this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {    
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalCostosInsumo.cs", "master_Leave"));
            }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Items seleccionados
        /// </summary>
        public List<DTO_pyProyectoMvto> ListInsumosReturn
        {
            get { return this._listInsumos; }
        }

       /// <summary>
        /// Indicador de Cancelar
        /// </summary>
        public bool isCancelSelected
        {
            get { return this.isCancel; }
        }

        /// <summary>
        /// Items seleccionados
        /// </summary>
        public Dictionary<string, decimal> recursosModif
        {
            get { return this.itemsModif; }
        }

        #endregion  
    }
}
