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
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QueryMvtoAuxiliar : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        private int userID = 0;
        private string empresaId;
        //Variables basicas
        private int _statusBarProgress;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private int documentID;
        private ModulesPrefix frmModule;
        private string UnboundPrefix = "Unbound_";

        private DTO_QueryMvtoAuxiliar auxiliarCurrent;
        private List<DTO_QueryMvtoAuxiliar> data = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Progreso de procesos (importación)
        /// </summary>
        protected int StatusBarProgressProgress
        {
            get
            {
                return _statusBarProgress;
            }
            set
            {
                _statusBarProgress = value;
                FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, value });
            }
        }

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public QueryMvtoAuxiliar()
        {
            this.InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this.InitControls();
                this.AddGridCols();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "DocumentForm"));
            }

        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this.documentID = AppQueries.QueryMvtoAuxiliar;
            this.frmModule = ModulesPrefix.co;
            //Inicia las variables del formulario
            this.empresaId = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterComprobante, AppMasters.coComprobante, true, true, true, true);
                this._bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, true);
                this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, true);
                this._bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterLineaPresup, AppMasters.plLineaPresupuesto, true, true, true, false);
                this._bc.InitMasterUC(this.masterConcCargo, AppMasters.coConceptoCargo, true, true, true, false);
                this._bc.InitMasterUC(this.masterDocumento, AppMasters.glDocumento, true, true, true, false);
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTipoBalance, AppMasters.coBalanceTipo, true, true, true, false);

                DateTime periodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                this.dtPeriodoInicio.DateTime = new DateTime(periodo.Year, 1, 1);
                this.dtPeriodoFinal.DateTime = periodo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            GridColumn Fecha = new GridColumn();
            Fecha.FieldName = this.UnboundPrefix + "Fecha";
            Fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
            Fecha.UnboundType = UnboundColumnType.DateTime;
            Fecha.VisibleIndex = 0;
            Fecha.Width = 65;
            Fecha.Visible = true;
            Fecha.OptionsColumn.AllowEdit = false;
            Fecha.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            Fecha.AppearanceCell.Options.UseTextOptions = true;
            this.gvResults.Columns.Add(Fecha);

            GridColumn DocumentoID = new GridColumn();
            DocumentoID.FieldName = this.UnboundPrefix + "DocumentoID";
            DocumentoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocumentoID");
            DocumentoID.UnboundType = UnboundColumnType.String;
            DocumentoID.VisibleIndex = 1;
            DocumentoID.Width = 45;
            DocumentoID.Visible = true;
            DocumentoID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(DocumentoID);

            GridColumn Comprobante = new GridColumn();
            Comprobante.FieldName = this.UnboundPrefix + "Comprobante";
            Comprobante.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Comprobante");
            Comprobante.UnboundType = UnboundColumnType.String;
            Comprobante.VisibleIndex = 2;
            Comprobante.Width = 80;
            Comprobante.Visible = true;
            Comprobante.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(Comprobante);

            GridColumn cuenta = new GridColumn();
            cuenta.FieldName = this.UnboundPrefix + "CuentaID";
            cuenta.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CuentaID");
            cuenta.UnboundType = UnboundColumnType.String;
            cuenta.VisibleIndex = 3;
            cuenta.Width = 70;
            cuenta.Visible = true;
            cuenta.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(cuenta);

            GridColumn TerceroID = new GridColumn();
            TerceroID.FieldName = this.UnboundPrefix + "TerceroID";
            TerceroID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TerceroID");
            TerceroID.UnboundType = UnboundColumnType.String;
            TerceroID.VisibleIndex = 4;
            TerceroID.Width = 80;
            TerceroID.Visible = true;
            TerceroID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(TerceroID);

            GridColumn CentroCostoID = new GridColumn();
            CentroCostoID.FieldName = this.UnboundPrefix + "CentroCostoID";
            CentroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
            CentroCostoID.UnboundType = UnboundColumnType.String;
            CentroCostoID.VisibleIndex = 5;
            CentroCostoID.Width = 70;
            CentroCostoID.Visible = true;
            CentroCostoID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(CentroCostoID);

            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this.UnboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 6;
            ProyectoID.Width = 70;
            ProyectoID.Visible = true;
            ProyectoID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(ProyectoID);

            GridColumn LineaPresupuestoID = new GridColumn();
            LineaPresupuestoID.FieldName = this.UnboundPrefix + "LineaPresupuestoID";
            LineaPresupuestoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPresupuestoID");
            LineaPresupuestoID.UnboundType = UnboundColumnType.String;
            LineaPresupuestoID.VisibleIndex = 7;
            LineaPresupuestoID.Width = 70;
            LineaPresupuestoID.Visible = true;
            LineaPresupuestoID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(LineaPresupuestoID);

            GridColumn ConceptoCargoID = new GridColumn();
            ConceptoCargoID.FieldName = this.UnboundPrefix + "ConceptoCargoID";
            ConceptoCargoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ConceptoCargoID");
            ConceptoCargoID.UnboundType = UnboundColumnType.String;
            ConceptoCargoID.VisibleIndex = 8;
            ConceptoCargoID.Width = 70;
            ConceptoCargoID.Visible = true;
            ConceptoCargoID.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(ConceptoCargoID);

            GridColumn PrefDoc = new GridColumn();
            PrefDoc.FieldName = this.UnboundPrefix + "PrefDoc";
            PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
            PrefDoc.UnboundType = UnboundColumnType.String;
            PrefDoc.VisibleIndex = 9;
            PrefDoc.Width = 70;
            PrefDoc.Visible = true;
            PrefDoc.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(PrefDoc);

            GridColumn Descriptivo = new GridColumn();
            Descriptivo.FieldName = this.UnboundPrefix + "Descriptivo";
            Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            Descriptivo.UnboundType = UnboundColumnType.String;
            Descriptivo.VisibleIndex = 10;
            Descriptivo.Width = 200;
            Descriptivo.Visible = true;
            Descriptivo.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(Descriptivo);

            GridColumn vlrMdaLoc = new GridColumn();
            vlrMdaLoc.FieldName = this.UnboundPrefix + "vlrMdaLoc";
            vlrMdaLoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaLoc");
            vlrMdaLoc.UnboundType = UnboundColumnType.Decimal;
            vlrMdaLoc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            vlrMdaLoc.AppearanceCell.Options.UseTextOptions = true;
            vlrMdaLoc.VisibleIndex = 11;
            vlrMdaLoc.Width = 100;
            vlrMdaLoc.Visible = true;          
            vlrMdaLoc.ColumnEdit = this.editValue;
            vlrMdaLoc.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(vlrMdaLoc);

            GridColumn vlrMdaExt = new GridColumn();
            vlrMdaExt.FieldName = this.UnboundPrefix + "vlrMdaExt";
            vlrMdaExt.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_vlrMdaExt");
            vlrMdaExt.UnboundType = UnboundColumnType.Decimal;
            vlrMdaExt.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            vlrMdaExt.AppearanceCell.Options.UseTextOptions = true;
            vlrMdaExt.VisibleIndex = 11;
            vlrMdaExt.Width = 70;
            vlrMdaExt.Visible = true;          
            vlrMdaExt.ColumnEdit = this.editValue;
            vlrMdaExt.OptionsColumn.AllowEdit = false;
            this.gvResults.Columns.Add(vlrMdaExt);

            //Ver
            GridColumn viewDoc = new GridColumn();
            viewDoc.FieldName = this.UnboundPrefix + "ViewDoc";
            viewDoc.OptionsColumn.ShowCaption = false;
            viewDoc.UnboundType = UnboundColumnType.String;
            viewDoc.Width = 50;
            viewDoc.ColumnEdit = this.editLink;
            viewDoc.VisibleIndex = 12;
            viewDoc.Visible = true;
            viewDoc.OptionsColumn.AllowEdit = true;
            this.gvResults.Columns.Add(viewDoc);

            this.gvResults.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.gvResults.OptionsView.ColumnAutoWidth = true;
        }

        /// <summary>
        /// Valida el Header
        /// </summary>
        /// <returns></returns>
        private string ValidateHeader()
        {
            string camposObligatorios = string.Empty;

            if (string.IsNullOrEmpty(this.txtDocumentoCOM.Text))
            {
                if (!this.masterComprobante.ValidID && !this.masterCuenta.ValidID && !this.masterTercero.ValidID)
                    camposObligatorios = "\n" + this.masterComprobante.LabelRsx + "\n" + this.masterCuenta.LabelRsx + "\n" + this.masterTercero.LabelRsx + "\n";

            }
            return camposObligatorios;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemExport.Visible = true;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemExport.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al salir de un control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;           
        }

        /// <summary>
        /// Evento que se ejecuta al salir de un control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtDocNro_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvResults_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length);

            if (fieldName == "Valor" || fieldName == "Base")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvResults_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.UnboundPrefix.Length);

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
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvResults_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
              if (e.Column.FieldName == this.UnboundPrefix + "ViewDoc")
                  e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "gvResults_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvResults_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.auxiliarCurrent = (DTO_QueryMvtoAuxiliar)this.gvResults.GetRow(e.FocusedRowHandle);
            if (this.auxiliarCurrent != null)
            {
                this.txtCuentaDesc.EditValue = this.auxiliarCurrent.CuentaDes.Value;
                this.txtTerceroDesc.EditValue = this.auxiliarCurrent.TerceroDes.Value;
                this.txtCentroDesc.EditValue = this.auxiliarCurrent.CentroCtoDes.Value;
                this.txtLineaDesc.EditValue = this.auxiliarCurrent.LineaPresDes.Value;
                this.txtConceptoDesc.EditValue = this.auxiliarCurrent.ConceptoDes.Value;
                this.txtProyectoDesc.EditValue = this.auxiliarCurrent.ProyectoDes.Value;
                this.txtDocDesc.EditValue = this.auxiliarCurrent.DocumentoDes.Value;
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvResults.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                long numDoc = this.data[fila].IdentificadorTR.Value.HasValue && this.data[fila].IdentificadorTR.Value != 0 ? this.data[fila].IdentificadorTR.Value.Value : this.data[fila].NumeroDoc.Value.Value;
                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(Convert.ToInt32(numDoc));
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "editLink_Click"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para busquedas
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                this.gvResults.PostEditor();
                string camposObligatorios = this.ValidateHeader();
                if (string.IsNullOrEmpty(camposObligatorios))
                {
                    DTO_QueryMvtoAuxiliar filter = new DTO_QueryMvtoAuxiliar();
                    filter.CuentaID.Value = this.masterCuenta.Value;
                    filter.TerceroID.Value = this.masterTercero.Value;
                    filter.CentroCostoID.Value = this.masterCentroCosto.Value;
                    filter.ProyectoID.Value = this.masterProyecto.Value;
                    filter.LineaPresupuestoID.Value = this.masterLineaPresup.Value;
                    filter.ConceptoCargoID.Value = this.masterConcCargo.Value;
                    filter.DocumentoID.Value = this.masterDocumento.ValidID ? Convert.ToInt32(this.masterDocumento.Value) : filter.DocumentoID.Value;
                    filter.PrefijoCOM.Value = this.masterPrefijo.Value;
                    filter.BalanceTipoID.Value = this.masterTipoBalance.Value;
                    filter.DocumentoNro.Value = !string.IsNullOrEmpty(this.txtDocNro.Text) ? Convert.ToInt32(this.txtDocNro.EditValue) : filter.DocumentoNro.Value;
                    filter.ComprobanteID.Value = this.masterComprobante.Value;
                    filter.ComprobanteNro.Value = !string.IsNullOrEmpty(this.txtCompNro.Text) ? Convert.ToInt32(this.txtCompNro.EditValue) : filter.ComprobanteNro.Value;
                    filter.DocumentoCOM.Value = this.txtDocumentoCOM.Text;
                    this.data = _bc.AdministrationModel.Comprobante_GetAuxByParameter(this.dtPeriodoInicio.DateTime, this.dtPeriodoFinal.DateTime, filter);
                    this.data = this.data.OrderByDescending(x => x.NumeroDoc.Value).ToList();
                    this.gcResults.DataSource = this.data;
                    this.gcResults.RefreshDataSource();
                    if (this.data.Count == 0)
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
                }
                else
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Py_CamposObligatorios), camposObligatorios));


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "TBSearch"));
            }
        }

        /// <summary>
        /// Boton para actualizar las busquedas
        /// </summary>
        public override void TBUpdate()
        {
            if (this.data != null)
            {
                string msgTitleSearch = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_GetData));
                string msgNewSearch = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewSearchClean);

                if (MessageBox.Show(msgNewSearch, msgTitleSearch, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.masterCuenta.Value = string.Empty;
                    this.masterConcCargo.Value = string.Empty;
                    this.masterLineaPresup.Value = string.Empty;
                    this.masterDocumento.Value = string.Empty;
                    this.masterPrefijo.Value = string.Empty;
                    this.masterProyecto.Value = string.Empty;
                    this.masterTercero.Value = string.Empty;
                    this.masterCentroCosto.Value = string.Empty;

                    this.data = null;
                    this.gcResults.DataSource = null;
                    this.gcResults.RefreshDataSource();
                }
            }
            else
            {

                this.masterCuenta.Value = string.Empty;
                this.masterConcCargo.Value = string.Empty;
                this.masterLineaPresup.Value = string.Empty;
                this.masterDocumento.Value = string.Empty;
                this.masterPrefijo.Value = string.Empty;
                this.masterProyecto.Value = string.Empty;
                this.masterTercero.Value = string.Empty;
                this.masterCentroCosto.Value = string.Empty;
            }
        }

        /// <summary>
        /// Boton para actualizar las busquedas
        /// </summary>
        public override void TBNew()
        {
            this.data = new List<DTO_QueryMvtoAuxiliar>();
            this.gcResults.DataSource = this.data;
            this.masterCuenta.Value = string.Empty;
            this.masterTercero.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterConcCargo.Value = string.Empty;
            this.masterLineaPresup.Value = string.Empty;
            this.masterDocumento.Value = string.Empty;
            this.masterPrefijo.Value = string.Empty;
            this.txtDocNro.EditValue = string.Empty;

            this.txtCuentaDesc.EditValue = string.Empty;
            this.txtTerceroDesc.EditValue = string.Empty;
            this.txtCentroDesc.EditValue = string.Empty;
            this.txtLineaDesc.EditValue = string.Empty;
            this.txtConceptoDesc.EditValue = string.Empty;
            this.txtProyectoDesc.EditValue = string.Empty;
            this.txtDocDesc.EditValue = string.Empty;
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.gvResults.DataRowCount > 0)
                {
                    DataTableOperations tableOp = new DataTableOperations();

                    List<DTO_ExportMvtoAuxiliar> tmp = new List<DTO_ExportMvtoAuxiliar>();
                    foreach (DTO_QueryMvtoAuxiliar mvto in this.data)
                    {
                        DTO_ExportMvtoAuxiliar ex = new DTO_ExportMvtoAuxiliar();
                        ex.Fecha.Value = mvto.Fecha.Value;
                        ex.DocumentoID.Value = mvto.DocumentoID.Value;
                        ex.Comprobante = mvto.Comprobante;
                        ex.CuentaID.Value = mvto.CuentaID.Value;
                        ex.TerceroID.Value = mvto.TerceroID.Value;
                        ex.CentroCostoID.Value = mvto.CentroCostoID.Value;
                        ex.ProyectoID.Value = mvto.ProyectoID.Value;
                        ex.LineaPresupuestoID.Value = mvto.LineaPresupuestoID.Value;
                        ex.ConceptoCargoID.Value = mvto.ConceptoCargoID.Value;
                        ex.PrefDoc = mvto.PrefDoc;
                        ex.Descriptivo.Value = mvto.Descriptivo.Value;
                        ex.vlrMdaLoc.Value = mvto.vlrMdaLoc.Value;
                        ex.vlrMdaExt.Value = mvto.vlrMdaExt.Value;
                        tmp.Add(ex);
                    }

                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportMvtoAuxiliar), tmp);
                    ReportExcelBase frm = new ReportExcelBase(tableAll,this.documentID);
                    frm.Show();
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QueryMvtoAuxiliar.cs", "TBExport"));
            }
        }

        #endregion
    
    }
}
