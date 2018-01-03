using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors;
using System.Reflection;
using NewAge.DTO.UDT;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalBeneficiosXEmpleado : Form
    {
        public ModalBeneficiosXEmpleado()
        {
            InitializeComponent();
            this.SetInitParameters();
            FormProvider.LoadResources(this, AppMasters.noEmpleado);
            FormProvider.LoadResources(this, AppForms.ModalBeneficiosNomina);
        }
        
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        DTO_noEmpleado _empleado = null;
        List<DTO_noBeneficiosxEmpleado> _lBeneficios = new List<DTO_noBeneficiosxEmpleado>();
        string unboundPrefix = "Unbound_";
        int documentID = 0;
        string empresaID = string.Empty;

        #endregion

        #region Delegados
              

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this._empleado = this.uc_Empleados.empleadoActivo;
            _bc.InitMasterUC(this.uc_mfBeneficio, AppMasters.noCompFlexible, true, true, false, true, null);
            _bc.InitMasterUC(this.uc_mfTercero, AppMasters.coTercero, true, true, false, true, null);
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);
            this.txtContratoNOID.Text = this._empleado.ContratoNOID.Value.ToString();
        }
                
        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {

        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this._lBeneficios = null;
            this.gcDocument.DataSource = null;
            this.uc_mfBeneficio.Refresh();
            this.uc_mfTercero.Refresh();
            this.txtContratoNOID.Text = string.Empty;
            this.txtValor.Text = string.Empty;
            this.uc_Empleados.Refresh();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this.documentID = AppForms.ModalBeneficiosNomina;
            this.empresaID = this._bc.AdministrationModel.Empresa.ID.Value;
            this.InitControls();
            this.AddGridCols();
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                GridColumn empleadoID = new GridColumn();
                empleadoID.FieldName = this.unboundPrefix + "EmpleadoID";
                empleadoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoID");
                empleadoID.UnboundType = UnboundColumnType.String;
                empleadoID.VisibleIndex = 0;
                empleadoID.Width = 200;
                empleadoID.Visible = true;
                empleadoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(empleadoID);

                GridColumn contratoNOID = new GridColumn();
                contratoNOID.FieldName = this.unboundPrefix + "ContratoNOID";
                contratoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ContratoNOID");
                contratoNOID.UnboundType = UnboundColumnType.Integer;
                contratoNOID.VisibleIndex = 0;
                contratoNOID.Width = 100;
                contratoNOID.Visible = true;
                contratoNOID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(contratoNOID);

                GridColumn compFlexibleID = new GridColumn();
                compFlexibleID.FieldName = this.unboundPrefix + "CompFlexibleID";
                compFlexibleID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CompFlexibleID");
                compFlexibleID.UnboundType = UnboundColumnType.String;
                compFlexibleID.VisibleIndex = 0;
                compFlexibleID.Width = 200;
                compFlexibleID.Visible = true;
                compFlexibleID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(compFlexibleID);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 0;
                valor.Width = 100;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valor);

                GridColumn terceroID = new GridColumn();
                terceroID.FieldName = this.unboundPrefix + "TerceroID";
                terceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
                terceroID.UnboundType = UnboundColumnType.String;
                terceroID.VisibleIndex = 0;
                terceroID.Width = 200;
                terceroID.Visible = true;
                terceroID.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(terceroID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-BeneficiosXEmpleado.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
            DTO_noBeneficiosxEmpleado beneficio = new DTO_noBeneficiosxEmpleado();
           
            #region Asigna datos a la fila

            beneficio.EmpresaID.Value = this.empresaID;
            beneficio.EmpleadoID.Value = this._empleado.ID.Value;
            beneficio.ContratoNOID.Value = this._empleado.ContratoNOID.Value;
            beneficio.CompFlexibleID.Value = this.uc_mfBeneficio.Value;
            beneficio.Valor.Value = !string.IsNullOrEmpty(this.txtValor.Text) ? Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) : 0;
            beneficio.TerceroID.Value = this.uc_mfTercero.Value;
            beneficio.ActivaInd.Value = true;
   
            #endregion

            #region Recarga la Grilla de novedades

            if (!this._lBeneficios.Any(x => x.EmpresaID.Value == beneficio.EmpresaID.Value && x.EmpleadoID.Value == beneficio.EmpleadoID.Value && x.CompFlexibleID.Value == beneficio.CompFlexibleID.Value))
            {
                this._lBeneficios.Add(beneficio);
                this.gcDocument.DataSource = this._lBeneficios;
                this.gvDocument.RefreshData();
            }
            else
            { 
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PkInUse));
            }
           

            #endregion
     
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        private void AfterInitialize()
        {
         
        }

        /// <summary>
        /// Carga los datos en la grilla de beneficios
        /// </summary>
        private void LoadData()
        {
            this._lBeneficios = this._bc.AdministrationModel.Nomina_GetBeneficioXEmpleado(this._empleado.ID.Value);
            this.gcDocument.DataSource = this._lBeneficios;
        }        

        /// <summary>
        /// Valores de los campos fk en la grilla
        /// </summary>
        private void EditarValoresFkGrilla(string columnName, string value)
        {
            try
            {
                if (columnName == "TerceroID")
                {
                    DTO_coTercero dto = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this.gvDocument.EditingValue.ToString(), true);
                    this._lBeneficios[this.gvDocument.FocusedRowHandle].TerceroID.Value = dto.ID.Value;
                }
                if (columnName == "CompFlexibleID")
                {
                    DTO_noCompFlexible dto = (DTO_noCompFlexible)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noCompFlexible, false, this.gvDocument.EditingValue.ToString(), true);
                    this._lBeneficios[this.gvDocument.FocusedRowHandle].CompFlexibleID.Value = dto.ID.Value;
                }
                
                this.gcDocument.DataSource = this._lBeneficios;
                this.gvDocument.PostEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
            string countMethod = "MasterSimple_Count";
            string dataMethod = "MasterSimple_GetPaged";
    
            string modFrmCode = props.DocumentoID.ToString();
            string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
            Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

            DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
            if (fktable.Jerarquica.Value.Value)
            {
                ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                modal.ShowDialog();
                this.EditarValoresFkGrilla(col, modal.returnValue);  
            }
            else
            {
                ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                modal.ShowDialog();
                this.EditarValoresFkGrilla(col, modal.returnValue);  
            }
        }

        #endregion
        
        #region Eventos Control Empleados

        /// <summary>
        /// Evento que asigna el numero de contrato al seleccionar el empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
            this._empleado = this.uc_Empleados.empleadoActivo;
            this.txtContratoNOID.Text = this._empleado.ContratoNOID.Value.ToString();
            this.LoadData();
        }     

        #endregion
               
        #region Eventos Controles Comunes

        /// <summary>
        /// Guarda los beneficios a cada empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var result = this._bc.AdministrationModel.Nomina_AddBeneficioXEmpleado(this._lBeneficios);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();
            }
                       

            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        #endregion

        #region Eventos Grilla de Beneficios

        /// <summary>
        /// Agrega o elimina un row en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
            {
                if (this.gvDocument.ActiveFilterString != string.Empty)
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                else
                {
                    this.AddNewRow();                   
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
                    int rowHandle = this.gvDocument.FocusedRowHandle;

                    if (this._lBeneficios.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this._lBeneficios.Remove(this._lBeneficios[rowHandle]);
                        //Si borra el primer registro
                        if (rowHandle == 0)
                            this.gvDocument.FocusedRowHandle = 0;
                        //Si selecciona el ultimo
                        else
                            this.gvDocument.FocusedRowHandle = rowHandle - 1;

                        this.gvDocument.RefreshData();
                    }
                }
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "TerceroID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "CompFlexibleID") 
            {
                e.RepositoryItem = this.editBtnGrid;
            }
            if (fieldName == "Valor")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operaciones al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "Valor")
            {
                Decimal val = (Decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._lBeneficios[this.gvDocument.FocusedRowHandle].Valor.Value = val;
            }
        }

        /// <summary>
        /// Eveto para el manejo de los campos fk en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            ButtonEdit origin = (ButtonEdit)sender;
            this.ShowFKModal(this.gvDocument.FocusedRowHandle, colName, origin);
        }

        /// <summary>
        /// Maneja campos en las grillas
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
      
        #endregion 
      
    }
}
