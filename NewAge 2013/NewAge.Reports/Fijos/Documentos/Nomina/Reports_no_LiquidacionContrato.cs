using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Reports
{
    public partial class Reports_no_LiquidacionContrato : ReportBase
    {
        #region Variables

        private string mdaLocal = string.Empty;    
        
        #endregion

        public Reports_no_LiquidacionContrato()
        {
            
        } 

        public Reports_no_LiquidacionContrato(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _empleadoID, int _vacaciones, string fechaFiltro) 
        {
            try
            {
                DateTime? fechaIni = null;
                // Convierte Valor en Letras
                this.mdaLocal = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                // Nombre del reporte padre invisible
                this.lblReportName.Visible = false;

                string terceroEmpresa = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                this.lblNitFirma.Text = terceroEmpresa;
                this.lblNitComent.Text = terceroEmpresa;

                if (!string.IsNullOrEmpty(fechaFiltro))
                    fechaIni = Convert.ToDateTime(fechaFiltro);
                
                this.DataSourceLiquidaContrato.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourceLiquidaContrato.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(_empleadoID.ToString()) ? _empleadoID.ToString() : string.Empty;
                this.DataSourceLiquidaContrato.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(_vacaciones.ToString()) ? _vacaciones.ToString() : null;
                this.DataSourceLiquidaContrato.Queries[0].Parameters[3].Value = fechaIni;

                this.ConfigureConnection(this.DataSourceLiquidaContrato);

                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                throw;
            }
        }

        /// <summary>
        /// Valor en Letra para el documento de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_vlrTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel vlrTot1 = FindControl("lbl_vlrTotal", true) as XRLabel;
            XRLabel vlrTot2 = FindControl("lbl_vlrTotal", true) as XRLabel;
            XRLabel vlrTot3 = FindControl("lbl_vlrTotal", true) as XRLabel;
            string favor = string.Empty;

            if (!string.IsNullOrEmpty(vlrTot1.Text))
            {
                decimal tot1 = Convert.ToDecimal(vlrTot1.Text);
                decimal tot2 = Convert.ToDecimal(vlrTot2.Text);
                decimal tot3 = Convert.ToDecimal(vlrTot3.Text);

                //if(tot1 < 0 || tot2 < 0 || tot3  < 0)
                //{
                //    tot1 = tot1 * -1;
                //    tot2 = tot2 * -1;
                //    tot3 = tot3 * -1;
                //    favor = "a favor de la empresa.";
                //}
                //else
                //{
                //    favor = "a favor del empleado.";
                //}

                this.lbl_vlrLetra.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, tot2);
                //this.lbl_vlrLetraOculto.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, tot1);
                //this.lbl_vlrLetraOculto2.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, tot3);
                this.lbl_favor.Text = favor;
            }
            this.lbl_vlrLetraOculto.Visible = false;
            this.lbl_vlrLetraOculto2.Visible = false;
        }
    }
}
