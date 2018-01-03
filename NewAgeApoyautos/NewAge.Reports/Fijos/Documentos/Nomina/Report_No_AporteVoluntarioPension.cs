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
    public partial class Report_No_AporteVoluntarioPension : ReportBase
    {
        public Report_No_AporteVoluntarioPension()
        {

        }

        public Report_No_AporteVoluntarioPension(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
      
            this.lbl_Desde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblDesde");
            this.lbl_Hasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Hasta");
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_AportesVoluntariosaPension");
            this.lbl_GranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_GranTotal");
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lblTotalGrupo");
            this.lbl_Fondo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35063_lbl_Fondo:");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime fechaIni, DateTime fechaFin, string empleadoFil, bool orderBy, String terceroid, String nofondosaludid, String nocajaid)
        {
            this.lblReportName.Visible = false;

            return this.ReportName; 
        }
    }
}
