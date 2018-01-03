using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Cc_PagaduriaIncoporaciones : ReportBase
    {
         public Report_Cc_PagaduriaIncoporaciones()
        {

        }

         public Report_Cc_PagaduriaIncoporaciones(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_AnexoIncorporacion");
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_Año");
            this.lblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_Desde");
            this.lblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_Hasta");
            this.lblPagaduria.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_Pagaduria");
            this.lblNombrePagaduria.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35101_NombrePagaduria");
        }

        protected override void SetInitParameters()     
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime FechaInicial, DateTime FechaFinal, string Pagaduria)
        {
            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_Cc_PagaduriaIncorporacion(FechaInicial, FechaFinal, Pagaduria);
            if (data.Count > 0)
            {
                data[0].PeriodoIncial = FechaInicial;
                data[0].PeriodoFinal = FechaFinal;
            }
            else
            {
                DTO_CarteraTotales sinDatos = new DTO_CarteraTotales();
                sinDatos.PeriodoIncial = FechaInicial;
                sinDatos.PeriodoFinal = FechaFinal;
            }

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
