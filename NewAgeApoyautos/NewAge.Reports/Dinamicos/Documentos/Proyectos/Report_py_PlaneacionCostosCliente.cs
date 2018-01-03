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

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_py_PlaneacionCostosCliente : ReportBase
    {
        #region Variables

        private DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();

        #endregion
        public Report_py_PlaneacionCostosCliente()
        {

        }

        public Report_py_PlaneacionCostosCliente(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_PlaneacionCostosCliente(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = string.Empty;
            base.lblNombreEmpresa.Font = new System.Drawing.Font("Arial Narrow", 15F, System.Drawing.FontStyle.Bold);
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_SolicitudTrabajo solicitud)
        {
            List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();

            #region Calcula valores del Multiplicador
            foreach (DTO_pyPreProyectoTarea tarea in solicitud.Detalle)
            {
                tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
            }
            foreach (DTO_pyPreProyectoTarea tarea in solicitud.DetalleTareasAdic)
            {
                tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
            } 
            #endregion

            this.solicitud = solicitud;
            list.Add(solicitud);

            if (solicitud.Header.APUIncluyeAIUInd.Value.Value)
            {
                this.tblRowAdmin.Visible = false;
                this.tblRowImpr.Visible = false;
                this.tblRowUtil.Visible = false;
                this.tblRowIndirecto.Visible = false;
                this.tblSubtotalInd.Visible = false;
            }
            this.tblCellVlrIVA.Text = solicitud.Header.ValorIVA.Value.ToString();
            this.tblCellVlrOtros.Text = solicitud.Header.ValorOtros.Value.ToString();

            if (solicitud.DetalleTareasAdic.Count > 0)
                this.tblTareasAdic.Visible = true;

            this.DataSource = list;
            this.ShowPreview();
            return this.ReportName;
        }

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellDetalleInd_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //Se valida si es Detalle para no resaltar
                if (this.tblCellDetalleInd.Text.Equals("True") || this.tblCellDetalleInd.Text.Equals("true"))
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                }                   
                else
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    this.tblCellUnd.Text = string.Empty;
                    this.tblCellCant.Text = string.Empty;
                    this.tblCellCostoUnit.Text = string.Empty;

                    //Valida si es Titulo
                    if (this.tblCellNivel.Text.Equals("0"))
                    {
                        this.tblCellTarea.Text = string.Empty;
                        this.tblCellCostoTotal.Text = string.Empty;
                    }
                }
                //Valida si oculta el codigo de Tarea Cliente
                if (this.tblCellImprimirTarea.Text.Equals("False") || this.tblCellImprimirTarea.Text.Equals("false"))
                    this.tblCellTarea.Text = string.Empty;

                //Valida si resalta el item como Titulo
                if (this.tblCellTituloNeg.Text.Equals("True") || this.tblCellTituloNeg.Text.Equals("true"))
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    //this.tblCellUnd.Text = string.Empty;
                    //this.tblCellCant.Text = string.Empty;
                    //this.tblCellCostoUnit.Text = string.Empty;                  
                }
                else
                {
                    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                    this.tblCellDesc.Font = new Font("Arial Narrow", 8, FontStyle.Regular);                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellDetalleIndAdic_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                //Se valida si es Detalle para no resaltar
                if (this.tblCellDetalleIndAdic.Text.Equals("True") || this.tblCellDetalleIndAdic.Text.Equals("true"))
                {
                    this.tblCellDescAdic.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                    this.tblRowTareaAdic.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                }
                    
                else
                {
                    this.tblRowTareaAdic.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    this.tblCellDescAdic.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                    //this.tblCellUnd.Text = string.Empty;  
                    //this.tblCellCant.Text = string.Empty;
                    //this.tblCellCostoUnit.Text = string.Empty;  

                    //Valida si es Titulo
                    if (this.tblCellNivelAdic.Text.Equals("0"))
                    {
                        this.tblCellTareaAdic.Text = string.Empty;
                        //this.tblCellCostoTotal.Text = string.Empty;
                    }
                }
                //Valida si oculta el codigo de Tarea Cliente
                if (this.tblCellImprTareaIndAdic.Text.Equals("False") || this.tblCellImprTareaIndAdic.Text.Equals("false"))
                    this.tblCellTareaAdic.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellVlrSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            #region Declara variables
            decimal totalTareasCliente = 0;
            decimal totalTareasAdicCliente = 0;
            decimal vlrAdmin = 0;
            decimal vlrImpr = 0;
            decimal vlrUtil = 0;
            decimal vlrIVA = 0; 
            #endregion

            #region Calcula Totales
            if (this.solicitud.Detalle.Count > 0)
                totalTareasCliente = this.solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value);
            if (this.solicitud.DetalleTareasAdic.Count > 0)
                totalTareasAdicCliente = this.solicitud.DetalleTareasAdic.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value);

            if (!this.solicitud.Header.APUIncluyeAIUInd.Value.Value)
            {
                vlrAdmin = totalTareasCliente * (this.solicitud.Header.PorEmpresaADM.Value.Value / 100);
                vlrImpr = totalTareasCliente * (this.solicitud.Header.PorEmpresaIMP.Value.Value / 100);
                vlrUtil = totalTareasCliente * (this.solicitud.Header.PorEmpresaUTI.Value.Value / 100);
            }
            vlrIVA = vlrUtil * Convert.ToDecimal(16 * 0.01);
            #endregion

            #region Asigna valores a controles
            this.tblCellVlrSubTotal.Text = totalTareasCliente.ToString("n2");
            this.tblCellVlrOtros.Text = totalTareasAdicCliente.ToString("n2");
            this.tblCellVlrAdmin.Text = vlrAdmin.ToString("n2");
            this.tblCellVlrImpr.Text = vlrImpr.ToString("n2");
            this.tblCellVlrUtil.Text = vlrUtil.ToString("n2");
            this.tblCellVlrSubTotalInd.Text = (vlrAdmin + vlrImpr + vlrUtil).ToString("n2");
            this.tblCellVlrIVA.Text = vlrIVA.ToString("n2");
            this.tblCellTOTAL.Text = (totalTareasCliente + totalTareasAdicCliente + vlrAdmin + vlrImpr + vlrUtil + vlrIVA).ToString("n2"); 
            #endregion
        }

    }
}
