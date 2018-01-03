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
    public partial class ConsultaComiteCompras : FormWithToolbar
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
        private int documentIDRecursos = AppQueries.QueryComiteTecnico;
        private ModulesPrefix frmModule;
        //Variables para importar
        private string unboundPrefix = "Unbound_";
        // Variables Formulario
        //Variables de datos
        private DTO_QueryComiteTecnico _rowCurrent = new DTO_QueryComiteTecnico();
        private GridView _gridDetalleCurrent = new GridView();
        private DTO_QueryComiteCompras _rowCompraCurrent = new DTO_QueryComiteCompras();
        private List<DTO_QueryComiteTecnico> _listProyectos = new List<DTO_QueryComiteTecnico>();
        private List<DTO_QueryComiteTecnico> _listProyectosNoPendientes = new List<DTO_QueryComiteTecnico>();
        private int _diasRequeridosParaOC = 0;
        private int _diasRequeridosParaRecibido = 0;
        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaComiteCompras()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "ConsultaComiteCompras"));
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
            ResponsableDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ResponsableDesc");
            ResponsableDesc.UnboundType = UnboundColumnType.String;
            ResponsableDesc.AppearanceCell.Options.UseTextOptions = true;
            ResponsableDesc.AppearanceCell.Options.UseFont = true;
            ResponsableDesc.VisibleIndex = 1;
            ResponsableDesc.Width = 90;
            ResponsableDesc.Visible = true;
            ResponsableDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ResponsableDesc);

            GridColumn ProyectoID = new GridColumn();
            ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
            ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProyectoID");
            ProyectoID.UnboundType = UnboundColumnType.String;
            ProyectoID.VisibleIndex = 2;
            ProyectoID.Width = 45;
            ProyectoID.Visible = true;
            ProyectoID.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoID);

            GridColumn ProyectoDesc = new GridColumn();
            ProyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
            ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProyectoDesc");
            ProyectoDesc.UnboundType = UnboundColumnType.String;
            ProyectoDesc.VisibleIndex = 3;
            ProyectoDesc.Width = 90;
            ProyectoDesc.Visible = true;
            ProyectoDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoDesc);

            GridColumn PrefDoc = new GridColumn();
            PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
            PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_PrefDoc");
            PrefDoc.UnboundType = UnboundColumnType.String;
            PrefDoc.VisibleIndex = 4;
            PrefDoc.Width = 35;
            PrefDoc.Visible = true;
            PrefDoc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(PrefDoc);

            GridColumn ClienteID = new GridColumn();
            ClienteID.FieldName = this.unboundPrefix + "ClienteID";
            ClienteID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ClienteID");
            ClienteID.UnboundType = UnboundColumnType.String;
            ClienteID.VisibleIndex = 5;
            ClienteID.Width = 50;
            ClienteID.Visible = true;
            ClienteID.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ClienteID);

            GridColumn ClienteDesc = new GridColumn();
            ClienteDesc.FieldName = this.unboundPrefix + "ClienteDesc";
            ClienteDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ClienteDesc");
            ClienteDesc.UnboundType = UnboundColumnType.String;
            ClienteDesc.VisibleIndex = 6;
            ClienteDesc.Width = 90;
            ClienteDesc.Visible = true;
            ClienteDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ClienteDesc);

            GridColumn FechaInicio = new GridColumn();
            FechaInicio.FieldName = this.unboundPrefix + "FechaInicio";
            FechaInicio.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaInicio");
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
            FechaFin.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaFin");
            FechaFin.UnboundType = UnboundColumnType.DateTime;
            FechaFin.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaFin.AppearanceCell.Options.UseTextOptions = true;
            FechaFin.VisibleIndex = 8;
            FechaFin.Width = 50;
            FechaFin.Visible = true;
            FechaFin.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(FechaFin);

            GridColumn DiasAtrasoOC = new GridColumn();
            DiasAtrasoOC.FieldName = this.unboundPrefix + "DiasAtrasoOC";
            DiasAtrasoOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoOC");
            DiasAtrasoOC.UnboundType = UnboundColumnType.Integer;
            DiasAtrasoOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoOC.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoOC.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoOC.AppearanceCell.Options.UseFont = true;
            DiasAtrasoOC.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoOC.VisibleIndex = 11;
            DiasAtrasoOC.Width = 50;
            DiasAtrasoOC.Visible = true;
            DiasAtrasoOC.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoOC);

            GridColumn DiasAtrasoRec = new GridColumn();
            DiasAtrasoRec.FieldName = this.unboundPrefix + "DiasAtrasoRec";
            DiasAtrasoRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoRec");
            DiasAtrasoRec.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoRec.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoRec.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoRec.AppearanceCell.Options.UseFont = true;
            DiasAtrasoRec.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoRec.VisibleIndex = 12;
            DiasAtrasoRec.Width = 50;
            DiasAtrasoRec.Visible = true;
            DiasAtrasoRec.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoRec);

            GridColumn DiasAtrasoFact = new GridColumn();
            DiasAtrasoFact.FieldName = this.unboundPrefix + "DiasAtrasoFact";
            DiasAtrasoFact.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoFact");
            DiasAtrasoFact.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoFact.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoFact.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoFact.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoFact.AppearanceCell.Options.UseFont = true;
            DiasAtrasoFact.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoFact.VisibleIndex = 13;
            DiasAtrasoFact.Width = 50;
            DiasAtrasoFact.Visible = true;
            DiasAtrasoFact.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DiasAtrasoFact);
            this.gvProyectos.OptionsView.ColumnAutoWidth = true;

            #endregion
            #region Grilla OC PEndientes(Solicitudes)
            GridColumn UsuarioResp = new GridColumn();
            UsuarioResp.FieldName = this.unboundPrefix + "UsuarioResp";
            UsuarioResp.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ResponsableDesc");
            UsuarioResp.UnboundType = UnboundColumnType.String;
            UsuarioResp.AppearanceCell.Options.UseTextOptions = true;
            UsuarioResp.AppearanceCell.Options.UseFont = true;
            UsuarioResp.VisibleIndex = 1;
            UsuarioResp.Width = 180;
            UsuarioResp.Visible = true;
            UsuarioResp.OptionsColumn.AllowEdit = false;
            this.gvOCPendientes.Columns.Add(UsuarioResp);

            GridColumn PrefDocCompra = new GridColumn();
            PrefDocCompra.FieldName = this.unboundPrefix + "PrefDocCompra";
            PrefDocCompra.Caption = "Solicitud";
            PrefDocCompra.UnboundType = UnboundColumnType.String;
            PrefDocCompra.VisibleIndex = 2;
            PrefDocCompra.Width = 78;
            PrefDocCompra.Visible = true;
            PrefDocCompra.ColumnEdit = this.editLink;
            this.gvOCPendientes.Columns.Add(PrefDocCompra);

            GridColumn FechaCreacion = new GridColumn();
            FechaCreacion.FieldName = this.unboundPrefix + "FechaCreacion";
            FechaCreacion.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaCreacion");
            FechaCreacion.UnboundType = UnboundColumnType.DateTime;
            FechaCreacion.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaCreacion.AppearanceCell.Options.UseTextOptions = true;
            FechaCreacion.VisibleIndex = 3;
            FechaCreacion.Width = 120;
            FechaCreacion.Visible = true;
            FechaCreacion.OptionsColumn.AllowEdit = false;
            this.gvOCPendientes.Columns.Add(FechaCreacion);
    
            GridColumn FechaEntregaOCPend = new GridColumn();
            FechaEntregaOCPend.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntregaOCPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaEntrega");
            FechaEntregaOCPend.UnboundType = UnboundColumnType.DateTime;
            FechaEntregaOCPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaEntregaOCPend.AppearanceCell.Options.UseTextOptions = true;
            FechaEntregaOCPend.VisibleIndex = 3;
            FechaEntregaOCPend.Width = 120;
            FechaEntregaOCPend.Visible = true;
            FechaEntregaOCPend.OptionsColumn.AllowEdit = false;
            this.gvOCPendientes.Columns.Add(FechaEntregaOCPend);

            GridColumn FechaAjustadaOCPend = new GridColumn();
            FechaAjustadaOCPend.FieldName = this.unboundPrefix + "FechaAjustada";
            FechaAjustadaOCPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaAjustada");
            FechaAjustadaOCPend.UnboundType = UnboundColumnType.DateTime;
            FechaAjustadaOCPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaAjustadaOCPend.AppearanceCell.Options.UseTextOptions = true;
            FechaAjustadaOCPend.VisibleIndex = 4;
            FechaAjustadaOCPend.Width = 120;
            FechaAjustadaOCPend.Visible = true;
            FechaAjustadaOCPend.OptionsColumn.AllowEdit = false;
            this.gvOCPendientes.Columns.Add(FechaAjustadaOCPend);


            GridColumn DiasAtrasoOCDet = new GridColumn();
            DiasAtrasoOCDet.FieldName = this.unboundPrefix + "DiasAtrasoOC";
            DiasAtrasoOCDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoOC");
            DiasAtrasoOCDet.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoOCDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoOCDet.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoOCDet.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoOCDet.AppearanceCell.Options.UseFont = true;
            DiasAtrasoOCDet.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoOCDet.VisibleIndex = 5;
            DiasAtrasoOCDet.Width = 95;
            DiasAtrasoOCDet.Visible = true;
            DiasAtrasoOCDet.OptionsColumn.AllowEdit = false;
            this.gvOCPendientes.Columns.Add(DiasAtrasoOCDet);

            GridColumn EditarSol = new GridColumn();
            EditarSol.FieldName = "Editar";
            EditarSol.UnboundType = UnboundColumnType.String;
            EditarSol.VisibleIndex = 6;
            EditarSol.Width = 60;
            EditarSol.Visible = true;
            EditarSol.OptionsColumn.ShowCaption = false;
            EditarSol.ColumnEdit = this.editBtnCompra;
            EditarSol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOCPendientes.Columns.Add(EditarSol);
            #endregion
            #region Grilla OC En Proceso(Ord Compra Sin Aprobar)
            GridColumn UsuarioRespOC = new GridColumn();
            UsuarioRespOC.FieldName = this.unboundPrefix + "UsuarioResp";
            UsuarioRespOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ResponsableDesc");
            UsuarioRespOC.UnboundType = UnboundColumnType.String;
            UsuarioRespOC.AppearanceCell.Options.UseTextOptions = true;
            UsuarioRespOC.AppearanceCell.Options.UseFont = true;
            UsuarioRespOC.VisibleIndex = 1;
            UsuarioRespOC.Width = 180;
            UsuarioRespOC.Visible = true;
            UsuarioRespOC.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(UsuarioRespOC);

            GridColumn PrefDocCompraOC = new GridColumn();
            PrefDocCompraOC.FieldName = this.unboundPrefix + "PrefDocCompra";
            PrefDocCompraOC.Caption = "Ord Compra";
            PrefDocCompraOC.UnboundType = UnboundColumnType.String;
            PrefDocCompraOC.VisibleIndex = 2;
            PrefDocCompraOC.Width = 78;
            PrefDocCompraOC.Visible = true;
            PrefDocCompraOC.OptionsColumn.ReadOnly = true;
            PrefDocCompraOC.ColumnEdit = this.editLink;
            this.gvOCEnProceso.Columns.Add(PrefDocCompraOC);

            GridColumn FechaCreacionOC = new GridColumn();
            FechaCreacionOC.FieldName = this.unboundPrefix + "FechaCreacion";
            FechaCreacionOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaCreacion");
            FechaCreacionOC.UnboundType = UnboundColumnType.DateTime;
            FechaCreacionOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaCreacionOC.AppearanceCell.Options.UseTextOptions = true;
            FechaCreacionOC.VisibleIndex = 3;
            FechaCreacionOC.Width = 90;
            FechaCreacionOC.Visible = true;
            FechaCreacionOC.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(FechaCreacionOC);

            GridColumn FechaEntregaOCEnProc = new GridColumn();
            FechaEntregaOCEnProc.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntregaOCEnProc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaEntrega");
            FechaEntregaOCEnProc.UnboundType = UnboundColumnType.DateTime;
            FechaEntregaOCEnProc.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaEntregaOCEnProc.AppearanceCell.Options.UseTextOptions = true;
            FechaEntregaOCEnProc.VisibleIndex = 3;
            FechaEntregaOCEnProc.Width = 120;
            FechaEntregaOCEnProc.Visible = true;
            FechaEntregaOCEnProc.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(FechaEntregaOCEnProc);

            GridColumn FechaAjustadaOCEnPro = new GridColumn();
            FechaAjustadaOCEnPro.FieldName = this.unboundPrefix + "FechaAjustada";
            FechaAjustadaOCEnPro.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaAjustada");
            FechaAjustadaOCEnPro.UnboundType = UnboundColumnType.DateTime;
            FechaAjustadaOCEnPro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaAjustadaOCEnPro.AppearanceCell.Options.UseTextOptions = true;
            FechaAjustadaOCEnPro.VisibleIndex = 4;
            FechaAjustadaOCEnPro.Width = 90;
            FechaAjustadaOCEnPro.Visible = true;
            FechaAjustadaOCEnPro.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(FechaAjustadaOCEnPro);


            GridColumn EstadoOC = new GridColumn();
            EstadoOC.FieldName = this.unboundPrefix + "Estado";
            EstadoOC.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_Estado");
            EstadoOC.UnboundType = UnboundColumnType.String;
            EstadoOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            EstadoOC.AppearanceCell.Options.UseTextOptions = true;
            EstadoOC.VisibleIndex = 5;
            EstadoOC.Width = 70;
            EstadoOC.Visible = true;
            EstadoOC.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(EstadoOC);

            GridColumn ProveedorID = new GridColumn();
            ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
            ProveedorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorID");
            ProveedorID.UnboundType = UnboundColumnType.String;
            ProveedorID.VisibleIndex = 6;
            ProveedorID.Width = 80;
            ProveedorID.Visible = true;
            ProveedorID.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(ProveedorID);

            GridColumn ProveedorDesc = new GridColumn();
            ProveedorDesc.FieldName = this.unboundPrefix + "ProveedorDesc";
            ProveedorDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorDesc");
            ProveedorDesc.UnboundType = UnboundColumnType.String;
            ProveedorDesc.VisibleIndex =7;
            ProveedorDesc.Width = 140;
            ProveedorDesc.Visible = true;
            ProveedorDesc.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(ProveedorDesc);

            GridColumn DiasAtrasoRecDet = new GridColumn();
            DiasAtrasoRecDet.FieldName = this.unboundPrefix + "DiasAtrasoRec";
            DiasAtrasoRecDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoRec");
            DiasAtrasoRecDet.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoRecDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoRecDet.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoRecDet.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoRecDet.AppearanceCell.Options.UseFont = true;
            DiasAtrasoRecDet.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoRecDet.VisibleIndex = 8;
            DiasAtrasoRecDet.Width = 95;
            DiasAtrasoRecDet.Visible = true;
            DiasAtrasoRecDet.OptionsColumn.AllowEdit = false;
            this.gvOCEnProceso.Columns.Add(DiasAtrasoRecDet);

            GridColumn EditarOC = new GridColumn();
            EditarOC.FieldName = "Editar";
            //EditarOC.UnboundType = UnboundColumnType.String;
            EditarOC.VisibleIndex = 9;
            EditarOC.Width = 60;
            EditarOC.Visible = true;
            EditarOC.OptionsColumn.ShowCaption = false;
            EditarOC.ColumnEdit = this.editBtnCompra;
            EditarOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOCEnProceso.Columns.Add(EditarOC);
            #endregion
            #region Grilla Recibidos Pendientes(Ord Compra)
            GridColumn UsuarioRespRecPEnd = new GridColumn();
            UsuarioRespRecPEnd.FieldName = this.unboundPrefix + "UsuarioResp";
            UsuarioRespRecPEnd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ResponsableDesc");
            UsuarioRespRecPEnd.UnboundType = UnboundColumnType.String;
            UsuarioRespRecPEnd.AppearanceCell.Options.UseTextOptions = true;
            UsuarioRespRecPEnd.AppearanceCell.Options.UseFont = true;
            UsuarioRespRecPEnd.VisibleIndex = 1;
            UsuarioRespRecPEnd.Width = 180;
            UsuarioRespRecPEnd.Visible = true;
            UsuarioRespRecPEnd.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(UsuarioRespOC);

            GridColumn PrefDocCompraRecPend = new GridColumn();
            PrefDocCompraRecPend.FieldName = this.unboundPrefix + "PrefDocCompra";
            PrefDocCompraRecPend.Caption = "Ord Compra";
            PrefDocCompraRecPend.UnboundType = UnboundColumnType.String;
            PrefDocCompraRecPend.VisibleIndex = 2;
            PrefDocCompraRecPend.Width = 78;
            PrefDocCompraRecPend.Visible = true;
            PrefDocCompraOC.OptionsColumn.ReadOnly = true;
            PrefDocCompraRecPend.ColumnEdit = this.editLink;
            this.gvRecPendientes.Columns.Add(PrefDocCompraRecPend);

            GridColumn FechaCreacionRecPend = new GridColumn();
            FechaCreacionRecPend.FieldName = this.unboundPrefix + "FechaCreacion";
            FechaCreacionRecPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaCreacion");
            FechaCreacionRecPend.UnboundType = UnboundColumnType.DateTime;
            FechaCreacionRecPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaCreacionRecPend.AppearanceCell.Options.UseTextOptions = true;
            FechaCreacionRecPend.VisibleIndex = 3;
            FechaCreacionRecPend.Width = 90;
            FechaCreacionRecPend.Visible = true;
            FechaCreacionRecPend.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(FechaCreacionRecPend);

            GridColumn FechaEntregaRecPend = new GridColumn();
            FechaEntregaRecPend.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntregaRecPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaEntrega");
            FechaEntregaRecPend.UnboundType = UnboundColumnType.DateTime;
            FechaEntregaRecPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaEntregaRecPend.AppearanceCell.Options.UseTextOptions = true;
            FechaEntregaRecPend.VisibleIndex = 3;
            FechaEntregaRecPend.Width = 120;
            FechaEntregaRecPend.Visible = true;
            FechaEntregaRecPend.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(FechaEntregaRecPend);

            GridColumn FechaAjustadaRecPend = new GridColumn();
            FechaAjustadaRecPend.FieldName = this.unboundPrefix + "FechaAjustada";
            FechaAjustadaRecPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaAjustada");
            FechaAjustadaRecPend.UnboundType = UnboundColumnType.DateTime;
            FechaAjustadaRecPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaAjustadaRecPend.AppearanceCell.Options.UseTextOptions = true;
            FechaAjustadaRecPend.VisibleIndex = 4;
            FechaAjustadaRecPend.Width = 90;
            FechaAjustadaRecPend.Visible = true;
            FechaAjustadaRecPend.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(FechaAjustadaRecPend);

            GridColumn ProveedorIDRecPend = new GridColumn();
            ProveedorIDRecPend.FieldName = this.unboundPrefix + "ProveedorID";
            ProveedorIDRecPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorID");
            ProveedorIDRecPend.UnboundType = UnboundColumnType.String;
            ProveedorIDRecPend.VisibleIndex = 5;
            ProveedorIDRecPend.Width = 80;
            ProveedorIDRecPend.Visible = true;
            ProveedorIDRecPend.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(ProveedorIDRecPend);

            GridColumn ProveedorDescRecPend = new GridColumn();
            ProveedorDescRecPend.FieldName = this.unboundPrefix + "ProveedorDesc";
            ProveedorDescRecPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorDesc");
            ProveedorDescRecPend.UnboundType = UnboundColumnType.String;
            ProveedorDescRecPend.VisibleIndex = 6;
            ProveedorDescRecPend.Width = 170;
            ProveedorDescRecPend.Visible = true;
            ProveedorDescRecPend.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(ProveedorDescRecPend);

            GridColumn DiasAtrasoRecPEnd = new GridColumn();
            DiasAtrasoRecPEnd.FieldName = this.unboundPrefix + "DiasAtrasoRec";
            DiasAtrasoRecPEnd.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoRec");
            DiasAtrasoRecPEnd.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoRecPEnd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoRecPEnd.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoRecPEnd.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoRecPEnd.AppearanceCell.Options.UseFont = true;
            DiasAtrasoRecPEnd.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoRecPEnd.VisibleIndex = 7;
            DiasAtrasoRecPEnd.Width = 95;
            DiasAtrasoRecPEnd.Visible = true;
            DiasAtrasoRecPEnd.OptionsColumn.AllowEdit = false;
            this.gvRecPendientes.Columns.Add(DiasAtrasoRecPEnd);

            GridColumn EditarRecPend = new GridColumn();
            EditarRecPend.FieldName = "Editar";
            //EditarOC.UnboundType = UnboundColumnType.String;
            EditarRecPend.VisibleIndex = 8;
            EditarRecPend.Width = 60;
            EditarRecPend.Visible = true;
            EditarRecPend.OptionsColumn.ShowCaption = false;
            EditarRecPend.ColumnEdit = this.editBtnCompra;
            EditarRecPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecPendientes.Columns.Add(EditarRecPend);
            #endregion
            #region Grilla Fact PEndientes(Recibidos)
            GridColumn UsuarioRespRec = new GridColumn();
            UsuarioRespRec.FieldName = this.unboundPrefix + "UsuarioResp";
            UsuarioRespRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ResponsableDesc");
            UsuarioRespRec.UnboundType = UnboundColumnType.String;
            UsuarioRespRec.AppearanceCell.Options.UseTextOptions = true;
            UsuarioRespRec.AppearanceCell.Options.UseFont = true;
            UsuarioRespRec.VisibleIndex = 1;
            UsuarioRespRec.Width = 140;
            UsuarioRespRec.Visible = true;
            UsuarioRespRec.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(UsuarioRespRec);

            GridColumn PrefDocCompraRec = new GridColumn();
            PrefDocCompraRec.FieldName = this.unboundPrefix + "PrefDocCompra";
            PrefDocCompraRec.Caption = "Recibido";
            PrefDocCompraRec.UnboundType = UnboundColumnType.String;
            PrefDocCompraRec.VisibleIndex = 2;
            PrefDocCompraRec.Width = 62;
            PrefDocCompraRec.Visible = true;
            PrefDocCompraOC.OptionsColumn.ReadOnly = true;
            PrefDocCompraRec.ColumnEdit = this.editLink;
            this.gvFactPendientes.Columns.Add(PrefDocCompraRec);

            GridColumn FechaCreacionRec = new GridColumn();
            FechaCreacionRec.FieldName = this.unboundPrefix + "FechaCreacion";
            FechaCreacionRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaCreacion");
            FechaCreacionRec.UnboundType = UnboundColumnType.DateTime;
            FechaCreacionRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaCreacionRec.AppearanceCell.Options.UseTextOptions = true;
            FechaCreacionRec.VisibleIndex = 3;
            FechaCreacionRec.Width = 90;
            FechaCreacionRec.Visible = true;
            FechaCreacionRec.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(FechaCreacionRec);
   
            GridColumn FechaEntregaFactPend = new GridColumn();
            FechaEntregaFactPend.FieldName = this.unboundPrefix + "FechaEntrega";
            FechaEntregaFactPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaEntrega");
            FechaEntregaFactPend.UnboundType = UnboundColumnType.DateTime;
            FechaEntregaFactPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaEntregaFactPend.AppearanceCell.Options.UseTextOptions = true;
            FechaEntregaFactPend.VisibleIndex = 3;
            FechaEntregaFactPend.Width = 120;
            FechaEntregaFactPend.Visible = true;
            FechaEntregaFactPend.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(FechaEntregaFactPend);

            GridColumn FechaAjustadaFactPend = new GridColumn();
            FechaAjustadaFactPend.FieldName = this.unboundPrefix + "FechaAjustada";
            FechaAjustadaFactPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_FechaAjustada");
            FechaAjustadaFactPend.UnboundType = UnboundColumnType.DateTime;
            FechaAjustadaFactPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            FechaAjustadaFactPend.AppearanceCell.Options.UseTextOptions = true;
            FechaAjustadaFactPend.VisibleIndex = 4;
            FechaAjustadaFactPend.Width = 90;
            FechaAjustadaFactPend.Visible = true;
            FechaAjustadaFactPend.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(FechaAjustadaFactPend);

            GridColumn EstadoRec = new GridColumn();
            EstadoRec.FieldName = this.unboundPrefix + "Estado";
            EstadoRec.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_Estado");
            EstadoRec.UnboundType = UnboundColumnType.String;
            EstadoRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            EstadoRec.AppearanceCell.Options.UseTextOptions = true;
            EstadoRec.VisibleIndex = 5;
            EstadoRec.Width = 70;
            EstadoRec.Visible = true;
            EstadoRec.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(EstadoRec);

            GridColumn ProveedorIDFactPend = new GridColumn();
            ProveedorIDFactPend.FieldName = this.unboundPrefix + "ProveedorID";
            ProveedorIDFactPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorID");
            ProveedorIDFactPend.UnboundType = UnboundColumnType.String;
            ProveedorIDFactPend.VisibleIndex = 6;
            ProveedorIDFactPend.Width = 65;
            ProveedorIDFactPend.Visible = true;
            ProveedorIDFactPend.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(ProveedorIDFactPend);

            GridColumn ProveedorDescFactPend = new GridColumn();
            ProveedorDescFactPend.FieldName = this.unboundPrefix + "ProveedorDesc";
            ProveedorDescFactPend.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_ProveedorDesc");
            ProveedorDescFactPend.UnboundType = UnboundColumnType.String;
            ProveedorDescFactPend.VisibleIndex =7;
            ProveedorDescFactPend.Width = 105;
            ProveedorDescFactPend.Visible = true;
            ProveedorDescFactPend.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(ProveedorDescFactPend);

            GridColumn DiasAtrasoFactDet = new GridColumn();
            DiasAtrasoFactDet.FieldName = this.unboundPrefix + "DiasAtrasoFact";
            DiasAtrasoFactDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_DiasAtrasoFact");
            DiasAtrasoFactDet.UnboundType = UnboundColumnType.Decimal;
            DiasAtrasoFactDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            DiasAtrasoFactDet.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DiasAtrasoFactDet.AppearanceCell.Options.UseTextOptions = true;
            DiasAtrasoFactDet.AppearanceCell.Options.UseFont = true;
            DiasAtrasoFactDet.AppearanceCell.Options.UseForeColor = true;
            DiasAtrasoFactDet.VisibleIndex = 8;
            DiasAtrasoFactDet.Width = 95;
            DiasAtrasoFactDet.Visible = true;
            DiasAtrasoFactDet.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(DiasAtrasoFactDet);

            GridColumn BodegaDesc = new GridColumn();
            BodegaDesc.FieldName = this.unboundPrefix + "BodegaDesc";
            BodegaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentIDRecursos + "_BodegaDesc");
            BodegaDesc.UnboundType = UnboundColumnType.String;
            BodegaDesc.VisibleIndex = 9;
            BodegaDesc.Width = 100;
            BodegaDesc.Visible = true;
            BodegaDesc.OptionsColumn.AllowEdit = false;
            this.gvFactPendientes.Columns.Add(BodegaDesc);

            GridColumn EditarRec = new GridColumn();
            EditarRec.FieldName = "Editar";
            //EditarRec.UnboundType = UnboundColumnType.String;
            EditarRec.VisibleIndex = 10;
            EditarRec.Width = 60;
            EditarRec.Visible = true;
            EditarRec.OptionsColumn.ShowCaption = false;
            EditarRec.ColumnEdit = this.editBtnCompra;
            EditarRec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvFactPendientes.Columns.Add(EditarRec);
            #endregion
            #region Grilla  Recursos  
            //Tarea
            GridColumn tareaCliente = new GridColumn();
            tareaCliente.FieldName = this.unboundPrefix + "DatoAdd4";
            tareaCliente.Caption = this._bc.GetResource(LanguageTypes.Forms, "Ítem Proy");
            tareaCliente.UnboundType = UnboundColumnType.String;
            tareaCliente.VisibleIndex = 0;
            tareaCliente.Width = 40;
            tareaCliente.Visible = true;
            tareaCliente.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(tareaCliente);

            //CodigoServicios
            GridColumn codBS = new GridColumn();
            codBS.FieldName = this.unboundPrefix + "CodigoBSID";
            codBS.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_CodigoBSID");
            codBS.UnboundType = UnboundColumnType.String;
            codBS.VisibleIndex = 1;
            codBS.Width = 60;
            codBS.Visible = true;
            codBS.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(codBS);

            //inReferenciaID
            GridColumn codRef = new GridColumn();
            codRef.FieldName = this.unboundPrefix + "inReferenciaID";
            codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_inReferenciaID");
            codRef.UnboundType = UnboundColumnType.String;
            codRef.VisibleIndex = 2;
            codRef.Width = 60;
            codRef.Visible = true;
            codRef.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(codRef);

            //Descriptivo
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "Descriptivo";
            desc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_Descriptivo");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 3;
            desc.Width = 230;
            desc.Visible = true;
            desc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(desc);

            //RefProveedor
            GridColumn RefProveedor = new GridColumn();
            RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
            RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_RefProveedor");
            RefProveedor.UnboundType = UnboundColumnType.String;
            RefProveedor.VisibleIndex = 4;
            RefProveedor.Width = 60;
            RefProveedor.Visible = true;
            RefProveedor.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RefProveedor);

            //MarcaInvID
            GridColumn MarcaInvID = new GridColumn();
            MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
            MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_MarcaInvID");
            MarcaInvID.UnboundType = UnboundColumnType.String;
            MarcaInvID.VisibleIndex = 5;
            MarcaInvID.Width = 50;
            MarcaInvID.Visible = true;
            MarcaInvID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(MarcaInvID);

            //UnidadInvID
            GridColumn unidad = new GridColumn();
            unidad.FieldName = this.unboundPrefix + "UnidadInvID";
            unidad.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_UnidadInvID"); ;
            unidad.UnboundType = UnboundColumnType.String;
            unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            unidad.AppearanceCell.Options.UseTextOptions = true;
            unidad.VisibleIndex = 6;
            unidad.Width = 40;
            unidad.Visible = true;
            unidad.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(unidad);

            //Cantidad total sin Solicitar(Pendiente)
            GridColumn CantidadPend = new GridColumn();
            CantidadPend.FieldName = this.unboundPrefix + "CantidadPend";
            CantidadPend.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_CantidadPend");
            CantidadPend.UnboundType = UnboundColumnType.Decimal;
            CantidadPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //CantidadPend.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CantidadPend.AppearanceCell.Options.UseTextOptions = true;
            CantidadPend.AppearanceCell.Options.UseFont = true;
            CantidadPend.VisibleIndex = 8;
            CantidadPend.Width = 50;
            CantidadPend.Visible = true;
            CantidadPend.ColumnEdit = this.editValue2Cant;
            CantidadPend.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadPend);      

            //ValorUni
            GridColumn valorUni = new GridColumn();
            valorUni.FieldName = this.unboundPrefix + "ValorUni";
            valorUni.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_ValorUni");
            valorUni.UnboundType = UnboundColumnType.Decimal;
            valorUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            valorUni.AppearanceCell.Options.UseTextOptions = true;
            valorUni.VisibleIndex = 11;
            valorUni.Width = 70;
            valorUni.Visible = false;
            valorUni.ColumnEdit = this.editValue2;
            valorUni.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(valorUni);

            //ValorTotML
            GridColumn valorTotML = new GridColumn();
            valorTotML.FieldName = this.unboundPrefix + "ValorTotML";
            valorTotML.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.CierreDetalleSolicitud + "_ValorTotML");
            valorTotML.UnboundType = UnboundColumnType.Decimal;
            valorTotML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            valorTotML.AppearanceCell.Options.UseTextOptions = true;
            valorTotML.VisibleIndex = 12;
            valorTotML.Width = 80;
            valorTotML.Visible = false;
            valorTotML.ColumnEdit = this.editValue2;
            valorTotML.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(valorTotML);
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
                this._listProyectos = this._bc.AdministrationModel.pyProyectoDocu_GetAllProyectos(this.dtFechaCorte.DateTime,false,true,false);
                List<DTO_prSolicitudResumen> solPendientes = this._bc.AdministrationModel.Solicitud_GetResumen(AppDocuments.OrdenCompra, this._bc.AdministrationModel.User, ModulesPrefix.pr, TipoMoneda.Local);
                List<DTO_prOrdenCompraResumen> ordCompraPend = this._bc.AdministrationModel.OrdenCompra_GetResumen(AppDocuments.Recibido, null, ModulesPrefix.pr, null);
                List<DTO_prRecibidoAprob> recPend = this._bc.AdministrationModel.Recibido_GetRecibidoNoFacturado(AppDocuments.Recibido, string.Empty,string.Empty,false);

                foreach (DTO_QueryComiteTecnico proy in this._listProyectos)
                {
                    foreach (DTO_QueryComiteCompras compra in proy.OrdenCompraPendientes)
                    {
                        DayOfWeek day = compra.FechaCreacion.Value.Value.DayOfWeek;
                        DateTime fechaLimite = !compra.FechaAjustada.Value.HasValue? compra.FechaCreacion.Value.Value.AddDays(this._diasRequeridosParaOC) : compra.FechaAjustada.Value.Value.Date;
                        compra.FechaEntrega.Value = fechaLimite;
                        if (solPendientes.Exists(x => x.NumeroDoc.Value == compra.NumDocCompra.Value))
                        {
                            compra.DiasAtrasoOC = compra.FechaCreacion.Value.Value.Date <= fechaLimite.Date ? (int)(this.dtFechaCorte.DateTime.Date - fechaLimite.Date).TotalDays : 0;
                            compra.DiasAtrasoOC = compra.DiasAtrasoOC >= 0 ? compra.DiasAtrasoOC : 0; //Si es negativo asigna 0 indicando que NO esta pendiente
                        }
                        else
                            compra.DiasAtrasoOC = 0;
                    }
                    foreach (DTO_QueryComiteCompras compra in proy.OrdenCompraEnProceso)
                    {
                        DayOfWeek day = compra.FechaCreacion.Value.Value.DayOfWeek;
                        DateTime fechaLimite = compra.FechaEntrega.Value.Value;
                        if (solPendientes.Exists(x => x.NumeroDoc.Value == compra.NumDocCompra.Value))
                        {
                            compra.DiasAtrasoOCEnProceso = compra.FechaCreacion.Value.Value.Date <= fechaLimite.Date? (int)(this.dtFechaCorte.DateTime - fechaLimite).TotalDays : 0;
                            compra.DiasAtrasoOCEnProceso = compra.DiasAtrasoOCEnProceso >= 0 ? compra.DiasAtrasoOCEnProceso : 0; //Si es negativo asigna 0 indicando que NO esta pendiente
                        }
                        else
                            compra.DiasAtrasoOCEnProceso = 0;
                    }
                    foreach (DTO_QueryComiteCompras compra in proy.RecibidoPendientes)
                    {
                        DayOfWeek day = compra.FechaCreacion.Value.Value.DayOfWeek;
                        DateTime fechaLimite = compra.FechaEntrega.Value.Value;
                        if (ordCompraPend.Exists(x => x.NumeroDoc.Value == compra.NumDocCompra.Value))
                        {
                            compra.DiasAtrasoRec = compra.FechaCreacion.Value.Value.Date <= fechaLimite.Date ? (int)(this.dtFechaCorte.DateTime - fechaLimite).TotalDays : 0;
                            compra.DiasAtrasoRec = compra.DiasAtrasoRec >= 0 ? compra.DiasAtrasoRec : 0; //Si es negativo asigna 0 indicando que NO esta pendiente
                        }
                        else
                            compra.DiasAtrasoRec = 0;
                    }
                    foreach (DTO_QueryComiteCompras compra in proy.FacturaPendientes)
                    {
                        DayOfWeek day = compra.FechaCreacion.Value.Value.DayOfWeek;
                        DateTime fechaLimite = !compra.FechaAjustada.Value.HasValue ? compra.FechaCreacion.Value.Value.AddDays(this._diasRequeridosParaRecibido) : compra.FechaAjustada.Value.Value;
                        compra.FechaEntrega.Value = fechaLimite;
                        //var doc = this._bc.AdministrationModel.glDocumentoControl_GetByID(compra.NumDocCompra.Value.Value);
                        if (recPend.Exists(x => x.NumeroDoc.Value == compra.NumDocCompra.Value))
                        {
                            compra.DiasAtrasoFact = compra.FechaCreacion.Value.Value.Date <= fechaLimite.Date? (int)(this.dtFechaCorte.DateTime - fechaLimite).TotalDays : 0;
                            compra.DiasAtrasoFact = compra.DiasAtrasoFact >= 0 ? compra.DiasAtrasoFact : 0; //Si es negativo asigna 0 indicando que NO esta pendiente
                        }
                        else
                            compra.DiasAtrasoFact = 0;
                    }
                    proy.DiasAtrasoOC = proy.OrdenCompraPendientes.Count > 0? proy.OrdenCompraPendientes.Max(x=>x.DiasAtrasoOC) : 0;
                    proy.DiasAtrasoOCEnProceso = proy.OrdenCompraEnProceso.Count > 0? proy.OrdenCompraEnProceso.Max(x => x.DiasAtrasoOCEnProceso) : 0;
                    proy.DiasAtrasoRec = proy.RecibidoPendientes.Count > 0? proy.RecibidoPendientes.Max(x => x.DiasAtrasoRec) : 0;
                    proy.DiasAtrasoFact = proy.FacturaPendientes.Count > 0 ? proy.FacturaPendientes.Max(x => x.DiasAtrasoFact) : 0;
                }
                this._listProyectosNoPendientes = this._listProyectos.FindAll(x => x.DiasAtrasoOC == 0 && x.DiasAtrasoOCEnProceso == 0 && x.DiasAtrasoRec == 0 && x.DiasAtrasoFact == 0);
                this._listProyectos.RemoveAll(x => x.DiasAtrasoOC == 0 && x.DiasAtrasoOCEnProceso == 0 && x.DiasAtrasoRec == 0 && x.DiasAtrasoFact == 0);

                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras", "LoadData"));
            }
        }

        /// <summary>
        /// Calcula los dias laborales
        /// </summary>
        /// <param name="add"></param>
        /// <param name="FechaInicial"></param>
        /// <returns></returns>
        protected DateTime DateAgregarLaborales(Int32 add, DateTime FechaInicial)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday) { FechaInicial = FechaInicial.AddDays(2); }
            if (FechaInicial.DayOfWeek == DayOfWeek.Sunday) { FechaInicial = FechaInicial.AddDays(1); }
            Int32 weeks = add / 5;
            add += weeks * 2;
            if (FechaInicial.DayOfWeek > FechaInicial.AddDays(add).DayOfWeek) { add += 2; }
            if (FechaInicial.AddDays(add).DayOfWeek == DayOfWeek.Saturday) { add += 2; }
            // Int32 libres = LibresEntre(FechaInicial, FechaInicial.AddDays(add));

            //if (libres > 0) { return DateAgregarLaborales(0, FechaInicial.AddDays(libres + add)); }
            ///else
            { return FechaInicial.AddDays(add); }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteTecnico", "LoadData"));
            }
        }

        /// <summary>
        /// Carga la información de cada compra
        /// </summary>
        private void LoadReferencias(int numeroDocCompra)
        {
            try
            {
                if (this._rowCompraCurrent.Detalle.Count == 0)
                {
                    DTO_glDocumentoControl ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(numeroDocCompra);
                    this._rowCompraCurrent.Detalle = this._bc.AdministrationModel.prDetalleDocu_GetPendienteForCierre(ctrl.DocumentoID.Value.Value, ctrl.PrefijoID.Value, ctrl.DocumentoNro.Value, string.Empty, string.Empty, string.Empty);
                    this._rowCompraCurrent.Detalle = this._rowCompraCurrent.Detalle.FindAll(x => x.Documento4ID.Value == _rowCurrent.NumeroDoc.Value);
                }
                this.gcRecurso.DataSource = null;
                this.gcRecurso.DataSource = this._rowCompraCurrent.Detalle;
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
            this._rowCompraCurrent = new DTO_QueryComiteCompras();
            this._listProyectos = new List<DTO_QueryComiteTecnico>();
            this._listProyectosNoPendientes = new List<DTO_QueryComiteTecnico>();
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
            this.documentID = AppQueries.QueryComiteCompras;
            this.AddGridCols();

            this._diasRequeridosParaOC = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosParaOC)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosParaOC)) : 0;
            this._diasRequeridosParaRecibido = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosParaRec)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasRequeridosParaRec)) : 0;

            this.dtFechaCorte.DateTime = DateTime.Now;
            this.LoadData();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras", "Form_FormClosed"));
            }
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

        }

        /// <summary>
        /// Al cambiar de valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkNoPendientes_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.chkNoPendientes.Checked)
                {
                    this._listProyectosNoPendientes = this._listProyectos.FindAll(x => x.DiasAtrasoOC == 0 && x.DiasAtrasoOCEnProceso == 0 && x.DiasAtrasoRec == 0 && x.DiasAtrasoFact == 0);
                    this._listProyectos.RemoveAll(x => x.DiasAtrasoOC == 0 && x.DiasAtrasoOCEnProceso == 0 && x.DiasAtrasoRec == 0 && x.DiasAtrasoFact == 0);
                }
                else
                    this._listProyectos.AddRange(this._listProyectosNoPendientes);

                this._listProyectos = this._listProyectos.OrderBy(x => x.NumeroDoc.Value).ToList();
                this.LoadGrids();
            }
            catch (Exception ex)
            {
                throw;
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
                    this._rowCurrent = (DTO_QueryComiteTecnico)this.gvProyectos.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvDocument_FocusedRowChanged"));
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
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

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
                    if (row.GetType() == typeof(DTO_QueryComiteCompras))
                    {
                        this._rowCompraCurrent = (DTO_QueryComiteCompras)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                        this.LoadReferencias(this._rowCompraCurrent.NumDocCompra.Value.Value);
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
        private void gvDetalleSol_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._gridDetalleCurrent = (GridView)sender;
                    this._rowCompraCurrent = (DTO_QueryComiteCompras)this._gridDetalleCurrent.GetRow(e.FocusedRowHandle);
                    this.LoadReferencias(this._rowCompraCurrent.NumDocCompra.Value.Value);
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
        private void gvOCPendientes_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    this._gridDetalleCurrent = (GridView)sender;
                    this._rowCompraCurrent = (DTO_QueryComiteCompras)this._gridDetalleCurrent.GetRow(e.RowHandle);
                    this.LoadReferencias(this._rowCompraCurrent.NumDocCompra.Value.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvOCPendientes_RowClick"));
            }
        }

        /// <summary>
        /// Permite cambiar el texto del campo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDetalleSol_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == this.unboundPrefix + "Estado")
            {
                if (e.Value.ToString().Equals("0"))
                    e.DisplayText = "Anulado";
                else if (e.Value.ToString().Equals("1"))
                    e.DisplayText = "Sin Aprobar";
                else if (e.Value.ToString().Equals("2"))
                    e.DisplayText = "Para Aprobación";
                else if (e.Value.ToString().Equals("3"))
                    e.DisplayText = "Aprobado";
                else if (e.Value.ToString().Equals("4"))
                    e.DisplayText = "Revertido";
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvRecurso_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        #endregion        

        /// <summary>
        /// Abre la modal para detalle
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtnCompra_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                ModalModificaFechas fact = new ModalModificaFechas(this._rowCurrent, this._rowCompraCurrent);
                fact.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaComiteCompras.cs", "editBtnCompra_ButtonClick: " + ex.Message));
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

        /// <summary>
        /// Al dar clic en los campos de doc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                GridView gv = (GridView)this.gcProyectos.FocusedView;
                if (gv.FocusedRowHandle >= 0)
                {
                    DTO_QueryComiteCompras row = (DTO_QueryComiteCompras)gv.GetRow(gv.FocusedRowHandle);

                    DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                    DTO_Comprobante comprobante = new DTO_Comprobante();

                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(row.NumDocCompra.Value.Value);
                    comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                    ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                    documentForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaMvto.cs", "editLink_Click"));
            }
        }
    }
}
