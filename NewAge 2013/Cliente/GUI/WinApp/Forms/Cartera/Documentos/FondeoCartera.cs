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
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class FondeoCartera : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            if (this.loadData)
            {
                this.LoadDetail();
                this.loadData = false;
            }
            else
                this.CleanData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_MigrarVentaCartera> migracionVentaCartera = new List<DTO_MigrarVentaCartera>();
        private DTO_ccCompraDocu compraDocu = new DTO_ccCompraDocu();

        //Variables para importar
        bool _isOK = true;
        DTO_TxResult result;
        List<DTO_TxResult> results;
        private string _compradorRsx = string.Empty;
        private string _ciudadNacimientoRsx = string.Empty;
        private string _zonaIDRsx = string.Empty;
        private string _ciudadRsx = string.Empty;

        //Variables privadas
        private string vendedorCartera = string.Empty;
        private bool loadData;
        private DateTime periodo;

        #endregion

        public FondeoCartera()
            : base()
        {
            //this.InitializeComponent();
        }

        public FondeoCartera(string mod)
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

                this.documentID = AppDocuments.FondeoCartera;
                this.frmModule = ModulesPrefix.cc;

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterVendedorCartera, AppMasters.ccVendedorCartera, true, true, true, false);

                //Carga la grilla con las columnas
                this.AddGridCols();

                //Modifica los paneles
                this.tlSeparatorPanel.RowStyles[0].Height = 150;
                this.tlSeparatorPanel.RowStyles[1].Height = 400;

                //Carga recursos importacion
                this._compradorRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorCartera");
                this._ciudadNacimientoRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_NacimientoCiudad");
                this._zonaIDRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ZonaID");
                this._ciudadRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Ciudad");

                this.dtFecha.Enabled = false;
                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.dtFechaFlujo1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);

                this.dtFechaFondeo.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);

                this.EnableControls(false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla superior
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
                libranza.VisibleIndex = 1;
                libranza.Width = 50;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //Cliente Id
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.VisibleIndex = 2;
                clienteID.Width = 60;
                clienteID.Visible = true;
                clienteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(clienteID);

                //Nombre Cliente
                GridColumn nombCliente = new GridColumn();
                nombCliente.FieldName = this.unboundPrefix + "Nombre";
                nombCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");
                nombCliente.UnboundType = UnboundColumnType.String;
                nombCliente.VisibleIndex = 3;
                nombCliente.Width = 150;
                nombCliente.Visible = true;
                nombCliente.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombCliente);

                //Comprador Final
                GridColumn compradorFinal = new GridColumn();
                compradorFinal.FieldName = this.unboundPrefix + "CompradorCartera";
                compradorFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorCartera");
                compradorFinal.UnboundType = UnboundColumnType.String;
                compradorFinal.VisibleIndex = 4;
                compradorFinal.Width = 50;
                compradorFinal.Visible = true;
                compradorFinal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compradorFinal);

                //Fecha Cuota 1
                GridColumn fechaCuota1 = new GridColumn();
                fechaCuota1.FieldName = this.unboundPrefix + "FechaCuota1";
                fechaCuota1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCuota1");
                fechaCuota1.UnboundType = UnboundColumnType.String;
                fechaCuota1.VisibleIndex = 5;
                fechaCuota1.Width = 50;
                fechaCuota1.Visible = true;
                fechaCuota1.OptionsColumn.AllowEdit = false;
                fechaCuota1.ColumnEdit = this.editDate;
                this.gvDocument.Columns.Add(fechaCuota1);

                //CuotasVend
                GridColumn cuotasVend = new GridColumn();
                cuotasVend.FieldName = this.unboundPrefix + "CuotasVendidas";
                cuotasVend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuotasVendidas");
                cuotasVend.UnboundType = UnboundColumnType.String;
                cuotasVend.VisibleIndex = 7;
                cuotasVend.Width = 50;
                cuotasVend.Visible = true;
                cuotasVend.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(cuotasVend);

                //TasaVenta
                GridColumn tasaVenta = new GridColumn();
                tasaVenta.FieldName = this.unboundPrefix + "TasaVenta";
                tasaVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TasaVenta");
                tasaVenta.UnboundType = UnboundColumnType.Decimal;
                tasaVenta.VisibleIndex = 8;
                tasaVenta.Width = 110;
                tasaVenta.OptionsColumn.AllowEdit = false;
                tasaVenta.ColumnEdit = this.editSpin7;
                this.gvDocument.Columns.Add(tasaVenta);

                //VlrCuota
                GridColumn vlrGiro = new GridColumn();
                vlrGiro.FieldName = this.unboundPrefix + "VlrCuota";
                vlrGiro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrGiro.UnboundType = UnboundColumnType.Decimal;
                vlrGiro.VisibleIndex = 9;
                vlrGiro.Width = 110;
                vlrGiro.OptionsColumn.AllowEdit = false;
                vlrGiro.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrGiro);

                //Vlr Libranza
                GridColumn vlrLibranza = new GridColumn();
                vlrLibranza.FieldName = this.unboundPrefix + "VlrLibranza";
                vlrLibranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                vlrLibranza.UnboundType = UnboundColumnType.Integer;
                vlrLibranza.VisibleIndex = 10;
                vlrLibranza.Width = 110;
                vlrLibranza.Visible = true;
                vlrLibranza.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                vlrLibranza.OptionsColumn.AllowEdit = false;
                vlrLibranza.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(vlrLibranza);

                //Vlr Venta
                GridColumn VlrVenta = new GridColumn();
                VlrVenta.FieldName = this.unboundPrefix + "VlrVenta";
                VlrVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrVenta");
                VlrVenta.UnboundType = UnboundColumnType.Integer;
                VlrVenta.VisibleIndex = 11;
                VlrVenta.Width = 110;
                VlrVenta.Visible = true;
                VlrVenta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                VlrVenta.OptionsColumn.AllowEdit = false;
                VlrVenta.ColumnEdit = editSpin;
                this.gvDocument.Columns.Add(VlrVenta);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            this.format += this._bc.GetImportExportFormat(typeof(DTO_MigrarVentaCartera), this.documentID);
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Comprador Cartera
            if (colName == this._compradorRsx)
                return AppMasters.ccCompradorCartera;
            //Ciudad
            if (colName == this._ciudadRsx || colName == this._ciudadNacimientoRsx)
                return AppMasters.glLugarGeografico;
            //Zona
            if (colName == this._zonaIDRsx)
                return AppMasters.glZona;

            return 0;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Valida un DTO de comprobante footer en la importacion
        /// </summary>
        /// <param name="dto">DTO a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        /// <param name="msgNoRel">Mensaje que indica que toca tener un codigo comprador, clienteID o nombre comprador</param>
        /// <param name="msgPositive">Mensaje de solo acepta valores positivos</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        private void ValidateDataImport(DTO_MigrarVentaCartera dto, DTO_TxResultDetail rd, string msgNoRel, string msgPositive, string msgEmptyField)
        {
            try
            {
                #region Validacion cliente / codigo / libranza
                if (string.IsNullOrWhiteSpace(dto.ClienteID.Value) && string.IsNullOrWhiteSpace(dto.CompradorCartera.Value) && (dto.Nombre.Value == null))
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID") +
                            _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompradorFinal") +
                            _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Nombre");

                    rdF.Message = msgNoRel;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
                #region Validacion de Valores

                if (dto.VlrCuota.Value.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }

                if (dto.VlrLibranza.Value.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrLibranza");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }

                if (dto.TasaVenta.Value.Value <= 0)
                {
                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                    rdF.Field = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TasaVenta");
                    rdF.Message = msgPositive;
                    rd.DetailsFields.Add(rdF);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "ValidateDataImport"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        private void LoadDetail()
        {
            this.txtRegistros.Text = this.migracionVentaCartera.Count.ToString();
            this.txtTotalFondeo.EditValue = (from c in this.migracionVentaCartera select c.VlrVenta.Value.Value).Sum();
            this.gcDocument.DataSource = this.migracionVentaCartera;
            this.gvDocument.BestFitColumns();
            this.gcDocument.RefreshDataSource();
            this.gvDocument.MoveFirst();
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            //Variables
            this.vendedorCartera = string.Empty;
            this.migracionVentaCartera = new List<DTO_MigrarVentaCartera>();

            //Controles
            this.masterVendedorCartera.Value = string.Empty;
            this.txtOferta.Text = string.Empty;
            this.txtRegistros.Text = string.Empty;
            this.txtTotalFondeo.EditValue = 0;

            this.gcDocument.DataSource = this.migracionVentaCartera;
            this.dtFechaFlujo1.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);
            this.dtFechaFondeo.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.Now.Day);
            FormProvider.Master.itemImport.Enabled = false;
        }

        /// <summary>
        /// Habilita los controles
        /// </summary>
        private void EnableControls(bool enable)
        {
            this.txtOferta.Enabled = enable;
            this.dtFechaFlujo1.Enabled = enable;
            this.dtFechaFondeo.Enabled = enable;
            this.txtRegistros.Enabled = enable;
            this.txtTotalFondeo.Enabled = enable;
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
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemPrint.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Print);
                    FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                    FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemImport.Visible = true;
                    FormProvider.Master.itemGenerateTemplate.Visible = true;
                    FormProvider.Master.itemUpdate.Visible = false;
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemDelete.Visible = false;
                    FormProvider.Master.itemSendtoAppr.Visible = false;
                    FormProvider.Master.itemCopy.Visible = false;
                    FormProvider.Master.itemPaste.Visible = false;
                    FormProvider.Master.itemExport.Visible = false;
                    FormProvider.Master.itemRevert.Visible = false;
                    FormProvider.Master.itemFilter.Visible = false;
                    FormProvider.Master.itemFilterDef.Visible = false;
                    FormProvider.Master.tbBreak1.Visible = false;
                    FormProvider.Master.tbBreak2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta cuando selecciona un comprador
        /// </summary>
        private void masterVendedorCartera_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.vendedorCartera != this.masterVendedorCartera.Value && !String.IsNullOrEmpty(this.masterVendedorCartera.Value))
                {
                    if (this.masterVendedorCartera.ValidID)
                    {
                        FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                        this.vendedorCartera = this.masterVendedorCartera.Value;
                        this.EnableControls(true);
                    }
                    else
                    {
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterVendedorCartera.LabelRsx);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "masterCompradorCartera_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "TBNew"));
            }
        }

        /// <summary>
        /// boton para importar informacion a la grilla
        /// </summary>
        public override void TBImport()
        {
            try
            {
                bool loadData = true;
                if (this.migracionVentaCartera.Count > 0)
                {
                    string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning));
                    string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewData);

                    if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.No)
                        loadData = false;
                }

                if (loadData)
                {
                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "TBNew"));
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
                if (String.IsNullOrWhiteSpace(this.txtOferta.Text))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblOferta.Text);
                    MessageBox.Show(msg);
                    this.txtOferta.Focus();
                }
                else
                {
                    if (this.migracionVentaCartera != null && this.migracionVentaCartera.Count != 0)
                    {
                        #region Carga el DTO_ccCompraDocu
                        short numCuotas = (short)this.migracionVentaCartera.Max(x => x.CuotasVendidas.Value.Value);
                        this.compraDocu.VendedorID.Value = this.vendedorCartera;
                        this.compraDocu.NumCuotas.Value = numCuotas;
                        this.compraDocu.Oferta.Value = this.txtOferta.Text;
                        this.compraDocu.FechaFondeo.Value = this.dtFechaFondeo.DateTime;
                        this.compraDocu.FechaPago1.Value = this.dtFechaFlujo1.DateTime;
                        this.compraDocu.FechaPagoUlt.Value = this.dtFechaFlujo1.DateTime.AddMonths(numCuotas);
                        this.compraDocu.Valor.Value = Convert.ToDecimal(this.txtTotalFondeo.EditValue, CultureInfo.InvariantCulture);
                        #endregion
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "TBSave"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta cualdo el usuario va a imprtar datos al detalle
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    //this.numeroDoc = 0;
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    this.result = new DTO_TxResult();
                    this.result.Result = ResultValue.OK;
                    this.result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    this.migracionVentaCartera = new List<DTO_MigrarVentaCartera>();
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgNoRel = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpNoComp);
                    string msgCompInvalid = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ImpCompInvalid);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    //Popiedades de la incorporacion
                    DTO_MigrarVentaCartera migracionVenta = new DTO_MigrarVentaCartera();
                    //DTO_ccVentaDeta ventaDeta = new DTO_ccVentaDeta();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_MigrarVentaCartera).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);
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

                    //Fks
                    fks.Add(this._compradorRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._ciudadNacimientoRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._zonaIDRsx, new List<Tuple<string, bool>>());
                    fks.Add(this._ciudadRsx, new List<Tuple<string, bool>>());


                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
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
                        #endregion
                        #region Valida si existe data en la lista importada
                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
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
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                this.result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                this.result.Details.Add(rdL);

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
                                    #region Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        if (colRsx == this._compradorRsx || colRsx == this._ciudadNacimientoRsx ||
                                            colRsx == this._zonaIDRsx || colRsx == this._ciudadRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupValid = new Tuple<string, bool>(line[colIndex].Trim(), true);
                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupValid))
                                                continue;
                                            else if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                int docId = this.GetMasterDocumentID(colRsx);
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, false, line[colIndex], true);

                                                if (dto == null)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                                else
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                            }
                                        }
                                    }
                                }
                                    #endregion
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                migracionVenta = new DTO_MigrarVentaCartera();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) && (colName == "CompradorCartera" || colName == "ClienteID" || colName == "Libranza"
                                            || colName == "VlrCuota" || colName == "VlrVenta" || colName == "VlrLibranza"
                                            || colName == "TasaVenta" || colName == "FechaCuota1" || colName == "CuotasVendidas"))
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Valida que el comprador de cartera sea el que se selecciono
                                        if (colValue != vendedorCartera && colName == "CompradorCartera")
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgCompInvalid;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }
                                        #endregion
                                        #region Validacion Formatos

                                        PropertyInfo pi = migracionVenta.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(migracionVenta, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
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
                                        //Valida formatos para las otras columnas
                                        else if (colValue != string.Empty)
                                        {
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
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
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                        }
                                        #endregion

                                        //Si paso las validaciones asigne el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "FondeoCartera.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                                this.migracionVentaCartera.Add(migracionVenta);
                            //this.ventasCartera.Add(ventaDeta);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares de la migracion de nomina
                    if (validList)
                    {
                        #region Variables generales
                        this.result = new DTO_TxResult();
                        this.result.Result = ResultValue.OK;
                        this.result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        #endregion
                        foreach (DTO_MigrarVentaCartera dto in this.migracionVentaCartera)
                        {
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this.migracionVentaCartera.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }
                            #endregion
                            #region Definicion de variables
                            //Variables de resultados
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";
                            createDTO = true;
                            #endregion
                            #region Validaciones particulares del DTO
                            this.ValidateDataImport(dto, rd, msgNoRel, msgPositive, msgEmptyField);
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                this.result.Details.Add(rd);
                                this.result.Result = ResultValue.NOK;

                                validList = false;
                            }
                            #endregion
                        }
                    }
                    #endregion

                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.loadData = true;
                            this.Invoke(this.refreshGridDelegate);
                        }

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }

            }
            catch (Exception ex)
            {
                this.migracionVentaCartera = new List<DTO_MigrarVentaCartera>();
                this._isOK = false;
                this.result.Result = ResultValue.NOK;
                this.result.ResultMessage = _bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "ProcesarThread");
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

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

                List<DTO_SerializedObject> results = new List<DTO_SerializedObject>();
                results = _bc.AdministrationModel.FondeoCartera_Add(this.documentID, this._actFlujo.ID.Value, this.compraDocu, this.migracionVentaCartera);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                #endregion

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

                        bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, AppDocuments.FondeoCartera, this._actFlujo.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                            this.isValid = false;
                        }
                        i++;
                    }

                    frm = new MessageForm(resultsNOK);
                }

                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                if (this.isValid)
                    this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FondeoCartera.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }

}
