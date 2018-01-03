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
    public partial class Report_In_MvtoInventario : ReportBase
    {
        #region Variables

        private DTO_SolicitudTrabajo solicitudTrab = new DTO_SolicitudTrabajo();

        #endregion
        public Report_In_MvtoInventario()
        {

        }

        public Report_In_MvtoInventario(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numDoc)
        {           
            base.lblNombreEmpresa.Font = new System.Drawing.Font("Arial Narrow", 15F, System.Drawing.FontStyle.Bold);
            base.lblNombreEmpresa.Visible = false;
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_MvtoInventarios mvto, int document, int numDoc, byte tipoMvto)
        {
            if (document == AppDocuments.TransaccionManual)
                base.lblReportName.Text = "Transacción Manual Inventarios";
            else if (document == AppDocuments.NotaEnvio)
                base.lblReportName.Text = "Nota de Envío Inventarios";
            if (document == AppDocuments.TransaccionAutomatica)
            {
                //Asigna el titulo segun el tipo de transaccion creada
                this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                DTO_glDocumentoControl ctrl = this._moduloGlobal.glDocumentoControl_GetByID(numDoc);
                DTO_glDocumentoControl ctrlPadre = this._moduloGlobal.glDocumentoControl_GetByID(ctrl.DocumentoPadre.Value.HasValue? ctrl.DocumentoPadre.Value.Value : 0);
                if (ctrlPadre != null && ctrlPadre.DocumentoID.Value == AppDocuments.Recibido)
                    base.lblReportName.Text = "Transacción Automática Inventarios (Recibido)";
                if (ctrlPadre != null && ctrlPadre.DocumentoID.Value == AppDocuments.OrdenDespacho)
                    base.lblReportName.Text = "Transacción Automática Inventarios (Despacho)";
                else
                    base.lblReportName.Text = "Transacción Manual Inventarios";
            }
               

            this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDataSource.Queries[0].Parameters[1].Value = numDoc;
            this.QueriesDataSource.Queries[0].Parameters[2].Value = tipoMvto == 3 ? 2 : tipoMvto; // Si es traslado pone por def mvto de Salida

            if (tipoMvto == (byte)TipoMovimientoInv.Entradas)
                this.chkEntrada.Checked = true;
            else if (tipoMvto == (byte)TipoMovimientoInv.Salidas)
                this.chkSalida.Checked = true;
            else if (tipoMvto == (byte)TipoMovimientoInv.Traslados)
                this.chkTraslado.Checked = true;

            base.ConfigureConnection(this.QueriesDataSource);
            this.CreateReport();
            return this.ReportName;
        }

    }
}
