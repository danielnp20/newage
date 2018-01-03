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
using System.Diagnostics;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class VentaCartera : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
            this.EnableControls(false);
            this.masterCompradorCartera.Focus();
        }

        #endregion

        public VentaCartera()
            : base()
        {
            //InitializeComponent();
        }

        public VentaCartera(string mod)
            : base(mod)
        {
        }

        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        private List<DTO_ccCreditoDocu> creditos = new List<DTO_ccCreditoDocu>();
        private DTO_VentaCartera venta = new DTO_VentaCartera();

        private string compradorCarteraID = string.Empty;
        private string oferta = string.Empty;
        private DateTime periodo;
        private string sectorCartera = string.Empty;

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.VentaCartera;
            this.frmModule = ModulesPrefix.cc;

            InitializeComponent();
            base.SetInitParameters();

            this.AddGridCols();

            //Modifica el tamaño de las Grillas
            this.tlSeparatorPanel.RowStyles[0].Height = 30;

            this.gvDocument.OptionsBehavior.AutoPopulateColumns = true;
            this.grpboxDetail.Dock = DockStyle.Fill;

            //Carga la maestra de comprador de cartera
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
            this._bc.InitMasterUC(this.masterCompradorCartera, AppMasters.ccCompradorCartera, true, true, true, false, filtros);

            //Tra el sector de cartera
            this.sectorCartera = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
            if (Convert.ToByte(sectorCartera) == (byte)SectorCartera.Financiero)
                this.gvDocument.Columns[this.unboundPrefix + "PortafolioID"].Visible = false;

            //Estable las fechas con base a la fecha del periodo
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);

            this.dtFecha.Enabled = false;
            this.EnableControls(false);
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 0;
                libranza.Width = 70;
                libranza.Visible = true;
                libranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Cliente ID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.Integer;
                clienteID.VisibleIndex = 1;
                clienteID.Width = 70;
                clienteID.Visible = true;
                clienteID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.Integer;
                nombCliente.VisibleIndex = 2;
                nombCliente.Width = 200;
                nombCliente.Visible = true;
                nombCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombCliente);

                //Portafolio
                GridColumn portafolio = new GridColumn();
                portafolio.FieldName = this.unboundPrefix + "PortafolioID";
                portafolio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Portafolio");
                portafolio.UnboundType = UnboundColumnType.String;
                portafolio.VisibleIndex = 3;
                portafolio.Width = 100;
                portafolio.Visible = true;
                portafolio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                portafolio.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(portafolio);

                //PrimeraCuota
                GridColumn primeraCuota = new GridColumn();
                primeraCuota.FieldName = this.unboundPrefix + "PrimeraCuota";
                primeraCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrimeraCuota");
                primeraCuota.UnboundType = UnboundColumnType.Integer;
                primeraCuota.VisibleIndex = 4;
                primeraCuota.Width = 50;
                primeraCuota.Visible = true;
                primeraCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                primeraCuota.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(primeraCuota);

                //Vlr Cuota
                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Integer;
                vlrCuota.VisibleIndex = 5;
                vlrCuota.Width = 100;
                vlrCuota.Visible = true;
                vlrCuota.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrCuota.AppearanceCell.Options.UseTextOptions = true;
                vlrCuota.OptionsColumn.AllowEdit = false;
                vlrCuota.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrCuota);

                //Vlr VlrPrestamo
                GridColumn VlrPrestamo = new GridColumn();
                VlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                VlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                VlrPrestamo.UnboundType = UnboundColumnType.Integer;
                VlrPrestamo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrPrestamo.AppearanceCell.Options.UseTextOptions = true;
                VlrPrestamo.VisibleIndex = 6;
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
                vlrNominal.VisibleIndex = 7;
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
                VlrVenta.VisibleIndex = 8;
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
                VlrUtilidad.VisibleIndex = 9;
                VlrUtilidad.Width = 100;
                VlrUtilidad.Visible = true;
                VlrUtilidad.OptionsColumn.AllowEdit = false;
                VlrUtilidad.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrUtilidad);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "VentaCartrea.cs-AddDocumentCols"));
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
            }
            else
                this.dtFecha.DateTime = DateTime.Now;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// 
        /// </summary>
        private void CalcularValores()
        {
            if (this.creditos != null && this.creditos.Count > 0)
            {
                this.txtVlrTotalVenta.EditValue = this.creditos.Sum(x => x.VlrVenta.Value.Value);
                this.txtVlrTotalNominal.EditValue = this.creditos.Sum(x => x.VlrLibranza.Value.Value);
                this.txtVlrTotalNeto.EditValue = this.creditos.Sum(x => x.VlrUtilidad.Value.Value);
            }
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this.masterCompradorCartera.Value = String.Empty;
            this.txtCartaAceptacion.Text = String.Empty;
            this.txtTasaMensual.Text = String.Empty;
            this.txtObservacion.Text = String.Empty;
            this.txtTasaMensual.EditValue = 0;
            this.txtTasaAnual.EditValue = 0;
            this.txtVlrTotalVenta.EditValue = 0;
            this.txtVlrTotalNominal.EditValue = 0;
            this.txtVlrTotalNeto.EditValue = 0;

            this.lkp_Oferta.EditValueChanged -= new System.EventHandler(this.lkp_Oferta_EditValueChanged);
            this.lkp_Oferta.EditValue = String.Empty;
            this.lkp_Oferta.Properties.DataSource = new Dictionary<string, string>();
            this.lkp_Oferta.EditValueChanged += new System.EventHandler(this.lkp_Oferta_EditValueChanged);

            //Variables
            this.compradorCarteraID = string.Empty;
            this.terceroID = string.Empty;
            this.oferta = string.Empty;

            this.creditos = null;
            this.gcDocument.DataSource = this.creditos;
        }

        /// <summary>
        /// Valida la info del cabezote
        /// </summary>
        private bool ValidateDoc()
        {
            if (String.IsNullOrWhiteSpace(this.masterCompradorCartera.Value))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCompradorCartera.LabelRsx);
                MessageBox.Show(msg);
                this.masterCompradorCartera.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.oferta))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblOferta.Text);
                MessageBox.Show(msg);
                this.lkp_Oferta.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(this.txtVlrTotalVenta.Text))
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), this.txtVlrTotalVenta.Text);
                MessageBox.Show(msg);
                this.masterCompradorCartera.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Funcion que habilita o deshabilita los controles de la pantalla
        /// </summary>
        /// <param name="enable"></param>
        private void EnableControls(bool enable)
        {
            //Header
            this.lkp_Oferta.Enabled = enable;
            this.txtCartaAceptacion.Enabled = enable;
            this.dtFecha.Enabled = enable;
            this.txtObservacion.Enabled = enable;
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
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemGenerateTemplate.Visible = false;
             
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Funcion que trae los creditos para compra o sustitucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCompradorCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.compradorCarteraID != this.masterCompradorCartera.Value)
                {
                    this.compradorCarteraID = this.masterCompradorCartera.Value;
                    if (this.masterCompradorCartera.ValidID)
                    {
                        #region Valida que el comprador de cartera sea diferente al del control de cartera

                        string compCarteraControl = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CodigoCarteraPropia);
                        if (String.Equals(this.compradorCarteraID, compCarteraControl))
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNotValid), this.compradorCarteraID);
                            MessageBox.Show(msg);
                            this.lkp_Oferta.Enabled = false;
                            return;
                        }

                        #endregion
                        #region Carga la informacion del comprador de cartera
                        DTO_ccCompradorCartera compCartera = (DTO_ccCompradorCartera)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCompradorCartera, false, this.compradorCarteraID, true);
                        if (String.IsNullOrWhiteSpace(compCartera.FactorRecompra.Value.ToString()))
                        {
                            string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CompradorCarteraNoFactor), this.compradorCarteraID);
                            MessageBox.Show(msg);
                            return;
                        }

                        #endregion
                        #region Carga las ofertas

                        this.lkp_Oferta.EditValueChanged -= new System.EventHandler(this.lkp_Oferta_EditValueChanged);

                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        List<string> ofertas = _bc.AdministrationModel.PreventaCartera_GetOfertas(this.compradorCarteraID, true);
                        ofertas.ForEach(o => dict.Add(o.Trim(), o.Trim()));
                        this.lkp_Oferta.Properties.DataSource = dict;
                        this.lkp_Oferta.EditValue = string.Empty;
                        this.lkp_Oferta.EditValueChanged += new System.EventHandler(this.lkp_Oferta_EditValueChanged);


                        #endregion

                        this.EnableControls(true);
                        this.lkp_Oferta.Focus();
                    }
                    else
                    {
                        this.EnableControls(false);
                        this.CleanData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "masterCompradorCartera_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta cuando se sale del contro de la oferta
        /// </summary>
        private void lkp_Oferta_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.oferta != this.lkp_Oferta.EditValue.ToString() && !String.IsNullOrWhiteSpace(this.compradorCarteraID))
                {
                    this.oferta = this.lkp_Oferta.EditValue.ToString();
                    this.venta = this._bc.AdministrationModel.VentaCartera_GetForVenta(this._actFlujo.ID.Value, this.masterCompradorCartera.Value, this.oferta);
                   
                    #region Carga la informacion de los creditos para vender

                    this.creditos = this.venta.Creditos;
                    if (this.creditos != null  && this.creditos.Count > 0)
                    {
                        this.dtFecha.DateTime = this.venta.VentaDocu.FechaVenta.Value.Value; 
                        this.dtFechaLiquida.DateTime = this.venta.VentaDocu.FechaLiquida.Value.Value;
                        this.dtFechaFlujo.DateTime = this.venta.VentaDocu.FechaPago1.Value.Value;

                        this.txtTasaMensual.EditValue = this.venta.VentaDocu.FactorCesion.Value;
                        this.txtTasaAnual.EditValue = this.venta.VentaDocu.TasaDescuento.Value;

                        #region Calcular el VlrPrestamo con las cuotas no pagas
                        foreach (var cred in this.creditos)
                        {
                            List<DTO_ccCreditoPlanPagos> pp = _bc.AdministrationModel.GetPlanPagos(cred.NumeroDoc.Value.Value);
                            pp = pp.Where(x => x.VlrPagadoCuota.Value == 0).ToList();
                            cred.VlrPrestamo.Value = pp.Sum(x => x.VlrCapital.Value);
                        } 
                        #endregion

                        this.creditos = this.creditos.OrderByDescending(c => c.VlrLibranza.Value).ToList();
                        this.gcDocument.DataSource = this.creditos;
                        this.CalcularValores();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_VentaCartera_CompCarteraNoVenta);
                        MessageBox.Show(string.Format(msg, this.compradorCarteraID));
                       
                        this.gcDocument.DataSource = null;
                        this.EnableControls(false);
                        this.lkp_Oferta.Enabled = true;
                        this.lkp_Oferta.Focus();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-VentaCartera.cs", "lkp_Oferta_EditValueChanged"));
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
                this.EnableControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Err (TBNew): " + ex.Message);
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocument.PostEditor();
            try
            {
                if (this.creditos.Count > 0 && this.ValidateDoc())
                {
                    this.venta.VentaDocu.FechaAceptacion.Value = this.dtFecha.DateTime.Date;
                    this.venta.VentaDocu.RefCartaAceptacion.Value = this.txtCartaAceptacion.Text;
                    this.venta.VentaDocu.Observacion.Value = this.txtObservacion.Text;

                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-VentaCartera.cs", "TBSave"));
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

                Tuple<int, List<DTO_SerializedObject>, string> tuple;
                tuple = _bc.AdministrationModel.VentaCartera_Add(this.documentID, this._actFlujo.ID.Value, this.venta);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion
                #region Carga los resultados

                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });

                List<DTO_SerializedObject> results = tuple.Item2;
                string fileName = string.Empty;
                this.isValid = true;
                MessageForm frm = null;
                bool checkResults = true;
                if (results.Count > 1)
                {
                    foreach (DTO_SerializedObject r in results)
                    {
                        if (r.GetType() == typeof(DTO_TxResult))
                        {
                            checkResults = false;
                            frm = new MessageForm((DTO_TxResult)r);
                            this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                            this.isValid = false;
                            break;
                        }
                    }
                  
                }
                #endregion
                if (!string.IsNullOrEmpty(sectorCartera) && Convert.ToByte(sectorCartera) != (byte)SectorCartera.Financiero)
                {
                    #region Envia los correos
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

                            bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.VentaCartera, this._actFlujo.seUsuarioID.Value, obj, false);
                            if (!isOK)
                            {
                                DTO_TxResult r = (DTO_TxResult)obj;
                                resultsNOK.Add(r);
                                this.isValid = false;
                            }
                            if (i == 0)
                            {
                                DTO_Alarma alarma = (DTO_Alarma)obj;
                                fileName = alarma.FileName;
                            }

                            i++;
                        }

                        frm = new MessageForm(resultsNOK);
                    }
                    #endregion
                    #region Genera los reportes
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    if (this.isValid)
                    {
                        #region Genera Reportes
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, tuple.Item3);
                        Process.Start(fileURl);
                        #endregion
                        #region Genera el Archivo csv
                        string fileURlArchivo = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, fileName);
                        Process.Start(fileURlArchivo);
                        #endregion
                        foreach (var r in results)
                        {
                            if (r.GetType() == typeof(DTO_Alarma))
                            {
                                DTO_Alarma tmp = (DTO_Alarma)r;
                                if (tmp.DocumentoID == AppDocuments.FacturaVenta.ToString())
                                {
                                    string reportFact = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(AppDocuments.FacturaVenta,tmp.NumeroDoc, false, ExportFormatType.pdf,0,0,0);
                                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(tmp.NumeroDoc), null, reportFact.ToString());
                                    Process.Start(fileURl);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if(this.isValid)
                {
                    DTO_TxResult resultOK = new DTO_TxResult();
                    resultOK.Result = ResultValue.OK;
                    MessageForm frmOK = new MessageForm(resultOK);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frmOK });
                    this.Invoke(this.saveDelegate);
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-VentaCartera.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
        
    }
}
