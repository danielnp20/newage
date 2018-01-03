using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_No_ProvisionesSaldos : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion

        public Report_No_ProvisionesSaldos()
        {

        }

        public Report_No_ProvisionesSaldos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35120_lbl_SaldosProvisiones");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string empleado)
        {
            try
            {
                this.lblPeriodo.Text = periodo.ToString("MMMM / yyyy").ToUpper();

                DTO_noComponenteNomina dtoComponen;
                DTO_noConceptoNOM dtoConcepto;
                string ctaVacacionesConsol = string.Empty;
                string ctaPrimaConsol = string.Empty;
                string ctaCesantiasConsol = string.Empty;
                string ctaIntCesantiasConsol = string.Empty;

                string ctaVacacionesProvis = string.Empty;
                string ctaPrimaProvis = string.Empty;
                string ctaCesantiasProvis = string.Empty;
                string ctaIntCesantiasProvis = string.Empty;

                #region Trae las cuentas consolidadas
                string compoVacaciones = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CompVacacionesConsolidadas);
                dtoComponen = (DTO_noComponenteNomina)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noComponenteNomina, compoVacaciones, true, false);
                ctaVacacionesConsol = dtoComponen != null ? dtoComponen.CuentaID.Value : string.Empty;

                string compoPrima = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CompPrimadeServiciosConsolidadas);
                dtoComponen = (DTO_noComponenteNomina)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noComponenteNomina, compoPrima, true, false);
                ctaPrimaConsol = dtoComponen != null ? dtoComponen.CuentaID.Value : string.Empty;

                string compoCesantias = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CompCesantiasConsolidadas);
                dtoComponen = (DTO_noComponenteNomina)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noComponenteNomina, compoCesantias, true, false);
                ctaCesantiasConsol = dtoComponen != null ? dtoComponen.CuentaID.Value : string.Empty;

                string compoIntCesantias = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_CompInteCesantiasLiquidadas);
                dtoComponen = (DTO_noComponenteNomina)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noComponenteNomina, compoIntCesantias, true, false);
                ctaIntCesantiasConsol = dtoComponen != null ? dtoComponen.CuentaID.Value : string.Empty;
                
                #endregion
                #region Trae las cuentas provision
                string concepVacaciones = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoVacacionesTiempo);
                dtoConcepto = (DTO_noConceptoNOM)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, concepVacaciones, true, false);
                ctaVacacionesProvis = dtoConcepto != null ? dtoConcepto.CuentaID.Value : string.Empty;

                string concepPrima = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoPrimaServicios);
                dtoConcepto = (DTO_noConceptoNOM)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, concepPrima, true, false);
                ctaPrimaProvis = dtoConcepto != null ? dtoConcepto.CuentaID.Value : string.Empty;

                string concepCesantias = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoCesantias);
                dtoConcepto = (DTO_noConceptoNOM)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, concepCesantias, true, false);
                ctaCesantiasProvis = dtoConcepto != null ? dtoConcepto.CuentaID.Value : string.Empty;

                string concepIntCesantias = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.no, AppControl.no_ConceptoInteresCesantias);
                dtoConcepto = (DTO_noConceptoNOM)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noConceptoNOM, concepIntCesantias, true, false);
                ctaIntCesantiasProvis = dtoConcepto != null ? dtoConcepto.CuentaID.Value : string.Empty;
                
                #endregion    

                //Valida que todas las cuentas existan
                if (string.IsNullOrEmpty(ctaVacacionesConsol) || string.IsNullOrEmpty(ctaVacacionesProvis) || string.IsNullOrEmpty(ctaPrimaConsol) || string.IsNullOrEmpty(ctaPrimaProvis) ||
                    string.IsNullOrEmpty(ctaCesantiasConsol) || string.IsNullOrEmpty(ctaCesantiasProvis) || string.IsNullOrEmpty(ctaIntCesantiasConsol) || string.IsNullOrEmpty(ctaIntCesantiasProvis))
                    return string.Empty;

                this.DataSourceQueries.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourceQueries.Queries[0].Parameters[1].Value = periodo;
                this.DataSourceQueries.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(empleado) ? empleado : null;
                this.DataSourceQueries.Queries[0].Parameters[3].Value = ctaVacacionesConsol;
                this.DataSourceQueries.Queries[0].Parameters[4].Value = ctaVacacionesProvis;
                this.DataSourceQueries.Queries[0].Parameters[5].Value = ctaPrimaConsol;
                this.DataSourceQueries.Queries[0].Parameters[6].Value = ctaPrimaProvis;
                this.DataSourceQueries.Queries[0].Parameters[7].Value = ctaCesantiasConsol;
                this.DataSourceQueries.Queries[0].Parameters[8].Value = ctaCesantiasProvis;
                this.DataSourceQueries.Queries[0].Parameters[9].Value = ctaIntCesantiasConsol;
                this.DataSourceQueries.Queries[0].Parameters[10].Value = ctaIntCesantiasProvis;              

                this.ConfigureConnection(this.DataSourceQueries);

                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
