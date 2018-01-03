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
    public partial class ConsultaEjecucion : FormWithToolbar
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
        private int _numeroDoc = 0;
        //Variables de datos
        private DTO_pyProyectoDocu _proyectoDocu = new DTO_pyProyectoDocu();
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_pyProyectoTarea _rowTarea = new DTO_pyProyectoTarea();
        private DTO_pyProyectoMvto _rowDetalle = new DTO_pyProyectoMvto();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoDeta> _listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
        private List<DTO_pyProyectoMvto> _listMvtos = new List<DTO_pyProyectoMvto>();
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ConsultaEjecucion()
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "ConsultaEjecucion"));
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
            TareaCliente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            TareaCliente.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaCliente.AppearanceCell.Options.UseTextOptions = true;
            TareaCliente.AppearanceCell.Options.UseFont = true;
            TareaCliente.VisibleIndex = 1;
            TareaCliente.Width = 30;
            TareaCliente.Visible = true;
            TareaCliente.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaCliente);

            GridColumn TareaDesc = new GridColumn();
            TareaDesc.FieldName = this.unboundPrefix + "Descriptivo";
            TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_Descriptivo");
            TareaDesc.UnboundType = UnboundColumnType.String;
            TareaDesc.VisibleIndex = 2;
            TareaDesc.Width = 230;
            TareaDesc.Visible = true;    
            TareaDesc.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(TareaDesc);

            GridColumn UnidadInvID = new GridColumn();
            UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvID.UnboundType = UnboundColumnType.String;
            UnidadInvID.VisibleIndex = 3;
            UnidadInvID.Width = 60;
            UnidadInvID.Visible = true;
            UnidadInvID.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(UnidadInvID);

            GridColumn CantidadPresup = new GridColumn();
            CantidadPresup.FieldName = this.unboundPrefix + "CantidadPresup";
            CantidadPresup.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantPresupuestado");
            CantidadPresup.UnboundType = UnboundColumnType.Decimal;
            CantidadPresup.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPresup.AppearanceCell.Options.UseTextOptions = true;
            CantidadPresup.VisibleIndex = 4;
            CantidadPresup.Width = 90;
            CantidadPresup.Visible = true;
            CantidadPresup.ColumnEdit = this.editValue2Cant;
            CantidadPresup.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantidadPresup);

            GridColumn CantidadEjec = new GridColumn();
            CantidadEjec.FieldName = this.unboundPrefix + "CantidadEjec";
            CantidadEjec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantEjecutado");
            CantidadEjec.UnboundType = UnboundColumnType.Decimal;
            CantidadEjec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadEjec.AppearanceCell.Options.UseTextOptions = true;
            CantidadEjec.VisibleIndex = 5;
            CantidadEjec.Width = 90;
            CantidadEjec.Visible = true;
            CantidadEjec.ColumnEdit = this.editValue2Cant;
            CantidadEjec.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantidadEjec);

            GridColumn CantidadPen = new GridColumn();
            CantidadPen.FieldName = this.unboundPrefix + "CantidadPend";
            CantidadPen.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantPendiente");
            CantidadPen.UnboundType = UnboundColumnType.Decimal;
            CantidadPen.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPen.AppearanceCell.Options.UseTextOptions = true;
            CantidadPen.VisibleIndex = 6;
            CantidadPen.Width = 90;
            CantidadPen.Visible = true;
            CantidadPen.ColumnEdit = this.editValue2Cant;
            CantidadPen.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(CantidadPen);

            GridColumn VlrPresupuestado = new GridColumn();
            VlrPresupuestado.FieldName = this.unboundPrefix + "VlrPresupuestado";
            VlrPresupuestado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrPresupuestado");
            VlrPresupuestado.UnboundType = UnboundColumnType.Decimal;
            VlrPresupuestado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestado.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestado.VisibleIndex = 7;
            VlrPresupuestado.Width = 90;
            VlrPresupuestado.Visible = false;
            VlrPresupuestado.ColumnEdit = this.editValue2;
            VlrPresupuestado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(VlrPresupuestado);

            GridColumn VlrEjecutado = new GridColumn();
            VlrEjecutado.FieldName = this.unboundPrefix + "VlrEjecutado";
            VlrEjecutado.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrEjecutado");
            VlrEjecutado.UnboundType = UnboundColumnType.Decimal;
            VlrEjecutado.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrEjecutado.AppearanceCell.Options.UseTextOptions = true;
            VlrEjecutado.VisibleIndex = 8;
            VlrEjecutado.Width = 90;
            VlrEjecutado.Visible = false;
            VlrEjecutado.ColumnEdit = this.editValue2;
            VlrEjecutado.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(VlrEjecutado);

            GridColumn VlrPendiente = new GridColumn();
            VlrPendiente.FieldName = this.unboundPrefix + "VlrPendiente";
            VlrPendiente.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrPendiente");
            VlrPendiente.UnboundType = UnboundColumnType.Decimal;
            VlrPendiente.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPendiente.AppearanceCell.Options.UseTextOptions = true;
            VlrPendiente.VisibleIndex = 9;
            VlrPendiente.Width = 90;
            VlrPendiente.Visible = false;
            VlrPendiente.ColumnEdit = this.editValue2;
            VlrPendiente.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(VlrPendiente);

            GridColumn PorcentajeEjec = new GridColumn();
            PorcentajeEjec.FieldName = this.unboundPrefix + "PorcentajeEjec";
            PorcentajeEjec.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_Porcentaje");
            PorcentajeEjec.UnboundType = UnboundColumnType.Decimal;
            PorcentajeEjec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            PorcentajeEjec.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PorcentajeEjec.AppearanceCell.BackColor = Color.LightSteelBlue;           
            PorcentajeEjec.AppearanceCell.Options.UseBackColor = true;
            PorcentajeEjec.AppearanceCell.Options.UseTextOptions = true;
            PorcentajeEjec.VisibleIndex = 10;
            PorcentajeEjec.Width = 70;
            PorcentajeEjec.Visible = false;
            PorcentajeEjec.ColumnEdit = this.editPorc;
            PorcentajeEjec.OptionsColumn.AllowEdit = false;
            this.gvTareas.Columns.Add(PorcentajeEjec);

            this.gvTareas.OptionsView.ColumnAutoWidth = true;

            #endregion

            #region Grilla  Recursos
            GridColumn RecursoID = new GridColumn();
            RecursoID.FieldName = this.unboundPrefix + "RecursoID";
            RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
            RecursoID.UnboundType = UnboundColumnType.String;
            RecursoID.VisibleIndex = 1;
            RecursoID.Width = 60;
            RecursoID.Visible = true;
            RecursoID.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoID);

            GridColumn RecursoDesc = new GridColumn();
            RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
            RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
            RecursoDesc.UnboundType = UnboundColumnType.String;
            RecursoDesc.VisibleIndex = 2;
            RecursoDesc.Width = 220;
            RecursoDesc.Visible = true;
            RecursoDesc.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(RecursoDesc);

            GridColumn UnidadInvIDDet = new GridColumn();
            UnidadInvIDDet.FieldName = this.unboundPrefix + "UnidadInvID";
            UnidadInvIDDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
            UnidadInvIDDet.UnboundType = UnboundColumnType.String;
            UnidadInvIDDet.VisibleIndex = 3;
            UnidadInvIDDet.Width = 60;
            UnidadInvIDDet.Visible = true;
            UnidadInvIDDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(UnidadInvIDDet);

            GridColumn CantidadPresupDet = new GridColumn();
            CantidadPresupDet.FieldName = this.unboundPrefix + "CantidadPresup";
            CantidadPresupDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantPresupuestado");
            CantidadPresupDet.UnboundType = UnboundColumnType.Decimal;
            CantidadPresupDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPresupDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadPresupDet.VisibleIndex = 4;
            CantidadPresupDet.Width = 90;
            CantidadPresupDet.Visible = true;
            CantidadPresupDet.ColumnEdit = this.editValue2Cant;
            CantidadPresupDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadPresupDet);

            GridColumn CantidadEjecDet = new GridColumn();
            CantidadEjecDet.FieldName = this.unboundPrefix + "CantidadEjec";
            CantidadEjecDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantEjecutado");
            CantidadEjecDet.UnboundType = UnboundColumnType.Decimal;
            CantidadEjecDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadEjecDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadEjecDet.VisibleIndex = 5;
            CantidadEjecDet.Width = 90;
            CantidadEjecDet.Visible = true;
            CantidadEjecDet.ColumnEdit = this.editValue2Cant;
            CantidadEjecDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadEjecDet);

            GridColumn CantidadPendDet = new GridColumn();
            CantidadPendDet.FieldName = this.unboundPrefix + "CantidadPend";
            CantidadPendDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_CantPendiente");
            CantidadPendDet.UnboundType = UnboundColumnType.Decimal;
            CantidadPendDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadPendDet.AppearanceCell.Options.UseTextOptions = true;
            CantidadPendDet.VisibleIndex = 6;
            CantidadPendDet.Width = 90;
            CantidadPendDet.Visible = true;
            CantidadPendDet.ColumnEdit = this.editValue2Cant;
            CantidadPendDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(CantidadPendDet);

            GridColumn VlrPresupuestadoDet = new GridColumn();
            VlrPresupuestadoDet.FieldName = this.unboundPrefix + "VlrPresupuestado";
            VlrPresupuestadoDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrPresupuestado");
            VlrPresupuestadoDet.UnboundType = UnboundColumnType.Decimal;
            VlrPresupuestadoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPresupuestadoDet.AppearanceCell.Options.UseTextOptions = true;
            VlrPresupuestadoDet.VisibleIndex = 7;
            VlrPresupuestadoDet.Width = 90;
            VlrPresupuestadoDet.Visible = false;
            VlrPresupuestadoDet.ColumnEdit = this.editValue2;
            VlrPresupuestadoDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrPresupuestadoDet);

            GridColumn VlrEjecutadoDet = new GridColumn();
            VlrEjecutadoDet.FieldName = this.unboundPrefix + "VlrEjecutado";
            VlrEjecutadoDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrEjecutado");
            VlrEjecutadoDet.UnboundType = UnboundColumnType.Decimal;
            VlrEjecutadoDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrEjecutadoDet.AppearanceCell.Options.UseTextOptions = true;
            VlrEjecutadoDet.VisibleIndex = 8;
            VlrEjecutadoDet.Width = 90;
            VlrEjecutadoDet.Visible = false;
            VlrEjecutadoDet.ColumnEdit = this.editValue2;
            VlrEjecutadoDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrEjecutadoDet);

            GridColumn VlrPendienteDet = new GridColumn();
            VlrPendienteDet.FieldName = this.unboundPrefix + "VlrPendiente";
            VlrPendienteDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_VlrPendiente");
            VlrPendienteDet.UnboundType = UnboundColumnType.Decimal;
            VlrPendienteDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            VlrPendienteDet.AppearanceCell.Options.UseTextOptions = true;
            VlrPendienteDet.VisibleIndex = 9;
            VlrPendienteDet.Width = 90;
            VlrPendienteDet.Visible = false;
            VlrPendienteDet.ColumnEdit = this.editValue2;
            VlrPendienteDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(VlrPendienteDet);

            GridColumn PorcentajeDet = new GridColumn();
            PorcentajeDet.FieldName = this.unboundPrefix + "PorcentajeEjec";
            PorcentajeDet.Caption = _bc.GetResource(LanguageTypes.Forms, AppQueries.QueryEjecucion + "_Porcentaje");
            PorcentajeDet.UnboundType = UnboundColumnType.Decimal;
            PorcentajeDet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            PorcentajeDet.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PorcentajeDet.AppearanceCell.BackColor = Color.LightSteelBlue;
            PorcentajeDet.AppearanceCell.Options.UseBackColor = true;
            PorcentajeDet.AppearanceCell.Options.UseTextOptions = true;
            PorcentajeDet.VisibleIndex = 10;
            PorcentajeDet.Width = 70;
            PorcentajeDet.Visible = true;
            PorcentajeDet.ColumnEdit = this.editPorc;
            PorcentajeDet.OptionsColumn.AllowEdit = false;
            this.gvRecurso.Columns.Add(PorcentajeDet);

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion", "LoadDocumentInfo"));
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
                DTO_SolicitudTrabajo transaccion = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijoID, docNro, numeroDoc, string.Empty, proyectoID, false,true,false,false);

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

                    #region Calcula Cantidades y valores correspondientes

                    foreach (var mvto in this._listMvtos)
                    {
                        mvto.CantidadPresup.Value = mvto.CantidadTOT.Value;
                        mvto.VlrPresupuestado.Value = mvto.CostoLocalTOT.Value*mvto.CantidadTarea.Value;

                        mvto.CantidadEjec.Value = mvto.CantidadBOD.Value + mvto.CantidadREC.Value;
                        mvto.VlrEjecutado.Value = mvto.CostoLocalTOT.Value * mvto.CantidadTarea.Value;

                        mvto.CantidadPend.Value = mvto.CantidadPresup.Value - mvto.CantidadEjec.Value;
                        mvto.VlrPendiente.Value = mvto.VlrPresupuestado.Value - mvto.VlrEjecutado.Value;

                        mvto.PorcentajeEjec.Value = mvto.CantidadPresup.Value != 0?(mvto.CantidadEjec.Value * 100) / mvto.CantidadPresup.Value : 0;
                    }

                    foreach (var tarea in _listTareasAll)
                    {
                        tarea.CantidadPresup.Value = this._listMvtos.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value).Sum(x => x.CantidadPresup.Value);
                        tarea.CantidadEjec.Value = this._listMvtos.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value).Sum(x => x.CantidadEjec.Value);
                        tarea.CantidadPend.Value = tarea.CantidadPresup.Value - tarea.CantidadEjec.Value;
                        tarea.VlrPresupuestado.Value = this._listMvtos.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value ).Sum(x => x.VlrPresupuestado.Value);
                        tarea.VlrEjecutado.Value = this._listMvtos.FindAll(x => x.TareaCliente.Value == tarea.TareaCliente.Value ).Sum(x => x.VlrEjecutado.Value);
                        tarea.VlrPendiente.Value = tarea.VlrPresupuestado.Value - tarea.VlrEjecutado.Value;
                    }
                    #endregion

                    this.masterProyecto.Value = transaccion.DocCtrl.ProyectoID.Value;
                    this.masterPrefijo.Value = transaccion.DocCtrl.PrefijoID.Value;
                    this.txtNro.Text = transaccion.DocCtrl.DocumentoNro.Value.ToString();
                    this.masterCliente.Value = transaccion.HeaderProyecto.ClienteID.Value;
                    this.txtLicitacion.Text = transaccion.HeaderProyecto.Licitacion.Value;
                    this.txtDescripcion.Text = transaccion.HeaderProyecto.DescripcionSOL.Value;                
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
        private void LoadGrids()
        {
            try
            {
                this.gcTarea.DataSource = this._listTareasAll;
                this.gcTarea.RefreshDataSource();
                this.gcRecurso.RefreshDataSource();
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

            this._ctrlProyecto = null;
            this._numeroDoc = 0;
            this._proyectoDocu = new DTO_pyProyectoDocu();
            this._rowTarea = new DTO_pyProyectoTarea();
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listRecursosXTareaAll = new List<DTO_pyProyectoDeta>();
            this.gcTarea.DataSource = this._listTareasAll;
            this.gcTarea.RefreshDataSource();

            this.gcRecurso.DataSource = null;
            this.gcRecurso.RefreshDataSource();

            this.masterProyecto.Focus();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppQueries.QueryEjecucion;
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
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion", "Form_FormClosed"));
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
                    //Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPresup"].VisibleIndex = 4;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadEjec"].VisibleIndex = 5;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 6;
                    this.gvTareas.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPresup"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadEjec"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPend"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPendiente"].Visible = false;

                    //Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPresup"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadEjec"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 6;
                    this.gvRecurso.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPresup"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadEjec"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPendiente"].Visible = false;
                    #endregion
                }
                else if (this.rdGroupVer.SelectedIndex == 1)
                {
                    #region Muestra Valores
                    //Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 7;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrEjecutado"].VisibleIndex = 8;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPendiente"].VisibleIndex = 9;
                    this.gvTareas.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPendiente"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPresup"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadEjec"].Visible = false;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPend"].Visible = false;

                    //Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 7;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrEjecutado"].VisibleIndex = 8;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPendiente"].VisibleIndex = 9;
                    this.gvRecurso.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPendiente"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPresup"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadEjec"].Visible = false;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPend"].Visible = false;
                    #endregion
                }
                else if (this.rdGroupVer.SelectedIndex == 2)
                {
                    #region Muestras las Cantidades y Valores
                    //Grilla Tareas
                    this.gvTareas.Columns[this.unboundPrefix + "TareaCliente"].VisibleIndex = 1;
                    this.gvTareas.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 2;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPresup"].VisibleIndex = 3;
                    this.gvTareas.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 4;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadEjec"].VisibleIndex = 5;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 6;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 7;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrEjecutado"].VisibleIndex = 8;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPendiente"].VisibleIndex = 9;
                    this.gvTareas.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPresup"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadEjec"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "CantidadPend"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = true;
                    this.gvTareas.Columns[this.unboundPrefix + "VlrPendiente"].Visible = true;
                    //Grilla Recursos
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoID"].VisibleIndex = 1;
                    this.gvRecurso.Columns[this.unboundPrefix + "RecursoDesc"].VisibleIndex = 2;
                    this.gvRecurso.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 3;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPresup"].VisibleIndex = 4;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadEjec"].VisibleIndex = 5;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 6;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].VisibleIndex = 7;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrEjecutado"].VisibleIndex = 8;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPendiente"].VisibleIndex = 9;
                    this.gvRecurso.Columns[this.unboundPrefix + "PorcentajeEjec"].VisibleIndex = 10;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPresup"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadEjec"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "CantidadPend"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPresupuestado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrEjecutado"].Visible = true;
                    this.gvRecurso.Columns[this.unboundPrefix + "VlrPendiente"].Visible = true;

                    #endregion
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
                    this._rowTarea = (DTO_pyProyectoTarea)this.gvTareas.GetRow(e.FocusedRowHandle);
                    this.gcRecurso.DataSource = this._listMvtos.FindAll(x => x.TareaCliente.Value == this._rowTarea.TareaCliente.Value && x.CantidadPROV.Value > 0);
                    this.gcRecurso.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "gvDocument_FocusedRowChanged"));
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

            if (fieldName == "DiasAtraso" && e.RowHandle >= 0)
            {

                //decimal cellvalue = Convert.ToDecimal(e.CellValue, CultureInfo.InvariantCulture);
                //if (cellvalue > 0)
                //    e.Appearance.ForeColor = Color.Red;
                //else
                //    e.Appearance.ForeColor = Color.Black;
            }
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
                DTO_pyProyectoTarea currentRow = (DTO_pyProyectoTarea)this.gvTareas.GetRow(e.RowHandle);
                if (currentRow != null)
                {
                    if (currentRow.DetalleInd.Value.Value)
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    else
                        e.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
                    this._rowDetalle = (DTO_pyProyectoMvto)this.gvRecurso.GetRow(e.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "gvRecurso_FocusedRowChanged"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "gvRecurso_CustomColumnDisplayText"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ConsultaEjecucion.cs", "editLink_Click"));
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

        #endregion
        
    }
}
