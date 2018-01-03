using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using NewAge.DTO.UDT;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Collections;
using SentenceTransformer;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class AprobacionOrdenCompra : FormWithToolbar
    {
        #region Delegados

        public delegate void RefreshData();
        public RefreshData refreshData;

        /// <summary>
        /// Delegado que finaliza el proceso de aprobacion
        /// </summary>
        public void RefreshDataMethod()
        {
            this.LoadData();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        List<DTO_prOrdenCompraAprob> _docs = null;
        //private int userID = 0;

        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private string unboundPrefixEP = "Unbound_";
        private string unboundPrefixDet = "Unbound_";
        private string unboundPrefixCarg = "Unbound_";        
        private bool multiMoneda;
        private DTO_prOrdenCompraAprob currentDoc = null;
        private DTO_prOrdenCompraAprobDet currentDet = null;
        private DTO_prSolicitudCargos currentCargo = null;
        private bool detailsLoaded = false;
        private bool detFooterLoaded = false;
        private int numDetails = 0;
        private int currentRow = -1;
        private int currentDetRow = -1;
        private int currentCargoRow = -1;
        private decimal _tasaCambio = 0;
        private DTO_glActividadPermiso tareaPerm;
        private bool allowValidate = true;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        #endregion

        public AprobacionOrdenCompra()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                //this.AfterInitialize();

                this.numDetails = Convert.ToInt32(_bc.GetControlValue(AppControl.PaginadorAprobacionDocumentos));

                //Asigna la lista de columnas
                this.AddDocumentCols();
                //this.AddEstadoPreCols();
                this.AddDetailCols();
                this.AddDetFooterCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    this.actividadFlujoID = actividades[0];
                    this.LoadData();
                }

                if (!string.IsNullOrWhiteSpace(this.actividadFlujoID))
                    this.actividadDTO = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, this.actividadFlujoID, true);
                #endregion
                this.refreshData = new RefreshData(RefreshDataMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "AprobacionOrdenCompra"));
            }
        }

        #region  Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            //this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.documentID = AppDocuments.OrdenCompAprob;
            this.frmModule = ModulesPrefix.pr;

            this.gcDocuments.ShowOnlyPredefinedDetails = true;

            _bc.InitMasterUC(this.masterLocalidad, AppMasters.glLocFisica, true, true, true, false);
            this.masterLocalidad.EnableControl(false);

            this.txtTotalMdaLocal.Enabled = false;
            this.txtTotalMdaExt.Enabled = false;

            //this.gcDetFooter.Enabled = false;
        }

        /// <summary>
        /// Carga la información del formulario
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.masterLocalidad.Value = string.Empty;

                this.txtTotalMdaExt.EditValue = 0;
                this.txtTotalMdaLocal.EditValue = 0;
                this.txtObservaciones.Text = string.Empty;
                this.txtInstrucciones.Text = string.Empty;
                this.txtObservOC.Text = string.Empty;

                List<DTO_prOrdenCompraAprob> temp = _bc.AdministrationModel.OrdenCompra_GetPendientesByModulo(this.documentID, this.actividadFlujoID, this._bc.AdministrationModel.User);
                this._docs = temp;
                foreach (DTO_prOrdenCompraAprob doc in this._docs)
                {                   
                    doc.FileUrl = string.Empty;
                }
                    
                this.LoadDocuments();

                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "LoadData.cs"));
            }
        }

        /// <summary>
        /// Carga la información en la grilla de decomentos
        /// </summary>
        private void LoadDocuments()
        {
            try
            {
                this.currentDoc = null;
                this.currentRow = -1;
                this.gcDocuments.DataSource = null;

                if (this._docs != null && this._docs.Count > 0)
                {
                    this.detailsLoaded = false;
                    this.allowValidate = false;
                    this.currentRow = 0;
                    this.gcDocuments.DataSource = this._docs;
                    this.allowValidate = true;

                    if (!detailsLoaded)
                    {
                        this.currentDoc = (DTO_prOrdenCompraAprob)this.gvDocuments.GetRow(this.currentRow);
                        this.LoadDetails();
                    }

                    string monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);
                    #region Assignar valores
                    this.masterLocalidad.Value = this.currentDoc.LugarEntrega.Value;
                    this.txtTotalMdaLocal.EditValue = this.currentDoc.ValorTotalML.Value.Value;
                    if (this.multiMoneda)
                    {
                        this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(monedaExtranjera, this.currentDoc.Fecha.Value.Value);
                        this.txtTotalMdaExt.EditValue = _tasaCambio != 0 ? this.currentDoc.ValorTotalML.Value.Value / _tasaCambio : 0;
                    }
                    else
                    {
                        this.lblTotalMdaExt.Visible = false;
                        this.txtTotalMdaExt.Visible = false;
                    }
                    this.txtObservOC.Text = this.currentDoc.ObservacionOC.Value;
                    this.txtInstrucciones.Text = this.currentDoc.Instrucciones.Value;
                    #endregion
                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.gcDetails.DataSource = null;
                    this.gcDetFooter.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "LoadDocuments.cs"));
            }
        }

        /// <summary>
        /// Carga la información sn la grilla del detalle
        /// </summary>
        private void LoadDetails()
        {
            try
            {
                this.currentDet = null;
                this.currentDetRow = -1;

                DTO_prOrdenCompraAprob doc = this.currentDoc;
                string prefijo = doc.PrefijoID.Value;
                int ordenNro = doc.DocumentoNro.Value.Value;

                //List<DTO_prSolicitudAsignDet> details = null;
                if (doc != null && this._docs.Exists(d => d.NumeroDoc.Value == doc.NumeroDoc.Value) 
                    && this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet.Count != 0)
                {
                    this.detFooterLoaded = false;
                    this.currentDetRow = 0;
                    //details = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet;
                    this.gcDetails.DataSource = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet;
                    this.currentDet = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).OrdenCompraAprobDet[this.currentDetRow];
                    
                    this.LoadDetFooter();
                    this.gvDetails.MoveFirst();
                }
                else
                {
                    //details = new List<DTO_prSolicitudAsignDet>();
                    this.gcDetails.DataSource = null;
                    this.gcDetFooter.DataSource = null;
                }

                //this.gcDetails.DataSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "LoadDetails.cs"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla del detalle
        /// </summary>
        private void LoadDetFooter()
        {
            try
            {
                this.currentCargo = null;
                this.currentCargoRow = -1;

                DTO_pyProyectoMvto filter = new DTO_pyProyectoMvto();
                
                foreach (DTO_prOrdenCompraAprobDet d in this.currentDoc.OrdenCompraAprobDet)
                {
                    filter.Consecutivo.Value = d.Detalle4ID.Value;
                    List< DTO_pyProyectoMvto> mvtos = this._bc.AdministrationModel.pyProyectoMvto_GetParameter(filter);
                    mvtos = mvtos.OrderByDescending(x=>x.Version.Value).ToList();
                    if (mvtos.Count > 0)
                    {
                        d.PresupuestoUni.Value = mvtos.First().CostoLocal.Value;
                        d.PresupuestoTotML.Value = d.PresupuestoUni.Value * d.CantidadOC.Value;
                    }
                }
                this.gvDetails.RefreshData();

                DTO_prOrdenCompraAprobDet det = this.currentDet;

                if (det != null && det.SolicitudCargos.Count > 0)
                {
                    this.currentCargoRow = 0;
                    this.gcDetFooter.DataSource = det.SolicitudCargos;
                    this.currentCargo = det.SolicitudCargos[this.currentCargoRow];
                    #region Assignar valores
                    this.txtObservaciones.Text = this.currentDet.Descriptivo.Value;
                    #endregion
                    this.gvDetFooter.MoveFirst();
                }
                else
                    this.gcDetFooter.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "LoadDetFooter.cs"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddDocumentCols()
        {
            try
            {
                //Aprobar
                GridColumn aprob = new GridColumn();
                aprob.FieldName = this.unboundPrefix + "Aprobado";
                aprob.Caption = "√";
                aprob.UnboundType = UnboundColumnType.Boolean;
                aprob.VisibleIndex = 0;
                aprob.Width = 15;
                aprob.Visible = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(aprob);

                //Rechazar
                GridColumn noAprob = new GridColumn();
                noAprob.FieldName = this.unboundPrefix + "Rechazado";
                noAprob.Caption = "X";
                noAprob.UnboundType = UnboundColumnType.Boolean;
                noAprob.VisibleIndex = 1;
                noAprob.Width = 15;
                noAprob.Visible = true;
                noAprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Rechazado");
                noAprob.AppearanceHeader.ForeColor = Color.Red;
                noAprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                noAprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                noAprob.AppearanceHeader.Options.UseTextOptions = true;
                noAprob.AppearanceHeader.Options.UseFont = true;
                noAprob.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Prefijo - Documento Numero
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 2;
                prefDoc.Width = 50;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(prefDoc);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 3;
                fecha.Width = 50;
                fecha.Visible = true;
                fecha.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(fecha);

                //ProveedorID
                GridColumn prov = new GridColumn();
                prov.FieldName = this.unboundPrefix + "ProveedorID";
                prov.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID");
                prov.UnboundType = UnboundColumnType.String;
                prov.VisibleIndex = 4;
                prov.Width = 80;
                prov.Visible = true;
                prov.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(prov);

                //ProveedorNombre
                GridColumn provName = new GridColumn();
                provName.FieldName = this.unboundPrefix + "ProveedorNombre";
                provName.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorNombre");
                provName.UnboundType = UnboundColumnType.String;
                provName.VisibleIndex = 5;
                provName.Width = 140;
                provName.Visible = true;
                provName.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(provName);

                //MonedaOrden
                GridColumn monedaOrden = new GridColumn();
                monedaOrden.FieldName = this.unboundPrefix + "MonedaOrden";
                monedaOrden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaOrden");
                monedaOrden.UnboundType = UnboundColumnType.String;
                monedaOrden.VisibleIndex = 6;
                monedaOrden.Width = 50;
                monedaOrden.Visible = true;
                monedaOrden.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(monedaOrden);

                //MonedaPago
                GridColumn monedaPago = new GridColumn();
                monedaPago.FieldName = this.unboundPrefix + "MonedaPago";
                monedaPago.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaPago");
                monedaPago.UnboundType = UnboundColumnType.String;
                monedaPago.VisibleIndex = 7;
                monedaPago.Width = 50;
                monedaPago.Visible = true;
                monedaPago.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(monedaPago);

                //ValorTotalML
                GridColumn valor = new GridColumn();
                valor.FieldName = this.unboundPrefix + "ValorTotalML";
                valor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotalML");
                valor.UnboundType = UnboundColumnType.Decimal;
                valor.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valor.AppearanceCell.Options.UseTextOptions = true;
                valor.VisibleIndex = 8;
                valor.Width = 90;
                valor.Visible = true;
                valor.ColumnEdit = this.editSpin;
                valor.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valor);

                //ValorTotalME
                GridColumn valorME = new GridColumn();
                valorME.FieldName = this.unboundPrefix + "ValorTotalME";
                valorME.Caption = _bc.GetResource(LanguageTypes.Forms, "Total Mda Extranj.");
                valorME.UnboundType = UnboundColumnType.Decimal;
                valorME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorME.AppearanceCell.Options.UseTextOptions = true;
                valorME.VisibleIndex = 9;
                valorME.Width = 90;
                valorME.Visible = true;
                valorME.ColumnEdit = this.editSpin;
                valorME.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(valorME);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms,"Observación Rechazo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 9;
                desc.Width = 120;
                desc.Visible = true;
                this.gvDocuments.Columns.Add(desc);

                //Archivo
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "ViewDoc";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.ColumnEdit = this.editLink;
                file.VisibleIndex = 10;
                file.Visible = true;
                file.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddDetailCols()
        {
            try
            {
                #region Columnas basicas

                //PrefDocSol
                GridColumn prefDocSol = new GridColumn();
                prefDocSol.FieldName = this.unboundPrefixDet + "PrefDocSol";
                prefDocSol.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDocSol");
                prefDocSol.UnboundType = UnboundColumnType.String;
                prefDocSol.VisibleIndex = 0;
                prefDocSol.Width = 70; 
                prefDocSol.Visible = true;
                prefDocSol.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(prefDocSol);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefixDet + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 60;
                codBS.Visible = true;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codBS);

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefixDet + "inReferenciaID";
                codRef.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 80;
                codRef.Visible = true;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefixDet + "MarcaInvID";
                MarcaInvID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 3;
                MarcaInvID.Width = 60;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(MarcaInvID);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefixDet + "RefProveedor";
                RefProveedor.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 4;
                RefProveedor.Width = 60;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(RefProveedor);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefixDet + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 5;
                desc.Width = 250;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(desc);              
                #endregion
                #region Columnas Visible               

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefixDet + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.VisibleIndex = 7;
                unidad.Width = 50;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(unidad);

                //Cantidad OrdenCompra
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefixDet + "CantidadOC";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadOC");
                cant.UnboundType = UnboundColumnType.Integer;
                cant.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cant.AppearanceCell.Options.UseTextOptions = true;
                cant.VisibleIndex = 8;
                cant.Width = 70;
                cant.Visible = true;
                cant.ColumnEdit = this.editValue2Cant;
                cant.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(cant);

                //ValorUni
                GridColumn valorUni = new GridColumn();
                valorUni.FieldName = this.unboundPrefixDet + "ValorUni";
                valorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                valorUni.UnboundType = UnboundColumnType.Decimal;
                valorUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorUni.AppearanceCell.Options.UseTextOptions = true;
                valorUni.VisibleIndex = 9;
                valorUni.Width = 75;
                valorUni.Visible = true;
                valorUni.ColumnEdit = this.editSpin;
                valorUni.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(valorUni);

                //ValorTotML
                GridColumn valorTotalML = new GridColumn();
                valorTotalML.FieldName = this.unboundPrefixDet + "ValorTotML";
                valorTotalML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotalML.UnboundType = UnboundColumnType.Decimal;
                valorTotalML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotalML.AppearanceCell.Options.UseTextOptions = true;
                valorTotalML.VisibleIndex = 10;
                valorTotalML.Width = 95;
                valorTotalML.Visible = true;
                valorTotalML.ColumnEdit = this.editSpin;
                valorTotalML.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(valorTotalML);

                //IvaTotML
                GridColumn ivaTotal = new GridColumn();
                ivaTotal.FieldName = this.unboundPrefixDet + "IvaTotML";
                ivaTotal.Caption = _bc.GetResource(LanguageTypes.Forms, "Iva Total ML");
                ivaTotal.UnboundType = UnboundColumnType.Decimal;
                ivaTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ivaTotal.AppearanceCell.Options.UseTextOptions = true;
                ivaTotal.VisibleIndex = 11;
                ivaTotal.Width = 100;
                ivaTotal.Visible = true;
                ivaTotal.ColumnEdit = this.editSpin;
                ivaTotal.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(ivaTotal);

                //ValorTotME
                GridColumn valorTotalME = new GridColumn();
                valorTotalME.FieldName = this.unboundPrefixDet + "ValorTotME";
                valorTotalME.Caption = _bc.GetResource(LanguageTypes.Forms, "Vlr Total ME");
                valorTotalME.UnboundType = UnboundColumnType.Decimal;
                valorTotalME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotalME.AppearanceCell.Options.UseTextOptions = true;
                valorTotalME.VisibleIndex = 12;
                valorTotalME.Width = 70;
                valorTotalME.Visible = true;
                valorTotalME.ColumnEdit = this.editSpin;
                valorTotalME.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(valorTotalME);

                //IvaTotalME
                GridColumn ivaTotalME = new GridColumn();
                ivaTotalME.FieldName = this.unboundPrefixDet + "IvaTotME";
                ivaTotalME.Caption = _bc.GetResource(LanguageTypes.Forms, "Iva Total ME");
                ivaTotalME.UnboundType = UnboundColumnType.Decimal;
                ivaTotalME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                ivaTotalME.AppearanceCell.Options.UseTextOptions = true;
                ivaTotalME.VisibleIndex = 13;
                ivaTotalME.Width = 70;
                ivaTotalME.Visible = true;
                ivaTotalME.ColumnEdit = this.editSpin;
                ivaTotalME.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(ivaTotalME);

                //PresupuestoUni
                GridColumn PresupuestoUni = new GridColumn();
                PresupuestoUni.FieldName = this.unboundPrefixDet + "PresupuestoUni";
                PresupuestoUni.Caption = _bc.GetResource(LanguageTypes.Forms, "Presup. Uni ");
                PresupuestoUni.UnboundType = UnboundColumnType.Decimal;
                PresupuestoUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                PresupuestoUni.AppearanceCell.Options.UseTextOptions = true;
                PresupuestoUni.VisibleIndex = 14;
                PresupuestoUni.Width = 85;
                PresupuestoUni.Visible = true;
                PresupuestoUni.ColumnEdit = this.editSpin;
                PresupuestoUni.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(PresupuestoUni);

                //IvaTotalME
                GridColumn PresupuestoTotML = new GridColumn();
                PresupuestoTotML.FieldName = this.unboundPrefixDet + "PresupuestoTotML";
                PresupuestoTotML.Caption = _bc.GetResource(LanguageTypes.Forms, "Presup. Total ");
                PresupuestoTotML.UnboundType = UnboundColumnType.Decimal;
                PresupuestoTotML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                PresupuestoTotML.AppearanceCell.Options.UseTextOptions = true;
                PresupuestoTotML.VisibleIndex = 15;
                PresupuestoTotML.Width = 80;
                PresupuestoTotML.Visible = true;
                PresupuestoTotML.ColumnEdit = this.editSpin;
                PresupuestoTotML.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(PresupuestoTotML);
                #endregion
                #region Columnas No Visibles

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixDet + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetails.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefixDet + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDetails.Columns.Add(solDocu);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefixDet + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDetails.Columns.Add(solDeta);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixDet + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //SolicitudDocuID
                GridColumn ocDocu = new GridColumn();
                ocDocu.FieldName = this.unboundPrefixDet + "OrdCompraDocuID";
                ocDocu.UnboundType = UnboundColumnType.Integer;
                ocDocu.Visible = false;
                this.gvDetails.Columns.Add(ocDocu);

                //SolicitudDetaID
                GridColumn ocDeta = new GridColumn();
                ocDeta.FieldName = this.unboundPrefixDet + "OrdCompraDetaID";
                ocDeta.UnboundType = UnboundColumnType.Integer;
                ocDeta.Visible = false;
                this.gvDetails.Columns.Add(ocDeta);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefixDet + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Asigna la lista de columnas del footer del detalle
        /// </summary>
        private void AddDetFooterCols()
        {
            try
            {
                #region Columnas Visibles
                //Proyecto
                GridColumn proyecto = new GridColumn();
                proyecto.FieldName = this.unboundPrefixCarg + "ProyectoID";
                proyecto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                proyecto.UnboundType = UnboundColumnType.String;
                proyecto.VisibleIndex = 1;
                proyecto.Width = 200;
                proyecto.Visible = true;
                proyecto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(proyecto);

                //Centro de costo
                GridColumn ctoCosto = new GridColumn();
                ctoCosto.FieldName = this.unboundPrefixCarg + "CentroCostoID";
                ctoCosto.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CentroCostoID");
                ctoCosto.UnboundType = UnboundColumnType.String;
                ctoCosto.VisibleIndex = 2;
                ctoCosto.Width = 200;
                ctoCosto.Visible = true;
                ctoCosto.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(ctoCosto);

                //Centro de costo
                GridColumn percent = new GridColumn();
                percent.FieldName = this.unboundPrefixCarg + "PorcentajeID";
                percent.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PorcentajeID");
                percent.UnboundType = UnboundColumnType.Decimal;
                percent.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                percent.AppearanceCell.Options.UseTextOptions = true;
                percent.VisibleIndex = 3;
                percent.Width = 200;
                percent.Visible = true;
                percent.ColumnEdit = this.editValue2Cant;
                percent.OptionsColumn.AllowEdit = true;
                this.gvDetFooter.Columns.Add(percent);
                #endregion
                #region Columnas No Visibles
                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefixCarg + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetFooter.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixCarg + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetFooter.Columns.Add(consDeta);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        protected virtual bool ValidateDocRow(int fila)
        {
            if (fila >= 0)
            {
                string rsxEmpty = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                GridColumn col = this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"];
                bool rechazado = (bool)this.gvDocuments.GetRowCellValue(fila, col);

                if (rechazado)
                {
                    col = this.gvDocuments.Columns[this.unboundPrefix + "Observacion"];
                    string desc = this.gvDocuments.GetRowCellValue(fila, col).ToString();

                    if (string.IsNullOrEmpty(desc))
                    {
                        string msg = string.Format(rsxEmpty, col.Caption);
                        this.gvDocuments.SetColumnError(col, msg);
                        return false;
                    }
                }
                
                this.gvDocuments.SetColumnError(col, string.Empty); 
            }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Documentos

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this.unboundPrefix.Length;
            if (dataType == typeof(DTO_prSolicitudCargos))
                unboundPrefixLen = this.unboundPrefixCarg.Length;
            if (dataType == typeof(DTO_prOrdenCompraAprobDet))
                unboundPrefixLen = this.unboundPrefixDet.Length;

            string fieldName = e.Column.FieldName.Substring(unboundPrefixLen);

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
        /// Se ejecuta cuando se cambia la fila de los documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.currentRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                    this.currentRow = e.FocusedRowHandle;

                this.currentDoc = (DTO_prOrdenCompraAprob)this.gvDocuments.GetRow(this.currentRow);
                this.LoadDetails();
                this.txtObservOC.Text = this.currentDoc.ObservacionOC.Value;
                this.txtInstrucciones.Text = this.currentDoc.Instrucciones.Value;
                this.txtTotalMdaLocal.EditValue = this.currentDoc.ValorTotalML.Value.Value;
                this.masterLocalidad.Value = this.currentDoc.LugarEntrega.Value;
                if (this.multiMoneda)
                    this.txtTotalMdaExt.EditValue = _tasaCambio != 0 ? this.currentDoc.ValorTotalML.Value.Value / _tasaCambio : 0;
                
                this.detailsLoaded = true;
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    this._docs[e.RowHandle].Rechazado.Value = false;
                    this.txtObservOC.Text = string.Empty;
                    this._docs[e.RowHandle].Observacion.Value = string.Empty;
                }
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    this._docs[e.RowHandle].Aprobado.Value = false;
                }
                else
                {
                    this.txtObservOC.Text = string.Empty;
                    this._docs[e.RowHandle].Observacion.Value = string.Empty;
                }
            };

            this.gcDocuments.RefreshDataSource();
            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Calcula los valores y hace operacines cuando los valores etstan engresados
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {

            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (fieldName == "Observacion" && e.Value.ToString() != this.txtObservOC.Text)
            {
                this.txtObservOC.Text = e.Value.ToString();
            }

            this.ValidateDocRow(e.RowHandle);
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (!this.allowValidate || !this.ValidateDocRow(this.currentRow))
                e.Allow = false;
        }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ViewDoc")
                e.DisplayText = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ViewDocument);
        }
        #endregion

        #region Eventos grilla de Detalles

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.gvDocuments.DataRowCount > 0 && this.currentDetRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDetails.RowCount - 1)
                    this.currentDetRow = e.FocusedRowHandle;

                this.currentDet = (DTO_prOrdenCompraAprobDet)this.gvDetails.GetRow(this.currentDetRow);
                this.LoadDetFooter();
                this.detFooterLoaded = true;
            }
        }

        /// <summary>
        /// Se ejecuta cuando se cambia la fila de los detalles
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetFooter_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (this.gvDocuments.DataRowCount > 0 && this.currentCargoRow != -1)
            {
                if (e.FocusedRowHandle <= this.gvDetFooter.RowCount - 1)
                    this.currentCargoRow = e.FocusedRowHandle;

                this.currentCargo = (DTO_prSolicitudCargos)this.gvDetFooter.GetRow(this.currentCargoRow);
            }
        }
      
        #endregion
                
        #region Eventos Barra de Herramientas

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

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            this.gvDocuments.PostEditor();
            this.gvDetails.PostEditor();
            try
            {
                if (this._docs != null && this._docs.Count != 0)
                {
                     if (this.currentDet != null)
                    {
                        foreach (DTO_prOrdenCompraAprob ord in this._docs.FindAll(x=>x.Aprobado.Value == true))
                        {
                            bool validValor = ord.OrdenCompraAprobDet.Any(x => x.ValorUni.Value == 0);
                            if (validValor)
                            {
                                MessageBox.Show("No puede aprobar items con valor unitario en $0, valide nuevamente");
                                return;
                            }                               
                        }
                    }

                    if (this.ValidateDocRow(this.currentRow))
                    {
                        Thread process = new Thread(this.SaveThread);
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Eventos Controles

        private void txtObserv_Leave(object sender, EventArgs e)
        {
            this._docs[this.currentRow].Observacion.Value = this.txtObservOC.Text;
            this.ValidateDocRow(this.currentRow);
        }

        /// <summary>
        /// Al entrar al link de la grilla para ver la descripcin de documento
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editLink_Click(object sender, EventArgs e)
        {
            try
            {
                int fila = this.gvDocuments.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._docs[fila].NumeroDoc.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOdenCompra.cs", "editLink_Click"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo que se ejecuta al asignar
        /// </summary>
        private void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                string estateCurrent = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAprobado);
                List<DTO_SerializedObject> results = _bc.AdministrationModel.OrdenCompra_AprobarRechazar(AppDocuments.OrdenCompAprob, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, false);
               
                FormProvider.Master.StopProgressBarThread(this.documentID);

                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
                List<int> docsOK = new List<int>();
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SendingMails) });
                foreach (object obj in results)
                {
                    #region Funciones de progreso
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                    percent = ((i + 1) * 100) / (results.Count);

                    if (FormProvider.Master.ProcessCanceled(this.documentID))
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser));
                        break;
                    }
                    #endregion

                    if (this._docs[i].Aprobado.Value.Value || this._docs[i].Rechazado.Value.Value)
                    {
                        MailType mType = this._docs[i].Aprobado.Value.Value ? MailType.Approve : MailType.Reject;
                        bool isOK = _bc.SendDocumentMail(mType, this.documentID, this.actividadDTO.seUsuarioID.Value, obj, false);
                        if (!isOK)
                        {
                            DTO_TxResult r = (DTO_TxResult)obj;
                            resultsNOK.Add(r);
                        }
                        else
                        {
                            DTO_Alarma r = (DTO_Alarma)obj;
                            int numDoc = Convert.ToInt32(r.NumeroDoc);
                            docsOK.Add(numDoc);
                        }
                    }
                    i++;
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

                #region Pregunta si desea abrir los reportes

                //bool deseaImp = false;
                //if (docsOK.Count > 0)
                //{
                //    string msgs = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_DeseaImprimirReporte);
                //    var result = MessageBox.Show(msgs, msgs, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //    if (result == DialogResult.Yes)
                //        deseaImp = true;
                //}

                #endregion
                #region Genera e imprime los reportes
                foreach (int item in docsOK)
                {
                    //this._bc.AdministrationModel.ReportesProveedores_OrdenCompra(item, 1, false,false);
                }
                #endregion

                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionOrdenCompra.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion         
    }
}
