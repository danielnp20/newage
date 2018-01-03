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
    public partial class PlaneacionTiempo : FormWithToolbar
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
        private List<int> select = new List<int>();
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        private bool isValid = true;
        private bool deleteOP = false;
        private PasteOpDTO pasteRet;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        //Variables para importar
        private string formatTareas = string.Empty;
        private string formatRecursos = string.Empty;
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        private int _numeroDoc = 0;
        private int indexDeta = 0;
        private bool _assignFechaInicio = false;
        //Variables de datos
        private DTO_pyPreProyectoDocu _preProyectoDocu = new DTO_pyPreProyectoDocu();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_pyPreProyectoTarea _rowTareaCurrent = new DTO_pyPreProyectoTarea();
        private DTO_pyPreProyectoDeta _rowRecursoCurrent = new DTO_pyPreProyectoDeta();
        private List<DTO_pyPreProyectoTarea> _listTareasAll = new List<DTO_pyPreProyectoTarea>();
        private List<DTO_pyPreProyectoTarea> _listTareasAdicion = new List<DTO_pyPreProyectoTarea>();
        private DTO_pyClaseProyecto _dtoClaseServicio = null;
        private string _tareaXDef = string.Empty;
        private string _trabajoXDef = string.Empty;
        private decimal _IVAPresupuesto = 0;
        private bool _tareaIsFocused = false;
        private decimal _totalRecxTarea = 0;
        private decimal _totalPresupuesto = 0;
        private decimal _totalCliente = 0;
        private decimal _porcPrestManoObra = 0;
        private string _trabajoCurrent = string.Empty;
        private short nivelMax = 0;
        private GridView gvRecursoCurrent = new GridView();
        #endregion

        #region Delegados

        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private  void RefreshGridMethod()
        {
            try
            {
                this.AssignRecursos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "RefreshGridMethod"));
            }
        }

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private  void SaveMethod() { this.RefreshForm(); }

        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private  void SendToApproveMethod() { }

        /// <summary>
        /// Delegado que finaliza el proceso de validacion 
        /// </summary>
        public delegate void EndImportar();
        public EndImportar endImportarDelegate;
        public void EndImportarMethod()
        {
            this.CalculateDates(true);
            foreach (var tar in this._listTareasAll)
                this.CalculateValues(tar);
            this.gcDocument.DataSource = this._listTareasAll;
            this.gcDocument.RefreshDataSource();
            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        private bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public PlaneacionTiempo()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
              
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo.cs", "PlanecionTiempo"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Principal

            GridColumn tareaCliente = new GridColumn();
            tareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            tareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            tareaCliente.UnboundType = UnboundColumnType.String;
            tareaCliente.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            tareaCliente.AppearanceCell.Options.UseTextOptions = true;
            tareaCliente.VisibleIndex = 1;
            tareaCliente.Width = 40;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(tareaCliente);

            GridColumn trabajoID = new GridColumn();
            trabajoID.FieldName = this.unboundPrefix + "TareaID";
            trabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            trabajoID.UnboundType = UnboundColumnType.String;
            trabajoID.VisibleIndex = 2;
            trabajoID.Width = 40;
            trabajoID.Visible = false;
            trabajoID.ColumnEdit = this.editBtnGrid;
            trabajoID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(trabajoID);

            GridColumn descriptivo = new GridColumn();
            descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            descriptivo.UnboundType = UnboundColumnType.String;
            descriptivo.VisibleIndex = 3;
            descriptivo.Width = 350;
            descriptivo.Visible = true;
            descriptivo.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(descriptivo);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.VisibleIndex = 4;
            UnidadInvID.Width = 40;
            UnidadInvID.Visible = true;
            UnidadInvID.ColumnEdit = this.editBtnGrid;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(UnidadInvID);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadAnalisis");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 5;
            Cantidad.Width = 50;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(Cantidad);

            GridColumn CostoTotalUnitML = new GridColumn();
            CostoTotalUnitML.FieldName = this.unboundPrefix + "CostoTotalUnitML";
            CostoTotalUnitML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOTUnit");
            CostoTotalUnitML.UnboundType = UnboundColumnType.Decimal;
            CostoTotalUnitML.VisibleIndex = 6;
            CostoTotalUnitML.Width = 5;
            CostoTotalUnitML.Visible = false;
            CostoTotalUnitML.ColumnEdit = this.editSpin;
            CostoTotalUnitML.OptionsColumn.AllowEdit = false;
            CostoTotalUnitML.AppearanceCell.BackColor = Color.Gainsboro;
            CostoTotalUnitML.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(CostoTotalUnitML);

            GridColumn CostoTotalML = new GridColumn();
            CostoTotalML.FieldName = this.unboundPrefix + "CostoTotalML";
            CostoTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoTotalML");
            CostoTotalML.UnboundType = UnboundColumnType.Decimal;
            CostoTotalML.VisibleIndex = 7;
            CostoTotalML.Width = 8;
            CostoTotalML.Visible = false;
            CostoTotalML.ColumnEdit = this.editSpin;
            CostoTotalML.OptionsColumn.AllowEdit = false;
            CostoTotalML.AppearanceCell.BackColor = Color.Gainsboro;
            CostoTotalML.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(CostoTotalML);

            GridColumn CostoLocalUnitCLI = new GridColumn();
            CostoLocalUnitCLI.FieldName = this.unboundPrefix + "CostoLocalUnitCLI";
            CostoLocalUnitCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalUnitCLI");
            CostoLocalUnitCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalUnitCLI.VisibleIndex = 8;
            CostoLocalUnitCLI.Width = 5;
            CostoLocalUnitCLI.Visible = false;
            CostoLocalUnitCLI.ColumnEdit = this.editSpin;
            CostoLocalUnitCLI.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(CostoLocalUnitCLI);

            GridColumn CostoLocalCLI = new GridColumn();
            CostoLocalCLI.FieldName = this.unboundPrefix + "CostoLocalCLI";
            CostoLocalCLI.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalCLI");
            CostoLocalCLI.UnboundType = UnboundColumnType.Decimal;
            CostoLocalCLI.VisibleIndex = 9;
            CostoLocalCLI.Width = 8;
            CostoLocalCLI.Visible = false;
            CostoLocalCLI.ColumnEdit = this.editSpin;
            CostoLocalCLI.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(CostoLocalCLI);

            GridColumn CostoDiferencia = new GridColumn();
            CostoDiferencia.FieldName = this.unboundPrefix + "CostoDiferenciaML";
            CostoDiferencia.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoDiferencia");
            CostoDiferencia.UnboundType = UnboundColumnType.Decimal;
            CostoDiferencia.VisibleIndex = 10;
            CostoDiferencia.Width = 8;
            CostoDiferencia.Visible = false;
            CostoDiferencia.ColumnEdit = this.editSpin;
            CostoDiferencia.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(CostoDiferencia);

            GridColumn FechaInicioTar = new GridColumn();
            FechaInicioTar.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicioTar.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaInicio");
            FechaInicioTar.UnboundType = UnboundColumnType.DateTime;
            FechaInicioTar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaInicioTar.AppearanceCell.Options.UseTextOptions = true;
            FechaInicioTar.VisibleIndex = 11;
            FechaInicioTar.Width = 80;
            FechaInicioTar.Visible = true;
            FechaInicioTar.OptionsColumn.AllowEdit = true;
            FechaInicioTar.AppearanceCell.BackColor = Color.Gainsboro;
            FechaInicioTar.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(FechaInicioTar);

            GridColumn FechaFinTar = new GridColumn();
            FechaFinTar.FieldName = this.unboundPrefix + "FechaFin";
            FechaFinTar.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaFin");
            FechaFinTar.UnboundType = UnboundColumnType.DateTime;
            FechaFinTar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFinTar.AppearanceCell.Options.UseTextOptions = true;
            FechaFinTar.VisibleIndex = 12;
            FechaFinTar.Width = 80;
            FechaFinTar.Visible = true;
            FechaFinTar.OptionsColumn.AllowEdit = true;
            FechaFinTar.AppearanceCell.BackColor = Color.Gainsboro;
            FechaFinTar.AppearanceCell.Options.UseBackColor = true;
            this.gvDocument.Columns.Add(FechaFinTar);

            GridColumn ImprimirTareaInd = new GridColumn();
            ImprimirTareaInd.FieldName = this.unboundPrefix + "ImprimirTareaInd";
            ImprimirTareaInd.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_ImprimirTareaInd");
            ImprimirTareaInd.UnboundType = UnboundColumnType.Boolean;
            ImprimirTareaInd.VisibleIndex = 13;
            ImprimirTareaInd.Width = 20;
            ImprimirTareaInd.Visible = false;
            ImprimirTareaInd.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(ImprimirTareaInd);

            GridColumn TituloPrintInd = new GridColumn();
            TituloPrintInd.FieldName = this.unboundPrefix + "TituloPrintInd";
            TituloPrintInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TituloPrintInd");
            TituloPrintInd.UnboundType = UnboundColumnType.Boolean;
            TituloPrintInd.VisibleIndex = 14;
            TituloPrintInd.Width = 20;
            TituloPrintInd.Visible = false;
            TituloPrintInd.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(TituloPrintInd);

            GridColumn Nivel = new GridColumn();
            Nivel.FieldName = this.unboundPrefix + "Nivel";
            Nivel.UnboundType = UnboundColumnType.Integer;
            Nivel.VisibleIndex = 15;
            Nivel.Width = 5;
            Nivel.Visible = false;
            Nivel.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(Nivel);

            GridColumn TareaPadre = new GridColumn();
            TareaPadre.FieldName = this.unboundPrefix + "TareaPadre";
            TareaPadre.UnboundType = UnboundColumnType.String;
            TareaPadre.VisibleIndex = 16;
            TareaPadre.Width = 5;
            TareaPadre.Visible = false;
            TareaPadre.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(TareaPadre);

            GridColumn DetalleInd = new GridColumn();
            DetalleInd.FieldName = this.unboundPrefix + "DetalleInd";
            DetalleInd.UnboundType = UnboundColumnType.Boolean;
            DetalleInd.VisibleIndex = 17;
            DetalleInd.Width = 5;
            DetalleInd.Visible = false;
            DetalleInd.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(DetalleInd);

            GridColumn CapituloTareaID = new GridColumn();
            CapituloTareaID.FieldName = this.unboundPrefix + "CapituloTareaID";
            CapituloTareaID.UnboundType = UnboundColumnType.Boolean;
            CapituloTareaID.VisibleIndex = 18;
            CapituloTareaID.Visible = false;
            CapituloTareaID.UnGroup();
            this.gvDocument.Columns.Add(CapituloTareaID);
            this.gvDocument.OptionsBehavior.Editable = true;

            #endregion

            #region Grilla Recursos x Trabajo
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 0;
            RecursoID.Width = 80;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 1;
            RecursoDesc.Width = 200;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDRec = new GridColumn();
            UnidadInvIDRec.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDRec.UnboundType = UnboundColumnType.String;
            UnidadInvIDRec.VisibleIndex = 2;
            UnidadInvIDRec.Width = 70;
            UnidadInvIDRec.Visible = true;
            UnidadInvIDRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDRec);

            GridColumn TrabajoID = new GridColumn();
            TrabajoID.FieldName = this.unboundPrefix + "TrabajoID";
            TrabajoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TrabajoID");
            TrabajoID.UnboundType = UnboundColumnType.String;
            TrabajoID.VisibleIndex = 3;
            TrabajoID.Width = 120;
            TrabajoID.Visible = false;
            TrabajoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(TrabajoID);

            GridColumn FactorID = new GridColumn();
            FactorID.FieldName = this.unboundPrefix + "FactorID";
            FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
            FactorID.UnboundType = UnboundColumnType.Decimal;
            FactorID.VisibleIndex = 5;
            FactorID.Width = 80;
            FactorID.Visible = true;
            FactorID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(FactorID);

            GridColumn CantidadRec = new GridColumn();
            CantidadRec.FieldName = this.unboundPrefix + "Cantidad";
            CantidadRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTrabajo");
            CantidadRec.UnboundType = UnboundColumnType.Decimal;
            CantidadRec.VisibleIndex = 6;
            CantidadRec.Width = 80;
            CantidadRec.Visible = false;
            CantidadRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadRec);

            GridColumn CanSolicitud = new GridColumn();
            CanSolicitud.FieldName = this.unboundPrefix + "CantSolicitud";
            CanSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
            CanSolicitud.UnboundType = UnboundColumnType.Decimal;
            CanSolicitud.VisibleIndex = 7;
            CanSolicitud.Width = 80;
            CanSolicitud.Visible = false;
            CanSolicitud.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CanSolicitud);

            GridColumn TiempoTotal = new GridColumn();
            TiempoTotal.FieldName = this.unboundPrefix + "TiempoTotal";
            TiempoTotal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TiempoTotal");
            TiempoTotal.UnboundType = UnboundColumnType.Integer;
            TiempoTotal.VisibleIndex = 8;
            TiempoTotal.Width = 80;
            TiempoTotal.Visible = true;
            TiempoTotal.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(TiempoTotal);

            GridColumn CostoLocal = new GridColumn();
            CostoLocal.FieldName = this.unboundPrefix + "CostoLocal";
            CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
            CostoLocal.UnboundType = UnboundColumnType.Decimal;
            CostoLocal.VisibleIndex = 9;
            CostoLocal.Width = 80;
            CostoLocal.Visible = false;
            CostoLocal.ColumnEdit = this.editSpin;
            CostoLocal.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CostoLocal);

            GridColumn CantidadTOT = new GridColumn();
            CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTOT");
            CantidadTOT.UnboundType = UnboundColumnType.Decimal;
            CantidadTOT.VisibleIndex = 10;
            CantidadTOT.Width = 80;
            CantidadTOT.Visible = false;
            CantidadTOT.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadTOT);

            GridColumn CostoLocalTOT = new GridColumn();
            CostoLocalTOT.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOT.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
            CostoLocalTOT.UnboundType = UnboundColumnType.Decimal;
            CostoLocalTOT.VisibleIndex = 11;
            CostoLocalTOT.Width = 80;
            CostoLocalTOT.Visible = false;
            CostoLocalTOT.ColumnEdit = this.editSpin;
            CostoLocalTOT.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CostoLocalTOT);

            GridColumn CostoTotalMLRec = new GridColumn();
            CostoTotalMLRec.FieldName = this.unboundPrefix + "CostoTotalML";
            CostoTotalMLRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
            CostoTotalMLRec.UnboundType = UnboundColumnType.Decimal;
            CostoTotalMLRec.VisibleIndex = 12;
            CostoTotalMLRec.Width = 80;
            CostoTotalMLRec.Visible = false;
            CostoTotalMLRec.ColumnEdit = this.editSpin;
            CostoTotalMLRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CostoTotalMLRec);

            GridColumn FechaInicio = new GridColumn();
            FechaInicio.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaInicio");
            FechaInicio.UnboundType = UnboundColumnType.DateTime;
            FechaInicio.VisibleIndex = 13;
            FechaInicio.Width = 40;
            FechaInicio.Visible = false;
            FechaInicio.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(FechaInicio);

            GridColumn FechaFin = new GridColumn();
            FechaFin.FieldName = this.unboundPrefix + "FechaFin";
            FechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaFin");
            FechaFin.UnboundType = UnboundColumnType.DateTime;
            FechaFin.VisibleIndex = 14;
            FechaFin.Width = 40;
            FechaFin.Visible = false;
            FechaFin.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(FechaFin);

            GridColumn TipoRecurso = new GridColumn();
            TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecurso.UnboundType = UnboundColumnType.Integer;
            TipoRecurso.Width = 80;
            TipoRecurso.Visible = false;
            TipoRecurso.Group();
            TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecurso.Columns.Add(TipoRecurso);

            this.gvRecurso.OptionsBehavior.Editable = true;
            #endregion

            #region Grilla Detalle Recursos
            GridColumn RecursoIDDet = new GridColumn();
            RecursoIDDet.FieldName = this.unboundPrefix + "RecursoID";
            RecursoIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoIDDet.UnboundType = UnboundColumnType.String;
            RecursoIDDet.VisibleIndex = 0;
            RecursoIDDet.Width = 80;
            RecursoIDDet.Visible = true;
            RecursoIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(RecursoIDDet);

            GridColumn RecursoDescet = new GridColumn();
            RecursoDescet.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDescet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDescet.UnboundType = UnboundColumnType.String;
            RecursoDescet.VisibleIndex = 1;
            RecursoDescet.Width = 200;
            RecursoDescet.Visible = true;
            RecursoDescet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(RecursoDescet);

            GridColumn UnidadInvIDDet = new GridColumn();
            UnidadInvIDDet.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDDet.UnboundType = UnboundColumnType.Integer;
            UnidadInvIDDet.VisibleIndex = 2;
            UnidadInvIDDet.Width = 80;
            UnidadInvIDDet.Visible = true;
            UnidadInvIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(UnidadInvIDDet);       

            GridColumn FactorIDDet = new GridColumn();
            FactorIDDet.FieldName = this.unboundPrefix + "FactorID";
            FactorIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Rendimiento");
            FactorIDDet.UnboundType = UnboundColumnType.Decimal;
            FactorIDDet.VisibleIndex = 5;
            FactorIDDet.Width = 80;
            FactorIDDet.Visible = true;
            FactorIDDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(FactorIDDet);

            GridColumn CantidadDet = new GridColumn();
            CantidadDet.FieldName = this.unboundPrefix + "Cantidad";
            CantidadDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
            CantidadDet.UnboundType = UnboundColumnType.Decimal;
            CantidadDet.VisibleIndex = 6;
            CantidadDet.Width = 80;
            CantidadDet.Visible = false;
            CantidadDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(CantidadDet);

            GridColumn CantSolicitudDet = new GridColumn();
            CantSolicitudDet.FieldName = this.unboundPrefix + "CantSolicitud";
            CantSolicitudDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
            CantSolicitudDet.UnboundType = UnboundColumnType.Decimal;
            CantSolicitudDet.VisibleIndex = 7;
            CantSolicitudDet.Width = 80;
            CantSolicitudDet.Visible = false;
            CantSolicitudDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(CantSolicitudDet);

            GridColumn TiempoTotalRec = new GridColumn();
            TiempoTotalRec.FieldName = this.unboundPrefix + "TiempoTotal";
            TiempoTotalRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TiempoTotal");
            TiempoTotalRec.UnboundType = UnboundColumnType.Integer;
            TiempoTotalRec.VisibleIndex = 8;
            TiempoTotalRec.Width = 80;
            TiempoTotalRec.Visible = true;
            TiempoTotalRec.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(TiempoTotalRec);

            GridColumn CostoLocalDet = new GridColumn();
            CostoLocalDet.FieldName = this.unboundPrefix + "CostoLocal";
            CostoLocalDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
            CostoLocalDet.UnboundType = UnboundColumnType.Decimal;
            CostoLocalDet.VisibleIndex = 8;
            CostoLocalDet.Width = 80;
            CostoLocalDet.Visible = false;
            CostoLocalDet.ColumnEdit = this.editSpin;
            CostoLocalDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(CostoLocalDet);

            GridColumn CantidadTOTDet = new GridColumn();
            CantidadTOTDet.FieldName = this.unboundPrefix + "CantidadTOT";
            CantidadTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadTOT");
            CantidadTOTDet.UnboundType = UnboundColumnType.Decimal;
            CantidadTOTDet.VisibleIndex = 9;
            CantidadTOTDet.Width = 80;
            CantidadTOTDet.Visible = false;
            CantidadTOTDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(CantidadTOTDet);

            GridColumn CostoLocalTOTDet = new GridColumn();
            CostoLocalTOTDet.FieldName = this.unboundPrefix + "CostoLocalTOT";
            CostoLocalTOTDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
            CostoLocalTOTDet.UnboundType = UnboundColumnType.Decimal;
            CostoLocalTOTDet.VisibleIndex = 10;
            CostoLocalTOTDet.Width = 80;
            CostoLocalTOTDet.Visible = false;
            CostoLocalTOTDet.ColumnEdit = this.editSpin;
            CostoLocalTOTDet.OptionsColumn.AllowEdit = false;
            this.gvDetalleRecurso.Columns.Add(CostoLocalTOTDet);

            GridColumn FechaInicioDet = new GridColumn();
            FechaInicioDet.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicioDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaInicio");
            FechaInicioDet.UnboundType = UnboundColumnType.DateTime;
            FechaInicioDet.VisibleIndex = 11;
            FechaInicioDet.Width = 80;
            FechaInicioDet.Visible = true;
            FechaInicioDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(FechaInicioDet);

            GridColumn FechaFinDet = new GridColumn();
            FechaFinDet.FieldName = this.unboundPrefix + "FechaFin";
            FechaFinDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaFin");
            FechaFinDet.UnboundType = UnboundColumnType.DateTime;
            FechaFinDet.VisibleIndex = 12;
            FechaFinDet.Width = 80;
            FechaFinDet.Visible = true;
            FechaFinDet.OptionsColumn.AllowEdit = true;
            this.gvDetalleRecurso.Columns.Add(FechaFinDet);

            GridColumn TipoRecursoDet = new GridColumn();
            TipoRecursoDet.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecursoDet.UnboundType = UnboundColumnType.Integer;
            TipoRecursoDet.Width = 80;
            TipoRecursoDet.Visible = false;
            TipoRecursoDet.Group();
            TipoRecursoDet.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecursoDet.SortOrder = ColumnSortOrder.Ascending;
            this.gvDetalleRecurso.Columns.Add(TipoRecursoDet);

            this.gvDetalleRecurso.OptionsBehavior.Editable = true;
            #endregion
        }

        /// <summary>
        /// Asigna los recursos  a cada tarea
        /// </summary>
        private void AssignRecursos()
        {
            try
            {
                if (this._tareaIsFocused)
                {
                    this._totalRecxTarea = 0;   
                    this._totalCliente = 0;
                    this._totalPresupuesto = 0;
                    if (this.chkRecursoXTrabInd.Checked)
                    {
                        #region Recursos X Trabajo
                        List<string> trabajos = this._rowTareaCurrent.Detalle.Select(x => x.TrabajoID.Value.ToString()).Distinct().ToList();
                        List<DTO_pyPreProyectoTarea> listTarea = new List<DTO_pyPreProyectoTarea>();
                        foreach (var t in trabajos)
                        {
                            DTO_pyPreProyectoTarea tarea = new DTO_pyPreProyectoTarea();
                            tarea.TareaID.Value = this._rowTareaCurrent.TareaID.Value;
                            tarea.Descriptivo.Value = this._rowTareaCurrent.Descriptivo.Value;
                            tarea.TrabajoID.Value = t;
                            tarea.UnidadInvID.Value = this._rowTareaCurrent.UnidadInvID.Value;
                            tarea.Cantidad.Value = 1;
                            tarea.Detalle = this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == t).ToList();
                            listTarea.Add(tarea);
                        }

                        foreach (var t in listTarea)
                            this.CalculateValues(t);

                        if (listTarea.Count > 0)
                        {
                            this._rowTareaCurrent.CostoTotalML.Value = listTarea.Sum(x => x.CostoTotalML.Value);
                            //this._rowCapituloCurrent.Detalle[this._rowTareaCurrent.Index].CostoTotalML.Value = this._rowTareaCurrent.CostoTotalML.Value;
                        }

                        #region Reasigna Detalle
                        this._rowTareaCurrent.Detalle = new List<DTO_pyPreProyectoDeta>();
                        foreach (var t in listTarea)
                            this._rowTareaCurrent.Detalle.AddRange(t.Detalle);
                        this._totalRecxTarea = this._rowTareaCurrent.CostoTotalML.Value.Value;
                        //this._rowCapituloCurrent.Detalle[this._rowTareaCurrent.Index].Detalle = this._rowTareaCurrent.Detalle;
                        
                        #endregion

                        this.gcRecurso.DataSource = listTarea;
                        #endregion
                    }
                    else
                    {
                        #region Recursos por Tarea
               
                        if (this._rowTareaCurrent.Detalle.Count > 0)
                            this.CalculateValues(this._rowTareaCurrent);

                        if (this.gvRecurso.DataRowCount > 0)
                            this.gvRecurso.FocusedRowHandle = 0;
                        this.gcRecurso.DataSource = this._rowTareaCurrent.Detalle;
                        #endregion
                    }

                    this.gcRecurso.RefreshDataSource();
                    this.gvRecurso.MoveFirst();
                    this.gvDocument.RefreshRow(this._rowTareaCurrent.Index);
                    foreach (DTO_pyPreProyectoTarea t in this._listTareasAll)
                    {
                        this._totalPresupuesto += t.CostoTotalML.Value.Value;
                        this._totalCliente += t.CostoLocalCLI.Value.Value;
                    }
                    this.txtCostoPresupuesto.EditValue = this._totalPresupuesto;
                    this.txtCostoCliente.EditValue = this._totalCliente; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        /// Carga la información del documento
        /// </summary>
        private void CargarInformacion(bool exist)
        {
            try
            {
                if (!exist)
                {
                    this._preProyectoDocu.ClienteID.Value = this.masterCliente.Value;
                    this._preProyectoDocu.DescripcionSOL.Value = this.txtDescripcion.Text;
                    this._preProyectoDocu.EmpresaNombre.Value = this.txtSolicitante.Text;
                    this._preProyectoDocu.ResponsableCLI.Value = this.txtResposableCli.Text;
                    this._preProyectoDocu.ResponsableEMP.Value = this.masterResponsableEmp.Value;
                    this._preProyectoDocu.ResponsableCorreo.Value = this.txtCorreo.Text;
                    this._preProyectoDocu.ResponsableTelefono.Value = this.txtTelefono.Text;
                    this._preProyectoDocu.RecursosXTrabajoInd.Value = this.chkRecursoXTrabInd.Checked;
                    this._preProyectoDocu.TipoSolicitud.Value = Convert.ToByte(this.cmbTipoSolicitud.EditValue);
                    this._preProyectoDocu.PropositoProyecto.Value = Convert.ToByte(this.cmbProposito.EditValue);
                    this._preProyectoDocu.Licitacion.Value = this.txtLicitacion.Text;
                    this._preProyectoDocu.APUIncluyeAIUInd.Value = this.chkAPUIncluyeAIU.Checked;
                    this._preProyectoDocu.Jerarquia.Value = Convert.ToByte(this.cmbJerarquia.EditValue);
                    this._preProyectoDocu.PorClienteADM.Value = Convert.ToDecimal(this.txtPorAdmClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorClienteIMP.Value = Convert.ToDecimal(this.txtPorImprClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorClienteUTI.Value = Convert.ToDecimal(this.txtPorUtilClient.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaADM.Value = Convert.ToDecimal(this.txtPorAdmEmp.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaIMP.Value = Convert.ToDecimal(this.txtPorImprEmp.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.PorEmpresaUTI.Value = Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture);
                    //Adicionales
                    this._preProyectoDocu.Valor.Value = Convert.ToDecimal(this.txtCostoPresupuesto.EditValue, CultureInfo.InvariantCulture);
                    this._preProyectoDocu.ValorCliente.Value = Convert.ToDecimal(this.txtCostoCliente.EditValue, CultureInfo.InvariantCulture);
                }
                else
                {
                    this.cmbTipoPresup.EditValue = this._dtoClaseServicio != null ? this._dtoClaseServicio.TipoPresupuesto.Value.ToString() : "1";
                    this.masterClaseServicio.Value = this._preProyectoDocu.ClaseServicioID.Value;
                    this.masterCliente.Value = this._preProyectoDocu.ClienteID.Value;
                    this.txtDescripcion.Text = this._preProyectoDocu.DescripcionSOL.Value;
                    this.txtSolicitante.Text = this._preProyectoDocu.EmpresaNombre.Value;
                    this.masterProyecto.Value = this._ctrl.ProyectoID.Value;
                    this.masterCentroCto.Value = this._ctrl.CentroCostoID.Value;
                    this.txtResposableCli.Text = this._preProyectoDocu.ResponsableCLI.Value;
                    this.masterResponsableEmp.Value = this._preProyectoDocu.ResponsableEMP.Value;
                    this.txtCorreo.Text = this._preProyectoDocu.ResponsableCorreo.Value;
                    this.txtTelefono.Text = this._preProyectoDocu.ResponsableTelefono.Value;
                    this.cmbTipoSolicitud.EditValue = this._preProyectoDocu.TipoSolicitud.Value.Value;
                    this.cmbProposito.EditValue = this._preProyectoDocu.PropositoProyecto.Value.Value;
                    this.txtObservaciones.Text = this._ctrl.Observacion.Value;
                    this.chkRecursoXTrabInd.Checked = this._preProyectoDocu.RecursosXTrabajoInd.Value.Value;
                    this.txtLicitacion.Text = this._preProyectoDocu.Licitacion.Value;
                    this.chkAPUIncluyeAIU.Checked = this._preProyectoDocu.APUIncluyeAIUInd.Value.Value;
                    this.chkRecursoXTrabInd.Checked = this._preProyectoDocu.RecursosXTrabajoInd.Value.Value;
                    this.cmbJerarquia.EditValue = this._preProyectoDocu.Jerarquia.Value.ToString();
                    this.masterAreaFuncional.Value = this._ctrl.AreaFuncionalID.Value;
                    this.txtPorAdmClient.EditValue = this._preProyectoDocu.PorClienteADM.Value;
                    this.txtPorImprClient.EditValue = this._preProyectoDocu.PorClienteIMP.Value;
                    this.txtPorUtilClient.EditValue = this._preProyectoDocu.PorClienteUTI.Value;
                    this.txtPorAdmEmp.EditValue = this._preProyectoDocu.PorEmpresaADM.Value;
                    this.txtPorImprEmp.EditValue = this._preProyectoDocu.PorEmpresaIMP.Value;
                    this.txtPorUtilEmp.EditValue = this._preProyectoDocu.PorEmpresaUTI.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "CargarInformacion"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void CalculateValues(DTO_pyPreProyectoTarea tarea, bool firstTime = false)
        {
            try
            {
                //Valida si calcula los tiempos de forma automatica(APU) o manual(TAREAS)
                if (this._dtoClaseServicio != null )
                {
                    #region Calcula Costos y Cantidades
                    foreach (DTO_pyPreProyectoDeta d in tarea.Detalle)
                    {
                        if (d.TipoRecurso.Value == (byte)TipoRecurso.Equipo || d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            d.TiempoTotal.Value = d.FactorID.Value != 0 ? Math.Round(tarea.Cantidad.Value.Value / d.FactorID.Value.Value, 2) : 0;
                        else
                            d.TiempoTotal.Value = Math.Round(tarea.Cantidad.Value.Value * d.FactorID.Value.Value, 2);
                    }
                    //if (tarea.Detalle.Count > 0 && this._dtoClaseServicio.TiempoTareaAutInd.Value.Value)
                    //{
                    //    tarea.FechaInicio.Value = tarea.Detalle.First().FechaInicio.Value;
                    //    tarea.FechaFin.Value = tarea.Detalle.Last().FechaFin.Value;
                    //}
                    #endregion
                    #region Asigna Fechas por Nivel
                    if (tarea.Nivel.Value > 1)
                    {
                        //Calcula fechas por cada Nivel
                        var rowPadre1 = this._listTareasAll.Find(x => x.TareaCliente.Value == tarea.TareaPadre.Value);
                        if (rowPadre1 != null)
                        {
                            var childs1 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == tarea.TareaPadre.Value).ToList();
                            rowPadre1.FechaInicio.Value = childs1.Min(y => y.FechaInicio.Value) != null ? childs1.Min(y => y.FechaInicio.Value).Value : rowPadre1.FechaInicio.Value;
                            rowPadre1.FechaFin.Value = childs1.Max(y => y.FechaFin.Value) != null ? childs1.Max(y => y.FechaFin.Value).Value : rowPadre1.FechaFin.Value;
                            #region Nivel 2
                            if (rowPadre1.Nivel.Value > 1)
                            {
                                var rowPadre2 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre1.TareaPadre.Value);
                                if (rowPadre2 != null)
                                {
                                    var childs2 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre1.TareaPadre.Value).ToList();
                                    rowPadre2.FechaInicio.Value = childs2.Min(y => y.FechaInicio.Value) != null ? childs2.Min(y => y.FechaInicio.Value).Value : rowPadre2.FechaInicio.Value;
                                    rowPadre2.FechaFin.Value = childs2.Max(y => y.FechaFin.Value) != null ? childs2.Max(y => y.FechaFin.Value).Value : rowPadre2.FechaFin.Value;
                                    #region Nivel 3
                                    if (rowPadre2.Nivel.Value > 1)
                                    {
                                        var rowPadre3 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre2.TareaPadre.Value);
                                        if (rowPadre3 != null)
                                        {
                                            var childs3 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre2.TareaPadre.Value).ToList();
                                            rowPadre3.FechaInicio.Value = childs3.Min(y => y.FechaInicio.Value) != null ? childs3.Min(y => y.FechaInicio.Value).Value : rowPadre3.FechaInicio.Value;
                                            rowPadre3.FechaFin.Value = childs3.Max(y => y.FechaFin.Value) != null ? childs3.Max(y => y.FechaFin.Value).Value : rowPadre3.FechaFin.Value;
                                            #region Nivel 4
                                            if (rowPadre3.Nivel.Value > 1)
                                            {
                                                var rowPadre4 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre3.TareaPadre.Value);
                                                if (rowPadre4 != null)
                                                {
                                                    var childs4 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre3.TareaPadre.Value).ToList();
                                                    rowPadre4.FechaInicio.Value = childs4.Min(y => y.FechaInicio.Value) != null ? childs4.Min(y => y.FechaInicio.Value).Value : rowPadre4.FechaInicio.Value;
                                                    rowPadre4.FechaFin.Value = childs4.Max(y => y.FechaFin.Value) != null ? childs4.Max(y => y.FechaFin.Value).Value : rowPadre4.FechaFin.Value;
                                                    #region Nivel 5
                                                    if (rowPadre4.Nivel.Value > 1)
                                                    {
                                                        var rowPadre5 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre4.TareaPadre.Value);
                                                        if (rowPadre5 != null)
                                                        {
                                                            var childs5 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre4.TareaPadre.Value).ToList();
                                                            rowPadre5.FechaInicio.Value = childs5.Min(y => y.FechaInicio.Value) != null ? childs5.Min(y => y.FechaInicio.Value).Value : rowPadre5.FechaInicio.Value;
                                                            rowPadre5.FechaFin.Value = childs5.Max(y => y.FechaFin.Value).Value != null ? childs5.Max(y => y.FechaFin.Value).Value : rowPadre5.FechaFin.Value;
                                                            #region Nivel 6
                                                            if (rowPadre5.Nivel.Value > 1)
                                                            {
                                                                var rowPadre6 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre5.TareaPadre.Value);
                                                                if ( rowPadre6 != null)
                                                                {
                                                                    var childs6 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre5.TareaPadre.Value).ToList();
                                                                    rowPadre6.FechaInicio.Value = childs6.Min(y => y.FechaInicio.Value) != null ? childs6.Min(y => y.FechaInicio.Value).Value : rowPadre6.FechaInicio.Value;
                                                                    rowPadre6.FechaFin.Value = childs6.Max(y => y.FechaFin.Value) != null ? childs6.Max(y => y.FechaFin.Value).Value : rowPadre6.FechaFin.Value;
                                                                    #region Nivel 7
                                                                    if (rowPadre6.Nivel.Value > 1)
                                                                    {
                                                                        var rowPadre7 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre6.TareaPadre.Value);
                                                                        if (rowPadre7 != null)
                                                                        {
                                                                            var childs7 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre6.TareaPadre.Value).ToList();
                                                                            rowPadre7.FechaInicio.Value = childs7.Min(y => y.FechaInicio.Value) != null ? childs7.Min(y => y.FechaInicio.Value).Value : rowPadre7.FechaInicio.Value;
                                                                            rowPadre7.FechaFin.Value = childs7.Max(y => y.FechaFin.Value) != null ? childs7.Max(y => y.FechaFin.Value).Value : rowPadre7.FechaFin.Value;
                                                                            #region Nivel 8
                                                                            if (rowPadre7.Nivel.Value > 1)
                                                                            {
                                                                                var rowPadre8 = this._listTareasAll.Find(x => x.TareaCliente.Value == rowPadre7.TareaPadre.Value);
                                                                                if (rowPadre8 != null)
                                                                                {
                                                                                    var childs8 = this._listTareasAll.FindAll(x => x.TareaPadre.Value == rowPadre7.TareaPadre.Value).ToList();
                                                                                    rowPadre8.FechaInicio.Value = childs8.Min(y => y.FechaInicio.Value) != null ? childs8.Min(y => y.FechaInicio.Value).Value : rowPadre8.FechaInicio.Value;
                                                                                    rowPadre8.FechaFin.Value = childs8.Max(y => y.FechaFin.Value) != null ? childs8.Max(y => y.FechaFin.Value).Value : rowPadre8.FechaFin.Value;

                                                                                }
                                                                            }
                                                                            #endregion 
                                                                        }
                                                                    }
                                                                    #endregion 
                                                                }
                                                            }
                                                            #endregion 
                                                        }
                                                    } 
                                                }
                                                #endregion
                                            }
                                            #endregion 
                                        }
                                    }
                                    #endregion 
                                }
                            } 
                            #endregion
                        }
                    }

                    if (this.chkRecursoXTrabInd.Checked)
                    {
                        //Asigna el costo por trabajo
                        decimal? costoxTrabajo = this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == this._trabajoCurrent).Sum(y => y.CostoLocalTOT.Value);
                        this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "CostoTotalML", costoxTrabajo);
                    }

                    #endregion
                    #region Actualiza Datos
                    if (!firstTime)
                    {
                        this.gvRecursoCurrent.RefreshData();
                        this.gvDocument.RefreshData();
                        this.gvRecurso.RefreshData(); 
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "CalculateValues"));
            }
        }

        /// <summary>
        /// Calcula la jerarquia de cada tarea para saber si es detalle o no
        /// </summary>
        /// <param name="tarea">tarea actual a validar</param>
        /// <param name="value">valor ingresado</param>
        private void CalculateHierarchy(DTO_pyPreProyectoTarea tarea, string value)
        {
            try
            {
                //Obtiene los niveles digitados en TareaCliente
                string[] nivel = value.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                tarea.Nivel.Value = Convert.ToInt16(nivel.Count(x => !x.Equals("0") && !x.Equals("00") && !x.Equals("000")));

                //Valida si el registro es detalle o no para calcular valores
                if (this.nivelMax == tarea.Nivel.Value)
                    tarea.DetalleInd.Value = true;
                else
                    tarea.DetalleInd.Value = false;

                //Asigna la tareaPadre de la tareaCliente digitada
                if (this.gvDocument.FocusedRowHandle == 0)
                    tarea.TareaPadre.Value = string.Empty;
                else
                {
                    int nivelPreview = tarea.Nivel.Value.Value - 1;
                    //string tareaPreview = string.Empty;
                    //for (int i = 0; i < nivelPreview; i++)
                    //{
                    //    tareaPreview += nivel[i]+".";
                    //}
                    //var rowPreview = this._listTareasAll.FindLast(x => x.Nivel.Value == nivelPreview && x.TareaCliente.Value.Contains(tareaPreview));
                    var rowPreview = this._listTareasAll.FindLast(x => x.Nivel.Value == nivelPreview);
                    if (rowPreview != null)
                        tarea.TareaPadre.Value = rowPreview.TareaCliente.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "CalculateHierarchy"));
            }
        }

        /// <summary>
        /// Calcula las fechas 
        /// </summary>
        /// <param name="firstTime"></param>
        private void CalculateDates(bool recursosAll)
        {
            try
            {
                if (!recursosAll)
                {
                    if (this.chkRecursoXTrabInd.Checked)
                    {
                        //Calcula fechas por trabajo
                        if (this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == this._trabajoCurrent).ToList().Count > 0)
                        {
                            //DTO_pyPreProyectoDeta this._rowRecursoCurrent = this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == this._trabajoCurrent).First();
                            this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "FechaInicio", this._rowRecursoCurrent.FechaInicio.Value);
                           if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Equipo || this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            {
                                #region Calcula la fechaFin del Recurso Actual con el Tiempo Total
                                if (this._rowRecursoCurrent.FechaInicio.Value != null)
                                {
                                    if (this.masterUnidadBaseDia.Value == this._rowRecursoCurrent.UnidadInvID.Value)
                                        this._rowRecursoCurrent.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)this._rowRecursoCurrent.TiempoTotal.Value);
                                    else
                                    {
                                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                        keysConvert.Add("UnidadInvID", this._rowRecursoCurrent.UnidadInvID.Value);
                                        keysConvert.Add("UnidadBase", this.masterUnidadBaseDia.Value);
                                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                        if (conversion != null && conversion.Factor.Value != 0)
                                        {
                                            decimal dias = this._rowRecursoCurrent.TiempoTotal.Value.Value / conversion.Factor.Value.Value;
                                            this._rowRecursoCurrent.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)dias);//Unidad estandar Dia
                                        }
                                    }
                                    this.gvRecursoCurrent.SetRowCellValue(this.gvRecursoCurrent.FocusedRowHandle, this.unboundPrefix + "FechaFin", this._rowRecursoCurrent.FechaFin.Value);

                                    if (this.gvRecursoCurrent.FocusedRowHandle == 0 && this.gvRecursoCurrent.DataRowCount > 1)
                                    {
                                        #region Si es el primer item, cambia la fecha Inicio de todos los recursos sino existe
                                        int index = 1;
                                        this._assignFechaInicio = false;
                                        foreach (DTO_pyPreProyectoDeta d in this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == this._trabajoCurrent))
                                        {
                                            if (d.FechaInicio.Value == null)
                                                this.gvRecursoCurrent.SetRowCellValue(index, this.unboundPrefix + "FechaInicio", this._rowRecursoCurrent.FechaInicio.Value);
                                            #region Calcula la fechaFin del Recurso con el Tiempo Total
                                            if (this._rowRecursoCurrent.FechaInicio.Value != null)
                                            {
                                                if (this.masterUnidadBaseDia.Value == d.UnidadInvID.Value)
                                                    d.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)d.TiempoTotal.Value);
                                                else
                                                {
                                                    Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                                    keysConvert.Add("UnidadInvID", d.UnidadInvID.Value);
                                                    keysConvert.Add("UnidadBase", this.masterUnidadBaseDia.Value);
                                                    DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                                    if (conversion != null && conversion.Factor.Value != 0)
                                                    {
                                                        decimal dias = d.TiempoTotal.Value.Value / conversion.Factor.Value.Value;
                                                        d.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)dias);//Unidad estandar Dia
                                                    }
                                                }
                                                this.gvRecursoCurrent.SetRowCellValue(this.gvRecursoCurrent.FocusedRowHandle, this.unboundPrefix + "FechaFin", d.FechaFin.Value);
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                        this._assignFechaInicio = true;

                                    //Valida si calcula los tiempos de forma automatica(APU) o manual(TAREAS)
                                    //if (this._dtoClaseServicio != null && this._dtoClaseServicio.TiempoTareaAutInd.Value.Value)
                                    //{
                                    //    DateTime? fechaFinxTrabajo = this._rowTareaCurrent.Detalle.FindAll(x => x.TrabajoID.Value == this._trabajoCurrent).Last().FechaFin.Value;
                                    //    this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "FechaFin", fechaFinxTrabajo);
                                    //    this._rowTareaCurrent.FechaFin.Value = this._rowTareaCurrent.Detalle.Last().FechaFin.Value;
                                    //}
                                }
                                #endregion 
                            }
                        }
                    }
                    else
                    {
                        //Calcula fechas por tarea
                        if (this._rowTareaCurrent.Detalle.Count > 0)
                        {
                            if (this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Equipo || this._rowRecursoCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            {
                                #region Calcula la fechaFin del Recurso con el Tiempo Total
                                if (this._rowRecursoCurrent.FechaInicio.Value != null)
                                {
                                    if (this.masterUnidadBaseDia.Value == this._rowRecursoCurrent.UnidadInvID.Value)
                                        this._rowRecursoCurrent.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)this._rowRecursoCurrent.TiempoTotal.Value);
                                    else
                                    {
                                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                        keysConvert.Add("UnidadInvID", this._rowRecursoCurrent.UnidadInvID.Value);
                                        keysConvert.Add("UnidadBase", this.masterUnidadBaseDia.Value);
                                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                        if (conversion != null && conversion.Factor.Value != 0)
                                        {
                                            decimal dias = this._rowRecursoCurrent.TiempoTotal.Value.Value / conversion.Factor.Value.Value;
                                            this._rowRecursoCurrent.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)dias);//Unidad estandar Dia
                                        }
                                    }
                                    this.gvRecurso.SetRowCellValue(this.gvRecurso.FocusedRowHandle, this.unboundPrefix + "FechaFin", this._rowRecursoCurrent.FechaFin.Value);

                                    if (this.gvRecurso.FocusedRowHandle == 0 && this.gvRecurso.DataRowCount > 1)
                                    {
                                        #region Si es el primer item, cambia la fecha Inicio de todos los recursos sino existe
                                        int index = 0;
                                        this._assignFechaInicio = false;
                                        foreach (DTO_pyPreProyectoDeta d in this._rowTareaCurrent.Detalle)
                                        {
                                            if (d.FechaInicio.Value == null)
                                                this.gvRecurso.SetRowCellValue(index, this.unboundPrefix + "FechaInicio", this._rowRecursoCurrent.FechaInicio.Value);
                                            #region Calcula la fechaFin del Recurso con el Tiempo Total
                                            if (this._rowRecursoCurrent.FechaInicio.Value != null)
                                            {
                                                if (this.masterUnidadBaseDia.Value == d.UnidadInvID.Value)
                                                    d.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)d.TiempoTotal.Value);
                                                else
                                                {
                                                    Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                                    keysConvert.Add("UnidadInvID", d.UnidadInvID.Value);
                                                    keysConvert.Add("UnidadBase", this.masterUnidadBaseDia.Value);
                                                    DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                                    if (conversion != null && conversion.Factor.Value != 0)
                                                    {
                                                        decimal dias = d.TiempoTotal.Value.Value / conversion.Factor.Value.Value;
                                                        d.FechaFin.Value = this._rowRecursoCurrent.FechaInicio.Value.Value.AddDays((double)dias);//Unidad estandar Dia
                                                    }
                                                }
                                                this.gvRecursoCurrent.SetRowCellValue(this.gvRecursoCurrent.FocusedRowHandle, this.unboundPrefix + "FechaFin", d.FechaFin.Value);
                                            }
                                            index++;
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    else
                                        this._assignFechaInicio = true;

                                    //Valida si calcula los tiempos de forma automatica(APU) o manual(TAREAS)
                                    //if (this._dtoClaseServicio != null && this._dtoClaseServicio.TiempoTareaAutInd.Value.Value)
                                    //{
                                    //    var detaxTarea = ObjectCopier.Clone(this._rowTareaCurrent.Detalle);
                                    //    DateTime? fechaFinxTarea = detaxTarea.OrderBy(x => x.FechaFin.Value).ToList().Last().FechaFin.Value;
                                    //    this._rowTareaCurrent.FechaFin.Value = fechaFinxTarea;
                                    //    this.gvDocument.RefreshData();
                                    //}
                                }
                                #endregion 
                            }

                        }
                    } 
                }
                else
                {
                    //Calcula fechas TODOS los recursos
                    foreach (DTO_pyPreProyectoTarea tarea in this._listTareasAll)
                    {
                        foreach (DTO_pyPreProyectoDeta rec in tarea.Detalle)
                        {
                            rec.FechaInicio.Value = tarea.FechaInicio.Value;  
                            if (rec.TipoRecurso.Value == (byte)TipoRecurso.Equipo || rec.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                            {
                                rec.TiempoTotal.Value = rec.FactorID.Value * tarea.Cantidad.Value;
                                #region Calcula la fechaFin del Recurso con el Tiempo Total
                                if (rec.FechaInicio.Value != null && rec.TiempoTotal.Value != null)
                                {
                                    if (this.masterUnidadBaseDia.Value == rec.UnidadInvID.Value)
                                    {
                                        if (rec.TiempoTotal.Value < 10000)
                                            rec.FechaFin.Value = rec.FechaInicio.Value.Value.AddDays((double)rec.TiempoTotal.Value);//Unidad estandar Dia
                                    }
                                    else
                                    {
                                        Dictionary<string, string> keysConvert = new Dictionary<string, string>();
                                        keysConvert.Add("UnidadInvID", rec.UnidadInvID.Value);
                                        keysConvert.Add("UnidadBase", this.masterUnidadBaseDia.Value);
                                        DTO_inConversionUnidad conversion = (DTO_inConversionUnidad)this._bc.GetMasterComplexDTO(AppMasters.inConversionUnidad, keysConvert, true);
                                        if (conversion != null && conversion.Factor.Value != 0)
                                        {
                                            decimal dias = rec.TiempoTotal.Value.Value / conversion.Factor.Value.Value;
                                            if (dias < 100000)
                                                rec.FechaFin.Value = rec.FechaInicio.Value.Value.AddDays((double)dias);//Unidad estandar Dia
                                        }
                                    }
                                }
                                else
                                    rec.FechaFin.Value = rec.FechaInicio.Value;
                                #endregion 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "CalculateDates"));
            }
        }

        /// <summary>
        /// Restaura los valores iniciales
        /// </summary>
        /// <param name="p"></param>
        private void CleanHeader(bool p)
        {
            try
            {
                if (p)
                {
                    this.masterCliente.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterResponsableEmp.Value = string.Empty;
                    this.txtDescripcion.Text = string.Empty;
                    this.txtSolicitante.Text = string.Empty;
                    this.txtResposableCli.Text = string.Empty;
                    this.txtCorreo.Text = string.Empty;
                    this.txtTelefono.Text = string.Empty;
                    this.txtObservaciones.Text = string.Empty;
                    this.txtPorAdmEmp.EditValue = 0;
                    this.txtPorImprEmp.EditValue = 0;
                    this.txtPorUtilEmp.EditValue = 0;
                    this.txtPorAdmClient.EditValue = 0;
                    this.txtPorImprClient.EditValue = 0;
                    this.txtPorUtilClient.EditValue = 0;
                    this.txtCostoPresupuesto.EditValue = 0;
                    this.txtCostoCliente.EditValue = 0;
                    this.txtIVA.EditValue = 0;
                    this.cmbTipoSolicitud.EditValue = ((int)TipoSolicitud.Cotizacion);
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "CleanHeader"));
            }
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, false, true, true);
                this._bc.InitMasterUC(this.masterAreaFuncional, AppMasters.glAreaFuncional, true, true, true, false);
                this._bc.InitMasterUC(this.masterClaseServicio, AppMasters.pyClaseProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
                this._bc.InitMasterUC(this.masterResponsableEmp, AppMasters.seUsuario, false, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);
                this._bc.InitMasterUC(this.masterCentroCto, AppMasters.coCentroCosto, true, true, true, false);
                this._bc.InitMasterUC(this.masterUnidadBaseDia, AppMasters.inUnidad, true, true, true, true);
                #region Combos

                Dictionary<string, string> datosTipoSolicitud = new Dictionary<string, string>();
                datosTipoSolicitud.Add("1", "Implementación");
                datosTipoSolicitud.Add("2", "Desarrollo");
                datosTipoSolicitud.Add("3", "Investigación");
                datosTipoSolicitud.Add("4", "Obra");
                datosTipoSolicitud.Add("5", "Interventoria");
                this.cmbTipoSolicitud.Properties.ValueMember = "Key";
                this.cmbTipoSolicitud.Properties.DisplayMember = "Value";
                this.cmbTipoSolicitud.Properties.DataSource = datosTipoSolicitud;
                this.cmbTipoSolicitud.EditValue = "1";

                Dictionary<string, string> datosProposito = new Dictionary<string, string>();
                datosProposito.Add(((int)TipoSolicitud.Cotizacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Cotizacion));
                datosProposito.Add(((int)TipoSolicitud.Licitacion).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Licitacion));
                datosProposito.Add(((int)TipoSolicitud.Interna).ToString(), this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Interna));
                this.cmbProposito.Properties.ValueMember = "Key";
                this.cmbProposito.Properties.DisplayMember = "Value";
                this.cmbProposito.Properties.DataSource = datosProposito;
                this.cmbProposito.EditValue = ((int)TipoSolicitud.Cotizacion).ToString();

                Dictionary<string, string> datosJerarquia = new Dictionary<string, string>();
                datosJerarquia.Add("1", "Nivel 1 - Ej: 1");
                datosJerarquia.Add("2", "Nivel 2 - Ej: 1.1");
                datosJerarquia.Add("3", "Nivel 3 - Ej: 1.1.1");
                datosJerarquia.Add("4", "Nivel 4 - Ej: 1.1.1.1");
                datosJerarquia.Add("5", "Nivel 5 - Ej: 1.1.1.1.1");
                datosJerarquia.Add("6", "Nivel 6 - Ej: 1.1.1.1.1.1");

                this.cmbJerarquia.Properties.ValueMember = "Key";
                this.cmbJerarquia.Properties.DisplayMember = "Value";
                this.cmbJerarquia.Properties.DataSource = datosJerarquia;
                this.cmbJerarquia.EditValue = "1";

                Dictionary<string, string> datosReporte = new Dictionary<string, string>();
                datosReporte.Add("1", "Presupuesto");
                datosReporte.Add("2", "Presupuesto Cliente");
                datosReporte.Add("3", "Presupuesto Comparativo");
                datosReporte.Add("4", "A.P.U Detallado");
                datosReporte.Add("5", "Prueba Chart");

                this.cmbReporte.Properties.ValueMember = "Key";
                this.cmbReporte.Properties.DisplayMember = "Value";
                this.cmbReporte.Properties.DataSource = datosReporte;
                this.cmbReporte.EditValue = "1";

                Dictionary<string, string> datosTipoPresupuesto = new Dictionary<string, string>();
                datosTipoPresupuesto.Add("1", "Cliente");
                datosTipoPresupuesto.Add("2", "Interno");
                this.cmbTipoPresup.Properties.ValueMember = "Key";
                this.cmbTipoPresup.Properties.DisplayMember = "Value";
                this.cmbTipoPresup.Properties.DataSource = datosTipoPresupuesto;
                this.cmbTipoPresup.EditValue = "1";

                #endregion

                this.formatRecursos = _bc.GetImportExportFormat(typeof(DTO_ExportRecursosDeta), AppForms.MasterReportXls);
                this.formatTareas = _bc.GetImportExportFormat(typeof(DTO_ExportTareasTiempos), AppForms.MasterReportXls);
                this._tareaXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TareaDefecto);
                this._trabajoXDef = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_TrabajoDefecto);
                string IVAPresup = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcIVAUtilidad);
                string prestacionMO = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcPrestacionManoObra);
                this.masterUnidadBaseDia.Value = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_UnidadTiempoDia);

                if (!string.IsNullOrEmpty(IVAPresup))
                    this._IVAPresupuesto = Convert.ToDecimal(IVAPresup, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(prestacionMO))
                    this._porcPrestManoObra = Convert.ToDecimal(prestacionMO, CultureInfo.InvariantCulture);
                else
                    this._porcPrestManoObra = 100;
                //Deshabilita los botones +- de la grilla
                //this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;
                this.masterResponsableEmp.Value = (this._bc.AdministrationModel.User).ID.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Habilita o desabuilita el header
        /// </summary>
        /// <param name="p"></param>
        private void EnableHeader(bool p)
        {
            this.masterClaseServicio.EnableControl(p);
            this.masterPrefijo.EnableControl(p);
            this.masterAreaFuncional.EnableControl(p);
            this.masterCliente.EnableControl(p);
            this.masterCentroCto.EnableControl(p);
            this.masterResponsableEmp.EnableControl(p);
            this.txtNro.Enabled = p;
            this.cmbJerarquia.Enabled = p;
            this.chkAPUIncluyeAIU.Enabled = p;
            this.dtFechaInicio.Enabled = p;
            this.txtPorAdmClient.Enabled = p;
            this.txtPorImprClient.Enabled = p;
            this.txtPorUtilClient.Enabled = p;
            this.txtPorAdmEmp.Enabled = p;
            this.txtPorImprEmp.Enabled = p;
            this.txtPorUtilEmp.Enabled = p;
            this.txtResposableCli.Enabled = p;
            this.txtCorreo.Enabled = p;
            this.txtTelefono.Enabled = p;
            this.cmbProposito.Enabled = p;
            this.cmbTipoSolicitud.Enabled = p;
        }

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

                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.py_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrid(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    this.gcDocument.DataSource = this._listTareasAll;
                    this.gcDocument.RefreshDataSource();
                    this.gcRecurso.DataSource = null;
                    this.gcRecurso.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del documento
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                int? docNro = null;
                if (!string.IsNullOrEmpty(this.txtNro.Text)) docNro = Convert.ToInt32(this.txtNro.Text);

                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(this.documentID, this.masterPrefijo.Value, docNro, null, string.Empty, this.masterProyecto.Value, true,false,false,false);
                if (transaccion != null)
                {
                    this._ctrl = transaccion.DocCtrl;
                    this._preProyectoDocu = transaccion.Header;
                    this._dtoClaseServicio = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false, transaccion.Header.ClaseServicioID.Value, true);
                    this._listTareasAll = transaccion.Detalle;
                    this.CargarInformacion(true);
                    foreach (var tarea in _listTareasAll)
                        this.CalculateValues(tarea, true);
                    this._listTareasAdicion = transaccion.DetalleTareasAdic;
                    this.UpdateValues();
                    this._numeroDoc = this._ctrl.NumeroDoc.Value != null ? this._ctrl.NumeroDoc.Value.Value : 0;
                    this.LoadGrid(true);
                    this.EnableHeader(this._numeroDoc != 0 ? false : true);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, "No Existe")); 
                    this._ctrl = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempoProy", "LoadGrid"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {
            this.masterPrefijo.Value = string.Empty;
            this.masterAreaFuncional.Value = string.Empty;
            this.masterClaseServicio.Value = string.Empty;
            this.txtNro.Text = string.Empty;
            this.CleanHeader(true);
            this.EnableHeader(true);

            this._ctrl = null;
            this._numeroDoc = 0;
            this._preProyectoDocu = new DTO_pyPreProyectoDocu();
            //this._rowCapituloCurrent = new DTO_CapituloSolicitud();
            this._rowTareaCurrent = new DTO_pyPreProyectoTarea();
            this._rowRecursoCurrent = new DTO_pyPreProyectoDeta();
            this._listTareasAll = new List<DTO_pyPreProyectoTarea>();
            this._listTareasAdicion = new List<DTO_pyPreProyectoTarea>();
            this._dtoClaseServicio = null;
            this._tareaIsFocused = false;
            this._totalRecxTarea = 0;
            this._totalPresupuesto = 0;
            this._totalCliente = 0;
            this._trabajoCurrent = string.Empty;
            this.gvRecursoCurrent = new GridView();
            this.gcDocument.DataSource = this._listTareasAll;
            this.gcDocument.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private  void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.PlaneacionTiempo;
            this.AddGridCols();
            this.InitControls();
            this.endImportarDelegate = new EndImportar(this.EndImportarMethod);

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
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
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private  bool ValidateRow(int fila)
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
        private  bool ValidGridCell(int fila, string colName, bool acceptNull, bool isFK, bool isHierarchy, int? FKDocID)
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
        private  bool ValidGridCellValue(int fila, string colName, bool acceptNull, bool acceptCero, bool OnlyPositive, bool invalidImp)
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
        /// Valida el Header
        /// </summary>
        /// <returns></returns>
        private string ValidateHeader()
        {
            string camposObligatorios = string.Empty;

            if (string.IsNullOrEmpty(this.masterAreaFuncional.Value))
                camposObligatorios = camposObligatorios + this.masterAreaFuncional.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.masterClaseServicio.Value))
                camposObligatorios = camposObligatorios + this.masterClaseServicio.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.masterCentroCto.Value))
                camposObligatorios = camposObligatorios + this.masterCentroCto.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.txtSolicitante.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblSolicitante.Text) + "\n";
            if (string.IsNullOrEmpty(this.masterResponsableEmp.Value))
                camposObligatorios = camposObligatorios + this.masterResponsableEmp.LabelRsx + "\n";
            if (string.IsNullOrEmpty(this.txtResposableCli.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblResponsableCli.Text) + "\n";
            if (string.IsNullOrEmpty(this.txtCorreo.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblCorreo.Text) + "\n";
            if (string.IsNullOrEmpty(this.txtTelefono.Text))
                camposObligatorios = camposObligatorios + this._bc.GetResource(LanguageTypes.Forms, lblTelefono.Text) + "\n";
            return camposObligatorios;
        }

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dtoInsumo">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgFecha">Mensaje que indica que la fecha esta en un periodo incorrecto</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un valor de cliente, codigo de empleado o libranza</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msgClienteRepetido">Mensaje para cliente repetido sin libranza</param>
        /// <param name="msgCodCliente">Mensaje para indicar que no se puede poner el codigo y el cliente</param>
        private void ValidateDataImport(DTO_ExportTareasTiempos dtoTarea, DTO_ExportRecursosDeta recurso, DTO_TxResultDetail rd, string msgFkNotFound, string msgEmptyField)
        {
            try
            {
                if (dtoTarea != null)
                {                    
                    #region Validacion Tarea
                    if (string.IsNullOrWhiteSpace(dtoTarea.TareaCliente.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Fecha Inicio
                    if (dtoTarea.FechaInicio.Value == null && dtoTarea.FechaFin.Value != null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaInicio");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Fecha Fin
                    if (dtoTarea.FechaFin.Value == null && dtoTarea.FechaInicio.Value != null)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaFin");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Consistencia Fechas
                    if (dtoTarea.FechaInicio.Value != null && dtoTarea.FechaFin.Value != null)
                    {
                        if(dtoTarea.FechaInicio.Value > dtoTarea.FechaFin.Value)
                        {
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FechaFin");
                            rdF.Message = "La fecha de Inicio debe ser inferior a la fecha Fin de la tarea, verifique";
                            rd.DetailsFields.Add(rdF);
                        }                      
                    }
                    #endregion
                }
                else if (recurso != null)
                {
                    #region Validacion Tarea
                    if (string.IsNullOrWhiteSpace(recurso.TareaCliente.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    else if (this._tareaIsFocused && recurso.TareaCliente.Value != this._rowTareaCurrent.TareaID.Value)
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID"); ;
                        rdF.Message = this._bc.GetResource(LanguageTypes.Forms, DictionaryMessages.Py_TareaInvalid);
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion                    
                    #region Validacion Trabajo
                    if (string.IsNullOrWhiteSpace(recurso.TareaID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TrabajoID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion Recurso
                    if (string.IsNullOrWhiteSpace(recurso.RecursoID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                    #region Validacion FactorID
                    if (string.IsNullOrWhiteSpace(recurso.FactorID.Value.ToString()))
                    {
                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                        rdF.Field = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                        rdF.Message = msgEmptyField;
                        rd.DetailsFields.Add(rdF);
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// Actualiza los costos generales
        /// </summary>
        private void UpdateValues()
        {
            try
            {
                this._totalPresupuesto = 0;
                this._totalCliente = 0;
                this._totalRecxTarea = 0;
                foreach (DTO_pyPreProyectoTarea tarea in this._listTareasAll.FindAll(x => x.DetalleInd.Value.Value))
                {
                    this._totalPresupuesto += tarea.CostoTotalML.Value.Value;
                    this._totalCliente += tarea.CostoLocalCLI.Value.Value;
                }
                //Actualiza valores APU
                //this.txtCostoAPU.EditValue = this._rowTareaCurrent.CostoTotalUnitML.Value;
                //this.txtCostoAIUxAPU.EditValue = this._rowTareaCurrent.VlrAIUxAPUAdmin.Value + this._rowTareaCurrent.VlrAIUxAPUImpr.Value + this._rowTareaCurrent.VlrAIUxAPUUtil.Value;
                //this.txtCostoTotalAPU.EditValue = this._rowTareaCurrent.CostoTotalUnitML.Value + this._rowTareaCurrent.VlrAIUxAPUAdmin.Value +
                //                                    this._rowTareaCurrent.VlrAIUxAPUImpr.Value + this._rowTareaCurrent.VlrAIUxAPUUtil.Value;

                //Actualiza valores generales
                this.txtCostoPresupuesto.EditValue = this._totalPresupuesto;
                this.txtCostoCliente.EditValue = this._totalCliente;
                decimal vlrUtilidad = Math.Round((this._totalPresupuesto * Convert.ToDecimal(this.txtPorUtilEmp.EditValue, CultureInfo.InvariantCulture)) / 100, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemImport.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemSendtoAppr.ToolTipText = this._bc.GetResource(LanguageTypes.ToolBar, "acc_approve");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtPeriod_EditValueChanged()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlanecionTiempo", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtFecha_Enter(object sender, EventArgs e) { }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void dtFecha_Leave(object sender, EventArgs e)
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

        #endregion

        #region Eventos Header

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.PreProyecto);
            ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtNro.Enabled = true;
                this.txtNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtNro.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterAreaFuncional_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.masterAreaFuncional.Value))
            {
                string prefijo = this._bc.GetPrefijo(this.masterAreaFuncional.Value, this.documentID);
                this.masterPrefijo.Value = prefijo;
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, System.EventArgs e)
        {
            if (this.masterCliente.ValidID)
            {
                DTO_faCliente cliente = (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this.masterCliente.Value, true);
                if (cliente != null)
                    this._preProyectoDocu.ClienteDesc.Value = cliente.Descriptivo.Value;
            }
        }

        /// <summary>
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            if(this.masterPrefijo.ValidID)
                this.LoadData();            
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterClaseServicio_Leave(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterProyecto_Load(object sender, EventArgs e)
        {
            if(this.masterProyecto.ValidID)
                this.LoadData();
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkRecursoXTrabInd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkRecursoXTrabInd.Checked)
                {
                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].VisibleIndex = 0;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaInicio"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaFin"].VisibleIndex = 5;

                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaInicio"].OptionsColumn.AllowEdit = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaFin"].OptionsColumn.AllowEdit = false;

                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocal"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadTOT"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocalTOT"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoTotalML"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TiempoTotal"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TipoRecurso"].UnGroup();
                   
                    this.gcRecurso.DataSource = null;
                    this.AssignRecursos();
                }
                else
                {
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 0;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "TiempoTotal"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaInicio"].VisibleIndex = 6;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaFin"].VisibleIndex = 7;

                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "FactorID"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitud"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "TiempoTotal"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaInicio"].OptionsColumn.AllowEdit = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "FechaFin"].OptionsColumn.AllowEdit = true;

                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocal"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoTotalML"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TrabajoID"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CostoLocalTOT"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "Cantidad"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "TipoRecurso"].Group();
                    this.gcRecurso.DataSource = null;
                    this.AssignRecursos();
                }
            }
            catch (Exception ex)
            {               
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "chkRecursoXTrabInd_CheckedChanged"));
            }
        }

        /// <summary>
        /// Al poner el mouse sobre el control
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void txt_MouseHover(object sender, EventArgs e)
        {
            try
            {
                MemoExEdit memo = (MemoExEdit)sender;
                switch (memo.Name)
                {
                    case "txtLicitacion":
                        this.txtLicitacion.ToolTip = memo.Text;
                        break;
                    case "txtDescripcion":
                        this.txtDescripcion.ToolTip = memo.Text;
                        break;
                    case "txtObservaciones":
                        this.txtObservaciones.ToolTip = memo.Text;
                        break;
                    case "txtSolicitante":
                        this.txtSolicitante.ToolTip = memo.Text;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "txt_MouseHover: " + ex.Message));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar el area funcional 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoPresup_EditValueChanged(object sender, EventArgs e)
        {
            if (this._dtoClaseServicio != null && this._dtoClaseServicio.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
            {
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].UnGroup();
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = false;

            }
            else if (this._dtoClaseServicio != null) // Interno
            {
                //this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].Group();
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortOrder = ColumnSortOrder.None;
                this.gvDocument.Columns[this.unboundPrefix + "CapituloTareaID"].SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "TareaID"].Visible = true;
                //this.gvDocument.Columns[this.unboundPrefix + "TareaCliente"].Visible = false;
            }
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSortByCap_Click(object sender, EventArgs e)
        {
            try
            {
                this._listTareasAll = this._listTareasAll.OrderBy(x => x.CapituloTareaID.Value).ToList();
                int count = 1;
                foreach (var t in this._listTareasAll)
                {
                    t.TareaCliente.Value = count.ToString();
                    count++;
                }
                this.gcDocument.DataSource = this._listTareasAll;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "txtSortByCap_Click"));
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
                    this._rowTareaCurrent = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.FocusedRowHandle);
                    this._tareaIsFocused = true;
                    this.AssignRecursos();
                    this.UpdateValues();
                }
                else
                {
                    this.gcRecurso.DataSource = null;
                    this.gcRecurso.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDocument_FocusedRowChanged"));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
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
                GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];

                this.gvDocument = (GridView)sender;
                this._rowTareaCurrent = (DTO_pyPreProyectoTarea)gvDocument.GetRow(e.RowHandle);
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
                    }
                }
                if (fieldName == "Cantidad")
                {
                    if (e.Value != null)
                    {
                        this.CalculateValues(this._rowTareaCurrent);
                        this.UpdateValues();
                    }
                }
                else if (fieldName == "CostoTotalML")
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.UpdateValues();
                }
                else if (fieldName == "CostoLocalCLI")
                {
                    //this._rowCapituloCurrent.Detalle[this._rowTareaCurrent.Index].CostoDiferenciaML.Value = this._rowTareaCurrent.CostoTotalML.Value - Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                    this.gvDocument.RefreshData();
                    this.UpdateValues();
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
        private void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            try
            {
                if (fieldName == "TareaCliente")
                {
                    if (!e.Value.ToString().EndsWith("."))
                        this.CalculateHierarchy(this._rowTareaCurrent, e.Value.ToString());
                }
  
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDocument_CellValueChanging"));
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
                if (this.gvDocument.DataRowCount > 0)
                {
                    GridColumn colIni = this.gvDocument.Columns[this.unboundPrefix + "FechaInicio"];
                    GridColumn colFin = this.gvDocument.Columns[this.unboundPrefix + "FechaFin"];

                    #region Validacion Consistencia Fechas
                    if (this._rowTareaCurrent.FechaInicio.Value != null && this._rowTareaCurrent.FechaFin.Value == null)
                    {
                        this.gvDocument.SetColumnError(colFin, "Digite una fecha");
                        e.Allow = false;
                    }
                    else if (this._rowTareaCurrent.FechaInicio.Value == null && this._rowTareaCurrent.FechaFin.Value != null)
                    {
                        this.gvDocument.SetColumnError(colIni, "Digite una fecha");
                        e.Allow = false;
                    }
                    else if (this._rowTareaCurrent.FechaInicio.Value != null && this._rowTareaCurrent.FechaFin.Value != null)
                    {
                        if (this._rowTareaCurrent.FechaInicio.Value > this._rowTareaCurrent.FechaFin.Value)
                        {
                            this.gvDocument.SetColumnError(colFin, "La fecha de Inicio debe ser inferior a la fecha Fin de la tarea, verifique");
                            e.Allow = false;
                        }
                    }
                    else
                    {
                        this.gvDocument.ClearColumnErrors();
                        e.Allow = true;
                    }
                    #endregion

                    if (e.Allow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "UDT_Cantidad" || pi.PropertyType.Name == "UDT_Valor")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                                pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "UDT_Cantidad" || pi.PropertyType.Name == "UDT_Valor")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "UDT_Cantidad" || pi.PropertyType.Name == "UDT_Valor")
                            e.Value = pi.GetValue(dto, null);
                        else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
                        {
                            UDT udtProp = (UDT)pi.GetValue(dto, null);
                            udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
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
                                pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "UDT_Cantidad" || pi.PropertyType.Name == "UDT_Valor")
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
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                DTO_pyPreProyectoTarea currentRow = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                    if (currentRow.DetalleInd.Value.Value)
                        e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    else
                        e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
        private void gvDocument_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "CapituloTareaID" && e.IsForGroupRow)
                {
                    DTO_pyPreProyectoTarea row = (DTO_pyPreProyectoTarea)this.gvDocument.GetRow(e.ListSourceRowIndex);
                    e.DisplayText = e.Value.ToString() + "  " + row.CapituloDesc.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvRecurso_CustomColumnDisplayText"));
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
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "editLink_Click"));
            }
        }

        #endregion

        #region Recurso-Trabajo

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.gvRecurso.DataRowCount > 0)
                {
                    this._trabajoCurrent = this.gvRecurso.GetRowCellValue(e.FocusedRowHandle, this.unboundPrefix + "TrabajoID").ToString();

                    if (this.chkRecursoXTrabInd.Checked)
                        this._rowRecursoCurrent = (DTO_pyPreProyectoDeta)this.gvRecursoCurrent.GetRow(e.FocusedRowHandle);
                    else
                        this._rowRecursoCurrent = (DTO_pyPreProyectoDeta)this.gvRecurso.GetRow(e.FocusedRowHandle);

                    this.txtModelo.EditValue = this._rowRecursoCurrent.Modelo.Value;
                    this.txtMarca.EditValue = this._rowRecursoCurrent.MarcaDesc.Value;
                }

                if (this.gvRecurso.FocusedRowHandle == 0)
                    this._assignFechaInicio = true;
                else
                    this._assignFechaInicio = false; 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvRecurso.Columns[this.unboundPrefix + fieldName];

                if (fieldName == "CantSolicitud" && !this.chkRecursoXTrabInd.Checked && e.RowHandle == this.gvRecurso.FocusedRowHandle) //&& this._assignFechaInicio)
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.CalculateDates(false);
                    this.gvRecursoCurrent.RefreshData();
                    this.gvRecurso.RefreshData();
                    this.gvDocument.RefreshData();
                }
                if (fieldName == "FechaInicio" && !this.chkRecursoXTrabInd.Checked && e.RowHandle == this.gvRecurso.FocusedRowHandle)// && this._assignFechaInicio)
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.CalculateDates(false);
                    this.gvRecursoCurrent.RefreshData();
                    this.gvRecurso.RefreshData();
                    this.gvDocument.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvRecurso_CellValueChanged"));
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
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = "HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = "SOFTWARE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        #endregion

        #region Detalle Recurso

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.gvRecursoCurrent = (GridView)sender;
                if (e.FocusedRowHandle >= 0 && this.gvRecursoCurrent.DataRowCount > 0)
                    this._rowRecursoCurrent = (DTO_pyPreProyectoDeta)this.gvRecursoCurrent.GetRow(e.FocusedRowHandle);
                if (this.gvRecursoCurrent.FocusedRowHandle == 0)
                    this._assignFechaInicio = true;
                else
                    this._assignFechaInicio = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudServicio.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleRecurso_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvDetalleRecurso.Columns[this.unboundPrefix + fieldName];

                if (fieldName == "CantSolicitud")
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.CalculateDates(false);
                    this.gvRecursoCurrent.RefreshData();
                    this.gvRecurso.RefreshData();
                    this.gvDocument.RefreshData();
                }

                if (fieldName == "FechaInicio" && e.RowHandle == this.gvRecurso.FocusedRowHandle ) //&& this._assignFechaInicio)
                {
                    this.CalculateValues(this._rowTareaCurrent);
                    this.CalculateDates(false);
                    this.gvRecursoCurrent.RefreshData();
                    this.gvRecurso.RefreshData();
                    this.gvDocument.RefreshData();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDetalleRecurso_CellValueChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
                    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "MATERIALES";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "EQUIPO";
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = "MANO DE OBRA";
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = "TRANSPORTES";
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = "HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = "SOFTWARE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "gvDetalleRecurso_CustomColumnDisplayText"));
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
            this.gvDocument.PostEditor();
            string camposObligatorios = this.ValidateHeader();
            if (string.IsNullOrEmpty(camposObligatorios))
            {
                this.CargarInformacion(false);
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            else
                MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios));
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
            string msgAprobar = string.Format(msgDoc, "Aprobar");

            if (MessageBox.Show(msgAprobar, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string camposObligatorios = string.Empty;
                if (!this.masterCliente.ValidID)
                    camposObligatorios = this.masterCliente.LabelRsx + "\n";
                if (!this.masterCentroCto.ValidID)
                    camposObligatorios = camposObligatorios + this.masterCentroCto.LabelRsx + "\n";
                if (!this.masterProyecto.ValidID)
                    camposObligatorios = camposObligatorios + this.masterProyecto.LabelRsx + "\n";

                if (string.IsNullOrEmpty(camposObligatorios))
                {
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios));
                }
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBImport()
        {
            if (this.masterClaseServicio.ValidID)
            {
                this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.formatTareas);
                Thread process = new Thread(this.ImportThreadTareas);
                process.Start();
            }
            else
            {
                MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterClaseServicio.LabelRsx));
            }
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
                string[] cols = this.formatTareas.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
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
        public override void TBExport()
        {
            try
            {
                if (this.masterClaseServicio.ValidID)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    List<DTO_ExportTareasTiempos> tmp = new List<DTO_ExportTareasTiempos>();
                    foreach (DTO_pyPreProyectoTarea tarea in this._listTareasAll)
                    {
                        DTO_ExportTareasTiempos ex = new DTO_ExportTareasTiempos();
                        ex.TareaCliente.Value = tarea.TareaCliente.Value;
                        ex.Descripcion.Value = tarea.Descriptivo.Value;
                        ex.UnidadInv.Value = tarea.UnidadInvID.Value;
                        ex.FechaInicio.Value = tarea.FechaInicio.Value;
                        ex.FechaFin.Value = tarea.FechaFin.Value;
                        tmp.Add(ex);
                    }
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportTareasTiempos), tmp);
                    System.Data.DataTable tableExport = new System.Data.DataTable();

                    ReportExcelBase frm = new ReportExcelBase(tableAll);
                    frm.Show();
                }
                else
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterClaseServicio.LabelRsx));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "TBExport"));
            }
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
                this.CargarInformacion(false);
                solicitud.Detalle = ObjectCopier.Clone(this._listTareasAll);
                solicitud.DetalleTareasAdic = ObjectCopier.Clone(this._listTareasAdicion);
                solicitud.DocCtrl = this._ctrl;
                solicitud.Header = ObjectCopier.Clone(this._preProyectoDocu);
                string reportName = this._bc.AdministrationModel.Reportes_py_PlaneacionCostos(solicitud, false, Convert.ToByte(this.cmbReporte.EditValue), null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "TBPrint"));
            }
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

                if (this._preProyectoDocu.NumeroDoc.Value != null && this._preProyectoDocu.NumeroDoc.Value != 0)
                    this._numeroDoc = this._preProyectoDocu.NumeroDoc.Value.Value;

                DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                transaccion.Header = ObjectCopier.Clone(this._preProyectoDocu);
                transaccion.Detalle = ObjectCopier.Clone(this._listTareasAll);
                transaccion.DetalleTareasAdic = ObjectCopier.Clone(this._listTareasAdicion);

                result = this._bc.AdministrationModel.SolicitudProyecto_Add(this.documentID, ref this._numeroDoc,
                         this.masterClaseServicio.Value, this.masterAreaFuncional.Value, this.masterPrefijo.Value,
                         this.masterProyecto.Value, this.masterCentroCto.Value, this.txtObservaciones.Text, transaccion);

                if (this._numeroDoc == 0 && result.Result == ResultValue.OK)
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_SuccessSol), this.masterPrefijo.Value, result.ExtraField));
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudProye.cs", "SaveThread"));
            }
            finally
            {
                //if (result.Result != ResultValue.NOK)
                //    this.Invoke(this.saveDelegate);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Envia al paso de Analisis de Tiempos
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result;
                if (this._numeroDoc == 0)
                    result = resultNOK;
                else
                {
                    DTO_SolicitudTrabajo transaccion = new DTO_SolicitudTrabajo();
                    transaccion.DocCtrl = ObjectCopier.Clone(this._ctrl);
                    transaccion.Header = ObjectCopier.Clone(this._preProyectoDocu);
                    transaccion.Detalle = ObjectCopier.Clone(this._listTareasAll);
                    transaccion.DetalleTareasAdic = ObjectCopier.Clone(this._listTareasAdicion);
                    result = _bc.AdministrationModel.SolicitudProyecto_AprobarProy(this.documentID, transaccion, this.dtFechaInicio.DateTime);
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                if (isOK)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    this.deleteOP = true;
                    this.Invoke(this.sendToApproveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PlaneacionTiempo.cs-SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public void ImportThreadTareas()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.Details = new List<DTO_TxResultDetail>();
            List<DTO_ExportTareasTiempos> listImport = new List<DTO_ExportTareasTiempos>();
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    //Lista con los dtos a subir y Fks a validas                    
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoRel);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFecha = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDate);


                    //Popiedades
                    DTO_ExportTareasTiempos tarea = new DTO_ExportTareasTiempos();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.formatTareas.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_ExportTareasTiempos).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, AppForms.MasterReportXls.ToString() + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    //Fks
                    //fks.Add(this._clienteRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            tarea = new DTO_ExportTareasTiempos();
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = tarea.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(tarea, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
                                                if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                                {
                                                    try
                                                    {
                                                        DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                                {
                                                    try
                                                    {
                                                        decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                        if (colValue.Trim().Contains(','))
                                                        {
                                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                            rdF.Field = colRsx;
                                                            rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                            rd.DetailsFields.Add(rdF);

                                                            createDTO = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PlaneacionTiempo.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                listImport.Add(tarea);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la migracion
                    if (validList)
                    {
                        #region Variables generales
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        //percent = 0;

                        #endregion
                        foreach (DTO_ExportTareasTiempos dto in listImport)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            //this.Invoke(this.UpdateProgressDelegate, new object[] { percent });
                            //percent = ((i + 1) * 100) / (listImport.Count);
                            i++;

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;
                            #endregion
                            #region Validaciones particulares del DTO
                            this.ValidateDataImport(dto, null, rd, msgFkNotFound, msgEmptyField);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                            }
                            #endregion
                        }
                        if (result.Details.Count > 0)
                        {
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }

                    }
                    #endregion
                    if (validList)
                    {
                        int indexTarea = 0;
                        foreach (var tiempo in listImport)
                        {
                            DTO_pyPreProyectoTarea tar = this._listTareasAll.Find(x=>x.TareaCliente.Value == tiempo.TareaCliente.Value);
                            if (tar != null)
                            {
                                tar.FechaInicio.Value = tiempo.FechaInicio.Value;
                                tar.FechaFin.Value = tiempo.FechaFin.Value;
                            }                          
                        }
                    }
                }
                else
                {
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = this.pasteRet.MsgResult;
                }
                #region Actualiza la información de la grilla
                if (result.Result == ResultValue.OK)
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(this.endImportarDelegate);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
                else
                {
                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    listImport = new List<DTO_ExportTareasTiempos>();
                }
                FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo.cs", "ImportThreadTareas"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion                
    }
}
