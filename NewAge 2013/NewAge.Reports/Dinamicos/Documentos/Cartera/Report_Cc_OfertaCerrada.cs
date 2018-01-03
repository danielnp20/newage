using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_OfertaCerrada : ReportBase
    {
        /// <summary>
        /// Construtor por defecto
        /// </summary>
        public Report_Cc_OfertaCerrada()
        {

        }

        /// <summary>
        /// Contructor para la conexccion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Cc_OfertaCerrada(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_ReporteMensualVentaDiaria");
            //this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Año");
            //this.xrLblFiltro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Filtrado");
            //this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Desde");
            //this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35085_Hasta");

        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los componentes
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        } 
       
        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDoc)
        {
            try
            {
                this.lblReportName.Visible = false;

                ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ccCartas> data = _moduloCartera.Report_Cc_Oferta(numeroDoc);

                this.DataSource = data;
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
