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

namespace NewAge.Reports.Fijos.Documentos.Cuentas_X_Pagar
{
    public partial class Report_Cp_CxPvsPagos  : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Cp_CxPvsPagos()
        {

        }

        public Report_Cp_CxPvsPagos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35117_CxPvsPagos");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodoIni, DateTime periodoFin, string cuentaID, string bancoCuentaID, string terceroID, byte orden)
        {
            #region Asigna Info Header
            string filter = string.Empty;
            this.lblPeriodoIni.Text = periodoIni.ToString("MMMM yyyy");
            this.lblPeriodoFin.Text = periodoFin.ToString("MMMM yyyy");
            if (!string.IsNullOrEmpty(cuentaID))
                filter += "Cuenta:  " + cuentaID + "      ";
            if (!string.IsNullOrEmpty(bancoCuentaID))
                filter += "Banco:  " + " " + bancoCuentaID + "      ";
            if (!string.IsNullOrEmpty(terceroID))
                filter += "Tercero:  " + " " + terceroID + "    ";
            this.lblFiltro.Text = filter;
            #endregion

            #region Order By Dinamico
            if (orden == 1) //Comprobante
            {
                string comp = "ComprobanteCxP";
                GroupField groupFieldComp = new GroupField(comp);
                this.Detail1.SortFields.Add(groupFieldComp);
            }
            else //Tercero
            {
                string tercero = "TerceroDesc";
                string factura = "Factura";
                GroupField groupFielTercer = new GroupField(tercero);
                GroupField groupFieldFactur = new GroupField(factura);
                this.Detail1.SortFields.Add(groupFielTercer);
                this.Detail1.SortFields.Add(groupFieldFactur);
            }
            #endregion

            #region Asigna Filtros
            this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDataSource.Queries[0].Parameters[1].Value = periodoIni.Date;
            this.QueriesDataSource.Queries[0].Parameters[2].Value = periodoFin.Date;
            this.QueriesDataSource.Queries[0].Parameters[3].Value = !string.IsNullOrEmpty(cuentaID) ? cuentaID : null;
            this.QueriesDataSource.Queries[0].Parameters[4].Value = !string.IsNullOrEmpty(bancoCuentaID) ? bancoCuentaID : null;
            this.QueriesDataSource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(terceroID) ? terceroID : null;
            this.QueriesDataSource.Queries[0].Parameters[6].Value = orden;
            #endregion

            base.ConfigureConnection(this.QueriesDataSource);
            base.CreateReport();
            return base.ReportName;
        }
    }
}
