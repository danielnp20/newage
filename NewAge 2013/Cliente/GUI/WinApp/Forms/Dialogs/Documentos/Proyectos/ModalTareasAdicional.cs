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

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalTareasAdicional : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_pyPreProyectoTarea> _listTareasAdic = null;
        private string unboundPrefix = "Unbound_";
        private bool isValid = false;
        private bool deleteOP = false;
        private int _documentID;
        private string tareaXDef = string.Empty;
        private DTO_pyPreProyectoTarea _rowTareaCurrent = new DTO_pyPreProyectoTarea();
        private List<DTO_glConsultaFiltro> _filtrosRef;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalTareasAdicional()
        {
           // this.InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalTareasAdicional(List<DTO_pyPreProyectoTarea> tareasAdic)
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();               
                this.InitControls(tareasAdic);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "ModalTareasAdicional")); 
            }
        }      

        #region Funciones Privadas

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowTarea()
        {
            try
            {
                DTO_pyPreProyectoTarea footerDet = new DTO_pyPreProyectoTarea();

                #region Asigna datos a la fila
                try
                {
                    if (this._listTareasAdic.Count > 0)
                    {
                        decimal indexTareaID = Convert.ToDecimal(this._listTareasAdic.Last().TareaID.Value, CultureInfo.InvariantCulture) + 1;
                        footerDet.Index = this._listTareasAdic.Last().Index + 1;
                        footerDet.TareaCliente.Value = this._listTareasAdic.Last().TareaCliente.Value;
                        footerDet.TareaID.Value = indexTareaID.ToString();
                        footerDet.Descriptivo.Value = this._listTareasAdic.Last().Descriptivo.Value;
                        footerDet.UnidadInvID.Value = this._listTareasAdic.Last().UnidadInvID.Value;
                        footerDet.CostoAdicionalInd.Value = true;
                        footerDet.Cantidad.Value = 1;
                        footerDet.CantidadTarea.Value = 1;
                        footerDet.CostoLocalCLI.Value = 0;
                        footerDet.CostoTotalUnitML.Value = 0;
                        footerDet.CostoTotalML.Value = 0;
                    }
                    else
                    {
                        footerDet.Index = 0;
                        footerDet.TareaCliente.Value = string.Empty;
                        footerDet.TareaID.Value = "11500"; //Consecutivo de tareas Adic
                        footerDet.Descriptivo.Value = string.Empty;
                        footerDet.UnidadInvID.Value = string.Empty;
                        footerDet.Cantidad.Value = 1;
                        footerDet.CantidadTarea.Value = 1;
                        footerDet.CostoLocalCLI.Value = 0;
                        footerDet.CostoTotalUnitML.Value = 0;
                        footerDet.CostoTotalML.Value = 0;
                        footerDet.CostoAdicionalInd.Value = true;
                    }
                    this._listTareasAdic.Add(footerDet);
                    this._rowTareaCurrent = footerDet;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
                this.gcDocument.DataSource = this._listTareasAdic;
                this.gcDocument.RefreshDataSource();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                GridColumn tareaCliente = new GridColumn();
                tareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
                tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
                tareaCliente.UnboundType = UnboundColumnType.String;
                tareaCliente.VisibleIndex = 1;
                tareaCliente.Width = 90;
                tareaCliente.Visible = true;
                tareaCliente.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(tareaCliente);

                GridColumn trabajoID = new GridColumn();
                trabajoID.FieldName = this.unboundPrefix + "TareaID";
                trabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
                trabajoID.UnboundType = UnboundColumnType.String;
                trabajoID.VisibleIndex = 2;
                trabajoID.Width = 50;
                trabajoID.Visible = false;
                trabajoID.ColumnEdit = this.editBtnGrid;
                trabajoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(trabajoID);

                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 3;
                descriptivo.Width = 250;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(descriptivo);

                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.VisibleIndex = 4;
                UnidadInvID.Width = 35;
                UnidadInvID.Visible = true;
                UnidadInvID.ColumnEdit = this.editBtnGrid;
                UnidadInvID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(UnidadInvID);

                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "Cantidad";
                Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
                Cantidad.UnboundType = UnboundColumnType.String;
                Cantidad.VisibleIndex = 5;
                Cantidad.Width = 35;
                Cantidad.Visible = true;
                Cantidad.ColumnEdit = this.editCant2;
                Cantidad.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(Cantidad);

                GridColumn CostoTotalUnitML = new GridColumn();
                CostoTotalUnitML.FieldName = this.unboundPrefix + "CostoTotalUnitML";
                CostoTotalUnitML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOTUnit");
                CostoTotalUnitML.UnboundType = UnboundColumnType.Decimal;
                CostoTotalUnitML.VisibleIndex = 6;
                CostoTotalUnitML.Width = 60;
                CostoTotalUnitML.Visible = true;
                CostoTotalUnitML.ColumnEdit = this.editSpin;
                CostoTotalUnitML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CostoTotalUnitML);

                GridColumn CostoTotalML = new GridColumn();
                CostoTotalML.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
                CostoTotalML.UnboundType = UnboundColumnType.Decimal;
                CostoTotalML.VisibleIndex = 7;
                CostoTotalML.Width = 80;
                CostoTotalML.Visible = true;
                CostoTotalML.ColumnEdit = this.editSpin;
                CostoTotalML.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CostoTotalML);

                GridColumn CostoLocalUnitCLI = new GridColumn();
                CostoLocalUnitCLI.FieldName = this.unboundPrefix + "CostoLocalUnitCLI";
                CostoLocalUnitCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalUnitCLI");
                CostoLocalUnitCLI.UnboundType = UnboundColumnType.Decimal;
                CostoLocalUnitCLI.VisibleIndex = 8;
                CostoLocalUnitCLI.Width = 50;
                CostoLocalUnitCLI.Visible = true;
                CostoLocalUnitCLI.ColumnEdit = this.editSpin;
                CostoLocalUnitCLI.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CostoLocalUnitCLI);

                GridColumn CostoLocalCLI = new GridColumn();
                CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
                CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalCLI");
                CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
                CostoLocalCLI.VisibleIndex = 9;
                CostoLocalCLI.Width = 80;
                CostoLocalCLI.Visible = true;
                CostoLocalCLI.ColumnEdit = this.editSpin;
                CostoLocalCLI.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CostoLocalCLI);

                GridColumn CostoDiferencia = new GridColumn();
                CostoDiferencia.FieldName = this.unboundPrefix + "CostoDiferenciaML";
                CostoDiferencia.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoDiferencia");
                CostoDiferencia.UnboundType = UnboundColumnType.Decimal;
                CostoDiferencia.VisibleIndex = 10;
                CostoDiferencia.Width = 80;
                CostoDiferencia.Visible = false;
                CostoDiferencia.ColumnEdit = this.editSpin;
                CostoDiferencia.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CostoDiferencia);

                GridColumn ImprimirTareaInd = new GridColumn();
                ImprimirTareaInd.FieldName = this.unboundPrefix + "ImprimirTareaInd";
                ImprimirTareaInd.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_ImprimirTareaInd");
                ImprimirTareaInd.UnboundType = UnboundColumnType.Boolean;
                ImprimirTareaInd.VisibleIndex = 11;
                ImprimirTareaInd.Width = 30;
                ImprimirTareaInd.Visible = true;
                ImprimirTareaInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ImprimirTareaInd);

                GridColumn Nivel = new GridColumn();
                Nivel.FieldName = this.unboundPrefix + "Nivel";
                Nivel.UnboundType = UnboundColumnType.Integer;
                Nivel.VisibleIndex = 14;
                Nivel.Width = 50;
                Nivel.Visible = false;
                Nivel.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Nivel);

                GridColumn TareaPadre = new GridColumn();
                TareaPadre.FieldName = this.unboundPrefix + "TareaPadre";
                TareaPadre.UnboundType = UnboundColumnType.String;
                TareaPadre.VisibleIndex = 15;
                TareaPadre.Width = 50;
                TareaPadre.Visible = false;
                TareaPadre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TareaPadre);

                GridColumn DetalleInd = new GridColumn();
                DetalleInd.FieldName = this.unboundPrefix + "DetalleInd";
                DetalleInd.UnboundType = UnboundColumnType.Boolean;
                DetalleInd.VisibleIndex = 16;
                DetalleInd.Width = 50;
                DetalleInd.Visible = false;
                DetalleInd.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DetalleInd);

                this.gvDocument.OptionsBehavior.Editable = true;
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional", "AddGridCols"));
            }
        }

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls(List<DTO_pyPreProyectoTarea> tareas)
        {
            #region Paginador
            //this._bc.Pagging_Init(this.pgGrid, this._pageSize);
            //this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
            //this.pgGrid.UpdatePageNumber(this._listRecursosDet.Count, true, true, false);
            //this.toolTipGrid.SetToolTip(this.gcDocument, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ToolTipGrid)); 
            #endregion

            try
            {                
                FormProvider.LoadResources(this, this._documentID);

                this.tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this._listTareasAdic = tareas;
                this.LoadData();
                
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "InitControls")); 
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalTareasProyecto;
            this._listTareasAdic = new List<DTO_pyPreProyectoTarea>();
            this.AddGridCols();           
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {

                this.gcDocument.DataSource = this._listTareasAdic;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "LoadData"));
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
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0)
                {    
                    #region Descriptivo
                    validField = this._bc.ValidGridCell(this.gvDocument, this.unboundPrefix, fila, "Descriptivo", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion                    

                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "ValidateRow"));
            }        
            return validRow;
        }

        /// <summary>
        /// Muestra el formulario modal para MAsterComplex
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKComplex(int row, string col, ButtonEdit be)
        {
            try
            {
                #region Tabla que corresponde a FK
                DTO_aplMaestraPropiedades propsFK = _bc.GetMasterPropertyByColId(col);
                int docFK = propsFK.DocumentoID;
                string modEmpGrupoFK = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(docFK));
                Tuple<int, string> tupFK = new Tuple<int, string>(Convert.ToInt32(docFK), modEmpGrupoFK);
                DTO_glTabla fktableFK = _bc.AdministrationModel.Tables[tupFK];
                bool jerarquicaFKInd = fktableFK.Jerarquica.Value.Value;
                #endregion

                string countMethod = "MasterComplex_Count";
                string dataMethod = "MasterComplex_GetPaged";

                string modFrmCode = docFK.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));

                //Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);
                //DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];

                ModalComplex modal = new ModalComplex(be, modFrmCode, countMethod, dataMethod, null, col, string.Empty, docFK, jerarquicaFKInd, this._filtrosRef);
                modal.ShowDialog();
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
                //this._totalPresupuesto = 0;
                //this._totalCliente = 0;
                //this._totalRecxTarea = 0;
                //foreach (DTO_pyPreProyectoTarea tarea in this._listTareasAll.FindAll(x => x.DetalleInd.Value.Value))
                //{
                //    this._totalPresupuesto += tarea.CostoTotalML.Value.Value;
                //    this._totalCliente += tarea.CostoLocalCLI.Value.Value;
                //}
                //this.txtCostoPresupuesto.EditValue = this._totalPresupuesto;
                //this.txtCostoCliente.EditValue = this._totalCliente;
                //decimal vlrUtilidad = Math.Round((this._totalPresupuesto * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
                //decimal vlrTotalBaseIVA = vlrUtilidad + Convert.ToDecimal(this.txtOtros.EditValue, CultureInfo.InvariantCulture);
                //this.txtIVA.EditValue = Math.Round((vlrTotalBaseIVA * Convert.ToDecimal(this.txtPorIVA.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gvDetalle_FocusedRowChanged"));
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
        /// se ejecuta al cambiar de foco de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {              
                this._rowTareaCurrent = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gvDocument_FocusedRowChanged"));
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
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                #region Agregar

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDocument.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                        {
                            this.AddNewRowTarea();
                        }
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                            if (isV)
                            {
                                this.AddNewRowTarea();
                            }
                        }
                    }
                }
                #endregion

                #region Eliminar
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    e.Handled = true;
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvDocument.FocusedRowHandle;

                        if (this._listTareasAdic.Count > 0)
                        {                           
                                this._listTareasAdic.RemoveAll(x => x.TareaCliente.Value == this._rowTareaCurrent.TareaCliente.Value &&
                                                                x.Descriptivo.Value == this._rowTareaCurrent.Descriptivo.Value);

                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvDocument.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvDocument.FocusedRowHandle = rowHandle - 1;

                            this.UpdateValues();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {            
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
                if (fieldName == "TareaID")
                {
                    DTO_pyTarea tareaRef = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, e.Value.ToString(), true);
                    if (tareaRef != null)
                    {
                        this._rowTareaCurrent.Descriptivo.Value = tareaRef.Descriptivo.Value;
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("TareaID", tareaRef.ID.Value);
                        DTO_pyTareaClase dtoTrab = (DTO_pyTareaClase)this._bc.GetMasterComplexDTO(AppMasters.pyTareaClase, pks, true);
                        if (dtoTrab != null)
                            this._rowTareaCurrent.TareaID.Value = dtoTrab.TareaID.Value;
                        else
                            this._rowTareaCurrent.TareaID.Value = string.Empty;
                    }
                    else
                        this._rowTareaCurrent.TareaID.Value = string.Empty;
                   
                }
                if (fieldName == "Cantidad")
                {
                    this._rowTareaCurrent.Cantidad.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.UpdateValues();
                }
                else if (fieldName == "CostoTotalML")
                {
                    this._rowTareaCurrent.CostoTotalML.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalCLI")
                {
                    this._rowTareaCurrent.CostoLocalCLI.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    this.UpdateValues();
                }
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gvDocument_CellValueChanging"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e) 
        {
            try
            {
                if(!this.deleteOP)
                    this.ValidateRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "gvDocument_BeforeLeaveRow"));
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
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                if (colName != "CapituloTrabajoID")
                    this.ShowFKModal(this.gvDocument.FocusedRowHandle,colName, origin);
                else
                    this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
                    //this.ShowFKComplex(this.gvDocument.FocusedRowHandle, colName, origin);               

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "editBtnGrid_ButtonClick"));
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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "editChek_CheckedChanged"));
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
            //this.pgGrid.UpdatePageNumber(this._listTareaDet.Count, false, false, false);
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
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "chkSelectAll_CheckedChanged"));
            }
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            if(this.ValidateRow(this.gvDocument.FocusedRowHandle))
              this.Close();
        }

        /// <summary>
        /// Cuando selecciona un item del radioGroup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnCapitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "rbtnTipoRecurso_SelectedIndexChanged"));
            }
        }

        /// <summary>
        /// Cuando cambia el valor del texto
        /// </summary>
        /// <param name="sender">Objeto que envia el evente</param>
        /// <param name="e">Evento</param>
        private void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //TextBox txt = (TextBox)sender;

                //if (txt.Name == "txtTareaCliente")
                //    this._tareaSelected.TareaCliente.Value = txt.Text;
                //else if (txt.Name == "txtDescriptivo")
                //    this._tareaSelected.Descriptivo.Value = txt.Text;
                //this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "txt_TextChanged"));
            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalTareasAdicional.cs", "master_Leave"));
            }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Items seleccionados
        /// </summary>
        public List<DTO_pyPreProyectoTarea> TareasAdic
        {
            get { return this._listTareasAdic; }
        }

        #endregion  

    }
}
