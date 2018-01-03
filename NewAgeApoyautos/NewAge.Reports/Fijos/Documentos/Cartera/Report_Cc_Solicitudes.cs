using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports
{
    public partial class Report_Cc_Solicitudes : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cc_Solicitudes()
        {

        }

        public Report_Cc_Solicitudes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_lblSolicitudes");
            this.xrLblFiltroLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_FiltroLibranza");
            this.xrLblFltroAsesor.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_FiltroAsesor");
            this.xrLblFiltroCliente.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_FiltroCliente");
            this.xrlblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_Desde");
            this.xrlblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_Hasta");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35096_LblTotal");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIncial, DateTime fechaFinal, string cliente, string libranza, string asesor, string estado)
        {
            this.lblReportName.Visible = false;
            this.xrLblCliente.Visible = false;
            this.xrLblAsesor.Visible = false;
            this.xrLblLibranza.Visible = false;
            this.xrLblFiltroCliente.Visible = false;
            this.xrLblFiltroLibranza.Visible = false;
            this.xrLblFltroAsesor.Visible = false;

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_Cc_Solicitudes(fechaIncial, fechaFinal, cliente, libranza, asesor, estado);

            #region Verifica si viene filtrado para pintar los filtros
            if (!string.IsNullOrEmpty(cliente))
            {
                this.xrLblFiltroCliente.Visible = true;
                this.xrLblCliente.Visible = true;
                this.xrLblCliente.Text = cliente;
            }
            if (!string.IsNullOrEmpty(libranza))
            {
                this.xrLblFiltroLibranza.Visible = true;
                this.xrLblLibranza.Visible = true;
                this.xrLblLibranza.Text = libranza;
            }
            if (!string.IsNullOrEmpty(asesor))
            {
                this.xrLblFltroAsesor.Visible = true;
                this.xrLblAsesor.Visible = true;
                this.xrLblAsesor.Text = asesor;
            } 
            #endregion

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }

    }
}
