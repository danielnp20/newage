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
    public partial class Report_Cc_Saldos : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_Saldos()
        {

        }

        public Report_Cc_Saldos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_SaldosCartera");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_LblFecha");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_LblTotalG");
            
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string cliente, string pagaduria, string lineaCredi, string compCartera, string asesor, string tipoCartera, bool isSaldoFavor)
        {
            try
            {
                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_CarteraTotales> totales = _moduloCartera.Report_Cc_Saldos(periodo, cliente, pagaduria, lineaCredi, compCartera, asesor, tipoCartera, isSaldoFavor);
                this.xrlFechaReportes.Text = periodo.ToString("MMMM' de 'yyyy");

                this.DataSource = totales;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, byte agrupamiento, byte romp, ExportFormatType formatType)
        {
            try
            {
                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                var result = _moduloCartera.Report_Cc_SaldosNuevo(tipoReporte, fechaIni, fechaFin, cliente, libranza, zonaID, ciudad, concesionario, asesor, lineaCredi, compCartera, agrupamiento, romp,formatType);
                this.xrlFechaReportes.Text = fechaFin.ToString("MMMM' de 'yyyy");

                List<DTO_SaldosReport> saldos =(List<DTO_SaldosReport>)result;

                this.DataSource = saldos;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
