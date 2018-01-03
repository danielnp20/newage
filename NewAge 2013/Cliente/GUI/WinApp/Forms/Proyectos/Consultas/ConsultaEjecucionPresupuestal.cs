using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.Globalization;
using NewAge.DTO.Attributes;
using System.Drawing;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ConsultaEjecucionPresupuestal : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        //Variables de datos
        private DTO_QueryEjecucionPresupuestal _rowCurrent = new DTO_QueryEjecucionPresupuestal();
        private DTO_QueryEjecucionPresupuestalDetalle _rowDetCurrent = new DTO_QueryEjecucionPresupuestalDetalle();
        private GridView _gridDetalleCurrent = new GridView();
        private List<DTO_QueryEjecucionPresupuestal> _listProyectos = new List<DTO_QueryEjecucionPresupuestal>();
 
        DateTime fecha = DateTime.Now;
        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaEjecucionPresupuestal()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal.cs", "ConsultaEjecucionPresupuestal"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Proyectos
            GridColumn Proyecto = new GridColumn();
            Proyecto.FieldName = this.unboundPrefix + "Proyecto";
            Proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Proyecto");
            Proyecto.UnboundType = UnboundColumnType.String;
            Proyecto.AppearanceCell.Options.UseTextOptions = true;
            Proyecto.AppearanceCell.Options.UseFont = true;
            Proyecto.VisibleIndex = 1;
            Proyecto.Width = 95;
            Proyecto.Visible = true;
            Proyecto.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Proyecto);


            GridColumn ProyectoDesc = new GridColumn();
            ProyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
            ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoDesc");
            ProyectoDesc.UnboundType = UnboundColumnType.String;
            ProyectoDesc.VisibleIndex = 2;
            ProyectoDesc.Width = 170;
            ProyectoDesc.Visible = true;
            ProyectoDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoDesc);

            GridColumn IngPresupuesto = new GridColumn();
            IngPresupuesto.FieldName = this.unboundPrefix + "IngPresupuesto";
            IngPresupuesto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngPresupuesto");
            IngPresupuesto.UnboundType = UnboundColumnType.Decimal;
            IngPresupuesto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngPresupuesto.VisibleIndex = 3;
            IngPresupuesto.AppearanceCell.Options.UseTextOptions = true;
            IngPresupuesto.Width = 130;
            IngPresupuesto.Visible = true;
            IngPresupuesto.OptionsColumn.AllowEdit = false;
            IngPresupuesto.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(IngPresupuesto);

            GridColumn IngEjecucion = new GridColumn();
            IngEjecucion.FieldName = this.unboundPrefix + "IngEjecucion";
            IngEjecucion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngEjecucion");
            IngEjecucion.UnboundType = UnboundColumnType.Decimal;
            IngEjecucion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngEjecucion.AppearanceCell.Options.UseTextOptions = true;
            IngEjecucion.VisibleIndex = 4;
            IngEjecucion.Width = 110;
            IngEjecucion.Visible = true;
            IngEjecucion.OptionsColumn.AllowEdit = false;
            IngEjecucion.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(IngEjecucion);

            GridColumn IngxEjecutar = new GridColumn();
            IngxEjecutar.FieldName = this.unboundPrefix + "IngxEjecutar";
            IngxEjecutar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngxEjecutar");
            IngxEjecutar.UnboundType = UnboundColumnType.Decimal;
            IngxEjecutar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngxEjecutar.AppearanceCell.Options.UseTextOptions = true;
            IngxEjecutar.VisibleIndex = 5;
            IngxEjecutar.Width = 110;
            IngxEjecutar.Visible = true;
            IngxEjecutar.OptionsColumn.AllowEdit = false;
            IngxEjecutar.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(IngxEjecutar);

            GridColumn CostoPresupuesto = new GridColumn();
            CostoPresupuesto.FieldName = this.unboundPrefix + "CostoPresupuesto";
            CostoPresupuesto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoPresupuesto");           
            CostoPresupuesto.UnboundType = UnboundColumnType.Decimal;
            CostoPresupuesto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoPresupuesto.AppearanceCell.Options.UseTextOptions = true;
            CostoPresupuesto.VisibleIndex = 6;
            CostoPresupuesto.Width = 110;
            CostoPresupuesto.Visible = true;
            CostoPresupuesto.OptionsColumn.AllowEdit = false;
            CostoPresupuesto.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(CostoPresupuesto);

            GridColumn CostoPresAjustado = new GridColumn();
            CostoPresAjustado.FieldName = this.unboundPrefix + "CostoPresAjustado";
            CostoPresAjustado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoPresAjustado");
            CostoPresAjustado.UnboundType = UnboundColumnType.Decimal;
            CostoPresAjustado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoPresAjustado.AppearanceCell.Options.UseTextOptions = true;
            CostoPresAjustado.VisibleIndex = 7;
            CostoPresAjustado.Width = 110;
            CostoPresAjustado.Visible = true;
            CostoPresAjustado.OptionsColumn.AllowEdit = false;
            CostoPresAjustado.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(CostoPresAjustado);

            GridColumn CostoEjecucion = new GridColumn();
            CostoEjecucion.FieldName = this.unboundPrefix + "CostoEjecucion";
            CostoEjecucion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoEjecucion");
            CostoEjecucion.UnboundType = UnboundColumnType.Decimal;
            CostoEjecucion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoEjecucion.AppearanceCell.Options.UseTextOptions = true;
            CostoEjecucion.VisibleIndex = 8;
            CostoEjecucion.Width = 110;
            CostoEjecucion.Visible = true;
            CostoEjecucion.OptionsColumn.AllowEdit = false;
            CostoEjecucion.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(CostoEjecucion);

            GridColumn CostoxEjecutar = new GridColumn();
            CostoxEjecutar.FieldName = this.unboundPrefix + "CostoxEjecutar";
            CostoxEjecutar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoxEjecutar");
            CostoxEjecutar.UnboundType = UnboundColumnType.Decimal;
            CostoxEjecutar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoxEjecutar.AppearanceCell.Options.UseTextOptions = true;
            CostoxEjecutar.AppearanceCell.Options.UseFont = true;
            CostoxEjecutar.AppearanceCell.Options.UseForeColor = true;
            CostoxEjecutar.VisibleIndex = 9;
            CostoxEjecutar.Width = 110;
            CostoxEjecutar.Visible = true;
            CostoxEjecutar.OptionsColumn.AllowEdit = false;
            CostoxEjecutar.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(CostoxEjecutar);

            GridColumn RentaPresupuesto = new GridColumn();
            RentaPresupuesto.FieldName = this.unboundPrefix + "RentaPresupuesto";
            RentaPresupuesto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaPresupuesto");
            RentaPresupuesto.UnboundType = UnboundColumnType.Decimal;
            RentaPresupuesto.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaPresupuesto.AppearanceCell.Options.UseTextOptions = true;
            RentaPresupuesto.AppearanceCell.Options.UseFont = true;
            RentaPresupuesto.AppearanceCell.Options.UseForeColor = true;
            RentaPresupuesto.VisibleIndex = 10;
            RentaPresupuesto.Width = 110;
            RentaPresupuesto.Visible = true;
            RentaPresupuesto.OptionsColumn.AllowEdit = false;
            RentaPresupuesto.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(RentaPresupuesto);

            GridColumn RentaPresAjustado = new GridColumn();
            RentaPresAjustado.FieldName = this.unboundPrefix + "RentaPresAjustado";
            RentaPresAjustado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaPresAjustado");
            RentaPresAjustado.UnboundType = UnboundColumnType.Decimal;
            RentaPresAjustado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaPresAjustado.AppearanceCell.Options.UseTextOptions = true;
            RentaPresAjustado.AppearanceCell.Options.UseFont = true;
            RentaPresAjustado.AppearanceCell.Options.UseForeColor = true;
            RentaPresAjustado.VisibleIndex = 11;
            RentaPresAjustado.Width = 110;
            RentaPresAjustado.Visible = true;
            RentaPresAjustado.OptionsColumn.AllowEdit = false;
            RentaPresAjustado.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(RentaPresAjustado);

            
            GridColumn RentaEjecucion = new GridColumn();
            RentaEjecucion.FieldName = this.unboundPrefix + "RentaEjecucion";
            RentaEjecucion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaEjecucion");
            RentaEjecucion.UnboundType = UnboundColumnType.Decimal;
            RentaEjecucion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaEjecucion.AppearanceCell.Options.UseTextOptions = true;
            RentaEjecucion.AppearanceCell.Options.UseFont = true;
            RentaEjecucion.AppearanceCell.Options.UseForeColor = true;
            RentaEjecucion.VisibleIndex = 12;
            RentaEjecucion.Width = 110;
            RentaEjecucion.Visible = true;
            RentaEjecucion.OptionsColumn.AllowEdit = false;
            RentaEjecucion.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(RentaEjecucion);

            GridColumn FactING = new GridColumn();
            FactING.FieldName = this.unboundPrefix + "FactINGxEjecutar";
            FactING.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactINGxEjecutar");
            FactING.UnboundType = UnboundColumnType.Decimal;
            FactING.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            FactING.AppearanceCell.Options.UseTextOptions = true;
            FactING.AppearanceCell.Options.UseFont = true;
            FactING.AppearanceCell.Options.UseForeColor = true;
            FactING.VisibleIndex = 13;
            FactING.Width = 110;
            FactING.Visible = true;
            FactING.OptionsColumn.AllowEdit = false;
            FactING.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(FactING);

            GridColumn FactCTO = new GridColumn();
            FactCTO.FieldName = this.unboundPrefix + "FactCTOxEjecutar";
            FactCTO.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactCTOxEjecutar");
            FactCTO.UnboundType = UnboundColumnType.Decimal;
            FactCTO.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            FactCTO.AppearanceCell.Options.UseTextOptions = true;
            FactCTO.AppearanceCell.Options.UseFont = true;
            FactCTO.AppearanceCell.Options.UseForeColor = true;
            FactCTO.VisibleIndex = 14;
            FactCTO.Width = 110;
            FactCTO.Visible = true;
            FactCTO.OptionsColumn.AllowEdit = false;
            FactCTO.ColumnEdit = this.editSpinPorcen;
            this.gvProyectos.Columns.Add(FactCTO);

            this.gvProyectos.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Sub Grilla Detalle
            GridColumn LineaPRE = new GridColumn();
            LineaPRE.FieldName = this.unboundPrefix + "LineaPRE";
            LineaPRE.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaPRE");
            LineaPRE.UnboundType = UnboundColumnType.String;
            LineaPRE.AppearanceCell.Options.UseTextOptions = true;
            LineaPRE.AppearanceCell.Options.UseFont = true;
            LineaPRE.VisibleIndex = 1;
            LineaPRE.Width = 95;
            LineaPRE.Visible = true;
            LineaPRE.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(LineaPRE);


            GridColumn LineaDesc = new GridColumn();
            LineaDesc.FieldName = this.unboundPrefix + "LineaDesc";
            LineaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_LineaDesc");
            LineaDesc.UnboundType = UnboundColumnType.String;
            LineaDesc.VisibleIndex = 2;
            LineaDesc.Width = 170;
            LineaDesc.Visible = true;
            LineaDesc.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(LineaDesc);

            GridColumn IngPresupuestoDet = new GridColumn();
            IngPresupuestoDet.FieldName = this.unboundPrefix + "IngPresupuesto";
            IngPresupuestoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngPresupuesto");
            IngPresupuestoDet.UnboundType = UnboundColumnType.Decimal;
            IngPresupuestoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngPresupuestoDet.VisibleIndex = 3;
            IngPresupuestoDet.AppearanceCell.Options.UseTextOptions = true;
            IngPresupuestoDet.Width = 130;
            IngPresupuestoDet.Visible = true;
            IngPresupuestoDet.OptionsColumn.AllowEdit = false;
            IngPresupuestoDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(IngPresupuestoDet);

            GridColumn IngEjecucionDet = new GridColumn();
            IngEjecucionDet.FieldName = this.unboundPrefix + "IngEjecucion";
            IngEjecucionDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngEjecucion");
            IngEjecucionDet.UnboundType = UnboundColumnType.Decimal;
            IngEjecucionDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngEjecucionDet.AppearanceCell.Options.UseTextOptions = true;
            IngEjecucionDet.VisibleIndex = 4;
            IngEjecucionDet.Width = 110;
            IngEjecucionDet.Visible = true;
            IngEjecucionDet.OptionsColumn.AllowEdit = false;
            IngEjecucionDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(IngEjecucionDet);

            GridColumn IngxEjecutarDet = new GridColumn();
            IngxEjecutarDet.FieldName = this.unboundPrefix + "IngxEjecutar";
            IngxEjecutarDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_IngxEjecutar");
            IngxEjecutarDet.UnboundType = UnboundColumnType.Decimal;
            IngxEjecutarDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            IngxEjecutarDet.AppearanceCell.Options.UseTextOptions = true;
            IngxEjecutarDet.VisibleIndex = 5;
            IngxEjecutarDet.Width = 110;
            IngxEjecutarDet.Visible = true;
            IngxEjecutarDet.OptionsColumn.AllowEdit = false;
            IngxEjecutarDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(IngxEjecutarDet);

            GridColumn CostoPresupuestoDet = new GridColumn();
            CostoPresupuestoDet.FieldName = this.unboundPrefix + "CostoPresupuesto";
            CostoPresupuestoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoPresupuesto");
            CostoPresupuestoDet.UnboundType = UnboundColumnType.Decimal;
            CostoPresupuestoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoPresupuestoDet.AppearanceCell.Options.UseTextOptions = true;
            CostoPresupuestoDet.VisibleIndex = 6;
            CostoPresupuestoDet.Width = 110;
            CostoPresupuestoDet.Visible = true;
            CostoPresupuestoDet.OptionsColumn.AllowEdit = false;
            CostoPresupuestoDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(CostoPresupuestoDet);

            GridColumn CostoPresAjustadoDet = new GridColumn();
            CostoPresAjustadoDet.FieldName = this.unboundPrefix + "CostoPresAjustado";
            CostoPresAjustadoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoPresAjustado");
            CostoPresAjustadoDet.UnboundType = UnboundColumnType.Decimal;
            CostoPresAjustadoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoPresAjustadoDet.AppearanceCell.Options.UseTextOptions = true;
            CostoPresAjustadoDet.VisibleIndex = 7;
            CostoPresAjustadoDet.Width = 110;
            CostoPresAjustadoDet.Visible = true;
            CostoPresAjustadoDet.OptionsColumn.AllowEdit = false;
            CostoPresAjustadoDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(CostoPresAjustadoDet);

            GridColumn CostoEjecucionDet = new GridColumn();
            CostoEjecucionDet.FieldName = this.unboundPrefix + "CostoEjecucion";
            CostoEjecucionDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoEjecucion");
            CostoEjecucionDet.UnboundType = UnboundColumnType.Decimal;
            CostoEjecucionDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoEjecucionDet.AppearanceCell.Options.UseTextOptions = true;
            CostoEjecucionDet.VisibleIndex = 8;
            CostoEjecucionDet.Width = 110;
            CostoEjecucionDet.Visible = true;
            CostoEjecucionDet.OptionsColumn.AllowEdit = false;
            CostoEjecucionDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(CostoEjecucionDet);

            GridColumn CostoxEjecutarDet = new GridColumn();
            CostoxEjecutarDet.FieldName = this.unboundPrefix + "CostoxEjecutar";
            CostoxEjecutarDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoxEjecutar");
            CostoxEjecutarDet.UnboundType = UnboundColumnType.Decimal;
            CostoxEjecutarDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CostoxEjecutarDet.AppearanceCell.Options.UseTextOptions = true;
            CostoxEjecutarDet.AppearanceCell.Options.UseFont = true;
            CostoxEjecutarDet.AppearanceCell.Options.UseForeColor = true;
            CostoxEjecutarDet.VisibleIndex = 9;
            CostoxEjecutarDet.Width = 110;
            CostoxEjecutarDet.Visible = true;
            CostoxEjecutarDet.OptionsColumn.AllowEdit = false;
            CostoxEjecutarDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(CostoxEjecutarDet);

            GridColumn RentaPresupuestoDet = new GridColumn();
            RentaPresupuestoDet.FieldName = this.unboundPrefix + "RentaPresupuesto";
            RentaPresupuestoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaPresupuesto");
            RentaPresupuestoDet.UnboundType = UnboundColumnType.Decimal;
            RentaPresupuestoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaPresupuestoDet.AppearanceCell.Options.UseTextOptions = true;
            RentaPresupuestoDet.AppearanceCell.Options.UseFont = true;
            RentaPresupuestoDet.AppearanceCell.Options.UseForeColor = true;
            RentaPresupuestoDet.VisibleIndex = 10;
            RentaPresupuestoDet.Width = 110;
            RentaPresupuestoDet.Visible = true;
            RentaPresupuestoDet.OptionsColumn.AllowEdit = false;
            RentaPresupuestoDet.ColumnEdit = this.editSpinPorcen;
            this.gvDetalle.Columns.Add(RentaPresupuestoDet);

            GridColumn RentaPresAjustadoDet = new GridColumn();
            RentaPresAjustadoDet.FieldName = this.unboundPrefix + "RentaPresAjustado";
            RentaPresAjustadoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaPresAjustado");
            RentaPresAjustadoDet.UnboundType = UnboundColumnType.Decimal;
            RentaPresAjustadoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaPresAjustadoDet.AppearanceCell.Options.UseTextOptions = true;
            RentaPresAjustadoDet.AppearanceCell.Options.UseFont = true;
            RentaPresAjustadoDet.AppearanceCell.Options.UseForeColor = true;
            RentaPresAjustadoDet.VisibleIndex = 11;
            RentaPresAjustadoDet.Width = 110;
            RentaPresAjustadoDet.Visible = true;
            RentaPresAjustadoDet.OptionsColumn.AllowEdit = false;
            RentaPresAjustadoDet.ColumnEdit = this.editSpinPorcen;
            this.gvDetalle.Columns.Add(RentaPresAjustadoDet);

            GridColumn RentaEjecucionDet = new GridColumn();
            RentaEjecucionDet.FieldName = this.unboundPrefix + "RentaEjecucion";
            RentaEjecucionDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RentaEjecucion");
            RentaEjecucionDet.UnboundType = UnboundColumnType.Decimal;
            RentaEjecucionDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            RentaEjecucionDet.AppearanceCell.Options.UseTextOptions = true;
            RentaEjecucionDet.AppearanceCell.Options.UseFont = true;
            RentaEjecucionDet.AppearanceCell.Options.UseForeColor = true;
            RentaEjecucionDet.VisibleIndex = 12;
            RentaEjecucionDet.Width = 110;
            RentaEjecucionDet.Visible = true;
            RentaEjecucionDet.OptionsColumn.AllowEdit = false;
            RentaEjecucionDet.ColumnEdit = this.editSpinPorcen;
            this.gvDetalle.Columns.Add(RentaEjecucionDet);

            GridColumn FactINGDet = new GridColumn();
            FactINGDet.FieldName = this.unboundPrefix + "FactINGxEjecutar";
            FactINGDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactINGxEjecutar");
            FactINGDet.UnboundType = UnboundColumnType.Decimal;
            FactINGDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            FactINGDet.AppearanceCell.Options.UseTextOptions = true;
            FactINGDet.AppearanceCell.Options.UseFont = true;
            FactINGDet.AppearanceCell.Options.UseForeColor = true;
            FactINGDet.VisibleIndex = 13;
            FactINGDet.Width = 110;
            FactINGDet.Visible = true;
            FactINGDet.OptionsColumn.AllowEdit = false;
            FactINGDet.ColumnEdit = this.editSpinPorcen;
            this.gvDetalle.Columns.Add(FactINGDet);

            GridColumn FactCTODet = new GridColumn();
            FactCTODet.FieldName = this.unboundPrefix + "FactCTOxEjecutar";
            FactCTODet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactCTOxEjecutar");
            FactCTODet.UnboundType = UnboundColumnType.Decimal;
            FactCTODet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            FactCTODet.AppearanceCell.Options.UseTextOptions = true;
            FactCTODet.AppearanceCell.Options.UseFont = true;
            FactCTODet.AppearanceCell.Options.UseForeColor = true;
            FactCTODet.VisibleIndex = 14;
            FactCTODet.Width = 110;
            FactCTODet.Visible = true;
            FactCTODet.OptionsColumn.AllowEdit = false;
            FactCTODet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(FactCTODet);

            GridColumn Documento = new GridColumn();
            Documento.FieldName = this.unboundPrefix + "Documento";
            Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
            Documento.UnboundType = UnboundColumnType.String;
            Documento.OptionsColumn.ShowCaption = false;
            Documento.VisibleIndex = 15;
            Documento.Width = 80;
            Documento.ColumnEdit = this.editLink;
            Documento.Visible = true;
            this.gvDetalle.Columns.Add(Documento);
            this.gvDetalle.OptionsView.ColumnAutoWidth = true;


            #endregion
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData()
        {
            try
            {
                this.dtFechaCorte.Enabled = false;
                this.dtFechaCorte.ReadOnly = false;

                this._listProyectos = this._bc.AdministrationModel.plEjecucionPresupuestal(this.dtFechaCorte.DateTime);


                //foreach (DTO_QueryFlujoFondos proy in this._listProyectos)
                //{
                //    this._listTareas = this._bc.AdministrationModel.tsFlujoFondos_Tareas(this.dtFechaCorte.DateTime,proy.Proyecto.Value);
                //}
                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids()
        {
            try
            {
                this.gcProyectos.DataSource = null;
                this.gcProyectos.DataSource = this._listProyectos;
                this.gcProyectos.RefreshDataSource();

//                this.gcTarea.DataSource = null;
//                this.gcTarea.DataSource = this._listTareas;
//                this.gcTarea.RefreshDataSource();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }


        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {

            this._rowCurrent = new DTO_QueryEjecucionPresupuestal();
            this._rowDetCurrent = new DTO_QueryEjecucionPresupuestalDetalle();
            this._listProyectos = new List<DTO_QueryEjecucionPresupuestal>();
            this.gcProyectos.DataSource = this._listProyectos;
            this.gcProyectos.RefreshDataSource();


        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.ts;
            this.documentID = AppQueries.QueryEjecucionPresupuestal;
            this.AddGridCols();

            this.dtFechaCorte.DateTime = DateTime.Now;
            

            this.LoadData();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }
        private string obtenerNombreMesNumero(int numeroMes)
        {
            try
            {
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(numeroMes);
                return nombreMes;
            }
            catch
            {
                return "Desconocido";
            }
        } 
        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            return true;
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this.frmModule);
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ;
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                ;
            }
        }

        #endregion

        #region Eventos Header Superior

        /// <summary>
        /// Evento que se ejecuta al salir del numero de documento (glDocumentoControl - NumeroDoc)
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNumeroDoc_Leave(object sender, EventArgs e) { }

        /// <summary>
        /// Evento que se ejecuta al pararse sobre el control de fecha
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFecha_Enter(object sender, EventArgs e) { }
       
        /// <summary>
        /// Valida que solo ingrese numeros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNumPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion

        #region Eventos Header
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            if (this.gvProyectos.DataRowCount > 0)
            {

            }
        }

        #endregion

        #region Eventos Grilla
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._rowCurrent != null && this._rowDetCurrent != null)
                {
                    //Type frm = null;
                    //frm = typeof(ModalViewDocContable);
                    //FormProvider.GetInstance(frm, new object[] { this._rowCurrent.Proyecto.Value, this._rowDetCurrent.LineaPRE.Value });


                    ModalViewDocContable fact = new ModalViewDocContable(this._rowDetCurrent.Proyecto.Value, this._rowDetCurrent.LineaPRE.Value);
                    fact.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ShowDocumentForm.cs", "OperacionesPendientes"));
            }

        }


        #region Proyectos

        private void gcProyectos_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            try
            {
                this._gridDetalleCurrent = (GridView)e.View;
                if (this._gridDetalleCurrent != null && this._gridDetalleCurrent.FocusedRowHandle >= 0 && this._gridDetalleCurrent.DataRowCount > 0)
                {
                    var row = this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                    if (row.GetType() == typeof(DTO_QueryEjecucionPresupuestalDetalle))
                    {
                        this._rowDetCurrent= (DTO_QueryEjecucionPresupuestalDetalle)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);

                    }
                    else if (row.GetType() == typeof(DTO_QueryEjecucionPresupuestal))
                        this._rowCurrent = (DTO_QueryEjecucionPresupuestal)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal.cs", "gcProyectos_FocusedViewChanged"));
            }

        }

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowCurrent = (DTO_QueryEjecucionPresupuestal)this.gvProyectos.GetRow(e.FocusedRowHandle);

                    if (this._rowCurrent != null)
                    {
                    //    this._listTareas = this._bc.AdministrationModel.tsFlujoFondos_Tareas(this.dtFechaCorte.DateTime, this._rowCurrent.Proyecto.Value);
                    }
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName =!e.Column.FieldName.Equals("Editar")? e.Column.FieldName.Substring(this.unboundPrefix.Length) : e.Column.FieldName;

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
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
                        e.Value = pi.GetValue(dto, null);
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


        #endregion

        #region Detalle
        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>

        private void gvDetalle_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Documento")
                e.DisplayText = "Ver Documento";

        }

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void gvDetalle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._gridDetalleCurrent = (GridView)sender;
                    this._rowDetCurrent = (DTO_QueryEjecucionPresupuestalDetalle)this._gridDetalleCurrent.GetRow(e.FocusedRowHandle);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucionPresupuestal.cs", "gvDetalle_FocusedRowChanged"));
            }
        }


        #endregion
        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
                   


    }
}
