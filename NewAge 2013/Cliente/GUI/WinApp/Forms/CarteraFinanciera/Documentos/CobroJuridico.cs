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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CobroJuridico : FormWithToolbar
    {
        #region Delegados

        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private void RefreshGridMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_ccCreditoDocu> cobrosJuridicos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> cobrosJuridicosTemp = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccSaldosComponentes> saldosComponentes = new List<DTO_ccSaldosComponentes>();
        private DTO_ccCreditoDocu _currentCredito = new DTO_ccCreditoDocu();
        //Variables privadas
        private string clienteID = String.Empty;
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private bool multiMoneda;
        private ModulesPrefix frmModule;
        private List<int> select = new List<int>();
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        private bool isValid = true;
        private byte _claseDeuda = 1;
        private string _compCapital = string.Empty;
        private string _compSeguro = string.Empty;

        private string tipoMvto = string.Empty;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private string unboundPrefix = "Unbound_";

        string polizaRsx = string.Empty;
        string polizaIntRsx = string.Empty;
        string polizaSaldoRsx = string.Empty;
        string polizaSaldoIntRsx =string.Empty;
        string capitalRsx = string.Empty;
        string capitalIntRsx = string.Empty;
        string capitalSaldoRsx = string.Empty;
        string capitalSaldoIntRsx = string.Empty;

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CobroJuridico()
        {
            this.Constructor();
            //this.dtFechaMvto
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CobroJuridico(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Funcion que llama el Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this.SetInitParameters();
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
            this.frmModule = ModulesPrefix.cf;

            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            this.refreshGridDelegate = new RefreshGrid(RefreshGridMethod);

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

            this.dtFechaEC.ReadOnly = true;
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterCliente.Value = String.Empty;

            //Variables
            this.clienteID = string.Empty;
            this.cobrosJuridicos = new List<DTO_ccCreditoDocu>();
            this.cobrosJuridicosTemp = new List<DTO_ccCreditoDocu>();
            this.gcDocument.DataSource = this.cobrosJuridicosTemp;
            this.gvDocument.RefreshData();
            this.gcHistoria.DataSource = null;
            this.gvHistoria.RefreshData();

            this.cmbEstadoActual.EditValue = "";
            this.cmbTipoMov.EditValue = "";
            this.cmbFiltro.EditValue = "";
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.CobroJuridico;
                this.frmModule = ModulesPrefix.cc;

                #region Info del header superior
                this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);

                this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                this.txtDocumentoID.Text = this.documentID.ToString();
                this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                this.txtNumeroDoc.Text = "0";

                this._compCapital = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                this._compSeguro = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);

                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.dtPeriod.DateTime = Convert.ToDateTime(periodoStr);
                this.dtPeriod.Enabled = false;
                #endregion
                #region Inicia los controles y las grillas
                //Carga la grilla con las columnas
                this.AddGridCols();

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);

                //llena Combos
                this.cmbEstadoActual.Properties.ReadOnly = true;
                #endregion
                #region Carga los diccionarios para los ddl

                //Estado de cartera cliente
                Dictionary<string, string> dicEstado = new Dictionary<string, string>();
                dicEstado.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_Normal"));
                dicEstado.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_Mora"));
                dicEstado.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_Prejuridico"));
                dicEstado.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_CobroJuridico"));
                dicEstado.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoPago"));
                dicEstado.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoIncumplido"));
                dicEstado.Add("7", this._bc.GetResource(LanguageTypes.Tables, "tbl_Castigada"));
                this.cmbEstadoActual.Properties.DataSource = dicEstado;

                //Clases 
                Dictionary<string, string> dicClaseDeuda = new Dictionary<string, string>();
                dicClaseDeuda.Add(((byte)ClaseDeuda.Principal).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_Principal"));
                dicClaseDeuda.Add(((byte)ClaseDeuda.Adicional).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_Adicional"));
                dicClaseDeuda.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Ambos"));
                dicClaseDeuda.Add("4", this._bc.GetResource(LanguageTypes.Tables, "Juzgado"));
                this.cmbClaseDeuda.Properties.DataSource = dicClaseDeuda;
                this.cmbClaseDeuda.EditValue = "1";

                //Filtros
                Dictionary<string, string> dicFiltro = new Dictionary<string, string>();
                dicFiltro.Add(string.Empty, string.Empty);
                dicFiltro.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoActual"));
                dicFiltro.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_Historia"));
                this.cmbFiltro.Properties.DataSource = dicFiltro;
                this.cmbFiltro.EditValue = "0";

                //Inicia los controles
                this.btnAbonos.Visible = false;
                this.lblFiltro.Visible = false;
                this.cmbFiltro.Visible = false;

                #endregion
                #region Estable la fecha con base a la fecha del periodo y fecha ultimo dia Cierre
                bool cierreValido = true;
                int diaCierre = 1;
                DateTime periodo = this.dtPeriod.DateTime.Date;
                string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
                if (indCierreDiaStr == "1")
                {
                    string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                    if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                        diaCierreStr = "1";

                    diaCierre = Convert.ToInt16(diaCierreStr);
                    if (diaCierre > DateTime.DaysInMonth(periodo.Year, periodo.Month))
                    {
                        cierreValido = false;
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));

                        this.masterCliente.EnableControl(false);
                        this.dtFechaDoc.Enabled = false;

                        FormProvider.Master.itemNew.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;
                        FormProvider.Master.itemPrint.Enabled = false;
                    }
                }               
                #endregion
                #region Revisa si el cierre es válido
                if (cierreValido)
                {
                    //this.dtFechaDoc.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                    //this.dtFechaDoc.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    this.dtFechaDoc.DateTime = new DateTime(periodo.Year, periodo.Month, diaCierre);

                }
                else
                {
                    //this.dtFechaDoc.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                    //this.dtFechaDoc.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    if (DateTime.Now.Month != periodo.Month)                     
                        this.dtFechaDoc.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    else                    
                        this.dtFechaDoc.DateTime = DateTime.Now;
                }
                #endregion
                #region Recursos
                this.polizaRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPoliza");
                this.polizaIntRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntPoliza");
                this.polizaSaldoRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoPolizaHist");
                this.polizaSaldoIntRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoIntPolizaHist");
                this.capitalRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCapital");
                this.capitalIntRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntCapital");
                this.capitalSaldoRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoCapitalHist");
                this.capitalSaldoIntRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoIntCapitalHist");
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Agrega Columnas Grilla Principal

                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 50;
                aprob.Visible = false;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(aprob);

                GridColumn editable = new GridColumn();
                editable.FieldName = this.unboundPrefix + "Editable";
                editable.Caption = "√";
                editable.UnboundType = UnboundColumnType.Boolean;
                editable.VisibleIndex = 0;
                editable.Width = 50;
                editable.Visible = true;
                editable.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Editable");
                editable.AppearanceHeader.ForeColor = Color.Lime;
                editable.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                editable.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                editable.AppearanceHeader.Options.UseTextOptions = true;
                editable.AppearanceHeader.Options.UseFont = true;
                editable.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(editable);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 1;
                libranza.Width = 60;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Fecha
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this.unboundPrefix + "FechaVto";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.DateTime;
                Fecha.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Fecha.AppearanceCell.Options.UseTextOptions = true;
                Fecha.VisibleIndex = 2;
                Fecha.Width = 50;
                Fecha.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Fecha);

                //VlrCapital
                GridColumn VlrCapital = new GridColumn();
                VlrCapital.FieldName = this.unboundPrefix + "VlrCapital";
                VlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCapital");
                VlrCapital.UnboundType = UnboundColumnType.Decimal;
                VlrCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCapital.AppearanceCell.Options.UseTextOptions = true;
                VlrCapital.VisibleIndex = 3;
                VlrCapital.Width = 100;
                VlrCapital.OptionsColumn.AllowEdit = false;
                VlrCapital.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrCapital);

                //VlrIntCapital
                GridColumn VlrIntCapital = new GridColumn();
                VlrIntCapital.FieldName = this.unboundPrefix + "VlrIntCapital";
                VlrIntCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntCapital");
                VlrIntCapital.UnboundType = UnboundColumnType.Decimal;
                VlrIntCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrIntCapital.AppearanceCell.Options.UseTextOptions = true;
                VlrIntCapital.VisibleIndex = 4;
                VlrIntCapital.Width = 100;
                VlrIntCapital.OptionsColumn.AllowEdit = false;
                VlrIntCapital.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrIntCapital);

                //VlrPoliza
                GridColumn VlrPoliza = new GridColumn();
                VlrPoliza.FieldName = this.unboundPrefix + "VlrPoliza";
                VlrPoliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPoliza");
                VlrPoliza.UnboundType = UnboundColumnType.Decimal;
                VlrPoliza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPoliza.AppearanceCell.Options.UseTextOptions = true;
                VlrPoliza.VisibleIndex = 5;
                VlrPoliza.Width = 100;
                VlrPoliza.OptionsColumn.AllowEdit = false;
                VlrPoliza.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrPoliza);

                //VlrIntPoliza
                GridColumn VlrIntPoliza = new GridColumn();
                VlrIntPoliza.FieldName = this.unboundPrefix + "VlrIntPoliza";
                VlrIntPoliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntPoliza");
                VlrIntPoliza.UnboundType = UnboundColumnType.Decimal;
                VlrIntPoliza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrIntPoliza.AppearanceCell.Options.UseTextOptions = true;
                VlrIntPoliza.VisibleIndex = 6;
                VlrIntPoliza.Width = 100;
                VlrIntPoliza.OptionsColumn.AllowEdit = false;
                VlrIntPoliza.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrIntPoliza);

                //VlrGastos
                GridColumn VlrGastos = new GridColumn();
                VlrGastos.FieldName = this.unboundPrefix + "VlrGastos";             
                VlrGastos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGastos");
                VlrGastos.UnboundType = UnboundColumnType.Decimal;
                VlrGastos.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGastos.AppearanceCell.Options.UseTextOptions = true;
                VlrGastos.VisibleIndex = 7;
                VlrGastos.Width = 80;
                VlrGastos.OptionsColumn.AllowEdit = false;
                VlrGastos.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrGastos);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.Visible = false;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);
                #endregion
                #region Agrega las columnas Subgrilla

                //ComponenteCarteraID
                GridColumn componenteCarteraID = new GridColumn();
                componenteCarteraID.FieldName = this.unboundPrefix + "ComponenteCarteraID";
                componenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                componenteCarteraID.UnboundType = UnboundColumnType.String;
                componenteCarteraID.VisibleIndex = 0;
                componenteCarteraID.Width = 70;
                componenteCarteraID.Visible = true;
                componenteCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(componenteCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(descripcion);

                //VlrCausado/VlrJuzgado
                GridColumn VlrCausado = new GridColumn();
                VlrCausado.FieldName = this.unboundPrefix + "VlrCausado";
                VlrCausado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCausado");
                VlrCausado.UnboundType = UnboundColumnType.Decimal;
                VlrCausado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCausado.AppearanceCell.Options.UseTextOptions = true;
                VlrCausado.VisibleIndex = 2;
                VlrCausado.Width = 100;
                VlrCausado.Visible = true;
                VlrCausado.OptionsColumn.AllowEdit = false;
                VlrCausado.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(VlrCausado);

                //VlrNoCausado/VlrExtra
                GridColumn VlrNoCausado = new GridColumn();
                VlrNoCausado.FieldName = this.unboundPrefix + "VlrNoCausado";
                VlrNoCausado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNoCausado");
                VlrNoCausado.UnboundType = UnboundColumnType.Decimal;
                VlrNoCausado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrNoCausado.AppearanceCell.Options.UseTextOptions = true;
                VlrNoCausado.VisibleIndex = 3;
                VlrNoCausado.Width = 100;
                VlrNoCausado.Visible = true;
                VlrNoCausado.OptionsColumn.AllowEdit = true;
                VlrNoCausado.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(VlrNoCausado);

                //VlrPagar
                GridColumn vlrPagar = new GridColumn();
                vlrPagar.FieldName = this.unboundPrefix + "AbonoValor";
                vlrPagar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPagar");
                vlrPagar.UnboundType = UnboundColumnType.Decimal;
                vlrPagar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPagar.AppearanceCell.Options.UseTextOptions = true;
                vlrPagar.VisibleIndex = 4;
                vlrPagar.Width = 100;
                vlrPagar.Visible = true;
                vlrPagar.OptionsColumn.AllowEdit = false;
                vlrPagar.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrPagar);

                //AbonoCJValor
                GridColumn AbonoCJValor = new GridColumn();
                AbonoCJValor.FieldName = this.unboundPrefix + "AbonoCJValor";
                AbonoCJValor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AbonoCJValor");
                AbonoCJValor.UnboundType = UnboundColumnType.Decimal;
                AbonoCJValor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                AbonoCJValor.AppearanceCell.Options.UseTextOptions = true;
                AbonoCJValor.VisibleIndex = 5;
                AbonoCJValor.Width = 100;
                AbonoCJValor.Visible = false;
                AbonoCJValor.OptionsColumn.AllowEdit = true;
                AbonoCJValor.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(AbonoCJValor);

                #endregion
                #region Agrega Columnas Grilla Secundaria

                //TipoMvto
                GridColumn TipoMvto = new GridColumn();
                TipoMvto.FieldName = this.unboundPrefix + "TipoMvto";
                TipoMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoMvto");
                TipoMvto.UnboundType = UnboundColumnType.Integer;
                TipoMvto.VisibleIndex = 0;
                TipoMvto.Width = 60;
                TipoMvto.OptionsColumn.AllowEdit = false;
                this.gvHistoria.Columns.Add(TipoMvto);

                //Observación
                GridColumn obs = new GridColumn();
                obs.FieldName = this.unboundPrefix + "Observacion";
                obs.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                obs.UnboundType = UnboundColumnType.Integer;
                obs.VisibleIndex = 1;
                obs.Width = 120;
                obs.OptionsColumn.AllowEdit = false;
                this.gvHistoria.Columns.Add(obs);

                //FechaInicial
                GridColumn FechaInicial = new GridColumn();
                FechaInicial.FieldName = this.unboundPrefix + "FechaInicial";
                FechaInicial.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaInicial");
                FechaInicial.UnboundType = UnboundColumnType.DateTime;
                FechaInicial.VisibleIndex = 2;
                FechaInicial.Width = 80;
                FechaInicial.OptionsColumn.AllowEdit = false;
                this.gvHistoria.Columns.Add(FechaInicial);

                //FechaFinal
                GridColumn FechaFinal = new GridColumn();
                FechaFinal.FieldName = this.unboundPrefix + "FechaFinal";
                FechaFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFinal");
                FechaFinal.UnboundType = UnboundColumnType.DateTime;
                FechaFinal.VisibleIndex = 3;
                FechaFinal.Width = 80;
                FechaFinal.OptionsColumn.AllowEdit = false;
                this.gvHistoria.Columns.Add(FechaFinal);

                //Comprobante
                GridColumn Comprobante = new GridColumn();
                Comprobante.FieldName = this.unboundPrefix + "Comprobante";
                Comprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Comprobante");
                Comprobante.UnboundType = UnboundColumnType.String;
                Comprobante.VisibleIndex = 4;
                Comprobante.Width = 60;
                Comprobante.OptionsColumn.AllowEdit = false;
                this.gvHistoria.Columns.Add(Comprobante);

                //MvtoCapital
                GridColumn MvtoCapital = new GridColumn();
                MvtoCapital.FieldName = this.unboundPrefix + "MvtoCapital";
                MvtoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCapital");
                MvtoCapital.UnboundType = UnboundColumnType.Decimal;
                MvtoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                MvtoCapital.AppearanceCell.Options.UseTextOptions = true;
                MvtoCapital.VisibleIndex = 5;
                MvtoCapital.Width = 100;
                MvtoCapital.OptionsColumn.AllowEdit = false;
                MvtoCapital.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(MvtoCapital);

                //MvtoInteres
                GridColumn MvtoInteres = new GridColumn();
                MvtoInteres.FieldName = this.unboundPrefix + "MvtoInteres";
                MvtoInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntCapital");
                MvtoInteres.UnboundType = UnboundColumnType.Decimal;
                MvtoInteres.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                MvtoInteres.AppearanceCell.Options.UseTextOptions = true;
                MvtoInteres.VisibleIndex = 6;
                MvtoInteres.Width = 100;
                MvtoInteres.OptionsColumn.AllowEdit = false;
                MvtoInteres.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(MvtoInteres);

                //MvtoGastos
                GridColumn MvtoGastos = new GridColumn();
                MvtoGastos.FieldName = this.unboundPrefix + "MvtoGastos";
                MvtoGastos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGastos");
                MvtoGastos.UnboundType = UnboundColumnType.Decimal;
                MvtoGastos.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                MvtoGastos.AppearanceCell.Options.UseTextOptions = true;
                MvtoGastos.VisibleIndex = 7;
                MvtoGastos.Width = 100;
                MvtoGastos.OptionsColumn.AllowEdit = false;
                MvtoGastos.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(MvtoGastos);


                //SaldoCapital
                GridColumn SaldoCapital = new GridColumn();
                SaldoCapital.FieldName = this.unboundPrefix + "SaldoCapital";
                SaldoCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoCapitalHist");
                SaldoCapital.UnboundType = UnboundColumnType.Decimal;
                SaldoCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoCapital.AppearanceCell.Options.UseTextOptions = true;
                SaldoCapital.VisibleIndex = 10;
                SaldoCapital.Width = 100;
                SaldoCapital.OptionsColumn.AllowEdit = false;
                SaldoCapital.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(SaldoCapital);

                //SaldoInteres
                GridColumn SaldoInteres = new GridColumn();
                SaldoInteres.FieldName = this.unboundPrefix + "SaldoInteres";
                SaldoInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoIntCapitalHist");
                SaldoInteres.UnboundType = UnboundColumnType.Decimal;
                SaldoInteres.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoInteres.AppearanceCell.Options.UseTextOptions = true;
                SaldoInteres.VisibleIndex = 11;
                SaldoInteres.Width = 100;
                SaldoInteres.OptionsColumn.AllowEdit = false;
                SaldoInteres.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(SaldoInteres);

                //SaldoGastos
                GridColumn SaldoGastos = new GridColumn();
                SaldoGastos.FieldName = this.unboundPrefix + "SaldoGastos";
                SaldoGastos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SaldoGastosHist");
                SaldoGastos.UnboundType = UnboundColumnType.Decimal;
                SaldoGastos.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoGastos.AppearanceCell.Options.UseTextOptions = true;
                SaldoGastos.VisibleIndex = 12;
                SaldoGastos.Width = 100;
                SaldoGastos.OptionsColumn.AllowEdit = false;
                SaldoGastos.ColumnEdit = this.editSpin;
                this.gvHistoria.Columns.Add(SaldoGastos);

                #endregion
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.gvHistoria.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Trae la información de los créditos deun cliente
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                //Tipos de movimiento
                Dictionary<string, string> dicTipoMov = new Dictionary<string, string>();
                dicTipoMov.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_Consulta"));
                if (this.masterCliente.ValidID)
                {
                    this.gcDocument.DataSource = null;
                    this.cobrosJuridicos = this._bc.AdministrationModel.GetCobroJuridicoByCliente(this.clienteID, this.dtFechaDoc.DateTime.Date);
                    if (this.cobrosJuridicos.Count > 0)
                    {
                        this._currentCredito = this.cobrosJuridicos.First();
                        this.cobrosJuridicos.ForEach(x => x.Aprobado.Value = true);
                        
                        #region Carga la fecha del documento

                        if ((this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico || this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago || this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido))   
                        {
                            DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                            if (!string.IsNullOrEmpty(cliente.FechaINIEstado.Value.ToString()))
                                this.dtFechaEC.DateTime = cliente.FechaINIEstado.Value.Value;
                        }
                        else if (!string.IsNullOrEmpty(this.cobrosJuridicos.First().EC_Fecha.Value.ToString()))
                        {
                            this.dtFechaEC.DateTime = this.cobrosJuridicos.First().EC_Fecha.Value.Value;
                        }

                        #endregion
                        #region Carga los tipos de movimientos permitidos según el estado del cliente

                        string editValue = "0";
                        this.cmbEstadoActual.EditValue = this._currentCredito.EstadoDeuda.Value.ToString();
                        if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico)
                        {
                            //dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                            dicTipoMov.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoPago"));
                            //dicTipoMov.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoIncum"));
                            dicTipoMov.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_Abono"));
                            dicTipoMov.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_Condonacion"));
                            //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                            editValue = "2";
                        }
                        else if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago)
                        {
                            //dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                            //dicTipoMov.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoPago"));
                            dicTipoMov.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoIncum"));
                            //dicTipoMov.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_Abono"));
                            //dicTipoMov.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_Condonacion"));
                            //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                            editValue = "3";

                        }
                        else if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                        {
                            //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                            //editValue = "6";
                        }
                        else
                        {
                            dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                            editValue = "1";
                        }
                            
                        #endregion

                        this.cmbTipoMov.Properties.DataSource = dicTipoMov;
                        this.cmbTipoMov.EditValue = editValue;

                        this.LoadTipoMvto();
                    }
                    else
                    {
                        this.cmbTipoMov.Properties.DataSource = dicTipoMov;
                        this.cmbTipoMov.EditValue = "0";

                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ClienteForCJ), this.clienteID);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Filtra los créditos según el tipo de movimiento
        /// </summary>
        private void LoadTipoMvto()
        {
            try
            {
                #region Inicia Controles

                this.btnAbonos.Visible = false;
                this.lblFiltro.Visible = false;
                this.cmbFiltro.Visible = false;

                this.gvDetalle.Columns[this.unboundPrefix + "AbonoCJValor"].Visible = false;

                #endregion

                this.cobrosJuridicosTemp = ObjectCopier.Clone(this.cobrosJuridicos);
                this.tipoMvto = this.cmbTipoMov.EditValue != null && this.cmbTipoMov.EditValue.ToString() != "" ? this.cmbTipoMov.EditValue.ToString() : "0";
                if (tipoMvto == "0")
                {
                    #region Consulta

                    this.lblFiltro.Visible = true;
                    this.cmbFiltro.Visible = true;

                    #endregion
                }
                else if (tipoMvto == "1")
                {
                    #region Envío Cobro jurídico

                    ////Obtiene los creditos que aun no estan en Cobro Juridico
                    //this.cobrosJuridicosTemp = this.cobrosJuridicosTemp.Where(x => x.EstadoDeuda.Value != (int)EstadoDeuda.Juridico
                    //                                    && x.EstadoDeuda.Value != (int)EstadoDeuda.AcuerdoPago && x.EstadoDeuda.Value != (int)EstadoDeuda.AcuerdoPagoIncumplido
                    //                                    && x.EC_Proposito.Value == (byte)PropositoEstadoCuenta.EnvioCobroJuridico && x.NumDocProceso.Value == null).ToList();

                    #endregion
                }
                else if (tipoMvto == "2" || tipoMvto == "3" || tipoMvto == "4")
                {
                    #region Cobro jurídico/Acuerdos...
                    //this.cobrosJuridicosTemp.RemoveAll(x=> !x.EC_Proposito.Value.HasValue || x.EC_Proposito.Value == 0);
                    #endregion
                }
                else if (tipoMvto == "5")
                {
                    #region Abono
                    this.gvDetalle.Columns[this.unboundPrefix + "AbonoCJValor"].Visible = true;
                    #endregion
                }

                this.gcDocument.DataSource = this.cobrosJuridicosTemp;
                this.gvDocument.MoveFirst();
                this.gvDocument.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "LoadTipoMvto"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
              
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                FormProvider.Master.itemPrint.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {                  
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemSearch.Enabled = true;
                    FormProvider.Master.itemUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }


        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al momento de salir del cliente
        /// </summary>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID)
                {
                    if (this.clienteID != this.masterCliente.Value)
                        this.clienteID = this.masterCliente.Value;   
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.masterCliente.Value))
                    {
                        string msg = String.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                        MessageBox.Show(msg);
                    }
                    this.CleanData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la clase de deuda
        /// </summary>
        private void cmbClaseDeuda_EditValueChanged(object sender, EventArgs e)
        {
            try 
            {
                int row = this.gvDocument.FocusedRowHandle;
                if (row >= 0 && this.cobrosJuridicosTemp.Count() >= row 
                    && this.cobrosJuridicosTemp[row].DetalleCJHistorico != null && this.cobrosJuridicosTemp[row].DetalleCJHistorico.Count > 0)
                {
                    byte claseDeuda = Convert.ToByte(this.cmbClaseDeuda.EditValue);

                    if (this._claseDeuda != claseDeuda)
                    {
                        this._claseDeuda = claseDeuda;
                        List<DTO_ccCJHistorico> hist = new List<DTO_ccCJHistorico>();
                        if (claseDeuda == (byte)ClaseDeuda.Principal)
                        {
                            hist = this.cobrosJuridicosTemp[row].DetalleCJHistorico.Where(d =>
                                d.ClaseDeuda.Value == claseDeuda
                                && (d.TipoMvto.Value == 1 || d.TipoMvto.Value == 2 || d.TipoMvto.Value == 3)).ToList();
                        }
                        else
                        {
                            hist = this.cobrosJuridicosTemp[row].DetalleCJHistorico.Where(d =>
                                d.ClaseDeuda.Value == claseDeuda
                                && (d.TipoMvto.Value == 2 || d.TipoMvto.Value == 4 || d.TipoMvto.Value == 5)).ToList();
                        }

                        this.gcHistoria.DataSource = hist;
                        this.gvHistoria.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "cmbClaseDeuda_EditValueChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInteres_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.masterCliente.ValidID && this._currentCredito != null)
                {
                    this._bc.AdministrationModel.ccCJHistorico_RecalcularInteresCJ(this.dtFechaDoc.DateTime.Date, this._currentCredito.Libranza.Value.Value);
                    this.LoadDocuments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridico.cs", "btnInteres_Click"));
            }
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    if (this.cobrosJuridicosTemp[e.FocusedRowHandle].DetalleCJHistorico.Count > 0 && this.cobrosJuridicosTemp[e.FocusedRowHandle].DetalleCJHistorico.First().ClaseDeuda.Value == 2)
                    {
                        this.gvHistoria.Columns[this.unboundPrefix + "MvtoCapital"].Caption = this.polizaRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "MvtoInteres"].Caption = this.polizaIntRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "SaldoCapital"].Caption = this.polizaSaldoRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "SaldoInteres"].Caption = this.polizaSaldoIntRsx;
                    }
                    else
                    {
                        this.gvHistoria.Columns[this.unboundPrefix + "MvtoCapital"].Caption = this.capitalRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "MvtoInteres"].Caption = this.capitalIntRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "SaldoCapital"].Caption = this.capitalSaldoRsx;
                        this.gvHistoria.Columns[this.unboundPrefix + "SaldoInteres"].Caption = this.capitalSaldoIntRsx;
                    }

                    this._currentCredito = (DTO_ccCreditoDocu)this.gvDocument.GetRow(e.FocusedRowHandle);

                    this.cmbClaseDeuda.EditValueChanged -= new System.EventHandler(this.cmbClaseDeuda_EditValueChanged);

                    #region Carga la fecha del documento

                    if ((this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico || this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago || this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido))
                    {
                        DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                        if (!string.IsNullOrEmpty(cliente.FechaINIEstado.Value.ToString()))
                            this.dtFechaEC.DateTime = cliente.FechaINIEstado.Value.Value;
                    }
                    else if (!string.IsNullOrEmpty(this.cobrosJuridicos.First().EC_Fecha.Value.ToString()))
                        this.dtFechaEC.DateTime = this.cobrosJuridicos.First().EC_Fecha.Value.Value;

                    #endregion
                    #region Carga los tipos de movimientos permitidos según el estado del cliente
                    Dictionary<string, string> dicTipoMov = new Dictionary<string, string>();
                    dicTipoMov.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_Consulta"));
                    string editValue = "0";
                    this.cmbEstadoActual.EditValue = this._currentCredito.EstadoDeuda.Value.ToString();
                    if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico)
                    {
                        //dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                        dicTipoMov.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoPago"));
                        //dicTipoMov.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoIncum"));
                        dicTipoMov.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_Abono"));
                        dicTipoMov.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_Condonacion"));
                        //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                        editValue = "2";
                    }
                    else if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago)
                    {
                        //dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                        //dicTipoMov.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoPago"));
                        dicTipoMov.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioAcuerdoIncum"));
                        //dicTipoMov.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_Abono"));
                        //dicTipoMov.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_Condonacion"));
                        //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                        editValue = "3";
                    }
                    else if (this._currentCredito.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                    {
                        //dicTipoMov.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_LiquidacionContrato"));
                        //editValue = "6";
                    }
                    else
                    {
                        dicTipoMov.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_EnvioCobroJur"));
                        editValue = "1";
                    }
                    this.cmbTipoMov.Properties.DataSource = dicTipoMov;
                    this.cmbTipoMov.EditValue = editValue;

                    this.tipoMvto = this.cmbTipoMov.EditValue != null && this.cmbTipoMov.EditValue.ToString() != "" ? this.cmbTipoMov.EditValue.ToString() : "0";

                    #endregion

                    //Clase de deuda priincipal
                    this._claseDeuda = (byte)ClaseDeuda.Principal;
                    this.cmbClaseDeuda.EditValue = this._claseDeuda.ToString();
                    List<DTO_ccCJHistorico> hist  = this._currentCredito.DetalleCJHistorico.Where(d =>d.ClaseDeuda.Value == this._claseDeuda
                            && (d.TipoMvto.Value == 1 || d.TipoMvto.Value == 2 || d.TipoMvto.Value == 3)).ToList();

                    this.gcHistoria.DataSource = hist;
                    this.gvHistoria.RefreshData();

                    this.cmbClaseDeuda.EditValueChanged += new System.EventHandler(this.cmbClaseDeuda_EditValueChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "gvDocument_FocusedRowChanged"));
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
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
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            try
            {              
                if (this._currentCredito.Editable.Value.Value)
                {
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, " Si no ha guardado puede perder los cambios realizados sobre el crédito actual,¿Desea continuar?");

                    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        e.Allow = true;
                    else
                        e.Allow = false;
                } 
                this.gvDetalle.PostEditor();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvHistoria_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (e.ListSourceRowIndex >= 0 && fieldName == "TipoMvto")
                {
                    DTO_ccCJHistorico row = (DTO_ccCJHistorico)this.gvHistoria.GetRow(e.ListSourceRowIndex);
                    if (row != null)
                    {
                        if (Convert.ToInt32(e.Value) == 0)
                            e.DisplayText = "ULT PAG";
                        else if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico)
                            e.DisplayText = "CJU";
                        else if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago)
                            e.DisplayText = "AP";
                        else if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                            e.DisplayText = "API";
                        else if (Convert.ToInt32(e.Value) == 2)
                            e.DisplayText = "INT";
                        else if (Convert.ToInt32(e.Value) == 3)
                            e.DisplayText = "ABO";
                        else if (Convert.ToInt32(e.Value) == 4)
                            e.DisplayText = "POL";
                        else if (Convert.ToInt32(e.Value) == 5)
                            e.DisplayText = "GTO";
                        else if (Convert.ToInt32(e.Value) == 6)
                            e.DisplayText = "JUZ";
                        else if (Convert.ToInt32(e.Value) == 7)
                            e.DisplayText = "SDO";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "gvHistoria_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            GridView view = (GridView)sender;
            if (fieldName == "VlrNoCausado")
            {
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.RowHandle);
                if (Convert.ToDecimal(e.Value) <= comp.AbonoValor.Value)
                {
                    comp.VlrCausado.Value = comp.AbonoValor.Value - comp.VlrNoCausado.Value;
                    this._currentCredito.VlrPoliza.Value = this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value == this._compSeguro).Sum(x => x.VlrNoCausado.Value);
                    this._currentCredito.VlrGastos.Value = this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value != this._compSeguro).Sum(x => x.VlrNoCausado.Value);
                    this._currentCredito.VlrCapital.Value = this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value == this._compCapital || x.ComponenteCarteraID.Value == this._compSeguro).Sum(x => x.VlrCausado.Value);
                }
                else
                {
                    comp.VlrNoCausado.Value = comp.AbonoValor.Value;
                    comp.VlrCausado.Value = 0;
                    this._currentCredito.VlrPoliza.Value =  this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value == this._compSeguro).Sum(x => x.VlrNoCausado.Value);
                    this._currentCredito.VlrGastos.Value =  this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value != this._compSeguro).Sum(x => x.VlrNoCausado.Value);
                    this._currentCredito.VlrCapital.Value = this._currentCredito.Detalle.FindAll(x => x.ComponenteCarteraID.Value == this._compCapital || x.ComponenteCarteraID.Value == this._compSeguro).Sum(x => x.VlrCausado.Value);

                }
            }
            this.gvDetalle.PostEditor();
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.RowHandle);
                if (comp.VlrNoCausado.Value > comp.AbonoValor.Value)
                {
                    this.gvDetalle.SetColumnError(view.Columns[this.unboundPrefix + "VlrNoCausado"], "El valor extra no puede exceder al Valor a Pagar");
                    e.Allow = false;
                }
                else
                {                    
                   this.gvDetalle.ClearColumnErrors();
                }
                   
                this.gvDetalle.PostEditor();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                DTO_ccCreditoComponentes comp = (DTO_ccCreditoComponentes)view.GetRow(e.FocusedRowHandle);
                if (comp.ComponenteCarteraID.Value.Equals(this._compCapital))
                    this.gvDetalle.Columns[this.unboundPrefix + "VlrNoCausado"].OptionsColumn.AllowEdit = false;
                else
                    this.gvDetalle.Columns[this.unboundPrefix + "VlrNoCausado"].OptionsColumn.AllowEdit = true;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoGral.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            this.gvDetalle.PostEditor();
            try
            {
                //if(this.cobrosJuridicosTemp.Count != this.cobrosJuridicos.Count)
                //{
                //    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_InvalidClienteEC));
                //    return;
                //}

                if (!string.IsNullOrWhiteSpace(this.tipoMvto) && this.tipoMvto != "0")
                {
                    this.saldosComponentes = new List<DTO_ccSaldosComponentes>();
                    List<DTO_ccCreditoDocu> cobrosTemp = this.cobrosJuridicosTemp.Where(x => x.Aprobado.Value == true).ToList();
                    if (cobrosTemp.Count > 0)
                    {
                        if (this._currentCredito != null && !this._currentCredito.Editable.Value.Value)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Debe seleccionar un crédito para guardar"));
                            return;
                        }

                        #region Válida si el usuario desea enviar los créditos a acuerdo de pago o acuerdode pago incumplido
                        if (this.tipoMvto == "2")
                    {
                        string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, "¿Está seguro de enviar el crédito a acuerdo de pago?");

                        if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                    else if (this.tipoMvto == "3") 
                    {
                        string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, "¿Está seguro de enviar el crédito a acuerdo de pago incumplido?");

                        if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                    //else if (this.tipoMvto == "6")
                    //{
                    //    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    //    string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, "¿Está seguro de enviar el crédito a liquidación de juzgado?");

                    //    if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                    //        return;
                    //}
                    #endregion

                        #region Valida si no tiene VlrPoliza y Vlrgastos (CobroJur>AcuerdoPago)
                        if (this.tipoMvto == "2")
                        {
                            bool existVlrPol = this._currentCredito.VlrPoliza.Value > 0 || this._currentCredito.VlrGastos.Value > 0? true : false;
                            if (existVlrPol)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "No puede guardar porque el crédito tiene valor de Póliza y Gastos"));
                                return;
                            }
                        }
                        #endregion

                        foreach (DTO_ccCreditoDocu credito in cobrosTemp)
                        {
                            credito.FechaDoc.Value = this.dtFechaDoc.DateTime;
                            foreach (DTO_ccCreditoComponentes carComp in credito.Detalle)
                            {
                                #region Carga el DTO de SaldoComponentes
                                DTO_ccSaldosComponentes saldoComp = new DTO_ccSaldosComponentes();
                                saldoComp.NumDocCredito.Value = credito.NumeroDoc.Value;
                                saldoComp.ComponenteCarteraID.Value = carComp.ComponenteCarteraID.Value;
                                saldoComp.Descriptivo.Value = carComp.Descripcion.Value;
                                saldoComp.TotalSaldo.Value = carComp.TotalValor.Value;
                                saldoComp.CuotaSaldo.Value = carComp.AbonoValor.Value;
                                saldoComp.AbonoValor.Value = carComp.AbonoValor.Value;
                                saldosComponentes.Add(saldoComp);
                                #endregion
                            }
                        }

                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridico.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para filtrar la información en pantalla
        /// </summary>
        public override void TBSearch()
        {
            if(this.masterCliente.ValidID)
                this.LoadDocuments();
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadDocuments();
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                if(this.cobrosJuridicosTemp.Count > 0)
                {
                    DTO_ccCreditoDocu cred = (DTO_ccCreditoDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                    byte claseDeuda = Convert.ToByte(this.cmbClaseDeuda.EditValue);
                    string reportName = this._bc.AdministrationModel.Report_Cc_CobroJuridicoHistoria(this.documentID,cred.NumeroDoc.Value.Value,claseDeuda);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                    //Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJUridico.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public void SaveThread()
        {
            try
            {
                #region Guarda la info
                List<DTO_SerializedObject> results = null;
                if (this.tipoMvto == "1" || this.tipoMvto == "2" || this.tipoMvto == "3")
                {
                    int docIdProceso = this.documentID;
                    TipoEstadoCartera nuevoEstadoCartera = TipoEstadoCartera.CobroJuridico;
                    if(tipoMvto == "2")
                    {
                        docIdProceso = AppDocuments.AcuerdoPago;
                        nuevoEstadoCartera = TipoEstadoCartera.AcuerdoPago; 
                    }
                    else if (tipoMvto == "3")
                    {
                        docIdProceso = AppDocuments.AcuerdoPagoIncumplido;
                        nuevoEstadoCartera = TipoEstadoCartera.AcuerdoPagoIncumplido;
                    }
                    //else if (tipoMvto == "6")
                    //{
                    //    docIdProceso = AppDocuments.LiquidacionJuzgadoCJ;
                    //   // nuevoEstadoCartera = (TipoEstadoCartera)this.cliente.EstadoCartera.Value;
                    //}

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { docIdProceso, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);

                    //Guarda la info
                    results = _bc.AdministrationModel.EnvioCobroJuridico(docIdProceso, this._actFlujo.ID.Value, this.masterCliente.Value, this._currentCredito,
                        this.saldosComponentes, this.dtFechaDoc.DateTime.Date,this.dtFechaEC.DateTime.Date, nuevoEstadoCartera);
                }
                
                //List<DTO_SerializedObject> results = _bc.AdministrationModel.CobroJuridico_Add(this.documentID, this._actFlujo.ID.Value, this.cobrosJuridicos, saldosComponentes, Convert.ToByte(this.cmbTipoMov.EditValue));
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this._currentCredito.Editable.Value = false;
                #endregion
                if (results != null)
                {
                    #region Carga los resultados
                    int i = 0;
                    int percent = 0;
                    List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                    this.isValid = true;
                    MessageForm frm = null;
                    bool checkResults = true;
                    if (results.Count == 1)
                    {
                        if (results[0].GetType() == typeof(DTO_TxResult))
                        {
                            checkResults = false;
                            frm = new MessageForm((DTO_TxResult)results[0]);
                            this.isValid = false;
                        }
                    }
                    #endregion
                    #region Envia correos y carga los resultados
                    if (checkResults)
                    {
                        foreach (object obj in results)
                        {
                            #region Funciones de progreso
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (results.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                                break;
                            }
                            #endregion

                            //if (this.cobrosJuridicos[i].Aprobado.Value.Value)
                            //{
                                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.CobroJuridico, this._actFlujo.seUsuarioID.Value, obj, false);
                                if (!isOK)
                                {
                                    DTO_TxResult r = (DTO_TxResult)obj;
                                    resultsNOK.Add(r);
                                    this.isValid = false;
                                }
                            //}

                            i++;
                        }

                        frm = new MessageForm(resultsNOK);
                    }
                    #endregion

                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    if (this.isValid)
                        this.Invoke(this.refreshGridDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridico.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion    
    }

}
