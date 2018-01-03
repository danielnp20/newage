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
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class TerceroDataForm : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private int _documentID;
        private ModulesPrefix frmModule;

        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string _tab = "\t";
        private string _unboundPrefix = "Unbound_";
        private bool _firstTime = true;
        private List<DTO_glGarantiaControl> detalle = new List<DTO_glGarantiaControl>();
        private List<DTO_glDocumentoAnexo> docAnexoList;

        #endregion

        /// <summary>
        /// Constructor con un documento
        /// </summary>
        /// <param name="doc"></param>
        public TerceroDataForm()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public TerceroDataForm(string mod)
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Funcion que llama el Constructor del Documento
        /// </summary>
        private void Constructor(string mod = null)
        {
            this.InitializeComponent();
            this._documentID = AppForms.TerceroDataForm;
            this.frmModule = ModulesPrefix.gl;
            this._frmName = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
            FormProvider.Master.Form_Load(this, this.frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

            this.AddGridCols();
            this.InitControls();
            this.LoadPageData();
            this._firstTime = false;
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            #region Garantias
            this._bc.InitMasterUC(this.masterDocumentoGar, AppMasters.glDocumento, true, true, true, false);
            this._bc.InitMasterUC(this.masterTerceroGar, AppMasters.coTercero, true, true, true, true);
            this._bc.InitMasterUC(this.masterPrefijoGar, AppMasters.glPrefijo, true, true, true, false);
            //Tipo
            Dictionary<string, string> dicTipo = new Dictionary<string, string>();
            dicTipo.Add("1", this._bc.GetResource(LanguageTypes.Tables, "Vencidas"));
            dicTipo.Add("2", this._bc.GetResource(LanguageTypes.Tables, "No Vencidas"));
            dicTipo.Add("3", this._bc.GetResource(LanguageTypes.Tables, "Todas"));
            this.cmbEstado.Properties.DataSource = dicTipo;
            this.cmbEstado.EditValue = 3;
            #endregion
            #region Anexos
            this._bc.InitMasterUC(this.masterDocumentoAnexo, AppMasters.glDocumento, true, true, true, false);
            this._bc.InitMasterUC(this.masterTerceroAnexos, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoAnexo, AppMasters.glPrefijo, true, true, true, false);
            #endregion
            #region Tercero Relacionado
            this._bc.InitMasterUC(this.masterTerceroTerceroRel, AppMasters.coTercero, true, true, true, false);
            #endregion
        }

        /// <summary>
        /// Cargar los datos para un página específica
        /// </summary>
        /// <param name="page"></param>
        private void LoadPageData()
        {
            try
            {
                if (this._firstTime)
                {
                    DTO_glGarantiaControl filter = new DTO_glGarantiaControl();
                    filter.TerceroID.Value = this.masterTerceroGar.Value;
                    filter.DocumentoID.Value = this.masterDocumentoGar.ValidID ? Convert.ToInt32(this.masterDocumentoGar.Value) : filter.DocumentoID.Value;
                    filter.ActivoInd.Value = this.chkActivo.Checked;
                    this.detalle = this._bc.AdministrationModel.glGarantiaControl_GetByParameter(filter, this.masterPrefijoGar.Value, null, 1);

                    this.gcGarantia.DataSource = this.detalle;
                    this.gvGarantia.RefreshData();
                }
                //Garantias
                else if (this.tcTerceros.SelectedTabPage == this.tpGarantias)
                {
                    if (this.ValidateHeader())
                    {
                        DTO_glGarantiaControl filter = new DTO_glGarantiaControl();
                        filter.TerceroID.Value = this.masterTerceroGar.Value;
                        filter.DocumentoID.Value = this.masterDocumentoGar.ValidID ? Convert.ToInt32(this.masterDocumentoGar.Value) : filter.DocumentoID.Value;
                        filter.ActivoInd.Value = this.chkActivo.Checked;
                        this.detalle = this._bc.AdministrationModel.glGarantiaControl_GetByParameter(filter, this.masterPrefijoGar.Value, null, Convert.ToByte(this.cmbEstado.EditValue));

                        this.gcGarantia.DataSource = this.detalle;
                        this.gvGarantia.RefreshData();
                    }
                }
                //Notas
                else if (this.tcTerceros.SelectedTabPage == this.tpAnexos)
                {
                    long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.glDocumentoAnexo, null, null);
                    var tmp = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glDocumentoAnexo, count, 1, null, null).ToList();
                    this.docAnexoList = tmp.Cast<DTO_glDocumentoAnexo>().ToList();

                    this.gcAnexos.DataSource = this.docAnexoList;
                    this.gvAnexos.RefreshData();
                }
                //Historial
                else if (this.tcTerceros.SelectedTabPage == this.tpTerceroRelacion)
                {
                    DTO_glConsulta query = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();

                    if (this.masterTerceroTerceroRel.ValidID)
                    {
                        filtros.Add(new DTO_glConsultaFiltro()
                                   {
                                       CampoFisico = "TerceroID",
                                       OperadorFiltro = OperadorFiltro.Igual,
                                       OperadorSentencia = OperadorSentencia.And,
                                       ValorFiltro = this.masterTerceroTerceroRel.Value
                                   }); 
                    }
                    query.Filtros = filtros;
                    long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.glTerceroReferencia,query, null);
                    var tmp = this._bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.glTerceroReferencia, count, 1, query, null).ToList();
                    List<DTO_glTerceroReferencia> terceroRefList = tmp.Cast<DTO_glTerceroReferencia>().ToList();

                    this.gcTerceroRelacion.DataSource = terceroRefList;
                    this.gvTerceroRelacion.RefreshData();
                }
            }
            catch (Exception)
            {
                ;
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
                GarantiaID.Width = 50;
                GarantiaID.Visible = true;
                GarantiaID.OptionsColumn.AllowEdit = false;
                this.gvGarantia.Columns.Add(GarantiaID);

                //GarantiaDesc
                GridColumn GarantiaDesc = new GridColumn();
                GarantiaDesc.FieldName = this._unboundPrefix + "GarantiaDesc";
                GarantiaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_GarantiaDesc");
                GarantiaDesc.UnboundType = UnboundColumnType.String;
                GarantiaDesc.VisibleIndex = 2;
                GarantiaDesc.Width = 60;
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
                VerDoc.ColumnEdit = this.linkVer;
                this.gvGarantia.Columns.Add(VerDoc);
                #endregion
                #region Anexos
                //DescriptivoNota
                GridColumn DescriptivoAnexo = new GridColumn();
                DescriptivoAnexo.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoAnexo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_DescriptivoAnexo");
                DescriptivoAnexo.UnboundType = UnboundColumnType.String;
                DescriptivoAnexo.VisibleIndex = 1;
                DescriptivoAnexo.Width = 120;
                DescriptivoAnexo.Visible = true;
                DescriptivoAnexo.OptionsColumn.AllowEdit = false;
                this.gvAnexos.Columns.Add(DescriptivoAnexo);

                //VerDocAnexo
                GridColumn VerDocAnexo = new GridColumn();
                VerDocAnexo.FieldName = this._unboundPrefix + "VerDoc";
                VerDocAnexo.OptionsColumn.ShowCaption = false;
                VerDocAnexo.UnboundType = UnboundColumnType.String;
                VerDocAnexo.Width = 50;
                VerDocAnexo.VisibleIndex = 2;
                VerDocAnexo.Visible = true;
                VerDocAnexo.ColumnEdit = this.linkVer;
                this.gvAnexos.Columns.Add(VerDocAnexo);

                #endregion
                #region TerceroRelacionado
                //Nombre
                GridColumn Nombre = new GridColumn();
                Nombre.FieldName = this._unboundPrefix + "Nombre";
                Nombre.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                Nombre.UnboundType = UnboundColumnType.String;
                Nombre.VisibleIndex = 1;
                Nombre.Width = 80;
                Nombre.Visible = true;
                Nombre.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Nombre);

                //TipoReferencia
                GridColumn TipoReferencia = new GridColumn();
                TipoReferencia.FieldName = this._unboundPrefix + "TipoReferencia";
                TipoReferencia.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_TipoReferencia");
                TipoReferencia.UnboundType = UnboundColumnType.Integer;
                TipoReferencia.VisibleIndex = 2;
                TipoReferencia.Width = 40;
                TipoReferencia.Visible = true;
                TipoReferencia.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(TipoReferencia);

                //Relacion
                GridColumn Relacion = new GridColumn();
                Relacion.FieldName = this._unboundPrefix + "Relacion";
                Relacion.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Relacion");
                Relacion.UnboundType = UnboundColumnType.String;
                Relacion.VisibleIndex = 3;
                Relacion.Width = 60;
                Relacion.Visible = true;
                Relacion.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Relacion);

                //Telefono
                GridColumn Telefono = new GridColumn();
                Telefono.FieldName = this._unboundPrefix + "Telefono";
                Telefono.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Telefono");
                Telefono.UnboundType = UnboundColumnType.String;
                Telefono.VisibleIndex = 4;
                Telefono.Width = 50;
                Telefono.Visible = true;
                Telefono.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Telefono);

                //Direccion
                GridColumn Direccion = new GridColumn();
                Direccion.FieldName = this._unboundPrefix + "Direccion";
                Direccion.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Direccion");
                Direccion.UnboundType = UnboundColumnType.String;
                Direccion.VisibleIndex = 5;
                Direccion.Width = 60;
                Direccion.Visible = true;
                Direccion.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Direccion);

                //Ciudad
                GridColumn Ciudad = new GridColumn();
                Ciudad.FieldName = this._unboundPrefix + "Ciudad";
                Ciudad.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Ciudad");
                Ciudad.UnboundType = UnboundColumnType.String;
                Ciudad.VisibleIndex = 6;
                Ciudad.Width = 60;
                Ciudad.Visible = true;
                Ciudad.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Ciudad);

                //Correo
                GridColumn Correo = new GridColumn();
                Correo.FieldName = this._unboundPrefix + "Correo";
                Correo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Correo");
                Correo.UnboundType = UnboundColumnType.String;
                Correo.VisibleIndex = 7;
                Correo.Width = 80;
                Correo.Visible = true;
                Correo.OptionsColumn.AllowEdit = false;
                this.gvTerceroRelacion.Columns.Add(Correo);

                #endregion
                this.gvGarantia.OptionsView.ColumnAutoWidth = true;
                this.gvAnexos.OptionsView.ColumnAutoWidth = true;
                this.gvTerceroRelacion.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TerceroDataForm.cs", "AddGridCols"));
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
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
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
                int fila = 0;
                if (this.tcTerceros.SelectedTabPage == this.tpGarantias)
                {
                    fila = this.gvGarantia.FocusedRowHandle;
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
                else
                {
                    fila = this.gvAnexos.FocusedRowHandle;
                    DTO_glDocumentoAnexo anexo = this.docAnexoList[this.gvAnexos.FocusedRowHandle];
                    DTO_glDocAnexoControl ac = this._bc.AdministrationModel.glDocAnexoControl_GetAnexosByReplica(anexo.ReplicaID.Value.Value);
                    string fileFormat = this._bc.GetControlValue(AppControl.NombreAnexoDocumento);
                    string fileName = ModulesPrefix.gl.ToString() + "&" + ac.ArchivoNombre.Value;
                    string url = _bc.UrlDocumentFile(TipoArchivo.AnexosDocumentos, null, null, fileName);

                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo);
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TerceroDataForm.cs", "linkVer_Click"));
            }

        }

        /// <summary>
        /// Se realiza al salir del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
            try
            {
                if (master.ValidID)
                {
                    DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, master.Value, true);

                    if (this.tcTerceros.SelectedTabPage == this.tpGarantias)
                    {
                        this.txtTelefonoGar.Text = tercero.Tel1.Value;
                        this.txtDireccionGar.Text = tercero.Direccion.Value;
                        this.txtCorreoGar.Text = tercero.CECorporativo.Value;
                    }
                    else if (this.tcTerceros.SelectedTabPage == this.tpAnexos)
                    {
                        this.txtTelefonoAnexos.Text = tercero.Tel1.Value;
                        this.txtDireccionAnexos.Text = tercero.Direccion.Value;
                        this.txtCorreoAnexos.Text = tercero.CECorporativo.Value;
                    }
                    else if (this.tcTerceros.SelectedTabPage == this.tpTerceroRelacion)
                    {
                        this.txtTelefonoTerceroRel.Text = tercero.Tel1.Value;
                        this.txtDireccionTerceroRel.Text = tercero.Direccion.Value;
                        this.txtCorreoTerceroRel.Text = tercero.CECorporativo.Value;
                    }
                }
                else
                {
                    if (this.tcTerceros.SelectedTabPage == this.tpGarantias)
                    {
                        this.txtTelefonoGar.Text = string.Empty;
                        this.txtDireccionGar.Text = string.Empty;
                        this.txtCorreoGar.Text = string.Empty;
                    }
                    else if (this.tcTerceros.SelectedTabPage == this.tpAnexos)
                    {
                        this.txtTelefonoAnexos.Text = string.Empty;
                        this.txtDireccionAnexos.Text = string.Empty;
                        this.txtCorreoAnexos.Text = string.Empty;
                    }
                    else if (this.tcTerceros.SelectedTabPage == this.tpTerceroRelacion)
                    {
                        this.txtTelefonoTerceroRel.Text = string.Empty;
                        this.txtDireccionTerceroRel.Text = string.Empty;
                        this.txtCorreoTerceroRel.Text = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        #endregion

        #region Eventos grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gv_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
                    e.Value = String.Empty;
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
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gv_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
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

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.detalle = new List<DTO_glGarantiaControl>();

                this.gcGarantia.DataSource = this.detalle;
                this.gvGarantia.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TerceroDataForm.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryEstadoEjecucion.cs", "TBSearch"));
            }
        }

        #endregion

    }
}
