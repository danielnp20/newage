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
    public partial class PresupuestoContable : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            if (documentID != AppDocuments.PresupuestoContable)
            {
                this.presupuesto = new DTO_Presupuesto();
                this.detList = new List<DTO_plPresupuestoDeta>();
                this.EnableControls(true); 
            }
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
                if (documentID != AppDocuments.PresupuestoContable)
                {
                    this.presupuesto = new DTO_Presupuesto();
                    this.detList = new List<DTO_plPresupuestoDeta>();
                    this.EnableControls(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "sendToApproveDelegate"));
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
                //this.dtPeriod.Enabled = false;
                //this.masterProyecto.EnableControl(false);
                this.gcDetail.DataSource = this.detList;
                for (int i = 0; i < this.detList.Count; i++)
                {
                    this.LoadParticiones(i, false);
                    this.LoadPorcent(this.detList[i].LineaPresupuestoID.Value, i);
                }
                this.gvDetail.MoveLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "sendToApproveDelegate"));
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
        protected bool actividadLineaPresupInd = false;
        protected bool validParticion = true;
        protected bool validHeader = false;
        protected bool initData = false;
        protected bool deleteOP = false;
        protected bool loadME = false;
        //Variables de documentos y detalles
        protected bool isValid_Det = true;
        protected bool disableValidate_Det = false;
        protected DTO_Presupuesto presupuesto = new DTO_Presupuesto();
        protected List<DTO_plPresupuestoDeta> detList = new List<DTO_plPresupuestoDeta>();
        protected int numeroDocPresup = 0;
        //Variables para importar
        protected PasteOpDTO pasteRet;
        protected string format;
        protected string formatSeparator = "\t";
        protected string unboundPrefix = "Unbound_";
        //Filtros y actividades     
        protected List<DTO_glConsultaFiltro> filtrosLineaPres = new List<DTO_glConsultaFiltro>();
        protected List<DTO_glConsultaFiltro> filtrosCentroCosto = new List<DTO_glConsultaFiltro>();
        protected List<DTO_glConsultaFiltro> filtrosProyecto = new List<DTO_glConsultaFiltro>();
        protected List<DTO_glConsultaFiltro> filtrosActividad = new List<DTO_glConsultaFiltro>();
        protected List<DTO_glConsultaFiltro> filtrosAreaFisica = new List<DTO_glConsultaFiltro>();
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
        public PresupuestoContable()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de la actividad
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.Presupuesto);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, AppDocuments.Presupuesto));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "PresupuestoContable"));
            }
        }

        #region Funciones protected del formulario

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this.documentID = AppDocuments.PresupuestoContable;
            this._frmModule = ModulesPrefix.co;
            this.format = _bc.GetImportExportFormat(typeof(DTO_plPresupuestoDeta), this.documentID);

            #region Inicia variables
            //Carga los valores por defecto   
            this.proyectoID = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.LineaPresupuestoIDxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
            this.centroCostoIDxDef = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.actividadLineaPresupInd = this._bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd).Equals("1") ? true : false;
           
            //Tasa de cambio
            string multimonedaStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
            if (multimonedaStr == "1")
            {
                //this.loadME = true;
                //this.lblTasaCambio.Visible = true;
                //this.txtTasaCambio.Visible = true;
                //this.lblMdaExtr1.Visible = true;
                //this.lblMdaExtr2.Visible = true;
                //this.lblMdaExtr3.Visible = true;
                //this.txt_Mes01_Extr.Visible = true;
                //this.txt_Mes02_Extr.Visible = true;
                //this.txt_Mes03_Extr.Visible = true;
                //this.txt_Mes04_Extr.Visible = true;
                //this.txt_Mes05_Extr.Visible = true;
                //this.txt_Mes06_Extr.Visible = true;
                //this.txt_Mes07_Extr.Visible = true;
                //this.txt_Mes08_Extr.Visible = true;
                //this.txt_Mes09_Extr.Visible = true;
                //this.txt_Mes10_Extr.Visible = true;
                //this.txt_Mes11_Extr.Visible = true;
                //this.txt_Mes12_Extr.Visible = true;
            }

            #endregion
            #region Inicia controles

            this.AddColsDetalle();
            this.GetFiltersMasters();
            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(periodoStr);
            this.dtPeriod.DateTime = new DateTime (this.periodo.Year,1,1);
            this.dtPeriod.MinValue = this.dtPeriod.DateTime;

            //Maestras
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false, null);      
            //Combo
            Dictionary<string, string> dicTipoProyecto = new Dictionary<string, string>();
            dicTipoProyecto.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Administrativo));
            //dicTipoProyecto.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Capex));
            //dicTipoProyecto.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Opex));
            //dicTipoProyecto.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inversion));
            this.cmbTipoProyecto.Properties.DataSource = dicTipoProyecto;
            this.cmbTipoProyecto.EditValue = 1;
            this.masterProyecto.Value = this.proyectoID;
            #endregion

            //Delegados
            this.saveDelegate = new Save(this.SaveMethod);
            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
            this.refreshDataDelegate = new RefreshData(this.RefreshDataMethod);
        }

        /// <summary>
        /// Agrega las columnas a la subgrilla
        /// </summary>
        protected void AddColsDetalle()
        {
            try
            {
                //AreaFisica(Campo)
                GridColumn AreaFisicaID = new GridColumn();
                AreaFisicaID.FieldName = this.unboundPrefix + "AreaFisicaID";
                AreaFisicaID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_AreaFisica");
                AreaFisicaID.UnboundType = UnboundColumnType.String;
                AreaFisicaID.VisibleIndex = 0;
                AreaFisicaID.Width = 40;
                AreaFisicaID.Visible = false;
                AreaFisicaID.OptionsColumn.AllowEdit = false;
                AreaFisicaID.ColumnEdit = this.editBtn_Doc;
                this.gvDetail.Columns.Add(AreaFisicaID);

                //ActividadID
                GridColumn ActividadID = new GridColumn();
                ActividadID.FieldName = this.unboundPrefix + "ActividadID";
                ActividadID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_ActividadID");
                ActividadID.UnboundType = UnboundColumnType.String;
                ActividadID.VisibleIndex = 1;
                ActividadID.Width = 40;
                ActividadID.Visible = false;
                ActividadID.OptionsColumn.AllowEdit = false;
                ActividadID.ColumnEdit = this.editBtn_Doc;
                this.gvDetail.Columns.Add(ActividadID);

                //CentroCostoID
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 2;
                centroCostoID.Width = 40;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = false;
                centroCostoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetail.Columns.Add(centroCostoID);

                //LineaPresupuestoID
                GridColumn lineaPresupuestoID = new GridColumn();
                lineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                lineaPresupuestoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_LineaPresupuestoID"); ;
                lineaPresupuestoID.UnboundType = UnboundColumnType.String;
                lineaPresupuestoID.VisibleIndex = 3;
                lineaPresupuestoID.Width = 40;
                lineaPresupuestoID.Visible = true;
                lineaPresupuestoID.OptionsColumn.AllowEdit = this.documentID == AppDocuments.PresupuestoContable ? true : false;
                lineaPresupuestoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetail.Columns.Add(lineaPresupuestoID);

                ////LineaPresupuestoDesc
                //GridColumn LineaPresupuestoDesc = new GridColumn();
                //LineaPresupuestoDesc.FieldName = this.unboundPrefix + "LineaPresDesc";
                //LineaPresupuestoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.GenerarPresupuesto + "_LineaPresDesc");
                //LineaPresupuestoDesc.UnboundType = UnboundColumnType.String;
                //LineaPresupuestoDesc.VisibleIndex = 3;
                //LineaPresupuestoDesc.Width = 60;
                //LineaPresupuestoDesc.Visible = true;
                //LineaPresupuestoDesc.OptionsColumn.AllowFocus = false;
                //LineaPresupuestoDesc.OptionsColumn.AllowEdit = false;
                //this.gvDetail.Columns.Add(LineaPresupuestoDesc);

                //Saldo Anterior Loc
                GridColumn saldoAntLoc = new GridColumn();
                saldoAntLoc.FieldName = this.unboundPrefix + "VlrSaldoAntLoc";
                saldoAntLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_SaldoAnteriorLoc");
                saldoAntLoc.UnboundType = UnboundColumnType.Decimal;
                saldoAntLoc.VisibleIndex = 4;
                saldoAntLoc.Width = 40;
                saldoAntLoc.Visible = this.documentID != AppDocuments.PresupuestoContable ? true : false;
                saldoAntLoc.OptionsColumn.AllowEdit = false;
                saldoAntLoc.OptionsColumn.AllowFocus = false;
                saldoAntLoc.ColumnEdit = this.editValor;
                this.gvDetail.Columns.Add(saldoAntLoc);

                //Movimiento Loc
                GridColumn mvtoLoc = new GridColumn();
                mvtoLoc.FieldName = this.unboundPrefix + "VlrMvtoLocal";
                mvtoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_MovimientoLoc");
                mvtoLoc.UnboundType = UnboundColumnType.Decimal;
                mvtoLoc.VisibleIndex = 5;
                mvtoLoc.Width = 40;
                mvtoLoc.Visible = true;
                mvtoLoc.OptionsColumn.AllowEdit = true;
                mvtoLoc.ColumnEdit = this.editValor;
                mvtoLoc.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                mvtoLoc.AppearanceCell.Options.UseTextOptions = true;
                mvtoLoc.AppearanceCell.Options.UseFont = true;
                this.gvDetail.Columns.Add(mvtoLoc);

                //NuevoSaldo Loc
                GridColumn nuevoSaldoLoc = new GridColumn();
                nuevoSaldoLoc.FieldName = this.unboundPrefix + "VlrNuevoSaldoLoc";
                nuevoSaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_NuevoSaldoLoc");
                nuevoSaldoLoc.UnboundType = UnboundColumnType.Decimal;
                nuevoSaldoLoc.VisibleIndex = 6;
                nuevoSaldoLoc.Width = 40;
                nuevoSaldoLoc.Visible = this.documentID != AppDocuments.PresupuestoContable ? true : false; ;
                nuevoSaldoLoc.OptionsColumn.AllowEdit = false;
                nuevoSaldoLoc.OptionsColumn.AllowFocus = false;
                nuevoSaldoLoc.ColumnEdit = this.editValor;
                this.gvDetail.Columns.Add(nuevoSaldoLoc);

                if (this.loadME)
                {
                    //Saldo Anterior Extr
                    GridColumn saldoAntExtr = new GridColumn();
                    saldoAntExtr.FieldName = this.unboundPrefix + "VlrSaldoAntExtr";
                    saldoAntExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_SaldoAnteriorExtr");
                    saldoAntExtr.UnboundType = UnboundColumnType.Decimal;
                    saldoAntExtr.VisibleIndex = 7;
                    saldoAntExtr.Width = 40;
                    saldoAntExtr.Visible = this.documentID != AppDocuments.PresupuestoContable ? true : false; ;
                    saldoAntExtr.OptionsColumn.AllowEdit = false;
                    saldoAntExtr.OptionsColumn.AllowFocus = false;
                    saldoAntExtr.ColumnEdit = this.editValor;
                    saldoAntExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    saldoAntExtr.AppearanceHeader.Options.UseTextOptions = true;
                    saldoAntExtr.AppearanceHeader.Options.UseFont = true;
                    saldoAntExtr.AppearanceHeader.Options.UseForeColor = true;
                    saldoAntExtr.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    saldoAntExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetail.Columns.Add(saldoAntExtr);

                    //Movimiento Extr
                    GridColumn mvtoExtr = new GridColumn();
                    mvtoExtr.FieldName = this.unboundPrefix + "VlrMvtoExtr";
                    mvtoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_MovimientoExtr");
                    mvtoExtr.UnboundType = UnboundColumnType.Decimal;
                    mvtoExtr.VisibleIndex = 8;
                    mvtoExtr.Width = 40;
                    mvtoExtr.Visible = true;
                    mvtoExtr.OptionsColumn.AllowEdit = true;
                    mvtoExtr.ColumnEdit = this.editValor;
                    mvtoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    mvtoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceHeader.Options.UseFont = true;
                    mvtoExtr.AppearanceHeader.Options.UseForeColor = true;
                    mvtoExtr.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    mvtoExtr.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceCell.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceCell.Options.UseFont = true;
                    this.gvDetail.Columns.Add(mvtoExtr);

                    //NuevoSaldo Extr
                    GridColumn nuevoSaldoExtr = new GridColumn();
                    nuevoSaldoExtr.FieldName = this.unboundPrefix + "VlrNuevoSaldoExtr";
                    nuevoSaldoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_NuevoSaldoExtr");
                    nuevoSaldoExtr.UnboundType = UnboundColumnType.Decimal;
                    nuevoSaldoExtr.VisibleIndex = 9;
                    nuevoSaldoExtr.Width = 40;
                    nuevoSaldoExtr.Visible = this.documentID != AppDocuments.PresupuestoContable ? true : false; ;
                    nuevoSaldoExtr.OptionsColumn.AllowEdit = false;
                    nuevoSaldoExtr.OptionsColumn.AllowFocus = false;
                    nuevoSaldoExtr.ColumnEdit = this.editValor;
                    nuevoSaldoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseFont = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseForeColor = true;
                    nuevoSaldoExtr.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    nuevoSaldoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetail.Columns.Add(nuevoSaldoExtr);
                }

                //Porcentaje01
                GridColumn Porcentaje01 = new GridColumn();
                Porcentaje01.FieldName = this.unboundPrefix + "Porcentaje01";
                Porcentaje01.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje01");
                Porcentaje01.UnboundType = UnboundColumnType.Decimal;
                Porcentaje01.VisibleIndex = 9;
                Porcentaje01.Width = 24;
                Porcentaje01.Visible = true;
                Porcentaje01.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje01);

                //Porcentaje02
                GridColumn Porcentaje02 = new GridColumn();
                Porcentaje02.FieldName = this.unboundPrefix + "Porcentaje02";
                Porcentaje02.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje02");
                Porcentaje02.UnboundType = UnboundColumnType.Decimal;
                Porcentaje02.VisibleIndex = 10;
                Porcentaje02.Width = 24;
                Porcentaje02.Visible = true;
                Porcentaje02.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje02);

                //Porcentaje03
                GridColumn Porcentaje03 = new GridColumn();
                Porcentaje03.FieldName = this.unboundPrefix + "Porcentaje03";
                Porcentaje03.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje03");
                Porcentaje03.UnboundType = UnboundColumnType.Decimal;
                Porcentaje03.VisibleIndex = 11;
                Porcentaje03.Width = 24;
                Porcentaje03.Visible = true;
                Porcentaje03.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje03);

                //Porcentaje04
                GridColumn Porcentaje04 = new GridColumn();
                Porcentaje04.FieldName = this.unboundPrefix + "Porcentaje04";
                Porcentaje04.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje04");
                Porcentaje04.UnboundType = UnboundColumnType.Decimal;
                Porcentaje04.VisibleIndex = 12;
                Porcentaje04.Width = 24;
                Porcentaje04.Visible = true;
                Porcentaje04.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje04);

                //Porcentaje05
                GridColumn Porcentaje05 = new GridColumn();
                Porcentaje05.FieldName = this.unboundPrefix + "Porcentaje05";
                Porcentaje05.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje05");
                Porcentaje05.UnboundType = UnboundColumnType.Decimal;
                Porcentaje05.VisibleIndex = 13;
                Porcentaje05.Width = 24;
                Porcentaje05.Visible = true;
                Porcentaje05.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje05);

                //Porcentaje06
                GridColumn Porcentaje06 = new GridColumn();
                Porcentaje06.FieldName = this.unboundPrefix + "Porcentaje06";
                Porcentaje06.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje06");
                Porcentaje06.UnboundType = UnboundColumnType.Decimal;
                Porcentaje06.VisibleIndex = 14;
                Porcentaje06.Width = 24;
                Porcentaje06.Visible = true;
                Porcentaje06.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje06);

                //Porcentaje07
                GridColumn Porcentaje07 = new GridColumn();
                Porcentaje07.FieldName = this.unboundPrefix + "Porcentaje07";
                Porcentaje07.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje07");
                Porcentaje07.UnboundType = UnboundColumnType.Decimal;
                Porcentaje07.VisibleIndex = 15;
                Porcentaje07.Width = 24;
                Porcentaje07.Visible = true;
                Porcentaje07.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje07);

                //Porcentaje08
                GridColumn Porcentaje08 = new GridColumn();
                Porcentaje08.FieldName = this.unboundPrefix + "Porcentaje08";
                Porcentaje08.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje08");
                Porcentaje08.UnboundType = UnboundColumnType.Decimal;
                Porcentaje08.VisibleIndex = 16;
                Porcentaje08.Width = 24;
                Porcentaje08.Visible = true;
                Porcentaje08.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje08);

                //Porcentaje09
                GridColumn Porcentaje09 = new GridColumn();
                Porcentaje09.FieldName = this.unboundPrefix + "Porcentaje09";
                Porcentaje09.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje09");
                Porcentaje09.UnboundType = UnboundColumnType.Decimal;
                Porcentaje09.VisibleIndex = 17;
                Porcentaje09.Width = 24;
                Porcentaje09.Visible = true;
                Porcentaje09.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje09);

                //Porcentaje10
                GridColumn Porcentaje10 = new GridColumn();
                Porcentaje10.FieldName = this.unboundPrefix + "Porcentaje10";
                Porcentaje10.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje10");
                Porcentaje10.UnboundType = UnboundColumnType.Decimal;
                Porcentaje10.VisibleIndex = 18;
                Porcentaje10.Width = 24;
                Porcentaje10.Visible = true;
                Porcentaje10.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje10);

                //Porcentaje11
                GridColumn Porcentaje11 = new GridColumn();
                Porcentaje11.FieldName = this.unboundPrefix + "Porcentaje11";
                Porcentaje11.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje11");
                Porcentaje11.UnboundType = UnboundColumnType.Decimal;
                Porcentaje11.VisibleIndex = 19;
                Porcentaje11.Width = 24;
                Porcentaje11.Visible = true;
                Porcentaje11.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje11);

                //Porcentaje12
                GridColumn Porcentaje12 = new GridColumn();
                Porcentaje12.FieldName = this.unboundPrefix + "Porcentaje12";
                Porcentaje12.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_Porcentaje12");
                Porcentaje12.UnboundType = UnboundColumnType.Decimal;
                Porcentaje12.VisibleIndex = 20;
                Porcentaje12.Width = 24;
                Porcentaje12.Visible = true;
                Porcentaje12.OptionsColumn.AllowEdit = false;
                this.gvDetail.Columns.Add(Porcentaje12);

                #region Columnas no visibles
                //Observacion Deta
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "DescripTExt";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_DescripTExt");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 21;
                DescripTExt.Width = 60;
                DescripTExt.Visible = false;
                DescripTExt.ColumnEdit = this.richText1;
                this.gvDetail.Columns.Add(DescripTExt);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PresupuestoContable.cs-AddCols_Det"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected virtual void AddNewRow_Det()
        {
            try
            {
                DTO_plPresupuestoDeta det = new DTO_plPresupuestoDeta(true);

                #region Asigna datos a la fila
                det.Consecutivo.Value = this.detList.Count == 0 ? 1 : this.detList.Last().Consecutivo.Value.Value + 1;
                if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
                {
                    det.ProyectoID.Value = this.masterProyecto.Value;
                    det.LocFisicaID.Value = this.locFisicaID;
                    det.AreaFisicaID.Value = this.areaFisicaID;
                    det.ActividadID.Value = this.actividadID;
                }
                det.CentroCostoID.Value = string.Empty;// this.centroCostoIDxDef;
                det.Ano.Value = this.dtPeriod.DateTime.Year;
                #endregion

                this.detList.Add(det);
                this.LoadDetails(false);
                //this.EnableControls(true);
                this.isValid_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PresupuestoContable.cs-AddNewRow_Det"));
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
                if (this.detList.Count > 0)
                {
                    bool validField = true;
                    this.isValid_Det = true;

                    int fila = this.gvDetail.FocusedRowHandle;

                    #region Validacion de nulls
                    #region Centro Costo
                    validField = _bc.ValidGridCell(this.gvDetail, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                    if (!validField)
                        this.isValid_Det = false;
                    #endregion
                    #region Linea Presupuesto
                    validField = _bc.ValidGridCell(this.gvDetail, string.Empty, fila, "LineaPresupuestoID", false, false, false, AppMasters.plLineaPresupuesto);
                    if (!validField)
                        this.isValid_Det = false;
                    #endregion
                    #endregion
                    #region Validacion de PKs
                    if (this.isValid_Det && this.detList.Count > 0)
                    {
                        DTO_plPresupuestoDeta det = this.detList[fila];
                        int count = this.detList.Where(x => x.CentroCostoID.Value == det.CentroCostoID.Value &&
                                                            x.LineaPresupuestoID.Value == det.LineaPresupuestoID.Value &&
                                                            x.ProyectoID.Value == det.ProyectoID.Value).Count();
                        if (count > 1)
                        {
                            this.isValid_Det = false;
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidPresDet));
                        }
                    }
                    if (!this.validParticion)
                    {
                        GridColumn col = this.gvDetail.Columns[this.unboundPrefix + "LineaPresupuestoID"];
                        this.gvDetail.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidKeyActividadLinea));
                        this.isValid_Det = false;
                        return this.isValid_Det;
                    }
                    #endregion
                    #region Valor movimiento
                    validField = _bc.ValidGridCellValue(this.gvDetail, string.Empty, fila, "VlrMvtoLocal", false, (this.detList[fila].VlrMvtoExtr.Value == 0 &&
                                this.documentID == AppDocuments.PresupuestoContable ? false : true), this.documentID == AppDocuments.ReclasifPresupuesto ? false : true, false);
                    if (!validField)
                        this.isValid_Det = false;
                    if (this.loadME)
                    {
                        validField = _bc.ValidGridCellValue(this.gvDetail, string.Empty, fila, "VlrMvtoExtr", false, this.detList[fila].VlrMvtoLocal.Value == 0 &&
                                 this.documentID == AppDocuments.PresupuestoContable ? false : true, this.documentID == AppDocuments.ReclasifPresupuesto ? false : true, false);
                        if (!validField)
                            this.isValid_Det = false;
                    }
                    #endregion
                    #region Calculo de datos
                    if (this.isValid_Det && this.detList.Count > 0)
                    {
                        #region Totales
                        DTO_plPresupuestoDeta det = this.detList[fila];

                        if (det.VlrNuevoSaldoLoc.Value < 0)
                        {
                            GridColumn col = this.gvDetail.Columns[unboundPrefix + "VlrNuevoSaldoLoc"];
                            this.gvDetail.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                            this.isValid_Det = false;
                            return false;
                        }

                        if (det.VlrNuevoSaldoExtr.Value < 0)
                        {
                            GridColumn col = this.gvDetail.Columns[unboundPrefix + "VlrNuevoSaldoExtr"];
                            this.gvDetail.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                            this.isValid_Det = false;
                            return false;
                        }

                        decimal sumML = Math.Round(det.ValorLoc00.Value.Value + det.ValorLoc01.Value.Value + det.ValorLoc02.Value.Value + det.ValorLoc03.Value.Value +
                                        det.ValorLoc04.Value.Value + det.ValorLoc05.Value.Value + det.ValorLoc06.Value.Value + det.ValorLoc07.Value.Value +
                                        det.ValorLoc08.Value.Value + det.ValorLoc09.Value.Value + det.ValorLoc10.Value.Value + det.ValorLoc11.Value.Value + det.ValorLoc12.Value.Value);

                        if (sumML != det.VlrMvtoLocal.Value.Value)
                        {
                            this.isValid_Det = false;
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                        }

                        if (this.loadME)
                        {
                            decimal sumME = Math.Round(det.ValorExt00.Value.Value + det.ValorExt01.Value.Value + det.ValorExt02.Value.Value + det.ValorExt03.Value.Value +
                                     det.ValorExt04.Value.Value + det.ValorExt05.Value.Value + det.ValorExt06.Value.Value + det.ValorExt07.Value.Value +
                                     det.ValorExt08.Value.Value + det.ValorExt09.Value.Value + det.ValorExt10.Value.Value + det.ValorExt11.Value.Value + det.ValorExt12.Value.Value);

                            if (sumME != det.VlrMvtoExtr.Value.Value)
                            {
                                this.isValid_Det = false;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                this.isValid_Det = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "ValidateRow_Doc"));
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
                {
                    this.LoadFooter(fila);
                    this.isValid_Det = true;
                    this.gvDetail.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detList[fila].NewRowPresup;
                    this.gvDetail.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detList[fila].NewRowPresup;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "RowIndexChanged_Det"));
            }
        }

        /// <summary>
        /// Limpia / Habilita el formulario
        /// </summary>
        protected virtual void EnableControls(bool enable)
        {
            //this.dtPeriod.Enabled = enable;
            //this.masterProyecto.EnableControl(enable);

            this.txt_Mes01_Local.Enabled = !enable;
            this.txt_Mes02_Local.Enabled = !enable;
            this.txt_Mes03_Local.Enabled = !enable;
            this.txt_Mes04_Local.Enabled = !enable;
            this.txt_Mes05_Local.Enabled = !enable;
            this.txt_Mes06_Local.Enabled = !enable;
            this.txt_Mes07_Local.Enabled = !enable;
            this.txt_Mes08_Local.Enabled = !enable;
            this.txt_Mes09_Local.Enabled = !enable;
            this.txt_Mes10_Local.Enabled = !enable;
            this.txt_Mes11_Local.Enabled = !enable;
            this.txt_Mes12_Local.Enabled = !enable;
            this.txt_Mes01_Extr.Enabled = !enable;
            this.txt_Mes02_Extr.Enabled = !enable;
            this.txt_Mes03_Extr.Enabled = !enable;
            this.txt_Mes04_Extr.Enabled = !enable;
            this.txt_Mes05_Extr.Enabled = !enable;
            this.txt_Mes06_Extr.Enabled = !enable;
            this.txt_Mes07_Extr.Enabled = !enable;
            this.txt_Mes08_Extr.Enabled = !enable;
            this.txt_Mes09_Extr.Enabled = !enable;
            this.txt_Mes10_Extr.Enabled = !enable;
            this.txt_Mes11_Extr.Enabled = !enable;
            this.txt_Mes12_Extr.Enabled = !enable;

            if (enable)
            {
                this.disableValidate_Det = true;

                this.initData = true;
                this.gcDetail.DataSource = this.detList;

                this.LoadDetails(true);
                this.initData = false;
                this.disableValidate_Det = false;

                this.proyectoID = string.Empty;
                this.masterProyecto.Value = string.Empty;
                //this.txtTasaCambio.Text = "0";
                this.cmbTipoProyecto.EditValue = "1";

                this.txt_Mes01_Local.Text = "0";
                this.txt_Mes02_Local.Text = "0";
                this.txt_Mes03_Local.Text = "0";
                this.txt_Mes04_Local.Text = "0";
                this.txt_Mes05_Local.Text = "0";
                this.txt_Mes06_Local.Text = "0";
                this.txt_Mes07_Local.Text = "0";
                this.txt_Mes08_Local.Text = "0";
                this.txt_Mes09_Local.Text = "0";
                this.txt_Mes10_Local.Text = "0";
                this.txt_Mes11_Local.Text = "0";
                this.txt_Mes12_Local.Text = "0";

                this.txt_Mes01_Extr.Text = "0";
                this.txt_Mes02_Extr.Text = "0";
                this.txt_Mes03_Extr.Text = "0";
                this.txt_Mes04_Extr.Text = "0";
                this.txt_Mes05_Extr.Text = "0";
                this.txt_Mes06_Extr.Text = "0";
                this.txt_Mes07_Extr.Text = "0";
                this.txt_Mes08_Extr.Text = "0";
                this.txt_Mes09_Extr.Text = "0";
                this.txt_Mes10_Extr.Text = "0";
                this.txt_Mes11_Extr.Text = "0";
                this.txt_Mes12_Extr.Text = "0";

                this.dtPeriod.Focus();
            }
            else
                FormProvider.Master.itemNew.Enabled = true;
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
            if (enable)
            {
                this.txt_Mes01_Local.Enabled = this.dtPeriod.DateTime.Month == 1 ? true : false;
                this.txt_Mes02_Local.Enabled = this.dtPeriod.DateTime.Month <= 2 ? true : false;
                this.txt_Mes03_Local.Enabled = this.dtPeriod.DateTime.Month <= 3 ? true : false;
                this.txt_Mes04_Local.Enabled = this.dtPeriod.DateTime.Month <= 4 ? true : false;
                this.txt_Mes05_Local.Enabled = this.dtPeriod.DateTime.Month <= 5 ? true : false;
                this.txt_Mes06_Local.Enabled = this.dtPeriod.DateTime.Month <= 6 ? true : false;
                this.txt_Mes07_Local.Enabled = this.dtPeriod.DateTime.Month <= 7 ? true : false;
                this.txt_Mes08_Local.Enabled = this.dtPeriod.DateTime.Month <= 8 ? true : false;
                this.txt_Mes09_Local.Enabled = this.dtPeriod.DateTime.Month <= 9 ? true : false;
                this.txt_Mes10_Local.Enabled = this.dtPeriod.DateTime.Month <= 10 ? true : false;
                this.txt_Mes11_Local.Enabled = this.dtPeriod.DateTime.Month <= 11 ? true : false;
                this.txt_Mes12_Local.Enabled = this.dtPeriod.DateTime.Month <= 12 ? true : false;
                this.txt_Mes01_Extr.Enabled = this.dtPeriod.DateTime.Month == 1 ? true : false;
                this.txt_Mes02_Extr.Enabled = this.dtPeriod.DateTime.Month <= 2 ? true : false;
                this.txt_Mes03_Extr.Enabled = this.dtPeriod.DateTime.Month <= 3 ? true : false;
                this.txt_Mes04_Extr.Enabled = this.dtPeriod.DateTime.Month <= 4 ? true : false;
                this.txt_Mes05_Extr.Enabled = this.dtPeriod.DateTime.Month <= 5 ? true : false;
                this.txt_Mes06_Extr.Enabled = this.dtPeriod.DateTime.Month <= 6 ? true : false;
                this.txt_Mes07_Extr.Enabled = this.dtPeriod.DateTime.Month <= 7 ? true : false;
                this.txt_Mes08_Extr.Enabled = this.dtPeriod.DateTime.Month <= 8 ? true : false;
                this.txt_Mes09_Extr.Enabled = this.dtPeriod.DateTime.Month <= 9 ? true : false;
                this.txt_Mes10_Extr.Enabled = this.dtPeriod.DateTime.Month <= 10 ? true : false;
                this.txt_Mes11_Extr.Enabled = this.dtPeriod.DateTime.Month <= 11 ? true : false;
                this.txt_Mes12_Extr.Enabled = this.dtPeriod.DateTime.Month <= 12 ? true : false;
            }
            else
            {
                this.txt_Mes01_Local.Enabled = false;
                this.txt_Mes02_Local.Enabled = false;
                this.txt_Mes03_Local.Enabled = false;
                this.txt_Mes04_Local.Enabled = false;
                this.txt_Mes05_Local.Enabled = false;
                this.txt_Mes06_Local.Enabled = false;
                this.txt_Mes07_Local.Enabled = false;
                this.txt_Mes08_Local.Enabled = false;
                this.txt_Mes09_Local.Enabled = false;
                this.txt_Mes10_Local.Enabled = false;
                this.txt_Mes11_Local.Enabled = false;
                this.txt_Mes12_Local.Enabled = false;
                this.txt_Mes01_Extr.Enabled = false;
                this.txt_Mes02_Extr.Enabled = false;
                this.txt_Mes03_Extr.Enabled = false;
                this.txt_Mes04_Extr.Enabled = false;
                this.txt_Mes05_Extr.Enabled = false;
                this.txt_Mes06_Extr.Enabled = false;
                this.txt_Mes07_Extr.Enabled = false;
                this.txt_Mes08_Extr.Enabled = false;
                this.txt_Mes09_Extr.Enabled = false;
                this.txt_Mes10_Extr.Enabled = false;
                this.txt_Mes11_Extr.Enabled = false;
                this.txt_Mes12_Extr.Enabled = false;
            }
        }

        /// <summary>
        /// Carga la info del formulario
        /// </summary>
        protected virtual void LoadData()
        {
            try
            {                
                #region Opex/Administrativo
                this.initData = true;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);

                #region Carga nuevo Presupuesto
                if (this.presupuesto == null)
                {
                    this.detList = new List<DTO_plPresupuestoDeta>();                            
                            
                        #region Filtra y recorre Proyectos
                        long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.coProyecto, null, null, true);
                        var proyectos = this._bc.AdministrationModel.MasterHierarchy_GetPaged(AppMasters.coProyecto, count, 1, null, null, true).ToList();
                        #endregion
                        foreach (var proy in proyectos.Where(x => x.MovInd.Value.Value))
                        {
                            DTO_coProyecto dtoProy = (DTO_coProyecto)proy;
                            DTO_coActividad dtoActividad = (DTO_coActividad)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coActividad, false, dtoProy.ActividadID.Value, true);
                            #region Llena el detalle con la info obtenida SI es de Presupuesto y SI es Opex o Admin
                            if (dtoProy.PresupuestalInd.Value.Value && (dtoActividad.ProyectoTipo.Value == (byte)ProyectoTipo.Opex ||
                                                                        dtoActividad.ProyectoTipo.Value == (byte)ProyectoTipo.Administracion))
                            {
                                DTO_plPresupuestoDeta deta = new DTO_plPresupuestoDeta(true);
                                deta.ProyectoID.Value = dtoProy.ID.Value;
                                deta.ActividadID.Value = dtoProy.ActividadID.Value;
                                deta.ContratoID.Value = dtoProy.ContratoID.Value;
                                deta.LocFisicaID.Value = dtoProy.LocFisicaID.Value;
                                //deta.AreaFisicaID.Value = campo.ID.Value;
                                deta.CentroCostoID.Value = this.centroCostoIDxDef;
                                deta.Ano.Value = this.dtPeriod.DateTime.Year;
                                deta.NumeroDoc.Value = 0;
                                this.detList.Add(deta);
                            }
                            #endregion
                        }
                }
                #endregion
                #region Carga Presupuesto Existente
                else
                {
                    if (this.presupuesto.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
                    {
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                    }
                    #region Carga el cabezote
                    this.dtPeriod.DateTime = this.periodo;
                    //this.txtTasaCambio.EditValue = this.presupuesto.DocCtrl.TasaCambioDOCU.Value.Value;
                    this.numeroDocPresup = this.presupuesto.DocCtrl.NumeroDoc.Value.Value;
                    #endregion

                    #region Carga Detalle
                    //Total del movimiento(Local)
                    this.presupuesto.Detalles.ForEach(x => x.VlrMvtoLocal.Value = Math.Round(x.ValorLoc00.Value.Value + x.ValorLoc01.Value.Value + x.ValorLoc02.Value.Value + x.ValorLoc03.Value.Value
                                                + x.ValorLoc04.Value.Value + x.ValorLoc05.Value.Value + x.ValorLoc06.Value.Value + x.ValorLoc07.Value.Value + x.ValorLoc08.Value.Value
                                                + x.ValorLoc09.Value.Value + x.ValorLoc10.Value.Value + x.ValorLoc11.Value.Value + x.ValorLoc12.Value.Value));
                    //Total del movimiento(Extr)
                    this.presupuesto.Detalles.ForEach(x => x.VlrMvtoExtr.Value = Math.Round(x.ValorExt00.Value.Value + x.ValorExt01.Value.Value + x.ValorExt02.Value.Value + x.ValorExt03.Value.Value
                                                + x.ValorExt04.Value.Value + x.ValorExt05.Value.Value + x.ValorExt06.Value.Value + x.ValorExt07.Value.Value + x.ValorExt08.Value.Value
                                                + x.ValorExt09.Value.Value + x.ValorExt10.Value.Value + x.ValorExt11.Value.Value + x.ValorExt12.Value.Value));

                    foreach (DTO_plPresupuestoDeta item in this.presupuesto.Detalles)
                    {
                        DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                        item.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                        DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                        item.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                        item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                        item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                        item.LoadParticionLocalInd = item.VlrMvtoLocal.Value != 0 ? false : true;
                        item.LoadParticionExtrInd = item.VlrMvtoExtr.Value != 0 ? false : true;
                    }
                    this.detList = this.presupuesto.Detalles;
                    #endregion

                }
                #endregion

                //this.proyectoID = this.detList.Count > 0 ? this.detList.First().ProyectoID.Value : string.Empty;
                this.validHeader = true;
                this.gcDetail.DataSource = this.detList;
                this.gcDetail.RefreshDataSource();
                this.isValid_Det = true;
                this.LoadDetails(true);
                this.initData = false;
                #endregion
            }

            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la info de la grilla del detalle
        /// </summary>
        protected virtual void LoadDetails(bool isDocExist)
        {
            try
            {
                this.disableValidate_Det = true;
                this.gcDetail.DataSource = this.detList;

                this.gvDetail.RefreshData();
                this.gvDetail.FocusedRowHandle = this.gvDetail.DataRowCount - 1;

                this.LoadParticiones(this.gvDetail.FocusedRowHandle, isDocExist);
                this.EnableFooter(true);

                this.disableValidate_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Carga las particiones del detalle
        /// </summary>
        /// <param name="index">Fila de la grilla</param>
        /// <param name="isDocExist">indica si el documento ya existe y solo asigna valores</param>
        /// <param name="loadByMoneda">Tipo de moneda a cargar valores</param>
        protected virtual void LoadParticiones(int index, bool isDocExist, TipoMoneda loadByMoneda = TipoMoneda.Both)
        {
            int numMeses = 12 - this.dtPeriod.DateTime.Month + 1;
            bool onlyFija = true;

            try
            {
                decimal tc = 0;// Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                if (this.detList.Count > 0)
                {
                    if (isDocExist)
                    {
                        #region Calculo de totales
                        this.txt_Mes01_Local.EditValue = this.detList[index].ValorLoc01.Value;
                        this.txt_Mes02_Local.EditValue = this.detList[index].ValorLoc02.Value;
                        this.txt_Mes03_Local.EditValue = this.detList[index].ValorLoc03.Value;
                        this.txt_Mes04_Local.EditValue = this.detList[index].ValorLoc04.Value;
                        this.txt_Mes05_Local.EditValue = this.detList[index].ValorLoc05.Value;
                        this.txt_Mes06_Local.EditValue = this.detList[index].ValorLoc06.Value;
                        this.txt_Mes07_Local.EditValue = this.detList[index].ValorLoc07.Value;
                        this.txt_Mes08_Local.EditValue = this.detList[index].ValorLoc08.Value;
                        this.txt_Mes09_Local.EditValue = this.detList[index].ValorLoc09.Value;
                        this.txt_Mes10_Local.EditValue = this.detList[index].ValorLoc10.Value;
                        this.txt_Mes11_Local.EditValue = this.detList[index].ValorLoc11.Value;
                        this.txt_Mes12_Local.EditValue = this.detList[index].ValorLoc12.Value;

                        this.txt_Mes01_Extr.EditValue = this.detList[index].ValorExt01.Value;
                        this.txt_Mes02_Extr.EditValue = this.detList[index].ValorExt02.Value;
                        this.txt_Mes03_Extr.EditValue = this.detList[index].ValorExt03.Value;
                        this.txt_Mes04_Extr.EditValue = this.detList[index].ValorExt04.Value;
                        this.txt_Mes05_Extr.EditValue = this.detList[index].ValorExt05.Value;
                        this.txt_Mes06_Extr.EditValue = this.detList[index].ValorExt06.Value;
                        this.txt_Mes07_Extr.EditValue = this.detList[index].ValorExt07.Value;
                        this.txt_Mes08_Extr.EditValue = this.detList[index].ValorExt08.Value;
                        this.txt_Mes09_Extr.EditValue = this.detList[index].ValorExt09.Value;
                        this.txt_Mes10_Extr.EditValue = this.detList[index].ValorExt10.Value;
                        this.txt_Mes11_Extr.EditValue = this.detList[index].ValorExt11.Value;
                        this.txt_Mes12_Extr.EditValue = this.detList[index].ValorExt12.Value;
                        #endregion
                    }
                    else
                    {
                        if (this.actividadLineaPresupInd)
                        {
                            //Obtiene el Control Costo a traves de la LineaPresupuesto y la actividad
                            Dictionary<string, string> pksActLinea = new Dictionary<string, string>();
                            pksActLinea.Add("LineaPresupuestoID", this.detList[index].LineaPresupuestoID.Value);
                            pksActLinea.Add("ActividadID", this.detList[index].ActividadID.Value);
                            DTO_plActividadLineaPresupuestal actividadLinea = (DTO_plActividadLineaPresupuestal)_bc.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pksActLinea, true);
                            if (actividadLinea != null)
                            {
                                if (actividadLinea.ControlCosto.Value == (byte)ControlCosto.Variable || actividadLinea.ControlCosto.Value == (byte)ControlCosto.Estacionario)
                                {
                                    //Distribucion Variable Mensual
                                    #region Asigna particion variable segun Tipo moneda
                                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyecto.Value, true);
                                    DTO_glLocFisica locFisica = (DTO_glLocFisica)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);

                                    if (locFisica != null)
                                    {
                                        Dictionary<string, string> pks = new Dictionary<string, string>();
                                        pks.Add("PeriodoID", this.dtPeriod.DateTime.ToShortDateString());
                                        pks.Add("AreaFisicaID", locFisica.AreaFisica.Value);
                                        pks.Add("TipoCosteo", actividadLinea.ControlCosto.Value.ToString());
                                        DTO_plDistribucionCampo distribxMes = (DTO_plDistribucionCampo)_bc.GetMasterComplexDTO(AppMasters.plDistribucionCampo, pks, true);

                                        if (distribxMes != null)
                                        {
                                            #region Calcula % de Ajuste para que la distribucion siempre sea del 100% sin importar el periodo actual
                                            int periodo = this.dtPeriod.DateTime.Month;
                                            decimal porcentajeAjuste = 0;
                                            switch (periodo)
                                            {
                                                case 1: //Enero
                                                    porcentajeAjuste = 100 / 100;
                                                    break;
                                                case 2: //Febrero
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje02.Value.Value + distribxMes.Porcentaje03.Value.Value +
                                                                            distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                            distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                            distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                            distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                            distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 3: //Marzo
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje03.Value.Value + distribxMes.Porcentaje04.Value.Value +
                                                                             distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                             distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                             distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                             distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 4: //Abril
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                            distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                            distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                            distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                            distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 5: //Mayo
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                             distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                             distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                             distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 6: //Junio
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                            distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                            distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                            distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 7: //Julio
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                             distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                             distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 8: //Agosto
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                            distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                            distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 9: //Septiembre
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                            distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 10: //Octubre
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                            distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 11: //Noviembre
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                                case 12: //Diciembre
                                                    porcentajeAjuste = 100 / (distribxMes.Porcentaje12.Value.Value);
                                                    break;
                                            }
                                            #endregion
                                            switch (loadByMoneda)
                                            {
                                                case TipoMoneda.Local:
                                                    #region Particion Mda Local
                                                    //Enero
                                                    if (this.dtPeriod.DateTime.Month == 1)
                                                        this.txt_Mes01_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes01_Local.EditValue = "0";
                                                    //Febrero
                                                    if (this.dtPeriod.DateTime.Month <= 2)
                                                        this.txt_Mes02_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes02_Local.EditValue = "0";
                                                    //Marzo
                                                    if (this.dtPeriod.DateTime.Month <= 3)
                                                        this.txt_Mes03_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes03_Local.EditValue = "0";
                                                    //Abril
                                                    if (this.dtPeriod.DateTime.Month <= 4)
                                                        this.txt_Mes04_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes04_Local.EditValue = "0";
                                                    //Mayo
                                                    if (this.dtPeriod.DateTime.Month <= 5)
                                                        this.txt_Mes05_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes05_Local.EditValue = "0";
                                                    //Junio
                                                    if (this.dtPeriod.DateTime.Month <= 6)
                                                        this.txt_Mes06_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes06_Local.EditValue = "0";
                                                    //Julio
                                                    if (this.dtPeriod.DateTime.Month <= 7)
                                                        this.txt_Mes07_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes07_Local.EditValue = "0";
                                                    //Agosto
                                                    if (this.dtPeriod.DateTime.Month <= 8)
                                                        this.txt_Mes08_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes08_Local.EditValue = "0";
                                                    //Septiembre
                                                    if (this.dtPeriod.DateTime.Month <= 9)
                                                        this.txt_Mes09_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes09_Local.EditValue = "0";
                                                    //Octubre
                                                    if (this.dtPeriod.DateTime.Month <= 10)
                                                        this.txt_Mes10_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes10_Local.EditValue = "0";
                                                    //Noviembre
                                                    if (this.dtPeriod.DateTime.Month <= 11)
                                                        this.txt_Mes11_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes11_Local.EditValue = "0";
                                                    //Diciembre
                                                    this.txt_Mes12_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                    #endregion
                                                    break;
                                                case TipoMoneda.Foreign:
                                                    #region Particion Mda Extr
                                                    //Enero
                                                    if (this.dtPeriod.DateTime.Month == 1)
                                                        this.txt_Mes01_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes01_Extr.EditValue = "0";
                                                    //Febrero
                                                    if (this.dtPeriod.DateTime.Month <= 2)
                                                        this.txt_Mes02_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes02_Extr.EditValue = "0";
                                                    //Marzo
                                                    if (this.dtPeriod.DateTime.Month <= 3)
                                                        this.txt_Mes03_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes03_Extr.EditValue = "0";
                                                    //Abril
                                                    if (this.dtPeriod.DateTime.Month <= 4)
                                                        this.txt_Mes04_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes04_Extr.EditValue = "0";
                                                    //Mayo
                                                    if (this.dtPeriod.DateTime.Month <= 5)
                                                        this.txt_Mes05_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes05_Extr.EditValue = "0";
                                                    //Junio
                                                    if (this.dtPeriod.DateTime.Month <= 6)
                                                        this.txt_Mes06_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes06_Extr.EditValue = "0";
                                                    //Julio
                                                    if (this.dtPeriod.DateTime.Month <= 7)
                                                        this.txt_Mes07_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes07_Extr.EditValue = "0";
                                                    //Agosto
                                                    if (this.dtPeriod.DateTime.Month <= 8)
                                                        this.txt_Mes08_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes08_Extr.EditValue = "0";
                                                    //Septiembre
                                                    if (this.dtPeriod.DateTime.Month <= 9)
                                                        this.txt_Mes09_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes09_Extr.EditValue = "0";
                                                    //Octubre
                                                    if (this.dtPeriod.DateTime.Month <= 10)
                                                        this.txt_Mes10_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes10_Extr.EditValue = "0";
                                                    //Noviembre
                                                    if (this.dtPeriod.DateTime.Month <= 11)
                                                        this.txt_Mes11_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                    else
                                                        this.txt_Mes11_Extr.EditValue = "0";
                                                    //Diciembre
                                                    this.txt_Mes12_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                    #endregion
                                                    break;
                                                case TipoMoneda.Both:
                                                    #region Particion Mda Local
                                                    if (this.detList[index].LoadParticionLocalInd)
                                                    {
                                                        //Enero
                                                        if (this.dtPeriod.DateTime.Month == 1)
                                                            this.txt_Mes01_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Local.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriod.DateTime.Month <= 2)
                                                            this.txt_Mes02_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Local.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriod.DateTime.Month <= 3)
                                                            this.txt_Mes03_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Local.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriod.DateTime.Month <= 4)
                                                            this.txt_Mes04_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Local.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriod.DateTime.Month <= 5)
                                                            this.txt_Mes05_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Local.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriod.DateTime.Month <= 6)
                                                            this.txt_Mes06_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Local.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriod.DateTime.Month <= 7)
                                                            this.txt_Mes07_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Local.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriod.DateTime.Month <= 8)
                                                            this.txt_Mes08_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Local.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriod.DateTime.Month <= 9)
                                                            this.txt_Mes09_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Local.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriod.DateTime.Month <= 10)
                                                            this.txt_Mes10_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Local.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriod.DateTime.Month <= 11)
                                                            this.txt_Mes11_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Local.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Local.EditValue = Math.Round((this.detList[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                    }
                                                    else
                                                    {
                                                        this.txt_Mes01_Local.EditValue = this.detList[index].ValorLoc01.Value;
                                                        this.txt_Mes02_Local.EditValue = this.detList[index].ValorLoc02.Value;
                                                        this.txt_Mes03_Local.EditValue = this.detList[index].ValorLoc03.Value;
                                                        this.txt_Mes04_Local.EditValue = this.detList[index].ValorLoc04.Value;
                                                        this.txt_Mes05_Local.EditValue = this.detList[index].ValorLoc05.Value;
                                                        this.txt_Mes06_Local.EditValue = this.detList[index].ValorLoc06.Value;
                                                        this.txt_Mes07_Local.EditValue = this.detList[index].ValorLoc07.Value;
                                                        this.txt_Mes08_Local.EditValue = this.detList[index].ValorLoc08.Value;
                                                        this.txt_Mes09_Local.EditValue = this.detList[index].ValorLoc09.Value;
                                                        this.txt_Mes10_Local.EditValue = this.detList[index].ValorLoc10.Value;
                                                        this.txt_Mes11_Local.EditValue = this.detList[index].ValorLoc11.Value;
                                                        this.txt_Mes12_Local.EditValue = this.detList[index].ValorLoc12.Value;
                                                    }
                                                    #endregion
                                                    #region Particion Mda Extr
                                                    if (this.detList[index].LoadParticionExtrInd)
                                                    {
                                                        //Enero
                                                        if (this.dtPeriod.DateTime.Month == 1)
                                                            this.txt_Mes01_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Extr.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriod.DateTime.Month <= 2)
                                                            this.txt_Mes02_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Extr.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriod.DateTime.Month <= 3)
                                                            this.txt_Mes03_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Extr.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriod.DateTime.Month <= 4)
                                                            this.txt_Mes04_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Extr.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriod.DateTime.Month <= 5)
                                                            this.txt_Mes05_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Extr.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriod.DateTime.Month <= 6)
                                                            this.txt_Mes06_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Extr.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriod.DateTime.Month <= 7)
                                                            this.txt_Mes07_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Extr.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriod.DateTime.Month <= 8)
                                                            this.txt_Mes08_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Extr.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriod.DateTime.Month <= 9)
                                                            this.txt_Mes09_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Extr.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriod.DateTime.Month <= 10)
                                                            this.txt_Mes10_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Extr.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriod.DateTime.Month <= 11)
                                                            this.txt_Mes11_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Extr.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Extr.EditValue = Math.Round((this.detList[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                    }
                                                    else
                                                    {
                                                        this.txt_Mes01_Extr.EditValue = this.detList[index].ValorExt01.Value;
                                                        this.txt_Mes02_Extr.EditValue = this.detList[index].ValorExt02.Value;
                                                        this.txt_Mes03_Extr.EditValue = this.detList[index].ValorExt03.Value;
                                                        this.txt_Mes04_Extr.EditValue = this.detList[index].ValorExt04.Value;
                                                        this.txt_Mes05_Extr.EditValue = this.detList[index].ValorExt05.Value;
                                                        this.txt_Mes06_Extr.EditValue = this.detList[index].ValorExt06.Value;
                                                        this.txt_Mes07_Extr.EditValue = this.detList[index].ValorExt07.Value;
                                                        this.txt_Mes08_Extr.EditValue = this.detList[index].ValorExt08.Value;
                                                        this.txt_Mes09_Extr.EditValue = this.detList[index].ValorExt09.Value;
                                                        this.txt_Mes10_Extr.EditValue = this.detList[index].ValorExt10.Value;
                                                        this.txt_Mes11_Extr.EditValue = this.detList[index].ValorExt11.Value;
                                                        this.txt_Mes12_Extr.EditValue = this.detList[index].ValorExt12.Value;
                                                    }
                                                    #endregion
                                                    break;
                                            }
                                            onlyFija = false;
                                            validParticion = true;
                                        }
                                    }
                                    #endregion
                                }
                            }
                            else
                                validParticion = false; //retorna falso para indicar que no existe parametrizacion
                        }
                        //Si la Distribucion Variable esta activa pero no existe parametrizacion se realiza Fija
                        if (onlyFija)
                        {
                            //Distribucion Fijo Mensual
                            #region Asigna particion segun el Tipo de Moneda
                            decimal vlrMesExtr = 0;
                            decimal vlrMesLocal = 0;
                            switch (loadByMoneda)
                            {
                                case TipoMoneda.Local:
                                    #region Particion Mda Local
                                    vlrMesLocal = Math.Round(this.detList[index].VlrMvtoLocal.Value.Value / numMeses, 2);
                                    //Enero
                                    if (this.dtPeriod.DateTime.Month == 1)
                                        this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes01_Local.EditValue = "0";
                                    //Febrero
                                    if (this.dtPeriod.DateTime.Month <= 2)
                                        this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes02_Local.EditValue = "0";
                                    //Marzo
                                    if (this.dtPeriod.DateTime.Month <= 3)
                                        this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes03_Local.EditValue = "0";
                                    //Abril
                                    if (this.dtPeriod.DateTime.Month <= 4)
                                        this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes04_Local.EditValue = "0";
                                    //Mayo
                                    if (this.dtPeriod.DateTime.Month <= 5)
                                        this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes05_Local.EditValue = "0";
                                    //Junio
                                    if (this.dtPeriod.DateTime.Month <= 6)
                                        this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes06_Local.EditValue = "0";
                                    //Julio
                                    if (this.dtPeriod.DateTime.Month <= 7)
                                        this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes07_Local.EditValue = "0";
                                    //Agosto
                                    if (this.dtPeriod.DateTime.Month <= 8)
                                        this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes08_Local.EditValue = "0";
                                    //Septiembre
                                    if (this.dtPeriod.DateTime.Month <= 9)
                                        this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes09_Local.EditValue = "0";
                                    //Octubre
                                    if (this.dtPeriod.DateTime.Month <= 10)
                                        this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes10_Local.EditValue = "0";
                                    //Noviembre
                                    if (this.dtPeriod.DateTime.Month <= 11)
                                        this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                    else
                                        this.txt_Mes11_Local.EditValue = "0";
                                    //Diciembre
                                    this.txt_Mes12_Local.EditValue = vlrMesLocal;
                                    #endregion
                                    break;
                                case TipoMoneda.Foreign:
                                    #region Particion Mda Extr
                                    vlrMesExtr = Math.Round(this.detList[index].VlrMvtoExtr.Value.Value / numMeses, 2);

                                    //Enero
                                    if (this.dtPeriod.DateTime.Month == 1)
                                        this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes01_Extr.EditValue = "0";
                                    //Febrero
                                    if (this.dtPeriod.DateTime.Month <= 2)
                                        this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes02_Extr.EditValue = "0";
                                    //Marzo
                                    if (this.dtPeriod.DateTime.Month <= 3)
                                        this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes03_Extr.EditValue = "0";
                                    //Abril
                                    if (this.dtPeriod.DateTime.Month <= 4)
                                        this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes04_Extr.EditValue = "0";
                                    //Mayo
                                    if (this.dtPeriod.DateTime.Month <= 5)
                                        this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes05_Extr.EditValue = "0";
                                    //Junio
                                    if (this.dtPeriod.DateTime.Month <= 6)
                                        this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes06_Extr.EditValue = "0";
                                    //Julio
                                    if (this.dtPeriod.DateTime.Month <= 7)
                                        this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes07_Extr.EditValue = "0";
                                    //Agosto
                                    if (this.dtPeriod.DateTime.Month <= 8)
                                        this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes08_Extr.EditValue = "0";
                                    //Septiembre
                                    if (this.dtPeriod.DateTime.Month <= 9)
                                        this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes09_Extr.EditValue = "0";
                                    //Octubre
                                    if (this.dtPeriod.DateTime.Month <= 10)
                                        this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes10_Extr.EditValue = "0";
                                    //Noviembre
                                    if (this.dtPeriod.DateTime.Month <= 11)
                                        this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                    else
                                        this.txt_Mes11_Extr.EditValue = "0";
                                    //Diciembre
                                    this.txt_Mes12_Extr.EditValue = vlrMesExtr;
                                    #endregion
                                    break;
                                case TipoMoneda.Both:
                                    #region Particion Mda Local
                                    if (this.detList[index].LoadParticionLocalInd)
                                    {
                                        vlrMesLocal = Math.Round(this.detList[index].VlrMvtoLocal.Value.Value / numMeses, 2);

                                        //Enero
                                        if (this.dtPeriod.DateTime.Month == 1)
                                            this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes01_Local.EditValue = "0";
                                        //Febrero
                                        if (this.dtPeriod.DateTime.Month <= 2)
                                            this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes02_Local.EditValue = "0";
                                        //Marzo
                                        if (this.dtPeriod.DateTime.Month <= 3)
                                            this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes03_Local.EditValue = "0";
                                        //Abril
                                        if (this.dtPeriod.DateTime.Month <= 4)
                                            this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes04_Local.EditValue = "0";
                                        //Mayo
                                        if (this.dtPeriod.DateTime.Month <= 5)
                                            this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes05_Local.EditValue = "0";
                                        //Junio
                                        if (this.dtPeriod.DateTime.Month <= 6)
                                            this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes06_Local.EditValue = "0";
                                        //Julio
                                        if (this.dtPeriod.DateTime.Month <= 7)
                                            this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes07_Local.EditValue = "0";
                                        //Agosto
                                        if (this.dtPeriod.DateTime.Month <= 8)
                                            this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes08_Local.EditValue = "0";
                                        //Septiembre
                                        if (this.dtPeriod.DateTime.Month <= 9)
                                            this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes09_Local.EditValue = "0";
                                        //Octubre
                                        if (this.dtPeriod.DateTime.Month <= 10)
                                            this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes10_Local.EditValue = "0";
                                        //Noviembre
                                        if (this.dtPeriod.DateTime.Month <= 11)
                                            this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes11_Local.EditValue = "0";
                                        //Diciembre
                                        this.txt_Mes12_Local.EditValue = vlrMesLocal;

                                    }
                                    else
                                    {
                                        this.txt_Mes01_Local.EditValue = this.detList[index].ValorLoc01.Value;
                                        this.txt_Mes02_Local.EditValue = this.detList[index].ValorLoc02.Value;
                                        this.txt_Mes03_Local.EditValue = this.detList[index].ValorLoc03.Value;
                                        this.txt_Mes04_Local.EditValue = this.detList[index].ValorLoc04.Value;
                                        this.txt_Mes05_Local.EditValue = this.detList[index].ValorLoc05.Value;
                                        this.txt_Mes06_Local.EditValue = this.detList[index].ValorLoc06.Value;
                                        this.txt_Mes07_Local.EditValue = this.detList[index].ValorLoc07.Value;
                                        this.txt_Mes08_Local.EditValue = this.detList[index].ValorLoc08.Value;
                                        this.txt_Mes09_Local.EditValue = this.detList[index].ValorLoc09.Value;
                                        this.txt_Mes10_Local.EditValue = this.detList[index].ValorLoc10.Value;
                                        this.txt_Mes11_Local.EditValue = this.detList[index].ValorLoc11.Value;
                                        this.txt_Mes12_Local.EditValue = this.detList[index].ValorLoc12.Value;
                                    }
                                    #endregion
                                    #region Particion Mda Extr
                                    if (this.detList[index].LoadParticionExtrInd)
                                    {
                                        vlrMesExtr = Math.Round(this.detList[index].VlrMvtoExtr.Value.Value / numMeses, 2);
                                        //Enero
                                        if (this.dtPeriod.DateTime.Month == 1)
                                            this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes01_Extr.EditValue = "0";
                                        //Febrero
                                        if (this.dtPeriod.DateTime.Month <= 2)
                                            this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes02_Extr.EditValue = "0";
                                        //Marzo
                                        if (this.dtPeriod.DateTime.Month <= 3)
                                            this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes03_Extr.EditValue = "0";
                                        //Abril
                                        if (this.dtPeriod.DateTime.Month <= 4)
                                            this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes04_Extr.EditValue = "0";
                                        //Mayo
                                        if (this.dtPeriod.DateTime.Month <= 5)
                                            this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes05_Extr.EditValue = "0";
                                        //Junio
                                        if (this.dtPeriod.DateTime.Month <= 6)
                                            this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes06_Extr.EditValue = "0";
                                        //Julio
                                        if (this.dtPeriod.DateTime.Month <= 7)
                                            this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes07_Extr.EditValue = "0";
                                        //Agosto
                                        if (this.dtPeriod.DateTime.Month <= 8)
                                            this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes08_Extr.EditValue = "0";
                                        //Septiembre
                                        if (this.dtPeriod.DateTime.Month <= 9)
                                            this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes09_Extr.EditValue = "0";
                                        //Octubre
                                        if (this.dtPeriod.DateTime.Month <= 10)
                                            this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes10_Extr.EditValue = "0";
                                        //Noviembre
                                        if (this.dtPeriod.DateTime.Month <= 11)
                                            this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes11_Extr.EditValue = "0";
                                        //Diciembre
                                        this.txt_Mes12_Extr.EditValue = vlrMesExtr;

                                    }
                                    else
                                    {
                                        this.txt_Mes01_Extr.EditValue = this.detList[index].ValorExt01.Value;
                                        this.txt_Mes02_Extr.EditValue = this.detList[index].ValorExt02.Value;
                                        this.txt_Mes03_Extr.EditValue = this.detList[index].ValorExt03.Value;
                                        this.txt_Mes04_Extr.EditValue = this.detList[index].ValorExt04.Value;
                                        this.txt_Mes05_Extr.EditValue = this.detList[index].ValorExt05.Value;
                                        this.txt_Mes06_Extr.EditValue = this.detList[index].ValorExt06.Value;
                                        this.txt_Mes07_Extr.EditValue = this.detList[index].ValorExt07.Value;
                                        this.txt_Mes08_Extr.EditValue = this.detList[index].ValorExt08.Value;
                                        this.txt_Mes09_Extr.EditValue = this.detList[index].ValorExt09.Value;
                                        this.txt_Mes10_Extr.EditValue = this.detList[index].ValorExt10.Value;
                                        this.txt_Mes11_Extr.EditValue = this.detList[index].ValorExt11.Value;
                                        this.txt_Mes12_Extr.EditValue = this.detList[index].ValorExt12.Value;
                                    }
                                    #endregion
                                    break;
                            }

                            #endregion
                        }
                        #region Carga la info en el DTO
                        this.detList[index].ValorLoc00.Value = 0;
                        this.detList[index].ValorLoc01.Value = Convert.ToDecimal(this.txt_Mes01_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc02.Value = Convert.ToDecimal(this.txt_Mes02_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc03.Value = Convert.ToDecimal(this.txt_Mes03_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc04.Value = Convert.ToDecimal(this.txt_Mes04_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc05.Value = Convert.ToDecimal(this.txt_Mes05_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc06.Value = Convert.ToDecimal(this.txt_Mes06_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc07.Value = Convert.ToDecimal(this.txt_Mes07_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc08.Value = Convert.ToDecimal(this.txt_Mes08_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc09.Value = Convert.ToDecimal(this.txt_Mes09_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc10.Value = Convert.ToDecimal(this.txt_Mes10_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc11.Value = Convert.ToDecimal(this.txt_Mes11_Local.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorLoc12.Value = Convert.ToDecimal(this.txt_Mes12_Local.EditValue, CultureInfo.InvariantCulture);

                        this.detList[index].ValorExt00.Value = 0;
                        this.detList[index].ValorExt01.Value = Convert.ToDecimal(this.txt_Mes01_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt02.Value = Convert.ToDecimal(this.txt_Mes02_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt03.Value = Convert.ToDecimal(this.txt_Mes03_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt04.Value = Convert.ToDecimal(this.txt_Mes04_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt05.Value = Convert.ToDecimal(this.txt_Mes05_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt06.Value = Convert.ToDecimal(this.txt_Mes06_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt07.Value = Convert.ToDecimal(this.txt_Mes07_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt08.Value = Convert.ToDecimal(this.txt_Mes08_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt09.Value = Convert.ToDecimal(this.txt_Mes09_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt10.Value = Convert.ToDecimal(this.txt_Mes10_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt11.Value = Convert.ToDecimal(this.txt_Mes11_Extr.EditValue, CultureInfo.InvariantCulture);
                        this.detList[index].ValorExt12.Value = Convert.ToDecimal(this.txt_Mes12_Extr.EditValue, CultureInfo.InvariantCulture);

                        if (this.loadME && tc != 0)
                        {
                            this.detList[index].EquivExt00.Value = 0;
                            this.detList[index].EquivExt01.Value = this.detList[index].ValorLoc01.Value / tc;
                            this.detList[index].EquivExt02.Value = this.detList[index].ValorLoc02.Value / tc;
                            this.detList[index].EquivExt03.Value = this.detList[index].ValorLoc03.Value / tc;
                            this.detList[index].EquivExt04.Value = this.detList[index].ValorLoc04.Value / tc;
                            this.detList[index].EquivExt05.Value = this.detList[index].ValorLoc05.Value / tc;
                            this.detList[index].EquivExt06.Value = this.detList[index].ValorLoc06.Value / tc;
                            this.detList[index].EquivExt07.Value = this.detList[index].ValorLoc07.Value / tc;
                            this.detList[index].EquivExt08.Value = this.detList[index].ValorLoc08.Value / tc;
                            this.detList[index].EquivExt09.Value = this.detList[index].ValorLoc09.Value / tc;
                            this.detList[index].EquivExt10.Value = this.detList[index].ValorLoc10.Value / tc;
                            this.detList[index].EquivExt11.Value = this.detList[index].ValorLoc11.Value / tc;
                            this.detList[index].EquivExt12.Value = this.detList[index].ValorLoc12.Value / tc;

                            this.detList[index].EquivLoc00.Value = 0;
                            this.detList[index].EquivLoc01.Value = this.detList[index].ValorExt01.Value * tc;
                            this.detList[index].EquivLoc02.Value = this.detList[index].ValorExt02.Value * tc;
                            this.detList[index].EquivLoc03.Value = this.detList[index].ValorExt03.Value * tc;
                            this.detList[index].EquivLoc04.Value = this.detList[index].ValorExt04.Value * tc;
                            this.detList[index].EquivLoc05.Value = this.detList[index].ValorExt05.Value * tc;
                            this.detList[index].EquivLoc06.Value = this.detList[index].ValorExt06.Value * tc;
                            this.detList[index].EquivLoc07.Value = this.detList[index].ValorExt07.Value * tc;
                            this.detList[index].EquivLoc08.Value = this.detList[index].ValorExt08.Value * tc;
                            this.detList[index].EquivLoc09.Value = this.detList[index].ValorExt09.Value * tc;
                            this.detList[index].EquivLoc10.Value = this.detList[index].ValorExt10.Value * tc;
                            this.detList[index].EquivLoc11.Value = this.detList[index].ValorExt11.Value * tc;
                            this.detList[index].EquivLoc12.Value = this.detList[index].ValorExt12.Value * tc;
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "LoadParticiones"));
            }
        }

        /// <summary>
        /// Carga porcentajes por mes
        /// </summary>
        /// <param name="lineaPresup">Linea Presupuesto</param>
        /// <param name="indexRow">fila a validar</param>
        protected virtual void LoadPorcent(string lineaPresup, int indexRow)
        {           
            if (this.actividadLineaPresupInd)
                validParticion = false; //retorna falso para indicar que no existe parametrizacion
            else
            {
                #region Asigna porcentajes particion fija
                //Porcentaje de Alicuota
                decimal PorcFijo = (100 / (decimal)12);
                #region Calcula % de Ajuste para que la distribucion siempre sea del 100% sin importar el periodo actual
                int periodo = this.dtPeriod.DateTime.Month;
                decimal porcentajeAjuste = 0;
                switch (periodo)
                {
                    case 1: //Enero
                        porcentajeAjuste = 100 / 100;
                        break;
                    case 2: //Febrero
                        porcentajeAjuste = 100 / (PorcFijo * 11);
                        break;
                    case 3: //Marzo
                        porcentajeAjuste = 100 / (PorcFijo * 10);
                        break;
                    case 4: //Abril
                        porcentajeAjuste = 100 / (PorcFijo * 9);
                        break;
                    case 5: //Mayo
                        porcentajeAjuste = 100 / (PorcFijo * 8);
                        break;
                    case 6: //Junio
                        porcentajeAjuste = 100 / (PorcFijo * 7);
                        break;
                    case 7: //Julio
                        porcentajeAjuste = 100 / (PorcFijo * 6);
                        break;
                    case 8: //Agosto
                        porcentajeAjuste = 100 / (PorcFijo * 5);
                        break;
                    case 9: //Septiembre
                        porcentajeAjuste = 100 / (PorcFijo * 4);
                        break;
                    case 10: //Octubre
                        porcentajeAjuste = 100 / (PorcFijo * 3);
                        break;
                    case 11: //Noviembre
                        porcentajeAjuste = 100 / (PorcFijo * 2);
                        break;
                    case 12: //Diciembre
                        porcentajeAjuste = 100 / (PorcFijo);
                        break;
                }
                #endregion

                this.detList[indexRow].Porcentaje01.Value = periodo <= 1 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje02.Value = periodo <= 2 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje03.Value = periodo <= 3 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje04.Value = periodo <= 4 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje05.Value = periodo <= 5 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje06.Value = periodo <= 6 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje07.Value = periodo <= 7 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje08.Value = periodo <= 8 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje09.Value = periodo <= 9 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje10.Value = periodo <= 10 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje11.Value = periodo <= 11 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                this.detList[indexRow].Porcentaje12.Value = periodo <= 12 ? Math.Round(PorcFijo * porcentajeAjuste, 3) : 0;
                #endregion
            }
        }

        /// <summary>
        /// Carga la info del footer 
        /// </summary>
        protected virtual void LoadFooter(int index)
        {
            try
            {
                if (this.detList.Count > 0 && !this.initData)
                {
                    DTO_plPresupuestoDeta det = this.detList[index];

                    this.txt_Mes01_Local.EditValue = det.ValorLoc01.Value.Value;
                    this.txt_Mes02_Local.EditValue = det.ValorLoc02.Value.Value;
                    this.txt_Mes03_Local.EditValue = det.ValorLoc03.Value.Value;
                    this.txt_Mes04_Local.EditValue = det.ValorLoc04.Value.Value;
                    this.txt_Mes05_Local.EditValue = det.ValorLoc05.Value.Value;
                    this.txt_Mes06_Local.EditValue = det.ValorLoc06.Value.Value;
                    this.txt_Mes07_Local.EditValue = det.ValorLoc07.Value.Value;
                    this.txt_Mes08_Local.EditValue = det.ValorLoc08.Value.Value;
                    this.txt_Mes09_Local.EditValue = det.ValorLoc09.Value.Value;
                    this.txt_Mes10_Local.EditValue = det.ValorLoc10.Value.Value;
                    this.txt_Mes11_Local.EditValue = det.ValorLoc11.Value.Value;
                    this.txt_Mes12_Local.EditValue = det.ValorLoc12.Value.Value;

                    this.txt_Mes01_Extr.EditValue = det.ValorExt01.Value.Value;
                    this.txt_Mes02_Extr.EditValue = det.ValorExt02.Value.Value;
                    this.txt_Mes03_Extr.EditValue = det.ValorExt03.Value.Value;
                    this.txt_Mes04_Extr.EditValue = det.ValorExt04.Value.Value;
                    this.txt_Mes05_Extr.EditValue = det.ValorExt05.Value.Value;
                    this.txt_Mes06_Extr.EditValue = det.ValorExt06.Value.Value;
                    this.txt_Mes07_Extr.EditValue = det.ValorExt07.Value.Value;
                    this.txt_Mes08_Extr.EditValue = det.ValorExt08.Value.Value;
                    this.txt_Mes09_Extr.EditValue = det.ValorExt09.Value.Value;
                    this.txt_Mes10_Extr.EditValue = det.ValorExt10.Value.Value;
                    this.txt_Mes11_Extr.EditValue = det.ValorExt11.Value.Value;
                    this.txt_Mes12_Extr.EditValue = det.ValorExt12.Value.Value;
                }
                else
                    this.LoadParticiones(this.gvDetail.FocusedRowHandle, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "LoadFooter"));
            }
        }

        /// <summary>
        /// Trae la tasa de cambio de acuerdo al tipo  de proyecto
        /// </summary>
        protected void LoadTasaCambio()
        {
            //Dictionary<string, string> pks = new Dictionary<string, string>();
            //pks.Add("PeriodoID", this.dtPeriod.DateTime.ToShortDateString());
            //pks.Add("ContratoID", this.masterContrato.Value);
            //pks.Add("Campo", this.masterCampo.Value);
            //DTO_plTasasPresupuesto tasas = (DTO_plTasasPresupuesto)_bc.GetMasterComplexDTO(AppMasters.plTasasPresupuesto, pks, true);

            //if (Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Capex || Convert.ToByte(this.cmbTipoProyecto.EditValue) == (byte)ProyectoTipo.Inversion)
            //    this.txtTasaCambio.EditValue = tasas != null ? tasas.TRMCapex.Value : 0;
            //else
            //    this.txtTasaCambio.EditValue = tasas != null ? tasas.TRMOpex.Value : 0;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "ShowFKModal"));
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
        protected virtual void ValidateDataImport(DTO_plPresupuestoDeta presupDet, DTO_TxResultDetail rd)
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
            if (this.detList.Count > 0)
            {
                int count = this.detList.Where(x => x.CentroCostoID.Value == presupDet.CentroCostoID.Value &&
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
            if (presupDet.VlrMvtoLocal.Value == 0 && presupDet.VlrMvtoExtr.Value == 0)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx + "-" + colRsxExtr;
                rdF.Message = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), rdF.Field);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region VlrNuevoSaldoLocal - VlrNuevoSaldoExtr
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNuevoSaldoLocal");
            string colRsxExtrSaldo = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNuevoSaldoExtr");
            if (presupDet.VlrNuevoSaldoLoc.Value < 0 || presupDet.VlrNuevoSaldoExtr.Value < 0)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx + "-" + colRsxExtrSaldo;
                rdF.Message = DictionaryMessages.pl_newValueInvalid;
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
            if (createDTO)
            {
                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, presupDet.LineaPresupuestoID.Value, true);
                presupDet.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, presupDet.CentroCostoID.Value, true);
                presupDet.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
            }
            #endregion

        }

        /// <summary>
        /// Obtiene los filtros para las maestras del encabezado y la grilla
        /// </summary>
        protected void GetFiltersMasters()
        {
          
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
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable", "masterProyecto_Leave"));
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
                if (this.masterProyecto.Value != this.proyectoID)
                    this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// ento al salir de un control de valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txt_Mes_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal tc = 0;// Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);
                int fila = this.gvDetail.FocusedRowHandle;
                TextEdit ctrl = (TextEdit)sender;
                decimal value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                if (fila < 0 || this.detList.Count == 0)
                    return;
                switch (ctrl.Name)
                {
                    #region Mda Local
                    case "txt_Mes00_Local":
                        this.detList[fila].ValorLoc00.Value = value;
                        this.detList[fila].EquivExt00.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc00.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes01_Local":
                        this.detList[fila].ValorLoc01.Value = value;
                        this.detList[fila].EquivExt01.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc01.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes02_Local":
                        this.detList[fila].ValorLoc02.Value = value;
                        this.detList[fila].EquivExt02.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc02.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes03_Local":
                        this.detList[fila].ValorLoc03.Value = value;
                        this.detList[fila].EquivExt03.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc03.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes04_Local":
                        this.detList[fila].ValorLoc04.Value = value;
                        this.detList[fila].EquivExt04.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc04.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes05_Local":
                        this.detList[fila].ValorLoc05.Value = value;
                        this.detList[fila].EquivExt05.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc05.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes06_Local":
                        this.detList[fila].ValorLoc06.Value = value;
                        this.detList[fila].EquivExt06.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc06.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes07_Local":
                        this.detList[fila].ValorLoc07.Value = value;
                        this.detList[fila].EquivExt07.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc07.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes08_Local":
                        this.detList[fila].ValorLoc08.Value = value;
                        this.detList[fila].EquivExt08.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc08.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes09_Local":
                        this.detList[fila].ValorLoc09.Value = value;
                        this.detList[fila].EquivExt09.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc09.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes10_Local":
                        this.detList[fila].ValorLoc10.Value = value;
                        this.detList[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc10.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes11_Local":
                        this.detList[fila].ValorLoc11.Value = value;
                        this.detList[fila].EquivExt11.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc11.Value.Value / tc, 2) : 0;
                        break;
                    case "txt_Mes12_Local":
                        this.detList[fila].ValorLoc12.Value = value;
                        this.detList[fila].EquivExt12.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorLoc12.Value.Value / tc, 2) : 0;
                        break;
                    #endregion
                    #region Mda Extranjera
                    case "txt_Mes00_Extr":
                        this.detList[fila].ValorExt00.Value = value;
                        this.detList[fila].EquivLoc00.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt00.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes01_Extr":
                        this.detList[fila].ValorExt01.Value = value;
                        this.detList[fila].EquivLoc01.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt01.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes02_Extr":
                        this.detList[fila].ValorExt02.Value = value;
                        this.detList[fila].EquivLoc02.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt02.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes03_Extr":
                        this.detList[fila].ValorExt03.Value = value;
                        this.detList[fila].EquivLoc03.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt03.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes04_Extr":
                        this.detList[fila].ValorExt04.Value = value;
                        this.detList[fila].EquivLoc04.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt04.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes05_Extr":
                        this.detList[fila].ValorExt05.Value = value;
                        this.detList[fila].EquivLoc05.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt05.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes06_Extr":
                        this.detList[fila].ValorExt06.Value = value;
                        this.detList[fila].EquivLoc06.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt06.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes07_Extr":
                        this.detList[fila].ValorExt07.Value = value;
                        this.detList[fila].EquivLoc07.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt07.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes08_Extr":
                        this.detList[fila].ValorExt08.Value = value;
                        this.detList[fila].EquivLoc08.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt08.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes09_Extr":
                        this.detList[fila].ValorExt09.Value = value;
                        this.detList[fila].EquivLoc09.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt09.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes10_Extr":
                        this.detList[fila].ValorExt10.Value = value;
                        this.detList[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt10.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes11_Extr":
                        this.detList[fila].ValorExt11.Value = value;
                        this.detList[fila].EquivLoc11.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt11.Value.Value * tc) : 0;
                        break;
                    case "txt_Mes12_Extr":
                        this.detList[fila].ValorExt12.Value = value;
                        this.detList[fila].EquivLoc12.Value = this.loadME && tc != 0 ? Math.Round(this.detList[fila].ValorExt12.Value.Value * tc) : 0;
                        break;
                    #endregion
                }

                //Calcula el Total del movimiento(Local)
                this.detList.ForEach(x => x.VlrMvtoLocal.Value = Math.Round(x.ValorLoc00.Value.Value + x.ValorLoc01.Value.Value + x.ValorLoc02.Value.Value + x.ValorLoc03.Value.Value
                                          + x.ValorLoc04.Value.Value + x.ValorLoc05.Value.Value + x.ValorLoc06.Value.Value + x.ValorLoc07.Value.Value + x.ValorLoc08.Value.Value
                                          + x.ValorLoc09.Value.Value + x.ValorLoc10.Value.Value + x.ValorLoc11.Value.Value + x.ValorLoc12.Value.Value));
                //Calcula el Total del movimiento(Extr)
                if (this.loadME)
                {
                    this.detList.ForEach(x => x.VlrMvtoExtr.Value = Math.Round(x.ValorExt00.Value.Value + x.ValorExt01.Value.Value + x.ValorExt02.Value.Value + x.ValorExt03.Value.Value
                                            + x.ValorExt04.Value.Value + x.ValorExt05.Value.Value + x.ValorExt06.Value.Value + x.ValorExt07.Value.Value + x.ValorExt08.Value.Value
                                            + x.ValorExt09.Value.Value + x.ValorExt10.Value.Value + x.ValorExt11.Value.Value + x.ValorExt12.Value.Value));
                }
                foreach (var item in this.detList)
                {
                    item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                    item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                    item.LoadParticionLocalInd = item.VlrMvtoLocal.Value != 0 ? false : true;
                    item.LoadParticionExtrInd = item.VlrMvtoExtr.Value != 0 ? false : true;
                }


                this.gcDetail.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable", "txt_Mes_Leave"));
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
                string fieldName = this.gvDetail.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "DescripTExt")
                    this.richEditControl.Document.Text = this.gvDetail.GetFocusedRowCellValue(fieldName).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "riPopup_QueryPopUp"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable", "btnNew_Click"));
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
                #region Filtra los proyectos
                filtrosProyecto.RemoveAll(x => x.CampoFisico.Equals("ProyectoTipo"));
                filtrosProyecto.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ProyectoTipo",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = ((byte)ProyectoTipo.Administracion).ToString()
                });
                    this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                    #endregion
                this.gvDetail.RefreshData();
                this.LoadTasaCambio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "cmbProyectoTipo_EditValueChanged"));
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
                string colName = this.gvDetail.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDetail.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "editBtn_Doc_ButtonClick"));
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
                if (this.validHeader)
                {
                    this.EnableControls(false);
                    this.LoadParticiones(this.gvDetail.FocusedRowHandle, true);
                    this.EnableFooter(true);
                }
                //else if (this.masterProyecto.ValidID)
                //{
                //    this.validHeader = true;
                //    //this.EnableForm(false);
                //}            
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "gcDetail_Enter"));
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
                    this.gvDetail.PostEditor();

                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                    {
                        #region Nuevo registro
                        if (this.gvDetail.ActiveFilterString != string.Empty)
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
                            int fila = this.gvDetail.FocusedRowHandle;
                            this.gvDetail.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detList[fila].NewRowPresup;
                            this.gvDetail.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detList[fila].NewRowPresup;
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
                            int rowHandle = this.gvDetail.FocusedRowHandle;

                            if (this.detList.Count == 1)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                e.Handled = true;
                            }
                            else
                            {
                                this.detList.RemoveAt(rowHandle);
                                //Si borra el primer registro
                                if (rowHandle == 0)
                                    this.gvDetail.FocusedRowHandle = 0;
                                //Si selecciona el ultimo
                                else
                                    this.gvDetail.FocusedRowHandle = rowHandle - 1;

                                this.gvDetail.RefreshData();
                                this.RowIndexChanged_Det(this.gvDetail.FocusedRowHandle);
                            }
                        }
                        e.Handled = true;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "gcDetail_EmbeddedNavigator_ButtonClick"));
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
                int indexRow = this.gvDetail.FocusedRowHandle;

                #region FKs

                if (fieldName == "LineaPresupuestoID")
                {
                    validField = _bc.ValidGridCell(this.gvDetail, string.Empty, e.RowHandle, fieldName, false, false, false, AppMasters.plLineaPresupuesto);
                    if (validField)
                        this.LoadPorcent(e.Value.ToString(), indexRow);
                }
                else if (fieldName == "CentroCostoID")
                {
                    validField = _bc.ValidGridCell(this.gvDetail, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                    DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, e.Value.ToString(), true);
                    this.detList[indexRow].CentroCostoDesc.Value = validField ? centroCto.Descriptivo.Value : string.Empty;
                }
                #endregion
                #region Valores

                //Movimiento Mda Local
                if (fieldName == "VlrMvtoLocal")
                {
                    this.detList[indexRow].VlrMvtoLocal.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                    this.detList[indexRow].VlrNuevoSaldoLoc.Value = this.detList[indexRow].VlrSaldoAntLoc.Value + this.detList[indexRow].VlrMvtoLocal.Value;
                    this.detList[indexRow].LoadParticionLocalInd = this.detList[indexRow].VlrMvtoLocal.Value != 0 ? false : true;

                    validField = _bc.ValidGridCellValue(this.gvDetail, string.Empty, e.RowHandle, fieldName, false, this.detList[indexRow].VlrMvtoExtr.Value == 0 &&
                                 this.documentID == AppDocuments.PresupuestoContable ? false : true, this.documentID == AppDocuments.ReclasifPresupuesto ? false : true, false);
                    if (validField)
                    {
                        this.LoadParticiones(this.gvDetail.FocusedRowHandle, false, TipoMoneda.Local);
                        this.LoadPorcent(this.detList[indexRow].LineaPresupuestoID.Value, indexRow);
                        this.EnableControls(false);
                        this.EnableFooter(true);
                        this.gcDetail.RefreshDataSource();
                    }
                }
                //Movimiento Mda Extr
                if (fieldName == "VlrMvtoExtr")
                {
                    this.detList[indexRow].VlrMvtoExtr.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                    this.detList[indexRow].VlrNuevoSaldoExtr.Value = this.detList[indexRow].VlrSaldoAntExtr.Value + this.detList[indexRow].VlrMvtoExtr.Value;
                    this.detList[indexRow].LoadParticionExtrInd = this.detList[indexRow].VlrMvtoExtr.Value != 0 ? false : true;

                    validField = _bc.ValidGridCellValue(this.gvDetail, string.Empty, e.RowHandle, fieldName, false, this.detList[indexRow].VlrMvtoLocal.Value == 0 &&
                                this.documentID == AppDocuments.PresupuestoContable ? false : true, this.documentID == AppDocuments.ReclasifPresupuesto ? false : true, false);
                    if (validField)
                    {
                        this.LoadParticiones(this.gvDetail.FocusedRowHandle, false, TipoMoneda.Foreign);
                        this.LoadPorcent(this.detList[indexRow].LineaPresupuestoID.Value, indexRow);
                        this.EnableControls(false);
                        this.EnableFooter(true);
                        this.gcDetail.RefreshDataSource();
                    }
                }


                #endregion
                if (!validField)
                    this.isValid_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Se realiza cuando se digita una tecla teniendo en cuenta la columna
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string colName = this.gvDetail.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                if (this.gvDetail.DataRowCount > 0 && this.gvDetail.IsLastRow && colName.Equals("DescripTExt") &&
                    e.KeyCode == Keys.Tab && this.documentID == AppDocuments.PresupuestoContable)
                {
                    bool isV = this.ValidateRow_Det();
                    if (isV)
                        this.AddNewRow_Det();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "gvDetail_KeyUp"));
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
            this.RowIndexChanged_Det(e.FocusedRowHandle);
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
                string colName = this.gvDetail.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                this.ShowFKModal(this.gvDetail.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "editBtn_Det_ButtonClick"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
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
                FormProvider.Master.itemImport.Visible = true;
                if (this.documentID != AppDocuments.PresupuestoContable)
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PresupuestoContable.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PresupuestoContable.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "PresupuestoContable.cs-Form_FormClosed"));
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
                this.detList = new List<DTO_plPresupuestoDeta>();
                this.EnableControls(true);
                this.validHeader = false;
                this.initData = false;
                this.deleteOP = false;
                this.isValid_Det = true;
                this.disableValidate_Det = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para eliminar(anular) un presupuesto
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgDelDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Document);

                if (MessageBox.Show(msgDelDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.gvDetail.ActiveFilterString = string.Empty;


                    List<int> numDocSelected = new List<int>();
                    numDocSelected.Add(this.presupuesto.DocCtrl.NumeroDoc.Value.Value);

                    DTO_TxResult result = _bc.AdministrationModel.glDocumentoControl_Anular(this.documentID, numDocSelected);
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();

                    if (result.Result == ResultValue.OK)
                        this.EnableControls(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para guardar
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDetail.PostEditor();
                this.gvDetail.Focus();

                if (this.detList.Count > 0)
                {

                    bool isValid = this.ValidateRow_Det();
                    if (isValid)
                    {
                        decimal vlrMvtoLocal = this.detList.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtr = this.detList.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (vlrMvtoLocal != 0 || vlrMvtoExtr != 0)
                        {
                            if (this.masterProyecto.ValidID)
                            {
                                Thread process = new Thread(this.SaveThread);
                                process.Start();
                            }
                            else
                            {
                                string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                                MessageBox.Show(string.Format(msg, this.masterProyecto.LabelRsx, this.masterProyecto.Value));
                                this.masterProyecto.Focus();
                            }                           
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaveMvtoInvalid));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this.gvDetail.PostEditor();
                this.gvDetail.Focus();
                if (this.detList.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    if (isValid)
                    {
                        decimal vlrMvtoLocal = this.detList.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtr = this.detList.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (vlrMvtoLocal != 0 || vlrMvtoExtr != 0)
                        {
                            Thread process = new Thread(this.SendToApproveThread);
                            process.Start();
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaveMvtoInvalid));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "TBSendtoAppr"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            this.gvDetail.ActiveFilterString = string.Empty;
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

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;
                    switch (fileExtenstion)
                    {
                        case ".xls":
                            this.gvDetail.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            this.gvDetail.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            this.gvDetail.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            this.gvDetail.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            this.gvDetail.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            this.gvDetail.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                }
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

                this.gvDetail.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                //FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                //ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                //FormProvider.Master.ProgressBarThread = new Thread(pth);
                //FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = null;
                decimal tc = 0;// Convert.ToDecimal(this.txtTasaCambio.EditValue, CultureInfo.InvariantCulture);

                //if (this.presupuesto == null)
                //    this.presupuesto = new DTO_Presupuesto();
                //this.presupuesto.Detalles = this.detList;
                //this.presupuesto.NumeroDocPresup.Value = this.numeroDocPresup;
                //obj = _bc.AdministrationModel.Presupuesto_Nuevo(AppDocuments.Presupuesto, this.dtPeriod.DateTime, this.proyectoID, tc, this.presupuesto, true);

                //FormProvider.Master.StopProgressBarThread(this.documentID);

                //bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this.actFlujo.seUsuarioID.Value, obj, true, true);
                //if (isOK)
                //{
                //    this.Invoke(this.saveDelegate);
                //    if (obj.GetType() == typeof(DTO_Alarma))
                //    {
                //        DTO_Alarma alarma = (DTO_Alarma)obj;
                //        this.numeroDocPresup = Convert.ToInt32(alarma.NumeroDoc);
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "SendToApproveThread"));
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

                this.gvDetail.ActiveFilterString = string.Empty;

                //FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                //FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                //ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                //FormProvider.Master.ProgressBarThread = new Thread(pth);
                //FormProvider.Master.ProgressBarThread.Start(this.documentID);

                //object obj = _bc.AdministrationModel.PresupuestoContable_Aprobar(AppDocuments.Presupuesto,this.numeroDocPresup, this.dtPeriod.DateTime);

                FormProvider.Master.StopProgressBarThread(this.documentID);
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PresupuestoContable.cs", "SendToApproveThread"));
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
                    DTO_plPresupuestoDeta presupuestoDet = null;
                    this.detList = new List<DTO_plPresupuestoDeta>();
                    bool createDTO = true;
                    bool validList = true;

                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_plPresupuestoDeta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PresupuestoContable + "_" + pi.Name);
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
                            presupuestoDet = new DTO_plPresupuestoDeta(true);
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
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "PresupuestoContable.cs - Creacion de DTO y validacion Formatos");
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
                                this.detList.Add(presupuestoDet);
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

                        for (int index = 0; index < this.detList.Count; ++index)
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
                            percent = ((i + 1) * 100) / (this.detList.Count);
                            i++;
                            #endregion
                            presupuestoDet = this.detList[index];
                            #region Valida y Asigna Totales Ambas Monedas
                            //Total del movimiento(Local)
                            decimal TotalMdaLocal = Math.Round(presupuestoDet.ValorLoc00.Value.Value + presupuestoDet.ValorLoc01.Value.Value + presupuestoDet.ValorLoc02.Value.Value + presupuestoDet.ValorLoc03.Value.Value
                                                      + presupuestoDet.ValorLoc04.Value.Value + presupuestoDet.ValorLoc05.Value.Value + presupuestoDet.ValorLoc06.Value.Value + presupuestoDet.ValorLoc07.Value.Value + presupuestoDet.ValorLoc08.Value.Value
                                                      + presupuestoDet.ValorLoc09.Value.Value + presupuestoDet.ValorLoc10.Value.Value + presupuestoDet.ValorLoc11.Value.Value + presupuestoDet.ValorLoc12.Value.Value);
                            if (TotalMdaLocal != 0)
                            {
                                presupuestoDet.VlrMvtoLocal.Value = TotalMdaLocal;
                                presupuestoDet.LoadParticionLocalInd = false;
                            }

                            if (this.loadME)
                            {
                                //Total del movimiento(Extr)
                                decimal TotalMdaExtr = Math.Round(presupuestoDet.ValorExt00.Value.Value + presupuestoDet.ValorExt01.Value.Value + presupuestoDet.ValorExt02.Value.Value + presupuestoDet.ValorExt03.Value.Value
                                                        + presupuestoDet.ValorExt04.Value.Value + presupuestoDet.ValorExt05.Value.Value + presupuestoDet.ValorExt06.Value.Value + presupuestoDet.ValorExt07.Value.Value + presupuestoDet.ValorExt08.Value.Value
                                                        + presupuestoDet.ValorExt09.Value.Value + presupuestoDet.ValorExt10.Value.Value + presupuestoDet.ValorExt11.Value.Value + presupuestoDet.ValorExt12.Value.Value);
                                if (TotalMdaExtr != 0)
                                {
                                    presupuestoDet.VlrMvtoExtr.Value = TotalMdaExtr;
                                    presupuestoDet.LoadParticionExtrInd = false;
                                }
                            }

                            presupuestoDet.VlrNuevoSaldoLoc.Value = presupuestoDet.VlrSaldoAntLoc.Value + presupuestoDet.VlrMvtoLocal.Value;
                            presupuestoDet.VlrNuevoSaldoExtr.Value = presupuestoDet.VlrSaldoAntExtr.Value + presupuestoDet.VlrMvtoExtr.Value;
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
                        this.detList = new List<DTO_plPresupuestoDeta>();
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
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp-DocumentAuxiliarForm.cs", "ImportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
