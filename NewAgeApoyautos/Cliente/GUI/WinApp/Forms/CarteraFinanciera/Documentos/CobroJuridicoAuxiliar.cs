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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CobroJuridicoAuxiliar : FormWithToolbar
    {
        #region Delegados
        private delegate void RefreshGrid();
        private RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        private void RefreshGridMethod()
        {
            this.LoadData();
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //DTO's
        private List<DTO_CobroJuridicoAuxiliar> cobrosJuridicos = new List<DTO_CobroJuridicoAuxiliar>();
        private List<DTO_ccSaldosComponentes> saldosComponentes = new List<DTO_ccSaldosComponentes>();

        //Variables privadas
        private string clienteID = String.Empty;
        private DateTime periodo;
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

        private bool importando = false;
        private string unboundPrefix = "Unbound_";

        string polizaRsx = string.Empty;
        string polizaIntRsx = string.Empty;
        string polizaSaldoRsx = string.Empty;
        string polizaSaldoIntRsx = string.Empty;
        string capitalRsx = string.Empty;
        string capitalIntRsx = string.Empty;
        string capitalSaldoRsx = string.Empty;
        string capitalSaldoIntRsx = string.Empty;

        #endregion

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CobroJuridicoAuxiliar(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CobroJuridicoAuxiliar()
        {
            //this.InitializeComponent();
            this.Constructor();
        }

        public void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this.SetInitParameters();
            this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

            if (!string.IsNullOrWhiteSpace(mod))
                this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

            FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            this.LoadDocumentInfo(true);
            this.LoadData();
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            //Variables
            this.clienteID = string.Empty;

            this.cobrosJuridicos = new List<DTO_CobroJuridicoAuxiliar>();
            this.gcDocument.DataSource = this.cobrosJuridicos;
            this.gvDocument.RefreshData();
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                        this.txtPrefix.Text = this.prefijoID;

                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected virtual void RowIndexChanged(int fila, bool oper) { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.CJNoIncluidosCartera;
                this.frmModule = ModulesPrefix.cc;

                //Carga la grilla con las columnas
                this.AddGridCols();

                //llena Combos
                Dictionary<string, string> dicEstado = new Dictionary<string, string>();
                dicEstado.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_Normal"));
                dicEstado.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_Mora"));
                dicEstado.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_Prejuridico"));
                dicEstado.Add("4", this._bc.GetResource(LanguageTypes.Tables, "tbl_CobroJuridico"));
                dicEstado.Add("5", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoPago"));
                dicEstado.Add("6", this._bc.GetResource(LanguageTypes.Tables, "tbl_AcuerdoIncumplido"));
                dicEstado.Add("7", this._bc.GetResource(LanguageTypes.Tables, "tbl_Castigada"));
                //this.cmbEstadoActual.Properties.DataSource = dicEstado;

                Dictionary<string, string> dicFiltro = new Dictionary<string, string>();
                dicFiltro.Add(string.Empty, string.Empty);
                dicFiltro.Add("0", this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoActual"));
                dicFiltro.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_Historia"));

                //Estable la fecha con base a la fecha del periodo y fecha ultimo dia Cierre
                string ultimoCierre = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.refreshGridDelegate = new RefreshGrid(RefreshGridMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "SetInitParameters"));
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
                //Seleccion
                GridColumn Seleccion = new GridColumn();
                Seleccion.FieldName = this.unboundPrefix + "Seleccionar";
                Seleccion.Caption = "√";
                Seleccion.UnboundType = UnboundColumnType.Boolean;
                Seleccion.VisibleIndex = 0;
                Seleccion.Width = 50;
                Seleccion.Visible = true;
                Seleccion.ToolTip = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Seleccionar");
                Seleccion.AppearanceHeader.ForeColor = Color.Lime;
                Seleccion.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Seleccion.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Seleccion.AppearanceHeader.Options.UseTextOptions = true;
                Seleccion.AppearanceHeader.Options.UseFont = true;
                Seleccion.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(Seleccion);

                //ClienteDesc
                GridColumn ClienteDesc = new GridColumn();
                ClienteDesc.FieldName = this.unboundPrefix + "ClienteDesc";
                ClienteDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteDesc");
                ClienteDesc.UnboundType = UnboundColumnType.String;
                ClienteDesc.VisibleIndex = 1;
                ClienteDesc.Width = 50;
                ClienteDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ClienteDesc);

                //Libranza
                GridColumn libranza = new GridColumn();
                libranza.FieldName = this.unboundPrefix + "Libranza";
                libranza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Libranza");
                libranza.UnboundType = UnboundColumnType.String;
                libranza.VisibleIndex = 2;
                libranza.Width = 50;
                libranza.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(libranza);

                //FechaDoc
                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "FechaDoc";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 3;
                FechaDoc.Width = 50;
                FechaDoc.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(FechaDoc);

                //TipoDoc
                GridColumn TipoDoc = new GridColumn();
                TipoDoc.FieldName = this.unboundPrefix + "DocumentoID";
                TipoDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoID");
                TipoDoc.UnboundType = UnboundColumnType.Integer;
                TipoDoc.VisibleIndex = 4;
                TipoDoc.Width = 40;
                TipoDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TipoDoc);

                //FechaCont
                GridColumn FechaCont = new GridColumn();
                FechaCont.FieldName = this.unboundPrefix + "FechaCont";
                FechaCont.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCont");
                FechaCont.UnboundType = UnboundColumnType.DateTime;
                FechaCont.VisibleIndex = 5;
                FechaCont.Width = 50;
                FechaCont.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaCont);

                //Comprobante
                GridColumn Comprobante = new GridColumn();
                Comprobante.FieldName = this.unboundPrefix + "Comprobante";
                Comprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Comprobante");
                Comprobante.UnboundType = UnboundColumnType.String;
                Comprobante.VisibleIndex = 6;
                Comprobante.Width = 60;
                Comprobante.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Comprobante);

                //VlrCapital
                GridColumn VlrCapital = new GridColumn();
                VlrCapital.FieldName = this.unboundPrefix + "VlrCapital";
                VlrCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCapital");
                VlrCapital.UnboundType = UnboundColumnType.Decimal;
                VlrCapital.VisibleIndex = 7;
                VlrCapital.Width = 120;
                VlrCapital.OptionsColumn.AllowEdit = false;
                VlrCapital.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrCapital);

                //VlrIntCapital
                GridColumn VlrIntCapital = new GridColumn();
                VlrIntCapital.FieldName = this.unboundPrefix + "VlrIntCapital";
                VlrIntCapital.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntCapital");
                VlrIntCapital.UnboundType = UnboundColumnType.Decimal;
                VlrIntCapital.VisibleIndex = 8;
                VlrIntCapital.Width = 100;
                VlrIntCapital.OptionsColumn.AllowEdit = false;
                VlrIntCapital.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrIntCapital);

                //VlrPoliza
                GridColumn VlrPoliza = new GridColumn();
                VlrPoliza.FieldName = this.unboundPrefix + "VlrPoliza";
                VlrPoliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPoliza");
                VlrPoliza.UnboundType = UnboundColumnType.Decimal;
                VlrPoliza.VisibleIndex = 9;
                VlrPoliza.Width = 120;
                VlrPoliza.OptionsColumn.AllowEdit = false;
                VlrPoliza.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrPoliza);

                //VlrIntPoliza
                GridColumn VlrIntPoliza = new GridColumn();
                VlrIntPoliza.FieldName = this.unboundPrefix + "VlrIntPoliza";
                VlrIntPoliza.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIntPoliza");
                VlrIntPoliza.UnboundType = UnboundColumnType.Decimal;
                VlrIntPoliza.VisibleIndex = 10;
                VlrIntPoliza.Width = 100;
                VlrIntPoliza.OptionsColumn.AllowEdit = false;
                VlrIntPoliza.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrIntPoliza);

                //VlrGastos
                GridColumn VlrGastos = new GridColumn();
                VlrGastos.FieldName = this.unboundPrefix + "VlrGastos";
                VlrGastos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrGastos");
                VlrGastos.UnboundType = UnboundColumnType.Decimal;
                VlrGastos.VisibleIndex = 11;
                VlrGastos.Width = 80;
                VlrGastos.OptionsColumn.AllowEdit = false;
                VlrGastos.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(VlrGastos);

                #region Columnas no visibles
                //ClienteID
                GridColumn clienteID = new GridColumn();
                clienteID.FieldName = this.unboundPrefix + "ClienteID";
                clienteID.UnboundType = UnboundColumnType.String;
                clienteID.Visible = false;
                this.gvDocument.Columns.Add(clienteID);

                //ComprobanteID
                GridColumn ComprobanteID = new GridColumn();
                ComprobanteID.FieldName = this.unboundPrefix + "ComprobanteID";
                ComprobanteID.UnboundType = UnboundColumnType.String;
                ComprobanteID.Visible = false;
                this.gvDocument.Columns.Add(ComprobanteID);

                //ComprobanteIDNro
                GridColumn ComprobanteIDNro = new GridColumn();
                ComprobanteIDNro.FieldName = this.unboundPrefix + "ComprobanteIDNro";
                ComprobanteIDNro.UnboundType = UnboundColumnType.Integer;
                ComprobanteIDNro.Visible = false;
                this.gvDocument.Columns.Add(ComprobanteIDNro);
                #endregion
                #endregion
                #region Agrega las columnas Subgrilla

                //ComponenteCarteraID
                GridColumn componenteCarteraID = new GridColumn();
                componenteCarteraID.FieldName = this.unboundPrefix + "Componente";
                componenteCarteraID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ComponenteCarteraID");
                componenteCarteraID.UnboundType = UnboundColumnType.String;
                componenteCarteraID.VisibleIndex = 0;
                componenteCarteraID.Width = 80;
                componenteCarteraID.Visible = true;
                componenteCarteraID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(componenteCarteraID);

                //Descripcion
                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "ComponenteDesc";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 1;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(descripcion);

                //VlrPagar
                GridColumn vlrPagar = new GridColumn();
                vlrPagar.FieldName = this.unboundPrefix + "VlrCuota";
                vlrPagar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrPagar.UnboundType = UnboundColumnType.Decimal;
                vlrPagar.VisibleIndex = 2;
                vlrPagar.Width = 130;
                vlrPagar.Visible = true;
                vlrPagar.OptionsColumn.AllowEdit = false;
                vlrPagar.ColumnEdit = this.editSpin;
                this.gvDetalle.Columns.Add(vlrPagar);

                #endregion
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Carga la Info general
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.cobrosJuridicos = new List<DTO_CobroJuridicoAuxiliar>();
                this.cobrosJuridicos = this._bc.AdministrationModel.GetCobroJuridicoFromAuxiliar();
                if (this.cobrosJuridicos.Count > 0)
                {
                    if (this.cobrosJuridicos.Count > 0)
                    {
                        this.gcDocument.DataSource = this.cobrosJuridicos;
                        this.gvDocument.MoveFirst();
                        this.gvDocument.BestFitColumns();
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                }
                else
                {
                    this.gcDocument.DataSource = this.cobrosJuridicos;
                    this.gvDocument.RefreshData();
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "LoadData"));
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
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemNew.Visible = false;
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
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                FormProvider.Master.itemUpdate.Enabled = true;
                if (FormProvider.Master.LoadFormTB)
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Leave(object sender, EventArgs e)
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
        protected void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgLostInfo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);

                    if (this.importando)
                        e.Cancel = true;
                    else
                        FormProvider.Master.Form_Closing(this, this.documentID);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_FormClosed(object sender, FormClosedEventArgs e)
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

        #region Eventos grilla de Documentos

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RowIndexChanged(e.FocusedRowHandle, false);
            if (e.FocusedRowHandle >= 0)
            {

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
        private void gvHistoria_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                //DTO_ccCJHistorico row = (DTO_ccCJHistorico)this.gvHistoria.GetRow(e.RowHandle);
                //if (fieldName == "TipoMvto")
                //{
                //    if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico)
                //        e.DisplayText = "CJU";
                //    else if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago)
                //        e.DisplayText = "AP";
                //    else if (Convert.ToInt32(e.Value) == 1 & row.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                //        e.DisplayText = "API";
                //    else if (Convert.ToInt32(e.Value) == 2)
                //        e.DisplayText = "Abo";
                //    else if (Convert.ToInt32(e.Value) == 3)
                //        e.DisplayText = "Int";
                //    else if (Convert.ToInt32(e.Value) == 4)
                //        e.DisplayText = "Pol";
                //    else if (Convert.ToInt32(e.Value) == 5)
                //        e.DisplayText = "Gto";
                //}
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
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "TBUpdate"));
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
                bool ExistSelect = this.cobrosJuridicos.Exists(x => x.Seleccionar.Value.Value);
                if (ExistSelect)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NotSelectedItemDetail);
                    MessageBox.Show(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                List<DTO_TxResult> results = this._bc.AdministrationModel.ccCJHistorico_Add(this.documentID, this.cobrosJuridicos);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                MessageForm frm = new MessageForm(results);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshGridDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CobroJuridicoAuxiliar.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }

}
