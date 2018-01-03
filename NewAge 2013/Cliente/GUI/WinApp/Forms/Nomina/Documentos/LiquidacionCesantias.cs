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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionCesantias : DocumentNominaBaseForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_noNominaPreliminar> _liquidaciones = new List<DTO_noNominaPreliminar>();
        DateTime fechaIniLiq;
        DateTime fechaFinLiq;
        List<DTO_noEmpleado> lEmpleados = new List<DTO_noEmpleado>();
        Dictionary<string, DTO_noNominaPreliminar> _repositoryTemporal = new Dictionary<string, DTO_noNominaPreliminar>();
        string actividadFlujoID = string.Empty;
        string resolucion = string.Empty;
        string conceptoCesantias;
        string conceptoInteresesCesantias;
        bool isAnual = true;


        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this._liquidaciones.Clear();
            this.LoadData(true);
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
            this.uc_Empleados.Init(this.LoadEmpleados());
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.dtFechaPago.DateTime = new DateTime(this.dtPeriod.DateTime.Year + 1, 1, 30);
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
            this.uc_Empleados.Refresh();
            this.uc_Empleados.Init(this.LoadEmpleados());
        }

        /// <summary>
        /// Carga el listado de empleados
        /// </summary>
        /// <returns></returns>
        private List<DTO_noEmpleado> LoadEmpleados()
        {
            List<DTO_noEmpleado> lempleados = new List<DTO_noEmpleado>();

            List<DTO_glConsultaFiltro> lfiltros = new List<DTO_glConsultaFiltro>();
            lfiltros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "Estado",
                ValorFiltro = "1",
                OperadorFiltro = OperadorFiltro.Igual,
                OperadorSentencia = "AND"
            });
            lfiltros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "TipoContrato",
                ValorFiltro = "1",
                OperadorFiltro = OperadorFiltro.Igual,
                OperadorSentencia = "OR"
            });
            lfiltros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "TipoContrato",
                ValorFiltro = "2",
                OperadorFiltro = OperadorFiltro.Igual,
                OperadorSentencia = "OR"
            });

            long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.noEmpleado, null, lfiltros, true);
            var ltemp = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.noEmpleado, count, 1, null, lfiltros, true);

            foreach (var item in ltemp)
                lempleados.Add((DTO_noEmpleado)item);

            return lempleados;
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
                if (this._liquidaciones == null || this._liquidaciones.Count == 0)
                {
                    this._liquidaciones = _bc.AdministrationModel.Nomina_NominaPreliminarGet(this.actividadFlujoID, this.dtPeriod.DateTime);
                    this._repositoryTemporal = (from l in this._liquidaciones.AsEnumerable()
                                                select new KeyValuePair<string, DTO_noNominaPreliminar>(l.EmpleadoID.Value, l)
                                 ).Select(x => new { x.Key, x.Value }).ToDictionary(y => y.Key, y => y.Value);
                }

                List<DTO_noNominaPreliminar> results = null;
                if (isAnual)
                {
                    results = (from d in this._repositoryTemporal
                               where d.Key == this.uc_Empleados.empleadoActivo.ID.Value && d.Value.DocLiquidacion.DatoAdd3.Value == ((int)TipoLiqCesantias.Anual).ToString()
                               select d.Value
                                    ).ToList();
                    this.valorColumn.OptionsColumn.AllowEdit = false;
                }
                else
                {
                    results = (from d in this._repositoryTemporal
                               where d.Key == this.uc_Empleados.empleadoActivo.ID.Value && d.Value.DocLiquidacion.DatoAdd3.Value == ((int)TipoLiqCesantias.Parcial).ToString()
                               select d.Value
                                    ).ToList();
                    this.valorColumn.OptionsColumn.AllowEdit = true;
                }
                this.gcDocument.DataSource = results;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.Cesantias;
            this.frmModule = ModulesPrefix.no;

            base.SetInitParameters();


            #region Carga la info de las actividades
            List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.PagoCesantiasAprob);
            if (actividades.Count != 1)
            {
                string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                MessageBox.Show(string.Format(msg, this.documentID.ToString()));
            }
            else
            {
                this.actividadFlujoID = actividades[0];
            }

            #endregion

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            this.conceptoCesantias = this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCesantias);
            this.conceptoInteresesCesantias = this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoInteresCesantias);


            //Asigna las propiedades al documento


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
                #region Header

                this.fechaCorteCesantiasColumn.FieldName = this.unboundPrefix + "FechaCorteCesantias";
                this.fechaCorteCesantiasColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaCorteCesantias");

                this.fechaPagoCesantiasColumn.FieldName = this.unboundPrefix + "FechaPagoCesantias";
                this.fechaPagoCesantiasColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaPagoCesantias");

                this.valorCesantiasColumn.FieldName = this.unboundPrefix + "ValorCesantias";
                this.valorCesantiasColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorCesantias");

                this.valorInteresesCesantiasColumn.FieldName = this.unboundPrefix + "ValorInteresesCesantias";
                this.valorInteresesCesantiasColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorInteresesCesantias");

                this.resolucionColumn.FieldName = this.unboundPrefix + "Resolucion";
                this.resolucionColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Resolucion");

                this.estadoColumn.FieldName = this.unboundPrefix + "Estado";
                this.estadoColumn.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Estado");

                #endregion

                #region Detalle

                this.conceptoNOIDColumn.FieldName = this.unboundPrefix + "ConceptoNOID";
                this.conceptoNOIDColumn.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNOID");

                this.conceptoNODescColumn.FieldName = this.unboundPrefix + "ConceptoNODesc";
                this.conceptoNODescColumn.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNODesc");

                this.diasColumn.FieldName = this.unboundPrefix + "Dias";
                this.diasColumn.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Dias");

                this.baseColumn.FieldName = this.unboundPrefix + "Base";
                this.baseColumn.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Base");

                this.valorColumn.FieldName = this.unboundPrefix + "Valor";
                this.valorColumn.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Valor");

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionCesantias.cs", "AddGridCols"));
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
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Determina los controles a mostrar dependiendo de la forma de liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtTipoLiquidacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioGroup opcion = (RadioGroup)sender;
            if (opcion.SelectedIndex == 0)
            {
                this.lblResolucion.Visible = false;
                this.txtResolucion.Visible = false;
                this.txtResolucion.Text = string.Empty;
                this.uc_Empleados.IsMultipleSeleccion = true;
                this.isAnual = true;
                this.dtFechaPago.DateTime = new DateTime(this.dtPeriod.DateTime.Year + 1, 1, 30);
            }
            if (opcion.SelectedIndex == 1)
            {
                this.uc_Empleados.IsMultipleSeleccion = false;
                this.lblResolucion.Visible = true;
                this.txtResolucion.Visible = true;
                this.isAnual = false;
                this.dtFechaPago.DateTime = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, 30);
            }
            this.RefreshDocument();
            this.LoadData(true);
        }

        #endregion

        #region Eventos Grilla


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

        /// <summary>
        /// Maneja campos en las grillas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        private void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ValorCesantias" || fieldName == "ValorInteresesCesantias")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Maneja campos de controles en la grilla 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Base" || fieldName == "Valor")
            {
                e.RepositoryItem = this.editValue;
            }
        }


        private void gvDetalle_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            var gridDetail = (GridView)sender;

            if (fieldName == "Valor")
            {
                decimal val = Decimal.Parse(gridDetail.GetRowCellValue(e.RowHandle, fieldName).ToString());
                var dto = this._liquidaciones[this.gvDocument.FocusedRowHandle];
                var conceptoNom = ((DTO_noLiquidacionPreliminar)gridDetail.GetRow(e.RowHandle)).ConceptoNOID.Value;
                int numeroDoc = ((DTO_noLiquidacionPreliminar)gridDetail.GetRow(e.RowHandle)).NumeroDoc.Value.Value;

                string msgCaption = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_CaptionUpdate);
                string msgUpdate = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_VerificationUpdate);

                if (this.valideValueChange(dto, conceptoNom, val))
                {
                    if (MessageBox.Show(msgUpdate, msgCaption, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool isCesantias = true;
                        if (conceptoNom == this.conceptoInteresesCesantias)
                            isCesantias = false;

                        this._bc.AdministrationModel.UpdateCesantias(numeroDoc, val, val, isCesantias);
                    }
                }
                this._liquidaciones.Clear();
                this.LoadData(true);
                this.gcDocument.RefreshDataSource();

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
        /// Se ejecuta cuando se selecciona un Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
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
                //Si la liquidación es anual
                if (isAnual)
                {
                    if (this.dtPeriod.DateTime.Month == 12)
                    {
                        this.lEmpleados.Clear();
                        List<DTO_noEmpleado> lTempEmpleados = this.uc_Empleados._empleados;
                        //Liquida Sueldo Empleados
                        foreach (var emp in lTempEmpleados)
                        {
                            if (emp.Visible.Value.Value)
                                lEmpleados.Add(emp);
                        }
                        this.fechaIniLiq = new DateTime(this.dtPeriod.DateTime.Year, 1, 1);
                        this.fechaFinLiq = new DateTime(this.dtPeriod.DateTime.Year, 12, 31);
                    }
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_LiquidacionAnualCesantias));
                        return;
                    }
                }
                //Si la liquidacion es Parcial
                else
                {
                    this.lEmpleados.Clear();
                    lEmpleados.Add(this.uc_Empleados.empleadoActivo);
                    this.fechaIniLiq = new DateTime(this.dtPeriod.DateTime.Year, 1, 1);
                    this.fechaFinLiq = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month - 1,
                                                        DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month - 1)
                                                    );
                }

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = new List<DTO_TxResult>();
                //agrega las novedades al sistema
                if (lEmpleados.Count > 0)
                    results = _bc.AdministrationModel.LiquidarCesantias(this.dtPeriod.DateTime,
                                                                        this.dtFecha.DateTime,
                                                                        this.fechaIniLiq,
                                                                        this.fechaFinLiq,
                                                                        this.dtFechaPago.DateTime,
                                                                        txtResolucion.Text,
                                                                        isAnual ? TipoLiqCesantias.Anual : TipoLiqCesantias.Parcial,
                                                                        this.lEmpleados);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionCesantias.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(this.saveDelegate);
            }
        }

        #endregion

        /// <summary>
        /// Valida el rango del valor modificado para el registro
        /// </summary>
        /// <param name="regLiq">registro de liquidación</param>
        /// <param name="value">nuevo valor</param>
        /// <returns></returns>
        private bool valideValueChange(DTO_noNominaPreliminar regDoc, string concepto, decimal value)
        {
            bool isValid = true;

            if (concepto == this.conceptoCesantias)
            {
                if (value > regDoc.ValorCesantias.Value.Value || value < 0)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_ValidationRange), regDoc.ValorCesantias.Value.Value.ToString()));
                    isValid = false;
                }
                return isValid;
            }
            if (concepto == this.conceptoInteresesCesantias)
            {
                if (value > regDoc.ValorInteresesCesantias.Value.Value || value < 0)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_ValidationRange), regDoc.ValorInteresesCesantias.Value.Value.ToString()));
                    isValid = false;
                }
                return isValid;
            }
            return isValid;
        }

    }
}
