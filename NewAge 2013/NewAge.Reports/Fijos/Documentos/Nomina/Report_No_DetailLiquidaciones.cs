using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_No_DetailLiquidaciones : ReportBase
    {
        /// <summary>
        /// Constructor x Defecto
        /// </summary>
        public Report_No_DetailLiquidaciones()
        {
            
        }

        public Report_No_DetailLiquidaciones(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
            this.lblReportName.Visible = false;



            switch (documentoID)
            {
                #region Nombre Reporte
                case 81:
                    this.lbl_ReportNombre.Text = "Nómina";
                    break;
                case 82:
                    this.lbl_ReportNombre.Text = "Vacaciones";
                    break;
                case 83:
                    this.lbl_ReportNombre.Text = "Prima";
                    break;
                case 84:
                    this.lbl_ReportNombre.Text = "Liquidación";
                    break;
                case 85:
                    this.lbl_ReportNombre.Text = "Cesantías";
                    break;
                case 86:
                    this.lbl_ReportNombre.Text = "Provisiones";
                    break;
                case 87:
                    this.lbl_ReportNombre.Text = "Planilla";
                    break; 
                #endregion
            }
            if (isPre)
                this.lbl_ReportNombre.Text = "Prenómina";
            
            ModuloNomina _moduloNomina = new ModuloNomina(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_ReportNominaInfoEmpleado> data = _moduloNomina.Report_No_DetailLiquidaciones(documentoID, periodo, orden, fechaini, fechaFin, isAll, isOrderByName, isPre,terceroid,operacionnoid,areafuncionalid,conceptonoid);

            //if (data != null && data.Count > 0)
            //{
                this.DataSource = data;
                this.CreateReport();
            //}
            //else
            //    this.ReportName = string.Empty;

            return this.ReportName;
        }
    }
}
