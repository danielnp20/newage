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
    public partial class Report_In_SaldosResumido : ReportBase
    {
        public Report_In_SaldosResumido()
        {

        }

        public Report_In_SaldosResumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Año");
            this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Mes");
            this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_SaldosResumen");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_GranTotal");
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
                    string referencia, string grupo, string clase, string tipo, string serie, string material, bool isSerial, string Libro)
        {
            try
            {
            this.lblReportName.Visible = false;

                #region Asigna Filtros
                this.sqlDataSource1.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.sqlDataSource1.Queries[0].Parameters[1].Value = año;
                this.sqlDataSource1.Queries[0].Parameters[2].Value = mesIni;
                #endregion        

                base.ConfigureConnection(this.sqlDataSource1);
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
