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
    public partial class LiquidacionProvisiones : DocumentNominaForm
    {
        public LiquidacionProvisiones()
        {
            // InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_noProvisionDeta> _liquidaciones = new List<DTO_noProvisionDeta>();
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
            this.gcDocument.UseEmbeddedNavigator = false;
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
            this.gcDocument.DataSource = null;
            FormProvider.Master.itemPrint.Enabled = false;
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
                if (this.uc_Empleados.empleadoActivo != null)
                {
                    this._liquidaciones = this._bc.AdministrationModel.Nomina_ProvisionDeta_Get(this.dtPeriod.DateTime, this.uc_Empleados.empleadoActivo.ContratoNOID.Value.Value);
                    this.gcDocument.DataSource = this._liquidaciones;
                }
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.Provisiones;

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

                GridColumn conceptoNOID = new GridColumn();
                conceptoNOID.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNOID");
                conceptoNOID.UnboundType = UnboundColumnType.String;
                conceptoNOID.VisibleIndex = 1;
                conceptoNOID.Width = 100;
                conceptoNOID.Visible = true;
                conceptoNOID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOID);

                GridColumn conceptoNODesc = new GridColumn();
                conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoNODesc");
                conceptoNODesc.UnboundType = UnboundColumnType.String;
                conceptoNODesc.VisibleIndex = 2;
                conceptoNODesc.Width = 200;
                conceptoNODesc.Visible = true;
                conceptoNODesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNODesc);

                GridColumn vlrConsolidadoINI = new GridColumn();
                vlrConsolidadoINI.FieldName = this.unboundPrefix + "VlrConsolidadoINI";
                vlrConsolidadoINI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrConsolidadoINI");
                vlrConsolidadoINI.UnboundType = UnboundColumnType.Decimal;
                vlrConsolidadoINI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrConsolidadoINI.AppearanceCell.Options.UseTextOptions = true;
                vlrConsolidadoINI.ColumnEdit = this.editValue;
                vlrConsolidadoINI.VisibleIndex = 3;
                vlrConsolidadoINI.Width = 100;
                vlrConsolidadoINI.Visible = true;
                vlrConsolidadoINI.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrConsolidadoINI);

                GridColumn vlrProvisionINI = new GridColumn();
                vlrProvisionINI.FieldName = this.unboundPrefix + "VlrProvisionINI";
                vlrProvisionINI.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrProvisionINI");
                vlrProvisionINI.UnboundType = UnboundColumnType.Decimal;
                vlrProvisionINI.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrProvisionINI.AppearanceCell.Options.UseTextOptions = true;
                vlrProvisionINI.ColumnEdit = this.editValue;
                vlrProvisionINI.VisibleIndex = 4;
                vlrProvisionINI.Width = 100;
                vlrProvisionINI.Visible = true;
                vlrProvisionINI.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrProvisionINI);

                GridColumn vlrProvisionMES = new GridColumn();
                vlrProvisionMES.FieldName = this.unboundPrefix + "VlrProvisionMES";
                vlrProvisionMES.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrProvisionMES");
                vlrProvisionMES.UnboundType = UnboundColumnType.Decimal;
                vlrProvisionMES.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrProvisionMES.AppearanceCell.Options.UseTextOptions = true;
                vlrProvisionMES.ColumnEdit = this.editValue;
                vlrProvisionMES.VisibleIndex = 5;
                vlrProvisionMES.Width = 100;
                vlrProvisionMES.Visible = true;
                vlrProvisionMES.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrProvisionMES);

                GridColumn vlrPagosMES = new GridColumn();
                vlrPagosMES.FieldName = this.unboundPrefix + "VlrPagosMES";
                vlrPagosMES.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrPagosMES");
                vlrPagosMES.UnboundType = UnboundColumnType.Decimal;
                vlrPagosMES.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                vlrPagosMES.AppearanceCell.Options.UseTextOptions = true;
                vlrPagosMES.ColumnEdit = this.editValue;
                vlrPagosMES.VisibleIndex = 6;
                vlrPagosMES.Width = 100;
                vlrPagosMES.Visible = true;
                vlrPagosMES.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(vlrPagosMES);

                GridColumn VlrConsolidadoMES = new GridColumn();
                VlrConsolidadoMES.FieldName = this.unboundPrefix + "VlrConsolidadoMES";
                VlrConsolidadoMES.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrConsolidadoMES");
                VlrConsolidadoMES.UnboundType = UnboundColumnType.Decimal;
                VlrConsolidadoMES.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                VlrConsolidadoMES.AppearanceCell.Options.UseTextOptions = true;
                VlrConsolidadoMES.ColumnEdit = this.editValue;
                VlrConsolidadoMES.VisibleIndex = 7;
                VlrConsolidadoMES.Width = 100;
                VlrConsolidadoMES.Visible = true;
                VlrConsolidadoMES.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(VlrConsolidadoMES);

                GridColumn sueldo = new GridColumn();
                sueldo.FieldName = this.unboundPrefix + "Sueldo";
                sueldo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Sueldo");
                sueldo.UnboundType = UnboundColumnType.Decimal;
                sueldo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                sueldo.AppearanceCell.Options.UseTextOptions = true;
                sueldo.ColumnEdit = this.editValue;
                sueldo.VisibleIndex = 8;
                sueldo.Width = 100;
                sueldo.Visible = true;
                sueldo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(sueldo);

                GridColumn auxilioTransporte = new GridColumn();
                auxilioTransporte.FieldName = this.unboundPrefix + "AuxilioTransporte";
                auxilioTransporte.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_AuxilioTransporte");
                auxilioTransporte.UnboundType = UnboundColumnType.Decimal;
                auxilioTransporte.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                auxilioTransporte.AppearanceCell.Options.UseTextOptions = true;
                auxilioTransporte.ColumnEdit = this.editValue;
                auxilioTransporte.VisibleIndex = 9;
                auxilioTransporte.Width = 100;
                auxilioTransporte.Visible = true;
                auxilioTransporte.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(auxilioTransporte);

                GridColumn baseNeta = new GridColumn();
                baseNeta.FieldName = this.unboundPrefix + "BaseNeta";
                baseNeta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BaseNeta");
                baseNeta.UnboundType = UnboundColumnType.Decimal;
                baseNeta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                baseNeta.AppearanceCell.Options.UseTextOptions = true;
                baseNeta.ColumnEdit = this.editValue;
                baseNeta.VisibleIndex = 10;
                baseNeta.Width = 100;
                baseNeta.Visible = true;
                baseNeta.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(baseNeta);

                GridColumn baseVariable = new GridColumn();
                baseVariable.FieldName = this.unboundPrefix + "BaseVariable";
                baseVariable.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_BaseVariable");
                baseVariable.UnboundType = UnboundColumnType.Decimal;
                baseVariable.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                baseVariable.AppearanceCell.Options.UseTextOptions = true;
                baseVariable.ColumnEdit = this.editValue;
                baseVariable.VisibleIndex = 11;
                baseVariable.Width = 100;
                baseVariable.Visible = true;
                baseVariable.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(baseVariable);

                GridColumn diasProvision = new GridColumn();
                diasProvision.FieldName = this.unboundPrefix + "DiasProvision";
                diasProvision.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasProvision");
                diasProvision.UnboundType = UnboundColumnType.Integer;
                diasProvision.VisibleIndex = 12;
                diasProvision.Width = 50;
                diasProvision.Visible = true;
                diasProvision.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(diasProvision);

                GridColumn diasTrabajados = new GridColumn();
                diasTrabajados.FieldName = this.unboundPrefix + "DiasTrabajados";
                diasTrabajados.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasTrabajados");
                diasTrabajados.UnboundType = UnboundColumnType.Integer;
                diasTrabajados.VisibleIndex = 13;
                diasTrabajados.Width = 50;
                diasTrabajados.Visible = true;
                diasTrabajados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(diasTrabajados);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionProvisiones.cs", "AddGridCols"));
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
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Sueldo" || fieldName == "AuxilioTransporte" || fieldName == "VlrProvisionMES"
                || fieldName == "VlrPagosMES" || fieldName == "VlrConsolidadoINI" || fieldName == "VlrProvisionINI"
                || fieldName == "VlrConsolidadoMES" || fieldName == "BaseVariable" || fieldName == "BaseNeta" || fieldName == "VlrProvisionMES"
                )
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

        #region Eventos Control Emmpleados

        /// <summary>
        /// Evento que se ejecuta cuando se selecciona un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = new List<DTO_TxResult>();
                //agrega las novedades al sistema
                if (lEmpleados.Count > 0)
                    results = _bc.AdministrationModel.LiquidarProvisiones(this.dtPeriod.DateTime, this.dtFecha.DateTime, lEmpleados);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionProvisiones.cs", "SaveThread"));
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
