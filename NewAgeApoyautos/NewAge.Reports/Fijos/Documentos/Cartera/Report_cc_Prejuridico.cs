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
using NewAge.DTO.Resultados;
using System.Diagnostics;
using System.Windows.Forms;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_cc_Prejuridico : ReportBase
    {
         #region Variables

        private string _mes;
        #endregion
        public Report_cc_Prejuridico()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        public Report_cc_Prejuridico(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_prejuridico"); 
            this.lblReportName.Text.ToUpper();
            this.lbl_Nombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_Nombre");
            this.lbl_Cedula.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_TerceroID");
            this.lbl_fechaIni.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_FechaINI");
            this.lbl_fechaFin.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_FechaFIN");
            this.lbl_plazo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_PlazoExtendido");
            this.lbl_sdoCapital.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_SdoCapital");
            this.lbl_sdoInicial.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_SaldoIncial");
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35115_total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string _tercero, int _mesIni, int _mesFin, int _año)
        {
            try
            {
                this.DataSourcePrejuridico.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.DataSourcePrejuridico.Queries[0].Parameters[1].Value = _tercero.ToString();
                this.DataSourcePrejuridico.Queries[0].Parameters[2].Value = !string.IsNullOrEmpty(_mesIni.ToString()) ? _mesIni.ToString() : null;
                this.DataSourcePrejuridico.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(_mesFin.ToString()) ? _mesFin.ToString() : null;
                this.DataSourcePrejuridico.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(_año.ToString()) ? _año.ToString() : null;

                base.ConfigureConnection(this.DataSourcePrejuridico);
                this.CreateReport();
                return base.ReportName;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: "+e);
                throw;
            }
        }
    }
}
