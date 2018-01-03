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
    public partial class Report_In_InventarioFisicoDiferencia : ReportBase
    {
        public Report_In_InventarioFisicoDiferencia()
        {

        }

        public Report_In_InventarioFisicoDiferencia(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
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
        public string GenerateReport(int documentID, string bodegaID, DateTime periodoID, List<DTO_inFisicoInventario> _listFisicoInventario)
        {
            try
            {
                this.lblReportName.Visible = false;
                DTO_inBodega bod = (DTO_inBodega)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inBodega, bodegaID, true, false);
                if (bod != null)
                    this.bodegaDesc.Text = bod.ID.Value + " - " + bod.Descriptivo.Value;

                List<DTO_inFisicoInventario> final = new List<DTO_inFisicoInventario>();
                DTO_inFisicoInventario fisico = new DTO_inFisicoInventario();
                ModuloInventarios _moduloInventarios = new ModuloInventarios(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                //Valida si consulta el inventario fisico o toma el que viene
                if (_listFisicoInventario.Count == 0)
                {
                    fisico.BodegaID.Value = bodegaID;
                    fisico.Periodo.Value = periodoID;
                    _listFisicoInventario = _moduloInventarios.InventarioFisico_Get(documentID, fisico);
                    fisico = new DTO_inFisicoInventario();
                }

                //Filtra los resultados
                _listFisicoInventario = _listFisicoInventario.FindAll(x => x.CantAjuste.Value != 0).OrderBy(x => x.ReferenciaP1P2Desc.Value).ToList();
                foreach (DTO_inFisicoInventario inv in _listFisicoInventario)
                {
                    inv.CostoUnitario.Value = Math.Abs((inv.CostoLocal.Value.Value / inv.CantKardex.Value.Value));
                    inv.CostoLocal.Value = ((inv.CostoLocal.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value);
                    inv.CostoExtra.Value = ((inv.CostoExtra.Value.Value / inv.CantKardex.Value.Value) * inv.CantAjuste.Value);
                    if (inv.CantAjuste.Value > 0)
                        inv.Observacion.Value = "E n t r a d a s";   
                    else                 
                        inv.Observacion.Value = "S a l i d a s";
                }
                fisico.DetalleReporte.AddRange(_listFisicoInventario);
                final.Add(fisico);

                this.DataSource = final;
                this.ShowPreview();
                return this.ReportName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
