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
    public partial class LiquidacionContrato : DocumentNominaForm
    {
        public LiquidacionContrato()
        {
            // InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_noLiquidacionPreliminar> _ldetalle = new List<DTO_noLiquidacionPreliminar>();
        DateTime fechaRetiro;
        Dictionary<string, string> _lcausasRetiro = new Dictionary<string, string>();

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
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.gcDocument.UseEmbeddedNavigator = false;
            this.dtFechaRetiro.DateTime = this.dtPeriod.DateTime;
            this.txtTotal.Properties.ReadOnly = true;

            this._lcausasRetiro.Add(((int)CausaRetiro.Renuncia).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa1"));
            this._lcausasRetiro.Add(((int)CausaRetiro.DespidoUnilateral).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa2"));
            this._lcausasRetiro.Add(((int)CausaRetiro.DespidoJustaCausa).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa3"));
            this._lcausasRetiro.Add(((int)CausaRetiro.TerminacionContrato).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa4"));
            this._lcausasRetiro.Add(((int)CausaRetiro.MuerteTrabajador).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa5"));
            this._lcausasRetiro.Add(((int)CausaRetiro.Jubilacion).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa6"));
            this._lcausasRetiro.Add(((int)CausaRetiro.SustituciónPatronal).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa7"));
            this._lcausasRetiro.Add(((int)CausaRetiro.CambioSalarioIntegral).ToString(), this._bc.GetResource(LanguageTypes.Forms, "84_Causa8"));

            this.lkpCausas.Properties.DisplayMember = "Value";
            this.lkpCausas.Properties.ValueMember = "Key";
            this.lkpCausas.Properties.DataSource = _lcausasRetiro;
            this.lkpCausas.EditValue = ((int)CausaRetiro.Renuncia).ToString();
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
                if (this.uc_Empleados.empleadoActivo != null)
                {
                    this._ldetalle = _bc.AdministrationModel.Nomina_LiquidacionPreliminarGetAll(AppDocuments.LiquidacionContrato, this.dtPeriod.DateTime, this.uc_Empleados.empleadoActivo);
                    this.gcDocument.DataSource = this._ldetalle;
                    this.txtTotal.Text = _ldetalle.Sum(x => x.Valor.Value).ToString();
                }
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.LiquidacionContrato;

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
                numeroDoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_NumeroDoc");
                numeroDoc.UnboundType = UnboundColumnType.Integer;
                numeroDoc.VisibleIndex = 0;
                numeroDoc.Width = 100;
                numeroDoc.Visible = true;
                numeroDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(numeroDoc);


                GridColumn conceptoNOM = new GridColumn();
                conceptoNOM.FieldName = this.unboundPrefix + "ConceptoNOID";
                conceptoNOM.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNOID");
                conceptoNOM.UnboundType = UnboundColumnType.String;
                conceptoNOM.VisibleIndex = 0;
                conceptoNOM.Width = 150;
                conceptoNOM.Visible = true;
                conceptoNOM.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNOM);

                GridColumn conceptoNODesc = new GridColumn();
                conceptoNODesc.FieldName = this.unboundPrefix + "ConceptoNODesc";
                conceptoNODesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_ConceptoNODesc");
                conceptoNODesc.UnboundType = UnboundColumnType.String;
                conceptoNODesc.VisibleIndex = 0;
                conceptoNODesc.Width = 400;
                conceptoNODesc.Visible = true;
                conceptoNODesc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(conceptoNODesc);

                GridColumn Dias = new GridColumn();
                Dias.FieldName = this.unboundPrefix + "Dias";
                Dias.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Dias");
                Dias.UnboundType = UnboundColumnType.Integer;
                Dias.VisibleIndex = 0;
                Dias.Width = 150;
                Dias.Visible = true;
                Dias.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Dias);

                GridColumn Base = new GridColumn();
                Base.FieldName = this.unboundPrefix + "Base";
                Base.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Nomina + "_Base");
                Base.UnboundType = UnboundColumnType.Decimal;
                Base.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Base.AppearanceCell.Options.UseTextOptions = true;
                Base.ColumnEdit = this.editValue;
                Base.VisibleIndex = 0;
                Base.Width = 150;
                Base.Visible = true;
                Base.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Base);

                GridColumn Valor = new GridColumn();
                Valor.FieldName = this.unboundPrefix + "Valor";
                Valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                Valor.UnboundType = UnboundColumnType.Decimal;
                Valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Valor.AppearanceCell.Options.UseTextOptions = true;
                Valor.ColumnEdit = this.editValue;
                Valor.VisibleIndex = 0;
                Valor.Width = 155;
                Valor.Visible = true;
                Valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Valor);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionContrato.cs", "AddGridCols"));
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
            this.fechaRetiro = this.dtFechaRetiro.DateTime;

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
            this.dtFechaRetiro.DateTime = this.dtPeriod.DateTime;
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
                if (MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_EmpleadoLiquidarContrato), this.uc_Empleados.empleadoActivo.ID.Value),
                                    this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.No_EmpleadoLiquidacionContrato), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                    FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                    ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                    FormProvider.Master.ProgressBarThread = new Thread(pth);
                    FormProvider.Master.ProgressBarThread.Start(this.documentID);

                    List<DTO_TxResult> results = new List<DTO_TxResult>();
                    //agrega las novedades al sistema

                    List<DTO_noEmpleado> lempleados = new List<DTO_noEmpleado>();
                    lempleados.Clear();
                    lempleados.Add(this.uc_Empleados.empleadoActivo);

                    results = _bc.AdministrationModel.LiquidarContrato(this.dtPeriod.DateTime, this.dtFecha.DateTime, lempleados, this.fechaRetiro, Convert.ToInt32(this.lkpCausas.EditValue));

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidarContrato.cs", "SaveThread"));
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
