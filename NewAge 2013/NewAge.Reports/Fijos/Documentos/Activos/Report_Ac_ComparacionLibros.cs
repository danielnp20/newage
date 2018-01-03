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
    public partial class Report_Ac_ComparacionLibros : ReportBase
    {
         public Report_Ac_ComparacionLibros()
        {

        }

         public Report_Ac_ComparacionLibros(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            ////////this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Saldos");
            ////////this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Año");
            ////////this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Mes");
            ////////this.xrLblClase.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Clase");
            ////////this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_SubTotal");
            ////////this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35087_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mes, string clase, string tipo, string grupo, string centroCost, string logFis, string proyecto)
        {
            this.lblReportName.Visible = false;

            ModuloActivosFijos _moduloActivosFijos = new ModuloActivosFijos(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ActivosTotales> data = _moduloActivosFijos.ReportesActivos_ComparacionLibros(año, mes, clase, tipo, grupo, centroCost, logFis, proyecto);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
