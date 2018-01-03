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
    public partial class QueryLibranza : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> creditosCancelados = new List<DTO_ccCreditoDocu>();
        private DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();
        private DTO_ccCreditoDocu consultaCredito = new DTO_ccCreditoDocu();
        private DTO_ccCreditoDocu selectedCredito = new DTO_ccCreditoDocu();
        private DTO_VentaCartera ventaCartera = new DTO_VentaCartera();
        private DTO_InfoCredito infoCredito = new DTO_InfoCredito();
        private DTO_InfoPagos infoPagos = new DTO_InfoPagos();

        //Variable glControl
        private string componenteCapital = string.Empty;
        private string componenteUsura = string.Empty;
        private string componenteMora = string.Empty;
        private string componenteInteres = string.Empty;
        private string componenteIntSeguro = string.Empty;
        private string componenteSeguro = string.Empty;
        private string componentePrejuridico = string.Empty;

        //Variables formulario
        private bool validate = true;
        private bool newSearch = false;
        private string clienteID = string.Empty;
        private int libranza = 0;
        private string compradorCarteraID = string.Empty;
        private SectorCartera sector = SectorCartera.Solidario;
        private Dictionary<int, DTO_ccCreditoDocu> datosGenerales = new Dictionary<int, DTO_ccCreditoDocu>();
        private Dictionary<int, DTO_InfoCredito> datosCartera = new Dictionary<int, DTO_InfoCredito>();
        private Dictionary<int, DTO_VentaCartera> datosCesion = new Dictionary<int, DTO_VentaCartera>();
        private Dictionary<int, DTO_InfoPagos> datosPagos = new Dictionary<int, DTO_InfoPagos>();

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryLibranza()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryLibranza(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);
                
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
                this.rbLibranza.Checked = true;
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "QueryLibranza.cs-QueryLibranza"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryLibranza;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
            this._bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
            this._bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente1, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente2, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente3, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterClienteCuota, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterClienteIncorp, AppMasters.ccCliente, true, true, true, false);
            this._bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false);
            this._bc.InitMasterUC(this.masterCompradorCarteraCesion, AppMasters.ccCompradorCartera, true, true, true, false);
            this._bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
            this._bc.InitMasterUC(this.masterTipoCredito, AppMasters.ccTipoCredito, true, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
            this._bc.InitMasterUC(this.masterGestionCobranza, AppMasters.ccCobranzaGestion, true, true, true, false);
            this._bc.InitMasterUC(this.masterEstadoCobranza, AppMasters.ccCobranzaEstado, true, true, true, false);
            this._bc.InitMasterUC(this.masterClienteMov, AppMasters.ccCliente, true, true, true, false);
          
            //Deshabilita las pestañas
            this.EnableTabs(false);

            //Deshabilita los botones +- de la grilla
            this.gcGenerales.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Carga los componentes de glControl
            this.componenteCapital = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
            this.componenteMora = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
            this.componenteUsura = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);
            this.componenteInteres = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);
            this.componenteSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteSeguroVida);
            this.componenteIntSeguro = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteInteresSeguro);
            this.componentePrejuridico = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponentePrejuridico);

            Dictionary<string, string> datosTipoEstado = new Dictionary<string, string>();
            datosTipoEstado.Add("0", "-");
            datosTipoEstado.Add("1", "Propio");
            datosTipoEstado.Add("2", "Cedido");
            datosTipoEstado.Add("3", "Cobro Jurídico");
            datosTipoEstado.Add("4", "Acuerdo Pago");
            datosTipoEstado.Add("5", "Acuerdo Pago Incumplido");
            datosTipoEstado.Add("6", "Castigada");
            this.cmbTipoEstado.Properties.ValueMember = "Key";
            this.cmbTipoEstado.Properties.DisplayMember = "Value";
            this.cmbTipoEstado.Properties.DataSource = datosTipoEstado;
            this.cmbTipoEstado.EditValue = "0";

            Dictionary<string, string> datosEstadoDeuda = new Dictionary<string, string>();
            datosEstadoDeuda.Add("0", "-");
            datosEstadoDeuda.Add("1", "Normal");
            datosEstadoDeuda.Add("2", "Mora");
            datosEstadoDeuda.Add("3", "PreJurídico");
            datosEstadoDeuda.Add("4", "Cobro Jurídico");
            datosEstadoDeuda.Add("5", "Acuerdo Pago");
            datosEstadoDeuda.Add("6", "Acuerdo Incumplido");
            datosEstadoDeuda.Add("7", "Otro");
            this.cmbEstadoDeuda.Properties.ValueMember = "Key";
            this.cmbEstadoDeuda.Properties.DisplayMember = "Value";
            this.cmbEstadoDeuda.Properties.DataSource = datosEstadoDeuda;
            this.cmbEstadoDeuda.EditValue = "0";

            //Deshabilita los controles cuando se carga la pantalla por primera vez
            this.txtLibranza.Enabled = false;
            this.masterCliente.Enabled = false;
            this.txtOferta.Enabled = false;
            this.masterCompradorCartera.Enabled = false;
            this.masterPagaduria.EnableControl(false);
            this.masterPagaduria.EnableControl(false);
            this.masterLineaCredito.EnableControl(false);
            this.masterTipoCredito.EnableControl(false);
            this.masterCentroPago.EnableControl(false);
            this.masterZona.EnableControl(false);
            this.masterAsesor.EnableControl(false);
            this.masterGestionCobranza.EnableControl(false);
            this.masterEstadoCobranza.EnableControl(false);

            string sectorStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            if (!string.IsNullOrWhiteSpace(sectorStr) && sectorStr != "0")
                sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);

            if (this.sector == SectorCartera.Financiero)
            {
                this.masterPagaduria.Visible = false;
                this.masterCentroPago.Visible = false;
            }          
        }

        /// <summary>
        /// Agrega las columnas a las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Datos Generales
                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this._unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 0;
                libranza.Width = 80;
                libranza.Visible = true;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(libranza);

                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this._unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 1;
                clienteID.Width = 100;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(clienteID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this._unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 2;
                nombre.Width = 130;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(nombre);

                //VlrCredito
                GridColumn vlrCredito = new GridColumn();
                vlrCredito.FieldName = this._unboundPrefix + "VlrPrestamo";
                vlrCredito.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrestamo");
                vlrCredito.UnboundType = UnboundColumnType.Integer;
                vlrCredito.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCredito.AppearanceCell.Options.UseTextOptions = true;
                vlrCredito.VisibleIndex = 3;
                vlrCredito.Width = 150;
                vlrCredito.Visible = true;
                vlrCredito.ColumnEdit = this.editValue;
                vlrCredito.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(vlrCredito);

                //VlrLibranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this._unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Integer;
                vlrLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrLibranza.AppearanceCell.Options.UseTextOptions = true;
                vlrLibranza.VisibleIndex = 4;
                vlrLibranza.Width = 150;
                vlrLibranza.Visible = true;
                vlrLibranza.ColumnEdit = this.editValue;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(vlrLibranza);

                //VlrCuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Integer;
                vlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuota.AppearanceCell.Options.UseTextOptions = true;
                vlrCuota.VisibleIndex = 5;
                vlrCuota.Width = 150;
                vlrCuota.Visible = true;
                vlrCuota.ColumnEdit = this.editValue;
                vlrCuota.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(vlrCuota);

                //NumCuotas
                GridColumn plazo = new GridColumn();
                plazo.FieldName = this._unboundPrefix + "Plazo";
                plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                plazo.UnboundType = UnboundColumnType.Integer;
                plazo.VisibleIndex = 6;
                plazo.Width = 120;
                plazo.Visible = true;
                plazo.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(plazo);

                //Vlr Capital
                GridColumn capital = new GridColumn();
                capital.FieldName = this._unboundPrefix + "VlrCapital";
                capital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Capital");
                capital.UnboundType = UnboundColumnType.Integer;
                capital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                capital.AppearanceCell.Options.UseTextOptions = true;
                capital.VisibleIndex = 6;
                capital.Width = 150;
                capital.Visible = true;
                capital.OptionsColumn.AllowEdit = false;
                capital.ColumnEdit = this.editValue;
                this.gvGenerales.Columns.Add(capital);

                //Cuotas en Mora
                GridColumn CuotasMora = new GridColumn();
                CuotasMora.FieldName = this._unboundPrefix + "CuotasMora";
                CuotasMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotasMora");
                CuotasMora.UnboundType = UnboundColumnType.Decimal;
                CuotasMora.VisibleIndex = 6;
                CuotasMora.Width = 150;
                CuotasMora.Visible = true;
                CuotasMora.OptionsColumn.AllowEdit = false;
                this.gvGenerales.Columns.Add(CuotasMora);

                //Ver Documento
                GridColumn file = new GridColumn();
                file.FieldName = this._unboundPrefix + "FileUrl";
                file.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SearchDocument");
                file.UnboundType = UnboundColumnType.String;
                file.Width = 100;
                file.VisibleIndex = 6;
                file.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                file.Visible = true;
                file.OptionsColumn.AllowEdit = true;
                file.AppearanceCell.ForeColor = Color.Blue;
                this.gvGenerales.Columns.Add(file);
                #region Detalle
                //Componente
                GridColumn componente = new GridColumn();
                componente.FieldName = this._unboundPrefix + "ComponenteCarteraID";
                componente.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ComponenteCarteraID");
                componente.UnboundType = UnboundColumnType.String;
                componente.Width = 80;
                componente.VisibleIndex = 0;
                componente.Visible = true;
                componente.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(componente);

                //descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descripcion";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.Width = 120;
                descriptivo.VisibleIndex = 1;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(descriptivo);

                //CuotaValor
                GridColumn cuotaValor = new GridColumn();
                cuotaValor.FieldName = this._unboundPrefix + "CuotaValor";
                cuotaValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaValor");
                cuotaValor.UnboundType = UnboundColumnType.Integer;
                cuotaValor.VisibleIndex = 2;
                cuotaValor.Width = 130;
                cuotaValor.Visible = true;
                cuotaValor.ColumnEdit = this.editValue;
                cuotaValor.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(cuotaValor);

                //TotalValor
                GridColumn totalValor = new GridColumn();
                totalValor.FieldName = this._unboundPrefix + "TotalValor";
                totalValor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalValor");
                totalValor.UnboundType = UnboundColumnType.Integer;
                totalValor.VisibleIndex = 3;
                totalValor.Width = 130;
                totalValor.Visible = true;
                totalValor.ColumnEdit = this.editValue;
                totalValor.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(totalValor); 
                #endregion
                #endregion
                #region Datos Plan Pagos
                //CuotaID
                GridColumn cuotaID = new GridColumn();
                cuotaID.FieldName = this._unboundPrefix + "CuotaID";
                cuotaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaID.UnboundType = UnboundColumnType.Integer;
                cuotaID.VisibleIndex = 0;
                cuotaID.Width = 80;
                cuotaID.Visible = true;
                cuotaID.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(cuotaID);

                //Fecha Cuota
                GridColumn fechaCuota = new GridColumn();
                fechaCuota.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuota.UnboundType = UnboundColumnType.DateTime;
                fechaCuota.VisibleIndex = 1;
                fechaCuota.Width = 80;
                fechaCuota.Visible = true;
                fechaCuota.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(fechaCuota);

                //DiasMora
                GridColumn DiasMora = new GridColumn();
                DiasMora.FieldName = this._unboundPrefix + "DiasMora";
                DiasMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DiasMora");
                DiasMora.UnboundType = UnboundColumnType.Integer;
                DiasMora.VisibleIndex = 1;
                DiasMora.Width = 40;
                DiasMora.Visible = true;
                DiasMora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                DiasMora.AppearanceCell.Options.UseTextOptions = true;
                DiasMora.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(DiasMora);

                //Abono
                GridColumn abono = new GridColumn();
                abono.FieldName = this._unboundPrefix + "VlrPagadoCuota";
                abono.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Abono");
                abono.UnboundType = UnboundColumnType.Integer;
                abono.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                abono.AppearanceCell.Options.UseTextOptions = true;
                abono.VisibleIndex = 2;
                abono.Width = 80;
                abono.Visible = true;
                abono.ColumnEdit = this.editValue;
                abono.Summary.Add(DevExpress.Data.SummaryItemType.Sum, abono.FieldName, "{0:c0}"); 
                abono.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(abono);

                //vlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Integer;
                vlrSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrSaldo.AppearanceCell.Options.UseTextOptions = true;
                vlrSaldo.VisibleIndex = 3;
                vlrSaldo.Width = 80;
                vlrSaldo.Visible = true;
                vlrSaldo.ColumnEdit = this.editValue;
                vlrSaldo.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrSaldo.FieldName, "{0:c0}"); 
                vlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrSaldo);

                //VlrCuota
                GridColumn vlrCuotaCartera = new GridColumn();
                vlrCuotaCartera.FieldName = this._unboundPrefix + "VlrCuota";
                vlrCuotaCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                vlrCuotaCartera.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCartera.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuotaCartera.AppearanceCell.Options.UseTextOptions = true;
                vlrCuotaCartera.VisibleIndex = 4;
                vlrCuotaCartera.Width = 150;
                vlrCuotaCartera.Visible = true;
                vlrCuotaCartera.ColumnEdit = this.editValue;
                vlrCuotaCartera.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrCuotaCartera.FieldName, "{0:c0}"); 
                vlrCuotaCartera.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrCuotaCartera);

                //VlrCapital
                GridColumn vlrCapital = new GridColumn();
                vlrCapital.FieldName = this._unboundPrefix + "VlrCapital";
                vlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                vlrCapital.UnboundType = UnboundColumnType.Integer;
                vlrCapital.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCapital.AppearanceCell.Options.UseTextOptions = true;
                vlrCapital.VisibleIndex = 5;
                vlrCapital.Width = 150;
                vlrCapital.Visible = true;
                vlrCapital.ColumnEdit = this.editValue;
                vlrCapital.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrCapital.FieldName, "{0:c0}"); 
                vlrCapital.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrCapital);

                //VlrInteres
                GridColumn vlrInteres = new GridColumn();
                vlrInteres.FieldName = this._unboundPrefix + "VlrInteres";
                vlrInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                vlrInteres.UnboundType = UnboundColumnType.Integer;
                vlrInteres.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrInteres.AppearanceCell.Options.UseTextOptions = true;
                vlrInteres.VisibleIndex = 6;
                vlrInteres.Width = 150;
                vlrInteres.Visible = true;
                vlrInteres.ColumnEdit = this.editValue;
                vlrInteres.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrInteres.FieldName, "{0:c0}"); 
                vlrInteres.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrInteres);

                //VlrSeguro
                GridColumn vlrSeguro = new GridColumn();
                vlrSeguro.FieldName = this._unboundPrefix + "VlrSeguro";
                vlrSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                vlrSeguro.UnboundType = UnboundColumnType.Integer;
                vlrSeguro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrSeguro.AppearanceCell.Options.UseTextOptions = true;
                vlrSeguro.VisibleIndex = 7;
                vlrSeguro.Width = 150;
                vlrSeguro.Visible = true;
                vlrSeguro.ColumnEdit = this.editValue;
                vlrSeguro.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrSeguro.FieldName, "{0:c0}"); 
                vlrSeguro.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrSeguro);

                //VlrMoraLiquida
                GridColumn VlrMoraLiquida = new GridColumn();
                VlrMoraLiquida.FieldName = this._unboundPrefix + "VlrMoraLiquida";
                VlrMoraLiquida.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMoraLiquida");
                VlrMoraLiquida.UnboundType = UnboundColumnType.Integer;
                VlrMoraLiquida.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrMoraLiquida.AppearanceCell.Options.UseTextOptions = true;
                VlrMoraLiquida.VisibleIndex = 8;
                VlrMoraLiquida.Width = 100;
                VlrMoraLiquida.Visible = true;
                VlrMoraLiquida.ColumnEdit = this.editValue;
                VlrMoraLiquida.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrMoraLiquida.FieldName, "{0:c0}");
                VlrMoraLiquida.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(VlrMoraLiquida);

                //VlrPrejuridico
                GridColumn VlrPrejuridico = new GridColumn();
                VlrPrejuridico.FieldName = this._unboundPrefix + "VlrPrejuridico";
                VlrPrejuridico.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridico");
                VlrPrejuridico.UnboundType = UnboundColumnType.Integer;
                VlrPrejuridico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrejuridico.AppearanceCell.Options.UseTextOptions = true;
                VlrPrejuridico.VisibleIndex = 9;
                VlrPrejuridico.Width = 100;
                VlrPrejuridico.Visible = true;
                VlrPrejuridico.ColumnEdit = this.editValue;
                VlrPrejuridico.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrPrejuridico.FieldName, "{0:c0}"); 
                VlrPrejuridico.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(VlrPrejuridico);

                //VlrOtrosFijos
                GridColumn vlrOtros = new GridColumn();
                vlrOtros.FieldName = this._unboundPrefix + "VlrOtrosFijos";
                vlrOtros.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosFijos");
                vlrOtros.UnboundType = UnboundColumnType.Integer;
                vlrOtros.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrOtros.AppearanceCell.Options.UseTextOptions = true;
                vlrOtros.VisibleIndex = 10;
                vlrOtros.Width = 150;
                vlrOtros.Visible = true;
                vlrOtros.ColumnEdit = this.editValue;
                vlrOtros.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrOtros.FieldName, "{0:c0}"); 
                vlrOtros.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrOtros);
                #endregion
                #region Datos Plan Pagos
                //CuotaID
                GridColumn cuotaIDSentencia = new GridColumn();
                cuotaIDSentencia.FieldName = this._unboundPrefix + "CuotaID";
                cuotaIDSentencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaIDSentencia.UnboundType = UnboundColumnType.Integer;
                cuotaIDSentencia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                cuotaIDSentencia.AppearanceCell.Options.UseTextOptions = true;
                cuotaIDSentencia.VisibleIndex = 0;
                cuotaIDSentencia.Width = 80;
                cuotaIDSentencia.Visible = true;
                cuotaIDSentencia.OptionsColumn.AllowEdit = false;
                this.gvInfoPagos.Columns.Add(cuotaIDSentencia);

                //Fecha Cuota
                GridColumn fechaCuotaSentencia = new GridColumn();
                fechaCuotaSentencia.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuotaSentencia.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuotaSentencia.UnboundType = UnboundColumnType.DateTime;
                fechaCuotaSentencia.VisibleIndex = 1;
                fechaCuotaSentencia.Width = 120;
                fechaCuotaSentencia.Visible = true;
                fechaCuotaSentencia.OptionsColumn.AllowEdit = false;
                this.gvInfoPagos.Columns.Add(fechaCuotaSentencia);

                //VlrCuotaSent
                GridColumn VlrCuotaSent = new GridColumn();
                VlrCuotaSent.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuotaSent.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuotaSent.UnboundType = UnboundColumnType.Integer;
                VlrCuotaSent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCuotaSent.AppearanceCell.Options.UseTextOptions = true;
                VlrCuotaSent.VisibleIndex = 2;
                VlrCuotaSent.Width = 120;
                VlrCuotaSent.Visible = true;
                VlrCuotaSent.ColumnEdit = this.editValue;
                VlrCuotaSent.OptionsColumn.AllowEdit = false;
                this.gvInfoPagos.Columns.Add(VlrCuotaSent);

                //VlrAbono
                GridColumn VlrAbono = new GridColumn();
                VlrAbono.FieldName = this._unboundPrefix + "Abono";
                VlrAbono.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Abono");
                VlrAbono.UnboundType = UnboundColumnType.Integer;
                VlrAbono.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrAbono.AppearanceCell.Options.UseTextOptions = true;
                VlrAbono.VisibleIndex = 3;
                VlrAbono.Width = 120;
                VlrAbono.Visible = true;
                VlrAbono.ColumnEdit = this.editValue;
                VlrAbono.OptionsColumn.AllowEdit = false;
                this.gvInfoPagos.Columns.Add(VlrAbono);

                //Saldo
                GridColumn SaldoSent = new GridColumn();
                SaldoSent.FieldName = this._unboundPrefix + "Saldo";
                SaldoSent.Caption = _bc.GetResource(LanguageTypes.Forms,"Saldo");
                SaldoSent.UnboundType = UnboundColumnType.Integer;
                SaldoSent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SaldoSent.AppearanceCell.Options.UseTextOptions = true;
                SaldoSent.VisibleIndex = 4;
                SaldoSent.Width = 120;
                SaldoSent.Visible = true;
                SaldoSent.ColumnEdit = this.editValue;
                SaldoSent.OptionsColumn.AllowEdit = false;
                this.gvInfoPagos.Columns.Add(SaldoSent);
                #endregion
                #region Datos Cuota Pago
                //CuotaID
                GridColumn cuotaIDPago = new GridColumn();
                cuotaIDPago.FieldName = this._unboundPrefix + "CuotaID";
                cuotaIDPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaIDPago.UnboundType = UnboundColumnType.Integer;
                cuotaIDPago.VisibleIndex = 0;
                cuotaIDPago.Width = 60;
                cuotaIDPago.Visible = true;
                cuotaIDPago.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(cuotaIDPago);

                //Tipo Doc
                GridColumn tipoDoc = new GridColumn();
                tipoDoc.FieldName = this._unboundPrefix + "TipoDocumento";
                tipoDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoDocumento");
                tipoDoc.UnboundType = UnboundColumnType.Integer;
                tipoDoc.VisibleIndex = 1;
                tipoDoc.Width = 80;
                tipoDoc.Visible = true;
                tipoDoc.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(tipoDoc);

                //CajaID
                GridColumn caja = new GridColumn();
                caja.FieldName = this._unboundPrefix + "CajaID";
                caja.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CajaID");
                caja.UnboundType = UnboundColumnType.String;
                caja.VisibleIndex = 2;
                caja.Width = 90;
                caja.Visible = true;
                caja.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(caja);

                //Pref Doc
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 3;
                prefDoc.Width = 85;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(prefDoc);

                //Comprobante
                GridColumn Comprobante = new GridColumn();
                Comprobante.FieldName = this._unboundPrefix + "Comprobante";
                Comprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Comprobante");
                Comprobante.UnboundType = UnboundColumnType.String;
                Comprobante.VisibleIndex = 4;
                Comprobante.Width = 85;
                Comprobante.Visible = true;
                Comprobante.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(Comprobante);

                //Fecha Pago
                GridColumn fechaCuotaInicial = new GridColumn();
                fechaCuotaInicial.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuotaInicial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuotaInicial.UnboundType = UnboundColumnType.DateTime;
                fechaCuotaInicial.VisibleIndex = 5;
                fechaCuotaInicial.Width = 80;
                fechaCuotaInicial.Visible = true;
                fechaCuotaInicial.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(fechaCuotaInicial);

                //Fecha Pago
                GridColumn fechaPagoCuota = new GridColumn();
                fechaPagoCuota.FieldName = this._unboundPrefix + "FechaPago";
                fechaPagoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaPago");
                fechaPagoCuota.UnboundType = UnboundColumnType.DateTime;
                fechaPagoCuota.VisibleIndex = 5;
                fechaPagoCuota.Width = 80;
                fechaPagoCuota.Visible = true;
                fechaPagoCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(fechaPagoCuota);

                //Fecha Consigna
                GridColumn fechaConsigna = new GridColumn();
                fechaConsigna.FieldName = this._unboundPrefix + "FechaConsigna";
                fechaConsigna.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaAplica");
                fechaConsigna.UnboundType = UnboundColumnType.DateTime;
                fechaConsigna.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaConsigna.AppearanceCell.Options.UseTextOptions = true;
                fechaConsigna.VisibleIndex = 6;
                fechaConsigna.Width = 80;
                fechaConsigna.Visible = true;
                fechaConsigna.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(fechaConsigna);

                //Valor Pago
                GridColumn vlrCuotaCarteraCuota = new GridColumn();
                vlrCuotaCarteraCuota.FieldName = this._unboundPrefix + "Valor";
                vlrCuotaCarteraCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                vlrCuotaCarteraCuota.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCarteraCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuotaCarteraCuota.AppearanceCell.Options.UseTextOptions = true;
                vlrCuotaCarteraCuota.VisibleIndex = 6;
                vlrCuotaCarteraCuota.Width = 120;
                vlrCuotaCarteraCuota.Visible = true;
                vlrCuotaCarteraCuota.ColumnEdit = this.editValue;
                vlrCuotaCarteraCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(vlrCuotaCarteraCuota);

                //VlrCapital
                GridColumn VlrCapitalCuota = new GridColumn();
                VlrCapitalCuota.FieldName = this._unboundPrefix + "VlrCapital";
                VlrCapitalCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                VlrCapitalCuota.UnboundType = UnboundColumnType.Integer;
                VlrCapitalCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCapitalCuota.AppearanceCell.Options.UseTextOptions = true;
                VlrCapitalCuota.VisibleIndex = 7;
                VlrCapitalCuota.Width = 120;
                VlrCapitalCuota.Visible = true;
                VlrCapitalCuota.ColumnEdit = this.editValue;
                VlrCapitalCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrCapitalCuota);

                //VlrInteres
                GridColumn VlrInteresCuota = new GridColumn();
                VlrInteresCuota.FieldName = this._unboundPrefix + "VlrInteres";
                VlrInteresCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                VlrInteresCuota.UnboundType = UnboundColumnType.Integer;
                VlrInteresCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrInteresCuota.AppearanceCell.Options.UseTextOptions = true;
                VlrInteresCuota.VisibleIndex = 8;
                VlrInteresCuota.Width = 120;
                VlrInteresCuota.Visible = true;
                VlrInteresCuota.ColumnEdit = this.editValue;
                VlrInteresCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrInteresCuota);

                //VlrSeguro
                GridColumn VlrSeguroCuota = new GridColumn();
                VlrSeguroCuota.FieldName = this._unboundPrefix + "VlrSeguro";
                VlrSeguroCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                VlrSeguroCuota.UnboundType = UnboundColumnType.Integer;
                VlrSeguroCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSeguroCuota.AppearanceCell.Options.UseTextOptions = true;
                VlrSeguroCuota.VisibleIndex = 9;
                VlrSeguroCuota.Width = 120;
                VlrSeguroCuota.Visible = true;
                VlrSeguroCuota.ColumnEdit = this.editValue;
                VlrSeguroCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrSeguroCuota);

                //VlrOtrosFijos
                GridColumn VlrOtrosFijosCuota = new GridColumn();
                VlrOtrosFijosCuota.FieldName = this._unboundPrefix + "VlrOtrosFijos";
                VlrOtrosFijosCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosFijos");
                VlrOtrosFijosCuota.UnboundType = UnboundColumnType.Integer;
                VlrOtrosFijosCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrOtrosFijosCuota.AppearanceCell.Options.UseTextOptions = true;
                VlrOtrosFijosCuota.VisibleIndex = 10;
                VlrOtrosFijosCuota.Width = 100;
                VlrOtrosFijosCuota.Visible = true;
                VlrOtrosFijosCuota.ColumnEdit = this.editValue;
                VlrOtrosFijosCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrOtrosFijosCuota);

                //vlrMoraPago
                GridColumn vlrMoraPago = new GridColumn();
                vlrMoraPago.FieldName = this._unboundPrefix + "VlrMoraPago";
                vlrMoraPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMoraPago");
                vlrMoraPago.UnboundType = UnboundColumnType.Integer;
                vlrMoraPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrMoraPago.AppearanceCell.Options.UseTextOptions = true;
                vlrMoraPago.VisibleIndex = 11;
                vlrMoraPago.Width = 100;
                vlrMoraPago.Visible = true;
                vlrMoraPago.ColumnEdit = this.editValue;
                vlrMoraPago.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(vlrMoraPago);

                //VlrPJ
                GridColumn vlrPJ = new GridColumn();
                vlrPJ.FieldName = this._unboundPrefix + "VlrPrejuridicoPago";
                vlrPJ.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridicoPago");
                vlrPJ.UnboundType = UnboundColumnType.Integer;
                vlrPJ.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPJ.AppearanceCell.Options.UseTextOptions = true;
                vlrPJ.VisibleIndex = 12;
                vlrPJ.Width = 100;
                vlrPJ.Visible = true;
                vlrPJ.ColumnEdit = this.editValue;
                vlrPJ.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(vlrPJ);

                //VlrOtrosComponentes
                GridColumn VlrOtrosComponentes = new GridColumn();
                VlrOtrosComponentes.FieldName = this._unboundPrefix + "VlrOtrosComponentes";
                VlrOtrosComponentes.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosComponentes");
                VlrOtrosComponentes.UnboundType = UnboundColumnType.Integer;
                VlrOtrosComponentes.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrOtrosComponentes.AppearanceCell.Options.UseTextOptions = true;
                VlrOtrosComponentes.VisibleIndex = 13;
                VlrOtrosComponentes.Width = 100;
                VlrOtrosComponentes.Visible = true;
                VlrOtrosComponentes.ColumnEdit = this.editValue;
                VlrOtrosComponentes.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrOtrosComponentes);

                //DiasMora
                GridColumn DiasMoraPP = new GridColumn();
                DiasMoraPP.FieldName = this._unboundPrefix + "DiasMora";
                DiasMoraPP.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DiasMora");
                DiasMoraPP.UnboundType = UnboundColumnType.Integer;
                DiasMoraPP.VisibleIndex = 14;
                DiasMoraPP.Width = 60;
                DiasMoraPP.Visible = true;
                DiasMoraPP.ColumnEdit = this.editCant;
                DiasMoraPP.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(DiasMoraPP);

                //Ver Doc
                GridColumn fileDocCuotas = new GridColumn();
                fileDocCuotas.FieldName = this._unboundPrefix + "FileUrl";
                fileDocCuotas.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SearchDocument");
                fileDocCuotas.UnboundType = UnboundColumnType.String;
                fileDocCuotas.Width = 40;
                fileDocCuotas.VisibleIndex = 15;
                fileDocCuotas.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                fileDocCuotas.Visible = true;
                fileDocCuotas.OptionsColumn.AllowEdit = true;
                fileDocCuotas.AppearanceCell.ForeColor = Color.Blue;
                this.gvCuotaPagos.Columns.Add(fileDocCuotas);
                #endregion
                #region Datos Cesion
                //CuotaID
                GridColumn cuotaIDCesion = new GridColumn();
                cuotaIDCesion.FieldName = this._unboundPrefix + "CuotaID";
                cuotaIDCesion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotaID");
                cuotaIDCesion.UnboundType = UnboundColumnType.Integer;
                cuotaIDCesion.VisibleIndex = 0;
                cuotaIDCesion.Width = 50;
                cuotaIDCesion.Visible = true;
                cuotaIDCesion.OptionsColumn.AllowEdit = false;
                this.gvCesion.Columns.Add(cuotaIDCesion);

                //Fecha Cuota
                GridColumn fechaCuotaCesion = new GridColumn();
                fechaCuotaCesion.FieldName = this._unboundPrefix + "FechaCuota";
                fechaCuotaCesion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaCuota");
                fechaCuotaCesion.UnboundType = UnboundColumnType.DateTime;
                fechaCuotaCesion.VisibleIndex = 1;
                fechaCuotaCesion.Width = 50;
                fechaCuotaCesion.Visible = true;
                fechaCuotaCesion.OptionsColumn.AllowEdit = false;
                this.gvCesion.Columns.Add(fechaCuotaCesion);

                //Fecha Pago
                GridColumn fechaPago = new GridColumn();
                fechaPago.FieldName = this._unboundPrefix + "FechaPago";
                fechaPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaPago");
                fechaPago.UnboundType = UnboundColumnType.DateTime;
                fechaPago.VisibleIndex = 2;
                fechaPago.Width = 50;
                fechaPago.Visible = true;
                fechaPago.OptionsColumn.AllowEdit = false;
                this.gvCesion.Columns.Add(fechaPago);

                //VlrCuota
                GridColumn vlrCuotaCesion = new GridColumn();
                vlrCuotaCesion.FieldName = this._unboundPrefix + "VlrCuota";
                vlrCuotaCesion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                vlrCuotaCesion.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCesion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuotaCesion.AppearanceCell.Options.UseTextOptions = true;
                vlrCuotaCesion.VisibleIndex = 3;
                vlrCuotaCesion.Width = 50;
                vlrCuotaCesion.Visible = true;
                vlrCuotaCesion.ColumnEdit = this.editValue;
                vlrCuotaCesion.OptionsColumn.AllowEdit = false;
                this.gvCesion.Columns.Add(vlrCuotaCesion);
                #endregion
                #region Datos Componentes
                //CuentaID
                GridColumn cuentaID = new GridColumn();
                cuentaID.FieldName = this._unboundPrefix + "CuentaID";
                cuentaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuentaID");
                cuentaID.UnboundType = UnboundColumnType.String;
                cuentaID.VisibleIndex = 0;
                cuentaID.Width = 50;
                cuentaID.Visible = true;
                cuentaID.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(cuentaID);
                
                //DescriptivoCuenta
                GridColumn descriptivoCuenta = new GridColumn();
                descriptivoCuenta.FieldName = this._unboundPrefix + "Descriptivo";
                descriptivoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                descriptivoCuenta.UnboundType = UnboundColumnType.Integer;
                descriptivoCuenta.VisibleIndex = 1;
                descriptivoCuenta.Width = 50;
                descriptivoCuenta.Visible = true;
                descriptivoCuenta.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(descriptivoCuenta);

                //Total Inicial
                GridColumn totalIncial = new GridColumn();
                totalIncial.FieldName = this._unboundPrefix + "TotalInicial";
                totalIncial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalInicial");
                totalIncial.UnboundType = UnboundColumnType.Integer;
                totalIncial.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                totalIncial.AppearanceCell.Options.UseTextOptions = true;
                totalIncial.VisibleIndex = 2;
                totalIncial.Width = 50;
                totalIncial.Visible = true;
                totalIncial.ColumnEdit = this.editValue;
                totalIncial.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(totalIncial);

                //Movimiento
                GridColumn movimiento = new GridColumn();
                movimiento.FieldName = this._unboundPrefix + "AbonoSaldo";
                movimiento.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Movimiento");
                movimiento.UnboundType = UnboundColumnType.Integer;
                movimiento.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                movimiento.AppearanceCell.Options.UseTextOptions = true;
                movimiento.VisibleIndex = 3;
                movimiento.Width = 50;
                movimiento.Visible = true;
                movimiento.ColumnEdit = this.editValue;
                movimiento.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(movimiento);

                //Total Saldo
                GridColumn totalSaldo = new GridColumn();
                totalSaldo.FieldName = this._unboundPrefix + "TotalSaldo";
                totalSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalSaldo");
                totalSaldo.UnboundType = UnboundColumnType.Integer;
                totalSaldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                totalSaldo.AppearanceCell.Options.UseTextOptions = true;
                totalSaldo.VisibleIndex = 4;
                totalSaldo.Width = 50;
                totalSaldo.Visible = true;
                totalSaldo.ColumnEdit = this.editValue;
                totalSaldo.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(totalSaldo);
                #endregion
                #region Datos Incorporaciones  
                //Num Reincorpora
                GridColumn NumeroINC = new GridColumn();
                NumeroINC.FieldName = this._unboundPrefix + "NumeroINC";
                NumeroINC.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NumeroINC");
                NumeroINC.UnboundType = UnboundColumnType.Integer;
                NumeroINC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                NumeroINC.AppearanceCell.Options.UseTextOptions = true;
                NumeroINC.VisibleIndex = 1;
                NumeroINC.Width = 40;
                NumeroINC.Visible = true;
                NumeroINC.OptionsColumn.AllowEdit = false;
                this.gvIncorporacion.Columns.Add(NumeroINC);

                //Novedad
                GridColumn novedad = new GridColumn();
                novedad.FieldName = this._unboundPrefix + "NovedadIncorporaID";
                novedad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_NovedadIncorporaID");
                novedad.UnboundType = UnboundColumnType.String;
                novedad.VisibleIndex = 2;
                novedad.Width = 50;
                novedad.Visible = true;
                novedad.OptionsColumn.AllowEdit = false;
                this.gvIncorporacion.Columns.Add(novedad);

                //TipoNovedad
                GridColumn tipoNovedad = new GridColumn();
                tipoNovedad.FieldName = this._unboundPrefix + "TipoNovedad";
                tipoNovedad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoNovedad");
                tipoNovedad.UnboundType = UnboundColumnType.String;
                tipoNovedad.VisibleIndex = 3;
                tipoNovedad.Width = 70;
                tipoNovedad.Visible = true;
                tipoNovedad.OptionsColumn.AllowEdit = false;
                this.gvIncorporacion.Columns.Add(tipoNovedad);                  

                //Valor Cuota
                GridColumn vlrIncorpora = new GridColumn();
                vlrIncorpora.FieldName = this._unboundPrefix + "ValorCuota";
                vlrIncorpora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCuota");
                vlrIncorpora.UnboundType = UnboundColumnType.Decimal;
                vlrIncorpora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrIncorpora.AppearanceCell.Options.UseTextOptions = true;
                vlrIncorpora.VisibleIndex = 4;
                vlrIncorpora.Width = 90;
                vlrIncorpora.Visible = true;
                vlrIncorpora.OptionsColumn.AllowEdit = false;
                vlrIncorpora.ColumnEdit = this.editValue;
                this.gvIncorporacion.Columns.Add(vlrIncorpora);

                //Plazo
                GridColumn plazoInc = new GridColumn();
                plazoInc.FieldName = this._unboundPrefix + "PlazoINC";
                plazoInc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                plazoInc.UnboundType = UnboundColumnType.Decimal;
                plazoInc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                plazoInc.AppearanceCell.Options.UseTextOptions = true;
                plazoInc.VisibleIndex = 5;
                plazoInc.Width = 30;
                plazoInc.Visible = true;
                plazoInc.OptionsColumn.AllowEdit = false;
                this.gvIncorporacion.Columns.Add(plazoInc);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this._unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 6;
                desc.Width = 200;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvIncorporacion.Columns.Add(desc);
                #endregion
                #region Datos Movimientos
                //Tipo Doc
                GridColumn tipoDocMov = new GridColumn();
                tipoDocMov.FieldName = this._unboundPrefix + "DocumentoID";
                tipoDocMov.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoDocumento");
                tipoDocMov.UnboundType = UnboundColumnType.Integer;
                tipoDocMov.VisibleIndex = 1;
                tipoDocMov.Width = 80;
                tipoDocMov.Visible = true;
                tipoDocMov.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(tipoDocMov);

                //Pref Doc
                GridColumn prefDocMvto = new GridColumn();
                prefDocMvto.FieldName = this._unboundPrefix + "PrefDoc";
                prefDocMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                prefDocMvto.UnboundType = UnboundColumnType.String;
                prefDocMvto.VisibleIndex = 2;
                prefDocMvto.Width = 85;
                prefDocMvto.Visible = true;
                prefDocMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(prefDocMvto);

                //ComprobanteMvto
                GridColumn ComprobanteMvto = new GridColumn();
                ComprobanteMvto.FieldName = this._unboundPrefix + "Comprobante";
                ComprobanteMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Comprobante");
                ComprobanteMvto.UnboundType = UnboundColumnType.String;
                ComprobanteMvto.VisibleIndex = 3;
                ComprobanteMvto.Width = 85;
                ComprobanteMvto.Visible = true;
                ComprobanteMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(ComprobanteMvto);

                //Fecha Aplica
                GridColumn fechaDocMvto = new GridColumn();
                fechaDocMvto.FieldName = this._unboundPrefix + "FechaDoc";
                fechaDocMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                fechaDocMvto.UnboundType = UnboundColumnType.DateTime;
                fechaDocMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaDocMvto.AppearanceCell.Options.UseTextOptions = true;
                fechaDocMvto.VisibleIndex = 4;
                fechaDocMvto.Width = 80;
                fechaDocMvto.Visible = true;
                fechaDocMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(fechaDocMvto);

                //Fecha Aplica
                GridColumn fechaAplica = new GridColumn();
                fechaAplica.FieldName = this._unboundPrefix + "FechaAplica";
                fechaAplica.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaAplica");
                fechaAplica.UnboundType = UnboundColumnType.DateTime;
                fechaAplica.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaAplica.AppearanceCell.Options.UseTextOptions = true;
                fechaAplica.VisibleIndex = 5;
                fechaAplica.Width = 80;
                fechaAplica.Visible = true;
                fechaAplica.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(fechaAplica);

                //Fecha Consigna
                GridColumn fechaConsignaMvto = new GridColumn();
                fechaConsignaMvto.FieldName = this._unboundPrefix + "FechaConsigna";
                fechaConsignaMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaConsigna");
                fechaConsignaMvto.UnboundType = UnboundColumnType.DateTime;
                fechaConsignaMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaConsignaMvto.AppearanceCell.Options.UseTextOptions = true;
                fechaConsignaMvto.VisibleIndex = 5;
                fechaConsignaMvto.Width = 80;
                fechaConsignaMvto.Visible = true;
                fechaConsignaMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(fechaConsignaMvto);

                //TotalItemMov
                GridColumn TotalItemMov = new GridColumn();
                TotalItemMov.FieldName = this._unboundPrefix + "TotalDocumento";
                TotalItemMov.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalDocumento");
                TotalItemMov.UnboundType = UnboundColumnType.Integer;
                TotalItemMov.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                TotalItemMov.AppearanceCell.Options.UseTextOptions = true;
                TotalItemMov.VisibleIndex = 6;
                TotalItemMov.Width = 120;
                TotalItemMov.Visible = true;
                TotalItemMov.ColumnEdit = this.editValue;
                TotalItemMov.Summary.Add(DevExpress.Data.SummaryItemType.Sum, TotalItemMov.FieldName, "{0:c0}");
                TotalItemMov.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(TotalItemMov);

                //VlrCapital
                GridColumn VlrCapitalCuotaMov = new GridColumn();
                VlrCapitalCuotaMov.FieldName = this._unboundPrefix + "VlrCapital";
                VlrCapitalCuotaMov.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                VlrCapitalCuotaMov.UnboundType = UnboundColumnType.Integer;
                VlrCapitalCuotaMov.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrCapitalCuotaMov.AppearanceCell.Options.UseTextOptions = true;
                VlrCapitalCuotaMov.VisibleIndex = 6;
                VlrCapitalCuotaMov.Width = 120;
                VlrCapitalCuotaMov.Visible = true;
                VlrCapitalCuotaMov.ColumnEdit = this.editValue;
                VlrCapitalCuotaMov.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrCapitalCuotaMov.FieldName, "{0:c0}");
                VlrCapitalCuotaMov.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrCapitalCuotaMov);

                //VlrInteres
                GridColumn VlrInteresMvto = new GridColumn();
                VlrInteresMvto.FieldName = this._unboundPrefix + "VlrInteres";
                VlrInteresMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                VlrInteresMvto.UnboundType = UnboundColumnType.Integer;
                VlrInteresMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrInteresMvto.AppearanceCell.Options.UseTextOptions = true;
                VlrInteresMvto.VisibleIndex = 7;
                VlrInteresMvto.Width = 120;
                VlrInteresMvto.Visible = true;
                VlrInteresMvto.ColumnEdit = this.editValue;
                VlrInteresMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrInteresMvto.FieldName, "{0:c0}");
                VlrInteresMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrInteresMvto);

                //VlrSeguro
                GridColumn VlrSeguroCuotaMov = new GridColumn();
                VlrSeguroCuotaMov.FieldName = this._unboundPrefix + "VlrSeguro";
                VlrSeguroCuotaMov.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                VlrSeguroCuotaMov.UnboundType = UnboundColumnType.Integer;
                VlrSeguroCuotaMov.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrSeguroCuotaMov.AppearanceCell.Options.UseTextOptions = true;
                VlrSeguroCuotaMov.VisibleIndex = 8;
                VlrSeguroCuotaMov.Width = 120;
                VlrSeguroCuotaMov.Visible = true;
                VlrSeguroCuotaMov.ColumnEdit = this.editValue;
                VlrSeguroCuotaMov.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSeguroCuotaMov.FieldName, "{0:c0}");
                VlrSeguroCuotaMov.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrSeguroCuotaMov);

                //VlrOtrCuota
                GridColumn VlrOtrosFijosCuotaMov = new GridColumn();
                VlrOtrosFijosCuotaMov.FieldName = this._unboundPrefix + "VlrOtrCuota";
                VlrOtrosFijosCuotaMov.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosFijos");
                VlrOtrosFijosCuotaMov.UnboundType = UnboundColumnType.Integer;
                VlrOtrosFijosCuotaMov.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrOtrosFijosCuotaMov.AppearanceCell.Options.UseTextOptions = true;
                VlrOtrosFijosCuotaMov.VisibleIndex = 9;
                VlrOtrosFijosCuotaMov.Width = 100;
                VlrOtrosFijosCuotaMov.Visible = true;
                VlrOtrosFijosCuotaMov.ColumnEdit = this.editValue;
                VlrOtrosFijosCuotaMov.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrOtrosFijosCuotaMov.FieldName, "{0:c0}");
                VlrOtrosFijosCuotaMov.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrOtrosFijosCuotaMov);

                //VlrMora
                GridColumn VlrMoraMvto = new GridColumn();
                VlrMoraMvto.FieldName = this._unboundPrefix + "VlrMora";
                VlrMoraMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMoraPago");
                VlrMoraMvto.UnboundType = UnboundColumnType.Integer;
                VlrMoraMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrMoraMvto.AppearanceCell.Options.UseTextOptions = true;
                VlrMoraMvto.VisibleIndex = 10;
                VlrMoraMvto.Width = 100;
                VlrMoraMvto.Visible = true;
                VlrMoraMvto.ColumnEdit = this.editValue;
                VlrMoraMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrMoraMvto.FieldName, "{0:c0}");
                VlrMoraMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrMoraMvto);

                //VlrPJ
                GridColumn vlrPJMvto = new GridColumn();
                vlrPJMvto.FieldName = this._unboundPrefix + "VlrPrejuridico";
                vlrPJMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrPrejuridicoPago");
                vlrPJMvto.UnboundType = UnboundColumnType.Integer;
                vlrPJMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPJMvto.AppearanceCell.Options.UseTextOptions = true;
                vlrPJMvto.VisibleIndex = 11;
                vlrPJMvto.Width = 100;
                vlrPJMvto.Visible = true;
                vlrPJMvto.ColumnEdit = this.editValue;
                vlrPJMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, vlrPJMvto.FieldName, "{0:c0}");
                vlrPJMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(vlrPJMvto);

                //VlrGastos
                GridColumn VlrGastosMvto = new GridColumn();
                VlrGastosMvto.FieldName = this._unboundPrefix + "VlrGastos";
                VlrGastosMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrGastos");
                VlrGastosMvto.UnboundType = UnboundColumnType.Integer;
                VlrGastosMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrGastosMvto.AppearanceCell.Options.UseTextOptions = true;
                VlrGastosMvto.VisibleIndex = 12;
                VlrGastosMvto.Width = 100;
                VlrGastosMvto.Visible = true;
                VlrGastosMvto.ColumnEdit = this.editValue;
                VlrGastosMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrGastosMvto.FieldName, "{0:c0}");
                VlrGastosMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(VlrGastosMvto);

                //VlrExtraMvto
                GridColumn VlrExtraMvto = new GridColumn();
                VlrExtraMvto.FieldName = this._unboundPrefix + "VlrExtra";
                VlrExtraMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrExtra");
                VlrExtraMvto.UnboundType = UnboundColumnType.Integer;
                VlrExtraMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrExtraMvto.AppearanceCell.Options.UseTextOptions = true;
                VlrExtraMvto.VisibleIndex = 13;
                VlrExtraMvto.Width = 100;
                VlrExtraMvto.Visible = true;
                VlrExtraMvto.ColumnEdit = this.editValue;
                VlrExtraMvto.OptionsColumn.AllowEdit = false;
                VlrExtraMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrExtraMvto.FieldName, "{0:c0}");
                this.gvMovimientos.Columns.Add(VlrExtraMvto);

                //SdoFavorMvto
                GridColumn SdoFavorMvto = new GridColumn();
                SdoFavorMvto.FieldName = this._unboundPrefix + "SdoFavor";
                SdoFavorMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SdoFavor");
                SdoFavorMvto.UnboundType = UnboundColumnType.Integer;
                SdoFavorMvto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                SdoFavorMvto.AppearanceCell.Options.UseTextOptions = true;
                SdoFavorMvto.VisibleIndex = 14;
                SdoFavorMvto.Width = 100;
                SdoFavorMvto.Visible = true;
                SdoFavorMvto.ColumnEdit = this.editValue;
                SdoFavorMvto.Summary.Add(DevExpress.Data.SummaryItemType.Sum, SdoFavorMvto.FieldName, "{0:c0}");
                SdoFavorMvto.OptionsColumn.AllowEdit = false;
                this.gvMovimientos.Columns.Add(SdoFavorMvto);

                //Ver Doc
                GridColumn fileMvto = new GridColumn();
                fileMvto.FieldName = this._unboundPrefix + "FileUrl";
                fileMvto.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SearchDocument");
                fileMvto.UnboundType = UnboundColumnType.String;
                fileMvto.Width = 40;
                fileMvto.VisibleIndex = 15;
                fileMvto.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                fileMvto.Visible = true;
                fileMvto.OptionsColumn.AllowEdit = true;
                fileMvto.ColumnEdit = this.linkEditViewFile;
                fileMvto.AppearanceCell.ForeColor = Color.Blue;
                this.gvMovimientos.Columns.Add(fileMvto);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            this.validate = false;

            //Header
            this.txtLibranza.Text = string.Empty;
            this.txtOferta.Text = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.masterCompradorCartera.Value = string.Empty;
            this.masterPagaduria.Value = string.Empty;
            this.masterLineaCredito.Value = string.Empty;
            this.masterTipoCredito.Value = string.Empty;
            this.masterAsesor.Value = string.Empty;
            this.masterCentroPago.Value = string.Empty;
            this.masterZona.Value = string.Empty;
            this.masterGestionCobranza.Value = string.Empty;
            this.masterEstadoCobranza.Value = string.Empty;
            this.txtVlrCredito.EditValue = 0;
            this.txtVlrLibranza.EditValue = 0;
            this.txtVlrGiro.EditValue = 0;
            this.txtValorCuota.EditValue = 0;
            this.txtPorInteres.EditValue = 0;
            this.txtPorSeguro.EditValue = 0;
            this.txtPlazo.EditValue = 0;
            this.chkVerTodos.Checked = false;

            //Footer
            this.credito = new DTO_ccCreditoDocu();
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.creditosCancelados = new List<DTO_ccCreditoDocu>();
            this.gcGenerales.DataSource = this.creditos;

            //Variables
            this.clienteID = string.Empty;
            this.libranza = 0;
            this.validate = true;
            this.datosGenerales = new Dictionary<int, DTO_ccCreditoDocu>();
            this.datosCartera = new Dictionary<int, DTO_InfoCredito>();
            this.datosCesion = new Dictionary<int, DTO_VentaCartera>();
            this.datosPagos = new Dictionary<int, DTO_InfoPagos>();

            this.EnableTabs(false);

        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void GetValues(DTO_ccCreditoDocu credito)
        {
            try
            {
                this.dtFechaLiquida.DateTime = credito.FechaLiquida.Value.Value;
                this.dtFechaCorte.DateTime = DateTime.Now;
                this.masterPagaduria.Value = credito.PagaduriaID.Value;
                this.masterLineaCredito.Value = credito.LineaCreditoID.Value;
                this.masterTipoCredito.Value = credito.TipoCreditoID.Value;
                this.masterAsesor.Value = credito.AsesorID.Value;
                this.masterCentroPago.Value = credito.CentroPagoID.Value;
                this.masterZona.Value = credito.ZonaID.Value;
                this.masterGestionCobranza.Value = credito.CobranzaGestionCierre.Value;
                this.masterEstadoCobranza.Value = credito.CobranzaEstadoID.Value;
                this.txtVlrCredito.EditValue = credito.VlrPrestamo.Value;
                this.txtVlrLibranza.EditValue = credito.VlrLibranza.Value;
                this.txtVlrGiro.EditValue = credito.VlrGiro.Value;
                this.txtValorCuota.EditValue = credito.VlrCuota.Value;
                this.txtPorInteres.EditValue = credito.PorInteres.Value;
                this.txtPorSeguro.EditValue = credito.PorSeguro.Value;
                this.txtPlazo.EditValue = credito.Plazo.Value;
                this.cmbTipoEstado.EditValue = credito.TipoEstado.Value.ToString();
                this.cmbEstadoDeuda.EditValue = credito.EstadoDeuda.Value.ToString();
                this.chkCanceladoInd.Checked = credito.CanceladoInd.Value.Value;
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "GetValues"));
            }
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageData(int page)
        {
            try
            {
                int libranza = this.selectedCredito.Libranza.Value.Value;
                int numDoc = this.selectedCredito.NumeroDoc.Value.Value;
                switch (page)
                {
                    #region Datos Generales
                    case 0:
                        break;
                    #endregion
                    #region Datos Cartera
                    case 1:
                        if (this.datosCartera[libranza] == null)
                        {
                            numDoc = this.selectedCredito.NumeroDoc.Value.Value;
                            DateTime fecha = DateTime.Now.Date;//  this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Calcula los valores
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;
                                    pp.VlrMoraLiquida.Value = pp.VlrMoraPago.Value;
                                    pp.VlrPrejuridico.Value = 0;

                                    pp.DiasMora.Value = (this.dtFechaCorte.DateTime - pp.FechaCuota.Value.Value).Days;
                                    pp.DiasMora.Value = pp.DiasMora.Value < 0 || pp.VlrSaldo.Value == 0 ? 0 : pp.DiasMora.Value;

                                    //Validación financiera
                                    if (this.sector == SectorCartera.Financiero)
                                        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;
                                }
                                #endregion
                                #region Carga la informacion del header
                                this.txtLibranza1.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterCliente1.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSaldoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                                this.txtSaldoNoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date > this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                                this.txtSaldoTotalPP.EditValue = Convert.ToInt64(this.txtSaldoVencidoPP.EditValue) + Convert.ToInt64(this.txtSaldoNoVencidoPP.EditValue);
                                this.txtCtasPendientes.Text = (from c in this.infoCredito.PlanPagos where c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                                this.txtCtasVencidas.Text = (from c in this.infoCredito.PlanPagos where c.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date && c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                                //this.txtSldoCapital.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                //this.txtSaldoLibranza.EditValue = Convert.ToInt32((from c in this.infoCredito.PlanPagos select c.VlrSaldo.Value.Value).Sum());
                                this.masterZona.Value = this.selectedCredito.ZonaID.Value;
                                this.txtPlazo.EditValue = this.selectedCredito.Plazo.Value;
                                this.txtPorInteres.EditValue = this.selectedCredito.PorInteres.Value;
                                this.txtPorSeguro.EditValue = this.selectedCredito.PorSeguro.Value;
                                this.txtValorCuota.EditValue = this.selectedCredito.VlrCuota.Value;
                                #endregion
                                #region Carga la informacion de la grilla
                                this.gcCartera.DataSource = this.infoCredito.PlanPagos;
                                this.gcCartera.RefreshDataSource();
                                this.gvCartera.BestFitColumns();
                                this.gvCartera.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoCartera), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.infoCredito = this.datosCartera[libranza];
                            #region Carga la informacion del header
                            this.txtLibranza1.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterCliente1.Value = this.selectedCredito.ClienteID.Value;
                            this.txtSaldoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                            this.txtSaldoNoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date > this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                            this.txtSaldoTotalPP.EditValue = Convert.ToInt64(this.txtSaldoVencidoPP.EditValue) + Convert.ToInt64(this.txtSaldoNoVencidoPP.EditValue);
                            this.txtCtasPendientes.Text = (from c in this.infoCredito.PlanPagos where c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                            this.txtCtasVencidas.Text = (from c in this.infoCredito.PlanPagos where c.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date && c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                            this.masterZona.Value = this.selectedCredito.ZonaID.Value;
                            this.txtPlazo.EditValue = this.selectedCredito.Plazo.Value;
                            this.txtPorInteres.EditValue = this.selectedCredito.PorInteres.Value;
                            this.txtPorSeguro.EditValue = this.selectedCredito.PorSeguro.Value;
                            this.txtValorCuota.EditValue = this.selectedCredito.VlrCuota.Value;
                            #endregion
                            #region Carga la infromacion de la grilla
                            this.gcCartera.DataSource = this.infoCredito.PlanPagos;
                            this.gvCartera.BestFitColumns();
                            this.gcCartera.RefreshDataSource();
                            this.gvCartera.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    #region Datos Cuota Pagos
                    case 2:
                        if (this.datosCartera[libranza] == null)
                        {
                            numDoc = this.selectedCredito.NumeroDoc.Value.Value;
                            DateTime fecha = this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Calcula los valores
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;

                                    //Validación financiera
                                    if (this.sector == SectorCartera.Financiero)
                                        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;
                                }
                                #endregion
                                #region Carga la informacion del header
                                this.txtLibranzaCuota.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterClienteCuota.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSaldoVencidoCuota.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                                this.txtSaldoNoVencidoCuota.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date > this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                                this.txtSaldoTotalCuota.EditValue = Convert.ToInt64(this.txtSaldoVencidoCuota.EditValue) + Convert.ToInt64(this.txtSaldoNoVencidoCuota.EditValue);
                                this.txtCtasPendientesCuota.Text = (from c in this.infoCredito.PlanPagos where c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                                this.txtCtasVencidasCuota.Text = (from c in this.infoCredito.PlanPagos where c.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date && c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                                #endregion
                                #region Carga la informacion de la grilla
                              
                                if (!this.datosPagos.ContainsKey(libranza) || this.datosPagos[libranza] == null)
                                {
                                    this.infoPagos = this._bc.AdministrationModel.GetInfoPagos(this.infoCredito.PlanPagos[0].NumeroDoc.Value.Value);
                                    this.datosPagos[libranza] = this.infoPagos;
                                }

                                if (this.infoPagos.CreditoPagos == null)
                                      break;

                                //Validación financiera
                                if (this.sector == SectorCartera.Financiero)
                                {
                                    this.infoPagos.CreditoPagos.ForEach(p =>
                                    {
                                        p.VlrSeguro.Value = p.VlrSeguro.Value + p.VlrOtro1.Value;
                                    });
                                }

                                this.gcCuotaPagos.DataSource = this.infoPagos.CreditoPagos;
                                this.gcCuotaPagos.RefreshDataSource();
                                this.gvCuotaPagos.BestFitColumns();
                                this.gvCuotaPagos.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoCartera), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.infoCredito = this.datosCartera[libranza];
                            #region Carga la informacion del header
                            this.txtLibranzaCuota.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterClienteCuota.Value = this.selectedCredito.ClienteID.Value;
                            this.txtSaldoVencidoCuota.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                            this.txtSaldoNoVencidoCuota.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date > this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                            this.txtSaldoTotalCuota.EditValue = Convert.ToInt64(this.txtSaldoVencidoCuota.EditValue) + Convert.ToInt64(this.txtSaldoNoVencidoCuota.EditValue);
                            this.txtCtasPendientesCuota.Text = (from c in this.infoCredito.PlanPagos where c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                            this.txtCtasVencidasCuota.Text = (from c in this.infoCredito.PlanPagos where c.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date && c.VlrSaldo.Value != 0 select c.CuotaID.Value).Count().ToString();
                            #endregion
                            #region Carga la informacion de la grilla

                            this.infoPagos = this._bc.AdministrationModel.GetInfoPagos(this.infoCredito.PlanPagos[0].NumeroDoc.Value.Value);
                            this.datosPagos[libranza] = this.infoPagos;

                            if (this.infoPagos.CreditoPagos == null)
                                break;

                            //Validación financiera
                            if (this.sector == SectorCartera.Financiero)
                            {
                                this.infoPagos.CreditoPagos.ForEach(p =>
                                {
                                    p.VlrSeguro.Value = p.VlrSeguro.Value + p.VlrOtro1.Value;
                                });
                            }                        

                            this.gcCuotaPagos.DataSource = this.infoPagos.CreditoPagos;
                            this.gvCuotaPagos.BestFitColumns();
                            this.gcCuotaPagos.RefreshDataSource();
                            this.gvCuotaPagos.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    #region Datos Cesion
                    case 3:
                        if (this.datosCesion[libranza] == null)
                        {
                            this.ventaCartera = this._bc.AdministrationModel.GetInfoVentaByLibranza(numDoc, libranza);

                            if (this.ventaCartera.VentaDocu != null)
                            {
                                this.datosCesion[libranza] = this.ventaCartera;
                                this.ventaCartera.Creditos = this.creditos;

                                #region Carga la informacion del header
                                this.txtLibranza2.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.dtCesion.DateTime = this.ventaCartera.VentaDocu.FechaVenta.Value.Value;
                                this.masterCliente2.Value = this.selectedCredito.ClienteID.Value;
                                this.masterCompradorCarteraCesion.Value = this.selectedCredito.CompradorCarteraID.Value;
                                this.txtVlrVenta.EditValue = this.ventaCartera.VentaDeta[0].VlrVenta.Value;
                                this.txtOferta1.Text = this.ventaCartera.VentaDocu.Oferta.Value;
                                this.txtPortafolio.Text = this.ventaCartera.VentaDeta[0].Portafolio.Value;
                                this.txtTasaVenta.EditValue = this.ventaCartera.VentaDocu.FactorCesion.Value;
                                this.txtVlrCredito2.EditValue = this.selectedCredito.VlrPrestamo.Value;
                                this.txtVlrLibranza2.EditValue = this.ventaCartera.VentaDeta[0].VlrLibranza.Value;
                                #endregion
                                #region Carga la infromacion de la grilla

                                this.ventaCartera.PlanPagos.ForEach(c => c.VlrCuota.Value = c.VlrCapitalCesion.Value.Value + c.VlrUtilidadCesion.Value.Value);
                                this.gcCesion.DataSource = this.ventaCartera.PlanPagos;
                                this.gcCesion.RefreshDataSource();
                                this.gvCesion.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoVenta), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.ventaCartera = this.datosCesion[libranza];
                            #region Carga la informacion del header
                            this.txtLibranza2.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterCliente2.Value = this.selectedCredito.ClienteID.Value;
                            this.txtVlrVenta.EditValue = this.ventaCartera.VentaDeta[0].VlrVenta.Value;
                            this.txtOferta1.Text = this.ventaCartera.VentaDocu.Oferta.Value;
                            this.txtPortafolio.Text = this.ventaCartera.VentaDeta[0].Portafolio.Value;
                            this.txtTasaVenta.EditValue = this.ventaCartera.VentaDocu.FactorCesion.Value;
                            //this.txtVlrCredito.EditValue = this.selectedCredito.VlrPrestamo.Value;
                            this.txtVlrLibranza2.EditValue = this.ventaCartera.VentaDeta[0].VlrLibranza.Value;
                            #endregion
                            #region Carga la infromacion de la grilla
                            this.gcCesion.DataSource = this.ventaCartera.PlanPagos;
                            this.gcCesion.RefreshDataSource();
                            this.gvCesion.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    #region Datos Componentes
                    case 4:
                        List<DTO_ccSaldosComponentes> saldosComps = new List<DTO_ccSaldosComponentes>();
                        if (this.datosCartera[libranza] == null)
                        {
                            DateTime fecha = this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Calcula los valores
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;

                                    //Validación financiera
                                    if (this.sector == SectorCartera.Financiero)
                                        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;
                                }
                                #endregion
                                saldosComps = this._bc.AdministrationModel.GetSaldoCuentasForCredito(numDoc, this.selectedCredito.ClienteID.Value);

                                #region Carga la informacion del header
                                this.txtLibranzaComponentes.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterCliente3.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSaldoCuentas.EditValue = saldosComps.Sum(s => s.TotalSaldo.Value.Value);
                                #endregion
                                #region Carga la infromacion de la grilla
                                this.gcComponentes.DataSource = saldosComps;
                                this.gcComponentes.RefreshDataSource();
                                this.gvComponentes.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoCartera), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.infoCredito = this.datosCartera[libranza];
                            saldosComps = this._bc.AdministrationModel.GetSaldoCuentasForCredito(numDoc, this.selectedCredito.ClienteID.Value);
                            #region Carga la informacion del header
                            this.txtLibranzaComponentes.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterCliente3.Value = this.selectedCredito.ClienteID.Value;
                            this.txtSaldoCuentas.EditValue = saldosComps.Sum(s => s.TotalSaldo.Value.Value);
                            #endregion
                            #region Carga la infromacion de la grilla
                            this.gcComponentes.DataSource = saldosComps;
                            this.gcCuotaPagos.RefreshDataSource();
                            this.gvCuotaPagos.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    #region Datos Incorporaciones
                    case 5:
                        List<DTO_ccIncorporacionDeta> incorp = new List<DTO_ccIncorporacionDeta>();
                        if (this.datosCartera[libranza] == null)
                        {
                            DateTime fecha = this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Calcula los valores
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;

                                    //Validación financiera
                                    if (this.sector == SectorCartera.Financiero)
                                        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;
                                }
                                #endregion
                                incorp = this._bc.AdministrationModel.IncorporacionCredito_GetByNumDocCred(numDoc);
                                #region Carga la informacion del header
                                this.txtLibranzaIncorp.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterClienteIncorp.Value = this.selectedCredito.ClienteID.Value;
                                //this.txtSaldoCuentas.EditValue = incorp.Sum(s => s.TotalSaldo.Value.Value);
                                #endregion
                                #region Carga la infromacion de la grilla
                                this.gcIncorporacion.DataSource = incorp;
                                this.gcIncorporacion.RefreshDataSource();
                                this.gvIncorporacion.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoCartera), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.infoCredito = this.datosCartera[libranza];
                            incorp = this._bc.AdministrationModel.IncorporacionCredito_GetByNumDocCred(numDoc);
                            #region Carga la informacion del header
                            this.txtLibranzaIncorp.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterClienteIncorp.Value = this.selectedCredito.ClienteID.Value;
                            //this.txtSaldoCuentas.EditValue = saldosComps.Sum(s => s.TotalSaldo.Value.Value);
                            #endregion
                            #region Carga la infromacion de la grilla
                            this.gcIncorporacion.DataSource = incorp;
                            this.gcIncorporacion.RefreshDataSource();
                            this.gvIncorporacion.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    #region Datos Movimientos Cart
                    case 6:
                        if (this.datosCartera[libranza] == null)
                        {
                            numDoc = this.selectedCredito.NumeroDoc.Value.Value;
                            DateTime fecha = this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Calcula los valores
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;

                                    //Validación financiera
                                    if (this.sector == SectorCartera.Financiero)
                                        pp.VlrSeguro.Value = pp.VlrSeguro.Value + pp.VlrOtro1.Value;
                                }
                                #endregion
                                #region Carga la informacion del header
                                this.txtLibranzaMov.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterClienteMov.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSdoCapitalMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtSdoLibranzaMov.EditValue = Convert.ToInt32((from c in this.infoCredito.PlanPagos select c.VlrSaldo.Value.Value).Sum());
                                this.txtMoraCapitalMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtMoraLibranzaMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                                this.txtVlrCreditoMov.EditValue = this.selectedCredito.VlrPrestamo.Value;
                                this.txtVlrLibranzaMov.EditValue = this.selectedCredito.VlrLibranza.Value;
                                this.txtCtasPendMov.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                                this.txtCtasMoraMov.Text = this.consultaCredito.CuotasMora.Value.ToString();
                                #endregion
                                #region Carga la informacion de la grilla
                                 List<DTO_QueryCarteraMvto> mvtos = this._bc.AdministrationModel.CarteraMvto_QueryByLibranza(libranza).OrderBy(x=>x.NumeroDoc.Value).ToList();
                                this.gcMovimientos.DataSource = mvtos;
                                this.gcMovimientos.RefreshDataSource();
                                this.gvMovimientos.BestFitColumns();
                                this.gvMovimientos.MoveFirst();
                                #endregion
                            }
                            else
                            {
                                string msg = String.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_QueryLibranza_NoInfoCartera), libranza);
                                MessageBox.Show(msg);
                            }
                        }
                        else
                        {
                            this.infoCredito = this.datosCartera[libranza];
                            #region Carga la informacion del header
                            this.txtLibranzaMov.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterClienteMov.Value = this.selectedCredito.ClienteID.Value;
                            this.txtSdoCapitalMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtSdoLibranzaMov.EditValue = Convert.ToInt32((from c in this.infoCredito.PlanPagos select c.VlrSaldo.Value.Value).Sum());
                            this.txtMoraCapitalMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtMoraLibranzaMov.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                            this.txtVlrCreditoMov.EditValue = this.selectedCredito.VlrPrestamo.Value;
                            this.txtVlrLibranzaMov.EditValue = this.selectedCredito.VlrLibranza.Value;
                            this.txtCtasPendMov.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                            this.txtCtasMoraMov.Text = this.consultaCredito.CuotasMora.Value.ToString();
                            #endregion
                            #region Carga la informacion de la grilla
                            List<DTO_QueryCarteraMvto> mvtos = this._bc.AdministrationModel.CarteraMvto_QueryByLibranza(libranza).OrderBy(x => x.NumeroDoc.Value).ToList();
                            this.gcMovimientos.DataSource = mvtos;
                            this.gcMovimientos.RefreshDataSource();
                            this.gvMovimientos.BestFitColumns();
                            this.gvMovimientos.MoveFirst();
                            #endregion
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "LoadPageData"));
            }
        }

        /// <summary>
        /// Funcion que realiza la opreacion de busqueda
        /// </summary>
        private void GetSearch()
        {
            this.newSearch = true;
            #region Busca por libranza
            if (rbLibranza.Checked && !String.IsNullOrEmpty(this.txtLibranza.Text))
            {
                #region Limpia los filtros
                this.masterCliente.Value = string.Empty;
                this.txtOferta.Text = string.Empty;
                this.masterCompradorCartera.Value = string.Empty;
                #endregion
                this.libranza = Convert.ToInt32(this.txtLibranza.Text);
                this.credito = this._bc.AdministrationModel.GetCreditoByLibranza(this.libranza);
                if (this.credito != null)
                    this.credito = this.credito.Estado.Value != (byte)EstadoDocControl.Revertido ? this.credito : null;
                if (this.credito != null)
                {                   
                    this.consultaCredito = this._bc.AdministrationModel.GetCreditoByLibranzaAndFechaCorte(this.libranza, this.credito.NumeroDoc.Value.Value,DateTime.Now);
                    if (this.consultaCredito != null)
                    {
                        if (!this.datosGenerales.ContainsKey(this.libranza))
                        {
                            this.datosGenerales.Add(this.libranza, this.consultaCredito);
                            this.datosCartera.Add(this.libranza, null);
                            this.datosCesion.Add(this.libranza, null);
                            this.datosPagos.Add(this.libranza, null);

                            this.EnableTabs(true);
                            this.creditos.Add(this.consultaCredito);
                            this.creditos = this.creditos.FindAll(x=>x.Estado.Value != (byte)EstadoDocControl.Revertido);
                            this.gcGenerales.DataSource = this.creditos;
                            this.gvGenerales.BestFitColumns();
                        }                      
                        this.gcGenerales.RefreshDataSource();
                        this.gvGenerales.MoveFirst();
                    }
                    else
                    {
                        this.EnableTabs(false);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }
                }
                else
                {
                    this.EnableTabs(false);
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    this.CleanData();
                }
            }
            #endregion
            #region Busca por cliente
            else if (this.rbCliente.Checked && this.masterCliente.ValidID)
            {
                #region Limpia los filtros
                this.txtLibranza.Text = string.Empty;
                this.txtOferta.Text = string.Empty;
                this.masterCompradorCartera.Value = string.Empty;
                #endregion
                this.clienteID = this.masterCliente.Value;
                this.clienteID = this.masterCliente.Value;
                var creditosAll = this._bc.AdministrationModel.GetCreditoByClienteAndFecha(this.clienteID, DateTime.Now, false, false);
                this.creditos = creditosAll.FindAll(x => x.Estado.Value != (byte)EstadoDocControl.Revertido && !x.CanceladoInd.Value.Value);
                this.creditosCancelados = creditosAll.FindAll(x => x.Estado.Value != (byte)EstadoDocControl.Revertido && x.CanceladoInd.Value.Value);
             
                if (this.creditos.Count != 0)
                {
                    if (this.creditos != null)
                    {
                        this.EnableTabs(true);
                        foreach (DTO_ccCreditoDocu creditosCli in this.creditos)
                        {
                            this.libranza = creditosCli.Libranza.Value.Value;
                            if (!this.datosGenerales.ContainsKey(this.libranza))
                            {
                                this.datosGenerales.Add(this.libranza, creditosCli);
                                this.datosCartera.Add(this.libranza, null);
                                this.datosCesion.Add(this.libranza, null);
                                this.datosPagos.Add(this.libranza, null);
                            }
                        }                        
                        this.gcGenerales.DataSource = this.creditos;
                        this.gcGenerales.RefreshDataSource();
                        this.gvGenerales.BestFitColumns();
                        this.gvGenerales.MoveFirst();
                    }
                    else
                    {
                        this.EnableTabs(false);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }
                }
                else
                {
                    this.EnableTabs(false);
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_NoCredByCliente), this.masterCliente.Value);
                    MessageBox.Show(msg);
                    if (this.creditosCancelados.Count > 0)
                    {
                        this.EnableTabs(true);
                        //this.creditos.AddRange(this.creditosCancelados);
                        this.chkVerTodos.Checked = true;
                    }
                    else
                        this.CleanData();
                }
            }
            #endregion
            #region Busca por comprador de cartera
            else if (rbCompCartera.Checked && this.masterCompradorCartera.ValidID)
            {
                #region Limpia los filtros
                this.masterCliente.Value = string.Empty;
                this.txtOferta.Text = string.Empty;
                this.txtLibranza.Text = string.Empty;
                #endregion
                this.compradorCarteraID = this.masterCompradorCartera.Value;
                var creditosAll = this._bc.AdministrationModel.GetCreditoByCompradorCartera(this.compradorCarteraID);
                this.creditos = creditosAll.FindAll(x => x.Estado.Value != (byte)EstadoDocControl.Revertido && !x.CanceladoInd.Value.Value);
                this.creditosCancelados = creditosAll.FindAll(x => x.Estado.Value != (byte)EstadoDocControl.Revertido && x.CanceladoInd.Value.Value);
                if (this.creditos != null)
                {
                    this.EnableTabs(true);
                    foreach (DTO_ccCreditoDocu credito in this.creditos)
                    {
                        this.libranza = credito.Libranza.Value.Value;
                        if (!this.datosGenerales.ContainsKey(this.libranza))
                        {
                            this.datosGenerales.Add(this.libranza, credito);
                            this.datosCartera.Add(this.libranza, null);
                            this.datosCesion.Add(this.libranza, null);
                            this.datosPagos.Add(this.libranza, null);
                        }
                    }                    
                    this.gcGenerales.DataSource = this.creditos;
                    this.gcGenerales.RefreshDataSource();
                    this.gvGenerales.BestFitColumns();
                    this.gvGenerales.MoveFirst();
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                    this.CleanData();
                    this.EnableTabs(false);
                }
            }
            #endregion
            #region Busca por oferta
            else if (rbOferta.Checked && !String.IsNullOrEmpty(this.txtOferta.Text))
            {

            }
            #endregion
            else
            {
                this.EnableTabs(false);
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidSearchCriteria));
                return;
            }
        }

        /// <summary>
        /// Funcion que habilita o deshabilita las pestañas
        /// </summary>
        /// <param name="enable"></param>
        private void EnableTabs(bool enable)
        {
            this.tc_QueryCreditos.TabPages[1].PageEnabled = enable;//Datos Datos plan pagos
            this.tc_QueryCreditos.TabPages[2].PageEnabled = enable;//Datos Cuotas Pagos
            this.tc_QueryCreditos.TabPages[3].PageEnabled = enable;//Datos Cesion
            this.tc_QueryCreditos.TabPages[4].PageEnabled = enable;//Datos Cuentas Componentes
            if(this.sector != SectorCartera.Financiero)
                this.tc_QueryCreditos.TabPages[5].PageEnabled = enable; //Incorporaciones
            else
                this.tc_QueryCreditos.TabPages[5].PageVisible = false;
            this.tc_QueryCreditos.TabPages[6].PageEnabled = enable; //Movimientos
        }

        /// <summary>
        /// Funcion que se encarga de habilitar los controles
        /// </summary>
        private void EnableControls()
        {
            this.txtVlrGiro.Enabled = true;
            this.txtVlrCredito.Enabled = true;
            this.lblVlrLibranza.Enabled = true;
            #region Bloquea los controles que no se este utilizando
            if (this.rbLibranza.Checked)
            {
                this.masterCliente.Enabled = false;
                this.txtOferta.Enabled = false;
                this.masterCompradorCartera.Enabled = false;
            }
            else if (this.rbCliente.Checked)
            {
                this.txtLibranza.Enabled = false;
                this.txtOferta.Enabled = false;
                this.masterCompradorCartera.Enabled = false;
            }
            else if (this.rbOferta.Checked)
            {
                this.txtLibranza.Enabled = false;
                this.masterCliente.Enabled = false;
                this.masterCompradorCartera.Enabled = false;
            }
            else
            {
                this.txtLibranza.Enabled = false;
                this.masterCliente.Enabled = false;
                this.txtOferta.Enabled = false;
            } 
            #endregion
        }

        #endregion Funciones Privadas

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemSave.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta cuando se cambia de pestaña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tc_QueryCreditos_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int index = tc_QueryCreditos.SelectedTabPageIndex;
            this.LoadPageData(index);
        }

        /// <summary>
        /// Evento que habilita los controles de acuerdo al filtro escogido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbLibranza_CheckedChanged(object sender, EventArgs e)
        {
            this.txtLibranza.Enabled = true;
            this.EnableControls();
            this.CleanData();
        }

        /// <summary>
        /// Evento que habilita los controles de acuerdo al filtro escogido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCliente_CheckedChanged(object sender, EventArgs e)
        {
            this.CleanData();
            this.masterCliente.Enabled = true;
            this.EnableControls();
        }

        /// <summary>
        /// Evento que habilita los controles de acuerdo al filtro escogido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbOferta_CheckedChanged(object sender, EventArgs e)
        {
            this.CleanData();
            this.txtOferta.Enabled = true;
            this.EnableControls();
        }

        /// <summary>
        /// Evento que habilita los controles de acuerdo al filtro escogido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCompCartera_CheckedChanged(object sender, EventArgs e)
        {
            this.CleanData();
            this.masterCompradorCartera.Enabled = true;
            this.EnableControls();
        }

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento/param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta al consultar una poliza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPoliza_Click(object sender, EventArgs e)
        {
            ModalPolizasCartera cot = new ModalPolizasCartera(string.Empty, this.libranza.ToString());
            cot.ShowDialog();
        }

        /// <summary>
        /// Obtiene el saldo actual del credito con fecha de corte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCorte_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.selectedCredito != null)
                {
                    #region Carga Saldos
                    this.infoCredito = this._bc.AdministrationModel.GetSaldoCredito(this.selectedCredito.NumeroDoc.Value.Value, this.dtFechaCorte.DateTime.Date, true, true, true, false);
                    if (this.infoCredito != null)
                    {
                        foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                        {
                            if (pp.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date)
                            {
                                pp.VlrCapital.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteCapital).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrInteres.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteInteres).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrSeguro.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && (x.ComponenteCarteraID.Value == this.componenteSeguro || x.ComponenteCarteraID.Value == this.componenteIntSeguro)).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrMoraLiquida.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteMora).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrPrejuridico.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componentePrejuridico).Sum(y => y.CuotaSaldo.Value);
                            }
                            else
                            {
                                pp.VlrCapital.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteCapital && x.PagoTotalInd.Value.Value).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrInteres.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteInteres && x.PagoTotalInd.Value.Value).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrSeguro.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && (x.ComponenteCarteraID.Value == this.componenteSeguro || x.ComponenteCarteraID.Value == this.componenteIntSeguro) && x.PagoTotalInd.Value.Value).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrMoraLiquida.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componenteMora && x.PagoTotalInd.Value.Value).Sum(y => y.CuotaSaldo.Value);
                                pp.VlrPrejuridico.Value = this.infoCredito.SaldosComponentes.FindAll(x => x.CuotaID.Value == pp.CuotaID.Value && x.ComponenteCarteraID.Value == this.componentePrejuridico && x.PagoTotalInd.Value.Value).Sum(y => y.CuotaSaldo.Value);
                            }

                            pp.DiasMora.Value = (this.dtFechaCorte.DateTime - pp.FechaCuota.Value.Value).Days;
                            pp.DiasMora.Value = pp.DiasMora.Value < 0 || pp.VlrSaldo.Value == 0 ? 0 : pp.DiasMora.Value;
                        }
                        this.txtSaldoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);// Convert.ToInt32((from c in info.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                        this.txtSaldoNoVencidoPP.EditValue = this.infoCredito.PlanPagos.FindAll(x => x.FechaCuota.Value.Value.Date > this.dtFechaCorte.DateTime.Date).Sum(x => x.VlrSaldo.Value);
                        this.txtSaldoTotalPP.EditValue = Convert.ToInt64(this.txtSaldoVencidoPP.EditValue) + Convert.ToInt64(this.txtSaldoNoVencidoPP.EditValue);
                        this.txtCtasPendientes.Text = (from c in this.infoCredito.PlanPagos select c.CuotaID.Value).Count().ToString();
                        this.txtCtasVencidas.Text = (from c in this.infoCredito.PlanPagos where c.FechaCuota.Value.Value.Date <= this.dtFechaCorte.DateTime.Date select c.CuotaID.Value).Count().ToString();
                    }
                    #endregion
                    #region Carga la informacion de la grilla
                    this.gcCartera.DataSource = this.infoCredito.PlanPagos;
                    this.gcCartera.RefreshDataSource();
                    this.gvCartera.BestFitColumns();
                    this.gvCartera.MoveFirst();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "btnGetCorte_Click"));
            }
        }

        /// <summary>
        /// Obtiene el saldo actual del credito con fecha de corte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGestionCobranza_Click(object sender, EventArgs e)
        {
            try
            {
                Type frm = typeof(GestionCobranza);
                if (this.selectedCredito != null && !string.IsNullOrEmpty(this.selectedCredito.ClienteID.Value))
                    FormProvider.GetInstance(frm, new object[] { this.selectedCredito.ClienteID.Value, this._frmModule.ToString() });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "btnGestionCobranza_Click")); ;
            }
        }

        /// <summary>
        /// Obtiene datos del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdicionales_Click(object sender, EventArgs e)
        {
            if (this.selectedCredito != null)
            {
                ModalInfoCredito mod = new ModalInfoCredito(this.selectedCredito.ClienteID.Value, this.selectedCredito);
                mod.ShowDialog();
            }
        }

        /// <summary>
        /// Muestra todos los creditos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkVerTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkVerTodos.Checked)
            {
                this.creditos.AddRange(this.creditosCancelados);
                foreach (DTO_ccCreditoDocu creditosCli in this.creditos)
                {
                    this.libranza = creditosCli.Libranza.Value.Value;
                    if (!this.datosGenerales.ContainsKey(this.libranza))
                    {
                        this.datosGenerales.Add(this.libranza, creditosCli);
                        this.datosCartera.Add(this.libranza, null);
                        this.datosCesion.Add(this.libranza, null);
                        this.datosPagos.Add(this.libranza, null);
                    }
                }
                this.EnableTabs(true);
                this.gcGenerales.DataSource = this.creditos;
                this.gcGenerales.RefreshDataSource();
                this.gvGenerales.BestFitColumns();
                this.gvGenerales.MoveFirst();
            }
            else
            {
                this.creditos = this.creditos.FindAll(x => !x.CanceladoInd.Value.Value);
                if (this.creditos.Count == 0)
                    this.EnableTabs(false);
                this.gcGenerales.DataSource = this.creditos;
                this.gcGenerales.RefreshDataSource();
                this.gvGenerales.BestFitColumns();
                this.gvGenerales.MoveFirst();
            }
        }

        /// <summary>
        /// Permite ver el extracto de movimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerExtracto_Click(object sender, EventArgs e)
        {
            try
            {
                string reportName = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(AppReports.ccReporteCredito, 2, null, DateTime.Today.Date, string.Empty, this.libranza, string.Empty, string.Empty,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null);
                Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "gvGenerales_FocusedRowChanged"));
            }
        }
        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
                    e.Value = true;
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
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.validate && e.FocusedRowHandle >= 0)
                {
                    int row = e.FocusedRowHandle;
                    this.selectedCredito = (DTO_ccCreditoDocu)this.gvGenerales.GetRow(row);
                    this.libranza = this.selectedCredito.Libranza.Value.Value;
                    this.GetValues(this.selectedCredito);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "gvGenerales_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.RepositoryItem = this.linkEditViewFile;
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGenerales_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = e.Column.Caption;
        }

        /// <summary>
        /// Evento que llama la funcionalidad de buscar documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkEditViewFile_Click(object sender, EventArgs e)
        {
            try
            {
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                if(this.tc_QueryCreditos.SelectedTabPageIndex == 2) // Pago Cuotas
                {
                    if(this.gvCuotaPagos.DataRowCount > 0)
                    {
                        DTO_ccCreditoPagos pago = (DTO_ccCreditoPagos)this.gvCuotaPagos.GetRow(this.gvCuotaPagos.FocusedRowHandle);
                        ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(pago.PagoDocu.Value.Value);
                        ctrl.ComprobanteIDNro.Value =  ctrl.ComprobanteIDNro.Value ?? 0;
                        comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);
                    }
                }
                else if (this.tc_QueryCreditos.SelectedTabPageIndex == 6) // Movimientos
                {
                    if (this.gvMovimientos.DataRowCount > 0)
                    {
                        DTO_QueryCarteraMvto mvto = (DTO_QueryCarteraMvto)this.gvMovimientos.GetRow(this.gvMovimientos.FocusedRowHandle);
                        ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(mvto.NumeroDoc.Value.Value);
                        ctrl.ComprobanteIDNro.Value = ctrl.ComprobanteIDNro.Value ?? 0;
                        comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);
                    }
                }
                else
                {
                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.selectedCredito.NumeroDoc.Value.Value);
                    ctrl.ComprobanteIDNro.Value = ctrl.ComprobanteIDNro.Value ?? 0;
                    comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);
                }

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-linkEditViewFile_Click"));
            }
        }
        
        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvIncorporacion_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this._unboundPrefix + "TipoNovedad")
                {                    
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_1);
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_2);
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_3);
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_4);
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_5);
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_6);
                     else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Cartera_TipoNovedad_6);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "gvIncorporacion_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Al cambiar el foco de los pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCuotaPagos_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.validate && e.FocusedRowHandle >= 0 && e.FocusedRowHandle < this.gvCuotaPagos.DataRowCount)
                {
                    DTO_ccCreditoPagos pagoCurrent = (DTO_ccCreditoPagos)this.gvCuotaPagos.GetRow(e.FocusedRowHandle);
                    if (pagoCurrent.PagoDocu.Value.HasValue)
                    {
                        DTO_glDocumentoControl ctrlPago = this._bc.AdministrationModel.glDocumentoControl_GetByID(pagoCurrent.PagoDocu.Value.Value);
                        this.txtDescripcionPago.Text = ctrlPago.Observacion.Value;
                    }
                    else
                        this.txtDescripcionPago.Text = string.Empty;
                }
                else
                    this.txtDescripcionPago.Text = string.Empty;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranzas.cs", "gvCuotaPagos_FocusedRowChanged"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para crear buscar
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearch);

                if (this.newSearch)
                {
                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string libTemp = this.txtLibranza.Text;
                        string cliTemp = this.masterCliente.Value;
                        string compCarteraTemp = this.masterCompradorCartera.Value;
                        this.CleanData();

                        if (rbLibranza.Checked)
                            this.txtLibranza.Text = libTemp;
                        if (rbCliente.Checked)
                            this.masterCliente.Value = cliTemp;
                        if (rbCompCartera.Checked)
                            this.masterCompradorCartera.Value = compCarteraTemp;
                        this.GetSearch();
                    }
                }
                else
                {
                    this.newSearch = true;
                    this.GetSearch();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryLibranza.cs", "TBSearch"));
            }
        }


        #endregion Eventos Barra Herramientas
    }
}
