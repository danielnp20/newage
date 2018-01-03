using System;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using DevExpress.XtraReports.UI;

namespace NewAge.Reports
{
    public partial class Report_No_BoletaPago : ReportBase
    {
        public Report_No_BoletaPago()
        {         
        }

        public Report_No_BoletaPago(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType,int? numeroDoc = null)
            : base(loggerConn, c, tx, empresa, userId, formatType,numeroDoc) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();            
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string empleadoID, int _mes, int _año, string _documentoNomina, string _quincena)
        {
            #region Documento de la Nomina

            #region Tipo de Documento
            // Encabezado del reporte para indicar que tipo de documento se exige
            string DocNom = string.Empty;
            switch (_documentoNomina)
            {
                case "81":
                    DocNom = "Nómina";
                    break;

                case "82":
                    DocNom = "Vacaciones";
                    break;

                case "83":
                    DocNom = "Prima";
                    break;

                case "84":
                    DocNom = "Liquidación de Contrato";
                    break;
            } 
            #endregion

            this.lbReporteNomina.Text = DocNom.ToString();

            #endregion


            this.lblReportName.Visible = false;

            this.DetalleBoletaPago.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DetalleBoletaPago.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(empleadoID.ToString()) ? empleadoID.ToString() : null;
            this.DetalleBoletaPago.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(_mes.ToString()) ? _mes.ToString() : null;
            this.DetalleBoletaPago.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(_año.ToString()) ? _año.ToString() : null;
            this.DetalleBoletaPago.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(_documentoNomina.ToString()) ? _documentoNomina.ToString() : null;
            this.DetalleBoletaPago.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(_quincena.ToString()) ? _quincena.ToString() : null;

            this.ConfigureConnection(this.DetalleBoletaPago);
            this.CreateReport();            

            return this.ReportName;
        }

    }
}