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
    public partial class ConsultaComiteTecnico : FormWithToolbar
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
        private DTO_QueryComiteTecnico _rowCurrent = new DTO_QueryComiteTecnico();
        private GridView _gridDetalleCurrent = new GridView();
        private DTO_QueryComiteTecnicoTareas _rowTareaCurrent = new DTO_QueryComiteTecnicoTareas();
        private List<DTO_QueryComiteTecnico> _listProyectos = new List<DTO_QueryComiteTecnico>();
        #endregion        
        
        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaComiteTecnico()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "ConsultaComiteTecnico"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Grilla Proyectos
            GridColumn ResponsableDesc = new GridColumn();
            ResponsableDesc.FieldName = this.unboundPrefix + "ResponsableDesc";
            ResponsableDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ResponsableDesc");
            ResponsableDesc.UnboundType = UnboundColumnType.String;
            //ResponsableDesc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            //ResponsableDesc.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ResponsableDesc.AppearanceCell.Options.UseTextOptions = true;
            ResponsableDesc.AppearanceCell.Options.UseFont = true;
            ResponsableDesc.VisibleIndex = 1;
            ResponsableDesc.Width = 90;
            ResponsableDesc.Visible = true;
            ResponsableDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ResponsableDesc);
            

            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 2;
            ProyectoID.Width = 45;
            ProyectoID.Visible = true;
            ProyectoID.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoID);

            GridColumn ProyectoDesc = new GridColumn();
            ProyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
            ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoDesc");
            ProyectoDesc.UnboundType = UnboundColumnType.String;
            ProyectoDesc.VisibleIndex = 3;
            ProyectoDesc.Width = 90;
            ProyectoDesc.Visible = true;
            ProyectoDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoDesc);

            GridColumn PrefDoc = new GridColumn();
            PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
            PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
            PrefDoc.UnboundType = UnboundColumnType.String;
            PrefDoc.VisibleIndex = 4;
            PrefDoc.Width = 35;
            PrefDoc.Visible = true;
            PrefDoc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(PrefDoc);

            GridColumn ClienteID = new GridColumn();
            ClienteID.FieldName = this.unboundPrefix + "ClienteID";
            ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteID");
            ClienteID.UnboundType = UnboundColumnType.String;
            ClienteID.VisibleIndex = 5;
            ClienteID.Width = 50;
            ClienteID.Visible = true;
            ClienteID.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ClienteID);

            GridColumn ClienteDesc = new GridColumn();
            ClienteDesc.FieldName = this.unboundPrefix + "ClienteDesc";
            ClienteDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ClienteDesc");
            ClienteDesc.UnboundType = UnboundColumnType.String;
            ClienteDesc.VisibleIndex = 6;
            ClienteDesc.Width = 90;
            ClienteDesc.Visible = true;
            ClienteDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ClienteDesc);

            GridColumn FechaInicio = new GridColumn();
            FechaInicio.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaInicio");
            FechaInicio.UnboundType = UnboundColumnType.DateTime;
            FechaInicio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaInicio.AppearanceCell.Options.UseTextOptions = true;
            FechaInicio.VisibleIndex = 7;
            FechaInicio.Width = 50;
            FechaInicio.Visible = true;
            FechaInicio.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(FechaInicio);

            GridColumn FechaFin = new GridColumn();
            FechaFin.FieldName = this.unboundPrefix + "FechaFin";
            FechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaFin");
            FechaFin.UnboundType = UnboundColumnType.DateTime;
            FechaFin.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFin.AppearanceCell.Options.UseTextOptions = true;
            FechaFin.VisibleIndex = 8;
            FechaFin.Width = 50;
            FechaFin.Visible = true;
            FechaFin.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(FechaFin);         

            GridColumn DiasAtrasoProy = new GridColumn();
            DiasAtrasoProy.FieldName = this.unboundPrefix + "DiasAtrasoProy";
            DiasAtrasoProy.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoProy");
            DiasAtrasoProy.UnboundType = UnboundColumnType.Integer;
            DiasAtrasoProy.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoProy.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoProy.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoProy.AppearanceCell.Options.UseFont = true;
            DiasAtrasoProy.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoProy.VisibleIndex = 11;
            DiasAtrasoProy.Width = 50;
            DiasAtrasoProy.Visible = true;
            DiasAtrasoProy.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoProy);

            GridColumn DiasAtrasoCompras = new GridColumn();
            DiasAtrasoCompras.FieldName = this.unboundPrefix + "DiasAtrasoCompras";
            DiasAtrasoCompras.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoCompras");
            DiasAtrasoCompras.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoCompras.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoCompras.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoCompras.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoCompras.AppearanceCell.Options.UseFont = true;
            DiasAtrasoCompras.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoCompras.VisibleIndex = 12;
            DiasAtrasoCompras.Width = 50;
            DiasAtrasoCompras.Visible = true;
            DiasAtrasoCompras.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoCompras);

            GridColumn DiasAtrasoEntrega = new GridColumn();
            DiasAtrasoEntrega.FieldName = this.unboundPrefix + "DiasAtrasoEntrega";
            DiasAtrasoEntrega.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoEntrega");
            DiasAtrasoEntrega.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoEntrega.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoEntrega.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoEntrega.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoEntrega.AppearanceCell.Options.UseFont = true;
            DiasAtrasoEntrega.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoEntrega.VisibleIndex = 13;
            DiasAtrasoEntrega.Width = 50;
            DiasAtrasoEntrega.Visible = true;
            DiasAtrasoEntrega.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoEntrega);
            this.gvProyectos.OptionsView.ColumnAutoWidth = true;

            #endregion
            #region Grilla Tareas
            GridColumn TareaCliente = new GridColumn();
            TareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
            TareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaCliente");
            TareaCliente.UnboundType = UnboundColumnType.String;
            //TareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //TareaCliente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaCliente.AppearanceCell.Options.UseTextOptions = true;
            TareaCliente.AppearanceCell.Options.UseFont = true;
            TareaCliente.VisibleIndex = 0;
            TareaCliente.Width = 40;
            TareaCliente.Visible = true;
            TareaCliente.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(TareaCliente);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            //TareaID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //TareaID.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaID.AppearanceCell.Options.UseTextOptions = true;
            TareaID.AppearanceCell.Options.UseFont = true;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 60;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(TareaID);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this.unboundPrefix + "Descriptivo";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 2;
            TareaDesc.Width = 250;
            TareaDesc.Visible = true;
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(TareaDesc);

            GridColumn FechaSolicitud = new GridColumn();
            FechaSolicitud.FieldName = this.unboundPrefix + "FechaSolicitud";
            FechaSolicitud.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaSolicitud");
            FechaSolicitud.UnboundType = UnboundColumnType.DateTime;
            FechaSolicitud.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaSolicitud.AppearanceCell.Options.UseTextOptions = true;
            FechaSolicitud.VisibleIndex = 3;
            FechaSolicitud.Width = 80;
            FechaSolicitud.Visible = true;
            FechaSolicitud.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FechaSolicitud);

            GridColumn FechaFinTrabajo = new GridColumn();
            FechaFinTrabajo.FieldName = this.unboundPrefix + "FechaTrabajo";
            FechaFinTrabajo.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaTrabajo");
            FechaFinTrabajo.UnboundType = UnboundColumnType.DateTime;
            FechaFinTrabajo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFinTrabajo.AppearanceCell.Options.UseTextOptions = true;
            FechaFinTrabajo.VisibleIndex = 4;
            FechaFinTrabajo.Width = 80;
            FechaFinTrabajo.Visible = true;
            FechaFinTrabajo.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FechaFinTrabajo);

            GridColumn FechaFinTarea = new GridColumn();
            FechaFinTarea.FieldName = this.unboundPrefix + "FechaFin";
            FechaFinTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaEntrega");
            FechaFinTarea.UnboundType = UnboundColumnType.DateTime;
            FechaFinTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFinTarea.AppearanceCell.Options.UseTextOptions = true;
            FechaFinTarea.VisibleIndex = 4;
            FechaFinTarea.Width = 80;
            FechaFinTarea.Visible = true;
            FechaFinTarea.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(FechaFinTarea);

            GridColumn DiasAtrasoProyTar = new GridColumn();
            DiasAtrasoProyTar.FieldName = this.unboundPrefix + "DiasAtrasoProy";
            DiasAtrasoProyTar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoProy");
            DiasAtrasoProyTar.UnboundType = UnboundColumnType.Integer;
            DiasAtrasoProyTar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoProyTar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoProyTar.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoProyTar.AppearanceCell.Options.UseFont = true;
            DiasAtrasoProyTar.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoProyTar.VisibleIndex = 5;
            DiasAtrasoProyTar.Width = 80;
            DiasAtrasoProyTar.Visible = true;
            DiasAtrasoProyTar.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DiasAtrasoProyTar);

            GridColumn DiasAtrasoComprasTar = new GridColumn();
            DiasAtrasoComprasTar.FieldName = this.unboundPrefix + "DiasAtrasoCompras";
            DiasAtrasoComprasTar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoCompras");
            DiasAtrasoComprasTar.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoComprasTar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoComprasTar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoComprasTar.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoComprasTar.AppearanceCell.Options.UseFont = true;
            DiasAtrasoComprasTar.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoComprasTar.VisibleIndex = 6;
            DiasAtrasoComprasTar.Width = 80;
            DiasAtrasoComprasTar.Visible = true;
            DiasAtrasoComprasTar.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DiasAtrasoComprasTar);

            GridColumn DiasAtrasoEntregaTar = new GridColumn();
            DiasAtrasoEntregaTar.FieldName = this.unboundPrefix + "DiasAtrasoEntrega";
            DiasAtrasoEntregaTar.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DiasAtrasoEntrega");
            DiasAtrasoEntregaTar.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoEntregaTar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoEntregaTar.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoEntregaTar.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoEntregaTar.AppearanceCell.Options.UseFont = true;
            DiasAtrasoEntregaTar.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoEntregaTar.VisibleIndex = 7;
            DiasAtrasoEntregaTar.Width = 80;
            DiasAtrasoEntregaTar.Visible = true;
            DiasAtrasoEntregaTar.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(DiasAtrasoEntregaTar);

            GridColumn Editar = new GridColumn();
            Editar.FieldName = "Editar";
            Editar.UnboundType = UnboundColumnType.String;
            Editar.VisibleIndex = 8;
            Editar.Width = 60;
            Editar.Visible = true;
            Editar.OptionsColumn.ShowCaption = false;
            Editar.ColumnEdit = this.editBtnTarea;
            Editar.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Columns.Add(Editar);

            #endregion
            #region Grilla  Recursos  

            //inReferenciaID
            GridColumn codRef = new GridColumn();
            codRef.FieldName = this.unboundPrefix + "RecursoID";
            codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            codRef.UnboundType = UnboundColumnType.String;
            codRef.VisibleIndex = 2;
            codRef.Width = 60;
            codRef.Visible = true;
            codRef.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(codRef);

            //Descriptivo
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "RecursoDesc";
            desc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoDesc");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 3;
            desc.Width = 230;
            desc.Visible = true;
            desc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(desc);

            //RefProveedor
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
            RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 4;
            RefProveedor.Width = 70;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RefProveedor);

            //MarcaInvID
            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_MarcaDesc");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.VisibleIndex = 5;
            MarcaInvID.Width = 70;
            MarcaInvID.Visible = true;
            MarcaInvID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(MarcaInvID);

            //UnidadInvID
            GridColumn unidad = new GridColumn();
            unidad.FieldName = this.unboundPrefix + "UnidadInvID";
            unidad.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID"); 
            unidad.UnboundType = UnboundColumnType.String;
            unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            unidad.AppearanceCell.Options.UseTextOptions = true;
            unidad.VisibleIndex = 6;
            unidad.Width = 60;
            unidad.Visible = true;
            unidad.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(unidad);

            //Cantidad total sin Solicitar(Pendiente)
            GridColumn CantidadPend = new GridColumn();
            CantidadPend.FieldName = this.unboundPrefix + "CantidadPend";
            CantidadPend.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadPend");
            CantidadPend.UnboundType = UnboundColumnType.Decimal;
            CantidadPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //CantidadPend.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadPend.AppearanceCell.Options.UseTextOptions = true;
            CantidadPend.AppearanceCell.Options.UseFont = true;
            CantidadPend.VisibleIndex = 8;
            CantidadPend.Width = 85;
            CantidadPend.Visible = true;
            CantidadPend.ColumnEdit = this.editValue2Cant;
            CantidadPend.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadPend);

            //Cantidad total sin OC(Pendiente)
            GridColumn CantidadSUM = new GridColumn();
            CantidadSUM.FieldName = this.unboundPrefix + "CantidadSUM";
            CantidadSUM.Caption = this._bc.GetResource(LanguageTypes.Forms, "Cant Pend OC");
            CantidadSUM.UnboundType = UnboundColumnType.Decimal;
            CantidadSUM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadSUM.AppearanceCell.Options.UseTextOptions = true;
            CantidadSUM.AppearanceCell.Options.UseFont = true;
            CantidadSUM.VisibleIndex = 9;
            CantidadSUM.Width = 70;
            CantidadSUM.Visible = true;
            CantidadSUM.ColumnEdit = this.editValue2Cant;
            CantidadSUM.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadSUM);

            //Cantidad total sin Solicitar(Pendiente)
            GridColumn CantidadREC = new GridColumn();
            CantidadREC.FieldName = this.unboundPrefix + "CantidadREC";
            CantidadREC.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_CantidadREC");
            CantidadREC.UnboundType = UnboundColumnType.Decimal;
            CantidadREC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //CantidadPend.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadREC.AppearanceCell.Options.UseTextOptions = true;
            CantidadREC.AppearanceCell.Options.UseFont = true;
            CantidadREC.VisibleIndex = 10;
            CantidadREC.Width = 85;
            CantidadREC.Visible = true;
            CantidadREC.ColumnEdit = this.editValue2Cant;
            CantidadREC.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadREC);

            this.gvRecurso.OptionsView.ColumnAutoWidth = true;
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
                int diasTerminacionTrabajos = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_DiasTerminaTrabajoEntrega)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_DiasTerminaTrabajoEntrega)) : 0;
                int diasReqOC = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompra)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosOrdCompra)) : 0;

                this._listProyectos = this._bc.AdministrationModel.pyProyectoDocu_GetAllProyectos(this.dtFechaCorte.DateTime,true,false,false);

                foreach (DTO_QueryComiteTecnico proy in this._listProyectos)
                {

                    DTO_pyActaEntregaDeta det = new DTO_pyActaEntregaDeta();
                    det.NumDocProyecto.Value = proy.NumeroDoc.Value;
                    List<DTO_pyProyectoTareaCliente> entregables = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(proy.NumeroDoc.Value.Value, string.Empty, string.Empty);

                    foreach (DTO_QueryComiteTecnicoTareas tarea in proy.Detalle)
                    {
                        List<DTO_pyProyectoMvto> mvtosxTarea = proy.Movimientos.FindAll(x=>x.TareaID.Value == tarea.TareaID.Value && x.TareaCliente.Value == tarea.TareaCliente.Value);
                        tarea.FechaSolicitud.Value = tarea.FechaInicio.Value.HasValue ? tarea.FechaInicio.Value.Value.AddDays(diasReqOC * -1) : tarea.FechaSolicitud.Value;
                        tarea.FechaTrabajo.Value = tarea.FechaFin.Value.HasValue ? tarea.FechaFin.Value.Value.AddDays(diasTerminacionTrabajos * -1) : tarea.FechaTrabajo.Value;
                        tarea.DiasAtrasoProy = 0;
                        tarea.DiasAtrasoCompras = 0;
                        tarea.DiasAtrasoEntrega = 0;
                        
                        //Solicitados (Atraso Solicitud)
                        if (mvtosxTarea.Any(x=>x.CantidadPROV.Value != x.CantidadTOT.Value && x.CantidadTOT.Value > 0)) //Valida si esta pendiente
                        {
                            //Trae las solicitudes vencidas, las ordena y toma la mas vencida para validar los dias de atraso
                            List<DTO_pyProyectoMvto> mvtosxTareaVenc = mvtosxTarea.FindAll(x => x.CantidadPROV.Value != x.CantidadTOT.Value && x.CantidadTOT.Value > 0);
                            mvtosxTareaVenc = mvtosxTareaVenc.OrderBy(x => x.FechaOrdCompra.Value).ToList();
                            DateTime? fechaOrdCompraVenc = mvtosxTareaVenc.First().FechaOrdCompra.Value;
                            tarea.FechaSolicitud.Value = tarea.FechaInicio.Value.HasValue ? fechaOrdCompraVenc : tarea.FechaSolicitud.Value;

                            tarea.DiasAtrasoProy = tarea.FechaSolicitud.Value.HasValue? (int)(this.dtFechaCorte.DateTime - tarea.FechaSolicitud.Value.Value).TotalDays : 0;
                            tarea.DiasAtrasoProy = tarea.DiasAtrasoProy < 0 ? 0 : tarea.DiasAtrasoProy;
                            tarea.Detalle = mvtosxTareaVenc.FindAll(x => x.CantidadPROV.Value != x.CantidadTOT.Value && x.CantidadTOT.Value > 0);
                        }

                        //Recibidos(Atraso Trabajo)
                        if (mvtosxTarea.Any(x => x.CantidadREC.Value <= x.CantidadTOT.Value && x.CantidadTOT.Value > 0)) //Valida si esta pendiente
                        {
                            tarea.DiasAtrasoCompras = tarea.FechaTrabajo.Value.HasValue ? (int)(this.dtFechaCorte.DateTime - tarea.FechaTrabajo.Value.Value).TotalDays : 0;
                            tarea.DiasAtrasoCompras = tarea.DiasAtrasoCompras < 0 ? 0 : tarea.DiasAtrasoCompras;
                            tarea.Detalle = mvtosxTarea.FindAll(x => x.CantidadREC.Value != x.CantidadTOT.Value && x.CantidadTOT.Value > 0);
                            tarea.Detalle.ForEach(x => x.CantidadSUM.Value = (x.CantidadPROV.Value - x.CantidadREC.Value) >= 0? x.CantidadPROV.Value - x.CantidadREC.Value : 0);//Asigna cant Pend Ord Compra
                        }  
                        //Entregas(Atraso Entregas)
                        List<DTO_pyProyectoTareaCliente> entregablesxTarea = entregables.FindAll(x => x.DetalleTareas.Any(y=>y.TareaID.Value == tarea.TareaID.Value && y.TareaCliente.Value == tarea.TareaCliente.Value));
                        if (entregablesxTarea.Count > 0) //Valida si esta pendiente
                        {
                            decimal cantidadTarea = 0;
                            decimal cantidadFacturada = 0;
                            List<DTO_pyProyectoPlanEntrega> fechasEntrega = new List<DTO_pyProyectoPlanEntrega>();
                            List<DTO_pyActaEntregaDeta> actasxTarea = new List<DTO_pyActaEntregaDeta>();
                            foreach (DTO_pyProyectoTareaCliente entr in entregablesxTarea)
                            {
                                cantidadTarea += (entr.Cantidad.Value.HasValue?entr.Cantidad.Value.Value : 0);
                                fechasEntrega.AddRange(entr.Detalle);
                                entr.DetalleActas.FindAll(y=>y.PorEntregado.Value > 0).ForEach(x => x.Cantidad.Value = x.Cantidad.Value.HasValue? x.Cantidad.Value : 0);
                                actasxTarea.AddRange(entr.DetalleActas);
                                cantidadFacturada += (entr.DetalleActas.FindAll(y => y.NumDocFactura.Value.HasValue).Sum(x => x.Cantidad.Value.Value));
                            }
                            if(cantidadFacturada != cantidadTarea)
                            {
                                //Trae las entregas que tengan cantidad entregada
                                if (fechasEntrega.Exists(x=>x.Cantidad.Value > 0))
                                {
                                    //Trae las entregas, las ordena y toma la mas vencida de acuerdo a las actas de cada plan de entrega
                                    List<DTO_pyProyectoPlanEntrega> fechasCantidad = fechasEntrega.FindAll(x => x.Cantidad.Value > 0);
                                    fechasCantidad = fechasCantidad.OrderBy(x => x.FechaEntrega.Value).ToList();
                                    DTO_pyProyectoPlanEntrega fechaMasVencida = fechasCantidad.First();
                                    foreach (DTO_pyProyectoPlanEntrega d in fechasCantidad)//valida la proxima entrega pendiente 
                                    {
                                        var cantActas = actasxTarea.FindAll(x => x.ConsTareaEntrega.Value == d.Consecutivo.Value).Sum(y => y.CantEntregada.Value);
                                        if (cantActas != d.Cantidad.Value)
                                        {
                                            fechaMasVencida = d;
                                            break;
                                        }                                           
                                    }
                                    tarea.FechaFin.Value = fechaMasVencida.FechaEntrega.Value;
                                    tarea.DiasAtrasoEntrega = fechaMasVencida.FechaEntrega.Value.HasValue ? (int)(this.dtFechaCorte.DateTime - fechaMasVencida.FechaEntrega.Value.Value).TotalDays : 0;
                                    tarea.DiasAtrasoEntrega = tarea.DiasAtrasoEntrega < 0 ? 0 : tarea.DiasAtrasoEntrega;
                                }
                            }
                        }
                    }
                    proy.DiasAtrasoProy = proy.Detalle.Count > 0 ? proy.Detalle.Max(x => x.DiasAtrasoProy) : 0;
                    proy.DiasAtrasoCompras = proy.Detalle.Count > 0 ? proy.Detalle.Max(x => x.DiasAtrasoCompras) : 0;
                    proy.DiasAtrasoEntrega = proy.Detalle.Count > 0 ? proy.Detalle.Max(x => x.DiasAtrasoEntrega) : 0;
                    proy.Movimientos = new List<DTO_pyProyectoMvto>();
                }
                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico", "LoadData"));
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
                this.gcRecurso.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionTiempo", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información de cada compra
        /// </summary>
        private void LoadGridRecursos()
        {
            try
            {
                //this._rowTareaCurrent.Detalle = this._rowTareaCurrent.Detalle.FindAll(x => x.CantidadPROV.Value != x.CantidadTOT.Value);
                foreach (DTO_pyProyectoMvto rec in this._rowTareaCurrent.Detalle)
                    rec.CantidadPend.Value = rec.CantidadTOT.Value - rec.CantidadPROV.Value;

                this.gcRecurso.DataSource = null;
                this.gcRecurso.DataSource = this._rowTareaCurrent.Detalle;
                this.gcRecurso.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico", "LoadReferencias"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {

            //this.masterProyecto.Value = string.Empty;
            //this.masterPrefijo.Value = string.Empty; 
            //this.txtNro.Text = string.Empty;
            //this.masterCliente.Value = string.Empty;
            //this.txtLicitacion.Text = string.Empty;
            //this.txtDescripcion.Text =string.Empty;      

            //this._ctrlProyecto = null;
            //this._numeroDoc = 0;
            //this._proyectoDocu = new DTO_pyProyectoDocu();
            this._rowCurrent = new DTO_QueryComiteTecnico();
            this._rowTareaCurrent = new DTO_QueryComiteTecnicoTareas();
            this._listProyectos = new List<DTO_QueryComiteTecnico>();
            this.gcProyectos.DataSource = this._listProyectos;
            this.gcProyectos.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();

            //this.masterProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppQueries.QueryComiteTecnico;
            this.AddGridCols();

            this.dtFechaCorte.DateTime = DateTime.Now;
            this.LoadData();

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico", "Form_Enter"));
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

        #region Proyectos

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
                    this._rowCurrent = (DTO_QueryComiteTecnico)this.gvProyectos.GetRow(e.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "gvDocument_FocusedRowChanged"));
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

        /// <summary>
        /// Cambia estylo del campo dependiendo del valor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
           // string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            //if (fieldName == "DiasAtraso" && e.RowHandle >= 0)
            //{

            //    decimal cellvalue = Convert.ToDecimal(e.CellValue, CultureInfo.InvariantCulture);
            //    if (cellvalue > 0)
            //        e.Appearance.ForeColor = Color.Red;
            //    else
            //        e.Appearance.ForeColor = Color.Black;
            //}
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
                //DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvProyectos.GetRow(e.RowHandle);
                //if (currentRow != null)
                //{
                //    if (currentRow.DetalleInd.Value.Value)
                //        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //    else
                //        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcProyectos_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            try
            {
                this._gridDetalleCurrent = (GridView)e.View;
                if (this._gridDetalleCurrent != null && this._gridDetalleCurrent.FocusedRowHandle >= 0 && this._gridDetalleCurrent.DataRowCount > 0)
                {
                    var row = this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                    if (row.GetType() == typeof(DTO_QueryComiteTecnicoTareas))
                    {
                        this._rowTareaCurrent= (DTO_QueryComiteTecnicoTareas)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                        this.LoadGridRecursos();
                    }
                    else if (row.GetType() == typeof(DTO_QueryComiteTecnico))
                        this._rowCurrent = (DTO_QueryComiteTecnico)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gcProyectos_FocusedViewChanged"));
            }
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
                    this._rowTareaCurrent = (DTO_QueryComiteTecnicoTareas)this._gridDetalleCurrent.GetRow(e.FocusedRowHandle);
                    this.LoadGridRecursos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvDetalleSol_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Al cambiar el foco de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalle_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    this._gridDetalleCurrent = (GridView)sender;
                    this._rowTareaCurrent = (DTO_QueryComiteTecnicoTareas)this._gridDetalleCurrent.GetRow(e.RowHandle);
                    this.LoadGridRecursos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvOCPendientes_RowClick"));
            }
        }


        #region Recurso

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                //if (e.FocusedRowHandle >= 0)
                //    this._rowDetalle = (DTO_pyProyectoDeta)this.gvRecurso.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "gvRecurso_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "gvRecurso_CustomColumnDisplayText"));
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
                //List<DTO_glDocumentoControl> ctrlsAnexos = this._bc.AdministrationModel.pyProyectoMvto_GetDocsAnexo(this._rowDetalle.ConsecMvto.Value);
                //ModalViewDocuments viewDocs = new ModalViewDocuments(ctrlsAnexos,Convert.ToByte(this.rdGroupVer.SelectedIndex));
                //viewDocs.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "editLink_Click"));
            }
        }

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnTarea_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ModalTareasComite fact = new ModalTareasComite(this._rowCurrent,this._rowTareaCurrent);
                fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico.cs", "editBtnTarea_ButtonClick: " + ex.Message));
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
