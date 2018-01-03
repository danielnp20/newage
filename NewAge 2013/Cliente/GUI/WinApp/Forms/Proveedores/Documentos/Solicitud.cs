using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Resultados;
using System.Text.RegularExpressions;
using SentenceTransformer;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Solicitud
    /// </summary>
    public partial class Solicitud : DocumentProvForm
    {
        public Solicitud()
        {
          // InitializeComponent();
        }

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;
            this.CleanFooter();

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtSolicitudNro.Enabled = true;
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;
            this.btnQueryDoc.Enabled = true;
            FormProvider.Master.itemSave.Enabled = true;
            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        protected override void SendToApproveMethod()
        {
            this.disableValidate = true;
            this.gcDocument.DataSource = this.data.Footer;
            this.disableValidate = false;
            this.CleanFooter();

            this.CleanHeader(true);
            this.EnableHeader(false);
            this.masterPrefijo.EnableControl(true);
            this.masterPrefijo.Focus();
            this.txtSolicitudNro.Enabled = true;
            this.btnQueryDoc.Enabled = true;
            this.EnableFooter(false);
            this.gcCargos.Enabled = false;

            FormProvider.Master.itemSendtoAppr.Enabled = false;
        }

        #endregion

        #region Variables formulario

        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        private DTO_glDocumentoControl _ctrl = null;
        private DTO_prSolicitudDocu _solHeader = null;
        private List<DTO_prSolicitudFooter> _solFooter = null;       
        private DTO_seUsuario _usuario = null;

        private bool _headerLoaded = false;
        private bool _prefijoFocus = false;
        private bool _usuarioFocus = false;
        private bool _txtSolicitudNroFocus = false;
        private double _daysHighPriority = 0;
        private double _daysMediumPriority = 0;
        private double _daysLowPriority = 0;
        private double _daysEntr = 0;
        private bool _copyData = false;
        private bool moduleProyectoActive = false;
        #endregion

        #region Propiedades

        /// <summary>
        /// Variable que maneja la informacion de los temporales
        /// </summary>
        private DTO_prSolicitud TempData
        {
            get
            {
                return (DTO_prSolicitud)this.data;
            }
            set
            {
                this.data = value;
            }
        }

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Funcion para validacion de fechas
        /// </summary>
        private void ValidateDates()
        {
            int currentMonth = this.dtPeriod.DateTime.Month;
            int currentYear = this.dtPeriod.DateTime.Year;
            int minDay = 1;
            int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);

            this.dtFechaSol.Properties.MinValue = new DateTime(currentYear, currentMonth, minDay);
            //this.dtFechaSol.Properties.MaxValue = new DateTime(currentYear, currentMonth, lastDay);
           // this.dtFechaSol.DateTime = new DateTime(currentYear, currentMonth, minDay);

            this.dtFechaEntr.DateTime = this.dtFechaSol.DateTime.AddDays(_daysEntr);
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Valida una fila
        /// </summary>
        /// <param name="fila">Indice de la fila a validar</param>
        /// <returns>Retorna si la info de la fila es valida o no</returns>
        protected override bool ValidateRow(int fila)
        {
            bool validRow = true;

            try
            {
                validRow = base.ValidateRow(fila);

                if (validRow)
                {
                    #region Valida que la suma de los netos no supere al maximo permitido en valorFondo ni inferior a 0
                    //if (Convert.ToDecimal(this.txtPorcICA.EditValue) != 0)
                    //{
                    //    GridColumn col = this.gvDocument.Columns[this.unboundPrefix + "TerceroID"];
                    //    string noCuentaIca = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Fa_NoCuentaIca);

                    //    if (this._cuentaIcaNoExiste)
                    //    {
                    //        this.gvDocument.SetColumnError(col, noCuentaIca);
                    //        validRow = false;
                    //    }
                    //}
                    #endregion
                }

                if (validRow)
                {
                    this.isValid = true;
                    //this.CalcularTotal();

                    if (!this.newReg)
                        this.UpdateTemp(this.data);
                }
                else
                    this.isValid = false;

                this.hasChanges = true;
                return validRow;
            }
            catch (Exception ex)
            {
                validRow = false;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "ValidateRow"));
            }

            return validRow;
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.Solicitud;
            this.frmModule = ModulesPrefix.pr;
            InitializeComponent();

            base.SetInitParameters();

            this.data = new DTO_prSolicitud();
            this._ctrl = new DTO_glDocumentoControl();
            this._solHeader = new DTO_prSolicitudDocu();
            this._solFooter = new List<DTO_prSolicitudFooter>();

            List<DTO_glConsultaFiltro> filtrosLugarRecibido = new List<DTO_glConsultaFiltro>();
            filtrosLugarRecibido.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "RecibidosInd",
                OperadorFiltro = OperadorFiltro.Igual,
                ValorFiltro = "1"
            });
          
            this._bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
            this._bc.InitMasterUC(this.masterAreaAprob, AppMasters.glAreaFuncional, true, true, true, false);
            this._bc.InitMasterUC(this.masterLugarEntr, AppMasters.glLocFisica, false, true, true, false, filtrosLugarRecibido);
            this._bc.InitMasterUC(this.masterUsuario, AppMasters.seUsuario, true, true, true, false);
            this._bc.InitMasterUC(this.masterPrefijoOrdenTrabajo, AppMasters.glPrefijo, true, true, true, false);

            //Llena los combos
            TablesResources.GetTableResources(this.cmbPrioridad, typeof(Prioridad));
            this.cmbPrioridad.SelectedIndexChanged += new EventHandler(cmbPrioridad_SelectedIndexChanged);
            TablesResources.GetTableResources(this.cmbDestino, typeof(Destino));  

            //Trae datos de glControl
            this._daysHighPriority = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadAlta)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadAlta)) : 0;
            this._daysMediumPriority = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadMedia)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadMedia)) : 0;
            this._daysLowPriority = !string.IsNullOrEmpty(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadBaja)) ? Convert.ToInt32(this._bc.GetControlValueByCompany(ModulesPrefix.pr, AppControl.pr_DiasPermitEntregasPrioridadBaja)) : 0;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();

            this.tlSeparatorPanel.RowStyles[0].Height = 50;
            this.tlSeparatorPanel.RowStyles[1].Height = 80;
            this.tlSeparatorPanel.RowStyles[2].Height = 170;
            this.cmbDestino.SelectedItem = this.cmbDestino.GetItem(((byte)Destino.OrdenCompra).ToString());

            var modules = this._bc.AdministrationModel.aplModulo_GetByVisible(1, false);
            bool controlSolicitudProyInd = this._bc.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_IndControlSolicitudesBS).Equals("1") ? true : false;
            if (modules.Any(x => x.ModuloID.Value == ModulesPrefix.py.ToString()) && controlSolicitudProyInd)
            {
                this.masterPrefijoOrdenTrabajo.Visible = true;
                this.lblNroProyecto.Visible = true;
                this.lblPrefijoProy.Visible = true;
                this.txtOrdenTrabajoNro.Visible = true;
                this.btnProyectoNro.Visible = true;
                this.moduleProyectoActive = true;

            }

            this.EnableFooter(false);
            this.EnableHeader(false);

            if (!this._headerLoaded)
            {
                this.txtNumeroDoc.Text = "0";
                this.masterPrefijo.EnableControl(true);
                this.txtSolicitudNro.Enabled = true;
                if (string.IsNullOrEmpty(this.txtSolicitudNro.Text)) this.txtSolicitudNro.Text = "0";
                this.cmbPrioridad.SelectedIndex = 0;
                this.cmbDestino.SelectedItem = this.cmbDestino.GetItem(((byte)Destino.OrdenCompra).ToString());
                this._daysEntr = _daysHighPriority;
                this.ValidateDates();
                this.masterPrefijo.Value = base.prefijoID;
                this.masterAreaAprob.Value = base.areaFuncionalID;
            }
        }

        /// <summary>
        /// Limpia y deja vacio los controles del header
        /// </summary>
        /// <param name="basic">Indica si tambien se limpian los controles basicos</param>
        protected override void CleanHeader(bool basic)
        {
            if (basic)
            {
                string periodo = _bc.GetControlValueByCompany(this.frmModule, AppControl.pr_Periodo);
                this.dtPeriod.Text = periodo;
                this.prefijoID = string.Empty;
                this.masterPrefijo.Value = this.txtPrefix.Text;
                this.txtNumeroDoc.Text = "0";
                this.txtSolicitudNro.Text = "0";
                this.masterPrefijoOrdenTrabajo.Value = string.Empty;
                this.txtOrdenTrabajoNro.Text = "0";
            }
            
            this._usuario = new DTO_seUsuario();

            this.masterAreaAprob.Value = base.areaFuncionalID ;
            this.masterLugarEntr.Value = string.Empty;
            this.masterUsuario.Value = string.Empty;

            this.cmbPrioridad.SelectedIndex = 0;
            this._daysEntr = this._daysHighPriority;
            this.ValidateDates();

            this.cmbDestino.SelectedItem = this.cmbDestino.GetItem(((byte)Destino.OrdenCompra).ToString());
            //this.monedaId = this.monedaLocal;
            this.txtDescDoc.Text = string.Empty;
            base.CleanHeader(basic);
        }

        /// <summary>
        /// Oculta los controles del formulario
        /// </summary>
        protected override void EnableHeader(bool enable)
        {
            this.dtFecha.Enabled = false;
            this.dtPeriod.Enabled = false;
            this.txtPrefix.Enabled = false;
            this.txtNumeroDoc.Enabled = false;

            this.masterPrefijo.EnableControl(enable);
            this.masterAreaAprob.EnableControl(false);
            this.masterLugarEntr.EnableControl(enable);
            this.masterUsuario.EnableControl(enable);
            this.masterPrefijoOrdenTrabajo.EnableControl(enable);

            this.txtSolicitudNro.Enabled = enable;
            this.txtOrdenTrabajoNro.Enabled = enable;
            this.cmbPrioridad.Enabled = enable;
            this.cmbDestino.Enabled = enable;
            //this.dtFechaSol.Enabled = enable;
            //this.dtFechaEntr.Enabled = false;
            this.txtDescDoc.Enabled = enable;  
            this.btnProyectoNro.Enabled = enable;       
        }

        /// <summary>
        /// Carga informacion de un temporal a partir del cabezote
        /// </summary>
        /// <returns>Retorna el header de un temporal</returns>
        protected override DTO_prSolicitud LoadTempHeader()
        {
            try
            {

                #region Load DocumentoControl
                this._ctrl.EmpresaID.Value = this.empresaID;
                this._ctrl.TerceroID.Value = this.defTercero; ////
                this._ctrl.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._ctrl.ComprobanteID.Value = string.Empty; ////this.comprobanteID;
                this._ctrl.ComprobanteIDNro.Value = 0;
                this._ctrl.MonedaID.Value = this.monedaLocal; ////
                this._ctrl.CuentaID.Value = string.Empty; ////
                this._ctrl.ProyectoID.Value = this.defProyecto; ////
                this._ctrl.CentroCostoID.Value = this.defCentroCosto; ////
                this._ctrl.LugarGeograficoID.Value = this.defLugarGeo; ////
                this._ctrl.LineaPresupuestoID.Value = this.defLineaPresupuesto;////
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.PeriodoDoc.Value = this.dtPeriod.DateTime;
                this._ctrl.PrefijoID.Value = this.masterPrefijo.Value;
                this._ctrl.TasaCambioCONT.Value = 0;////
                this._ctrl.TasaCambioDOCU.Value = 0;////
                this._ctrl.DocumentoNro.Value = Convert.ToInt32(txtSolicitudNro.Text);
                this._ctrl.DocumentoID.Value = this.documentID;
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocInterno;////
                this._ctrl.PeriodoUltMov.Value = this.dtPeriod.DateTime;
                this._ctrl.seUsuarioID.Value = this.userID;
                this._ctrl.AreaFuncionalID.Value = this.areaFuncionalID;
                this._ctrl.ConsSaldo.Value = 0;
                this._ctrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                this._ctrl.Observacion.Value = this.txtDescDoc.Text;
                this._ctrl.FechaDoc.Value = this.dtFechaSol.DateTime;
                this._ctrl.Descripcion.Value = this.txtDocDesc.Text;
                this._ctrl.Valor.Value = 0;
                this._ctrl.Iva.Value = 0;
                #endregion
                #region Load SolicitudHeader
                this._solHeader.EmpresaID.Value = this.empresaID;
                this._solHeader.NumeroDoc.Value = Convert.ToInt32(this.txtNumeroDoc.Text);
                this._solHeader.Prioridad.Value = Convert.ToByte((((ComboBoxItem)(this.cmbPrioridad.SelectedItem)).Value));
                this._solHeader.FechaEntrega.Value = this.dtFechaEntr.DateTime;
                this._solHeader.LugarEntrega.Value = this.masterLugarEntr.Value;
                this._solHeader.AreaAprobacion.Value = this.masterAreaAprob.Value;
                this._solHeader.UsuarioSolicita.Value = this.masterUsuario.Value;
                this._solHeader.Destino.Value = Convert.ToByte((((ComboBoxItem)(this.cmbDestino.SelectedItem)).Value));
                #endregion

                //this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                this.monedaId = this._ctrl.MonedaID.Value;

                DTO_prSolicitud sol = new DTO_prSolicitud();
                sol.Header = this._solHeader;
                sol.DocCtrl = this._ctrl;
                sol.Footer = new List<DTO_prSolicitudFooter>();
                this._solFooter = sol.Footer;

                return sol;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "LoadTempHeader"));
                return null;
            }
        }
        
        /// <summary>
        /// valida la informacion del header
        /// </summary>
        protected override bool ValidateHeader()
        {
            #region Valida los datos obligatorios
            
            #region Valida datos en la maestra de Prefijo
            if (!this.masterPrefijo.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de AreaAprob
            if (!this.masterAreaAprob.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAreaAprob.CodeRsx);

                MessageBox.Show(msg);
                this.masterAreaAprob.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de Usuario
            if (!this.masterUsuario.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterUsuario.CodeRsx);

                MessageBox.Show(msg);
                this.masterUsuario.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de LugarRecibido
            if (!this.masterLugarEntr.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblLugarEntrega.Text);

                MessageBox.Show(msg);
                this.masterLugarEntr.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la maestra de LugarRecibido
            if (!this.masterUsuario.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterUsuario.CodeRsx);

                MessageBox.Show(msg);
                this.masterUsuario.Focus();

                return false;
            }
            #endregion

            #region Valida datos en la fecha de la solicitud
            if (string.IsNullOrEmpty(this.dtFechaSol.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaSol");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaSol.Focus();
                return false;
            }
            #endregion

            #region Valida datos en la fecha de entrega
            if (string.IsNullOrEmpty(this.dtFechaEntr.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblFechaEntr");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.dtFechaEntr.Focus();
                return false;
            }
            #endregion

            #region Valida datos en el descriptivo
            if (string.IsNullOrEmpty(this.txtDescDoc.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDescDoc");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.txtDescDoc.Focus();
                return false;
            }
            #endregion

            #region Valida datos en el Destino
            if (string.IsNullOrEmpty(this.cmbDestino.Text))
            {
                string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblDestino");
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                MessageBox.Show(msg);

                this.cmbDestino.Focus();
                return false;
            }
            #endregion

            #region Valida datos del Modulo Proyectos
            if (this.moduleProyectoActive)
            {
                if (!this.masterPrefijoOrdenTrabajo.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblPrefijoProy.Text);
                    MessageBox.Show(msg);

                    this.masterPrefijoOrdenTrabajo.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtOrdenTrabajoNro.Text))
                {
                    string txtRsx = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString() + "_lblProyectoNro");
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), txtRsx);
                    MessageBox.Show(msg);

                    this.txtOrdenTrabajoNro.Focus();
                    return false;
                } 
            }
            #endregion
            #endregion
            return true;
        }

        /// <summary>
        /// Carga la informacion temporal del documento
        /// Si no tiene informacion del comprobante el temporal se guardo mal
        /// </summary>
        /// <param name="leg">Informacion del temporal</param>
        protected override void LoadTempData(DTO_prSolicitud sol)
        {
            try
            {
                DTO_glDocumentoControl ctrl = sol.DocCtrl;
                DTO_prSolicitudDocu solHeader = sol.Header;

                if (sol.Footer == null)
                    sol.Footer = new List<DTO_prSolicitudFooter>();
                this._solFooter = sol.Footer;

                this.masterPrefijo.Value = ctrl.PrefijoID.Value;
                this.masterLugarEntr.Value = solHeader.LugarEntrega.Value;
                this.masterAreaAprob.Value = solHeader.AreaAprobacion.Value;
                this.masterUsuario.Value = solHeader.UsuarioSolicita.Value;

                this.txtSolicitudNro.Text = ctrl.DocumentoNro.Value.Value.ToString();
                this.dtFechaSol.DateTime = ctrl.FechaDoc.Value.Value;
                this.dtFechaEntr.DateTime = solHeader.FechaEntrega.Value.Value;

                Prioridad prior = (Prioridad)solHeader.Prioridad.Value.Value;
                string priorString = ((int)prior).ToString();
                this.cmbPrioridad.SelectedItem = this.cmbPrioridad.GetItem(priorString);
                Destino dest = (Destino)solHeader.Destino.Value.Value;
                string destString = ((int)dest).ToString();
                this.cmbDestino.SelectedItem = this.cmbDestino.GetItem(destString);

                this.txtDescDoc.Text = ctrl.Descripcion.Value;

                this.monedaId = ctrl.MonedaID.Value;

                this.dtPeriod.DateTime = ctrl.PeriodoDoc.Value.Value;
                this.txtNumeroDoc.Text = ctrl.NumeroDoc.Value.Value.ToString();
                this.dtFecha.DateTime = ctrl.FechaDoc.Value.Value;

                //Si se presenta un problema asignando la tasa de cambio lo bloquea
                if (this.ValidateHeader())
                {
                    this.EnableHeader(false);
                    this.data = sol;
                    this._ctrl = sol.DocCtrl;
                    this._solHeader = sol.Header;
                    this._solFooter = sol.Footer;

                    this.validHeader = true;
                    this._headerLoaded = true;

                    this.LoadData(true);
                    this.gcDocument.Focus();
                }
                else
                    this.CleanHeader(true);
            }
            catch (Exception ex)
            {
                 MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "LoadTempData"));
            }
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
            FormProvider.Master.itemPrint.Visible = true;
            if (FormProvider.Master.LoadFormTB)
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
        }
        
        #endregion

        #region Eventos Header

        /// <summary>
        /// Evento que se ejecuta al entrar el prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Enter(object sender, EventArgs e)
        {
            this._prefijoFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del prefijo control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterPrefijo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_prefijoFocus)
                {
                    _prefijoFocus = false;
                    if (this.masterPrefijo.ValidID)
                    {
                        this.prefijoID = this.masterPrefijo.Value;
                        //this.txtPrefix.Text = this.prefijoID;
                    }
                    else
                        CleanHeader(true);  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "masterPrefijo_Leave"));
            }
        }

        /// <summary>
        /// Valida que solo numeros se pueden escribir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSolicitudNro_KeyPress(object sender, KeyPressEventArgs e)
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
        private void txtSolicitudNro_Enter(object sender, EventArgs e)
        {           
            this._txtSolicitudNroFocus = true;
            if (!this.masterPrefijo.ValidID)
            {
                this._txtSolicitudNroFocus = false;
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPrefijo.CodeRsx);

                MessageBox.Show(msg);
                this.masterPrefijo.Focus();
            }
        }

        /// <summary>
        /// Valida que el numero del recibo ingresado exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtSolicitudNro_Leave(object sender, EventArgs e)
        {
            if (this._txtSolicitudNroFocus)
            {
                _txtSolicitudNroFocus = false;
                if (this.txtSolicitudNro.Text == string.Empty)
                    this.txtSolicitudNro.Text = "0";

                if (this.txtSolicitudNro.Text == "0")
                {
                    #region Nueva solicitud
                    this.gcDocument.DataSource = null;
                    this.data = null;
                    this.newDoc = true;
                    this.EnableHeader(true);
                    this.masterPrefijo.EnableControl(false);
                    this.txtSolicitudNro.Enabled = false;
                    this.masterUsuario.Value = this._bc.AdministrationModel.User.ID.Value;
                    this.masterLugarEntr.Value = string.Empty;
                    this.masterAreaAprob.Value =  this._bc.AdministrationModel.User.AreaFuncionalID.Value;
                    this.txtDescDoc.Text = string.Empty;
                    #endregion
                }
                else
                {
                    try
                    {
                        DTO_prSolicitud Sol = _bc.AdministrationModel.Solicitud_Load(this.documentID, this.masterPrefijo.Value, Convert.ToInt32(this.txtSolicitudNro.Text));
                        //Valida si existe
                        if (Sol == null || Sol.Header == null)
                        {
                            MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_NoSolicitudes)); ////
                            this.txtSolicitudNro.Focus();
                            this.validHeader = false;
                            return;
                        }
                        if (this._copyData)
                        {
                            Sol.DocCtrl.NumeroDoc.Value = 0;
                            Sol.DocCtrl.DocumentoNro.Value = 0;
                            Sol.Header.NumeroDoc.Value = 0;                           
                            Sol.DocCtrl.Estado.Value = (byte)EstadoDocControl.SinAprobar;
                            this._copyData = false;  
                        }

                        this.newDoc = false;

                        //Carga los datos
                        this._ctrl = Sol.DocCtrl;
                        this._solHeader = Sol.Header;

                        #region Asigna los valores
                        this.txtNumeroDoc.Text = this._ctrl.NumeroDoc.Value.Value.ToString();
                        this.masterPrefijo.Value = this._ctrl.PrefijoID.Value;
                        this.masterLugarEntr.Value = this._solHeader.LugarEntrega.Value;
                        this.masterAreaAprob.Value = this._solHeader.AreaAprobacion.Value;
                        this.masterUsuario.Value = this._solHeader.UsuarioSolicita.Value;

                        this.txtSolicitudNro.Text = this._ctrl.DocumentoNro.Value.Value.ToString();
                        this.dtFechaSol.DateTime = this._ctrl.FechaDoc.Value.Value;
                        this.dtFechaEntr.DateTime = this._solHeader.FechaEntrega.Value.Value;                           
                        this.txtDescDoc.Text = this._ctrl.Observacion.Value;

                        Prioridad prior = (Prioridad)_solHeader.Prioridad.Value.Value;
                        string priorString = ((int)prior).ToString();
                        this.cmbPrioridad.SelectedItem = this.cmbPrioridad.GetItem(priorString);
                        Destino dest = (Destino)_solHeader.Destino.Value.Value;
                        string destString = ((int)dest).ToString();
                        this.cmbDestino.SelectedItem = this.cmbDestino.GetItem(destString);

                        //this._tipoMonedaOr = this._ctrl.MonedaID.Value == this.monedaLocal ? TipoMoneda_LocExt.Local : TipoMoneda_LocExt.Foreign;
                        this.monedaId = this._ctrl.MonedaID.Value;
                        this._headerLoaded = true;
                            
                        if (Sol.Footer != null)
                        {
                            this._solFooter = Sol.Footer;
                            this.gcDocument.Focus();
                        }
                        else
                            this._solFooter = new List<DTO_prSolicitudFooter>();

                        this.data = Sol;
                        this.LoadData(true);                           
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "txtSolicitudNro_Leave"));
                    }
                }
            }
        }

        /// <summary>
        /// Evento que se ejecuta al entrar el usuario control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterUsuario_Enter(object sender, EventArgs e)
        {
            this._usuarioFocus = true;
        }

        /// <summary>
        /// Evento que se ejecuta al salir del usuario control 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void masterUsuario_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._usuarioFocus)
                {
                    this._usuarioFocus = false;
                    if (this.masterUsuario.ValidID)
                    {
                        if (this._usuario == null || this._usuario.ID.Value != this.masterUsuario.Value)
                        {
                            //Trae informacion sobre el usuario                    
                            this._usuario = (DTO_seUsuario)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, false, this.masterUsuario.Value, true);

                            this.masterAreaAprob.Value = this._usuario.AreaFuncionalID.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "masterUsuario_Leave"));
            }
        }        

        /// <summary>
        /// valida la edición de las fechas
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void dtFechas_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                this.ValidateDates();
            }
            catch (Exception)
            { ; }

        }

        /// <summary>
        /// valida la edición de las fechas segun nivel de prioridad
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void cmbPrioridad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Prioridad)Enum.Parse(typeof(Prioridad),(this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.High)
                this._daysEntr = this._daysHighPriority;
            if ((Prioridad)Enum.Parse(typeof(Prioridad), (this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.Medium)
                this._daysEntr = this._daysMediumPriority;
            if ((Prioridad)Enum.Parse(typeof(Prioridad), (this.cmbPrioridad.SelectedItem as ComboBoxItem).Value) == Prioridad.Low)
                this._daysEntr = this._daysLowPriority;
            this.ValidateDates();
        }

        /// <summary>
        /// Consulta de documentos
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Envento</param>
        private void btnQueryDoc_Click(object sender, EventArgs e)
        {
            if (this.btnQueryDoc.Focused)
            {
                List<int> docs = new List<int>();
                docs.Add(AppDocuments.Solicitud);
                ModalQuerySolicitud getDocControl = new ModalQuerySolicitud(docs);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtSolicitudNro.Enabled = true;
                    this.txtSolicitudNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtSolicitudNro.Focus();
                    this.btnQueryDoc.Focus();
                    this.btnQueryDoc.Enabled = false;
                }
            }
            else
            {
                ModalQueryDocument getDocControl = new ModalQueryDocument(null);
                getDocControl.ShowDialog();
                if (getDocControl.DocumentoControl != null)
                {
                    if (getDocControl.CopiadoInd)
                        this._copyData = true;
                    this.txtOrdenTrabajoNro.Enabled = true;
                    this.txtOrdenTrabajoNro.Text = getDocControl.DocumentoControl.DocumentoNro.Value.ToString();
                    this.masterPrefijoOrdenTrabajo.Value = getDocControl.DocumentoControl.PrefijoID.Value;
                    this.txtOrdenTrabajoNro.Focus();
                    this.btnProyectoNro.Focus();
                }
            }
        }

        /// <summary>
        /// Valida que el numero  exista
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void txtProyectoNro_Leave(object sender, EventArgs e)
        {
            if (this.masterPrefijoOrdenTrabajo.ValidID && !string.IsNullOrEmpty(this.txtOrdenTrabajoNro.Text))
            {
                //Trae el documento de la Solicitud de Proyectos
                DTO_glDocumentoControl doc = this._bc.AdministrationModel.glDocumentoControl_GetInternalDoc(AppDocuments.Proyecto, this.masterPrefijoOrdenTrabajo.Value, Convert.ToInt32(this.txtOrdenTrabajoNro.Text));
                if (doc != null)
                {
                    //Trae el detalle generado en proveedores por la Orden de Trabajo (Modulo Proyectos)
                    base._listSolicitudProyectos = this._bc.AdministrationModel.prDetalleDocu_GetByNumeroDoc(doc.NumeroDoc.Value.Value, false);
                    if (base._listSolicitudProyectos == null)
                    {
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_DetailProvNotExist));
                        this.txtOrdenTrabajoNro.Focus();
                        this.validHeader = false;
                        return;
                    }
                    else
                    {
                        //Valida si existen ya Solicitudes de Proveedores con el documento consultado para ver cantidad disponible
                        DTO_prDetalleDocu detaFilter = new DTO_prDetalleDocu();
                        detaFilter.Documento2ID.Value = doc.NumeroDoc.Value;
                        List<DTO_prDetalleDocu> listSolicitudExist = this._bc.AdministrationModel.prDetalleDocu_GetParameter(detaFilter).Where(x => x.NumeroDoc.Value != x.Documento2ID.Value).ToList();
                           foreach (var solExist in listSolicitudExist)
                                 base._listSolicitudProyectos.Where(x => x.CodigoBSID.Value == solExist.CodigoBSID.Value && x.inReferenciaID.Value == solExist.inReferenciaID.Value).ToList().ForEach(z => z.CantidadDoc2.Value -= solExist.CantidadSol.Value);  
                    }
                }
                else
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_NoDocument));
                    this.txtOrdenTrabajoNro.Focus();
                    this.validHeader = false;
                    return;
                }
            }
            
        }
        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Evento que se ejecuta al entrar a la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_Enter(object sender, EventArgs e)
        {
            if (this.ValidateHeader())
                this.validHeader = true;
            else
                this.validHeader = false;

            //Si el diseño esta cargado y el header es valido
            if (this.validHeader)
            {
                this.ValidHeaderTB();
                if (this.txtSolicitudNro.Text == "0")
                {
                   // FormProvider.Master.itemSendtoAppr.Enabled = false;
                    FormProvider.Master.itemExport.Enabled = false;
                    FormProvider.Master.itemPrint.Enabled = false;
                }

                #region Si entra al detalle y no tiene datos
                this.EnableHeader(false);
                try
                {
                    if (!this._headerLoaded)
                    {
                        DTO_prSolicitud sol = this.LoadTempHeader();
                        this._solHeader = sol.Header;
                        this._solFooter = sol.Footer;
                        this.TempData = sol;
                                               
                        this.LoadData(true);

                        this.UpdateTemp(this.data);
                        this._headerLoaded = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "gcDocument_Enter" + ex.Message));
                }
                #endregion
                #region Si ya tiene datos cargados
                //if (!this.dataLoaded)
                //{
                //    MessageBox.Show(_bc.GetResourceError(DictionaryMessages.Err_DocInvalidHeader));
                //    return;
                //}
                #endregion
            }
        }

        /// <summary>
        /// Evento que se ejecuta al presionar un boton por defecto de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gcDocument_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            base.gcDocument_EmbeddedNavigator_ButtonClick(sender, e);

            if (!this.validHeader)
                this.masterPrefijo.Focus();

            if (this.txtNumeroDoc.Text != "0")
            {
                //FormProvider.Master.itemSendtoAppr.Enabled = false;
                FormProvider.Master.itemPrint.Enabled = false;
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
                this.cleanDoc = true;
                base.TBNew();
                if (this.cleanDoc)
                {
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();

                    this.gvDocument.ActiveFilterString = string.Empty;
                    this.disableValidate = true;
                    this.gcDocument.DataSource = this.data.Footer;
                    this.disableValidate = false;
                    this.gvCargos.ActiveFilterString = string.Empty;
                    this.gcCargos.DataSource = null;
                    this.gvCargos.RefreshData();

                    this._prefijoFocus = false;
                    this._txtSolicitudNroFocus = false;
                    this.CleanHeader(true);
                    this.EnableHeader(false);
                    this.masterPrefijo.EnableControl(true);
                    this.masterPrefijoOrdenTrabajo.EnableControl(true);
                    this.txtSolicitudNro.Enabled = true;
                    this.txtOrdenTrabajoNro.Enabled = true;
                    this.btnQueryDoc.Enabled = true;
                    this.btnProyectoNro.Enabled = true;
                    this.masterPrefijo.Focus();                 
                    this._headerLoaded = false;
                    this.cleanDoc = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                base.TBSave();
                this.gvDocument.PostEditor();
                this.gvDocument.ActiveFilterString = string.Empty;
                if (!this.isValidCargo)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Pr_PorcentajeNoCien));
                    return;
                }
                if (this.ValidGrid())
                {
                    FormProvider.Master.itemSave.Enabled = false;
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para enviar a aprobacion un comprobante
        /// </summary>
        public override void TBSendtoAppr()
        {
                this.gvDocument.PostEditor();

                this.gvDocument.ActiveFilterString = string.Empty;
                if (this.ValidGrid())
                {
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }    
        }

        /// <summary>
        /// Boton para imprimir reporte
        /// </summary>
        public override void TBPrint()
        {
            try
            {
                if (this._ctrl != null && this._ctrl.NumeroDoc.Value.HasValue && this._ctrl.NumeroDoc.Value != 0)
                {
                    bool isPreliminar = this._ctrl.Estado.Value == (byte)EstadoDocControl.Aprobado ? false : true;
                    string reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(this.documentID, this._ctrl.NumeroDoc.Value.Value, true, 0);
                    string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, this._ctrl.NumeroDoc.Value, null, reportName);
                    Process.Start(fileURl);          
                }
           }           
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "TBPrint"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                int numeroDoc = 0;
                bool update = false;
                if (this._ctrl.NumeroDoc.Value.Value != 0)
                {
                    numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                    update = true;
                };

                #region Valida si el Modulo de Proyectos esta activo
                if (this.moduleProyectoActive)
                {
                    foreach (var item in this.TempData.Footer)
                    {
                        item.DetalleDocu.Documento2ID.Value = this._listSolicitudProyectos.First().Documento2ID.Value;
                        item.DetalleDocu.Detalle2ID.Value = this._listSolicitudProyectos.First().Detalle2ID.Value;
                    }
                }
                #endregion
                DTO_SerializedObject result = _bc.AdministrationModel.Solicitud_Guardar(this.documentID, this.TempData.DocCtrl, this.TempData.Header,null, this.TempData.Footer, update, out numeroDoc);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    #region Genera el reporte
                    string reportName = this._bc.AdministrationModel.ReportesProveedores_SolicitudOrRecibidoDoc(this.documentID,numeroDoc,true,0);
                    //string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Documentos, numeroDoc, null,reportName);
                    //Process.Start(fileURl);
                    #endregion

                    _bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), _bc.AdministrationModel.User);
                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();
                    this._headerLoaded = false;
                    this.Invoke(this.saveDelegate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        /// <summary>
        /// Hilo de Enviar para aprobación
        /// </summary>
        public override void SendToApproveThread()
        {
            try
            {
                DTO_TxResult resultNOK = new DTO_TxResult();
                resultNOK.Result = ResultValue.NOK;

                this.gvDocument.ActiveFilterString = string.Empty;

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this.documentID, this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (this._bc.AdministrationModel.ConsultarProgresoProveedores(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);
                DTO_SerializedObject result = new DTO_SerializedObject() ;

                if (this._ctrl == null)
                {
                    result = resultNOK;
                    return;
                }
                else if (this.data.DocCtrl.NumeroDoc.Value.Value == 0)
                {
                    int numeroDoc = 0;
                    bool update = false;
                    if (this._ctrl.NumeroDoc.Value.Value != 0)
                    {
                        numeroDoc = this._ctrl.NumeroDoc.Value.Value;
                        update = true;
                    }
                    #region Valida si el Modulo de Proyectos esta activo
                    if (this.moduleProyectoActive)
                    {
                        foreach (var item in this.TempData.Footer)
                        {
                            item.DetalleDocu.Documento2ID.Value = this._listSolicitudProyectos.First().Documento2ID.Value;
                            item.DetalleDocu.Detalle2ID.Value = this._listSolicitudProyectos.First().Detalle2ID.Value;
                            item.DetalleDocu.CantidadDoc2.Value = item.DetalleDocu.CantidadSol.Value;
                        }
                    } 
                    #endregion
                    result = this._bc.AdministrationModel.Solicitud_Guardar(this.documentID, this.TempData.DocCtrl, this.TempData.Header, null, this.TempData.Footer, update, out numeroDoc);
                    this._ctrl.NumeroDoc.Value = numeroDoc;
                }
                if (result.GetType() != typeof(DTO_TxResult))               
                    result = this._bc.AdministrationModel.Solicitud_SendToAprob(this.documentID, this._ctrl.NumeroDoc.Value.Value, true);
                FormProvider.Master.StopProgressBarThread(this.documentID);
                bool isOK = this._bc.SendDocumentMail(MailType.SendToApprove, this.documentID, this._actFlujo.seUsuarioID.Value, result, true,true);
                if (isOK)
                {
                    this._bc.AdministrationModel.aplTemporales_Clean(this.documentID.ToString(), this._bc.AdministrationModel.User);

                    this.newDoc = true;
                    this.deleteOP = true;
                    this.data = new DTO_prSolicitud();
                    this._ctrl = new DTO_glDocumentoControl();
                    this._solHeader = new DTO_prSolicitudDocu();
                    this._solFooter = new List<DTO_prSolicitudFooter>();
                    this._headerLoaded = false;
                    this.Invoke(this.sendToApproveDelegate);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this._bc.GetResourceForException(ex, "WinApp-Solicitud.cs", "SendToApproveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }
        
        #endregion
    }
}
