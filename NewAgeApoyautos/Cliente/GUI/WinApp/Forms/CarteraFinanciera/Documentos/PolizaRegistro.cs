using DevExpress.XtraEditors;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.Librerias.Project;
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
    public partial class PolizaRegistro : FormWithToolbar
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
        private List<DTO_ccSolicitudDocu> _listSolicitudes = new List<DTO_ccSolicitudDocu>();
        private int? _numDocSolicitud = 0;
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

        public PolizaRegistro()
        {
            this.Constructor(null);
        }

        public PolizaRegistro(string mod) 
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
                    this.masterTercero.EnableControl(false);
                    this.txtPoliza.Enabled = false;
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "PolizaRegistro"));
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
                this.documentID = AppDocuments.PolizasCartera;
                this.frmModule = ModulesPrefix.cc;

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, true);
                this._bc.InitMasterUC(this.masterAseguradora, AppMasters.ccAseguradora, true, true, true, true);
                this._bc.InitMasterUC(this.masterSegurosAsesor, AppMasters.ccSegurosAsesor, true, true, true, true);

                Dictionary<byte, string> dicTipoMvto = new Dictionary<byte, string>();
                dicTipoMvto.Add((byte)EstadoPoliza.NuevaConCredito, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_NuevoConCredito"));
                dicTipoMvto.Add((byte)EstadoPoliza.Renovada, this._bc.GetResource(LanguageTypes.Tables, "Renovación"));
                dicTipoMvto.Add((byte)EstadoPoliza.NuevaSinCredito, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_NuevoSinCredito"));
                dicTipoMvto.Add((byte)EstadoPoliza.PolizaExternaNueva, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_PolizaExternaNueva"));
                dicTipoMvto.Add((byte)EstadoPoliza.PolizaExternaRenovada, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_PolizaExternaRenovac"));
                dicTipoMvto.Add((byte)EstadoPoliza.PolizaPagada, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_PolizaPagada"));
                dicTipoMvto.Add((byte)EstadoPoliza.PolizaGastos, this._bc.GetResource(LanguageTypes.Tables, "tbl_EstadoPoliza_PolizaGastos"));
                this.lkpTipoMvto.Properties.DataSource = dicTipoMvto;
                this.lkpTipoMvto.EditValue = (byte)EstadoPoliza.NuevaConCredito;

                //Recursos
                this.rsxActiva = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Activa");
                this.rsxAnulada = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Anulada");
                this.rsxRevocada = this._bc.GetResource(LanguageTypes.Forms, this.documentID + "_Revocada");

                this.dtFechaPoliza.DateTime = DateTime.Now.Date;
                this.dtFechaInicio.DateTime = DateTime.Now.Date;

                this.dtPeriod.DateTime = Convert.ToDateTime(_bc.GetControlValueByCompany(this.frmModule, AppControl.co_Periodo));
                this.dtPeriod.Enabled = false;

                //Por defecto pone a fecha del día
                int currentMonth = this.dtPeriod.DateTime.Month;
                int currentYear = this.dtPeriod.DateTime.Year;
                this.dtFechaMvto.DateTime = new DateTime(currentYear, currentMonth, 1);
                this.dtFechaMvto.Properties.MinValue = new DateTime(currentYear, currentMonth, 1);
                this.dtFechaMvto.Properties.MaxValue = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));

                this.EnableFooter(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._listSolicitudes = new List<DTO_ccSolicitudDocu>();
            this._listCreditos = new List<DTO_ccCreditoDocu>();

            this.lkpTipoMvto.Enabled = true; 
            this.lkpTipoMvto.EditValue = (byte)EstadoPoliza.NuevaConCredito;
            this.lkpSolicitudes.Properties.DataSource = this._listSolicitudes;
            this.lkpCreditos.Properties.DataSource = this._listCreditos;
            this.masterTercero.Value = String.Empty;
            this.masterAseguradora.Value = string.Empty;
            this.masterSegurosAsesor.Value = string.Empty;
            this.chkFinanciacion.Checked = false;
            this.chkColectivaInd.Checked = false;
            this.chkColectivaInd.Enabled = false;
            this.txtPoliza.Text = string.Empty;
            this.txtEstadoPoliza.Text = string.Empty;
            this.txtValor.EditValue = 0;
            this._polizaEstado = null;
            this._numDocCredito = 0;
            this._numDocSolicitud = 0;

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
            //this.masterTercero.EnableControl(enabled);
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
            this.masterAseguradora.EnableControl(enabled);
            this.masterSegurosAsesor.EnableControl(enabled);
            this.dtFechaInicio.Enabled = enabled;
            this.dtFechaPago.Enabled = enabled;
            this.dtFechaPoliza.Enabled = enabled;
            this.dtFechaTermina.Enabled = enabled;
            this.txtValor.Enabled = enabled;
            this.txtNCRevoca.Enabled = enabled;
            this.chkFinanciacion.Enabled = enabled;

            if(enabled)
            {

                if (this._tipoMvto == (byte)EstadoPoliza.NuevaConCredito || this._tipoMvto == (byte)EstadoPoliza.Renovada)
                {
                    this.chkFinanciacion.Enabled = false;
                    this.chkFinanciacion.Checked = true;
                    this.chkColectivaInd.Enabled = false;
                }
                else if (this._tipoMvto == (byte)EstadoPoliza.PolizaExternaNueva || this._tipoMvto == (byte)EstadoPoliza.PolizaExternaRenovada)
                {
                    this.chkFinanciacion.Enabled = false;
                    this.chkFinanciacion.Checked = false;
                    this.chkColectivaInd.Enabled = true;
                }
                else
                {
                    if (this.lkpCreditos.EditValue != null)
                    {
                        int libranza = Convert.ToInt32(this.lkpCreditos.EditValue);
                        DTO_ccCreditoDocu cred = this._listCreditos.FirstOrDefault(c => c.Libranza.Value == libranza);

                        if (cred != null && (cred.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico
                            || cred.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago
                            || cred.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido))
                        {
                            this.chkFinanciacion.Enabled = false;
                            this.chkFinanciacion.Checked = false;
                        }
                        else
                        {
                            this.chkFinanciacion.Enabled = true;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Carga la info dela poliza 
        /// </summary>
        private void LoadPoliza(DTO_ccPolizaEstado exist)
        {
            try
            {
                this._listSolicitudes = new List<DTO_ccSolicitudDocu>();
                this._listCreditos = new List<DTO_ccCreditoDocu>();
                if (!string.IsNullOrWhiteSpace(this.terceroID) && !string.IsNullOrWhiteSpace(this.poliza))
                {
                    #region Crea filtro y Trae la Poliza si existe

                    DTO_ccPolizaEstado polizaTemp = new DTO_ccPolizaEstado();
                    polizaTemp.TerceroID.Value = this.masterTercero.Value;
                    polizaTemp.Poliza.Value = this.txtPoliza.Text;
                    polizaTemp.AnuladaIND.Value = false;
                    this._polizaEstado = exist;

                    #endregion
                    if (this._polizaEstado != null)
                    {
                        #region Valida y Asigna data
                        this.lkpTipoMvto.EditValue = this._polizaEstado.EstadoPoliza.Value.Value;

                        if (this._tipoMvto == (byte)EstadoPoliza.Renovada)
                        {
                            if (this._polizaEstado.NumDocCredito.Value == null)
                            {
                                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cf_PolizaObligNotExist), this._polizaEstado.Poliza.Value);
                                MessageBox.Show(msg);
                                return;
                            }
                        }

                        this.aseguradoraID = this._polizaEstado.AseguradoraID.Value;
                        this._aseguradora = (DTO_ccAseguradora)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAseguradora, false, this.aseguradoraID, true);
                       
                        this.masterAseguradora.Value = this._polizaEstado.AseguradoraID.Value;
                        this.masterSegurosAsesor.Value = this._polizaEstado.SegurosAsesorID.Value;
                        if (this._polizaEstado.FinanciadaIND.Value.HasValue)
                            this.chkFinanciacion.Checked = this._polizaEstado.FinanciadaIND.Value.Value;
                        if (this._polizaEstado.ColectivaInd.Value.HasValue)
                            this.chkColectivaInd.Checked = this._polizaEstado.ColectivaInd.Value.Value;
                        else
                            this.chkColectivaInd.Checked = false;
                        this.txtNCRevoca.Text = this._polizaEstado.NCRevoca.Value;
                        this.txtPoliza.Text = this._polizaEstado.Poliza.Value;
                       
                        this.dtFechaPoliza.DateTime = this._polizaEstado.FechaLiqSeguro.Value.Value;
                        this.dtFechaInicio.DateTime = this._polizaEstado.FechaVigenciaINI.Value.Value;
                        this.dtFechaTermina.DateTime = this._polizaEstado.FechaVigenciaFIN.Value.Value;
                        this.dtFechaMvto.DateTime = DateTime.Now.Date;
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
                        
                        #endregion
                        #region Asigna Solicitud o Credito

                        this.LoadObligaciones();
                        if (this._tipoMvto == (byte)EstadoPoliza.NuevaConCredito) 
                            this.lkpSolicitudes.EditValue = this._polizaEstado.Solicitud.Value;
                        else if (this._tipoMvto == (byte)EstadoPoliza.Renovada)
                            this.lkpCreditos.EditValue = this._polizaEstado.Libranza.Value;
                        else
                        {
                            this.lkpSolicitudes.EditValue = this._polizaEstado.Solicitud.Value;
                            this.lkpCreditos.EditValue = this._polizaEstado.Libranza.Value;
                        }

                        //Trae la información de la solicitud
                        if (this._listSolicitudes.Count > 0 && this.lkpSolicitudes.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpSolicitudes.EditValue.ToString()))
                        {
                            DTO_ccSolicitudDocu res = this._listSolicitudes.Find(x => x.Libranza.Value == Convert.ToInt32(this.lkpSolicitudes.EditValue));
                            this._numDocSolicitud = null;
                            if (res != null)
                                this._numDocSolicitud = res.NumeroDoc.Value;
                            
                            this._polizaEstado.NumDocSolicitud.Value = this._numDocSolicitud;
                        }

                        //Trae la información del crédito
                        if (this._listCreditos.Count > 0 && this.lkpCreditos.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString()))
                        {
                            DTO_ccCreditoDocu res = this._listCreditos.Find(x => x.Libranza.Value == Convert.ToInt32(this.lkpCreditos.EditValue));
                            this._numDocCredito = null;
                            if (res != null)
                                this._numDocCredito = res.NumeroDoc.Value;
                           
                            this._polizaEstado.NumDocCredito.Value = this._numDocCredito;
                        }

                        #endregion

                        FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Delete);
                        this.EnableHeader(false);
                    }
                    else
                    {
                        this.LoadObligaciones();

                        this._polizaEstado = new DTO_ccPolizaEstado();
                        this._numDocCredito = 0;
                        this._numDocSolicitud = 0;

                        FormProvider.Master.itemDelete.Enabled = false;
                    }

                    //this.lkpTipoMvto.Enabled = false; // Se deshabilita porque los creditos deb=penden del tipo
                    this.EnableFooter(true);
                }
                else
                {
                    this._listSolicitudes.Clear();
                    this._listCreditos.Clear();
                    this._numDocCredito = 0;
                    this._numDocSolicitud = 0;

                    this.lkpSolicitudes.Properties.DataSource = this._listSolicitudes;
                    this.lkpCreditos.Properties.DataSource = this._listCreditos;

                    FormProvider.Master.itemDelete.Enabled = false;
                    this.lkpTipoMvto.Enabled = true;
                    this.EnableFooter(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "LoadPoliza"));
            }
        }

        /// <summary>
        /// Obtiene Solicitudes y Creditos del Tercero
        /// </summary>
        private void LoadObligaciones()
        {
            this.lkpCreditos.EditValue = null;
            this.lkpSolicitudes.EditValue = null;
            this.lkpCreditos.Properties.DataSource = null;
            this.lkpSolicitudes.Properties.DataSource = null;
            if (this._tipoMvto == (byte)EstadoPoliza.NuevaConCredito)
            {
                //Lista de solicitudes
                this._listSolicitudes = this._bc.AdministrationModel.GetSolicitudesByCliente(this.masterTercero.Value);

                List<int?> sol1 = (from p in this._listSolicitudes where p.VlrSolicitado.Value.Value > 0 select p.Libranza.Value).Distinct().ToList();
                this.lkpSolicitudes.Properties.DataSource = sol1;
            }
            else if (this._tipoMvto == (byte)EstadoPoliza.Renovada || this._tipoMvto == (byte)EstadoPoliza.PolizaExternaRenovada)
            {
                //Lista de créditos
                this._listCreditos = this._bc.AdministrationModel.GetCreditoByCliente(this.masterTercero.Value);
                //this._listCreditos = this._listCreditos.Where(c => c.EstadoDeuda.Value != (byte)EstadoDeuda.Juridico
                //    && c.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPago && c.EstadoDeuda.Value != (byte)EstadoDeuda.AcuerdoPagoIncumplido).ToList();

                List<int?> cred1 = (from p in this._listCreditos where !p.CanceladoInd.Value.Value select p.Libranza.Value).Distinct().ToList();
                this.lkpCreditos.Properties.DataSource = cred1;
            }         
            else if (this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito || this._tipoMvto == (byte)EstadoPoliza.PolizaExternaNueva || 
                     this._tipoMvto == (byte)EstadoPoliza.PolizaPagada || this._tipoMvto == (byte)EstadoPoliza.PolizaGastos)
            {
                //Lista de solicitudes
                this._listSolicitudes = this._bc.AdministrationModel.GetSolicitudesByCliente(this.masterTercero.Value);
                List<int?> sol1 = (from p in this._listSolicitudes
                                   where this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito ? p.VlrSolicitado.Value.Value == 0 : p.VlrSolicitado.Value.Value > 0
                                   select p.Libranza.Value).Distinct().ToList();
                this.lkpSolicitudes.Properties.DataSource = sol1;

                ////Lista de créditos
                //this._listCreditos = this._bc.AdministrationModel.GetCreditoByCliente(this.masterTercero.Value);
                //List<int?> cred1 = (from p in this._listCreditos where !p.CanceladoInd.Value.Value select p.Libranza.Value).Distinct().ToList();
                //this.lkpCreditos.Properties.DataSource = cred1;
            }
        }

        /// <summary>
        /// Calcula la fecha de pago
        /// </summary>
        private void LoadFechaPago()
        {
            try 
            {
                int diasPago = this.masterAseguradora.ValidID && this._aseguradora.DiasPago.Value.HasValue ? this._aseguradora.DiasPago.Value.Value : 0;
                this.dtFechaPago.DateTime = this.dtFechaInicio.DateTime.AddDays(diasPago);

                while (this.dtFechaPago.DateTime.DayOfWeek != DayOfWeek.Friday)
                    this.dtFechaPago.DateTime = this.dtFechaPago.DateTime.AddDays(-1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "LoadFechaPago"));
            }
        }

        /// <summary>
        /// Valida el cabezote
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            #region Valida datos en la maestra de Tercero
            if (!this.masterTercero.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTercero.CodeRsx);

                MessageBox.Show(msg);
                this.masterTercero.Focus();

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
            #region Valida datos en la maestra de Seguros Asesor
            if (!this.masterSegurosAsesor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterSegurosAsesor.CodeRsx);

                MessageBox.Show(msg);
                this.masterSegurosAsesor.Focus();

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
            #region Valida datos en Solicitud
            if ((this.lkpSolicitudes.EditValue == null || string.IsNullOrWhiteSpace(this.lkpSolicitudes.EditValue.ToString())) 
                && (this.lkpCreditos.EditValue == null || string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString())))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), (this.lblSolicitud.Text + " u " + this.lblLibranza.Text));
                MessageBox.Show(msg);
                this.lkpSolicitudes.Focus();

                return false;
            }
            #endregion
            #region Valida el tipo de movimiento

            if (this._tipoMvto == (byte)EstadoPoliza.Renovada)
            {
                if (this.lkpCreditos.EditValue == null || string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString()))
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblLibranza.Text);
                    MessageBox.Show(msg);
                    this.lkpCreditos.Focus();

                    return false;
                }
            }
            else if (this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito)
            {
                if (!this.chkFinanciacion.Checked && string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString()))
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblLibranza.Text);
                    MessageBox.Show(msg);
                    this.lkpCreditos.Focus();

                    return false;
                }
            }
            else if (this._tipoMvto == (byte)EstadoPoliza.PolizaExternaNueva)
            {  
                if ((this.lkpCreditos.EditValue == null || string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString())) &&
                   (this.lkpSolicitudes.EditValue == null || string.IsNullOrWhiteSpace(this.lkpSolicitudes.EditValue.ToString())))
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), (this.lblSolicitud.Text + " u " + this.lblLibranza.Text));
                    MessageBox.Show(msg);
                    this.lkpSolicitudes.Focus();

                    return false;
                }
            }

            #endregion

            return true;
        }
        
        /// <summary>
        /// Muestra el formulario modal para una FK
        /// </summary>
        /// <param name="row">Fila donde esta la FK</param>
        /// <param name="col">Columna seleccionada</param>
        /// <param name="be">Boton que ejecuta la accion</param>
        private void ShowFKModal(int row, string col, ButtonEdit be)
        {
            this.isModalFormOpened = true;
            try
            {
                DTO_aplMaestraPropiedades props = _bc.GetMasterPropertyByColId(col);
                string countMethod = "MasterSimple_Count";
                string dataMethod = "MasterSimple_GetPaged";

                string modFrmCode = props.DocumentoID.ToString();
                string modEmpGrupo = _bc.GetMaestraEmpresaGrupoByDocumentID(Convert.ToInt32(modFrmCode));
                Tuple<int, string> tup = new Tuple<int, string>(Convert.ToInt32(modFrmCode), modEmpGrupo);

                DTO_glTabla fktable = _bc.AdministrationModel.Tables[tup];
                if (fktable.Jerarquica.Value.Value)
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, true);
                    modal.ShowDialog();
                }
                else
                {
                    ModalMaster modal = new ModalMaster(be, modFrmCode, countMethod, dataMethod, null, props.ColumnaID, string.Empty, false);
                    modal.ShowDialog();
                }
            }
            finally
            {
                this.isModalFormOpened = false;
            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que se ejecuta al momento de salir del componente de cartera
        /// </summary>
        private void masterTercero_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.masterTercero.ValidID)
                {
                    //Carga la info de poliza estado
                    this.terceroID = this.masterTercero.Value;
                    this.LoadPoliza(null);
                }
                else
                {
                    this.terceroID = string.Empty;
                    this._listSolicitudes.Clear();
                    this._listCreditos.Clear();
                    this._numDocCredito = 0;
                    this._numDocSolicitud = 0;

                    this.lkpSolicitudes.Properties.DataSource = this._listSolicitudes;
                    this.lkpCreditos.Properties.DataSource = this._listCreditos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "masterTercero_Leave"));
            }
        }

        /// <summary>
        /// Evento al salir de la aseguradora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterAseguradora_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.aseguradoraID != this.masterAseguradora.Value)
                {
                    if (this.masterAseguradora.ValidID)
                    {
                        //Carga la info de poliza estado
                        this.aseguradoraID = this.masterAseguradora.Value;
                        this._aseguradora = (DTO_ccAseguradora)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccAseguradora, false, this.aseguradoraID, true);
                        this.LoadFechaPago();
                    }
                    else
                    {
                        this._aseguradora = null;
                        this.dtFechaPago.DateTime = this.dtFechaInicio.DateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "masterAseguradora_Leave"));
            }

        }

        /// <summary>
        /// Evento al salir de la aseguradora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterSegurosAsesor_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito && this.segurosAsesorID != this.masterSegurosAsesor.Value)
                {
                    if (this.masterSegurosAsesor.ValidID)
                    {
                        this.segurosAsesorID = this.masterSegurosAsesor.Value;
                        DTO_ccSegurosAsesor asesor = (DTO_ccSegurosAsesor)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccSegurosAsesor, false, this.segurosAsesorID, true);
                        if (asesor.AsesorInternoInd.Value.Value)
                        {
                            this.chkFinanciacion.Checked = true;
                            this.chkFinanciacion.Enabled = true;
                        }
                        else
                        {
                            this.chkFinanciacion.Checked = false;
                            this.chkFinanciacion.Enabled = false;
                        }
                    }
                    else
                    {
                        this.chkFinanciacion.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "masterAseguradora_Leave"));
            }
        }

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtPoliza_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Name == "txtPoliza")
            {
                #region Cambio la póliza
                if (this.poliza != this.txtPoliza.Text)
                {
                    //Carga la póliza
                    this.poliza = this.txtPoliza.Text;
                    this.LoadPoliza(null);
                }
                #endregion
            }
        }

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lkpTipoMvto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.masterTercero.EnableControl(true);
                this._tipoMvto = Convert.ToByte(this.lkpTipoMvto.EditValue);
                if (this._tipoMvto == (byte)EstadoPoliza.NuevaConCredito)
                {
                    this.lblNCRevoca.Visible = false;
                    this.txtNCRevoca.Visible = false;
                    this.chkFinanciacion.Checked = true;
                    this.chkColectivaInd.Enabled = false;
                    this.lkpCreditos.Enabled = false;
                    this.lkpSolicitudes.Enabled = true;
                }
                else if (this._tipoMvto == (byte)EstadoPoliza.Renovada)
                {
                    this.lblNCRevoca.Visible = false;
                    this.txtNCRevoca.Visible = false;
                    this.chkFinanciacion.Checked = true;
                    this.chkColectivaInd.Enabled = false;
                    this.lkpCreditos.Enabled = true;
                    this.lkpSolicitudes.Enabled = false;
                }
                else if (this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito)
                {
                    this.lblNCRevoca.Visible = false;
                    this.txtNCRevoca.Visible = false;
                    this.chkFinanciacion.Checked = true;
                    this.chkColectivaInd.Enabled = false;
                    this.lkpCreditos.Enabled = true;
                    this.lkpSolicitudes.Enabled = true;
                }
                else if (this._tipoMvto == (byte)EstadoPoliza.PolizaExternaNueva || this._tipoMvto == (byte)EstadoPoliza.PolizaExternaRenovada ||
                         this._tipoMvto == (byte)EstadoPoliza.PolizaPagada || this._tipoMvto == (byte)EstadoPoliza.PolizaGastos)
                {
                    this.lblNCRevoca.Visible = false;
                    this.txtNCRevoca.Visible = false;
                    this.chkFinanciacion.Checked = false;
                    this.chkColectivaInd.Enabled = true;
                    this.lkpCreditos.Enabled = true;
                    this.lkpSolicitudes.Enabled = true;
                }

                if (this.masterTercero.ValidID && !string.IsNullOrEmpty(this.txtPoliza.Text))
                {
                    this.terceroID = this.masterTercero.Value;
                    this.masterTercero.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "cmbTipoMvto_EditValueChanged"));
            }

        }

        /// <summary>
        /// Al salir del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void lkpSolicitudes_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this._cleanCombos && this.lkpCreditos.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString())
                    && this._tipoMvto != (byte)EstadoPoliza.NuevaSinCredito)
                {
                    this._cleanCombos = false;
                    this.lkpCreditos.EditValue = null;
                    this._cleanCombos = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "lkpCreditos_EditValueChanged"));
            }
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
                if (this._cleanCombos && this.lkpSolicitudes.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpSolicitudes.EditValue.ToString())
                    && this._tipoMvto != (byte)EstadoPoliza.NuevaSinCredito)
                {
                    this._cleanCombos = false;
                    this.lkpSolicitudes.EditValue = null;
                    this._cleanCombos = true;
                }

                if (this._tipoMvto == (byte)EstadoPoliza.NuevaSinCredito && this.lkpCreditos.EditValue != null)
                {
                    int libranza = Convert.ToInt32(this.lkpCreditos.EditValue);
                    DTO_ccCreditoDocu cred = this._listCreditos.FirstOrDefault(c => c.Libranza.Value == libranza);

                    if (cred != null && ( cred.EstadoDeuda.Value == (byte)EstadoDeuda.Juridico 
                        || cred.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPago 
                        || cred.EstadoDeuda.Value == (byte)EstadoDeuda.AcuerdoPagoIncumplido))
                    {
                        this.chkFinanciacion.Enabled = false;
                        this.chkFinanciacion.Checked = false;
                    }
                    else
                    { 
                        this.chkFinanciacion.Enabled = true; 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "lkpCreditos_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la fecha de la póliza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaPoliza_EditValueChanged(object sender, EventArgs e)
        {
            try 
            {
            this.dtFechaInicio.DateTime = this.dtFechaPoliza.DateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "dtFechaPoliza_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al cambiar la fecha de inicio de la poliza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFechaInicio_EditValueChanged(object sender, EventArgs e)
        {
            try 
            {
                this.dtFechaTermina.DateTime = this.dtFechaInicio.DateTime.AddMonths(12).AddDays(-1);
                this.LoadFechaPago();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "dtFechaInicio_EditValueChanged"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al seleccionar una poliza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPoliza_Click(object sender, EventArgs e)
        {
            ModalPolizasCartera cot = new ModalPolizasCartera(this.masterTercero.Value, string.Empty);
            cot.ShowDialog();
            if (cot.RowSelected != null)
            {
                this.terceroID = cot.RowSelected.TerceroID.Value;
                this.poliza = cot.RowSelected.Poliza.Value;
                this.masterTercero.Value = cot.RowSelected.TerceroID.Value;
                this.txtPoliza.Text = cot.RowSelected.Poliza.Value;
                this.LoadPoliza(cot.RowSelected);
            }          
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
                this.masterTercero.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "TBNew"));
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
                    #region Carga el DTO de Poliza Estado

                    this._polizaEstado.TerceroID.Value = this.masterTercero.Value;
                    this._polizaEstado.Poliza.Value = this.txtPoliza.Text;
                    this._polizaEstado.EstadoPoliza.Value = Convert.ToByte(this.lkpTipoMvto.EditValue);
                    this._polizaEstado.FechaVigenciaINI.Value = this.dtFechaInicio.DateTime;
                    this._polizaEstado.FechaVigenciaFIN.Value = this.dtFechaTermina.DateTime;
                    this._polizaEstado.FechaPagoSeguro.Value = this.dtFechaPago.DateTime;
                    this._polizaEstado.AseguradoraID.Value = this.masterAseguradora.Value;
                    this._polizaEstado.SegurosAsesorID.Value = this.masterSegurosAsesor.Value;
                    this._polizaEstado.FinanciadaIND.Value = this.chkFinanciacion.Checked;
                    this._polizaEstado.ColectivaInd.Value = this.chkColectivaInd.Checked;
                    this._polizaEstado.FechaMvto.Value = this.dtFechaMvto.DateTime;

                    if (this.lkpSolicitudes.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpSolicitudes.EditValue.ToString()))
                    {
                        DTO_ccSolicitudDocu res = this._listSolicitudes.Find(x => x.Libranza.Value == Convert.ToInt32(this.lkpSolicitudes.EditValue));
                        this._numDocSolicitud = null;
                        if (res != null)
                            this._numDocSolicitud = res.NumeroDoc.Value;
                        this._polizaEstado.NumDocSolicitud.Value = this._numDocSolicitud;
                    }

                    if (this.lkpCreditos.EditValue != null && !string.IsNullOrWhiteSpace(this.lkpCreditos.EditValue.ToString()))
                    {
                        DTO_ccCreditoDocu res = this._listCreditos.Find(x => x.Libranza.Value == Convert.ToInt32(this.lkpCreditos.EditValue));
                        this._numDocCredito = null;
                        if (res != null)
                            this._numDocCredito = res.NumeroDoc.Value;

                        this._polizaEstado.NumDocCredito.Value = this._numDocCredito;
                        //if (this._tipoMvto == (byte)EstadoPoliza.Renovada)
                        //    this._polizaEstado.NumeroDocLiquida.Value = this._numDocCredito;
                    }

                    this._polizaEstado.VlrPoliza.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                    this._polizaEstado.FechaLiqSeguro.Value = this.dtFechaPoliza.DateTime;

                    #endregion
                    #region Salva la poliza

                    DTO_TxResult result = this._bc.AdministrationModel.RegistroPoliza(this.documentID, this._tipoMvto, this._polizaEstado);
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();

                    if (result.Result == ResultValue.OK)
                        this.CleanData();

                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para anular una póliza
        /// </summary>
        public override void TBDelete()
        {
            try
            {
                string msgTitleDelete = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                string msgDoc = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.DocumentEstadoConfirm);
                string msgBorrar = string.Format(msgDoc, "Borrar");
                if (MessageBox.Show(msgBorrar, msgTitleDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (this._polizaEstado.NumeroDocLiquida.Value.HasValue)
                    {
                        DTO_glDocumentoControl ctrl = this._bc.AdministrationModel.glDocumentoControl_GetByID(this._polizaEstado.NumeroDocLiquida.Value.Value);                   
                        if (ctrl != null && ctrl.DocumentoID.Value == AppDocuments.RenovacionPoliza)
                        {
                            MessageBox.Show("No es posible modificar ni eliminar porque ya fue procesada la renovación");
                            return;
                        }                   
                    }                      
                         
                    this._polizaEstado.AnuladaIND.Value = true;
                    this._bc.AdministrationModel.PolizaEstado_Delete(this.masterTercero.Value,this.txtPoliza.Text);

                    MessageForm frm = new MessageForm(new DTO_TxResult());
                    frm.ShowDialog();

                    this.CleanData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-PolizaRegistro.cs", "TBSave"));
            }
        }

        #endregion

    }
}
