using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.Negocio;
using NewAge.DTO.Reportes;
using System.Collections.Generic;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Gl_DocumentosPendientes : ReportBase
    {
        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Report_Gl_DocumentosPendientes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor Con la cadena de conexion
        /// </summary>
        /// <param name="c"></param>
        /// <param name="tx"></param>
        /// <param name="empresa"></param>
        /// <param name="userId"></param>
        /// <param name="formatType"></param>
        public Report_Gl_DocumentosPendientes(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType)
            : base(loggerConn, c, tx, empresa, userId, formatType)
        {         
            this.lblAño.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Año");
            this.lblMes.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35000_Mes");
        }

        /// <summary>
        /// Inicializa los componentes
        /// </summary>
        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public DTO_TxResult GenerateReport(DateTime periodo,byte tipoReport, string modulo, string documentoID)
        {
            try
            {
                DTO_TxResult result = new DTO_TxResult();
                ModuloGlobal _moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this.Empresa, this._userID, this.loggerConnectionStr);
                List<DTO_GlobalTotal> documentos = this._moduloGlobal.ReportesGlobal_DocumentosPendiente(periodo,tipoReport, modulo,documentoID);
                string excluyeDocs = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_DocumentosExclCierreMes);
                if (tipoReport == 1)
                {
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35031_DocumentosPendientes");
                    #region Excluye Documentos
                    string[] docs = excluyeDocs.Split(',');
                    foreach (string doc in docs)
                    {
                        foreach (DTO_GlobalTotal g in documentos)
                            g.DetallesDocPendientes.RemoveAll(x => x.DocumentoID.Value == Convert.ToInt32(doc));
                    } 
                    #endregion                   
                }
                else
                    this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35031_DocumentosProcesados");
                                
                if (documentos.Count != 0)
                {                   
                    this.DataSource = documentos;
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
