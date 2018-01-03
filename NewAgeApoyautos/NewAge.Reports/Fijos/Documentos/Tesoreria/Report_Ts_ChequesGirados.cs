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
    public partial class Report_Ts_ChequesGirados : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Ts_ChequesGirados()
        {

        }

        public Report_Ts_ChequesGirados(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lbl_NombreReporte.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35070_lbl_ChequesGirados");
            this.lbl_De.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35070_lbl_De");
            this.lbl_A.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35070_lbl_A");
            this.lbl_Total.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35070_lbl_Total");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string bancoID, string terceroID, DateTime fechaIni, DateTime fechaFin, string orden, bool? nombreBen)
        {
            this.lblReportName.Visible = false;
            ModuloTesoreria _moduloTesoreria = new ModuloTesoreria(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_tsChequesGiradosTotales> data = _moduloTesoreria.GetChequesDetaAux(bancoID, terceroID, orden, fechaIni, fechaFin);

            #region Valida el Beneficiario
            if (nombreBen != null && nombreBen.Value)
            {
                foreach (var t in data)
                {
                    foreach (var d in t.Detalles)
                        d.Nombre.Value = d.Beneficiario.Value;
                }
            } 
            #endregion

            if (data.Count > 0)
            {
                data.FirstOrDefault().FechaIni = fechaIni;
                data.FirstOrDefault().FechaFin = fechaFin;
            }

            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
