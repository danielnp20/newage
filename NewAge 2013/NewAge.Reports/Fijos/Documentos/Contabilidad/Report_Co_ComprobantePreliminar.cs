using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Negocio;
using System.Collections.Generic;
using NewAge.DTO.Reportes;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Co_ComprobantePreliminar : ReportBaseLandScape
    {
        public Report_Co_ComprobantePreliminar()
        {

        }

        public Report_Co_ComprobantePreliminar(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.xrlblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Año");
            this.xrlblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Mes");
            this.xrLblComprobante.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobante");
            this.xrLblNro.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Nro");
            this.xrLblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Fecha");
            this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Total");
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");
            //this.xrlblTitle.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35042_Comprobantes");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(int documentID, int año, int mes, string comprobanteID, string libro, string comprobanteInicial, string comprobanteFinal)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloContabilidad _moduloContabilidad = new ModuloContabilidad(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ReportComprobanteTotal> data = _moduloContabilidad.ReportesContabilidad_ComprobantesPreliminar(año, mes, comprobanteID, libro, comprobanteInicial, comprobanteFinal);
                
                if (data.Count != 0)
                {                  
                    int index = 1;
                    foreach (DTO_ReportComprobanteTotal f in data)
                    {
                        #region Asigna datos para ordenar
                        if (documentID != AppDocuments.CajaMenorContabiliza && documentID != AppDocuments.LegalGastosContabiliza)
                        {
                            //Asigna datos adicionales
                            foreach (var d in f.Detalles)
                            {
                                d.DatoAdd.Value = index.ToString();
                                d.Index = index;
                                index++;
                            }
                        }
                        else
                        {
                            //Asigna datos adicionales para Caja Menor y Legalizacion Gastos
                            foreach (var d in f.Detalles)
                            {
                                d.Index = string.IsNullOrEmpty(d.DatoAdd.Value) ? 0 : Convert.ToInt32(d.DatoAdd.Value);
                                index++;
                            }
                        } 
                        #endregion
                        f.Detalles = f.Detalles.OrderBy(x => x.Index).ToList();
                    }
                    
                    this.DataSource = data;
                    this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }

                else
                    result.Result = ResultValue.NOK;

                return result;
            }
            catch (Exception)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                return result;
            }
        }

    }
}
