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
using NewAge.DTO.Resultados;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Ac_ImportacionesTemporales : ReportBase
    {
         public Report_Ac_ImportacionesTemporales()
        {

        }

         public Report_Ac_ImportacionesTemporales(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35041_ImportTemp");
            this.xrLblPeriodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35041_Periodo");
            this.lblTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35041_FechaV");
            this.lblTipoRef.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35041_TipoRef");
            //this.xrLblTotal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_TotalMvoCuenta");
            //this.xrLblSaldoFinal.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35008_SaldoFinalCuenta");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime Periodo, string Plaqueta, string Serial, string TipoRef, string Rompimiento)
        {
            try
            {
                this.xrLabel3.Text = Periodo.ToString().Substring(0, 10);
                DTO_TxResult result = new DTO_TxResult();
                ModuloActivosFijos _moduloActivosFijos = new ModuloActivosFijos(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ActivosTotales> data = _moduloActivosFijos.ReportesActivos_ImportacionesTemporales(Periodo,Plaqueta,Serial,TipoRef,Rompimiento);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    if (Rompimiento == "MesVencimiento-TipoRef")
                    {
                        this.GroupHeader1.Level =0;
                        this.GroupHeader2.Level=1;
                        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                        new DevExpress.XtraReports.UI.GroupField("TipoRef.Value", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});                       
                    }
                    if (Rompimiento == "TipoRef-MesVencimiento")
                    {
                        this.GroupHeader1.Level = 1;
                        this.GroupHeader2.Level = 0;
                        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                        new DevExpress.XtraReports.UI.GroupField("FechaVencimiento.Value", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                    }
                    this.CreateReport();
                    result.ExtraField = this.ReportName;
                    result.Result = ResultValue.OK;
                }
                else
                {
                    result.Result = ResultValue.NOK;
                }
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
