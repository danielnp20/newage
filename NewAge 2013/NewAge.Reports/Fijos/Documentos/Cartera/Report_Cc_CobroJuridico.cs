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
    public partial class Report_Cc_CobroJuridico : ReportBaseLandScape
    {
        #region Variables
        #endregion
        public Report_Cc_CobroJuridico()
        {
            
        }

        public Report_Cc_CobroJuridico(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            base.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, AppReports.ccCobroJuridico + "_LiqCobroJuridico");          
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte claseDeuda, byte tipoReporte, string cliente, string obligacion)
        {
            List<DTO_ReporCobroJuridico> result = new List<DTO_ReporCobroJuridico>();

            try
            {
                #region Asigna Info Header
                string filter = string.Empty;
                string rsxClaseDeuda = claseDeuda == 1? this._moduloGlobal.GetResource(LanguageTypes.Tables,"tbl_CJPrincipal") : this._moduloGlobal.GetResource(LanguageTypes.Tables,"tbl_CJAdicional");
                string rxsTipoReporte = tipoReporte == 1? this._moduloGlobal.GetResource(LanguageTypes.Tables, "tbl_CJJuzgado") : this._moduloGlobal.GetResource(LanguageTypes.Tables, "tbl_CJAdicional");

                if (!string.IsNullOrEmpty(claseDeuda.ToString()))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35106_ClaseDeuda") + " " + rsxClaseDeuda + "\t ";
                if (!string.IsNullOrEmpty(tipoReporte.ToString()))
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35106_TipoReporte") + " " + rxsTipoReporte + "\t   ";
                if (!string.IsNullOrEmpty(cliente.ToString())) // Cliente
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35106_Cliente") + " " + cliente + "\t ";
                if (!string.IsNullOrEmpty(obligacion.ToString())) // Obligacion
                    filter += this._moduloGlobal.GetResource(LanguageTypes.Forms, "35106_Obligacion") + " " + obligacion + "\t ";
                this.lblFilter.Text = filter;
                #endregion

                ModuloCarteraFin _carteraFin = new ModuloCarteraFin(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                result = _carteraFin.Report_CobroJuridicoGet(claseDeuda, cliente, obligacion);

                List<DTO_ReporCobroJuridico> detaFinal = new List<DTO_ReporCobroJuridico>(); //Lista Final, Llena el datasource del reporte
                List<DTO_ReporCobroJuridico> deta = new List<DTO_ReporCobroJuridico>();      // Llena la lista final 
                List<DTO_ReporCobroJuridico> capTemp = new List<DTO_ReporCobroJuridico>();   // Lista de grupos de capital temp
                foreach (DTO_ReporCobroJuridico cob in result)
                {
                    if (cob.TipoMvto.Value == 2)
                    {
                        DTO_ReporCobroJuridico interes = new DTO_ReporCobroJuridico();
                        interes.Descripcion = "TOTAL INTERESES";
                        interes.InteresMora.Value = capTemp.Sum(x => x.InteresMora.Value);
                        deta.Add(interes);

                        DTO_ReporCobroJuridico totalCap = new DTO_ReporCobroJuridico();
                        totalCap.Descripcion = "TOTAL CAPITAL + TOTAL INTERESES ";
                        totalCap.InteresMora.Value = interes.InteresMora.Value + cob.VlrCapital.Value;
                        deta.Add(totalCap);

                        DTO_ReporCobroJuridico abon = new DTO_ReporCobroJuridico();
                        abon.Descripcion = "MENOS ABONO " + cob.FechaMvto.Value.Value.ToShortDateString();
                        abon.InteresMora.Value = cob.InteresMora.Value;
                        deta.Add(abon);

                        DTO_ReporCobroJuridico saldo = new DTO_ReporCobroJuridico();
                        saldo.Descripcion = "NUEVO SALDO " + cob.FechaMvto.Value.Value.ToShortDateString();
                        saldo.InteresMora.Value = totalCap.InteresMora.Value + abon.InteresMora.Value;
                        deta.Add(saldo);

                        capTemp = new List<DTO_ReporCobroJuridico>();
                    }
                    else
                    {
                        cob.Descripcion = "Liquidación Interes";
                        capTemp.Add(cob);
                    }

                    if (cob.TipoMvto.Value != 2)                    
                        deta.Add(cob);
                }
                DTO_ReporCobroJuridico dtoFinal = new DTO_ReporCobroJuridico();
                dtoFinal.Detalle.AddRange(deta);
                detaFinal.Add(dtoFinal);
                this.DataSource = detaFinal;

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
