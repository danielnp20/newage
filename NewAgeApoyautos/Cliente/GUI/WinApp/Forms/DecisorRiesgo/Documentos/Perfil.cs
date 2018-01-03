
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
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class Perfil : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();
        protected int documentReportID = 0;
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
        private string _clienteID = String.Empty;
        private string _pagaduriaID = String.Empty;
        private string _libranzaID = String.Empty;
        private string _centroPagoID = String.Empty;
        private string _tipoCreditoID = string.Empty;

        //Datos por defecto
        private string _asesorXDef = String.Empty;
        private string _comercioXDef = String.Empty;
        private string _lugGeoXDef = String.Empty;
        private string _centroPagoXDef = String.Empty;
        private string _pagaduriaXDef = String.Empty;
        private Dictionary<string, string> actFlujoForDevolucion = new Dictionary<string, string>();
        private List<string> actividadesFlujo = new List<string>();

        //Variables ToolBar
        private bool _isLoaded;
        private bool _readOnly = false;

        private DateTime periodo = DateTime.Now;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Perfil()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public Perfil(string cliente, int libranzaNro, bool readOnly)
        {
            this.Constructor(cliente, libranzaNro, readOnly);
        }

        /// <summary>
        /// Contrustor del formulario
        /// </summary>
        public void Constructor(string cliente = null, int libranzaNro = 0,bool readOnly = false)
        {
            InitializeComponent();
            try
            {   
                this.SetInitParameters();
                this.groupControl5.Visible = false;
                this._frmName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());
                this._frmModule = ModulesPrefix.dr;

                FormProvider.Master.Form_Load(this, this._frmModule, this._documentID, this._frmName, this.Form_Enter, this.Form_Leave, this.Form_FormClosing, this.Form_FormClosed);

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);

                if (this._frmModule == ModulesPrefix.dr && actividades.Count > 0)
                {
                    this.nextActID = string.Empty;
                    string actividadFlujoID = actividades[0];
                    this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                }
                else
                {
                    #region Carga la info de la proxima actividad
                    List<string> NextActs = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(AppDocuments.SolicitudLibranza);
                    if (NextActs.Count != 1)
                    {
                        string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                        MessageBox.Show(string.Format(msg, AppDocuments.SolicitudLibranza.ToString()));
                        this.EnableHeader(false);
                    }
                    else
                    {
                        this.nextActID = NextActs[0];
                        //string actividadFlujoID = actividades[0];
                        //this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                    }
                    #endregion
                }
                #endregion
                #region Carga Actividades a devolver
                //Carga la actividades a revertir
                List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this._actFlujo.ID.Value);
                this.actFlujoForDevolucion.Add("Ninguna", "Devolver Flujo");
                foreach (DTO_glActividadFlujo act in actPadres)
                {
                    this.actividadesFlujo.Add(act.ID.Value);
                    this.actFlujoForDevolucion.Add(act.ID.Value, act.Descriptivo.Value);
                }
                //Carga el rescurso de Anulado
                //string msgAnulado = this._bc.GetResource(LanguageTypes.Tables, DictionaryTables.Doc_EstateAnulado);
                //msgAnulado = msgAnulado.ToUpper();

                ////Agrega el texto de anulado
                //this.actividadesFlujo.Add(msgAnulado);
                //this.actFlujoForReversion.Add("Anular", msgAnulado);

                this.cmbActividades.Properties.DataSource = this.actFlujoForDevolucion;
                this.cmbActividades.EditValue = "Ninguna";
                #endregion
                this.LoadData(cliente, libranzaNro);
                this._readOnly = readOnly;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "Constructor"));
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
                this._documentID = AppQueries.QueryPerfil;
                this._frmModule = ModulesPrefix.dr;

                //Carga la informacion de la maestras
                this._bc.InitMasterUC(this.masterTerceroDocTipoDeudor, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCony, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod1, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod2, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterTerceroDocTipoCod3, AppMasters.coTerceroDocTipo, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalDeudor, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCony, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod1, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod2, AppMasters.coActEconomica, true, true, true, false);
                this._bc.InitMasterUC(this.masterActEconPrincipalCod3, AppMasters.coActEconomica, true, true, true, false);

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                //Asesor
                this._asesorXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);
                string asesorInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoDeAsesores);

                //this.masterComercio
                this._comercioXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PuntoComercialPorDefecto);
                string comercioInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoPuntosComerciales);

                //Lugar Geográfico
                this._lugGeoXDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Centro de Pago
                this._centroPagoXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);

                //Pagaduría
                this._pagaduriaXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PagaduriaXDefecto);

                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;
                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;                
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {

            this.txtPriApellidoDeudor.Text = String.Empty;
            this.txtSdoApellidoDeudor.Text = String.Empty;
            this.txtPriNombreDeudor.Text = String.Empty;
            this.txtSdoNombreDeudor.Text = String.Empty;
            this.txtValorIngDeud.Text = "0";
            this.masterTerceroDocTipoDeudor.Value = this._lugGeoXDef;

            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._data = new DTO_DigitaSolicitudDecisor();

            //Variables
            this._clienteID = String.Empty;
            this._pagaduriaID = String.Empty;
            this._centroPagoID = String.Empty;
            this._tipoCreditoID = string.Empty;

            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            this.txtPriApellidoDeudor.ReadOnly = !enabled;
            this.txtSdoApellidoDeudor.ReadOnly = !enabled;
            this.txtPriNombreDeudor.ReadOnly = !enabled;
            this.txtSdoNombreDeudor.ReadOnly = !enabled;

        }

        ///// <summary>
        ///// Verifica que los campos obligatorios esten bn
        ///// </summary>
        ///// <returns></returns>
        //private bool ValidateData()
        //{
        //    string result = string.Empty;
        //    string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
        //    string msgnoCoincide = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoCoincide);

        //    #region Hace las Validaciones de la tabla drSolicitudDatosPersonales

        //    #region Datos Deudor
        //    if (string.IsNullOrEmpty(this.txtPriApellidoDeudor.Text))
        //    {
        //        result += string.Format(msgVacio, this.lblPriApellido.Text + " Deudor: ") + "\n";
        //        this.txtPriApellidoDeudor.Focus();
        //    }
        //    if (string.IsNullOrEmpty(this.txtPriNombreDeudor.Text))
        //    {
        //        result += string.Format(msgVacio, this.lblPriNombre.Text + " Deudor: ") + "\n";
        //        this.txtPriNombreDeudor.Focus();
        //    }
        //    if (!this.masterTerceroDocTipoDeudor.ValidID)
        //    {
        //        result += string.Format(msgVacio, this.lblTipoDoc.Text + " Deudor: ") + "\n";
        //        this.masterTerceroDocTipoDeudor.Focus();
        //    }
        //    if (string.IsNullOrEmpty(this.dtFechaExpDeudor.Text))
        //    {
        //        result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Deudor: ") + "\n";
        //        this.dtFechaExpDeudor.Focus();
        //    }
        //    if (string.IsNullOrEmpty(this.txtCedulaDeudor.Text))
        //    {
        //        result += string.Format(msgVacio, this.lblNroDocumento.Text + " Deudor: ") + "\n";
        //        this.txtCedulaDeudor.Focus();
        //    }
        //    if (string.IsNullOrEmpty(this.dtVigenciaFMIDeudor.Text))
        //    {
        //        result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Deudor: ") + "\n";
        //        this.dtVigenciaFMIDeudor.Focus();
        //    }
        //    if (!this.masterActEconPrincipalDeudor.ValidID)
        //    {
        //        result += string.Format(msgVacio, this.lblActEconomica1.Text + " Deudor: ") + "\n";
        //        this.masterActEconPrincipalDeudor.Focus();
        //    }

        //    #endregion
        //    if (!string.IsNullOrEmpty(this.txtCedulaCony.Text))
        //    {
        //        #region Datos Conyugue

        //        if (string.IsNullOrEmpty(this.txtPriApellidoCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriApellido.Text + " Conyugue: ") + "\n";
        //            this.txtPriApellidoCony.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtPriNombreCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriNombre.Text + " Conyugue: ") + "\n";
        //            this.txtPriNombreCony.Focus();
        //        }
        //        if (!this.masterTerceroDocTipoCony.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblTipoDoc.Text + " Conyugue: ") + "\n";
        //            this.masterTerceroDocTipoCony.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaExpCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Conyugue: ") + "\n";
        //            this.dtFechaExpCony.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtCedulaCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblNroDocumento.Text + " Conyugue: ") + "\n";
        //            this.txtCedulaCony.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaNacCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Conyugue: ") + "\n";
        //            this.dtFechaNacCony.Focus();
        //        }
                
        //        if (!this.masterActEconPrincipalCony.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblActEconomica1.Text + " Conyugue: ") + "\n";
        //            this.masterActEconPrincipalCony.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtCorreoCony.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblCorreo.Text + " Conyugue: ") + "\n";
        //            this.txtCorreoCony.Focus();
        //        }

        //        if (string.IsNullOrEmpty(this.masterCiudadCony.Value))
        //        {
        //            result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Conyugue: ") + "\n";
        //            this.masterCiudadCony.Focus();
        //        }
        //        #endregion
        //    }
        //    if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
        //    {
        //        #region Datos Codeudor1
        //        if (string.IsNullOrEmpty(this.txtPriApellidoCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor1: ") + "\n";
        //            this.txtPriApellidoCod1.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtPriNombreCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor1: ") + "\n";
        //            this.txtPriNombreCod1.Focus();
        //        }
        //        if (!this.masterTerceroDocTipoCod1.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor1: ") + "\n";
        //            this.masterTerceroDocTipoCod1.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaExpCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor1: ") + "\n";
        //            this.dtFechaExpCod1.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtCedulaCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor1: ") + "\n";
        //            this.txtCedulaCod1.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaNacCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor1: ") + "\n";
        //            this.dtFechaNacCod1.Focus();
        //        }
                
        //        if (!this.masterActEconPrincipalCod1.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor1: ") + "\n";
        //            this.masterActEconPrincipalCod1.Focus();
        //        }
                
        //        if (string.IsNullOrEmpty(this.txtCorreoCod1.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor1: ") + "\n";
        //            this.txtCorreoCod1.Focus();
        //        }

        //        if (string.IsNullOrEmpty(this.masterCiudadCod1.Value))
        //        {
        //            result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor1: ") + "\n";
        //            this.masterCiudadCod1.Focus();
        //        }
        //        #endregion
        //    }
        //    if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
        //    {
        //        #region Datos Codeudor2
        //        if (string.IsNullOrEmpty(this.txtPriApellidoCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor2: ") + "\n";
        //            this.txtPriApellidoCod2.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtPriNombreCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor2: ") + "\n";
        //            this.txtPriNombreCod2.Focus();
        //        }
        //        if (!this.masterTerceroDocTipoCod2.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor2: ") + "\n";
        //            this.masterTerceroDocTipoCod2.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaExpCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor2: ") + "\n";
        //            this.dtFechaExpCod2.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtCedulaCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor2: ") + "\n";
        //            this.txtCedulaCod2.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaNacCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor2: ") + "\n";
        //            this.dtFechaNacCod2.Focus();
        //        }
                
        //        if (!this.masterActEconPrincipalCod2.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor2: ") + "\n";
        //            this.masterActEconPrincipalCod2.Focus();
        //        }
               

        //        if (string.IsNullOrEmpty(this.txtCorreoCod2.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor2: ") + "\n";
        //            this.txtCorreoCod2.Focus();
        //        }

        //        if (string.IsNullOrEmpty(this.masterCiudadCod2.Value))
        //        {
        //            result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor2: ") + "\n";
        //            this.masterCiudadCod2.Focus();
        //        }
        //        #endregion
        //    }
        //    if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
        //    {
        //        #region Datos Codeudor3
        //        if (string.IsNullOrEmpty(this.txtPriApellidoCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor3: ") + "\n";
        //            this.txtPriApellidoCod3.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtPriNombreCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor3: ") + "\n";
        //            this.txtPriNombreCod3.Focus();
        //        }
        //        if (!this.masterTerceroDocTipoCod3.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor3: ") + "\n";
        //            this.masterTerceroDocTipoCod3.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaExpCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaExpedicion.Text + " Codeudor3: ") + "\n";
        //            this.dtFechaExpCod3.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.txtCedulaCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor3: ") + "\n";
        //            this.txtCedulaCod3.Focus();
        //        }
        //        if (string.IsNullOrEmpty(this.dtFechaNacCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblFechaNacimiento.Text + " Codeudor3: ") + "\n";
        //            this.dtFechaNacCod3.Focus();
        //        }
                
        //        if (!this.masterActEconPrincipalCod3.ValidID)
        //        {
        //            result += string.Format(msgVacio, this.lblActEconomica1.Text + " Codeudor3: ") + "\n";
        //            this.masterActEconPrincipalCod3.Focus();
        //        }
                
        //        if (string.IsNullOrEmpty(this.txtCorreoCod3.Text))
        //        {
        //            result += string.Format(msgVacio, this.lblCorreo.Text + " Codeudor3: ") + "\n";
        //            this.txtCorreoCod3.Focus();
        //        }

        //        if (string.IsNullOrEmpty(this.masterCiudadCod3.Value))
        //        {
        //            result += string.Format(msgVacio, this.lblCiudadResidencia.Text + " Codeudor3: ") + "\n";
        //            this.masterCiudadCod3.Focus();
        //        }
        //        #endregion
        //    }

        //    if (string.IsNullOrEmpty(result))
        //        return true;
        //    else
        //    {
        //        MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
        //        return false;
        //    }
        //    #endregion

        //    #region glDocumentoControl
        //    this._ctrl.PeriodoDoc.Value = this.periodo;
        //    this._ctrl.PeriodoUltMov.Value = this.periodo;
        //    this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion
        //    if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value.Value == 0)
        //    {
        //        this._ctrl.DocumentoID.Value = this._documentID;
        //        this._ctrl.NumeroDoc.Value = 0;
        //        this._ctrl.FechaDoc.Value = DateTime.Now.Month == this.periodo.Month && DateTime.Now.Year == this.periodo.Year ? DateTime.Now : new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
        //        this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
        //        this._ctrl.Descripcion.Value = "Solicitud Crédito " + this.txtCedulaDeudor.Text;
        //        this._ctrl.Fecha.Value = DateTime.Now;
        //        this._ctrl.LugarGeograficoID.Value = this.masterTerceroDocTipoDeudor.Value;
        //        this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
        //        this._ctrl.PrefijoID.Value = this._bc.GetPrefijo(this._ctrl.AreaFuncionalID.Value, this._documentID);
        //        this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
        //        this._ctrl.TasaCambioDOCU.Value = 0;
        //        this._ctrl.TasaCambioCONT.Value = 0;
        //        this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValorIngDeud.EditValue, CultureInfo.InvariantCulture);
        //        this._ctrl.Iva.Value = 0;
        //        this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
        //    }
        //    #endregion           

        //    return true;
        //}

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            string result = string.Empty;
            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            #region Hace las Validaciones
            #region Datos Deudor
            //if (string.IsNullOrEmpty(this.txtCedulaDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Deudor: ") + "\n";
            //    this.txtCedulaDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtPriApellidoDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblPriApellido.Text + " Deudor: ") + "\n";
            //    this.txtPriApellidoDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtPriNombreDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblPriNombre.Text + " Deudor: ") + "\n";
            //    this.txtPriNombreDeudor.Focus();
            //}
            //if (!this.masterTerceroDocTipoDeudor.ValidID)
            //{
            //    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Deudor: ") + "\n";
            //    this.masterTerceroDocTipoDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.dtFechaExpDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblFechaExp.Text + " Deudor: ") + "\n";
            //    this.dtFechaExpDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.dtFechaNacDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblFechaNac.Text + " Deudor: ") + "\n";
            //    this.dtFechaNacDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtCelular1Deudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblCelular1.Text + " Deudor: ") + "\n";
            //    this.txtCelular1Deudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtCorreoDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblEmail.Text + " Deudor: ") + "\n";
            //    this.txtCorreoDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtDireccionDeudor.EditValue.ToString()))
            //{
            //    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Deudor: ") + "\n";
            //    this.txtDireccionDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.masterCiudadResDeudor.Value))
            //{
            //    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Deudor: ") + "\n";
            //    this.masterCiudadResDeudor.Focus();
            //}
            //if (string.IsNullOrEmpty(this.txtTelResidenciaDeudor.Text))
            //{
            //    result += string.Format(msgVacio, this.lblTelResid.Text + " Deudor:") + "\n";
            //    this.txtTelResidenciaDeudor.Focus();
            //}
            //if (!string.IsNullOrEmpty(this.masterCiudadResDeudor.Value) && !this.masterCiudadResDeudor.ValidID)
            //{
            //    result += string.Format("La ciudad residencia del Deudor no es válida, verifique") + "\n";
            //    this.masterCiudadResDeudor.Focus();
            //}
            //if (!string.IsNullOrEmpty(this.masterTerceroDocTipoDeudor.Value) && !this.masterTerceroDocTipoDeudor.ValidID)
            //{
            //    result += string.Format("El Tipo de documento del Deudor no es válido, verifique") + "\n";
            //    this.masterTerceroDocTipoDeudor.Focus();
            //}
            #endregion

            if (!string.IsNullOrEmpty(this.txtCedulaCod1.Text))
            {
                #region Datos CoDeudor 1        
                //if (String.IsNullOrEmpty(this.txtCedulaCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 1: ") + "\n";
                //    this.txtCedulaCod1.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriApellidoCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 1: ") + "\n";
                //    this.txtPriApellidoCod1.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriNombreCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 1: ") + "\n";
                //    this.txtPriNombreCod1.Focus();
                //}
                //if (!this.masterTerceroDocTipoCod1.ValidID)
                //{
                //    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 1: ") + "\n";
                //    this.masterTerceroDocTipoCod1.Focus();
                //}

                //if (string.IsNullOrEmpty(this.dtFechaExpCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 1:") + "\n";
                //    this.dtFechaExpCod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.dtFechaNacCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 1: ") + "\n";
                //    this.dtFechaNacCod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCelular1Cod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 1: ") + "\n";
                //    this.txtCelular1Cod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCorreoCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 1: ") + "\n";
                //    this.txtCorreoCod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtDireccionCod1.EditValue.ToString()))
                //{
                //    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 1: ") + "\n";
                //    this.txtDireccionCod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.masterCiudadResCod1.Value))
                //{
                //    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 1: ") + "\n";
                //    this.masterCiudadResCod1.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtTelResidenciaCod1.Text))
                //{
                //    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 1:") + "\n";
                //    this.txtTelResidenciaCod1.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterCiudadResCod1.Value) && !this.masterCiudadResCod1.ValidID)
                //{
                //    result += string.Format("La ciudad residencia del Codeudor 1 no es válida, verifique") + "\n";
                //    this.masterCiudadResCod1.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod1.Value) && !this.masterTerceroDocTipoCod1.ValidID)
                //{
                //    result += string.Format("El Tipo de documento del Codeudor 1 no es válido, verifique") + "\n";
                //    this.masterTerceroDocTipoCod1.Focus();
                //}
                #endregion
            }

            if (!string.IsNullOrEmpty(this.txtCedulaCod2.Text))
            {
                #region Datos CoDeudor 2  
                //if (String.IsNullOrEmpty(this.txtCedulaCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 2: ") + "\n";
                //    this.txtCedulaCod2.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriApellidoCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 2: ") + "\n";
                //    this.txtPriApellidoCod2.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriNombreCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 2: ") + "\n";
                //    this.txtPriNombreCod2.Focus();
                //}
                //if (!this.masterTerceroDocTipoCod2.ValidID)
                //{
                //    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 2: ") + "\n";
                //    this.masterTerceroDocTipoCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.dtFechaExpCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 2:") + "\n";
                //    this.dtFechaExpCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.dtFechaNacCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 2: ") + "\n";
                //    this.dtFechaNacCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCelular1Cod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 2: ") + "\n";
                //    this.txtCelular1Cod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCorreoCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 2: ") + "\n";
                //    this.txtCorreoCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtDireccionCod2.EditValue.ToString()))
                //{
                //    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 2: ") + "\n";
                //    this.txtDireccionCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.masterCiudadResCod2.Value))
                //{
                //    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 2: ") + "\n";
                //    this.masterCiudadResCod2.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtTelResidenciaCod2.Text))
                //{
                //    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 2:") + "\n";
                //    this.txtTelResidenciaCod2.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterCiudadResCod2.Value) && !this.masterCiudadResCod2.ValidID)
                //{
                //    result += string.Format("La ciudad residencia del Codeudor 2 no es válida, verifique") + "\n";
                //    this.masterCiudadResCod2.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod2.Value) && !this.masterTerceroDocTipoCod2.ValidID)
                //{
                //    result += string.Format("El Tipo de documento del Codeudor 2 no es válido, verifique") + "\n";
                //    this.masterTerceroDocTipoCod2.Focus();
                //}
                #endregion
            }
            if (!string.IsNullOrEmpty(this.txtCedulaCod3.Text))
            {
                #region Datos CoDeudor 3     
                //if (String.IsNullOrEmpty(this.txtCedulaCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblNroDocumento.Text + " Codeudor 3: ") + "\n";
                //    this.txtCedulaCod3.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriApellidoCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriApellido.Text + " Codeudor 3: ") + "\n";
                //    this.txtPriApellidoCod3.Focus();
                //}
                //if (String.IsNullOrEmpty(this.txtPriNombreCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblPriNombre.Text + " Codeudor 3: ") + "\n";
                //    this.txtPriNombreCod3.Focus();
                //}
                //if (!this.masterTerceroDocTipoCod3.ValidID)
                //{
                //    result += string.Format(msgVacio, this.lblTipoDoc.Text + " Codeudor 3: ") + "\n";
                //    this.masterTerceroDocTipoCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.dtFechaExpCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaExp.Text + " Codeudor 3:") + "\n";
                //    this.dtFechaExpCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.dtFechaNacCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblFechaNac.Text + " Codeudor 3: ") + "\n";
                //    this.dtFechaNacCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCelular1Cod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblCelular1.Text + " Codeudor 3: ") + "\n";
                //    this.txtCelular1Cod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtCorreoCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblEmail.Text + " Codeudor 3: ") + "\n";
                //    this.txtCorreoCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtDireccionCod3.EditValue.ToString()))
                //{
                //    result += string.Format(msgVacio, this.lblDirResidencia.Text + " Codeudor 3: ") + "\n";
                //    this.txtDireccionCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.masterCiudadResCod3.Value))
                //{
                //    result += string.Format(msgVacio, this.lblCiudadRes.Text + " Codeudor 3: ") + "\n";
                //    this.masterCiudadResCod3.Focus();
                //}
                //if (string.IsNullOrEmpty(this.txtTelResidenciaCod3.Text))
                //{
                //    result += string.Format(msgVacio, this.lblTelResid.Text + " Codeudor 3: ") + "\n";
                //    this.txtTelResidenciaCod3.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterCiudadResCod3.Value) && !this.masterCiudadResCod3.ValidID)
                //{
                //    result += string.Format("La ciudad residencia del Codeudor 3 no es válida, verifique") + "\n";
                //    this.masterCiudadResCod3.Focus();
                //}
                //if (!string.IsNullOrEmpty(this.masterTerceroDocTipoCod3.Value) && !this.masterTerceroDocTipoCod3.ValidID)
                //{
                //    result += string.Format("El Tipo de documento del Codeudor 3 no es válido, verifique") + "\n";
                //    this.masterTerceroDocTipoCod3.Focus();
                //}
                #endregion
            }

            if (string.IsNullOrEmpty(result))
                return true;
            else
            {
                MessageBox.Show("Verifique los siguientes campos: \n\n" + result);
                return false;
            }
            #endregion
            #region glDocumentoControl
            //this._ctrl.PeriodoDoc.Value = this.periodo;
            //this._ctrl.PeriodoUltMov.Value = this.periodo;
            //this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion
            //if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value.Value == 0)
            //{
            //    this._ctrl.DocumentoID.Value = this._documentID;
            //    this._ctrl.NumeroDoc.Value = 0;
            //    this._ctrl.FechaDoc.Value = DateTime.Now.Month == this.periodo.Month && DateTime.Now.Year == this.periodo.Year ? DateTime.Now : new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
            //    this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
            //    this._ctrl.Descripcion.Value = "Solicitud Crédito " + this.txtCedulaDeudor.Text;
            //    this._ctrl.Fecha.Value = DateTime.Now;
            //    this._ctrl.LugarGeograficoID.Value = this.masterTerceroDocTipoDeudor.Value;
            //    this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
            //    this._ctrl.PrefijoID.Value = this._bc.GetPrefijo(this._ctrl.AreaFuncionalID.Value, this._documentID);
            //    this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            //    this._ctrl.TasaCambioDOCU.Value = 0;
            //    this._ctrl.TasaCambioCONT.Value = 0;
            //    this._ctrl.Valor.Value = 0;
            //    this._ctrl.Iva.Value = 0;
            //    this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            //}
            #endregion           

            return true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void AssignValues(bool isGet)
        {
            try
            {
                DTO_drSolicitudDatosPersonales deudor = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 1);
                DTO_drSolicitudDatosPersonales conyuge = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 2);
                DTO_drSolicitudDatosPersonales codeudor1 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 3);
                DTO_drSolicitudDatosPersonales codeudor2 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 4);
                DTO_drSolicitudDatosPersonales codeudor3 = this._data.DatosPersonales.Find(x => x.TipoPersona.Value == 5);
                DTO_drSolicitudDatosVehiculo vehiculo = this._data.DatosVehiculo;
                DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;

                if (isGet)
                {
                    #region Asigna datos a los controles
                    //Deudor (TipoPersona 1)
                    this.txtCedulaDeudor.Text = this._data.SolicituDocu.ClienteID.Value.ToString();
                    this.txtCedulaDeudor.ReadOnly = true;
                    this.lblVersion.Text = "Vers: " + (this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value.ToString() : "1");
                    if (deudor != null)
                    {
                        #region Datos Personales
                        this.txtPriApellidoDeudor.Text = deudor.ApellidoPri.Value;
                        this.txtSdoApellidoDeudor.Text = deudor.ApellidoSdo.Value;
                        this.txtPriNombreDeudor.Text = deudor.NombrePri.Value;
                        this.txtSdoNombreDeudor.Text = deudor.NombreSdo.Value;
                        this.masterActEconPrincipalDeudor.Value = deudor.ActEconomica1.Value;

                        this.masterTerceroDocTipoDeudor.Value = deudor.TerceroDocTipoID.Value;
                        this.dtFechaExpDeudor.DateTime = deudor.FechaExpDoc.Value.HasValue ? deudor.FechaExpDoc.Value.Value : DateTime.Now;
                        
                        this.txtFincaRaizDeudor.EditValue = deudor.PF_FincaRaiz.Value;
                        this.txtMoraActualDeudor.EditValue = deudor.PF_MorasActuales.Value;
                        this.txtMoraUltDeudor.EditValue = deudor.PF_MorasUltAno.Value;
                        this.txtReporteNegativoDeudor.EditValue = deudor.PF_MorasUltAno.Value;
                        this.txtEstadoActualDeudor.EditValue = deudor.PF_EstadoActual.Value;
                        this.txtProbMoraDeudor.EditValue = deudor.PF_Probabilidad.Value;
                        this.txtEstabilidadDeudor.EditValue = deudor.PF_Estabilidad.Value;
                        this.txtUbicabilidadDeudor.EditValue = deudor.PF_Ubicabilidad.Value;
                        this.txtMaxFinanDeudor.EditValue = deudor.PF_PorMaxFinancia.Value;

                        this.txtValorIngDeud.EditValue = deudor.IngresosSOP.Value.HasValue ? deudor.IngresosSOP.Value : 0;
                        this.txtValorIngSoporDeud.EditValue = deudor.PF_IngresoEstimado.Value.HasValue ? deudor.PF_IngresoEstimado.Value : 0;

                        this.chkRecibosDeudor.EditValue = deudor.PF_RecAguayLuz.Value;
                        this.chkConfCelularDeudor.EditValue = deudor.PF_ConfirmaCel.Value;
                        this.chkConfEmailDeudor.EditValue = deudor.PF_ConfirmaMail.Value;

                        this.txtVigenciaFMIDeudor.EditValue = deudor.PF_VigenciaFMI.Value;
                        this.txtVigenciaConsultaDeudor.EditValue = deudor.PF_VigenciaConsData.Value;
                        
                        #endregion
                        
                    }
                    else
                    {
                        this.txtPriApellidoDeudor.Text = this._data.SolicituDocu.ApellidoPri.Value.ToString();
                        this.txtSdoApellidoDeudor.Text = this._data.SolicituDocu.ApellidoSdo.Value.ToString();
                        this.txtPriNombreDeudor.Text = this._data.SolicituDocu.NombrePri.Value.ToString();
                        this.txtSdoNombreDeudor.Text = this._data.SolicituDocu.NombreSdo.Value.ToString();
                    }
                    //Conyuge (TipoPersona 2)
                    if (conyuge != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCony.Text = conyuge.TerceroID.Value.ToString();
                        this.txtPriApellidoCony.Text = conyuge.ApellidoPri.Value;
                        this.txtSdoApellidoCony.Text = conyuge.ApellidoSdo.Value;
                        this.txtPriNombreCony.Text = conyuge.NombrePri.Value;
                        this.txtSdoNombreCony.Text = conyuge.NombreSdo.Value;
                        this.masterActEconPrincipalCony.Value = conyuge.ActEconomica1.Value;

                        this.masterTerceroDocTipoCony.Value = conyuge.TerceroDocTipoID.Value;
                        this.dtFechaExpCony.DateTime = conyuge.FechaExpDoc.Value.HasValue ? conyuge.FechaExpDoc.Value.Value : DateTime.Now;

                        this.txtFincaRaizCony.EditValue = conyuge.PF_FincaRaiz.Value;
                        this.txtMoraActualCony.EditValue = conyuge.PF_MorasActuales.Value;
                        this.txtMoraUltCony.EditValue = conyuge.PF_MorasUltAno.Value;
                        this.txtReporteNegativoCony.EditValue = conyuge.PF_MorasUltAno.Value;
                        this.txtEstadoActualCony.EditValue = conyuge.PF_EstadoActual.Value;
                        this.txtProbMoraCony.EditValue = conyuge.PF_Probabilidad.Value;
                        this.txtEstabilidadCony.EditValue = conyuge.PF_Estabilidad.Value;
                        this.txtUbicabilidadCony.EditValue = conyuge.PF_Ubicabilidad.Value;
                        this.txtMaxFinanCony.EditValue = conyuge.PF_PorMaxFinancia.Value;

                        this.txtValorIngCony.EditValue = conyuge.IngresosSOP.Value.HasValue ? conyuge.IngresosSOP.Value : 0;
                        this.txtValorIngSoporCony.EditValue = conyuge.PF_IngresoEstimado.Value.HasValue ? conyuge.PF_IngresoEstimado.Value : 0;

                        this.chkRecibosCony.EditValue = conyuge.PF_RecAguayLuz.Value;
                        this.chkConfCelularCony.EditValue = conyuge.PF_ConfirmaCel.Value;
                        this.chkConfEmailCony.EditValue = conyuge.PF_ConfirmaMail.Value;

                        this.txtVigenciaFMICony.EditValue = conyuge.PF_VigenciaFMI.Value;
                        this.txtVigenciaConsultaCony.EditValue = conyuge.PF_VigenciaConsData.Value;
        
                        

                        this.linkConyuge.Visible = false;
                        #endregion
                    }
                    //Codeudor1 (TipoPersona 3)
                    if (codeudor1 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod1.Text = codeudor1.TerceroID.Value.ToString();
                        this.txtPriApellidoCod1.Text = codeudor1.ApellidoPri.Value;
                        this.txtSdoApellidoCod1.Text = codeudor1.ApellidoSdo.Value;
                        this.txtPriNombreCod1.Text = codeudor1.NombrePri.Value;
                        this.txtSdoNombreCod1.Text = codeudor1.NombreSdo.Value;
                        this.masterActEconPrincipalCod1.Value = codeudor1.ActEconomica1.Value;                        
                        this.masterTerceroDocTipoCod1.Value = codeudor1.TerceroDocTipoID.Value;
                        this.dtFechaExpCod1.DateTime = codeudor1.FechaExpDoc.Value.HasValue ? codeudor1.FechaExpDoc.Value.Value : DateTime.Now;

                        this.txtFincaRaizCod1.EditValue = codeudor1.PF_FincaRaiz.Value;
                        this.txtMoraActualCod1.EditValue = codeudor1.PF_MorasActuales.Value;
                        this.txtMoraUltCod1.EditValue = codeudor1.PF_MorasUltAno.Value;
                        this.txtReporteNegativoCod1.EditValue = codeudor1.PF_MorasUltAno.Value;
                        this.txtEstadoActualCod1.EditValue = codeudor1.PF_EstadoActual.Value;
                        this.txtProbMoraCod1.EditValue = codeudor1.PF_Probabilidad.Value;
                        this.txtEstabilidadCod1.EditValue = codeudor1.PF_Estabilidad.Value;
                        this.txtUbicabilidadCod1.EditValue = codeudor1.PF_Ubicabilidad.Value;
                        this.txtMaxFinanCod1.EditValue = codeudor1.PF_PorMaxFinancia.Value;

                        this.txtValorIngCod1.EditValue = codeudor1.IngresosSOP.Value.HasValue ? codeudor1.IngresosSOP.Value : 0;
                        this.txtValorIngSoporCod1.EditValue = codeudor1.PF_IngresoEstimado.Value.HasValue ? codeudor1.PF_IngresoEstimado.Value : 0;

                        this.chkRecibosCod1.EditValue = codeudor1.PF_RecAguayLuz.Value;
                        this.chkConfCelularCod1.EditValue = codeudor1.PF_ConfirmaCel.Value;
                        this.chkConfEmailCod1.EditValue = codeudor1.PF_ConfirmaMail.Value;

                        this.txtVigenciaFMICod1.EditValue = codeudor1.PF_VigenciaFMI.Value;
                        this.txtVigenciaConsultaCod1.EditValue = codeudor1.PF_VigenciaConsData.Value;

                        this.linkCodeudor1.Visible = false;
                        #endregion
                    }
                    //Codeudor2 (TipoPersona 4)
                    if (codeudor2 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod2.Text = codeudor2.TerceroID.Value.ToString();
                        this.txtPriApellidoCod2.Text = codeudor2.ApellidoPri.Value;
                        this.txtSdoApellidoCod2.Text = codeudor2.ApellidoSdo.Value;
                        this.txtPriNombreCod2.Text = codeudor2.NombrePri.Value;
                        this.txtSdoNombreCod2.Text = codeudor2.NombreSdo.Value;
                        this.masterActEconPrincipalCod2.Value = codeudor2.ActEconomica1.Value;                       
                        this.masterTerceroDocTipoCod2.Value = codeudor2.TerceroDocTipoID.Value;
                        this.dtFechaExpCod2.DateTime = codeudor2.FechaExpDoc.Value.HasValue ? codeudor2.FechaExpDoc.Value.Value : DateTime.Now;

                        this.txtFincaRaizCod2.EditValue = codeudor2.PF_FincaRaiz.Value;
                        this.txtMoraActualCod2.EditValue = codeudor2.PF_MorasActuales.Value;
                        this.txtMoraUltCod2.EditValue = codeudor2.PF_MorasUltAno.Value;
                        this.txtReporteNegativoCod2.EditValue = codeudor2.PF_MorasUltAno.Value;
                        this.txtEstadoActualCod2.EditValue = codeudor2.PF_EstadoActual.Value;
                        this.txtProbMoraCod2.EditValue = codeudor2.PF_Probabilidad.Value;
                        this.txtEstabilidadCod2.EditValue = codeudor2.PF_Estabilidad.Value;
                        this.txtUbicabilidadCod2.EditValue = codeudor2.PF_Ubicabilidad.Value;
                        this.txtMaxFinanCod2.EditValue = codeudor2.PF_PorMaxFinancia.Value;

                        this.txtValorIngCod2.EditValue = codeudor2.IngresosSOP.Value.HasValue ? codeudor2.IngresosSOP.Value : 0;
                        this.txtValorIngSoporCod2.EditValue = codeudor2.PF_IngresoEstimado.Value.HasValue ? codeudor2.PF_IngresoEstimado.Value : 0;

                        this.chkRecibosCod2.EditValue = codeudor2.PF_RecAguayLuz.Value;
                        this.chkConfCelularCod2.EditValue = codeudor2.PF_ConfirmaCel.Value;
                        this.chkConfEmailCod2.EditValue = codeudor2.PF_ConfirmaMail.Value;

                        this.txtVigenciaFMICod2.EditValue = codeudor2.PF_VigenciaFMI.Value;
                        this.txtVigenciaConsultaCod2.EditValue = codeudor2.PF_VigenciaConsData.Value;

                        this.linkCodeudor2.Visible = false;
                        #endregion
                    }
                    //Codeudor3 (TipoPersona 5)
                    if (codeudor3 != null)
                    {
                        #region Datos Personales
                        this.txtCedulaCod3.Text = codeudor3.TerceroID.Value.ToString();
                        this.txtPriApellidoCod3.Text = codeudor3.ApellidoPri.Value;
                        this.txtSdoApellidoCod3.Text = codeudor3.ApellidoSdo.Value;
                        this.txtPriNombreCod3.Text = codeudor3.NombrePri.Value;
                        this.txtSdoNombreCod3.Text = codeudor3.NombreSdo.Value;
                        this.masterActEconPrincipalCod3.Value = codeudor3.ActEconomica1.Value;

                        this.masterTerceroDocTipoCod3.Value = codeudor3.TerceroDocTipoID.Value;
                        this.dtFechaExpCod3.DateTime = codeudor3.FechaExpDoc.Value.HasValue ? codeudor3.FechaExpDoc.Value.Value : DateTime.Now;

                        this.txtFincaRaizCod3.EditValue = codeudor3.PF_FincaRaiz.Value;
                        this.txtMoraActualCod3.EditValue = codeudor3.PF_MorasActuales.Value;
                        this.txtMoraUltCod3.EditValue = codeudor3.PF_MorasUltAno.Value;
                        this.txtReporteNegativoCod3.EditValue = codeudor3.PF_MorasUltAno.Value;
                        this.txtEstadoActualCod3.EditValue = codeudor3.PF_EstadoActual.Value;
                        this.txtProbMoraCod3.EditValue = codeudor3.PF_Probabilidad.Value;
                        this.txtEstabilidadCod3.EditValue = codeudor3.PF_Estabilidad.Value;
                        this.txtUbicabilidadCod3.EditValue = codeudor3.PF_Ubicabilidad.Value;
                        this.txtMaxFinanCod3.EditValue = codeudor3.PF_PorMaxFinancia.Value;

                        this.txtValorIngCod3.EditValue = codeudor3.IngresosSOP.Value.HasValue ? codeudor3.IngresosSOP.Value : 0;
                        this.txtValorIngSoporCod3.EditValue = codeudor3.PF_IngresoEstimado.Value.HasValue ? codeudor3.PF_IngresoEstimado.Value : 0;

                        this.chkRecibosCod3.EditValue = codeudor3.PF_RecAguayLuz.Value;
                        this.chkConfCelularCod3.EditValue = codeudor3.PF_ConfirmaCel.Value;
                        this.chkConfEmailCod3.EditValue = codeudor3.PF_ConfirmaMail.Value;

                        this.txtVigenciaFMICod3.EditValue = codeudor3.PF_VigenciaFMI.Value;
                        this.txtVigenciaConsultaCod3.EditValue = codeudor3.PF_VigenciaConsData.Value;
                        this.linkCodeudor3.Visible = false;
                        #endregion

                    }
                    #endregion
                }
                else
                {
                    #region Llena datos de los controles para salvar
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "AssignValues"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="libranzaNro"></param>
        private void LoadData(string cliente, int libranzaNro)
        {
            this._isLoaded = false;
            this._data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(libranzaNro);
            this._libranzaID = String.Empty;
            if (this._data != null)
            {
                #region Solicitud existente
                this._ctrl = this._data.DocCtrl;

                if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)                  
                {
                    this._libranzaID = this._data.SolicituDocu.Libranza.Value.ToString();
                    this.AssignValues(true);
                    this.EnableHeader(true);
                }

                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAprobada));
                    CleanData();

                    this.txtCedulaDeudor.Focus();
                }
                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                    CleanData();

                    this.txtCedulaDeudor.Focus();
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
                FormProvider.Master.itemUpdate.Visible = false;
                FormProvider.Master.itemSave.Visible = true;
                FormProvider.Master.itemDelete.Visible = true;
                FormProvider.Master.itemSendtoAppr.Visible = true;
                FormProvider.Master.itemSave.Enabled = true;

                if (FormProvider.Master.LoadFormTB)
                {
                    // SecurityManager.HasAccess(this._documentID, FormsActions.Edit);
                    FormProvider.Master.itemDelete.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.Delete);
                    FormProvider.Master.itemSendtoAppr.Enabled = SecurityManager.HasAccess(this._documentID, FormsActions.SendtoAppr);
                    FormProvider.Master.itemSearch.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Perfil.cs", "Form_FormClosed"));
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
        //private void btnSiguiente_Click(object sender, EventArgs e)
        //{
        //    if (this.tabControl.SelectedTabPageIndex == 0)
        //        this.tabControl.SelectedTabPageIndex = 1;
        //    else if (this.tabControl.SelectedTabPageIndex == 1)
        //        this.tabControl.SelectedTabPageIndex = 2;
        //    else if (this.tabControl.SelectedTabPageIndex == 2)
        //        this.tabControl.SelectedTabPageIndex = 0;
        //}

        //private void btnAtras_Click(object sender, EventArgs e)
        //{
        //    if (this.tabControl.SelectedTabPageIndex == 0)
        //        this.tabControl.SelectedTabPageIndex = 2;
        //    else if (this.tabControl.SelectedTabPageIndex == 1)
        //        this.tabControl.SelectedTabPageIndex = 0;
        //    else if (this.tabControl.SelectedTabPageIndex == 2)
        //        this.tabControl.SelectedTabPageIndex = 1;
        //}

        private void linkConyuge_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (this.linkConyuge.Dock == DockStyle.Fill)
            {
                this.linkConyuge.Dock = DockStyle.None;
            }
            else
            {
                this.linkConyuge.Dock = DockStyle.Fill;
            }
        }
        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor1.Dock == DockStyle.Fill)
            {
                linkCodeudor1.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor1.Dock = DockStyle.Fill;
            }
        }
        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor2.Dock == DockStyle.Fill)
            {
                linkCodeudor2.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor2.Dock = DockStyle.Fill;
            }
        }

        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            if (linkCodeudor3.Dock == DockStyle.Fill)
            {
                linkCodeudor3.Dock = DockStyle.None;
            }
            else
            {
                linkCodeudor3.Dock = DockStyle.Fill;
            }
        }

        /// <summary>
        /// Devuelve el flujo al dar clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDevolver_Click(object sender, EventArgs e)
        {
            if (this.actividadesFlujo.Exists(x => x == this.cmbActividades.EditValue.ToString() && this._ctrl != null))
            {
                DTO_TxResult res = this._bc.AdministrationModel.DevolverFlujoDocumento(this._documentID, this.cmbActividades.EditValue.ToString(), this._ctrl.NumeroDoc.Value, string.Empty);
                if (res.Result == ResultValue.OK)
                    FormProvider.CloseCurrent();
            }
        }

        /// <summary>
        /// Al cmabiar el valor del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbActividades_EditValueChanged(object sender, EventArgs e)
        {
            if (this.actividadesFlujo.Exists(x => x == this.cmbActividades.EditValue.ToString() && this._ctrl != null))
                this.lblDevolver.Visible = true;
            else
                this.lblDevolver.Visible = false;
        }
       
        private void lblFincaRaiz_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true,0 });

        }

        private void lblMoraActual_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 2 });

        }

        private void lbMoraUltimo_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 3 });

        }

        private void lblReporteNegativo_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 4 });

        }

        private void lblEstadoActual_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 5 });

        }

        private void lblProbabilidadMora_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 1 });

        }

        private void lblEstabilidad_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 6 });

        }

        private void lblUbicabilidad_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 7 });

        }

        private void lblMaxFinancia_Click(object sender, EventArgs e)
        {
            Type frm = typeof(DetallePerfil);
            FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true, 8 });

        }

        #endregion Eventos Formulario

        private void lblActEconomica1_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            this.documentReportID = AppReports.drReportePerfil;
            string reportName = this._bc.AdministrationModel.Report_Dr_DecisorByNumeroDoc(this.documentReportID, Convert.ToInt32(this._ctrl.NumeroDoc.Value), Convert.ToInt32(this._libranzaID), ExportFormatType.pdf);
            //Process.Start(this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString()));
        }


        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        /// 

        //public override void TBSave()
        //{
        //    try
        //    {
        //       // this._data.DatosPersonales.Clear();
        //        this.AssignValues(false);
        //        this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
        //        if (this.ValidateData())
        //        {
        //            DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
        //            DTO_TxResult result = _bc.AdministrationModel.DigitacionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, false);

        //                    if (result.Result == ResultValue.OK)
        //            {
        //                //#region Obtiene el nombre

        //                //string nombre = this._data.SolicituDocu.NombrePri.Value;
        //                //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.NombreSdo.Value))
        //                //    nombre += " " + this._data.SolicituDocu.NombreSdo.Value;
        //                //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.ApellidoPri.Value))
        //                //    nombre += " " + this._data.SolicituDocu.ApellidoPri.Value;
        //                //if (!string.IsNullOrWhiteSpace(this._data.SolicituDocu.ApellidoSdo.Value))
        //                //    nombre += " " + this._data.SolicituDocu.ApellidoSdo.Value;

        //                //#endregion
        //                //#region Variables para el mail

        //                //DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

        //                //string body = string.Empty;
        //                //string subject = string.Empty;
        //                //string email = user.CorreoElectronico.Value;

        //                //string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
        //                //string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_SentToAprobCartera_Body);
        //                //string formName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

        //                //#endregion
        //                //#region Envia el correo
        //                //subject = string.Format(subjectApr, formName);
        //                //body = string.Format(bodyApr, formName, this.txtCedulaDeudor.Text.Trim(), nombre, this._data.SolicituDocu.Observacion.Value);
        //                //_bc.SendMail(this._documentID, subject, body, email);
        //                //#endregion

        //                //Actualiza el control para las financieras
        //                string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
        //                if (sectorLib == ((byte)SectorCartera.Financiero).ToString()) //Financieras
        //                {
        //                    string numeroControl = _bc.AdministrationModel.Empresa.NumeroControl.Value;
        //                    _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numeroControl).ToList();
        //                }

        //                this.txtCedulaDeudor.Focus();

        //                string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaGuardada);
        //                MessageBox.Show(string.Format(msg, this._libranzaID));
        //            }
        //            else
        //            {
        //                MessageForm frm = new MessageForm(result);
        //                frm.ShowDialog();
        //            }
        //        }                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
        //    }
        //}

        /// <summary>
        /// Boton para salvar un nuevo documento
        /// </summary>
        //public override void TBSendtoAppr()
        //{
        //    try
        //    {
        //        string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "Realmente desea aprobar el registro de la solicitud?  ");
        //        if (MessageBox.Show(msgDoc, "Confirmación", MessageBoxButtons.YesNo) == DialogResult.No)
        //            return;
        //        this.AssignValues(false);
        //        this._data.DatosPersonales.RemoveAll(x => string.IsNullOrEmpty(x.TerceroID.Value));
        //        if (this.ValidateData())
        //        {
        //            DTO_TxResult result = _bc.AdministrationModel.DigitacionSolicitud_Add(this._documentID, this._actFlujo.ID.Value, this._data, true);

        //            MessageForm frm = new MessageForm(result);
        //            frm.ShowDialog();
        //            if (result.Result == ResultValue.OK)
        //            {
        //                this.CleanData();
        //                //CIerra el formulario
        //                FormProvider.CloseCurrent();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-SolicitudRegistro.cs", "TBSave"));
        //    }
        //}


        #endregion Eventos Barra Herramientas

        /// <summary>
        /// Boton para editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

    }
}