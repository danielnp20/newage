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
    public partial class Report_No_CesantiasDocumento : ReportBase
    {
        //Valor
        private string mdaLocal = string.Empty;

        public Report_No_CesantiasDocumento()
        {
            
        } 

        public Report_No_CesantiasDocumento(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _empleadoID, int docID, string fechaFiltro) 
        {
            try
            {
                DateTime? fechaIni = null;
                // Convierte Valor en Letras
                this.mdaLocal = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                // Nombre del reporte padre invisible
                this.lblReportName.Visible = false;

                if (!string.IsNullOrEmpty(fechaFiltro))
                    fechaIni = Convert.ToDateTime(fechaFiltro);

                string terceroEmpresa = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                this.lblNitFirma.Text = terceroEmpresa;

                this.DataSourceDocVacaciones2.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(_empleadoID.ToString()) ? _empleadoID.ToString() : null;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(docID.ToString()) ? docID.ToString() : null;
                this.DataSourceDocVacaciones2.Queries[0].Parameters[3].Value = fechaIni;

                this.ConfigureConnection(this.DataSourceDocVacaciones2);

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
            XRLabel vlrTot1 = FindControl("lblTotal", true) as XRLabel;
            XRLabel vlrTot2 = FindControl("lblTotal", true) as XRLabel;

            if (!string.IsNullOrEmpty(vlrTot1.Text))
            {
                decimal tot1 = Convert.ToDecimal(vlrTot1.Text);
                decimal tot2 = Convert.ToDecimal(vlrTot2.Text);

                this.lbl_vlrLetra.Text = CurrencyFormater.GetCurrencyString("ES", mdaLocal, tot2);
            }
        }
    }
}
