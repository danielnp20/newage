using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using NewAge.DTO.Negocio;
using DevExpress.Data;
using NewAge.DTO.Resultados;
using System.Reflection;
using NewAge.DTO.Attributes; 
using NewAge.DTO.UDT;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RechazoCredito : ProcessForm
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        private string libranzaID;
        private DTO_ccCreditoDocu credito;

        #endregion

        public RechazoCredito()
        {
            //InitializeComponent();
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.documentID = AppDocuments.RechazoCredito;

            this.InitializeComponent();
            FormProvider.LoadResources(this, this.documentID);

            //Carga la info inicial de los controles (centro de pago y periodo)
            _bc.InitMasterUC(this.masterBanco, AppMasters.tsBancosCuenta, true, true, true, false);
            _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, true, true, true, false);
            this.masterBanco.EnableControl(true);
            this.masterCliente.EnableControl(false);

            //Periodo
            _bc.InitPeriodUC(this.dtPeriod, 0);
            string periodoStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
            this.dtPeriod.DateTime = Convert.ToDateTime(periodoStr);
            this.dtFecha.DateTime = DateTime.Now.Date;

            if (this.dtPeriod.DateTime.Month != this.dtFecha.DateTime.Month || this.dtPeriod.DateTime.Year != this.dtFecha.DateTime.Year)
            {
                DateTime fechaIni = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, 1);
                DateTime fechaFin = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month));

                this.dtFecha.Properties.MinValue = fechaIni;
                this.dtFecha.Properties.MaxValue = fechaFin;
                this.dtFecha.DateTime = fechaFin;
            }

            bool cierreValido = true;
            int diaCierre = 1;
            string indCierreDiaStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd);
            if (indCierreDiaStr == "1")
            {
                string diaCierreStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
                if (string.IsNullOrWhiteSpace(diaCierreStr) || diaCierreStr == "0")
                    diaCierreStr = "1";

                diaCierre = Convert.ToInt16(diaCierreStr);
                if (diaCierre > DateTime.DaysInMonth(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month))
                {
                    cierreValido = false;
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_DiaCerrado));
                }
            }

            if(!cierreValido)
            {
                this.masterBanco.EnableControl(false);
                this.txtLibranza.Enabled = false;
                this.btnProcesar.Enabled = false;
            }
        }

        #endregion

        #region Funciones Privadas

        private void CleanData()
        {
            try
            {
                this.libranzaID = string.Empty;
                this.credito = null;

                this.txtLibranza.Text = string.Empty;
                this.masterBanco.Value = string.Empty;
                this.masterCliente.Value = string.Empty;
                this.txtVlrGiro.EditValue = 0;
                this.txtVlrPrestamo.EditValue = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RechazoCredito.cs", "CleanData"));
            }
        }

        /// <summary>
        /// Valida la información antes de ejecutar el proceso
        /// </summary>
        private bool ValidateData()
        {
            try 
            { 
                //Valida que el crédito exista
                if (string.IsNullOrEmpty(this.txtLibranza.Text) || string.IsNullOrWhiteSpace(this.libranzaID))
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblLibranza.Text);
                    MessageBox.Show(msg);
                    this.txtLibranza.Focus();
                    return false;
                }

                //Valida el banco
                if (!this.masterBanco.ValidID)
                {
                    string msg = string.Format(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterBanco.LabelRsx);
                    MessageBox.Show(msg);
                    this.masterBanco.Focus();
                    return false;
                }

                DTO_tsBancosCuenta banco = (DTO_tsBancosCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.tsBancosCuenta, false, this.masterBanco.Value, true);
                DTO_coDocumento docBanco = (DTO_coDocumento)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coDocumento, false, banco.coDocumentoID.Value, true);
                if (string.IsNullOrWhiteSpace(docBanco.CuentaLOC.Value))
                {
                    string msg = string.Format(this._bc.GetResourceError(DictionaryMessages.Err_Co_DocNoCta), banco.coDocumentoID.Value);
                    MessageBox.Show(msg);
                    this.masterBanco.Focus();
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RechazoCredito.cs", "ValidateData"));
                return false;
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento que permite crear, habilitar o deshabilitar las propiedades del documento con base a la Libranza  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.libranzaID != this.txtLibranza.Text.Trim() && !String.IsNullOrWhiteSpace(this.txtLibranza.Text))
                {
                    string tmp = this.txtLibranza.Text;
                    int libranzaTmp = Convert.ToInt32(this.txtLibranza.Text.Trim());
                    this.credito = _bc.AdministrationModel.GetCreditoByLibranza(libranzaTmp);

                    if (this.credito != null)
                    {
                        if (credito.DocRechazo.Value.HasValue)
                        {
                            this.CleanData();
                            this.txtLibranza.Text = tmp;
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoRechazado);
                            MessageBox.Show(msg);

                            return;
                        }

                        DTO_glDocumentoControl ctrl = _bc.AdministrationModel.glDocumentoControl_GetByID(this.credito.NumeroDoc.Value.Value);
                        if(ctrl.Estado.Value != (byte)EstadoDocControl.Aprobado)
                        {
                            this.CleanData();
                            this.txtLibranza.Text = tmp;
                            string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_CreditoNoAprob);
                            MessageBox.Show(msg);

                            return;
                        }

                        this.libranzaID = this.txtLibranza.Text.Trim();

                        // Asigna los valores del crédito
                        this.masterCliente.Value = this.credito.ClienteID.Value;
                        this.txtVlrGiro.EditValue = this.credito.VlrGiro.Value;
                        this.txtVlrPrestamo.EditValue = this.credito.VlrPrestamo.Value;
                    }
                    else
                    {
                        string libTemp = this.libranzaID;
                        this.CleanData();
                        this.txtLibranza.Text = libTemp;
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaNoExiste);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCredito.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Evento que se encarga de verificar las inconsistencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
            if(this.ValidateData())
            {
                DTO_TxResult result = _bc.AdministrationModel.Credito_Rechazo(this.documentID, this._actFlujo.ID.Value, this.credito, this.dtFecha.DateTime, this.masterBanco.Value);
                MessageForm frm = new MessageForm(result);
                frm.ShowDialog();

                if (result.Result == ResultValue.OK)
                    this.CleanData();
            }
        }

        #endregion
    }
}
