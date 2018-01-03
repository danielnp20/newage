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
    public partial class ModalViewDocuments : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected List<DTO_glDocumentoControl> _listDocuments = null;
        protected DTO_glDocumentoControl _docCtrl = null;
        private string unboundPrefix = "Unbound_";
        private int _documentID;
        private byte _showCantOrValue = 0;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalViewDocuments()
        {            
            this.InitializeComponent();            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalViewDocuments(List<DTO_glDocumentoControl> docs, byte showCantOrValue = 0)
        {
            try
            {
                this.InitializeComponent();
                this._showCantOrValue = showCantOrValue;
                this.SetInitParameters();
                FormProvider.LoadResources(this, this._documentID);
                this.LoadData(docs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalViewDocuments.cs", "ModalViewDocuments")); ;
            }
        }      

        #region Funciones Virtuales
     
        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalQueryDocument;
            this._listDocuments = new List<DTO_glDocumentoControl>();
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this.unboundPrefix + "DocumentoID";
                DocumentoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                DocumentoID.UnboundType = UnboundColumnType.String;
                DocumentoID.VisibleIndex = 0;
                DocumentoID.Width = 40;
                DocumentoID.Visible = true;
                DocumentoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoID);

                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocMask");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 1;
                DocumentoPrefijoNro.Width = 70;
                DocumentoPrefijoNro.Visible = true;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocumentoTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoTercero");
                DocumentoTercero.UnboundType = UnboundColumnType.String;
                DocumentoTercero.VisibleIndex = 2;
                DocumentoTercero.Width = 50;
                DocumentoTercero.Visible = true;
                DocumentoTercero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoTercero);

                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descripcion";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 3;
                Descriptivo.Width = 180;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "Fecha";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaDoc.AppearanceCell.Options.UseTextOptions = true;
                FechaDoc.VisibleIndex = 4;
                FechaDoc.Width = 70;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                if (this._showCantOrValue == 0) // Muestra Cantidad
                {
                    GridColumn Cantidad = new GridColumn();
                    Cantidad.FieldName = this.unboundPrefix + "Cantidad";
                    Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cantidad");
                    Cantidad.UnboundType = UnboundColumnType.Decimal;
                    Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    Cantidad.AppearanceCell.Options.UseTextOptions = true;
                    Cantidad.VisibleIndex = 5;
                    Cantidad.Width = 90;
                    Cantidad.Visible = true;
                    Cantidad.OptionsColumn.AllowEdit = false;
                    Cantidad.ColumnEdit = this.editCant2;
                    this.gvDocument.Columns.Add(Cantidad);
                }
                else if(this._showCantOrValue == 1) // Muestra Valor
                {
                    GridColumn Valor = new GridColumn();
                    Valor.FieldName = this.unboundPrefix + "Valor";
                    Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                    Valor.UnboundType = UnboundColumnType.Decimal;
                    Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    Valor.AppearanceCell.Options.UseTextOptions = true;
                    Valor.VisibleIndex = 5;
                    Valor.Width = 90;
                    Valor.Visible = true;
                    Valor.OptionsColumn.AllowEdit = false;
                    Valor.ColumnEdit = this.editSpin;
                    this.gvDocument.Columns.Add(Valor);
                }
                else // Muestra Ambos
                {
                    GridColumn Cantidad = new GridColumn();
                    Cantidad.FieldName = this.unboundPrefix + "Cantidad";
                    Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Cantidad");
                    Cantidad.UnboundType = UnboundColumnType.Decimal;
                    Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    Cantidad.AppearanceCell.Options.UseTextOptions = true;
                    Cantidad.VisibleIndex = 5;
                    Cantidad.Width = 90;
                    Cantidad.Visible = true;
                    Cantidad.OptionsColumn.AllowEdit = false;
                    Cantidad.ColumnEdit = this.editCant2;
                    this.gvDocument.Columns.Add(Cantidad);

                    GridColumn Valor = new GridColumn();
                    Valor.FieldName = this.unboundPrefix + "Valor";
                    Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                    Valor.UnboundType = UnboundColumnType.Decimal;
                    Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    Valor.AppearanceCell.Options.UseTextOptions = true;
                    Valor.VisibleIndex = 6;
                    Valor.Width = 90;
                    Valor.Visible = true;
                    Valor.OptionsColumn.AllowEdit = false;
                    Valor.ColumnEdit = this.editSpin;
                    this.gvDocument.Columns.Add(Valor);
                }

                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Observacion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Observacion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 7;
                Descripcion.Width = 100;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                GridColumn Estado = new GridColumn();
                Estado.FieldName = this.unboundPrefix + "Estado";
                Estado.Caption = _bc.GetResource(LanguageTypes.Forms, "Estado");
                Estado.UnboundType = UnboundColumnType.String;
                Estado.VisibleIndex = 8;
                Estado.Width = 70;
                Estado.Visible = true;
                Estado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Estado);  

                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrl";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.VisibleIndex = 9;
                file.Visible = true;
                file.ColumnEdit = this.editLink;
                this.gvDocument.Columns.Add(file);

                #region Columnas no visibles
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
            
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocuments", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private  void LoadData(List<DTO_glDocumentoControl> docs)
        {
            try
            {
                this._listDocuments = docs;      
                this.gcDocument.DataSource = this._listDocuments;
                this.gcDocument.RefreshDataSource();
                if (this._listDocuments.Count > 0)
                    this._docCtrl = this._listDocuments[0];
                else
                    this._docCtrl = null;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocuments.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentInt(int documentoID, string prefijoID, int docNro)
        {
            try
            {
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(documentoID, prefijoID, docNro);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocuments.cs", "GetDocumentInt"));
                return null;
            }
        }

        /// <summary>
        /// Obtiene un documento externo
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentExt(int documentoID, string terceroID, string docTercero)
        {
            try
            {
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(documentoID, terceroID, docTercero);
                return doc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocuments.cs", "GetDocumentExt"));
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
                    {
                        int fila = this.gvDocument.FocusedRowHandle;

                        DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                        DTO_Comprobante comprobante = new DTO_Comprobante();

                        ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listDocuments[fila].NumeroDoc.Value.Value);
                        comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                        ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                        documentForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
            if (fieldName == "Estado")
            { 
                if (e.Value.ToString().Equals("-1"))
                    e.DisplayText = "Cerrado";
                else if (e.Value.ToString().Equals("0"))
                    e.DisplayText = "Anulado";
                else if (e.Value.ToString().Equals("1"))
                    e.DisplayText = "Sin Aprobar";
                else if (e.Value.ToString().Equals("2"))
                    e.DisplayText = "Para Aprobación";
                else if (e.Value.ToString().Equals("3"))
                    e.DisplayText = "Aprobado";
                else if (e.Value.ToString().Equals("4"))
                    e.DisplayText = "Revertido";
                else if (e.Value.ToString().Equals("5"))
                    e.DisplayText = "Devuelto";
                else if (e.Value.ToString().Equals("6"))
                    e.DisplayText = "Radicado";
                else if (e.Value.ToString().Equals("7"))
                    e.DisplayText = "Revisado";
                else if (e.Value.ToString().Equals("8"))
                    e.DisplayText = "Contabilizado";
            }
              
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._listDocuments[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionSolicitud.cs", "editLink_Click"));
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnAccept_Click(object sender, EventArgs e)
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

        #endregion

    }
}
