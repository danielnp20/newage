﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_Cp_CuentasXCobrarMulti : ReportBaseLandScape
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_CuentasXCobrarMulti()
        {

        }

        public Report_Cp_CuentasXCobrarMulti(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35007_lbl_CuentasXCobrar");
            this.xrLblSubTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35007_lbl_SubTotal");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35007_lbl_Total");
            this.xrLblPeriodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35007_lbl_Periodo");
            this.xrLblTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35007_lbl_Tercero");
            //this.lbl_NombreReporte.Text.ToUpper();
            //this.lbl_Mes.Text.ToUpper();
            //this.lbl_Empresa2.Text = this.Empresa.Descriptivo.Value;
            //this.lbl_Empresa2.Text.ToUpper();
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fecha,  string Tercero, int Moneda, string Cuenta)
        {
            this.xrLabel1.Text = fecha.ToString().Substring(0, 10);

            ModuloFacturacion _moduloFa = new ModuloFacturacion(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_FacturacionTotales> data = _moduloFa.Report_CuentasXCobrar(Tercero, Moneda, Cuenta, fecha);

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

        private void xrTableCell25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
