using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using SentenceTransformer;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using DevExpress.XtraReports.UI;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class GarantiaControl : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void RefreshDataMethod()
        {
            this.LoadData();
            this.CleanControls();
        }

        #endregion

        #region Variables
        private int _documentID;
        private ModulesPrefix frmModule;

        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string _tab = "\t";
        private string _unboundPrefix = "Unbound_";
        private bool _firstTime = true;
        private List<DTO_glGarantiaControl> detalle = new List<DTO_glGarantiaControl>();
        private DTO_glGarantiaControl _currentRow = new DTO_glGarantiaControl();
        private bool deleteOP = false;
        private bool isValid = false;
        #endregion

        public GarantiaControl()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Asigna la lista de columnas
                this.AddGridCols();
                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "GarantiaControl"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.ControlGarantias;
            this.frmModule = ModulesPrefix.gl;

            this.gcGarantia.ShowOnlyPredefinedDetails = true;
            this.InitControls();
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.masterDocumentoGar, AppMasters.glDocumento, true, false, true, false);
            this._bc.InitMasterUC(this.masterTerceroGar, AppMasters.coTercero, true, true, false, true);
            this._bc.InitMasterUC(this.masterPrefijoGar, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterFaseColda, AppMasters.ccFasecolda, true, true, true, false);
            this._bc.InitMasterUC(this.masterGarantia, AppMasters.glGarantia, true, true, true, false);

            //Combo Tipo
            Dictionary<string, string> dicEstado = new Dictionary<string, string>();
            dicEstado.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Vencidas"));
            dicEstado.Add("2", this._bc.GetResource(LanguageTypes.Tables, "No Vencidas"));
            dicEstado.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Todas"));
            this.cmbEstado.Properties.DataSource = dicEstado;
            this.cmbEstado.EditValue = 3;

            //Combo TipoGarantia
            Dictionary<string, string> dicTipoGar = new Dictionary<string, string>();
            dicTipoGar.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Garantia Hipotecaria"));
            dicTipoGar.Add("2", this._bc.GetResource(LanguageTypes.Tables, "Garantia Prendaria"));
            dicTipoGar.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Garantia Personal"));
            dicTipoGar.Add("4", this._bc.GetResource(LanguageTypes.Tables, "Garantia Póliza"));
            this.cmbTipoGarantia.Properties.DataSource = dicTipoGar;

            //Combo FuentePre
            Dictionary<string, string> dicFuentePre = new Dictionary<string, string>();
            dicFuentePre.Add("1", "Factura");
            dicFuentePre.Add("2","Fasecolda");
            dicFuentePre.Add("3", "Revista Motor");
            this.cmbFuentePre.Properties.DataSource = dicFuentePre;
            this.cmbFuentePre.EditValue = 1;

            //Combo FuenteHipot
            Dictionary<string, string> dicFuenteHipot = new Dictionary<string, string>();
            dicFuenteHipot.Add("1", "Comercial");
            dicFuenteHipot.Add("2", "Comercial Otro");
            dicFuenteHipot.Add("3", "Predial");
            this.cmbFuenteHip.Properties.DataSource = dicFuenteHipot;
            this.cmbFuenteHip.EditValue = 1;

            //Combo TipoVehiculo
            Dictionary<string, string> dicTipoVeh = new Dictionary<string, string>();
            dicTipoVeh.Add("1", "Particular");
            dicTipoVeh.Add("2", "Público");
            this.cmbTipoVehiculo.Properties.DataSource = dicTipoVeh;
            this.cmbTipoVehiculo.EditValue = 1;

            //Combo TipoInmueble
            Dictionary<string, string> dicTipoInmueb = new Dictionary<string, string>();
            dicTipoInmueb.Add("0", "N/A");
            dicTipoInmueb.Add("1", "Apto");
            dicTipoInmueb.Add("2", "Casa");
            dicTipoInmueb.Add("3", "Finca");
            dicTipoInmueb.Add("4", "Lote");
            dicTipoInmueb.Add("5", "Local Comercial");
            dicTipoInmueb.Add("6", "Bodega");
            this.cmbTipoInmueble.Properties.DataSource = dicTipoInmueb;
            this.cmbTipoInmueble.EditValue = 0;

            this.gbGarantiaHipotecaria.Enabled = false;
            this.gbGarantiaPrendaria.Enabled = false;
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadData()
        {
            try
            {
                if (this.ValidateHeader())
                {
                    DTO_glGarantiaControl filter = new DTO_glGarantiaControl();
                    filter.TerceroID.Value = this.masterTerceroGar.Value;
                    filter.DocumentoID.Value = this.masterDocumentoGar.ValidID ? Convert.ToInt32(this.masterDocumentoGar.Value) : filter.DocumentoID.Value;
                    filter.ActivoInd.Value = this.chkActivo.Checked;
                    this.detalle = this._bc.AdministrationModel.glGarantiaControl_GetByParameter(filter, this.masterPrefijoGar.Value, null, Convert.ToByte(this.cmbEstado.EditValue));

                    this.gcGarantia.DataSource = null;
                    this.gcGarantia.DataSource = this.detalle;
                    this.gvGarantia.RefreshData();
                    this.gvGarantia.FocusedRowHandle = this.gvGarantia.DataRowCount - 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "LoadData"));
            }

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Garantias
                //GarantiaID
                GridColumn GarantiaID = new GridColumn();
                GarantiaID.FieldName = this._unboundPrefix + "GarantiaID";
                GarantiaID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaID");
                GarantiaID.UnboundType = UnboundColumnType.DateTime;
                GarantiaID.VisibleIndex = 1;
                GarantiaID.Width = 35;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(GarantiaID);

                //GarantiaDesc
                GridColumn GarantiaDesc = new GridColumn();
                GarantiaDesc.FieldName = this._unboundPrefix + "GarantiaDesc";
                GarantiaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaDesc");
                GarantiaDesc.UnboundType = UnboundColumnType.String;
                GarantiaDesc.VisibleIndex = 2;
                GarantiaDesc.Width = 70;
                GarantiaDesc.Visible = true;
                GarantiaDesc.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(GarantiaDesc);

                //PrefDocLlam(Documento)
                GridColumn PrefDocLlam = new GridColumn();
                PrefDocLlam.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDocLlam.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDocLlam.UnboundType = UnboundColumnType.String;
                PrefDocLlam.VisibleIndex = 4;
                PrefDocLlam.Width = 60;
                PrefDocLlam.Visible = true;
                PrefDocLlam.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(PrefDocLlam);

                //CodigoGarantia
                GridColumn CodigoGarantia = new GridColumn();
                CodigoGarantia.FieldName = this._unboundPrefix + "CodigoGarantia";
                CodigoGarantia.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodigoGarantia");
                CodigoGarantia.UnboundType = UnboundColumnType.String;
                CodigoGarantia.VisibleIndex = 5;
                CodigoGarantia.Width = 70;
                CodigoGarantia.Visible = true;
                CodigoGarantia.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(CodigoGarantia);

                //FechaVto
                GridColumn FechaVto = new GridColumn();
                FechaVto.FieldName = this._unboundPrefix + "FechaVTO";
                FechaVto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaVto");
                FechaVto.UnboundType = UnboundColumnType.DateTime;
                FechaVto.VisibleIndex = 6;
                FechaVto.Width = 70;
                FechaVto.Visible = true;
                FechaVto.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(FechaVto);

                //VlrAsegurado
                GridColumn VlrAsegurado = new GridColumn();
                VlrAsegurado.FieldName = this._unboundPrefix + "VlrAsegurado";
                VlrAsegurado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_VlrAsegurado");
                VlrAsegurado.UnboundType = UnboundColumnType.Decimal;
                VlrAsegurado.VisibleIndex = 7;
                VlrAsegurado.Width = 70;
                VlrAsegurado.Visible = true;
                VlrAsegurado.OptionsColumn.AllowEdit = false;
                VlrAsegurado.ColumnEdit = this.editSpin;
                this.gvGarantia.Columns.Add(VlrAsegurado);

                //Descripcion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this._unboundPrefix + "Descripcion";
                Descripcion.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 8;
                Descripcion.Width = 90;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(Descripcion);

                //VerDoc
                GridColumn VerDoc = new GridColumn();
                VerDoc.FieldName = this._unboundPrefix + "VerDoc";
                VerDoc.OptionsColumn.ShowCaption = false;
                VerDoc.UnboundType = UnboundColumnType.String;
                VerDoc.Width = 50;
                VerDoc.VisibleIndex = 9;
                VerDoc.Visible = true;
                VerDoc.ColumnEdit = this.editLink;
                this.gvGarantia.Columns.Add(VerDoc);
                #endregion
                this.gvGarantia.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                //#region FechaVto
                //DTO_prContratoPolizas pol = (DTO_prContratoPolizas)this.gvGarantia.GetFocusedRow();
                //string msg = pol != null && pol.FechaVto.Value != null ? string.Empty : "Fecha Vencimiento Vacio";
                //GridColumn col = this.gvGarantia.Columns[this._unboundPrefix + "FechaVto"];
                //this.gvGarantia.SetColumnError(col, msg);
                //validField = string.IsNullOrEmpty(msg) ? true : false;
                //if (!validField)
                //    validRow = false;
                //#endregion
                //#region Observacion
                //validField = _bc.ValidGridCell(this.gvGarantia, string.Empty, fila, "Observacion", false, false, false, null);
                //if (!validField)
                //    validRow = false;
                //#endregion
                this.isValid = validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_glGarantiaControl garan = new DTO_glGarantiaControl();
            try
            {

                if (this.ValidateHeader())
                {
                    #region Asigna datos a la fila
                    garan.ActivoInd.Value = true;
                    garan.Consecutivo.Value = 0;
                    garan.DocumentoID.Value = this._documentID;
                    garan.TerceroID.Value = this.detalle.Count > 0 ? this.detalle.Last().TerceroID.Value : this.masterTerceroGar.Value;
                    garan.GarantiaID.Value = this.detalle.Count > 0 ? this.detalle.Last().GarantiaID.Value : string.Empty;
                    garan.GarantiaDesc.Value = this.detalle.Count > 0 ? this.detalle.Last().GarantiaDesc.Value : string.Empty;
                    garan.FechaINI.Value = DateTime.Today;
                    garan.FechaVTO.Value = DateTime.Today;
                    #endregion

                    this.detalle.Add(garan);
                    this.gcGarantia.DataSource = this.detalle;
                    this.gvGarantia.RefreshData();
                    this.gvGarantia.FocusedRowHandle = this.gvGarantia.DataRowCount - 1;
                    this.pnDatosBasicos.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "AddNewRow"));
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            #region Valida datos en la maestra de Tercero
            if (!this.masterTerceroGar.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTerceroGar.CodeRsx);

                MessageBox.Show(msg);
                this.masterTerceroGar.Focus();

                return false;
            }
            #endregion
            return true;
        }

        /// <summary>
        /// Limpia los controles del form
        /// </summary>
        private void CleanControls()
        {
            this.masterTerceroGar.Value = string.Empty;
            this.masterDocumentoGar.Value = string.Empty;
            this.masterPrefijoGar.Value = string.Empty;
            this.masterGarantia.Value = string.Empty;
            this.txtDocNroGar.Text = string.Empty;
            this.txtAnoHip.Text = string.Empty;
            this.txtCodigoGaranHip.Text = string.Empty;
            this.txtPrenda.Text = string.Empty;
            this.txtCodigoGaranPre.Text = string.Empty;
            this.txtEscritura.Text = string.Empty;
            this.txtCorreoGar.Text = string.Empty;
            this.txtMarcaFasecolda.Text = string.Empty;
            this.txtClaseFasecolda.Text = string.Empty;
            this.txtDireccion.Text = string.Empty;
            this.txtDireccionGar.Text = string.Empty;
            this.txtModeloPre.Text = string.Empty;
            this.txtTelefonoGar.Text = string.Empty;
            this.txtChasis.Text = string.Empty;
            this.txtSerie.Text = string.Empty;
            this.txtMotor.Text = string.Empty;
            this.txtVlrAsegurado.EditValue = 0;
            this.txtVlrFuente.EditValue = 0;
            this.masterTerceroGar.Focus();

            this.gbGarantiaHipotecaria.Enabled = false;
            this.gbGarantiaPrendaria.Enabled = false;
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Add);
                    FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
            }
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGarantia_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                this.deleteOP = false;
                if (this.isValid || this.gvGarantia.RowCount == 0)
                    this.AddNewRow();
                else
                {
                    this.gvGarantia.PostEditor();
                    bool isV = this.ValidateRow(this.gvGarantia.FocusedRowHandle);
                    if (isV)
                        this.AddNewRow();
                }
            }
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove && this.gvGarantia.RowCount > 0)
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                //Revisa si desea cargar los temporales
                if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.deleteOP = true;
                    int rowHandle = this.gvGarantia.FocusedRowHandle;

                    this.gvGarantia.DeleteRow(this.gvGarantia.FocusedRowHandle);

                    if (this.gvGarantia.RowCount > 0)
                    {
                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvGarantia.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvGarantia.FocusedRowHandle = rowHandle - 1;
                    }
                    else
                        this.gvGarantia.Focus();
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGarantia_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this._unboundPrefix.Length;

            string fieldName = e.Column.FieldName.Substring(unboundPrefixLen);

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
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
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
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGarantia_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (this.gvGarantia.FocusedRowHandle >= 0)
                {
                    this._currentRow = (DTO_glGarantiaControl)this.gvGarantia.GetRow(this.gvGarantia.FocusedRowHandle);
                    DTO_glGarantia garan = (DTO_glGarantia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glGarantia, false, this._currentRow.GarantiaID.Value, true);
                    this.masterGarantia.Value = this._currentRow.GarantiaID.Value;
                    this.cmbTipoGarantia.EditValue = garan != null ? garan.GarantiaTipo.Value.ToString() : this.cmbTipoGarantia.EditValue;
                    if (this.cmbTipoGarantia.EditValue != null && this.cmbTipoGarantia.EditValue.Equals("1")) // Hipotecaria
                    {
                        this.gbGarantiaHipotecaria.Enabled = true;
                        this.gbGarantiaPrendaria.Enabled = false;

                        //Limpia controles
                        string codigoGarantia = this._currentRow.CodigoGarantia.Value;
                        string codigoGarantia1 = this._currentRow.CodigoGarantia1.Value;
                        this.txtCodigoGaranPre.Text = string.Empty;
                        this.txtModeloPre.Text = string.Empty;
                        this.cmbFuentePre.EditValue = 1;
                        this.masterFaseColda.Value = string.Empty;
                        if (this._currentRow.VehiculoTipo.Value != null)
                            this.cmbTipoVehiculo.EditValue = 1;
                        this.txtChasis.Text = string.Empty;
                        this.txtSerie.Text = string.Empty;
                        this.txtMotor.Text = string.Empty;
                        this.txtMarcaFasecolda.Text = string.Empty;
                        this.txtClaseFasecolda.Text = string.Empty;
                        this.txtPrenda.Text = string.Empty;

                        if (this._currentRow.InmuebleTipo.Value != null)
                            this.cmbTipoInmueble.EditValue = this._currentRow.InmuebleTipo.Value.ToString();
                        this.txtDireccion.Text = this._currentRow.Direccion.Value;
                        this.txtCodigoGaranHip.Text = codigoGarantia;
                        this.txtEscritura.Text = codigoGarantia1;
                        this.txtAnoHip.Text = this._currentRow.Modelo.Value.ToString();
                        if (this._currentRow.FuenteHIP.Value != null)
                            this.cmbFuenteHip.EditValue = this._currentRow.FuenteHIP.Value.ToString();

                    }
                    else if (this.cmbTipoGarantia.EditValue != null &&  this.cmbTipoGarantia.EditValue.Equals("2")) // Prendaria
                    {
                        this.gbGarantiaHipotecaria.Enabled = false;
                        this.gbGarantiaPrendaria.Enabled = true;

                        //Limpia controles
                        string codigoGarantia = this._currentRow.CodigoGarantia.Value;
                        string codigoGarantia1 = this._currentRow.CodigoGarantia1.Value;
                        this.cmbTipoInmueble.EditValue = 0;
                        this.txtDireccion.Text = string.Empty;
                        this.txtCodigoGaranHip.Text = string.Empty;
                        this.txtAnoHip.Text = string.Empty;
                        this.cmbFuenteHip.EditValue = 1;
                        this.txtMarcaFasecolda.Text = string.Empty;
                        this.txtClaseFasecolda.Text = string.Empty;
                        this.txtEscritura.Text = string.Empty;

                        this.txtCodigoGaranPre.Text = codigoGarantia;
                        this.txtPrenda.Text = codigoGarantia1;
                        this.txtModeloPre.Text = this._currentRow.Modelo.Value.ToString();
                        if (this._currentRow.FuentePRE.Value != null)
                            this.cmbFuentePre.EditValue = this._currentRow.FuentePRE.Value.ToString();
                        this.masterFaseColda.Value = this._currentRow.FaseColdaID.Value;
                        if (this.masterFaseColda.ValidID)
                        {
                            DTO_ccFasecolda fase = (DTO_ccFasecolda)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, false, this.masterFaseColda.Value, true);
                            this.txtMarcaFasecolda.Text = fase.Marca.Value;
                            this.txtClaseFasecolda.Text = fase.Clase.Value;
                        }
                        if (this._currentRow.VehiculoTipo.Value != null)
                            this.cmbTipoVehiculo.EditValue = this._currentRow.VehiculoTipo.Value.ToString();
                        this.txtChasis.Text = this._currentRow.Dato1.Value;
                        this.txtSerie.Text = this._currentRow.Dato2.Value;
                        this.txtMotor.Text = this._currentRow.Dato3.Value;

                      
                    }
                    else
                    {
                        this.gbGarantiaHipotecaria.Enabled = false;
                        this.gbGarantiaPrendaria.Enabled = false;

                        //Limpia controles
                        this.txtCodigoGaranPre.Text = string.Empty;
                        this.txtModeloPre.Text = string.Empty;
                        this.cmbFuentePre.EditValue = 1;
                        this.masterFaseColda.Value = string.Empty;
                        if (this._currentRow.VehiculoTipo.Value != null)
                            this.cmbTipoVehiculo.EditValue = 1;
                        this.txtChasis.Text = string.Empty;
                        this.txtSerie.Text = string.Empty;
                        this.txtMotor.Text = string.Empty;
                        this.cmbTipoInmueble.EditValue = 0;
                        this.txtDireccion.Text = string.Empty;
                        this.txtCodigoGaranHip.Text = string.Empty;
                        this.txtPrenda.Text = string.Empty;
                        this.txtEscritura.Text = string.Empty;
                        this.txtAnoHip.Text = string.Empty;
                        this.cmbFuenteHip.EditValue = 1;
                        this.txtMarcaFasecolda.Text = string.Empty;
                        this.txtClaseFasecolda.Text = string.Empty;
                    }

                    this.dtFechaInicio.DateTime = this._currentRow.FechaINI != null ? this._currentRow.FechaINI.Value.Value : this.dtFechaInicio.DateTime;
                    this.dtFechaVto.DateTime = this._currentRow.FechaVTO != null ? this._currentRow.FechaVTO.Value.Value : this.dtFechaVto.DateTime;
                    this.txtVlrFuente.EditValue = this._currentRow.VlrFuente.Value.HasValue ? this._currentRow.VlrFuente.Value : 0;
                    this.txtVlrAsegurado.EditValue = this._currentRow.VlrAsegurado.Value.HasValue ? this._currentRow.VlrAsegurado.Value : 0;
                    
                    this.chkNuevo.Checked = this._currentRow.NuevoInd.Value != null ? this._currentRow.NuevoInd.Value.Value : true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "gvGarantia_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines cuando los valores etstan engresados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGarantia_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                this.ValidateRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "gvGarantia_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvGarantia_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
            this.deleteOP = false;

            if (validRow)
            {
                this.isValid = true;
            }
            else
            {
                e.Allow = false;
                this.isValid = false;
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvGarantia_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "VerDoc")
            {
                if (!string.IsNullOrEmpty(this.detalle[e.ListSourceRowIndex].NumeroDoc.Value.ToString()))
                    e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
            }
            if (fieldName == "UnidadTiempo")
            {
                if (Convert.ToInt32(e.Value) == 1 || (Convert.ToInt32(e.Value) == 2))
                    e.DisplayText = "Hora";
                else if (Convert.ToInt32(e.Value) == 3 || (Convert.ToInt32(e.Value) == 4) || (Convert.ToInt32(e.Value) == 5))
                    e.DisplayText = "Día";
            }
            if (fieldName == "Estado")
            {
                if (Convert.ToInt32(e.Value) == 0)
                    e.DisplayText = "AC";
                else
                    e.DisplayText = "CE";
            }

        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanControls();
                this.detalle = new List<DTO_glGarantiaControl>();
                this.pnDatosBasicos.Enabled = false;

                this.gcGarantia.DataSource = this.detalle;
                this.gvGarantia.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvGarantia.PostEditor();
            try
            {

                Thread process = new Thread(this.SaveThread);
                process.Start();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Eventos Controles


        /// <summary>
        /// Evento que se ejecuta para ver un anexo existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkVer_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvGarantia.FocusedRowHandle;

                if (!string.IsNullOrEmpty(this.detalle[fila].NumeroDoc.Value.ToString()))
                {
                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.detalle[fila].NumeroDoc.Value.Value);
                    comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "linkVer_Click"));
            }

        }

        /// <summary>
        /// Se realiza al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterGarantia_Leave(object sender, EventArgs e)
        {
            try
            {
                ControlsUC.uc_MasterFind txt = (ControlsUC.uc_MasterFind)sender;
                int fila = this.gvGarantia.FocusedRowHandle;

                if (fila >= 0 && txt.ValidID)
                {
                    if (txt.Name == "masterFaseColda")
                    {
                        DTO_ccFasecolda fase = (DTO_ccFasecolda)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccFasecolda, false, txt.Value, true);
                        if (fase != null)
                        {
                            this.detalle[fila].FaseColdaID.Value = txt.Value;
                            this.txtMarcaFasecolda.Text = fase.Marca.Value;
                            this.txtClaseFasecolda.Text = fase.Clase.Value;
                        }
                        else
                        {
                            this.txtMarcaFasecolda.Text = string.Empty;
                            this.txtClaseFasecolda.Text = string.Empty;
                        }
                    }

                    if (txt.Name == "masterGarantia")
                    {
                        DTO_glGarantia garan = (DTO_glGarantia)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glGarantia, false, txt.Value, true);
                        this.detalle[fila].GarantiaID.Value = txt.Value;
                        this.detalle[fila].GarantiaDesc.Value = garan.Descriptivo.Value;
                        this.cmbTipoGarantia.EditValue = garan.GarantiaTipo.Value.ToString();
                    }
                }
                this.gvGarantia.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "masterGarantia_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.gvGarantia.FocusedRowHandle >= 0)
                {
                    if (sender.GetType() == typeof(TextBox))
                    {
                        TextBox txt = (TextBox)sender;

                        if (txt.Name == "txtCodigoGaranPre")
                            this._currentRow.CodigoGarantia.Value = txt.Text;
                        else if (txt.Name == "txtPrenda") 
                            this._currentRow.CodigoGarantia1.Value = txt.Text;
                        else if (txt.Name == "txtModeloPre" && !string.IsNullOrEmpty(txt.Text))
                            this._currentRow.Modelo.Value = Convert.ToInt16(txt.Text);
                        else if (txt.Name == "txtFuentePre" && !string.IsNullOrEmpty(txt.Text))
                            this._currentRow.FuentePRE.Value = Convert.ToByte(txt.Text);
                        else if (txt.Name == "txtDireccion")
                            this._currentRow.Direccion.Value = txt.Text;
                        else if (txt.Name == "txtCodigoGaranHip")
                            this._currentRow.CodigoGarantia.Value = txt.Text;
                        else if (txt.Name == "txtEscritura")
                            this._currentRow.CodigoGarantia1.Value = txt.Text;
                        else if (txt.Name == "txtAnoHip" && !string.IsNullOrEmpty(txt.Text))
                        {
                            this._currentRow.Ano.Value = Convert.ToInt16(txt.Text);
                            this._currentRow.Modelo.Value = Convert.ToInt16(txt.Text);
                        }
                        else if (txt.Name == "txtChasis") 
                            this._currentRow.Dato1.Value = txt.Text;
                        else if (txt.Name == "txtSerie")
                            this._currentRow.Dato2.Value = txt.Text;
                        else if (txt.Name == "txtMotor")
                            this._currentRow.Dato3.Value = txt.Text;
                    }
                    if (sender.GetType() == typeof(TextEdit))
                    {
                        TextEdit txt = (TextEdit)sender;
                        if (txt.Name == "txtVlrFuente" && !string.IsNullOrEmpty(txt.EditValue.ToString()))
                            this._currentRow.VlrFuente.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                        if (txt.Name == "txtVlrAsegurado" && !string.IsNullOrEmpty(txt.EditValue.ToString()))
                            this._currentRow.VlrAsegurado.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                    if (sender.GetType() == typeof(DateEdit))
                    {
                        DateEdit txt = (DateEdit)sender;
                        if (txt.Name == "dtFechaVto")
                            this._currentRow.FechaVTO.Value = txt.DateTime;
                        if (txt.Name == "dtFechaInicio")
                            this._currentRow.FechaINI.Value = txt.DateTime;
                    }
                    if (sender.GetType() == typeof(LookUpEdit))
                    {
                        LookUpEdit cmb = (LookUpEdit)sender;
                        if (cmb.Name == "cmbTipoGarantia" && cmb.EditValue.Equals("1")) //Hipotecaria
                        {
                            this.gbGarantiaHipotecaria.Enabled = true;
                            this.gbGarantiaPrendaria.Enabled = false;
                        }
                        else if (cmb.Name == "cmbTipoGarantia" && cmb.EditValue.Equals("2")) // Prendaria
                        {
                            this.gbGarantiaHipotecaria.Enabled = false;
                            this.gbGarantiaPrendaria.Enabled = true;
                        }
                        else if (cmb.Name == "cmbTipoGarantia")
                        {
                            this.gbGarantiaHipotecaria.Enabled = false;
                            this.gbGarantiaPrendaria.Enabled = false;
                        }
                        if (cmb.Name == "cmbFuentePre")
                            this._currentRow.FuentePRE.Value = Convert.ToByte(cmb.EditValue);                     
                        else if (cmb.Name == "cmbFuenteHip")
                            this._currentRow.FuenteHIP.Value = Convert.ToByte(cmb.EditValue);
                        else if (cmb.Name == "cmbTipoVehiculo")
                            this._currentRow.VehiculoTipo.Value = Convert.ToByte(cmb.EditValue);
                        else if (cmb.Name == "cmbTipoInmueble")
                            this._currentRow.InmuebleTipo.Value = Convert.ToByte(cmb.EditValue);
                    }
                }
                this.gvGarantia.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "txt_TextChanged"));
            }

        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFuentePre_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFuenteHip_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNuevo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvGarantia.FocusedRowHandle;
                if (fila >= 0)
                {
                    this.detalle[fila].NuevoInd.Value = this.chkNuevo.Checked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "LoadData"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al asignar
        /// </summary>
        private void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                DTO_TxResult results = _bc.AdministrationModel.glGarantiaControl_Add(this._documentID, this.detalle);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(results);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                //if (results.Result == ResultValue.OK)
                //    this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GarantiaControl.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        }

        #endregion
    }
}
