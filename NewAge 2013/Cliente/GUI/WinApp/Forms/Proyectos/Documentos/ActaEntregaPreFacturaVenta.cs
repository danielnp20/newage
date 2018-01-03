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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ActaEntregaPreFacturaVenta : FormWithToolbar
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
        private bool isValid = true;
        //Variables de datos
        private DTO_glDocumentoControl _ctrlProyecto = null;
        private DTO_pyProyectoDocu _headerProy = null;
        private DTO_pyProyectoTareaCliente _rowTareaClienteCurrent = null;
        private List<DTO_pyProyectoTareaCliente> _listTareasActa = new List<DTO_pyProyectoTareaCliente>();
        private List<DTO_faFacturacionFooter> _listDetalleFact = new List<DTO_faFacturacionFooter>();
        //Variables de datos Proyecto
        private List<DTO_pyProyectoTarea> _listTareasAll = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTarea> _listTareasAdic = new List<DTO_pyProyectoTarea>();
        private List<DTO_pyProyectoTareaCliente> _listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();       
        private List<DTO_pyActaEntregaDeta> _listActaDeta = new List<DTO_pyActaEntregaDeta>();
        private List<DTO_pyActaEntregaDeta> _listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
        private DTO_faCliente cliente = null;
        private decimal vlrProyectoTotal = 0;
        private decimal vlrEjecutadoProy = 0;
        private decimal porcEntregado = 0;
        private decimal vlrAnticipoCtaCobro = 0;
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
        public ActaEntregaPreFacturaVenta()
        {
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                this.LoadDetailFactura();
                this.frmModule = ModulesPrefix.py;

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta.cs", "ActaEntregaPreFacturaVenta"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Add Columns in Grid
        /// </summary>
        private void AddGridCols()
        {
            #region Columnas Header
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
            aprob.Width = 20;
            aprob.Visible = true;
            this.gvHeader.Columns.Add(aprob);

            GridColumn TareaEntregable = new GridColumn();
            TareaEntregable.FieldName = this.unboundPrefix + "TareaEntregable";
            TareaEntregable.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_TareaEntregable");
            TareaEntregable.UnboundType = UnboundColumnType.String;
            TareaEntregable.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            TareaEntregable.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TareaEntregable.AppearanceCell.Options.UseTextOptions = true;
            TareaEntregable.AppearanceCell.Options.UseFont = true;
            TareaEntregable.VisibleIndex = 1;
            TareaEntregable.Width = 25;
            TareaEntregable.Visible = true;
            TareaEntregable.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(TareaEntregable);

            GridColumn Descripcion = new GridColumn();
            Descripcion.FieldName = this.unboundPrefix + "Descripcion";
            Descripcion.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Entregables + "_Descriptivo");
            Descripcion.UnboundType = UnboundColumnType.String;
            Descripcion.VisibleIndex = 2;
            Descripcion.Width = 250;
            Descripcion.Visible = true;
            Descripcion.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Descripcion);

            GridColumn Cantidad = new GridColumn();
            Cantidad.FieldName = this.unboundPrefix + "Cantidad";
            Cantidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Cantidad");
            Cantidad.UnboundType = UnboundColumnType.String;
            Cantidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            Cantidad.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Cantidad.AppearanceCell.Options.UseTextOptions = true;
            Cantidad.AppearanceCell.Options.UseFont = true;
            Cantidad.VisibleIndex = 5;
            Cantidad.Width = 60;
            Cantidad.Visible = true;
            Cantidad.ColumnEdit = this.editValue2Cant;
            Cantidad.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(Cantidad);      

            GridColumn ValorFactura = new GridColumn();
            ValorFactura.FieldName = this.unboundPrefix + "ValorAEntregar";
            ValorFactura.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorAEntregar");
            ValorFactura.UnboundType = UnboundColumnType.String;
            ValorFactura.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            ValorFactura.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ValorFactura.AppearanceCell.Options.UseTextOptions = true;
            ValorFactura.AppearanceCell.Options.UseFont = true;
            ValorFactura.VisibleIndex = 6;
            ValorFactura.Width = 60;
            ValorFactura.Visible = true;
            ValorFactura.ColumnEdit = this.editValue2;
            ValorFactura.OptionsColumn.AllowEdit = false;
            this.gvHeader.Columns.Add(ValorFactura);
            this.gvHeader.OptionsView.ColumnAutoWidth = true;      
            #endregion                       
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
                this._bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, true, true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDetailFactura()
        {
            try
            {
                #region Crea glMovimientoDeta
                this._listDetalleFact = new List<DTO_faFacturacionFooter>();
                List<DTO_faFacturacionFooter> listTemp = new List<DTO_faFacturacionFooter>();
                if (this.cliente == null)
                    this.cliente = this._headerProy != null? (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this._headerProy.ClienteID.Value, true) : null;

                //Recorre los entregables con actas de entrega
                foreach (DTO_pyProyectoTareaCliente entregable in this._listTareasActa.FindAll(x => x.SelectInd.Value.Value))
                {
                    //Recorre las actas de cada entrega
                    foreach (DTO_pyActaEntregaDeta acta in entregable.DetalleActas)
                    {
                        //Trae el plan de entrega de cada acta
                        DTO_pyProyectoPlanEntrega plan = entregable.Detalle.Find(x => x.Consecutivo.Value == acta.ConsTareaEntrega.Value);
                        //Recorre las tareas de Proyecto de cada entregable
                        foreach (DTO_pyProyectoTarea tarea in entregable.DetalleTareas)
                        {
                            #region Crea Detalle fact 
                            DTO_faFacturacionFooter footer = new DTO_faFacturacionFooter();
                            DTO_pyTarea dtoTarea = (DTO_pyTarea)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyTarea, false, tarea.TareaID.Value, true);

                            footer.Movimiento.ImprimeInd.Value = true;
                            footer.Movimiento.EmpresaID.Value = this._bc.AdministrationModel.Empresa.ID.Value;
                            footer.Movimiento.ServicioID.Value = entregable.ServicioID.Value;
                            footer.Movimiento.BodegaID.Value = string.Empty;
                            footer.Movimiento.inReferenciaID.Value = string.Empty;
                            footer.Movimiento.Parametro1.Value = string.Empty;
                            footer.Movimiento.Parametro2.Value = string.Empty;    
                            footer.Movimiento.CentroCostoID.Value = this._ctrlProyecto.CentroCostoID.Value;
                            footer.Movimiento.TerceroID.Value = cliente.TerceroID.Value;
                            footer.Movimiento.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                            footer.Movimiento.LineaPresupuestoID.Value = dtoTarea != null? dtoTarea.LineaPresupuestoID.Value: string.Empty;                           
                            footer.Movimiento.PlaquetaID.Value = string.Empty;
                            footer.Movimiento.DocSoporte.Value = tarea.Consecutivo.Value;//Asigna el consecutivo de la tarea;
                            footer.Movimiento.DocSoporteTER.Value = "0";
                            footer.Movimiento.SerialID.Value = string.Empty;
                            footer.Movimiento.EmpaqueInvID.Value = string.Empty;
                            footer.Movimiento.DatoAdd4.Value = this._ctrlProyecto.NumeroDoc.Value.ToString();//Asigna el numDoc del proyecto
                            footer.Movimiento.EstadoInv.Value = (int)EstadoInv.Activo;
                            footer.Movimiento.IdentificadorTr.Value = 0;
                            footer.Movimiento.CantidadEMP.Value = 0;

                            //Valida si es entrega individual para asignar valores y cantidades
                            if (dtoTarea != null && !dtoTarea.EntregaIndividualInd.Value.Value)
                            {
                                footer.Movimiento.CantidadUNI.Value = Math.Ceiling(tarea.Cantidad.Value.Value * (acta.PorEntregado.Value.Value / 100));
                                footer.Movimiento.Valor1LOC.Value = Math.Round(tarea.CostoLocalCLI.Value.Value * (acta.PorEntregado.Value.Value / 100), 0);
                            }
                            else 
                            {
                                footer.Movimiento.CantidadUNI.Value = acta.Cantidad.Value;
                                footer.Movimiento.Valor1LOC.Value =  Math.Round(acta.ValorFactura.Value.Value,0);
                            }
                            footer.Movimiento.ValorUNI.Value = footer.Movimiento.CantidadUNI.Value != 0 ? Math.Round((footer.Movimiento.Valor1LOC.Value.Value / footer.Movimiento.CantidadUNI.Value.Value),0) : 0;
                            footer.ValorBruto = footer.Movimiento.Valor1LOC.Value.Value;
                            footer.ValorNeto = footer.Movimiento.Valor1LOC.Value.Value;
                            footer.ValorTotal = footer.Movimiento.Valor1LOC.Value.Value;
                            footer.ValorIVA = 0;
                            footer.Movimiento.DescripTExt.Value = tarea.Descriptivo.Value;
                            footer.Movimiento.DescriptivoTarea.Value = tarea.Descriptivo.Value;
                            footer.Movimiento.TareaID.Value = tarea.TareaID.Value;
                            if (entregable.NumeroDocActa.Value != null)
                            {
                                DTO_glDocumentoControl ctrlActa = this._bc.AdministrationModel.glDocumentoControl_GetByID(entregable.NumeroDocActa.Value.Value);
                                footer.Movimiento.DocSoporteTER.Value = !string.IsNullOrEmpty(ctrlActa.DocumentoTercero.Value) ? ctrlActa.DocumentoTercero.Value : "N/A";
                            }

                            listTemp.Add(footer);
                            #endregion
                        }
                    }
                }

                //Resume el detalle por tareas
                List<string> listMvtosFact = listTemp.Select(x => x.Movimiento.DescriptivoTarea.Value).Distinct().ToList();
                foreach (string tarea in listMvtosFact)
                {
                    #region Crea detalle final
                    DTO_faFacturacionFooter mvto = listTemp.Find(x => x.Movimiento.DescriptivoTarea.Value == tarea);
                    if (mvto != null)
                    {
                        mvto.Index = 0;
                        mvto.Movimiento.NroItem.Value = this._listDetalleFact.Count() + 1;
                        mvto.Movimiento.CantidadUNI.Value = listTemp.FindAll(x => x.Movimiento.DescriptivoTarea.Value == tarea).Sum(y => y.Movimiento.CantidadUNI.Value);
                        mvto.Movimiento.Valor1LOC.Value = listTemp.FindAll(x => x.Movimiento.DescriptivoTarea.Value == tarea).Sum(y => y.Movimiento.Valor1LOC.Value);
                        mvto.Movimiento.ValorUNI.Value = listTemp.FindAll(x => x.Movimiento.DescriptivoTarea.Value == tarea).Sum(y => y.Movimiento.ValorUNI.Value);
                        mvto.ValorBruto = mvto.Movimiento.Valor1LOC.Value.Value;
                        mvto.ValorNeto = mvto.Movimiento.Valor1LOC.Value.Value;
                        mvto.ValorTotal = mvto.Movimiento.Valor1LOC.Value.Value;
                        mvto.ValorRetenciones = 0;
                        mvto.ValorIVA = 0;
                        this._listDetalleFact.Add(mvto);  
                    }
                    #endregion
                }
                #endregion
                this.LoadGrids(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta", "LoadDetailFactura"));
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
            this._listActaDetaExist = this._listActaDetaExist.FindAll(x => x.NumDocFactura.Value == null && x.Estado.Value == (Int16)EstadoDocControl.Aprobado).ToList();
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
                footerDet.SelectInd.Value = true;
                this._listTareasActa.Add(footerDet);
            }
            #endregion

            //Carga los items de la facturacion automaticamente
            foreach (DTO_pyProyectoTareaCliente t in this._listTareasActa)
                this.LoadDetailFactura();
            
            this.LoadGrids(false);
        }

        /// <summary>
        /// Carga la información del detalle
        /// </summary>
        /// <param name="firstTime"></param>
        private void LoadGrids(bool loadFooter)
        {
            try
            {
                if (loadFooter)
                {
                    this.gcFooterFact.DataSource = null;
                    this.gcFooterFact.DataSource = this._listDetalleFact;
                    this.gcFooterFact.RefreshDataSource();
                }
                else
                {
                    this.gcHeader.DataSource = null;
                    this.gcHeader.DataSource = this._listTareasActa;
                    this.gcHeader.RefreshDataSource();

                    //Limpia el detalle
                    //this._listDetalleFact = new List<DTO_faFacturacionFooter>();
                    this.gcFooterFact.DataSource = null;
                    this.gcFooterFact.DataSource = this._listDetalleFact;
                    this.gcFooterFact.RefreshDataSource();
                }

                this.txtTotal.EditValue = this._listDetalleFact.Sum(x => x.Movimiento.Valor1LOC.Value);

                decimal? vlrFactura = this._listDetalleFact.Sum(x => x.Movimiento.Valor1LOC.Value);
                decimal porcNuevaEntrega = this.vlrProyectoTotal != 0 ? (vlrFactura.Value) * 100 / this.vlrProyectoTotal : 0;
                this.txtPorcFacturaActual.EditValue = porcNuevaEntrega;
                if (this.txtVlrAmortizacion.Visible && vlrFactura <= this.vlrProyectoTotal)
                    this.txtVlrAmortizacion.EditValue = Math.Round(Convert.ToDecimal(this.vlrAnticipoCtaCobro, CultureInfo.InvariantCulture) * (porcNuevaEntrega / 100), 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFactura", "LoadData"));
            }
        }

        /// <summary>
        /// Refrescar Formulario
        /// </summary>
        private void RefreshForm()
        { 
            this._ctrlProyecto = null;
            this._numeroDocProy = 0;
            this._rowTareaClienteCurrent = null;
            this._listTareasAll = new List<DTO_pyProyectoTarea>();
            this._listTareasAdic = new List<DTO_pyProyectoTarea>();
            this._listEntregablesProy = new List<DTO_pyProyectoTareaCliente>();
            this._listTareasActa = new  List<DTO_pyProyectoTareaCliente>();
            this._listActaDeta = new  List<DTO_pyActaEntregaDeta>();
            this._listActaDetaExist = new List<DTO_pyActaEntregaDeta>();
            this._listDetalleFact = new List<DTO_faFacturacionFooter>();
            this.gcHeader.DataSource = null;
            this.gcHeader.RefreshDataSource();
            this.gcFooterFact.DataSource = null;    
            this.gcFooterFact.RefreshDataSource();
            this.ucProyecto.CleanControl();
            this.masterBodega.Value = string.Empty;     
            this.txtVlrAmortizacion.EditValue = 0;
            this.txtPorcFacturaActual.EditValue = 0;
            this.txtPorRetegarantia.EditValue = 0;
            this.txtTotal.EditValue = 0;
            this.vlrProyectoTotal = 0;
            this.vlrEjecutadoProy = 0;
            this.porcEntregado = 0;
            this.vlrAnticipoCtaCobro = 0;
            this.isValid = true;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            InitializeComponent();
            this.frmModule = ModulesPrefix.py;
            this.documentID = AppDocuments.ActaEntregaPreFactVenta;
            this.AddGridCols();
            this.InitControls();

            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
        }

        /// <summary>
        /// Obtiene el valor del anticipo
        /// </summary>
        private Tuple<decimal,decimal> GetSaldoProyecto()
        {
            try
            {
                if (this._headerProy != null)
                {
                    this.vlrEjecutadoProy = 0;
                    string servicioCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioCtaCobro);
                    string tipoFacturaCtaCobro = this._bc.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_TipoFacturaCtaCobro);

                    //Trae las facturas de ventas aprobadas del proyecto
                    DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                    filter.DatoAdd4.Value = this._headerProy.NumeroDoc.Value.ToString();
                    filter.ProyectoID.Value = this._ctrlProyecto.ProyectoID.Value;
                    List<DTO_glMovimientoDeta> mvtos = this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, false);
                    mvtos.AddRange(this._bc.AdministrationModel.glMovimientoDeta_GetByParameter(filter, true));
                    this.vlrAnticipoCtaCobro = mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.FacturaTipoID.Value == tipoFacturaCtaCobro && x.ServicioID.Value == servicioCtaCobro && !x.DatoAdd5.Value.Equals("INV")).Sum(x => x.Valor1LOC.Value.Value);
                    mvtos = mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.DocSoporte.Value.HasValue && !x.DatoAdd5.Value.Equals("INV"));

                    decimal ivaProy = this._headerProy.PorIVA.Value.HasValue ? this._headerProy.PorIVA.Value.Value / 100 : 0;
                    //Recorre las Facturas de Venta con tareas relacionadas
                    foreach (DTO_glMovimientoDeta mvto in mvtos)
                    {
                        //Valida si la tarea esta relacionada
                        if (this._listTareasAll.Exists(x => x.Consecutivo.Value == mvto.DocSoporte.Value))
                            this.vlrEjecutadoProy += mvto.Valor1LOC.Value.Value;
                    }

                    this.vlrEjecutadoProy = Math.Round(this.vlrEjecutadoProy, 0);
                    this.vlrProyectoTotal = Math.Round(this._listTareasAll.Sum(x => x.CostoLocalCLI.Value.Value), 0);
                    this.porcEntregado = this.vlrProyectoTotal != 0 ? this.vlrEjecutadoProy * 100 / this.vlrProyectoTotal : 0;

                    this.txtPorRetegarantia.EditValue = this._headerProy.PorcRteGarantia.Value.HasValue? this._headerProy.PorcRteGarantia.Value : 0;
                    decimal vlrAmortizacion = Convert.ToDecimal(this.txtVlrAmortizacion.EditValue, CultureInfo.InvariantCulture);
                    decimal porcAmortiza = Convert.ToDecimal(this.txtPorcFacturaActual.EditValue, CultureInfo.InvariantCulture);
                    return new Tuple<decimal, decimal>(vlrAmortizacion, Math.Round(porcAmortiza, 2));
                }
                else
                    return null;      
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-FacturaVenta.cs", "GetSaldoProyecto"));
                return null;
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
                if (this.cliente == null)
                    this.cliente = this._headerProy != null ? (DTO_faCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, false, this._headerProy.ClienteID.Value, true) : null;
                DTO_coCuentaSaldo saldoFilter = new DTO_coCuentaSaldo();
                saldoFilter.PeriodoID.Value = DateTime.Now.Date.Month == periodo.Month? periodo : new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
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
                this.txtSaldoAnticipo.EditValue = saldoAntic;

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
                FormProvider.Master.itemSave.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSendtoAppr.ToolTipText = this._bc.GetResource(LanguageTypes.ToolBar, "acc_approve");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta", "Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta", "Form_FormClosed"));
            }
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
                    this._headerProy = this.ucProyecto.ProyectoInfo.HeaderProyecto;
                    this._listEntregablesProy = this._bc.AdministrationModel.pyProyectoTareaCliente_GetByNumeroDoc(this._numeroDocProy, string.Empty, string.Empty);

                    DTO_glConsulta consultaBod = new DTO_glConsulta();
                    List<DTO_glConsultaFiltro> filtrosCargos = new List<DTO_glConsultaFiltro>();
                    filtrosCargos.Add(new DTO_glConsultaFiltro()
                    {
                        CampoFisico = "ProyectoID",
                        OperadorFiltro = OperadorFiltro.Igual,
                        ValorFiltro = this._ctrlProyecto.ProyectoID.Value
                    });
                    consultaBod.Filtros = filtrosCargos;
                    long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.inBodega, consultaBod, null, true);
                    List<DTO_inBodega> masterBod = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.inBodega, count, 1, consultaBod, null, true).Cast<DTO_inBodega>().ToList();
                    if (masterBod.Count > 0)
                        this.masterBodega.Value = masterBod.First().ID.Value;

                    this.gcHeader.DataSource = null;
                    this.gcFooterFact.DataSource = null;

                    this.GetSaldoProyecto();
                    this.LoadActas();                   
                    this.GetSaldoAnticipo();
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidDocument));
                    this._ctrlProyecto = new DTO_glDocumentoControl();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPrefacturaVenta", "LoadData"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResumenEjecucion_Click(object sender, EventArgs e)
        {
            ModalResumenActaEjecucion frm = new ModalResumenActaEjecucion(this.ucProyecto.ProyectoInfo, this._listEntregablesProy, this._listDetalleFact);
            frm.ShowDialog();
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
                    this._rowTareaClienteCurrent = (DTO_pyProyectoTareaCliente)this.gvHeader.GetRow(e.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta.cs", "gvDocument_FocusedRowChanged"));
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
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            GridColumn col = this.gvHeader.Columns[this.unboundPrefix + fieldName];

            if (fieldName == "ValorFactura")
            {
                this._rowTareaClienteCurrent.ValorFactura.Value = this._rowTareaClienteCurrent.Detalle.Sum(x => x.ValorFactura.Value);
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
                if (fieldName == "TareaCliente")
                {
                    DTO_pyProyectoTareaCliente tarea = this._listEntregablesProy.Find(x => x.TareaEntregable.Value == e.Value.ToString());
                    if (tarea != null)
                    {
                        this._rowTareaClienteCurrent.Descripcion.Value = tarea.Descripcion.Value;
                        this._rowTareaClienteCurrent.ValorFactura.Value = tarea.ValorFactura.Value != null? tarea.ValorFactura.Value : 0;
                        this._rowTareaClienteCurrent.Observaciones.Value = tarea.Observaciones.Value;
                        this._rowTareaClienteCurrent.Detalle = tarea.Detalle;
                        this._rowTareaClienteCurrent.Detalle.ForEach(x =>
                            {
                                x.PorEntregado.Value = this._listActaDeta.FindAll(y => y.ConsTareaEntrega.Value == x.Consecutivo.Value).Sum(z => z.PorEntregado.Value);
                                x.PorPendiente.Value = x.PorEntrega.Value- x.PorEntregado.Value;
                                x.ValorAEntregar.Value = x.PorEntrega.Value != 0? (x.PorPendiente.Value * x.ValorFactura.Value) / x.PorEntrega.Value : 0;
                            });
                        this._rowTareaClienteCurrent.ValorAEntregar.Value = this._rowTareaClienteCurrent.Detalle.Sum(x => x.ValorAEntregar.Value);
                    }
                    this.gvHeader.RefreshData();
                }               
                else if (fieldName == "SelectInd")
                {
                    this.gvHeader.PostEditor();
                    this._rowTareaClienteCurrent.SelectInd.Value = Convert.ToBoolean(e.Value);
                    this.LoadDetailFactura();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntregaPreFacturaVenta.cs", "gvHeader_CellValueChanging"));
            }
        }

        #endregion

        #region Eventos Grilla Detalle

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvFooterFact_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            //if (!this.disableValidate)
            //{
            //    bool validRow = this.deleteOP ? true : this.ValidateRow(e.RowHandle);
            //    this.deleteOP = false;

            //    if (validRow)
            //    {
            //        this.isValid = true;
            //    }
            //    else
            //    {
            //        e.Allow = false;
            //        this.isValid = false;
            //    }
            //}
        }

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
            this.txtTotal.EditValue = 0;
            this.txtSaldoAnticipo.EditValue = 0;
            this.txtPorRetegarantia.EditValue = 0;
            this.txtVlrAmortizacion.EditValue = 0;
        }

        /// <summary>
        /// Enviar a aprobación
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.gvHeader.PostEditor();

            //trae el anticipo
            var anticipo = this.GetSaldoProyecto();
            this._headerProy.VlrAnticipoFactVenta = anticipo.Item1;
            this._headerProy.VlrPorcAnticipo = anticipo.Item2;

            decimal saldoAnticipo = this.GetSaldoAnticipo();
            if (this._headerProy.VlrAnticipoFactVenta > saldoAnticipo && saldoAnticipo != 0)
            {
                MessageBox.Show("El valor de la amortización de anticipo supera el saldo del anticipo real del proyecto. Saldo: $" + saldoAnticipo.ToString("n2"));
                return;
            }

            string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
            string msgAprobar = string.Format(msgDoc, "Aprobar");

            if (MessageBox.Show(msgAprobar, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Thread process = new Thread(this.SendToApproveThread);
                process.Start();
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Envia al paso de Analisis de Tiempos
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
               
                var res = this._bc.AdministrationModel.ActaEntrega_ApprovePreFactura(this.documentID, this._ctrlProyecto, this._headerProy, this._listTareasActa, this._listDetalleFact);

                bool isOK = _bc.SendDocumentMail(MailType.NotSend, AppDocuments.FacturaVenta, this._bc.AdministrationModel.User.ID.Value, res, true,true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ActaEntregaPrefacturaVta.cs-SendToApproveThread"));
            }
            finally
            {
                this.Invoke(this.sendToApproveDelegate);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }


        #endregion

    }
}
