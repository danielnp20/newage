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
    public partial class Report_Cc_EstadoClientes : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_Cc_EstadoClientes()
        {

        }

        public Report_Cc_EstadoClientes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_EstadoDeCuenta");
            //this.lbl_NombreReporte.Text.ToUpper();
            //this.lbl_FechaIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_FechaIni");
            //this.lbl_FechaFin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_FechaFin");
            //this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            //this.lbl_Subtotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotal");
            //this.lbl_SaldoIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35066_lbl_SaldoIni");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        /// 
        public string GenerateReport( string cliente, string obligacion,byte tipoEstado)
        {
            this.lblReportName.Visible = false;

            this.SQL.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.SQL.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(cliente) ? cliente : null;
            this.SQL.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(obligacion) ? obligacion : null;
            this.SQL.Queries[0].Parameters[3].Value = tipoEstado;

            base.ConfigureConnection(this.SQL);
            this.CreateReport();

            return this.ReportName;
        }
    }
}
