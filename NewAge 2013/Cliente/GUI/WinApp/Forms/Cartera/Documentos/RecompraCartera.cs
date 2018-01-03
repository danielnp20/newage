using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RecompraCartera : DocumentForm
    {
        #region Delegados
        
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            if (this.opcRecompra == this.comboVal_RecompraMasiva)
            {
                this.LoadData(false);
                foreach (DTO_ccRecompraDeta rec in this.listToSave)
                    this.CalcularRecompra(true,rec);
            }
            else
            {
                this.gcDocument.DataSource = null;
                this.gcDocument.DataSource = this.listToSave;
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
        }

        #endregion

        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
                
        private DTO_RecompraCartera recompraCartera = new DTO_RecompraCartera();
        private List<DTO_ccRecompraDeta> listMaduracion = new List<DTO_ccRecompraDeta>();
        private List<DTO_ccRecompraDeta> listRecompras = new List<DTO_ccRecompraDeta>();
        private List<DTO_ccRecompraDeta> listToSave = new List<DTO_ccRecompraDeta>();
        private DTO_ccRecompraDeta _rowCurrent = new DTO_ccRecompraDeta();
        private SectorCartera sector = SectorCartera.Solidario;
        List<int> currentLibranzas = new List<int>();
        private string tipoTasaVenta;
        private string compradorID = string.Empty;
        private DTO_ccCompradorCartera comprador = null;
        private string terceroComprador = string.Empty;
        private string portafolioID = string.Empty;
        private decimal factorRecompra = 0;
        private DateTime periodo;

        private int opcRecompra = 0;
        private int comboVal_Maduracion = 1;
        private int comboVal_Recompra = 2;
        private int comboVal_Individual = 3;
        private int comboVal_RecompraMasiva = 4;
        private int? libranza = null;
        private bool loadData = true;

        #endregion

        public RecompraCartera()
            : base()
        {
            //this.InitializeComponent();
        }

        public RecompraCartera(string mod)
            : base(mod)
        {
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.RecompraCartera;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            this.AddDocumentCols();

            //Carga el combo
            this.loadData = false;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(this.comboVal_Maduracion, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Maduracion"));
            dic.Add(this.comboVal_Recompra, this._bc.GetResource(LanguageTypes.Tables, "162_tbl_Recompra"));
            dic.Add(this.comboVal_Individual, this._bc.GetResource(LanguageTypes.Tables, "Individual"));
            dic.Add(this.comboVal_RecompraMasiva, this._bc.GetResource(LanguageTypes.Tables, "Recompra Masiva"));

            this.lkp_Recompra.Properties.DataSource = dic;
            this.lkp_Recompra.EditValue = this.comboVal_Maduracion;
            this.loadData = true;

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[0].Height = 40;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            //Sector cartera
            string sectorStr = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            this.sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorStr);

            //Estable las fechas con base a la fecha del periodo
            base.dtFecha.Enabled = false;
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);         

             //Carga la Informacion del Hedear
            _bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false);

            //Control 
            this.tipoTasaVenta = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_TasaVenta);

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
                if (!string.IsNullOrWhiteSpace(this.compradorID))
                {
                    if (this.opcRecompra == this.comboVal_Individual && string.IsNullOrEmpty(this.txtLibranza.Text))
                    {
                        MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Cc_InvalidLibranza), ""));
                        return;
                    }

                    this.listRecompras = new List<DTO_ccRecompraDeta>();
                    this.listMaduracion = new List<DTO_ccRecompraDeta>();
                    this.listToSave = new List<DTO_ccRecompraDeta>();
                    this.gcDocument.DataSource = null;

                    string msgError = string.Empty;
                    this.libranza = null;
                    this.libranza = !string.IsNullOrEmpty(this.txtLibranza.Text) ? Convert.ToInt32(this.txtLibranza.Text) : this.libranza;
                    List<int> libranzasFilter = new List<int>();
                    if (this.libranza != null)
                        libranzasFilter.Add(this.libranza.Value);
                    else if (this.opcRecompra == this.comboVal_RecompraMasiva)
                        libranzasFilter = this.currentLibranzas;

                    this.recompraCartera = this._bc.AdministrationModel.RecompraCartera_GetForCompraAndSustitucion(this.compradorID, libranzasFilter,ref msgError);
                    if (!string.IsNullOrEmpty(msgError))
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Error, msgError));
                    else if (this.recompraCartera.RecompraDeta.Count == 0)
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecompraCartera_NoCreditos));
                    else
                    {
                        this.listRecompras = (from c in this.recompraCartera.RecompraDeta where c.TipoEstado.Value.Value == (int)TipoEstadoCartera.Cedida select c).ToList();

                        if (this.comprador.MaduracionAntInd.Value.Value)
                            this.listMaduracion = (from c in this.recompraCartera.RecompraDeta where c.TipoEstado.Value.Value == (int)TipoEstadoCartera.Cedida select c).ToList();
                        this.loadData = true;
                    }

                    //Obliga a volver a cargar los datos de la grilla
                    //this.opcRecompra = 0;
                    //this.lkp_Recompra.EditValue = this.comboVal_Recompra;                   
                    this.lkp_Recompra_EditValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                //Aprobado
                GridColumn aprobado = new GridColumn();
                aprobado.FieldName = this.unboundPrefix + "Aprobado";
                aprobado.Caption = "√";
                aprobado.UnboundType = UnboundColumnType.Boolean;
                aprobado.VisibleIndex = 0;
                aprobado.Width = 50;
                aprobado.OptionsColumn.AllowEdit = true;
                aprobado.Visible = true;
                aprobado.ColumnEdit = editChkBox;
                aprobado.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprobado.AppearanceHeader.ForeColor = Color.Lime;
                aprobado.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprobado.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprobado.AppearanceHeader.Options.UseTextOptions = true;
                aprobado.AppearanceHeader.Options.UseFont = true;
                aprobado.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(aprobado);

                //Oferta
                GridColumn oferta = new GridColumn();
                oferta.FieldName = this.unboundPrefix + "Oferta";
                oferta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Oferta");
                oferta.UnboundType = UnboundColumnType.String;
                oferta.VisibleIndex = 1;
                oferta.Width = 80;
                oferta.Visible = true;
                oferta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                oferta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(oferta);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 2;
                libranza.Width = 80;
                libranza.Visible = true;
                libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Cliente ID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.Integer;
                clienteID.VisibleIndex = 3;
                clienteID.Width = 100;
                clienteID.Visible = true;
                clienteID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.Integer;
                nombCliente.VisibleIndex = 4;
                nombCliente.Width = 200;
                nombCliente.Visible = true;
                nombCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombCliente);

                //CompradorCarteraID
                GridColumn compradorCarteraID = new GridColumn();
                compradorCarteraID.FieldName = this.unboundPrefix + "CompradorCarteraID";
                compradorCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorCarteraID");
                compradorCarteraID.UnboundType = UnboundColumnType.Integer;
                compradorCarteraID.VisibleIndex = 5;
                compradorCarteraID.Width = 80;
                compradorCarteraID.Visible = true;
                compradorCarteraID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                compradorCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compradorCarteraID);

                //Cuotas Recompra
                GridColumn cuotaRecompra = new GridColumn();
                cuotaRecompra.FieldName = this.unboundPrefix + "CuotasRecompra";
                cuotaRecompra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotasRecompra");
                cuotaRecompra.UnboundType = UnboundColumnType.Integer;
                cuotaRecompra.VisibleIndex = 6;
                cuotaRecompra.Width = 40;
                cuotaRecompra.Visible = true;
                cuotaRecompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                cuotaRecompra.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cuotaRecompra);

                //Portafolio
                GridColumn portafolio = new GridColumn();
                portafolio.FieldName = this.unboundPrefix + "Portafolio";
                portafolio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Portafolio");
                portafolio.UnboundType = UnboundColumnType.String;
                portafolio.VisibleIndex = 7;
                portafolio.Width = 80;
                portafolio.Visible = true;
                portafolio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                portafolio.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(portafolio);

                //Factor Recompra
                GridColumn factorRecompra = new GridColumn();
                factorRecompra.FieldName = this.unboundPrefix + "FactorRecompra";
                factorRecompra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactorRecompra");
                factorRecompra.UnboundType = UnboundColumnType.Integer;
                factorRecompra.VisibleIndex = 8;
                factorRecompra.Width = 80;
                factorRecompra.Visible = true;
                factorRecompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                factorRecompra.OptionsColumn.AllowEdit = false;
                factorRecompra.ColumnEdit = editSpinPorc;
                this.gvDocument.Columns.Add(factorRecompra);

                //Vlr Libranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Integer;
                vlrLibranza.VisibleIndex = 9;
                vlrLibranza.Width = 200;
                vlrLibranza.Visible = true;
                vlrLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                vlrLibranza.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrLibranza);

                //Vlr Prepago
                GridColumn vlrPrepago = new GridColumn();
                vlrPrepago.FieldName = this.unboundPrefix + "VlrPrepago";
                vlrPrepago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrepago");
                vlrPrepago.UnboundType = UnboundColumnType.Integer;
                vlrPrepago.VisibleIndex = 10;
                vlrPrepago.Width = 200;
                vlrPrepago.Visible = true;
                vlrPrepago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                vlrPrepago.OptionsColumn.AllowEdit = false;
                vlrPrepago.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrPrepago);

                //Vlr Recompra
                GridColumn vlrRecompra = new GridColumn();
                vlrRecompra.FieldName = this.unboundPrefix + "VlrRecompra";
                vlrRecompra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrRecompra");
                vlrRecompra.UnboundType = UnboundColumnType.Integer;
                vlrRecompra.VisibleIndex = 11;
                vlrRecompra.Width = 200;
                vlrRecompra.Visible = true;
                vlrRecompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                vlrRecompra.OptionsColumn.AllowEdit = false;
                vlrRecompra.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrRecompra);

                //Vlr Utilidad
                GridColumn VlrUtilidad = new GridColumn();
                VlrUtilidad.FieldName = this.unboundPrefix + "VlrUtilidad";
                VlrUtilidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrUtilidad");
                VlrUtilidad.UnboundType = UnboundColumnType.Integer;
                VlrUtilidad.VisibleIndex = 12;
                VlrUtilidad.Width = 200;
                VlrUtilidad.Visible = true;
                VlrUtilidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                VlrUtilidad.OptionsColumn.AllowEdit = false;
                VlrUtilidad.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrUtilidad);

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.format = libranza.Caption;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "RecompraCartera.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void AfterInitialize()
        {
            if (DateTime.Now.Month != this.periodo.Month)
            {
                this.dtFechaCorte.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));

                this.dtFecha.Properties.MinValue = new DateTime(this.periodo.Year, this.periodo.Month, 1);
                this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            }
            else
            {
                this.dtFecha.DateTime = DateTime.Now;
                this.dtFechaCorte.DateTime = DateTime.Now;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.loadData = false;
            this.libranza = null;
            this.masterCompradorCartera.Value = String.Empty;
            this.compradorID = string.Empty;
            this.comprador = null;
            this.txtDocRecompra.Text = string.Empty;

            this.txtVlrTotalRecompra.EditValue = 0;
            this.txtVlrTotalSaldo.EditValue = 0;

            this.listMaduracion = new List<DTO_ccRecompraDeta>();
            this.listRecompras = new List<DTO_ccRecompraDeta>();
            this.listToSave = new List<DTO_ccRecompraDeta>();
            this.currentLibranzas = new List<int>();
            this.recompraCartera = new DTO_RecompraCartera();
            this.gcDocument.DataSource = this.recompraCartera.RecompraDeta;

            this.lkp_Recompra.EditValue = this.comboVal_Maduracion;
            FormProvider.Master.itemGenerateTemplate.Enabled = false;
            FormProvider.Master.itemImport.Enabled = false;

            this.txtLibranza.Text = string.Empty;
            this.txtLibranza.Visible = false;
            this.lblLibranza.Visible = false;

            this.loadData = true;
        }

        /// <summary>
        /// Valida la info de la importación
        /// </summary>
        private void ValidateImport(int libranza, List<int> currentLibranzas, ref DTO_ccRecompraDeta recompra, ref DTO_TxResultDetail rd, string msgInvalidLibranza, 
            string msgLibranzaAdded, string msgNoSaldo, string msgNoVendido, string msgSustituido, string msgRecompra, string msgInvalidComprador)
        {
            try
            {
                #region Valida que la libranza no exista en la lista
                if (currentLibranzas.Contains(libranza))
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = msgLibranzaAdded;
                    return;
                }
                #endregion
                #region Valida la libranza y el comprador

                //Trae el crédito
                DTO_ccCreditoDocu credito = _bc.AdministrationModel.GetCreditoByLibranza(libranza);
                if(credito == null)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = msgInvalidLibranza;
                    return;
                }

                //Verifica que se encuentre vendido
                if(credito.TipoEstado.Value != (byte)TipoEstadoCartera.Cedida)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Format(msgNoVendido, libranza);
                    return;
                }

                //Verifica el comprador
                if(credito.CompradorCarteraID.Value != this.compradorID)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = msgInvalidComprador;
                    return;
                }

                #endregion
                #region Valida que no este recomprado ni sustituido

                //Verifica que no se encuentre sustituido
                if (credito.SustituidoInd.Value.HasValue && credito.SustituidoInd.Value.Value)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Format(msgSustituido, libranza);
                    return;
                }

                //Verifica que no se encuentre recomprado
                DTO_ccVentaDeta ventaDeta = _bc.AdministrationModel.ccVentaDeta_GetByNumDocLibranza(credito.NumeroDoc.Value.Value);
                if (ventaDeta.NumDocRecompra.Value.HasValue)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = string.Format(msgRecompra, libranza);
                    return;
                }

                #endregion
                #region Valida el saldo

                List<DTO_ccCreditoPlanPagos> pp = _bc.AdministrationModel.GetPlanPagos(credito.NumeroDoc.Value.Value);
                decimal vlrCuota = pp.Sum(p => p.VlrCuota.Value.Value);
                decimal vlrPagadoCuota = pp.Sum(p => p.VlrPagadoCuota.Value.Value);
                decimal vlrSaldo = vlrCuota - vlrPagadoCuota;
                if (vlrCuota == vlrPagadoCuota)
                {
                    rd = new DTO_TxResultDetail();
                    rd.Message = msgNoSaldo;
                    return;
                }

                #endregion
                #region Llena el registro de la recompra

                // Carga la info de los flujos
                int flujosPagados = 0;
                decimal vlrSaldoFlujos = _bc.AdministrationModel.Credito_GetSaldoFlujos(credito.NumeroDoc.Value.Value, out flujosPagados);

                //Trae la info del cliente
                DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, credito.ClienteID.Value, true);

                //Tare la oferta
                DTO_ccVentaDocu ventaDocu = _bc.AdministrationModel.ccVentaDocu_GetByID(credito.NumeroDoc.Value.Value);

                //Carga la info de la recompra
                recompra = new DTO_ccRecompraDeta();
                recompra.Aprobado.Value = false;
                recompra.PrepagoInd.Value = false;
                recompra.NumDocCredito.Value = credito.NumeroDoc.Value;
                recompra.Oferta.Value = ventaDocu.Oferta.Value;
                recompra.Libranza.Value = libranza;
                recompra.ClienteID.Value = credito.ClienteID.Value;
                recompra.Nombre.Value = cliente.Descriptivo.Value;
                recompra.CompradorCarteraID.Value = credito.CompradorCarteraID.Value;
                recompra.CuotasRecompra.Value = ventaDeta.CuotasVend.Value - flujosPagados;
                recompra.TipoEstado.Value = credito.TipoEstado.Value;
                recompra.CuotaID.Value = ventaDeta.CuotaID.Value;
                recompra.VlrCuota.Value = ventaDeta.VlrCuota.Value;
                recompra.VlrLibranza.Value = vlrSaldoFlujos;
                recompra.VlrPrepago.Value = credito.VlrPrepago.Value;
                recompra.FlujosPagados.Value = flujosPagados;
                recompra.VlrDerechos.Value = 0;
                #endregion
            }
            catch (Exception ex)
            {
                rd = new DTO_TxResultDetail();
                rd.Message = ex.Message;
            }
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            if (String.IsNullOrWhiteSpace(this.txtDocRecompra.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblDocRecompra.Text);
                MessageBox.Show(msg);
                return false;
            }

            if (!this.listToSave.Any(x => x.Aprobado.Value == true))
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecompraCartera_NoSelected);
                MessageBox.Show(msg);
                return false;
            }

            if (this.listToSave.Any(x => x.Aprobado.Value.HasValue && x.Aprobado.Value.Value && x.VlrRecompra.Value == null))// && x.VlrRecompra.Value > 0))
            {
                string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecompraCartera_VlrRecompra);
                MessageBox.Show(msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calcula el valor de recompra
        /// </summary>
        /// <param name="seleccion">Permite saber si es seleccionado en la grilla</param>
        private void CalcularRecompra(bool seleccion, DTO_ccRecompraDeta row)
        {
            if (seleccion)
            {
                row = row ?? (DTO_ccRecompraDeta)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                
                row.Aprobado.Value = seleccion;
                if (row.VlrCapital.Value != row.VlrCapitalContab.Value)
                {
                    gvDocument.SetColumnError(this.gvDocument.Columns[this.unboundPrefix + "Aprobado"], "El valor del capital no coincide con el registrado en los saldos contables que corresponde a " + row.VlrCapitalContab.Value.Value.ToString("c0"));
                    row.Aprobado.Value = false;
                    return;
                }
                else if (row.VlrInteres.Value != row.VlrInteresContab.Value)
                {
                    gvDocument.SetColumnError(this.gvDocument.Columns[this.unboundPrefix+"Aprobado"], "El valor del interés no coincide con el registrado en los saldos contables que corresponde a " + row.VlrInteresContab.Value.Value.ToString("c0"));
                    row.Aprobado.Value = false;
                    return;
                }

                // Asigna la info del comprador
                row.FactorRecompra.Value = this.opcRecompra == this.comboVal_Maduracion ? 0 : this.factorRecompra;
                row.Portafolio.Value = this.portafolioID;

                //Asigna los valores de la recompra
                decimal vlrLibranzaRecompra = row.VlrLibranza.Value.Value;
                decimal vlrNeto = row.VlrNeto.Value == null ? 0 : row.VlrNeto.Value.Value;

                decimal factorReal = this.opcRecompra == this.comboVal_Maduracion ? 0 : this.factorRecompra;

                double b = Convert.ToDouble(1 + (factorReal / 100));
                double exp = Convert.ToDouble(row.CuotasRecompra.Value.Value);
                decimal vlrRecompraFinal = Math.Round(vlrLibranzaRecompra / Convert.ToDecimal(Math.Pow(b, exp)));

                row.VlrRecompra.Value = 0;
                if (this.opcRecompra == this.comboVal_Recompra)
                {
                    row.VlrRecompra.Value = vlrRecompraFinal;
                    row.VlrUtilidad.Value = Math.Round(row.VlrLibranza.Value.Value - vlrRecompraFinal);
                }
                else if (this.opcRecompra == this.comboVal_Maduracion)
                {
                    row.VlrRecompra.Value = row.VlrSaldosPagos.Value.Value;
                    row.VlrUtilidad.Value = 0;
                }
                else if (this.opcRecompra == this.comboVal_Individual || this.opcRecompra == this.comboVal_RecompraMasiva)
                {
                    if (this.comprador.TipoControlRecursos.Value == (byte)TipoControlRecursos.RecursosDisponibles)
                    {
                        row.VlrRecompra.Value = row.VlrCapitalCesion.Value;
                        row.VlrUtilidad.Value = row.VlrCapital.Value - row.VlrCapitalCesion.Value;
                    }
                    else
                    {
                        if (this.comprador.ResponsabilidadInd.Value.Value)
                        {
                            row.VlrRecompra.Value = vlrRecompraFinal;
                            row.VlrUtilidad.Value = Math.Round(row.VlrLibranza.Value.Value - vlrRecompraFinal);
                        }
                        else
                        {
                            row.VlrRecompra.Value = row.VlrCapitalCesion.Value;
                            row.VlrUtilidad.Value = row.VlrCapital.Value - row.VlrCapitalCesion.Value;
                        }
                    }
                }
                row.VlrNeto.Value = vlrNeto + row.VlrUtilidad.Value.Value;

                this.gvDocument.RefreshRow(this.gvDocument.FocusedRowHandle);
                // Recalcula los valores
                this.txtVlrTotalSaldo.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrLibranza.Value);
                this.txtVlrTotalRecompra.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrRecompra.Value);
            }
            else
            {
                if (this.opcRecompra != this.comboVal_Individual && this.opcRecompra != this.comboVal_RecompraMasiva)
                {
                    // Recalcula los valores
                    row = row ?? (DTO_ccRecompraDeta)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                    row.Aprobado.Value = seleccion;
                    this.txtVlrTotalSaldo.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrLibranza.Value);
                    this.txtVlrTotalRecompra.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrRecompra.Value);

                    row.FactorRecompra.Value = null;
                    row.Portafolio.Value = null;
                    row.VlrRecompra.Value = null;
                    row.VlrUtilidad.Value = null;
                }
                else
                {
                    row.VlrRecompra.Value = 0;
                    this.txtVlrTotalSaldo.EditValue = 0;
                    this.txtVlrTotalRecompra.EditValue = 0;
                }
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
            base.Form_Enter(sender, e);

            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = true;
            FormProvider.Master.itemSearch.Visible = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemSearch.Enabled = true;

                //Se calcula según el combo
                FormProvider.Master.itemGenerateTemplate.Enabled = false;
                FormProvider.Master.itemImport.Enabled = false;
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta cuando se modifica el combo de tipo recompra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_Recompra_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.loadData)
                {
                    this.opcRecompra = Convert.ToInt32(this.lkp_Recompra.EditValue);
                    if (this.opcRecompra == this.comboVal_Maduracion)
                    {
                        if (this.comprador != null && !this.comprador.MaduracionAntInd.Value.Value)
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_RecompraCartera_CompradorSinMadAnt));

                        this.listToSave = this.listMaduracion;
                        FormProvider.Master.itemGenerateTemplate.Enabled = false;
                        FormProvider.Master.itemImport.Enabled = false;
                        this.txtLibranza.Visible = false;
                        this.lblLibranza.Visible = false;
                        this.gbSaldoCesion.Visible = false;
                        this.gbSaldoCredito.Visible = false;
                        this.txtLibranza.Text = string.Empty;
                    }
                    else if (this.opcRecompra == this.comboVal_Recompra)
                    {
                        this.listToSave = this.listRecompras;
                        FormProvider.Master.itemGenerateTemplate.Enabled = true;
                        FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                        this.txtLibranza.Visible = false;
                        this.lblLibranza.Visible = false;
                        this.gbSaldoCesion.Visible = false;
                        this.gbSaldoCredito.Visible = false;
                        this.txtLibranza.Text = string.Empty;
                    }
                    else if (this.opcRecompra == this.comboVal_RecompraMasiva)
                    {
                        this.listToSave = this.listRecompras;
                        FormProvider.Master.itemGenerateTemplate.Enabled = true;
                        FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                        this.txtLibranza.Visible = false;
                        this.lblLibranza.Visible = false;
                        this.gbSaldoCesion.Visible = false;
                        this.gbSaldoCredito.Visible = false;
                        this.txtLibranza.Text = string.Empty;
                    }
                    else
                    {
                        this.listToSave = this.listRecompras;
                        FormProvider.Master.itemGenerateTemplate.Enabled = false;
                        FormProvider.Master.itemImport.Enabled = false;
                        this.txtLibranza.Visible = true;
                        this.lblLibranza.Visible = true;                    
                        this.gbSaldoCesion.Visible = true;
                        this.gbSaldoCredito.Visible = true;
                        if (this.listToSave.Count  > 0)
                        {
                            this.txtVlrCapital.EditValue = this.listToSave.First().VlrCapital.Value;
                            this.txtVlrInteres.EditValue = this.listToSave.First().VlrInteres.Value;
                            this.txtVlrCesion.EditValue = this.listToSave.First().VlrCapitalCesion.Value;
                            this.txtVlrUtilidad.EditValue = this.listToSave.First().VlrUtilidadCesion.Value;
                            this.txtVlrDerechos.EditValue = this.listToSave.First().VlrDerechosCesion.Value;                            
                        }
                    }

                    this.gcDocument.DataSource = this.listToSave;
                    this.gcDocument.RefreshDataSource();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "lkp_Recompra_EditValueChanged"));
            }
        }

        /// <summary>
        /// Funcion que trae los creditos para compra o sustitucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCompradorCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.loadData && this.compradorID != this.masterCompradorCartera.Value)
                {
                    if (this.masterCompradorCartera.ValidID && !String.IsNullOrWhiteSpace(this.masterCompradorCartera.Value))
                    {
                        // Limpia los filtros
                        this.compradorID = this.masterCompradorCartera.Value;
                        this.comprador = (DTO_ccCompradorCartera)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCompradorCartera, false, this.compradorID, true);

                        // Carga los datos
                        //this.LoadData(false);

                        this.terceroComprador = this.comprador.TerceroID.Value;
                        this.factorRecompra = this.comprador.FactorRecompra.Value == null ? 1 : (int)this.comprador.FactorRecompra.Value.Value;

                        // Revisa si es Anual o mensual
                        if (this.tipoTasaVenta == ((byte)TasaVenta.EfectivaAnual).ToString())
                        {
                            double te1 = Convert.ToDouble(this.factorRecompra / 100);
                            this.factorRecompra = Convert.ToDecimal(Math.Pow(te1 + 1, 1d / 12d)) - 1;
                            this.factorRecompra *= 100;
                        }

                        // Carga la información del portafolio
                        if (this.comprador.PortafolioInd.Value.Value)
                        {
                            Dictionary<string, string> dicCompPortafolio = new Dictionary<string, string>();
                            dicCompPortafolio.Add("CompradorCarteraID", this.compradorID);
                            DTO_ccCompradorPortafilio compPortafolio = (DTO_ccCompradorPortafilio)_bc.GetMasterComplexDTO(AppMasters.ccCompradorPortafilio, dicCompPortafolio, true);
                            if (compPortafolio == null)
                            {
                                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNoPortafolio), this.compradorID);
                                MessageBox.Show(msg);
                                return;
                            }
                            this.portafolioID = compPortafolio.Portafolio.Value;
                        }
                        else
                            this.portafolioID = string.Empty;
                    }
                    else
                    {
                        this.listMaduracion = new List<DTO_ccRecompraDeta>();
                        this.listRecompras = new List<DTO_ccRecompraDeta>();
                        this.listToSave = new List<DTO_ccRecompraDeta>();

                        this.gcDocument.DataSource = this.listToSave;
                    }
                }
                if (!this.masterCompradorCartera.ValidID)
                {
                    this.comprador = null;
                    this.compradorID = string.Empty;
                    this.terceroComprador = string.Empty;
                    this.factorRecompra = 0;
                }

                if (this.txtLibranza.Visible)
                    this.txtLibranza.Focus();
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, KeyPressEventArgs e)
        {
            //this.LoadData(false);
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            try
            {
                #region Generales

                if (fieldName == "Aprobado")
                {
                    this.CalcularRecompra((bool)e.Value,this._rowCurrent);
                }

                #endregion

                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "gvDocument_CellValueChanging"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                this._rowCurrent = (DTO_ccRecompraDeta)this.gvDocument.GetRow(e.FocusedRowHandle);
            }   
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.opcRecompra == this.comboVal_Recompra && this.gvDocument.FocusedRowHandle >= 0)
                {
                    this.gvDocument.PostEditor();
                    if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                    {
                        string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                        string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                        //Revisa si desea cargar los temporales
                        if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int fila = this.gvDocument.FocusedRowHandle;
                          
                            // Recalcula los valores
                            if (this._rowCurrent.Aprobado.Value.Value)
                            {
                                this.txtVlrTotalSaldo.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrLibranza.Value);
                                this.txtVlrTotalRecompra.EditValue = this.listToSave.FindAll(x => x.Aprobado.Value.Value).Sum(x => x.VlrRecompra.Value);
                            }

                            this.listToSave.RemoveAt(fila);
                            this.gvDocument.RefreshData();
                        }
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAuxiliarForm.cs-", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e) 
        {
            if (this.gvDocument.HasColumnErrors)
                e.Allow = false;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err (TBNew): " + ex.Message);
            }
        }

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.txtVlrTotalSaldo.EditValue = 0;
                this.txtVlrTotalRecompra.EditValue = 0;
                this.LoadData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err (TBUpdate): " + ex.Message);
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDocument.PostEditor();
                bool isValid = this.ValidateDoc();

                if (isValid && !this.gvDocument.HasColumnErrors)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err (TBSave): " + ex.Message);
            }
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

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar los datos de la plantilla
        /// </summary>
        public override void TBImport()
        {
            try
            {
                if(string.IsNullOrWhiteSpace(this.masterCompradorCartera.Value) || !this.masterCompradorCartera.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCompradorCartera.LabelRsx);
                    MessageBox.Show(msg);
                    return;
                }

                bool importData = true;
                if (this.listToSave.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) != DialogResult.Yes)
                        importData = false;
                }

                //Importa los datos
                if(importData)
                {
                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);

                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TBGenerateTemplate.cs", "TBImport"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                #region Carga la informacion del ccRecompraDocu
                this.recompraCartera.RecompraDocu.FechaCorte.Value = this.dtFechaCorte.DateTime;
                this.recompraCartera.RecompraDocu.CompradorCarteraID.Value = this.compradorID;
                this.recompraCartera.RecompraDocu.DocRecompra.Value = this.txtDocRecompra.Text;
                this.recompraCartera.RecompraDocu.FactorRecompra.Value = this.opcRecompra == this.comboVal_Maduracion ? 0 : this.factorRecompra;
                this.recompraCartera.RecompraDocu.TerceroID.Value = this.terceroComprador;
                this.recompraCartera.RecompraDocu.ValorRecompra.Value = Convert.ToDecimal(this.txtVlrTotalRecompra.EditValue, CultureInfo.InvariantCulture);
                #endregion
                #region Actualiza la lista incluyendo solo los que estan seleccionados para recomprar    
                this.recompraCartera.RecompraDeta = this.listToSave.Where(x => x.Aprobado.Value.Value).ToList();    
                #endregion
                #region Guarda la info
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                bool isMaduracionAnt = this.opcRecompra == this.comboVal_Maduracion;
                results = _bc.AdministrationModel.RecompraCartera_Add(this.documentID, this._actFlujo.ID.Value,this.opcRecompra == this.comboVal_Individual || 
                                                                      this.opcRecompra == this.comboVal_RecompraMasiva? true: false, isMaduracionAnt, this.recompraCartera);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Procesa los resultados 
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                int i = 0;
                int percent = 0;
                #region Variables para el mail

                DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                string body = string.Empty;
                string subject = string.Empty;
                string email = user.CorreoElectronico.Value;

                string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Approved_Subject);
                string subjectRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_Rejected_Subject);
                string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_ApprovedCartera_Body);
                string bodyRech = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_RejectedCartera_Body);
                string formName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                #endregion
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object result in results)
                {
                    #region Manejo de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion
                    #region Envia el correo
                    DTO_ccRecompraDeta recompraDeta = this.recompraCartera.RecompraDeta[i];
                    if (recompraDeta.Aprobado.Value.Value)
                    {
                        if (result.GetType() == typeof(DTO_TxResult))
                        {
                            resultsNOK.Add((DTO_TxResult)result);
                            this.isValid = false;
                        }
                        else
                        {
                            #region Envia el correo
                            if (recompraDeta.Aprobado.Value.Value)
                            {
                                subject = string.Format(subjectApr, formName);
                                body = string.Format(bodyApr, formName, recompraDeta.NumDocCredito.Value, recompraDeta.ClienteID.Value, string.Empty);
                            }
                            _bc.SendMail(this.documentID, subject, body, email);
                            #endregion
                        }
                    }
                    #endregion
                    i++;
                }
                #endregion
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                if (this.isValid)
                    this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "SaveThread"));
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
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);
                    DictionaryProgress.IniciarProceso(this._bc.AdministrationModel.User.ReplicaID.Value.Value, documentID);

                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error

                    DTO_TxResult result = new DTO_TxResult();
                    result.Details = new List<DTO_TxResultDetail>();
                    this.currentLibranzas = this.listRecompras.Select(l => l.Libranza.Value.Value).ToList();
                    List<DTO_ccRecompraDeta> recomprasTemp = ObjectCopier.Clone(this.listRecompras);

                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgInvalidLibranza = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                    string msgLibranzaAdded = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAdded);
                    string msgNoSaldo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoSinSaldos);
                    string msgNoVendido = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_NoVendido);
                    string msgSustituido = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_Sustituido);
                    string msgRecompra = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_Recompra);
                    string msgInvalidComprador = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_InvalidCompradorCartera);

                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas

                    List<int> libranzas = new List<int>();
                    List<string> colNames = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    string colRsx = colNames[0];

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
                            rd.line = i + 1;
                            rd.Message = "OK";

                            bool validLibranza = true;
                            string libranza = string.Empty;

                            #region Info básica

                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length > 1)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                validLibranza = false;
                                continue;
                            }
                            else
                            {
                                libranza = line[0];
                            }

                            #endregion
                            #region Validacion de Nulls
                            if (string.IsNullOrEmpty(libranza))
                            {
                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                rdF.Field = colRsx;
                                rdF.Message = msgEmptyField;
                                rd.DetailsFields.Add(rdF);

                                validLibranza = false;
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
                                    rdF.Field = colRsx;
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
                                libranzas.Add(Convert.ToInt32(libranza));
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida que existan las libranza

                    if (validList)
                    {
                        int i = 0;
                        foreach (int lib in libranzas)
                        {
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, ((i + 1) * 100) / libranzas.Count });
                            ++i;
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer

                            if (lines.Length == 1)
                            {
                                result.ResultMessage = msgNoCopyField;
                                result.Result = ResultValue.NOK;
                                validList = false;
                            }

                            #endregion

                            DTO_ccRecompraDeta recompra = null;
                            DTO_TxResultDetail rd = null;
                            this.ValidateImport(lib, this.currentLibranzas, ref recompra, ref rd, msgInvalidLibranza, msgLibranzaAdded, msgNoSaldo, msgNoVendido,
                                msgSustituido, msgRecompra, msgInvalidComprador);
                            
                            if (rd == null)
                            {
                                this.currentLibranzas.Add(lib);
                                recomprasTemp.Add(recompra);
                            }
                            else
                            {
                                rd.line = i + 1;
                                result.Result = ResultValue.NOK;
                                result.Details.Add(rd);
                                validList = false;
                            }
                        }
                    }

                    #endregion

                    if (validList)
                    {
                        //DictionaryProgress.BatchProgress[tupProgress] = 100;
                        this.listRecompras = ObjectCopier.Clone(recomprasTemp);
                        this.listToSave = this.listRecompras;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RecompraCartera.cs", "ImportThread"));
            }
            finally
            {
                if (!this.pasteRet.Success)
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });                 
                }
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
               
    }
}
