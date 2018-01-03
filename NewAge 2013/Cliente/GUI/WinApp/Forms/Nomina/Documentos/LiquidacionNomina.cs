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
using NewAge.Reports;
using DevExpress.XtraReports.UI;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionNomina : DocumentNominaForm
    {
        public LiquidacionNomina()
        {
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private List<DTO_noLiquidacionPreliminar> _ldetalle = null;
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


        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.gcDocument.UseEmbeddedNavigator = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);

        }


        /// <summary>
        /// lista las novedades del empleado seleccionado
        /// </summary>
        /// <returns></returns>
        private List<DTO_noLiquidacionPreliminar> GetNovedades()
        {
            if (this.uc_Empleados.empleadoActivo != null)
            {
                this._ldetalle = _bc.AdministrationModel.Nomina_LiquidacionPreliminarGetAll(AppDocuments.Nomina, this.dtPeriod.DateTime, this.uc_Empleados.empleadoActivo);

                decimal devengos = 0, deducciones = 0, total = 0;

                foreach (var detalle in _ldetalle)
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
            return _ldetalle;
        }

        /// <summary>
        /// Carga el detalle de la novedad
        /// </summary>
        /// <param name="dto"></param>
        private void LoadDetailNovedad(DTO_noLiquidacionPreliminar dto)
        {

        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this.lEmpleados = new List<DTO_noEmpleado>();
        }


        /// <summary> 
        /// Validaciones Propias del Documento 81 de Nomina
        /// </summary>
        private DTO_TxResult ValidacionNomina(DTO_noEmpleado emp)
        {
            var estadoEmpl = this._bc.AdministrationModel.Nomina_GetEstadoLiquidaciones(emp.ID.Value);

            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            if (estadoEmpl.EnVacaciones.Value.Value == 1)
            {
                result.Result = ResultValue.NOK;
                //validacion.EmpleadoID.Value = emp.ID.Value;
                //validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                //validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_EstadoVacaciones");
                //validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionVacaciones");
            }

            //Si la liquidación de Nomina ya esta aprobada
            if (estadoEmpl.EstadoLiqNomina.Value == (byte)EstadoDocControl.Aprobado)
            {
                result.Result = ResultValue.NOK;
                //validacion.EmpleadoID.Value = emp.ID.Value;
                //validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                //validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_EstadoLiqAprobada");
                //validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionLiqAprobada");
            }

            //Si la liquidación de Vacaciones Preliminar
            if (estadoEmpl.EstadoLiqVacaciones.Value == (byte)EstadoDocControl.ParaAprobacion)
            {
                result.Result = ResultValue.NOK;
                //validacion.EmpleadoID.Value = emp.ID.Value;
                //validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                //validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_EstadoLiqVacaciones");
                //validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionLiqVacaciones");
            }
            return result;
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
                var detalle = this.GetNovedades();
                if (detalle != null && detalle.Count() > 0)
                {
                    this.gcDocument.DataSource = detalle;
                }
                else
                    this.gcDocument.DataSource = null;
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
            this.documentID = AppDocuments.Nomina;

            base.SetInitParameters();

            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;
            this.InitControls();
            this.AddGridCols();
            this.AfterInitialize();
            this.LoadData(true);

        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Prenomina

                GridColumn numeroDoc = new GridColumn();
                numeroDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_NumeroDoc");
                numeroDoc.UnboundType = UnboundColumnType.Integer;
                numeroDoc.VisibleIndex = 0;
                numeroDoc.Width = 100;
                numeroDoc.Visible = false;
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
                Dias.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                Dias.AppearanceCell.Options.UseTextOptions = true;
                Dias.ColumnEdit = this.editCant;
                Dias.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dias);

                GridColumn Base = new GridColumn();
                Base.FieldName = this.unboundPrefix + "Base";
                Base.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Base");
                Base.UnboundType = UnboundColumnType.String;
                Base.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Base.AppearanceCell.Options.UseTextOptions = true;
                Base.VisibleIndex = 0;
                Base.Width = 150;
                Base.Visible = true;
                Base.ColumnEdit = this.editValue;
                Base.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Base);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.String;
                Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor.AppearanceCell.Options.UseTextOptions = true;
                Valor.VisibleIndex = 0;
                Valor.Width = 155;
                Valor.Visible = true;
                Valor.ColumnEdit = this.editValue;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);


                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionNomina.cs", "AddGridCols"));
            }
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

            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;

            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
            FormProvider.Master.itemSave.ToolTipText = this._bc.GetResource(LanguageTypes.Forms, "29001_Liquidar");

            if (this.uc_Empleados._empleados == null || this.uc_Empleados._empleados.Count() == 0)
            {
                FormProvider.Master.itemSave.Enabled = false;
                FormProvider.Master.itemNew.Enabled = false;
                FormProvider.Master.itemDelete.Enabled = false;
            }
        }

        #endregion

        #region Eventos Header


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
            ////Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            //string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            //if (fieldName == "Valor")
            //{
            //    this.editValue.Mask.EditMask = "c0";
            //    e.RepositoryItem = this.editValue;
            //}
        }

        /// <summary>
        /// Maneja el cambio de registros en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                int index = e.FocusedRowHandle;
                if (index >= 0)
                {
                    if (_ldetalle != null && _ldetalle.Count > 0)
                    {
                        if (index > this._ldetalle.Count)
                            index = this._ldetalle.Count - 1;

                        DTO_noLiquidacionPreliminar novedad = _ldetalle[index];
                        this.LoadDetailNovedad(novedad);
                    }
                    else
                    {
                        this.RefreshDocument();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionNomina.cs", "gvDocument_FocusedRowChanged"));
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
        /// Funciona para el eliminación de uno ó mas documentos Preliquidados
        /// </summary>
        public override void TBDelete()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            this.lEmpleados.Clear();
            List<DTO_noEmpleado> lTempEmpleados = this.uc_Empleados._empleados;
            //Liquida Sueldo Empleados
            foreach (var emp in lTempEmpleados)
            {
                if (emp.Visible.Value.Value)
                    lEmpleados.Add(emp);
            }

            if (this.lEmpleados != null || this.lEmpleados.Count > 0)
            {


            }
            else
            {
                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.Err_No_EmpleadoSelect;
            }

            MessageForm frm = new MessageForm(result);
            this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
        }

        #endregion

        #region Eventos Control Empleados

        // <summary>
        // Evento que se ejecuta cuando se selecciona un empleado
        // </summary>
        // <param name="sender"></param>
        // <param name="e"></param>
        private void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
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
                this.lEmpleados.Clear();
                List<DTO_noEmpleado> lTempEmpleados = this.uc_Empleados._empleados;
                //Liquida Sueldo Empleados
                foreach (var emp in lTempEmpleados)
                {
                    if (emp.Visible.Value.Value)
                        lEmpleados.Add(emp);
                }

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { AppDocuments.Nomina, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = new List<DTO_TxResult>();
                ModalNominaValidacion validateForm = new ModalNominaValidacion(lEmpleados, AppDocuments.Nomina);
                if (validateForm.HayValidaciones)
                {
                    validateForm.ShowDialog();
                    if (!validateForm.Continuar)
                    {
                        return;
                    }
                    else
                    {
                        lEmpleados = validateForm.Empleados;
                    }
                }

                //agrega las novedades al sistema
                if (lEmpleados.Count > 0)
                    results = _bc.AdministrationModel.LiquidarNomina(this.dtPeriod.DateTime, this.dtFecha.DateTime, lEmpleados);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionNomina.cs", "SaveThread"));
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
