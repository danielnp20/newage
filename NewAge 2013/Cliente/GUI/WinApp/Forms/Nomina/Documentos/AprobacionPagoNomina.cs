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
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class AprobacionPagoNomina : DocumentNominaAprobacionForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private decimal _totalNomina = 0;
        private decimal _totalPagados = 0;
        private decimal _totalAPagar = 0;
        private List<DTO_noNominaPreliminar> _liquidaciones = null;
        private bool IsFirstTime = false;
        Dictionary<int, List<DTO_noNominaPreliminar>> _repositoryTemp = new Dictionary<int,List<DTO_noNominaPreliminar>>();

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this._repositoryTemp.Remove(this.documentID);
            this.RefreshDocument();
            this.LoadData();
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

        }

        /// <summary>
        /// Carga la Informacion del Combo de Documentos
        /// </summary>
        protected override void LookUpDocumentosDataSource()
        {
            DTO_glDocumento dtoDoc = null;
            this.documentos = new Dictionary<string, string>();
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoNominaAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoVacacionesAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoPrimaAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoCesantiasAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, AppDocuments.PagoLiqContratoAprob.ToString(), true);
            this.documentos.Add(dtoDoc.ID.Value, dtoDoc.Descriptivo.Value);
            
            base.LookUpDocumentosDataSource();
        
        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.txtTotalNomina.Text = string.Empty;
            this.txtTotalAPagar.Text = string.Empty;
            this.txtTotalPagados.Text = string.Empty;
            this.txtTotalPendientes.Text = string.Empty;
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
                if (this._repositoryTemp.Count == 0)
                {
                    this.Liquidaciones = this._bc.AdministrationModel.Nomina_NominaPreliminarGet(this.actividadFlujoID, this.dtPeriod.DateTime);
                    this._repositoryTemp.Add(this.documentID, this._liquidaciones);
                }
                else
                {
                    if (!this._repositoryTemp.Any(x => x.Key == this.documentID))
                    {
                        this.Liquidaciones = this._bc.AdministrationModel.Nomina_NominaPreliminarGet(this.actividadFlujoID, this.dtPeriod.DateTime);
                        if(this.Liquidaciones.Count > 0)
                            this._repositoryTemp.Add(this.documentID, this._liquidaciones);
                    }
                    else
                        this.Liquidaciones = (from r in this._repositoryTemp
                                              where r.Key == this.documentID
                                              select r.Value
                                             ).FirstOrDefault();
                }
                this.gcDocument.DataSource = this.Liquidaciones;
                this.gcDocument.RefreshDataSource();
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
            this.documentID = AppDocuments.PagoNominaAprob;

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
            empleadoName.Width = 300;
            empleadoName.Visible = true;
            empleadoName.OptionsColumn.AllowEdit = false;
            this.gvDocument.Columns.Add(empleadoName);


            GridColumn observacion = new GridColumn();
            observacion.FieldName = this.unboundPrefix + "Observacion";
            observacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
            observacion.UnboundType = UnboundColumnType.String;
            observacion.VisibleIndex = 0;
            observacion.Width = 440;
            observacion.Visible = true;
            observacion.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(observacion);

            GridColumn valorPago = new GridColumn();
            valorPago.FieldName = this.unboundPrefix + "Valor";
            valorPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
            valorPago.UnboundType = UnboundColumnType.Decimal;
            valorPago.VisibleIndex = 0;
            valorPago.Width = 200;
            valorPago.Visible = true;
            valorPago.OptionsColumn.AllowEdit = true;
            this.gvDocument.Columns.Add(valorPago);


           
            #endregion

            #region Detalle

            GridColumn conceptoNOID = new GridColumn();
            conceptoNOID.FieldName = this.unboundPrefix + "ConceptoNOID";
            conceptoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNOID");
            conceptoNOID.UnboundType = UnboundColumnType.String;
            conceptoNOID.VisibleIndex = 0;
            conceptoNOID.Width = 100;
            conceptoNOID.Visible = true;
            conceptoNOID.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(conceptoNOID);

            GridColumn conceptoNODesc = new GridColumn();
            conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
            conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNODesc");
            conceptoNODesc.UnboundType = UnboundColumnType.String;
            conceptoNODesc.VisibleIndex = 0;
            conceptoNODesc.Width = 200;
            conceptoNODesc.Visible = true;
            conceptoNODesc.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(conceptoNODesc);

            GridColumn dias = new GridColumn();
            dias.FieldName = this.unboundPrefix + "Dias";
            dias.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Dias");
            dias.UnboundType = UnboundColumnType.Decimal;
            dias.VisibleIndex = 0;
            dias.Width = 100;
            dias.Visible = true;
            dias.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(dias);

            GridColumn Base = new GridColumn();
            Base.FieldName = this.unboundPrefix + "Base";
            Base.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Base");
            Base.UnboundType = UnboundColumnType.Decimal;
            Base.VisibleIndex = 0;
            Base.Width = 150;
            Base.Visible = true;
            Base.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(Base);

            GridColumn valor = new GridColumn();
            valor.FieldName = this.unboundPrefix + "Valor";
            valor.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Valor");
            valor.UnboundType = UnboundColumnType.Decimal;
            valor.VisibleIndex = 0;
            valor.Width = 150;
            valor.Visible = true;
            valor.OptionsColumn.AllowEdit = false;
            this.gvPreliminar.Columns.Add(valor);


            #endregion
           
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {           
            this.dtFecha.Enabled = false;
            if (this.Liquidaciones != null && this.Liquidaciones.Count > 0)
            {
                this._totalNomina = this.Liquidaciones.Sum(x => x.Valor.Value.Value);
            }
            this.txtTotalNomina.Text = this._totalNomina.ToString();
            this.txtTotalPagados.Text = this._totalPagados.ToString();
            this.txtTotalAPagar.Text = this._totalAPagar.ToString();
            this.txtTotalPendientes.Text = (this._totalNomina - this._totalPagados).ToString();
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
            if (this.IsFirstTime)
            {
                base.lookUpDocumentos_EditValueChanged(sender, e);

                this.LoadDocumentInfo(true);
                this.LoadData();
                this.AfterInitialize();
            }
            else
            {
                this.IsFirstTime = true;
            }
        }

        /// <summary>
        /// Selecciona todos los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
              

        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvPreliminar_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            base.gvPreliminar_CustomUnboundColumnData(sender, e);
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
            base.gvPreliminar_CustomRowCellEdit(sender, e);
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
                if (this.Liquidaciones.Count == 0)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_No_DocLiquidacionPendientes));
                }
                else
                {
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);
                    //agrega las novedades al sistema
                    var result = this._bc.AdministrationModel.Nomina_AprobarLiquidacion(this.Liquidaciones, this.actividadFlujoID);

                    FormProvider.Master.StopProgressBarThread(this.documentID);

                    #region Genera Reporte
                    DTO_TxResult res = (DTO_TxResult)result;
                    //Crea el reporte de Boleta Pago Nomina
                    if (res.Result == ResultValue.OK)// && this.documentID == AppDocuments.PagoNominaAprob)
                    {
                        
                        foreach (DTO_noNominaPreliminar liq in this.Liquidaciones.FindAll(x=>x.Seleccionar.Value.Value))
                            this._bc.AdministrationModel.Report_No_BoletaPago(liq.EmpleadoID.Value,liq.PeriodoID.Value.Value.Month, liq.PeriodoID.Value.Value.Year,AppDocuments.Nomina.ToString(),string.Empty, ExportFormatType.pdf,liq.NumeroDoc.Value);              
                    } 
                    #endregion

                    MessageForm frm = new MessageForm(result);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionPagoNomina.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(saveDelegate);            
            }
        }           

        #endregion      

        #region Propiedades

        /// <summary>
        /// Listado de Empleados para pagos
        /// </summary>
        protected virtual List<DTO_noNominaPreliminar> Liquidaciones
        {
            get { return this._liquidaciones; }
            set { this._liquidaciones = value; }
        }

        #endregion
    }
}
