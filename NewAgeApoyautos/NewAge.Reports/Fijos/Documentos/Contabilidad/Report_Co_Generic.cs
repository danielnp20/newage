using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_Generic : ReportBaseLandScape
    {
        public Report_Co_Generic()
        {

        }

        public Report_Co_Generic(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35095_ResumenSaldos");
            this.xrlblPeriodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35095_Periodo");
            this.xrLblCodLinea.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35095_CodLinea");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mesInicial, int mesFin, string libro, string cuenta,
            string tercero, string proyecto, string centroCosto, string lineaPresupuestal)
        {
            //this.lblReportName.Visible = false;

            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            //List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_Auxiliar(año, mesInicial, mesFin, libro,cuenta,tercero,
             //   proyecto,centroCosto,lineaPresupuestal);

            //this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
