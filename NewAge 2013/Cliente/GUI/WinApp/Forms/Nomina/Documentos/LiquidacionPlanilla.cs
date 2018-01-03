using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Threading;
using System.IO;
using DevExpress.XtraEditors;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidacionPlanilla : DocumentNominaBaseForm
    {
        public LiquidacionPlanilla()
        {
            //this.InitializeComponent(); 
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();

        Dictionary<int, string> itemsTipoPlanilla = null;
        Dictionary<int, string> itemsTipoContizante = null;
        Dictionary<int, string> itemsSubtipo = null;

        DTO_noPlanillaAportesDeta _planillaAportes = null;
        List<DTO_noPlanillaAportesDeta> _lplanillasTemp = new List<DTO_noPlanillaAportesDeta>();
        private bool EditRegMode = false;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que actualiza el formulario despues de salvar
        /// </summary>
        protected override void SaveMethod()
        {
            ModalNominaValidacion validateForm = new ModalNominaValidacion(this.uc_Empleados._empleados, AppDocuments.PlanillaAutoLiquidAportes);
            if (validateForm.HayValidaciones)
            {
                validateForm.ShowDialog();
                if (!validateForm.Continuar)
                {
                    return;
                }
                else
                {
                    this.uc_Empleados._empleados = validateForm.Empleados;
                }
            }

            FormProvider.Master.Invoke(FormProvider.Master.ReportStatusDelegate, new object[] { AppDocuments.Nomina, _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.SavingServer) });
            FormProvider.Master.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoNomina(this.documentID));

            ParameterizedThreadStart pth = new ParameterizedThreadStart(FormProvider.Master.CheckServerProcessStatus);
            FormProvider.Master.ProgressBarThread = new Thread(pth);
            FormProvider.Master.ProgressBarThread.Start(this.documentID);

            List<DTO_TxResult> results = new List<DTO_TxResult>();

            this.uc_Empleados._empleados = this.uc_Empleados._empleados.FindAll(x => x.ActivoInd.Value.Value);
            if (this.uc_Empleados._empleados.Count > 0)
            {
                results = _bc.AdministrationModel.LiquidarPlanilla(this.dtPeriod.DateTime, this.dtFecha.DateTime, this.uc_Empleados._empleados);
            }
            else
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = DictionaryMessages.Err_No_EmpleadoSelect;
                results.Add(result);
            }

            FormProvider.Master.StopProgressBarThread(this.documentID);
            List<DTO_TxResult> resultsNOK = new List<DTO_TxResult>();
            foreach (DTO_TxResult result in results)
            {
                if (result.Result == ResultValue.NOK)
                    resultsNOK.Add(result);
            }

            MessageForm frm = new MessageForm(resultsNOK);
            this.Invoke(FormProvider.Master.ShowResultDialogDelegate, new Object[] { frm });    

            //Recarga la grilla de novedades
            this.LoadData(false);
        }


        #endregion
        
        #region Funciones Privadas
                
        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {
            this.EditModeControls(true);

            //Carga Combo de Tipo de Documento
            long countTipoDocumento = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.coTerceroDocTipo, null, null, true);
            var itemsTipoDocumento = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coTerceroDocTipo, countTipoDocumento, 1, null, null, true);
            Dictionary<string, string> itemsTipoDoc = new Dictionary<string, string>();
            foreach (var item in itemsTipoDocumento)
                itemsTipoDoc.Add(item.ID.Value, item.Descriptivo.Value);
            this.lkCmbTipoDoc.Properties.DataSource = itemsTipoDoc;
            this.lkCmbTipoDoc.EditValue = itemsTipoDocumento.Select(x => x.ID).FirstOrDefault();

            //Carga Combo Tipo Contizante
            itemsTipoPlanilla = new Dictionary<int, string>();
            itemsTipoPlanilla.Add(1, "Normal");
            itemsTipoPlanilla.Add(2, "Suspención Laboral/Licencia Remunerada");
            this.lkCmbTipoPlanilla.Properties.DataSource = this.itemsTipoPlanilla;
            this.lkCmbTipoPlanilla.EditValue =1;


            //Carga Combo Tipo Contizante
            itemsTipoContizante = new Dictionary<int, string>();
            itemsTipoContizante.Add(1, "01-General");
            itemsTipoContizante.Add(2, "20-Sena Lectivo");
            itemsTipoContizante.Add(3, "19-Sena Productivo");

            this.lkCmbTipoCotizante.Properties.DataSource = this.itemsTipoContizante;
            this.lkCmbTipoCotizante.EditValue = 1;

            //Carga Combo SubTipo
            itemsSubtipo = new Dictionary<int, string>();
            itemsSubtipo.Add(1, "00-General");
            itemsSubtipo.Add(2, "04-No aporta Pensión");
            this.lkCmbSubtipo.Properties.DataSource = this.itemsSubtipo;
            this.lkCmbSubtipo.EditValue = 1;

            //Inicializa Master Find's
            this._bc.InitMasterUC(this.uc_LugarGeo, AppMasters.glLugarGeografico, true, true, true, false);
            this._bc.InitMasterUC(this.uc_FondoPension, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_FondoPensionTR, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_FondoSalud, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_FondoSaludTR, AppMasters.noFondo, false, true, true, false);
            this._bc.InitMasterUC(this.uc_CajaCompensacion, AppMasters.noCaja, false, true, true, false);          
        
        }

        /// <summary>
        /// Carga la información de en los controles 
        /// </summary>
        private void LoadInfoControls()
        {
            if (_planillaAportes != null)
            {
                this.lkCmbTipoPlanilla.EditValue = this._planillaAportes.TipoTrplanilla.Value.ToString();

                List<DTO_glConsultaFiltro> lfiltros = new List<DTO_glConsultaFiltro>();
                lfiltros.Add(new DTO_glConsultaFiltro()
                {
                    CampoFisico = "TipoDocNomina",
                    ValorFiltro = this._planillaAportes.TipoDocNomina.Value.ToString(),
                    OperadorFiltro = SentenceTransformer.OperadorFiltro.Igual,
                    OperadorSentencia = "AND"
                });

                long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.coTerceroDocTipo, null, lfiltros, true);
                var tipoDoc = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.coTerceroDocTipo, count, 1, null, lfiltros, true).FirstOrDefault();

                if (tipoDoc != null)
                    this.lkCmbTipoDoc.EditValue = ((DTO_coTerceroDocTipo)tipoDoc).ID.Value;

                this.lkCmbTipoCotizante.EditValue = this._planillaAportes.TipoCotizante.Value.ToString();
                this.lkCmbSubtipo.EditValue = this._planillaAportes.SubTipo.Value.ToString();
                this.txtIndExt.Text = this._planillaAportes.ExteriorInd.Value.Value ? "1" : "0";
                this.txtIndExterior.Text = this._planillaAportes.ExtranjeroInd.Value.Value ? "1" : "0";
                this.uc_LugarGeo.Value = this._planillaAportes.LugarGeograficoID.Value;
                this.txtSueldo.Text = this._planillaAportes.Sueldo.Value.ToString();
                this.checkSalIntegral.Checked = this._planillaAportes.SalIntegralInd.Value.Value;
                this.checkINGInd.Checked = this._planillaAportes.INGInd.Value.Value;
                this.checkRETInd.Checked = this._planillaAportes.RETInd.Value.Value;
                this.checkTDEInd.Checked = this._planillaAportes.TDEInd.Value.Value;
                this.checkTAEInd.Checked = this._planillaAportes.TAEInd.Value.Value;
                this.checkTDPInd.Checked = this._planillaAportes.TDPInd.Value.Value;
                this.checkTAPInd.Checked = this._planillaAportes.TAPInd.Value.Value;
                this.checkVSPInd.Checked = this._planillaAportes.VSPInd.Value.Value;
                this.checkVTEInd.Checked = this._planillaAportes.VTEInd.Value.Value;
                this.checkVSTInd.Checked = this._planillaAportes.VSTInd.Value.Value;
                this.checkSLNInd.Checked = this._planillaAportes.SLNInd.Value.Value;
                this.checkIGEInd.Checked = this._planillaAportes.IGEInd.Value.Value;
                this.checkLMAInd.Checked = this._planillaAportes.LMAInd.Value.Value;
                this.checkVACInd.Checked = this._planillaAportes.VACInd.Value.Value;
                this.checkAVPInd.Checked = this._planillaAportes.AVPInd.Value.Value;
                this.checkVCTInd.Checked = this._planillaAportes.VCTInd.Value.Value;
                this.checkIRPInd.Checked = this._planillaAportes.IRPInd.Value.Value;

                this.uc_FondoPension.Value = this._planillaAportes.FondoPension.Value;
                this.txtDiasCotizadosPension.EditValue = this._planillaAportes.DiasCotizadosPEN.Value;
                this.txtIngresoBasePension.EditValue = this._planillaAportes.IngresoBasePEN.Value;
                this.txtTarifaPension.EditValue = this._planillaAportes.TarifaPEN.Value;
                this.txtVlrEmpresaPEN.EditValue = this._planillaAportes.VlrEmpresaPEN.Value;
                this.txtVlrTrabajadorPEN.EditValue = this._planillaAportes.VlrTrabajadorPEN.Value;

                this.txtVlrSolidaridad.EditValue = this._planillaAportes.VlrSolidaridad.Value;
                this.txtVlrSubsistencia.EditValue = this._planillaAportes.VlrSubsistencia.Value;
                this.txtVlrNoRetenido.EditValue = this._planillaAportes.VlrNoRetenido.Value;

                this.uc_FondoSalud.Value = this._planillaAportes.FondoSalud.Value;
                this.txtDiasCotizadosSLD.EditValue = this._planillaAportes.DiasCotizadosSLD.Value;
                this.txtIngresoBaseSLD.EditValue = this._planillaAportes.IngresoBaseSLD.Value;
                this.txtTarifaSLD.EditValue = this._planillaAportes.TarifaSLD.Value;
                this.txtVlrEmpresaSLD.EditValue = this._planillaAportes.VlrEmpresaSLD.Value;
                this.txtVlrTrabajadorSLD.EditValue = this._planillaAportes.VlrTrabajadorSLD.Value;
                this.txtVlrUPC.EditValue = this._planillaAportes.VlrUPC.Value;

                this.txtDiasARP.EditValue = this._planillaAportes.DiasCotizadosARP.Value;
                this.txtTarifaARP.EditValue = this._planillaAportes.TarifaARP.Value;
                this.txtIngresoBaseARP.EditValue = this._planillaAportes.IngresoBaseARP.Value;

                this.txtCentroTrabajo.EditValue = this._planillaAportes.CtoARP.Value;
                this.txtValorAporteARP.EditValue = this._planillaAportes.VlrARP.Value;
                this.uc_CajaCompensacion.Value = this._planillaAportes.CajaNOID.Value;
                this.txtDiasCotizacionCC.EditValue = this._planillaAportes.DiasCotizadosCCF.Value;
                this.txtIngresoBaseCC.EditValue = this._planillaAportes.IngresoBaseCCF.Value;
                this.txtTarifaCC.EditValue = this._planillaAportes.TarifaCCF.Value;
                this.txtValorAporteCC.EditValue = this._planillaAportes.VlrCCF.Value;
                this.txtTarifaICBF.EditValue = this._planillaAportes.TarifaIBF.Value;
                this.txtValorICBF.EditValue = this._planillaAportes.VlrICBF.Value;
                this.txtTarifaSENA.EditValue = this._planillaAportes.TarifaSEN.Value;
                this.txtValorSENA.EditValue = this._planillaAportes.VlrSEN.Value;
            }
            else
            {
                this.lkCmbTipoPlanilla.EditValue = 1;
                this.lkCmbTipoCotizante.EditValue = 1;
                this.lkCmbSubtipo.EditValue = 1;
                this.txtIndExt.Text = "0";
                this.txtIndExterior.Text = "0";
                this.uc_LugarGeo.Value = string.Empty;
                this.txtSueldo.Text = "0";
                this.checkSalIntegral.Checked = false;
                this.checkINGInd.Checked = false;
                this.checkRETInd.Checked = false;
                this.checkTDEInd.Checked = false;
                this.checkTAEInd.Checked = false;
                this.checkTDPInd.Checked = false;
                this.checkTAPInd.Checked = false;
                this.checkVSPInd.Checked = false;
                this.checkVTEInd.Checked = false;
                this.checkVSTInd.Checked = false;
                this.checkSLNInd.Checked = false;
                this.checkIGEInd.Checked = false;
                this.checkLMAInd.Checked = false;
                this.checkVACInd.Checked = false;
                this.checkAVPInd.Checked = false;
                this.checkVCTInd.Checked = false;
                this.checkIRPInd.Checked = false;

                this.uc_FondoPension.Value = string.Empty;
                this.txtDiasCotizadosPension.EditValue = 0;
                this.txtIngresoBasePension.EditValue = 0;
                this.txtTarifaPension.EditValue = 0;
                this.txtVlrEmpresaPEN.EditValue = 0;
                this.txtVlrTrabajadorPEN.EditValue = 0;

                this.txtVlrSolidaridad.EditValue = 0;
                this.txtVlrSubsistencia.EditValue = 0;
                this.txtVlrNoRetenido.EditValue = 0;

                this.uc_FondoSalud.Value = string.Empty;
                this.txtDiasCotizadosSLD.EditValue = 0;
                this.txtIngresoBaseSLD.EditValue = 0;
                this.txtTarifaSLD.EditValue = 0;
                this.txtVlrEmpresaSLD.EditValue = 0;
                this.txtVlrTrabajadorSLD.EditValue = 0;
                this.txtVlrUPC.EditValue = 0;

                this.txtDiasARP.EditValue = 0;
                this.txtTarifaARP.EditValue = 0;
                this.txtIngresoBaseARP.EditValue = 0;
                this.txtValorAporteARP.EditValue = 0;

                this.txtCentroTrabajo.EditValue = string.Empty;
                this.uc_CajaCompensacion.Value = string.Empty;
                this.txtDiasCotizacionCC.EditValue = 0;
                this.txtIngresoBaseCC.EditValue = 0;
                this.txtTarifaCC.EditValue = 0;
                this.txtValorAporteCC.EditValue = 0;
                this.txtTarifaICBF.EditValue = 0;
                this.txtValorICBF.EditValue = 0;
                this.txtTarifaSENA.EditValue = 0;
                this.txtValorSENA.EditValue = 0;
            }
        }

        /// <summary>
        /// Carga el objeto 
        /// </summary>
        private void LoadObject()
        {
            try
            {
                this._planillaAportes.TipoTrplanilla.Value = !string.IsNullOrEmpty(this.lkCmbTipoPlanilla.EditValue.ToString()) ? byte.Parse(this.lkCmbTipoPlanilla.EditValue.ToString()) : byte.Parse("0");
                this._planillaAportes.TipoDocNomina.Value = ((DTO_coTerceroDocTipo)this.lkCmbTipoDoc.GetSelectedDataRow()).TipoDocNomina.Value;
                this._planillaAportes.TipoCotizante.Value = !string.IsNullOrEmpty(this.lkCmbTipoCotizante.EditValue.ToString()) ? byte.Parse(this.lkCmbTipoCotizante.EditValue.ToString()) : byte.Parse("0");
                this._planillaAportes.SubTipo.Value = !string.IsNullOrEmpty(this.lkCmbSubtipo.EditValue.ToString()) ? byte.Parse(this.lkCmbSubtipo.EditValue.ToString()) : byte.Parse("0");
                this._planillaAportes.ExteriorInd.Value = false;
                this._planillaAportes.ExtranjeroInd.Value = false;
                this._planillaAportes.LugarGeograficoID.Value = this.uc_LugarGeo.Value;
                this._planillaAportes.Sueldo.Value = !string.IsNullOrEmpty(this.txtSueldo.Text) ? decimal.Parse(this.txtSueldo.Text) : 0;
                this._planillaAportes.SalIntegralInd.Value = this.checkSalIntegral.Checked;
                this._planillaAportes.INGInd.Value = this.checkINGInd.Checked;
                this._planillaAportes.RETInd.Value = this.checkRETInd.Checked;
                this._planillaAportes.TDEInd.Value = this.checkTDEInd.Checked;
                this._planillaAportes.TAEInd.Value = this.checkTAEInd.Checked;
                this._planillaAportes.TDPInd.Value = this.checkTDPInd.Checked;
                this._planillaAportes.TAPInd.Value = this.checkTAPInd.Checked;
                this._planillaAportes.VSPInd.Value = this.checkVSPInd.Checked;
                this._planillaAportes.VTEInd.Value = this.checkVTEInd.Checked;
                this._planillaAportes.VSTInd.Value = this.checkVSTInd.Checked;
                this._planillaAportes.SLNInd.Value = this.checkSLNInd.Checked;
                this._planillaAportes.IGEInd.Value = this.checkIGEInd.Checked;
                this._planillaAportes.LMAInd.Value = this.checkLMAInd.Checked;
                this._planillaAportes.VACInd.Value = this.checkVACInd.Checked;
                this._planillaAportes.AVPInd.Value = this.checkAVPInd.Checked;
                this._planillaAportes.VCTInd.Value = this.checkVCTInd.Checked;
                this._planillaAportes.IRPInd.Value = this.checkIRPInd.Checked;

                this._planillaAportes.FondoPension.Value = this.uc_FondoPension.Value;
                this._planillaAportes.DiasCotizadosPEN.Value = !string.IsNullOrEmpty(this.txtDiasCotizadosPension.Text) ? byte.Parse(this.txtDiasCotizadosPension.Text) : byte.Parse("0");
                this._planillaAportes.IngresoBasePEN.Value = !string.IsNullOrEmpty(this.txtIngresoBasePension.Text) ? decimal.Parse(this.txtIngresoBasePension.Text) : decimal.Parse("0");
                this._planillaAportes.TarifaPEN.Value = !string.IsNullOrEmpty(this.txtTarifaPension.Text) ? decimal.Parse(this.txtTarifaPension.Text) : decimal.Parse("0");
                this._planillaAportes.VlrEmpresaPEN.Value = !string.IsNullOrEmpty(this.txtVlrEmpresaPEN.Text) ? decimal.Parse(this.txtVlrEmpresaPEN.Text) : decimal.Parse("0");
                this._planillaAportes.VlrTrabajadorPEN.Value = !string.IsNullOrEmpty(this.txtVlrTrabajadorPEN.Text) ? decimal.Parse(this.txtVlrTrabajadorPEN.Text) : decimal.Parse("0");
                this._planillaAportes.VlrSolidaridad.Value = !string.IsNullOrEmpty(this.txtVlrSolidaridad.Text) ? decimal.Parse(this.txtVlrSolidaridad.Text) : decimal.Parse("0");
                this._planillaAportes.VlrSubsistencia.Value = !string.IsNullOrEmpty(this.txtVlrSubsistencia.Text) ? decimal.Parse(this.txtVlrSubsistencia.Text) : decimal.Parse("0");
                this._planillaAportes.VlrNoRetenido.Value = !string.IsNullOrEmpty(this.txtVlrNoRetenido.Text) ? decimal.Parse(this.txtVlrNoRetenido.Text) : decimal.Parse("0");
                this._planillaAportes.FondoSalud.Value = this.uc_FondoSalud.Value;
                this._planillaAportes.DiasCotizadosSLD.Value = !string.IsNullOrEmpty(this.txtDiasCotizadosSLD.Text) ? byte.Parse(this.txtDiasCotizadosSLD.Text) : byte.Parse("0");
                this._planillaAportes.IngresoBaseSLD.Value = !string.IsNullOrEmpty(this.txtIngresoBaseSLD.Text) ? decimal.Parse(this.txtIngresoBaseSLD.Text) : decimal.Parse("0");
                this._planillaAportes.TarifaSLD.Value = !string.IsNullOrEmpty(this.txtTarifaSLD.Text) ? decimal.Parse(this.txtTarifaSLD.Text) : decimal.Parse("0");
                this._planillaAportes.VlrEmpresaSLD.Value = !string.IsNullOrEmpty(this.txtVlrEmpresaSLD.Text) ? decimal.Parse(this.txtVlrEmpresaSLD.Text) : decimal.Parse("0");
                this._planillaAportes.VlrEmpresaSLD.Value = !string.IsNullOrEmpty(this.txtVlrTrabajadorSLD.Text) ? decimal.Parse(this.txtVlrTrabajadorSLD.Text) : decimal.Parse("0");
                this._planillaAportes.VlrUPC.Value = !string.IsNullOrEmpty(this.txtVlrUPC.Text) ? decimal.Parse(this.txtVlrUPC.Text) : decimal.Parse("0");
                this._planillaAportes.DiasCotizadosARP.Value = !string.IsNullOrEmpty(this.txtDiasARP.Text) ? byte.Parse(this.txtDiasARP.Text) : byte.Parse("0");
                this._planillaAportes.TarifaARP.Value = !string.IsNullOrEmpty(this.txtTarifaARP.Text) ? decimal.Parse(this.txtTarifaARP.Text) : decimal.Parse("0");
                this._planillaAportes.IngresoBaseARP.Value = !string.IsNullOrEmpty(this.txtIngresoBaseARP.Text) ? decimal.Parse(this.txtIngresoBaseARP.Text) : decimal.Parse("0");
                this._planillaAportes.VlrARP.Value = !string.IsNullOrEmpty(this.txtValorAporteARP.Text) ? decimal.Parse(this.txtValorAporteARP.Text) : decimal.Parse("0");
                this._planillaAportes.CtoARP.Value = this.txtCentroTrabajo.Text;
                this._planillaAportes.VlrARP.Value = !string.IsNullOrEmpty(this.txtValorAporteARP.Text) ? decimal.Parse(this.txtValorAporteARP.Text) : decimal.Parse("0");
                this._planillaAportes.CajaNOID.Value = this.uc_CajaCompensacion.Value;
                this._planillaAportes.DiasCotizadosCCF.Value = !string.IsNullOrEmpty(this.txtDiasCotizacionCC.Text) ? byte.Parse(this.txtDiasCotizacionCC.Text) : byte.Parse("0");
                this._planillaAportes.IngresoBaseCCF.Value = !string.IsNullOrEmpty(this.txtIngresoBaseCC.Text) ? decimal.Parse(this.txtIngresoBaseCC.Text) : decimal.Parse("0");
                this._planillaAportes.TarifaCCF.Value = !string.IsNullOrEmpty(this.txtTarifaCC.Text) ? decimal.Parse(this.txtTarifaCC.Text) : decimal.Parse("0");
                this._planillaAportes.VlrCCF.Value = !string.IsNullOrEmpty(this.txtValorAporteCC.Text) ? decimal.Parse(this.txtValorAporteCC.Text) : decimal.Parse("0");
                this._planillaAportes.TarifaCCF.Value = !string.IsNullOrEmpty(this.txtTarifaICBF.Text) ? decimal.Parse(this.txtTarifaICBF.Text) : decimal.Parse("0");
                this._planillaAportes.VlrICBF.Value = !string.IsNullOrEmpty(this.txtValorICBF.Text) ? decimal.Parse(this.txtValorICBF.Text) : decimal.Parse("0");
                this._planillaAportes.TarifaSEN.Value = !string.IsNullOrEmpty(this.txtTarifaSENA.Text) ? decimal.Parse(this.txtTarifaSENA.Text) : decimal.Parse("0");
                this._planillaAportes.VlrSEN.Value = !string.IsNullOrEmpty(this.txtValorSENA.Text) ? decimal.Parse(this.txtValorSENA.Text) : decimal.Parse("0");
                this._planillaAportes.IndExoneradoCCF.Value = true;
                this._planillaAportes.VlrIncapacidad.Value = !string.IsNullOrEmpty(this.txtVlrIncapacidad.Text) ? decimal.Parse(this.txtVlrIncapacidad.Text) : decimal.Parse("0");
                this._planillaAportes.VlrMaternidad.Value = !string.IsNullOrEmpty(this.txtVlrMaternidad.Text) ? decimal.Parse(this.txtVlrMaternidad.Text) : decimal.Parse("0");
                this._planillaAportes.AutorizacionEnf.Value = this.txtAutorizacionEnf.Text;
                this._planillaAportes.AutorizacioMat.Value = this.txtAutorizacioMat.Text;
            }
            catch (Exception ex)
            { 
            
            }
        }

        /// <summary>
        /// Edición de los controles
        /// </summary>
        /// <param name="estado">estado</param>
        private void EditModeControls(bool estado)
        {
            this.lkCmbTipoPlanilla.Properties.ReadOnly = estado;
            this.lkCmbTipoDoc.Properties.ReadOnly = estado;
            this.lkCmbTipoCotizante.Properties.ReadOnly = estado;
            this.lkCmbSubtipo.Properties.ReadOnly = estado;
            this.uc_LugarGeo.EnableControl(!estado);
            this.txtSueldo.Properties.ReadOnly = estado;
            this.checkSalIntegral.Checked = estado;
            this.txtIndExterior.Properties.ReadOnly = !estado;
            this.txtIndExt.Properties.ReadOnly = !estado;
            this.checkINGInd.Enabled = !estado;
            this.checkRETInd.Enabled = !estado;
            this.checkTDEInd.Enabled = !estado;
            this.checkTAEInd.Enabled = !estado;
            this.checkTDPInd.Enabled = !estado;
            this.checkTAPInd.Enabled = !estado;
            this.checkVSPInd.Enabled = !estado;
            this.checkVTEInd.Enabled = !estado;
            this.checkVSTInd.Enabled = !estado;
            this.checkSLNInd.Enabled = !estado;
            this.checkIGEInd.Enabled = !estado;
            this.checkLMAInd.Enabled = !estado;
            this.checkVACInd.Enabled = !estado;
            this.checkAVPInd.Enabled = !estado;
            this.checkVCTInd.Enabled = !estado;
            this.checkIRPInd.Enabled = !estado;
            this.uc_FondoPension.EnableControl(!estado);
            this.uc_FondoPensionTR.EnableControl(!estado);
            this.txtDiasCotizadosPension.Properties.ReadOnly = estado;
            this.txtIngresoBasePension.Properties.ReadOnly = estado;
            this.txtTarifaPension.Properties.ReadOnly = estado;
            this.txtVlrTrabajadorPEN.Properties.ReadOnly = estado;
            this.txtVlrEmpresaPEN.Properties.ReadOnly = estado;
            this.txtVlrSolidaridad.Properties.ReadOnly = estado;
            this.txtVlrSubsistencia.Properties.ReadOnly = estado;
            this.txtVlrNoRetenido.Properties.ReadOnly = estado;
            this.uc_FondoSalud.EnableControl(!estado);
            this.uc_FondoSaludTR.EnableControl(!estado);
            this.txtDiasCotizadosSLD.Properties.ReadOnly = estado;
            this.txtIngresoBaseSLD.Properties.ReadOnly = estado;
            this.txtTarifaSLD.Properties.ReadOnly = estado;
            this.txtVlrTrabajadorSLD.Properties.ReadOnly = estado;
            this.txtVlrEmpresaSLD.Properties.ReadOnly = estado;
            this.txtVlrUPC.Properties.ReadOnly = estado;
            this.txtAutorizacioMat.Properties.ReadOnly = estado;
            this.txtVlrIncapacidad.Properties.ReadOnly = estado;
            this.txtAutorizacionEnf.Properties.ReadOnly = estado;
            this.txtVlrMaternidad.Properties.ReadOnly = estado;
            this.txtDiasARP.Properties.ReadOnly = estado;
            this.txtIngresoBaseARP.Properties.ReadOnly = estado;
            this.txtTarifaARP.Properties.ReadOnly = estado;
            this.txtCentroTrabajo.Properties.ReadOnly = estado;
            this.txtValorAporteARP.Properties.ReadOnly = estado;
            this.uc_CajaCompensacion.EnableControl(!estado);
            this.txtDiasCotizacionCC.Properties.ReadOnly = estado;
            this.txtIngresoBaseCC.Properties.ReadOnly = estado;
            this.txtTarifaCC.Properties.ReadOnly = estado;
            this.txtValorAporteCC.Properties.ReadOnly = estado;
            this.txtTarifaICBF.Properties.ReadOnly = estado;
            this.txtValorICBF.Properties.ReadOnly = estado;
            this.txtTarifaSENA.Properties.ReadOnly = estado;
            this.txtValorSENA.Properties.ReadOnly = estado;

        }
        
        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa el formulario
        /// </summary>
        protected override void SetInitParameters()
        {
            InitializeComponent();
            this.documentID = AppDocuments.PlanillaAutoLiquidAportes;
            //Inicia las variables del formulario
            this.userID = _bc.AdministrationModel.User.ReplicaID.Value.Value;
            this.areaFuncionalID = _bc.AdministrationModel.User.AreaFuncionalID.Value;
            //Asigna las propiedades al documento
            this.frmModule = ModulesPrefix.no;

            base.SetInitParameters();            
            this.InitControls();
        }

        /// <summary>
        /// Se ejecuta despues de inicializar 
        /// </summary>
        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            //this.lEmpleados = this._bc.AdministrationModel.Nomina_noEmpleadoGet(AppDocuments.Nomina, this.dtPeriod.DateTime, (byte)EstadoDocControl.Aprobado, true, (byte)EstadoEmpleado.Activo);
            this.uc_Empleados.IsMultipleSeleccion = false;
            this.uc_Empleados.Init();
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            this.LoadData(false);  
        }

        /// <summary>
        /// Evento que carga la información del objeto base
        /// </summary>
        /// <param name="firstTime">indica si se realiza al cargar el formulario</param>
        protected override void LoadData(bool firstTime)
        {
            if (!firstTime)
            {
                if (this.uc_Empleados.empleadoActivo != null)
                {
                    this._planillaAportes = this._lplanillasTemp.Where(x => x.EmpresaID.Value == this.empresaID && x.EmpleadoID.Value == this.uc_Empleados.empleadoActivo.ID.Value).FirstOrDefault();
                    if (this._planillaAportes == null)
                    {
                        this._planillaAportes = this._bc.AdministrationModel.Nomina_GetPlanillaAportes(this.uc_Empleados.empleadoActivo.ID.Value, this.dtPeriod.DateTime);
                        if (this.EditRegMode)
                            this._lplanillasTemp.Add(this._planillaAportes);
                    }
                    this.LoadInfoControls();
                }
            }
        }     

        #endregion

        #region Eventos Controles

        /// <summary>
        /// Boton Liquidar Planilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLiquidar_Click(object sender, EventArgs e)
        {
            Thread process = new Thread(this.SaveThread);
            process.Start();
        }

        /// <summary>
        /// Boton Procesar Planilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcesar_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// Boton Archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportTxt_Click(object sender, EventArgs e)
        {
            try
            {
                List<DTO_noPlanillaAportesDeta> planillas = new List<DTO_noPlanillaAportesDeta>();
                string result = string.Empty;
                foreach (var item in this.uc_Empleados._empleados)
                {
                    var plan = this._bc.AdministrationModel.Nomina_GetPlanillaAportes(item.ID.Value, this.dtPeriod.DateTime);
                    if (plan != null) planillas.Add(plan);
                }

                if (planillas.Count > 0)
                {
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "CSV (.csv)|*.csv|Text File (.txt)|*.txt";
                        if (saveDialog.ShowDialog() != DialogResult.Cancel)
                            this._bc.AdministrationModel.Reportes_No_GenerarArchivoPlanilla(saveDialog.FileName, planillas);
                    }
                }
                else
                    MessageBox.Show("No hay planillas para exportar del periodo actual");
            }
            catch (Exception ex)
            {
               MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionPlanillaAportes.cs", "btnExportTxt_Click"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValor_Leave(object sender, EventArgs e)
        {
            try
            {
                TextEdit txt = (TextEdit)sender;
                if (this._planillaAportes != null)
                {
                    if (txt.Name == "txtVlrTrabajadorSLD")
                    {
                        this._planillaAportes.VlrTrabajadorSLD.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                    else if (txt.Name == "txtVlrEmpresaSLD")
                    {
                        this._planillaAportes.VlrEmpresaSLD.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                    else if (txt.Name == "txtVlrTrabajadorPEN")
                    {
                        this._planillaAportes.VlrTrabajadorPEN.Value = Convert.ToDecimal(txt.EditValue,CultureInfo.InvariantCulture);
                    }
                    else if (txt.Name == "txtVlrEmpresaPEN")
                    {
                        this._planillaAportes.VlrEmpresaPEN.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                    else if (txt.Name == "txtValorAporteARP")
                    {
                        this._planillaAportes.VlrARP.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                    else if (txt.Name == "txtValorAporteCC")
                    {
                        this._planillaAportes.VlrCCF.Value = Convert.ToDecimal(txt.EditValue, CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Eventos MDI

        /// <summary>
        /// Se ejecuta cuando la forma esta activa sobre el resto
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void Form_Enter(object sender, EventArgs e)
        {
            base.Form_Enter(sender, e);

            FormProvider.Master.tbBreak.Visible = false;
            FormProvider.Master.itemFilter.Visible = false;
            FormProvider.Master.itemFilterDef.Visible = false;
            FormProvider.Master.tbBreak0.Visible = false;
            FormProvider.Master.itemGenerateTemplate.Visible = false;
            FormProvider.Master.itemCopy.Visible = false;
            FormProvider.Master.itemPaste.Visible = false;
            FormProvider.Master.itemExport.Visible = false;
            FormProvider.Master.tbBreak1.Visible = false;
            FormProvider.Master.itemImport.Visible = false;
            FormProvider.Master.itemRevert.Visible = false;
            FormProvider.Master.itemUpdate.Visible = false;
            FormProvider.Master.itemSendtoAppr.Visible = false;
            FormProvider.Master.itemNew.Visible = false;
            FormProvider.Master.itemDelete.Visible = false;
            FormProvider.Master.itemUpdate.Enabled = false;
            FormProvider.Master.itemEdit.Visible = true;
            if (FormProvider.Master.LoadFormTB)
            {
                FormProvider.Master.itemPrint.Enabled = true;
                FormProvider.Master.itemEdit.Enabled = true;
                FormProvider.Master.itemSave.Enabled = false;
            }
            else
            {
                FormProvider.Master.itemUpdate.Enabled = true;
            }
        }      

        #endregion

        #region Eventos Control Empleados
        
        /// <summary>
        /// Evento ejecutado al seleccionar el empleado de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
            if (this._planillaAportes != null)
            {   
                if (this._lplanillasTemp.Exists(x => x.EmpleadoID.Value == this._planillaAportes.EmpleadoID.Value))
                   this.LoadObject();               

                //if (this._lplanillasTemp.Exists(x => x.EmpleadoID.Value == this.uc_Empleados.empleadoActivo.ID.Value))
                //{
                //    this.EditModeControls(false);
                //    this.EditRegMode = true;
                //    FormProvider.Master.itemSave.Enabled = true;
                //}

                //if (this.EditRegMode)
                //{
                //    if (MessageBox.Show(DictionaryMessages.No_EmpleadoEditPlanillaOK, DictionaryMessages.No_EmpleadoEditPlanilla, MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    {
                //        this.LoadObject();
                //        if (!this._lplanillasTemp.Contains(this._planillaAportes))
                //            this._lplanillasTemp.Add(this._planillaAportes);
                //        else
                //        {
                //            this._lplanillasTemp.Remove(this._planillaAportes);
                //            this._lplanillasTemp.Add(this._planillaAportes);
                //        }
                //        FormProvider.Master.itemSave.Enabled = true;
                //    }
                //    this.EditRegMode = false;
                //    this.EditModeControls(true);
                //}
            }
            this.LoadData(false);            
        }

        #endregion

        #region Eventos Barra de Herramientas

        /// <summary>
        /// Boton para actualizar la lista de documentos
        /// </summary>
        public override void TBSave()
        {
            //agrega las novedades al sistema
            var result = this._bc.AdministrationModel.Nomina_PlanillaAportesDeta_Upd(this._lplanillasTemp);
            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();   
        }

        /// <summary>
        /// Click del boton Editar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void TBEdit()
        {
            this.EditModeControls(false);
            this.EditRegMode = true;
            FormProvider.Master.itemSave.Enabled = true;
            if (!this._lplanillasTemp.Contains(this._planillaAportes))
                this._lplanillasTemp.Add(this._planillaAportes);
            else
            {
                this._lplanillasTemp.Remove(this._planillaAportes);
                this._lplanillasTemp.Add(this._planillaAportes);
            }
            FormProvider.Master.itemSave.Enabled = true;
        }

        //public override void tbed

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de salvar
        /// </summary>
        public override void SaveThread()
        {
            try
            {
                this.Invoke(this.saveDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-LiquidacionPlanillaAportes.cs", "SaveThread"));
            }                       

        }


        #endregion
    }
}
