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
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class NovedadesContrato : DocumentNominaForm
    {
        //public NovedadesContrato()
        //{
        //    InitializeComponent();
        //}
   
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noNovedadesContrato> _novedades = null;
        private int origen;
        private bool _isFirstLoad = true;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            //agrega las novedades al sistema
            if (this._novedades != null)
            {
                var result = _bc.AdministrationModel.Nomina_AddNovedadesContrato(_novedades);
                if (result.Result == ResultValue.OK)
                {
                    this.RefreshDocument();
                }

                //Recarga la grilla de novedades
                this._novedades = _bc.AdministrationModel.Nomina_GetNovedadesContrato(this.uc_Empleados.empleadoActivo.ID.Value);
                gcDocument.DataSource = _novedades;

                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoNoFound));
            }
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
            //Recarga la grilla de novedades
            gcDocument.DataSource = this._novedades;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            _bc.InitMasterUC(this.uc_MasterConcepto, AppMasters.noContratoNov, true, true, false, true);
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);            
            this.editValue.EditValueChanged += new EventHandler(editValue_EditValueChanged);          
            this.FieldsEnabled(true);
         }       

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.uc_MasterConcepto.EnableControl(estado);
            this.txtValor.Enabled = estado;
            this.dtFechaInicial.Enabled = estado;
            this.dtFechaFinal.Enabled = estado;
            this.txtDocumento.Enabled = estado;
            this.txtObservacion.Enabled = estado;
        }

        /// <summary>
        /// lista las novedades del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noNovedadesContrato> GetNovedades()
        {
            _novedades = _bc.AdministrationModel.Nomina_GetNovedadesContrato(this.uc_Empleados.empleadoActivo.ID.Value);
            return _novedades;
        }

        /// <summary>
        /// Carga el detalle de la novedad
        /// </summary>
        /// <param name="dto"></param>
        private void LoadDetailNovedad(DTO_noNovedadesContrato dto)
        {
            this.uc_MasterConcepto.Value = dto.ContratoNONovID.Value;
            //this.dtFechaInicial.DateTime = dto.FechaInicial.Value.Value;
            //this.dtFechaFinal.DateTime = dto.FechaFinal.Value.Value;
            this.txtDocumento.Text = dto.Documento.Value;
            this.txtValor.Text = dto.Valor.Value.ToString();
            this.txtObservacion.Text = dto.Observacion.Value;
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            this._novedades = null;
        }

        /// <summary>
        /// Valores de los campos fk en la grilla
        /// </summary>
        private void EditarValoresFkGrilla()
        {
            DTO_noContratoNov dto = (DTO_noContratoNov)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noContratoNov, false, this.gvDocument.EditingValue.ToString(), true);
            this._novedades[this.gvDocument.FocusedRowHandle].ContratoNONovDesc.Value = dto.Descriptivo.Value;
            this.gcDocument.DataSource = this._novedades;
            this.gvDocument.PostEditor();
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
                    FormProvider.Master.itemSave.Enabled = true;
                    //Recarga la grilla de novedades
                    this._novedades = _bc.AdministrationModel.Nomina_GetNovedadesContrato(this.uc_Empleados.empleadoActivo.ID.Value);
                    gcDocument.DataSource = null;
                    gcDocument.DataSource = this._novedades;
                }
                else
                {
                    gcDocument.DataSource = null;
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
            this.documentID = AppDocuments.NovedadesContrato;

            base.SetInitParameters();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;
          
            this.InitControls();
            this.AddGridCols();
            this.LoadData(true);
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                GridColumn ContratoNONovID = new GridColumn();
                ContratoNONovID.FieldName = this.unboundPrefix + "ContratoNONovID";
                ContratoNONovID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ContratoNONovID");
                ContratoNONovID.UnboundType = UnboundColumnType.String;
                ContratoNONovID.VisibleIndex = 0;
                ContratoNONovID.Width = 100;
                ContratoNONovID.Visible = true;
                ContratoNONovID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ContratoNONovID);

                GridColumn ContratoNONovDesc = new GridColumn();
                ContratoNONovDesc.FieldName = this.unboundPrefix + "ContratoNONovDesc";
                ContratoNONovDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ContratoNONovDesc");
                ContratoNONovDesc.UnboundType = UnboundColumnType.String;
                ContratoNONovDesc.VisibleIndex = 0;
                ContratoNONovDesc.Width = 200;
                ContratoNONovDesc.Visible = true;
                ContratoNONovDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ContratoNONovDesc);

                GridColumn fechaInicial = new GridColumn();
                fechaInicial.FieldName = this.unboundPrefix + "FechaInicial";
                fechaInicial.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaInicial");
                fechaInicial.UnboundType = UnboundColumnType.DateTime;
                fechaInicial.VisibleIndex = 0;
                fechaInicial.Width = 100;
                fechaInicial.Visible = true;
                fechaInicial.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(fechaInicial);


                GridColumn fechaFinal = new GridColumn();
                fechaFinal.FieldName = this.unboundPrefix + "FechaFinal";
                fechaFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFinal");
                fechaFinal.UnboundType = UnboundColumnType.DateTime;
                fechaFinal.VisibleIndex = 0;
                fechaFinal.Width = 100;
                fechaFinal.Visible = true;
                fechaFinal.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(fechaFinal);


                GridColumn documento = new GridColumn();
                documento.FieldName = this.unboundPrefix + "Documento";
                documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
                documento.UnboundType = UnboundColumnType.String;
                documento.VisibleIndex = 0;
                documento.Width = 200;
                documento.Visible = true;
                documento.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(documento);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 0;
                valor.Width = 100;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valor);

                GridColumn Observacion = new GridColumn();
                Observacion.FieldName = this.unboundPrefix + "Observacion";
                Observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                Observacion.UnboundType = UnboundColumnType.String;
                Observacion.VisibleIndex = 0;
                Observacion.Width = 200;
                Observacion.Visible = true;
                Observacion.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(Observacion);

                GridColumn activaInd = new GridColumn();
                activaInd.FieldName = this.unboundPrefix + "ActivaInd";
                activaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivaInd");
                activaInd.UnboundType = UnboundColumnType.Boolean;
                activaInd.VisibleIndex = 0;
                activaInd.Width = 150;
                activaInd.Visible = true;
                activaInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(activaInd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NovedadesContrato.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            try
            {
                base.AddNewRow();
                DTO_noNovedadesContrato novedad = new DTO_noNovedadesContrato();

                if (!string.IsNullOrEmpty(uc_MasterConcepto.Value))
                {

                    DTO_noContratoNov contratoNov = (DTO_noContratoNov)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noContratoNov, false, this.uc_MasterConcepto.Value, true);

                    if (this._novedades != null)
                    {
                        if (_novedades.Any(x => x.ContratoNONovID.Value == contratoNov.ID.Value))
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoExist));
                            return;
                        }

                        foreach (var nov in this._novedades)
                        {
                            if (this.dtFechaInicial.DateTime > nov.FechaInicial.Value.Value)
                            {
                                if (this.dtFechaInicial.DateTime <= nov.FechaFinal.Value)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoExist));
                                    return;
                                }
                            }
                            else
                            {
                                if (this.dtFechaInicial.DateTime >= nov.FechaInicial.Value)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoExist));
                                    return;
                                }
                            }
                        }
                    }

                    #region Asigna datos a la fila

                    novedad.EmpresaID.Value = this.empresaID;
                    novedad.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
                    novedad.ContratoNONovID.Value = contratoNov.ID.Value;
                    novedad.ContratoNONovDesc.Value = contratoNov.Descriptivo.Value;
                    novedad.FechaInicial.Value = this.dtFechaInicial.DateTime;
                    novedad.FechaFinal.Value = this.dtFechaFinal.DateTime;
                    novedad.Valor.Value = !string.IsNullOrEmpty(this.txtValor.Text) ? Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) : 0;
                    novedad.ActivaInd.Value = true;
                    novedad.Documento.Value = this.txtDocumento.Text;
                    novedad.Observacion.Value = this.txtObservacion.Text;
                    novedad.ContratoNOID.Value = this.uc_Empleados.empleadoActivo.ContratoNOID.Value;

                    #endregion

                    #region Recarga la Grilla de novedades

                    if (this._novedades == null)
                        this._novedades = new List<DTO_noNovedadesContrato>();

                    this._novedades.Add(novedad);
                    this.gcDocument.DataSource = this._novedades;
                    this.gvDocument.PostEditor();
                    this.gvDocument.RefreshData();

                    #endregion

                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoSelect));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NovedadesContrato.cs", "AddNewRow"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            this.dtFechaInicial.DateTime = this.dtPeriod.DateTime;
            this.dtFechaFinal.DateTime = this.dtPeriod.DateTime.AddDays(1);
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
            FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);  
        }

        #endregion

        #region Eventos Header
               
        /// <summary>
        /// Agrega un registro a la grilla de documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.AddNewRow();
        }

        /// <summary>
        /// Evento que valida el rango corrector de la fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (!_isFirstLoad)
            {
                if (dtFechaInicial.DateTime > dtFechaFinal.DateTime)
                {
                    this.dtFechaFinal.DateTime = this.dtFechaInicial.DateTime.AddDays(1);
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoDateStartNotValid));
                }
            }
            this._isFirstLoad = false;
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
                    
                    this._novedades.Remove(this._novedades[rowHandle]);
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
        
        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ContratoNONovID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "Valor")
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
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
   
            if (fieldName == "FechaInicial")
            {
                DateTime val = (DateTime)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._novedades[this.gvDocument.FocusedRowHandle].FechaInicial.Value = val;               
            }
            if (fieldName == "FechaFinal")
            {
                DateTime val = (DateTime)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                DateTime valIni = this._novedades[this.gvDocument.FocusedRowHandle].FechaInicial.Value.Value;
                if (val <= valIni)
                {
                    this.LoadData(true);
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoExist));
                }
                else
                {
                    this._novedades[this.gvDocument.FocusedRowHandle].FechaFinal.Value = val;  
                }
            }
            if (fieldName == "Documento")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._novedades[this.gvDocument.FocusedRowHandle].Documento.Value = val;
            }
            if (fieldName == "Observacion")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._novedades[this.gvDocument.FocusedRowHandle].Observacion.Value = val;
            }
            if (fieldName == "ActivaInd")
            {
                bool val = (bool)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._novedades[this.gvDocument.FocusedRowHandle].ActivaInd.Value = val;
            }           

        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            base.gvDocument_FocusedRowChanged(sender, e);

            int index = e.FocusedRowHandle;
            if (index>=0 && _novedades != null && _novedades.Count > 0)
            {
                if (index > this._novedades.Count)
                index = this._novedades.Count - 1;

                DTO_noNovedadesContrato novedad = _novedades[index];
                this.LoadDetailNovedad(novedad);
            }
            else
            {
                this.RefreshDocument();
            }
        }

        protected void editValue_EditValueChanged(object sender, EventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            TextEdit origin = (TextEdit)sender;
            if (colName == "Valor")
                this._novedades[this.gvDocument.FocusedRowHandle].Valor.Value = !string.IsNullOrEmpty(origin.Text) ? Convert.ToDecimal(origin.EditValue, CultureInfo.InvariantCulture) : 0;
        }

        protected void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            ButtonEdit origin = (ButtonEdit)sender;
            if (colName == "ContratoNONovID")
            {
                this.EditarValoresFkGrilla();
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
            if (this.dtFechaFinal.DateTime < this.dtFechaInicial.DateTime)
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadContratoDateStartNotValid));
                return;
            }

            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para eliminar novedades
        /// </summary>
        public override void TBDelete()
        {
            base.TBDelete();
            if (this.gvDocument.FocusedRowHandle >= 0 && this._novedades != null)
            {
                DTO_noNovedadesContrato novedad = this._novedades[this.gvDocument.FocusedRowHandle];
                DTO_TxResult result = this._bc.AdministrationModel.Nomina_noNovedadesContrato_Delete(novedad);
                this.RefreshDocument();
                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();
            }                
        }
                
        #endregion

        #region Eventos Control Empleados

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NovedadesContrato.cs", "SaveThread"));
            }
        }

        #endregion            
        
    }
}
