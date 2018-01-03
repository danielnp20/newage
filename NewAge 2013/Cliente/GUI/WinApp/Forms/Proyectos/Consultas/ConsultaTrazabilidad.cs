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
    public partial class ConsultaTrazabilidad : FormWithToolbar
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
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        //Variables para importar
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        private int _numeroDoc = 0;
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_QueryTrazabilidad _rowTarea = new DTO_QueryTrazabilidad();
        private DTO_QueryTrazabilidad _rowDetalle = new DTO_QueryTrazabilidad();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdicion = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoDeta> _listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
        private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        private List<DTO_QueryTrazabilidad> _listTrazResumen = new List<DTO_QueryTrazabilidad>();
        private DTO_pyClaseProyecto _dtoClaseServicio = new DTO_pyClaseProyecto();
        private List<DTO_ExportTrazabilidad> _listExportExcel = new List<DTO_ExportTrazabilidad>();
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaTrazabilidad()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDocumentInfo(true);
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "ConsultaTrazabilidad"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Tareas
            GridColumn TareaCliente = new GridColumn();
            TareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            TareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            TareaCliente.UnboundType = UnboundColumnType.String;
            TareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            TareaCliente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaCliente.AppearanceCell.Options.UseTextOptions = true;
            TareaCliente.AppearanceCell.Options.UseFont = true;
            TareaCliente.VisibleIndex = 0;
            TareaCliente.Width = 30;
            TareaCliente.Visible = true;
            TareaCliente.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaCliente);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            TareaID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaID.AppearanceCell.Options.UseTextOptions = true;
            TareaID.AppearanceCell.Options.UseFont = true;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 50;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaID);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this.unboundPrefix + "TareaDesc";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaDesc");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 2;
            TareaDesc.Width = 300;
            TareaDesc.Visible = true;
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaDesc);       

            GridColumn CantPresupuestado = new GridColumn();
            CantPresupuestado.FieldName = this.unboundPrefix + "CantPresupuestado";
            CantPresupuestado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPresupuestado");
            CantPresupuestado.UnboundType = UnboundColumnType.Decimal;
            CantPresupuestado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPresupuestado.AppearanceCell.Options.UseTextOptions = true;
            CantPresupuestado.VisibleIndex = 3;
            CantPresupuestado.Width = 75;
            CantPresupuestado.Visible = true;
            CantPresupuestado.ColumnEdit = this.editValue2Cant;
            CantPresupuestado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantPresupuestado);

            GridColumn CantSolicitado = new GridColumn();
            CantSolicitado.FieldName = this.unboundPrefix + "CantSolicitado";
            CantSolicitado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantSolicitado");
            CantSolicitado.UnboundType = UnboundColumnType.Decimal;
            CantSolicitado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantSolicitado.AppearanceCell.Options.UseTextOptions = true;
            CantSolicitado.VisibleIndex = 4;
            CantSolicitado.Width = 75;
            CantSolicitado.Visible = false;
            CantSolicitado.ColumnEdit = this.editValue2Cant;
            CantSolicitado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantSolicitado);

            GridColumn CantComprado = new GridColumn();
            CantComprado.FieldName = this.unboundPrefix + "CantComprado";
            CantComprado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantComprado");
            CantComprado.UnboundType = UnboundColumnType.Decimal;
            CantComprado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantComprado.AppearanceCell.Options.UseTextOptions = true;
            CantComprado.VisibleIndex = 5;
            CantComprado.Width = 75;
            CantComprado.Visible = false;
            CantComprado.ColumnEdit = this.editValue2Cant;
            CantComprado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantComprado);

            GridColumn CantRecibido = new GridColumn();
            CantRecibido.FieldName = this.unboundPrefix + "CantRecibido";
            CantRecibido.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantRecibido");
            CantRecibido.UnboundType = UnboundColumnType.Decimal;
            CantRecibido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantRecibido.AppearanceCell.Options.UseTextOptions = true;
            CantRecibido.VisibleIndex = 6;
            CantRecibido.Width = 75;
            CantRecibido.Visible = false;
            CantRecibido.ColumnEdit = this.editValue2Cant;
            CantRecibido.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantRecibido);

            GridColumn CantConsumido = new GridColumn();
            CantConsumido.FieldName = this.unboundPrefix + "CantConsumido";
            CantConsumido.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantConsumido");
            CantConsumido.UnboundType = UnboundColumnType.Decimal;
            CantConsumido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantConsumido.AppearanceCell.Options.UseTextOptions = true;
            CantConsumido.VisibleIndex = 7;
            CantConsumido.Width = 75;
            CantConsumido.Visible = false;
            CantConsumido.ColumnEdit = this.editValue2Cant;
            CantConsumido.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantConsumido);

            GridColumn CantFacturado = new GridColumn();
            CantFacturado.FieldName = this.unboundPrefix + "CantFacturado";
            CantFacturado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantFacturado");
            CantFacturado.UnboundType = UnboundColumnType.Decimal;
            CantFacturado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantFacturado.AppearanceCell.Options.UseTextOptions = true;
            CantFacturado.VisibleIndex = 8;
            CantFacturado.Width = 75;
            CantFacturado.Visible = false;
            CantFacturado.ColumnEdit = this.editValue2Cant;
            CantFacturado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantFacturado);

            GridColumn VlrPresupuestado = new GridColumn();
            VlrPresupuestado.FieldName = this.unboundPrefix + "VlrPresupuestado";
            VlrPresupuestado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrPresupuestado");
            VlrPresupuestado.UnboundType = UnboundColumnType.Decimal;
            VlrPresupuestado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestado.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestado.VisibleIndex = 9;
            VlrPresupuestado.Width = 75;
            VlrPresupuestado.Visible = false;
            VlrPresupuestado.ColumnEdit = this.editValue2;
            VlrPresupuestado.OptionsColumn.AllowEdit = false;
            VlrPresupuestado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrPresupuestado.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrPresupuestado);

            GridColumn VlrSolicitado = new GridColumn();
            VlrSolicitado.FieldName = this.unboundPrefix + "VlrSolicitado";
            VlrSolicitado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrSolicitado");
            VlrSolicitado.UnboundType = UnboundColumnType.Decimal;
            VlrSolicitado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrSolicitado.AppearanceCell.Options.UseTextOptions = true;
            VlrSolicitado.VisibleIndex = 10;
            VlrSolicitado.Width = 75;
            VlrSolicitado.Visible = false;
            VlrSolicitado.ColumnEdit = this.editValue2;
            VlrSolicitado.OptionsColumn.AllowEdit = false;
            VlrSolicitado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSolicitado.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrSolicitado);

            GridColumn VlrComprado = new GridColumn();
            VlrComprado.FieldName = this.unboundPrefix + "VlrComprado";
            VlrComprado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrComprado");
            VlrComprado.UnboundType = UnboundColumnType.Decimal;
            VlrComprado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrComprado.AppearanceCell.Options.UseTextOptions = true;
            VlrComprado.VisibleIndex = 11;
            VlrComprado.Width = 75;
            VlrComprado.Visible = false;
            VlrComprado.ColumnEdit = this.editValue2;
            VlrComprado.OptionsColumn.AllowEdit = false;
            VlrComprado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrComprado.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrComprado);

            GridColumn VlrRecibido = new GridColumn();
            VlrRecibido.FieldName = this.unboundPrefix + "VlrRecibido";
            VlrRecibido.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrRecibido");
            VlrRecibido.UnboundType = UnboundColumnType.Decimal;
            VlrRecibido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrRecibido.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibido.VisibleIndex = 12;
            VlrRecibido.Width = 75;
            VlrRecibido.Visible = false;
            VlrRecibido.ColumnEdit = this.editValue2;
            VlrRecibido.OptionsColumn.AllowEdit = false;
            VlrRecibido.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrRecibido.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrRecibido);

            GridColumn VlrConsumido = new GridColumn();
            VlrConsumido.FieldName = this.unboundPrefix + "VlrConsumido";
            VlrConsumido.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrConsumido");
            VlrConsumido.UnboundType = UnboundColumnType.Decimal;
            VlrConsumido.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrConsumido.AppearanceCell.Options.UseTextOptions = true;
            VlrConsumido.VisibleIndex = 13;
            VlrConsumido.Width = 75;
            VlrConsumido.Visible = false;
            VlrConsumido.ColumnEdit = this.editValue2;
            VlrConsumido.OptionsColumn.AllowEdit = false;
            VlrConsumido.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrConsumido.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrConsumido);

            GridColumn VlrFacturado = new GridColumn();
            VlrFacturado.FieldName = this.unboundPrefix + "VlrFacturado";
            VlrFacturado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrFacturado");
            VlrFacturado.UnboundType = UnboundColumnType.Decimal;
            VlrFacturado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrFacturado.AppearanceCell.Options.UseTextOptions = true;
            VlrFacturado.VisibleIndex = 14;
            VlrFacturado.Width = 75;
            VlrFacturado.Visible = false;
            VlrFacturado.ColumnEdit = this.editValue2;
            VlrFacturado.OptionsColumn.AllowEdit = false;
            VlrFacturado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrFacturado.FieldName, "{0:c0}");
            this.gvTareas.Columns.Add(VlrFacturado);

            this.gvTareas.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Grilla  Recursos
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 1;
            RecursoID.Width = 90;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 2;
            RecursoDesc.Width = 160;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvID.VisibleIndex = 3;
            UnidadInvID.Width = 30;
            UnidadInvID.Visible = true;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvID);

            //MarcaInvID
            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.VisibleIndex = 4;
            MarcaInvID.Width = 60;
            MarcaInvID.Visible = true;
            MarcaInvID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(MarcaInvID);

            //RefProveedor
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
            RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 5;
            RefProveedor.Width = 60;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RefProveedor);

            GridColumn CantPresupuestadoRec = new GridColumn();
            CantPresupuestadoRec.FieldName = this.unboundPrefix + "CantPresupuestado";
            CantPresupuestadoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPresupuestado");
            CantPresupuestadoRec.UnboundType = UnboundColumnType.String;
            CantPresupuestadoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPresupuestadoRec.AppearanceCell.Options.UseTextOptions = true;
            CantPresupuestadoRec.VisibleIndex = 6;
            CantPresupuestadoRec.Width = 100;
            CantPresupuestadoRec.Visible = true;
            CantPresupuestadoRec.ColumnEdit = this.editValue2Cant;
            CantPresupuestadoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantPresupuestadoRec);

            GridColumn CantSolicitadoREc = new GridColumn();
            CantSolicitadoREc.FieldName = this.unboundPrefix + "CantSolicitado";
            CantSolicitadoREc.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantSolicitado");
            CantSolicitadoREc.UnboundType = UnboundColumnType.String;
            CantSolicitadoREc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantSolicitadoREc.AppearanceCell.Options.UseTextOptions = true;
            CantSolicitadoREc.VisibleIndex = 7;
            CantSolicitadoREc.Width = 100;
            CantSolicitadoREc.Visible = true;
            CantSolicitadoREc.ColumnEdit = this.editValue2Cant;
            CantSolicitadoREc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantSolicitadoREc);

            GridColumn CantCompradoREc = new GridColumn();
            CantCompradoREc.FieldName = this.unboundPrefix + "CantComprado";
            CantCompradoREc.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantComprado");
            CantCompradoREc.UnboundType = UnboundColumnType.String;
            CantCompradoREc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantCompradoREc.AppearanceCell.Options.UseTextOptions = true;
            CantCompradoREc.VisibleIndex = 8;
            CantCompradoREc.Width = 100;
            CantCompradoREc.Visible = true;
            CantCompradoREc.ColumnEdit = this.editValue2Cant;
            CantCompradoREc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantCompradoREc);

            GridColumn CantRecibidoREc = new GridColumn();
            CantRecibidoREc.FieldName = this.unboundPrefix + "CantRecibido";
            CantRecibidoREc.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantRecibido");
            CantRecibidoREc.UnboundType = UnboundColumnType.String;
            CantRecibidoREc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantRecibidoREc.AppearanceCell.Options.UseTextOptions = true;
            CantRecibidoREc.VisibleIndex = 9;
            CantRecibidoREc.Width = 100;
            CantRecibidoREc.Visible = true;
            CantRecibidoREc.ColumnEdit = this.editValue2Cant;
            CantRecibidoREc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantRecibidoREc);

            GridColumn CantConsumidoRec = new GridColumn();
            CantConsumidoRec.FieldName = this.unboundPrefix + "CantConsumido";
            CantConsumidoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantConsumido");
            CantConsumidoRec.UnboundType = UnboundColumnType.String;
            CantConsumidoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantConsumidoRec.AppearanceCell.Options.UseTextOptions = true;
            CantConsumidoRec.VisibleIndex = 10;
            CantConsumidoRec.Width = 100;
            CantConsumidoRec.Visible = true;
            CantConsumidoRec.ColumnEdit = this.editValue2Cant;
            CantConsumidoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantConsumidoRec);


            GridColumn CantFacturadoRec = new GridColumn();
            CantFacturadoRec.FieldName = this.unboundPrefix + "CantFacturado";
            CantFacturadoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantFacturado");
            CantFacturadoRec.UnboundType = UnboundColumnType.Decimal;
            CantFacturadoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantFacturadoRec.AppearanceCell.Options.UseTextOptions = true;
            CantFacturadoRec.VisibleIndex = 11;
            CantFacturadoRec.Width = 90;
            CantFacturadoRec.Visible = true;
            CantFacturadoRec.ColumnEdit = this.editValue2Cant;
            CantFacturadoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantFacturadoRec);

            GridColumn VlrPresupuestadoREc = new GridColumn();
            VlrPresupuestadoREc.FieldName = this.unboundPrefix + "VlrPresupuestado";
            VlrPresupuestadoREc.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrPresupuestado");
            VlrPresupuestadoREc.UnboundType = UnboundColumnType.String;
            VlrPresupuestadoREc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestadoREc.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestadoREc.VisibleIndex = 12;
            VlrPresupuestadoREc.Width = 100;
            VlrPresupuestadoREc.Visible = false;
            VlrPresupuestadoREc.ColumnEdit = this.editValue2;
            VlrPresupuestadoREc.OptionsColumn.AllowEdit = false;
            VlrPresupuestadoREc.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrPresupuestadoREc.FieldName, "{0:c0}");
            this.gvRecurso.Columns.Add(VlrPresupuestadoREc);

            GridColumn VlrSolicitadoRec = new GridColumn();
            VlrSolicitadoRec.FieldName = this.unboundPrefix + "VlrSolicitado";
            VlrSolicitadoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrSolicitado");
            VlrSolicitadoRec.UnboundType = UnboundColumnType.String;
            VlrSolicitadoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrSolicitadoRec.AppearanceCell.Options.UseTextOptions = true;
            VlrSolicitadoRec.VisibleIndex = 13;
            VlrSolicitadoRec.Width = 100;
            VlrSolicitadoRec.Visible = false;
            VlrSolicitadoRec.ColumnEdit = this.editValue2;
            VlrSolicitadoRec.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSolicitadoRec.FieldName, "{0:c0}");
            VlrSolicitadoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrSolicitadoRec);

            GridColumn VlrCompradoRec = new GridColumn();
            VlrCompradoRec.FieldName = this.unboundPrefix + "VlrComprado";
            VlrCompradoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrComprado");
            VlrCompradoRec.UnboundType = UnboundColumnType.String;
            VlrCompradoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrCompradoRec.AppearanceCell.Options.UseTextOptions = true;
            VlrCompradoRec.VisibleIndex = 14;
            VlrCompradoRec.Width = 100;
            VlrCompradoRec.Visible = false;
            VlrCompradoRec.ColumnEdit = this.editValue2;
            VlrCompradoRec.OptionsColumn.AllowEdit = false;
            VlrCompradoRec.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrCompradoRec.FieldName, "{0:c0}");
            this.gvRecurso.Columns.Add(VlrCompradoRec);

            GridColumn VlrRecibidoRec = new GridColumn();
            VlrRecibidoRec.FieldName = this.unboundPrefix + "VlrRecibido";
            VlrRecibidoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrRecibido");
            VlrRecibidoRec.UnboundType = UnboundColumnType.String;
            VlrRecibidoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrRecibidoRec.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibidoRec.VisibleIndex = 15;
            VlrRecibidoRec.Width = 100;
            VlrRecibidoRec.Visible = false;
            VlrRecibidoRec.ColumnEdit = this.editValue2;
            VlrRecibidoRec.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrRecibidoRec.FieldName, "{0:c0}");
            VlrRecibidoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrRecibidoRec);

            GridColumn VlrConsumidoRec = new GridColumn();
            VlrConsumidoRec.FieldName = this.unboundPrefix + "VlrConsumido";
            VlrConsumidoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrConsumido");
            VlrConsumidoRec.UnboundType = UnboundColumnType.String;
            VlrConsumidoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrConsumidoRec.AppearanceCell.Options.UseTextOptions = true;
            VlrConsumidoRec.VisibleIndex = 16;
            VlrConsumidoRec.Width = 100;
            VlrConsumidoRec.Visible = false;
            VlrConsumidoRec.ColumnEdit = this.editValue2;
            VlrConsumidoRec.OptionsColumn.AllowEdit = false;
            VlrConsumidoRec.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrConsumidoRec.FieldName, "{0:c0}");
            this.gvRecurso.Columns.Add(VlrConsumidoRec);

            GridColumn VlrFacturadoRec = new GridColumn();
            VlrFacturadoRec.FieldName = this.unboundPrefix + "VlrFacturado";
            VlrFacturadoRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrFacturado");
            VlrFacturadoRec.UnboundType = UnboundColumnType.Decimal;
            VlrFacturadoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrFacturadoRec.AppearanceCell.Options.UseTextOptions = true;
            VlrFacturadoRec.VisibleIndex = 17;
            VlrFacturadoRec.Width = 100;
            VlrFacturadoRec.Visible = false;
            VlrFacturadoRec.ColumnEdit = this.editValue2;
            VlrFacturadoRec.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrFacturadoRec);

            //Ver
            GridColumn file = new GridColumn();
            file.FieldName = this.unboundPrefix + "ViewDoc";
            file.OptionsColumn.ShowCaption = false;
            file.UnboundType = UnboundColumnType.String;
            file.Width = 60;
            file.ColumnEdit = this.editLink;
            file.VisibleIndex = 18;
            file.Visible = true;
            file.OptionsColumn.AllowEdit = true;
            this.gvRecurso.Columns.Add(file);

            GridColumn TipoRecurso = new GridColumn();
            TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecurso.UnboundType = UnboundColumnType.Integer;
            TipoRecurso.Width = 80;
            TipoRecurso.Visible = false;
            TipoRecurso.Group();
            TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecurso.Columns.Add(TipoRecurso);


            this.gvRecurso.OptionsView.ColumnAutoWidth = true;
            #endregion

            #region Grilla Recursos Consolidado
            GridColumn RecursoIDConsol = new GridColumn();
            RecursoIDConsol.FieldName = this.unboundPrefix + "RecursoID";
            RecursoIDConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoIDConsol.UnboundType = UnboundColumnType.String;
            RecursoIDConsol.VisibleIndex = 1;
            RecursoIDConsol.Width = 85;
            RecursoIDConsol.Visible = true;
            RecursoIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(RecursoIDConsol);

            GridColumn RecursoDescConsol = new GridColumn();
            RecursoDescConsol.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDescConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDescConsol.UnboundType = UnboundColumnType.String;
            RecursoDescConsol.VisibleIndex = 2;
            RecursoDescConsol.Width = 200;
            RecursoDescConsol.Visible = true;
            RecursoDescConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(RecursoDescConsol);

            GridColumn UnidadInvIDConsol = new GridColumn();
            UnidadInvIDConsol.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDConsol.UnboundType = UnboundColumnType.String;
            UnidadInvIDConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            UnidadInvIDConsol.AppearanceCell.Options.UseTextOptions = true;
            UnidadInvIDConsol.VisibleIndex = 3;
            UnidadInvIDConsol.Width = 30;
            UnidadInvIDConsol.Visible = true;
            UnidadInvIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(UnidadInvIDConsol);

            //MarcaInvID
            GridColumn MarcaInvIDConsol = new GridColumn();
            MarcaInvIDConsol.FieldName = this.unboundPrefix + "MarcaInvID";
            MarcaInvIDConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
            MarcaInvIDConsol.UnboundType = UnboundColumnType.String;
            MarcaInvIDConsol.VisibleIndex = 4;
            MarcaInvIDConsol.Width = 60;
            MarcaInvIDConsol.Visible = true;
            MarcaInvIDConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(MarcaInvIDConsol);

            //RefProveedor
            GridColumn RefProveedorConsol = new GridColumn();
            RefProveedorConsol.FieldName = this.unboundPrefix + "RefProveedor";
            RefProveedorConsol.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
            RefProveedorConsol.UnboundType = UnboundColumnType.String;
            RefProveedorConsol.VisibleIndex = 5;
            RefProveedorConsol.Width = 60;
            RefProveedorConsol.Visible = true;
            RefProveedorConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(RefProveedorConsol);

            GridColumn CantPresupRealRec = new GridColumn();
            CantPresupRealRec.FieldName = this.unboundPrefix + "CantPresupReal";
            CantPresupRealRec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPresupReal");
            CantPresupRealRec.UnboundType = UnboundColumnType.Decimal;
            CantPresupRealRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPresupRealRec.AppearanceCell.Options.UseTextOptions = true;
            CantPresupRealRec.VisibleIndex = 5;
            CantPresupRealRec.Width = 40;
            CantPresupRealRec.Visible = true;
            CantPresupRealRec.AppearanceCell.ForeColor = Color.Gray;
            CantPresupRealRec.AppearanceCell.Options.UseForeColor = true;
            CantPresupRealRec.ColumnEdit = this.editValue2Cant;
            CantPresupRealRec.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantPresupRealRec);

            GridColumn CantPresupuestadoRecConsol = new GridColumn();
            CantPresupuestadoRecConsol.FieldName = this.unboundPrefix + "CantPresupuestado";
            CantPresupuestadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantPresupuestado");
            CantPresupuestadoRecConsol.UnboundType = UnboundColumnType.String;
            CantPresupuestadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPresupuestadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantPresupuestadoRecConsol.VisibleIndex = 6;
            CantPresupuestadoRecConsol.Width = 120;
            CantPresupuestadoRecConsol.Visible = true;
            CantPresupuestadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantPresupuestadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantPresupuestadoRecConsol);

            GridColumn CantSolicitadoRecConsol = new GridColumn();
            CantSolicitadoRecConsol.FieldName = this.unboundPrefix + "CantSolicitado";
            CantSolicitadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantSolicitado");
            CantSolicitadoRecConsol.UnboundType = UnboundColumnType.String;
            CantSolicitadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantSolicitadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantSolicitadoRecConsol.VisibleIndex = 7;
            CantSolicitadoRecConsol.Width = 120;
            CantSolicitadoRecConsol.Visible = true;
            CantSolicitadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantSolicitadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantSolicitadoRecConsol);

            GridColumn CantCompradoREcConsol = new GridColumn();
            CantCompradoREcConsol.FieldName = this.unboundPrefix + "CantComprado";
            CantCompradoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantComprado");
            CantCompradoREcConsol.UnboundType = UnboundColumnType.String;
            CantCompradoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantCompradoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            CantCompradoREcConsol.VisibleIndex = 8;
            CantCompradoREcConsol.Width = 120;
            CantCompradoREcConsol.Visible = true;
            CantCompradoREcConsol.ColumnEdit = this.editValue2Cant;
            CantCompradoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantCompradoREcConsol);

            GridColumn CantRecibidoREcConsol = new GridColumn();
            CantRecibidoREcConsol.FieldName = this.unboundPrefix + "CantRecibido";
            CantRecibidoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantRecibido");
            CantRecibidoREcConsol.UnboundType = UnboundColumnType.String;
            CantRecibidoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantRecibidoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            CantRecibidoREcConsol.VisibleIndex = 9;
            CantRecibidoREcConsol.Width = 120;
            CantRecibidoREcConsol.Visible = true;
            CantRecibidoREcConsol.ColumnEdit = this.editValue2Cant;
            CantRecibidoREcConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantRecibidoREcConsol);

            GridColumn CantConsumidoRecConsol = new GridColumn();
            CantConsumidoRecConsol.FieldName = this.unboundPrefix + "CantConsumido";
            CantConsumidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantConsumido");
            CantConsumidoRecConsol.UnboundType = UnboundColumnType.String;
            CantConsumidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantConsumidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantConsumidoRecConsol.VisibleIndex = 10;
            CantConsumidoRecConsol.Width = 120;
            CantConsumidoRecConsol.Visible = true;
            CantConsumidoRecConsol.ColumnEdit = this.editValue2Cant;
            CantConsumidoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantConsumidoRecConsol);


            GridColumn CantFacturadoRecConsol = new GridColumn();
            CantFacturadoRecConsol.FieldName = this.unboundPrefix + "CantFacturado";
            CantFacturadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_CantFacturado");
            CantFacturadoRecConsol.UnboundType = UnboundColumnType.Decimal;
            CantFacturadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantFacturadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            CantFacturadoRecConsol.VisibleIndex = 11;
            CantFacturadoRecConsol.Width = 90;
            CantFacturadoRecConsol.Visible = true;
            CantFacturadoRecConsol.ColumnEdit = this.editValue2Cant;
            CantFacturadoRecConsol.OptionsColumn.AllowEdit = false;
            this.gvRecursoConsol.Columns.Add(CantFacturadoRecConsol);

            GridColumn VlrPresupuestadoREcConsol = new GridColumn();
            VlrPresupuestadoREcConsol.FieldName = this.unboundPrefix + "VlrPresupuestado";
            VlrPresupuestadoREcConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrPresupuestado");
            VlrPresupuestadoREcConsol.UnboundType = UnboundColumnType.String;
            VlrPresupuestadoREcConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestadoREcConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestadoREcConsol.VisibleIndex = 12;
            VlrPresupuestadoREcConsol.Width = 120;
            VlrPresupuestadoREcConsol.Visible = false;
            VlrPresupuestadoREcConsol.ColumnEdit = this.editValue2;
            VlrPresupuestadoREcConsol.OptionsColumn.AllowEdit = false;
            VlrPresupuestadoREcConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrPresupuestadoREcConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrPresupuestadoREcConsol);

            GridColumn VlrSolicitadoRecConsol = new GridColumn();
            VlrSolicitadoRecConsol.FieldName = this.unboundPrefix + "VlrSolicitado";
            VlrSolicitadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrSolicitado");
            VlrSolicitadoRecConsol.UnboundType = UnboundColumnType.String;
            VlrSolicitadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrSolicitadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrSolicitadoRecConsol.VisibleIndex = 13;
            VlrSolicitadoRecConsol.Width = 120;
            VlrSolicitadoRecConsol.Visible = false;
            VlrSolicitadoRecConsol.ColumnEdit = this.editValue2;
            VlrSolicitadoRecConsol.OptionsColumn.AllowEdit = false;
            VlrSolicitadoRecConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrSolicitadoRecConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrSolicitadoRecConsol);

            GridColumn VlrCompradoRecConsol = new GridColumn();
            VlrCompradoRecConsol.FieldName = this.unboundPrefix + "VlrComprado";
            VlrCompradoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrComprado");
            VlrCompradoRecConsol.UnboundType = UnboundColumnType.String;
            VlrCompradoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrCompradoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrCompradoRecConsol.VisibleIndex = 14;
            VlrCompradoRecConsol.Width = 120;
            VlrCompradoRecConsol.Visible = false;
            VlrCompradoRecConsol.ColumnEdit = this.editValue2;
            VlrCompradoRecConsol.OptionsColumn.AllowEdit = false;
            VlrCompradoRecConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrCompradoRecConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrCompradoRecConsol);

            GridColumn VlrRecibidoRecConsol = new GridColumn();
            VlrRecibidoRecConsol.FieldName = this.unboundPrefix + "VlrRecibido";
            VlrRecibidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrRecibido");
            VlrRecibidoRecConsol.UnboundType = UnboundColumnType.String;
            VlrRecibidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrRecibidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrRecibidoRecConsol.VisibleIndex = 15;
            VlrRecibidoRecConsol.Width = 120;
            VlrRecibidoRecConsol.Visible = false;
            VlrRecibidoRecConsol.ColumnEdit = this.editValue2;
            VlrRecibidoRecConsol.OptionsColumn.AllowEdit = false;
            VlrRecibidoRecConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrRecibidoRecConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrRecibidoRecConsol);

            GridColumn VlrConsumidoRecConsol = new GridColumn();
            VlrConsumidoRecConsol.FieldName = this.unboundPrefix + "VlrConsumido";
            VlrConsumidoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrConsumido");
            VlrConsumidoRecConsol.UnboundType = UnboundColumnType.String;
            VlrConsumidoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrConsumidoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrConsumidoRecConsol.VisibleIndex = 16;
            VlrConsumidoRecConsol.Width = 120;
            VlrConsumidoRecConsol.Visible = false;
            VlrConsumidoRecConsol.ColumnEdit = this.editValue2;
            VlrConsumidoRecConsol.OptionsColumn.AllowEdit = false;
            VlrConsumidoRecConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrConsumidoRecConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrConsumidoRecConsol);

            GridColumn VlrFacturadoRecConsol = new GridColumn();
            VlrFacturadoRecConsol.FieldName = this.unboundPrefix + "VlrFacturado";
            VlrFacturadoRecConsol.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryTrazabilidad + "_VlrFacturado");
            VlrFacturadoRecConsol.UnboundType = UnboundColumnType.Decimal;
            VlrFacturadoRecConsol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrFacturadoRecConsol.AppearanceCell.Options.UseTextOptions = true;
            VlrFacturadoRecConsol.VisibleIndex = 17;
            VlrFacturadoRecConsol.Width = 90;
            VlrFacturadoRecConsol.Visible = false;
            VlrFacturadoRecConsol.ColumnEdit = this.editValue2;
            VlrFacturadoRecConsol.OptionsColumn.AllowEdit = false;
            VlrFacturadoRecConsol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, VlrFacturadoRecConsol.FieldName, "{0:c0}");
            this.gvRecursoConsol.Columns.Add(VlrFacturadoRecConsol);

            //Ver
            GridColumn fileConsol = new GridColumn();
            fileConsol.FieldName = this.unboundPrefix + "ViewDoc";
            fileConsol.OptionsColumn.ShowCaption = false;
            fileConsol.UnboundType = UnboundColumnType.String;
            fileConsol.Width = 60;
            fileConsol.ColumnEdit = this.editLink;
            fileConsol.VisibleIndex = 18;
            fileConsol.Visible = true;
            fileConsol.OptionsColumn.AllowEdit = true;
            this.gvRecursoConsol.Columns.Add(fileConsol);

            GridColumn TipoRecursoConsol = new GridColumn();
            TipoRecursoConsol.FieldName = this.unboundPrefix + "TipoRecurso";
            TipoRecursoConsol.UnboundType = UnboundColumnType.Integer;
            TipoRecursoConsol.Width = 80;
            TipoRecursoConsol.Visible = false;
            TipoRecursoConsol.Group();
            TipoRecursoConsol.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            TipoRecursoConsol.SortOrder = ColumnSortOrder.Ascending;
            this.gvRecursoConsol.Columns.Add(TipoRecursoConsol);

            this.gvRecursoConsol.OptionsView.ColumnAutoWidth = true;
            #endregion
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, true);
                this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, true);
                this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
    
                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                    this.masterPrefijo.Value = this.prefijoID;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadData(string prefijoID, int? docNro, int? numeroDoc, string proyectoID, bool actaTrabajoExist = false)
        {
            try
            {
                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, false,true,false,true,true);

                if (transaccion != null)
                {
                    if (transaccion.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }

                    this._proyectoDocu = transaccion.HeaderProyecto;
                    this._listTareasAll = transaccion.DetalleProyecto;
                    this._listMvtos = transaccion.Movimientos;
                    this._ctrlProyecto = transaccion.DocCtrl;
                    this._listTrazResumen = transaccion.ResumenTrazabilidad;

                    this.masterProyecto.Value = transaccion.DocCtrl.ProyectoID.Value;
                    this.masterPrefijo.Value = transaccion.DocCtrl.PrefijoID.Value;
                    this.txtNro.Text = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                    this.masterCliente.Value = transaccion.HeaderProyecto.ClienteID.Value;
                    this.txtLicitacion.Text = transaccion.HeaderProyecto.Licitacion.Value;
                    this.txtDescripcion.Text = transaccion.HeaderProyecto.DescripcionSOL.Value;

                    this.txtVlrContratado.EditValue = transaccion.DetalleProyecto.Sum(x=>x.CostoLocalCLI.Value);
                    this.txtVlrPresupuesto.EditValue = transaccion.DetalleProyecto.Sum(x => x.CostoTotalML.Value);
                    this.txtVlrFacturado.EditValue = this._listTrazResumen.Sum(x => x.VlrFacturado.Value);
                    this.txtVlrComprado.EditValue = this._listTrazResumen.Sum(x => x.VlrComprado.Value);
                    this.txtVlrConsumido.EditValue = this._listTrazResumen.Sum(x => x.VlrConsumido.Value);
                    this.txtVlrRecibido.EditValue = this._listTrazResumen.Sum(x => x.VlrRecibido.Value);  

                    this.LoadGrids();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids(bool exportData = false)
        {
            try
            {
                this._listTrazResumen = this._listTrazResumen.OrderBy(x=>x.ConsecTarea.Value).ToList();
                this.gcTarea.DataSource = this._listTrazResumen;

                List<DTO_QueryTrazabilidad> detaAll = new List<DTO_QueryTrazabilidad>();
                List<DTO_QueryTrazabilidad> detaDistinct = new List<DTO_QueryTrazabilidad>();
                foreach (DTO_QueryTrazabilidad traz in this._listTrazResumen)
                    detaAll.AddRange(traz.Detalle);

                List<string> recursosDist = detaAll.Select(x => x.RecursoID.Value).Distinct().ToList();
                if (!exportData) //Llena datos para las grillas
                {
                    foreach (string rec in recursosDist)
                    {
                        DTO_QueryTrazabilidad d = ObjectCopier.Clone(detaAll.Find(x => x.RecursoID.Value == rec));
                        d.Detalle = detaAll.FindAll(x => x.RecursoID.Value == rec);
                        d.CantPresupuestado.Value = d.Detalle.Sum(x => x.CantPresupuestado.Value);
                        d.CantSolicitado.Value = d.Detalle.Sum(x => x.CantSolicitado.Value);
                        d.CantComprado.Value = d.Detalle.Sum(x => x.CantComprado.Value);
                        d.CantRecibido.Value = d.Detalle.Sum(x => x.CantRecibido.Value);
                        d.CantConsumido.Value = d.Detalle.Sum(x => x.CantConsumido.Value);
                        d.CantFacturado.Value = d.Detalle.Sum(x => x.CantFacturado.Value);
                        d.VlrPresupuestado.Value = d.Detalle.Sum(x => x.VlrPresupuestado.Value);
                        d.VlrSolicitado.Value = d.Detalle.Sum(x => x.VlrSolicitado.Value);
                        d.VlrComprado.Value = d.Detalle.Sum(x => x.VlrComprado.Value);
                        d.VlrRecibido.Value = d.Detalle.Sum(x => x.VlrRecibido.Value);
                        d.VlrConsumido.Value = d.Detalle.Sum(x => x.VlrConsumido.Value);
                        d.VlrFacturado.Value = d.Detalle.Sum(x => x.VlrFacturado.Value);
                        detaDistinct.Add(d);
                    }

                    this.gcRecursoConsol.DataSource = detaDistinct;

                    this.gcTarea.RefreshDataSource();
                    this.gcRecurso.RefreshDataSource();
                }
                else // Llena lista para Exportar
                {
                    foreach (string rec in recursosDist)
                    {
                        DTO_QueryTrazabilidad d = detaAll.Find(x => x.RecursoID.Value == rec);
                        d.Detalle = detaAll.FindAll(x => x.RecursoID.Value == rec);
                        DTO_ExportTrazabilidad export = new DTO_ExportTrazabilidad();
                        export.RecursoID.Value = d.RecursoID.Value;
                        export.RecursoDesc.Value = d.RecursoDesc.Value;
                        export.MarcaInvID.Value = d.MarcaInvID.Value;
                        export.RefProveedor.Value = d.RefProveedor.Value;
                        export.TareaCliente.Value = d.TareaCliente.Value;
                        export.TareaDesc.Value = d.TareaDesc.Value;
                        export.UnidadInvID.Value = d.UnidadInvID.Value;
                        export.CantPresupuestado.Value = d.Detalle.Sum(x => x.CantPresupuestado.Value);
                        export.CantSolicitado.Value = d.Detalle.Sum(x => x.CantSolicitado.Value);
                        export.CantComprado.Value = d.Detalle.Sum(x => x.CantComprado.Value);
                        export.CantRecibido.Value = d.Detalle.Sum(x => x.CantRecibido.Value);
                        export.CantConsumido.Value = d.Detalle.Sum(x => x.CantConsumido.Value);
                        export.CantFacturado.Value = d.Detalle.Sum(x => x.CantFacturado.Value);
                        export.VlrPresupuestado.Value = d.Detalle.Sum(x => x.VlrPresupuestado.Value);
                        export.VlrSolicitado.Value = d.Detalle.Sum(x => x.VlrSolicitado.Value);
                        export.VlrComprado.Value = d.Detalle.Sum(x => x.VlrComprado.Value);
                        export.VlrRecibido.Value = d.Detalle.Sum(x => x.VlrRecibido.Value);
                        export.VlrConsumido.Value = d.Detalle.Sum(x => x.VlrConsumido.Value);
                        export.VlrFacturado.Value = d.Detalle.Sum(x => x.VlrFacturado.Value);
                        this._listExportExcel.Add(export);
                    }
                }
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

            this.masterProyecto.Value = string.Empty;
            this.masterPrefijo.Value = string.Empty; 
            this.txtNro.Text = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.txtLicitacion.Text = string.Empty;
            this.txtDescripcion.Text =string.Empty;
            this.txtVlrContratado.EditValue = 0;
            this.txtVlrFacturado.EditValue = 0;
            this.txtVlrPresupuesto.EditValue = 0;
            this.txtVlrComprado.EditValue = 0;
            this.txtVlrConsumido.EditValue = 0;
            this.txtVlrRecibido.EditValue = 0;

            this._ctrlProyecto = null;
            this._numeroDoc = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu();
            this._rowTarea = new DTO_QueryTrazabilidad();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasAdicion = new List<DTO_pyProyectoTarea>();
            this._listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
            this._listTrazResumen = new  List<DTO_QueryTrazabilidad>();
            this._dtoClaseServicio = new DTO_pyClaseProyecto();
            this.gcTarea.DataSource = this._listTrazResumen;
            this.gcTarea.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();
            this.gcRecursoConsol.DataSource = null;
            this.gcRecursoConsol.RefreshDataSource();
            this.masterPrefijo.Value = this.prefijoID;
            this.masterProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppQueries.QueryTrazabilidad;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
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
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "Form_FormClosed"));
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
        /// Verifica si hay un documento Existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_Leave(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNro.Text) && !string.IsNullOrEmpty(this.masterPrefijo.Value))
            {
                int docNro = Convert.ToInt32(this.txtNro.Text);
                DTO_glDocumentoControl docCtrl = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.masterPrefijo.Value, docNro);
                if (docCtrl != null)
                    this.LoadData(this.masterPrefijo.Value, docNro, null, string.Empty);
                else
                {
                    string pref = this.masterPrefijo.Value;
                    MessageBox.Show("Este proyecto o documento no existe");
                    this.RefreshForm();
                    this.masterPrefijo.Value = pref;
                    this.txtNro.Text = docNro.ToString();
                    this.masterPrefijo.Focus();
                }
                this.gvRecursoConsol.ActiveFilterString = string.Empty;
            }
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdGroupVer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdGroupVer.SelectedIndex == 0)
                {
                    #region Muestras las Cantidades
                    #region Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 3;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 4;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 5;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 6;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 7;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 8;

                    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;

                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = false; 
                    #endregion
                    #region Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 6;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 7;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 8;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 9;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 11;
                    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 13;

                    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                    ///this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = false; 
                    #endregion
                    #region Grilla Recursos Consolidado
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 6;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 7;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 8;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 9;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 10;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 11;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 13;

                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                    ///this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrComprado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrRecibido"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrConsumido"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrFacturado"].Visible = false;
                    #endregion
                    #endregion
                    this.gvRecursoConsol.OptionsView.ShowFooter = false;
                    this.gvTareas.OptionsView.ShowFooter = false;
                }
                else if (this.rdGroupVer.SelectedIndex == 1)
                {
                    #region Muestra Valores
                    #region Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 11;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;

                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;

                    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = false; 
                    #endregion
                    #region Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 11;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;
                    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 16;

                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                    //this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = false; 
                    #endregion
                    #region Grilla Recursos Consolidado
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 10;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 11;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 12;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 13;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 14;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 15;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 16;

                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                    //this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].Visible = true;

                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantSolicitado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantComprado"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantRecibido"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantConsumido"].Visible = false;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantFacturado"].Visible = false;
                    #endregion
                    #endregion
                    this.gvRecursoConsol.OptionsView.ShowFooter = true;
                    this.gvTareas.OptionsView.ShowFooter = true;
                }
                else if (this.rdGroupVer.SelectedIndex == 2)
                {
                    #region Muestras las Cantidades y Valores
                    #region Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "TareaDesc"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 3;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 4;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 5;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 6;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 7;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 8;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 9;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 10;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 11;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 12;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 13;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 14;

                    this.gvTareas.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    //this.gvTareas.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true; 
                    #endregion
                    #region Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 6;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 7;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 8;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 9;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 11;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 12;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 13;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 14;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 15;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 16;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 17;
                    this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 18;

                    this.gvRecurso.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                    //this.gvRecurso.Columns[this.unboundPrefix + "ViewDoc"].Visible = true; 
                    #endregion
                    #region Grilla Recursos Consolidado
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantPresupuestado"].VisibleIndex = 6;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantSolicitado"].VisibleIndex = 7;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantComprado"].VisibleIndex = 8;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantRecibido"].VisibleIndex = 9;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantConsumido"].VisibleIndex = 10;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantFacturado"].VisibleIndex = 11;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 13;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrSolicitado"].VisibleIndex = 14;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrComprado"].VisibleIndex = 15;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrRecibido"].VisibleIndex = 16;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrConsumido"].VisibleIndex = 17;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrFacturado"].VisibleIndex = 18;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].VisibleIndex = 19;

                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantPresupuestado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantSolicitado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantComprado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantRecibido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantConsumido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "CantFacturado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrSolicitado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrComprado"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrRecibido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrConsumido"].Visible = true;
                    this.gvRecursoConsol.Columns[this.unboundPrefix + "VlrFacturado"].Visible = true;
                    //this.gvRecursoConsol.Columns[this.unboundPrefix + "ViewDoc"].Visible = true; 
                    #endregion
                    #endregion
                    this.gvRecursoConsol.OptionsView.ShowFooter = true;
                    this.gvTareas.OptionsView.ShowFooter = true;
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad", "rdGroupVer_SelectedIndexChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            if (this.masterProyecto.ValidID)
            {
                this.LoadData(string.Empty, null, null, this.masterProyecto.Value);
                this.gvRecursoConsol.ActiveFilterString = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.Proyecto);
                ModalFindDocSolicitud getDocControl = new ModalFindDocSolicitud(docs, false, true);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                    this.LoadData(getDocControl.DocumentoControl.PrefijoID.Value, getDocControl.DocumentoControl.DocumentoNro.Value, null, string.Empty);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        #endregion

        #region Eventos Grilla

        #region Tareas

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
                    this._rowTarea = (DTO_QueryTrazabilidad)this.gvTareas.GetRow(e.FocusedRowHandle);
                    this.gcRecurso.DataSource = this._rowTarea.Detalle;
                    this.gcRecurso.RefreshDataSource();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "gvDocument_FocusedRowChanged"));
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
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                }
                if (e.Value == null && pi != null && pi.PropertyType.Name == "UDT_Cantidad")
                    e.Value = 0;
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                        pi.PropertyType.Name == "Double")
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" ||
                            pi.PropertyType.Name == "Double")
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
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);            
        }

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                DTO_QueryTrazabilidad currentRow = (DTO_QueryTrazabilidad)this.gvTareas.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                    //if (currentRow.DetalleInd.Value.Value)
                    //    e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    //else
                    //    e.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Recurso-Trabajo

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                    this._rowDetalle = (DTO_QueryTrazabilidad)this.gvRecurso.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna mascaras
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
                    //double rowValue = Convert.ToDouble(this.gvRecurso.GetGroupRowValue(e.GroupRowHandle, e.Column));
                    if (Convert.ToByte(e.Value) == 1)
                        e.DisplayText = "MATERIALES";
                    else if (Convert.ToByte(e.Value) == 2)
                        e.DisplayText = "EQUIPO-HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 3)
                        e.DisplayText = "MANO DE OBRA";
                    else if (Convert.ToByte(e.Value) == 4)
                        e.DisplayText = "TRANSPORTES";
                    else if (Convert.ToByte(e.Value) == 5)
                        e.DisplayText = "HERRAMIENTA";
                    else if (Convert.ToByte(e.Value) == 6)
                        e.DisplayText = "SOFTWARE";
                }
                else if (e.Column.FieldName == this.unboundPrefix + "ViewDoc")
                    e.DisplayText = _bc.GetResource(LanguageTypes.Messages, "Ver Doc. Anexos");

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        #endregion        

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl.SelectedTabPageIndex == 0)
                {
                    List<DTO_glDocumentoControl> ctrlsAnexos = this._bc.AdministrationModel.pyProyectoMvto_GetDocsAnexo(this._rowDetalle.ConsecMvto.Value);
                    ModalViewDocuments viewDocs = new ModalViewDocuments(ctrlsAnexos, Convert.ToByte(this.rdGroupVer.SelectedIndex));
                    viewDocs.Show();
                }
                else
                {
                    DTO_QueryTrazabilidad row = (DTO_QueryTrazabilidad)this.gvRecursoConsol.GetRow(this.gvRecursoConsol.FocusedRowHandle);
                    List<DTO_glDocumentoControl> ctrlsAnexosAll = new List<DTO_glDocumentoControl>();
                    List<DTO_glDocumentoControl> ctrlsAnexosDistinc = new List<DTO_glDocumentoControl>();
                    if (row != null)
                    {
                        foreach (var r in row.Detalle)
                            ctrlsAnexosAll.AddRange(this._bc.AdministrationModel.pyProyectoMvto_GetDocsAnexo(r.ConsecMvto.Value));
                        List<int?> docDistinct = ctrlsAnexosAll.Select(x => x.DocumentoID.Value).Distinct().ToList();
                        ctrlsAnexosAll = ctrlsAnexosAll.OrderBy(x => x.DocumentoID.Value).ThenBy(x => x.PrefDoc.Value).ToList();
                        //Filtra los documentos por ID y PrefDoc (Evita unir docs diferentes con el mismo PrefDoc)
                        foreach (int? docID in docDistinct)
	                    {
                            List<string> ctrlDistinct = ctrlsAnexosAll.FindAll(y => y.DocumentoID.Value == docID).Select(x => x.PrefDoc.Value).Distinct().ToList();
		                    foreach (string d in ctrlDistinct)
                            {
                                DTO_glDocumentoControl doc = ctrlsAnexosAll.Find(x => x.PrefDoc.Value == d && x.DocumentoID.Value == docID);
                                //doc.Cantidad.Value = ctrlsAnexosAll.FindAll(x => x.PrefDoc.Value == d && x.DocumentoID.Value == docID).Sum(y => y.Cantidad.Value);
                                //doc.Valor.Value = ctrlsAnexosAll.FindAll(x => x.PrefDoc.Value == d && x.DocumentoID.Value == docID).Sum(y => y.Valor.Value);
                                ctrlsAnexosDistinc.Add(doc);
                            } 
	                    }
                        ModalViewDocuments viewDocs = new ModalViewDocuments(ctrlsAnexosDistinc, Convert.ToByte(this.rdGroupVer.SelectedIndex));
                        viewDocs.Show(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "editLink_Click"));
            }
        }


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
                this.LoadData(string.Empty, null, null, this.masterProyecto.Value, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBExport()
        {
            try
            {
                if (this.masterProyecto.ValidID)
                {
                    DataTableOperations tableOp = new DataTableOperations();
                    this.LoadGrids(true);
                    List<DTO_ExportTareas> tmp = new List<DTO_ExportTareas>();                  
                    System.Data.DataTable tableAll = tableOp.Convert_GenericListToDataTable(typeof(DTO_ExportTrazabilidad), this._listExportExcel);
                    
                    if (this.rdGroupVer.SelectedIndex == 0)
                    {
                        tableAll.Columns.Remove("VlrPresupuestado");
                        tableAll.Columns.Remove("VlrSolicitado");
                        tableAll.Columns.Remove("VlrComprado");
                        tableAll.Columns.Remove("VlrRecibido");
                        tableAll.Columns.Remove("VlrConsumido");
                        tableAll.Columns.Remove("VlrFacturado");
                    }
                    else if (this.rdGroupVer.SelectedIndex == 1)
                    {
                        tableAll.Columns.Remove("CantPresupuestado");
                        tableAll.Columns.Remove("CantSolicitado");
                        tableAll.Columns.Remove("CantComprado");
                        tableAll.Columns.Remove("CantRecibido");
                        tableAll.Columns.Remove("CantConsumido");
                        tableAll.Columns.Remove("CantFacturado");
                    }             
                    ReportExcelBase frm = new ReportExcelBase(tableAll,this.documentID);
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaTrazabilidad.cs", "TBExport"));
            }
        }

        #endregion

    }
}
