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
using DevExpress.XtraEditors.Mask;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class NovedadesNomina : DocumentNominaForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private int origen;
        private int novedadIndex = 0;
    
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            //agrega las novedades al sistema
            var result = _bc.AdministrationModel.Nomina_AddNovedadNomina(this.Novedades);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();
            }

            //Recarga la grilla de novedades
            this.LoadData(true);

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
                gcDocument.DataSource = this.Novedades;
        }
                     
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            List<DTO_glConsultaFiltro> lfiltro = new List<DTO_glConsultaFiltro>();
       
            lfiltro.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "TipoLiquidacion",
                ValorFiltro = "1",
                OperadorFiltro = OperadorFiltro.Diferente,
            });

            _bc.InitMasterUC(this.uc_MasterConcepto, AppMasters.noConceptoNOM, true, true, false, true, lfiltro);

            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);
            this.editChkBox.CheckedChanged += new EventHandler(editChkBox_CheckedChanged);
            this.editValue.EditValueChanged += new EventHandler(editValue_EditValueChanged);
            this.editCmb.SelectedValueChanged += new EventHandler(editCmb_SelectedValueChanged);
            TablesResources.GetTableResources(this.cmbPeriodo, typeof(PeriodoPago));
            int periodoNOM = Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_LiquidaNominaQuincenal));
            if (periodoNOM == (int)PeriodoPagoNomina.Mensual)
            {
                this.cmbPeriodo.RemoveItem(((int)PeriodoPago.PrimeraQuincena).ToString());
                this.cmbPeriodo.RemoveItem(((int)PeriodoPago.SegundaQuincena).ToString());
            }           
            this.cmbPeriodo.SelectedIndex = 0;
            this.FieldsEnabled(true);
        }
             
        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado) 
        {
            this.txtValor.Enabled = estado;
            this.cmbPeriodo.Enabled = estado;
            this.chkFijaInd.Enabled = estado;
            this.uc_MasterConcepto.EnableControl(estado);            
        }

        /// <summary>
        /// lista las novedades del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noNovedadesNomina> GetNovedades()
        {    
            this.Novedades = _bc.AdministrationModel.Nomina_GetNovedades(this.uc_Empleados.empleadoActivo.ID.Value);
            return this.Novedades;       
        }

        /// <summary>
        /// Carga el detalle de la novedad
        /// </summary>
        /// <param name="dto"></param>
        private void LoadDetailNovedad(DTO_noNovedadesNomina dto)
        {
            this.txtValor.Text = dto.Valor.Value.ToString();
            this.uc_MasterConcepto.Value = dto.ConceptoNOID.Value;
            this.cmbPeriodo.SelectedValue = dto.PeriodoPago.Value;
            this.chkFijaInd.Checked = dto.FijaInd.Value.Value;
        }
      
        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {            
            this.gcDocument.DataSource = null;
            this.txtValor.Text = string.Empty;
            this.uc_MasterConcepto.ResetText();
            this.Novedades.Clear();
            this.FieldsEnabled(true);
        }

        /// <summary>
        /// Valores de los campos fk en la grilla
        /// </summary>
        private void EditarValoresFkGrilla()
        {
            DTO_noConceptoNOM dto = (DTO_noConceptoNOM)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, this.gvDocument.EditingValue.ToString(), true);
            this.Novedades[this.gvDocument.FocusedRowHandle].ConceptoNODesc.Value = dto.Descriptivo.Value;
            this.gcDocument.DataSource = this.Novedades;
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
                this.Novedades = this._bc.AdministrationModel.Nomina_GetNovedades(this.uc_Empleados.empleadoActivo.ID.Value);
                if (this.Novedades != null && this.Novedades.Count > 0)
                {
                    gcDocument.DataSource = null;
                    gcDocument.DataSource = this.Novedades;
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                }
                else
                {
                    gcDocument.DataSource = null;
                    FormProvider.Master.itemDelete.Enabled = false;
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
            //Comprobante
            if (colName == this._conceptoRsx)
                return AppMasters.noConceptoNOM;
            
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
            this.documentID = AppDocuments.NovedadesNomina;

            base.SetInitParameters();
            this.format = _bc.GetImportExportFormat(typeof(DTO_noNovedadesNomina), this.documentID);

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
                #region Novedades Nomina

                GridColumn conceptoNOM = new GridColumn();
                conceptoNOM.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
                conceptoNOM.UnboundType = UnboundColumnType.String;
                conceptoNOM.VisibleIndex = 0;
                conceptoNOM.Width = 100;
                conceptoNOM.Visible = true;
                conceptoNOM.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOM);

                GridColumn conceptoNOMDesc = new GridColumn();
                conceptoNOMDesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNOMDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
                conceptoNOMDesc.UnboundType = UnboundColumnType.String;
                conceptoNOMDesc.VisibleIndex = 0;
                conceptoNOMDesc.Width = 500;
                conceptoNOMDesc.Visible = true;
                conceptoNOMDesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOMDesc);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.String;
                valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valor.AppearanceCell.Options.UseTextOptions = true;
                valor.VisibleIndex = 0;
                valor.Width = 100;
                valor.Visible = true;
                valor.ColumnEdit = this.editValue;
                valor.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(valor);

                GridColumn FijaInd = new GridColumn();
                FijaInd.FieldName = this.unboundPrefix + "FijaInd";
                FijaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FijaInd");
                FijaInd.UnboundType = UnboundColumnType.Boolean;
                FijaInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FijaInd.AppearanceCell.Options.UseTextOptions = true;
                FijaInd.VisibleIndex = 0;
                FijaInd.Width = 100;
                FijaInd.Visible = true;
                FijaInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(FijaInd);

                GridColumn PeriodoPago = new GridColumn();
                PeriodoPago.FieldName = this.unboundPrefix + "PeriodoPago";
                PeriodoPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PeriodoPago");
                PeriodoPago.UnboundType = UnboundColumnType.Integer;
                PeriodoPago.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                PeriodoPago.AppearanceCell.Options.UseTextOptions = true;
                PeriodoPago.VisibleIndex = 0;
                PeriodoPago.Width = 100;
                PeriodoPago.Visible = true;
                PeriodoPago.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(PeriodoPago);

                GridColumn ActivaInd = new GridColumn();
                ActivaInd.FieldName = this.unboundPrefix + "ActivaInd";
                ActivaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ActivaInd");
                ActivaInd.UnboundType = UnboundColumnType.Boolean;
                ActivaInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ActivaInd.AppearanceCell.Options.UseTextOptions = true;
                ActivaInd.VisibleIndex = 0;
                ActivaInd.Width = 100;
                ActivaInd.Visible = true;
                ActivaInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(ActivaInd);


                #endregion 
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NovedadesNomina.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
           
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            base.AddNewRow();
            DTO_noNovedadesNomina novedad = new DTO_noNovedadesNomina();

            if (!string.IsNullOrEmpty(uc_MasterConcepto.Value))
            {
                DTO_noConceptoNOM dtoConcepto = (DTO_noConceptoNOM)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, uc_MasterConcepto.Value, true);
                
                if (!this.Novedades.Any(x => x.ConceptoNOID.Value == dtoConcepto.ID.Value))
                {
                    #region Asigna datos a la fila

                    decimal valor = 0;
                    if (!string.IsNullOrEmpty(this.txtValor.Text))
                        valor = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);

                    novedad.EmpresaID.Value = this.empresaID;
                    novedad.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
                    novedad.ConceptoNOID.Value = dtoConcepto.ID.Value;
                    novedad.ConceptoNODesc.Value = dtoConcepto.Descriptivo.Value;
                    novedad.Valor.Value = valor;
                    novedad.FijaInd.Value = this.chkFijaInd.Checked;
                    novedad.PeriodoPago.Value = Convert.ToInt32((this.cmbPeriodo.SelectedItem as ComboBoxItem).Value);
                    novedad.OrigenNovedad.Value = 1;//Indica si la novedad se ingresa de forma manual
                    novedad.ActivaInd.Value = true;

                    #endregion

                    if (this.Novedades == null)
                        this.Novedades = new List<DTO_noNovedadesNomina>();

                    this.Novedades.Add(novedad);
                    this.gcDocument.DataSource = this.Novedades;
                    this.gvDocument.PostEditor();
                    this.gvDocument.RefreshData();

                    #region Asigna la visibilidad de las columnas

                    this.gvDocument.Columns[this.unboundPrefix + "ConceptoNOID"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_NovedadNominaExist));
                }

                    #endregion
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_ConceptoNOMSelect));
            }           
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
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
            }
        
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Habilita el boton guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_Leave(object sender, EventArgs e)
        {
            this.origen = 1;
        }

        /// <summary>
        /// Agreda un elemento a la grilla de datos
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

                    if (this.Novedades.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this.Novedades.Remove(this.Novedades[rowHandle]);
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

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ConceptoNOID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }

            if (fieldName == "FijaInd" || fieldName == "ActivaInd")
            {
                e.RepositoryItem = this.editChkBox;
            }
                      
            if (fieldName == "PeriodoPago")
            {
                this.editCmb.Items.Clear();
                // Primera Quincena
                this.editCmb.Items.Add(((int)PeriodoPago.PrimeraQuincena).ToString() +  "-"  +  _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Primera_Quincena));        
                // Segunda Quincena
                this.editCmb.Items.Add(((int)PeriodoPago.SegundaQuincena).ToString() + "-" + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Segunda_Quincena));
                // Ambas Quincenas
                this.editCmb.Items.Add(((int)PeriodoPago.AmbasQuincenas).ToString() + "-" + _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Ambas_Quincena));

                this.editCmb.Mask.MaskType = MaskType.RegEx;
                e.RepositoryItem = this.editCmb;
            }

            
        }
                       
        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int index = e.FocusedRowHandle;
            if (index >= 0)
            {
                if (this.Novedades != null && this.Novedades.Count > 0)
                {
                    if (index > this.Novedades.Count)
                        index = this.Novedades.Count - 1;

                    DTO_noNovedadesNomina novedad = this.Novedades[index];
                    this.LoadDetailNovedad(novedad);
                    this.novedadIndex = index;
                }
                else
                {
                    this.RefreshDocument();
                }
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
            this.EditarValoresFkGrilla();  
        }

        /// <summary>
        /// Evento para modificar Dto en campos booleanos 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editChkBox_CheckedChanged(object sender, EventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            CheckEdit origin = (CheckEdit)sender;
            if (colName == "ActivaInd")
                this.Novedades[this.gvDocument.FocusedRowHandle].ActivaInd.Value = origin.Checked;
            if (colName == "FijaInd")
                this.Novedades[this.gvDocument.FocusedRowHandle].FijaInd.Value = origin.Checked;
        }

        /// <summary>
        /// Edita el campo Valor en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editValue_EditValueChanged(object sender, EventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            TextEdit origin = (TextEdit)sender;
            if (colName == "Valor")
                this.Novedades[this.gvDocument.FocusedRowHandle].Valor.Value = !string.IsNullOrEmpty(origin.Text) ? Convert.ToDecimal(origin.EditValue, CultureInfo.InvariantCulture) : 0;
        }

        /// <summary>
        /// Evento modifica valor de los campos tipo combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editCmb_SelectedValueChanged(object sender, EventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            ComboBoxEdit origin = (ComboBoxEdit)sender;
            if (colName == "PeriodoPago")
            {
                string key = origin.SelectedItem.ToString().Split('-')[0];
                this.Novedades[this.gvDocument.FocusedRowHandle].PeriodoPago.Value = Convert.ToInt32(key);
                origin.Text = key;
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
                    base.TBImport();                      
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

        /// <summary>
        /// Elimina la novedad existente
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                string msgTitleLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_Title_DeleteNovedad);
                string msgLoadTemp = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_Body_DeleteNovedad);
                
                if (this.Novedades != null && this.Novedades.Count > 0)
                {
                     //Revisa si desea cargar los temporales
                    if (MessageBox.Show(msgLoadTemp, msgTitleLoadTemp, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this._bc.AdministrationModel.Nomina_DelNovedadesNomina(this.Novedades[this.novedadIndex]);
                        this.RefreshDocument();
                    }                    
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
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

            if (this.Novedades != null && this.Novedades.Count > 0)
                FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
            else
                FormProvider.Master.itemDelete.Enabled = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-NovedadesNomina.cs", "NovedadesNomina.cs-SaveThread"));
            }
        }

        #endregion                   

             
   }
}
