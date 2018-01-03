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
using DevExpress.XtraEditors;
using System.Reflection;
using SentenceTransformer;
using NewAge.DTO.UDT;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{

    /// <summary>
    /// Formulario para buscar documentos
    /// </summary>
    public partial class ModalQueryDocument : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_glDocumentoControl> _listDocuments = null;
        protected List<DTO_glDocumentoControl> _listDocumentSelected = null;
        protected DTO_glDocumentoControl _docCtrl = null;
        protected string unboundPrefix = "Unbound_";
        protected int _documentID;
        private int _pageSize = 0;
        private int pageNum = 0;
        private bool openDocument;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalQueryDocument()
        {
           this.InitializeComponent();
           this.MultipleSelection = false;
           this.SetInitParameters();
           this.InitControls(new List<int>());
           this.btnCopy.Visible = false;
           this.openDocument = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fechaInicial">Fecha para los controles de filtro</param>
        /// <param name="filterDocument">Lista de Documentos a mostrar</param>
        /// <param name="isMulSelection">Si permite seleccionar y retornar varios Documento Control</param>
        public ModalQueryDocument(List<int> filterDocument, bool isMulSelection = false, bool enableCopy = true)
        {
            try
            {
                this.InitializeComponent();
                if (isMulSelection)
                    this.MultipleSelection = isMulSelection;
                this.SetInitParameters();
                this.InitControls(filterDocument);
                this.btnCopy.Enabled = enableCopy;
                this.openDocument = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalQueryDocument.cs", "ModalQueryDocument")); ;
            }
        }      

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        protected virtual void InitControls(List<int> filterDocument)
        {
            #region Controles Maestras
            List<DTO_glConsultaFiltro> consultaDoc = new List<DTO_glConsultaFiltro>();
            if (filterDocument != null)
                foreach (int doc in filterDocument)
                {
                    consultaDoc.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "DocumentoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = doc.ToString(),
                        OperadorSentencia = "OR"
                    });
                }
            this._bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false, consultaDoc);
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCtoCosto, AppMasters.coCentroCosto, true, true, true, false);
            this.masterDocumento.Value = (filterDocument != null && filterDocument.Count > 0)? filterDocument[0].ToString() : string.Empty;
            #endregion

            #region Controles Fecha
            string periodo = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo);
            DateTime fechaInicial = !string.IsNullOrEmpty(periodo) ? DateTime.Parse(periodo) : DateTime.Now;
            this.dtFechaInicial.DateTime = fechaInicial;
            this.dtFechaFinal.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            #endregion

            #region Controles combo

            Dictionary<string, string> dicEstate = new Dictionary<string, string>();
            dicEstate.Add("", this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField));
            dicEstate.Add("-1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateCerrado));
            dicEstate.Add("0", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado));
            dicEstate.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateSinAprobar));
            dicEstate.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateParaAprobacion));
            dicEstate.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado));
            dicEstate.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevertido));
            dicEstate.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateDevuelto));
            dicEstate.Add("6", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRadicado));
            dicEstate.Add("7", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateRevisado));
            dicEstate.Add("8", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateContabilizado));
            this.cmbEstate.EditValue = "";
            this.cmbEstate.Properties.DataSource = dicEstate;

            #endregion

            #region Paginador
            this._bc.Pagging_Init(this.pgGrid, this._pageSize);
            this._bc.Pagging_SetEvent(this.pgGrid, this.pagging_Click);
            this.pgGrid.UpdatePageNumber(this._listDocuments.Count, true, true, false);
            this.toolTipGrid.SetToolTip(this.gcDocument, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ToolTipGrid)); 
            #endregion

            FormProvider.LoadResources(this, this._documentID);
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        protected virtual void SetInitParameters()
        {
            this._documentID = AppForms.ModalQueryDocument;
            this._listDocuments = new List<DTO_glDocumentoControl>();
            this._listDocumentSelected = new List<DTO_glDocumentoControl>();
            this._pageSize = Convert.ToInt32(this._bc.GetControlValue(AppControl.PaginadorMaestra));
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddGridCols()
        {
            try
            {
                if (this.MultipleSelection)
                {
                    GridColumn marca = new GridColumn();
                    marca.FieldName = this.unboundPrefix + "Marca";
                    marca.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Marca");
                    marca.UnboundType = UnboundColumnType.Boolean;
                    marca.VisibleIndex = 0;
                    marca.Width = 25;
                    marca.Visible = true;
                    marca.OptionsColumn.ShowCaption = false;
                    marca.OptionsColumn.AllowEdit = true;
                    marca.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    marca.ColumnEdit = this.editChkBox;
                    this.gvDocument.Columns.Add(marca);
                    this.chkSelectAll.Visible = true;
                }

                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocMask");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 0;
                DocumentoPrefijoNro.Width = 100;
                DocumentoPrefijoNro.Visible = true;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 0;
                TerceroID.Width = 50;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                GridColumn TerceroDesc = new GridColumn();
                TerceroDesc.FieldName = this.unboundPrefix + "TerceroDesc";
                TerceroDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroDesc");
                TerceroDesc.UnboundType = UnboundColumnType.String;
                TerceroDesc.VisibleIndex = 0;
                TerceroDesc.Width = 120;
                TerceroDesc.Visible = true;
                TerceroDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroDesc);

                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 45;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Observacion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 0;
                Descripcion.Width = 130;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "FechaDoc";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 0;
                FechaDoc.Width = 80;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 0;
                Valor.Width = 120;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                Valor.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(Valor);

                #region Columnas no visibles
                GridColumn Estado = new GridColumn();
                Estado.FieldName = this.unboundPrefix + "Estado";
                Estado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
                Estado.UnboundType = UnboundColumnType.Integer;
                Estado.VisibleIndex = 0;
                Estado.Width = 80;
                Estado.Visible = false;
                Estado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Estado);

                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this.unboundPrefix + "DocumentoID";
                DocumentoID.UnboundType = UnboundColumnType.Integer;
                DocumentoID.Visible = false;
                this.gvDocument.Columns.Add(DocumentoID);

                GridColumn PrefijoID = new GridColumn();
                PrefijoID.FieldName = this.unboundPrefix + "PrefijoID";
                PrefijoID.UnboundType = UnboundColumnType.String;
                PrefijoID.Visible = false;
                this.gvDocument.Columns.Add(PrefijoID);

                GridColumn DocumentoNro = new GridColumn();
                DocumentoNro.FieldName = this.unboundPrefix + "DocumentoNro";
                DocumentoNro.UnboundType = UnboundColumnType.String;
                DocumentoNro.Visible = false;
                this.gvDocument.Columns.Add(DocumentoNro);

                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocumentoTercero.UnboundType = UnboundColumnType.String;
                DocumentoTercero.Visible = false;
                this.gvDocument.Columns.Add(DocumentoTercero);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        protected virtual void LoadData()
        {
            try
            {
                //Carga el filtro
                this._docCtrl = new DTO_glDocumentoControl();
                this._docCtrl.DocumentoID.Value = Convert.ToInt32(this.masterDocumento.Value);
                this._docCtrl.PrefijoID.Value = this.masterPrefijo.Value;
                if (!string.IsNullOrEmpty(this.txtDocumentoNro.Text))
                    this._docCtrl.DocumentoNro.Value = Convert.ToInt32(this.txtDocumentoNro.Text);
                if (!string.IsNullOrEmpty(this.cmbEstate.EditValue.ToString()))
                    this._docCtrl.Estado.Value = Convert.ToInt16(this.cmbEstate.EditValue.ToString());
                this._docCtrl.TerceroID.Value = this.masterTercero.Value;
                this._docCtrl.DocumentoTercero.Value = this.txtDocTercero.Text;
                this._docCtrl.ProyectoID.Value = this.masterProyecto.Value;
                this._docCtrl.CentroCostoID.Value = this.masterCtoCosto.Value;
                this._docCtrl.FechaDoc.Value = this.dtFechaInicial.DateTime;
                this._docCtrl.FechaFinal.Value = this.dtFechaFinal.DateTime;
                this._docCtrl.FechaInicial.Value = this.dtFechaInicial.DateTime;

                this._listDocuments = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(this._docCtrl);
                this._listDocuments = this._listDocuments.OrderBy(x => x.PrefDoc.Value).ToList();
                //Asigna valores
                foreach (DTO_glDocumentoControl doc in this._listDocuments)
                {
                    if (doc.DocumentoTipo.Value != (byte)DocumentoTipo.DocInterno)
                        doc.PrefDoc.Value = doc.TerceroID.Value + "-" + doc.DocumentoTercero.Value;
                    DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, doc.ProyectoID.Value, true);
                    if (proy != null)
                        doc.ProyectoDesc.Value = proy.Descriptivo.Value;
                }
                this._listDocuments = this._listDocuments.OrderByDescending(x => x.NumeroDoc.Value).ToList();
                this.pgGrid.UpdatePageNumber(this._listDocuments.Count,false,true,false);
                this.gcDocument.DataSource = this._listDocuments;
                this.gcDocument.RefreshDataSource();
                if (this._listDocuments.Count > 0)
                    this._docCtrl = this._listDocuments[0];
                else
                    this._docCtrl = null;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        protected virtual DTO_glDocumentoControl GetDocumentInt()
        {
            try
            {
                string prefijo = masterPrefijo.Value;
                int docId = Convert.ToInt32(masterDocumento.Value);
                int numDoc = Convert.ToInt32(txtDocumentoNro.Text);
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(docId, prefijo, numDoc);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument.cs", "GetDocumentInt"));
                return null;
            }
        }

        /// <summary>
        /// Obtiene un documento externo
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        protected virtual DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = masterTercero.Value;
                int docId = Convert.ToInt32(this.masterDocumento.Value);
                string docExt = this.txtDocTercero.Text;
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(docId, tercero, docExt);

                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument.cs", "GetDocumentExt"));
                return null;
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Carga la Data en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32")
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
        /// Cierra el modal y retorna el activo seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.openDocument && this._docCtrl != null)
                {
                    DTO_Comprobante comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, this._docCtrl.PeriodoDoc.Value.Value, this._docCtrl.ComprobanteID.Value, this._docCtrl.ComprobanteIDNro.Value.Value, null, null, null);

                    ShowDocumentForm documentForm = new ShowDocumentForm(this._docCtrl, comprobante);
                    documentForm.Show();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument", "gvDocument_DoubleClick"));
            }
        }

        /// <summary>
        /// Selecciona el activo 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (this._listDocuments.Count > 0)
                this._docCtrl = this._listDocuments[e.FocusedRowHandle];
        }

        /// <summary>
        /// Se realiza al digitar una tecla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDocument_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.gvDocument.FocusedRowHandle >= 0)
                        this.Close();
                }
                if (e.KeyCode == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Al cambiar el valor de un campo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //if (Convert.ToBoolean(e.Value))
                //    this._listDocumentSelected.Add(this._listDocuments[e.RowHandle]);
                //else
                //    this._listDocumentSelected.Remove(this._listDocuments[e.RowHandle]);

                //this.gvDocument.RefreshData();
                //this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Se ejecuta al seleccionar registro de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editChek_CheckedChanged(object sender, EventArgs e)
        {
            int index = this.gvDocument.FocusedRowHandle;
            if (((CheckEdit)sender).Checked)
                this._listDocumentSelected.Add(this._listDocuments[index]);
            else
                this._listDocumentSelected.Remove(this._listDocuments[index]);
            this.gvDocument.RefreshData();
            this.gcDocument.RefreshDataSource();
        }
        
        #endregion

        #region Eventos

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroInv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Evento que se ejecuta al darle click al paginador
        /// </summary>
        /// <param name="sender">Objeto que inicia el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void pagging_Click(object sender, System.EventArgs e)
        {
            this.pgGrid.UpdatePageNumber(this._listDocuments.Count, false, false, false);
        }

        /// <summary>
        /// Busca los documentos filtrados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            if (this.masterDocumento.ValidID)
            {
                int docID = Convert.ToInt32(this.masterDocumento.Value);
                bool canAccess = SecurityManager.HasAccess(docID, FormsActions.Get);
                if (!canAccess)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentNoAccess));
                    return;
                }
                this.LoadData();
            }
            else
            {
                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                MessageBox.Show(string.Format(msg, this.masterDocumento.LabelRsx));
            }
        }

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckEdit)sender).Checked)
            {
                this._listDocumentSelected.AddRange(this._listDocuments);
                foreach (var doc in this._listDocuments)
                    doc.Marca.Value = true;
            }
            else
            {
                this._listDocumentSelected.Clear();
                foreach (var doc in this._listDocuments)
                    doc.Marca.Value = false;
            }
            this.gcDocument.DataSource  = this._listDocuments;
            this.gcDocument.RefreshDataSource();
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.openDocument && this._docCtrl != null)
                {
                    this._docCtrl.ComprobanteIDNro.Value = this._docCtrl.ComprobanteIDNro.Value ?? 0;
                    DTO_Comprobante comprobante = this._bc.AdministrationModel.Comprobante_Get(true, false, this._docCtrl.PeriodoDoc.Value.Value, this._docCtrl.ComprobanteID.Value, this._docCtrl.ComprobanteIDNro.Value.Value, null, null, null);

                    ShowDocumentForm documentForm = new ShowDocumentForm(this._docCtrl, comprobante);
                    documentForm.Show();
                }

                this.CopiadoInd = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalQueryDocument", "gvDocument_DoubleClick"));
            }
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.CopiadoInd = true;
            this.Close();
        }

        /// <summary>
        /// Cierra el form
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Documento Control Seleccionado
        /// </summary>
        public DTO_glDocumentoControl DocumentoControl
        {
            get { return this._docCtrl; }
        }

        /// <summary>
        /// Documentos Control Seleccionados
        /// </summary>
        public List<DTO_glDocumentoControl> ListaDocSelected
        {
            get { return this._listDocumentSelected; }
        }

        /// <summary>
        /// Indica si permite selecionar y retornar varios Docs
        /// </summary>
        public Boolean MultipleSelection
        {
            get;
            set;
        }

        /// <summary>
        /// Indica si es Copiado o Consulta
        /// </summary>
        public Boolean CopiadoInd = false;

        #endregion
    }
}
