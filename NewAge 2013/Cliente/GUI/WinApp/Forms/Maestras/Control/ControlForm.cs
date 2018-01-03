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
using DevExpress.XtraTab;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.ControlsUC;
using DevExpress.XtraEditors;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    /// <summary>
    /// Formulario maestro para la tabla de control
    /// </summary>
    public partial class ControlForm : FormWithToolbar
    {
        #region Variables
        //Para uso general de los formularios
        BaseController _bc = BaseController.GetInstance();
        protected int userID = 0;
        //Para manejo de propiedades
        protected string EmpresaNro = string.Empty;
        protected int DocumentID;
        protected ModulesPrefix FrmModule;
        protected bool isFinanciera = false;
        //Internas del formulario
        private Dictionary<string, string> _ctrlValues = new Dictionary<string, string>();
        private string _modId;
        private string _ctrlPrefix = "ctrl_";
        private string _lblPrefix = "lbl_";
        private FormTypes _frmType = FormTypes.Control;
        private string _frmName;

        #endregion

        ///<summary>
        /// Constructor 
        /// </summary>
        public ControlForm()
        {
            try
            {
                InitializeComponent();

                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this.DocumentID.ToString());
                ModulesPrefix mod = this.isFinanciera ? ModulesPrefix.cf : this.FrmModule;
                FormProvider.Master.Form_Load(this, mod, this.DocumentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                this.LoadControlInfo();
                this.AfterInitialize();

                this._modId = ((int)this.FrmModule).ToString();
                if (this._modId.Length == 1)
                    this._modId = "0" + this._modId;
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "ControlForm"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Carga la informacion de la tabla glControl en sus respectivos controles
        /// </summary>
        private void LoadControlInfo()
        {
            this.LoopControls(true);
        }

        /// <summary>
        /// Recorre todos los controles de las pestañas
        /// </summary>
        /// <param name="read">Indica si se esta consultando (true) o  se esta escribiendo</param>
        private bool LoopControls(bool read)
        {
            bool result = true;
            
            try
            {
                this._ctrlValues = new Dictionary<string, string>();
                int index = 0;
                foreach (XtraTabPage tab in this.tcControl.TabPages)
                {
                    if (tab.PageVisible)
                    {
                        foreach (Control c in tab.Controls)
                        {
                            if (read)
                                this.SetControlData(index, c.Controls);
                            else
                            {
                                bool temp = this.GetControlData(index, c.Controls);
                                if (!temp)
                                    return false;
                            }
                        }
                    }
                    index++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "LoopControls"));
                return false;
            }

            return result;
        }

        /// <summary>
        /// Lee los valores de la tabla de control y los asigna al formulario
        /// </summary>
        /// <param name="read">Indica si se esta consultando (3) o  se esta escribiendo</param>
        /// <param name="tabIndex">Indice de la pestaña que se esta consultando</param>
        /// <param name="ctrls">Lista de controles</param>
        private bool SetControlData(int tabIndex, Control.ControlCollection ctrls)
        {
            try
            {
                if (ctrls.Owner.Name.StartsWith(this._ctrlPrefix))
                    this.SetControlFormValue(ctrls.Owner);

                //Controles del formulario
                if (ctrls.Count > 0)
                {
                    foreach (Control c in ctrls)
                    {
                        if (c.Name.StartsWith(this._ctrlPrefix))
                            this.SetControlFormValue(c);

                        if (c.Controls.Count > 0)
                            this.SetControlData(tabIndex, c.Controls);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "SetControlData"));
                return false;
            }
        }

        /// <summary>
        /// Asigna el valor de los controles del formulario a la tabla de control
        /// </summary>
        /// <param name="read">Indica si se esta consultando (true) o  se esta escribiendo</param>
        /// <param name="tabIndex">Indice de la pestaña que se esta consultando</param>
        /// <param name="ctrls">Lista de controles</param>
        private bool GetControlData(int tabIndex, Control.ControlCollection ctrls)
        {
            try
            {
                string msgEmptyField = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField);
                string msg_FkNotFound = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.FkNotFound);
                bool result = true;

                if (ctrls.Owner.Name.StartsWith(this._ctrlPrefix))
                    result = this.GetControlFormValues(ctrls.Owner, tabIndex, msgEmptyField, msg_FkNotFound);

                //Controles del formulario
                if (ctrls.Count > 0)
                {
                    foreach (Control c in ctrls)
                    {
                        if (c.Name.StartsWith(this._ctrlPrefix))
                            result = this.GetControlFormValues(c, tabIndex, msgEmptyField, msg_FkNotFound);

                        if (c.Controls.Count > 0)
                            result = this.GetControlData(tabIndex, c.Controls);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "GetControlData"));
                return false;
            }
        }

        /// <summary>
        /// Asigna a un control los datos traidos de la tabla de control
        /// Read: true
        /// </summary>
        /// <param name="c">Control</param>
        /// <returns>Retorna si el control es valido</returns>
        private void SetControlFormValue(Control c)
        {
            try
            {
                string rsxField = string.Empty;
                string ctrlName = c.Name;
                string ctrlID = ctrlName.Substring(this._ctrlPrefix.Length);

                string ctrlValue = this.FrmModule == ModulesPrefix.gl ? _bc.GetControlValue(Convert.ToInt32(ctrlID), true) : _bc.GetControlValueByCompany(this.FrmModule, ctrlID, true);

                //Para cualquier modulo oculta el control del periodo
                if (ctrlID == "001")
                    c.Enabled = false;

                if (c is uc_MasterFind)
                {
                    uc_MasterFind ctrl = (uc_MasterFind)c;
                    if (!string.IsNullOrWhiteSpace(ctrlValue))
                        ctrl.Value = ctrlValue;
                }
                else if (c is uc_PeriodoEdit)
                {
                    uc_PeriodoEdit ctrl = (uc_PeriodoEdit)c;
                    if (!string.IsNullOrWhiteSpace(ctrlValue))
                    {
                        string[] p = ctrlValue.Split('/');
                        int d = Convert.ToInt32(p[2]);
                        int m = Convert.ToInt32(p[1]);
                        int y = Convert.ToInt32(p[0]);
                        ctrl.DateTime = new DateTime(y, m, d);
                    }
                }
                else if (c is TextBox)
                {
                    TextBox ctrl = (TextBox)c;
                    c.Text = ctrlValue;
                }
                else if (c is TextEdit)
                {
                    TextEdit ctrl = (TextEdit)c;
                    c.Text = ctrlValue;
                }
                else if (c is CheckBox)
                {
                    CheckBox ctrl = (CheckBox)c;
                    ctrl.Checked = ctrlValue == "1" ? true : false;
                }
                else if (c is DateEdit)
                {
                    DateEdit ctrl = (DateEdit)c;
                    try
                    {
                        ctrl.DateTime = Convert.ToDateTime(ctrlValue);
                    }
                    catch (Exception e)
                    {
                        ctrl.DateTime = DateTime.Now;
                    }
                }
                else if (c is ComboBoxEdit)
                {
                    ComboBoxEdit ctrl = (ComboBoxEdit)c;
                    ctrl.Text = ctrlValue;
                }
                else if (c is ComboBoxEx)
                {
                    ComboBoxEx ctrl = (ComboBoxEx)c;
                    ctrl.SelectedItem =ctrl.GetItem(ctrlValue);//.Value;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "SetControlFormValue"));
            }
        }

        /// <summary>
        /// Lee los valores de un control y los asigna al diccionario para salvar
        /// Read: false
        /// </summary>
        /// <param name="c">Control</param>
        /// <param name="tabIndex">Indice de la pestaña donde esta el control</param>
        /// <param name="msgEmptyField">Mensaje de campo vacio</param>
        /// <param name="msg_FkNotFound">Mensaje de llave foranea no encontrada</param>
        /// <returns>Retorna si el control es valido</returns>
        private bool GetControlFormValues(Control c, int tabIndex, string msgEmptyField, string msg_FkNotFound)
        {
            try
            {
                string ctrlValue = string.Empty;
                string rsxField = string.Empty;
                string ctrlName = c.Name;
                string ctrlID = ctrlName.Substring(this._ctrlPrefix.Length);
                string glCtrlID = this.FrmModule == ModulesPrefix.gl ? ctrlID : this.EmpresaNro + this._modId + ctrlID;

                if (c is uc_MasterFind)
                {
                    uc_MasterFind ctrl = (uc_MasterFind)c;
                    if (!string.IsNullOrWhiteSpace(ctrl.Value) && !ctrl.ValidID)
                    {
                        Control[] coll = this.Controls.Find(this._lblPrefix + ctrlID, true);
                        Label lbl = (Label)coll.First();
                        MessageBox.Show(string.Format(msg_FkNotFound, lbl.Text));
                        this.tcControl.SelectedTabPageIndex = tabIndex;
                        return false;
                    }
                    this._ctrlValues.Add(glCtrlID, ctrl.Value);
                }
                else if (c is uc_PeriodoEdit)
                {
                    uc_PeriodoEdit ctrl = (uc_PeriodoEdit)c;
                    this._ctrlValues.Add(glCtrlID, ctrl.DateTime.ToString(FormatString.ControlDate));
                }
                else if (c is TextBox)
                {
                    TextBox ctrl = (TextBox)c;
                    this._ctrlValues.Add(glCtrlID, c.Text);
                }
                else if (c is TextEdit)
                {
                    TextEdit ctrl = (TextEdit)c;
                    if (ctrl.EditValue == null)
                        this._ctrlValues.Add(glCtrlID, "0");
                    else
                    {
                        string val = ctrl.Properties.Mask.EditMask == "P" && string.IsNullOrWhiteSpace(ctrl.EditValue.ToString()) ? "0" : ctrl.EditValue.ToString();
                        this._ctrlValues.Add(glCtrlID, val);
                    }
                }
                else if (c is CheckBox)
                {
                    CheckBox ctrl = (CheckBox)c;
                    string val = ctrl.Checked ? "1" : "0";
                    this._ctrlValues.Add(glCtrlID, val);
                }
                else if (c is DateEdit)
                {
                    DateEdit ctrl = (DateEdit)c;
                    string val = ctrl.DateTime.ToString(FormatString.ControlDate);
                    this._ctrlValues.Add(glCtrlID, val);
                }
                else if (c is ComboBoxEdit)
                {
                    ComboBoxEdit ctrl = (ComboBoxEdit)c;
                    this._ctrlValues.Add(glCtrlID, ctrl.Text);
                }
                else if (c is ComboBoxEx)
                {
                    ComboBoxEx ctrl = (ComboBoxEx)c;

                    if (ctrl.SelectedItem != null)
                    {
                        ComboBoxItem item = (ComboBoxItem)ctrl.SelectedItem;
                        this._ctrlValues.Add(glCtrlID, item.Value);
                    }
                    else
                        this._ctrlValues.Add(glCtrlID, string.Empty);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "GetControlFormValues"));
                return false;
            }
        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected virtual void SetInitParameters()
        {
            //Inicia las variables del formulario
            this.EmpresaNro = _bc.AdministrationModel.Empresa.NumeroControl.Value;
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected virtual void AfterInitialize() { }

        /// <summary>
        /// Valida el formulario del control
        /// </summary>
        protected virtual bool ValidateControl() { return true; }

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
                ModulesPrefix mod = this.isFinanciera ? ModulesPrefix.cf : this.FrmModule; 
                FormProvider.Master.Form_Enter(this, this.DocumentID, this._frmType, mod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "Form_Enter"));
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
                FormProvider.Master.Form_Leave(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "Form_Leave"));
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
                FormProvider.Master.Form_Closing(this, this.DocumentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "Form_Closing"));
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
                ModulesPrefix mod = this.isFinanciera ? ModulesPrefix.cf : this.FrmModule;
                FormProvider.Master.Form_FormClosed(this._frmName, this.GetType(), mod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ControlForm.cs", "Form_FormClosed"));
            }
        }

        #endregion

        #region Barra de Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                bool result = this.LoopControls(false);
                if (result)
                {
                    bool isValid = this.ValidateControl();
                    if (isValid)
                    {
                        _bc.AdministrationModel.glControl_UpdateModuleData(this._ctrlValues);
                        _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, _bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();

                        this.LoadControlInfo();
                        MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.ResultOK));
                    }
                }
            }
            catch (Exception ex)
            {
                string rsx = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_UpdateData);
                MessageBox.Show(rsx);
            }
        }

        #endregion

    }
}
