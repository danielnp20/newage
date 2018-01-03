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
    public partial class PagoPlanilla : DocumentNominaPagosForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private decimal _totalNomina = 0;
        private List<DTO_noPlanillaAportesDeta> _liquidaciones = null;
        private bool IsFirstTime = false;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this._bc.InitMasterUC(this.uc_MasterFindBancoGeneral, AppMasters.tsBanco, true, true, true, false); 
            TablesResources.GetTableResources(this.editLook, AppMasters.noEmpleado, "TipoCuenta", DictionaryTables.TipoCuenta);
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(editBtnGrid_ButtonClick);
        }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected override void LookUpDocumentosDataSource()
        {
            DTO_glDocumento dtoDoc = null;
            this.documentos = new Dictionary<string, string>();
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoPlanilla.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            this.lookUpDocumentos.Properties.ReadOnly = true;
            base.LookUpDocumentosDataSource();        
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.gcDetail.DataSource = null;
            FormProvider.Master.itemPrint.Enabled = false;
            FormProvider.Master.itemSave.Enabled = false;
            this.txtTotalNomina.Text = string.Empty;
            this.uc_MasterFindBancoGeneral.Refresh();
            this._totalNomina = 0;
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
                #region Carga Liquidaciones Aprobadas

                this._liquidaciones = this._bc.AdministrationModel.Nomina_GetAllPlanillaAportes(this.dtPeriod.DateTime);
                this._totalNomina = 0;
                this.gcDocument.DataSource = this._liquidaciones;

                #endregion
               
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

            this.documentID = AppDocuments.PagoPlanilla;
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

            #region Encabezado

            GridColumn empleadoID = new GridColumn();
            empleadoID.FieldName = this.unboundPrefix + "EmpleadoID";
            empleadoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ID");
            empleadoID.UnboundType = UnboundColumnType.String;
            empleadoID.VisibleIndex = 0;
            empleadoID.Width = 100;
            empleadoID.Visible = true;
            empleadoID.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(empleadoID);

            GridColumn empleadoName = new GridColumn();
            empleadoName.FieldName = this.unboundPrefix + "NombreEmpleado";
            empleadoName.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            empleadoName.UnboundType = UnboundColumnType.String;
            empleadoName.VisibleIndex = 0;
            empleadoName.Width = 200;
            empleadoName.Visible = true;
            empleadoName.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(empleadoName);

            GridColumn banco = new GridColumn();
            banco.FieldName = this.unboundPrefix + "BancoID";
            banco.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BancoID");
            banco.UnboundType = UnboundColumnType.String;
            banco.VisibleIndex = 0;
            banco.Width = 100;
            banco.Visible = true;
            banco.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(banco);

            GridColumn bancoDesc = new GridColumn();
            bancoDesc.FieldName = this.unboundPrefix + "BancoDesc";
            bancoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BancoDesc");
            bancoDesc.UnboundType = UnboundColumnType.String;
            bancoDesc.VisibleIndex = 0;
            bancoDesc.Width = 200;
            bancoDesc.Visible = true;
            bancoDesc.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(bancoDesc);


            GridColumn tipoCuenta = new GridColumn();
            tipoCuenta.FieldName = this.unboundPrefix + "TipoCuenta";
            tipoCuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TipoCuenta");
            tipoCuenta.UnboundType = UnboundColumnType.String;
            tipoCuenta.VisibleIndex = 0;
            tipoCuenta.Width = 110;
            tipoCuenta.Visible = true;
            tipoCuenta.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(tipoCuenta);

            GridColumn cuenta = new GridColumn();
            cuenta.FieldName = this.unboundPrefix + "CuentaAbono";
            cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaAbono");
            cuenta.UnboundType = UnboundColumnType.String;
            cuenta.VisibleIndex = 0;
            cuenta.Width = 250;
            cuenta.Visible = true;
            cuenta.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(cuenta);

            GridColumn valorPago = new GridColumn();
            valorPago.FieldName = this.unboundPrefix + "Valor";
            valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            valorPago.UnboundType = UnboundColumnType.Decimal;
            valorPago.VisibleIndex = 0;
            valorPago.Width = 100;
            valorPago.Visible = true;
            valorPago.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(valorPago);


           
            #endregion       

            #region Detalle

 
            GridColumn conceptoNOM = new GridColumn();
            conceptoNOM.FieldName = this.unboundPrefix + "ConceptoNOID";
            conceptoNOM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
            conceptoNOM.UnboundType = UnboundColumnType.String;
            conceptoNOM.VisibleIndex = 0;
            conceptoNOM.Width = 100;
            conceptoNOM.Visible = true;
            conceptoNOM.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(conceptoNOM);

            GridColumn conceptoNODesc = new GridColumn();
            conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
            conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
            conceptoNODesc.UnboundType = UnboundColumnType.String;
            conceptoNODesc.VisibleIndex = 0;
            conceptoNODesc.Width = 250;
            conceptoNODesc.Visible = true;
            conceptoNODesc.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(conceptoNODesc);

            GridColumn Dias = new GridColumn();
            Dias.FieldName = this.unboundPrefix + "Dias";
            Dias.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias");
            Dias.UnboundType = UnboundColumnType.Integer;
            Dias.VisibleIndex = 0;
            Dias.Width = 100;
            Dias.Visible = true;
            Dias.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(Dias);

            GridColumn Base = new GridColumn();
            Base.FieldName = this.unboundPrefix + "Base";
            Base.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Base");
            Base.UnboundType = UnboundColumnType.String;
            Base.VisibleIndex = 0;
            Base.Width = 150;
            Base.Visible = true;
            Base.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(Base);

            GridColumn Valor = new GridColumn();
            Valor.FieldName = this.unboundPrefix + "Valor";
            Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            Valor.UnboundType = UnboundColumnType.String;
            Valor.VisibleIndex = 0;
            Valor.Width = 150;
            Valor.Visible = true;
            Valor.OptionsColumn.AllowEdit = false;
            this.gvDetail.Columns.Add(Valor);

            #endregion

        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {           
            this.dtFecha.Enabled = false;
            this.txtTotalNomina.Text = this._totalNomina.ToString();
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
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;

            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSave.Enabled = true;
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

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void chkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {            
            base.chkSeleccionarTodos_CheckedChanged(sender, e);
        }

        /// <summary>
        /// Evento encargado de generar el comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerarCompronbante_Click(object sender, System.EventArgs e)
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
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
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDocument.Columns[this.unboundPrefix + fieldName];
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar de registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            
        }
               
        /// <summary>
        /// Evalua Maestra Banco en la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string colName = this.gvDocument.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            ButtonEdit origin = (ButtonEdit)sender;           
        }

        /// <summary>
        /// Evento generación de campos en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetail_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// Aplica formato a los campos en la grilla de detalle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetail_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Valor")
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);     

                //agrega las novedades al sistema
                //var result = this._bc.AdministrationModel.Nomina_PagoNomina(this.Liquidaciones, this.actividadFlujoID);

                //FormProvider.Master.StopProgressBarThread(this.documentID);
                //if (result.Result == ResultValue.OK)
                //{
                //    this.RefreshDocument();
                //}

                //MessageForm frm = new MessageForm(result);
                //this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PagoNomina", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }           

        #endregion      
       
    }
}
