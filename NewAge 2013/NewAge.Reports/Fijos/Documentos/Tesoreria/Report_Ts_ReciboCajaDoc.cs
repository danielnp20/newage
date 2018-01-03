using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using NewAge.Negocio;

namespace NewAge.Reports.Fijos.Documentos.Tesoreria
{
    public partial class Report_Ts_ReciboCajaDoc : ReportBase
    {
        #region Variables

        private string _mes;
        #endregion
        public Report_Ts_ReciboCajaDoc()
        {

        }

        public Report_Ts_ReciboCajaDoc(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int numDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType,numDoc)
        {
            //this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "Estado Cuenta");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int documentID, int numeroDoc)
        {
            try
            {
                this.lblNombreEmpresa.Visible = false;
                this.lblReportName.Visible = false;
                List<DTO_ReportReciboCaja> data = new List<DTO_ReportReciboCaja>();
                if (documentID == AppDocuments.ReciboCaja || documentID == AppDocuments.RecaudosManuales ||
                    documentID == AppDocuments.RecaudosMasivos || documentID == AppDocuments.PagosTotales)
                {
                    ModuloGlobal modGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                    ModuloTesoreria modTesoreria = new ModuloTesoreria(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                    DTO_glDocumentoControl ctrlRecibo = modGlobal.glDocumentoControl_GetByID(numeroDoc);
                    DTO_ReciboCaja recibo = modTesoreria.ReciboCaja_GetForLoad(documentID, ctrlRecibo.PrefijoID.Value, ctrlRecibo.DocumentoNro.Value.Value);
                    if (recibo != null)
                    {
                        DTO_ReportReciboCaja result = modTesoreria.DtoReciboCajaReport(numeroDoc, recibo.Comp);
                        result.ComprobanteID = result.ComprobanteID + "-" + result.ComprobanteNro;
                        result.ReciboNro = result.CajaID + "-" + result.ReciboNro;
                        data.Add(result);
                        this.DataSource = data;
                        this.CreateReport();
                    }
                }
                return this.ReportName;

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}