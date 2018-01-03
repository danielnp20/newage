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
    public partial class Reports_Cc_Amortizacion : ReportBaseLandScape
    {
        #region Variables
        #endregion
        public Reports_Cc_Amortizacion()
        {
            
        }

        public Reports_Cc_Amortizacion(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, AppReports.ccAmortizacion + "_Amortizacion");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string cliente, string obligacion)
        {          
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                
                if (!string.IsNullOrEmpty(cliente.ToString())) // Cliente
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35108_Cliente") + " " + cliente + "\t ";
                if (!string.IsNullOrEmpty(obligacion.ToString())) // Obligacion
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35108_Obligacion") + " " + obligacion + "\t ";
                this.lblFilter.Text = filter;
                #endregion

                this.BDAmortizacion.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.BDAmortizacion.Queries[0].Parameters[1].Value = string.IsNullOrEmpty (cliente) ? null : cliente;
                this.BDAmortizacion.Queries[0].Parameters[2].Value = string.IsNullOrEmpty (obligacion) ? null : obligacion;

                base.ConfigureConnection(this.BDAmortizacion);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
