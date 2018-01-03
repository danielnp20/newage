
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

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class DetallePerfil : FormWithToolbar
    {
        #region Variables

        private BaseController _bc = BaseController.GetInstance();

        //Para manejo de propiedades
        private FormTypes _frmType = FormTypes.DocumentAprob;

        private string _frmName;
        private string _libranzaID;
        private int _documentID;
        private ModulesPrefix _frmModule;
        private string _unboundPrefix = "Unbound_";

        //DTOs        
        private DTO_glDocumentoControl _ctrl = new DTO_glDocumentoControl();
        private DTO_DigitaSolicitudDecisor _data = new DTO_DigitaSolicitudDecisor();
        private DTO_glActividadFlujo _actFlujo = new DTO_glActividadFlujo();

        //Identificador de la proxima actividad
        private string nextActID;

        //Variables ToolBar
        private bool _isLoaded;
        private bool _readOnly = false;

        private DateTime periodo = DateTime.Now;

        #endregion Variables

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DetallePerfil()
        {
            this.Constructor();
        }

        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public DetallePerfil(string cliente, int libranzaNro, bool readOnly, int Hoja)
        {
            this.Constructor(cliente, libranzaNro, readOnly,Hoja);
        }

        /// <summary>
        /// Constructor del formulario
        /// </summary>
        public void Constructor(string cliente = null, int libranzaNro = 0,bool readOnly = false, int hoja=0)
        {
            InitializeComponent();
            try
            {   
                this.SetInitParameters();
                this.groupControl5.Visible = false;
                this.groupControl6.Visible = false;
                this.groupControl11.Visible = false;
                this.groupControl16.Visible = false;
                this.groupControl21.Visible = false;
                this.groupControl26.Visible = false;
                this.groupControl31.Visible = false;
                this.groupControl36.Visible = false;
                this.groupControl41.Visible = false;
                this.groupControl46.Visible = false;
                this.groupControl51.Visible = false;
                this.groupControl53.Visible = false;
                
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
                this.LoadData(cliente, libranzaNro);
                this._readOnly = readOnly;

                this.tabControl.SelectedTabPageIndex = hoja;
                switch (hoja)
                {
                    case 0:
                        this._frmName = "Finca Raiz";
                        this.Text = "Finca Raiz";
                        this.label111.Text="Finca Raiz";
                        break;
                    case 1:
                        this.Text = "Probabilidad";
                        this.label111.Text="Probabilidad";
                        break;
                    case 2:
                        this.label111.Text = "Mora Actual";
                        break;
                    case 3:
                        this.label111.Text = "Mora Ultimo Año";
                        break;
                    case 4:
                        this.label111.Text = "Reportes Negativos";
                        break;
                    case 5:
                        this.label111.Text = "Estado Actual";
                        break;
                    case 6:
                        this.label111.Text = "Estabilidad";
                        break;
                    case 7:
                        this.label111.Text = "Ubicabilidad";
                        break;
                    case 8:
                        this.label111.Text = "% Max Financiacion";
                        break;
                    case 9:
                        this.label111.Text = "Endeudamiento";
                        break;
                    case 10:
                        this.label111.Text = "Cuotas";
                        break;
                    case 11:
                        this.label111.Text = "Ingreso";
                        break;
                    case 12:
                        this.label111.Text = "Capacidad de Pago";
                        break;


                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "Constructor"));
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
                this._documentID = AppQueries.QueryDetallePerfil;
                this._frmModule = ModulesPrefix.dr;


                this.linkConyuge.Dock = DockStyle.Fill;
                this.linkCodeudor1.Dock = DockStyle.Fill;
                this.linkCodeudor2.Dock = DockStyle.Fill;
                this.linkCodeudor3.Dock = DockStyle.Fill;

                this.linkConyuge0.Dock = DockStyle.Fill;
                this.linkCodeudor10.Dock = DockStyle.Fill;
                this.linkCodeudor20.Dock = DockStyle.Fill;
                this.linkCodeudor30.Dock = DockStyle.Fill;

                this.linkConyuge1.Dock = DockStyle.Fill;
                this.linkCodeudor11.Dock = DockStyle.Fill;
                this.linkCodeudor21.Dock = DockStyle.Fill;
                this.linkCodeudor31.Dock = DockStyle.Fill;

                this.linkConyuge2.Dock = DockStyle.Fill;
                this.linkCodeudor12.Dock = DockStyle.Fill;
                this.linkCodeudor22.Dock = DockStyle.Fill;
                this.linkCodeudor32.Dock = DockStyle.Fill;

                this.linkConyuge3.Dock = DockStyle.Fill;
                this.linkCodeudor13.Dock = DockStyle.Fill;
                this.linkCodeudor23.Dock = DockStyle.Fill;
                this.linkCodeudor33.Dock = DockStyle.Fill;

                this.linkConyuge4.Dock = DockStyle.Fill;
                this.linkCodeudor14.Dock = DockStyle.Fill;
                this.linkCodeudor24.Dock = DockStyle.Fill;
                this.linkCodeudor34.Dock = DockStyle.Fill;

                this.linkConyuge5.Dock = DockStyle.Fill;
                this.linkCodeudor15.Dock = DockStyle.Fill;
                this.linkCodeudor25.Dock = DockStyle.Fill;
                this.linkCodeudor35.Dock = DockStyle.Fill;

                this.linkConyuge6.Dock = DockStyle.Fill;
                this.linkCodeudor16.Dock = DockStyle.Fill;
                this.linkCodeudor26.Dock = DockStyle.Fill;
                this.linkCodeudor36.Dock = DockStyle.Fill;

                this.linkConyuge7.Dock = DockStyle.Fill;
                this.linkCodeudor17.Dock = DockStyle.Fill;
                this.linkCodeudor27.Dock = DockStyle.Fill;
                this.linkCodeudor37.Dock = DockStyle.Fill;

                this.linkConyuge8.Dock = DockStyle.Fill;
                this.linkCodeudor18.Dock = DockStyle.Fill;
                this.linkCodeudor28.Dock = DockStyle.Fill;
                this.linkCodeudor38.Dock = DockStyle.Fill;

                this.linkConyuge9.Dock = DockStyle.Fill;
                this.linkCodeudor19.Dock = DockStyle.Fill;
                this.linkCodeudor29.Dock = DockStyle.Fill;
                this.linkCodeudor39.Dock = DockStyle.Fill;

                this.linkConyuge10.Dock = DockStyle.Fill;
                this.linkCodeudor110.Dock = DockStyle.Fill;
                this.linkCodeudor210.Dock = DockStyle.Fill;
                this.linkCodeudor310.Dock = DockStyle.Fill;

                this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;                
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "SetInitParameters"));
            }
        }

        /// <summary>
        /// Refresca el contenido del formulario
        /// </summary>
        private void CleanData()
        {


            //Footer
            this.EnableHeader(true);
            this._ctrl = new DTO_glDocumentoControl();
            this._data = new DTO_DigitaSolicitudDecisor();


            FormProvider.Master.itemSave.Enabled = true;
        }

        /// <summary>
        /// Funcion que no permite editar los campos del header
        /// </summary>
        /// <param name="enabled">Indicador para ver si se habilitan o deshabilitan los campos de la grilla</param>
        private void EnableHeader(bool enabled)
        {

        }

      

        /// <summary>
        /// Verifiva que el header sea valido
        /// </summary>
        /// <returns></returns>
        private bool ValidateHeader()
        {
            string result = string.Empty;
            string msgVacio = this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.EmptyField_Col);
            #region Hace las Validaciones


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
                DTO_drSolicitudDatosOtros otros = this._data.OtrosDatos;
                DTO_ccSolicitudDataCreditoDatos datCredDeudor = this._data.DataCreditoDatos.Find(x => x.TipoId.Value == "1");
                DTO_ccSolicitudDataCreditoDatos datCredConyuge = this._data.DataCreditoDatos.Find(x => x.TipoId.Value == "2");
                DTO_ccSolicitudDataCreditoDatos datCredCodeudor1 = this._data.DataCreditoDatos.Find(x => x.TipoId.Value == "3");
                DTO_ccSolicitudDataCreditoDatos datCredCodeudor2 = this._data.DataCreditoDatos.Find(x => x.TipoId.Value == "4");
                DTO_ccSolicitudDataCreditoDatos datCredcodeudor3 = this._data.DataCreditoDatos.Find(x => x.TipoId.Value == "5");
                DTO_ccSolicitudDataCreditoScore ScoreDeudor = this._data.DataCreditoScore.Find(x => x.TipoId.Value == "1");
                DTO_ccSolicitudDataCreditoScore ScoreConyuge = this._data.DataCreditoScore.Find(x => x.TipoId.Value == "2");
                DTO_ccSolicitudDataCreditoScore ScoreCodeudor1 = this._data.DataCreditoScore.Find(x => x.TipoId.Value == "3");
                DTO_ccSolicitudDataCreditoScore ScoreCodeudor2 = this._data.DataCreditoScore.Find(x => x.TipoId.Value == "4");
                DTO_ccSolicitudDataCreditoScore Scorecodeudor3 = this._data.DataCreditoScore.Find(x => x.TipoId.Value == "5");                


                if (isGet)
                {
                    #region Asigna datos a los controles
                    //Deudor (TipoPersona 1)
                    this.lblVersion.Text = "Vers: " + (this._data.SolicituDocu.VersionNro.Value.HasValue ? this._data.SolicituDocu.VersionNro.Value.ToString() : "1");
                    #region deudor
                    if (deudor != null)
                    {
                        #region Finca Raiz

                        this.txtInmuebleDeudor.EditValue = deudor.NroInmuebles.Value;
                        this.txtHipotecasDeudor.EditValue = deudor.HipotecasNro.Value;
                        this.txtRestriccionDeudor.EditValue = deudor.RestriccionesNro.Value;
                        this.txtPorcTablaDeudor.EditValue = deudor.PF_FincaRaizDato.Value;
                        this.txtFactorDeudor.EditValue = deudor.PF_FincaRaiz.Value;
                        #endregion
                        #region Probabilidad
                        this.txtDeudor101.EditValue = deudor.PF_MorasUltAno.Value;
                        this.txtDeudor102.EditValue = ScoreDeudor.Puntaje.Value;
                        this.txtDeudor103.EditValue = deudor.PF_FactorAcierta.Value;
                        this.txtDeudor104.EditValue = deudor.PF_AciertaResultado.Value;
                        #endregion
                        #region Mora Actual
                        this.txtDeudor201.EditValue = deudor.PF_MorasActuales.Value;
                        this.txtDeudor202.EditValue = datCredDeudor.EstadoAct30.Value;
                        this.txtDeudor203.EditValue = deudor.PF_MorasAct30Dato.Value;
                        this.txtDeudor204.EditValue = deudor.PF_MorasAct30.Value;
                        this.txtDeudor205.EditValue = datCredDeudor.EstadoAct60.Value;
                        this.txtDeudor206.EditValue = deudor.PF_MorasAct60Dato.Value;
                        this.txtDeudor207.EditValue = deudor.PF_MorasAct60.Value;
                        this.txtDeudor208.EditValue = datCredDeudor.EstadoAct90.Value;
                        this.txtDeudor209.EditValue = deudor.PF_MorasAct90Dato.Value;
                        this.txtDeudor210.EditValue = deudor.PF_MorasAct90.Value;
                        this.txtDeudor211.EditValue = datCredDeudor.EstadoAct120.Value;
                        this.txtDeudor212.EditValue = deudor.PF_MorasAct120Dato.Value;
                        this.txtDeudor213.EditValue = deudor.PF_MorasAct120.Value;
                        #endregion
                        #region Mora Ultimo Año
                        this.txtDeudor301.EditValue = deudor.PF_MorasUltAno.Value;
                        this.txtDeudor302.EditValue = datCredDeudor.EstadoHis30.Value;
                        this.txtDeudor303.EditValue = deudor.PF_MorasUlt30Dato.Value;
                        this.txtDeudor304.EditValue = deudor.PF_MorasUlt30.Value;
                        this.txtDeudor305.EditValue = datCredDeudor.EstadoHis60.Value;
                        this.txtDeudor306.EditValue = deudor.PF_MorasUlt60Dato.Value;
                        this.txtDeudor307.EditValue = deudor.PF_MorasUlt60.Value;
                        this.txtDeudor308.EditValue = datCredDeudor.EstadoHis90.Value;
                        this.txtDeudor309.EditValue = deudor.PF_MorasUlt90Dato.Value;
                        this.txtDeudor310.EditValue = deudor.PF_MorasUlt90.Value;
                        this.txtDeudor311.EditValue = datCredDeudor.EstadoHis120.Value;
                        this.txtDeudor312.EditValue = deudor.PF_MorasUlt120Dato.Value;
                        this.txtDeudor313.EditValue = deudor.PF_MorasUlt120.Value;
                        #endregion
                        #region Reportes Negativos
                        this.txtDeudor401.EditValue = deudor.PF_RepNegativos.Value;
                        this.txtDeudor402.EditValue = datCredDeudor.EstadoActCob.Value;
                        this.txtDeudor403.EditValue = deudor.PF_ObligacionCOBDato.Value;
                        this.txtDeudor404.EditValue = deudor.PF_ObligacionCOB.Value;
                        this.txtDeudor405.EditValue = datCredDeudor.EstadoActDud.Value;
                        this.txtDeudor406.EditValue = deudor.PF_ObligacionDUDDato.Value;
                        this.txtDeudor407.EditValue = deudor.PF_ObligacionDUD.Value;
                        this.txtDeudor408.EditValue = datCredDeudor.EstadoActCas.Value;
                        this.txtDeudor409.EditValue = deudor.PF_ObligacionCASDato.Value;
                        this.txtDeudor410.EditValue = deudor.PF_ObligacionCAS.Value;
                        this.txtDeudor411.EditValue = datCredDeudor.CtasEmbargadas.Value;
                        this.txtDeudor412.EditValue = deudor.PF_ObligacionEMBDato.Value;
                        this.txtDeudor413.EditValue = deudor.PF_ObligacionEMB.Value;
                        this.txtDeudor414.EditValue = datCredDeudor.EstadoHisRec.Value;
                        this.txtDeudor415.EditValue = deudor.PF_ObligacionRECDato.Value;
                        this.txtDeudor416.EditValue = deudor.PF_ObligacionREC.Value;
                        this.txtDeudor417.EditValue = datCredDeudor.CtasMalManejo.Value;
                        this.txtDeudor418.EditValue = deudor.PF_ObligacionCANDato.Value;
                        this.txtDeudor419.EditValue = deudor.PF_ObligacionCAN.Value;
                        #endregion
                        #region Estado Actual
                        this.txtDeudor501.EditValue = deudor.PF_EstadoActual.Value;
                        this.txtDeudor502.EditValue = datCredDeudor.NumeObligACT.Value;
                        this.txtDeudor503.EditValue = deudor.PF_PorObligacionesDato.Value;
                        this.txtDeudor504.EditValue = deudor.PF_PorObligaciones.Value;
                        this.txtDeudor505.EditValue = datCredDeudor.NumeroTDC.Value;
                        this.txtDeudor506.EditValue = deudor.PF_PorUtilizaTDCDato.Value;
                        this.txtDeudor507.EditValue = deudor.PF_PorUtilizaTDC.Value;
                        this.txtDeudor508.EditValue = datCredDeudor.PeorEndeudT2.Value;
                        this.txtDeudor509.EditValue = deudor.PF_PeorCalificacionDato.Value;
                        this.txtDeudor510.EditValue = deudor.PF_PeorCalificacion.Value;
                        this.txtDeudor511.EditValue = datCredDeudor.UltConsultas.Value;
                        this.txtDeudor512.EditValue = deudor.PF_Consultas6MesesDato.Value;
                        this.txtDeudor513.EditValue = deudor.PF_Consultas6Meses.Value;
                        #endregion
                        #region Estabilidad
                        this.txtDeudor601.EditValue = deudor.PF_Estabilidad.Value;
                        this.txtDeudor602.EditValue = deudor.PF_DireccDesdeMeses.Value;
                        this.txtDeudor603.EditValue = deudor.PF_DireccDesdeDato.Value;
                        this.txtDeudor604.EditValue = deudor.PF_DireccDesde.Value;
                        this.txtDeudor605.EditValue = 0;
                        this.txtDeudor606.EditValue = deudor.PF_EntidadesNumDato.Value;
                        this.txtDeudor607.EditValue = deudor.PF_EntidadesNum.Value;
                        this.txtDeudor608.EditValue = deudor.PF_CelularDesdeMeses.Value;
                        this.txtDeudor609.EditValue = deudor.PF_CelularDesdeDato.Value;
                        this.txtDeudor610.EditValue = deudor.PF_CelularDesde.Value;
                        this.txtDeudor611.EditValue = deudor.PF_CorreoDesdeMeses.Value;
                        this.txtDeudor612.EditValue = deudor.PF_CorreoDesdeDato.Value;
                        this.txtDeudor613.EditValue = deudor.PF_CorreoDesde.Value;
                        #endregion
                        #region Ubicabilidad
                        this.txtDeudor701.EditValue = deudor.PF_Ubicabilidad.Value;
                        this.txtDeudor702.EditValue = deudor.PF_DireccionNumCant.Value;
                        this.txtDeudor703.EditValue = deudor.PF_DireccionNumDato.Value;
                        this.txtDeudor704.EditValue = deudor.PF_DireccionNum.Value;
                        this.txtDeudor705.EditValue = deudor.PF_TelefonoNumCant.Value;
                        this.txtDeudor706.EditValue = deudor.PF_TelefonoNumDato.Value;
                        this.txtDeudor707.EditValue = deudor.PF_TelefonoNum.Value;
                        this.txtDeudor708.EditValue = deudor.PF_CelularNumCant.Value;
                        this.txtDeudor709.EditValue = deudor.PF_CelularNumDato.Value;
                        this.txtDeudor710.EditValue = deudor.PF_CelularNum.Value;
                        this.txtDeudor711.EditValue = deudor.PF_CorreoNumCant.Value;
                        this.txtDeudor712.EditValue = deudor.PF_CorreoNumDato.Value;
                        this.txtDeudor713.EditValue = deudor.PF_CorreoNum.Value;                   
                        #endregion

                        #region % Max financiacion
                        this.txtDeudor801.EditValue = deudor.PF_PorMaxFincaRaiz.Value;
                        this.txtDeudor802.EditValue = deudor.PF_PorMaxMorasActuales.Value;
                        this.txtDeudor803.EditValue = deudor.PF_PorMaxMorasUltAno.Value;
                        this.txtDeudor804.EditValue = deudor.PF_PorMaxRepNegativos.Value;
                        this.txtDeudor805.EditValue = deudor.PF_PorMaxEstadoActual.Value;
                        this.txtDeudor806.EditValue = deudor.PF_PorMaxProbabilidad.Value;
                        this.txtDeudor807.EditValue = deudor.PF_PorMaxEstabilidad.Value;
                        this.txtDeudor808.EditValue = deudor.PF_PorMaxUbicabilidad.Value;
                        this.txtDeudor809.EditValue = Convert.ToDecimal(deudor.PF_PorMaxFincaRaiz.Value) + Convert.ToDecimal(deudor.PF_PorMaxMorasActuales.Value)
                            + Convert.ToDecimal(deudor.PF_PorMaxMorasUltAno.Value) + Convert.ToDecimal(deudor.PF_PorMaxRepNegativos.Value)
                            + Convert.ToDecimal(deudor.PF_PorMaxEstadoActual.Value) + Convert.ToDecimal(deudor.PF_PorMaxProbabilidad.Value)
                            + Convert.ToDecimal(deudor.PF_PorMaxEstabilidad.Value) + Convert.ToDecimal(deudor.PF_PorMaxUbicabilidad.Value);
                        
                        #endregion
                        # region otras tablas
                        this.txtDeudor810.EditValue = deudor.PF_PorMaxFinancia.Value;
                        this.txtDeudor811.EditValue = deudor.PF_PorMaxFinDeuCon.Value;
                        this.txtDeudor812.EditValue = otros.PF_PlazoFinal.Value;
                        this.txtDeudor813.EditValue = deudor.PF_CapacidadPago.Value;
                        this.txtDeudor814.EditValue = deudor.PF_CapPagAdCon.Value;
                        this.txtDeudor815.EditValue = deudor.PF_CapPagAdDeu.Value;

                        this.txtOtros801.EditValue = otros.SMMLV.Value;
                        this.txtOtros802.EditValue = otros.PF_VlrMinimoGar.Value;

                        this.txtOtros803.EditValue = otros.PF_VlrMinimoFirma2.Value;
                        this.txtOtros804.EditValue = otros.PF_VlrMinimoFirma3.Value;

                        # endregion

                        #region Endeudamiento
                        this.txtDeudor901.EditValue = datCredDeudor.VlrSaldoVIV.Value;
                        this.txtDeudor902.EditValue = datCredDeudor.VlrSaldoBAN.Value;
                        this.txtDeudor903.EditValue = datCredDeudor.VlrSaldoFIN.Value;
                        this.txtDeudor904.EditValue = datCredDeudor.VlrSaldoCOP.Value;
                        this.txtDeudor905.EditValue = datCredDeudor.VlrUtilizadoTDC.Value;
                        this.txtDeudor906.EditValue = datCredDeudor.VlrSaldoREA.Value;
                        this.txtDeudor907.EditValue = datCredDeudor.VlrCuotasCEL.Value;
                        this.txtDeudor908.EditValue = Convert.ToDecimal(datCredDeudor.VlrSaldoVIV.Value) + Convert.ToDecimal(datCredDeudor.VlrSaldoBAN.Value)
                             + Convert.ToDecimal(datCredDeudor.VlrSaldoFIN.Value) + Convert.ToDecimal(datCredDeudor.VlrSaldoCOP.Value)
                             + Convert.ToDecimal(datCredDeudor.VlrUtilizadoTDC.Value) + Convert.ToDecimal(datCredDeudor.VlrCuotasCEL.Value) + Convert.ToDecimal(datCredDeudor.VlrSaldoREA.Value);
                        #endregion


                        #region Cuotas
                        this.txtDeudor1001.EditValue = datCredDeudor.VlrCuotasVIV.Value;
                        this.txtDeudor1002.EditValue = deudor.PF_EstCtasVIV.Value;
                        this.txtDeudor1003.EditValue = deudor.PF_CtasTotVIV.Value;
                        this.txtDeudor1004.EditValue = datCredDeudor.VlrCuotasBAN.Value;
                        this.txtDeudor1005.EditValue = deudor.PF_EstCtasBAN.Value;
                        this.txtDeudor1006.EditValue = deudor.PF_CtasTotBAN.Value;
                        this.txtDeudor1007.EditValue = datCredDeudor.VlrCuotasFIN.Value;
                        this.txtDeudor1008.EditValue = deudor.PF_EstCtasFIN.Value;
                        this.txtDeudor1009.EditValue = deudor.PF_CtasTotFIN.Value;
                        this.txtDeudor1010.EditValue = datCredDeudor.VlrCuotasCOP.Value;
                        this.txtDeudor1011.EditValue = deudor.PF_EstCtasCOP.Value;
                        this.txtDeudor1012.EditValue = deudor.PF_CtasTotCOP.Value;
                        this.txtDeudor1013.EditValue = datCredDeudor.VlrCuotasTDC.Value;
                        this.txtDeudor1014.EditValue = deudor.PF_EstCtasTDC.Value;
                        this.txtDeudor1015.EditValue = deudor.PF_CtasTotTDC.Value;
                        this.txtDeudor1016.EditValue = datCredDeudor.VlrCuotasREA.Value;
                        this.txtDeudor1017.EditValue = deudor.PF_EstCtasREA.Value;
                        this.txtDeudor1018.EditValue = deudor.PF_CtasTotREA.Value;
                        this.txtDeudor1019.EditValue = datCredDeudor.VlrCuotasCEL.Value;
                        this.txtDeudor1020.EditValue = deudor.PF_EstCtasCEL.Value;
                        this.txtDeudor1021.EditValue = deudor.PF_CtasTotCEL.Value;
                        this.txtDeudor1022.EditValue = deudor.PF_CtasTotIngEst.Value;
                        #endregion

                        #region Endeudamiento
                        this.txtDeudor1101.EditValue = deudor.PF_QuiantiMIN.Value;
                        this.txtDeudor1102.EditValue = deudor.PF_QuiantiMAX.Value;
                        this.txtDeudor1103.EditValue = deudor.PF_QuantoIngrEst.Value;
                        this.txtDeudor1104.EditValue = deudor.PF_IngrEstxQuanto.Value;
                        this.txtDeudor1105.EditValue = deudor.IngresosREG.Value;
                        this.txtDeudor1106.EditValue = deudor.PF_FactIngresosREG.Value;
                        this.txtDeudor1107.EditValue = deudor.IngresosSOP.Value;
                        this.txtDeudor1108.EditValue = deudor.PF_IngrCapacPAG.Value;
                        this.txtDeudor1109.EditValue = deudor.PF_PorIngrAporta.Value;
                        this.chkDeudor1110.EditValue = deudor.PF_ReqSopIngrInd.Value;
                        this.txtDeudor1111.EditValue = deudor.PF_PorIngrPagoCtas.Value;
                        this.txtDeudor1112.EditValue = deudor.PF_IngrDispPagoCtas.Value;
                        this.txtDeudor1113.EditValue = deudor.PF_CuotasACT.Value;
                        this.txtDeudor1114.EditValue = deudor.PF_IngrDispApoyos.Value;
                        #endregion

                    }
                    else
                    {
                    }
                    #endregion
                    //Conyuge (TipoPersona 2)
                    #region Conyuge
                    if (conyuge != null)
                    {
                        this.linkConyuge.Dock = DockStyle.None;
                        this.linkConyuge0.Dock = DockStyle.None;
                        this.linkConyuge1.Dock = DockStyle.None;
                        this.linkConyuge2.Dock = DockStyle.None;
                        this.linkConyuge3.Dock = DockStyle.None;
                        this.linkConyuge4.Dock = DockStyle.None;
                        this.linkConyuge5.Dock = DockStyle.None;
                        this.linkConyuge6.Dock = DockStyle.None;
                        this.linkConyuge7.Dock = DockStyle.None;
                        this.linkConyuge8.Dock = DockStyle.None;
                        this.linkConyuge9.Dock = DockStyle.None;
                        this.linkConyuge10.Dock = DockStyle.None;

                        #region Finca Raiz

                        this.txtInmuebleCony.EditValue = conyuge.NroInmuebles.Value;
                        this.txtHipotecasCony.EditValue = conyuge.HipotecasNro.Value;
                        this.txtRestriccionCony.EditValue = conyuge.RestriccionesNro.Value;
                        this.txtPorcTablaCony.EditValue = conyuge.PF_FincaRaizDato.Value;
                        this.txtFactorCony.EditValue = conyuge.PF_FincaRaiz.Value;
                        #endregion
                        #region Probabilidad
                        this.txtCony101.EditValue = conyuge.PF_MorasUltAno.Value;
                        this.txtCony102.EditValue = ScoreConyuge.Puntaje.Value;
                        this.txtCony103.EditValue = conyuge.PF_FactorAcierta.Value;
                        this.txtCony104.EditValue = conyuge.PF_AciertaResultado.Value;
                        #endregion
                        #region Mora Actual
                        this.txtCony201.EditValue = conyuge.PF_MorasActuales.Value;
                        this.txtCony202.EditValue = datCredConyuge.EstadoAct30.Value;
                        this.txtCony203.EditValue = conyuge.PF_MorasAct30Dato.Value;
                        this.txtCony204.EditValue = conyuge.PF_MorasAct30.Value;
                        this.txtCony205.EditValue = datCredConyuge.EstadoAct60.Value;
                        this.txtCony206.EditValue = conyuge.PF_MorasAct60Dato.Value;
                        this.txtCony207.EditValue = conyuge.PF_MorasAct60.Value;
                        this.txtCony208.EditValue = datCredConyuge.EstadoAct90.Value;
                        this.txtCony209.EditValue = conyuge.PF_MorasAct90Dato.Value;
                        this.txtCony210.EditValue = conyuge.PF_MorasAct90.Value;
                        this.txtCony211.EditValue = datCredConyuge.EstadoAct120.Value;
                        this.txtCony212.EditValue = conyuge.PF_MorasAct120Dato.Value;
                        this.txtCony213.EditValue = conyuge.PF_MorasAct120.Value;
                        #endregion
                        #region Mora Ultimo Año
                        this.txtCony301.EditValue = conyuge.PF_MorasUltAno.Value;
                        this.txtCony302.EditValue = datCredConyuge.EstadoHis30.Value;
                        this.txtCony303.EditValue = conyuge.PF_MorasUlt30Dato.Value;
                        this.txtCony304.EditValue = conyuge.PF_MorasUlt30.Value;
                        this.txtCony305.EditValue = datCredConyuge.EstadoHis60.Value;
                        this.txtCony306.EditValue = conyuge.PF_MorasUlt60Dato.Value;
                        this.txtCony307.EditValue = conyuge.PF_MorasUlt60.Value;
                        this.txtCony308.EditValue = datCredConyuge.EstadoHis90.Value;
                        this.txtCony309.EditValue = conyuge.PF_MorasUlt90Dato.Value;
                        this.txtCony310.EditValue = conyuge.PF_MorasUlt90.Value;
                        this.txtCony311.EditValue = datCredConyuge.EstadoHis120.Value;
                        this.txtCony312.EditValue = conyuge.PF_MorasUlt120Dato.Value;
                        this.txtCony313.EditValue = conyuge.PF_MorasUlt120.Value;
                        #endregion
                        #region Reportes Negativos
                        this.txtCony401.EditValue = conyuge.PF_RepNegativos.Value;
                        this.txtCony402.EditValue = datCredConyuge.EstadoActCob.Value;
                        this.txtCony403.EditValue = conyuge.PF_ObligacionCOBDato.Value;
                        this.txtCony404.EditValue = conyuge.PF_ObligacionCOB.Value;
                        this.txtCony405.EditValue = datCredConyuge.EstadoActDud.Value;
                        this.txtCony406.EditValue = conyuge.PF_ObligacionDUDDato.Value;
                        this.txtCony407.EditValue = conyuge.PF_ObligacionDUD.Value;
                        this.txtCony408.EditValue = datCredConyuge.EstadoActCas.Value;
                        this.txtCony409.EditValue = conyuge.PF_ObligacionCASDato.Value;
                        this.txtCony410.EditValue = conyuge.PF_ObligacionCAS.Value;
                        this.txtCony411.EditValue = datCredConyuge.CtasEmbargadas.Value;
                        this.txtCony412.EditValue = conyuge.PF_ObligacionEMBDato.Value;
                        this.txtCony413.EditValue = conyuge.PF_ObligacionEMB.Value;
                        this.txtCony414.EditValue = datCredConyuge.EstadoHisRec.Value;
                        this.txtCony415.EditValue = conyuge.PF_ObligacionRECDato.Value;
                        this.txtCony416.EditValue = conyuge.PF_ObligacionREC.Value;
                        this.txtCony417.EditValue = datCredConyuge.CtasMalManejo.Value;
                        this.txtCony418.EditValue = conyuge.PF_ObligacionCANDato.Value;
                        this.txtCony419.EditValue = conyuge.PF_ObligacionCAN.Value;
                        #endregion
                        #region Estado Actual
                        this.txtCony501.EditValue = conyuge.PF_EstadoActual.Value;
                        this.txtCony502.EditValue = datCredConyuge.NumeObligACT.Value;
                        this.txtCony503.EditValue = conyuge.PF_PorObligacionesDato.Value;
                        this.txtCony504.EditValue = conyuge.PF_PorObligaciones.Value;
                        this.txtCony505.EditValue = datCredConyuge.NumeroTDC.Value;
                        this.txtCony506.EditValue = conyuge.PF_PorUtilizaTDCDato.Value;
                        this.txtCony507.EditValue = conyuge.PF_PorUtilizaTDC.Value;
                        this.txtCony508.EditValue = datCredConyuge.PeorEndeudT2.Value;
                        this.txtCony509.EditValue = conyuge.PF_PeorCalificacionDato.Value;
                        this.txtCony510.EditValue = conyuge.PF_PeorCalificacion.Value;
                        this.txtCony511.EditValue = datCredConyuge.UltConsultas.Value;
                        this.txtCony512.EditValue = conyuge.PF_Consultas6MesesDato.Value;
                        this.txtCony513.EditValue = conyuge.PF_Consultas6Meses.Value;
                        #endregion
                        #region Estabilidad
                        this.txtCony601.EditValue = conyuge.PF_Estabilidad.Value;
                        this.txtCony602.EditValue = conyuge.PF_DireccDesdeMeses.Value;
                        this.txtCony603.EditValue = conyuge.PF_DireccDesdeDato.Value;
                        this.txtCony604.EditValue = conyuge.PF_DireccDesde.Value;
                        this.txtCony605.EditValue = 0;
                        this.txtCony606.EditValue = conyuge.PF_EntidadesNumDato.Value;
                        this.txtCony607.EditValue = conyuge.PF_EntidadesNum.Value;
                        this.txtCony608.EditValue = conyuge.PF_CelularDesdeMeses.Value;
                        this.txtCony609.EditValue = conyuge.PF_CelularDesdeDato.Value;
                        this.txtCony610.EditValue = conyuge.PF_CelularDesde.Value;
                        this.txtCony611.EditValue = conyuge.PF_CorreoDesdeMeses.Value;
                        this.txtCony612.EditValue = conyuge.PF_CorreoDesdeDato.Value;
                        this.txtCony613.EditValue = conyuge.PF_CorreoDesde.Value;
                        #endregion
                        #region Ubicabilidad
                        this.txtCony701.EditValue = conyuge.PF_Ubicabilidad.Value;
                        this.txtCony702.EditValue = conyuge.PF_DireccionNumCant.Value;
                        this.txtCony703.EditValue = conyuge.PF_DireccionNumDato.Value;
                        this.txtCony704.EditValue = conyuge.PF_DireccionNum.Value;
                        this.txtCony705.EditValue = conyuge.PF_TelefonoNumCant.Value;
                        this.txtCony706.EditValue = conyuge.PF_TelefonoNumDato.Value;
                        this.txtCony707.EditValue = conyuge.PF_TelefonoNum.Value;
                        this.txtCony708.EditValue = conyuge.PF_CelularNumCant.Value;
                        this.txtCony709.EditValue = conyuge.PF_CelularNumDato.Value;
                        this.txtCony710.EditValue = conyuge.PF_CelularNum.Value;
                        this.txtCony711.EditValue = conyuge.PF_CorreoNumCant.Value;
                        this.txtCony712.EditValue = conyuge.PF_CorreoNumDato.Value;
                        this.txtCony713.EditValue = conyuge.PF_CorreoNum.Value;
                        #endregion

                        #region % Max financiacion
                        this.txtCony801.EditValue = conyuge.PF_PorMaxFincaRaiz.Value;
                        this.txtCony802.EditValue = conyuge.PF_PorMaxMorasActuales.Value;
                        this.txtCony803.EditValue = conyuge.PF_PorMaxMorasUltAno.Value;
                        this.txtCony804.EditValue = conyuge.PF_PorMaxRepNegativos.Value;
                        this.txtCony805.EditValue = conyuge.PF_PorMaxEstadoActual.Value;
                        this.txtCony806.EditValue = conyuge.PF_PorMaxProbabilidad.Value;
                        this.txtCony807.EditValue = conyuge.PF_PorMaxEstabilidad.Value;
                        this.txtCony808.EditValue = conyuge.PF_PorMaxUbicabilidad.Value;
                        this.txtCony809.EditValue = Convert.ToDecimal(conyuge.PF_PorMaxFincaRaiz.Value) + Convert.ToDecimal(conyuge.PF_PorMaxMorasActuales.Value)
                            + Convert.ToDecimal(conyuge.PF_PorMaxMorasUltAno.Value) + Convert.ToDecimal(conyuge.PF_PorMaxRepNegativos.Value)
                            + Convert.ToDecimal(conyuge.PF_PorMaxEstadoActual.Value) + Convert.ToDecimal(conyuge.PF_PorMaxProbabilidad.Value)
                            + Convert.ToDecimal(conyuge.PF_PorMaxEstabilidad.Value) + Convert.ToDecimal(conyuge.PF_PorMaxUbicabilidad.Value);

                        #endregion


                        #region Endeudamiento
                        this.txtCony901.EditValue = datCredConyuge.VlrSaldoVIV.Value;
                        this.txtCony902.EditValue = datCredConyuge.VlrSaldoBAN.Value;
                        this.txtCony903.EditValue = datCredConyuge.VlrSaldoFIN.Value;
                        this.txtCony904.EditValue = datCredConyuge.VlrSaldoCOP.Value;
                        this.txtCony905.EditValue = datCredConyuge.VlrUtilizadoTDC.Value;
                        this.txtCony906.EditValue = datCredConyuge.VlrSaldoREA.Value;
                        this.txtCony907.EditValue = datCredConyuge.VlrCuotasCEL.Value;
                        this.txtCony908.EditValue = Convert.ToDecimal(datCredConyuge.VlrSaldoVIV.Value) + Convert.ToDecimal(datCredConyuge.VlrSaldoBAN.Value)
                             + Convert.ToDecimal(datCredConyuge.VlrSaldoFIN.Value) + Convert.ToDecimal(datCredConyuge.VlrSaldoCOP.Value)
                             + Convert.ToDecimal(datCredConyuge.VlrUtilizadoTDC.Value) + Convert.ToDecimal(datCredConyuge.VlrCuotasCEL.Value) + Convert.ToDecimal(datCredConyuge.VlrSaldoREA.Value);
                        #endregion


                        #region Cuotas
                        this.txtCony1001.EditValue = datCredConyuge.VlrCuotasVIV.Value;
                        this.txtCony1002.EditValue = conyuge.PF_EstCtasVIV.Value;
                        this.txtCony1003.EditValue = conyuge.PF_CtasTotVIV.Value;
                        this.txtCony1004.EditValue = datCredConyuge.VlrCuotasBAN.Value;
                        this.txtCony1005.EditValue = conyuge.PF_EstCtasBAN.Value;
                        this.txtCony1006.EditValue = conyuge.PF_CtasTotBAN.Value;
                        this.txtCony1007.EditValue = datCredConyuge.VlrCuotasFIN.Value;
                        this.txtCony1008.EditValue = conyuge.PF_EstCtasFIN.Value;
                        this.txtCony1009.EditValue = conyuge.PF_CtasTotFIN.Value;
                        this.txtCony1010.EditValue = datCredConyuge.VlrCuotasCOP.Value;
                        this.txtCony1011.EditValue = conyuge.PF_EstCtasCOP.Value;
                        this.txtCony1012.EditValue = conyuge.PF_CtasTotCOP.Value;
                        this.txtCony1013.EditValue = datCredConyuge.VlrCuotasTDC.Value;
                        this.txtCony1014.EditValue = conyuge.PF_EstCtasTDC.Value;
                        this.txtCony1015.EditValue = conyuge.PF_CtasTotTDC.Value;
                        this.txtCony1016.EditValue = datCredConyuge.VlrCuotasREA.Value;
                        this.txtCony1017.EditValue = conyuge.PF_EstCtasREA.Value;
                        this.txtCony1018.EditValue = conyuge.PF_CtasTotREA.Value;
                        this.txtCony1019.EditValue = datCredConyuge.VlrCuotasCEL.Value;
                        this.txtCony1020.EditValue = conyuge.PF_EstCtasCEL.Value;
                        this.txtCony1021.EditValue = conyuge.PF_CtasTotCEL.Value;
                        this.txtCony1022.EditValue = conyuge.PF_CtasTotIngEst.Value;
                        #endregion

                        #region Endeudamiento
                        this.txtCony1101.EditValue = conyuge.PF_QuiantiMIN.Value;
                        this.txtCony1102.EditValue = conyuge.PF_QuiantiMAX.Value;
                        this.txtCony1103.EditValue = conyuge.PF_QuantoIngrEst.Value;
                        this.txtCony1104.EditValue = conyuge.PF_IngrEstxQuanto.Value;
                        this.txtCony1105.EditValue = conyuge.IngresosREG.Value.Value;
                        this.txtCony1106.EditValue = conyuge.PF_FactIngresosREG.Value;
                        this.txtCony1107.EditValue = conyuge.IngresosSOP.Value;
                        this.txtCony1108.EditValue = conyuge.PF_IngrCapacPAG.Value;
                        this.txtCony1109.EditValue = conyuge.PF_PorIngrAporta.Value;
                        this.chkCony1110.EditValue = conyuge.PF_ReqSopIngrInd.Value;
                        this.txtCony1111.EditValue = conyuge.PF_PorIngrPagoCtas.Value;
                        this.txtCony1112.EditValue = conyuge.PF_IngrDispPagoCtas.Value;
                        this.txtCony1113.EditValue = conyuge.PF_CuotasACT.Value;
                        this.txtCony1114.EditValue = conyuge.PF_IngrDispApoyos.Value;
                        #endregion

                    }
                    else
                    {
                    }
                    #endregion

                    //Codeudor1 (TipoPersona 3)
                    #region Codeudor1
                    if (codeudor1 != null)
                    {
                        this.linkCodeudor1.Dock = DockStyle.None;
                        this.linkCodeudor10.Dock = DockStyle.None;
                        this.linkCodeudor11.Dock = DockStyle.None;
                        this.linkCodeudor12.Dock = DockStyle.None;
                        this.linkCodeudor13.Dock = DockStyle.None;
                        this.linkCodeudor14.Dock = DockStyle.None;
                        this.linkCodeudor15.Dock = DockStyle.None;
                        this.linkCodeudor16.Dock = DockStyle.None;
                        this.linkCodeudor17.Dock = DockStyle.None;
                        this.linkCodeudor18.Dock = DockStyle.None;
                        this.linkCodeudor19.Dock = DockStyle.None;
                        this.linkCodeudor110.Dock = DockStyle.None;

                        #region Finca Raiz

                        this.txtInmuebleCod1.EditValue = codeudor1.NroInmuebles.Value;
                        this.txtHipotecasCod1.EditValue = codeudor1.HipotecasNro.Value;
                        this.txtRestriccionCod1.EditValue = codeudor1.RestriccionesNro.Value;
                        this.txtPorcTablaCod1.EditValue = codeudor1.PF_FincaRaizDato.Value;
                        this.txtFactorCod1.EditValue = codeudor1.PF_FincaRaiz.Value;
                        #endregion
                        #region Probabilidad
                        this.txtCod1101.EditValue = codeudor1.PF_MorasUltAno.Value;
                        this.txtCod1102.EditValue = ScoreCodeudor1.Puntaje.Value;
                        this.txtCod1103.EditValue = codeudor1.PF_FactorAcierta.Value;
                        this.txtCod1104.EditValue = codeudor1.PF_AciertaResultado.Value;
                        #endregion
                        #region Mora Actual
                        this.txtCod1201.EditValue = codeudor1.PF_MorasActuales.Value;
                        this.txtCod1202.EditValue = datCredCodeudor1.EstadoAct30.Value;
                        this.txtCod1203.EditValue = codeudor1.PF_MorasAct30Dato.Value;
                        this.txtCod1204.EditValue = codeudor1.PF_MorasAct30.Value;
                        this.txtCod1205.EditValue = datCredCodeudor1.EstadoAct60.Value;
                        this.txtCod1206.EditValue = codeudor1.PF_MorasAct60Dato.Value;
                        this.txtCod1207.EditValue = codeudor1.PF_MorasAct60.Value;
                        this.txtCod1208.EditValue = datCredCodeudor1.EstadoAct90.Value;
                        this.txtCod1209.EditValue = codeudor1.PF_MorasAct90Dato.Value;
                        this.txtCod1210.EditValue = codeudor1.PF_MorasAct90.Value;
                        this.txtCod1211.EditValue = datCredCodeudor1.EstadoAct120.Value;
                        this.txtCod1212.EditValue = codeudor1.PF_MorasAct120Dato.Value;
                        this.txtCod1213.EditValue = codeudor1.PF_MorasAct120.Value;
                        #endregion
                        #region Mora Ultimo Año
                        this.txtCod1301.EditValue = codeudor1.PF_MorasUltAno.Value;
                        this.txtCod1302.EditValue = datCredCodeudor1.EstadoHis30.Value;
                        this.txtCod1303.EditValue = codeudor1.PF_MorasUlt30Dato.Value;
                        this.txtCod1304.EditValue = codeudor1.PF_MorasUlt30.Value;
                        this.txtCod1305.EditValue = datCredCodeudor1.EstadoHis60.Value;
                        this.txtCod1306.EditValue = codeudor1.PF_MorasUlt60Dato.Value;
                        this.txtCod1307.EditValue = codeudor1.PF_MorasUlt60.Value;
                        this.txtCod1308.EditValue = datCredCodeudor1.EstadoHis90.Value;
                        this.txtCod1309.EditValue = codeudor1.PF_MorasUlt90Dato.Value;
                        this.txtCod1310.EditValue = codeudor1.PF_MorasUlt90.Value;
                        this.txtCod1311.EditValue = datCredCodeudor1.EstadoHis120.Value;
                        this.txtCod1312.EditValue = codeudor1.PF_MorasUlt120Dato.Value;
                        this.txtCod1313.EditValue = codeudor1.PF_MorasUlt120.Value;
                        #endregion
                        #region Reportes Negativos
                        this.txtCod1401.EditValue = codeudor1.PF_RepNegativos.Value;
                        this.txtCod1402.EditValue = datCredCodeudor1.EstadoActCob.Value;
                        this.txtCod1403.EditValue = codeudor1.PF_ObligacionCOBDato.Value;
                        this.txtCod1404.EditValue = codeudor1.PF_ObligacionCOB.Value;
                        this.txtCod1405.EditValue = datCredCodeudor1.EstadoActDud.Value;
                        this.txtCod1406.EditValue = codeudor1.PF_ObligacionDUDDato.Value;
                        this.txtCod1407.EditValue = codeudor1.PF_ObligacionDUD.Value;
                        this.txtCod1408.EditValue = datCredCodeudor1.EstadoActCas.Value;
                        this.txtCod1409.EditValue = codeudor1.PF_ObligacionCASDato.Value;
                        this.txtCod1410.EditValue = codeudor1.PF_ObligacionCAS.Value;
                        this.txtCod1411.EditValue = datCredCodeudor1.CtasEmbargadas.Value;
                        this.txtCod1412.EditValue = codeudor1.PF_ObligacionEMBDato.Value;
                        this.txtCod1413.EditValue = codeudor1.PF_ObligacionEMB.Value;
                        this.txtCod1414.EditValue = datCredCodeudor1.EstadoHisRec.Value;
                        this.txtCod1415.EditValue = codeudor1.PF_ObligacionRECDato.Value;
                        this.txtCod1416.EditValue = codeudor1.PF_ObligacionREC.Value;
                        this.txtCod1417.EditValue = datCredCodeudor1.CtasMalManejo.Value;
                        this.txtCod1418.EditValue = codeudor1.PF_ObligacionCANDato.Value;
                        this.txtCod1419.EditValue = codeudor1.PF_ObligacionCAN.Value;
                        #endregion
                        #region Estado Actual
                        this.txtCod1501.EditValue = codeudor1.PF_EstadoActual.Value;
                        this.txtCod1502.EditValue = datCredCodeudor1.NumeObligACT.Value;
                        this.txtCod1503.EditValue = codeudor1.PF_PorObligacionesDato.Value;
                        this.txtCod1504.EditValue = codeudor1.PF_PorObligaciones.Value;
                        this.txtCod1505.EditValue = datCredCodeudor1.NumeroTDC.Value;
                        this.txtCod1506.EditValue = codeudor1.PF_PorUtilizaTDCDato.Value;
                        this.txtCod1507.EditValue = codeudor1.PF_PorUtilizaTDC.Value;
                        this.txtCod1508.EditValue = datCredCodeudor1.PeorEndeudT2.Value;
                        this.txtCod1509.EditValue = codeudor1.PF_PeorCalificacionDato.Value;
                        this.txtCod1510.EditValue = codeudor1.PF_PeorCalificacion.Value;
                        this.txtCod1511.EditValue = datCredCodeudor1.UltConsultas.Value;
                        this.txtCod1512.EditValue = codeudor1.PF_Consultas6MesesDato.Value;
                        this.txtCod1513.EditValue = codeudor1.PF_Consultas6Meses.Value;
                        #endregion
                        #region Estabilidad
                        this.txtCod1601.EditValue = codeudor1.PF_Estabilidad.Value;
                        this.txtCod1602.EditValue = codeudor1.PF_DireccDesdeMeses.Value;
                        this.txtCod1603.EditValue = codeudor1.PF_DireccDesdeDato.Value;
                        this.txtCod1604.EditValue = codeudor1.PF_DireccDesde.Value;
                        this.txtCod1605.EditValue = 0;
                        this.txtCod1606.EditValue = codeudor1.PF_EntidadesNumDato.Value;
                        this.txtCod1607.EditValue = codeudor1.PF_EntidadesNum.Value;
                        this.txtCod1608.EditValue = codeudor1.PF_CelularDesdeMeses.Value;
                        this.txtCod1609.EditValue = codeudor1.PF_CelularDesdeDato.Value;
                        this.txtCod1610.EditValue = codeudor1.PF_CelularDesde.Value;
                        this.txtCod1611.EditValue = codeudor1.PF_CorreoDesdeMeses.Value;
                        this.txtCod1612.EditValue = codeudor1.PF_CorreoDesdeDato.Value;
                        this.txtCod1613.EditValue = codeudor1.PF_CorreoDesde.Value;
                        #endregion
                        #region Ubicabilidad
                        this.txtCod1701.EditValue = codeudor1.PF_Ubicabilidad.Value;
                        this.txtCod1702.EditValue = codeudor1.PF_DireccionNumCant.Value;
                        this.txtCod1703.EditValue = codeudor1.PF_DireccionNumDato.Value;
                        this.txtCod1704.EditValue = codeudor1.PF_DireccionNum.Value;
                        this.txtCod1705.EditValue = codeudor1.PF_TelefonoNumCant.Value;
                        this.txtCod1706.EditValue = codeudor1.PF_TelefonoNumDato.Value;
                        this.txtCod1707.EditValue = codeudor1.PF_TelefonoNum.Value;
                        this.txtCod1708.EditValue = codeudor1.PF_CelularNumCant.Value;
                        this.txtCod1709.EditValue = codeudor1.PF_CelularNumDato.Value;
                        this.txtCod1710.EditValue = codeudor1.PF_CelularNum.Value;
                        this.txtCod1711.EditValue = codeudor1.PF_CorreoNumCant.Value;
                        this.txtCod1712.EditValue = codeudor1.PF_CorreoNumDato.Value;
                        this.txtCod1713.EditValue = codeudor1.PF_CorreoNum.Value;
                        #endregion

                        #region % Max financiacion
                        this.txtCod1801.EditValue = codeudor1.PF_PorMaxFincaRaiz.Value;
                        this.txtCod1802.EditValue = codeudor1.PF_PorMaxMorasActuales.Value;
                        this.txtCod1803.EditValue = codeudor1.PF_PorMaxMorasUltAno.Value;
                        this.txtCod1804.EditValue = codeudor1.PF_PorMaxRepNegativos.Value;
                        this.txtCod1805.EditValue = codeudor1.PF_PorMaxEstadoActual.Value;
                        this.txtCod1806.EditValue = codeudor1.PF_PorMaxProbabilidad.Value;
                        this.txtCod1807.EditValue = codeudor1.PF_PorMaxEstabilidad.Value;
                        this.txtCod1808.EditValue = codeudor1.PF_PorMaxUbicabilidad.Value;
                        this.txtCod1809.EditValue = Convert.ToDecimal(codeudor1.PF_PorMaxFincaRaiz.Value) + Convert.ToDecimal(codeudor1.PF_PorMaxMorasActuales.Value)
                            + Convert.ToDecimal(codeudor1.PF_PorMaxMorasUltAno.Value) + Convert.ToDecimal(codeudor1.PF_PorMaxRepNegativos.Value)
                            + Convert.ToDecimal(codeudor1.PF_PorMaxEstadoActual.Value) + Convert.ToDecimal(codeudor1.PF_PorMaxProbabilidad.Value)
                            + Convert.ToDecimal(codeudor1.PF_PorMaxEstabilidad.Value) + Convert.ToDecimal(codeudor1.PF_PorMaxUbicabilidad.Value);

                        #endregion


                        #region Endeudamiento
                        this.txtCod1901.EditValue = datCredCodeudor1.VlrSaldoVIV.Value;
                        this.txtCod1902.EditValue = datCredCodeudor1.VlrSaldoBAN.Value;
                        this.txtCod1903.EditValue = datCredCodeudor1.VlrSaldoFIN.Value;
                        this.txtCod1904.EditValue = datCredCodeudor1.VlrSaldoCOP.Value;
                        this.txtCod1905.EditValue = datCredCodeudor1.VlrUtilizadoTDC.Value;
                        this.txtCod1906.EditValue = datCredCodeudor1.VlrSaldoREA.Value;
                        this.txtCod1907.EditValue = datCredCodeudor1.VlrCuotasCEL.Value;
                        this.txtCod1908.EditValue = Convert.ToDecimal(datCredCodeudor1.VlrSaldoVIV.Value) + Convert.ToDecimal(datCredCodeudor1.VlrSaldoBAN.Value)
                             + Convert.ToDecimal(datCredCodeudor1.VlrSaldoFIN.Value) + Convert.ToDecimal(datCredCodeudor1.VlrSaldoCOP.Value)
                             + Convert.ToDecimal(datCredCodeudor1.VlrUtilizadoTDC.Value) + Convert.ToDecimal(datCredCodeudor1.VlrCuotasCEL.Value) + Convert.ToDecimal(datCredCodeudor1.VlrSaldoREA.Value);
                        #endregion


                        #region Cuotas
                        this.txtCod11001.EditValue = datCredCodeudor1.VlrCuotasVIV.Value;
                        this.txtCod11002.EditValue = codeudor1.PF_EstCtasVIV.Value;
                        this.txtCod11003.EditValue = codeudor1.PF_CtasTotVIV.Value;
                        this.txtCod11004.EditValue = datCredCodeudor1.VlrCuotasBAN.Value;
                        this.txtCod11005.EditValue = codeudor1.PF_EstCtasBAN.Value;
                        this.txtCod11006.EditValue = codeudor1.PF_CtasTotBAN.Value;
                        this.txtCod11007.EditValue = datCredCodeudor1.VlrCuotasFIN.Value;
                        this.txtCod11008.EditValue = codeudor1.PF_EstCtasFIN.Value;
                        this.txtCod11009.EditValue = codeudor1.PF_CtasTotFIN.Value;
                        this.txtCod11010.EditValue = datCredCodeudor1.VlrCuotasCOP.Value;
                        this.txtCod11011.EditValue = codeudor1.PF_EstCtasCOP.Value;
                        this.txtCod11012.EditValue = codeudor1.PF_CtasTotCOP.Value;
                        this.txtCod11013.EditValue = datCredCodeudor1.VlrCuotasTDC.Value;
                        this.txtCod11014.EditValue = codeudor1.PF_EstCtasTDC.Value;
                        this.txtCod11015.EditValue = codeudor1.PF_CtasTotTDC.Value;
                        this.txtCod11016.EditValue = datCredCodeudor1.VlrCuotasREA.Value;
                        this.txtCod11017.EditValue = codeudor1.PF_EstCtasREA.Value;
                        this.txtCod11018.EditValue = codeudor1.PF_CtasTotREA.Value;
                        this.txtCod11019.EditValue = datCredCodeudor1.VlrCuotasCEL.Value;
                        this.txtCod11020.EditValue = codeudor1.PF_EstCtasCEL.Value;
                        this.txtCod11021.EditValue = codeudor1.PF_CtasTotCEL.Value;
                        this.txtCod11022.EditValue = codeudor1.PF_CtasTotIngEst.Value;
                        #endregion

                        #region Endeudamiento
                        this.txtCod11101.EditValue = codeudor1.PF_QuiantiMIN.Value;
                        this.txtCod11102.EditValue = codeudor1.PF_QuiantiMAX.Value;
                        this.txtCod11103.EditValue = codeudor1.PF_QuantoIngrEst.Value;
                        this.txtCod11104.EditValue = codeudor1.PF_IngrEstxQuanto.Value;
                        this.txtCod11105.EditValue = codeudor1.IngresosREG.Value;
                        this.txtCod11106.EditValue = codeudor1.PF_FactIngresosREG.Value;
                        this.txtCod11107.EditValue = codeudor1.IngresosSOP.Value;
                        this.txtCod11108.EditValue = codeudor1.PF_IngrCapacPAG.Value;
                        this.txtCod11109.EditValue = codeudor1.PF_PorIngrAporta.Value;
                        this.chkCod11110.EditValue = codeudor1.PF_ReqSopIngrInd.Value;
                        this.txtCod11111.EditValue = codeudor1.PF_PorIngrPagoCtas.Value;
                        this.txtCod11112.EditValue = codeudor1.PF_IngrDispPagoCtas.Value;
                        this.txtCod11113.EditValue = codeudor1.PF_CuotasACT.Value;
                        this.txtCod11114.EditValue = codeudor1.PF_IngrDispApoyos.Value;
                        #endregion

                    }
                    else
                    {
                    }
                    #endregion
                    //Codeudor2 (TipoPersona 4)
                    #region Codeudor2
                    if (codeudor2 != null)
                    {
                        this.linkCodeudor2.Dock = DockStyle.None;
                        this.linkCodeudor20.Dock = DockStyle.None;
                        this.linkCodeudor21.Dock = DockStyle.None;
                        this.linkCodeudor22.Dock = DockStyle.None;
                        this.linkCodeudor23.Dock = DockStyle.None;
                        this.linkCodeudor24.Dock = DockStyle.None;
                        this.linkCodeudor25.Dock = DockStyle.None;
                        this.linkCodeudor26.Dock = DockStyle.None;
                        this.linkCodeudor27.Dock = DockStyle.None;
                        this.linkCodeudor28.Dock = DockStyle.None;
                        this.linkCodeudor29.Dock = DockStyle.None;
                        this.linkCodeudor210.Dock = DockStyle.None;
                        
                        #region Finca Raiz

                        this.txtInmuebleCod2.EditValue = codeudor2.NroInmuebles.Value.Value;
                        this.txtHipotecasCod2.EditValue = codeudor2.HipotecasNro.Value.Value;
                        this.txtRestriccionCod2.EditValue = codeudor2.RestriccionesNro.Value.Value;
                        this.txtPorcTablaCod2.EditValue = codeudor2.PF_FincaRaizDato.Value.Value;
                        this.txtFactorCod2.EditValue = codeudor2.PF_FincaRaiz.Value.Value;
                        #endregion
                        #region Probabilidad
                        this.txtCod2101.EditValue = codeudor2.PF_MorasUltAno.Value;
                        this.txtCod2102.EditValue = ScoreCodeudor2.Puntaje.Value;
                        this.txtCod2103.EditValue = codeudor2.PF_FactorAcierta.Value;
                        this.txtCod2104.EditValue = codeudor2.PF_AciertaResultado.Value;
                        #endregion
                        #region Mora Actual
                        this.txtCod2201.EditValue = codeudor2.PF_MorasActuales.Value;
                        this.txtCod2202.EditValue = datCredCodeudor2.EstadoAct30.Value;
                        this.txtCod2203.EditValue = codeudor2.PF_MorasAct30Dato.Value;
                        this.txtCod2204.EditValue = codeudor2.PF_MorasAct30.Value;
                        this.txtCod2205.EditValue = datCredCodeudor2.EstadoAct60.Value;
                        this.txtCod2206.EditValue = codeudor2.PF_MorasAct60Dato.Value;
                        this.txtCod2207.EditValue = codeudor2.PF_MorasAct60.Value;
                        this.txtCod2208.EditValue = datCredCodeudor2.EstadoAct90.Value;
                        this.txtCod2209.EditValue = codeudor2.PF_MorasAct90Dato.Value;
                        this.txtCod2210.EditValue = codeudor2.PF_MorasAct90.Value;
                        this.txtCod2212.EditValue = datCredCodeudor2.EstadoAct120.Value;
                        this.txtCod2212.EditValue = codeudor2.PF_MorasAct120Dato.Value;
                        this.txtCod2213.EditValue = codeudor2.PF_MorasAct120.Value;
                        #endregion
                        #region Mora Ultimo Año
                        this.txtCod2301.EditValue = codeudor2.PF_MorasUltAno.Value;
                        this.txtCod2302.EditValue = datCredCodeudor2.EstadoHis30.Value;
                        this.txtCod2303.EditValue = codeudor2.PF_MorasUlt30Dato.Value;
                        this.txtCod2304.EditValue = codeudor2.PF_MorasUlt30.Value;
                        this.txtCod2305.EditValue = datCredCodeudor2.EstadoHis60.Value;
                        this.txtCod2306.EditValue = codeudor2.PF_MorasUlt60Dato.Value;
                        this.txtCod2307.EditValue = codeudor2.PF_MorasUlt60.Value;
                        this.txtCod2308.EditValue = datCredCodeudor2.EstadoHis90.Value;
                        this.txtCod2309.EditValue = codeudor2.PF_MorasUlt90Dato.Value;
                        this.txtCod2310.EditValue = codeudor2.PF_MorasUlt90.Value;
                        this.txtCod2312.EditValue = datCredCodeudor2.EstadoHis120.Value;
                        this.txtCod2312.EditValue = codeudor2.PF_MorasUlt120Dato.Value;
                        this.txtCod2313.EditValue = codeudor2.PF_MorasUlt120.Value;
                        #endregion
                        #region Reportes Negativos
                        this.txtCod2401.EditValue = codeudor2.PF_RepNegativos.Value;
                        this.txtCod2402.EditValue = datCredCodeudor2.EstadoActCob.Value;
                        this.txtCod2403.EditValue = codeudor2.PF_ObligacionCOBDato.Value;
                        this.txtCod2404.EditValue = codeudor2.PF_ObligacionCOB.Value;
                        this.txtCod2405.EditValue = datCredCodeudor2.EstadoActDud.Value;
                        this.txtCod2406.EditValue = codeudor2.PF_ObligacionDUDDato.Value;
                        this.txtCod2407.EditValue = codeudor2.PF_ObligacionDUD.Value;
                        this.txtCod2408.EditValue = datCredCodeudor2.EstadoActCas.Value;
                        this.txtCod2409.EditValue = codeudor2.PF_ObligacionCASDato.Value;
                        this.txtCod2410.EditValue = codeudor2.PF_ObligacionCAS.Value;
                        this.txtCod2411.EditValue = datCredCodeudor2.CtasEmbargadas.Value;
                        this.txtCod2412.EditValue = codeudor2.PF_ObligacionEMBDato.Value;
                        this.txtCod2413.EditValue = codeudor2.PF_ObligacionEMB.Value;
                        this.txtCod2414.EditValue = datCredCodeudor2.EstadoHisRec.Value;
                        this.txtCod2415.EditValue = codeudor2.PF_ObligacionRECDato.Value;
                        this.txtCod2416.EditValue = codeudor2.PF_ObligacionREC.Value;
                        this.txtCod2417.EditValue = datCredCodeudor2.CtasMalManejo.Value;
                        this.txtCod2418.EditValue = codeudor2.PF_ObligacionCANDato.Value;
                        this.txtCod2419.EditValue = codeudor2.PF_ObligacionCAN.Value;
                        #endregion
                        #region Estado Actual
                        this.txtCod2501.EditValue = codeudor2.PF_EstadoActual.Value;
                        this.txtCod2502.EditValue = datCredCodeudor2.NumeObligACT.Value;
                        this.txtCod2503.EditValue = codeudor2.PF_PorObligacionesDato.Value;
                        this.txtCod2504.EditValue = codeudor2.PF_PorObligaciones.Value;
                        this.txtCod2505.EditValue = datCredCodeudor2.NumeroTDC.Value;
                        this.txtCod2506.EditValue = codeudor2.PF_PorUtilizaTDCDato.Value;
                        this.txtCod2507.EditValue = codeudor2.PF_PorUtilizaTDC.Value;
                        this.txtCod2508.EditValue = datCredCodeudor2.PeorEndeudT2.Value;
                        this.txtCod2509.EditValue = codeudor2.PF_PeorCalificacionDato.Value;
                        this.txtCod2510.EditValue = codeudor2.PF_PeorCalificacion.Value;
                        this.txtCod2511.EditValue = datCredCodeudor2.UltConsultas.Value;
                        this.txtCod2512.EditValue = codeudor2.PF_Consultas6MesesDato.Value;
                        this.txtCod2513.EditValue = codeudor2.PF_Consultas6Meses.Value;
                        #endregion
                        #region Estabilidad
                        this.txtCod2602.EditValue = codeudor2.PF_Estabilidad.Value;
                        this.txtCod2602.EditValue = codeudor2.PF_DireccDesdeMeses.Value;
                        this.txtCod2603.EditValue = codeudor2.PF_DireccDesdeDato.Value;
                        this.txtCod2604.EditValue = codeudor2.PF_DireccDesde.Value;
                        this.txtCod2605.EditValue = 0;
                        this.txtCod2606.EditValue = codeudor2.PF_EntidadesNumDato.Value;
                        this.txtCod2607.EditValue = codeudor2.PF_EntidadesNum.Value;
                        this.txtCod2608.EditValue = codeudor2.PF_CelularDesdeMeses.Value;
                        this.txtCod2609.EditValue = codeudor2.PF_CelularDesdeDato.Value;
                        this.txtCod2610.EditValue = codeudor2.PF_CelularDesde.Value;
                        this.txtCod2611.EditValue = codeudor2.PF_CorreoDesdeMeses.Value;
                        this.txtCod2612.EditValue = codeudor2.PF_CorreoDesdeDato.Value;
                        this.txtCod2613.EditValue = codeudor2.PF_CorreoDesde.Value;
                        #endregion
                        #region Ubicabilidad
                        this.txtCod2701.EditValue = codeudor2.PF_Ubicabilidad.Value;
                        this.txtCod2702.EditValue = codeudor2.PF_DireccionNumCant.Value;
                        this.txtCod2703.EditValue = codeudor2.PF_DireccionNumDato.Value;
                        this.txtCod2704.EditValue = codeudor2.PF_DireccionNum.Value;
                        this.txtCod2705.EditValue = codeudor2.PF_TelefonoNumCant.Value;
                        this.txtCod2706.EditValue = codeudor2.PF_TelefonoNumDato.Value;
                        this.txtCod2707.EditValue = codeudor2.PF_TelefonoNum.Value;
                        this.txtCod2708.EditValue = codeudor2.PF_CelularNumCant.Value;
                        this.txtCod2709.EditValue = codeudor2.PF_CelularNumDato.Value;
                        this.txtCod2710.EditValue = codeudor2.PF_CelularNum.Value;
                        this.txtCod2711.EditValue = codeudor2.PF_CorreoNumCant.Value;
                        this.txtCod2712.EditValue = codeudor2.PF_CorreoNumDato.Value;
                        this.txtCod2713.EditValue = codeudor2.PF_CorreoNum.Value;
                        #endregion

                        #region % Max financiacion
                        this.txtCod2801.EditValue = codeudor2.PF_PorMaxFincaRaiz.Value;
                        this.txtCod2802.EditValue = codeudor2.PF_PorMaxMorasActuales.Value;
                        this.txtCod2803.EditValue = codeudor2.PF_PorMaxMorasUltAno.Value;
                        this.txtCod2804.EditValue = codeudor2.PF_PorMaxRepNegativos.Value;
                        this.txtCod2805.EditValue = codeudor2.PF_PorMaxEstadoActual.Value;
                        this.txtCod2806.EditValue = codeudor2.PF_PorMaxProbabilidad.Value;
                        this.txtCod2807.EditValue = codeudor2.PF_PorMaxEstabilidad.Value;
                        this.txtCod2808.EditValue = codeudor2.PF_PorMaxUbicabilidad.Value;
                        this.txtCod2809.EditValue = Convert.ToDecimal(codeudor2.PF_PorMaxFincaRaiz.Value) + Convert.ToDecimal(codeudor2.PF_PorMaxMorasActuales.Value)
                            + Convert.ToDecimal(codeudor2.PF_PorMaxMorasUltAno.Value) + Convert.ToDecimal(codeudor2.PF_PorMaxRepNegativos.Value)
                            + Convert.ToDecimal(codeudor2.PF_PorMaxEstadoActual.Value) + Convert.ToDecimal(codeudor2.PF_PorMaxProbabilidad.Value)
                            + Convert.ToDecimal(codeudor2.PF_PorMaxEstabilidad.Value) + Convert.ToDecimal(codeudor2.PF_PorMaxUbicabilidad.Value);

                        #endregion


                        #region Endeudamiento
                        this.txtCod2901.EditValue = datCredCodeudor2.VlrSaldoVIV.Value;
                        this.txtCod2902.EditValue = datCredCodeudor2.VlrSaldoBAN.Value;
                        this.txtCod2903.EditValue = datCredCodeudor2.VlrSaldoFIN.Value;
                        this.txtCod2904.EditValue = datCredCodeudor2.VlrSaldoCOP.Value;
                        this.txtCod2905.EditValue = datCredCodeudor2.VlrUtilizadoTDC.Value;
                        this.txtCod2906.EditValue = datCredCodeudor2.VlrSaldoREA.Value;
                        this.txtCod2907.EditValue = datCredCodeudor2.VlrCuotasCEL.Value;
                        this.txtCod2908.EditValue = Convert.ToDecimal(datCredCodeudor2.VlrSaldoVIV.Value) + Convert.ToDecimal(datCredCodeudor2.VlrSaldoBAN.Value)
                             + Convert.ToDecimal(datCredCodeudor2.VlrSaldoFIN.Value) + Convert.ToDecimal(datCredCodeudor2.VlrSaldoCOP.Value)
                             + Convert.ToDecimal(datCredCodeudor2.VlrUtilizadoTDC.Value) + Convert.ToDecimal(datCredCodeudor2.VlrCuotasCEL.Value) + Convert.ToDecimal(datCredCodeudor2.VlrSaldoREA.Value);
                        #endregion


                        #region Cuotas
                        this.txtCod21001.EditValue = datCredCodeudor2.VlrCuotasVIV.Value;
                        this.txtCod21002.EditValue = codeudor2.PF_EstCtasVIV.Value;
                        this.txtCod21003.EditValue = codeudor2.PF_CtasTotVIV.Value;
                        this.txtCod21004.EditValue = datCredCodeudor2.VlrCuotasBAN.Value;
                        this.txtCod21005.EditValue = codeudor2.PF_EstCtasBAN.Value;
                        this.txtCod21006.EditValue = codeudor2.PF_CtasTotBAN.Value;
                        this.txtCod21007.EditValue = datCredCodeudor2.VlrCuotasFIN.Value;
                        this.txtCod21008.EditValue = codeudor2.PF_EstCtasFIN.Value;
                        this.txtCod21009.EditValue = codeudor2.PF_CtasTotFIN.Value;
                        this.txtCod21010.EditValue = datCredCodeudor2.VlrCuotasCOP.Value;
                        this.txtCod21012.EditValue = codeudor2.PF_EstCtasCOP.Value;
                        this.txtCod21012.EditValue = codeudor2.PF_CtasTotCOP.Value;
                        this.txtCod21013.EditValue = datCredCodeudor2.VlrCuotasTDC.Value;
                        this.txtCod21014.EditValue = codeudor2.PF_EstCtasTDC.Value;
                        this.txtCod21015.EditValue = codeudor2.PF_CtasTotTDC.Value;
                        this.txtCod21016.EditValue = datCredCodeudor2.VlrCuotasREA.Value;
                        this.txtCod21017.EditValue = codeudor2.PF_EstCtasREA.Value;
                        this.txtCod21018.EditValue = codeudor2.PF_CtasTotREA.Value;
                        this.txtCod21019.EditValue = datCredCodeudor2.VlrCuotasCEL.Value;
                        this.txtCod21020.EditValue = codeudor2.PF_EstCtasCEL.Value;
                        this.txtCod21022.EditValue = codeudor2.PF_CtasTotCEL.Value;
                        this.txtCod21022.EditValue = codeudor2.PF_CtasTotIngEst.Value;
                        #endregion

                        #region Endeudamiento
                        this.txtCod21102.EditValue = codeudor2.PF_QuiantiMIN.Value;
                        this.txtCod21102.EditValue = codeudor2.PF_QuiantiMAX.Value;
                        this.txtCod21103.EditValue = codeudor2.PF_QuantoIngrEst.Value;
                        this.txtCod21104.EditValue = codeudor2.PF_IngrEstxQuanto.Value;
                        this.txtCod21105.EditValue = codeudor2.IngresosREG.Value;
                        this.txtCod21106.EditValue = codeudor2.PF_FactIngresosREG.Value;
                        this.txtCod21107.EditValue = codeudor2.IngresosSOP.Value;
                        this.txtCod21108.EditValue = codeudor2.PF_IngrCapacPAG.Value;
                        this.txtCod21109.EditValue = codeudor2.PF_PorIngrAporta.Value;
                        this.chkCod21110.EditValue = codeudor2.PF_ReqSopIngrInd.Value;
                        this.txtCod21112.EditValue = codeudor2.PF_PorIngrPagoCtas.Value;
                        this.txtCod21112.EditValue = codeudor2.PF_IngrDispPagoCtas.Value;
                        this.txtCod21113.EditValue = codeudor2.PF_CuotasACT.Value;
                        this.txtCod21114.EditValue = codeudor2.PF_IngrDispApoyos.Value;
                        #endregion

                    }
                    else
                    {
                    }
                    #endregion                    //Codeudor3 (TipoPersona 5)
                    //Codeudor3 (TipoPersona 5)

                    #region codeudor3
                    if (codeudor3 != null)
                    {
                        this.linkCodeudor3.Dock = DockStyle.None;
                        this.linkCodeudor30.Dock = DockStyle.None;
                        this.linkCodeudor31.Dock = DockStyle.None;
                        this.linkCodeudor32.Dock = DockStyle.None;
                        this.linkCodeudor33.Dock = DockStyle.None;
                        this.linkCodeudor34.Dock = DockStyle.None;
                        this.linkCodeudor35.Dock = DockStyle.None;
                        this.linkCodeudor36.Dock = DockStyle.None;
                        this.linkCodeudor37.Dock = DockStyle.None;
                        this.linkCodeudor38.Dock = DockStyle.None;
                        this.linkCodeudor39.Dock = DockStyle.None;
                        this.linkCodeudor310.Dock = DockStyle.None;
                        #region Finca Raiz

                        this.txtInmuebleCod3.EditValue = codeudor3.NroInmuebles.Value.Value;
                        this.txtHipotecasCod3.EditValue = codeudor3.HipotecasNro.Value.Value;
                        this.txtRestriccionCod3.EditValue = codeudor3.RestriccionesNro.Value.Value;
                        this.txtPorcTablaCod3.EditValue = codeudor3.PF_FincaRaizDato.Value;
                        this.txtFactorCod3.EditValue = codeudor3.PF_FincaRaiz.Value;
                        #endregion
                        #region Probabilidad
                        this.txtCod3101.EditValue = codeudor3.PF_MorasUltAno.Value;
                        this.txtCod3102.EditValue = Scorecodeudor3.Puntaje.Value;
                        this.txtCod3103.EditValue = codeudor3.PF_FactorAcierta.Value;
                        this.txtCod3104.EditValue = codeudor3.PF_AciertaResultado.Value;
                        #endregion
                        #region Mora Actual
                        this.txtCod3201.EditValue = codeudor3.PF_MorasActuales.Value;
                        this.txtCod3202.EditValue = datCredcodeudor3.EstadoAct30.Value;
                        this.txtCod3203.EditValue = codeudor3.PF_MorasAct30Dato.Value;
                        this.txtCod3204.EditValue = codeudor3.PF_MorasAct30.Value;
                        this.txtCod3205.EditValue = datCredcodeudor3.EstadoAct60.Value;
                        this.txtCod3206.EditValue = codeudor3.PF_MorasAct60Dato.Value;
                        this.txtCod3207.EditValue = codeudor3.PF_MorasAct60.Value;
                        this.txtCod3208.EditValue = datCredcodeudor3.EstadoAct90.Value;
                        this.txtCod3209.EditValue = codeudor3.PF_MorasAct90Dato.Value;
                        this.txtCod3210.EditValue = codeudor3.PF_MorasAct90.Value;
                        this.txtCod3212.EditValue = datCredcodeudor3.EstadoAct120.Value;
                        this.txtCod3212.EditValue = codeudor3.PF_MorasAct120Dato.Value;
                        this.txtCod3213.EditValue = codeudor3.PF_MorasAct120.Value;
                        #endregion
                        #region Mora Ultimo Año
                        this.txtCod3301.EditValue = codeudor3.PF_MorasUltAno.Value;
                        this.txtCod3302.EditValue = datCredcodeudor3.EstadoHis30.Value;
                        this.txtCod3303.EditValue = codeudor3.PF_MorasUlt30Dato.Value;
                        this.txtCod3304.EditValue = codeudor3.PF_MorasUlt30.Value;
                        this.txtCod3305.EditValue = datCredcodeudor3.EstadoHis60.Value;
                        this.txtCod3306.EditValue = codeudor3.PF_MorasUlt60Dato.Value;
                        this.txtCod3307.EditValue = codeudor3.PF_MorasUlt60.Value;
                        this.txtCod3308.EditValue = datCredcodeudor3.EstadoHis90.Value;
                        this.txtCod3309.EditValue = codeudor3.PF_MorasUlt90Dato.Value;
                        this.txtCod3310.EditValue = codeudor3.PF_MorasUlt90.Value;
                        this.txtCod3312.EditValue = datCredcodeudor3.EstadoHis120.Value;
                        this.txtCod3312.EditValue = codeudor3.PF_MorasUlt120Dato.Value;
                        this.txtCod3313.EditValue = codeudor3.PF_MorasUlt120.Value;
                        #endregion
                        #region Reportes Negativos
                        this.txtCod3401.EditValue = codeudor3.PF_RepNegativos.Value;
                        this.txtCod3402.EditValue = datCredcodeudor3.EstadoActCob.Value;
                        this.txtCod3403.EditValue = codeudor3.PF_ObligacionCOBDato.Value;
                        this.txtCod3404.EditValue = codeudor3.PF_ObligacionCOB.Value;
                        this.txtCod3405.EditValue = datCredcodeudor3.EstadoActDud.Value;
                        this.txtCod3406.EditValue = codeudor3.PF_ObligacionDUDDato.Value;
                        this.txtCod3407.EditValue = codeudor3.PF_ObligacionDUD.Value;
                        this.txtCod3408.EditValue = datCredcodeudor3.EstadoActCas.Value;
                        this.txtCod3409.EditValue = codeudor3.PF_ObligacionCASDato.Value;
                        this.txtCod3410.EditValue = codeudor3.PF_ObligacionCAS.Value;
                        this.txtCod3411.EditValue = datCredcodeudor3.CtasEmbargadas.Value;
                        this.txtCod3412.EditValue = codeudor3.PF_ObligacionEMBDato.Value;
                        this.txtCod3413.EditValue = codeudor3.PF_ObligacionEMB.Value;
                        this.txtCod3414.EditValue = datCredcodeudor3.EstadoHisRec.Value;
                        this.txtCod3415.EditValue = codeudor3.PF_ObligacionRECDato.Value;
                        this.txtCod3416.EditValue = codeudor3.PF_ObligacionREC.Value;
                        this.txtCod3417.EditValue = datCredcodeudor3.CtasMalManejo.Value;
                        this.txtCod3418.EditValue = codeudor3.PF_ObligacionCANDato.Value;
                        this.txtCod3419.EditValue = codeudor3.PF_ObligacionCAN.Value;
                        #endregion
                        #region Estado Actual
                        this.txtCod3501.EditValue = codeudor3.PF_EstadoActual.Value;
                        this.txtCod3502.EditValue = datCredcodeudor3.NumeObligACT.Value;
                        this.txtCod3503.EditValue = codeudor3.PF_PorObligacionesDato.Value;
                        this.txtCod3504.EditValue = codeudor3.PF_PorObligaciones.Value;
                        this.txtCod3505.EditValue = datCredcodeudor3.NumeroTDC.Value;
                        this.txtCod3506.EditValue = codeudor3.PF_PorUtilizaTDCDato.Value;
                        this.txtCod3507.EditValue = codeudor3.PF_PorUtilizaTDC.Value;
                        this.txtCod3508.EditValue = datCredcodeudor3.PeorEndeudT2.Value;
                        this.txtCod3509.EditValue = codeudor3.PF_PeorCalificacionDato.Value;
                        this.txtCod3510.EditValue = codeudor3.PF_PeorCalificacion.Value;
                        this.txtCod3511.EditValue = datCredcodeudor3.UltConsultas.Value;
                        this.txtCod3512.EditValue = codeudor3.PF_Consultas6MesesDato.Value;
                        this.txtCod3513.EditValue = codeudor3.PF_Consultas6Meses.Value;
                        #endregion
                        #region Estabilidad
                        this.txtCod3602.EditValue = codeudor3.PF_Estabilidad.Value;
                        this.txtCod3602.EditValue = codeudor3.PF_DireccDesdeMeses.Value;
                        this.txtCod3603.EditValue = codeudor3.PF_DireccDesdeDato.Value;
                        this.txtCod3604.EditValue = codeudor3.PF_DireccDesde.Value;
                        this.txtCod3605.EditValue = 0;
                        this.txtCod3606.EditValue = codeudor3.PF_EntidadesNumDato.Value;
                        this.txtCod3607.EditValue = codeudor3.PF_EntidadesNum.Value;
                        this.txtCod3608.EditValue = codeudor3.PF_CelularDesdeMeses.Value;
                        this.txtCod3609.EditValue = codeudor3.PF_CelularDesdeDato.Value;
                        this.txtCod3610.EditValue = codeudor3.PF_CelularDesde.Value;
                        this.txtCod3611.EditValue = codeudor3.PF_CorreoDesdeMeses.Value;
                        this.txtCod3612.EditValue = codeudor3.PF_CorreoDesdeDato.Value;
                        this.txtCod3613.EditValue = codeudor3.PF_CorreoDesde.Value;
                        #endregion
                        #region Ubicabilidad
                        this.txtCod3701.EditValue = codeudor3.PF_Ubicabilidad.Value;
                        this.txtCod3702.EditValue = codeudor3.PF_DireccionNumCant.Value;
                        this.txtCod3703.EditValue = codeudor3.PF_DireccionNumDato.Value;
                        this.txtCod3704.EditValue = codeudor3.PF_DireccionNum.Value;
                        this.txtCod3705.EditValue = codeudor3.PF_TelefonoNumCant.Value;
                        this.txtCod3706.EditValue = codeudor3.PF_TelefonoNumDato.Value;
                        this.txtCod3707.EditValue = codeudor3.PF_TelefonoNum.Value;
                        this.txtCod3708.EditValue = codeudor3.PF_CelularNumCant.Value;
                        this.txtCod3709.EditValue = codeudor3.PF_CelularNumDato.Value;
                        this.txtCod3710.EditValue = codeudor3.PF_CelularNum.Value;
                        this.txtCod3711.EditValue = codeudor3.PF_CorreoNumCant.Value;
                        this.txtCod3712.EditValue = codeudor3.PF_CorreoNumDato.Value;
                        this.txtCod3713.EditValue = codeudor3.PF_CorreoNum.Value;
                        #endregion

                        #region % Max financiacion
                        this.txtCod3801.EditValue = codeudor3.PF_PorMaxFincaRaiz.Value;
                        this.txtCod3802.EditValue = codeudor3.PF_PorMaxMorasActuales.Value;
                        this.txtCod3803.EditValue = codeudor3.PF_PorMaxMorasUltAno.Value;
                        this.txtCod3804.EditValue = codeudor3.PF_PorMaxRepNegativos.Value;
                        this.txtCod3805.EditValue = codeudor3.PF_PorMaxEstadoActual.Value;
                        this.txtCod3806.EditValue = codeudor3.PF_PorMaxProbabilidad.Value;
                        this.txtCod3807.EditValue = codeudor3.PF_PorMaxEstabilidad.Value;
                        this.txtCod3808.EditValue = codeudor3.PF_PorMaxUbicabilidad.Value;
                        this.txtCod3809.EditValue = Convert.ToDecimal(codeudor3.PF_PorMaxFincaRaiz.Value) + Convert.ToDecimal(codeudor3.PF_PorMaxMorasActuales.Value)
                            + Convert.ToDecimal(codeudor3.PF_PorMaxMorasUltAno.Value) + Convert.ToDecimal(codeudor3.PF_PorMaxRepNegativos.Value)
                            + Convert.ToDecimal(codeudor3.PF_PorMaxEstadoActual.Value) + Convert.ToDecimal(codeudor3.PF_PorMaxProbabilidad.Value)
                            + Convert.ToDecimal(codeudor3.PF_PorMaxEstabilidad.Value) + Convert.ToDecimal(codeudor3.PF_PorMaxUbicabilidad.Value);

                        #endregion

                        #region Endeudamiento
                        this.txtCod3901.EditValue = datCredcodeudor3.VlrSaldoVIV.Value;
                        this.txtCod3902.EditValue = datCredcodeudor3.VlrSaldoBAN.Value;
                        this.txtCod3903.EditValue = datCredcodeudor3.VlrSaldoFIN.Value;
                        this.txtCod3904.EditValue = datCredcodeudor3.VlrSaldoCOP.Value;
                        this.txtCod3905.EditValue = datCredcodeudor3.VlrUtilizadoTDC.Value;
                        this.txtCod3906.EditValue = datCredcodeudor3.VlrSaldoREA.Value;
                        this.txtCod3907.EditValue = datCredcodeudor3.VlrCuotasCEL.Value;
                        this.txtCod3908.EditValue = Convert.ToDecimal(datCredcodeudor3.VlrSaldoVIV.Value) + Convert.ToDecimal(datCredcodeudor3.VlrSaldoBAN.Value)
                             + Convert.ToDecimal(datCredcodeudor3.VlrSaldoFIN.Value) + Convert.ToDecimal(datCredcodeudor3.VlrSaldoCOP.Value)
                             + Convert.ToDecimal(datCredcodeudor3.VlrUtilizadoTDC.Value) + Convert.ToDecimal(datCredcodeudor3.VlrCuotasCEL.Value) + Convert.ToDecimal(datCredcodeudor3.VlrSaldoREA.Value);
                        #endregion


                        #region Cuotas
                        this.txtCod31001.EditValue = datCredcodeudor3.VlrCuotasVIV.Value;
                        this.txtCod31002.EditValue = codeudor3.PF_EstCtasVIV.Value;
                        this.txtCod31003.EditValue = codeudor3.PF_CtasTotVIV.Value;
                        this.txtCod31004.EditValue = datCredcodeudor3.VlrCuotasBAN.Value;
                        this.txtCod31005.EditValue = codeudor3.PF_EstCtasBAN.Value;
                        this.txtCod31006.EditValue = codeudor3.PF_CtasTotBAN.Value;
                        this.txtCod31007.EditValue = datCredcodeudor3.VlrCuotasFIN.Value;
                        this.txtCod31008.EditValue = codeudor3.PF_EstCtasFIN.Value;
                        this.txtCod31009.EditValue = codeudor3.PF_CtasTotFIN.Value;
                        this.txtCod31010.EditValue = datCredcodeudor3.VlrCuotasCOP.Value;
                        this.txtCod31012.EditValue = codeudor3.PF_EstCtasCOP.Value;
                        this.txtCod31012.EditValue = codeudor3.PF_CtasTotCOP.Value;
                        this.txtCod31013.EditValue = datCredcodeudor3.VlrCuotasTDC.Value;
                        this.txtCod31014.EditValue = codeudor3.PF_EstCtasTDC.Value;
                        this.txtCod31015.EditValue = codeudor3.PF_CtasTotTDC.Value;
                        this.txtCod31016.EditValue = datCredcodeudor3.VlrCuotasREA.Value;
                        this.txtCod31017.EditValue = codeudor3.PF_EstCtasREA.Value;
                        this.txtCod31018.EditValue = codeudor3.PF_CtasTotREA.Value;
                        this.txtCod31019.EditValue = datCredcodeudor3.VlrCuotasCEL.Value;
                        this.txtCod31020.EditValue = codeudor3.PF_EstCtasCEL.Value;
                        this.txtCod31022.EditValue = codeudor3.PF_CtasTotCEL.Value;
                        this.txtCod31022.EditValue = codeudor3.PF_CtasTotIngEst.Value;
                        #endregion

                        #region Endeudamiento
                        this.txtCod31102.EditValue = codeudor3.PF_QuiantiMIN.Value;
                        this.txtCod31102.EditValue = codeudor3.PF_QuiantiMAX.Value;
                        this.txtCod31103.EditValue = codeudor3.PF_QuantoIngrEst.Value;
                        this.txtCod31104.EditValue = codeudor3.PF_IngrEstxQuanto.Value;
                        this.txtCod31105.EditValue = codeudor3.IngresosREG.Value;
                        this.txtCod31106.EditValue = codeudor3.PF_FactIngresosREG.Value;
                        this.txtCod31107.EditValue = codeudor3.IngresosSOP.Value;
                        this.txtCod31108.EditValue = codeudor3.PF_IngrCapacPAG.Value;
                        this.txtCod31109.EditValue = codeudor3.PF_PorIngrAporta.Value;
                        this.chkCod31110.EditValue = codeudor3.PF_ReqSopIngrInd.Value;
                        this.txtCod31112.EditValue = codeudor3.PF_PorIngrPagoCtas.Value;
                        this.txtCod31112.EditValue = codeudor3.PF_IngrDispPagoCtas.Value;
                        this.txtCod31113.EditValue = codeudor3.PF_CuotasACT.Value;
                        this.txtCod31114.EditValue = codeudor3.PF_IngrDispApoyos.Value;
                        #endregion

                    }
                    else
                    {
                    }
                    #endregion                 
                    //Capacidad de Pago
                    #region Capacidad de Pago
                    this.txtVeh001.EditValue = otros.PF_PorIngrPagoCtas.Value;
                    this.txtVeh002.EditValue = otros.PF_IngrDispApoyosDEU.Value;
                    this.txtVeh003.EditValue = otros.PF_IngrDispApoyosCON.Value;
                    this.txtVeh004.EditValue = otros.PF_IngrDispApoyos.Value;
                    this.txtVeh005.EditValue = otros.PF_ReqSopIngrIndDEU.Value;
                    this.txtVeh006.EditValue = otros.PF_ReqSopIngrIndCON.Value;
                    
                    this.txtVeh007.EditValue = otros.PF_Cuantia.Value;
                    this.txtVeh008.EditValue = otros.PF_TasaTablEva1.Value;
                    this.txtVeh009.EditValue = otros.PF_FactTablEva.Value;
                    this.txtVeh010.EditValue = otros.PF_TasaTablEva2.Value;
                    this.txtVeh011.EditValue = otros.PF_TasaPonderada.Value;
                    #region solicitado
                    this.txtsol1.EditValue = otros.PF_VlrMontoSOL.Value;
                    this.txtsol2.EditValue = otros.PF_CtaFinanciaSOL.Value;
                    this.txtsol3.EditValue = otros.PF_CtaSeguroSOL.Value;
                    this.txtsol4.EditValue = otros.PF_CtaTotalSOL.Value;
                    this.txtsol5.EditValue = otros.PF_CtaApoyosDifSOL.Value;
                    this.txtsol6.EditValue = otros.PF_IngDispApoyosSOL.Value;
                    this.txtsol7.EditValue = otros.PF_IngReqDeu1SOL.Value;
                    this.txtsol8.EditValue = otros.PF_IngReqDeu2SOL.Value;
                    this.txtsol9.EditValue = otros.PF_IngReqDeu3SOL.Value;
                    this.txtsol10.EditValue = otros.PF_IngReqDeuFinSOL.Value;
                    this.txtsol11.EditValue = otros.PF_IngReqCon1SOL.Value;
                    this.txtsol12.EditValue = otros.PF_IngReqCon2SOL.Value;
                    this.txtsol13.EditValue = otros.PF_IngReqConFinSOL.Value;
                    this.txtsol14.EditValue = 0;
                    #endregion
                    #region ajustados
                    this.txtaju1.EditValue = otros.PF_VlrMontoAJU.Value;
                    this.txtaju2.EditValue = otros.PF_CtaFinanciaAJU.Value;
                    this.txtaju3.EditValue = otros.PF_CtaSeguroAJU.Value;
                    this.txtaju4.EditValue = otros.PF_CtaTotalAJU.Value;
                    this.txtaju5.EditValue = otros.PF_CtaApoyosDifAJU.Value;
                    this.txtaju6.EditValue = otros.PF_IngDispApoyosAJU.Value;
                    this.txtaju7.EditValue = otros.PF_IngReqDeu1AJU.Value;
                    this.txtaju8.EditValue = otros.PF_IngReqDeu2AJU.Value;
                    this.txtaju9.EditValue = otros.PF_IngReqDeuAJU.Value;
                    this.txtaju10.EditValue = otros.PF_IngReqDeuFinAJU.Value;
                    this.txtaju11.EditValue = otros.PF_IngReqCon1AJU.Value;
                    this.txtaju12.EditValue = otros.PF_IngReqCon2AJU.Value;
                    this.txtaju13.EditValue = otros.PF_IngReqConFinAJU.Value;
                    this.txtaju14.EditValue = 0;
                    #endregion
                    #region FIN
                    this.txtfin1.EditValue = otros.PF_VlrMontoFIN.Value;
                    this.txtfin2.EditValue = otros.PF_CtaFinanciaFIN.Value;
                    this.txtfin3.EditValue = otros.PF_CtaSeguroFIN.Value;
                    this.txtfin4.EditValue = otros.PF_CtaTotalFIN.Value;
                    this.txtfin5.EditValue = otros.PF_CtaApoyosDifFIN.Value;
                    this.txtfin6.EditValue = otros.PF_IngDispApoyosFIN.Value;
                    this.txtfin7.EditValue = otros.PF_IngReqDeu1FIN.Value;
                    this.txtfin8.EditValue = otros.PF_IngReqDeu2FIN.Value;
                    this.txtfin9.EditValue = otros.PF_IngReqDeu3FIN.Value;
                    this.txtfin10.EditValue = otros.PF_IngReqDeuFinFIN.Value;
                    this.txtfin11.EditValue = otros.PF_IngReqCon1FIN.Value;
                    this.txtfin12.EditValue = otros.PF_IngReqCon2FIN.Value;
                    this.txtfin13.EditValue = otros.PF_IngReqConFinFIN.Value;
                    this.txtfin14.EditValue = 0;
                    #endregion
                    #endregion
                }
                #endregion
                else
                {
                    #region Llena datos de los controles para salvar
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "AssignValues"));
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
                }
                else if (this._ctrl.Estado.Value.Value == (int)EstadoDocControl.Cerrado)
                {
                    //Mostrar mensaje de que esta libranza esta cerrada
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Cc_LibranzaCerrada));
                    CleanData();
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "Form_Enter"));
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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "Form_Closing"));
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

                Type frm = typeof(Perfil);
                FormProvider.GetInstance(frm, new object[] { this._data.SolicituDocu.ClienteRadica.Value, this._data.SolicituDocu.Libranza.Value, true });

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DetallePerfil.cs", "Form_FormClosed"));
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
            //if (this.linkConyuge.Dock == DockStyle.Fill)
            //{
            //    this.linkConyuge.Dock = DockStyle.None;
            //}
            //else
            //{
            //    this.linkConyuge.Dock = DockStyle.Fill;
            //}
        }
        private void linkCodeudor1_OpenLink(object sender, OpenLinkEventArgs e)
        {
            //if (linkCodeudor1.Dock == DockStyle.Fill)
            //{
            //    linkCodeudor1.Dock = DockStyle.None;
            //}
            //else
            //{
            //    linkCodeudor1.Dock = DockStyle.Fill;
            //}
        }

        private void linkCodeudor2_OpenLink(object sender, OpenLinkEventArgs e)
        {
            //if (linkCodeudor2.Dock == DockStyle.Fill)
            //{
            //    linkCodeudor2.Dock = DockStyle.None;
            //}
            //else
            //{
            //    linkCodeudor2.Dock = DockStyle.Fill;
            //}
        }

        private void linkCodeudor3_OpenLink(object sender, OpenLinkEventArgs e)
        {
            //if (linkCodeudor3.Dock == DockStyle.Fill)
            //{
            //    linkCodeudor3.Dock = DockStyle.None;
            //}
            //else
            //{
            //    linkCodeudor3.Dock = DockStyle.Fill;
            //}
        }



        #endregion Eventos Formulario

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textEdit47_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit66_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit67_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit68_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit147_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void tpFincaRaiz_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label136_Click(object sender, EventArgs e)
        {

        }

        private void textEdit113_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit86_EditValueChanged(object sender, EventArgs e)
        {

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