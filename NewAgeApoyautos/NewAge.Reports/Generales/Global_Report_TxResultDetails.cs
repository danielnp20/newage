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
    public partial class Report_TxResultDetails : ReportBase
    {
        /// <summary>
        /// Constructor x Defecto
        /// </summary>
        public Report_TxResultDetails()
        {

        }

        public Report_TxResultDetails(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId)
            : base(loggerConn, c, tx, empresa, userId, ExportFormatType.pdf) { }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(List<DTO_TxResult> resutlDatDetails)
        {
            this.lblReportName.Visible = false;

            List<DTO_TxResultInconsistencias> inconsisList = new List<DTO_TxResultInconsistencias>();
            for (int i = 0; i < resutlDatDetails.Count; i++)
            {
                try
                {
                    if (resutlDatDetails[i].Result == ResultValue.NOK)
                    {
                        int posicion = 0;
                        List<DTO_TxResultDetail> details = new List<DTO_TxResultDetail>();
                        DTO_TxResultInconsistencias inconsis = new DTO_TxResultInconsistencias();
                        details = resutlDatDetails[i].Details;
                        for (int a = 0; a < resutlDatDetails[i].Details.Count; a++)
                        {
                            inconsis.Fields = resutlDatDetails[i].Details[a].DetailsFields;
                            inconsis.Linea = resutlDatDetails[i].Details[a].line;
                            inconsis.Mensaje = resutlDatDetails[i].Details[a].Message;
                        }
                        inconsis.Result = resutlDatDetails[i].Result.ToString();
                        if (resutlDatDetails[i].ResultMessage == null)
                            resutlDatDetails[i].ResultMessage = string.Empty;
                        inconsis.ResultMessage = resutlDatDetails[i].ResultMessage.ToString();
                        posicion = i + 1;
                        inconsis.posicion = "( " + posicion + " )";
                        inconsisList.Add(inconsis);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            #region Else
            //else
            //{
            //    DTO_TxResultInconsistencias inconsis = new DTO_TxResultInconsistencias();
            //    inconsis.Result = resutlDat.Result.ToString();
            //    inconsis.ResultMessage = resutlDat.ResultMessage.ToString();
            //    inconsisList.Add(inconsis);
            //} 
            #endregion

            this.DataSource = inconsisList;

            //this.ExportToXlsx(this.Path);
            this.ExportToPdf(this.Path);
            return this.ReportName;
        }

    }
}
