using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.API.Word;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Threading;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using System.Globalization;
using SentenceTransformer;
using DevExpress.XtraGrid.Views.Grid;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Salida : DocumentForm
    {
        #region Variables Formulario

        //Obtiene la instancia del controlador
        private BaseController _bc = BaseController.GetInstance();
        //Variables de data
        private DTO_inCostosExistencias _costosMvto;
        private DTO_OrdenSalida ordenSalida = new DTO_OrdenSalida();
        private List<DTO_inControlSaldosCostos> _existenciasByBodega = new List<DTO_inControlSaldosCostos>();
        private List<DTO_pyProyectoDeta> recursosProyectoAll = new List<DTO_pyProyectoDeta>();
        private DTO_glDocumentoControl _docCtrlOrdenSalida = new DTO_glDocumentoControl();
        private DTO_pyProyectoDeta _rowRecursoCurrent = new DTO_pyProyectoDeta();
        private DTO_pyProyectoMvto _rowMvtoCurrent = new DTO_pyProyectoMvto();
        private DTO_inBodega _bodegaOrden = null;
        //Variables con valores x defecto (glControl) 
        protected string tipoMovDet = string.Empty;
        protected string tipoMovReval = string.Empty;
        private bool validHeader;
        private bool cleanDoc = true;
        private bool _copyData = false;
        private bool existOrden = false;
        #endregion
        
        #region Delegados
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.CleanData();
        } 
        #endregion

        public Salida()
        {
           //this.InitializeComponent();
        }

        #region Funciones Privadas

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._costosMvto = new DTO_inCostosExistencias();
            this.recursosProyectoAll = new List<DTO_pyProyectoDeta>();
            this._docCtrlOrdenSalida = new DTO_glDocumentoControl();
            this.ordenSalida = new DTO_OrdenSalida();
            this.cleanDoc = true;
            this.validHeader = false;

            this.masterBodega.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterCliente.Value = string.Empty;
            this.masterPrefijoProyecto.Value = string.Empty;
            this.txtNroProyecto.Text = "0";
            this.txtLicitacion.Text = string.Empty;
            this.txtDesc.Text = string.Empty;

            this.EnableHeader(true);

            this.gcDocument.DataSource = this.recursosProyectoAll;
            this.gcTarea.DataSource = null;
            this.gcTarea.RefreshDataSource();
            this.newDoc = true;
            this.masterBodega.Focus();
            this.btnQueryDoc.Enabled = true;
            this.existOrden = false;
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected void EnableHeader(bool enable)
        {
            this.btnQueryDoc.Enabled = enable;
            this.masterBodega.EnableControl(enable);
            this.masterProyecto.EnableControl(false);
            this.masterPrefijoProyecto.EnableControl(false);
            this.txtNroProyecto.Enabled = enable;            
        }

        /// <summary>
        /// valida la informacion del header
        /// </summary>
        private bool ValidateHeader()
        {
            bool result = true;
            if (!this.masterBodega.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterBodega.CodeRsx);

                MessageBox.Show(msg);
                this.masterBodega.Focus();

                result = false;
            }
            if (!this.masterProyecto.ValidID)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterProyecto.CodeRsx);

                MessageBox.Show(msg);
                this.masterProyecto.Focus();
            }  
            //    result = false;
            //}
            //if (!this.masterPrefijoOrdenTrabajo.ValidID)
            //{
            //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijoOrdenTrabajo.CodeRsx);

            //    MessageBox.Show(msg);
            //    this.masterPrefijoOrdenTrabajo.Focus();

            //    result = false;
            //}

            //if (string.IsNullOrEmpty(this.txtNroOrdenTrabajo.Text))
            //{
            //    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblNro.Text);

            //    MessageBox.Show(msg);
            //    this.txtNroOrdenTrabajo.Focus();

            //    result = false;
            //}   
            return result;
        }

        /// <summary>
        /// Agrega las columnas a la grilla principal del documento
        /// </summary>
        protected override void AddGridCols()
        {
            try
            {
                #region Grilla Referencias/Recursos
                #region Columnas visibles

                //RecursoID/ReferenciaID
                GridColumn RecursoID = new GridColumn();
                RecursoID.FieldName = this.unboundPrefix + "RecursoID";
                RecursoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_RecursoID");
                RecursoID.UnboundType = UnboundColumnType.String;
                RecursoID.VisibleIndex = 1;
                RecursoID.Width = 60;
                RecursoID.Visible = true;
                RecursoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RecursoID);

                //RecursoDesc
                GridColumn RecursoDesc = new GridColumn();
                RecursoDesc.FieldName = this.unboundPrefix + "RecursoDesc";
                RecursoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_RecursoDesc");
                RecursoDesc.UnboundType = UnboundColumnType.String;
                RecursoDesc.VisibleIndex = 2;
                RecursoDesc.Width = 200;
                RecursoDesc.Visible = true;
                RecursoDesc.OptionsColumn.AllowEdit = false;
                RecursoDesc.OptionsColumn.AllowFocus = false;
                this.gvDocument.Columns.Add(RecursoDesc);

                //UnidadInvID
                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_UnidadInvID");
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
                UnidadInvID.VisibleIndex = 3;
                UnidadInvID.Width = 50;
                UnidadInvID.Visible = true;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(UnidadInvID);

                //CantExistencias Inv
                GridColumn CantExistencias = new GridColumn();
                CantExistencias.FieldName = this.unboundPrefix + "CantExistencias";
                CantExistencias.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_CantExistencias");
                CantExistencias.UnboundType = UnboundColumnType.Integer;
                CantExistencias.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantExistencias.AppearanceCell.Options.UseTextOptions = true;
                CantExistencias.VisibleIndex = 5;
                CantExistencias.Width = 60;
                CantExistencias.Visible = true;
                CantExistencias.OptionsColumn.AllowEdit = false;
                CantExistencias.OptionsColumn.AllowFocus = false;
                CantExistencias.ColumnEdit = this.editValue;
                this.gvDocument.Columns.Add(CantExistencias);

                //Pendientes
                GridColumn CantidadPend = new GridColumn();
                CantidadPend.FieldName = this.unboundPrefix + "CantidadPend";
                CantidadPend.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_CantidadPend");
                CantidadPend.UnboundType = UnboundColumnType.Integer;
                CantidadPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadPend.AppearanceCell.Options.UseTextOptions = true;
                CantidadPend.VisibleIndex = 6;
                CantidadPend.Width = 10;
                CantidadPend.Visible = false;
                CantidadPend.OptionsColumn.AllowEdit = false;
                CantidadPend.OptionsColumn.AllowFocus = false;
                CantidadPend.ColumnEdit = this.editValue;
                this.gvDocument.Columns.Add(CantidadPend);

                //CantidadDespacho
                GridColumn CantDespacho = new GridColumn();
                CantDespacho.FieldName = this.unboundPrefix + "CantDespacho";
                CantDespacho.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_CantidadDespacho");
                CantDespacho.UnboundType = UnboundColumnType.Integer;
                CantDespacho.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantDespacho.AppearanceCell.Options.UseTextOptions = true;
                CantDespacho.VisibleIndex = 7;
                CantDespacho.Width = 60;
                CantDespacho.Visible = true;
                CantDespacho.OptionsColumn.AllowEdit = false;
                CantDespacho.ColumnEdit = this.editValue;
                this.gvDocument.Columns.Add(CantDespacho);

                #endregion
                #region Columnas No Visibles

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);

                #endregion
                this.gvDocument.OptionsView.ColumnAutoWidth = true; 
                #endregion
                #region Grilla Items Relacionados
                //TareaCliente
                GridColumn TareaCliente = new GridColumn();
                TareaCliente.FieldName = this.unboundPrefix + "TareaCliente";
                TareaCliente.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_TareaCliente");
                TareaCliente.UnboundType = UnboundColumnType.String;
                TareaCliente.VisibleIndex = 1;
                TareaCliente.Width = 60;
                TareaCliente.Visible = true;
                TareaCliente.OptionsColumn.AllowEdit = false;
                this.gvTarea.Columns.Add(TareaCliente);

                //TareaDesc
                GridColumn TareaDesc = new GridColumn();
                TareaDesc.FieldName = this.unboundPrefix + "TareaDesc";
                TareaDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_TareaDesc");
                TareaDesc.UnboundType = UnboundColumnType.String;
                TareaDesc.VisibleIndex = 2;
                TareaDesc.Width = 200;
                TareaDesc.Visible = true;
                TareaDesc.OptionsColumn.AllowEdit = false;
                TareaDesc.OptionsColumn.AllowFocus = false;
                this.gvTarea.Columns.Add(TareaDesc);

                //CantidadTOT
                GridColumn CantidadTOT = new GridColumn();
                CantidadTOT.FieldName = this.unboundPrefix + "CantidadTOT";
                CantidadTOT.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.PlaneacionRecursos + "_CantidadTOT");
                CantidadTOT.UnboundType = UnboundColumnType.Integer;
                CantidadTOT.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadTOT.AppearanceCell.Options.UseTextOptions = true;
                CantidadTOT.VisibleIndex = 3;
                CantidadTOT.Width = 10;
                CantidadTOT.Visible = true;
                CantidadTOT.OptionsColumn.AllowEdit = false;
                CantidadTOT.OptionsColumn.AllowFocus = false;
                CantidadTOT.ColumnEdit = this.editValue;
                this.gvTarea.Columns.Add(CantidadTOT);

                //CantidadPend
                GridColumn CantidadPendItem = new GridColumn();
                CantidadPendItem.FieldName = this.unboundPrefix + "CantidadPend";
                CantidadPendItem.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_CantidadPend");
                CantidadPendItem.UnboundType = UnboundColumnType.Integer;
                CantidadPendItem.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadPendItem.AppearanceCell.Options.UseTextOptions = true;
                CantidadPendItem.VisibleIndex = 6;
                CantidadPendItem.Width = 10;
                CantidadPendItem.Visible = true;
                CantidadPendItem.OptionsColumn.AllowEdit = false;
                CantidadPendItem.OptionsColumn.AllowFocus = false;
                CantidadPendItem.ColumnEdit = this.editValue;
                this.gvTarea.Columns.Add(CantidadPendItem);

                //CantidadDespacho
                GridColumn CantidadDespachoItem = new GridColumn();
                CantidadDespachoItem.FieldName = this.unboundPrefix + "CantidadDespacho";
                CantidadDespachoItem.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.OrdenDespacho + "_CantidadDespacho");
                CantidadDespachoItem.UnboundType = UnboundColumnType.Integer;
                CantidadDespachoItem.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadDespachoItem.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CantidadDespachoItem.AppearanceCell.Options.UseTextOptions = true;
                CantidadDespachoItem.AppearanceCell.Options.UseFont = true;
                CantidadDespachoItem.AppearanceCell.BackColor = Color.PeachPuff;
                CantidadDespachoItem.AppearanceCell.Options.UseBackColor = true;
                CantidadDespachoItem.VisibleIndex = 7;
                CantidadDespachoItem.Width = 60;
                CantidadDespachoItem.Visible = true;
                CantidadDespachoItem.OptionsColumn.AllowEdit = false;
                CantidadDespachoItem.ColumnEdit = this.editValue;
                this.gvTarea.Columns.Add(CantidadDespachoItem);
            
                this.gvTarea.OptionsView.ColumnAutoWidth = true;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Salida.cs-AddDocumentCols"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadGrid()
        {
            try
            {
                this.gcDocument.DataSource = this.recursosProyectoAll;
                this.gcDocument.RefreshDataSource();
                if (this.gvDocument.DataRowCount > 0)
                {
                    this._rowRecursoCurrent = (DTO_pyProyectoDeta)this.gvDocument.GetRow(this.gvDocument.FocusedRowHandle);
                    this.gcTarea.DataSource = this._rowRecursoCurrent.DetalleMvto;
                    this.gcTarea.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {                
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "Salida.cs-LoadGrid"));
            }
        }
        
        /// <summary>
        /// Crea las ordenes de salida para guardar
        /// </summary>
        private bool CreateOrdenSalida()
        { 
            bool isOK = true;
            try
            {
                #region Actualiza el detalle de la Orden de Salida
                foreach (var detaProy in this.recursosProyectoAll.FindAll(x => x.CantDespacho.Value > 0))
                {
                    foreach (var ord in this.ordenSalida.Footer)
                    {
                        if (detaProy.DetalleMvto.Exists(x => x.Consecutivo.Value == ord.ConsProyectoMvto.Value))
                            ord.CantidadAPR.Value = detaProy.DetalleMvto.Find(x => x.Consecutivo.Value == ord.ConsProyectoMvto.Value).CantidadDespacho.Value;
                    }
                }
                #endregion

                if (this.ordenSalida.Footer.Count == 0)
                    return false;
            }
            catch (Exception)
            {
                return isOK = false;
            }
            return isOK;
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Despacho;
            this.frmModule = ModulesPrefix.@in;

            InitializeComponent();
            base.SetInitParameters();

            this.AddGridCols();

        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            this.newDoc = true;
            //Modifica el tamaño de las secciones
            this.tlSeparatorPanel.RowStyles[0].Height = 135;
            this.tlSeparatorPanel.RowStyles[1].Height = 140;
            this.tlSeparatorPanel.RowStyles[2].Height = 200;
            this.editValue.Mask.EditMask = "n2";
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[1].Visible = false;

            List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
            filtros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "OrdenSalidaInd",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "1"
            });

            //Carga controles del Header
            this._bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, true, true, filtros);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoProyecto, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterCliente, AppMasters.faCliente, true, true, true, false);

            this.masterProyecto.EnableControl(false);
            this.masterPrefijoProyecto.EnableControl(false);

            this.masterBodega.Focus();
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected override void LoadData(bool byProyectoID)
        {
            DTO_SolicitudTrabajo proyecto = null;
            bool proyectoExist = false;
            if (byProyectoID)  // Por proyectoID      
                proyecto = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, string.Empty, null, null, string.Empty, this.masterProyecto.Value, false,true,false,false);
            else   //Por Prefijo-Nro
                proyecto = this._bc.AdministrationModel.SolicitudProyecto_Load(AppDocuments.Proyecto, this.masterPrefijoProyecto.Value, Convert.ToInt32(this.txtNroProyecto.Text), null, string.Empty, string.Empty, false, true, false, false);

            if (proyecto != null && proyecto.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado)
            {
                this.masterPrefijoProyecto.Value = proyecto.DocCtrl.PrefijoID.Value;
                this.txtNroProyecto.Text = proyecto.DocCtrl.DocumentoNro.Value.ToString();
                this.masterProyecto.Value = proyecto.DocCtrl.ProyectoID.Value;
                this.masterCliente.Value = proyecto.HeaderProyecto.ClienteID.Value;
                this.txtLicitacion.Text = proyecto.HeaderProyecto.Licitacion.Value;
                this.txtDesc.Text = proyecto.HeaderProyecto.DescripcionSOL.Value;
                proyectoExist = true;
            }
            else
                MessageBox.Show("Este proyecto no es válido y/o la orden de despacho ya fue aprobada");

            if (proyectoExist)
            {
                //Recorre los recursos Disponibles
                foreach (var rec in this.recursosProyectoAll)
                {
                    //Obtiene los movimientos de Inventarios(glMovimientoDeta)
                    DTO_glMovimientoDeta mvtoInvFilter = new DTO_glMovimientoDeta();
                    mvtoInvFilter.inReferenciaID.Value = rec.RecursoID.Value;
                    mvtoInvFilter.BodegaID.Value = this.masterBodega.Value;
                    mvtoInvFilter.ProyectoID.Value = proyecto.DocCtrl.ProyectoID.Value;
                    mvtoInvFilter.EntradaSalida.Value = (byte)EntradaSalida.Entrada;
                    var detFacturas = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(mvtoInvFilter,false);
                    rec.DetalleMvto = new List<DTO_pyProyectoMvto>();
                    foreach (DTO_glMovimientoDeta d in detFacturas)
                    {
                        //Valida y Obtiene los movimientos del Proyecto(pyProyectoMvto)
                        if (!rec.DetalleMvto.Exists(x => x.TareaCliente.Value == (proyecto.Movimientos.Find(y => y.Consecutivo.Value == d.DocSoporte.Value).TareaCliente.Value)))
                        {
                            rec.DetalleMvto.AddRange(proyecto.Movimientos.FindAll(x => x.Consecutivo.Value == d.DocSoporte.Value));
                            foreach (var mvto in rec.DetalleMvto)
                            {
                                mvto.CantidadDespacho.Value = 0;
                                if (this.existOrden)
                                {
                                    //Obtiene las cantidades de despacho creadas
                                    if (this.ordenSalida.Footer.Exists(x => x.ConsProyectoMvto.Value == mvto.Consecutivo.Value))
                                        mvto.CantidadDespacho.Value = this.ordenSalida.Footer.Find(x => x.ConsProyectoMvto.Value == mvto.Consecutivo.Value).CantidadAPR.Value;
                                }
                                mvto.CantidadPend.Value = mvto.CantidadREC.Value - mvto.CantidadINV.Value - mvto.CantidadBOD.Value - mvto.CantidadDespacho.Value;
                            }
                        }
                    }
                    rec.CantDespacho.Value = rec.DetalleMvto.Sum(x => x.CantidadDespacho.Value);
                }
                this.recursosProyectoAll = this.recursosProyectoAll.FindAll(x => x.CantDespacho.Value > 0).ToList();
            }
            this.LoadGrid();
        }       

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);
           
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemSave.Visible = false;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
            }
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNroOrdenTrab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el documento de transporte
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtNroOrdenTrab_Leave(object sender, EventArgs e)
        {
        
        }

        /// <summary>
        /// Valida que el campo Fk al salir
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void master_Leave(object sender, EventArgs e)
        {
            try
            {
                ControlsUC.uc_MasterFind master = (ControlsUC.uc_MasterFind)sender;
                switch (master.ColId)
                {
                    case "BodegaID" :
                        if (master.ValidID && this.gvDocument.DataRowCount == 0)
	                     {
                             #region Trae los inventarios por bodega
                             DTO_inControlSaldosCostos filter = new DTO_inControlSaldosCostos();
                             this.recursosProyectoAll = new List<DTO_pyProyectoDeta>();
                             filter.BodegaID.Value = master.Value;
                             this._existenciasByBodega = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(this.documentID, filter);
                             this._bodegaOrden = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, master.Value, true);
                             foreach (var saldo in this._existenciasByBodega)
                             {
                                 DTO_pyProyectoDeta deta = new DTO_pyProyectoDeta();
                                 deta.RecursoID.Value = saldo.inReferenciaID.Value;
                                 deta.RecursoDesc.Value = saldo.ReferenciaP1P2Desc.Value;
                                 deta.UnidadInvID.Value = saldo.UnidadInvID.Value;
                                 deta.CantExistencias.Value = saldo.CantidadDisp.Value;
                                 deta.CantDespacho.Value = 0;
                                 this.recursosProyectoAll.Add(deta);
                             }
                             this.masterBodega.EnableControl(false); 
                             #endregion

                            DTO_OrdenSalida exist = this._bc.AdministrationModel.OrdenSalida_GeyByBodega(master.Value, null);
                            if (exist != null && exist.Header.DocSalidaINV.Value != null)
                                exist = null;
                            if (exist != null)
                            {
                                if(exist.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    MessageBox.Show("El estado de la Orden de Despacho aún no está Aprobado");
                                    return;
                                }
                                this.ordenSalida = exist;
                                this.existOrden = true;
                                this.masterProyecto.Value = exist.DocCtrl.ProyectoID.Value;
                                this.LoadData(true);
                            }
                            else
                            {
                                this.existOrden = false;
                                MessageBox.Show("No se encuentra ninguna Orden de Despacho actual");
                            }
                        }
                        break;                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-OrdenLegalizacion.cs", "master_Leave: " + ex.Message));
                this.txtNroProyecto.Focus();
            }
        }

        /// <summary>
        /// Se encargar de buscar un documento 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDocQuery_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            docs.Add(AppDocuments.ActaTrabajo);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs, false, false);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                this.txtNroProyecto.Enabled = true;
                this.txtNroProyecto.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                this.masterPrefijoProyecto.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                this.txtNroProyecto.Focus();
                this.btnQueryDoc.Focus();
                this.btnQueryDoc.Enabled = false;
            }
        }

        #endregion

        #region Eventos Grilla (Recursos)

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.ValidateHeader())
                    this.validHeader = true;
                else
                    this.validHeader = false;
                #region Si entra al detalle y no tiene datos
              
                try
                {
                    if (this.validHeader &&  this.recursosProyectoAll.Count > 0)
                    {                      
                        this.LoadGrid();
                        this.EnableHeader(false);
                        this.validHeader = true;
                        this.ValidHeaderTB();
                        this.newDoc = false;
                    }                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Salida.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion                
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "Salida.cs-gcDocument_Enter: " + ex.Message));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
                            DTO_pyProyectoDeta dtoM = (DTO_pyProyectoDeta)e.Row;
                            pi = dtoM.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM, null), null);
                            }
                            else
                            {
                                fi = dtoM.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                                        e.Value = fi.GetValue(dtoM);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM), null);
                                }
                            }
                        }
                    }
                }
            }

            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
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
                        else
                        {
                            DTO_pyProyectoDeta dtoM = (DTO_pyProyectoDeta)e.Row;
                            pi = dtoM.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    e.Value = pi.GetValue(dtoM, null);
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoM, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoM.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                    {
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoM);
                                        udtProp.SetValueFromString(e.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) 
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            try
            {
                if (fieldName == "inReferenciaID")
                {
                    this._costosMvto = new DTO_inCostosExistencias();
                    DTO_inControlSaldosCostos saldos = new DTO_inControlSaldosCostos();
                    DTO_inMovimientoFooter footer = new DTO_inMovimientoFooter();
                    saldos.Periodo.Value = this.dtPeriod.DateTime;
                    //saldos.EstadoInv.Value = Convert.ToByte(this.cmbEstado.EditValue);
                    saldos.CosteoGrupoInvID.Value = this.masterBodega.Value;
                    saldos.inReferenciaID.Value = e.Value.ToString();
                    var rrr = this._bc.AdministrationModel.inControlSaldosCostos_GetByParameter(documentID, saldos);
                    decimal saldoDispInit = this._bc.AdministrationModel.Transaccion_SaldoExistByReferencia(documentID, saldos, ref this._costosMvto);
                    if (saldoDispInit != 0)
                    {
                        //this.footerServicioDetaSelected[e.RowHandle].Movimiento.CantidadUNI.Value = saldoDispInit;
                        //this.footerServicioDetaSelected[e.RowHandle].Movimiento.ValorActualUniLOC.Value = saldoDispInit != 0 ? (this._costosMvto.CtoLocSaldoIni.Value + this._costosMvto.CtoLocEntrada.Value - this._costosMvto.CtoLocSalida.Value) / saldoDispInit : 0;
                        //this.footerServicioDetaSelected[e.RowHandle].Movimiento.ValorActualUniEXT.Value = saldoDispInit != 0 ? (this._costosMvto.CtoExtSaldoIni.Value + this._costosMvto.CtoExtEntrada.Value - this._costosMvto.CtoExtSalida.Value) / saldoDispInit : 0;
                        //footer.Movimiento = this.footerServicioDetaSelected[e.RowHandle].Movimiento;
                    }
                    else
                    {
                        MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_CantityAvailable), 0));
                        this.validHeader = false;
                        return;
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp", "Salida.cs-gvDocument_CellValueChanged: " + ex.Message));
            }
        }

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void gvDocument_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                    this._rowRecursoCurrent = (DTO_pyProyectoDeta)this.gvDocument.GetRow(e.FocusedRowHandle);
                    this.gcTarea.DataSource = this._rowRecursoCurrent.DetalleMvto;
                    this.gcTarea.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Salida.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        ///Al hacer clic sobre cada fila
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvDocument_RowClick(object sender, RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    this._rowRecursoCurrent = (DTO_pyProyectoDeta)this.gvDocument.GetRow(e.RowHandle);
                    this.gcTarea.DataSource = this._rowRecursoCurrent.DetalleMvto;
                    this.gcTarea.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Salida.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        #endregion

        #region Eventos Grilla Detalle(Items)

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                if (fieldName == "Marca" && e.Value == null)
                    e.Value = this.select.Contains(e.ListSourceRowIndex);
                else
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
            }
            if (e.IsSetData)
            {
                if (fieldName == "Marca")
                {
                    bool value = Convert.ToBoolean(e.Value);
                    if (value)
                        this.select.Add(e.ListSourceRowIndex);
                    else
                        this.select.Remove(e.ListSourceRowIndex);
                }
                else
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
        }

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvTarea_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras.cs", "gvDocument_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvTarea_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                GridColumn col = this.gvTarea.Columns[this.unboundPrefix + fieldName];
                this._rowMvtoCurrent = (DTO_pyProyectoMvto)this.gvTarea.GetRow(e.RowHandle);
                if (fieldName == "CantidadDespacho")
                {
                    decimal sol = Convert.ToDecimal(e.Value);
                    if ((this._rowMvtoCurrent.CantidadREC.Value - this._rowMvtoCurrent.CantidadINV.Value - this._rowMvtoCurrent.CantidadBOD.Value) >= sol)
                    {
                        this._rowMvtoCurrent.CantidadPend.Value = this._rowMvtoCurrent.CantidadREC.Value - this._rowMvtoCurrent.CantidadINV.Value - this._rowMvtoCurrent.CantidadBOD.Value - sol;
                        this._rowRecursoCurrent.CantDespacho.Value = this._rowRecursoCurrent.DetalleMvto.Sum(x => x.CantidadDespacho.Value);
                        this.gvTarea.ClearColumnErrors();
                    }
                    else
                        this.gvTarea.SetColumnError(col, "La cantidad solicitada no puede exceder a la disponible");
                }
                this.gvTarea.RefreshData();
                this.gvDocument.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PlaneacionCompras.cs", "gvDocument_CellValueChanged"));
            }
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
                base.TBNew();
                if (this.cleanDoc)
                    this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Salida.cs", "TBNew: " + ex.Message));

            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion
        /// </summary>
        public override void TBSendtoAppr()
        {

            this.gvDocument.PostEditor();
            this.gvDocument.Focus();
            this.gvDocument.ActiveFilterString = string.Empty;
            if (this.ValidateHeader() && this.CreateOrdenSalida() && this.recursosProyectoAll.Count > 0)
            {
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoInventarios(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject obj = this._bc.AdministrationModel.OrdenSalida_ApproveMvtoInv(this.documentID, this.ordenSalida);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, obj, true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Salida.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
      
    }
}
