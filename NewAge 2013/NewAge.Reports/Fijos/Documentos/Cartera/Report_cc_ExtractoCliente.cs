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
    public partial class Report_cc_ExtractoCliente : ReportBase
    {
        #region Variables
        decimal valorLibranza=0;

        #endregion
        public Report_cc_ExtractoCliente()
        {
            
        }

        public Report_cc_ExtractoCliente(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Pagaduria Nomina");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int? libranza,  string cliente)
        {
            try
            {
                #region Asigna Info Header
               // this.lblPEriodo.Text = fechaFin.ToString(FormatString.Period);
                #endregion

                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = libranza;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(cliente)? cliente : null;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        


        private void lblValorSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel abono = FindControl("lblValorAbono", true) as XRLabel;
            XRLabel otros = FindControl("lblValorOtro", true) as XRLabel;

            decimal vlrAbono = 0;
            decimal vlrotros = 0;

            string textAbono = abono.Text.Replace("$", "");
            if (!string.IsNullOrEmpty(textAbono))
            {
                 vlrAbono = Convert.ToDecimal(textAbono);                
            }

            string textOtros = otros.Text.Replace("$", "");
            if (!string.IsNullOrEmpty(textOtros))
            {
                
                vlrotros = Convert.ToDecimal(textOtros);           
            }
            valorLibranza = valorLibranza + vlrAbono - vlrotros;
            this.lblValorSaldo.Text = "$" + valorLibranza.ToString("n0");
        }


        

        private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {


            XRLabel Libranza = FindControl("lblVlrLibranza", true) as XRLabel;
            if (!string.IsNullOrEmpty(Libranza.Text))
            {
                string textLibranza = Libranza.Text.Replace("$", "");
                decimal vlrLib = Convert.ToDecimal(textLibranza);
                valorLibranza = vlrLib;
            }
        }

        
        

        
    }
}
