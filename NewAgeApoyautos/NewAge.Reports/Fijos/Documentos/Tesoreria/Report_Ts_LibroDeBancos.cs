using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Tesoreria
{
    public partial class Report_Ts_LibroDeBancos : ReportBase
    {

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Report_Ts_LibroDeBancos()
        {
            
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        /// <param name="numDoc">Numero de documento para los pagos</param>
        public Report_Ts_LibroDeBancos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_FechaInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_FechaInicial");
            this.lbl_FechaFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_FechaFinal");
            this.lbl_Nombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_Nombre");
            this.lbl_NombreReport.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_LibroBancos");
            this.lbl_Cuenta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_Cuenta");
            this.lbl_SaldoIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_SaldoInicial");
            this.lbl_SaldoFin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35083_FechaFinal");

        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string filtro)
        {
             this.lblReportName.Visible = false;
            ModuloTesoreria _moduloTesoreria = new ModuloTesoreria(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_LibroBancos> data = _moduloTesoreria.Report_Ts_LibroBancos(fechaIni, fechaFin, filtro);
            for (int i = 0; i < data.Count; i++)
            {
                data[i].FechaIni.Value = fechaIni;
                data[i].FechaFin.Value = fechaFin;
            }
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
