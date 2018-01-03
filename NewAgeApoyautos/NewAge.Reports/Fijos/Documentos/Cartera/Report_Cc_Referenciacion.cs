using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Cc_Referenciacion : ReportBase
    {
        public Report_Cc_Referenciacion()
        {

        }

        public Report_Cc_Referenciacion(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35100_AnexoReferenciacion");
            this.lblLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35100_Libranza");
            this.lblFechaLibranza.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35100_FechaLibranza");
            this.lblCliente.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35100_Cliente");
            this.lblNombre.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35100_Nombre");
        }

        protected override void SetInitParameters()     
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(string libranza, string cliente, DateTime FechaRef, bool _llamadaCodEfect)
        {
            DTO_TxResult result = new DTO_TxResult();

            ModuloCartera _moduloCartera = new ModuloCartera(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            List<DTO_CarteraTotales> data = _moduloCartera.ReportesCartera_Cc_Referenciacion(libranza, cliente, FechaRef, _llamadaCodEfect);
          
            if (data.Count != 0)
            {
                this.DataSource = data;
                this.CreateReport();
                result.ExtraField = this.ReportName;
                result.Result = ResultValue.OK;
            }
            else
            {
                result.Result = ResultValue.NOK;
            }

            return result;
        }

    }
}
