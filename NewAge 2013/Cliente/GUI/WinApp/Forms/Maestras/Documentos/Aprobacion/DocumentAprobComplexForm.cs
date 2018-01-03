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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class DocumentAprobComplexForm : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void RefreshDataMethod()
        {
            this.currentRow = -1;
            this.LoadDocuments();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        protected int userID = 0;
        //Para manejo de propiedades
        protected string empresaID = string.Empty;
        protected int documentID;
        protected ModulesPrefix frmModule;
        protected string unboundPrefix = "Unbound_";
        protected bool multiMoneda;
        protected object currentDoc = null;
        protected object currentDet = null;
        protected bool allowValidate = true;
        protected bool detailsLoaded = false;
        protected bool detFooterLoaded = false;
        protected int numDetails = 0;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;

        protected int currentRow = -1;
        protected int currentDetRow = -1;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        #endregion

        public DocumentAprobComplexForm()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AfterInitialize();

                this.numDetails = Convert.ToInt32(_bc.GetControlValue(AppControl.PaginadorAprobacionDocumentos));

                //Asigna la lista de columnas
                this.AddDocumentCols();
                this.AddDetailCols();
                this.AddDetFooterCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadDocuments();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion

                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobComplexForm.cs", "DocumentAprobComplexForm"));
            }
        }

        #region  Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() { }
        
        /// <summary>
        /// Carga la información de las grilla de documentos
        /// </summary>
        protected virtual void LoadDocuments() { }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        protected virtual void LoadDetails() { }

        /// <summary>
        /// Carga la información de las grilla del footer del detalle
        /// </summary>
        protected virtual void LoadDetFooter() { }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        protected virtual void AddDetailCols() { }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        protected virtual void AddDetFooterCols() { }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected virtual void AddDocumentCols()
        {
            //Aprobar
            GridColumn aprob = new GridColumn();
            aprob.FieldName = this.unboundPrefix + "Aprobado";
            aprob.Caption = "√";
            aprob.UnboundType = UnboundColumnType.Boolean;
            aprob.VisibleIndex = 0;
            aprob.Width = 15;
            aprob.Visible = true;
            aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
            aprob.AppearanceHeader.ForeColor = Color.Lime;
            aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            aprob.AppearanceHeader.Options.UseTextOptions = true;
            aprob.AppearanceHeader.Options.UseFont = true;
            aprob.AppearanceHeader.Options.UseForeColor = true;
            this.gvDocuments.Columns.Add(aprob);

            //Rechazar
            GridColumn noAprob = new GridColumn();
            noAprob.FieldName = this.unboundPrefix + "Rechazado";
            noAprob.Caption = "X";
            noAprob.UnboundType = UnboundColumnType.Boolean;
            noAprob.VisibleIndex = 1;
            noAprob.Width = 15;
            noAprob.Visible = true;
            noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
            noAprob.AppearanceHeader.ForeColor = Color.Red;
            noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            noAprob.AppearanceHeader.Options.UseTextOptions = true;
            noAprob.AppearanceHeader.Options.UseFont = true;
            noAprob.AppearanceHeader.Options.UseForeColor = true;
            this.gvDocuments.Columns.Add(noAprob);

            //Observacion
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Observacion";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 2;
            desc.Width = 100;
            desc.Visible = true;
            this.gvDocuments.Columns.Add(desc);
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected virtual bool ValidateDocRow(int fila)
        {                
            string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
            bool rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);

            if (rechazado)
            {
                col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();

                if (string.IsNullOrEmpty(desc))
                {
                    string msg = string.Format(rsxEmpty, col.Caption);
                    this.gvDocuments.SetColumnError(col, msg);
                    return false;
                }
            }
            return true;
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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobComplexForm.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobComplexForm.cs", "Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobComplexForm.cs", "Form_FormClosing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobComplexForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            if (dataType == typeof(DTO_prSolicitudFooter))
                dto = ((DTO_prSolicitudFooter)dto).DetalleDocu;
            if (dataType == typeof(DTO_prOrdenCompraFooter))
                dto = ((DTO_prOrdenCompraFooter)dto).DetalleDocu;
            int unboundPrefixLen = this.unboundPrefix.Length;
            if (dataType == typeof(DTO_prSolicitudCargos))
                unboundPrefixLen = "UnboundPref_".Length;


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
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "FileUrl")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewFile);
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                e.RepositoryItem = this.richText1;

            if (fieldName == "FileUrl")
                e.RepositoryItem = this.editLink;
        }
        
        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Observacion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                    e.Allow = false;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;

                this.currentDoc = this.gvDocuments.GetRow(this.currentRow);
                this.LoadDetails();
                this.detailsLoaded = true;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        { }

        #endregion

        #region Eventos grilla de Detalles
        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        { }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentDetRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDetails.RowCount - 1)
                    this.currentDetRow = e.FocusedRowHandle;

                this.currentDet = this.gvDetails.GetRow(this.currentDetRow);
                this.LoadDetFooter();
                this.detFooterLoaded = true;
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDetFooter_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        { }
        #endregion

        #region Eventos Controles

        /// <summary>
        /// Se realiza cuando el usuario elige una tarea 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        //protected virtual void cmbUserTareas_SelectedValueChanged(object sender, EventArgs e) {}

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void editLink_Click(object sender, EventArgs e) { }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocuments.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvDocuments.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.LoadDocuments();
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
        protected virtual void ApproveThread() { }

        #endregion
    }
}
