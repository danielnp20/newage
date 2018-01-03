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
using DevExpress.XtraEditors;
using NewAge.DTO.Attributes;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AumentoSueldo : DocumentNominaForm
    {        
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_AumentoSalarial> _sueldoEmpleados = new List<DTO_AumentoSalarial>();
        
        //Variables con los recursos de las Fks
        public string _empleadoIDRsx = string.Empty;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            //agrega las novedades al sistema
            var result = _bc.AdministrationModel.Nomina_UpdSalarioEmpleado(_sueldoEmpleados);
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
            //Recarga la grilla de novedades
            gcDocument.DataSource = this._sueldoEmpleados;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.uc_Empleados.Init();
            //this.uc_Empleados.CheckRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_CheckRowEmpleado_Click);
            //this.uc_Empleados.CheckAllRowsEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_CheckAllRowsEmpleado_Click);
            this.dtFechaAumento.DateTime = DateTime.Now;
            this.editDate.EditValueChanged += new EventHandler(editDate_EditValueChanged);
        }

    
     
        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this._sueldoEmpleados = null;
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
                gcDocument.DataSource = this._sueldoEmpleados;
            }            
        }

        /// <summary>
        /// Trae el documento id de una maestra de acuerdo al nombre de un campo
        /// </summary>
        /// <param name="colName">Nombre del campo</param>
        /// <returns>Retorna el documento id de una maestra</returns>
        protected override int GetMasterDocumentID(string colName)
        {
            //Maestras
            if (colName == this._empleadoIDRsx)
                return AppMasters.noEmpleado;

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
            this.documentID = AppDocuments.AumentoSueldo;

            base.SetInitParameters();

            this.format = _bc.GetImportExportFormat(typeof(DTO_AumentoSalarial), this.documentID);
            this._empleadoIDRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoID");

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;
          
            this.InitControls();
            this.AddGridCols();
            this.LoadData(true);
            this.AfterInitialize();
            this.tlSeparatorPanel.RowStyles[0].Height = 70;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                GridColumn empleadoID = new GridColumn();
                empleadoID.FieldName = this.unboundPrefix + "EmpleadoID";
                empleadoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoID");
                empleadoID.UnboundType = UnboundColumnType.String;
                empleadoID.VisibleIndex = 0;
                empleadoID.Width = 100;
                empleadoID.Visible = true;
                empleadoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(empleadoID);

                GridColumn nombreEmpleado = new GridColumn();
                nombreEmpleado.FieldName = this.unboundPrefix + "NombreEmpleado";
                nombreEmpleado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NombreEmpleado");
                nombreEmpleado.UnboundType = UnboundColumnType.String;
                nombreEmpleado.VisibleIndex = 0;
                nombreEmpleado.Width = 210;
                nombreEmpleado.Visible = true;
                nombreEmpleado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(nombreEmpleado);

                GridColumn fechaAumento = new GridColumn();
                fechaAumento.FieldName = this.unboundPrefix + "FechaAumento";
                fechaAumento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaAumento");
                fechaAumento.UnboundType = UnboundColumnType.DateTime;
                fechaAumento.VisibleIndex = 0;
                fechaAumento.Width = 120;
                fechaAumento.Visible = true;
                fechaAumento.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(fechaAumento);

                GridColumn sueldo = new GridColumn();
                sueldo.FieldName = this.unboundPrefix + "Sueldo";
                sueldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Sueldo");
                sueldo.UnboundType = UnboundColumnType.Decimal;
                sueldo.VisibleIndex = 0;
                sueldo.Width = 150;
                sueldo.Visible = true;
                sueldo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(sueldo);


                GridColumn nuevoSueldo = new GridColumn();
                nuevoSueldo.FieldName = this.unboundPrefix + "NuevoSueldo";
                nuevoSueldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NuevoSueldo");
                nuevoSueldo.UnboundType = UnboundColumnType.Decimal;
                nuevoSueldo.VisibleIndex = 0;
                nuevoSueldo.Width = 150;
                nuevoSueldo.Visible = true;
                nuevoSueldo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(nuevoSueldo);


                GridColumn aumento = new GridColumn();
                aumento.FieldName = this.unboundPrefix + "Aumento";
                aumento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aumento");
                aumento.UnboundType = UnboundColumnType.Decimal;
                aumento.VisibleIndex = 0;
                aumento.Width = 150;
                aumento.Visible = true;
                aumento.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(aumento);

                GridColumn dias = new GridColumn();
                dias.FieldName = this.unboundPrefix + "Dias";
                dias.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias");
                dias.UnboundType = UnboundColumnType.Integer;
                dias.VisibleIndex = 0;
                dias.Width = 70;
                dias.Visible = true;
                dias.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(dias);

                GridColumn ajuste = new GridColumn();
                ajuste.FieldName = this.unboundPrefix + "Ajuste";
                ajuste.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Ajuste");
                ajuste.UnboundType = UnboundColumnType.Decimal;
                ajuste.VisibleIndex = 0;
                ajuste.Width = 150;
                ajuste.Visible = true;
                ajuste.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ajuste);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AumentoSueldo.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            if (this._sueldoEmpleados == null)
                this._sueldoEmpleados = new List<DTO_AumentoSalarial>();

            if (this.uc_Empleados.empleadoActivo.Visible.Value.Value)
            {
                DTO_AumentoSalarial aumentoRow = new DTO_AumentoSalarial();
                aumentoRow.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
                aumentoRow.NombreEmpleado.Value = this.uc_Empleados.empleadoActivo.Descriptivo.Value;
                aumentoRow.Sueldo.Value = this.uc_Empleados.empleadoActivo.Sueldo.Value;
                aumentoRow.NuevoSueldo.Value = this.uc_Empleados.empleadoActivo.Sueldo.Value;
                aumentoRow.Ajuste.Value = 0;
                aumentoRow.Dias.Value = 0;
                aumentoRow.Aumento.Value = 0;
                aumentoRow.FechaAumento.Value = this.dtFechaAumento.DateTime;
                this._sueldoEmpleados.Add(aumentoRow);
            }
            else
            {
                DTO_AumentoSalarial deleteRow = new DTO_AumentoSalarial();
                deleteRow = this._sueldoEmpleados.Where(x => x.EmpleadoID.Value == this.uc_Empleados.empleadoActivo.ID.Value).FirstOrDefault();
                if (deleteRow != null)
                    this._sueldoEmpleados.Remove(deleteRow);
            }

            this.gcDocument.DataSource = null;
            this.gcDocument.DataSource = this._sueldoEmpleados;
            this.gvDocument.PostEditor();
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.no_Periodo);
            this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
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
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        #endregion

        #region Eventos Header

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

                    if (this._sueldoEmpleados.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this._sueldoEmpleados.Remove(this._sueldoEmpleados[rowHandle]);
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
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Sueldo" || fieldName == "NuevoSueldo" || fieldName == "Aumento" || fieldName == "Ajuste")
            {
                e.RepositoryItem = this.editValue;
            }
            if (fieldName == "FechaAumento")
            {
                e.RepositoryItem = this.editDate;
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
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "NuevoSueldo")
            {
                //Calculos Sueldo
                decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Aumento.Value =  val - this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Sueldo.Value;
                
                if (this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].FechaAumento.Value.Value < this.dtPeriod.DateTime)
                { 
                    TimeSpan timer = this.dtPeriod.DateTime.Subtract(this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].FechaAumento.Value.Value);
                    this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Dias.Value = timer.Days;
                    this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Ajuste.Value = (this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Aumento.Value / 30) * this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Dias.Value;
                }
            }
        }

        /// <summary>
        /// Actualiza  la fecha de Aumento en la grilla y la informacion asociada a la misma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editDate_EditValueChanged(object sender, EventArgs e)
        {
            DateEdit fechaAumento = (DateEdit)sender;
            this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].FechaAumento.Value = fechaAumento.DateTime;
            if (this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].FechaAumento.Value.Value < this.dtPeriod.DateTime)
            {
                TimeSpan timer = this.dtPeriod.DateTime.Subtract(this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].FechaAumento.Value.Value);
                this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Dias.Value = timer.Days;
                this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Ajuste.Value = (this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Aumento.Value / 30) * this._sueldoEmpleados[this.gvDocument.FocusedRowHandle].Dias.Value;
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
        /// Genera la plantilla de excel para el documento
        /// </summary>
        public override void TBGenerateTemplate()
        {
            base.TBGenerateTemplate();
        }

        /// <summary>
        /// Importa informacion de Aumento Salarial a la grilla
        /// </summary>
        public override void TBImport()
        {            
            base.TBImport();
        }
                
        #endregion

        #region Eventos Control Empleados

        /// <summary>
        /// Evento que carga todos los empleados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_CheckAllRowsEmpleado_Click(object sender, EventArgs e)
        {
            CheckBox activa = (CheckBox)sender;

            if (this._sueldoEmpleados == null)
                this._sueldoEmpleados = new List<DTO_AumentoSalarial>();

            if (activa.Checked)
            {
                foreach (var _empleado in this.uc_Empleados._empleados)
                {
                    DTO_AumentoSalarial aumentoRow = new DTO_AumentoSalarial();
                    aumentoRow.EmpleadoID.Value = _empleado.ID.Value;
                    aumentoRow.NombreEmpleado.Value = _empleado.Descriptivo.Value;
                    aumentoRow.Sueldo.Value = ((DTO_noEmpleado)_empleado).Sueldo.Value;
                    aumentoRow.NuevoSueldo.Value = ((DTO_noEmpleado)_empleado).Sueldo.Value;
                    aumentoRow.Ajuste.Value = 0;
                    aumentoRow.Dias.Value = 0;
                    aumentoRow.Aumento.Value = 0;
                    aumentoRow.FechaAumento.Value = this.dtFechaAumento.DateTime;
                    this._sueldoEmpleados.Add(aumentoRow);
                }
            }
            else
            {
                this._sueldoEmpleados = null;
            }

            this.gcDocument.DataSource = null;
            this.gcDocument.DataSource = this._sueldoEmpleados;
            this.gvDocument.PostEditor();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se activa o desactiva un row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_CheckRowEmpleado_Click(object sender, EventArgs e)
        {
            CheckEdit activa = (CheckEdit)sender;

            if (this._sueldoEmpleados == null)
                this._sueldoEmpleados = new List<DTO_AumentoSalarial>();

            if (activa.Checked)
            {
                DTO_AumentoSalarial aumentoRow = new DTO_AumentoSalarial();
                aumentoRow.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
                aumentoRow.NombreEmpleado.Value = this.uc_Empleados.empleadoActivo.Descriptivo.Value;
                aumentoRow.Sueldo.Value = this.uc_Empleados.empleadoActivo.Sueldo.Value;
                aumentoRow.NuevoSueldo.Value = this.uc_Empleados.empleadoActivo.Sueldo.Value;
                aumentoRow.Ajuste.Value = 0;
                aumentoRow.Dias.Value = 0;
                aumentoRow.Aumento.Value = 0;
                aumentoRow.FechaAumento.Value = this.dtFechaAumento.DateTime;
                this._sueldoEmpleados.Add(aumentoRow);
            }
            else
            {
                DTO_AumentoSalarial deleteRow = new DTO_AumentoSalarial();
                deleteRow = this._sueldoEmpleados.Where(x => x.EmpleadoID.Value == this.uc_Empleados.empleadoActivo.ID.Value).FirstOrDefault();
                if (deleteRow != null)
                    this._sueldoEmpleados.Remove(deleteRow);
            }

            this.gcDocument.DataSource = null;
            this.gcDocument.DataSource = this._sueldoEmpleados;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AumentoSueldo.cs", "SaveThread"));
            }
        }

        /// <summary>
        /// Importa datos desde Excel
        /// </summary>
        public override void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    bool sendToServer = true;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, List<Tuple<string, bool>>> fks = new Dictionary<string, List<Tuple<string, bool>>>();
                    List<string> fkNames = new List<string>();
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                    string msgInvalidFormat = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgImportInvalidLength = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_InvalidLength);
                    string msgFkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                    string msgNoCopyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    string msgFkHierarchyFather = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Import_NotHierarchyFather);
                    string msgCero = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField);
                    string msgPositive = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue);
                    string msgCtaCargoProy = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_CtaCargoCosto);
                    string msgCtaPeriodClosed = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_CtaPeriodClosed);
                    //Popiedades de un comprobante
                    DTO_AumentoSalarial det = new DTO_AumentoSalarial();
                    bool createDTO = true;
                    bool validList = true;
                    #endregion
                    #region Llena las listas de las columnas y FKs
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    PropertyInfo[] pis = typeof(DTO_AumentoSalarial).GetProperties();

                    //Recorre el objeto y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pis)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_" + pi.Name);

                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    //Fks
                    fks.Add(_empleadoIDRsx, new List<Tuple<string, bool>>());

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                        }
                        //Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (se revienta si el numero de columnas al importar es menor al necesario)
                            if (line.Length < colNames.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];

                                    //Revisa la info de las FKs
                                    if (!string.IsNullOrWhiteSpace(line[colIndex]))
                                    {
                                        if (colRsx == _empleadoIDRsx)
                                        {
                                            colVals[colRsx] = line[colIndex].ToUpper();

                                            Tuple<string, bool> tupInvalid = new Tuple<string, bool>(line[colIndex].Trim(), false);

                                            if (fks[colRsx].Contains(tupInvalid))
                                            {
                                                DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                rdF.Field = colRsx;
                                                rdF.Message = string.Format(msgFkNotFound, line[colIndex].Trim());
                                                rd.DetailsFields.Add(rdF);

                                                createDTO = false;
                                            }
                                            else
                                            {
                                                int docId = this.GetMasterDocumentID(colRsx);

                                                bool isInt = docId == AppMasters.glDocumento ? true : false;
                                                object dto = _bc.GetMasterDTO(AppMasters.MasterType.Simple, docId, isInt, line[colIndex], true);

                                                bool hierarchyFather = false;
                                                if (dto is DTO_MasterHierarchyBasic)
                                                {
                                                    if ((dto as DTO_MasterHierarchyBasic).MovInd.Value == false)
                                                        hierarchyFather = true;
                                                }
                                                if (dto != null && !hierarchyFather)
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), true));
                                                }
                                                else
                                                {
                                                    fks[colRsx].Add(new Tuple<string, bool>(line[colIndex].Trim(), false));

                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    if (hierarchyFather)
                                                        rdF.Message = string.Format(msgFkHierarchyFather, line[colIndex]);
                                                    else
                                                        rdF.Message = string.Format(msgFkNotFound, line[colIndex]);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            if (createDTO)
                            {
                                det = new DTO_AumentoSalarial();
                                for (int colIndex = 0; colIndex < colNames.Count(); colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion de Nulls (Campos basicos)
                                        if (string.IsNullOrEmpty(colValue) &&
                                                (colRsx == _empleadoIDRsx ||
                                                colName == "FechaAumento" ||
                                                colName == "Sueldo" ||
                                                colName == "NuevoSueldo"
                                              )
                                        )
                                        {
                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                            rdF.Field = colRsx;
                                            rdF.Message = msgEmptyField;
                                            rd.DetailsFields.Add(rdF);

                                            createDTO = false;
                                        }

                                        #endregion
                                        #region Validacion Formatos
                                        PropertyInfo pi = det.GetType().GetProperty(colName);
                                        UDT udt = (UDT)pi.GetValue(det, null);
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        //Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colVals[colRsx] = colVal;
                                            //Si paso las validaciones asigne el valor al DTO
                                            if (createDTO)
                                            {
                                                udt.SetValueFromString(colVal);
                                            }
                                        }

                                        #region Otros Formatos

                                        //Valida formatos para las otras columnas
                                        if (colValue != string.Empty)
                                        {
                                            if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                            {
                                                try
                                                {
                                                    DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                            {
                                                try
                                                {
                                                    int val = Convert.ToInt32(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                            {
                                                try
                                                {
                                                    long val = Convert.ToInt64(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                            {
                                                try
                                                {
                                                    short val = Convert.ToInt16(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                            {
                                                try
                                                {
                                                    byte val = Convert.ToByte(colValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                            {
                                                try
                                                {
                                                    decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                    if (colValue.Trim().Contains(','))
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }

                                        } //validacion si no es null
                                        #endregion

                                        #endregion
                                        //Si paso las validaciones asigne el valor al DTO
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                        {
                                            udt.SetValueFromString(colValue);
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = _bc.GetResourceForException(ex1, "WinApp", "DocumentNominaForm.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);

                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                            }

                            if (createDTO && validList)
                            {
                                DTO_noEmpleado empleado = (DTO_noEmpleado)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, false, det.EmpleadoID.Value, true);
                                det.NombreEmpleado.Value = empleado.Descriptivo.Value;
                                det.Aumento.Value = det.NuevoSueldo.Value - det.Sueldo.Value;
                                det.Dias.Value = 0;
                                det.Ajuste.Value = 0;

                                if (det.FechaAumento.Value.Value < this.dtPeriod.DateTime)
                                {
                                    TimeSpan timer = this.dtPeriod.DateTime.Subtract(det.FechaAumento.Value.Value);
                                    det.Dias.Value = timer.Days;
                                    det.Ajuste.Value = (det.Aumento.Value / 30) * det.Dias.Value;
                                }

                                if (!this._sueldoEmpleados.Any(x => x.EmpleadoID.Value == det.EmpleadoID.Value))
                                {
                                    this._sueldoEmpleados.Add(det);
                                }
                            }
                            else
                                validList = false;
                        }
                    }
                    #endregion
                    #region Valida las restricciones particulares del comprobante
                    if (validList)
                    {
                        result.Details = new List<DTO_TxResultDetail>();

                        int index = this.NumFila;
                        int i = 0;
                        FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ValidatingData) });
                        percent = 0;
                        foreach (DTO_AumentoSalarial dto in this._sueldoEmpleados)
                        {
                            createDTO = true;
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this._sueldoEmpleados.Count);

                            if (FormProvider.Master.ProcessCanceled(this.documentID))
                            {
                                result.Details = new List<DTO_TxResultDetail>();
                                result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                                result.Result = ResultValue.NOK;
                                break;
                            }

                            i++;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Validaciones particulares del documento al importar del DTO
                            //this.ValidateDataImport(dto, cta, rd, msgCero, msgVals);
                            #endregion

                            result.Details.Add(rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                result.ResultMessage = ResultValue.NOK.ToString();
                            }
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (sendToServer && result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        if (result.Result.Equals(ResultValue.OK))
                        {
                            this.Invoke(this.refreshGridDelegate);
                        }
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.importando = false;
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion      
        
    }
}
