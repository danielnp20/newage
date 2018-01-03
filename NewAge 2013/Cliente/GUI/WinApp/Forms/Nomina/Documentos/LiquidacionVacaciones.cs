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
using NewAge.DTO.Resultados;
using System.Threading;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionVacaciones : DocumentNominaForm
    {
        public LiquidacionVacaciones()
        {
           // InitializeComponent();
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_noLiquidacionesDocu> _liquidaciones = new List<DTO_noLiquidacionesDocu>();
        DTO_noEmpleado _empleado = null;
        int diasTomados = 0;

        #endregion

        #region Delegados

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
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this._empleado = this.uc_Empleados.empleadoActivo;
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
                    this._liquidaciones = _bc.AdministrationModel.Nomina_GetLiquidacionesDocu(AppDocuments.Vacaciones, this.dtPeriod.DateTime, this._empleado);
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

                GridColumn FechaIni1 = new GridColumn();
                FechaIni1.FieldName = this.unboundPrefix + "FechaIni1";
                FechaIni1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaIni1");
                FechaIni1.UnboundType = UnboundColumnType.DateTime;
                FechaIni1.VisibleIndex = 0;
                FechaIni1.Width = 120;
                FechaIni1.Visible = true;
                FechaIni1.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaIni1);

                GridColumn FechaFin1 = new GridColumn();
                FechaFin1.FieldName = this.unboundPrefix + "FechaFin1";
                FechaFin1.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFin1");
                FechaFin1.UnboundType = UnboundColumnType.DateTime;
                FechaFin1.VisibleIndex = 0;
                FechaFin1.Width = 120;
                FechaFin1.Visible = true;
                FechaFin1.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaFin1);

                GridColumn FechaIni2 = new GridColumn();
                FechaIni2.FieldName = this.unboundPrefix + "FechaIni2";
                FechaIni2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaIni2");
                FechaIni2.UnboundType = UnboundColumnType.DateTime;
                FechaIni2.VisibleIndex = 0;
                FechaIni2.Width = 150;
                FechaIni2.Visible = true;
                FechaIni2.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaIni2);

                GridColumn FechaFin2 = new GridColumn();
                FechaFin2.FieldName = this.unboundPrefix + "FechaFin2";
                FechaFin2.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFin2");
                FechaFin2.UnboundType = UnboundColumnType.DateTime;
                FechaFin2.VisibleIndex = 0;
                FechaFin2.Width = 150;
                FechaFin2.Visible = true;
                FechaFin2.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(FechaFin2);

                GridColumn diasTomados = new GridColumn();
                diasTomados.FieldName = this.unboundPrefix + "Dias1";
                diasTomados.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias1");
                diasTomados.UnboundType = UnboundColumnType.Integer;
                diasTomados.VisibleIndex = 0;
                diasTomados.Width = 110;
                diasTomados.Visible = true;
                diasTomados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(diasTomados);

                GridColumn diasPagados = new GridColumn();
                diasPagados.FieldName = this.unboundPrefix + "Dias2";
                diasPagados.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Dias2");
                diasPagados.UnboundType = UnboundColumnType.Integer;
                diasPagados.VisibleIndex = 0;
                diasPagados.Width = 110;
                diasPagados.Visible = true;
                diasPagados.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(diasPagados);

                GridColumn diasTotal = new GridColumn();
                diasTotal.FieldName = this.unboundPrefix + "Total";
                diasTotal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Total");
                diasTotal.UnboundType = UnboundColumnType.Integer;
                diasTotal.VisibleIndex = 0;
                diasTotal.Width = 110;
                diasTotal.Visible = true;
                diasTotal.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(diasTotal);

                GridColumn resolucion = new GridColumn();
                resolucion.FieldName = this.unboundPrefix + "DatoAdd2";
                resolucion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DatoAdd2");
                resolucion.UnboundType = UnboundColumnType.String;
                resolucion.VisibleIndex = 0;
                resolucion.Width = 150;
                resolucion.Visible = true;
                resolucion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(resolucion);

                GridColumn procesado = new GridColumn();
                procesado.FieldName = this.unboundPrefix + "ProcesadoInd";
                procesado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProcesadoInd");
                procesado.UnboundType = UnboundColumnType.Decimal;
                procesado.VisibleIndex = 0;
                procesado.Width = 87;
                procesado.Visible = true;
                procesado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(procesado);


                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacaciones", "AddGridCols"));
            }
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        protected override void AfterInitialize()
        {
            this.diasTomados = 0;
            if (this._liquidaciones != null && this._liquidaciones.Count > 0)
            {

                foreach (var liq in this._liquidaciones)
                    this.diasTomados += (liq.Dias1.Value.Value + liq.Dias2.Value.Value);
            }

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
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemSave.Visible = false;
            FormProvider.Master.itemNew.Visible = false;
            FormProvider.Master.itemUpdate.Visible = true;
        }

        #endregion

        #region Eventos Controles


        /// <summary>
        /// Trae la información de la liquidación si existe o crea una nueva liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLiquidar_Click(object sender, System.EventArgs e)
        {
            try
            {
                ModalLiquidacionVacaciones frmDetalle = null;
                DTO_noLiquidacionesDocu liqActual = (DTO_noLiquidacionesDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (liqActual == null || liqActual.ProcesadoInd.Value.Value)
                {
                    frmDetalle = new ModalLiquidacionVacaciones(this.dtPeriod.DateTime, this.dtFecha.DateTime, this._empleado, this.diasTomados);
                    frmDetalle.ShowDialog();
                }
                else
                {                   
                    frmDetalle = new ModalLiquidacionVacaciones(this.dtPeriod.DateTime,this._empleado, liqActual, this.diasTomados);
                    frmDetalle.ShowDialog();
                }

                this.LoadData(true);
                this.diasTomados = 0;

                if (this._liquidaciones != null && this._liquidaciones.Count > 0)
                {
                    foreach (var liq in this._liquidaciones)
                        this.diasTomados += (liq.Dias1.Value.Value + liq.Dias2.Value.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacaciones", "btnLiquidar_Click"));
            }
        }

        /// <summary>
        /// Trae la información de la liquidación si existe o crea una nueva liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                ModalLiquidacionVacaciones frmDetalle = null;
                DTO_noLiquidacionesDocu liqActual = (DTO_noLiquidacionesDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (liqActual != null)
                {
                    frmDetalle = new ModalLiquidacionVacaciones(this.dtPeriod.DateTime, this._empleado, liqActual, this.diasTomados, true);
                    frmDetalle.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacaciones", "btnView_Click"));
            }
        }

        /// <summary>
        /// Trae la información de la liquidación si existe o crea una nueva liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewLiq_Click(object sender, EventArgs e)
        {
            try
            {
                ModalLiquidacionVacaciones frmDetalle = null;
                DTO_noLiquidacionesDocu liqActual = (DTO_noLiquidacionesDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (liqActual == null || liqActual.ProcesadoInd.Value.Value)
                {
                    frmDetalle = new ModalLiquidacionVacaciones(this.dtPeriod.DateTime, this.dtFecha.DateTime, this._empleado, this.diasTomados);
                    frmDetalle.ShowDialog();
                }                

                this.LoadData(true);
                this.diasTomados = 0;

                if (this._liquidaciones != null && this._liquidaciones.Count > 0)
                {
                    foreach (var liq in this._liquidaciones)
                        this.diasTomados += (liq.Dias1.Value.Value + liq.Dias2.Value.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacaciones", "btnLiquidar_Click"));
            }
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
            try
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
            catch
            {
                throw;
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

        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                base.gvDocument_FocusedRowChanged(sender, e);

                DTO_noLiquidacionesDocu liqActual = (DTO_noLiquidacionesDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (liqActual != null && liqActual.ProcesadoInd.Value.Value)
                    this.btnLiquidar.Enabled = false;
                else if (liqActual != null && !liqActual.ProcesadoInd.Value.Value)
                    this.btnLiquidar.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public override void TBUpdate()
        {
            base.TBUpdate();
            this.LoadData(true);
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
            try
            {
                this._empleado = this.uc_Empleados.empleadoActivo;
                this.LoadData(true);
                this.diasTomados = 0;

                DTO_noLiquidacionesDocu liqActual = (DTO_noLiquidacionesDocu)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                if (liqActual != null && liqActual.ProcesadoInd.Value.Value)
                    this.btnLiquidar.Enabled = false;
                else if (liqActual != null && !liqActual.ProcesadoInd.Value.Value)
                    this.btnLiquidar.Enabled = true;
                else
                    this.btnLiquidar.Enabled = true;

                if (this._liquidaciones != null && this._liquidaciones.Count > 0)
                {
                    foreach (var liq in this._liquidaciones)
                        this.diasTomados += (liq.Dias1.Value.Value + liq.Dias2.Value.Value);

                    if (!this._liquidaciones.Any(x => !x.ProcesadoInd.Value.Value))//Valida si debe crear una liq nueva
                        this.btnNewLiq.Visible = true;
                    else
                        this.btnNewLiq.Visible = false;
                }
                else
                    this.btnNewLiq.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionVacaciones", "uc_Empleados_SelectRowEmpleado_Click"));
            }
        }

        #endregion

    }
}
