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
using System.Diagnostics;
using SentenceTransformer;
using System.Globalization;
using System.Reflection;
using NewAge.DTO.Attributes;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using System.Collections.ObjectModel;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PreventaCartera : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de guardar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.LoadFilters();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int docRecursos;

        //DTO's
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private List<DTO_ccCreditoDocu> creditosToSave = new List<DTO_ccCreditoDocu>();
        private DTO_VentaCartera ventaCartera = new DTO_VentaCartera();
        private Dictionary<int, List<DTO_ccCreditoPlanPagos>> planPagos = new Dictionary<int, List<DTO_ccCreditoPlanPagos>>();
        private List<int> numeroDocs = new List<int>();
        private DataTable vistaCesiones = new DataTable();

        //Variables privadas
        private string tipoTasaVenta;
        private string compradorCarteraID = String.Empty;
        private DTO_ccCompradorCartera compradorCartera;
        private string oferta = String.Empty;
        private DateTime fechaLiquida = DateTime.Now;
        private DateTime fechaFlujo = DateTime.Now;
        private string libranzaID = String.Empty;
        private string lineaCreditoID = String.Empty;
        private string clienteID = String.Empty;
        private DateTime periodo;
        private decimal tasaAnual;
        private decimal tasaMensual;
        private bool loadDocuments = true;
        private bool sendToApprove = false;
        private string sectorCartera = string.Empty;
        //Variables Reportes
        private string fileURl;
        private string reportName;
        #endregion

        public PreventaCartera()
            : base()
        {
            //this.InitializeComponent();
        }

        public PreventaCartera(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.InitializeComponent();
                base.SetInitParameters();

                this.docRecursos = AppDocuments.PreseleccionLibranzas;
                this.documentID = AppDocuments.Preventa;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 230;
                this.tlSeparatorPanel.RowStyles[1].Height = 365;
                this.tlSeparatorPanel.RowStyles[2].Height = 130;

                //Carga la indormacion de Header
                #region Crea los filtros del comprador Cartera

                //Inversionista FinalInd
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "InversionistaFinalInd",
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = "0"
                });

                //Comprador propio
                string compradorPropio = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "CompradorCarteraID",
                    OperadorFiltro = OperadorFiltro.Diferente,
                    OperadorSentencia = OperadorSentencia.And,
                    ValorFiltro = compradorPropio
                });

                #endregion

                this._bc.InitMasterUC(this.masterCompradoCartera, AppMasters.ccCompradorCartera, true, true, true, false, filtros);
                this._bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);

                //Validación para cooperativas
                this.tipoTasaVenta = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_TasaVenta);

                //Tra el sector de cartera
                this.sectorCartera = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                if (Convert.ToByte(sectorCartera) == (byte)SectorCartera.Financiero)
                    this.gvDocument.Columns[this.unboundPrefix + "PortafolioID"].Visible = false;
                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.chkFactorUtilidad.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {

                this.editSpin.Mask.EditMask = "c0";

                //Prevendida
                GridColumn vendidaInd = new GridColumn();
                vendidaInd.FieldName = this.unboundPrefix + "VendidaInd";
                vendidaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VendidaInd");
                vendidaInd.UnboundType = UnboundColumnType.Boolean;
                vendidaInd.VisibleIndex = 1;
                vendidaInd.Width = 42;
                vendidaInd.OptionsColumn.AllowEdit = true;
                vendidaInd.ColumnEdit = editChkBox;
                this.gvDocument.Columns.Add(vendidaInd);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 2;
                libranza.Width = 50;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Cliente Id
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 60;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 180;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombCliente);

                //Comprador Cartera
                GridColumn compradorCarteraID = new GridColumn();
                compradorCarteraID.FieldName = this.unboundPrefix + "CompradorCarteraID";
                compradorCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_CompradorCarteraID");
                compradorCarteraID.UnboundType = UnboundColumnType.String;
                compradorCarteraID.VisibleIndex = 5;
                compradorCarteraID.Width = 50;
                compradorCarteraID.Visible = true;
                compradorCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compradorCarteraID);

                //PortafolioID
                GridColumn portafolioID = new GridColumn();
                portafolioID.FieldName = this.unboundPrefix + "PortafolioID";
                portafolioID.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_PortafolioID");
                portafolioID.UnboundType = UnboundColumnType.String;
                portafolioID.VisibleIndex = 5;
                portafolioID.Width = 50;
                portafolioID.Visible = true;
                portafolioID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(portafolioID);

                //CuotaID
                GridColumn primeraCuota = new GridColumn();
                primeraCuota.FieldName = this.unboundPrefix + "PrimeraCuota";
                primeraCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_PrimeraCuota");
                primeraCuota.UnboundType = UnboundColumnType.Integer;
                primeraCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                primeraCuota.AppearanceCell.Options.UseTextOptions = true;
                primeraCuota.VisibleIndex = 7;
                primeraCuota.Width = 40;
                primeraCuota.Visible = true;
                primeraCuota.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(primeraCuota);

                //NumCuotas
                GridColumn NumCuotas = new GridColumn();
                NumCuotas.FieldName = this.unboundPrefix + "NumCuotas";
                NumCuotas.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_NumCuotas");
                NumCuotas.UnboundType = UnboundColumnType.Integer;
                NumCuotas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                NumCuotas.AppearanceCell.Options.UseTextOptions = true;
                NumCuotas.VisibleIndex = 8;
                NumCuotas.Width = 50;
                NumCuotas.Visible = true;
                NumCuotas.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(NumCuotas);

                //VlrCuota
                GridColumn vlrGiro = new GridColumn();
                vlrGiro.FieldName = this.unboundPrefix + "VlrCuota";
                vlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.docRecursos + "_VlrCuota");
                vlrGiro.UnboundType = UnboundColumnType.Integer;
                vlrGiro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrGiro.AppearanceCell.Options.UseTextOptions = true;
                vlrGiro.VisibleIndex = 9;
                vlrGiro.Width = 100;
                vlrGiro.OptionsColumn.AllowEdit = false;
                vlrGiro.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrGiro);

                //Vlr VlrPrestamo
                GridColumn VlrPrestamo = new GridColumn();
                VlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                VlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                VlrPrestamo.UnboundType = UnboundColumnType.Integer;
                VlrPrestamo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrestamo.AppearanceCell.Options.UseTextOptions = true;
                VlrPrestamo.VisibleIndex = 10;
                VlrPrestamo.Width = 100;
                VlrPrestamo.Visible = true;
                VlrPrestamo.OptionsColumn.AllowEdit = false;
                VlrPrestamo.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrPrestamo);

                //Vlr Nominal
                GridColumn vlrNominal = new GridColumn();
                vlrNominal.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrNominal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrNominal");
                vlrNominal.UnboundType = UnboundColumnType.Integer;
                vlrNominal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrNominal.AppearanceCell.Options.UseTextOptions = true;
                vlrNominal.VisibleIndex = 11;
                vlrNominal.Width = 100;
                vlrNominal.Visible = true;
                vlrNominal.OptionsColumn.AllowEdit = false;
                vlrNominal.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrNominal);

                //Vlr Venta
                GridColumn VlrVenta = new GridColumn();
                VlrVenta.FieldName = this.unboundPrefix + "VlrVenta";
                VlrVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrVenta");
                VlrVenta.UnboundType = UnboundColumnType.Integer;
                VlrVenta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrVenta.AppearanceCell.Options.UseTextOptions = true;
                VlrVenta.VisibleIndex = 12;
                VlrVenta.Width = 100;
                VlrVenta.Visible = true;
                VlrVenta.OptionsColumn.AllowEdit = false;
                VlrVenta.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrVenta);

                //Vlr Utilidad
                GridColumn VlrUtilidad = new GridColumn();
                VlrUtilidad.FieldName = this.unboundPrefix + "VlrUtilidad";
                VlrUtilidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrUtilidad");
                VlrUtilidad.UnboundType = UnboundColumnType.Integer;
                VlrUtilidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrUtilidad.AppearanceCell.Options.UseTextOptions = true;
                VlrUtilidad.VisibleIndex = 13;
                VlrUtilidad.Width = 100;
                VlrUtilidad.Visible = true;
                VlrUtilidad.OptionsColumn.AllowEdit = false;
                VlrUtilidad.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrUtilidad);
                this.gvDocument.OptionsView.ColumnAutoWidth = true;

                this.format = libranza.Caption + formatSeparator + portafolioID.Caption;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            try
            {
                #region Limpia los filtros previos
                this.txtLibranza.Text = String.Empty;
                this.masterCliente.Value = string.Empty;
                this.masterLineaCredito.Value = string.Empty;
                this.libranzaID = String.Empty;
                this.clienteID = string.Empty;
                this.lineaCreditoID = String.Empty;

                this.chkSeleccionadas.Checked = false;

                DateTime fechaIni = this.dtFechaInicio.DateTime;
                DateTime fechaFin = this.dtFechaFin.DateTime;
                #endregion
                DTO_VentaCartera venta = string.IsNullOrWhiteSpace(this.oferta) ? new DTO_VentaCartera() : 
                    this._bc.AdministrationModel.PreventaCartera_GetCreditos(this._actFlujo.ID.Value, this.compradorCarteraID, this.oferta, fechaIni, fechaFin);

                this.creditos = venta.Creditos;
                this.creditosToSave = ObjectCopier.Clone(venta.Creditos);
                if (this.creditos.Count > 0)
                {
                    #region Asigna la información al de los créditos al formulario
           
                    this.loadDocuments = false;
                    if (venta.VentaDocu != null)
                    {
                        this.ventaCartera.VentaDocu.NumeroDoc.Value = venta.VentaDocu.NumeroDoc.Value.Value;
                        this.dtFechaLiquida.DateTime = venta.VentaDocu.FechaLiquida != null ? venta.VentaDocu.FechaLiquida.Value.Value : this.dtFechaLiquida.DateTime;
                    }                       
                    else
                        this.ventaCartera.VentaDocu = new DTO_ccVentaDocu();

                    //Asigna antes la tasa si existe la venta para calcular los valores despues
                    if (venta.VentaDocu != null)
                    {
                        this.txtTasaMensual.EditValue = venta.VentaDocu.FactorCesion.Value.Value;
                        this.tasaMensual = venta.VentaDocu.FactorCesion.Value.Value;
                        this.txtTasaAnual.EditValue = venta.VentaDocu.TasaDescuento.Value.Value;
                        this.tasaAnual = venta.VentaDocu.TasaDescuento.Value.Value;
                    }

                    this.LoadFilters();
                    if (venta.VentaDocu != null)
                    {
                        this.masterCompradoCartera.Value = venta.VentaDocu.CompradorCarteraID.Value;                       
                        this.dtFecha.DateTime = venta.VentaDocu.FechaVenta.Value.Value;
                        this.dtFechaLiquida.DateTime = venta.VentaDocu.FechaLiquida.Value.Value;
                        this.CalcularFechaFlujo();
                        this.dtFechaFlujo1.DateTime = venta.VentaDocu.FechaPago1.Value.Value.Date >= this.dtFechaFlujo1.Properties.MinValue.Date ?
                            venta.VentaDocu.FechaPago1.Value.Value : this.dtFechaFlujo1.Properties.MinValue.Date;                       
                        this.fechaLiquida = venta.VentaDocu.FechaLiquida.Value.Value;
                        this.fechaFlujo = venta.VentaDocu.FechaPago1.Value.Value;
                        this.chkFactorUtilidad.Checked = venta.VentaDocu.FactorUtilidadInd.Value.Value;

                        this.chkSeleccionadas.Checked = false;
                    }
                    //else
                    //{
                    //    this.chkSeleccionadas.Checked = true;
                    //}

                    this.loadDocuments = true;
                    this.EnableHeader(true);

                    // Revisa si la tasa es Anual o mensual
                    if (this.tipoTasaVenta == ((byte)TasaVenta.EfectivaAnual).ToString())
                    {
                        this.txtTasaAnual.Enabled = true;
                        this.txtTasaMensual.Enabled = false;
                    }
                    else 
                    {
                        this.txtTasaAnual.Enabled = false;
                        this.txtTasaMensual.Enabled = true;
                    }

                    //Revisa si habilita la fecha de flujo
                    if (this.compradorCartera.TipoControlRecursos.Value.Value == (byte)TipoControlRecursos.Flujo)
                    {
                        this.dtFechaFlujo1.Enabled = true;
                    }
                    else
                    {
                        this.dtFechaFlujo1.Enabled = false;
                    }

                    #endregion
                }
                else
                {
                    #region No hay info
                    if (venta.VentaDocu == null)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PreventaCartera_NoCredito));
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(this.oferta))
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_PreventaCartera_OfertaExistente));
                        
                        this.EnableHeader(false);
                    }

                    this.gcDocument.DataSource = null;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            if (DateTime.Now.Month != periodo.Month)
            {
                this.dtFecha.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                this.dtFecha.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                this.dtFecha.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                this.dtFecha.Enabled = true;
                this.dtFechaFlujo1.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFechaFlujo1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));

                this.EnableHeader(false);
                this.dtFechaLiquida.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFechaLiquida.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            }
            else
            {
                this.dtFecha.DateTime = DateTime.Now;

                this.dtFecha.Enabled = true;
                this.dtFechaFlujo1.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFechaFlujo1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);

                this.EnableHeader(false);
                this.dtFechaLiquida.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFechaLiquida.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);
            }

            this.dtFechaFin.DateTime = this.dtFechaLiquida.DateTime;
            this.dtFechaInicio.DateTime = this.dtFechaFin.DateTime.AddMonths(-3);
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion que pinta la informacion en la grilla
        /// </summary>
        /// <param name="loadHeader">Indica si tambien se debe pintar la información del cabezote</param>
        private void LoadFilters()
        {
            try
            {
                this.creditosToSave = this.creditos;

                //filtro de seleccionadas
                if (this.chkSeleccionadas.Checked)
                    this.creditosToSave = this.creditosToSave.Where(c => c.VendidaInd.Value.Value).ToList();

                //Filtro de la pagaduria
                if (this.masterLineaCredito.ValidID)
                {
                   this.creditosToSave = this.creditosToSave.Where(c => c.LineaCreditoID.Value == this.masterLineaCredito.Value).ToList();
                }

                //Filtro de la libranza
                if (!string.IsNullOrWhiteSpace(this.txtLibranza.Text))
                {
                    this.creditosToSave = this.creditosToSave.Where(c => c.Libranza.Value.ToString() == this.txtLibranza.Text.Trim()).ToList();
                }

                //Filtro del cliente
                if (this.masterCliente.ValidID)
                {
                    this.creditosToSave = this.creditosToSave.Where(c => c.ClienteID.Value == this.masterCliente.Value).ToList();
                }

                //Carga la información de la venta
                for (int i = 0; i < this.creditosToSave.Count; i++)
                {
                    DTO_ccCreditoDocu credito = this.creditosToSave[i];
                    this.LoadInfoVentaForCredito(credito);
                }

                this.gcDocument.DataSource = null;
                this.creditosToSave = this.creditosToSave.OrderByDescending(c => c.VlrLibranza.Value).ToList();
                this.gcDocument.DataSource = this.creditosToSave;
                this.CalcularTotales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "LoadDocuments"));
            }
        }

        /// <summary>
        /// Carga la información de la venta para un crédito
        /// </summary>
        /// <param name="credito"></param>
        private void LoadInfoVentaForCredito(DTO_ccCreditoDocu credito)
        {
            try
            {
                    //Revisa si existe el plan de pagos
                    List<DTO_ccCreditoPlanPagos> pp;
                    if (!this.planPagos.ContainsKey(credito.NumeroDoc.Value.Value))
                    {
                        pp = _bc.AdministrationModel.GetPlanPagos(credito.NumeroDoc.Value.Value);
                        pp = pp.Where(x => x.VlrPagadoCuota.Value == 0).ToList();
                        this.planPagos[credito.NumeroDoc.Value.Value] = pp;
                        credito.VlrPrestamo.Value = pp.Sum(x => x.VlrCapital.Value);
                        if (pp.Count == 0)
                        {
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldoVenta);
                            MessageBox.Show(string.Format(msg, credito.Libranza.Value.ToString()));
                        }
                    }
                    else
                    {
                        pp = this.planPagos[credito.NumeroDoc.Value.Value];
                        credito.VlrPrestamo.Value = pp.Sum(x => x.VlrCapital.Value);
                    }

                    decimal sumVlrCuota = 0;
                    if (credito.VendidaInd.Value.Value)
                    {
                        #region Calcula el factor de utilidad
                        //DTO_glConsulta consulta = new DTO_glConsulta();
                        //List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        //string compCap = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);

                        //DTO_InfoCredito saldoCred = this._bc.AdministrationModel.GetSaldoCredito(credito.NumeroDoc.Value.Value, DateTime.Now, false, false, false);
                        //List<DTO_ccCreditoPlanPagos> cuotasPendSinAbon = saldoCred.PlanPagos.FindAll(x => x.VlrPagadoCuota.Value == 0).ToList();
                        //int vlrCapital = cuotasPendSinAbon.Count > 0 ? Convert.ToInt32(cuotasPendSinAbon.Sum(x => x.VlrCapital.Value.Value)) : 0;

                        //#region Trae el factor Cesion
                        //filtros.Add(new DTO_glConsultaFiltro()
                        //{
                        //    CampoFisico = "LineaCreditoID",
                        //    OperadorFiltro = OperadorFiltro.Contiene,
                        //    ValorFiltro = credito.LineaCreditoID.Value,
                        //    OperadorSentencia = "AND"
                        //});
                        //filtros.Add(new DTO_glConsultaFiltro()
                        //{
                        //    CampoFisico = "Plazo",
                        //    OperadorFiltro = OperadorFiltro.Contiene,
                        //    ValorFiltro = credito.NumCuotas.Value.ToString(),
                        //    OperadorSentencia = "AND"
                        //});
                        //filtros.Add(new DTO_glConsultaFiltro()
                        //{
                        //    CampoFisico = "Monto",
                        //    OperadorFiltro = OperadorFiltro.Contiene,
                        //    ValorFiltro = vlrCapital.ToString(),
                        //    OperadorSentencia = "AND"
                        //});
                        //filtros.Add(new DTO_glConsultaFiltro()
                        //{
                        //    CampoFisico = "ComponenteCarteraID",
                        //    OperadorFiltro = OperadorFiltro.Contiene,
                        //    ValorFiltro = compCap
                        //});
                        //consulta.Filtros = filtros;
                        //long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.ccLineaComponentePlazo, consulta, true);
                        //List<DTO_ccLineaComponentePlazo> list = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.ccLineaComponentePlazo, count, 1, consulta, true).Cast<DTO_ccLineaComponentePlazo>().ToList();

                        //#endregion

                        //if (list.Count > 0)
                        //{
                        //    DTO_ccLineaComponentePlazo factor = list[0];
                        //    factor.FactorCesion.Value = factor.FactorCesion.Value ?? 0;
                        //    credito.VlrVenta.Value = credito.VlrLibranza.Value * (factor.FactorCesion.Value / 100);
                        //    credito.VlrUtilidad.Value = credito.VlrVenta.Value - vlrCapital;
                        //} 
                        #endregion

                        decimal vpn = 0;
                        decimal saldoCap = 0;
                        decimal saldoOtros = 0;
                        if (pp.Count > 0)
                        {
                            double vlrTemp = Convert.ToDouble(1 + (this.tasaMensual / 100));
                            for (int i = 0; i < pp.Count; ++i)
                            {
                                saldoCap += pp[i].VlrCapital.Value.Value;
                                saldoOtros += pp[i].VlrSeguro.Value.Value + pp[i].VlrOtrosFijos.Value.Value;
                                decimal vlrCuota =  compradorCartera.TipoLiquidacion.Value.Value == (byte)TipoLiquidacionComprador.CapitalInteres ?
                                                    pp[i].VlrCapital.Value.Value + pp[i].VlrInteres.Value.Value : pp[i].VlrCuota.Value.Value;

                                double temp = (1 / Math.Pow(vlrTemp, i + 1));
                                vpn += Convert.ToDecimal(temp) * vlrCuota;

                                sumVlrCuota += vlrCuota;
                            }

                            //decimal vlrCuotaVenta = Convert.ToInt32(sumVlrCuota / pp.Count);
                            //for (int i = 0; i < pp.Count; ++i)
                            //{
                                
                            //}

                            #region Calculo de la tasa diaria y dias de ajuste

                            if (Convert.ToByte(sectorCartera) == (byte)SectorCartera.Financiero)
                            {
                                if (compradorCartera.PeriodoGracia.Value != null)
                                {
                                    int periodoGracia = compradorCartera.PeriodoGracia.Value.Value - 1;
                                    double diasAjuste = periodoGracia * 30;

                                    int diaVenta = this.fechaLiquida.Day == 31 ? 30 : this.fechaLiquida.Day;
                                    int diaCorte = this.dtFechaFlujo1.DateTime.Day == 31 ? 30 : this.dtFechaFlujo1.DateTime.Day;
                                    DateTime fechaTemp = this.dtFechaLiquida.DateTime.AddMonths(periodoGracia);
                                    int diasDif = (int)(this.dtFechaFlujo1.DateTime.Date - fechaTemp).TotalDays;
                                    int mesesDif = Convert.ToInt32(diasDif / 30) - 1;

                                    diasAjuste += mesesDif * 30;
                                    if (diaCorte >= diaVenta)
                                    {
                                        diasAjuste += diaCorte - this.fechaLiquida.Day;
                                    }
                                    else
                                    {
                                        int diasAux = 30 - this.fechaLiquida.Day + diaCorte + 1;
                                        if (diasAux > 30)
                                            diasAux = 30;

                                        diasAjuste += diasAux;
                                    }
                                    double tasaDia = Math.Pow(1 + Convert.ToDouble((tasaMensual / 100)), (1d / 30d)) - 1;
                                    vpn *= Convert.ToDecimal(Math.Pow(1 + tasaDia / 100, -diasAjuste));

                                    //Variables del credito
                                    vpn = Convert.ToInt32(vpn);
                                    credito.VlrVenta.Value = vpn;
                                    credito.VlrUtilidad.Value = vpn - saldoCap;
                                }
                            }
                            else 
                            {
                                decimal mayorValor = 0;
                                decimal vlrOtros = 0;
                                decimal vlrAsistencias = 0;
                                decimal porCapital = 0;
                                decimal porAsistencia = 0;
                                decimal porProvGral = string.IsNullOrEmpty(this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ReservaUtilidad)) ? 0 : Convert.ToDecimal(this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ReservaUtilidad));
                                string comAsistencia = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteAsistencia);
                                string comCap = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteCapital);
                                string compInteres = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.cc, AppControl.cc_ComponenteInteresCorriente);

                                if(credito.Detalle.Count == 0 )
                                    credito.Detalle = this._bc.AdministrationModel.ccCreditoComponentes_GetByNumDocCred(credito.NumeroDoc.Value.Value);
                                foreach (var credComp in credito.Detalle)
                                {
                                    DTO_ccCarteraComponente comp = (DTO_ccCarteraComponente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCarteraComponente, false, credComp.ComponenteCarteraID.Value, true);
                                    mayorValor += comp != null && comp.TipoComponente.Value == (byte)TipoComponente.MayorValor? credComp.TotalValor.Value.Value : 0;
                                    if (credComp.ComponenteCarteraID.Value == comAsistencia)
                                        porAsistencia = credComp.PorCapital.Value.Value;
                                    else if (credComp.ComponenteCarteraID.Value == comCap)
                                        porCapital = credComp.PorCapital.Value.Value;

                                    if (comp.TipoComponente.Value == (byte)TipoComponente.ComponenteCuota && credComp.ComponenteCarteraID.Value != compInteres)
                                        vlrOtros += credComp.TotalValor.Value.Value;
                                    if (comp.AsistenciaInd.Value.Value)
                                        vlrAsistencias += credComp.TotalValor.Value.Value;
                                }                                
                                int diaAjuste =this.dtFechaFlujo1.DateTime.Day - this.dtFechaLiquida.DateTime.Day;
                                decimal factorGr = Math.Round(Convert.ToDecimal(Math.Pow(1 + vlrTemp, -compradorCartera.PeriodoGracia.Value.Value)), 10);
                                vpn = Math.Round((vpn * factorGr),0);
                                double tasaDia = Math.Pow(1 + Convert.ToDouble((tasaMensual / 100)), (1d / 30d)) - 1;
                                decimal desc = Math.Round((vpn * Convert.ToDecimal(Math.Pow(1 + tasaDia, -diaAjuste))), 0);                               
                                compradorCartera.PorReservaVta.Value = compradorCartera.PorReservaVta.Value ?? 0;
                                credito.VlrComision.Value = Math.Round(desc * (compradorCartera.PorReservaVta.Value.Value / 100), 0);
                                credito.VlrVenta.Value = desc - credito.VlrComision.Value;
                                credito.VlrUtilidad.Value = desc - saldoCap + mayorValor;
                                if (porCapital == 100)
                                {
                                    credito.VlrSdoCapital.Value = saldoCap;
                                    credito.VlrSdoAsistencias.Value = Math.Round((saldoOtros * vlrAsistencias) / vlrOtros,0);
                                    credito.VlrSdoOtros.Value = 0;
                                    //Valida si el decimal es mayor a 0.5 para redondear (error de redondeo)
                                    decimal vlrDec = (saldoCap + vlrAsistencias) * (porProvGral / 100) - (int)((saldoCap + vlrAsistencias) * (porProvGral / 100));
                                    if (vlrDec == (decimal)(0.5))
                                        credito.VlrProvGeneral.Value = Math.Ceiling((saldoCap + vlrAsistencias) * (porProvGral / 100));
                                    else
                                        credito.VlrProvGeneral.Value = Math.Round((saldoCap + vlrAsistencias) * (porProvGral / 100), 0);
                                }
                                else
                                {
                                    credito.VlrSdoCapital.Value = Math.Round(saldoCap * (porCapital / 100), 0);
                                    credito.VlrSdoAsistencias.Value = saldoCap - credito.VlrSdoCapital.Value;// Math.Round(saldoCap * (porAsistencia / 100), 0);
                                    credito.VlrSdoOtros.Value = 0;// Math.Round(saldoCap * ((100 - porCapital - porAsistencia) / 100), 0);
                                    //Valida si el decimal es mayor a 0.5 para redondear (error de redondeo)
                                    decimal vlrDec = (saldoCap) * (porProvGral / 100) - (int)((saldoCap) * (porProvGral / 100));
                                    if (vlrDec == (decimal)(0.5))
                                        credito.VlrProvGeneral.Value = Math.Ceiling((saldoCap) * (porProvGral / 100));
                                    else
                                        credito.VlrProvGeneral.Value = Math.Round((saldoCap) * (porProvGral / 100), 0);
                                }
                              
                             
                            }
                            #endregion
                        }                       
                    }
                    else
                    {
                        for (int i = 0; i < pp.Count; ++i)
                        {
                            sumVlrCuota += this.compradorCartera.TipoLiquidacion.Value.Value == (byte)TipoLiquidacionComprador.CapitalInteres ?
                                pp[i].VlrCapital.Value.Value + pp[i].VlrInteres.Value.Value : pp[i].VlrCuota.Value.Value; 
                        }

                        //Variables del credito
                        credito.VlrVenta.Value = 0;
                        credito.VlrUtilidad.Value = 0;
                    }

                    credito.PrimeraCuota.Value = pp.Count > 0 ? pp[0].CuotaID.Value.Value : 0;
                    credito.VlrCuota.Value = pp.Count > 0 ? Convert.ToInt32(sumVlrCuota / pp.Count) : 0;
                    credito.VlrLibranza.Value = sumVlrCuota;
                    credito.NumCuotas.Value = pp.Count;           
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "LoadInfoVenta"));
            }
        }

        /// <summary>
        /// (Des)Habilita los campos del cabezote
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableHeader(bool enabled)
        {
            this.masterCliente.EnableControl(enabled);
            this.masterLineaCredito.EnableControl(enabled);
            this.txtLibranza.Enabled = enabled;
            this.dtFechaLiquida.Enabled = enabled;

            this.txtTasaAnual.Enabled = enabled;
            this.txtTasaMensual.Enabled = enabled;
            this.dtFechaLiquida.Enabled = enabled;
            this.dtFechaFlujo1.Enabled = enabled;
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.loadDocuments = false;
            this.txtTasaMensual.EditValue = 0;
            this.txtTasaAnual.EditValue = 0;
            this.txtVenta.Text = "0";
            this.txtPreseleccion.Text = "0";
            this.txtVlrTotalNominal.EditValue = 0;
            this.txtTotalPrestamo.EditValue = 0;

            this.tasaMensual = 0;

            this.compradorCarteraID = String.Empty;
            this.libranzaID = String.Empty;
            this.clienteID = String.Empty;
            this.oferta = String.Empty;
            this.lineaCreditoID = string.Empty;
            this.EnableHeader(false);

            this.compradorCartera = null;
            this.creditos = new List<DTO_ccCreditoDocu>();
            this.creditosToSave = new List<DTO_ccCreditoDocu>();
            this.planPagos = new Dictionary<int, List<DTO_ccCreditoPlanPagos>>();
            
            this.fechaLiquida = DateTime.Now;
            this.fechaFlujo = DateTime.Now;
            this.masterCompradoCartera.Value = this.compradorCarteraID;
            this.masterCliente.Value = this.clienteID;
            this.masterLineaCredito.Value = this.lineaCreditoID;

            this.cmbOferta.Text = string.Empty;
            this.cmbOferta.Items.Clear();
            this.gcDocument.DataSource = this.creditos;
            this.txtTasaMensual.Enabled = false;
            this.txtTasaAnual.Enabled = false;
            this.cmbOferta.Enabled = false;
            this.dtFechaLiquida.Enabled = false;
            this.dtFechaFlujo1.Enabled = false;
            this.dtFechaLiquida.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            this.dtFechaFlujo1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));

            this.loadDocuments = true;
        }

        /// <summary>
        /// Funcion que establece los valores de los controles de la pantalla
        /// </summary>
        private void CalcularTotales()
        {
            if (this.creditos != null && this.creditos.Count > 0)
            {
                this.txtPreseleccion.Text = this.creditosToSave.Count().ToString();
                this.txtVenta.Text = this.creditosToSave.Where(c => c.VendidaInd.Value.Value).Count().ToString();
                this.txtTotalPrestamo.EditValue = this.creditosToSave.Sum(x => x.VlrVenta.Value.Value);
                this.txtVlrTotalNominal.EditValue = this.creditosToSave.Sum(x => x.VlrLibranza.Value.Value);
            }
        }

        /// <summary>
        /// Calcula la tasa efectiva anual y mensual
        /// </summary>
        private void CalcularTasas(decimal tasa)
        {
            try 
            {
                if (this.compradorCartera != null)
                {
                    // Revisa si es Anual o mensual
                    if (this.tipoTasaVenta == ((byte)TasaVenta.EfectivaAnual).ToString())
                    {
                        this.tasaAnual = tasa;

                        double te1 = Convert.ToDouble(tasaAnual / 100);
                        this.tasaMensual = Convert.ToDecimal(Math.Pow(te1 + 1, 1d / 12d)) - 1;
                        this.tasaMensual *= 100;

                        this.txtTasaAnual.Enabled = true;
                        this.txtTasaMensual.Enabled = false;
                    }
                    else
                    {
                        this.tasaMensual = tasa;
                        this.tasaAnual = Convert.ToDecimal(Math.Pow((1 + (double)tasaMensual/100), 12)) - 1;
                        this.tasaAnual *= 100;

                        this.txtTasaAnual.Enabled = false;
                        this.txtTasaMensual.Enabled = true;
                    }

                    this.txtTasaMensual.EditValue = this.tasaMensual;
                    this.txtTasaAnual.EditValue = this.tasaAnual;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "CalcularTasas"));
            }
        }

        /// <summary>
        /// Calcula la fecha 1 del flujo
        /// </summary>
        private void CalcularFechaFlujo()
        {
            try
            {
                this.fechaLiquida = this.dtFechaLiquida.DateTime.Date;
                if (this.compradorCartera.TipoControlRecursos.Value.Value == (byte)TipoControlRecursos.Flujo)
                {
                    this.dtFechaFlujo1.Enabled = true;
                    //Agrega un mes por defecto
                    DateTime fechaTemp = new DateTime(this.fechaLiquida.Year, this.fechaLiquida.Month, this.compradorCartera.DiaCorte.Value.Value);
                    //fechaTemp = fechaTemp.AddMonths(1);

                    //Agrega segun los periodos de gracia
                    fechaTemp = fechaTemp.AddMonths(this.compradorCartera.PeriodoGracia.Value.Value);

                    //Valida dia de liquida con dias corte para agregar un mes
                    if(this.fechaLiquida.Day > this.compradorCartera.DiaCorte.Value.Value)
                        fechaTemp = fechaTemp.AddMonths(1);

                    this.dtFechaFlujo1.DateTime = fechaTemp;
                    this.dtFechaFlujo1.Properties.MinValue = this.dtFechaFlujo1.DateTime;
                }
                else
                {
                    this.dtFechaFlujo1.Enabled = false;
                    this.dtFechaFlujo1.DateTime = this.dtFechaLiquida.DateTime.AddMonths(1);
                    this.dtFechaFlujo1.Properties.MinValue = this.dtFechaFlujo1.DateTime;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "CalcularFechaFlujo"));
            }
        }

        /// <summary>
        /// Salva las preventas
        /// </summary>
        private void Save()
        {
            this.gvDocument.PostEditor();
            try
            {
                this.ventaCartera.VentaDeta = new List<DTO_ccVentaDeta>();
                List<DTO_ccCreditoDocu> ventasFinal = this.creditosToSave.Where(x => x.VendidaInd.Value.Value || x.IsPreventa.Value.Value).ToList();
                if (ventasFinal.Count() > 0)
                {
                    //Variables de validaciones
                    DTO_TxResult result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();

                    #region Carga la informacion del portafolio

                    List<string> portafolios = new List<string>();
                    if (this.compradorCartera.PortafolioInd.Value.Value)
                    {
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "CompradorCarteraID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = this.compradorCarteraID,

                        });
                        consulta.Filtros.AddRange(filtros);

                        List<DTO_MasterComplex> masterDTOs = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.ccCompradorPortafilio, 100, 1, consulta, true).ToList();
                        if (masterDTOs.Count == 0)
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNoPortafolio), this.compradorCarteraID);
                            MessageBox.Show(msg);
                            return;
                        }

                        List<DTO_ccCompradorPortafilio> portafoliosDTO = masterDTOs.Cast<DTO_ccCompradorPortafilio>().ToList();
                        portafoliosDTO.ForEach(p => portafolios.Add(p.Portafolio.Value.ToUpper()));
                    }

                    #endregion
                    #region Validaciones
                    int i = 0;
                    foreach (DTO_ccCreditoDocu credito in ventasFinal)
                    {
                        ++i;
                        if (this.compradorCartera.PortafolioInd.Value.Value && credito.VendidaInd.Value.Value)
                        {
                            if (string.IsNullOrWhiteSpace(credito.PortafolioID.Value))
                            {
                                //Valida el portafolio
                                result.Result = ResultValue.NOK;
                                result.Details.Add(new DTO_TxResultDetail()
                                {
                                    line = i,
                                    Message = DictionaryMessages.Err_Cc_CreditoSinPortafolio + "&&" + credito.Libranza.Value.ToString()
                                });
                            }
                            else if (!portafolios.Contains(credito.PortafolioID.Value.ToUpper()))
                            {
                                //Valida el portafolio
                                result.Result = ResultValue.NOK;
                                result.Details.Add(new DTO_TxResultDetail()
                                {
                                    line = i,
                                    Message = DictionaryMessages.Err_Cc_InvalidPortafolioComprador + "&&" + credito.Libranza.Value.ToString()
                                });
                            }
                        }
                    }

                    if (result.Result == ResultValue.NOK)
                    {
                        MessageForm msgForm = new MessageForm(result);
                        msgForm.ShowDialog();

                        return;
                    }

                    #endregion
                    #region Carga el DTO de Venta Docu

                    int plazoMaximo = this.creditosToSave.Max(x => x.NumCuotas.Value.Value);
                    
                    this.ventaCartera.VentaDocu.CompradorCarteraID.Value = this.compradorCarteraID;
                    this.ventaCartera.VentaDocu.TipoVenta.Value = 1; //Recaudo
                    this.ventaCartera.VentaDocu.FactorCesion.Value = this.tasaMensual;
                    this.ventaCartera.VentaDocu.TasaDescuento.Value = this.tasaAnual;
                    this.ventaCartera.VentaDocu.Oferta.Value = this.oferta;
                    this.ventaCartera.VentaDocu.VlrVenta.Value = Convert.ToDecimal(this.txtTotalPrestamo.EditValue, CultureInfo.InvariantCulture);
                    this.ventaCartera.VentaDocu.FechaPago1.Value = this.dtFechaFlujo1.DateTime;
                    this.ventaCartera.VentaDocu.FechaLiquida.Value = this.dtFechaLiquida.DateTime;
                    this.ventaCartera.VentaDocu.NumCuotas.Value = (short)plazoMaximo;
                    this.ventaCartera.VentaDocu.FechaPagoUlt.Value = this.dtFechaFlujo1.DateTime.AddMonths(plazoMaximo);
                    this.ventaCartera.VentaDocu.TerceroID.Value = this.terceroID;
                    this.ventaCartera.VentaDocu.FactorUtilidadInd.Value = this.chkFactorUtilidad.Checked;
                    #endregion
                    #region Carga los detalles de la venta
                    this.ventaCartera.VentaDeta = new List<DTO_ccVentaDeta>();
                    foreach (DTO_ccCreditoDocu credito in ventasFinal)
                    {
                        #region Carga el crédito
                        credito.TasaEfectivaVenta.Value = this.tasaAnual;

                        //Detalle de venta deta
                        DTO_ccVentaDeta ventaDeta = new DTO_ccVentaDeta();
                        ventaDeta.Aprobado.Value = credito.VendidaInd.Value;
                        ventaDeta.AsignaCarteraInd.Value = credito.VendidaInd.Value;
                        ventaDeta.NumDocCredito.Value = credito.NumeroDoc.Value;
                        ventaDeta.NumDocSustituye.Value = credito.NumeroDocSustituye.Value != null ? credito.NumeroDocSustituye.Value : null;
                        ventaDeta.Portafolio.Value = credito.PortafolioID.Value;
                        ventaDeta.CuotaID.Value = credito.PrimeraCuota.Value;
                        ventaDeta.VlrCuota.Value = credito.VlrCuota.Value;
                        ventaDeta.CuotasVend.Value = credito.NumCuotas.Value;
                        ventaDeta.VlrLibranza.Value = credito.VlrLibranza.Value;
                        ventaDeta.VlrVenta.Value = credito.VlrVenta.Value;
                        ventaDeta.VlrUtilidad.Value = credito.VlrUtilidad.Value;
                        ventaDeta.VlrTotalDerechos.Value = credito.VlrUtilidad.Value;
                        ventaDeta.VlrProvComprador.Value = credito.VlrComision.Value;
                        ventaDeta.VlrProvGeneral.Value = credito.VlrProvGeneral.Value;
                        ventaDeta.VlrSdoCapital.Value = credito.VlrSdoCapital.Value;
                        ventaDeta.VlrSdoAsistencias.Value = credito.VlrSdoAsistencias.Value;
                        ventaDeta.VlrSdoOtros.Value = credito.VlrSdoOtros.Value;
                        ventaDeta.FactorCesion.Value = this.tasaMensual;
                        ventaDeta.VlrSustLibranza.Value = credito.LibranzaSustituye.Value != null ? credito.VlrLibranza.Value : null;
                        ventaDeta.VlrNeto.Value = credito.VlrUtilidad.Value.Value;
                        ventaDeta.CompradorFinal.Value = this.compradorCarteraID;
                        ventaDeta.IsPreventa.Value = credito.IsPreventa.Value.Value;
                        this.ventaCartera.VentaDeta.Add(ventaDeta);
                        #endregion
                    }
                    #endregion

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "Save"));
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
                base.Form_Enter(sender, e);

                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = true;
                FormProvider.Master.itemImport.Visible = true;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemExport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que determina el comprado de cartera del credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCompradoCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.masterCompradoCartera.Value))
                {
                    if (this.compradorCarteraID != this.masterCompradoCartera.Value)
                    {
                        if (this.masterCompradoCartera.ValidID)
                        {
                            this.compradorCarteraID = this.masterCompradoCartera.Value;
                            #region Valida que el comprador de cartera sea diferente al del control de cartera
                            
                            string compCarteraControl = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);
                            if (String.Equals(this.compradorCarteraID, compCarteraControl))
                            {
                                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNotValid), this.compradorCarteraID);
                                MessageBox.Show(msg);
                                return;
                            }

                            this.compradorCartera = (DTO_ccCompradorCartera)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCompradorCartera, false, this.compradorCarteraID, true);
                           
                            #endregion
                            #region Carga la informacion del comprador de cartera y la tasa 
                    
                            if (String.IsNullOrWhiteSpace(this.compradorCartera.FactorCesion.Value.ToString()))
                            {
                                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNoFactor), this.compradorCarteraID);
                                MessageBox.Show(msg);
                                return;
                            }
                            this.terceroID = this.compradorCartera.TerceroID.Value;
                            
                            #endregion
                            #region Calcula las tasas
                            this.CalcularTasas(this.compradorCartera.FactorCesion.Value.Value);
                            #endregion
                            #region Calcula la fecha de flujo
                            this.CalcularFechaFlujo();
                            #endregion
                            #region Carga la informacion del portafolio

                            if (this.compradorCartera.PortafolioInd.Value.Value)
                                this.gvDocument.Columns[this.unboundPrefix + "PortafolioID"].OptionsColumn.AllowEdit = true;
                            else
                                this.gvDocument.Columns[this.unboundPrefix + "PortafolioID"].OptionsColumn.AllowEdit = false;

                            #endregion
                            #region Limpia los filtros previos
                            this.txtLibranza.Text = String.Empty;
                            this.masterCliente.Value = String.Empty;
                            this.libranzaID = String.Empty;
                            this.clienteID = String.Empty;
                            this.masterLineaCredito.Value = String.Empty;
                            this.lineaCreditoID = String.Empty;
                            #endregion
                            #region Carga las ofertas
                           
                            this.cmbOferta.SelectedValueChanged -= new System.EventHandler(this.cmbOferta_Leave);
                            this.cmbOferta.Items.Clear();
                            List<string> ofertas = _bc.AdministrationModel.PreventaCartera_GetOfertas(this.compradorCarteraID, false);
                            ofertas.ForEach(o => this.cmbOferta.Items.Add(o.Trim()));
                            this.cmbOferta.SelectedIndex = -1;
                            this.cmbOferta.SelectedValueChanged += new System.EventHandler(this.cmbOferta_Leave);
                            this.cmbOferta.Enabled = true;
                            this.cmbOferta.Focus();
                            
                            #endregion
                            this.gvDocument.Columns[1].OptionsColumn.AllowEdit = true;
                            this.chkSeleccionadas_CheckedChanged(sender, e);
                        }
                        else
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCompradoCartera.LabelRsx);
                            MessageBox.Show(msg);
                            this.CleanData();
                            this.masterCompradoCartera.Focus();
                        }
                    }
                }
                else
                {
                    this.compradorCarteraID = String.Empty;
                    this.gvDocument.Columns[1].OptionsColumn.AllowEdit = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "masterPagaduria_Leave"));
            }
        }

        /// <summary>
        /// Evento que determina cuando se selecciona una nueva oferta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbOferta_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.compradorCarteraID) && this.masterCompradoCartera.ValidID)//&& !string.IsNullOrWhiteSpace())
                {
                    this.oferta = this.cmbOferta.Text.Trim();
                    this.LoadData(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "cmbOferta_SelectedValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el valor de la tasa de venta
        /// </summary>       
        private void txtTasaMensual_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal tasaTemp = Convert.ToDecimal(this.txtTasaMensual.EditValue);
                if (this.creditos != null && this.creditos.Count > 0 && this.tasaMensual != tasaTemp)
                {
                    this.tasaMensual = Convert.ToDecimal(this.txtTasaMensual.EditValue);
                    this.CalcularTasas(this.tasaMensual);
                    this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "txtTasaMensual_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el valor de la tasa de venta
        /// </summary>       
        private void txtTasaAnual_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal tasaTemp = Convert.ToDecimal(this.txtTasaAnual.EditValue);
                if (this.creditos != null && this.creditos.Count > 0 && this.tasaAnual != tasaTemp)
                {
                    this.tasaAnual = Convert.ToDecimal(this.txtTasaAnual.EditValue);
                    this.CalcularTasas(this.tasaAnual);
                    this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "txtTasaAnual_Leave"));
            }
        }

        /// <summary>
        /// Valida que el usuario haya ingresado una fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void dtFecha_DateTimeChanged(object sender, EventArgs e)
        {
            this.dtFechaFlujo1.Properties.MinValue = this.dtFecha.DateTime;
            this.dtFechaLiquida.Properties.MinValue = this.dtFecha.DateTime;
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el valor de la tasa de venta
        /// </summary>       
        private void dtFechaLiquida_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.loadDocuments && this.fechaLiquida != this.dtFechaLiquida.DateTime.Date)
                {
                    DateTime fechaFlujoTemp = this.fechaFlujo;
                    this.CalcularFechaFlujo();

                    if (this.fechaFlujo == fechaFlujoTemp)
                        this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "dtFechaVenta_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cambia el valor de la tasa de venta
        /// </summary>       
        private void dtFechaFlujo1_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.loadDocuments && this.fechaFlujo != this.dtFechaFlujo1.DateTime.Date)
                {
                    this.fechaFlujo = this.dtFechaFlujo1.DateTime.Date;
                    this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "dtFechaVenta_DateTimeChanged"));
            }
        }

        /// <summary>
        /// Evento que filtra los creditos por medio de la pagaduria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterLineaCredito_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.lineaCreditoID != this.masterLineaCredito.Value)
                {
                    if (string.IsNullOrWhiteSpace(this.masterLineaCredito.Value) || this.masterLineaCredito.ValidID)
                    {
                        this.creditosToSave = this.creditos;
                        this.lineaCreditoID = this.masterLineaCredito.Value;
                        this.LoadFilters();
                    }
                    else
                    {
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "masterPagaduria_Leave"));
            }
        }

        /// <summary>
        /// Evento que filtra los creditos por la libranza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.libranzaID != this.txtLibranza.Text.Trim())
                {
                    this.creditosToSave = this.creditos; 
                    this.libranzaID = this.txtLibranza.Text;
                    this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que filtra los creditos por el cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.clienteID != this.masterCliente.Value)
                {
                    if (string.IsNullOrWhiteSpace(this.masterCliente.Value) || this.masterCliente.ValidID)
                    {
                        this.creditosToSave = this.creditos; 
                        this.clienteID = this.masterCliente.Value;
                        this.LoadFilters();
                    }
                    else
                    {
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Evento que muestra los creditos disponibles para asignar comprador de cartera
        /// </summary>
        private void chkSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.creditos != null && this.creditos.Count > 0 && this.loadDocuments)
                {
                    this.LoadFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "chkLibres_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que asigna un valor aprobado o rechazado a todos los registros
        /// </summary>
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkAll.Checked)
                {
                    for (int i = 0; i < this.creditosToSave.Count; i++)
                    {
                        DTO_ccCreditoDocu credito = this.creditosToSave[i];

                        credito.VendidaInd.Value = true;
                        credito.CompradorCarteraID.Value = this.masterCompradoCartera.Value;
                        this.LoadInfoVentaForCredito(credito);
                    }
                }
                else
                {
                    for (int i = 0; i < this.gvDocument.DataRowCount; i++)
                    {
                        DTO_ccCreditoDocu credito = this.creditosToSave[i];

                        credito.VendidaInd.Value = false;
                        credito.PortafolioID.Value = String.Empty;
                        credito.CompradorCarteraID.Value = String.Empty;
                        this.LoadInfoVentaForCredito(credito);
                    }
                }

                this.CalcularTotales();
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "chkAll_CheckedChanged"));
            }
        }

        /// <summary>
        /// Evento que calcula el factor de utilidad 
        /// </summary>
        private void chkFactorUtilidad_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (var crd in this.creditosToSave)
                    this.LoadInfoVentaForCredito(crd);
             
                this.CalcularTotales();
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "chkAll_CheckedChanged"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "VendidaInd")
                {
                    DTO_ccCreditoDocu credito = (DTO_ccCreditoDocu)this.gvDocument.GetFocusedRow();
                    if ((bool)e.Value)
                    {
                        credito.VendidaInd.Value = true;
                        credito.CompradorCarteraID.Value = this.masterCompradoCartera.Value;
                    }
                    else
                    {
                        credito.VendidaInd.Value = false;
                        credito.PortafolioID.Value = string.Empty;
                        credito.CompradorCarteraID.Value = string.Empty;
                    }

                    this.LoadInfoVentaForCredito(credito);
                    this.CalcularTotales();
                    this.gcDocument.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "gvDocument_CellValueChanging"));
            }
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Funcion que limpia el formulario
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Funcion que refresca el formulario
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this._actFlujo.ID.Value) && this.compradorCartera != null && !string.IsNullOrWhiteSpace(this.oferta))
                {
                    this.LoadData(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "TBUpdate"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.sendToApprove = false;
            this.Save();
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.sendToApprove = true;
            this.Save();
        }

        /// <summary>
        /// Boton para generar la plantilla de importar datos
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

                //excell_app.AddData(1, 1, libranzaCol);

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar los datos de la plantilla
        /// </summary>
        public override void TBImport()
        {
            try
            {
                if (this.creditosToSave.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                        Thread process = new Thread(this.ImportThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBGenerateTemplate.cs", "TBImport"));
            }
        }

        /// <summary>
        /// Boton para Exportar datos de la Planilla
        /// </summary>
        public override void TBExport()
        {
            try
            {
                List<DTO_ccCreditoDocu> ventasFinal = this.creditosToSave.Where(x => x.VendidaInd.Value.Value).ToList();
                if (ventasFinal.Count > 0)
                {
                    numeroDocs = ventasFinal.Select(x => x.NumeroDoc.Value.Value).Distinct().ToList();
                    if (numeroDocs.Count != 0)
	                {
		                vistaCesiones = this._bc.AdministrationModel.ExportExcel_cc_GetVistaCesionesByPreventa(this.numeroDocs); 
	                }

                    //Exporta Reporte a Excel.
                    if (this.vistaCesiones.Rows.Count != 0)
                    {
                        ReportExcelBase frm = new ReportExcelBase(this.vistaCesiones);
                        frm.Show();
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex,"WinApp-TBExport.cs", "TBExport"));
                throw;
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                Tuple<int, List<DTO_SerializedObject>> tuple;
                tuple = _bc.AdministrationModel.PreventaCartera_Add(this.documentID, this._actFlujo.ID.Value, this.sendToApprove, this.dtFechaLiquida.DateTime, this.ventaCartera);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Ordena los resultados
                int i = 0;
                int percent = 0;
                List<DTO_SerializedObject> results = tuple.Item2;
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
                #region Envía correos o muestra los errores
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

                        if (this.creditosToSave[i].VendidaInd.Value.Value)
                        {
                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.Preventa, this._actFlujo.seUsuarioID.Value, obj, false);
                            if (!isOK)
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                        }

                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.saveDelegate);
                #endregion
                #region Genera Reportes (en comentarios)

                //if (tuple.Item1 != 0)
                //{
                //    reportName = _bc.AdministrationModel.Report_Cc_Oferta(tuple.Item1, ExportFormatType.pdf);
                //    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, tuple.Item1, null, reportName.ToString());
                //    Process.Start(fileURl);
                //}
                //else
                //{
                //    string message = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_NoseGeneroReporte);
                //    MessageBox.Show(message);
                //}

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error

                    DTO_TxResult result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    List<DTO_ccCreditoDocu> creditosTemp = ObjectCopier.Clone(this.creditos);

                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgInvalidLibranza = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNotInList);
                    string msgNoPortafolio = _bc.GetResourceError(DictionaryMessages.Err_Cc_CreditoSinPortafolio);
                    string msgInvalidPortafolio = _bc.GetResourceError(DictionaryMessages.Err_Cc_InvalidPortafolioComprador);

                    bool validList = true;
                    #endregion
                    #region Carga la informacion del portafolio

                    List<string> portafolios = new List<string>();
                    if (this.compradorCartera.PortafolioInd.Value.Value)
                    {
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "CompradorCarteraID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = this.compradorCarteraID,

                        });
                        consulta.Filtros.AddRange(filtros);

                        List<DTO_MasterComplex> masterDTOs = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.ccCompradorPortafilio, 100, 1, consulta, true).ToList();
                        if (masterDTOs.Count == 0)
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNoPortafolio), this.compradorCarteraID);
                            MessageBox.Show(msg);
                            return;
                        }

                        List<DTO_ccCompradorPortafilio> portafoliosDTO = masterDTOs.Cast<DTO_ccCompradorPortafilio>().ToList();
                        portafoliosDTO.ForEach(p => portafolios.Add(p.Portafolio.Value.ToUpper()));
                    }

                    #endregion
                    #region Llena las listas de las columnas

                    Dictionary<string, string> libranzas = new Dictionary<string, string>();
                    List<string> colNames = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    string colRsxLibranza = colNames[0];
                    string colRsxPortafolio = colNames[1];

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer

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
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            bool validLibranza = true;
                            string libranza = string.Empty;
                            string portafolio = string.Empty;

                            #region Info básica
                            
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length > 2)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                validList = false;
                                continue;
                            }
                            else
                            {
                                libranza = line[0];
                                portafolio = line[1];
                            }

                            #endregion
                            #region Validacion de Nulls
                            if (string.IsNullOrEmpty(libranza))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = colRsxLibranza;
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                validLibranza = false;
                            }

                            #endregion
                            #region Valida el portafolio

                            if (this.compradorCartera.PortafolioInd.Value.Value)
                            {
                                if (string.IsNullOrEmpty(portafolio))
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = colRsxPortafolio;
                                    rdF.Message = msgNoPortafolio;
                                    rd.DetailsFields.Add(rdF);

                                    validLibranza = false;
                                }
                                else if (!portafolios.Contains(portafolio.Trim().ToUpper()))
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = colRsxPortafolio;
                                    rdF.Message = msgInvalidPortafolio;
                                    rd.DetailsFields.Add(rdF);

                                    validLibranza = false;
                                }
                            }
                            #endregion
                            #region Validacion Formatos
                            if (validLibranza)
                            {
                                try
                                {
                                    int val = Convert.ToInt32(libranza);
                                }
                                catch (Exception ex)
                                {
                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                    rdF.Field = colRsxLibranza;
                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                    rd.DetailsFields.Add(rdF);

                                    validLibranza = false;
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Details.Add(rd);
                                result.Result = ResultValue.NOK;
                            }

                            if (validLibranza && validList)
                                libranzas.Add(libranza, portafolio);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida que existan las libranza

                    int j = 0;
                    foreach (string lib in libranzas.Keys)
                    {
                        ++j;
                        if (creditosTemp.Any(c => c.Libranza.Value.ToString() == lib))
                        {
                            creditosTemp.FirstOrDefault(c => c.Libranza.Value.ToString() == lib).VendidaInd.Value = true;
                            creditosTemp.FirstOrDefault(c => c.Libranza.Value.ToString() == lib).PortafolioID.Value = libranzas[lib];
                            creditosTemp.FirstOrDefault(c => c.Libranza.Value.ToString() == lib).CompradorCarteraID.Value = this.compradorCarteraID;
                        }
                        else
                        {
                            result.Result = ResultValue.NOK;
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.line = j;
                            rd.Message = string.Format(msgInvalidLibranza, lib);

                            result.Details.Add(rd);
                            validList = false;
                        }
                    }
                    this.Invoke(this.refreshGridDelegate);

                    #endregion

                    if (validList)
                    {
                        this.creditos = creditosTemp;
                        this.Invoke(this.refreshGridDelegate); 
                    }
                    else 
                    {
                        MessageForm msg = new MessageForm(result);
                        msg.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreventaCartera.cs", "ImportThread"));
            }
            finally
            {
                if (!this.pasteRet.Success)
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
        }

        #endregion

    }
}
