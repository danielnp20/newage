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
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalAPUProyectos : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_pyPreProyectoDeta> _listDetRecursosAll = null;
        private DTO_pyPreProyectoDeta detCurrent = new DTO_pyPreProyectoDeta();
        private DTO_pyPreProyectoTarea _tareaCurrent = new DTO_pyPreProyectoTarea();
        private string unboundPrefix = "Unbound_";
        private int _documentID;
        private bool _APUIncluyeAIUInd = false;
        private bool _isAPUCliente = false;
        private decimal _porcPrestManoObra = 1;
        private decimal _tasaCambio = 0;
        private decimal _porIVA = 0;
        private byte _tipoRedondeo = 1;
        private List<int> _recursosDelete = new List<int>();
        private string _lugarGeoxDef = string.Empty;
        private GridView gridCurrent = new GridView();
        private bool _equipoCantidadInd = false;
        private bool _personalCantidadInd = false;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalAPUProyectos()
        {
           // this.InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalAPUProyectos(DTO_pyPreProyectoTarea tarea, bool AIUInd, decimal porcAdmin, decimal porcImpr, decimal porcUtil, decimal tasaCambio, 
                                 byte tipoRedondeo, bool equipoCantInd,bool personalCantInd, decimal porcIVA, bool isAPUCliente = false)
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();
                this._APUIncluyeAIUInd = AIUInd;
                this._tareaCurrent = tarea;
                this._isAPUCliente = isAPUCliente;
                this._tasaCambio = tasaCambio;
                this._tipoRedondeo = tipoRedondeo;
                this._equipoCantidadInd = equipoCantInd;
                this._personalCantidadInd = personalCantInd;
                if (!this._isAPUCliente)
                    this._listDetRecursosAll = tarea.Detalle;
                else
                    this._listDetRecursosAll = tarea.DetalleAPUCliente;
                this.txtPorcAdmin.EditValue = porcAdmin;
                this.txtPorcImpr.EditValue = porcImpr;
                this.txtPorcUtil.EditValue = porcUtil;
                this._porIVA = porcIVA;
                this.InitControls();               
                this.Text = this.Text + "-"+tarea.TareaCliente.Value +"-" + tarea.Descriptivo.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalAPUProyectoss.cs", "ModalAPUProyectoss")); ;
            }
        }      

        #region Funciones privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        protected virtual void InitControls()
        {
            try
            {
                FormProvider.LoadResources(this, this._documentID);

                this._lugarGeoxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
                string prestacionMO = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_PorcPrestacionManoObra);
                if (!string.IsNullOrEmpty(prestacionMO) && !prestacionMO.Equals("0"))
                    this._porcPrestManoObra = Convert.ToDecimal(prestacionMO, CultureInfo.InvariantCulture)/100;

                this.LoadDataGrids();

                this.gcEquipo.RefreshDataSource();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectoss.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        protected virtual void SetInitParameters()
        {
            this._documentID = AppForms.ModalRecursosTrabajo;
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            try
            {
                #region Grilla Equipo
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 0;
                RecursoID.Width = 60;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = true;
                this.gvEquipo.Columns.Add(RecursoID);

                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 1;
                RecursoDesc.Width = 200;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                this.gvEquipo.Columns.Add(RecursoDesc);

                GridColumn UnidadInvIDrec = new GridColumn();
                UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrec.UnboundType = UnboundColumnType.String;
                UnidadInvIDrec.VisibleIndex = 2;
                UnidadInvIDrec.Width = 60;
                UnidadInvIDrec.Visible = true;
                UnidadInvIDrec.OptionsColumn.AllowEdit = false;
                this.gvEquipo.Columns.Add(UnidadInvIDrec);

                // Cant x Rec
                GridColumn CantidadRec = new GridColumn();
                CantidadRec.FieldName = this.unboundPrefix + "Cantidad";
                CantidadRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
                CantidadRec.UnboundType = UnboundColumnType.Decimal;
                CantidadRec.VisibleIndex = 5;
                CantidadRec.Width = 80;
                CantidadRec.Visible = false;
                CantidadRec.ColumnEdit = this.editValue2Cant;
                CantidadRec.OptionsColumn.AllowEdit = false;
                this.gvEquipo.Columns.Add(CantidadRec);

                //Costo Unit
                GridColumn CostoLocal = new GridColumn();
                CostoLocal.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocal.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocal.UnboundType = UnboundColumnType.Decimal;
                CostoLocal.VisibleIndex = 6;
                CostoLocal.Width = 80;
                CostoLocal.Visible = true;
                CostoLocal.ColumnEdit = this.editSpin;
                //CostoLocal.ColumnEdit = this.editBtnUpdate;
                CostoLocal.OptionsColumn.AllowEdit = true;
                this.gvEquipo.Columns.Add(CostoLocal);

                //Cant x 
                GridColumn FactorID = new GridColumn();
                FactorID.FieldName = this.unboundPrefix + "FactorID";
                FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorID.UnboundType = UnboundColumnType.Decimal;
                FactorID.VisibleIndex = 7;
                FactorID.Width = 80;
                FactorID.Visible = true;
                FactorID.ColumnEdit = this.editValue8Cant;
                FactorID.OptionsColumn.AllowEdit = true;
                this.gvEquipo.Columns.Add(FactorID);              

                //Cant X Trabajo
                GridColumn CanSolicitud = new GridColumn();
                CanSolicitud.FieldName = this.unboundPrefix + "CantSolicitud";
                CanSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
                CanSolicitud.UnboundType = UnboundColumnType.Decimal;
                CanSolicitud.VisibleIndex = 8;
                CanSolicitud.Width = 80;
                CanSolicitud.Visible = false;
                CanSolicitud.ColumnEdit = this.editValue2Cant;
                CanSolicitud.OptionsColumn.AllowEdit = true;
                this.gvEquipo.Columns.Add(CanSolicitud);


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
                this.gvEquipo.Columns.Add(CostoLocalTOT);

                GridColumn CostoTotalMLRec = new GridColumn();
                CostoTotalMLRec.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalMLRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoTotalMLRec.UnboundType = UnboundColumnType.Decimal;
                CostoTotalMLRec.VisibleIndex = 12;
                CostoTotalMLRec.Width = 80;
                CostoTotalMLRec.Visible = false;
                CostoTotalMLRec.ColumnEdit = this.editSpin;
                CostoTotalMLRec.OptionsColumn.AllowEdit = false;
                this.gvEquipo.Columns.Add(CostoTotalMLRec);

                GridColumn TipoRecurso = new GridColumn();
                TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecurso.UnboundType = UnboundColumnType.Integer;
                TipoRecurso.Width = 80;
                TipoRecurso.Visible = false;
                this.gvEquipo.Columns.Add(TipoRecurso);
                #endregion
                #region Grilla Material
                GridColumn RecursoIDMaterial = new GridColumn();
                RecursoIDMaterial.FieldName = this.unboundPrefix + "RecursoID";
                RecursoIDMaterial.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoIDMaterial.UnboundType = UnboundColumnType.String;
                RecursoIDMaterial.VisibleIndex = 0;
                RecursoIDMaterial.Width = 60;
                RecursoIDMaterial.Visible = true;
                RecursoIDMaterial.OptionsColumn.AllowEdit = true;
                this.gvMateriales.Columns.Add(RecursoIDMaterial);

                GridColumn RecursoDescMat = new GridColumn();
                RecursoDescMat.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDescMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDescMat.UnboundType = UnboundColumnType.String;
                RecursoDescMat.VisibleIndex = 1;
                RecursoDescMat.Width = 200;
                RecursoDescMat.Visible = true;
                RecursoDescMat.OptionsColumn.AllowEdit = false;
                this.gvMateriales.Columns.Add(RecursoDescMat);

                GridColumn UnidadInvIDMat = new GridColumn();
                UnidadInvIDMat.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDMat.UnboundType = UnboundColumnType.String;
                UnidadInvIDMat.VisibleIndex = 2;
                UnidadInvIDMat.Width = 60;
                UnidadInvIDMat.Visible = true;
                UnidadInvIDMat.OptionsColumn.AllowEdit = false;
                this.gvMateriales.Columns.Add(UnidadInvIDMat);                

                // Cant x Rec
                GridColumn CantidadRecMat = new GridColumn();
                CantidadRecMat.FieldName = this.unboundPrefix + "Cantidad";
                CantidadRecMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
                CantidadRecMat.UnboundType = UnboundColumnType.Decimal;
                CantidadRecMat.VisibleIndex = 5;
                CantidadRecMat.Width = 80;
                CantidadRecMat.Visible = false;
                CantidadRecMat.ColumnEdit = this.editValue2Cant;
                CantidadRecMat.OptionsColumn.AllowEdit = false;
                this.gvMateriales.Columns.Add(CantidadRecMat);

                //Costo Unit
                GridColumn CostoLocalMat = new GridColumn();
                CostoLocalMat.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocalMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocalMat.UnboundType = UnboundColumnType.Decimal;
                CostoLocalMat.VisibleIndex = 6;
                CostoLocalMat.Width = 80;
                CostoLocalMat.Visible = true;
                CostoLocalMat.ColumnEdit = this.editSpin;
                //CostoLocalMat.ColumnEdit = this.editBtnUpdate;
                CostoLocalMat.OptionsColumn.AllowEdit = true;
                this.gvMateriales.Columns.Add(CostoLocalMat);

                //Cant x 
                GridColumn FactorIDMat = new GridColumn();
                FactorIDMat.FieldName = this.unboundPrefix + "FactorID";
                FactorIDMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorIDMat.UnboundType = UnboundColumnType.Decimal;
                FactorIDMat.VisibleIndex = 7;
                FactorIDMat.Width = 80;
                FactorIDMat.Visible = true;
                FactorIDMat.ColumnEdit = this.editValue8Cant;
                FactorIDMat.OptionsColumn.AllowEdit = true;
                this.gvMateriales.Columns.Add(FactorIDMat);

                //Cant X Trabajo
                GridColumn CanSolicitudMat = new GridColumn();
                CanSolicitudMat.FieldName = this.unboundPrefix + "CantSolicitud";
                CanSolicitudMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
                CanSolicitudMat.UnboundType = UnboundColumnType.Decimal;
                CanSolicitudMat.VisibleIndex = 8;
                CanSolicitudMat.Width = 80;
                CanSolicitudMat.Visible = false;
                CanSolicitudMat.ColumnEdit = this.editValue2Cant;
                CanSolicitudMat.OptionsColumn.AllowEdit = true;
                this.gvMateriales.Columns.Add(CanSolicitudMat);
             

                GridColumn CostoLocalTOTMat = new GridColumn();
                CostoLocalTOTMat.FieldName = this.unboundPrefix + "CostoLocalTOT";
                CostoLocalTOTMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
                CostoLocalTOTMat.UnboundType = UnboundColumnType.Decimal;
                CostoLocalTOTMat.VisibleIndex = 10;
                CostoLocalTOTMat.Width = 80;
                CostoLocalTOTMat.Visible = true;
                CostoLocalTOTMat.ColumnEdit = this.editSpin;
                CostoLocalTOTMat.OptionsColumn.AllowEdit = false;
                CostoLocalTOTMat.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                CostoLocalTOTMat.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
                this.gvMateriales.Columns.Add(CostoLocalTOTMat);

                GridColumn CostoTotalMLRecMat = new GridColumn();
                CostoTotalMLRecMat.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalMLRecMat.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoTotalMLRecMat.UnboundType = UnboundColumnType.Decimal;
                CostoTotalMLRecMat.VisibleIndex = 12;
                CostoTotalMLRecMat.Width = 80;
                CostoTotalMLRecMat.Visible = false;
                CostoTotalMLRecMat.ColumnEdit = this.editSpin;
                CostoTotalMLRecMat.OptionsColumn.AllowEdit = false;
                this.gvMateriales.Columns.Add(CostoTotalMLRecMat);

                GridColumn TipoRecursoMat = new GridColumn();
                TipoRecursoMat.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecursoMat.UnboundType = UnboundColumnType.Integer;
                TipoRecursoMat.Width = 80;
                TipoRecursoMat.Visible = false;
                this.gvMateriales.Columns.Add(TipoRecursoMat);
                #endregion
                #region Grilla Transporte
                GridColumn RecursoIDTrans = new GridColumn();
                RecursoIDTrans.FieldName = this.unboundPrefix + "RecursoID";
                RecursoIDTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoIDTrans.UnboundType = UnboundColumnType.String;
                RecursoIDTrans.VisibleIndex = 0;
                RecursoIDTrans.Width = 60;
                RecursoIDTrans.Visible = true;
                RecursoIDTrans.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(RecursoIDTrans);

                GridColumn RecursoDescTrans = new GridColumn();
                RecursoDescTrans.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDescTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDescTrans.UnboundType = UnboundColumnType.String;
                RecursoDescTrans.VisibleIndex = 1;
                RecursoDescTrans.Width = 200;
                RecursoDescTrans.Visible = true;
                RecursoDescTrans.OptionsColumn.AllowEdit = false;
                this.gvTransporte.Columns.Add(RecursoDescTrans);

                GridColumn UnidadInvIDrecTrans = new GridColumn();
                UnidadInvIDrecTrans.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrecTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrecTrans.UnboundType = UnboundColumnType.String;
                UnidadInvIDrecTrans.VisibleIndex = 2;
                UnidadInvIDrecTrans.Width = 60;
                UnidadInvIDrecTrans.Visible = true;
                UnidadInvIDrecTrans.OptionsColumn.AllowEdit = false;
                this.gvTransporte.Columns.Add(UnidadInvIDrecTrans);

                //Peso_Cantidad 
                GridColumn Peso_Cantidad = new GridColumn();
                Peso_Cantidad.FieldName = this.unboundPrefix + "Peso_Cantidad";
                Peso_Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Peso_CantidadTR");
                Peso_Cantidad.UnboundType = UnboundColumnType.Decimal;
                Peso_Cantidad.VisibleIndex = 3;
                Peso_Cantidad.Width = 50;
                Peso_Cantidad.Visible = true;
                Peso_Cantidad.ToolTip = "Peso aprox. de la carga";
                Peso_Cantidad.ColumnEdit = this.editValue2Cant;
                Peso_Cantidad.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(Peso_Cantidad);

                //Distancia_Turnos 
                GridColumn Distancia_Turnos = new GridColumn();
                Distancia_Turnos.FieldName = this.unboundPrefix + "Distancia_Turnos";
                Distancia_Turnos.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Distancia_TurnosTR");
                Distancia_Turnos.UnboundType = UnboundColumnType.Decimal;
                Distancia_Turnos.VisibleIndex = 4;
                Distancia_Turnos.Width = 50;
                Distancia_Turnos.Visible = true;
                Distancia_Turnos.ToolTip = "Distancia de recorrido";
                Distancia_Turnos.ColumnEdit = this.editValue8Cant;
                Distancia_Turnos.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(Distancia_Turnos);

                //Cant x 
                GridColumn FactorIDTrans = new GridColumn();
                FactorIDTrans.FieldName = this.unboundPrefix + "FactorID";
                FactorIDTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorIDTrans.UnboundType = UnboundColumnType.Decimal;
                FactorIDTrans.VisibleIndex = 5;
                FactorIDTrans.Width = 80;
                FactorIDTrans.Visible = true;
                FactorIDTrans.ColumnEdit = this.editValue8Cant;
                FactorIDTrans.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(FactorIDTrans);

                // Cant x Rec
                GridColumn CantidadRecTrans = new GridColumn();
                CantidadRecTrans.FieldName = this.unboundPrefix + "Cantidad";
                CantidadRecTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
                CantidadRecTrans.UnboundType = UnboundColumnType.Decimal;
                CantidadRecTrans.VisibleIndex = 6;
                CantidadRecTrans.Width = 80;
                CantidadRecTrans.Visible = false;
                CantidadRecTrans.ColumnEdit = this.editValue2Cant;
                CantidadRecTrans.OptionsColumn.AllowEdit = false;
                this.gvTransporte.Columns.Add(CantidadRecTrans);

                //Cant X Trabajo
                GridColumn CanSolicitudTrans = new GridColumn();
                CanSolicitudTrans.FieldName = this.unboundPrefix + "CantSolicitud";
                CanSolicitudTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
                CanSolicitudTrans.UnboundType = UnboundColumnType.Decimal;
                CanSolicitudTrans.VisibleIndex = 7;
                CanSolicitudTrans.Width = 80;
                CanSolicitudTrans.Visible = false;
                CanSolicitudTrans.ColumnEdit = this.editValue2Cant;
                CanSolicitudTrans.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(CanSolicitudTrans);

                //Costo Unit
                GridColumn CostoLocalTrans = new GridColumn();
                CostoLocalTrans.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocalTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocalTrans.UnboundType = UnboundColumnType.Decimal;
                CostoLocalTrans.VisibleIndex = 9;
                CostoLocalTrans.Width = 80;
                CostoLocalTrans.Visible = true;
                CostoLocalTrans.ColumnEdit = this.editSpin;
                //CostoLocalTrans.ColumnEdit = this.editBtnUpdate;
                CostoLocalTrans.OptionsColumn.AllowEdit = true;
                this.gvTransporte.Columns.Add(CostoLocalTrans);

                GridColumn CostoLocalTOTTrans = new GridColumn();
                CostoLocalTOTTrans.FieldName = this.unboundPrefix + "CostoLocalTOT";
                CostoLocalTOTTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
                CostoLocalTOTTrans.UnboundType = UnboundColumnType.Decimal;
                CostoLocalTOTTrans.VisibleIndex = 10;
                CostoLocalTOTTrans.Width = 80;
                CostoLocalTOTTrans.Visible = true;
                CostoLocalTOTTrans.ColumnEdit = this.editSpin;
                CostoLocalTOTTrans.OptionsColumn.AllowEdit = false;
                CostoLocalTOTTrans.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                CostoLocalTOTTrans.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
                this.gvTransporte.Columns.Add(CostoLocalTOTTrans);

                GridColumn CostoTotalMLRecTrans = new GridColumn();
                CostoTotalMLRecTrans.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalMLRecTrans.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoTotalMLRecTrans.UnboundType = UnboundColumnType.Decimal;
                CostoTotalMLRecTrans.VisibleIndex = 12;
                CostoTotalMLRecTrans.Width = 80;
                CostoTotalMLRecTrans.Visible = false;
                CostoTotalMLRecTrans.ColumnEdit = this.editSpin;
                CostoTotalMLRecTrans.OptionsColumn.AllowEdit = false;
                this.gvTransporte.Columns.Add(CostoTotalMLRecTrans);

                GridColumn TipoRecursoTrans = new GridColumn();
                TipoRecursoTrans.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecursoTrans.UnboundType = UnboundColumnType.Integer;
                TipoRecursoTrans.Width = 80;
                TipoRecursoTrans.Visible = false;
                this.gvTransporte.Columns.Add(TipoRecursoTrans);
                #endregion
                #region Grilla ManoObra
                GridColumn RecursoIDMO = new GridColumn();
                RecursoIDMO.FieldName = this.unboundPrefix + "RecursoID";
                RecursoIDMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoIDMO.UnboundType = UnboundColumnType.String;
                RecursoIDMO.VisibleIndex = 0;
                RecursoIDMO.Width = 50;
                RecursoIDMO.Visible = true;
                RecursoIDMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(RecursoIDMO);

                GridColumn RecursoDescMO = new GridColumn();
                RecursoDescMO.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDescMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDescMO.UnboundType = UnboundColumnType.String;
                RecursoDescMO.VisibleIndex = 1;
                RecursoDescMO.Width = 150;
                RecursoDescMO.Visible = true;
                RecursoDescMO.OptionsColumn.AllowEdit = false;
                this.gvManoObra.Columns.Add(RecursoDescMO);

                GridColumn UnidadInvIDrecMO = new GridColumn();
                UnidadInvIDrecMO.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrecMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrecMO.UnboundType = UnboundColumnType.String;
                UnidadInvIDrecMO.VisibleIndex = 2;
                UnidadInvIDrecMO.Width = 40;
                UnidadInvIDrecMO.Visible = true;
                UnidadInvIDrecMO.OptionsColumn.AllowEdit = false;
                this.gvManoObra.Columns.Add(UnidadInvIDrecMO);

                //Peso_Cantidad 
                GridColumn Peso_CantidadMO = new GridColumn();
                Peso_CantidadMO.FieldName = this.unboundPrefix + "Peso_Cantidad";
                Peso_CantidadMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Peso_CantidadMO");
                Peso_CantidadMO.UnboundType = UnboundColumnType.Decimal;
                Peso_CantidadMO.VisibleIndex = 3;
                Peso_CantidadMO.Width = 40;
                Peso_CantidadMO.Visible = true;
                Peso_CantidadMO.ColumnEdit = this.editValue2Cant;
                Peso_CantidadMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(Peso_CantidadMO);

                //CostoLocalInicial
                GridColumn CostoLocalInicial = new GridColumn();
                CostoLocalInicial.FieldName = this.unboundPrefix + "CostoLocalInicial";
                CostoLocalInicial.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalInicial");
                CostoLocalInicial.UnboundType = UnboundColumnType.Decimal;
                CostoLocalInicial.VisibleIndex = 4;
                CostoLocalInicial.Width = 60;
                CostoLocalInicial.Visible = true;
                CostoLocalInicial.ToolTip = "Tarifa inicial parametrizada en Recurso Costo Base";
                CostoLocalInicial.ColumnEdit = this.editSpin;
                //CostoLocConPrestacion.ColumnEdit = this.editBtnUpdate;
                CostoLocalInicial.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(CostoLocalInicial);

                //CostoLocConPrestacion
                GridColumn CostoLocConPrestacion = new GridColumn();
                CostoLocConPrestacion.FieldName = this.unboundPrefix + "CostoLocConPrestacion";
                CostoLocConPrestacion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalPrestacion");
                CostoLocConPrestacion.UnboundType = UnboundColumnType.Decimal;
                CostoLocConPrestacion.VisibleIndex = 4;
                CostoLocConPrestacion.Width = 60;
                CostoLocConPrestacion.Visible = true;
                CostoLocConPrestacion.ToolTip = "Tarifa Inicial + Prestaciones";
                CostoLocConPrestacion.ColumnEdit = this.editSpin;
                //CostoLocConPrestacion.ColumnEdit = this.editBtnUpdate;
                CostoLocConPrestacion.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(CostoLocConPrestacion);


                //Distancia_Turnos 
                GridColumn Distancia_TurnosMO = new GridColumn();
                Distancia_TurnosMO.FieldName = this.unboundPrefix + "Distancia_Turnos";
                Distancia_TurnosMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Distancia_TurnosMO");
                Distancia_TurnosMO.UnboundType = UnboundColumnType.Decimal;
                Distancia_TurnosMO.VisibleIndex = 5;
                Distancia_TurnosMO.Width = 40;
                Distancia_TurnosMO.Visible = true;
                Distancia_TurnosMO.ToolTip = "Cantidad de Turnos del personal";
                Distancia_TurnosMO.ColumnEdit = this.editValue8Cant;
                Distancia_TurnosMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(Distancia_TurnosMO);

                //Factor 
                GridColumn FactorIDMO = new GridColumn();
                FactorIDMO.FieldName = this.unboundPrefix + "FactorID";
                FactorIDMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_FactorID");
                FactorIDMO.UnboundType = UnboundColumnType.Decimal;
                FactorIDMO.VisibleIndex = 6;
                FactorIDMO.Width = 70;
                FactorIDMO.Visible = true;
                FactorIDMO.ColumnEdit = this.editValue8Cant;
                FactorIDMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(FactorIDMO);

                // Cant x Rec
                GridColumn CantidadRecMO = new GridColumn();
                CantidadRecMO.FieldName = this.unboundPrefix + "Cantidad";
                CantidadRecMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Cantidad");
                CantidadRecMO.UnboundType = UnboundColumnType.Decimal;
                CantidadRecMO.VisibleIndex = 7;
                CantidadRecMO.Width = 50;
                CantidadRecMO.Visible = false;
                CantidadRecMO.ColumnEdit = this.editValue2Cant;
                CantidadRecMO.OptionsColumn.AllowEdit = false;
                this.gvManoObra.Columns.Add(CantidadRecMO);

                //Cant X Trabajo
                GridColumn CanSolicitudMO = new GridColumn();
                CanSolicitudMO.FieldName = this.unboundPrefix + "CantSolicitud";
                CanSolicitudMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantSolicitud");
                CanSolicitudMO.UnboundType = UnboundColumnType.Decimal;
                CanSolicitudMO.VisibleIndex = 8;
                CanSolicitudMO.Width = 50;
                CanSolicitudMO.Visible = false;
                CanSolicitudMO.ColumnEdit = this.editValue2Cant;
                CanSolicitudMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(CanSolicitudMO);

                //Costo Unit
                GridColumn CostoLocalMO = new GridColumn();
                CostoLocalMO.FieldName = this.unboundPrefix + "CostoLocal";
                CostoLocalMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoLocalMO.UnboundType = UnboundColumnType.Decimal;
                CostoLocalMO.VisibleIndex = 9;
                CostoLocalMO.Width = 70;
                CostoLocalMO.Visible = true;
                CostoLocalMO.ColumnEdit = this.editSpin;
                CostoLocalMO.ToolTip = "Tarifa Final con prestaciones(185%)";
                CostoLocalMO.OptionsColumn.AllowEdit = true;
                this.gvManoObra.Columns.Add(CostoLocalMO);

                GridColumn CostoLocalTOTMO = new GridColumn();
                CostoLocalTOTMO.FieldName = this.unboundPrefix + "CostoLocalTOT";
                CostoLocalTOTMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocalTOT");
                CostoLocalTOTMO.UnboundType = UnboundColumnType.Decimal;
                CostoLocalTOTMO.VisibleIndex = 10;
                CostoLocalTOTMO.Width = 80;
                CostoLocalTOTMO.Visible = true;
                CostoLocalTOTMO.ColumnEdit = this.editSpin;
                CostoLocalTOTMO.OptionsColumn.AllowEdit = false;
                CostoLocalTOTMO.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                CostoLocalTOTMO.SummaryItem.DisplayFormat = "A.P.U = {0:c2}";
                this.gvManoObra.Columns.Add(CostoLocalTOTMO);

                GridColumn CostoTotalMLRecMO = new GridColumn();
                CostoTotalMLRecMO.FieldName = this.unboundPrefix + "CostoTotalML";
                CostoTotalMLRecMO.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CostoLocal");
                CostoTotalMLRecMO.UnboundType = UnboundColumnType.Decimal;
                CostoTotalMLRecMO.VisibleIndex = 12;
                CostoTotalMLRecMO.Width = 80;
                CostoTotalMLRecMO.Visible = false;
                CostoTotalMLRecMO.ColumnEdit = this.editSpin;
                CostoTotalMLRecMO.OptionsColumn.AllowEdit = false;
                this.gvManoObra.Columns.Add(CostoTotalMLRecMO);

                GridColumn TipoRecursoMO = new GridColumn();
                TipoRecursoMO.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecursoMO.UnboundType = UnboundColumnType.Integer;
                TipoRecursoMO.Visible = false;
                this.gvManoObra.Columns.Add(TipoRecursoMO);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectoss", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected virtual void LoadDataGrids()
        {
            try
            {
                this.gcEquipo.DataSource = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Equipo).ToList(); 
                this.gcEquipo.RefreshDataSource();

                this.gcMateriales.DataSource = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Insumo).ToList(); 
                this.gcMateriales.RefreshDataSource();

                this.gcTransporte.DataSource = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Transporte).ToList(); 
                this.gcTransporte.RefreshDataSource();

                this.gcManoObra.DataSource = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Personal).ToList(); 
                this.gcManoObra.RefreshDataSource();

                this.CalculateValues();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectoss.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void CalculateValues()
        {
            try
            {
                #region Calcula Costos y Cantidades
                foreach (DTO_pyPreProyectoDeta d in this._listDetRecursosAll)
                {
                    decimal rend = (d.Cantidad.Value.Value * d.FactorID.Value.Value); //Rendimiento
                    d.CantidadTOT.Value = 1;//Cantidad total del recurso                   
                    if (d.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                    {
                        if (!this._equipoCantidadInd)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * rend, 2);
                    }
                    else if (d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        if (!this._personalCantidadInd)
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * rend, 2);
                    }
                    else
                        d.CostoLocalTOT.Value = Math.Round(d.CostoLocal.Value.Value * rend,2); //Valor Total por recurso
                    if (d.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        #region Obtiene Costo original
                        Dictionary<string, string> pks = new Dictionary<string, string>();
                        pks.Add("RecursoID", d.RecursoID.Value);
                        pks.Add("LugarGeograficoID", this._lugarGeoxDef);
                        DTO_pyRecursoCostoBase recursoCosto = (DTO_pyRecursoCostoBase)this._bc.GetMasterComplexDTO(AppMasters.pyRecursoCostoBase, pks, true);
                        decimal costoOriginal = recursoCosto != null ? recursoCosto.CostoLocalEMP.Value.Value : 0;                        
                        #endregion                        
                        decimal cantPersonal = !string.IsNullOrEmpty(d.Peso_Cantidad.Value.ToString()) && d.Peso_Cantidad.Value != 0 ? d.Peso_Cantidad.Value.Value : 1;
                        d.CostoLocalInicial.Value = d.CostoLocal.Value != costoOriginal ? (d.CostoLocal.Value / cantPersonal) / this._porcPrestManoObra : d.CostoLocal.Value;
                        d.CostoLocConPrestacion.Value = d.CostoLocalInicial.Value * this._porcPrestManoObra;
                        d.CostoLocal.Value = d.CostoLocConPrestacion.Value * cantPersonal;
                        if (!this._personalCantidadInd)
                            d.CostoLocalTOT.Value = Math.Round(d.CantidadTOT.Value.Value * d.CostoLocal.Value.Value / (rend != 0 ? rend : 1), 2);
                        else
                            d.CostoLocalTOT.Value = Math.Round(d.CantidadTOT.Value.Value * d.CostoLocal.Value.Value * rend, 2); 
                    }
                     if (d.CostoExtra.Value != 0 && this._tasaCambio != 0)
                            d.CostoExtra.Value = d.CostoLocal.Value / this._tasaCambio;
                }
                #endregion
                #region Asigna valores
                decimal costoTotalUnitTarea = this._listDetRecursosAll.Sum(x => x.CostoLocalTOT.Value.Value);
                #region Calcula AIU por APU
                if (this._APUIncluyeAIUInd)
                {
                    decimal AIUAdminAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorcAdmin.EditValue, CultureInfo.InvariantCulture)) / 100, 2);
                    decimal AIUImprAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorcImpr.EditValue, CultureInfo.InvariantCulture)) / 100, 2);
                    decimal AIUUtilAPU = Math.Round((costoTotalUnitTarea * Convert.ToDecimal(this.txtPorcUtil.EditValue, CultureInfo.InvariantCulture)) / 100, 2);
                    this._listDetRecursosAll.ForEach(x =>
                    {
                        x.VlrAIUAdmin.Value = AIUAdminAPU;
                        x.VlrAIUImpr.Value = AIUImprAPU;
                        x.VlrAIUUtil.Value = AIUUtilAPU;
                    });
                    this._tareaCurrent.VlrAIUxAPUAdmin.Value = AIUAdminAPU;
                    this._tareaCurrent.VlrAIUxAPUImpr.Value = AIUImprAPU;
                    this._tareaCurrent.VlrAIUxAPUUtil.Value = AIUUtilAPU;
                    this._tareaCurrent.VlrAIUxAPUIVA.Value = Math.Round(this._tareaCurrent.Detalle.Where(x => x.TipoRecurso.Value != (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value) * this._porIVA,2);
                    this.txtVlrIVA.EditValue = this._tareaCurrent.VlrAIUxAPUIVA.Value;
                    this.txtPorIVA.EditValue = this._porIVA*100;
                    costoTotalUnitTarea += AIUAdminAPU + AIUImprAPU + AIUUtilAPU;
                }
                this.UpdateValues();
                #endregion            

                #endregion      
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "CalculateValues"));
            }
        }

        /// <summary>
        /// Actualiza los costos generales
        /// </summary>
        private void UpdateValues()
        {
            try
            {
                decimal subTotalEquipo = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Equipo).Sum(x=>x.CostoLocalTOT.Value.Value);
                decimal subTotalMaterial = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Insumo).Sum(x => x.CostoLocalTOT.Value.Value);
                decimal subTotalTransporte = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Transporte).Sum(x => x.CostoLocalTOT.Value.Value);
                decimal subTotalManoObra = this._listDetRecursosAll.FindAll(x => x.TipoRecurso.Value == (byte)TipoRecurso.Personal).Sum(x => x.CostoLocalTOT.Value.Value);

                this.txtSubTotalEquipo.EditValue = subTotalEquipo;
                this.txtSubTotalMaterial.EditValue = subTotalMaterial;
                this.txtSubTotalTransporte.EditValue = subTotalTransporte;
                this.txtSubTotalManoObra.EditValue = subTotalManoObra;
                this.txtSubTotalDirecto.EditValue = subTotalEquipo + subTotalMaterial + subTotalTransporte + subTotalManoObra;

                this.txtVlrAdmin.EditValue = this._tareaCurrent.VlrAIUxAPUAdmin.Value;
                this.txtVlrImpr.EditValue = this._tareaCurrent.VlrAIUxAPUImpr.Value;
                this.txtVlrUtil.EditValue = this._tareaCurrent.VlrAIUxAPUUtil.Value;
                this.txtSubTotalIndirecto.EditValue = this._tareaCurrent.VlrAIUxAPUAdmin.Value + this._tareaCurrent.VlrAIUxAPUImpr.Value + this._tareaCurrent.VlrAIUxAPUUtil.Value;
                decimal costoAIU = Convert.ToDecimal(this.txtSubTotalIndirecto.EditValue, CultureInfo.InvariantCulture);               

                //Redonde y Asigna totales
                decimal costoAPUFinal = 0;
                if (this._tipoRedondeo == 2)//Redonde hacia arriba
                    costoAPUFinal = Math.Ceiling(subTotalEquipo + subTotalMaterial + subTotalTransporte + subTotalManoObra + costoAIU);
                else if (this._tipoRedondeo == 3) //Redondea hacia abajo
                    costoAPUFinal = Math.Floor(subTotalEquipo + subTotalMaterial + subTotalTransporte + subTotalManoObra + costoAIU);
                else
                    costoAPUFinal = Math.Round(subTotalEquipo + subTotalMaterial + subTotalTransporte + subTotalManoObra + costoAIU, 0);

                this.txtTOTAL.EditValue = costoAPUFinal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "gvDetalle_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
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
        /// Cuando selecciona un item de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                this.gridCurrent = (GridView)sender;
                if (e.FocusedRowHandle >= 0)
                    this.detCurrent = (DTO_pyPreProyectoDeta)this.gridCurrent.GetRow(e.FocusedRowHandle);
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
        private void gv_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                this.gridCurrent = (GridView)sender;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gridCurrent.Columns[this.unboundPrefix + fieldName];

                if (e.RowHandle >= 0)
                    this.detCurrent = (DTO_pyPreProyectoDeta)this.gridCurrent.GetRow(e.RowHandle);

                if (fieldName == "RecursoID")
                {
                    DTO_pyRecurso rec = (DTO_pyRecurso)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, false, e.Value.ToString(), true);
                    if (rec != null)
                    {
                        this.gridCurrent.SetRowCellValue(this.gridCurrent.FocusedRowHandle, this.unboundPrefix + "RecursoDesc", rec.Descriptivo.Value);
                        this.gridCurrent.SetRowCellValue(this.gridCurrent.FocusedRowHandle, this.unboundPrefix + "UnidadInvID", rec.UnidadInvID.Value);
                    }
                    this.gridCurrent.RefreshData();
                }
                else if (fieldName == "CantSolicitud")
                {
                    this.CalculateValues();
                }
                else if (fieldName == "FactorID")
                {
                    this.CalculateValues();
                }
                else if (fieldName == "CostoLocal")
                {
                    this.CalculateValues();
                }
                else if (fieldName == "Peso_Cantidad")
                {
                    #region Calcula valores de transporte
                    if (this.detCurrent != null && this.detCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                    {
                        if (e.Value != null)
                            this.detCurrent.Peso_Cantidad.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        else
                            this.detCurrent.Peso_Cantidad.Value = 1;
                        if (this.detCurrent.Distancia_Turnos.Value != null)
                            this.detCurrent.FactorID.Value = this.detCurrent.Distancia_Turnos.Value * this.detCurrent.Peso_Cantidad.Value;
                        this.gvTransporte.RefreshData();
                        this.CalculateValues();
                    } 
                    #endregion
                    #region Calcula valores de Mano de Obra
                    else if (this.detCurrent != null && this.detCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        if (e.Value != null)
                        {                           
                            this.detCurrent.Peso_Cantidad.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                            this.detCurrent.CostoLocConPrestacion.Value = this.detCurrent.CostoLocalInicial.Value * this._porcPrestManoObra;
                            this.detCurrent.CostoLocal.Value = this.detCurrent.CostoLocConPrestacion.Value * this.detCurrent.Peso_Cantidad.Value;
                        }
                        else
                            this.detCurrent.Peso_Cantidad.Value = 1;
                        if (e.Value != null && this.detCurrent.Distancia_Turnos.Value != null)
                        {
                            this.detCurrent.CostoLocal.Value = this.detCurrent.CostoLocConPrestacion.Value * this.detCurrent.Peso_Cantidad.Value;
                            this.detCurrent.FactorID.Value = this.detCurrent.FactorID.Value * this.detCurrent.Distancia_Turnos.Value;
                        }

                        this.gvManoObra.RefreshData();
                        this.CalculateValues();
                    } 
                    #endregion
                    this.CalculateValues();
                }
                else if (fieldName == "Distancia_Turnos")
                {
                    #region Calcula valores de transporte
                    if (this.detCurrent != null && this.detCurrent.TipoRecurso.Value == (byte)TipoRecurso.Transporte)
                    {
                        if (e.Value != null)
                            this.detCurrent.Distancia_Turnos.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        else
                            this.detCurrent.Distancia_Turnos.Value = 1;
                        if (this.detCurrent.Peso_Cantidad.Value != null)
                            this.detCurrent.FactorID.Value = this.detCurrent.Distancia_Turnos.Value * this.detCurrent.Peso_Cantidad.Value;
                        this.gvTransporte.RefreshData();
                        this.CalculateValues();
                    } 
                    #endregion
                    #region Calcula valores de mano de obra
                    else if (this.detCurrent != null && this.detCurrent.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    {
                        if (e.Value != null)
                            this.detCurrent.Distancia_Turnos.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        else
                            this.detCurrent.Distancia_Turnos.Value = 1;
                        if (this.detCurrent.Peso_Cantidad.Value != null)
                        {
                            this.detCurrent.CostoLocal.Value = this.detCurrent.CostoLocalInicial.Value * this.detCurrent.Peso_Cantidad.Value *this._porcPrestManoObra;
                            this.detCurrent.FactorID.Value = this.detCurrent.FactorID.Value * this.detCurrent.Distancia_Turnos.Value;
                        }
                        this.gvManoObra.RefreshData();
                        this.CalculateValues();
                    }
                    this.CalculateValues(); 
                    #endregion
                }
                else if (fieldName == "CostoLocConPrestacion")
                {
                    this.detCurrent.Peso_Cantidad.Value = this.detCurrent.Peso_Cantidad.Value != null ? this.detCurrent.Peso_Cantidad.Value : 1;
                    this.detCurrent.CostoLocal.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) * this.detCurrent.Peso_Cantidad.Value;
                    this.CalculateValues();
                }
                else if (fieldName == "CostoLocalInicial")
                {
                    this.detCurrent.CostoLocConPrestacion.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) * this._porcPrestManoObra;
                    this.detCurrent.Peso_Cantidad.Value = this.detCurrent.Peso_Cantidad.Value != null ? this.detCurrent.Peso_Cantidad.Value : 1;
                    this.detCurrent.CostoLocal.Value = this.detCurrent.CostoLocConPrestacion.Value * this.detCurrent.Peso_Cantidad.Value;
                    this.CalculateValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "gvRecurso_CellValueChanged"));
            }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetRecursoExist_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._listDetRecursosAll.Count > 0)
                {
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    if (MessageBox.Show("Desea cargar un A.P.U existente y sobreesribir el actual?", "Agregar A.P.U", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ModalGetAPUByTarea modal = new ModalGetAPUByTarea();
                        modal.ShowDialog();
                        if (!modal.isCancelSelected)
                        {
                            #region Acumula los recursos a eliminar
                            foreach (DTO_pyPreProyectoDeta rec in this._listDetRecursosAll)
                            {
                                if (rec.Consecutivo.Value.HasValue && !this._recursosDelete.Exists(x => x == rec.Consecutivo.Value))
                                    this._recursosDelete.Add(rec.Consecutivo.Value.Value);
                            }
                            #endregion

                            foreach (var det in modal.TareaSelected.Detalle)
                            {
                                det.Consecutivo.Value = null;
                                det.ConsecTarea.Value = null;
                                det.NumeroDoc.Value = null;
                            }
                            this._listDetRecursosAll = modal.TareaSelected.Detalle;
                            if (!this._isAPUCliente)
                                this._tareaCurrent.Detalle = this._listDetRecursosAll;
                            else
                                this._tareaCurrent.DetalleAPUCliente = this._listDetRecursosAll;
                            this.LoadDataGrids(); 
                        }
                    }
                }
                else
                {
                    ModalGetAPUByTarea modal = new ModalGetAPUByTarea();
                    modal.ShowDialog();
                    if (!modal.isCancelSelected && modal.TareaSelected != null)
                    {                      
                        foreach (var det in modal.TareaSelected.Detalle)
                        {
                            det.Consecutivo.Value = null;
                            det.ConsecTarea.Value = null;
                            det.NumeroDoc.Value = null;
                        }
                        this._listDetRecursosAll = modal.TareaSelected.Detalle;

                        if (!this._isAPUCliente)
                            this._tareaCurrent.Detalle = this._listDetRecursosAll;
                        else
                            this._tareaCurrent.DetalleAPUCliente = this._listDetRecursosAll;

                        this.LoadDataGrids(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "btnGetRecursoExist_Click: " + ex.Message));
            }
        }           

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAddRecurso_Click(object sender, EventArgs e)
        {
            try
            {
                ModalRecursosTrabajo modal = new ModalRecursosTrabajo(this._listDetRecursosAll.ToList(), _tasaCambio);
                modal.ShowDialog();
                List<DTO_pyPreProyectoDeta> listTmp = new List<DTO_pyPreProyectoDeta>();
           
                //Agrega los nuevos recursos consolidandolos
                foreach (DTO_pyPreProyectoDeta rec in modal.ListSelected)
                {
                    rec.CostoLocal.Value = rec.CostoExtra.Value != 0 && rec.CostoExtra.Value != null && rec.CostoLocal.Value == 0 ? rec.CostoExtra.Value * this._tasaCambio : rec.CostoLocal.Value;  
                    if (_listDetRecursosAll.Exists(x => x.RecursoID.Value == rec.RecursoID.Value))
                        listTmp.Add(this._listDetRecursosAll.Find(x => x.RecursoID.Value == rec.RecursoID.Value));
                    else
                        listTmp.Add(rec);                     
                }
                this._listDetRecursosAll = listTmp;

                if (!this._isAPUCliente)
                    this._tareaCurrent.Detalle = this._listDetRecursosAll;
                else
                    this._tareaCurrent.DetalleAPUCliente = this._listDetRecursosAll;

                #region Acumula los recursos a eliminar
                foreach (int rec in modal.RecursosDelete)
                {
                    if (!this._recursosDelete.Exists(x => x == rec))
                        this._recursosDelete.Add(rec);
                } 
                #endregion

                this.LoadDataGrids();             
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "btnAddRecurso_Click: " + ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal("UnidadInvID", origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Al hacer doble click al control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSpin_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string fieldName = this.gridCurrent.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                if (fieldName.Equals("CostoLocal") || fieldName.Equals("CostoLocalInicial"))
                {
                    //if (MessageBox.Show("¿Desea cambiar el costo del insumo?", "Actualizar costo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    ButtonEdit edit = (ButtonEdit)sender;
                    //    Dictionary<string, string> pks = new Dictionary<string, string>();
                    //    pks.Add("RecursoID", this.detCurrent.RecursoID.Value);
                    //    pks.Add("LugarGeograficoID", this._lugarGeoxDef);
                    //    DTO_pyRecursoCostoBase recursoCosto = (DTO_pyRecursoCostoBase)this._bc.GetMasterComplexDTO(AppMasters.pyRecursoCostoBase, pks, true);
                    //    if (recursoCosto != null)
                    //    {
                    //        recursoCosto.CostoLocalEMP.Value = Convert.ToDecimal(edit.EditValue);
                    //        this._bc.AdministrationModel.MasterComplex_Update(AppMasters.pyRecursoCostoBase, recursoCosto);
                    //    }
                    //} 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalAPUProyectos.cs", "editSpin_DoubleClick: " + ex.Message));
            }
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_pyPreProyectoDeta> ListSelected
        {
            get 
            {
                if(!this._isAPUCliente)
                    this._tareaCurrent.Detalle = this._listDetRecursosAll;
                else
                    this._tareaCurrent.DetalleAPUCliente = this._listDetRecursosAll;
                return this._listDetRecursosAll;
            }
        }

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<int> RecursosDelete
        {
            get { return this._recursosDelete; }
        }

        #endregion       

      }
}
