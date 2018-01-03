using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_GastosEmpresa : ReportBase
    {
        public Report_No_GastosEmpresa()
        {
            
        }

        public Report_No_GastosEmpresa(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_gastosEmpresa");
            this.lbl_Desde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_Desde");
            this.lbl_Hasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lbl_Hasta");
            this.lbl_TotalFondo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lblTotalFondo");
            this.lbTAportes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lblTotalAportes");
            this.lbl_Total_Privisiones.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lblTotalProvisiones");
            this.lbl_Nomina.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35113_lblTotalNomina");
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

            this.DataSourceNomina.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceNomina.Queries[0].Parameters[1].Value = Convert.ToDateTime(fechaIni);
            this.DataSourceNomina.Queries[0].Parameters[2].Value = Convert.ToDateTime(fechaFin);
            this.DataSourceNomina.Queries[0].Parameters[3].Value = Tercero.ToString();

            this.DataSourceAportes.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceAportes.Queries[0].Parameters[1].Value = Convert.ToDateTime(fechaIni);
            this.DataSourceAportes.Queries[0].Parameters[2].Value = Convert.ToDateTime(fechaFin);
            this.DataSourceAportes.Queries[0].Parameters[3].Value = Tercero.ToString();

            this.DataSourceProvisiones.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceProvisiones.Queries[0].Parameters[1].Value = Convert.ToDateTime(fechaIni);
            this.DataSourceProvisiones.Queries[0].Parameters[2].Value = Convert.ToDateTime(fechaFin);
            this.DataSourceProvisiones.Queries[0].Parameters[3].Value = Tercero.ToString();

            this.DataSourceTotales.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceTotales.Queries[0].Parameters[1].Value = Convert.ToDateTime(fechaIni);
            this.DataSourceTotales.Queries[0].Parameters[2].Value = Convert.ToDateTime(fechaFin);
            this.DataSourceTotales.Queries[0].Parameters[3].Value = Tercero.ToString();

            base.ConfigureConnection(this.DataSourceNomina);
            base.ConfigureConnection(this.DataSourceAportes);
            base.ConfigureConnection(this.DataSourceProvisiones);
            base.ConfigureConnection(this.DataSourceTotales);
            this.CreateReport();

            return this.ReportName;
        }
    }
}
