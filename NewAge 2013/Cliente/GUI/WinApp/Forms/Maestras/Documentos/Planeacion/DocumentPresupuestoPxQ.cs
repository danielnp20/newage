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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using DevExpress.XtraEditors.Controls;
using System.Globalization;
using System.IO;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DocumentPresupuestoPxQ : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            //this.presupuesto = new DTO_Presupuesto();
            //this.detListFinal = new List<DTO_plDocumentDocumentPresupuestoPxQDeta>();
            //this.EnableControls(true);
        }

        protected delegate void SendToApprove();
        protected SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected void SendToApproveMethod()
        {
            try
            {
                //this.presupuesto = new DTO_Presupuesto();
                //this.detListFinal = new List<DTO_plDocumentDocumentPresupuestoPxQDeta>();
                //this.EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentDocumentPresupuestoPxQ.cs", "sendToApproveDelegate"));
            }
        }

        protected delegate void RefreshData();
        protected RefreshData refreshDataDelegate;
        /// <summary>
        /// Delegado que actualiza o refresca el formulario 
        /// </summary>
        protected void RefreshDataMethod()
        {
            try
            {
                this.dtPeriod.Enabled = false;
                this.masterProyecto.EnableControl(false);
                this.gcDocument.DataSource = this.detListFinal;
                //this.gvDetail.FocusedRowHandle = 0;
                //for (int i = 0; i < this.detList.Count; i++)
                //    this.LoadParticiones(i, false);
                //this.gcDetail.RefreshDataSource();
                this.gvDocument.MoveLast();
                //this.LoadFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentDocumentPresupuestoPxQ.cs", "sendToApproveDelegate"));
            }
        }

        #endregion

        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected int documentID;

        //Variables de MDI
        protected FormTypes _frmType = FormTypes.Document;
        protected string _frmName;
        protected ModulesPrefix _frmModule;
        //Variables con valores x defecto (glControl)
        protected string LineaPresupuestoIDxDef = string.Empty;
        //Variables de formulario
        protected DateTime periodo = DateTime.Now;
        protected string proyectoID = string.Empty;
        protected string locFisicaID = string.Empty;
        protected string areaFisicaID = string.Empty;
        protected string actividadID = string.Empty;
        protected string centroCostoIDxDef = string.Empty;
        protected bool validHeader = false;
        protected bool deleteOP = false;
        protected bool loadME = false;
        //Variables de documentos y detalles
        protected bool isValid_Det = true;
        protected bool disableValidate_Det = false;
        protected DTO_Presupuesto presupuesto = new DTO_Presupuesto();
        protected List<DTO_plPresupuestoPxQDeta> detListFinal = new List<DTO_plPresupuestoPxQDeta>();
        protected int numeroDocPresup = 0;
        //Variables para importar
        protected PasteOpDTO pasteRet;
        protected string format;
        protected string formatSeparator = "\t";
        protected string unboundPrefix = "Unbound_";
        //Filtros y actividades     
        protected List<DTO_glConsultaFiltro> filtrosLineaPres;
        protected List<DTO_glConsultaFiltro> filtrosCentroCosto;
        protected List<DTO_glConsultaFiltro> filtrosProyecto;
        protected List<DTO_glConsultaFiltro> filtrosActividad;
        protected List<DTO_glConsultaFiltro> filtrosAreaFisica;
        protected DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();

        #endregion Variables

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        protected bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DocumentPresupuestoPxQ()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de la actividad
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this.actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "DocumentPresupuestoPxQ"));
            }
        }

        #region Funciones protected del formulario

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            #region Inicia variables
            //Carga los valores por defecto               
            this.LineaPresupuestoIDxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.centroCostoIDxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            //Tasa de cambio
            string multimonedaStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
            if (multimonedaStr == "1")
            {
                this.loadME = true;
                this.lblTasaCambio.Visible = true;
                this.txtTasaCambio.Visible = true;
            }

            #endregion
            #region Inicia controles

            this.AddGriCols();
            this.GetFiltersMasters();
            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = this.periodo;
            this.dtPeriod.MinValue = this.periodo;

            //Maestras
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false, this.filtrosProyecto);
            this._bc.InitMasterUC(this.masterActividad, AppMasters.coActividad, true, true, true, false, this.filtrosActividad);
            this._bc.InitMasterUC(this.masterCampo, AppMasters.glAreaFisica, false, true, true, false, this.filtrosAreaFisica);
            this._bc.InitMasterUC(this.masterLineaFilter, AppMasters.plLineaPresupuesto, true, true, true, false, this.filtrosLineaPres);
            this._bc.InitMasterUC(this.masterContrato, AppMasters.pyContrato, true, true, true, false);
            this._bc.InitMasterUC(this.masterRecursoFilter, AppMasters.plRecurso, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoOC, AppMasters.glPrefijo, true, true, true, false);

            //Combo
            Dictionary<string, string> dicTipoProyecto = new Dictionary<string, string>();
            dicTipoProyecto.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Capex));
            dicTipoProyecto.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Opex));
            dicTipoProyecto.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inversion));
            dicTipoProyecto.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Administrativo));
            this.cmbTipoProyecto.Properties.DataSource = dicTipoProyecto;
            this.cmbTipoProyecto.EditValue = 1;

            #endregion

            //Delegados
            this.saveDelegate = new Save(this.SaveMethod);
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
            this.refreshDataDelegate = new RefreshData(this.RefreshDataMethod);
        }

        /// <summary>
        /// Agrega las columnas a la subgrilla
        /// </summary>
        protected void AddGriCols()
        {
            try
            {
                #region Grilla Principal
                //AreaFisica(Campo)
                GridColumn AreaFisicaID = new GridColumn();
                AreaFisicaID.FieldName = this.unboundPrefix + "AreaFisicaID";
                AreaFisicaID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_AreaFisica");
                AreaFisicaID.UnboundType = UnboundColumnType.String;
                AreaFisicaID.VisibleIndex = 0;
                AreaFisicaID.Width = 40;
                AreaFisicaID.Visible = true;
                AreaFisicaID.OptionsColumn.AllowEdit = false;
                AreaFisicaID.ColumnEdit = this.editBtn_Doc;
                this.gvDocument.Columns.Add(AreaFisicaID);

                //AreaFisicaDesc
                GridColumn AreaFisicaDesc = new GridColumn();
                AreaFisicaDesc.FieldName = this.unboundPrefix + "AreaFisicaDesc";
                AreaFisicaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_AreaFisicaDesc");
                AreaFisicaDesc.UnboundType = UnboundColumnType.String;
                AreaFisicaDesc.VisibleIndex = 1;
                AreaFisicaDesc.Width = 40;
                AreaFisicaDesc.Visible = true;
                AreaFisicaDesc.OptionsColumn.AllowEdit = false;
                AreaFisicaDesc.ColumnEdit = this.editBtn_Doc;
                this.gvDocument.Columns.Add(AreaFisicaDesc);

                //PresupuestoLoc
                GridColumn PresupuestoLoc = new GridColumn();
                PresupuestoLoc.FieldName = this.unboundPrefix + "PresupuestoLoc";
                PresupuestoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_PresupuestoLoc");
                PresupuestoLoc.UnboundType = UnboundColumnType.Decimal;
                PresupuestoLoc.VisibleIndex = 2;
                PresupuestoLoc.Width = 40;
                PresupuestoLoc.Visible = true;
                PresupuestoLoc.OptionsColumn.AllowEdit = false;
                PresupuestoLoc.ColumnEdit = this.editValor;
                this.gvDocument.Columns.Add(PresupuestoLoc);

                //PresupuestoExt
                GridColumn PresupuestoExt = new GridColumn();
                PresupuestoExt.FieldName = this.unboundPrefix + "PresupuestoExt";
                PresupuestoExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_PresupuestoExt");
                PresupuestoExt.UnboundType = UnboundColumnType.Decimal;
                PresupuestoExt.VisibleIndex = 3;
                PresupuestoExt.Width = 40;
                PresupuestoExt.Visible = true;
                PresupuestoExt.OptionsColumn.AllowEdit = false;
                PresupuestoExt.ColumnEdit = this.editValor;
                this.gvDocument.Columns.Add(PresupuestoExt);

                #endregion

                #region Grilla Detalle
                //RecursoID
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 0;
                RecursoID.Width = 40;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                RecursoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(RecursoID);

                //LineaPresupuestoID
                GridColumn lineaPresupuestoID = new GridColumn();
                lineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                lineaPresupuestoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_LineaPresupuestoID");
                lineaPresupuestoID.UnboundType = UnboundColumnType.String;
                lineaPresupuestoID.VisibleIndex = 1;
                lineaPresupuestoID.Width = 40;
                lineaPresupuestoID.Visible = true;
                lineaPresupuestoID.OptionsColumn.AllowEdit = false;
                lineaPresupuestoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(lineaPresupuestoID);

                //CodigoBSID
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 2;
                CodigoBSID.Width = 40;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                CodigoBSID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(CodigoBSID);

                //UnidadInvID
                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.VisibleIndex = 3;
                UnidadInvID.Width = 30;
                UnidadInvID.Visible = true;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                CodigoBSID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(UnidadInvID);

                //ValorUniOCLoc
                GridColumn ValorUniOCLoc = new GridColumn();
                ValorUniOCLoc.FieldName = this.unboundPrefix + "ValorUniOCLoc";
                ValorUniOCLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_ValorUniOCLoc");
                ValorUniOCLoc.UnboundType = UnboundColumnType.Decimal;
                ValorUniOCLoc.VisibleIndex = 4;
                ValorUniOCLoc.Width = 35;
                ValorUniOCLoc.Visible = this.documentID == AppDocuments.PresupuestoPxQ? true : false;
                ValorUniOCLoc.OptionsColumn.AllowEdit = false;
                ValorUniOCLoc.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(ValorUniOCLoc);

                //ValorUniOCExt
                GridColumn ValorUniOCExt = new GridColumn();
                ValorUniOCExt.FieldName = this.unboundPrefix + "ValorUniOCExt";
                ValorUniOCExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_ValorUniOCExt");
                ValorUniOCExt.UnboundType = UnboundColumnType.Decimal;
                ValorUniOCExt.VisibleIndex = 5;
                ValorUniOCExt.Width = 35;
                ValorUniOCExt.Visible = this.documentID == AppDocuments.PresupuestoPxQ ? true : false; ;
                ValorUniOCExt.OptionsColumn.AllowEdit = false;
                ValorUniOCExt.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(ValorUniOCExt);

                //PorcentajeVar
                GridColumn PorcentajeVar = new GridColumn();
                PorcentajeVar.FieldName = this.unboundPrefix + "PorcentajeVar";
                PorcentajeVar.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_PorcentajeVar");
                PorcentajeVar.UnboundType = UnboundColumnType.Decimal;
                PorcentajeVar.VisibleIndex = 6;
                PorcentajeVar.Width = 30;
                PorcentajeVar.Visible = this.documentID == AppDocuments.PresupuestoPxQ ? true : false; 
                PorcentajeVar.OptionsColumn.AllowEdit = true;
                this.gvDetalle.Columns.Add(PorcentajeVar);

                //ValorUniLoc
                GridColumn ValorUniLoc = new GridColumn();
                ValorUniLoc.FieldName = this.unboundPrefix + "ValorUniLoc";
                ValorUniLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_ValorUniLoc");
                ValorUniLoc.UnboundType = UnboundColumnType.Decimal;
                ValorUniLoc.VisibleIndex = 7;
                ValorUniLoc.Width = 40;
                ValorUniLoc.Visible = true;
                ValorUniLoc.OptionsColumn.AllowEdit = this.documentID == AppDocuments.PresupuestoPxQ ? true : false;
                ValorUniLoc.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(ValorUniLoc);

                //ValorUniExt
                GridColumn ValorUniExt = new GridColumn();
                ValorUniExt.FieldName = this.unboundPrefix + "ValorUniExt";
                ValorUniExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_ValorUniExt");
                ValorUniExt.UnboundType = UnboundColumnType.Decimal;
                ValorUniExt.VisibleIndex = 8;
                ValorUniExt.Width = 40;
                ValorUniExt.Visible = true;
                ValorUniExt.OptionsColumn.AllowEdit = this.documentID == AppDocuments.PresupuestoPxQ ? true : false;
                ValorUniExt.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(ValorUniExt);

                //CantidadPRELoc
                GridColumn CantidadPRELoc = new GridColumn();
                CantidadPRELoc.FieldName = this.unboundPrefix + "CantidadPRELoc";
                CantidadPRELoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_CantidadPRELoc");
                CantidadPRELoc.UnboundType = UnboundColumnType.Decimal;
                CantidadPRELoc.VisibleIndex = 9;
                CantidadPRELoc.Width = 35;
                CantidadPRELoc.Visible = true;
                CantidadPRELoc.OptionsColumn.AllowEdit = this.documentID == AppDocuments.PresupuestoPxQ ? true : false;
                this.gvDetalle.Columns.Add(CantidadPRELoc);

                //CantidadPREExt
                GridColumn CantidadPREExt = new GridColumn();
                CantidadPREExt.FieldName = this.unboundPrefix + "CantidadPREExt";
                CantidadPREExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_CantidadPREExt");
                CantidadPREExt.UnboundType = UnboundColumnType.Decimal;
                CantidadPREExt.VisibleIndex = 10;
                CantidadPREExt.Width = 35;
                CantidadPREExt.Visible = true;
                CantidadPREExt.OptionsColumn.AllowEdit = this.documentID == AppDocuments.PresupuestoPxQ ? true : false;
                this.gvDetalle.Columns.Add(CantidadPREExt);

                //PresupuestoLocDet
                GridColumn PresupuestoLocDet = new GridColumn();
                PresupuestoLocDet.FieldName = this.unboundPrefix + "PresupuestoLoc";
                PresupuestoLocDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_PresupuestoLocDet");
                PresupuestoLocDet.UnboundType = UnboundColumnType.Decimal;
                PresupuestoLocDet.VisibleIndex = 11;
                PresupuestoLocDet.Width = 45;
                PresupuestoLocDet.Visible = true;
                PresupuestoLocDet.OptionsColumn.AllowEdit = false;
                PresupuestoLocDet.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(PresupuestoLocDet);

                //PresupuestoExtDet
                GridColumn PresupuestoExtDet = new GridColumn();
                PresupuestoExtDet.FieldName = this.unboundPrefix + "PresupuestoExt";
                PresupuestoExtDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_PresupuestoExtDet");
                PresupuestoExtDet.UnboundType = UnboundColumnType.Decimal;
                PresupuestoExtDet.VisibleIndex = 12;
                PresupuestoExtDet.Width = 45;
                PresupuestoExtDet.Visible = true;
                PresupuestoExtDet.OptionsColumn.AllowEdit = false;
                PresupuestoExtDet.ColumnEdit = this.editValor;
                this.gvDetalle.Columns.Add(PresupuestoExtDet);

                //NuevaCantidadPRELoc
                GridColumn NuevaCantidadPRELoc = new GridColumn();
                NuevaCantidadPRELoc.FieldName = this.unboundPrefix + "NuevaCantidadPRELoc";
                NuevaCantidadPRELoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_NuevaCantidadPRELoc");
                NuevaCantidadPRELoc.UnboundType = UnboundColumnType.Decimal;
                NuevaCantidadPRELoc.VisibleIndex = 13;
                NuevaCantidadPRELoc.Width = 37;
                NuevaCantidadPRELoc.Visible = this.documentID != AppDocuments.PresupuestoPxQ ? true : false;
                NuevaCantidadPRELoc.OptionsColumn.AllowEdit = true;
                NuevaCantidadPRELoc.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                NuevaCantidadPRELoc.AppearanceHeader.Options.UseTextOptions = true;
                NuevaCantidadPRELoc.AppearanceHeader.Options.UseFont = true;
                NuevaCantidadPRELoc.AppearanceHeader.Options.UseForeColor = true;
                NuevaCantidadPRELoc.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevaCantidadPRELoc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                this.gvDetalle.Columns.Add(NuevaCantidadPRELoc);

                //NuevaCantidadPREExt
                GridColumn NuevaCantidadPREExt = new GridColumn();
                NuevaCantidadPREExt.FieldName = this.unboundPrefix + "NuevaCantidadPREExt";
                NuevaCantidadPREExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_NuevaCantidadPREExt");
                NuevaCantidadPREExt.UnboundType = UnboundColumnType.Decimal;
                NuevaCantidadPREExt.VisibleIndex = 14;
                NuevaCantidadPREExt.Width = 37;
                NuevaCantidadPREExt.Visible = this.documentID != AppDocuments.PresupuestoPxQ ? true : false;
                NuevaCantidadPREExt.OptionsColumn.AllowEdit = true;
                NuevaCantidadPREExt.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                NuevaCantidadPREExt.AppearanceHeader.Options.UseTextOptions = true;
                NuevaCantidadPREExt.AppearanceHeader.Options.UseFont = true;
                NuevaCantidadPREExt.AppearanceHeader.Options.UseForeColor = true;
                NuevaCantidadPREExt.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevaCantidadPREExt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gvDetalle.Columns.Add(NuevaCantidadPREExt);

                //NuevoPresupuestoLocDet
                GridColumn NuevoPresupuestoLocDet = new GridColumn();
                NuevoPresupuestoLocDet.FieldName = this.unboundPrefix + "NuevoPresupuestoLoc";
                NuevoPresupuestoLocDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_NuevoPresupuestoLocDet");
                NuevoPresupuestoLocDet.UnboundType = UnboundColumnType.Decimal;
                NuevoPresupuestoLocDet.VisibleIndex = 15;
                NuevoPresupuestoLocDet.Width = 40;
                NuevoPresupuestoLocDet.Visible = this.documentID != AppDocuments.PresupuestoPxQ ? true : false; 
                NuevoPresupuestoLocDet.OptionsColumn.AllowEdit = false;
                NuevoPresupuestoLocDet.ColumnEdit = this.editValor;
                NuevoPresupuestoLocDet.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                NuevoPresupuestoLocDet.AppearanceHeader.Options.UseTextOptions = true;
                NuevoPresupuestoLocDet.AppearanceHeader.Options.UseFont = true;
                NuevoPresupuestoLocDet.AppearanceHeader.Options.UseForeColor = true;
                NuevoPresupuestoLocDet.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevoPresupuestoLocDet.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gvDetalle.Columns.Add(NuevoPresupuestoLocDet);

                //NuevoPresupuestoExtDet
                GridColumn NuevoPresupuestoExtDet = new GridColumn();
                NuevoPresupuestoExtDet.FieldName = this.unboundPrefix + "NuevoPresupuestoExt";
                NuevoPresupuestoExtDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_NuevoPresupuestoExtDet");
                NuevoPresupuestoExtDet.UnboundType = UnboundColumnType.Decimal;
                NuevoPresupuestoExtDet.VisibleIndex = 16;
                NuevoPresupuestoExtDet.Width = 40;
                NuevoPresupuestoExtDet.Visible = this.documentID != AppDocuments.PresupuestoPxQ ? true : false;
                NuevoPresupuestoExtDet.OptionsColumn.AllowEdit = false;
                NuevoPresupuestoExtDet.ColumnEdit = this.editValor;
                NuevoPresupuestoExtDet.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                NuevoPresupuestoExtDet.AppearanceHeader.Options.UseTextOptions = true;
                NuevoPresupuestoExtDet.AppearanceHeader.Options.UseFont = true;
                NuevoPresupuestoExtDet.AppearanceHeader.Options.UseForeColor = true;
                NuevoPresupuestoExtDet.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                NuevoPresupuestoExtDet.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.gvDetalle.Columns.Add(NuevoPresupuestoExtDet);

                //Observacion Deta
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "DescripTExt";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_DescripTExt");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 17;
                DescripTExt.Width = 50;
                DescripTExt.Visible = true;
                DescripTExt.ColumnEdit = this.richText1;
                this.gvDetalle.Columns.Add(DescripTExt);

                #region Columnas no visibles
                //NumeroDocOC
                GridColumn NumeroDocOC = new GridColumn();
                NumeroDocOC.FieldName = this.unboundPrefix + "NumeroDocOC";
                NumeroDocOC.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_NumeroDocOC");
                NumeroDocOC.UnboundType = UnboundColumnType.String;
                NumeroDocOC.VisibleIndex = 0;
                NumeroDocOC.Width = 40;
                NumeroDocOC.Visible = false;
                NumeroDocOC.OptionsColumn.AllowEdit = false;
                NumeroDocOC.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(NumeroDocOC);

                //ActividadID
                GridColumn ActividadID = new GridColumn();
                ActividadID.FieldName = this.unboundPrefix + "ActividadID";
                ActividadID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_ActividadID");
                ActividadID.UnboundType = UnboundColumnType.String;
                ActividadID.VisibleIndex = 1;
                ActividadID.Width = 40;
                ActividadID.Visible = false;
                ActividadID.OptionsColumn.AllowEdit = false;
                ActividadID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(ActividadID);

                //CentroCostoID
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 2;
                centroCostoID.Width = 40;
                centroCostoID.Visible = false;
                centroCostoID.OptionsColumn.AllowEdit = false;
                centroCostoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetalle.Columns.Add(centroCostoID);

                #endregion

                #endregion

                this.gvDetalle.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentPresupuestoPxQ.cs-AddGriCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected virtual void AddNewRow_Det()
        {
            try
            {
                DTO_plPresupuestoPxQDeta det = new DTO_plPresupuestoPxQDeta(true);

                #region Asigna datos a la fila
                det.Consecutivo.Value = this.detListFinal.Count == 0 ? 1 : this.detListFinal.Last().Consecutivo.Value.Value + 1;
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                {
                    det.ProyectoID.Value = this.masterProyecto.Value;
                    det.LocFisicaID.Value = this.locFisicaID;
                    det.AreaFisicaID.Value = this.areaFisicaID;
                    det.ActividadID.Value = this.actividadID;
                }
                det.CentroCostoID.Value = this.centroCostoIDxDef;
                det.Ano.Value = this.dtPeriod.DateTime.Year;
                #endregion

                this.detListFinal.Add(det);
                this.LoadGrid();
                this.isValid_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentPresupuestoPxQ.cs-AddNewRow_Det"));
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected virtual bool ValidateRow_Det()
        {
            try
            {
                if (this.detListFinal.Count > 0)
                {
                    bool validField = true;
                    this.isValid_Det = true;

                    int fila = this.gvDocument.FocusedRowHandle;

                    #region Validacion de nulls
                    #region Linea Presupuesto
                    //validField = _bc.ValidGridCell(this.gvDocument, string.Empty, fila, "LineaPresupuestoID", false, false, false, AppMasters.plLineaPresupuesto);
                    //if (!validField)
                    //    this.isValid_Det = false;
                    #endregion
                    #endregion

                }
            }
            catch (Exception ex)
            {
                this.isValid_Det = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "ValidateRow_Doc"));
            }

            return this.isValid_Det;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// </summary>
        protected virtual void RowIndexChanged_Det(int fila)
        {
            try
            {
                if (!this.disableValidate_Det & fila >= 0)
                    this.isValid_Det = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "RowIndexChanged_Det"));
            }
        }

        /// <summary>
        /// Limpia / Habilita el formulario
        /// </summary>
        protected virtual void EnableControls(bool enable)
        {
            this.dtPeriod.Enabled = enable;
            this.masterProyecto.EnableControl(enable);

            if (enable)
            {
                //this.disableValidate_Det = true;
                //this.gcDocument.DataSource = this.detListFinal;
                this.LoadGrid();
                //this.disableValidate_Det = false;

                this.proyectoID = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.masterContrato.Value = string.Empty;
                this.masterActividad.Value = string.Empty;
                this.masterCampo.Value = string.Empty;
                this.masterLineaFilter.Value = string.Empty;
                this.masterRecursoFilter.Value = string.Empty;
                this.txtTasaCambio.Text = "0";
                this.cmbTipoProyecto.EditValue = "1";

                this.txtTotalCantidad.Text = "0";
                this.txtTotalLoc.Text = "0";
                this.txtTotalExt.Text = "0";

                this.dtPeriod.Focus();
            }
            else
                FormProvider.Master.itemNew.Enabled = true;
        }

        /// <summary>
        /// Pregunta si desea reemplazar el documento actual por una nueva fuente de datos
        /// </summary>
        /// <returns></returns>
        protected virtual bool ReplaceDocument()
        {
            string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

            if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;

            return false;
        }

        /// <summary>
        /// Carga la info del formulario
        /// </summary>
        protected virtual void LoadData()
        {
            try
            {
                #region Capex / Inversion
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                {
                    if (this.masterProyecto.ValidID)
                    {
                        #region Valida el Proyecto/Campo/Contrato
                        DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                        if (string.IsNullOrEmpty(proy.LocFisicaID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterProyecto.CodeRsx, "Loc. Física"));
                            this.validHeader = false;
                            return;
                        }
                        DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                        if (locFisica != null && string.IsNullOrEmpty(locFisica.AreaFisica.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, "Loc. Física del Proyecto", this.masterCampo.CodeRsx));
                            this.validHeader = false;
                            return;
                        }
                        DTO_glAreaFisica campo = (DTO_glAreaFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false, locFisica.AreaFisica.Value, true);
                        if (campo != null && string.IsNullOrEmpty(campo.ContratoID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterCampo.CodeRsx, "Contrato"));
                            this.validHeader = false;
                            return;
                        }
                        if (string.IsNullOrEmpty(proy.ActividadID.Value))
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                            MessageBox.Show(string.Format(msg, this.masterProyecto, this.masterActividad.CodeRsx));
                            this.validHeader = false;
                            return;
                        }

                        this.locFisicaID = proy.LocFisicaID.Value;
                        this.areaFisicaID = locFisica.AreaFisica.Value;
                        this.masterCampo.Value = this.areaFisicaID;
                        this.masterContrato.Value = campo.ContratoID.Value;
                        this.actividadID = proy.ActividadID.Value;
                        this.masterActividad.Value = this.actividadID;
                        #endregion
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

                        //Carga Presupuesto PxQ
                        this.presupuesto = this._bc.AdministrationModel.PresupuestoPxQ_GetPresupuestoPxQConsolidado(AppDocuments.Presupuesto, this.masterProyecto.Value, this.periodo,
                                                Convert.ToByte(this.cmbTipoProyecto.EditValue), this.masterContrato.Value, this.masterActividad.Value, this.masterCampo.Value, (byte)EstadoDocControl.SinAprobar);

                        if (this.presupuesto == null)
                        {
                            this.proyectoID = this.masterProyecto.Value;
                            #region Carga info inicial si No existe Presupuesto PxQ
                            this.presupuesto = this._bc.AdministrationModel.PresupuestoPxQ_GetDataPxQ(this.documentID, Convert.ToByte(this.cmbTipoProyecto.EditValue),
                                                                                                      this.masterProyecto.Value, this.dtPeriod.DateTime, this.masterContrato.Value, this.masterActividad.Value,
                                                                                                      this.masterCampo.Value, this.masterLineaFilter.Value, this.masterRecursoFilter.Value);
                            this.detListFinal = this.presupuesto != null ? this.presupuesto.DetallesPxQ : new List<DTO_plPresupuestoPxQDeta>();
                            #endregion
                        }
                        else
                        {
                            #region Carga presupuesto Existente
                            #region Carga el cabezote y tb
                            this.proyectoID = this.masterProyecto.Value;
                            this.dtPeriod.DateTime = this.periodo;
                            this.txtTasaCambio.EditValue = this.presupuesto.DocCtrl.TasaCambioDOCU.Value.Value;
                            this.numeroDocPresup = this.presupuesto.DocCtrl.NumeroDoc.Value.Value;
                            if (this.presupuesto.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                            {
                                FormProvider.Master.itemSave.Enabled = false;
                                FormProvider.Master.itemSendtoAppr.Enabled = false;
                            }
                            else
                                FormProvider.Master.itemSendtoAppr.Enabled = true;
                            #endregion
                            #region Carga Detalle
                            List<DTO_plPresupuestoPxQDeta> listDistinct = new List<DTO_plPresupuestoPxQDeta>();
                            List<string> distinctAreaFisica = (from c in this.presupuesto.DetallesPxQ select c.AreaFisicaID.Value).Distinct().ToList();
                            #region Distinct x AreaFisica
                            foreach (string area in distinctAreaFisica)
                            {
                                DTO_plPresupuestoPxQDeta deta = new DTO_plPresupuestoPxQDeta(true);
                                deta.AreaFisicaID.Value = area;
                                deta.AreaFisicaDesc.Value = this.presupuesto.DetallesPxQ.Find(x => x.AreaFisicaID.Value == area).AreaFisicaDesc.Value;
                                deta.PresupuestoLoc.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniLoc.Value * x.CantidadPRELoc.Value);
                                deta.PresupuestoExt.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniExt.Value * x.CantidadPREExt.Value);
                                deta.Detalle.AddRange(this.presupuesto.DetallesPxQ.Where(x => x.AreaFisicaID.Value == area));
                                deta.Detalle = deta.Detalle.OrderBy(x => x.RecursoID.Value).ToList();
                                listDistinct.Add(deta);
                            }
                            this.detListFinal = listDistinct.OrderBy(x => x.AreaFisicaID.Value).ToList();
                            #endregion
                            #endregion

                            this.dtPeriod.Enabled = false;
                            this.masterProyecto.EnableControl(false);
                            #endregion
                        }

                        this.validHeader = true;
                        this.isValid_Det = true;
                        this.LoadGrid();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterProyecto.LabelRsx, this.masterProyecto.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                    }
                }
                #endregion
                #region Opex/Administrativo
                else
                {
                    if (this.masterContrato.ValidID)
                    {
                        #region Valida el Campo/Actividad
                        if (!string.IsNullOrEmpty(this.masterCampo.Value) && !this.masterCampo.ValidID)
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                            MessageBox.Show(string.Format(msg, this.masterCampo.LabelRsx, this.masterCampo.Value));
                            this.validHeader = false;
                            return;
                        }
                        if (!string.IsNullOrEmpty(this.masterActividad.Value) && !this.masterActividad.ValidID)
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                            MessageBox.Show(string.Format(msg, this.masterActividad.LabelRsx, this.masterActividad.Value));
                            this.validHeader = false;
                            return;
                        }
                        this.areaFisicaID = this.masterCampo.Value;
                        this.actividadID = this.masterActividad.Value;
                        #endregion
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

                        //Carga Presupuesto PxQ
                        this.presupuesto = this._bc.AdministrationModel.PresupuestoPxQ_GetPresupuestoPxQConsolidado(AppDocuments.Presupuesto, this.masterProyecto.Value, this.periodo,
                                               Convert.ToByte(this.cmbTipoProyecto.EditValue), this.masterContrato.Value, this.masterActividad.Value, this.masterCampo.Value, (byte)EstadoDocControl.SinAprobar);

                        if (this.presupuesto == null)
                        {
                            this.presupuesto = this._bc.AdministrationModel.PresupuestoPxQ_GetDataPxQ(this.documentID, Convert.ToByte(this.cmbTipoProyecto.EditValue),
                                                                                                      this.masterProyecto.Value, this.dtPeriod.DateTime, this.masterContrato.Value, this.masterActividad.Value,
                                                                                                      this.masterCampo.Value, this.masterLineaFilter.Value, this.masterRecursoFilter.Value);
                            this.detListFinal = this.presupuesto != null ? this.presupuesto.DetallesPxQ : new List<DTO_plPresupuestoPxQDeta>();
                        }
                        else
                        {
                            #region Asigna el Presupuesto Existente
                            #region Carga el cabezote y tb
                            this.dtPeriod.DateTime = this.periodo;
                            this.txtTasaCambio.EditValue = this.presupuesto.DocCtrl.TasaCambioDOCU.Value.Value;
                            this.numeroDocPresup = this.presupuesto.DocCtrl.NumeroDoc.Value.Value;
                            if (this.presupuesto.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                            {
                                FormProvider.Master.itemSave.Enabled = false;
                                FormProvider.Master.itemSendtoAppr.Enabled = false;
                            }
                            else
                                FormProvider.Master.itemSendtoAppr.Enabled = true;
                            #endregion
                            #region Carga Detalle
                            List<DTO_plPresupuestoPxQDeta> listDistinct = new List<DTO_plPresupuestoPxQDeta>();
                            List<string> distinctAreaFisica = (from c in this.presupuesto.DetallesPxQ select c.AreaFisicaID.Value).Distinct().ToList();
                            #region Distinct x AreaFisica
                            foreach (string area in distinctAreaFisica)
                            {
                                DTO_plPresupuestoPxQDeta deta = new DTO_plPresupuestoPxQDeta(true);
                                deta.AreaFisicaID.Value = area;
                                deta.AreaFisicaDesc.Value = this.presupuesto.DetallesPxQ.Find(x => x.AreaFisicaID.Value == area).AreaFisicaDesc.Value;
                                deta.PresupuestoLoc.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniLoc.Value * x.CantidadPRELoc.Value);
                                deta.PresupuestoExt.Value = this.presupuesto.DetallesPxQ.FindAll(x => x.AreaFisicaID.Value == area).Sum(x => x.ValorUniExt.Value * x.CantidadPREExt.Value);
                                deta.Detalle.AddRange(this.presupuesto.DetallesPxQ.Where(x => x.AreaFisicaID.Value == area));
                                deta.Detalle = deta.Detalle.OrderBy(x => x.RecursoID.Value).ToList();
                                listDistinct.Add(deta);
                            }
                            #endregion
                            this.detListFinal = listDistinct.OrderBy(x => x.AreaFisicaID.Value).ToList();
                            this.proyectoID = detListFinal.Count > 0 ? detListFinal.First().ProyectoID.Value : string.Empty;
                            #endregion
                            #endregion
                        }
                        this.proyectoID = detListFinal.Count > 0 ? detListFinal.First().Detalle.First().ProyectoID.Value : string.Empty;
                        this.validHeader = true;
                        this.isValid_Det = true;
                        this.LoadGrid();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterContrato.LabelRsx, this.masterContrato.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeader = false;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la info de la grilla del detalle
        /// </summary>
        protected virtual void LoadGrid()
        {
            try
            {
                this.disableValidate_Det = true;
                this.gcDocument.DataSource = this.detListFinal;

                this.gvDocument.RefreshData();
                this.gvDocument.FocusedRowHandle = this.gvDocument.DataRowCount - 1;
                this.disableValidate_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Trae la tasa de cambio de acuerdo al tipo  de proyecto
        /// </summary>
        protected void LoadTasaCambio()
        {
            Dictionary<string, string> pks = new Dictionary<string, string>();
            pks.Add("PeriodoID", this.dtPeriod.DateTime.ToShortDateString());
            pks.Add("ContratoID", this.masterContrato.Value);
            pks.Add("Campo", this.masterCampo.Value);
            DTO_plTasasPresupuesto tasas = (DTO_plTasasPresupuesto)_bc.GetMasterComplexDTO(AppMasters.plTasasPresupuesto, pks, true);

            if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                this.txtTasaCambio.EditValue = tasas != null ? tasas.TRMCapex.Value : 0;
            else
                this.txtTasaCambio.EditValue = tasas != null ? tasas.TRMOpex.Value : 0;
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        protected virtual void ShowFKModal(int row, string col, ButtonEdit be)
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
                ModalMaster modal = null;
                if (fktable.Jerarquica.Value.Value)
                {
                    if (col == "CentroCostoID")
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, this.filtrosCentroCosto);
                    else
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    if (col == "LineaPresupuestoID")
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, this.filtrosLineaPres);
                    else
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "ShowFKModal"));
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Valida un DTO de footer en la importacion
        /// </summary>
        /// <param name="presupDet">detalle a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        protected virtual void ValidateDataImport(DTO_plPresupuestoPxQDeta presupDet, DTO_TxResultDetail rd)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;

            #region Valida FKs
            #region CentroCostoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            rdF = this._bc.ValidGridCell(colRsx, presupDet.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Linea Presupuesto
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            rdF = this._bc.ValidGridCell(colRsx, presupDet.LineaPresupuestoID.Value, false, false, false, AppMasters.plLineaPresupuesto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Validacion de PK compuesta
            if (this.detListFinal.Count > 0)
            {
                int count = this.detListFinal.Where(x => x.CentroCostoID.Value == presupDet.CentroCostoID.Value &&
                                                    x.LineaPresupuestoID.Value == presupDet.LineaPresupuestoID.Value &&
                                                    x.ProyectoID.Value == presupDet.ProyectoID.Value).Count();
                if (count > 1)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = colRsx;
                    rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidPresDet);
                    createDTO = false;
                    rd.DetailsFields.Add(rdF);
                }
            }
            #endregion
            #region Valida Valores
            #region VlrMvtoLocal - VlrMvtoExtr
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrMvtoLocal");
            string colRsxExtr = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrMvtoExtr");
            if (presupDet.PresupuestoLoc.Value == 0 && presupDet.PresupuestoExt.Value == 0)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx + "-" + colRsxExtr;
                rdF.Message = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), rdF.Field);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion

            #endregion
            if (createDTO)
            {
                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, presupDet.LineaPresupuestoID.Value, true);
                //presupDet.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, presupDet.CentroCostoID.Value, true);
                //presupDet.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
            }
            #endregion

        }

        /// <summary>
        /// Obtiene los filtros para las maestras del encabezado y la grilla
        /// </summary>
        protected void GetFiltersMasters()
        {
            bool GrupoPresupuestalUsuarioInd = this._bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ValidaGrupoPresu).Equals("1") ? true : false;
            if (GrupoPresupuestalUsuarioInd)
            {
                #region Variables
                List<string> listLineaPresupuesto = new List<string>();
                List<string> listCentroCosto = new List<string>();
                List<string> listGruposPresupuesto = new List<string>();
                List<string> listAreasPresupuesto = new List<string>();
                List<string> listActividades = new List<string>();
                List<string> listAreaFisicas = new List<string>();
                List<string> listLocFisicas = new List<string>();
                int userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
                #endregion

                //Trae Grupos Presupuestales x User existentes 
                long countGrupoPresUser = _bc.AdministrationModel.MasterComplex_Count(AppMasters.plGrupoPresupuestalUsuario, null, null);
                var grupPresupUser = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.plGrupoPresupuestalUsuario, countGrupoPresUser, 1, null, null).ToList();

                if (grupPresupUser != null && grupPresupUser.Count > 0)
                {
                    foreach (var grupoUser in grupPresupUser)
                    {
                        #region Llena la lista de GruposPresupuesto
                        DTO_plGrupoPresupuestalUsuario dtoGrupoPres = (DTO_plGrupoPresupuestalUsuario)grupoUser;
                        if (dtoGrupoPres.seUsuarioID.Value == userID.ToString())
                        {
                            if (!listGruposPresupuesto.Contains(dtoGrupoPres.GrupoPresupuestoID.Value))
                                listGruposPresupuesto.Add(dtoGrupoPres.GrupoPresupuestoID.Value);
                            if (!listAreasPresupuesto.Contains(dtoGrupoPres.AreaPresupuestalID.Value))
                                listAreasPresupuesto.Add(dtoGrupoPres.AreaPresupuestalID.Value);
                        }
                        #endregion
                    }

                    #region Obtiene Linea Presup y Centro Costo Permitidos(plGrupoPresupuestalLinea)
                    #region Crea Filtros
                    List<DTO_glConsultaFiltro> filtrosGrupoPresupLinea = new List<DTO_glConsultaFiltro>();
                    foreach (string grupoPresup in listGruposPresupuesto)
                    {
                        filtrosGrupoPresupLinea.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "GrupoPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = grupoPresup,
                            OperadorSentencia = "OR"
                        });
                    }
                    DTO_glConsulta consultaGrupoPresLinea = new DTO_glConsulta();
                    consultaGrupoPresLinea.Filtros = filtrosGrupoPresupLinea;
                    #endregion
                    //Trae los Grupos Presupuestales x Linea fitrados  
                    long countGrupoPresLinea = _bc.AdministrationModel.MasterComplex_Count(AppMasters.plGrupoPresupuestalLinea, consultaGrupoPresLinea, null);
                    var grupPresupLinea = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.plGrupoPresupuestalLinea, countGrupoPresUser, 1, consultaGrupoPresLinea, null).ToList();

                    if (grupPresupLinea != null && grupPresupLinea.Count > 0)
                    {
                        foreach (var grupoLinea in grupPresupLinea)
                        {
                            #region Llena la lista de LineasPresupuesto y CentroCosto
                            DTO_plGrupoPresupuestalLinea dtoTipo = (DTO_plGrupoPresupuestalLinea)grupoLinea;
                            if (!listLineaPresupuesto.Contains(dtoTipo.LineaPresupuestoID.Value))
                                listLineaPresupuesto.Add(dtoTipo.LineaPresupuestoID.Value);
                            if (!listCentroCosto.Contains(dtoTipo.CentroCostoID.Value))
                                listCentroCosto.Add(dtoTipo.CentroCostoID.Value);
                            #endregion
                        }
                    }
                    #endregion
                    #region Obtiene Actividad  Permitidas (plGrupoPresupuestalActividad)
                    #region Crea Filtros  para plGrupoPresupuestalActividad
                    List<DTO_glConsultaFiltro> filtrosGrupoPresupActividad = new List<DTO_glConsultaFiltro>();
                    foreach (string grupoPresup in listGruposPresupuesto)
                    {
                        filtrosGrupoPresupActividad.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "GrupoPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = grupoPresup,
                            OperadorSentencia = "OR"
                        });
                    }
                    DTO_glConsulta consultaGrupoPresActiv = new DTO_glConsulta();
                    consultaGrupoPresActiv.Filtros = filtrosGrupoPresupActividad;
                    #endregion
                    //Trae los Grupos Presupuestales x Activ fitrados  
                    long countGrupoPresActiv = _bc.AdministrationModel.MasterComplex_Count(AppMasters.plGrupoPresupuestalActividad, consultaGrupoPresActiv, null);
                    var grupPresupActiv = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.plGrupoPresupuestalActividad, countGrupoPresUser, 1, consultaGrupoPresActiv, null).ToList();

                    foreach (var grupoActiv in grupPresupActiv)
                    {
                        #region Llena la lista de Actividades
                        DTO_plGrupoPresupuestalActividad dtoGrupAct = (DTO_plGrupoPresupuestalActividad)grupoActiv;
                        if (!listActividades.Contains(dtoGrupAct.ActividadID.Value))
                            listActividades.Add(dtoGrupAct.ActividadID.Value);
                        #endregion
                    }

                    #endregion
                    #region Obtiene AreasFisicas  Permitidas (glAreaFisica)
                    #region Crea Filtros  para glAreaFisica
                    filtrosAreaFisica = new List<DTO_glConsultaFiltro>();
                    this.filtrosAreaFisica.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "TipoArea",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = "1",
                        OperadorSentencia = "AND"
                    });
                    foreach (string areaPres in listAreasPresupuesto)
                    {
                        this.filtrosAreaFisica.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "AreaPresupuestalID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = areaPres.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }
                    DTO_glConsulta consultaAreaFisica = new DTO_glConsulta();
                    consultaAreaFisica.Filtros = filtrosAreaFisica;
                    #endregion
                    //Trae las areasFisicas fitradas  
                    long countAreaFisica = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glAreaFisica, consultaAreaFisica, null, true);
                    var areaFisicas = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.glAreaFisica, countAreaFisica, 1, consultaAreaFisica, null, true).ToList();
                    #endregion
                    #region Obtiene LocFisicas Permitidas (glLocFisica)
                    #region Crea Filtros  para glLocFisica
                    List<DTO_glConsultaFiltro> filtrosLocFisica = new List<DTO_glConsultaFiltro>();
                    foreach (var areaFis in areaFisicas)
                    {
                        filtrosLocFisica.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "AreaFisica",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = areaFis.ID.Value,
                            OperadorSentencia = "OR"
                        });
                    }
                    DTO_glConsulta consultaLocFisica = new DTO_glConsulta();
                    consultaLocFisica.Filtros = filtrosLocFisica;
                    #endregion
                    //Trae loc Fisicas fitradas  
                    long countLocFisica = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glLocFisica, consultaLocFisica, null, true);
                    var locFisicas = _bc.AdministrationModel.MasterHierarchy_GetPaged(AppMasters.glLocFisica, countLocFisica, 1, consultaLocFisica, null, true).ToList();

                    foreach (var locFis in locFisicas)
                    {
                        #region Llena la lista de locFisicas
                        if (!listLocFisicas.Contains(locFis.ID.Value))
                            listLocFisicas.Add(locFis.ID.Value);
                        #endregion
                    }
                    #endregion

                    #region Valida si el Usuario tiene seguridades
                    //Dictionary<string, string> pks = new Dictionary<string, string>();
                    //pks.Add("Usuario", this.dtPeriod.DateTime.ToShortDateString());
                    //pks.Add("Campo", this.masterCampo.Value);
                    //DTO_seUsuarioGrupo userPermiso = (DTO_seUsuarioGrupo)_bc.GetMasterComplexDTO(AppMasters.seUsuarioGrupo, pks, true); 
                    #endregion

                    #region DEFINE LOS FILTROS PARA GRILLA
                    //Crea filtros para LineaPresupuestoID
                    this.filtrosLineaPres = new List<DTO_glConsultaFiltro>();
                    foreach (string lineaPres in listLineaPresupuesto)
                    {
                        this.filtrosLineaPres.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "LineaPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = lineaPres.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }

                    //Crea filtros para CentroCosto
                    this.filtrosCentroCosto = new List<DTO_glConsultaFiltro>();
                    foreach (string centroCto in listCentroCosto)
                    {
                        this.filtrosCentroCosto.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "CentroCostoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = centroCto.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }
                    #endregion
                    #region DEFINE LOS FILTROS PARA MASTER FIND
                    //Crea filtros para maestra Actividad
                    this.filtrosActividad = new List<DTO_glConsultaFiltro>();
                    foreach (string activ in listActividades)
                    {
                        this.filtrosActividad.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "ActividadID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = activ.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }

                    //Crea filtros maestra AreaFisica(Campo)
                    this.filtrosAreaFisica = new List<DTO_glConsultaFiltro>();
                    this.filtrosAreaFisica.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "TipoArea",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = "1",
                        OperadorSentencia = "AND"
                    });
                    foreach (string areaPres in listAreasPresupuesto)
                    {
                        this.filtrosAreaFisica.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "AreaPresupuestalID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = areaPres.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }
                    //Crea filtros para maestra Proyectos
                    this.filtrosProyecto = new List<DTO_glConsultaFiltro>();
                    foreach (string areaPres in listActividades)
                    {
                        this.filtrosProyecto.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "ActividadID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = areaPres.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }
                    if (this.filtrosProyecto.Count > 0)
                        this.filtrosProyecto.Last().OperadorSentencia = "AND";
                    foreach (string loFisica in listLocFisicas)
                    {
                        this.filtrosProyecto.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "LocFisicaID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = loFisica.TrimEnd(),
                            OperadorSentencia = "OR"
                        });
                    }
                    #endregion
                }
            }
        }

        #endregion

        #region Eventos Controles header

        /// <summary>
        /// Evento al salir del control del proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProyecto.ValidID && this.masterProyecto.Value != this.proyectoID)
                {
                    #region Valida el Proyecto/Campo/Contrato
                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                    if (proy != null && string.IsNullOrEmpty(proy.LocFisicaID.Value))
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                        MessageBox.Show(string.Format(msg, this.masterProyecto.CodeRsx, "Loc. Física"));
                        this.validHeader = false;
                        return;
                    }
                    DTO_glLocFisica locFisica = (DTO_glLocFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);
                    if (locFisica != null && string.IsNullOrEmpty(locFisica.AreaFisica.Value))
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                        MessageBox.Show(string.Format(msg, "Loc. Física del Proyecto", this.masterCampo.CodeRsx));
                        this.validHeader = false;
                        return;
                    }
                    DTO_glAreaFisica campo = (DTO_glAreaFisica)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFisica, false, locFisica.AreaFisica.Value, true);
                    if (campo != null && string.IsNullOrEmpty(campo.ContratoID.Value))
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                        MessageBox.Show(string.Format(msg, this.masterCampo.CodeRsx, "Contrato"));
                        this.validHeader = false;
                        return;
                    }
                    if (string.IsNullOrEmpty(proy.ActividadID.Value))
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeIncompleteFK);
                        MessageBox.Show(string.Format(msg, this.masterProyecto, this.masterActividad.CodeRsx));
                        this.validHeader = false;
                        return;
                    }

                    DTO_coActividad activ = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, proy.ActividadID.Value, true);
                    if (Convert.ToInt32(this.cmbTipoProyecto.EditValue) != activ.ProyectoTipo.Value)
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.pl_InvalidProyecto));                    
                        this.validHeader = false;
                        return;
                    }

                    this.locFisicaID = proy.LocFisicaID.Value;
                    this.areaFisicaID = locFisica.AreaFisica.Value;
                    this.masterCampo.Value = this.areaFisicaID;
                    this.masterContrato.Value = campo.ContratoID.Value;
                    this.actividadID = proy.ActividadID.Value;
                    this.masterActividad.Value = this.actividadID;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ", "masterProyecto_Leave"));
            }
        }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtPeriod_EditValueChanged()
        {
            try
            {
                this.periodo = this.dtPeriod.DateTime;
                //if (this.masterProyecto.Value != this.proyectoID)
                //    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            try
            {
                string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "DescripTExt")
                    this.richEditControl.Document.Text = this.gvDocument.GetFocusedRowCellValue(fieldName).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "riPopup_QueryPopUp"));
            }
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void editLink_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Evento para ingresar un nuevo presupuesto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ", "btnNew_Click"));
            }
        }

        /// <summary>
        /// Evento para consultar presupuestos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.LoadData();
            this.LoadTasaCambio();
        }

        /// <summary>
        /// Al cambiar la opcion del control
        /// </summary>
        /// <param name="sender">Objeto que envia el vento</param>
        /// <param name="e">Evento</param>
        private void cmbTipoProyecto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Opex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Administracion)
                {
                    this.masterProyecto.EnableControl(true);
                    this.masterContrato.EnableControl(true);
                    this.masterActividad.EnableControl(true);
                    this.masterCampo.EnableControl(true);
                    this.masterProyecto.Value = string.Empty;
                    this.masterContrato.Value = string.Empty;
                    this.masterActividad.Value = string.Empty;
                    this.masterCampo.Value = string.Empty;
                    #region Filtra los proyectos
                    filtrosProyecto.RemoveAll(x => x.CampoFisico.Equals("ProyectoTipo"));
                    filtrosProyecto.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ProyectoTipo",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = ((byte)ProyectoTipo.Opex).ToString(),
                        OperadorSentencia = "OR"
                    });
                    filtrosProyecto.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ProyectoTipo",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = ((byte)ProyectoTipo.Administracion).ToString()
                    });
                    this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false, filtrosProyecto);
                    this._bc.InitMasterUC(this.masterContrato, AppMasters.pyContrato, true, true, true, true);
                    #endregion
                }
                else
                {
                    this.masterProyecto.EnableControl(true);
                    this.masterContrato.EnableControl(false);
                    this.masterActividad.EnableControl(false);
                    this.masterCampo.EnableControl(false);
                    this.masterProyecto.Value = string.Empty;
                    this.masterContrato.Value = string.Empty;
                    this.masterActividad.Value = string.Empty;
                    this.masterCampo.Value = string.Empty;
                    #region Filtra los proyectos
                    filtrosProyecto.RemoveAll(x => x.CampoFisico.Equals("ProyectoTipo"));
                    filtrosProyecto.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ProyectoTipo",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = ((byte)ProyectoTipo.Capex).ToString(),
                        OperadorSentencia = "OR"
                    });
                    filtrosProyecto.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ProyectoTipo",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = ((byte)ProyectoTipo.Inversion).ToString()
                    });
                    this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true, filtrosProyecto);
                    this._bc.InitMasterUC(this.masterContrato, AppMasters.pyContrato, true, true, true, false);
                    #endregion
                }
                this.gvDocument.RefreshData();
                this.LoadTasaCambio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "cmbProyectoTipo_EditValueChanged"));
            }
        }
        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gv_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    UDT udtProp = (UDT)pi.GetValue(dto, null);
                    udtProp.SetValueFromString(e.Value.ToString());
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void editBtn_Doc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "editBtn_Doc_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gcDetail_Enter(object sender, EventArgs e)
        {
            try
            {
                //if (this.validHeader)
                //    this.EnableControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "gcDetail_Enter"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gcDetail_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.validHeader)
                {
                    this.gvDocument.PostEditor();

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        #region Nuevo registro
                        if (this.gvDocument.ActiveFilterString != string.Empty)
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                        else
                        {
                            this.deleteOP = false;
                            if (this.isValid_Det)
                                this.AddNewRow_Det();
                            else
                            {
                                bool isV = this.ValidateRow_Det();
                                if (isV)
                                    this.AddNewRow_Det();
                            }
                            int fila = this.gvDocument.FocusedRowHandle;
                        }
                        #endregion
                    }
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        #region Borrar registro
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.deleteOP = true;
                            int rowHandle = this.gvDocument.FocusedRowHandle;

                            if (this.detListFinal.Count == 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                e.Handled = true;
                            }
                            else
                            {
                                this.detListFinal.RemoveAt(rowHandle);
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvDocument.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvDocument.FocusedRowHandle = rowHandle - 1;

                                this.gvDocument.RefreshData();
                                this.RowIndexChanged_Det(this.gvDocument.FocusedRowHandle);
                            }
                        }
                        e.Handled = true;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                bool validField = true;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                int indexRowGrid = this.gvDocument.FocusedRowHandle;
                int indexRowSubGrid = e.RowHandle;

                #region FKs

                if (fieldName == "LineaPresupuestoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, false, false, AppMasters.plLineaPresupuesto);
                }
                if (fieldName == "CentroCostoID")
                {
                    validField = _bc.ValidGridCell(this.gvDocument, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                    DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, e.Value.ToString(), true);
                }
                #endregion
                #region Valores

                //Porcentaje Variacion
                if (fieldName == "PorcentajeVar")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniOCLoc.Value + (this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniOCLoc.Value *
                                                                                                 Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) / 100);
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniExt.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniOCExt.Value + (this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniOCExt.Value *
                                                                                                 Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture) / 100);
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniLoc.Value * this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].CantidadPRELoc.Value;
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniExt.Value * this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].CantidadPREExt.Value;
                    this.detListFinal[indexRowGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoLoc.Value);
                    this.detListFinal[indexRowGrid].PresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoExt.Value);
                }
                //CantidadPRELoc
                if (fieldName == "CantidadPRELoc")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniLoc.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoLoc.Value);
                }
                //CantidadPREExt
                if (fieldName == "CantidadPREExt")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniExt.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoExt.Value);
                }
                //CantidadPRELoc
                if (fieldName == "NuevaCantidadPRELoc")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].NuevoPresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniLoc.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.NuevoPresupuestoLoc.Value);
                }
                //CantidadPREExt
                if (fieldName == "NuevaCantidadPREExt")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].NuevoPresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].ValorUniExt.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoExt.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.NuevoPresupuestoExt.Value);
                }
                //ValorUniLoc
                if (fieldName == "ValorUniLoc")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].CantidadPRELoc.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoLoc.Value);
                }
                //ValorUniExt
                if (fieldName == "ValorUniExt")
                {
                    this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle[indexRowSubGrid].CantidadPREExt.Value * Convert.ToDecimal(e.Value);
                    this.detListFinal[indexRowGrid].PresupuestoLoc.Value = this.detListFinal[indexRowGrid].Detalle.Sum(x => x.PresupuestoLoc.Value);
                }

                #endregion
                if (!validField)
                    this.isValid_Det = false;

                this.gvDetalle.RefreshData();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!this.disableValidate_Det)
            {
                bool validRow = this.deleteOP ? true : this.ValidateRow_Det();
                this.deleteOP = false;

                if (validRow)
                    this.isValid_Det = true;
                else
                {
                    e.Allow = false;
                    this.isValid_Det = false;
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDetail_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                this.RowIndexChanged_Det(e.FocusedRowHandle);
                this.txtLineaPresDesc.Text = this.detListFinal[this.gvDocument.FocusedRowHandle].Detalle[e.FocusedRowHandle].LineaPresDesc.Value;
                this.txtCodigoBSDesc.Text = this.detListFinal[this.gvDocument.FocusedRowHandle].Detalle[e.FocusedRowHandle].CodigoBSDesc.Value;
                this.txtRecursoDesc.Text = this.detListFinal[this.gvDocument.FocusedRowHandle].Detalle[e.FocusedRowHandle].RecursoDesc.Value;
                this.masterPrefijoOC.Value = this.detListFinal[this.gvDocument.FocusedRowHandle].Detalle[e.FocusedRowHandle].PrefijoIDOC.Value;
                this.txtNroOC.Text = this.detListFinal[this.gvDocument.FocusedRowHandle].Detalle[e.FocusedRowHandle].NroOC.Value.ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void editBtn_Det_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "editBtn_Det_ButtonClick"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "DescripTExt")
                    e.RepositoryItem = this.riPopup;
            }
            catch (Exception ex)
            {
                throw ex;
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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this._frmModule);

                FormProvider.Master.itemCopy.Visible = false;

                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;

                if (this.documentID != AppDocuments.PresupuestoPxQ)
                {
                    FormProvider.Master.itemGenerateTemplate.Visible = false;
                    FormProvider.Master.itemImport.Visible = false;
                }

                if (FormProvider.Master.LoadFormTB)
                {
                    //Deshabilitados
                    //FormProvider.Master.itemFilter.Enabled = false;
                    //FormProvider.Master.itemFilterDef.Enabled = false;
                    FormProvider.Master.itemGenerateTemplate.Enabled = false;

                    FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                }
                FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Export);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentPresupuestoPxQ.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentPresupuestoPxQ.cs-Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentPresupuestoPxQ.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para crear un nuevo presupuesto desde cero
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.detListFinal = new List<DTO_plPresupuestoPxQDeta>();
                this.EnableControls(true);
                this.validHeader = false;
                this.deleteOP = false;
                this.isValid_Det = true;
                this.disableValidate_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para guardar
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();
                this.gvDocument.Focus();

                if (this.detListFinal.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    if (isValid)
                    {
                        decimal vlrMvtoLocal = this.detListFinal.Sum(x => x.PresupuestoLoc.Value.Value);
                        decimal vlrMvtoExtr = this.detListFinal.Sum(x => x.PresupuestoExt.Value.Value);
                        //if (vlrMvtoLocal != 0 || vlrMvtoExtr != 0)
                        //{
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                        //}
                        //else
                        //    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaveMvtoInvalid));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this.gvDocument.PostEditor();
                this.gvDocument.Focus();
                if (this.detListFinal.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    if (isValid)
                    {
                        Thread process = new Thread(this.SendToApproveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBSendtoAppr"));
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
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                List<DTO_plPresupuestoPxQDeta> all = new List<DTO_plPresupuestoPxQDeta>();
                this.detListFinal.ForEach(doc => all.AddRange(doc.Detalle));

                #region Carga las tablas

                DataTableOperations tableOp = new DataTableOperations();
                DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_plPresupuestoPxQDeta), all);
                DataTable tableExport = new DataTable();

                #endregion
                #region Carga la info de las columnas

                //Básicas
                tableExport.Columns.Add(new DataColumn("Vigencia"));
                tableExport.Columns.Add(new DataColumn("Contrato"));
                tableExport.Columns.Add(new DataColumn("Actividad"));
                tableExport.Columns.Add(new DataColumn("Recurso"));
                tableExport.Columns.Add(new DataColumn("LineaPresupuesto"));
                tableExport.Columns.Add(new DataColumn("CodigoBS"));
                tableExport.Columns.Add(new DataColumn("Referencia"));
                tableExport.Columns.Add(new DataColumn("Costo Unit $"));
                tableExport.Columns.Add(new DataColumn("Costo Unit U$"));
                tableExport.Columns.Add(new DataColumn("Unidad"));
                tableExport.Columns.Add(new DataColumn("PorcentajeVar"));

                //Campos
                List<string> campos = (from c in this.detListFinal select c.AreaFisicaID.Value).Distinct().ToList();
                foreach (string campo in campos)
                {
                    List<DataColumn> camposCols = new List<DataColumn>();
                    tableExport.Columns.Add(new DataColumn(campo + "-Proyecto"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Costo Unit $"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Costo Unit U$"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Cant $"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Cant U$"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Valor $"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Valor U$"));
                    tableExport.Columns.Add(new DataColumn(campo + "-Observacion"));
                }

                #endregion

                foreach (DataRow filaOrigen in tableAll.Rows)
                {
                    int count =
                    tableExport.AsEnumerable().Where(row =>
                           row["Vigencia"].ToString().Trim() == filaOrigen["Ano"].ToString().Trim()
                        && row["Contrato"].ToString().Trim() == filaOrigen["ContratoID"].ToString().Trim()
                        && row["Actividad"].ToString().Trim() == filaOrigen["ActividadID"].ToString().Trim()
                        && row["Recurso"].ToString().Trim() == filaOrigen["RecursoID"].ToString().Trim()
                        && row["LineaPresupuesto"].ToString().Trim() == filaOrigen["LineaPresupuestoID"].ToString().Trim()
                        && row["CodigoBS"].ToString().Trim() == filaOrigen["CodigoBSID"].ToString().Trim()
                        && row["Referencia"].ToString().Trim() == filaOrigen["inReferenciaID"].ToString().Trim()
                    ).Count();

                    if (count > 0)
                    {
                        #region Fila existente

                        DataRow updateRow =
                        tableExport.AsEnumerable().Where(row =>
                               row["Vigencia"].ToString().Trim() == filaOrigen["Ano"].ToString().Trim()
                            && row["Contrato"].ToString().Trim() == filaOrigen["ContratoID"].ToString().Trim()
                            && row["Actividad"].ToString().Trim() == filaOrigen["ActividadID"].ToString().Trim()
                            && row["Recurso"].ToString().Trim() == filaOrigen["RecursoID"].ToString().Trim()
                            && row["LineaPresupuesto"].ToString().Trim() == filaOrigen["LineaPresupuestoID"].ToString().Trim()
                            && row["CodigoBS"].ToString().Trim() == filaOrigen["CodigoBSID"].ToString().Trim()
                            && row["Referencia"].ToString().Trim() == filaOrigen["inReferenciaID"].ToString().Trim()
                        ).First();

                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Proyecto"] = filaOrigen["ProyectoID"];
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Costo Unit $"] = filaOrigen["ValorUniLoc"];
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Costo Unit U$"] = filaOrigen["ValorUniExt"];
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Cant $"] = filaOrigen["CantidadPRELoc"];
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Cant U$"] = filaOrigen["CantidadPREExt"];
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Observacion"] = filaOrigen["DescripTExt"];

                        decimal valML = string.IsNullOrWhiteSpace(filaOrigen["ValorUniLoc"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["ValorUniLoc"], CultureInfo.InvariantCulture);
                        decimal valME = string.IsNullOrWhiteSpace(filaOrigen["ValorUniExt"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["ValorUniExt"], CultureInfo.InvariantCulture);
                        decimal cantML = string.IsNullOrWhiteSpace(filaOrigen["CantidadPRELoc"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["CantidadPRELoc"], CultureInfo.InvariantCulture);
                        decimal cantME = string.IsNullOrWhiteSpace(filaOrigen["CantidadPREExt"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["CantidadPREExt"], CultureInfo.InvariantCulture);

                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Valor $"] = Convert.ToDecimal(valML * cantML, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        updateRow[filaOrigen["AreaFisicaID"].ToString() + "-Valor U$"] = Convert.ToDecimal(valME * cantME, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        #endregion
                    }
                    else
                    {
                        #region Nuevo fila

                        DataRow fila = tableExport.NewRow();

                        //Info de Pks
                        fila["Vigencia"] = filaOrigen["Ano"];
                        fila["Contrato"] = filaOrigen["ContratoID"];
                        fila["Actividad"] = filaOrigen["ActividadID"];
                        fila["Recurso"] = filaOrigen["RecursoID"];
                        fila["LineaPresupuesto"] = filaOrigen["LineaPresupuestoID"];
                        fila["CodigoBS"] = filaOrigen["CodigoBSID"];
                        fila["Referencia"] = filaOrigen["inReferenciaID"];
                        fila["Costo Unit $"] = filaOrigen["ValorUniOCLoc"];
                        fila["Costo Unit U$"] = filaOrigen["ValorUniOCExt"];
                        fila["Unidad"] = filaOrigen["UnidadInvID"];
                        fila["PorcentajeVar"] = filaOrigen["PorcentajeVar"];

                        //Info de campos
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Proyecto"] = filaOrigen["ProyectoID"];
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Costo Unit $"] = filaOrigen["ValorUniLoc"];
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Costo Unit U$"] = filaOrigen["ValorUniExt"];
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Cant $"] = filaOrigen["CantidadPRELoc"];
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Cant U$"] = filaOrigen["CantidadPREExt"];
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Observacion"] = filaOrigen["DescripTExt"];

                        decimal valML = string.IsNullOrWhiteSpace(filaOrigen["ValorUniLoc"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["ValorUniLoc"], CultureInfo.InvariantCulture);
                        decimal valME = string.IsNullOrWhiteSpace(filaOrigen["ValorUniExt"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["ValorUniExt"], CultureInfo.InvariantCulture);
                        decimal cantML = string.IsNullOrWhiteSpace(filaOrigen["CantidadPRELoc"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["CantidadPRELoc"], CultureInfo.InvariantCulture);
                        decimal cantME = string.IsNullOrWhiteSpace(filaOrigen["CantidadPREExt"].ToString()) ? 0 : Convert.ToDecimal(filaOrigen["CantidadPREExt"], CultureInfo.InvariantCulture);

                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Valor $"] = Convert.ToDecimal(valML * cantML, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                        fila[filaOrigen["AreaFisicaID"].ToString() + "-Valor U$"] = Convert.ToDecimal(valME * cantME, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);

                        tableExport.Rows.Add(fila);

                        #endregion
                    }
                }

                ReportExcelBase frm = new ReportExcelBase(tableExport);
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBExport"));
            }
        }
        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            try
            {
                //Revisa que cumple las condiciones
                if (!this.ReplaceDocument())
                    return;

                this.gvDocument.ActiveFilterString = string.Empty;

                if (this.masterProyecto.ValidID)
                {
                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                    MessageBox.Show(string.Format(msg, this.masterProyecto.LabelRsx, this.masterProyecto.Value));
                    this.masterProyecto.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "TBImport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public virtual void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = null;
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

                if (this.presupuesto == null)
                    this.presupuesto = new DTO_Presupuesto();
                this.presupuesto.NumeroDocPresup.Value = this.numeroDocPresup;
                this.presupuesto.DetallesPxQ = this.detListFinal;
                int documentSave = 0;
                if (this.documentID == AppDocuments.PresupuestoPxQ)
                    documentSave = AppDocuments.Presupuesto;
                else if (this.documentID == AppDocuments.AdicionPresupuestoPxQ)
                {
                    documentSave = AppDocuments.AdicionPresupuesto;
                    this.presupuesto.DocCtrl = null;
                }
                else if (this.documentID == AppDocuments.ReclasifPresupuestoPxQ)
                {
                    documentSave = AppDocuments.ReclasifPresupuesto;
                    this.presupuesto.DocCtrl = null;
                }
                else if (this.documentID == AppDocuments.TrasladoPresupuestoPxQ)
                {
                    documentSave = AppDocuments.TrasladoPresupuesto;
                    this.presupuesto.DocCtrl = null;
                }
                if (string.IsNullOrEmpty(this.proyectoID))
                {
                    result.Result = ResultValue.NOK;
                    return;
                }
                obj = _bc.AdministrationModel.PresupuestoPxQ_Add(documentSave, this.dtPeriod.DateTime, this.proyectoID, tc, this.presupuesto, true);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this.actFlujo.seUsuarioID.Value, obj, true, true);
                if (isOK)
                {
                    this.Invoke(this.saveDelegate);
                    if (obj.GetType() == typeof(DTO_Alarma))
                    {
                        DTO_Alarma alarma = (DTO_Alarma)obj;
                        this.numeroDocPresup = Convert.ToInt32(alarma.NumeroDoc);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public virtual void SendToApproveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = null;
                decimal tc = Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

                if (this.presupuesto == null)
                    this.presupuesto = new DTO_Presupuesto();
                this.presupuesto.NumeroDocPresup.Value = this.numeroDocPresup;
                obj = _bc.AdministrationModel.PresupuestoPxQ_SendToAprob(this.documentID, this.numeroDocPresup);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this.actFlujo.seUsuarioID.Value, obj, true, true);
                if (isOK)
                    this.Invoke(this.sendToApproveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuestoPxQ.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public virtual void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgInvalidFormat = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    DTO_plPresupuestoPxQDeta presupuestoDet = null;
                    this.detListFinal = new List<DTO_plPresupuestoPxQDeta>();
                    bool createDTO = true;
                    bool validList = true;

                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_plPresupuestoPxQDeta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoPxQ + "_" + pi.Name);
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
                            presupuestoDet = new DTO_plPresupuestoPxQDeta(true);
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
                                        PropertyInfo pi = presupuestoDet.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(presupuestoDet, null) : null;
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
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "DocumentPresupuestoPxQ.cs - Creacion de DTO y validacion Formatos");
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
                                this.detListFinal.Add(presupuestoDet);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this.detListFinal.Count; ++index)
                        {
                            #region Variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Message = "OK";
                            #endregion
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this.detListFinal.Count);
                            i++;
                            #endregion
                            presupuestoDet = this.detListFinal[index];
                            #region Valida y Asigna Totales Ambas Monedas
                            //Total del movimiento(Local)
                            //decimal TotalMdaLocal = Math.Round(presupuestoDet.ValorLoc00.Value.Value + presupuestoDet.ValorLoc01.Value.Value + presupuestoDet.ValorLoc02.Value.Value + presupuestoDet.ValorLoc03.Value.Value
                            //                          + presupuestoDet.ValorLoc04.Value.Value + presupuestoDet.ValorLoc05.Value.Value + presupuestoDet.ValorLoc06.Value.Value + presupuestoDet.ValorLoc07.Value.Value + presupuestoDet.ValorLoc08.Value.Value
                            //                          + presupuestoDet.ValorLoc09.Value.Value + presupuestoDet.ValorLoc10.Value.Value + presupuestoDet.ValorLoc11.Value.Value + presupuestoDet.ValorLoc12.Value.Value);
                            //if (TotalMdaLocal != 0)
                            //{
                            //    presupuestoDet.VlrMvtoLocal.Value = TotalMdaLocal;
                            //    presupuestoDet.LoadParticionLocalInd = false;
                            //}

                            //if (this.loadME)
                            //{
                            //    //Total del movimiento(Extr)
                            //    decimal TotalMdaExtr = Math.Round(presupuestoDet.ValorExt00.Value.Value + presupuestoDet.ValorExt01.Value.Value + presupuestoDet.ValorExt02.Value.Value + presupuestoDet.ValorExt03.Value.Value
                            //                            + presupuestoDet.ValorExt04.Value.Value + presupuestoDet.ValorExt05.Value.Value + presupuestoDet.ValorExt06.Value.Value + presupuestoDet.ValorExt07.Value.Value + presupuestoDet.ValorExt08.Value.Value
                            //                            + presupuestoDet.ValorExt09.Value.Value + presupuestoDet.ValorExt10.Value.Value + presupuestoDet.ValorExt11.Value.Value + presupuestoDet.ValorExt12.Value.Value);
                            //    if (TotalMdaExtr != 0)
                            //    {
                            //        presupuestoDet.VlrMvtoExtr.Value = TotalMdaExtr;
                            //        presupuestoDet.LoadParticionExtrInd = false;
                            //    }
                            //}
                            presupuestoDet.Ano.Value = this.dtPeriod.DateTime.Year;
                            presupuestoDet.ProyectoID.Value = !string.IsNullOrEmpty(presupuestoDet.ProyectoID.Value) ? presupuestoDet.ProyectoID.Value : this.proyectoID;
                            presupuestoDet.Consecutivo.Value = index + 1;
                            #endregion
                            #region Valida consistencia datos
                            this.ValidateDataImport(presupuestoDet, rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "Detalle NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(this.refreshDataDelegate);

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        this.detListFinal = new List<DTO_plPresupuestoPxQDeta>();
                    }
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp-DocumentPresupuestoPxQ.cs", "ImportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
