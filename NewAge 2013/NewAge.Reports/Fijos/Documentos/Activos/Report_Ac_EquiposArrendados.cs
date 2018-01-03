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
    public partial class Report_Ac_EquiposArrendados : ReportBase
    {
         public Report_Ac_EquiposArrendados()
        {

        }

         public Report_Ac_EquiposArrendados(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {
            this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35035_EquiposArrendados");
            this.xrLblPeriodo.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35035_Periodo");
            this.lblTercero.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35035_Tercero");
            this.lblTipoRef.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35035_TipoRef");
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
        public DTO_TxResult GenerateReport(DateTime Periodo, int Estado, string Tercero, string Plaqueta, string Serial, string TipoRef, string Rompimiento)
        {
            try
            {
                this.xrLabel3.Text = Periodo.ToString().Substring(0, 10);
                DTO_TxResult result = new DTO_TxResult();
                ModuloActivosFijos _moduloActivosFijos = new ModuloActivosFijos(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_ActivosTotales> data = _moduloActivosFijos.ReportesActivos_EquiposArrendados(Periodo,Estado,Tercero,Plaqueta,Serial,TipoRef,Rompimiento);

                if (data.Count != 0)
                {
                    this.DataSource = data;
                    if (Rompimiento == "Tercero-TipoRef")
                    {
                        this.GroupHeader1.Level =1;
                        this.GroupHeader2.Level=2;
                        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                        new DevExpress.XtraReports.UI.GroupField("TipoRef.Value", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});                       
                    }
                    if (Rompimiento == "TipoRef-Tercero")
                    {
                        this.GroupHeader1.Level = 2;
                        this.GroupHeader2.Level = 1;
                        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
                        new DevExpress.XtraReports.UI.GroupField("Tercero.Value", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
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
