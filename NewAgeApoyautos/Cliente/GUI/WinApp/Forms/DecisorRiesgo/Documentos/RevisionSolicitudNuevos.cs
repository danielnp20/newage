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
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class RevisionSolicitudNuevos : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        //Identificador de la proxima actividad
        private string nextActID;

        //Variables formulario
        private DateTime periodo = DateTime.Now;

        private string _clienteID = String.Empty;
        private string _clienteDesc = String.Empty;

        private string _conyugueID = String.Empty;
        private string _conyugueDesc = String.Empty;

        private string _codeudor1ID = String.Empty;
        private string _codeudor1Desc = String.Empty;

        private string _codeudor2ID = String.Empty;
        private string _codeudor2Desc = String.Empty;

        private string _codeudor3ID = String.Empty;
        private string _codeudor3Desc = String.Empty;

        private bool _isLoaded;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public RevisionSolicitudNuevos()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public RevisionSolicitudNuevos(string cliente, int libranzaNro)
        {
            this.Constructor(cliente, libranzaNro);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string cliente = null, int libranzaNro = 0)
        {
            InitializeComponent();
            try
            {
                this.SetInitParameters();
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.dr;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);
                
                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.dr, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                
                this.LoadData(cliente, libranzaNro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "Constructor"));
            }
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            try
            {
                this._documentID = AppDocuments.RegistroSolicitud;
                this._frmModule = ModulesPrefix.dr;

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;

                this.linkConyugeRevision .Dock = DockStyle.Fill;
                this.linkCodeudor1Revision.Dock = DockStyle.Fill;                
                this.linkCodeudor2Revision.Dock = DockStyle.Fill;
                this.linkCodeudor3Revision.Dock = DockStyle.Fill;
                this.linkCodeudor3Revision.Dock = DockStyle.Fill;

                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;                               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {

            //Variables
            this._clienteID = String.Empty;
            this._clienteDesc = String.Empty;
            this._conyugueID = String.Empty;
            this._conyugueDesc = String.Empty;

            this._codeudor1ID = String.Empty;
            this._codeudor1Desc = String.Empty;

            this._codeudor2ID = String.Empty;
            this._codeudor2Desc = String.Empty;

            this._codeudor3ID = String.Empty;
            this._codeudor3Desc = String.Empty;

            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            //this.txtPriApellidoDeudor.ReadOnly = !enabled;
            //this.txtSdoApellidoDeudor.ReadOnly = !enabled;
            //this.txtPriNombreDeudor.ReadOnly = !enabled;
            //this.txtSdoNombreDeudor.ReadOnly = !enabled;

        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            #region Hace las Validaciones          

            //Valida que este escrito el primer apellido
            //if (String.IsNullOrEmpty(this.txtPriApellidoDeudor.Text))
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriApellido.Text);
            //    MessageBox.Show(msg);
            //    this.txtPriApellidoDeudor.Focus();
            //    return false;
            //}

            ////Valida que este escrito el Primer nombre
            //if (String.IsNullOrEmpty(this.txtPriNombre.Text))
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriNombre.Text);
            //    MessageBox.Show(msg);
            //    this.txtPriNombre.Focus();
            //    return false;
            //}

            ////Valida que la ciudad exista
            //if (!this.masterTerceroDocTipoDeudor.ValidID)
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTerceroDocTipoDeudor.LabelRsx);
            //    MessageBox.Show(msg);
            //    this.masterTerceroDocTipoDeudor.Focus();
            //    return false;
            //}

            ////Valida que este escrito el numero de Libranza
            //if (String.IsNullOrEmpty(this.txtCedulaDeudor.Text))
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lblTipoDoc.Text);
            //    MessageBox.Show(msg);
            //    this.txtCedulaDeudor.Focus();
            //    return false;
            //}



            ////Valida que la línea de crédito exista
            //if (!string.IsNullOrWhiteSpace(this.masterLineaCredito.Value) && !this.masterLineaCredito.ValidID)
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
            //    MessageBox.Show(msg);
            //    this.masterLineaCredito.Focus();
            //    return false;
            //}

            #endregion
            #region glDocumentoControl
            this._ctrl.PeriodoDoc.Value = this.periodo;
            this._ctrl.PeriodoUltMov.Value = this.periodo;
            this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion
            if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value.Value == 0)
            {
                //this._ctrl.DocumentoID.Value = this._documentID;
                //this._ctrl.NumeroDoc.Value = 0;
                //this._ctrl.FechaDoc.Value = DateTime.Now.Month == this.periodo.Month && DateTime.Now.Year == this.periodo.Year ? DateTime.Now : new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                //this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                //this._ctrl.Descripcion.Value = "Solicitud Crédito " + this.txtCedulaDeudor.Text;
                //this._ctrl.Fecha.Value = DateTime.Now;
                //this._ctrl.LugarGeograficoID.Value = this.masterTerceroDocTipoDeudor.Value;
                //this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
                //this._ctrl.PrefijoID.Value = this._bc.GetPrefijo(this._ctrl.AreaFuncionalID.Value, this._documentID);
                //this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                //this._ctrl.TasaCambioDOCU.Value = 0;
                //this._ctrl.TasaCambioCONT.Value = 0;
                //this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValorIngDeud.EditValue, CultureInfo.InvariantCulture);
                //this._ctrl.Iva.Value = 0;
                //this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            }
            #endregion           

            return true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void AssignValues(bool isGet)
        {
            DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
            DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
            DTO_drSolicitudDatosPersonales codeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
            DTO_drSolicitudDatosPersonales codeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);
            DTO_drSolicitudDatosPersonales codeudor3 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 5);
            //DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo.Find(x => x.NumeroDoc.Value == this._data.DocCtrl.NumeroDoc.Value);

            if (isGet) //Asigna datos a los controles
            {
                //Deudor (TipoPersona 1)
                if (deudor != null)
                {
                    _clienteID = deudor.NumeroDoc.Value.ToString();

                    #region Proceso 1                    
                    this.chkEmpleadoDeudor.Checked = false;
                    this.chkNingunaDeudor.Checked = false;
                    this.lblRegistradoDeudor.Text = "Ingresos Registrados" + deudor.IngresosREG.Value.ToString();
                    this.chkRegistradoDeudor.Checked = false;
                    this.lblSoportadoDeudor.Text = "Ingresos Soportados" + deudor.IngresosSOP.Value.ToString();
                    this.chkSoportadoDeudor.Checked = false;
                    this.txtDesc1Deudor.Text = "";
                    this.txtDesc2Deudor.Text = "";
                    this.txtDesc3Deudor.Text = "";
                    this.chkValidaDeudor.Checked = false;
                    this.lblInmuebleDeudor.Text = deudor.NroInmuebles.Value + " Inmuebles";
                    this.lblHipotecaDeudor.Text = deudor.NroInmuebles.Value + " Hipotecas";
                    this.lblRestriccionDeudor.Text = deudor.NroInmuebles.Value + " Restricciones";
                    this.chkInmuebleDeudor.Checked = false;
                    this.chkRestriccionDeudor.Checked = false;
                    this.chkRestriccionDeudor.Checked = false;
                    #endregion

                    #region Proceso 2
                    this.chkNombreDeudor.Checked = false;
                    this.chkFotcopiaCedulaDeudor.Checked = false;
                    this.chkCamaraDeudor.Checked = false;
                    this.chkCamaraVigenteDeudor.Checked = false;
                    #endregion
                }                
                //Conyuge (TipoPersona 2)
                if (conyuge != null)
                {
                    _conyugueID = conyuge.NumeroDoc.Value.ToString();                    

                    #region Proceso 1
                    
                    this.chkEmpleadoCony.Checked = false;
                    this.chkNingunaCony.Checked = false;
                    this.lblRegistradoCony.Text = "Ingresos Registrados" + conyuge.IngresosREG.Value.ToString();
                    this.chkRegistradoCony.Checked = false;
                    this.lblSoportadoCony.Text = "Ingresos Soportados" + conyuge.IngresosSOP.Value.ToString();
                    this.chkSoportadoCony.Checked = false;
                    this.txtDesc1Cony.Text = "";
                    this.txtDesc2Cony.Text = "";
                    this.txtDesc3Cony.Text = "";
                    this.chkValidaCony.Checked = false;
                    this.lblInmuebleCony.Text = conyuge.NroInmuebles.Value + " Inmuebles";
                    this.lblHipotecaCony.Text = conyuge.NroInmuebles.Value + " Hipotecas";
                    this.lblRestriccionCony.Text = conyuge.NroInmuebles.Value + " Restricciones";
                    this.chkInmuebleCony.Checked = false;
                    this.chkRestriccionCony.Checked = false;
                    this.chkRestriccionCony.Checked = false;
                    #endregion

                    #region Proceso 2
                    this.chkNombreCony.Checked = false;
                    this.chkFotcopiaCedulaCony.Checked = false;
                    this.chkCamaraCony.Checked = false;
                    this.chkCamaraVigenteCony.Checked = false;
                    #endregion

                }
                //Codeudor1 (TipoPersona 3)
                if (codeudor1 != null)
                {
                    _codeudor1ID = codeudor1.NumeroDoc.Value.ToString();                    

                    #region Proceso 1                    
                    this.chkEmpleadoCod1.Checked = false;
                    this.chkNingunaCod1.Checked = false;
                    this.lblRegistradoCod1.Text = "Ingresos Registrados" + codeudor1.IngresosREG.Value.ToString();
                    this.chkRegistradoCod1.Checked = false;
                    this.lblSoportadoCod1.Text = "Ingresos Soportados" + codeudor1.IngresosSOP.Value.ToString();
                    this.chkSoportadoCod1.Checked = false;
                    this.txtDesc1Cod1.Text = "";
                    this.txtDesc2Cod1.Text = "";
                    this.txtDesc3Cod1.Text = "";
                    this.chkValidaCod1.Checked = false;
                    this.lblInmuebleCod1.Text = codeudor1.NroInmuebles.Value + " Inmuebles";
                    this.lblHipotecaCod1.Text = codeudor1.NroInmuebles.Value + " Hipotecas";
                    this.lblRestriccionCod1.Text = codeudor1.NroInmuebles.Value + " Restricciones";
                    this.chkInmuebleCod1.Checked = false;
                    this.chkRestriccionCod1.Checked = false;
                    this.chkRestriccionCod1.Checked = false;
                    #endregion

                    #region Proceso 2
                    this.chkNombreCod1.Checked = false;
                    this.chkFotcopiaCedulaCod1.Checked = false;
                    this.chkCamaraCod1.Checked = false;
                    this.chkCamaraVigenteCod1.Checked = false;
                    #endregion

                }
                //Codeudor2 (TipoPersona 4)
                if (codeudor2 != null)
                {
                    _codeudor2ID = codeudor2.NumeroDoc.Value.ToString();                    

                    #region Proceso 1                    
                    this.chkEmpleadoCod2.Checked = false;
                    this.chkNingunaCod2.Checked = false;
                    this.lblRegistradoCod2.Text = "Ingresos Registrados" + codeudor2.IngresosREG.Value.ToString();
                    this.chkRegistradoCod2.Checked = false;
                    this.lblSoportadoCod2.Text = "Ingresos Soportados" + codeudor2.IngresosSOP.Value.ToString();
                    this.chkSoportadoCod2.Checked = false;
                    this.txtDesc1Cod2.Text = "";
                    this.txtDesc2Cod2.Text = "";
                    this.txtDesc3Cod2.Text = "";
                    this.chkValidaCod2.Checked = false;
                    this.lblInmuebleCod2.Text = codeudor2.NroInmuebles.Value + " Inmuebles";
                    this.lblHipotecaCod2.Text = codeudor2.NroInmuebles.Value + " Hipotecas";
                    this.lblRestriccionCod2.Text = codeudor2.NroInmuebles.Value + " Restricciones";
                    this.chkInmuebleCod2.Checked = false;
                    this.chkRestriccionCod2.Checked = false;
                    this.chkRestriccionCod2.Checked = false;
                    #endregion

                    #region Proceso 2
                    this.chkNombreCod2.Checked = false;
                    this.chkFotcopiaCedulaCod2.Checked = false;
                    this.chkCamaraCod2.Checked = false;
                    this.chkCamaraVigenteCod2.Checked = false;
                    #endregion
                }
                //Codeudor3 (TipoPersona 5)2
                if (codeudor3 != null)
                {
                    _codeudor3ID = codeudor3.NumeroDoc.Value.ToString();
                    

                    #region Proceso 1
                    
                    this.chkEmpleadoCod3.Checked = false;
                    this.chkNingunaCod3.Checked = false;
                    this.lblRegistradoCod3.Text = "Ingresos Registrados" + codeudor3.IngresosREG.Value.ToString();
                    this.chkRegistradoCod3.Checked = false;
                    this.lblSoportadoCod3.Text = "Ingresos Soportados" + codeudor3.IngresosSOP.Value.ToString();
                    this.chkSoportadoCod3.Checked = false;
                    this.txtDesc1Cod3.Text = "";
                    this.txtDesc2Cod3.Text = "";
                    this.txtDesc3Cod3.Text = "";
                    this.chkValidaCod3.Checked = false;
                    this.lblInmuebleCod3.Text = codeudor3.NroInmuebles.Value + " Inmuebles";
                    this.lblHipotecaCod3.Text = codeudor3.NroInmuebles.Value + " Hipotecas";
                    this.lblRestriccionCod3.Text = codeudor3.NroInmuebles.Value + " Restricciones";
                    this.chkInmuebleCod3.Checked = false;
                    this.chkRestriccionCod3.Checked = false;
                    this.chkRestriccionCod3.Checked = false;
                    #endregion

                    #region Proceso 2
                    this.chkNombreCod3.Checked = false;
                    this.chkFotcopiaCedulaCod3.Checked = false;
                    this.chkCamaraCod3.Checked = false;
                    this.chkCamaraVigenteCod3.Checked = false;
                    #endregion
                }
                //if (vehiculo != null)
                //{
                //    // Datos Vehiculo
                //    #region datos Vehiculo
                //    this.chkvehiculo.Checked = false;
                //    #endregion
                //}
            }
            else //Llena datos de los controles para salvar
            {
                #region Deudor (TipoPersona 1)
                DTO_drSolicitudDatosPersonales deudorNew = new DTO_drSolicitudDatosPersonales();
                
  
                if (deudor == null)
                    this._data.DatosPersonales.Add(deudorNew);
                else
                    deudor = deudorNew;
                #endregion
                #region Conyuge (TipoPersona 2)
                DTO_drSolicitudDatosPersonales conyugeNew = new DTO_drSolicitudDatosPersonales();
       
                if (conyuge == null)
                    this._data.DatosPersonales.Add(conyugeNew);
                else
                    conyuge = conyugeNew;
                #endregion

                #region Codeudor1 (TipoPersona 3)
                DTO_drSolicitudDatosPersonales cod1New = new DTO_drSolicitudDatosPersonales();

                if (codeudor1 == null)
                    this._data.DatosPersonales.Add(cod1New);
                else
                    codeudor1 = cod1New;
                #endregion

                #region Codeudor2 (TipoPersona 4)
                DTO_drSolicitudDatosPersonales cod2New = new DTO_drSolicitudDatosPersonales();

                if (codeudor2 == null)
                    this._data.DatosPersonales.Add(cod2New);
                else
                    codeudor2 = cod2New;
                #endregion

                #region Codeudor3 (TipoPersona 5)
                DTO_drSolicitudDatosPersonales cod3New = new DTO_drSolicitudDatosPersonales();


                if (codeudor3 == null)
                    this._data.DatosPersonales.Add(cod3New);
                else
                    codeudor3 = cod3New;
                #endregion


            //    #region datos Vehiculo
            //    DTO_drSolicitudDatosVehiculo vehiculoNew = new DTO_drSolicitudDatosVehiculo();

               
            //    if (vehiculo == null)
            //        this._data.DatosVehiculo.Add(vehiculoNew);
            //    else
            //        vehiculo = vehiculoNew;
            //    #endregion
            }
          }
        private void LoadData(string cliente, int libranzaNro)
        {
            this._isLoaded = false;
            this._data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(libranzaNro);

            if (this._data != null)
            {
                #region Solicitud existente
                this._ctrl = this._data.DocCtrl;

                if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)                  
                {
                    this.AssignValues(true);
                    this.EnableHeader(true);
                }

                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAprobada));
                    CleanData();

                    this.chkEmpleadoDeudor.Focus();
                }
                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                    CleanData();

                    this.chkEmpleadoDeudor.Focus();
                }

                #endregion
            }
            else
            {
                this.CleanData();
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
                FormProvider.Master.itemSearch.Visible = false;
                FormProvider.Master.itemNew.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                if (FormProvider.Master.LoadFormTB)
                {
                    FormProvider.Master.itemSave.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Delete);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento que revisa que la libranza sea numerica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLibranza_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Siguiente
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e"><Objeto que envia el evento/param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
            {
                this.tabControl.SelectedTabPageIndex = 1;
                this.lblTitulo1.Visible = true;
                this.lblTitulo2.Visible = false;
                this.lblTitulo3.Visible = false;
                this.lblTitulo4.Visible = false;
                this.lblTitulo5.Visible = false;
                this.lblTitulo6.Visible = false;
                this.lblTitulo1.Text = "Proceso de revisión1";                
            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
                this.tabControl.SelectedTabPageIndex = 2;                
                this.lblTitulo1.Visible = true;
                this.lblTitulo2.Visible = true;
                this.lblTitulo3.Visible = false;
                this.lblTitulo4.Visible = false;
                this.lblTitulo5.Visible = false;
                this.lblTitulo6.Visible = false;
                this.lblTitulo1.Text = "Proceso de revisión1";
                this.lblTitulo2.Text = "-Deudor:" + _clienteDesc+ "cc:" + _clienteID;
                if (_conyugueID != string.Empty)
                    this.lblTitulo3.Text = "-Conyugue:" +_conyugueDesc+ "cc:"+_conyugueID;
                if (_codeudor1ID != string.Empty)
                    this.lblTitulo4.Text = "-Cod1:" + _codeudor1Desc + "cc:" + _codeudor1ID;
                if (_codeudor2ID != string.Empty)
                    this.lblTitulo5.Text = "-Cod2:" + _codeudor2Desc + "cc:" + _codeudor2ID;
                if (_codeudor3ID != string.Empty)
                    this.lblTitulo6.Text = "-Cod3:" + _codeudor3Desc + "cc:" + _codeudor3ID;

            }
            else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 0;
        }

        /// <summary>
        /// Atras
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="e"><Objeto que envia el evento/param>
        private void btnAtras_Click(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
                this.tabControl.SelectedTabPageIndex = 2;
            else if (this.tabControl.SelectedTabPageIndex == 1)
                this.tabControl.SelectedTabPageIndex = 0;
            else if (this.tabControl.SelectedTabPageIndex == 2)
                this.tabControl.SelectedTabPageIndex = 1;
        }

        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor1.Dock == DockStyle.Fill)
                linkCodeudor1.Dock = DockStyle.None;
            else
                linkCodeudor1.Dock = DockStyle.Fill;
        }

        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor2.Dock == DockStyle.Fill)
                linkCodeudor2.Dock = DockStyle.None;
            else
                linkCodeudor2.Dock = DockStyle.Fill;
        }

        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor3.Dock == DockStyle.Fill)
                linkCodeudor3.Dock = DockStyle.None;
            else
                linkCodeudor3.Dock = DockStyle.Fill;
        }

        private void linkConyuge_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyuge.Dock == DockStyle.Fill)
                this.linkConyuge.Dock = DockStyle.None;
            else
                this.linkConyuge.Dock = DockStyle.Fill;
        }

        private void linkConyugeInmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyugeRevision.Dock == DockStyle.Fill)
                this.linkConyugeRevision.Dock = DockStyle.None;
            else
                this.linkConyugeRevision.Dock = DockStyle.Fill;
        }

        private void linkCodeudor1Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor1Revision.Dock == DockStyle.Fill)
                this.linkCodeudor1Revision.Dock = DockStyle.None;
            else
                this.linkCodeudor1Revision.Dock = DockStyle.Fill;
        }

        private void linkCodeudor2Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor2Revision.Dock == DockStyle.Fill)
                this.linkCodeudor2Revision.Dock = DockStyle.None;
            else
                this.linkCodeudor2Revision.Dock = DockStyle.Fill;
        }

        private void linkCodeudor3Inmueble_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkCodeudor3Revision.Dock == DockStyle.Fill)
                this.linkCodeudor3Revision.Dock = DockStyle.None;
            else
                this.linkCodeudor3Revision.Dock = DockStyle.Fill;
        }

        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (this.tabControl.SelectedTabPageIndex == 0)
            {
             
            }
            else if (this.tabControl.SelectedTabPageIndex == 1)
            {
             
            }
            else if (this.tabControl.SelectedTabPageIndex == 2)
            {
             
            }

        }

        #endregion Eventos Formulario

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        public override void TBSave()
        {
            try
            {
                if (this.ValidateHeader())
                {
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();

                    if (this._ctrl.NumeroDoc.Value.HasValue && this._ctrl.NumeroDoc.Value != 0)
                    {
                        //string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        //string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "La edición de la solicitud eliminará los datos de preliquidación si existen, desea continuar");
                        //if (MessageBox.Show(msgDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                        //    return;
                    }

                    DTO_TxResult result = _bc.AdministrationModel.SolicitudLibranza_Add(this._documentID, solLibranza);
                    if (result.Result == ResultValue.OK)
                    {
                        //Actualiza el control para las financieras
                        string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                        if (sectorLib == ((byte)SectorCartera.Financiero).ToString()) //Financieras
                        {
                            string numeroControl = _bc.AdministrationModel.Empresa.NumeroControl.Value;
                            _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numeroControl).ToList();
                        }

                        this.CleanData();
                        this.chkEmpleadoDeudor.Focus();

                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
                        MessageBox.Show(string.Format(msg, result.ExtraField));
                    }
                    else
                    {
                        MessageForm frm = new MessageForm(result);
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-RevisionSolicitudNuevos.cs", "TBSave"));
            }
        }


        #endregion Eventos Barra Herramientas

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void lblNombreCony_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
    }
}
