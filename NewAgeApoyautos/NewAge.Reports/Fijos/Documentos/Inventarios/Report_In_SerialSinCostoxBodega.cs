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
using NewAge.DTO.UDT;

namespace NewAge.Reports.Fijos
{
    public partial class Report_In_SerialSinCostosxBodega : ReportBase
    {
        public Report_In_SerialSinCostosxBodega()
        {

        }

        public Report_In_SerialSinCostosxBodega(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Año");
            this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Mes");
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35080_Seriales");
            this.xrLblBodega.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Bodega");
            this.xrLblTotalSerial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35080_TotalSerial");
            this.xrlblReferencia.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35080_Referencia");
            
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mesIni, string bodega, string tipoBodega,
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial)
        {
            this.lblReportName.Visible = false;

            ModuloInventarios _moduloInventarios = new ModuloInventarios(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportInventariosSaldosTotal> data = _moduloInventarios.ReportesInventarios_SerialSinCostos(año, mesIni, bodega, tipoBodega, referencia,
                grupo, clase, tipo, serie, material, isSerial);
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
