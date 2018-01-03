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

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_Aportes_a_Clientes : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_Cc_Aportes_a_Clientes()
        {

        }

        public Report_Cc_Aportes_a_Clientes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_AportesCliente"); // lblReportName
            this.lbl_Nombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Nombre");
            this.lbl_Cedula.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Cedula");
            this.lbl_saldo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Saldo");
            this.lblReportName.Text.ToUpper();
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");//
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int Año, int Mes,string _tercero, ExportFormatType formatType)
        {
            this.DataSource_Aporte_a_Cliente.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSource_Aporte_a_Cliente.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(Año.ToString()) ? Año.ToString() : null;
            this.DataSource_Aporte_a_Cliente.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(Mes.ToString()) ? Mes.ToString() : null;
            this.DataSource_Aporte_a_Cliente.Queries[0].Parameters[3].Value = _tercero.ToString();

            base.ConfigureConnection (this.DataSource_Aporte_a_Cliente);
            this.CreateReport();
            return this.ReportName;
        }
    }
}
