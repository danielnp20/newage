using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using System.Windows.Forms;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_EjecucionPresupuestal : ReportBase
    {

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_Co_EjecucionPresupuestal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor Con la cadena de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Co_EjecucionPresupuestal(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Visible = false;
            this.lblNombreEmpresa.Visible = false;
        }

        /// <summary>
        /// Inicializa los componentes
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DateTime periodo, string proyecto, string centroCto, byte rompimiento, string libro, string monedaID)
        {
            try
            {
                this.lblPeriodo.Text = periodo.ToString("MMMM 'de' yyyy").ToUpper();

                if (rompimiento == 1)
                {
                    this.GroupHeader1.Visible = false;
                    this.GroupHeader1.GroupFields.Clear();
                    //this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                    //new DevExpress.XtraReports.UI.GroupField("LineaPresupuestoID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                }
                else if (rompimiento == 2)
                {
                    this.GroupHeader1.GroupFields.Clear();
                    this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                    new DevExpress.XtraReports.UI.GroupField("CentroCostoID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                    this.lblGroup2.DataBindings.Clear();
                    this.lblGroup2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                    new DevExpress.XtraReports.UI.XRBinding("Text", null, "Contabilidad_Ejecucion Presupuestal.CentroCostoID")});
                    this.lblGroup3.DataBindings.Clear();
                    this.lblGroup3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                    new DevExpress.XtraReports.UI.XRBinding("Text", null, "Contabilidad_Ejecucion Presupuestal.CtoCostoDesc")});
                    this.lblGroup1.Text = "Centro Costo:";

                }
                this.Queriesdatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.Queriesdatasource.Queries[0].Parameters[1].Value = libro;
                this.Queriesdatasource.Queries[0].Parameters[2].Value = monedaID;
                this.Queriesdatasource.Queries[0].Parameters[3].Value = periodo.Date;
                this.Queriesdatasource.Queries[0].Parameters[4].Value = rompimiento;
                this.Queriesdatasource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(centroCto)? centroCto : null;//cto
                this.Queriesdatasource.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(proyecto) ? proyecto : null;//cto;
                base.ConfigureConnection(this.Queriesdatasource);
                this.CreateReport();
                return base.ReportName;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR: " + e);
                throw;
            }
        }
    }
}
