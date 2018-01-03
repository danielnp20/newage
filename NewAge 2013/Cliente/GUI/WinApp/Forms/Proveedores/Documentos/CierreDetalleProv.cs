using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using SentenceTransformer;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class CierreDetalleProv : FormWithToolbar
    {
        #region Delegados
        protected delegate void RefreshGrid();
        protected RefreshGrid refreshGridDelegate;
        /// <summary>
        /// Delegado que finaliza el proceso de importación
        /// </summary>
        protected void RefreshGridMethod()
        {
            this.LoadData();
            FormProvider.Master.itemSendtoAppr.Enabled = true;
        }

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            this.LoadData();
            FormProvider.Master.itemSave.Enabled = true;
            this.chkMarcarItem.Text = "Marcar Todos";
            this.chkMarcarItem.Checked = false;

        }
        #endregion

        #region Variables formulario

        //Para uso general de los formularios
        private BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private string _frmNewName;        
        
        //Variables de datos
        private List<DTO_prDetalleDocu> _detalle = new List<DTO_prDetalleDocu>();
        private List<DTO_prSolicitudCargos> _cargos = new List<DTO_prSolicitudCargos>();
        private DTO_prDetalleDocu _currentRow = new DTO_prDetalleDocu();
        private DTO_glDocumentoControl _dtoCtrl = new DTO_glDocumentoControl();

        //Variables para gvCargos
        private string unboundPrefixCargo = "UnboundPref_";
        private string _codBienServicio = string.Empty;

        //Variables con otros recursos
        private string _cantidadSolRsx = string.Empty;
        private string _cantidadOCRsx = string.Empty;
        private string _porcentajeRsx = string.Empty;
        private string _descRsx = string.Empty;
        private string _solCargosRsx = string.Empty;

        Dictionary<string, Tuple<DTO_prBienServicio, decimal>> cacheBienServ = new Dictionary<string, Tuple<DTO_prBienServicio, decimal>>();
        Dictionary<string, decimal> cacheBienServClase = new Dictionary<string, decimal>();
        Dictionary<string, decimal> cacheConCargo = new Dictionary<string, decimal>();
        Dictionary<string, decimal> cacheCuenta = new Dictionary<string, decimal>();

        private int userID = 0;
        //Para manejo de propiedades
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private bool multiMoneda;
        private bool disableValidate = false;
        //Internas del formulario
        private string areaFuncionalID;
        private string prefijoID;
        private string terceroID;
        private string comprobanteID;
        private bool dataLoaded = false;
        private int indexFila = 0;
        private bool isValid = true;
        private bool deleteOP = false;
        private bool newDoc = false;
        private bool newReg = false;
        private string lastColName = string.Empty;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        //Variables para importar
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        //Variables Moneda
        private string monedaLocal;
        private string monedaExtranjera;
        private string monedaId;
        private string monedaOrden;
        private TipoMoneda _tipoMonedaOr = TipoMoneda.Local;
        private decimal valorTotalDoc = 0;
        private decimal valorIVATotalDoc = 0;
        private int _daysEntr = 0;
        private decimal valorTotalPagosMes = 0;
        private decimal _vlrTasaCambio = 0;

        //variables para funciones particulares
        private bool cleanDoc = true;
      
        #endregion

        public CierreDetalleProv()
        {
            try
            {
                //this.InitializeComponent();
                this.SetInitParameters();
                this.multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                if (!string.IsNullOrWhiteSpace(this._frmNewName))
                    this._frmName = this._frmNewName;

                this.LoadDocumentInfo(true);
                this.frmModule = (ModulesPrefix.pr);
                
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                 
                }
                this.AfterInitialize();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm.cs", "DocumentForm"));
            }
        }      

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected  void SetInitParameters()
        {
            this.documentID = AppDocuments.CierreDetalleSolicitud;
            this.frmModule = ModulesPrefix.pr;
            InitializeComponent();

            //Carga info de las monedas
            this.monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this.monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);       
        
            this.gcDocument.ShowOnlyPredefinedDetails = true;
            
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true,false, true, false);
            this._bc.InitMasterUC(this.masterProveedorFilter, AppMasters.prProveedor, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterCodigoBS, AppMasters.prBienServicio, true, true, true, false);
            this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false);

            this.refreshGridDelegate = new RefreshGrid(this.RefreshGridMethod);
            this.saveDelegate = new Save(this.SaveMethod);
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected  void AfterInitialize()
        {
            this.AddGridCols();
            this.EnableFooter(false);

            this.chkDocumentos.SelectedIndex = 0;
            this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        protected void AddGridCols()
        {
            try
            {
                #region Columnas visibles
                this.editValue.Mask.EditMask = "n2";

                //prefDoc
                GridColumn prefDoc = new GridColumn();
                prefDoc.FieldName = this.unboundPrefix + "PrefDoc";
                prefDoc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDoc"); 
                prefDoc.UnboundType = UnboundColumnType.String;
                prefDoc.VisibleIndex = 0;
                prefDoc.Width = 50;
                prefDoc.Visible = true;
                prefDoc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(prefDoc);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = this._bc.GetResource(LanguageTypes.Forms, "Código BS"); 
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 1;
                codBS.Width = 60;
                codBS.Visible = true;
                codBS.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codBS);

                //inReferenciaID
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID"); 
                codRef.UnboundType = UnboundColumnType.String;
                codRef.VisibleIndex = 2;
                codRef.Width = 60;
                codRef.Visible = true;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(codRef);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo"); 
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 3;
                desc.Width = 230;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(desc);

                //RefProveedor
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_RefProveedor");
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.VisibleIndex = 4;
                RefProveedor.Width = 60;
                RefProveedor.Visible = true;
                RefProveedor.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(RefProveedor);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MarcaInvID");
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.VisibleIndex = 5;
                MarcaInvID.Width = 50;
                MarcaInvID.Visible = true;
                MarcaInvID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(MarcaInvID);      

                //UnidadInvID
                GridColumn unidad = new GridColumn();
                unidad.FieldName = this.unboundPrefix + "UnidadInvID";
                unidad.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_UnidadInvID"); ;
                unidad.UnboundType = UnboundColumnType.String;
                unidad.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                unidad.AppearanceCell.Options.UseTextOptions = true;
                unidad.VisibleIndex = 6;
                unidad.Width = 40;
                unidad.Visible = true;
                unidad.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(unidad);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID"); 
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 7;
                ProyectoID.Width = 40;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProyectoID);     

                //Cantidad total sin Solicitar(Pendiente)
                GridColumn CantidadPend = new GridColumn();
                CantidadPend.FieldName = this.unboundPrefix + "CantidadPend";
                CantidadPend.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadPend");
                CantidadPend.UnboundType = UnboundColumnType.Decimal;
                CantidadPend.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadPend.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CantidadPend.AppearanceCell.Options.UseTextOptions = true;
                CantidadPend.AppearanceCell.Options.UseFont = true;
                CantidadPend.VisibleIndex = 8;
                CantidadPend.Width = 50;
                CantidadPend.Visible = true;
                CantidadPend.ColumnEdit = this.editValue;
                CantidadPend.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(CantidadPend);

                //Cantidad a cerrar
                GridColumn CantidadCierre = new GridColumn();
                CantidadCierre.FieldName = this.unboundPrefix + "CantidadCierre";
                CantidadCierre.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadCierre");
                CantidadCierre.UnboundType = UnboundColumnType.Decimal;
                CantidadCierre.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                CantidadCierre.AppearanceCell.Options.UseTextOptions = true;
                CantidadCierre.VisibleIndex = 9;
                CantidadCierre.Width = 50;
                CantidadCierre.Visible = true;
                CantidadCierre.ColumnEdit = this.editValue;
                CantidadCierre.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(CantidadCierre);

                //ValorUni
                GridColumn valorUni = new GridColumn();
                valorUni.FieldName = this.unboundPrefix + "ValorUni";
                valorUni.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorUni");
                valorUni.UnboundType = UnboundColumnType.Decimal;
                valorUni.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorUni.AppearanceCell.Options.UseTextOptions = true;
                valorUni.VisibleIndex = 11;
                valorUni.Width = 70;
                valorUni.Visible = false;
                valorUni.ColumnEdit = this.editValue;
                valorUni.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorUni);

                //ValorTotML
                GridColumn valorTotML = new GridColumn();
                valorTotML.FieldName = this.unboundPrefix + "ValorTotML";
                valorTotML.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotML.UnboundType = UnboundColumnType.Decimal;
                valorTotML.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotML.AppearanceCell.Options.UseTextOptions = true;
                valorTotML.VisibleIndex = 12;
                valorTotML.Width = 80;
                valorTotML.Visible = false;
                valorTotML.ColumnEdit = this.editSpin;
                valorTotML.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotML);

                //ProveedorID
                GridColumn ProveedorID = new GridColumn();
                ProveedorID.FieldName = this.unboundPrefix + "ProveedorID";
                ProveedorID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProveedorID"); 
                ProveedorID.UnboundType = UnboundColumnType.String;
                ProveedorID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
                ProveedorID.AppearanceCell.Options.UseTextOptions = true;
                ProveedorID.VisibleIndex = 13;
                ProveedorID.Width = 40;
                ProveedorID.Visible = false;
                ProveedorID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(ProveedorID);

                //SolicitarInd
                GridColumn SolicitarInd = new GridColumn();
                SolicitarInd.FieldName = this.unboundPrefix + "SolicitarInd";
                SolicitarInd.Caption = "√ " + this._bc.GetResource(LanguageTypes.Forms, "Solicitar");
                SolicitarInd.UnboundType = UnboundColumnType.Boolean;
                SolicitarInd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
                SolicitarInd.AppearanceCell.Options.UseTextOptions = true;
                SolicitarInd.AppearanceHeader.Options.UseFont = true;
                SolicitarInd.AppearanceHeader.Options.UseForeColor = true;
                SolicitarInd.AppearanceHeader.ForeColor = Color.Lime;
                SolicitarInd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                SolicitarInd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                SolicitarInd.ToolTip = this._bc.GetResource(LanguageTypes.Forms, "Solicitar nuevamente");
                SolicitarInd.VisibleIndex = 14;
                SolicitarInd.Width = 40;
                SolicitarInd.Visible = false;
                this.gvDocument.Columns.Add(SolicitarInd);
                #endregion
                #region Columnas No Visible
                //valorTotME
                GridColumn valorTotME = new GridColumn();
                valorTotME.FieldName = this.unboundPrefix + "ValorTotME";
                valorTotME.Caption = _bc.GetResource(LanguageTypes.Forms, this.documentID + "_ValorTotML");
                valorTotME.UnboundType = UnboundColumnType.Decimal;
                valorTotME.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                valorTotME.AppearanceCell.Options.UseTextOptions = true;
                valorTotME.VisibleIndex = 11;
                valorTotME.Width = 80;
                valorTotME.Visible = false;
                valorTotME.ColumnEdit = this.editSpin;
                valorTotME.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(valorTotME);

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDocument.Columns.Add(numDoc);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDocument.Columns.Add(consDeta);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDocument.Columns.Add(solDocu);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDocument.Columns.Add(solDeta);

                //OrdCompraDocuID
                GridColumn ordCompDocu = new GridColumn();
                ordCompDocu.FieldName = this.unboundPrefix + "OrdCompraDocuID";
                ordCompDocu.UnboundType = UnboundColumnType.Integer;
                ordCompDocu.Visible = false;
                this.gvDocument.Columns.Add(ordCompDocu);

                //OrdCompraDetaID
                GridColumn ordCompDeta = new GridColumn();
                ordCompDeta.FieldName = this.unboundPrefix + "OrdCompraDetaID";
                ordCompDeta.UnboundType = UnboundColumnType.Integer;
                ordCompDeta.Visible = false;
                this.gvDocument.Columns.Add(ordCompDeta);

                //RecibidoDocuID
                GridColumn RecibidoDocuID = new GridColumn();
                RecibidoDocuID.FieldName = this.unboundPrefix + "RecibidoDocuID";
                RecibidoDocuID.UnboundType = UnboundColumnType.Integer;
                RecibidoDocuID.Visible = false;
                this.gvDocument.Columns.Add(RecibidoDocuID);

                //RecibidoDetaID
                GridColumn RecibidoDetaID = new GridColumn();
                RecibidoDetaID.FieldName = this.unboundPrefix + "RecibidoDetaID";
                RecibidoDetaID.UnboundType = UnboundColumnType.Integer;
                RecibidoDetaID.Visible = false;
                this.gvDocument.Columns.Add(RecibidoDetaID);   

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDocument.Columns.Add(colIndex);
                #endregion
                this.gvDocument.OptionsView.ColumnAutoWidth = true;
                this.gvDocument.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Revisa si se cumplen condiciones particulares para salvar los re
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanSave()
        {
            bool gridValid = true;
            try
            {
                if (this._detalle != null && this._detalle.Count > 0 && !this._detalle.Any(x=>x.CantidadCierre.Value != 0))
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "No ha seleccionado ninguna cantidad de cierre"));
                    gridValid = false;                    
                }                
                return gridValid;
            }
            catch (Exception)
            {
                return gridValid = false;
            }
        }

        /// <summary>
        /// Calcula los valores de cada item de solicitud de OC
        /// </summary>
        protected void CalculateValues(int index)
        {
            try
            {
                #region ValorUni
                //if (this._currentRow.ValorUni.Value > 0)
                //{
                //    List<DTO_prOrdenCompraFooter> tempFooterList = this.documentID == AppDocuments.OrdenCompra ? this.GetValuesAIU(index, this._detalle.Count) : null;
                //    if (tempFooterList != null)
                //    {
                //        if (string.IsNullOrEmpty(this._codBienServicio))
                //        {
                //            thisAIU = new List<DTO_prOrdenCompraFooter>();
                //            thisAIU.AddRange(tempFooterList);
                //            //this._detalle.AddRange(tempFooterList);
                //            this._codBienServicio =this._currentRow.CodigoBSID.Value;
                //        }
                //    }

                //    if (this._solicitudes.Count > 0)
                //        if (this._solicitudes.Exists(x => x.ConsecutivoDetaID.Value ==this._currentRow.SolicitudDetaID.Value))
                //            this._solicitudes.Find(x => x.ConsecutivoDetaID.Value ==this._currentRow.SolicitudDetaID.Value).ValorUni.Value =this._currentRow.ValorUni.Value.Value;

                //    this.GetValuesDocument(index);

                //    this.gvDocument.RefreshData();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "CalculateValues"));
            }

        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected void CleanControls()
        {           
            this.masterPrefijo.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterProveedorFilter.Value = string.Empty;
            this.txtProveedorDesc.Text = string.Empty;
            this.txtProyectoDesc.Text = string.Empty;
            this.txtDocNro.Text = string.Empty;
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        protected virtual void EnableFooter(bool enable)
        {
       
        }

        /// <summary>
        /// Loads the document main info
        /// </summary>
        private void LoadDocumentInfo(bool firstTime)
        {
            try
            {
                if (firstTime)
                {
                    //Llena el area funcional
                    this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
                    DTO_MasterBasic basicDTO = (DTO_MasterBasic)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
                    if (basicDTO == null)
                        MessageBox.Show("El área funcional del usuario NO existe");
                    else
                        this.txtAF.Text = basicDTO.Descriptivo.Value;

                    this.prefijoID = _bc.GetPrefijo(this.areaFuncionalID, this.documentID);
                    DTO_glDocumento dtoDoc = (DTO_glDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);

                    if (string.IsNullOrEmpty(this.prefijoID))
                    {
                        this.lblPrefix.Visible = false;
                        this.txtPrefix.Visible = false;
                    }
                    else
                        this.txtPrefix.Text = this.prefijoID;

                    this.terceroID = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    this.txtDocumentoID.Text = this.documentID.ToString();
                    this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
                    this.txtNumeroDoc.Text = "0";

                    string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo);
                    this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
                    this.dtFecha.DateTime = this.dtPeriod.DateTime;
                    if (this.documentID == AppDocuments.ComprobanteManual || this.documentID == AppDocuments.DocumentoContable)
                        this.dtPeriod.Enabled = true;
                    else
                        this.dtPeriod.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "LoadDocumentInfo"));
            }
        }

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        protected void LoadData()
        {
            try
            {     
                if (this.chkDocumentos.SelectedIndex == 0)// Solicitud
                {
                    int? docNro = null;
                    docNro = string.IsNullOrEmpty(this.txtDocNro.Text)? docNro : Convert.ToInt32(this.txtDocNro.Text);
                    this._detalle = this._bc.AdministrationModel.prDetalleDocu_GetPendienteForCierre(AppDocuments.Solicitud, this.masterPrefijo.Value, docNro, this.masterProveedorFilter.Value, this.masterReferencia.Value,this.masterCodigoBS.Value);
                                               
                }
                else if (this.chkDocumentos.SelectedIndex == 1) //Orden Compra
                {
                    int? docNro = null;
                    docNro = string.IsNullOrEmpty(this.txtDocNro.Text) ? docNro : Convert.ToInt32(this.txtDocNro.Text);
                    this._detalle = this._bc.AdministrationModel.prDetalleDocu_GetPendienteForCierre(AppDocuments.OrdenCompra, this.masterPrefijo.Value, docNro, this.masterProveedorFilter.Value, this.masterReferencia.Value, this.masterCodigoBS.Value);
                }               

                if (this.masterProyecto.ValidID)
                    this._detalle = this._detalle.FindAll(x => x.ProyectoID.Value == this.masterProyecto.Value).ToList();

                this._detalle = this._detalle.OrderByDescending(x => x.NumeroDoc.Value).ToList();
                this.gvDocument.MoveFirst();
                this.gcDocument.DataSource = this._detalle;
                this.gcDocument.RefreshDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "CIerreDetalleProv.cs", "LoadData"));
            }           
        }

        /// <summary>
        /// Extrae datos de la grilla para las cajas de texto
        /// </summary>
        /// <param name="isNew">Identifica si es un nuevo registro</param>
        /// <param name="rowIndex">Numero de la fila</param>
        protected  void LoadEditGridData(bool isNew, int rowIndex)
        {
            if (!this.disableValidate)
            {
                try
                {
                    if (rowIndex >= 0)
                        this._currentRow = (DTO_prDetalleDocu)this.gvDocument.GetRow(rowIndex);
                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "LoadEditGridData"));
                }
            }
        }

        /// <summary>
        /// Calcula valores 
        /// </summary>
        protected void GetValuesDocument(int index)
        {
            this.valorTotalDoc = 0;
            this.valorIVATotalDoc = 0;

            //if (this.monedaLocal.Equals(this.monedaOrden))
            //{
            //    this.txtValorIVA.EditValue =this._currentRow.IvaTotML.Value.Value;
            //    this.txtValorTotal.EditValue =this._currentRow.ValorTotML.Value.Value;
            //}
            //else
            //{
            //    this.txtValorIVA.EditValue =this._currentRow.IvaTotME.Value.Value;
            //    this.txtValorTotal.EditValue =this._currentRow.ValorTotME.Value.Value;
            //}

            //this.txtValorAIU.EditValue =this._currentRow.ValorAIU.Value.Value;
            //this.txtValorIVAAIU.EditValue =this._currentRow.VlrIVAAIU.Value.Value;

            //if (this._vlrTasaCambio != 0)
            //{
            //    decimal porcIVA = (this._currentRow.PorcentajeIVA.Value.Value / 100);
            //    decimal? vlrTot =this._currentRow.ValorUni.Value *this._currentRow.CantidadOC.Value;
            //    if (this.monedaOrden.Equals(this.monedaLocal))
            //    {
            //       this._currentRow.IvaTotME.Value = (vlrTot * porcIVA) / this._vlrTasaCambio;
            //       this._currentRow.ValorTotME.Value = (vlrTot / this._vlrTasaCambio);
            //    }
            //    else
            //    {
            //       this._currentRow.IvaTotML.Value = (vlrTot * porcIVA) * this._vlrTasaCambio;
            //       this._currentRow.ValorTotML.Value = (vlrTot * this._vlrTasaCambio);
            //    }
            //}

            #region Asigna totales
            //foreach (var footer in _detalle)
            //{
            //    if (this.monedaLocal.Equals(this.monedaOrden))
            //    {
            //        this.valorTotalDoc += footer.DetalleDocu.ValorTotML.Value.Value;
            //        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotML.Value.Value;
            //    }
            //    else
            //    {
            //        this.valorTotalDoc += footer.DetalleDocu.ValorTotME.Value.Value;
            //        this.valorIVATotalDoc += footer.DetalleDocu.IvaTotME.Value.Value;
            //    }
            //}
            //this._detalle.HeaderOrdenCompra.Valor.Value = this.valorTotalDoc;
            //this._detalle.HeaderOrdenCompra.IVA.Value = this.valorIVATotalDoc;
            //this.txtValorDoc.EditValue = this.valorTotalDoc;
            //this.txtValorIVADoc.EditValue = this.valorIVATotalDoc;
            #endregion
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected  bool ValidateRow(int fila)
        {
            bool validRow = true;
            bool validField = true;

            try
            {
                GridColumn col = new GridColumn();
                if (validRow)
                {
                    this.isValid = true;                  
                }
                else
                    this.isValid = false;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "ValidateRow"));
            }

            return validRow;
        }

        #endregion

        #region Eventos Header

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida al cambiar el index
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadData();
            if (this.chkDocumentos.SelectedIndex == 0)
            {
                this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"].VisibleIndex = 0;
                this.gvDocument.Columns[this.unboundPrefix + "CodigoBSID"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].VisibleIndex = 2;
                this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 4;
                this.gvDocument.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 5;
                this.gvDocument.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 6;               
                this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 7;
                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].VisibleIndex = 8;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 9;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadCierre"].VisibleIndex = 10;

                this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CodigoBSID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "RefProveedor"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "MarcaInvID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].Visible = true; 
                this.gvDocument.Columns[this.unboundPrefix + "CantidadPend"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadCierre"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorUni"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "ProveedorID"].Visible = false;
                this.gvDocument.Columns[this.unboundPrefix + "SolicitarInd"].Visible = false;
                this.masterProveedorFilter.Visible = false;
                this.documentID = AppDocuments.CierreDetalleSolicitud;
                this.LoadDocumentInfo(true);
            }
            else if (this.chkDocumentos.SelectedIndex == 1)
            {
                this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"].VisibleIndex = 0;
                this.gvDocument.Columns[this.unboundPrefix + "CodigoBSID"].VisibleIndex = 1;
                this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].VisibleIndex = 2;
                this.gvDocument.Columns[this.unboundPrefix + "RefProveedor"].VisibleIndex = 3;
                this.gvDocument.Columns[this.unboundPrefix + "MarcaInvID"].VisibleIndex = 4;
                this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].VisibleIndex = 5;
                this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].VisibleIndex = 6;
                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].VisibleIndex = 8;
                this.gvDocument.Columns[this.unboundPrefix + "ValorUni"].VisibleIndex = 8;
                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].VisibleIndex = 9;
                this.gvDocument.Columns[this.unboundPrefix + "ProveedorID"].VisibleIndex = 10;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadPend"].VisibleIndex = 11;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadCierre"].VisibleIndex = 12;

                this.gvDocument.Columns[this.unboundPrefix + "PrefDoc"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CodigoBSID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "inReferenciaID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "RefProveedor"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "MarcaInvID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "Descriptivo"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "UnidadInvID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ProyectoID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadPend"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "CantidadCierre"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorUni"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ValorTotML"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "ProveedorID"].Visible = true;
                this.gvDocument.Columns[this.unboundPrefix + "SolicitarInd"].Visible = true;
                this.masterProveedorFilter.Visible = true;
                this.documentID = AppDocuments.CierreDetalleOrdenComp;
                this.LoadDocumentInfo(true);
            }           
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            DTO_glDocumentoControl ctrl = null;
            List<int> docs = new List<int>();
            if(this.chkDocumentos.SelectedIndex == 0)//Trae solicitudes
            {
                docs.Add(AppDocuments.Solicitud);
                ModalQuerySolicitud getDocControl = new ModalQuerySolicitud(docs, false,false);
                getDocControl.ShowDialog();
                ctrl = getDocControl.DocumentoControl;
            }              
            else
            {
                docs.Add(AppDocuments.OrdenCompra);//Trae OC
                ModalQueryOrdCompra getDocControl = new ModalQueryOrdCompra(docs,false, false);
                getDocControl.ShowDialog();
                ctrl = getDocControl.DocumentoControl;
            }          
            if (ctrl != null)
            {              
                this.txtDocNro.Text = ctrl.DocumentoNro.Value.ToString();
                this.masterPrefijo.Value = ctrl.PrefijoID.Value;
            }
        }

        /// <summary>
        /// Permite cargar los items autom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkMarcarItem.Checked)
            {

                foreach (var item in this._detalle)
                    item.CantidadCierre.Value = item.CantidadPend.Value;

                this.chkMarcarItem.Text = "Desmarcar Todos";
            }
            else
            {
                foreach (var item in this._detalle)
                    item.CantidadCierre.Value = 0;

                this.chkMarcarItem.Text = "Marcar Todos";
            }
            this.gvDocument.MoveFirst();
            this.gcDocument.DataSource = this._detalle;
            this.gcDocument.RefreshDataSource();
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (this._currentRow.CantidadCierre.Value > this._currentRow.CantidadPend.Value)
                {
                    e.Allow = false;
                    this.isValid = false;
                    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "CantidadCierre"];
                    this.gvDocument.SetColumnError(col, "Cantidad de cierre Inválida");
                }
                else
                {
                    e.Allow = true;
                    this.isValid = true;
                    this.gvDocument.ClearColumnErrors();
                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            {
                object dto = (object)e.Row;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (e.IsGetData)
                {
                    PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                            e.Value = pi.GetValue(dto, null);
                        else
                            e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                    else
                    {
                        FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                        if (fi != null)
                        {
                            if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                e.Value = fi.GetValue(dto);
                            else
                                e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                        else
                        {
                            pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    e.Value = pi.GetValue(dto, null);
                                else
                                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                            }
                            else
                            {
                                fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                        e.Value = fi.GetValue(dto);
                                    else
                                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
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
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                        {
                            pi.SetValue(dto, e.Value, null);
                            //e.Value = pi.GetValue(dto, null);
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
                            if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                            {
                                pi.SetValue(dto, e.Value, null);
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
                            DTO_prOrdenCompraFooter dtoDet = (DTO_prOrdenCompraFooter)e.Row;
                            pi = dtoDet.DetalleDocu.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                            if (pi != null)
                            {
                                if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                {
                                    e.Value = pi.GetValue(dtoDet.DetalleDocu, null);
                                }
                                else
                                {
                                    UDT udtProp = (UDT)pi.GetValue(dtoDet.DetalleDocu, null);
                                    udtProp.SetValueFromString(e.Value.ToString());
                                }
                            }
                            else
                            {
                                fi = dtoDet.DetalleDocu.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                                if (fi != null)
                                {
                                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double" || pi.PropertyType.Name == "Decimal" || pi.PropertyType.Name == "DateTime" || pi.PropertyType.Name == "Byte")
                                    {
                                        pi.SetValue(dto, e.Value, null);
                                        //e.Value = pi.GetValue(dto, null);
                                    }
                                    else
                                    {
                                        UDT udtProp = (UDT)fi.GetValue(dtoDet.DetalleDocu);
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
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void gvDocument_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle >= 0 && this._detalle.Count > 0)
                {
                    this._currentRow = (DTO_prDetalleDocu)this.gvDocument.GetRow(e.FocusedRowHandle);
                    if (this._currentRow != null)
                    {
                        this.txtProyectoDesc.Text = this._currentRow.ProyectoDesc.Value;
                        this.txtProveedorDesc.Text = this._currentRow.ProveedorDesc.Value;
                    }                   
                }
                else
                {
                    this.txtProyectoDesc.Text = string.Empty;
                    this.txtProyectoDesc.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CIerreDetalleProv.cs", "gvDocument_BeforeLeaveRow"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region
                if (fieldName == "SolicitarInd" && e.RowHandle >= 0)
                {
                    DTO_prDetalleDocu row = (DTO_prDetalleDocu)this.gvDocument.GetRow(e.RowHandle);
                    if(Convert.ToBoolean(e.Value))
                        row.CantidadCierre.Value = row.CantidadPend.Value;
                }
                else if (fieldName == "CantidadCierre" && e.RowHandle >= 0)
                {
                    DTO_prDetalleDocu row = (DTO_prDetalleDocu)this.gvDocument.GetRow(e.RowHandle);
                    if (row.SolicitarInd.Value.Value)
                        row.CantidadCierre.Value = row.CantidadPend.Value;
                }
                #endregion
                this.gvDocument.RefreshRow(e.RowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreDetalle.cs.cs", "Recibido.cs-gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operaciones mientras se modifican los valores del campo en la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocument_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e) { }

        /// <summary>
        /// Asigna texto por defecto para la columna de archivos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void gvDocuments_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e) { }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected void Form_Enter(object sender, EventArgs e)
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
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemPrint.Visible = false;

            FormProvider.Master.itemSave.Visible = true;
            FormProvider.Master.itemSearch.Visible = true;
            FormProvider.Master.itemSearch.Enabled = true;
            FormProvider.Master.itemSave.Enabled = true;
               
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                
                    string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgLostInfo = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.LostInfo);

                    if (MessageBox.Show(msgLostInfo, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);                      
                    }
                    else
                    {
                        e.Cancel = true;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentForm", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para iniciar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                if (this.cleanDoc)
                {                  
                    this. _detalle = new List<DTO_prDetalleDocu>();
                    this. _cargos = new List<DTO_prSolicitudCargos>();
                    this._currentRow = new DTO_prDetalleDocu();
                    this._dtoCtrl = new DTO_glDocumentoControl();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this._detalle;
                    this.disableValidate = false;

                    this.CleanControls();
                    this.masterPrefijo.EnableControl(true);
                    this.masterPrefijo.Focus();
                    this.txtDocNro.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this.chkMarcarItem.Text = "Marcar Todos";
                    this.chkMarcarItem.Checked = false;

                }
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
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
            try
            {
                if (this.CanSave())
                {
                    this.gvDocument.PostEditor();

                    #region Campos variables DTO_glDocumentoControl
                    this._dtoCtrl.DocumentoID.Value = this.documentID;
                    this._dtoCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                    this._dtoCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                    this._dtoCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                    this._dtoCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                    this._dtoCtrl.PrefijoID.Value = this.txtPrefix.Text;
                    this._dtoCtrl.Observacion.Value = this.txtDocDesc.Text;
                    this._dtoCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                    if(this.documentID == AppDocuments.CierreDetalleSolicitud)
                        this._dtoCtrl.Descripcion.Value = "Cierre Detalle de Solicitudes";
                    else if (this.documentID == AppDocuments.CierreDetalleOrdenComp)
                        this._dtoCtrl.Descripcion.Value = "Cierre detalle de Orden Compra";
                    else if (this.documentID == AppDocuments.CierreDetalleRecibidos)
                        this._dtoCtrl.Descripcion.Value = "Cierre detalle de Recibidos";
                    this._dtoCtrl.Valor.Value = 0;
                    this._dtoCtrl.Iva.Value = 0;
                    this._dtoCtrl.MonedaID.Value = this.monedaLocal;
                    this._dtoCtrl.TasaCambioCONT.Value = 0;
                    this._dtoCtrl.TasaCambioDOCU.Value = 0;
                    this._dtoCtrl.TerceroID.Value = string.Empty;
                    this._dtoCtrl.DocumentoTercero.Value = string.Empty;
                    this._dtoCtrl.Estado.Value = (byte)EstadoDocControl.Aprobado;
                    this._dtoCtrl.seUsuarioID.Value = this.userID;
                    this._dtoCtrl.Fecha.Value = DateTime.Now;
                    #endregion
                    
                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = false;
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                 this.LoadData();
                 this.chkMarcarItem.Text = "Marcar Todos";
                 this.chkMarcarItem.Checked=false;
            }
            catch (Exception ex)
            {
                throw ex;
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
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                this._detalle = this._detalle.FindAll(x => x.CantidadCierre.Value != 0).ToList();
                DTO_SerializedObject result = _bc.AdministrationModel.CierreDetalle_Guardar(this.documentID, this._dtoCtrl,this._detalle);             

                 bool isOK = true;
                if(!string.IsNullOrEmpty(this._actFlujo.seUsuarioID.Value))                   
                    isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);
                else
                    isOK = _bc.SendDocumentMail(MailType.NotSend, this.documentID, this._bc.AdministrationModel.User.ID.Value, result, true, true);

                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "CIerreDetalleProv.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion


    }
}
