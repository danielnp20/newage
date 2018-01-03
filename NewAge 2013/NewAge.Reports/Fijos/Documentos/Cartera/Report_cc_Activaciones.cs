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
    public partial class Report_cc_Activaciones : ReportBase
    {
        #region Variables
        #endregion
        public Report_cc_Activaciones()
        {
            
        }

        public Report_cc_Activaciones(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Activaciones");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime? fechaIni, DateTime fechaFin, string cliente)
        {
            try
            {
                #region Asigna Info Header
               // this.lblPEriodo.Text = fechaFin.ToString(FormatString.Period);
                #endregion

                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = fechaFin;
                #endregion        

                base.ConfigureConnection(this.QueriesDatasource);
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
                     if (!string.IsNullOrEmpty(this.xrLabel1.Text))            
                        this.xrLabel2.Text = "CON GARANTIAS";
                     else
                         this.xrLabel2.Text = "SIN GARANTIAS";
        
        }
    }
}
