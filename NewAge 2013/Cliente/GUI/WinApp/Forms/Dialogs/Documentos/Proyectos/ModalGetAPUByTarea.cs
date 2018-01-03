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
    public partial class ModalGetAPUByTarea : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_pyPreProyectoTarea> _listTareasAll = null;
        private string unboundPrefix = "Unbound_";
        private bool isValid = false;
        private bool deleteOP = false;
        private bool isCancel = false;
        private int _documentID;
        private string tareaXDef = string.Empty;
        private DTO_pyPreProyectoTarea _rowTareaCurrent = new DTO_pyPreProyectoTarea();
        private List<DTO_pyPreProyectoTarea> _listTareas = new List<DTO_pyPreProyectoTarea>();
        private List<DTO_glDocumentoControl> _listDocuments = null;
        private List<DTO_pyPreProyectoDocu> _listPreproyectos = new List<DTO_pyPreProyectoDocu>();
        private DTO_glDocumentoControl _docCtrl = null;
        private DTO_pyPreProyectoDocu _docuCurrent = null;
        private List<DTO_glConsultaFiltro> _filtrosRef;


        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalGetAPUByTarea()
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();               
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "ModalGetAPUByTarea")); 
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
                #region Grilla Document
                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 1;
                DocumentoPrefijoNro.Width = 30;
                DocumentoPrefijoNro.Visible = true;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn ClaseServicioID = new GridColumn();
                ClaseServicioID.FieldName = this.unboundPrefix + "ClaseServicioID";
                ClaseServicioID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClaseServicioID");
                ClaseServicioID.UnboundType = UnboundColumnType.String;
                ClaseServicioID.VisibleIndex = 2;
                ClaseServicioID.Width = 60;
                ClaseServicioID.Visible = true;
                ClaseServicioID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ClaseServicioID);

                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this.unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 3;
                ClienteID.Width = 60;
                ClienteID.Visible = false;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ClienteID);

                GridColumn EmpresaNombre = new GridColumn();
                EmpresaNombre.FieldName = this.unboundPrefix + "EmpresaNombre";
                EmpresaNombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EmpresaNombre");
                EmpresaNombre.UnboundType = UnboundColumnType.String;
                EmpresaNombre.VisibleIndex = 4;
                EmpresaNombre.Width = 80;
                EmpresaNombre.Visible = true;
                EmpresaNombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(EmpresaNombre);

                GridColumn Licitacion = new GridColumn();
                Licitacion.FieldName = this.unboundPrefix + "Licitacion";
                Licitacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Licitacion");
                Licitacion.UnboundType = UnboundColumnType.String;
                Licitacion.VisibleIndex = 5;
                Licitacion.Width = 70;
                Licitacion.Visible = true;
                Licitacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Licitacion);

                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "DescripcionSOL";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DescripcionSOL");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 6;
                Descripcion.Width = 130;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 7;
                Valor.Width = 70;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                Valor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(Valor);

                //GridColumn file = new GridColumn();
                //file.FieldName = this.unboundPrefix + "FileUrl";
                //file.OptionsColumn.ShowCaption = false;
                //file.UnboundType = UnboundColumnType.String;
                //file.Width = 60;
                //file.VisibleIndex = 6;
                //file.Visible = true;
                //file.ColumnEdit = this.editLink;
                //this.gvDocument.Columns.Add(file);

                this.gvDocument.OptionsBehavior.Editable = true;
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                #endregion
                #region Grilla Tareas
                GridColumn tareaCliente = new GridColumn();
                tareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
                tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
                tareaCliente.UnboundType = UnboundColumnType.String;
                tareaCliente.VisibleIndex = 1;
                tareaCliente.Width = 90;
                tareaCliente.Visible = true;
                tareaCliente.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(tareaCliente);

                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 3;
                descriptivo.Width = 250;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(descriptivo);

                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.VisibleIndex = 4;
                UnidadInvID.Width = 35;
                UnidadInvID.Visible = true;
                UnidadInvID.ColumnEdit = this.editBtnGrid;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(UnidadInvID);

                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "Cantidad";
                Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
                Cantidad.UnboundType = UnboundColumnType.String;
                Cantidad.VisibleIndex = 5;
                Cantidad.Width = 35;
                Cantidad.Visible = true;
                Cantidad.ColumnEdit = this.editCant2;
                Cantidad.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(Cantidad);

                GridColumn CostoTotalUnitML = new GridColumn();
                CostoTotalUnitML.FieldName = this.unboundPrefix + "CostoTotalUnitML";
                CostoTotalUnitML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOTUnit");
                CostoTotalUnitML.UnboundType = UnboundColumnType.Decimal;
                CostoTotalUnitML.VisibleIndex = 6;
                CostoTotalUnitML.Width = 60;
                CostoTotalUnitML.Visible = true;
                CostoTotalUnitML.ColumnEdit = this.editSpin;
                CostoTotalUnitML.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(CostoTotalUnitML);

                GridColumn CostoTotalML = new GridColumn();
                CostoTotalML.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
                CostoTotalML.UnboundType = UnboundColumnType.Decimal;
                CostoTotalML.VisibleIndex = 7;
                CostoTotalML.Width = 80;
                CostoTotalML.Visible = true;
                CostoTotalML.ColumnEdit = this.editSpin;
                CostoTotalML.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(CostoTotalML);

                GridColumn CostoLocalUnitCLI = new GridColumn();
                CostoLocalUnitCLI.FieldName = this.unboundPrefix + "CostoLocalUnitCLI";
                CostoLocalUnitCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalUnitCLI");
                CostoLocalUnitCLI.UnboundType = UnboundColumnType.Decimal;
                CostoLocalUnitCLI.VisibleIndex = 8;
                CostoLocalUnitCLI.Width = 50;
                CostoLocalUnitCLI.Visible = false;
                CostoLocalUnitCLI.ColumnEdit = this.editSpin;
                CostoLocalUnitCLI.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(CostoLocalUnitCLI);

                GridColumn CostoLocalCLI = new GridColumn();
                CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
                CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalCLI");
                CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
                CostoLocalCLI.VisibleIndex = 9;
                CostoLocalCLI.Width = 80;
                CostoLocalCLI.Visible = false;
                CostoLocalCLI.ColumnEdit = this.editSpin;
                CostoLocalCLI.OptionsColumn.AllowEdit = false;
                this.gvTareas.Columns.Add(CostoLocalCLI);

                this.gvTareas.OptionsBehavior.Editable = true;
                this.gvTareas.OptionsView.ColumnAutoWidth = true; 
                #endregion
                #region Grilla Recursos
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 0;
                RecursoID.Width = 50;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = true;
                this.gvDetalle.Columns.Add(RecursoID);

                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 1;
                RecursoDesc.Width = 130;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(RecursoDesc);

                GridColumn UnidadInvIDrec = new GridColumn();
                UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrec.UnboundType = UnboundColumnType.String;
                UnidadInvIDrec.VisibleIndex = 2;
                UnidadInvIDrec.Width = 50;
                UnidadInvIDrec.Visible = true;
                UnidadInvIDrec.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(UnidadInvIDrec);

                //Factor
                GridColumn FactorID = new GridColumn();
                FactorID.FieldName = this.unboundPrefix + "FactorID";
                FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorID.UnboundType = UnboundColumnType.Decimal;
                FactorID.VisibleIndex = 5;
                FactorID.Width = 80;
                FactorID.Visible = true;
                //FactorID.ColumnEdit = this.editValue6Cant;
                FactorID.OptionsColumn.AllowEdit = true;
                this.gvDetalle.Columns.Add(FactorID);     
   
                GridColumn CantidadTOT = new GridColumn();
                CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
                CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTOT");
                CantidadTOT.UnboundType = UnboundColumnType.Decimal;
                CantidadTOT.VisibleIndex = 8;
                CantidadTOT.Width = 80;
                CantidadTOT.Visible = false;
                //CantidadTOT.ColumnEdit = this.editValue2Cant;
                CantidadTOT.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CantidadTOT);

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
                this.gvDetalle.Columns.Add(CostoLocal);

                GridColumn CostoLocalTOT = new GridColumn();
                CostoLocalTOT.FieldName = this.unboundPrefix + "CostoLocalTOT";
                CostoLocalTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
                CostoLocalTOT.UnboundType = UnboundColumnType.Decimal;
                CostoLocalTOT.VisibleIndex = 10;
                CostoLocalTOT.Width = 80;
                CostoLocalTOT.Visible = true;
                CostoLocalTOT.ColumnEdit = this.editSpin;
                CostoLocalTOT.OptionsColumn.AllowEdit = false;
                CostoLocalTOT.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                CostoLocalTOT.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
                this.gvDetalle.Columns.Add(CostoLocalTOT);           

                GridColumn TipoRecurso = new GridColumn();
                TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecurso.UnboundType = UnboundColumnType.Integer;
                TipoRecurso.Width = 80;
                TipoRecurso.Visible = false;
                TipoRecurso.Group();
                TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
                this.gvDetalle.Columns.Add(TipoRecurso);

                this.gvDetalle.OptionsBehavior.Editable = true;
                this.gvDetalle.OptionsView.ColumnAutoWidth = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea", "AddGridCols"));
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
                this.tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this.LoadData();

                this.gcDocument.RefreshDataSource();
                this.gcTareas.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "InitControls")); 
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalGetAPUByTarea;
            this._listTareasAll = new List<DTO_pyPreProyectoTarea>();
            this.AddGridCols();           
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private void LoadData()
        {
            try
            {
                //Carga el filtro
                this._docCtrl = new DTO_glDocumentoControl();
                this._docCtrl.DocumentoID.Value = AppDocuments.PreProyecto;                
                //if (!string.IsNullOrEmpty(this.txtNroComprobante.Text))
                //    this._docCtrl.ComprobanteIDNro.Value = Convert.ToInt32(this.txtNroComprobante.Text);
                //this._docCtrl.PeriodoDoc.Value = this.dtPeriodoDoc.DateTime;

                this._listDocuments = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(this._docCtrl);
                this._listDocuments = this._listDocuments.OrderBy(x => x.PrefDoc.Value).ToList();
                //Asigna valores
                foreach (DTO_glDocumentoControl doc in this._listDocuments)
                {
                    DTO_pyPreProyectoDocu docu = this._bc.AdministrationModel.pyPreProyectoDocu_Get(doc.NumeroDoc.Value.Value);
                    docu.PrefDoc.Value = doc.PrefDoc.Value;
                    //docu.DetalleTareas = this._bc.AdministrationModel.pyPreProyectoTarea_Get(doc.NumeroDoc.Value, string.Empty, string.Empty);
                    //docu.DetalleTareas = docu.DetalleTareas.FindAll(x => x.DetalleInd.Value.Value).ToList();
                    docu.Valor.Value = doc.Valor.Value;
                    this._listPreproyectos.Add(docu);
                }
                this._listPreproyectos = this._listPreproyectos.OrderBy(x => x.PrefDoc.Value).ToList();
                this.gcDocument.DataSource = this._listPreproyectos;
                if (this._listPreproyectos.Count > 0)
                {
                    this._docuCurrent = this._listPreproyectos[0];
                    this._docuCurrent.DetalleTareas = this._docuCurrent.DetalleTareas.OrderBy(x => x.TareaCliente.Value).ThenBy(y => y.Index).ToList();
                    this.gcTareas.DataSource = this._docuCurrent.DetalleTareas;
                }                   
                else
                    this._docuCurrent = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "LoadData"));
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

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// se ejecuta al cambiar de foco de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._docuCurrent = (DTO_pyPreProyectoDocu)this.gvDocument.GetRow(e.FocusedRowHandle);

                    if (this._docuCurrent.DetalleTareas.Count == 0)
                    {
                        this._docuCurrent.DetalleTareas = this._bc.AdministrationModel.pyPreProyectoTarea_Get(this._docuCurrent.NumeroDoc.Value, string.Empty, string.Empty);
                        this._docuCurrent.DetalleTareas = this._docuCurrent.DetalleTareas.FindAll(x => x.DetalleInd.Value.Value).ToList();
                        this._docuCurrent.Valor.Value = this._docuCurrent.DetalleTareas.Sum(x => x.CostoTotalML.Value);
                        this._docuCurrent.DetalleTareas = this._docuCurrent.DetalleTareas.OrderBy(x => x.TareaCliente.Value).ThenBy(y=>y.Index).ToList();
                    }
                    this.gcTareas.DataSource = this._docuCurrent.DetalleTareas;
                    this.gcTareas.RefreshDataSource();
                    if (this._docuCurrent.DetalleTareas.Count > 0)
                    {
                        this._rowTareaCurrent = (DTO_pyPreProyectoTarea)this.gvTareas.GetRow(this.gvTareas.FocusedRowHandle);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "gvDocument_FocusedRowChanged"));
            }
        }

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
        private void gvTarea_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {    
                if(e.FocusedRowHandle >= 0)       
                  this._rowTareaCurrent = (DTO_pyPreProyectoTarea)this.gvTareas.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Se realiza al digitar una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcTarea_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.gvTareas.FocusedRowHandle >= 0)
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
        private void gvTarea_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {            
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvTareas.Columns[this.unboundPrefix + fieldName];                
              
                if (fieldName == "CostoLocalCLI")
                {
                    //this._rowTareaCurrent.CostoLocalCLI.Value = e.Value != null ? Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) : 0;
                    //this.UpdateValues();
                }
                this.gvTareas.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
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
                string colName = this.gvTareas.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                if (colName != "CapituloTrabajoID")
                    this.ShowFKModal(this.gvTareas.FocusedRowHandle,colName, origin);
                else
                    this.ShowFKModal(this.gvTareas.FocusedRowHandle, colName, origin);
                    //this.ShowFKComplex(this.gvDocument.FocusedRowHandle, colName, origin);               

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "editBtnGrid_ButtonClick"));
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
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "editChek_CheckedChanged"));
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcTareas_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalGetAPUByTarea.cs", "master_Leave"));
            }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documento Control Seleccionado
        /// </summary>
        public DTO_glDocumentoControl DocumentoControl
        {
            get { return this._docCtrl; }
        }

        /// <summary>
        /// Items seleccionados
        /// </summary>
        public List<DTO_pyPreProyectoTarea> ListSelected
        {
            get { return this._listTareasAll; }
        }

        /// <summary>
        /// Tarea seleccionada
        /// </summary>
        public DTO_pyPreProyectoTarea TareaSelected
        {
            get { return this._rowTareaCurrent; }
        }

        /// <summary>
        /// Indicador de Cancelar
        /// </summary>
        public bool isCancelSelected
        {
            get { return this.isCancel; }
        }

        #endregion  

     

    }
}
