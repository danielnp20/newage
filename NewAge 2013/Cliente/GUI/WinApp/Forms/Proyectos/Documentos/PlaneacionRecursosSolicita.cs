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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Threading;
using NewAge.DTO.Resultados;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using NewAge.DTO.UDT;
using System.Reflection;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PlaneacionRecursosSolicita : FormWithToolbar
    {  
        #region Variables

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private List<DTO_pyProyectoMvto> _mvtosSelected = null;
        private List<DTO_pyProyectoMvto> _mvtosAll = null;
        private int userID = 0;
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private bool multiMoneda;
        private bool allowValidate = true;
        private string actividadFlujoID = string.Empty;
        private DTO_glActividadFlujo actividadDTO = null;

        private int currentRow = -1;
        private string monedaID;
        private string monedaExtranjeraID;
        private string areaFuncionalID;
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
      
        #endregion

        #region Delegados

        private delegate void RefreshData();
        private RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        private void RefreshDataMethod()
        {
            this.LoadDocuments();
            this.chkSeleccionar.Checked = false;
            FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
        }

        #endregion

        public PlaneacionRecursosSolicita()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();          
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.AfterInitialize();

                //Asigna la lista de columnas
                this.AddGridCols();

                //#region Carga la info de las actividades
                //List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                //if (actividades.Count != 1)
                //{
                //    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                //    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                //}
                //else
                //{
                //    this.actividadFlujoID = actividades[0];
                //    this.LoadDocuments();
                //}

                //if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                //    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                //#endregion
                this.LoadDocuments();
                this.refreshData = new RefreshData(RefreshDataMethod);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "DocumentAprobBasicForm"));
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Documentos

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
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Seleccionar");
                aprob.VisibleIndex = 0;
                aprob.Width = 35;
                aprob.Visible = true;
                this.gvRecurso.Columns.Add(aprob);

                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 1;
                RecursoID.Width = 70;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(RecursoID);

                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_RecursoIDDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 2;
                RecursoDesc.Width = 450;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(RecursoDesc);

                GridColumn UnidadInvIDrec = new GridColumn();
                UnidadInvIDrec.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvIDrec.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.PreProyecto + "_UnidadInvID");
                UnidadInvIDrec.UnboundType = UnboundColumnType.String;
                UnidadInvIDrec.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UnidadInvIDrec.AppearanceCell.Options.UseTextOptions = true;
                UnidadInvIDrec.VisibleIndex = 3;
                UnidadInvIDrec.Width = 50;
                UnidadInvIDrec.Visible = true;
                UnidadInvIDrec.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(UnidadInvIDrec);

                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 4;
                MarcaInvID.Width = 60;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(MarcaInvID);

                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 5;
                RefProveedor.Width = 60;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(RefProveedor);

                GridColumn CodigoBSID = new GridColumn();
                CodigoBSID.FieldName = this.unboundPrefix + "CodigoBSID";
                CodigoBSID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                CodigoBSID.UnboundType = UnboundColumnType.String;
                CodigoBSID.VisibleIndex = 6;
                CodigoBSID.Width = 70;
                CodigoBSID.Visible = true;
                CodigoBSID.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(CodigoBSID);

                GridColumn CodigoBSDesc = new GridColumn();
                CodigoBSDesc.FieldName = this.unboundPrefix + "CodigoBSDesc";
                CodigoBSDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSDesc");
                CodigoBSDesc.UnboundType = UnboundColumnType.String;
                CodigoBSDesc.VisibleIndex = 7;
                CodigoBSDesc.Width = 120;
                CodigoBSDesc.Visible = true;
                CodigoBSDesc.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(CodigoBSDesc);

                GridColumn CantidadSUM = new GridColumn();
                CantidadSUM.FieldName = this.unboundPrefix + "CantidadSUM";
                CantidadSUM.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSUM");
                CantidadSUM.UnboundType = UnboundColumnType.Decimal;
                CantidadSUM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadSUM.AppearanceCell.Options.UseTextOptions = true;
                CantidadSUM.VisibleIndex = 8;
                CantidadSUM.Width = 80;
                CantidadSUM.Visible = true;
                CantidadSUM.ColumnEdit = this.editValue2;
                CantidadSUM.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(CantidadSUM);

                GridColumn FechaOrdCompra = new GridColumn();
                FechaOrdCompra.FieldName = this.unboundPrefix + "FechaOrdCompra";
                FechaOrdCompra.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaOrdCompra");
                FechaOrdCompra.UnboundType = UnboundColumnType.DateTime;
                FechaOrdCompra.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                FechaOrdCompra.AppearanceCell.Options.UseTextOptions = true;
                FechaOrdCompra.VisibleIndex = 9;
                FechaOrdCompra.Width = 100;
                FechaOrdCompra.Visible = true;
                FechaOrdCompra.OptionsColumn.AllowEdit = false;
                this.gvRecurso.Columns.Add(FechaOrdCompra);

                GridColumn TipoRecurso = new GridColumn();
                TipoRecurso.FieldName = this.unboundPrefix + "TipoRecurso";
                TipoRecurso.UnboundType = UnboundColumnType.Integer;
                TipoRecurso.Width = 5;
                TipoRecurso.Visible = false;
                TipoRecurso.Group();
                TipoRecurso.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
                TipoRecurso.SortOrder = ColumnSortOrder.Ascending;
                this.gvRecurso.Columns.Add(TipoRecurso);
                #endregion

                #region Detalles

                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 0;
                PrefDoc.Width = 70;
                PrefDoc.Visible = true;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(PrefDoc);

                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 0;
                ProyectoID.Width = 60;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(ProyectoID);

                GridColumn TareaCliente = new GridColumn();
                TareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
                TareaCliente.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaCliente");
                TareaCliente.UnboundType = UnboundColumnType.String;
                TareaCliente.VisibleIndex = 1;
                TareaCliente.Width = 120;
                TareaCliente.Visible = true;
                TareaCliente.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(TareaCliente);

                GridColumn TareaDesc = new GridColumn();
                TareaDesc.FieldName = this.unboundPrefix + "TareaDesc";
                TareaDesc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_TareaDesc");
                TareaDesc.UnboundType = UnboundColumnType.String;
                TareaDesc.VisibleIndex = 2;
                TareaDesc.Width = 250;
                TareaDesc.Visible = true;
                TareaDesc.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(TareaDesc);

                GridColumn Cantidad = new GridColumn();
                Cantidad.FieldName = this.unboundPrefix + "CantidadSOL";
                Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSOL");
                Cantidad.UnboundType = UnboundColumnType.Integer;
                Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                Cantidad.AppearanceCell.Options.UseTextOptions = true;
                Cantidad.VisibleIndex = 3;
                Cantidad.Width = 100;
                Cantidad.Visible = true;
                Cantidad.ColumnEdit = this.editValue2;
                Cantidad.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(Cantidad);

                GridColumn FactorID = new GridColumn();
                FactorID.FieldName = this.unboundPrefix + "FactorID";
                FactorID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_FactorID");
                FactorID.UnboundType = UnboundColumnType.Decimal;
                FactorID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                FactorID.AppearanceCell.Options.UseTextOptions = true;
                FactorID.VisibleIndex = 4;
                FactorID.Width = 100;
                FactorID.Visible = true;
                FactorID.ColumnEdit = this.editValue2;
                FactorID.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(FactorID);
                #endregion

                this.gvRecurso.OptionsView.ColumnAutoWidth = true;
                this.gvTarea.OptionsView.ColumnAutoWidth = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita", "AddCols_Documentos"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private  void SetInitParameters()
        {
            this.documentID = AppDocuments.PlaneacionComprasAprob;
            this.frmModule = ModulesPrefix.py;
        }

        /// <summary>
        /// Carga la información de la grilla de documentos
        /// </summary>
        private  void LoadDocuments()
        {
            try
            {
                this._mvtosSelected = new List<DTO_pyProyectoMvto>();
                this._mvtosAll = this._bc.AdministrationModel.CompraRecursos_GetPendientesForApprove(this.dtFecha.DateTime);

                if (this.masterProyecto.ValidID)
                    this._mvtosAll = this._mvtosAll.FindAll(x => x.ProyectoID.Value == this.masterProyecto.Value).ToList();

                //mvtos = mvtos.FindAll(x => x.TipoRecurso.Value != (byte)TipoRecurso.Personal).ToList();
                List<string> recursosDistinct = this._mvtosAll.Select(x => x.RecursoID.Value.ToString()).Distinct().ToList();
                foreach (string rec in recursosDistinct)
                {
                    DTO_pyProyectoMvto det = this._mvtosAll.Find(x => x.RecursoID.Value == rec);
                    det.DetalleTareas = this._mvtosAll.FindAll(x => x.RecursoID.Value == rec).ToList();
                    foreach (var mvto in det.DetalleTareas)
                    {
                        //mvto.CantidadTOT.Value = mvto.FactorID.Value * mvto.CantidadTarea.Value;
                        mvto.CostoTotalML.Value = mvto.CantidadTarea.Value * mvto.CostoLocalTOT.Value;
                    }
                    det.CantidadSUM.Value = det.DetalleTareas.Sum(x => x.CantidadSOL.Value);
                    det.CostoTotalML.Value = det.DetalleTareas.Sum(x => x.CostoTotalML.Value);
                    this._mvtosSelected.Add(det);
                }


                this._mvtosSelected = this._mvtosSelected.OrderBy(x => x.RecursoDesc.Value).ToList();

                this.currentRow = -1;
                this.gcRecurso.DataSource = null;

                if (this._mvtosSelected.Count > 0)
                {
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this._mvtosSelected = this._mvtosSelected.OrderBy(x => x.RecursoDesc.Value).ToList();
                    this.gcRecurso.DataSource = this._mvtosSelected;
                    this.allowValidate = true;
                    this.gvRecurso.MoveFirst();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita", "LoadData"));
            }
        }

        /// <summary>
        /// Se ejectua despues de inicializar el formulario
        /// </summary>
        private  void AfterInitialize()
        {
            this.grpboxHeader.Visible = true;
            this.dtFecha.DateTime = DateTime.Today;
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true,true, true, false);
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento para validar el check de la columna aprobado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSeleccionar.Checked)
            {
               for (int i = 0; i < this.gvRecurso.DataRowCount; i++)
                   this._mvtosSelected[i].SelectInd.Value = true;
            }
            else
            {
               for (int i = 0; i < this.gvRecurso.DataRowCount; i++)
                   this._mvtosSelected[i].SelectInd.Value = false;
            }
            this.gcRecurso.RefreshDataSource();
        }

        /// <summary>
        /// Busca los recursos hasta la fecha indicada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadDocuments();
                this.chkSeleccionar.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita.cs", "btnFind_Click"));
            }
        }
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Enter(object sender, EventArgs e)
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
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemUpdate.Enabled = true;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    //FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentAprobBasicForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Grilla
        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            #region Generales
            if (fieldName == "Aprobado")
            {
                //if ((bool)e.Value)
                //    this._docs[e.RowHandle].Rechazado.Value = false;
            }           
            #endregion

            this.gcRecurso.RefreshDataSource();
            //this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
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
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == this.unboundPrefix + "TipoRecurso" && e.IsForGroupRow)
                {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita.cs", "gvRecurso_CustomColumnDisplayText"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            string fieldName = e.Column.FieldName.Length >= this.unboundPrefix.Length ? e.Column.FieldName.Substring(this.unboundPrefix.Length) : string.Empty;
            if (fieldName == "Observacion")
                e.RepositoryItem = this.richText1;

            if (fieldName == "FileUrl")
                e.RepositoryItem = this.editLink;
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "Observacion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita.cs", "gvRecurso_CustomRowCellEditForEditing"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            //if (this.currentRow != -1)
            //{
            //    if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
            //        e.Allow = false;
            //}
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void gvRecurso_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvRecurso.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;
            }
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void editLink_Click(object sender, EventArgs e) { }

        #endregion

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvRecurso.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "Observacion")
                this.richEditControl.Document.Text = this.gvRecurso.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private  void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvRecurso.PostEditor();
            FormProvider.Master.itemSave.Enabled = false;
            try
            {              

                if (this._mvtosSelected != null && this._mvtosSelected.Count(x=>x.SelectInd.Value.Value) > 0)
                {
                    foreach (DTO_pyProyectoMvto mvto in this._mvtosSelected.FindAll(x=>x.SelectInd.Value.Value))
                    {                       
                        this._mvtosAll.ForEach(m =>    
                        {
                            if(m.RecursoID.Value == mvto.RecursoID.Value)
                               m.SelectInd.Value = true;                           
                        });
                    }
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadDocuments();
                this.chkSeleccionar.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita.cs", "TBUpdate"));
            }
        }

        #endregion

        #region Hilos
        /// <summary>
        /// Hilo que se ejecuta al enviar a aprobacion
        /// </summary>
        protected void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_SerializedObject result = this._bc.AdministrationModel.CompraRecursos_ApproveSolicitudOC(this.documentID, this._mvtosAll.FindAll(x=>x.SelectInd.Value.Value).ToList());
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, string.Empty, result, true, true);
                if (isOK)
                {
                    //this.newDoc = true;
                    //this.deleteOP = true;
                    //this.data = new DTO_Convenios();
                    //this._listConvenio = new List<DTO_prConvenio>();
                    //this.terceroID = string.Empty;
                    //this.validHeader = false;
                    this.Invoke(this.refreshData);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionRecursosSolicita.cs", "ApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion      
    }
}
