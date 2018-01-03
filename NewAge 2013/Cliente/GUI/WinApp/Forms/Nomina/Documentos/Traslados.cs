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
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.Data;
using DevExpress.XtraVerticalGrid.Events;
using NewAge.DTO.Resultados;
using System.Threading;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Traslados : DocumentNominaForm
    {
        public Traslados()
        {
          //  InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noTraslado> _traslados = null;
        private int origen;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {  
            //agrega los traslado
            var result = _bc.AdministrationModel.Nomina_AddTraslado(_traslados);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();                
            }

            //Recarga la grilla de traslados
            this._traslados = _bc.AdministrationModel.Nomina_GetTraslados(this.uc_Empleados.empleadoActivo.ID.Value);
            gcDocument.DataSource = _traslados;

            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
            //Recarga la grilla de novedades
            gcDocument.DataSource = this._traslados;
        }

        private delegate void AgregarTraslado();
        private AgregarTraslado agregarTrasladoDelegate;

        private void AgregarTrasladoMethod()
        {
            _traslados = new List<DTO_noTraslado>();

            DTO_noTraslado traslado = new DTO_noTraslado();
            traslado.EmpresaID.Value = this.empresaID;
            traslado.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.ToString();
            traslado.FechaTraslado.Value = this.dtFechaTranslado.DateTime;
            traslado.ContratoNOID.Value = Convert.ToInt16(this.txtContratoNO.Text);
            traslado.OperacionNOID.Value = this.uc_MasterOperacion.Value;
            traslado.Descripcion.Value = this.txtDescripcion.Text;
            traslado.NumeroDoc.Value = 1; //TODO Consultar de donde se saca?
           _traslados.Add(traslado);

            //agrega los traslado
            var result = _bc.AdministrationModel.Nomina_AddTraslado(_traslados);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();               
            }

            //Recarga la grilla de traslados
            this._traslados = _bc.AdministrationModel.Nomina_GetTraslados(this.uc_Empleados.empleadoActivo.ID.Value);
            gcDocument.DataSource = _traslados;

            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            _bc.InitMasterUC(this.uc_MasterOperacion, AppMasters.noOperacion, true, true, false, true);
            _bc.InitMasterUC(this.uc_MasterProyecto, AppMasters.coProyecto, true, true, false, true);
            _bc.InitMasterUC(this.uc_MasterCentroCosto, AppMasters.coCentroCosto, true, true, false, true);
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.FieldsEnabled(true);
        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.uc_MasterOperacion.EnableControl(estado);
            this.txtDescripcion.Enabled = estado;
            this.txtContratoNO.Enabled = estado;
            this.dtFechaTranslado.Enabled = estado;
            this.uc_MasterCentroCosto.EnableControl(estado);
            this.uc_MasterProyecto.EnableControl(estado);
        }

        /// <summary>
        /// lista las traslados del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noTraslado> GetNovedades()
        {
            _traslados = _bc.AdministrationModel.Nomina_GetTraslados(this.uc_Empleados.empleadoActivo.ID.Value);
            return _traslados;
        }

        /// <summary>
        /// Carga el detalle de la novedad
        /// </summary>
        /// <param name="dto"></param>
        private void LoadDetailNovedad(DTO_noTraslado dto)
        {
            this.dtFechaTranslado.DateTime = dto.FechaTraslado.Value.Value;
            this.txtDescripcion.Text = dto.Descripcion.Value;
            this.uc_MasterOperacion.Value = dto.OperacionNOID.Value;
            this.txtContratoNO.Text = dto.ContratoNOID.Value.ToString();
            this.uc_MasterCentroCosto.Value = dto.CentroCostoID.Value;
            this.uc_MasterProyecto.Value = dto.ProyectoID.Value;
       }
      
        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            this.LoadData(true);
        }


        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                if (this.uc_Empleados.empleadoActivo != null)
                {
                    this._traslados = _bc.AdministrationModel.Nomina_GetTraslados(this.uc_Empleados.empleadoActivo.ID.Value);
                    this.gcDocument.DataSource = this._traslados;
                }
            }
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            return 0;
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
            this.documentID = AppDocuments.Traslados;

            base.SetInitParameters();

            this.agregarTrasladoDelegate = new AgregarTraslado(this.AgregarTrasladoMethod);

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;

            this.InitControls();  
            this.AddGridCols();
            this.LoadData(true);
            this.AfterInitialize();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Traslados

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaTraslado";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaTraslado");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 0;
                fecha.Width = 100;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(fecha);

                GridColumn contratoNOID = new GridColumn();
                contratoNOID.FieldName = this.unboundPrefix + "ContratoNOID";
                contratoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ContratoNOID");
                contratoNOID.UnboundType = UnboundColumnType.Integer;
                contratoNOID.VisibleIndex = 0;
                contratoNOID.Width = 100;
                contratoNOID.Visible = true;
                contratoNOID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(contratoNOID);

                GridColumn operacionNOID = new GridColumn();
                operacionNOID.FieldName = this.unboundPrefix + "OperacionNOID";
                operacionNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_OperacionNOID");
                operacionNOID.UnboundType = UnboundColumnType.String;
                operacionNOID.VisibleIndex = 0;
                operacionNOID.Width = 100;
                operacionNOID.Visible = true;
                operacionNOID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(operacionNOID);

                GridColumn proyectoID = new GridColumn();
                proyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                proyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyectoID.UnboundType = UnboundColumnType.String;
                proyectoID.VisibleIndex = 0;
                proyectoID.Width = 100;
                proyectoID.Visible = true;
                proyectoID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(proyectoID);

                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 0;
                centroCostoID.Width = 100;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(centroCostoID);

                GridColumn descripcion = new GridColumn();
                descripcion.FieldName = this.unboundPrefix + "Descripcion";
                descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descripcion");
                descripcion.UnboundType = UnboundColumnType.String;
                descripcion.VisibleIndex = 0;
                descripcion.Width = 200;
                descripcion.Visible = true;
                descripcion.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(descripcion);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Traslados.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            base.AddNewRow();
            DTO_noTraslado traslado = new DTO_noTraslado();

            #region Asigna datos a la fila

            traslado.EmpresaID.Value = this.empresaID;
            traslado.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
            traslado.FechaTraslado.Value = this.dtFechaTranslado.DateTime;
            traslado.NumeroDoc.Value = 0;
            traslado.ContratoNOID.Value = !string.IsNullOrEmpty(this.txtContratoNO.Text) ? Convert.ToInt32(this.txtContratoNO.Text) : 0;
            traslado.Descripcion.Value = this.txtDescripcion.Text;
            traslado.OperacionNOID.Value = this.uc_MasterOperacion.Value;
            traslado.ProyectoID.Value = this.uc_MasterProyecto.Value;
            traslado.CentroCostoID.Value = this.uc_MasterCentroCosto.Value;
          
            #endregion

            if (this._traslados == null)
                this._traslados = new List<DTO_noTraslado>();

            this._traslados.Add(traslado);
            this.gcDocument.DataSource = this._traslados;
            this.gvDocument.RefreshData();
            this.gvDocument.PostEditor();

        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
          
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
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = true;
            FormProvider.Master.itemRevert.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
                FormProvider.Master.itemImport.Enabled = true;
            }
        }

        #endregion

        #region Eventos Header

        private void txtContratoNO_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtContratoNO.Text))
                FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Evento boton Adicionar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.AddNewRow();
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                if (this.gvDocument.ActiveFilterString != string.Empty)
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                else
                {
                    this.deleteOP = false;
                    if (this.isValid)
                    {
                        this.newReg = true;
                        this.AddNewRow();
                    }
                    else
                    {
                        bool isV = this.ValidateRow(this.gvDocument.FocusedRowHandle);
                        if (isV)
                        {
                            this.newReg = true;
                            this.AddNewRow();
                        }
                    }
                }
            }

            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                e.Handled = true;
                //Revisa si desea cargar los temporales
                if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.deleteOP = true;
                    int rowHandle = this.gvDocument.FocusedRowHandle;

                    if (this._traslados.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this._traslados.Remove(this._traslados[rowHandle]);
                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvDocument.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvDocument.FocusedRowHandle = rowHandle - 1;

                        this.gvDocument.RefreshData();
                        this.RowIndexChanged(this.gvDocument.FocusedRowHandle, true);
                    }
                }
            }
        }

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
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "OperacionNOID" || fieldName == "ProyectoID" || fieldName == "CentroCostoID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            FormProvider.Master.itemSave.Enabled = true;

            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "FechaTraslado")
            {
                DateTime val = (DateTime)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].FechaTraslado.Value = val;
            }
            if (fieldName == "OperacionNOID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].OperacionNOID.Value = val;
            }
            if (fieldName == "ProyectoID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].ProyectoID.Value = val;
            }
            if (fieldName == "CentroCostoID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].CentroCostoID.Value = val;
            }
            if (fieldName == "Descripcion")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].Descripcion.Value = val;
            }
            if (fieldName == "ContratoNOID")
            {
                int val = (int)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._traslados[this.gvDocument.FocusedRowHandle].ContratoNOID.Value = val;
            }
            

        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FormProvider.Master.itemSave.Enabled = true;
            if (e.FocusedRowHandle >= 0)
            {
                if (_traslados != null && _traslados.Count > 0)
                {
                    DTO_noTraslado traslado = _traslados[e.FocusedRowHandle];
                    this.LoadDetailNovedad(traslado);
                }
                else
                {
                    this.RefreshDocument();
                }
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

        /// <summary>
        /// Boton para importir datos
        /// </summary>
        public override void TBImport()
        {
            try
            {
                try
                {

                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region Eventos Control Emmpleados

        /// <summary>
        /// Evento que se ejecuta cuando se selecciona un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
            this.gcDocument.DataSource = this.GetNovedades();
            this.gvDocument.RefreshData();
            this.gvDocument.PostEditor();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Traslados.cs", "SaveThread"));
            }
        }

        #endregion            

    }
}
