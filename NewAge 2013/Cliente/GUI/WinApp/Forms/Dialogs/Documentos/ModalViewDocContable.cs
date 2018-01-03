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
    public partial class ModalViewDocContable : Form
    {
        #region Variables

        protected BaseController _bc = BaseController.GetInstance();
        protected DTO_QueryMvtoAuxiliar filter = null;
        private string unboundPrefix = "Unbound_";
        private int _documentID;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalViewDocContable()
        {            
            this.InitializeComponent();            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ModalViewDocContable(string proyectoID, string lineaPresup)
        {
            try
            {
                this.InitializeComponent();
                this.SetInitParameters();
                FormProvider.LoadResources(this, this._documentID);
                this.filter = new DTO_QueryMvtoAuxiliar();
                this.filter.ProyectoID.Value = proyectoID;
                this.filter.LineaPresupuestoID.Value = lineaPresup;
                this.btnAccept.Visible = false;
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ModalViewDocContable.cs", "ModalViewDocContable")); ;
            }
        }      

        #region Funciones Virtuales
     
        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>             
        private void SetInitParameters()
        {
            this._documentID = AppForms.ModalQueryDocument;
            this.AddGridCols();           
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 0;
                TerceroID.Width = 50;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                GridColumn TerceroDes = new GridColumn();
                TerceroDes.FieldName = this.unboundPrefix + "TerceroDes";
                TerceroDes.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroDesc");
                TerceroDes.UnboundType = UnboundColumnType.String;
                TerceroDes.VisibleIndex = 1;
                TerceroDes.Width = 150;
                TerceroDes.Visible = true;
                TerceroDes.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroDes);

                GridColumn DocumentoID = new GridColumn();
                DocumentoID.FieldName = this.unboundPrefix + "DocumentoID";
                DocumentoID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoID");
                DocumentoID.UnboundType = UnboundColumnType.String;
                DocumentoID.VisibleIndex = 2;
                DocumentoID.Width = 40;
                DocumentoID.Visible = true;
                DocumentoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoID);

                GridColumn DocumentoPrefijoNro = new GridColumn();
                DocumentoPrefijoNro.FieldName = this.unboundPrefix + "PrefDoc";
                DocumentoPrefijoNro.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                DocumentoPrefijoNro.UnboundType = UnboundColumnType.String;
                DocumentoPrefijoNro.VisibleIndex = 3;
                DocumentoPrefijoNro.Width = 70;
                DocumentoPrefijoNro.Visible = false;
                DocumentoPrefijoNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoPrefijoNro);

                GridColumn DocumentoTercero = new GridColumn();
                DocumentoTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                DocumentoTercero.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumentoTercero");
                DocumentoTercero.UnboundType = UnboundColumnType.String;
                DocumentoTercero.VisibleIndex = 4;
                DocumentoTercero.Width = 80;
                DocumentoTercero.Visible = true;
                DocumentoTercero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(DocumentoTercero);

                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
                Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 5;
                Descriptivo.Width = 180;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                GridColumn Comprobante = new GridColumn();
                Comprobante.FieldName = this.unboundPrefix + "Comprobante";
                Comprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Comprobante");
                Comprobante.UnboundType = UnboundColumnType.String;
                Comprobante.VisibleIndex = 6;
                Comprobante.Width = 50;
                Comprobante.Visible = true;
                Comprobante.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Comprobante);

                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "Fecha";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaDoc.AppearanceCell.Options.UseTextOptions = true;
                FechaDoc.VisibleIndex = 7;
                FechaDoc.Width = 70;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                GridColumn ValorVenta = new GridColumn();
                ValorVenta.FieldName = this.unboundPrefix + "ValorVenta";
                ValorVenta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorVenta");
                ValorVenta.UnboundType = UnboundColumnType.Decimal;
                ValorVenta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorVenta.AppearanceCell.Options.UseTextOptions = true;
                ValorVenta.VisibleIndex = 8;
                ValorVenta.Width = 90;
                ValorVenta.Visible = true;
                ValorVenta.OptionsColumn.AllowEdit = false;
                ValorVenta.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(ValorVenta);

                GridColumn ValorCompra = new GridColumn();
                ValorCompra.FieldName = this.unboundPrefix + "ValorCompra";
                ValorCompra.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_ValorCompra");
                ValorCompra.UnboundType = UnboundColumnType.Decimal;
                ValorCompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ValorCompra.AppearanceCell.Options.UseTextOptions = true;
                ValorCompra.VisibleIndex = 9;
                ValorCompra.Width = 90;
                ValorCompra.Visible = true;
                ValorCompra.OptionsColumn.AllowEdit = false;
                ValorCompra.ColumnEdit = this.editSpin;
                this.gvDocument.Columns.Add(ValorCompra);

                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "ViewDoc";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.VisibleIndex = 10;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocContable", "AddGridCols"));
            }
        }

        /// <summary>
        /// Carga la informacion en la grilla
        /// </summary>
        private  void LoadData()
        {
            try
            {
                List<DTO_QueryMvtoAuxiliar> result = new List<DTO_QueryMvtoAuxiliar>();
                List <DTO_QueryMvtoAuxiliar> auxiliares = this._bc.AdministrationModel.Comprobante_GetAuxByParameter(null, null, this.filter);
                foreach (DTO_QueryMvtoAuxiliar r in auxiliares)
                {
                    if (r.DocumentoID.Value == AppDocuments.FacturaVenta || r.DocumentoID.Value == AppDocuments.NotaCredito)
                    {
                        r.ValorVenta.Value = r.vlrMdaLoc.Value;
                        r.DocumentoTercero.Value = r.PrefijoCOM.Value + '-' + r.PrefDoc;
                    }
                    else
                        r.ValorCompra.Value = r.vlrMdaLoc.Value;                        
                    
                }

                this.gcDocument.DataSource = auxiliares;                
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocContable.cs", "LoadData"));
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

                        DTO_QueryMvtoAuxiliar row = (DTO_QueryMvtoAuxiliar)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);

                        if (row != null)
                        {
                            ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(row.NumeroDoc.Value.Value);
                            comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                            ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                            documentForm.Show();
                        }
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

                DTO_QueryMvtoAuxiliar row = (DTO_QueryMvtoAuxiliar)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);

                if (row != null)
                {
                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(row.NumeroDoc.Value.Value);
                    comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalViewDocCont.cs", "editLink_Click"));
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

    }
}
