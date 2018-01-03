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
using System.Linq;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Cc_EstCuentaCesionResumen : ReportBase
    {
        protected ModuloContabilidad _moduloContab;
        private DateTime periodo;
        private string cuentaSaldoCartera = string.Empty;
        private string cuentaInteres = string.Empty;
        #region Variables
        #endregion
        public Report_Cc_EstCuentaCesionResumen()
        {
            
        }

        public Report_Cc_EstCuentaCesionResumen(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = "";
            base.lblNombreEmpresa.Text = "";   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza, string zonaID, string ciudad, string concesionario, string asesor, string lineaCredi, string compCartera)
        {
            try
            {
                this._moduloContab = new ModuloContabilidad(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
               
                #region Obtiene valores para consulta
                this.periodo = fechaIni.Value;
                this.cuentaSaldoCartera = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CuentaOrdenCapitalCesion);  
                #endregion

                #region Asigna Info Header
                this.lblFilter.Text = "Periodo: " + " " + (fechaIni.Value).ToString(FormatString.Period) + "\t   "; 
                #endregion

                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(compCartera)? compCartera : null;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = fechaIni;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Antes de imprimir reporte consulta datos por tercero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblTerceroID_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //Trae el saldo del comprador 
                string tercero = this.lblTerceroID.Text;
                List<DTO_coCuentaSaldo> saldos = this._moduloContab.Saldos_GetByTerceroCartera(this.periodo, tercero, this.cuentaSaldoCartera, string.Empty);
                this.lblSaldoCartera.Text = saldos.Count > 0 ? saldos.Sum(x => x.DbOrigenLocML.Value.Value + x.DbSaldoIniLocML.Value.Value + x.CrOrigenLocML.Value.Value + x.CrSaldoIniLocML.Value.Value + x.DbOrigenExtML.Value.Value + x.DbSaldoIniExtML.Value.Value + x.CrOrigenExtML.Value.Value + x.CrSaldoIniExtML.Value.Value).ToString("n0") : "0";
            }
            catch (Exception ex)
            {               
                ;
            }
        }
    }
}
