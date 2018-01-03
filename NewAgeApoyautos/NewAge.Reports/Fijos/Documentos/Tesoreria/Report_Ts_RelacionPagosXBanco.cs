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
    public partial class Report_Ts_RelacionPagosXBanco : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Ts_RelacionPagosXBanco()
        {

        }

        public Report_Ts_RelacionPagosXBanco(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReport.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_RelacionDePagos");
            this.lbl_A.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_lbl_A:");
            this.lbl_ChequesGirados.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_ChequesGiradosDe:");
            this.lbl_Nit.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_Beneficiario:");
            this.lbl_Banco.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_Banco:");
            this.lbl_Cheque.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_Cheque:");
            this.lbl_Valor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35084_Valor:");
            
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string bancoID, string nit, string numCheque)
        {
            this.lblReportName.Visible = false;

            #region Asigna Info Header
            this.lblFechaIni.Text = fechaIni.ToShortDateString();
            this.lblFechaFin.Text = fechaFin.ToShortDateString();
            #endregion

            #region Asigna Filtros
            this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDataSource.Queries[0].Parameters[1].Value = fechaIni;
            this.QueriesDataSource.Queries[0].Parameters[2].Value = fechaFin;
            this.QueriesDataSource.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(bancoID) ? bancoID : null;
            this.QueriesDataSource.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(nit) ? nit : null;
            this.QueriesDataSource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(numCheque) ? numCheque : null;
            #endregion

            base.ConfigureConnection(this.QueriesDataSource);
            base.CreateReport();
            return base.ReportName;



        }
    }
}