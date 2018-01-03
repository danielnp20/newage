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
using DevExpress.DataAccess.Sql.Native;
using DevExpress.DataAccess.Sql;
using System.Data;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_ResumenSaldo : ReportBase
    {
        protected ModuloContabilidad _moduloContab;
        private DateTime periodo;
        #region Variables
        private string cesionario = string.Empty;
        private List<decimal> valorRecInteres = new List<decimal>();
        private List<decimal> valorSdoCarteraAnt = new List<decimal>();
        private List<decimal> valorSaldoPrimaComp = new List<decimal>();
        #endregion
        public Report_Cc_ResumenSaldo()
        {
            
        }

        public Report_Cc_ResumenSaldo(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = "";
            base.lblNombreEmpresa.Text = "";   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime? fechaIni,  string compCartera, string cliente, int? libranza,int? agrupamiento)
        {
            try
            {
                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = fechaIni;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(compCartera) ? compCartera : null;
            
                #endregion        
                #region Asigna Info Header
                string filter = string.Empty;
                filter = this._moduloGlobal.GetResource(LanguageTypes.Forms, + AppReports.ReportBase + "_FechaFin") + " " + Convert.ToDateTime(fechaIni).ToShortDateString() + "\t   ";
                if (!string.IsNullOrEmpty(cliente))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ClienteID") + " " + cliente + "\t   ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_Libranza") + " " + libranza.ToString() + "\t   ";
                if (!string.IsNullOrEmpty(compCartera))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "Cesionario:") + " " + compCartera + "\t   ";

                this.lblFilter.Text = filter;
                #endregion

                base.ConfigureConnection(this.QueriesDatasource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


    }
}
