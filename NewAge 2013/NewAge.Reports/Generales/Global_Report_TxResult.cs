using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.DTO.Resultados;
using System.Collections.Generic;

namespace NewAge.Reports.Fijos.General
{
    public partial class Report_TxResult : ReportBase
    {
        /// <summary>
        /// Constructor x Defecto
        /// </summary>
        public Report_TxResult()
        {

        }

        public Report_TxResult(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId)
            : base(loggerConn, c, tx, empresa, userId, ExportFormatType.pdf) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }


        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_TxResult resutlDat)
        {
            this.lblReportName.Visible = false;

            List<DTO_TxResultInconsistencias> inconsisList = new List<DTO_TxResultInconsistencias>();
            if (resutlDat.Details.Count != 0)
            {
                foreach (var item in resutlDat.Details)
                {
                    DTO_TxResultInconsistencias inconsis = new DTO_TxResultInconsistencias();
                    inconsis.Fields = item.DetailsFields;
                    inconsis.Linea = item.line;
                    inconsis.Mensaje = item.Message;
                    inconsis.Result = resutlDat.Result.ToString();

                    if (!string.IsNullOrEmpty(resutlDat.ResultMessage))
                        inconsis.ResultMessage = resutlDat.ResultMessage.ToString();

                    inconsisList.Add(inconsis);
                } 
            }
            else
            {
                DTO_TxResultInconsistencias inconsis = new DTO_TxResultInconsistencias();
                inconsis.Result = resutlDat.Result.ToString();
                inconsis.ResultMessage = resutlDat.ResultMessage.ToString();
                inconsisList.Add(inconsis);
            }

            this.DataSource = inconsisList;

            //this.ExportToXlsx(this.Path);
            this.ExportToPdf(this.Path);
            return this.ReportName;
        }

    }
}
