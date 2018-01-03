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
using System.Globalization;

namespace NewAge.Reports.Fijos
{
    public partial class Report_py_PlaneacionCostosAPU : ReportBase
    {
        #region Variables

        private DTO_SolicitudTrabajo _sol = new DTO_SolicitudTrabajo();

        #endregion
        public Report_py_PlaneacionCostosAPU()
        {

        }

        public Report_py_PlaneacionCostosAPU(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = string.Empty;
            base.lblNombreEmpresa.Text = string.Empty;
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte tipoReport, DTO_SolicitudTrabajo solicitud,bool useMultiplicadorInd)
        {
            List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
            solicitud.Detalle = solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value && x.Detalle.Count > 0);           
            try
            {
                DTO_pyClaseProyecto claseproy = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, solicitud.Header.ClaseServicioID.Value, true, false);
               
                #region Ordena los items
                try
                {
                    if (claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros)
                    {
                        solicitud.Detalle = solicitud.Detalle.OrderBy(x => Convert.ToInt32(x.TareaCliente.Value)).ToList();
                        solicitud.Detalle.ForEach(x => x.Index = Convert.ToInt32(x.TareaCliente.Value));
                    }
                    else
                        solicitud.Detalle = solicitud.Detalle.OrderBy(x => x.Index).ToList();
                }
                catch (Exception) { solicitud.Detalle = solicitud.Detalle.OrderBy(x => x.Index).ToList(); }
                #endregion
                if (tipoReport == 5) //Valores del Cliente
                {
                    #region Calcula valores del Multiplicador
                    foreach (DTO_pyPreProyectoTarea tarea in solicitud.Detalle)
                    {
                        tarea.PorDescuento.Value = tarea.PorDescuento.Value == null ? 0 : tarea.PorDescuento.Value;
                        foreach (var det in tarea.Detalle)
                        {   
                            det.VlrAIUAdmin.Value =  det.VlrAIUAdmin.Value?? 0;
                            det.VlrAIUImpr.Value = det.VlrAIUImpr.Value ?? 0;
                            det.VlrAIUUtil.Value = det.VlrAIUUtil.Value ?? 0;

                            det.CostoLocalTOT.Value = (det.CostoLocalTOT.Value - (det.CostoLocalTOT.Value.Value * (tarea.PorDescuento.Value.Value / 100))) * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                            det.CostoLocal.Value = (det.CostoLocal.Value - (det.CostoLocal.Value.Value * (tarea.PorDescuento.Value.Value / 100))) * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                            det.CostoLocalInicial.Value = (det.CostoLocalInicial.Value - (det.CostoLocalInicial.Value.Value * (tarea.PorDescuento.Value.Value / 100))) * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                            //det.CostoLocalTOT.Value = det.CostoLocalTOT.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100) - (det.CostoLocalTOT.Value.Value * (tarea.PorDescuento.Value.Value / 100));
                            //det.CostoLocal.Value = det.CostoLocal.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100) - (det.CostoLocal.Value.Value * (tarea.PorDescuento.Value.Value / 100));
                            //det.CostoLocalInicial.Value = det.CostoLocalInicial.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100) - (det.CostoLocalInicial.Value.Value * (tarea.PorDescuento.Value.Value / 100));
                            det.VlrAIUAdmin.Value = det.VlrAIUAdmin.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                            det.VlrAIUImpr.Value = det.VlrAIUImpr.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                            det.VlrAIUUtil.Value = det.VlrAIUUtil.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                        }
                        tarea.VlrAIUxAPUAdmin.Value = tarea.VlrAIUxAPUAdmin.Value ?? 0;
                        tarea.VlrAIUxAPUImpr.Value = tarea.VlrAIUxAPUImpr.Value ?? 0;
                        tarea.VlrAIUxAPUUtil.Value = tarea.VlrAIUxAPUUtil.Value ?? 0;
                        tarea.VlrAIUxAPUAdmin.Value = tarea.VlrAIUxAPUAdmin.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                        tarea.VlrAIUxAPUImpr.Value = tarea.VlrAIUxAPUImpr.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                        tarea.VlrAIUxAPUUtil.Value = tarea.VlrAIUxAPUUtil.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);

                        if (solicitud.Header.TipoRedondeo.Value == 2)//Redonde hacia arriba
                        {
                            tarea.CostoTotalUnitML.Value = Math.Ceiling(tarea.Detalle.Sum(x => x.CostoLocalTOT.Value.Value));
                            tarea.CostoTotalUnitML.Value += Math.Ceiling(tarea.Detalle.Sum(x => (x.VlrAIUAdmin.Value.Value + x.VlrAIUImpr.Value.Value + x.VlrAIUUtil.Value.Value)));
                        }
                        else if (solicitud.Header.TipoRedondeo.Value == 3) //Redondea hacia abajo
                        {
                            tarea.CostoTotalUnitML.Value = Math.Floor(tarea.Detalle.Sum(x => x.CostoLocalTOT.Value.Value));
                            tarea.CostoTotalUnitML.Value += Math.Floor(tarea.Detalle.Sum(x => (x.VlrAIUAdmin.Value.Value + x.VlrAIUImpr.Value.Value + x.VlrAIUUtil.Value.Value)));
                        }
                        else //Redondea Normal
                        {
                            tarea.CostoTotalUnitML.Value = Math.Round(tarea.Detalle.Sum(x => x.CostoLocalTOT.Value.Value), 0);
                            tarea.CostoTotalUnitML.Value += Math.Round((tarea.VlrAIUxAPUAdmin.Value.Value + tarea.VlrAIUxAPUImpr.Value.Value + tarea.VlrAIUxAPUUtil.Value.Value),0);
                        }

                        tarea.CostoTotalML.Value = tarea.CostoTotalUnitML.Value * tarea.Cantidad.Value;
                    }
                    #endregion
                }


                this._sol = solicitud;
                list.Add(solicitud);

                this.DataSource = list;
                base.imgLogoEmpresa.Image = null;
                this.ShowPreview();

                return this.ReportName;
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
        private void tblCellDetalleInd_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                ////Se valida si es Detalle para no resaltar
                //if (this.tblCellDetalleInd.Text.Equals("True") || this.tblCellDetalleInd.Text.Equals("true"))
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                //else
                //{
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                //    this.tblCellUnd.Text = string.Empty;  
                //    this.tblCellCant.Text = string.Empty;
                //    this.tblCellCostoUnit.Text = string.Empty;  
                 
                //    //Valida si es Titulo
                //    if (this.tblCellNivel.Text.Equals("0"))
                //    {                        
                //        //this.tblCellTarea.Text = string.Empty;
                //        this.tblCellCostoTotal.Text = string.Empty;
                //    }
                //}     
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

    }
}
