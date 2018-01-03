using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using NewAge.Librerias.Project;

namespace NewAge.Reports
{
    public partial class Report_Co_LibroMayor : ReportBase
    {
        public Report_Co_LibroMayor()
        {

        }

        public Report_Co_LibroMayor(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35001_Año");
            this.xrLblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35001_Mes");
            this.xrLblTipoBalance.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35001_TipoBalance");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35001_LibroMayor");
            this.xrLblGranTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35001_GranTotal");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int año, int mes, string tipoBalance/*, string cuentaIni, string cuentaFin*/)
        {
            this.lblReportName.Visible = false;
            ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);  
            string longCuenta = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_NivelCuentasLibroMayor);
            List<DTO_ReportLibroMayorTotales> data = _moduloContabilidad.ReportesContabildiad_LibroMayor(año, mes, tipoBalance/*, cuentaIni, cuentaFin*/);

            if (!string.IsNullOrEmpty(longCuenta))
            {
                int lenght = Convert.ToInt32(longCuenta);
                foreach (var item in data)
                    item.Detalles = item.Detalles.Where(x => x.CuentaID.Value.Length == lenght).ToList();
            }
           
            this.DataSource = data;
            this.CreateReport();

            return this.ReportName;
        }
    }
}
