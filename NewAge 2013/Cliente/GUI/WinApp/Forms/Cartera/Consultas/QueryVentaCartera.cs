using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors.Controls;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryVentaCartera : QueryForm
    {
        public QueryVentaCartera() 
            : base()
        {
            //InitializeComponent();
        }

        public QueryVentaCartera(string mod)
            :base(mod)
        {
            //InitializeComponent();
        }

        #region Variables
        private DTO_QueryDetailFactura _detalle = null;
        private List<DTO_QueryVentaCartera> _data = null;        
        private Dictionary<int, string> _filter = new Dictionary<int, string>();
        private string _periodoDefult = string.Empty;
        private TipoVentaCartera _tipoVenta = TipoVentaCartera.Todas;

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Columnas de grilla principal

                //Oferta
                GridColumn Oferta = new GridColumn();
                Oferta.FieldName = this._unboundPrefix + "Oferta";
                Oferta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Oferta");
                Oferta.UnboundType = UnboundColumnType.String;
                Oferta.VisibleIndex = 1;
                Oferta.Width = 80;
                Oferta.Visible = true;
                Oferta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Oferta);

                //FechaPago1
                GridColumn FechaPago1 = new GridColumn();
                FechaPago1.FieldName = this._unboundPrefix + "FechaPago1";
                FechaPago1.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaPago1");
                FechaPago1.UnboundType = UnboundColumnType.DateTime;
                FechaPago1.VisibleIndex = 2;
                FechaPago1.Width = 90;
                FechaPago1.Visible = true;
                FechaPago1.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaPago1);

                //FechaPagoUlt
                GridColumn FechaPagoUlt = new GridColumn();
                FechaPagoUlt.FieldName = this._unboundPrefix + "FechaPagoUlt";
                FechaPagoUlt.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaPagoUlt");
                FechaPagoUlt.UnboundType = UnboundColumnType.DateTime;
                FechaPagoUlt.VisibleIndex = 3;
                FechaPagoUlt.Width = 90;
                FechaPagoUlt.Visible = true;
                FechaPagoUlt.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaPagoUlt);

                //Observacion
                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this._unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.DateTime;
                Observacion.VisibleIndex = 4;
                Observacion.Width = 120;
                Observacion.Visible = true;
                Observacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Observacion);

                //FechaDoc
                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this._unboundPrefix + "FechaDoc";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 5;
                FechaDoc.Width = 90;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                //FactorCesion
                GridColumn FactorCesion = new GridColumn();
                FactorCesion.FieldName = this._unboundPrefix + "FactorCesion";
                FactorCesion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FactorCesion");
                FactorCesion.UnboundType = UnboundColumnType.Integer;
                FactorCesion.VisibleIndex = 6;
                FactorCesion.Width = 80;
                FactorCesion.Visible = true;
                FactorCesion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FactorCesion);

                //VlrVenta
                GridColumn VlrVenta = new GridColumn();
                VlrVenta.FieldName = this._unboundPrefix + "VlrVenta";
                VlrVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrVenta");
                VlrVenta.UnboundType = UnboundColumnType.Decimal;
                VlrVenta.VisibleIndex = 7;
                VlrVenta.Width = 100;
                VlrVenta.Visible = true;
                VlrVenta.ColumnEdit = this.TextEdit;
                VlrVenta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(VlrVenta);

                //VlrLibranza
                GridColumn VlrLibranza = new GridColumn();
                VlrLibranza.FieldName = this._unboundPrefix + "VlrLibranza";
                VlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrLibranza");
                VlrLibranza.UnboundType = UnboundColumnType.Decimal;
                VlrLibranza.VisibleIndex = 8;
                VlrLibranza.Width = 100;
                VlrLibranza.Visible = true;
                VlrLibranza.ColumnEdit = this.TextEdit;
                VlrLibranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(VlrLibranza);

                //SaldoFlujo
                GridColumn SaldoFlujo = new GridColumn();
                SaldoFlujo.FieldName = this._unboundPrefix + "SaldoFlujo";
                SaldoFlujo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFlujo");
                SaldoFlujo.UnboundType = UnboundColumnType.Decimal;
                SaldoFlujo.VisibleIndex = 9;
                SaldoFlujo.Width = 100;
                SaldoFlujo.Visible = true;
                SaldoFlujo.ColumnEdit = this.TextEdit;
                SaldoFlujo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(SaldoFlujo);

                //CantLibranzas
                GridColumn CantLibranzas = new GridColumn();
                CantLibranzas.FieldName = this._unboundPrefix + "TotalLibranza";
                CantLibranzas.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TotalLibranza");
                CantLibranzas.UnboundType = UnboundColumnType.Integer;
                CantLibranzas.VisibleIndex = 10;
                CantLibranzas.Width = 70;
                CantLibranzas.Visible = true;
                CantLibranzas.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CantLibranzas.AppearanceCell.Options.UseTextOptions = true;
                CantLibranzas.AppearanceCell.Options.UseFont = true;
                CantLibranzas.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CantLibranzas);

                // CredPendientes
                GridColumn CredPendientes = new GridColumn();
                CredPendientes.FieldName = this._unboundPrefix + "CredPendientes";
                CredPendientes.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CredPendientes");
                CredPendientes.UnboundType = UnboundColumnType.Integer;
                CredPendientes.VisibleIndex = 11;
                CredPendientes.Width = 70;
                CredPendientes.Visible = true;
                CredPendientes.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CredPendientes.AppearanceCell.Options.UseTextOptions = true;
                CredPendientes.AppearanceCell.Options.UseFont = true;
                CredPendientes.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CredPendientes);

                // CredMora
                GridColumn CredMora = new GridColumn();
                CredMora.FieldName = this._unboundPrefix + "CredMora";
                CredMora.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CredMora");
                CredMora.UnboundType = UnboundColumnType.Integer;
                CredMora.VisibleIndex = 12;
                CredMora.Width = 70;
                CredMora.Visible = true;
                CredMora.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CredMora.AppearanceCell.Options.UseTextOptions = true;
                CredMora.AppearanceCell.Options.UseFont = true;
                CredMora.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CredMora);

                // CredPrepagados
                GridColumn CredPrepagados = new GridColumn();
                CredPrepagados.FieldName = this._unboundPrefix + "CredPrepagados";
                CredPrepagados.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CredPrepagados");
                CredPrepagados.UnboundType = UnboundColumnType.Integer;
                CredPrepagados.VisibleIndex = 13;
                CredPrepagados.Width = 70;
                CredPrepagados.Visible = true;
                CredPrepagados.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CredPrepagados.AppearanceCell.Options.UseTextOptions = true;
                CredPrepagados.AppearanceCell.Options.UseFont = true;
                CredPrepagados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CredPrepagados);

                // CredRecompra
                GridColumn CredRecompra = new GridColumn();
                CredRecompra.FieldName = this._unboundPrefix + "CredRecompra";
                CredRecompra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CredRecompra");
                CredRecompra.UnboundType = UnboundColumnType.Integer;
                CredRecompra.VisibleIndex = 14;
                CredRecompra.Width = 70;
                CredRecompra.Visible = true;
                CredRecompra.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CredRecompra.AppearanceCell.Options.UseTextOptions = true;
                CredRecompra.AppearanceCell.Options.UseFont = true;
                CredRecompra.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CredRecompra);

                this.gvDocument.BestFitColumns();
                #endregion

                #region Grilla Interna de Detalle

                // Libranza
                GridColumn Libranza = new GridColumn();
                Libranza.FieldName = this._unboundPrefix + "Libranza";
                Libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Libranza");
                Libranza.UnboundType = UnboundColumnType.Integer;
                Libranza.VisibleIndex = 1;
                Libranza.Width = 70;
                Libranza.Visible = true;
                Libranza.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Libranza);

                // ClienteID
                GridColumn ClienteID = new GridColumn();
                ClienteID.FieldName = this._unboundPrefix + "ClienteID";
                ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ClienteID");
                ClienteID.UnboundType = UnboundColumnType.String;
                ClienteID.VisibleIndex = 2;
                ClienteID.Width = 80;
                ClienteID.Visible = true;
                ClienteID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ClienteID);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 3;
                Descriptivo.Width = 100;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Descriptivo);

                //Plazo
                GridColumn Plazo = new GridColumn();
                Plazo.FieldName = this._unboundPrefix + "Plazo";
                Plazo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Plazo");
                Plazo.UnboundType = UnboundColumnType.Integer;
                Plazo.VisibleIndex = 4;
                Plazo.Width = 60;
                Plazo.Visible = true;
                Plazo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Plazo);

                //VlrCuota
                GridColumn VlrCuota = new GridColumn();
                VlrCuota.FieldName = this._unboundPrefix + "VlrCuota";
                VlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrCuota");
                VlrCuota.UnboundType = UnboundColumnType.Decimal;
                VlrCuota.VisibleIndex = 5;
                VlrCuota.Width = 90;
                VlrCuota.Visible = true;
                VlrCuota.ColumnEdit = this.TextEdit;
                VlrCuota.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrCuota);

                //CuotasVend
                GridColumn CuotasVend = new GridColumn();
                CuotasVend.FieldName = this._unboundPrefix + "CuotasVend";
                CuotasVend.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotasVend");
                CuotasVend.UnboundType = UnboundColumnType.Integer;
                CuotasVend.VisibleIndex = 6;
                CuotasVend.Width = 80;
                CuotasVend.Visible = true;
                CuotasVend.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CuotasVend);

                //VlrLibranzaDet
                GridColumn VlrLibranzaDet = new GridColumn();
                VlrLibranzaDet.FieldName = this._unboundPrefix + "VlrLibranza";
                VlrLibranzaDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrLibranza");
                VlrLibranzaDet.UnboundType = UnboundColumnType.Decimal;
                VlrLibranzaDet.VisibleIndex = 7;
                VlrLibranzaDet.Width = 90;
                VlrLibranzaDet.Visible = true;
                VlrLibranzaDet.ColumnEdit = this.TextEdit;
                VlrLibranzaDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrLibranzaDet);

                //FactorCesionDet
                GridColumn FactorCesionDet = new GridColumn();
                FactorCesionDet.FieldName = this._unboundPrefix + "FactorCesion";
                FactorCesionDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FactorCesion");
                FactorCesionDet.UnboundType = UnboundColumnType.Integer;
                FactorCesionDet.VisibleIndex = 8;
                FactorCesionDet.Width = 80;
                FactorCesionDet.Visible = true;
                FactorCesionDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(FactorCesionDet);

                //VlrVentaDet
                GridColumn VlrVentaDet = new GridColumn();
                VlrVentaDet.FieldName = this._unboundPrefix + "VlrVenta";
                VlrVentaDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrVenta");
                VlrVentaDet.UnboundType = UnboundColumnType.Decimal;
                VlrVentaDet.VisibleIndex = 9;
                VlrVentaDet.Width = 90;
                VlrVentaDet.Visible = true;
                VlrVentaDet.ColumnEdit = this.TextEdit;
                VlrVentaDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(VlrVentaDet);

                //SaldoFlujoDet
                GridColumn SaldoFlujoDet = new GridColumn();
                SaldoFlujoDet.FieldName = this._unboundPrefix + "SaldoFlujo";
                SaldoFlujoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFlujo");
                SaldoFlujoDet.UnboundType = UnboundColumnType.Decimal;
                SaldoFlujoDet.VisibleIndex = 10;
                SaldoFlujoDet.Width = 80;
                SaldoFlujoDet.Visible = true;
                SaldoFlujoDet.ColumnEdit = this.TextEdit;
                SaldoFlujoDet.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SaldoFlujoDet);

                //AlturaFlujo
                GridColumn AlturaFlujo = new GridColumn();
                AlturaFlujo.FieldName = this._unboundPrefix + "AlturaFlujo";
                AlturaFlujo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_AlturaFlujo");
                AlturaFlujo.UnboundType = UnboundColumnType.Integer;
                AlturaFlujo.VisibleIndex = 11;
                AlturaFlujo.Width = 70;
                AlturaFlujo.Visible = true;
                AlturaFlujo.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(AlturaFlujo);

                //SaldoFlujoCAP
                GridColumn SaldoFlujoCAP = new GridColumn();
                SaldoFlujoCAP.FieldName = this._unboundPrefix + "SaldoFlujoCAP";
                SaldoFlujoCAP.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SaldoFlujoCAP");
                SaldoFlujoCAP.UnboundType = UnboundColumnType.Decimal;
                SaldoFlujoCAP.VisibleIndex = 12;
                SaldoFlujoCAP.Width = 80;
                SaldoFlujoCAP.Visible = true;
                SaldoFlujoCAP.ColumnEdit = this.TextEdit;
                SaldoFlujoCAP.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SaldoFlujoCAP);

                //CuotasVEN
                GridColumn CuotasVEN = new GridColumn();
                CuotasVEN.FieldName = this._unboundPrefix + "CuotasVEN";
                CuotasVEN.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CuotasVEN");
                CuotasVEN.UnboundType = UnboundColumnType.Integer;
                CuotasVEN.VisibleIndex = 13;
                CuotasVEN.Width = 60;
                CuotasVEN.Visible = true;
                CuotasVEN.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CuotasVEN);

                //CapitalVEN
                GridColumn CapitalVEN = new GridColumn();
                CapitalVEN.FieldName = this._unboundPrefix + "CapitalVEN";
                CapitalVEN.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_CapitalVEN");
                CapitalVEN.UnboundType = UnboundColumnType.Decimal;
                CapitalVEN.VisibleIndex = 14;
                CapitalVEN.Width = 70;
                CapitalVEN.Visible = true;
                CapitalVEN.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CapitalVEN);

                //FechaDocPrepago
                GridColumn FechaDocPrepago = new GridColumn();
                FechaDocPrepago.FieldName = this._unboundPrefix + "FechaDocPrepago";
                FechaDocPrepago.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDocPrepago");
                FechaDocPrepago.UnboundType = UnboundColumnType.DateTime;
                FechaDocPrepago.VisibleIndex = 15;
                FechaDocPrepago.Width = 80;
                FechaDocPrepago.Visible = true;
                FechaDocPrepago.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(FechaDocPrepago);

                //FechaDocRecompra
                GridColumn FechaDocRecompra = new GridColumn();
                FechaDocRecompra.FieldName = this._unboundPrefix + "FechaDoc";
                FechaDocRecompra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDocRecompra");
                FechaDocRecompra.UnboundType = UnboundColumnType.DateTime;
                FechaDocRecompra.VisibleIndex = 16;
                FechaDocRecompra.Width = 80;
                FechaDocRecompra.Visible = true;
                FechaDocRecompra.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(FechaDocRecompra);

                //EC_Fecha
                GridColumn EC_Fecha = new GridColumn();
                EC_Fecha.FieldName = this._unboundPrefix + "EC_Fecha";
                EC_Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_EC_Fecha");
                EC_Fecha.UnboundType = UnboundColumnType.DateTime;
                EC_Fecha.VisibleIndex = 17;
                EC_Fecha.Width = 100;
                EC_Fecha.Visible = false;
                EC_Fecha.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(EC_Fecha);

                this.gvDetalle.BestFitColumns();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this._documentID = AppQueries.VentaCartera;
            this._frmModule = ModulesPrefix.cc;
            this.saveDelegate = new Save(this.SaveMethod);
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        protected override void InitControls()
        {
            try
            {
                //Inicia los controles maestras
                this._bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, true);
                this._periodoDefult = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
                this.dtMesINI.DateTime = !string.IsNullOrEmpty( this._periodoDefult) ? Convert.ToDateTime(this._periodoDefult): DateTime.Now;
                this.dtMesFIN.DateTime = !string.IsNullOrEmpty(this._periodoDefult) ? Convert.ToDateTime(this._periodoDefult) : DateTime.Now;
                this.dtMesINI.Enabled = false;
                this.dtMesFIN.Enabled = false;

                //Carga el Compo de filtro
                this._filter.Add(0, this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Todos));
                this._filter.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Pendientes"));              
                this._filter.Add(2, this._bc.GetResource(LanguageTypes.Tables, "En Mora"));
                this._filter.Add(3, this._bc.GetResource(LanguageTypes.Tables, "Prepagados"));
                this._filter.Add(4, this._bc.GetResource(LanguageTypes.Tables, "Recomprados"));
              
                this.cmbFiltro.Properties.DataSource = this._filter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Initontrols"));
            }
        }

        /// <summary>
        /// Funcion que se encarga de limpiar los controles
        /// </summary>
        protected override void CleanData(bool cleanAll)
        {
            if (!cleanAll)
            {
                this.masterCompradorCartera.Value = string.Empty;
                this.gcDocument.DataSource = null;
            }
            else 
            {               
                this.masterCompradorCartera.Value = string.Empty;
                this.txtOferta.Text = string.Empty;
                this.gcDocument.DataSource = null;
                this.masterCompradorCartera.Focus();
                this.dtMesINI.Enabled = false;
                this.dtMesFIN.Enabled = false;
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterCompradorCar_Leave(object sender, EventArgs e)
        {
            if (this.masterCompradorCartera.ValidID)
            {
                DTO_ccCierreMesCartera filter = new DTO_ccCierreMesCartera();
                filter.CompradorCarteraID.Value = this.masterCompradorCartera.Value;
                List<DTO_ccCierreMesCartera> cierres = this._bc.AdministrationModel.ccCierreMesCartera_GetByParameter(filter);
                if (cierres != null && cierres.Count > 0)
                {
                    this.dtMesINI.DateTime = cierres.Min(x => x.Periodo.Value.Value);
                    this.dtMesFIN.DateTime = cierres.Max(x => x.Periodo.Value.Value);
                    int lastDayINI = DateTime.DaysInMonth(this.dtMesINI.DateTime.Year, this.dtMesINI.DateTime.Month);
                    int lastDayFIN = DateTime.DaysInMonth(this.dtMesFIN.DateTime.Year, this.dtMesFIN.DateTime.Month);

                    this.dtMesINI.MinValue = new DateTime(this.dtMesINI.DateTime.Year, this.dtMesINI.DateTime.Month, 1);
                    this.dtMesINI.MaxValue = new DateTime(this.dtMesINI.DateTime.Year, this.dtMesINI.DateTime.Month, lastDayINI);
                    this.dtMesINI.DateTime = new DateTime(this.dtMesINI.DateTime.Year, this.dtMesINI.DateTime.Month, 1);

                    this.dtMesFIN.MinValue = new DateTime(this.dtMesFIN.DateTime.Year, this.dtMesFIN.DateTime.Month, 1);
                    this.dtMesFIN.MaxValue = new DateTime(this.dtMesFIN.DateTime.Year, this.dtMesFIN.DateTime.Month, lastDayFIN);
                    this.dtMesFIN.DateTime = new DateTime(this.dtMesFIN.DateTime.Year, this.dtMesFIN.DateTime.Month, 1);
                }
                else 
                {
                    this.dtMesINI.DateTime = DateTime.Now;
                    this.dtMesFIN.DateTime = DateTime.Now;
                }

                this.dtMesINI.Enabled = true;
                this.dtMesFIN.Enabled = true;
            }
        }

        /// <summary>
        /// Evento que habilita los controles para realizar la consulta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbFilter_EditValueChanged(object sender, EventArgs e)
        {
            switch ((int)this.cmbFiltro.EditValue)
            {
                case 0:
                    this._tipoVenta = TipoVentaCartera.Todas;
                    break;
                case 1:
                    this._tipoVenta = TipoVentaCartera.EnMora;
                    break;
                case 2:
                    this._tipoVenta = TipoVentaCartera.Pendiente;
                    break;
                case 3:
                    this._tipoVenta = TipoVentaCartera.Prepagada;
                    break;
                case 4:
                    this._tipoVenta = TipoVentaCartera.Recomprada;
                    break;
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_Enter"));
            }
        }
        #endregion

        #region Eventos Grillas
        #region Detail

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetalle_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = e.Column.Caption;
        }

        #endregion
        #endregion

        #region Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            { 
                this._data = new List<DTO_QueryVentaCartera>();
                this._detalle = new DTO_QueryDetailFactura();
                this.CleanData(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                FormProvider.Master.Focus();
                if (!string.IsNullOrWhiteSpace(this.dtMesINI.DateTime.ToString()))
                {
                    if (this.masterCompradorCartera.ValidID)
                    {                        
                        this.gcDocument.DataSource = null;
                        this._data = new List<DTO_QueryVentaCartera>();

                        this._data = this._bc.AdministrationModel.VentaCartera_GetForCompradorCart(this.masterCompradorCartera.Value,this.txtOferta.Text, this.dtMesINI.DateTime,this.dtMesFIN.DateTime, _tipoVenta);
                        this.gcDocument.DataSource = this._data;
                    }
                    else
                    {
                        string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCompradorCartera.LabelRsx);
                        MessageBox.Show(msg);
                        this.masterCompradorCartera.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
