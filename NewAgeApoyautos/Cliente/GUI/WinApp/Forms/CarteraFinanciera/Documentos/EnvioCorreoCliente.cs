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
using System.Threading;
using System.Windows.Forms;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EnvioCorreoCliente : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes frmType = FormTypes.Other;

        //Variables generales
        private string frmName;
        private int documentID;
        private ModulesPrefix frmModule;
        private DTO_glActividadFlujo actFlujo = new DTO_glActividadFlujo();
        private bool isModalFormOpened;

        //Variables de operación
        private List<DTO_CorreoCliente> _listCorreos = new List<DTO_CorreoCliente>();
        private List<string> _listMails = new List<string>();
        private string _asunto = string.Empty;
        private string _mensaje = string.Empty;

        #endregion

        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected virtual void SaveMethod() { this.CleanData(); }


        #endregion

        #region Constructor

        public EnvioCorreoCliente()
        {
            this.Constructor(null);
        }

        public EnvioCorreoCliente(string mod) 
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

                this.LoadClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "EnvioCorreoCliente"));
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
                this.documentID = AppForms.EnvioCorreoClientes;
                this.frmModule = ModulesPrefix.cc;

                //Carga la maestra de comprador de cartera
                this._bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, true);

                DateTime periodo = Convert.ToDateTime(_bc.GetControlValueByCompany(this.frmModule, AppControl.cc_Periodo));

                //if (DateTime.Now.Month != periodo.Month)
                //{
                //    this.dtFecha.Properties.MinValue = new DateTime(periodo.Year, periodo.Month, 1);
                //    this.dtFecha.Properties.MaxValue = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                //    this.dtFecha.DateTime = new DateTime(periodo.Year, periodo.Month, DateTime.DaysInMonth(periodo.Year, periodo.Month));
                //}
                //else
                //    this.dtFecha.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Limpia el formulario
        /// </summary>
        private void CleanData()
        {
            this._listCorreos = new List<DTO_CorreoCliente>();
            this.masterCliente.Value = String.Empty;
            this.chkCliente.Checked = true;
            this.chkConyuge.Checked = false;
            this.chkCodeudor.Checked = false;
            this.txtAsunto.Text = string.Empty;
            this.uc_Revelaciones.ValueHTML = string.Empty;
            this.btnDestinatarios.Enabled = false;
            this.LoadClientes();
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
            return true;
        }

        /// <summary>
        /// Carga los correos de los clientes que tengan saldos
        /// </summary>
        private void LoadClientes()
        {
            try
            {
                this._listCorreos = this._bc.AdministrationModel.GetCorreosCliente(this.masterCliente.Value, this.chkCliente.Checked, this.chkConyuge.Checked, this.chkCodeudor.Checked);
                if (this._listCorreos.Count > 0)
                    this.btnDestinatarios.Enabled = true;
                else
                    this.btnDestinatarios.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "masterTercero_Leave"));
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
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemSave.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemNew.Enabled = true;
                FormProvider.Master.itemSendtoAppr.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "Form_FormClosed"));
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
                if (this.masterCliente.ValidID)
                {
                    this.LoadClientes();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "masterTercero_Leave"));
            }
        }

        /// <summary>
        /// Evento que se ejecuta al momento de salir del componente de cartera
        /// </summary>
        private void btnDestinatarios_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int count = 0;
                foreach (var cli in this._listCorreos)
                    list.Add((count++).ToString(), cli);

                ModalStandar frm = new ModalStandar(this.documentID, list);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "SaveThread"));
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
                this.masterCliente.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "TBNew"));
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBUpdate()
        {

            try
            {
                this._listCorreos = this._bc.AdministrationModel.GetCorreosCliente(this.masterCliente.Value, this.chkCliente.Checked, this.chkConyuge.Checked, this.chkCodeudor.Checked);
                if (this._listCorreos.Count > 0)
                    this.btnDestinatarios.Enabled = true;
                else
                    this.btnDestinatarios.Enabled = false;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSendtoAppr()
        {
            try
            {
                if (this._listCorreos.Count > 0)
                {
                    this._asunto = this.txtAsunto.Text;
                    this._mensaje = this.uc_Revelaciones.ValueHTML;
                    if (this.masterCliente.ValidID)
                        this._listCorreos = this._listCorreos.FindAll(x=>x.ClienteID.Value == this.masterCliente.Value).ToList();
                    Thread process = new Thread(this.SendToApproveThread);
                    process.Start();
                }
                else
                {
                    MessageBox.Show("Lista de destinatarios vacía, verificar nuevamente");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-VentaCartera.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public virtual void SendToApproveThread()
        {
            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { AppDocuments.Nomina, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                DTO_TxResult result = new DTO_TxResult();
                #region Variables para el mail

                string filesPath = this._bc.GetControlValue(AppControl.RutaFisicaArchivos);
                string fileFormat = this._bc.GetControlValue(AppControl.NombreArchivoDocumentos);
                string docsPath = this._bc.GetControlValue(AppControl.RutaDocumentos);
                #endregion               
                foreach (DTO_CorreoCliente mail in this._listCorreos)
                {
                    #region Envia el correo con archivos adjuntos
                    this._bc.SendMail(this.documentID, this._asunto, this._mensaje, mail.Correo.Value);
                    #endregion
                }
              
                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-EnvioCorreoCliente.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this.documentID);
                this.Invoke(this.saveDelegate);
            }
        }

        #endregion

    
    }
}
