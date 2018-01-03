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
    public partial class Report_cc_DevolucionSolicitud : ReportBase
    {

        #region Variable
        // variable para convertir en letras los valores
        //private string mdaLocal = string.Empty;
        ////Variable de error
        //string Libranzavalidation = string.Empty;
        #endregion

        public Report_cc_DevolucionSolicitud(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblNombreEmpresa.Visible = true;
            //this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, + AppReports.ccDevolucionSolicitud + "_devsolicitud");
            this.lblReportName.Visible = false;
            this.lbl_ReportNameDev.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_devsolicitud"); // Validar cual es
            this.lbl_cliente.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_cliente");
            this.lbl_nombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_nombre");
            this.lbl_fechaDev.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_fechaDev");
            this.lbl_nroDev.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_nroDev");
            this.lbl_seUsuario.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_seUsuario");
            this.tbl_codCausal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_codCausal");
            this.tbl_descMaestra.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_descMaestra");
            this.tbl_descMaestraGrupo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_desCausalGrupo");
            this.tbl_observaciones.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ccDevolucionSolicitud + "_Observaciones");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _credito, int _numDoc, int _numDev)
        {
            try
            {
                // Parametros para trabar sobre el Query
                this.DataSourceDevolucionSolicitud.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourceDevolucionSolicitud.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(_credito.ToString()) ? _credito.ToString() : null;
                //this.DataSourceDevolucionSolicitud.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(_numDoc.ToString()) ? _numDoc.ToString() : null;
                this.DataSourceDevolucionSolicitud.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(_numDev.ToString()) ? _numDev.ToString() : null;

                base.ConfigureConnection(this.DataSourceDevolucionSolicitud);

                //this.ShowPreview();
                this.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
               return string.Empty;
            }
        }
    }
}
