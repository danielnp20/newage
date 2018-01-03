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
using SentenceTransformer;
using System.Globalization;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class PreFacturaPagoAnticipado : FormWithToolbar
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
        //Variables del Proyecto
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_pyProyectoDocu _headerProy = null;
        private List<DTO_pyProyectoTareaCliente> _listTareasActa = new List<DTO_pyProyectoTareaCliente>();
        private List<DTO_faFacturacionFooter> _listDetalleFact = new List<DTO_faFacturacionFooter>();
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTareaCliente> _listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();       
        private List<DTO_pyActaEntregaDeta> _listActaDeta = new List<DTO_pyActaEntregaDeta>();
        private List<DTO_pyActaEntregaDeta> _listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
        //Variables de facturas
        private DTO_faFacturacion _factura = null;
        private DTO_coTercero _regFiscalEmp = new DTO_coTercero();
        private string _refFiscalTercero = string.Empty;
        private string tipoFacturaCtaCobro = string.Empty;
        private string tipoFacturaProyecto = string.Empty;
        private string servicioCtaCobro = string.Empty;
        private string servicioxDefecto = string.Empty;
        private string prefijoCtaCobro = string.Empty;
        private string moneda = string.Empty;
        //Valores del proyecto
        private decimal vlrProyectoTotal = 0;
        private decimal vlrProyectoTotalconIVA = 0;
        private decimal vlrEjecutadoProy = 0;
        private decimal porcEntregado = 0;      
        #endregion

        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected virtual void SaveMethod() { this.RefreshForm(); }
        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public PreFacturaPagoAnticipado()
        {
            try
            {
                this.SetInitParameters();
                this.Text = "Prefactura Directa";
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado.cs", "PreFacturaPagoAnticipado"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {            
            #region Columnas Footer Fact
            GridColumn NroItem = new GridColumn();
            NroItem.FieldName = this.unboundPrefix + "NroItem";
            NroItem.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.FacturaVenta + "_NroItem") + " Fact";
            NroItem.UnboundType = UnboundColumnType.Integer;
            NroItem.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            NroItem.AppearanceCell.Options.UseTextOptions = true;
            NroItem.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NroItem.AppearanceCell.Options.UseFont = true;
            NroItem.VisibleIndex = 0;
            NroItem.Width = 25;
            NroItem.Visible = true;
            NroItem.OptionsColumn.AllowEdit = true;
            NroItem.ToolTip = "Si desea agrupar items asocielos con el mismo consecutivo";
            this.gvFooterFact.Columns.Add(NroItem);

            GridColumn TareaID = new GridColumn();
            TareaID.FieldName = this.unboundPrefix + "TareaID";
            TareaID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_TareaID");
            TareaID.UnboundType = UnboundColumnType.String;
            TareaID.AppearanceCell.Options.UseTextOptions = true;
            TareaID.VisibleIndex = 1;
            TareaID.Width = 30;
            TareaID.Visible = true;
            TareaID.OptionsColumn.AllowEdit = false;
            this.gvFooterFact.Columns.Add(TareaID);

            GridColumn DescriptivoTarea = new GridColumn();
            DescriptivoTarea.FieldName = this.unboundPrefix + "DescriptivoTarea";
            DescriptivoTarea.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ActaEntrega + "_DescriptivoTarea");
            DescriptivoTarea.UnboundType = UnboundColumnType.String;
            DescriptivoTarea.VisibleIndex = 2;
            DescriptivoTarea.Width = 150;
            DescriptivoTarea.Visible = true;
            DescriptivoTarea.OptionsColumn.AllowEdit = false;
            this.gvFooterFact.Columns.Add(DescriptivoTarea);

            GridColumn DescripTExtFact = new GridColumn();
            DescripTExtFact.FieldName = this.unboundPrefix + "DescripTExt";
            DescripTExtFact.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.ActaEntrega + "_DescripTExt");
            DescripTExtFact.UnboundType = UnboundColumnType.String;
            DescripTExtFact.VisibleIndex = 2;
            DescripTExtFact.Width = 200;
            DescripTExtFact.Visible = true;
            DescripTExtFact.OptionsColumn.AllowEdit = true;
            this.gvFooterFact.Columns.Add(DescripTExtFact);

            GridColumn LineaPresupuestoID = new GridColumn();
            LineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
            LineaPresupuestoID.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.FacturaVenta + "_LineaPresupuestoID");
            LineaPresupuestoID.UnboundType = UnboundColumnType.String;
            LineaPresupuestoID.VisibleIndex = 2;
            LineaPresupuestoID.Width = 50;
            LineaPresupuestoID.Visible = true;
            LineaPresupuestoID.OptionsColumn.AllowEdit = false;
            this.gvFooterFact.Columns.Add(LineaPresupuestoID);

            GridColumn CantidadFact = new GridColumn();
            CantidadFact.FieldName = this.unboundPrefix + "CantidadUNI";
            CantidadFact.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.FacturaVenta + "_CantidadUNI");
            CantidadFact.UnboundType = UnboundColumnType.String;
            CantidadFact.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            CantidadFact.AppearanceCell.Options.UseTextOptions = true;
            CantidadFact.VisibleIndex = 2;
            CantidadFact.Width = 100;
            CantidadFact.Visible = true;
            CantidadFact.ColumnEdit = this.editValue2Cant;
            CantidadFact.OptionsColumn.AllowEdit = false;
            this.gvFooterFact.Columns.Add(CantidadFact);

            GridColumn Valor1LOC = new GridColumn();
            Valor1LOC.FieldName = this.unboundPrefix + "Valor1LOC";
            Valor1LOC.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.FacturaVenta + "_Valor1LOC");
            Valor1LOC.UnboundType = UnboundColumnType.String;
            Valor1LOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Valor1LOC.AppearanceCell.Options.UseTextOptions = true;
            Valor1LOC.VisibleIndex = 3;
            Valor1LOC.Width = 100;
            Valor1LOC.Visible = true;
            Valor1LOC.ColumnEdit = this.editValue2;
            Valor1LOC.OptionsColumn.AllowEdit = false;
            this.gvFooterFact.Columns.Add(Valor1LOC);
            #endregion                       
        }

        /// <summary>
        /// Inicializar controles
        /// </summary>
        private void InitControls()
        {
            try
            {
                this.ucProyecto.Init(false, false, false, false);
                this.ucProyecto.LoadProyectoInfo_Leave += new UC_Proyecto.EventHandler(this.ucProyecto_LoadProyectoInfo_Click);
                this._bc.InitMasterUC(this.masterMoneda, AppMasters.glMoneda, true, true, false, true);
                this.dtFechaDoc.DateTime = DateTime.Today;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Carga la información
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadActas()
        {
            this._listTareasActa = new List<DTO_pyProyectoTareaCliente>();

            #region Obtiene las actas existentes del proyecto
            DTO_pyActaEntregaDeta det = new DTO_pyActaEntregaDeta();
            det.NumDocProyecto.Value = this._numeroDocProy;            
            this._listActaDetaExist = this._bc.AdministrationModel.pyActaEntregaDeta_GetByParameter(det);
            this._listActaDetaExist = this._listActaDetaExist.FindAll(x => x.NumDocFactura.Value == null && x.ValorFactura.Value != 0 && x.Estado.Value == (Int16)EstadoDocControl.Aprobado).ToList();
            List<int?> consTareasEntr = this._listActaDetaExist.Select(x => x.ConsTareaCliente.Value).Distinct().ToList();
            foreach (int? tarea in consTareasEntr)
            {
                DTO_pyProyectoTareaCliente footerDet = this._listEntregablesProy.Find(x => x.Consecutivo.Value == tarea);
                footerDet.TareaEntregable.Value = this._listActaDetaExist.Find(x => x.ConsTareaCliente.Value == tarea).TareaEntregable.Value;
                footerDet.Descripcion.Value = this._listActaDetaExist.Find(x => x.ConsTareaCliente.Value == tarea).Descripcion.Value;
                footerDet.NumeroDocActa.Value = this._listActaDetaExist.Find(x => x.ConsTareaCliente.Value == tarea).NumeroDoc.Value;
                footerDet.ValorAEntregar.Value = this._listActaDetaExist.FindAll(x => x.ConsTareaCliente.Value == tarea).Sum(y => y.ValorFactura.Value);
                footerDet.Cantidad.Value = this._listActaDetaExist.FindAll(x => x.ConsTareaCliente.Value == tarea).Sum(y => y.Cantidad.Value);
                footerDet.DetalleActas = this._listActaDetaExist.FindAll(x => x.ConsTareaCliente.Value == tarea);
                this._listTareasActa.Add(footerDet);
            }
            #endregion                   

            this.LoadGrids(false);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadEntregables()
        {
            try
            {
                List<DTO_pyProyectoTareaCliente> _listEntregablesExist = ObjectCopier.Clone(this._listEntregablesProy);
                this._listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();

                if (_listEntregablesExist.Count == 0)
                {
                    DTO_pyProyectoTareaCliente entr = new DTO_pyProyectoTareaCliente();
                    entr.NumeroDoc.Value = this._numeroDocProy;
                    entr.Descripcion.Value = this.txtDescripcion.Text;
                    entr.TareaEntregable.Value = "1";
                    //entr.ServicioID.Value = this.masterServicio.Value;
                    entr.ServicioDesc.Value = this.txtDescripcion.Text;
                    entr.VlrTotalTareas.Value = this._listTareasAll.Sum(x => x.CostoLocalCLI.Value);
                    entr.MonedaFactura.Value = this._ctrlProyecto.MonedaID.Value;
                    this._listEntregablesProy.Add(entr);

                    foreach (DTO_pyProyectoTarea tarProy in _listTareasAll)
                    {
                        tarProy.TareaEntregable.Value = entr.TareaEntregable.Value;
                    }

                    //Programacion
                    //foreach (DTO_pyProyectoTareaCliente entr in this._listEntregablesProy)
                    //{
                    //    DTO_pyProyectoPlanEntrega planEntr = new DTO_pyProyectoPlanEntrega();
                    //    planEntr.Index = 1;
                    //    planEntr.FacturaInd.Value = true;
                    //    planEntr.PorEntrega.Value = 100;
                    //    planEntr.TipoEntrega.Value = 1;
                    //    planEntr.FechaEntrega.Value = entr.DetalleTareas.Count > 0 ? entr.DetalleTareas.Last().FechaFin.Value : DateTime.Now;
                    //    planEntr.ValorFactura.Value = entr.ValorAEntregar.Value;
                    //    planEntr.TareaEntregable.Value = this._rowTareaClienteCurrent.TareaEntregable.Value;
                    //    planEntr.Cantidad.Value = entr.DetalleTareas.Sum(x => x.Cantidad.Value);
                    //    entr.ValorFactura.Value = planEntr.ValorFactura.Value;
                    //    entr.Detalle.Add(planEntr);
                    //}

                    //Actas
                    #region Carga DocumentoControl
                    //this._ctrlActa = new DTO_glDocumentoControl();
                    //this._ctrlActa.DocumentoID.Value = AppDocuments.ActaEntrega;
                    //this._ctrlActa.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    //this._ctrlActa.MonedaID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.MonedaID.Value;
                    //this._ctrlActa.ProyectoID.Value = this.ucProyecto.ProyectoID;
                    //this._ctrlActa.Fecha.Value = DateTime.Now;
                    //this._ctrlActa.PeriodoDoc.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.PrefijoID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.PrefijoID.Value;
                    //this._ctrlActa.TasaCambioCONT.Value = 0;
                    //this._ctrlActa.TasaCambioDOCU.Value = 0;
                    //this._ctrlActa.DocumentoNro.Value = 0;
                    //this._ctrlActa.PeriodoUltMov.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.seUsuarioID.Value = this.userID;
                    //this._ctrlActa.AreaFuncionalID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.AreaFuncionalID.Value;
                    //this._ctrlActa.ConsSaldo.Value = 0;
                    //this._ctrlActa.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    //this._ctrlActa.Observacion.Value = this.txtObservacion.Text;
                    //this._ctrlActa.FechaDoc.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.Descripcion.Value = "Acta de Entrega Proyecto";
                    //this._ctrlActa.DocumentoTercero.Value = this.txtDocTercero.Text;
                    //this._ctrlActa.Valor.Value = 0;
                    //this._ctrlActa.Iva.Value = 0;
                    #endregion
                    foreach (DTO_pyProyectoTareaCliente tarea in this._listEntregablesProy)
                    {
                        foreach (DTO_pyActaEntregaDeta det in tarea.DetalleActas)
                        {
                            det.EntregaFinalInd.Value = det.PorPendiente.Value == 0 ? true : false;
                            det.PorEntregado.Value = det.PorAEntregar.Value;
                            det.Cantidad.Value = det.CantAEntregar.Value;
                            det.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                        }
                    }

                }
                else
                {
                    #region Carga DocumentoControl Acta
                    //this._ctrlActa = new DTO_glDocumentoControl();
                    //this._ctrlActa.DocumentoID.Value = AppDocuments.ActaEntrega;
                    //this._ctrlActa.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    //this._ctrlActa.MonedaID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.MonedaID.Value;
                    //this._ctrlActa.ProyectoID.Value = this.ucProyecto.ProyectoID;
                    //this._ctrlActa.Fecha.Value = DateTime.Now;
                    //this._ctrlActa.PeriodoDoc.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.PrefijoID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.PrefijoID.Value;
                    //this._ctrlActa.TasaCambioCONT.Value = 0;
                    //this._ctrlActa.TasaCambioDOCU.Value = 0;
                    //this._ctrlActa.DocumentoNro.Value = 0;
                    //this._ctrlActa.PeriodoUltMov.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.seUsuarioID.Value = this.userID;
                    //this._ctrlActa.AreaFuncionalID.Value = this.ucProyecto.ProyectoInfo.DocCtrl.AreaFuncionalID.Value;
                    //this._ctrlActa.ConsSaldo.Value = 0;
                    //this._ctrlActa.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    //this._ctrlActa.Observacion.Value = this.txtObservacion.Text;
                    //this._ctrlActa.FechaDoc.Value = this.dtFechaActa.DateTime;
                    //this._ctrlActa.Descripcion.Value = "Acta de Entrega Proyecto";
                    //this._ctrlActa.DocumentoTercero.Value = this.txtDocTercero.Text;
                    //this._ctrlActa.Valor.Value = 0;
                    //this._ctrlActa.Iva.Value = 0;
                    #endregion
                    foreach (DTO_pyProyectoTareaCliente entr in _listEntregablesExist)
                    {
                        if (entr.DetalleTareas.Count > 1) //valida que pueda cambiar al descripcion
                            entr.Descripcion.Value = this.txtDescripcion.Text;
                        foreach (DTO_pyActaEntregaDeta det in entr.DetalleActas)
                        {
                            det.EntregaFinalInd.Value = det.PorPendiente.Value == 0 ? true : false;
                            det.PorEntregado.Value = det.PorAEntregar.Value;
                            det.Cantidad.Value = det.CantAEntregar.Value;
                            det.UsuarioID.Value = this._bc.AdministrationModel.User.ID.Value;
                        }
                    }

                    //foreach (DTO_pyProyectoTarea tarProy in _listTareasAll)
                    //{
                    //    DTO_pyProyectoTareaCliente entr = new DTO_pyProyectoTareaCliente();
                    //    DTO_pyTarea tarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, tarProy.TareaID.Value, true);
                    //    if (tarea != null && tarea.TipoTarea.Value == 1)
                    //    {
                    //        entr.NumeroDoc.Value = this._numeroDocProy;
                    //        entr.Descripcion.Value = tarProy.Descriptivo.Value;
                    //        entr.TareaEntregable.Value = (this._listEntregablesProy.Count + 1).ToString();
                    //        entr.ServicioID.Value = tarea.ServicioID.Value;
                    //        entr.ServicioDesc.Value = tarea.ServicioDesc.Value;
                    //        if (tarea.EntregaIndividualInd.Value.Value)
                    //            entr.Cantidad.Value = tarProy.Cantidad.Value;
                    //        else
                    //            entr.Cantidad.Value = 0;
                    //        entr.VlrTotalTareas.Value = tarProy.CostoLocalCLI.Value;
                    //        entr.MonedaFactura.Value = this._ctrlProyecto.MonedaID.Value;
                    //        if (_listEntregablesExist.Exists(x => x.Descripcion.Value == entr.Descripcion.Value && x.TareaEntregable.Value == entr.TareaEntregable.Value))
                    //            entr.Consecutivo.Value = _listEntregablesExist.Find(x => x.Descripcion.Value == entr.Descripcion.Value && x.TareaEntregable.Value == entr.TareaEntregable.Value).Consecutivo.Value;
                    //        this._listEntregablesProy.Add(entr);
                    //        //Asigna el entregable
                    //        tarProy.TareaEntregable.Value = entr.TareaEntregable.Value;
                    //    }
                    //}
                }                

                


            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "LoadEntregables"));
            }
        
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids(bool loadFooter)
        {
           
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        { 
            this._ctrlProyecto = null;
            this._numeroDocProy = 0;
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();
            this._listTareasActa = new  List<DTO_pyProyectoTareaCliente>();
            this._listActaDeta = new  List<DTO_pyActaEntregaDeta>();
            this._listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
            this._listDetalleFact = new List<DTO_faFacturacionFooter>();
            this._factura = null;
            this.vlrProyectoTotal = 0;
            this.vlrProyectoTotalconIVA = 0;
            this.vlrEjecutadoProy = 0;
            this.porcEntregado = 0;
            this.ucProyecto.CleanControl();
            this.pnDetalle.Enabled = false;
            this.txtObservacion.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtVlrFacturaActual.EditValue = 0;
            this.txtPorcFacturaActual.EditValue = 0;

            this.txtPorcFacturado.EditValue = 0;
            this.txtTotalFacturado.EditValue = 0;
            this.txtTotalProyecto.EditValue = 0;
            this.txtVlrIVAProyecto.EditValue = 0;
            this.txtVlrIVAFacturado.EditValue = 0;
            this.txtVlrFacturado.EditValue = 0;
            this.txtVlrProyecto.EditValue = 0;
            this.txtVlrPendiente.EditValue = 0;
            this.txtVlrAnticipos.EditValue = 0;
            this.txtVlrAmortizacion.EditValue = 0;
        }      

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected bool LoadFactura()
        {
            try
            {
                #region Validaciones
                if (string.IsNullOrEmpty(this.txtDescripcion.Text))
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col),this.lblDescripcion.Text));
                    return false;
                }               
                else if (Convert.ToDecimal(this.txtVlrFacturaActual.EditValue) <= 0)
                {
                    MessageBox.Show(string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), this.lblValorFacturaActual.Text));
                    return false;
                }

                DTO_coDocumento coDocumento = null;
                string tipoFactura = this.rbtTipo.SelectedIndex == 0 ? this.tipoFacturaProyecto : this.tipoFacturaCtaCobro;
                string servicio = this.rbtTipo.SelectedIndex == 0 ? this.servicioxDefecto : this.servicioCtaCobro;
                string cuenta = string.Empty;
                DTO_faFacturaTipo dtoFacturaTipo = (DTO_faFacturaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, tipoFactura, true);
                if (dtoFacturaTipo != null)
                {
                    coDocumento = (DTO_coDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, dtoFacturaTipo.coDocumentoID.Value, true);
                    if (coDocumento == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, "No existe el coDocumento en la clase de Proyecto"));
                        return false;
                    }
                    else
                    {
                        #region Valida la Cuenta
                        cuenta = this.masterMoneda.Value == this.moneda ? coDocumento.CuentaLOC.Value : coDocumento.CuentaEXT.Value;
                        DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, cuenta, true);
                        if (dtoCuenta != null)
                        {
                            DTO_glConceptoSaldo concSaldoDoc = (DTO_glConceptoSaldo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, dtoCuenta.ConceptoSaldoID.Value, true);
                            if (concSaldoDoc.coSaldoControl.Value.Value != (int)SaldoControl.Doc_Interno)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidCuentaTipoFact));
                                return false;
                            }
                            else if (coDocumento.DocumentoID.Value != AppDocuments.FacturaVenta.ToString())
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_InvalidDocFact));
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_NocoContCta) + " : " + coDocumento.ID.Value);
                            return false;
                        }
                        #endregion
                    }
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, "No existe o no es válido el tipo de factura " + tipoFactura));
                    return false;
                }
                //Valida el Servicio
                DTO_faServicios serv = (DTO_faServicios)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, servicio, true);
                if (serv == null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, "No existe o no es válido el servicio " + servicio));
                    return false;
                }
                //Valida el prefijo de la Cta Cobro
                if (this.rbtTipo.SelectedIndex == 1)
                {
                    DTO_MasterBasic pref = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glPrefijo, false, this.prefijoCtaCobro, true);
                    if (pref == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Error, "No existe o no es válido el prefijo de la Cuenta de Cobro - " + this.prefijoCtaCobro));
                        return false;
                    }
                }
                
                #endregion
                #region Variables
                DTO_faCliente cliente = (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this._headerProy.ClienteID.Value, true);             
                string asesorxDef = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_AsesorPorDefecto);
                string zonaxDef = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ZonaxDefecto);
                string comprobanteID = coDocumento != null ? coDocumento.ComprobanteID.Value : string.Empty;
                decimal porcFacturaActual = Convert.ToDecimal(this.txtPorcFacturaActual.EditValue, CultureInfo.InvariantCulture);
                #endregion
                this._factura = new DTO_faFacturacion();                
                #region Carga DocumentoControl
                this._factura.DocCtrl.PrefijoID.Value = this.rbtTipo.SelectedIndex == 0 ? this._ctrlProyecto.PrefijoID.Value : this.prefijoCtaCobro;
                this._factura.DocCtrl.EmpresaID.Value = this.empresaID;
                this._factura.DocCtrl.TerceroID.Value = this._ctrlProyecto.TerceroID.Value;
                this._factura.DocCtrl.NumeroDoc.Value = 0;
                this._factura.DocCtrl.ComprobanteID.Value = comprobanteID;
                this._factura.DocCtrl.ComprobanteIDNro.Value = 0;
                this._factura.DocCtrl.MonedaID.Value = this.masterMoneda.Value;
                this._factura.DocCtrl.CuentaID.Value = cuenta;
                this._factura.DocCtrl.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                this._factura.DocCtrl.CentroCostoID.Value = this._ctrlProyecto.CentroCostoID.Value;
                this._factura.DocCtrl.LugarGeograficoID.Value = this._ctrlProyecto.LugarGeograficoID.Value;
                this._factura.DocCtrl.LineaPresupuestoID.Value = !string.IsNullOrEmpty(this._ctrlProyecto.LineaPresupuestoID.Value) ? this._ctrlProyecto.LineaPresupuestoID.Value : this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                this._factura.DocCtrl.Fecha.Value = DateTime.Now;
                this._factura.DocCtrl.FechaDoc.Value = this.dtFechaDoc.DateTime;
                this._factura.DocCtrl.PeriodoDoc.Value = new DateTime( this.dtFechaDoc.DateTime.Year, this.dtFechaDoc.DateTime.Month, 1);              
                this._factura.DocCtrl.TasaCambioCONT.Value = 0;
                this._factura.DocCtrl.TasaCambioDOCU.Value = 0;
                this._factura.DocCtrl.DocumentoNro.Value = 0;
                this._factura.DocCtrl.DocumentoID.Value = AppDocuments.FacturaVenta;
                this._factura.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                this._factura.DocCtrl.PeriodoUltMov.Value = this._factura.DocCtrl.PeriodoDoc.Value;
                this._factura.DocCtrl.seUsuarioID.Value = this.userID;
                this._factura.DocCtrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value; 
                this._factura.DocCtrl.ConsSaldo.Value = 0;
                this._factura.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this._factura.DocCtrl.Observacion.Value = !string.IsNullOrEmpty(this.txtObservacion.Text) ? this.txtObservacion.Text : this.txtDescripcion.Text;
                this._factura.DocCtrl.Descripcion.Value = "Prefactura Venta " +(this.rbtTipo.SelectedIndex == 0 ?  "Directa" : "Cuenta de Cobro");
                this._factura.DocCtrl.Valor.Value = Convert.ToDecimal(this.txtVlrFacturaActual.EditValue, CultureInfo.InvariantCulture);
                #endregion
                #region Carga FacturaHeader
                this._factura.Header.EmpresaID.Value = this.empresaID;
                this._factura.Header.NumeroDoc.Value = 0;
                this._factura.Header.AsesorID.Value = asesorxDef;
                this._factura.Header.FacturaTipoID.Value = tipoFactura;
                this._factura.Header.DocumentoREL.Value = 0;
                this._factura.Header.FacturaREL.Value = 0; 
                this._factura.Header.MonedaPago.Value = this.masterMoneda.Value;
                this._factura.Header.ClienteID.Value = this._headerProy.ClienteID.Value;
                this._factura.Header.ListaPrecioID.Value = cliente.ListaPrecioID.Value;
                this._factura.Header.ZonaID.Value = zonaxDef;
                this._factura.Header.TasaPago.Value = 1; 
                this._factura.Header.FechaVto.Value = this.dtFechaDoc.DateTime;
                this._factura.Header.ObservacionENC.Value = this.txtDescripcion.Text;//Descripcion del detalle 
                this._factura.Header.FormaPago.Value = "N/A";
                this._factura.Header.Valor.Value = Convert.ToDecimal(this.txtVlrFacturaActual.EditValue,CultureInfo.InvariantCulture);
                this._factura.Header.Iva.Value = 0;
                this._factura.Header.Bruto.Value = 0;
                this._factura.Header.PorcPtoPago.Value = 0;
                this._factura.Header.PorcRteGarantia.Value = 0;
                this._factura.Header.FechaPtoPago.Value = this.dtFechaDoc.DateTime;
                this._factura.Header.ValorPtoPago.Value = 0;
                this._factura.Header.Porcentaje1.Value = 0;
                this._factura.Header.FacturaFijaInd.Value = false;
                this._factura.Header.RteGarantiaIvaInd.Value =false;
                this._factura.Header.PorcRteGarantia.Value = 0;
                this._factura.Header.ValorRteGarantia =  0;
                this._factura.Header.Retencion10.Value = 0;
                this._factura.Header.ValorAnticipo = 0;
                this._factura.Header.PorcAnticipo.Value = this.chkIVA.Visible ? Math.Round(porcFacturaActual, 5) : 0;
                this._factura.Header.IncluyeIVAInd.Value = this.chkIVA.Visible? this.chkIVA.Checked : this._factura.Header.IncluyeIVAInd.Value;
                this._factura.Header.ValorAnticipo = this.txtVlrAnticipos.Visible? Convert.ToDecimal(this.txtVlrAmortizacion.EditValue, CultureInfo.InvariantCulture) : 0;              
                #endregion
                #region Carga Detalle
                this._factura.Footer = new List<DTO_faFacturacionFooter>();               
                #region Crea Detalle Inicial
                DTO_faFacturacionFooter det = new DTO_faFacturacionFooter();
                det.Index = 0;
                det.Movimiento.ImprimeInd.Value = true;
                det.Movimiento.NroItem.Value = 0;
                det.Movimiento.ServicioID.Value = servicio;
                det.Movimiento.CentroCostoID.Value = this._factura.DocCtrl.CentroCostoID.Value;
                det.Movimiento.LineaPresupuestoID.Value = this._factura.DocCtrl.LineaPresupuestoID.Value;
                det.Movimiento.TerceroID.Value = this._factura.DocCtrl.TerceroID.Value;
                det.Movimiento.ProyectoID.Value = this._factura.DocCtrl.ProyectoID.Value;                
                det.Movimiento.EmpresaID.Value = this.empresaID;
                det.Movimiento.BodegaID.Value = string.Empty;
                det.Movimiento.PlaquetaID.Value = string.Empty;
                det.Movimiento.inReferenciaID.Value = string.Empty;
                det.Movimiento.EstadoInv.Value = (int)EstadoInv.Activo;
                det.Movimiento.Parametro1.Value = string.Empty;
                det.Movimiento.Parametro2.Value = string.Empty;
                det.Movimiento.IdentificadorTr.Value = 0;
                det.Movimiento.SerialID.Value = string.Empty;
                det.Movimiento.EmpaqueInvID.Value = string.Empty;
                det.Movimiento.CantidadEMP.Value = 1;
                det.Movimiento.CantidadUNI.Value = 1;
                det.Movimiento.DescripTExt.Value = this.txtDescripcion.Text;
                det.Movimiento.ActivoID.Value = 0;
                det.Movimiento.DocSoporteTER.Value = "N/A";
                det.Movimiento.DatoAdd4.Value = this._ctrlProyecto.NumeroDoc.Value.ToString();
                det.Movimiento.ValorUNI.Value = 0;
                det.Movimiento.Valor1LOC.Value = 0;
                det.ValorBruto = 0;
                det.ValorIVA = 0;
                det.ValorTotal = 0;
                det.ValorNeto = 0;
                det.ValorOtros = 0;
                det.ValorRetenciones = 0;
                det.ValorRFT = 0;
                det.ValorRICA = 0;
                det.ValorRIVA = 0;
                if (this.rbtTipo.SelectedIndex == 1)//Cuenta de Cobro
                {
                    det.Movimiento.ValorUNI.Value = Convert.ToDecimal(this.txtVlrFacturaActual.EditValue,CultureInfo.InvariantCulture);
                    det.Movimiento.Valor1LOC.Value = Math.Round(det.Movimiento.ValorUNI.Value.Value, 0);
                    det.ValorBruto = det.Movimiento.Valor1LOC.Value.Value;                   
                    det.ValorTotal = det.Movimiento.Valor1LOC.Value.Value;
                    det.ValorNeto = det.Movimiento.Valor1LOC.Value.Value;                 
                }
                this._factura.Footer.Add(det);
                #endregion
                //Si es prefactura Crea el detalle de cada tarea prorrateando el valor
                if (this.rbtTipo.SelectedIndex == 0)
                {                  
                    foreach (DTO_pyProyectoTarea tarea in this._listTareasAll)
                    {
                        #region Crea Detalle fact
                        DTO_faFacturacionFooter footer = new DTO_faFacturacionFooter();
                        DTO_pyTarea dtoTarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, tarea.TareaID.Value, true);
                        footer.Movimiento.ImprimeInd.Value = false;
                        footer.Movimiento.NroItem.Value = 0;
                        footer.Movimiento.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                        footer.Movimiento.ServicioID.Value = this.servicioxDefecto;
                        footer.Movimiento.BodegaID.Value = string.Empty;
                        footer.Movimiento.inReferenciaID.Value = string.Empty;
                        footer.Movimiento.Parametro1.Value = string.Empty;
                        footer.Movimiento.Parametro2.Value = string.Empty;
                        footer.Movimiento.CentroCostoID.Value = this._ctrlProyecto.CentroCostoID.Value;
                        footer.Movimiento.TerceroID.Value = cliente.TerceroID.Value;
                        footer.Movimiento.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                        footer.Movimiento.LineaPresupuestoID.Value = dtoTarea != null ? dtoTarea.LineaPresupuestoID.Value : string.Empty;
                        footer.Movimiento.PlaquetaID.Value = string.Empty;
                        footer.Movimiento.DocSoporte.Value = tarea.Consecutivo.Value;//Asigna el consecutivo de la tarea;
                        footer.Movimiento.DocSoporteTER.Value = "0";
                        footer.Movimiento.SerialID.Value = string.Empty;
                        footer.Movimiento.EmpaqueInvID.Value = string.Empty;
                        footer.Movimiento.DatoAdd4.Value = this._ctrlProyecto.NumeroDoc.Value.ToString();//Asigna el numDoc del proyecto
                        footer.Movimiento.EstadoInv.Value = (int)EstadoInv.Activo;
                        footer.Movimiento.IdentificadorTr.Value = 0;
                        footer.Movimiento.CantidadEMP.Value = 0;

                        footer.Movimiento.CantidadUNI.Value = Math.Ceiling(tarea.Cantidad.Value.Value * (porcFacturaActual / 100));
                        footer.Movimiento.Valor1LOC.Value = Math.Round(tarea.CostoLocalCLI.Value.Value * (porcFacturaActual / 100), 0);

                        footer.Movimiento.ValorUNI.Value = footer.Movimiento.CantidadUNI.Value != 0 ? Math.Round(footer.Movimiento.Valor1LOC.Value.Value / footer.Movimiento.CantidadUNI.Value.Value,2) : 0;
                        footer.ValorBruto = footer.Movimiento.Valor1LOC.Value.Value;
                        footer.ValorNeto = footer.Movimiento.Valor1LOC.Value.Value;
                        footer.ValorTotal = footer.Movimiento.Valor1LOC.Value.Value;
                        footer.ValorIVA = 0;
                        footer.Movimiento.DescripTExt.Value = tarea.Descriptivo.Value;
                        footer.Movimiento.DescriptivoTarea.Value = tarea.Descriptivo.Value;
                        footer.Movimiento.TareaID.Value = tarea.TareaID.Value;
                        if (footer.Movimiento.CantidadUNI.Value > 0)
                            this._factura.Footer.Add(footer);
                        #endregion
                    }
                    this.GetRetenciones();
                    this._factura.Header.Iva.Value = this._factura.Footer.Sum(x => x.ValorIVA);

                    //Valida si es proyecto AIU para calcular valores correspondientes
                    if (this._headerProy != null && this._headerProy.APUIncluyeAIUInd.Value.Value)
                    {
                        this._factura.Header.PorcAdministracion = this._headerProy.PorEmpresaADM.Value.Value;
                        this._factura.Header.PorcImprevistos = this._headerProy.PorEmpresaIMP.Value.Value;
                        this._factura.Header.PorcUtilidad = this._headerProy.PorEmpresaUTI.Value.Value;

                        this._factura.Header.Administracion.Value = Math.Round(this._factura.Footer.Sum(x => x.ValorBruto) * (this._factura.Header.PorcAdministracion / 100), 0);
                        this._factura.Header.Imprevistos.Value = Math.Round(this._factura.Footer.Sum(x => x.ValorBruto) * (this._factura.Header.PorcImprevistos / 100), 0);
                        this._factura.Header.Utilidad.Value = Math.Round(this._factura.Footer.Sum(x => x.ValorBruto) * (this._factura.Header.PorcUtilidad / 100), 0);
                        this._factura.Header.Iva.Value = this._headerProy.PorIVA.Value.HasValue ? Math.Round(this._factura.Header.Utilidad.Value.Value * (this._headerProy.PorIVA.Value.Value / 100), 0) : 0;
                        this._factura.Header.Valor.Value = this._factura.Header.Valor.Value + this._factura.Header.Iva.Value;
                    }

                }


                #endregion        
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-PrefacturaPagoAnticipado.cs", "LoadTempHeader"));
                return false;
            }
        }

        /// <summary>
        /// Calcula los valores de retenciones     
        /// </summary>
        /// <param name="masivoInd">Indica si es masivo el calculo</param>
        private void GetRetenciones()
        {
            try
            {
                decimal porcIVA = 0;
                decimal porcRIVA = 0;
                decimal porcRFT = 0;
                decimal porcOtros = 0;
                string conceptoCargo = string.Empty;
                DTO_coTercero clienteTer = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._ctrlProyecto.TerceroID.Value, true);
                this._refFiscalTercero = clienteTer.ReferenciaID.Value;
                DTO_faServicios servicio = (DTO_faServicios)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.faServicios, false, this.servicioxDefecto, true);
                DTO_faConceptos conceptos = (DTO_faConceptos)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faConceptos, false, servicio.ConceptoIngresoID.Value, true);
                if (conceptos != null)
                    conceptoCargo = conceptos.ConceptoCargoID.Value;


                //Calcula impuestos para todo el detalle
                foreach (DTO_faFacturacionFooter f in this._factura.Footer)
                {
                    if (!string.IsNullOrEmpty(conceptoCargo))
                    {
                        #region Crea consulta para la tabla coImpuesto
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        DTO_glConsultaFiltro filtro;

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalEmpresaID";
                        filtro.ValorFiltro = _regFiscalEmp.ReferenciaID.Value;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "RegimenFiscalTerceroID";
                        filtro.ValorFiltro = this._refFiscalTercero;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ConceptoCargoID";
                        filtro.ValorFiltro = conceptoCargo;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "LugarGeograficoID";
                        filtro.ValorFiltro = this._ctrlProyecto.LugarGeograficoID.Value;
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        filtro = new DTO_glConsultaFiltro();
                        filtro.CampoFisico = "ImpuestoTipoID";
                        filtro.ValorFiltro = this._bc.GetControlValueByCompany(ModulesPrefix.fa,AppControl.fa_CodigoIVA);
                        filtro.OperadorFiltro = OperadorFiltro.Igual;
                        filtro.OperadorSentencia = "and";
                        filtros.Add(filtro);

                        consulta.Filtros = filtros;
                        #endregion
                        #region Trae los impuestos existentes
                        long count = this._bc.AdministrationModel.MasterComplex_Count(AppMasters.coImpuesto, consulta, null);
                        var listCoImp = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.coImpuesto, count, 1, consulta, null).ToList();
                        if (listCoImp.Count > 0)
                        {
                            DTO_coImpuesto dtoImp = (DTO_coImpuesto)listCoImp.First();
                            DTO_coPlanCuenta cta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, dtoImp.CuentaID.Value, true);
                            try
                            {
                                porcIVA = cta.ImpuestoPorc.Value.Value;                                
                            }
                            catch (Exception)
                            {
                                string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Co_PorcentajeImpCuentaInvalid);
                                MessageBox.Show(string.Format(msg, cta.ID.Value));
                            }
                        }
                        #endregion                        
                    }

                    f.PorcIVA = porcIVA;
                    f.PorcRIVA = porcRIVA;
                    f.PorcRFT = porcRFT;
                    f.PorcOtros = porcOtros;
                    this._factura.Header.Porcentaje1.Value = this._factura.Header.Porcentaje1.Value ?? 0;
                    decimal vlrIva = Math.Round((f.PorcIVA / 100) * f.ValorBruto, 0);
                    decimal vlrRetenciones = Math.Round((f.PorcRIVA / 100) * f.ValorBruto, 0) + Math.Round((this._factura.Header.Porcentaje1.Value.Value / 1000) * f.ValorBruto, 0) + Math.Round((f.PorcRFT / 100) * f.ValorBruto, 0);
                    f.ValorIVA = vlrIva;
                    f.ValorRIVA = Math.Round((f.PorcRIVA / 100) * f.ValorBruto, 0);
                    f.ValorRICA = Math.Round((this._factura.Header.Porcentaje1.Value.Value / 1000) * f.ValorBruto, 0);
                    f.ValorRFT = Math.Round((f.PorcRFT / 100) * f.ValorBruto, 0);

                    f.ValorRetenciones = vlrRetenciones;
                    f.ValorTotal = f.ValorBruto + vlrIva;
                    f.ValorNeto = (f.ValorBruto + vlrIva) - vlrRetenciones;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "GetRetenciones"));
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.PrefacturaDirecta;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.servicioCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa,AppControl.fa_ServicioCtaCobro);
            this.servicioxDefecto = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioxDefecto);
            this.tipoFacturaCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_TipoFacturaCtaCobro);
            this.masterMoneda.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._regFiscalEmp = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto), true);
            this.prefijoCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_PrefijoCtaCobro);

            this.masterMoneda.EnableControl(false);
            this.moneda = this.masterMoneda.Value;
            this.rbtTipo.SelectedIndex = 1;
            this.saveDelegate = new Save(this.SaveMethod);
        }

        /// <summary>
        /// Obtiene el valor del anticipo
        /// </summary>
        private void GetSaldoProyecto()
        {
            try
            {
                if (this._headerProy != null)
                {
                    //Trae las facturas de ventas aprobadas del proyecto
                    this.vlrEjecutadoProy = 0;
                    DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                    filter.DatoAdd4.Value = this._headerProy.NumeroDoc.Value.ToString();
                    filter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                    List<DTO_glMovimientoDeta> mvtos = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, false);
                    mvtos.AddRange(this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, true));
                    decimal anticipos = mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.FacturaTipoID.Value == this.tipoFacturaCtaCobro && x.ServicioID.Value == this.servicioCtaCobro && !x.DatoAdd5.Value.Equals("INV")).Sum(x => x.Valor1LOC.Value.Value);
                    mvtos = mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.DocSoporte.Value.HasValue && !x.DatoAdd5.Value.Equals("INV"));

                    //Recorre las Facturas de Venta con tareas relacionadas                   
                    decimal ivaProy = this._headerProy.PorIVA.Value.HasValue? this._headerProy.PorIVA.Value.Value/100 : 0;
                    foreach (DTO_glMovimientoDeta mvto in mvtos)
                    {
                        //Valida si la tarea esta relacionada
                        if (this._listTareasAll.Exists(x => x.Consecutivo.Value == mvto.DocSoporte.Value))
                            this.vlrEjecutadoProy += mvto.Valor1LOC.Value.Value;
                    }
                    this.vlrEjecutadoProy = Math.Round(this.vlrEjecutadoProy,0);
                    this.vlrProyectoTotal = Math.Round(this._listTareasAll.Sum(x => x.CostoLocalCLI.Value.Value),0);
                    this.porcEntregado = this.vlrProyectoTotal != 0 ? this.vlrEjecutadoProy * 100 / this.vlrProyectoTotal : 0;

                    //Valores Proyecto
                    this.txtVlrProyecto.EditValue = this.vlrProyectoTotal;
                    this.txtVlrIVAProyecto.EditValue = Math.Round(this.vlrProyectoTotal * ivaProy, 0); 
                    this.txtTotalProyecto.EditValue = this.vlrProyectoTotal + Math.Round((this.vlrProyectoTotal * ivaProy),0);

                    //Valores Factura
                    this.txtVlrFacturado.EditValue = this.vlrEjecutadoProy;
                    this.vlrProyectoTotalconIVA = Math.Round(this.vlrEjecutadoProy * ivaProy, 0);
                    this.txtVlrIVAFacturado.EditValue = this.vlrProyectoTotalconIVA;
                    this.txtTotalFacturado.EditValue = this.vlrEjecutadoProy + Math.Round((this.vlrEjecutadoProy * ivaProy),0);

                    if(this._headerProy.APUIncluyeAIUInd.Value.Value)
                    {
                        this.txtVlrIVAFacturado.Visible = false;
                        this.txtTotalFacturado.Visible = false;
                    }
                    else
                    {
                        this.txtVlrIVAFacturado.Visible = true;
                        this.txtTotalFacturado.Visible = true;
                    }

                    this.txtVlrPendiente.EditValue = (this.vlrProyectoTotal + Math.Round((this.vlrProyectoTotal * ivaProy),0)) - (this.vlrEjecutadoProy + Math.Round((this.vlrEjecutadoProy * ivaProy),0));
                    this.txtPorcFacturado.EditValue = this.porcEntregado;

                    //Asigna anticipos
                    this.txtVlrAnticipos.EditValue = anticipos;
                                             
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetSaldoProyecto"));                
            }
        }

        /// <summary>
        /// Obtiene el saldo del tercero para Anticipos
        /// </summary>
        /// <returns></returns>
        private decimal GetSaldoAnticipo()
        {
            try
            {
                decimal saldoAntic = 0;
                string cuentaAntic = this._bc.GetControlValueByCompanyAllowEmpty(ModulesPrefix.fa, AppControl.fa_CuentaAnticiposMdaLocal);
                string libroFunc = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                DateTime periodo = Convert.ToDateTime(this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo));
                DTO_faCliente cliente = this._headerProy != null ? (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this._headerProy.ClienteID.Value, true) : null;

                DTO_coCuentaSaldo saldoFilter = new DTO_coCuentaSaldo();
                saldoFilter.PeriodoID.Value = DateTime.Now.Date.Month == periodo.Month ? periodo : new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                saldoFilter.BalanceTipoID.Value = libroFunc;
                saldoFilter.CuentaID.Value = cuentaAntic;
                saldoFilter.TerceroID.Value = cliente.TerceroID.Value;
                saldoFilter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                if (!string.IsNullOrEmpty(saldoFilter.CuentaID.Value))
                {
                    List<DTO_coCuentaSaldo> saldosList = this._bc.AdministrationModel.Saldos_GetByParameter(saldoFilter);
                    saldoAntic = Math.Abs(saldosList.Sum(x => x.DbOrigenLocML.Value.Value + x.DbOrigenExtML.Value.Value + x.CrOrigenLocML.Value.Value +
                        x.CrOrigenExtML.Value.Value + x.DbSaldoIniLocML.Value.Value + x.DbSaldoIniExtML.Value.Value + x.CrSaldoIniLocML.Value.Value + x.CrSaldoIniExtML.Value.Value));

                }
                return saldoAntic;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetSaldoAnticipo"));
                return 0;
            }
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
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.tbBreak2.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Formulario

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
                    this._ctrlProyecto = this.ucProyecto.ProyectoInfo.DocCtrl;
                    this._headerProy = this.ucProyecto.ProyectoInfo.HeaderProyecto;
                    this._listEntregablesProy = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);

                    DTO_pyClaseProyecto claseProy = (DTO_pyClaseProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, false,this._headerProy.ClaseServicioID.Value, true);
                    this.tipoFacturaProyecto = claseProy.FacturaTipoID.Value;

                    DTO_faFacturaTipo dtoFacturaTipo = (DTO_faFacturaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faFacturaTipo, false, this.rbtTipo.SelectedIndex == 0 ? this.tipoFacturaProyecto : this.tipoFacturaCtaCobro, true);
                    if (dtoFacturaTipo != null)
                        this.txtDescripcion.Text = dtoFacturaTipo.ObservacionENC.Value;

                    this.GetSaldoProyecto();

                    //Valida el valor del anticipo del proyecto para Cuenta de Cobro
                    if (this.rbtTipo.SelectedIndex == 1 && this._headerProy != null && this._headerProy.ValorAnticipoInicial.Value > 0)
                    {
                        this.txtVlrFacturaActual.EditValue = this._headerProy.ValorAnticipoInicial.Value;
                        this.chkIVA.Checked = this._headerProy.RteGarantiaIvaInd.Value.HasValue ? this._headerProy.RteGarantiaIvaInd.Value.Value : false;
                        bool OK = true;
                        if (this._headerProy.ValorAnticipoInicial.Value + this.vlrEjecutadoProy > this.vlrProyectoTotal)
                        {
                            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
                            string msgAprobar = string.Format(msgDoc, "Aprobar");

                            if (MessageBox.Show("El valor facturado más el abono supera el valor total del proyecto, desea continuar?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.No)
                                OK = false;
                        }
                        if (OK)
                        {
                            decimal porcNuevaEntrega = this.vlrProyectoTotal != 0 ? (this._headerProy.ValorAnticipoInicial.Value.Value) * 100 / this.vlrProyectoTotal : 0;
                            this.txtPorcFacturaActual.EditValue = porcNuevaEntrega;
                            if (this.txtVlrAmortizacion.Visible && this._headerProy.ValorAnticipoInicial.Value <= this.vlrProyectoTotal)
                                this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.txtVlrAnticipos.EditValue, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100), 0);
                        }
                        else
                            this.txtVlrFacturaActual.EditValue = this.vlrProyectoTotal;
                    }
                    this.pnDetalle.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado", "LoadData"));
            }
        }

        /// <summary>
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValorFactura_EditValueChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVlrFacturaActual_Leave(object sender, EventArgs e)
        {
            decimal vlrFactura = Convert.ToDecimal(this.txtVlrFacturaActual.EditValue, CultureInfo.InvariantCulture);
            bool OK = true;
            if (vlrFactura + this.vlrEjecutadoProy > this.vlrProyectoTotal)
            {
                string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
                string msgAprobar = string.Format(msgDoc, "Aprobar");

                if (MessageBox.Show("El valor facturado más el abono supera el valor total del proyecto, desea continuar?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.No)
                    OK = false;
            }

            if (OK)
            {
                decimal porcNuevaEntrega = this.vlrProyectoTotal != 0 ? (vlrFactura) * 100 / this.vlrProyectoTotal : 0;
                this.txtPorcFacturaActual.EditValue = porcNuevaEntrega;
                if (this.txtVlrAmortizacion.Visible && vlrFactura <= this.vlrProyectoTotal)
                    this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.txtVlrAnticipos.EditValue, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100),0);
            }
            else
            {
                this.txtVlrFacturaActual.EditValue = this.vlrProyectoTotal;
            }
        }

        /// <summary>
        /// UC de Proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPorcNuevoAbono_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                decimal porcNuevaEntrega = Convert.ToDecimal(this.txtPorcFacturaActual.EditValue, CultureInfo.InvariantCulture);
                decimal vlrFactura = this.vlrProyectoTotal * porcNuevaEntrega / 100;
                this.txtVlrFacturaActual.EditValue = vlrFactura;
                if (this.txtVlrAmortizacion.Visible && vlrFactura <= this.vlrProyectoTotal)
                    this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.txtVlrAnticipos.EditValue, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100));

            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkIVA_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIVA.Checked)
            {
                //decimal porcNuevaEntrega = Convert.ToDecimal(this.txtPorcFacturaActual.EditValue, CultureInfo.InvariantCulture);
                //decimal vlrFactura = (this.vlrProyectoTotal) * porcNuevaEntrega / 100;
                //this.txtVlrFacturaActual.EditValue = vlrFactura;
                //if (this.txtVlrAmortizacion.Visible && vlrFactura <= (this.vlrProyectoTotal))
                //    this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.txtVlrAnticipos.EditValue, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100));
            }
        }

        /// <summary>
        /// Seleccion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtTipoTraslado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rbtTipo.SelectedIndex == 0) //Prefactura 
            {
                this.txtVlrAmortizacion.Visible = true;
                this.txtVlrAnticipos.Visible = true;
                this.lblAmortiza.Visible = true;
                this.lblAnticipos.Visible = true;
                this.chkIVA.Visible = false;
            }
            else if (this.rbtTipo.SelectedIndex == 1) //Cuenta de Cobro
            {
                this.txtVlrAmortizacion.Visible = false;
                this.txtVlrAnticipos.Visible = false;
                this.lblAmortiza.Visible = false;
                this.lblAnticipos.Visible = false;
                this.chkIVA.Visible = true;

                //Valida el valor del anticipo del proyecto si existe para Cuenta de Cobro
                if (this._headerProy != null && this._headerProy.ValorAnticipoInicial.Value > 0)
                {
                    this.txtVlrFacturaActual.EditValue = this._headerProy.ValorAnticipoInicial.Value;
                    this.chkIVA.Checked = this._headerProy.RteGarantiaIvaInd.Value.HasValue? this._headerProy.RteGarantiaIvaInd.Value.Value : false;
                    bool OK = true;
                    if (this._headerProy.ValorAnticipoInicial.Value + this.vlrEjecutadoProy > this.vlrProyectoTotal)
                    {
                        string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
                        string msgAprobar = string.Format(msgDoc, "Aprobar");

                        if (MessageBox.Show("El valor facturado más el abono supera el valor total del proyecto, desea continuar?", "Advertencia", MessageBoxButtons.YesNo) == DialogResult.No)
                            OK = false;
                    }
                    if (OK)
                    {
                        decimal porcNuevaEntrega = this.vlrProyectoTotal != 0 ? (this._headerProy.ValorAnticipoInicial.Value.Value) * 100 / this.vlrProyectoTotal : 0;
                        this.txtPorcFacturaActual.EditValue = porcNuevaEntrega;
                        if (this.txtVlrAmortizacion.Visible && this._headerProy.ValorAnticipoInicial.Value <= this.vlrProyectoTotal)
                            this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.txtVlrAnticipos.EditValue, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100), 0);
                    }
                    else
                        this.txtVlrFacturaActual.EditValue = this.vlrProyectoTotal;
                }
            }           
        }
     
        #endregion

        #region Eventos Grilla Header

        /// <summary>
        /// Se ejecutar cuando se selecciona un registro de la Grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHeader_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PreFacturaPagoAnticipado.cs", "gvDocument_FocusedRowChanged"));
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
        /// Al modificar las celdas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvHeader_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
                     
        }
            
        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFooterFact_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            object dto = (object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal")
                        e.Value = pi.GetValue(dto, null);
                    else
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || fi.FieldType.Name == "Decimal")
                            e.Value = fi.GetValue(dto);
                        else
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                    }
                    else
                    {
                        DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                        pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Decimal")
                                e.Value = pi.GetValue(dtoM.Movimiento, null);
                            else
                                e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dtoM.Movimiento, null), null);
                        }
                        else
                        {
                            fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || fi.FieldType.Name == "Decimal")
                                    e.Value = fi.GetValue(dtoM.Movimiento);
                                else
                                    e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dtoM.Movimiento), null);
                            }
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
                        DTO_faFacturacionFooter dtoM = (DTO_faFacturacionFooter)e.Row;
                        pi = dtoM.Movimiento.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                e.Value = pi.GetValue(dtoM.Movimiento, null);
                            else
                            {
                                UDT udtProp = (UDT)pi.GetValue(dtoM.Movimiento, null);
                                udtProp.SetValueFromString(e.Value.ToString());
                            }
                        }
                        else
                        {
                            fi = dtoM.Movimiento.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (fi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                                {
                                    //e.Value = pi.GetValue(dto, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)fi.GetValue(dtoM.Movimiento);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFooterFact_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvFooterFact.Columns[this.unboundPrefix + fieldName];
            this.gvFooterFact.PostEditor();
            try
            {
                DTO_faFacturacionFooter row = (DTO_faFacturacionFooter)this.gvFooterFact.GetRow(e.RowHandle);
                if (fieldName == "NroItem")
                {
                    if (row != null && this._listDetalleFact.Count(x => x.Movimiento.NroItem.Value == Convert.ToInt32(e.Value)) > 0)
                    {
                        DTO_faFacturacionFooter filter = this._listDetalleFact.FindAll(x => x.Movimiento.NroItem.Value == Convert.ToInt32(e.Value)).First();
                        row.Movimiento.DescripTExt.Value = filter.Movimiento.DescripTExt.Value;
                    }
                }
                else if (fieldName == "DescripTExt")
                {
                    foreach (DTO_faFacturacionFooter tar in this._listDetalleFact.FindAll(x => x.Movimiento.NroItem.Value == row.Movimiento.NroItem.Value))
                    {
                        tar.Movimiento.DescripTExt.Value = e.Value.ToString();
                    }
                }

                this.gvFooterFact.RefreshData();
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// Enviar a aprobación
        /// </summary>
        public override void TBSave()
        {
            if (this.LoadFactura())
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public void SaveThread()
        {
            try
            {
                DTO_SerializedObject result = new DTO_SerializedObject();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCFT(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                int numeroDoc = 0;
                result = _bc.AdministrationModel.FacturaVenta_Guardar(AppDocuments.FacturaVenta, this._factura.DocCtrl, this._factura.Header, this._factura.Footer, false, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = this._bc.SendDocumentMail(MailType.NotSend, this.documentID, this._bc.AdministrationModel.User.ID.Value, result, true,true);

                #region Genera Reporte

                if (result.GetType() == typeof(DTO_Alarma))
                {
                    string reportName = string.Empty;
                    string fileURl;
                    DTO_Alarma alarma = (DTO_Alarma)result;
                    if(this.rbtTipo.SelectedIndex == 0)
                        reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(AppDocuments.FacturaVenta, alarma.NumeroDoc, false, ExportFormatType.pdf, this._factura.Header.ValorAnticipo, 0,0);
                    else
                        reportName = this._bc.AdministrationModel.ReportesFacturacion_FacturaVenta(AppReports.faCuentaCobro, alarma.NumeroDoc, false, ExportFormatType.pdf, this._factura.Header.ValorAnticipo,0, 0);

                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, Convert.ToInt32(alarma.NumeroDoc), null, reportName.ToString());
                    Process.Start(fileURl);
                }

                #endregion

                if (isOK)
                {
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentFacturaForm.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion                                   
    }
}
