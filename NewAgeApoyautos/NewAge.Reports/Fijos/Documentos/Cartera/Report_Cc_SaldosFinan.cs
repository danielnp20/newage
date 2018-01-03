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
using DevExpress.DataAccess.ConnectionParameters;
using System.Configuration;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_SaldosFinan : ReportBaseLandScape
    {
        #region Variables
        private byte rompimiento = 0;
        #endregion
        public Report_Cc_SaldosFinan()
        {

        }

        public Report_Cc_SaldosFinan(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, AppReports.ccReportesCartera.ToString());
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera, byte? agrupamiento, byte? romp)
        {
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                this.rompimiento = romp.Value;
                filter = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_FechaFin") + " " + fechaFin.ToShortDateString() + "\t   ";
                if (!string.IsNullOrEmpty(cliente))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ClienteID") + " " + cliente + "\t   ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_Libranza") + " " + libranza.ToString() + "\t   ";
                if (!string.IsNullOrEmpty(zonaID))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ZonaID") + " " + zonaID + "\t   ";
                if (!string.IsNullOrEmpty(ciudad))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_Ciudad") + " " + ciudad + "\t   ";
                if (!string.IsNullOrEmpty(concesionario))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ConcesionarioID") + " " + concesionario + "\t   ";
                if (!string.IsNullOrEmpty(asesor))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_AsesorID") + " " + asesor + "\t   ";
                if (!string.IsNullOrEmpty(lineaCredi))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_LineaCreditoID") + " " + lineaCredi + "\t   ";
                if (!string.IsNullOrEmpty(compCartera))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_CompradorID") + " " + compCartera;

                this.lblFilter.Text = filter;
                #endregion

                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(cliente) ? cliente : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(libranza.ToString()) ? libranza : null;
                this.QueriesDataSource.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(zonaID) ? zonaID : null;
                this.QueriesDataSource.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(ciudad) ? ciudad : null;
                this.QueriesDataSource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(concesionario) ? concesionario : null;
                this.QueriesDataSource.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(asesor) ? asesor : null;
                this.QueriesDataSource.Queries[0].Parameters[7].Value = !string.IsNullOrEmpty(lineaCredi) ? lineaCredi : null;
                this.QueriesDataSource.Queries[0].Parameters[8].Value = !string.IsNullOrEmpty(compCartera) ? compCartera : null;
                this.QueriesDataSource.Queries[0].Parameters[9].Value = !string.IsNullOrEmpty(fechaIni.ToString()) ? fechaIni : null;
                this.QueriesDataSource.Queries[0].Parameters[10].Value = fechaFin;
                #endregion              

                base.ConfigureConnection(this.QueriesDataSource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Se realiza antes de pintar el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Report_Cc_SaldosFinan_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //Asigna el Rompimiento
            if (this.rompimiento != 0 && this.rompimiento != null)
            {
                this.GroupHeader1.Visible = true;
                this.GroupFooter2.Visible = true;
                string grupo = string.Empty;
                if (this.rompimiento == 1)
                    grupo = "ConcesionarioID";
                else if (this.rompimiento == 2)
                    grupo = "AsesorID";
                else if (this.rompimiento == 3)
                    grupo = "LineaCreditoID";
                else if (this.rompimiento == 4)
                    grupo = "CompradorCarteraID";

                //Crea el grupo
                GroupField groupField = new GroupField(grupo);
                this.GroupHeader1.GroupFields.Add(groupField);

                //Asigna valores
                this.lblRomp1.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccReportesCartera + "_" + grupo) + ": ";
                this.Romp1.DataBindings.Add("Text", this.DataSource, this.QueriesDataSource.Queries[0].Name + "." + grupo);
            }
        }
    }
}
