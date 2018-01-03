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
using NewAge.DTO.Reportes;
using NewAge.DTO.UDT;
using System.Linq;

namespace NewAge.Reports.Fijos
{
    public partial class Report_In_InventarioFisico : ReportBase
    {
        public Report_In_InventarioFisico()
        {

        }

        public Report_In_InventarioFisico(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            //this.xrLblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Año");
            //this.xrLblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Saldos");
            //this.xrLblBodega.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35071_Bodega");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int documentID, string bodegaID, DateTime periodoID, List<DTO_inFisicoInventario> _listFisicoInventario, InventarioFisicoReportType tipoReporte)
        {
            this.lblReportName.Visible = false;
            DTO_inBodega bod = (DTO_inBodega)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaID, true, false);
            if (bod != null)
                this.bodegaDesc.Text = bod.ID.Value + "-" + bod.Descriptivo.Value;
            if (tipoReporte == InventarioFisicoReportType.Conteo)
            {
                //Valida las columnas correspondientes al reporte
                this.titExistencias.Text = "Conteo 1";
                this.titFisico.Text = "Conteo 2";
                this.titFisico.Text = "Conteo 2";
                this.rowExistencias.DataBindings.Clear();
                this.rowExistencias.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, "DetalleReporte.CantKardex.Value", "____")});
                this.rowFisico.DataBindings.Clear();
                this.rowFisico.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                new DevExpress.XtraReports.UI.XRBinding("Text", null, "DetalleReporte.CantFisico.Value", "____")});
            }
         
            List<DTO_inFisicoInventario> final = new List<DTO_inFisicoInventario>();
            DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
            ModuloInventarios _moduloInventarios = new ModuloInventarios(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
            if(_listFisicoInventario.Count == 0)
            {              
                fisico.BodegaID.Value = bodegaID;
                fisico.Periodo.Value = periodoID;
                _listFisicoInventario = _moduloInventarios.InventarioFisico_Get(documentID, fisico);
            }
            fisico = new DTO_inFisicoInventario();
            fisico.DetalleReporte.AddRange(_listFisicoInventario.OrderBy(x=>x.UbicacionID).ThenBy(x=>x.ReferenciaP1P2Desc.Value).ToList());
            final.Add(fisico);          

            this.DataSource = final;
            this.ShowPreview();
            return this.ReportName;
        }
    }
}
