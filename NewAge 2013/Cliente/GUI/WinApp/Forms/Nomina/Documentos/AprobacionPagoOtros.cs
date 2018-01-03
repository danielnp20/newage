using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using System.Threading;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AprobacionPagoOtros : DocumentNominaAprobacionForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private decimal _totalNomina = 0;
        private decimal _totalPagados = 0;
        private decimal _totalAPagar = 0;
        private List<DTO_NominaPlanillaContabilizacion> _liquidaciones = null;
        private bool IsFirstTime = false;
        List<int> documents = new List<int>();

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            var result = this._bc.AdministrationModel.Nomina_AprobarPagosTerceros(this.Liquidaciones);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();
            }

            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
            this.LoadData();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {           
            TablesResources.GetTableResources(this.editLook, AppMasters.noEmpleado, "TipoCuenta", DictionaryTables.TipoCuenta);
            this.editLook.EditValueChanged += new EventHandler(editLook_EditValueChanged);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);
        }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected override void LookUpDocumentosDataSource()
        {
            DTO_glDocumento dtoDoc = null;
            this.documentos = new Dictionary<string, string>();
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoOtrosAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            this.lookUpDocumentos.Enabled = false;
            base.LookUpDocumentosDataSource();
        
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            FormProvider.Master.itemPrint.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            this.txtTotalNomina.Text = string.Empty;
          
            this._totalAPagar = 0;
            this._totalNomina = 0;
            this._totalPagados = 0;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData()
        {
            if (this.IsFirstTime)
            {
                this.Liquidaciones = this._bc.AdministrationModel.noPlanillaAportesDeta_GetValoreXTercero(true);
                foreach (var liq in this.Liquidaciones)
                {
                    liq.Total.Value = liq.Valor.Value + liq.Valor2.Value;
                }

                if (this.Liquidaciones != null || this.Liquidaciones.Count > 0)
                {
                    this._totalNomina = this.Liquidaciones.Sum(x => x.Total.Value.Value);
                    this.txtTotalNomina.EditValue = this._totalNomina;
                }

                this.gcDocument.DataSource = this.Liquidaciones;
            }
            else
                this.IsFirstTime = true;
        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// <param name="oper">Indica si se debe ejecutar una segunda operacion</param>
        /// </summary>
        protected override void RowIndexChanged(int fila, bool oper)
        {

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.no;
            this.documentID = AppDocuments.PagoOtrosAprob;
            
            this.documents.Add(AppDocuments.Nomina);
            this.documents.Add(AppDocuments.Vacaciones);
            this.documents.Add(AppDocuments.Prima);
            this.noGeneraActividad = true;

            base.SetInitParameters();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento           

            this.LookUpDocumentosDataSource();
            this.InitControls();
            this.AddGridCols();
            this.LoadData();
            this.AfterInitialize();
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            base.AddNewRow();
        }

        /// <summary>
        /// Muestra las columnas en la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            base.AddGridCols();
            this.gvDocument.Columns[this.unboundPrefix+"Seleccionar"].OptionsColumn.AllowEdit = false;
            base.chkSeleccionarTodos.Visible = false;
            #region Encabezado

            GridColumn terceroID = new GridColumn();
            terceroID.FieldName = this.unboundPrefix + "TerceroID";
            terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
            terceroID.UnboundType = UnboundColumnType.String;
            terceroID.VisibleIndex = 0;
            terceroID.Width = 100;
            terceroID.Visible = true;
            terceroID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(terceroID);

            GridColumn terceroDesc = new GridColumn();
            terceroDesc.FieldName = this.unboundPrefix + "TerceroDesc";
            terceroDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroDesc");
            terceroDesc.UnboundType = UnboundColumnType.String;
            terceroDesc.VisibleIndex = 0;
            terceroDesc.Width = 200;
            terceroDesc.Visible = true;
            terceroDesc.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(terceroDesc);
           
            GridColumn valorPago = new GridColumn();
            valorPago.FieldName = this.unboundPrefix + "Total";
            valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Total");
            valorPago.UnboundType = UnboundColumnType.Decimal;
            valorPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            valorPago.AppearanceCell.Options.UseTextOptions = true;
            valorPago.VisibleIndex = 0;
            valorPago.Width = 100;
            valorPago.Visible = true;
            valorPago.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(valorPago);

            #endregion
            #region Detalle

            GridColumn EmpleadoID = new GridColumn();
            EmpleadoID.FieldName = this.unboundPrefix + "EmpleadoID";
            EmpleadoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "Cédula");
            EmpleadoID.UnboundType = UnboundColumnType.String;
            EmpleadoID.VisibleIndex = 0;
            EmpleadoID.Width = 100;
            EmpleadoID.Visible = true;
            EmpleadoID.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(EmpleadoID);

            GridColumn EmpleadoDesc = new GridColumn();
            EmpleadoDesc.FieldName = this.unboundPrefix + "EmpleadoDesc";
            EmpleadoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "Nombre");
            EmpleadoDesc.UnboundType = UnboundColumnType.String;
            EmpleadoDesc.VisibleIndex = 1;
            EmpleadoDesc.Width = 200;
            EmpleadoDesc.Visible = true;
            EmpleadoDesc.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(EmpleadoDesc);

            GridColumn valorPagoDet = new GridColumn();
            valorPagoDet.FieldName = this.unboundPrefix + "Total";
            valorPagoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Total");
            valorPagoDet.UnboundType = UnboundColumnType.Decimal;
            valorPagoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            valorPagoDet.AppearanceCell.Options.UseTextOptions = true;
            valorPagoDet.VisibleIndex = 2;
            valorPagoDet.Width = 100;
            valorPagoDet.Visible = true;
            valorPagoDet.OptionsColumn.AllowEdit = true;
            this.gvPreliminar.Columns.Add(valorPagoDet);
            #endregion

        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {           
            this.dtFecha.Enabled = false;           
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = true;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = true;
            FormProvider.Master.itemRevert.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = true;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemImport.Enabled = true;
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Cambia el Documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void lookUpDocumentos_EditValueChanged(object sender, EventArgs e)
        {
            base.lookUpDocumentos_EditValueChanged(sender, e);
            this.LoadDocumentInfo(true);
            this.RefreshDocument();
            this.LoadData();
            this.AfterInitialize();
        }


        protected override void chkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {            
            foreach (var liq in this.Liquidaciones)
                liq.Seleccionar.Value = ((CheckEdit)sender).Checked;

            base.chkSeleccionarTodos_CheckedChanged(sender, e);
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            base.gvDocument_CustomRowCellEdit(sender, e);
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor" || fieldName == "Total")
            {
                e.RepositoryItem = this.editValue;
            }   
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
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
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            base.gvDocument_CellValueChanged(sender, e);
        }
        
        /// <summary>
        /// Cambia valor de los campos chek
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void editChkBox_EditValueChanged(object sender, EventArgs e)
        {
            CheckEdit origin = ((CheckEdit)sender);
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Seleccionar")
            {
                this.Liquidaciones[this.gvDocument.FocusedRowHandle].Seleccionar.Value = origin.Checked;
            }     
        }

        /// <summary>
        /// Evalua combo Tipo de Cuenta en la Grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editLook_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Evalua Maestra Banco en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
                  
        }


        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvPreliminar_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime" || pi.PropertyType.Name == "UDTSQL_datetime")
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
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvPreliminar_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            base.gvDocument_CustomRowCellEdit(sender, e);
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor" || fieldName == "Total")
            {
                e.RepositoryItem = this.editValue;
            }
        }
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para recargar los controles del formulario
        /// </summary>
        public override void TBNew()
        {
            this.RefreshDocument();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }               

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionPagoOtros.cs", "SaveThread"));
            }
        }           

        #endregion      

        #region Propiedades

        /// <summary>
        /// Listado de Empleados para pagos
        /// </summary>
        protected virtual List<DTO_NominaPlanillaContabilizacion> Liquidaciones
        {
            get { return this._liquidaciones; }
            set { this._liquidaciones = value; }
        }

        #endregion
    }
}
