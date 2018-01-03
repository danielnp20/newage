﻿using System;
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
using NewAge.Librerias.Project;
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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class AprobacionRecibido : FormWithToolbar
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
        List<DTO_prRecibidoAprob> _docs = null;
        //private int userID = 0;

        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";
        private string unboundPrefixDet = "UnboundPrefDet_";        
        private bool multiMoneda;
        private DTO_prRecibidoAprob currentDoc = null;
        private DTO_prRecibidoAprobDet currentDet = null;
        private bool detailsLoaded = false;
        private bool detFooterLoaded = false;
        private int numDetails = 0;
        private int currentRow = -1;
        private int currentDetRow = -1;
        private DTO_glActividadPermiso tareaPerm;
        private bool allowValidate = true;
        protected string actividadFlujoID = string.Empty;
        protected DTO_glActividadFlujo actividadDTO = null;

        //Variables Privadas
        private FormTypes _frmType = FormTypes.DocumentAprob;
        private string _frmName;
        private string _monedaLocal = string.Empty;
        #endregion

        public AprobacionRecibido()
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
                this.AddDetailCols();

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "AprobacionRecibido"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = _bc.AdministrationModel.Empresa.ID.Value;
            //this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;

            this.documentID = AppDocuments.RecibidoAprob;
            this.frmModule = ModulesPrefix.pr;

            this.gcDocuments.ShowOnlyPredefinedDetails = true;

            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, false);
            this._bc.InitMasterUC(this.masterProveedor_det, AppMasters.prProveedor, true, true, true, false);
            this._bc.InitMasterUC(this.masterCodigoBS_det, AppMasters.prBienServicio, true, true, true, false);
            this._bc.InitMasterUC(this.masterReferencia_det, AppMasters.inReferencia, true, true, true, false);

            //this.masterProveedor.EnableControl(false);
            this.masterProveedor_det.EnableControl(false);
            this.masterCodigoBS_det.EnableControl(false);
            this.masterReferencia_det.EnableControl(false);

            string periodo = this._bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
            this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.dtDate.DateTime = Convert.ToDateTime(periodo);
            this.dtDate.Enabled = false;

            this.txtObservRech.Enabled = false;
            this.txtObservGR.Enabled = false;

            this.gcGoodRec.Enabled = false;

            this.txtCostoLocal.Enabled = false;
            this.txtCostoExt.Enabled = false;
            this.txtIVALocal.Enabled = false;
            this.txtIVAExtr.Enabled = false;
        }

        /// <summary>
        /// Carga la información del formulario
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.masterProveedor.Value = string.Empty;
                this.masterProveedor_det.Value = string.Empty;
                this.masterCodigoBS_det.Value = string.Empty;
                this.masterReferencia_det.Value = string.Empty;

                this.txtObservRech.Text = string.Empty;
                this.txtObservGR.Text = string.Empty;

                this.rbSi.Checked = true;
                this.rbNoCump.Checked = true;

                this.txtCostoLocal.EditValue = 0;
                this.txtCostoExt.EditValue = 0;
                this.txtIVALocal.EditValue = 0;
                this.txtIVAExtr.EditValue = 0;

                List<DTO_prRecibidoAprob> temp = _bc.AdministrationModel.Recibido_GetPendientesByModulo(this.documentID, actividadFlujoID, this._bc.AdministrationModel.User);
                this._docs = temp;
                this._docs  = this._docs.OrderBy(t => t.PrefDoc).ToList();
                foreach (var item in this._docs)
                    item.FileUrl = string.Empty;
                this.LoadDocuments();
               
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Approve);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "cs-LoadData"));
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
                        this.currentDoc = (DTO_prRecibidoAprob)this.gvDocuments.GetRow(this.currentRow);
                        this.LoadDetails();
                    }                    
                    this.gvDocuments.MoveFirst();
                }
                else
                {
                    this.gcDetails.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "cs-LoadDocuments"));
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

                DTO_prRecibidoAprob doc = this.currentDoc;
                //string prefijo = doc.PrefijoID.Value;
                //int ordenNro = doc.DocumentoNro.Value.Value;

                //List<DTO_prSolicitudAsignDet> details = null;
                if (doc != null && this._docs.Exists(d => d.NumeroDoc.Value == doc.NumeroDoc.Value) 
                    && this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).Detalle.Count != 0)
                {
                    this.detFooterLoaded = false;
                    this.currentDetRow = 0;
                    //details = this._docsFiltered.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).SolicitudAsignDet;
                    this.gcDetails.DataSource = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).Detalle;
                    this.currentDet = this._docs.Find(d => d.NumeroDoc.Value == doc.NumeroDoc.Value).Detalle[this.currentDetRow];

                    
                    this.gvDetails.MoveFirst();
                }
                else
                {
                    this.gcDetails.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "AprobacionSolicitud.cs-LoadDetails"));
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
                aprob.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                aprob.AppearanceHeader.ForeColor = Color.Lime;
                aprob.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aprob.AppearanceHeader.Options.UseTextOptions = true;
                aprob.AppearanceHeader.Options.UseFont = true;
                aprob.AppearanceHeader.Options.UseForeColor = true;
                aprob.ToolTip = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Aprobado");
                aprob.OptionsColumn.AllowEdit = true;
              
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
                noAprob.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(noAprob);

                //Prefijo - Documento Numero
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc");
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 2;
                prefDoc.Width = 60;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(prefDoc);

                //Fecha
                GridColumn fecha = new GridColumn();
                fecha.FieldName = this.unboundPrefix + "Fecha";
                fecha.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Fecha");
                fecha.UnboundType = UnboundColumnType.DateTime;
                fecha.VisibleIndex = 3;
                fecha.Width = 70;
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
                provName.Width = 130;
                provName.Visible = true;
                provName.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(provName);

                //MonedaOrden
                GridColumn monedaOrden = new GridColumn();
                monedaOrden.FieldName = this.unboundPrefix + "MonedaID";
                monedaOrden.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaID");
                monedaOrden.UnboundType = UnboundColumnType.String;
                monedaOrden.VisibleIndex = 6;
                monedaOrden.Width = 50;
                monedaOrden.Visible = true;
                monedaOrden.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(monedaOrden);

                //CostoML
                GridColumn costoML = new GridColumn();
                costoML.FieldName = this.unboundPrefix + "CostoML";
                costoML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoML");
                costoML.UnboundType = UnboundColumnType.Decimal;
                costoML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                costoML.AppearanceCell.Options.UseTextOptions = true;
                costoML.VisibleIndex = 7;
                costoML.Width = 90;
                costoML.Visible = false;
                costoML.ColumnEdit = this.editSpin;
                costoML.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(costoML);

                //CostoME
                GridColumn costoME = new GridColumn();
                costoME.FieldName = this.unboundPrefix + "CostoME";
                costoME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CostoME");
                costoME.UnboundType = UnboundColumnType.Decimal;
                costoME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                costoME.AppearanceCell.Options.UseTextOptions = true;
                costoME.VisibleIndex = 8;
                costoME.Width = 90;
                costoME.Visible = false;
                costoME.ColumnEdit = this.editSpin;
                costoME.OptionsColumn.AllowEdit = false;
                this.gvDocuments.Columns.Add(costoME);

                //Observacion
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Observacion";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Observacion");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 9;
                desc.Width = 130;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = true;
                this.gvDocuments.Columns.Add(desc);

                //Documento
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "AprobacionRecibido.cs-AddGridCols"));
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

                //PrefDoc
                GridColumn PrefDoc = new GridColumn();
                PrefDoc.FieldName = this.unboundPrefixDet + "PrefDoc";
                PrefDoc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDocOC");
                PrefDoc.UnboundType = UnboundColumnType.String;
                PrefDoc.VisibleIndex = 0;
                PrefDoc.Width = 80;
                PrefDoc.Visible = true;
                PrefDoc.Fixed = FixedStyle.Left;
                PrefDoc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(PrefDoc);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefixDet + "CodigoBSID";
                codBS.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 80;
                codBS.Visible = true;
                codBS.Fixed = FixedStyle.Left;
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
                codRef.Fixed = FixedStyle.Left;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefixDet + "Descriptivo";
                desc.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 250;
                desc.Visible = true;
                codRef.Fixed = FixedStyle.Left;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(desc);

                //SerialID
                GridColumn SerialID = new GridColumn();
                SerialID.FieldName = this.unboundPrefixDet + "SerialID";
                SerialID.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                SerialID.UnboundType = UnboundColumnType.String;
                SerialID.VisibleIndex = 4;
                SerialID.Width = 80;
                SerialID.Visible = true;
                SerialID.Fixed = FixedStyle.Left;
                SerialID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(SerialID);
                #endregion
                #region Columnas extras
                #region Columnas Visible
           
                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefixDet + "UnidadInvID";
                unidad.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID");
                unidad.UnboundType = UnboundColumnType.String;
                unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                unidad.AppearanceCell.Options.UseTextOptions = true;
                unidad.VisibleIndex = 5;
                unidad.Width = 50;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(unidad);

                //Cantidad Recibido
                GridColumn cant = new GridColumn();
                cant.FieldName = this.unboundPrefixDet + "CantidadRec";
                cant.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadRec");
                cant.UnboundType = UnboundColumnType.Integer;
                cant.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cant.AppearanceCell.Options.UseTextOptions = true;
                cant.VisibleIndex = 6;
                cant.Width = 50;
                cant.Visible = true;
                cant.ColumnEdit = this.editValue2;
                cant.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(cant);

                //Documento
                GridColumn file = new GridColumn();
                file.FieldName = this.unboundPrefix + "FileUrlDet";
                file.OptionsColumn.ShowCaption = false;
                file.UnboundType = UnboundColumnType.String;
                file.Width = 60;
                file.ColumnEdit = this.editLink;
                file.VisibleIndex = 7;
                file.Visible = true;
                file.OptionsColumn.AllowEdit = true;
                this.gvDetails.Columns.Add(file);

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

                //OrdCompraDocuID
                GridColumn ocDocu = new GridColumn();
                ocDocu.FieldName = this.unboundPrefixDet + "OrdCompraDocuID";
                ocDocu.UnboundType = UnboundColumnType.Integer;
                ocDocu.Visible = false;
                this.gvDetails.Columns.Add(ocDocu);

                //OrdCompraDetaID
                GridColumn ocDeta = new GridColumn();
                ocDeta.FieldName = this.unboundPrefixDet + "OrdCompraDetaID";
                ocDeta.UnboundType = UnboundColumnType.Integer;
                ocDeta.Visible = false;
                this.gvDetails.Columns.Add(ocDeta);

                //RecibidoDocuID
                GridColumn recDocu = new GridColumn();
                recDocu.FieldName = this.unboundPrefixDet + "RecibidoDocuID";
                recDocu.UnboundType = UnboundColumnType.Integer;
                recDocu.Visible = false;
                this.gvDetails.Columns.Add(recDocu);

                //RecibidoDetaID
                GridColumn recDeta = new GridColumn();
                recDeta.FieldName = this.unboundPrefixDet + "RecibidoDetaID";
                recDeta.UnboundType = UnboundColumnType.Integer;
                recDeta.Visible = false;
                this.gvDetails.Columns.Add(recDeta);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefixDet + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefixDet + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "AprobacionSolicitud.cs-AddGridCols"));
            }
        }
        
        /// <summary>
        /// Valida las filas del documento
        /// </summary>
        private bool ValidateDocRow(int fila)
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

                col = this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"];
                bool aprobado = (bool)this.gvDocuments.GetRowCellValue(fila, col);
                if (aprobado)
                {
                    this._docs[fila].ConformidadInd.Value = this.rbSi.Checked;
                    this._docs[fila].Calificacion.Value = (this.rbNoCump.Checked) ? (byte)Calificacion.NoCumple :
                        (this.rbParc.Checked) ? (byte)Calificacion.Parcialmente : (this.rbTotal.Checked) ? (byte)Calificacion.Totalmente :
                        (byte)Calificacion.SuperaExpect;
                }
                else
                {
                    this._docs[fila].ConformidadInd.Value = null;
                    this._docs[fila].Calificacion.Value = null;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "RecibidoAprobacion.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "RecibidoAprobacion.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "RecibidoAprobacion.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "RecibidoAprobacion.cs-Form_FormClosed"));
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
            if (dataType == typeof(DTO_prRecibidoAprobDet))
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
            try
            {
                if (this.gvDocuments.DataRowCount > 0 && this.currentRow != -1)
                {
                    if (e.FocusedRowHandle <= this.gvDocuments.RowCount - 1)
                        this.currentRow = e.FocusedRowHandle;

                    this.currentDoc = (DTO_prRecibidoAprob)this.gvDocuments.GetRow(this.currentRow);
                    this.gcGoodRec.Enabled = this.currentDoc.Aprobado.Value.Value;
                    this.txtObservRech.Enabled = this.currentDoc.Rechazado.Value.Value;
                    this.LoadDetails();
                    this.gvDetails_FocusedRowChanged(sender, e);
                    this.detailsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "cs-gvDocuments_FocusedRowChanged"));
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
                    this.txtObservRech.Enabled = false;
                    this.txtObservRech.Text = string.Empty;
                    this.txtObservGR.Enabled = true;
                    this._docs[e.RowHandle].Observacion.Value = string.Empty;                    
                }
                this.gcGoodRec.Enabled = (bool)e.Value;
            }
            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    this._docs[e.RowHandle].Aprobado.Value = false;
                    this.txtObservRech.Enabled = true;
                    this.txtObservGR.Enabled = false;
                    this.txtObservGR.Text = string.Empty;
                    this.gcGoodRec.Enabled = false;
                }
                else
                {
                    this.txtObservRech.Enabled = false;
                    this.txtObservRech.Text = string.Empty;
                    this.txtObservGR.Enabled = true;
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

            //if (fieldName == "Observacion" && e.Value.ToString() != this.txtObservRech.Text)
            //{
            //    this.txtObservRech.Text = e.Value.ToString();
            //}

            //this.ValidateDocRow(e.RowHandle);
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
        private void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            if (fieldName == "ViewDoc" || fieldName == "FileUrlDet")
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

                this.currentDet = (DTO_prRecibidoAprobDet)this.gvDetails.GetRow(this.currentDetRow);

                this.masterProveedor_det.Value = this.currentDoc.ProveedorID.Value;
                this.masterCodigoBS_det.Value = this.currentDet.CodigoBSID.Value;
                this.masterReferencia_det.Value = this.currentDet.inReferenciaID.Value;
                this.txtCostoLocal.EditValue = this.currentDet.ValorUni.Value * this.currentDet.CantidadRec.Value;
                this.txtIVALocal.EditValue = this.currentDet.IVAUni.Value * this.currentDet.CantidadRec.Value;
                this.txtCostoExt.EditValue = this.multiMoneda && this.currentDoc.TasaCambioDOCU.Value != 0 ? (this.currentDet.ValorUni.Value * this.currentDet.CantidadRec.Value) / this.currentDoc.TasaCambioDOCU.Value : 0;
                this.txtIVAExtr.EditValue = this.multiMoneda && this.currentDoc.TasaCambioDOCU.Value != 0 ? (this.currentDet.IVAUni.Value * this.currentDet.CantidadRec.Value) / this.currentDoc.TasaCambioDOCU.Value : 0;
                this.detFooterLoaded = true;
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

        #region Eventos controles

        /// <summary>
        /// Se realiza al dejar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtObservRech_Leave(object sender, EventArgs e)
        {
            this._docs[this.currentRow].Observacion.Value = this.txtObservRech.Text;
            this.ValidateDocRow(this.currentRow);
        }

        /// <summary>
        /// Se realiza al dejar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtObservGR_Leave(object sender, EventArgs e)
        {
            this._docs[this.currentRow].Observacion.Value = this.txtObservGR.Text;
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
                this.currentDoc = (DTO_prRecibidoAprob)this.gvDocuments.GetRow(this.currentRow);
                int filaDet = this.gvDetails.FocusedRowHandle;

                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                DTO_Comprobante comprobante = new DTO_Comprobante();

                if(this.gvDocuments.IsFocusedView)
                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.currentDoc.NumeroDoc.Value.Value);
                else
                    ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this.currentDoc.Detalle[filaDet].OrdCompraDocuID.Value.Value);
                comprobante = !string.IsNullOrEmpty(ctrl.ComprobanteID.Value) ? this._bc.AdministrationModel.Comprobante_Get(true, false, ctrl.PeriodoDoc.Value.Value, ctrl.ComprobanteID.Value, ctrl.ComprobanteIDNro.Value.Value, null, null, null) : null;

                ShowDocumentForm documentForm = new ShowDocumentForm(ctrl, comprobante);
                documentForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "editLink_Click"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterProveedor.ValidID)
                {
                    this._docs = this._docs.FindAll(x => x.ProveedorID.Value == this.masterProveedor.Value);
                    this.LoadDocuments();
                }
                else
                {
                    this.LoadData();
                }                
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "masterProveedor_Leave"));
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
                List<DTO_SerializedObject> results = _bc.AdministrationModel.Recibido_AprobarRechazar(AppDocuments.RecibidoAprob, this.actividadFlujoID, this._bc.AdministrationModel.User, this._docs, false);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                int i = 0;
                int percent = 0;
                List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();

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
                    }
                    i++;
                }
                MessageForm frm = new MessageForm(resultsNOK);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                this.Invoke(this.refreshData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-AprobacionRecibido.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion

    }
}
