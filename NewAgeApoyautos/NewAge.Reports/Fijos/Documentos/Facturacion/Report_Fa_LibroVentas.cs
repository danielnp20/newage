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
using NewAge.DTO.Reportes;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Fa_LibroVentas : ReportBase
    {
        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_Fa_LibroVentas()
        {

        }

        /// <summary>
        /// Contructor para coneccion de datos
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Fa_LibroVentas(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35099_Año");
            this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35099_Desde");
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35099_LibroDeVentas");
            this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35099_Hasta");
            this.xrLabel2.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35099_TotalxDia");

        }

        /// <summary>
        /// Inicializa los el repporte
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, int diaFinal, string cliente, string prefijo, string NroFactura)
        {
            this.lblReportName.Visible = false;

            ModuloFacturacion _moduloFacturacion = new ModuloFacturacion(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_FacturacionTotales> data = _moduloFacturacion.ReportesFacturacion_LibroVentas(periodo, diaFinal, cliente, prefijo, NroFactura);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
