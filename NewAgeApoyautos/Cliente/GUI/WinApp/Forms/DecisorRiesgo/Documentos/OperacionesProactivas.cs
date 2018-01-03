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
    public partial class OperacionesProactivas : FormWithToolbar
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
        private DTO_ccSolicitudDocu _header = new DTO_ccSolicitudDocu();
        private List<DTO_ccSolicitudAnexo> _anexos = new List<DTO_ccSolicitudAnexo>();
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

        //Variables ToolBar
        private bool _isLoaded;
        private bool _asesorInd;
        private bool _comercioInd;
        private SectorCartera _sector;

        private DateTime periodo = DateTime.Now;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public OperacionesProactivas()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public OperacionesProactivas(string mod)
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
                this.AddGridCols();

                #region Carga la info de las actividades
                List<string> actividades = _bc.AdministrationModel.glActividadFlujo_GetActividadesByDocumentID(this._documentID);
                if (actividades.Count != 1)
                {
                    string msg = _bc.GetResource(LanguageTypes.Error, DictionaryMessages.Err_Gl_DocMultActivities);
                    MessageBox.Show(string.Format(msg, this._documentID.ToString()));
                    this.EnableHeader(false);
                }
                else
                {
                    if (this._frmModule == ModulesPrefix.cf)
                    {
                        this.nextActID = string.Empty;
                        string actividadFlujoID = actividades[0];
                        this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                    }
                    else
                    {
                        #region Carga la info de la prioxima actividad
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
                            string actividadFlujoID = actividades[0];
                            this._actFlujo = (DTO_glActividadFlujo)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, false, actividadFlujoID, true);
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "Constructor"));
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
                this._documentID = AppDocuments.SolicitudLibranza;
                this._frmModule = ModulesPrefix.cc;

                //Carga la informacion de la maestras
                _bc.InitMasterUC(this.masterCliente, AppMasters.ccCliente, false, true, true, false);
                _bc.InitMasterUC(this.masterAsesor, AppMasters.ccAsesor, true, true, true, false);
                _bc.InitMasterUC(this.masterCiudad, AppMasters.glLugarGeografico, true, true, true, false);
                _bc.InitMasterUC(this.masterCentroPago, AppMasters.ccCentroPagoPAG, true, true, true, false);
                _bc.InitMasterUC(this.masterPagaduria, AppMasters.ccPagaduria, true, true, true, false);
                _bc.InitMasterUC(this.masterComercio, AppMasters.ccConcesionario, false, true, true, false);
                _bc.InitMasterUC(this.masterCooperativa, AppMasters.ccCooperativa, true, true, true, false);
                _bc.InitMasterUC(this.masterLineaCredito, AppMasters.ccLineaCredito, true, true, true, false);
                _bc.InitMasterUC(this.masterTipoCredito, AppMasters.ccTipoCredito, true, true, true, false);
                _bc.InitMasterUC(this.masterCodeudor1, AppMasters.coTercero, false, true, true, false);
                _bc.InitMasterUC(this.masterCodeudor2, AppMasters.coTercero, false, true, true, false);
                _bc.InitMasterUC(this.masterCodeudor3, AppMasters.coTercero, false, true, true, false);

                //Deshabilita los botones +- de la grilla
                this.gcDocument.EmbeddedNavigator.Buttons.Remove.Enabled = false;
                this.gcDocument.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

                string strPeriodo = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                this.periodo = Convert.ToDateTime(strPeriodo);

                //Asesor
                this._asesorXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_AsesorXDefecto);
                string asesorInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoDeAsesores);
                this._asesorInd = asesorInd == "1" ? true : false;
                if (!_asesorInd)
                    this.masterAsesor.EnableControl(false);

                //this.masterComercio
                this._comercioXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PuntoComercialPorDefecto);
                string comercioInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ManejoPuntosComerciales);
                this._comercioInd = comercioInd == "1" ? true : false;
                if (!_comercioInd)
                {
                    this.masterComercio.EnableControl(false);
                    this.masterComercio.Value = this._comercioXDef;
                }

                //Lugar Geográfico
                this._lugGeoXDef = _bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_LugarGeoXDefecto);

                //Centro de Pago
                this._centroPagoXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CentroPagoPorDefecto);

                //Pagaduría
                this._pagaduriaXDef = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_PagaduriaXDefecto);

                //Carga el tipo de garaantía
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(string.Empty, string.Empty);
                dic.Add("1", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoGarantia_v1"));
                dic.Add("2", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoGarantia_v2"));
                dic.Add("3", this._bc.GetResource(LanguageTypes.Tables, "tbl_restr_TipoGarantia_v3"));
                this.lkp_TipoGarantia.Properties.DataSource = dic;

                //Carga los datos por defecto
                string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                this._sector = (SectorCartera)Enum.Parse(typeof(SectorCartera), sectorLib);
                if (this._sector == SectorCartera.Solidario)
                    this.lkp_TipoGarantia.EditValue = "1";
                else
                {
                    this.masterCooperativa.Visible = false;
                    this.lkp_TipoGarantia.EditValue = "3";
                    this.masterCentroPago.Visible = false;
                    this.masterPagaduria.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
                this.gvDocument.Columns.Clear();

                #region Doc Anexo Lista

                //Campo de marca
                GridColumn marca = new GridColumn();
                marca.FieldName = this._unboundPrefix + "IncluidoInd";
                marca.Caption = "√";
                marca.UnboundType = UnboundColumnType.Boolean;
                marca.VisibleIndex = 0;
                marca.Width = 15;
                marca.Visible = true;
                marca.ToolTip = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_IncluidoInd");
                marca.AppearanceHeader.ForeColor = Color.Lime;
                marca.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                marca.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                marca.AppearanceHeader.Options.UseTextOptions = true;
                marca.AppearanceHeader.Options.UseFont = true;
                marca.AppearanceHeader.Options.UseForeColor = true;
                this.gvDocument.Columns.Add(marca);

                //DocumListaID
                GridColumn documListaID = new GridColumn();
                documListaID.FieldName = this._unboundPrefix + "DocumListaID";
                documListaID.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_DocumListaID");
                documListaID.UnboundType = UnboundColumnType.String;
                documListaID.VisibleIndex = 1;
                documListaID.Width = 80;
                documListaID.Visible = true;
                documListaID.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(documListaID);

                //Descriptivo
                GridColumn descriptivo = new GridColumn();
                descriptivo.FieldName = this._unboundPrefix + "Descriptivo";
                descriptivo.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descriptivo");
                descriptivo.UnboundType = UnboundColumnType.String;
                descriptivo.VisibleIndex = 2;
                descriptivo.Width = 100;
                descriptivo.Visible = true;
                descriptivo.OptionsColumn.AllowEdit = false;
                this.gvDocument.Columns.Add(descriptivo);

                //Observaciones
                GridColumn observaciones = new GridColumn();
                observaciones.FieldName = this._unboundPrefix + "Descripcion";
                observaciones.Caption = _bc.GetResource(LanguageTypes.Forms, this._documentID + "_Descripcion");
                observaciones.UnboundType = UnboundColumnType.String;
                observaciones.VisibleIndex = 3;
                observaciones.Width = 150;
                observaciones.Visible = true;
                observaciones.OptionsColumn.AllowEdit = true;
                this.gvDocument.Columns.Add(observaciones);

                #endregion Doc Anexo Lista
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "AddGridCols"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {
            //Header
            int value = Convert.ToInt32(this.lkp_TipoGarantia.EditValue);
            if (value == (int)TipoGarantia.Real)
                this.txtLibranza.Text = "0";
            else
                this.txtLibranza.Text = String.Empty;

            this.masterCliente.Value = String.Empty;
            this.masterComercio.Value = string.Empty;
            this.masterCooperativa.Value = string.Empty;
            this.masterLineaCredito.Value = string.Empty;
            this.masterTipoCredito.Value = string.Empty;
            this.masterCodeudor1.Value = string.Empty;
            this.masterCodeudor2.Value = string.Empty;
            this.masterCodeudor3.Value = string.Empty;
            this.txtPriApellido.Text = String.Empty;
            this.txtSdoApellido.Text = String.Empty;
            this.txtPriNombre.Text = String.Empty;
            this.txtSdoNombre.Text = String.Empty;
            this.comboPlazo.SelectedIndex = -1;
            this.txtValor.Text = "0";
            this.txtObservacion.Text = String.Empty;
            this.txtObsReversion.Text = string.Empty;
            this.chkCompraCartera.Checked = false;

            this.masterComercio.Value = this._comercioXDef;
            this.masterCiudad.Value = this._lugGeoXDef;
            if (value != (int)TipoGarantia.Libranza)
            {
                this.masterAsesor.Value = this._asesorXDef;
                this.masterCentroPago.Value = this._centroPagoXDef;
            }
            else
            {
                this.masterAsesor.Value = string.Empty;
                this.masterCentroPago.Value = string.Empty;
            }
            this.masterComercio.Value = this._comercioXDef;           
            this.masterPagaduria.Value = this._pagaduriaXDef;

            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._header = new DTO_ccSolicitudDocu();
            this._anexos = new List<DTO_ccSolicitudAnexo>();

            //Variables
            this._clienteID = String.Empty;
            this._pagaduriaID = String.Empty;
            this._libranzaID = String.Empty;
            this._centroPagoID = String.Empty;
            this._tipoCreditoID = string.Empty;

            this.gcDocument.Enabled = true;
            this.gcDocument.DataSource = this._anexos;

            this.btnCambiarLibranza.Visible = false;
            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {
            this.lkp_TipoGarantia.Enabled = enabled;
            //this.masterCliente.EnableControl(enabled);
            this.txtPriApellido.Enabled = enabled;
            this.txtSdoApellido.Enabled = enabled;
            this.txtPriNombre.Enabled = enabled;
            this.txtSdoNombre.Enabled = enabled;
            this.masterAsesor.EnableControl(enabled && this._asesorInd ? true : false);
            this.masterComercio.EnableControl(enabled && this._comercioInd ? true : false);
            //this.masterCiudad.EnableControl(enabled);
            //this.masterCentroPago.EnableControl(enabled);
            //this.masterCooperativa.EnableControl(enabled);
            //this.masterLineaCredito.EnableControl(enabled);
            //this.masterTipoCredito.EnableControl(enabled);
            //this.masterCodeudor1.EnableControl(enabled);
            //this.masterCodeudor2.EnableControl(enabled);
            //this.masterCodeudor3.EnableControl(enabled);
            //this.chkCompraCartera.Enabled = enabled;
            //this.comboPlazo.Enabled = enabled;
            //this.txtLibranza.Enabled = enabled && Convert.ToInt32(this.lkp_TipoGarantia.EditValue) != (int)TipoGarantia.Real ? true : false;
            //this.txtValor.Enabled = enabled;
            //this.txtObservacion.Enabled = enabled;
            //this.txtObsReversion.Enabled = enabled;
        }

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            #region Hace las Validaciones
            //Valida que el usuario exista
            if (String.IsNullOrEmpty(this.masterCliente.Value))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.masterCliente.LabelRsx);
                MessageBox.Show(msg);
                this.masterCliente.Focus();
                return false;
            }

            //Valida que este escrito el primer apellido
            if (String.IsNullOrEmpty(this.txtPriApellido.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriApellido.Text);
                MessageBox.Show(msg);
                this.txtPriApellido.Focus();
                return false;
            }

            ////Valida que este escrito el Primer nombre
            //if (String.IsNullOrEmpty(this.txtPriNombre.Text))
            //{
            //    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPriNombre.Text);
            //    MessageBox.Show(msg);
            //    this.txtPriNombre.Focus();
            //    return false;
            //}

            //Valida que el asesor exista
            if (!this.masterAsesor.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterAsesor.LabelRsx);
                MessageBox.Show(msg);
                this.masterAsesor.Focus();
                return false;
            }

            //Valida que el concesionario(comercio) exista
            if (!this.masterComercio.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterComercio.LabelRsx);
                MessageBox.Show(msg);
                this.masterComercio.Focus();
                return false;
            }

            //Valida que la ciudad exista
            if (!this.masterCiudad.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCiudad.LabelRsx);
                MessageBox.Show(msg);
                this.masterCiudad.Focus();
                return false;
            }

            //Valida que este escrito el numero de Libranza
            if (String.IsNullOrEmpty(this.txtLibranza.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidField), this.lblLibranza.Text);
                MessageBox.Show(msg);
                this.txtLibranza.Focus();
                return false;
            }

            //Valida que el centro de pago exista
            if (!this.masterCentroPago.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCentroPago.LabelRsx);
                MessageBox.Show(msg);
                this.masterCentroPago.Focus();
                return false;
            }

            //Valida que la pagaduria exista
            if (!this.masterPagaduria.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterPagaduria.LabelRsx);
                MessageBox.Show(msg);
                this.masterPagaduria.Focus();
                return false;
            }

            //Valida que la cooperativa exista
            if (!string.IsNullOrWhiteSpace(this.masterCooperativa.Value) && !this.masterCooperativa.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterCooperativa.LabelRsx);
                MessageBox.Show(msg);
                this.masterCooperativa.Focus();
                return false;
            }

            //Valida que la línea de crédito exista
            if (!string.IsNullOrWhiteSpace(this.masterLineaCredito.Value) && !this.masterLineaCredito.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterLineaCredito.LabelRsx);
                MessageBox.Show(msg);
                this.masterLineaCredito.Focus();
                return false;
            }

            //Valida que ell tipo de credito exista
            if (!this.masterTipoCredito.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoCredito.LabelRsx);
                MessageBox.Show(msg);
                this.masterTipoCredito.Focus();
                return false;
            }

            //Valida que el plazo del credito sea valido
            if (String.IsNullOrEmpty(this.comboPlazo.Text))
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col), this.lblPlazo.Text);
                MessageBox.Show(msg);
                this.comboPlazo.Focus();
                return false;
            }

            //Valida que el valor del credito no sea negativo
            if(this._sector == SectorCartera.Financiero)
            {
                if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) < 0)
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblValor.Text);
                    MessageBox.Show(msg);
                    this.txtValor.Focus();
                    return false;
                }
            }
            else 
            {
                if (Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture) <= 0)
                {
                    string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.PositiveValue), this.lblValor.Text);
                    MessageBox.Show(msg);
                    this.txtValor.Focus();
                    return false;
                }
            }

            //Valida que el codeudor1 exista
            if (!string.IsNullOrWhiteSpace(this.masterCodeudor1.Value) && !this.masterCodeudor1.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblCodeudor1.Text);
                MessageBox.Show(msg);
                this.masterCodeudor1.Focus();
                return false;
            }

            //Valida que el codeudor2 exista
            if (!string.IsNullOrWhiteSpace(this.masterCodeudor2.Value) && !this.masterCodeudor2.ValidID)
            {
                string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.lblCodeudor1.Text);
                MessageBox.Show(msg);
                this.masterCodeudor2.Focus();
                return false;
            }

            #endregion
            #region glDocumentoControl
            this._ctrl.PeriodoDoc.Value = this.periodo;
            this._ctrl.PeriodoUltMov.Value = this.periodo;
            this._ctrl.Observacion.Value = string.Empty;//Se borra la observacion de la reversion
            if (this._ctrl.NumeroDoc.Value == null || this._ctrl.NumeroDoc.Value.Value == 0)
            {
                this._ctrl.DocumentoID.Value = this._documentID;
                this._ctrl.NumeroDoc.Value = 0;
                this._ctrl.FechaDoc.Value = DateTime.Now.Month == this.periodo.Month && DateTime.Now.Year == this.periodo.Year ? DateTime.Now : new DateTime(this.periodo.Year, this.periodo.Month, DateTime.DaysInMonth(this.periodo.Year, this.periodo.Month));
                this._ctrl.DocumentoTipo.Value = (byte)DocumentoTipo.DocExterno;
                this._ctrl.Descripcion.Value = _sector == SectorCartera.Solidario ? "Solicitud Libranza " : "Solicitud Crédito " + this.txtLibranza.Text;
                this._ctrl.Fecha.Value = DateTime.Now;
                this._ctrl.LugarGeograficoID.Value = this.masterCiudad.Value;
                this._ctrl.AreaFuncionalID.Value = this._bc.AdministrationModel.User.AreaFuncionalID.Value;
                this._ctrl.PrefijoID.Value = this._bc.GetPrefijo(this._ctrl.AreaFuncionalID.Value, this._documentID);
                this._ctrl.MonedaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                this._ctrl.TasaCambioDOCU.Value = 0;
                this._ctrl.TasaCambioCONT.Value = 0;
                this._ctrl.Valor.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
                this._ctrl.Iva.Value = 0;
                this._ctrl.Estado.Value = Convert.ToInt32(this.lkp_TipoGarantia.EditValue) == (int)TipoGarantia.Real ? (byte)EstadoDocControl.ParaAprobacion : (byte)EstadoDocControl.SinAprobar;
                this._ctrl.seUsuarioID.Value = this._bc.AdministrationModel.User.ReplicaID.Value.Value;
            }
            #endregion
            #region ccSolicitudDocu
            if (this.masterCliente.ValidID)
            {
                this._header.ClienteID.Value = this.masterCliente.Value;
                this._header.ClienteRadica.Value = this.masterCliente.Value;
            }
            else
            {
                this._header.ClienteID.Value = String.Empty;
                this._header.ClienteRadica.Value = this.masterCliente.Value;
            }

            this._header.TipoGarantia.Value = Convert.ToByte(this.lkp_TipoGarantia.EditValue);
            this._header.IncorporacionTipo.Value = 1;
            this._header.IncorporacionPreviaInd.Value = false;
            this._header.ApellidoPri.Value = this.txtPriApellido.Text;
            this._header.ApellidoSdo.Value = this.txtSdoApellido.Text;
            this._header.NombrePri.Value = this.txtPriNombre.Text;
            this._header.NombreSdo.Value = this.txtSdoNombre.Text;
            this._header.Libranza.Value =  Convert.ToInt32(this.txtLibranza.Text);
            this._header.LineaCreditoID.Value = !string.IsNullOrWhiteSpace(this.masterLineaCredito.Value) ? this.masterLineaCredito.Value : this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_LineaCredito);
            this._header.CooperativaID.Value = this.masterCooperativa.Value;
            this._header.AsesorID.Value = this.masterAsesor.Value;
            this._header.ConcesionarioID.Value = this.masterComercio.Value;
            this._header.CentroPagoID.Value = this.masterCentroPago.Value;
            this._header.PagaduriaID.Value = this.masterPagaduria.Value;
            this._header.ZonaID.Value = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Zona);
            this._header.Ciudad.Value = this.masterCiudad.Value;
            this._header.TipoCredito.Value = 1;
            this._header.CompraCarteraInd.Value = this.chkCompraCartera.Checked;
            this._header.PorInteres.Value = 0;
            this._header.PorSeguro.Value = 0;
            this._header.VlrPrestamo.Value = this._header.VlrPrestamo.Value.HasValue? this._header.VlrPrestamo.Value : 0;
            this._header.VlrSolicitado.Value = Convert.ToDecimal(this.txtValor.EditValue, CultureInfo.InvariantCulture);
            this._header.VlrAdicional.Value = 0;
            this._header.VlrLibranza.Value = 0;
            this._header.VlrCompra.Value = 0;
            this._header.VlrDescuento.Value = 0;
            this._header.VlrGiro.Value = 0;
            this._header.Plazo.Value = Convert.ToInt16(this.comboPlazo.Text);
            this._header.VlrCuota.Value = 0;
            this._header.VlrCupoDisponible.Value = 0;
            this._header.VlrCapacidad.Value = 0;
            this._header.PagoVentanillaInd.Value = false;
            this._header.RechazoInd.Value = false;
            this._header.TipoCreditoID.Value = this.masterTipoCredito.Value;
            this._header.Codeudor1.Value = this.masterCodeudor1.Value;
            this._header.Codeudor2.Value = this.masterCodeudor2.Value;
            this._header.Codeudor3.Value = this.masterCodeudor3.Value;
            this._header.Observacion.Value = this.txtObservacion.Text;
            #endregion

            this.btnCambiarLibranza.Visible = false;
            return true;
        }

        /// <summary>
        /// Funcion que carga en el header los valores previamente guardados
        /// </summary>
        private void GetValues()
        {
            this.txtObsReversion.Text = this._ctrl.Observacion.Value;

            //Header
            this.lkp_TipoGarantia.EditValue = this._header.TipoGarantia.Value != null && this._header.TipoGarantia.Value.HasValue ? 
                this._header.TipoGarantia.Value.Value.ToString() : "1";

            this.masterCliente.Value = this._header.ClienteRadica.Value;
            this.txtPriApellido.Text = this._header.ApellidoPri.Value;
            this.txtSdoApellido.Text = this._header.ApellidoSdo.Value;
            this.txtPriNombre.Text = this._header.NombrePri.Value;
            this.txtSdoNombre.Text = this._header.NombreSdo.Value;
            this.txtLibranza.Text = this._header.Libranza.Value.ToString();
            this.masterAsesor.Value = this._header.AsesorID.Value;
            this.masterComercio.Value = this._header.ConcesionarioID.Value;
            this.masterCiudad.Value = this._header.Ciudad.Value;
            this.masterCentroPago.Value = this._header.CentroPagoID.Value;
            this.masterPagaduria.Value = this._header.PagaduriaID.Value;
            this.masterCooperativa.Value = this._header.CooperativaID.Value;
            this.masterLineaCredito.Value = this._header.LineaCreditoID.Value;
            this.chkCompraCartera.Checked = this._header.CompraCarteraInd.Value.Value;
            this.comboPlazo.Text = this._header.Plazo.Value.ToString();
            this.txtValor.EditValue = this._header.VlrSolicitado.Value;
            this.masterTipoCredito.Value = this._header.TipoCreditoID.Value;
            this.masterCodeudor1.Value = this._header.Codeudor1.Value;
            this.masterCodeudor2.Value = this._header.Codeudor2.Value;
            this.masterCodeudor3.Value = this._header.Codeudor3.Value;
            this.txtObservacion.Text = this._header.Observacion.Value;

            //Footer
            this.gcDocument.DataSource = this._anexos;

            //Variables
            this._libranzaID = this.txtLibranza.Text;
            this._clienteID = this.masterCliente.Value;
            this._centroPagoID = this.masterCentroPago.Value;
            this._pagaduriaID = this.masterPagaduria.Value;
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "Form_Closing"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "Form_FormClosed"));
            }
        }

        #endregion Eventos MDI

        #region Eventos Formulario

        /// <summary>
        /// Evento al cambiar el tiepo de crédito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterTipoCredito_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._tipoCreditoID != this.masterTipoCredito.Value)
                {
                    this.txtValor.Enabled = true;
                    this.txtValor.EditValue = 0;
                    this.comboPlazo.Enabled = true;
                    this.comboPlazo.Text = "12";
                    if (this.masterTipoCredito.ValidID)
                    {
                        this._tipoCreditoID = this.masterTipoCredito.Value;
                        DTO_ccTipoCredito tipoDTO = (DTO_ccTipoCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, false, this.masterTipoCredito.Value, true);

                        TipoCredito tipoCredito = (TipoCredito)Enum.Parse(typeof(TipoCredito), tipoDTO.TipoCredito.Value.Value.ToString());
                        
                        if (tipoCredito == TipoCredito.PolizaSinCredito)
                        {
                            this.txtValor.Enabled = false;
                            this.txtValor.EditValue = 0;
                        }
                    }
                    else
                    {
                        this.txtValor.EditValue = 0;
                        this.txtValor.Enabled = false;
                        string msg = string.Format(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.InvalidCode), this.masterTipoCredito.LabelRsx);
                        MessageBox.Show(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                this._tipoCreditoID = string.Empty;
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DigitacionCreditoFinanciera.cs", "masterCliente_Leave"));
            }

        }

        /// <summary>
        /// Trae los datos de coTercero, para almacenarlos en los campos de texto
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

                    DTO_ccCliente cliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, this.masterCliente.Value, true);
                    if (this.masterCliente.ValidID)
                    {
                        DTO_coTercero clienteTercero = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, cliente.TerceroID.Value, true);

                        this.txtPriApellido.Text = clienteTercero.ApellidoPri.ToString();
                        this.txtSdoApellido.Text = clienteTercero.ApellidoSdo.ToString();
                        this.txtPriNombre.Text = clienteTercero.NombrePri.ToString();
                        this.txtSdoNombre.Text = clienteTercero.NombreSdo.ToString();
                        this.masterCiudad.Value = cliente.LaboralCiudad.Value;
                        this.masterAsesor.Value = cliente.AsesorID.Value;
                        //this.masterCentroPago.Value = !string.IsNullOrWhiteSpace(cliente.CentroPago1.Value) ? cliente.CentroPago1.Value : this._centroPagoXDef;
                        this.masterCentroPago_Leave(sender, e);
                    }

                    if (Convert.ToInt32(this.lkp_TipoGarantia.EditValue) == (int)TipoGarantia.Real)
                    {
                        List<DTO_ccSolicitudDocu> creditosAll = _bc.AdministrationModel.GetSolicitudesByCliente(this.masterCliente.Value);
                        if (!string.IsNullOrEmpty(this.txtLibranza.Text))
                            creditosAll = creditosAll.FindAll(x => x.Libranza.Value == Convert.ToInt32(this.txtLibranza.Text));
                        List<DTO_ccSolicitudDocu> creditos = creditosAll.FindAll(
                            c => c.Estado.Value.Value == (byte)EstadoDocControl.SinAprobar || c.Estado.Value.Value == (byte)EstadoDocControl.ParaAprobacion);

                        #region Cartera Financiera (Real)
                        if (creditos.Count == 1)
                        {
                            this._isLoaded = false;
                            this._libranzaID = creditos[0].Libranza.Value.Value.ToString();
                            DTO_SolicitudLibranza solicitud = _bc.AdministrationModel.SolicitudLibranza_GetByLibranza(creditos[0].Libranza.Value.Value, string.Empty);

                            #region Credito existente

                            this._ctrl = solicitud.DocCtrl;
                            this._header = solicitud.Header;
                            this._anexos = solicitud.Anexos;

                            this.GetValues();
                            this.EnableHeader(true);

                            //Valida el tipo de crédito
                            this._tipoCreditoID = this.masterTipoCredito.Value;
                            DTO_ccTipoCredito tipoDTO = (DTO_ccTipoCredito)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccTipoCredito, false, this.masterTipoCredito.Value, true);

                            TipoCredito tipoCredito = (TipoCredito)Enum.Parse(typeof(TipoCredito), tipoDTO.TipoCredito.Value.Value.ToString());
                            if (tipoCredito == TipoCredito.PolizaSinCredito)
                            {
                                this.txtValor.Enabled = false;
                                this.txtValor.EditValue = 0;
                            }

                            if (!this._isLoaded)
                                this.masterPagaduria_Leave(sender, e);

                            this.gcDocument.Enabled = true;

                            #endregion
                        }
                        else if (creditos.Count > 1)
                        {
                            #region Mas de un crédito

                            string creditosStr = string.Empty;
                            for (int i = 0; i < creditos.Count; ++i)
                            {
                                creditosStr += creditos[i].Libranza.Value.Value.ToString();
                                if (i != creditos.Count - 1)
                                    creditosStr += ",";
                            }

                            string msg = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_MultiplesCreditos);
                            MessageBox.Show(string.Format(msg, creditosStr));
                            CleanData();

                            this.masterCliente.Focus();
                            this.masterCliente.Value = cliente.ID.Value;
                            this.lkp_TipoGarantia.EditValue = "3";

                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "masterCliente_Leave"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterCentroPago_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._centroPagoID != this.masterCentroPago.Value)
                {
                    this._centroPagoID = this.masterCentroPago.Value;

                    if (this.masterCentroPago.ValidID)
                    {
                        DTO_ccCentroPagoPAG centroPago = (DTO_ccCentroPagoPAG)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCentroPagoPAG, false, masterCentroPago.Value, true);
                        this.masterPagaduria.Value = centroPago.PagaduriaID.Value;
                        this.masterPagaduria_Leave(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "masterCentroPago_Leave"));
            }

        }

        /// <summary>
        /// Evento que carga los documentos anexos con base a una pagaduria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void masterPagaduria_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this._pagaduriaID != this.masterPagaduria.Value)
                {
                    this._pagaduriaID = this.masterPagaduria.Value;
                    this._anexos = new List<DTO_ccSolicitudAnexo>();

                    if (this.masterPagaduria.ValidID)
                    {
                        List<DTO_MasterBasic> _pagaduriaAnexos = this._bc.AdministrationModel.ccAnexosLista_GetByPagaduria(this._pagaduriaID);
                        foreach (DTO_MasterBasic basic in _pagaduriaAnexos)
                        {
                            DTO_ccSolicitudAnexo anexo = new DTO_ccSolicitudAnexo();
                            anexo.DocumListaID.Value = basic.ID.Value;
                            anexo.Descriptivo.Value = basic.Descriptivo.Value;
                            anexo.Descripcion.Value = String.Empty;
                            anexo.IncluidoInd.Value = false;

                            this._anexos.Add(anexo);
                        }
                    }
                    this._isLoaded = true;
                    this.gcDocument.DataSource = this._anexos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "masterPagaduria_Leave"));
            }

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
                if (this._libranzaID != this.txtLibranza.Text.Trim())
                {
                    this._isLoaded = false;
                    string tmp = this.txtLibranza.Text;
                    this._libranzaID = this.txtLibranza.Text.Trim();
                    int libranzaTemp = Convert.ToInt32(this._libranzaID);
                    DTO_SolicitudLibranza solicitud = _bc.AdministrationModel.SolicitudLibranza_GetByLibranza(libranzaTemp, this.nextActID);

                    if (solicitud.DocCtrl != null)
                    {
                        #region Credito existente

                        this._ctrl = solicitud.DocCtrl;
                        this._header = solicitud.Header;
                        this._anexos = solicitud.Anexos;

                        if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.SinAprobar || (this._header.DevueltaInd.Value.HasValue && this._header.DevueltaInd.Value.Value))
                        {
                            this.GetValues();
                            this.EnableHeader(true);

                            if (!this._isLoaded)
                                this.masterPagaduria_Leave(sender, e);

                            this.gcDocument.Enabled = true;
                        }
                        else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.ParaAprobacion)
                        {
                            //Desabilita la grilla
                            this.btnCambiarLibranza.Visible = true;
                            this.GetValues();
                            this.EnableHeader(false);

                            if (!this._isLoaded)
                                this.masterPagaduria_Leave(sender, e);

                            //this.gcDocument.Enabled = false;
                            //FormProvider.Master.itemSave.Enabled = false;

                        }
                        else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Aprobado)
                        {
                            //Mostrar mensaje de que esta libranza esta cerrada
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaAprobada));
                            CleanData();

                            this.txtLibranza.Focus();
                        }
                        else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                        {
                            //Mostrar mensaje de que esta libranza esta cerrada
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                            CleanData();

                            this.txtLibranza.Focus();
                        }

                        #endregion
                    }
                    else
                    {
                        this.CleanData();
                        this.txtLibranza.Text = tmp;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "txtLibranza_Leave"));
            }
        }

        /// <summary>
        /// Boton para cambiar libranza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiarLibranza_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtLibranza.Enabled = true;
                FormProvider.Master.itemSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "btnCambiarLibranza_Click"));
            }
        }

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
        /// Cambio del tipo de garantía
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkp_TipoGarantia_EditValueChanged(object sender, EventArgs e)
        {
            if (this._ctrl != null && ( this._ctrl.Estado.Value == (int)EstadoDocControl.Aprobado || 
                                        this._ctrl.Estado.Value == (int)EstadoDocControl.Cerrado ||
                                        this._ctrl.Estado.Value == null))
                this.CleanData();
            int value = Convert.ToInt32(this.lkp_TipoGarantia.EditValue);

            if(value == (int)TipoGarantia.Libranza)
            {
                this.anexosContainer.Visible = true;
                this.txtLibranza.Enabled = true;
                this.txtLibranza.Text = string.Empty;
                this.masterCentroPago.EnableControl(true); 
                this.masterCentroPago.Value = string.Empty;
                this.masterPagaduria.Value = string.Empty;
                this.txtLibranza.Focus();
            }
            else if (value == (int)TipoGarantia.TipoDocumento)
            {
                this.anexosContainer.Visible = true;
                this.txtLibranza.Enabled = true;
                this.txtLibranza.Text = string.Empty;
                this.masterCentroPago.EnableControl(false);
                this.masterCentroPago.Value = this._centroPagoXDef;
                this.masterPagaduria.Value = this._pagaduriaXDef;
                this.txtLibranza.Focus();
            }
            else
            {
                this.anexosContainer.Visible = false;
                //this.txtLibranza.Enabled = false;
                this.txtLibranza.Text = "0";
                this.masterCentroPago.EnableControl(false);
                this.masterCentroPago.Value = this._centroPagoXDef;
                this.masterPagaduria.Value = this._pagaduriaXDef;
                this.masterCliente.Focus();
            }
        }

        /// <summary>
        /// Deshabilita el scroll del spin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_Spin(object sender, SpinEventArgs e)
        {
            e.Handled = true;
        }

        #endregion Eventos Formulario

        #region Eventos Grilla

        /// <summary>
        /// Maneja campos de controles en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDocument_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "IncluidoInd")
                {
                    e.RepositoryItem = this.editChkBox;
                }

                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Asigna los valores a cada columna de la grilla
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            Object dto = (Object)e.Row;
            string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);

            if (e.IsGetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else
                    {
                        e.Value = pi.PropertyType.GetProperty("Value").GetValue(pi.GetValue(dto, null), null);
                    }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (fi.FieldType.Name == "String" || fi.FieldType.Name == "Int16" || fi.FieldType.Name == "Int32" || fi.FieldType.Name == "Double")
                        {
                            e.Value = fi.GetValue(dto);
                        }
                        else
                        {
                            e.Value = fi.FieldType.GetProperty("Value").GetValue(fi.GetValue(dto), null);
                        }
                    }
                }
            }
            if (e.IsSetData)
            {
                PropertyInfo pi = dto.GetType().GetProperty(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                if (e.Value == null)
                    e.Value = String.Empty;
                if (pi != null)
                {
                    if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                    {
                        e.Value = pi.GetValue(dto, null);
                    }
                    else if (pi.PropertyType.Name == "UDTSQL_smalldatetime")
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(Convert.ToDateTime(e.Value).ToShortDateString());
                    }
                    else
                    {
                        UDT udtProp = (UDT)pi.GetValue(dto, null);
                        udtProp.SetValueFromString(e.Value.ToString());
                    }
                }
                else
                {
                    FieldInfo fi = dto.GetType().GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        if (pi.PropertyType.Name == "String" || pi.PropertyType.Name == "Int16" || pi.PropertyType.Name == "Int32" || pi.PropertyType.Name == "Double")
                        {
                            //e.Value = pi.GetValue(dto, null);
                        }
                        else
                        {
                            UDT udtProp = (UDT)fi.GetValue(dto);
                            udtProp.SetValueFromString(e.Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Asigna controles a la grilla cuando entra a edicion de una celda
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void gvDocument_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                string fieldName = e.Column.FieldName.Substring(this._unboundPrefix.Length);
                if (fieldName == "Descripcion")
                {
                    e.RepositoryItem = this.riPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Eventos Grilla

        #region Eventos editor de texto

        /// <summary>
        /// Toma los valores de la grilla y los envia al popup al momento de abrirlo
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryPopUp(object sender, CancelEventArgs e)
        {
            string fieldName = this.gvDocument.FocusedColumn.FieldName.Substring(this._unboundPrefix.Length);
            if (fieldName == "Descripcion")
                this.richEditControl.Document.Text = this.gvDocument.GetFocusedRowCellValue(fieldName).ToString();
        }

        /// <summary>
        /// Toma los valores ingresados en el popup al momento de cerrarlo y los envia a la celda de la grilla 
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void riPopup_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
            e.Value = this.richEditControl.Document.Text;
        }

        #endregion

        #region Eventos Barra Herramientas

        /// <summary>
        /// Boton para crear nuevo registro
        /// </summary>
        public override void TBNew()
        {
            try
            {
                this.CleanData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "TBNew"));
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
                    this.gvDocument.PostEditor();
                    DTO_SolicitudLibranza solLibranza = new DTO_SolicitudLibranza();
                    List<DTO_ccTareaChequeoLista> tareas = new List<DTO_ccTareaChequeoLista>();
                    solLibranza.AddData(this._ctrl, this._header, this._anexos, tareas);

                    if (this._ctrl.NumeroDoc.Value.HasValue && this._ctrl.NumeroDoc.Value != 0)
                    {
                        string msgTitleWarning = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Title_Warning);
                        string msgDoc = this._bc.GetResource(LanguageTypes.Messages, "La edición de la solicitud eliminará los datos de preliquidación si existen, desea continuar");
                        if (MessageBox.Show(msgDoc, msgTitleWarning, MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    DTO_TxResult result = _bc.AdministrationModel.SolicitudLibranza_Add(this._documentID, solLibranza);
                    if (result.Result == ResultValue.OK)
                    {
                        #region Obtiene el nombre

                        string nombre = this._header.NombrePri.Value;
                        if (!string.IsNullOrWhiteSpace(this._header.NombreSdo.Value))
                            nombre += " " + this._header.NombreSdo.Value;
                        if (!string.IsNullOrWhiteSpace(this._header.ApellidoPri.Value))
                            nombre += " " + this._header.ApellidoPri.Value;
                        if (!string.IsNullOrWhiteSpace(this._header.ApellidoSdo.Value))
                            nombre += " " + this._header.ApellidoSdo.Value;

                        #endregion
                        #region Variables para el mail

                        DTO_seUsuario user = _bc.AdministrationModel.seUsuario_GetUserbyID(this._actFlujo.seUsuarioID.Value);

                        string body = string.Empty;
                        string subject = string.Empty;
                        string email = user.CorreoElectronico.Value;

                        string subjectApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_DocSendToAppr_Subject);
                        string bodyApr = _bc.GetResource(LanguageTypes.Mail, DictionaryMessages.Mail_SentToAprobCartera_Body);
                        string formName = _bc.GetResource(LanguageTypes.Forms, this._documentID.ToString());

                        #endregion
                        #region Envia el correo
                        subject = string.Format(subjectApr, formName);
                        body = string.Format(bodyApr, formName, this.txtLibranza.Text.Trim(), nombre, this._header.Observacion.Value);
                        _bc.SendMail(this._documentID, subject, body, email);
                        #endregion

                        //Actualiza el control para las financieras
                        string sectorLib = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SectorCartera);
                        if (sectorLib == ((byte)SectorCartera.Financiero).ToString()) //Financieras
                        {
                            string numeroControl = _bc.AdministrationModel.Empresa.NumeroControl.Value;
                            _bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numeroControl).ToList();
                        }

                        this.CleanData();
                        this.txtLibranza.Focus();

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-OperacionesProactivas.cs", "TBSave"));
            }
        }

        #endregion Eventos Barra Herramientas


    }
}