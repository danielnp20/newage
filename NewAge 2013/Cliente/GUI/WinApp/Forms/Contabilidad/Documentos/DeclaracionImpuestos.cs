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
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Furmulario de declaracion de impuestos (IVA y Retenciones)
    /// </summary>
    public partial class DeclaracionImpuestos : FormWithToolbar
    {
        #region Variables
        BaseController _bc = BaseController.GetInstance();
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string unboundPrefix = "Unbound_";
        //Info de grilla
        private int currentRow;
        private List<DTO_DeclaracionImpuesto> data;
        //Recursos
        private string rsxAprobado;
        private string rsxSinAprobar;
        #endregion

        public DeclaracionImpuestos()
        {
            try
            {
                this.InitializeComponent();

                this._documentID = AppDocuments.DeclaracionImpuestos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.co;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Inicia los controles de usuario maestras
                _bc.InitPeriodUC(this.dtPeriod, 0);

                //Carga las variables iniciales
                this.rsxAprobado = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                this.rsxSinAprobar = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateSinAprobar);

                this.AddGridCols();
                DateTime now = DateTime.Now;
                this.dtPeriod.DateTime = new DateTime(now.Year, now.Month, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "DeclaracionImpuestos"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            //Seleccionar
            GridColumn select = new GridColumn();
            select.FieldName = this.unboundPrefix + "Seleccionar";
            select.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Seleccionar");
            select.UnboundType = UnboundColumnType.Boolean;
            select.VisibleIndex = 0;
            select.Width = 15;
            this.gvDeclaraciones.Columns.Add(select);

            //Declaracion
            GridColumn declaracion = new GridColumn();
            declaracion.FieldName = this.unboundPrefix + "ImpuestoDeclID";
            declaracion.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Declaracion");
            declaracion.UnboundType = UnboundColumnType.String;
            declaracion.VisibleIndex = 1;
            declaracion.Width = 60;
            declaracion.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(declaracion);

            //Descripcion
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Descriptivo";
            desc.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 2;
            desc.Width = 130;
            desc.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(desc);

            //Año
            GridColumn year = new GridColumn();
            year.FieldName = this.unboundPrefix + "AñoFiscal";
            year.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_AñoFiscal");
            year.UnboundType = UnboundColumnType.String;
            year.VisibleIndex = 3;
            year.Width = 35;
            year.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(year);

            //Periodo
            GridColumn period = new GridColumn();
            period.FieldName = this.unboundPrefix + "Periodo";
            period.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Periodo");
            period.UnboundType = UnboundColumnType.DateTime;
            period.VisibleIndex = 4;
            period.Width = 50;
            period.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(period);

            //Fecha
            GridColumn fecha = new GridColumn();
            fecha.FieldName = this.unboundPrefix + "Fecha";
            fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Fecha");
            fecha.UnboundType = UnboundColumnType.DateTime;
            fecha.VisibleIndex = 5;
            fecha.Width = 50;
            fecha.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(fecha);

            //Estado
            GridColumn estado = new GridColumn();
            estado.FieldName = this.unboundPrefix + "EstadoRsx";
            estado.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
            estado.UnboundType = UnboundColumnType.String;
            estado.VisibleIndex = 6;
            estado.Width = 60;
            estado.OptionsColumn.AllowEdit = false;
            this.gvDeclaraciones.Columns.Add(estado);
        }

        /// <summary>
        /// Actualiza la informacion de la grilla
        /// </summary>
        private void LoadGridData()
        {
            try
            {
                this.currentRow = -1;
                
                data = _bc.AdministrationModel.DeclaracionesImpuestos_Get(this.dtPeriod.DateTime);
                foreach (DTO_DeclaracionImpuesto imp in data)
                {
                    //Carga el recurso del estado
                    if (!string.IsNullOrEmpty(imp.EstadoRsx))
                        imp.EstadoRsx = imp.EstadoRsx == EstadoDocControl.Aprobado.ToString() ? rsxAprobado : rsxSinAprobar;
                }

                this.gcDeclaraciones.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "LoadGridData"));
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

                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                FormProvider.Master.itemUpdate.Visible = true;

                FormProvider.Master.itemSearch.Enabled = true;
                FormProvider.Master.itemUpdate.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Form

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void dtPeriod_EditValueChanged()
        {
            try
            {
                this.LoadGridData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "dtPeriod_Leave"));
            }
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDeclaraciones_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                #region Trae datos
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
                #endregion
            }
            if (e.IsSetData)
            {
                #region Asigna datos
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
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDeclaraciones_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Seleccionar")
            {
                GridColumn col = this.gvDeclaraciones.Columns[this.unboundPrefix + "Seleccionar"];
                int index = this.gvDeclaraciones.FocusedRowHandle;

                for (int i = 0; i < this.data.Count; ++i)
                {
                    if (i != index)
                        this.data[i].Seleccionar.Value = false;
                }

                this.currentRow = index;
                this.gvDeclaraciones.RefreshData();
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton de busqueda
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.gvDeclaraciones.PostEditor();
                if (this.currentRow != -1)
                {
                    DTO_DeclaracionImpuesto imp = this.data[this.currentRow];
                    EstadoDocControl est = EstadoDocControl.Cerrado;
                    if (imp.Estado != null && imp.Estado.Value.HasValue)
                        est = (EstadoDocControl)Enum.Parse(typeof(EstadoDocControl), imp.Estado.Value.Value.ToString());

                    ProcesarDeclaracion procFrm = new ProcesarDeclaracion(this, imp.ImpuestoDeclID.Value, imp.PeriodoCalendario.Value.Value, 
                        imp.AñoFiscal.Value.Value, imp.PeriodoConsulta.Value.Value, imp.NumeroDoc.Value, est);
                    procFrm.ShowDialog();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoSearchCriteria));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para actualizar la información
        /// </summary>
        public override void TBUpdate()
        {
            this.LoadGridData();
        }

        #endregion
    }
}
