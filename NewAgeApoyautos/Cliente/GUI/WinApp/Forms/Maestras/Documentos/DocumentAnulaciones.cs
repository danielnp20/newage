using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class DocumentAnulaciones : DocumentForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que refresca los datos actuales del documento
        /// </summary>
        protected override void RefreshGridMethod()
        {
            this.docs = new List<DTO_glDocumento>();
            this.docCtrls = new List<DTO_glDocumentoControl>();
            this.LoadData();
            this.select = new List<int>();
        }

        #endregion

        #region Variables

        protected BaseController _bc = BaseController.GetInstance();

        protected List<DTO_glDocumento> docs;
        protected List<DTO_glDocumentoControl> docCtrls;
        protected bool filterInd = false;
        private string filterActRsx = string.Empty;
        private string filterDesactRsx = string.Empty;

        #endregion     
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mod"></param>
        public DocumentAnulaciones(string mod = null) : base(mod)
        {
            //  this.InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los controles de la aplicacion
        /// </summary>
        protected virtual void InitControls()
        {
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
            this.gbFilter.Enabled = false;

            this.dtFechaInicial.DateTime = base.dtPeriod.DateTime;
            this.dtFechaFinal.DateTime = new DateTime(base.dtPeriod.DateTime.Year, base.dtPeriod.DateTime.Month, DateTime.DaysInMonth(base.dtPeriod.DateTime.Year, base.dtPeriod.DateTime.Month));

            this.tlSeparatorPanel.RowStyles[0].Height = 50;
            this.tlSeparatorPanel.RowStyles[1].Height = 70;
            this.tlSeparatorPanel.RowStyles[2].Height = 30;

            this.filterActRsx = this._bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_btnFilter");
            this.filterDesactRsx = this._bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_btnFilterDesc");

            //Deshabilita los botones +- de la grilla
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
        }  

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
            this.AddGridCols();
        }    

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime = false)
        {
            try
            {
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                if (this.lkpDocumentos.EditValue != null)
                {
                    ctrl.DocumentoID.Value = Convert.ToInt32(((UDT_BasicID)(this.lkpDocumentos.EditValue)).Value);
                    ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                    if (ctrl.DocumentoID.Value == AppDocuments.RadicacionFactura)
                    {
                        ctrl.DocumentoID.Value = AppDocuments.CausarFacturas;
                        ctrl.Estado.Value = (byte)EstadoDocControl.Radicado;
                    }
                    ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;

                    if (this.filterInd)
                    {
                        ctrl.PrefijoID.Value = this.masterPrefijo.Value;
                        if (!string.IsNullOrEmpty(this.txtDocumentoNro.Text))
                            ctrl.DocumentoNro.Value = Convert.ToInt32(this.txtDocumentoNro.Text);
                        ctrl.TerceroID.Value = this.masterTercero.Value;
                        ctrl.DocumentoTercero.Value = this.txtDocTercero.Text;
                        ctrl.FechaInicial.Value = this.dtFechaInicial.DateTime.Date;
                        ctrl.FechaFinal.Value = this.dtFechaFinal.DateTime.Date;
                    }

                    this.docCtrls = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(ctrl);
                    ctrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    this.docCtrls.AddRange(this._bc.AdministrationModel.glDocumentoControl_GetByParameter(ctrl));
                    if (this.docCtrls != null)
                    {
                        foreach (DTO_glDocumentoControl doc in this.docCtrls)
                        {
                            if (!string.IsNullOrEmpty(doc.TerceroID.Value))
                            {
                                DTO_coTercero tercero = (DTO_coTercero)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, doc.TerceroID.Value, true);
                                doc.TerceroDesc.Value = tercero.Descriptivo.Value;
                            }
                            doc.Comprobante.Value = doc.ComprobanteID.Value +"-"+ doc.ComprobanteIDNro.Value;
                        }
                    }
                    //this.docCtrls = this.docCtrls.OrderBy(x => x.DocumentoNro.Value).ToList();
                    this.gcDocument.DataSource = this.docCtrls;
                    this.gcDocument.RefreshDataSource();
                    for (int i = 0; i < this.gvDocument.DataRowCount; i++)
                        this.gvDocument.SetRowCellValue(i, this.unboundPrefix + "Marca", false); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                base.AddGridCols();

                //NumeroDoc
                GridColumn NumeroDoc = new GridColumn();
                NumeroDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                NumeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_NumeroDoc");
                NumeroDoc.UnboundType = UnboundColumnType.String;
                NumeroDoc.VisibleIndex = 1;
                NumeroDoc.Width = 50;
                NumeroDoc.Visible = true;
                NumeroDoc.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(NumeroDoc);

                //FechaDoc
                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this.unboundPrefix + "FechaDoc";
                FechaDoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 2;
                FechaDoc.Width = 60;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaDoc);

                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 3;
                PrefDoc.Width = 70;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PrefDoc);

                //ComprobanteID
                GridColumn ComprobanteID = new GridColumn();
                ComprobanteID.FieldName = this.unboundPrefix + "Comprobante";
                ComprobanteID.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_ComprobanteID");
                ComprobanteID.UnboundType = UnboundColumnType.String;
                ComprobanteID.VisibleIndex = 4;
                ComprobanteID.Width = 80;
                ComprobanteID.Visible = true;
                ComprobanteID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ComprobanteID);

                //ComprobanteNro
                GridColumn ComprobanteNro = new GridColumn();
                ComprobanteNro.FieldName = this.unboundPrefix + "ComprobanteIDNro";
                ComprobanteNro.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_ComprobanteIDNro");
                ComprobanteNro.UnboundType = UnboundColumnType.String;
                ComprobanteNro.VisibleIndex = 5;
                ComprobanteNro.Width = 50;
                ComprobanteNro.Visible = false;
                ComprobanteNro.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ComprobanteNro);

                //MonedaID
                GridColumn MonedaID = new GridColumn();
                MonedaID.FieldName = this.unboundPrefix + "MonedaID";
                MonedaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_MonedaID");
                MonedaID.UnboundType = UnboundColumnType.String;
                MonedaID.VisibleIndex = 7;
                MonedaID.Width = 60;
                MonedaID.Visible = false;
                MonedaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaID);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this.unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 8;
                TerceroID.Width = 70;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                //Descripcion tercero
                GridColumn terceroDesc = new GridColumn();
                terceroDesc.FieldName = this.unboundPrefix + "TerceroDesc";
                terceroDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_TerceroDesc");
                terceroDesc.UnboundType = UnboundColumnType.String;
                terceroDesc.VisibleIndex = 9;
                terceroDesc.Width = 100;
                terceroDesc.Visible = true;
                terceroDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(terceroDesc);

                //DocumentoTercero
                GridColumn docTercero = new GridColumn();
                docTercero.FieldName = this.unboundPrefix + "DocumentoTercero";
                docTercero.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_DocumentoTercero");
                docTercero.UnboundType = UnboundColumnType.String;
                docTercero.VisibleIndex = 10;
                docTercero.Width = 80;
                docTercero.Visible = true;
                docTercero.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(docTercero);

                //Observacion
                GridColumn Descripcion = new GridColumn();
                Descripcion.FieldName = this.unboundPrefix + "Descripcion";
                Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_Descripcion");
                Descripcion.UnboundType = UnboundColumnType.String;
                Descripcion.VisibleIndex = 11;
                Descripcion.Width = 130;
                Descripcion.Visible = true;
                Descripcion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descripcion);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 12;
                ProyectoID.Width = 60;
                ProyectoID.Visible = false;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                CentroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_CentroCostoID");
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.VisibleIndex = 13;
                CentroCostoID.Width = 60;
                CentroCostoID.Visible = false;
                CentroCostoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CentroCostoID);

                //CentroCostoID
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, AppForms.DocumentReversionForm + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 14;
                valor.Width = 60;
                valor.Visible = true;
                valor.ColumnEdit = this.editSpin;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valor);


                this.gvDocument.OptionsView.ColumnAutoWidth = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentoReversiones.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void AfterInitialize()
        {
            this.InitControls();
        }    

        #endregion

        #region Eventos MDI

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                base.Form_Enter(sender, e);
                //Manejo de Botones de la Barra de Herramientas
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
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
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;

                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Enter"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento para lista los documetnos asociados por glDocumento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void lkpDocumentos_EditValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "lkpDocumentos_EditValueChanged"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Boton de Filtro de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilter_Cheked(object sender, System.EventArgs e)
        {
            try
            {
                this.gbFilter.Enabled = this.btnFilter.Checked;
                this.filterInd = this.btnFilter.Checked;
                if (!this.filterInd)
                {
                    this.btnFilter.Text = this.filterActRsx;
                    this.lkpDocumentos_EditValueChanged(this.lkpDocumentos, e);
                }
                else
                    this.btnFilter.Text = this.filterDesactRsx;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "btnFilter_Cheked"));
            }
        }

        /// <summary>
        /// Boton de Filtro de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "btnFilter_Cheked"));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
                {
                    #region propiedades por reflection
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
                    #endregion
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
                    #region propiedades por reflection
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
                    #endregion
                }
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Marca")
            {
                e.RepositoryItem = this.editChkBox;
            }
            if (fieldName == "NumeroDoc")
            {
                e.RepositoryItem = this.LinkEdit;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operaciones mientras se modifican los valores del campo en la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == this.unboundPrefix + "Marca")
                this.docCtrls[e.RowHandle].Marca.Value = Convert.ToBoolean(e.Value);
        }

        /// <summary>
        /// Link al documento de activos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LinkEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                int fila = this.gvDocument.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.docCtrls[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "LinkEdit_Click"));
            }
        }

        #endregion            

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            base.TBSave();
            this.gvDocument.PostEditor();
            try
            {
                bool select = false;
                foreach (DTO_glDocumentoControl doc in this.docCtrls)
                {
                    if (doc.Marca.Value.Value)
                    {
                        select = true;
                        break;
                    }
                }
                if (select)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAnulaciones.cs", "TBUpdate"));
            }
        }

        #endregion
    }
}