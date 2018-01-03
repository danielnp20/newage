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
    public partial class Report_Co_AuxiliarxTerceroMultimoneda : ReportBase
    {
        public Report_Co_AuxiliarxTerceroMultimoneda()
        {

        }

        public Report_Co_AuxiliarxTerceroMultimoneda(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Auxiliar");
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Mes");
            this.xrLblLibro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Libro");
            //this.xrLblCuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Cuenta");
            this.xrLblNroRegistros.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_Nroregistros");
            this.xrLblSaldoInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoInicial");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvoCuenta");
            //this.xrLblSaldoFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoFinalCuenta");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaInicial, DateTime fechaFinal, string libro, string cuentaInicial, string cuentaFin, string tercero, string proyecto, string centroCosto,
            string lineaPresupuestal)
        {
            this.lblReportName.Visible = false;

            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportLibroDiarioTotales> data = _moduloContabilidad.ReportesContabilidad_AuxiliarxTercero( fechaInicial,  fechaFinal, libro, cuentaInicial, cuentaFin, tercero,
                 proyecto, centroCosto, lineaPresupuestal);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
