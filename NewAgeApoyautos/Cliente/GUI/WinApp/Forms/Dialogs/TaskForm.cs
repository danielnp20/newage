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
using System.Diagnostics;
using System.Net;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using NewAge.DTO.Resultados;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class TaskForm : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private int _documentID;
        private ModulesPrefix frmModule;

        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string _tab = "\t";
        private string _unboundPrefix = "Unbound_";
        private bool _firstTime = true;
        List<DTO_InfoTarea> detalle = new List<DTO_InfoTarea>();
        List<DTO_InfoTarea> detalleNotas = new List<DTO_InfoTarea>();
        private bool deleteOP = false;
        private bool isValid = false;

        #endregion

        /// <summary>
        /// Constructor con un documento
        /// </summary>
        /// <param name="doc"></param>
        public TaskForm()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public TaskForm(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Funcion que llama el Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this._documentID = AppForms.TaskForm;
            this.frmModule = ModulesPrefix.gl;
            this._frmName = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
            FormProvider.Master.Form_Load(this, this.frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

            this.AddGridCols();
            this.InitControls();
            this.LoadPageData();
            this._firstTime = false;
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            #region Pendientes
            this._bc.InitMasterUC(this.masterActividadFlujoPend, AppMasters.glActividadFlujo, false, true, true, false);
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);

            //Orden
            Dictionary<string, string> dicOrden = new Dictionary<string, string>();
            dicOrden.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Fecha"));
            dicOrden.Add("2", this._bc.GetResource(LanguageTypes.Tables, "Tarea"));
            dicOrden.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Tercero"));
            this.cmbOrden.Properties.DataSource = dicOrden;
            this.cmbOrden.EditValue = 1;

            //Tipo
            Dictionary<string, string> dicTipo = new Dictionary<string, string>();
            dicTipo.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Vencidas"));
            dicTipo.Add("2", this._bc.GetResource(LanguageTypes.Tables, "No Vencidas"));
            dicTipo.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Todas"));
            this.cmbEstado.Properties.DataSource = dicTipo;
            this.cmbEstado.EditValue = 1;

            this.tpPendientes.Focus();
            #endregion
            #region Llamadas
            this._bc.InitMasterUC(this.masterActivFlujoLlam, AppMasters.glActividadFlujo, false, true, true, false);
            this._bc.InitMasterUC(this.masterTerceroLlamadas, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.masterDocumentoLlamadas, AppMasters.glDocumento, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoLlamadas, AppMasters.glPrefijo, true, true, true, false);
            #endregion            
            #region Historia
            this._bc.InitMasterUC(this.masterActividadFlujoHist, AppMasters.glActividadFlujo, false, true, true, false);
            this._bc.InitMasterUC(this.masterTerceroHist, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.masterDocumentoHist, AppMasters.glDocumento, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoHist, AppMasters.glPrefijo, true, true, true, false);
            #endregion
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageData()
        {
            bool? vencidas = (bool?)null;
            EstadoTareaIncumplimiento tipo = EstadoTareaIncumplimiento.Abiertas;

            if(this.cmbEstado.EditValue.ToString() == "1")
                vencidas = true;
            else if(this.cmbEstado.EditValue.ToString() == "2")
                vencidas = false;
            else if (this.cmbEstado.EditValue.ToString() == "3")
                tipo = EstadoTareaIncumplimiento.Todas;

            if (this._firstTime)
            {
                this.detalle = this._bc.AdministrationModel.glActividadEstado_GetPendientesByParameter(null, null, string.Empty, null, null, string.Empty,
                    string.Empty, null, tipo, false, vencidas);
               
                this.gcDocPendientes.DataSource = this.detalle;
                this.gvDocPendientes.RefreshData();

                //Por defecto ordena por fecha
                this.detalle = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();

                this.dtFechaIniPend.DateTime = this.detalle.Count > 0 ? this.detalle.First().FechaInicio.Value.Value : DateTime.Now;
                this.dtFechaFinPend.DateTime = this.detalle.Count > 0 ? this.detalle.Last().FechaInicio.Value.Value : DateTime.Now;

            }
            //Pendientes
            else if (this.tcTask.SelectedTabPage == this.tpPendientes)
            {
                this.detalle = this._bc.AdministrationModel.glActividadEstado_GetPendientesByParameter(null, null, this.masterActividadFlujoPend.Value, 
                    this.dtFechaIniPend.DateTime, this.dtFechaFinPend.DateTime, this.masterTercero.Value,string.Empty, null, tipo, false, vencidas);

                if (this.cmbOrden.EditValue.Equals("1"))
                    this.detalle = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();
                else if (this.cmbOrden.EditValue.Equals("2"))
                    this.detalle = this.detalle.OrderBy(x => x.ActividadFlujoID.Value).ToList();
                else if (this.cmbOrden.EditValue.Equals("3"))
                    this.detalle = this.detalle.OrderBy(x => x.TerceroID.Value).ToList();
           
                this.gcDocPendientes.DataSource = this.detalle;
                this.gvDocPendientes.RefreshData();

                //Asigna valores de fecha
                var detaFechas = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();
                this.dtFechaIniPend.DateTime = detaFechas.Count > 0 ? detaFechas.First().FechaInicio.Value.Value : DateTime.Now;
                this.dtFechaFinPend.DateTime = detaFechas.Count > 0 ? detaFechas.Last().FechaInicio.Value.Value : DateTime.Now;
            }
            //Llamadas
            else if (this.tcTask.SelectedTabPage == this.tpLlamadas)
            {                
                this.detalle = this._bc.AdministrationModel.glActividadEstado_GetPendientesByParameter(null, null, this.masterActivFlujoLlam.Value, 
                    this.dtFechaIniPend.DateTime, this.dtFechaFinPend.DateTime, string.Empty, string.Empty, null, tipo, true, false);

                this.gcDocLLamadas.DataSource = this.detalle;
                this.gvDocLlamadas.RefreshData();

                //Asigna valores de fecha
                var detaFechas = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();
                this.dtFechaIniLlam.DateTime = detaFechas.Count > 0 ? detaFechas.First().FechaInicio.Value.Value : DateTime.Now;
                this.dtFechaFinLlam.DateTime = detaFechas.Count > 0 ? detaFechas.Last().FechaInicio.Value.Value : DateTime.Now;
            }
            //Notas
            else if (this.tcTask.SelectedTabPage == this.tpNotas)
            {
                this.detalleNotas = this._bc.AdministrationModel.glActividadEstado_GetPendientesByParameter(null, AppDocuments.RecordatorioActividadEstado, string.Empty, 
                    this.dtFechaIniNotas.DateTime, this.dtFechaFinNotas.DateTime, string.Empty, string.Empty, null, EstadoTareaIncumplimiento.Abiertas, false, false);
               
                this.gcDocNotas.DataSource = this.detalleNotas;
                this.gvDocNotas.RefreshData();
            }
            //Historial
            else if (this.tcTask.SelectedTabPage == this.tpHistoria)
            {
                tipo = EstadoTareaIncumplimiento.Todas;

                this.detalle = this._bc.AdministrationModel.glActividadEstado_GetPendientesByParameter(null, null, this.masterActividadFlujoHist.Value, 
                    this.dtFechaIniPend.DateTime, this.dtFechaFinPend.DateTime, string.Empty, string.Empty, null, tipo, false, vencidas);
              
                this.gcDocHistoria.DataSource = this.detalle;
                this.gvDocHistoria.RefreshData();

                //Asigna valores de fecha
                var detaFechas = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();
                this.dtFechaIniHist.DateTime = detaFechas.Count > 0 ? detaFechas.First().FechaInicio.Value.Value : DateTime.Now;
                this.dtFechaFinHist.DateTime = detaFechas.Count > 0 ? detaFechas.Last().FechaInicio.Value.Value : DateTime.Now;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Pendientes
                //FechaInicioPend
                GridColumn FechaInicioPend = new GridColumn();
                FechaInicioPend.FieldName = this._unboundPrefix + "FechaInicio";
                FechaInicioPend.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaInicio");
                FechaInicioPend.UnboundType = UnboundColumnType.DateTime;
                FechaInicioPend.VisibleIndex = 1;
                FechaInicioPend.Width = 50;
                FechaInicioPend.Visible = true;
                FechaInicioPend.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(FechaInicioPend);

                //Incumplimiento
                GridColumn Incumplimiento = new GridColumn();
                Incumplimiento.FieldName = this._unboundPrefix + "Incumplimiento";
                Incumplimiento.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Incumplimiento");
                Incumplimiento.UnboundType = UnboundColumnType.Integer;
                Incumplimiento.VisibleIndex = 2;
                Incumplimiento.Width = 60;
                Incumplimiento.Visible = true;
                Incumplimiento.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(Incumplimiento);

                //UnidadTiempo
                GridColumn UnidadTiempo = new GridColumn();
                UnidadTiempo.FieldName = this._unboundPrefix + "UnidadTiempo";
                UnidadTiempo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_UnidadTiempo");
                UnidadTiempo.UnboundType = UnboundColumnType.Integer;
                UnidadTiempo.VisibleIndex = 3;
                UnidadTiempo.Width = 50;
                UnidadTiempo.Visible = true;
                UnidadTiempo.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(UnidadTiempo);

                //ActividadFlujoID(Tarea)
                GridColumn ActividadFlujoID = new GridColumn();
                ActividadFlujoID.FieldName = this._unboundPrefix + "ActividadFlujoID";
                ActividadFlujoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoID");
                ActividadFlujoID.UnboundType = UnboundColumnType.String;
                ActividadFlujoID.VisibleIndex = 4;
                ActividadFlujoID.Width = 60;
                ActividadFlujoID.Visible = true;
                ActividadFlujoID.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(ActividadFlujoID);

                //ActividadFlujoDesc
                GridColumn ActividadFlujoDesc = new GridColumn();
                ActividadFlujoDesc.FieldName = this._unboundPrefix + "ActividadFlujoDesc";
                ActividadFlujoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoDesc");
                ActividadFlujoDesc.UnboundType = UnboundColumnType.String;
                ActividadFlujoDesc.VisibleIndex = 5;
                ActividadFlujoDesc.Width = 90;
                ActividadFlujoDesc.Visible = true;
                ActividadFlujoDesc.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(ActividadFlujoDesc);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 6;
                TerceroID.Width = 60;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(TerceroID);

                //TerceroDesc
                GridColumn TerceroDesc = new GridColumn();
                TerceroDesc.FieldName = this._unboundPrefix + "TerceroDesc";
                TerceroDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroDesc");
                TerceroDesc.UnboundType = UnboundColumnType.String;
                TerceroDesc.VisibleIndex = 7;
                TerceroDesc.Width = 90;
                TerceroDesc.Visible = true;
                TerceroDesc.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(TerceroDesc);

                //DocumentoID(TipoDoc)
                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this._unboundPrefix + "DocumentoID";
                DocumentoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                DocumentoID.UnboundType = UnboundColumnType.Integer;
                DocumentoID.VisibleIndex = 8;
                DocumentoID.Width = 50;
                DocumentoID.Visible = true;
                DocumentoID.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(DocumentoID);

                //PrefDoc(Documento)
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 9;
                PrefDoc.Width = 60;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(PrefDoc);

                //UsuarioID(Responsable)
                GridColumn UsuarioID = new GridColumn();
                UsuarioID.FieldName = this._unboundPrefix + "UsuarioID";
                UsuarioID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_UsuarioID");
                UsuarioID.UnboundType = UnboundColumnType.String;
                UsuarioID.VisibleIndex = 10;
                UsuarioID.Width = 70;
                UsuarioID.Visible = true;
                UsuarioID.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(UsuarioID);

                //Observaciones
                GridColumn Observaciones = new GridColumn();
                Observaciones.FieldName = this._unboundPrefix + "Observaciones";
                Observaciones.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observaciones");
                Observaciones.UnboundType = UnboundColumnType.String;
                Observaciones.VisibleIndex = 11;
                Observaciones.Width = 90;
                Observaciones.Visible = true;
                Observaciones.OptionsColumn.AllowEdit = false;
                this.gvDocPendientes.Columns.Add(Observaciones);

                //VerDoc
                GridColumn VerDoc = new GridColumn();
                VerDoc.FieldName = this._unboundPrefix + "VerDoc";
                VerDoc.OptionsColumn.ShowCaption = false;
                VerDoc.UnboundType = UnboundColumnType.String;
                VerDoc.Width = 50;
                VerDoc.VisibleIndex = 12;
                VerDoc.Visible = true;
                VerDoc.ColumnEdit = this.linkVer;
                this.gvDocPendientes.Columns.Add(VerDoc);

                #region Columnas Ocultas
                GridColumn PrefijoID = new GridColumn();
                PrefijoID.FieldName = this._unboundPrefix + "PrefijoID";
                PrefijoID.UnboundType = UnboundColumnType.String;
                PrefijoID.Visible = false;
                this.gvDocPendientes.Columns.Add(PrefijoID);

                GridColumn DocumentoNro = new GridColumn();
                DocumentoNro.FieldName = this._unboundPrefix + "DocumentoNro";
                DocumentoNro.UnboundType = UnboundColumnType.String;
                DocumentoNro.Visible = false;
                this.gvDocPendientes.Columns.Add(DocumentoNro);

                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this._unboundPrefix + "DocumentoTercero";
                DocumentoTercero.UnboundType = UnboundColumnType.String;
                DocumentoTercero.Visible = false;
                this.gvDocPendientes.Columns.Add(DocumentoTercero);
                #endregion
                #endregion
                #region Llamadas
                //FechaInicio
                GridColumn FechaInicio = new GridColumn();
                FechaInicio.FieldName = this._unboundPrefix + "FechaInicio";
                FechaInicio.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaInicio");
                FechaInicio.UnboundType = UnboundColumnType.DateTime;
                FechaInicio.VisibleIndex = 1;
                FechaInicio.Width = 50;
                FechaInicio.Visible = true;
                FechaInicio.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(FechaInicio);

                //TerceroIDLlam
                GridColumn TerceroIDLlam = new GridColumn();
                TerceroIDLlam.FieldName = this._unboundPrefix + "TerceroID";
                TerceroIDLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroIDLlam.UnboundType = UnboundColumnType.String;
                TerceroIDLlam.VisibleIndex = 2;
                TerceroIDLlam.Width = 60;
                TerceroIDLlam.Visible = true;
                TerceroIDLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(TerceroIDLlam);

                //TerceroDescLlam
                GridColumn TerceroDescLlam = new GridColumn();
                TerceroDescLlam.FieldName = this._unboundPrefix + "TerceroDesc";
                TerceroDescLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroDesc");
                TerceroDescLlam.UnboundType = UnboundColumnType.String;
                TerceroDescLlam.VisibleIndex = 3;
                TerceroDescLlam.Width = 90;
                TerceroDescLlam.Visible = true;
                TerceroDescLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(TerceroDescLlam);

                //ActividadFlujoID(Tarea)
                GridColumn ActividadFlujoIDLlam = new GridColumn();
                ActividadFlujoIDLlam.FieldName = this._unboundPrefix + "ActividadFlujoID";
                ActividadFlujoIDLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoID");
                ActividadFlujoIDLlam.UnboundType = UnboundColumnType.String;
                ActividadFlujoIDLlam.VisibleIndex = 4;
                ActividadFlujoIDLlam.Width = 60;
                ActividadFlujoIDLlam.Visible = true;
                ActividadFlujoIDLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(ActividadFlujoIDLlam);

                //ActividadFlujoDescLlam
                GridColumn ActividadFlujoDescLlam = new GridColumn();
                ActividadFlujoDescLlam.FieldName = this._unboundPrefix + "ActividadFlujoDesc";
                ActividadFlujoDescLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoDesc");
                ActividadFlujoDescLlam.UnboundType = UnboundColumnType.String;
                ActividadFlujoDescLlam.VisibleIndex = 5;
                ActividadFlujoDescLlam.Width = 90;
                ActividadFlujoDescLlam.Visible = true;
                ActividadFlujoDescLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(ActividadFlujoDescLlam);

                //LlamadaDesc
                GridColumn LlamadaDesc = new GridColumn();
                LlamadaDesc.FieldName = this._unboundPrefix + "LlamadaDesc";
                LlamadaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_LlamadaDesc");
                LlamadaDesc.UnboundType = UnboundColumnType.String;
                LlamadaDesc.VisibleIndex = 6;
                LlamadaDesc.Width = 90;
                LlamadaDesc.Visible = true;
                LlamadaDesc.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(LlamadaDesc);

                //DocumentoIDLLam(TipoDoc)
                GridColumn DocumentoIDLLam = new GridColumn();
                DocumentoIDLLam.FieldName = this._unboundPrefix + "DocumentoID";
                DocumentoIDLLam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                DocumentoIDLLam.UnboundType = UnboundColumnType.Integer;
                DocumentoIDLLam.VisibleIndex = 7;
                DocumentoIDLLam.Width = 60;
                DocumentoIDLLam.Visible = true;
                DocumentoIDLLam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(DocumentoIDLLam);

                //PrefDocLlam(Documento)
                GridColumn PrefDocLlam = new GridColumn();
                PrefDocLlam.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDocLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDocLlam.UnboundType = UnboundColumnType.String;
                PrefDocLlam.VisibleIndex = 8;
                PrefDocLlam.Width = 60;
                PrefDocLlam.Visible = true;
                PrefDocLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(PrefDocLlam);

                //seUsuarioIDLlam(Responsable)
                GridColumn UsuarioIDLlam = new GridColumn();
                UsuarioIDLlam.FieldName = this._unboundPrefix + "UsuarioID";
                UsuarioIDLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_UsuarioID");
                UsuarioIDLlam.UnboundType = UnboundColumnType.String;
                UsuarioIDLlam.VisibleIndex = 9;
                UsuarioIDLlam.Width = 70;
                UsuarioIDLlam.Visible = true;
                UsuarioIDLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(UsuarioIDLlam);

                //ObservacionesLlam
                GridColumn ObservacionesLlam = new GridColumn();
                ObservacionesLlam.FieldName = this._unboundPrefix + "Observaciones";
                ObservacionesLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observaciones");
                ObservacionesLlam.UnboundType = UnboundColumnType.String;
                ObservacionesLlam.VisibleIndex = 10;
                ObservacionesLlam.Width = 90;
                ObservacionesLlam.Visible = true;
                ObservacionesLlam.OptionsColumn.AllowEdit = false;
                this.gvDocLlamadas.Columns.Add(ObservacionesLlam);

                #region Columnas Ocultas
                GridColumn PrefijoIDLlam = new GridColumn();
                PrefijoIDLlam.FieldName = this._unboundPrefix + "PrefijoID";
                PrefijoIDLlam.UnboundType = UnboundColumnType.String;
                PrefijoIDLlam.Visible = false;
                this.gvDocLlamadas.Columns.Add(PrefijoIDLlam);

                GridColumn DocumentoNroLlam = new GridColumn();
                DocumentoNroLlam.FieldName = this._unboundPrefix + "DocumentoNro";
                DocumentoNroLlam.UnboundType = UnboundColumnType.String;
                DocumentoNroLlam.Visible = false;
                this.gvDocLlamadas.Columns.Add(DocumentoNroLlam);

                GridColumn DocumentoTerceroLlam = new GridColumn();
                DocumentoTerceroLlam.FieldName = this._unboundPrefix + "DocumentoTercero";
                DocumentoTerceroLlam.UnboundType = UnboundColumnType.String;
                DocumentoTerceroLlam.Visible = false;
                this.gvDocLlamadas.Columns.Add(DocumentoTerceroLlam);
                #endregion
                #endregion
                #region Notas
                //Descriptivo
                GridColumn DescriptivoNota = new GridColumn();
                DescriptivoNota.FieldName = this._unboundPrefix + "Observaciones";
                DescriptivoNota.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observaciones");
                DescriptivoNota.UnboundType = UnboundColumnType.String;
                DescriptivoNota.VisibleIndex = 1;
                DescriptivoNota.Width = 120;
                DescriptivoNota.Visible = true;
                DescriptivoNota.OptionsColumn.AllowEdit = true;
                this.gvDocNotas.Columns.Add(DescriptivoNota);

                //FechaInicioNota
                GridColumn FechaInicioNota = new GridColumn();
                FechaInicioNota.FieldName = this._unboundPrefix + "FechaInicio";
                FechaInicioNota.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaInicio");
                FechaInicioNota.UnboundType = UnboundColumnType.DateTime;
                FechaInicioNota.VisibleIndex = 2;
                FechaInicioNota.Width = 50;
                FechaInicioNota.Visible = true;
                FechaInicioNota.ColumnEdit = this.editDate;
                FechaInicioNota.OptionsColumn.AllowEdit = true;
                this.gvDocNotas.Columns.Add(FechaInicioNota);

                //CerradoNota(Cumplida)
                GridColumn CerradoNota = new GridColumn();
                CerradoNota.FieldName = this._unboundPrefix + "CerradoInd";
                CerradoNota.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CerradoInd");
                CerradoNota.UnboundType = UnboundColumnType.Boolean;
                CerradoNota.VisibleIndex = 3;
                CerradoNota.Width = 50;
                CerradoNota.Visible = true;
                CerradoNota.OptionsColumn.AllowEdit = true;
                this.gvDocNotas.Columns.Add(CerradoNota);
                #endregion
                #region Historia
                //ActividadFlujoIDHistoria(Tarea)
                GridColumn ActividadFlujoIDHist = new GridColumn();
                ActividadFlujoIDHist.FieldName = this._unboundPrefix + "ActividadFlujoID";
                ActividadFlujoIDHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoID");
                ActividadFlujoIDHist.UnboundType = UnboundColumnType.String;
                ActividadFlujoIDHist.VisibleIndex = 1;
                ActividadFlujoIDHist.Width = 60;
                ActividadFlujoIDHist.Visible = true;
                ActividadFlujoIDHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(ActividadFlujoIDHist);

                //ActividadFlujoDesHist
                GridColumn ActividadFlujoDesHist = new GridColumn();
                ActividadFlujoDesHist.FieldName = this._unboundPrefix + "ActividadFlujoDesc";
                ActividadFlujoDesHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ActividadFlujoDesc");
                ActividadFlujoDesHist.UnboundType = UnboundColumnType.String;
                ActividadFlujoDesHist.VisibleIndex = 2;
                ActividadFlujoDesHist.Width = 90;
                ActividadFlujoDesHist.Visible = true;
                ActividadFlujoDesHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(ActividadFlujoDesHist);

                //FechaInicio
                GridColumn FechaInicioHist = new GridColumn();
                FechaInicioHist.FieldName = this._unboundPrefix + "FechaInicio";
                FechaInicioHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaInicioHist");
                FechaInicioHist.UnboundType = UnboundColumnType.DateTime;
                FechaInicioHist.VisibleIndex = 3;
                FechaInicioHist.Width = 60;
                FechaInicioHist.Visible = true;
                FechaInicioHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(FechaInicioHist);

                //FechaFin
                GridColumn FechaFinHist = new GridColumn();
                FechaFinHist.FieldName = this._unboundPrefix + "FechaFin";
                FechaFinHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaFin");
                FechaFinHist.UnboundType = UnboundColumnType.DateTime;
                FechaFinHist.VisibleIndex = 4;
                FechaFinHist.Width = 60;
                FechaFinHist.Visible = true;
                FechaFinHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(FechaFinHist);

                //FechaCerrado
                GridColumn FechaCerrado = new GridColumn();
                FechaCerrado.FieldName = this._unboundPrefix + "FechaCerrado";
                FechaCerrado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCerrado");
                FechaCerrado.UnboundType = UnboundColumnType.DateTime;
                FechaCerrado.VisibleIndex = 5;
                FechaCerrado.Width = 60;
                FechaCerrado.Visible = true;
                FechaCerrado.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(FechaCerrado);

                //CerradoInd
                GridColumn CerradoInd = new GridColumn();
                CerradoInd.FieldName = this._unboundPrefix + "CerradoInd";
                CerradoInd.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
                CerradoInd.UnboundType = UnboundColumnType.Integer;
                CerradoInd.VisibleIndex = 6;
                CerradoInd.Width = 50;
                CerradoInd.Visible = true;
                CerradoInd.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(CerradoInd);

                //IncumplimientoHist
                GridColumn IncumplimientoHist = new GridColumn();
                IncumplimientoHist.FieldName = this._unboundPrefix + "Incumplimiento";
                IncumplimientoHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Incumplimiento");
                IncumplimientoHist.UnboundType = UnboundColumnType.Integer;
                IncumplimientoHist.VisibleIndex = 7;
                IncumplimientoHist.Width = 60;
                IncumplimientoHist.Visible = true;
                IncumplimientoHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(IncumplimientoHist);

                //UnidadTiempoHist
                GridColumn UnidadTiempoHist = new GridColumn();
                UnidadTiempoHist.FieldName = this._unboundPrefix + "UnidadTiempo";
                UnidadTiempoHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_UnidadTiempo");
                UnidadTiempoHist.UnboundType = UnboundColumnType.Integer;
                UnidadTiempoHist.VisibleIndex = 8;
                UnidadTiempoHist.Width = 60;
                UnidadTiempoHist.Visible = true;
                UnidadTiempoHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(UnidadTiempoHist);

                //PrefDocHist(Documento)
                GridColumn PrefDocHist = new GridColumn();
                PrefDocHist.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDocHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDocHist.UnboundType = UnboundColumnType.String;
                PrefDocHist.VisibleIndex = 9;
                PrefDocHist.Width = 70;
                PrefDocHist.Visible = true;
                PrefDocHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(PrefDocHist);

                //seUsuarioIDHist(Responsable)
                GridColumn UsuarioIDHist = new GridColumn();
                UsuarioIDHist.FieldName = this._unboundPrefix + "UsuarioID";
                UsuarioIDHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_UsuarioID");
                UsuarioIDHist.UnboundType = UnboundColumnType.String;
                UsuarioIDHist.VisibleIndex = 10;
                UsuarioIDHist.Width = 70;
                UsuarioIDHist.Visible = true;
                UsuarioIDHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(UsuarioIDHist);

                //ObservacionesHist
                GridColumn ObservacionesHist = new GridColumn();
                ObservacionesHist.FieldName = this._unboundPrefix + "Observaciones";
                ObservacionesHist.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observaciones");
                ObservacionesHist.UnboundType = UnboundColumnType.String;
                ObservacionesHist.VisibleIndex = 11;
                ObservacionesHist.Width = 90;
                ObservacionesHist.Visible = true;
                ObservacionesHist.OptionsColumn.AllowEdit = false;
                this.gvDocHistoria.Columns.Add(ObservacionesHist);

                //VerDoc
                GridColumn VerDocHist = new GridColumn();
                VerDocHist.FieldName = this._unboundPrefix + "VerDoc";
                VerDocHist.OptionsColumn.ShowCaption = false;
                VerDocHist.UnboundType = UnboundColumnType.String;
                VerDocHist.Width = 50;
                VerDocHist.VisibleIndex = 12;
                VerDocHist.Visible = true;
                VerDocHist.ColumnEdit = this.linkVer;
                this.gvDocHistoria.Columns.Add(VerDocHist);

                #region Columnas Ocultas
                GridColumn PrefijoIDHist = new GridColumn();
                PrefijoIDHist.FieldName = this._unboundPrefix + "PrefijoID";
                PrefijoIDHist.UnboundType = UnboundColumnType.String;
                PrefijoIDHist.Visible = false;
                this.gvDocHistoria.Columns.Add(PrefijoIDHist);

                GridColumn DocumentoNroHist = new GridColumn();
                DocumentoNroHist.FieldName = this._unboundPrefix + "DocumentoNro";
                DocumentoNroHist.UnboundType = UnboundColumnType.String;
                DocumentoNroHist.Visible = false;
                this.gvDocHistoria.Columns.Add(DocumentoNroHist);

                GridColumn DocumentoTerceroHist = new GridColumn();
                DocumentoTerceroHist.FieldName = this._unboundPrefix + "DocumentoTercero";
                DocumentoTerceroHist.UnboundType = UnboundColumnType.String;
                DocumentoTerceroHist.Visible = false;
                this.gvDocHistoria.Columns.Add(DocumentoTerceroHist);
                #endregion
                #endregion
                this.gvDocPendientes.OptionsView.ColumnAutoWidth = true;
                this.gvDocLlamadas.OptionsView.ColumnAutoWidth = true;
                this.gvDocNotas.OptionsView.ColumnAutoWidth = true;
                this.gvDocHistoria.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TaskForm.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected bool ValidateRow(int fila)
        {
            bool validRow = true;
            try
            {
                //#region FechaVto
                //DTO_prContratoPolizas pol = (DTO_prContratoPolizas)this.gvGarantia.GetFocusedRow();
                //string msg = pol != null && pol.FechaVto.Value != null ? string.Empty : "Fecha Vencimiento Vacio";
                //GridColumn col = this.gvGarantia.Columns[this._unboundPrefix + "FechaVto"];
                //this.gvGarantia.SetColumnError(col, msg);
                //validField = string.IsNullOrEmpty(msg) ? true : false;
                //if (!validField)
                //    validRow = false;
                //#endregion
                //#region Observacion
                //validField = _bc.ValidGridCell(this.gvGarantia, string.Empty, fila, "Observacion", false, false, false, null);
                //if (!validField)
                //    validRow = false;
                //#endregion
                this.isValid = validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalConvenioProveedor.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_InfoTarea tarea = new DTO_InfoTarea();
            try
            {

                #region Asigna datos a la fila
                tarea.CerradoInd.Value = false;
                tarea.DocumentoID.Value = AppDocuments.RecordatorioActividadEstado;
                tarea.FechaInicio.Value = DateTime.Today.Date;
                #endregion

                this.detalleNotas.Add(tarea);
                this.gcDocNotas.DataSource = this.detalleNotas;
                this.gvDocNotas.RefreshData();
                this.gvDocNotas.FocusedRowHandle = this.gvDocNotas.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TaskForm.cs", "AddNewRow"));
            }
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this.frmModule);

                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento que se ejecuta para ver un anexo existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkVer_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = 0;
                if (this.tcTask.SelectedTabPage == this.tpPendientes)
                    fila = this.gvDocPendientes.FocusedRowHandle;
                else
                    fila = this.gvDocHistoria.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.detalle[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TaskForm.cs", "linkVer_Click"));
            }

        }

        /// <summary>
        /// Evento que se ejecuta al cambiar el valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbOrden_EditValueChanged(object sender, EventArgs e)
        {
            if (!this._firstTime)
            {
                if (this.cmbOrden.EditValue.Equals("1"))
                    this.detalle = this.detalle.OrderBy(x => x.FechaInicio.Value).ToList();
                else if (this.cmbOrden.EditValue.Equals("2"))
                    this.detalle = this.detalle.OrderBy(x => x.ActividadFlujoID.Value).ToList();
                else if (this.cmbOrden.EditValue.Equals("3"))
                    this.detalle = this.detalle.OrderBy(x => x.TerceroID.Value).ToList();
                this.gcDocPendientes.DataSource = this.detalle;
                this.gvDocPendientes.RefreshData();
            }
        }

        /// <summary>
        /// Se realiza al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            try
            {
                if (master.ValidID)
                {
                    DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);

                    if (this.tcTask.SelectedTabPage == this.tpLlamadas)
                    {
                        this.txtTelefonoLlam.Text = tercero.Tel1.Value;
                        this.txtDireccionLlam.Text = tercero.Direccion.Value;
                        this.txtCorreoLlam.Text = tercero.CECorporativo.Value;
                    }
                    else if (this.tcTask.SelectedTabPage == this.tpHistoria)
                    {
                        this.txtTelefonoHist.Text = tercero.Tel1.Value;
                        this.txtDireccionHist.Text = tercero.Direccion.Value;
                        this.txtCorreoHist.Text = tercero.CECorporativo.Value;
                    }
                }
                else
                {
                    if (this.tcTask.SelectedTabPage == this.tpLlamadas)
                    {
                        this.txtTelefonoLlam.Text = string.Empty;
                        this.txtDireccionLlam.Text = string.Empty; 
                        this.txtCorreoLlam.Text = string.Empty; 
                    }
                    else if (this.tcTask.SelectedTabPage == this.tpHistoria)
                    {
                        this.txtTelefonoHist.Text = string.Empty;
                        this.txtDireccionHist.Text = string.Empty;
                        this.txtCorreoHist.Text = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Se realiza al cambiar de pestana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcTask_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (this.tcTask.SelectedTabPage == this.tpNotas)
            {
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                this.dtFechaIniNotas.DateTime = DateTime.Now.Date;
                this.dtFechaFinNotas.DateTime = DateTime.Now.Date;
            }
            else
                FormProvider.Master.itemSave.Enabled = false;
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gv_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
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
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
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
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gv_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "VerDoc")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
            if (fieldName == "UnidadTiempo")
            {
                if (Convert.ToInt32(e.Value) == 1 || (Convert.ToInt32(e.Value) == 2))
                    e.DisplayText = "Hora";
                else if (Convert.ToInt32(e.Value) == 3 || (Convert.ToInt32(e.Value) == 4) || (Convert.ToInt32(e.Value) == 5))
                    e.DisplayText = "Día";
            }
            if (fieldName == "CerradoInd")
            {
                if (Convert.ToInt32(e.Value) == 0)
                    e.DisplayText = "AC";
                else
                    e.DisplayText = "CE";
            }

        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocNotas_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                this.deleteOP = false;
                if (this.isValid || this.gvDocNotas.RowCount == 0)
                    this.AddNewRow();
                else
                {
                    this.gvDocNotas.PostEditor();
                    bool isV = this.ValidateRow(this.gvDocNotas.FocusedRowHandle);
                    if (isV)
                        this.AddNewRow();
                }
            }
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove && this.gvDocNotas.RowCount > 0)
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                //Revisa si desea cargar los temporales
                if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.deleteOP = true;
                    int rowHandle = this.gvDocNotas.FocusedRowHandle;

                    this.gvDocNotas.DeleteRow(this.gvDocNotas.FocusedRowHandle);

                    if (this.gvDocNotas.RowCount > 0)
                    {
                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvDocNotas.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvDocNotas.FocusedRowHandle = rowHandle - 1;
                    }
                    else
                        this.gvDocNotas.Focus();
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocNotas_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
            this.deleteOP = false;

            if (validRow)
            {
                this.isValid = true;
            }
            else
            {
                e.Allow = false;
                this.isValid = false;
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.detalle = new List<DTO_InfoTarea>();

                this.gcDocPendientes.DataSource = this.detalle;
                this.gcDocLLamadas.DataSource = this.detalle;
                this.gcDocNotas.DataSource = this.detalleNotas;
                this.gcDocHistoria.DataSource = this.detalle;
                this.gvDocPendientes.RefreshData();
                this.gvDocLlamadas.RefreshData();
                this.gvDocNotas.RefreshData();
                this.gvDocHistoria.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-TaskForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocNotas.PostEditor();
            try
            {

                Thread process = new Thread(this.SaveThread);
                process.Start();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al asignar
        /// </summary>
        private void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                DTO_TxResult results = _bc.AdministrationModel.glActividadEstado_AddNotas(AppDocuments.RecordatorioActividadEstado, this.detalleNotas);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(results);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TaskForm.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        #endregion
    }
}
