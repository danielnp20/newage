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
    public partial class Report_Cp_CajaMenor : ReportBaseLandScape
    {
        private ModuloCuentasXPagar _moduloCxP = null;

        public Report_Cp_CajaMenor()
        {

        }

        public Report_Cp_CajaMenor(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc)
            : base(loggerConn, c, tx, empresa, userId, formatType,numDoc)
        {
            DTO_glDocumentoControl doc = this._moduloGlobal.glDocumentoControl_GetByID(numeroDoc.Value);
            if(doc != null && doc.DocumentoID.Value == AppDocuments.LegalizacionGastos)
                this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "24");      
            else
                this.lblReportName.Text = this._moduloGlobal.GetResource(LanguageTypes.Forms, "35049");   
        }

        protected override void SetInitParameters()
        {
            this.InitializeComponent();
            base.SetInitParameters();
        }
        
        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int? numeroDoc, string prefijoID,int? docNro, bool isPreliminar)
        {
            try
            {
                List<DTO_Legalizacion> data = new List<DTO_Legalizacion>();
                if (isPreliminar)
                {
                    this.Watermark.Font = new System.Drawing.Font("Arial", 120F);
                    this.Watermark.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.Watermark.Text = "PRELIMINAR";
                    this.Watermark.TextTransparency = 119;
                }
                this._moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                DTO_Legalizacion leg = this._moduloCxP.Legalizacion_Get(numeroDoc.Value);

                DTO_cpCajaMenor caja = (DTO_cpCajaMenor)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.cpCajaMenor, leg.Header.CajaMenorID.Value, true, false);
                if (caja != null)
                {
                    leg.Header.Responsable.Value = caja.Responsable.Value;
                    leg.Header.ResponsableDesc.Value = caja.ResponsableDesc.Value; 
                }
                data.Add(leg);

                this.DataSource = data;
                this.CreateReport();

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
