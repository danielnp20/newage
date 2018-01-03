using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_AportesCajaCompensacion : ReportBase
    {
        public Report_No_AportesCajaCompensacion()
        {
            
        }

        public Report_No_AportesCajaCompensacion(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_AportesCajaCompensacion");
            this.lbl_Desde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_Desde");
            this.lbl_Hasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_Hasta");
            this.lbl_Caja.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_Caja");
            this.lbl_TotalFondo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lblTotalFondo");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string Tercero)  
        {
            this.lblReportName.Visible = false;
            this.lblFechaIni.Text = fechaIni.ToString();
            this.lblFechaFin.Text = fechaFin.ToString();

            this.DataSourceCajaCompensacion.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceCajaCompensacion.Queries[0].Parameters[1].Value = Tercero.ToString();
            this.DataSourceCajaCompensacion.Queries[0].Parameters[2].Value = Convert.ToDateTime(fechaIni); 
            this.DataSourceCajaCompensacion.Queries[0].Parameters[3].Value = Convert.ToDateTime(fechaFin);


            base.ConfigureConnection(this.DataSourceCajaCompensacion);
            this.CreateReport();

            return this.ReportName;
        }
    }
}
