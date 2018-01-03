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
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ContabilizarNomina : DocumentNominaBaseForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        List<DTO_NominaContabilizacion> _lDocumentos = new List<DTO_NominaContabilizacion>();
        List<DTO_NominaContabilizacion> _lDocumentosAll = new List<DTO_NominaContabilizacion>();


        #endregion

        public ContabilizarNomina()
        {
            //this.InitializeComponent();
        }        

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.RefreshDocument();
            this.LoadData(true);
        }

       
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.no;
            this.documentID = AppProcess.ContabilizarNomina;
            base.SetInitParameters();
            this.AddGridCols();
            this.LoadData(true);
        }

        /// <summary>
        /// Columnas Grillas
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Documentos

                GridColumn liquidacion = new GridColumn();
                liquidacion.FieldName = this.unboundPrefix + "Liquidacion";
                liquidacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Liquidacion");
                liquidacion.UnboundType = UnboundColumnType.Integer;
                liquidacion.VisibleIndex = 0;
                liquidacion.Width = 100;
                liquidacion.Visible = true;
                liquidacion.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(liquidacion);


                GridColumn periodo = new GridColumn();
                periodo.FieldName = this.unboundPrefix + "Periodo";
                periodo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Periodo");
                periodo.UnboundType = UnboundColumnType.DateTime;
                periodo.VisibleIndex = 0;
                periodo.Width = 150;
                periodo.Visible = true;
                periodo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(periodo);

                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "Valor";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Valor");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.VisibleIndex = 0;
                valor.Width = 400;
                valor.Visible = true;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valor);
               
                #endregion

                #region Detalles

                GridColumn empleadoID = new GridColumn();
                empleadoID.FieldName = this.unboundPrefix + "EmpleadoID";
                empleadoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_EmpleadoID");
                empleadoID.UnboundType = UnboundColumnType.String;
                empleadoID.VisibleIndex = 0;
                empleadoID.Width = 100;
                empleadoID.Visible = true;
                empleadoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(empleadoID);
                this.gvAportes.Columns.Add(empleadoID);
                this.gvProvisiones.Columns.Add(empleadoID);

                GridColumn empleado = new GridColumn();
                empleado.FieldName = this.unboundPrefix + "Empleado";
                empleado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Empleado");
                empleado.UnboundType = UnboundColumnType.String;
                empleado.VisibleIndex = 1;
                empleado.Width = 250;
                empleado.Visible = true;
                empleado.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(empleado);
                this.gvAportes.Columns.Add(empleado);
                this.gvProvisiones.Columns.Add(empleado);

                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 2;
                centroCostoID.Width = 100;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(centroCostoID);
                this.gvAportes.Columns.Add(centroCostoID);
                this.gvProvisiones.Columns.Add(centroCostoID);

                GridColumn proyectoID = new GridColumn();
                proyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                proyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyectoID.UnboundType = UnboundColumnType.String;
                proyectoID.VisibleIndex = 3;
                proyectoID.Width = 100;
                proyectoID.Visible = true;
                proyectoID.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(proyectoID);
                this.gvAportes.Columns.Add(proyectoID);
                this.gvProvisiones.Columns.Add(proyectoID);

                GridColumn valorDetalle = new GridColumn();
                valorDetalle.FieldName = this.unboundPrefix + "ValorDetalle";
                valorDetalle.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorDetalle");
                valorDetalle.UnboundType = UnboundColumnType.Decimal;
                valorDetalle.VisibleIndex = 4;
                valorDetalle.Width = 100;
                valorDetalle.Visible = true;
                valorDetalle.OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns.Add(valorDetalle);

                #endregion

                #region Aportes

                GridColumn VlrEmpresaPEN = new GridColumn();
                VlrEmpresaPEN.FieldName = this.unboundPrefix + "VlrEmpresaPEN";
                VlrEmpresaPEN.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrEmpresaPEN");
                VlrEmpresaPEN.UnboundType = UnboundColumnType.Decimal;
                VlrEmpresaPEN.VisibleIndex = 5;
                VlrEmpresaPEN.Width = 100;
                VlrEmpresaPEN.Visible = true;
                VlrEmpresaPEN.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrEmpresaPEN);

                GridColumn VlrTrabajadorPEN = new GridColumn();
                VlrTrabajadorPEN.FieldName = this.unboundPrefix + "VlrTrabajadorPEN";
                VlrTrabajadorPEN.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrTrabajadorPEN");
                VlrTrabajadorPEN.UnboundType = UnboundColumnType.Decimal;
                VlrTrabajadorPEN.VisibleIndex = 6;
                VlrTrabajadorPEN.Width = 100;
                VlrTrabajadorPEN.Visible = true;
                VlrTrabajadorPEN.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrTrabajadorPEN);

                GridColumn VlrSolidaridad = new GridColumn();
                VlrSolidaridad.FieldName = this.unboundPrefix + "VlrSolidaridad";
                VlrSolidaridad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSolidaridad");
                VlrSolidaridad.UnboundType = UnboundColumnType.Decimal;
                VlrSolidaridad.VisibleIndex = 7;
                VlrSolidaridad.Width = 100;
                VlrSolidaridad.Visible = true;
                VlrSolidaridad.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrSolidaridad);

                GridColumn VlrSubsistencia = new GridColumn();
                VlrSubsistencia.FieldName = this.unboundPrefix + "VlrSubsistencia";
                VlrSubsistencia.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSubsistencia");
                VlrSubsistencia.UnboundType = UnboundColumnType.Decimal;
                VlrSubsistencia.VisibleIndex = 8;
                VlrSubsistencia.Width = 100;
                VlrSubsistencia.Visible = true;
                VlrSubsistencia.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrSubsistencia);

                GridColumn VlrEmpresaSLD = new GridColumn();
                VlrEmpresaSLD.FieldName = this.unboundPrefix + "VlrEmpresaSLD";
                VlrEmpresaSLD.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrEmpresaSLD");
                VlrEmpresaSLD.UnboundType = UnboundColumnType.Decimal;
                VlrEmpresaSLD.VisibleIndex = 9;
                VlrEmpresaSLD.Width = 100;
                VlrEmpresaSLD.Visible = true;
                VlrEmpresaSLD.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrEmpresaSLD);

                GridColumn VlrTrabajadorSLD = new GridColumn();
                VlrTrabajadorSLD.FieldName = this.unboundPrefix + "VlrTrabajadorSLD";
                VlrTrabajadorSLD.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrTrabajadorSLD");
                VlrTrabajadorSLD.UnboundType = UnboundColumnType.Decimal;
                VlrTrabajadorSLD.VisibleIndex = 10;
                VlrTrabajadorSLD.Width = 100;
                VlrTrabajadorSLD.Visible = true;
                VlrTrabajadorSLD.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrTrabajadorSLD);

                GridColumn VlrARP = new GridColumn();
                VlrARP.FieldName = this.unboundPrefix + "VlrARP";
                VlrARP.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrARP");
                VlrARP.UnboundType = UnboundColumnType.Decimal;
                VlrARP.VisibleIndex = 11;
                VlrARP.Width = 100;
                VlrARP.Visible = true;
                VlrARP.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrARP);

                GridColumn VlrCCF = new GridColumn();
                VlrCCF.FieldName = this.unboundPrefix + "VlrCCF";
                VlrCCF.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrCCF");
                VlrCCF.UnboundType = UnboundColumnType.Decimal;
                VlrCCF.VisibleIndex = 12;
                VlrCCF.Width = 100;
                VlrCCF.Visible = true;
                VlrCCF.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrCCF);

                GridColumn VlrSEN = new GridColumn();
                VlrSEN.FieldName = this.unboundPrefix + "VlrSEN";
                VlrSEN.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrSEN");
                VlrSEN.UnboundType = UnboundColumnType.Decimal;
                VlrSEN.VisibleIndex = 13;
                VlrSEN.Width = 100;
                VlrSEN.Visible = true;
                VlrSEN.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrSEN);

                GridColumn VlrIBF = new GridColumn();
                VlrIBF.FieldName = this.unboundPrefix + "VlrIBF";
                VlrIBF.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_VlrIBF");
                VlrIBF.UnboundType = UnboundColumnType.Decimal;
                VlrIBF.VisibleIndex = 14;
                VlrIBF.Width = 100;
                VlrIBF.Visible = true;
                VlrIBF.OptionsColumn.AllowEdit = false;
                this.gvAportes.Columns.Add(VlrIBF);

                #endregion

                #region Provisiones

                GridColumn ValorProvision = new GridColumn();
                ValorProvision.FieldName = this.unboundPrefix + "ValorProvision";
                ValorProvision.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorProvision");
                ValorProvision.UnboundType = UnboundColumnType.Decimal;
                ValorProvision.VisibleIndex = 5;
                ValorProvision.Width = 100;
                ValorProvision.Visible = true;
                ValorProvision.OptionsColumn.AllowEdit = false;
                this.gvProvisiones.Columns.Add(ValorProvision);

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContabilizacionNomina.cs", "AddGridCols"));
            }

        }

        /// <summary>
        /// Carga la informacion de las liquidaciones
        /// </summary>
        /// <param name="firstTime">si es en la primera carga</param>
        protected override void LoadData(bool firstTime)
        {
            if (firstTime)
            {
                DateTime periodo = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_Periodo));
                this._lDocumentos = this._bc.AdministrationModel.noLiquidacionesDocu_GetTotal(periodo);
                this._lDocumentosAll = ObjectCopier.Clone(this._lDocumentos);
                if (this.rbtProceso.SelectedIndex <= 0)//Todos
                    this.gcDocument.DataSource = this._lDocumentos;
                else if (this.rbtProceso.SelectedIndex == 1) // Nomina
                    this.gcDocument.DataSource = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.N);
                else if (this.rbtProceso.SelectedIndex == 2) // Provisiones
                    this.gcDocument.DataSource = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.Pr); 
                else if (this.rbtProceso.SelectedIndex == 3) // Planilla
                    this.gcDocument.DataSource = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.Pl); 
                this.gcDocument.RefreshDataSource();
            }
        }

        #endregion

        #region Funciones Privadas

        public void RefreshDocument()
        {
            this.gcDocument.DataSource = null;
            this._lDocumentos.Clear();            
        }

        #endregion

        #region Eventos


        protected  void rbtProceso_SelectIndex(object sender, EventArgs e)
        {
            this.LoadData(true);
        }

        #endregion

        #region Eventos Grillas

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        /// Aplica formato a los campos en la grilla de documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
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
        protected void gvDetalle_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        protected void gvDetalle_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ValorDetalle")
            {
                e.RepositoryItem = this.editValue;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvAportes_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        protected void gvAportes_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "VlrEmpresaPEN" || fieldName == "VlrTrabajadorPEN" || fieldName == "VlrSolidaridad" || fieldName == "VlrSubsistencia" ||
                fieldName == "VlrEmpresaSLD" || fieldName == "VlrTrabajadorSLD" || fieldName == "VlrARP" || fieldName == "VlrCCF" ||
                fieldName == "VlrSEN" || fieldName == "VlrIBF")
            {
                e.RepositoryItem = this.editValue;
            }
        }
        
        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvProvisiones_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
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
        protected void gvProvisiones_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ValorProvision")
            {
                e.RepositoryItem = this.editValue;
            }
        }


        #endregion

        #region Eventos MDI

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
            FormProvider.Master.itemUpdate.Visible = true;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemUpdate.Enabled = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemPrint.Enabled = true;
                FormProvider.Master.itemSave.Enabled = true;
            }
            else
            {
               
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
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
               
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { AppDocuments.Nomina, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                List<DTO_TxResult> results = new List<DTO_TxResult>();
                if (this.rbtProceso.SelectedIndex == 1) // Nomina
                    this._lDocumentos = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.N);
                else if (this.rbtProceso.SelectedIndex == 2) // Provisiones
                    this._lDocumentos = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.Pr);
                else if (this.rbtProceso.SelectedIndex == 3) // Planilla
                    this._lDocumentos = this._lDocumentos.FindAll(x => x.Liquidacion.Value == (byte)TipoLiquidacion.Pl);

                if (this._lDocumentos != null && this._lDocumentos.Count > 0)
                {
                    List<DTO_NominaContabilizacionDetalle> lTemp = new List<DTO_NominaContabilizacionDetalle>();
                    foreach (var item in _lDocumentos)
	                {
                        lTemp.AddRange(item.Detalle);
	                }
                    results = _bc.AdministrationModel.Proceso_ContabilizarNomina(this.documentID, this.dtPeriod.DateTime, this.dtFecha.DateTime, lTemp,(byte)this.rbtProceso.SelectedIndex);
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ContabilizarNomina.cs", "SaveThread"));
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
