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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SentenceTransformer;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario para aprobacion de documentos
    /// </summary>
    public partial class Recibido : FormWithToolbar
    {
        #region Delegados

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private void SaveMethod()
        {
            if (this._isOK)
            {
                this._data = new DTO_prRecibido();
                this._listOrdenCompraSelect = new List<DTO_prOrdenCompraResumen>();

                this.disableValidate = true;
                this.gcDetails.DataSource = this._listOrdenCompraSelect;
                this.disableValidate = false;

                this.CleanHeader(true);
                this.EnableHeader(false);
                this.masterPrefijoRec.EnableControl(true);
                this.masterPrefijoRec.Focus();

                this.txtDesc.Text = string.Empty;
                this.masterMarca.Value = string.Empty;
                this.txtRefProveedor.Text = string.Empty;
                this.masterReferencia_det.Value = string.Empty;
                this.txtValorLocalOC.EditValue = 0;
                this.txtValorExtrOC.EditValue = 0;
            }

            FormProvider.Master.itemSendtoAppr.Enabled = true;
            FormProvider.Master.itemSave.Enabled = true;
        }

        //private delegate void ShowResultDialog(MessageForm frm);
        //private ShowResultDialog ShowResultDialogDelegate;
        ///// <summary>
        ///// Delegado que muestra un resultado dentro de un dialogo
        ///// </summary>
        //private void ShowResultDialogMethod(MessageForm frm)
        //{
        //    frm.ShowDialog();
        //}
        #endregion

        #region Variables
        private BaseController _bc = BaseController.GetInstance();
        private DTO_prRecibido _data = null;
        private List<DTO_prOrdenCompraResumen> _listOrdenCompraSelect;
        private bool _txtRecibidoNroFocus = false;

        private int userID = 0;
        private string empresaID = string.Empty;
        private int documentID;
        private ModulesPrefix frmModule;
        private string unboundPrefix = "Unbound_";      
      
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;

        private bool _proveedorFocus = false;
        private bool _prefijoFocus = false;
        private bool _controlFocus = false;
        private string proveedorID;
        private bool bodegaTransitoInd = false;
        private bool _copyData = false;

        private bool _multiMoneda;
        private string _monedaLocal;
        private string _monedaExtranjera;
        private decimal _tasaCambio = 0;
        private string defProyecto = string.Empty;
        private string defCentroCosto = string.Empty;
        private string defLineaPresupuesto = string.Empty;
        private string defLugarGeo = string.Empty;
        private string areaFuncionalID = string.Empty;
        private string UsoReferenciaCodigoInd = string.Empty;
        private Incoterm _incoterm;
        private bool _isOK = true;

        private List<Tuple<string, string>> Filtros;
        private bool disableValidate = false;
        protected DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        #endregion

        public Recibido()
        {
            try
            {
                this.InitializeComponent();

                this.SetInitParameters();
                this._multiMoneda = _bc.AdministrationModel.MultiMoneda;
                this._frmName =this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                this._data = new DTO_prRecibido();
                this._listOrdenCompraSelect = new List<DTO_prOrdenCompraResumen>();
                this.Filtros = new List<Tuple<string, string>>();

                //Asigna la lista de columnas
                this.AddGridCols();

                //Limpia y deabilita los controles del formulario
                this.CleanHeader(true);
                this.EnableHeader(false);
                this.masterPrefijoRec.EnableControl(true);
                this.gcDetails.Enabled = false;
                this.masterMarca.EnableControl(false);
                this.masterReferencia_det.EnableControl(false);
                //this.txtDesc.Enabled = false;
                this.txtRecibidoNro.Enabled = false;
                this.LoadTasaCambio();

                #region Carga la info de las actividades
                List<string> actividades = this._bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
                this.TBNew();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido"));
            }
        }

        #region  Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.empresaID = this._bc.AdministrationModel.Empresa.ID.Value;
            this.userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;

            //Carga info de las monedas
            this._monedaLocal = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._monedaExtranjera = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

            this.defProyecto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            this.defCentroCosto = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            this.areaFuncionalID = this._bc.AdministrationModel.User.AreaFuncionalID.Value;

            this.documentID = AppDocuments.Recibido;
            this.frmModule = ModulesPrefix.pr;

            DTO_MasterBasic basicDTO = (DTO_MasterBasic)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glAreaFuncional, false, this.areaFuncionalID, true);
            this.txtAF.Text = basicDTO.Descriptivo.Value;

            DTO_glDocumento dtoDoc = (DTO_glDocumento)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, true, this.documentID.ToString(), true);
            this.txtDocumentoID.Text = this.documentID.ToString();
            this.txtDocDesc.Text = dtoDoc.Descriptivo.Value;
            this.txtNumeroDoc.Text = "0";

            this.lblPrefix.Visible = false;
            this.txtPrefix.Visible = false;

            string periodo = this._bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
            this.dtPeriod.DateTime = Convert.ToDateTime(periodo);
            this.dtFecha.DateTime = this.dtPeriod.DateTime;
            this.dtPeriod.Enabled = false;

            //Inicia los controles del formulario            
            List<DTO_glConsultaFiltro> filtrosLugarRecibido = new List<DTO_glConsultaFiltro>();
            filtrosLugarRecibido.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "RecibidosInd",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "1"
            });

            this.UsoReferenciaCodigoInd = (this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_IndUsoCodigoReferenciaRecibidos));
            
            if (this.UsoReferenciaCodigoInd.Equals("1"))
                this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferenciaCod, true, true, true, false);
            else
                this._bc.InitMasterUC(this.masterReferencia, AppMasters.inReferencia, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoRec, AppMasters.glPrefijo, true, true, true,true);
            this._bc.InitMasterUC(this.masterPrefijoOC, AppMasters.glPrefijo, false, true, true, false);
            this._bc.InitMasterUC(this.masterProveedor, AppMasters.prProveedor, true, true, true, true);
            this._bc.InitMasterUC(this.masterCodigoBS, AppMasters.prBienServicio, true, true, true, false);
            this._bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
            this._bc.InitMasterUC(this.masterMonedaFilter, AppMasters.glMoneda, false, true, true, false);
            this._bc.InitMasterUC(this.masterBodega, AppMasters.inBodega, true, true, true, false);
            this._bc.InitMasterUC(this.masterLugarEntrega, AppMasters.glLocFisica, true, true, true, false, filtrosLugarRecibido);
            this._bc.InitMasterUC(this.masterMarca, AppMasters.inMarca, true, true, true, false);
            this._bc.InitMasterUC(this.masterReferencia_det, AppMasters.inReferencia, true, true, true, false);
            this.masterMonedaFilter.Value = this._monedaLocal;
            this.masterPrefijoRec.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);

            //this.gcDetails.ShowOnlyPredefinedDetails = true;
            this.lblOrdenCompNro.BringToFront();
            this.txtOrdenCompNro.BringToFront();

            //this.gvDetails.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetails.Appearance.HeaderPanel.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            this.saveDelegate = new Save(this.SaveMethod);
            this.masterPrefijoRec.Focus();
        }
        
        /// <summary>
        /// Asigna la lista de columnas del detalle
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                #region Columnas basicas
                //FechaOC
                GridColumn fechaOC = new GridColumn();
                fechaOC.FieldName = this.unboundPrefix + "FechaOC";
                fechaOC.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_FechaOC");
                fechaOC.UnboundType = UnboundColumnType.DateTime;
                fechaOC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                fechaOC.AppearanceCell.Options.UseTextOptions = true;
                fechaOC.VisibleIndex = 0;
                fechaOC.Width = 35;
                fechaOC.Visible = true;
                fechaOC.OptionsColumn.AllowEdit = false;
                fechaOC.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(fechaOC);

                //Documento OrdenCompra
                GridColumn docOC = new GridColumn();
                docOC.FieldName = this.unboundPrefix + "PrefDocOC";
                docOC.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDocOC");
                docOC.UnboundType = UnboundColumnType.String;
                docOC.VisibleIndex = 1;
                docOC.Width = 35;
                docOC.Visible = true;
                docOC.OptionsColumn.AllowEdit = false;
                docOC.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(docOC);

                //Documento Solicitud
                GridColumn docSol = new GridColumn();
                docSol.FieldName = this.unboundPrefix + "PrefDocSol";
                docSol.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_PrefDocSol");
                docSol.UnboundType = UnboundColumnType.String;
                docSol.VisibleIndex = 2;
                docSol.Width = 35;
                docSol.Visible = true;
                docSol.OptionsColumn.AllowEdit = false;
                docSol.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(docSol);

                //ProyectoID
                GridColumn ProyectoID = new GridColumn();
                ProyectoID.FieldName = this.unboundPrefix + "ProyectoID";
                ProyectoID.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_ProyectoID");
                ProyectoID.UnboundType = UnboundColumnType.String;
                ProyectoID.VisibleIndex = 3;
                ProyectoID.Width = 35;
                ProyectoID.Visible = true;
                ProyectoID.OptionsColumn.AllowEdit = false;
                ProyectoID.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(ProyectoID);

                //CodigoServicios
                GridColumn codBS = new GridColumn();
                codBS.FieldName = this.unboundPrefix + "CodigoBSID";
                codBS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CodigoBSID");
                codBS.UnboundType = UnboundColumnType.String;
                codBS.VisibleIndex = 4;
                codBS.Width = 40;
                codBS.Visible = true;
                codBS.OptionsColumn.AllowEdit = false;
                codBS.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(codBS);

                //Descriptivo
                GridColumn desc = new GridColumn();
                desc.FieldName = this.unboundPrefix + "Descriptivo";
                desc.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Descriptivo");
                desc.UnboundType = UnboundColumnType.String;
                desc.VisibleIndex = 6;
                desc.Width = 180;
                desc.Visible = true;
                desc.OptionsColumn.AllowEdit = false;
                desc.OptionsColumn.AllowFocus = false;
                this.gvDetails.Columns.Add(desc);

                //UnidadInvID
                GridColumn UnidadInvID = new GridColumn();
                UnidadInvID.FieldName = this.unboundPrefix + "UnidadInvID";
                UnidadInvID.Caption = "UM";
                UnidadInvID.UnboundType = UnboundColumnType.String;
                UnidadInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                UnidadInvID.AppearanceCell.Options.UseTextOptions = true;
                UnidadInvID.VisibleIndex = 7;
                UnidadInvID.Width = 25;
                UnidadInvID.Visible = true;
                UnidadInvID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(UnidadInvID);

                //unidadEmp
                GridColumn EmpaqueInvID = new GridColumn();
                EmpaqueInvID.FieldName = this.unboundPrefix + "EmpaqueInvID";
                EmpaqueInvID.Caption = "Empaque";
                EmpaqueInvID.UnboundType = UnboundColumnType.String;
                EmpaqueInvID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                EmpaqueInvID.AppearanceCell.Options.UseTextOptions = true;
                EmpaqueInvID.AppearanceCell.ForeColor = Color.Gray;
                EmpaqueInvID.AppearanceCell.Options.UseForeColor = true;
                EmpaqueInvID.VisibleIndex = 8;
                EmpaqueInvID.Width = 30;
                EmpaqueInvID.Visible = true;
                EmpaqueInvID.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(EmpaqueInvID);

                //CantidadEmp
                GridColumn cantEmpaque = new GridColumn();
                cantEmpaque.FieldName = this.unboundPrefix + "CantEmpaque";
                cantEmpaque.Caption = "Cant Empaque";
                cantEmpaque.UnboundType = UnboundColumnType.Integer;
                cantEmpaque.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantEmpaque.AppearanceHeader.Font = new System.Drawing.Font("Arial Narrow", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                cantEmpaque.AppearanceCell.Options.UseTextOptions = true;
                cantEmpaque.AppearanceCell.ForeColor = Color.Gray;
                cantEmpaque.AppearanceCell.Options.UseForeColor = true;
                cantEmpaque.AppearanceHeader.Options.UseFont = true;
                cantEmpaque.VisibleIndex = 9;
                cantEmpaque.Width = 35;
                cantEmpaque.Visible = true;
                cantEmpaque.ColumnEdit = this.editSpinDecimal;
                cantEmpaque.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(cantEmpaque);
                #endregion
                #region Columnas extras
                #region Columnas Visible
                //Cantidad Solicitud
                GridColumn cantS = new GridColumn();
                cantS.FieldName = this.unboundPrefix + "CantidadSol";
                cantS.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadSol");
                cantS.UnboundType = UnboundColumnType.Integer;
                cantS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantS.AppearanceCell.Options.UseTextOptions = true;
                cantS.VisibleIndex = 10;
                cantS.Width = 35;
                cantS.Visible = true;
                cantS.OptionsColumn.AllowEdit = false;
                cantS.OptionsColumn.AllowFocus = false;
                cantS.ColumnEdit = this.editSpinDecimal;
                this.gvDetails.Columns.Add(cantS);

                //Cantidad Solicitud
                GridColumn cantP = new GridColumn();
                cantP.FieldName = this.unboundPrefix + "CantidadOC";
                cantP.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadOC");
                cantP.UnboundType = UnboundColumnType.Integer;
                cantP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantP.AppearanceCell.Options.UseTextOptions = true;
                cantP.VisibleIndex = 11;
                cantP.Width = 35;
                cantP.Visible = true;
                cantP.OptionsColumn.AllowEdit = false;
                cantP.OptionsColumn.AllowFocus = false;
                cantP.ColumnEdit = this.editSpinDecimal;
                this.gvDetails.Columns.Add(cantP);

                //Cantidad Recibido
                GridColumn cantR = new GridColumn();
                cantR.FieldName = this.unboundPrefix + "CantidadRec";
                cantR.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_CantidadRec");
                cantR.UnboundType = UnboundColumnType.Integer;
                cantR.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                cantR.AppearanceCell.Options.UseTextOptions = true;
                cantR.VisibleIndex = 12;
                cantR.Width = 35;
                cantR.Visible = true;
                cantR.OptionsColumn.AllowEdit = true;
                cantR.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                cantR.AppearanceCell.Options.UseTextOptions = true;
                cantR.AppearanceCell.Options.UseFont = true;
                cantR.ColumnEdit = this.editSpinDecimal;
                this.gvDetails.Columns.Add(cantR);          

                //Serial
                GridColumn serial = new GridColumn();
                serial.FieldName = this.unboundPrefix + "SerialID";
                serial.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_SerialID");
                serial.UnboundType = UnboundColumnType.String;
                serial.VisibleIndex = 13;
                serial.Width = 35;
                serial.Visible = true;
                serial.OptionsColumn.AllowEdit = true;              
                this.gvDetails.Columns.Add(serial);

                //MonedaID
                GridColumn moneda = new GridColumn();
                moneda.FieldName = this.unboundPrefix + "MonedaIDOC";
                moneda.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_MonedaIDOC");
                moneda.UnboundType = UnboundColumnType.String;
                moneda.VisibleIndex = 14;
                moneda.Width = 40;
                moneda.Visible = true;
                moneda.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(moneda);

                #endregion
                #region Columnas No Visibles

                //CodigoReferencia
                GridColumn codRef = new GridColumn();
                codRef.FieldName = this.unboundPrefix + "inReferenciaID";
                codRef.Caption = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_inReferenciaID");
                codRef.UnboundType = UnboundColumnType.String;
                codRef.Visible = false;
                codRef.OptionsColumn.AllowEdit = false;
                this.gvDetails.Columns.Add(codRef);               

                //NumeroDoc
                GridColumn numDoc = new GridColumn();
                numDoc.FieldName = this.unboundPrefix + "NumeroDoc";
                numDoc.UnboundType = UnboundColumnType.Integer;
                numDoc.Visible = false;
                this.gvDetails.Columns.Add(numDoc);

                //SolicitudDocuID
                GridColumn solDocu = new GridColumn();
                solDocu.FieldName = this.unboundPrefix + "SolicitudDocuID";
                solDocu.UnboundType = UnboundColumnType.Integer;
                solDocu.Visible = false;
                this.gvDetails.Columns.Add(solDocu);

                //OrdCompraDocuID
                GridColumn ocDocu = new GridColumn();
                ocDocu.FieldName = this.unboundPrefix + "OrdCompraDocuID";
                ocDocu.UnboundType = UnboundColumnType.Integer;
                ocDocu.Visible = false;
                this.gvDetails.Columns.Add(ocDocu);

                //ConsecutivoDetaID
                GridColumn consDeta = new GridColumn();
                consDeta.FieldName = this.unboundPrefix + "ConsecutivoDetaID";
                consDeta.UnboundType = UnboundColumnType.Integer;
                consDeta.Visible = false;
                this.gvDetails.Columns.Add(consDeta);

                //SolicitudDetaID
                GridColumn solDeta = new GridColumn();
                solDeta.FieldName = this.unboundPrefix + "SolicitudDetaID";
                solDeta.UnboundType = UnboundColumnType.Integer;
                solDeta.Visible = false;
                this.gvDetails.Columns.Add(solDeta);

                //OrdCompraDetaID
                GridColumn ocDeta = new GridColumn();
                ocDeta.FieldName = this.unboundPrefix + "OrdCompraDetaID";
                ocDeta.UnboundType = UnboundColumnType.Integer;
                ocDeta.Visible = false;
                this.gvDetails.Columns.Add(ocDeta);

                //RefProveedor / MOdelo
                GridColumn RefProveedor = new GridColumn();
                RefProveedor.FieldName = this.unboundPrefix + "RefProveedor";
                RefProveedor.UnboundType = UnboundColumnType.String;
                RefProveedor.Visible = false;
                this.gvDetails.Columns.Add(RefProveedor);

                //MarcaInvID
                GridColumn MarcaInvID = new GridColumn();
                MarcaInvID.FieldName = this.unboundPrefix + "MarcaInvID";
                MarcaInvID.UnboundType = UnboundColumnType.String;
                MarcaInvID.Visible = false;
                this.gvDetails.Columns.Add(MarcaInvID);

                //Indice de la fila
                GridColumn colIndex = new GridColumn();
                colIndex.FieldName = this.unboundPrefix + "Index";
                colIndex.UnboundType = UnboundColumnType.Integer;
                colIndex.Visible = false;
                this.gvDetails.Columns.Add(colIndex);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recobodp.cs-AddGridCols"));
            }
        }

        /// <summary>
        /// Limpia controles
        /// </summary>
        /// <param name="cleanAll"></param>
        private void CleanHeader(bool cleanAll)
        {
            if (cleanAll)
                this.txtRecibidoNro.Text = "0";
            this.masterProveedor.Value = string.Empty;
            this.masterCodigoBS.Value = string.Empty;
            this.masterReferencia.Value = string.Empty;
            this.masterBodega.Value = string.Empty;
            this.masterPrefijoOC.Value = string.Empty;
            this.masterLugarEntrega.Value = string.Empty;
            this.txtOrdenCompNro.Text = string.Empty;
            this.txtDocTransporte.Text = string.Empty;
            this.txtManifiesto.Text = string.Empty;
            this.lblDocTransporte.Visible = false;
            this.lblManifiestoCarga.Visible = false;
            this.txtDocTransporte.Visible = false;
            this.txtManifiesto.Visible = false;

            FormProvider.Master.itemSendtoAppr.Enabled = true;
            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Habilita o deshbilita el header
        /// </summary>
        /// <param name="enable"></param>
        private void EnableHeader(bool enable)
        {
            this.masterPrefijoRec.EnableControl(enable);
            this.masterProveedor.EnableControl(enable);
            this.masterCodigoBS.EnableControl(enable);
            this.masterReferencia.EnableControl(enable);
            this.masterBodega.EnableControl(enable);
            this.masterPrefijoOC.EnableControl(enable);
            this.masterLugarEntrega.EnableControl(enable);
            this.txtOrdenCompNro.Enabled = enable;

            this.btnCargDescarg.Enabled = enable;
            this.btnTraer.Enabled = enable;
            this.btnQueryDocOC.Enabled = enable;
        }

        /// <summary>
        /// Valida los registros de las ordenes de compra
        /// </summary>
        /// <param name="fila">identificador de la fila</param>
        /// <returns></returns>
        private bool ValidateRow(int fila)
        {
            GridColumn col = new GridColumn();

            try
            {
                if (this._listOrdenCompraSelect.Count > 0)
                {
                    col = this.gvDetails.Columns[this.unboundPrefix + "CantidadRec"];
                    if (!this._listOrdenCompraSelect[fila].invSerialInd && this._listOrdenCompraSelect[fila].CantidadRec.Value > this._listOrdenCompraSelect[fila].CantidadOC.Value ||
                        this._listOrdenCompraSelect[fila].invSerialInd && this._listOrdenCompraSelect[fila].CantidadRec.Value > 1 ||
                        this._listOrdenCompraSelect[fila].invSerialInd && this._listOrdenCompraSelect[fila].CantidadRec.Value < 0)
                    {
                        this.gvDetails.FocusedRowHandle = fila;
                        this.gvDetails.SetColumnError(col, "Valor invalido");
                        return false;
                    }

                    col = this.gvDetails.Columns[this.unboundPrefix + "SerialID"];
                    if (this._listOrdenCompraSelect[fila].invSerialInd && this._listOrdenCompraSelect[fila].CantidadRec.Value.Value > 0)
                    {
                        if (string.IsNullOrEmpty(this._listOrdenCompraSelect[fila].SerialID.Value.Trim()))
                        {
                            this.gvDetails.FocusedRowHandle = fila;
                            this.gvDetails.SetColumnError(col, "Valor invalido");
                            return false;
                        }

                        DTO_acActivoControl activo = new DTO_acActivoControl();
                        activo.SerialID.Value = this._listOrdenCompraSelect[fila].SerialID.Value;
                        int result = this._bc.AdministrationModel.acActivoControl_GetByParameterForTranfer(activo).Count;
                        if (result > 0)
                        {
                            this.gvDetails.FocusedRowHandle = fila;
                            this.gvDetails.SetColumnError(col, DictionaryMessages.In_AlreadyExistSerial);
                            return false;
                        }
                    }

                    this.gvDetails.SetColumnError(col, null); 
                }
                return true;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-ValidateRow"));
                return false;
            }
        }

        /// <summary>
        /// Carga el valor de la tasa de cambio de acuerdo a los items seleccionados
        /// </summary>
        /// <returns>Retorna el valor de la tasa de cambio</returns>
        private decimal LoadTasaCambio()
        {
            try
            {
                this._tasaCambio = this._bc.AdministrationModel.TasaDeCambio_Get(this._monedaExtranjera, this.dtFecha.DateTime);
                this.txtTasaCambio.EditValue = this._tasaCambio;
                return this._tasaCambio;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs", "LoadTasaCambio"));
                return 0 ;
            }
        }

        /// <summary>
        /// Habilita o deshabilita los campos de transporte
        /// </summary>
        /// <param name="visibleInd">indica si oculto o no los controles</param>
        private void EnableFieldsTransporte(bool visibleInd)
        {
            //No requiere Doc de transporte
            this.lblDocTransporte.Visible = visibleInd;
            this.lblManifiestoCarga.Visible = visibleInd;
            this.txtDocTransporte.Visible = visibleInd;
            this.txtManifiesto.Visible = visibleInd;
            this.bodegaTransitoInd = visibleInd;
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
                FormProvider.Master.itemPrint.Visible = true;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                if (FormProvider.Master.LoadFormTB)
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Leave"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "DocumentAprobComplexForm.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos grilla de Detalles

        private void gvDetails_DataSourceChanged(object sender, EventArgs e)
        {
            if (this._listOrdenCompraSelect.Count > 0)
            {
                this.masterReferencia_det.Value = this._listOrdenCompraSelect[0].inReferenciaID.Value;
                this.txtDesc.Text = this._listOrdenCompraSelect[0].Descriptivo.Value;
                this.masterMarca.Value = this._listOrdenCompraSelect[0].MarcaInvID.Value;
                this.txtRefProveedor.Text = this._listOrdenCompraSelect[0].RefProveedor.Value;
                this.txtValorLocalOC.EditValue = this._listOrdenCompraSelect[0].ValorTotMLOC.Value; 
                this.txtValorExtrOC.EditValue = this._listOrdenCompraSelect[0].ValorTotMEOC.Value; 
            }
        }

        /// <summary>
        /// asigna controles a la grilla cuando sale de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                if (fieldName == "SerialID" && this._listOrdenCompraSelect.Count > 0)
                {
                    if (this._listOrdenCompraSelect[e.RowHandle].invSerialInd)
                        e.Column.OptionsColumn.AllowEdit = true;
                    else
                    {
                        e.Column.OptionsColumn.AllowEdit = false;
                        e.RepositoryItem = this.editEmpty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs", "gvDetails_CustomRowCellEdit"));
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            Type dataType = dto.GetType();
            int unboundPrefixLen = this.unboundPrefix.Length;

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
        private void gvDetails_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (!disableValidate && e.FocusedRowHandle >= 0 && this._listOrdenCompraSelect.Count > 0)
                {
                    this.masterReferencia_det.Value = this._listOrdenCompraSelect[e.FocusedRowHandle].inReferenciaID.Value;
                    this.txtDesc.Text = this._listOrdenCompraSelect[e.FocusedRowHandle].Descriptivo.Value;
                    this.txtRefProveedor.Text = this._listOrdenCompraSelect[e.FocusedRowHandle].RefProveedor.Value;
                    this.masterMarca.Value = this._listOrdenCompraSelect[e.FocusedRowHandle].MarcaInvID.Value;
                    this.txtValorLocalOC.EditValue = this._listOrdenCompraSelect[e.FocusedRowHandle].ValorTotMLOC.Value;
                    this.txtValorExtrOC.EditValue = this._listOrdenCompraSelect[e.FocusedRowHandle].ValorTotMEOC.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-gvDetails_FocusedRowChanged"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

                #region Se modifican CantidadRec
                if (fieldName == "CantidadRec")
                {
                    if (this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value == null)
                        this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value = 0;

                    if (!this._listOrdenCompraSelect[e.RowHandle].invSerialInd && this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value.Value > this._listOrdenCompraSelect[e.RowHandle].CantidadOC.Value.Value ||
                        this._listOrdenCompraSelect[e.RowHandle].invSerialInd && this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value.Value > 1 ||
                        this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value.Value < 0)
                        this.gvDetails.SetColumnError(e.Column, "Valor invalido");
                    else
                        this.gvDetails.SetColumnError(e.Column, null);
                }
                #endregion
                #region Se modifican Otros
                if (fieldName == "SerialID")
                {
                    if (this._listOrdenCompraSelect[e.RowHandle].invSerialInd && this._listOrdenCompraSelect[e.RowHandle].CantidadRec.Value.Value > 0 && string.IsNullOrEmpty(this._listOrdenCompraSelect[e.RowHandle].SerialID.Value.Trim()))
                        this.gvDetails.SetColumnError(e.Column, "Valor invalido");
                    else
                        this.gvDetails.SetColumnError(e.Column, null);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-gvDetails_CellValueChanged"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetails_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (!disableValidate && !this.ValidateRow(e.RowHandle))
                e.Allow = false;
        }
        #endregion

        #region Eventos del Header

        /// <summary>
        /// Evento que se ejecuta al entrar el control de prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoRec_Enter(object sender, EventArgs e)
        {
                this._prefijoFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijoRec_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._prefijoFocus)
                {
                    this._prefijoFocus = false;
                    if (this.masterPrefijoRec.ValidID)
                    {
                        this.EnableHeader(true);
                        this.masterPrefijoRec.EnableControl(false);
                        this.btnCargDescarg.Enabled = false;
                        this.masterProveedor.Focus();
                        //this.prefijoID = this.masterPrefijo.Value;
                        //this.txtPrefix.Text = this.prefijoID;
                    }
                    else
                    {
                        this.CleanHeader(true);
                        this.EnableHeader(false);
                        this.masterPrefijoRec.EnableControl(true);
                        //this.masterPrefijoRec.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-masterPrefijoRec_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el control de proveedor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Enter(object sender, EventArgs e)
        {
            if (this.masterPrefijoRec.ValidID)
                this._proveedorFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del control de proveedor
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterProveedor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._proveedorFocus)
                {
                    if (this.masterProveedor.ValidID)
                    {
                        this._proveedorFocus = false;
                        //if (this.masterProveedor.Value != this.proveedorID && this._detList != null && this._detList.Count > 0)
                        //{ 
                        //    string msg = "Nuevo valor no corresponde a "
                        //    if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                        
                        //}
                    }
                    else
                    {
                        if (this._listOrdenCompraSelect != null && this._listOrdenCompraSelect.Count > 0)
                        {
                            this.masterProveedor.Value = this.proveedorID;
                            this._proveedorFocus = false;
                        }
                        //else
                        //    this.masterProveedor.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-masterProveedor_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterControl_Enter(object sender, EventArgs e)
        {
            if (this.masterProveedor.ValidID)
                _controlFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterControl_Leave(object sender, EventArgs e)
        {
            try
            {
                ControlsUC.uc_MasterFind master  = (ControlsUC.uc_MasterFind)sender;
                if (!this._controlFocus)
                    masterProveedor.Focus();
                else
                {
                    this._controlFocus = false;
                    if (this.masterBodega.ValidID && master.Name.Equals(this.masterBodega.Name))
                    {
                        DTO_prProveedor prov = (DTO_prProveedor)this._bc.GetMasterDTO(Librerias.Project.AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.masterProveedor.Value, true);
                        //Valida el proveedor
                        if (prov.TipoProveedor.Value == (byte)TipoProveedor.Extranjero)
                        {
                            DTO_inBodega bod = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodega.Value, true);
                            DTO_inBodegaTipo bodTipo = (DTO_inBodegaTipo)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodegaTipo, false, bod.BodegaTipoID.Value, true);
                            this.masterLugarEntrega.Value = bod.LocFisicaID.Value;

                            foreach (DTO_prOrdenCompraResumen oc in this._listOrdenCompraSelect)
                            {
                                DTO_prOrdenCompra orden = this._bc.AdministrationModel.OrdenCompra_Load(AppDocuments.OrdenCompra, oc.PrefijoIDOC.Value, oc.DocumentoNroOC.Value.Value);
                                if (orden.HeaderOrdenCompra.Inconterm.Value != null)
                                {
                                    this._incoterm = (Incoterm)orden.HeaderOrdenCompra.Inconterm.Value;
                                    if (this._incoterm != Incoterm.FAS && this._incoterm != Incoterm.FCA && this._incoterm != Incoterm.EXW)
                                    {
                                        if (bodTipo.BodegaTipo.Value == (byte)TipoBodega.Transito)
                                            this.EnableFieldsTransporte(true);
                                        else
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_BodegaTransitoRequired));
                                            this.masterBodega.Focus();
                                        }
                                    }
                                    else
                                    {   //No requiere Doc de transporte
                                        this.EnableFieldsTransporte(false);
                                        if (bodTipo.BodegaTipo.Value != (byte)TipoBodega.PuertoFOB)
                                        {
                                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_BodegaPuertoRequired));
                                            this.masterBodega.Focus();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        { 
                            //No requiere Doc de transporte
                            this.EnableFieldsTransporte(false);
                            DTO_inBodega bod = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodega.Value, true);
                            this.masterLugarEntrega.Value = bod.LocFisicaID.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-masterControl_Leave"));
            }
        }

        /// <summary>
        /// Trae registros del detalle para hacer el documento de recibido
        /// </summary>
        private void btnTraer_Click(object sender, EventArgs e)
        {
            this.Filtros = new List<Tuple<string, string>>();
            if (this.masterProveedor.ValidID)
            {
                this.proveedorID = this.masterProveedor.Value;
                this.Filtros.Add(Tuple.Create("oc.ProveedorID ", this.masterProveedor.Value));
                this.Filtros.Add(Tuple.Create("oc.MonedaPago ", this.masterMonedaFilter.Value));
                if (this.masterCodigoBS.ValidID)
                    this.Filtros.Add(Tuple.Create("detOC.CodigoBSID ", this.masterCodigoBS.Value));
                if (this.masterReferencia.ValidID)
                    this.Filtros.Add(Tuple.Create("detOC.inReferenciaID ", this.masterReferencia.Value));
                if (this.masterPrefijoOC.ValidID)
                    this.Filtros.Add(Tuple.Create("ctrlOC.PrefijoID ", this.masterPrefijoOC.Value));
                if (this.masterProyecto.ValidID)
                    this.Filtros.Add(Tuple.Create("solCargo.ProyectoID ", this.masterProyecto.Value));
                if (!string.IsNullOrEmpty(this.txtOrdenCompNro.Text.Trim()) && this.txtOrdenCompNro.Text != "0")
                    this.Filtros.Add(Tuple.Create("ctrlOC.DocumentoNro ", this.txtOrdenCompNro.Text));

                this._listOrdenCompraSelect = this._bc.AdministrationModel.OrdenCompra_GetResumen(this.documentID, this._bc.AdministrationModel.User, ModulesPrefix.pr, this.Filtros);

                this.disableValidate = true;
                this.gcDetails.DataSource = this._listOrdenCompraSelect;
                this.disableValidate = false;

                if (this._listOrdenCompraSelect != null && this._listOrdenCompraSelect.Count > 0)
                {
                    this.btnCargDescarg.Enabled = true;
                    this.gcDetails.Enabled = true;
                }
                else
                {
                    this.btnCargDescarg.Enabled = false;
                    this.gcDetails.Enabled = false;
                }
                this.gvDetails.Focus();
            }
            else
            {
                string err = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode);
                MessageBox.Show(string.Format(err, this.masterProveedor.LabelRsx));
            }
        }

        /// <summary>
        /// Carga los cantidades recibidos con cantidades pendientes 
        /// </summary>
        private void btnCargDescarg_Click(object sender, EventArgs e)
        {
            if (this._listOrdenCompraSelect != null && this._listOrdenCompraSelect.Count > 0)
            {
                if (this._listOrdenCompraSelect.TrueForAll(det => det.CantidadOC.Value.Value == det.CantidadRec.Value.Value))
                {
                    this._listOrdenCompraSelect.ForEach(det => det.CantidadRec.Value = 0);
                }
                else
                {
                    this._listOrdenCompraSelect.ForEach(det => det.CantidadRec.Value = det.CantidadOC.Value.Value);
                }
            }
            this.gvDetails.PostEditor();
            this.gcDetails.RefreshDataSource();
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRecibidoNro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "\\d+") && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back)
                e.Handled = true;
            if (e.KeyChar == 46)
                e.Handled = true;
        }

        /// <summary>
        /// Valida que el usuario haya ingresado prefijo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtRecibidoNro_Enter(object sender, EventArgs e)
        {
            this._txtRecibidoNroFocus = true;
            if (!this.masterPrefijoRec.ValidID)
            {
                this._txtRecibidoNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijoRec.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijoRec.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtRecibidoNro_Leave(object sender, EventArgs e)
        {
            if (this._txtRecibidoNroFocus)
            {
                _txtRecibidoNroFocus = false;
                if (this.txtRecibidoNro.Text == string.Empty)
                    this.txtRecibidoNro.Text = "0";

                if (this.txtRecibidoNro.Text == "0")
                {
                    #region Nuevo Recibido
                    this.gcDetails.DataSource = null;
                    this._data = null;
                    this.EnableHeader(true);
                    this.masterPrefijoRec.EnableControl(false);
                    this.txtRecibidoNro.Enabled = false;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_prRecibido recibido = _bc.AdministrationModel.Recibido_Load(this.documentID, this.masterPrefijoRec.Value, Convert.ToInt32(this.txtRecibidoNro.Text));
                        //Valida si existe
                        if (recibido == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Recibido no Existe"));
                            this.txtRecibidoNro.Focus();
                            return;
                        }
                        foreach (var rec in recibido.Footer)
                        {
                            DTO_prOrdenCompraResumen dto = new DTO_prOrdenCompraResumen();
                            DTO_glDocumentoControl ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(rec.DetalleDocu.SolicitudDocuID.Value.Value);
                            dto.PrefDocSol = ctrl.PrefDoc.Value;
                            dto.FechaOC.Value = ctrl.FechaDoc.Value;
                            ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(rec.DetalleDocu.OrdCompraDocuID.Value.Value);
                            dto.PrefDocOC = ctrl.PrefDoc.Value;                                                                               
                            dto.ProyectoID.Value = recibido.DocCtrl.ProyectoID.Value;
                            dto.CodigoBSID.Value = rec.DetalleDocu.CodigoBSID.Value;
                            dto.Descriptivo.Value = rec.DetalleDocu.Descriptivo.Value;
                            dto.CantidadSol.Value = Math.Abs(rec.DetalleDocu.CantidadSol.Value.Value);
                            dto.CantidadOC.Value = Math.Abs(rec.DetalleDocu.CantidadOC.Value.Value);
                            dto.CantidadRec.Value = rec.DetalleDocu.CantidadRec.Value;
                            dto.UnidadInvID.Value = rec.DetalleDocu.UnidadInvID.Value;
                            dto.SerialID.Value = rec.DetalleDocu.SerialID.Value;
                            dto.MonedaIDOC.Value = rec.DetalleDocu.MonedaID.Value;
                            this._listOrdenCompraSelect.Add(dto);
                        }
                        this._data = recibido;
                        this.gcDetails.DataSource = this._listOrdenCompraSelect;
                        this.gcDetails.RefreshDataSource();
                        this.disableValidate = false;

                        FormProvider.Master.itemSendtoAppr.Enabled = false;
                        FormProvider.Master.itemSave.Enabled = false;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "txtRecibidoNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            List<int> docs = new List<int>();
            if(!this.btnQueryDocOC.Focused)
                docs.Add(AppDocuments.Recibido);
            else
                docs.Add(AppDocuments.OrdenCompra);
            ModalQueryDocument getDocControl = new ModalQueryDocument(docs);
            getDocControl.ShowDialog();
            if (getDocControl.DocumentoControl != null)
            {
                if (getDocControl.CopiadoInd)
                    this._copyData = true;
                if (!this.btnQueryDocOC.Focused)
                {
                    this.txtRecibidoNro.Enabled = true;
                    this.txtRecibidoNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijoRec.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtRecibidoNro.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
                else
                {
                    this.txtOrdenCompNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijoOC.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtOrdenCompNro.Focus();
                    this.btnQueryDocOC.Focus();
                }
            }
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void masterMonedaFilter_Leave(object sender, EventArgs e)
        {
            try
            {
                this._listOrdenCompraSelect = this._listOrdenCompraSelect.FindAll(x => x.MonedaPagoOC.Value == this.masterMonedaFilter.Value);
                this.gcDetails.DataSource = this._listOrdenCompraSelect;
                this.gcDetails.RefreshDataSource();
            }
            catch (Exception ex)
            {
                  MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-masterMonedaFilter_Leave"));
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
                this._listOrdenCompraSelect = new List<DTO_prOrdenCompraResumen>();
                this._data = new DTO_prRecibido();
                this.Filtros = new List<Tuple<string, string>>();

                this.disableValidate = true;
                this.gcDetails.DataSource = this._listOrdenCompraSelect;
                this.disableValidate = false;

                this._proveedorFocus = false;
                this._prefijoFocus = false;
                this._controlFocus = false;

                this.CleanHeader(true);
                this.EnableHeader(false);
                this.masterPrefijoRec.EnableControl(true);
                this.txtDesc.Text = string.Empty;
                this.masterMarca.Value = string.Empty;
                this.txtRefProveedor.Text = string.Empty;
                this.masterReferencia_det.Value = string.Empty;
                this.txtValorLocalOC.EditValue = 0;
                this.txtValorExtrOC.EditValue = 0;
                this.btnQueryDoc.Enabled = true;
                this.masterPrefijoRec.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                this.masterPrefijoRec.Focus();

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-TBNew"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion 
        /// </summary>
        public override void TBSendtoAppr()
        {
            this.gvDetails.PostEditor();
            try
            {
                string err = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode);
                if (this._listOrdenCompraSelect != null && this._listOrdenCompraSelect.Count > 0)
                {
                    #region Verifica que exista la cantidad a recibir
                    if (this._listOrdenCompraSelect.Exists(det => det.CantidadRec.Value.Value > 0))
                    {
                        #region Verifica que corresponda el proveedor
                        if (this.proveedorID == this.masterProveedor.Value)
                        {
                            #region Verifica que exista la Bodega y el Lugar de Entrega
                            if (this.masterBodega.ValidID)
                            {
                                if (this.bodegaTransitoInd && string.IsNullOrEmpty(this.txtDocTransporte.Text) && string.IsNullOrEmpty(this.txtManifiesto.Text))
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_TransporteManifiestoEmpty));
                                else
                                {
                                    List<string> od = this._listOrdenCompraSelect.FindAll(x=>x.CantidadRec.Value > 0).Select(x => x.PrefDocOC).Distinct().ToList();
                                    if (od.Count == 1)
                                    {
                                        if (this._listOrdenCompraSelect.TrueForAll(det => this.ValidateRow(det.Index)))
                                        {
                                            if (!this.masterLugarEntrega.ValidID)
                                            {
                                                DTO_inBodega bod = (DTO_inBodega)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, false, this.masterBodega.Value, true);
                                                this.masterLugarEntrega.Value = bod.LocFisicaID.Value;
                                                if (string.IsNullOrEmpty(bod.LocFisicaID.Value))
                                                    MessageBox.Show(string.Format(err, this.masterLugarEntrega.LabelRsx));

                                            }

                                            #region Load Recibido
                                            this._data = new DTO_prRecibido();
                                            #region Load Header
                                            this._data.Header.BodegaID.Value = this.masterBodega.Value;
                                            this._data.Header.ProveedorID.Value = this.proveedorID;
                                            this._data.Header.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                                            this._data.Header.LugarEntrega.Value = this.masterLugarEntrega.Value;
                                            #endregion
                                            #region Load DocumentoControl
                                            this._data.DocCtrl.EmpresaID.Value = this.empresaID;
                                            DTO_prProveedor proveedor = (DTO_prProveedor)this._bc.GetMasterDTO(NewAge.Librerias.Project.AppMasters.MasterType.Simple, AppMasters.prProveedor, false, this.proveedorID, true);
                                            this._data.DocCtrl.TerceroID.Value = proveedor.TerceroID.Value;
                                            this._data.DocCtrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                                            this._data.DocCtrl.ComprobanteID.Value = string.Empty;
                                            this._data.DocCtrl.ComprobanteIDNro.Value = 0;
                                            this._data.DocCtrl.MonedaID.Value = this.masterMonedaFilter.Value;
                                            this._data.DocCtrl.CuentaID.Value = string.Empty;
                                            this._data.DocCtrl.ProyectoID.Value = this.defProyecto;
                                            this._data.DocCtrl.CentroCostoID.Value = this.defCentroCosto;
                                            this._data.DocCtrl.LugarGeograficoID.Value = this.defLugarGeo;
                                            this._data.DocCtrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;
                                            this._data.DocCtrl.Fecha.Value = DateTime.Now;
                                            this._data.DocCtrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                                            this._data.DocCtrl.PrefijoID.Value = this.masterPrefijoRec.Value;
                                            this._data.DocCtrl.TasaCambioCONT.Value = this._tasaCambio;
                                            this._data.DocCtrl.TasaCambioDOCU.Value = this._tasaCambio;
                                            this._data.DocCtrl.DocumentoNro.Value = Convert.ToInt32(txtRecibidoNro.Text);
                                            this._data.DocCtrl.DocumentoID.Value = this.documentID;
                                            this._data.DocCtrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;
                                            this._data.DocCtrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                                            this._data.DocCtrl.seUsuarioID.Value = this.userID;
                                            this._data.DocCtrl.AreaFuncionalID.Value = this.areaFuncionalID;
                                            this._data.DocCtrl.ConsSaldo.Value = 0;
                                            this._data.DocCtrl.Estado.Value = (byte)EstadoDocControl.ParaAprobacion;
                                            this._data.DocCtrl.Observacion.Value = string.Empty;
                                            this._data.DocCtrl.FechaDoc.Value = this.dtFecha.DateTime;
                                            this._data.DocCtrl.Descripcion.Value = this.txtDocDesc.Text;
                                            this._data.DocCtrl.Valor.Value = 0;
                                            this._data.DocCtrl.Iva.Value = 0;

                                            #endregion
                                            #endregion

                                            FormProvider.Master.itemSendtoAppr.Enabled = false;
                                            FormProvider.Master.itemSave.Enabled = false;

                                            Thread process = new Thread(this.SendToApproveThread);
                                            process.Start();
                                        } 
                                    }
                                    else
                                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "Debe selecccionar solo una orden de compra para continuar"));

                                }
                            }
                            else
                                MessageBox.Show(string.Format(err, this.masterBodega.LabelRsx));
                            #endregion
                        }
                        else
                            MessageBox.Show(string.Format(err, this.masterProveedor.LabelRsx));
                        #endregion
                    }
                    else
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoData));
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-TBSave"));
            }
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                if (this._data != null && this._data.DocCtrl.NumeroDoc.Value.HasValue && this._data.DocCtrl.NumeroDoc.Value != 0)
                {
                    bool isPreliminar = this._data.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado ? false : true;
                    string reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(this.documentID, this._data.DocCtrl.NumeroDoc.Value.Value, isPreliminar, 0);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this._data.DocCtrl.NumeroDoc.Value, null, reportName);
                    Process.Start(fileURl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs", "TBPrint"));
            }
        }
        #endregion        

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                DTO_SerializedObject result = this._bc.AdministrationModel.Recibido_Guardar(this.documentID, this._data.DocCtrl, this._data.Header, this._listOrdenCompraSelect, out numeroDoc, this.txtDocTransporte.Text, this.txtManifiesto.Text);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                this._isOK = this._bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true, true);

                if (result.GetType() == typeof(DTO_Alarma))
                {
                    string reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(this.documentID, numeroDoc, true, 0);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null, reportName);
                    Process.Start(fileURl);
                }
                this.Invoke(this.saveDelegate);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Recibido.cs.cs", "Recibido.cs-SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        #endregion

        /// <summary>
        /// La tasa de cambio del recibido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTasaCambio_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this._tasaCambio = Convert.ToDecimal(this.txtTasaCambio.EditValue, System.Globalization.CultureInfo.InvariantCulture);
                if (this._tasaCambio != 0)
                {
                    //foreach (DTO_prOrdenCompraResumen det in this._listOrdenCompraSelect)
                    //{
                    //    det.TasaOrdenOC.Value = this._tasaCambio;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OrdenCompra.cs", "txtTasaCambio_TextChanged"));
            }
        }
    }
}
