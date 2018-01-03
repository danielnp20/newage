using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using SentenceTransformer;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class BloqueosAprob : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void RefreshDataMethod()
        {
            this.LoadData();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        List<DTO_prOrdenCompraAprob> _docs = null;
        //private int userID = 0;

        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private string unboundPrefixEP = "Unbound_";
        private string unboundPrefixDet = "Unbound_";
        private string unboundPrefixCarg = "Unbound_";        
        private bool multiMoneda;
        private DTO_prOrdenCompraAprob currentDoc = null;
        private DTO_prOrdenCompraAprobDet currentDet = null;
        private DTO_prSolicitudCargos currentCargo = null;
        private bool detailsLoaded = false;
        private bool detFooterLoaded = false;
        private int numDetails = 0;
        private int currentRow = -1;
        private int currentDetRow = -1;
        private int currentCargoRow = -1;
        private decimal _tasaCambio = 0;
        private DTO_glActividadPermiso tareaPerm;
        private bool allowValidate = true;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        #endregion

        public BloqueosAprob()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Asigna la lista de columnas
                this.AddDocumentCols();
                //this.AddEstadoPreCols();
                this.AddDetailCols();
                this.AddOrigenTrasladoCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadData();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion
                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "BloqueosAprob"));
            }
        }

        #region  Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            //this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.documentID = AppDocuments.AprobacionBloqueos;
            this.frmModule = ModulesPrefix.pl;

            this.gcDocuments.ShowOnlyPredefinedDetails = true;

            //Maestras
            this._bc.InitMasterUC(this.masterAreaFuncional, AppMasters.glAreaFuncional, false, true, true, true);
            this._bc.InitMasterUC(this.masterPrefijoDoc, AppMasters.glPrefijo, true, false, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterActividad, AppMasters.coActividad, true, true, true, false);
            this._bc.InitMasterUC(this.masterRecurso, AppMasters.plRecurso, true, true, true, false);

            //Combo
            Dictionary<string, string> dicTipoDoc = new Dictionary<string, string>();
            dicTipoDoc.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_All));
            dicTipoDoc.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Solicitudes));
            dicTipoDoc.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_OrdenCompra));
            this.cmbTipoDoc.Properties.DataSource = dicTipoDoc;
            this.cmbTipoDoc.EditValue = 1;
        }

        /// <summary>
        /// Carga la información del formulario
        /// </summary>
        private void LoadData()
        {
            try
            {
                //this.masterProveedor.Value = string.Empty;
                //this.masterAreaFuncional.Value = string.Empty;

                //this.txtTotalMdaExt.EditValue = 0;
                //this.txtTotalMdaLocal.EditValue = 0;
                //this.txtObservDetalle.Text = string.Empty;
                //this.txtJustif.Text = string.Empty;
                //this.txtObservDoc.Text = string.Empty;

                //List<DTO_prOrdenCompraAprob> temp = _bc.AdministrationModel.OrdenCompra_GetPendientesByModulo(this.documentID, this.actividadFlujoID, this._bc.AdministrationModel.User);
                //this._docs = temp;
                //foreach (var item in this._docs)
                //    item.FileUrl = string.Empty;
                //this.LoadDocuments();

                //FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información en la grilla de decomentos
        /// </summary>
        private void LoadDocuments()
        {
            //this.currentDoc = null;
            //this.currentRow = -1;
            //this.gcDocuments.DataSource = null;
            
            //if (this._docs != null && this._docs.Count > 0)
            //{
            //    this.detailsLoaded = false;
            //    this.allowValidate = false;
            //    this.currentRow = 0;
            //    this.gcDocuments.DataSource = this._docs;
            //    this.allowValidate = true;

            //    if (!detailsLoaded)
            //    {
            //        this.currentDoc = (DTO_prOrdenCompraAprob)this.gvDocuments.GetRow(this.currentRow);   
            //        this.LoadDetails();
            //    }

            //    string monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
            //    #region Assignar valores
            //    this.masterProveedor.Value = this.currentDoc.ProveedorID.Value;
            //    this.masterAreaFuncional.Value = this.currentDoc.LugarEntrega.Value;
            //    this.txtTotalMdaLocal.EditValue = this.currentDoc.ValorTotalML.Value.Value;
            //    if (this.multiMoneda)
            //    {
            //        this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjera, this.currentDoc.Fecha.Value.Value);
            //        this.txtTotalMdaExt.EditValue = _tasaCambio != 0 ? this.currentDoc.ValorTotalML.Value.Value / _tasaCambio : 0;
            //    }
            //    else
            //    {
            //        this.lblTotalMdaExt.Visible = false;
            //        this.txtTotalMdaExt.Visible = false;
            //    }
            //    this.txtJustif.Text = this.currentDoc.Justificacion.Value;
            //    #endregion
            //    this.gvDocuments.MoveFirst();
            //}
            //else
            //{
            //    this.gcDetails.DataSource = null;
            //    this.gcOrigen.DataSource = null;
            //}
        }

        /// <summary>
        /// Carga la información sn la grilla del detalle
        /// </summary>
        private void LoadDetails()
        {
            try
            {
                //this.currentDet = null;
                //this.currentDetRow = -1;

                //DTO_prOrdenCompraAprob doc = this.currentDoc;
                //string prefijo = doc.PrefijoID.Value;
                //int ordenNro = doc.DocumentoNro.Value.Value;

                ////List<DTO_prSolicitudAsignDet> details = null;
                //if (doc != null && this._docs.Exists(d => d.NumeroDoc.Value == doc.NumeroDoc.Value) 
                //    && this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet.Count != 0)
                //{
                //    this.detFooterLoaded = false;
                //    this.currentDetRow = 0;
                //    //details = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet;
                //    this.gcDetails.DataSource = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet;
                //    this.currentDet = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet[this.currentDetRow];
                    
                //    this.LoadDetFooter();
                //    this.gvDetails.MoveFirst();
                //}
                //else
                //{
                //    //details = new List<DTO_prSolicitudAsignDet>();
                //    this.gcDetails.DataSource = null;
                //    this.gcOrigen.DataSource = null;
                //}

                //this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        private void LoadDetFooter()
        {
            try
            {
                //this.currentCargo = null;
                //this.currentCargoRow = -1;

                //DTO_prOrdenCompraAprobDet det = this.currentDet;

                //if (det != null && det.SolicitudCargos.Count > 0)
                //{
                //    this.currentCargoRow = 0;
                //    this.gcOrigen.DataSource = det.SolicitudCargos;
                //    this.currentCargo = det.SolicitudCargos[this.currentCargoRow];
                //    #region Assignar valores
                //    this.txtObservDetalle.Text = this.currentDet.Descriptivo.Value;
                //    #endregion
                //    this.gvOrigenTraslado.MoveFirst();
                //}
                //else
                //    this.gcOrigen.DataSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Gerencia
                GridColumn Gerencia = new GridColumn();
                Gerencia.FieldName = this.unboundPrefix + "Gerencia";
                Gerencia.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Gerencia");
                Gerencia.UnboundType = UnboundColumnType.String;
                Gerencia.VisibleIndex = 1;
                Gerencia.Width = 50;
                Gerencia.Visible = true;
                Gerencia.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(Gerencia);

                //TipoDoc
                GridColumn TipoDoc = new GridColumn();
                TipoDoc.FieldName = this.unboundPrefix + "TipoDoc";
                TipoDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoDoc");
                TipoDoc.UnboundType = UnboundColumnType.Integer;
                TipoDoc.VisibleIndex = 2;
                TipoDoc.Width = 50;
                TipoDoc.Visible = true;
                TipoDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(TipoDoc);

                //Prefijo - Documento Numero
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 3;
                prefDoc.Width = 50;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(prefDoc);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 4;
                fecha.Width = 50;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);    

                //MonedaID
                GridColumn MonedaID = new GridColumn();
                MonedaID.FieldName = this.unboundPrefix + "MonedaID";
                MonedaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                MonedaID.UnboundType = UnboundColumnType.String;
                MonedaID.VisibleIndex = 5;
                MonedaID.Width = 50;
                MonedaID.Visible = true;
                MonedaID.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(MonedaID);

                //Accion
                GridColumn Accion = new GridColumn();
                Accion.FieldName = this.unboundPrefix + "Accion";
                Accion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Accion");
                Accion.UnboundType = UnboundColumnType.Integer;
                Accion.VisibleIndex = 6;
                Accion.Width = 50;
                Accion.Visible = true;
                Accion.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(Accion);

                //VerAprob
                GridColumn VerAprob = new GridColumn();
                VerAprob.FieldName = this.unboundPrefix + "VerAprob";
                VerAprob.OptionsColumn.ShowCaption = false;
                VerAprob.UnboundType = UnboundColumnType.String;
                VerAprob.Width = 60;
                VerAprob.ColumnEdit = this.editLink;
                VerAprob.VisibleIndex = 7;
                VerAprob.Visible = true;
                VerAprob.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(VerAprob);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddDetailCols()
        {
            try
            {
                #region Columnas basicas

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefixDet + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 1;
                ProyectoID.Width = 60;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(ProyectoID);

                //RecursoID
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefixDet + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 2;
                RecursoID.Width = 60;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(RecursoID);

                //LineaPresupuestoID
                GridColumn LineaPresupuestoID = new GridColumn();
                LineaPresupuestoID.FieldName = this.unboundPrefixDet + "LineaPresupuestoID";
                LineaPresupuestoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
                LineaPresupuestoID.UnboundType = UnboundColumnType.String;
                LineaPresupuestoID.VisibleIndex = 3;
                LineaPresupuestoID.Width = 60;
                LineaPresupuestoID.Visible = true;
                LineaPresupuestoID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(LineaPresupuestoID);

                //CodigoServicios
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefixDet + "CodigoBSID";
                CodigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 4;
                CodigoBSID.Width = 60;
                CodigoBSID.Visible = true;
                CodigoBSID.Fixed = FixedStyle.Left;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(CodigoBSID);        

                //PresupuestoCant
                GridColumn PresupuestoCant = new GridColumn();
                PresupuestoCant.FieldName = this.unboundPrefixDet + "PresupuestoCant";
                PresupuestoCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PresupuestoCant");
                PresupuestoCant.UnboundType = UnboundColumnType.Decimal;
                PresupuestoCant.VisibleIndex = 5;
                PresupuestoCant.Width = 60;
                PresupuestoCant.Visible = true;
                PresupuestoCant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(PresupuestoCant);

                //PresupuestoValor
                GridColumn PresupuestoValor = new GridColumn();
                PresupuestoValor.FieldName = this.unboundPrefixDet + "PresupuestoValor";
                PresupuestoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PresupuestoValor");
                PresupuestoValor.UnboundType = UnboundColumnType.Decimal;
                PresupuestoValor.VisibleIndex = 6;
                PresupuestoValor.Width = 60;
                PresupuestoValor.Visible = true;
                PresupuestoValor.ColumnEdit = this.editSpin;
                PresupuestoValor.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(PresupuestoValor);

                //SolicitadoCant
                GridColumn SolicitadoCant = new GridColumn();
                SolicitadoCant.FieldName = this.unboundPrefixDet + "SolicitadoCant";
                SolicitadoCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SolicitadoCant");
                SolicitadoCant.UnboundType = UnboundColumnType.Decimal;
                SolicitadoCant.VisibleIndex = 7;
                SolicitadoCant.Width = 60;
                SolicitadoCant.Visible = true;
                SolicitadoCant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(SolicitadoCant);

                //SolicitadoValor
                GridColumn SolicitadoValor = new GridColumn();
                SolicitadoValor.FieldName = this.unboundPrefixDet + "SolicitadoValor";
                SolicitadoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SolicitadoValor");
                SolicitadoValor.UnboundType = UnboundColumnType.Decimal;
                SolicitadoValor.VisibleIndex = 8;
                SolicitadoValor.Width = 60;
                SolicitadoValor.Visible = true;
                SolicitadoValor.ColumnEdit = this.editSpin;
                SolicitadoValor.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(SolicitadoValor);

                //DisponibleCant
                GridColumn DisponibleCant = new GridColumn();
                DisponibleCant.FieldName = this.unboundPrefixDet + "DisponibleCant";
                DisponibleCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DisponibleCant");
                DisponibleCant.UnboundType = UnboundColumnType.Decimal;
                DisponibleCant.VisibleIndex = 9;
                DisponibleCant.Width = 60;
                DisponibleCant.Visible = true;
                DisponibleCant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(DisponibleCant);

                //DisponibleValor
                GridColumn DisponibleValor = new GridColumn();
                DisponibleValor.FieldName = this.unboundPrefixDet + "DisponibleValor";
                DisponibleValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DisponibleValor");
                DisponibleValor.UnboundType = UnboundColumnType.Decimal;
                DisponibleValor.VisibleIndex = 10;
                DisponibleValor.Width = 60;
                DisponibleValor.Visible = true;
                DisponibleValor.ColumnEdit = this.editSpin;
                DisponibleValor.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(DisponibleValor);

                //SobreEjecucionCant
                GridColumn SobreEjecucionCant = new GridColumn();
                SobreEjecucionCant.FieldName = this.unboundPrefixDet + "SobreEjecucionCant";
                SobreEjecucionCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SobreEjecucionCant");
                SobreEjecucionCant.UnboundType = UnboundColumnType.Decimal;
                SobreEjecucionCant.VisibleIndex = 11;
                SobreEjecucionCant.Width = 60;
                SobreEjecucionCant.Visible = true;
                SobreEjecucionCant.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(SobreEjecucionCant);

                //SobreEjecucionValor
                GridColumn SobreEjecucionValor = new GridColumn();
                SobreEjecucionValor.FieldName = this.unboundPrefixDet + "SobreEjecucionValor";
                SobreEjecucionValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SobreEjecucionValor");
                SobreEjecucionValor.UnboundType = UnboundColumnType.Decimal;
                SobreEjecucionValor.VisibleIndex = 12;
                SobreEjecucionValor.Width = 60;
                SobreEjecucionValor.Visible = true;
                SobreEjecucionValor.ColumnEdit = this.editSpin;
                SobreEjecucionValor.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(SobreEjecucionValor);

                //Adicion
                GridColumn Adicion = new GridColumn();
                Adicion.FieldName = this.unboundPrefixDet + "Adicion";
                Adicion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Adicion");
                Adicion.UnboundType = UnboundColumnType.Decimal;
                Adicion.VisibleIndex = 13;
                Adicion.Width = 60;
                Adicion.Visible = true;
                Adicion.ColumnEdit = this.editSpin;
                Adicion.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(Adicion);

                //Traslado
                GridColumn Traslado = new GridColumn();
                Traslado.FieldName = this.unboundPrefixDet + "Traslado";
                Traslado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Traslado");
                Traslado.UnboundType = UnboundColumnType.Decimal;
                Traslado.VisibleIndex = 14;
                Traslado.Width = 60;
                Traslado.Visible = true;
                Traslado.ColumnEdit = this.editSpin;
                Traslado.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(Traslado);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        private void AddOrigenTrasladoCols()
        {
            try
            {
                #region Columnas basicas

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefixDet + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 1;
                ProyectoID.Width = 60;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvOrigenTraslado.Columns.Add(ProyectoID);

                //RecursoID
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefixDet + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 2;
                RecursoID.Width = 60;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                this.gvOrigenTraslado.Columns.Add(RecursoID);

                //LineaPresupuestoID
                GridColumn LineaPresupuestoID = new GridColumn();
                LineaPresupuestoID.FieldName = this.unboundPrefixDet + "LineaPresupuestoID";
                LineaPresupuestoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
                LineaPresupuestoID.UnboundType = UnboundColumnType.String;
                LineaPresupuestoID.VisibleIndex = 3;
                LineaPresupuestoID.Width = 60;
                LineaPresupuestoID.Visible = true;
                LineaPresupuestoID.OptionsColumn.AllowEdit = false;
                this.gvOrigenTraslado.Columns.Add(LineaPresupuestoID);

                //CodigoServicios
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefixDet + "CodigoBSID";
                CodigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 4;
                CodigoBSID.Width = 70;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvOrigenTraslado.Columns.Add(CodigoBSID);
        
                //DisponibleCant
                GridColumn DisponibleCant = new GridColumn();
                DisponibleCant.FieldName = this.unboundPrefixDet + "DisponibleCant";
                DisponibleCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DisponibleCant");
                DisponibleCant.UnboundType = UnboundColumnType.Decimal;
                DisponibleCant.VisibleIndex = 5;
                DisponibleCant.Width = 60;
                DisponibleCant.Visible = true;
                DisponibleCant.OptionsColumn.AllowEdit = true;
                this.gvOrigenTraslado.Columns.Add(DisponibleCant);

                //DisponibleValor
                GridColumn DisponibleValor = new GridColumn();
                DisponibleValor.FieldName = this.unboundPrefixDet + "DisponibleValor";
                DisponibleValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DisponibleValor");
                DisponibleValor.UnboundType = UnboundColumnType.Decimal;
                DisponibleValor.VisibleIndex = 6;
                DisponibleValor.Width = 60;
                DisponibleValor.Visible = true;
                DisponibleValor.ColumnEdit = this.editSpin;
                DisponibleValor.OptionsColumn.AllowEdit = true;
                this.gvOrigenTraslado.Columns.Add(DisponibleValor);

                //TrasladoCant
                GridColumn TrasladoCant = new GridColumn();
                TrasladoCant.FieldName = this.unboundPrefixDet + "TrasladoCant";
                TrasladoCant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrasladoCant");
                TrasladoCant.UnboundType = UnboundColumnType.Decimal;
                TrasladoCant.VisibleIndex = 7;
                TrasladoCant.Width = 60;
                TrasladoCant.Visible = true;
                TrasladoCant.OptionsColumn.AllowEdit = true;
                this.gvOrigenTraslado.Columns.Add(TrasladoCant);

                //TrasladoValor
                GridColumn TrasladoValor = new GridColumn();
                TrasladoValor.FieldName = this.unboundPrefixDet + "TrasladoValor";
                TrasladoValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TrasladoValor");
                TrasladoValor.UnboundType = UnboundColumnType.Decimal;
                TrasladoValor.VisibleIndex = 8;
                TrasladoValor.Width = 60;
                TrasladoValor.Visible = true;
                TrasladoValor.ColumnEdit = this.editSpin;
                TrasladoValor.OptionsColumn.AllowEdit = true;
                this.gvOrigenTraslado.Columns.Add(TrasladoValor);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected virtual bool ValidateDocRow(int fila)
        {
            string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
            bool rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);

            if (rechazado)
            {
                col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();

                if (string.IsNullOrEmpty(desc))
                {
                    string msg = string.Format(rsxEmpty, col.Caption);
                    this.gvDocuments.SetColumnError(col, msg);
                    return false;
                }
            }

            this.gvDocuments.SetColumnError(col, string.Empty);
            return true;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this.unboundPrefix.Length;
            if (dataType == typeof(DTO_prSolicitudCargos))
                unboundPrefixLen = this.unboundPrefixCarg.Length;
            if (dataType == typeof(DTO_prOrdenCompraAprobDet))
                unboundPrefixLen = this.unboundPrefixDet.Length;

            string fieldName = e.Column.FieldName.Substring(unboundPrefixLen);

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
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //if (this.currentRow != -1)
            //{
            //    if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
            //        this.currentRow = e.FocusedRowHandle;

            //    this.currentDoc = (DTO_prOrdenCompraAprob)this.gvDocuments.GetRow(this.currentRow);
            //    this.LoadDetails();
            //    this.txtObservDoc.Text = this.currentDoc.Observacion.Value;
                
            //    this.detailsLoaded = true;
            //}
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    this._docs[e.RowHandle].Rechazado.Value = false;
                    this.txtObservDoc.Enabled = false;
                    this.txtObservDoc.Text = string.Empty;
                    this._docs[e.RowHandle].Observacion.Value = string.Empty;
                }
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    this._docs[e.RowHandle].Aprobado.Value = false;
                    this.txtObservDoc.Enabled = true;
                }
                else
                {
                    this.txtObservDoc.Enabled = false;
                    this.txtObservDoc.Text = string.Empty;
                    this._docs[e.RowHandle].Observacion.Value = string.Empty;
                }
            };

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Calcula los valores y hace operacines cuando los valores etstan engresados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Observacion" && e.Value.ToString() != this.txtObservDoc.Text)
            {
                this.txtObservDoc.Text = e.Value.ToString();
            }

            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                e.Allow = false;
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ValorTotalML" || fieldName == "ValorUni" || fieldName == "ValorTotML" || fieldName == "IvaTotML")
            {
                e.RepositoryItem = this.editSpin;
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ViewDoc")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }
        #endregion

        #region Eventos grilla de Detalles

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentDetRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDetails.RowCount - 1)
                    this.currentDetRow = e.FocusedRowHandle;

                this.currentDet = (DTO_prOrdenCompraAprobDet)this.gvDetails.GetRow(this.currentDetRow);
                this.LoadDetFooter();
                this.detFooterLoaded = true;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetFooter_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentCargoRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvOrigenTraslado.RowCount - 1)
                    this.currentCargoRow = e.FocusedRowHandle;

                this.currentCargo = (DTO_prSolicitudCargos)this.gvOrigenTraslado.GetRow(this.currentCargoRow);
            }
        }
      
        #endregion
                
        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            this.gvDetails.PostEditor();
            try
            {
                if (this._docs != null && this._docs.Count != 0)
                {
                    if (this.ValidateDocRow(this.currentRow))
                    {
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Eventos Controles

        private void txtObserv_Leave(object sender, EventArgs e)
        {
            this._docs[this.currentRow].Observacion.Value = this.txtObservDoc.Text;
            this.ValidateDocRow(this.currentRow);
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
                int fila = this.gvDocuments.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._docs[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOdenCompra.cs", "editLink_Click"));
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                List<DTO_SerializedObject> results = _bc.AdministrationModel.OrdenCompra_AprobarRechazar(AppDocuments.OrdenCompAprob, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, false);
               
                FormProvider.Master.StopProgressBarThread(this.documentID);

                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<int> docsOK = new List<int>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object obj in results)
                {
                    #region Funciones de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (this._docs[i].Aprobado.Value.Value || this._docs[i].Rechazado.Value.Value)
                    {
                        MailType mType = this._docs[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                        }
                        else
                        {
                            DTO_Alarma r = (DTO_Alarma)obj;
                            int numDoc = Convert.ToInt32(r.NumeroDoc);
                            docsOK.Add(numDoc);
                        }
                    }
                    i++;
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                #region Pregunta si desea abrir los reportes

                bool deseaImp = false;
                if (docsOK.Count > 0)
                {
                    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                    var result = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        deseaImp = true;
                }

                #endregion
                #region Genera e imprime los reportes
                foreach (int item in docsOK)
                {
                    string reportName = this._bc.AdministrationModel.ReportesProveedores_OrdenCompra(item, 1,false);
                    if (deseaImp)
                    {
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, item, null, reportName.ToString());
                        Process.Start(fileURl);
                    }
                }
                #endregion

                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BloqueosAprob.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

 
    }
}
