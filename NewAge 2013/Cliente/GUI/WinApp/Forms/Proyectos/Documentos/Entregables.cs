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
    public partial class Entregables : FormWithToolbar
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
        private DTO_pyProyectoTarea _rowTarea = new DTO_pyProyectoTarea();
        private DTO_pyProyectoTareaCliente _rowTareaCliente = new DTO_pyProyectoTareaCliente();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdic = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTareaCliente> _listEntregables = new List<DTO_pyProyectoTareaCliente>();
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public Entregables()
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "Entregables"));
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
            TareaEntregable.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaEntregable.AppearanceCell.Options.UseTextOptions = true;
            TareaEntregable.AppearanceCell.Options.UseFont = true;
            TareaEntregable.VisibleIndex = 1;
            TareaEntregable.Width = 30;
            TareaEntregable.Visible = true;
            TareaEntregable.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(TareaEntregable);

            GridColumn Descripcion = new GridColumn();
            Descripcion.FieldName = this.unboundPrefix + "Descripcion";
            Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descripcion.UnboundType = UnboundColumnType.String;
            Descripcion.VisibleIndex = 2;
            Descripcion.Width = 230;
            Descripcion.Visible = true;
            Descripcion.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(Descripcion);

            GridColumn ServicioID = new GridColumn();
            ServicioID.FieldName = this.unboundPrefix + "ServicioID";
            ServicioID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_ServicioID");
            ServicioID.UnboundType = UnboundColumnType.String;
            ServicioID.VisibleIndex = 3;
            ServicioID.Width = 60;
            ServicioID.Visible = true;
            ServicioID.ColumnEdit = this.editBtnGrid;
            ServicioID.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(ServicioID);

            GridColumn ServicioDesc = new GridColumn();
            ServicioDesc.FieldName = this.unboundPrefix + "ServicioDesc";
            ServicioDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_ServicioDesc");
            ServicioDesc.UnboundType = UnboundColumnType.String;
            ServicioDesc.VisibleIndex = 4;
            ServicioDesc.Width = 120;
            ServicioDesc.Visible = true;
            ServicioDesc.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(ServicioDesc);

            GridColumn FechaInicio = new GridColumn();
            FechaInicio.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_FechaInicio");
            FechaInicio.UnboundType = UnboundColumnType.DateTime;
            FechaInicio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaInicio.AppearanceCell.Options.UseTextOptions = true;
            FechaInicio.VisibleIndex = 3;
            FechaInicio.Width = 70;
            FechaInicio.Visible = false;
            FechaInicio.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(FechaInicio);

            GridColumn FechaFinal = new GridColumn();
            FechaFinal.FieldName = this.unboundPrefix + "FechaFinal";
            FechaFinal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_FechaFinal");
            FechaFinal.UnboundType = UnboundColumnType.DateTime;
            FechaFinal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFinal.AppearanceCell.Options.UseTextOptions = true;
            FechaFinal.VisibleIndex = 4;
            FechaFinal.Width = 70;
            FechaFinal.Visible = false;
            FechaFinal.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(FechaFinal);

            GridColumn MonedaFactura = new GridColumn();
            MonedaFactura.FieldName = this.unboundPrefix + "MonedaFactura";
            MonedaFactura.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_MonedaFactura");
            MonedaFactura.UnboundType = UnboundColumnType.String;
            MonedaFactura.VisibleIndex = 5;
            MonedaFactura.Width = 70;
            MonedaFactura.Visible = false;
            MonedaFactura.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(MonedaFactura);

            GridColumn ValorFactura = new GridColumn();
            ValorFactura.FieldName = this.unboundPrefix + "ValorFactura";
            ValorFactura.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_ValorFactura");
            ValorFactura.UnboundType = UnboundColumnType.String;
            ValorFactura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorFactura.AppearanceCell.Options.UseTextOptions = true;
            ValorFactura.VisibleIndex = 6;
            ValorFactura.Width = 70;
            ValorFactura.Visible = false;
            ValorFactura.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(ValorFactura);

            GridColumn Observaciones = new GridColumn();
            Observaciones.FieldName = this.unboundPrefix + "Observaciones";
            Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Observaciones");
            Observaciones.UnboundType = UnboundColumnType.String;
            Observaciones.VisibleIndex = 7;
            Observaciones.Width = 70;
            Observaciones.Visible = false;
            Observaciones.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Observaciones);

            GridColumn VlrTotalTareas = new GridColumn();
            VlrTotalTareas.FieldName = this.unboundPrefix + "VlrTotalTareas";
            VlrTotalTareas.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_VlrTotalTareas");
            VlrTotalTareas.UnboundType = UnboundColumnType.String;
            VlrTotalTareas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrTotalTareas.AppearanceCell.Options.UseTextOptions = true;
            VlrTotalTareas.VisibleIndex = 8;
            VlrTotalTareas.Width = 70;
            VlrTotalTareas.Visible = true;
            VlrTotalTareas.ColumnEdit = this.editValue2;
            VlrTotalTareas.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(VlrTotalTareas);

            this.gvHeader.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Grilla Detalle
            
            GridColumn tareaID = new GridColumn();
            tareaID.FieldName = this.unboundPrefix + "TareaID";
            tareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            tareaID.UnboundType = UnboundColumnType.String;
            tareaID.VisibleIndex = 1;
            tareaID.Width = 45;
            tareaID.Visible = true;
            tareaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(tareaID);

            GridColumn tareaClienteDet = new GridColumn();
            tareaClienteDet.FieldName = this.unboundPrefix + "TareaCliente";
            tareaClienteDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            tareaClienteDet.UnboundType = UnboundColumnType.String;
            tareaClienteDet.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaClienteDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tareaClienteDet.AppearanceCell.Options.UseTextOptions = true;
            tareaClienteDet.AppearanceCell.Options.UseFont = true;
            tareaClienteDet.VisibleIndex = 2;
            tareaClienteDet.Width = 25;
            tareaClienteDet.Visible = true;
            //tareaClienteDet.ColumnEdit = this.editCmbTareas;
            tareaClienteDet.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(tareaClienteDet);

            GridColumn TareaEntregableDet = new GridColumn();
            TareaEntregableDet.FieldName = this.unboundPrefix + "TareaEntregable";
            TareaEntregableDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaEntregable");
            TareaEntregableDet.UnboundType = UnboundColumnType.String;
            TareaEntregableDet.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaEntregableDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            TareaEntregableDet.AppearanceCell.Options.UseTextOptions = true;
            TareaEntregableDet.AppearanceCell.Options.UseFont = true;
            TareaEntregableDet.VisibleIndex = 3;
            TareaEntregableDet.Width = 35;
            TareaEntregableDet.Visible = true;
            TareaEntregableDet.ColumnEdit = this.editCmbTareas;
            TareaEntregableDet.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(TareaEntregableDet);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 4;
            descriptivo.Width = 350;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 5;
            UnidadInvID.Width = 30;
            UnidadInvID.Visible = true;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(UnidadInvID);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
            Cantidad.UnboundType = UnboundColumnType.Decimal;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 6;
            Cantidad.Width = 30;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Cantidad);

            GridColumn CostoLocalCLI = new GridColumn();
            CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
            CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
            CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalCLI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoLocalCLI.AppearanceCell.Options.UseTextOptions = true;
            CostoLocalCLI.VisibleIndex = 7;
            CostoLocalCLI.Width = 80;
            CostoLocalCLI.Visible = true;
            CostoLocalCLI.ColumnEdit = this.editValue2;
            CostoLocalCLI.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(CostoLocalCLI);

            GridColumn DetalleInd = new GridColumn();
            DetalleInd.FieldName = this.unboundPrefix + "DetalleInd";
            DetalleInd.UnboundType = UnboundColumnType.Boolean;
            DetalleInd.VisibleIndex = 8;
            DetalleInd.Width = 50;
            DetalleInd.Visible = false;
            DetalleInd.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DetalleInd);

            GridColumn CapituloTareaID = new GridColumn();
            CapituloTareaID.FieldName = this.unboundPrefix + "CapituloTareaID";
            CapituloTareaID.UnboundType = UnboundColumnType.String;
            CapituloTareaID.VisibleIndex = 7;
            CapituloTareaID.Visible = false;
            this.gvDetalle.Columns.Add(CapituloTareaID);

            #endregion
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowTarea()
        {
            try
            {
                DTO_pyProyectoTareaCliente tarea = new DTO_pyProyectoTareaCliente();
                tarea.NumeroDoc.Value = this._numeroDocProy;
                tarea.MonedaFactura.Value = this._ctrlProyecto.MonedaID.Value;
                tarea.Cantidad.Value = 0;
                this._listEntregables.Add(tarea);             
                this._rowTareaCliente = tarea;

                this.gcHeader.DataSource = this._listEntregables;
                this.gcHeader.RefreshDataSource();
                this.gvHeader.FocusedRowHandle = this.gvHeader.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Entregables.cs", "AddNewRow: " + ex.Message));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "InitControls"));
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
                this.gcHeader.DataSource = this._listEntregables;
                this.gcDetalle.DataSource = this._listTareasAll;
                this.gcHeader.RefreshDataSource();
                this.gcDetalle.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadGrids"));
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
            this._rowTarea = new DTO_pyProyectoTarea();
            this._rowTareaCliente = new DTO_pyProyectoTareaCliente();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listEntregables = new List<DTO_pyProyectoTareaCliente>();
            this.gcHeader.DataSource = null;
            this.gcHeader.RefreshDataSource();

            this.gcDetalle.DataSource = null;
            this.gcDetalle.RefreshDataSource();
            this.isValid = true;
            this.ucProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.Entregables;
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
                if (fila >= 0)
                {
                    #region TareaEntregable
                    validField = this._bc.ValidGridCell(this.gvHeader, this.unboundPrefix, fila, "TareaEntregable", false, false, false, null);
                    if (!validField)
                        validRow = false; 
                    else
                    {
                        int count = this._listEntregables.Count(x => x.TareaEntregable.Value == this._rowTareaCliente.TareaEntregable.Value);
                        if(count > 1)
                        {
                            GridColumn col = this.gvHeader.Columns[this.unboundPrefix + "TareaEntregable"];
                            string colVal = this.gvHeader.GetRowCellValue(fila, col).ToString();
                            this.gvHeader.SetColumnError(col, string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_AlreadyExistInGrid),this._rowTareaCliente.TareaEntregable.Value));
                            validRow = false; 
                        }
                    }
                    #endregion                    
                    #region Descriptivo
                    validField = this._bc.ValidGridCell(this.gvHeader, this.unboundPrefix, fila, "Descripcion", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    #endregion
                    #region ServicioID
                    validField = this._bc.ValidGridCell(this.gvHeader, this.unboundPrefix, fila, "ServicioID", false, true, false, AppMasters.faServicios);
                    if (!validField)
                        validRow = false;
                    #endregion                    
                    #region ServicioDesc
                    validField = this._bc.ValidGridCell(this.gvHeader, this.unboundPrefix, fila, "ServicioDesc", false, false, false, null);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "ValidateRow"));
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
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemDelete.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables", "Form_FormClosed"));
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

                    #region Trae los entregables Existentes
                    var entregablesExist = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);
                    foreach (var ent in entregablesExist)
                    {
                        if (!this._listEntregables.Exists(x => x.TareaEntregable.Value == ent.TareaEntregable.Value))
                            this._listEntregables.Add(ent);
                    }

                    #region LLena combo de entregables
                    Dictionary<string, string> cmbEntregables = new Dictionary<string, string>();
                    foreach (var t in this._listEntregables)
                    {
                        t.NumeroDoc.Value = this._numeroDocProy;
                        t.VlrTotalTareas.Value = t.DetalleTareas.Sum(x => x.CostoLocalCLI.Value);
                        cmbEntregables.Add(t.TareaEntregable.Value, t.Descripcion.Value);
                    }
                    cmbEntregables.Add("Ninguno", "");
                    this.editCmbTareas.ValueMember = "Key";
                    this.editCmbTareas.DisplayMember = "Key";
                    this.editCmbTareas.DataSource = cmbEntregables;  
                    #endregion
                      
                    #endregion

                    #region Valida el tipo de Proyecto
                    DTO_pyClaseProyecto clase = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, this.ucProyecto.ProyectoInfo.HeaderProyecto.ClaseServicioID.Value, true);
                    if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion) 
                    {
                        this.gvDetalle.Columns[this.unboundPrefix + "CapituloTareaID"].UnGroup();
                    }
                    else if (clase != null && clase.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) 
                    {
                        this.gvDetalle.Columns[this.unboundPrefix + "CapituloTareaID"].Group();
                        this.gvDetalle.Columns[this.unboundPrefix + "CapituloTareaID"].SortOrder = ColumnSortOrder.None;
                    } 
                    #endregion

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
        /// Carga las tareas del proyecto como entregables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadTarea_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDetalle.DataRowCount > 0)
                {
                    List<DTO_pyProyectoTareaCliente> _listEntregablesExist = ObjectCopier.Clone(this._listEntregables);
                    this._listEntregables = new List<DTO_pyProyectoTareaCliente>();
                    foreach (DTO_pyProyectoTarea tarProy in _listTareasAll)
                    {
                        DTO_pyProyectoTareaCliente entr = new DTO_pyProyectoTareaCliente();
                        DTO_pyTarea tarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, tarProy.TareaID.Value, true);
                        if (tarea != null && tarea.TipoTarea.Value == 1)
                        {
                            entr.NumeroDoc.Value = this._numeroDocProy;
                            entr.Descripcion.Value = tarProy.Descriptivo.Value;
                            entr.TareaEntregable.Value = (this._listEntregables.Count + 1).ToString();
                            entr.ServicioID.Value = tarea.ServicioID.Value;
                            entr.ServicioDesc.Value = tarea.ServicioDesc.Value;
                            if (tarea.EntregaIndividualInd.Value.Value)
                                entr.Cantidad.Value = tarProy.Cantidad.Value;
                            else
                                entr.Cantidad.Value = 0;
                            entr.VlrTotalTareas.Value = tarProy.CostoLocalCLI.Value;
                            entr.MonedaFactura.Value = this._ctrlProyecto.MonedaID.Value;
                            if (_listEntregablesExist.Exists(x => x.Descripcion.Value == entr.Descripcion.Value && x.TareaEntregable.Value == entr.TareaEntregable.Value))
                                entr.Consecutivo.Value = _listEntregablesExist.Find(x => x.Descripcion.Value == entr.Descripcion.Value && x.TareaEntregable.Value == entr.TareaEntregable.Value).Consecutivo.Value;
                            this._listEntregables.Add(entr);
                            //Asigna el entregable
                            tarProy.TareaEntregable.Value = entr.TareaEntregable.Value;
                            this.gvDetalle.RefreshData();
                        }
                    }
                    this.gcHeader.DataSource = this._listEntregables;
                    this.gcHeader.RefreshDataSource();
                    this.gvHeader.MoveFirst();

                    this.gcDetalle.DataSource = null;
                    this.gcDetalle.DataSource = this._listTareasAll;
                    this.gcDetalle.RefreshDataSource();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "btnLoadTarea_Click"));
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
                    this._rowTareaCliente = (DTO_pyProyectoTareaCliente)this.gvHeader.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gvDocument_FocusedRowChanged"));
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
                    if (this.gvHeader.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        if (this._ctrlProyecto != null)
                        {
                            this.deleteOP = false;
                            if (this.isValid)
                                this.AddNewRowTarea();
                            else
                            {
                                bool isV = this.ValidateRow(this.gvHeader.FocusedRowHandle);
                                if (isV)
                                    this.AddNewRowTarea();
                            }
                        }
                        else
                            MessageBox.Show("Debe digitar un proyecto para crear entregables");
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
                        int rowHandle = this.gvHeader.FocusedRowHandle;

                        if (this._listEntregables.Count > 0)
                        {
                            this._listEntregables.RemoveAll(x => x.TareaEntregable.Value == this._rowTareaCliente.TareaEntregable.Value &&
                                                                x.Descripcion.Value == this._rowTareaCliente.Descripcion.Value);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvHeader.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvHeader.FocusedRowHandle = rowHandle - 1;

                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
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
                if (!this.deleteOP && this.gvHeader.DataRowCount > 0)
                    this.ValidateRow(e.RowHandle);
                
                if (!this.isValid)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gvDocument_BeforeLeaveRow"));
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
            catch (Exception)
            {
                
                throw;
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

        /// <summary>
        /// Al modificar las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];

            try
            {  
                if (fieldName == "ServicioID")
                {
                    DTO_MasterBasic clase = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faServicios, false, e.Value.ToString(), true);
                    if (clase != null)
                        this._rowTareaCliente.ServicioDesc.Value = clase.Descriptivo.Value;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvDocument_CellValueChanging"));
            }
        }
        #endregion

        #region Detalle

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
                    this._rowTarea = (DTO_pyProyectoTarea)this.gvDetalle.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvDetalle.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                    if (currentRow.DetalleInd.Value.Value)
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    else
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                if (e.Column.FieldName == this.unboundPrefix + "CapituloTareaID" && e.IsForGroupRow)
                {
                    string capituloDesc = this._listTareasAll.Find(x => x.CapituloTareaID.Value == e.Value.ToString()).CapituloDesc.Value;
                    e.DisplayText = e.Value.ToString() + "  " + capituloDesc;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDetalle_Enter(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> entregables = new Dictionary<string, string>();
                foreach (var tar in this._listEntregables)
                    entregables.Add(tar.TareaEntregable.Value, tar.Descripcion.Value);

                entregables.Add("Ninguno","");
                this.editCmbTareas.ValueMember = "Key";
                this.editCmbTareas.DisplayMember = "Key";
                this.editCmbTareas.DataSource = entregables;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gcDetalle_Enter"));
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

            try
            {
                if (fieldName == "TareaEntregable" && this._rowTareaCliente != null)
                {
                    foreach (var entr in this._listEntregables)
                        entr.VlrTotalTareas.Value = this._listTareasAll.FindAll(x => x.TareaEntregable.Value == entr.TareaEntregable.Value).Sum(y => y.CostoLocalCLI.Value);
                    this.gvHeader.RefreshData();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gvDetalle_CellValueChanged"));
            }
        }

        #endregion        

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {
            this.gvHeader.PostEditor();

            if (this.ValidateRow(this.gvHeader.FocusedRowHandle))
            {
                if (!this._listEntregables.Any(x => string.IsNullOrEmpty(x.ServicioID.Value)))
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                    MessageBox.Show("El servicio no puede estar vacío, verificar los entregables");
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
        /// Nuevo
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
                    MessageBox.Show("El proyecto actual tiene ya Facturas de Venta aprobadas,no puede eliminar datos. Facturas: " + facturas);
                    return;
                }

                if (MessageBox.Show("Desea eliminar los entregables del proyecto?", "Eliminar " + this.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool deleteProgramacion = this._listEntregables.Any(x => x.Detalle.Count > 0) ? true : false;
                    bool deleteActas = this._listEntregables.Any(x => x.DetalleActas.Count > 0) ? true : false;
                    bool anulaPreFacturas = this._listEntregables.Any(x => x.DetalleActas.Any(y => y.NumDocFactura.Value.HasValue)) ? true : false;

                    DTO_TxResult res = this._bc.AdministrationModel.EntregasCliente_Delete(this.documentID, this._numeroDocProy, this._listEntregables, true, deleteProgramacion, deleteActas, anulaPreFacturas);
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

                result = this._bc.AdministrationModel.EntregasCliente_Add(this.documentID, this._numeroDocProy, this._listEntregables,this._listTareasAll,this._listTareasAdic,null);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID,this.userID.ToString(), result, true, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "SaveThread"));
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
