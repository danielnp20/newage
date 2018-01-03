using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;
using DevExpress.XtraEditors.Controls;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class TrasladoPresupuesto : FormWithToolbar
    {
        #region Delegados

        private delegate void Save();
        private Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        private void SaveMethod()
        {
            this.presupuestoOrigen = new DTO_Presupuesto();
            this.presupuestoDestino = new DTO_Presupuesto();
            this.detListOrigen = new List<DTO_plPresupuestoDeta>();
            this.detListDestino = new List<DTO_plPresupuestoDeta>();
            this.EnableControls(true);
        }

        private delegate void SendToApprove();
        private SendToApprove sendToApproveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void SendToApproveMethod()
        {
            try
            {
                this.presupuestoOrigen = new DTO_Presupuesto();
                this.presupuestoDestino = new DTO_Presupuesto();
                this.detListOrigen = new List<DTO_plPresupuestoDeta>();
                this.detListDestino = new List<DTO_plPresupuestoDeta>();
                this.EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "sendToApproveDelegate"));
            }
        }

        private delegate void RefreshData();
        private RefreshData refreshDataDelegate;
        /// <summary>
        /// Delegado que actualiza o refresca el formulario 
        /// </summary>
        private void RefreshDataMethod()
        {
            try
            {
                this.dtPeriodOrigen.Enabled = false;
                this.masterProyectoOrigen.EnableControl(false);
                this.masterProyectoDestino.EnableControl(false);
                this.gcDetailOrigen.DataSource = this.detListOrigen;
                this.gcDetailDestino.DataSource = this.detListDestino;
                //this.gvDetailOrigen.FocusedRowHandle = 0;
                for (int i = 0; i < this.detListOrigen.Count; i++)
                    this.LoadParticiones(i, false);
                for (int i = 0; i < this.detListDestino.Count; i++)
                    this.LoadParticiones(i, false);
                //this.gcDetailOrigen.RefreshDataSource();
                this.gvDetailOrigen.MoveLast();
                this.gvDetailDestino.MoveLast();
                //this.LoadFooter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "sendToApproveDelegate"));
            }
        }

        #endregion

        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private int documentID;

        //Variables de MDI
        private FormTypes _frmType = FormTypes.Document;
        private string _frmName;
        private ModulesPrefix _frmModule;
        //Variables con valores x defecto (glControl)
        private string defLineaPresupuesto = string.Empty;
        //Variables de recursos 
        //Variables del formulario
        private DateTime periodo = DateTime.Now;
        private string proyectoIDOrigen = string.Empty;
        private string proyectoIDDestino = string.Empty;
        private bool validHeaderOrigen = false;
        private bool validHeaderDestino = false;
        private bool initData = false;
        private bool deleteOP = false;
        private bool loadME = false;
        private bool actividadLineaPresupInd = false;
        private bool validParticion = true;
        //Variables de documentos y detalles
        private bool isValid_Det_Origen = true;
        private bool isValid_Det_Destino = true;
        private bool disableValidate_Det_Origen = false;
        private bool disableValidate_Det_Destino = false;
        private DTO_Presupuesto presupuestoOrigen = new DTO_Presupuesto();
        private DTO_Presupuesto presupuestoDestino = new DTO_Presupuesto();
        private List<DTO_plPresupuestoDeta> detListOrigen = new List<DTO_plPresupuestoDeta>();
        private List<DTO_plPresupuestoDeta> detListDestino = new List<DTO_plPresupuestoDeta>();
        protected int numeroDocPresupOr = 0;
        private bool IsFocusedGridOrigen = true;
        private bool trasladoDirectoInd = false;
        //Variables para importar
        private PasteOpDTO pasteRet;
        private string format;
        private string formatSeparator = "\t";
        private string unboundPrefix = "Unbound_";
        //Filtros y actividades
        private DTO_glConsulta consulta = null;
        protected List<DTO_glConsultaFiltro> filtrosLineaPres;
        protected List<DTO_glConsultaFiltro> filtrosCentroCosto;
        private DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();

        #endregion Variables

        #region Propiedades

        /// <summary>
        /// Indica si un formulario modal esta abierto
        /// </summary>
        private bool IsModalFormOpened
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public TrasladoPresupuesto()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());
                FormProvider.Master.Form_Load(this, this._frmModule, this.documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de la actividad
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this.documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this.documentID.ToString()));
                }
                else
                {
                    string actividadFlujoID = actividades[0];
                    this.actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "TrasladoPresupuesto"));
            }
        }

        #region Funciones private del formulario

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.TrasladoPresupuesto;
                this._frmModule = ModulesPrefix.pl;
                #region Inicia variables
                //Carga los valores por defecto               
                this.defLineaPresupuesto = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LineaPresupXDefecto);
                this.actividadLineaPresupInd = this._bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ConsPresupuestalInd).Equals("1") ? true : false;
                //Tasa de cambio
                string multimonedaStr = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
                if (multimonedaStr == "1")
                {
                    this.loadME = true;
                    this.lblTasaCambioOrigen.Visible = true;
                    this.txtTasaCambioOrigen.Visible = true;
                    this.lblTasacambioDestino.Visible = true;
                    this.txtTasaCambioDestino.Visible = true;
                    this.lblMdaExtr1.Visible = true;
                    this.lblMdaExtr2.Visible = true;
                    this.lblMdaExtr3.Visible = true;
                    this.txt_Mes01_Extr.Visible = true;
                    this.txt_Mes02_Extr.Visible = true;
                    this.txt_Mes03_Extr.Visible = true;
                    this.txt_Mes04_Extr.Visible = true;
                    this.txt_Mes05_Extr.Visible = true;
                    this.txt_Mes06_Extr.Visible = true;
                    this.txt_Mes07_Extr.Visible = true;
                    this.txt_Mes08_Extr.Visible = true;
                    this.txt_Mes09_Extr.Visible = true;
                    this.txt_Mes10_Extr.Visible = true;
                    this.txt_Mes11_Extr.Visible = true;
                    this.txt_Mes12_Extr.Visible = true;
                }

                #endregion
                #region Inicia controles

                //Periodo
                _bc.InitPeriodUC(this.dtPeriodOrigen, 0);
                string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(periodoStr);
                this.dtPeriodOrigen.DateTime = this.periodo;
                this.dtPeriodOrigen.MinValue = this.periodo;

                //Maestras
                this._bc.InitMasterUC(this.masterProyectoOrigen, AppMasters.coProyecto, true, true, true, false);
                this._bc.InitMasterUC(this.masterProyectoDestino, AppMasters.coProyecto, true, true, true, false);
                this.AddColsDetalle();
                this.AddColsDetalleDestino();
                this.masterProyectoDestino.EnableControl(false);

                //Combo
                Dictionary<string, string> dicTipoProyecto = new Dictionary<string, string>();
                dicTipoProyecto.Add("1", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Capex));
                dicTipoProyecto.Add("2", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Opex));
                dicTipoProyecto.Add("3", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Inversion));
                dicTipoProyecto.Add("4", this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Administrativo));
                this.cmbTipoProyectoOrigen.Properties.DataSource = dicTipoProyecto;
                this.cmbTipoProyectoOrigen.EditValue = 1;
                this.cmbTipoProyectoDest.Properties.DataSource = dicTipoProyecto;
                this.cmbTipoProyectoDest.EditValue = 1;
                #endregion

                ///Controles
                this.gcDetailOrigen.EmbeddedNavigator.Buttons.CustomButtons[0].Visible = false;
                this.gcDetailOrigen.EmbeddedNavigator.Buttons.Remove.Visible = false;

                //Delegados
                this.saveDelegate = new Save(this.SaveMethod);
                this.sendToApproveDelegate = new SendToApprove(this.SendToApproveMethod);
                this.refreshDataDelegate = new RefreshData(this.RefreshDataMethod);

                this.GetFiltersLineaPresup();
                this.format = _bc.GetImportExportFormat(typeof(DTO_plPresupuestoDeta), this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuesto.cs", "InitControls"));
            }
        }

        /// <summary>
        /// Limpia / Habilita el formulario
        /// </summary>
        private void EnableControls(bool enable)
        {
            this.dtPeriodOrigen.Enabled = enable;
            this.masterProyectoOrigen.EnableControl(enable);
            //this.masterProyectoDestino.EnableControl(enable);

            this.txt_Mes01_Local.Enabled = !enable;
            this.txt_Mes02_Local.Enabled = !enable;
            this.txt_Mes03_Local.Enabled = !enable;
            this.txt_Mes04_Local.Enabled = !enable;
            this.txt_Mes05_Local.Enabled = !enable;
            this.txt_Mes06_Local.Enabled = !enable;
            this.txt_Mes07_Local.Enabled = !enable;
            this.txt_Mes08_Local.Enabled = !enable;
            this.txt_Mes09_Local.Enabled = !enable;
            this.txt_Mes10_Local.Enabled = !enable;
            this.txt_Mes11_Local.Enabled = !enable;
            this.txt_Mes12_Local.Enabled = !enable;
            this.txt_Mes01_Extr.Enabled = !enable;
            this.txt_Mes02_Extr.Enabled = !enable;
            this.txt_Mes03_Extr.Enabled = !enable;
            this.txt_Mes04_Extr.Enabled = !enable;
            this.txt_Mes05_Extr.Enabled = !enable;
            this.txt_Mes06_Extr.Enabled = !enable;
            this.txt_Mes07_Extr.Enabled = !enable;
            this.txt_Mes08_Extr.Enabled = !enable;
            this.txt_Mes09_Extr.Enabled = !enable;
            this.txt_Mes10_Extr.Enabled = !enable;
            this.txt_Mes11_Extr.Enabled = !enable;
            this.txt_Mes12_Extr.Enabled = !enable;

            if (enable)
            {
                this.disableValidate_Det_Origen = true;
                this.disableValidate_Det_Destino = true;

                this.initData = true;
                this.gcDetailOrigen.DataSource = this.detListOrigen;
                this.gcDetailDestino.DataSource = this.detListDestino;

                this.LoadDetails(true);
                this.initData = false;
                this.disableValidate_Det_Origen = false;
                this.disableValidate_Det_Destino = false;

                this.proyectoIDOrigen = string.Empty;
                this.proyectoIDDestino = string.Empty;
                this.masterProyectoOrigen.Value = string.Empty;
                this.masterProyectoDestino.Value = string.Empty;
                this.txtTasaCambioOrigen.Text = "0";
                this.txtTasaCambioDestino.Text = "0";

                this.txt_Mes01_Local.Text = "0";
                this.txt_Mes02_Local.Text = "0";
                this.txt_Mes03_Local.Text = "0";
                this.txt_Mes04_Local.Text = "0";
                this.txt_Mes05_Local.Text = "0";
                this.txt_Mes06_Local.Text = "0";
                this.txt_Mes07_Local.Text = "0";
                this.txt_Mes08_Local.Text = "0";
                this.txt_Mes09_Local.Text = "0";
                this.txt_Mes10_Local.Text = "0";
                this.txt_Mes11_Local.Text = "0";
                this.txt_Mes12_Local.Text = "0";

                this.txt_Mes01_Extr.Text = "0";
                this.txt_Mes02_Extr.Text = "0";
                this.txt_Mes03_Extr.Text = "0";
                this.txt_Mes04_Extr.Text = "0";
                this.txt_Mes05_Extr.Text = "0";
                this.txt_Mes06_Extr.Text = "0";
                this.txt_Mes07_Extr.Text = "0";
                this.txt_Mes08_Extr.Text = "0";
                this.txt_Mes09_Extr.Text = "0";
                this.txt_Mes10_Extr.Text = "0";
                this.txt_Mes11_Extr.Text = "0";
                this.txt_Mes12_Extr.Text = "0";

                this.dtPeriodOrigen.Focus();
            }
            else
                FormProvider.Master.itemNew.Enabled = true;
        }

        /// <summary>
        /// Habilita o deshabilita los controles del footer
        /// </summary>
        private void EnableFooter(bool enable)
        {
            if (enable)
            {
                this.txt_Mes01_Local.Enabled = this.dtPeriodOrigen.DateTime.Month == 1 ? true : false;
                this.txt_Mes02_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 2 ? true : false;
                this.txt_Mes03_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 3 ? true : false;
                this.txt_Mes04_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 4 ? true : false;
                this.txt_Mes05_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 5 ? true : false;
                this.txt_Mes06_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 6 ? true : false;
                this.txt_Mes07_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 7 ? true : false;
                this.txt_Mes08_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 8 ? true : false;
                this.txt_Mes09_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 9 ? true : false;
                this.txt_Mes10_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 10 ? true : false;
                this.txt_Mes11_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 11 ? true : false;
                this.txt_Mes12_Local.Enabled = this.dtPeriodOrigen.DateTime.Month <= 12 ? true : false;
                this.txt_Mes01_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month == 1 ? true : false;
                this.txt_Mes02_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 2 ? true : false;
                this.txt_Mes03_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 3 ? true : false;
                this.txt_Mes04_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 4 ? true : false;
                this.txt_Mes05_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 5 ? true : false;
                this.txt_Mes06_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 6 ? true : false;
                this.txt_Mes07_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 7 ? true : false;
                this.txt_Mes08_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 8 ? true : false;
                this.txt_Mes09_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 9 ? true : false;
                this.txt_Mes10_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 10 ? true : false;
                this.txt_Mes11_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 11 ? true : false;
                this.txt_Mes12_Extr.Enabled = this.dtPeriodOrigen.DateTime.Month <= 12 ? true : false;
            }
            else
            {
                this.txt_Mes01_Local.Enabled = false;
                this.txt_Mes02_Local.Enabled = false;
                this.txt_Mes03_Local.Enabled = false;
                this.txt_Mes04_Local.Enabled = false;
                this.txt_Mes05_Local.Enabled = false;
                this.txt_Mes06_Local.Enabled = false;
                this.txt_Mes07_Local.Enabled = false;
                this.txt_Mes08_Local.Enabled = false;
                this.txt_Mes09_Local.Enabled = false;
                this.txt_Mes10_Local.Enabled = false;
                this.txt_Mes11_Local.Enabled = false;
                this.txt_Mes12_Local.Enabled = false;
                this.txt_Mes01_Extr.Enabled = false;
                this.txt_Mes02_Extr.Enabled = false;
                this.txt_Mes03_Extr.Enabled = false;
                this.txt_Mes04_Extr.Enabled = false;
                this.txt_Mes05_Extr.Enabled = false;
                this.txt_Mes06_Extr.Enabled = false;
                this.txt_Mes07_Extr.Enabled = false;
                this.txt_Mes08_Extr.Enabled = false;
                this.txt_Mes09_Extr.Enabled = false;
                this.txt_Mes10_Extr.Enabled = false;
                this.txt_Mes11_Extr.Enabled = false;
                this.txt_Mes12_Extr.Enabled = false;
            }
        }

        /// <summary>
        /// Carga la info del formulario
        /// </summary>
        private void LoadData()
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.masterProyectoOrigen.ValidID)
                    {
                        this.initData = true;
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                        this.presupuestoOrigen = this._bc.AdministrationModel.Presupuesto_GetConsolidadoTotal(AppDocuments.Presupuesto, this.masterProyectoOrigen.Value, this.dtPeriodOrigen.DateTime,
                                                      Convert.ToByte(this.cmbTipoProyectoOrigen.EditValue), string.Empty, string.Empty, string.Empty);

                        if (presupuestoOrigen == null)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_PresupuestoNotExist));
                            FormProvider.Master.itemSave.Enabled = false;
                            this.validHeaderOrigen = false;
                            this.proyectoIDOrigen = string.Empty;
                            return;
                        }
                        else
                        {
                            if (this.presupuestoOrigen.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                                FormProvider.Master.itemSave.Enabled = false;
                                this.validHeaderOrigen = false;
                                return;
                            }
                            #region Carga Encabezado
                            this.proyectoIDOrigen = this.presupuestoOrigen.DocCtrl.ProyectoID.Value;
                            this.dtPeriodOrigen.DateTime = this.presupuestoOrigen.DocCtrl.PeriodoDoc.Value.Value;
                            this.txtTasaCambioOrigen.EditValue = this.presupuestoOrigen.DocCtrl.TasaCambioDOCU.Value.Value;
                            this.numeroDocPresupOr = this.presupuestoOrigen.DocCtrl.NumeroDoc.Value.Value;
                            #endregion
                            #region Verifica si Existen Adiciones
                            DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                            filter.DocumentoID.Value = AppDocuments.AdicionPresupuesto;
                            filter.ProyectoID.Value = this.masterProyectoOrigen.Value;
                            filter.PeriodoDoc.Value = this.dtPeriodOrigen.DateTime;
                            List<DTO_glDocumentoControl> listDocControl = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                            foreach (DTO_glDocumentoControl doc in listDocControl)
                            {
                                if (doc.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_DocAdicionPendientes));
                                    this.validHeaderOrigen = false;
                                    return;
                                }
                            }
                            #endregion
                            #region Verifica si Existen Reclasificaciones
                            DTO_glDocumentoControl filterReclas = new DTO_glDocumentoControl();
                            filterReclas.DocumentoID.Value = AppDocuments.ReclasifPresupuesto;
                            filterReclas.ProyectoID.Value = this.masterProyectoDestino.Value;
                            filterReclas.PeriodoDoc.Value = this.periodo;
                            List<DTO_glDocumentoControl> listDocControlReclas = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filterReclas);
                            foreach (DTO_glDocumentoControl doc in listDocControl)
                            {
                                if (doc.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_DocAdicionPendientes));
                                    this.validHeaderDestino = false;
                                    return;
                                }
                            }
                            #endregion
                            #region Carga Detalle(Verifica si existen traslados)
                            DTO_Presupuesto trasladoExist = this._bc.AdministrationModel.Presupuesto_GetNuevo(AppDocuments.TrasladoPresupuesto, this.masterProyectoOrigen.Value, this.periodo, false);
                            if (trasladoExist == null || trasladoExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Aprobado || trasladoExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Anulado || trasladoExist.DocCtrl.Estado.Value == (byte)EstadoDocControl.Revertido)
                            {
                                int i = 1;
                                foreach (DTO_plPresupuestoDeta item in presupuestoOrigen.Detalles)
                                {
                                    DTO_plPresupuestoDeta presNew = new DTO_plPresupuestoDeta(true);
                                    DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                                    DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                                    presNew.ProyectoID.Value = item.ProyectoID.Value;
                                    presNew.LineaPresupuestoID.Value = lineaPres.ID.Value;
                                    presNew.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                                    presNew.CentroCostoID.Value = centroCto.ID.Value;
                                    presNew.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                                    presNew.VlrSaldoAntLoc.Value = item.VlrSaldoAntLoc.Value;
                                    presNew.VlrSaldoAntExtr.Value = item.VlrSaldoAntExtr.Value;
                                    presNew.VlrNuevoSaldoLoc.Value = item.VlrNuevoSaldoLoc.Value;
                                    presNew.VlrNuevoSaldoExtr.Value = item.VlrNuevoSaldoExtr.Value;
                                    presNew.LoadParticionLocalInd = false;
                                    presNew.LoadParticionExtrInd = false;
                                    presNew.NewRowPresup = false;
                                    presNew.Consecutivo.Value = i;
                                    this.detListOrigen.Add(presNew);
                                    i++;
                                }
                                this.presupuestoOrigen = null;
                            }
                            else
                            {
                                int i = 1;
                                foreach (DTO_plPresupuestoDeta item in trasladoExist.Detalles)
                                {
                                    DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                                    DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                                    item.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                                    item.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                                    item.VlrMvtoLocal.Value = Math.Round(item.ValorLoc00.Value.Value + item.ValorLoc01.Value.Value + item.ValorLoc02.Value.Value + item.ValorLoc03.Value.Value
                                                              + item.ValorLoc04.Value.Value + item.ValorLoc05.Value.Value + item.ValorLoc06.Value.Value + item.ValorLoc07.Value.Value + item.ValorLoc08.Value.Value
                                                              + item.ValorLoc09.Value.Value + item.ValorLoc10.Value.Value + item.ValorLoc11.Value.Value + item.ValorLoc12.Value.Value);
                                    item.VlrMvtoExtr.Value = Math.Round(item.ValorExt00.Value.Value + item.ValorExt01.Value.Value + item.ValorExt02.Value.Value + item.ValorExt03.Value.Value
                                                             + item.ValorExt04.Value.Value + item.ValorExt05.Value.Value + item.ValorExt06.Value.Value + item.ValorExt07.Value.Value + item.ValorExt08.Value.Value
                                                             + item.ValorExt09.Value.Value + item.ValorExt10.Value.Value + item.ValorExt11.Value.Value + item.ValorExt12.Value.Value);
                                    item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                                    item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                                    item.LoadParticionLocalInd = false;
                                    item.LoadParticionExtrInd = false;
                                    item.NewRowPresup = false;
                                    item.Consecutivo.Value = i;
                                    if (item.ProyectoID.Value == this.masterProyectoOrigen.Value)
                                        this.detListOrigen.Add(item);
                                    else
                                        this.detListDestino.Add(item);
                                    i++;
                                }
                                this.presupuestoOrigen.DocCtrl = trasladoExist.DocCtrl;
                                this.presupuestoOrigen.Detalles = this.detListOrigen;
                                this.presupuestoDestino.DocCtrl = trasladoExist.DocCtrl;
                                this.presupuestoDestino.Detalles = this.detListDestino;
                            }
                            #endregion

                            this.validHeaderOrigen = true;
                            this.dtPeriodOrigen.Enabled = false;
                            this.masterProyectoOrigen.EnableControl(false);
                            this.masterProyectoDestino.EnableControl(true);
                            if (trasladoExist != null && trasladoExist.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado && trasladoExist.DocCtrl.Estado.Value != (byte)EstadoDocControl.Anulado && trasladoExist.DocCtrl.Estado.Value != (byte)EstadoDocControl.Revertido)
                            {
                                this.proyectoIDDestino = this.detListDestino.First().ProyectoID.Value;
                                this.masterProyectoDestino.Value = this.proyectoIDDestino;
                                this.txtTasaCambioDestino.EditValue = trasladoExist.DocCtrl.TasaCambioDOCU.Value.Value;
                                this.validHeaderDestino = true;
                                this.masterProyectoDestino.EnableControl(false);
                                this.initData = false;
                                this.gcDetailDestino.DataSource = this.detListDestino;
                                this.gcDetailDestino.RefreshDataSource();
                                this.isValid_Det_Destino = true;
                            }
                        }
                        this.gcDetailOrigen.DataSource = this.detListOrigen;
                        this.gcDetailOrigen.RefreshDataSource();
                        this.isValid_Det_Origen = true;
                        this.LoadDetails(true);
                        this.initData = false;

                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterProyectoOrigen.LabelRsx, this.masterProyectoOrigen.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeaderOrigen = false;
                    }
                }
                else
                {
                    if (this.masterProyectoDestino.ValidID)
                    {
                        this.initData = true;
                        FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                        this.presupuestoDestino = this._bc.AdministrationModel.Presupuesto_GetConsolidadoTotal(AppDocuments.Presupuesto, this.masterProyectoDestino.Value, this.periodo,
                                                       Convert.ToByte(this.cmbTipoProyectoDest.EditValue), string.Empty, string.Empty, string.Empty);

                        if (presupuestoDestino == null)
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_PresupuestoNotExist));
                            FormProvider.Master.itemSave.Enabled = false;
                            this.validHeaderDestino = false;
                            this.proyectoIDDestino = string.Empty;
                            return;
                        }
                        else
                        {
                            if (this.presupuestoDestino.DocCtrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                            {
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EstateInvalid));
                                FormProvider.Master.itemSave.Enabled = false;
                                this.validHeaderDestino = false;
                                return;
                            }
                            #region Carga Encabezado
                            this.proyectoIDDestino = this.presupuestoDestino.DocCtrl.ProyectoID.Value;
                            this.txtTasaCambioDestino.EditValue = this.presupuestoDestino.DocCtrl.TasaCambioDOCU.Value.Value;
                            #endregion
                            #region Verifica si Existen Adiciones
                            DTO_glDocumentoControl filter = new DTO_glDocumentoControl();
                            filter.DocumentoID.Value = AppDocuments.AdicionPresupuesto;
                            filter.ProyectoID.Value = this.masterProyectoDestino.Value;
                            filter.PeriodoDoc.Value = this.periodo;
                            List<DTO_glDocumentoControl> listDocControl = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filter);
                            foreach (DTO_glDocumentoControl doc in listDocControl)
                            {
                                if (doc.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_DocAdicionPendientes));
                                    this.validHeaderDestino = false;
                                    return;
                                }
                            }
                            #endregion
                            #region Verifica si Existen Reclasificaciones
                            DTO_glDocumentoControl filterReclas = new DTO_glDocumentoControl();
                            filterReclas.DocumentoID.Value = AppDocuments.ReclasifPresupuesto;
                            filterReclas.ProyectoID.Value = this.masterProyectoDestino.Value;
                            filterReclas.PeriodoDoc.Value = this.periodo;
                            List<DTO_glDocumentoControl> listDocControlReclas = this._bc.AdministrationModel.glDocumentoControl_GetByParameter(filterReclas);
                            foreach (DTO_glDocumentoControl doc in listDocControl)
                            {
                                if (doc.Estado.Value != (byte)EstadoDocControl.Aprobado)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_DocAdicionPendientes));
                                    this.validHeaderDestino = false;
                                    return;
                                }
                            }
                            #endregion
                            #region Carga Detalle(Verifica si existen)
                            int i = 1;
                            foreach (DTO_plPresupuestoDeta item in presupuestoDestino.Detalles)
                            {
                                DTO_plPresupuestoDeta presNew = new DTO_plPresupuestoDeta(true);
                                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, item.LineaPresupuestoID.Value, true);
                                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, item.CentroCostoID.Value, true);
                                presNew.ProyectoID.Value = item.ProyectoID.Value;
                                presNew.LineaPresupuestoID.Value = lineaPres.ID.Value;
                                presNew.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                                presNew.CentroCostoID.Value = centroCto.ID.Value;
                                presNew.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
                                presNew.VlrSaldoAntLoc.Value = item.VlrSaldoAntLoc.Value;
                                presNew.VlrSaldoAntExtr.Value = item.VlrSaldoAntExtr.Value;
                                presNew.VlrNuevoSaldoLoc.Value = item.VlrNuevoSaldoLoc.Value;
                                presNew.VlrNuevoSaldoExtr.Value = item.VlrNuevoSaldoExtr.Value;
                                presNew.LoadParticionLocalInd = false;
                                presNew.LoadParticionExtrInd = false;
                                presNew.NewRowPresup = false;
                                presNew.Consecutivo.Value = i;
                                this.detListDestino.Add(presNew);
                                i++;
                            }
                            this.presupuestoDestino = null;
                            #endregion
                            this.validHeaderDestino = true;
                            this.masterProyectoDestino.EnableControl(false);
                        }
                        this.gcDetailDestino.DataSource = this.detListDestino;
                        this.gcDetailDestino.RefreshDataSource();
                        this.isValid_Det_Destino = true;
                        this.LoadDetails(true);
                        this.initData = false;
                    }
                    else
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                        MessageBox.Show(string.Format(msg, this.masterProyectoDestino.LabelRsx, this.masterProyectoDestino.Value));
                        FormProvider.Master.itemSave.Enabled = false;
                        this.validHeaderDestino = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuesto.cs", "LoadData"));
            }
        }

        /// <summary>
        /// Pregunta si desea reemplazar el documento actual por una nueva fuente de datos
        /// </summary>
        /// <returns></returns>
        private bool ReplaceDocument()
        {
            string msgTitleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
            string msgNewDoc = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NewDocument);

            if (MessageBox.Show(msgNewDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                return true;

            return false;
        }

        /// <summary>
        /// Carga la info de la grilla del detalle
        /// </summary>
        private void LoadDetails(bool isDoc)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    this.disableValidate_Det_Origen = true;
                    this.gcDetailOrigen.DataSource = this.detListOrigen;

                    this.gvDetailOrigen.RefreshData();
                    this.gvDetailOrigen.FocusedRowHandle = this.gvDetailOrigen.DataRowCount - 1;

                    this.LoadParticiones(this.gvDetailOrigen.FocusedRowHandle, isDoc);
                    this.EnableFooter(true);

                    this.disableValidate_Det_Origen = false;
                }
                else
                {
                    this.disableValidate_Det_Destino = true;
                    this.gcDetailDestino.DataSource = this.detListDestino;

                    this.gvDetailDestino.RefreshData();
                    this.gvDetailDestino.FocusedRowHandle = this.gvDetailDestino.DataRowCount - 1;

                    this.LoadParticiones(this.gvDetailDestino.FocusedRowHandle, isDoc);
                    this.EnableFooter(true);

                    this.disableValidate_Det_Destino = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "LoadDetails"));
            }
        }

        /// <summary>
        /// Carga las particiones del detalle
        /// </summary>
        private void LoadParticiones(int index, bool isDoc, TipoMoneda loadByMoneda = TipoMoneda.Both)
        {
            try
            {
                int numMeses = 12 - this.dtPeriodOrigen.DateTime.Month + 1;
                bool onlyFija = true;
                if (this.IsFocusedGridOrigen)
                {
                    decimal tc = Convert.ToDecimal(this.txtTasaCambioOrigen.EditValue, CultureInfo.InvariantCulture);
                    if (this.detListOrigen.Count > 0)
                        if (isDoc)
                        {
                            #region Calculo de totales
                            this.txt_Mes01_Local.EditValue = this.detListOrigen[index].ValorLoc01.Value;
                            this.txt_Mes02_Local.EditValue = this.detListOrigen[index].ValorLoc02.Value;
                            this.txt_Mes03_Local.EditValue = this.detListOrigen[index].ValorLoc03.Value;
                            this.txt_Mes04_Local.EditValue = this.detListOrigen[index].ValorLoc04.Value;
                            this.txt_Mes05_Local.EditValue = this.detListOrigen[index].ValorLoc05.Value;
                            this.txt_Mes06_Local.EditValue = this.detListOrigen[index].ValorLoc06.Value;
                            this.txt_Mes07_Local.EditValue = this.detListOrigen[index].ValorLoc07.Value;
                            this.txt_Mes08_Local.EditValue = this.detListOrigen[index].ValorLoc08.Value;
                            this.txt_Mes09_Local.EditValue = this.detListOrigen[index].ValorLoc09.Value;
                            this.txt_Mes10_Local.EditValue = this.detListOrigen[index].ValorLoc10.Value;
                            this.txt_Mes11_Local.EditValue = this.detListOrigen[index].ValorLoc11.Value;
                            this.txt_Mes12_Local.EditValue = this.detListOrigen[index].ValorLoc12.Value;

                            this.txt_Mes01_Extr.EditValue = this.detListOrigen[index].ValorExt01.Value;
                            this.txt_Mes02_Extr.EditValue = this.detListOrigen[index].ValorExt02.Value;
                            this.txt_Mes03_Extr.EditValue = this.detListOrigen[index].ValorExt03.Value;
                            this.txt_Mes04_Extr.EditValue = this.detListOrigen[index].ValorExt04.Value;
                            this.txt_Mes05_Extr.EditValue = this.detListOrigen[index].ValorExt05.Value;
                            this.txt_Mes06_Extr.EditValue = this.detListOrigen[index].ValorExt06.Value;
                            this.txt_Mes07_Extr.EditValue = this.detListOrigen[index].ValorExt07.Value;
                            this.txt_Mes08_Extr.EditValue = this.detListOrigen[index].ValorExt08.Value;
                            this.txt_Mes09_Extr.EditValue = this.detListOrigen[index].ValorExt09.Value;
                            this.txt_Mes10_Extr.EditValue = this.detListOrigen[index].ValorExt10.Value;
                            this.txt_Mes11_Extr.EditValue = this.detListOrigen[index].ValorExt11.Value;
                            this.txt_Mes12_Extr.EditValue = this.detListOrigen[index].ValorExt12.Value;
                            #endregion
                        }
                        else
                        {

                            if (this.actividadLineaPresupInd)
                            {
                                //Obtiene el Control Costo a traves de la LineaPresupuesto y la actividad
                                Dictionary<string, string> pksActLinea = new Dictionary<string, string>();
                                pksActLinea.Add("LineaPresupuestoID", this.detListOrigen[index].LineaPresupuestoID.Value);
                                pksActLinea.Add("ActividadID", this.detListOrigen[index].ActividadID.Value);
                                DTO_plActividadLineaPresupuestal actividadLinea = (DTO_plActividadLineaPresupuestal)_bc.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pksActLinea, true);
                                if (actividadLinea != null)
                                {
                                    if (actividadLinea.ControlCosto.Value == (byte)ControlCosto.Variable || actividadLinea.ControlCosto.Value == (byte)ControlCosto.Estacionario)
                                    {
                                        //Distribucion Variable Mensual
                                        #region Asigna particion variable segun Tipo moneda
                                        DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyectoOrigen.Value, true);
                                        DTO_glLocFisica locFisica = (DTO_glLocFisica)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);

                                        if (locFisica != null)
                                        {
                                            Dictionary<string, string> pks = new Dictionary<string, string>();
                                            pks.Add("PeriodoID", this.dtPeriodOrigen.DateTime.ToShortDateString());
                                            pks.Add("AreaFisicaID", locFisica.AreaFisica.Value);
                                            pks.Add("TipoCosteo", actividadLinea.ControlCosto.Value.ToString());
                                            DTO_plDistribucionCampo distribxMes = (DTO_plDistribucionCampo)_bc.GetMasterComplexDTO(AppMasters.plDistribucionCampo, pks, true);

                                            if (distribxMes != null)
                                            {
                                                #region Calcula % de Ajuste para que la distribucion siempre sea del 100% sin importar el periodo actual
                                                int periodo = this.dtPeriodOrigen.DateTime.Month;
                                                decimal porcentajeAjuste = 0;
                                                switch (periodo)
                                                {
                                                    case 1: //Enero
                                                        porcentajeAjuste = 100 / 100;
                                                        break;
                                                    case 2: //Febrero
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje02.Value.Value + distribxMes.Porcentaje03.Value.Value +
                                                                                distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                                distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 3: //Marzo
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje03.Value.Value + distribxMes.Porcentaje04.Value.Value +
                                                                                distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                                distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 4: //Abril
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                                distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 5: //Mayo
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                                distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 6: //Junio
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 7: //Julio
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 8: //Agosto
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 9: //Septiembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 10: //Octubre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 11: //Noviembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 12: //Diciembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                }
                                                #endregion

                                                switch (loadByMoneda)
                                                {
                                                    case TipoMoneda.Local:
                                                        #region Particion Mda Local
                                                        //Enero
                                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                            this.txt_Mes01_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Local.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                            this.txt_Mes02_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Local.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                            this.txt_Mes03_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Local.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                            this.txt_Mes04_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Local.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                            this.txt_Mes05_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Local.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                            this.txt_Mes06_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Local.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                            this.txt_Mes07_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Local.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                            this.txt_Mes08_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Local.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                            this.txt_Mes09_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Local.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                            this.txt_Mes10_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Local.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                            this.txt_Mes11_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Local.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                        #endregion
                                                        break;
                                                    case TipoMoneda.Foreign:
                                                        #region Particion Mda Extr
                                                        //Enero
                                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                            this.txt_Mes01_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Extr.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                            this.txt_Mes02_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Extr.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                            this.txt_Mes03_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Extr.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                            this.txt_Mes04_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Extr.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                            this.txt_Mes05_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Extr.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                            this.txt_Mes06_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Extr.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                            this.txt_Mes07_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Extr.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                            this.txt_Mes08_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Extr.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                            this.txt_Mes09_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Extr.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                            this.txt_Mes10_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Extr.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                            this.txt_Mes11_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Extr.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                        #endregion
                                                        break;
                                                    case TipoMoneda.Both:
                                                        #region Particion Mda Local
                                                        if (this.detListOrigen[index].LoadParticionLocalInd)
                                                        {
                                                            //Enero
                                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                                this.txt_Mes01_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes01_Local.EditValue = "0";
                                                            //Febrero
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                                this.txt_Mes02_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes02_Local.EditValue = "0";
                                                            //Marzo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                                this.txt_Mes03_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes03_Local.EditValue = "0";
                                                            //Abril
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                                this.txt_Mes04_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes04_Local.EditValue = "0";
                                                            //Mayo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                                this.txt_Mes05_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes05_Local.EditValue = "0";
                                                            //Junio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                                this.txt_Mes06_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes06_Local.EditValue = "0";
                                                            //Julio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                                this.txt_Mes07_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes07_Local.EditValue = "0";
                                                            //Agosto
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                                this.txt_Mes08_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes08_Local.EditValue = "0";
                                                            //Septiembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                                this.txt_Mes09_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes09_Local.EditValue = "0";
                                                            //Octubre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                                this.txt_Mes10_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes10_Local.EditValue = "0";
                                                            //Noviembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                                this.txt_Mes11_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes11_Local.EditValue = "0";
                                                            //Diciembre
                                                            this.txt_Mes12_Local.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                        }
                                                        else
                                                        {
                                                            this.txt_Mes01_Local.EditValue = this.detListOrigen[index].ValorLoc01.Value;
                                                            this.txt_Mes02_Local.EditValue = this.detListOrigen[index].ValorLoc02.Value;
                                                            this.txt_Mes03_Local.EditValue = this.detListOrigen[index].ValorLoc03.Value;
                                                            this.txt_Mes04_Local.EditValue = this.detListOrigen[index].ValorLoc04.Value;
                                                            this.txt_Mes05_Local.EditValue = this.detListOrigen[index].ValorLoc05.Value;
                                                            this.txt_Mes06_Local.EditValue = this.detListOrigen[index].ValorLoc06.Value;
                                                            this.txt_Mes07_Local.EditValue = this.detListOrigen[index].ValorLoc07.Value;
                                                            this.txt_Mes08_Local.EditValue = this.detListOrigen[index].ValorLoc08.Value;
                                                            this.txt_Mes09_Local.EditValue = this.detListOrigen[index].ValorLoc09.Value;
                                                            this.txt_Mes10_Local.EditValue = this.detListOrigen[index].ValorLoc10.Value;
                                                            this.txt_Mes11_Local.EditValue = this.detListOrigen[index].ValorLoc11.Value;
                                                            this.txt_Mes12_Local.EditValue = this.detListOrigen[index].ValorLoc12.Value;
                                                        }
                                                        #endregion
                                                        #region Particion Mda Extr
                                                        if (this.detListOrigen[index].LoadParticionExtrInd)
                                                        {
                                                            //Enero
                                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                                this.txt_Mes01_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes01_Extr.EditValue = "0";
                                                            //Febrero
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                                this.txt_Mes02_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes02_Extr.EditValue = "0";
                                                            //Marzo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                                this.txt_Mes03_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes03_Extr.EditValue = "0";
                                                            //Abril
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                                this.txt_Mes04_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes04_Extr.EditValue = "0";
                                                            //Mayo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                                this.txt_Mes05_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes05_Extr.EditValue = "0";
                                                            //Junio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                                this.txt_Mes06_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes06_Extr.EditValue = "0";
                                                            //Julio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                                this.txt_Mes07_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes07_Extr.EditValue = "0";
                                                            //Agosto
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                                this.txt_Mes08_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes08_Extr.EditValue = "0";
                                                            //Septiembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                                this.txt_Mes09_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes09_Extr.EditValue = "0";
                                                            //Octubre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                                this.txt_Mes10_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes10_Extr.EditValue = "0";
                                                            //Noviembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                                this.txt_Mes11_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes11_Extr.EditValue = "0";
                                                            //Diciembre
                                                            this.txt_Mes12_Extr.EditValue = Math.Round((this.detListOrigen[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                        }
                                                        else
                                                        {
                                                            this.txt_Mes01_Extr.EditValue = this.detListOrigen[index].ValorExt01.Value;
                                                            this.txt_Mes02_Extr.EditValue = this.detListOrigen[index].ValorExt02.Value;
                                                            this.txt_Mes03_Extr.EditValue = this.detListOrigen[index].ValorExt03.Value;
                                                            this.txt_Mes04_Extr.EditValue = this.detListOrigen[index].ValorExt04.Value;
                                                            this.txt_Mes05_Extr.EditValue = this.detListOrigen[index].ValorExt05.Value;
                                                            this.txt_Mes06_Extr.EditValue = this.detListOrigen[index].ValorExt06.Value;
                                                            this.txt_Mes07_Extr.EditValue = this.detListOrigen[index].ValorExt07.Value;
                                                            this.txt_Mes08_Extr.EditValue = this.detListOrigen[index].ValorExt08.Value;
                                                            this.txt_Mes09_Extr.EditValue = this.detListOrigen[index].ValorExt09.Value;
                                                            this.txt_Mes10_Extr.EditValue = this.detListOrigen[index].ValorExt10.Value;
                                                            this.txt_Mes11_Extr.EditValue = this.detListOrigen[index].ValorExt11.Value;
                                                            this.txt_Mes12_Extr.EditValue = this.detListOrigen[index].ValorExt12.Value;
                                                        }
                                                        #endregion
                                                        break;
                                                }
                                                onlyFija = false;
                                                validParticion = true;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            if (onlyFija)
                            {
                                //Distribucion Fijo Mensual
                                #region Asigna particion segun el Tipo de Moneda
                                decimal vlrMesExtr = 0;
                                decimal vlrMesLocal = 0;
                                switch (loadByMoneda)
                                {
                                    case TipoMoneda.Local:
                                        #region Particion Mda Local
                                        vlrMesLocal = Math.Round(this.detListOrigen[index].VlrMvtoLocal.Value.Value / numMeses, 2);
                                        //Enero
                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                            this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes01_Local.EditValue = 0;
                                        //Febrero
                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                            this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes02_Local.EditValue = 0;
                                        //Marzo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                            this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes03_Local.EditValue = 0;
                                        //Abril
                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                            this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes04_Local.EditValue = 0;
                                        //Mayo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                            this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes05_Local.EditValue = 0;
                                        //Junio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                            this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes06_Local.EditValue = 0;
                                        //Julio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                            this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes07_Local.EditValue = 0;
                                        //Agosto
                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                            this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes08_Local.EditValue = 0;
                                        //Septiembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                            this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes09_Local.EditValue = 0;
                                        //Octubre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                            this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes10_Local.EditValue = 0;
                                        //Noviembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                            this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes11_Local.EditValue = 0;
                                        //Diciembre
                                        this.txt_Mes12_Local.EditValue = vlrMesLocal;
                                        #endregion
                                        break;
                                    case TipoMoneda.Foreign:
                                        #region Particion Mda Extr
                                        vlrMesExtr = Math.Round(this.detListOrigen[index].VlrMvtoExtr.Value.Value / numMeses, 2);

                                        //Enero
                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                            this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes01_Extr.EditValue = 0;
                                        //Febrero
                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                            this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes02_Extr.EditValue = 0;
                                        //Marzo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                            this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes03_Extr.EditValue = 0;
                                        //Abril
                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                            this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes04_Extr.EditValue = 0;
                                        //Mayo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                            this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes05_Extr.EditValue = 0;
                                        //Junio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                            this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes06_Extr.EditValue = 0;
                                        //Julio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                            this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes07_Extr.EditValue = 0;
                                        //Agosto
                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                            this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes08_Extr.EditValue = 0;
                                        //Septiembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                            this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes09_Extr.EditValue = 0;
                                        //Octubre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                            this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes10_Extr.EditValue = 0;
                                        //Noviembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                            this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes11_Extr.EditValue = 0;
                                        //Diciembre
                                        this.txt_Mes12_Extr.EditValue = vlrMesExtr;
                                        #endregion
                                        break;
                                    case TipoMoneda.Both:
                                        #region Particion Mda Local
                                        if (this.detListOrigen[index].LoadParticionLocalInd)
                                        {
                                            vlrMesLocal = Math.Round(this.detListOrigen[index].VlrMvtoLocal.Value.Value / numMeses, 2);

                                            //Enero
                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes01_Local.EditValue = 0;
                                            //Febrero
                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes02_Local.EditValue = 0;
                                            //Marzo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes03_Local.EditValue = 0;
                                            //Abril
                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes04_Local.EditValue = 0;
                                            //Mayo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes05_Local.EditValue = 0;
                                            //Junio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes06_Local.EditValue = 0;
                                            //Julio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes07_Local.EditValue = 0;
                                            //Agosto
                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes08_Local.EditValue = 0;
                                            //Septiembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes09_Local.EditValue = 0;
                                            //Octubre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes10_Local.EditValue = 0;
                                            //Noviembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes11_Local.EditValue = 0;
                                            //Diciembre
                                            this.txt_Mes12_Local.EditValue = vlrMesLocal;

                                        }
                                        else
                                        {
                                            this.txt_Mes01_Local.EditValue = this.detListOrigen[index].ValorLoc01.Value;
                                            this.txt_Mes02_Local.EditValue = this.detListOrigen[index].ValorLoc02.Value;
                                            this.txt_Mes03_Local.EditValue = this.detListOrigen[index].ValorLoc03.Value;
                                            this.txt_Mes04_Local.EditValue = this.detListOrigen[index].ValorLoc04.Value;
                                            this.txt_Mes05_Local.EditValue = this.detListOrigen[index].ValorLoc05.Value;
                                            this.txt_Mes06_Local.EditValue = this.detListOrigen[index].ValorLoc06.Value;
                                            this.txt_Mes07_Local.EditValue = this.detListOrigen[index].ValorLoc07.Value;
                                            this.txt_Mes08_Local.EditValue = this.detListOrigen[index].ValorLoc08.Value;
                                            this.txt_Mes09_Local.EditValue = this.detListOrigen[index].ValorLoc09.Value;
                                            this.txt_Mes10_Local.EditValue = this.detListOrigen[index].ValorLoc10.Value;
                                            this.txt_Mes11_Local.EditValue = this.detListOrigen[index].ValorLoc11.Value;
                                            this.txt_Mes12_Local.EditValue = this.detListOrigen[index].ValorLoc12.Value;
                                        }
                                        #endregion
                                        #region Particion Mda Extr
                                        if (this.detListOrigen[index].LoadParticionExtrInd)
                                        {
                                            vlrMesExtr = Math.Round(this.detListOrigen[index].VlrMvtoExtr.Value.Value / numMeses, 2);
                                            //Enero
                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes01_Extr.EditValue = 0;
                                            //Febrero
                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes02_Extr.EditValue = 0;
                                            //Marzo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes03_Extr.EditValue = 0;
                                            //Abril
                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes04_Extr.EditValue = 0;
                                            //Mayo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes05_Extr.EditValue = 0;
                                            //Junio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes06_Extr.EditValue = 0;
                                            //Julio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes07_Extr.EditValue = 0;
                                            //Agosto
                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes08_Extr.EditValue = 0;
                                            //Septiembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes09_Extr.EditValue = 0;
                                            //Octubre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes10_Extr.EditValue = 0;
                                            //Noviembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes11_Extr.EditValue = 0;
                                            //Diciembre
                                            this.txt_Mes12_Extr.EditValue = vlrMesExtr;

                                        }
                                        else
                                        {
                                            this.txt_Mes01_Extr.EditValue = this.detListOrigen[index].ValorExt01.Value;
                                            this.txt_Mes02_Extr.EditValue = this.detListOrigen[index].ValorExt02.Value;
                                            this.txt_Mes03_Extr.EditValue = this.detListOrigen[index].ValorExt03.Value;
                                            this.txt_Mes04_Extr.EditValue = this.detListOrigen[index].ValorExt04.Value;
                                            this.txt_Mes05_Extr.EditValue = this.detListOrigen[index].ValorExt05.Value;
                                            this.txt_Mes06_Extr.EditValue = this.detListOrigen[index].ValorExt06.Value;
                                            this.txt_Mes07_Extr.EditValue = this.detListOrigen[index].ValorExt07.Value;
                                            this.txt_Mes08_Extr.EditValue = this.detListOrigen[index].ValorExt08.Value;
                                            this.txt_Mes09_Extr.EditValue = this.detListOrigen[index].ValorExt09.Value;
                                            this.txt_Mes10_Extr.EditValue = this.detListOrigen[index].ValorExt10.Value;
                                            this.txt_Mes11_Extr.EditValue = this.detListOrigen[index].ValorExt11.Value;
                                            this.txt_Mes12_Extr.EditValue = this.detListOrigen[index].ValorExt12.Value;
                                        }
                                        #endregion
                                        break;
                                }

                                #endregion
                            }
                            #region Carga la info en el DTO
                            this.detListOrigen[index].ValorLoc00.Value = 0;
                            this.detListOrigen[index].ValorLoc01.Value = Convert.ToDecimal(this.txt_Mes01_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc02.Value = Convert.ToDecimal(this.txt_Mes02_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc03.Value = Convert.ToDecimal(this.txt_Mes03_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc04.Value = Convert.ToDecimal(this.txt_Mes04_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc05.Value = Convert.ToDecimal(this.txt_Mes05_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc06.Value = Convert.ToDecimal(this.txt_Mes06_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc07.Value = Convert.ToDecimal(this.txt_Mes07_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc08.Value = Convert.ToDecimal(this.txt_Mes08_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc09.Value = Convert.ToDecimal(this.txt_Mes09_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc10.Value = Convert.ToDecimal(this.txt_Mes10_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc11.Value = Convert.ToDecimal(this.txt_Mes11_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorLoc12.Value = Convert.ToDecimal(this.txt_Mes12_Local.EditValue, CultureInfo.InvariantCulture);

                            this.detListOrigen[index].ValorExt00.Value = 0;
                            this.detListOrigen[index].ValorExt01.Value = Convert.ToDecimal(this.txt_Mes01_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt02.Value = Convert.ToDecimal(this.txt_Mes02_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt03.Value = Convert.ToDecimal(this.txt_Mes03_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt04.Value = Convert.ToDecimal(this.txt_Mes04_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt05.Value = Convert.ToDecimal(this.txt_Mes05_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt06.Value = Convert.ToDecimal(this.txt_Mes06_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt07.Value = Convert.ToDecimal(this.txt_Mes07_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt08.Value = Convert.ToDecimal(this.txt_Mes08_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt09.Value = Convert.ToDecimal(this.txt_Mes09_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt10.Value = Convert.ToDecimal(this.txt_Mes10_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt11.Value = Convert.ToDecimal(this.txt_Mes11_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListOrigen[index].ValorExt12.Value = Convert.ToDecimal(this.txt_Mes12_Extr.EditValue, CultureInfo.InvariantCulture);

                            if (this.loadME && tc != 0)
                            {
                                this.detListOrigen[index].EquivExt00.Value = 0;
                                this.detListOrigen[index].EquivExt01.Value = this.detListOrigen[index].ValorLoc01.Value / tc;
                                this.detListOrigen[index].EquivExt02.Value = this.detListOrigen[index].ValorLoc02.Value / tc;
                                this.detListOrigen[index].EquivExt03.Value = this.detListOrigen[index].ValorLoc03.Value / tc;
                                this.detListOrigen[index].EquivExt04.Value = this.detListOrigen[index].ValorLoc04.Value / tc;
                                this.detListOrigen[index].EquivExt05.Value = this.detListOrigen[index].ValorLoc05.Value / tc;
                                this.detListOrigen[index].EquivExt06.Value = this.detListOrigen[index].ValorLoc06.Value / tc;
                                this.detListOrigen[index].EquivExt07.Value = this.detListOrigen[index].ValorLoc07.Value / tc;
                                this.detListOrigen[index].EquivExt08.Value = this.detListOrigen[index].ValorLoc08.Value / tc;
                                this.detListOrigen[index].EquivExt09.Value = this.detListOrigen[index].ValorLoc09.Value / tc;
                                this.detListOrigen[index].EquivExt10.Value = this.detListOrigen[index].ValorLoc10.Value / tc;
                                this.detListOrigen[index].EquivExt11.Value = this.detListOrigen[index].ValorLoc11.Value / tc;
                                this.detListOrigen[index].EquivExt12.Value = this.detListOrigen[index].ValorLoc12.Value / tc;

                                this.detListOrigen[index].EquivLoc00.Value = 0;
                                this.detListOrigen[index].EquivLoc01.Value = this.detListOrigen[index].ValorExt01.Value * tc;
                                this.detListOrigen[index].EquivLoc02.Value = this.detListOrigen[index].ValorExt02.Value * tc;
                                this.detListOrigen[index].EquivLoc03.Value = this.detListOrigen[index].ValorExt03.Value * tc;
                                this.detListOrigen[index].EquivLoc04.Value = this.detListOrigen[index].ValorExt04.Value * tc;
                                this.detListOrigen[index].EquivLoc05.Value = this.detListOrigen[index].ValorExt05.Value * tc;
                                this.detListOrigen[index].EquivLoc06.Value = this.detListOrigen[index].ValorExt06.Value * tc;
                                this.detListOrigen[index].EquivLoc07.Value = this.detListOrigen[index].ValorExt07.Value * tc;
                                this.detListOrigen[index].EquivLoc08.Value = this.detListOrigen[index].ValorExt08.Value * tc;
                                this.detListOrigen[index].EquivLoc09.Value = this.detListOrigen[index].ValorExt09.Value * tc;
                                this.detListOrigen[index].EquivLoc10.Value = this.detListOrigen[index].ValorExt10.Value * tc;
                                this.detListOrigen[index].EquivLoc11.Value = this.detListOrigen[index].ValorExt11.Value * tc;
                                this.detListOrigen[index].EquivLoc12.Value = this.detListOrigen[index].ValorExt12.Value * tc;
                            }

                            #endregion
                        }
                }
                else
                {
                    decimal tc = Convert.ToDecimal(this.txtTasaCambioDestino.EditValue, CultureInfo.InvariantCulture);
                    if (this.detListDestino.Count > 0)
                        if (isDoc)
                        {
                            #region Calculo de totales
                            this.txt_Mes01_Local.EditValue = this.detListDestino[index].ValorLoc01.Value;
                            this.txt_Mes02_Local.EditValue = this.detListDestino[index].ValorLoc02.Value;
                            this.txt_Mes03_Local.EditValue = this.detListDestino[index].ValorLoc03.Value;
                            this.txt_Mes04_Local.EditValue = this.detListDestino[index].ValorLoc04.Value;
                            this.txt_Mes05_Local.EditValue = this.detListDestino[index].ValorLoc05.Value;
                            this.txt_Mes06_Local.EditValue = this.detListDestino[index].ValorLoc06.Value;
                            this.txt_Mes07_Local.EditValue = this.detListDestino[index].ValorLoc07.Value;
                            this.txt_Mes08_Local.EditValue = this.detListDestino[index].ValorLoc08.Value;
                            this.txt_Mes09_Local.EditValue = this.detListDestino[index].ValorLoc09.Value;
                            this.txt_Mes10_Local.EditValue = this.detListDestino[index].ValorLoc10.Value;
                            this.txt_Mes11_Local.EditValue = this.detListDestino[index].ValorLoc11.Value;
                            this.txt_Mes12_Local.EditValue = this.detListDestino[index].ValorLoc12.Value;

                            this.txt_Mes01_Extr.EditValue = this.detListDestino[index].ValorExt01.Value;
                            this.txt_Mes02_Extr.EditValue = this.detListDestino[index].ValorExt02.Value;
                            this.txt_Mes03_Extr.EditValue = this.detListDestino[index].ValorExt03.Value;
                            this.txt_Mes04_Extr.EditValue = this.detListDestino[index].ValorExt04.Value;
                            this.txt_Mes05_Extr.EditValue = this.detListDestino[index].ValorExt05.Value;
                            this.txt_Mes06_Extr.EditValue = this.detListDestino[index].ValorExt06.Value;
                            this.txt_Mes07_Extr.EditValue = this.detListDestino[index].ValorExt07.Value;
                            this.txt_Mes08_Extr.EditValue = this.detListDestino[index].ValorExt08.Value;
                            this.txt_Mes09_Extr.EditValue = this.detListDestino[index].ValorExt09.Value;
                            this.txt_Mes10_Extr.EditValue = this.detListDestino[index].ValorExt10.Value;
                            this.txt_Mes11_Extr.EditValue = this.detListDestino[index].ValorExt11.Value;
                            this.txt_Mes12_Extr.EditValue = this.detListDestino[index].ValorExt12.Value;
                            #endregion
                        }
                        else
                        {
                            if (this.actividadLineaPresupInd)
                            {
                                //Obtiene el Control Costo a traves de la LineaPresupuesto y la actividad
                                Dictionary<string, string> pksActLinea = new Dictionary<string, string>();
                                pksActLinea.Add("LineaPresupuestoID", this.detListDestino[index].LineaPresupuestoID.Value);
                                pksActLinea.Add("ActividadID", this.detListDestino[index].ActividadID.Value);
                                DTO_plActividadLineaPresupuestal actividadLinea = (DTO_plActividadLineaPresupuestal)_bc.GetMasterComplexDTO(AppMasters.plActividadLineaPresupuestal, pksActLinea, true);
                                if (actividadLinea != null)// && !linePres.PresMensualVariableInd.Value.Value)
                                {
                                    if (actividadLinea.ControlCosto.Value == (byte)ControlCosto.Variable || actividadLinea.ControlCosto.Value == (byte)ControlCosto.Estacionario)
                                    {
                                        //Distribucion Variable Mensual
                                        #region Asigna particion variable segun Tipo moneda
                                        DTO_coProyecto proy = (DTO_coProyecto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, false, this.masterProyectoOrigen.Value, true);
                                        DTO_glLocFisica locFisica = (DTO_glLocFisica)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLocFisica, false, proy.LocFisicaID.Value, true);

                                        if (locFisica != null)
                                        {
                                            Dictionary<string, string> pks = new Dictionary<string, string>();
                                            pks.Add("PeriodoID", this.dtPeriodOrigen.DateTime.ToShortDateString());
                                            pks.Add("AreaFisicaID", locFisica.AreaFisica.Value);
                                            pks.Add("TipoCosteo", actividadLinea.ControlCosto.Value.ToString());
                                            DTO_plDistribucionCampo distribxMes = (DTO_plDistribucionCampo)_bc.GetMasterComplexDTO(AppMasters.plDistribucionCampo, pks, true);
                                            if (distribxMes != null)
                                            {
                                                #region Calcula % de Ajuste para que la distribucion siempre sea del 100% sin importar el periodo actual
                                                int periodo = this.dtPeriodOrigen.DateTime.Month;
                                                decimal porcentajeAjuste = 0;
                                                switch (periodo)
                                                {
                                                    case 1: //Enero
                                                        porcentajeAjuste = 100 / 100;
                                                        break;
                                                    case 2: //Febrero
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje02.Value.Value + distribxMes.Porcentaje03.Value.Value +
                                                                                distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                                distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 3: //Marzo
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje03.Value.Value + distribxMes.Porcentaje04.Value.Value +
                                                                                distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                                distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 4: //Abril
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje04.Value.Value + distribxMes.Porcentaje05.Value.Value +
                                                                                distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 5: //Mayo
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje05.Value.Value + distribxMes.Porcentaje06.Value.Value +
                                                                                distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 6: //Junio
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje06.Value.Value + distribxMes.Porcentaje07.Value.Value +
                                                                                distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 7: //Julio
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje07.Value.Value + distribxMes.Porcentaje08.Value.Value +
                                                                                distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 8: //Agosto
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje08.Value.Value + distribxMes.Porcentaje09.Value.Value +
                                                                                distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 9: //Septiembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje09.Value.Value + distribxMes.Porcentaje10.Value.Value +
                                                                                distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 10: //Octubre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje10.Value.Value + distribxMes.Porcentaje11.Value.Value +
                                                                                distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 11: //Noviembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje11.Value.Value + distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                    case 12: //Diciembre
                                                        porcentajeAjuste = 100 / (distribxMes.Porcentaje12.Value.Value);
                                                        break;
                                                }
                                                #endregion
                                                switch (loadByMoneda)
                                                {
                                                    case TipoMoneda.Local:
                                                        #region Particion Mda Local
                                                        //Enero
                                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                            this.txt_Mes01_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Local.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                            this.txt_Mes02_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Local.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                            this.txt_Mes03_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Local.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                            this.txt_Mes04_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Local.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                            this.txt_Mes05_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Local.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                            this.txt_Mes06_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Local.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                            this.txt_Mes07_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Local.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                            this.txt_Mes08_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Local.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                            this.txt_Mes09_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Local.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                            this.txt_Mes10_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Local.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                            this.txt_Mes11_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Local.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                        #endregion
                                                        break;
                                                    case TipoMoneda.Foreign:
                                                        #region Particion Mda Extr
                                                        //Enero
                                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                            this.txt_Mes01_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes01_Extr.EditValue = "0";
                                                        //Febrero
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                            this.txt_Mes02_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes02_Extr.EditValue = "0";
                                                        //Marzo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                            this.txt_Mes03_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes03_Extr.EditValue = "0";
                                                        //Abril
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                            this.txt_Mes04_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes04_Extr.EditValue = "0";
                                                        //Mayo
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                            this.txt_Mes05_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes05_Extr.EditValue = "0";
                                                        //Junio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                            this.txt_Mes06_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes06_Extr.EditValue = "0";
                                                        //Julio
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                            this.txt_Mes07_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes07_Extr.EditValue = "0";
                                                        //Agosto
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                            this.txt_Mes08_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes08_Extr.EditValue = "0";
                                                        //Septiembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                            this.txt_Mes09_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes09_Extr.EditValue = "0";
                                                        //Octubre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                            this.txt_Mes10_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes10_Extr.EditValue = "0";
                                                        //Noviembre
                                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                            this.txt_Mes11_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                        else
                                                            this.txt_Mes11_Extr.EditValue = "0";
                                                        //Diciembre
                                                        this.txt_Mes12_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);
                                                        #endregion
                                                        break;
                                                    case TipoMoneda.Both:
                                                        #region Particion Mda Local
                                                        if (this.detListDestino[index].LoadParticionLocalInd)
                                                        {
                                                            //Enero
                                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                                this.txt_Mes01_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes01_Local.EditValue = "0";
                                                            //Febrero
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                                this.txt_Mes02_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes02_Local.EditValue = "0";
                                                            //Marzo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                                this.txt_Mes03_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes03_Local.EditValue = "0";
                                                            //Abril
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                                this.txt_Mes04_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes04_Local.EditValue = "0";
                                                            //Mayo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                                this.txt_Mes05_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes05_Local.EditValue = "0";
                                                            //Junio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                                this.txt_Mes06_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes06_Local.EditValue = "0";
                                                            //Julio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                                this.txt_Mes07_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes07_Local.EditValue = "0";
                                                            //Agosto
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                                this.txt_Mes08_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes08_Local.EditValue = "0";
                                                            //Septiembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                                this.txt_Mes09_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes09_Local.EditValue = "0";
                                                            //Octubre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                                this.txt_Mes10_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes10_Local.EditValue = "0";
                                                            //Noviembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                                this.txt_Mes11_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes11_Local.EditValue = "0";
                                                            //Diciembre
                                                            this.txt_Mes12_Local.EditValue = Math.Round((this.detListDestino[index].VlrMvtoLocal.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                        }
                                                        else
                                                        {
                                                            this.txt_Mes01_Local.EditValue = this.detListDestino[index].ValorLoc01.Value;
                                                            this.txt_Mes02_Local.EditValue = this.detListDestino[index].ValorLoc02.Value;
                                                            this.txt_Mes03_Local.EditValue = this.detListDestino[index].ValorLoc03.Value;
                                                            this.txt_Mes04_Local.EditValue = this.detListDestino[index].ValorLoc04.Value;
                                                            this.txt_Mes05_Local.EditValue = this.detListDestino[index].ValorLoc05.Value;
                                                            this.txt_Mes06_Local.EditValue = this.detListDestino[index].ValorLoc06.Value;
                                                            this.txt_Mes07_Local.EditValue = this.detListDestino[index].ValorLoc07.Value;
                                                            this.txt_Mes08_Local.EditValue = this.detListDestino[index].ValorLoc08.Value;
                                                            this.txt_Mes09_Local.EditValue = this.detListDestino[index].ValorLoc09.Value;
                                                            this.txt_Mes10_Local.EditValue = this.detListDestino[index].ValorLoc10.Value;
                                                            this.txt_Mes11_Local.EditValue = this.detListDestino[index].ValorLoc11.Value;
                                                            this.txt_Mes12_Local.EditValue = this.detListDestino[index].ValorLoc12.Value;
                                                        }
                                                        #endregion
                                                        #region Particion Mda Extr
                                                        if (this.detListDestino[index].LoadParticionExtrInd)
                                                        {
                                                            //Enero
                                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                                this.txt_Mes01_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje01.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes01_Extr.EditValue = "0";
                                                            //Febrero
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                                this.txt_Mes02_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje02.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes02_Extr.EditValue = "0";
                                                            //Marzo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                                this.txt_Mes03_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje03.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes03_Extr.EditValue = "0";
                                                            //Abril
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                                this.txt_Mes04_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje04.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes04_Extr.EditValue = "0";
                                                            //Mayo
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                                this.txt_Mes05_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje05.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes05_Extr.EditValue = "0";
                                                            //Junio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                                this.txt_Mes06_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje06.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes06_Extr.EditValue = "0";
                                                            //Julio
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                                this.txt_Mes07_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje07.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes07_Extr.EditValue = "0";
                                                            //Agosto
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                                this.txt_Mes08_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje08.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes08_Extr.EditValue = "0";
                                                            //Septiembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                                this.txt_Mes09_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje09.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes09_Extr.EditValue = "0";
                                                            //Octubre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                                this.txt_Mes10_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje10.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes10_Extr.EditValue = "0";
                                                            //Noviembre
                                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                                this.txt_Mes11_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje11.Value.Value / 100 * porcentajeAjuste), 2);
                                                            else
                                                                this.txt_Mes11_Extr.EditValue = "0";
                                                            //Diciembre
                                                            this.txt_Mes12_Extr.EditValue = Math.Round((this.detListDestino[index].VlrMvtoExtr.Value.Value * distribxMes.Porcentaje12.Value.Value / 100 * porcentajeAjuste), 2);

                                                        }
                                                        else
                                                        {
                                                            this.txt_Mes01_Extr.EditValue = this.detListDestino[index].ValorExt01.Value;
                                                            this.txt_Mes02_Extr.EditValue = this.detListDestino[index].ValorExt02.Value;
                                                            this.txt_Mes03_Extr.EditValue = this.detListDestino[index].ValorExt03.Value;
                                                            this.txt_Mes04_Extr.EditValue = this.detListDestino[index].ValorExt04.Value;
                                                            this.txt_Mes05_Extr.EditValue = this.detListDestino[index].ValorExt05.Value;
                                                            this.txt_Mes06_Extr.EditValue = this.detListDestino[index].ValorExt06.Value;
                                                            this.txt_Mes07_Extr.EditValue = this.detListDestino[index].ValorExt07.Value;
                                                            this.txt_Mes08_Extr.EditValue = this.detListDestino[index].ValorExt08.Value;
                                                            this.txt_Mes09_Extr.EditValue = this.detListDestino[index].ValorExt09.Value;
                                                            this.txt_Mes10_Extr.EditValue = this.detListDestino[index].ValorExt10.Value;
                                                            this.txt_Mes11_Extr.EditValue = this.detListDestino[index].ValorExt11.Value;
                                                            this.txt_Mes12_Extr.EditValue = this.detListDestino[index].ValorExt12.Value;
                                                        }
                                                        #endregion
                                                        break;
                                                }
                                                onlyFija = false;
                                                validParticion = true;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            if (onlyFija)
                            {
                                //Distribucion Fijo Mensual
                                #region Asigna particion segun el Tipo de Moneda
                                decimal vlrMesExtr = 0;
                                decimal vlrMesLocal = 0;
                                switch (loadByMoneda)
                                {
                                    case TipoMoneda.Local:
                                        #region Particion Mda Local
                                        vlrMesLocal = Math.Round(this.detListDestino[index].VlrMvtoLocal.Value.Value / numMeses, 2);
                                        //Enero
                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                            this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes01_Local.EditValue = 0;
                                        //Febrero
                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                            this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes02_Local.EditValue = 0;
                                        //Marzo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                            this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes03_Local.EditValue = 0;
                                        //Abril
                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                            this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes04_Local.EditValue = 0;
                                        //Mayo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                            this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes05_Local.EditValue = 0;
                                        //Junio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                            this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes06_Local.EditValue = 0;
                                        //Julio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                            this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes07_Local.EditValue = 0;
                                        //Agosto
                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                            this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes08_Local.EditValue = 0;
                                        //Septiembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                            this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes09_Local.EditValue = 0;
                                        //Octubre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                            this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes10_Local.EditValue = 0;
                                        //Noviembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                            this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                        else
                                            this.txt_Mes11_Local.EditValue = 0;
                                        //Diciembre
                                        this.txt_Mes12_Local.EditValue = vlrMesLocal;
                                        #endregion
                                        break;
                                    case TipoMoneda.Foreign:
                                        #region Particion Mda Extr
                                        vlrMesExtr = Math.Round(this.detListDestino[index].VlrMvtoExtr.Value.Value / numMeses, 2);

                                        //Enero
                                        if (this.dtPeriodOrigen.DateTime.Month == 1)
                                            this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes01_Extr.EditValue = 0;
                                        //Febrero
                                        if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                            this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes02_Extr.EditValue = 0;
                                        //Marzo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                            this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes03_Extr.EditValue = 0;
                                        //Abril
                                        if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                            this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes04_Extr.EditValue = 0;
                                        //Mayo
                                        if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                            this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes05_Extr.EditValue = 0;
                                        //Junio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                            this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes06_Extr.EditValue = 0;
                                        //Julio
                                        if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                            this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes07_Extr.EditValue = 0;
                                        //Agosto
                                        if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                            this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes08_Extr.EditValue = 0;
                                        //Septiembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                            this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes09_Extr.EditValue = 0;
                                        //Octubre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                            this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes10_Extr.EditValue = 0;
                                        //Noviembre
                                        if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                            this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                        else
                                            this.txt_Mes11_Extr.EditValue = 0;
                                        //Diciembre
                                        this.txt_Mes12_Extr.EditValue = vlrMesExtr;
                                        #endregion
                                        break;
                                    case TipoMoneda.Both:
                                        #region Particion Mda Local
                                        if (this.detListDestino[index].LoadParticionLocalInd)
                                        {
                                            vlrMesLocal = Math.Round(this.detListDestino[index].VlrMvtoLocal.Value.Value / numMeses, 2);

                                            //Enero
                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                this.txt_Mes01_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes01_Local.EditValue = 0;
                                            //Febrero
                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                this.txt_Mes02_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes02_Local.EditValue = 0;
                                            //Marzo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                this.txt_Mes03_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes03_Local.EditValue = 0;
                                            //Abril
                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                this.txt_Mes04_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes04_Local.EditValue = 0;
                                            //Mayo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                this.txt_Mes05_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes05_Local.EditValue = 0;
                                            //Junio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                this.txt_Mes06_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes06_Local.EditValue = 0;
                                            //Julio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                this.txt_Mes07_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes07_Local.EditValue = 0;
                                            //Agosto
                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                this.txt_Mes08_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes08_Local.EditValue = 0;
                                            //Septiembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                this.txt_Mes09_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes09_Local.EditValue = 0;
                                            //Octubre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                this.txt_Mes10_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes10_Local.EditValue = 0;
                                            //Noviembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                this.txt_Mes11_Local.EditValue = vlrMesLocal;
                                            else
                                                this.txt_Mes11_Local.EditValue = 0;
                                            //Diciembre
                                            this.txt_Mes12_Local.EditValue = vlrMesLocal;

                                        }
                                        else
                                        {
                                            this.txt_Mes01_Local.EditValue = this.detListDestino[index].ValorLoc01.Value;
                                            this.txt_Mes02_Local.EditValue = this.detListDestino[index].ValorLoc02.Value;
                                            this.txt_Mes03_Local.EditValue = this.detListDestino[index].ValorLoc03.Value;
                                            this.txt_Mes04_Local.EditValue = this.detListDestino[index].ValorLoc04.Value;
                                            this.txt_Mes05_Local.EditValue = this.detListDestino[index].ValorLoc05.Value;
                                            this.txt_Mes06_Local.EditValue = this.detListDestino[index].ValorLoc06.Value;
                                            this.txt_Mes07_Local.EditValue = this.detListDestino[index].ValorLoc07.Value;
                                            this.txt_Mes08_Local.EditValue = this.detListDestino[index].ValorLoc08.Value;
                                            this.txt_Mes09_Local.EditValue = this.detListDestino[index].ValorLoc09.Value;
                                            this.txt_Mes10_Local.EditValue = this.detListDestino[index].ValorLoc10.Value;
                                            this.txt_Mes11_Local.EditValue = this.detListDestino[index].ValorLoc11.Value;
                                            this.txt_Mes12_Local.EditValue = this.detListDestino[index].ValorLoc12.Value;
                                        }
                                        #endregion
                                        #region Particion Mda Extr
                                        if (this.detListDestino[index].LoadParticionExtrInd)
                                        {
                                            vlrMesExtr = Math.Round(this.detListDestino[index].VlrMvtoExtr.Value.Value / numMeses, 2);
                                            //Enero
                                            if (this.dtPeriodOrigen.DateTime.Month == 1)
                                                this.txt_Mes01_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes01_Extr.EditValue = 0;
                                            //Febrero
                                            if (this.dtPeriodOrigen.DateTime.Month <= 2)
                                                this.txt_Mes02_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes02_Extr.EditValue = 0;
                                            //Marzo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 3)
                                                this.txt_Mes03_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes03_Extr.EditValue = 0;
                                            //Abril
                                            if (this.dtPeriodOrigen.DateTime.Month <= 4)
                                                this.txt_Mes04_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes04_Extr.EditValue = 0;
                                            //Mayo
                                            if (this.dtPeriodOrigen.DateTime.Month <= 5)
                                                this.txt_Mes05_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes05_Extr.EditValue = 0;
                                            //Junio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 6)
                                                this.txt_Mes06_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes06_Extr.EditValue = 0;
                                            //Julio
                                            if (this.dtPeriodOrigen.DateTime.Month <= 7)
                                                this.txt_Mes07_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes07_Extr.EditValue = 0;
                                            //Agosto
                                            if (this.dtPeriodOrigen.DateTime.Month <= 8)
                                                this.txt_Mes08_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes08_Extr.EditValue = 0;
                                            //Septiembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 9)
                                                this.txt_Mes09_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes09_Extr.EditValue = 0;
                                            //Octubre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 10)
                                                this.txt_Mes10_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes10_Extr.EditValue = 0;
                                            //Noviembre
                                            if (this.dtPeriodOrigen.DateTime.Month <= 11)
                                                this.txt_Mes11_Extr.EditValue = vlrMesExtr;
                                            else
                                                this.txt_Mes11_Extr.EditValue = 0;
                                            //Diciembre
                                            this.txt_Mes12_Extr.EditValue = vlrMesExtr;

                                        }
                                        else
                                        {
                                            this.txt_Mes01_Extr.EditValue = this.detListDestino[index].ValorExt01.Value;
                                            this.txt_Mes02_Extr.EditValue = this.detListDestino[index].ValorExt02.Value;
                                            this.txt_Mes03_Extr.EditValue = this.detListDestino[index].ValorExt03.Value;
                                            this.txt_Mes04_Extr.EditValue = this.detListDestino[index].ValorExt04.Value;
                                            this.txt_Mes05_Extr.EditValue = this.detListDestino[index].ValorExt05.Value;
                                            this.txt_Mes06_Extr.EditValue = this.detListDestino[index].ValorExt06.Value;
                                            this.txt_Mes07_Extr.EditValue = this.detListDestino[index].ValorExt07.Value;
                                            this.txt_Mes08_Extr.EditValue = this.detListDestino[index].ValorExt08.Value;
                                            this.txt_Mes09_Extr.EditValue = this.detListDestino[index].ValorExt09.Value;
                                            this.txt_Mes10_Extr.EditValue = this.detListDestino[index].ValorExt10.Value;
                                            this.txt_Mes11_Extr.EditValue = this.detListDestino[index].ValorExt11.Value;
                                            this.txt_Mes12_Extr.EditValue = this.detListDestino[index].ValorExt12.Value;
                                        }
                                        #endregion
                                        break;
                                }
                                #endregion
                            }
                            #region Carga la info en el DTO
                            this.detListDestino[index].ValorLoc00.Value = 0;
                            this.detListDestino[index].ValorLoc01.Value = Convert.ToDecimal(this.txt_Mes01_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc02.Value = Convert.ToDecimal(this.txt_Mes02_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc03.Value = Convert.ToDecimal(this.txt_Mes03_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc04.Value = Convert.ToDecimal(this.txt_Mes04_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc05.Value = Convert.ToDecimal(this.txt_Mes05_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc06.Value = Convert.ToDecimal(this.txt_Mes06_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc07.Value = Convert.ToDecimal(this.txt_Mes07_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc08.Value = Convert.ToDecimal(this.txt_Mes08_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc09.Value = Convert.ToDecimal(this.txt_Mes09_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc10.Value = Convert.ToDecimal(this.txt_Mes10_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc11.Value = Convert.ToDecimal(this.txt_Mes11_Local.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorLoc12.Value = Convert.ToDecimal(this.txt_Mes12_Local.EditValue, CultureInfo.InvariantCulture);

                            this.detListDestino[index].ValorExt00.Value = 0;
                            this.detListDestino[index].ValorExt01.Value = Convert.ToDecimal(this.txt_Mes01_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt02.Value = Convert.ToDecimal(this.txt_Mes02_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt03.Value = Convert.ToDecimal(this.txt_Mes03_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt04.Value = Convert.ToDecimal(this.txt_Mes04_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt05.Value = Convert.ToDecimal(this.txt_Mes05_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt06.Value = Convert.ToDecimal(this.txt_Mes06_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt07.Value = Convert.ToDecimal(this.txt_Mes07_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt08.Value = Convert.ToDecimal(this.txt_Mes08_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt09.Value = Convert.ToDecimal(this.txt_Mes09_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt10.Value = Convert.ToDecimal(this.txt_Mes10_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt11.Value = Convert.ToDecimal(this.txt_Mes11_Extr.EditValue, CultureInfo.InvariantCulture);
                            this.detListDestino[index].ValorExt12.Value = Convert.ToDecimal(this.txt_Mes12_Extr.EditValue, CultureInfo.InvariantCulture);

                            if (this.loadME && tc != 0)
                            {
                                this.detListDestino[index].EquivExt00.Value = 0;
                                this.detListDestino[index].EquivExt01.Value = this.detListDestino[index].ValorLoc01.Value / tc;
                                this.detListDestino[index].EquivExt02.Value = this.detListDestino[index].ValorLoc02.Value / tc;
                                this.detListDestino[index].EquivExt03.Value = this.detListDestino[index].ValorLoc03.Value / tc;
                                this.detListDestino[index].EquivExt04.Value = this.detListDestino[index].ValorLoc04.Value / tc;
                                this.detListDestino[index].EquivExt05.Value = this.detListDestino[index].ValorLoc05.Value / tc;
                                this.detListDestino[index].EquivExt06.Value = this.detListDestino[index].ValorLoc06.Value / tc;
                                this.detListDestino[index].EquivExt07.Value = this.detListDestino[index].ValorLoc07.Value / tc;
                                this.detListDestino[index].EquivExt08.Value = this.detListDestino[index].ValorLoc08.Value / tc;
                                this.detListDestino[index].EquivExt09.Value = this.detListDestino[index].ValorLoc09.Value / tc;
                                this.detListDestino[index].EquivExt10.Value = this.detListDestino[index].ValorLoc10.Value / tc;
                                this.detListDestino[index].EquivExt11.Value = this.detListDestino[index].ValorLoc11.Value / tc;
                                this.detListDestino[index].EquivExt12.Value = this.detListDestino[index].ValorLoc12.Value / tc;

                                this.detListDestino[index].EquivLoc00.Value = 0;
                                this.detListDestino[index].EquivLoc01.Value = this.detListDestino[index].ValorExt01.Value * tc;
                                this.detListDestino[index].EquivLoc02.Value = this.detListDestino[index].ValorExt02.Value * tc;
                                this.detListDestino[index].EquivLoc03.Value = this.detListDestino[index].ValorExt03.Value * tc;
                                this.detListDestino[index].EquivLoc04.Value = this.detListDestino[index].ValorExt04.Value * tc;
                                this.detListDestino[index].EquivLoc05.Value = this.detListDestino[index].ValorExt05.Value * tc;
                                this.detListDestino[index].EquivLoc06.Value = this.detListDestino[index].ValorExt06.Value * tc;
                                this.detListDestino[index].EquivLoc07.Value = this.detListDestino[index].ValorExt07.Value * tc;
                                this.detListDestino[index].EquivLoc08.Value = this.detListDestino[index].ValorExt08.Value * tc;
                                this.detListDestino[index].EquivLoc09.Value = this.detListDestino[index].ValorExt09.Value * tc;
                                this.detListDestino[index].EquivLoc10.Value = this.detListDestino[index].ValorExt10.Value * tc;
                                this.detListDestino[index].EquivLoc11.Value = this.detListDestino[index].ValorExt11.Value * tc;
                                this.detListDestino[index].EquivLoc12.Value = this.detListDestino[index].ValorExt12.Value * tc;
                            }

                            #endregion
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "LoadParticiones"));
            }
        }

        /// <summary>
        /// Carga la info del footer 
        /// </summary>
        private void LoadFooter(int index)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.detListOrigen.Count > 0 && !this.initData)
                    {
                        DTO_plPresupuestoDeta det = this.detListOrigen[index];

                        this.txt_Mes01_Local.EditValue = det.ValorLoc01.Value.Value;
                        this.txt_Mes02_Local.EditValue = det.ValorLoc02.Value.Value;
                        this.txt_Mes03_Local.EditValue = det.ValorLoc03.Value.Value;
                        this.txt_Mes04_Local.EditValue = det.ValorLoc04.Value.Value;
                        this.txt_Mes05_Local.EditValue = det.ValorLoc05.Value.Value;
                        this.txt_Mes06_Local.EditValue = det.ValorLoc06.Value.Value;
                        this.txt_Mes07_Local.EditValue = det.ValorLoc07.Value.Value;
                        this.txt_Mes08_Local.EditValue = det.ValorLoc08.Value.Value;
                        this.txt_Mes09_Local.EditValue = det.ValorLoc09.Value.Value;
                        this.txt_Mes10_Local.EditValue = det.ValorLoc10.Value.Value;
                        this.txt_Mes11_Local.EditValue = det.ValorLoc11.Value.Value;
                        this.txt_Mes12_Local.EditValue = det.ValorLoc12.Value.Value;

                        this.txt_Mes01_Extr.EditValue = det.ValorExt01.Value.Value;
                        this.txt_Mes02_Extr.EditValue = det.ValorExt02.Value.Value;
                        this.txt_Mes03_Extr.EditValue = det.ValorExt03.Value.Value;
                        this.txt_Mes04_Extr.EditValue = det.ValorExt04.Value.Value;
                        this.txt_Mes05_Extr.EditValue = det.ValorExt05.Value.Value;
                        this.txt_Mes06_Extr.EditValue = det.ValorExt06.Value.Value;
                        this.txt_Mes07_Extr.EditValue = det.ValorExt07.Value.Value;
                        this.txt_Mes08_Extr.EditValue = det.ValorExt08.Value.Value;
                        this.txt_Mes09_Extr.EditValue = det.ValorExt09.Value.Value;
                        this.txt_Mes10_Extr.EditValue = det.ValorExt10.Value.Value;
                        this.txt_Mes11_Extr.EditValue = det.ValorExt11.Value.Value;
                        this.txt_Mes12_Extr.EditValue = det.ValorExt12.Value.Value;
                    }
                    else
                        this.LoadParticiones(this.gvDetailOrigen.FocusedRowHandle, false);
                }
                else
                {
                    if (this.detListDestino.Count > 0 && !this.initData)
                    {
                        DTO_plPresupuestoDeta det = this.detListDestino[index];

                        this.txt_Mes01_Local.EditValue = det.ValorLoc01.Value.Value;
                        this.txt_Mes02_Local.EditValue = det.ValorLoc02.Value.Value;
                        this.txt_Mes03_Local.EditValue = det.ValorLoc03.Value.Value;
                        this.txt_Mes04_Local.EditValue = det.ValorLoc04.Value.Value;
                        this.txt_Mes05_Local.EditValue = det.ValorLoc05.Value.Value;
                        this.txt_Mes06_Local.EditValue = det.ValorLoc06.Value.Value;
                        this.txt_Mes07_Local.EditValue = det.ValorLoc07.Value.Value;
                        this.txt_Mes08_Local.EditValue = det.ValorLoc08.Value.Value;
                        this.txt_Mes09_Local.EditValue = det.ValorLoc09.Value.Value;
                        this.txt_Mes10_Local.EditValue = det.ValorLoc10.Value.Value;
                        this.txt_Mes11_Local.EditValue = det.ValorLoc11.Value.Value;
                        this.txt_Mes12_Local.EditValue = det.ValorLoc12.Value.Value;

                        this.txt_Mes01_Extr.EditValue = det.ValorExt01.Value.Value;
                        this.txt_Mes02_Extr.EditValue = det.ValorExt02.Value.Value;
                        this.txt_Mes03_Extr.EditValue = det.ValorExt03.Value.Value;
                        this.txt_Mes04_Extr.EditValue = det.ValorExt04.Value.Value;
                        this.txt_Mes05_Extr.EditValue = det.ValorExt05.Value.Value;
                        this.txt_Mes06_Extr.EditValue = det.ValorExt06.Value.Value;
                        this.txt_Mes07_Extr.EditValue = det.ValorExt07.Value.Value;
                        this.txt_Mes08_Extr.EditValue = det.ValorExt08.Value.Value;
                        this.txt_Mes09_Extr.EditValue = det.ValorExt09.Value.Value;
                        this.txt_Mes10_Extr.EditValue = det.ValorExt10.Value.Value;
                        this.txt_Mes11_Extr.EditValue = det.ValorExt11.Value.Value;
                        this.txt_Mes12_Extr.EditValue = det.ValorExt12.Value.Value;
                    }
                    else
                        this.LoadParticiones(this.gvDetailDestino.FocusedRowHandle, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "LoadFooter"));
            }
        }

        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            this.IsModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                ModalMaster modal = null;
                if (fktable.Jerarquica.Value.Value)
                {
                    //if (col == "LineaPresupuestoID")
                    //    modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, this.filtrosLineaPres);
                    if (col == "CentroCostoID")
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, this.filtrosCentroCosto);
                    else
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    if (col == "LineaPresupuestoID")
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true, this.filtrosLineaPres);
                    else
                        modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "ShowFKModal"));
            }
            finally
            {
                this.IsModalFormOpened = false;
            }
        }

        /// <summary>
        /// Valida un DTO de footer en la importacion
        /// </summary>
        /// <param name="presupDet">detalle a validar</param>
        /// <param name="rd">Variable que va guardando los mensajes de error</param>
        private void ValidateDataImport(DTO_plPresupuestoDeta presupDet, DTO_TxResultDetail rd)
        {
            string colRsx;
            DTO_TxResultDetailFields rdF;
            bool createDTO = true;

            #region Valida FKs
            #region CentroCostoID
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_CentroCostoID");
            rdF = this._bc.ValidGridCell(colRsx, presupDet.CentroCostoID.Value, false, true, true, AppMasters.coCentroCosto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Linea Presupuesto
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_LineaPresupuestoID");
            rdF = this._bc.ValidGridCell(colRsx, presupDet.LineaPresupuestoID.Value, false, false, false, AppMasters.plLineaPresupuesto);
            if (rdF != null)
            {
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region Validacion de PK compuesta
            if (this.detListOrigen.Count > 0)
            {
                int count = this.detListOrigen.Where(x => x.CentroCostoID.Value == presupDet.CentroCostoID.Value &&
                                                          x.LineaPresupuestoID.Value == presupDet.LineaPresupuestoID.Value &&
                                                          x.ProyectoID.Value == presupDet.ProyectoID.Value).Count();
                if (count > 1)
                {
                    rdF = new DTO_TxResultDetailFields();
                    rdF.Field = colRsx;
                    rdF.Message = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidPresDet);
                    createDTO = false;
                    rd.DetailsFields.Add(rdF);
                }
            }
            #endregion
            #endregion
            #region Valida Valores
            #region VlrMvtoLocal - VlrMvtoExtr
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrMvtoLocal");
            string colRsxExtr = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrMvtoExtr");
            if (presupDet.VlrMvtoLocal.Value == 0 && presupDet.VlrMvtoExtr.Value == 0)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx + "-" + colRsxExtr;
                rdF.Message = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ZeroField), rdF.Field);
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #region VlrNuevoSaldoLocal - VlrNuevoSaldoExtr
            colRsx = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrNuevoSaldoLocal");
            string colRsxExtrSaldo = this._bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_VlrNuevoSaldoExtr");
            if (presupDet.VlrNuevoSaldoLoc.Value < 0 || presupDet.VlrNuevoSaldoExtr.Value < 0)
            {
                rdF = new DTO_TxResultDetailFields();
                rdF.Field = colRsx + "-" + colRsxExtrSaldo;
                rdF.Message = DictionaryMessages.pl_newValueInvalid;
                createDTO = false;
                rd.DetailsFields.Add(rdF);
            }
            #endregion
            #endregion
            if (createDTO)
            {
                DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, presupDet.LineaPresupuestoID.Value, true);
                presupDet.LineaPresDesc.Value = lineaPres.Descriptivo.Value;
                DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, presupDet.CentroCostoID.Value, true);
                presupDet.CentroCostoDesc.Value = centroCto.Descriptivo.Value;
            }


        }

        /// <summary>
        /// Agrega las columnas a la subgrilla
        /// </summary>
        private void AddColsDetalle()
        {
            try
            {
                //CentroCostoID
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 0;
                centroCostoID.Width = 40;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = this.documentID == AppDocuments.Presupuesto ? true : false;
                centroCostoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetailOrigen.Columns.Add(centroCostoID);
                //this.gvDetailDestino.Columns.Add(centroCostoID);

                //centroCostoDesc
                GridColumn centroCostoDesc = new GridColumn();
                centroCostoDesc.FieldName = this.unboundPrefix + "CentroCostoDesc";
                centroCostoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_CentroCostoDesc");
                centroCostoDesc.UnboundType = UnboundColumnType.String;
                centroCostoDesc.VisibleIndex = 1;
                centroCostoDesc.Width = 60;
                centroCostoDesc.Visible = true;
                centroCostoDesc.OptionsColumn.AllowFocus = false;
                centroCostoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetailOrigen.Columns.Add(centroCostoDesc);
                //this.gvDetailDestino.Columns.Add(centroCostoDesc);

                //LineaPresupuestoID
                GridColumn lineaPresupuestoID = new GridColumn();
                lineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                lineaPresupuestoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_LineaPresupuestoID"); ;
                lineaPresupuestoID.UnboundType = UnboundColumnType.String;
                lineaPresupuestoID.VisibleIndex = 2;
                lineaPresupuestoID.Width = 40;
                lineaPresupuestoID.Visible = true;
                lineaPresupuestoID.OptionsColumn.AllowEdit = this.documentID == AppDocuments.Presupuesto ? true : false;
                lineaPresupuestoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetailOrigen.Columns.Add(lineaPresupuestoID);
                //this.gvDetailDestino.Columns.Add(lineaPresupuestoID);

                //LineaPresupuestoDesc
                GridColumn LineaPresupuestoDesc = new GridColumn();
                LineaPresupuestoDesc.FieldName = this.unboundPrefix + "LineaPresDesc";
                LineaPresupuestoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_LineaPresDesc");
                LineaPresupuestoDesc.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDesc.VisibleIndex = 3;
                LineaPresupuestoDesc.Width = 60;
                LineaPresupuestoDesc.Visible = true;
                LineaPresupuestoDesc.OptionsColumn.AllowFocus = false;
                LineaPresupuestoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetailOrigen.Columns.Add(LineaPresupuestoDesc);
                //this.gvDetailDestino.Columns.Add(LineaPresupuestoDesc);

                //Saldo Anterior Loc
                GridColumn saldoAntLoc = new GridColumn();
                saldoAntLoc.FieldName = this.unboundPrefix + "VlrSaldoAntLoc";
                saldoAntLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_SaldoAnteriorLoc");
                saldoAntLoc.UnboundType = UnboundColumnType.Decimal;
                saldoAntLoc.VisibleIndex = 4;
                saldoAntLoc.Width = 40;
                saldoAntLoc.Visible = this.documentID != AppDocuments.Presupuesto ? true : false;
                saldoAntLoc.OptionsColumn.AllowEdit = false;
                saldoAntLoc.OptionsColumn.AllowFocus = false;
                saldoAntLoc.ColumnEdit = this.editValor;
                this.gvDetailOrigen.Columns.Add(saldoAntLoc);
                //this.gvDetailDestino.Columns.Add(saldoAntLoc);

                //Movimiento Loc
                GridColumn mvtoLoc = new GridColumn();
                mvtoLoc.FieldName = this.unboundPrefix + "VlrMvtoLocal";
                mvtoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_MovimientoLoc");
                mvtoLoc.UnboundType = UnboundColumnType.Decimal;
                mvtoLoc.VisibleIndex = 5;
                mvtoLoc.Width = 40;
                mvtoLoc.Visible = true;
                mvtoLoc.OptionsColumn.AllowEdit = true;
                mvtoLoc.ColumnEdit = this.editValor;
                mvtoLoc.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                mvtoLoc.AppearanceCell.Options.UseTextOptions = true;
                mvtoLoc.AppearanceCell.Options.UseFont = true;
                this.gvDetailOrigen.Columns.Add(mvtoLoc);
                //this.gvDetailDestino.Columns.Add(mvtoLoc);

                //NuevoSaldo Loc
                GridColumn nuevoSaldoLoc = new GridColumn();
                nuevoSaldoLoc.FieldName = this.unboundPrefix + "VlrNuevoSaldoLoc";
                nuevoSaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_NuevoSaldoLoc");
                nuevoSaldoLoc.UnboundType = UnboundColumnType.Decimal;
                nuevoSaldoLoc.VisibleIndex = 6;
                nuevoSaldoLoc.Width = 40;
                nuevoSaldoLoc.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                nuevoSaldoLoc.OptionsColumn.AllowEdit = false;
                nuevoSaldoLoc.OptionsColumn.AllowFocus = false;
                nuevoSaldoLoc.ColumnEdit = this.editValor;
                this.gvDetailOrigen.Columns.Add(nuevoSaldoLoc);
                //this.gvDetailDestino.Columns.Add(nuevoSaldoLoc);

                if (this.loadME)
                {
                    //Saldo Anterior Extr
                    GridColumn saldoAntExtr = new GridColumn();
                    saldoAntExtr.FieldName = this.unboundPrefix + "VlrSaldoAntExtr";
                    saldoAntExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_SaldoAnteriorExtr");
                    saldoAntExtr.UnboundType = UnboundColumnType.Decimal;
                    saldoAntExtr.VisibleIndex = 7;
                    saldoAntExtr.Width = 40;
                    saldoAntExtr.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                    saldoAntExtr.OptionsColumn.AllowEdit = false;
                    saldoAntExtr.OptionsColumn.AllowFocus = false;
                    saldoAntExtr.ColumnEdit = this.editValor;
                    saldoAntExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    saldoAntExtr.AppearanceHeader.Options.UseTextOptions = true;
                    saldoAntExtr.AppearanceHeader.Options.UseFont = true;
                    saldoAntExtr.AppearanceHeader.Options.UseForeColor = true;
                    saldoAntExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    saldoAntExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetailOrigen.Columns.Add(saldoAntExtr);
                    //this.gvDetailDestino.Columns.Add(saldoAntExtr);

                    //Movimiento Extr
                    GridColumn mvtoExtr = new GridColumn();
                    mvtoExtr.FieldName = this.unboundPrefix + "VlrMvtoExtr";
                    mvtoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_MovimientoExtr");
                    mvtoExtr.UnboundType = UnboundColumnType.Decimal;
                    mvtoExtr.VisibleIndex = 8;
                    mvtoExtr.Width = 40;
                    mvtoExtr.Visible = true;
                    mvtoExtr.OptionsColumn.AllowEdit = true;
                    mvtoExtr.ColumnEdit = this.editValor;
                    mvtoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    mvtoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceHeader.Options.UseFont = true;
                    mvtoExtr.AppearanceHeader.Options.UseForeColor = true;
                    mvtoExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    mvtoExtr.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceCell.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceCell.Options.UseFont = true;
                    this.gvDetailOrigen.Columns.Add(mvtoExtr);
                    //this.gvDetailDestino.Columns.Add(mvtoExtr);

                    //NuevoSaldo Extr
                    GridColumn nuevoSaldoExtr = new GridColumn();
                    nuevoSaldoExtr.FieldName = this.unboundPrefix + "VlrNuevoSaldoExtr";
                    nuevoSaldoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_NuevoSaldoExtr");
                    nuevoSaldoExtr.UnboundType = UnboundColumnType.Decimal;
                    nuevoSaldoExtr.VisibleIndex = 9;
                    nuevoSaldoExtr.Width = 40;
                    nuevoSaldoExtr.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                    nuevoSaldoExtr.OptionsColumn.AllowEdit = false;
                    nuevoSaldoExtr.OptionsColumn.AllowFocus = false;
                    nuevoSaldoExtr.ColumnEdit = this.editValor;
                    nuevoSaldoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseFont = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseForeColor = true;
                    nuevoSaldoExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    nuevoSaldoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetailOrigen.Columns.Add(nuevoSaldoExtr);
                    //this.gvDetailDestino.Columns.Add(nuevoSaldoExtr);
                }

                //Observacion Deta
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "DescripTExt";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_DescripTExt");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 10;
                DescripTExt.Width = 60;
                DescripTExt.Visible = true;
                DescripTExt.ColumnEdit = this.richText1;
                this.gvDetailOrigen.Columns.Add(DescripTExt);
                //this.gvDetailDestino.Columns.Add(DescripTExt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-AddCols_Det"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la subgrilla
        /// </summary>
        private void AddColsDetalleDestino()
        {
            try
            {
                //CentroCostoID
                GridColumn centroCostoID = new GridColumn();
                centroCostoID.FieldName = this.unboundPrefix + "CentroCostoID";
                centroCostoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_CentroCostoID");
                centroCostoID.UnboundType = UnboundColumnType.String;
                centroCostoID.VisibleIndex = 0;
                centroCostoID.Width = 40;
                centroCostoID.Visible = true;
                centroCostoID.OptionsColumn.AllowEdit = this.documentID == AppDocuments.Presupuesto ? true : false;
                centroCostoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetailDestino.Columns.Add(centroCostoID);

                //centroCostoDesc
                GridColumn centroCostoDesc = new GridColumn();
                centroCostoDesc.FieldName = this.unboundPrefix + "CentroCostoDesc";
                centroCostoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_CentroCostoDesc");
                centroCostoDesc.UnboundType = UnboundColumnType.String;
                centroCostoDesc.VisibleIndex = 1;
                centroCostoDesc.Width = 60;
                centroCostoDesc.Visible = true;
                centroCostoDesc.OptionsColumn.AllowFocus = false;
                centroCostoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetailDestino.Columns.Add(centroCostoDesc);

                //LineaPresupuestoID
                GridColumn lineaPresupuestoID = new GridColumn();
                lineaPresupuestoID.FieldName = this.unboundPrefix + "LineaPresupuestoID";
                lineaPresupuestoID.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_LineaPresupuestoID"); ;
                lineaPresupuestoID.UnboundType = UnboundColumnType.String;
                lineaPresupuestoID.VisibleIndex = 2;
                lineaPresupuestoID.Width = 40;
                lineaPresupuestoID.Visible = true;
                lineaPresupuestoID.OptionsColumn.AllowEdit = this.documentID == AppDocuments.Presupuesto ? true : false;
                lineaPresupuestoID.ColumnEdit = this.editBtn_Doc;
                this.gvDetailDestino.Columns.Add(lineaPresupuestoID);

                //LineaPresupuestoDesc
                GridColumn LineaPresupuestoDesc = new GridColumn();
                LineaPresupuestoDesc.FieldName = this.unboundPrefix + "LineaPresDesc";
                LineaPresupuestoDesc.Caption = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_LineaPresDesc");
                LineaPresupuestoDesc.UnboundType = UnboundColumnType.String;
                LineaPresupuestoDesc.VisibleIndex = 3;
                LineaPresupuestoDesc.Width = 60;
                LineaPresupuestoDesc.Visible = true;
                LineaPresupuestoDesc.OptionsColumn.AllowFocus = false;
                LineaPresupuestoDesc.OptionsColumn.AllowEdit = false;
                this.gvDetailDestino.Columns.Add(LineaPresupuestoDesc);

                //Saldo Anterior Loc
                GridColumn saldoAntLoc = new GridColumn();
                saldoAntLoc.FieldName = this.unboundPrefix + "VlrSaldoAntLoc";
                saldoAntLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_SaldoAnteriorLoc");
                saldoAntLoc.UnboundType = UnboundColumnType.Decimal;
                saldoAntLoc.VisibleIndex = 4;
                saldoAntLoc.Width = 40;
                saldoAntLoc.Visible = this.documentID != AppDocuments.Presupuesto ? true : false;
                saldoAntLoc.OptionsColumn.AllowEdit = false;
                saldoAntLoc.OptionsColumn.AllowFocus = false;
                saldoAntLoc.ColumnEdit = this.editValor;
                this.gvDetailDestino.Columns.Add(saldoAntLoc);

                //Movimiento Loc
                GridColumn mvtoLoc = new GridColumn();
                mvtoLoc.FieldName = this.unboundPrefix + "VlrMvtoLocal";
                mvtoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_MovimientoLoc");
                mvtoLoc.UnboundType = UnboundColumnType.Decimal;
                mvtoLoc.VisibleIndex = 5;
                mvtoLoc.Width = 40;
                mvtoLoc.Visible = true;
                mvtoLoc.OptionsColumn.AllowEdit = true;
                mvtoLoc.ColumnEdit = this.editValor;
                mvtoLoc.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                mvtoLoc.AppearanceCell.Options.UseTextOptions = true;
                mvtoLoc.AppearanceCell.Options.UseFont = true;
                this.gvDetailDestino.Columns.Add(mvtoLoc);

                //NuevoSaldo Loc
                GridColumn nuevoSaldoLoc = new GridColumn();
                nuevoSaldoLoc.FieldName = this.unboundPrefix + "VlrNuevoSaldoLoc";
                nuevoSaldoLoc.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_NuevoSaldoLoc");
                nuevoSaldoLoc.UnboundType = UnboundColumnType.Decimal;
                nuevoSaldoLoc.VisibleIndex = 6;
                nuevoSaldoLoc.Width = 40;
                nuevoSaldoLoc.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                nuevoSaldoLoc.OptionsColumn.AllowEdit = false;
                nuevoSaldoLoc.OptionsColumn.AllowFocus = false;
                nuevoSaldoLoc.ColumnEdit = this.editValor;
                this.gvDetailDestino.Columns.Add(nuevoSaldoLoc);

                if (this.loadME)
                {
                    //Saldo Anterior Extr
                    GridColumn saldoAntExtr = new GridColumn();
                    saldoAntExtr.FieldName = this.unboundPrefix + "VlrSaldoAntExtr";
                    saldoAntExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_SaldoAnteriorExtr");
                    saldoAntExtr.UnboundType = UnboundColumnType.Decimal;
                    saldoAntExtr.VisibleIndex = 7;
                    saldoAntExtr.Width = 40;
                    saldoAntExtr.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                    saldoAntExtr.OptionsColumn.AllowEdit = false;
                    saldoAntExtr.OptionsColumn.AllowFocus = false;
                    saldoAntExtr.ColumnEdit = this.editValor;
                    saldoAntExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    saldoAntExtr.AppearanceHeader.Options.UseTextOptions = true;
                    saldoAntExtr.AppearanceHeader.Options.UseFont = true;
                    saldoAntExtr.AppearanceHeader.Options.UseForeColor = true;
                    saldoAntExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    saldoAntExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetailDestino.Columns.Add(saldoAntExtr);

                    //Movimiento Extr
                    GridColumn mvtoExtr = new GridColumn();
                    mvtoExtr.FieldName = this.unboundPrefix + "VlrMvtoExtr";
                    mvtoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_MovimientoExtr");
                    mvtoExtr.UnboundType = UnboundColumnType.Decimal;
                    mvtoExtr.VisibleIndex = 8;
                    mvtoExtr.Width = 40;
                    mvtoExtr.Visible = true;
                    mvtoExtr.OptionsColumn.AllowEdit = true;
                    mvtoExtr.ColumnEdit = this.editValor;
                    mvtoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    mvtoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceHeader.Options.UseFont = true;
                    mvtoExtr.AppearanceHeader.Options.UseForeColor = true;
                    mvtoExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    mvtoExtr.AppearanceCell.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    mvtoExtr.AppearanceCell.Options.UseTextOptions = true;
                    mvtoExtr.AppearanceCell.Options.UseFont = true;
                    this.gvDetailDestino.Columns.Add(mvtoExtr);

                    //NuevoSaldo Extr
                    GridColumn nuevoSaldoExtr = new GridColumn();
                    nuevoSaldoExtr.FieldName = this.unboundPrefix + "VlrNuevoSaldoExtr";
                    nuevoSaldoExtr.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_NuevoSaldoExtr");
                    nuevoSaldoExtr.UnboundType = UnboundColumnType.Decimal;
                    nuevoSaldoExtr.VisibleIndex = 9;
                    nuevoSaldoExtr.Width = 40;
                    nuevoSaldoExtr.Visible = this.documentID != AppDocuments.Presupuesto ? true : false; ;
                    nuevoSaldoExtr.OptionsColumn.AllowEdit = false;
                    nuevoSaldoExtr.OptionsColumn.AllowFocus = false;
                    nuevoSaldoExtr.ColumnEdit = this.editValor;
                    nuevoSaldoExtr.AppearanceHeader.ForeColor = Color.LightSteelBlue;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseTextOptions = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseFont = true;
                    nuevoSaldoExtr.AppearanceHeader.Options.UseForeColor = true;
                    nuevoSaldoExtr.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    nuevoSaldoExtr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    this.gvDetailDestino.Columns.Add(nuevoSaldoExtr);
                }

                //Observacion Deta
                GridColumn DescripTExt = new GridColumn();
                DescripTExt.FieldName = this.unboundPrefix + "DescripTExt";
                DescripTExt.Caption = _bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_DescripTExt");
                DescripTExt.UnboundType = UnboundColumnType.String;
                DescripTExt.VisibleIndex = 10;
                DescripTExt.Width = 60;
                DescripTExt.Visible = true;
                DescripTExt.ColumnEdit = this.richText1;
                this.gvDetailDestino.Columns.Add(DescripTExt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-AddCols_Det"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow_Det()
        {
            try
            {
                DTO_plPresupuestoDeta det = new DTO_plPresupuestoDeta(true);

                #region Asigna datos a la fila
                det.Consecutivo.Value = this.detListDestino.Count == 0 ? 1 : this.detListDestino.Last().Consecutivo.Value.Value + 1;
                det.ProyectoID.Value = this.masterProyectoDestino.Value;
                #endregion

                this.detListDestino.Add(det);
                this.LoadDetails(false);
                //this.EnableControls(true);
                this.isValid_Det_Destino = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-AddNewRow_Det"));
            }
        }

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        private bool ValidateRow_Det()
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.detListOrigen.Count > 0)
                    {
                        bool validField = true;
                        this.isValid_Det_Origen = true;

                        int fila = this.gvDetailOrigen.FocusedRowHandle;

                        #region Validacion de nulls
                        #region Centro Costo
                        validField = _bc.ValidGridCell(this.gvDetailOrigen, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                        if (!validField)
                            this.isValid_Det_Origen = false;
                        #endregion
                        #region Linea Presupuesto
                        validField = _bc.ValidGridCell(this.gvDetailOrigen, string.Empty, fila, "LineaPresupuestoID", false, false, false, AppMasters.plLineaPresupuesto);
                        if (!validField)
                            this.isValid_Det_Origen = false;
                        #endregion
                        #endregion
                        #region Validacion de PKs
                        if (this.isValid_Det_Origen && this.detListOrigen.Count > 0)
                        {
                            DTO_plPresupuestoDeta det = this.detListOrigen[fila];
                            int count = this.detListOrigen.Where(x => x.CentroCostoID.Value == det.CentroCostoID.Value &&
                                                                      x.LineaPresupuestoID.Value == det.LineaPresupuestoID.Value &&
                                                                      x.ProyectoID.Value == det.ProyectoID.Value).Count();
                            if (count > 1)
                            {
                                this.isValid_Det_Origen = false;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidPresDet));
                            }
                        }
                        if (!this.validParticion)
                        {
                            GridColumn col = this.gvDetailOrigen.Columns[this.unboundPrefix + "LineaPresupuestoID"];
                            this.gvDetailOrigen.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidKeyActividadLinea));
                            this.isValid_Det_Origen = false;
                            return this.isValid_Det_Origen;
                        }
                        #endregion
                        #region Valor movimiento
                        validField = _bc.ValidGridCellValue(this.gvDetailOrigen, string.Empty, fila, "VlrMvtoLocal", false, true, false, false);
                        if (!validField)
                            this.isValid_Det_Origen = false;
                        if (this.loadME)
                        {
                            validField = _bc.ValidGridCellValue(this.gvDetailOrigen, string.Empty, fila, "VlrMvtoExtr", false, true, false, false);
                            if (!validField)
                                this.isValid_Det_Origen = false;
                        }
                        #endregion
                        #region Calculo de datos
                        if (this.isValid_Det_Origen && this.detListOrigen.Count > 0)
                        {
                            #region Totales
                            DTO_plPresupuestoDeta det = this.detListOrigen[fila];

                            if (det.VlrNuevoSaldoLoc.Value < 0)
                            {
                                GridColumn col = this.gvDetailOrigen.Columns[unboundPrefix + "VlrNuevoSaldoLoc"];
                                this.gvDetailOrigen.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                                this.isValid_Det_Origen = false;
                                return false;
                            }

                            if (det.VlrNuevoSaldoExtr.Value < 0)
                            {
                                GridColumn col = this.gvDetailOrigen.Columns[unboundPrefix + "VlrNuevoSaldoExtr"];
                                this.gvDetailOrigen.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                                this.isValid_Det_Origen = false;
                                return false;
                            }
                            decimal sumML = Math.Round(det.ValorLoc00.Value.Value + det.ValorLoc01.Value.Value + det.ValorLoc02.Value.Value + det.ValorLoc03.Value.Value +
                                            det.ValorLoc04.Value.Value + det.ValorLoc05.Value.Value + det.ValorLoc06.Value.Value + det.ValorLoc07.Value.Value +
                                            det.ValorLoc08.Value.Value + det.ValorLoc09.Value.Value + det.ValorLoc10.Value.Value + det.ValorLoc11.Value.Value + det.ValorLoc12.Value.Value);

                            if (sumML != det.VlrMvtoLocal.Value.Value)
                            {
                                this.isValid_Det_Origen = false;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                            }
                            if (this.loadME)
                            {
                                decimal sumME = Math.Round(det.ValorExt00.Value.Value + det.ValorExt01.Value.Value + det.ValorExt02.Value.Value + det.ValorExt03.Value.Value +
                                         det.ValorExt04.Value.Value + det.ValorExt05.Value.Value + det.ValorExt06.Value.Value + det.ValorExt07.Value.Value +
                                         det.ValorExt08.Value.Value + det.ValorExt09.Value.Value + det.ValorExt10.Value.Value + det.ValorExt11.Value.Value + det.ValorExt12.Value.Value);

                                if (sumME != det.VlrMvtoExtr.Value.Value)
                                {
                                    this.isValid_Det_Origen = false;
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    if (this.detListDestino.Count > 0)
                    {
                        bool validField = true;
                        this.isValid_Det_Destino = true;

                        int fila = this.gvDetailDestino.FocusedRowHandle;

                        #region Validacion de nulls
                        #region Centro Costo
                        validField = _bc.ValidGridCell(this.gvDetailDestino, string.Empty, fila, "CentroCostoID", false, true, true, AppMasters.coCentroCosto);
                        if (!validField)
                            this.isValid_Det_Destino = false;
                        #endregion
                        #region Linea Presupuesto
                        validField = _bc.ValidGridCell(this.gvDetailDestino, string.Empty, fila, "LineaPresupuestoID", false, false, false, AppMasters.plLineaPresupuesto);
                        if (!validField)
                            this.isValid_Det_Destino = false;
                        #endregion
                        #endregion
                        #region Validacion de PKs
                        if (this.isValid_Det_Destino && this.detListDestino.Count > 0)
                        {
                            DTO_plPresupuestoDeta det = this.detListDestino[fila];
                            int count = this.detListDestino.Where(x => x.CentroCostoID.Value == det.CentroCostoID.Value &&
                                                                       x.LineaPresupuestoID.Value == det.LineaPresupuestoID.Value &&
                                                                       x.ProyectoID.Value == det.ProyectoID.Value).Count();
                            if (count > 1)
                            {
                                this.isValid_Det_Destino = false;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidPresDet));
                            }
                        }
                        if (!this.validParticion)
                        {
                            GridColumn col = this.gvDetailDestino.Columns[this.unboundPrefix + "LineaPresupuestoID"];
                            this.gvDetailDestino.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidKeyActividadLinea));
                            this.isValid_Det_Destino = false;
                            return this.isValid_Det_Destino;
                        }
                        #endregion
                        #region Valor movimiento
                        validField = _bc.ValidGridCellValue(this.gvDetailDestino, string.Empty, fila, "VlrMvtoLocal", false, true, false, false);
                        if (!validField)
                            this.isValid_Det_Destino = false;
                        if (this.loadME)
                        {
                            validField = _bc.ValidGridCellValue(this.gvDetailDestino, string.Empty, fila, "VlrMvtoExtr", false, true, false, false);
                            if (!validField)
                                this.isValid_Det_Destino = false;
                        }
                        #endregion
                        #region Calculo de datos
                        if (this.isValid_Det_Destino && this.detListDestino.Count > 0)
                        {
                            #region Totales
                            DTO_plPresupuestoDeta det = this.detListDestino[fila];

                            if (det.VlrNuevoSaldoLoc.Value < 0)
                            {
                                GridColumn col = this.gvDetailDestino.Columns[unboundPrefix + "VlrNuevoSaldoLoc"];
                                this.gvDetailDestino.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                                this.isValid_Det_Destino = false;
                                return false;
                            }

                            if (det.VlrNuevoSaldoExtr.Value < 0)
                            {
                                GridColumn col = this.gvDetailDestino.Columns[unboundPrefix + "VlrNuevoSaldoExtr"];
                                this.gvDetailDestino.SetColumnError(col, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_newValueInvalid));
                                this.isValid_Det_Destino = false;
                                return false;
                            }
                            decimal sumML = Math.Round(det.ValorLoc00.Value.Value + det.ValorLoc01.Value.Value + det.ValorLoc02.Value.Value + det.ValorLoc03.Value.Value +
                                            det.ValorLoc04.Value.Value + det.ValorLoc05.Value.Value + det.ValorLoc06.Value.Value + det.ValorLoc07.Value.Value +
                                            det.ValorLoc08.Value.Value + det.ValorLoc09.Value.Value + det.ValorLoc10.Value.Value + det.ValorLoc11.Value.Value + det.ValorLoc12.Value.Value);

                            if (sumML != det.VlrMvtoLocal.Value.Value)
                            {
                                this.isValid_Det_Destino = false;
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                            }
                            if (this.loadME)
                            {
                                decimal sumME = Math.Round(det.ValorExt00.Value.Value + det.ValorExt01.Value.Value + det.ValorExt02.Value.Value + det.ValorExt03.Value.Value +
                                         det.ValorExt04.Value.Value + det.ValorExt05.Value.Value + det.ValorExt06.Value.Value + det.ValorExt07.Value.Value +
                                         det.ValorExt08.Value.Value + det.ValorExt09.Value.Value + det.ValorExt10.Value.Value + det.ValorExt11.Value.Value + det.ValorExt12.Value.Value);

                                if (sumME != det.VlrMvtoExtr.Value.Value)
                                {
                                    this.isValid_Det_Destino = false;
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_InvalidValMes));
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                this.isValid_Det_Origen = false;
                this.isValid_Det_Destino = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "ValidateRow_Doc"));
            }
            if (this.IsFocusedGridOrigen)
                return this.isValid_Det_Origen;
            else
                return this.isValid_Det_Destino;

        }

        /// <summary>
        /// Evento que obliga que se ejecute una funcion al cambiar de fila
        /// <param name="fila">Fila que se debe actualizar</param>
        /// </summary>
        private void RowIndexChanged_Det(int fila)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (!this.disableValidate_Det_Origen & fila >= 0)
                    {
                        this.LoadFooter(fila);
                        this.isValid_Det_Origen = true;
                        this.gvDetailOrigen.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detListOrigen[fila].NewRowPresup;
                        this.gvDetailOrigen.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detListOrigen[fila].NewRowPresup;
                    }
                }
                else
                {
                    if (!this.disableValidate_Det_Destino & fila >= 0)
                    {
                        this.LoadFooter(fila);
                        this.isValid_Det_Destino = true;
                        this.gvDetailDestino.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detListDestino[fila].NewRowPresup;
                        this.gvDetailDestino.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detListDestino[fila].NewRowPresup;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "RowIndexChanged_Det"));
            }
        }

        /// <summary>
        /// Realiza el traslado automatico del los valores digitados de una lista a otra
        /// </summary>
        private void AssignTrasladoDirecto()
        {
            try
            {
                if (this.trasladoDirectoInd && this.detListOrigen.Count > 0 && this.detListDestino.Count > 0)
                {
                    if (this.IsFocusedGridOrigen)
                    {
                        string fieldName = this.gvDetailOrigen.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                        int indexOrigen = this.gvDetailOrigen.FocusedRowHandle;

                        //Verifica que si exista la LineaPres-CentroCto en la lista contigua
                        int count = this.detListDestino.Where(x => x.CentroCostoID.Value == this.detListOrigen[indexOrigen].CentroCostoID.Value &&
                                                                   x.LineaPresupuestoID.Value == this.detListOrigen[indexOrigen].LineaPresupuestoID.Value &&
                                                                   x.ProyectoID.Value == this.detListOrigen[indexOrigen].ProyectoID.Value).Count();

                        #region Mda Local
                        if (fieldName == "VlrMvtoLocal" && this.detListOrigen[indexOrigen].VlrMvtoLocal.Value != 0)
                        {
                            if (count > 0)
                            {
                                //Si existe obtiene el index del item contiguo
                                int indexDestino = this.detListDestino.Where(x => x.CentroCostoID.Value == this.detListOrigen[indexOrigen].CentroCostoID.Value &&
                                                                                x.LineaPresupuestoID.Value == this.detListOrigen[indexOrigen].LineaPresupuestoID.Value &&
                                                                                x.ProyectoID.Value == this.detListOrigen[indexOrigen].ProyectoID.Value).ToList().First().Consecutivo.Value.Value;
                                indexDestino = indexDestino - 1;
                                //Verifica que el saldo disponible sea valido
                                if (this.detListDestino[indexDestino].VlrSaldoAntLoc.Value < this.detListOrigen[indexOrigen].VlrMvtoLocal.Value)
                                {
                                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoNotAvailable);
                                    MessageBox.Show(string.Format(msg, this.detListOrigen[indexOrigen].LineaPresupuestoID.Value, this.detListOrigen[indexOrigen].CentroCostoID.Value));
                                    this.isValid_Det_Origen = false;
                                    this.chkDirecto.Checked = false;
                                    return;
                                }
                                //Asigna valores al item contiguo
                                this.detListDestino[indexDestino].VlrMvtoLocal.Value = this.detListDestino[indexDestino].VlrMvtoLocal.Value + (this.detListOrigen[indexOrigen].VlrMvtoLocal.Value * -1);
                                this.detListDestino[indexDestino].VlrNuevoSaldoLoc.Value = this.detListDestino[indexDestino].VlrSaldoAntLoc.Value + this.detListDestino[indexDestino].VlrMvtoLocal.Value;
                                this.detListDestino[indexDestino].LoadParticionLocalInd = this.detListDestino[indexDestino].VlrMvtoLocal.Value != 0 ? false : true;
                                this.IsFocusedGridOrigen = false;
                                this.LoadParticiones(indexDestino, false, TipoMoneda.Local);
                                this.gcDetailDestino.RefreshDataSource();
                                this.IsFocusedGridOrigen = true;
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_LineaPresNotExist));
                                this.isValid_Det_Origen = false;
                                this.chkDirecto.Checked = false;
                            }
                        }
                        #endregion
                        #region Mda Extranjera
                        if (fieldName == "VlrMvtoExtr" && this.detListOrigen[indexOrigen].VlrMvtoExtr.Value != 0)
                        {
                            if (count > 0)
                            {
                                //Si existe obtiene el index del item existente
                                int indexDestino = this.detListDestino.Where(x => x.CentroCostoID.Value == this.detListOrigen[indexOrigen].CentroCostoID.Value &&
                                                                                x.LineaPresupuestoID.Value == this.detListOrigen[indexOrigen].LineaPresupuestoID.Value &&
                                                                                x.ProyectoID.Value == this.detListOrigen[indexOrigen].ProyectoID.Value).ToList().First().Consecutivo.Value.Value;
                                indexDestino = indexDestino - 1;
                                //Verifica que el saldo disponible sea valido
                                if (this.detListDestino[indexDestino].VlrSaldoAntExtr.Value < this.detListOrigen[indexOrigen].VlrMvtoExtr.Value)
                                {
                                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoNotAvailable);
                                    MessageBox.Show(string.Format(msg, this.detListOrigen[indexOrigen].LineaPresupuestoID.Value, this.detListOrigen[indexOrigen].CentroCostoID.Value));
                                    this.isValid_Det_Origen = false;
                                    this.chkDirecto.Checked = false;
                                    return;
                                }
                                //Asigna valores al item existente
                                this.detListDestino[indexDestino].VlrMvtoExtr.Value = this.detListDestino[indexDestino].VlrMvtoExtr.Value + (this.detListOrigen[indexOrigen].VlrMvtoExtr.Value * -1);
                                this.detListDestino[indexDestino].VlrNuevoSaldoExtr.Value = this.detListDestino[indexDestino].VlrSaldoAntExtr.Value + this.detListDestino[indexDestino].VlrMvtoExtr.Value;
                                this.detListDestino[indexDestino].LoadParticionExtrInd = this.detListDestino[indexDestino].VlrMvtoExtr.Value != 0 ? false : true;
                                this.IsFocusedGridOrigen = false;
                                this.LoadParticiones(indexDestino, false, TipoMoneda.Foreign);
                                this.gcDetailDestino.RefreshDataSource();
                                this.IsFocusedGridOrigen = true;
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_LineaPresNotExist));
                                this.isValid_Det_Origen = false;
                                this.chkDirecto.Checked = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        string fieldName = this.gvDetailDestino.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                        int indexDestino = this.gvDetailDestino.FocusedRowHandle;

                        //Verifica que si exista la LineaPres-CentroCto en la lista contigua
                        int count = this.detListOrigen.Where(x => x.CentroCostoID.Value == this.detListDestino[indexDestino].CentroCostoID.Value &&
                                                                   x.LineaPresupuestoID.Value == this.detListDestino[indexDestino].LineaPresupuestoID.Value &&
                                                                   x.ProyectoID.Value == this.detListDestino[indexDestino].ProyectoID.Value).Count();

                        #region Mda Local
                        if (fieldName == "VlrMvtoLocal" && this.detListDestino[indexDestino].VlrMvtoLocal.Value != 0)
                        {
                            if (count > 0)
                            {
                                //Si existe obtiene el index del item contiguo
                                int indexOrigen = this.detListOrigen.Where(x => x.CentroCostoID.Value == this.detListDestino[indexDestino].CentroCostoID.Value &&
                                                                                x.LineaPresupuestoID.Value == this.detListDestino[indexDestino].LineaPresupuestoID.Value &&
                                                                                x.ProyectoID.Value == this.detListDestino[indexDestino].ProyectoID.Value).ToList().First().Consecutivo.Value.Value;
                                indexOrigen = indexOrigen - 1;
                                //Verifica que el saldo disponible sea valido
                                if (this.detListOrigen[indexOrigen].VlrSaldoAntLoc.Value < this.detListDestino[indexDestino].VlrMvtoLocal.Value)
                                {
                                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoNotAvailable);
                                    MessageBox.Show(string.Format(msg, this.detListDestino[indexDestino].LineaPresupuestoID.Value, this.detListDestino[indexDestino].CentroCostoID.Value));
                                    this.isValid_Det_Origen = false;
                                    this.chkDirecto.Checked = false;
                                    return;
                                }
                                //Asigna valores al item contiguo
                                this.detListOrigen[indexOrigen].VlrMvtoLocal.Value = this.detListOrigen[indexOrigen].VlrMvtoLocal.Value + (this.detListDestino[indexDestino].VlrMvtoLocal.Value * -1);
                                this.detListOrigen[indexOrigen].VlrNuevoSaldoLoc.Value = this.detListOrigen[indexOrigen].VlrSaldoAntLoc.Value + this.detListOrigen[indexOrigen].VlrMvtoLocal.Value;
                                this.detListOrigen[indexOrigen].LoadParticionLocalInd = this.detListOrigen[indexOrigen].VlrMvtoLocal.Value != 0 ? false : true;
                                this.IsFocusedGridOrigen = true;
                                this.LoadParticiones(indexOrigen, false, TipoMoneda.Local);
                                this.gcDetailOrigen.RefreshDataSource();
                                this.IsFocusedGridOrigen = false;
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_LineaPresNotExist));
                                this.isValid_Det_Origen = false;
                                this.chkDirecto.Checked = false;
                            }
                        }
                        #endregion
                        #region Mda Extranjera
                        if (fieldName == "VlrMvtoExtr" && this.detListDestino[indexDestino].VlrMvtoExtr.Value != 0)
                        {
                            if (count > 0)
                            {
                                //Si existe obtiene el index del item existente
                                int indexOrigen = this.detListOrigen.Where(x => x.CentroCostoID.Value == this.detListDestino[indexDestino].CentroCostoID.Value &&
                                                                                x.LineaPresupuestoID.Value == this.detListDestino[indexDestino].LineaPresupuestoID.Value &&
                                                                                x.ProyectoID.Value == this.detListDestino[indexDestino].ProyectoID.Value).ToList().First().Consecutivo.Value.Value;
                                indexOrigen = indexOrigen - 1;
                                //Verifica que el saldo disponible sea valido
                                if (this.detListOrigen[indexOrigen].VlrSaldoAntExtr.Value < this.detListDestino[indexDestino].VlrMvtoExtr.Value)
                                {
                                    string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoNotAvailable);
                                    MessageBox.Show(string.Format(msg, this.detListDestino[indexDestino].LineaPresupuestoID.Value, this.detListDestino[indexDestino].CentroCostoID.Value));
                                    this.isValid_Det_Origen = false;
                                    this.chkDirecto.Checked = false;
                                    return;
                                }
                                //Asigna valores al item existente
                                this.detListOrigen[indexOrigen].VlrMvtoExtr.Value = this.detListOrigen[indexOrigen].VlrMvtoExtr.Value + (this.detListDestino[indexDestino].VlrMvtoExtr.Value * -1);
                                this.detListOrigen[indexOrigen].VlrNuevoSaldoExtr.Value = this.detListOrigen[indexOrigen].VlrSaldoAntExtr.Value + this.detListOrigen[indexOrigen].VlrMvtoExtr.Value;
                                this.detListOrigen[indexOrigen].LoadParticionExtrInd = this.detListOrigen[indexOrigen].VlrMvtoExtr.Value != 0 ? false : true;
                                this.IsFocusedGridOrigen = true;
                                this.LoadParticiones(indexOrigen, false, TipoMoneda.Foreign);
                                this.gcDetailOrigen.RefreshDataSource();
                                this.IsFocusedGridOrigen = false;
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_LineaPresNotExist));
                                this.isValid_Det_Origen = false;
                                this.chkDirecto.Checked = false;
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Obtiene los filtros de la linea presupuestal y Centro de Costo de la grilla
        /// </summary>
        private void GetFiltersLineaPresup()
        {
            bool GrupoPresupuestalUsuarioInd = this._bc.GetControlValueByCompany(ModulesPrefix.pl, AppControl.pl_ValidaGrupoPresu).Equals("1") ? true : false;
            if (GrupoPresupuestalUsuarioInd)
            {
                #region Variables
                List<string> listLineaPresupuesto = new List<string>();
                List<string> listCentroCosto = new List<string>();
                List<string> listGruposPresupuesto = new List<string>();
                int userID = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
                #endregion

                //Trae los Grupos Presupuestales x User que existen  
                long countGrupoPresUser = _bc.AdministrationModel.MasterComplex_Count(AppMasters.plGrupoPresupuestalUsuario, null, null);
                var grupPresupUser = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.plGrupoPresupuestalUsuario, countGrupoPresUser, 1, null, null).ToList();

                if (grupPresupUser != null && grupPresupUser.Count > 0)
                {
                    foreach (var grupoUser in grupPresupUser)
                    {
                        #region Llena la lista de GruposPresupuesto
                        DTO_plGrupoPresupuestalUsuario dtoTipo = (DTO_plGrupoPresupuestalUsuario)grupoUser;
                        if (dtoTipo.seUsuarioID.Value == userID.ToString())
                            listGruposPresupuesto.Add(dtoTipo.GrupoPresupuestoID.Value);
                        #endregion
                    }

                    #region Crea Filtros  para plGrupoPresupuestalLinea
                    List<DTO_glConsultaFiltro> filtrosGrupoPresupLinea = new List<DTO_glConsultaFiltro>();
                    foreach (string grupoPresup in listGruposPresupuesto)
                    {
                        filtrosGrupoPresupLinea.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "GrupoPresupuestoID",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = grupoPresup,
                            OperadorSentencia = "OR"
                        });
                    }
                    DTO_glConsulta consultaGrupoPresLinea = new DTO_glConsulta();
                    consultaGrupoPresLinea.Filtros = filtrosGrupoPresupLinea;
                    #endregion
                    //Trae los Grupos Presupuestales x Linea fitrados  
                    long countGrupoPresLinea = _bc.AdministrationModel.MasterComplex_Count(AppMasters.plGrupoPresupuestalLinea, consultaGrupoPresLinea, null);
                    var grupPresupLinea = _bc.AdministrationModel.MasterComplex_GetPaged(AppMasters.plGrupoPresupuestalLinea, countGrupoPresUser, 1, consultaGrupoPresLinea, null).ToList();

                    if (grupPresupLinea != null && grupPresupLinea.Count > 0)
                    {
                        foreach (var grupoLinea in grupPresupLinea)
                        {
                            #region Llena la lista de LineasPresupuesto y CentroCosto
                            DTO_plGrupoPresupuestalLinea dtoTipo = (DTO_plGrupoPresupuestalLinea)grupoLinea;
                            if (!listLineaPresupuesto.Contains(dtoTipo.LineaPresupuestoID.Value))
                                listLineaPresupuesto.Add(dtoTipo.LineaPresupuestoID.Value);
                            if (!listCentroCosto.Contains(dtoTipo.CentroCostoID.Value))
                                listCentroCosto.Add(dtoTipo.CentroCostoID.Value);
                            #endregion
                        }

                        #region CREA LOS FILTROS PARA GRILLA
                        //Crea filtros para LineaPresupuestoID
                        this.filtrosLineaPres = new List<DTO_glConsultaFiltro>();
                        foreach (string lineaPres in listLineaPresupuesto)
                        {
                            this.filtrosLineaPres.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "LineaPresupuestoID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = lineaPres,
                                OperadorSentencia = "OR"
                            });
                        }

                        //Crea filtros para CentroCosto
                        this.filtrosCentroCosto = new List<DTO_glConsultaFiltro>();
                        foreach (string centroCto in listCentroCosto)
                        {
                            this.filtrosCentroCosto.Add(new DTO_glConsultaFiltro()
                            {
                                CampoFisico = "CentroCostoID",
                                OperadorFiltro = OperadorFiltro.Igual,
                                ValorFiltro = centroCto,
                                OperadorSentencia = "OR"
                            });
                        }
                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Evento al salir del control del proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterProyecto_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.masterProyectoOrigen.Value != this.proyectoIDOrigen)
                        this.LoadData();
                }
                else
                {
                    if (this.masterProyectoDestino.Value != this.proyectoIDDestino)
                        this.LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto", "masterProyecto_Leave"));
            }
        }

        /// <summary>
        /// Evalua datos al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtPeriod_EditValueChanged()
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    this.periodo = this.dtPeriodOrigen.DateTime;
                    if (this.masterProyectoOrigen.Value != this.proyectoIDOrigen)
                        this.LoadData();
                }
                else
                {
                    //this.periodo = this.dtPeriodOrigen.DateTime;
                    if (this.masterProyectoDestino.Value != this.proyectoIDDestino)
                        this.LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto", "dtPeriod_EditValueChanged"));
            }
        }

        /// <summary>
        /// ento al salir de un control de valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Mes_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    TextEdit ctrl = (TextEdit)sender;
                    decimal value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                    decimal tc = Convert.ToDecimal(this.txtTasaCambioOrigen.EditValue, CultureInfo.InvariantCulture);
                    int fila = this.gvDetailOrigen.FocusedRowHandle;
                    if (fila < 0 || this.detListOrigen.Count == 0)
                        return;
                    switch (ctrl.Name)
                    {
                        #region Mda Local
                        case "txt_Mes00_Local":
                            this.detListOrigen[fila].ValorLoc00.Value = value;
                            this.detListOrigen[fila].EquivExt00.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc00.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes01_Local":
                            this.detListOrigen[fila].ValorLoc01.Value = value;
                            this.detListOrigen[fila].EquivExt01.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc01.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes02_Local":
                            this.detListOrigen[fila].ValorLoc02.Value = value;
                            this.detListOrigen[fila].EquivExt02.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc02.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes03_Local":
                            this.detListOrigen[fila].ValorLoc03.Value = value;
                            this.detListOrigen[fila].EquivExt03.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc03.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes04_Local":
                            this.detListOrigen[fila].ValorLoc04.Value = value;
                            this.detListOrigen[fila].EquivExt04.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc04.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes05_Local":
                            this.detListOrigen[fila].ValorLoc05.Value = value;
                            this.detListOrigen[fila].EquivExt05.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc05.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes06_Local":
                            this.detListOrigen[fila].ValorLoc06.Value = value;
                            this.detListOrigen[fila].EquivExt06.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc06.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes07_Local":
                            this.detListOrigen[fila].ValorLoc07.Value = value;
                            this.detListOrigen[fila].EquivExt07.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc07.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes08_Local":
                            this.detListOrigen[fila].ValorLoc08.Value = value;
                            this.detListOrigen[fila].EquivExt08.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc08.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes09_Local":
                            this.detListOrigen[fila].ValorLoc09.Value = value;
                            this.detListOrigen[fila].EquivExt09.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc09.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes10_Local":
                            this.detListOrigen[fila].ValorLoc10.Value = value;
                            this.detListOrigen[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc10.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes11_Local":
                            this.detListOrigen[fila].ValorLoc11.Value = value;
                            this.detListOrigen[fila].EquivExt11.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc11.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes12_Local":
                            this.detListOrigen[fila].ValorLoc12.Value = value;
                            this.detListOrigen[fila].EquivExt12.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorLoc12.Value.Value / tc, 2) : 0;
                            break;
                        #endregion
                        #region Mda Extranjera
                        case "txt_Mes00_Extr":
                            this.detListOrigen[fila].ValorExt00.Value = value;
                            this.detListOrigen[fila].EquivLoc00.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt00.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes01_Extr":
                            this.detListOrigen[fila].ValorExt01.Value = value;
                            this.detListOrigen[fila].EquivLoc01.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt01.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes02_Extr":
                            this.detListOrigen[fila].ValorExt02.Value = value;
                            this.detListOrigen[fila].EquivLoc02.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt02.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes03_Extr":
                            this.detListOrigen[fila].ValorExt03.Value = value;
                            this.detListOrigen[fila].EquivLoc03.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt03.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes04_Extr":
                            this.detListOrigen[fila].ValorExt04.Value = value;
                            this.detListOrigen[fila].EquivLoc04.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt04.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes05_Extr":
                            this.detListOrigen[fila].ValorExt05.Value = value;
                            this.detListOrigen[fila].EquivLoc05.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt05.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes06_Extr":
                            this.detListOrigen[fila].ValorExt06.Value = value;
                            this.detListOrigen[fila].EquivLoc06.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt06.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes07_Extr":
                            this.detListOrigen[fila].ValorExt07.Value = value;
                            this.detListOrigen[fila].EquivLoc07.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt07.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes08_Extr":
                            this.detListOrigen[fila].ValorExt08.Value = value;
                            this.detListOrigen[fila].EquivLoc08.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt08.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes09_Extr":
                            this.detListOrigen[fila].ValorExt09.Value = value;
                            this.detListOrigen[fila].EquivLoc09.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt09.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes10_Extr":
                            this.detListOrigen[fila].ValorExt10.Value = value;
                            this.detListOrigen[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt10.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes11_Extr":
                            this.detListOrigen[fila].ValorExt11.Value = value;
                            this.detListOrigen[fila].EquivLoc11.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt11.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes12_Extr":
                            this.detListOrigen[fila].ValorExt12.Value = value;
                            this.detListOrigen[fila].EquivLoc12.Value = this.loadME && tc != 0 ? Math.Round(this.detListOrigen[fila].ValorExt12.Value.Value * tc) : 0;
                            break;
                        #endregion
                    }

                    //Calcula el Total del movimiento(Local)
                    this.detListOrigen.ForEach(x => x.VlrMvtoLocal.Value = Math.Round(x.ValorLoc00.Value.Value + x.ValorLoc01.Value.Value + x.ValorLoc02.Value.Value + x.ValorLoc03.Value.Value
                                              + x.ValorLoc04.Value.Value + x.ValorLoc05.Value.Value + x.ValorLoc06.Value.Value + x.ValorLoc07.Value.Value + x.ValorLoc08.Value.Value
                                              + x.ValorLoc09.Value.Value + x.ValorLoc10.Value.Value + x.ValorLoc11.Value.Value + x.ValorLoc12.Value.Value));
                    //Calcula el Total del movimiento(Extr)
                    if (this.loadME)
                    {
                        this.detListOrigen.ForEach(x => x.VlrMvtoExtr.Value = Math.Round(x.ValorExt00.Value.Value + x.ValorExt01.Value.Value + x.ValorExt02.Value.Value + x.ValorExt03.Value.Value
                                                + x.ValorExt04.Value.Value + x.ValorExt05.Value.Value + x.ValorExt06.Value.Value + x.ValorExt07.Value.Value + x.ValorExt08.Value.Value
                                                + x.ValorExt09.Value.Value + x.ValorExt10.Value.Value + x.ValorExt11.Value.Value + x.ValorExt12.Value.Value));
                    }
                    foreach (var item in this.detListOrigen)
                    {
                        item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                        item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                        item.LoadParticionLocalInd = item.VlrMvtoLocal.Value != 0 ? false : true;
                        item.LoadParticionExtrInd = item.VlrMvtoExtr.Value != 0 ? false : true;
                    }
                    this.gcDetailOrigen.RefreshDataSource();
                }
                else
                {
                    TextEdit ctrl = (TextEdit)sender;
                    decimal value = Convert.ToDecimal(ctrl.EditValue, CultureInfo.InvariantCulture);
                    decimal tc = Convert.ToDecimal(this.txtTasaCambioDestino.EditValue, CultureInfo.InvariantCulture);
                    int fila = this.gvDetailDestino.FocusedRowHandle;

                    if (fila < 0 || this.detListDestino.Count == 0)
                        return;
                    switch (ctrl.Name)
                    {
                        #region Mda Local
                        case "txt_Mes00_Local":
                            this.detListDestino[fila].ValorLoc00.Value = value;
                            this.detListDestino[fila].EquivExt00.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc00.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes01_Local":
                            this.detListDestino[fila].ValorLoc01.Value = value;
                            this.detListDestino[fila].EquivExt01.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc01.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes02_Local":
                            this.detListDestino[fila].ValorLoc02.Value = value;
                            this.detListDestino[fila].EquivExt02.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc02.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes03_Local":
                            this.detListDestino[fila].ValorLoc03.Value = value;
                            this.detListDestino[fila].EquivExt03.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc03.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes04_Local":
                            this.detListDestino[fila].ValorLoc04.Value = value;
                            this.detListDestino[fila].EquivExt04.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc04.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes05_Local":
                            this.detListDestino[fila].ValorLoc05.Value = value;
                            this.detListDestino[fila].EquivExt05.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc05.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes06_Local":
                            this.detListDestino[fila].ValorLoc06.Value = value;
                            this.detListDestino[fila].EquivExt06.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc06.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes07_Local":
                            this.detListDestino[fila].ValorLoc07.Value = value;
                            this.detListDestino[fila].EquivExt07.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc07.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes08_Local":
                            this.detListDestino[fila].ValorLoc08.Value = value;
                            this.detListDestino[fila].EquivExt08.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc08.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes09_Local":
                            this.detListDestino[fila].ValorLoc09.Value = value;
                            this.detListDestino[fila].EquivExt09.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc09.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes10_Local":
                            this.detListDestino[fila].ValorLoc10.Value = value;
                            this.detListDestino[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc10.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes11_Local":
                            this.detListDestino[fila].ValorLoc11.Value = value;
                            this.detListDestino[fila].EquivExt11.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc11.Value.Value / tc, 2) : 0;
                            break;
                        case "txt_Mes12_Local":
                            this.detListDestino[fila].ValorLoc12.Value = value;
                            this.detListDestino[fila].EquivExt12.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorLoc12.Value.Value / tc, 2) : 0;
                            break;
                        #endregion
                        #region Mda Extranjera
                        case "txt_Mes00_Extr":
                            this.detListDestino[fila].ValorExt00.Value = value;
                            this.detListDestino[fila].EquivLoc00.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt00.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes01_Extr":
                            this.detListDestino[fila].ValorExt01.Value = value;
                            this.detListDestino[fila].EquivLoc01.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt01.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes02_Extr":
                            this.detListDestino[fila].ValorExt02.Value = value;
                            this.detListDestino[fila].EquivLoc02.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt02.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes03_Extr":
                            this.detListDestino[fila].ValorExt03.Value = value;
                            this.detListDestino[fila].EquivLoc03.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt03.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes04_Extr":
                            this.detListDestino[fila].ValorExt04.Value = value;
                            this.detListDestino[fila].EquivLoc04.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt04.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes05_Extr":
                            this.detListDestino[fila].ValorExt05.Value = value;
                            this.detListDestino[fila].EquivLoc05.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt05.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes06_Extr":
                            this.detListDestino[fila].ValorExt06.Value = value;
                            this.detListDestino[fila].EquivLoc06.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt06.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes07_Extr":
                            this.detListDestino[fila].ValorExt07.Value = value;
                            this.detListDestino[fila].EquivLoc07.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt07.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes08_Extr":
                            this.detListDestino[fila].ValorExt08.Value = value;
                            this.detListDestino[fila].EquivLoc08.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt08.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes09_Extr":
                            this.detListDestino[fila].ValorExt09.Value = value;
                            this.detListDestino[fila].EquivLoc09.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt09.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes10_Extr":
                            this.detListDestino[fila].ValorExt10.Value = value;
                            this.detListDestino[fila].EquivLoc10.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt10.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes11_Extr":
                            this.detListDestino[fila].ValorExt11.Value = value;
                            this.detListDestino[fila].EquivLoc11.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt11.Value.Value * tc) : 0;
                            break;
                        case "txt_Mes12_Extr":
                            this.detListDestino[fila].ValorExt12.Value = value;
                            this.detListDestino[fila].EquivLoc12.Value = this.loadME && tc != 0 ? Math.Round(this.detListDestino[fila].ValorExt12.Value.Value * tc) : 0;
                            break;
                        #endregion
                    }

                    //Calcula el Total del movimiento(Local)
                    this.detListDestino.ForEach(x => x.VlrMvtoLocal.Value = Math.Round(x.ValorLoc00.Value.Value + x.ValorLoc01.Value.Value + x.ValorLoc02.Value.Value + x.ValorLoc03.Value.Value
                                              + x.ValorLoc04.Value.Value + x.ValorLoc05.Value.Value + x.ValorLoc06.Value.Value + x.ValorLoc07.Value.Value + x.ValorLoc08.Value.Value
                                              + x.ValorLoc09.Value.Value + x.ValorLoc10.Value.Value + x.ValorLoc11.Value.Value + x.ValorLoc12.Value.Value));
                    //Calcula el Total del movimiento(Extr)
                    if (this.loadME)
                    {
                        this.detListDestino.ForEach(x => x.VlrMvtoExtr.Value = Math.Round(x.ValorExt00.Value.Value + x.ValorExt01.Value.Value + x.ValorExt02.Value.Value + x.ValorExt03.Value.Value
                                                + x.ValorExt04.Value.Value + x.ValorExt05.Value.Value + x.ValorExt06.Value.Value + x.ValorExt07.Value.Value + x.ValorExt08.Value.Value
                                                + x.ValorExt09.Value.Value + x.ValorExt10.Value.Value + x.ValorExt11.Value.Value + x.ValorExt12.Value.Value));
                    }
                    foreach (var item in this.detListDestino)
                    {
                        item.VlrNuevoSaldoLoc.Value = item.VlrSaldoAntLoc.Value + item.VlrMvtoLocal.Value;
                        item.VlrNuevoSaldoExtr.Value = item.VlrSaldoAntExtr.Value + item.VlrMvtoExtr.Value;
                        item.LoadParticionLocalInd = item.VlrMvtoLocal.Value != 0 ? false : true;
                        item.LoadParticionExtrInd = item.VlrMvtoExtr.Value != 0 ? false : true;
                    }


                    this.gcDetailDestino.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto", "txt_Mes_Leave"));
            }
        }

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    string fieldName = this.gvDetailOrigen.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                    if (fieldName == "DescripTExt")
                        this.richEditControl.Document.Text = this.gvDetailOrigen.GetFocusedRowCellValue(fieldName).ToString();
                }
                else
                {
                    string fieldName = this.gvDetailDestino.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                    if (fieldName == "DescripTExt")
                        this.richEditControl.Document.Text = this.gvDetailDestino.GetFocusedRowCellValue(fieldName).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "riPopup_QueryPopUp"));
            }
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        /// <summary>
        /// Entra al proyecto origen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitPanel_Panel1_Enter(object sender, EventArgs e)
        {
            this.IsFocusedGridOrigen = true;
        }

        /// <summary>
        /// Entra el proyecto destino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitPanel_Panel2_Enter(object sender, EventArgs e)
        {
            this.IsFocusedGridOrigen = false;
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void chkDirecto_CheckedChanged(object sender, EventArgs e)
        {
            this.trasladoDirectoInd = this.chkDirecto.Checked;
            this.AssignTrasladoDirecto();
        }

        #endregion

        #region Eventos grilla

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gv_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                        e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = string.Empty;
                if (pi != null)
                {
                    UDT udtProp = (UDT)pi.GetValue(dto, null);
                    udtProp.SetValueFromString(e.Value.ToString());
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        UDT udtProp = (UDT)fi.GetValue(dto);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtn_Doc_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    string colName = this.gvDetailOrigen.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKModal(this.gvDetailOrigen.FocusedRowHandle, colName, origin);
                }
                else
                {
                    string colName = this.gvDetailDestino.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKModal(this.gvDetailDestino.FocusedRowHandle, colName, origin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "editBtn_Doc_ButtonClick"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDetailOrigen_Enter(object sender, EventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.validHeaderOrigen)
                    {
                        this.EnableControls(false);
                        this.LoadParticiones(this.gvDetailOrigen.FocusedRowHandle, true);
                        this.EnableFooter(true);
                    }
                }
                else
                {
                    if (this.validHeaderDestino)
                    {
                        this.EnableControls(false);
                        this.LoadParticiones(this.gvDetailDestino.FocusedRowHandle, true);
                        this.EnableFooter(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "gcDetailOrigen_Enter"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gcDetailOrigen_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    if (this.validHeaderOrigen)
                    {
                        this.gvDetailOrigen.PostEditor();

                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                        {
                            #region Nuevo registro
                            if (this.gvDetailOrigen.ActiveFilterString != string.Empty)
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                            else
                            {
                                this.deleteOP = false;
                                if (this.isValid_Det_Origen)
                                    this.AddNewRow_Det();
                                else
                                {
                                    bool isV = this.ValidateRow_Det();
                                    if (isV)
                                        this.AddNewRow_Det();
                                }
                                int fila = this.gvDetailOrigen.FocusedRowHandle;
                                this.gvDetailOrigen.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detListOrigen[fila].NewRowPresup;
                                this.gvDetailOrigen.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detListOrigen[fila].NewRowPresup;
                            }
                            #endregion
                        }
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                        {
                            #region Borrar registro
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                            //Revisa si desea cargar los temporales
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.deleteOP = true;
                                int rowHandle = this.gvDetailOrigen.FocusedRowHandle;

                                if (this.detListOrigen.Count == 1)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                    e.Handled = true;
                                }
                                else
                                {
                                    this.detListOrigen.RemoveAt(rowHandle);
                                    //Si borra el primer registro
                                    if (rowHandle == 0)
                                        this.gvDetailOrigen.FocusedRowHandle = 0;
                                    //Si selecciona el ultimo
                                    else
                                        this.gvDetailOrigen.FocusedRowHandle = rowHandle - 1;

                                    this.gvDetailOrigen.RefreshData();
                                    this.RowIndexChanged_Det(this.gvDetailOrigen.FocusedRowHandle);
                                }
                            }
                            e.Handled = true;
                            #endregion
                        }
                    }
                }
                else
                {
                    if (this.validHeaderDestino)
                    {
                        this.gvDetailDestino.PostEditor();

                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Custom)
                        {
                            #region Nuevo registro
                            if (this.gvDetailDestino.ActiveFilterString != string.Empty)
                                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoAddInFilter));
                            else
                            {
                                this.deleteOP = false;
                                if (this.isValid_Det_Destino)
                                    this.AddNewRow_Det();
                                else
                                {
                                    bool isV = this.ValidateRow_Det();
                                    if (isV)
                                        this.AddNewRow_Det();
                                }
                                int fila = this.gvDetailDestino.FocusedRowHandle;
                                this.gvDetailDestino.Columns[this.unboundPrefix + "CentroCostoID"].OptionsColumn.AllowEdit = this.detListDestino[fila].NewRowPresup;
                                this.gvDetailDestino.Columns[this.unboundPrefix + "LineaPresupuestoID"].OptionsColumn.AllowEdit = this.detListDestino[fila].NewRowPresup;
                            }
                            #endregion
                        }
                        if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
                        {
                            #region Borrar registro
                            string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Delete);
                            string msgDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Delete_Register);

                            //Revisa si desea cargar los temporales
                            if (MessageBox.Show(msgDelete, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.deleteOP = true;
                                int rowHandle = this.gvDetailDestino.FocusedRowHandle;

                                if (this.detListDestino.Count == 1)
                                {
                                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.RowsNeeded));
                                    e.Handled = true;
                                }
                                else
                                {
                                    this.detListDestino.RemoveAt(rowHandle);
                                    //Si borra el primer registro
                                    if (rowHandle == 0)
                                        this.gvDetailDestino.FocusedRowHandle = 0;
                                    //Si selecciona el ultimo
                                    else
                                        this.gvDetailDestino.FocusedRowHandle = rowHandle - 1;

                                    this.gvDetailDestino.RefreshData();
                                    this.RowIndexChanged_Det(this.gvDetailDestino.FocusedRowHandle);
                                }
                            }
                            e.Handled = true;
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "gcDetailOrigen_EmbeddedNavigator_ButtonClick"));
            }
        }

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetailOrigen_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                bool validField = true;
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (this.IsFocusedGridOrigen)
                {
                    int fila = this.gvDetailOrigen.FocusedRowHandle;

                    #region FKs

                    if (fieldName == "LineaPresupuestoID")
                    {
                        validField = _bc.ValidGridCell(this.gvDetailOrigen, string.Empty, e.RowHandle, fieldName, false, false, false, AppMasters.plLineaPresupuesto);
                        DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, e.Value.ToString(), true);
                        this.detListOrigen[fila].LineaPresDesc.Value = validField ? lineaPres.Descriptivo.Value : string.Empty;
                    }
                    if (fieldName == "CentroCostoID")
                    {
                        validField = _bc.ValidGridCell(this.gvDetailOrigen, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                        DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, e.Value.ToString(), true);
                        this.detListOrigen[fila].CentroCostoDesc.Value = validField ? centroCto.Descriptivo.Value : string.Empty;
                    }
                    #endregion
                    #region Valores

                    //Movimiento Mda Local
                    if (fieldName == "VlrMvtoLocal")
                    {
                        this.detListOrigen[fila].VlrMvtoLocal.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this.detListOrigen[fila].VlrNuevoSaldoLoc.Value = this.detListOrigen[fila].VlrSaldoAntLoc.Value + this.detListOrigen[fila].VlrMvtoLocal.Value;
                        this.detListOrigen[fila].LoadParticionLocalInd = this.detListOrigen[fila].VlrMvtoLocal.Value != 0 ? false : true;

                        validField = _bc.ValidGridCellValue(this.gvDetailOrigen, string.Empty, e.RowHandle, fieldName, false, true, false, false);

                        if (validField)
                        {
                            this.LoadParticiones(this.gvDetailOrigen.FocusedRowHandle, false, TipoMoneda.Local);
                            this.EnableControls(false);
                            this.EnableFooter(true);
                            this.gcDetailOrigen.RefreshDataSource();
                            this.AssignTrasladoDirecto();
                        }

                    }
                    //Movimiento Mda Extr
                    if (fieldName == "VlrMvtoExtr")
                    {
                        this.detListOrigen[fila].VlrMvtoExtr.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this.detListOrigen[fila].VlrNuevoSaldoExtr.Value = this.detListOrigen[fila].VlrSaldoAntExtr.Value + this.detListOrigen[fila].VlrMvtoExtr.Value;
                        this.detListOrigen[fila].LoadParticionExtrInd = this.detListOrigen[fila].VlrMvtoExtr.Value != 0 ? false : true;

                        validField = _bc.ValidGridCellValue(this.gvDetailOrigen, string.Empty, e.RowHandle, fieldName, false, true, false, false);
                        if (validField)
                        {
                            this.LoadParticiones(this.gvDetailOrigen.FocusedRowHandle, false, TipoMoneda.Foreign);
                            this.EnableControls(false);
                            this.EnableFooter(true);
                            this.gcDetailOrigen.RefreshDataSource();
                            this.AssignTrasladoDirecto();
                        }
                    }


                    #endregion
                    if (!validField)
                        this.isValid_Det_Origen = false;
                }
                else
                {
                    int fila = this.gvDetailDestino.FocusedRowHandle;

                    #region FKs

                    if (fieldName == "LineaPresupuestoID")
                    {
                        validField = _bc.ValidGridCell(this.gvDetailDestino, string.Empty, e.RowHandle, fieldName, false, false, false, AppMasters.plLineaPresupuesto);
                        DTO_plLineaPresupuesto lineaPres = (DTO_plLineaPresupuesto)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.plLineaPresupuesto, false, e.Value.ToString(), true);
                        this.detListDestino[fila].LineaPresDesc.Value = validField ? lineaPres.Descriptivo.Value : string.Empty;
                    }
                    if (fieldName == "CentroCostoID")
                    {
                        validField = _bc.ValidGridCell(this.gvDetailDestino, string.Empty, e.RowHandle, fieldName, false, true, true, AppMasters.coCentroCosto);
                        DTO_coCentroCosto centroCto = (DTO_coCentroCosto)this._bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coCentroCosto, false, e.Value.ToString(), true);
                        this.detListDestino[fila].CentroCostoDesc.Value = validField ? centroCto.Descriptivo.Value : string.Empty;
                    }
                    #endregion
                    #region Valores

                    //Movimiento Mda Local
                    if (fieldName == "VlrMvtoLocal")
                    {
                        this.detListDestino[fila].VlrMvtoLocal.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this.detListDestino[fila].VlrNuevoSaldoLoc.Value = this.detListDestino[fila].VlrSaldoAntLoc.Value + this.detListDestino[fila].VlrMvtoLocal.Value;
                        this.detListDestino[fila].LoadParticionLocalInd = this.detListDestino[fila].VlrMvtoLocal.Value != 0 ? false : true;

                        validField = _bc.ValidGridCellValue(this.gvDetailDestino, string.Empty, e.RowHandle, fieldName, false, true, false, false);
                        if (validField)
                        {
                            this.LoadParticiones(this.gvDetailDestino.FocusedRowHandle, false, TipoMoneda.Local);
                            this.EnableControls(false);
                            this.EnableFooter(true);
                            this.gcDetailDestino.RefreshDataSource();
                            this.AssignTrasladoDirecto();
                        }
                    }
                    //Movimiento Mda Extr
                    if (fieldName == "VlrMvtoExtr")
                    {
                        this.detListDestino[fila].VlrMvtoExtr.Value = Convert.ToDecimal(e.Value, CultureInfo.InvariantCulture);
                        this.detListDestino[fila].VlrNuevoSaldoExtr.Value = this.detListDestino[fila].VlrSaldoAntExtr.Value + this.detListDestino[fila].VlrMvtoExtr.Value;
                        this.detListDestino[fila].LoadParticionExtrInd = this.detListDestino[fila].VlrMvtoExtr.Value != 0 ? false : true;

                        validField = _bc.ValidGridCellValue(this.gvDetailDestino, string.Empty, e.RowHandle, fieldName, false, true, false, false);
                        if (validField)
                        {
                            this.LoadParticiones(this.gvDetailDestino.FocusedRowHandle, false, TipoMoneda.Foreign);
                            this.EnableControls(false);
                            this.EnableFooter(true);
                            this.gcDetailDestino.DataSource = this.detListDestino;
                            this.gcDetailDestino.RefreshDataSource();
                            this.AssignTrasladoDirecto();
                        }
                    }


                    #endregion
                    if (!validField)
                        this.isValid_Det_Destino = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "gvDocument_CellValueChanged"));
            }
        }

        /// <summary>
        /// Se realiza cuando se digita una tecla teniendo en cuenta la columna
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetailOrigen_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    string colName = this.gvDetailOrigen.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                    if (this.gvDetailOrigen.DataRowCount > 0 && this.gvDetailOrigen.IsLastRow && colName.Equals("DescripTExt") &&
                        e.KeyCode == Keys.Tab && this.documentID == AppDocuments.Presupuesto)
                    {
                        bool isV = this.ValidateRow_Det();
                        if (isV)
                            this.AddNewRow_Det();
                    }
                }
                else
                {
                    string colName = this.gvDetailDestino.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);
                    if (this.gvDetailDestino.DataRowCount > 0 && this.gvDetailDestino.IsLastRow && colName.Equals("DescripTExt") &&
                        e.KeyCode == Keys.Tab && this.documentID == AppDocuments.Presupuesto)
                    {
                        bool isV = this.ValidateRow_Det();
                        if (isV)
                            this.AddNewRow_Det();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentPresupuesto.cs", "gvDetail_KeyUp"));
            }
        }

        /// <summary>
        /// Valida los datos de la fila antes de cambiar el foco
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetailOrigen_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (this.IsFocusedGridOrigen)
            {
                if (!this.disableValidate_Det_Origen)
                {
                    bool validRow = this.deleteOP ? true : this.ValidateRow_Det();
                    this.deleteOP = false;

                    if (validRow)
                        this.isValid_Det_Origen = true;
                    else
                    {
                        e.Allow = false;
                        this.isValid_Det_Origen = false;
                    }
                }
            }
            else
            {
                if (!this.disableValidate_Det_Destino)
                {
                    bool validRow = this.deleteOP ? true : this.ValidateRow_Det();
                    this.deleteOP = false;

                    if (validRow)
                        this.isValid_Det_Destino = true;
                    else
                    {
                        e.Allow = false;
                        this.isValid_Det_Destino = false;
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se presenta al seleccionar una fila de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDetailOrigen_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RowIndexChanged_Det(e.FocusedRowHandle);
        }

        /// <summary>
        /// Ocurre al presionar un botos para traer una FK
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void editBtn_Det_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (this.IsFocusedGridOrigen)
                {
                    string colName = this.gvDetailOrigen.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKModal(this.gvDetailOrigen.FocusedRowHandle, colName, origin);
                }
                else
                {
                    string colName = this.gvDetailDestino.FocusedColumn.FieldName.Substring(this.unboundPrefix.Length);

                    ButtonEdit origin = (ButtonEdit)sender;
                    this.ShowFKModal(this.gvDetailDestino.FocusedRowHandle, colName, origin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "editBtn_Det_ButtonClick"));
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocuments_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
                if (fieldName == "DescripTExt")
                    e.RepositoryItem = this.riPopup;
            }
            catch (Exception ex)
            {
                throw ex;
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
                FormProvider.Master.Form_Enter(this, this.documentID, this._frmType, this._frmModule);

                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;

                if (this.documentID != AppDocuments.Presupuesto)
                {
                    FormProvider.Master.itemGenerateTemplate.Visible = false;
                    FormProvider.Master.itemImport.Visible = false;
                }

                if (FormProvider.Master.LoadFormTB)
                {
                    //Deshabilitados
                    //FormProvider.Master.itemFilter.Enabled = false;
                    //FormProvider.Master.itemFilterDef.Enabled = false;
                    FormProvider.Master.itemGenerateTemplate.Enabled = false;

                    FormProvider.Master.itemImport.Enabled = false;
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Add);
                    FormProvider.Master.itemGenerateTemplate.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.GenerateTemplate);
                    FormProvider.Master.itemImport.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Import);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void Form_Leave(object sender, EventArgs e)
        {
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "TrasladoPresupuesto.cs-Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para crear un nuevo presupuesto desde cero
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.detListOrigen = new List<DTO_plPresupuestoDeta>();
                this.detListDestino = new List<DTO_plPresupuestoDeta>();
                this.EnableControls(true);
                this.validHeaderOrigen = false;
                this.validHeaderDestino = false;
                this.initData = false;
                this.deleteOP = false;
                this.isValid_Det_Origen = true;
                this.isValid_Det_Destino = true;
                this.disableValidate_Det_Origen = false;
                this.disableValidate_Det_Destino = false;
                this.masterProyectoDestino.EnableControl(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "TBDelete"));
            }
        }

        /// <summary>
        /// Boton para guardar
        /// </summary>
        public override void TBSave()
        {
            try
            {
                this.gvDetailOrigen.PostEditor();
                this.gvDetailOrigen.Focus();
                this.gvDetailDestino.PostEditor();
                this.gvDetailDestino.Focus();
                if (this.detListOrigen.Count > 0 && this.detListDestino.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    #region Recorre ambas grillas para ver que existan valores digitados
                    foreach (var item in this.detListOrigen)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    foreach (var item in this.detListDestino)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    #endregion
                    #region Valida los datos antes de guardar
                    if (isValid)
                    {
                        decimal vlrMvtoLocalOrigen = this.detListOrigen.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtrOrigen = this.detListOrigen.Sum(x => x.VlrMvtoExtr.Value.Value);
                        decimal vlrMvtoLocalDestino = this.detListDestino.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtrDestino = this.detListDestino.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (Math.Abs(vlrMvtoLocalOrigen + vlrMvtoLocalDestino) == 0 && Math.Abs(vlrMvtoExtrOrigen + vlrMvtoExtrDestino) == 0)
                        {
                            Thread process = new Thread(this.SaveThread);
                            process.Start();
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoMvtoInvalid));
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                this.gvDetailOrigen.PostEditor();
                this.gvDetailOrigen.Focus();
                this.gvDetailDestino.PostEditor();
                this.gvDetailDestino.Focus();
                if (this.detListOrigen.Count > 0 && this.detListDestino.Count > 0)
                {
                    bool isValid = this.ValidateRow_Det();
                    #region Recorre ambas grillas para ver que existan valores digitados
                    foreach (var item in this.detListOrigen)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    foreach (var item in this.detListDestino)
                    {
                        isValid = item.VlrMvtoLocal.Value != 0 || item.VlrMvtoExtr.Value != 0 ? true : false;
                        if (isValid) break;
                    }
                    #endregion
                    #region Valida los datos antes de guardar
                    if (isValid)
                    {
                        decimal vlrMvtoLocalOrigen = this.detListOrigen.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtrOrigen = this.detListOrigen.Sum(x => x.VlrMvtoExtr.Value.Value);
                        decimal vlrMvtoLocalDestino = this.detListDestino.Sum(x => x.VlrMvtoLocal.Value.Value);
                        decimal vlrMvtoExtrDestino = this.detListDestino.Sum(x => x.VlrMvtoExtr.Value.Value);
                        if (Math.Abs(vlrMvtoLocalOrigen + vlrMvtoLocalDestino) == 0 && Math.Abs(vlrMvtoExtrOrigen + vlrMvtoExtrDestino) == 0)
                        {
                            Thread process = new Thread(this.SendToApproveThread);
                            process.Start();
                        }
                        else
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.pl_SaldoMvtoInvalid));
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "TBSendtoAppr"));
            }
        }

        /// <summary>
        /// Boton para exportar la data actual
        /// </summary>
        public override void TBGenerateTemplate()
        {
            try
            {
                ExcelGenerator excell_app = new ExcelGenerator();

                int row = 1;
                int col = 1;
                string[] cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries);
                for (int colIndex = 0; colIndex < cols.Length; colIndex++)
                {
                    string colName = cols[colIndex];
                    excell_app.AddData(row, col, colName);
                    col++;
                }

                MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SuccessTemplate));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "TBGenerateTemplate"));
            }
        }

        /// <summary>
        /// Boton para importar datos la data actual
        /// </summary>
        public override void TBImport()
        {
            //Revisa que cumple las condiciones
            if (!this.ReplaceDocument())
                return;

            if (this.IsFocusedGridOrigen)
            {
                this.gvDetailOrigen.ActiveFilterString = string.Empty;

                if (this.masterProyectoOrigen.ValidID)
                {
                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                    MessageBox.Show(string.Format(msg, this.masterProyectoOrigen.LabelRsx, this.masterProyectoOrigen.Value));
                    this.masterProyectoOrigen.Focus();
                }
            }
            else
            {
                this.gvDetailDestino.ActiveFilterString = string.Empty;

                if (this.masterProyectoDestino.ValidID)
                {
                    this.pasteRet = CopyPasteExtension.PasteFromClipBoard(this.format);
                    Thread process = new Thread(this.ImportThread);
                    process.Start();
                }
                else
                {
                    string msg = this._bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_CodeInvalid);
                    MessageBox.Show(string.Format(msg, this.masterProyectoDestino.LabelRsx, this.masterProyectoDestino.Value));
                    this.masterProyectoDestino.Focus();
                }
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public void SaveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDetailOrigen.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = null;
                decimal tc = Convert.ToDecimal(this.txtTasaCambioOrigen.EditValue, CultureInfo.InvariantCulture);

                if (this.presupuestoOrigen == null)
                    this.presupuestoOrigen = new DTO_Presupuesto();
                this.presupuestoOrigen.Detalles = this.detListOrigen;
                this.presupuestoOrigen.Detalles.AddRange(this.detListDestino);
                this.numeroDocPresupOr = presupuestoOrigen.DocCtrl.NumeroDoc.Value.Value;
                obj = _bc.AdministrationModel.Presupuesto_Nuevo(this.documentID, this.dtPeriodOrigen.DateTime, this.proyectoIDOrigen, tc, this.presupuestoOrigen, true);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this.actFlujo.seUsuarioID.Value, obj, true, true);
                if (isOK)
                    this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public void SendToApproveThread()
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.OK;
                result.Details = new List<DTO_TxResultDetail>();

                DTO_TxResultDetail rd = new DTO_TxResultDetail();
                rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                rd.line = 1;
                rd.Message = "OK";

                this.gvDetailOrigen.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                object obj = null;
                decimal tc = Convert.ToDecimal(this.txtTasaCambioOrigen.EditValue, CultureInfo.InvariantCulture);

                if (this.presupuestoOrigen == null)
                    this.presupuestoOrigen = new DTO_Presupuesto();
                this.presupuestoOrigen.Detalles = this.detListOrigen;
                this.presupuestoOrigen.Detalles.AddRange(this.detListDestino);
                this.presupuestoOrigen.NumeroDocPresup.Value = this.numeroDocPresupOr;
                obj = _bc.AdministrationModel.Presupuesto_Nuevo(this.documentID, this.dtPeriodOrigen.DateTime, this.proyectoIDOrigen, tc, this.presupuestoOrigen, false);

                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this.actFlujo.seUsuarioID.Value, obj, true, true);
                if (isOK)
                    this.Invoke(this.sendToApproveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-TrasladoPresupuesto.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de importacion
        /// </summary>
        public void ImportThread()
        {
            try
            {
                if (this.pasteRet.Success)
                {
                    var text = pasteRet.MsgResult;
                    string[] lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    #region Variables de función y mensajes de error
                    DTO_TxResult result = new DTO_TxResult();
                    result.Result = ResultValue.OK;
                    result.Details = new List<DTO_TxResultDetail>();
                    //Lista con los dtos a subir y Fks a validas
                    Dictionary<string, string> colNames = new Dictionary<string, string>();
                    Dictionary<string, Object> colVals = new Dictionary<string, Object>();
                    //Mensajes de error
                    string msgInvalidFormat = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidFormat);
                    string msgNoCopyField = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCopyField);
                    string msgIncompleteLine = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.IncompleteLine);
                    DTO_plPresupuestoDeta presupuestoDet = null;
                    this.detListOrigen = new List<DTO_plPresupuestoDeta>();
                    bool createDTO = true;
                    bool validList = true;

                    #endregion
                    #region Llena las listas de las columnas
                    List<string> cols = this.format.Split(new string[] { this.formatSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    List<PropertyInfo> pisSupplMig = typeof(DTO_plPresupuestoDeta).GetProperties().ToList();

                    //Recorre el DTO de migracion y revisa el nombre real de la columna
                    foreach (PropertyInfo pi in pisSupplMig)
                    {
                        if (!Attribute.IsDefined(pi, typeof(NotImportable)))
                        {
                            string colRsx = this._bc.GetResource(LanguageTypes.Forms, AppDocuments.Presupuesto + "_" + pi.Name);
                            for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                            {
                                if (cols[colIndex] == colRsx)
                                {
                                    colVals.Add(colRsx, string.Empty);
                                    colNames.Add(colRsx, pi.Name);
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                    #region Llena información para enviar a la grilla (lee filas)
                    FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ReadRows) });
                    int percent = 0;
                    for (int i = 0; i < lines.Length; ++i)
                    {
                        #region Aumenta el porcentaje y revisa que tenga lineas para leer
                        FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                        percent = ((i + 1) * 100) / (lines.Length);

                        if (FormProvider.Master.ProcessCanceled(this.documentID))
                        {
                            result.Details = new List<DTO_TxResultDetail>();
                            result.ResultMessage = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ActionCancelUser);
                            result.Result = ResultValue.NOK;
                            break;
                        }

                        if (lines.Length == 1)
                        {
                            result.ResultMessage = msgNoCopyField;
                            result.Result = ResultValue.NOK;
                            validList = false;
                        }
                        #endregion
                        #region Recorre todas las columnas y verifica que tengan datos validos
                        string[] line = lines[i].Split(new string[] { CopyPasteExtension.tabChar }, StringSplitOptions.None);
                        if (i > 0 && line.Length > 0)
                        {
                            createDTO = true;

                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i;
                            rd.Message = "OK";

                            #region Info básica
                            //Llena los valores de las columnas (manda error si el numero de columnas al importar es menor al necesario)
                            if (line.Length < cols.Count)
                            {
                                result.Result = ResultValue.NOK;
                                DTO_TxResultDetail rdL = new DTO_TxResultDetail();
                                rdL.line = i;
                                rdL.Message = msgIncompleteLine;
                                result.Details.Add(rdL);

                                createDTO = false;
                                validList = false;
                                continue;
                            }
                            else
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    colVals[colRsx] = line[colIndex];
                                }
                            }

                            #endregion
                            #region Creacion de DTO y validacion Formatos
                            presupuestoDet = new DTO_plPresupuestoDeta(true);
                            if (createDTO)
                            {
                                for (int colIndex = 0; colIndex < cols.Count; colIndex++)
                                {
                                    string colRsx = cols[colIndex];
                                    try
                                    {
                                        string colName = colNames[colRsx];
                                        string colValue = colVals[colRsx].ToString().Trim();

                                        #region Validacion Formatos
                                        UDT udt;
                                        PropertyInfo pi = presupuestoDet.GetType().GetProperty(colName);
                                        udt = pi != null ? (UDT)pi.GetValue(presupuestoDet, null) : null;
                                        PropertyInfo piUDT = udt.GetType().GetProperty("Value");

                                        #region Comprueba los valores solo para los booleanos
                                        if (piUDT.PropertyType.Equals(typeof(bool)) || piUDT.PropertyType.Equals(typeof(Nullable<bool>)))
                                        {
                                            string colVal = "false";
                                            if (colValue.Trim() != string.Empty)
                                            {
                                                colVal = "true";
                                                if (colValue.ToLower() != "x")
                                                {
                                                    DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                    rdF.Field = colRsx;
                                                    rdF.Message = msgInvalidFormat + " (x)";
                                                    rd.DetailsFields.Add(rdF);

                                                    createDTO = false;
                                                }
                                            }
                                            colValue = colVal;
                                            colVals[colRsx] = colVal;
                                        }
                                        #endregion
                                        else
                                        {
                                            if (colValue != string.Empty)
                                            {
                                                #region Valores de Fecha
                                                if (piUDT.PropertyType.Equals(typeof(DateTime)) || piUDT.PropertyType.Equals(typeof(Nullable<DateTime>)))
                                                {
                                                    try
                                                    {
                                                        DateTime val = DateTime.ParseExact(colValue, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDate);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                                #region Valores Numericos
                                                else if (piUDT.PropertyType.Equals(typeof(int)) || piUDT.PropertyType.Equals(typeof(Nullable<int>)))
                                                {
                                                    try
                                                    {
                                                        int val = Convert.ToInt32(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInvalidNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(long)) || piUDT.PropertyType.Equals(typeof(Nullable<long>)))
                                                {
                                                    try
                                                    {
                                                        long val = Convert.ToInt64(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatInteger);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(short)) || piUDT.PropertyType.Equals(typeof(Nullable<short>)))
                                                {
                                                    try
                                                    {
                                                        short val = Convert.ToInt16(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatLimitNumber);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(byte)) || piUDT.PropertyType.Equals(typeof(Nullable<byte>)))
                                                {
                                                    try
                                                    {
                                                        byte val = Convert.ToByte(colValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatNumberRange);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                else if (piUDT.PropertyType.Equals(typeof(decimal)) || piUDT.PropertyType.Equals(typeof(Nullable<decimal>)))
                                                {
                                                    try
                                                    {
                                                        decimal val = Convert.ToDecimal(colValue, CultureInfo.InvariantCulture);
                                                        if (colValue.Trim().Contains(','))
                                                        {
                                                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                            rdF.Field = colRsx;
                                                            rdF.Message = msgInvalidFormat + _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                            rd.DetailsFields.Add(rdF);

                                                            createDTO = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                                        rdF.Field = colRsx;
                                                        rdF.Message = msgInvalidFormat + this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FormatDecimal);
                                                        rd.DetailsFields.Add(rdF);

                                                        createDTO = false;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        //Asigna el valor al DTO
                                        udt.ColRsx = colRsx;
                                        if (createDTO && !string.IsNullOrWhiteSpace(colValue))
                                            udt.SetValueFromString(colValue);
                                        #endregion
                                    }
                                    catch (Exception ex1)
                                    {
                                        DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                                        rdF.Field = colRsx;
                                        rdF.Message = this._bc.GetResourceForException(ex1, "WinApp", "MigracionNewFact.cs - Creacion de DTO y validacion Formatos");
                                        rd.DetailsFields.Add(rdF);
                                        createDTO = false;
                                    }
                                }
                            }
                            #endregion
                            #region Carga la informacion de los resultados
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "NOK";
                                result.Result = ResultValue.NOK;
                                createDTO = false;
                            }

                            if (createDTO)
                                this.detListOrigen.Add(presupuestoDet);
                            else
                                validList = false;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                    #region Valida las restricciones particulares
                    if (validList)
                    {
                        result = new DTO_TxResult();
                        result.Result = ResultValue.OK;
                        result.Details = new List<DTO_TxResultDetail>();

                        int i = 0;
                        percent = 0;

                        for (int index = 0; index < this.detListOrigen.Count; ++index)
                        {
                            #region Variables
                            DTO_TxResultDetail rd = new DTO_TxResultDetail();
                            DTO_TxResultDetailFields rdF = new DTO_TxResultDetailFields();
                            rd.DetailsFields = new List<DTO_TxResultDetailFields>();
                            rd.line = i + 1;
                            rd.Message = "OK";
                            #endregion
                            #region Aumenta el porcentaje y revisa que tenga lineas para leer
                            FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, percent });
                            percent = ((i + 1) * 100) / (this.detListOrigen.Count);
                            i++;
                            #endregion
                            presupuestoDet = this.detListOrigen[index];
                            #region Valida y Asigna Totales Ambas Monedas
                            //Total del movimiento(Local)
                            decimal TotalMdaLocal = Math.Round(presupuestoDet.ValorLoc00.Value.Value + presupuestoDet.ValorLoc01.Value.Value + presupuestoDet.ValorLoc02.Value.Value + presupuestoDet.ValorLoc03.Value.Value
                                                      + presupuestoDet.ValorLoc04.Value.Value + presupuestoDet.ValorLoc05.Value.Value + presupuestoDet.ValorLoc06.Value.Value + presupuestoDet.ValorLoc07.Value.Value + presupuestoDet.ValorLoc08.Value.Value
                                                      + presupuestoDet.ValorLoc09.Value.Value + presupuestoDet.ValorLoc10.Value.Value + presupuestoDet.ValorLoc11.Value.Value + presupuestoDet.ValorLoc12.Value.Value);
                            if (TotalMdaLocal != 0)
                            {
                                presupuestoDet.VlrMvtoLocal.Value = TotalMdaLocal;
                                presupuestoDet.LoadParticionLocalInd = false;
                            }

                            if (this.loadME)
                            {
                                //Total del movimiento(Extr)
                                decimal TotalMdaExtr = Math.Round(presupuestoDet.ValorExt00.Value.Value + presupuestoDet.ValorExt01.Value.Value + presupuestoDet.ValorExt02.Value.Value + presupuestoDet.ValorExt03.Value.Value
                                                        + presupuestoDet.ValorExt04.Value.Value + presupuestoDet.ValorExt05.Value.Value + presupuestoDet.ValorExt06.Value.Value + presupuestoDet.ValorExt07.Value.Value + presupuestoDet.ValorExt08.Value.Value
                                                        + presupuestoDet.ValorExt09.Value.Value + presupuestoDet.ValorExt10.Value.Value + presupuestoDet.ValorExt11.Value.Value + presupuestoDet.ValorExt12.Value.Value);
                                if (TotalMdaExtr != 0)
                                {
                                    presupuestoDet.VlrMvtoExtr.Value = TotalMdaExtr;
                                    presupuestoDet.LoadParticionExtrInd = false;
                                }
                            }

                            presupuestoDet.VlrNuevoSaldoLoc.Value = presupuestoDet.VlrSaldoAntLoc.Value + presupuestoDet.VlrMvtoLocal.Value;
                            presupuestoDet.VlrNuevoSaldoExtr.Value = presupuestoDet.VlrSaldoAntExtr.Value + presupuestoDet.VlrMvtoExtr.Value;
                            presupuestoDet.Consecutivo.Value = index + 1;
                            #endregion
                            #region Valida consistencia datos
                            this.ValidateDataImport(presupuestoDet, rd);
                            if (rd.DetailsFields.Count > 0)
                            {
                                result.Details.Add(rd);
                                rd.Message = "Detalle NOK";
                                result.Result = ResultValue.NOK;
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #region Actualiza la información de la grilla
                    if (result.Result == ResultValue.OK)
                    {
                        MessageForm frm = new MessageForm(result);
                        if (result.Result.Equals(ResultValue.OK))
                            this.Invoke(this.refreshDataDelegate);

                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                        this.detListOrigen = new List<DTO_plPresupuestoDeta>();
                    }
                    FormProvider.Master.Invoke(FormProvider.Master.UpdateProgressDelegate, new object[] { this.documentID, 100 });
                    #endregion
                }
                else
                {
                    MessageForm frm = new MessageForm(pasteRet.MsgResult, MessageType.Error);
                    this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(_bc.GetResourceForException(e, "WinApp-DocumentAuxiliarForm.cs", "ImportThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion
    }
}
