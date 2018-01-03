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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionVacacionesColectivas : DocumentNominaForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noLiquidacionPreliminar> _liquidaciones = null;
        DTO_noEmpleado _empleado = null;
        int diasTomados = 0;
        DateTime periodoLiqIni;
        DateTime periodoLiqFin;
        bool IsFirtsTime = false;
        List<DTO_noEmpleado> lEmpleados = new List<DTO_noEmpleado>();

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
            //Recarga la grilla de novedades
            this.LoadData(true);
        }

        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected override void RefreshGridMethod()
        {
            base.RefreshGridMethod();
            gcDocument.DataSource = null;
            this.lEmpleados = new List<DTO_noEmpleado>();
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this._empleado = this.uc_Empleados.empleadoActivo;
            this.dtFechaIni.DateTime = this.dtPeriod.DateTime;
            this.dtFechaFin.DateTime = this.dtPeriod.DateTime.AddDays(28);
            this.gcDocument.UseEmbeddedNavigator = false;
        }

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado) { }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.Novedades = null;
            this.FieldsEnabled(true);
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
                if (this._empleado != null)
                {
                    this._liquidaciones = _bc.AdministrationModel.Nomina_LiquidacionPreliminarGetAll(AppDocuments.Vacaciones, this.dtPeriod.DateTime, this._empleado);
                    this.gcDocument.DataSource = this._liquidaciones;
                    decimal devengos = 0, deducciones = 0, total = 0;

                    foreach (var detalle in this._liquidaciones)
                    {
                        if (detalle.Valor.Value > 0)
                            devengos = devengos + detalle.Valor.Value.Value;

                        if (detalle.Valor.Value < 0)
                            deducciones = deducciones + detalle.Valor.Value.Value;
                    }

                    total = devengos + deducciones;

                    this.txtTotalDevengos.Text = devengos.ToString();
                    this.txtTotalDeducciones.Text = deducciones.ToString();
                    this.txtTotalPago.Text = total.ToString();
                }
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.Vacaciones;

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
                #region Detalle Grilla

                GridColumn numeroDoc = new GridColumn();
                numeroDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumeroDoc");
                numeroDoc.UnboundType = UnboundColumnType.Integer;
                numeroDoc.VisibleIndex = 0;
                numeroDoc.Width = 100;
                numeroDoc.Visible = true;
                numeroDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numeroDoc);


                GridColumn conceptoNOM = new GridColumn();
                conceptoNOM.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
                conceptoNOM.UnboundType = UnboundColumnType.String;
                conceptoNOM.VisibleIndex = 0;
                conceptoNOM.Width = 150;
                conceptoNOM.Visible = true;
                conceptoNOM.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOM);

                GridColumn conceptoNODesc = new GridColumn();
                conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
                conceptoNODesc.UnboundType = UnboundColumnType.String;
                conceptoNODesc.VisibleIndex = 0;
                conceptoNODesc.Width = 400;
                conceptoNODesc.Visible = true;
                conceptoNODesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNODesc);

                GridColumn Dias = new GridColumn();
                Dias.FieldName = this.unboundPrefix + "Dias";
                Dias.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias");
                Dias.UnboundType = UnboundColumnType.Integer;
                Dias.VisibleIndex = 0;
                Dias.Width = 150;
                Dias.Visible = true;
                Dias.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dias);

                GridColumn Base = new GridColumn();
                Base.FieldName = this.unboundPrefix + "Base";
                Base.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Base");
                Base.UnboundType = UnboundColumnType.String;
                Base.VisibleIndex = 0;
                Base.Width = 150;
                Base.Visible = true;
                Base.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Base);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.String;
                Valor.VisibleIndex = 0;
                Valor.Width = 155;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacacionesColectivas.cs", "AddGridCols"));
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
                FormProvider.Master.itemGenerateTemplate.Enabled = true;
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento para el control de fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaIni_EditValueChanged(object sender, EventArgs e)
        {
            if (IsFirtsTime)
            {
                if (this.dtFechaIni.DateTime >= this.dtFechaFin.DateTime)
                {
                    this.dtFechaIni.DateTime = this.dtPeriod.DateTime;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.No_DateStartNotValid));
                }
            }
            this.IsFirtsTime = true;
        }

        /// <summary>
        /// Evento para el control de fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaFin_EditValueChanged(object sender, EventArgs e)
        {
            if (IsFirtsTime)
            {
                if (this.dtFechaIni.DateTime >= this.dtFechaFin.DateTime)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.No_DateStartNotValid));
                }
                else
                {
                    decimal diasCausados = 0;
                    int diasVacaciones = this._bc.AdministrationModel.CalcularDiasVacaciones(
                                                                            this._empleado.ID.Value,
                                                                            this.dtFechaIni.DateTime,
                                                                            this.dtFechaFin.DateTime,
                                                                            out diasCausados);

                    this.txtDiasVac.Text = diasVacaciones.ToString();
                }
            }
            this.IsFirtsTime = true;
        }

        #endregion

        #region Eventos Grilla

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
            if (fieldName == "ProcesadoInd")
            {
                e.RepositoryItem = this.editChkBox;
            }
            if (fieldName == "Valor")
            {
                this.editValue.Mask.EditMask = "c0";
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


        #endregion

        #region Eventos Control Empleados

        /// <summary>
        /// Evento que se ejecuta cuando se selecciona un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
            this._empleado = this.uc_Empleados.empleadoActivo;
            this.LoadData(true);
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
                this.lEmpleados.Clear();
                List<DTO_noEmpleado> lTempEmpleados = this.uc_Empleados._empleados;
                //Liquida Sueldo Empleados
                foreach (var emp in lTempEmpleados)
                {
                    if (emp.Visible.Value.Value)
                        lEmpleados.Add(emp);
                }

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = new List<DTO_TxResult>();
                //agrega las novedades al sistema
                if (lEmpleados.Count > 0)
                    results = _bc.AdministrationModel.LiquidarVacaciones(this.dtPeriod.DateTime, this.dtFecha.DateTime, lEmpleados, this.dtFechaIni.DateTime, this.dtFechaFin.DateTime, Convert.ToInt32(this.txtDiasVac.Text), 0, false, false, this.txtResolucion.Text, this.dtFechaFin.DateTime, dtFechaIni.DateTime, this.dtFechaFin.DateTime, dtFechaIni.DateTime);
                else
                {
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.NOK;
                    result.ResultMessage = DictionaryMessages.Err_No_EmpleadoSelect;
                    results.Add(result);
                }

                FormProvider.Master.StopProgressBarThread(this.documentID);
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                foreach (DTO_TxResult result in results)
                {
                    if (result.Result == ResultValue.NOK)
                        resultsNOK.Add(result);
                }

                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacacionesColectivas.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(this.saveDelegate);
            }
        }

        #endregion




    }
}
