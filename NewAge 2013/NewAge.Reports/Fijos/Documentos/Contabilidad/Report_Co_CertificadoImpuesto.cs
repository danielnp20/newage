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
using System.Linq;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Co_CertificadoImpuesto : ReportBase
    {
        #region Variables
        #endregion
        public Report_Co_CertificadoImpuesto()
        {
            
        }

        public Report_Co_CertificadoImpuesto(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)  {}

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte tipoRep, DateTime fechaIni, DateTime fechaFin, string tercero)
        {
            try
            {
                #region Valida Impuestos
                string impuesto1 = string.Empty, impuesto2 = string.Empty;
                if (tipoRep == 1) // Retefuente
                {
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "CERTIFICADO DE RETENCION EN LA FUENTE");
                    impuesto1 = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteFuente);
                }
                else if (tipoRep == 2) // IVA
                {
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "CERTIFICADO DE RETENCION DE IVA");
                    impuesto1 = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteIVA);
                    impuesto2 = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                }
                else if (tipoRep == 3) // ICA
                {
                      this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "CERTIFICADO DE RETENCION DE ICA");
                      impuesto1 = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoReteICA);
                }
                else if (tipoRep == 4) // Imp Consumo
                {
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "CERTIFICADO DE IMPUESTO AL CONSUMO");
                    impuesto1 = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoTipoImpuestoConsumo);
                }
                
                #endregion
                string tipoLibro = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional).ToString();

                string terceroEmpresa = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                string nitDian = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_NitDIAN);
                DTO_coTercero dtoTerEmpresa = (DTO_coTercero)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, terceroEmpresa, true, false);
                
                #region Asigna Info Header
                string filter = string.Empty;
                this.lblPeriodo.Text = fechaIni.ToString("MMMM - yyyy").ToUpper() + "     " + fechaFin.ToString("MMMM - yyyy").ToUpper();
                this.lblAño.Text = fechaIni.Year.ToString();
                
                if (dtoTerEmpresa != null)
	            {
                    this.lblNitEmpresa.Text = dtoTerEmpresa.ID.Value;
                    this.lblNombreEmpresa.Text = dtoTerEmpresa.Descriptivo.Value;
                    this.lblDireccion.Text = dtoTerEmpresa.Direccion.Value;		          
	            }
                this.lblFechaFooter.Text = DateTime.Now.Date.ToString("MMMM - yyyy").ToUpper();
                #endregion

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(nitDian) ? nitDian : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = fechaIni.Year;
                this.QueriesDataSource.Queries[0].Parameters[3].Value = fechaIni.Month;
                this.QueriesDataSource.Queries[0].Parameters[4].Value = fechaFin.Month;
                this.QueriesDataSource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(impuesto1) ? impuesto1 : null;
                this.QueriesDataSource.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(impuesto2) ? impuesto2 : null;
                this.QueriesDataSource.Queries[0].Parameters[7].Value = !string.IsNullOrEmpty(tercero) ? tercero : null;
                this.QueriesDataSource.Queries[0].Parameters[8].Value = !string.IsNullOrEmpty(tipoLibro) ? tipoLibro : null;

                #endregion        

                base.ConfigureConnection(this.QueriesDataSource);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// Antes de imprimir el reporte
        /// </summary>
        private void lblTotalLetras_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                string valor = this.lblTotalTercero.Text;

                if (!string.IsNullOrEmpty(valor))
                {
                    decimal v = Convert.ToDecimal(valor);
                    if (v >= 0)
                        this.lblTotalLetras.Text = "SON:   " + CurrencyFormater.GetCurrencyString("ES", "COP", v);
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
    }
}
