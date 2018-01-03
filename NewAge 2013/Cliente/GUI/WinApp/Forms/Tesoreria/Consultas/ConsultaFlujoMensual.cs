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
    public partial class ConsultaFlujoMensual : FormWithToolbar
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
        private DTO_QueryFlujoFondos _rowCurrent = new DTO_QueryFlujoFondos();
        private GridView _gridDetalleCurrent = new GridView();
        private List<DTO_QueryFlujoFondos> _listProyectos = new List<DTO_QueryFlujoFondos>();
 
        private DTO_QueryFlujoFondosTareas _rowTareaCurrent = new DTO_QueryFlujoFondosTareas();
        private GridView _gridDetalleTareaCurrent = new GridView();
        private List<DTO_QueryFlujoFondosTareas> _listTareas = new List<DTO_QueryFlujoFondosTareas>();
 
        private string semana = string.Empty;
        DateTime fecha = DateTime.Now;
        #endregion        

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaFlujoMensual()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.ts;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual.cs", "ConsultaFlujoMensual"));
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
            Proyecto.Width = 90;
            Proyecto.Visible = true;
            Proyecto.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Proyecto);

            GridColumn ProyectoDesc = new GridColumn();
            ProyectoDesc.FieldName = this.unboundPrefix + "ProyectoDesc";
            ProyectoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoDesc");
            ProyectoDesc.UnboundType = UnboundColumnType.String;
            ProyectoDesc.VisibleIndex = 2;
            ProyectoDesc.Width = 150;
            ProyectoDesc.Visible = true;
            ProyectoDesc.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(ProyectoDesc);

            GridColumn DocProyecto = new GridColumn();
            DocProyecto.FieldName = this.unboundPrefix + "DocProyecto";
            DocProyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_DocProyecto");
            DocProyecto.UnboundType = UnboundColumnType.String;
            DocProyecto.VisibleIndex = 3;
            DocProyecto.Width = 80;
            DocProyecto.Visible = true;
            DocProyecto.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(DocProyecto);         

            GridColumn PerA = new GridColumn();
            PerA.FieldName = this.unboundPrefix + "PerA";
            PerA.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerA");
            PerA.UnboundType = UnboundColumnType.Decimal;
            PerA.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerA.VisibleIndex = 4;
            PerA.AppearanceCell.Options.UseTextOptions = true;
            PerA.Width = 90;
            PerA.Visible = true;
            PerA.OptionsColumn.AllowEdit = false;
            PerA.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(PerA);


            GridColumn Per0 = new GridColumn();
            Per0.FieldName = this.unboundPrefix + "Per0";
            this.semana = this._bc.AdministrationModel.Global_Mes(0); 
            Per0.Caption = this.semana.ToString();
            Per0.UnboundType = UnboundColumnType.Decimal;
            Per0.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per0.AppearanceCell.Options.UseTextOptions = true;
            Per0.VisibleIndex = 5;
            Per0.Width = 90;
            Per0.Visible = true;
            Per0.ColumnEdit = this.editValue2;
            Per0.OptionsColumn.AllowEdit = false;
            this.gvProyectos.Columns.Add(Per0);

            GridColumn Per1 = new GridColumn();
            Per1.FieldName = this.unboundPrefix + "Per1";
            this.semana = this._bc.AdministrationModel.Global_Mes(1); 
            Per1.Caption = this.semana.ToString();
            Per1.UnboundType = UnboundColumnType.Decimal;
            Per1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per1.AppearanceCell.Options.UseTextOptions = true;
            Per1.VisibleIndex = 6;
            Per1.Width = 90;
            Per1.Visible = true;
            Per1.OptionsColumn.AllowEdit = false;
            Per1.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per1);

            GridColumn Per2 = new GridColumn();
            Per2.FieldName = this.unboundPrefix + "Per2";
            this.semana = this._bc.AdministrationModel.Global_Mes(2); 
            Per2.Caption = this.semana.ToString();
            Per2.UnboundType = UnboundColumnType.Decimal;
            Per2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per2.AppearanceCell.Options.UseTextOptions = true;
            Per2.VisibleIndex = 7;
            Per2.Width = 90;
            Per2.Visible = true;
            Per2.OptionsColumn.AllowEdit = false;
            Per2.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per2);

            GridColumn Per3 = new GridColumn();
            Per3.FieldName = this.unboundPrefix + "Per3";
            this.semana = this._bc.AdministrationModel.Global_Mes(3); 
            Per3.Caption = this.semana.ToString();
            Per3.UnboundType = UnboundColumnType.Decimal;
            Per3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per3.AppearanceCell.Options.UseTextOptions = true;
            Per3.VisibleIndex = 8;
            Per3.Width = 90;
            Per3.Visible = true;
            Per3.OptionsColumn.AllowEdit = false;
            Per3.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per3);

            GridColumn Per4 = new GridColumn();
            Per4.FieldName = this.unboundPrefix + "Per4";
            this.semana = this._bc.AdministrationModel.Global_Mes(4); 
            Per4.Caption = this.semana.ToString();
            Per4.UnboundType = UnboundColumnType.Decimal;
            Per4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per4.AppearanceCell.Options.UseTextOptions = true;
            Per4.VisibleIndex = 9;
            Per4.Width = 90;
            Per4.Visible = true;
            Per4.OptionsColumn.AllowEdit = false;
            Per4.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per4);

            GridColumn Per5 = new GridColumn();
            Per5.FieldName = this.unboundPrefix + "Per5";
            this.semana = this._bc.AdministrationModel.Global_Mes(5); 
            Per5.Caption = this.semana.ToString();
            Per5.UnboundType = UnboundColumnType.Decimal;
            Per5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per5.AppearanceCell.Options.UseTextOptions = true;
            Per5.VisibleIndex = 10;
            Per5.Width = 90;
            Per5.Visible = true;
            Per5.OptionsColumn.AllowEdit = false;
            Per5.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per5);

            GridColumn Per6 = new GridColumn();
            Per6.FieldName = this.unboundPrefix + "Per6";
            this.semana = this._bc.AdministrationModel.Global_Mes(6); 
            Per6.Caption = this.semana.ToString();
            Per6.UnboundType = UnboundColumnType.Decimal;
            Per6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per6.AppearanceCell.Options.UseTextOptions = true;
            Per6.AppearanceCell.Options.UseFont = true;
            Per6.AppearanceCell.Options.UseForeColor = true;
            Per6.VisibleIndex = 11;
            Per6.Width = 90;
            Per6.Visible = true;
            Per6.OptionsColumn.AllowEdit = false;
            Per6.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(Per6);

            GridColumn PerM = new GridColumn();
            PerM.FieldName = this.unboundPrefix + "PerM";
            PerM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerM");
            PerM.UnboundType = UnboundColumnType.Decimal;
            PerM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerM.AppearanceCell.Options.UseTextOptions = true;
            PerM.AppearanceCell.Options.UseFont = true;
            PerM.AppearanceCell.Options.UseForeColor = true;
            PerM.VisibleIndex = 12;
            PerM.Width = 90;
            PerM.Visible = true;
            PerM.OptionsColumn.AllowEdit = false;
            PerM.ColumnEdit = this.editValue2;
            this.gvProyectos.Columns.Add(PerM);

            this.gvProyectos.OptionsView.ColumnAutoWidth = true;

            #endregion
            #region Sub Grilla Proyectos
            GridColumn Documento = new GridColumn();
            Documento.FieldName = this.unboundPrefix + "Documento";
            Documento.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
            Documento.UnboundType = UnboundColumnType.String;
            Documento.AppearanceCell.Options.UseTextOptions = true;
            Documento.AppearanceCell.Options.UseFont = true;
            Documento.VisibleIndex = 0;
            Documento.Width = 100;
            Documento.Visible = true;
            Documento.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Documento);


            GridColumn PerADet = new GridColumn();
            PerADet.FieldName = this.unboundPrefix + "PerA";
            PerADet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerA");
            PerADet.UnboundType = UnboundColumnType.Decimal;
            PerADet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerADet.VisibleIndex = 2;
            PerADet.AppearanceCell.Options.UseTextOptions = true;
            PerADet.Width = 90;
            PerADet.Visible = true;
            PerADet.OptionsColumn.AllowEdit = false;
            PerADet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(PerADet);


            GridColumn Per0Det = new GridColumn();
            Per0Det.FieldName = this.unboundPrefix + "Per0";
            this.semana = this._bc.AdministrationModel.Global_Mes(0);
            Per0Det.Caption = this.semana.ToString();
            Per0Det.UnboundType = UnboundColumnType.Decimal;
            Per0Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per0Det.AppearanceCell.Options.UseTextOptions = true;
            Per0Det.VisibleIndex = 3;
            Per0Det.Width = 90;
            Per0Det.Visible = true;
            Per0Det.ColumnEdit = this.editValue2;
            Per0Det.OptionsColumn.AllowEdit = false;
            this.gvDetalle.Columns.Add(Per0Det);

            GridColumn Per1Det = new GridColumn();
            Per1Det.FieldName = this.unboundPrefix + "Per1";
            this.semana = this._bc.AdministrationModel.Global_Mes(1);
            Per1Det.Caption = this.semana.ToString();
            Per1Det.UnboundType = UnboundColumnType.Decimal;
            Per1Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per1Det.AppearanceCell.Options.UseTextOptions = true;
            Per1Det.VisibleIndex = 4;
            Per1Det.Width = 90;
            Per1Det.Visible = true;
            Per1Det.OptionsColumn.AllowEdit = false;
            Per1Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per1Det);

            GridColumn Per2Det = new GridColumn();
            Per2Det.FieldName = this.unboundPrefix + "Per2";
            this.semana = this._bc.AdministrationModel.Global_Mes(2);
            Per2Det.Caption = this.semana.ToString();
            Per2Det.UnboundType = UnboundColumnType.Decimal;
            Per2Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per2Det.AppearanceCell.Options.UseTextOptions = true;
            Per2Det.VisibleIndex = 5;
            Per2Det.Width = 90;
            Per2Det.Visible = true;
            Per2Det.OptionsColumn.AllowEdit = false;
            Per2Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per2Det);

            GridColumn Per3Det = new GridColumn();
            Per3Det.FieldName = this.unboundPrefix + "Per3";
            this.semana = this._bc.AdministrationModel.Global_Mes(3);
            Per3Det.Caption = this.semana.ToString();
            Per3Det.UnboundType = UnboundColumnType.Decimal;
            Per3Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per3Det.AppearanceCell.Options.UseTextOptions = true;
            Per3Det.VisibleIndex = 6;
            Per3Det.Width = 90;
            Per3Det.Visible = true;
            Per3Det.OptionsColumn.AllowEdit = false;
            Per3Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per3Det);

            GridColumn Per4Det = new GridColumn();
            Per4Det.FieldName = this.unboundPrefix + "Per4";
            this.semana = this._bc.AdministrationModel.Global_Mes(4);
            Per4Det.Caption = this.semana.ToString();
            Per4Det.UnboundType = UnboundColumnType.Decimal;
            Per4Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per4Det.AppearanceCell.Options.UseTextOptions = true;
            Per4Det.VisibleIndex = 7;
            Per4Det.Width = 90;
            Per4Det.Visible = true;
            Per4Det.OptionsColumn.AllowEdit = false;
            Per4Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per4Det);

            GridColumn Per5Det = new GridColumn();
            Per5Det.FieldName = this.unboundPrefix + "Per5";
            this.semana = this._bc.AdministrationModel.Global_Mes(5);
            Per5Det.Caption = this.semana.ToString();
            Per5Det.UnboundType = UnboundColumnType.Decimal;
            Per5Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per5Det.AppearanceCell.Options.UseTextOptions = true;
            Per5Det.VisibleIndex = 8;
            Per5Det.Width = 90;
            Per5Det.Visible = true;
            Per5Det.OptionsColumn.AllowEdit = false;
            Per5Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per5Det);

            GridColumn Per6Det = new GridColumn();
            Per6Det.FieldName = this.unboundPrefix + "Per6";
            this.semana = this._bc.AdministrationModel.Global_Mes(6);
            Per6Det.Caption = this.semana.ToString();
            Per6Det.UnboundType = UnboundColumnType.Decimal;
            Per6Det.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per6Det.AppearanceCell.Options.UseTextOptions = true;
            Per6Det.AppearanceCell.Options.UseFont = true;
            Per6Det.AppearanceCell.Options.UseForeColor = true;
            Per6Det.VisibleIndex = 11;
            Per6Det.Width = 90;
            Per6Det.Visible = true;
            Per6Det.OptionsColumn.AllowEdit = false;
            Per6Det.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(Per6Det);

            GridColumn PerMDet = new GridColumn();
            PerMDet.FieldName = this.unboundPrefix + "PerM";
            PerMDet.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerM");
            PerMDet.UnboundType = UnboundColumnType.Decimal;
            PerMDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerMDet.AppearanceCell.Options.UseTextOptions = true;
            PerMDet.AppearanceCell.Options.UseFont = true;
            PerMDet.AppearanceCell.Options.UseForeColor = true;
            PerMDet.VisibleIndex = 12;
            PerMDet.Width = 90;
            PerMDet.Visible = true;
            PerMDet.OptionsColumn.AllowEdit = false;
            PerMDet.ColumnEdit = this.editValue2;
            this.gvDetalle.Columns.Add(PerMDet);


            #endregion
            #region Grilla  Tareas  

            //inReferenciaID
            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 100;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(TareaID);

            //Descriptivo
            GridColumn desc = new GridColumn();
            desc.FieldName = this.unboundPrefix + "DescTarea";
            desc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_TareaDesc");
            desc.UnboundType = UnboundColumnType.String;
            desc.VisibleIndex = 2;
            desc.Width = 150;
            desc.Visible = true;
            desc.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(desc);

            GridColumn PerATarea = new GridColumn();
            PerATarea.FieldName = this.unboundPrefix + "PerA";
            PerATarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerA");
            PerATarea.UnboundType = UnboundColumnType.Decimal;
            PerATarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerATarea.VisibleIndex = 3;
            PerATarea.AppearanceCell.Options.UseTextOptions = true;
            PerATarea.Width = 90;
            PerATarea.Visible = true;
            PerATarea.OptionsColumn.AllowEdit = false;
            PerATarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(PerATarea);


            GridColumn Per0Tarea = new GridColumn();
            Per0Tarea.FieldName = this.unboundPrefix + "Per0";
            this.semana = this._bc.AdministrationModel.Global_Mes(0); 
            Per0Tarea.Caption = this.semana.ToString();
            Per0Tarea.UnboundType = UnboundColumnType.Decimal;
            Per0Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per0Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per0Tarea.VisibleIndex = 4;
            Per0Tarea.Width = 90;
            Per0Tarea.Visible = true;
            Per0Tarea.ColumnEdit = this.editValue2;
            Per0Tarea.OptionsColumn.AllowEdit = false;
            this.gvTarea.Columns.Add(Per0Tarea);

            GridColumn Per1Tarea = new GridColumn();
            Per1Tarea.FieldName = this.unboundPrefix + "Per1";
            this.semana = this._bc.AdministrationModel.Global_Mes(1); 
            Per1Tarea.Caption = this.semana.ToString();
            Per1Tarea.UnboundType = UnboundColumnType.Decimal;
            Per1Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per1Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per1Tarea.VisibleIndex = 5;
            Per1Tarea.Width = 90;
            Per1Tarea.Visible = true;
            Per1Tarea.OptionsColumn.AllowEdit = false;
            Per1Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per1Tarea);

            GridColumn Per2Tarea = new GridColumn();
            Per2Tarea.FieldName = this.unboundPrefix + "Per2";
            this.semana = this._bc.AdministrationModel.Global_Mes(2); 
            Per2Tarea.Caption = this.semana.ToString();
            Per2Tarea.UnboundType = UnboundColumnType.Decimal;
            Per2Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per2Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per2Tarea.VisibleIndex = 6;
            Per2Tarea.Width = 90;
            Per2Tarea.Visible = true;
            Per2Tarea.OptionsColumn.AllowEdit = false;
            Per2Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per2Tarea);

            GridColumn Per3Tarea = new GridColumn();
            Per3Tarea.FieldName = this.unboundPrefix + "Per3";
            this.semana = this._bc.AdministrationModel.Global_Mes(3); 
            Per3Tarea.Caption = this.semana.ToString();
            Per3Tarea.UnboundType = UnboundColumnType.Decimal;
            Per3Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per3Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per3Tarea.VisibleIndex = 7;
            Per3Tarea.Width = 90;
            Per3Tarea.Visible = true;
            Per3Tarea.OptionsColumn.AllowEdit = false;
            Per3Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per3Tarea);

            GridColumn Per4Tarea = new GridColumn();
            Per4Tarea.FieldName = this.unboundPrefix + "Per4";
            this.semana = this._bc.AdministrationModel.Global_Mes(4); 
            Per4Tarea.Caption = this.semana.ToString();
            Per4Tarea.UnboundType = UnboundColumnType.Decimal;
            Per4Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per4Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per4Tarea.VisibleIndex = 8;
            Per4Tarea.Width = 90;
            Per4Tarea.Visible = true;
            Per4Tarea.OptionsColumn.AllowEdit = false;
            Per4Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per4Tarea);

            GridColumn Per5Tarea = new GridColumn();
            Per5Tarea.FieldName = this.unboundPrefix + "Per5";
            this.semana = this._bc.AdministrationModel.Global_Mes(5); 
            Per5Tarea.Caption = this.semana.ToString();
            Per5Tarea.UnboundType = UnboundColumnType.Decimal;
            Per5Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per5Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per5Tarea.VisibleIndex = 9;
            Per5Tarea.Width = 90;
            Per5Tarea.Visible = true;
            Per5Tarea.OptionsColumn.AllowEdit = false;
            Per5Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per5Tarea);

            GridColumn Per6Tarea = new GridColumn();
            Per6Tarea.FieldName = this.unboundPrefix + "Per6";
            this.semana = this._bc.AdministrationModel.Global_Mes(6); 
            Per6Tarea.Caption = this.semana.ToString();
            Per6Tarea.UnboundType = UnboundColumnType.Decimal;
            Per6Tarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per6Tarea.AppearanceCell.Options.UseTextOptions = true;
            Per6Tarea.AppearanceCell.Options.UseFont = true;
            Per6Tarea.AppearanceCell.Options.UseForeColor = true;
            Per6Tarea.VisibleIndex = 10;
            Per6Tarea.Width = 90;
            Per6Tarea.Visible = true;
            Per6Tarea.OptionsColumn.AllowEdit = false;
            Per6Tarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(Per6Tarea);

            GridColumn PerMTarea = new GridColumn();
            PerMTarea.FieldName = this.unboundPrefix + "PerM";
            PerMTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerM");
            PerMTarea.UnboundType = UnboundColumnType.Decimal;
            PerMTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerMTarea.AppearanceCell.Options.UseTextOptions = true;
            PerMTarea.AppearanceCell.Options.UseFont = true;
            PerMTarea.AppearanceCell.Options.UseForeColor = true;
            PerMTarea.VisibleIndex = 12;
            PerMTarea.Width = 90;
            PerMTarea.Visible = true;
            PerMTarea.OptionsColumn.AllowEdit = false;
            PerMTarea.ColumnEdit = this.editValue2;
            this.gvTarea.Columns.Add(PerMTarea);


            this.gvTarea.OptionsView.ColumnAutoWidth = true;
            #endregion
            #region Sub Grilla Tareas
            GridColumn DocumentoTarea = new GridColumn();
            DocumentoTarea.FieldName = this.unboundPrefix + "Documento";
            DocumentoTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Documento");
            DocumentoTarea.UnboundType = UnboundColumnType.String;
            DocumentoTarea.AppearanceCell.Options.UseTextOptions = true;
            DocumentoTarea.AppearanceCell.Options.UseFont = true;
            DocumentoTarea.VisibleIndex = 1;
            DocumentoTarea.Width = 100;
            DocumentoTarea.Visible = true;
            DocumentoTarea.OptionsColumn.AllowEdit = false;
            this.gvDetalleTarea.Columns.Add(DocumentoTarea);


            GridColumn PerADetTarea = new GridColumn();
            PerADetTarea.FieldName = this.unboundPrefix + "PerA";
            PerADetTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerA");
            PerADetTarea.UnboundType = UnboundColumnType.Decimal;
            PerADetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerADetTarea.VisibleIndex = 2;
            PerADetTarea.AppearanceCell.Options.UseTextOptions = true;
            PerADetTarea.Width = 90;
            PerADetTarea.Visible = true;
            PerADetTarea.OptionsColumn.AllowEdit = false;
            PerADetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(PerADetTarea);


            GridColumn Per0DetTarea = new GridColumn();
            Per0DetTarea.FieldName = this.unboundPrefix + "Per0";
            this.semana = this._bc.AdministrationModel.Global_Mes(0); 
            Per0DetTarea.Caption = this.semana.ToString();
            Per0DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per0DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per0DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per0DetTarea.VisibleIndex = 3;
            Per0DetTarea.Width = 90;
            Per0DetTarea.Visible = true;
            Per0DetTarea.ColumnEdit = this.editValue2;
            Per0DetTarea.OptionsColumn.AllowEdit = false;
            this.gvDetalleTarea.Columns.Add(Per0DetTarea);

            GridColumn Per1DetTarea = new GridColumn();
            Per1DetTarea.FieldName = this.unboundPrefix + "Per1";
            this.semana = this._bc.AdministrationModel.Global_Mes(1); 
            Per1DetTarea.Caption = this.semana.ToString();
            Per1DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per1DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per1DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per1DetTarea.VisibleIndex = 4;
            Per1DetTarea.Width = 90;
            Per1DetTarea.Visible = true;
            Per1DetTarea.OptionsColumn.AllowEdit = false;
            Per1DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per1DetTarea);

            GridColumn Per2DetTarea = new GridColumn();
            Per2DetTarea.FieldName = this.unboundPrefix + "Per2";
            this.semana = this._bc.AdministrationModel.Global_Mes(2); 
            Per2DetTarea.Caption = this.semana.ToString();
            Per2DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per2DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per2DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per2DetTarea.VisibleIndex = 5;
            Per2DetTarea.Width = 90;
            Per2DetTarea.Visible = true;
            Per2DetTarea.OptionsColumn.AllowEdit = false;
            Per2DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per2DetTarea);

            GridColumn Per3DetTarea = new GridColumn();
            Per3DetTarea.FieldName = this.unboundPrefix + "Per3";
            this.semana = this._bc.AdministrationModel.Global_Mes(3); 
            this.semana = obtenerNombreMesNumero(this.fecha.Month);
            Per3DetTarea.Caption = this.semana.ToString();
            Per3DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per3DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per3DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per3DetTarea.VisibleIndex = 6;
            Per3DetTarea.Width = 90;
            Per3DetTarea.Visible = true;
            Per3DetTarea.OptionsColumn.AllowEdit = false;
            Per3DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per3DetTarea);

            GridColumn Per4DetTarea = new GridColumn();
            Per4DetTarea.FieldName = this.unboundPrefix + "Per4";
            this.semana = this._bc.AdministrationModel.Global_Mes(4); 
            Per4DetTarea.Caption = this.semana.ToString();
            Per4DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per4DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per4DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per4DetTarea.VisibleIndex = 7;
            Per4DetTarea.Width = 90;
            Per4DetTarea.Visible = true;
            Per4DetTarea.OptionsColumn.AllowEdit = false;
            Per4DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per4DetTarea);

            GridColumn Per5DetTarea = new GridColumn();
            Per5DetTarea.FieldName = this.unboundPrefix + "Per5";
            this.semana = this._bc.AdministrationModel.Global_Mes(5); 
            Per5DetTarea.Caption = this.semana.ToString();
            Per5DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per5DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per5DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per5DetTarea.VisibleIndex = 8;
            Per5DetTarea.Width = 90;
            Per5DetTarea.Visible = true;
            Per5DetTarea.OptionsColumn.AllowEdit = false;
            Per5DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per5DetTarea);

            GridColumn Per6DetTarea = new GridColumn();
            Per6DetTarea.FieldName = this.unboundPrefix + "Per6";
            this.semana = this._bc.AdministrationModel.Global_Mes(6); 
            Per6DetTarea.Caption = this.semana.ToString();
            Per6DetTarea.UnboundType = UnboundColumnType.Decimal;
            Per6DetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Per6DetTarea.AppearanceCell.Options.UseTextOptions = true;
            Per6DetTarea.AppearanceCell.Options.UseFont = true;
            Per6DetTarea.AppearanceCell.Options.UseForeColor = true;
            Per6DetTarea.VisibleIndex = 11;
            Per6DetTarea.Width = 90;
            Per6DetTarea.Visible = true;
            Per6DetTarea.OptionsColumn.AllowEdit = false;
            Per6DetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(Per6DetTarea);

            GridColumn PerMDetTarea = new GridColumn();
            PerMDetTarea.FieldName = this.unboundPrefix + "PerM";
            PerMDetTarea.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PerM");
            PerMDetTarea.UnboundType = UnboundColumnType.Decimal;
            PerMDetTarea.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            PerMDetTarea.AppearanceCell.Options.UseTextOptions = true;
            PerMDetTarea.AppearanceCell.Options.UseFont = true;
            PerMDetTarea.AppearanceCell.Options.UseForeColor = true;
            PerMDetTarea.VisibleIndex = 12;
            PerMDetTarea.Width = 90;
            PerMDetTarea.Visible = true;
            PerMDetTarea.OptionsColumn.AllowEdit = false;
            PerMDetTarea.ColumnEdit = this.editValue2;
            this.gvDetalleTarea.Columns.Add(PerMDetTarea);


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

                this._listProyectos = this._bc.AdministrationModel.tsFlujoFondos(this.dtFechaCorte.DateTime);


                //foreach (DTO_QueryFlujoFondos proy in this._listProyectos)
                //{
                //    this._listTareas = this._bc.AdministrationModel.tsFlujoFondos_Tareas(this.dtFechaCorte.DateTime,proy.Proyecto.Value);
                //}
                this.LoadGrids();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual", "LoadData"));
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
        /// Carga la información de cada compra
        /// </summary>
        private void LoadGridTareas()
        {
            try
            {
                this.gcTarea.DataSource = null;
                this.gcTarea.DataSource = this._listTareas;
                this.gcTarea.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual", "LoadReferencias"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        {

            this._rowCurrent = new DTO_QueryFlujoFondos();
            this._rowTareaCurrent = new DTO_QueryFlujoFondosTareas();
            this._listProyectos = new List<DTO_QueryFlujoFondos>();
            this.gcProyectos.DataSource = this._listProyectos;
            this.gcProyectos.RefreshDataSource();

            this.gcTarea.DataSource = null;
            this.gcTarea.RefreshDataSource();

        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.ts;
            this.documentID = AppQueries.QueryFlujoMensual;
            this.AddGridCols();

            this.dtFechaCorte.DateTime = DateTime.Now;
            

            this.LoadData();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Permite obtener el nombre del mes
        /// </summary>
        /// <param name="numeroMes"></param>
        /// <returns></returns>
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual", "Form_Enter"));
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
                    this._rowCurrent = (DTO_QueryFlujoFondos)this.gvProyectos.GetRow(e.FocusedRowHandle);

                    if (this._rowCurrent != null)
                    {
                        this._listTareas = this._bc.AdministrationModel.tsFlujoFondos_Tareas(this.dtFechaCorte.DateTime, this._rowCurrent.Proyecto.Value, null);
                    }
                    this.LoadGridTareas();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual.cs", "gvDocument_FocusedRowChanged"));
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
                    if (row.GetType() == typeof(DTO_QueryFlujoFondosTareas))
                    {
                        this._rowTareaCurrent = (DTO_QueryFlujoFondosTareas)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                        this.LoadGridTareas();
                    }
                    else if (row.GetType() == typeof(DTO_QueryFlujoFondos))
                        this._rowCurrent = (DTO_QueryFlujoFondos)this._gridDetalleCurrent.GetRow(this._gridDetalleCurrent.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual.cs", "gcProyectos_FocusedViewChanged"));
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
                    //this._rowTareaCurrent = (DTO_QueryFlujoFondosTareas)this._gridDetalleCurrent.GetRow(e.FocusedRowHandle);
                    DTO_QueryFlujoFondosDetalle _rowDet = (DTO_QueryFlujoFondosDetalle)this._gridDetalleCurrent.GetRow(e.FocusedRowHandle);
                    
                    this._listTareas = this._bc.AdministrationModel.tsFlujoFondos_Tareas(this.dtFechaCorte.DateTime, this._rowCurrent.Proyecto.Value, _rowDet.Documento.Value =="Recaudos" ? true : false);

                    this.LoadGridTareas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaFlujoMensual.cs", "gvDetalleSol_FocusedRowChanged"));
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
