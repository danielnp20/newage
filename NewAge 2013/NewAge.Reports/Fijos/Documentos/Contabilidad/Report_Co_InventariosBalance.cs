using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;
using NewAge.Librerias.Project;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_InventariosBalance : ReportBase
    {
        #region Variables

        /// <summary>
        /// Listado de inventario Balance
        /// </summary>
        List<DTO_ContabilidadTotal> data = new List<DTO_ContabilidadTotal>();

        #endregion

        public Report_Co_InventariosBalance()
        {

        }

        public Report_Co_InventariosBalance(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Año");
            this.xrLblDesde.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Desde");
            this.xrLblHasta.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_Hasta");
            this.xrLblTipoBalan.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_TipoBalance");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35003_InventarioBalance");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int mesIni, int mesFin, string Libro, string cuentaIni, string cuentaFin, int _año)
        {
            try
            {
                #region Colocarle nombre a los meses
                #region Mes Inicial

                string MsIn = string.Empty;

                switch (mesIni)
                {
                    case 1:
                        MsIn = "Enero";
                        break;
                    case 2:
                        MsIn = "Febrero";
                        break;
                    case 3:
                        MsIn = "Marzo";
                        break;
                    case 4:
                        MsIn = "Abril";
                        break;
                    case 5:
                        MsIn = "Mayo";
                        break;
                    case 6:
                        MsIn = "Junio";
                        break;
                    case 7:
                        MsIn = "Julio";
                        break;
                    case 8:
                        MsIn = "Agosto";
                        break;
                    case 9:
                        MsIn = "Septiembre";
                        break;
                    case 10:
                        MsIn = "Octubre";
                        break;
                    case 11:
                        MsIn = "Noviembre";
                        break;
                    case 12:
                        MsIn = "Diciembre";
                        break;
                    case 13:
                        MsIn = "Diciembre";
                        break;
                }

                #endregion
                #region Mes Final

                string MsFn = string.Empty;

                switch (mesFin)
                {
                    case 1:
                        MsFn = "Enero";
                        break;
                    case 2:
                        MsFn = "Febrero";
                        break;
                    case 3:
                        MsFn = "Marzo";
                        break;
                    case 4:
                        MsFn = "Abril";
                        break;
                    case 5:
                        MsFn = "Mayo";
                        break;
                    case 6:
                        MsFn = "Junio";
                        break;
                    case 7:
                        MsFn = "Julio";
                        break;
                    case 8:
                        MsFn = "Agosto";
                        break;
                    case 9:
                        MsFn = "Septiembre";
                        break;
                    case 10:
                        MsFn = "Octubre";
                        break;
                    case 11:
                        MsFn = "Noviembre";
                        break;
                    case 12:
                        MsFn = "Diciembre";
                        break;
                    case 13:
                        MsFn = "Diciembre";
                        break;
                }
                this.año.Text = _año.ToString();
                this.xrLblMesIni.Text = MsIn.ToString();
                this.xrLblFechaFin.Text = MsFn.ToString();
                #endregion 
                #endregion

                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                this.data = _moduloContabilidad.ReportesContabilidad_InventarioBalance(mesIni, mesFin, Libro, cuentaIni, cuentaFin,_año);

                this.DataSource = data;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void tblTercero_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            e.Cancel = true;
            for (int i = 0; i < this.data.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.data[i].DetallesInventarioBalance[0].TerceroID.Value))
                {
                    //tblTercero.Visible = true;
                    e.Cancel = false;
                }
            }
        }
    }
}
