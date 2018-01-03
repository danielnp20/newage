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
    public partial class CambioDatosCliente : FormWithToolbar
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
        private string _clienteID = String.Empty;
        private DTO_ccCliente cliente = new DTO_ccCliente();
        private DTO_coTercero tercero = new DTO_coTercero();


        //Variables auxiliares del formulario
        private DateTime periodo;
        private bool isModalFormOpened;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CambioDatosCliente()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public CambioDatosCliente(string mod)
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "DigitacionCredito"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this._documentID = AppMasters.ccClienteCambioDatos;
            //this._documentID = AppDocuments.CambioDatosCliente;
            this._frmModule = ModulesPrefix.cc;

            //Carga la informacion de la maestras
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
            _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);

            this.masterCliente.EnableControl(true); 
            //this.masterPagaduria.EnableControl(false);

            //Establece la fecha del periodo actual
            string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.periodo = Convert.ToDateTime(strPeriodo);

            this.EnableHeader(false);
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            try
            {
                //this.masterCliente.EnableControl(enabled);

                this.txtDireccion.Enabled = enabled;
                this.txtTelefono1.Enabled = enabled;
                this.txtTelefono2.Enabled = enabled;

                this.txtCelular1.Enabled = enabled;
                this.txtCelular2.Enabled = enabled;
                this.txtCorreo.Enabled = enabled;
                this.rbTipoCambio.Enabled = enabled;
                this.masterCiudad.EnableControl(enabled);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "EnableHeader"));
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
                this.masterCliente.Text = String.Empty;
                this.txtDireccion.Text = String.Empty;
                this.masterCiudad.Text = String.Empty;
                this.txtTelefono1.Text = String.Empty;
                this.txtTelefono2.Text = String.Empty;
                this.txtCelular1.Text = String.Empty;
                this.txtCelular2.Text = String.Empty;
                this.txtCorreo.Text = String.Empty;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "CleanData"));
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

                //Valida que el usuario exista
                if (!this.masterCliente.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCliente.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCliente.Focus();
                    return false;
                }

                //Valida que la Ciudad exista
                if (!this.masterCiudad.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCiudad.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterCiudad.Focus();
                    return false;
                }        

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "ValidateHeader"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario



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
                //this.txtLibranza.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "TBNew"));
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

                    int cambio = Convert.ToInt32(this.rbTipoCambio.EditValue);
                    this.cliente.DireccionAct.Value = this.txtDireccion.Text;
                    this.cliente.CiudadAct.Value = this.masterCiudad.Value;
                    this.cliente.Telefono1Act.Value = this.txtTelefono1.Text;
                    this.cliente.Telefono2Act.Value = this.txtTelefono2.Text;
                    this.cliente.Celular1Act.Value = this.txtCelular1.Text;
                    this.cliente.Celular2Act.Value = this.txtCelular2.Text;
                    this.cliente.CorreoAct.Value = this.txtCorreo.Text;

                    this.tercero= (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, cliente.ID.Value, true);
                    if (cambio == 1)
                    {
                        this.cliente.ResidenciaDir.Value = this.txtDireccion.Text;
                        this.cliente.ResidenciaCiudad.Value = this.masterCiudad.Value;
                        this.cliente.Telefono1.Value = this.txtTelefono1.Text;
                        this.cliente.Telefono2.Value = this.txtTelefono2.Text;
                        this.cliente.Celular1.Value = this.txtCelular1.Text;
                        this.cliente.Celular2.Value = this.txtCelular2.Text;
                        this.cliente.Correo.Value = this.txtCorreo.Text;

                        this.tercero.Direccion.Value = this.txtDireccion.Text;
                        this.tercero.LugarGeograficoID.Value = this.masterCiudad.Value;
                        this.tercero.Telefono1.Value = this.txtTelefono1.Text;
                        this.tercero.Telefono2.Value = this.txtTelefono2.Text;
                        this.tercero.Celular1.Value = this.txtCelular1.Text;
                        this.tercero.Celular2.Value = this.txtCelular2.Text;
                        this.tercero.CECorporativo.Value = this.txtCorreo.Text;

                    }
                    DTO_TxResult result = _bc.AdministrationModel.Cliente_CambioDatos(this._documentID, this.cliente, this.tercero);

                    
                    MessageForm frm = new MessageForm(result);
                    frm.ShowDialog();
                    if (result.Result == ResultValue.OK)
                    {
                        this.CleanData();
                        this.EnableHeader(false);
                        this.masterCliente.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CambioDatosCliente.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas
        /// <summary>
        /// Trae los datos de cliente y coTercero, para almacenarlos en los campos de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void masterCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._clienteID != this.masterCliente.Value)
                {
                    this._clienteID = this.masterCliente.Value;

                    this.cliente= (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                    
                    if (this.masterCliente.ValidID)
                    {
                        DTO_coTercero clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, cliente.TerceroID.Value, true);

                        this.EnableHeader(true);

                        this.txtDireccion.Text = cliente.DireccionAct.ToString();                        
                        this.txtDireccion.Text=this.cliente.DireccionAct.Value ;
                        this.masterCiudad.Value = cliente.CiudadAct.Value;
                        this.txtTelefono1.Text=this.cliente.Telefono1Act.Value;
                        this.txtTelefono2.Text=this.cliente.Telefono2Act.Value;
                        this.txtCelular1.Text=this.cliente.Celular1Act.Value;
                        this.txtCelular2.Text=this.cliente.Celular2Act.Value;
                        this.txtCorreo.Text=this.cliente.CorreoAct.Value;
                        this.rbTipoCambio.Focus();

                    }
                }
                else                
                {
                    //this.GetSaldosCedula(this.masterCliente.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudLibranza.cs", "masterCliente_Leave"));
            }

        }

        private void txtDireccion_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            CodificacionDireccion dir = new CodificacionDireccion((DevExpress.XtraEditors.ButtonEdit)sender);
            dir.ShowDialog();
        }

    }
}