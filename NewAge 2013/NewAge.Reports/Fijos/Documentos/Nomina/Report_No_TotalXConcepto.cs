using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_TotalXConcepto : ReportBase
    {
         public Report_No_TotalXConcepto()
        {
            
        }

         public Report_No_TotalXConcepto(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool orderByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            switch (documentoID)
            {
                case 81:
                    this.lbl_ReporteNombre.Text = "Nómina";
                    break;
                case 82:
                    this.lbl_ReporteNombre.Text = "Vacaciones";
                    break;
                case 83:
                    this.lbl_ReporteNombre.Text = "Prima";
                    break;
                case 84:
                    this.lbl_ReporteNombre.Text = "Liquidación";
                    break;
                case 85:
                    this.lbl_ReporteNombre.Text = "Cesantías";
                    break;
                case 86:
                    this.lbl_ReporteNombre.Text = "Provisiones";
                    break;
                case 87:
                    this.lbl_ReporteNombre.Text = "Planilla";
                    break;
            }
            this.lblReportName.Visible = false;
            ModuloNomina _moduloNomina = new ModuloNomina(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_FormulariosResumidoDevengosDedTotal> dataTotal = new List<DTO_FormulariosResumidoDevengosDedTotal>();
            DTO_FormulariosResumidoDevengosDedTotal data = _moduloNomina.Report_No_TotalXConcepto(documentoID, periodo, orden, fechaini, fechaFin, isAll, orderByName, isPre,terceroid,operacionnoid,areafuncionalid,conceptonoid);
            
            dataTotal.Add(data);
            this.DataSource = dataTotal;

            this.CreateReport();
            return this.ReportName;
        }
    }
}
