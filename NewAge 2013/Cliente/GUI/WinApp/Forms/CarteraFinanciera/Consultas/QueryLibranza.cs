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
        private DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();
        private DTO_ccCreditoDocu consultaCredito = new DTO_ccCreditoDocu();
        private DTO_ccCreditoDocu selectedCredito = new DTO_ccCreditoDocu();
        private DTO_VentaCartera ventaCartera = new DTO_VentaCartera();
        private DTO_InfoCredito infoCredito = new DTO_InfoCredito();

        //Variable glControl
        private string componenteCapital = string.Empty;
        private string componenteUsura = string.Empty;
        private string componenteMora = string.Empty;


        //Variables formulario
        private bool validate = true;
        private bool newSearch = false;
        private string clienteID = string.Empty;
        private int libranzaID = 0;
        private int libranza = 0;
        private string compradorCarteraID = string.Empty;
        private Dictionary<int, DTO_ccCreditoDocu> datosGenerales = new Dictionary<int, DTO_ccCreditoDocu>();
        private Dictionary<int, DTO_InfoCredito> datosCartera = new Dictionary<int, DTO_InfoCredito>();
        private Dictionary<int, DTO_VentaCartera> datosCesion = new Dictionary<int, DTO_VentaCartera>();

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public QueryLibranza()
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AddGridCols();
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
            this._bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false);
            this._bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
            this._bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
          
            //Deshabilita las pestañas
            this.enableTabs(false);

            //Deshabilita los botones +- de la grilla
            this.gcGenerales.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            //Carga los componentes de glControl
            this.componenteCapital = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
            this.componenteMora = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteMora);
            this.componenteUsura = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteUsura);

            //Carga los combos de la fecha
            this.dtFechaDe.EditValue = DateTime.Now;
            this.dtFechaHasta.EditValue = DateTime.Now;

            //Deshabilita los controles cuando se carga la pantalla por primera vez
            this.txtLibranza.Enabled = false;
            this.masterCliente.Enabled = false;
            this.txtOferta.Enabled = false;
            this.masterCompradorCartera.Enabled = false;
            this.masterPagaduria.EnableControl(false);
            this.masterPagaduria.EnableControl(false);
            this.masterLineaCredito.EnableControl(false);
            this.masterCentroPago.EnableControl(false);
            this.masterZona.EnableControl(false);
            this.masterAsesor.EnableControl(false);
            //this.txtVlrGiro.Enabled = false;
            //this.txtVlrLibranza.Enabled = false;
            //this.txtVlrCredito.Enabled = false;         
            //this.dtFechaDe.Enabled = false;
            //this.dtFechaHasta.Enabled = false;
            //this.txtValorCuota.Enabled = false;
            //this.txtPorInteres.Enabled = false;
            //this.txtPorSeguro.Enabled = false;
            //this.txtPlazo.Enabled = true;
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

                //Abono
                GridColumn abono = new GridColumn();
                abono.FieldName = this._unboundPrefix + "VlrPagadoCuota";
                abono.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Abono");
                abono.UnboundType = UnboundColumnType.Integer;
                abono.VisibleIndex = 2;
                abono.Width = 80;
                abono.Visible = true;
                abono.ColumnEdit = this.editValue;
                abono.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(abono);

                //vlrSaldo
                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this._unboundPrefix + "VlrSaldo";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.DateTime;
                vlrSaldo.VisibleIndex = 3;
                vlrSaldo.Width = 80;
                vlrSaldo.Visible = true;
                vlrSaldo.ColumnEdit = this.editValue;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrSaldo);

                //VlrCuota
                GridColumn vlrCuotaCartera = new GridColumn();
                vlrCuotaCartera.FieldName = this._unboundPrefix + "VlrCuota";
                vlrCuotaCartera.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                vlrCuotaCartera.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCartera.VisibleIndex = 4;
                vlrCuotaCartera.Width = 150;
                vlrCuotaCartera.Visible = true;
                vlrCuotaCartera.ColumnEdit = this.editValue;
                vlrCuotaCartera.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrCuotaCartera);

                //VlrCapital
                GridColumn vlrCapital = new GridColumn();
                vlrCapital.FieldName = this._unboundPrefix + "VlrCapital";
                vlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCapital");
                vlrCapital.UnboundType = UnboundColumnType.Integer;
                vlrCapital.VisibleIndex = 5;
                vlrCapital.Width = 150;
                vlrCapital.Visible = true;
                vlrCapital.ColumnEdit = this.editValue;
                vlrCapital.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrCapital);

                //VlrInteres
                GridColumn vlrInteres = new GridColumn();
                vlrInteres.FieldName = this._unboundPrefix + "VlrInteres";
                vlrInteres.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrInteres");
                vlrInteres.UnboundType = UnboundColumnType.Integer;
                vlrInteres.VisibleIndex = 6;
                vlrInteres.Width = 150;
                vlrInteres.Visible = true;
                vlrInteres.ColumnEdit = this.editValue;
                vlrInteres.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrInteres);

                //VlrSeguro
                GridColumn vlrSeguro = new GridColumn();
                vlrSeguro.FieldName = this._unboundPrefix + "VlrSeguro";
                vlrSeguro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrSeguro");
                vlrSeguro.UnboundType = UnboundColumnType.Integer;
                vlrSeguro.VisibleIndex = 7;
                vlrSeguro.Width = 150;
                vlrSeguro.Visible = true;
                vlrSeguro.ColumnEdit = this.editValue;
                vlrSeguro.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrSeguro);

                //VlrOtrosFijos
                GridColumn vlrOtros = new GridColumn();
                vlrOtros.FieldName = this._unboundPrefix + "VlrOtrosFijos";
                vlrOtros.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosFijos");
                vlrOtros.UnboundType = UnboundColumnType.Integer;
                vlrOtros.VisibleIndex = 8;
                vlrOtros.Width = 150;
                vlrOtros.Visible = true;
                vlrOtros.ColumnEdit = this.editValue;
                vlrOtros.OptionsColumn.AllowEdit = false;
                this.gvCartera.Columns.Add(vlrOtros);
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

                //Pref Doc
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 2;
                prefDoc.Width = 90;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(prefDoc);

                //Fecha Pago
                GridColumn fechaPagoCuota = new GridColumn();
                fechaPagoCuota.FieldName = this._unboundPrefix + "FechaPago";
                fechaPagoCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaPago");
                fechaPagoCuota.UnboundType = UnboundColumnType.DateTime;
                fechaPagoCuota.VisibleIndex = 3;
                fechaPagoCuota.Width = 80;
                fechaPagoCuota.Visible = true;
                fechaPagoCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(fechaPagoCuota);

                //Valor Pago
                GridColumn vlrCuotaCarteraCuota = new GridColumn();
                vlrCuotaCarteraCuota.FieldName = this._unboundPrefix + "Valor";
                vlrCuotaCarteraCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                vlrCuotaCarteraCuota.UnboundType = UnboundColumnType.Integer;
                vlrCuotaCarteraCuota.VisibleIndex = 4;
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
                VlrCapitalCuota.VisibleIndex = 5;
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
                VlrInteresCuota.VisibleIndex = 6;
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
                VlrSeguroCuota.VisibleIndex = 7;
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
                VlrOtrosFijosCuota.VisibleIndex = 8;
                VlrOtrosFijosCuota.Width = 100;
                VlrOtrosFijosCuota.Visible = true;
                VlrOtrosFijosCuota.ColumnEdit = this.editValue;
                VlrOtrosFijosCuota.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrOtrosFijosCuota);

                //VlrMoraLiquida
                GridColumn vlrMoraLiquida = new GridColumn();
                vlrMoraLiquida.FieldName = this._unboundPrefix + "VlrMoraliquida";
                vlrMoraLiquida.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMoraliquida");
                vlrMoraLiquida.UnboundType = UnboundColumnType.Integer;
                vlrMoraLiquida.VisibleIndex = 9;
                vlrMoraLiquida.Width = 100;
                vlrMoraLiquida.Visible = true;
                vlrMoraLiquida.ColumnEdit = this.editValue;
                vlrMoraLiquida.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(vlrMoraLiquida);

                //vlrMoraPago
                GridColumn vlrMoraPago = new GridColumn();
                vlrMoraPago.FieldName = this._unboundPrefix + "VlrMoraPago";
                vlrMoraPago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrMoraPago");
                vlrMoraPago.UnboundType = UnboundColumnType.Integer;
                vlrMoraPago.VisibleIndex = 10;
                vlrMoraPago.Width = 100;
                vlrMoraPago.Visible = true;
                vlrMoraPago.ColumnEdit = this.editValue;
                vlrMoraPago.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(vlrMoraPago);

                //VlrAjusteUsura
                GridColumn VlrAjusteUsura = new GridColumn();
                VlrAjusteUsura.FieldName = this._unboundPrefix + "VlrAjusteUsura";
                VlrAjusteUsura.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrAjusteUsura");
                VlrAjusteUsura.UnboundType = UnboundColumnType.Integer;
                VlrAjusteUsura.VisibleIndex = 11;
                VlrAjusteUsura.Width = 100;
                VlrAjusteUsura.Visible = true;
                VlrAjusteUsura.ColumnEdit = this.editValue;
                VlrAjusteUsura.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrAjusteUsura);

                //VlrOtrosComponentes
                GridColumn VlrOtrosComponentes = new GridColumn();
                VlrOtrosComponentes.FieldName = this._unboundPrefix + "VlrOtrosComponentes";
                VlrOtrosComponentes.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrOtrosComponentes");
                VlrOtrosComponentes.UnboundType = UnboundColumnType.Integer;
                VlrOtrosComponentes.VisibleIndex = 12;
                VlrOtrosComponentes.Width = 100;
                VlrOtrosComponentes.Visible = true;
                VlrOtrosComponentes.ColumnEdit = this.editValue;
                VlrOtrosComponentes.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(VlrOtrosComponentes);

                //DiasMora
                GridColumn DiasMora = new GridColumn();
                DiasMora.FieldName = this._unboundPrefix + "DiasMora";
                DiasMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DiasMora");
                DiasMora.UnboundType = UnboundColumnType.Integer;
                DiasMora.VisibleIndex = 13;
                DiasMora.Width = 100;
                DiasMora.Visible = true;
                DiasMora.OptionsColumn.AllowEdit = false;
                this.gvCuotaPagos.Columns.Add(DiasMora);
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
                totalIncial.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalIncial");
                totalIncial.UnboundType = UnboundColumnType.Integer;
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
                totalSaldo.VisibleIndex = 4;
                totalSaldo.Width = 50;
                totalSaldo.Visible = true;
                totalSaldo.ColumnEdit = this.editValue;
                totalSaldo.OptionsColumn.AllowEdit = false;
                this.gvComponentes.Columns.Add(totalSaldo);
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
            this.masterAsesor.Value = string.Empty;
            this.masterCentroPago.Value = string.Empty;
            this.masterZona.Value = string.Empty;
            this.txtVlrCredito.EditValue = 0;
            this.txtVlrLibranza.EditValue = 0;
            this.txtVlrGiro.EditValue = 0;
            this.dtFechaDe.EditValue = DateTime.Now;
            this.dtFechaHasta.EditValue = DateTime.Now;
            this.txtValorCuota.EditValue = 0;
            this.txtPorInteres.EditValue = 0;
            this.txtPorSeguro.EditValue = 0;
            this.txtPlazo.EditValue = 0;

            //Footer
            this.credito = new DTO_ccCreditoDocu();
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.gcGenerales.DataSource = this.creditos;

            //Variables
            this.clienteID = string.Empty;
            this.libranzaID = 0;
            this.validate = true;
            this.datosGenerales = new Dictionary<int, DTO_ccCreditoDocu>();
            this.datosCartera = new Dictionary<int, DTO_InfoCredito>();
            this.datosCesion = new Dictionary<int, DTO_VentaCartera>();

            this.enableTabs(false);

        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void GetValues(DTO_ccCreditoDocu credito)
        {
            this.masterPagaduria.Value = credito.PagaduriaID.Value;
            this.masterLineaCredito.Value = credito.LineaCreditoID.Value;
            this.masterAsesor.Value = credito.AsesorID.Value;
            this.masterCentroPago.Value = credito.CentroPagoID.Value;
            this.masterZona.Value = credito.ZonaID.Value;
            this.txtVlrCredito.EditValue = credito.VlrPrestamo.Value;
            this.txtVlrLibranza.EditValue = credito.VlrLibranza.Value;
            this.txtVlrGiro.EditValue = credito.VlrGiro.Value;
            this.txtValorCuota.EditValue = credito.VlrCuota.Value;
            this.txtPorInteres.EditValue = credito.PorInteres.Value;
            this.txtPorSeguro.EditValue = credito.PorSeguro.Value;
            this.txtPlazo.EditValue = credito.Plazo.Value;
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
                            DateTime fecha = this.selectedCredito.FechaLiquida.Value.Value;
                            this.infoCredito = this._bc.AdministrationModel.GetInfoCredito(numDoc, fecha);
                            if (this.infoCredito != null)
                            {
                                this.datosCartera[libranza] = this.infoCredito;
                                #region Carga la informacion del header
                                this.txtLibranza1.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterCliente1.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSldoCapital.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtSldoLibranza.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes select c.CuotaSaldo.Value).Sum());
                                this.txtMoraCapital.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtMoraLibranza.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                                this.txtVlrCredito1.EditValue = this.selectedCredito.VlrPrestamo.Value;
                                this.txtVlrLibranza1.EditValue = this.selectedCredito.VlrLibranza.Value;
                                this.txtCtasPendientes.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                                this.txtCtasMora.Text = "0";
                                this.masterZona.Value = this.selectedCredito.ZonaID.Value;
                                this.txtPlazo.EditValue = this.selectedCredito.Plazo.Value;
                                this.txtPorInteres.EditValue = this.selectedCredito.PorInteres.Value;
                                this.txtPorSeguro.EditValue = this.selectedCredito.PorSeguro.Value;
                                this.txtValorCuota.EditValue = this.selectedCredito.VlrCuota.Value;
                                #endregion
                                #region Carga la infromacion de la grilla
                                foreach (DTO_ccCreditoPlanPagos pp in this.infoCredito.PlanPagos)
                                {
                                    if (pp.VlrPagadoCuota.Value.Value != 0)
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value - pp.VlrPagadoCuota.Value.Value;
                                    else
                                        pp.VlrSaldo.Value = pp.VlrCuota.Value.Value;                                 
                                }
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
                            this.txtSldoCapital.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtSldoLibranza.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes select c.CuotaSaldo.Value).Sum());
                            this.txtMoraCapital.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtMoraLibranza.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                            this.txtVlrCredito1.EditValue = this.selectedCredito.VlrPrestamo.Value;
                            this.txtVlrLibranza1.EditValue = this.selectedCredito.VlrLibranza.Value;
                            this.txtCtasPendientes.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                            this.txtCtasMora.Text = "0";
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
                                #region Carga la informacion del header
                                this.txtLibranzaCuota.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterClienteCuota.Value = this.selectedCredito.ClienteID.Value;
                                this.txtSaldoCapitalCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtSaldoLibranzaCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes select c.CuotaSaldo.Value).Sum());
                                this.txtMoraCapitalCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                                this.txtMoraLibranzaCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                                this.txtVlrCreditoCuota.EditValue = this.selectedCredito.VlrPrestamo.Value;
                                this.txtVlrLibranzaCuota.EditValue = this.selectedCredito.VlrLibranza.Value;
                                this.txtCtasPendientesCuota.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                                this.txtCtasMoraCuota.Text = "0";
                                #endregion
                                #region Carga la infromacion de la grilla
                                DTO_InfoPagos pagos = this._bc.AdministrationModel.GetInfoPagos(numDoc);
                                if (pagos.CreditoPagos == null)
                                      break;
                                this.gcCuotaPagos.DataSource = pagos.CreditoPagos;
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
                            this.txtSaldoCapitalCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtSaldoLibranzaCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes select c.CuotaSaldo.Value).Sum());
                            this.txtMoraCapitalCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora && c.ComponenteCarteraID.Value == this.componenteCapital select c.CuotaSaldo.Value).Sum());
                            this.txtMoraLibranzaCuota.EditValue = Convert.ToInt32((from c in this.infoCredito.SaldosComponentes where c.ComponenteCarteraID.Value == this.componenteMora select c.CuotaSaldo.Value).Sum());
                            this.txtVlrCreditoCuota.EditValue = this.selectedCredito.VlrPrestamo.Value;
                            this.txtVlrLibranzaCuota.EditValue = this.selectedCredito.VlrLibranza.Value;
                            this.txtCtasPendientesCuota.Text = (from c in this.infoCredito.PlanPagos where c.VlrCuota.Value != c.VlrPagadoCuota.Value select c.CuotaID.Value).Count().ToString();
                            this.txtCtasMoraCuota.Text = "0";
                            #endregion
                            #region Carga la infromacion de la grilla
                            DTO_InfoPagos pagos = this._bc.AdministrationModel.GetInfoPagos(this.infoCredito.PlanPagos[0].NumeroDoc.Value.Value);
                            if (pagos.CreditoPagos == null)
                                break;
                            this.gcCuotaPagos.DataSource = pagos.CreditoPagos;
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
                            this.ventaCartera = this._bc.AdministrationModel.GetInfoVentaByLibranza(numDoc);
                            if (this.ventaCartera.VentaDocu != null)
                            {
                                this.datosCesion[libranza] = this.ventaCartera;
                                this.ventaCartera.CrediDocu = this.creditos;
                                #region Carga la informacion del header
                                this.txtLibranza2.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterCliente2.Value = this.selectedCredito.ClienteID.Value;
                                this.txtVlrVenta.EditValue = this.ventaCartera.VentaDeta[0].VlrVenta.Value;
                                this.txtOferta1.Text = this.ventaCartera.VentaDocu.Oferta.Value;
                                this.txtPortafolio.Text = this.ventaCartera.VentaDeta[0].Portafolio.Value;
                                this.txtTasaVenta.EditValue = this.ventaCartera.VentaDocu.FactorCesion.Value;
                                this.txtVlrCredito2.EditValue = this.selectedCredito.VlrPrestamo.Value;
                                this.txtVlrLibranza2.EditValue = this.ventaCartera.VentaDeta[0].VlrLibranza.Value;
                                #endregion
                                #region Carga la infromacion de la grilla
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
                                #region Carga la informacion del header
                                this.txtLibranzaComponentes.Text = this.selectedCredito.Libranza.Value.ToString();
                                this.masterCliente3.Value = this.selectedCredito.ClienteID.Value;
                                #endregion
                                #region Carga la infromacion de la grilla
                                saldosComps = this._bc.AdministrationModel.GetSaldoCuentasForCredito(numDoc, this.selectedCredito.ClienteID.Value);
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
                            #region Carga la informacion del header
                            this.txtLibranzaComponentes.Text = this.selectedCredito.Libranza.Value.ToString();
                            this.masterCliente3.Value = this.selectedCredito.ClienteID.Value;
                            #endregion
                            #region Carga la infromacion de la grilla
                            saldosComps = this._bc.AdministrationModel.GetSaldoCuentasForCredito(numDoc, this.selectedCredito.ClienteID.Value);
                            this.gcComponentes.DataSource = saldosComps;
                            this.gcCuotaPagos.RefreshDataSource();
                            this.gvCuotaPagos.MoveFirst();
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
            #region Busca por libranza
            if (rbLibranza.Checked && !String.IsNullOrEmpty(this.txtLibranza.Text))
            {
                #region Limpia los filtros
                this.masterCliente.Value = string.Empty;
                this.txtOferta.Text = string.Empty;
                this.masterCompradorCartera.Value = string.Empty;
                #endregion
                this.libranzaID = Convert.ToInt32(this.txtLibranza.Text);
                this.credito = this._bc.AdministrationModel.GetCreditoByLibranza(this.libranzaID);
                if (this.credito != null)
                {
                    this.consultaCredito = this._bc.AdministrationModel.GetCreditoByLibranzaAndFechaCorte(this.libranzaID, this.credito.NumeroDoc.Value.Value, DateTime.Now);
                    if (this.consultaCredito != null)
                    {
                        if (!this.datosGenerales.ContainsKey(this.libranzaID))
                        {
                            this.datosGenerales.Add(this.libranzaID, this.consultaCredito);
                            this.datosCartera.Add(this.libranzaID, null);
                            this.datosCesion.Add(this.libranzaID, null);
                            this.enableTabs(true);
                            this.creditos.Add(this.consultaCredito);
                            this.gcGenerales.DataSource = this.creditos;
                            this.gvGenerales.BestFitColumns();
                        }                      
                        this.gcGenerales.RefreshDataSource();
                        this.gvGenerales.MoveFirst();
                    }
                    else
                    {
                        this.enableTabs(false);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }
                }
                else
                {
                    this.enableTabs(false);
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
                this.creditos = this._bc.AdministrationModel.GetCreditoByClienteAndFecha(this.clienteID, DateTime.Now, (DateTime)this.dtFechaDe.EditValue, (DateTime)this.dtFechaHasta.EditValue);

                if (this.creditos.Count != 0)
                {
                    if (this.creditos != null)
                    {
                        this.enableTabs(true);
                        foreach (DTO_ccCreditoDocu creditosCli in this.creditos)
                        {
                            this.libranza = creditosCli.Libranza.Value.Value;
                            if (!this.datosGenerales.ContainsKey(this.libranza))
                            {
                                this.datosGenerales.Add(this.libranza, creditosCli);
                                this.datosCartera.Add(this.libranza, null);
                                this.datosCesion.Add(this.libranza, null);
                            }
                        }
                        this.gcGenerales.DataSource = this.creditos;
                        this.gcGenerales.RefreshDataSource();
                        this.gvGenerales.BestFitColumns();
                        this.gvGenerales.MoveFirst();
                    }
                    else
                    {
                        this.enableTabs(false);
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                        this.CleanData();
                    }
                }
                else
                {
                    this.enableTabs(false);
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_Cartera_ClienteSinLibranzas), this.masterCliente.Value, this.dtFechaDe.EditValue, this.dtFechaHasta.EditValue);
                    MessageBox.Show(msg);
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
                this.creditos = this._bc.AdministrationModel.GetCreditoByCompradorCartera(this.compradorCarteraID);
                if (this.creditos != null)
                {
                    this.enableTabs(true);
                    foreach (DTO_ccCreditoDocu credito in this.creditos)
                    {
                        this.libranza = credito.Libranza.Value.Value;
                        if (!this.datosGenerales.ContainsKey(this.libranza))
                        {
                            this.datosGenerales.Add(this.libranza, credito);
                            this.datosCartera.Add(this.libranza, null);
                            this.datosCesion.Add(this.libranza, null);
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
                    this.enableTabs(false);
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
                this.enableTabs(false);
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidSearchCriteria));
                return;
            }
        }

        /// <summary>
        /// Funcion que habilita o deshabilita las pestañas
        /// </summary>
        /// <param name="enable"></param>
        private void enableTabs(bool enable)
        {
            this.tc_QueryCreditos.TabPages[1].PageEnabled = enable;
            this.tc_QueryCreditos.TabPages[2].PageEnabled = enable;
            this.tc_QueryCreditos.TabPages[3].PageEnabled = enable;
            this.tc_QueryCreditos.TabPages[4].PageEnabled = enable;
        }

        /// <summary>
        /// Funcion que se encarga de habilitar los controles
        /// </summary>
        private void enableControls()
        {
            //this.masterPagaduria.EnableControl(true);
            //this.masterLineaCredito.EnableControl(true);
            //this.masterAsesor.EnableControl(true);
            //this.masterCentroPago.EnableControl(true);
            this.txtVlrGiro.Enabled = true;
            this.txtVlrCredito.Enabled = true;
            this.lblVlrLibranza.Enabled = true;
            if (!this.rbLibranza.Checked)
            {
                this.dtFechaDe.Enabled = true;
                this.dtFechaHasta.Enabled = true;
            }

            #region Bloquea los controles que no se este utilizando
            if (this.rbLibranza.Checked)
            {
                this.masterCliente.Enabled = false;
                this.txtOferta.Enabled = false;
                this.masterCompradorCartera.Enabled = false;
                this.dtFechaDe.Enabled = false;
                this.dtFechaHasta.Enabled = false;
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
            this.enableControls();
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
            this.enableControls();
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
            this.enableControls();
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
            this.enableControls();
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
                if (this.validate)
                {
                    int row = e.FocusedRowHandle;
                    this.selectedCredito = (DTO_ccCreditoDocu)this.gvGenerales.GetRow(row);
                    this.GetValues(this.selectedCredito);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.selectedCredito.NumeroDoc.Value.Value);
                comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null);

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultarDocumentoCxP.cs-linkEditViewFile_Click"));
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
                        this.newSearch = false;
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
