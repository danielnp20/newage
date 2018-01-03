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
using System.Linq;


namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_EstadoCuentaFinan : ReportBaseLandScape
    {
        #region Variables

        #endregion
        public Report_Cc_EstadoCuentaFinan()
        {

        }

        public Report_Cc_EstadoCuentaFinan(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType,numDoc)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Estado Cuenta");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _nameProposito, int numDoc, DateTime fechaCorte, DateTime? fechaFNC, byte diasFNC,  bool isAprobada)
        {
            try
            {
                this.lblFilter.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_FechaFin") + " " + fechaCorte.ToShortDateString() + "\t   ";
                this.lbl_nameTipo.Text = "Tipo: " + _nameProposito.ToString();

                if (_nameProposito.Equals("Restructuración Abono") || _nameProposito.Equals("Restructuración Plazo"))
                    this.tblAbono.Visible = true;
                if (fechaFNC != null)
                    this.lblFechaIntNC.Text = "(" + diasFNC.ToString() + ") " + fechaFNC.Value.ToShortDateString() + "-->" + fechaCorte.Date.ToShortDateString();

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = numDoc;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = fechaCorte;

                this.QueriesDataSource.Queries[1].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[1].Parameters[1].Value = numDoc;
                this.QueriesDataSource.Queries[1].Parameters[2].Value = fechaCorte;
                #endregion

                base.ConfigureConnection(this.QueriesDataSource);
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
