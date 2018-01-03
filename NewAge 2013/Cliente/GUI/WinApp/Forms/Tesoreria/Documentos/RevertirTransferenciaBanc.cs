using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using System.Threading;
using NewAge.DTO.Resultados;
using SentenceTransformer;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class RevertirTransferenciaBanc : FormWithToolbar
    {
        #region Delegados
        private delegate void RefresForm();
        private RefresForm refresForm;
        /// <summary>
        /// Delegado que actualiza el formulario despues de enviar un documento para aprobacion
        /// </summary>
        private void RefreshFormMethod() { this.CleanControls(); }
        #endregion

        #region Variables
        //Para uso general de los formularios
        private BaseController _bc = BaseController.GetInstance();
        private string _frmName;
        private int documentID;
        private ModulesPrefix frmModule;
        private string areaFuncionalID;
        private string prefijoID;
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();
        private DTO_Comprobante comprobante = null;

        #endregion     

        public RevertirTransferenciaBanc()
        {
            this.SetInitParameters();
          

            this.LoadDocumentInfo(true);
            this.InitControls();

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
            #endregion
        }

        #region Funciones Virtuales

        /// <summary>
        /// Limpia los controles de la aplicacion
        /// </summary>
        private void CleanControls()
        {
            this.masterTercero.Value = string.Empty;
            this.masterCuentaCxP.Value = string.Empty;
            this.masterCuentaBanco.Value = string.Empty;
            this.txtDocTercero.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtValor.EditValue = 0;
            this.comprobante = null;
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
        /// Inicializa los controles de la aplicacion
        /// </summary>
        private  void InitControls()
        {
            this._bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, false,true);
            this._bc.InitMasterUC(this.masterCuentaCxP, AppMasters.coPlanCuenta, true, true, true, false);
            this._bc.InitMasterUC(this.masterCuentaBanco, AppMasters.coPlanCuenta, true, true, true, false);
            this.masterCuentaBanco.EnableControl(false);
            this.masterCuentaCxP.EnableControl(false);
        }  

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        private void SetInitParameters()
        {
            this.InitializeComponent();
          
            this.frmModule = ModulesPrefix.ts;
            this.documentID = AppDocuments.RevertirTransferenciaBanc;
            this._frmName = _bc.GetResource(LanguageTypes.Forms, this.documentID.ToString());        
            this.InitControls();

            this.refresForm = new RefresForm(this.RefreshFormMethod);
        }    

        /// <summary>
        /// Carga la información de las grilla
        /// </summary>
        /// <param name="firstTime">Si es la primera vez que carga la grilla</param>
        /// <param name="refresh">Si debe volver a cargar la data del servidor</param>
        private void LoadData(bool firstTime = false)
        {
            try
            {
                DTO_glDocumentoControl ctrl = new DTO_glDocumentoControl();
                if (this.masterTercero.ValidID && !string.IsNullOrEmpty(this.txtDocTercero.Text))
                {
                    this.comprobante = this._bc.AdministrationModel.Comprobante_GetTransfBancariaByTercero(this.masterTercero.Value, this.txtDocTercero.Text);
                    if (this.comprobante != null)
                    {
                        this.txtDescripcion.Text = this.comprobante.Footer.First().Descriptivo.Value;
                        this.masterCuentaBanco.Value = this.comprobante.CuentaID;
                        this.masterCuentaCxP.Value = this.comprobante.Footer.First().CuentaID.Value;
                        this.txtValor.EditValue = this.comprobante.Footer.First().vlrMdaLoc.Value;
                    } 
                    else
                    {
                        MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages,DictionaryMessages.Cp_NoFact));
                        this.txtDescripcion.Text = string.Empty;
                        this.masterCuentaBanco.Value = string.Empty;
                        this.masterCuentaCxP.Value = string.Empty;
                        this.txtValor.EditValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirTransferenciaBanc.cs", "TBUpdate"));
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form_Enter(object sender, EventArgs e)
        {
            try
            {
                //Manejo de Botones de la Barra de Herramientas
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;             
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemUpdate.Visible = true;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemNew.Visible = true;
                FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this.documentID, FormsActions.Edit);
                FormProvider.Master.itemUpdate.Enabled = true;
                FormProvider.Master.itemNew.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DeclaracionImpuestos.cs", "Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Leave"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_Closing"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma se cierra
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        public void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), this.frmModule);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega", "Form_FormClosed"));
            }
        }

        #endregion

        #region Eventos Formulario

        /// <summary>
        /// Evento para lista los documetnos asociados por glDocumento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void lkpDocumentos_EditValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirTransferenciaBanc.cs", "lkpDocumentos_EditValueChanged"));
            }
        }

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
        /// Boton de Filtro de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirTransferenciaBanc.cs", "btnFilter_Cheked"));
            }
        }

        #endregion

        #region Eventos Barra de Herramientas


        /// <summary>
        /// Nuevo
        /// </summary>
        public override void TBNew()
        {
            this.CleanControls();
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                if (this.masterTercero.ValidID && this.comprobante != null && this.comprobante.Footer.Count > 0)
                {
                    Thread process = new Thread(this.SaveThread);
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirTransferenciaBanc.cs", "TBSave"));
            }
        }

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBUpdate()
        {
            try
            {
                this.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevertirTransferenciaBanc.cs", "TBUpdate"));
            }
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Guarda la información del proceso
        /// </summary>
        public void SaveThread()
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;

            try
            {
                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoOpConjuntas(this.documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this.documentID);

                var res = this._bc.AdministrationModel.TransferenciasBancarias_Revertir(this.documentID, this.comprobante);
                FormProvider.Master.StopProgressBarThread(this.documentID);

                bool isOK = _bc.SendDocumentMail(MailType.NewDoc, this.documentID, this._bc.AdministrationModel.User.ID.Value, res, true, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ActaEntrega.cs", "SaveThread"));
            }
            finally
            {
                if (result.Result == ResultValue.OK)
                    this.Invoke(this.refresForm);
                FormProvider.Master.StopProgressBarThread(this.documentID);
            }
        }

        #endregion                
    }
}