

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Globalization;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class QuerySobreEjecucion  : FormWithToolbar
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();

        private List<DTO_plSobreEjecucion > _cierresQuery;
        private ModulesPrefix _frmModule;
        private FormTypes _frmType = FormTypes.Query;
        private string _frmName;
        private string _unboundPrefix = "Unbound_";
        private int _documentID;

        private string _monedaLocal = string.Empty;
        private string _monedaExtr = string.Empty;

        #endregion

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public QuerySobreEjecucion ()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();
                this._frmName = this._bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                // Trae la fuente de datos y los filtra
                this.AddGridCols();
                this.InitControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "QuerySobreEjecucion "));
            }
        }

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppQueries.QuerySobreEjecucion;
            this._frmModule = ModulesPrefix.pl;
        }

        /// <summary>
        /// Inicia los controlesdel form
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._monedaExtr = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                //Carga controles de maestras
                this._bc.InitMasterUC(this.masterAreaAprob, AppMasters.glAreaFuncional, false, true, false, false);

                Dictionary<string, string> dicEstado = new Dictionary<string, string>();
                dicEstado.Add("0", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_All));
                dicEstado.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Estado_PorSolicitar));
                dicEstado.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Estado_PorRevisar));
                dicEstado.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Estado_PorAprobar));
                dicEstado.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Estado_Aprobada));
                dicEstado.Add("5", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_Estado_Negada));
                this.cmbEstado.Properties.DataSource = dicEstado;
                this.cmbEstado.EditValue = 1;

                this.masterAreaAprob.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "InitControls"));
            }
        }

        /// <summary>
        /// Inicia las columnas de las grillas
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columnas de Grilla principal

                #region Columnas visibles

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this._unboundPrefix + "ProyectoID";
                ProyectoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 60;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);

                //AreaAprobacion
                GridColumn AreaAprobacion = new GridColumn();
                AreaAprobacion.FieldName = this._unboundPrefix + "AreaAprobacion";
                AreaAprobacion.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_AreaAprobacion");
                AreaAprobacion.UnboundType = UnboundColumnType.String;
                AreaAprobacion.VisibleIndex = 1;
                AreaAprobacion.Width = 60;
                AreaAprobacion.Visible = true;
                AreaAprobacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(AreaAprobacion);

                //LineaPresupuesto
                GridColumn LineaPresupuesto = new GridColumn();
                LineaPresupuesto.FieldName = this._unboundPrefix + "LineaPresupuestoID";
                LineaPresupuesto.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_LineaPresupuestoID");
                LineaPresupuesto.UnboundType = UnboundColumnType.String;
                LineaPresupuesto.VisibleIndex = 2;
                LineaPresupuesto.Width = 60;
                LineaPresupuesto.Visible = true;
                LineaPresupuesto.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(LineaPresupuesto);

                //Descriptivo
                GridColumn Descriptivo = new GridColumn();
                Descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                Descriptivo.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                Descriptivo.UnboundType = UnboundColumnType.String;
                Descriptivo.VisibleIndex = 3;
                Descriptivo.Width = 120;
                Descriptivo.Visible = true;
                Descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Descriptivo);

                //MonedaOrigen
                GridColumn MonedaOrigen = new GridColumn();
                MonedaOrigen.FieldName = this._unboundPrefix + "MonedaOrigen";
                MonedaOrigen.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_MonedaOrigen");
                MonedaOrigen.UnboundType = UnboundColumnType.Integer;
                MonedaOrigen.VisibleIndex = 4;
                MonedaOrigen.Width = 40;
                MonedaOrigen.Visible = true;
                MonedaOrigen.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MonedaOrigen);

                //Solicitado
                GridColumn Solicitado = new GridColumn();
                Solicitado.FieldName = this._unboundPrefix + "Solicitado";
                Solicitado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Solicitado");
                Solicitado.UnboundType = UnboundColumnType.Decimal;
                Solicitado.VisibleIndex = 5;
                Solicitado.Width = 70;
                Solicitado.Visible = true;
                Solicitado.ColumnEdit = this.TextEdit;
                Solicitado.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Solicitado);

                //Disponible
                GridColumn Disponible = new GridColumn();
                Disponible.FieldName = this._unboundPrefix + "Disponible";
                Disponible.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Disponible");
                Disponible.UnboundType = UnboundColumnType.Decimal;
                Disponible.VisibleIndex = 6;
                Disponible.Width = 70;
                Disponible.Visible = true;
                Disponible.ColumnEdit = this.TextEdit;
                Disponible.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(Disponible);

                //EnProceso
                GridColumn EnProceso = new GridColumn();
                EnProceso.FieldName = this._unboundPrefix + "EnProceso";
                EnProceso.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_EnProceso");
                EnProceso.UnboundType = UnboundColumnType.Decimal;
                EnProceso.VisibleIndex = 7;
                EnProceso.Width = 70;
                EnProceso.Visible = true;
                EnProceso.ColumnEdit = this.TextEdit;
                EnProceso.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(EnProceso);

                //PorAprobar
                GridColumn PorAprobar = new GridColumn();
                PorAprobar.FieldName = this._unboundPrefix + "PorAprobar";
                PorAprobar.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PorAprobar");
                PorAprobar.UnboundType = UnboundColumnType.Decimal;
                PorAprobar.VisibleIndex = 7;
                PorAprobar.Width = 70;
                PorAprobar.Visible = true;
                PorAprobar.ColumnEdit = this.TextEdit;
                PorAprobar.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(PorAprobar);

                //SolicitaPresupuestoInd
                GridColumn SolicitaPresupuestoInd = new GridColumn();
                SolicitaPresupuestoInd.FieldName = this._unboundPrefix + "SolicitaPresInd";
                SolicitaPresupuestoInd.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_SolicitaPresupuestoInd");
                SolicitaPresupuestoInd.UnboundType = UnboundColumnType.Decimal;
                SolicitaPresupuestoInd.VisibleIndex = 8;
                SolicitaPresupuestoInd.Width = 40;
                SolicitaPresupuestoInd.Visible = true;
                SolicitaPresupuestoInd.ColumnEdit = this.TextEdit2;
                SolicitaPresupuestoInd.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(SolicitaPresupuestoInd);

                #endregion
                #region Columnas no Visibles

                //CentroCostoID
                GridColumn CentroCostoID = new GridColumn();
                CentroCostoID.FieldName = this._unboundPrefix + "CentroCostoID";
                CentroCostoID.UnboundType = UnboundColumnType.String;
                CentroCostoID.Visible = false;
                this.gvDocument.Columns.Add(CentroCostoID);

                //PrefijoID
                GridColumn PrefijoID = new GridColumn();
                PrefijoID.FieldName = this._unboundPrefix + "PrefijoID";
                PrefijoID.UnboundType = UnboundColumnType.String;
                PrefijoID.Visible = false;
                this.gvDocument.Columns.Add(PrefijoID);

                //DocumentoNro
                GridColumn DocumentoNro = new GridColumn();
                DocumentoNro.FieldName = this._unboundPrefix + "DocumentoNro";
                DocumentoNro.UnboundType = UnboundColumnType.Integer;
                DocumentoNro.Visible = false;
                this.gvDocument.Columns.Add(DocumentoNro);

              
                #endregion

                #endregion

                #region Columnas Detalle Nivel 1
                #region Columnas visibles

                //Estado
                GridColumn Estado = new GridColumn();
                Estado.FieldName = this._unboundPrefix + "Estado";
                Estado.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Estado");
                Estado.UnboundType = UnboundColumnType.Integer;
                Estado.VisibleIndex = 0;
                Estado.Width = 50;
                Estado.Visible = true;
                Estado.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(Estado);

                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this._unboundPrefix + "PrefDoc";
                PrefDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 1;
                PrefDoc.Width = 50;
                PrefDoc.Visible = false;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(PrefDoc);

                //FechaDoc
                GridColumn FechaDoc = new GridColumn();
                FechaDoc.FieldName = this._unboundPrefix + "FechaDoc";
                FechaDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_FechaDoc");
                FechaDoc.UnboundType = UnboundColumnType.DateTime;
                FechaDoc.VisibleIndex = 2;
                FechaDoc.Width = 50;
                FechaDoc.Visible = true;
                FechaDoc.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(FechaDoc);

                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this._unboundPrefix + "ProveedorID";
                ProveedorID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_ProveedorID");
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.VisibleIndex = 3;
                ProveedorID.Width = 70;
                ProveedorID.Visible = true;
                ProveedorID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(ProveedorID);

                //Descriptivo
                GridColumn DescriptivoDeta1 = new GridColumn();
                DescriptivoDeta1.FieldName = this._unboundPrefix + "Descriptivo";
                DescriptivoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                DescriptivoDeta1.UnboundType = UnboundColumnType.String;
                DescriptivoDeta1.VisibleIndex = 4;
                DescriptivoDeta1.Width = 100;
                DescriptivoDeta1.Visible = true;
                DescriptivoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DescriptivoDeta1);

                //CodigoBSID
                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this._unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 5;
                CodigoBSID.Width = 50;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(CodigoBSID);

                //Solicitado
                GridColumn SolicitadoDeta1 = new GridColumn();
                SolicitadoDeta1.FieldName = this._unboundPrefix + "Solicitado";
                SolicitadoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Solicitado");
                SolicitadoDeta1.UnboundType = UnboundColumnType.Decimal;
                SolicitadoDeta1.VisibleIndex = 6;
                SolicitadoDeta1.Width = 70;
                SolicitadoDeta1.Visible = true;
                SolicitadoDeta1.ColumnEdit = this.TextEdit;
                SolicitadoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(SolicitadoDeta1);

                //Disponible
                GridColumn DisponibleDeta1 = new GridColumn();
                DisponibleDeta1.FieldName = this._unboundPrefix + "Disponible";
                DisponibleDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_Disponible");
                DisponibleDeta1.UnboundType = UnboundColumnType.Decimal;
                DisponibleDeta1.VisibleIndex = 7;
                DisponibleDeta1.Width = 70;
                DisponibleDeta1.Visible = true;
                DisponibleDeta1.ColumnEdit = this.TextEdit;
                DisponibleDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(DisponibleDeta1);

                //EnProceso
                GridColumn EnProcesoDeta1 = new GridColumn();
                EnProcesoDeta1.FieldName = this._unboundPrefix + "EnProceso";
                EnProcesoDeta1.Caption = this._bc.GetResource(LanguageTypes.Forms, this._documentID + "_EnProceso");
                EnProcesoDeta1.UnboundType = UnboundColumnType.Decimal;
                EnProcesoDeta1.VisibleIndex = 8;
                EnProcesoDeta1.Width = 70;
                EnProcesoDeta1.Visible = true;
                EnProcesoDeta1.ColumnEdit = this.TextEdit;
                EnProcesoDeta1.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(EnProcesoDeta1);
             
                #endregion
                #endregion

                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.gvDetalle.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-", "QuerySobreEjecucion-AddGridCols"));
            }
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            #region Valida datos en el combo Estado
            if (string.IsNullOrEmpty(this.cmbEstado.EditValue.ToString()))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblTipoProyecto.Text);
                MessageBox.Show(msg);
                this.cmbEstado.Focus();

                return false;
            }
            #endregion            

            #region Valida datos en la maestra de Area Aprobacion
            //if (!this.masterAreaAprob.ValidID)
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblAreaAprob.Text);
            //    MessageBox.Show(msg);
            //    this.cmbEstado.Focus();

            //    return false;
            //}
            #endregion

            return true;
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Get);
                FormProvider.Master.itemSearch.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Search);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "Form_Enter: " + ex.Message));
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
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "Form_Leave: " + ex.Message));
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "Form_Closing: " + ex.Message));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "Form_FormClosed: " + ex.Message));
            }
        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

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
                    e.Value = String.Empty;
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
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                    else
                    {
                        DTO_plSobreEjecucion dtoDet = (DTO_plSobreEjecucion)e.Row;
                        pi = dtoDet.Detalle.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                e.Value = pi.GetValue(dtoDet.Detalle, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoDet.Detalle, null), null);
                        }
                        else
                        {
                            fi = dtoDet.Detalle.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                    e.Value = fi.GetValue(dtoDet.Detalle);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoDet.Detalle), null);
                            }
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
                        pi.SetValue(dto, e.Value, null);
                        //e.Value = pi.GetValue(dto, null);
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
                            pi.SetValue(dto, e.Value, null);
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                    else
                    {
                        DTO_plSobreEjecucion dtoDet = (DTO_plSobreEjecucion)e.Row;
                        pi = dtoDet.Detalle.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                            {
                                e.Value = pi.GetValue(dtoDet.Detalle, null);
                            }
                            else
                            {
                                UDT udtProp = (UDT)pi.GetValue(dtoDet.Detalle, null);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            fi = dtoDet.Detalle.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                {
                                    pi.SetValue(dto, e.Value, null);
                                    //e.Value = pi.GetValue(dto, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)fi.GetValue(dtoDet.Detalle);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Valida el tipo de proyecto al seleccionarlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbEstado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "cmbEstado_EditValueChanged"));
            }
        }

        /// <summary>
        ///Al salir del control se ejecuta
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
           
        }
        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this._cierresQuery = new List<DTO_plSobreEjecucion>();

                this.gcDocument.DataSource = this._cierresQuery;
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                if (this.ValidateHeader())
                {                 
                    int estado = (int)this.cmbEstado.EditValue;
                    this._cierresQuery = this._bc.AdministrationModel.plSobreEjecucion_GetOrdenCompraSobreEjec(this._documentID, estado, this.masterAreaAprob.Value);
                    if (this._cierresQuery.Count > 0)
                        this.gvDocument.FocusedRowHandle = 0;

                    this.gcDocument.DataSource = this._cierresQuery;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-QuerySobreEjecucion", "TBSearch"));
            }
        }

        #endregion

    }
}
