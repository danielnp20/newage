using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class EntregablesProgramacion : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        private int _numeroDocProy = 0;
        private bool isValid = true;
        private bool deleteOP = false;
        //Variables de datos
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_pyProyectoPlanEntrega _rowEntrega = new DTO_pyProyectoPlanEntrega();
        private DTO_pyProyectoTareaCliente _rowTareaClienteCurrent = null;
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdic = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTareaCliente> _listTareasClienteEntr = new List<DTO_pyProyectoTareaCliente>();
        private List<int> _entregasDeleted = new List<int>();
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public EntregablesProgramacion()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "EntregablesProgramacion"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Header
            GridColumn TareaEntregable = new GridColumn();
            TareaEntregable.FieldName = this.unboundPrefix + "TareaEntregable";
            TareaEntregable.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_TareaEntregable");
            TareaEntregable.UnboundType = UnboundColumnType.String;
            TareaEntregable.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            TareaEntregable.AppearanceCell.Options.UseTextOptions = true;
            TareaEntregable.VisibleIndex = 1;
            TareaEntregable.Width = 30;
            TareaEntregable.Visible = true;
            TareaEntregable.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(TareaEntregable);

            GridColumn Descripcion = new GridColumn();
            Descripcion.FieldName = this.unboundPrefix + "Descripcion";
            Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descripcion.UnboundType = UnboundColumnType.String;
            Descripcion.VisibleIndex = 2;
            Descripcion.Width = 230;
            Descripcion.Visible = true;
            Descripcion.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Descripcion);

            GridColumn FechaInicio = new GridColumn();
            FechaInicio.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaInicio");
            FechaInicio.UnboundType = UnboundColumnType.DateTime;
            FechaInicio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaInicio.AppearanceCell.Options.UseTextOptions = true;
            FechaInicio.VisibleIndex = 3;
            FechaInicio.Width = 50;
            FechaInicio.Visible = true;
            FechaInicio.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(FechaInicio);

            GridColumn FechaFinal = new GridColumn();
            FechaFinal.FieldName = this.unboundPrefix + "FechaFinal";
            FechaFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFinal");
            FechaFinal.UnboundType = UnboundColumnType.DateTime;
            FechaFinal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFinal.AppearanceCell.Options.UseTextOptions = true;
            FechaFinal.VisibleIndex = 4;
            FechaFinal.Width = 50;
            FechaFinal.Visible = true;
            FechaFinal.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(FechaFinal);

            GridColumn MonedaFactura = new GridColumn();
            MonedaFactura.FieldName = this.unboundPrefix + "MonedaFactura";
            MonedaFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaFactura");
            MonedaFactura.UnboundType = UnboundColumnType.String;
            MonedaFactura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            MonedaFactura.AppearanceCell.Options.UseTextOptions = true;
            MonedaFactura.VisibleIndex = 5;
            MonedaFactura.Width = 50;
            MonedaFactura.Visible = true;
            MonedaFactura.ColumnEdit = this.editBtnGrid;
            MonedaFactura.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(MonedaFactura);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 6;
            Cantidad.Width = 50;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Cantidad);

            GridColumn ValorAEntregar = new GridColumn();
            ValorAEntregar.FieldName = this.unboundPrefix + "ValorAEntregar";
            ValorAEntregar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAEntregar");
            ValorAEntregar.UnboundType = UnboundColumnType.String;
            ValorAEntregar.AppearanceCell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorAEntregar.AppearanceCell.Options.UseTextOptions = true;
            ValorAEntregar.AppearanceCell.Options.UseFont = true;
            ValorAEntregar.VisibleIndex = 7;
            ValorAEntregar.Width = 80;
            ValorAEntregar.Visible = true;
            ValorAEntregar.ColumnEdit = this.editValue2;
            ValorAEntregar.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(ValorAEntregar);

            GridColumn ValorFactura = new GridColumn();
            ValorFactura.FieldName = this.unboundPrefix + "ValorFactura";
            ValorFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrTotalFactura");
            ValorFactura.UnboundType = UnboundColumnType.String;
            ValorFactura.AppearanceCell.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorFactura.AppearanceCell.Options.UseTextOptions = true;
            ValorFactura.AppearanceCell.Options.UseFont = true;
            ValorFactura.VisibleIndex = 8;
            ValorFactura.Width = 80;
            ValorFactura.Visible = true;
            ValorFactura.ColumnEdit = this.editValue2;
            ValorFactura.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(ValorFactura);

            GridColumn Observaciones = new GridColumn();
            Observaciones.FieldName = this.unboundPrefix + "Observaciones";
            Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observaciones");
            Observaciones.UnboundType = UnboundColumnType.String;
            Observaciones.VisibleIndex = 9;
            Observaciones.Width = 200;
            Observaciones.Visible = true;
            Observaciones.ColumnEdit = this.editPoPup;
            Observaciones.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(Observaciones);

            this.gvHeader.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Grilla Detalle

            GridColumn Index = new GridColumn();
            Index.FieldName = this.unboundPrefix + "Index";
            Index.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Index");
            Index.UnboundType = UnboundColumnType.Integer;
            Index.VisibleIndex = 0;
            Index.Width = 18;
            Index.Visible = true;
            Index.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Index);

            GridColumn FechaEntrega = new GridColumn();
            FechaEntrega.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
            FechaEntrega.UnboundType = UnboundColumnType.DateTime;
            FechaEntrega.VisibleIndex = 1;
            FechaEntrega.Width = 50;
            FechaEntrega.Visible = true;
            FechaEntrega.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(FechaEntrega);

            GridColumn PorEntrega = new GridColumn();
            PorEntrega.FieldName = this.unboundPrefix + "PorEntrega";
            PorEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorEntrega");
            PorEntrega.UnboundType = UnboundColumnType.Decimal;
            PorEntrega.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PorEntrega.AppearanceCell.Options.UseTextOptions = true;
            PorEntrega.VisibleIndex = 2;
            PorEntrega.Width = 45;
            PorEntrega.Visible = true;
            PorEntrega.ColumnEdit = this.editPorc;
            PorEntrega.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(PorEntrega);

            GridColumn CantidadEntrega = new GridColumn();
            CantidadEntrega.FieldName = this.unboundPrefix + "Cantidad";
            CantidadEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            CantidadEntrega.UnboundType = UnboundColumnType.String;
            CantidadEntrega.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadEntrega.AppearanceCell.Options.UseTextOptions = true;
            CantidadEntrega.VisibleIndex = 3;
            CantidadEntrega.Width = 50;
            CantidadEntrega.Visible = true;
            CantidadEntrega.ColumnEdit = this.editValue2Cant;
            CantidadEntrega.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(CantidadEntrega);

            GridColumn TipoEntrega = new GridColumn();
            TipoEntrega.FieldName = this.unboundPrefix + "TipoEntrega";
            TipoEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoEntrega");
            TipoEntrega.UnboundType = UnboundColumnType.String;
            TipoEntrega.VisibleIndex = 4;
            TipoEntrega.Width = 35;
            TipoEntrega.Visible = true;
            TipoEntrega.ColumnEdit = this.editCmb;
            TipoEntrega.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(TipoEntrega);

            GridColumn FacturaInd = new GridColumn();
            FacturaInd.FieldName = this.unboundPrefix + "FacturaInd";
            FacturaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FacturaInd");
            FacturaInd.UnboundType = UnboundColumnType.Boolean;
            FacturaInd.VisibleIndex = 5;
            FacturaInd.Width = 30;
            FacturaInd.Visible = true;
            FacturaInd.ColumnEdit = this.editCheck;
            FacturaInd.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(FacturaInd);

            GridColumn ValorFacturaDet = new GridColumn();
            ValorFacturaDet.FieldName = this.unboundPrefix + "ValorFactura";
            ValorFacturaDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorFactura");
            ValorFacturaDet.UnboundType = UnboundColumnType.Decimal;
            ValorFacturaDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorFacturaDet.AppearanceCell.Options.UseTextOptions = true;
            ValorFacturaDet.VisibleIndex = 6;
            ValorFacturaDet.Width = 55;
            ValorFacturaDet.Visible = true;
            ValorFacturaDet.ColumnEdit = this.editValue2;
            ValorFacturaDet.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(ValorFacturaDet);

            GridColumn ObservacionesDet = new GridColumn();
            ObservacionesDet.FieldName = this.unboundPrefix + "Observaciones";
            ObservacionesDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observaciones");
            ObservacionesDet.UnboundType = UnboundColumnType.String;
            ObservacionesDet.VisibleIndex = 7;
            ObservacionesDet.Width = 160;
            ObservacionesDet.ColumnEdit = this.editPoPup;
            ObservacionesDet.Visible = true;
            ObservacionesDet.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(ObservacionesDet);

            #endregion

            #region Grilla Tareas          

            GridColumn IndexTarea = new GridColumn();
            IndexTarea.FieldName = this.unboundPrefix + "Index";
            IndexTarea.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Index");
            IndexTarea.UnboundType = UnboundColumnType.Integer;
            IndexTarea.VisibleIndex = 0;
            IndexTarea.Width = 15;
            IndexTarea.Visible = false;
            IndexTarea.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(IndexTarea);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 28;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaID);

            GridColumn Descriptivo = new GridColumn();
            Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descriptivo.UnboundType = UnboundColumnType.String;
            Descriptivo.VisibleIndex = 2;
            Descriptivo.Width = 70;
            Descriptivo.Visible = true;
            Descriptivo.OptionsColumn.AllowEdit = true;
            this.gvTareas.Columns.Add(Descriptivo);
            #endregion
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowEntrega()
        {
            try
            {
                if (this._rowTareaClienteCurrent != null)
                {
                    DTO_pyProyectoPlanEntrega footerDet = new DTO_pyProyectoPlanEntrega();
                    footerDet.Index = this._rowTareaClienteCurrent.Detalle.Count > 0? this._rowTareaClienteCurrent.Detalle.Last().Index +1 : 1;
                    footerDet.FacturaInd.Value = true;
                    footerDet.PorEntrega.Value = 0;
                    footerDet.ValorFactura.Value = 0;
                    footerDet.TareaEntregable.Value = this._rowTareaClienteCurrent.TareaEntregable.Value;
                    this._rowTareaClienteCurrent.Detalle.Add(footerDet);

                    this.gcDetalle.DataSource = this._rowTareaClienteCurrent.Detalle;
                    this.gcDetalle.RefreshDataSource();
                    this.gvDetalle.FocusedRowHandle = this.gvDetalle.DataRowCount - 1;   

                    if (this._rowTareaClienteCurrent.Detalle.Count > 0)
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this.ucProyecto.Init(false, false, false, false);
                this.ucProyecto.LoadProyectoInfo_Leave += new UC_Proyecto.EventHandler(this.ucProyecto_LoadProyectoInfo_Click);            
                    
                Dictionary<string, string> datosTipoEntrega = new Dictionary<string, string>();
                datosTipoEntrega.Add("1", "Final");
                datosTipoEntrega.Add("2","Parcial");

                this.editCmb.ValueMember = "Key";
                this.editCmb.DisplayMember = "Value";
                this.editCmb.DataSource = datosTipoEntrega;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcHeader.DataSource = this._listTareasClienteEntr;
                this.gcHeader.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {
            this.ucProyecto.CleanControl();   
            this._ctrlProyecto = null;
            this._numeroDocProy = 0;
            this._rowEntrega = new DTO_pyProyectoPlanEntrega();
            this._rowTareaClienteCurrent = null;
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasClienteEntr = new List<DTO_pyProyectoTareaCliente>();          

            this.gcDetalle.DataSource = null;
            this.gcDetalle.RefreshDataSource();
            this.gcTareas.DataSource = null;
            this.gcTareas.RefreshDataSource();
            this.gcHeader.DataSource = null;
            this.gcHeader.RefreshDataSource();
            this._entregasDeleted = new List<int>();
            this.isValid = true;
            this.ucProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.ProgramacionEntregables;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
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
        private bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0 && this._rowEntrega != null)
                {
                    #region FechaEntrega
                    int count = this._rowTareaClienteCurrent.Detalle.Count(x => x.FechaEntrega.Value == this._rowEntrega.FechaEntrega.Value);
                    if (count > 1)
                    {
                        GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + "FechaEntrega"];
                        string colVal = this.gvDetalle.GetRowCellValue(fila, col).ToString();
                        this.gvDetalle.SetColumnError(col, string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_AlreadyExistInGrid), this._rowEntrega.FechaEntrega.Value));
                        validRow = false;
                    }
                    #endregion                    
                    #region PorEntrega
                    validField = this._bc.ValidGridCellValue(this.gvDetalle, this.unboundPrefix, fila, "PorEntrega", false, true, true, false);
                    if (!validField)
                        validRow = false;
                    else
                    {
                        //decimal porc = this._rowTareaClienteCurrent.Detalle.Sum(x => x.PorEntrega.Value.Value);
                        //if(porc > 100)
                        //{
                        //    GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + "PorEntrega"];
                        //    string colVal = this.gvDetalle.GetRowCellValue(fila, col).ToString();
                        //    this.gvDetalle.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_PorcentajeEntregaInvalid));
                        //    validRow = false;
                        //}
                    }
                    #endregion
                    #region TipoEntrega
                    validField = this._bc.ValidGridCellValue(this.gvDetalle, this.unboundPrefix, fila, "TipoEntrega", false,false, true, false);
                    if (!validField)
                        validRow = false;
                    #endregion
                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
                else
                    this.isValid = true;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "ValidateRow"));
            }
            return validRow;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;               
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_Enter(object sender, EventArgs e) { }
       
        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucProyecto_LoadProyectoInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ucProyecto.ProyectoInfo != null)
                {
                    if (this.ucProyecto.ProyectoInfo.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }
                    this._numeroDocProy = this.ucProyecto.ProyectoInfo.DocCtrl.NumeroDoc.Value.Value;
                    this._listTareasAll = this.ucProyecto.ProyectoInfo.DetalleProyecto;
                    this._listTareasAdic = this.ucProyecto.ProyectoInfo.DetalleProyectoTareaAdic;
                    this._ctrlProyecto = this.ucProyecto.ProyectoInfo.DocCtrl;

                    this._listTareasClienteEntr = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);

                    foreach (var t in this._listTareasClienteEntr) //Recorre los entregables
                    {                      
                        foreach (var plan in t.Detalle) //Recorre el plan de entregas
                        {
                            foreach (var tar in  t.DetalleTareas) //Recorre las tareas del entregable
	                        {
                                tar.Index = plan.Index;
                                if(tar.ConsEntrega.Value == plan.Consecutivo.Value)
                                    plan.DetalleTareas.Add(tar);
	                        }                            
                        }
                        t.ValorAEntregar.Value = t.DetalleTareas.Sum(x => x.CostoLocalCLI.Value);                       
                    }                       

                    this.LoadGrids();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "ucProyecto_LoadProyectoInfo_Click"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadFechasProgr_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DTO_pyProyectoTareaCliente entr in this._listTareasClienteEntr)
                {
                    DTO_pyProyectoPlanEntrega planEntr = new DTO_pyProyectoPlanEntrega();
                    planEntr.Index = 1;
                    planEntr.FacturaInd.Value = true;
                    planEntr.PorEntrega.Value = 100;
                    planEntr.TipoEntrega.Value = 1;
                    planEntr.FechaEntrega.Value = entr.DetalleTareas.Count > 0 ? entr.DetalleTareas.Max().FechaFin.Value : DateTime.Now.Date;
                    planEntr.FechaEntrega.Value = !planEntr.FechaEntrega.Value.HasValue ? entr.FechaFinal.Value : planEntr.FechaEntrega.Value;
                    planEntr.FechaEntrega.Value = entr.Detalle.Count == 0 && !planEntr.FechaEntrega.Value.HasValue? DateTime.Now.Date : planEntr.FechaEntrega.Value;
                    planEntr.ValorFactura.Value = entr.ValorAEntregar.Value;
                    planEntr.TareaEntregable.Value = entr.TareaEntregable.Value;
                    planEntr.Cantidad.Value = entr.DetalleTareas.Sum(x=>x.Cantidad.Value);
                    planEntr.ConsecTarea.Value = entr.Consecutivo.Value;
                    if (entr.Detalle.Count == 0)
                        entr.Detalle.Add(planEntr);
                    else if (entr.Detalle.Count == 1)
                    {
                        planEntr.Consecutivo.Value = entr.Detalle[0].Consecutivo.Value;
                        entr.ValorFactura.Value = planEntr.ValorFactura.Value;
                        entr.Detalle = new List<DTO_pyProyectoPlanEntrega>();
                        entr.Detalle.Add(planEntr);
                    }
                    else if (entr.Detalle.Count > 1)
                    {
                        foreach (DTO_pyProyectoPlanEntrega det in entr.Detalle.FindAll(x=>!x.FechaEntrega.Value.HasValue))
                        {
                            det.FechaEntrega.Value = entr.DetalleTareas.Count > 0 ? entr.DetalleTareas.Max().FechaFin.Value : DateTime.Now.Date;
                            det.FechaEntrega.Value = !det.FechaEntrega.Value.HasValue ? entr.FechaFinal.Value : det.FechaEntrega.Value;
                            det.FechaEntrega.Value = !det.FechaEntrega.Value.HasValue ? DateTime.Now.Date : det.FechaEntrega.Value;
                            //if (entr.Detalle.Any(x => x.FechaEntrega.Value == planEntr.FechaEntrega.Value))
                            //    planEntr.Consecutivo.Value = entr.Detalle.Find(x => x.FechaEntrega.Value == planEntr.FechaEntrega.Value).Consecutivo.Value;                
                        }
                    }
                }
                this.gvHeader.RefreshData();
                this.gcDetalle.DataSource = this._rowTareaClienteCurrent.Detalle;
                this.gcDetalle.RefreshDataSource();
                this.gvDetalle.FocusedRowHandle = this.gvDetalle.DataRowCount - 1;
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgrm", "btnLoadFechasProgr_Click"));
            }
        }
        #endregion

        #region Eventos Grilla

        #region Tareas

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowTareaClienteCurrent = (DTO_pyProyectoTareaCliente)this.gvHeader.GetRow(e.FocusedRowHandle);
                    this.gcDetalle.DataSource = null;
                    this.gcDetalle.DataSource = this._rowTareaClienteCurrent.Detalle;
                    this.gcDetalle.RefreshDataSource();
                    this.gcTareas.DataSource = this._rowTareaClienteCurrent.DetalleTareas;
                    this.gcTareas.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
            {
                Object dto = (Object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                    if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                        e.Value = 0;
                }
                if (e.IsSetData)
                {

                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (e.Value == null)
                        e.Value = string.Empty;
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            if (!string.IsNullOrEmpty(e.Value.ToString()))
                                udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                            else
                                udtProp.SetValueFromString(e.Value.ToString());
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double")
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
            catch (Exception ex)
            {
                ;
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
                string colName = this.gvHeader.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                if (colName.Equals("MonedaFactura"))
                    colName = "MonedaID";
                this.ShowFKModal(this.gvHeader.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "editBtnGrid_ButtonClick"));
            }
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.deleteOP && this.gvDetalle.DataRowCount > 0 && this.gvDetalle.IsFocusedView)
                    this.ValidateRow(e.RowHandle);

                if (!this.isValid)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvDetalle_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowEntrega = (DTO_pyProyectoPlanEntrega)this.gvDetalle.GetRow(e.FocusedRowHandle);
                    if (this._rowTareaClienteCurrent != null)
                        this.gcTareas.DataSource = this._rowTareaClienteCurrent.DetalleTareas;
                    this.gcTareas.RefreshDataSource();
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDetalle_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                #region Agregar

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvDetalle.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                            this.AddNewRowEntrega();
                        else
                        {
                            bool isV = this.ValidateRow(this.gvDetalle.FocusedRowHandle);
                            if (isV)
                                this.AddNewRowEntrega();
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
                        int rowHandle = this.gvDetalle.FocusedRowHandle;

                        this._rowTareaClienteCurrent.Detalle.RemoveAll(x => x.FechaEntrega.Value == this._rowEntrega.FechaEntrega.Value);

                        //Asigna las tareas a eliminar de bd
                        if (this._rowEntrega.Consecutivo.Value != null && !this._entregasDeleted.Exists(x => x == this._rowEntrega.Consecutivo.Value.Value))
                            this._entregasDeleted.Add(this._rowEntrega.Consecutivo.Value.Value);

                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvDetalle.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvDetalle.FocusedRowHandle = rowHandle - 1;
                        this.gcDetalle.RefreshDataSource();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];
            this._rowEntrega = (DTO_pyProyectoPlanEntrega)this.gvDetalle.GetRow(e.RowHandle);
            try
            {
                if (fieldName == "ValorFactura")
                {
                    this._rowTareaClienteCurrent.ValorFactura.Value = this._rowTareaClienteCurrent.Detalle.Sum(x => x.ValorFactura.Value);
                    this.gvHeader.RefreshData();
                }
                else if (fieldName == "PorEntrega")
                {
                    this._rowEntrega.Cantidad.Value = (((decimal)e.Value) * this._rowTareaClienteCurrent.Cantidad.Value.Value)/100;
                    this._rowEntrega.ValorFactura.Value = this._rowTareaClienteCurrent.ValorAEntregar.Value * ((decimal)e.Value/100);
                    this._rowTareaClienteCurrent.ValorFactura.Value = this._rowTareaClienteCurrent.Detalle.Sum(x => x.ValorFactura.Value);
                    this.gvHeader.RefreshData();
                }
                else if (fieldName == "Cantidad")
                {
                    if (this._rowTareaClienteCurrent.Cantidad.Value.HasValue && this._rowTareaClienteCurrent.Cantidad.Value != 0)
                    {
                        this._rowEntrega.PorEntrega.Value = (100 * (decimal)e.Value) / this._rowTareaClienteCurrent.Cantidad.Value.Value;
                        this._rowEntrega.ValorFactura.Value = this._rowTareaClienteCurrent.ValorAEntregar.Value * Math.Round((this._rowEntrega.PorEntrega.Value.Value / 100),5);
                        this._rowTareaClienteCurrent.ValorFactura.Value = this._rowTareaClienteCurrent.Detalle.Sum(x => x.ValorFactura.Value);
                        this.gvHeader.RefreshData();
                    }
                    else
                        this._rowEntrega.Cantidad.Value = 0;
                }
                else if (fieldName == "FechaEntrega")
                {
                    this._rowTareaClienteCurrent.FechaFinal.Value = this._rowTareaClienteCurrent.Detalle.OrderBy(x => x.FechaEntrega.Value).Last().FechaEntrega.Value;
                    this.gvHeader.RefreshData();
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvDetalle_CellValueChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDetalle_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._rowTareaClienteCurrent != null)
                {
                    decimal porc = this._rowTareaClienteCurrent.Detalle.Sum(x => x.PorEntrega.Value.Value);
                    if (this._rowTareaClienteCurrent.Detalle.Count > 0 && porc != 100)// 100%
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_PorcentajeEntregaInvalid));
                        this.isValid = false;
                    } 
                }
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gcDetalle_Leave"));
            }
        }

        #endregion        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTareas_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvTareas.Columns[this.unboundPrefix + fieldName];

            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvDetalle_CellValueChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTareas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {       
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    var tarea = (DTO_pyProyectoTarea)this.gvTareas.GetRow(e.FocusedRowHandle);
                    //Recorre las tareas para seleccionar las asignadas
                    foreach (DTO_pyProyectoPlanEntrega plan in this._rowTareaClienteCurrent.Detalle)
                    {
                        if ( this._rowEntrega.Index != plan.Index)
                        {
                            //if (plan.DetalleTareas.Exists(x => x.Consecutivo.Value == tarea.Consecutivo.Value))
                            //{
                            //    this.gvTareas.Columns[this.unboundPrefix + "Select"].OptionsColumn.AllowEdit = false;
                            //    break;
                            //}
                            //else
                            //{
                            //    this.gvTareas.Columns[this.unboundPrefix + "Select"].OptionsColumn.AllowEdit = true;
                            //}  
                        }
                    }
                    this.gcTareas.DataSource = this._rowTareaClienteCurrent.DetalleTareas;
                    this.gcTareas.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "gvDetalle_CellValueChanged"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {
            this.gvHeader.PostEditor();

            if (this.ValidateRow(this.gvDetalle.FocusedRowHandle) && this.isValid)
            {
                Thread process = new Thread(this.SaveThread);
                process.Start(); 
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Eliminar
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();

                //Trae las facturas de ventas del proyecto
                filter.DatoAdd4.Value = this._ctrlProyecto.NumeroDoc.Value.ToString();
                filter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                List<DTO_glMovimientoDeta> mvtosFact = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, false);
                mvtosFact = mvtosFact.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado).ToList();

                if (mvtosFact.Count > 0)
                {
                    string facturas = string.Empty;
                    foreach (var fac in mvtosFact)
                        facturas += (!facturas.Contains(fac.Prefijo_Documento.Value) ? fac.Prefijo_Documento.Value + " " : string.Empty);
                    MessageBox.Show("El proyecto actual tiene ya Facturas de Venta aprobadas,si desea eliminar los datos. Facturas: " + facturas);
                    return;
                }

                if (MessageBox.Show("Desea eliminar los entregables del proyecto?", "Eliminar " + this.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool deleteProgramacion = this._listTareasClienteEntr.Any(x => x.Detalle.Count > 0) ? true : false;
                    bool deleteActas = this._listTareasClienteEntr.Any(x => x.DetalleActas.Count > 0) ? true : false;
                    bool anulaPreFacturas = this._listTareasClienteEntr.Any(x => x.DetalleActas.Any(y => y.NumDocFactura.Value.HasValue)) ? true : false;

                    DTO_TxResult res = this._bc.AdministrationModel.EntregasCliente_Delete(this.documentID, this._numeroDocProy, this._listTareasClienteEntr, false, true, deleteActas, anulaPreFacturas);
                    this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this.userID.ToString(), res, true, false);
                    this.RefreshForm();
                }
            }
            catch (Exception ex)
            { ; }

        }

        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                result = this._bc.AdministrationModel.EntregasCliente_Add(this.documentID, this._numeroDocProy, this._listTareasClienteEntr, null,null,_entregasDeleted);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID,this.userID.ToString(), result, true, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EntregablesProgramacion.cs", "SaveThread"));
            }
            finally
            {
                //if (result.Result != ResultValue.NOK)
                //    this.Invoke(this.saveDelegate);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion          
      
    }
}
