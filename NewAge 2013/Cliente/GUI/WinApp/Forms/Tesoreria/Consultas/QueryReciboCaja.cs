using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Collections.Generic;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryReciboCaja : FormWithToolbar
    {
        #region Variables
        private int _documentID;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private BaseController _bc = BaseController.GetInstance();
        private string _unboundPrefix = "Unbound_";
        private List<DTO_QueryReciboCaja> _data;
        private DTO_QueryReciboCaja _dataItem;
        Dictionary<int, string> _ordenCheque = new Dictionary<int, string>();
        private int _filaDetaOn;
        private int _filaDeta;
        #endregion

        public QueryReciboCaja()
        {
            InitializeComponent();
            this.SetInitParameters();
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
            FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QueryReciboCaja;
            this._frmModule = ModulesPrefix.ts;

            this.AddGridCols();
            this.InitControls();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void InitControls()
        {
            //Inicia los controles Master Find
            this._bc.InitMasterUC(this.masterCaja, AppMasters.tsCaja, true, true, true, false);
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
   
            string periodo = this._bc.GetControlValueByCompany(ModulesPrefix.ts, AppControl.ts_Periodo);
            this.dtFechaIni.DateTime = !string.IsNullOrEmpty(periodo) ? Convert.ToDateTime(periodo) : DateTime.Now;
            this.dtFechaFin.DateTime = this.dtFechaIni.DateTime;

            //Carga el Combo de orden
            this._ordenCheque.Add(1, "Por Nro Recibo");
            this._ordenCheque.Add(2, "Por Tercero");
            this.cmbOrden.Properties.DataSource = this._ordenCheque;
            this.cmbOrden.EditValue = 1;
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 0;
                PrefDoc.Width = 50;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PrefDoc);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this._unboundPrefix + "FechaDoc";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 1;
                fecha.Width = 50;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(fecha);

                //TerceroID
                GridColumn TerceroID = new GridColumn();
                TerceroID.FieldName = this._unboundPrefix + "TerceroID";
                TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_TerceroID");
                TerceroID.UnboundType = UnboundColumnType.String;
                TerceroID.VisibleIndex = 2;
                TerceroID.Width = 70;
                TerceroID.Visible = true;
                TerceroID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(TerceroID);

                //Nombre
                GridColumn nombre = new GridColumn();
                nombre.FieldName = this._unboundPrefix + "Nombre";
                nombre.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Nombre");
                nombre.UnboundType = UnboundColumnType.String;
                nombre.VisibleIndex = 3;
                nombre.Width = 100;
                nombre.Visible = true;
                nombre.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombre);

                //Valor
                GridColumn Valor = new GridColumn();
                Valor.FieldName = this._unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.VisibleIndex = 4;
                Valor.Width = 50;
                Valor.Visible = true;
                Valor.ColumnEdit = this.editValue;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                //Busqueda de documentos
                GridColumn fileDeta = new GridColumn();
                fileDeta.FieldName = this._unboundPrefix + "Documents";
                fileDeta.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_SearchDocument");
                fileDeta.UnboundType = UnboundColumnType.String;
                fileDeta.Width = 50;
                fileDeta.VisibleIndex = 5;
                fileDeta.ColumnEdit = this.LinkEdit;
                fileDeta.AppearanceHeader.Font = new Font("Arial", 6.50F, FontStyle.Bold, GraphicsUnit.Pixel);
                fileDeta.Visible = true;
                this.gvDocument.Columns.Add(fileDeta);
                
                this.gvDetalle.OptionsBehavior.Editable =true;
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "QueryReciboCaja.cs-AddGridCols"));
            }
        }
        #endregion

        #region Eventos MDI
        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);               
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryReciboCaja.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryReciboCaja.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryReciboCaja.cs", "Form_FormClosing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryReciboCaja.cs", "Form_FormClosed"));
            }
        }
        #endregion

        #region Eventos
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvDocument_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {            
            this._filaDeta = e.RowHandle;
            this._dataItem = this._data[this._filaDeta];
        }

        /// <summary>
        /// Asigna el text a un campo de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void gvDocument_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Documents")
                e.DisplayText = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewFile);
        }

        /// <summary>
        /// Evento del linck que carga y llama la pantalla de busquedad de docmentos desde la grilla de arriba
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkEdit_Click(object sender, EventArgs e)
        {
            DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
            DTO_Comprobante comprobante = new DTO_Comprobante();

            ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._dataItem.NumeroDoc.Value.Value);
            comprobante = ctrl != null ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value,ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

            ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
            documentForm.Show();
        }

        #endregion

        #region Evenos Grilla Detalle Interno
       
        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalleCustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            this._filaDetaOn = e.RowHandle;
            //if (this._filaDetaOn < this._data[this._filaDeta].Detalle.Count)
            //    this._dataDetaItem = this._data[this._filaDeta].Detalle[this._filaDetaOn];   
        }
  
        #endregion

        #region Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._data = new List<DTO_QueryReciboCaja>();
                this._dataItem = new DTO_QueryReciboCaja();

                this.gcDocument.DataSource = null;
                this.gcDocument.RefreshDataSource();
                this.masterTercero.Value = string.Empty;
                this.masterCaja.Value = string.Empty;
                this.txtNumReciboCaja.Text = string.Empty;
                this.cmbOrden.EditValue = 1;
                this.masterCaja.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-QueryReciboCaja.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (string.IsNullOrEmpty(this.dtFechaIni.Text) && string.IsNullOrEmpty(this.dtFechaFin.Text))
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_RangoNull);
                    MessageBox.Show(msg);
                    return;
                }
                if (!this.masterCaja.ValidID)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    MessageBox.Show(string.Format(msg,this.masterCaja.CodeRsx));
                    return;
                }
                this._data = this._bc.AdministrationModel.ReciboCaja_GetByParameter(this.masterCaja.Value, this.masterTercero.Value, Convert.ToDateTime(this.dtFechaIni.DateTime), Convert.ToDateTime(this.dtFechaFin.DateTime), txtNumReciboCaja.Text);

                if (this.cmbOrden.EditValue.Equals(1))
                    this._data = this._data.OrderBy(x => x.NumReciboCaja.Value).ToList();
                else
                    this._data = this._data.OrderBy(x => x.TerceroID.Value).ToList();

                this.gcDocument.DataSource = this._data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "QueryReciboCaja.cs-TBSearch"));
                throw;
            }
        } 
        #endregion

    }
}
