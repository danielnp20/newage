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
using DevExpress.DataAccess.Sql.Native;
using DevExpress.DataAccess.Sql;
using System.Data;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_ProveedoresTrazabilizad : ReportBaseLandScape
    {
        protected ModuloContabilidad _moduloContab;
        private DateTime periodo;
        #region Variables
        private string cesionario = string.Empty;
        private List<decimal> valorRecInteres = new List<decimal>();
        private List<decimal> valorSdoCarteraAnt = new List<decimal>();
        private List<decimal> valorSaldoPrimaComp = new List<decimal>();
        #endregion
        public Report_ProveedoresTrazabilizad()
        {
            
        }

        public Report_ProveedoresTrazabilizad(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
        public string GenerateReport(DateTime fechaIni, DateTime? fechaFin,  bool resumidoInd, bool pendientesInd, string proveedorID,string proyectoID)
        {
            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                if(resumidoInd)
                {
                    this.lblTitulo.Text = this.lblTitulo.Text + "- Resumido";
                    this.cellTitParcial.Text = "% Rec";
                    this.cellParcial.DataBindings.Clear();
                    this.cellParcial.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                        new DevExpress.XtraReports.UI.XRBinding("Text", null, "Query.PorcRecibido", "{0:n1}%")});
                }
                else
                    this.lblTitulo.Text = this.lblTitulo.Text + "- Detallado";

                filter = "De: " + Convert.ToDateTime(fechaIni).ToShortDateString() + "\tHasta: " + Convert.ToDateTime(fechaFin).ToShortDateString() + "\t   "; 
                if (!string.IsNullOrEmpty(proveedorID))
                    filter += "Proveedor:  " + proveedorID + "\t   ";
                if (!string.IsNullOrEmpty(proyectoID))
                    filter += "Proyecto: " + proyectoID + "\t   ";
                this.lblFilter.Text = filter;
                #endregion

                string balanceDef = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = fechaIni;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = fechaFin;
                this.QueriesDatasource.Queries[0].Parameters[3].Value = balanceDef;
                this.QueriesDatasource.Queries[0].Parameters[4].Value = resumidoInd ? 1 : 0;
                this.QueriesDatasource.Queries[0].Parameters[5].Value = pendientesInd ? 1 : 0;
                this.QueriesDatasource.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(proveedorID) ? proveedorID : null;
                this.QueriesDatasource.Queries[0].Parameters[7].Value = !string.IsNullOrEmpty(proyectoID) ? proyectoID : null;

                #endregion               

                base.ConfigureConnection(this.QueriesDatasource);
                this.ShowPreview();
                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void final_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void cellParcial_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            //if (string.IsNullOrEmpty(this.cellNroRec.Text) )
            //    cell.Text = string.Empty;
        }

        private void xrTableRow1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            if (string.IsNullOrEmpty(this.cellNroRec.Text))
            {
               // row.BackColor = Color.DarkGray;
                //row.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //row.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
               // row.BackColor = Color.Transparent;
            }
        }      
    }
}
