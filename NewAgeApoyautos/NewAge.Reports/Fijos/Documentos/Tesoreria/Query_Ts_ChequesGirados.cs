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

namespace NewAge.Reports.Fijos
{
    public partial class Query_Ts_ChequesGirados : ReportBase
    {
        public Query_Ts_ChequesGirados()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        /// <param name="numDoc">Numero de documento para los pagos</param>
        public Query_Ts_ChequesGirados(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType, numDoc)
        {
            this.lbl_NitDesc.Text = _moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
            this.lblNombreEmpresa.Text = string.Empty;
            this.lbl_NombreEmp.Text = this.Empresa.Descriptivo.Value;
            this.lbl_NIT.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_N.I.T");
            this.lbl_Documento.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "223130_lbl_Documento");
            this.lbl_CiudadFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_CiudadFecha");
            this.lbl_Beneficiario.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Beneficiario");
            this.lbl_Cheque.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Cheque");
            this.lbl_Banco.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Banco");
            this.lbl_TotalDocumento.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_TotalDoc");
            this.lbl_Sello.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_FirmaYselloBeneficiario");
            this.lbl_Aprobado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Aprobado");
            this.lbl_Revizado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Revizado");
            this.lbl_Contabilizado.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Contabilizado");
            this.lbl_NumCE.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_NroCE");
            this.lbl_Identy.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_Identy");
            this.lbl_CtaNum.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "22310_lbl_CtaNum");
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(List<DTO_ChequesGirados> data)
        {
            if (data.Count != 0)
            {
                this.lblReportName.Visible = false;
                this.lbl_VlrGirado.Text = CurrencyFormater.GetCurrencyString("ES1", "COP", data[0].VlrGirado.Value.Value);
                this.DataSource = data;
                this.CreateReport();
            }
            return this.ReportName;
        }
    }
}
