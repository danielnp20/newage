using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using DevExpress.XtraEditors.Controls;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryForm : FormWithToolbar
    {
        #region Variables
        protected int _documentID;
        protected BaseController _bc = BaseController.GetInstance();
        protected ModulesPrefix _frmModule;
        protected FormTypes _frmType = FormTypes.Query;
        protected string _frmName;
        protected string _unboundPrefix = "Unbound_";
        private DTO_QueryDetailFactura _detalle = null;
        private List<DTO_QueryHeadFactura> _data = null;
        private Dictionary<int, string> _tipoFac = new Dictionary<int, string>();
        private Dictionary<int, string> _tipoConsulta = new Dictionary<int, string>();
        private string _tercero = string.Empty;
        private string _periodoDefult = string.Empty;
        #endregion

        //public QueryForm()
        //{
        //    InitializeComponent();
        //}

        public QueryForm(string mod = null)
        {
            InitializeComponent();
            this.SetInitParameters();
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

            if (!string.IsNullOrWhiteSpace(mod))
                this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

            FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
        }

        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            this.CleanData(true);
        }

        #endregion

        #region Funciones Protected

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        protected virtual void AddGridCols()
        {
            try
            {
                // PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 0;
                PrefDoc.Width = 100;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PrefDoc);

                //Fecha
                GridColumn Fecha = new GridColumn();
                Fecha.FieldName = this._unboundPrefix + "Fecha";
                Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
                Fecha.UnboundType = UnboundColumnType.DateTime;
                Fecha.VisibleIndex = 1;
                Fecha.Width = 70;
                Fecha.Visible = true;
                Fecha.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Fecha);      
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "ConsultaDocumentosFact.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this.AddGridCols();
            this.InitControls();
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        protected virtual void InitControls() { }

        /// <summary>
        /// Funcion que se encarga de limpiar los controles
        /// </summary>
        protected virtual void CleanData(bool cleanAll) { }

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
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_FormClosing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaDocumentosFact.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Permite saber la vista actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gcDocument_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)  {}

        #region Document

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
        /// Evento que se ejecuta al cambiar de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) { }

        #endregion

        #region Detail

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvDetalle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al cambiar de fila
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void gvDetalle_FocusedViewChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) { }


        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetalle_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = e.Column.Caption;
        }

        /// <summary>
        /// Evento que llama la funcionalidad de buscar documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void linkEditViewFile_Click(object sender, EventArgs e) { }

        #endregion

        #endregion

     
    }
}
