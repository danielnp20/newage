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
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Prestamos : DocumentNominaForm
    {
        public Prestamos()
        {
           // InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noPrestamo> _prestamos = null;
        public Dictionary<string, string> lPeridos = new Dictionary<string, string>();
     
        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            //agrega las novedades al sistema
            var result = _bc.AdministrationModel.Nomina_AddPrestamo(_prestamos);
            if (result.Result == ResultValue.OK)
                this.RefreshDocument();

            //Recarga la grilla de prestamos
            this._prestamos = _bc.AdministrationModel.Nomina_GetPrestamos(this.uc_Empleados.empleadoActivo.ID.Value);
            gcDocument.DataSource = _prestamos;

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
            gcDocument.DataSource = this._prestamos;
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            
            List<DTO_glConsultaFiltro> lfiltro = new List<DTO_glConsultaFiltro>();
            lfiltro.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "Ind_40",
                ValorFiltro = "1",
                OperadorFiltro = OperadorFiltro.Igual,
                OperadorSentencia = "AND"
            });

            lfiltro.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "Tipo",
                ValorFiltro = "2",
                OperadorFiltro = OperadorFiltro.Igual,
            });
            
            this._bc.InitMasterUC(this.uc_MasterConcepto, AppMasters.noConceptoNOM, true, true, false, true, lfiltro);
            
            this.lPeridos.Add(((int)PeriodoPago.PrimeraQuincena).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_ComFlexPeriodo_v1"));
            this.lPeridos.Add(((int)PeriodoPago.SegundaQuincena).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_ComFlexPeriodo_v2"));
            this.lPeridos.Add(((int)PeriodoPago.AmbasQuincenas).ToString(), this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_ComFlexPeriodo_v3"));
            this.lkpCmbPeriodo.Properties.DataSource = this.lPeridos;
            this.lkpCmbPeriodo.EditValue = ((int)PeriodoPago.PrimeraQuincena).ToString();

            this.editlookUpEdit.EditValueChanged += new EventHandler(editlookUpEdit_EditValueChanged);
            
            this.FieldsEnabled(true);
        }    
       

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {
            this.dtFechaPrestamo.Properties.ReadOnly = !estado;
            this.txtValorPrestamo.Properties.ReadOnly = !estado;
            this.txtValorCuota.Properties.ReadOnly = !estado;
            this.txtDescPrima.Properties.ReadOnly = !estado;
            this.lkpCmbPeriodo.Properties.ReadOnly = !estado;
            this.uc_MasterConcepto.EnableControl(estado);
        }

        /// <summary>
        /// lista los prestamos del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noPrestamo> GetPrestamos()
        {
            _prestamos = _bc.AdministrationModel.Nomina_GetPrestamos(this.uc_Empleados.empleadoActivo.ID.Value);
            return _prestamos;
        }

        /// <summary>
        /// Carga el detalle de la novedad
        /// </summary>
        /// <param name="dto"></param>
        private void LoadDetailNovedad(DTO_noPrestamo dto)
        {
            this.dtFechaPrestamo.DateTime = dto.FechaPrestamo.Value.Value;
            this.txtValorPrestamo.Text = dto.VlrPrestamo.Value.ToString();
            this.txtValorCuota.Text = dto.VlrCuota.Value.ToString();
            this.txtDescPrima.Text = dto.DtoPrima.Value.ToString();
            this.txtAbono.Text = dto.VlrAbono.Value.ToString();        
            this.lkpCmbPeriodo.EditValue = dto.QuincenaPagos.Value.ToString();
            this.uc_MasterConcepto.Value = dto.ConceptoNOID.Value;
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.dtFecha.Enabled = false;
            FormProvider.Master.itemPrint.Enabled = false;
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
                    this._prestamos = _bc.AdministrationModel.Nomina_GetPrestamos(this.uc_Empleados.empleadoActivo.ID.Value);
                    this.gcDocument.DataSource = this._prestamos;
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
            this.documentID = AppDocuments.PrestamosEmpleados;

            base.SetInitParameters();

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

                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "FechaPrestamo";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaPrestamo");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 0;
                fecha.Width = 115;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(fecha);

                GridColumn conceptoNOID = new GridColumn();
                conceptoNOID.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
                conceptoNOID.UnboundType = UnboundColumnType.String;
                conceptoNOID.VisibleIndex = 0;
                conceptoNOID.Width = 90;
                conceptoNOID.Visible = true;
                conceptoNOID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOID);

                GridColumn conceptoNODesc = new GridColumn();
                conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
                conceptoNODesc.UnboundType = UnboundColumnType.String;
                conceptoNODesc.VisibleIndex = 0;
                conceptoNODesc.Width = 200;
                conceptoNODesc.Visible = true;
                conceptoNODesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNODesc);
                

                GridColumn vlrPrestamo = new GridColumn();
                vlrPrestamo.FieldName = this.unboundPrefix + "VlrPrestamo";
                vlrPrestamo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPrestamo");
                vlrPrestamo.UnboundType = UnboundColumnType.Decimal;
                vlrPrestamo.VisibleIndex = 0;
                vlrPrestamo.Width = 120;
                vlrPrestamo.Visible = true;
                vlrPrestamo.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(vlrPrestamo);

                GridColumn vlrCuota = new GridColumn();
                vlrCuota.FieldName = this.unboundPrefix + "VlrCuota";
                vlrCuota.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCuota");
                vlrCuota.UnboundType = UnboundColumnType.Decimal;
                vlrCuota.VisibleIndex = 0;
                vlrCuota.Width = 120;
                vlrCuota.Visible = true;
                vlrCuota.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(vlrCuota);

                GridColumn dtoPrima = new GridColumn();
                dtoPrima.FieldName = this.unboundPrefix + "DtoPrima";
                dtoPrima.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DtoPrima");
                dtoPrima.UnboundType = UnboundColumnType.Decimal;
                dtoPrima.VisibleIndex = 0;
                dtoPrima.Width = 120;
                dtoPrima.Visible = true;
                dtoPrima.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(dtoPrima);

                GridColumn vlrAbono = new GridColumn();
                vlrAbono.FieldName = this.unboundPrefix + "VlrAbono";
                vlrAbono.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrAbono");
                vlrAbono.UnboundType = UnboundColumnType.Decimal;
                vlrAbono.VisibleIndex = 0;
                vlrAbono.Width = 120;
                vlrAbono.Visible = true;
                vlrAbono.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrAbono);

                GridColumn vlrSaldo = new GridColumn();
                vlrSaldo.FieldName = this.unboundPrefix + "VlrSaldo";
                vlrSaldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSaldo");
                vlrSaldo.UnboundType = UnboundColumnType.Decimal;
                vlrSaldo.VisibleIndex = 0;
                vlrSaldo.Width = 120;
                vlrSaldo.Visible = true;
                vlrSaldo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrSaldo);               

                GridColumn quincenaPagos = new GridColumn();
                quincenaPagos.FieldName = this.unboundPrefix + "QuincenaPagos";
                quincenaPagos.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_QuincenaPagos");
                quincenaPagos.UnboundType = UnboundColumnType.String;
                quincenaPagos.VisibleIndex = 0;
                quincenaPagos.Width = 100;
                quincenaPagos.Visible = true;
                quincenaPagos.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(quincenaPagos);
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Prestamos.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        protected override void AddNewRow()
        {
            base.AddNewRow();
            DTO_noPrestamo prestamo = new DTO_noPrestamo();

            if (!string.IsNullOrEmpty(uc_MasterConcepto.Value))
            {

                #region Asigna datos a la fila

                prestamo.EmpresaID.Value = this.empresaID;
                prestamo.EmpleadoID.Value = this.uc_Empleados.empleadoActivo.ID.Value;
                prestamo.Numero.Value = 0;
                prestamo.FechaPrestamo.Value = this.dtFechaPrestamo.DateTime;
                prestamo.VlrPrestamo.Value = !string.IsNullOrEmpty(txtValorPrestamo.Text) ? Convert.ToDecimal(this.txtValorPrestamo.EditValue, CultureInfo.InvariantCulture) : 0;
                prestamo.VlrCuota.Value = !string.IsNullOrEmpty(txtValorCuota.Text) ? Convert.ToDecimal(this.txtValorCuota.EditValue, CultureInfo.InvariantCulture) : 0;
                prestamo.DtoPrima.Value = !string.IsNullOrEmpty(txtDescPrima.Text) ? Convert.ToDecimal(this.txtDescPrima.EditValue, CultureInfo.InvariantCulture) : 0;
                prestamo.VlrAbono.Value = 0;
                prestamo.QuincenaPagos.Value = Convert.ToByte(this.lkpCmbPeriodo.EditValue);
                prestamo.ActivoInd.Value = true;
                prestamo.ConceptoNOID.Value = this.uc_MasterConcepto.Value;
                prestamo.ConceptoNODesc.Value = ((DTO_noConceptoNOM)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, this.uc_MasterConcepto.Value, true)).Descriptivo.Value;
                prestamo.VlrSaldo.Value = Convert.ToDecimal(prestamo.VlrPrestamo.Value, CultureInfo.InvariantCulture);

                #endregion

                if (this._prestamos == null)
                    this._prestamos = new List<DTO_noPrestamo>();

                //Conceptos cuentas X Pagar asociado al prestamo
                string conCXP = this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCuentasXPagar);
                prestamo.DocCxP.Value = Convert.ToInt32(conCXP);
                DTO_noConceptoNOM conceptoNOM = (DTO_noConceptoNOM)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, false, this.uc_MasterConcepto.Value, true);
                prestamo.DocPrestamo.Value = conceptoNOM.TipoTercero.Value;

                this._prestamos.Add(prestamo);
                this.gcDocument.DataSource = null;
                this.gcDocument.DataSource = this._prestamos;
                this.gvDocument.RefreshData();
                this.gvDocument.PostEditor();
                FormProvider.Master.itemSave.Enabled = true;
            }
            else
            {
                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_ConceptoNOMSelect));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            this.dtFechaPrestamo.DateTime = dtPeriod.DateTime;
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

        /// <summary>
        /// Adiciona Registros a al grilla de selección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            this.AddNewRow();
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
            this.gcDocument.DataSource = this.GetPrestamos();
            this.gvDocument.RefreshData();
            this.gvDocument.PostEditor();
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

                    if (this._prestamos.Count == 1)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                        e.Handled = true;
                    }
                    else
                    {
                        this._prestamos.Remove(this._prestamos[rowHandle]);
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
            if (fieldName == "ConceptoNOID")
            {
                e.RepositoryItem = this.editBtnGrid;
            }         
            if (fieldName == "QuincenaPagos")
            {
                this.editlookUpEdit.DataSource = this.lPeridos;
                e.RepositoryItem = this.editlookUpEdit;
            }
            if (fieldName == "VlrPrestamo" || fieldName == "VlrCuota" || fieldName == "DtoPrima" || fieldName == "VlrAbono" || fieldName == "VlrSaldo")
            {
                e.RepositoryItem = this.editValue;
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

            if (fieldName == "ConceptoNOID")
            {
                string val = (string)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].ConceptoNOID.Value = val;
            }
            if (fieldName == "ActivaInd")
            {
                bool val = (bool)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].ActivoInd.Value = val;
            }
            if (fieldName == "VlrPrestamo")
            {
                decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].VlrPrestamo.Value = val;               
            }
            if (fieldName == "VlrCuota")
            {
                decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].VlrCuota.Value = val;
            }
            if (fieldName == "DtoPrima")
            {
                decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].DtoPrima.Value = val;
            }
            if (fieldName == "VlrAbono")
            {
                decimal val = (decimal)this.gvDocument.GetRowCellValue(e.RowHandle, col);
                this._prestamos[this.gvDocument.FocusedRowHandle].VlrAbono.Value = val;
            }          
        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                if (_prestamos != null && _prestamos.Count > 0)
                {
                    DTO_noPrestamo prestamo = _prestamos[e.FocusedRowHandle];
                    this.LoadDetailNovedad(prestamo);
                }
                else
                {
                    this.RefreshDocument();
                }
            }
        }

        /// <summary>
        /// Evento modifica valor de los campos tipo combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editlookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            LookUpEdit origin = (LookUpEdit)sender;
            if (colName == "QuincenaPagos")
            {
                byte key = Convert.ToByte(origin.EditValue);
                this._prestamos[this.gvDocument.FocusedRowHandle].QuincenaPagos.Value = key;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Prestamos.cs", "SaveThread"));
            }
        }

        #endregion      
     
    }
}
