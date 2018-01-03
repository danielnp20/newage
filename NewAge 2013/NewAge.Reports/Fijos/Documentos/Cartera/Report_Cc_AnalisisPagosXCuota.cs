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
    public partial class Report_Cc_AnalisisPagosXCuota : ReportBaseLandScape
    {
        #region Variables
        ModuloCartera _modulocart;
        #endregion
        public Report_Cc_AnalisisPagosXCuota()
        {

        }

        public Report_Cc_AnalisisPagosXCuota(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte? tipoReporte, DateTime? fechaIni, DateTime fechaFin, string cliente, int? libranza)
        {
            try
            {
                this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, AppReports.ccRepAnalisisPagos.ToString()) + " por Cuota ";

                #region Asigna Info Header
                string filter = string.Empty;
                filter = this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_FechaFin") + " " + fechaFin.ToShortDateString() + "\t   ";
                if (!string.IsNullOrEmpty(cliente))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_ClienteID") + " " + cliente + "\t   ";
                if (!string.IsNullOrEmpty(libranza.ToString()))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, +AppReports.ReportBase + "_Libranza") + " " + libranza.ToString() + "\t   ";
                this.lblFilter.Text = filter;
                #endregion

                List<DTO_QueryCarteraMvto> res = new List<DTO_QueryCarteraMvto>();
                this._modulocart = new ModuloCartera(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);              
                List<DTO_QueryCarteraMvto> mvtos = this._modulocart.CarteraMvto_QueryByLibranza(libranza,cliente,true,tipoReporte == 2? true : false);

                //Asigna valores de pagos del Cliente
                if (tipoReporte == 2)
                {
                    this.lblReportName.Text = "Pagos Cliente por Cuota";

                    this.TituloPagoClientes.Visible = true;
                    this.tbPagoClientes.Visible = true;
                    this.TituloAnalisis.Visible = false;
                    this.tbAnalisisPagos.Visible = false;

                    this.lblComprador.Visible = false;
                    this.lblNombreCOmprador.Visible = false;
                    this.lblEstado.Visible = false;
                    this.txtComprador.Visible = false;
                    this.txtNombreCompra.Visible = false;
                    this.txtEstado.Visible = false;

                    this.lblSubTotal.LocationFloat = new DevExpress.Utils.PointFloat(45F, 3.666728F);
                    this.lblTotal.LocationFloat = new DevExpress.Utils.PointFloat(45F, 9.333305F);
                    this.tbSubTotal.LocationFloat = new DevExpress.Utils.PointFloat(145.2829F, 2F);
                    this.tbTotal.LocationFloat = new DevExpress.Utils.PointFloat(145F, 10.00001F);

                    mvtos.RemoveAll(x => x.DocumentoID.Value == 0);

                    //Filtra los documentos de pago
                    mvtos = mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.RecaudosManuales || x.DocumentoID.Value == 90167 ||
                                              x.DocumentoID.Value == AppDocuments.RecaudosMasivos || x.DocumentoID.Value == 90166 ||
                                              x.DocumentoID.Value == AppDocuments.RecaudoReoperacion || x.DocumentoID.Value == 90183 ||
                                              x.DocumentoID.Value == AppDocuments.PagosTotales || x.DocumentoID.Value == 90168);

                    //Coloca los valores positivos
                    foreach (DTO_QueryCarteraMvto p in mvtos)
                    {
                        if (p.DocumentoID.Value == AppDocuments.RecaudosManuales || p.DocumentoID.Value == AppDocuments.RecaudosMasivos ||
                            p.DocumentoID.Value == AppDocuments.PagosTotales || p.DocumentoID.Value == AppDocuments.ReciboCaja)
                        {
                            p.VlrCapital.Value = Math.Abs(p.VlrCapital.Value.Value);
                            p.VlrInteres.Value = Math.Abs(p.VlrInteres.Value.Value);
                            p.VlrSeguro.Value = Math.Abs(p.VlrSeguro.Value.Value);
                            p.VlrMora.Value = Math.Abs(p.VlrMora.Value.Value);
                            p.VlrOtrCuota.Value = Math.Abs(p.VlrOtrCuota.Value.Value);
                            p.VlrPrejuridico.Value = Math.Abs(p.VlrPrejuridico.Value.Value);
                            p.VlrGastos.Value = Math.Abs(p.VlrGastos.Value.Value);
                            p.VlrExtra.Value = Math.Abs(p.VlrExtra.Value.Value);
                            p.SdoFavor.Value = Math.Abs(p.SdoFavor.Value.Value);
                            p.TotalDocumento.Value = Math.Abs(p.TotalDocumento.Value.Value); 
                        }
                    }
                }

                DTO_QueryCarteraMvto mvto = new DTO_QueryCarteraMvto();
                mvto.DetalleReport.AddRange(mvtos);
                res.Add(mvto);
                this.DataSource = res;
                base.CreateReport();
                return base.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
