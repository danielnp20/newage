using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
using SentenceTransformer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RenovacionPoliza : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        //Para manejo de propiedades
        private FormTypes frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string frmName;
        private int documentID;
        private ModulesPrefix frmModule;
        private DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();
        private bool isModalFormOpened;

        //Variables de operación
        private DTO_ccPolizaEstado _polizaEstado = null;
        private DTO_ccAseguradora _aseguradora = null;
        private List<DTO_ccCreditoDocu> _listCreditos = new List<DTO_ccCreditoDocu>();
        private DTO_ccCreditoDocu _credito = null;
        private int? _numDocCredito = 0;
        private byte _tipoMvto = 1;
        private bool _cleanCombos = true;

        private string terceroID = string.Empty;
        private string aseguradoraID = string.Empty;
        private string segurosAsesorID = string.Empty;
        private string poliza = string.Empty;

        private string rsxActiva = string.Empty;
        private string rsxAnulada = string.Empty;
        private string rsxRevocada = string.Empty;

        #endregion

        #region Constructor

        public RenovacionPoliza()
        {
            this.Constructor(null);
        }

        public RenovacionPoliza(string mod) 
        {
            this.Constructor(mod);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string mod = null)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this.frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this.frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);

                FormProvider.Master.Form_Load(this, this.frmModule, this.documentID, this.frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

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
                    this.actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "RenovacionPoliza"));
            }
        }

        #endregion

        #region Funciones privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.RenovarPolizaGral; 
                this.frmModule = ModulesPrefix.cc;

                //Carga la maestra de componentes de cartera
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                filtros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "PolizaInd",
                    OperadorFiltro = OperadorFiltro.Igual,
                    ValorFiltro = true.ToString(),
                    OperadorSentencia = OperadorSentencia.Or
                });                

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, true);
                this._bc.InitMasterUC(this.masterAseguradora, AppMasters.ccAseguradora, true, true, true, true);
                this._bc.InitMasterUC(this.masterSegurosAsesor, AppMasters.ccSegurosAsesor, true, true, true, true);
                this._bc.InitMasterUC(this.masterComponente, AppMasters.ccCarteraComponente, true, true, true, true, filtros);

                //Estable la fecha con base a la fecha del periodo
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo);
                DateTime periodo = Convert.ToDateTime(strPeriodo);

                if (DateTime.Now.Month != periodo.Month)
                {
                    this.dtFechaDoc.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                    this.dtFechaDoc.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                    this.dtFechaDoc.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                }
                else
                    this.dtFechaDoc.DateTime = DateTime.Now;

                this.dtFechaPoliza.DateTime = DateTime.Now.Date;
                this.dtFechaInicio.DateTime = DateTime.Now.Date;

                Dictionary<byte, string> tipoPoliza = new Dictionary<byte, string>();
                tipoPoliza.Add(1, this._bc.GetResource(LanguageTypes.Tables, "Cobro Juridico"));
                tipoPoliza.Add(2, this._bc.GetResource(LanguageTypes.Tables, "Gastos"));
                this.cmbTipoPoliza.Properties.ValueMember = "Key";
                this.cmbTipoPoliza.Properties.DisplayMember = "Value";
                this.cmbTipoPoliza.Properties.DataSource = tipoPoliza;
                this.cmbTipoPoliza.EditValue = 1; 

                this.EnableFooter(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._listCreditos = new List<DTO_ccCreditoDocu>();

            this.lkpCreditos.Properties.DataSource = this._listCreditos;
            this.masterCliente.Value = String.Empty;
            this.masterAseguradora.Value = string.Empty;
            this.masterSegurosAsesor.Value = string.Empty;
            this.chkFinanciacion.Checked = false;
            this.txtPoliza.Text = string.Empty;
            this.txtEstadoPoliza.Text = string.Empty;
            this.txtValor.EditValue = 0;
            this._polizaEstado = null;
            this._numDocCredito = 0;

            this.dtFechaPago.DateTime = DateTime.Now.Date;
            this.dtFechaPoliza.DateTime = DateTime.Now.Date;
            this.dtFechaInicio.DateTime = DateTime.Now.Date;

            this.terceroID = string.Empty;
            this.aseguradoraID = string.Empty;
            this.segurosAsesorID = string.Empty;
            this.poliza = string.Empty;

            this._aseguradora = null;
            this._polizaEstado = null;
            FormProvider.Master.itemDelete.Enabled = false;

            this.EnableHeader(true);
            this.EnableFooter(false);
        }

        /// <summary>
        /// Habilita y deshabilita el cabezote (PK)
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableHeader(bool enabled)
        {
            //this.masterCliente.EnableControl(enabled);
            //this.txtPoliza.Enabled = enabled;
            //this.lkpSolicitudes.Enabled = enabled;
            //this.lkpCreditos.Enabled = enabled;
        }

        /// <summary>
        /// Habilita y deshabilita el cabezote (PK)
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableFooter(bool enabled)
        {
            this.txtPoliza.Enabled = enabled;
            this.masterAseguradora.EnableControl(enabled);
            this.masterSegurosAsesor.EnableControl(enabled);
            this.dtFechaInicio.Enabled = enabled;
            this.dtFechaPago.Enabled = enabled;
            this.dtFechaPoliza.Enabled = enabled;
            this.dtFechaTermina.Enabled = enabled;
            this.txtValor.Enabled = enabled;
            this.txtNCRevoca.Enabled = enabled;
            this.chkFinanciacion.Enabled = false;

        }

        /// <summary>
        /// Carga la info dela poliza 
        /// </summary>
        private void LoadPoliza(DTO_ccPolizaEstado exist)
        {
            try
            {
                this._listCreditos = new List<DTO_ccCreditoDocu>();
                if (this.masterCliente.ValidID)
                {
                    #region Trae la Poliza si existe
                    this._polizaEstado = exist;

                    this.aseguradoraID = this._polizaEstado.AseguradoraID.Value;
                    this._aseguradora = (DTO_ccAseguradora)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAseguradora, false, this.aseguradoraID, true);

                    this.masterAseguradora.Value = this._polizaEstado.AseguradoraID.Value;
                    this.masterSegurosAsesor.Value = this._polizaEstado.SegurosAsesorID.Value;
                    if (this._polizaEstado.FinanciadaIND.Value.HasValue)
                        this.chkFinanciacion.Checked = this._polizaEstado.FinanciadaIND.Value.Value;
                    this.txtNCRevoca.Text = this._polizaEstado.NCRevoca.Value;
                    this.txtPoliza.Text = this._polizaEstado.Poliza.Value;

                    this.dtFechaPoliza.DateTime = this._polizaEstado.FechaLiqSeguro.Value.Value;
                    this.dtFechaInicio.DateTime = this._polizaEstado.FechaVigenciaINI.Value.Value;
                    this.dtFechaTermina.DateTime = this._polizaEstado.FechaVigenciaFIN.Value.Value;
                    if (this._polizaEstado.FechaPagoSeguro.Value.HasValue)
                        this.dtFechaPago.DateTime = this._polizaEstado.FechaPagoSeguro.Value.Value;

                    //Estado
                    if (this._polizaEstado.AnuladaIND.Value.HasValue && this._polizaEstado.AnuladaIND.Value.Value)
                        this.txtEstadoPoliza.Text = this.rsxAnulada;
                    else if (this._polizaEstado.NumDocPagoRevoca.Value.HasValue)
                        this.txtEstadoPoliza.Text = this.rsxRevocada;
                    else
                        this.txtEstadoPoliza.Text = this.rsxActiva;

                    this.txtValor.EditValue = this._polizaEstado.VlrPoliza.Value;
                    this._polizaEstado.NumDocCredito.Value = this._credito.NumeroDoc.Value;

                    #endregion
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "LoadPoliza"));
            }
        }

        /// <summary>
        /// Valida el cabezote
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            #region Valida datos en la maestra de Tercero
            if (!this.masterCliente.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.CodeRsx);

                MessageBox.Show(msg);
                this.masterCliente.Focus();

                return false;
            }
            #endregion
            #region Valida datos en Poliza
            if (string.IsNullOrEmpty(this.lkpCreditos.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblLibranza.Text);

                MessageBox.Show(msg);
                this.lkpCreditos.Focus();
                return false;
            }
            #endregion
            #region Valida datos en la maestra de Aseguradora
            if (!this.masterAseguradora.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAseguradora.CodeRsx);

                MessageBox.Show(msg);
                this.masterAseguradora.Focus();

                return false;
            }
            #endregion            
            #region Valida datos en Poliza
            if (string.IsNullOrEmpty(this.txtPoliza.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblPoliza.Text);

                MessageBox.Show(msg);
                this.txtPoliza.Focus();

                return false;
            }
            #endregion
            #region Valida Valor
            if (Convert.ToDecimal(this.txtValor.EditValue) == 0)
            {
                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ts_ValorATrasladarInvalido));
                this.txtValor.Focus();
                return false;
            }
            #endregion
            #region Valida Fechas Vigencia
            if (this.dtFechaInicio.DateTime > this.dtFechaTermina.DateTime)
            {
                string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_FechaVigenciaInvalid), this.lblFechaTermina.Text, this.lblInicioVigencia.Text);

                MessageBox.Show(msg);
                this.dtFechaTermina.Focus();
            }
            #endregion              
            return true;
        }
        
        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Enter(this, this.documentID, this.frmType, this.frmModule);
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemDelete.Visible = true;

                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                    FormProvider.Master.itemDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
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
                FormProvider.Master.Form_Closing(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this.frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al momento de salir del componente de cartera
        /// </summary>
        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                this._listCreditos = new List<DTO_ccCreditoDocu>();
                if (this.masterCliente.ValidID)
                {
                    this._listCreditos = _bc.AdministrationModel.GetCreditoByCliente(this.masterCliente.Value);
                    this._listCreditos = this._listCreditos.FindAll(x => x.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico || x.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago || x.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido);
                    
                    if (this._listCreditos.Count == 0)
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages,"No existe libranzas del cliente con estado permitido para renovar");
                        MessageBox.Show(msg);
                        this.CleanData();
                    }
                    else
                        this.lkpCreditos.Properties.DataSource = this._listCreditos;
                }
                else
                    this.lkpCreditos.Properties.DataSource = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Desistimiento.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtPoliza_Leave(object sender, EventArgs e)
        {
                         
        }

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lkpCreditos_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.lkpCreditos.Text))
                {
                    this._credito = this._bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this.lkpCreditos.Text));
                    if (this._credito != null)
                    {
                        #region Valida el estado
                        if (this._credito.EstadoDeuda.Value != (byte)EstadoDeuda.Juridico && this._credito.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPago && this._credito.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPagoIncumplido)
                        {
                            MessageBox.Show("Este crédito no se encuentra en el estado permitido para renovar");
                            this._listCreditos.RemoveAll(x => x.Libranza.Value == this._credito.Libranza.Value);
                            this.lkpCreditos.Text = string.Empty;
                            this._credito = null;
                            return;
                        }
                        #endregion

                        DTO_ccPolizaEstado polizaTemp = new DTO_ccPolizaEstado();
                        polizaTemp.TerceroID.Value = this.masterCliente.Value;
                        polizaTemp.AnuladaIND.Value = false;
                        polizaTemp.Poliza.Value = this.txtPoliza.Text;
                        polizaTemp.Libranza.Value = this._credito.Libranza.Value;
                        List<DTO_ccPolizaEstado> polizas = this._bc.AdministrationModel.PolizaEstado_GetByParameter(polizaTemp);
                        polizas = polizas.FindAll(x => x.NumeroDocLiquida.Value == null);
                        if (polizas.Count > 0)
                            this.LoadPoliza(polizas.First());
                        else
                        {
                            MessageBox.Show("Esta oligación no registra ninguna póliza o ya se encuentra liquidada");
                            this.lkpCreditos.Text = string.Empty;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "lkpCreditos_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar una poliza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPoliza_Click(object sender, EventArgs e)
        {
            ModalPolizasCartera cot = new ModalPolizasCartera(this.masterCliente.Value, string.Empty,true);
            cot.ShowDialog();
            if (cot.RowSelected != null)
            {
                if (cot.RowSelected.Libranza.Value.HasValue)
                    this._credito = this._bc.AdministrationModel.GetCreditoByLibranza(cot.RowSelected.Libranza.Value.Value);
                this.masterCliente.Value = cot.RowSelected.TerceroID.Value;
                if (this._credito != null)
                {
                    this._listCreditos.Add(this._credito);
                    this.lkpCreditos.Properties.DataSource = this._listCreditos;
                    this.lkpCreditos.Text = cot.RowSelected.Libranza.Value.ToString();
                }
                else               
                    this.LoadPoliza(cot.RowSelected);
            }          
        }

        /// <summary>
        /// Al cambiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoPoliza_EditValueChanged(object sender, EventArgs e)
        {
            if(Convert.ToInt16(this.cmbTipoPoliza.EditValue) == 1)
            {
                this.masterComponente.Value = string.Empty;
                this.masterComponente.Visible = false;
            }
            else
                this.masterComponente.Visible = true;
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                if (this.ValidateHeader())
                {                   
                    #region Salva la poliza
                    this._polizaEstado.FechaMvto.Value = this.dtFechaDoc.DateTime;
                    DTO_TxResult result = this._bc.AdministrationModel.CobroJuridicoRenovacionPoliza_Add(this.documentID, this._polizaEstado);
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();

                    if (result.Result == ResultValue.OK)
                        this.CleanData();

                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RenovacionPoliza.cs", "TBSave"));
            }
        }
        #endregion

     
    }
}
