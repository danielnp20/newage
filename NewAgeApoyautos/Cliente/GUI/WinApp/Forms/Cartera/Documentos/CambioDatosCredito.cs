using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.ComponentModel;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System.Linq;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CambioDatosCredito : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        //Variables generales
        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //Variables formulario        
        private DTO_ccCliente cliente = new DTO_ccCliente();
        private DTO_ccCreditoDocu credito = new DTO_ccCreditoDocu();
        private string _libranzaID = string.Empty;
        private string _centroPagoID = string.Empty;

        //Variables auxiliares del formulario
        private DateTime periodo;
        private bool isModalFormOpened;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CambioDatosCredito()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CambioDatosCredito(string mod)
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
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                if (!string.IsNullOrWhiteSpace(mod))
                    this._frmModule = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), mod);
                
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "DigitacionCredito"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppDocuments.CambioDatosCredito;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
            _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
            _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
            _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);
            _bc.InitMasterUC(this.masterZona, AppMasters.glZona, true, true, true, false);
            _bc.InitMasterUC(this.masterCooperativa, AppMasters.ccCooperativa, true, true, true, false);
            _bc.InitMasterUC(this.masterNovedad, AppMasters.ccIncorporacionNovedad, true, true, true, false);
            _bc.InitMasterUC(this.masterCobranzaEstado, AppMasters.ccCobranzaEstado, true, true, true, false);
            _bc.InitMasterUC(this.masterCobranzaGestion, AppMasters.ccCobranzaGestion, true, true, true, false);
            _bc.InitMasterUC(this.masterSiniestroEstado, AppMasters.ccSiniestroEstado, true, true, true, false);

            this.masterCliente.EnableControl(false); 
            this.masterPagaduria.EnableControl(false);

            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);
            this.dtFecha.Properties.MaxValue = new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            this.dtFecha.DateTime = new DateTime(this.periodo.Year, this.periodo.Month, this.periodo.Day);

            this.EnableHeader(false);
            this.txtLibranza.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            try
            {
                this.masterCooperativa.EnableControl(enabled);
                this.masterCentroPago.EnableControl(enabled);
                this.masterCiudad.EnableControl(enabled);
                this.masterZona.EnableControl(enabled);
                this.masterNovedad.EnableControl(enabled);
                this.masterCobranzaEstado.EnableControl(enabled);
                this.masterCobranzaGestion.EnableControl(enabled);
                this.masterSiniestroEstado.EnableControl(enabled);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "EnableHeader"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            try
            {
                //Header
                this.txtLibranza.Text = String.Empty;
                this.masterCliente.Value = String.Empty;
                this.masterCooperativa.Value = string.Empty;
                this.masterCentroPago.Value = String.Empty;
                this.masterPagaduria.Value = String.Empty;
                this.masterCiudad.Value = String.Empty;
                this.masterZona.Value = String.Empty;
                this.masterNovedad.Value = String.Empty;
                this.masterCobranzaEstado.Value = String.Empty;
                this.masterCobranzaGestion.Value = String.Empty;
                this.masterSiniestroEstado.Value = String.Empty;

                this.txtObservacion.Text = String.Empty;

                this.comboPlazo.SelectedIndex = -1;
                this.txtVlrSolicitado.EditValue = 0;
                this.txtVlrAdicional.EditValue = 0;
                this.txtVlrPrestamo.EditValue = 0;
                this.txtVlrCompra.EditValue = 0;
                this.txtVlrCuota.EditValue = 0;
                this.txtVlrLibranza.EditValue = 0;
                this.txtVlrGiroTerceros.EditValue = 0;
                this.txtVlrCliente.EditValue = 0;
                this.txtVlrGiro.EditValue = 0;
                this.txtVlrDescuento.EditValue = 0;

                //Footer
                this.credito = null;

                //Variables
                this._libranzaID = string.Empty;
                this._centroPagoID = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "CleanData"));
            }

        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            try
            {
                #region Validaciones básicas

                //Valida que este escrito el numero de Libranza
                if (string.IsNullOrEmpty(this.txtLibranza.Text) || string.IsNullOrWhiteSpace(this._libranzaID))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lblLibranza.Text);
                    MessageBox.Show(msg);
                    this.txtLibranza.Focus();
                    return false;
                }

                //Valida que el usuario exista
                if (!this.masterCliente.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCliente.Focus();
                    return false;
                }

                //Valida que la cooperativa
                if (!this.masterCooperativa.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCooperativa.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCooperativa.Focus();
                    return false;
                }
                
                //Valida que el centro de pago exista
                if (!this.masterCentroPago.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCentroPago.Focus();
                    return false;
                }
             
                //Valida que la pagaduria exista
                if (!this.masterPagaduria.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPagaduria.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterPagaduria.Focus();
                    return false;
                }
               
                //Valida que la ciudad exista
                if (!this.masterCiudad.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCiudad.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCiudad.Focus();
                    return false;
                }
                
                //Valida que la zona exista
                if (!this.masterZona.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterZona.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterZona.Focus();
                    return false;
                }

                //Valida que la la novedad exista
                if (!this.masterNovedad.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterNovedad.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterNovedad.Focus();
                    return false;
                }

                //Valida que el estado de cobranza exista
                if (!this.masterCobranzaEstado.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCobranzaEstado.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCobranzaEstado.Focus();
                    return false;
                }

                //Valida que la gestion de cobranza exista
                if (!this.masterCobranzaGestion.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCobranzaGestion.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCobranzaGestion.Focus();
                    return false;
                }

                //Valida que el estado de siniestro exista
                if (!this.masterSiniestroEstado.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterSiniestroEstado.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterSiniestroEstado.Focus();
                    return false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "ValidateHeader"));
            }

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

        #endregion Funciones Privadas

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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemNew.Visible = true;
                    FormProvider.Master.itemNew.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Approve) ||
                        SecurityManager.HasAccess(this._documentID, FormsActions.Edit);

                    FormProvider.Master.itemSearch.Visible = false;
                    FormProvider.Master.itemUpdate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Closing(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "Form_Closing"));
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
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this._frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que verifica que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsSeparator(e.KeyChar))
                e.Handled = false;
            else
                e.Handled = true;
        }

        /// <summary>
        /// Evento que permite crear, habilitar o deshabilitar las propiedades del documento con base a la Libranza  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._libranzaID != this.txtLibranza.Text.Trim() && !String.IsNullOrWhiteSpace(this.txtLibranza.Text))
                {
                    string tmp = this.txtLibranza.Text;
                    this._libranzaID = this.txtLibranza.Text.Trim();
                    this.credito = _bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this.txtLibranza.Text));
                    if (this.credito != null)
                    {
                        this.EnableHeader(true);
                        this._libranzaID = this._libranzaID.ToString();

                        this.masterCliente.Value = this.credito.ClienteID.Value;
                        this.masterCooperativa.Value = this.credito.CooperativaID.Value;
                        this.masterCentroPago.Value = this.credito.CentroPagoID.Value;
                        this.masterPagaduria.Value = this.credito.PagaduriaID.Value;
                        this.masterCiudad.Value = this.credito.Ciudad.Value;
                        this.masterZona.Value = this.credito.ZonaID.Value;
                        this.masterNovedad.Value = this.credito.NovedadIncorporaID.Value;
                        this.masterCobranzaEstado.Value = this.credito.CobranzaEstadoID.Value;
                        this.masterCobranzaGestion.Value = this.credito.CobranzaGestionID.Value;
                        this.masterSiniestroEstado.Value = this.credito.SiniestroEstadoID.Value;

                        this.txtObservacion.Text = _bc.AdministrationModel.User.ID.Value + " - " + this.dtFecha.DateTime.ToString(FormatString.ControlDate);

                        this.comboPlazo.Text = this.credito.Plazo.Value.ToString();
                        this.txtVlrAdicional.EditValue = this.credito.VlrAdicional.Value;
                        this.txtVlrSolicitado.EditValue = this.credito.VlrSolicitado.Value;
                        this.txtVlrPrestamo.EditValue = this.credito.VlrPrestamo.Value;
                        this.txtVlrCuota.EditValue = this.credito.VlrCuota.Value;
                        this.txtVlrLibranza.EditValue = this.credito.VlrLibranza.Value;
                        this.txtVlrCompra.EditValue = this.credito.VlrCompra.Value;
                        this.txtVlrGiro.EditValue = this.credito.VlrGiro.Value;

                        //Variables
                        this._libranzaID = this.txtLibranza.Text;
                        this._centroPagoID = this.credito.CentroPagoID.Value;
                    }
                    else
                    {
                        this._libranzaID = string.Empty;
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoDisponible);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Eventro que carga la pagaduria cuando se seleciona un centro pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            if (this._centroPagoID != this.masterCentroPago.Value)
            {
                if (this.masterCentroPago.ValidID)
                {
                    this._centroPagoID = this.masterCentroPago.Value;
                    DTO_ccCentroPagoPAG centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, masterCentroPago.Value, true);
                    this.masterPagaduria.Value = centroPago.PagaduriaID.Value;
                }
                else
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                    MessageBox.Show(msg);
                }
            }
        }

        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
                this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "TBNew"));
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
                    string titleWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                    string msgWarning = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_ActualizarDatosCredito);

                    if (MessageBox.Show(msgWarning, titleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    DTO_TxResult result = _bc.AdministrationModel.Credito_CambioDatos(this._documentID, this.credito, this.masterCentroPago.Value,
                        this.masterPagaduria.Value, this.masterZona.Value, this.masterCiudad.Value, this.masterCooperativa.Value, this.masterNovedad.Value, 
                        this.masterCobranzaEstado.Value, this.masterCobranzaGestion.Value, this.masterSiniestroEstado.Value, this.txtObservacion.Text);

                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                    if (result.Result == ResultValue.OK)
                    {
                        this.CleanData();
                        this.txtLibranza.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCredito.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas

    }
}