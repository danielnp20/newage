using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;
using System.Threading;
using DevExpress.XtraEditors;
using NewAge.DTO.Resultados;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ActaEntrega : FormWithToolbar
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
        private int _numeroDocProy = 0;
        private int _numeroDocActa = 0;
        private bool isValid = true;
        private bool deleteOP = false;
        //Variables de datos
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_glDocumentoControl _ctrlActa = null;
        private DTO_coProyecto _proyecto = null;
        private DTO_pyActaEntregaDeta _rowEntrega = new DTO_pyActaEntregaDeta();
        private DTO_pyProyectoTareaCliente _rowTareaClienteCurrent = null;
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdic = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTareaCliente> _listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();
        private List<DTO_pyProyectoTareaCliente> _listTareasCliente = new List<DTO_pyProyectoTareaCliente>();
        private List<DTO_pyActaEntregaDeta> _listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
        #endregion

        #region Delegados
        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void SendToApproveMethod() { this.RefreshForm(); } 
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ActaEntrega()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "ActaEntrega"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Header
            //Seleccionar
            GridColumn aprob = new GridColumn();
            aprob.FieldName = this.unboundPrefix + "SelectInd";
            aprob.Caption = "√";
            aprob.UnboundType = UnboundColumnType.Boolean;
            aprob.AppearanceHeader.ForeColor = Color.Lime;
            aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            aprob.AppearanceHeader.Options.UseTextOptions = true;
            aprob.AppearanceHeader.Options.UseFont = true;
            aprob.AppearanceHeader.Options.UseForeColor = true;
            aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms,"Seleccionar");
            aprob.VisibleIndex = 0;
            aprob.Width = 35;
            aprob.Visible = true;
            this.gvHeader.Columns.Add(aprob);

            GridColumn TareaEntregable = new GridColumn();
            TareaEntregable.FieldName = this.unboundPrefix + "TareaEntregable";
            TareaEntregable.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_TareaCliente");
            TareaEntregable.UnboundType = UnboundColumnType.String;
            TareaEntregable.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            TareaEntregable.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaEntregable.AppearanceCell.Options.UseTextOptions = true;
            TareaEntregable.AppearanceCell.Options.UseFont = true;
            TareaEntregable.VisibleIndex = 1;
            TareaEntregable.Width = 30;
            TareaEntregable.Visible = true;
            //TareaEntregable.ColumnEdit = this.editCmb;
            TareaEntregable.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(TareaEntregable);

            GridColumn Descripcion = new GridColumn();
            Descripcion.FieldName = this.unboundPrefix + "Descripcion";
            Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descripcion.UnboundType = UnboundColumnType.String;
            Descripcion.VisibleIndex = 2;
            Descripcion.Width = 175;
            Descripcion.Visible = true;
            Descripcion.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Descripcion);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.VisibleIndex = 3;
            Cantidad.Width = 40;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            Cantidad.AppearanceCell.BackColor = Color.Gainsboro;
            Cantidad.AppearanceCell.Options.UseBackColor = true;
            this.gvHeader.Columns.Add(Cantidad);

            GridColumn PorEntregado = new GridColumn();
            PorEntregado.FieldName = this.unboundPrefix + "PorEntregado";
            PorEntregado.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorEntregado");
            PorEntregado.UnboundType = UnboundColumnType.String;
            PorEntregado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PorEntregado.AppearanceCell.Options.UseTextOptions = true;
            PorEntregado.AppearanceHeader.ForeColor = Color.Aquamarine;
            PorEntregado.AppearanceHeader.Options.UseForeColor = true;
            PorEntregado.VisibleIndex = 4;
            PorEntregado.Width = 45;
            PorEntregado.Visible = true;
            PorEntregado.ColumnEdit = this.editValue2Cant;
            PorEntregado.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(PorEntregado);

            GridColumn CantidadEntregada = new GridColumn();
            CantidadEntregada.FieldName = this.unboundPrefix + "CantEntregada";
            CantidadEntregada.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantEntregada");
            CantidadEntregada.UnboundType = UnboundColumnType.String;
            CantidadEntregada.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadEntregada.AppearanceCell.Options.UseTextOptions = true;
            CantidadEntregada.AppearanceHeader.ForeColor = Color.Orange;
            CantidadEntregada.AppearanceHeader.Options.UseForeColor = true;
            CantidadEntregada.VisibleIndex = 5;
            CantidadEntregada.Width = 45;
            CantidadEntregada.Visible = true;
            CantidadEntregada.ColumnEdit = this.editValue2Cant;
            CantidadEntregada.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(CantidadEntregada);

            GridColumn ValorFactura = new GridColumn();
            ValorFactura.FieldName = this.unboundPrefix + "ValorFactura";
            ValorFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorFactura");
            ValorFactura.UnboundType = UnboundColumnType.String;
            ValorFactura.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorFactura.AppearanceCell.Options.UseTextOptions = true;
            ValorFactura.AppearanceCell.Options.UseFont = true;
            ValorFactura.VisibleIndex = 6;
            ValorFactura.Width = 60;
            ValorFactura.Visible = true;
            ValorFactura.ColumnEdit = this.editValue2;
            ValorFactura.AppearanceCell.BackColor = Color.Gainsboro;
            ValorFactura.AppearanceCell.Options.UseBackColor = true;
            ValorFactura.OptionsColumn.AllowEdit = false;
            ValorFactura.Summary.Add(DevExpress.Data.SummaryItemType.Sum, ValorFactura.FieldName, "{0:c0}");
            this.gvHeader.Columns.Add(ValorFactura);

            GridColumn ValorEntregado = new GridColumn();
            ValorEntregado.FieldName = this.unboundPrefix + "ValorEntregado";
            ValorEntregado.Caption = _bc.GetResource(LanguageTypes.Forms,"Valor Entregado");
            ValorEntregado.UnboundType = UnboundColumnType.String;
            ValorEntregado.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorEntregado.AppearanceCell.Options.UseTextOptions = true;
            ValorEntregado.AppearanceCell.Options.UseFont = true;
            ValorEntregado.VisibleIndex = 7;
            ValorEntregado.Width = 60;
            ValorEntregado.Visible = true;
            ValorEntregado.ColumnEdit = this.editValue2;
            ValorEntregado.OptionsColumn.AllowEdit = false;
            ValorEntregado.Summary.Add(DevExpress.Data.SummaryItemType.Sum, ValorEntregado.FieldName, "{0:c0}");
            this.gvHeader.Columns.Add(ValorEntregado);

            GridColumn ValorAEntregar = new GridColumn();
            ValorAEntregar.FieldName = this.unboundPrefix + "ValorAEntregar";
            ValorAEntregar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAEntregar");
            ValorAEntregar.UnboundType = UnboundColumnType.String;
            ValorAEntregar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorAEntregar.AppearanceCell.Options.UseTextOptions = true;
            ValorAEntregar.AppearanceCell.Options.UseFont = true;
            ValorAEntregar.VisibleIndex = 8;
            ValorAEntregar.Width = 55;
            ValorAEntregar.Visible = true;
            ValorAEntregar.ColumnEdit = this.editValue2;
            ValorAEntregar.OptionsColumn.AllowEdit = false;
            ValorAEntregar.Summary.Add(DevExpress.Data.SummaryItemType.Sum, ValorAEntregar.FieldName, "{0:c0}");
            this.gvHeader.Columns.Add(ValorAEntregar);

            GridColumn PorAEntregar = new GridColumn();
            PorAEntregar.FieldName = this.unboundPrefix + "PorAEntregar";
            PorAEntregar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorAEntregar");
            PorAEntregar.UnboundType = UnboundColumnType.String;
            PorAEntregar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PorAEntregar.AppearanceCell.Options.UseTextOptions = true;
            PorAEntregar.AppearanceCell.Options.UseFont = true;
            PorAEntregar.VisibleIndex = 9;
            PorAEntregar.Width = 60;
            PorAEntregar.Visible = true;
            PorAEntregar.ColumnEdit = this.editPorc;
            PorAEntregar.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(PorAEntregar);

            GridColumn Observaciones = new GridColumn();
            Observaciones.FieldName = this.unboundPrefix + "Observaciones";
            Observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observaciones");
            Observaciones.UnboundType = UnboundColumnType.String;
            Observaciones.VisibleIndex = 10;
            Observaciones.Width = 110;
            Observaciones.Visible = true;
            Observaciones.ColumnEdit = this.editPoPup;
            Observaciones.OptionsColumn.AllowEdit = true;
            this.gvHeader.Columns.Add(Observaciones);
           

            this.gvHeader.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Grilla Detalle

            GridColumn FechaEntrega = new GridColumn();
            FechaEntrega.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
            FechaEntrega.UnboundType = UnboundColumnType.DateTime;
            FechaEntrega.VisibleIndex = 1;
            FechaEntrega.Width = 30;
            FechaEntrega.Visible = true;
            FechaEntrega.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FechaEntrega);

            GridColumn ObservacionesDet = new GridColumn();
            ObservacionesDet.FieldName = this.unboundPrefix + "Observaciones";
            ObservacionesDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observaciones");
            ObservacionesDet.UnboundType = UnboundColumnType.String;
            ObservacionesDet.VisibleIndex = 2;
            ObservacionesDet.Width = 100;
            ObservacionesDet.ColumnEdit = this.editPoPup;
            ObservacionesDet.Visible = true;
            ObservacionesDet.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(ObservacionesDet);

            GridColumn PorEntregaProg = new GridColumn();
            PorEntregaProg.FieldName = this.unboundPrefix + "PorEntregaProg";
            PorEntregaProg.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorEntrega");
            PorEntregaProg.UnboundType = UnboundColumnType.Decimal;
            PorEntregaProg.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PorEntregaProg.AppearanceCell.Options.UseTextOptions = true;
            PorEntregaProg.AppearanceHeader.ForeColor = Color.Aquamarine;
            PorEntregaProg.AppearanceHeader.Options.UseForeColor = true;
            PorEntregaProg.VisibleIndex = 3;
            PorEntregaProg.Width = 35;
            PorEntregaProg.Visible = true;
            PorEntregaProg.ColumnEdit = this.editPorc;
            PorEntregaProg.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(PorEntregaProg);

            GridColumn PorEntregadoDet = new GridColumn();
            PorEntregadoDet.FieldName = this.unboundPrefix + "PorEntregado";
            PorEntregadoDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorEntregado");
            PorEntregadoDet.UnboundType = UnboundColumnType.Decimal;
            PorEntregadoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PorEntregadoDet.AppearanceCell.Options.UseTextOptions = true;
            PorEntregadoDet.AppearanceHeader.ForeColor = Color.Aquamarine;
            PorEntregadoDet.AppearanceHeader.Options.UseForeColor = true;
            PorEntregadoDet.VisibleIndex = 4;
            PorEntregadoDet.Width = 35;
            PorEntregadoDet.Visible = false;
            PorEntregadoDet.ColumnEdit = this.editPorc;
            PorEntregadoDet.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(PorEntregadoDet);

            GridColumn PorPendiente = new GridColumn();
            PorPendiente.FieldName = this.unboundPrefix + "PorPendiente";
            PorPendiente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorPendiente");
            PorPendiente.UnboundType = UnboundColumnType.Decimal;
            PorPendiente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PorPendiente.AppearanceCell.Options.UseFont = true;
            PorPendiente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PorPendiente.AppearanceCell.Options.UseTextOptions = true;
            PorPendiente.AppearanceHeader.ForeColor = Color.Aquamarine;
            PorPendiente.AppearanceHeader.Options.UseForeColor = true;
            PorPendiente.VisibleIndex = 5;
            PorPendiente.Width = 35;
            PorPendiente.Visible = true;
            PorPendiente.ColumnEdit = this.editPorc;
            PorPendiente.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(PorPendiente);

            GridColumn PorAEntregarDet = new GridColumn();
            PorAEntregarDet.FieldName = this.unboundPrefix + "PorAEntregar";
            PorAEntregarDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorAEntregar");
            PorAEntregarDet.UnboundType = UnboundColumnType.String;
            PorAEntregarDet.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PorAEntregarDet.AppearanceCell.Options.UseTextOptions = true;
            PorAEntregarDet.AppearanceCell.Options.UseFont = true;
            PorAEntregarDet.AppearanceHeader.ForeColor = Color.Aquamarine;
            PorAEntregarDet.AppearanceHeader.Options.UseForeColor = true;
            PorAEntregarDet.VisibleIndex = 6;
            PorAEntregarDet.Width = 60;
            PorAEntregarDet.Visible = true;
            PorAEntregarDet.ColumnEdit = this.editPorc;
            PorAEntregarDet.OptionsColumn.AllowEdit = true;
            PorAEntregarDet.AppearanceCell.BackColor = Color.Gainsboro;
            PorAEntregarDet.AppearanceCell.Options.UseBackColor = true;
            this.gvDetalle.Columns.Add(PorAEntregarDet);

            GridColumn CantProgramada = new GridColumn();
            CantProgramada.FieldName = this.unboundPrefix + "CantProgramada";
            CantProgramada.Caption = _bc.GetResource(LanguageTypes.Forms, "Cant Program");
            CantProgramada.UnboundType = UnboundColumnType.Decimal;
            CantProgramada.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantProgramada.AppearanceCell.Options.UseTextOptions = true;
            CantProgramada.AppearanceHeader.ForeColor = Color.Orange;
            CantProgramada.AppearanceHeader.Options.UseForeColor = true;
            CantProgramada.VisibleIndex = 7;
            CantProgramada.Width = 35;
            CantProgramada.Visible = true;
            CantProgramada.ColumnEdit = this.editValue2Cant;
            CantProgramada.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantProgramada);

            GridColumn CantFinal = new GridColumn();
            CantFinal.FieldName = this.unboundPrefix + "Cantidad";
            CantFinal.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            CantFinal.UnboundType = UnboundColumnType.Decimal;
            CantFinal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantFinal.AppearanceCell.Options.UseTextOptions = true;
            CantFinal.AppearanceHeader.ForeColor = Color.Orange;
            CantFinal.AppearanceHeader.Options.UseForeColor = true;
            CantFinal.VisibleIndex = 8;
            CantFinal.Width = 35;
            CantFinal.Visible = false;
            CantFinal.ColumnEdit = this.editValue2Cant;
            CantFinal.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantFinal);

            GridColumn CantEntregada = new GridColumn();
            CantEntregada.FieldName = this.unboundPrefix + "CantEntregada";
            CantEntregada.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantEntregada");
            CantEntregada.UnboundType = UnboundColumnType.Decimal;
            CantEntregada.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantEntregada.AppearanceCell.Options.UseTextOptions = true;
            CantEntregada.AppearanceHeader.ForeColor = Color.Orange;
            CantEntregada.AppearanceHeader.Options.UseForeColor = true;
            CantEntregada.VisibleIndex = 9;
            CantEntregada.Width = 35;
            CantEntregada.Visible = false;
            CantEntregada.ColumnEdit = this.editValue2Cant;
            CantEntregada.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantEntregada);

            GridColumn CantPendiente = new GridColumn();
            CantPendiente.FieldName = this.unboundPrefix + "CantPendiente";
            CantPendiente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantPendiente");
            CantPendiente.UnboundType = UnboundColumnType.Decimal;
            CantPendiente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantPendiente.AppearanceCell.Options.UseFont = true;
            CantPendiente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantPendiente.AppearanceCell.Options.UseTextOptions = true;
            CantPendiente.AppearanceHeader.ForeColor = Color.Orange;
            CantPendiente.AppearanceHeader.Options.UseForeColor = true;
            CantPendiente.VisibleIndex = 10;
            CantPendiente.Width = 35;
            CantPendiente.Visible = true;
            CantPendiente.ColumnEdit = this.editValue2Cant;
            CantPendiente.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(CantPendiente);

            GridColumn CantAEntregar = new GridColumn();
            CantAEntregar.FieldName = this.unboundPrefix + "CantAEntregar";
            CantAEntregar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantAEntregar");
            CantAEntregar.UnboundType = UnboundColumnType.String;
            CantAEntregar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantAEntregar.AppearanceCell.Options.UseTextOptions = true;
            CantAEntregar.AppearanceHeader.ForeColor = Color.Orange;
            CantAEntregar.AppearanceHeader.Options.UseForeColor = true;
            CantAEntregar.AppearanceCell.Options.UseFont = true;
            CantAEntregar.VisibleIndex = 11;
            CantAEntregar.Width = 60;
            CantAEntregar.Visible = true;
            CantAEntregar.ColumnEdit = this.editValue2Cant;
            CantAEntregar.OptionsColumn.AllowEdit = true;
            CantAEntregar.AppearanceCell.BackColor = Color.Gainsboro;
            CantAEntregar.AppearanceCell.Options.UseBackColor = true;
            this.gvDetalle.Columns.Add(CantAEntregar);

            GridColumn FacturaInd = new GridColumn();
            FacturaInd.FieldName = this.unboundPrefix + "FacturaInd";
            FacturaInd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FacturaInd");
            FacturaInd.UnboundType = UnboundColumnType.Boolean;
            FacturaInd.VisibleIndex = 12;
            FacturaInd.Width = 20;
            FacturaInd.Visible = true;
            FacturaInd.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FacturaInd);

            GridColumn ValorAEntregarDet = new GridColumn();
            ValorAEntregarDet.FieldName = this.unboundPrefix + "ValorAEntregar";
            ValorAEntregarDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAEntregar");
            ValorAEntregarDet.UnboundType = UnboundColumnType.Decimal;
            ValorAEntregarDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorAEntregarDet.AppearanceCell.Options.UseTextOptions = true;
            ValorAEntregarDet.VisibleIndex = 13;
            ValorAEntregarDet.Width = 40;
            ValorAEntregarDet.Visible = true;
            ValorAEntregarDet.ColumnEdit = this.editValue2;
            ValorAEntregarDet.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(ValorAEntregarDet);

            GridColumn ValorFacturaDet = new GridColumn();
            ValorFacturaDet.FieldName = this.unboundPrefix + "ValorFactura";
            ValorFacturaDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorFactura");
            ValorFacturaDet.UnboundType = UnboundColumnType.Decimal;
            ValorFacturaDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorFacturaDet.AppearanceCell.Options.UseTextOptions = true;
            ValorFacturaDet.VisibleIndex = 14;
            ValorFacturaDet.Width = 40;
            ValorFacturaDet.Visible = false;
            ValorFacturaDet.ColumnEdit = this.editValue2;
            ValorFacturaDet.OptionsColumn.AllowEdit = true;
            this.gvDetalle.Columns.Add(ValorFacturaDet);

            #endregion

            #region Grilla Tareas         
            GridColumn IndexTarea = new GridColumn();
            IndexTarea.FieldName = this.unboundPrefix + "Index";
            IndexTarea.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Index");
            IndexTarea.UnboundType = UnboundColumnType.Integer;
            IndexTarea.VisibleIndex = 0;
            IndexTarea.Width = 15;
            IndexTarea.Visible = false;
            IndexTarea.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(IndexTarea);

            GridColumn TareaCliente = new GridColumn();
            TareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            TareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaCliente");
            TareaCliente.UnboundType = UnboundColumnType.String;
            TareaCliente.VisibleIndex = 1;
            TareaCliente.Width = 28;
            TareaCliente.Visible = true;
            TareaCliente.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaCliente);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 2;
            TareaID.Width = 35;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaID);

            GridColumn Descriptivo = new GridColumn();
            Descriptivo.FieldName = this.unboundPrefix + "Descriptivo";
            Descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descriptivo.UnboundType = UnboundColumnType.String;
            Descriptivo.VisibleIndex = 3;
            Descriptivo.Width = 90;
            Descriptivo.Visible = true;
            Descriptivo.OptionsColumn.AllowEdit = true;
            this.gvTareas.Columns.Add(Descriptivo);
            #endregion

            //Agrega Columnas a ComboGrid
            GridColumn col1 = this.editGridCmb.View.Columns.AddField("TareaEntregable");
            col1.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_TareaCliente");
            col1.UnboundType = UnboundColumnType.String;
            col1.VisibleIndex = 0;

            GridColumn col2 = this.editGridCmb.View.Columns.AddField("Descripcion");
            col2.UnboundType = UnboundColumnType.String;
            col2.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            col2.VisibleIndex = 1;
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRowTarea()
        {
            try
            {
                //DTO_pyProyectoTareaCliente footerDet = new DTO_pyProyectoTareaCliente();
                //footerDet.ValorFactura.Value = 0;
                //footerDet.ValorAEntregar.Value = 0;
                //this._listTareasActa.Add(footerDet);
                ////this._rowTareaClienteCurrent = footerDet;

                //this.gcHeader.DataSource = this._listTareasActa;
                //this.gcHeader.RefreshDataSource();
                //this.gvHeader.FocusedRowHandle = this.gvHeader.DataRowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "AddNewRow: " + ex.Message));
            }
        }

        /// <summary>
        ///Carga la info del acta antes de guardar
        /// </summary>
        private void CreateActa()
        {
            try
            {
                if (this._numeroDocActa == 0)
                {
                    #region Carga DocumentoControl
                    this._ctrlActa = new DTO_glDocumentoControl();
                    this._ctrlActa.DocumentoID.Value = AppDocuments.ActaEntrega;
                    this._ctrlActa.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this._ctrlActa.MonedaID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.MonedaID.Value;
                    this._ctrlActa.ProyectoID.Value = this.ucProyecto.ProyectoID;
                    this._ctrlActa.Fecha.Value = DateTime.Now.Date;
                    this._ctrlActa.PeriodoDoc.Value = this.dtFechaActa.DateTime;
                    this._ctrlActa.PrefijoID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.PrefijoID.Value;
                    this._ctrlActa.TasaCambioCONT.Value = 0;
                    this._ctrlActa.TasaCambioDOCU.Value = 0;
                    this._ctrlActa.DocumentoNro.Value = 0;                  
                    this._ctrlActa.PeriodoUltMov.Value = this.dtFechaActa.DateTime;
                    this._ctrlActa.seUsuarioID.Value = this.userID;
                    this._ctrlActa.AreaFuncionalID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.AreaFuncionalID.Value;
                    this._ctrlActa.ConsSaldo.Value = !this._proyecto.ConsActaEntrega.Value.HasValue? 1 : this._proyecto.ConsActaEntrega.Value;
                    this._ctrlActa.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                    this._ctrlActa.Observacion.Value = this.txtObservacion.Text;
                    this._ctrlActa.FechaDoc.Value = this.dtFechaActa.DateTime;
                    this._ctrlActa.Descripcion.Value = "Acta de Entrega Proyecto";
                    this._ctrlActa.DocumentoTercero.Value = this.txtDocTercero.Text;
                    this._ctrlActa.Valor.Value = 0;
                    this._ctrlActa.Iva.Value = 0;
                    #endregion            
                    foreach (DTO_pyProyectoTareaCliente tarea in this._listTareasCliente)
                    {
                        foreach (DTO_pyActaEntregaDeta det in tarea.DetalleActas)
                        {
                            det.EntregaFinalInd.Value = det.PorPendiente.Value == 0 ? true : false;
                            det.PorEntregado.Value = det.PorAEntregar.Value;
                            det.Cantidad.Value = det.CantAEntregar.Value;
                            det.RespCliente.Value = this.txtRespCliente.Text;
                            det.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                        }
                    }
                }
                else
                {
                    #region Carga DocumentoControl
                    this._ctrlActa = this._bc.AdministrationModel.glDocumentoControl_GetByID(_numeroDocActa);
                    this._ctrlActa.ConsSaldo.Value = !this._ctrlActa.ConsSaldo.Value.HasValue || this._ctrlActa.ConsSaldo.Value == 0 ? this._proyecto.ConsActaEntrega.Value : this._ctrlActa.ConsSaldo.Value;
                    #endregion
                    foreach (DTO_pyProyectoTareaCliente tarea in this._listTareasCliente)
                    {
                        foreach (DTO_pyActaEntregaDeta det in tarea.DetalleActas)
                        {
                            det.NumeroDoc.Value = this._ctrlActa.NumeroDoc.Value;
                            det.EntregaFinalInd.Value = det.PorPendiente.Value == 0 ? true : false;
                            det.PorEntregado.Value = det.PorAEntregar.Value;
                            det.Cantidad.Value = det.CantAEntregar.Value;
                            det.RespCliente.Value = this.txtRespCliente.Text;
                            det.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "CreateActaDeta"));
            }
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this.dtFechaActa.DateTime = DateTime.Today;
                this.ucProyecto.Init(false,false,false,false);
                this.ucProyecto.LoadProyectoInfo_Leave += new UC_Proyecto.EventHandler(this.ucProyecto_LoadProyectoInfo_Click);       
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "InitControls"));
            }
        }
        
        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadActaExistente()
        {
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.txtNroActa.Text) && this._numeroDocProy != 0)
                    {
                        int consActa = Convert.ToInt32(this.txtNroActa.Text);
                        DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                        filter.DocumentoID.Value = AppDocuments.ActaEntrega;
                        filter.ProyectoID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.ProyectoID.Value;
                        filter.ConsSaldo.Value = consActa;
                        filter.Estado.Value = 2;//Para aprobacion
                        List<DTO_glDocumentoControl> ctrlList = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                        if (ctrlList != null && ctrlList.Count > 0)
                        {
                            this._numeroDocActa = ctrlList.First().NumeroDoc.Value.Value;
                            DTO_pyActaEntregaDeta actaFilter = new DTO_pyActaEntregaDeta();
                            actaFilter.NumDocProyecto.Value = this._numeroDocProy;
                            actaFilter.NumeroDoc.Value = this._numeroDocActa;
                            this._listActaDetaExist = this._bc.AdministrationModel.pyActaEntregaDeta_GetByParameter(actaFilter);

                            if (this._listActaDetaExist.Count > 0)
                            {
                                foreach (DTO_pyActaEntregaDeta ac in this._listActaDetaExist)
                                {
                                    DTO_pyProyectoTareaCliente tarCli = this._listTareasCliente.Find(x => x.Consecutivo.Value == ac.ConsTareaCliente.Value);
                                    tarCli.DetalleActas.RemoveAll(x => x.Consecutivo.Value == ac.Consecutivo.Value);
                                    if (tarCli.DetalleActas.Any(x=>x.Estado.Value == (byte)EstadoDocControl.Aprobado))//Si quedan actas es porque ya han sido aprobadas
                                    {
                                        DTO_pyActaEntregaDeta exist = tarCli.DetalleActas.Find(x => x.Estado.Value == (byte)EstadoDocControl.Aprobado);
                                        exist.CantAEntregar.Value = ac.Cantidad.Value;
                                        exist.DocumentoNro.Value = ctrlList.First().DocumentoNro.Value;
                                        exist.PorAEntregar.Value = ac.PorEntregado.Value;
                                        exist.ValorAEntregar.Value = ac.ValorFactura.Value;
                                        exist.ValorFactura.Value = tarCli.ValorFactura.Value;
                                        tarCli.ValorAEntregar.Value = tarCli.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                                        tarCli.PorAEntregar.Value = tarCli.DetalleActas.Sum(x => x.PorAEntregar.Value);

                                        if (!tarCli.DetalleActas.Any(x => x.PorAEntregar.Value != 0))
                                            tarCli.SelectInd.Value = false;
                                        else
                                            tarCli.SelectInd.Value = true;
                                    }
                                    else
                                    {
                                        ac.CantProgramada.Value = tarCli.Detalle.Where(x => x.Consecutivo.Value == ac.ConsTareaEntrega.Value).Sum(x => x.Cantidad.Value);
                                        ac.CantAEntregar.Value = ac.Cantidad.Value;
                                        ac.CantPendiente.Value = tarCli.Cantidad.Value - ac.Cantidad.Value;
                                        ac.Descripcion.Value = tarCli.Descripcion.Value;
                                        ac.DocumentoNro.Value = ctrlList.First().DocumentoNro.Value;
                                        ac.PorAEntregar.Value = ac.PorEntregado.Value;
                                        ac.PorPendiente.Value = ac.PorEntregaProg.Value - ac.PorEntregado.Value;
                                        ac.TareaEntregable.Value = tarCli.TareaEntregable.Value;
                                        ac.ValorAEntregar.Value = ac.ValorFactura.Value;
                                        ac.ValorFactura.Value = tarCli.ValorFactura.Value;
                                        tarCli.DetalleActas.Add(ac);
                                        tarCli.ValorAEntregar.Value = tarCli.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                                        tarCli.PorAEntregar.Value = tarCli.DetalleActas.Sum(x => x.PorAEntregar.Value);

                                        if (!tarCli.DetalleActas.Any(x => x.PorAEntregar.Value != 0))
                                            tarCli.SelectInd.Value = false;
                                        else
                                            tarCli.SelectInd.Value = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this._listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
                            this._numeroDocActa = 0;
                            this._proyecto = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this._ctrlProyecto.ProyectoID.Value, true);
                            this._proyecto.ConsActaEntrega.Value = this._proyecto.ConsActaEntrega.Value ?? 0;
                            this._proyecto.ConsActaEntrega.Value = this._proyecto.ConsActaEntrega.Value + 1;
                            this.txtNroActa.Text = this._proyecto.ConsActaEntrega.Value.ToString();
                        }
                        this.LoadGrids();
                        this.gcDetalle.RefreshDataSource();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-txtNroActa_Leave", "LoadData"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
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
                this.gcHeader.DataSource = this._listTareasCliente;
                this.gcHeader.RefreshDataSource();
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
            this._ctrlProyecto = null;
            this._proyecto = null;
            this._numeroDocProy = 0;
            this._numeroDocActa = 0;
            this._rowEntrega = new DTO_pyActaEntregaDeta();
            this._rowTareaClienteCurrent = null;
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasAdic = new List<DTO_pyProyectoTarea>();
            this._listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();
            this._listTareasCliente = new  List<DTO_pyProyectoTareaCliente>();
            this._listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
            this.gcHeader.DataSource = null;
            this.gcHeader.RefreshDataSource();
            this.ucProyecto.CleanControl();
            this.gcDetalle.DataSource = null;
            this.gcDetalle.RefreshDataSource();
            this.gcTareas.DataSource = null;
            this.gcTareas.RefreshDataSource();
            this.btnQueryDoc.Enabled = true;
            this.txtDocTercero.Text = string.Empty;
            this.txtNroActa.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.txtRespCliente.Text = string.Empty;
            this.isValid = true;

            FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.ActaEntrega;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be, List<DTO_glConsultaFiltro> filtros = null)
        {
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                if (col == "TareaID")
                {
                    props.DocumentoID = AppMasters.pyTarea;
                    props.DTOTipo = "DTO_pyTarea";
                    props.ModuloID = ModulesPrefix.py;
                    props.NombreTabla = "pyTarea";
                }

                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, filtros);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false, filtros);
                    modal.ShowDialog();
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0)
                {
                    #region TareaEntregable
                    validField = this._bc.ValidGridCell(this.gvHeader, this.unboundPrefix, fila, "TareaEntregable", false, false, false, null);
                    if (!validField)
                        validRow = false;
                    else
                    {
                        int count = this._listTareasCliente.Count(x => x.TareaEntregable.Value == this._rowTareaClienteCurrent.TareaEntregable.Value);
                        if (count > 1)
                        {
                            GridColumn col = this.gvHeader.Columns[this.unboundPrefix + "TareaEntregable"];
                            string colVal = this.gvHeader.GetRowCellValue(fila, col).ToString();
                            this.gvHeader.SetColumnError(col, string.Format(this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_AlreadyExistInGrid), this._rowTareaClienteCurrent.TareaEntregable.Value));
                            validRow = false;
                        }
                    }
                    #endregion                    
                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "ValidateRow"));
            }
            return validRow;
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRowDet(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                if (fila >= 0)
                {
                    GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + "PorAEntregar"];
                    if (this._rowEntrega != null && (this._rowEntrega.PorEntregaProg.Value) < this._rowEntrega.PorAEntregar.Value)
                    {
                        this.gvDetalle.SetColumnError(col, "El Porcentaje no puede ser negativo");
                        validRow = false;
                    }
                    else
                    {
                        this.gvDetalle.SetColumnError(col, "");
                        validRow = true;
                    }

                    #region Observaciones
                    //validField = this._bc.ValidGridCell(this.gvDetalle, this.unboundPrefix, fila, "Observaciones", false, false, false, null);
                    //if (!validField)
                    //    validRow = false;                    
                    #endregion                    
                    if (validRow)
                        this.isValid = true;
                    else
                        this.isValid = false;
                }
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "ValidateRowDet"));
            }
            return validRow;
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
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.itemPrint.Enabled = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_FormClosed"));
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
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucProyecto_LoadProyectoInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ucProyecto.ProyectoInfo != null)
                {
                    if (this.ucProyecto.ProyectoInfo.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                    {
                        MessageBox.Show("El Proyecto no se encuentra Aprobado");
                        return;
                    }
                    this._numeroDocProy = this.ucProyecto.ProyectoInfo.DocCtrl.NumeroDoc.Value.Value;
                    this._listTareasAll = this.ucProyecto.ProyectoInfo.DetalleProyecto;
                    this._listTareasAdic = this.ucProyecto.ProyectoInfo.DetalleProyectoTareaAdic;
                    this._ctrlProyecto = this.ucProyecto.ProyectoInfo.DocCtrl;
                    this._listTareasCliente = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);
                    this.txtRespCliente.Text = this.ucProyecto.ProyectoInfo.HeaderProyecto.ResponsableCLI.Value;
                    this._proyecto = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this._ctrlProyecto.ProyectoID.Value, true);
                    this._proyecto.ConsActaEntrega.Value = this._proyecto.ConsActaEntrega.Value?? 0;
                    this.txtNroActa.Text = this._proyecto.ConsActaEntrega.Value.ToString();
                    this.LoadActaExistente();
                    this.LoadGrids();
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                    this._proyecto = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras", "LoadData"));
            }
        }

        /// <summary>
        /// Trae un acta existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroActa_Leave(object sender, EventArgs e)
        {
            if (this._proyecto != null && this._proyecto.ConsActaEntrega.Value.ToString().Equals(this.txtNroActa.EditValue))
            {
                this.LoadActaExistente();
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
            }
            else
            {
                FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemSave.Enabled = false;
            }
        }

        /// <summary>
        /// Consultas las actas existentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.ActaEntrega);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtNroActa.Enabled = true;
                this.txtNroActa.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.txtNroActa.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
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
        private void gvHeader_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.gvHeader.DataRowCount > 0)
                {
                    this._rowTareaClienteCurrent = (DTO_pyProyectoTareaCliente)this.gvHeader.GetRow(e.FocusedRowHandle);
                    this.gcDetalle.DataSource = null;
                    this.gcDetalle.DataSource = this._rowTareaClienteCurrent.DetalleActas;
                    this.gcDetalle.RefreshDataSource();

                    //Tareas
                    this.gcTareas.DataSource = this._rowTareaClienteCurrent.DetalleTareas;
                    this.gcTareas.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvHeader_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnGrid_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                string colName = this.gvHeader.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                ButtonEdit origin = (ButtonEdit)sender;
                if (colName.Equals("MonedaFactura"))
                    colName = "MonedaID";
                this.ShowFKModal(this.gvHeader.FocusedRowHandle, colName, origin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCostos.cs", "editBtnGrid_ButtonClick"));
            }
        }

        /// <summary>
        /// Boton eliminar de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcHeader_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                #region Agregar

                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                {
                    if (this.gvHeader.ActiveFilterString != string.Empty)
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                    else
                    {
                        this.deleteOP = false;
                        if (this.isValid)
                            this.AddNewRowTarea();
                        else
                        {
                            bool isV = this.ValidateRow(this.gvHeader.FocusedRowHandle);
                            if (isV)
                                this.AddNewRowTarea();
                        }
                    }
                }
                #endregion

                #region Eliminar
                if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                {
                    string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                    string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);
                    e.Handled = true;
                    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.deleteOP = true;
                        int rowHandle = this.gvHeader.FocusedRowHandle;

                        if (this._listTareasCliente.Count > 0)
                        {
                            this._listTareasCliente.RemoveAll(x => x.TareaEntregable.Value == this._rowTareaClienteCurrent.TareaEntregable.Value &&
                                                                x.Descripcion.Value == this._rowTareaClienteCurrent.Descripcion.Value);
                            //Si borra el primer registro
                            if (rowHandle == 0)
                                this.gvHeader.FocusedRowHandle = 0;
                            //Si selecciona el ultimo
                            else
                                this.gvHeader.FocusedRowHandle = rowHandle - 1;

                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Entregables.cs", "gcDocument_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Al modificar las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHeader_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvHeader.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "ValorFactura")
            {
                this._rowTareaClienteCurrent.ValorFactura.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.ValorFactura.Value);
                this.gvHeader.RefreshData();
            }
        }

        /// <summary>
        /// Al modificar las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHeader_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvHeader.Columns[this.unboundPrefix + fieldName];

            try
            {
                if (fieldName == "TareaEntregable")
                {
                    DTO_pyProyectoTareaCliente tarea = this._listEntregablesProy.Find(x => x.TareaEntregable.Value == e.Value.ToString());
                    if (tarea != null)
                    {
                        this._rowTareaClienteCurrent.TareaEntregable.Value= tarea.TareaEntregable.Value;
                        this._rowTareaClienteCurrent.ValorFactura.Value = tarea.ValorFactura.Value;
                        this._rowTareaClienteCurrent.Observaciones.Value = tarea.Observaciones.Value;
                        this._rowTareaClienteCurrent.Descripcion.Value = this._listEntregablesProy.Find(x=>x.TareaEntregable.Value == e.Value.ToString()).Descripcion.Value;
                        this._rowTareaClienteCurrent.DetalleActas = tarea.DetalleActas;
                        this._rowTareaClienteCurrent.PorAEntregar.Value = tarea.DetalleActas.Sum(x => x.PorAEntregar.Value);
                        this._rowTareaClienteCurrent.ValorAEntregar.Value = tarea.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                        this.gcDetalle.DataSource = null;
                        this.gcDetalle.DataSource = this._rowTareaClienteCurrent.DetalleActas;
                        this.gcDetalle.RefreshDataSource();
                        this.gvHeader.RefreshData();

                        //Tareas
                        this.gcTareas.DataSource = this._rowTareaClienteCurrent.DetalleTareas;
                        this.gcTareas.RefreshDataSource();
                    }
                 }
                if (fieldName == "SelectInd")
                {
                    if (Convert.ToBoolean(e.Value))
                    {
                        foreach (DTO_pyActaEntregaDeta acta in this._rowTareaClienteCurrent.DetalleActas)
                        {
                            acta.CantAEntregar.Value = acta.CantPendiente.Value;
                            acta.PorAEntregar.Value = acta.PorPendiente.Value;

                            acta.CantPendiente.Value = 0;
                            acta.PorPendiente.Value = 0;
                            acta.ValorAEntregar.Value = Math.Round(acta.PorEntregaProg.Value != 0 ? (acta.PorAEntregar.Value.Value * acta.ValorFactura.Value.Value) / acta.PorEntregaProg.Value.Value : 0, 2);
                            this._rowTareaClienteCurrent.ValorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                            this._rowTareaClienteCurrent.PorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.PorAEntregar.Value);

                            this.gvDetalle.RefreshData();
                            this.gvHeader.RefreshRow(this.gvHeader.FocusedRowHandle);
                            this.isValid = true;
                        } 
                    }
                    else
                    {
                        foreach (DTO_pyActaEntregaDeta acta in this._rowTareaClienteCurrent.DetalleActas)
                        {
                            acta.CantPendiente.Value += acta.CantAEntregar.Value;
                            acta.PorPendiente.Value += acta.PorAEntregar.Value;

                            acta.CantAEntregar.Value = 0;
                            acta.PorAEntregar.Value = 0;
                            acta.ValorAEntregar.Value = Math.Round(acta.PorEntregaProg.Value != 0 ? (acta.PorAEntregar.Value.Value * acta.ValorFactura.Value.Value) / acta.PorEntregaProg.Value.Value : 0, 2);
                            this._rowTareaClienteCurrent.ValorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                            this._rowTareaClienteCurrent.PorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.PorAEntregar.Value);

                            this.gvDetalle.RefreshData();
                            this.gvHeader.RefreshRow(this.gvHeader.FocusedRowHandle);
                            this.isValid = true;
                        }
                    }
                }             
                this.gvHeader.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvHeader_CellValueChanging"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvHeader_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.deleteOP && this.gvHeader.DataRowCount > 0)
                    this.ValidateRow(e.RowHandle);

                if (!this.isValid)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvDetalle_BeforeLeaveRow"));
            }
        }

        #endregion

        #region Detalle

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (!this.deleteOP && this.gvDetalle.DataRowCount > 0)
                    this.ValidateRowDet(e.RowHandle);

                if (!this.isValid)
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvDetalle_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this.gvDetalle.DataRowCount > 0)
                    this._rowEntrega = (DTO_pyActaEntregaDeta)this.gvDetalle.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvRecurso_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvDetalle.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "PorAEntregar")
            {
                //Calcula cantidad de acuerdo al % digitado sobre la cantidad programada
                this._rowEntrega.CantAEntregar.Value = (((decimal)e.Value) * this._rowEntrega.CantProgramada.Value) / 100;
                if (this._rowEntrega.PorAEntregar.Value  >= 0)
                {
                    //Porcentajes
                    this._rowEntrega.PorPendiente.Value = this._rowEntrega.PorEntregaProg.Value - this._rowTareaClienteCurrent.PorEntregado.Value -  this._rowEntrega.PorAEntregar.Value;
                    this._rowEntrega.ValorAEntregar.Value = Math.Round(this._rowEntrega.PorEntregaProg.Value != 0 ? (this._rowEntrega.PorAEntregar.Value.Value * this._rowEntrega.ValorFactura.Value.Value) / this._rowEntrega.PorEntregaProg.Value.Value : 0, 2);
                    this._rowTareaClienteCurrent.ValorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                    this._rowTareaClienteCurrent.PorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.PorAEntregar.Value);

                    //Cantidades: Cant Program - Cant ya entregada - cant a entregar actual
                    this._rowEntrega.CantPendiente.Value = this._rowEntrega.CantProgramada.Value - this._rowTareaClienteCurrent.CantEntregada.Value - this._rowEntrega.CantAEntregar.Value;
                    
                    this.gvDetalle.RefreshRow(this.gvDetalle.FocusedRowHandle);
                    this.gvHeader.RefreshRow(this.gvHeader.FocusedRowHandle);
                    this.gvDetalle.SetColumnError(col, "");
                    this.isValid = true;
                }
                else
                { 
                    this.gvDetalle.SetColumnError(col,"El Porcentaje no puede ser negativo");
                    this.isValid = false;
                }

                if (!this._rowTareaClienteCurrent.DetalleActas.Any(x=>x.PorAEntregar.Value != 0))
                    this._rowTareaClienteCurrent.SelectInd.Value = false;
                else
                    this._rowTareaClienteCurrent.SelectInd.Value = true;
            }
            else if (fieldName == "CantAEntregar")
            {
                //Calcula porcentaje de acuerdo a la cantidad digitada sobre la cantidad programada
                this._rowEntrega.PorAEntregar.Value = (100 * (decimal)e.Value) / this._rowEntrega.CantProgramada.Value;
                if (this._rowEntrega.PorAEntregar.Value  >= 0)
                {
                    //Porcentajes
                    this._rowEntrega.PorPendiente.Value = this._rowEntrega.PorEntregaProg.Value - this._rowTareaClienteCurrent.PorEntregado.Value - this._rowEntrega.PorAEntregar.Value;
                    this._rowEntrega.ValorAEntregar.Value = Math.Round(this._rowEntrega.PorEntregaProg.Value != 0 ? (this._rowEntrega.PorAEntregar.Value.Value * this._rowEntrega.ValorFactura.Value.Value) / this._rowEntrega.PorEntregaProg.Value.Value : 0, 2);
                    this._rowTareaClienteCurrent.ValorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.ValorAEntregar.Value);
                    this._rowTareaClienteCurrent.PorAEntregar.Value = this._rowTareaClienteCurrent.DetalleActas.Sum(x => x.PorAEntregar.Value);

                    //Cantidades
                    this._rowEntrega.CantPendiente.Value = this._rowEntrega.CantProgramada.Value - this._rowTareaClienteCurrent.CantEntregada.Value - this._rowEntrega.CantAEntregar.Value;

                    this.gvDetalle.RefreshRow(this.gvDetalle.FocusedRowHandle);
                    this.gvHeader.RefreshRow(this.gvHeader.FocusedRowHandle);
                    this.gvDetalle.SetColumnError(col, "");
                    this.isValid = true;
                }
                else
                {
                    this.gvDetalle.SetColumnError(col, "La cantidad no puede ser negativa");
                    this.isValid = false;
                }

                if (!this._rowTareaClienteCurrent.DetalleActas.Any(x => x.PorAEntregar.Value != 0))
                    this._rowTareaClienteCurrent.SelectInd.Value = false;
                else
                    this._rowTareaClienteCurrent.SelectInd.Value = true;

                this.gvHeader.UpdateSummary();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.gvDetalle.FocusedRowHandle >= 0)
                    this._rowEntrega = (DTO_pyActaEntregaDeta)this.gvDetalle.GetRow(this.gvDetalle.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gvRecurso_FocusedRowChanged"));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcDetalle_Leave(object sender, EventArgs e)
        {
            try
            {
                this.gvDetalle.PostEditor();
                this.ValidateRowDet(this.gvDetalle.FocusedRowHandle);
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "gcDetalle_Leave"));
            }
        }


        #endregion        

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSave()
        {        
            this.gvHeader.PostEditor();
            this.gvDetalle.PostEditor();
            if (this.ValidateRow(this.gvHeader.FocusedRowHandle) && this.ValidateRowDet(this.gvDetalle.FocusedRowHandle))
            {
                this.CreateActa();
                //if (this._listTareasCliente.Any(x => x.SelectInd.Value.Value) && this._listTareasCliente.Any(x => x.DetalleActas.Count(y=>y.PorAEntregar.Value != 0) > 0))
                    this.SaveThread();
                //else
                //    MessageBox.Show("No ha seleccionado ningún item con cantidad válida para crear o actualizar el acta");
            }
        }

        /// <summary>
        /// Salvar
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.gvHeader.PostEditor();
            this.gvDetalle.PostEditor();
            if (this.ValidateRow(this.gvHeader.FocusedRowHandle) && this.ValidateRowDet(this.gvDetalle.FocusedRowHandle))
            {
                this.CreateActa();
                if (this._listTareasCliente.Any(x => x.SelectInd.Value.Value))
                    this.ApproveThread();
                else
                    MessageBox.Show("No ha seleccionado ningún item  con cantidad válida para aprobar el acta");
            }
        }

        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.RefreshForm();
        }

        /// <summary>
        /// Eliminar
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();

                //Trae las facturas de ventas del proyecto
                filter.DatoAdd4.Value = this._ctrlProyecto.NumeroDoc.Value.ToString();
                filter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                List<DTO_glMovimientoDeta> mvtosFact = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, false);
                mvtosFact = mvtosFact.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado).ToList();

                if (mvtosFact.Count > 0)
                {
                    string facturas = string.Empty;
                    foreach (var fac in mvtosFact)
                        facturas += (!facturas.Contains(fac.Prefijo_Documento.Value) ? fac.Prefijo_Documento.Value + " " : string.Empty);
                    MessageBox.Show("El proyecto actual tiene ya Facturas de Venta aprobadas,si desea eliminar los datos. Facturas: " + facturas);
                    return;
                }

                if (MessageBox.Show("Desea eliminar los entregables del proyecto?", "Eliminar " + this.Text, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool deleteProgramacion = this._listEntregablesProy.Any(x => x.Detalle.Count > 0) ? true : false;
                    bool deleteActas = this._listEntregablesProy.Any(x => x.DetalleActas.Count > 0) ? true : false;
                    bool anulaPreFacturas = this._listEntregablesProy.Any(x => x.DetalleActas.Any(y => y.NumDocFactura.Value.HasValue)) ? true : false;

                    DTO_TxResult res = this._bc.AdministrationModel.EntregasCliente_Delete(this.documentID, this._numeroDocProy, this._listTareasCliente, false, false, true, anulaPreFacturas);
                    this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this.userID.ToString(), res, true, false);
                    this.RefreshForm();
                }
            }
            catch (Exception ex)
            { ; }

        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                DTO_SolicitudTrabajo solicitud = ObjectCopier.Clone(this.ucProyecto.ProyectoInfo);
                int docNro = !string.IsNullOrEmpty(this.txtNroActa.Text) ? Convert.ToInt32(this.txtNroActa.Text) : 1;
                string fileURl = this._bc.AdministrationModel.Reportes_py_ActaEntrega(solicitud, docNro,this.dtFechaActa.DateTime);
                //Process.Start(fileURl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                var res = this._bc.AdministrationModel.ActaEntrega_Add(this.documentID, this._ctrlActa, this._listTareasCliente.FindAll(x=>x.SelectInd.Value.Value),this._proyecto,this._numeroDocActa == 0? false : true);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._bc.AdministrationModel.User.ID.Value, res, true, true);
                if (res.GetType() == typeof(DTO_Alarma))
                {
                    DTO_Alarma alarma = (DTO_Alarma)res;
                    this._numeroDocActa = 0;// Convert.ToInt32(alarma.NumeroDoc);
                    this.gcHeader.DataSource = null;
                    this.gcDetalle.DataSource = null;
                    this.gcTareas.DataSource = null;
                    this._listTareasCliente = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);
                    this.LoadActaExistente();                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "SaveThread"));
            }
            finally
            {              
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Aprobar la información del proceso
        /// </summary>
        public void ApproveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.NOK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                var res = this._bc.AdministrationModel.ActaEntrega_Aprobar(this.documentID, this._ctrlActa, this._proyecto);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._bc.AdministrationModel.User.ID.Value, res, true, true);
                if (res.GetType() == typeof(DTO_Alarma))
                {
                    DTO_Alarma alarma = (DTO_Alarma)res;
                    this._numeroDocActa = 0;// Convert.ToInt32(alarma.NumeroDoc);
                    this.gcHeader.DataSource = null;
                    this.gcDetalle.DataSource = null;
                    this.gcTareas.DataSource = null;
                    this._listTareasCliente = new List<DTO_pyProyectoTareaCliente>();// this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);
                    this.LoadGrids();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion
      
    }
}
