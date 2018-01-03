using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using NewAge.DTO.Negocio;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.Librerias.ExceptionHandler;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Threading;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario de documentos
    /// </summary>
    public partial class ReclasificacionSaldos : FormWithToolbar
    {
        #region Delegados

        protected delegate void Save();
        protected Save saveDelegate;
        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected void SaveMethod()
        {
            this.ResetControls();
        }

        #endregion

        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        //Variables Privadas
        private FormTypes _frmType = FormTypes.Document;
        private DTO_glDocumentoControl _docControl = null;
        private string _frmName;
        private bool _useQuery;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _actFlujoID = string.Empty;

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ReclasificacionSaldos()
        {
            this.InitializeComponent();
            try
            {
                this._documentID = AppDocuments.ReclasificacionSaldos;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.co;
                FormProvider.LoadResources(this, this._documentID);
                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                //Inicia los controles de usuario maestras
                _bc.InitMasterUC(this.masterDocument, AppMasters.glDocumento, true, true, true, false);
                _bc.InitMasterUC(this.masterPrefijo, AppMasters.glPrefijo, true, true, true, false);
                _bc.InitMasterUC(this.masterTercero, AppMasters.coTercero, true, true, true, false);
                _bc.InitMasterUC(this.masterCuenta, AppMasters.coPlanCuenta, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroCosto, AppMasters.coCentroCosto, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroCostoEdit, AppMasters.coCentroCosto, true, true, true, false);
                _bc.InitMasterUC(this.masterProyecto, AppMasters.coProyecto, true, true, true, false);
                _bc.InitMasterUC(this.masterProyectoEdit, AppMasters.coProyecto, true, true, true, false);
                _bc.InitMasterUC(this.masterLugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
                _bc.InitMasterUC(this.masterLugarGeoEdit, AppMasters.glLugarGeografico, true, true, true, false);
                this.periodoEdit.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                this.saveDelegate = new Save(this.SaveMethod);

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                }
                else
                {
                    this._actFlujoID = actividades[0];
                    this.ChangeStatusControls(1);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "ReclasificacionSaldos.cs-ReclasificacionSaldos"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Cambia estado de los controles segun indice
        /// </summary>
        /// <param name="index">Indice a cambiar estado (1 - Interno / 2 - Externo)</param>
        private void ChangeStatusControls(int index)
        {
            switch (index)
            {
                case 1:
                    #region Doc Interno
                    this.masterPrefijo.EnableControl(true);
                    this.masterTercero.EnableControl(false);
                    this.txtNumDocInterno.Enabled = true;
                    this.txtDocExterno.Enabled = false;

                    this.masterTercero.Value = string.Empty;
                    this.txtDocExterno.Text = string.Empty;
                    #endregion
                    break;
                case 2:
                    #region Doc Externo
                    this.masterPrefijo.EnableControl(false);
                    this.masterTercero.EnableControl(true);
                    this.txtNumDocInterno.Enabled = false;
                    this.txtDocExterno.Enabled = true;

                    this.masterPrefijo.Value = string.Empty;
                    this.txtNumDocInterno.Text = string.Empty;
                    #endregion
                    break;
                default:
                    #region Error - sin seleccion
                    this.masterPrefijo.EnableControl(false);
                    this.masterTercero.EnableControl(false);
                    this.txtNumDocInterno.Enabled = false;
                    this.txtDocExterno.Enabled = false;

                    this.masterTercero.Value = string.Empty;
                    this.txtDocExterno.Text = string.Empty;
                    this.masterPrefijo.Value = string.Empty;
                    this.txtNumDocInterno.Text = string.Empty;
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// Obtiene un documento interno
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentInt()
        {
            try
            {
                string prefijo = this.masterPrefijo.Value;
                int docId = Convert.ToInt32(this.masterDocument.Value);
                int numDoc = Convert.ToInt32(this.txtNumDocInterno.Text);
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetInternalDoc(docId, prefijo, numDoc);
                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene un documento externo
        /// </summary>
        /// <returns>un dto de gDocumentoControl</returns>
        private DTO_glDocumentoControl GetDocumentExt()
        {
            try
            {
                string tercero = this.masterTercero.Value;
                int docId = Convert.ToInt32(this.masterDocument.Value);
                string docExt = this.txtDocExterno.Text;
                DTO_glDocumentoControl doc = _bc.AdministrationModel.glDocumentoControl_GetExternalDoc(docId, tercero, docExt);

                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Resetea los controles al confirmar una nueva consulta
        /// </summary>
        private void ResetControls()
        {
            this.masterDocument.Value = string.Empty;
            this.masterPrefijo.Value = string.Empty;
            this.masterTercero.Value = string.Empty;
            this.masterCuenta.Value = string.Empty;
            this.masterCentroCosto.Value = string.Empty;
            this.masterCentroCostoEdit.Value = string.Empty;
            this.masterProyecto.Value = string.Empty;
            this.masterProyectoEdit.Value = string.Empty;
            this.masterLugarGeo.Value = string.Empty;
            this.masterLugarGeoEdit.Value = string.Empty;
            this.txtDocExterno.Text = string.Empty;
            this.txtNumDocInterno.Text = string.Empty;
            this.txtObservacion.Text = string.Empty;
            this.txtSaldos.Text = string.Empty;
            this.gctrlHeader.Enabled = true;
            this.gbQuery.Enabled = false;
            this.txtSaldosExtr.Enabled = false;
            this.lblMonedaExtr.Visible = false;
            this.lblMonedaLocal.Text = string.Empty;
            this.lblMonedaExtr.Text = string.Empty;

            FormProvider.Master.itemSearch.Enabled = true;
            FormProvider.Master.itemSave.Enabled = false;
            this._useQuery = false;
        }

        /// <summary>
        /// Asigna los valores a los campos de detalle
        /// </summary>
        private void AssignValues()
        {
            //Trae valores por defecto
            string centroCostoDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);
            string proyectoDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            string lugarGeogDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);
            
            //Valida los campos de la cuenta para mostrar
            DTO_coPlanCuenta dtoCuenta = (DTO_coPlanCuenta)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coPlanCuenta, false, this._docControl.CuentaID.Value, true);
            if (dtoCuenta != null)
            {
                if (dtoCuenta.CentroCostoInd.Value.Value)
                {
                    centroCostoDef = this._docControl.CentroCostoID.Value;
                    this.masterCentroCostoEdit.EnableControl(true);
                }
                else
                    this.masterCentroCostoEdit.EnableControl(false);

                if (dtoCuenta.ProyectoInd.Value.Value)
                {
                    proyectoDef = this._docControl.ProyectoID.Value;
                    this.masterProyectoEdit.EnableControl(true);
                }
                else
                    this.masterProyectoEdit.EnableControl(false);
                
                if (dtoCuenta.LugarGeograficoInd.Value.Value)
                {
                    lugarGeogDef = this._docControl.LugarGeograficoID.Value;
                    this.masterLugarGeoEdit.EnableControl(true);
                }
                else
                    this.masterLugarGeoEdit.EnableControl(false);
            }

            //Muestra los valores correspondientes en el detalle
            this.periodoEdit.DateTime = this._docControl.PeriodoDoc.Value.Value;
            this.masterCuenta.Value = this._docControl.CuentaID.Value;
            this.masterCentroCosto.Value = centroCostoDef;
            this.masterCentroCostoEdit.Value = centroCostoDef;
            this.masterProyecto.Value = proyectoDef;
            this.masterProyectoEdit.Value = proyectoDef;
            this.masterLugarGeo.Value = lugarGeogDef;
            this.masterLugarGeoEdit.Value = lugarGeogDef;


            this.GetSaldos(dtoCuenta);
            this.txtObservacion.Text = this._docControl.Observacion.Value;
            
            this._useQuery = true;
           
            //Administra los controles correspondientes
            this.gbQuery.Enabled = true;
            this.gctrlHeader.Enabled = false;
            FormProvider.Master.itemSearch.Enabled = false;
            FormProvider.Master.itemSave.Enabled = true;
           
        }

        /// <summary>
        /// Obtiene los saldos del documento actual
        /// </summary>
        /// <param name="dtoCuenta">Cuenta para obtener saldos</param>
        private void GetSaldos(DTO_coPlanCuenta dtoCuenta)
        {
            DTO_coCuentaSaldo saldoDoc = null;
            try
            {  
                //Trae la cuenta para obtener saldos
                if (dtoCuenta != null)
                    saldoDoc = _bc.AdministrationModel.Saldo_GetByDocumento(dtoCuenta.ID.Value, dtoCuenta.ConceptoSaldoID.Value, this._docControl.NumeroDoc.Value.Value, string.Empty);

                if (saldoDoc == null)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));
                    return;
                }

                //Revisa el concepto de saldo
                DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, false, dtoCuenta.ConceptoSaldoID.Value, true);
                if (cSaldo.coSaldoControl.Value.Value != (byte)SaldoControl.Doc_Externo && cSaldo.coSaldoControl.Value.Value != (byte)SaldoControl.Doc_Interno)
                {
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Co_DocNotBalance));
                    return;
                }

                string monedaLocal = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string monedaExtranjera = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaExtranjera);

                //Trae el valor de la moneda local
                decimal sumaML = saldoDoc.DbOrigenLocML.Value.Value + saldoDoc.DbOrigenExtML.Value.Value + saldoDoc.CrOrigenLocML.Value.Value + saldoDoc.CrOrigenExtML.Value.Value
                    + saldoDoc.DbSaldoIniLocML.Value.Value + saldoDoc.DbSaldoIniExtML.Value.Value + saldoDoc.CrSaldoIniLocML.Value.Value + saldoDoc.CrSaldoIniExtML.Value.Value;
                    
                this.lblMonedaLocal.Text = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_lblMonedaLocal");
                this.lblMonedaLocal.Text += "(" + monedaLocal + ")";
                this.txtSaldos.Text = Math.Round(sumaML, 2).ToString();

                //Si es multimoneda trae y muestra los valores extranjeros
                if (_bc.AdministrationModel.MultiMoneda)
                {
                    decimal sumaME = saldoDoc.DbOrigenLocME.Value.Value + saldoDoc.DbOrigenExtME.Value.Value + saldoDoc.CrOrigenLocME.Value.Value + saldoDoc.CrOrigenExtME.Value.Value
                        + saldoDoc.DbSaldoIniLocME.Value.Value + saldoDoc.DbSaldoIniExtME.Value.Value + saldoDoc.CrSaldoIniLocME.Value.Value + saldoDoc.CrSaldoIniExtME.Value.Value;
                        
                    this.lblMonedaExtr.Text = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_lblMonedaExtr");
                    this.lblMonedaExtr.Text += "(" + monedaExtranjera + ")";
                    this.txtSaldosExtr.Visible = true;
                    this.txtSaldosExtr.Text = Math.Round(sumaME, 2).ToString();
                    this.lblMonedaExtr.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionSaldos.cs", "GetSaldos"));
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
                FormProvider.Master.Form_Enter(this, this._documentID, this._frmType, this._frmModule);
                
                FormProvider.Master.itemDelete.Visible = false;
                FormProvider.Master.tbBreak.Visible = false;
                FormProvider.Master.itemPrint.Visible = false;
                FormProvider.Master.itemFilter.Visible = false;
                FormProvider.Master.itemFilterDef.Visible = false;
                FormProvider.Master.tbBreak0.Visible = false;
                FormProvider.Master.itemGenerateTemplate.Visible = false;
                FormProvider.Master.itemCopy.Visible = false;
                FormProvider.Master.itemPaste.Visible = false;
                FormProvider.Master.itemImport.Visible = false;
                FormProvider.Master.itemExport.Visible = false;
                FormProvider.Master.tbBreak1.Visible = false;
                FormProvider.Master.itemRevert.Visible = false;
                FormProvider.Master.itemSendtoAppr.Visible = false;
                FormProvider.Master.itemSearch.Visible = true;
                

                if (FormProvider.Master.LoadFormTB)
                {
                    if (FormProvider.Master.itemSave.Enabled)
                        FormProvider.Master.itemSearch.Enabled = false;
                    else
                        FormProvider.Master.itemSearch.Enabled = true;

                    FormProvider.Master.itemSave.Enabled = false;
                    FormProvider.Master.itemSearch.Enabled = true;
                    FormProvider.Master.itemGenerateTemplate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentForm.cs-Form_Enter"));
            }
        }

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected virtual void Form_Leave(object sender, EventArgs e)
        {
            try
            {
                FormProvider.Master.Form_Leave(this, this._documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentForm.cs-Form_Leave"));
            }
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentForm.cs-Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp", "DocumentForm.cs-Form_FormClosed"));
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
                DialogResult Result = DialogResult.Yes;
                if (this._useQuery)
                {
                    Result = MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, "Desea cambiar la consulta actual sin guardar?"), this._frmName, MessageBoxButtons.YesNo);
                }
                if (Result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.ResetControls();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                Thread process = new Thread(this.SaveThread);
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Boton para buscar saldos
        /// </summary>
        public override void TBSearch()
        {
            try
            {
                string msg = string.Empty;

                if (this.masterDocument.ValidID)
                {
                    if (rbtPrefijo.Checked)
                    {
                        if (this.masterPrefijo.ValidID)
                        {
                            if (!string.IsNullOrEmpty(this.txtNumDocInterno.Text))
                            {
                                this._docControl = this.GetDocumentInt();
                                if (this._docControl != null)
                                    this.AssignValues();
                                else
                                {
                                    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                    MessageBox.Show(msg);
                                }
                            }
                            else
                            {
                                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                                this.txtNumDocInterno.Focus();
                            }
                        }
                        else
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            MessageBox.Show(string.Format(msg, this.masterPrefijo.LabelRsx));
                            this.masterPrefijo.Focus();
                        }
                    }
                    else if (rbtTercero.Checked)
                    {
                        if (this.masterTercero.ValidID)
                        {
                            if (!string.IsNullOrEmpty(this.txtDocExterno.Text))
                            {
                                this._docControl = this.GetDocumentExt();
                                if (this._docControl != null)
                                    this.AssignValues();
                                else
                                {
                                    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                    MessageBox.Show(msg);
                                }
                            }
                            else
                            {
                                msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound);
                                MessageBox.Show(msg);
                                this.txtDocExterno.Focus();
                            }
                        }
                        else
                        {
                            msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                            MessageBox.Show(string.Format(msg, this.masterTercero.LabelRsx));
                            this.masterTercero.Focus();
                        }
                    }
                }
                else
                {
                    msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
                    MessageBox.Show(string.Format(msg, this.masterDocument.LabelRsx));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionSaldos.cs", "TBSearch"));
            }
         }           

        #endregion

        #region Eventos Header

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtPrefijo_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(1);
        }

        /// <summary>
        /// revisa el estado del control
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void rbtTercero_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeStatusControls(2);
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
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

                FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { this._documentID, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
                FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCont(this._documentID));

                ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
                FormProvider.Master.ProgressBarThread = new Thread(pth);
                FormProvider.Master.ProgressBarThread.Start(this._documentID);

                result = _bc.AdministrationModel.Comprobante_ReclasificacionSaldos(this._documentID, this._actFlujoID, _docControl.NumeroDoc.Value.Value,
                    this.masterProyectoEdit.Value,this.masterCentroCostoEdit.Value,this.masterLugarGeoEdit.Value,this.txtObservacion.Text);
                FormProvider.Master.StopProgressBarThread(this._documentID);

                MessageForm frm = new MessageForm(result);
                this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm }); 

                if(result.Result == ResultValue.OK)
                    this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ReclasificacionSaldos.cs", "SaveThread"));
            }
            finally
            {
                FormProvider.Master.StopProgressBarThread(this._documentID);
            }
        
        }

        #endregion

    }
}
