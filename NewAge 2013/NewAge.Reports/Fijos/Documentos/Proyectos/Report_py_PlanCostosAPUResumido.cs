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

namespace NewAge.Reports.Fijos
{
    public partial class Report_py_PlanCostosAPUResumido : ReportBase
    {
        #region Variables

        private DTO_SolicitudTrabajo solicitudTrab = new DTO_SolicitudTrabajo();

        #endregion
        public Report_py_PlanCostosAPUResumido()
        {

        }

        public Report_py_PlanCostosAPUResumido(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = "Lista Resumida de Insumos para Compras";
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
        public string GenerateReport(DTO_SolicitudTrabajo solicitud, bool useValorCliente)
        {
            List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();

            if (solicitud.Movimientos.Count == 0)
            {
                List<DTO_pyPreProyectoDeta> detalleRecursos = new List<DTO_pyPreProyectoDeta>();
                foreach (DTO_pyPreProyectoTarea tarea in solicitud.Detalle)
                    detalleRecursos.AddRange(tarea.Detalle.ToList());

                //detalleRecursos = detalleRecursos.FindAll(x => x.RecursoID.Value == "1001003").ToList();
                #region Calcula valores del cliente

                foreach (DTO_pyPreProyectoDeta det in detalleRecursos)
                {
                    det.CantidadTOT.Value = det.FactorID.Value * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                    if (useValorCliente)
                    {
                        det.CostoLocalTOT.Value = (det.CostoLocalTOT.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100)) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                        det.CostoLocal.Value = (det.CostoLocal.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100)) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                        det.CostoLocalInicial.Value = (det.CostoLocalInicial.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100)) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                    }
                    else
                    {
                        det.CostoLocalTOT.Value = (det.CostoLocalTOT.Value) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                        det.CostoLocal.Value = (det.CostoLocal.Value) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                        det.CostoLocalInicial.Value = (det.CostoLocalInicial.Value) * solicitud.Detalle.Find(x => x.Consecutivo.Value == det.ConsecTarea.Value).Cantidad.Value;
                    }
                }
                #endregion

                List<string> recursosDistinct = detalleRecursos.Select(x => x.RecursoID.Value.ToString()).Distinct().ToList();
                foreach (string rec in recursosDistinct)
                {
                    DTO_pyPreProyectoDeta det = detalleRecursos.Find(x => x.RecursoID.Value == rec);                  

                    DTO_pyProyectoMvto mvto = new DTO_pyProyectoMvto();
                    decimal cantSUM = 0;
                    decimal costoSUM = 0;
                    string tareasInsumo = string.Empty;
                    mvto.RecursoID.Value = rec;
                    mvto.RecursoDesc.Value = det.RecursoDesc.Value + "  ";
                    mvto.UnidadInvID.Value = det.UnidadInvID.Value;                
                    mvto.TipoRecurso.Value = det.TipoRecurso.Value;
                    cantSUM = detalleRecursos.FindAll(x => x.RecursoID.Value == rec).Sum(y => y.CantidadTOT.Value.Value);
                    costoSUM = detalleRecursos.FindAll(x => x.RecursoID.Value == rec).Sum(y => y.CostoLocalTOT.Value.Value);
                    foreach (DTO_pyPreProyectoDeta r in detalleRecursos.FindAll(x => x.RecursoID.Value == rec))
                        tareasInsumo += solicitud.Detalle.Find(x=>x.Consecutivo.Value == r.ConsecTarea.Value.Value).TareaCliente.Value + "-";

                    tareasInsumo = tareasInsumo.Substring(0, tareasInsumo.Length - 1);
                    mvto.RecursoDesc.Value += "(" + tareasInsumo + ")";
                    mvto.CostoLocal.Value = cantSUM != 0? costoSUM/cantSUM : 0;
                    mvto.CostoTotalML.Value = costoSUM;
                    mvto.CantidadSUM.Value = cantSUM;
                    //if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Personal)
                    //{
                    //    decimal total = detalleRecursos.FindAll(x => x.RecursoID.Value == rec).Sum(x => x.CostoLocal.Value.Value);
                    //    decimal cantPers = detalleRecursos.FindAll(x => x.RecursoID.Value == rec && x.Peso_Cantidad.Value != null).Sum(x => x.Peso_Cantidad.Value.Value);
                    //    mvto.CostoLocal.Value = cantPers != 0 && cantPers != null ? total / cantPers : 0;
                    //    mvto.CantidadSUM.Value = Math.Round(mvto.CostoLocal.Value.Value != 0? costoSUM / mvto.CostoLocal.Value.Value: 0,2);
                    //}
                    //else if (mvto.TipoRecurso.Value == (byte)TipoRecurso.Equipo)
                    //    mvto.CantidadSUM.Value = Math.Round(mvto.CostoLocal.Value.Value != 0 ? costoSUM / mvto.CostoLocal.Value.Value : 0,2);

                    solicitud.Movimientos.Add(mvto);
                }
            }
            this.solicitudTrab = solicitud;
            list.Add(solicitud);

            this.DataSource = list;
            this.ShowPreview();
            return this.ReportName;
        }

    }
}
