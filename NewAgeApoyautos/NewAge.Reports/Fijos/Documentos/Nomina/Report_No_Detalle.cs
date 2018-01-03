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
    public partial class Report_No_Detalle : ReportBase
    {
        public Report_No_Detalle()
        {
            
        }

        public Report_No_Detalle(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte 
        /// </summary>
        public string GenerateReport(int documentoID, DateTime periodo, string orden, DateTime fechaini, DateTime fechaFin, bool isAll, bool isOrderByName, bool isPre, String terceroid, String operacionnoid, String areafuncionalid, String conceptonoid)
        {
            switch (documentoID)
            {
                case 81:
                    this.lbl_NombreReporte.Text = "Nómina";
                    break;
                case 82:
                    this.lbl_NombreReporte.Text = "Vacaciones";
                    break;
                case 83:
                    this.lbl_NombreReporte.Text = "Prima";
                    break;
                case 84:
                    this.lbl_NombreReporte.Text = "Liquidación";
                    break;
                case 85:
                    this.lbl_NombreReporte.Text = "Cesantías";
                    break;
                case 86:
                    this.lbl_NombreReporte.Text = "Provisiones";
                    break;
                case 87:
                    this.lbl_NombreReporte.Text = "Planilla";
                    break;
            }
            this.lblReportName.Visible = false;

            this.CreateReport();
            return this.ReportName;
        }

    }
}
